﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmDataManager
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.AoiData = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.IsValid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SourceName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Description = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataLayerSource = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BtnAdd = New System.Windows.Forms.Button()
        Me.BtnEdit = New System.Windows.Forms.Button()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.BtnClip = New System.Windows.Forms.Button()
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.BtnSelectAoi = New System.Windows.Forms.Button()
        Me.PnlAoi = New System.Windows.Forms.Panel()
        Me.PnlMain = New System.Windows.Forms.Panel()
        Me.BtnDefaultSettings = New System.Windows.Forms.Button()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlAoi.SuspendLayout()
        Me.PnlMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToOrderColumns = True
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.AoiData, Me.IsValid, Me.SourceName, Me.Description, Me.DataLayerSource})
        Me.DataGridView1.Location = New System.Drawing.Point(2, 4)
        Me.DataGridView1.MaximumSize = New System.Drawing.Size(829, 319)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(829, 319)
        Me.DataGridView1.TabIndex = 0
        '
        'AoiData
        '
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AoiData.DefaultCellStyle = DataGridViewCellStyle2
        Me.AoiData.HeaderText = "Aoi Data"
        Me.AoiData.Name = "AoiData"
        Me.AoiData.ReadOnly = True
        Me.AoiData.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.AoiData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.AoiData.Width = 90
        '
        'IsValid
        '
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IsValid.DefaultCellStyle = DataGridViewCellStyle3
        Me.IsValid.HeaderText = "Valid ?"
        Me.IsValid.Name = "IsValid"
        Me.IsValid.ReadOnly = True
        Me.IsValid.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.IsValid.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.IsValid.Width = 75
        '
        'SourceName
        '
        Me.SourceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black
        Me.SourceName.DefaultCellStyle = DataGridViewCellStyle4
        Me.SourceName.HeaderText = "Name"
        Me.SourceName.Name = "SourceName"
        Me.SourceName.ReadOnly = True
        Me.SourceName.Width = 74
        '
        'Description
        '
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Description.DefaultCellStyle = DataGridViewCellStyle5
        Me.Description.HeaderText = "Description"
        Me.Description.Name = "Description"
        Me.Description.ReadOnly = True
        Me.Description.Width = 205
        '
        'DataLayerSource
        '
        Me.DataLayerSource.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataLayerSource.DefaultCellStyle = DataGridViewCellStyle6
        Me.DataLayerSource.HeaderText = "Data Source"
        Me.DataLayerSource.Name = "DataLayerSource"
        Me.DataLayerSource.ReadOnly = True
        Me.DataLayerSource.Width = 119
        '
        'BtnAdd
        '
        Me.BtnAdd.Enabled = False
        Me.BtnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAdd.Location = New System.Drawing.Point(6, 329)
        Me.BtnAdd.Name = "BtnAdd"
        Me.BtnAdd.Size = New System.Drawing.Size(75, 25)
        Me.BtnAdd.TabIndex = 5
        Me.BtnAdd.Text = "Add"
        Me.BtnAdd.UseVisualStyleBackColor = True
        Me.BtnAdd.Visible = False
        '
        'BtnEdit
        '
        Me.BtnEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnEdit.Location = New System.Drawing.Point(92, 329)
        Me.BtnEdit.Name = "BtnEdit"
        Me.BtnEdit.Size = New System.Drawing.Size(75, 25)
        Me.BtnEdit.TabIndex = 6
        Me.BtnEdit.Text = "Edit"
        Me.BtnEdit.UseVisualStyleBackColor = True
        '
        'BtnDelete
        '
        Me.BtnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDelete.Location = New System.Drawing.Point(177, 329)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(75, 25)
        Me.BtnDelete.TabIndex = 7
        Me.BtnDelete.Text = "Delete"
        Me.BtnDelete.UseVisualStyleBackColor = True
        Me.BtnDelete.Visible = False
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(767, 330)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(64, 25)
        Me.BtnClose.TabIndex = 25
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'BtnClip
        '
        Me.BtnClip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClip.Location = New System.Drawing.Point(392, 329)
        Me.BtnClip.Name = "BtnClip"
        Me.BtnClip.Size = New System.Drawing.Size(194, 25)
        Me.BtnClip.TabIndex = 26
        Me.BtnClip.Text = "Clip selected layer(s) to AOI"
        Me.BtnClip.UseVisualStyleBackColor = True
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(183, 4)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(512, 22)
        Me.TxtAoiPath.TabIndex = 59
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(114, 7)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 16)
        Me.Label5.TabIndex = 58
        Me.Label5.Text = "AOI Path:"
        '
        'BtnSelectAoi
        '
        Me.BtnSelectAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectAoi.Location = New System.Drawing.Point(3, 1)
        Me.BtnSelectAoi.Name = "BtnSelectAoi"
        Me.BtnSelectAoi.Size = New System.Drawing.Size(105, 28)
        Me.BtnSelectAoi.TabIndex = 57
        Me.BtnSelectAoi.Text = "Select AOI"
        Me.BtnSelectAoi.UseVisualStyleBackColor = True
        '
        'PnlAoi
        '
        Me.PnlAoi.Controls.Add(Me.TxtAoiPath)
        Me.PnlAoi.Controls.Add(Me.BtnSelectAoi)
        Me.PnlAoi.Controls.Add(Me.Label5)
        Me.PnlAoi.Location = New System.Drawing.Point(2, 2)
        Me.PnlAoi.Name = "PnlAoi"
        Me.PnlAoi.Size = New System.Drawing.Size(701, 30)
        Me.PnlAoi.TabIndex = 60
        Me.PnlAoi.Visible = False
        '
        'PnlMain
        '
        Me.PnlMain.Controls.Add(Me.BtnDefaultSettings)
        Me.PnlMain.Controls.Add(Me.BtnClip)
        Me.PnlMain.Controls.Add(Me.BtnAdd)
        Me.PnlMain.Controls.Add(Me.DataGridView1)
        Me.PnlMain.Controls.Add(Me.BtnEdit)
        Me.PnlMain.Controls.Add(Me.BtnClose)
        Me.PnlMain.Controls.Add(Me.BtnDelete)
        Me.PnlMain.Location = New System.Drawing.Point(2, 36)
        Me.PnlMain.Name = "PnlMain"
        Me.PnlMain.Size = New System.Drawing.Size(834, 360)
        Me.PnlMain.TabIndex = 61
        '
        'BtnDefaultSettings
        '
        Me.BtnDefaultSettings.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDefaultSettings.Location = New System.Drawing.Point(261, 329)
        Me.BtnDefaultSettings.Name = "BtnDefaultSettings"
        Me.BtnDefaultSettings.Size = New System.Drawing.Size(123, 25)
        Me.BtnDefaultSettings.TabIndex = 27
        Me.BtnDefaultSettings.Text = "Default Settings"
        Me.BtnDefaultSettings.UseVisualStyleBackColor = True
        '
        'FrmDataManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(839, 397)
        Me.Controls.Add(Me.PnlMain)
        Me.Controls.Add(Me.PnlAoi)
        Me.Name = "FrmDataManager"
        Me.ShowIcon = False
        Me.Text = "Data Manager"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlAoi.ResumeLayout(False)
        Me.PnlAoi.PerformLayout()
        Me.PnlMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents BtnAdd As System.Windows.Forms.Button
    Friend WithEvents BtnEdit As System.Windows.Forms.Button
    Friend WithEvents BtnDelete As System.Windows.Forms.Button
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents BtnClip As System.Windows.Forms.Button
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents BtnSelectAoi As System.Windows.Forms.Button
    Friend WithEvents PnlAoi As System.Windows.Forms.Panel
    Friend WithEvents PnlMain As System.Windows.Forms.Panel
    Friend WithEvents AoiData As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents IsValid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SourceName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Description As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataLayerSource As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BtnDefaultSettings As System.Windows.Forms.Button
End Class
