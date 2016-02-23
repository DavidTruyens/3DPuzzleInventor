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
        Me.DXFButton = New System.Windows.Forms.Button()
        Me.DeleteButton = New System.Windows.Forms.Button()
        CType(Me.SliceThickness, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumberOfSlices, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Ydir
        '
        Me.Ydir.AutoSize = True
        Me.Ydir.Location = New System.Drawing.Point(168, 44)
        Me.Ydir.Name = "Ydir"
        Me.Ydir.Size = New System.Drawing.Size(43, 17)
        Me.Ydir.TabIndex = 14
        Me.Ydir.Text = "Ydir"
        Me.Ydir.UseVisualStyleBackColor = True
        '
        'Zdir
        '
        Me.Zdir.AutoSize = True
        Me.Zdir.Location = New System.Drawing.Point(168, 67)
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
        Me.Xdir.Location = New System.Drawing.Point(168, 21)
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
        Me.SliceThickness.Location = New System.Drawing.Point(29, 118)
        Me.SliceThickness.Name = "SliceThickness"
        Me.SliceThickness.Size = New System.Drawing.Size(111, 20)
        Me.SliceThickness.TabIndex = 11
        Me.SliceThickness.Value = New Decimal(New Integer() {4, 0, 0, 65536})
        Me.SliceThickness.Visible = False
        '
        'NumberOfSlices
        '
        Me.NumberOfSlices.Location = New System.Drawing.Point(29, 68)
        Me.NumberOfSlices.Name = "NumberOfSlices"
        Me.NumberOfSlices.Size = New System.Drawing.Size(111, 20)
        Me.NumberOfSlices.TabIndex = 10
        Me.NumberOfSlices.Value = New Decimal(New Integer() {8, 0, 0, 0})
        '
        'GetBodyButton
        '
        Me.GetBodyButton.Location = New System.Drawing.Point(29, 21)
        Me.GetBodyButton.Name = "GetBodyButton"
        Me.GetBodyButton.Size = New System.Drawing.Size(111, 23)
        Me.GetBodyButton.TabIndex = 15
        Me.GetBodyButton.Text = "Get body"
        Me.GetBodyButton.UseVisualStyleBackColor = True
        '
        'DXFButton
        '
        Me.DXFButton.Enabled = False
        Me.DXFButton.Location = New System.Drawing.Point(29, 44)
        Me.DXFButton.Name = "DXFButton"
        Me.DXFButton.Size = New System.Drawing.Size(111, 23)
        Me.DXFButton.TabIndex = 16
        Me.DXFButton.Text = "Create Assembly"
        Me.DXFButton.UseVisualStyleBackColor = True
        '
        'DeleteButton
        '
        Me.DeleteButton.Location = New System.Drawing.Point(29, 94)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(111, 23)
        Me.DeleteButton.TabIndex = 17
        Me.DeleteButton.Text = "Delete Puzzle"
        Me.DeleteButton.UseVisualStyleBackColor = True
        Me.DeleteButton.Visible = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(260, 152)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.DXFButton)
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
    Friend WithEvents DXFButton As Button
    Friend WithEvents DeleteButton As Button
End Class
