<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.SliceThickness = New System.Windows.Forms.NumericUpDown()
        Me.NumberOfSlices = New System.Windows.Forms.NumericUpDown()
        Me.GetBodyButton = New System.Windows.Forms.Button()
        Me.NESTButton = New System.Windows.Forms.Button()
        Me.GenerateDXFButton = New System.Windows.Forms.Button()
        Me.ToolCompensation = New System.Windows.Forms.CheckBox()
        Me.ToolReverse = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ToolDiamBox = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.XDirRadio = New System.Windows.Forms.RadioButton()
        Me.ZDirRadio = New System.Windows.Forms.RadioButton()
        Me.YDirRadio = New System.Windows.Forms.RadioButton()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.CutOutRev = New System.Windows.Forms.CheckBox()
        Me.TargetVolBox = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ScalingCheckBox = New System.Windows.Forms.CheckBox()
        CType(Me.SliceThickness, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumberOfSlices, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ToolDiamBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TargetVolBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SliceThickness
        '
        Me.SliceThickness.DecimalPlaces = 2
        Me.SliceThickness.Location = New System.Drawing.Point(298, 68)
        Me.SliceThickness.Name = "SliceThickness"
        Me.SliceThickness.Size = New System.Drawing.Size(128, 20)
        Me.SliceThickness.TabIndex = 11
        Me.SliceThickness.Value = New Decimal(New Integer() {36, 0, 0, 131072})
        '
        'NumberOfSlices
        '
        Me.NumberOfSlices.Location = New System.Drawing.Point(298, 29)
        Me.NumberOfSlices.Name = "NumberOfSlices"
        Me.NumberOfSlices.Size = New System.Drawing.Size(128, 20)
        Me.NumberOfSlices.TabIndex = 10
        Me.NumberOfSlices.Value = New Decimal(New Integer() {6, 0, 0, 0})
        '
        'GetBodyButton
        '
        Me.GetBodyButton.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.GetBodyButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.GetBodyButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.GetBodyButton.FlatAppearance.BorderSize = 0
        Me.GetBodyButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.GetBodyButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.GetBodyButton.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GetBodyButton.Location = New System.Drawing.Point(8, 8)
        Me.GetBodyButton.Name = "GetBodyButton"
        Me.GetBodyButton.Size = New System.Drawing.Size(182, 33)
        Me.GetBodyButton.TabIndex = 15
        Me.GetBodyButton.Text = "1. Create Puzzle"
        Me.GetBodyButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.GetBodyButton.UseVisualStyleBackColor = False
        '
        'NESTButton
        '
        Me.NESTButton.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.NESTButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.NESTButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.NESTButton.Enabled = False
        Me.NESTButton.FlatAppearance.BorderSize = 0
        Me.NESTButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.NESTButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.NESTButton.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NESTButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.NESTButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.NESTButton.Location = New System.Drawing.Point(8, 47)
        Me.NESTButton.Name = "NESTButton"
        Me.NESTButton.Size = New System.Drawing.Size(182, 32)
        Me.NESTButton.TabIndex = 16
        Me.NESTButton.Text = "2. Flatten Puzzle"
        Me.NESTButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.NESTButton.UseVisualStyleBackColor = False
        '
        'GenerateDXFButton
        '
        Me.GenerateDXFButton.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.GenerateDXFButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.GenerateDXFButton.Enabled = False
        Me.GenerateDXFButton.FlatAppearance.BorderSize = 0
        Me.GenerateDXFButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.GenerateDXFButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.GenerateDXFButton.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GenerateDXFButton.Location = New System.Drawing.Point(8, 85)
        Me.GenerateDXFButton.Name = "GenerateDXFButton"
        Me.GenerateDXFButton.Size = New System.Drawing.Size(182, 32)
        Me.GenerateDXFButton.TabIndex = 19
        Me.GenerateDXFButton.Text = "3. Export DXF"
        Me.GenerateDXFButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.GenerateDXFButton.UseVisualStyleBackColor = False
        '
        'ToolCompensation
        '
        Me.ToolCompensation.AutoSize = True
        Me.ToolCompensation.Checked = True
        Me.ToolCompensation.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolCompensation.Location = New System.Drawing.Point(444, 11)
        Me.ToolCompensation.Name = "ToolCompensation"
        Me.ToolCompensation.Size = New System.Drawing.Size(117, 17)
        Me.ToolCompensation.TabIndex = 20
        Me.ToolCompensation.Text = "Tool Compensation"
        Me.ToolCompensation.UseVisualStyleBackColor = True
        '
        'ToolReverse
        '
        Me.ToolReverse.AutoSize = True
        Me.ToolReverse.Location = New System.Drawing.Point(444, 34)
        Me.ToolReverse.Name = "ToolReverse"
        Me.ToolReverse.Size = New System.Drawing.Size(90, 17)
        Me.ToolReverse.TabIndex = 21
        Me.ToolReverse.Text = "Tool Reverse"
        Me.ToolReverse.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(441, 55)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 13)
        Me.Label1.TabIndex = 23
        Me.Label1.Text = "Tool Diameter (mm)"
        '
        'ToolDiamBox
        '
        Me.ToolDiamBox.DecimalPlaces = 2
        Me.ToolDiamBox.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.ToolDiamBox.Location = New System.Drawing.Point(444, 71)
        Me.ToolDiamBox.Name = "ToolDiamBox"
        Me.ToolDiamBox.Size = New System.Drawing.Size(120, 20)
        Me.ToolDiamBox.TabIndex = 24
        Me.ToolDiamBox.Value = New Decimal(New Integer() {2, 0, 0, 65536})
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(295, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(105, 13)
        Me.Label2.TabIndex = 25
        Me.Label2.Text = "PlateThickness (mm)"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(295, 13)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(122, 13)
        Me.Label3.TabIndex = 27
        Me.Label3.Text = "Number of PrimaryPlates"
        '
        'XDirRadio
        '
        Me.XDirRadio.AutoSize = True
        Me.XDirRadio.Checked = True
        Me.XDirRadio.Location = New System.Drawing.Point(204, 32)
        Me.XDirRadio.Name = "XDirRadio"
        Me.XDirRadio.Size = New System.Drawing.Size(32, 17)
        Me.XDirRadio.TabIndex = 29
        Me.XDirRadio.TabStop = True
        Me.XDirRadio.Text = "X"
        Me.XDirRadio.UseVisualStyleBackColor = True
        '
        'ZDirRadio
        '
        Me.ZDirRadio.AutoSize = True
        Me.ZDirRadio.Location = New System.Drawing.Point(204, 78)
        Me.ZDirRadio.Name = "ZDirRadio"
        Me.ZDirRadio.Size = New System.Drawing.Size(32, 17)
        Me.ZDirRadio.TabIndex = 30
        Me.ZDirRadio.Text = "Z"
        Me.ZDirRadio.UseVisualStyleBackColor = True
        '
        'YDirRadio
        '
        Me.YDirRadio.AutoSize = True
        Me.YDirRadio.Location = New System.Drawing.Point(204, 55)
        Me.YDirRadio.Name = "YDirRadio"
        Me.YDirRadio.Size = New System.Drawing.Size(32, 17)
        Me.YDirRadio.TabIndex = 31
        Me.YDirRadio.Text = "Y"
        Me.YDirRadio.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(196, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(92, 13)
        Me.Label4.TabIndex = 32
        Me.Label4.Text = "Opening Direction"
        '
        'CutOutRev
        '
        Me.CutOutRev.AutoSize = True
        Me.CutOutRev.Location = New System.Drawing.Point(298, 92)
        Me.CutOutRev.Name = "CutOutRev"
        Me.CutOutRev.Size = New System.Drawing.Size(100, 17)
        Me.CutOutRev.TabIndex = 33
        Me.CutOutRev.Text = "Cutout Reverse"
        Me.CutOutRev.UseVisualStyleBackColor = True
        '
        'TargetVolBox
        '
        Me.TargetVolBox.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.TargetVolBox.Location = New System.Drawing.Point(444, 110)
        Me.TargetVolBox.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.TargetVolBox.Name = "TargetVolBox"
        Me.TargetVolBox.Size = New System.Drawing.Size(120, 20)
        Me.TargetVolBox.TabIndex = 35
        Me.TargetVolBox.Value = New Decimal(New Integer() {3000, 0, 0, 0})
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(441, 94)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(99, 13)
        Me.Label5.TabIndex = 34
        Me.Label5.Text = "TargetVolume (cm³)"
        '
        'ScalingCheckBox
        '
        Me.ScalingCheckBox.AutoSize = True
        Me.ScalingCheckBox.Checked = True
        Me.ScalingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ScalingCheckBox.Location = New System.Drawing.Point(299, 111)
        Me.ScalingCheckBox.Name = "ScalingCheckBox"
        Me.ScalingCheckBox.Size = New System.Drawing.Size(99, 17)
        Me.ScalingCheckBox.TabIndex = 36
        Me.ScalingCheckBox.Text = "Volume Scaling"
        Me.ScalingCheckBox.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ClientSize = New System.Drawing.Size(569, 143)
        Me.Controls.Add(Me.ScalingCheckBox)
        Me.Controls.Add(Me.TargetVolBox)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.CutOutRev)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.YDirRadio)
        Me.Controls.Add(Me.ZDirRadio)
        Me.Controls.Add(Me.XDirRadio)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ToolDiamBox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ToolReverse)
        Me.Controls.Add(Me.ToolCompensation)
        Me.Controls.Add(Me.GenerateDXFButton)
        Me.Controls.Add(Me.NESTButton)
        Me.Controls.Add(Me.GetBodyButton)
        Me.Controls.Add(Me.SliceThickness)
        Me.Controls.Add(Me.NumberOfSlices)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(1000, 500)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Puzzle Generator"
        CType(Me.SliceThickness, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumberOfSlices, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ToolDiamBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TargetVolBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SliceThickness As NumericUpDown
    Friend WithEvents NumberOfSlices As NumericUpDown
    Friend WithEvents GetBodyButton As Button
    Friend WithEvents NESTButton As Button
    Friend WithEvents GenerateDXFButton As Button
    Friend WithEvents ToolCompensation As CheckBox
    Friend WithEvents ToolReverse As CheckBox
    Friend WithEvents Label1 As Label
    Friend WithEvents ToolDiamBox As NumericUpDown
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents XDirRadio As RadioButton
    Friend WithEvents ZDirRadio As RadioButton
    Friend WithEvents YDirRadio As RadioButton
    Friend WithEvents Label4 As Label
    Friend WithEvents CutOutRev As CheckBox
    Friend WithEvents TargetVolBox As NumericUpDown
    Friend WithEvents Label5 As Label
    Friend WithEvents ScalingCheckBox As CheckBox
End Class
