Public Class DirectionForm
    Private Sub Xbutton_Click(sender As Object, e As EventArgs) Handles Xbutton.Click
        Me.DialogResult = DialogResult.Yes
        Me.Close()

    End Sub

    Private Sub YButton_Click(sender As Object, e As EventArgs) Handles YButton.Click
        Me.DialogResult = DialogResult.No
        Me.Close()
    End Sub

    Private Sub ZButton_Click(sender As Object, e As EventArgs) Handles ZButton.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class