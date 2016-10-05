<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmPEandSRObs
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
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.BtnSelectAoi = New System.Windows.Forms.Button()
        Me.TxtSrPath = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnCalculate = New System.Windows.Forms.Button()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.BtnSetSR = New System.Windows.Forms.Button()
        Me.BtnSetPE = New System.Windows.Forms.Button()
        Me.TxtPEPath = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TxtSrDate = New System.Windows.Forms.TextBox()
        Me.TxtSrValue = New System.Windows.Forms.TextBox()
        Me.txtPeValue = New System.Windows.Forms.TextBox()
        Me.txtPEDate = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.BtnAbout = New System.Windows.Forms.Button()
        Me.LblStatus = New System.Windows.Forms.Label()
        Me.TxtStationId = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TxtStationElev = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(172, 38)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(512, 22)
        Me.TxtAoiPath.TabIndex = 68
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(111, 41)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 16)
        Me.Label5.TabIndex = 67
        Me.Label5.Text = "AOI Path:"
        '
        'BtnSelectAoi
        '
        Me.BtnSelectAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectAoi.Location = New System.Drawing.Point(2, 34)
        Me.BtnSelectAoi.Name = "BtnSelectAoi"
        Me.BtnSelectAoi.Size = New System.Drawing.Size(105, 28)
        Me.BtnSelectAoi.TabIndex = 66
        Me.BtnSelectAoi.Text = "Select AOI"
        Me.BtnSelectAoi.UseVisualStyleBackColor = True
        '
        'TxtSrPath
        '
        Me.TxtSrPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtSrPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSrPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtSrPath.Location = New System.Drawing.Point(129, 67)
        Me.TxtSrPath.Name = "TxtSrPath"
        Me.TxtSrPath.ReadOnly = True
        Me.TxtSrPath.Size = New System.Drawing.Size(489, 22)
        Me.TxtSrPath.TabIndex = 70
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(2, 70)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(128, 16)
        Me.Label1.TabIndex = 69
        Me.Label1.Text = "Solar radiation data:"
        '
        'BtnCalculate
        '
        Me.BtnCalculate.Enabled = False
        Me.BtnCalculate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCalculate.Location = New System.Drawing.Point(527, 258)
        Me.BtnCalculate.Name = "BtnCalculate"
        Me.BtnCalculate.Size = New System.Drawing.Size(75, 25)
        Me.BtnCalculate.TabIndex = 93
        Me.BtnCalculate.Text = "Calculate"
        Me.BtnCalculate.UseVisualStyleBackColor = True
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(608, 258)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(75, 25)
        Me.BtnClose.TabIndex = 92
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'BtnSetSR
        '
        Me.BtnSetSR.Enabled = False
        Me.BtnSetSR.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSetSR.Location = New System.Drawing.Point(624, 64)
        Me.BtnSetSR.Name = "BtnSetSR"
        Me.BtnSetSR.Size = New System.Drawing.Size(60, 25)
        Me.BtnSetSR.TabIndex = 94
        Me.BtnSetSR.Text = "Set"
        Me.BtnSetSR.UseVisualStyleBackColor = True
        '
        'BtnSetPE
        '
        Me.BtnSetPE.Enabled = False
        Me.BtnSetPE.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSetPE.Location = New System.Drawing.Point(623, 190)
        Me.BtnSetPE.Name = "BtnSetPE"
        Me.BtnSetPE.Size = New System.Drawing.Size(60, 25)
        Me.BtnSetPE.TabIndex = 98
        Me.BtnSetPE.Text = "Set"
        Me.BtnSetPE.UseVisualStyleBackColor = True
        '
        'TxtPEPath
        '
        Me.TxtPEPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtPEPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPEPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtPEPath.Location = New System.Drawing.Point(138, 193)
        Me.TxtPEPath.Name = "TxtPEPath"
        Me.TxtPEPath.ReadOnly = True
        Me.TxtPEPath.Size = New System.Drawing.Size(480, 22)
        Me.TxtPEPath.TabIndex = 97
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(1, 196)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(130, 16)
        Me.Label2.TabIndex = 96
        Me.Label2.Text = "Potential evap. data:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(2, 97)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(129, 16)
        Me.Label3.TabIndex = 99
        Me.Label3.Text = "Date last calculated:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(251, 96)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(95, 16)
        Me.Label4.TabIndex = 100
        Me.Label4.Text = "January value:"
        '
        'TxtSrDate
        '
        Me.TxtSrDate.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtSrDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSrDate.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtSrDate.Location = New System.Drawing.Point(136, 94)
        Me.TxtSrDate.Name = "TxtSrDate"
        Me.TxtSrDate.ReadOnly = True
        Me.TxtSrDate.Size = New System.Drawing.Size(94, 22)
        Me.TxtSrDate.TabIndex = 101
        '
        'TxtSrValue
        '
        Me.TxtSrValue.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtSrValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSrValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtSrValue.Location = New System.Drawing.Point(360, 94)
        Me.TxtSrValue.Name = "TxtSrValue"
        Me.TxtSrValue.ReadOnly = True
        Me.TxtSrValue.Size = New System.Drawing.Size(94, 22)
        Me.TxtSrValue.TabIndex = 102
        Me.TxtSrValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPeValue
        '
        Me.txtPeValue.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.txtPeValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPeValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtPeValue.Location = New System.Drawing.Point(360, 221)
        Me.txtPeValue.Name = "txtPeValue"
        Me.txtPeValue.ReadOnly = True
        Me.txtPeValue.Size = New System.Drawing.Size(94, 22)
        Me.txtPeValue.TabIndex = 106
        Me.txtPeValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPEDate
        '
        Me.txtPEDate.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.txtPEDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPEDate.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtPEDate.Location = New System.Drawing.Point(136, 221)
        Me.txtPEDate.Name = "txtPEDate"
        Me.txtPEDate.ReadOnly = True
        Me.txtPEDate.Size = New System.Drawing.Size(94, 22)
        Me.txtPEDate.TabIndex = 105
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(251, 224)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(95, 16)
        Me.Label6.TabIndex = 104
        Me.Label6.Text = "January value:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(1, 224)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(129, 16)
        Me.Label7.TabIndex = 103
        Me.Label7.Text = "Date last calculated:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(2, 9)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(483, 16)
        Me.Label8.TabIndex = 116
        Me.Label8.Text = "Extract the potential evap and solar radiation observations for an AOI"
        '
        'BtnAbout
        '
        Me.BtnAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAbout.Location = New System.Drawing.Point(491, 4)
        Me.BtnAbout.Name = "BtnAbout"
        Me.BtnAbout.Size = New System.Drawing.Size(105, 28)
        Me.BtnAbout.TabIndex = 115
        Me.BtnAbout.Text = "Tell me more"
        Me.BtnAbout.UseVisualStyleBackColor = True
        '
        'LblStatus
        '
        Me.LblStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblStatus.ForeColor = System.Drawing.Color.Red
        Me.LblStatus.Location = New System.Drawing.Point(5, 251)
        Me.LblStatus.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblStatus.Name = "LblStatus"
        Me.LblStatus.Size = New System.Drawing.Size(364, 20)
        Me.LblStatus.TabIndex = 117
        '
        'TxtStationId
        '
        Me.TxtStationId.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtStationId.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtStationId.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtStationId.Location = New System.Drawing.Point(136, 123)
        Me.TxtStationId.Name = "TxtStationId"
        Me.TxtStationId.ReadOnly = True
        Me.TxtStationId.Size = New System.Drawing.Size(94, 22)
        Me.TxtStationId.TabIndex = 119
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(2, 126)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(68, 16)
        Me.Label9.TabIndex = 118
        Me.Label9.Text = "Station ID:"
        '
        'TxtStationElev
        '
        Me.TxtStationElev.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtStationElev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtStationElev.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtStationElev.Location = New System.Drawing.Point(360, 123)
        Me.TxtStationElev.Name = "TxtStationElev"
        Me.TxtStationElev.ReadOnly = True
        Me.TxtStationElev.Size = New System.Drawing.Size(94, 22)
        Me.TxtStationElev.TabIndex = 121
        Me.TxtStationElev.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(251, 127)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(110, 16)
        Me.Label10.TabIndex = 120
        Me.Label10.Text = "Station elevation:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(459, 127)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(49, 16)
        Me.Label11.TabIndex = 122
        Me.Label11.Text = "meters"
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label12.Location = New System.Drawing.Point(2, 151)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(693, 37)
        Me.Label12.TabIndex = 123
        Me.Label12.Text = "Note: The station elevation is used to calculate the basin_tsta_hru parameter. Th" & _
    "is calculation requires the hru_elev parameter to also be calculated in meters"
        '
        'FrmPEandSRObs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(696, 295)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.TxtStationElev)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.TxtStationId)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.LblStatus)
        Me.Controls.Add(Me.BtnAbout)
        Me.Controls.Add(Me.txtPeValue)
        Me.Controls.Add(Me.txtPEDate)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.TxtSrValue)
        Me.Controls.Add(Me.TxtSrDate)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.BtnSetPE)
        Me.Controls.Add(Me.TxtPEPath)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.BtnSetSR)
        Me.Controls.Add(Me.BtnCalculate)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.TxtSrPath)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.BtnSelectAoi)
        Me.Controls.Add(Me.Label8)
        Me.Name = "FrmPEandSRObs"
        Me.ShowIcon = False
        Me.Text = "Calculate PE and SR Obs parameters"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents BtnSelectAoi As System.Windows.Forms.Button
    Friend WithEvents TxtSrPath As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents BtnCalculate As System.Windows.Forms.Button
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents BtnSetSR As System.Windows.Forms.Button
    Friend WithEvents BtnSetPE As System.Windows.Forms.Button
    Friend WithEvents TxtPEPath As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TxtSrDate As System.Windows.Forms.TextBox
    Friend WithEvents TxtSrValue As System.Windows.Forms.TextBox
    Friend WithEvents txtPeValue As System.Windows.Forms.TextBox
    Friend WithEvents txtPEDate As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents BtnAbout As System.Windows.Forms.Button
    Friend WithEvents LblStatus As System.Windows.Forms.Label
    Friend WithEvents TxtStationId As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TxtStationElev As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
End Class
