Imports System.IO

Public Class SCBuilder
    Private BoxVisible As Boolean
    Private ActiveDraw As Boolean

    Private Sub pboxScene_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pboxScene.MouseDown
        If Not BoxVisible Then
            Coord = New Rectangle(e.X, e.Y, 30, 30)
            DrawBox()
            BoxVisible = True
        Else
            ActiveDraw = True
            Dim ThisPoint As New Point(e.X, e.Y)
            Dim ClosestCorner As String = GetClosestRectBounds(ThisPoint)
            ReDrawBox(ClosestCorner, ThisPoint)
        End If
    End Sub
    Private Sub ReDrawBox(ByVal ThisCorner As String, ByVal PPoint As Point)
        Dim DeltaHgt As Integer
        Dim DeltaWid As Integer
        Select Case ThisCorner
            Case "UL"
                DeltaHgt = Coord.Y - PPoint.Y
                DeltaWid = Coord.X - PPoint.X
                Coord.Y = PPoint.Y
                Coord.X = PPoint.X
                Coord.Width += DeltaWid
                Coord.Height += DeltaHgt
            Case "UR"
                DeltaHgt = Coord.Y - PPoint.Y
                DeltaWid = Coord.X + Coord.Width - PPoint.X
                Coord.Y = PPoint.Y
                Coord.Height += DeltaHgt
                Coord.Width -= DeltaWid
            Case "LR"
                DeltaHgt = Coord.Y + Coord.Height - PPoint.Y
                DeltaWid = Coord.X + Coord.Width - PPoint.X
                Coord.Height -= DeltaHgt
                Coord.Width -= DeltaWid
            Case "LL"
                DeltaHgt = Coord.Y + Coord.Height - PPoint.Y
                DeltaWid = Coord.X - PPoint.X
                Coord.X = PPoint.X
                Coord.Height -= DeltaHgt
                Coord.Width += DeltaWid
        End Select
        DrawBox()
    End Sub

    Private Sub pboxScene_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pboxScene.MouseMove
        If Not ActiveDraw Then Exit Sub
        Dim ThisPoint As New Point(e.X, e.Y)
        Dim ClosestCorner As String = GetClosestRectBounds(ThisPoint)
        ReDrawBox(ClosestCorner, ThisPoint)
    End Sub

    Private Function GetClosestRectBounds(ByVal PPoint As Point) As String
        '1= upperleft, 2= upperright, 3= lowerright, 4= lowerleft... it's clockwise
        Dim UpperLeft As Point
        Dim UpperRight As Point
        Dim LowerRight As Point
        Dim LowerLeft As Point

        UpperLeft = New Point(Coord.X, Coord.Y)
        UpperRight = New Point(Coord.X + Coord.Width, Coord.Y)
        LowerRight = New Point(Coord.X + Coord.Width, Coord.Y + Coord.Height)
        LowerLeft = New Point(Coord.X, Coord.Y + Coord.Height)

        Dim ULTravel As Double = DistanceBetweenPoints(PPoint, UpperLeft)
        Dim URTravel As Double = DistanceBetweenPoints(PPoint, UpperRight)
        Dim LRTravel As Double = DistanceBetweenPoints(PPoint, LowerRight)
        Dim LLTRavel As Double = DistanceBetweenPoints(PPoint, LowerLeft)

        Dim ClosestBound As Double = Math.Min(ULTravel, URTravel)
        ClosestBound = Math.Min(ClosestBound, LRTravel)
        ClosestBound = Math.Min(ClosestBound, LLTRavel)

        Select Case ClosestBound
            Case ULTravel
                Return "UL"
            Case URTravel
                Return "UR"
            Case LRTravel
                Return "LR"
            Case LLTRavel
                Return "LL"
        End Select
        Return "UL"
    End Function

    Private Function DistanceBetweenPoints(ByVal Point1 As Point, ByVal Point2 As Point) As Double
        Dim DiffPoints As Double
        Dim DiffX As Integer = Point2.X - Point1.X
        Dim DiffY As Integer = Point2.Y - Point1.Y
        DiffPoints = Math.Sqrt(DiffX * DiffX + DiffY * DiffY)
        Return DiffPoints
    End Function

    Private Sub pboxScene_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pboxScene.MouseUp
        ActiveDraw = False
        If rbOCR.Checked Then
            Dim TempBmp As New Bitmap(MasterPath & "tempbmp.bmp")
            Dim SCPoint As New Point(Coord.X, Coord.Y)
            Dim SCSize As New Size(Coord.Width, Coord.Height)
            Crop(TempBmp, SCPoint, SCSize)
            Dim ThisOCR As OCRResult = OCR.ReadText(TempBmp)
            lblSCConfidence.Text = ThisOCR.Confidence
            txtSCText.Text = ThisOCR.Text
            TempBmp.Dispose()
        End If
    End Sub


    Private Sub pboxScene_SizeChanged(sender As Object, e As System.EventArgs) Handles pboxScene.SizeChanged
        ResizePbox()
    End Sub

    Private Sub ResizePbox()
        Me.Width = pboxScene.Width + 20
        Me.Height = pboxScene.Height + 140
        btnSave.Top = Me.Height - 105
        txtSCName.Top = btnSave.Top
        lblSCName.Top = btnSave.Top + 5
        txtSCName.Left = lblSCName.Left + lblSCName.Width
        txtSCName.Width = pboxScene.Width - txtSCName.Left
        rbImage.Top = btnSave.Top
        rbOCR.Top = rbImage.Top + 23
        lblSCText.Top = rbOCR.Top
        txtSCText.Top = rbOCR.Top
        txtSCText.Left = txtSCName.Left
        txtSCText.Width = txtSCName.Width
        rbMotion.Top = rbOCR.Top + 23
        lblSCConfidence.Top = rbMotion.Top
    End Sub
    Private Sub DrawBox()
        Dim p As New System.Drawing.Pen(Color.Red, 1)
        p.DashPattern = New Single() {4, 3} '{4.0F, 2.0F, 1.0F, 3.0F}
        Dim g As System.Drawing.Graphics
        pboxScene.Refresh()
        g = pboxScene.CreateGraphics
        g.DrawRectangle(p, Coord.X, Coord.Y, Coord.Width, Coord.Height)
    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        SaveSC()
    End Sub

    Private Sub SaveSC()
        'check for illegal characters in name
        Dim SyntaxCheck As String = IllegalCharacters(txtSCName.Text)
        If SyntaxCheck = "illegal" Then Exit Sub
        txtSCName.Text = SyntaxCheck
        If txtSCName.Text = vbNullString Then
            MsgBox("You need to name this SceneCheck")
            Exit Sub
        End If
        'check for coordinates
        If Coord = Nothing Then
            MsgBox("You haven't selected coordinates")
            Exit Sub
        End If

        Dim SCName As String = txtSCName.Text
        Dim SCPath As String = MasterPath & "SceneChecks\" & SCName & "\"
        If Not Directory.Exists(MasterPath & "SceneChecks\") Then Directory.CreateDirectory(MasterPath & "SceneChecks\")
        If Not Directory.Exists(SCPath) Then
            Directory.CreateDirectory(SCPath)
        Else
            Dim Prompt As String = "Overwrite " & SCName & "?"
            Dim Rez As Integer = MsgBox(Prompt, MsgBoxStyle.YesNo)
            If Rez = MsgBoxResult.No Then
                MsgBox("SceneCheck not saved")
                Exit Sub
            End If
        End If

        'Save Bitmap
        Dim TempBmp As Bitmap
        Dim SCBmpFile As String = SCPath & "SCBmp.bmp"
        TempBmp = New Bitmap(MasterPath & "tempbmp.bmp")
        Dim SCPoint As Point
        Dim SCSize As Size
        SCPoint = New Point(Coord.X, Coord.Y)
        SCSize = New Size(Coord.Width, Coord.Height)
        Crop(TempBmp, SCPoint, SCSize)
        TempBmp.Save(SCBmpFile)

        'Save Coordinates and specs
        Dim SCFile As String = SCPath & "SCFile.txt"
        Dim SCText As String = "Top: " & SCPoint.Y
        Dim SCType As String = "Image"
        If rbMotion.Checked Then SCType = "Motion"
        If rbOCR.Checked Then SCType = "Text"
        SCText &= vbCrLf & "Left: " & SCPoint.X
        SCText &= vbCrLf & "Width: " & SCSize.Width
        SCText &= vbCrLf & "Height: " & SCSize.Height
        SCText &= vbCrLf & "Orig Resolution: " & pboxScene.Width.ToString & "x" & pboxScene.Height.ToString
        SCText &= vbCrLf & "Type: " & SCType
        SCText &= vbCrLf & "Text: " & Trim(txtSCText.Text)
        Dim oFile As System.IO.FileStream = Nothing
        Dim oWrite As System.IO.StreamWriter = Nothing
        oFile = New System.IO.FileStream(SCFile, IO.FileMode.Create, IO.FileAccess.Write)
        oWrite = New System.IO.StreamWriter(oFile)
        oWrite.Write(SCText)
        'oWrite.WriteLine(strButton)
        oWrite.Close()
        oFile.Close()
        oFile.Dispose()
    End Sub

    Private Sub MotionDetectionToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles MotionDetectionToolStripMenuItem.Click
        rbMotion.Checked = True
    End Sub

    Private Sub ImageDetectionToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ImageDetectionToolStripMenuItem.Click
        rbImage.Checked = True
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        SaveSC()
    End Sub

    Private Sub SCBuilder_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.ShowInTaskbar = False
    End Sub

    Private Sub rbMotion_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbMotion.CheckedChanged
        If rbMotion.Checked Then
            lblSCText.ForeColor = Color.Gray
            lblSCConfidence.ForeColor = Color.Gray
            txtSCText.Enabled = False
        End If
    End Sub

    Private Sub TextReconitionToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TextReconitionToolStripMenuItem.Click
        rbOCR.Checked = True
    End Sub

    Private Sub rbImage_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbImage.CheckedChanged
        If rbImage.Checked Then
            lblSCText.ForeColor = Color.Gray
            lblSCConfidence.ForeColor = Color.Gray
            txtSCText.Enabled = False
        End If
    End Sub

    Private Sub rbOCR_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbOCR.CheckedChanged
        If rbOCR.Checked Then
            lblSCText.ForeColor = Color.Black
            lblSCConfidence.ForeColor = Color.Black
            txtSCText.Enabled = True
        End If
    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As System.Object, e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Dim ThisFile As String
        Dim PixDirectory As String = "C:\Users\labtown\Desktop\OCR Samples\"
        If Not Directory.Exists(PixDirectory) Then Directory.CreateDirectory(PixDirectory)
        Dim OpenFil As New OpenFileDialog
        OpenFil.InitialDirectory = PixDirectory
        OpenFil.FilterIndex = 6
        OpenFil.Filter = "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff"
        OpenFil.RestoreDirectory = False
        OpenFil.ShowDialog()
        If OpenFil.FileName <> vbNullString Then ThisFile = OpenFil.FileName
    End Sub

    Private Sub LoadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadToolStripMenuItem.Click
        If Not Directory.Exists(SCDirectory) Then
            MsgBox("No Saved OCR, Scene, or Motion Checks")
            Exit Sub
        End If
        Dim DirectoryList As New ArrayList
        Dim Dirs() As String = Directory.GetDirectories(SCDirectory)
        DirectoryList.AddRange(Dirs)
        ViewSceneChecks.Items.Clear()

        For Each DirectoryName As String In Dirs
            DirectoryName = Replace(DirectoryName, SCDirectory, "")
            ViewSceneChecks.Items.Add(DirectoryName)
        Next
        ViewSceneChecks.Left = pboxScene.Left + 20
        ViewSceneChecks.Top = pboxScene.Top + 20
        ViewSceneChecks.Height = pboxScene.Height - 40
        ViewSceneChecks.Visible = True
    End Sub

    Private Sub ViewSceneChecks_Click(sender As Object, e As EventArgs) Handles ViewSceneChecks.Click
        If Not Directory.Exists(SCDirectory) Then
            MsgBox("No Saved Scene Checks")
            Exit Sub
        End If

        Dim ThisFolder As String = ViewSceneChecks.SelectedItem.ToString
        Dim LoadSCFile As String = SCDirectory & ThisFolder & "\SCFile.txt"

        Using reader As StreamReader = New StreamReader(LoadSCFile)
            ' Read each line from file
            Dim Line As String = vbNullString
            Do
                Line = reader.ReadLine
                If Line = "" Then Exit Do
                Dim LinArr() As String = Split(Line, ":")
                Select Case UCase(LinArr(0))
                    Case Is = "TOP"
                        Coord.Y = CInt(Val(Trim(LinArr(1))))
                    Case Is = "LEFT"
                        Coord.X = CInt(Val(Trim(LinArr(1))))
                    Case Is = "WIDTH"
                        Coord.Width = CInt(Val(Trim(LinArr(1))))
                    Case Is = "HEIGHT"
                        Coord.Height = CInt(Val(Trim(LinArr(1))))
                    Case Is = "ORIG RESOLUTION"
                        '= CInt(Val(Trim(LinArr(1))))
                    Case Is = "TYPE"
                        If Trim(UCase(LinArr(1))) = "TEXT" Then
                            rbOCR.Checked = True
                        ElseIf Trim(UCase(LinArr(1))) = "MOTION" Then
                            rbMotion.Checked = True
                        ElseIf Trim(UCase(LinArr(1))) = "IMAGE" Then
                            rbImage.Checked = True
                        End If
                    Case Is = "TEXT"
                        '= CInt(Val(Trim(LinArr(1))))
                End Select
            Loop Until Line = ""
        End Using

        ViewSceneChecks.Visible = False

        DrawBox()
        BoxVisible = True

        If rbOCR.Checked Then
            Dim TempBmp As New Bitmap(MasterPath & "tempbmp.bmp")
            Dim SCPoint As New Point(Coord.X, Coord.Y)
            Dim SCSize As New Size(Coord.Width, Coord.Height)
            Crop(TempBmp, SCPoint, SCSize)
            Dim ThisOCR As OCRResult = OCR.ReadText(TempBmp)
            lblSCConfidence.Text = ThisOCR.Confidence
            txtSCText.Text = ThisOCR.Text
            TempBmp.Dispose()
        End If

    End Sub

    Private Sub ViewSceneChecks_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ViewSceneChecks.SelectedIndexChanged

    End Sub

    Private Sub pboxScene_Click(sender As Object, e As EventArgs) Handles pboxScene.Click

    End Sub

End Class