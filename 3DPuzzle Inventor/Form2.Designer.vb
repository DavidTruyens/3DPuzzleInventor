<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form2))
        Me.NewXbutton = New System.Windows.Forms.Button()
        Me.NewYButton = New System.Windows.Forms.Button()
        Me.NewZButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'NewXbutton
        '
        Me.NewXbutton.BackColor = System.Drawing.Color.Transparent
        Me.NewXbutton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.NewXbutton.FlatAppearance.BorderSize = 0
        Me.NewXbutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.NewXbutton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.NewXbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.NewXbutton.ForeColor = System.Drawing.Color.Transparent
        Me.NewXbutton.Location = New System.Drawing.Point(8, 23)
        Me.NewXbutton.Name = "NewXbutton"
        Me.NewXbutton.Size = New System.Drawing.Size(81, 87)
        Me.NewXbutton.TabIndex = 0
        Me.NewXbutton.UseVisualStyleBackColor = False
        '
        'NewYButton
        '
        Me.NewYButton.BackColor = System.Drawing.Color.Transparent
        Me.NewYButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.NewYButton.FlatAppearance.BorderSize = 0
        Me.NewYButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.NewYButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.NewYButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.NewYButton.ForeColor = System.Drawing.Color.Transparent
        Me.NewYButton.Location = New System.Drawing.Point(102, 21)
        Me.NewYButton.Name = "NewYButton"
        Me.NewYButton.Size = New System.Drawing.Size(81, 87)
        Me.NewYButton.TabIndex = 1
        Me.NewYButton.UseVisualStyleBackColor = False
        '
        'NewZButton
        '
        Me.NewZButton.BackColor = System.Drawing.Color.Transparent
        Me.NewZButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.NewZButton.FlatAppearance.BorderSize = 0
        Me.NewZButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.NewZButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.NewZButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.NewZButton.ForeColor = System.Drawing.Color.Transparent
        Me.NewZButton.Location = New System.Drawing.Point(195, 21)
        Me.NewZButton.Name = "NewZButton"
        Me.NewZButton.Size = New System.Drawing.Size(81, 87)
        Me.NewZButton.TabIndex = 2
        Me.NewZButton.UseVisualStyleBackColor = False
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ClientSize = New System.Drawing.Size(284, 113)
        Me.Controls.Add(Me.NewZButton)
        Me.Controls.Add(Me.NewYButton)
        Me.Controls.Add(Me.NewXbutton)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Form2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form2"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents NewXbutton As Button
    Friend WithEvents NewYButton As Button
    Friend WithEvents NewZButton As Button
End Class
