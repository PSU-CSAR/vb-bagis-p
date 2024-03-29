﻿Imports System.IO
Imports System.Text
Imports BAGIS_ClassLibrary
Imports Microsoft.VisualBasic.FileIO
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.SpatialAnalyst
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geoprocessing
Imports System.IO.Compression
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.Geometry

Module ParameterModule

    Public FLAG As String = "@"
    Public PARAM_FLAG As String = FLAG & "P"
    Public SECTION_FLAG As String = FLAG & "S"
    Public HEADER_FLAG As String = FLAG & "H"
    Public TABLE_FLAG As String = FLAG & "T"
    Private BOUND As String = "bound"
    Private DIMENSION As String = "dimension"
    Private ROLE As String = "role"
    Public LEN_KEY As String = "len"
    Public DESCR As String = "Descr"
    Public MODIFIED_AT As String = "modifed_at"
    Public VERSION As String = "Version"
    Public CREATED_AT As String = "created_at"
    Public CREATED_BY As String = "created_by"
    Public BAGIS_P As String = "BAGIS-P"
    Public CONVERTED_FROM As String = "converted_from"
    Public DATE_FORMAT As String = "date_format"
    Public NHRU As String = "nhru"
    Public NRADPL As String = "nradpl"
    Public NGW As String = "ngw"
    Public NSSR As String = "nssr"
    Public NMONTHS As String = "nmonths"
    Public BASIN_AREA As String = "basin_area"
    Public BASIN_LAT As String = "basin_lat"
    Public HEADER_KEY As String = "header_key"
    Public MISSING_VALUE As String = "missing_value"
    Public NUM_MONTHS As Short = 12
    Public HRU_ELEV As String = "hru_elev"
    Public BASIN_TSTA_HRU = "basin_tsta_hru"

    'Retrieves the single dimension parameters from the parameter file and populates selected parameters from
    'the hru dataset
    Public Function BA_GetParameterMap(ByVal fullPath As String, ByVal token As String, ByVal intHru As Integer, _
                                       ByVal aoiPath As String) As Hashtable
        Dim parser As TextFieldParser = Nothing
        Dim hTable As Hashtable = New Hashtable
        Dim fClass As IFeatureClass = Nothing
        Dim geoDataSet As IGeoDataset = Nothing
        Dim pCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing
        Dim pArea As IArea = Nothing
        Dim pSpatialRefFactory As ISpatialReferenceFactory2 = New SpatialReferenceEnvironment
        Dim pSpatialRef As ISpatialReference
        Dim centroid As IPoint
        Dim projCoordSys As IProjectedCoordinateSystem
        Dim pGeographicCoordSys As IGeographicCoordinateSystem
        Try
            parser = New TextFieldParser(fullPath)
            parser.SetDelimiters({"" & token & ""})
            Do While parser.EndOfData = False
                Dim fields As String() = parser.ReadFields
                'Second line
                'Dim fields2 As String()
                If fields(0) = PARAM_FLAG Then
                    Dim paramName As String = Trim(fields(1))
                    Dim pValue(0) As String
                    Dim isDimension As Boolean = False
                    pValue(0) = Trim(fields(2))
                    If Not parser.EndOfData Then
                        Dim strTest As String = parser.PeekChars(20)
                        'fields2 = parser.ReadFields
                        'If fields2(0).IndexOf(BOUND) > -1 Then
                        '    'Need to put something here to handle array of parameters
                        'End If
                        'If fields2(1).Equals(DIMENSION) Then
                        '    isDimension = True
                        'End If
                        If strTest.IndexOf(BOUND) > -1 Then
                            'Need to put something here to handle array of parameters
                        End If
                        If strTest.IndexOf(DIMENSION) > -1 Then
                            isDimension = True
                        End If
                        Dim newParam As Parameter = New Parameter(paramName, pValue, isDimension)
                        hTable.Add(paramName, newParam)
                    End If
                End If
            Loop
            'nhru
            Dim updateParam As Parameter = hTable(NHRU)
            If updateParam IsNot Nothing Then
                updateParam.value(0) = intHru
                hTable(NHRU) = updateParam
            End If
            'nradpl
            Dim nradplParam As Parameter = hTable(NRADPL)
            If nradplParam IsNot Nothing Then
                nradplParam.value(0) = intHru
                hTable(NRADPL) = nradplParam
            End If
            'basin_area
            Dim folderPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi, True)
            fClass = BA_OpenFeatureClassFromGDB(folderPath, BA_EnumDescription(AOIClipFile.AOIExtentCoverage))
            If fClass IsNot Nothing Then
                pCursor = fClass.Search(Nothing, False)
                pFeature = pCursor.NextFeature
                pArea = CType(pFeature.Shape, IArea)
                Dim totalArea As Double = pArea.Area
                geoDataSet = CType(fClass, IGeoDataset)
                pSpatialRef = geoDataSet.SpatialReference
                projCoordSys = TryCast(pSpatialRef, IProjectedCoordinateSystem)
                'Total AOI area if > 1 polygon
                pFeature = pCursor.NextFeature
                Do While pFeature IsNot Nothing
                    pArea = CType(pFeature.Shape, IArea)
                    totalArea += pArea.Area
                    pFeature = pCursor.NextFeature
                Loop
                If projCoordSys IsNot Nothing Then
                    Dim pLinearUnit As ILinearUnit = projCoordSys.CoordinateUnit
                    Dim MetersPerUnit As Double = pLinearUnit.MetersPerUnit
                    updateParam = hTable(BASIN_AREA)
                    If updateParam IsNot Nothing Then
                        updateParam.value(0) = totalArea * (MetersPerUnit ^ 2) / BA_SQ_METERS_PER_ACRE
                        hTable(BASIN_AREA) = updateParam
                    End If
                End If
                'basin_lat
                centroid = (pArea.Centroid)
                pGeographicCoordSys = pSpatialRefFactory.CreateGeographicCoordinateSystem(esriSRGeoCSType.esriSRGeoCS_NAD1927)
                pSpatialRef = CType(pGeographicCoordSys, ISpatialReference)
                pSpatialRef.SetFalseOriginAndUnits(-180, -90, 1000000)
                Dim pGeo As IGeometry = CType(centroid, IGeometry)
                pGeo.Project(pSpatialRef)
                updateParam = hTable(BASIN_LAT)
                If updateParam IsNot Nothing Then
                    updateParam.value(0) = centroid.Y
                    hTable(BASIN_LAT) = updateParam
                End If
            End If
            Return hTable
        Catch ex As Exception
            Dim errMsg As String = "An error occurred while reading the template file: " & fullPath & " on or about " & _
                "line number: " & parser.LineNumber & "."
            Windows.Forms.MessageBox.Show(errMsg, "Error reading template", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
            Debug.Print("Error occurred on line number " & parser.LineNumber)
            Debug.Print("BA_GetParameterMap Exception: " & ex.Message)
            Return Nothing
        Finally
            fClass = Nothing
            geoDataSet = Nothing
            pCursor = Nothing
            pFeature = Nothing
            pArea = Nothing
            pSpatialRef = Nothing
            centroid = Nothing
            projCoordSys = Nothing
            pGeographicCoordSys = Nothing
            parser.Close()
        End Try
    End Function

    Public Function BA_GetTableMap(ByVal fullPath As String, ByVal token As String, ByVal paramTable As Hashtable) As Hashtable
        Dim parser As TextFieldParser = Nothing
        Dim hTable As Hashtable = New Hashtable
        Try
            parser = New TextFieldParser(fullPath)
            parser.SetDelimiters({"" & token & ""})
            parser.HasFieldsEnclosedInQuotes = True
            Do While parser.EndOfData = False
                Dim fields As String() = parser.ReadFields
                'Next line
                Dim nextFields As String()
                'Header line
                Dim headerFields As String() = Nothing
                If fields(0) = TABLE_FLAG Then
                    'We have a table
                    Dim newTable As ParameterTable = Nothing
                    Dim tName As String = Nothing
                    Dim dimension1 As String = Nothing
                    Dim dimension2 As String = Nothing
                    Dim values(,) As String = Nothing
                    Dim headers() As String = Nothing
                    'Peek at the next line to see if it is a multi-dimensional table
                    headerFields = parser.ReadFields
                    If Trim(headerFields(0)) = BOUND Then
                        Dim XBound As Integer = 0
                        Dim YBound As Integer = 0
                        ReDim nextFields(headerFields.GetUpperBound(0))
                        Array.Copy(headerFields, nextFields, headerFields.Length)
                        'This is a 2D table; Get some information from second line
                        tName = fields(1)
                        Dim tempDimension = Trim(nextFields(1))
                        tempDimension.Trim("""")
                        Dim d As String() = tempDimension.Split(token)
                        dimension1 = Trim(d(0))
                        dimension2 = Trim(d(1))
                        'Get the next row which may have the number of fields(X) and columns(Y)
                        Dim nextRow As String() = parser.ReadFields
                        If Trim(nextRow(0)).ToLower = LEN_KEY Then
                            'Split the line on the comma
                            Dim arrSize As String() = Trim(nextRow(1)).Split(",")
                            'Assign X and Y values
                            XBound = CInt(arrSize(1))
                            YBound = CInt(arrSize(0))
                        Else
                            'Otherwise we have to check the dimension to get the values
                            XBound = BA_GetValueForDimension(paramTable, dimension2)
                            YBound = BA_GetValueForDimension(paramTable, dimension1)
                        End If
                        'Resize headers array
                        ReDim headers(XBound - 1)
                        'Populate headers array; It is 0 - YBound
                        For i As Integer = 0 To XBound - 1
                            headers(i) = CStr(i)
                        Next
                        ReDim values(YBound - 1, XBound - 1)
                        'Skip the header row
                        nextRow = parser.ReadFields
                        'Loop through the lines to populate the values array
                        For i As Integer = 0 To YBound - 1
                            nextRow = parser.ReadFields
                            For j As Integer = 0 To headers.Length - 1
                                values(i, j) = nextRow(j + 1)
                            Next
                        Next
                    ElseIf Trim(headerFields(0)) = HEADER_FLAG Then
                        Exit Do
                    End If
                    If tName Is Nothing Then
                        'Don't know why I wasn't just using the table name
                        ''This is a regular table
                        ''We will name the table after the first column of the header row; 
                        'tName = Trim(headerFields(1))
                        dimension1 = Trim(fields(1))
                        tName = dimension1
                        Dim colHeader As String = Nothing
                        Dim count As Integer = 0
                        'How many column headers (parameters) do we have?
                        'Assume a parameter is only listed once in a file
                        Dim firstCell As String = Trim(headerFields(0))
                        'We may have to skip the description to get to the len/header
                        Do While firstCell <> LEN_KEY
                            headerFields = parser.ReadFields
                            firstCell = Trim(headerFields(0))
                        Loop
                        Dim numRows As Integer = CInt(Trim(headerFields(1)))
                        Do While firstCell <> HEADER_FLAG
                            headerFields = parser.ReadFields
                            firstCell = Trim(headerFields(0))
                        Loop

                        Dim maxColumns As Integer = headerFields.GetUpperBound(0)
                        Do While count < maxColumns
                            colHeader = Trim(headerFields(count + 1))
                            If colHeader.Length < 1 Then
                                Exit Do
                            Else
                                count += 1
                            End If
                        Loop
                        ReDim headers(count - 1)
                        'Populate the Headers array
                        For i As Integer = 0 To count - 1
                            headers(i) = Trim(headerFields(i + 1))
                        Next
                        'Redim Y, X
                        ReDim values(numRows - 1, headers.Length - 1)
                        For i As Integer = 0 To numRows - 1
                            Dim nextRow As String() = parser.ReadFields
                            For j As Integer = 0 To headers.Length - 1
                                values(i, j) = nextRow(j + 1)
                            Next
                        Next
                    Else

                    End If
                    If dimension2 Is Nothing Then
                        newTable = New ParameterTable(tName, dimension1, values, headers)
                    Else
                        newTable = New ParameterTable(tName, dimension1, dimension2, values, headers)
                    End If
                    'Add non-spatial table to table
                    If dimension1 <> NHRU Then
                        hTable.Add(tName, newTable)
                    End If
                End If
            Loop
            Return hTable
        Catch ex As Exception
            Dim errMsg As String = "An error occurred while reading the template file: " & fullPath & " on or about " & _
                "line number: " & parser.LineNumber & "."
            Windows.Forms.MessageBox.Show(errMsg, "Error reading template", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
            Debug.Print("Error occurred on line number " & parser.LineNumber)
            Debug.Print("BA_GetTableMap Exception: " & ex.Message)
            Return Nothing
        Finally
            parser.Close()
        End Try
    End Function

    Public Function BA_GetValueForDimension(ByVal paramsTable As Hashtable, ByVal dimensionName As String) As String
        Dim keys As ICollection = paramsTable.Keys
        Dim param As Parameter = Nothing
        For Each pKey As String In keys
            If pKey = dimensionName Then
                param = CType(paramsTable(pKey), Parameter)
                Dim value As String() = param.value
                Return value(0)
            End If
        Next
        Return Nothing
    End Function

    Public Function BA_GetDimensionList(ByVal inputTable As Hashtable) As IList(Of String)
        Dim dimList As IList(Of String) = Nothing
        If inputTable IsNot Nothing AndAlso inputTable.Keys.Count > 0 Then
            dimList = New List(Of String)
            Dim keys As ICollection = inputTable.Keys
            Dim param As Parameter = Nothing
            For Each pKey As String In keys
                param = CType(inputTable(pKey), Parameter)
                If param.isDimension = True Then
                    dimList.Add(param.name)
                End If
            Next
        End If
        Return dimList
    End Function

    Public Function BA_GetTablesByDimension(ByVal inputTable As Hashtable) As Hashtable
        Dim dimTable As Hashtable = Nothing
        If inputTable IsNot Nothing AndAlso inputTable.Keys.Count > 0 Then
            dimTable = New Hashtable
            Dim keys As ICollection = inputTable.Keys
            Dim paramTable As ParameterTable = Nothing
            For Each pKey As String In keys
                paramTable = CType(inputTable(pKey), ParameterTable)
                'First dimension
                Dim dim1 As String = paramTable.Dimension1
                If Not String.IsNullOrEmpty(dim1) Then
                    Dim tableList As IList(Of ParameterTable) = dimTable(dim1)
                    If tableList Is Nothing Then
                        tableList = New List(Of ParameterTable)
                    End If
                    tableList.Add(paramTable)
                    dimTable(dim1) = tableList
                End If
                'Second dimension
                Dim dim2 As String = paramTable.Dimension2
                If Not String.IsNullOrEmpty(dim2) Then
                    Dim tableList As IList(Of ParameterTable) = dimTable(dim2)
                    If tableList Is Nothing Then
                        tableList = New List(Of ParameterTable)
                    End If
                    tableList.Add(paramTable)
                    dimTable(dim2) = tableList
                End If
            Next
        End If
        Return dimTable
    End Function

    Public Function BA_ExportParameterFile(ByVal outputFile As String, ByVal description As String, ByVal pVersion As String, _
                                           ByVal parameterMap As Hashtable, ByVal tableMap As Hashtable, ByVal hruParamFolder As String, _
                                           ByVal hruParamFile As String, ByVal polygonCount As Integer, ByVal spatialTable As Hashtable, _
                                           ByVal missingValue As String, ByVal radplSpatialParameters As IList(Of String), _
                                           ByVal layerSpatialParameters As IDictionary(Of String, LayerParameter)) As BA_ReturnCode
        Dim sw As StreamWriter = Nothing
        Dim sb As StringBuilder = New StringBuilder
        Dim pTable As ITable = Nothing
        Dim pTableSort As ITableSort = New TableSort
        Dim pCursor As ICursor = Nothing
        Dim radplCursor As ICursor = Nothing
        Dim pRow As IRow = Nothing
        Dim pFields As IFields = Nothing
        Dim layerFeatClass As IFeatureClass = Nothing
        Dim layerCursor As IFeatureCursor = Nothing
        Dim layerFeature As IFeature = Nothing
        Dim layerQuery As IQueryFilter = New QueryFilter
        Try
            sw = New StreamWriter(outputFile)
            'Write section header
            sb.Append(SECTION_FLAG)
            sb.Append(",")
            sb.Append("Parameter")
            'sb.Append(",")
            sw.WriteLine(sb.ToString)
            sb.Remove(0, sb.Length)
            sb.Append(DESCR)
            sb.Append(",")
            sb.Append("""" & description & """")
            'sb.Append(",")
            sw.WriteLine(sb.ToString)
            sb.Remove(0, sb.Length)
            sb.Append(MODIFIED_AT)
            sb.Append(",")
            Dim rightNow As Date = DateTime.Now
            sb.Append(rightNow.ToString("ddd MMM d H:mm:ss %K yyyy"))
            'sb.Append(",")
            sw.WriteLine(sb.ToString)
            sb.Remove(0, sb.Length)
            sb.Append(VERSION)
            sb.Append(",")
            sb.Append(pVersion)
            'sb.Append(",")
            sw.WriteLine(sb.ToString)
            sb.Remove(0, sb.Length)
            sb.Append(CREATED_AT)
            sb.Append(",")
            sb.Append(rightNow.ToString("ddd MMM d H:mm:ss %K yyyy"))
            'sb.Append(",")
            sw.WriteLine(sb.ToString)
            sb.Remove(0, sb.Length)
            'Write all single dimension parameters
            Dim keys As ICollection = parameterMap.Keys
            'Copy collection into an array so it can be sorted
            Dim sortArray(keys.Count - 1) As String
            keys.CopyTo(sortArray, 0)
            Array.Sort(sortArray)
            Dim param As Parameter = Nothing
            For i As Integer = 0 To sortArray.Length - 1
                Dim pKey As String = sortArray(i)
                param = CType(parameterMap(pKey), Parameter)
                sb.Append(PARAM_FLAG)
                sb.Append(",")
                sb.Append(param.name)
                sb.Append(",")
                sb.Append(param.value(0))
                'sb.Append(",")
                sw.WriteLine(sb.ToString)
                sb.Remove(0, sb.Length)
                If param.isDimension = True Then
                    sb.Append(ROLE)
                    sb.Append(",")
                    sb.Append(DIMENSION)
                    'sb.Append(",")
                    sw.WriteLine(sb.ToString)
                    sb.Remove(0, sb.Length)
                End If
            Next
            'Write all tables except nhru and nradpl dimensions
            Dim tableKeys As ICollection = tableMap.Keys
            Dim paramTable As ParameterTable = Nothing
            'Sort table names so tables come out in alphabetical order
            Dim keysA() As String = GetSortedTableNames(tableKeys, tableMap)
            For Each pKey As String In keysA
                paramTable = tableMap(pKey)
                If paramTable.Dimension1 <> NHRU And _
                    paramTable.Dimension1 <> NRADPL Then
                    'Write @T line
                    sb.Append(TABLE_FLAG)
                    sb.Append(",")
                    If String.IsNullOrEmpty(paramTable.Dimension2) Then
                        sb.Append(paramTable.Dimension1)
                    Else
                        sb.Append(paramTable.Name)
                    End If
                    'sb.Append(",")
                    sw.WriteLine(sb.ToString)
                    sb.Remove(0, sb.Length)
                    If String.IsNullOrEmpty(paramTable.Dimension2) Then
                        'Regular table
                        sb.Append("description")
                        sb.Append(",")
                        sb.Append("Parameter bound by " & paramTable.Dimension1)
                        'sb.Append(",")
                        sw.WriteLine(sb.ToString)
                        sb.Remove(0, sb.Length)
                        sb.Append(LEN_KEY)
                        sb.Append(",")
                        sb.Append(paramTable.Values.GetUpperBound(0) + 1)
                        sw.WriteLine(sb.ToString)
                        sb.Remove(0, sb.Length)
                    Else
                        '2D table
                        sb.Append(BOUND)
                        sb.Append(",")
                        sb.Append("""" & paramTable.Dimension1 & ", ")
                        sb.Append(paramTable.Dimension2 & """")
                        sw.WriteLine(sb.ToString)
                        sb.Remove(0, sb.Length)
                        sb.Append(LEN_KEY)
                        sb.Append(",")
                        sb.Append("""" & paramTable.Values.GetUpperBound(0) + 1 & "," & paramTable.Values.GetUpperBound(1) + 1 & """")
                        'sb.Append(",")
                        sw.WriteLine(sb.ToString)
                        sb.Remove(0, sb.Length)
                    End If
                    'Write header @H line
                    sb.Append(HEADER_FLAG)
                    sb.Append(",")
                    For i As Integer = 0 To paramTable.Headers.Length - 1
                        sb.Append(paramTable.Headers(i))
                        sb.Append(",")
                    Next
                    'Trim trailing comma
                    sb.Remove(sb.Length - 1, 1)
                    sw.WriteLine(sb.ToString)
                    sb.Remove(0, sb.Length)
                    'Write values
                    For i As Integer = 0 To paramTable.Values.GetUpperBound(0)
                        '---insert blank cell---
                        sb.Append(",")
                        For j As Integer = 0 To paramTable.Values.GetUpperBound(1)
                            sb.Append(paramTable.Values(i, j))
                            sb.Append(",")
                        Next
                        'Trim trailing comma
                        sb.Remove(sb.Length - 1, 1)
                        sw.WriteLine(sb.ToString)
                        sb.Remove(0, sb.Length)
                    Next

                End If
            Next
            'Append nhru parameters
            If BA_Folder_ExistsWindowsIO(hruParamFolder) Then
                pTable = BA_OpenTableFromGDB(hruParamFolder, hruParamFile)
                pTableSort.Table = pTable
                pTableSort.Fields = BA_FIELD_HRU_ID
                pTableSort.Sort(Nothing)
                If pTable IsNot Nothing Then
                    'Write @T line
                    sb.Append(TABLE_FLAG)
                    sb.Append(",")
                    sb.Append(NHRU)
                    sb.Append(",")
                    sw.WriteLine(sb.ToString)
                    sb.Remove(0, sb.Length)
                    'description,Parameter bound by nhru,
                    sb.Append("description")
                    sb.Append(",")
                    sb.Append("Parameter bound by " & NHRU)
                    sb.Append(",")
                    sw.WriteLine(sb.ToString)
                    sb.Remove(0, sb.Length)
                    'missing value metadata
                    If Not String.IsNullOrEmpty(missingValue) Then
                        sb.Append(MISSING_VALUE)
                        sb.Append(",")
                        sb.Append(missingValue)
                        sw.WriteLine(sb.ToString)
                        sb.Remove(0, sb.Length)
                    End If
                    'put all parameters into a structure so we can sort them
                    Dim dictMaster As IDictionary(Of String, IDictionary(Of String, String)) =
                        New Dictionary(Of String, IDictionary(Of String, String))
                    Dim spatialParamNames As List(Of String) = New List(Of String)
                    'spatial parameters generated by BAGIS-P
                    pFields = pTable.Fields
                    For i As Integer = 0 To pFields.FieldCount - 1
                        Dim pField As Field = pFields.Field(i)
                        If IncludeField(pField, radplSpatialParameters, layerSpatialParameters) = True Then
                            spatialParamNames.Add(pField.Name)
                        End If
                    Next
                    'parameters from layer parameters
                    For Each key As String In layerSpatialParameters.Keys
                        If Not spatialParamNames.Contains(key) Then
                            spatialParamNames.Add(key)
                        End If
                    Next
                    'other spatial parameters not generated by BAGIS-p
                    For Each key As String In spatialTable.Keys
                        If Not radplSpatialParameters.Contains(key) Then
                            If Not layerSpatialParameters.ContainsKey(key) Then
                                If Not spatialParamNames.Contains(key) Then
                                    spatialParamNames.Add(key)
                                End If
                            End If
                        End If
                    Next
                    'sort the field names
                    spatialParamNames.Sort()

                    pCursor = pTableSort.Rows
                    pRow = pCursor.NextRow
                    Dim rowCount As Integer = 0
                    While pRow IsNot Nothing
                        Dim nextHruId As String = "-1"
                        Dim idxHruId As Integer = pTable.FindField(BA_FIELD_HRU_ID)
                        Dim hruEntry As IDictionary(Of String, String) = Nothing
                        If idxHruId > 0 Then
                            nextHruId = Convert.ToString(pRow.Value(idxHruId))
                            If Not dictMaster.Keys.Contains(nextHruId) Then
                                dictMaster.Add(nextHruId, New Dictionary(Of String, String))
                                hruEntry = New Dictionary(Of String, String)
                            Else
                                hruEntry = dictMaster(nextHruId)
                            End If
                        End If
                        'BAGIS-P spatial parameters
                        For j As Integer = 0 To pFields.FieldCount - 1
                            Dim nextField As IField = pRow.Fields.Field(j)
                            If IncludeField(nextField, radplSpatialParameters, layerSpatialParameters) Then
                                If Not hruEntry.Keys.Contains(nextField.Name) Then
                                    hruEntry.Add(nextField.Name, pRow.Value(j))
                                End If
                            End If
                        Next

                        'Append parameters from layers if we have any
                        If layerSpatialParameters.Keys.Count > 0 Then
                            'Extract the path to the hru gdb from the hruParamFolder so we don't have to add another variable
                            Dim hruFolder As String = "PleaseReturn"
                            Dim strTemp As String = BA_GetBareName(hruParamFolder, hruFolder)
                            Dim hruName As String = BA_GetBareName(hruFolder)
                            Dim hruGdbFolder As String = hruFolder & hruName & ".gdb"
                            layerFeatClass = BA_OpenFeatureClassFromGDB(hruGdbFolder, BA_GetBareName(BA_EnumDescription(PublicPath.HruZonesVector)))
                            If layerFeatClass IsNot Nothing Then
                                layerQuery.WhereClause = BA_FIELD_HRU_ID & " = " & nextHruId
                                layerCursor = layerFeatClass.Search(layerQuery, False)
                                If layerCursor IsNot Nothing Then
                                    'We assume there is only one feature with an id
                                    layerFeature = layerCursor.NextFeature
                                    If layerFeature IsNot Nothing Then
                                        For Each key As String In layerSpatialParameters.Keys
                                            Dim idxParam As String = layerFeatClass.FindField(key)
                                            If idxParam > -1 Then
                                                If Not hruEntry.Keys.Contains(key) Then
                                                    hruEntry.Add(key, layerFeature.Value(idxParam))
                                                End If
                                            End If
                                        Next
                                    End If
                                End If
                            End If
                        End If

                        'Append user-specified spatial param column values
                        For Each spKey As String In spatialTable.Keys
                            If Not radplSpatialParameters.Contains(spKey) Then
                                If Not layerSpatialParameters.ContainsKey(spKey) Then
                                    Dim spColumn As IList(Of String) = spatialTable(spKey)
                                    If Not hruEntry.Keys.Contains(spKey) Then
                                        hruEntry.Add(spKey, spColumn.Item(rowCount))
                                    End If
                                End If
                            End If
                        Next

                        rowCount += 1
                        dictMaster(nextHruId) = hruEntry
                        pRow = pCursor.NextRow
                    End While

                    'Print warning if there is different number of rows in parameter table than number of polygons in shapefile
                    'Increment rowCount by 1 because it is zero-based
                    If rowCount <> polygonCount Then
                        sb.Append("The number of rows in the parameter table (" & rowCount & ") differs" & vbCrLf)
                        sb.Append("from the number of polygons in the selected HRU dataset (" & polygonCount & ")." & vbCrLf)
                        sb.Append("This may cause incorrect results when running a model.")
                        Windows.Forms.MessageBox.Show(sb.ToString, "Different number of rows", _
                                                      Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Information)
                        sb.Remove(0, sb.Length)
                    End If

                    'write table headers @H
                    sb.Append(HEADER_FLAG)
                    sb.Append(",")
                    'append ERAMS ID column header
                    sb.Append(BA_FIELD_HRU_ID)
                    sb.Append(",")
                    For Each paramName As String In spatialParamNames
                        sb.Append(paramName)
                        sb.Append(",")
                    Next
                    'Trim trailing comma
                    sb.Remove(sb.Length - 1, 1)
                    sw.WriteLine(sb.ToString)
                    sb.Remove(0, sb.Length)
                    For Each hruId As String In dictMaster.Keys
                        Dim hruEntry As IDictionary(Of String, String) = dictMaster(hruId)
                        'Empty cell so the table lines up
                        sb.Append(",")
                        sb.Append(hruId)
                        sb.Append(",")
                        For Each paramName As String In spatialParamNames
                            sb.Append(hruEntry(paramName))
                            sb.Append(",")
                        Next
                        sb.Remove(sb.Length - 1, 1)
                        sw.WriteLine(sb.ToString)
                        sb.Remove(0, sb.Length)
                    Next
                    'pCursor = pTableSort.Rows
                    'pRow = pCursor.NextRow
                    'Dim rowCount As Integer = 0
                    'While pRow IsNot Nothing
                    '    'Empty cell so the table lines up
                    '    sb.Append(",")
                    '    'Dim idxERamsId As Integer = pTable.FindField(BA_FIELD_ERAMS_ID)
                    '    Dim nextHruId As String = "-1"
                    '    Dim idxHruId As Integer = pTable.FindField(BA_FIELD_HRU_ID)
                    '    If idxHruId > 0 Then
                    '        sb.Append(pRow.Value(idxHruId))
                    '        nextHruId = Convert.ToString(pRow.Value(idxHruId))
                    '    End If
                    '    sb.Append(",")
                    '    For j As Integer = 0 To pFields.FieldCount - 1
                    '        Dim nextField As IField = pRow.Fields.Field(j)
                    '        If IncludeField(nextField, radplSpatialParameters, layerSpatialParameters) Then
                    '            sb.Append(pRow.Value(j))
                    '            sb.Append(",")
                    '        End If
                    '    Next

                    '    'Append parameters from layers if we have any
                    '    If layerSpatialParameters.Keys.Count > 0 Then
                    '        'Extract the path to the hru gdb from the hruParamFolder so we don't have to add another variable
                    '        Dim hruFolder As String = "PleaseReturn"
                    '        Dim strTemp As String = BA_GetBareName(hruParamFolder, hruFolder)
                    '        Dim hruName As String = BA_GetBareName(hruFolder)
                    '        Dim hruGdbFolder As String = hruFolder & hruName & ".gdb"
                    '        layerFeatClass = BA_OpenFeatureClassFromGDB(hruGdbFolder, BA_GetBareName(BA_EnumDescription(PublicPath.HruZonesVector)))
                    '        If layerFeatClass IsNot Nothing Then
                    '            layerQuery.WhereClause = BA_FIELD_HRU_ID & " = " & nextHruId
                    '            layerCursor = layerFeatClass.Search(layerQuery, False)
                    '            If layerCursor IsNot Nothing Then
                    '                'We assume there is only one feature with an id
                    '                layerFeature = layerCursor.NextFeature
                    '                If layerFeature IsNot Nothing Then
                    '                    For Each key As String In layerSpatialParameters.Keys
                    '                        Dim idxParam As String = layerFeatClass.FindField(key)
                    '                        If idxParam > -1 Then
                    '                            sb.Append(layerFeature.Value(idxParam))
                    '                            sb.Append(",")
                    '                        End If
                    '                    Next
                    '                End If
                    '            End If
                    '        End If
                    '    End If

                    '    'Append user-specified spatial param column values
                    '    For Each spHeader As String In spatialColumns
                    '        If Not radplSpatialParameters.Contains(spHeader) Then
                    '            If Not layerSpatialParameters.ContainsKey(spHeader) Then
                    '                Dim spColumn As IList(Of String) = spatialTable(spHeader)
                    '                sb.Append(spColumn.Item(rowCount))
                    '                sb.Append(",")
                    '            End If
                    '        End If
                    '    Next
                    '    'Trim trailing comma
                    '    sb.Remove(sb.Length - 1, 1)
                    '    sw.WriteLine(sb.ToString)
                    '    sb.Remove(0, sb.Length)
                    '    pRow = pCursor.NextRow
                    '    rowCount += 1
                    'End While

                    '**** Write NRADPL table ****
                    ' This list is populated from the bagis_parameters.txt file to indicate if NRADPL table should be created
                    If radplSpatialParameters.Count > 0 Then
                        'Write @T line
                        sb.Append(TABLE_FLAG)
                        sb.Append(",")
                        sb.Append(NRADPL)
                        sb.Append(",")
                        sw.WriteLine(sb.ToString)
                        sb.Remove(0, sb.Length)
                        'description,Parameter bound by nradpl,
                        sb.Append("description")
                        sb.Append(",")
                        sb.Append("Parameter bound by " & NRADPL)
                        sb.Append(",")
                        sw.WriteLine(sb.ToString)
                        sb.Remove(0, sb.Length)
                        'missing value metadata
                        If Not String.IsNullOrEmpty(missingValue) Then
                            sb.Append(MISSING_VALUE)
                            sb.Append(",")
                            sb.Append(missingValue)
                            sw.WriteLine(sb.ToString)
                            sb.Remove(0, sb.Length)
                        End If
                        'write table headers @H
                        pFields = pTable.Fields
                        sb.Append(HEADER_FLAG)
                        sb.Append(",")
                        For i As Integer = 0 To pFields.FieldCount - 1
                            Dim pField As Field = pFields.Field(i)
                            If radplSpatialParameters.Contains(pField.Name) Then
                                sb.Append(pField.Name)
                                sb.Append(",")
                            End If
                        Next
                        'Append user-specified radpl param column headers
                        Dim spatialColumns As IList(Of String) = New List(Of String)
                        spatialColumns = New String(spatialTable.Keys.Count - 1) {}
                        Dim radplIdx As Integer = 0
                        For Each key As String In spatialTable.Keys
                            If radplSpatialParameters.Contains(key) Then
                                sb.Append(key)
                                sb.Append(",")
                                'Also store key value in a reference array because order in Hashtable is not guaranteed
                                spatialColumns(radplIdx) = key
                                radplIdx += 1
                            End If
                        Next
                        'Trim trailing comma
                        sb.Remove(sb.Length - 1, 1)
                        sw.WriteLine(sb.ToString)
                        sb.Remove(0, sb.Length)
                        radplCursor = pTableSort.Rows
                        pRow = radplCursor.NextRow
                        Dim radPlRow As Integer = 0
                        While pRow IsNot Nothing
                            'Empty cell so the table lines up
                            sb.Append(",")
                            For j As Integer = 0 To pFields.FieldCount - 1
                                Dim nextField As IField = pRow.Fields.Field(j)
                                If radplSpatialParameters.Contains(nextField.Name) Then
                                    sb.Append(pRow.Value(j))
                                    sb.Append(",")
                                End If
                            Next
                            'Append user-specified spatial param column values
                            radplIdx = 0 'Re-initalize counter
                            For Each spHeader As String In spatialColumns
                                If radplSpatialParameters.Contains(spHeader) Then
                                    Dim spColumn As IList(Of String) = spatialTable(spHeader)
                                    sb.Append(spColumn.Item(radPlRow))
                                    sb.Append(",")
                                End If
                            Next
                            'Trim trailing comma
                            sb.Remove(sb.Length - 1, 1)
                            sw.WriteLine(sb.ToString)
                            sb.Remove(0, sb.Length)
                            pRow = radplCursor.NextRow
                            radPlRow += 1
                        End While
                    End If
                End If
            End If
        Catch ex As Exception
            Debug.Print("BA_ExportParameterFile Exception: " & ex.Message)
        Finally
            If sw IsNot Nothing Then
                sw.Close()
            End If
            pTable = Nothing
            pTableSort = Nothing
            pCursor = Nothing
            radplCursor = Nothing
            pRow = Nothing
            pFields = Nothing
            layerFeatClass = Nothing
            layerCursor = Nothing
            layerFeature = Nothing
            layerQuery = Nothing
        End Try
    End Function

    'Public Function BA_AppendERamsIdToFeatureClass(ByVal hruGdbFolder As String, ByVal vName As String, _
    '                                             ByVal tableName As String) As BA_ReturnCode
    '    Dim pTable As ITable = Nothing
    '    Dim tableSort As ITableSort = New TableSort
    '    Dim cursor As ICursor = Nothing
    '    Dim updateCursor As IFeatureCursor = Nothing
    '    Dim pRow As IRow = Nothing
    '    Dim pGeoDataset As IGeoDataset = Nothing
    '    Dim pFeatureClass As IFeatureClass = Nothing
    '    Dim idxERamsId As Long = -1
    '    Dim idxFlowAccId As Long = -1
    '    Dim omsTable As Hashtable = Nothing
    '    Dim pFeature As IFeature = Nothing
    '    Try
    '        pGeoDataset = BA_OpenFeatureClassFromGDB(hruGdbFolder, vName)
    '        If pGeoDataset IsNot Nothing Then
    '            'Open grid_zones_v
    '            pFeatureClass = CType(pGeoDataset, IFeatureClass)
    '            'Get column index OMS_ID; If it doesn't exist, add it
    '            idxERamsId = pFeatureClass.FindField(BA_FIELD_ERAMS_ID)
    '            If idxERamsId < 0 Then
    '                Dim pFieldOms As IFieldEdit = New Field
    '                With pFieldOms
    '                    .Type_2 = esriFieldType.esriFieldTypeInteger
    '                    .Name_2 = BA_FIELD_ERAMS_ID
    '                End With
    '                pFeatureClass.AddField(pFieldOms)
    '                idxERamsId = pFeatureClass.FindField(BA_FIELD_ERAMS_ID)
    '            End If
    '            idxFlowAccId = pFeatureClass.FindField(BA_FIELD_FLOW_ACCUM)
    '            If idxFlowAccId < 0 Then
    '                Dim pFieldAcc As IFieldEdit = New Field
    '                With pFieldAcc
    '                    .Type_2 = esriFieldType.esriFieldTypeInteger
    '                    .Name_2 = BA_FIELD_FLOW_ACCUM
    '                End With
    '                pFeatureClass.AddField(pFieldAcc)
    '                idxFlowAccId = pFeatureClass.FindField(BA_FIELD_FLOW_ACCUM)
    '            End If
    '        End If
    '        pTable = BA_OpenTableFromGDB(hruGdbFolder, tableName)
    '        If pTable IsNot Nothing Then
    '            'Instantiate hash to store omsId's; A tableSort cursor cannot perform updates :-(
    '            omsTable = New Hashtable
    '            tableSort.Table = pTable
    '            tableSort.Fields = StatisticsFieldName.MAX.ToString
    '            tableSort.Sort(Nothing)
    '            cursor = tableSort.Rows
    '            Dim idxHruId As Integer = cursor.Fields.FindField(BA_FIELD_HRU_ID)
    '            Dim idxMaxId As Integer = cursor.Fields.FindField(StatisticsFieldName.MAX.ToString)
    '            pRow = cursor.NextRow
    '            Dim omsId As Integer = 1
    '            Do Until pRow Is Nothing
    '                Dim hruId As String = CStr(pRow.Value(idxHruId))
    '                Dim accumValue As Integer = pRow.Value(idxMaxId)
    '                Dim values As Integer() = {omsId, accumValue}
    '                omsTable(hruId) = values
    '                omsId += 1
    '                pRow = cursor.NextRow
    '            Loop
    '            If omsTable.Keys.Count > 0 Then
    '                updateCursor = pFeatureClass.Update(New QueryFilter(), False)
    '                idxHruId = updateCursor.Fields.FindField(BA_FIELD_HRU_ID)
    '                pFeature = updateCursor.NextFeature
    '                Do Until pFeature Is Nothing
    '                    Dim hruId As String = Convert.ToString(pFeature.Value(idxHruId))
    '                    Dim values As Integer() = omsTable(hruId)
    '                    pFeature.Value(idxERamsId) = values(0)
    '                    pFeature.Value(idxFlowAccId) = values(1)
    '                    updateCursor.UpdateFeature(pFeature)
    '                    pFeature = updateCursor.NextFeature
    '                Loop
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Debug.Print("BA_AppendERamsId Exception: " & ex.Message)
    '    Finally
    '        pTable = Nothing
    '        tableSort = Nothing
    '        cursor = Nothing
    '        updateCursor = Nothing
    '        pRow = Nothing
    '        pFeatureClass = Nothing
    '        pFeature = Nothing
    '        pGeoDataset = Nothing
    '        GC.WaitForPendingFinalizers()
    '        GC.Collect()
    '    End Try
    'End Function

    'Public Function BA_FeatureClassHasERamsId(ByVal pFolder As String, ByVal pFile As String) As Boolean
    '    Dim pGeoDataset As IGeoDataset = Nothing
    '    Dim pFeatureClass As IFeatureClass = Nothing
    '    Dim cursor As ICursor = Nothing
    '    Dim pRow As IRow = Nothing
    '    Dim idxERamsId As Long = -1

    '    Try
    '        pGeoDataset = BA_OpenFeatureClassFromGDB(pFolder, pFile)
    '        If pGeoDataset IsNot Nothing Then
    '            'Open grid_zones_v
    '            pFeatureClass = CType(pGeoDataset, IFeatureClass)
    '            'Get column index OMS_ID; If it doesn't exist, add it
    '            idxERamsId = pFeatureClass.FindField(BA_FIELD_ERAMS_ID)
    '            If idxERamsId < 0 Then
    '                Dim pFieldOms As IFieldEdit = New Field
    '                With pFieldOms
    '                    .Type_2 = esriFieldType.esriFieldTypeInteger
    '                    .Name_2 = BA_FIELD_ERAMS_ID
    '                End With
    '                pFeatureClass.AddField(pFieldOms)
    '                idxERamsId = pFeatureClass.FindField(BA_FIELD_ERAMS_ID)
    '            Else
    '                'The column already exists
    '                cursor = pFeatureClass.Search(Nothing, Nothing)
    '                If cursor IsNot Nothing Then
    '                    pRow = cursor.NextRow
    '                    'And is populated so we don't need to proceed
    '                    If pRow IsNot Nothing Then
    '                        If Not IsDBNull(pRow.Value(idxERamsId)) AndAlso pRow.Value(idxERamsId) > 0 Then
    '                            Return True
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        End If
    '        Return False
    '    Catch ex As Exception
    '        Debug.Print("BA_FeatureClassHasERamsId Exception: " & ex.Message)
    '        Return False
    '    Finally
    '        pGeoDataset = Nothing
    '        pFeatureClass = Nothing
    '        cursor = Nothing
    '        pRow = Nothing
    '        GC.WaitForPendingFinalizers()
    '        GC.Collect()
    '    End Try
    'End Function

    'Public Sub BA_AppendERamsIdToParameterTable(ByVal hruGdbPath As String, ByVal tableName As String)
    '    Dim pGeoDataSet As IGeoDataset = Nothing
    '    Dim pFeatureClass As IFeatureClass = Nothing
    '    Dim pFeatureCursor As IFeatureCursor = Nothing
    '    Dim pFeature As IFeature
    '    Dim pTable As ITable = Nothing
    '    Dim pCursor As ICursor = Nothing
    '    Dim pRow As IRow = Nothing
    '    Dim xRefTable As Hashtable = Nothing
    '    Try
    '        pGeoDataSet = BA_OpenFeatureClassFromGDB(hruGdbPath, BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruZonesVector), False))
    '        If pGeoDataSet IsNot Nothing Then
    '            pFeatureClass = CType(pGeoDataSet, IFeatureClass)
    '            Dim idxFCHruId As Integer = pFeatureClass.FindField(BA_FIELD_HRU_ID)
    '            Dim idxFCERamsId As Integer = pFeatureClass.FindField(BA_FIELD_ERAMS_ID)
    '            'If either of the source columns are missing; Stop processing
    '            If idxFCHruId < 1 Or idxFCERamsId < 1 Then
    '                Debug.Print("Missing source column from feature class")
    '                Exit Sub
    '            End If
    '            xRefTable = New Hashtable
    '            pFeatureCursor = pFeatureClass.Search(Nothing, False)
    '            pFeature = pFeatureCursor.NextFeature
    '            'Loop through feature class and put omsId into a reference table
    '            Do Until pFeature Is Nothing
    '                Dim hruId As String = CStr(pFeature.Value(idxFCHruId))
    '                Dim omsId As Integer = pFeature.Value(idxFCERamsId)
    '                xRefTable(hruId) = omsId
    '                pFeature = pFeatureCursor.NextFeature
    '            Loop

    '            Dim hruPath As String = "PleaseReturn"
    '            Dim tempFile As String = BA_GetBareName(hruGdbPath, hruPath)
    '            Dim tableFolder As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb)
    '            pTable = BA_OpenTableFromGDB(tableFolder, tableName)
    '            If pTable IsNot Nothing Then
    '                Dim idxTHRUId As Integer = pTable.FindField(BA_FIELD_HRU_ID)
    '                'Check to see if the OMS_ID field exists; If it doesn't, add it
    '                Dim idxTERamsId As Integer = pTable.FindField(BA_FIELD_ERAMS_ID)
    '                If idxTERamsId < 1 Then
    '                    Dim pFieldERams As IFieldEdit = New Field
    '                    With pFieldERams
    '                        .Type_2 = esriFieldType.esriFieldTypeInteger
    '                        .Name_2 = BA_FIELD_ERAMS_ID
    '                    End With
    '                    pTable.AddField(pFieldERams)
    '                    idxTERamsId = pTable.FindField(BA_FIELD_ERAMS_ID)
    '                End If

    '                pCursor = pTable.Update(Nothing, False)
    '                pRow = pCursor.NextRow
    '                Do Until pRow Is Nothing
    '                    Dim hruId As String = CStr(pRow.Value(idxTHRUId))
    '                    Dim omsId As Integer = xRefTable(hruId)
    '                    If omsId > 0 Then
    '                        pRow.Value(idxTERamsId) = omsId
    '                        pCursor.UpdateRow(pRow)
    '                    End If
    '                    pRow = pCursor.NextRow
    '                Loop
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Debug.Print("BA_AppendERamsIdToParameterTable Exception " & ex.Message)
    '    Finally
    '        pGeoDataSet = Nothing
    '        pFeatureClass = Nothing
    '        pFeatureCursor = Nothing
    '        pFeature = Nothing
    '        pTable = Nothing
    '        pCursor = Nothing
    '        pRow = Nothing
    '    End Try
    'End Sub

    Public Function BA_GetParameterDescriptionPath() As String
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim settingsPath As String = bExt.SettingsPath
        If settingsPath IsNot Nothing Then
            settingsPath = settingsPath & BA_EnumDescription(PublicPath.BagisPParameterDescriptions)
            Dim parentFolder As String = ""
            Dim fileName As String = BA_GetBareName(settingsPath, parentFolder)
            If Not BA_Folder_ExistsWindowsIO(parentFolder) Then
                fileName = BA_GetBareName(parentFolder)
                Dim newPath As String = BA_CreateFolder(bExt.SettingsPath, fileName)
                If newPath IsNot Nothing Then
                    Return settingsPath
                End If
            Else
                Return settingsPath
            End If
        End If
        Return Nothing
    End Function

    Public Function BA_GetParameterDescriptionHash(ByVal fullPath As String, ByVal token As String) As Hashtable
        Dim parser As TextFieldParser = Nothing
        Dim hTable As Hashtable = New Hashtable
        Try
            parser = New TextFieldParser(fullPath)
            parser.SetDelimiters({"" & token & ""})
            Do While parser.EndOfData = False
                Dim fields As String() = parser.ReadFields
                Dim key As String = Trim(fields(0))
                hTable(key) = Trim(fields(1))
            Loop
            Return hTable
        Catch ex As Exception
            Debug.Print("BA_GetParameterDescriptionHash Exception:" & ex.Message)
            Return Nothing
        Finally
            parser.Close()
        End Try
    End Function

    'Returns the value from a raster at the location of a point
    'Assumes there is only one point in the file
    Public Function BA_QueryRasterFromPoint(ByVal pointFileFolder As String, ByVal pointFileName As String, _
                                            ByVal rasterFolder As String, ByVal rasterName As String) As Integer
        Dim pFeatureClass As IFeatureClass = Nothing
        Dim pExtract As IExtractionOp2 = New RasterExtractionOp
        Dim pRasterDS As IGeoDataset = Nothing
        Dim pTable As ITable = Nothing
        Dim pCursor As ICursor = Nothing
        Dim pRow As IRow = Nothing

        Try
            pFeatureClass = BA_OpenFeatureClassFromGDB(pointFileFolder, pointFileName)
            pRasterDS = BA_OpenRasterFromGDB(rasterFolder, rasterName)
            If pRasterDS IsNot Nothing Then
                pTable = pExtract.Sample(pFeatureClass, pRasterDS, esriGeoAnalysisResampleEnum.esriGeoAnalysisResampleNearest)
                'The field name is always different because the source raster is stored in memory
                'We will assume that the value we want is always in the last field; Fingers crossed!
                Dim idxFlow As Integer = pTable.Fields.FieldCount - 1
                If idxFlow > -1 Then
                    pCursor = pTable.Search(Nothing, False)
                    pRow = pCursor.NextRow
                    Return Convert.ToInt32(pRow.Value(idxFlow))
                End If
            End If
            Return -1
        Catch ex As Exception
            Debug.Print("BA_QueryRasterFromPoint Exception: " & ex.Message)
            Return -1
        Finally
            pFeatureClass = Nothing
            pExtract = Nothing
            pRasterDS = Nothing
            pTable = Nothing
            pCursor = Nothing
            pRow = Nothing
        End Try
    End Function

    'Sort the selected subbasins's by maxAccumulation value
    'And assign an ID
    Public Sub BA_CalculateSubbasinId(ByRef subAoiTable As Hashtable)
        Dim sortedArray(subAoiTable.Keys.Count - 1) As Subbasin
        Dim i As Integer = 0
        For Each sName As String In subAoiTable.Keys
            Dim sAoi As Subbasin = subAoiTable(sName)
            sortedArray(i) = sAoi
            i += 1
        Next
        System.Array.Sort(sortedArray, Subbasin.maxAccumAscending)
        Dim id As Integer = 1
        For j As Integer = 0 To sortedArray.GetUpperBound(0)
            Dim sAoi As Subbasin = sortedArray(j)
            sAoi.Id = id
            subAoiTable(sAoi.Name) = sAoi
            id += 1
        Next
    End Sub

    Public Sub BA_UpdateSubbasinAttributeTable(ByVal folderName As String, ByVal fileName As String,
                                             ByVal subbasinTable As Hashtable, ByVal maxSubBasinId As Int16,
                                             ByVal whereClause As String)
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pTable As ITable = Nothing
        Dim pCursor As ICursor = Nothing
        Dim pQF As IQueryFilter = New QueryFilter
        Dim pRow As IRow = Nothing
        Try
            pRasterBandCollection = BA_OpenRasterFromGDB(folderName, fileName)
            If pRasterBandCollection IsNot Nothing Then
                pRasterBand = pRasterBandCollection.Item(0)
                pTable = pRasterBand.AttributeTable
                'Create update fields if they don't exist
                Dim idxSubbasinId As Integer = -1
                Dim idxName As Integer = -1
                Dim idxGaugeNumber As Integer = -1

                idxSubbasinId = pTable.FindField(BA_FIELD_HRU_SUBBASIN)
                If idxSubbasinId < 0 Then
                    Dim pFieldId As IFieldEdit = New Field
                    With pFieldId
                        .Type_2 = esriFieldType.esriFieldTypeInteger
                        .Name_2 = BA_FIELD_HRU_SUBBASIN
                    End With
                    pTable.AddField(pFieldId)
                    idxSubbasinId = pTable.FindField(BA_FIELD_HRU_SUBBASIN)
                End If

                idxName = pTable.FindField(BA_FIELD_SUBBASIN_NAME)
                If idxName < 0 Then
                    Dim pFieldName As IFieldEdit = New Field
                    With pFieldName
                        .Type_2 = esriFieldType.esriFieldTypeString
                        .Name_2 = BA_FIELD_SUBBASIN_NAME
                    End With
                    pTable.AddField(pFieldName)
                    idxName = pTable.FindField(BA_FIELD_SUBBASIN_NAME)
                End If

                idxGaugeNumber = pTable.FindField(BA_FIELD_GAUGE_NUMBER)
                If idxGaugeNumber < 0 Then
                    Dim pFieldGauge As IFieldEdit = New Field
                    With pFieldGauge
                        .Type_2 = esriFieldType.esriFieldTypeString
                        .Name_2 = BA_FIELD_GAUGE_NUMBER
                    End With
                    pTable.AddField(pFieldGauge)
                    idxGaugeNumber = pTable.FindField(BA_FIELD_GAUGE_NUMBER)
                End If

                For Each pName As String In subbasinTable.Keys
                    Dim sBasin As Subbasin = subbasinTable(pName)
                    Dim sb As StringBuilder = New StringBuilder
                    sb.Append(BA_FIELD_VALUE & " IN (")
                    Dim valuesList As IList(Of Integer) = sBasin.CombineValueList
                    For Each sVal As Integer In valuesList
                        sb.Append(sVal & ",")
                    Next
                    'Trim trailing comma
                    sb.Remove(sb.Length - 1, 1)
                    sb.Append(" )")
                    pQF.WhereClause = sb.ToString
                    pCursor = pTable.Update(pQF, False)
                    pRow = pCursor.NextRow
                    Do Until pRow Is Nothing
                        pRow.Value(idxSubbasinId) = sBasin.Id
                        pRow.Value(idxName) = sBasin.Name
                        pRow.Value(idxGaugeNumber) = sBasin.GaugeNumber
                        pCursor.UpdateRow(pRow)
                        pRow = pCursor.NextRow
                    Loop
                Next
                ' Update the cells that aren't in a subbasin
                pQF.WhereClause = whereClause
                pCursor = pTable.Update(pQF, False)
                pRow = pCursor.NextRow  'there will only be one row
                If pRow IsNot Nothing Then
                    pRow.Value(idxSubbasinId) = maxSubBasinId + 1
                    pRow.Value(idxName) = "Undefined"
                    pRow.Value(idxGaugeNumber) = "Unknown"
                    pCursor.UpdateRow(pRow)
                End If
            End If
        Catch ex As Exception
            Debug.Print("BA_UpdateSubbasinAttributeTable Exception: " & ex.Message)
        Finally
            pRasterBand = Nothing
            pRasterBandCollection = Nothing
            pCursor = Nothing
            pRow = Nothing
            pTable = Nothing
        End Try
    End Sub

    Public Sub BA_AppendSubbasinIdToParameterTable(ByVal aoiPath As String, ByVal hruName As String, ByVal subAoiLayerName As String,
                                                 ByVal paramTableName As String, ByVal snapRasterPath As String)
        'Run zonal stats with Marjority option to create table
        Dim zoneFilePath As String = BA_GetHruPathGDB(aoiPath, PublicPath.HruDirectory, hruName)
        Dim zoneFileName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruZonesVector), False, False)
        Dim valueFilePath As String = aoiPath & BA_EnumDescription(PublicPath.BagisSubAoiGdb)
        Dim tableName As String = "tmpSubbasin"
        Dim subTable As ITable = Nothing
        Dim subCursor As ICursor = Nothing
        Dim subRow As IRow = Nothing
        Dim pTable As ITable = Nothing
        Dim pCursor As ICursor = Nothing
        Dim pRow As IRow = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pRasterTable As ITable = Nothing
        Dim pRasterCursor As ICursor = Nothing
        Dim pRasterRow As IRow = Nothing

        Try
            Dim success As BA_ReturnCode = BA_ZonalStatisticsAsTable(zoneFilePath, zoneFileName, BA_FIELD_HRU_ID, valueFilePath & "\" & subAoiLayerName,
                                                zoneFilePath, tableName, snapRasterPath, StatisticsTypeString.MAJORITY)
            'Put values in a Hashtable that can be queried
            Dim idTable As Hashtable = New Hashtable
            Dim subbasinTable As Hashtable = New Hashtable  'Secondary lookup hashtable for hru_subbasin values
            If success = BA_ReturnCode.Success Then
                subTable = BA_OpenTableFromGDB(zoneFilePath, tableName)
                If subTable IsNot Nothing Then
                    subCursor = subTable.Search(Nothing, False)
                    subRow = subCursor.NextRow
                    Dim idxMajority As Integer = subTable.FindField(StatisticsFieldName.MAJORITY.ToString)
                    Dim idxHruId As Integer = subTable.FindField(BA_FIELD_HRU_ID)
                    Do Until subRow Is Nothing
                        Dim key As String = Convert.ToString(subRow.Value(idxHruId))
                        Dim majValue As String = Convert.ToString(subRow.Value(idxMajority))
                        'Only add subAOIId to table if it's valid
                        If majValue > -1 Then
                            idTable(key) = majValue
                        End If
                        subRow = subCursor.NextRow
                    Loop
                    'Delete table
                    success = BA_Remove_TableFromGDB(zoneFilePath, tableName)
                    'Populate the subbasin hashtable with the hru_subbasin value rather than the value (id) resulting 
                    'from zonal statistics
                    pRasterBandCollection = BA_OpenRasterFromGDB(valueFilePath, subAoiLayerName)
                    If pRasterBandCollection IsNot Nothing Then
                        pRasterBand = pRasterBandCollection.Item(0)
                        pRasterTable = pRasterBand.AttributeTable
                        pRasterCursor = pRasterTable.Search(Nothing, False)
                        pRasterRow = pRasterCursor.NextRow
                        Dim idxValueId = pRasterTable.FindField(BA_FIELD_VALUE)
                        Dim idxSubbasinId = pRasterTable.FindField(BA_FIELD_HRU_SUBBASIN)
                        Do Until pRasterRow Is Nothing
                            Dim value As String = CStr(pRasterRow.Value(idxValueId))
                            Dim hrusubbasin As String = CStr(pRasterRow.Value(idxSubbasinId))
                            For Each key As String In idTable.Keys
                                If idTable(key).Equals(value) Then
                                    subbasinTable(key) = hrusubbasin    'Add hru_subbasin value to subbasin table
                                End If
                            Next
                            pRasterRow = pRasterCursor.NextRow
                        Loop
                    End If
                    If success = BA_ReturnCode.Success Then
                        Dim hruPath As String = "PleaseReturn"
                        Dim tempStr As String = BA_GetBareName(zoneFilePath, hruPath)
                        Dim tableFolder As String = hruPath & BA_GetBareName(BA_EnumDescription(PublicPath.BagisParamGdb))
                        pTable = BA_OpenTableFromGDB(tableFolder, paramTableName)
                        If pTable IsNot Nothing Then
                            Dim idxPHruId As Integer = pTable.FindField(BA_FIELD_HRU_ID)
                            Dim idxPSubId As Integer = pTable.FindField(BA_FIELD_HRU_SUBBASIN)
                            'Check to see if the HRU_SUBBASIN field exists; If it doesn't, add it
                            If idxPSubId < 1 Then
                                Dim pFieldSub As IFieldEdit = New Field
                                With pFieldSub
                                    .Type_2 = esriFieldType.esriFieldTypeInteger
                                    .Name_2 = BA_FIELD_HRU_SUBBASIN
                                End With
                                pTable.AddField(pFieldSub)
                                idxPSubId = pTable.FindField(BA_FIELD_HRU_SUBBASIN)
                            End If
                            'Cursor through rows on parameterTable
                            pCursor = pTable.Update(Nothing, False)
                            pRow = pCursor.NextRow
                            Do Until pRow Is Nothing
                                Dim strHruId As String = Convert.ToString(pRow.Value(idxPHruId))
                                Dim subId As Integer = subbasinTable(strHruId)
                                'If HRU_ID doesn't exist in hashtable, populate with a zero
                                pRow.Value(idxPSubId) = subId
                                pCursor.UpdateRow(pRow)
                                pRow = pCursor.NextRow
                            Loop
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Debug.Print("BA_AppendSubbasinIdToParameterTable Exception: " & ex.Message)
        Finally
            subTable = Nothing
            subCursor = Nothing
            subRow = Nothing
            pTable = Nothing
            pCursor = Nothing
            pRow = Nothing
            pRasterBandCollection = Nothing
            pRasterBand = Nothing
            pRasterTable = Nothing
            pRasterCursor = Nothing
            pRasterRow = Nothing
        End Try
    End Sub

    Public Function BA_CreateHruIdField(ByVal filePath As String, ByVal fileName As String) As BA_ReturnCode
        Dim pFeatureClass As IFeatureClass = Nothing
        Dim pFieldHruId As IFieldEdit = New Field
        Dim pFieldGridCode As IField = Nothing
        Dim pCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing
        Try
            'Open feature class
            pFeatureClass = BA_OpenFeatureClassFromGDB(filePath, fileName)
            If pFeatureClass Is Nothing Then
                Return BA_ReturnCode.ReadError
            End If
            'Get indexes for fields
            Dim idxHruId As Long = pFeatureClass.FindField(BA_FIELD_HRU_ID)
            Dim idxGridCode As Long = pFeatureClass.FindField(BA_FIELD_GRIDCODE_GDB)
            'Need to check for 2 different grid code field names because ESRI changed the field name in 10.2
            If idxGridCode = -1 Then
                idxGridCode = pFeatureClass.FindField(BA_FIELD_GRIDCODE)
            End If
            'Get GridCode field so we can access the type
            If idxGridCode > 0 Then
                pFieldGridCode = pFeatureClass.Fields.Field(idxGridCode)
            Else
                Return BA_ReturnCode.ReadError
            End If
            If idxHruId < 0 Then
                'Create field for HRU_ID
                With pFieldHruId
                    'Set the type to the same type as the source field
                    .Type_2 = pFieldGridCode.Type
                    .Name_2 = BA_FIELD_HRU_ID
                End With
                pFeatureClass.AddField(pFieldHruId)
                'Set index for HRU_ID field
                idxHruId = pFeatureClass.FindField(BA_FIELD_HRU_ID)
            End If

            'Get the first record
            pCursor = pFeatureClass.Update(Nothing, False)
            pFeature = pCursor.NextFeature
            Do While Not pFeature Is Nothing
                'Populate the HRU_ID column with the value from the GRID_CODE column
                pFeature.Value(idxHruId) = pFeature.Value(idxGridCode)
                'Save changes
                pCursor.UpdateFeature(pFeature)
                pFeature = pCursor.NextFeature
            Loop
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_CreateHruIdField Exception" & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pFeatureClass = Nothing
            pFieldHruId = Nothing
            pFieldGridCode = Nothing
            pCursor = Nothing
        End Try

    End Function

    Public Function BA_CreateShapefileWithParameters(ByVal aoiPath As String, ByVal hruName As String, _
                                                     ByVal profileName As String, _
                                                     ByVal outputFolder As String, ByVal outputFile As String) As BA_ReturnCode

        Try
            Dim hruGdbPath As String = BA_GetHruPathGDB(aoiPath, PublicPath.HruDirectory, hruName)
            Dim featureClassName As String = BA_GetBareName(BA_EnumDescription(PublicPath.HruZonesVector))
            outputFolder = BA_StandardizePathString(outputFolder, False)
            'Delete old shapefile if it exists
            If BA_Shapefile_Exists(outputFolder & "\" & outputFile) Then
                BA_Remove_Shapefile(outputFolder, BA_StandardizeShapefileName(outputFile, False))
            End If
            'Copy the features from the GDB to a shapefile
            Dim success As BA_ReturnCode = BA_ConvertGDBToShapefile(hruGdbPath, featureClassName, outputFolder, outputFile)
            If success = BA_ReturnCode.Success Then
                Dim hruParamGdbPath As String = BA_GetHruPath(aoiPath, PublicPath.HruDirectory, hruName) & BA_EnumDescription(PublicPath.BagisParamGdb)
                Dim tableName As String = profileName & BA_PARAM_TABLE_SUFFIX
                'Join the shapefile to the parameter table
                success = BA_JoinField(outputFolder & "\" & outputFile, BA_FIELD_HRU_ID, hruParamGdbPath & "\" & tableName, BA_FIELD_HRU_ID, Nothing)
                Return success
            End If
            Return BA_ReturnCode.UnknownError
        Catch ex As Exception
            Debug.Print("BA_CreateShapefileWithParameter Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Public Function BA_IsNonContiguousHru(ByVal pFolder As String, ByVal vName As String, rName As String) As Boolean
        Dim pGeoDataSet As IGeoDataset = Nothing
        Dim pFeatureClass As IFeatureClass = Nothing
        Dim pFeatureCursor As IFeatureCursor = Nothing
        Dim pDataStatistics As IDataStatistics = New DataStatistics
        Dim statisticsResults As ESRI.ArcGIS.esriSystem.IStatisticsResults = Nothing
        Dim pRasterDataset As IRasterDataset = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand
        Dim pTable As ITable

        Try
            pGeoDataSet = BA_OpenFeatureClassFromGDB(pFolder, vName)
            If pGeoDataSet IsNot Nothing Then
                'Get a record count from the feature class
                pFeatureClass = CType(pGeoDataSet, IFeatureClass)
                pFeatureCursor = pFeatureClass.Search(Nothing, False)
                'initialize properties for the dataStatistics interface
                pDataStatistics.Field = BA_FIELD_HRU_ID
                pDataStatistics.Cursor = pFeatureCursor
                'Get the result statistics
                statisticsResults = pDataStatistics.Statistics
                'Get a record count from the raster
                Dim rasterCount As Integer
                pRasterDataset = CType(BA_OpenRasterFromGDB(pFolder, rName), IRasterDataset)
                pRasterBandCollection = CType(pRasterDataset, IRasterBandCollection)
                pRasterBand = pRasterBandCollection.Item(0)
                pTable = pRasterBand.AttributeTable
                If pTable IsNot Nothing Then
                    rasterCount = pTable.RowCount(Nothing)
                End If
                'If the record count <> unique values for HRU_ID we have > 1 record with the
                'same HRU_ID and therefore this HRU is non-contiguous
                If statisticsResults.Count > rasterCount Then
                    Return True
                Else
                    Return False
                End If
            End If
        Catch ex As Exception
            Debug.Print("BA_IsNonContiguousHru Exception: " & ex.Message)
            Return False
        Finally
            pGeoDataSet = Nothing
            pFeatureClass = Nothing
            pFeatureCursor = Nothing
            pDataStatistics = Nothing
            statisticsResults = Nothing
            pRasterDataset = Nothing
            pRasterBandCollection = Nothing
            pRasterBand = Nothing
            pTable = Nothing
        End Try
    End Function

    Public Function BA_ReadNhruParams(ByVal fullPath As String, ByVal token As String, ByVal newHruCount As Integer, _
                                      ByVal reqSpatialParameters As IList(Of String), _
                                      ByVal missingSpatialParameters As IList(Of String), ByVal missingValue As Integer) As Hashtable
        Dim parser As TextFieldParser = Nothing
        Dim pTable As Hashtable = New Hashtable
        Try
            parser = New TextFieldParser(fullPath)
            parser.SetDelimiters({"" & token & ""})
            parser.HasFieldsEnclosedInQuotes = True
            Do While parser.EndOfData = False
                Dim fields As String() = parser.ReadFields
                'Header line
                Dim headerFields As String() = Nothing
                If fields(0) = TABLE_FLAG Then
                    Dim tName As String = Trim(fields(1))
                    If tName = NHRU Then
                        headerFields = parser.ReadFields
                        Dim firstCell As String = Trim(headerFields(0))
                        'We may have to skip the description to get to the len/header
                        Do While firstCell <> LEN_KEY
                            headerFields = parser.ReadFields
                            firstCell = Trim(headerFields(0))
                        Loop
                        Dim templateHruCount As Integer = CInt(Trim(headerFields(1)))
                        Do While firstCell <> HEADER_FLAG
                            headerFields = parser.ReadFields
                            firstCell = Trim(headerFields(0))
                        Loop
                        Dim colHeader As String = tName
                        Dim count As Integer = 0
                        'How many column headers (parameters) do we have?
                        Dim maxColumns As Integer = headerFields.GetUpperBound(0)
                        Do While count < maxColumns
                            colHeader = Trim(headerFields(count + 1))
                            If colHeader.Length < 1 Then
                                Exit Do
                            Else
                                count += 1
                            End If
                        Loop
                        Dim headers(count - 1) As String
                        'Create an entry in the Table for each parameter; Column name is the key
                        For i As Integer = 0 To count - 1
                            Dim headerField As String = Trim(headerFields(i + 1))
                            Dim column As IList(Of String) = New List(Of String)
                            pTable.Add(headerField, column)
                        Next
                        Dim EOF As Boolean
                        Dim lastRow As String() = Nothing
                        Dim nextRow As String() = Nothing
                        Dim rowCount As Integer = templateHruCount
                        If newHruCount < templateHruCount Then rowCount = newHruCount
                        For i As Integer = 1 To rowCount
                            Try
                                If EOF = False Then
                                    nextRow = parser.ReadFields
                                    'Look at the second cell, first is always empty
                                    If String.IsNullOrEmpty(nextRow(1)) Then
                                        'This row does not contain the parameter information
                                        'Use data from last row if we have it
                                        If lastRow IsNot Nothing Then
                                            Array.Copy(lastRow, nextRow, lastRow.Length)
                                        Else
                                            'Declare the array with null values at each position
                                            nextRow = New String(headers.Length) {}
                                        End If
                                        ' We are at the end of the parameters section
                                        EOF = True
                                    Else
                                        'Copy the values into lastRow in case we have fewer hru in template than in editor
                                        lastRow = New String(headers.Length) {}
                                        Array.Copy(nextRow, lastRow, nextRow.Length)
                                    End If
                                Else
                                    'Use data from last row if we have it
                                    If lastRow IsNot Nothing Then
                                        Array.Copy(lastRow, nextRow, lastRow.Length)
                                    Else
                                        'Declare the array with null values at each position
                                        nextRow = New String(headers.Length) {}
                                    End If
                                End If
                            Catch ex As Exception
                                'No next line, we are at EOF
                                EOF = True
                                'Declare the array with null values at each position
                                nextRow = New String(headers.Length) {}
                            End Try

                            Dim idx As Integer = 1
                            '---add value to each column---
                            For j As Integer = 0 To count - 1
                                Dim headerField As String = Trim(headerFields(j + 1))
                                Dim column As IList(Of String) = pTable(Trim(headerField))
                                column.Add(nextRow(idx))
                                idx += 1
                            Next
                        Next
                        'Add extra rows if newHruCount > rowCount
                        If newHruCount > rowCount Then
                            Dim hruIdField As String = Trim(headerFields(1))
                            Dim hruIdColumn As IList(Of String) = pTable(hruIdField)
                            Dim idx As Integer = rowCount + 1
                            Do Until hruIdColumn.Count = newHruCount
                                'Add hru id
                                Dim column As IList(Of String) = pTable(hruIdField)
                                column.Add(idx)
                                '---add value to each column (except hru id)---
                                For j As Integer = 1 To count - 1
                                    Dim headerField As String = Trim(headerFields(j + 1))
                                    column = pTable(Trim(headerField))
                                    column.Add(lastRow(j + 1))
                                Next
                                hruIdColumn = pTable(hruIdField)
                                idx += 1
                            Loop
                        End If
                        'Remove columns that are generated by BAGIS-P
                        If reqSpatialParameters IsNot Nothing Then
                            Dim columnNames As IList(Of String) = New List(Of String)
                            For Each key As String In pTable.Keys
                                columnNames.Add(key)
                            Next
                            For Each pName As String In columnNames
                                If reqSpatialParameters.Contains(pName) Then
                                    pTable.Remove(pName)
                                End If
                                'Don't duplicate HRU_ID if it's in the template
                                If pName.Equals(BA_FIELD_HRU_ID) Then pTable.Remove(pName)
                            Next
                        End If
                        If missingSpatialParameters IsNot Nothing Then
                            For Each pName In missingSpatialParameters
                                Dim column As IList(Of String) = New List(Of String)
                                For i As Integer = 1 To newHruCount
                                    column.Add(CStr(missingValue))
                                Next
                                pTable(pName) = column
                            Next
                        End If
                    End If
                End If
            Loop
            Return pTable
        Catch ex As Exception
            Dim errMsg As String = "An error occurred while reading the template file: " & fullPath & " on or about " & _
                "line number: " & parser.LineNumber & "."
            Windows.Forms.MessageBox.Show(errMsg, "Error reading template", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
            Debug.Print("Error occurred on line number " & parser.LineNumber)
            Debug.Print("BA_ReadNhruParams: " & ex.Message)
            Return pTable
        Finally
            parser.Close()
        End Try
    End Function

    Public Function BA_ReplaceNoDataValuesInTable(ByVal folderPath As String, ByVal tableName As String, ByVal replaceValue As String) As BA_ReturnCode
        Dim pTable As ITable
        Dim pCursor As ICursor
        Dim nextRow As IRow
        Try
            Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(folderPath)
            If workspaceType = BAGIS_ClassLibrary.WorkspaceType.Geodatabase Then
                pTable = BA_OpenTableFromGDB(folderPath, tableName)
            Else
                pTable = BA_OpenTableFromFile(folderPath, tableName)
            End If
            If pTable IsNot Nothing Then
                pCursor = pTable.Update(Nothing, False)
                nextRow = pCursor.NextRow
                Do While nextRow IsNot Nothing
                    Dim dirtyFlag As Boolean = False
                    For i As Integer = 0 To nextRow.Fields.FieldCount - 1
                        If IsDBNull(nextRow.Value(i)) Then
                            nextRow.Value(i) = replaceValue
                            dirtyFlag = True
                        End If
                    Next
                    If dirtyFlag = True Then
                        pCursor.UpdateRow(nextRow)
                    End If
                    nextRow = pCursor.NextRow
                Loop
                Return BA_ReturnCode.Success
            End If
            Return BA_ReturnCode.UnknownError
        Catch ex As Exception
            Debug.Print("BA_ReplaceNoDataValuesInTable Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pTable = Nothing
            pCursor = Nothing
        End Try
    End Function

    Private Function IncludeField(ByVal pField As IField, ByVal radplSpatialParameters As IList(Of String), _
                                  ByVal layerSpatialParameters As IDictionary(Of String, LayerParameter)) As Boolean
        If pField.Type <> esriFieldType.esriFieldTypeOID Then
            If pField.Name <> BA_FIELD_HRU_ID Then
                If radplSpatialParameters.Contains(pField.Name) Then
                    Return False
                Else
                    If layerSpatialParameters.ContainsKey(pField.Name) Then
                        Return False
                    Else
                        Return True
                    End If
                End If
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Private Function GetSortedTableNames(ByVal tableNames As ICollection, ByVal tableMap As Hashtable) As String()
        'Regular tables
        Dim tables1 As List(Of String) = New List(Of String)
        '2D tables
        Dim tables2 As List(Of String) = New List(Of String)
        For Each tName As String In tableNames
            Dim paramTable As ParameterTable = tableMap(tName)
            If String.IsNullOrEmpty(paramTable.Dimension2) Then
                'Regular table, add to its list
                tables1.Add(tName)
            Else
                tables2.Add(tName)
            End If
        Next
        Dim keysA(tableMap.Count - 1) As String
        tables1.Sort()
        tables2.Sort()
        tables1.CopyTo(keysA, 0)
        tables2.CopyTo(keysA, tables1.Count)
        Return keysA
    End Function

    Public Function BA_FindClosestFeatureOID(ByVal searchGeo As IGeometry, ByVal targetFeatureClassPath As String, ByRef stationInfoParam As AoiParameter) As Integer
        Dim targetFolder As String = "PleaseReturn"
        Dim targetFile As String = BA_GetBareName(targetFeatureClassPath, targetFolder)
        Dim featureClass As IFeatureClass = Nothing
        Dim fCursor As IFeatureCursor = Nothing
        Dim queryFeature As IFeature = Nothing
        Dim queryGeometry As IGeometry = Nothing
        Dim proxOperator As IProximityOperator = Nothing
        Dim closestOID As Integer = -1
        Dim closestDistance As Double = -1
        Try
            If searchGeo Is Nothing Then Return closestOID
            Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(targetFeatureClassPath)
            If wType = WorkspaceType.Geodatabase Then
                featureClass = BA_OpenFeatureClassFromGDB(targetFolder, targetFile)
            Else
                featureClass = BA_OpenFeatureClassFromFile(targetFolder, targetFile)
            End If
            If featureClass IsNot Nothing Then
                fCursor = featureClass.Search(Nothing, False)
                If fCursor IsNot Nothing Then
                    Dim idxZValue As Integer = fCursor.FindField("Z")
                    Dim idxSid As Integer = fCursor.FindField("SID")
                    Dim values As IList(Of String) = New List(Of String) From {Nothing, Nothing}
                    queryFeature = fCursor.NextFeature
                    Do While queryFeature IsNot Nothing
                        queryGeometry = queryFeature.Shape
                        proxOperator = CType(queryGeometry, IProximityOperator)
                        Dim distance As Double = proxOperator.ReturnDistance(searchGeo)
                        If closestDistance < 0 Then
                            closestDistance = distance
                            closestOID = queryFeature.OID
                            values(FrmPEandSRObs.IDX_STATION_ID) = Convert.ToString(queryFeature.Value(idxSid))
                            values(FrmPEandSRObs.IDX_STATION_ELEV) = Convert.ToString(queryFeature.Value(idxZValue))
                        ElseIf distance < closestDistance Then
                            closestDistance = distance
                            closestOID = queryFeature.OID
                            values(FrmPEandSRObs.IDX_STATION_ID) = Convert.ToString(queryFeature.Value(idxSid))
                            values(FrmPEandSRObs.IDX_STATION_ELEV) = Convert.ToString(queryFeature.Value(idxZValue))
                        End If
                        queryFeature = fCursor.NextFeature
                    Loop
                    stationInfoParam.ValuesList = values
                    stationInfoParam.DateUpdated = DateTime.Now
                    Return closestOID
                End If
            End If
            Return closestOID
        Catch ex As Exception
            Debug.Print("BA_FindClosestFeatureOID: " & ex.Message)
            Return -1
        Finally
            featureClass = Nothing
            fCursor = Nothing
            queryFeature = Nothing
            queryGeometry = Nothing
            proxOperator = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Public Function BA_Create_grid_zones_v(ByVal hruGdbPath As String) As BA_ReturnCode
        'Convert raster grid to 'grid_zones_v' shapefile
        Dim retVal As Short = BA_Raster2PolygonShapefileFromPath(hruGdbPath & BA_EnumDescription(PublicPath.HruGrid), hruGdbPath & BA_EnumDescription(PublicPath.HruZonesVector), False)
        Dim success As BA_ReturnCode = BA_ReturnCode.UnknownError
        If retVal = 1 Then
            Dim zonesVectorName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruZonesVector), False)
            Dim tempVectorName As String = "dissolve_v"
            'If successful, add HRU_ID column and copy values from 'grid_code' column
            success = BA_CreateHruIdField(hruGdbPath, zonesVectorName)
            'If this is a non-contiguous HRU, we need to dissolve on HRU ID
            If success = BA_ReturnCode.Success AndAlso BA_IsNonContiguousHru(hruGdbPath, zonesVectorName, BA_GetBareName(BA_EnumDescription(PublicPath.HruGrid))) = True Then
                success = BA_Dissolve(hruGdbPath & "\" & zonesVectorName, BA_FIELD_HRU_ID, hruGdbPath & "\" & tempVectorName)
                If success = BA_ReturnCode.Success Then
                    'After dissolving, remove the original shapefile
                    retVal = BA_Remove_ShapefileFromGDB(hruGdbPath, zonesVectorName)
                    If retVal = 1 Then
                        'And rename the dissolved file to grid_zones_v
                        BA_RenameFeatureClassInGDB(hruGdbPath, tempVectorName, zonesVectorName)
                    End If
                End If
            End If
        End If
    End Function

    Public Function BA_LoadLayerParametersFromXml(ByVal hruPath As String) As IDictionary(Of String, LayerParameter)
        Dim logPath As String = hruPath & BA_EnumDescription(PublicPath.LayerParametersLogXml)
        If BA_File_ExistsWindowsIO(logPath) Then
            Dim obj As Object = SerializableData.Load(logPath, GetType(LayerParametersLog))
            If obj IsNot Nothing Then
                Dim lTable As LayerParametersLog = CType(obj, LayerParametersLog)
                Dim paramDict As IDictionary(Of String, LayerParameter) = New Dictionary(Of String, LayerParameter)
                For Each param As LayerParameter In lTable.LayerParameters
                    paramDict(param.Name) = param
                Next
                Return paramDict
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function

    Public Function BA_ExportPeAndSrFile(ByVal outputFile As String, ByVal aoiName As String, _
                                         ByVal pe_param As AoiParameter, ByVal sr_param As AoiParameter, _
                                         ByVal missingValue As String) As BA_ReturnCode
        Dim sb As StringBuilder = New StringBuilder
        Try
            Using sw = New StreamWriter(outputFile)
                'Write section header
                sb.Append(TABLE_FLAG)
                sb.Append(",")
                sb.Append("obs_monthly")
                sw.WriteLine(sb.ToString)
                sb.Remove(0, sb.Length)
                sb.Append(LEN_KEY)
                sb.Append(",")
                sb.Append(NUM_MONTHS)
                sw.WriteLine(sb.ToString)
                sb.Remove(0, sb.Length)
                sb.Append(CREATED_AT)
                sb.Append(",")
                Dim rightNow As Date = DateTime.Now
                sb.Append(rightNow.ToString("0:MM/dd/yy"))
                sw.WriteLine(sb.ToString)
                sb.Remove(0, sb.Length)
                sb.Append(CREATED_BY)
                sb.Append(",")
                sb.Append(BAGIS_P)
                sw.WriteLine(sb.ToString)
                sb.Remove(0, sb.Length)
                sb.Append(CONVERTED_FROM)
                sb.Append(",")
                sb.Append(BAGIS_P)
                sw.WriteLine(sb.ToString)
                sb.Remove(0, sb.Length)
                sb.Append(DATE_FORMAT)
                sb.Append(",")
                sb.Append("MM")
                sw.WriteLine(sb.ToString)
                sb.Remove(0, sb.Length)
                sb.Append("aoi")
                sb.Append(",")
                sb.Append(aoiName)
                sw.WriteLine(sb.ToString)
                sb.Remove(0, sb.Length)
                sb.Append("unit_for_PE")
                sb.Append(",")
                sb.Append("inch/day")
                sw.WriteLine(sb.ToString)
                sb.Remove(0, sb.Length)
                sb.Append("unit_for_SR")
                sb.Append(",")
                sb.Append("langleys/day or calories/sq cm/day")
                sw.WriteLine(sb.ToString)
                sb.Remove(0, sb.Length)
                sb.Append(HEADER_FLAG)
                sb.Append(",")
                sb.Append("date")
                sb.Append(",")
                sb.Append(BA_Aoi_Parameter_PE_Obs)
                sb.Append(",")
                sb.Append(BA_Aoi_Parameter_SR_Obs)
                sw.WriteLine(sb.ToString)
                sb.Remove(0, sb.Length)
                sb.Append("Type")
                sb.Append(",")
                sb.Append("Date")
                sb.Append(",")
                sb.Append("Real")
                sb.Append(",")
                sb.Append("Real")
                sw.WriteLine(sb.ToString)
                sb.Remove(0, sb.Length)
                'Prepare to write table of values
                Dim peValues As IList(Of String) = New List(Of String)
                Dim srValues As IList(Of String) = New List(Of String)
                If pe_param IsNot Nothing Then
                    peValues = pe_param.ValuesList
                End If
                If sr_param IsNot Nothing Then
                    srValues = sr_param.ValuesList
                End If
                'Write out table
                For i As Short = 1 To NUM_MONTHS
                    sb.Append(",")
                    sb.Append(i)
                    sb.Append(",")
                    Dim nextPe As String = missingValue
                    If peValues.Count >= i - 1 Then
                        If peValues.Item(i - 1) IsNot Nothing Then
                            nextPe = peValues.Item(i - 1)
                        End If
                    End If
                    sb.Append(nextPe)
                    sb.Append(",")
                    Dim nextSr As String = missingValue
                    If srValues.Count >= i - 1 Then
                        If srValues.Item(i - 1) IsNot Nothing Then
                            nextSr = srValues.Item(i - 1)
                        End If
                    End If
                    sb.Append(nextSr)
                    sw.WriteLine(sb.ToString)
                    sb.Remove(0, sb.Length)
                Next
            End Using
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_ExportPeAndSrFile Exception: " & ex.Message)
            Return BA_ReturnCode.WriteError
        End Try
    End Function

End Module
