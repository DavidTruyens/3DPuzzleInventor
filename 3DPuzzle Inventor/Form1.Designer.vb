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
        CType(Me.SliceThickness, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumberOfSlices, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SliceThickness
        '
        Me.SliceThickness.DecimalPlaces = 2
        Me.SliceThickness.Location = New System.Drawing.Point(201, 498)
        Me.SliceThickness.Name = "SliceThickness"
        Me.SliceThickness.Size = New System.Drawing.Size(128, 20)
        Me.SliceThickness.TabIndex = 11
        Me.SliceThickness.Value = New Decimal(New Integer() {4, 0, 0, 65536})
        Me.SliceThickness.Visible = False
        '
        'NumberOfSlices
        '
        Me.NumberOfSlices.Location = New System.Drawing.Point(201, 472)
        Me.NumberOfSlices.Name = "NumberOfSlices"
        Me.NumberOfSlices.Size = New System.Drawing.Size(128, 20)
        Me.NumberOfSlices.TabIndex = 10
        Me.NumberOfSlices.Value = New Decimal(New Integer() {6, 0, 0, 0})
        '
        'GetBodyButton
        '
        Me.GetBodyButton.BackColor = System.Drawing.Color.Transparent
        Me.GetBodyButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.GetBodyButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.GetBodyButton.FlatAppearance.BorderSize = 0
        Me.GetBodyButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.GetBodyButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.GetBodyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GetBodyButton.Location = New System.Drawing.Point(0, 35)
        Me.GetBodyButton.Name = "GetBodyButton"
        Me.GetBodyButton.Size = New System.Drawing.Size(358, 62)
        Me.GetBodyButton.TabIndex = 15
        Me.GetBodyButton.UseVisualStyleBackColor = False
        '
        'NESTButton
        '
        Me.NESTButton.BackColor = System.Drawing.Color.Transparent
        Me.NESTButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.NESTButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.NESTButton.Enabled = False
        Me.NESTButton.FlatAppearance.BorderSize = 0
        Me.NESTButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.NESTButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.NESTButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.NESTButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.NESTButton.Location = New System.Drawing.Point(33, 114)
        Me.NESTButton.Name = "NESTButton"
        Me.NESTButton.Size = New System.Drawing.Size(361, 60)
        Me.NESTButton.TabIndex = 16
        Me.NESTButton.UseVisualStyleBackColor = False
        '
        'GenerateDXFButton
        '
        Me.GenerateDXFButton.BackColor = System.Drawing.Color.Transparent
        Me.GenerateDXFButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.GenerateDXFButton.Enabled = False
        Me.GenerateDXFButton.FlatAppearance.BorderSize = 0
        Me.GenerateDXFButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.GenerateDXFButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.GenerateDXFButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GenerateDXFButton.Location = New System.Drawing.Point(0, 193)
        Me.GenerateDXFButton.Name = "GenerateDXFButton"
        Me.GenerateDXFButton.Size = New System.Drawing.Size(358, 60)
        Me.GenerateDXFButton.TabIndex = 19
        Me.GenerateDXFButton.UseVisualStyleBackColor = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ClientSize = New System.Drawing.Size(393, 354)
        Me.Controls.Add(Me.GenerateDXFButton)
        Me.Controls.Add(Me.NESTButton)
        Me.Controls.Add(Me.GetBodyButton)
        Me.Controls.Add(Me.SliceThickness)
        Me.Controls.Add(Me.NumberOfSlices)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(1000, 500)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Puzzle Generator"
        CType(Me.SliceThickness, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumberOfSlices, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SliceThickness As NumericUpDown
    Friend WithEvents NumberOfSlices As NumericUpDown
    Friend WithEvents GetBodyButton As Button
    Friend WithEvents NESTButton As Button
    Friend WithEvents GenerateDXFButton As Button
End Class
