Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports System.IO
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.DataSourcesRaster

Public Class FrmParametersFromLayers

    Dim m_aoi As Aoi
    Dim m_layersList As IList(Of LayerListItem) = New List(Of LayerListItem)
    Dim m_idxLayerValues As Short = 0
    Dim m_idxParamValues As Short = 1

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim aoi As Aoi = bExt.aoi
        If aoi IsNot Nothing Then
            Try
                m_aoi = aoi
                TxtAoiPath.Text = m_aoi.FilePath
                Me.Text = "Parameters from layers (AOI: " & m_aoi.Name & m_aoi.ApplicationVersion & " )"
                'Load layer lists
                'Create a DirectoryInfo of the HRU directory.
                Dim zonesDirectory As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, Nothing)
                Dim dirZones As New DirectoryInfo(zonesDirectory)
                Dim dirZonesArr As DirectoryInfo() = Nothing
                If dirZones.Exists Then
                    dirZonesArr = dirZones.GetDirectories
                    LoadHruLayers(dirZonesArr)
                    LoadRasters()
                    RdoHru.Checked = True
                End If

            Catch ex As Exception
                MessageBox.Show("Unable to load current aoi. Exception: " & ex.Message)
            End Try
        End If

    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnSelectAoi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelectAoi.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers
        Dim bagisPExt As BagisPExtension = BagisPExtension.GetExtension

        Try
            'initialize and open mini browser
            With pGxDialog
                .AllowMultiSelect = False
                .ButtonCaption = "Select"
                .Title = "Select AOI Folder"
                .ObjectFilter = pFilter
                bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
            End With

            If bObjectSelected = False Then Exit Sub

            'get the name of the selected folder
            Dim pGxDataFolder As IGxFile
            pGxDataFolder = pGxObject.Next
            DataPath = pGxDataFolder.Path
            If String.IsNullOrEmpty(DataPath) Then Exit Sub 'user cancelled the action

            'check AOI/BASIN status
            Dim success As BA_ReturnCode = BA_CheckAoiStatus(DataPath, My.ArcMap.Application.hWnd, My.ArcMap.Document)
            If success = BA_ReturnCode.Success Then
                BA_SetDefaultProjection(My.ArcMap.Application)
                Dim aoiName As String = BA_GetBareName(DataPath)
                m_aoi = New Aoi(aoiName, DataPath, Nothing, bagisPExt.version)
                TxtAoiPath.Text = m_aoi.FilePath
                'ResetForm()
                Me.Text = "Parameters from layers (AOI: " & aoiName & m_aoi.ApplicationVersion & " )"
                bagisPExt.aoi = m_aoi

                'Load layer lists
                ' Create a DirectoryInfo of the HRU directory.
                Dim zonesDirectory As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, Nothing)
                Dim dirZones As New DirectoryInfo(zonesDirectory)
                Dim dirZonesArr As DirectoryInfo() = Nothing
                If dirZones.Exists Then
                    dirZonesArr = dirZones.GetDirectories
                    LoadHruLayers(dirZonesArr)
                    LoadRasters()
                    RdoHru.Checked = True
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("BtnSelectAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadHruLayers(ByVal dirZonesArr As DirectoryInfo())
        LstHruLayers.Items.Clear()
        If dirZonesArr IsNot Nothing Then
            Dim item As LayerListItem
            For Each dri In dirZonesArr
                Dim hruFilePath As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruGrid)
                Dim hruXmlFilePath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruXml)
                ' Add hru to the list if the grid exists
                If BA_File_Exists(hruFilePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) And _
                   BA_File_ExistsWindowsIO(hruXmlFilePath) Then
                    'Assume hru is discrete raster since we create it to be so
                    item = New LayerListItem(dri.Name, hruFilePath, LayerType.Raster, True)
                    LstHruLayers.Items.Add(item)
                End If
            Next dri
        End If
    End Sub

    Private Sub LoadRasters()
        Dim AOIVectorList() As String = Nothing
        Dim AOIRasterList() As String = Nothing
        Dim layerPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
        BA_ListLayersinGDB(layerPath, AOIRasterList, AOIVectorList)

        'display raster layers
        Dim RasterCount As Integer = UBound(AOIRasterList)
        If RasterCount > 0 Then
            For i = 1 To RasterCount
                Dim fullLayerPath As String = layerPath & "\" & AOIRasterList(i)
                Dim isDiscrete As Boolean = BA_IsIntegerRasterGDB(fullLayerPath)
                Dim item As LayerListItem = New LayerListItem(AOIRasterList(i), fullLayerPath, LayerType.Raster, isDiscrete)
                m_layersList.Add(item)
            Next
        End If
    End Sub

    Private Sub RdoRaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RdoRaster.Click
        LstAoiRasterLayers.Items.Clear()

        ' load only discrete rasters
        For Each item As LayerListItem In m_layersList
            If item.IsDiscrete = True Then
                LstAoiRasterLayers.Items.Add(item)
            End If
        Next
        GrdValues.Rows.Clear()
    End Sub

    Private Sub RdoHru_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RdoHru.CheckedChanged
        LstAoiRasterLayers.Items.Clear()
        LstAoiRasterLayers.Items.AddRange(LstHruLayers.Items)
        GrdValues.Rows.Clear()
    End Sub

    Private Sub LstAoiRasterLayers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LstAoiRasterLayers.SelectedIndexChanged
        Dim item As LayerListItem = LstAoiRasterLayers.SelectedItem
        Dim selGeoDataset As IGeoDataset = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pRasterStats As IRasterStatistics = Nothing

        Try
            If item IsNot Nothing Then
                Dim parentPath As String = "PleaseReturn"
                Dim fileName As String = BA_GetBareName(item.Value, parentPath)
                selGeoDataset = BA_OpenRasterFromGDB(parentPath, fileName)
                If selGeoDataset IsNot Nothing Then
                    pRasterBandCollection = CType(selGeoDataset, IRasterBandCollection)
                    pRasterBand = pRasterBandCollection.Item(0)
                    Dim validVAT As Boolean = False
                    pRasterBand.HasTable(validVAT)
                    If validVAT = False Then
                        Dim errMsg As String = "The selected raster does not have an attribute table. Please use ArcMap to create an attribute table for the raster."
                        MessageBox.Show(errMsg, "Missing attribute table", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                    CopyUniqueValuesToReclass()
                Else
                    Dim errorMsg As String = "Unable to open layer '" & item.Name & "'"
                    MessageBox.Show(errorMsg, "Invalid layer", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    GrdValues.Rows.Clear()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("LstAoiRasterLayers_SelectedIndexChanged Exception: " + ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterStats)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBandCollection)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(selGeoDataset)
        End Try

    End Sub

    Private Sub CopyUniqueValuesToReclass()
        GrdValues.Rows.Clear()
        Dim pGeoDataset As IGeoDataset = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pTable As ITable = Nothing
        Dim pCursor As ICursor = Nothing
        Dim pData As IDataStatistics = New DataStatistics
        Dim pEnumVar As IEnumerator = Nothing
        Try
            Dim item As LayerListItem = LstAoiRasterLayers.SelectedItem
            Dim parentPath As String = "PleaseReturn"
            Dim fileName As String = BA_GetBareName(item.Value, parentPath)

            If item IsNot Nothing Then
                Dim pWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(parentPath)
                If pWorkspaceType = WorkspaceType.Raster Then
                    pGeoDataset = BA_OpenRasterFromFile(parentPath, fileName)
                ElseIf pWorkspaceType = WorkspaceType.Geodatabase Then
                    pGeoDataset = BA_OpenRasterFromGDB(parentPath, fileName)
                End If

                pRasterBandCollection = CType(pGeoDataset, IRasterBandCollection)
                pRasterBand = pRasterBandCollection.Item(0)
                pTable = pRasterBand.AttributeTable
                pCursor = pTable.Search(Nothing, False)
                pData.Field = BA_FIELD_VALUE
                pData.Cursor = pCursor
                pEnumVar = pData.UniqueValues
                Dim valueCount As Integer = pData.UniqueValueCount
                Dim maxValues As Integer = 300
                If valueCount > maxValues Then
                    MessageBox.Show("Cannot reclassify this raster. Number of unique values exceeds " & CStr(maxValues) & ".")
                    Exit Sub
                End If
                pEnumVar.MoveNext()
                Dim pObj As Object = pEnumVar.Current
                While pObj IsNot Nothing
                    Dim pArray As String() = {pObj, pObj}
                    GrdValues.Rows.Add(pArray)
                    pEnumVar.MoveNext()
                    pObj = pEnumVar.Current
                End While
            End If
        Catch ex As Exception
            MessageBox.Show("CopyUniqueValuesToReclass Exception: " + ex.Message)
        Finally
            ManageCalculateButton()
            pEnumVar = Nothing
            pData = Nothing
            pCursor = Nothing
            pTable = Nothing
            pRasterBand = Nothing
            pRasterBandCollection = Nothing
            pGeoDataset = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try

    End Sub

    Private Sub BtnCalculate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCalculate.Click
        'Add field to grid_zones_v
        Dim hruItem As LayerListItem = LstHruLayers.SelectedItem
        Dim hruGdbPath As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, hruItem.Name)
        Dim v_name As String = BA_GetBareName(BA_EnumDescription(PublicPath.HruZonesVector))
        'Make sure grid_zones_v exists
        If Not BA_File_Exists(hruGdbPath & "\" & v_name, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            Dim retVal As BA_ReturnCode = BA_Create_grid_zones_v(hruGdbPath)
            If retVal <> BA_ReturnCode.Success Then
                MessageBox.Show("The " & v_name & " file could not be created." & _
                                " Parameters from Layers cannot be calculated.")
                Exit Sub
            End If
        End If
        Dim zonesFC As IFeatureClass = BA_OpenFeatureClassFromGDB(hruGdbPath, v_name)
        Dim idxHruId As Integer = zonesFC.FindField(BA_FIELD_HRU_ID)
        If idxHruId < 0 Then
            Dim retVal As BA_ReturnCode = BA_CreateHruIdField(hruGdbPath, v_name)
            If retVal <> BA_ReturnCode.Success Then
                MessageBox.Show("The HRU_ID field could not be found in the selected HRU. Parameters from layers cannot be calculated.")
                Exit Sub
            End If
        End If
        Dim idxField As Integer = zonesFC.FindField(TxtParamName.Text)
        If idxField > -1 Then
            'The field already exists; ask user if they want to overwrite
            Dim strMessage As String = "The output parameter name already exists for this HRU. Do you want to overwrite the current values? "
            Dim res As DialogResult = MessageBox.Show(strMessage, "Name already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If res <> DialogResult.Yes Then
                TxtParamName.Focus()
                Exit Sub
            End If
        Else
            idxField = BA_AddUserFieldToVector(hruGdbPath, v_name, TxtParamName.Text, _
                                                   esriFieldType.esriFieldTypeDouble, 0)
        End If
        If idxField > -1 Then
            Dim tableName As String = "tmpTable"
            Dim rasterItem As LayerListItem = LstAoiRasterLayers.SelectedItem
            Dim snapRasterPath As String = BA_GeodatabasePath(TxtAoiPath.Text, GeodatabaseNames.Aoi, True) & BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
            Dim success As BA_ReturnCode = BA_ZonalStatisticsAsTable(hruGdbPath, v_name, BA_FIELD_HRU_ID, rasterItem.Value, _
                                                hruGdbPath, tableName, snapRasterPath, StatisticsTypeString.MAJORITY)
            If success = BA_ReturnCode.Success Then
                success = AppendParametersToHru(hruGdbPath, v_name, tableName, idxField)
            End If
        End If

        ResetForm()
    End Sub

    Private Sub TxtParamName_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtParamName.Leave
        '@ToDo: Need to validate
        'Names must begin with a letter, not a number or special character such as an asterisk (*) or percent sign (%).
        'Names should not contain spaces.
        ManageCalculateButton()
    End Sub

    Private Sub ManageCalculateButton()
        'Does the parameter have a name "
        If String.IsNullOrEmpty(TxtParamName.Text) Then
            BtnCalculate.Enabled = False
            Exit Sub
        End If
        'Has an HRU dataset been selected ?
        If LstHruLayers.SelectedIndex < 0 Then
            BtnCalculate.Enabled = False
            Exit Sub
        End If
        'Is the layers value table populated ? 
        If GrdValues.Rows.Count < 1 Then
            BtnCalculate.Enabled = False
            Exit Sub
        End If
        ' Do we have a numeric value for each of the layer values
        For Each pRow As DataGridViewRow In GrdValues.Rows
            Dim paramValue As Object = pRow.Cells(m_idxParamValues).Value
            If Not IsNumeric(paramValue) Then
                BtnCalculate.Enabled = False
                Exit Sub
            End If
        Next
        BtnCalculate.Enabled = True
    End Sub

    Private Sub LstHruLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstHruLayers.SelectedIndexChanged
        ManageCalculateButton()
    End Sub

    Private Sub ResetForm()
        LstHruLayers.ClearSelected()
        LstAoiRasterLayers.ClearSelected()
        RdoHru.Checked = True
        TxtParamName.Text = Nothing
        GrdValues.Rows.Clear()
    End Sub

    Private Function AppendParametersToHru(ByVal hruGdbPath As String, ByVal v_name As String, ByVal t_name As String, ByVal idxParam As Integer) As BA_ReturnCode
        Dim fClass As IFeatureClass = Nothing
        Dim pTable As ITable = Nothing
        Dim tCursor As ICursor = Nothing
        Dim tRow As IRow = Nothing
        Dim fCursor As IFeatureCursor = Nothing
        Dim fFeature As IFeature = Nothing
        Dim fQuery As IQueryFilter = New QueryFilter()
        Try
            fClass = BA_OpenFeatureClassFromGDB(hruGdbPath, v_name)
            If fClass IsNot Nothing Then
                pTable = BA_OpenTableFromGDB(hruGdbPath, t_name)
                If pTable IsNot Nothing Then
                    Dim idxTHruId As Integer = pTable.FindField(BA_FIELD_HRU_ID)
                    Dim idxTMajorityId As Integer = -1
                    If idxTHruId > -1 Then
                        idxTMajorityId = pTable.FindField(StatisticsTypeString.MAJORITY.ToString)
                        If idxTMajorityId > -1 Then
                            tCursor = pTable.Search(Nothing, False)
                            If tCursor IsNot Nothing Then
                                tRow = tCursor.NextRow
                                Do While tRow IsNot Nothing
                                    Dim hruId As String = Convert.ToString(tRow.Value(idxTHruId))
                                    Dim paramValue As Double = Convert.ToDouble(tRow.Value(idxTMajorityId))
                                    'Now get the value out of the grid in case user wanted to change it
                                    For Each gRow As DataGridViewRow In GrdValues.Rows
                                        Dim layerValue As Double = Convert.ToDouble(gRow.Cells(m_idxLayerValues).Value)
                                        If layerValue = paramValue Then
                                            paramValue = Convert.ToDouble(gRow.Cells(m_idxParamValues).Value)
                                            Exit For
                                        End If
                                    Next
                                    fQuery.WhereClause = BA_FIELD_HRU_ID & " = " & hruId
                                    fCursor = fClass.Update(fQuery, False)
                                    If fCursor IsNot Nothing Then
                                        'We assume there is only one row with the unique id
                                        fFeature = fCursor.NextFeature
                                        fFeature.Value(idxParam) = paramValue
                                        fFeature.Store()
                                    End If
                                    tRow = tCursor.NextRow
                                Loop
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Debug.Print("AppendParametersToHru Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally

        End Try
    End Function
End Class