Public Class Output

    Private Sub Output_ResizeEnd(sender As Object, e As System.EventArgs) Handles Me.ResizeEnd
        txtOutput.Width = Me.Width - 10
        txtOutput.Height = Me.Height - 60
    End Sub

    Private Sub Output_SizeChanged(sender As Object, e As System.EventArgs) Handles Me.SizeChanged
        txtOutput.Width = Me.Width - 10
        txtOutput.Height = Me.Height - 60
    End Sub

    Private Sub Output_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.ShowInTaskbar = False
    End Sub

    Private Sub ClearToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ClearToolStripMenuItem.Click
        txtOutput.Text = ""
    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As System.Object, e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub
End Class