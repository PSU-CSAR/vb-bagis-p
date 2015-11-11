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
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.BtnSetPE = New System.Windows.Forms.Button()
        Me.TxtPEPath = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(174, 10)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(512, 22)
        Me.TxtAoiPath.TabIndex = 68
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(113, 13)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 16)
        Me.Label5.TabIndex = 67
        Me.Label5.Text = "AOI Path:"
        '
        'BtnSelectAoi
        '
        Me.BtnSelectAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectAoi.Location = New System.Drawing.Point(4, 6)
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
        Me.TxtSrPath.Location = New System.Drawing.Point(131, 39)
        Me.TxtSrPath.Name = "TxtSrPath"
        Me.TxtSrPath.ReadOnly = True
        Me.TxtSrPath.Size = New System.Drawing.Size(489, 22)
        Me.TxtSrPath.TabIndex = 70
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(4, 42)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(128, 16)
        Me.Label1.TabIndex = 69
        Me.Label1.Text = "Solar radiation data:"
        '
        'BtnCalculate
        '
        Me.BtnCalculate.Enabled = False
        Me.BtnCalculate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCalculate.Location = New System.Drawing.Point(530, 230)
        Me.BtnCalculate.Name = "BtnCalculate"
        Me.BtnCalculate.Size = New System.Drawing.Size(75, 25)
        Me.BtnCalculate.TabIndex = 93
        Me.BtnCalculate.Text = "Calculate"
        Me.BtnCalculate.UseVisualStyleBackColor = True
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(611, 230)
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
        Me.BtnSetSR.Location = New System.Drawing.Point(626, 37)
        Me.BtnSetSR.Name = "BtnSetSR"
        Me.BtnSetSR.Size = New System.Drawing.Size(60, 25)
        Me.BtnSetSR.TabIndex = 94
        Me.BtnSetSR.Text = "Set"
        Me.BtnSetSR.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.ForeColor = System.Drawing.Color.Black
        Me.TextBox1.Location = New System.Drawing.Point(7, 94)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(612, 42)
        Me.TextBox1.TabIndex = 95
        Me.TextBox1.Text = "Note: Please select the data layer for January; BAGIS-P will locate the remaining" & _
            " 11 months using January file name for reference (ie: the location of 01 in the " & _
            "name)"
        '
        'BtnSetPE
        '
        Me.BtnSetPE.Enabled = False
        Me.BtnSetPE.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSetPE.Location = New System.Drawing.Point(626, 67)
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
        Me.TxtPEPath.Location = New System.Drawing.Point(131, 67)
        Me.TxtPEPath.Name = "TxtPEPath"
        Me.TxtPEPath.ReadOnly = True
        Me.TxtPEPath.Size = New System.Drawing.Size(489, 22)
        Me.TxtPEPath.TabIndex = 97
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(4, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(130, 16)
        Me.Label2.TabIndex = 96
        Me.Label2.Text = "Potential evap. data:"
        '
        'FrmPEandSRObs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(696, 262)
        Me.Controls.Add(Me.BtnSetPE)
        Me.Controls.Add(Me.TxtPEPath)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.BtnSetSR)
        Me.Controls.Add(Me.BtnCalculate)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.TxtSrPath)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.BtnSelectAoi)
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
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents BtnSetPE As System.Windows.Forms.Button
    Friend WithEvents TxtPEPath As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
