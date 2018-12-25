Option Strict Off
Option Explicit On
Imports MSScriptControl
Imports System.IO
Imports System.Threading
Imports System.Net.Sockets
Imports System.Text

Public Class Scripting_Engine
    'PUBLIC SCENE CHECK VARIABLES PASSED TO/FROM MSSCRIPTCONTROL
    Private Structure CommandType
        Dim cmdName As String
        Dim cmdFunction As String
        Dim cmdExample As String
    End Structure

    Private Structure AutomationType
        Dim autoName As String
        Dim autoFunction As String
        Dim autoExample As String
    End Structure

    Private Structure TCPSendParams
        'SktAddress, SktPort, SktParameters, SktTimeOut, SktEO
        Dim TCPAddress As String
        Dim TCPPort As Integer
        Dim TCPParams As String
        Dim TCPTimeOut As Decimal
        Dim TCPEOF As String
        Dim TCPThreaded As Boolean
    End Structure

    Private Structure ArgumentsIndexed
        Dim Argument As String
        Dim Occurence As Integer
    End Structure
    Private WithEvents CommsEngine As New AsynchronousClient

    Private TCPInstance As TCPSendParams
    Private myscript As New ScriptControl
    Private CurrentRTBLine As Decimal
    Private lstCustomCommands As New List(Of CommandType)
    Private lstCustomAutomation As New List(Of AutomationType)
    Private lstVBCommands As New List(Of String)
    Private lstWordDelims As New List(Of String)
    Private ScriptDisplayMode As Boolean
    Private SocketThrd As Thread
    Private TCPResult As String
    Public PWRServerCMD As String

    Private Delegate Sub SetResponseText(Data As String)

    Private Function Message(txt As String) As Boolean
        MsgBox(txt)
        Return True
    End Function

    Private Function GetCode(ByVal FilName As String) As String
        FilName = MasterPath & FilName
        Dim Contents As String = vbNullString
        If File.Exists(FilName) Then
            Dim FilReader As New StreamReader(FilName)
            Contents = FilReader.ReadToEnd()
        End If
        Return Contents
    End Function

    Private Sub Scripting_Engine_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        rtbCode.Width = Me.Width - 20
        rtbCode.Height = Me.Height - 80
        txtCommand.Width = Me.Width - txtCommand.Left - 20
    End Sub

    Public Sub txtCommand_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCommand.TextChanged
        'If Not ActiveRun Then Exit Sub - DON'T DO THIS... IT CAUSES LT TO HANG AFTER A ROUTINE
        Dim LeftPart As String = vbNullString
        Try
            'Automation Directory
            If InStr(LCase(txtCommand.Text), "hideme") <> 0 AndAlso LCase(txtCommand.Text) <> "(unhideme)" Then
                ScriptDisplayMode = False
                LeftPart = Replace(LCase(txtCommand.Text), "hideme", "")
                LeftPart = Replace(LeftPart, "(_scdirectory=", "")
                LeftPart = Replace(LeftPart, ")", "")
                AutomationSCDirectory = ActiveAutomationDirectory & LeftPart & "\"
                Exit Sub
            ElseIf LCase(txtCommand.Text) = "(unhideme)" Then
                ScriptDisplayMode = True
                Exit Sub
            End If

            If Not AppTools.ActiveRun Then
                txtCommand.Text = ""
                Exit Sub
            End If
            If txtCommand.Text = vbNullString OrElse txtCommand.Text = "" Then Exit Sub
            If InStr(txtCommand.Text, "(") = 0 Then Exit Sub
            If ScriptDisplayMode = True Then HighlightScript()
            Dim Parenthesis As Integer = InStr(txtCommand.Text, "(")
            LeftPart = Mid(txtCommand.Text, 1, Parenthesis - 1)
        Catch ex As Exception

        End Try

        Application.DoEvents()

        Select Case LCase(LeftPart)
            Case "acpower"
                Parse_Power()
            Case "countdown"
                Parse_CountDown()
            Case "devicetype"
                Parse_DeviceType()
            Case "killfile"
                Parse_KillFile()
            Case "ircommand"
                ParseIR()
            Case "ocrcheck"
                Parse_PerformOCR()
            Case "pause"
                Parse_Delay()
            Case "runscript"
                Parse_RunScript()
            Case "scenecheck"
                Parse_PerformSC()
            Case "screenout"
                Parse_Output()
            Case "selectdevices"
                Parse_SelectDevices()
            Case "snapshot"
                Parse_SnapShot()
            Case "sparta_http"
                Parse_SPARTA()
            Case "startrecord"
                Parse_Record()
            Case "stoprecord"
                Parse_StopRecord()
            Case "tcpsend"
                Parse_TCP()
            Case "unhideme"
                ScriptDisplayMode = True
            Case "writelog"
                Parse_WriteLog()
        End Select
    End Sub

    Private Sub Parse_SPARTA()
        'SPARTA_HTTP("-ARGS GetCount, -IP 108.20.221.234:1122")
        Dim Communication As String = vbNullString
        Dim Addr As String = vbNullString
        lblCurrentCommand.Text = txtCommand.Text
        Dim SPARTA_Params As String = Replace(LCase(txtCommand.Text), "sparta_http(", "")
        SPARTA_Params = Replace(SPARTA_Params, ")", "")
        Dim SPARTA_Arr() As String = Split(SPARTA_Params, ",")
        Communication = LCase(Trim(SPARTA_Arr(0)))
        Addr = LCase(Trim(SPARTA_Arr(1)))

        TV.TalkToSPARTA(Communication, Addr)
    End Sub

    Private Sub Parse_RunScript()
        Try
            Dim strVariable As String = Replace(LCase(txtCommand.Text), "runscript(", "")
            strVariable = Replace(strVariable, ")", "")
            If File.Exists(strVariable) Then Process.Start(strVariable)
        Catch ex As Exception

        End Try
        txtCommand.Text = "Done"
    End Sub

    Private Sub Parse_WriteLog()
        Try
            Dim strVariable As String = Replace(LCase(txtCommand.Text), "writelog(", "")
            strVariable = Replace(strVariable, ")", "")

            Dim OWrite As Boolean = False
            If Mid(UCase(strVariable), 1, 2) = "O~" Then
                OWrite = True
                strVariable = Mid(strVariable, 3, Len(strVariable) - 2)
            End If

            Dim StrText As String = Output.txtOutput.Text
            Dim strArr() As String = Split(strVariable, "|")
            Dim strFile As String = strArr(0)
            If strArr.Count > 1 Then StrText = strArr(1)

            If File.Exists(strFile) AndAlso Not OWrite Then StrText = vbCrLf & StrText
            If Not OWrite = True Then
                File.AppendAllText(strFile, StrText)
            Else
                File.WriteAllText(strFile, StrText)
            End If

        Catch ex As Exception

        End Try
        txtCommand.Text = "Done"
    End Sub
    Private Sub Parse_KillFile()
        Try
            Dim FileName As String = Replace(LCase(txtCommand.Text), "killfile(", "")
            FileName = Replace(FileName, ")", "")
            If File.Exists(FileName) Then File.Delete(FileName)
        Catch ex As Exception
            txtCommand.Text = "Fail"
        End Try
        txtCommand.Text = "Success"
    End Sub

    Private Sub Parse_CountDown()
        lblCurrentCommand.Text = txtCommand.Text
        Dim RealEstate As Rectangle = Screen.PrimaryScreen.Bounds
        CountDown.Top = CInt(RealEstate.Height / 2)
        CountDown.Left = CInt(RealEstate.Width / 2)
        CountDown.TopMost = True
        CountDown.Text = LabTownStreetAddress
        CountDown.Show()
        Dim strMinutes As String = Replace(LCase(txtCommand.Text), "countdown(", "")
        strMinutes = Replace(strMinutes, ")", "")
        Dim Seconds As Integer = CInt(CDec(Val(strMinutes)) * 60)
        Dim RequestedDelay As Integer = Seconds

        'Dim SecondsElapsed As Integer
        Dim CountDownTime As New Stopwatch
        CountDownTime.Start()
        Dim RemainingSeconds As Integer = Seconds
        For ElapsedSeconds = 1 To Seconds + 1
            If Not AppTools.ActiveRun Then Exit For
            Dim MinutesPart As Integer = Math.Truncate(RemainingSeconds / 60)
            Dim SecondsPart As Integer = RemainingSeconds - (MinutesPart * 60)
            strMinutes = MinutesPart.ToString
            If Len(strMinutes) < 2 Then strMinutes = "0" & strMinutes
            Dim strSeconds As String = SecondsPart.ToString
            If Len(strSeconds) < 2 Then strSeconds = "0" & strSeconds
            Dim strTimeLeft As String = strMinutes & ":" & strSeconds
            CountDown.txtCountDown.Text = strTimeLeft
            Do
                Application.DoEvents()
                Thread.Sleep(50)
            Loop Until CountDownTime.ElapsedMilliseconds >= ElapsedSeconds * 1000
            RemainingSeconds -= 1

            If Not CountDown.CanFocus Then Exit For
        Next

        CountDownTime.Stop()

        txtCommand.Text = ""
        CountDown.Close()
    End Sub
    Private Sub Parse_DeviceType()
        txtCommand.Text = ActiveDevice.DeviceType
        Application.DoEvents()
    End Sub
    Private Sub Parse_TCP()
        lblCurrentCommand.Text = txtCommand.Text
        Dim SocketCommand As String = Replace(txtCommand.Text, "tcpsend(", "")

 

        For IntN = 1 To 7
            Dim ReplaceText As String = vbNullString
            Select Case IntN
                Case 1
                    ReplaceText = "Tcpsend("
                Case 2
                    ReplaceText = "TCpsend("
                Case 3
                    ReplaceText = "TCPsend("
                Case 4
                    ReplaceText = "TCPSend("
                Case 5
                    ReplaceText = "TCPSEnd("
                Case 6
                    ReplaceText = "TCPSENd("
                Case 7
                    ReplaceText = "TCPSEND("
            End Select
            If InStr(SocketCommand, ReplaceText) <> 0 Then SocketCommand = Replace(txtCommand.Text, ReplaceText, "")
        Next
        SocketCommand = Replace(SocketCommand, ")", "")

        'Load Arguments 
        Dim Arguments() As String = {"-ARGS ", "-IP ", "-TO ", "-EOF ", "-ASYN "}
        'Argument Occurences
        Dim ArgumentsByOccurrence As New List(Of ArgumentsIndexed)
        For ArgIndex As Integer = 0 To UBound(Arguments)
            Dim ThisArgument As ArgumentsIndexed
            ThisArgument.Argument = Arguments(ArgIndex)
            ThisArgument.Occurence = InStr(UCase(SocketCommand), ThisArgument.Argument)
            ArgumentsByOccurrence.Add(ThisArgument)
        Next
        'Sort the Argument List by Occurences
        Dim SortedArguments As New List(Of ArgumentsIndexed)
        Dim Min As Integer = Len(SocketCommand)
        Dim PrevMin As Integer = 0
        For IntN = 1 To Arguments.Count
            'find the lowest occuring argument 
            Dim ThisArgument As ArgumentsIndexed = Nothing
            For ArgIndex As Integer = 0 To ArgumentsByOccurrence.Count - 1
                Dim Occurence As Integer = ArgumentsByOccurrence(ArgIndex).Occurence
                If Occurence > PrevMin AndAlso Occurence < Min Then
                    Min = Occurence
                    ThisArgument = ArgumentsByOccurrence(ArgIndex)
                End If
            Next
            PrevMin = Min
            Min = Len(SocketCommand)
            If ThisArgument.Occurence <> 0 Then SortedArguments.Add(ThisArgument)
        Next

        With TCPInstance
            .TCPParams = ""
            .TCPTimeOut = 0
            .TCPEOF = ""
            For ArgIndex As Integer = 0 To SortedArguments.Count - 1
                Dim ArgPlacement As Integer = SortedArguments(ArgIndex).Occurence
                Dim ArgLength As Integer
                If ArgIndex < SortedArguments.Count - 1 Then
                    ArgLength = SortedArguments(ArgIndex + 1).Occurence - ArgPlacement
                Else
                    ArgLength = Len(SocketCommand) - ArgPlacement + 1
                End If

                Dim ThisArgument As String = Mid(SocketCommand, ArgPlacement, ArgLength)
                Dim ArgumentType As String = SortedArguments(ArgIndex).Argument
                ThisArgument = Trim(Mid(ThisArgument, Len(ArgumentType) + 1, Len(ThisArgument) - Len(ArgumentType)))
                Select Case ArgumentType
                    Case "-ARGS "
                        .TCPParams = ThisArgument
                    Case "-IP "
                        Dim ParamArr() As String = Split(ThisArgument, ":")
                        .TCPAddress = ParamArr(0)
                        .TCPPort = CInt(ParamArr(1))
                    Case "-TO "
                        .TCPTimeOut = CDec(ThisArgument)
                    Case "-EOF "
                        .TCPEOF = ThisArgument
                    Case "-ASYN "
                        .TCPThreaded = CBool(ThisArgument)
                End Select
            Next

            cboxInbox.Checked = False
            TCPSendTimeout = False

            If .TCPThreaded = False Then
                CommsEngine.Main(.TCPAddress, .TCPPort, .TCPParams, .TCPTimeOut, .TCPEOF)
                Application.DoEvents()
                txtCommand.Text = ""
            Else
                SocketThrd = New Thread(AddressOf ThreadTask)
                SocketThrd.IsBackground = True
                SocketThrd.Start()
            End If
        End With
        Application.DoEvents()
    End Sub
    Private Sub CommsEngine_MessageReceived(Data As String) Handles CommsEngine.MessageReceived
        'txtResponses.text = Data
        If txtResponses.InvokeRequired Then
            txtResponses.Invoke(New SetResponseText(AddressOf CommsEngine_MessageReceived), Data)
        Else
            txtResponses.Text = Data
        End If
    End Sub


    Private Sub ThreadTask()
        With TCPInstance
            CommsEngine.Main(.TCPAddress, .TCPPort, .TCPParams, .TCPTimeOut, .TCPEOF)
        End With
    End Sub

    Private Sub Parse_Record()
        lblCurrentCommand.Text = txtCommand.Text
        Dim CapturePath As String = Replace(LCase(txtCommand.Text), "startrecord(", "")
        CapturePath = Replace(CapturePath, ")", "")
        Select Case ActiveDevice.VideoServerType
            Case "AXIS_M7001"
                TV.AMC.StartRecordMedia(CapturePath, 8, "h264") ' 8 for video only RTP/RTSP
            Case "AXIS_243SA"
                TV.AMC.StartRecordMedia(CapturePath, 16, "mpeg4")
            Case "AXIS_P7216"
                TV.AMC.StartRecordMedia(CapturePath, 16, "h264") ' 16 for video and audio RTP/RTSP
            Case "BLACKMAGICPRO_H264"
        End Select
        txtCommand.Text = ""
    End Sub

    Private Sub Parse_StopRecord()
        lblCurrentCommand.Text = txtCommand.Text
        TV.AMC.StopRecordMedia()
        txtCommand.Text = ""
    End Sub

    Private Sub Parse_Power()
        lblCurrentCommand.Text = txtCommand.Text
        Dim ONOFF As String = Replace(LCase(txtCommand.Text), "acpower(", "")
        ONOFF = Replace(ONOFF, ")", "") & " "
        PWRServerCMD = ONOFF
        Timer2.Enabled = True 'Form1.ACServerTransactionByBatch(ONOFF & ActiveDevice.PwrServerPort, ActiveDevice.PwrServerIP)
        Delay(8000)
        txtCommand.Text = ""
        Cursor = Cursors.Default
    End Sub

    Private Sub RemoveBold()
        rtbCode.SelectionStart = 0
        rtbCode.SelectionLength = rtbCode.Text.Length
        rtbCode.SelectionFont = rtbCode.Font
        rtbCode.SelectionBackColor = Color.White
        rtbCode.Refresh()
        Application.DoEvents()
    End Sub

    Private Sub RemoveColor()
        rtbCode.SelectionStart = 0
        rtbCode.SelectionLength = rtbCode.Text.Length
        rtbCode.SelectionColor = Color.Black
        rtbCode.Refresh()
        Application.DoEvents()
    End Sub

    Private Sub HighlightScript()
        'First Remove all highlighting
        RemoveBold()
        Dim FoundLine As Boolean = False
        Dim Tried As Boolean = False
        'make the currentline bold
        If CurrentRTBLine = 0 Then CurrentRTBLine -= 1
        Dim SearchString As String = Trim(LCase(txtCommand.Text))
  
        For IntN As Integer = 1 To 2
            If IntN = 2 Then
                Dim ParenthesisDelim As Integer = InStr(SearchString, "(")
                If ParenthesisDelim = 0 Then Exit For
                SearchString = Mid(SearchString, 1, ParenthesisDelim)
            End If
            Do
                For LineNo = Math.Floor(CurrentRTBLine) + 1 To rtbCode.Lines.Count - 1
                    rtbCode.SelectionStart = rtbCode.GetFirstCharIndexFromLine(LineNo)
                    rtbCode.SelectionLength = rtbCode.Lines(LineNo).Length
                    Dim LookInString As String = Trim(LCase(Replace(rtbCode.SelectedText, Chr(34), "")))
                    If InStr(LookInString, SearchString) > 0 Then
                        CurrentRTBLine = LineNo
                        If CurrentRTBLine = 0 Then CurrentRTBLine = 0.9
                        rtbCode.SelectionFont = New Font(rtbCode.SelectionFont, FontStyle.Bold)
                        rtbCode.SelectionBackColor = Color.LightGray
                        Application.DoEvents()
                        FoundLine = True
                        Exit For
                    End If
                Next
                If FoundLine = False Then CurrentRTBLine = -1
                If Tried Then Exit Do
                Tried = True
            Loop Until FoundLine = True
            If FoundLine Then Exit For
        Next
    End Sub

    'Private Sub Parse_SelectDevice()
    '    lblCurrentCommand.Text = txtCommand.Text
    '    Dim Devicename As String = Replace(LCase(txtCommand.Text), "selectdevice(", "")
    '    Devicename = Replace(Devicename, ")", "")
    '    Form1.cboDevice.Text = Devicename
    '    txtCommand.Text = ""
    'End Sub

    Private Sub Parse_SelectDevices()
        lblCurrentCommand.Text = txtCommand.Text
        Dim Devices As String = Replace(LCase(txtCommand.Text), "selectdevices(", "")
        Devices = Replace(Devices, ")", "")
        SimulStartDevice = CInt(Val(Devices))
        SimulStopDevice = SimulStartDevice
        Dim DevArr() As String = Nothing
        If InStr(Devices, ",") <> 0 Then
            DevArr = Split(Devices, ",")
            SimulStartDevice = DevArr(0)
            SimulStopDevice = DevArr(1)
        End If
        Form1.cboDevice.SelectedIndex = SimulStartDevice - 1
        If Not DevArr Is Nothing Then
            If DevArr.Count > 1 Then
                SimulStartDevice = DevArr(0)
                SimulStopDevice = DevArr(1)
            End If
        End If
        BuildDeviceLists()


        txtCommand.Text = ""

    End Sub

    Public Sub BuildDeviceLists(Optional ByVal OneButtonOnly As Boolean = False)
        lst1Devices.Clear()
        lst2Devices.Clear()
        lst3Devices.Clear()
        lst4Devices.Clear()
        lst5Devices.Clear()
        lst6Devices.Clear()

        For Each ThisDevice As IRDevice In lstDevices
            If ThisDevice.Index < SimulStartDevice OrElse ThisDevice.Index > SimulStopDevice Then Continue For
            Select Case ThisDevice.IRServerPort
                Case 1
                    lst1Devices.Add(ThisDevice)
                Case 2
                    lst2Devices.Add(ThisDevice)
                Case 3
                    lst3Devices.Add(ThisDevice)
                Case 4
                    lst4Devices.Add(ThisDevice)
                Case 5
                    lst5Devices.Add(ThisDevice)
                Case 6
                    lst6Devices.Add(ThisDevice)
            End Select
        Next
    End Sub


    Private Sub Parse_SnapShot()
        lblCurrentCommand.Text = txtCommand.Text
        Dim CapturePath As String = Replace(LCase(txtCommand.Text), "snapshot(", "")
        CapturePath = Replace(CapturePath, ")", "")
        TV.AMC.SaveCurrentImage(0, CapturePath)
        txtCommand.Text = ""
    End Sub

    Private Sub Parse_Delay()
        lblCurrentCommand.Text = txtCommand.Text
        Application.DoEvents()
        Dim strDelay As String = Replace(txtCommand.Text, "Pause(", "")
        strDelay = Replace(strDelay, ")", "")
        Dim intDelay As Integer = CInt(Val(strDelay))
        If intDelay < 50 Then intDelay = 50
        Dim intIncrements As Integer = CInt(intDelay / 50)
        For IntN As Integer = 1 To intIncrements
            Thread.Sleep(50)
            Application.DoEvents()
        Next
        txtCommand.Text = vbNullString
    End Sub
    Private Sub Parse_Output()
        lblCurrentCommand.Text = txtCommand.Text
        Application.DoEvents()
        Output.Show()
        If LCase(txtCommand.Text) = "screenout(clear)" OrElse LCase(txtCommand.Text) = "screenout(clr)" Then
            Output.txtOutput.Text = vbNullString
        Else
            Dim Parenthesis As Integer = InStr(txtCommand.Text, "(")
            Dim RightPart As String = Mid(txtCommand.Text, Parenthesis + 1, Len(txtCommand.Text) - Parenthesis - 1)
            Dim SpaceDelim As Integer = InStr(RightPart, " ")
            If SpaceDelim > 0 Then
                Dim FunctionalSpec As String = Mid(RightPart, 1, SpaceDelim - 1)
                Select Case LCase(FunctionalSpec)
                    Case Is = "appendline"
                        RightPart = Mid(RightPart, 12, Len(RightPart) - 11)
                        Output.txtOutput.Text &= RightPart
                    Case Is = "clr"
                        RightPart = Mid(RightPart, 5, Len(RightPart) - 4)
                        Output.txtOutput.Text = RightPart
                    Case Else
                        If Output.txtOutput.Text <> vbNullString Then
                            Output.txtOutput.Text &= vbCrLf & RightPart
                        Else : Output.txtOutput.Text &= RightPart
                        End If
                End Select
            Else
                If Output.txtOutput.Text <> vbNullString Then
                    Output.txtOutput.Text &= vbCrLf & RightPart
                Else : Output.txtOutput.Text &= RightPart
                End If
            End If

            Output.WindowState = FormWindowState.Normal
        End If
        Application.DoEvents()
        txtCommand.Text = vbNullString
    End Sub
    Private Sub ParseIR()
        Dim XMitPeriod As Integer = CInt(1000 / IRFrequency)
        Dim StartTime As New Stopwatch
        Dim LongForm As String = Replace(LCase(txtCommand.Text), "ircommand(", "")
        LongForm = Replace(LongForm, ")", "")
        Dim SCArr() As String = Split(LongForm, ",")
        For IntN As Integer = 0 To SCArr.Count - 1
            If Not AppTools.ActiveRun Then Exit For
            Dim ThisIR As String = Trim(LCase(SCArr(IntN)))
            lblCurrentCommand.Text = ThisIR
            Application.DoEvents()
            If Mid(ThisIR, 1, 6) = "delay." Then
                Dim SleepTime As Integer = CInt(Val(Replace(ThisIR, "delay.", "")))
                If SleepTime > 0 AndAlso SleepTime < 10000 Then Threading.Thread.Sleep(SleepTime)
            Else
                StartTime.Start()
                Form1.RemoteButtonPress(ThisIR)
                Do
                    Application.DoEvents()
                Loop Until StartTime.ElapsedMilliseconds >= XMitPeriod
                StartTime.Stop()
                StartTime.Reset()
            End If
            If Not ActiveRun Then Exit For
        Next
        txtCommand.Text = vbNullString
    End Sub

    Private Sub Parse_PerformSC()
        If Not Directory.Exists(SCFramesDirectory) Then Directory.CreateDirectory(SCFramesDirectory)
        lblCurrentCommand.Text = Replace(txtCommand.Text, "*_", "")
        Application.DoEvents()
        Dim LongForm As String = Replace(LCase(txtCommand.Text), "scenecheck(", "")
        LongForm = Replace(LongForm, ")", "")
        Dim SCArr() As String = Split(LongForm, ",")
        Dim ThisSCName As String = SCArr(0)
        If SCArr.Count < 2 Then Exit Sub

        ActiveSCName = ThisSCName
        SCDirectory = MasterPath & "SceneChecks\"
        If InStr(ActiveSCName, "*_") <> 0 Then
            SCDirectory = AutomationSCDirectory
            ActiveSCName = Replace(LCase(ActiveSCName), "*_", "")
            ThisSCName = ActiveSCName
        End If

        Dim ThisSCDuration As Double = CDbl(Val(SCArr(1)))
        If File.Exists(SCDirectory & ThisSCName & "\scfile.txt") AndAlso File.Exists(SCDirectory & ThisSCName & "\SCBmp.bmp") Then
            Dim SceneTop As Integer
            Dim SceneLeft As Integer
            Dim SceneWidth As Integer
            Dim SceneHeight As Integer
            Dim SceneType As String = vbNullString
            Dim sr As New StreamReader(SCDirectory & ThisSCName & "\scfile.txt")
            Do While sr.Peek() >= 0
                Dim ThisLine() As String = Split(sr.ReadLine(), ":")
                Select Case LCase(ThisLine(0))
                    Case "top"
                        SceneTop = CInt(Val(ThisLine(1)))
                    Case "left"
                        SceneLeft = CInt(Val(ThisLine(1)))
                    Case "width"
                        SceneWidth = CInt(Val(ThisLine(1)))
                    Case "height"
                        SceneHeight = CInt(Val(ThisLine(1)))
                    Case "orig resolution"
                    Case "type"
                        SceneType = Trim(ThisLine(1))
                    Case "text"
                    Case Else
                End Select
            Loop
            sr.Close()
            Dim SceneSensitivity As Decimal = 1
            If SCArr.Count > 2 Then SceneSensitivity = CDec(Val(SCArr(2)))
            ExecuteSceneCheck(ThisSCName, ThisSCDuration, SceneTop, SceneLeft, SceneWidth, SceneHeight, SceneType, SceneSensitivity)
        Else
            MsgBox("couldn't locate SceneCheck file")
            txtCommand.Text = "SC Failed"
        End If
    End Sub
    Private Sub Parse_PerformOCR()
        If Not Directory.Exists(SCFramesDirectory) Then Directory.CreateDirectory(SCFramesDirectory)
        'remove Automation Directory header if any
        lblCurrentCommand.Text = Replace(txtCommand.Text, "*_", "")
        Application.DoEvents()
        'remove boiler plate
        Dim LongForm As String = Replace(LCase(txtCommand.Text), "ocrcheck(", "")
        LongForm = Replace(LongForm, ")", "")
        'separate variables
        Dim SCArr() As String = Split(LongForm, ",")
        'set SC name
        Dim ThisSCName As String = SCArr(0)
        'set duration
        Dim ThisSCDuration As Double = 0.03
        If SCArr.Count > 1 Then ThisSCDuration = CDbl(Val(SCArr(1)))
        'set search text
        OCRSearchText = "anytext"
        If SCArr.Count > 2 Then OCRSearchText = Trim(SCArr(2))
        'set SC directory - Standard/Automation
        ActiveSCName = ThisSCName
        SCDirectory = MasterPath & "SceneChecks\"
        If InStr(ActiveSCName, "*_") <> 0 Then
            SCDirectory = AutomationSCDirectory
            ActiveSCName = Replace(LCase(ActiveSCName), "*_", "")
            ThisSCName = ActiveSCName
        End If

        Dim SceneTop As Integer
        Dim SceneLeft As Integer
        Dim SceneWidth As Integer
        Dim SceneHeight As Integer
        Dim SceneType As String = vbNullString
        Dim sr As New StreamReader(SCDirectory & ThisSCName & "\scfile.txt")
        Do While sr.Peek() >= 0
            Dim ThisLine() As String = Split(sr.ReadLine(), ":")
            Select Case LCase(ThisLine(0))
                Case "top"
                    SceneTop = CInt(Val(ThisLine(1)))
                Case "left"
                    SceneLeft = CInt(Val(ThisLine(1)))
                Case "width"
                    SceneWidth = CInt(Val(ThisLine(1)))
                Case "height"
                    SceneHeight = CInt(Val(ThisLine(1)))
                Case "orig resolution"
                Case "type"
                    SceneType = Trim(ThisLine(1))
                Case "text"
                Case Else
            End Select
        Loop
        sr.Close()

        ExecuteOCR(ThisSCName, ThisSCDuration, SceneTop, SceneLeft, SceneWidth, SceneHeight, SceneType)

    End Sub
    Private Sub SetTabs()
        rtbCode.SelectionTabs = New Integer() {20, 40, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300}
    End Sub
    Private Sub Scripting_Engine_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.ShowInTaskbar = False
        'NEED TO ADDCODE FOR ALL FUNCTIONS: SCENECHECKS, DEVICESELECTIONS, IRCOMMANDS, ETC
        'CAN THEY BE ACTUAL FUNCTIONS OR MUST THEY BE SUBROUTINES ONLY??
        Dim StartString As String = "Sub ActiveSub()" & vbCrLf & vbCrLf & "End Sub"
        SetTabs()
        rtbCode.Text = StartString

        ConfigureScriptingEngine()
        LoadVBCommands()
        LoadWordDelimiters()
        'LoadALLAutomation()

    End Sub

    Public Sub LoadALLAutomation()
        If Not Directory.Exists(MasterPath & "Automation/") Then Directory.CreateDirectory(MasterPath & "Automation/")
        Dim AutomationDirectories As New List(Of String)
        Dim AutomationDirectoriesPath As String = FTPAddress & "Canned_Automation/"
        AutomationDirectories = AppTools.FTPDirectoryList(AutomationDirectoriesPath)
        If AutomationDirectories Is Nothing Then Exit Sub
        'GET THE VERSION DIRECTORIES
        For Each AutoDirect As String In AutomationDirectories
            Dim NewAutoDirectory As String = MasterPath & "Automation\" & AutoDirect
            If Not Directory.Exists(NewAutoDirectory) Then Directory.CreateDirectory(NewAutoDirectory)
            'GET THE ROUTINE DIRECTORIES
            Dim RoutineDirectories As New List(Of String)
            Dim RoutineDirectoriesPath As String = AutomationDirectoriesPath & AutoDirect & "/"
            RoutineDirectories = AppTools.FTPDirectoryList(RoutineDirectoriesPath)
            For Each RoutDirect As String In RoutineDirectories
                Dim NewRoutDirectory As String = NewAutoDirectory & "\" & RoutDirect
                If Not Directory.Exists(NewRoutDirectory) Then Directory.CreateDirectory(NewRoutDirectory)
                'GET THE ROUTINE TEXT FILE
                Dim SourceRoutinePath As String = Replace(RoutineDirectoriesPath, FTPAddress, "") & RoutDirect & "/" & RoutDirect & ".rtn"
                Dim DestinationRoutinePath As String = NewRoutDirectory & "\" & RoutDirect & ".rtn"
                AppTools.FTPFile(SourceRoutinePath, DestinationRoutinePath)

                'GET THE SCENECHECK DIRECTORIES
                Dim SceneCheckDirectories As New List(Of String)
                Dim SceneCheckDirectoriesPath As String = RoutineDirectoriesPath & RoutDirect & "/SceneChecks/"
                SceneCheckDirectories = AppTools.FTPDirectoryList(SceneCheckDirectoriesPath)
                For Each SCDirect As String In SceneCheckDirectories
                    Dim NewSCDirectory As String = NewRoutDirectory & "\" & SCDirect
                    If Not Directory.Exists(NewSCDirectory) Then Directory.CreateDirectory(NewSCDirectory)
                    'GET THE SCENECHECK FILES
                    Dim SourceSCPath As String = Replace(SceneCheckDirectoriesPath, FTPAddress, "") & SCDirect & "/SCBmp.bmp"
                    Dim DestinationSCPath As String = NewSCDirectory & "\SCBmp.bmp"
                    AppTools.FTPFile(SourceSCPath, DestinationSCPath)
                    SourceSCPath = Replace(SceneCheckDirectoriesPath, FTPAddress, "") & SCDirect & "/SCFile.txt"
                    DestinationSCPath = NewSCDirectory & "\SCFile.txt"
                    AppTools.FTPFile(SourceSCPath, DestinationSCPath)

                Next
            Next
        Next

    End Sub

    Private Sub LoadWordDelimiters()
        If lstWordDelims Is Nothing Then
        Else : lstWordDelims.Clear()
        End If
        lstWordDelims.Add(" ")
        lstWordDelims.Add(vbTab)
        lstWordDelims.Add(vbCr)
        lstWordDelims.Add(vbLf)
        lstWordDelims.Add(vbCrLf)
        lstWordDelims.Add(":")
        lstWordDelims.Add("(")
        lstWordDelims.Add("{")
        lstWordDelims.Add("'")
        lstWordDelims.Add(vbNullChar)
        lstWordDelims.Add(vbNullString)
        lstWordDelims.Add(vbNewLine)
    End Sub

    Private Sub LoadVBCommands()
        If lstVBCommands Is Nothing Then
        Else : lstVBCommands.Clear()
        End If
        'DATE/TIME FUNCTIONS
        lstVBCommands.Add("CDate")
        lstVBCommands.Add("Date")
        lstVBCommands.Add("DateAdd")
        lstVBCommands.Add("DateDiff")
        lstVBCommands.Add("DatePart")
        lstVBCommands.Add("DateSerial")
        lstVBCommands.Add("DateValue")
        lstVBCommands.Add("Day")
        lstVBCommands.Add("FormatDateTime")
        lstVBCommands.Add("Hour")
        lstVBCommands.Add("IsDate")
        lstVBCommands.Add("Minute")
        lstVBCommands.Add("Month")
        lstVBCommands.Add("MonthName")
        lstVBCommands.Add("Now")
        lstVBCommands.Add("Second")
        lstVBCommands.Add("Time")
        lstVBCommands.Add("Timer")
        lstVBCommands.Add("TimeSerial")
        lstVBCommands.Add("TimeValue")
        lstVBCommands.Add("WeekDay")
        lstVBCommands.Add("WeekdayName")
        lstVBCommands.Add("Year")
        'CONVERSIONS
        lstVBCommands.Add("Asc")
        lstVBCommands.Add("CBool")
        lstVBCommands.Add("CByte")
        lstVBCommands.Add("CCur")
        lstVBCommands.Add("CDate")
        lstVBCommands.Add("CDbl")
        lstVBCommands.Add("Chr")
        lstVBCommands.Add("CInt")
        lstVBCommands.Add("CLng")
        lstVBCommands.Add("CSng")
        lstVBCommands.Add("CStr")
        lstVBCommands.Add("Hex")
        lstVBCommands.Add("Oct")
        'FORMAT FUNCTIONS
        lstVBCommands.Add("FormatCurrency")
        lstVBCommands.Add("FormatDateTime")
        lstVBCommands.Add("FormatNumber")
        lstVBCommands.Add("FormatPercent")
        'MATH FUNCTIONS
        lstVBCommands.Add("Abs")
        lstVBCommands.Add("Atn")
        lstVBCommands.Add("Cos")
        lstVBCommands.Add("Exp")
        lstVBCommands.Add("Hex")
        lstVBCommands.Add("Int")
        lstVBCommands.Add("Fix")
        lstVBCommands.Add("Log")
        lstVBCommands.Add("Pct")
        lstVBCommands.Add("Rnd")
        lstVBCommands.Add("Sgn")
        lstVBCommands.Add("Sin")
        lstVBCommands.Add("Sqr")
        lstVBCommands.Add("Tan")
        'ARRAY FUNCTIONS
        lstVBCommands.Add("Array")
        lstVBCommands.Add("Filter")
        lstVBCommands.Add("IsArray")
        lstVBCommands.Add("Join")
        lstVBCommands.Add("LBound")
        lstVBCommands.Add("Split")
        lstVBCommands.Add("UBound")
        'STRING FUNCTIONS
        lstVBCommands.Add("InStr")
        lstVBCommands.Add("InStrRev")
        lstVBCommands.Add("LCase")
        lstVBCommands.Add("Left")
        lstVBCommands.Add("Len")
        lstVBCommands.Add("LTrim")
        lstVBCommands.Add("RTrim")
        lstVBCommands.Add("Trim")
        lstVBCommands.Add("Mid")
        lstVBCommands.Add("Replace")
        lstVBCommands.Add("Right")
        lstVBCommands.Add("Space")
        lstVBCommands.Add("StrComp")
        lstVBCommands.Add("String")
        lstVBCommands.Add("StrReverse")
        lstVBCommands.Add("UCase")
        'OTHER FUNCTIONS/KEYWORDS
        lstVBCommands.Add("CreateObject")
        lstVBCommands.Add("Eval")
        lstVBCommands.Add("IsEmpty")
        lstVBCommands.Add("Empty")
        lstVBCommands.Add("Is")
        lstVBCommands.Add("Nothing")
        lstVBCommands.Add("Null")
        'lstVBCommands.Add("Not")
        lstVBCommands.Add("True")
        lstVBCommands.Add("False")
        lstVBCommands.Add("IsNull")
        lstVBCommands.Add("IsNumeric")
        lstVBCommands.Add("IsObject")
        lstVBCommands.Add("RGB")
        lstVBCommands.Add("ScriptEngine")
        lstVBCommands.Add("ScriptEngineBuildVersion")
        lstVBCommands.Add("ScriptEngineMajorVersion")
        lstVBCommands.Add("ScriptEngineMinorVersion")
        lstVBCommands.Add("TypeName")
        lstVBCommands.Add("VarType")
        'PROCEDURES
        lstVBCommands.Add("Sub")
        lstVBCommands.Add("End")
        lstVBCommands.Add("Exit")
        lstVBCommands.Add("Do")
        lstVBCommands.Add("Loop")
        lstVBCommands.Add("For")
        lstVBCommands.Add("Next")
        lstVBCommands.Add("Until")
        lstVBCommands.Add("While")
        lstVBCommands.Add("If")
        lstVBCommands.Add("Then")
        lstVBCommands.Add("Else")
        lstVBCommands.Add("ElseIf")
        lstVBCommands.Add("Select")
        lstVBCommands.Add("Case")
        lstVBCommands.Add("Step")
        lstVBCommands.Add("Each")
        'lstVBCommands.Add("Else:")
        lstVBCommands.Add("Function")
        lstVBCommands.Add("Dim")
        'lstVBCommands.Add("=")
        'lstVBCommands.Add("+")
        'lstVBCommands.Add("-")
        'lstVBCommands.Add("/")
        lstVBCommands.Add("Option")
        lstVBCommands.Add("Explicit")
        lstVBCommands.Add("Call")
    End Sub

    Private Sub ConfigureScriptingEngine(Optional ByVal blnReset As Boolean = False)
        Try
            'Initiatiate ScriptControl
            'myscript.State = ScriptControlStates.Connected
            If blnReset Then myscript.Reset()
            myscript.UseSafeSubset = True
            myscript.Language = "VBScript"
            myscript.AllowUI = True

            'Add Variables and Controls
            myscript.AddObject("txtCmmnd", txtCommand)
            myscript.AddObject("RunButton", btnRun)
            myscript.AddObject("txtScreen", Output.txtOutput)
            myscript.AddObject("TCPReply", txtResponses)
            myscript.Timeout = 65000

            'Add Custom Code
            AddCustomCode(MasterPath & "CustomScript")
        Catch ex As Exception
            'MsgBox(ex.Message & " => Scripting_Engine ConfigureScriptingEngine")
        End Try

    End Sub

    Private Sub AddCustomAutomation(Optional ByVal OnlyNames = False)
        'MAKE THE LIST
        If lstCustomAutomation Is Nothing Then
        Else : lstCustomAutomation.Clear()
        End If

        Dim ThisAuto As AutomationType = Nothing
        For Each Dir As String In Directory.GetDirectories(ActiveAutomationDirectory)
            ThisAuto.autoName = Replace(Dir, ActiveAutomationDirectory, "")
            Dim AutoBlurb() As String = GetAutoBlurb(Dir)
            If Not AutoBlurb Is Nothing Then
                ThisAuto.autoFunction = AutoBlurb(0)
                If AutoBlurb.Count > 2 Then ThisAuto.autoExample = AutoBlurb(1)
            End If
            lstCustomAutomation.Add(ThisAuto)

            If Not OnlyNames AndAlso ActiveAutomationDirectory <> "" Then
                Dim ScriptFile As String = ActiveAutomationDirectory & ThisAuto.autoName & "\" & ThisAuto.autoName & ".rtn"
                Dim ScriptToRun As String = vbNullString
                If File.Exists(ScriptFile) Then
                    Dim FilReader As New StreamReader(ScriptFile)
                    ScriptToRun = FilReader.ReadToEnd()
                End If
                myscript.AddCode(ScriptToRun)
            End If
        Next

        ToolBox.lbxScripts.Items.Clear()
        For Each auto As AutomationType In lstCustomAutomation
            ToolBox.lbxScripts.Items.Add(auto.autoName)
        Next

    End Sub
    Private Function GetAutoBlurb(ByRef AutoDir As String) As String()
        Dim RoutineFile As String = AutoDir
        Do
            Dim SlashDelim As Integer = InStr(RoutineFile, "\")
            If SlashDelim <> 0 Then
                RoutineFile = Mid(RoutineFile, SlashDelim + 1, Len(RoutineFile) - SlashDelim)
            Else : Exit Do
            End If
        Loop
        Dim AutoPath As String = AutoDir & "\" & RoutineFile & ".rtn"

        If Not File.Exists(AutoPath) Then Return Nothing
        Dim line As String
        Dim FullLine As String = vbNullString
        Using reader As StreamReader = New StreamReader(AutoPath)
            For IntLine As Integer = 1 To 11
                line = reader.ReadLine
                If IntLine = 1 Then Continue For
                If line = "" Then Exit For
                If Mid(line, 1, 1) <> "'" Then Exit For
                If line = "'" Then FullLine &= vbCrLf
                FullLine &= Mid(line, 2, Len(line) - 1) & vbCrLf
            Next
        End Using
        Dim StringArr() As String = Split(FullLine, "|")
        Return StringArr
    End Function

    Private Sub AddCustomCode(ByVal SourceDir As String)
        If lstCustomCommands Is Nothing Then
        Else : lstCustomCommands.Clear()
        End If


        Dim di As New DirectoryInfo(SourceDir)
        Dim diArr As IO.FileInfo() = di.GetFiles()
        Dim dra As IO.FileInfo
        For Each dra In diArr
            Dim ThisCommand As CommandType = Nothing
            ThisCommand.cmdName = LCase(Mid(dra.ToString, 1, Len(dra.ToString) - 4))
            GetCommand(ThisCommand)
            lstCustomCommands.Add(ThisCommand)
            Dim ScriptFile As String = "CustomScript\" & dra.ToString
            Dim ScriptToRun As String = GetCode(ScriptFile)
            myscript.AddCode(ScriptToRun)
        Next

        ToolBox.lbxCode.Items.Clear()
        For Each ThisCommand As CommandType In lstCustomCommands
            ToolBox.lbxCode.Items.Add(ThisCommand.cmdName)
        Next
    End Sub

    Private Sub GetCommand(ByRef ThisCommand As CommandType)
        Dim CommandPath As String = MasterPath & "CustomScript\" & ThisCommand.cmdName & ".txt"
        Dim line As String
        Using reader As StreamReader = New StreamReader(CommandPath)
            For IntLine As Integer = 1 To 11
                line = Trim(reader.ReadLine)
                If IntLine = 1 Then Continue For ' sub or function declaration/title
                If line = "" OrElse Mid(line, 1, 1) <> "'" Then Exit For 'quit on the first empty line or  character
                If line.Length > 1 Then line = Mid(line, 2, Len(line) - 1)
                If InStr(LCase(line), "copythis: ") <> 0 Then
                    ThisCommand.cmdExample = Mid(line, 11, Len(line) - 10)
                ElseIf line = "'" Then
                    ThisCommand.cmdFunction &= vbCrLf
                Else
                    ThisCommand.cmdFunction &= line & vbCrLf
                End If
            Next
        End Using

    End Sub


    Private Sub SaveToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        'PERFORM SYNTAX CHECK
        'can't use "*_" in sc name

        Dim RoutineDirectory As String = MasterPath & "Routines\"
        If Not Directory.Exists(RoutineDirectory) Then Directory.CreateDirectory(RoutineDirectory)
        Dim SaveFil As New SaveFileDialog
        SaveFil.InitialDirectory = RoutineDirectory
        SaveFil.DefaultExt = "rtn"
        SaveFil.Filter = "Routine|*.rtn"
        SaveFil.RestoreDirectory = False
        'SaveFil.CreatePrompt = True
        If SaveFil.ShowDialog = vbOK Then
            'Save Coordinates and specs
            Dim oFile As System.IO.FileStream = Nothing
            Dim oWrite As System.IO.StreamWriter = Nothing
            oFile = New System.IO.FileStream(SaveFil.FileName, IO.FileMode.Create, IO.FileAccess.Write)
            oWrite = New System.IO.StreamWriter(oFile)
            oWrite.Write(rtbCode.Text)
            'oWrite.WriteLine(strButton)
            oWrite.Close()
            oFile.Close()
            oFile.Dispose()
            Me.Text = "Scripting Engine- " & Path.GetFileNameWithoutExtension(SaveFil.FileName)
        End If
        HighlightSyntax()
        rtbCode.SelectionStart = 0
    End Sub
    Private Sub ResetConfigs()
        IRFrequency = 6
        ConfigureScriptingEngine(True)
        CurrentRTBLine = 0
    End Sub
    Private Sub NewToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Dim StartString As String = "Sub ActiveSub()" & vbCrLf & vbCrLf & "End Sub"
        SetTabs()
        rtbCode.Text = StartString

        Me.Text = "Scripting Engine"
        ResetConfigs()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        Dim RoutineDirectory As String = MasterPath & "Routines\"
        If Not Directory.Exists(RoutineDirectory) Then Directory.CreateDirectory(RoutineDirectory)
        Dim OpenFil As New OpenFileDialog
        OpenFil.InitialDirectory = RoutineDirectory
        OpenFil.DefaultExt = "rtn"
        OpenFil.Filter = "Routine|*.rtn"
        OpenFil.RestoreDirectory = False
        OpenFil.ShowDialog()
        If OpenFil.FileName <> vbNullString Then
            ResetConfigs()
            Dim sr As New StreamReader(OpenFil.FileName)
            SetTabs()
            rtbCode.Text = sr.ReadToEnd()
            sr.Close()
        End If
        Me.Text = "Scripting Engine- " & Path.GetFileNameWithoutExtension(OpenFil.FileName)
        HighlightSyntax()
        rtbCode.SelectionStart = 0
    End Sub

    Private Sub btnRun_Click(sender As System.Object, e As System.EventArgs) Handles btnRun.Click

        If btnRun.Text = "Run" Then
            Try
                ActiveRun = True
                If ActiveAutomationDirectory <> "" And Not ActiveAutomationDirectory Is Nothing Then BuildCustomAutomationMenu()
                ScriptDisplayMode = True
                HighlightSyntax()
                'Dim ScriptToRun As String = rtbCode.Text
                rtbCode.SelectionStart = 0
                rtbCode.SelectionLength = InStr(rtbCode.Text, ")")
                Dim ActiveSub As String = Trim(Replace(rtbCode.SelectedText.ToLower, "sub ", ""))
                Dim ScriptToRun As String = AddFor_LoopProtection()

                myscript.AddCode(ScriptToRun)
                CurrentRTBLine = 0
                btnRun.Text = "Stop"
                Application.DoEvents()
                myscript.ExecuteStatement(ActiveSub)
            Catch ex As Exception
                If Not ex.Message = "Type mismatch: 'CDbl'" Then MsgBox(ex.Message & " => Scripting_Engine.btnRun.Click")
            End Try
        Else
            Try
                ActiveRun = False
                TV.AMC.StopRecordMedia()
                TV.BackgroundWorker1.CancelAsync()
                btnRun.Text = "Run"
                Application.DoEvents()
                txtCommand.Text = vbNullString
                txtCommand.Text = ""
                If TV.BackgroundWorker1.IsBusy Then TV.BackgroundWorker1.CancelAsync()

            Catch ex As Exception
                'MsgBox(ex.Message & " => Scripting_Engine.btnRun.Click")
            End Try
        End If

        ActiveRun = False
        RemoveBold()
        btnRun.Text = "Run"
        lblCurrentCommand.Text = ""


    End Sub

    Private Function IsAWordDelimiter(ByVal strCharacter As String) As Boolean
        For Each strDelimiter As String In lstWordDelims
            If strCharacter = strDelimiter Then
                Return True
                Exit Function
            End If
        Next
        Return False
    End Function
    Private Sub HighlightSyntax()
        RemoveColor()
        'LINE BY LINE HIGHLIGHT WORDS
        For LineNo = 0 To rtbCode.Lines.Count - 1
            rtbCode.SelectionStart = rtbCode.GetFirstCharIndexFromLine(LineNo)
            rtbCode.SelectionLength = rtbCode.Lines(LineNo).Length
            Dim LookInString As String = LCase(rtbCode.SelectedText)
            'First find a word on the line
            Dim LeftLetterNumber As Integer = 0
            Dim LeftLetterFound As Boolean = False
            Dim RightLetterNumber As Integer = 0
            For intCharacter = 1 To Len(LookInString)
                Dim WordFound As Boolean = False
                Dim strChar As String = Mid(LookInString, intCharacter, 1)
                'Go to next line if a green letter is found
                If strChar = "'" Then
                    rtbCode.SelectionStart = rtbCode.GetFirstCharIndexFromLine(LineNo) + (intCharacter - 1)
                    rtbCode.SelectionLength = rtbCode.Lines(LineNo).Length - (intCharacter - 1)
                    rtbCode.SelectionColor = Color.Green
                    Exit For
                End If
                'Otherwise find a word
                If Not IsAWordDelimiter(strChar) Then
                    If Not LeftLetterFound Then 'Left-most letter
                        LeftLetterNumber = rtbCode.GetFirstCharIndexFromLine(LineNo) + (intCharacter - 1)
                        LeftLetterFound = True
                        Continue For
                    ElseIf intCharacter = Len(LookInString) Then ' end of line
                        RightLetterNumber = rtbCode.GetFirstCharIndexFromLine(LineNo) + (intCharacter)
                        WordFound = True
                    End If
                End If
                If IsAWordDelimiter(strChar) AndAlso LeftLetterFound Then 'Right-most letter
                    RightLetterNumber = rtbCode.GetFirstCharIndexFromLine(LineNo) + (intCharacter - 1)
                    WordFound = True
                End If
                If WordFound Then
                    'reset search
                    LeftLetterFound = False
                    WordFound = False
                    'define the word
                    rtbCode.SelectionStart = LeftLetterNumber
                    rtbCode.SelectionLength = RightLetterNumber - LeftLetterNumber
                    Dim strWord As String = LCase(rtbCode.SelectedText)
                    'Check to see if it's VBScript-recognized syntax
                    Dim ReColored As Boolean = False
                    For Each strCommand As String In lstVBCommands
                        If strWord = LCase(strCommand) Then
                            rtbCode.SelectionColor = Color.Blue
                            ReColored = True
                            Exit For
                        End If
                    Next
                    'Check to see if it's a Custom-command
                    If ReColored Then Continue For
                    For Each strCommand As CommandType In lstCustomCommands
                        If strWord = LCase(strCommand.cmdName) Then
                            rtbCode.SelectionColor = Color.Purple
                            Exit For
                        End If
                    Next
                    'Check to see if it's Canned Automation
                    For Each strAutomation As AutomationType In lstCustomAutomation
                        If strWord = LCase(strAutomation.autoName) Then
                            rtbCode.SelectionColor = Color.Chocolate
                            Exit For
                        End If
                    Next

                End If
            Next
        Next
        Application.DoEvents()
    End Sub

    Private Function AddFor_LoopProtection() As String
        Dim NewText As String = vbNullString
        For LineNo = 0 To rtbCode.Lines.Count - 1
            rtbCode.SelectionStart = rtbCode.GetFirstCharIndexFromLine(LineNo)
            rtbCode.SelectionLength = rtbCode.Lines(LineNo).Length
            'Dim LookInString As String = LCase(rtbCode.SelectedText)
            Dim LookInString As String = rtbCode.SelectedText
            LookInString = Trim(LookInString)
            If Not LookInString = "" Then RemoveLeadingSpaces(LookInString)
            If Mid(LookInString.ToLower, 1, 4) = "loop" OrElse Mid(LookInString.tolower, 1, 4) = "next" Then
                NewText &= vbCr & "if runbutton.text = " & Chr(34) & "Run" & Chr(34) & " then exit sub" & vbCr
            End If
            NewText &= vbCr & LookInString
        Next
        Application.DoEvents()
        Return NewText
    End Function

    Private Sub RemoveLeadingSpaces(ByRef LookInString As String)
        Try
            Do
                'Dim Character As String = Mid(LookInString, 1, 1)
                Dim Character As String = Mid(LookInString.ToLower, 1, 1)
                If Character = vbTab OrElse Character = " " OrElse Character = vbCr OrElse Character = vbCrLf OrElse Character = vbLf OrElse Character = vbNullChar OrElse Character = vbNullString OrElse Character = vbBack Then
                    LookInString = Mid(LookInString, 2, Len(LookInString) - 1)
                Else : Exit Do
                End If
            Loop
        Catch ex As Exception

        End Try
    End Sub
    Public Sub CustomCodeSelection(ByVal i As String)
        For Each ThisCommand As CommandType In lstCustomCommands
            If LCase(ThisCommand.cmdName) = LCase(i) Then
                Dim Title As String = ThisCommand.cmdName
                Dim Prompt As String = ThisCommand.cmdFunction
                MsgBox(Prompt, MsgBoxStyle.OkOnly, Title)
                Exit For
            End If
        Next

    End Sub

    Public Sub BuildCustomAutomationMenu()
        'Dim NamesOnly As Boolean = True
        AddCustomAutomation()
    End Sub

    Public Sub CustomAutomationExpound(ByVal i As String)
        For Each ThisAuto As AutomationType In lstCustomAutomation
            If LCase(ThisAuto.autoName) = LCase(i) Then
                Dim Title As String = ThisAuto.autoName
                Dim Prompt As String = ThisAuto.autoFunction & vbCrLf & ThisAuto.autoExample
                MsgBox(Prompt, MsgBoxStyle.OkOnly, Title)
                Exit For
            End If
        Next
    End Sub

    Private Sub DelayToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles DelayToolStripMenuItem.Click
        Dim prompt As String = "Enter the Maximum IR Frequency (button presses per second)"
        Dim Title As String = "IR Frequency"
        Dim DefaultResponse As String = "6"
        Dim Result As Integer = CInt(Val(InputBox(prompt, Title, DefaultResponse)))
        If Result > 0 Then IRFrequency = Result
    End Sub

    Private Sub Cut_Click(sender As System.Object, e As System.EventArgs)
        rtbCode.Cut()
    End Sub

    Private Sub Paste_Click(sender As System.Object, e As System.EventArgs)
        rtbCode.Paste()
    End Sub

    Private Sub Copy_Click(sender As System.Object, e As System.EventArgs)
        rtbCode.Copy()
    End Sub

    Private Sub Copy_Click_1(sender As System.Object, e As System.EventArgs) Handles Copy.Click
        rtbCode.Copy()
    End Sub

    Private Sub Paste_Click_1(sender As System.Object, e As System.EventArgs) Handles Paste.Click
        rtbCode.Paste()
    End Sub

    Private Sub Cut_Click_1(sender As System.Object, e As System.EventArgs) Handles Cut.Click
        rtbCode.Cut()
    End Sub

    Private Function IsValidFileName(ByVal fn As String) As Boolean
        Try
            Dim fi As New IO.FileInfo(fn)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Private Sub SaveToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem1.Click
        Dim RoutineDirectory As String = MasterPath & "Routines\"
        Dim FilName As String = Replace(Me.Text, "Scripting Engine- ", "")
        If FilName = "Scripting Engine" Then
            Dim Prompt As String = "You must first name this routine"
            Dim Title As String = "Routine Name"
            FilName = InputBox(Prompt, Title)
            If FilName <> "" AndAlso FilName <> vbNullString Then
                If Not IsValidFileName(FilName) Then
                    MsgBox("Illegal Characters - Routine Not Saved")
                    Exit Sub
                End If
                If File.Exists(RoutineDirectory & FilName & ".rtn") Then
                    Prompt = FilName & "already exists." & vbCrLf & "Overwrite"
                    Title = "OverWrite?"
                    If MsgBox(Prompt, vbYesNo, Title) = vbNo Then
                        MsgBox("Routine Not Saved")
                        Exit Sub
                    End If
                End If
            End If
        End If

        If Not Directory.Exists(RoutineDirectory) Then Directory.CreateDirectory(RoutineDirectory)

        FilName = RoutineDirectory & FilName & ".rtn"

        Dim oFile As System.IO.FileStream = Nothing
        Dim oWrite As System.IO.StreamWriter = Nothing
        oFile = New System.IO.FileStream(FilName, IO.FileMode.Create, IO.FileAccess.Write)
        oWrite = New System.IO.StreamWriter(oFile)
        oWrite.Write(rtbCode.Text)
        'oWrite.WriteLine(strButton)
        oWrite.Close()
        oFile.Close()
        oFile.Dispose()

        HighlightSyntax()
        rtbCode.SelectionStart = 0
    End Sub

    Private Sub FTPAddressToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FTPAddressToolStripMenuItem.Click
        FTPAddress = InputBox("Enter the FTP Server Address", "FTP Server Assignment", FTPAddress)
        FTPAddress = Replace(LCase(FTPAddress), "ftp:", "")
        FTPAddress = Replace(FTPAddress, "/", "")
        FTPAddress = Replace(FTPAddress, "\", "")
    End Sub

    Private Sub rtbCode_MouseUp(sender As Object, e As MouseEventArgs) Handles rtbCode.MouseUp
        If ActiveRun Then lblCurrentCommand.Focus() 'rtbCode.DeselectAll() 'me.focus
    End Sub

    Private Sub VBScriptingSchoolToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VBScriptingSchoolToolStripMenuItem.Click
        Dim URL As String = "http://www.w3schools.com/asp/vbscript_ref_functions.asp"
        Process.Start(URL)
    End Sub

    Private Sub txtResponses_TextChanged(sender As Object, e As EventArgs) Handles txtResponses.TextChanged
        cboxInbox.Checked = True
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Timer2.Enabled = False
        If InStr(UCase(PWRServerCMD), "ALL") = 0 Then PWRServerCMD &= ActiveDevice.PwrServerPort
        Form1.ACServerTransactionByBatch(PWRServerCMD, ActiveDevice.PwrServerIP)
        Delay(8000)
        Try
            For Each p As Process In Process.GetProcesses
                If p.ProcessName = "cmd" Then p.Kill()
                Debug.Print(p.ProcessName)
            Next
        Catch ex As Exception

        End Try
        Cursor = Cursors.Default
    End Sub
End Class
