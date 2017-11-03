Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster
Imports System.Text
Imports System.Windows.Forms
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework

Public Class FrmAddData

    Dim m_layerTable As Hashtable
    Dim m_selLayerName As String
    Dim m_selDataSource As DataSource
    Dim m_layerType As LayerType
    Dim m_DirtyFlag As Boolean = False
    Dim m_settingsPath As String = Nothing
    Dim m_aoiPath As String = Nothing
    Dim m_jhDict As IDictionary(Of String, String)
    Dim m_jhDescrDict As IDictionary(Of String, String)
    Private Const UNDEFINED As String = "Undefined"

    Public Sub New(ByVal layerTable As Dictionary(Of String, DataSource), ByVal aoiPath As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_layerTable = New Hashtable
        For Each kvp As KeyValuePair(Of String, DataSource) In layerTable
            m_layerTable.Add(kvp.Key, kvp.Value)
        Next kvp

        If String.IsNullOrEmpty(aoiPath) Then
            m_settingsPath = BA_GetBagisPSettingsPath()
        Else
            m_settingsPath = BA_GetLocalSettingsPath(aoiPath)
            m_aoiPath = aoiPath
        End If

        InitJHCoeff()
        LoadMeasurementUnitTypes()
        AddHandlersToJHButtons()    'Has to be after LoadMeasurementTypes so events don't fire when loading

        'Enable admin capabilities if user has entered the admin password
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        If bExt.ProfileAdministrator = True Then EnableAdminActions()
    End Sub

    Public Sub New(ByVal layerTable As Dictionary(Of String, DataSource), ByVal selLayerName As String, ByVal aoiPath As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_layerTable = New Hashtable
        For Each kvp As KeyValuePair(Of String, DataSource) In layerTable
            m_layerTable.Add(kvp.Key, kvp.Value)
        Next kvp
        m_selLayerName = selLayerName
        If String.IsNullOrEmpty(aoiPath) Then
            m_settingsPath = BA_GetBagisPSettingsPath()
        Else
            m_settingsPath = BA_GetLocalSettingsPath(aoiPath)
            m_aoiPath = aoiPath
        End If

        m_selDataSource = m_layerTable(m_selLayerName)
        TxtName.Text = selLayerName
        TxtDescription.Text = m_selDataSource.Description
        TxtSource.Text = m_selDataSource.Source
        m_layerType = m_selDataSource.LayerType
        If m_layerType = LayerType.ImageService Then
            CboDataType.Enabled = False
            CboUnits.Enabled = False
        End If

        InitJHCoeff()
        LoadMeasurementUnitTypes()
        AddHandlersToJHButtons()    'Has to be after LoadMeasurementTypes so events don't fire when loading

        'Enable admin capabilities if user has entered the admin password
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        If bExt.ProfileAdministrator = True Then EnableAdminActions()
    End Sub

    Private Sub InitJHCoeff()
        m_jhDict = New Dictionary(Of String, String)
        m_jhDict.Add(rdoJulTMin.Text, BA_JH_Coef_Jul_Tmin)
        m_jhDict.Add(rdoJulTMax.Text, BA_JH_Coef_Jul_Tmax)
        m_jhDict.Add(rdoAugTMin.Text, BA_JH_Coef_Aug_Tmin)
        m_jhDict.Add(rdoAugTMax.Text, BA_JH_Coef_Aug_Tmax)

        m_jhDescrDict = New Dictionary(Of String, String)
        m_jhDescrDict.Add(BA_JH_Coef_Jul_Tmin, "July Min Temperature for JH_Coef estimation")
        m_jhDescrDict.Add(BA_JH_Coef_Jul_Tmax, "July Max Temperature for JH_Coef estimation")
        m_jhDescrDict.Add(BA_JH_Coef_Aug_Tmin, "August Min Temperature for JH_Coef estimation")
        m_jhDescrDict.Add(BA_JH_Coef_Aug_Tmax, "August Max Temperature for JH_Coef estimation")

    End Sub

    Private Sub AddHandlersToJHButtons()
        'Add listener to JH radio buttons
        AddHandler Me.rdoJulTMin.CheckedChanged, AddressOf OnJhButtonChange
        AddHandler Me.rdoJulTMax.CheckedChanged, AddressOf OnJhButtonChange
        AddHandler Me.rdoAugTMin.CheckedChanged, AddressOf OnJhButtonChange
        AddHandler Me.rdoAugTMax.CheckedChanged, AddressOf OnJhButtonChange
        AddHandler Me.rdoOtherTemp.CheckedChanged, AddressOf OnJhButtonChange
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Protected Friend ReadOnly Property DirtyFlag() As Boolean
        Get
            Return m_DirtyFlag
        End Get
    End Property

    Private Sub BtnSelectSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelectSource.Click
        'Declare ArcObjects outside of Try/Catch so we can dispose of them in Finally
        Dim bObjectSelected As Boolean
        Dim filterCollection As IGxObjectFilterCollection = New GxDialogClass()
        Dim pFilter As IGxObjectFilter = New GxFilterGeoDatasets
        filterCollection.AddFilter(pFilter, True)
        Dim imageFilter As IGxObjectFilter = New GxFilterImageServers
        filterCollection.AddFilter(imageFilter, False)
        Dim pGxDialog As IGxDialog = CType(filterCollection, IGxDialog)

        'Collection of layers to be assigned here
        Dim pGxObjects As IEnumGxObject = Nothing
        Dim pGxDataset As IGxDataset
        Try
            'initialize and open mini browser
            With pGxDialog
                .AllowMultiSelect = False
                .ButtonCaption = "Select"
                .Title = "Select data source"
                'open dialog passing handle to Application from AddIn
                bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObjects)
            End With
            'no file selected; exit
            If bObjectSelected = Nothing Then Exit Sub

            'Enable combo boxes in case they were disabled previously
            CboDataType.Enabled = True
            CboUnits.Enabled = True

            Dim pGxObj As IGxObject = pGxObjects.Next
            If pGxObj.Category.Equals(BA_EnumDescription(GxFilterCategory.ImageService)) Then
                'get the url of the selected image service
                Dim agsObj As IGxAGSObject = CType(pGxObj, IGxAGSObject)
                TxtSource.Text = agsObj.AGSServerObjectName.URL
                m_layerType = LayerType.ImageService
                ' Set the units, if applicable

            Else
                'get first dataset
                pGxDataset = CType(pGxObj, IGxDataset)
                'no dataset has been selected
                If pGxDataset Is Nothing Then Exit Sub
                Dim folderPath As String = pGxDataset.Dataset.Workspace.PathName
                'Folder path may have trailing backslash if it is a grid
                folderPath = BA_StandardizePathString(folderPath)
                Dim fileName As String = pGxDataset.DatasetName.Name
                TxtSource.Text = folderPath & "\" & fileName
                SetLayerType(pGxDataset.Type)
            End If

            'Appends the units (if applicable) and sets the UI
            AppendUnitsToDataSource()
        Catch ex As Exception
            Debug.Print("BtnSelectSource_Click Exception: " & ex.Message)
        Finally

        End Try
    End Sub

    Private Sub ManageApplyButton()
        Dim layerName As String = Trim(TxtName.Text)
        Dim source As String = Trim(TxtSource.Text)
        If String.IsNullOrEmpty(layerName) Or
            String.IsNullOrEmpty(source) Then
            BtnApply.Enabled = False
        Else
            BtnApply.Enabled = True
        End If
    End Sub

    Private Sub TxtName_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtName.DoubleClick
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        If bExt.ProfileAdministrator = False Then
            Dim frmPassword As FrmProfilePassword = New FrmProfilePassword()
            frmPassword.ShowDialog()
            'If user supplied an admin password, admin rights will now be set to true in extension
            If bExt.ProfileAdministrator = True Then EnableAdminActions()
        End If
    End Sub

    Private Sub TxtLayerName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtName.TextChanged
        ManageApplyButton()
    End Sub

    Private Sub TxtSource_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtSource.TextChanged
        ManageApplyButton()
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        '24-APR-2012 As of this date we aren't saving the data field
        'Validate that the user has selected a field
        'If String.IsNullOrEmpty(CboDataField.SelectedItem) Then
        '    Dim sb As StringBuilder = New StringBuilder
        '    sb.Append("Please select a field from the data" & vbCrLf)
        '    sb.Append("source. A data field is required.")
        '    MessageBox.Show(sb.ToString, "Missing field", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    CboDataField.Focus()
        '    Exit Sub
        'End If

        'Don't allow spaces in layer name
        Dim layerName As String = Trim(TxtName.Text)
        layerName = layerName.Replace(" ", "_")
        'Don't allow hyphens in layer name
        layerName = layerName.Replace("-", "_")
        'Get layerType enumeration
        Dim pLayerType As LayerType = m_layerType

        'Determine the measurement unit type and units
        Dim selUnitType As MeasurementUnitType = MeasurementUnitType.Missing
        Dim selUnit As MeasurementUnit = MeasurementUnit.Missing
        Dim selSlopeUnit As SlopeUnit
        If CboDataType.SelectedIndex > 0 Then
            selUnitType = BA_GetMeasurementUnitType(CboDataType.SelectedItem)
            If selUnitType = MeasurementUnitType.Slope And CboUnits.SelectedIndex > -1 Then
                selSlopeUnit = BA_GetSlopeUnit(CboUnits.SelectedItem)
            ElseIf CboUnits.SelectedIndex > -1 Then
                selUnit = BA_GetMeasurementUnit(CboUnits.SelectedItem)
            End If
        End If
        Dim unitSource As DataSource = BA_ValidateMeasurementUnits(m_layerTable, selUnitType, _
                                                                   selUnit, selSlopeUnit)
        'We may have a measurement unit conflict
        If unitSource IsNot Nothing Then
            If unitSource.Name = layerName Then
                'Do nothing; We are updating an existing layer of the same name and unit
            Else
                Dim units As String
                If unitSource.MeasurementUnitType = MeasurementUnitType.Slope Then
                    units = BA_EnumDescription(unitSource.SlopeUnit)
                Else
                    units = BA_EnumDescription(unitSource.MeasurementUnit)
                End If
                Dim sb As StringBuilder = New StringBuilder
                sb.Append("There is an existing data source named " & vbCrLf)
                sb.Append(unitSource.Name & " that uses unit type " & unitSource.MeasurementUnitType.ToString & vbCrLf)
                sb.Append("and units " & units & ". This is not compatible" & vbCrLf)
                sb.Append("with your current selection of " & CboUnits.SelectedItem & "." & vbCrLf)
                sb.Append("All data sources that share a measurement unit type" & vbCrLf)
                sb.Append("must use the same units." & vbCrLf)
                sb.Append("Use the adminstrator interface to update the existing data source.")
                MessageBox.Show(sb.ToString, "Measurement unit error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        'Check to see if > 1 layer in the data manager has the same file name
        'If so, warn user that it may not be able to be clipped to an aoi with another file of the same name
        Dim source As String = Trim(TxtSource.Text) 'This is the full path
        Dim newFileName As String = Nothing
        If m_layerType <> LayerType.ImageService Then
            newFileName = BA_GetBareName(source)
        Else
            newFileName = source
        End If
        Dim sb1 As StringBuilder = New StringBuilder
        For Each key As String In m_layerTable.Keys
            Dim nextDataSource As DataSource = m_layerTable(key)
            'Only need to check custom layers and layers that are NOT the selected layer
            If nextDataSource.AoiLayer = False AndAlso layerName <> m_selLayerName Then
                If m_layerType <> LayerType.ImageService Then
                    Dim fileName2 As String = BA_GetBareName(nextDataSource.Source)
                    If fileName2.ToUpper = newFileName.ToUpper Then
                        'Add prefix to warning message; The stringbuilder hasn't been initialized yet
                        If sb1.Length < 1 Then
                            sb1.Append("The data source you are trying to add" & vbCrLf)
                            sb1.Append("has the same file name as the following" & vbCrLf)
                            sb1.Append("data source(s):" & vbCrLf & vbCrLf)
                        End If
                        sb1.Append("Name: " & nextDataSource.Name & " Path:" & nextDataSource.Source & vbCrLf)
                    End If
                Else
                    If newFileName.Trim.ToUpper = nextDataSource.Source.Trim.ToUpper Then
                        'Add prefix to warning message; The stringbuilder hasn't been initialized yet
                        If sb1.Length < 1 Then
                            sb1.Append("The data source you are trying to add" & vbCrLf)
                            sb1.Append("is trying to use the same image service as the following" & vbCrLf)
                            sb1.Append("data source(s):" & vbCrLf & vbCrLf)
                        End If
                        sb1.Append("Name: " & nextDataSource.Name & " Path:" & nextDataSource.Source & vbCrLf)
                    End If
                End If
            End If
        Next
        'Add suffix to warning message if there were any conflicts and pop message
        If sb1.Length > 0 Then
            sb1.Append(vbCrLf & "You may not clip data sources with the" & vbCrLf)
            sb1.Append("same file name or image service to an AOI. If you plan to use this data" & vbCrLf)
            sb1.Append("source with the data source(s) listed above and it is a file, you can rename " & vbCrLf)
            sb1.Append("the file with a unique name before adding it in " & vbCrLf)
            sb1.Append("the Data Manager." & vbCrLf & vbCrLf)
            sb1.Append("Do you still wish to add this data source ?")
            Dim result As DialogResult = MessageBox.Show(sb1.ToString, "Duplicate file name", MessageBoxButtons.YesNo, _
                                                         MessageBoxIcon.Warning)
            If result <> DialogResult.Yes Then
                Exit Sub
            End If
        End If

        'Verify that we want to overwrite an existing layer if it has the same name
        'as the name on the form and is NOT the selected layer
        Dim overwriteLayer As DataSource = m_layerTable(layerName)
        If overwriteLayer IsNot Nothing AndAlso layerName <> m_selLayerName Then
            Dim sb As StringBuilder = New StringBuilder
            sb.Append("You are about to overwrite an" & vbCrLf)
            sb.Append("existing data layer '" & layerName & "'." & vbCrLf)
            sb.Append("Do you wish to continue?")
            Dim result As DialogResult = MessageBox.Show(sb.ToString, "Existing data layer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If result = Windows.Forms.DialogResult.Yes Then
                m_layerTable.Remove(layerName)
            Else
                TxtName.Focus()
                Exit Sub
            End If
        End If

        'Commenting because name is now read-only and we can't have duplicate names; May need to reinstate if users
        'want to have their own names for jh_coeff layer
        'Verify there are no existing layers with the same jh_coeff role
        'For Each key As String In m_layerTable.Keys
        '    Dim pSource As DataSource = m_layerTable(key)
        '    If pSource.JH_Coeff IsNot Nothing Then
        '        Dim newJHCoeff As String = GetJHCoeff()
        '        If pSource.JH_Coeff.Equals(newJHCoeff) Then
        '            If Not pSource.Name.Equals(TxtName.Text) Then
        '                Dim sb As StringBuilder = New StringBuilder
        '                sb.Append("There is an existing data source named" & vbCrLf)
        '                sb.Append(pSource.Name & " configured as the JH Coefficient layer for" & vbCrLf)
        '                sb.Append(newJHCoeff & "." & vbCrLf)
        '                sb.Append("Only one layer at a time can be designated as" & vbCrLf)
        '                sb.Append(newJHCoeff & ".")
        '                MessageBox.Show(sb.ToString, "Existing JH Coefficient layer", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '                Exit Sub
        '            End If
        '        End If
        '    End If
        'Next

        'Update an existing layer if we have one
        If Not String.IsNullOrEmpty(m_selLayerName) Then
            Dim pLayer As DataSource = m_layerTable(m_selLayerName)
            pLayer.Name = layerName
            pLayer.Source = Trim(TxtSource.Text)
            pLayer.Description = TxtDescription.Text
            pLayer.LayerType = pLayerType
            pLayer.MeasurementUnitType = selUnitType
            If CboDataType.SelectedIndex > 0 Then
                If pLayer.MeasurementUnitType = MeasurementUnitType.Slope Then
                    pLayer.MeasurementUnit = MeasurementUnit.Missing
                    pLayer.SlopeUnit = selSlopeUnit
                Else
                    pLayer.MeasurementUnit = selUnit
                    pLayer.SlopeUnit = SlopeUnit.Missing
                End If
            Else
                pLayer.MeasurementUnit = selUnit
            End If
            'Update jh_coef property
            pLayer.JH_Coeff = GetJHCoeff()

            'Update layer in table
            m_layerTable.Item(m_selLayerName) = pLayer
            m_selDataSource = pLayer
        End If

        If String.IsNullOrEmpty(m_selLayerName) Then
            'Add new layer to list so we can persist it
            Dim id As Integer = BA_GetNextDataSourceId(m_settingsPath)
            Dim newLayer As DataSource = New DataSource(id, layerName, TxtDescription.Text, source, False, _
                                                        pLayerType)
            If CboDataType.SelectedIndex > 0 Then
                newLayer.MeasurementUnitType = selUnitType
                If newLayer.MeasurementUnitType = MeasurementUnitType.Slope Then
                    newLayer.MeasurementUnit = MeasurementUnit.Missing
                    newLayer.SlopeUnit = selSlopeUnit
                Else
                    newLayer.MeasurementUnit = selUnit
                    newLayer.SlopeUnit = SlopeUnit.Missing
                End If
            Else
                newLayer.MeasurementUnitType = MeasurementUnitType.Missing
                newLayer.MeasurementUnit = MeasurementUnit.Missing
            End If

            'Update jh_coef property
            newLayer.JH_Coeff = GetJHCoeff()

            'This is a new source; We need to add it to the table
            m_layerTable.Item(layerName) = newLayer
            m_selDataSource = newLayer
        End If

        'If this is a local data source, we need to clip it to the aoi
        Dim success As BA_ReturnCode = BA_ReturnCode.Success
        If Not String.IsNullOrEmpty(m_aoiPath) Then
            Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 5)
            Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Clipping the data source ", "Clipping...")
            pStepProg.Show()
            progressDialog2.ShowDialog()
            pStepProg.Step()
            '1. Verify local settings (aoipath\param folder) path exists
            Dim settingsPath As String = BA_GetLocalSettingsPath(m_aoiPath)
            '2. Get dataBin path from aoi string
            Dim dataBinPath As String = BA_GetDataBinPath(m_aoiPath)
            '3. SetDatumInExtension
            SetDatumInExtension(m_aoiPath)
            '4. ClipAOI and check return code so we only save it if we were successful
            success = BA_ClipLayerToAoi(m_aoiPath, dataBinPath, m_layerTable(layerName))
            '5. Set path of new datasource to file name
            Dim aoiSource As DataSource = m_layerTable(layerName)
            Dim fileName As String = BA_GetBareName(aoiSource.Source)
            aoiSource.Source = fileName
            m_layerTable.Item(layerName) = aoiSource
            If pStepProg IsNot Nothing Then
                pStepProg.Hide()
                pStepProg = Nothing
                progressDialog2.HideDialog()
                progressDialog2 = Nothing
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End If

        If success = BA_ReturnCode.Success Then
            'Persist changes to settings.xml
            Dim srcList As List(Of DataSource) = New List(Of DataSource)
            For Each key As String In m_layerTable.Keys
                Dim pDS As DataSource = m_layerTable(key)
                srcList.Add(pDS)
            Next
            BA_SaveDataLayers(srcList, m_settingsPath)
            If m_layerType <> LayerType.ImageService Then
                UpdateMeasurementUnits()
            End If
        End If
        m_DirtyFlag = True
        Me.Close()
    End Sub

    '24-APR-2012 As of this date we aren't saving the data field
    'Private Sub PopulateCboDataField(ByVal pGeoDataset As IGeoDataset, ByVal data_type As esriDatasetType, _
    '                                 ByVal selField As String)
    '    Dim pFeatureClass As IFeatureClass = Nothing
    '    Dim pRasterBandColl As IRasterBandCollection = Nothing
    '    Dim pRasterBand As IRasterBand = Nothing
    '    Dim pTable As ITable = Nothing
    '    Dim pFields As IFields = Nothing
    '    Try
    '        CboDataField.Items.Clear()
    '        Select Case data_type
    '            Case 4, 5 'shapefile
    '                pFeatureClass = CType(pGeoDataset, FeatureClass)
    '                pFields = pFeatureClass.Fields
    '                TxtLayerType.Text = LayerType.Vector.ToString
    '            Case 12, 13 'raster
    '                pRasterBandColl = CType(pGeoDataset, IRasterBandCollection)
    '                pRasterBand = pRasterBandColl.Item(0)
    '                pTable = pRasterBand.AttributeTable
    '                pFields = pTable.Fields
    '                TxtLayerType.Text = LayerType.Raster.ToString
    '            Case Else 'unsupported format
    '                Dim sb As StringBuilder = New StringBuilder
    '                sb.Append("The type of the selected data source is not supported by BAGIS-P.")
    '                MessageBox.Show(sb.ToString, "Unsupported type", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '                Exit Sub
    '        End Select

    '        If pFields IsNot Nothing Then
    '            For i As Integer = 0 To pFields.FieldCount - 1
    '                Dim pField As IField = pFields.Field(i)
    '                CboDataField.Items.Add(pField.AliasName)
    '                If pField.AliasName = selField Then
    '                    CboDataField.SelectedItem = pField.AliasName
    '                End If
    '            Next
    '        End If
    '    Catch ex As Exception
    '        Debug.Print("PopulateCboDataField Exception: " & ex.Message)
    '    Finally
    '        pFeatureClass = Nothing
    '        pRasterBandColl = Nothing
    '        pRasterBand = Nothing
    '        pFields = Nothing
    '        GC.WaitForPendingFinalizers()
    '        GC.Collect()
    '    End Try

    'End Sub

    Private Sub SetLayerType(ByVal data_type As esriDatasetType)
        Select Case data_type
            Case 4, 5 'shapefile
                m_layerType = LayerType.Vector
            Case 12, 13 'raster
                m_layerType = LayerType.Raster
            Case Else 'unsupported format
                Dim sb As StringBuilder = New StringBuilder
                sb.Append("The type of the selected data source is not supported by BAGIS-P.")
                MessageBox.Show(sb.ToString, "Unsupported type", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
        End Select
    End Sub

    ' Sets the datum string from the source DEM in the bagis-p extension
    Private Sub SetDatumInExtension(ByVal aoiPath As String)
        Dim pExt As BagisPExtension = BagisPExtension.GetExtension
        Dim workspaceType As WorkspaceType = workspaceType.Geodatabase
        Dim parentPath As String = aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
        Dim pGeoDataSet As IGeoDataset = BA_OpenRasterFromGDB(parentPath, BA_EnumDescription(MapsFileName.filled_dem_gdb))
        If pGeoDataSet IsNot Nothing Then
            'Spatial reference for the dataset in question
            Dim pSpRef As ESRI.ArcGIS.Geometry.ISpatialReference = pGeoDataSet.SpatialReference
            If pSpRef IsNot Nothing Then
                pExt.Datum = BA_DatumString(pSpRef)
                pExt.SpatialReference = pSpRef.Name
            End If
            pSpRef = Nothing
        End If
        pGeoDataSet = Nothing
        GC.WaitForPendingFinalizers()
        GC.Collect()
    End Sub

    Private Sub LoadMeasurementUnitTypes()
        CboDataType.Items.Clear()
        'Add blank to first position
        CboDataType.Items.Add(UNDEFINED)
        Dim enumValues As System.Array = System.[Enum].GetValues(GetType(MeasurementUnitType))
        'Start adding at position 1 to exclude "Missing" value
        For i As Integer = 1 To enumValues.Length - 1
            CboDataType.Items.Add(enumValues(i).ToString)
        Next

        'Set the measurement unit in the UI, if appropriate
        If m_selDataSource IsNot Nothing AndAlso _
            m_selDataSource.MeasurementUnitType <> MeasurementUnitType.Missing Then
            For Each strItem As String In CboDataType.Items
                If strItem = m_selDataSource.MeasurementUnitType.ToString Then
                    CboDataType.SelectedItem = strItem
                End If
            Next
        Else
            CboDataType.SelectedIndex = 0
        End If

    End Sub

    Private Sub LoadMeasurementUnits()
        CboUnits.Items.Clear()
        Dim strUnitType As String = CboDataType.SelectedItem
        Dim measUnitType As MeasurementUnitType = BA_GetMeasurementUnitType(strUnitType)
        Select Case measUnitType
            Case MeasurementUnitType.Depth
                CboUnits.Items.Add(MeasurementUnit.Inches.ToString)
                CboUnits.Items.Add(MeasurementUnit.Millimeters.ToString)
            Case MeasurementUnitType.Elevation
                CboUnits.Items.Add(MeasurementUnit.Feet.ToString)
                CboUnits.Items.Add(MeasurementUnit.Meters.ToString)
            Case MeasurementUnitType.Temperature
                CboUnits.Items.Add(MeasurementUnit.Celsius.ToString)
                CboUnits.Items.Add(MeasurementUnit.Fahrenheit.ToString)
            Case MeasurementUnitType.Slope
                CboUnits.Items.Add(BA_EnumDescription(SlopeUnit.Degree))
                CboUnits.Items.Add(BA_EnumDescription(SlopeUnit.PctSlope))
        End Select
        If CboUnits.Items.Count > 0 Then
            CboUnits.SelectedIndex = 0
        End If
    End Sub

    Private Sub CboDataType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboDataType.SelectedIndexChanged
        If Not CboDataType.SelectedItem.Equals(UNDEFINED) Then
            LoadMeasurementUnits()
            CboUnits.Visible = True
            LblUnits.Visible = True
            If CboDataType.SelectedItem.Equals(BA_EnumDescription(MeasurementUnitType.Temperature)) Then
                PnlJhCoeff.Visible = True
            Else
                DisableJhCoeffButtons()
            End If

            If m_selDataSource IsNot Nothing Then
                For Each strItem As String In CboUnits.Items
                    If m_selDataSource.MeasurementUnitType = MeasurementUnitType.Slope And _
                        strItem = BA_EnumDescription(m_selDataSource.SlopeUnit) Then
                        CboUnits.SelectedItem = strItem
                    ElseIf strItem = m_selDataSource.MeasurementUnit.ToString Then
                        CboUnits.SelectedItem = strItem
                    End If
                Next
                If CboDataType.SelectedItem.Equals(BA_EnumDescription(MeasurementUnitType.Temperature)) Then CheckHCoeffRadioButton()
            End If
        Else
            CboUnits.Visible = False
            LblUnits.Visible = False
            DisableJhCoeffButtons()
        End If
    End Sub

    Private Sub UpdateMeasurementUnits()
        Dim errorSb As StringBuilder = New StringBuilder()
        errorSb.Append("An error occurred while trying to update the measurement units ")
        errorSb.Append("in the layer metadata. The measurement units could not be updated.")
        Dim inputFolder As String = Nothing
        Dim inputFile As String = Nothing
        If String.IsNullOrEmpty(m_aoiPath) Then
            'This is a public data source
            inputFolder = "PleaseReturn"
            inputFile = BA_GetBareName(m_selDataSource.Source, inputFolder)
        Else
            'Otherwise it's a local data source
            inputFolder = BA_GetDataBinPath(m_aoiPath)
            inputFile = m_selDataSource.Source
        End If
        Dim tagsList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                           m_selDataSource.LayerType, _
                                                           BA_XPATH_TAGS)
        'Check to see if we need to upgrade the metadata
        If tagsList IsNot Nothing AndAlso tagsList.Count = 0 Then
            Dim fgdcList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                           m_selDataSource.LayerType, _
                                                           BA_XPATH_METSTDN)
            If fgdcList IsNot Nothing Then
                For Each innerText As String In fgdcList
                    If innerText.IndexOf("FGDC") > -1 Then
                        'We have older FGDC metadata to upgrade
                        Dim success As BA_ReturnCode = BA_UpgradeMetadata(inputFolder, inputFile)
                        If success = BA_ReturnCode.Success Then
                            tagsList = BA_ReadMetaData(inputFolder, inputFile, _
                                                           m_selDataSource.LayerType, _
                                                           BA_XPATH_TAGS)
                        End If
                    End If
                Next
            End If
        End If
        'We have existing tags at "/metadata/dataIdInfo/searchKeys/keyword"
        If tagsList IsNot Nothing AndAlso tagsList.Count > 0 Then
            Dim updateBagisTag As Boolean = False
            For Each pInnerText As String In tagsList
                'We have an existing BAGIS tag
                If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                    'Extract inner tags
                    Dim finalLength As Integer = pInnerText.Length - BA_BAGIS_TAG_PREFIX.Length - BA_BAGIS_TAG_SUFFIX.Length
                    Dim innerText As String = ""
                    If finalLength > 0 Then
                        innerText = pInnerText.Substring(BA_BAGIS_TAG_PREFIX.Length, finalLength)
                    End If
                    Dim pContents As String() = innerText.Split(";")
                    If CboDataType.SelectedIndex > -1 Then
                        'We need to record the units in the tag
                        Dim updateCategory As Boolean = False
                        Dim updateValue As Boolean = False
                        Dim i As Integer = 0
                        For Each pString As String In pContents
                            'The zUnit category
                            If pString.IndexOf(BA_ZUNIT_CATEGORY_TAG) > -1 Then
                                'Overwrite existing value if there is one
                                pContents(i) = BA_ZUNIT_CATEGORY_TAG & m_selDataSource.MeasurementUnitType.ToString & ";"
                                updateCategory = True
                            ElseIf pString.IndexOf(BA_ZUNIT_VALUE_TAG) > -1 Then
                                'Overwrite existing value if there is one
                                If m_selDataSource.MeasurementUnitType = MeasurementUnitType.Slope Then
                                    pContents(i) = BA_ZUNIT_VALUE_TAG & BA_EnumDescription(m_selDataSource.SlopeUnit) & ";"
                                Else
                                    pContents(i) = BA_ZUNIT_VALUE_TAG & m_selDataSource.MeasurementUnit.ToString & ";"
                                End If
                                updateValue = True
                            End If
                            i += 1
                        Next
                        If updateCategory = False Then
                            System.Array.Resize(pContents, pContents.Length + 1)
                            'Put the category in the old last position
                            pContents(pContents.GetUpperBound(0)) = BA_ZUNIT_CATEGORY_TAG & m_selDataSource.MeasurementUnitType.ToString & ";"
                        End If
                        If updateValue = False Then
                            System.Array.Resize(pContents, pContents.Length + 1)
                            If m_selDataSource.MeasurementUnitType = MeasurementUnitType.Slope Then
                                'Put the value in the old last position
                                pContents(pContents.GetUpperBound(0)) = BA_ZUNIT_VALUE_TAG & BA_EnumDescription(m_selDataSource.SlopeUnit) & ";"
                            Else
                                'Put the value in the old last position
                                pContents(pContents.GetUpperBound(0)) = BA_ZUNIT_VALUE_TAG & m_selDataSource.MeasurementUnit.ToString & ";"
                            End If
                        End If
                    Else
                        'We need to remove the units from the tag if they are there
                        'Check first to make sure there is something to remove
                        If pContents.Length > 0 Then
                            Dim contentsList As List(Of String) = New List(Of String)
                            For Each pString As String In pContents
                                'Only add the innerText if it isn't unit-related
                                If pString.IndexOf(BA_ZUNIT_CATEGORY_TAG) < 0 And _
                                    pString.IndexOf(BA_ZUNIT_VALUE_TAG) < 0 Then
                                    contentsList.Add(pString)
                                End If
                            Next
                            pContents = contentsList.ToArray
                        End If
                    End If
                    'Reassemble the updated innerText
                    Dim sb As StringBuilder = New StringBuilder()
                    sb.Append(BA_BAGIS_TAG_PREFIX)
                    For Each pString As String In pContents
                        If Not String.IsNullOrEmpty(pString) Then
                            sb.Append(pString & " ")
                        End If
                    Next
                    'Trim off trailing space
                    Dim newInnerText As String = sb.ToString
                    newInnerText = newInnerText.Remove(Len(newInnerText) - 1, 1)
                    newInnerText = newInnerText & BA_BAGIS_TAG_SUFFIX
                    Dim success As BA_ReturnCode = BA_UpdateMetadata(inputFolder, inputFile, m_selDataSource.LayerType, _
                                                                     BA_XPATH_TAGS, newInnerText, BA_BAGIS_TAG_PREFIX.Length)
                    If success <> BA_ReturnCode.Success Then
                        MessageBox.Show(errorSb.ToString, "Update error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Sub
                    End If
                    updateBagisTag = True
                End If
            Next
            'We had existing "keyword" tags but no BAGIS tag; Need to add
            If updateBagisTag = False Then
                Dim bagisTag As String = CreateBagisTag()
                Dim success As BA_ReturnCode = BA_UpdateMetadata(inputFolder, inputFile, m_selDataSource.LayerType, _
                                                                 BA_XPATH_TAGS, bagisTag, BA_BAGIS_TAG_PREFIX.Length)
                If success <> BA_ReturnCode.Success Then
                    MessageBox.Show(errorSb.ToString, "Update error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If
            End If
        Else
            'We need to add a new tag at "/metadata/dataIdInfo/searchKeys/keyword"
            Dim bagisTag As String = CreateBagisTag()
            Dim success As BA_ReturnCode = BA_UpdateMetadata(inputFolder, inputFile, m_selDataSource.LayerType, _
                                                             BA_XPATH_TAGS, bagisTag, BA_BAGIS_TAG_PREFIX.Length)
            If success <> BA_ReturnCode.Success Then
                MessageBox.Show(errorSb.ToString, "Update error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
        End If

    End Sub

    Private Function CreateBagisTag() As String
        Dim sb As StringBuilder = New StringBuilder
        sb.Append(BA_BAGIS_TAG_PREFIX)
        sb.Append(BA_ZUNIT_CATEGORY_TAG & m_selDataSource.MeasurementUnitType.ToString & "; ")
        If m_selDataSource.MeasurementUnitType = MeasurementUnitType.Slope Then
            sb.Append(BA_ZUNIT_VALUE_TAG & BA_EnumDescription(m_selDataSource.SlopeUnit) & ";")
        Else
            sb.Append(BA_ZUNIT_VALUE_TAG & m_selDataSource.MeasurementUnit.ToString & ";")
        End If
        sb.Append(BA_BAGIS_TAG_SUFFIX)
        Return sb.ToString
    End Function

    Public Sub EnableAdminActions()
        TxtName.ReadOnly = False
        TxtDescription.ReadOnly = False
        If m_layerType <> LayerType.ImageService Then
            CboDataType.Enabled = True
            CboUnits.Enabled = True
        End If
        PnlJhCoeff.Enabled = True
    End Sub

    Private Function GetJHCoeff() As String
        Dim retVal As String = Nothing
        For Each ctrl As Control In PnlJhCoeff.Controls
            If ctrl.GetType() Is GetType(RadioButton) Then
                Dim rButton As RadioButton = CType(ctrl, RadioButton)
                If rButton.Checked = True Then
                    If m_jhDict.ContainsKey(rButton.Text) Then
                        retVal = m_jhDict(rButton.Text)
                        Return retVal
                    End If
                End If
            End If
        Next
        Return retVal
    End Function

    Private Sub CheckHCoeffRadioButton()
        If String.IsNullOrEmpty(m_selDataSource.JH_Coeff) Then
            rdoOtherTemp.Checked = True
            Exit Sub
        End If
        Dim checkVal As String = Nothing
        For Each ctrl As Control In PnlJhCoeff.Controls
            If ctrl.GetType() Is GetType(RadioButton) Then
                Dim rButton As RadioButton = CType(ctrl, RadioButton)
                If m_jhDict.ContainsKey(rButton.Text) Then
                    checkVal = m_jhDict(rButton.Text)
                    If checkVal.Equals(m_selDataSource.JH_Coeff) Then
                        rButton.Checked = True
                        TxtName.ReadOnly = True
                        TxtDescription.ReadOnly = True
                        Exit Sub
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub OnJhButtonChange(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim rb = CType(sender, RadioButton)
        If rb.Checked = True Then
            'If other temperature chosen, set name and descr fields to null
            If rb.Name.Equals("rdoOtherTemp") Then
                TxtName.Text = ""
                TxtName.ReadOnly = False
                TxtDescription.Text = ""
                TxtDescription.ReadOnly = False
                Exit Sub
            End If
            TxtName.Text = GetJHCoeff()
            TxtName.ReadOnly = True
            For Each key As String In m_jhDescrDict.Keys
                If key = TxtName.Text Then
                    TxtDescription.Text = m_jhDescrDict(key)
                    TxtDescription.ReadOnly = True
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub DisableJhCoeffButtons()
        'Uncheck all temperature options and hide panel if temperature is not selected
        For Each ctrl As Control In PnlJhCoeff.Controls
            If ctrl.GetType() Is GetType(RadioButton) Then
                Dim rButton As RadioButton = CType(ctrl, RadioButton)
                rButton.Checked = False
            End If
        Next
        'If m_selDataSource IsNot Nothing Then m_selDataSource.JH_Coeff = Nothing
        PnlJhCoeff.Visible = False
    End Sub

    'This method carries forward the units of the data source being edited, if there is one
    Private Sub AppendUnitsToDataSource()
        If m_layerType = LayerType.ImageService Then
            CboDataType.Enabled = False
            CboUnits.Enabled = False
            Dim tempDataSource As DataSource = New DataSource(1, "tempName", "", TxtSource.Text, False, m_layerType)
            tempDataSource.IsValid = True
            Dim dataDict As IDictionary(Of String, DataSource) = New Dictionary(Of String, DataSource)
            dataDict.Add("tempName", tempDataSource)
            Dim success As BA_ReturnCode = BA_AppendUnitsToDataSources(dataDict, Nothing)
            If success = BA_ReturnCode.Success Then
                If tempDataSource.MeasurementUnitType <> MeasurementUnitType.Missing Then
                    For Each strItem As String In CboDataType.Items
                        If strItem = tempDataSource.MeasurementUnitType.ToString Then
                            CboDataType.SelectedItem = strItem
                        End If
                    Next
                Else
                    CboDataType.SelectedIndex = 0
                End If
            End If
            MessageBox.Show("Units for ImageService layers are read-only! The units for this " + _
                            "data source will be set by the ImageService, if applicable.", "BAGIS-P", _
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf m_selDataSource IsNot Nothing Then
            'Can't update units for image server data sources; Warn user

            m_selDataSource.Source = TxtSource.Text
            m_selDataSource.IsValid = True      'Required by BA_AppendUnitsToDataSources
            Dim dataDict As IDictionary(Of String, DataSource) = New Dictionary(Of String, DataSource)
            dataDict.Add(m_selDataSource.Name, m_selDataSource)
            Dim success As BA_ReturnCode = BA_AppendUnitsToDataSources(dataDict, Nothing)
            If success = BA_ReturnCode.Success Then
                m_selDataSource = dataDict(m_selDataSource.Name)
                'Set the measurement unit in the UI, if appropriate
                If m_selDataSource.MeasurementUnitType <> MeasurementUnitType.Missing Then
                    For Each strItem As String In CboDataType.Items
                        If strItem = m_selDataSource.MeasurementUnitType.ToString Then
                            CboDataType.SelectedItem = strItem
                        End If
                    Next
                Else
                    CboDataType.SelectedIndex = 0
                End If
            End If
        End If
    End Sub

End Class