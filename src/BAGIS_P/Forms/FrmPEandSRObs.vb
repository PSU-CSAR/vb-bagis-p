Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry

Public Class FrmPEandSRObs

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
        Dim aoi_v As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiVector), False)
        Dim validSpatialReference As Boolean = BA_VectorProjectionMatch(fullPath, aoiGdb & aoi_v)

        If validSpatialReference Then
            TxtSrPath.Text = fullPath
        Else
            Dim bagisPExt As BagisPExtension = BagisPExtension.GetExtension
            MessageBox.Show("The selected layer '" & pDatasetName.Name & "' cannot be used because the projection does not match the AOI projection. Please reproject or select another data layer and try again.", "Invalid projection", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                    MessageBox.Show(oid)
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
    End Sub
End Class