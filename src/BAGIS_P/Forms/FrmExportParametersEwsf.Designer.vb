<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmExportParametersEwsf
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
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.BtnSelectAoi = New System.Windows.Forms.Button()
        Me.LblHruLayers = New System.Windows.Forms.Label()
        Me.LstHruLayers = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtNHru = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtNumParameters = New System.Windows.Forms.TextBox()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.LblParameterTemplate = New System.Windows.Forms.Label()
        Me.TxtParameterTemplate = New System.Windows.Forms.TextBox()
        Me.BtnSetTemplate = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.BtnEditParameters = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TxtOutputFolder = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.BtnSetOutput = New System.Windows.Forms.Button()
        Me.BtnExport = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.LstProfiles = New System.Windows.Forms.ListBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TxtModified = New System.Windows.Forms.TextBox()
        Me.TxtCreated = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TxtVersion = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.BtnEditHruParameters = New System.Windows.Forms.Button()
        Me.TxtDescription = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.BtnDefaultTemplate = New System.Windows.Forms.Button()
        Me.CboResampleHru = New System.Windows.Forms.ComboBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TxtHruResample = New System.Windows.Forms.TextBox()
        Me.TextBox8 = New System.Windows.Forms.TextBox()
        Me.TextBox7 = New System.Windows.Forms.TextBox()
        Me.TxtHruResolution = New System.Windows.Forms.TextBox()
        Me.CboResampleDem = New System.Windows.Forms.ComboBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.TxtDemResample = New System.Windows.Forms.TextBox()
        Me.TextBox9 = New System.Windows.Forms.TextBox()
        Me.TxtDemResolution = New System.Windows.Forms.TextBox()
        Me.TextBox11 = New System.Windows.Forms.TextBox()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.CkParametersOnly = New System.Windows.Forms.CheckBox()
        Me.LblStatus = New System.Windows.Forms.Label()
        Me.TimerStatus = New System.Windows.Forms.Timer(Me.components)
        Me.BtnViewBagisParams = New System.Windows.Forms.Button()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.TxtMinPolySize = New System.Windows.Forms.TextBox()
        Me.LblPreCalculated = New System.Windows.Forms.Label()
        Me.CkUsePreCalculated = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.CkPeAndSrObs = New System.Windows.Forms.CheckBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(176, 9)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(512, 22)
        Me.TxtAoiPath.TabIndex = 62
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(115, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 16)
        Me.Label5.TabIndex = 61
        Me.Label5.Text = "AOI Path:"
        '
        'BtnSelectAoi
        '
        Me.BtnSelectAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectAoi.Location = New System.Drawing.Point(4, 6)
        Me.BtnSelectAoi.Name = "BtnSelectAoi"
        Me.BtnSelectAoi.Size = New System.Drawing.Size(105, 28)
        Me.BtnSelectAoi.TabIndex = 60
        Me.BtnSelectAoi.Text = "Select AOI"
        Me.BtnSelectAoi.UseVisualStyleBackColor = True
        '
        'LblHruLayers
        '
        Me.LblHruLayers.AutoSize = True
        Me.LblHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblHruLayers.Location = New System.Drawing.Point(1, 47)
        Me.LblHruLayers.Name = "LblHruLayers"
        Me.LblHruLayers.Size = New System.Drawing.Size(137, 16)
        Me.LblHruLayers.TabIndex = 64
        Me.LblHruLayers.Text = "HRU Layers in AOI"
        '
        'LstHruLayers
        '
        Me.LstHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstHruLayers.FormattingEnabled = True
        Me.LstHruLayers.ItemHeight = 16
        Me.LstHruLayers.Location = New System.Drawing.Point(4, 67)
        Me.LstHruLayers.Name = "LstHruLayers"
        Me.LstHruLayers.Size = New System.Drawing.Size(150, 100)
        Me.LstHruLayers.TabIndex = 63
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(1, 177)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 16)
        Me.Label1.TabIndex = 65
        Me.Label1.Text = "nhru"
        '
        'TxtNHru
        '
        Me.TxtNHru.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNHru.Location = New System.Drawing.Point(38, 175)
        Me.TxtNHru.Name = "TxtNHru"
        Me.TxtNHru.ReadOnly = True
        Me.TxtNHru.Size = New System.Drawing.Size(60, 22)
        Me.TxtNHru.TabIndex = 66
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(1, 352)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(162, 16)
        Me.Label2.TabIndex = 67
        Me.Label2.Text = "Number of parameters"
        '
        'TxtNumParameters
        '
        Me.TxtNumParameters.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNumParameters.Location = New System.Drawing.Point(6, 370)
        Me.TxtNumParameters.Name = "TxtNumParameters"
        Me.TxtNumParameters.ReadOnly = True
        Me.TxtNumParameters.Size = New System.Drawing.Size(60, 22)
        Me.TxtNumParameters.TabIndex = 68
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(811, 461)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(65, 25)
        Me.BtnClose.TabIndex = 81
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'LblParameterTemplate
        '
        Me.LblParameterTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblParameterTemplate.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LblParameterTemplate.Location = New System.Drawing.Point(166, 47)
        Me.LblParameterTemplate.Name = "LblParameterTemplate"
        Me.LblParameterTemplate.Size = New System.Drawing.Size(153, 16)
        Me.LblParameterTemplate.TabIndex = 82
        Me.LblParameterTemplate.Text = "Parameter Template"
        '
        'TxtParameterTemplate
        '
        Me.TxtParameterTemplate.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtParameterTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtParameterTemplate.ForeColor = System.Drawing.Color.Black
        Me.TxtParameterTemplate.Location = New System.Drawing.Point(323, 44)
        Me.TxtParameterTemplate.Name = "TxtParameterTemplate"
        Me.TxtParameterTemplate.ReadOnly = True
        Me.TxtParameterTemplate.Size = New System.Drawing.Size(325, 22)
        Me.TxtParameterTemplate.TabIndex = 83
        '
        'BtnSetTemplate
        '
        Me.BtnSetTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSetTemplate.Location = New System.Drawing.Point(654, 44)
        Me.BtnSetTemplate.Name = "BtnSetTemplate"
        Me.BtnSetTemplate.Size = New System.Drawing.Size(100, 25)
        Me.BtnSetTemplate.TabIndex = 84
        Me.BtnSetTemplate.Text = "Set Template"
        Me.BtnSetTemplate.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        Me.OpenFileDialog1.Filter = "csv files (*.csv)|*.csv|text files (*.txt)|*.txt*"
        Me.OpenFileDialog1.RestoreDirectory = True
        Me.OpenFileDialog1.Title = "Parameter Template"
        '
        'BtnEditParameters
        '
        Me.BtnEditParameters.Enabled = False
        Me.BtnEditParameters.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnEditParameters.Location = New System.Drawing.Point(728, 107)
        Me.BtnEditParameters.Name = "BtnEditParameters"
        Me.BtnEditParameters.Size = New System.Drawing.Size(147, 25)
        Me.BtnEditParameters.TabIndex = 85
        Me.BtnEditParameters.Text = "Edit Parameters"
        Me.BtnEditParameters.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(166, 102)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(160, 16)
        Me.Label4.TabIndex = 86
        Me.Label4.Text = "Template Description"
        '
        'TxtOutputFolder
        '
        Me.TxtOutputFolder.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtOutputFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtOutputFolder.ForeColor = System.Drawing.Color.Black
        Me.TxtOutputFolder.Location = New System.Drawing.Point(323, 281)
        Me.TxtOutputFolder.Name = "TxtOutputFolder"
        Me.TxtOutputFolder.ReadOnly = True
        Me.TxtOutputFolder.Size = New System.Drawing.Size(394, 22)
        Me.TxtOutputFolder.TabIndex = 89
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(167, 284)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(153, 16)
        Me.Label6.TabIndex = 88
        Me.Label6.Text = "Output Path"
        '
        'BtnSetOutput
        '
        Me.BtnSetOutput.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSetOutput.Location = New System.Drawing.Point(730, 278)
        Me.BtnSetOutput.Name = "BtnSetOutput"
        Me.BtnSetOutput.Size = New System.Drawing.Size(147, 25)
        Me.BtnSetOutput.TabIndex = 90
        Me.BtnSetOutput.Text = "Set Output"
        Me.BtnSetOutput.UseVisualStyleBackColor = True
        '
        'BtnExport
        '
        Me.BtnExport.Enabled = False
        Me.BtnExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnExport.Location = New System.Drawing.Point(740, 461)
        Me.BtnExport.Name = "BtnExport"
        Me.BtnExport.Size = New System.Drawing.Size(65, 25)
        Me.BtnExport.TabIndex = 91
        Me.BtnExport.Text = "Export"
        Me.BtnExport.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(167, 307)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(549, 16)
        Me.Label7.TabIndex = 92
        Me.Label7.Text = "The output file is saved as a plain text ASCII file in eWSF parameter file format" & _
            "."
        '
        'LstProfiles
        '
        Me.LstProfiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstProfiles.FormattingEnabled = True
        Me.LstProfiles.ItemHeight = 16
        Me.LstProfiles.Location = New System.Drawing.Point(4, 223)
        Me.LstProfiles.Name = "LstProfiles"
        Me.LstProfiles.Size = New System.Drawing.Size(150, 100)
        Me.LstProfiles.Sorted = True
        Me.LstProfiles.TabIndex = 94
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(1, 204)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(61, 16)
        Me.Label8.TabIndex = 93
        Me.Label8.Text = "Profiles"
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Filter = "csv files (*.csv)|*.csv"
        Me.SaveFileDialog1.RestoreDirectory = True
        Me.SaveFileDialog1.Title = "Output Path"
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(166, 133)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(75, 16)
        Me.Label9.TabIndex = 95
        Me.Label9.Text = "Modified"
        '
        'TxtModified
        '
        Me.TxtModified.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtModified.Location = New System.Drawing.Point(237, 130)
        Me.TxtModified.Name = "TxtModified"
        Me.TxtModified.ReadOnly = True
        Me.TxtModified.Size = New System.Drawing.Size(125, 22)
        Me.TxtModified.TabIndex = 96
        '
        'TxtCreated
        '
        Me.TxtCreated.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtCreated.Location = New System.Drawing.Point(438, 130)
        Me.TxtCreated.Name = "TxtCreated"
        Me.TxtCreated.ReadOnly = True
        Me.TxtCreated.Size = New System.Drawing.Size(125, 22)
        Me.TxtCreated.TabIndex = 98
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(369, 133)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(75, 16)
        Me.Label10.TabIndex = 97
        Me.Label10.Text = "Created"
        '
        'TxtVersion
        '
        Me.TxtVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtVersion.Location = New System.Drawing.Point(638, 130)
        Me.TxtVersion.Name = "TxtVersion"
        Me.TxtVersion.ReadOnly = True
        Me.TxtVersion.Size = New System.Drawing.Size(75, 22)
        Me.TxtVersion.TabIndex = 100
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(573, 133)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(75, 16)
        Me.Label11.TabIndex = 99
        Me.Label11.Text = "Version"
        '
        'BtnEditHruParameters
        '
        Me.BtnEditHruParameters.Enabled = False
        Me.BtnEditHruParameters.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnEditHruParameters.Location = New System.Drawing.Point(728, 139)
        Me.BtnEditHruParameters.Name = "BtnEditHruParameters"
        Me.BtnEditHruParameters.Size = New System.Drawing.Size(147, 25)
        Me.BtnEditHruParameters.TabIndex = 101
        Me.BtnEditHruParameters.Text = "Edit HRU Parameters"
        Me.BtnEditHruParameters.UseVisualStyleBackColor = True
        '
        'TxtDescription
        '
        Me.TxtDescription.BackColor = System.Drawing.Color.White
        Me.TxtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDescription.ForeColor = System.Drawing.Color.Black
        Me.TxtDescription.Location = New System.Drawing.Point(321, 102)
        Me.TxtDescription.Name = "TxtDescription"
        Me.TxtDescription.Size = New System.Drawing.Size(394, 22)
        Me.TxtDescription.TabIndex = 104
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.ForeColor = System.Drawing.Color.Black
        Me.TextBox1.Location = New System.Drawing.Point(169, 72)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(650, 15)
        Me.TextBox1.TabIndex = 105
        Me.TextBox1.Text = "HRU parameter values not calculated by BAGIS-P will be copied from the parameter " & _
            "template"
        '
        'BtnDefaultTemplate
        '
        Me.BtnDefaultTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDefaultTemplate.Location = New System.Drawing.Point(756, 44)
        Me.BtnDefaultTemplate.Name = "BtnDefaultTemplate"
        Me.BtnDefaultTemplate.Size = New System.Drawing.Size(120, 25)
        Me.BtnDefaultTemplate.TabIndex = 106
        Me.BtnDefaultTemplate.Text = "Default Template"
        Me.BtnDefaultTemplate.UseVisualStyleBackColor = True
        '
        'CboResampleHru
        '
        Me.CboResampleHru.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboResampleHru.FormattingEnabled = True
        Me.CboResampleHru.Location = New System.Drawing.Point(695, 347)
        Me.CboResampleHru.Name = "CboResampleHru"
        Me.CboResampleHru.Size = New System.Drawing.Size(100, 24)
        Me.CboResampleHru.TabIndex = 123
        '
        'TextBox2
        '
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(534, 349)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(166, 20)
        Me.TextBox2.TabIndex = 122
        Me.TextBox2.Text = "Resampling technique"
        '
        'TxtHruResample
        '
        Me.TxtHruResample.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtHruResample.Location = New System.Drawing.Point(455, 347)
        Me.TxtHruResample.Name = "TxtHruResample"
        Me.TxtHruResample.Size = New System.Drawing.Size(67, 22)
        Me.TxtHruResample.TabIndex = 121
        '
        'TextBox8
        '
        Me.TextBox8.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox8.Location = New System.Drawing.Point(362, 349)
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.ReadOnly = True
        Me.TextBox8.Size = New System.Drawing.Size(90, 15)
        Me.TextBox8.TabIndex = 120
        Me.TextBox8.Text = "Resample to"
        '
        'TextBox7
        '
        Me.TextBox7.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox7.Location = New System.Drawing.Point(169, 349)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.ReadOnly = True
        Me.TextBox7.Size = New System.Drawing.Size(121, 15)
        Me.TextBox7.TabIndex = 119
        Me.TextBox7.Text = "HRU resolution"
        '
        'TxtHruResolution
        '
        Me.TxtHruResolution.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtHruResolution.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtHruResolution.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtHruResolution.ForeColor = System.Drawing.Color.Black
        Me.TxtHruResolution.Location = New System.Drawing.Point(279, 349)
        Me.TxtHruResolution.Name = "TxtHruResolution"
        Me.TxtHruResolution.ReadOnly = True
        Me.TxtHruResolution.Size = New System.Drawing.Size(75, 15)
        Me.TxtHruResolution.TabIndex = 124
        '
        'CboResampleDem
        '
        Me.CboResampleDem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboResampleDem.FormattingEnabled = True
        Me.CboResampleDem.Location = New System.Drawing.Point(695, 374)
        Me.CboResampleDem.Name = "CboResampleDem"
        Me.CboResampleDem.Size = New System.Drawing.Size(100, 24)
        Me.CboResampleDem.TabIndex = 130
        '
        'TextBox3
        '
        Me.TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(534, 377)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(166, 20)
        Me.TextBox3.TabIndex = 129
        Me.TextBox3.Text = "Resampling technique"
        '
        'TxtDemResample
        '
        Me.TxtDemResample.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDemResample.Location = New System.Drawing.Point(455, 373)
        Me.TxtDemResample.Name = "TxtDemResample"
        Me.TxtDemResample.Size = New System.Drawing.Size(67, 22)
        Me.TxtDemResample.TabIndex = 128
        '
        'TextBox9
        '
        Me.TextBox9.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox9.Location = New System.Drawing.Point(362, 377)
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.ReadOnly = True
        Me.TextBox9.Size = New System.Drawing.Size(90, 15)
        Me.TextBox9.TabIndex = 127
        Me.TextBox9.Text = "Resample to"
        '
        'TxtDemResolution
        '
        Me.TxtDemResolution.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtDemResolution.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtDemResolution.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDemResolution.ForeColor = System.Drawing.Color.Black
        Me.TxtDemResolution.Location = New System.Drawing.Point(279, 377)
        Me.TxtDemResolution.Name = "TxtDemResolution"
        Me.TxtDemResolution.ReadOnly = True
        Me.TxtDemResolution.Size = New System.Drawing.Size(75, 15)
        Me.TxtDemResolution.TabIndex = 126
        '
        'TextBox11
        '
        Me.TextBox11.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox11.Location = New System.Drawing.Point(169, 376)
        Me.TextBox11.Name = "TextBox11"
        Me.TextBox11.ReadOnly = True
        Me.TextBox11.Size = New System.Drawing.Size(121, 15)
        Me.TextBox11.TabIndex = 125
        Me.TextBox11.Text = "DEM resolution"
        '
        'TextBox5
        '
        Me.TextBox5.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TextBox5.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox5.ForeColor = System.Drawing.Color.Black
        Me.TextBox5.Location = New System.Drawing.Point(170, 398)
        Me.TextBox5.Multiline = True
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.ReadOnly = True
        Me.TextBox5.Size = New System.Drawing.Size(600, 17)
        Me.TextBox5.TabIndex = 132
        Me.TextBox5.Text = "If no resampling resolution is provided, the data layers will be exported using t" & _
            "heir original resolution"
        '
        'CkParametersOnly
        '
        Me.CkParametersOnly.AutoSize = True
        Me.CkParametersOnly.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkParametersOnly.Location = New System.Drawing.Point(699, 435)
        Me.CkParametersOnly.Name = "CkParametersOnly"
        Me.CkParametersOnly.Size = New System.Drawing.Size(178, 20)
        Me.CkParametersOnly.TabIndex = 133
        Me.CkParametersOnly.Text = "Output parameter file only"
        Me.CkParametersOnly.UseVisualStyleBackColor = True
        '
        'LblStatus
        '
        Me.LblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblStatus.ForeColor = System.Drawing.Color.Red
        Me.LblStatus.Location = New System.Drawing.Point(238, 438)
        Me.LblStatus.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblStatus.Name = "LblStatus"
        Me.LblStatus.Size = New System.Drawing.Size(438, 22)
        Me.LblStatus.TabIndex = 134
        '
        'TimerStatus
        '
        '
        'BtnViewBagisParams
        '
        Me.BtnViewBagisParams.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnViewBagisParams.Location = New System.Drawing.Point(3, 396)
        Me.BtnViewBagisParams.Name = "BtnViewBagisParams"
        Me.BtnViewBagisParams.Size = New System.Drawing.Size(135, 25)
        Me.BtnViewBagisParams.TabIndex = 135
        Me.BtnViewBagisParams.Text = "View parameter list"
        Me.BtnViewBagisParams.UseVisualStyleBackColor = True
        '
        'TextBox4
        '
        Me.TextBox4.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox4.Location = New System.Drawing.Point(170, 327)
        Me.TextBox4.Multiline = True
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ReadOnly = True
        Me.TextBox4.Size = New System.Drawing.Size(121, 20)
        Me.TextBox4.TabIndex = 136
        Me.TextBox4.Text = "Min polygon size"
        '
        'TxtMinPolySize
        '
        Me.TxtMinPolySize.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtMinPolySize.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtMinPolySize.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMinPolySize.ForeColor = System.Drawing.Color.Black
        Me.TxtMinPolySize.Location = New System.Drawing.Point(293, 328)
        Me.TxtMinPolySize.Name = "TxtMinPolySize"
        Me.TxtMinPolySize.ReadOnly = True
        Me.TxtMinPolySize.Size = New System.Drawing.Size(159, 15)
        Me.TxtMinPolySize.TabIndex = 137
        '
        'LblPreCalculated
        '
        Me.LblPreCalculated.Enabled = False
        Me.LblPreCalculated.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPreCalculated.Location = New System.Drawing.Point(166, 169)
        Me.LblPreCalculated.Name = "LblPreCalculated"
        Me.LblPreCalculated.Size = New System.Drawing.Size(491, 19)
        Me.LblPreCalculated.TabIndex = 138
        '
        'CkUsePreCalculated
        '
        Me.CkUsePreCalculated.AutoSize = True
        Me.CkUsePreCalculated.Enabled = False
        Me.CkUsePreCalculated.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkUsePreCalculated.Location = New System.Drawing.Point(172, 189)
        Me.CkUsePreCalculated.Name = "CkUsePreCalculated"
        Me.CkUsePreCalculated.Size = New System.Drawing.Size(689, 20)
        Me.CkUsePreCalculated.TabIndex = 139
        Me.CkUsePreCalculated.Text = "Export pre-calculated nhru parameters (duplicate BAGIS-P output parameters will b" & _
            "e overwritten)"
        Me.CkUsePreCalculated.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Enabled = False
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(168, 209)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(680, 18)
        Me.Label3.TabIndex = 140
        Me.Label3.Text = "Please use the BAGIS-P 'Parameters from Layers Tool' to manage pre-calculated HRU" & _
            " parameters"
        '
        'CkPeAndSrObs
        '
        Me.CkPeAndSrObs.AutoSize = True
        Me.CkPeAndSrObs.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkPeAndSrObs.Location = New System.Drawing.Point(172, 231)
        Me.CkPeAndSrObs.Name = "CkPeAndSrObs"
        Me.CkPeAndSrObs.Size = New System.Drawing.Size(582, 20)
        Me.CkPeAndSrObs.TabIndex = 141
        Me.CkPeAndSrObs.Text = "Export observed Potential Evaporation and Solar Radiation  to the nmonths table"
        Me.CkPeAndSrObs.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(168, 251)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(549, 18)
        Me.Label12.TabIndex = 142
        Me.Label12.Text = "Please use the BAGIS-P 'PE_Obs and SR_Obs Tool' to manage these parameters"
        '
        'FrmExportParametersEwsf
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(880, 493)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.CkPeAndSrObs)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.CkUsePreCalculated)
        Me.Controls.Add(Me.LblPreCalculated)
        Me.Controls.Add(Me.TxtMinPolySize)
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.BtnViewBagisParams)
        Me.Controls.Add(Me.LblStatus)
        Me.Controls.Add(Me.CkParametersOnly)
        Me.Controls.Add(Me.TextBox5)
        Me.Controls.Add(Me.CboResampleDem)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.TxtDemResample)
        Me.Controls.Add(Me.TextBox9)
        Me.Controls.Add(Me.TxtDemResolution)
        Me.Controls.Add(Me.TextBox11)
        Me.Controls.Add(Me.TxtHruResolution)
        Me.Controls.Add(Me.CboResampleHru)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TxtHruResample)
        Me.Controls.Add(Me.TextBox8)
        Me.Controls.Add(Me.TextBox7)
        Me.Controls.Add(Me.BtnDefaultTemplate)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.TxtDescription)
        Me.Controls.Add(Me.BtnEditHruParameters)
        Me.Controls.Add(Me.TxtVersion)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.TxtCreated)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.TxtModified)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.LstProfiles)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.BtnExport)
        Me.Controls.Add(Me.BtnSetOutput)
        Me.Controls.Add(Me.TxtOutputFolder)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.BtnEditParameters)
        Me.Controls.Add(Me.BtnSetTemplate)
        Me.Controls.Add(Me.TxtParameterTemplate)
        Me.Controls.Add(Me.LblParameterTemplate)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.TxtNumParameters)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtNHru)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LblHruLayers)
        Me.Controls.Add(Me.LstHruLayers)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.BtnSelectAoi)
        Me.Name = "FrmExportParametersEwsf"
        Me.ShowIcon = False
        Me.Text = "Export Parameters"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents BtnSelectAoi As System.Windows.Forms.Button
    Friend WithEvents LblHruLayers As System.Windows.Forms.Label
    Friend WithEvents LstHruLayers As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TxtNHru As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TxtNumParameters As System.Windows.Forms.TextBox
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents LblParameterTemplate As System.Windows.Forms.Label
    Friend WithEvents TxtParameterTemplate As System.Windows.Forms.TextBox
    Friend WithEvents BtnSetTemplate As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents BtnEditParameters As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TxtOutputFolder As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents BtnSetOutput As System.Windows.Forms.Button
    Friend WithEvents BtnExport As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents LstProfiles As System.Windows.Forms.ListBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TxtModified As System.Windows.Forms.TextBox
    Friend WithEvents TxtCreated As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TxtVersion As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents BtnEditHruParameters As System.Windows.Forms.Button
    Friend WithEvents TxtDescription As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents BtnDefaultTemplate As System.Windows.Forms.Button
    Friend WithEvents CboResampleHru As System.Windows.Forms.ComboBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TxtHruResample As System.Windows.Forms.TextBox
    Friend WithEvents TextBox8 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents TxtHruResolution As System.Windows.Forms.TextBox
    Friend WithEvents CboResampleDem As System.Windows.Forms.ComboBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TxtDemResample As System.Windows.Forms.TextBox
    Friend WithEvents TextBox9 As System.Windows.Forms.TextBox
    Friend WithEvents TxtDemResolution As System.Windows.Forms.TextBox
    Friend WithEvents TextBox11 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents CkParametersOnly As System.Windows.Forms.CheckBox
    Friend WithEvents LblStatus As System.Windows.Forms.Label
    Friend WithEvents TimerStatus As System.Windows.Forms.Timer
    Friend WithEvents BtnViewBagisParams As System.Windows.Forms.Button
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TxtMinPolySize As System.Windows.Forms.TextBox
    Friend WithEvents LblPreCalculated As System.Windows.Forms.Label
    Friend WithEvents CkUsePreCalculated As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents CkPeAndSrObs As System.Windows.Forms.CheckBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
End Class
