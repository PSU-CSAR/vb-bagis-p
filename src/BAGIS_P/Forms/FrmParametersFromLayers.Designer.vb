<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmParametersFromLayers
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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.BtnSelectAoi = New System.Windows.Forms.Button()
        Me.BtnCalculate = New System.Windows.Forms.Button()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.LblHruLayers = New System.Windows.Forms.Label()
        Me.LstHruLayers = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GrdCalcParameters = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BtnDeleteSelected = New System.Windows.Forms.Button()
        Me.BtnViewBagisParams = New System.Windows.Forms.Button()
        Me.LblToolTip = New System.Windows.Forms.Label()
        Me.GrpLayerType = New System.Windows.Forms.GroupBox()
        Me.RdoHru = New System.Windows.Forms.RadioButton()
        Me.RdoRaster = New System.Windows.Forms.RadioButton()
        Me.LstAoiRasterLayers = New System.Windows.Forms.ListBox()
        Me.LblRasterLayers = New System.Windows.Forms.Label()
        Me.GrpReclass = New System.Windows.Forms.GroupBox()
        Me.TxtStatus = New System.Windows.Forms.TextBox()
        Me.TxtParamName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GrdValues = New System.Windows.Forms.DataGridView()
        Me.LayerValues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ParameterValues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.BtnViewLog = New System.Windows.Forms.Button()
        Me.BtnView = New System.Windows.Forms.Button()
        CType(Me.GrdCalcParameters, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrpLayerType.SuspendLayout()
        Me.GrpReclass.SuspendLayout()
        CType(Me.GrdValues, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.TxtAoiPath.TabIndex = 65
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(115, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 16)
        Me.Label5.TabIndex = 64
        Me.Label5.Text = "AOI Path:"
        '
        'BtnSelectAoi
        '
        Me.BtnSelectAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectAoi.Location = New System.Drawing.Point(4, 6)
        Me.BtnSelectAoi.Name = "BtnSelectAoi"
        Me.BtnSelectAoi.Size = New System.Drawing.Size(105, 28)
        Me.BtnSelectAoi.TabIndex = 63
        Me.BtnSelectAoi.Text = "Select AOI"
        Me.BtnSelectAoi.UseVisualStyleBackColor = True
        '
        'BtnCalculate
        '
        Me.BtnCalculate.Enabled = False
        Me.BtnCalculate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCalculate.Location = New System.Drawing.Point(462, 527)
        Me.BtnCalculate.Name = "BtnCalculate"
        Me.BtnCalculate.Size = New System.Drawing.Size(175, 25)
        Me.BtnCalculate.TabIndex = 93
        Me.BtnCalculate.Text = "(Re)Calculate Parameters"
        Me.BtnCalculate.UseVisualStyleBackColor = True
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(642, 527)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(65, 25)
        Me.BtnClose.TabIndex = 92
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'LblHruLayers
        '
        Me.LblHruLayers.AutoSize = True
        Me.LblHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblHruLayers.Location = New System.Drawing.Point(1, 43)
        Me.LblHruLayers.Name = "LblHruLayers"
        Me.LblHruLayers.Size = New System.Drawing.Size(137, 16)
        Me.LblHruLayers.TabIndex = 95
        Me.LblHruLayers.Text = "HRU Layers in AOI"
        '
        'LstHruLayers
        '
        Me.LstHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstHruLayers.FormattingEnabled = True
        Me.LstHruLayers.ItemHeight = 16
        Me.LstHruLayers.Location = New System.Drawing.Point(4, 63)
        Me.LstHruLayers.Name = "LstHruLayers"
        Me.LstHruLayers.Size = New System.Drawing.Size(150, 116)
        Me.LstHruLayers.TabIndex = 94
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(1, 198)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(218, 16)
        Me.Label1.TabIndex = 96
        Me.Label1.Text = "Calculated parameters in HRU"
        '
        'GrdCalcParameters
        '
        Me.GrdCalcParameters.AllowUserToAddRows = False
        Me.GrdCalcParameters.AllowUserToDeleteRows = False
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.GrdCalcParameters.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.GrdCalcParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdCalcParameters.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2})
        Me.GrdCalcParameters.Location = New System.Drawing.Point(4, 218)
        Me.GrdCalcParameters.Margin = New System.Windows.Forms.Padding(4)
        Me.GrdCalcParameters.MultiSelect = False
        Me.GrdCalcParameters.Name = "GrdCalcParameters"
        Me.GrdCalcParameters.ReadOnly = True
        Me.GrdCalcParameters.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GrdCalcParameters.Size = New System.Drawing.Size(315, 163)
        Me.GrdCalcParameters.TabIndex = 97
        '
        'DataGridViewTextBoxColumn1
        '
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridViewTextBoxColumn1.DefaultCellStyle = DataGridViewCellStyle7
        Me.DataGridViewTextBoxColumn1.HeaderText = "Parameter"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 140
        '
        'DataGridViewTextBoxColumn2
        '
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle8
        Me.DataGridViewTextBoxColumn2.HeaderText = "Date Calculated"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 130
        '
        'BtnDeleteSelected
        '
        Me.BtnDeleteSelected.Enabled = False
        Me.BtnDeleteSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDeleteSelected.Location = New System.Drawing.Point(6, 388)
        Me.BtnDeleteSelected.Name = "BtnDeleteSelected"
        Me.BtnDeleteSelected.Size = New System.Drawing.Size(150, 25)
        Me.BtnDeleteSelected.TabIndex = 98
        Me.BtnDeleteSelected.Text = "Delete Selected"
        Me.BtnDeleteSelected.UseVisualStyleBackColor = True
        '
        'BtnViewBagisParams
        '
        Me.BtnViewBagisParams.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnViewBagisParams.Location = New System.Drawing.Point(3, 527)
        Me.BtnViewBagisParams.Name = "BtnViewBagisParams"
        Me.BtnViewBagisParams.Size = New System.Drawing.Size(135, 25)
        Me.BtnViewBagisParams.TabIndex = 136
        Me.BtnViewBagisParams.Text = "View parameter list"
        Me.BtnViewBagisParams.UseVisualStyleBackColor = True
        '
        'LblToolTip
        '
        Me.LblToolTip.AutoSize = True
        Me.LblToolTip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblToolTip.Location = New System.Drawing.Point(388, 42)
        Me.LblToolTip.Name = "LblToolTip"
        Me.LblToolTip.Size = New System.Drawing.Size(297, 16)
        Me.LblToolTip.TabIndex = 137
        Me.LblToolTip.Text = "Use a raster to set HRU parameter values"
        '
        'GrpLayerType
        '
        Me.GrpLayerType.Controls.Add(Me.RdoHru)
        Me.GrpLayerType.Controls.Add(Me.RdoRaster)
        Me.GrpLayerType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpLayerType.Location = New System.Drawing.Point(413, 64)
        Me.GrpLayerType.Name = "GrpLayerType"
        Me.GrpLayerType.Size = New System.Drawing.Size(86, 72)
        Me.GrpLayerType.TabIndex = 138
        Me.GrpLayerType.TabStop = False
        Me.GrpLayerType.Text = "Layer type"
        '
        'RdoHru
        '
        Me.RdoHru.AutoSize = True
        Me.RdoHru.Location = New System.Drawing.Point(10, 25)
        Me.RdoHru.Name = "RdoHru"
        Me.RdoHru.Size = New System.Drawing.Size(56, 20)
        Me.RdoHru.TabIndex = 3
        Me.RdoHru.TabStop = True
        Me.RdoHru.Text = "HRU"
        Me.RdoHru.UseVisualStyleBackColor = True
        '
        'RdoRaster
        '
        Me.RdoRaster.AutoSize = True
        Me.RdoRaster.Location = New System.Drawing.Point(10, 51)
        Me.RdoRaster.Name = "RdoRaster"
        Me.RdoRaster.Size = New System.Drawing.Size(66, 20)
        Me.RdoRaster.TabIndex = 0
        Me.RdoRaster.TabStop = True
        Me.RdoRaster.Text = "Raster"
        Me.RdoRaster.UseVisualStyleBackColor = True
        '
        'LstAoiRasterLayers
        '
        Me.LstAoiRasterLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstAoiRasterLayers.FormattingEnabled = True
        Me.LstAoiRasterLayers.HorizontalScrollbar = True
        Me.LstAoiRasterLayers.ItemHeight = 16
        Me.LstAoiRasterLayers.Location = New System.Drawing.Point(511, 83)
        Me.LstAoiRasterLayers.Name = "LstAoiRasterLayers"
        Me.LstAoiRasterLayers.Size = New System.Drawing.Size(146, 116)
        Me.LstAoiRasterLayers.TabIndex = 139
        '
        'LblRasterLayers
        '
        Me.LblRasterLayers.AutoSize = True
        Me.LblRasterLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRasterLayers.Location = New System.Drawing.Point(516, 64)
        Me.LblRasterLayers.Name = "LblRasterLayers"
        Me.LblRasterLayers.Size = New System.Drawing.Size(91, 16)
        Me.LblRasterLayers.TabIndex = 140
        Me.LblRasterLayers.Text = "Select layer"
        '
        'GrpReclass
        '
        Me.GrpReclass.Controls.Add(Me.TxtStatus)
        Me.GrpReclass.Controls.Add(Me.TxtParamName)
        Me.GrpReclass.Controls.Add(Me.Label2)
        Me.GrpReclass.Controls.Add(Me.GrdValues)
        Me.GrpReclass.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpReclass.Location = New System.Drawing.Point(345, 272)
        Me.GrpReclass.Name = "GrpReclass"
        Me.GrpReclass.Size = New System.Drawing.Size(364, 250)
        Me.GrpReclass.TabIndex = 141
        Me.GrpReclass.TabStop = False
        Me.GrpReclass.Text = "Reclass"
        '
        'TxtStatus
        '
        Me.TxtStatus.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtStatus.ForeColor = System.Drawing.Color.Red
        Me.TxtStatus.Location = New System.Drawing.Point(8, 47)
        Me.TxtStatus.Multiline = True
        Me.TxtStatus.Name = "TxtStatus"
        Me.TxtStatus.ReadOnly = True
        Me.TxtStatus.Size = New System.Drawing.Size(350, 20)
        Me.TxtStatus.TabIndex = 143
        '
        'TxtParamName
        '
        Me.TxtParamName.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.TxtParamName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtParamName.Location = New System.Drawing.Point(149, 15)
        Me.TxtParamName.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtParamName.MaxLength = 30
        Me.TxtParamName.Name = "TxtParamName"
        Me.TxtParamName.Size = New System.Drawing.Size(208, 22)
        Me.TxtParamName.TabIndex = 142
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(4, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(146, 16)
        Me.Label2.TabIndex = 142
        Me.Label2.Text = "Output param name:"
        '
        'GrdValues
        '
        Me.GrdValues.AllowUserToAddRows = False
        Me.GrdValues.AllowUserToDeleteRows = False
        Me.GrdValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdValues.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.LayerValues, Me.ParameterValues})
        Me.GrdValues.Location = New System.Drawing.Point(8, 73)
        Me.GrdValues.Name = "GrdValues"
        Me.GrdValues.Size = New System.Drawing.Size(350, 165)
        Me.GrdValues.TabIndex = 5
        '
        'LayerValues
        '
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LayerValues.DefaultCellStyle = DataGridViewCellStyle9
        Me.LayerValues.HeaderText = "Layer Values"
        Me.LayerValues.Name = "LayerValues"
        Me.LayerValues.ReadOnly = True
        Me.LayerValues.Width = 145
        '
        'ParameterValues
        '
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ParameterValues.DefaultCellStyle = DataGridViewCellStyle10
        Me.ParameterValues.HeaderText = "Parameter Values"
        Me.ParameterValues.Name = "ParameterValues"
        Me.ParameterValues.Width = 145
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.ForeColor = System.Drawing.Color.Black
        Me.TextBox1.Location = New System.Drawing.Point(353, 217)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(350, 50)
        Me.TextBox1.TabIndex = 144
        Me.TextBox1.Text = "Note: Single cell HRUs will be assigned the NoData value because the MAJORITY too" & _
            "l does not support single-cell evaluation"
        '
        'BtnViewLog
        '
        Me.BtnViewLog.Enabled = False
        Me.BtnViewLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnViewLog.Location = New System.Drawing.Point(169, 388)
        Me.BtnViewLog.Name = "BtnViewLog"
        Me.BtnViewLog.Size = New System.Drawing.Size(150, 25)
        Me.BtnViewLog.TabIndex = 145
        Me.BtnViewLog.Text = "View Log"
        Me.BtnViewLog.UseVisualStyleBackColor = True
        '
        'BtnView
        '
        Me.BtnView.Enabled = False
        Me.BtnView.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnView.Location = New System.Drawing.Point(6, 419)
        Me.BtnView.Name = "BtnView"
        Me.BtnView.Size = New System.Drawing.Size(170, 25)
        Me.BtnView.TabIndex = 146
        Me.BtnView.Text = "View Value(s) On Map"
        Me.BtnView.UseVisualStyleBackColor = True
        '
        'FrmParametersFromLayers
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(719, 554)
        Me.Controls.Add(Me.BtnView)
        Me.Controls.Add(Me.BtnViewLog)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.GrpReclass)
        Me.Controls.Add(Me.LstAoiRasterLayers)
        Me.Controls.Add(Me.LblRasterLayers)
        Me.Controls.Add(Me.GrpLayerType)
        Me.Controls.Add(Me.LblToolTip)
        Me.Controls.Add(Me.BtnViewBagisParams)
        Me.Controls.Add(Me.BtnDeleteSelected)
        Me.Controls.Add(Me.GrdCalcParameters)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LblHruLayers)
        Me.Controls.Add(Me.LstHruLayers)
        Me.Controls.Add(Me.BtnCalculate)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.BtnSelectAoi)
        Me.Name = "FrmParametersFromLayers"
        Me.ShowIcon = False
        Me.Text = "Parameters from layers"
        CType(Me.GrdCalcParameters, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrpLayerType.ResumeLayout(False)
        Me.GrpLayerType.PerformLayout()
        Me.GrpReclass.ResumeLayout(False)
        Me.GrpReclass.PerformLayout()
        CType(Me.GrdValues, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents BtnSelectAoi As System.Windows.Forms.Button
    Friend WithEvents BtnCalculate As System.Windows.Forms.Button
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents LblHruLayers As System.Windows.Forms.Label
    Friend WithEvents LstHruLayers As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GrdCalcParameters As System.Windows.Forms.DataGridView
    Friend WithEvents BtnDeleteSelected As System.Windows.Forms.Button
    Friend WithEvents BtnViewBagisParams As System.Windows.Forms.Button
    Friend WithEvents LblToolTip As System.Windows.Forms.Label
    Friend WithEvents GrpLayerType As System.Windows.Forms.GroupBox
    Friend WithEvents RdoHru As System.Windows.Forms.RadioButton
    Friend WithEvents RdoRaster As System.Windows.Forms.RadioButton
    Friend WithEvents LstAoiRasterLayers As System.Windows.Forms.ListBox
    Friend WithEvents LblRasterLayers As System.Windows.Forms.Label
    Friend WithEvents GrpReclass As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GrdValues As System.Windows.Forms.DataGridView
    Friend WithEvents TxtParamName As System.Windows.Forms.TextBox
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LayerValues As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ParameterValues As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TxtStatus As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents BtnViewLog As System.Windows.Forms.Button
    Friend WithEvents BtnView As System.Windows.Forms.Button
End Class
