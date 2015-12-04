<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmEditMethod
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtMethod = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TxtModelName = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TxtToolboxName = New System.Windows.Forms.TextBox()
        Me.TxtDescription = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TxtToolboxPath = New System.Windows.Forms.TextBox()
        Me.BtnModel = New System.Windows.Forms.Button()
        Me.LblModelName = New System.Windows.Forms.Label()
        Me.LblFieldNames = New System.Windows.Forms.Label()
        Me.TxtFieldNames = New System.Windows.Forms.TextBox()
        Me.LstMethods = New System.Windows.Forms.ListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.BtnEditModel = New System.Windows.Forms.Button()
        Me.BtnResetToolboxPath = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(416, 12)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(168, 25)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "Current method:"
        '
        'TxtMethod
        '
        Me.TxtMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMethod.Location = New System.Drawing.Point(705, 9)
        Me.TxtMethod.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TxtMethod.Name = "TxtMethod"
        Me.TxtMethod.ReadOnly = True
        Me.TxtMethod.Size = New System.Drawing.Size(320, 30)
        Me.TxtMethod.TabIndex = 13
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(416, 55)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(137, 25)
        Me.Label8.TabIndex = 28
        Me.Label8.Text = "Model name:"
        '
        'TxtModelName
        '
        Me.TxtModelName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtModelName.Location = New System.Drawing.Point(705, 51)
        Me.TxtModelName.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TxtModelName.Name = "TxtModelName"
        Me.TxtModelName.ReadOnly = True
        Me.TxtModelName.Size = New System.Drawing.Size(266, 30)
        Me.TxtModelName.TabIndex = 27
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(416, 102)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(156, 25)
        Me.Label7.TabIndex = 26
        Me.Label7.Text = "Toolbox name:"
        '
        'TxtToolboxName
        '
        Me.TxtToolboxName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtToolboxName.Location = New System.Drawing.Point(705, 97)
        Me.TxtToolboxName.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TxtToolboxName.Name = "TxtToolboxName"
        Me.TxtToolboxName.ReadOnly = True
        Me.TxtToolboxName.Size = New System.Drawing.Size(430, 30)
        Me.TxtToolboxName.TabIndex = 25
        '
        'TxtDescription
        '
        Me.TxtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDescription.Location = New System.Drawing.Point(705, 180)
        Me.TxtDescription.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TxtDescription.Multiline = True
        Me.TxtDescription.Name = "TxtDescription"
        Me.TxtDescription.Size = New System.Drawing.Size(430, 87)
        Me.TxtDescription.TabIndex = 30
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(416, 183)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(127, 25)
        Me.Label4.TabIndex = 29
        Me.Label4.Text = "Description:"
        '
        'BtnApply
        '
        Me.BtnApply.Enabled = False
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(1044, 566)
        Me.BtnApply.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(96, 38)
        Me.BtnApply.TabIndex = 33
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(694, 566)
        Me.BtnClose.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(96, 38)
        Me.BtnClose.TabIndex = 32
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(416, 143)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(145, 25)
        Me.Label3.TabIndex = 35
        Me.Label3.Text = "Toolbox path:"
        '
        'TxtToolboxPath
        '
        Me.TxtToolboxPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtToolboxPath.Location = New System.Drawing.Point(705, 138)
        Me.TxtToolboxPath.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TxtToolboxPath.Name = "TxtToolboxPath"
        Me.TxtToolboxPath.ReadOnly = True
        Me.TxtToolboxPath.Size = New System.Drawing.Size(430, 30)
        Me.TxtToolboxPath.TabIndex = 34
        '
        'BtnModel
        '
        Me.BtnModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnModel.Location = New System.Drawing.Point(982, 48)
        Me.BtnModel.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnModel.Name = "BtnModel"
        Me.BtnModel.Size = New System.Drawing.Size(154, 38)
        Me.BtnModel.TabIndex = 36
        Me.BtnModel.Text = "Select model"
        Me.BtnModel.UseVisualStyleBackColor = True
        '
        'LblModelName
        '
        Me.LblModelName.AutoSize = True
        Me.LblModelName.Location = New System.Drawing.Point(470, 82)
        Me.LblModelName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblModelName.Name = "LblModelName"
        Me.LblModelName.Size = New System.Drawing.Size(57, 20)
        Me.LblModelName.TabIndex = 37
        Me.LblModelName.Text = "Label5"
        Me.LblModelName.Visible = False
        '
        'LblFieldNames
        '
        Me.LblFieldNames.AutoSize = True
        Me.LblFieldNames.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblFieldNames.Location = New System.Drawing.Point(416, 280)
        Me.LblFieldNames.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblFieldNames.Name = "LblFieldNames"
        Me.LblFieldNames.Size = New System.Drawing.Size(152, 25)
        Me.LblFieldNames.TabIndex = 51
        Me.LblFieldNames.Text = "Field name(s):"
        '
        'TxtFieldNames
        '
        Me.TxtFieldNames.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtFieldNames.Location = New System.Drawing.Point(705, 280)
        Me.TxtFieldNames.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TxtFieldNames.Name = "TxtFieldNames"
        Me.TxtFieldNames.ReadOnly = True
        Me.TxtFieldNames.Size = New System.Drawing.Size(430, 30)
        Me.TxtFieldNames.TabIndex = 50
        '
        'LstMethods
        '
        Me.LstMethods.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstMethods.FormattingEnabled = True
        Me.LstMethods.ItemHeight = 25
        Me.LstMethods.Location = New System.Drawing.Point(8, 40)
        Me.LstMethods.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.LstMethods.Name = "LstMethods"
        Me.LstMethods.Size = New System.Drawing.Size(386, 304)
        Me.LstMethods.Sorted = True
        Me.LstMethods.TabIndex = 52
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(8, 9)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(132, 25)
        Me.Label2.TabIndex = 53
        Me.Label2.Text = "All methods:"
        '
        'BtnDelete
        '
        Me.BtnDelete.Enabled = False
        Me.BtnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDelete.Location = New System.Drawing.Point(804, 566)
        Me.BtnDelete.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(96, 38)
        Me.BtnDelete.TabIndex = 54
        Me.BtnDelete.Text = "Delete"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'BtnEditModel
        '
        Me.BtnEditModel.Enabled = False
        Me.BtnEditModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnEditModel.Location = New System.Drawing.Point(912, 566)
        Me.BtnEditModel.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnEditModel.Name = "BtnEditModel"
        Me.BtnEditModel.Size = New System.Drawing.Size(122, 38)
        Me.BtnEditModel.TabIndex = 55
        Me.BtnEditModel.Text = "Edit Model"
        Me.BtnEditModel.UseVisualStyleBackColor = True
        '
        'BtnResetToolboxPath
        '
        Me.BtnResetToolboxPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnResetToolboxPath.Location = New System.Drawing.Point(12, 566)
        Me.BtnResetToolboxPath.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnResetToolboxPath.Name = "BtnResetToolboxPath"
        Me.BtnResetToolboxPath.Size = New System.Drawing.Size(218, 38)
        Me.BtnResetToolboxPath.TabIndex = 56
        Me.BtnResetToolboxPath.Text = "Reset toolbox path(s)"
        Me.BtnResetToolboxPath.UseVisualStyleBackColor = True
        '
        'FrmEditMethod
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(144.0!, 144.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(1155, 615)
        Me.Controls.Add(Me.BtnResetToolboxPath)
        Me.Controls.Add(Me.BtnEditModel)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.LstMethods)
        Me.Controls.Add(Me.LblFieldNames)
        Me.Controls.Add(Me.TxtFieldNames)
        Me.Controls.Add(Me.LblModelName)
        Me.Controls.Add(Me.BtnModel)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TxtToolboxPath)
        Me.Controls.Add(Me.BtnApply)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.TxtDescription)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.TxtModelName)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.TxtToolboxName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtMethod)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "FrmEditMethod"
        Me.ShowIcon = False
        Me.Text = "Method editor"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TxtMethod As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TxtModelName As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TxtToolboxName As System.Windows.Forms.TextBox
    Friend WithEvents TxtDescription As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TxtToolboxPath As System.Windows.Forms.TextBox
    Friend WithEvents BtnModel As System.Windows.Forms.Button
    Friend WithEvents LblModelName As System.Windows.Forms.Label
    Friend WithEvents LblFieldNames As System.Windows.Forms.Label
    Friend WithEvents TxtFieldNames As System.Windows.Forms.TextBox
    Friend WithEvents LstMethods As System.Windows.Forms.ListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents BtnDelete As System.Windows.Forms.Button
    Friend WithEvents BtnEditModel As System.Windows.Forms.Button
    Friend WithEvents BtnResetToolboxPath As System.Windows.Forms.Button
End Class
