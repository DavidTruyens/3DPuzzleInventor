Public Class Form2
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles NewXbutton.Click
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub NewYButton_Click(sender As Object, e As EventArgs) Handles NewYButton.Click
        Me.DialogResult = DialogResult.Yes
    End Sub

    Private Sub NewZButton_Click(sender As Object, e As EventArgs) Handles NewZButton.Click
        Me.DialogResult = DialogResult.Ignore
    End Sub
End Class