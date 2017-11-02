Imports BAGIS_ClassLibrary
Imports System.IO
Imports System.Text
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geoprocessing
Imports ESRI.ArcGIS.esriSystem
Imports System.Windows.Forms

Module MethodModule

    Public Function BA_LoadMethodFromXml(ByVal myPath As String, ByVal methodName As String) As Method
        Dim xmlPath As String = BA_BagisPXmlPath(myPath, methodName)
        If BA_File_ExistsWindowsIO(xmlPath) Then
            Dim obj As Object = SerializableData.Load(xmlPath, GetType(Method))
            If obj IsNot Nothing Then
                Dim pMethod As Method = CType(obj, Method)
                Return pMethod
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function

    ' Loads a list of all the methods
    Public Function BA_LoadMethodsFromXml(ByVal myPath As String) As List(Of Method)
        If BA_Folder_ExistsWindowsIO(myPath) Then
            Dim dirInfo As New DirectoryInfo(myPath)
            Dim allDirectories As FileInfo() = dirInfo.GetFiles
            Dim allMethods As List(Of Method) = New List(Of Method)
            For Each pFile As FileInfo In allDirectories
                Dim fileExt As String = pFile.Extension
                'Make sure we are trying to serialize an xml file
                If fileExt.ToLower = BA_FILE_EXT_XML Then
                    Dim obj As Object = SerializableData.Load(pFile.FullName, GetType(Method))
                    If obj IsNot Nothing Then
                        Dim pMethod As Method = CType(obj, Method)
                        allMethods.Add(pMethod)
                    End If
                End If
            Next
            Return allMethods
        Else
            Return Nothing
        End If
    End Function

    'Calculates and returns a model system parameter
    Public Function BA_CalculateSystemParameter(ByVal paramName As String, ByVal hruPath As String, _
                                                ByVal profileName As String, ByVal pDataTable As Hashtable) As String
        Dim modelParam As SystemModelParameterName = BA_GetSystemModelParameterName(paramName)
        Dim aoiPath As String = ""
        Dim pos As Integer = hruPath.IndexOf(BA_EnumDescription(PublicPath.HruDirectory))
        If pos > -1 Then
            aoiPath = hruPath.Substring(0, pos)
        End If

        Select Case modelParam
            Case SystemModelParameterName.sys_param_table
                'C:\Docs\Lesley\ochoco_FGDB\zones\hru_ca\param.gdb\Profile1_params
                Dim sb As StringBuilder = New StringBuilder()
                sb.Append(hruPath)
                sb.Append(BA_EnumDescription(PublicPath.BagisParamGdb))
                sb.Append("\" & profileName & BA_PARAM_TABLE_SUFFIX)
                Return sb.ToString
            Case SystemModelParameterName.sys_hru_folder
                'C:\Docs\Lesley\ochoco_FGDB\zones\hru_ca
                Return BA_StandardizePathString(hruPath)
            Case SystemModelParameterName.sys_aoi_path
                'C:\Docs\Lesley\ochoco_FGDB
                Return aoiPath
            Case SystemModelParameterName.sys_hru_name
                'hru_ca
                Return BA_GetBareName(hruPath)
            Case SystemModelParameterName.sys_slope_path
                'C:\Docs\Lesley\ochoco_FGDB\surfaces.gdb\slope
                Dim sb As StringBuilder = New StringBuilder()
                sb.Append(BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces, True))
                sb.Append(BA_GetBareName(BA_EnumDescription(PublicPath.Slope)))
                Return sb.ToString
            Case SystemModelParameterName.sys_units_elevation
                'Get the elevation units from filled_dem
                Dim pUnit As MeasurementUnit = MeasurementUnit.Missing
                Dim inputFolder As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces)
                Dim inputFile As String = BA_EnumDescription(MapsFileName.filled_dem_gdb)
                Dim tagsList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                           LayerType.Raster, BA_XPATH_TAGS)
                If tagsList IsNot Nothing Then
                    For Each pInnerText As String In tagsList
                        'This is our BAGIS tag
                        If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                            Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                            If strUnits IsNot Nothing Then
                                pUnit = BA_GetMeasurementUnit(strUnits)
                            End If
                            Exit For
                        End If
                    Next
                End If
                Return ControlChars.Quote & BA_EnumDescription(pUnit) & ControlChars.Quote
            Case SystemModelParameterName.sys_units_temperature
                Dim pUnit As MeasurementUnit = MeasurementUnit.Missing
                For Each pKey As String In pDataTable.Keys
                    Dim pDataSource As DataSource = pDataTable(pKey)
                    If pDataSource.MeasurementUnitType = MeasurementUnitType.Temperature Then
                        pUnit = pDataSource.MeasurementUnit
                        Exit For
                    End If
                Next
                Return ControlChars.Quote & BA_EnumDescription(pUnit) & ControlChars.Quote
            Case SystemModelParameterName.sys_units_depth
                Dim pUnit As MeasurementUnit = MeasurementUnit.Missing
                Dim inputFolder As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Prism)
                Dim inputFile As String = AOIPrismFolderNames.annual.ToString
                Dim tagsList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                           LayerType.Raster, BA_XPATH_TAGS)
                If tagsList IsNot Nothing Then
                    For Each pInnerText As String In tagsList
                        'This is our BAGIS tag
                        If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                            Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                            If strUnits IsNot Nothing Then
                                pUnit = BA_GetMeasurementUnit(strUnits)
                            End If
                            Exit For
                        End If
                    Next
                End If
                Return ControlChars.Quote & BA_EnumDescription(pUnit) & ControlChars.Quote
            Case SystemModelParameterName.sys_units_slope
                Dim pUnit As SlopeUnit = SlopeUnit.Missing
                Dim inputFolder As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces)
                Dim inputFile As String = BA_GetBareName(BA_EnumDescription(PublicPath.Slope))
                Dim tagsList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                           LayerType.Raster, BA_XPATH_TAGS)
                If tagsList IsNot Nothing Then
                    For Each pInnerText As String In tagsList
                        'This is our BAGIS tag
                        If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                            Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                            If strUnits IsNot Nothing Then
                                pUnit = BA_GetMeasurementUnit(strUnits)
                            End If
                            Exit For
                        End If
                    Next
                End If
                Return ControlChars.Quote & BA_EnumDescription(pUnit) & ControlChars.Quote
            Case SystemModelParameterName.sys_jh_coef_table
                Return BA_GetBareName(BA_EnumDescription(PublicPath.JhCoefAoiTable))
            Case Else
                Return Nothing
        End Select
    End Function

    'Returns the file path to the selected db source when supplied with the db source name
    Public Function BA_CalculateDbParameter(ByVal aoiPath As String, ByVal pDataTable As Hashtable, _
                                            ByVal paramName As String, ByVal pMethod As Method) As String
        'Only run function if paramName has "db_" prefix
        If paramName.Substring(0, BA_DATABIN_PREFIX.Length).ToLower = BA_DATABIN_PREFIX Then
            'Get the data source alias (name) out of the BAGIS-P method
            Dim dataSourceName As String = BA_GetParamValueFromMethod(pMethod, paramName)
            If Not String.IsNullOrEmpty(dataSourceName) Then
                Dim selDataSource As DataSource = pDataTable(dataSourceName)
                If selDataSource IsNot Nothing Then
                    Dim fullSourcePath As String = BA_GetDataBinPath(aoiPath) & "\" & selDataSource.Source
                    Return fullSourcePath
                End If
            End If
        End If
        Return Nothing
    End Function

    Public Function BA_GetSystemModelParameterName(ByVal paramName As String) As SystemModelParameterName
        For Each param In [Enum].GetValues(GetType(SystemModelParameterName))
            If param.ToString = paramName Then
                Return param
            End If
        Next
        Return Nothing
    End Function

    Public Function BA_LoadDataSources(ByVal settingsPath As String) As Hashtable
        Dim pSettings As Settings = BA_CreateOrLoadSettingsFile(settingsPath)
        Dim layerTable As Hashtable = New Hashtable
        If pSettings.DataSources IsNot Nothing AndAlso pSettings.DataSources.Count > 0 Then
            Dim paramIdx = settingsPath.IndexOf(BA_EnumDescription(PublicPath.BagisParamFolder))
            For Each dLayer As DataSource In pSettings.DataSources
                Dim sourcePath As String = dLayer.Source
                If paramIdx > -1 Then
                    Dim parentPath = "Please Return"
                    Dim tempFileName = BA_GetBareName(settingsPath, parentPath)
                    'Trim trailing \
                    parentPath = parentPath.Substring(0, parentPath.Length - 1)
                    sourcePath = parentPath & BA_EnumDescription(PublicPath.BagisDataBinGdb) & "\" & dLayer.Source
                End If
                If dLayer.AoiLayer Then
                    'If the layer is an aoi layer, we assume it is valid
                    dLayer.IsValid = dLayer.AoiLayer
                    layerTable.Add(dLayer.Name, dLayer)
                ElseIf dLayer.LayerType = LayerType.Raster Then
                    Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(sourcePath)
                    If BA_File_Exists(sourcePath, wType, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset) Then
                        dLayer.IsValid = True
                        layerTable.Add(dLayer.Name, dLayer)
                    Else
                        dLayer.IsValid = False
                    End If
                Else
                    Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(sourcePath)
                    If BA_File_Exists(sourcePath, wType, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass) Then
                        layerTable.Add(dLayer.Name, dLayer)
                        dLayer.IsValid = True
                    Else
                        dLayer.IsValid = False
                    End If
                End If
            Next
        End If
        Return layerTable
    End Function

    Public Function BA_LoadAllDataSources(ByVal settingsPath As String) As Dictionary(Of String, DataSource)
        Dim pSettings As Settings = BA_CreateOrLoadSettingsFile(settingsPath)
        Dim layerTable As Dictionary(Of String, DataSource) = New Dictionary(Of String, DataSource)
        If pSettings.DataSources IsNot Nothing AndAlso pSettings.DataSources.Count > 0 Then
            Dim paramIdx = settingsPath.IndexOf(BA_EnumDescription(PublicPath.BagisParamFolder))
            For Each dLayer As DataSource In pSettings.DataSources
                Dim sourcePath As String = dLayer.Source
                If paramIdx > -1 Then
                    Dim parentPath = "Please Return"
                    Dim tempFileName = BA_GetBareName(settingsPath, parentPath)
                    'Trim trailing \
                    parentPath = parentPath.Substring(0, parentPath.Length - 1)
                    sourcePath = parentPath & BA_EnumDescription(PublicPath.BagisDataBinGdb) & "\" & dLayer.Source
                End If
                If dLayer.AoiLayer Then
                    dLayer.IsValid = True
                ElseIf dLayer.LayerType = LayerType.Raster Then
                    Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(sourcePath)
                    dLayer.IsValid = BA_File_Exists(sourcePath, wType, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset)
                Else
                    Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(sourcePath)
                    dLayer.IsValid = BA_File_Exists(sourcePath, wType, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass)
                End If
                layerTable.Add(dLayer.Name, dLayer)
            Next
        End If
        Return layerTable
    End Function

    Public Function BA_CreateOrLoadSettingsFile(ByVal settingsPath As String) As Settings
        Dim pSettings As Settings = Nothing
        'First try to load an existing settings file
        If BA_File_ExistsWindowsIO(settingsPath) Then
            Dim obj As Object = SerializableData.Load(settingsPath, GetType(Settings))
            If obj IsNot Nothing Then
                pSettings = CType(obj, Settings)
            End If
        End If
        'If settings file doesn't exist then build/save a new one
        If pSettings Is Nothing Then
            Dim dataLayerList As List(Of DataSource) = New List(Of DataSource)
            'Aspect layer
            Dim slopeLayer As DataSource = New DataSource(1, "Aspect", "Slope aspect", "AOI aspect layer", True, LayerType.Raster)
            dataLayerList.Add(slopeLayer)
            'Elevation layer
            Dim elevLayer As DataSource = New DataSource(2, "Elevation", "Filled DEM", "AOI filled DEM layer", True, LayerType.Raster)
            dataLayerList.Add(elevLayer)
            'Flow accumulation layer
            Dim flowLayer As DataSource = New DataSource(3, "FlowAcc", "Flow accumulation", "AOI flow accumulation layer", True, LayerType.Raster)
            dataLayerList.Add(flowLayer)
            pSettings = New Settings()
            pSettings.DataSources = dataLayerList
            pSettings.Save(settingsPath)
        End If
        Return pSettings
    End Function

    Public Function BA_GetLocalMethodsDir(ByVal aoiPath As String) As String
        'trim trailing backslash from aoiPath
        aoiPath = BA_StandardizePathString(aoiPath)
        Dim sb As StringBuilder = New StringBuilder
        sb.Append(aoiPath)
        sb.Append(BA_EnumDescription(PublicPath.BagisLocalMethods))
        'return the profiles folder if it exists
        If BA_Folder_ExistsWindowsIO(sb.ToString) Then
            Return sb.ToString
        Else
            'Otherwise create it before returning
            Dim methodsFolder As String = BA_GetBareName(BA_EnumDescription(PublicPath.BagisLocalMethods))
            Dim paramFolder As String = BA_EnumDescription(PublicPath.BagisParamFolder)
            'Trim leading backslash
            If paramFolder(0) = "\" Then
                paramFolder = paramFolder.Remove(0, 1)
            End If
            Dim newFolder As String = BA_CreateFolder(aoiPath, paramFolder)
            If Not String.IsNullOrEmpty(newFolder) Then
                Dim retFolder As String = BA_CreateFolder(newFolder, methodsFolder)
                If Not String.IsNullOrEmpty(retFolder) Then
                    Return retFolder
                End If
            End If
        End If
        Return Nothing
    End Function

    'Pass in an Hashtable of data sources to check their units and populate, if appropriate
    Public Function BA_AppendUnitsToDataSources(ByRef pDataTable As Hashtable, ByVal aoiPath As String) As BA_ReturnCode
        If pDataTable IsNot Nothing AndAlso pDataTable.Keys.Count > 0 Then
            For Each pKey As String In pDataTable.Keys
                Dim pDataSource As DataSource = pDataTable(pKey)
                Dim inputFolder As String = Nothing
                Dim inputFile As String = Nothing
                If pDataSource.AoiLayer = False AndAlso pDataSource.IsValid = True Then
                    If String.IsNullOrEmpty(aoiPath) Then
                        'This is a public data source
                        If BA_GetWorkspaceTypeFromPath(pDataSource.Source) = WorkspaceType.ImageServer Then
                            inputFolder = pDataSource.Source
                        Else
                            inputFolder = "PleaseReturn"
                            inputFile = BA_GetBareName(pDataSource.Source, inputFolder)
                        End If
                    Else
                        'Otherwise it's a local data source
                        inputFolder = BA_GetDataBinPath(aoiPath)
                        inputFile = BA_GetBareName(pDataSource.Source)
                    End If
                    Dim tagsList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                                       pDataSource.LayerType, _
                                                                       BA_XPATH_TAGS)
                    If tagsList IsNot Nothing AndAlso tagsList.Count > 0 Then
                        For Each pInnerText As String In tagsList
                            'This is our BAGIS tag
                            If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                                Dim strCategory As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_CATEGORY_TAG)
                                If Not String.IsNullOrEmpty(strCategory) Then
                                    Dim pUnitType As MeasurementUnitType = BA_GetMeasurementUnitType(strCategory)
                                    pDataSource.MeasurementUnitType = pUnitType
                                End If
                                Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                                If Not String.IsNullOrEmpty(strUnits) Then
                                    Dim pUnits As MeasurementUnit = BA_GetMeasurementUnit(strUnits)
                                    If pUnits <> MeasurementUnit.Missing Then
                                        pDataSource.MeasurementUnit = pUnits
                                    Else
                                        'Some special treatment here in case it is a slope unit
                                        Dim slopeUnits As SlopeUnit = BA_GetSlopeUnit(strUnits)
                                        If slopeUnits <> SlopeUnit.Missing Then
                                            pDataSource.SlopeUnit = slopeUnits
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If
            Next
        End If
    End Function

    'Wrapper function to allow Dictionary as input to BA_AppendUnitsToDataSources
    Public Function BA_AppendUnitsToDataSources(ByRef pDataTable As IDictionary(Of String, DataSource), ByVal aoiPath As String) As BA_ReturnCode
        Dim addTable As Hashtable = New Hashtable
        For Each kvp As KeyValuePair(Of String, DataSource) In pDataTable
            addTable.Add(kvp.Key, kvp.Value)
        Next kvp
        Dim success As BA_ReturnCode = BA_AppendUnitsToDataSources(addTable, aoiPath)
        If success = BA_ReturnCode.Success Then
            For Each key As String In addTable.Keys
                Dim dSource As DataSource = addTable(key)
                pDataTable(key) = dSource
            Next
        End If
    End Function

    Public Sub BA_SetMeasurementUnitsForAoi(ByVal aoiPath As String, ByVal aDataTable As Hashtable, _
                                         ByRef slopeUnit As SlopeUnit, ByRef elevUnit As MeasurementUnit, _
                                         ByRef depthUnit As MeasurementUnit, ByRef degreeUnit As MeasurementUnit, _
                                         ByRef prismLayersExist As Boolean)
        'Slope units
        Dim inputFolder As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces)
        Dim inputFile As String = BA_GetBareName(BA_EnumDescription(PublicPath.Slope))
        Dim tagsList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                   LayerType.Raster, BA_XPATH_TAGS)
        If tagsList IsNot Nothing Then
            For Each pInnerText As String In tagsList
                'This is our BAGIS tag
                If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                    Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                    If strUnits IsNot Nothing Then
                        slopeUnit = BA_GetSlopeUnit(strUnits)
                    End If
                    Exit For
                End If
            Next
        End If

        'Elevation units
        inputFile = BA_EnumDescription(MapsFileName.filled_dem_gdb)
        tagsList = BA_ReadMetaData(inputFolder, inputFile, _
                                   LayerType.Raster, BA_XPATH_TAGS)
        If tagsList IsNot Nothing Then
            For Each pInnerText As String In tagsList
                'This is our BAGIS tag
                If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                    Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                    If strUnits IsNot Nothing Then
                        elevUnit = BA_GetMeasurementUnit(strUnits)
                    End If
                    Exit For
                End If
            Next
        End If

        'Depth units
        inputFolder = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Prism)
        inputFile = AOIPrismFolderNames.annual.ToString
        If BA_File_Exists(inputFolder & "\" & inputFile, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            prismLayersExist = True
            tagsList = BA_ReadMetaData(inputFolder, inputFile, _
                               LayerType.Raster, BA_XPATH_TAGS)
            If tagsList IsNot Nothing Then
                For Each pInnerText As String In tagsList
                    'This is our BAGIS tag
                    If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                        Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                        If strUnits IsNot Nothing Then
                            depthUnit = BA_GetMeasurementUnit(strUnits)
                        End If
                        Exit For
                    End If
                Next
            End If
        Else
            prismLayersExist = False
            depthUnit = MeasurementUnit.Missing
        End If

        'Degree units
        If aDataTable IsNot Nothing Then
            For Each key In aDataTable.Keys
                Dim pSource As DataSource = aDataTable(key)
                If Not pSource.AoiLayer Then
                    inputFolder = BA_GetDataBinPath(aoiPath)
                    inputFile = pSource.Source
                    tagsList = BA_ReadMetaData(inputFolder, inputFile, pSource.LayerType, BA_XPATH_TAGS)
                    If tagsList IsNot Nothing Then
                        For Each pInnerText As String In tagsList
                            'This is our BAGIS tag
                            If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                                Dim strCategory As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_CATEGORY_TAG)
                                Dim unitCategory As MeasurementUnitType = BA_GetMeasurementUnitType(strCategory)
                                If unitCategory = MeasurementUnitType.Temperature Then
                                    Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                                    If strUnits IsNot Nothing Then
                                        degreeUnit = BA_GetMeasurementUnit(strUnits)
                                    End If
                                End If
                                Exit For
                            End If
                        Next

                    End If
                End If
            Next
        End If

    End Sub

    Public Function BA_ValidateMeasurementUnitsForAoi(ByVal aDataTable As Hashtable, ByVal depthUnit As MeasurementUnit, _
                                                      ByVal elevUnit As MeasurementUnit, ByVal slopeUnit As SlopeUnit, _
                                                      ByVal degreeUnit As MeasurementUnit) As String
        Dim sb As StringBuilder = New StringBuilder
        If aDataTable IsNot Nothing Then
            For Each key In aDataTable.Keys
                Dim pSource As DataSource = aDataTable(key)
                If Not pSource.AoiLayer Then
                    Dim unitType As MeasurementUnitType = pSource.MeasurementUnitType
                    Select Case unitType
                        Case MeasurementUnitType.Depth
                            Dim unit As MeasurementUnit = pSource.MeasurementUnit
                            If unit <> depthUnit Then
                                sb.AppendLine("The depth measurement units for data source " & pSource.Name)
                                sb.AppendLine("do not match those of the Prism data in this AOI.")
                                sb.AppendLine("The depth measurement units for this AOI are " & BA_EnumDescription(depthUnit))
                                sb.AppendLine("Redefine this data source with the correct units using the")
                                sb.AppendLine("Public Data Manager and reclip the data source to this AOI")
                                sb.AppendLine()
                            End If
                        Case MeasurementUnitType.Elevation
                            Dim unit As MeasurementUnit = pSource.MeasurementUnit
                            If unit <> elevUnit Then
                                sb.AppendLine("The elevation measurement units for data source " & pSource.Name)
                                sb.AppendLine("do not match those of the filled DEM in this AOI.")
                                sb.AppendLine("The elevation measurement units for this AOI are " & BA_EnumDescription(elevUnit))
                                sb.AppendLine("Redefine this data source with the correct units using the")
                                sb.AppendLine("Public Data Manager and reclip the data source to this AOI")
                                sb.AppendLine()
                            End If
                        Case MeasurementUnitType.Slope
                            Dim unit As SlopeUnit = pSource.SlopeUnit
                            If unit <> slopeUnit Then
                                sb.AppendLine("The slope measurement units for data source " & pSource.Name)
                                sb.AppendLine("do not match those of the slope layer in this AOI.")
                                sb.AppendLine("The slope measurement units for this AOI are " & BA_EnumDescription(slopeUnit))
                                sb.AppendLine("Redefine this data source with the correct units using the")
                                sb.AppendLine("Public Data Manager and reclip the data source to this AOI")
                                sb.AppendLine()
                            End If
                        Case MeasurementUnitType.Temperature
                            Dim unit As MeasurementUnit = pSource.MeasurementUnit
                            If unit <> degreeUnit Then
                                sb.AppendLine("The degree measurement units for data source " & pSource.Name)
                                sb.AppendLine("do not match those of the another temperature layer in this AOI.")
                                sb.AppendLine("The degree measurement units for this AOI are " & BA_EnumDescription(degreeUnit))
                                sb.AppendLine("Redefine this data source with the correct units using the")
                                sb.AppendLine("Public Data Manager and reclip the data source to this AOI")
                                sb.AppendLine()
                            End If
                    End Select
                End If
            Next
        End If
        Return sb.ToString
    End Function

    'Validate units to make sure we don't have data sources with different units
    'for the same unit type
    Public Function BA_ValidateMeasurementUnits(ByVal pLayerTable As Hashtable, ByVal pUnitType As MeasurementUnitType, _
                                                ByVal pUnit As MeasurementUnit, ByVal pSlopeUnit As SlopeUnit) As DataSource
        Dim dataSource As DataSource = Nothing
        If pLayerTable IsNot Nothing And pLayerTable.Keys.Count > 0 Then
            For Each strKey As String In pLayerTable.Keys
                Dim nextDataSource As DataSource = pLayerTable(strKey)
                If nextDataSource.MeasurementUnitType <> MeasurementUnitType.Missing Then
                    If nextDataSource.MeasurementUnitType = pUnitType Then
                        If nextDataSource.MeasurementUnitType = MeasurementUnitType.Slope Then
                            If nextDataSource.SlopeUnit <> pSlopeUnit Then
                                Return nextDataSource
                            End If
                        Else
                            If nextDataSource.MeasurementUnit <> pUnit Then
                                Return nextDataSource
                            End If
                        End If
                    End If
                End If
            Next
        End If
        Return dataSource
    End Function

    'This method gets the value for a given parameter name out of a method. Sometimes we store parameter names
    'at the method level. For example a custom data source name such as 'db_landfire_evt'
    Public Function BA_GetParamValueFromMethod(ByVal pMethod As Method, ByVal paramName As String) As String
        Dim strValue As String = Nothing
        Dim methodParams As List(Of ModelParameter) = pMethod.ModelParameters
        If methodParams IsNot Nothing Then
            For Each methodParam As ModelParameter In methodParams
                If methodParam.Name = paramName Then
                    strValue = methodParam.Value
                    Return strValue
                End If
            Next
        End If
        Return strValue
    End Function

    Public Function BA_GetJHLayerPaths(ByVal aoiPath As String, ByRef unitsDataSource As DataSource) As IDictionary(Of String, String)
        Dim returnDictionary As IDictionary(Of String, String) = New Dictionary(Of String, String)
        Dim settingsPath As String = BA_GetBagisPSettingsPath()
        Dim dataTable As Dictionary(Of String, DataSource) = BA_LoadAllDataSources(settingsPath)
        For Each dName As String In dataTable.Keys
            Dim dSource As DataSource = dataTable(dName)
            If Not String.IsNullOrEmpty(dSource.JH_Coeff) Then
                Dim jhRole As String = dSource.JH_Coeff
                Dim fileName As String = BA_GetBareName(dSource.Source)
                Dim lclPath As String = BA_GetDataBinPath(aoiPath) & "\" & fileName
                If BA_File_Exists(lclPath, WorkspaceType.Geodatabase, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset) Then
                    returnDictionary.Add(jhRole, lclPath)
                    If unitsDataSource Is Nothing Then
                        unitsDataSource = dSource
                    End If
                End If
            End If
        Next
        Return returnDictionary
    End Function

    Public Function BA_ExecuteJHModel(ByVal aoiPath As String, ByVal hruPath As String, ByVal selProfile As String, _
                                     ByVal toolBoxPrefix As String, ByVal layerPaths As IDictionary(Of String, String), _
                                     ByVal unitsDataSource As DataSource, ByVal outputFieldName As String) As BA_ReturnCode
        Dim pModel As IGPTool
        Dim pParamArray As IVariantArray = New VarArray
        Try
            pModel = BA_OpenModel(toolBoxPrefix, "bagis_method_building_blocks.tbx", "JHCoefAOI")
            Dim params As List(Of ModelParameter) = BA_GetModelParameters(pModel)
            'Getting the units from 1 data source only for better performance
            Dim dataTable As Hashtable = New Hashtable
            dataTable.Add(unitsDataSource.Name, unitsDataSource)
            BA_AppendUnitsToDataSources(dataTable, aoiPath)
            For Each mParam As ModelParameter In params
                Dim pValue As String = BA_CalculateSystemParameter(mParam.Name, hruPath, selProfile, dataTable)
                If pValue Is Nothing Then
                    'The parameter was not a system parameter
                    If mParam.Name = "db_jul_tmin_grid" Then
                        mParam.Value = layerPaths(BA_JH_Coef_Jul_Tmin)
                    ElseIf mParam.Name = "db_jul_tmax_grid" Then
                        mParam.Value = layerPaths(BA_JH_Coef_Jul_Tmax)
                    ElseIf mParam.Name = "db_aug_tmax_grid" Then
                        mParam.Value = layerPaths(BA_JH_Coef_Aug_Tmax)
                    ElseIf mParam.Name = "db_aug_tmin_grid" Then
                        mParam.Value = layerPaths(BA_JH_Coef_Aug_Tmin)
                    ElseIf mParam.Name.Substring(0, BA_FIELD_PREFIX.Length).ToLower = BA_FIELD_PREFIX Then
                        mParam.Value = outputFieldName
                    End If
                Else
                    mParam.Value = pValue
                End If
                pParamArray.Add(mParam.Value)
                Debug.Print(mParam.Name & ": " & mParam.Value)
            Next

            Dim errorMessage As String = Nothing
            Dim warningMessage As String = Nothing
            Dim scratchDir As String = aoiPath & BA_EnumDescription(PublicPath.BagisPDefaultWorkspace)
            Dim success As BA_ReturnCode = BA_ExecuteModel(pModel.Toolbox.PathName, pModel.Name, pParamArray, scratchDir, errorMessage, warningMessage)
            If Not String.IsNullOrEmpty(errorMessage) Then
                MessageBox.Show("An error occurred while executing the model. " & errorMessage & vbCrLf, "Error message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return BA_ReturnCode.UnknownError
            End If
            'If Not String.IsNullOrEmpty(warningMessage) Then
            '    MessageBox.Show(warningMessage & vbCrLf, "Warning message", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            'End If
            Return success
        Catch ex As Exception
            Debug.Print("BA_ExecuteJHModel Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pModel = Nothing
            pParamArray = Nothing
        End Try
    End Function

    Public Function BA_ReadJHCoeffResults(ByVal gdbPath As String, ByVal fileName As String, _
                                          ByVal fieldName As String) As Double
        Dim result As Double = BA_9999
        Dim rTable As ITable = Nothing
        Dim pCursor As ICursor = Nothing
        Dim pRow As IRow = Nothing
        Dim pFields As IFields = Nothing
        Try
            rTable = BA_OpenTableFromGDB(gdbPath, fileName)
            If rTable IsNot Nothing Then
                If rTable IsNot Nothing Then
                    pFields = rTable.Fields
                    Dim idxJh As Integer = rTable.FindField(fieldName)
                    If idxJh > -1 Then
                        pCursor = rTable.Search(Nothing, False)
                        'There is only one row in this table
                        pRow = pCursor.NextRow
                        If pRow IsNot Nothing Then result = CDbl(pRow.Value(idxJh))
                    End If
                End If
            End If
        Catch ex As Exception
            Debug.Print("BA_ReadJHCoeffResults Exception: " & ex.Message)
        Finally
            rTable = Nothing
            pCursor = Nothing
            pRow = Nothing
            pFields = Nothing
        End Try
        Return result
    End Function

    Public Function BA_UpdateParametersInNmonthsTable(ByRef nmonthsTable As ParameterTable, ByVal updateTable As IDictionary(Of String, AoiParameter), _
                                                      ByVal paramNamesToExport As IList(Of String)) As ParameterTable
        'Put ParameterTable information into structures that are easier to use
        Dim headerList As List(Of String) = New List(Of String)
        headerList.AddRange(nmonthsTable.Headers)
        Dim monthsList As IList(Of List(Of String)) = New List(Of List(Of String))
        For i As Integer = 0 To nmonthsTable.Values.GetUpperBound(0)
            Dim paramsList As IList(Of String) = New List(Of String)
            For j As Integer = 0 To nmonthsTable.Values.GetUpperBound(1)
                paramsList.Add(nmonthsTable.Values(i, j))
            Next
            monthsList.Add(paramsList)
        Next

        'Loop through the parameters we want to export
        For Each pName As String In paramNamesToExport
            Dim nextParam As AoiParameter = Nothing
            If updateTable.ContainsKey(pName) Then nextParam = updateTable(pName)
            If nextParam IsNot Nothing Then
                Dim idxCol As Short = -1
                'Looking for the column existing in the table
                For i As Short = 0 To headerList.Count - 1
                    If headerList(i).Equals(pName) Then
                        idxCol = i
                        Exit For
                    End If
                Next

                'Add the column header if we didn't find it
                If idxCol = -1 Then
                    headerList.Add(pName)
                    idxCol = headerList.Count - 1
                    'Add an entry to each paramList too
                    For Each paramList As List(Of String) In monthsList
                        paramList.Add(BA_9999)
                    Next
                End If

                'We found the column
                If idxCol > -1 Then
                    Dim i As Short = 0
                    For Each paramList As List(Of String) In monthsList
                        ''The parameter is a single value and should be populated to all months
                        If Not String.IsNullOrEmpty(nextParam.Value) Then
                            Dim nextParamValue As String = nextParam.Value
                            paramList(idxCol) = nextParamValue
                        ElseIf nextParam.ValuesList IsNot Nothing AndAlso nextParam.ValuesList.Count = NUM_MONTHS Then
                            paramList(idxCol) = nextParam.ValuesList(i)
                        End If
                        i += 1
                    Next
                End If
            End If
        Next

        'Reassemble the data in ParameterTable structure
        Dim updatedHeaders() As String = headerList.ToArray
        Dim updatedValues(monthsList.Count - 1, headerList.Count - 1) As String
        Dim m As Short = 0
        For Each paramList As List(Of String) In monthsList
            Dim p As Integer = 0
            For Each param As String In paramList
                updatedValues(m, p) = param
                p += 1
            Next
            m += 1
        Next
        Dim retVal As ParameterTable = New ParameterTable(nmonthsTable.Name, nmonthsTable.Dimension1, nmonthsTable.Dimension2, updatedValues, updatedHeaders)
        Return retVal
    End Function

    Public Function BA_LoadAoiParameters(ByVal settingsPath As String) As IDictionary(Of String, AoiParameter)
        Dim pSettings As Settings = BA_CreateOrLoadSettingsFile(settingsPath)
        Dim paramTable As IDictionary(Of String, AoiParameter) = New Dictionary(Of String, AoiParameter)
        If pSettings.AoiParameters IsNot Nothing AndAlso pSettings.AoiParameters.Count > 0 Then
            For Each param As AoiParameter In pSettings.AoiParameters
                paramTable.Add(param.Name, param)
            Next
        End If
        Return paramTable
    End Function

    Public Function BA_SaveAOIParameters(ByVal aoiParameters As IList(Of AoiParameter), _
                                         ByVal settingsPath As String) As BA_ReturnCode
        Try
            Dim settings As Settings = BA_CreateOrLoadSettingsFile(settingsPath)
            If settings IsNot Nothing Then
                settings.AoiParameters = aoiParameters
                settings.Save(settingsPath)
                Return BA_ReturnCode.Success
            Else
                Return BA_ReturnCode.ReadError
            End If
        Catch ex As Exception
            Debug.Print("BA_SaveAOIParameters exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

End Module
