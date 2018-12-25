Imports System.IO

Public Class ToolBox
    Private CopyThis As Boolean

    Private Sub ToolBox_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim AutoDirString As String = vbNullString
        Dim ActiveIndex As Integer
        If Not ActiveAutomationDirectory = "" AndAlso Not ActiveAutomationDirectory = vbNullString Then
            If Mid(ActiveAutomationDirectory, Len(ActiveAutomationDirectory), 1) = "\" Then AutoDirString = Mid(ActiveAutomationDirectory, 1, Len(ActiveAutomationDirectory) - 1)
        End If
        AutoDirString = Path.GetFileName(AutoDirString)

        Me.ShowInTaskbar = False
        lbxCode.ForeColor = Color.Purple
        lbxScripts.ForeColor = Color.Chocolate
        Dim IntN As Integer
        For Each Dir As String In Directory.GetDirectories(MasterPath & "Automation")
            Dim listdir As String = Replace(Dir, MasterPath & "Automation\", "")
            cbxCannisters.Items.Add(listdir)
            If AutoDirString <> "" AndAlso AutoDirString <> vbNullString Then
                If InStr(listdir, AutoDirString) <> 0 Then ActiveIndex = IntN
            End If
            IntN += 1
        Next

        cbxCannisters.SelectedIndex = ActiveIndex

        'If AutoDirString <> vbNullString AndAlso AutoDirString <> "" Then
        '    cbxCannisters.SelectedIndex = 0
        '    'cbxCannisters.SelectedText = Path.GetFileName(AutoDirString)
        'End If


        lbxScripts.Height = Me.Height - lbxScripts.Top - 40
    End Sub

    Private Sub lbxCode_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles lbxCode.MouseDown
        tmrCopyThis.Enabled = False
        tmrCopyThis.Enabled = True
    End Sub

    Private Sub lbxCode_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lbxCode.SelectedIndexChanged
        If lbxCode.SelectedItem Is Nothing Then Exit Sub
        If tmrCopyThis.Enabled = False Then
            Dim str As String = lbxCode.SelectedItem.ToString
            Scripting_Engine.CustomCodeSelection(str)
        End If
        If Not lbxCode.Text = Nothing Then Clipboard.SetText(lbxCode.Text & "()")
    End Sub

    'Private Sub WriteAutoDirectory(ByVal ThisSTr As String)
    '    Dim dirFile As String = MasterPath & "Automation Directory.txt"
    '    If Not Directory.Exists(MasterPath) Then Directory.CreateDirectory(MasterPath)
    '    'If Not File.Exists(AddressFile) Then File.Create(AddressFile)
    '    Dim oFile As System.IO.FileStream = Nothing
    '    Dim oWrite As System.IO.StreamWriter = Nothing
    '    oFile = New System.IO.FileStream(dirFile, IO.FileMode.Create, IO.FileAccess.Write)
    '    oWrite = New System.IO.StreamWriter(oFile)
    '    oWrite.Write(ThisSTr)
    '    'oWrite.WriteLine(strButton)
    '    oWrite.Close()
    '    oFile.Close()
    '    oFile.Dispose()

    'End Sub

    Private Sub cbxCannisters_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbxCannisters.SelectedIndexChanged
        Dim str As String = cbxCannisters.SelectedItem.ToString
        ActiveAutomationDirectory = MasterPath & "Automation\" & str & "\"
        WriteSettings()
        Scripting_Engine.BuildCustomAutomationMenu()

        WriteSettings()
        'WriteAutoDirectory(str)

    End Sub

    Private Sub lbxScripts_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles lbxScripts.MouseDown
        tmrCopyThis.Enabled = False
        tmrCopyThis.Enabled = True
    End Sub

    Private Sub lbxScripts_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lbxScripts.SelectedIndexChanged
        If lbxScripts.SelectedItem Is Nothing Then Exit Sub
        If tmrCopyThis.Enabled = False Then
            Dim str As String = lbxScripts.SelectedItem.ToString
            Scripting_Engine.CustomAutomationExpound(str)
        End If
        If Not lbxScripts.Text = Nothing Then Clipboard.SetText(lbxScripts.Text & "()")
    End Sub

    Private Sub LoadToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LoadToolStripMenuItem.Click
        Scripting_Engine.LoadALLAutomation()
        MsgBox("Done")
    End Sub

    Private Sub ToolBox_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        lbxScripts.Height = Me.Height - lbxScripts.Top - 40
        lbxCode.Width = Me.Width - 40
        lbxScripts.Width = Me.Width - 40
        cbxCannisters.Width = Me.Width - 40
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles tmrCopyThis.Tick
        tmrCopyThis.Enabled = False
    End Sub

End Class