Imports System.IO
Imports System.Threading
Imports System.Drawing.Imaging
Imports AXISMEDIACONTROLLib
Imports System.Runtime.InteropServices
Imports System.ServiceModel

Public Class TV
    Private StreamRez As Rectangle
    Private SCBmp As Bitmap = Nothing
    Private NextImageOnly As Boolean
    Private CurrentImage As Bitmap
    Private ServerError As Boolean
    Private StartErr As String
    Private Launched As Boolean

    Private Structure CharacterSet
        Dim CharValue As String
        Dim CharInc As Integer
    End Structure
    Private Structure PatternElement
        Dim StringValue As String
        Dim StringPosition As Integer
        Dim StringDominance As Integer
    End Structure

    Private Function GetStreamResolution() As Rectangle
        Dim FilPath As String = MasterPath & "TempBmp.bmp"
        AMC.SaveCurrentImage(1, FilPath)
        Do
            Application.DoEvents()
        Loop Until File.Exists(FilPath)
        Delay(500)

        Dim TempBmp As New Bitmap(FilPath)
        Dim Rez As Rectangle
        Rez.Height = CInt(TempBmp.Height)
        Rez.Width = CInt(TempBmp.Width)
        TempBmp.Dispose()
        If File.Exists(FilPath) Then File.Delete(FilPath)
        Return Rez
    End Function

    Private Sub btnStretch_Click(sender As System.Object, e As System.EventArgs) Handles btnStretch.Click
        AutoRez()
        ShowYourself()
    End Sub

    Private Sub AutoRez()
        Try
            StreamRez = GetStreamResolution()
            Me.Height = StreamRez.Height + 95
            Me.Width = StreamRez.Width + 20
            ResizeTV()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ShowYourself()
        Try
            Form1.WindowState = Me.WindowState 'FormWindowState.Normal
            Output.WindowState = Me.WindowState 'FormWindowState.Normal
            Scripting_Engine.WindowState = Me.WindowState 'FormWindowState.Normal
            ToolBox.WindowState = Me.WindowState 'FormWindowState.Normal
            CountDown.WindowState = Me.WindowState
        Catch ex As Exception
        End Try
    End Sub



    Private Sub ResizeTV()
        AMC.Width = Me.Width - 20
        AMC.Height = Me.Height - 95
        btnStretch.Top = Me.Height - btnStretch.Height - 40
        btnPlayback.Top = btnStretch.Top
        btnSceneCheck.Top = btnStretch.Top
        SetTheTable()
        lblURL.Left = CInt(Me.Width / 2 - lblURL.Width / 2)
    End Sub

    Private Sub TV_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ActiveRun = False
        Scripting_Engine.btnRun.Text = "Run"
        StopRemoteServices()
    End Sub

    Private Sub TV_ResizeEnd(sender As Object, e As System.EventArgs) Handles Me.ResizeEnd
        ResizeTV()
    End Sub

    Public Sub RefreshVideoServer(ByVal DeviceIP As String, ByVal devicetype As String, ByVal Camera As Integer)
        AMC.StopRecordMedia()
        AMC.StopRecord()
        AMC.Stop()
        Select Case (devicetype)
            Case "AXIS_M7001"
                AMC.MediaURL = "axrtsphttp://" & ActiveDevice.VideoServerIP & "/axis-media/media.amp"
                AMC.MediaType = "h264"
                'AMC.MediaType = "mjpeg"
                AMC.Play()
                If Not WaitForAxisOK() = "OK" Then MsgBox(StartErr)
            Case "AXIS_7401"

                '''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''
                'AMC.MediaURL = "axrtsphttp://" & ActiveDevice.VideoServerIP & "/axis-media/media.amp?camera=" & Camera.ToString
                'AMC.MediaType = "mjpeg"
                'AMC.Play()
                'If Not WaitForAxisOK() = "OK" Then MsgBox(StartErr)
                '''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''

                AMC.MediaURL = "axrtsphttp://" & ActiveDevice.VideoServerIP & "/axis-media/media.amp?videocodec=h264"
                AMC.Play()
            Case "AXIS_243SA"
                'WORKS(-MPEG4)
                AMC.MediaURL = "axrtsphttp://" & ActiveDevice.VideoServerIP & "/mpeg4/media.amp"
                AMC.Play()
            Case "AXIS-MJPEG"
                'WORKS(-MJPEG)
                AMC.MediaURL = "http://" & ActiveDevice.VideoServerIP & "/axis-cgi/mjpg/video.cgi"
                AMC.MediaType = "mjpeg"
                AMC.Play()
                ' If Not WaitForAxisOK() = "OK" Then MsgBox(StartErr)
            Case "AXIS_P7216"
                AMC.MediaURL = "axrtsphttp://" & ActiveDevice.VideoServerIP & "/axis-media/media.amp?camera=" & Camera.ToString
                AMC.MediaType = "h264"
                'AMC.MediaType = "mjpeg"
                AMC.Play()
                If Not WaitForAxisOK() = "OK" Then MsgBox(StartErr)
            Case "BLACKMAGICPRO_H264"

        End Select

    End Sub

    Private Function WaitForAxisOK() As String
        Dim StartTime As New Stopwatch
        StartTime.Start()
        StartErr = vbNullString
        Do
            Application.DoEvents()
            If AMCStatusOK() Then Exit Do
            If ServerError Then Exit Do
            If StartTime.ElapsedMilliseconds > 20000 Then
                StartErr = "Too Long"
                Exit Do
            End If
        Loop
        If StartErr <> vbNullString Then Return StartErr
        Return "OK"

    End Function

    Private Function AMCStatusOK() As Boolean
        'AMC_STATUS.AMC_STATUS_INITIALIZED = 1
        'AMC_STATUS.AMC_STATUS_PLAYING = 2
        'AMC_STATUS.AMC_STATUS_EXTENDEDTEXT = 1024
        'AMC_STATUS.AMC_STATUS_OPENING_RECEIVE_AUDIO = 65536
        'AMC_STATUS.AMC_STATUS_RECEIVE_AUDI0 = 262144

        'MINIMUM CONFIGURATION = 1027
        'IF OPENING AUDIO = 1027 + 65536 = 66563
        'IF RECEIVING AUDIO = 1027 + 262144 = 263171

        If AMC.Status = 1027 OrElse AMC.Status = 66563 OrElse AMC.Status = 263171 Then Return True
        Return False
    End Function

    Private Sub SignOn()
        'GET LABTOWN'S STREET ADDRESS, FTP URL, AND ACTIVE AUTOMATION DIRECTORY
        ReadSettings()

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''FTP
        'If FTPAddress = vbNullString OrElse FTPAddress = "" Then
        '    FTPAddress = InputBox("You must specify an FTP Address", "Server Address", "ftp://108.20.145.115/")
        '    If FTPAddress = vbNullString OrElse FTPAddress = "" Then
        '        MsgBox("Failed to Specify FTP Server Address" & vbCrLf & "Exiting LabTown")
        '        End
        '    End If
        'End If
        ''STREET
        If Not GetMyAddress() = True OrElse CheckIfRunning("VirtualRemoteControl") = True Then
            LabTownStreetAddress = InputBox("PLease enter a Street address")
            If LabTownStreetAddress = "" Then
                MsgBox("Failed to Specify an Asset" & vbCrLf & "Exiting LabTown")
                End
            End If
        End If
        ''GET PASSWORD
        'frmPassword.Show()
        'Do
        '    Application.DoEvents()
        'Loop Until AMC.MediaPassword <> ""
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        AMC.MediaUsername = "root"
        AMC.MediaPassword = "SPARTA"
        'BUILD STREET-SPECIFIC DIRECTORIES
        If LabTownStreetAddress <> vbNullString AndAlso LabTownStreetAddress <> "" Then
            DeviceDirectory = MasterPath & LabTownStreetAddress & "\Devices\"
            SCFramesDirectory = MasterPath & LabTownStreetAddress & "\SCFrames\"
            CommandDirectory = MasterPath & LabTownStreetAddress & "\Commands\"
            If Not Directory.Exists(DeviceDirectory) Then Directory.CreateDirectory(DeviceDirectory)
            If Not Directory.Exists(SCFramesDirectory) Then Directory.CreateDirectory(SCFramesDirectory)
            If Not Directory.Exists(CommandDirectory) Then Directory.CreateDirectory(CommandDirectory)
            ButtonFile = CommandDirectory & "BigButtonFile.txt"
        End If
        'GET DEVICES

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Dim FTPAvailable As Boolean = GetDevices()
        'LOAD DEVICE LIST
        LoadDevices()
        'If lstDevices.Count = 0 AndAlso Not FTPAvailable Then
        '    MsgBox("No Devices Found For Specified Street" & vbCrLf & _
        '           "Locally or at FTP Address" & vbCrLf & _
        '           "Exiting MediaLab")
        '    End
        'ElseIf Not FTPAvailable Then
        '    MsgBox("Unable to Locate Devices at FTP Address" & vbCrLf & _
        '           "Loading LabTown from Local Drive")
        'End If
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'ON SUCCESSFUL DEVICE LOAD, SAVE SETTINGS
        WriteSettings()
        'AND OPEN DEVICE PORTS
        SelectMultipleDevices()
        'GET RC AND BUTTONS
        Dim ButtonPath As String

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'If FTPAvailable Then
        '    ButtonPath = GetButtonFolder()
        'Else : ButtonPath = "C:\VRC\" & LabTownStreetAddress & "\Commands\"
        'End If
        ButtonPath = "C:\VRC\" & LabTownStreetAddress & "\Commands\"
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        GetIRFunctions(ButtonPath)
        Form1.LoadButtons()
        LoadRemote()
        'GET ANY NEW CUSTOM SCRIPTS

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'If FTPAvailable Then GetScripts()
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    End Sub
    Private Sub LoadRemote()
        Dim DestPath As String = "C:\VRC\" & LabTownStreetAddress & "\RC.png"
        If AppTools.RemoteControl <> vbNullString Then
            AppTools.FTPFile("Remote_Controls/" & AppTools.RemoteControl & "/RC.png", DestPath)
            Dim FileReady As Boolean
            Do
                FileReady = AppTools.IsFileAvailable(DestPath)
                Thread.Sleep(100)
            Loop Until FileReady = True
        End If
        If Not File.Exists(DestPath) Then
            MsgBox("Could Not Locate Remote Control Image")
            End
        End If
        Dim RCImage As New Bitmap(DestPath)
        Form1.pixRemoteControl.Image = RCImage
    End Sub
    Private Function GetButtonFolder() As String
        Dim ThisRemote As String = FTPAddress & "Streets/" & LabTownStreetAddress & "/RC.txt"
        If AppTools.FTPCheckIfFileExists(ThisRemote) Then
            AppTools.RemoteControl = AppTools.FTPGetFileText(ThisRemote)
            If AppTools.RemoteControl <> vbNullString Then Return FTPAddress & "Remote_Controls/" & RemoteControl & "/Commands/"
        End If
        Return FTPAddress & "Streets/" & LabTownStreetAddress & "/Commands/"
    End Function

    Private Function CheckIfRunning(sProcessName As String) As Boolean
        Dim bRet As Boolean = False
        Try
            Dim ProcessCount As Integer = Process.GetProcessesByName(sProcessName).Count()
            'Dim listProc() As Process
            'listProc = Process.GetProcessesByName(sProcessName)
            'If listProc.Length > 0 Then
            If ProcessCount > 1 Then
                Return True ' Process is running
            Else
                Return False ' Process is not running
            End If
        Catch ex As Exception
            Return True
        End Try
    End Function

    Private Sub CreateDirectories()
        If Not Directory.Exists(MasterPath) Then Directory.CreateDirectory(MasterPath)
        If Not Directory.Exists(MasterPath & "Automation\") Then Directory.CreateDirectory(MasterPath & "Automation\")
        If Not Directory.Exists(MasterPath & "Routines\") Then Directory.CreateDirectory(MasterPath & "Routines\")
        If Not Directory.Exists(MasterPath & "SceneChecks\") Then Directory.CreateDirectory(MasterPath & "SceneChecks\")
        If Not Directory.Exists(MasterPath & "CustomScript\") Then Directory.CreateDirectory(MasterPath & "CustomScript\")
    End Sub

    Private Sub TV_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        CreateDirectories()
        SignOn()

        PlaceScriptingEngine()
        PlaceRemoteControl()
        PlaceToolbox()
        WaitForAxisOK()
        SetTheTable()
        AMC.ShowToolbar = True
        AutoRez()
        'StartRemoteServices("http://localhost:8003/adcomparator")

        Output.WindowState = FormWindowState.Minimized
        Output.Show()
        LTInitialized = True
    End Sub

    Private Sub GetScripts()
        If Not Directory.Exists(MasterPath & "CustomScript\") Then Directory.CreateDirectory(MasterPath & "CustomScript\")
        Dim TempLstScripts As New List(Of String)
        Dim SourcePath As String = FTPAddress & "Streets/" & LabTownStreetAddress & "/CustomScript/"
        TempLstScripts = AppTools.FTPDirectoryList(SourcePath)
        If TempLstScripts Is Nothing Then Exit Sub
        For Each item As String In TempLstScripts
            Dim FilePath = "Streets/" & LabTownStreetAddress & "/CustomScript/" & item
            Dim DestinationPath As String = MasterPath & "CustomScript\" & item
            If Not File.Exists(DestinationPath) Then AppTools.FTPFile(FilePath, DestinationPath)
        Next
    End Sub
    Private Sub GetIRFunctions(ByVal SourcePath As String)
        Dim TempLstIRCommands As New List(Of String)
        If Not UCase(Mid(SourcePath, 1, 2)) = "C:" Then
            TempLstIRCommands = AppTools.FTPDirectoryList(SourcePath)
        Else : TempLstIRCommands = LocalDirectoryList(SourcePath)
        End If
        If TempLstIRCommands Is Nothing Then
            MsgBox("Unable to locate Remote Control commands from " & SourcePath)
            End
        End If
        Dim NewButton As Boolean = False
        For Each item As String In TempLstIRCommands
            Dim FilePath = "Remote_Controls/" & RemoteControl & "/Commands/" & item
            Dim DestinationPath As String = CommandDirectory & item
            If Not File.Exists(DestinationPath) Then
                NewButton = True
                AppTools.FTPFile(FilePath, DestinationPath)
            End If
        Next
        If NewButton = True Then 'Replace buttonfile
            Dim FilePath = "Remote_Controls/" & RemoteControl & "/Commands/BigButtonFile.txt"
            Dim DestinationPath As String = CommandDirectory & "BigButtonFile.txt"
            AppTools.FTPFile(FilePath, DestinationPath)
        End If
    End Sub

    Private Function GetMyAddress() As Boolean
        If Not File.Exists(MasterPath & "Settings.txt") Then Return False
        If LabTownStreetAddress = "" OrElse LabTownStreetAddress = vbNullString Then Return False
        Return True
    End Function

    Private Sub PlaceToolbox()
        ToolBox.Show()
        ToolBox.Top = Me.Top

        Dim RealEstate As Rectangle = Screen.PrimaryScreen.Bounds
        ToolBox.Height = Scripting_Engine.Height
    End Sub

    Private Sub PlaceRemoteControl()
        Form1.Show()
        Form1.Top = Me.Top + Me.Height - CInt(Form1.Height / 3)
        Form1.Left = Me.Left + Me.Width - CInt(Form1.Width / 5)
    End Sub
    Private Sub PlaceScriptingEngine()
        Scripting_Engine.Show()
        Dim RealEstate As Rectangle = Screen.PrimaryScreen.Bounds
        Scripting_Engine.Height = RealEstate.Height - Scripting_Engine.Top
    End Sub
    Private Sub SetTheTable()
        'Form1.GoToLowerLeft()
        Form1.GoDark(True)
        Scripting_Engine.Left = Me.Left + Me.Width
        Scripting_Engine.Top = Me.Top
        Output.Left = Me.Left
        Output.Top = Me.Top + Me.Height
        Output.Width = Me.Width
        ToolBox.Top = Scripting_Engine.Top
        ToolBox.Left = Scripting_Engine.Left + Scripting_Engine.Width
        ToolBox.Show()
        If Not Launched Then GoToUpperRight()
    End Sub
    Private Sub GoToUpperRight()
        Dim RealEstate As Rectangle = Screen.PrimaryScreen.Bounds
        Me.Top = RealEstate.Top
        Me.Left = RealEstate.Left
        Launched = True
    End Sub
    Private Sub AMC_OnError(sender As System.Object, e As AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEvent) Handles AMC.OnError
        StartErr = e.theErrorInfo
        'If InStr(UCase(StartErr), "CONNECTION LOST") <> 0 Then
        '    MsgBox(Now())
        'End If
        If InStr(LCase(StartErr), "password") <> 0 Then
            MsgBox("Incorrect Password" & "Exiting MediaLab")
            End
        End If
        ServerError = True
    End Sub

    Private Sub btnPlayback_Click(sender As System.Object, e As System.EventArgs) Handles btnPlayback.Click
        If btnPlayback.Text = "Play Live" Then
            btnPlayback.Text = "Play File"
            AMC.Stop()
            Thread.Sleep(50)
            AMC.MediaURL = AMC.MediaURL
            AMC.Play()
        Else
            btnPlayback.Text = "Play Live"
            Dim FilDialogue As New OpenFileDialog
            '"Text files (*.txt)|*.txt|All files (*.*)|*.*"
            FilDialogue.Filter = "Advanced System Format (*.asf)|*.asf|All files (*.*)|*.*"
            FilDialogue.InitialDirectory = "C:\Users\Earl\Documents\AXIS Media Control - Recordings\"
            If FilDialogue.ShowDialog = Windows.Forms.DialogResult.OK Then
                AMC.MediaFile = FilDialogue.FileName
                AMC.Stop()
                AMC.Play()
            End If
        End If
    End Sub

    Private Sub AMC_OnNewImage(sender As Object, e As System.EventArgs) Handles AMC.OnNewImage
        If NextImageOnly Then
            Dim ThisBmp As String = MasterPath & "TempBmp.bmp"
            AMC.SaveCurrentImage(0, ThisBmp)
            NextImageOnly = False
        ElseIf CaptureFrames Then
            'Dim TempFile As String = MasterPath & "SCFrames\"
            Dim TempFile As String = SCFramesDirectory
            FramesTaken += 1
            If FramesTaken = 2 Then
                If Not BackgroundWorker1.IsBusy Then
                    BWBusy = True
                    BackgroundWorker1.RunWorkerAsync(ActiveSCName)
                End If
            End If
            Dim jpgFile As String = TempFile & FramesTaken.ToString & ".jpg"
            AMC.SaveCurrentImage(1, jpgFile)
            If FramesTaken >= SCFrameCount + 1 Then
                CaptureFrames = False
            End If
        End If
    End Sub

    Public Sub CloneBmp(ByRef source As Bitmap)
        Try
            Dim rectSource As New Rectangle(New Point, source.Size)
            Dim rectDestination As New Rectangle(New Point, source.Size)
            Using result As New Bitmap(source.Width, source.Height)
                Using objGraphics As Graphics = Graphics.FromImage(result)
                    objGraphics.DrawImage(source, rectDestination, rectSource, GraphicsUnit.Pixel)
                End Using
                source.Dispose()
                source = DirectCast(result.Clone, Bitmap)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub btnSceneCheck_Click(sender As System.Object, e As System.EventArgs) Handles btnSceneCheck.Click
        NextImageOnly = True
        Do
            Application.DoEvents()
        Loop Until NextImageOnly = False
        Thread.Sleep(100)
        Dim ThisBmpFile As String = MasterPath & "TempBmp.bmp"
        Dim ThisBmp As New Bitmap(ThisBmpFile)
        CloneBmp(ThisBmp)
        SCBuilder.pboxScene.Image = CType(ThisBmp.Clone, Image)
        SCBuilder.pboxScene.Refresh()
        SCBuilder.Show()
    End Sub

    Public Sub BackgroundWorker1_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim gothere As Integer = 0
        'IMAGE DETECT
        Dim ThisActiveSC As String = DirectCast(e.Argument, String)
        Dim CroppedSCDirectory As String = MasterPath & LabTownStreetAddress & "\SCCrops\"
        Dim OrigSCDirectory As String = SCFramesDirectory
        Dim GotBmp As Boolean

        Dim WaitForFiles As New Stopwatch
        WaitForFiles.Start()
        Dim FileCount As Integer
        Do While FileCount = 0
            FileCount = (My.Computer.FileSystem.GetFiles(OrigSCDirectory)).Count
            If WaitForFiles.ElapsedMilliseconds > 5000 Then Exit Do
            If FileCount > 0 Then Exit Do
        Loop
        WaitForFiles.Stop()
        WaitForFiles.Reset()
        If FileCount = 0 Then
            ActiveSceneFound = False
            AppTools.OCRText = "##$$%%!!"
            Exit Sub
        End If

        If CurrentSceneCheck.Type = "Image" Then
            'get the saved SC Bitmap and FrameData
            Dim CompareBmp As New Bitmap(SCDirectory & ThisActiveSC & "\SCBmp.bmp")
            Dim CompareFrameData As FrameData = Pixels.ExtractPixels(CompareBmp)
            Dim Thresh As Decimal = CDec(0.14 * CurrentSceneCheck.Sensitivity)
            ActiveSceneFound = False
            Try
                Dim IntN As Integer
                Do
                    IntN += 1
                    If BackgroundWorker1.CancellationPending Then Exit Do
                    'Verify Next BitMap Exists
                    Dim StopWatch1 As New Stopwatch
                    StopWatch1.Start()
                    Do While Not BackgroundWorker1.CancellationPending
                        Application.DoEvents()
                        If File.Exists(OrigSCDirectory & IntN + 1 & ".jpg") Then Exit Do
                        If StopWatch1.ElapsedMilliseconds > 5000 Then Exit Do
                    Loop
                    StopWatch1.Reset()
                    If File.Exists(OrigSCDirectory & IntN & ".jpg") Then
                        Dim ThisBmp As Bitmap = Nothing
                        Try
                            ThisBmp = New Bitmap(OrigSCDirectory & IntN & ".jpg")
                            GotBmp = True
                        Catch ex As Exception
                            Delay(333)
                            If GotBmp = False Then
                                IntN -= 1
                                GotBmp = True
                            Else : GotBmp = False ' only retry once
                            End If
                        End Try
                        If GotBmp Then
                            GotBmp = False
                            Dim ThisPt As New Point(CurrentSceneCheck.Rect.X, CurrentSceneCheck.Rect.Y)
                            Dim ThisSize As New Size(CurrentSceneCheck.Rect.Width, CurrentSceneCheck.Rect.Height)
                            Crop(ThisBmp, ThisPt, ThisSize)
                            'compare 
                            'The LOWER the threshold, the MORE sensitive the Scene Check
                            If CheckForSameness(ThisBmp, CompareBmp, Thresh) Then 'if motion is less than threshold 
                                AppTools.FoundFrame = IntN
                                ActiveSceneFound = True
                                Exit Do
                            End If
                            If Not ThisBmp Is Nothing Then ThisBmp.Dispose()
                        End If
                    End If
                Loop Until IntN = SCFrameCount
            Catch ex As Exception
                'MsgBox("BackgroundWorker => " & ex.Message)
            End Try
            If Not CompareBmp Is Nothing Then CompareBmp.Dispose()
        ElseIf CurrentSceneCheck.Type = "Motion" Then
            'MOTION DETECT
            'get the saved SC Bitmap and FrameData
            Dim CompareBmp As Bitmap = Nothing
            Dim CompareFrameData As FrameData = Nothing
            Dim Thresh As Decimal = CDec(0.07 * CurrentSceneCheck.Sensitivity)
            ActiveSceneFound = False
            Try
                Dim intN As Integer
                Do
                    intN += 1
                    If BackgroundWorker1.CancellationPending Then Exit Do
                    'verify NEXT bitmap exists
                    Dim Stopwatch1 As New Stopwatch
                    Stopwatch1.Start()
                    Do While Not BackgroundWorker1.CancellationPending
                        Application.DoEvents()
                        If File.Exists(OrigSCDirectory & intN + 1 & ".jpg") Then Exit Do
                        If Stopwatch1.ElapsedMilliseconds > 5000 Then Exit Do
                    Loop
                    Stopwatch1.Reset()
                    'crop the coordinates
                    If File.Exists(OrigSCDirectory & intN & ".jpg") Then
                        Dim ThisBmp As Bitmap = Nothing
                        Try
                            GotBmp = True
                            ThisBmp = New Bitmap(OrigSCDirectory & intN & ".jpg")
                        Catch ex As Exception
                            Delay(333)
                            If GotBmp = False Then
                                intN -= 1
                                GotBmp = True
                            Else : GotBmp = False ' only retry once
                            End If
                        End Try
                        If GotBmp = True Then
                            GotBmp = False
                            Dim ThisPt As New Point(CurrentSceneCheck.Rect.X, CurrentSceneCheck.Rect.Y)
                            Dim ThisSize As New Size(CurrentSceneCheck.Rect.Width, CurrentSceneCheck.Rect.Height)
                            Crop(ThisBmp, ThisPt, ThisSize)
                            'compare crop
                            If intN = 1 Then
                                'get the saved SC Bitmap and FrameData
                                CompareBmp = New Bitmap(ThisBmp)
                                CompareFrameData = Pixels.ExtractPixels(CompareBmp)
                            Else
                                'compare 
                                'The LOWER the threshold, the MORE sensitive the scene check is
                                If Not CheckForSameness(ThisBmp, CompareBmp, Thresh) Then 'if motion is more than threshold 
                                    AppTools.FoundFrame = intN
                                    ActiveSceneFound = True
                                    Exit Do
                                End If
                            End If
                            If Not ThisBmp Is Nothing Then ThisBmp.Dispose()
                        End If
                    End If
                Loop Until intN = SCFrameCount
            Catch ex As Exception
                'MsgBox("BackgroundWorker => " & ex.Message)
            End Try
            If Not CompareBmp Is Nothing Then CompareBmp.Dispose()
        ElseIf CurrentSceneCheck.Type = "Text" Then
            'Dim SecureOCR As Boolean = False
            'If LCase(OCRSearchText) = "anytext" And CurrentSceneCheck.Duration > 0.1 Then SecureOCR = True
            Dim lstSecureOCR As New List(Of String)
            ActiveSceneFound = False
            Try
                Dim OCRNum As Integer
                Dim lstOCR As New List(Of Integer)
                Do
                    Dim OCRfile As String = vbNullString
                    If BackgroundWorker1.CancellationPending Then Exit Do
                    'get most recent scene check file
                    Dim StopWatch1 As New Stopwatch
                    StopWatch1.Start()
                    Do While Not BackgroundWorker1.CancellationPending
                        Try
                            If StopWatch1.ElapsedMilliseconds > 5000 Then Exit Do
                            If (My.Computer.FileSystem.GetFiles(OrigSCDirectory)).Count > 0 Then
                                OCRfile = Directory.GetFiles(OrigSCDirectory).OrderByDescending(Function(f) New FileInfo(f).LastWriteTime).First()
                                If InStr(OCRfile, ".jpg") <> 0 Then Exit Do
                            End If
                            Application.DoEvents()
                        Catch ex As Exception

                        End Try
                    Loop
                    StopWatch1.Reset()
                    If Not File.Exists(OCRfile) Then Exit Do
                    'verify file is accessible / done writing
                    Dim ThisBmp As Bitmap = Nothing
                    For IntTryToOpen = 1 To 2
                        Try
                            ThisBmp = New Bitmap(OCRfile)
                            GotBmp = True
                            Exit For
                        Catch ex As Exception
                            Delay(333)
                        End Try
                    Next
                    If GotBmp Then
                        'get file number
                        Dim OCRfileStr As String = Replace(LCase(OCRfile), LCase(OrigSCDirectory), "")
                        OCRfileStr = Replace(LCase(OCRfileStr), ".jpg", "")
                        OCRfileStr = Replace(OCRfileStr, "\", "")
                        OCRNum = CInt(Val(OCRfileStr))
                        lstOCR.Add(OCRNum)
                        'crop image / create bitmap
                        GotBmp = False
                        Dim ThisPt As New Point(CurrentSceneCheck.Rect.X, CurrentSceneCheck.Rect.Y)
                        Dim ThisSize As New Size(CurrentSceneCheck.Rect.Width, CurrentSceneCheck.Rect.Height)
                        Crop(ThisBmp, ThisPt, ThisSize)
                        'extract text
                        Dim ThisOCRResult As OCRResult
                        ThisOCRResult = OCR.ReadText(ThisBmp)
                        AppTools.OCRText = ThisOCRResult.Text
                        'Application.DoEvents()
                        'If SecureOCR Then
                        '    lstSecureOCR.Add(ThisOCRResult.Text)
                        'ElseIf InStr(LCase(ThisOCRResult.Text), LCase(OCRSearchText)) <> 0 OrElse LCase(OCRSearchText) = "anytext" Then
                        If InStr(LCase(ThisOCRResult.Text), LCase(OCRSearchText)) <> 0 OrElse LCase(OCRSearchText) = "anytext" Then
                            'HERE'S WHERE I FIX THE ANYTEXT OCR
                            AppTools.FoundFrame = OCRNum
                            ActiveSceneFound = True
                            Exit Do
                        End If
                        If Not ThisBmp Is Nothing Then ThisBmp.Dispose()
                    End If
                Loop Until OCRNum >= SCFrameCount
                'If SecureOCR Then AppTools.OCRText = DominantCompositeText(lstSecureOCR)
                'MsgBox(DominantCompositeTextByPatternsWithWildCards(lstSecureOCR))
            Catch ex As Exception
                'MsgBox("BackgroundWorker => " & ex.Message)
            End Try
        End If
        CaptureFrames = False
        'SCDone = True
    End Sub

    Private Function FindAllPatterns(ByVal OCRs As List(Of String)) As List(Of List(Of String))
        'FIND ALL PATTERNS
        '1  A/B=   123-4  and 89
        '2  A/C=   123-45 and -789        
        '3  A/D=   123    and 456  and 89
        '4  A/E=   123    and 45   and 89
        '5  B/C=   123-4  and 89
        '6  B/D=   123    and 89
        '7  B/E=   123    and 89
        '8  C/D=   123    and 45   and 89
        '9  C/E=   123    and 45-  and 89
        '10 D/E=   123045 and 89
        Dim ResultList As New List(Of List(Of String))
        For intA As Integer = 0 To OCRs.Count - 2
            Dim A As String = OCRs(intA)
            For intB As Integer = intA + 1 To OCRs.Count - 1
                Dim B As String = OCRs(intB)
                Dim result As List(Of String) = FindPatterns(UCase(A), UCase(B))
                If Not result Is Nothing Then ResultList.Add(result)
            Next
        Next
        Return ResultList
    End Function

    Private Function FindAllPatternsWithWildCards(ByVal OCRs As List(Of String)) As List(Of String)
        'Find First Set of Patterns 
        '1  A/B=   123-4****89
        '2  A/C=   123-45*-789        
        '3  A/D=   123*456**89
        '4  A/E=   123*45***89
        '5  B/C=   123-4****89
        '6  B/D=   123******89
        '7  B/E=   123******89
        '8  C/D=   123*45**89
        '9  C/E=   123*45-*89
        '10 D/E=   123045**89
        Dim ResultList As New List(Of String)
        For intA As Integer = 0 To OCRs.Count - 2
            Dim A As String = OCRs(intA)
            For intB As Integer = intA + 1 To OCRs.Count - 1
                Dim B As String = OCRs(intB)
                Dim MatchStringWithWildCards As String = FindPatternsWithWildCards(UCase(A), UCase(B))
                If Not MatchStringWithWildCards Is Nothing Then ResultList.Add(MatchStringWithWildCards)
            Next
        Next
        Return ResultList
    End Function

    Private Function AddPositionsToPatterns(ByVal PList As List(Of List(Of String))) As List(Of PatternElement)
        'LIST ALL UNIQUE PATTERNS AND THEIR POSITINS
        'Pattern    Position
        '123-4      0
        '89         1000
        '123-45     0
        '-789       1000
        '123        0   
        '456        1
        '45         1
        '45-        1
        '123045     0
        Dim ResultPatterns As New List(Of PatternElement)
        For Each Patterns As List(Of String) In PList
            For Each IndivPattern As String In Patterns
                If ResultPatterns.Count = 0 Then
                    Dim ThisPattern As PatternElement
                    ThisPattern.StringValue = IndivPattern
                    ThisPattern.StringPosition = Patterns.IndexOf(IndivPattern)
                    ResultPatterns.Add(ThisPattern)
                    Continue For
                End If
                Dim AlreadySaved As Boolean = False
                For Each SavedPattern As PatternElement In ResultPatterns
                    If SavedPattern.StringValue = IndivPattern Then
                        AlreadySaved = True
                        Exit For
                    End If
                Next
                If Not AlreadySaved Then
                    Dim ThisPattern As PatternElement
                    ThisPattern.StringValue = IndivPattern
                    ThisPattern.StringPosition = Patterns.IndexOf(IndivPattern)
                    If ThisPattern.StringPosition = Patterns.Count - 1 Then ThisPattern.StringPosition = 1000
                    ResultPatterns.Add(ThisPattern)
                End If
            Next
        Next
        Return ResultPatterns
    End Function

    Private Function AddWeightingToPatterns(ByRef PatternsDefined As List(Of PatternElement), ByVal OCRs As List(Of String)) As List(Of Decimal)
        'DETERMINE THE % INCIDENCE OF EACH PATTERN
        'also - build a weighting list
        '
        'Pattern    Position    Percent
        '123-4      0           60
        '89         1000        100
        '123-45     0           40
        '-789       1000        40    
        '123        0           100
        '456        1           40
        '45         1           80
        '45-        1           20
        '123045     0           20
        Dim TheseWeights As New List(Of Decimal)
        Dim PercentQuantum As Decimal = CDec(1 / OCRs.Count)
        For intPattern As Integer = 0 To PatternsDefined.Count - 1
            Dim Pattern As PatternElement = PatternsDefined(intPattern)
            Dim PercentIncidence As Decimal = 0
            For Each OCR As String In OCRs
                If InStr(OCR, Pattern.StringValue) <> 0 Then PercentIncidence += PercentQuantum
            Next
            Dim alreadylogged As Boolean = False
            PercentIncidence = CInt(PercentIncidence * 100)
            If TheseWeights.Count > 0 Then
                For Each thisentry As Decimal In TheseWeights
                    If thisentry = PercentIncidence Then
                        alreadylogged = True
                        Exit For
                    End If
                Next
            End If
            If Not alreadylogged Then TheseWeights.Add(PercentIncidence)
            Pattern.StringDominance = CInt(Math.Ceiling(PercentIncidence))
            PatternsDefined(intPattern) = Pattern
        Next
        If Not TheseWeights.Count = 0 Then TheseWeights.Sort()
        Return TheseWeights
    End Function

    Private Function MaxMinAvgSTD(ByRef MinLen As Integer, ByRef MaxLen As Integer, ByRef Meanlen As Decimal, ByVal OCRs As List(Of String)) As Decimal
        'DETERMINE STRING LENGTH PARAMETERS
        'min=10
        'max = 11
        'avg = 10
        'sd = 1
        Dim TotLen As Decimal = 0
        Dim lstLengths As New List(Of Integer)
        For Each OCR As String In OCRs
            Dim ThisLength As Integer = Len(OCR)
            lstLengths.Add(Len(OCR))
            MinLen = Math.Min(MinLen, ThisLength)
            MaxLen = Math.Max(MaxLen, ThisLength)
            TotLen += ThisLength
        Next
        Meanlen = CDec(TotLen / OCRs.Count)
        Dim lstSquares As New List(Of Decimal)
        For Each Int As Integer In lstLengths
            Dim Delta As Decimal = Int - Meanlen
            lstSquares.Add(Delta * Delta)
        Next
        TotLen = 0
        For Each Squared As Decimal In lstSquares
            TotLen += Squared
        Next
        Dim SDMEan As Decimal = TotLen / lstSquares.Count
        Return CDec(Math.Sqrt(SDMEan))
    End Function
    Private Function DominantCompositeTextByPatternsWithWildCards(ByVal fIRSTOrigOCRstrings As List(Of String)) As String
        'REMOVE NULLSTRINGS
        Dim ORIGOCRSTRINGS As New List(Of String)
        For Each THISSTRING As String In fIRSTOrigOCRstrings
            ClearUnprintables(THISSTRING)
            If THISSTRING = vbNullString Then Continue For
            ORIGOCRSTRINGS.Add(THISSTRING)
        Next
        If ORIGOCRSTRINGS.Count = 0 Then Return vbNullString

        'DETERMINE NUMBER OF WORDS
        Dim totwords As Integer = 0
        Dim AvgWords As Integer = 0
        For Each thisString As String In ORIGOCRSTRINGS
            Dim strArray() As String = Split(thisString)
            totwords += strArray.Count
        Next
        If Not totwords = 0 AndAlso Not ORIGOCRSTRINGS.Count = 0 Then AvgWords = CInt(Math.Round(totwords / ORIGOCRSTRINGS.Count))
        'REMOVE OUTLIER STRINGS (TOO MANY OR TOO FEW WORDS)
        Dim OCRStrings As New List(Of String)
        For Each thisstring As String In ORIGOCRSTRINGS
            Dim strArray() As String = Split(thisstring)
            If strArray.Count = AvgWords Then
                OCRStrings.Add(thisstring)
            End If
        Next
        ORIGOCRSTRINGS.Clear()
        'BUILD A LIST OF STRINGS
        Dim WordPatternsList As New List(Of List(Of String))
        For intN = 0 To AvgWords - 1
            Dim WordList As New List(Of String)
            For Each thisString As String In OCRStrings
                Dim strARray() As String = Split(thisString)
                WordList.Add(strARray(intN))
            Next
            'GET AVERAGE LENGTH OF THE WORD LIST
            Dim MinLen As Integer = 1000
            Dim MaxLen As Integer = 0
            Dim MeanLen As Decimal = 0
            Dim StdDev As Decimal = MaxMinAvgSTD(MinLen, MaxLen, MeanLen, WordList)
            Dim PatternsList As New List(Of List(Of String))
            If StdDev + MeanLen >= 3 Then
                WordPatternsList = FindAllPatterns(WordList)
                'DELETE NULL PATTERNS
                For Each thisList As List(Of String) In WordPatternsList
                    Dim RecordList As New List(Of String)
                    For Each thisstring As String In thisList
                        ClearUnprintables(thisstring)
                        If Not thisstring = vbNullString Then RecordList.Add(thisstring)
                    Next
                    If Not RecordList.Count = 0 Then PatternsList.Add(RecordList)
                Next
            Else
                Dim Recordlist As New List(Of String)
                For Each thisstring As String In WordList
                    ClearUnprintables(thisstring)
                    If Not thisstring = vbNullString Then
                        Recordlist.Add(thisstring)
                        PatternsList.Add(Recordlist)
                    End If
                Next
            End If
            'AT THIS POINT PATTERNSLIST HAS EVERY PATTER COMBINATION FOR THE SELECTED WORD
            'GET THE AVERAGE NUMBER OF PATTERNS IN PATTERNSLIST
            Dim TotPatternCount As Integer = 0
            Dim AvgPatternCount As Integer = 0
            For Each THISLIST As List(Of String) In PatternsList
                TotPatternCount += THISLIST.Count
            Next
            If TotPatternCount <> 0 AndAlso PatternsList.Count <> 0 Then AvgPatternCount = CInt(Math.Round(TotPatternCount / PatternsList.Count))
            'REMOVE OUTLIERS (TOO MANY OR TOO FEW PATTERNS
            Dim FinalWordPatternsList As New List(Of List(Of String))
            For Each ThisList As List(Of String) In PatternsList
                If ThisList.Count = AvgPatternCount Then FinalWordPatternsList.Add(ThisList)
            Next
        Next



        'A=123-456-789
        'B=123-444-489
        'C=123-45-789
        'D=1230456089
        'E=123045--89

        'Find Patterns with WildCards
        '1  A/B=   123-4****89
        '2  A/C=   123-45*-789        
        '3  A/D=   123*456**89
        '4  A/E=   123*45***89
        '5  B/C=   123-4****89
        '6  B/D=   123******89
        '7  B/E=   123******89
        '8  C/D=   123*45**89
        '9  C/E=   123*45-*89
        '10 D/E=   123045**89
        Dim PatternsListWithWildCards As New List(Of String)
        PatternsListWithWildCards = FindAllPatternsWithWildCards(OCRStrings)

        Return "SomeString"


    End Function


    Private Sub ClearUnprintables(ByRef TheString As String)
        For IntASCii As Integer = 1 To 31
            Dim DeleteChar As Char = Chr(IntASCii)
            TheString = Replace(TheString, DeleteChar, "")
        Next

    End Sub

    Private Function DominantCompositeTextByPatterns(ByVal OCRstrings As List(Of String)) As String
        If OCRstrings.Count = 0 Then Return vbNullString
        'A=123-456-789
        'B=123-444-489
        'C=123-45-789
        'D=1230456089
        'E=123045--89
        Dim PatternsList As New List(Of List(Of String))
        PatternsList = FindAllPatterns(OCRstrings)

        Dim PatternsDefined As New List(Of PatternElement)
        PatternsDefined = AddPositionsToPatterns(PatternsList)

        Dim PercentQuantum As Decimal = CDec(1 / OCRstrings.Count)
        Dim Weights As New List(Of Decimal)
        Weights = AddWeightingToPatterns(PatternsDefined, OCRstrings)

        Dim MinLen As Integer = 10000
        Dim MaxLen As Integer = 0
        Dim TotLen As Decimal = 0
        Dim MeanLen As Decimal = 0
        ' Dim StdDEv As Integer = MaxMinAvgSTD(MinLen, MaxLen, MeanLen, OCRstrings)

        'BUILD COMPOSITE
        'Raw Dominance                              Concantenated /w wild cards         Minimum composite
        '100%   123 and 89                          123*89                              min: 12389 (5 Char)             too short-go deep
        '80%    45                                  123*45*89                           min: 1234589 (7 Char)           too short-go deeper
        '60%    123-4                               123-4*45*89                         min: 123-44589 (9 Char)         too short-deeper still
        '40%    123-45 and 456 and -789             123-4*456*-789                      min: 123-4456-789 (12 Char)     too long - find overlap     123-456-789
        Dim StopString As String = vbNullString
        Dim WeightSpecificPatternList As New List(Of PatternElement)
        Dim MidEntries As New List(Of PatternElement)
        Dim StartEntries As New List(Of PatternElement)
        Dim StopEntries As New List(Of PatternElement)
        For intN = Weights.Count - 1 To 0 Step -1
            Dim FinalString As String = vbNullString
            Dim StartString As String = vbNullString
            Dim MiddleString As String = vbNullString
            If MidEntries.Count > 0 Then MidEntries.Clear()
            If StartEntries.Count > 0 Then StartEntries.Clear()
            If StopEntries.Count > 0 Then StopEntries.Clear()
            'counting down from 100% match
            Dim Weighting As Decimal = Weights(intN)
            'collect all the string patterns with dominance at or above the current weighting (% match)
            If WeightSpecificPatternList.Count > 0 Then WeightSpecificPatternList.Clear()
            For Each StringPattern As PatternElement In PatternsDefined
                If StringPattern.StringDominance >= Weighting Then WeightSpecificPatternList.Add(StringPattern)
            Next
            Dim MaxMidPosition As Integer = 0
            'fill WeightSpecificPatternList
            For Each stringpattern As PatternElement In WeightSpecificPatternList
                If stringpattern.StringDominance < Weighting Then Continue For
                Select Case stringpattern.StringPosition
                    Case 0
                        'fill startentries list
                        StartEntries.Add(stringpattern)
                        StartString = stringpattern.StringValue
                    Case 1000
                        'fill stopentries list
                        StopEntries.Add(stringpattern)
                        StopString = stringpattern.StringValue
                    Case Else
                        'fill midentries list
                        MidEntries.Add(stringpattern)
                        MaxMidPosition = Math.Max(MaxMidPosition, stringpattern.StringPosition)
                End Select
            Next
            'build composites of starts and stops
            If StartEntries.Count > 1 Then
                For intEntry As Integer = 0 To StartEntries.Count - 2
                    Dim A As String = StartEntries(intEntry).StringValue
                    Dim B As String = StartEntries(intEntry + 1).StringValue
                    If InStr(A, B) <> 0 Then
                        StartString = A
                    ElseIf InStr(B, A) <> 0 Then
                        StartString = B
                    Else
                        Dim StartStringPatterns As List(Of String) = FindPatterns(A, B)
                        'Here's where I take off
                    End If
                Next
            End If
            If StopEntries.Count > 1 Then
                For intentry As Integer = 0 To StopEntries.Count - 2
                    Dim A As String = StopEntries(intentry).StringValue
                    Dim B As String = StopEntries(intentry).StringValue
                    If InStr(A, B) <> 0 Then
                        StopString = A
                    ElseIf InStr(B, A) <> 0 Then
                        StopString = B
                    Else
                        Dim STopSTringPatterns As List(Of String) = FindPatterns(A, B)
                        'here's where I take off
                    End If
                Next
            End If

            'place the midentries into position and build composites
            If MidEntries.Count > 0 Then
                For ThisPos As Integer = 1 To MaxMidPosition
                    For Each midelement As PatternElement In MidEntries
                        Dim wildcard As String = vbNullString
                        If MiddleString <> vbNullString Then wildcard = "*"
                        If midelement.StringPosition = ThisPos Then MiddleString &= midelement.StringValue
                    Next
                Next
            End If

            FinalString = StartString & "*" & MiddleString & "*" & StopString
            Dim Stringlen As Integer = Len(Replace(FinalString, "*", vbNullString))
            If Stringlen >= MinLen AndAlso Stringlen <= MaxLen Then
                'If Stringlen >= MeanLen - StdDEv AndAlso Stringlen <= MeanLen + StdDEv Then
                '    Exit For
                'End If
            End If
        Next

        Return "SomeString"

    End Function

    Private Function FindPatterns(ByVal AString As String, ByVal BString As String) As List(Of String) 'UPPERCASE ONLY
        'A=123-456-789
        'B=123-444-489
        'C=123-45-789
        'D=1230456089
        'E=123045--89

        'Find First Set of Patterns 
        '1  A/B=   123-4  and 89
        '2  A/C=   123-45 and -789        
        '3  A/D=   123    and 456  and 89
        '4  A/E=   123    and 45   and 89
        '5  B/C=   123-4  and 89
        '6  B/D=   123    and 89
        '7  B/E=   123    and 89
        '8  C/D=   123    and 45   and 89
        '9  C/E=   123    and 45-  and 89
        '10 D/E=   123045 and 89
        Dim AChar As String = vbNullString
        Dim BChar As String = vbNullString
        Dim lstMatches As New List(Of String)
        'determine how far off in aboslute position, to allow for a correlating letter
        Dim LetterPositionCorrelationSlop As Integer = CInt(Math.Ceiling(0.1 * Len(AString)))
        'bail if the strings don't correlate in length
        If Len(AString) > Len(BString) + LetterPositionCorrelationSlop OrElse Len(AString) < Len(BString) - LetterPositionCorrelationSlop Then Return lstMatches
        'find the first matched character that falls within the correlating position
        Dim APosition As Integer = 1
        Dim BPosition As Integer
        Do
            Dim MatchedChar As String = vbNullString
            For IntA = APosition To Len(AString)
                AChar = Mid(AString, IntA, 1)
                Dim BStart As Integer = Math.Max(IntA - LetterPositionCorrelationSlop, 1)
                Dim BStop As Integer = Math.Min(IntA + LetterPositionCorrelationSlop, Len(BString))
                For IntB = BStart To BStop
                    BChar = Mid(BString, IntB, 1)
                    If BChar = AChar Then
                        MatchedChar = BChar
                        APosition = IntA
                        BPosition = IntB
                        Exit For
                    End If
                Next
                If MatchedChar <> vbNullString Then Exit For
            Next
            'bail if no match
            If MatchedChar = vbNullString Then Return lstMatches
            'find all successive matched characters (if any)
            Dim MatchedString As String = MatchedChar
            Do
                APosition += 1
                BPosition += 1
                If BPosition > Len(BString) OrElse APosition > Len(AString) Then Exit Do
                AChar = Mid(AString, APosition, 1)
                BChar = Mid(BString, BPosition, 1)
                If AChar = BChar Then
                    MatchedString &= AChar
                Else
                    Exit Do
                End If
            Loop
            'save matched string (if any)
            If Len(MatchedString) > 1 Then
                lstMatches.Add(MatchedString)
            End If
            If BPosition > Len(BString) OrElse APosition > Len(AString) Then Exit Do
        Loop

        Return lstMatches
    End Function
    Private Function FindPatternsWithWildCards(ByVal AString As String, ByVal BString As String) As String 'UPPERCASE ONLY
        'A=123-456-789
        'B=123-444-489
        'C=123-45-789
        'D=1230456089
        'E=123045--89

        'Find First Set of Patterns 
        '1  A/B=   123-4****89
        '2  A/C=   123-45*-789        
        '3  A/D=   123*456**89
        '4  A/E=   123*45***89
        '5  B/C=   123-4****89
        '6  B/D=   123******89
        '7  B/E=   123******89
        '8  C/D=   123*45**89
        '9  C/E=   123*45-*89
        '10 D/E=   123045**89
        Dim AChar As String = vbNullString
        Dim BChar As String = vbNullString
        Dim MatchedStringWithWildCards As String = vbNullString
        'determine how far off in aboslute position, to allow for a correlating letter
        Dim LetterPositionCorrelationSlop As Integer = CInt(Math.Ceiling(0.1 * Len(AString)))
        'bail if the strings don't correlate in length
        If Len(AString) > Len(BString) + LetterPositionCorrelationSlop OrElse Len(AString) < Len(BString) - LetterPositionCorrelationSlop Then Return MatchedStringWithWildCards
        'find the first matched character that falls within the correlating position
        Dim APosition As Integer = 0
        Dim BPosition As Integer = 1
        Dim BStart As Integer = 1
        Dim ListMatchWithWildCards As New List(Of String)
        Do
            Dim MatchedChar As String = vbNullString
            APosition = ListMatchWithWildCards.Count + 1
            If APosition < 1 Then APosition = 1
            For IntA = APosition To Len(AString)
                AChar = Mid(AString, IntA, 1)
                Dim BStop As Integer = Math.Min(IntA + LetterPositionCorrelationSlop, Len(BString))
                For IntB = BStart To BStop
                    BChar = Mid(BString, IntB, 1)
                    If BChar = AChar Then
                        MatchedChar = BChar
                        APosition = IntA
                        BPosition = IntB
                        Exit For
                    End If
                Next
                If MatchedChar <> vbNullString Then
                    Exit For
                Else
                    ListMatchWithWildCards.Add("*")
                End If
            Next
            'bail if no match
            If MatchedChar = vbNullString Then
                For Each strEntry In ListMatchWithWildCards
                    MatchedStringWithWildCards &= strEntry
                Next
                Return MatchedStringWithWildCards
            End If
            'find all successive matched characters (if any)
            Dim FoundConsecMatch As Boolean = False
            Do
                APosition += 1
                BPosition += 1
                If BPosition > Len(BString) OrElse APosition > Len(AString) Then Exit Do
                AChar = Mid(AString, APosition, 1)
                BChar = Mid(BString, BPosition, 1)
                If AChar = BChar Then
                    FoundConsecMatch = True
                    If MatchedChar <> vbNullString Then
                        ListMatchWithWildCards.Add(MatchedChar)
                    End If
                    MatchedChar = vbNullString
                    ListMatchWithWildCards.Add(BChar)
                    BStart = BPosition + 1
                Else
                    If Not FoundConsecMatch Then
                        ListMatchWithWildCards.Add("*")
                    End If
                    Exit Do
                End If
            Loop
            If BPosition > Len(BString) OrElse APosition > Len(AString) Then Exit Do
        Loop
        'Build String from list of characters
        For Each strEntry In ListMatchWithWildCards
            MatchedStringWithWildCards &= strEntry
        Next
        Return MatchedStringWithWildCards
    End Function

    Private Function FindPatternsWithWildCards_NonExclusive(ByVal AString As String, ByVal BString As String) As String 'UPPERCASE ONLY
        'If Len(BString) > Len(AString) Then
        '    Dim tempstring As String = AString
        '    AString = BString
        '    BString = tempstring
        'End If

        'A=123-456-789
        'B=123-444-489
        'C=123-45-789
        'D=1230456089
        'E=123045--89

        'Find First Set of Patterns 
        '1  A/B=   123-4****89
        '2  A/C=   123-45*-789        
        '3  A/D=   123*456**89
        '4  A/E=   123*45***89
        '5  B/C=   123-4****89
        '6  B/D=   123******89
        '7  B/E=   123******89
        '8  C/D=   123*45**89
        '9  C/E=   123*45-*89
        '10 D/E=   123045**89
        Dim AChar As String = vbNullString
        Dim BChar As String = vbNullString
        Dim MatchedStringWithWildCards As String = vbNullString
        'determine how far off in aboslute position, to allow for a correlating letter
        Dim LetterPositionCorrelationSlop As Integer = CInt(Math.Ceiling(0.1 * Len(AString)))
        'bail if the strings don't correlate in length
        If Len(AString) > Len(BString) + 2 * LetterPositionCorrelationSlop OrElse Len(AString) < Len(BString) - 2 * LetterPositionCorrelationSlop Then Return MatchedStringWithWildCards
        Dim APosition As Integer = 0
        Dim BPosition As Integer = 1
        Dim BStart As Integer = 1
        Dim ListMatchWithWildCards As New List(Of String)
        Dim Conditional As String = "None"
        Do
            'find the first matched character that falls within the correlating position
            Dim MatchedChar As String = vbNullString
            APosition = ListMatchWithWildCards.Count + 1
            If APosition < 1 Then APosition = 1
            For IntA = APosition To Len(AString)
                AChar = Mid(AString, IntA, 1)
                Dim BStop As Integer = Math.Min(IntA + LetterPositionCorrelationSlop, Len(BString))
                For IntB = BStart To BStop
                    BChar = Mid(BString, IntB, 1)
                    If BChar = AChar Then
                        MatchedChar = BChar
                        Conditional = "None"
                    ElseIf BChar = "*" Then
                        MatchedChar = AChar
                        Conditional = "Bstring"
                    ElseIf AChar = "*" Then
                        MatchedChar = BChar
                        Conditional = "AString"
                    End If
                    If Not MatchedChar = vbNullString Then
                        APosition = IntA
                        BPosition = IntB
                        Exit For
                    End If
                Next
                If MatchedChar <> vbNullString Then
                    Exit For
                Else
                    ListMatchWithWildCards.Add("*")
                End If
            Next
            'bail if no match
            If MatchedChar = vbNullString Then
                For Each strEntry In ListMatchWithWildCards
                    MatchedStringWithWildCards &= strEntry
                Next
                Return MatchedStringWithWildCards
            End If
            'find all successive matched characters (if any)
            Dim FoundConsecMatch As Boolean = False
            Do
                APosition += 1
                BPosition += 1
                If BPosition > Len(BString) OrElse APosition > Len(AString) Then Exit Do
                AChar = Mid(AString, APosition, 1)
                BChar = Mid(BString, BPosition, 1)
                Dim SavChar As String = vbNullString
                Dim GetOut As Boolean = False
                If AChar = BChar Then
                    SavChar = AChar
                ElseIf AChar = "*" Then
                    Conditional = "AString"
                    SavChar = BChar
                ElseIf BChar = "*" Then
                    Conditional = "BString"
                    SavChar = AChar
                Else
                    If Conditional <> "None" Then ' if the previous character was conditional 
                        If Conditional = "AString" Then 'and we saved the BString side
                            If AChar = ListMatchWithWildCards(ListMatchWithWildCards.Count - 1) Then ' and the current AString character = the saved, BString side
                                ListMatchWithWildCards(ListMatchWithWildCards.Count - 1) = "*" 'then put a wild card in front of the last saved character
                                ListMatchWithWildCards.Add(AChar) ' displacing the character
                                BPosition -= 1
                            Else : GetOut = True
                            End If
                        ElseIf Conditional = "BString" Then
                            If BChar = ListMatchWithWildCards(ListMatchWithWildCards.Count - 1) Then
                                ListMatchWithWildCards(ListMatchWithWildCards.Count - 1) = "*"
                                ListMatchWithWildCards.Add(BChar)
                                APosition -= 1
                            Else : GetOut = True
                            End If
                        End If
                    Else : GetOut = True
                    End If
                    If GetOut Then
                        If Not FoundConsecMatch Then ListMatchWithWildCards.Add("*")
                        Exit Do
                    End If
                End If
                If SavChar <> vbNullString Then
                    FoundConsecMatch = True
                    If MatchedChar <> vbNullString Then
                        ListMatchWithWildCards.Add(MatchedChar)
                    End If
                    MatchedChar = vbNullString
                    ListMatchWithWildCards.Add(SavChar)
                    BStart = BPosition + 1
                End If
            Loop
            If BPosition > Len(BString) OrElse APosition > Len(AString) Then Exit Do
        Loop
        'Build String from list of characters
        For Each strEntry In ListMatchWithWildCards
            MatchedStringWithWildCards &= strEntry
        Next
        Return MatchedStringWithWildCards
    End Function
    Private Function DominantCompositeText(ByVal OCRstrings As List(Of String)) As String
        If OCRstrings.Count = 0 Then Return vbNullString
        'find the lengthiest string
        Dim MaxLen As Integer = 0
        For Each StringItem As Object In OCRstrings
            Dim Thisstring As String = StringItem.ToString
            MaxLen = Math.Max(MaxLen, Len(Thisstring))
        Next
        If MaxLen = 0 Then Return vbNullString

        Dim ThisChar As CharacterSet
        Dim FinalString As String = vbNullString

        'for each successive character 1 to max len
        For IntChar As Integer = 1 To MaxLen
            Dim SavChar As String = vbNullString
            Dim MaxIncident As Integer = 0
            For Each ThisString As String In OCRstrings
                'Get the first/next character
                If Len(ThisString) < IntChar Then Continue For
                ThisChar.CharValue = Mid(ThisString, IntChar, 1)
                'Get the dominant character for the alpha value of this character, IF you haven't already done so
                If ThisChar.CharValue = SavChar Then Continue For
                For Each Item As String In OCRstrings
                    If Len(Item) < IntChar Then Continue For
                    If Mid(Item, IntChar, 1) = ThisChar.CharValue Then ThisChar.CharInc += 1
                Next
                If ThisChar.CharInc > MaxIncident Then
                    MaxIncident = ThisChar.CharInc
                    SavChar = ThisChar.CharValue
                End If
            Next
            FinalString &= SavChar
        Next
        Return FinalString
    End Function
    Private Sub ShowAutomationPanelToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ShowAutomationPanelToolStripMenuItem.Click
        Scripting_Engine.Show()
        SetTheTable()
    End Sub

    Private Sub ShowIRRemoteControlToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ShowIRRemoteControlToolStripMenuItem.Click
        Dim FTPAvail As Boolean = GetDevices()
        LoadDevices()
        Form1.Show()
        SetTheTable()
    End Sub

    Private Sub ShowOutputScreenToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ShowOutputScreenToolStripMenuItem.Click
        Output.WindowState = FormWindowState.Normal
        Output.Show()
        SetTheTable()
    End Sub


    Private Sub ShowAutomationToolboxToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ShowAutomationToolboxToolStripMenuItem.Click
        'ToolBox.Show()
        PlaceToolbox()
        SetTheTable()

    End Sub

    Private Sub DeleteAllFiles(ByVal ThisDirectory As String)
        Try
            For Each foundfile As String In My.Computer.FileSystem.GetFiles(ThisDirectory)
                My.Computer.FileSystem.DeleteFile(foundfile)
            Next
            If File.Exists(MasterPath & "Settings.txt") Then File.Delete(MasterPath & "Settings.txt")
        Catch

        End Try

    End Sub

    Private Sub NewAddressToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles NewAddressToolStripMenuItem.Click
        Dim Title As String = "Address Change"
        Dim Prompt As String = "Are you sure you wish to change address?" & vbCrLf & "All settings will be lost"
        Dim GoAhead As Integer = MsgBox(Prompt, MsgBoxStyle.YesNo, Title)
        If GoAhead = MsgBoxResult.No Then Exit Sub
        'DeleteAllFiles(MasterPath & "CustomScript\")
        'DeleteAllFiles(MasterPath & "Commands\")
        'Try
        '    If Directory.Exists(DeviceDirectory) Then Directory.Delete(DeviceDirectory, True)
        'Catch ex As Exception

        'End Try
        AMC.Stop()
        AMC.MediaPassword = vbNullString
        Form1.Close()
        LabTownStreetAddress = ""
        WriteSettings()
        SignOn()
        Form1.Show()
        SetTheTable()
    End Sub

    Private Sub BugListToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles BugListToolStripMenuItem.Click
        BugList.Show()
    End Sub

    Private Sub TV_SizeChanged(sender As Object, e As System.EventArgs) Handles Me.SizeChanged
        If LTInitialized = True Then ShowYourself()
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        BWBusy = False
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            TalkToSPARTA("setstart", "192.168.1.8:8003")

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        ''Dim A As String = "123ab456def789ghi0j"
        ''Dim B As String = "123abc456def789ghl0j"

        ''A=123-456-789
        ''B=123-444-489
        ''C=123-45-789
        ''D=1230456089
        ''E=123045--89
        'Dim a As New List(Of String)
        'a.Add("123-456-789")
        'a.Add("123-444-489")
        'a.Add("123-45-789")
        'a.Add("1230456089")
        'a.Add("123045--89")
        'Dim Rez As String = DominantCompositeTextByPatternsWithWildCards(a)
    End Sub

    Public Sub TalkToSPARTA(ByVal Args As String, ByVal WhereTo As String)

        Try
            StartRemoteServices("http://" & WhereTo & "/adcomparator")
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

        Dim PassInteger As Integer
        If InStr(Args, "(") <> 0 Then
            Dim ArgsArr() As String = Split(Args, "(")
            Args = ArgsArr(0)
            PassInteger = CInt(Val(Replace(ArgsArr(1), ")", "")))
        End If

        Try
            Select Case Args
                Case "getcount"
                    Dim i As Integer = _serviceAdComparator.GetCount
                    Scripting_Engine.txtCommand.Text = i.ToString
                Case "getstatus"
                    Scripting_Engine.txtCommand.Text = _serviceAdComparator.GetStatus.ToString
                Case "setstart"
                    _serviceAdComparator.SetStart(PassInteger)
                    Scripting_Engine.txtCommand.Text = Args & "_OK"
                Case "setstop"
                    _serviceAdComparator.SetStop(1)
                    Scripting_Engine.txtCommand.Text = Args & "_OK"
            End Select
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

#Region "Service Methods"
#Region "Declarations"
    Private _serviceAdComparator As AdComparatorClient
#End Region

#Region "Private Methods"
    Public Sub StartRemoteServices(ByVal MyHost As String)
        Try
            'If _serviceAdComparator.State <> CommunicationState.Closed Then StopRemoteServices()
            _serviceAdComparator = New AdComparatorClient(New BasicHttpBinding, New EndpointAddress(MyHost))
            _serviceAdComparator.Open()
            'MessageBox.Show(String.Format("AdExam.Status = {0}`", _serviceAdComparator.GetStatus))
        Catch ex As CommunicationException
            Debug.WriteLine(ex.Message)
        End Try
    End Sub

    Public Sub StopRemoteServices()
        Try
            If _serviceAdComparator IsNot Nothing Then
                _serviceAdComparator.Close()
                Do
                    Application.DoEvents()
                Loop Until _serviceAdComparator.State = CommunicationState.Closed
            End If
        Catch ex As CommunicationException
            Debug.WriteLine(ex.Message)
        End Try
    End Sub
#End Region
#End Region

End Class