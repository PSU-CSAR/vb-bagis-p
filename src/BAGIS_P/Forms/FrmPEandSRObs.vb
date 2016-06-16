Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.esriSystem

Public Class FrmPEandSRObs

    Private Const SR_COLUMN_PREFIX As String = "SR"
    Dim m_january As DateTime = New DateTime(2015, 1, 1)
    Dim m_pe_prefix As String = Nothing
    Dim m_pe_suffix As String = Nothing
    Dim m_aoiParamTable As IDictionary(Of String, AoiParameter)

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim aoi As Aoi = bExt.aoi
        If aoi IsNot Nothing Then
            Try
                TxtAoiPath.Text = aoi.FilePath
                BA_SetDefaultProjection(My.ArcMap.Application)
                BA_SetDatumInExtension(TxtAoiPath.Text)
                Me.Text = "Calculate PE and SR Obs parameters (AOI: " & aoi.Name & aoi.ApplicationVersion & " )"
                PopulateForm()
            Catch ex As Exception
                MessageBox.Show("Unable to load current aoi. Exception: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub BtnSelectAoi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelectAoi.Click
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
                BA_SetDefaultProjection(My.ArcMap.Application)
                Dim aoiName As String = BA_GetBareName(DataPath)
                Dim pAoi As Aoi = New Aoi(aoiName, DataPath, Nothing, bagisPExt.version)
                TxtAoiPath.Text = pAoi.FilePath
                BA_SetDatumInExtension(TxtAoiPath.Text)
                'ResetForm()
                Me.Text = "Calculate PE and SR Obs parameters (AOI: " & aoiName & pAoi.ApplicationVersion & " )"
                bagisPExt.aoi = pAoi
                PopulateForm()
            End If
        Catch ex As Exception
            MessageBox.Show("BtnSelectAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnSetSR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSetSR.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog()
        Dim pGxObject As IEnumGxObject = Nothing

        Dim pFilter As IGxObjectFilter = New GxFilterPointFeatureClasses()

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select solar radiation data layer"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDataset As IGxDataset
        pGxDataset = pGxObject.Next
        Dim pDatasetName As IDatasetName = pGxDataset.DatasetName
        Dim fullPath As String = pDatasetName.WorkspaceName.PathName & "\" & pDatasetName.Name

        'Ensure selected layer uses the correct projection
        Dim aoiGdb As String = BA_GeodatabasePath(TxtAoiPath.Text, GeodatabaseNames.Aoi, True)
        Dim aoiProjection As String = Nothing    'Variable to hold SR projection for error message
        Dim aoi_v As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiVector), False)
        Dim validSpatialReference As Boolean = BA_VectorProjectionMatch(fullPath, aoiGdb & aoi_v, aoiProjection)

        If validSpatialReference Then
            TxtSrPath.Text = fullPath
        Else
            MessageBox.Show("The selected layer '" & pDatasetName.Name & "' cannot be used because the projection does not match the AOI projection '" & aoiProjection & _
                            "'. Please reproject or select another data layer and try again.", "Invalid projection", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub BtnSetPE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSetPE.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing

        Dim pFilter As IGxObjectFilter = New GxFilterRasterDatasets

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select potential evaporation data layer for January"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDataset As IGxDataset = pGxObject.Next
        Dim pDatasetName As IDatasetName = pGxDataset.DatasetName
        Dim Data_Path As String = pDatasetName.WorkspaceName.PathName
        Dim fullPath As String = pDatasetName.WorkspaceName.PathName & "\" & pDatasetName.Name

        'Ensure selected layer uses the correct projection
        Dim aoiGdb As String = BA_GeodatabasePath(TxtAoiPath.Text, GeodatabaseNames.Aoi, True)
        Dim aoiDatum As String = Nothing    'Variable to hold PE projection for error message
        Dim aoi_v As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiVector), False)
        Dim validDatum As Boolean = BA_DatumMatchFiles(fullPath, esriDatasetType.esriDTRasterDataset, aoiGdb & aoi_v, esriDatasetType.esriDTFeatureClass, aoiDatum)

        If Not validDatum Then
            MessageBox.Show("The selected layer '" & pDatasetName.Name & "' cannot be used because the datum does not match the AOI datum '" & aoiDatum & _
                            "'. Please reproject or select another data layer and try again.", "Invalid datum", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim strSearch As String = m_january.Month.ToString("D2")
        Dim idxMonth As Integer = pDatasetName.Name.IndexOf(strSearch)
        Dim missingMonths As IList(Of String) = New List(Of String)
        'We have January, now look for other 11
        If idxMonth > -1 Then
            m_pe_prefix = pDatasetName.Name.Substring(0, idxMonth)
            m_pe_suffix = pDatasetName.Name.Substring(idxMonth + 2, pDatasetName.Name.Length - (idxMonth + 2))
            Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(pDatasetName.WorkspaceName.PathName)
            For i As Short = 2 To NUM_MONTHS
                Dim fileName As String = m_pe_prefix & i.ToString("D2") & m_pe_suffix
                If Not BA_File_Exists(pDatasetName.WorkspaceName.PathName & "\" & fileName, wType, esriDatasetType.esriDTRasterDataset) Then
                    'missingMonths.Add(pDatasetName.WorkspaceName.PathName & "\" & fileName)
                    missingMonths.Add(i.ToString("D2"))
                End If
            Next
            If missingMonths.Count > 0 Then
                Dim errMsg As String = "Some months associated with the selected layer '" & pDatasetName.Name & "' could not be found. If this layer is used, the PE value will not be calculated for the following months: " & vbCrLf & vbCrLf
                For i As Short = 0 To missingMonths.Count - 1
                    errMsg += missingMonths(i) & vbCrLf
                Next
                MessageBox.Show(errMsg, "Missing data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Else
            MessageBox.Show("The selected layer '" & pDatasetName.Name & "' cannot be used because there is not a '01' in the name indicating that it contains data for January. Please select another data layer", _
                            "Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'Passed all the tests!
        TxtPEPath.Text = fullPath
    End Sub

    Private Sub ManageCalculateButton()
        If Not String.IsNullOrEmpty(TxtAoiPath.Text) AndAlso _
            Not String.IsNullOrEmpty(TxtSrPath.Text) AndAlso _
            Not String.IsNullOrEmpty(TxtPEPath.Text) Then
            BtnCalculate.Enabled = True
        Else
            BtnCalculate.Enabled = False
        End If
    End Sub

    Private Sub ManageFileButtons()
        If String.IsNullOrEmpty(TxtAoiPath.Text) Then
            BtnSetSR.Enabled = False
            BtnSetPE.Enabled = False
        Else
            BtnSetSR.Enabled = True
            BtnSetPE.Enabled = True
        End If
    End Sub

    Private Sub TxtAoiPath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtAoiPath.TextChanged
        ManageCalculateButton()
        ManageFileButtons()
    End Sub

    Private Sub TxtSrPath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtSrPath.TextChanged
        ManageCalculateButton()
    End Sub

    Private Sub TxtPEPath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtPEPath.TextChanged
        ManageCalculateButton()
    End Sub

    Private Function CalculateSRObs() As BA_ReturnCode
        Dim pCentroid As IPoint = Nothing
        Try
            Dim aoiGDB As String = BA_GeodatabasePath(TxtAoiPath.Text, GeodatabaseNames.Aoi)
            Dim aoiFile As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiVector), False)
            pCentroid = FindCentroid(aoiGDB, aoiFile)
            Dim zValueClosestFeature As String = Nothing
            Dim oid As Integer = BA_FindClosestFeatureOID(pCentroid, TxtSrPath.Text, zValueClosestFeature)
            'Create AOI Parameter for zValue so we can persist it
            Dim elevParameter As AoiParameter = New AoiParameter(BA_Aoi_Parameter_SR_Elevation, zValueClosestFeature)
            elevParameter.DateUpdated = DateTime.Now
            If m_aoiParamTable.ContainsKey(BA_Aoi_Parameter_SR_Obs) Then
                m_aoiParamTable(BA_Aoi_Parameter_SR_Elevation) = elevParameter
            Else
                m_aoiParamTable.Add(BA_Aoi_Parameter_SR_Elevation, elevParameter)
            End If
            Dim missingValuesCount As Short = 0
            Dim newParameter As AoiParameter = ExtractSrValues(oid, missingValuesCount)
            If m_aoiParamTable.ContainsKey(BA_Aoi_Parameter_SR_Obs) Then
                m_aoiParamTable(BA_Aoi_Parameter_SR_Obs) = newParameter
            Else
                m_aoiParamTable.Add(BA_Aoi_Parameter_SR_Obs, newParameter)
            End If
            TxtSrDate.Text = newParameter.DateUpdated.ToString("MM/dd/yyyy")
            If newParameter.ValuesList IsNot Nothing Then
                TxtSrValue.Text = newParameter.ValuesList(0)
            End If
            If missingValuesCount > 0 Then
                MessageBox.Show("One or more solar radiation values could not be determined.", "Warning", _
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            Debug.Print("CalculateSRObs Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pCentroid = Nothing
        End Try
    End Function

    Private Sub BtnCalculate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCalculate.Click
        BtnCalculate.Enabled = False
        LblStatus.Text = "Calculating solar radiation"
        CalculateSRObs()
        LblStatus.Text = "Calculating potential evaporation"
        CalculatePEObs()
        LblStatus.Text = "Saving values to AOI"
        SaveValues()
        LblStatus.Text = Nothing
        MessageBox.Show("PE and SR Obs calculations are complete!", "Calculations complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        BtnCalculate.Enabled = True
    End Sub

    Private Sub PopulateForm()
        Dim localSettingsPath As String = BA_GetLocalSettingsPath(TxtAoiPath.Text)
        m_aoiParamTable = BA_LoadAoiParameters(localSettingsPath)
        If m_aoiParamTable IsNot Nothing Then
            If m_aoiParamTable.ContainsKey(BA_Aoi_Parameter_SR_Obs) Then
                Dim srObsParam As AoiParameter = m_aoiParamTable(BA_Aoi_Parameter_SR_Obs)
                If srObsParam IsNot Nothing Then
                    TxtSrDate.Text = srObsParam.DateUpdated.ToString("MM/dd/yyyy")
                    If srObsParam.ValuesList IsNot Nothing Then
                        TxtSrValue.Text = srObsParam.ValuesList(0)
                    End If
                End If
            Else
                TxtSrDate.Text = Nothing
                TxtSrValue.Text = Nothing
            End If
            If m_aoiParamTable.ContainsKey(BA_Aoi_Parameter_PE_Obs) Then
                Dim peObsParam As AoiParameter = m_aoiParamTable(BA_Aoi_Parameter_PE_Obs)
                If peObsParam IsNot Nothing Then
                    txtPEDate.Text = peObsParam.DateUpdated.ToString("MM/dd/yyyy")
                    If peObsParam.ValuesList IsNot Nothing Then
                        txtPeValue.Text = peObsParam.ValuesList(0)
                    End If
                End If
            Else
                txtPEDate.Text = Nothing
                txtPeValue.Text = Nothing
            End If
        End If
        'Reset SR and PE data sources in case user selected an AOI with different projection
        'We check the projections when the data source is set
        TxtSrPath.Text = Nothing
        TxtPEPath.Text = Nothing
    End Sub

    Private Function ExtractSrValues(ByVal oid As Integer, ByRef missingValues As Short) As AoiParameter
        Dim fClass As IFeatureClass = Nothing
        Dim pQF As IQueryFilter = New QueryFilter
        Dim fCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing
        Try
            Dim srValues As List(Of String) = New List(Of String)
            If oid > -1 Then
                Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(TxtSrPath.Text)
                Dim parentPath As String = "PleaseReturn"
                Dim fileName As String = BA_GetBareName(TxtSrPath.Text, parentPath)
                If wType = WorkspaceType.Geodatabase Then
                    fClass = BA_OpenFeatureClassFromGDB(parentPath, fileName)
                Else
                    fClass = BA_OpenFeatureClassFromFile(parentPath, fileName)
                End If
                If fClass IsNot Nothing Then
                    pQF.WhereClause = BA_FIELD_OBJECT_ID & " = " & oid
                    fCursor = fClass.Search(pQF, False)
                    pFeature = fCursor.NextFeature
                    If pFeature IsNot Nothing Then
                        Dim srFields As IFields = pFeature.Fields
                        Dim idxSR As Integer
                        For i As Short = 1 To NUM_MONTHS
                            Dim fieldName As String = SR_COLUMN_PREFIX & i.ToString("D2")
                            idxSR = srFields.FindField(fieldName)
                            If idxSR > -1 Then
                                srValues.Add(Convert.ToString(pFeature.Value(idxSR)))
                            Else
                                srValues.Add(BA_9999)
                                missingValues += 1
                            End If
                        Next
                    End If
                End If
            End If
            Dim returnObj As AoiParameter = New AoiParameter(BA_Aoi_Parameter_SR_Obs)
            'Populate srValues with default values if there was an error
            If srValues.Count < NUM_MONTHS Then
                missingValues = 0
                srValues.Clear()
                For i As Short = 1 To NUM_MONTHS
                    srValues.Add(BA_9999)
                    missingValues += 1
                Next
            End If
            returnObj.ValuesList = srValues
            Return returnObj
        Catch ex As Exception
            Debug.Print("ExtractSrValues Exception " & ex.Message)
            Return Nothing
        Finally
            fClass = Nothing
            pQF = Nothing
            fCursor = Nothing
            pFeature = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Private Function SaveValues() As BA_ReturnCode
        Dim srcList As IList(Of AoiParameter) = New List(Of AoiParameter)
        For Each key As String In m_aoiParamTable.Keys
            Dim param As AoiParameter = m_aoiParamTable(key)
            srcList.Add(param)
        Next
        Dim settingsPath As String = BA_GetLocalSettingsPath(TxtAoiPath.Text)
        Return BA_SaveAOIParameters(srcList, settingsPath)
    End Function

    Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click
        Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.PeAndObsTool)
        toolHelpForm.ShowDialog()
    End Sub

    Private Function CalculatePEObs() As BA_ReturnCode
        Dim pCentroid As IPoint = Nothing
        Dim pTable As ITable = Nothing
        Dim pCursor As ICursor = Nothing
        Dim pRow As IRow = Nothing
        Dim tableName As String = "tblPE"
        Dim zoneFilePath As String = BA_GeodatabasePath(TxtAoiPath.Text, GeodatabaseNames.Aoi)
        Dim zoneFileName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiVector), False)
        Dim snapRasterPath As String = BA_GeodatabasePath(TxtAoiPath.Text, GeodatabaseNames.Aoi, True) & BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
        Dim peFolder As String = "PleaseReturn"
        Dim peFileName As String = BA_GetBareName(TxtPEPath.Text, peFolder)
        Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(peFolder)
        Dim valuesList As IList(Of String) = New List(Of String)

        Try
            Dim useZonalStatistics As Boolean = UseZonalStats(zoneFilePath, zoneFileName, peFolder, peFileName)
            If useZonalStatistics = False Then pCentroid = FindCentroid(zoneFilePath, zoneFileName)
            For i As Short = 1 To NUM_MONTHS
                Dim fileName As String = m_pe_prefix & i.ToString("D2") & m_pe_suffix
                Dim valueFilePath As String = peFolder & fileName
                Dim mmValue As Double = CDbl(BA_9999)
                If BA_File_Exists(valueFilePath, wType, esriDatasetType.esriDTRasterDataset) Then
                    LblStatus.Text = "Calculating potential evaporation for " & MonthName(i)
                    If useZonalStatistics = False Then
                        mmValue = ExtractValueAtCentroid(pCentroid, peFolder, fileName)
                    Else
                        Dim success As BA_ReturnCode = BA_ZonalStatisticsAsTable(zoneFilePath, zoneFileName, BA_FIELD_AOI_NAME, valueFilePath, _
                                                                                 zoneFilePath, tableName, snapRasterPath, StatisticsTypeString.MEAN)
                        If success = BA_ReturnCode.Success Then
                            pTable = BA_OpenTableFromGDB(zoneFilePath, tableName)
                            If pTable IsNot Nothing Then
                                Dim idxMean As Integer = pTable.FindField(StatisticsFieldName.MEAN.ToString)
                                If idxMean > -1 Then
                                    pCursor = pTable.Search(Nothing, False)
                                    pRow = pCursor.NextRow
                                    If pRow IsNot Nothing Then mmValue = Convert.ToDouble(pRow.Value(idxMean))
                                End If
                            End If
                        End If
                    End If
                End If
                'Convert millimeters to inches
                Dim inchesValue As Double = mmValue / BA_Inches_To_Millimeters
                'Divide by # of days in month to get inches per day
                Dim peValue As String = Convert.ToString(inchesValue / DateTime.DaysInMonth(m_january.Year, i))
                valuesList.Add(peValue)
            Next
            'Create the new parameter and add to table
            Dim newParameter As AoiParameter = New AoiParameter(BA_Aoi_Parameter_PE_Obs)
            newParameter.ValuesList = valuesList
            If m_aoiParamTable.ContainsKey(BA_Aoi_Parameter_PE_Obs) Then
                m_aoiParamTable(BA_Aoi_Parameter_PE_Obs) = newParameter
            Else
                m_aoiParamTable.Add(BA_Aoi_Parameter_PE_Obs, newParameter)
            End If
            txtPEDate.Text = newParameter.DateUpdated.ToString("MM/dd/yyyy")
            If newParameter.ValuesList IsNot Nothing Then
                txtPeValue.Text = newParameter.ValuesList(0)
            End If
        Catch ex As Exception
            Debug.Print("CalculatePEObs() Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pCentroid = Nothing
            pTable = Nothing
            pRow = Nothing
            pCursor = Nothing
        End Try
    End Function

    Private Function ExtractValueAtCentroid(ByVal pCentroid As IPoint, ByVal folderName As String, ByVal fileName As String) As Double
        Dim pIdentify As IIdentify = Nothing
        Dim pRasterLy As IRasterLayer = New RasterLayer
        Dim pRasterDS As IRasterDataset = Nothing
        Dim pIDArray As IArray = Nothing
        Dim pRasObj As IRasterIdentifyObj = Nothing
        Dim returnVal As Double = BA_9999
        Try
            Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(folderName)
            If wType = WorkspaceType.Raster Then
                pRasterDS = BA_OpenRasterFromFile(folderName, fileName)
            ElseIf wType = WorkspaceType.Geodatabase Then
                pRasterDS = BA_OpenRasterFromGDB(folderName, fileName)
            End If
            If pRasterDS IsNot Nothing Then
                pRasterLy.CreateFromDataset(pRasterDS)
                pIdentify = CType(pRasterLy, IIdentify)
                'Identify on the raster layer
                pIDArray = pIdentify.Identify(pCentroid)
                If pIDArray IsNot Nothing Then
                    pRasObj = pIDArray.Element(0)
                    Double.TryParse(pRasObj.Name, returnVal)
                    'Set to default value if the parse didn't work
                    If returnVal = 0 Then returnVal = BA_9999
                End If
            End If
            Return returnVal
        Catch ex As Exception
            Debug.Print("FindCentroid() Exception: " & ex.Message)
            Return BA_9999
        Finally
            pRasterDS = Nothing
            pRasterLy = Nothing
            pRasObj = Nothing
            pIDArray = Nothing
            pIdentify = Nothing
        End Try
    End Function

    Private Function FindCentroid(ByVal folderName As String, ByVal fileName As String) As IPoint
        Dim aoiFeatures As IFeatureClass = Nothing
        Dim fCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing
        Dim pArea As IArea = Nothing
        Dim pCentroid As IPoint = Nothing
        Try
            aoiFeatures = BA_OpenFeatureClassFromGDB(folderName, fileName)
            If aoiFeatures IsNot Nothing Then
                fCursor = aoiFeatures.Search(Nothing, False)
                If fCursor IsNot Nothing Then
                    Dim largestArea As Double = 0
                    ' There may be > one feature in AOI vector, use the biggest one
                    pFeature = fCursor.NextFeature
                    Do While pFeature IsNot Nothing
                        pArea = pFeature.Shape
                        If pArea.Area > largestArea Then
                            largestArea = pArea.Area
                            pCentroid = pArea.Centroid
                        End If
                        pFeature = fCursor.NextFeature
                    Loop
                End If
            End If
            Return pCentroid
        Catch ex As Exception
            Debug.Print("FindCentroid() Exception: " & ex.Message)
            Return Nothing
        Finally
            aoiFeatures = Nothing
            fCursor = Nothing
            pFeature = Nothing
            pArea = Nothing
        End Try
    End Function

    'Zonal statistics doesn't work if the aoi (zones layer) is too small relative to the statistics (raster) layer;
    'This method returns false if the AOI area is < 200% of the cell area
    'Assuming both layers are projected and have a linear unit
    Private Function UseZonalStats(ByVal aoiFolderName As String, ByVal aoiFileName As String, ByVal peFolderName As String, _
                                   ByVal peFileName As String) As Boolean
        Dim aoiFeatures As IFeatureClass = Nothing
        Dim fCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing
        Dim pArea As IArea = Nothing
        Dim geoDataSet As IGeoDataset = Nothing
        Dim pSpatialRef As ISpatialReference
        Dim projCoordSys As IProjectedCoordinateSystem

        Try
            aoiFeatures = BA_OpenFeatureClassFromGDB(aoiFolderName, aoiFileName)
            If aoiFeatures IsNot Nothing Then
                geoDataSet = CType(aoiFeatures, IGeoDataset)
                pSpatialRef = geoDataSet.SpatialReference
                projCoordSys = TryCast(pSpatialRef, IProjectedCoordinateSystem)
                Dim pLinearUnit As ILinearUnit = projCoordSys.CoordinateUnit
                Dim largestArea As Double = 0
                fCursor = aoiFeatures.Search(Nothing, False)
                If fCursor IsNot Nothing Then
                    ' There may be > one feature in AOI vector, use the biggest one
                    pFeature = fCursor.NextFeature
                    Do While pFeature IsNot Nothing
                        pArea = pFeature.Shape
                        If pArea.Area > largestArea Then
                            largestArea = pArea.Area
                        End If
                        pFeature = fCursor.NextFeature
                    Loop
                End If
                largestArea = largestArea * pLinearUnit.MetersPerUnit

                Dim cellSize As Double = BA_CellSize(peFolderName, peFileName)
                Dim linearUnit As ESRI.ArcGIS.Geometry.ILinearUnit = BA_GetLinearUnitOfProjectedRaster(peFolderName, peFileName)
                Dim cellArea As Double = cellSize * cellSize * linearUnit.MetersPerUnit
                If largestArea < cellArea * 2 Then
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Debug.Print("UseZonalStats() Exception: " & ex.Message)
            Return True
        End Try
    End Function
End Class