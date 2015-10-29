Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.Geodatabase

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
        TxtSrPath.Text = pDatasetName.WorkspaceName.PathName & "\" & pDatasetName.Name

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
End Class