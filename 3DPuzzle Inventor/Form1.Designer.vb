<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Ydir = New System.Windows.Forms.RadioButton()
        Me.Zdir = New System.Windows.Forms.RadioButton()
        Me.Xdir = New System.Windows.Forms.RadioButton()
        Me.SliceThickness = New System.Windows.Forms.NumericUpDown()
        Me.NumberOfSlices = New System.Windows.Forms.NumericUpDown()
        Me.GetBodyButton = New System.Windows.Forms.Button()
        Me.NESTButton = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.GenerateDXFButton = New System.Windows.Forms.Button()
        CType(Me.SliceThickness, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumberOfSlices, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Ydir
        '
        Me.Ydir.AutoSize = True
        Me.Ydir.Location = New System.Drawing.Point(126, 32)
        Me.Ydir.Name = "Ydir"
        Me.Ydir.Size = New System.Drawing.Size(43, 17)
        Me.Ydir.TabIndex = 14
        Me.Ydir.Text = "Ydir"
        Me.Ydir.UseVisualStyleBackColor = True
        '
        'Zdir
        '
        Me.Zdir.AutoSize = True
        Me.Zdir.Location = New System.Drawing.Point(126, 55)
        Me.Zdir.Name = "Zdir"
        Me.Zdir.Size = New System.Drawing.Size(43, 17)
        Me.Zdir.TabIndex = 13
        Me.Zdir.Text = "Zdir"
        Me.Zdir.UseVisualStyleBackColor = True
        '
        'Xdir
        '
        Me.Xdir.AutoSize = True
        Me.Xdir.Checked = True
        Me.Xdir.Location = New System.Drawing.Point(126, 9)
        Me.Xdir.Name = "Xdir"
        Me.Xdir.Size = New System.Drawing.Size(43, 17)
        Me.Xdir.TabIndex = 12
        Me.Xdir.TabStop = True
        Me.Xdir.Text = "Xdir"
        Me.Xdir.UseVisualStyleBackColor = True
        '
        'SliceThickness
        '
        Me.SliceThickness.DecimalPlaces = 2
        Me.SliceThickness.Location = New System.Drawing.Point(29, 125)
        Me.SliceThickness.Name = "SliceThickness"
        Me.SliceThickness.Size = New System.Drawing.Size(111, 20)
        Me.SliceThickness.TabIndex = 11
        Me.SliceThickness.Value = New Decimal(New Integer() {4, 0, 0, 65536})
        Me.SliceThickness.Visible = False
        '
        'NumberOfSlices
        '
        Me.NumberOfSlices.Location = New System.Drawing.Point(29, 99)
        Me.NumberOfSlices.Name = "NumberOfSlices"
        Me.NumberOfSlices.Size = New System.Drawing.Size(111, 20)
        Me.NumberOfSlices.TabIndex = 10
        Me.NumberOfSlices.Value = New Decimal(New Integer() {8, 0, 0, 0})
        '
        'GetBodyButton
        '
        Me.GetBodyButton.Location = New System.Drawing.Point(6, 5)
        Me.GetBodyButton.Name = "GetBodyButton"
        Me.GetBodyButton.Size = New System.Drawing.Size(111, 23)
        Me.GetBodyButton.TabIndex = 15
        Me.GetBodyButton.Text = "Get body"
        Me.GetBodyButton.UseVisualStyleBackColor = True
        '
        'NESTButton
        '
        Me.NESTButton.Enabled = False
        Me.NESTButton.Location = New System.Drawing.Point(6, 28)
        Me.NESTButton.Name = "NESTButton"
        Me.NESTButton.Size = New System.Drawing.Size(111, 23)
        Me.NESTButton.TabIndex = 16
        Me.NESTButton.Text = "Nest it DIY"
        Me.NESTButton.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(29, 151)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(111, 20)
        Me.Button1.TabIndex = 18
        Me.Button1.Text = "Get Surface ID"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'GenerateDXFButton
        '
        Me.GenerateDXFButton.Enabled = False
        Me.GenerateDXFButton.Location = New System.Drawing.Point(6, 51)
        Me.GenerateDXFButton.Name = "GenerateDXFButton"
        Me.GenerateDXFButton.Size = New System.Drawing.Size(111, 23)
        Me.GenerateDXFButton.TabIndex = 19
        Me.GenerateDXFButton.Text = "Generate DXF"
        Me.GenerateDXFButton.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(174, 81)
        Me.Controls.Add(Me.GenerateDXFButton)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.NESTButton)
        Me.Controls.Add(Me.GetBodyButton)
        Me.Controls.Add(Me.Ydir)
        Me.Controls.Add(Me.Zdir)
        Me.Controls.Add(Me.Xdir)
        Me.Controls.Add(Me.SliceThickness)
        Me.Controls.Add(Me.NumberOfSlices)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "Puzzle Generator"
        CType(Me.SliceThickness, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumberOfSlices, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Ydir As RadioButton
    Friend WithEvents Zdir As RadioButton
    Friend WithEvents Xdir As RadioButton
    Friend WithEvents SliceThickness As NumericUpDown
    Friend WithEvents NumberOfSlices As NumericUpDown
    Friend WithEvents GetBodyButton As Button
    Friend WithEvents NESTButton As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents GenerateDXFButton As Button
End Class
