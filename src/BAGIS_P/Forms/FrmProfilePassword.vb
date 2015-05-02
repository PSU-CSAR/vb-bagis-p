Imports System.Windows.Forms

Public Class FrmProfilePassword

    Dim m_adminPassword As String = "NWCC_PSU"
    Dim m_frmProfile As FrmProfileBuilder
    Dim m_frmDataManager As FrmDataManager
    Dim m_frmAddData As FrmAddData

    Public Sub New(ByVal frmProfile As FrmProfileBuilder)
        InitializeComponent()
        m_frmProfile = frmProfile
        TxtPassword.Focus()
    End Sub

    Public Sub New(ByVal frmDataManager As FrmDataManager)
        InitializeComponent()
        m_frmDataManager = frmDataManager
        TxtPassword.Focus()
    End Sub

    Public Sub New(ByVal frmAddData As FrmAddData)
        InitializeComponent()
        m_frmAddData = frmAddData
        TxtPassword.Focus()
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnEnter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEnter.Click
        Dim pExt As BagisPExtension = BagisPExtension.GetExtension
        If TxtPassword.Text.Equals(m_adminPassword) Then
            If m_frmProfile IsNot Nothing Then m_frmProfile.EnableAdminButtons()
            If m_frmDataManager IsNot Nothing Then m_frmDataManager.EnableAdminButtons()
            If m_frmAddData IsNot Nothing Then m_frmAddData.EnableAdminActions()
            pExt.ProfileAdministrator = True
            Me.Close()
        Else
            pExt.ProfileAdministrator = False
            MessageBox.Show("The password you supplied was invalid", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
End Class