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
        ClearReclass(Nothing, False)
    End Sub

    Private Sub RdoHru_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RdoHru.CheckedChanged
        LstAoiRasterLayers.Items.Clear()
        LstAoiRasterLayers.Items.AddRange(LstHruLayers.Items)
        ClearReclass(Nothing, False)
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
                    ClearReclass(pRasterBand, validVAT)
                Else
                    Dim errorMsg As String = "Unable to open layer '" & item.Name & "'"
                    MessageBox.Show(errorMsg, "Invalid layer", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ClearReclass(Nothing, False)
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

    Private Sub ClearReclass(ByVal rasterBand As IRasterBand, ByVal hasAttributeTable As Boolean)
        CboReclassField.Items.Clear()
        Dim idxValue As Integer = -1
        If rasterBand IsNot Nothing AndAlso hasAttributeTable = True Then
            Dim pTable As ITable = rasterBand.AttributeTable
            Dim pFields As IFields = pTable.Fields
            Dim uBound As Integer = pFields.FieldCount - 1
            For i = 1 To uBound
                Dim pField As IField = pFields.Field(i)
                CboReclassField.Items.Add(pField.Name)
                'Always set the selected index to 'VALUE'
                If pField.Name.ToUpper.Equals(BA_FIELD_VALUE) Then idxValue = i
            Next
            If idxValue > -1 Then
                CboReclassField.SelectedIndex = idxValue - 1
            Else
                CboReclassField.SelectedIndex = 0
            End If
        Else
            DataGridView1.Rows.Clear()
        End If
    End Sub

    Private Sub CopyUniqueValuesToReclass()
        DataGridView1.Rows.Clear()
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
                Dim fieldName As String = CStr(CboReclassField.SelectedItem)
                pData.Field = fieldName
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
                    DataGridView1.Rows.Add(pArray)
                    pEnumVar.MoveNext()
                    pObj = pEnumVar.Current
                End While
            End If
        Catch ex As Exception
            MessageBox.Show("CopyUniqueValuesToReclass Exception: " + ex.Message)
        Finally
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

    Private Sub CboReclassField_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboReclassField.SelectedIndexChanged
        If CboReclassField.SelectedIndex > -1 Then
            CopyUniqueValuesToReclass()
        End If
    End Sub
End Class