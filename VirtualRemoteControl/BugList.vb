Public Class BugList

    Private Sub BugList_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.ShowInTaskbar = False

        txtBugs.Text = _
    "There is no means for creating new Remote Controls or RC commands" & vbCrLf & _
    "Scripting Engine progress doesn't properly update/hightlight" & vbCrLf & _
    "Third-party players such as Windows Media Player, don't successfully render the video" & vbCrLf & _
    "FTP needs to be sFTP" & vbCrLf & _
    "Need to add fault tolerance to the FTP schema" & vbCrLf & _
    "The File Server schema needs to be more efficient. Incorporate a FileOfFile structure and a tagging convention for remote control files"
    End Sub

    Private Sub BugList_ResizeEnd(sender As Object, e As System.EventArgs) Handles Me.ResizeEnd
        txtBugs.Width = Me.Width - 30
        txtBugs.Height = Me.Height - 60
    End Sub

    Private Sub txtBugs_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtBugs.TextChanged

    End Sub
End Class