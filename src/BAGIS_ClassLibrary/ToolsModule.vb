﻿Option Strict Off
Option Explicit On

Imports ESRI.ArcGIS.Geoprocessor
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataManagementTools
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.SpatialAnalyst
Imports System.Text
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Geoprocessing
Imports ESRI.ArcGIS.GeoprocessingUI
Imports ESRI.ArcGIS.SpatialAnalystTools
Imports ESRI.ArcGIS.ConversionTools
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.GeoDatabaseUI
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.AnalysisTools
Imports ESRI.ArcGIS.Geometry

Public Module ToolsModule

    'Arguments
    'zoneFilePath: The path name to the zone file
    'zoneFileName: The name of the zone file. This is the file that will have the new column appended
    'valueLayerFilePath: The full path and file name to the raster that is the input for zonal statistics
    'newFieldName: The name of the new field to be appended to zoneLayerName file
    'overwrite: If the column exists on zoneLayerName
    'statistic type: Statistic to calculate; optional; if not selected, mean is the default
    'Returns
    '-1 if input error
    '0 if another error
    '1 if successful
    Public Function BA_ZonalStats2Att(ByVal zoneFilePath As String, ByVal zoneFileName As String,
                                      ByVal tempFilePath As String, ByVal tempFileName As String,
                                      ByVal valueLayerFilePath As String, ByVal newFieldName As String,
                                      ByVal snapRasterPath As String, ByVal overwrite As Boolean,
                                      Optional ByVal statisticConst As StatisticsTypeString = StatisticsTypeString.MEAN) As Short
        ' Validate input file names
        Dim zoneLayerFilePath As String = zoneFilePath + "\" + zoneFileName
        Dim valueLayerWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(valueLayerFilePath)

        If Not BA_File_Exists(zoneLayerFilePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            If Not BA_File_Exists(zoneLayerFilePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                Return -1
            End If
        ElseIf Not BA_File_Exists(valueLayerFilePath, valueLayerWorkspaceType, esriDatasetType.esriDTRasterDataset) Then
            Return -1
        End If

        ' Is the value raster datatype compatible with the chosen statistic type?
        If RasterValidForStatisticType(valueLayerFilePath, statisticConst) = False Then
            Return -1
        End If

        'The output table that holds results of zonal calculation
        Dim pOutputTable As ITable = Nothing
        'The geoprocessor tool to use
        Dim tool As ESRI.ArcGIS.SpatialAnalystTools.ZonalStatisticsAsTable = New ESRI.ArcGIS.SpatialAnalystTools.ZonalStatisticsAsTable()
        'The zone dataset
        Dim pZoneDataset As IGeoDataset = Nothing

        ' Get a temp workspace.
        Dim pWSFactory As IWorkspaceFactory = New ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactory
        tempFilePath = BA_StandardizePathString(tempFilePath, False)
        Dim pWorkSpace As IFeatureWorkspace = pWSFactory.OpenFromFile(tempFilePath, 0)
        Dim pFeatureClass As IFeatureClass = Nothing
        Dim disposeDataSet As IDataset = Nothing

        Try
            'Open zoneFile to check layer type
            Dim layerIsRaster As Boolean
            pZoneDataset = BA_GetDataSet(zoneFilePath, zoneFileName)
            If TypeOf pZoneDataset Is IFeatureClass Then
                layerIsRaster = False
            ElseIf TypeOf pZoneDataset Is IRasterDataset Then
                layerIsRaster = True
            End If

            ' Configure and run ZonalStatistic tool
            'Dim srcField As String = BA_FIELD_ID 'set the default value
            Dim srcField As String = Nothing    'Should be using grid code instead of id
            If layerIsRaster Then
                tool.in_zone_data = zoneLayerFilePath
                tool.zone_field = BA_FIELD_VALUE
            Else
                tool.in_zone_data = zoneLayerFilePath

                'Get shape FeatureClass
                Dim zoneWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(zoneFilePath)
                If zoneWorkspaceType = WorkspaceType.Raster Then
                    pFeatureClass = BA_OpenFeatureClassFromFile(zoneFilePath, zoneFileName)
                ElseIf zoneWorkspaceType = WorkspaceType.Geodatabase Then
                    pFeatureClass = BA_OpenFeatureClassFromGDB(zoneFilePath, zoneFileName)
                End If
                If pFeatureClass Is Nothing Then
                    Return -1
                    Exit Function
                End If

                Dim idxField As Long = pFeatureClass.FindField(BA_FIELD_HRUID_NC)
                If idxField < 0 Then
                    'Accomodate both grid_code and gridcode as field names
                    Dim idxGridCode As Long = pFeatureClass.FindField(BA_FIELD_GRIDCODE)
                    If idxGridCode > -1 Then
                        srcField = BA_FIELD_GRIDCODE
                    Else
                        srcField = BA_FIELD_GRIDCODE_GDB
                    End If
                End If
                tool.zone_field = srcField
            End If
            tool.in_value_raster = valueLayerFilePath
            tool.out_table = tempFilePath & "\" & tempFileName
            tool.statistics_type = statisticConst.ToString
            Dim success As Short = Execute_Geoprocessing(tool, False, snapRasterPath)
            If success > 0 Then
                ' Load table results into memory
                pOutputTable = pWorkSpace.OpenTable(tempFileName)
                CheckColumnName(newFieldName, statisticConst, layerIsRaster)
                If Not layerIsRaster Then
                    UpdateVectorColumn(pOutputTable, pZoneDataset, BA_FieldNameFromTypeString(statisticConst), newFieldName, srcField, overwrite)
                Else
                    UpdateRasterColumn(pOutputTable, pZoneDataset, BA_FieldNameFromTypeString(statisticConst), newFieldName, overwrite)
                End If
                ' Delete temp table if it exists
                If pOutputTable IsNot Nothing Then
                    disposeDataSet = CType(pOutputTable, IDataset)
                    disposeDataSet.Delete()
                End If
                Return 1
            Else
                ' Delete temp table if it exists
                If pOutputTable IsNot Nothing Then
                    disposeDataSet = CType(pOutputTable, IDataset)
                    disposeDataSet.Delete()
                End If
                Return -1
            End If
        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message)
            ' Delete temp table if it exists
            If pOutputTable IsNot Nothing Then
                disposeDataSet = CType(pOutputTable, IDataset)
                disposeDataSet.Delete()
            End If
            Return -1
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pOutputTable)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pZoneDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tool)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWorkSpace)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWSFactory)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatureClass)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(disposeDataSet)
        End Try

    End Function

    'Returns an IGeoDataSet when provided with a file path and name. Contains logic 
    'to open either a feature class or a raster dataset. Knowledge of file format 
    'is not required to call this subroutine.
    Public Function BA_GetDataSet(ByVal zoneLayerPath As String, ByVal zoneLayerName As String) As IGeoDataset
        Dim pWSF As IWorkspaceFactory = Nothing
        Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(zoneLayerPath)
        If workspaceType = WorkspaceType.Raster Then
            pWSF = New ShapefileWorkspaceFactory
        ElseIf workspaceType = WorkspaceType.Geodatabase Then
            pWSF = New ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactory
        End If

        Dim pWorkspace As IWorkspace = pWSF.OpenFromFile(zoneLayerPath, 0)
        Dim pEnumDSName As IEnumDatasetName = pWorkspace.DatasetNames(esriDatasetType.esriDTFeatureClass)
        Dim returnDataset As IGeoDataset
        Dim pDSName As IDatasetName = pEnumDSName.Next
        Dim foundIt As Boolean = False
        While Not pDSName Is Nothing
            If pDSName.Name = zoneLayerName Then
                foundIt = True
                Exit While
            End If
            pDSName = pEnumDSName.Next
        End While
        If foundIt = True Then
            'zoneLayerFile must be a feature
            Dim pFeatWSpace As IFeatureWorkspace = CType(pWorkspace, IFeatureWorkspace)
            Dim pFeatClass As IFeatureClass = pFeatWSpace.OpenFeatureClass(zoneLayerName)
            returnDataset = CType(pFeatClass, IGeoDataset)
        Else
            If workspaceType = WorkspaceType.Raster Then
                pWSF = New RasterWorkspaceFactory
            ElseIf workspaceType = WorkspaceType.Geodatabase Then
                ' Do nothing; We already have a FileGDBWorkspaceFactory()
            End If
            Dim pRasterWSpace As IRasterWorkspaceEx = CType(pWSF.OpenFromFile(zoneLayerPath, 0), IRasterWorkspaceEx)
            Dim rasterDataset As IRasterDataset = pRasterWSpace.OpenRasterDataset(zoneLayerName)
            returnDataset = CType(rasterDataset, IGeoDataset)
        End If
        Return returnDataset
    End Function

    Private Function Execute_Geoprocessing(ByVal tool As IGPProcess, ByVal addOutputs As Boolean,
                                           ByVal snapRasterPath As String) As Short
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim pResult As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult = Nothing
        Try
            GP.OverwriteOutput = True
            GP.SetEnvironmentValue("cellsize", "MINOF")
            GP.AddOutputsToMap = addOutputs
            'BA_ListGPEnvironmentSettings(GP)
            If Not String.IsNullOrEmpty(snapRasterPath) Then
                GP.SetEnvironmentValue("snapRaster", snapRasterPath)
            ElseIf Not String.IsNullOrEmpty(GP.GetEnvironmentValue("snapRaster")) Then
                'Had to add this because ArcMap was sometimes caches the snapRaster path inappropriately
                'This clears it out if none is supplied
                GP.SetEnvironmentValue("snapRaster", Nothing)
            End If
            pResult = GP.Execute(tool, Nothing)
            Return 1
        Catch ex As Exception
            For c As Integer = 0 To GP.MessageCount - 1
                Debug.Print("GP error: " & GP.GetMessage(c))
            Next
            If GP.MessageCount > 0 Then
                MessageBox.Show("Geoprocessor error: " + GP.GetMessages(2))
            Else
                MessageBox.Show("Exception: " + ex.Message)
            End If
            Return -1
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tool)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pResult)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
        End Try
    End Function

    Private Function Execute_GeoprocessingWithMask(ByVal tool As IGPProcess, ByVal maskPath As String,
                                                   ByVal addOutputs As Boolean, ByVal snapRasterPath As String) As BA_ReturnCode
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim pResult As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult
        Try
            GP.OverwriteOutput = True
            GP.SetEnvironmentValue("cellsize", "MINOF")
            GP.SetEnvironmentValue("mask", maskPath)
            GP.AddOutputsToMap = addOutputs
            If Not String.IsNullOrEmpty(snapRasterPath) Then
                GP.SetEnvironmentValue("snapRaster", snapRasterPath)
            ElseIf Not String.IsNullOrEmpty(GP.GetEnvironmentValue("snapRaster")) Then
                'Had to add this because ArcMap was sometimes caches the snapRaster path inappropriately
                'This clears it out if none is supplied
                GP.SetEnvironmentValue("snapRaster", Nothing)
            End If
            pResult = GP.Execute(tool, Nothing)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            If GP.MessageCount > 0 Then
                MessageBox.Show("Geoprocessor error: " + GP.GetMessages(2))
            Else
                MessageBox.Show("Exception: " + ex.Message)
            End If
            Return BA_ReturnCode.UnknownError
        Finally
            GP = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Function

    Private Function Execute_GeoprocessingWithCellSize(ByVal tool As IGPProcess, ByVal addOutputs As Boolean,
                                                       ByVal snapRasterPath As String, ByVal cellSize As String) As Short
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim pResult As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult = Nothing
        Try
            GP.OverwriteOutput = True
            GP.SetEnvironmentValue("cellsize", cellSize)
            GP.AddOutputsToMap = addOutputs
            'BA_ListGPEnvironmentSettings(GP)
            If Not String.IsNullOrEmpty(snapRasterPath) Then
                GP.SetEnvironmentValue("snapRaster", snapRasterPath)
            ElseIf Not String.IsNullOrEmpty(GP.GetEnvironmentValue("snapRaster")) Then
                'Had to add this because ArcMap was sometimes caches the snapRaster path inappropriately
                'This clears it out if none is supplied
                GP.SetEnvironmentValue("snapRaster", Nothing)
            End If
            pResult = GP.Execute(tool, Nothing)
            Return 1
        Catch ex As Exception
            For c As Integer = 0 To GP.MessageCount - 1
                Debug.Print("GP error: " & GP.GetMessage(c))
            Next
            If GP.MessageCount > 0 Then
                MessageBox.Show("Geoprocessor error: " + GP.GetMessages(2))
            Else
                MessageBox.Show("Exception: " + ex.Message)
            End If
            Return -1
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tool)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pResult)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
        End Try
    End Function

    Private Function UpdateVectorColumn(ByVal sourceTable As ITable, ByRef targetDataSet As IGeoDataset,
                                           ByVal sourceColName As String, ByVal targetColName As String,
                                           ByVal srcField As String, ByVal overwrite As Boolean) As Short

        'get unique list of gridcode from the feature class
        Dim pFClass As IFeatureClass = CType(targetDataSet, IFeatureClass)
        Dim pData As IDataStatistics = New DataStatistics
        Dim pEnumVar As System.Collections.IEnumerator = Nothing
        Dim pDataCursor As ICursor = Nothing
        Dim pTargetQFilter As IQueryFilter = New QueryFilter
        Dim pTargetCursor As IFeatureCursor = Nothing
        Dim pSourceCursor As ICursor = Nothing
        Dim pSourceRow As IRow = Nothing
        Dim pFeature As IFeature
        Dim pField As IField = New Field
        Dim pFld As IFieldEdit2 = CType(pField, IFieldEdit2)

        Try
            pDataCursor = pFClass.Search(Nothing, False)
            pData.Field = srcField
            pData.Cursor = pDataCursor
            pEnumVar = pData.UniqueValues
            Dim pCount As Short = pData.UniqueValueCount

            If pCount <= 0 Then 'no features in feature class
                Return -1
            End If

            'get source field indexes
            Dim pSourceZoneIdx As Short = sourceTable.FindField(srcField)
            Dim pSourceValueIdx As Short = sourceTable.FindField(sourceColName)

            'get target field index
            Dim pTargetIndex As Short = pFClass.FindField(targetColName)
            'Add field if needed
            If pTargetIndex < 0 Then
                pFld.Name_2 = targetColName
                pFld.Type_2 = esriFieldType.esriFieldTypeDouble
                pFld.Length_2 = BA_BOUND_FIELD_WIDTH
                pFld.Required_2 = False
                ' Add field
                pFClass.AddField(pFld)
                pTargetIndex = pFClass.FindField(targetColName)
            ElseIf overwrite = False Then
                'Field already exists and we do not want to overwrite it
                Return 1
            End If

            'non-recycling cursor returns all records from source
            pSourceCursor = sourceTable.Search(Nothing, False)
            pSourceRow = pSourceCursor.NextRow
            Dim counter As Integer = 1

            Do While Not pSourceRow Is Nothing
                Dim newValue As Object = pSourceRow.Value(pSourceValueIdx)
                Dim newZone As Short = CInt(pSourceRow.Value(pSourceZoneIdx))
                pTargetQFilter.WhereClause = srcField & " = " & newZone
                pTargetCursor = pFClass.Update(pTargetQFilter, False)
                pFeature = pTargetCursor.NextFeature

                Do While Not pFeature Is Nothing
                    pFeature.Value(pTargetIndex) = newValue
                    pTargetCursor.UpdateFeature(pFeature)
                    pFeature = pTargetCursor.NextFeature
                Loop

                pTargetCursor = Nothing
                pFeature = Nothing
                pSourceRow = pSourceCursor.NextRow
                counter += 1
                If counter Mod 10 = 0 Then
                    Debug.Print("updated: " & counter)
                End If
            Loop
            Return 1
        Catch ex As Exception
            MessageBox.Show("BA_UpdateVectorColumn Exception: " & ex.Message)
            Return -1
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pData)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnumVar)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDataCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSourceRow)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTargetCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTargetQFilter)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSourceCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFClass)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pData)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pField)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFld)
        End Try

    End Function

    Private Function UpdateRasterColumn(ByVal sourceTable As ITable, ByRef targetDataSet As IGeoDataset,
                                           ByVal sourceColName As String, ByVal targetColName As String,
                                           ByVal overwrite As Boolean) As Short

        'open raster attribute table
        Dim pRDataset As IRasterDataset = CType(targetDataSet, IRasterDataset)
        Dim pBandCol As IRasterBandCollection = pRDataset
        Dim pRasterBand As IRasterBand = pBandCol.Item(0)
        Dim pTargetTable As ITable = pRasterBand.AttributeTable
        Dim pQFilter As IQueryFilter = New QueryFilter
        Dim pTargetQFilter As IQueryFilter = New QueryFilter
        Dim pField As IField = New Field
        Dim pFld As IFieldEdit2 = CType(pField, IFieldEdit2)
        Dim pSourceCursor As ICursor = Nothing
        Dim pSourceRow As IRow = Nothing
        Dim pTargetCursor As ICursor = Nothing
        Dim pRow As IRow = Nothing

        Try
            'get source field indexes
            Dim pSourceZoneIdx As Short = sourceTable.FindField(BA_FIELD_VALUE)
            Dim pSourceValueIdx As Short = sourceTable.FindField(sourceColName)

            'add targetColName field
            'check if field exist
            Dim pTargetIdx As Short = pTargetTable.FindField(targetColName)

            ' Define field type
            If pTargetIdx < 0 Then 'add field
                pFld.Name_2 = targetColName
                pFld.Type_2 = esriFieldType.esriFieldTypeDouble
                pFld.Length_2 = BA_BOUND_FIELD_WIDTH
                pFld.Required_2 = False
                ' Add field
                pTargetTable.AddField(pFld)
                pTargetIdx = pTargetTable.FindField(targetColName)
            ElseIf overwrite = False Then
                'Field already exists and we do not want to overwrite it
                Return 1
            End If

            'non-recycling cursor returns all records from source
            pSourceCursor = sourceTable.Search(Nothing, False)
            pSourceRow = pSourceCursor.NextRow

            Do While Not pSourceRow Is Nothing
                Dim newValue As Object = pSourceRow.Value(pSourceValueIdx)
                Dim newZone As Short = CInt(pSourceRow.Value(pSourceZoneIdx))
                pTargetQFilter.WhereClause = BA_FIELD_VALUE & " = " & newZone
                pTargetCursor = pTargetTable.Update(pTargetQFilter, False)
                pRow = pTargetCursor.NextRow

                Do While Not pRow Is Nothing
                    pRow.Value(pTargetIdx) = newValue
                    pTargetCursor.UpdateRow(pRow)
                    pRow = pTargetCursor.NextRow
                Loop

                pTargetCursor = Nothing
                pRow = Nothing
                pSourceRow = pSourceCursor.NextRow
            Loop
            Return 1

        Catch ex As Exception
            MessageBox.Show("BA_UpdateRasterColumn exception: " & ex.Message)
            Return -1
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTargetQFilter)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pQFilter)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTargetTable)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pBandCol)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSourceRow)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSourceCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTargetCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRow)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pField)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFld)
        End Try

    End Function


    Private Sub CheckColumnName(ByRef colName As String, ByVal statisticType As StatisticsTypeString,
                                ByVal layerIsRaster As Boolean)
        If String.IsNullOrEmpty(colName) Then
            colName = BA_FieldNameFromTypeString(statisticType)
        End If
        'trim column name if needed; maximum for shapefile or grid
        If layerIsRaster Then
            colName = Left(colName, BA_GRID_FIELD_NAME_MAX_LENGTH)
        Else
            colName = Left(colName, BA_SHAPEFILE_FIELD_NAME_MAX_LENGTH)
        End If
    End Sub

    'Arguments:
    'layerList: List of full pathnames for all rasters to be combined
    'outputLayerPath: Path where output layer is to be written
    'outputLayerName: Name of output layer file
    'overwrite: Overwrite output layer if it exits
    Public Function BA_ZoneOverlay(ByVal maskFilePath As String, ByVal layerList As List(Of String),
                                   ByVal outputLayerPath As String, ByVal outputLayerName As String,
                                   ByVal addOutputs As Boolean, ByVal overwrite As Boolean,
                                   ByVal snapRasterPath As String, ByVal workspaceType As WorkspaceType) As BA_ReturnCode
        Dim outputFilePath As String = outputLayerPath & "\" & outputLayerName
        If BA_File_ExistsIDEUtil(outputFilePath) AndAlso overwrite = False Then
            Return BA_ReturnCode.NotSupportedOperation
        End If

        Dim tool As ESRI.ArcGIS.SpatialAnalystTools.Combine = New ESRI.ArcGIS.SpatialAnalystTools.Combine
        Dim sb As StringBuilder = New StringBuilder()
        Try
            ' format of in_rasters is String like this: "filepath1; filepath2; filepath3"
            If layerList.Count > 0 Then
                For Each layerName In layerList
                    If workspaceType = WorkspaceType.Raster Then
                        If BA_IsIntegerRaster(layerName) Then
                            sb.Append(layerName)
                            sb.Append("; ")
                        End If
                    ElseIf workspaceType = WorkspaceType.Geodatabase Then
                        If BA_IsIntegerRasterGDB(layerName) Then
                            sb.Append(layerName)
                            sb.Append("; ")
                        End If
                    Else
                        Return BA_ReturnCode.NotSupportedOperation
                    End If
                Next
            Else
                Return BA_ReturnCode.NotSupportedOperation
            End If

            ' trim off closing separator
            sb.Remove(sb.Length - 2, 2)
            tool.in_rasters = sb.ToString
            tool.out_raster = outputFilePath
            Return Execute_GeoprocessingWithMask(tool, maskFilePath, addOutputs, snapRasterPath)
        Catch ex As Exception
            MessageBox.Show("BA_ZoneOverlay Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            tool = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try

    End Function

    'Opens a raster and checks the properties to see if it is an integer raster
    Public Function BA_IsIntegerRaster(ByVal fullLayerFilePath As String) As Boolean
        Dim filePath As String = "blank"
        Dim fileName As String = BA_GetBareName(fullLayerFilePath, filePath)
        Dim pGeoDS As IGeoDataset = Nothing
        Dim pRasterBandColl As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pRasterP As IRasterProps = Nothing
        Try
            pGeoDS = BA_OpenRasterFromFile(filePath, fileName)
            If pGeoDS Is Nothing Then
                Return False
            End If
            pRasterBandColl = CType(pGeoDS, IRasterBandCollection)
            pRasterBand = pRasterBandColl.Item(0)
            pRasterP = pRasterBand
            Return pRasterP.IsInteger
        Catch ex As Exception
            MessageBox.Show("BA_IsIntegerRaster Exception: " + ex.Message)
            Return False
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterP)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBandColl)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGeoDS)
        End Try

    End Function

    Private Function RasterValidForStatisticType(ByVal fullLayerFilePath As String,
                                                    ByVal statisticConst As StatisticsTypeString) As Boolean
        Dim layerIsInteger As Boolean = BA_IsIntegerRasterGDB(fullLayerFilePath)
        'If layer is integer all statistics are supported
        If layerIsInteger = True Then
            Return True
        End If
        'Otherwise, the following statistics are not
        Select Case statisticConst
            Case StatisticsTypeString.MAJORITY
                Return False
            Case StatisticsTypeString.MINORITY
                Return False
            Case StatisticsTypeString.MEDIAN
                Return False
            Case StatisticsTypeString.VARIETY
                Return False
            Case Else
                Return True
        End Select
    End Function

    Private WithEvents m_toolboxEvents As IGPToolbox

    'Sample of invoking a Geoprocessing tool using the GPToolCommandHelper. 
    'This example uses the Clip tool. This subroutine is not currently used by BAGIS.
    Public Sub BA_InvokeTool(ByVal application As ESRI.ArcGIS.Framework.IApplication)
        'attempting to open a spatial statistics dialog that is normally run from ArcToolBox
        'Create a toolbox workspace factory
        Dim pToolboxWorkspaceFactory As IWorkspaceFactory = New ToolboxWorkspaceFactory

        'Open a toolbox workspace
        Dim pToolboxWorkspace As IToolboxWorkspace
        pToolboxWorkspace = pToolboxWorkspaceFactory.OpenFromFile("C:\Program Files\ArcGIS\Desktop10.0\ArcToolbox\Toolboxes", 0)

        'Open a toolbox by Name
        Dim pGPToolbox As IGPToolbox = pToolboxWorkspace.OpenToolbox("Analysis Tools")
        m_toolboxEvents = pGPToolbox

        'tool
        Dim pGPTool As IGPTool = pGPToolbox.OpenTool("Clip")

        Try
            ' Retrieve array of parameters from tool and populate it
            Dim pParams As ESRI.ArcGIS.esriSystem.IArray = pGPTool.ParameterInfo
            'Input features
            Dim pParameter As IGPParameter = pParams.Element(0)
            Dim pParamEdit As IGPParameterEdit = pParameter
            Dim pDataType As IGPDataType = pParameter.DataType
            Dim sValue As String = "C:\Docs\Lesley\NRCS_Code_Migration\BA_Class_Library_v3\BA_Class_Library_v3\test\data\snotel_sites.shp"
            pParamEdit.Value = pDataType.CreateValue(sValue)
            'Clip features
            pParameter = pParams.Element(1)
            pParamEdit = pParameter
            pDataType = pParameter.DataType
            sValue = "C:\Docs\Lesley\NRCS_Code_Migration\BA_Class_Library_v3\BA_Class_Library_v3\test\data\aoi_v.shp"
            pParamEdit.Value = pDataType.CreateValue(sValue)
            'Output features
            pParameter = pParams.Element(2)
            pParamEdit = pParameter
            pDataType = pParameter.DataType
            sValue = "C:\Docs\Lesley\NRCS_Code_Migration\BA_Class_Library_v3\BA_Class_Library_v3\test\data\scratch\blah.shp"
            pParamEdit.Value = pDataType.CreateValue(sValue)
            'XY Tolerance
            pParameter = pParams.Element(3)
            pParamEdit = pParameter
            pDataType = pParameter.DataType
            Dim iValue As Short = 1
            pParamEdit.Value = pDataType.CreateValue(iValue)

            Dim msgs As IGPMessages = New GPMessages
            Dim pToolHelper As IGPToolCommandHelper2 = New GPToolCommandHelper
            pToolHelper.SetTool(pGPTool)
            Dim success As Boolean = False
            pToolHelper.InvokeModal(application.hWnd, pParams, success, msgs)
            pParams = pGPTool.ParameterInfo
            pParameter = pParams.Element(3)
            Dim blah As String = pParameter.Value.GetAsText
            If (msgs IsNot Nothing AndAlso msgs.Count > 0) Then
                ProcessGPMessages(msgs)
            End If

        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message)
        Finally
            'pArcToolbox = Nothing
            'pExtension = Nothing
            'pUID = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try


    End Sub

    Private Sub ProcessGPMessages(ByVal gpMsgs As IGPMessages)
        Dim ir As ESRI.ArcGIS.esriSystem.IArray = gpMsgs.Messages
        Dim ic As Integer = ir.Count
        Dim sb As StringBuilder = New StringBuilder
        For i As Integer = 0 To ic - 1
            'Console.WriteLine(gpMsgs.GetMessage(i).Description)
            sb.Append(gpMsgs.GetMessage(i).Description)
            sb.Append(vbCrLf)
        Next i
        MessageBox.Show(sb.ToString)
    End Sub

    'Copies an ITable to a multi-dimensional array. The list of fields to copy is 
    'passed in in a String array
    Public Function BA_CopyTableToArray(ByVal fieldNames As String(), ByVal table As ITable) As Long(,)
        Dim pCursor As ICursor = Nothing
        Dim pRow As IRow = Nothing
        Try
            Dim arrayLength As Integer = table.RowCount(Nothing) - 1
            Dim newArray(arrayLength, fieldNames.GetUpperBound(0)) As Long
            Dim index(fieldNames.GetUpperBound(0)) As Short
            For i As Short = 0 To index.GetUpperBound(0) Step 1
                index(i) = table.FindField(fieldNames(i))
            Next
            pCursor = table.Search(Nothing, False)

            For j As Short = 0 To arrayLength Step 1
                pRow = pCursor.NextRow
                For k As Short = 0 To index.GetUpperBound(0) Step 1
                    newArray(j, k) = pRow.Value(index(k))
                Next
            Next
            Return newArray
        Catch ex As Exception
            MessageBox.Show("BA_CopyTableToArray Exception: " & ex.Message)
            Return Nothing
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRow)
        End Try

    End Function

    ' Informational function to dump the GP environment settings to the console.
    ' This function is not currently being used by BAGIS
    Public Sub BA_ListGPEnvironmentSettings(ByVal GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor)
        ' list all the Environments, hold the return of the method in an Enumeration
        Dim gpEnumEnv As IGpEnumList = GP.ListEnvironments("*")
        Dim strEnv As String = gpEnumEnv.Next
        While strEnv.Length > 0
            Debug.WriteLine(strEnv)
            strEnv = gpEnumEnv.Next
        End While
    End Sub

    ' Use Resample geoprocessing tool to resample raster to specified cellSize
    ' Uses default nearest neighbor resampling algorithm
    Public Function BA_Resample_Raster(ByVal inputRaster As String, ByVal outputRaster As String,
                                       ByVal cellSize As Double, ByVal snapRasterPath As String,
                                       ByVal resamplingType As String) As BA_ReturnCode
        Dim tool As New Resample
        Dim retCode As BA_ReturnCode = BA_ReturnCode.UnknownError
        tool.in_raster = inputRaster
        tool.out_raster = outputRaster
        tool.cell_size = cellSize
        If Not String.IsNullOrEmpty(resamplingType) Then
            tool.resampling_type = resamplingType
        End If
        Try
            Dim success As Short = Execute_Geoprocessing(tool, False, snapRasterPath)
            If success > 0 Then retCode = BA_ReturnCode.Success
            Return retCode
        Catch ex As Exception
            MessageBox.Show("BA_Resample_Raster Exception: " & ex.Message)
            Return retCode
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tool)
        End Try

    End Function

    ' Use Geoprocessor to delete a workspace (folder)
    Public Function BA_Remove_FolderGP(ByVal workspacePath As String) As BA_ReturnCode

        Dim tool As Delete = New Delete
        Try
            tool.in_data = workspacePath
            tool.data_type = "Folder"
            'Don't pass snap raster here because it's not a spatial analyst tool
            Execute_Geoprocessing(tool, False, Nothing)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MsgBox("BA_Remove_Workspace Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tool)
        End Try

    End Function

    ' Use Geoprocessor to delete a workspace (folder)
    Public Function BA_Remove_WorkspaceGP(ByVal workspacePath As String) As BA_ReturnCode

        Dim tool As Delete = New Delete
        Try
            tool.in_data = workspacePath
            tool.data_type = "Workspace"
            'Don't pass snap raster here because it's not a spatial analyst tool
            Execute_Geoprocessing(tool, False, Nothing)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MsgBox("BA_Remove_WorkspaceGP Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tool)
        End Try

    End Function

    'Uses the Geoprocessor Reclassify tool to reclassify a raster from an array of 
    'ReclassItem(). 
    Public Function BA_ReclassifyRasterFromTable(ByVal inputFolderPath As String, ByVal reclassField As String,
                                                 ByVal reclassItems As ReclassItem(), ByVal outputFolderPath As String,
                                                 ByVal actionType As ActionType, ByVal snapRasterPath As String) As BA_ReturnCode
        Dim tool As Reclassify = New Reclassify
        Try
            tool.in_raster = inputFolderPath
            If actionType = ActionType.ReclDisc Then
                tool.remap = BA_CreateDiscreteReclass(reclassItems)
            ElseIf actionType = ActionType.ReclCont Then
                tool.remap = BA_CreateContinuousReclass(reclassItems)
            End If
            tool.reclass_field = reclassField
            tool.out_raster = outputFolderPath
            Execute_Geoprocessing(tool, False, snapRasterPath)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_ReclassifyRasterFromTable Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tool)
        End Try
    End Function

    Public Function BA_ReclassifyRasterFromTableWithNoData(ByVal inputFolderPath As String, ByVal reclassField As String,
                                                           ByVal reclassItems As ReclassItem(), ByVal outputFolderPath As String,
                                                           ByVal snapRasterPath As String) As BA_ReturnCode
        Dim tool As Reclassify = New Reclassify
        Try
            Dim sb As StringBuilder = New StringBuilder
            ' Loop through reclassItems and add to string
            For Each item In reclassItems
                sb.Append((item.FromValue - 0.01))
                sb.Append(" ")
                sb.Append((item.ToValue + 0.01))
                sb.Append(" ")
                If item.OutputValue = -9999 Then
                    sb.Append("NoData")
                Else
                    sb.Append(item.OutputValue)
                End If
                sb.Append(";")
            Next
            ' Remove trailing semi-colon
            sb = sb.Remove(sb.Length - 1, 1)

            tool.in_raster = inputFolderPath
            tool.remap = sb.ToString
            tool.reclass_field = reclassField
            tool.out_raster = outputFolderPath
            Execute_Geoprocessing(tool, False, snapRasterPath)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_ReclassifyRasterFromTable Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tool)
        End Try
    End Function

    'Uses the Geoprocessor Reclassify tool to reclassify a raster from an array of 
    'ReclassTextItem().
    Public Function BA_ReclassifyRasterFromReclassTextItems(ByVal inputFolderPath As String, ByVal reclassField As String,
                                                            ByVal reclassTextItems As ReclassTextItem(), ByVal outputFolderPath As String,
                                                            ByVal snapRasterPath As String) As BA_ReturnCode
        Dim tool As Reclassify = New Reclassify
        Try
            tool.in_raster = inputFolderPath
            tool.remap = BA_CreateDiscreteTextReclass(reclassTextItems)
            tool.reclass_field = reclassField
            tool.out_raster = outputFolderPath
            Execute_Geoprocessing(tool, False, snapRasterPath)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_ReclassifyRasterFromTable Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tool)
        End Try
    End Function

    ' Creates a reclass string to be used as input for the Geoprocessor Reclassify tool
    ' Sample output: 10.99 11.01 41;30.99 31.01 2;40.99 41.01 1;41.99 42.01 1; 
    '                42.99 43.01 1;51.99 52.01 5;70.99 71.01 2;89.99 90.01 4
    Public Function BA_CreateDiscreteReclass(ByVal reclassItems As ReclassItem()) As String
        Dim sb As StringBuilder = New StringBuilder
        ' Loop through reclassItems and add to string
        For Each item In reclassItems
            sb.Append((item.FromValue - 0.01))
            sb.Append(" ")
            sb.Append((item.ToValue + 0.01))
            sb.Append(" ")
            sb.Append(item.OutputValue)
            sb.Append(";")
        Next
        ' Remove trailing semi-colon
        sb = sb.Remove(sb.Length - 1, 1)
        'Debug.Print("Remap --->" & sb.ToString)
        Return sb.ToString
    End Function

    ' Creates a reclass string to be used as input for the Geoprocessor Reclassify tool
    ' Sample output: ELK CABIN 1;SANTA FE 2 
    Public Function BA_CreateDiscreteTextReclass(ByVal reclassTextItems As ReclassTextItem()) As String
        Dim sb As StringBuilder = New StringBuilder
        ' Loop through reclassItems and add to string
        For Each item In reclassTextItems
            sb.Append(item.FromValue)
            sb.Append(" ")
            'sb.Append(item.FromValue)
            'sb.Append(" ")
            sb.Append(item.OutputValue)
            sb.Append(";")
        Next
        ' Remove trailing semi-colon
        sb = sb.Remove(sb.Length - 1, 1)
        'Debug.Print("Remap --->" & sb.ToString)
        Return sb.ToString
    End Function


    ' Creates a reclass string to be used as input for the Geoprocessor Reclassify tool
    ' Sample output: 10.99 11.01 41;30.99 31.01 2;40.99 41.01 1;41.99 42.01 1; 
    '                42.99 43.01 1;51.99 52.01 5;70.99 71.01 2;89.99 90.01 4
    Public Function BA_CreateContinuousReclass(ByVal reclassItems As ReclassItem()) As String
        Dim sb As StringBuilder = New StringBuilder
        ' Loop through reclassItems and add to string
        For Each item In reclassItems
            sb.Append((item.FromValue))
            sb.Append(" ")
            sb.Append((item.ToValue))
            sb.Append(" ")
            sb.Append(item.OutputValue)
            sb.Append(";")
        Next
        ' Remove trailing semi-colon
        sb = sb.Remove(sb.Length - 1, 1)
        'Debug.Print("Remap --->" & sb.ToString)
        Return sb.ToString
    End Function

    ' Use FeatureToRaster GP tool to convert input feature class to raster
    Public Function BA_Feature2RasterGP(ByVal inFeaturesPath As String, ByVal outRasterPath As String,
                                        ByVal inField As String, ByVal cellSize As Double,
                                        ByVal snapRasterPath As String) As BA_ReturnCode
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim pResult As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult = Nothing
        Dim tool As FeatureToRaster = New FeatureToRaster
        Try
            tool.cell_size = cellSize
            tool.in_features = inFeaturesPath
            tool.field = inField
            tool.out_raster = outRasterPath
            GP.OverwriteOutput = True
            GP.AddOutputsToMap = False
            If Not String.IsNullOrEmpty(snapRasterPath) Then
                GP.SetEnvironmentValue("snapRaster", snapRasterPath)
            End If
            pResult = GP.Execute(tool, Nothing)

            ' Build the raster attribute table; Don't overwrite if it already exists; Combine doesn't create attribute table if
            ' large number of polygons
            Dim success As BA_ReturnCode = BA_BuildRasterAttributeTable(outRasterPath, False)
            Return success
        Catch ex As Exception
            For c As Integer = 0 To GP.MessageCount - 1
                Debug.Print("GP error: " & GP.GetMessage(c))
            Next
            If GP.MessageCount > 0 Then
                MessageBox.Show(" BA_Feature2RasterGP Geoprocessor error: " + GP.GetMessages(2))
            Else
                MessageBox.Show("BA_Feature2RasterGP Exception: " + ex.Message)
            End If
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tool)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pResult)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
        End Try
    End Function

    ' Calculate focal statistics with GP
    Public Function BA_FocalStatistics_GP(ByVal inRaster As String, ByVal outRasterDataset As String,
                                          ByVal maskDataset As String, ByVal neighborhood As String,
                                          ByVal statisticsType As String, ByVal snapRasterPath As String) As BA_ReturnCode
        Dim tool As FocalStatistics = New FocalStatistics
        tool.in_raster = inRaster
        tool.out_raster = outRasterDataset
        tool.neighborhood = neighborhood
        tool.statistics_type = statisticsType
        If Not String.IsNullOrEmpty(maskDataset) Then
            Return Execute_GeoprocessingWithMask(tool, maskDataset, False, snapRasterPath)
        Else
            If Execute_Geoprocessing(tool, False, Nothing) = 1 Then
                Return BA_ReturnCode.Success
            Else
                Return BA_ReturnCode.UnknownError
            End If
        End If
    End Function

    'Delete data from disk
    Public Function BA_DeleteLayer_GP(ByVal inData As String) As BA_ReturnCode
        Dim tool As Delete = New Delete
        tool.in_data = inData
        'No snapRaster path as this isn't a spatial analyst tool
        If Execute_Geoprocessing(tool, False, Nothing) = 1 Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    Public Function BA_Raster2Polygon_GP(ByVal inRaster As String, ByRef outFeatures As String, ByVal snapRasterPath As String) As BA_ReturnCode
        Dim tool As RasterToPolygon = New RasterToPolygon
        tool.in_raster = inRaster
        tool.out_polygon_features = outFeatures
        tool.simplify = False
        If Execute_Geoprocessing(tool, False, snapRasterPath) = 1 Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    ' Dissolve a feature class on a specified field
    Public Function BA_Dissolve(ByVal inFeatures As String, ByVal inField As String, ByVal outFeatures As String) As BA_ReturnCode
        Dim tool As Dissolve = New Dissolve
        tool.in_features = inFeatures
        tool.multi_part = "true"
        tool.out_feature_class = outFeatures
        tool.dissolve_field = inField
        'No snapRasterPath because not a spatial analyst tool
        If Execute_Geoprocessing(tool, False, Nothing) = 1 Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    'Copy a feature class from one workspace to another
    Public Function BA_CopyFeatures(ByVal inFeatures As String, ByVal outFeatureClass As String) As BA_ReturnCode
        Dim tool As CopyFeatures = New CopyFeatures
        tool.in_features = inFeatures
        tool.out_feature_class = outFeatureClass
        'No snap raster path, not a spatial analyst tool
        If Execute_Geoprocessing(tool, False, Nothing) = 1 Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    Public Function BA_UpgradeMetadata(ByVal inputFolder As String, ByVal inputFile As String) As BA_ReturnCode
        Dim tool As UpgradeMetadata = New UpgradeMetadata
        Try
            Dim fullFilePath As String = inputFolder & inputFile
            tool.Source_Metadata = fullFilePath
            tool.Upgrade_Type = "FGDC_TO_ARCGIS"
            'No snap raster path, not a spatial analyst tool
            If Execute_Geoprocessing(tool, False, Nothing) = 1 Then
                Return BA_ReturnCode.Success
            Else
                Return BA_ReturnCode.UnknownError
            End If
        Catch ex As Exception
            Debug.Print(" BA_UpgradeMetadata: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            tool = Nothing
        End Try
    End Function

    'Use slope geoprocessing tool to create a slope layer
    Public Function BA_Calculate_Slope(ByVal demPath As String, ByVal outputPath As String,
                                       ByVal slopeUnit As SlopeUnit, ByVal snapRasterPath As String) As BA_ReturnCode
        Dim tool As Slope = New Slope
        Try
            tool.in_raster = demPath
            tool.out_raster = outputPath
            Select Case slopeUnit
                Case SlopeUnit.PctSlope
                    tool.output_measurement = "PERCENT_RISE"
                Case SlopeUnit.Degree
                    tool.output_measurement = "DEGREE"
            End Select

            Dim retVal As Short = Execute_Geoprocessing(tool, False, snapRasterPath)
            If retVal = 1 Then
                Return BA_ReturnCode.Success
            Else
                Return BA_ReturnCode.UnknownError
            End If
        Catch ex As Exception
            Debug.Print("BA_Calculate_Slope: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            tool = Nothing
        End Try

    End Function

    '13-JUL-2011 This function works but commenting out because I don't plan to use
    'raster catalog functionality
    ' Create a raster catalog when provided with a path, name, and coordinate system
    'Public Function BA_CreateRasterCatalog_GP(ByVal cPath As String, ByVal cName As String, _
    '                                          ByVal coordSys As Object) As BA_ReturnCode
    '    Dim tool As New CreateRasterCatalog()

    '    Try
    '        'Set parameters 
    '        tool.out_path = cPath
    '        tool.out_name = cName
    '        tool.raster_spatial_reference = coordSys
    '        tool.spatial_reference = coordSys
    '        Execute_Geoprocessing(tool, False)
    '        Return BA_ReturnCode.Success
    '    Catch ex As Exception
    '        MessageBox.Show("BA_CreateRasterCatalog_GP Exception: " + ex.Message)
    '        Return BA_ReturnCode.UnknownError
    '    Finally
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tool)
    '    End Try

    'End Function

    Public Function BA_ZonalStatisticsAsTable(ByVal zoneFilePath As String, ByVal zoneFileName As String, _
                                              ByVal zoneField As String, ByVal valueLayerFilePath As String, _
                                              ByVal tempFilePath As String, ByVal tempFileName As String, _
                                              ByVal snapRasterPath As String, _
                                              Optional ByVal statisticConst As StatisticsTypeString = StatisticsTypeString.MEAN) As BA_ReturnCode
        ' Validate input file names
        Dim zoneLayerFilePath As String = zoneFilePath + "\" + zoneFileName
        Dim valueLayerWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(valueLayerFilePath)

        If Not BA_File_Exists(zoneLayerFilePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            If Not BA_File_Exists(zoneLayerFilePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                Return -1
            End If
        ElseIf Not BA_File_Exists(valueLayerFilePath, valueLayerWorkspaceType, esriDatasetType.esriDTRasterDataset) Then
            Return -1
        End If

        'Delete table if it already exists
        Dim pTable As ITable = Nothing
        pTable = BA_OpenTableFromGDB(tempFilePath, tempFileName)
        If pTable IsNot Nothing Then
            BA_Remove_TableFromGDB(tempFilePath, tempFileName)
        End If

        'The geoprocessor tool to use
        Dim tool As ESRI.ArcGIS.SpatialAnalystTools.ZonalStatisticsAsTable = New ESRI.ArcGIS.SpatialAnalystTools.ZonalStatisticsAsTable()
        'The zone dataset
        Dim pZoneDataset As IGeoDataset = Nothing
        Dim success As Short = -1

        Try
            ' Configure and run ZonalStatistic tool
            tool.in_zone_data = zoneLayerFilePath
            tool.zone_field = zoneField
            tool.in_value_raster = valueLayerFilePath
            tool.out_table = tempFilePath & "\" & tempFileName
            tool.statistics_type = statisticConst.ToString
            success = Execute_Geoprocessing(tool, False, snapRasterPath)
            If success = 1 Then
                Return BA_ReturnCode.Success
            Else
                Return BA_ReturnCode.UnknownError
            End If
        Catch ex As Exception
            Debug.Print("BA_ZonalStatisticsAsTable Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            tool = Nothing
            pTable = Nothing
        End Try

    End Function

    'The JoinField tool joins a table to a feature class and adds the table fields to the input feature class
    'If you don't want to alter the original feature class, you need to create a copy before using this
    Public Function BA_JoinField(ByVal inputFeatureClassPath As String, ByVal inJoinField As String, _
                                 ByVal joinTablePath As String, ByVal tableJoinField As String, ByVal fields As String) As BA_ReturnCode
        Dim tool As JoinField = New JoinField
        Dim success As Short = -1
        Try
            tool.in_data = inputFeatureClassPath
            tool.in_field = inJoinField
            tool.join_table = joinTablePath
            tool.join_field = tableJoinField
            If Not String.IsNullOrEmpty(fields) Then
                tool.fields = fields
            End If
            'No snaprasterpath for this tool
            success = Execute_Geoprocessing(tool, False, Nothing)
            If success = 1 Then
                Return BA_ReturnCode.Success
            Else
                Return BA_ReturnCode.UnknownError
            End If
        Catch ex As Exception
            Debug.Print("BA_JoinField Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            tool = Nothing
        End Try
    End Function


    Public Function BA_ExtractByMask(ByVal maskFilePath As String, ByVal inputRasterPath As String, _
                                     ByVal snapRasterPath As String, ByVal outputRasterPath As String) As BA_ReturnCode
        Dim tool As ExtractByMask = New ExtractByMask
        Try
            tool.in_mask_data = maskFilePath
            tool.in_raster = inputRasterPath
            tool.out_raster = outputRasterPath
            Dim retVal As Integer = Execute_Geoprocessing(tool, False, snapRasterPath)
            If retVal = 1 Then
                Return BA_ReturnCode.Success
            Else
                Return BA_ReturnCode.UnknownError
            End If
        Catch ex As Exception
            Debug.Print("BA_ExtractByMask Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            tool = Nothing
        End Try
    End Function

    Public Function BA_ExtractByAttributes(ByVal inputRasterPath As String, _
                                           ByVal snapRasterPath As String, ByVal outputRasterPath As String, _
                                           ByVal whereClause As String) As BA_ReturnCode
        Dim tool As ExtractByAttributes = New ExtractByAttributes
        Try
            tool.where_clause = whereClause
            tool.in_raster = inputRasterPath
            tool.out_raster = outputRasterPath
            Dim retVal As Integer = Execute_Geoprocessing(tool, False, snapRasterPath)
            If retVal = 1 Then
                Return BA_ReturnCode.Success
            Else
                Return BA_ReturnCode.UnknownError
            End If
        Catch ex As Exception
            Debug.Print("BA_ExtractByAttributes Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            tool = Nothing
        End Try
    End Function

    'ignoreNoData added June 6, 2017
    Public Function BA_GetCellStatistics(ByVal inRasters As String, ByVal snapRasterPath As String, _
                                         ByVal statisticsType As String, ByVal outputRasterPath As String, _
                                         ByVal ignoreNoData As String)
        Dim tool As CellStatistics = New CellStatistics
        Try
            tool.in_rasters_or_constants = inRasters
            tool.out_raster = outputRasterPath
            tool.statistics_type = statisticsType
            If Not String.IsNullOrEmpty(ignoreNoData) Then
                tool.ignore_nodata = ignoreNoData
            End If
            Dim retVal As Integer = Execute_Geoprocessing(tool, False, snapRasterPath)
            If retVal = 1 Then
                Return BA_ReturnCode.Success
            Else
                Return BA_ReturnCode.UnknownError
            End If
            Return BA_ReturnCode.UnknownError
        Catch ex As Exception
            Debug.Print("BA_GetCellStatistics Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            tool = Nothing
        End Try
    End Function

    Public Function BA_SetValueToNull(ByVal inputPath As String, ByVal outputPath As String, ByVal oldValue As Integer) As BA_ReturnCode
        Dim toolSetNull As SetNull = New SetNull

        Try
            toolSetNull.in_conditional_raster = inputPath
            toolSetNull.in_false_raster_or_constant = inputPath
            toolSetNull.where_clause = "VALUE = " & oldValue
            toolSetNull.out_raster = outputPath
            'snapRasterPath not required
            Dim retVal As Integer = Execute_Geoprocessing(toolSetNull, False, Nothing)
            If retVal = 1 Then
                Return BA_ReturnCode.Success
            Else
                Return BA_ReturnCode.UnknownError
            End If
            Return BA_ReturnCode.UnknownError
        Catch ex As Exception
            Debug.Print("BA_SetValueToNull Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            toolSetNull = Nothing
        End Try
    End Function

    Public Function BA_XSLTransformToHtml(ByVal sourceFile As String, ByVal xsltTemplate As String, ByVal outputFile As String) As BA_ReturnCode
        Dim tool As XSLTransform = New XSLTransform
        Try
            tool.source = sourceFile
            tool.xslt = xsltTemplate
            tool.output = outputFile
            'snapRasterPath not required
            Dim retVal As Integer = Execute_Geoprocessing(tool, False, Nothing)
            If retVal = 1 Then
                Return BA_ReturnCode.Success
            Else
                Return BA_ReturnCode.UnknownError
            End If
            Return BA_ReturnCode.UnknownError
        Catch ex As Exception
            Debug.Print("BA_XSLTransformToHtml Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            tool = Nothing
        End Try
    End Function

    'Uses the Geoprocessor Reclassify tool to reclassify a raster from a formatted reclass String
    ' Sample input: 10.99 11.01 41;30.99 31.01 2;40.99 41.01 1;41.99 42.01 1; 
    '                42.99 43.01 1;51.99 52.01 5;70.99 71.01 2;89.99 90.01 4
    Public Function BA_ReclassifyRasterFromString(ByVal inputFolderPath As String, ByVal reclassField As String,
                                                  ByVal reclassString As String, ByVal outputFolderPath As String,
                                                  ByVal snapRasterPath As String) As BA_ReturnCode
        Dim tool As Reclassify = New Reclassify
        Try
            tool.in_raster = inputFolderPath
            tool.reclass_field = reclassField
            tool.out_raster = outputFolderPath
            tool.remap = reclassString
            Execute_Geoprocessing(tool, False, snapRasterPath)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_ReclassifyRasterFromString Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tool)
        End Try
    End Function

    'Uses the Geoprocessor Reclassify tool to reclassify a raster from a formatted reclass String
    ' Sample input: 10.99 11.01 41;30.99 31.01 2;40.99 41.01 1;41.99 42.01 1; 
    '                42.99 43.01 1;51.99 52.01 5;70.99 71.01 2;89.99 90.01 4
    Public Function BA_ReclassifyRasterFromStringWithMask(ByVal inputFolderPath As String, ByVal reclassField As String,
                                                  ByVal reclassString As String, ByVal outputFolderPath As String,
                                                  ByVal snapRasterPath As String, ByVal maskPath As String) As BA_ReturnCode
        Dim tool As Reclassify = New Reclassify
        Try
            tool.in_raster = inputFolderPath
            tool.reclass_field = reclassField
            tool.out_raster = outputFolderPath
            tool.remap = reclassString
            Execute_GeoprocessingWithMask(tool, maskPath, False, snapRasterPath)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_ReclassifyRasterFromString Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tool)
        End Try
    End Function

    Public Function BA_MergeFeatures(ByVal inFeatures As String, ByVal outputFolderPath As String, _
                                     ByVal snapRasterPath As String) As BA_ReturnCode
        Dim tool As Merge = New Merge
        Try
            tool.inputs = inFeatures
            tool.output = outputFolderPath
            Execute_Geoprocessing(tool, False, snapRasterPath)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_MergeFeatures Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tool)
        End Try
    End Function

    ' Dissolve a feature class on a specified field
    Public Function BA_Intersect(ByVal inFeatures As String, ByVal outFeatures As String) As BA_ReturnCode
        Dim tool As Intersect = New Intersect
        tool.in_features = inFeatures
        tool.out_feature_class = outFeatures
        'No snapRasterPath because not a spatial analyst tool
        If Execute_Geoprocessing(tool, False, Nothing) = 1 Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    Public Function BA_Erase(ByVal inFeatures As String, ByVal eraseFeatures As String, ByVal outFeatures As String) As BA_ReturnCode
        Dim tool As [Erase] = New [Erase]
        tool.in_features = inFeatures
        tool.erase_features = eraseFeatures
        tool.out_feature_class = outFeatures
        'No snapRasterPath because not a spatial analyst tool
        If Execute_Geoprocessing(tool, False, Nothing) = 1 Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    Public Function BA_BuildRasterAttributeTable(ByVal inRaster As String, ByVal overwrite As Boolean) As BA_ReturnCode
        Dim tool As BuildRasterAttributeTable = New BuildRasterAttributeTable
        tool.in_raster = inRaster
        tool.overwrite = overwrite
        'No snapRasterPath because not a spatial analyst tool
        If Execute_Geoprocessing(tool, False, Nothing) = 1 Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    Public Function BA_MultipartToSinglepart(ByVal inFeatures As String, ByVal outFeatureClass As String) As BA_ReturnCode
        Dim tool As MultipartToSinglepart = New MultipartToSinglepart()
        tool.in_features = inFeatures
        tool.out_feature_class = outFeatureClass
        'No snapRasterPath because not a spatial analyst tool
        If Execute_Geoprocessing(tool, False, Nothing) = 1 Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    Public Function BA_Copy(ByVal inData As String, ByVal outData As String) As BA_ReturnCode
        Dim tool As Copy = New Copy
        tool.in_data = inData
        tool.out_data = outData
        'No snapRasterPath because not a spatial analyst tool
        If Execute_Geoprocessing(tool, False, Nothing) = 1 Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    Public Function BA_ExtractValuesToPoints(ByVal inPointFeatures As String, ByVal inRaster As String, ByVal outPointFeatures As String, _
                                             ByVal snapRasterpath As String, ByVal addAttributes As Boolean) As BA_ReturnCode
        Dim tool As ExtractValuesToPoints = New ExtractValuesToPoints
        tool.in_point_features = inPointFeatures
        tool.in_raster = inRaster
        tool.out_point_features = outPointFeatures
        tool.add_attributes = addAttributes.ToString
        If Execute_Geoprocessing(tool, False, snapRasterpath) = 1 Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    Public Function BA_CalculateField(ByVal inTable As String, ByVal calcField As String, _
                                      ByVal expression As String, ByVal expressionType As String) As BA_ReturnCode
        Dim tool As CalculateField = New CalculateField
        tool.in_table = inTable
        tool.field = calcField
        tool.expression = expression
        tool.expression_type = expressionType
        'No snapRasterPath because not a spatial analyst tool
        If Execute_Geoprocessing(tool, False, Nothing) = 1 Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    Public Function BA_Aggregate(ByVal inRaster As String, ByVal outRaster As String, ByVal cellFactor As String, _
                                 ByVal cellSize As String, ByVal snapRasterPath As String, ByVal aggregationType As String) As BA_ReturnCode
        Dim tool As Aggregate = New Aggregate
        tool.in_raster = inRaster
        tool.out_raster = outRaster
        tool.cell_factor = cellFactor
        tool.aggregation_type = aggregationType
        'Note: As of AGS 10.2 env settings cell size needs to be set to cell factor to line up correctly
        'Makes no sense to us; May change
        If Execute_GeoprocessingWithCellSize(tool, False, snapRasterPath, cellSize) = 1 Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    Public Function BA_Sample(ByVal inRasterPaths As String, ByVal inLocationPath As String, ByVal outTablePath As String, _
                              ByVal snapRasterPath As String, ByVal resamplingType As String, ByVal cellSize As String) As BA_ReturnCode
        Dim tool As Sample = New Sample
        tool.in_rasters = inRasterPaths
        tool.in_location_data = inLocationPath
        tool.out_table = outTablePath
        tool.resampling_type = resamplingType
        If Execute_GeoprocessingWithCellSize(tool, False, snapRasterPath, cellSize) Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    ' Calculate focal statistics with GP
    Public Function BA_FocalStatistics_CellSize(ByVal inRaster As String, ByVal outRasterDataset As String, _
                                                ByVal neighborhood As String, ByVal statisticsType As String, _
                                                ByVal snapRasterPath As String, ByVal cellSize As String) As BA_ReturnCode
        Dim tool As FocalStatistics = New FocalStatistics
        tool.in_raster = inRaster
        tool.out_raster = outRasterDataset
        tool.neighborhood = neighborhood
        tool.statistics_type = statisticsType
        If Execute_GeoprocessingWithCellSize(tool, False, snapRasterPath, cellSize) Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    Public Function BA_Union(ByVal inputFeatures As String, ByVal outputFeatures As String) As BA_ReturnCode
        Dim tool As Union = New Union()
        tool.in_features = inputFeatures
        tool.out_feature_class = outputFeatures
        tool.join_attributes = "ALL"
        'No snapRasterPath because not a spatial analyst tool
        If Execute_Geoprocessing(tool, False, Nothing) = 1 Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    Public Function BA_EuclideanDistance(ByVal inputFeatures As String, ByVal outRaster As String, ByVal cellSize As String, _
                                         ByVal maskPath As String, ByVal snapRasterPath As String, ByVal extentPath As String) As BA_ReturnCode
        Dim tool As EucDistance = New EucDistance()
        tool.in_source_data = inputFeatures
        tool.out_distance_raster = outRaster
        tool.cell_size = cellSize
        Return Execute_GeoprocessingWithExtent(tool, maskPath, False, snapRasterPath, cellSize, extentPath)
    End Function

    Public Function BA_RasterCalculator(ByVal outputRasterPath As String, ByVal expression As String, ByVal snapRasterPath As String,
                                        ByVal maskPath As String)
        Dim tool As RasterCalculator = New RasterCalculator()
        tool.output_raster = outputRasterPath
        tool.expression = expression
        'No snapRasterPath because not a spatial analyst tool
        Return Execute_GeoprocessingWithMask(tool, maskPath, False, snapRasterPath)
    End Function

    Public Function BA_RasterToPoint(ByVal inputRasterPath As String, ByVal outFeaturePath As String, _
                                     ByVal aField As String) As BA_ReturnCode
        Dim tool As RasterToPoint = New RasterToPoint()
        tool.in_raster = inputRasterPath
        tool.out_point_features = outFeaturePath
        tool.raster_field = aField
        'No snapRasterPath because not a spatial analyst tool
        If Execute_Geoprocessing(tool, False, Nothing) = 1 Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    Public Function BA_Buffer(ByVal inputFeaturesPath As String, ByVal outFeaturesPath As String, _
                              ByVal strDistance As String, ByVal strDissolveOption As String) As BA_ReturnCode
        Dim tool As Buffer = New Buffer()
        tool.in_features = inputFeaturesPath
        tool.out_feature_class = outFeaturesPath
        tool.buffer_distance_or_field = strDistance
        If Not String.IsNullOrEmpty(strDissolveOption) Then
            tool.dissolve_option = strDissolveOption
        End If
        'No snapRasterPath because not a spatial analyst tool
        If Execute_Geoprocessing(tool, False, Nothing) = 1 Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    Public Function BA_AppendFeatures(ByVal inFeatures As String, ByVal targetFeatures As String) As BA_ReturnCode
        Dim tool As Append = New Append
        Try
            tool.inputs = inFeatures
            tool.target = targetFeatures
            Execute_Geoprocessing(tool, False, Nothing)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_AppendFeatures Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

    Private Function Execute_GeoprocessingWithExtent(ByVal tool As IGPProcess, ByVal maskPath As String, _
                                                     ByVal addOutputs As Boolean, ByVal snapRasterPath As String, _
                                                     ByVal strCellSize As String, ByVal extentPath As String) As BA_ReturnCode
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim pResult As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult
        Try
            GP.OverwriteOutput = True
            GP.SetEnvironmentValue("cellsize", strCellSize)
            GP.SetEnvironmentValue("mask", maskPath)
            GP.SetEnvironmentValue("extent", extentPath)
            GP.AddOutputsToMap = addOutputs
            If Not String.IsNullOrEmpty(snapRasterPath) Then
                GP.SetEnvironmentValue("snapRaster", snapRasterPath)
            ElseIf Not String.IsNullOrEmpty(GP.GetEnvironmentValue("snapRaster")) Then
                'Had to add this because ArcMap was sometimes caches the snapRaster path inappropriately
                'This clears it out if none is supplied
                GP.SetEnvironmentValue("snapRaster", Nothing)
            End If
            pResult = GP.Execute(tool, Nothing)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            If GP.MessageCount > 0 Then
                MessageBox.Show("Geoprocessor error: " + GP.GetMessages(2))
            Else
                MessageBox.Show("Exception: " + ex.Message)
            End If
            Return BA_ReturnCode.UnknownError
        Finally
            GP = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Function

    Public Function BA_Times(ByVal inRasterPath1 As String, ByVal inRasterPath2 As String, _
                             ByVal outRasterPath As String) As BA_ReturnCode
        Dim tool As Times = New Times
        Try
            tool.in_raster_or_constant1 = inRasterPath1
            tool.in_raster_or_constant2 = inRasterPath2
            tool.out_raster = outRasterPath
            Execute_Geoprocessing(tool, False, Nothing)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_Times Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

    Public Function BA_SnapPourPoint(ByVal inAccumulationRaster As String, ByVal inPourPointData As String, _
                                     ByVal snapDistance As String, ByVal outRasterPath As String) As BA_ReturnCode
        Dim tool As SnapPourPoint = New SnapPourPoint
        Try
            tool.in_accumulation_raster = inAccumulationRaster
            tool.in_pour_point_data = inPourPointData
            tool.snap_distance = snapDistance
            tool.out_raster = outRasterPath
            Dim retVal As Short = Execute_Geoprocessing(tool, False, Nothing)
            If retVal = 1 Then
                Return BA_ReturnCode.Success
            Else
                Return BA_ReturnCode.UnknownError
            End If
        Catch ex As Exception
            MessageBox.Show("BA_SnapPourPoint Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

    Public Function BA_Watershed(ByVal inFlowDirectionRaster As String, ByVal inPourPointData As String, _
                                 ByVal outRasterPath As String) As BA_ReturnCode
        Dim tool As Watershed = New Watershed
        Try
            tool.in_flow_direction_raster = inFlowDirectionRaster
            tool.in_pour_point_data = inPourPointData
            tool.out_raster = outRasterPath
            Dim retVal As Short = Execute_Geoprocessing(tool, False, Nothing)
            If retVal = 1 Then
                Return BA_ReturnCode.Success
            Else
                Return BA_ReturnCode.UnknownError
            End If
        Catch ex As Exception
            MessageBox.Show("BA_Watershed Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

    Public Function BA_Raster2ASCII(ByVal inRasterPath As String, ByVal outRasterPath As String) As BA_ReturnCode
        Dim tool As RasterToASCII = New RasterToASCII
        Try
            tool.in_raster = inRasterPath
            tool.out_ascii_file = outRasterPath
            Execute_Geoprocessing(tool, False, Nothing)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_Raster2ASCII Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

End Module


