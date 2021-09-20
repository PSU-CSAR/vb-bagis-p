<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmProfileBuilder
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.BtnSelectAoi = New System.Windows.Forms.Button()
        Me.LblHruLayers = New System.Windows.Forms.Label()
        Me.GrdProfiles = New System.Windows.Forms.DataGridView()
        Me.Profiles = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BtnEditProfile = New System.Windows.Forms.Button()
        Me.BtnProfileCopy = New System.Windows.Forms.Button()
        Me.BtnProfileDelete = New System.Windows.Forms.Button()
        Me.BtnProfileNew = New System.Windows.Forms.Button()
        Me.BtnEditMethod = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TxtModelName = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TxtToolboxName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtSelMethod = New System.Windows.Forms.TextBox()
        Me.TxtDescription = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TxtNumMethods = New System.Windows.Forms.TextBox()
        Me.LblCurrentProfile = New System.Windows.Forms.Label()
        Me.TxtProfileName = New System.Windows.Forms.TextBox()
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.GrdMethods = New System.Windows.Forms.DataGridView()
        Me.Methods = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.IncludeMethod = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.BtnAddMethod = New System.Windows.Forms.Button()
        Me.BtnMethodDelete = New System.Windows.Forms.Button()
        Me.BtnMethodNew = New System.Windows.Forms.Button()
        Me.BtnVerify = New System.Windows.Forms.Button()
        Me.BtnCalculate = New System.Windows.Forms.Button()
        Me.PnlProfile = New System.Windows.Forms.Panel()
        Me.LblVerifyMethods = New System.Windows.Forms.Label()
        Me.BtnToggleUse = New System.Windows.Forms.Button()
        Me.LblStatus = New System.Windows.Forms.Label()
        Me.BtnExportProfile = New System.Windows.Forms.Button()
        Me.BtnImport = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GrdHruLayers = New System.Windows.Forms.DataGridView()
        Me.HruName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Completed = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GrdCompleteProfiles = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LblCompletedProfiles = New System.Windows.Forms.Label()
        Me.PnlSubbasin = New System.Windows.Forms.Panel()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.TxtSubbasinCount = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.CboSubbasinId = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.CkAppendSubbasin = New System.Windows.Forms.CheckBox()
        Me.TxtLblPath = New System.Windows.Forms.TextBox()
        Me.TxtNoData = New System.Windows.Forms.TextBox()
        Me.LblNoData = New System.Windows.Forms.Label()
        Me.BtnViewLog = New System.Windows.Forms.Button()
        CType(Me.GrdProfiles, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GrdMethods, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlProfile.SuspendLayout()
        CType(Me.GrdHruLayers, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GrdCompleteProfiles, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlSubbasin.SuspendLayout()
        Me.SuspendLayout()
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(205, 9)
        Me.TxtAoiPath.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(400, 26)
        Me.TxtAoiPath.TabIndex = 100
        '
        'BtnSelectAoi
        '
        Me.BtnSelectAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectAoi.Location = New System.Drawing.Point(4, 6)
        Me.BtnSelectAoi.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnSelectAoi.Name = "BtnSelectAoi"
        Me.BtnSelectAoi.Size = New System.Drawing.Size(116, 29)
        Me.BtnSelectAoi.TabIndex = 54
        Me.BtnSelectAoi.Text = "Select AOI"
        Me.BtnSelectAoi.UseVisualStyleBackColor = True
        '
        'LblHruLayers
        '
        Me.LblHruLayers.AutoSize = True
        Me.LblHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblHruLayers.Location = New System.Drawing.Point(692, 5)
        Me.LblHruLayers.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblHruLayers.Name = "LblHruLayers"
        Me.LblHruLayers.Size = New System.Drawing.Size(170, 20)
        Me.LblHruLayers.TabIndex = 58
        Me.LblHruLayers.Text = "HRU Layers in AOI"
        '
        'GrdProfiles
        '
        Me.GrdProfiles.AllowUserToAddRows = False
        Me.GrdProfiles.AllowUserToDeleteRows = False
        Me.GrdProfiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdProfiles.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Profiles})
        Me.GrdProfiles.Location = New System.Drawing.Point(6, 181)
        Me.GrdProfiles.Margin = New System.Windows.Forms.Padding(4)
        Me.GrdProfiles.Name = "GrdProfiles"
        Me.GrdProfiles.ReadOnly = True
        Me.GrdProfiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GrdProfiles.Size = New System.Drawing.Size(266, 312)
        Me.GrdProfiles.TabIndex = 59
        '
        'Profiles
        '
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Profiles.DefaultCellStyle = DataGridViewCellStyle8
        Me.Profiles.HeaderText = "Profiles"
        Me.Profiles.Name = "Profiles"
        Me.Profiles.ReadOnly = True
        Me.Profiles.Width = 265
        '
        'BtnEditProfile
        '
        Me.BtnEditProfile.Enabled = False
        Me.BtnEditProfile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnEditProfile.Location = New System.Drawing.Point(72, 499)
        Me.BtnEditProfile.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnEditProfile.Name = "BtnEditProfile"
        Me.BtnEditProfile.Size = New System.Drawing.Size(59, 26)
        Me.BtnEditProfile.TabIndex = 63
        Me.BtnEditProfile.Text = "Edit"
        Me.BtnEditProfile.UseVisualStyleBackColor = True
        Me.BtnEditProfile.Visible = False
        '
        'BtnProfileCopy
        '
        Me.BtnProfileCopy.Enabled = False
        Me.BtnProfileCopy.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnProfileCopy.Location = New System.Drawing.Point(208, 499)
        Me.BtnProfileCopy.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnProfileCopy.Name = "BtnProfileCopy"
        Me.BtnProfileCopy.Size = New System.Drawing.Size(59, 26)
        Me.BtnProfileCopy.TabIndex = 62
        Me.BtnProfileCopy.Text = "Copy"
        Me.BtnProfileCopy.UseVisualStyleBackColor = True
        Me.BtnProfileCopy.Visible = False
        '
        'BtnProfileDelete
        '
        Me.BtnProfileDelete.Enabled = False
        Me.BtnProfileDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnProfileDelete.Location = New System.Drawing.Point(136, 499)
        Me.BtnProfileDelete.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnProfileDelete.Name = "BtnProfileDelete"
        Me.BtnProfileDelete.Size = New System.Drawing.Size(66, 26)
        Me.BtnProfileDelete.TabIndex = 61
        Me.BtnProfileDelete.Text = "Delete"
        Me.BtnProfileDelete.UseVisualStyleBackColor = True
        '
        'BtnProfileNew
        '
        Me.BtnProfileNew.Enabled = False
        Me.BtnProfileNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnProfileNew.Location = New System.Drawing.Point(9, 499)
        Me.BtnProfileNew.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnProfileNew.Name = "BtnProfileNew"
        Me.BtnProfileNew.Size = New System.Drawing.Size(59, 26)
        Me.BtnProfileNew.TabIndex = 60
        Me.BtnProfileNew.Text = "New"
        Me.BtnProfileNew.UseVisualStyleBackColor = True
        Me.BtnProfileNew.Visible = False
        '
        'BtnEditMethod
        '
        Me.BtnEditMethod.Enabled = False
        Me.BtnEditMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnEditMethod.Location = New System.Drawing.Point(481, 81)
        Me.BtnEditMethod.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnEditMethod.Name = "BtnEditMethod"
        Me.BtnEditMethod.Size = New System.Drawing.Size(116, 29)
        Me.BtnEditMethod.TabIndex = 78
        Me.BtnEditMethod.Text = "Edit Method"
        Me.BtnEditMethod.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(265, 58)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(116, 20)
        Me.Label8.TabIndex = 77
        Me.Label8.Text = "Model name:"
        '
        'TxtModelName
        '
        Me.TxtModelName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtModelName.Location = New System.Drawing.Point(430, 58)
        Me.TxtModelName.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtModelName.Name = "TxtModelName"
        Me.TxtModelName.ReadOnly = True
        Me.TxtModelName.Size = New System.Drawing.Size(168, 26)
        Me.TxtModelName.TabIndex = 76
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(264, 31)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(131, 20)
        Me.Label7.TabIndex = 75
        Me.Label7.Text = "Toolbox name:"
        '
        'TxtToolboxName
        '
        Me.TxtToolboxName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtToolboxName.Location = New System.Drawing.Point(399, 30)
        Me.TxtToolboxName.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtToolboxName.Name = "TxtToolboxName"
        Me.TxtToolboxName.ReadOnly = True
        Me.TxtToolboxName.Size = New System.Drawing.Size(198, 26)
        Me.TxtToolboxName.TabIndex = 74
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(264, 6)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(145, 20)
        Me.Label1.TabIndex = 71
        Me.Label1.Text = "Current method:"
        '
        'TxtSelMethod
        '
        Me.TxtSelMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSelMethod.Location = New System.Drawing.Point(399, 4)
        Me.TxtSelMethod.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtSelMethod.Name = "TxtSelMethod"
        Me.TxtSelMethod.ReadOnly = True
        Me.TxtSelMethod.Size = New System.Drawing.Size(198, 26)
        Me.TxtSelMethod.TabIndex = 70
        '
        'TxtDescription
        '
        Me.TxtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDescription.Location = New System.Drawing.Point(105, 59)
        Me.TxtDescription.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtDescription.Multiline = True
        Me.TxtDescription.Name = "TxtDescription"
        Me.TxtDescription.ReadOnly = True
        Me.TxtDescription.Size = New System.Drawing.Size(153, 46)
        Me.TxtDescription.TabIndex = 69
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(4, 55)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(112, 20)
        Me.Label4.TabIndex = 68
        Me.Label4.Text = "Description:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(4, 31)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(179, 20)
        Me.Label3.TabIndex = 67
        Me.Label3.Text = "Number of methods:"
        '
        'TxtNumMethods
        '
        Me.TxtNumMethods.Location = New System.Drawing.Point(171, 30)
        Me.TxtNumMethods.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtNumMethods.Name = "TxtNumMethods"
        Me.TxtNumMethods.ReadOnly = True
        Me.TxtNumMethods.Size = New System.Drawing.Size(48, 26)
        Me.TxtNumMethods.TabIndex = 66
        '
        'LblCurrentProfile
        '
        Me.LblCurrentProfile.AutoSize = True
        Me.LblCurrentProfile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCurrentProfile.Location = New System.Drawing.Point(4, 6)
        Me.LblCurrentProfile.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblCurrentProfile.Name = "LblCurrentProfile"
        Me.LblCurrentProfile.Size = New System.Drawing.Size(137, 20)
        Me.LblCurrentProfile.TabIndex = 65
        Me.LblCurrentProfile.Text = "Current profile:"
        '
        'TxtProfileName
        '
        Me.TxtProfileName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtProfileName.Location = New System.Drawing.Point(129, 4)
        Me.TxtProfileName.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtProfileName.Name = "TxtProfileName"
        Me.TxtProfileName.ReadOnly = True
        Me.TxtProfileName.Size = New System.Drawing.Size(132, 26)
        Me.TxtProfileName.TabIndex = 64
        '
        'BtnApply
        '
        Me.BtnApply.Enabled = False
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(139, 111)
        Me.BtnApply.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(134, 29)
        Me.BtnApply.TabIndex = 80
        Me.BtnApply.Text = "Apply Changes"
        Me.BtnApply.UseVisualStyleBackColor = True
        Me.BtnApply.Visible = False
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(811, 549)
        Me.BtnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(75, 29)
        Me.BtnClose.TabIndex = 79
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'GrdMethods
        '
        Me.GrdMethods.AllowUserToAddRows = False
        Me.GrdMethods.AllowUserToDeleteRows = False
        Me.GrdMethods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdMethods.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Methods, Me.ColStatus, Me.IncludeMethod})
        Me.GrdMethods.Location = New System.Drawing.Point(292, 181)
        Me.GrdMethods.Margin = New System.Windows.Forms.Padding(4)
        Me.GrdMethods.Name = "GrdMethods"
        Me.GrdMethods.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GrdMethods.Size = New System.Drawing.Size(304, 312)
        Me.GrdMethods.TabIndex = 81
        '
        'Methods
        '
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Methods.DefaultCellStyle = DataGridViewCellStyle9
        Me.Methods.HeaderText = "Methods"
        Me.Methods.Name = "Methods"
        Me.Methods.ReadOnly = True
        Me.Methods.Width = 150
        '
        'ColStatus
        '
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ColStatus.DefaultCellStyle = DataGridViewCellStyle10
        Me.ColStatus.HeaderText = "Status"
        Me.ColStatus.Name = "ColStatus"
        Me.ColStatus.ReadOnly = True
        Me.ColStatus.Width = 115
        '
        'IncludeMethod
        '
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle11.ForeColor = System.Drawing.Color.Silver
        DataGridViewCellStyle11.NullValue = False
        Me.IncludeMethod.DefaultCellStyle = DataGridViewCellStyle11
        Me.IncludeMethod.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.IncludeMethod.HeaderText = "Use"
        Me.IncludeMethod.Name = "IncludeMethod"
        Me.IncludeMethod.ReadOnly = True
        Me.IncludeMethod.Width = 50
        '
        'BtnAddMethod
        '
        Me.BtnAddMethod.Enabled = False
        Me.BtnAddMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAddMethod.Location = New System.Drawing.Point(415, 499)
        Me.BtnAddMethod.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnAddMethod.Name = "BtnAddMethod"
        Me.BtnAddMethod.Size = New System.Drawing.Size(196, 26)
        Me.BtnAddMethod.TabIndex = 84
        Me.BtnAddMethod.Text = "Add Method(s) to Profile"
        Me.BtnAddMethod.UseVisualStyleBackColor = True
        Me.BtnAddMethod.Visible = False
        '
        'BtnMethodDelete
        '
        Me.BtnMethodDelete.Enabled = False
        Me.BtnMethodDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMethodDelete.Location = New System.Drawing.Point(346, 499)
        Me.BtnMethodDelete.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnMethodDelete.Name = "BtnMethodDelete"
        Me.BtnMethodDelete.Size = New System.Drawing.Size(66, 26)
        Me.BtnMethodDelete.TabIndex = 83
        Me.BtnMethodDelete.Text = "Delete"
        Me.BtnMethodDelete.UseVisualStyleBackColor = True
        Me.BtnMethodDelete.Visible = False
        '
        'BtnMethodNew
        '
        Me.BtnMethodNew.Enabled = False
        Me.BtnMethodNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMethodNew.Location = New System.Drawing.Point(286, 499)
        Me.BtnMethodNew.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnMethodNew.Name = "BtnMethodNew"
        Me.BtnMethodNew.Size = New System.Drawing.Size(59, 26)
        Me.BtnMethodNew.TabIndex = 82
        Me.BtnMethodNew.Text = "New"
        Me.BtnMethodNew.UseVisualStyleBackColor = True
        Me.BtnMethodNew.Visible = False
        '
        'BtnVerify
        '
        Me.BtnVerify.Enabled = False
        Me.BtnVerify.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnVerify.Location = New System.Drawing.Point(610, 148)
        Me.BtnVerify.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnVerify.Name = "BtnVerify"
        Me.BtnVerify.Size = New System.Drawing.Size(71, 29)
        Me.BtnVerify.TabIndex = 85
        Me.BtnVerify.Text = "Verify"
        Me.BtnVerify.UseVisualStyleBackColor = True
        '
        'BtnCalculate
        '
        Me.BtnCalculate.Enabled = False
        Me.BtnCalculate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCalculate.Location = New System.Drawing.Point(682, 148)
        Me.BtnCalculate.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnCalculate.Name = "BtnCalculate"
        Me.BtnCalculate.Size = New System.Drawing.Size(212, 29)
        Me.BtnCalculate.TabIndex = 86
        Me.BtnCalculate.Text = "(Re)Calculate Parameters"
        Me.BtnCalculate.UseVisualStyleBackColor = True
        '
        'PnlProfile
        '
        Me.PnlProfile.Controls.Add(Me.LblVerifyMethods)
        Me.PnlProfile.Controls.Add(Me.BtnToggleUse)
        Me.PnlProfile.Controls.Add(Me.LblStatus)
        Me.PnlProfile.Controls.Add(Me.BtnExportProfile)
        Me.PnlProfile.Controls.Add(Me.BtnImport)
        Me.PnlProfile.Controls.Add(Me.GrdMethods)
        Me.PnlProfile.Controls.Add(Me.GrdProfiles)
        Me.PnlProfile.Controls.Add(Me.BtnProfileNew)
        Me.PnlProfile.Controls.Add(Me.BtnAddMethod)
        Me.PnlProfile.Controls.Add(Me.BtnProfileDelete)
        Me.PnlProfile.Controls.Add(Me.BtnMethodDelete)
        Me.PnlProfile.Controls.Add(Me.BtnProfileCopy)
        Me.PnlProfile.Controls.Add(Me.BtnMethodNew)
        Me.PnlProfile.Controls.Add(Me.BtnEditProfile)
        Me.PnlProfile.Controls.Add(Me.TxtProfileName)
        Me.PnlProfile.Controls.Add(Me.BtnApply)
        Me.PnlProfile.Controls.Add(Me.LblCurrentProfile)
        Me.PnlProfile.Controls.Add(Me.TxtNumMethods)
        Me.PnlProfile.Controls.Add(Me.BtnEditMethod)
        Me.PnlProfile.Controls.Add(Me.Label3)
        Me.PnlProfile.Controls.Add(Me.Label8)
        Me.PnlProfile.Controls.Add(Me.Label4)
        Me.PnlProfile.Controls.Add(Me.TxtModelName)
        Me.PnlProfile.Controls.Add(Me.TxtDescription)
        Me.PnlProfile.Controls.Add(Me.Label7)
        Me.PnlProfile.Controls.Add(Me.TxtSelMethod)
        Me.PnlProfile.Controls.Add(Me.TxtToolboxName)
        Me.PnlProfile.Controls.Add(Me.Label1)
        Me.PnlProfile.Location = New System.Drawing.Point(-2, 41)
        Me.PnlProfile.Margin = New System.Windows.Forms.Padding(4)
        Me.PnlProfile.Name = "PnlProfile"
        Me.PnlProfile.Size = New System.Drawing.Size(612, 541)
        Me.PnlProfile.TabIndex = 87
        '
        'LblVerifyMethods
        '
        Me.LblVerifyMethods.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblVerifyMethods.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblVerifyMethods.ForeColor = System.Drawing.Color.Red
        Me.LblVerifyMethods.Location = New System.Drawing.Point(281, 225)
        Me.LblVerifyMethods.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblVerifyMethods.Name = "LblVerifyMethods"
        Me.LblVerifyMethods.Size = New System.Drawing.Size(327, 117)
        Me.LblVerifyMethods.TabIndex = 91
        Me.LblVerifyMethods.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    Verifying methods. Please wait!"
        Me.LblVerifyMethods.Visible = False
        '
        'BtnToggleUse
        '
        Me.BtnToggleUse.Enabled = False
        Me.BtnToggleUse.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnToggleUse.Location = New System.Drawing.Point(490, 148)
        Me.BtnToggleUse.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnToggleUse.Name = "BtnToggleUse"
        Me.BtnToggleUse.Size = New System.Drawing.Size(109, 29)
        Me.BtnToggleUse.TabIndex = 89
        Me.BtnToggleUse.Text = "Toggle Use"
        Me.BtnToggleUse.UseVisualStyleBackColor = True
        '
        'LblStatus
        '
        Me.LblStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblStatus.ForeColor = System.Drawing.Color.Red
        Me.LblStatus.Location = New System.Drawing.Point(295, 116)
        Me.LblStatus.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblStatus.Name = "LblStatus"
        Me.LblStatus.Size = New System.Drawing.Size(304, 25)
        Me.LblStatus.TabIndex = 88
        '
        'BtnExportProfile
        '
        Me.BtnExportProfile.Enabled = False
        Me.BtnExportProfile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnExportProfile.Location = New System.Drawing.Point(174, 148)
        Me.BtnExportProfile.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnExportProfile.Name = "BtnExportProfile"
        Me.BtnExportProfile.Size = New System.Drawing.Size(116, 29)
        Me.BtnExportProfile.TabIndex = 87
        Me.BtnExportProfile.Text = "Export Profile"
        Me.BtnExportProfile.UseVisualStyleBackColor = True
        Me.BtnExportProfile.Visible = False
        '
        'BtnImport
        '
        Me.BtnImport.Enabled = False
        Me.BtnImport.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnImport.Location = New System.Drawing.Point(5, 148)
        Me.BtnImport.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnImport.Name = "BtnImport"
        Me.BtnImport.Size = New System.Drawing.Size(169, 29)
        Me.BtnImport.TabIndex = 86
        Me.BtnImport.Text = "Import Public Profile"
        Me.BtnImport.UseVisualStyleBackColor = True
        '
        'GrdHruLayers
        '
        Me.GrdHruLayers.AllowUserToAddRows = False
        Me.GrdHruLayers.AllowUserToDeleteRows = False
        Me.GrdHruLayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdHruLayers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.HruName, Me.Completed})
        Me.GrdHruLayers.Location = New System.Drawing.Point(610, 26)
        Me.GrdHruLayers.Margin = New System.Windows.Forms.Padding(4)
        Me.GrdHruLayers.Name = "GrdHruLayers"
        Me.GrdHruLayers.ReadOnly = True
        Me.GrdHruLayers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GrdHruLayers.Size = New System.Drawing.Size(279, 116)
        Me.GrdHruLayers.TabIndex = 87
        '
        'HruName
        '
        DataGridViewCellStyle12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HruName.DefaultCellStyle = DataGridViewCellStyle12
        Me.HruName.HeaderText = "HRU Name"
        Me.HruName.Name = "HruName"
        Me.HruName.ReadOnly = True
        Me.HruName.Width = 145
        '
        'Completed
        '
        DataGridViewCellStyle13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Completed.DefaultCellStyle = DataGridViewCellStyle13
        Me.Completed.HeaderText = "# Complete"
        Me.Completed.Name = "Completed"
        Me.Completed.ReadOnly = True
        Me.Completed.Width = 140
        '
        'GrdCompleteProfiles
        '
        Me.GrdCompleteProfiles.AllowUserToAddRows = False
        Me.GrdCompleteProfiles.AllowUserToDeleteRows = False
        Me.GrdCompleteProfiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdCompleteProfiles.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2})
        Me.GrdCompleteProfiles.Location = New System.Drawing.Point(610, 230)
        Me.GrdCompleteProfiles.Margin = New System.Windows.Forms.Padding(4)
        Me.GrdCompleteProfiles.MultiSelect = False
        Me.GrdCompleteProfiles.Name = "GrdCompleteProfiles"
        Me.GrdCompleteProfiles.ReadOnly = True
        Me.GrdCompleteProfiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GrdCompleteProfiles.Size = New System.Drawing.Size(279, 116)
        Me.GrdCompleteProfiles.TabIndex = 89
        '
        'DataGridViewTextBoxColumn1
        '
        DataGridViewCellStyle14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridViewTextBoxColumn1.DefaultCellStyle = DataGridViewCellStyle14
        Me.DataGridViewTextBoxColumn1.HeaderText = "Profile Name"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 155
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "Completed"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 135
        '
        'LblCompletedProfiles
        '
        Me.LblCompletedProfiles.AutoSize = True
        Me.LblCompletedProfiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCompletedProfiles.Location = New System.Drawing.Point(649, 209)
        Me.LblCompletedProfiles.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblCompletedProfiles.Name = "LblCompletedProfiles"
        Me.LblCompletedProfiles.Size = New System.Drawing.Size(169, 20)
        Me.LblCompletedProfiles.TabIndex = 88
        Me.LblCompletedProfiles.Text = "Completed Profiles"
        '
        'PnlSubbasin
        '
        Me.PnlSubbasin.Controls.Add(Me.RichTextBox1)
        Me.PnlSubbasin.Controls.Add(Me.TxtSubbasinCount)
        Me.PnlSubbasin.Controls.Add(Me.Label10)
        Me.PnlSubbasin.Controls.Add(Me.CboSubbasinId)
        Me.PnlSubbasin.Controls.Add(Me.Label9)
        Me.PnlSubbasin.Controls.Add(Me.CkAppendSubbasin)
        Me.PnlSubbasin.Location = New System.Drawing.Point(611, 380)
        Me.PnlSubbasin.Margin = New System.Windows.Forms.Padding(2)
        Me.PnlSubbasin.Name = "PnlSubbasin"
        Me.PnlSubbasin.Size = New System.Drawing.Size(275, 161)
        Me.PnlSubbasin.TabIndex = 94
        '
        'RichTextBox1
        '
        Me.RichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextBox1.Location = New System.Drawing.Point(4, 98)
        Me.RichTextBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.ReadOnly = True
        Me.RichTextBox1.Size = New System.Drawing.Size(271, 66)
        Me.RichTextBox1.TabIndex = 95
        Me.RichTextBox1.Text = "Please use the Subbasin ID Tool on the " & Global.Microsoft.VisualBasic.ChrW(10) & "BAGIS-P AOI Parameterization menu to " & Global.Microsoft.VisualBasic.ChrW(10) & "man" &
    "age the Subbasin ID Layers"
        '
        'TxtSubbasinCount
        '
        Me.TxtSubbasinCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSubbasinCount.Location = New System.Drawing.Point(220, 79)
        Me.TxtSubbasinCount.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtSubbasinCount.Name = "TxtSubbasinCount"
        Me.TxtSubbasinCount.ReadOnly = True
        Me.TxtSubbasinCount.Size = New System.Drawing.Size(62, 24)
        Me.TxtSubbasinCount.TabIndex = 94
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(4, 80)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(233, 18)
        Me.Label10.TabIndex = 98
        Me.Label10.Text = "# of Subbasins in the ID layer:"
        '
        'CboSubbasinId
        '
        Me.CboSubbasinId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboSubbasinId.FormattingEnabled = True
        Me.CboSubbasinId.Location = New System.Drawing.Point(10, 52)
        Me.CboSubbasinId.Margin = New System.Windows.Forms.Padding(2)
        Me.CboSubbasinId.Name = "CboSubbasinId"
        Me.CboSubbasinId.Size = New System.Drawing.Size(155, 28)
        Me.CboSubbasinId.Sorted = True
        Me.CboSubbasinId.TabIndex = 97
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(6, 32)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(185, 18)
        Me.Label9.TabIndex = 96
        Me.Label9.Text = "Subbasin ID layer used:"
        '
        'CkAppendSubbasin
        '
        Me.CkAppendSubbasin.AutoSize = True
        Me.CkAppendSubbasin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkAppendSubbasin.Location = New System.Drawing.Point(4, 11)
        Me.CkAppendSubbasin.Margin = New System.Windows.Forms.Padding(4)
        Me.CkAppendSubbasin.Name = "CkAppendSubbasin"
        Me.CkAppendSubbasin.Size = New System.Drawing.Size(315, 22)
        Me.CkAppendSubbasin.TabIndex = 95
        Me.CkAppendSubbasin.Text = "Append Subbasin ID to Parameter File"
        Me.CkAppendSubbasin.UseVisualStyleBackColor = True
        '
        'TxtLblPath
        '
        Me.TxtLblPath.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtLblPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtLblPath.Location = New System.Drawing.Point(129, 12)
        Me.TxtLblPath.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtLblPath.Name = "TxtLblPath"
        Me.TxtLblPath.ReadOnly = True
        Me.TxtLblPath.Size = New System.Drawing.Size(75, 19)
        Me.TxtLblPath.TabIndex = 87
        Me.TxtLblPath.Text = "AOI Path:"
        '
        'TxtNoData
        '
        Me.TxtNoData.Location = New System.Drawing.Point(762, 181)
        Me.TxtNoData.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtNoData.Name = "TxtNoData"
        Me.TxtNoData.Size = New System.Drawing.Size(43, 26)
        Me.TxtNoData.TabIndex = 87
        Me.TxtNoData.Text = "-99"
        '
        'LblNoData
        '
        Me.LblNoData.AutoSize = True
        Me.LblNoData.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblNoData.Location = New System.Drawing.Point(632, 182)
        Me.LblNoData.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblNoData.Name = "LblNoData"
        Me.LblNoData.Size = New System.Drawing.Size(137, 20)
        Me.LblNoData.TabIndex = 101
        Me.LblNoData.Text = "No Data Value:"
        '
        'BtnViewLog
        '
        Me.BtnViewLog.Enabled = False
        Me.BtnViewLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnViewLog.Location = New System.Drawing.Point(611, 355)
        Me.BtnViewLog.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnViewLog.Name = "BtnViewLog"
        Me.BtnViewLog.Size = New System.Drawing.Size(91, 29)
        Me.BtnViewLog.TabIndex = 102
        Me.BtnViewLog.Text = "View Log"
        Me.BtnViewLog.UseVisualStyleBackColor = True
        '
        'FrmProfileBuilder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(120.0!, 120.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(895, 584)
        Me.Controls.Add(Me.BtnViewLog)
        Me.Controls.Add(Me.LblNoData)
        Me.Controls.Add(Me.TxtNoData)
        Me.Controls.Add(Me.TxtLblPath)
        Me.Controls.Add(Me.PnlSubbasin)
        Me.Controls.Add(Me.GrdCompleteProfiles)
        Me.Controls.Add(Me.LblCompletedProfiles)
        Me.Controls.Add(Me.GrdHruLayers)
        Me.Controls.Add(Me.PnlProfile)
        Me.Controls.Add(Me.BtnCalculate)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.BtnVerify)
        Me.Controls.Add(Me.LblHruLayers)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.BtnSelectAoi)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "FrmProfileBuilder"
        Me.ShowIcon = False
        Me.Text = "Profile Builder"
        CType(Me.GrdProfiles, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GrdMethods, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlProfile.ResumeLayout(False)
        Me.PnlProfile.PerformLayout()
        CType(Me.GrdHruLayers, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GrdCompleteProfiles, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlSubbasin.ResumeLayout(False)
        Me.PnlSubbasin.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents BtnSelectAoi As System.Windows.Forms.Button
    Friend WithEvents LblHruLayers As System.Windows.Forms.Label
    Friend WithEvents GrdProfiles As System.Windows.Forms.DataGridView
    Friend WithEvents BtnEditProfile As System.Windows.Forms.Button
    Friend WithEvents BtnProfileCopy As System.Windows.Forms.Button
    Friend WithEvents BtnProfileDelete As System.Windows.Forms.Button
    Friend WithEvents BtnProfileNew As System.Windows.Forms.Button
    Friend WithEvents BtnEditMethod As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TxtModelName As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TxtToolboxName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TxtSelMethod As System.Windows.Forms.TextBox
    Friend WithEvents TxtDescription As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TxtNumMethods As System.Windows.Forms.TextBox
    Friend WithEvents LblCurrentProfile As System.Windows.Forms.Label
    Friend WithEvents TxtProfileName As System.Windows.Forms.TextBox
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents GrdMethods As System.Windows.Forms.DataGridView
    Friend WithEvents BtnAddMethod As System.Windows.Forms.Button
    Friend WithEvents BtnMethodDelete As System.Windows.Forms.Button
    Friend WithEvents BtnMethodNew As System.Windows.Forms.Button
    Friend WithEvents BtnVerify As System.Windows.Forms.Button
    Friend WithEvents BtnCalculate As System.Windows.Forms.Button
    Friend WithEvents PnlProfile As System.Windows.Forms.Panel
    Friend WithEvents BtnImport As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Methods As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents IncludeMethod As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents GrdHruLayers As System.Windows.Forms.DataGridView
    Friend WithEvents GrdCompleteProfiles As System.Windows.Forms.DataGridView
    Friend WithEvents LblCompletedProfiles As System.Windows.Forms.Label
    Friend WithEvents PnlSubbasin As System.Windows.Forms.Panel
    Friend WithEvents TxtSubbasinCount As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents CboSubbasinId As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents CkAppendSubbasin As System.Windows.Forms.CheckBox
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents TxtLblPath As System.Windows.Forms.TextBox
    Friend WithEvents TxtNoData As System.Windows.Forms.TextBox
    Friend WithEvents LblNoData As System.Windows.Forms.Label
    Friend WithEvents BtnExportProfile As System.Windows.Forms.Button
    Friend WithEvents LblStatus As System.Windows.Forms.Label
    Friend WithEvents BtnViewLog As System.Windows.Forms.Button
    Friend WithEvents BtnToggleUse As System.Windows.Forms.Button
    Friend WithEvents Profiles As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents HruName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Completed As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LblVerifyMethods As System.Windows.Forms.Label
End Class
