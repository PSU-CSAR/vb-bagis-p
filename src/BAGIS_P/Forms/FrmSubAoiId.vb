﻿Imports System.Windows.Forms
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports System.Text

Public Class FrmSubAoiId


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim aoi As Aoi = bExt.aoi
        If aoi IsNot Nothing Then
            Try
                TxtAoiPath.Text = aoi.FilePath
                Me.Text = "Manage SubbasinId Layers (AOI: " & aoi.Name & aoi.ApplicationVersion & " )"

                RefreshSubbasinGrid()
                RefreshGrdId()
                'Bring the window to the front
                Me.WindowState = FormWindowState.Normal
                Me.BringToFront()

            Catch ex As Exception
                MessageBox.Show("Unable to load current aoi. Exception: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub RefreshSubbasinGrid()
        GrdSubbasin.Rows.Clear()
        Dim subbasinPaths As IList(Of String) = BA_GetListOfSubbasinPaths(TxtAoiPath.Text)
        For Each subbasin As String In subbasinPaths
            Dim sName As String = BA_GetBareName(subbasin)
            '---create a row---
            Dim item As New DataGridViewRow
            item.CreateCells(GrdSubbasin)
            item.Cells(0).Value = sName
            GrdSubbasin.Rows.Add(item)
        Next
        'De-select any rows
        If GrdSubbasin.Rows.Count > 0 Then
            GrdSubbasin.CurrentCell = Nothing
            GrdSubbasin.ClearSelection()
            GrdSubbasin.Refresh()
        End If
        'Manage display button
        If GrdSubbasin.Rows.Count > 0 Then
            BtnDisplaySubbasin.Enabled = True
        Else
            BtnDisplaySubbasin.Enabled = False
        End If
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnDisplaySubbasin_Click(sender As System.Object, e As System.EventArgs) Handles BtnDisplaySubbasin.Click
        Dim pFSymbol As ISimpleFillSymbol = New SimpleFillSymbol
        Dim pColorRamp As IColorRamp = New PresetColorRamp
        Dim pStyleGallery As IStyleGallery = Nothing
        Dim pEnumStyleGallery As IEnumStyleGalleryItem = Nothing
        Dim pStyleItem As IStyleGalleryItem = Nothing
        Dim pRasterDS As IRasterDataset = Nothing
        Dim pRaster As IRaster = Nothing
        Dim pLayerEffects As ILayerEffects = Nothing
        Dim pTempLayer As ILayer = Nothing

        Try
            pStyleGallery = My.Document.StyleGallery
            pEnumStyleGallery = pStyleGallery.Items("Color Ramps", "ESRI.style", "Default Schemes")
            pEnumStyleGallery.Reset()

            pStyleItem = pEnumStyleGallery.Next
            Do Until pStyleItem Is Nothing
                If pStyleItem.Name = "Cool Tones" Then
                    pColorRamp = pStyleItem.Item
                    Exit Do
                End If
                pStyleItem = pEnumStyleGallery.Next
            Loop

            'assign value to the colorramp
            With pColorRamp
                .Size = GrdSubbasin.Rows.Count
                .CreateRamp(True)
            End With

            Dim i As Integer = 0
            For Each pRow As DataGridViewRow In GrdSubbasin.Rows
                Dim includeSub As Boolean = pRow.Cells(1).Value
                Dim displayName As String = pRow.Cells(0).Value
                If includeSub = True Then
                    Dim aoiGdbPath As String = BA_GeodatabasePath(displayName, GeodatabaseNames.Aoi)
                    Dim layerPathName As String = TxtAoiPath.Text & "\" & aoiGdbPath & BA_EnumDescription(PublicPath.AoiGrid)

                    Dim pUVRen As IRasterUniqueValueRenderer = New RasterUniqueValueRenderer
                    Dim pRasRen As IRasterRenderer = pUVRen

                    ' Connect renderer and raster 
                    pRasterDS = BA_OpenRasterFromGDB(TxtAoiPath.Text & "\" & aoiGdbPath, BA_GetBareName(BA_EnumDescription(PublicPath.AoiGrid)))
                    pRaster = pRasterDS.CreateDefaultRaster
                    pRasRen.Raster = pRaster
                    pRasRen.Update()

                    ' Configure UniqueValue renderer
                    pUVRen.HeadingCount = 1   ' Use one heading 
                    pUVRen.ClassCount(0) = 1
                    pUVRen.Field = BA_FIELD_VALUE

                    Dim Value As Object = 1
                    pUVRen.AddValue(0, 0, Value)
                    pUVRen.Label(0, 0) = CStr(Value)
                    pFSymbol.Color = pColorRamp.Color(i)
                    pUVRen.Symbol(0, 0) = pFSymbol

                    'add raster to current data frame
                    Dim pRLayer As IRasterLayer = New RasterLayer
                    pRLayer.CreateFromDataset(pRasterDS)
                    pRLayer.Name = displayName

                    ' Update render and refresh layer
                    pRasRen.Update()
                    pRLayer.Renderer = pUVRen

                    'set layer transparency
                    pLayerEffects = pRLayer
                    If pLayerEffects.SupportsTransparency Then
                        pLayerEffects.Transparency = 50
                    End If

                    'check if a layer with the assigned name exists
                    'search layer of the specified name, if found
                    Dim pMap As IMap = My.Document.FocusMap
                    Dim nlayers As Integer = pMap.LayerCount
                    For i = nlayers To 1 Step -1
                        pTempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                        If displayName = pTempLayer.Name Then 'remove the layer
                            pMap.DeleteLayer(pTempLayer)
                        End If
                    Next

                    My.Document.AddLayer(pRLayer)
                    My.Document.UpdateContents()

                    i += 1
                    pUVRen = Nothing
                    pRasRen = Nothing
                    pRLayer = Nothing
                Else
                    'If the layer is not checked, remove it if it is displayed
                    Dim pMap As IMap = My.Document.FocusMap
                    Dim nlayers As Integer = pMap.LayerCount
                    For i = nlayers To 1 Step -1
                        pTempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                        If displayName = pTempLayer.Name Then 'remove the layer
                            pMap.DeleteLayer(pTempLayer)
                        End If
                    Next
                End If
            Next
            'refresh the active view
            My.Document.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
        Catch ex As Exception
            Debug.Print("BtnDisplaySubbasin_Click " & ex.Message)
        Finally
            pFSymbol = Nothing
            pColorRamp = Nothing
            pRasterDS = Nothing
            pRaster = Nothing
            pLayerEffects = Nothing
            pTempLayer = Nothing
        End Try
    End Sub

    Private Sub BtnCreateSubbasin_Click(sender As System.Object, e As System.EventArgs) Handles BtnCreateSubbasin.Click
        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = Nothing
        Dim progressDialog2 As IProgressDialog2 = Nothing
        Dim pGeodataset As IGeoDataset = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pTable As ITable = Nothing
        Dim pQF As IQueryFilter = New QueryFilter
        Try
            Dim message, title As String
            Dim defaultValue As String = Nothing
            Dim outputFile As String = Nothing
            Dim subbasinPath As String = TxtAoiPath.Text & BA_EnumDescription(PublicPath.BagisSubAoiGdb)

            ' Set prompt.
            message = "Enter a name for the Subbasin Id layer:"
            ' Set title.
            title = "Create Subbasin Id Layer"

            ' Display message, title, and default value.
            outputFile = InputBox(message, title, defaultValue, 150, 150)
            ' If user has clicked Cancel, set myValue to defaultValue 
            If String.IsNullOrEmpty(outputFile) Then
                MessageBox.Show("You did not enter a name for the Subbasin Id layer. No layer will be created.", title,
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                If BA_File_Exists(subbasinPath & "\" & outputFile, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                    Dim returnValue As DialogResult = MessageBox.Show("A file exists with that name. Do you wish to overwrite it?" & vbCrLf & "This action cannot be undone.", title,
                                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    If returnValue <> Windows.Forms.DialogResult.Yes Then
                        Exit Sub
                    Else
                        BA_RemoveRasterFromGDB(subbasinPath, outputFile)
                    End If
                End If
            End If

            pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 20)
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Checking target geodatabase...", "Creating geodatabase")
            pStepProg.Show()
            progressDialog2.ShowDialog()
            pStepProg.Step()
            'Create subaoi.gdb in aoi if it doesn't exist; This is where we store the subbasin layers
            Dim success As BA_ReturnCode = BA_ReturnCode.UnknownError
            If Not BA_Folder_ExistsWindowsIO(subbasinPath) Then
                Dim gdbName As String = BA_GetBareName(subbasinPath)
                success = BA_CreateFileGdb(TxtAoiPath.Text, gdbName)
            Else
                success = BA_ReturnCode.Success
            End If
            If success = BA_ReturnCode.Success Then
                'The list of the grid values for the combined subbasin layer; In same order as subbasinList
                Dim subbasinTable As Hashtable = New Hashtable
                Dim fileBase As String = "subTemp"
                Dim maskFolder As String = BA_GeodatabasePath(TxtAoiPath.Text, GeodatabaseNames.Aoi)
                Dim i As Integer = 1
                For Each pRow As DataGridViewRow In GrdSubbasin.Rows
                    Dim includeSub As Boolean = pRow.Cells(1).Value
                    If includeSub = True Then
                        progressDialog2.Description = "Replacing NoData cells in subbasin layers"
                        pStepProg.Step()
                        Dim displayName As String = pRow.Cells(0).Value
                        Dim pSubbasin As SubBasin = New SubBasin(displayName)
                        pSubbasin.TempLayerName = fileBase & i
                        subbasinTable(displayName) = pSubbasin
                        Dim aoiGdbPath As String = BA_GeodatabasePath(displayName, GeodatabaseNames.Aoi)
                        Dim inputFolder As String = TxtAoiPath.Text & "\" & aoiGdbPath
                        'Delete raster if it exists before creating new one
                        If BA_File_Exists(subbasinPath & "\" & fileBase & i, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                            BA_RemoveRasterFromGDB(subbasinPath, fileBase & i)
                        End If
                        'Query and set max accumulation value on subbasin object
                        Dim accumValue As Integer = BA_QueryRasterFromPoint(inputFolder, BA_EnumDescription(MapsFileName.PourPoint),
                                                                       BA_GeodatabasePath(TxtAoiPath.Text, GeodatabaseNames.Surfaces),
                                                                       BA_EnumDescription(MapsFileName.flow_accumulation_gdb))
                        pSubbasin.MaxAccumValue = accumValue
                        Dim gaugeNumber As String = BA_QueryStringFieldFromVector(inputFolder, BA_EnumDescription(MapsFileName.PourPoint),
                                                                                  BA_FIELD_AWDB_ID)
                        pSubbasin.GaugeNumber = gaugeNumber
                        subbasinTable(displayName) = pSubbasin
                        success = BA_ReplaceNoDataCells(inputFolder, BA_GetBareName(BA_EnumDescription(PublicPath.AoiGrid)),
                                                           subbasinPath, pSubbasin.TempLayerName, "-1", maskFolder,
                                                           BA_GetBareName(BA_EnumDescription(PublicPath.AoiGrid)))
                        If success = BA_ReturnCode.Success Then
                            pSubbasin.TempFilePath = subbasinPath & "\" & pSubbasin.TempLayerName
                            subbasinTable(displayName) = pSubbasin
                        End If
                        i += 1
                    End If
                Next
                If success = BA_ReturnCode.Success Then
                    progressDialog2.Description = "Calculating subbasin ID values"
                    pStepProg.Step()
                    BA_CalculateSubbasinId(subbasinTable)
                    progressDialog2.Description = "Generating combined subbasin layer"
                    pStepProg.Step()
                    Dim maskFilePath As String = maskFolder & BA_EnumDescription(PublicPath.AoiGrid)
                    Dim combineList As IList(Of String) = New List(Of String)
                    Dim maxId As Int16 = 0
                    For Each pKey As String In subbasinTable.Keys
                        Dim sAoi As SubBasin = subbasinTable(pKey)
                        combineList.Add(sAoi.TempFilePath)
                        If sAoi.Id > maxId Then
                            maxId = sAoi.Id
                        End If
                    Next
                    success = BA_ZoneOverlay(maskFilePath, combineList, subbasinPath, outputFile,
                                             False, True, maskFilePath, WorkspaceType.Geodatabase)
                    If success = BA_ReturnCode.Success Then
                        progressDialog2.Description = "Appending attributes to subbasin layer"
                        pStepProg.Step()
                        'Delete temporary subbasin layers
                        For Each pKey As String In subbasinTable.Keys
                            Dim sAoi As SubBasin = subbasinTable(pKey)
                            BA_RemoveRasterFromGDB(subbasinPath, sAoi.TempLayerName)
                        Next
                        pGeodataset = BA_OpenRasterFromGDB(subbasinPath, outputFile)
                        If pGeodataset IsNot Nothing Then
                            pRasterBandCollection = CType(pGeodataset, IRasterBandCollection)
                            pRasterBand = pRasterBandCollection.Item(0)
                            pTable = pRasterBand.AttributeTable
                            Dim idxValue As Integer = pTable.FindField(BA_FIELD_VALUE)
                            Dim sortedArray(subbasinTable.Keys.Count - 1) As SubBasin
                            Dim j As Integer = 0
                            For Each sName As String In subbasinTable.Keys
                                Dim sAoi As SubBasin = subbasinTable(sName)
                                sortedArray(j) = sAoi
                                j += 1
                            Next
                            'Sort the subbasins so we get the lowest ID's first
                            System.Array.Sort(sortedArray, SubBasin.maxAccumAscending)
                            'Keep track of which combine codes have been assigned to a subbasin
                            Dim usedCombineCodes As IList(Of Integer) = New List(Of Integer)
                            'Loop through the sorted sub AOI's
                            For k As Integer = 0 To sortedArray.GetUpperBound(0)
                                'Get all the rows assigned to that subbasin
                                Dim sAoi As SubBasin = sortedArray(k)
                                pQF.WhereClause = """" & sAoi.TempLayerName & """ = 1"
                                Dim pCursor As ICursor = pTable.Search(pQF, False)
                                Dim pRow As IRow = pCursor.NextRow
                                Dim combineValList As IList(Of Integer) = New List(Of Integer)
                                Do Until pRow Is Nothing
                                    Dim pValue As Integer = pRow.Value(idxValue)
                                    'If that combine code hasn't been assigned to an upstream subbasin
                                    'Assign it to this one
                                    If Not usedCombineCodes.Contains(pValue) Then
                                        combineValList.Add(pValue)
                                    End If
                                    'Save the collection of Combine codes to the subbasin
                                    usedCombineCodes.Add(pValue)
                                    pRow = pCursor.NextRow
                                Loop
                                sAoi.CombineValueList = combineValList
                                subbasinTable(sAoi.Name) = sAoi
                                pRow = Nothing
                                pCursor = Nothing
                            Next
                            Dim valuesCursor As ICursor = Nothing
                            Dim pDataStatistics As IDataStatistics = New DataStatistics
                            valuesCursor = pTable.Search(Nothing, False)
                            'initialize properties for the dataStatistics interface
                            pDataStatistics.Field = BA_FIELD_VALUE
                            pDataStatistics.Cursor = valuesCursor
                            Dim valuesEnum As IEnumerator = pDataStatistics.UniqueValues
                            Dim whereClause As String = ""
                            valuesEnum.MoveNext()
                            Dim nextValue As Integer = Convert.ToInt32(valuesEnum.Current)
                            Do While nextValue > 0
                                If usedCombineCodes.Contains(nextValue) Then
                                    'Do nothing; the Value is used
                                Else
                                    whereClause = BA_FIELD_VALUE + " = " + CStr(nextValue)
                                    Exit Do
                                End If
                                valuesEnum.MoveNext()
                                nextValue = Convert.ToInt32(valuesEnum.Current)
                            Loop
                            valuesCursor = Nothing
                            pDataStatistics = Nothing
                            valuesEnum = Nothing
                            If Not String.IsNullOrEmpty(whereClause) Then
                                BA_UpdateSubbasinAttributeTable(subbasinPath, outputFile, subbasinTable, maxId, whereClause)
                            End If
                        End If
                    End If
                End If
                RefreshGrdId()
            End If

        Catch ex As Exception
            Debug.Print("BtnCreateSubbasin_Click Exception: " & ex.Message)
        Finally
            If pStepProg IsNot Nothing Then
                pStepProg.Hide()
                pStepProg = Nothing
                progressDialog2.HideDialog()
                progressDialog2 = Nothing
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try

    End Sub

    Private Sub BtnSelectAoi_Click(sender As System.Object, e As System.EventArgs) Handles BtnSelectAoi.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers
        Dim bagisPExt As BagisPExtension = BagisPExtension.GetExtension

        Try
            'TestSort()
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
                'attempt to set default projection to Albers
                success = BA_SetDefaultProjection(My.ArcMap.Application)
                If success = BA_ReturnCode.Success Then
                    Dim aoiName As String = BA_GetBareName(DataPath)
                    Dim pAoi As Aoi = New Aoi(aoiName, DataPath, Nothing, bagisPExt.version)
                    TxtAoiPath.Text = pAoi.FilePath
                    'ResetForm()
                    Me.Text = "Manage Subbasin Id Layers (AOI: " & aoiName & pAoi.ApplicationVersion & " )"
                    bagisPExt.aoi = pAoi

                    RefreshSubbasinGrid()
                    RefreshGrdId()
                End If
            Else
                TxtAoiPath.Text = ""
                Me.Text = "Manage Subbasin Id Layers (AOI:  )"
            End If
            'Bring the window to the front
            Me.WindowState = FormWindowState.Normal
            Me.BringToFront()
        Catch ex As Exception
            MessageBox.Show("BtnSelectAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub RefreshGrdId()
        GrdId.Rows.Clear()
        Dim subbasinGdbPath As String = TxtAoiPath.Text & BA_EnumDescription(PublicPath.BagisSubAoiGdb)
        If BA_Folder_ExistsWindowsIO(subbasinGdbPath) Then
            Dim rasterList As String() = Nothing
            Dim vectorList As String() = Nothing
            BA_ListLayersinGDB(subbasinGdbPath, rasterList, vectorList)
            'rasterList has an empty first position as a holdover from VBA
            For i As Integer = 1 To rasterList.GetUpperBound(0)
                '---create a row---
                Dim item As New DataGridViewRow
                item.CreateCells(GrdId)
                item.Cells(0).Value = rasterList(i)
                item.Cells(1).Value = False
                GrdId.Rows.Add(item)
            Next
        End If
        'De-select any rows
        If GrdId.Rows.Count > 0 Then
            GrdId.CurrentCell = Nothing
            GrdId.ClearSelection()
            GrdId.Refresh()
        End If
        'Manage display button
        If GrdId.Rows.Count > 0 Then
            BtnDisplayId.Enabled = True
        Else
            BtnDisplayId.Enabled = False
        End If
    End Sub

    Private Sub GrdSubbasin_CellContentClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GrdSubbasin.CellContentClick
        Dim idxColumn As Integer = e.ColumnIndex
        'We changed one of the checkboxes
        If idxColumn = 1 Then
            Dim selectedLayers As Integer = 0
            For Each pRow As DataGridViewRow In GrdSubbasin.Rows
                Dim selected As Boolean = Convert.ToBoolean(pRow.Cells(idxColumn).EditedFormattedValue)
                If selected = True Then
                    selectedLayers += 1
                    Exit For
                End If
            Next
            If selectedLayers > 0 Then
                BtnCreateSubbasin.Enabled = True
            Else
                BtnCreateSubbasin.Enabled = False
            End If
        End If
    End Sub

    Private Sub GrdId_CellContentClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GrdId.CellContentClick
        Dim idxColumn As Integer = e.ColumnIndex
        'We changed one of the checkboxes
        If idxColumn = 1 Then
            Dim selectedLayers As Integer = 0
            For Each pRow As DataGridViewRow In GrdId.Rows
                Dim selected As Boolean = Convert.ToBoolean(pRow.Cells(idxColumn).EditedFormattedValue)
                If selected = True Then
                    selectedLayers += 1
                    Exit For
                End If
            Next
            If selectedLayers > 0 Then
                BtnDelete.Enabled = True
                BtnRename.Enabled = True
            Else
                BtnDelete.Enabled = False
                BtnRename.Enabled = False
            End If
        End If
    End Sub

    Private Sub BtnDisplayId_Click(sender As Object, e As System.EventArgs) Handles BtnDisplayId.Click
        Dim subbasinPath As String = TxtAoiPath.Text & BA_EnumDescription(PublicPath.BagisSubAoiGdb)
        Dim pTempLayer As ILayer = Nothing
        For Each pRow As DataGridViewRow In GrdId.Rows
            Dim includeSub As Boolean = pRow.Cells(1).Value
            Dim displayName As String = pRow.Cells(0).Value
            If includeSub = True Then
                BA_DisplayRasterWithSymbol(My.Document, subbasinPath & "\" & displayName,
                                           displayName, MapsDisplayStyle.Cool_Tones, 50,
                                           WorkspaceType.Geodatabase)
            Else
                'If the layer is not checked, remove it if it is displayed
                Dim pMap As IMap = My.Document.FocusMap
                Dim nlayers As Integer = pMap.LayerCount
                For i = nlayers To 1 Step -1
                    pTempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                    If displayName = pTempLayer.Name Then 'remove the layer
                        pMap.DeleteLayer(pTempLayer)
                    End If
                Next
                pTempLayer = Nothing
            End If
        Next

    End Sub

    Private Sub BtnDelete_Click(sender As System.Object, e As System.EventArgs) Handles BtnDelete.Click
        Dim sb As StringBuilder = New StringBuilder()
        sb.Append("The selected layers will be permanently deleted." & vbCrLf)
        sb.Append("This action cannot be undone. Do you wish to continue ?" & vbCrLf)
        Dim returnValue As DialogResult = MessageBox.Show(sb.ToString, "Delete layer(s)", _
                                                          MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If returnValue = Windows.Forms.DialogResult.Yes Then
            Dim subbasinPath As String = TxtAoiPath.Text & BA_EnumDescription(PublicPath.BagisSubAoiGdb)
            Dim pTempLayer As ILayer = Nothing
            Dim pMap As IMap = My.Document.FocusMap
            Dim nlayers As Integer = pMap.LayerCount
            For Each pRow As DataGridViewRow In GrdId.Rows
                Dim deleteSub As Boolean = pRow.Cells(1).Value
                Dim layerName As String = pRow.Cells(0).Value
                If deleteSub = True Then
                    'Remove layer from map before we try to delete it
                    For i = nlayers To 1 Step -1
                        pTempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                        If layerName = pTempLayer.Name Then 'remove the layer
                            pMap.DeleteLayer(pTempLayer)
                        End If
                    Next
                    pTempLayer = Nothing
                    BA_RemoveRasterFromGDB(subbasinPath, layerName)
                End If
            Next
            RefreshGrdId()
        End If
    End Sub

    Private Sub BtnRename_Click(sender As System.Object, e As System.EventArgs) Handles BtnRename.Click
        Dim subbasinPath As String = TxtAoiPath.Text & BA_EnumDescription(PublicPath.BagisSubAoiGdb)
        Dim pTempLayer As ILayer = Nothing
        Dim pMap As IMap = My.Document.FocusMap
        Dim nlayers As Integer = pMap.LayerCount
        For Each pRow As DataGridViewRow In GrdId.Rows
            Dim renameSub As Boolean = pRow.Cells(1).Value
            Dim layerName As String = pRow.Cells(0).Value
            If renameSub = True Then
                'Remove layer from map before we try to rename it
                Dim layerRemoved As Boolean = False
                For i = nlayers To 1 Step -1
                    pTempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                    If layerName = pTempLayer.Name Then 'remove the layer
                        pMap.DeleteLayer(pTempLayer)
                        layerRemoved = True
                    End If
                Next
                pTempLayer = Nothing
                ' Set prompt.
                Dim message As String = "Enter a new name for the " & layerName & " layer:"
                ' Set title.
                Dim title As String = "Rename Subbasin Id Layer"
                ' Display message, title, and default value.
                Dim outputFile As String = InputBox(message, title, "", 150, 150)
                ' If user has clicked Cancel, set myValue to defaultValue 
                If String.IsNullOrEmpty(outputFile) Then
                    MessageBox.Show("You did not enter a new name. This layer will not be renamed.", title,
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                Else
                    If BA_File_Exists(subbasinPath & "\" & outputFile, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                        Dim returnValue As DialogResult = MessageBox.Show("A file exists with that name. Do you wish to overwrite it?" & vbCrLf & "This action cannot be undone.", title,
                                                                          MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                        If returnValue <> Windows.Forms.DialogResult.Yes Then
                            Exit Sub
                        Else
                            BA_RemoveRasterFromGDB(subbasinPath, outputFile)
                        End If
                    End If
                End If
                BA_RenameRasterInGDB(subbasinPath, layerName, outputFile)
                If layerRemoved Then
                    BA_DisplayRasterWithSymbol(My.Document, subbasinPath & "\" & outputFile,
                           outputFile, MapsDisplayStyle.Cool_Tones, 50,
                           WorkspaceType.Geodatabase)
                End If
            End If
        Next
        RefreshGrdId()

    End Sub
End Class