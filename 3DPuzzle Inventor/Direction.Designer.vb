<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DirectionForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DirectionForm))
        Me.Xbutton = New System.Windows.Forms.Button()
        Me.YButton = New System.Windows.Forms.Button()
        Me.ZButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Xbutton
        '
        Me.Xbutton.BackColor = System.Drawing.Color.Transparent
        Me.Xbutton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Xbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Xbutton.Location = New System.Drawing.Point(2, 27)
        Me.Xbutton.Name = "Xbutton"
        Me.Xbutton.Size = New System.Drawing.Size(120, 113)
        Me.Xbutton.TabIndex = 0
        Me.Xbutton.UseVisualStyleBackColor = False
        '
        'YButton
        '
        Me.YButton.BackColor = System.Drawing.Color.Transparent
        Me.YButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.YButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.YButton.Location = New System.Drawing.Point(127, 27)
        Me.YButton.Name = "YButton"
        Me.YButton.Size = New System.Drawing.Size(113, 113)
        Me.YButton.TabIndex = 1
        Me.YButton.UseVisualStyleBackColor = False
        '
        'ZButton
        '
        Me.ZButton.BackColor = System.Drawing.Color.Transparent
        Me.ZButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ZButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ZButton.Location = New System.Drawing.Point(246, 26)
        Me.ZButton.Name = "ZButton"
        Me.ZButton.Size = New System.Drawing.Size(113, 113)
        Me.ZButton.TabIndex = 2
        Me.ZButton.UseVisualStyleBackColor = False
        '
        'DirectionForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ClientSize = New System.Drawing.Size(364, 146)
        Me.Controls.Add(Me.ZButton)
        Me.Controls.Add(Me.YButton)
        Me.Controls.Add(Me.Xbutton)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "DirectionForm"
        Me.Opacity = 0R
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Direction"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Xbutton As Button
    Friend WithEvents YButton As Button
    Friend WithEvents ZButton As Button
End Class
