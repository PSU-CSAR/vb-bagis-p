﻿Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports System.Text
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog

Public Class FrmDataManager

    Dim m_dataTable As Dictionary(Of String, DataSource)
    Dim m_settingsPath As String
    Dim m_aoi As Aoi
    Const idx_IsAoi As Int16 = 0
    Const idx_IsValid As Int16 = 1
    Const idx_Name As Int16 = 2
    Const idx_Descr As Int16 = 3
    Const idx_Source As Int16 = 4
    Dim m_slopeUnit As SlopeUnit
    Dim m_elevUnit As MeasurementUnit
    Dim m_depthUnit As MeasurementUnit
    Dim m_degreeUnit As MeasurementUnit

    Public Sub New(ByVal pMode As String)

        ' This call is required by the designer.
        InitializeComponent()

        Dim pStepProg As IStepProgressor = Nothing
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = Nothing

        ' Add any initialization after the InitializeComponent() call.
        If pMode = BA_BAGISP_MODE_PUBLIC Then
            Try
                Me.Height = DataGridView1.Height + 100
                Dim pnt1 As System.Drawing.Point = New System.Drawing.Point(2, 2)
                PnlMain.Location = pnt1
                m_settingsPath = BA_GetBagisPSettingsPath()
                ' Create/configure the ProgressDialog. This automatically displays the dialog
                pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 5)
                progressDialog2 = BA_GetProgressDialog(pStepProg, "Loading and verifying data sources...", "Data manager loading")
                pStepProg.Step()
                m_dataTable = BA_LoadAllDataSources(m_settingsPath)
                pStepProg.Step()
                BA_AppendUnitsToDataSources(m_dataTable, Nothing)
                BtnAdd.Enabled = True
                Me.Text = "Data Manager (Public)"
                'Enable admin capabilities if user has entered the admin password
                Dim bExt As BagisPExtension = BagisPExtension.GetExtension
                If bExt.ProfileAdministrator = True Then EnableAdminButtons()
                progressDialog2.HideDialog()
            Catch ex As Exception
                ' Clean up step progressor
                If progressDialog2 IsNot Nothing Then
                    progressDialog2.HideDialog()
                End If
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
                MessageBox.Show("Error loading data sources. Exception: " & ex.Message)
            End Try
        Else
            PnlAoi.Visible = True
            BtnClip.Visible = False
            BtnAdd.Visible = False
            BtnEdit.Visible = False
            BtnDefaultSettings.Visible = False
            BtnDelete.Location = New System.Drawing.Point(4, 329)
            'Delete button always visible for local data manager
            BtnDelete.Visible = True

            Dim bExt As BagisPExtension = BagisPExtension.GetExtension
            Dim aoi As Aoi = bExt.aoi
            If aoi Is Nothing Then
                Me.Text = "Data Manager (AOI: Not selected)"
            Else
                ' Create/configure the ProgressDialog. This automatically displays the dialog
                pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 5)
                progressDialog2 = BA_GetProgressDialog(pStepProg, "Loading local data sources...", "Data manager loading")
                pStepProg.Step()
                Try
                    m_aoi = aoi
                    TxtAoiPath.Text = m_aoi.FilePath
                    Me.Text = "Data Manager (AOI: " & m_aoi.Name & m_aoi.ApplicationVersion & " )"

                    SetDatumInExtension(m_aoi.FilePath)
                    m_settingsPath = BA_GetLocalSettingsPath(m_aoi.FilePath)
                    Dim aoiHashtable As Hashtable = BA_LoadDataSources(m_settingsPath)
                    BA_AppendUnitsToDataSources(aoiHashtable, m_aoi.FilePath)
                    pStepProg.Step()
                    Dim prismLayersExist As Boolean = False
                    BA_SetMeasurementUnitsForAoi(m_aoi.FilePath, aoiHashtable, m_slopeUnit, m_elevUnit, _
                                                 m_depthUnit, m_degreeUnit, prismLayersExist)
                    pStepProg.Step()
                    If m_slopeUnit = SlopeUnit.Missing Or _
                        m_elevUnit = MeasurementUnit.Missing Or _
                        (m_depthUnit = MeasurementUnit.Missing And prismLayersExist) Then
                        Dim frmDataUnits As FrmDataUnits = New FrmDataUnits(m_aoi, m_slopeUnit, m_elevUnit, m_depthUnit, prismLayersExist)
                        frmDataUnits.ShowDialog()
                        'Update with changes
                        BA_SetMeasurementUnitsForAoi(m_aoi.FilePath, aoiHashtable, m_slopeUnit, m_elevUnit, _
                                                     m_depthUnit, m_degreeUnit, prismLayersExist)
                    End If
                    Dim errorMsg As String = BA_ValidateMeasurementUnitsForAoi(aoiHashtable, m_depthUnit, m_elevUnit, _
                                                                               m_slopeUnit, m_degreeUnit)
                    If errorMsg.Length > 0 Then
                        MessageBox.Show(errorMsg, "Measurement unit error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                    m_dataTable = New Dictionary(Of String, DataSource)
                    For Each key In aoiHashtable.Keys
                        m_dataTable.Add(key, aoiHashtable(key))
                    Next
                    progressDialog2.HideDialog()
                Catch ex As Exception
                    ' Clean up step progressor
                    If progressDialog2 IsNot Nothing Then
                        progressDialog2.HideDialog()
                    End If
                    ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
                    ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
                    MessageBox.Show("Error loading current aoi. Exception: " & ex.Message)
                End Try
            End If
        End If

        ReloadGrid()
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
        Dim aoiPath As String = Nothing
        If m_aoi IsNot Nothing Then
            aoiPath = m_aoi.FilePath
        End If
        Dim frmAddDataLayer As FrmAddData = New FrmAddData(m_dataTable, aoiPath)
        frmAddDataLayer.ShowDialog()
        If frmAddDataLayer.DirtyFlag = True Then
            m_dataTable = BA_LoadAllDataSources(m_settingsPath)
            BA_AppendUnitsToDataSources(m_dataTable, Nothing)
            ReloadGrid()
        End If
    End Sub

    Private Sub ReloadGrid()
        DataGridView1.Rows.Clear()

        'Populate dataGridView from layer hashtable
        If m_dataTable IsNot Nothing Then
            For Each key In m_dataTable.Keys
                Dim pSource As DataSource = m_dataTable(key)
                '---create a row---
                Dim item As New DataGridViewRow
                item.CreateCells(DataGridView1)
                With item
                    .Cells(idx_IsAoi).Value = pSource.AoiLayerDescr
                    .Cells(idx_IsValid).Value = pSource.IsValidDescr
                    .Cells(idx_Name).Value = key
                    .Cells(idx_Descr).Value = pSource.Description
                    'Pre-pend the local source path to the custom layer name if
                    'we are displaying in local mode. We know we are in
                    'local mode if there is an aoi
                    Dim strSource As String = pSource.Source
                    If m_aoi IsNot Nothing AndAlso pSource.AoiLayer = False Then
                        strSource = BA_GetDataBinPath(m_aoi.FilePath) & "\" & strSource
                    End If
                    .Cells(idx_Source).Value = strSource
                End With
                '---add the row---
                DataGridView1.Rows.Add(item)
            Next

            Dim sortCol As DataGridViewColumn = DataGridView1.Columns(idx_Name)
            DataGridView1.Sort(sortCol, System.ComponentModel.ListSortDirection.Ascending)
            For Each pRow As DataGridViewRow In DataGridView1.SelectedRows
                pRow.Selected = False
            Next
        End If
        ManageButtons()
    End Sub

    Private Sub ManageButtons()
        Dim pCollection As DataGridViewSelectedRowCollection = DataGridView1.SelectedRows
        Dim modCount As Integer = 0
        For Each pRow As DataGridViewRow In pCollection
            Dim isAoi As String = pRow.Cells(idx_IsAoi).Value
            If isAoi = BA_User_Data Then modCount += 1
        Next
        If modCount > 0 Then
            If m_aoi Is Nothing Then
                BtnEdit.Enabled = True
            End If
            BtnDelete.Enabled = True
            BtnClip.Enabled = True
        Else
            BtnEdit.Enabled = False
            BtnDelete.Enabled = False
            BtnClip.Enabled = False
        End If
    End Sub

    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDelete.Click
        Dim pCollection As DataGridViewSelectedRowCollection = DataGridView1.SelectedRows
        Dim modCount As Integer = 0
        For Each pRow As DataGridViewRow In pCollection
            Dim isAoi As String = pRow.Cells(idx_IsAoi).Value
            If isAoi = BA_User_Data Then modCount += 1
        Next

        If modCount > 0 Then
            Dim sb As StringBuilder = New StringBuilder
            sb.Append("The selected data source(s) will be deleted." & vbCrLf)
            sb.Append("AOI data sources cannot be deleted or edited." & vbCrLf)
            sb.Append("This action cannot be undone. Do you wish" & vbCrLf)
            sb.Append("to continue ?")
            Dim result As DialogResult = MessageBox.Show(sb.ToString, "Delete data layer(s)", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If result = DialogResult.Yes Then
                For Each pRow As DataGridViewRow In pCollection
                    Dim isAoi As String = pRow.Cells(idx_IsAoi).Value
                    'Only delete layer if it isn't an aoi layer
                    Dim success As Integer = 1
                    If isAoi = BA_User_Data Then
                        Dim layerName As String = CStr(pRow.Cells(idx_Name).Value)
                        If m_aoi IsNot Nothing Then
                            'Delete data if it is associated with the databin of an AOI
                            Dim dSource As DataSource = m_dataTable(layerName)
                            'Pass in dataBin path as parent path
                            'And source (file name) as file name
                            If dSource.LayerType = LayerType.Raster Then
                                success = BA_RemoveRasterFromGDB(BA_GetDataBinPath(m_aoi.FilePath), dSource.Source)
                            Else
                                success = BA_Remove_ShapefileFromGDB(BA_GetDataBinPath(m_aoi.FilePath), dSource.Source)
                            End If
                        End If
                        If success = 1 Then
                            m_dataTable.Remove(layerName)
                        End If
                    End If
                Next
                Dim dataLayerList As List(Of DataSource) = New List(Of DataSource)
                For Each key As String In m_dataTable.Keys
                    dataLayerList.Add(m_dataTable(key))
                Next
                BA_SaveDataLayers(dataLayerList, m_settingsPath)
                ReloadGrid()
            End If
        End If
    End Sub

    'This keeps the first line from being highlighted in the dataGridView
    Private Sub FrmDataLayerManager_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DataGridView1.ClearSelection()
        DataGridView1.Refresh()
    End Sub

    Private Sub DataGridView1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.SelectionChanged
        ManageButtons()
    End Sub

    Private Sub BtnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEdit.Click
        Dim pCollection As DataGridViewSelectedRowCollection = DataGridView1.SelectedRows
        Dim modCount As Integer = 0
        Dim editRow As Integer = 0
        Dim i As Integer = 0
        For Each tRow As DataGridViewRow In pCollection
            Dim isAoi As String = tRow.Cells(idx_IsAoi).Value
            If isAoi = BA_User_Data Then
                modCount += 1
                editRow = i
            End If
            i += 1
        Next
        If modCount > 1 Then
            Dim sb As StringBuilder = New StringBuilder()
            sb.Append("You can only edit one data layer at a time." & vbCrLf)
            sb.Append("AOI data sources cannot be deleted or edited.")
            MessageBox.Show(sb.ToString, "Multiple layers selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        'Get the first item
        Dim pRow As DataGridViewRow = pCollection.Item(editRow)
        Dim pName As String = CStr(pRow.Cells(idx_Name).Value)
        Dim aoiPath As String = Nothing
        If m_aoi IsNot Nothing Then
            aoiPath = m_aoi.FilePath
        End If
        Dim frmAddDataLayer As FrmAddData = New FrmAddData(m_dataTable, pName, aoiPath)
        frmAddDataLayer.ShowDialog()
        If frmAddDataLayer.DirtyFlag = True Then
            m_dataTable = BA_LoadAllDataSources(m_settingsPath)
            BA_AppendUnitsToDataSources(m_dataTable, Nothing)
            ReloadGrid()
        End If

        'Enable admin capabilities if user has entered the admin password from the edit screen
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        If bExt.ProfileAdministrator = True Then EnableAdminButtons()
    End Sub

    Private Sub BtnClip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClip.Click
        Try
            Dim pCollection As DataGridViewSelectedRowCollection = DataGridView1.SelectedRows
            Dim selDataSources As IList(Of String) = New List(Of String)
            For Each pRow As DataGridViewRow In pCollection
                Dim aoiData As String = CStr(pRow.Cells(idx_IsAoi).Value)
                If aoiData = BA_User_Data Then
                    Dim isValid As String = CStr(pRow.Cells(idx_IsValid).Value)
                    If isValid = BA_Valid_Data Then
                        Dim key As String = CStr(pRow.Cells(idx_Name).Value)
                        selDataSources.Add(key)
                    End If
                End If
            Next
            If selDataSources.Count > 0 Then
                Dim frmClipDataSource As FrmClipDataSource = New FrmClipDataSource(m_dataTable, selDataSources)
                frmClipDataSource.ShowDialog()
            Else
                Dim msg As String = "No valid data sources that are not built-in aoi data sources were selected to clip"
                MessageBox.Show(msg, "No valid data sources selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            Debug.Print("BtnClip_Click Exception: " & ex.Message)
        Finally
        End Try

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

            'attempt to set default projection to Albers
            success = BA_SetDefaultProjection(My.ArcMap.Application)
            If success = BA_ReturnCode.Success Then
                Dim aoiName As String = BA_GetBareName(DataPath)
                m_aoi = New Aoi(aoiName, DataPath, Nothing, bagisPExt.version)
                TxtAoiPath.Text = m_aoi.FilePath
                'ResetForm()
                Me.Text = "Data Manager (AOI: " & aoiName & m_aoi.ApplicationVersion & " )"
                bagisPExt.aoi = m_aoi
                SetDatumInExtension(m_aoi.FilePath)
                m_settingsPath = BA_GetLocalSettingsPath(m_aoi.FilePath)
                Dim aoiHashtable As Hashtable = BA_LoadDataSources(m_settingsPath)
                BA_AppendUnitsToDataSources(aoiHashtable, m_aoi.FilePath)
                Dim prismLayersExist As Boolean = False
                BA_SetMeasurementUnitsForAoi(m_aoi.FilePath, aoiHashtable, m_slopeUnit, m_elevUnit, _
                                             m_depthUnit, m_degreeUnit, prismLayersExist)
                If m_slopeUnit = SlopeUnit.Missing Or _
                    m_elevUnit = MeasurementUnit.Missing Or _
                    (m_depthUnit = MeasurementUnit.Missing And prismLayersExist) Then
                    Dim frmDataUnits As FrmDataUnits = New FrmDataUnits(m_aoi, m_slopeUnit, m_elevUnit, m_depthUnit, prismLayersExist)
                    frmDataUnits.ShowDialog()
                    'Update with changes
                    BA_SetMeasurementUnitsForAoi(m_aoi.FilePath, aoiHashtable, m_slopeUnit, m_elevUnit, _
                                                 m_depthUnit, m_degreeUnit, prismLayersExist)
                End If
                Dim errorMsg As String = BA_ValidateMeasurementUnitsForAoi(aoiHashtable, m_depthUnit, m_elevUnit, _
                                                                           m_slopeUnit, m_degreeUnit)
                If errorMsg.Length > 0 Then
                    MessageBox.Show(errorMsg, "Measurement unit error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                m_dataTable = New Dictionary(Of String, DataSource)
                For Each key In aoiHashtable.Keys
                    m_dataTable.Add(key, aoiHashtable(key))
                Next
                ReloadGrid()
            End If
        Catch ex As Exception
            MessageBox.Show("BtnSelectAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    'This is a simplistic updating of the BAGIS tag for the clipped, recalculated slope layer
    'Because we create the slope layer, we assume there is no existing metadata
    Private Sub UpdateSlopeUnits(ByVal inputFolder As String, ByVal inputFile As String, ByVal slopeUnit As SlopeUnit)
        'We need to add a new tag at "/metadata/dataIdInfo/searchKeys/keyword"
        Dim sb As StringBuilder = New StringBuilder
        sb.Append(BA_BAGIS_TAG_PREFIX)
        sb.Append(BA_ZUNIT_CATEGORY_TAG & MeasurementUnitType.Slope.ToString & "; ")

        sb.Append(BA_ZUNIT_VALUE_TAG & BA_EnumDescription(slopeUnit) & ";")
        sb.Append(BA_BAGIS_TAG_SUFFIX)
        BA_UpdateMetadata(inputFolder, inputFile, LayerType.Raster, _
            BA_XPATH_TAGS, sb.ToString, BA_BAGIS_TAG_PREFIX.Length)
    End Sub

    Public Sub EnableAdminButtons()
        BtnAdd.Visible = True
        BtnDelete.Visible = True
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick
        'Only fire if name column is double-clicked
        If e.ColumnIndex = idx_Name Then
            'Check to see if admin capabilities are already enabled before asking again
            Dim bExt As BagisPExtension = BagisPExtension.GetExtension
            If bExt.ProfileAdministrator = False Then
                Dim frmPassword As FrmProfilePassword = New FrmProfilePassword()
                frmPassword.ShowDialog()
                'If user supplied the admin password, admin rights will now be set to true in extension
                If bExt.ProfileAdministrator = True Then EnableAdminButtons()
            End If
        End If
    End Sub

    Private Function QueryDefaultDataSources() As IList(Of BagisImageService)
        Dim settingsUrl As String = "https://raw.githubusercontent.com/PSU-CSAR/vb-bagis-p/master/settings/bagis-p.json"
        Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(settingsUrl)
        Try
            Dim mySettings As ServerSettings = New ServerSettings()
            Using resp As System.Net.WebResponse = req.GetResponse()
                Dim ser As System.Runtime.Serialization.Json.DataContractJsonSerializer = New System.Runtime.Serialization.Json.DataContractJsonSerializer(mySettings.[GetType]())
                mySettings = CType(ser.ReadObject(resp.GetResponseStream), ServerSettings)
            End Using
            Dim count As Integer = mySettings.datasources.Count
            Return mySettings.datasources
        Catch ex As Exception
            Debug.Print("QueryDefaultDataSources Exception: " + ex.Message)
            Return Nothing
        End Try
    End Function

    Private Sub BtnDefaultSettings_Click(sender As System.Object, e As System.EventArgs) Handles BtnDefaultSettings.Click
        If Not String.IsNullOrEmpty(m_settingsPath) Then
            Dim overwriteSettings As Boolean = False
            If System.IO.File.Exists(m_settingsPath) Then
                Dim res As DialogResult = MessageBox.Show("The default data sources will overwrite your current data sources. " + _
                                                          "This action cannot be undone. Do you wish to continue ?", "BAGIS-P", _
                                                          MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If res <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                End If
            End If
            overwriteSettings = True
            ' Create/configure a step progressor
            Dim pStepProg As IStepProgressor = Nothing
            Dim progressDialog2 As IProgressDialog2 = Nothing
            Try
                pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 5)
                progressDialog2 = BA_GetProgressDialog(pStepProg, "Working ...", "Configuring default data sources ")
                pStepProg.Show()
                progressDialog2.ShowDialog()
                pStepProg.Step()
                BtnDefaultSettings.Enabled = False
                Dim imageServiceList As IList(Of BagisImageService) = QueryDefaultDataSources()
                If imageServiceList.Count > 0 Then
                    pStepProg.Step()
                    ' Delete all datasources from the underlying datatable except for aoi
                    Dim keysToDelete As IList(Of String) = New List(Of String)
                    For Each strKey As String In m_dataTable.Keys
                        Dim dSource As DataSource = m_dataTable(strKey)
                        If Not dSource.AoiLayer Then
                            keysToDelete.Add(strKey)
                        End If
                    Next
                    For Each strKey As String In keysToDelete
                        m_dataTable.Remove(strKey)
                    Next
                    ' Read and add the default data sources
                    For Each iService As BagisImageService In imageServiceList
                        Dim dSource As DataSource = New DataSource(iService)
                        dSource.IsValid = BA_File_ExistsImageServer(dSource.Source)
                        m_dataTable.Add(dSource.Name, dSource)
                    Next
                    pStepProg.Step()
                    Dim success As BA_ReturnCode = BA_AppendUnitsToDataSources(m_dataTable, Nothing)
                    pStepProg.Step()
                    If success = BA_ReturnCode.Success AndAlso overwriteSettings = True Then
                        System.IO.File.Delete(m_settingsPath)
                        Dim dataLayerList As List(Of DataSource) = New List(Of DataSource)
                        For Each key As String In m_dataTable.Keys
                            dataLayerList.Add(m_dataTable(key))
                        Next
                        success = BA_SaveDataLayers(dataLayerList, m_settingsPath)
                        ReloadGrid()
                    Else
                        MessageBox.Show("An error occurred while trying to load the default data sources", "BAGIS-P", _
                                        MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            Catch ex As Exception
                Debug.Print("BtnDefaultSettings_Click Exception: " + ex.Message)
            Finally
                BtnDefaultSettings.Enabled = True
                If pStepProg IsNot Nothing Then
                    pStepProg.Hide()
                    pStepProg = Nothing
                    progressDialog2.HideDialog()
                    progressDialog2 = Nothing
                End If
            End Try
        End If
    End Sub
End Class