<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmAddData
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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtDescription = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TxtSource = New System.Windows.Forms.TextBox()
        Me.BtnSelectSource = New System.Windows.Forms.Button()
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.PnlJhCoeff = New System.Windows.Forms.Panel()
        Me.rdoOtherTemp = New System.Windows.Forms.RadioButton()
        Me.rdoAugTMax = New System.Windows.Forms.RadioButton()
        Me.rdoAugTMin = New System.Windows.Forms.RadioButton()
        Me.rdoJulTMax = New System.Windows.Forms.RadioButton()
        Me.rdoJulTMin = New System.Windows.Forms.RadioButton()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.CboDataType = New System.Windows.Forms.ComboBox()
        Me.LblUnits = New System.Windows.Forms.Label()
        Me.CboUnits = New System.Windows.Forms.ComboBox()
        Me.PnlJhCoeff.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(2, 69)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 16)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Name:"
        '
        'TxtName
        '
        Me.TxtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtName.Location = New System.Drawing.Point(95, 66)
        Me.TxtName.Name = "TxtName"
        Me.TxtName.ReadOnly = True
        Me.TxtName.Size = New System.Drawing.Size(295, 22)
        Me.TxtName.TabIndex = 13
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(2, 97)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 16)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Description:"
        '
        'TxtDescription
        '
        Me.TxtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDescription.Location = New System.Drawing.Point(95, 97)
        Me.TxtDescription.Multiline = True
        Me.TxtDescription.Name = "TxtDescription"
        Me.TxtDescription.ReadOnly = True
        Me.TxtDescription.Size = New System.Drawing.Size(570, 58)
        Me.TxtDescription.TabIndex = 31
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(2, 168)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 16)
        Me.Label3.TabIndex = 33
        Me.Label3.Text = "Source:"
        '
        'TxtSource
        '
        Me.TxtSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSource.Location = New System.Drawing.Point(95, 166)
        Me.TxtSource.Name = "TxtSource"
        Me.TxtSource.ReadOnly = True
        Me.TxtSource.Size = New System.Drawing.Size(502, 22)
        Me.TxtSource.TabIndex = 32
        '
        'BtnSelectSource
        '
        Me.BtnSelectSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectSource.Location = New System.Drawing.Point(603, 165)
        Me.BtnSelectSource.Name = "BtnSelectSource"
        Me.BtnSelectSource.Size = New System.Drawing.Size(64, 25)
        Me.BtnSelectSource.TabIndex = 34
        Me.BtnSelectSource.Text = "Select"
        Me.BtnSelectSource.UseVisualStyleBackColor = True
        '
        'BtnApply
        '
        Me.BtnApply.Enabled = False
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(607, 220)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(64, 25)
        Me.BtnApply.TabIndex = 36
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(537, 220)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(64, 25)
        Me.BtnCancel.TabIndex = 35
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'PnlJhCoeff
        '
        Me.PnlJhCoeff.Controls.Add(Me.rdoOtherTemp)
        Me.PnlJhCoeff.Controls.Add(Me.rdoAugTMax)
        Me.PnlJhCoeff.Controls.Add(Me.rdoAugTMin)
        Me.PnlJhCoeff.Controls.Add(Me.rdoJulTMax)
        Me.PnlJhCoeff.Controls.Add(Me.rdoJulTMin)
        Me.PnlJhCoeff.Enabled = False
        Me.PnlJhCoeff.Location = New System.Drawing.Point(217, 6)
        Me.PnlJhCoeff.Name = "PnlJhCoeff"
        Me.PnlJhCoeff.Size = New System.Drawing.Size(454, 30)
        Me.PnlJhCoeff.TabIndex = 47
        Me.PnlJhCoeff.Visible = False
        '
        'rdoOtherTemp
        '
        Me.rdoOtherTemp.AutoSize = True
        Me.rdoOtherTemp.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rdoOtherTemp.Location = New System.Drawing.Point(317, 3)
        Me.rdoOtherTemp.Name = "rdoOtherTemp"
        Me.rdoOtherTemp.Size = New System.Drawing.Size(139, 20)
        Me.rdoOtherTemp.TabIndex = 4
        Me.rdoOtherTemp.TabStop = True
        Me.rdoOtherTemp.Text = "Other Temperature"
        Me.rdoOtherTemp.UseVisualStyleBackColor = True
        '
        'rdoAugTMax
        '
        Me.rdoAugTMax.AutoSize = True
        Me.rdoAugTMax.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rdoAugTMax.Location = New System.Drawing.Point(233, 3)
        Me.rdoAugTMax.Name = "rdoAugTMax"
        Me.rdoAugTMax.Size = New System.Drawing.Size(78, 20)
        Me.rdoAugTMax.TabIndex = 3
        Me.rdoAugTMax.TabStop = True
        Me.rdoAugTMax.Text = "Aug Max"
        Me.rdoAugTMax.UseVisualStyleBackColor = True
        '
        'rdoAugTMin
        '
        Me.rdoAugTMin.AutoSize = True
        Me.rdoAugTMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rdoAugTMin.Location = New System.Drawing.Point(153, 3)
        Me.rdoAugTMin.Name = "rdoAugTMin"
        Me.rdoAugTMin.Size = New System.Drawing.Size(74, 20)
        Me.rdoAugTMin.TabIndex = 2
        Me.rdoAugTMin.TabStop = True
        Me.rdoAugTMin.Text = "Aug Min"
        Me.rdoAugTMin.UseVisualStyleBackColor = True
        '
        'rdoJulTMax
        '
        Me.rdoJulTMax.AutoSize = True
        Me.rdoJulTMax.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rdoJulTMax.Location = New System.Drawing.Point(76, 3)
        Me.rdoJulTMax.Name = "rdoJulTMax"
        Me.rdoJulTMax.Size = New System.Drawing.Size(71, 20)
        Me.rdoJulTMax.TabIndex = 1
        Me.rdoJulTMax.TabStop = True
        Me.rdoJulTMax.Text = "Jul Max"
        Me.rdoJulTMax.UseVisualStyleBackColor = True
        '
        'rdoJulTMin
        '
        Me.rdoJulTMin.AutoSize = True
        Me.rdoJulTMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rdoJulTMin.Location = New System.Drawing.Point(3, 3)
        Me.rdoJulTMin.Name = "rdoJulTMin"
        Me.rdoJulTMin.Size = New System.Drawing.Size(67, 20)
        Me.rdoJulTMin.TabIndex = 0
        Me.rdoJulTMin.TabStop = True
        Me.rdoJulTMin.Text = "Jul Min"
        Me.rdoJulTMin.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(2, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 16)
        Me.Label4.TabIndex = 42
        Me.Label4.Text = "Data type:"
        '
        'CboDataType
        '
        Me.CboDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboDataType.Enabled = False
        Me.CboDataType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboDataType.FormattingEnabled = True
        Me.CboDataType.Location = New System.Drawing.Point(88, 6)
        Me.CboDataType.Name = "CboDataType"
        Me.CboDataType.Size = New System.Drawing.Size(121, 24)
        Me.CboDataType.TabIndex = 44
        '
        'LblUnits
        '
        Me.LblUnits.AutoSize = True
        Me.LblUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblUnits.Location = New System.Drawing.Point(2, 39)
        Me.LblUnits.Name = "LblUnits"
        Me.LblUnits.Size = New System.Drawing.Size(39, 16)
        Me.LblUnits.TabIndex = 43
        Me.LblUnits.Text = "Unit:"
        Me.LblUnits.Visible = False
        '
        'CboUnits
        '
        Me.CboUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboUnits.Enabled = False
        Me.CboUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboUnits.FormattingEnabled = True
        Me.CboUnits.Location = New System.Drawing.Point(88, 36)
        Me.CboUnits.Name = "CboUnits"
        Me.CboUnits.Size = New System.Drawing.Size(121, 24)
        Me.CboUnits.TabIndex = 45
        Me.CboUnits.Visible = False
        '
        'FrmAddData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(677, 248)
        Me.Controls.Add(Me.CboUnits)
        Me.Controls.Add(Me.PnlJhCoeff)
        Me.Controls.Add(Me.LblUnits)
        Me.Controls.Add(Me.CboDataType)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.BtnApply)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnSelectSource)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TxtSource)
        Me.Controls.Add(Me.TxtDescription)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "FrmAddData"
        Me.ShowIcon = False
        Me.Text = "Data Source Editor"
        Me.PnlJhCoeff.ResumeLayout(False)
        Me.PnlJhCoeff.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TxtName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TxtDescription As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TxtSource As System.Windows.Forms.TextBox
    Friend WithEvents BtnSelectSource As System.Windows.Forms.Button
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents PnlJhCoeff As System.Windows.Forms.Panel
    Friend WithEvents rdoAugTMax As System.Windows.Forms.RadioButton
    Friend WithEvents rdoAugTMin As System.Windows.Forms.RadioButton
    Friend WithEvents rdoJulTMax As System.Windows.Forms.RadioButton
    Friend WithEvents rdoJulTMin As System.Windows.Forms.RadioButton
    Friend WithEvents rdoOtherTemp As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents CboDataType As System.Windows.Forms.ComboBox
    Friend WithEvents LblUnits As System.Windows.Forms.Label
    Friend WithEvents CboUnits As System.Windows.Forms.ComboBox
End Class
