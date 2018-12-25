Public Class frmPassword

    Private Sub frmPassword_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If txtPassword.Text = "" Then End

        TV.AMC.MediaUsername = "root"
        TV.AMC.MediaPassword = txtPassword.Text
    End Sub

    Private Sub btnOK_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
        done()
    End Sub

    Private Sub txtPassword_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtPassword.KeyDown
        If e.KeyValue = 13 Then done()
    End Sub

    Private Sub done()
        If txtPassword.Text = "" Then
            MsgBox("No Password Provided" & vbCrLf & "MediaLab will exit")
            End
        End If
        TV.AMC.MediaUsername = "root"
        TV.AMC.MediaPassword = txtPassword.Text
        Me.Close()
    End Sub

End Class