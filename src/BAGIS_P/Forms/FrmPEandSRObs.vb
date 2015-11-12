Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry

Public Class FrmPEandSRObs

    Private Const SR_COLUMN_PREFIX As String = "SR"
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
            Exit Sub
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
        TxtPEPath.Text = Data_Path & "\" & pDatasetName.Name
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
        Dim aoiFeatures As IFeatureClass = Nothing
        Dim srGDS As IGeoDataset = Nothing
        Dim fCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing
        Dim pArea As IArea = Nothing
        Dim pCentroid As IPoint = Nothing
        Try
            Dim aoiGDB As String = BA_GeodatabasePath(TxtAoiPath.Text, GeodatabaseNames.Aoi)
            Dim aoiFile As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiVector), False)
            aoiFeatures = BA_OpenFeatureClassFromGDB(aoiGDB, aoiFile)
            If aoiFeatures IsNot Nothing Then
                fCursor = aoiFeatures.Search(Nothing, False)
                If fCursor IsNot Nothing Then
                    ' There is only one feature in AOI vector, always take the first one
                    pFeature = fCursor.NextFeature
                    If pFeature IsNot Nothing Then
                        pArea = pFeature.Shape
                        pCentroid = pArea.Centroid
                    End If
                    Dim oid As Integer = BA_FindClosestFeatureOID(pCentroid, TxtSrPath.Text)
                    If oid > -1 Then
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
                    End If
                End If
            End If
        Catch ex As Exception
            Debug.Print("CalculateSRObs Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            aoiFeatures = Nothing
            srGDS = Nothing
            fCursor = Nothing
            pFeature = Nothing
            pArea = Nothing
            pCentroid = Nothing
        End Try
    End Function

    Private Sub BtnCalculate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCalculate.Click
        CalculateSRObs()
        SaveValues()
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
        End If
    End Sub

    Private Function ExtractSrValues(ByVal oid As Integer, ByRef missingValues As Short) As AoiParameter
        Dim fClass As IFeatureClass = Nothing
        Dim pQF As IQueryFilter = New QueryFilter
        Dim fCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing
        Try
            Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(TxtSrPath.Text)
            Dim parentPath As String = "PleaseReturn"
            Dim fileName As String = BA_GetBareName(TxtSrPath.Text, parentPath)
            If wType = WorkspaceType.Geodatabase Then
                fClass = BA_OpenFeatureClassFromGDB(parentPath, fileName)
            Else
                fClass = BA_OpenFeatureClassFromFile(parentPath, fileName)
            End If
            Dim srValues As List(Of String) = New List(Of String)
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
End Class