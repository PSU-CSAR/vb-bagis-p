Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports BAGIS_ClassLibrary
Imports System.IO
Imports System.Windows.Forms
Imports System.Text
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports Microsoft.VisualBasic.FileIO
Imports ESRI.ArcGIS.GeoAnalyst

Public Class FrmExportParametersEwsf

    Dim m_aoi As Aoi
    Dim m_paramsTable As Hashtable
    Dim m_tablesTable As Hashtable
    Dim m_spatialParamsTable As Hashtable
    Dim m_reqSpatialParameters As IList(Of String)
    Dim m_missingSpatialParameters As IList(Of String)
    Dim m_radplSpatialParameters As IList(Of String)
    Dim m_bagisParameterFilePath As String
    Dim m_exportMessage As String
    Dim m_linearUnit As String
    Dim m_hru As Hru
    Dim m_aoiParamTable As IDictionary(Of String, AoiParameter)
    Dim m_hasSrObs As Boolean = False
    Dim m_hasPeObs As Boolean = False
    Private Const METER As String = "Meter"
    Private Const FOOT As String = "Foot"

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ' Add items to CboResampleHru
        CboResampleHru.Items.Add(BA_Resample_Majority)
        CboResampleHru.Items.Add(BA_Resample_Nearest)
        CboResampleHru.SelectedItem = BA_Resample_Majority
        ' Add items to CboResampleDem
        CboResampleDem.Items.Add(BA_Resample_Bilinear)
        CboResampleDem.Items.Add(BA_Resample_Nearest)
        CboResampleDem.Items.Add(BA_Resample_Cubic)
        CboResampleDem.SelectedItem = BA_Resample_Bilinear


        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim aoi As Aoi = bExt.aoi
        If aoi IsNot Nothing Then
            Try
                m_aoi = aoi
                TxtAoiPath.Text = m_aoi.FilePath
                Me.Text = "Export Parameters (AOI: " & m_aoi.Name & m_aoi.ApplicationVersion & " )"
                TimerStatus.Interval = 1000
                'Load layer lists
                'Create a DirectoryInfo of the HRU directory.
                Dim zonesDirectory As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, Nothing)
                Dim dirZones As New DirectoryInfo(zonesDirectory)
                Dim dirZonesArr As DirectoryInfo() = Nothing
                If dirZones.Exists Then
                    dirZonesArr = dirZones.GetDirectories
                    LoadHruLayers(dirZonesArr)
                End If

                SetDemResolution()
                ManageAoiParameterFields()

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
                Me.Text = "Export Parameters (AOI: " & aoiName & m_aoi.ApplicationVersion & " )"
                bagisPExt.aoi = m_aoi

                'Load layer lists
                ' Create a DirectoryInfo of the HRU directory.
                Dim zonesDirectory As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, Nothing)
                Dim dirZones As New DirectoryInfo(zonesDirectory)
                Dim dirZonesArr As DirectoryInfo() = Nothing
                If dirZones.Exists Then
                    dirZonesArr = dirZones.GetDirectories
                    LoadHruLayers(dirZonesArr)
                End If

                SetDemResolution()
                ManageAoiParameterFields()
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

    Private Sub LoadProfileList(ByVal hruName As String)
        LstProfiles.Items.Clear()
        Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, hruName)
        Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb)
        If BA_Folder_ExistsWindowsIO(hruParamPath) Then
            Dim tNames As IList(Of String) = BA_ListTablesInGDB(hruParamPath)
            If tNames IsNot Nothing Then
                For Each pName As String In tNames
                    LstProfiles.Items.Add(pName)
                Next
            End If
        End If
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub LstHruLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstHruLayers.SelectedIndexChanged
        If LstHruLayers.SelectedIndex > -1 Then
            'Derive the file path for the HRU vector to be displayed
            Dim selItem As LayerListItem = TryCast(LstHruLayers.SelectedItem, LayerListItem)
            Dim hruGdbName As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
            Dim vName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruZonesVector), False)
            Dim hruCount As Integer = BA_CountPolygons(hruGdbName, vName)
            If hruCount > 0 Then
                TxtNHru.Text = CStr(hruCount)
            Else
                TxtNHru.Text = "0"
            End If
            'Load selected hru from xml
            Dim hruInputPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
            Dim aoi As Aoi = BA_LoadHRUFromXml(hruInputPath)
            For Each anHru In aoi.HruList
                ' We found the hru the user selected
                m_hru = anHru
            Next
            LoadProfileList(selItem.Name)
            SetMinPolySize(hruGdbName)
            SetHruResolution()
        End If
    End Sub

    Private Sub BtnSetTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSetTemplate.Click
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            SetTemplate(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub BtnDefaultTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDefaultTemplate.Click
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim settingsPath As String = bExt.SettingsPath
        Dim profilesFolder As String = BA_GetPublicProfilesPath(settingsPath)
        Dim templatepath As String = profilesFolder & BA_EnumDescription(PublicPath.DefaultParameterTemplate)
        If Not BA_File_ExistsWindowsIO(templatepath) Then
            Dim errMsg As String = "The default parameter template could not be located. "
            errMsg += "BAGIS-P is looking for the template at " & templatepath & "."
            MessageBox.Show(errMsg, "Default template not found", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            SetTemplate(templatepath)
        End If
    End Sub

    Private Function SetTemplate(ByVal pathToFile As String) As BA_ReturnCode
        Dim validParamFile As Boolean = False
        Try
            Using sr As StreamReader = File.OpenText(pathToFile)
                If sr.Peek <> 0 Then
                    Dim firstLine As String = sr.ReadLine
                    If Not String.IsNullOrEmpty(firstLine) Then
                        Dim firstChars As String = firstLine.Substring(0, 2)
                        If firstChars = SECTION_FLAG Then
                            validParamFile = True
                        End If
                    End If
                End If
            End Using
        Catch ex As Exception
            Debug.Print("SetTemplate Exception: " & ex.Message)
        End Try
        If validParamFile = True Then
            TxtParameterTemplate.Text = pathToFile
            'TxtDescription.Text = "Descr: East Fork Carson River, CA" & vbCrLf & "Modified: Wed Aug 29 17:41:47 MDT 2012 " & vbCrLf _
            '                    & "Version: 1.7 " & vbCrLf & "Created: Tue Aug 21 16:02:12 MDT 2012"
            GetTemplateDetails()
            Return BA_ReturnCode.Success
        Else
            MessageBox.Show("The file you selected is not a valid template.", "Invalid template", MessageBoxButtons.OK, MessageBoxIcon.Information)
            TxtParameterTemplate.Text = Nothing
        End If
        Return BA_ReturnCode.UnknownError
    End Function

    Private Sub BtnSetOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSetOutput.Click
        Try
            If SaveFileDialog1.ShowDialog = DialogResult.OK Then
                Dim fName As String = SaveFileDialog1.FileName
                'Append .csv extension if not there already
                If Microsoft.VisualBasic.Strings.Right(fName, 4).ToLower = ".csv" Then
                    fName = fName & ".csv"
                End If
                TxtOutputFolder.Text = SaveFileDialog1.FileName
            Else
                TxtOutputFolder.Text = Nothing
            End If
        Catch ex As Exception
            Debug.Print("BtnSetOutput_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub LstProfiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstProfiles.SelectedIndexChanged
        TxtNumParameters.Text = Nothing
        Dim paramTable As ITable = Nothing
        Dim pFields As IFields = Nothing
        Dim selItem As LayerListItem = TryCast(LstHruLayers.SelectedItem, LayerListItem)
        Try
            If LstProfiles.SelectedIndex > -1 Then
                Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
                Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb)
                If BA_Folder_ExistsWindowsIO(hruParamPath) Then
                    Dim selProfile As String = TryCast(LstProfiles.SelectedItem, String)
                    If selProfile IsNot Nothing Then
                        Dim tableName As String = selProfile & BA_PARAM_TABLE_SUFFIX
                        paramTable = BA_OpenTableFromGDB(hruParamPath, tableName)
                        If paramTable IsNot Nothing Then
                            pFields = paramTable.Fields
                            If pFields IsNot Nothing Then
                                'Subtract the id, the hru_id, and the oms_id columns
                                TxtNumParameters.Text = pFields.FieldCount - 3
                            End If
                        End If
                    End If
                End If
            End If
            ManageEditParametersButton()
            ManageExportButton()
        Catch ex As Exception
            Debug.Print("LstProfiles_SelectedIndexChanged Exception: " & ex.Message)
        Finally
            pFields = Nothing
            paramTable = Nothing
        End Try
    End Sub

    Private Sub BtnEditParameters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEditParameters.Click
        If TxtParameterTemplate.Text.Length > 1 Then
            Try
                Dim frmEditParameters As FrmEditParameters = New FrmEditParameters(Me, m_paramsTable, m_tablesTable, TxtParameterTemplate.Text)
                frmEditParameters.ShowDialog()
            Catch ex As Exception
                Debug.Print("FrmEditParameters initialization Exception: " & ex.Message)
                MessageBox.Show("The selected parameter template could not be read by BAGIS-P. Please select another template.", "Invalid Template", _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("Please select a template to edit.", "No template selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub GetTemplateDetails()
        Dim parser As TextFieldParser = Nothing

        Try
            parser = New TextFieldParser(TxtParameterTemplate.Text)
            parser.SetDelimiters({","})
            parser.HasFieldsEnclosedInQuotes = True
            Dim fields As String() = parser.ReadFields
            If fields(0) = SECTION_FLAG Then
                ' Get the modified date
                If Not parser.EndOfData Then
                    fields = parser.ReadFields
                    If fields(0) = "Descr" Then
                        TxtDescription.Text = fields(1)
                    End If
                End If
                Do Until parser.EndOfData
                    fields = parser.ReadFields
                    If fields(0) = PARAM_FLAG Then
                        Exit Do
                    ElseIf fields(0) = DESCR Then
                        'Description
                        TxtDescription.Text = fields(1)
                    ElseIf fields(0) = MODIFIED_AT Then
                        'Modified date
                        TxtModified.Text = fields(1)
                        'Date format: Wed Aug 29 17:41:47 MDT 
                    ElseIf fields(0) = VERSION Then
                        'Version
                        TxtVersion.Text = fields(1)
                    ElseIf fields(0) = "CreatedAt" Then
                        'Version
                        TxtCreated.Text = fields(1)
                    End If
                Loop
            End If
        Catch ex As Exception
            Debug.Print("GetTemplateDetails Exception: " & ex.Message)
        Finally
            parser.Close()
        End Try
    End Sub

    Private Sub ManageExportButton()
        If String.IsNullOrEmpty(TxtParameterTemplate.Text) Or String.IsNullOrEmpty(TxtOutputFolder.Text) Or _
            LstProfiles.SelectedIndex < 0 Then
            BtnExport.Enabled = False
        Else
            BtnExport.Enabled = True
        End If
    End Sub

    Private Sub ManageEditParametersButton()
        If String.IsNullOrEmpty(TxtParameterTemplate.Text) Or String.IsNullOrEmpty(TxtNHru.Text) Then
            BtnEditParameters.Enabled = False
            BtnEditHruParameters.Enabled = False
        Else
            BtnEditParameters.Enabled = True
            If LstProfiles.SelectedIndex > -1 Then
                BtnEditHruParameters.Enabled = True
            Else
                BtnEditHruParameters.Enabled = False
            End If
        End If
    End Sub

    Private Sub EnableButtons(ByVal enabled As Boolean)
        BtnSelectAoi.Enabled = enabled
        BtnSetTemplate.Enabled = enabled
        BtnDefaultTemplate.Enabled = enabled
        BtnEditParameters.Enabled = enabled
        BtnEditHruParameters.Enabled = enabled
        BtnSetOutput.Enabled = enabled
        BtnExport.Enabled = enabled
    End Sub

    Private Sub TxtParameterTemplate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtParameterTemplate.TextChanged
        ManageExportButton()
        ManageEditParametersButton()
    End Sub

    Private Sub TxtOutputFolder_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtOutputFolder.TextChanged
        ManageExportButton()
    End Sub

    Private Sub TxtNHru_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtNHru.TextChanged
        ManageEditParametersButton()
    End Sub

    Private Sub BtnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnExport.Click
        'Check to make sure resample resolutions are the same
        If Not TxtDemResample.Text.Equals(TxtHruResample.Text) Then
            MessageBox.Show("The HRU and DEM resample resolutions must be the same in eWSF", "Resample resolution mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            TxtHruResample.Focus()
            TxtHruResample.SelectAll()
            Exit Sub
        End If
        EnableButtons(False)
        TimerStatus.Enabled = True

        'Check to see if jh_coeff has been previously calculated
        Dim jhcoeffValue As Double = BA_9999
        If m_aoiParamTable IsNot Nothing AndAlso m_aoiParamTable.ContainsKey(BA_Aoi_Parameter_jh_coef) Then
            Dim jhcoeffParam As AoiParameter = m_aoiParamTable(BA_Aoi_Parameter_jh_coef)
            jhcoeffValue = CDbl(jhcoeffParam.Value)
            Dim sb As StringBuilder = New StringBuilder
            sb.Append("The mean Jenson-Haise PET coefficient was last calculated for this AOI on ")
            sb.Append(jhcoeffParam.DateUpdated.ToString("MM/dd/yy"))
            sb.Append("." & vbCrLf & "The value on that date was ")
            sb.Append(jhcoeffValue.ToString("F5") & "." & vbCrLf)
            sb.Append("Click 'Yes' to use this value or 'No' to recalculate. " & vbCrLf)
            sb.Append("Note that recalculating the value may take several minutes.")
            Dim res As DialogResult = MessageBox.Show(sb.ToString, "Jenson-Haise PET coefficient", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If res <> DialogResult.Yes Then
                jhcoeffValue = BA_9999
            End If
        End If

        m_exportMessage = "Reading parameters from template .........."
        LblStatus.Text = m_exportMessage
        'If we did not edit the paramsTable, it will be nothing and we need to initialize it from the template
        If m_paramsTable Is Nothing Then
            m_paramsTable = BA_GetParameterMap(TxtParameterTemplate.Text, ",", CInt(TxtNHru.Text), TxtAoiPath.Text)
        End If
        'Replace nhru parameter with correct value
        Dim newValue() As String = {TxtNHru.Text}
        Dim nHruParam As Parameter = m_paramsTable(NHRU)
        If nHruParam Is Nothing Then
            nHruParam = New Parameter(NHRU, newValue, True)
        Else
            nHruParam.value = newValue
        End If
        m_paramsTable(NHRU) = nHruParam
        'Replace nradpl parameter with correct value
        Dim nRadplParam As Parameter = m_paramsTable(NRADPL)
        If nRadplParam Is Nothing Then
            nRadplParam = New Parameter(NRADPL, newValue, True)
        Else
            nRadplParam.value = newValue
        End If
        m_paramsTable(NRADPL) = nRadplParam

        'Read log to extract missing data value
        Dim selItem As LayerListItem = CType(LstHruLayers.SelectedItem, LayerListItem)
        Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
        Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb)
        Dim missingData As String = Nothing
        Dim logProfile As LogProfile = BA_LoadLogProfileFromXml(hruPath & "\" & TryCast(LstProfiles.SelectedItem, String) & "_params_log.xml")
        If logProfile IsNot Nothing Then
            missingData = logProfile.NoDataValue
        End If

        If m_tablesTable Is Nothing Then
            'If we did not edit the paramsTable, it will be nothing and we need to initialize it from the template
            m_tablesTable = BA_GetTableMap(TxtParameterTemplate.Text, ",", m_paramsTable)
        End If

        If m_reqSpatialParameters Is Nothing Then
            'Read the list of BAGIS-P parameter names to exclude
            ReadBagisParameterNames()
        End If

        If jhcoeffValue = BA_9999 Then
            m_exportMessage = "Calculating JH Coefficient for AOI .........."
            LblStatus.Text = m_exportMessage

            'Calculating JH_Coeff for AOI
            ' 1. Open public settings file to get file name for each jh layer
            ' 2. Check to see if the layer exists in aoi param.gdb
            Dim unitsDataSource As DataSource = Nothing 'A data source populated during next function call to be queried when determining units
            Dim jhLayerPaths As IDictionary(Of String, String) = BA_GetJHLayerPaths(m_aoi.FilePath, unitsDataSource)
            Dim warning As String = ""
            If jhLayerPaths.Count < 4 Then
                warning += "One or more layers required to calculate the JH_Coeff are "
                warning += "undefined or missing from the current AOI. The jh_coeff column in the "
                warning += NMONTHS & " table should be edited manually." & vbCrLf & vbCrLf
            Else
                ' 3. Check to see if model exists in local methods folder
                ' 4. Populate model parameters and run model
                Dim bExt As BagisPExtension = BagisPExtension.GetExtension
                Dim settingsPath As String = bExt.SettingsPath
                Dim toolBoxPrefix As String = BA_GetPublicMethodsPath(settingsPath)
                Dim jh_table As String = BA_GetBareName(BA_EnumDescription(PublicPath.JhCoefAoiTable))
                Dim jh_success As BA_ReturnCode = BA_ExecuteJHModel(m_aoi.FilePath, hruPath, TryCast(LstProfiles.SelectedItem, String), _
                                                                toolBoxPrefix, jhLayerPaths, unitsDataSource, jh_table)
                If jh_success <> BA_ReturnCode.Success Then
                    warning += "An error occurred while calculating the JH_Coeff. The jh_coeff "
                    warning += " column in the " & NMONTHS & " table should be edited manually." & vbCrLf & vbCrLf
                Else
                    ' 5. Read jh_coeff value from output table
                    jhcoeffValue = BA_ReadJHCoeffResults(hruParamPath, jh_table, jh_table)

                    If jhcoeffValue <> BA_9999 Then
                        ' 6. Save the value to the aoiParameters in the settings.xml
                        jh_success = SaveJhCoef(m_aoiParamTable, jhcoeffValue, BA_GetLocalSettingsPath(m_aoi.FilePath))
                        If jh_success <> BA_ReturnCode.Success Then
                            warning += "The results of the AOI JH_Coeff calculation could not "
                            warning += "be saved to the AOI settings.xml file. Please contact a "
                            warning += "system administrator."
                            warning += vbCrLf & vbCrLf
                        End If
                        ' 7. Delete output table
                        jh_success = BA_Remove_TableFromGDB(hruParamPath, jh_table)
                        If jh_success <> BA_ReturnCode.Success Then
                            warning += "The table created for the AOI JH_Coeff calculation could "
                            warning += "not be deleted. You should manually delete this table "
                            warning += "the path is: " & vbCrLf & vbCrLf
                            warning += hruParamPath & "\" & jh_table
                        End If
                    End If
                End If

                If Not String.IsNullOrEmpty(warning) Then
                    MessageBox.Show(warning, "Invalid jh_coeff calculation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            End If
        End If

        ' 7. find AOI-level in nmonths table and overwrite them with calculated values
        Dim nmonthsTable As ParameterTable = m_tablesTable(NMONTHS)
        If nmonthsTable IsNot Nothing Then
            nmonthsTable = BA_UpdateParametersInNmonthsTable(nmonthsTable, m_aoiParamTable)
            m_tablesTable(NMONTHS) = nmonthsTable
        End If

        m_exportMessage = "Reading spatial parameters calculated by BAGIS-P .........."
        LblStatus.Text = m_exportMessage
        Dim tableName As String = CStr(LstProfiles.SelectedItem) & BA_PARAM_TABLE_SUFFIX
        Dim success As Boolean = VerifyParameterValuesInTable(hruParamPath, tableName, True)
        Dim retVal As BA_ReturnCode = BA_ReturnCode.Success

        If success = True Then
            If m_spatialParamsTable Is Nothing Then
                'If we did not edit the spatial params table, it will be nothing and we need to initialize it from the template
                m_spatialParamsTable = BA_ReadNhruParams(TxtParameterTemplate.Text, ",", TxtNHru.Text, m_reqSpatialParameters, _
                                                         m_missingSpatialParameters, missingData)
            End If
            m_exportMessage = "Generating parameter export file  .........."
            LblStatus.Text = m_exportMessage
            BA_ExportParameterFile(TxtOutputFolder.Text, TxtDescription.Text, TxtVersion.Text, m_paramsTable, m_tablesTable, hruParamPath, _
                                   tableName, CInt(TxtNHru.Text), m_spatialParamsTable, missingData, m_radplSpatialParameters)
            Dim zipFolder As String = Nothing
            If CkParametersOnly.Checked = False Then
                'Export Geodatabase file to shapefile
                Dim targetFolder As String = "Please Return"
                Dim targetFile As String = BA_GetBareName(TxtOutputFolder.Text, targetFolder)
                Dim tempBagisFolder As String = "BagisZip"
                'Delete tempBagisFolder if it exists to make sure we don't have old data
                If BA_Folder_ExistsWindowsIO(targetFolder & tempBagisFolder) Then
                    BA_Remove_Folder(targetFolder & tempBagisFolder)
                End If
                zipFolder = BA_CreateFolder(targetFolder, tempBagisFolder)
                'Strip extension from parameter file name so we can add the .shp
                If Not String.IsNullOrEmpty(targetFile) Then
                    Dim idxExtension As Integer = targetFile.IndexOf(".")
                    If idxExtension > 0 Then
                        targetFile = targetFile.Substring(0, idxExtension)
                    End If
                End If
                Dim hruGdbName As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
                Dim vName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruZonesVector), False)
                retVal = BA_ConvertGDBToShapefile(hruGdbName, vName, zipFolder, targetFile)
                'Copy the parameter file into the tempBagisFolder
                File.Copy(TxtOutputFolder.Text, zipFolder & "\" & BA_GetBareName(TxtOutputFolder.Text), True)
                m_exportMessage = "Converting HRU zones to ASCII .........."
                LblStatus.Text = m_exportMessage
                Dim hruClipPath As String = AddZonesToZipFolder(zipFolder, hruGdbName, targetFile)
                If String.IsNullOrEmpty(hruClipPath) Then
                    MessageBox.Show("An error occurred while packaging the HRU zones and DEM for eWsf. They are not included in the zip file.", "HRU zones error", MessageBoxButtons.OK)
                Else
                    'Check to be sure the # of zones matches nhru
                    Dim hruRows As Integer = BA_CountRowsInRaster(hruClipPath)
                    If hruRows <> CInt(TxtNHru.Text) Then
                        BA_Remove_Folder(targetFolder & tempBagisFolder)
                        MessageBox.Show("The number of HRU's in the resampled HRU layer (" & hruRows & ") is different from nhru. The export process has been aborted.", _
                                        "Invalid number of HRU zones", MessageBoxButtons.OK)
                        Exit Sub
                    End If

                    'Need to use the hru raster instead of vector to get exactly the right number of columns and rows
                    m_exportMessage = "Converting DEM layer to ASCII .........."
                    LblStatus.Text = m_exportMessage
                    retVal = AddDemToZipFolder(zipFolder, hruClipPath, targetFile)
                    If retVal <> BA_ReturnCode.Success Then
                        MessageBox.Show("An error occurred while packaging the DEM for eWsf. It is not included in the zip file.", "DEM error", MessageBoxButtons.OK)
                    End If
                    Dim hruClipName As String = BA_GetBareName(hruClipPath)
                    If Not hruClipName.Equals(GRID) Then
                        BA_RemoveRasterFromGDB(hruGdbName, hruClipPath)
                    End If
                End If
                'Zip up the folder
                m_exportMessage = "Generating zip file  .........."
                LblStatus.Text = m_exportMessage
                Dim zipFileName As String = BA_StandardizeShapefileName(targetFile, False) & ".zip"
                retVal = BA_ZipFolder(zipFolder, zipFileName)
            End If

            If success = True And retVal = BA_ReturnCode.Success Then
                BA_Remove_Folder(zipFolder)
                TimerStatus.Enabled = False
                m_exportMessage = ""
                LblStatus.Text = m_exportMessage
                MessageBox.Show("Parameter file export complete !", _
                                "File export", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            TimerStatus.Enabled = False
            m_exportMessage = ""
            LblStatus.Text = m_exportMessage
        End If
        EnableButtons(True)
    End Sub

    Public WriteOnly Property ParamsTable As Hashtable
        Set(ByVal value As Hashtable)
            m_paramsTable = value
        End Set
    End Property

    Public WriteOnly Property TablesTable As Hashtable
        Set(ByVal value As Hashtable)
            m_tablesTable = value
        End Set
    End Property

    Public WriteOnly Property SpatialParamsTable As Hashtable
        Set(ByVal value As Hashtable)
            m_spatialParamsTable = value
        End Set
    End Property

    Private Sub BtnEditHruParameters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEditHruParameters.Click
        Dim nhru As Integer = CInt(TxtNHru.Text)
        If m_reqSpatialParameters Is Nothing Then
            ReadBagisParameterNames()
        End If
        Dim selItem As LayerListItem = CType(LstHruLayers.SelectedItem, LayerListItem)
        Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
        Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb)
        Dim tableName As String = CStr(LstProfiles.SelectedItem) & BA_PARAM_TABLE_SUFFIX
        VerifyParameterValuesInTable(hruParamPath, tableName, False)
        'Read log to extract missing data value
        Dim missingData As String = Nothing
        Dim logProfile As LogProfile = BA_LoadLogProfileFromXml(hruPath & "\" & TryCast(LstProfiles.SelectedItem, String) & "_params_log.xml")
        If logProfile IsNot Nothing Then
            missingData = logProfile.NoDataValue
        End If
        Dim frmEditHruParameters As FrmEditHruParameters = New FrmEditHruParameters(Me, TxtParameterTemplate.Text, ",", _
                                                                                    m_reqSpatialParameters, m_missingSpatialParameters, _
                                                                                    m_spatialParamsTable, missingData)
        frmEditHruParameters.ShowDialog()
    End Sub

    Private Sub ReadBagisParameterNames()
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim settingsPath As String = bExt.SettingsPath
        Dim methodsPath As String = BA_GetPublicMethodsPath(settingsPath)
        Dim sr As StreamReader = Nothing
        m_reqSpatialParameters = New List(Of String)
        m_radplSpatialParameters = New List(Of String)
        Try
            'open file for input
            If BA_File_ExistsWindowsIO(methodsPath & BA_EnumDescription(PublicPath.BagisParameters)) Then
                m_bagisParameterFilePath = methodsPath & BA_EnumDescription(PublicPath.BagisParameters)
                sr = File.OpenText(methodsPath & BA_EnumDescription(PublicPath.BagisParameters))
                Dim line As String
                ' Read and display the lines from the file until the end 
                ' of the file is reached.
                Do
                    line = sr.ReadLine()
                    If Not String.IsNullOrEmpty(line) Then
                        Dim chrComment As Char = "#"
                        line = Trim(line)
                        If line(0) <> chrComment Then
                            Dim lineArr As String() = line.Split(",")
                            Dim pName As String = lineArr(0)
                            Dim pDimension As String = lineArr(1)
                            If pDimension = NRADPL Then
                                m_radplSpatialParameters.Add(pName)
                            End If
                            m_reqSpatialParameters.Add(pName)
                        End If
                    End If
                Loop Until line Is Nothing
            End If
        Catch ex As Exception
            Debug.Print("ReadBagisParameterNames Exception: " & ex.Message)
        Finally
            'Don't forget to close the file handle
            If sr IsNot Nothing Then sr.Close()
        End Try
    End Sub

    Private Function VerifyParameterValuesInTable(ByVal hruParamFolder As String, ByVal hruParamFile As String, _
                                                  ByVal showWarningMessage As Boolean) As Boolean
        'm_bagisParameterNames
        Dim pTable As ITable = Nothing
        Dim pFields As IFields = Nothing
        m_missingSpatialParameters = New List(Of String)
        Try
            pTable = BA_OpenTableFromGDB(hruParamFolder, hruParamFile)
            If pTable IsNot Nothing Then
                pFields = pTable.Fields
                For Each reqParam As String In m_reqSpatialParameters
                    Dim foundIt As Boolean = False
                    For i As Integer = 0 To pFields.FieldCount - 1
                        Dim pField As Field = pFields.Field(i)
                        Dim fieldName As String = pField.Name
                        If fieldName = reqParam Then
                            foundIt = True
                            Exit For
                        End If
                    Next
                    If foundIt = False Then
                        m_missingSpatialParameters.Add(reqParam)
                    End If
                Next

                If m_missingSpatialParameters.Count > 0 And showWarningMessage = True Then
                    Dim sb As StringBuilder = New StringBuilder
                    sb.Append("The following parameters were missing from the profile you selected: " & vbCrLf)
                    For Each missing As String In m_missingSpatialParameters
                        sb.Append(missing & vbCrLf)
                    Next
                    sb.Append(vbCrLf)
                    sb.Append("You can manually set the missing parameters using the " & vbCrLf)
                    sb.Append("'Edit Hru Parameters' screen." & vbCrLf & vbCrLf)
                    sb.Append("If these parameters are not required, please update the BAGIS-P" & vbCrLf)
                    sb.Append("configuration file located at: " & vbCrLf)
                    sb.Append(m_bagisParameterFilePath & vbCrLf & vbCrLf)
                    sb.Append("Do you still wish to export the parameters ?")
                    Dim res As DialogResult = MessageBox.Show(sb.ToString, "Missing parameters", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If res = Windows.Forms.DialogResult.Yes Then
                        Return True
                    Else
                        Return False
                    End If
                End If
                Return True
            Else
                MessageBox.Show("The parameters could not be located for the profile you selected", "Missing parameters", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If
        Catch ex As Exception
            Debug.Print("VerifyParameterValuesInTable Exception: " & ex.Message)
            Return False
        End Try
    End Function

    Private Sub LblParameterTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LblParameterTemplate.Click
        Dim mText = "BAGIS-P's export function uses an input parameter file template to"
        mText = mText & " produce an output parameter file. It stores non-spatial parameters,"
        mText = mText & " spatial parameters that have a dimension of ""nhru"" (i.e., the number"
        mText = mText & " of HRUs in an AOI) that were calculated by BAGIS-P, and spatial"
        mText = mText & " parameters that have a dimension of nhru but were not calculated"
        mText = mText & " by BAGIS-P." & vbCrLf & vbCrLf
        mText = mText & "During the export process, BAGIS-P retrieves parameter values from"
        mText = mText & " the input template file and copies them to the export file except"
        mText = mText & " for spatial parameters calculated by BAGIS-P. Parameters not "
        mText = mText & " calculated by BAGIS-P can be edited using buttons on the Export Parameters form."
        MessageBox.Show(mText, "Parameter Template", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub SetHruResolution()
        If LstHruLayers.SelectedIndex > -1 Then
            'Derive the file path for the HRU vector to be displayed
            Dim selItem As LayerListItem = TryCast(LstHruLayers.SelectedItem, LayerListItem)
            Dim hruGdbPath As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
            Dim cellSize As Double = BA_CellSize(hruGdbPath, GRID)
            Dim linearUnit As ESRI.ArcGIS.Geometry.ILinearUnit = BA_GetLinearUnitOfProjectedRaster(hruGdbPath, GRID)
            Dim unitLabel As String = "Unknown"
            If linearUnit.Name = METER Then
                unitLabel = BA_EnumDescription(MeasurementUnit.Meters)
            ElseIf linearUnit.Name = FOOT Then
                unitLabel = BA_EnumDescription(MeasurementUnit.Feet)
            End If
            If linearUnit IsNot Nothing Then
                TxtHruResolution.Text = Math.Round(cellSize, 2) & " " & unitLabel
                'TxtHruUnits.Text = unitLabel
            Else
                TxtHruResolution.Text = Math.Round(cellSize, 2)
            End If
        End If
    End Sub

    Private Sub SetDemResolution()
        Dim surfacesFolder As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Surfaces)
        Dim cellSize As Double = BA_CellSize(surfacesFolder, BA_EnumDescription(MapsFileName.filled_dem_gdb))
        Dim linearUnit As ESRI.ArcGIS.Geometry.ILinearUnit = BA_GetLinearUnitOfProjectedRaster(surfacesFolder, BA_EnumDescription(MapsFileName.filled_dem_gdb))
        Dim unitLabel As String = "Unknown"
        If linearUnit.Name = METER Then
            unitLabel = BA_EnumDescription(MeasurementUnit.Meters)
        ElseIf linearUnit.Name = FOOT Then
            unitLabel = BA_EnumDescription(MeasurementUnit.Feet)
        End If
        If linearUnit IsNot Nothing Then
            TxtDemResolution.Text = Math.Round(cellSize, 2) & " " & unitLabel
            m_linearUnit = linearUnit.Name
        Else
            TxtDemResolution.Text = Math.Round(cellSize, 2)
        End If
    End Sub

    Private Function AddDemToZipFolder(ByVal zipFolder As String, ByVal hruRasterPath As String, ByVal outputFileBase As String) As BA_ReturnCode
        Dim demCellSize As Double = 0
        Dim inputDataSet As IGeoDataset = Nothing
        Dim demDataSet As IGeoDataset = Nothing
        Dim transformOp As ITransformationOp = New RasterTransformationOp
        Dim exportOp As IRasterExportOp = New RasterConversionOp
        Try
            Dim surfacesFolder As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Surfaces, True)
            Dim inputRasterPath As String = surfacesFolder & BA_EnumDescription(MapsFileName.filled_dem_gdb)
            Dim clippedDem As String = "clipDem"
            Dim resampleDem As String = "reDem"
            Dim outputRasterPath As String = zipFolder & "\" & clippedDem
            Dim readyToResample As Boolean = True
            If Not String.IsNullOrEmpty(TxtDemResample.Text) Then
                demCellSize = CDbl(TxtDemResample.Text)
            End If
            If demCellSize > 0 Then
                'Need to resample
                inputDataSet = BA_OpenRasterFromGDB(surfacesFolder, BA_EnumDescription(MapsFileName.filled_dem_gdb))
                'Get resample method from form
                Dim resampleEnum As esriGeoAnalysisResampleEnum = GetResampleEnum(CboResampleDem.SelectedItem.ToString)
                demDataSet = transformOp.Resample(inputDataSet, demCellSize, resampleEnum)
                Dim retVal As Short = BA_SaveRasterDataset(demDataSet, zipFolder, resampleDem)
                If retVal = 1 Then
                    inputRasterPath = zipFolder & "\" & resampleDem
                Else
                    readyToResample = False
                End If
                demDataSet = Nothing
            End If
            If readyToResample Then
                'Clip DEM to hru raster layer
                Dim snapRasterPath As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Aoi, True) & BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
                Dim success As BA_ReturnCode = BA_ExtractByMask(hruRasterPath, inputRasterPath, snapRasterPath, outputRasterPath)
                If success = BA_ReturnCode.Success Then
                    demDataSet = BA_OpenRasterFromFile(zipFolder, clippedDem)
                    exportOp.ExportToASCII(demDataSet, zipFolder & "\" & outputFileBase & "_dem.asc")
                    'Remove source files after ASCII dataset is created
                    If BA_File_ExistsRaster(zipFolder, resampleDem) Then
                        BA_Remove_Raster(zipFolder, resampleDem)
                    End If
                    Dim retVal As Integer = BA_Remove_Raster(zipFolder, clippedDem)
                    Return BA_ReturnCode.Success
                End If
            End If
            'End If
            Return BA_ReturnCode.UnknownError
        Catch ex As Exception
            Debug.Print("AddDemToZipFolder" & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            inputDataSet = Nothing
            demDataSet = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Private Function AddZonesToZipFolder(ByVal zipFolder As String, ByVal hruGdbName As String, ByVal outputFileBase As String) As String
        Dim hruCellSize As Double = 0
        Dim inputDataSet As IGeoDataset = Nothing
        Dim hruDataSet As IGeoDataset = Nothing
        Dim exportOp As IRasterExportOp = New RasterConversionOp
        Try
            If Not String.IsNullOrEmpty(TxtHruResample.Text) Then
                hruCellSize = CDbl(TxtHruResample.Text)
            End If
            'This is the path that will be used to clip the dem so the extents are EXACTLY the same
            Dim hruClipPath As String = hruGdbName & "\" & GRID
            Dim tempFileName As String = "tmpResample"
            If hruCellSize > 0 Then
                'Need to resample
                'inputDataSet = BA_OpenRasterFromGDB(hruGdbName, GRID)
                Dim outputRaster As String = hruGdbName & "\" & tempFileName
                Dim snapRasterPath As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Aoi, True) & BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
                Dim success As BA_ReturnCode = BA_Resample_Raster(hruGdbName & "\" & GRID, outputRaster, hruCellSize, snapRasterPath, CboResampleHru.SelectedItem.ToString)
                If success = BA_ReturnCode.Success Then
                    hruDataSet = BA_OpenRasterFromGDB(hruGdbName, tempFileName)
                    hruClipPath = outputRaster
                Else
                    Throw New System.Exception("Unable to resample hru raster dataset.")
                End If
            Else
                'Open the dataset directly
                hruDataSet = BA_OpenRasterFromGDB(hruGdbName, GRID)
            End If
            If hruDataSet IsNot Nothing Then
                exportOp.ExportToASCII(hruDataSet, zipFolder & "\" & outputFileBase & "_hru.asc")
                Return hruClipPath
            End If
            Return Nothing
        Catch ex As Exception
            Debug.Print("AddZonesToZipFolder" & ex.Message)
            Return Nothing
        Finally
            inputDataSet = Nothing
            hruDataSet = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Private Function GetResampleEnum(ByVal txtResample As String) As esriGeoAnalysisResampleEnum
        Select Case txtResample
            Case BA_Resample_Nearest
                Return esriGeoAnalysisResampleEnum.esriGeoAnalysisResampleNearest
            Case BA_Resample_Cubic
                Return esriGeoAnalysisResampleEnum.esriGeoAnalysisResampleCubic
            Case Else
                'Per design, bilinear is the default for DEM
                Return esriGeoAnalysisResampleEnum.esriGeoAnalysisResampleBilinear
        End Select
    End Function

    Private Sub TimerStatus_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerStatus.Tick
        If LblStatus.Text.Length > 5 Then
            Dim newMsg As String = LblStatus.Text.Substring(1)
            LblStatus.Text = newMsg
        Else
            LblStatus.Text = m_exportMessage
        End If
        Application.DoEvents()
    End Sub

    Private Sub BtnViewBagisParams_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnViewBagisParams.Click
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim settingsPath As String = bExt.SettingsPath
        Dim parameterFilePath As String = BA_GetPublicMethodsPath(settingsPath) & BA_EnumDescription(PublicPath.BagisParameters)
        If BA_File_ExistsWindowsIO(parameterFilePath) Then
            Process.Start("iexplore.exe", parameterFilePath)
        Else
            MessageBox.Show("Missing BAGIS configuration file at: " & parameterFilePath, "Missing file", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Function SaveJhCoef(ByVal aoiParamTable As IDictionary(Of String, AoiParameter), ByVal jhCoef As Double, _
                                ByVal settingsPath As String) As BA_ReturnCode
        Dim jhCoefParam As AoiParameter = New AoiParameter(BA_Aoi_Parameter_JH_Coef, Convert.ToString(jhCoef))
        If aoiParamTable.ContainsKey(BA_Aoi_Parameter_JH_Coef) Then
            aoiParamTable(BA_Aoi_Parameter_JH_Coef) = jhCoefParam
        Else
            aoiParamTable.Add(BA_Aoi_Parameter_JH_Coef, jhCoefParam)
        End If
        Dim srcList As IList(Of AoiParameter) = New List(Of AoiParameter)
        For Each key As String In aoiParamTable.Keys
            Dim param As AoiParameter = aoiParamTable(key)
            srcList.Add(param)
        Next
        Return BA_SaveAOIParameters(srcList, settingsPath)
    End Function

    Private Sub SetMinPolySize(ByVal hruGdbName As String)
        Dim statResults As BA_DataStatistics        'internal unit is sq km
        Dim minPolyArea As Double
        Dim queryField As String = BA_FIELD_AREA_SQKM
        Dim measureUnit As MeasurementUnit
        If m_hru IsNot Nothing Then
            If m_linearUnit Is Nothing Then
                MessageBox.Show("Could not determine units from DEM. Minimum polygon size will be displayed in square kilometers.")
                measureUnit = MeasurementUnit.SquareKilometers
            ElseIf m_linearUnit.Equals(METER) Then
                measureUnit = MeasurementUnit.SquareKilometers
            ElseIf m_linearUnit.Equals(FOOT) Then
                queryField = BA_FIELD_AREA_ACRE
                measureUnit = MeasurementUnit.Acres
            End If
            If m_hru.AllowNonContiguousHru Then
                statResults = BA_GetAreaStatistics(hruGdbName, BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruPolyVector), False), _
                                                   BA_FIELD_SHAPE_AREA, measureUnit)
                If statResults.Maximum > 0 Then
                    minPolyArea = statResults.Minimum
                Else
                    MessageBox.Show("Can't get min polygon size")
                End If
            Else
                If BA_GetDataStatistics(hruGdbName & BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruVector), False, True), _
                                        queryField, statResults) <> 0 Then
                    MessageBox.Show("Can't get min polygon size")
                Else
                    minPolyArea = statResults.Minimum
                End If
            End If
        End If
        TxtMinPolySize.Text = Format(minPolyArea, "###,###,##0.00000") & " " & BA_EnumDescription(measureUnit)
    End Sub

    Private Sub ManageAoiParameterFields()
        Dim localSettingsPath As String = BA_GetLocalSettingsPath(m_aoi.FilePath)
        m_aoiParamTable = BA_LoadAoiParameters(localSettingsPath)
        If m_aoiParamTable IsNot Nothing Then
            If m_aoiParamTable.ContainsKey(BA_Aoi_Parameter_PE_Obs) Then
                Dim pe_param As AoiParameter = m_aoiParamTable(BA_Aoi_Parameter_PE_Obs)
                If pe_param.ValuesList IsNot Nothing And pe_param.ValuesList.Count = NUM_MONTHS Then m_hasPeObs = True
            End If
            If m_aoiParamTable.ContainsKey(BA_Aoi_Parameter_SR_Obs) Then
                Dim sr_param As AoiParameter = m_aoiParamTable(BA_Aoi_Parameter_SR_Obs)
                If sr_param.ValuesList IsNot Nothing And sr_param.ValuesList.Count = NUM_MONTHS Then m_hasSrObs = True
            End If
        End If
        If m_hasPeObs AndAlso m_hasSrObs Then
            CkPeAndSrObs.Checked = True
        Else
            CkPeAndSrObs.Checked = False
        End If
    End Sub

    Private Sub CkPeAndSrObs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CkPeAndSrObs.CheckedChanged
        If CType(sender, CheckBox).Checked Then
            Dim sb As StringBuilder = New StringBuilder
            If m_hasPeObs = False And m_hasSrObs = False Then
                sb.Append("The observed Potential Evaporation and Solar Radiation have not been calculated for this AOI." & vbCrLf)
                CType(sender, CheckBox).Checked = False
            ElseIf m_hasPeObs = False Then
                sb.Append("The observed Potential Evaporation has not been calculated for this AOI." & vbCrLf)
            ElseIf m_hasSrObs = False Then
                sb.Append("The observed Solar Radiation has not been calculated for this AOI." & vbCrLf)
            End If
            If sb.Length > 0 Then
                sb.Append(vbCrLf & "Please use the BAGIS-P 'PE_Obs and SR_Obs Tool' to manage these parameters." & vbCrLf)
                MessageBox.Show(sb.ToString, "Observed Potential Evaporation and Solar Radiation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub
End Class