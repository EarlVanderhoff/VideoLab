Imports System.IO
Imports System.Text
Imports System.Net.Sockets
Imports System.Net
Imports System.Threading


Public Structure RCButton
    public intNumber As Integer
    Dim strName As String
    Dim strFunction As String
    Dim decRelativeLeft As Decimal
    Dim decRelativeTop As Decimal
    Dim decRelativeWidth As Decimal
    Dim decRelativeHeight As Decimal
    Dim blEnabled As Boolean
    Dim blViz As Boolean
    Dim intType As Integer ' 0 = continuous, 1 = one-shot
    Dim intShape As Integer  ' 0 = ellipse, 1 = rectangle
End Structure

Public Class Form1
    Public Shared TempButton As RCButton
    Public Shared listButton As New List(Of RCButton)
    Public Shared ButtonX As Integer
    Public Shared ButtonY As Integer
    Public Shared ButtonWidth As Integer
    Public Shared ButtonHeight As Integer
    Public Shared PixWidth As Integer
    Public Shared PixHeight As Integer

    'TCP Variables
    'Private LstIRSockets As List(Of TcpClient)

    Private IRSocket As New TcpClient
    Private VideoSwitchSocket As New TcpClient
    Private ScriptSocket As Socket
    Private ACTelnetSocket As New TcpClient

    Private IRStream As NetworkStream
    Private VideoSwitchStream As NetworkStream
    Private ScriptStream As NetworkStream
    Private ACStream As NetworkStream

    Private OutIRStream As Byte()
    Private OutVideoSwitchStream As Byte()
    Private OutScriptStream As Byte()
    Private OutACStream As Byte()

    Private ReceivedIRData As String
    Private ReceivedACData As String
    Private ReceivedScriptData As String
    Private ReceivedVideoSwitchData As String

    Private ACServerResponse As String
    Private ACCommandSucceeded As Boolean
    Private IRPortNumber As Integer = 4998

    'IR Variables
    Private IRCount As Integer
    Private Prefix As String
    'FormSize Variables
    Private VRCState As String = "Normal"
    Private NormalTop As Integer
    Private NormalLeft As Integer
    Private NormalWidth As Integer
    Private NormalHeight As Integer
    Private NormalBack As Color

    Public Shared AdminMode As Boolean

    Public Function ScriptSocketSend(ByVal CommandString As String, ByVal ScriptAddress As String, ByVal ScriptPort As Integer) As String
        Try
            ScriptSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            ScriptSocket.Connect(ScriptAddress, ScriptPort)
            Dim ScriptOutStream As Byte() = Encoding.ASCII.GetBytes(CommandString)
            ScriptSocket.BeginSend(ScriptOutStream, 0, ScriptOutStream.Length, SocketFlags.None, New AsyncCallback(AddressOf HandleReceivedScriptData), ScriptSocket)

        Catch ex As Exception
            ScriptSocket.Close()
            Return ex.Message & " => ScriptSocketSend"
        End Try
        Return "ScriptSocket OK"
    End Function

    Private Sub HandleReceivedScriptData(ByVal ar As IAsyncResult)
        ScriptSocket.Close()
    End Sub

    Private Function IRSocketSend(ByVal CommandString As String) As String

        Try
            If Not IRSocket.Connected Then
                IRSocket.Connect(ActiveDevice.IRServerIP, IRPortNumber)
            Else

            End If
            OutIRStream = Encoding.ASCII.GetBytes(CommandString)
            Dim inStream(100000) As Byte
            IRStream = IRSocket.GetStream()
            IRStream.Write(OutIRStream, 0, OutIRStream.Length)
            IRStream.Flush()
            IRStream.Read(inStream, 0, CInt(IRSocket.ReceiveBufferSize))
            ReceivedIRData = Encoding.ASCII.GetString(inStream)
            HandleReceivedData(ReceivedIRData)
        Catch ex As Exception
            Return ex.Message & " => IRSocketSend"
            IRSocketClose()
        End Try
        Return "IRSocket OK"
    End Function

    Private Function ACSocketSend(ByVal CommandString As String, ByVal ServerPort As Integer) As String
        ACServerResponse = vbNullString
        Try
            If Not ACTelnetSocket.Connected Then
                ACTelnetSocket.Connect(ActiveDevice.PwrServerIP, ServerPort)
                Pause(1000)
            End If
            OutACStream = Encoding.ASCII.GetBytes(CommandString)
            Dim inStream(100000) As Byte
            ACStream = ACTelnetSocket.GetStream()
            ACStream.Write(OutACStream, 0, OutACStream.Length)
            ACStream.Flush()
            ACStream.Read(inStream, 0, CInt(ACTelnetSocket.ReceiveBufferSize))
            ReceivedACData = Encoding.ASCII.GetString(inStream)
            HandleReceivedTelnetData(ReceivedACData)
        Catch ex As Exception
            Return ex.Message & " => ACSocketSend"
            ACSocketClose()
        End Try
        Delay(3333)
        WaitForTelnetResponse()

        Return "AC Socket OK"

    End Function

    Public Sub ACServerTransactionByBatch(ByVal ON_OFF As String, ByVal myIP As String)
        'Set OBJ = CreateObject("WScript.Shell")
        'OBJ.run "cmd"
        'WScript.Sleep 500
        'OBJ.SendKeys "telnet 192.168.1.21"
        'OBJ.SendKeys("{Enter}")
        'WScript.Sleep 500
        'OBJ.SendKeys "sysadmin"
        'OBJ.SendKeys("{Enter}")
        'WScript.Sleep 500
        'OBJ.SendKeys "PASS"
        'OBJ.SendKeys("{Enter}")
        'WScript.Sleep 500
        'OBJ.SendKeys "ON 1"
        'OBJ.SendKeys("{Enter}")
        'WScript.Sleep 800
        'OBJ.SendKeys "exit"
        'OBJ.SendKeys("{Enter}")
        'WScript.Terminate()
        Dim Script As New List(Of String)
        'start command window shell
        Script.Add("Set OBJ=CreateObject(" & Chr(34) & "WScript.Shell" & Chr(34) & ")")
        Script.Add("OBJ.run " & Chr(34) & "cmd" & Chr(34))
        Script.Add("WScript.Sleep 500")
        'start telnet session
        Script.Add("OBJ.SendKeys " & Chr(34) & "telnet " & myIP & Chr(34))
        Script.Add("OBJ.SendKeys(" & Chr(34) & "{Enter}" & Chr(34) & ")")
        Script.Add("WScript.Sleep 500")
        'send username
        Script.Add("OBJ.SendKeys " & Chr(34) & "sysadmin" & Chr(34))
        Script.Add("OBJ.SendKeys(" & Chr(34) & "{Enter}" & Chr(34) & ")")
        Script.Add("WScript.Sleep 500")
        'send password
        Script.Add("OBJ.SendKeys " & Chr(34) & "PASS" & Chr(34))
        Script.Add("OBJ.SendKeys(" & Chr(34) & "{Enter}" & Chr(34) & ")")
        Script.Add("WScript.Sleep 500")
        'send command
        Script.Add("OBJ.SendKeys " & Chr(34) & ON_OFF & Chr(34))
        Script.Add("OBJ.SendKeys(" & Chr(34) & "{Enter}" & Chr(34) & ")")
        Script.Add("WScript.Sleep 800")
        'exit
        Script.Add("OBJ.SendKeys " & Chr(34) & "exit" & Chr(34))
        Script.Add("OBJ.SendKeys(" & Chr(34) & "{Enter}" & Chr(34) & ")")

        'write file
        File.WriteAllLines("C:\VRC\PowerServerScripts\Script.vbs", Script)
        Delay(200)

        If Script IsNot Nothing Then Script.Clear()

        Cursor = Cursors.WaitCursor

        Process.Start("C:\VRC\PowerServerScripts\Power.bat")




    End Sub
    Public Function ACServerTransaction(ByVal ON_OFF As String) As String
        ACCommandSucceeded = False
        If LCase(ActiveDevice.PwrServerIP) = "na" OrElse LCase(ActiveDevice.PwrServerIP) = "0" Then
            Return "IP not avail"
            Exit Function
        End If
        Dim TelnetPort As Integer = 23
        Dim TelnetAddress As String = ActiveDevice.PwrServerIP
        If InStr(TelnetAddress, ":") <> 0 Then
            Dim TelnetArr() As String = Split(TelnetAddress, ":")
            TelnetAddress = TelnetArr(0)
            TelnetPort = CInt(Val(TelnetArr(1)))
        End If
        Try

            'Instantiate the socket
            ACTelnetSocket = New TcpClient
            'Connect to the Lantronix Server
            ACTelnetSocket.Connect(TelnetAddress, TelnetPort)
            WaitForTelnetResponse()

            'LANTRONIX ROUTINE
            ACSocketSend("sysadmin" & vbCr, TelnetPort)
            Delay(100)
            ACSocketSend("PASS" & vbCr, TelnetPort)
            Delay(100)
            ACSocketSend(ON_OFF, TelnetPort)
            Delay(100)
            ACSocketSend("exit" & vbCr, TelnetPort)
            ACSocketClose()
            'If ACCommandSucceeded Then Exit For
        Catch ex As Exception
            ACSocketClose()
            Scripting_Engine.txtCommand.Text = "TelnetFailure"
            Return ex.Message & " => ACServerTransaction"
        End Try
        If ACCommandSucceeded = True Then
            Scripting_Engine.txtCommand.Text = "TelnetSuccess"
        Else
            Scripting_Engine.txtCommand.Text = "TelnetFailure"
        End If
        Application.DoEvents()
        Return "ACSocket OK"
    End Function

    Public Function ACServerTransactionWAS(ByVal ON_OFF As String) As String
        ACCommandSucceeded = False
        If LCase(ActiveDevice.PwrServerIP) = "na" OrElse LCase(ActiveDevice.PwrServerIP) = "0" Then
            Return "IP not avail"
            Exit Function
        End If
        Dim TelnetPort As Integer = 23
        Dim TelnetAddress As String = ActiveDevice.PwrServerIP
        If InStr(TelnetAddress, ":") <> 0 Then
            Dim TelnetArr() As String = Split(TelnetAddress, ":")
            TelnetAddress = TelnetArr(0)
            TelnetPort = CInt(Val(TelnetArr(1)))
        End If
        Try
            For IntN = 1 To 3
                ACTelnetSocket = New TcpClient
                ACTelnetSocket.Connect(TelnetAddress, TelnetPort)
                'WaitForTelnetResponse()
                ACSocketSend(vbCr, TelnetPort)

                If InStr(UCase(ActiveDevice.PwrServerType), "RPC-3") = 0 Then
                    'LANTRONIX ROUTINE
                    ACSocketSend("sysadmin" & vbCr, TelnetPort)
                    ACSocketSend("PASS" & vbCr, TelnetPort)
                    ACSocketSend(ON_OFF, TelnetPort)
                    ACSocketSend("EXIT" & vbCr, TelnetPort)
                Else
                    'BAYTECH ROUTINE
                    ACSocketSend("1" & vbCr, TelnetPort)
                    Delay(500)
                    ACSocketSend(ON_OFF, TelnetPort)
                    Delay(1500)
                    ACSocketSend("LOGOUT" & vbCr, TelnetPort)
                    Delay(500)
                End If

                ACSocketClose()
                If ACCommandSucceeded Then Exit For

            Next
        Catch ex As Exception
            ACSocketClose()
            Scripting_Engine.txtCommand.Text = "TelnetFailure"
            Return ex.Message & " => ACServerTransaction"
        End Try
        If ACCommandSucceeded = True Then
            Scripting_Engine.txtCommand.Text = "TelnetSuccess"
        Else
            Scripting_Engine.txtCommand.Text = "TelnetFailure"
        End If
        Application.DoEvents()
        Return "ACSocket OK"
    End Function
    Public Sub SendSerialData(ByVal SwitchNo As Integer, ByVal SwitchIPAddr As String, ByVal SwitchIPPort As Integer)
        'Dim SwitchIP As String = ActiveDevice.VidSwIP
        If Not VideoSwitchSocket.Connected Then VideoSwitchSocket.Connect(SwitchIPAddr, SwitchIPPort)

        OutVideoSwitchStream = Encoding.ASCII.GetBytes(TranslateVidSwitchNo(SwitchNo))

        Dim inStream(100000) As Byte
        VideoSwitchStream = VideoSwitchSocket.GetStream()
        VideoSwitchStream.Write(OutVideoSwitchStream, 0, OutVideoSwitchStream.Length)
        VideoSwitchStream.Flush()

        Dim ReadSerial As Boolean = False
        If ReadSerial = False Then
            Pause(500)
        Else
            VideoSwitchStream.Read(inStream, 0, CInt(VideoSwitchSocket.ReceiveBufferSize))
            ReceivedVideoSwitchData = Encoding.ASCII.GetString(inStream)
            HandleSerialReceivedDate(ReceivedVideoSwitchData)
        End If
    End Sub
    Private Sub HandleSerialReceivedDate(ByVal msg As String)
        txtResponse.Text = msg
    End Sub

    Private Sub HandleReceivedTelnetData(ByVal msg As String)
        'MsgBox(msg)
        ACServerResponse = msg
    End Sub

    Private Sub HandleReceivedData(ByVal msg As String)
        txtResponse.Text = msg
    End Sub

    Private Function ACSocketClose() As String
        Try
            ACTelnetSocket.Close()
        Catch ex As Exception
            Return ex.Message & " => ACSocketClose"
        End Try
        Return "ACSocket OK"
    End Function

    Private Function IRSocketClose() As String
        Try
            IRSocket.Close()
        Catch ex As Exception
            Return ex.Message & " => IRSocketClose"
        End Try
        Return "IRSocket OK"
    End Function

    Public Function GetConnectorAddress(IRDev As Integer) As String
        Dim FirstPart As String = "4"""
        If IRDev > 3 Then FirstPart = "5"
        Dim SecondPart As String = IRDev.ToString
        If IRDev > 3 Then SecondPart = (IRDev - 3).ToString
        Return FirstPart & ":" & SecondPart
    End Function

    Public Sub SaveButtonList()
        Dim AllButtonString As String = vbNullString
        For Each item As RCButton In listButton
            AllButtonString &= CreateSaveString(item) & vbCrLf
        Next
        SaveButton(AllButtonString)
    End Sub

    Private Function CreateSaveString(ThisButton As RCButton) As String
        Dim builder As New StringBuilder
        Dim Delimiter As String = "|"
        builder.Append(ThisButton.strName & Delimiter)
        builder.Append(ThisButton.blEnabled & Delimiter)
        builder.Append(ThisButton.blViz & Delimiter)
        builder.Append(ThisButton.decRelativeHeight & Delimiter)
        builder.Append(ThisButton.intNumber & Delimiter)
        builder.Append(ThisButton.intShape & Delimiter)
        builder.Append(ThisButton.intType & Delimiter)
        builder.Append(ThisButton.decRelativeWidth & Delimiter)
        builder.Append(ThisButton.decRelativeLeft & Delimiter)
        builder.Append(ThisButton.decRelativeTop & Delimiter)
        'builder.Append(ThisButton.strFunction & Delimiter)
        Return builder.ToString
    End Function

    Private Sub SaveButton(ByVal strButton As String)
        Dim oFile As System.IO.FileStream = Nothing
        Dim oWrite As System.IO.StreamWriter = Nothing
        oFile = New System.IO.FileStream(ButtonFile, IO.FileMode.Create, IO.FileAccess.Write)
        oWrite = New System.IO.StreamWriter(oFile)
        oWrite.Write(strButton)
        'oWrite.WriteLine(strButton)
        oWrite.Close()
        oFile.Close()
    End Sub

    Private Sub DefineButton(ByVal ButtonStyle As Integer)
        ButtonHeight = 30
        ButtonWidth = 30
        With TempButton
            .intNumber = 1
            .strName = "unassigned"
            .strFunction = "Unassigned"
            .decRelativeLeft = CDec(ButtonX / pixRemoteControl.Width)
            .decRelativeTop = CDec(ButtonY / pixRemoteControl.Height)
            .decRelativeWidth = CDec(ButtonWidth / pixRemoteControl.Width)
            .decRelativeHeight = CDec(ButtonHeight / pixRemoteControl.Height)
            .blEnabled = True
            .blViz = True
            .intType = 1
            .intShape = ButtonStyle
        End With
        DrawButton(TempButton)
        PixWidth = pixRemoteControl.Width
        PixHeight = pixRemoteControl.Height
        frmButtonTailor.Show()
    End Sub

    Public Sub DrawButton(ByVal ThisButton As RCButton)
        Dim DrawWidth As Integer = CInt(ThisButton.decRelativeWidth * pixRemoteControl.Width)
        Dim DrawHeight As Integer = CInt(ThisButton.decRelativeHeight * pixRemoteControl.Height)
        Dim DrawX As Integer = CInt((ThisButton.decRelativeLeft * pixRemoteControl.Width) - DrawWidth / 2)
        Dim DrawY As Integer = CInt((ThisButton.decRelativeTop * pixRemoteControl.Height) - DrawHeight / 2)

        Dim p As New System.Drawing.Pen(Color.Red, 2)
        p.DashPattern = New Single() {4, 3} '{4.0F, 2.0F, 1.0F, 3.0F}
        Dim g As System.Drawing.Graphics
        If Not ShowAllButtons Then pixRemoteControl.Refresh()
        g = pixRemoteControl.CreateGraphics
        Select Case ThisButton.intShape
            Case 0
                p.DashCap = System.Drawing.Drawing2D.DashCap.Round
                g.DrawEllipse(p, DrawX, DrawY, DrawWidth, DrawHeight)
            Case 1
                g.DrawRectangle(p, DrawX, DrawY, DrawWidth, DrawHeight)
        End Select
    End Sub

    Public Sub SendIRCommand(ByVal thisbutton As RCButton)
        IRCount += 1
        Dim Prefix As String = "sendir," & GetConnectorAddress(ActiveDevice.IRServerPort) & "," & IRCount.ToString & ","
        Dim Commandstring As String = thisbutton.strFunction
        Commandstring = Replace(Commandstring, Chr(34), "")
        Commandstring = Prefix & Commandstring & vbCr

        Dim SockResult As String = IRSocketSend(Commandstring)
        'If Not SockResult = "IRSocket OK" Then MsgBox(SockResult)
    End Sub

    Private Sub ctsMenuEllipse_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles ctsMenuEllipse.MouseDown
        DefineButton(0)
    End Sub

    Private Sub ctsMenuRectangle_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles ctsMenuRectangle.MouseDown
        DefineButton(1)
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If IRSocket.Connected Then IRSocket.Close()
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.ShowInTaskbar = False
    End Sub

    Public Function LoadButtons() As Boolean
        'LOAD DEFAULT BUTTONS
        If Not File.Exists(ButtonFile) Then Return False
        If Not listButton Is Nothing Then listButton.Clear()

        Dim line As String

        Using reader As StreamReader = New StreamReader(ButtonFile)
            Dim ThisCommand As New RCCommands
            Do
                ' Read one line from file
                line = reader.ReadLine
                If line = "" Then Exit Do
                Dim Thisbutton As RCButton = (CreateButton(line))
                If InStr(LCase(Thisbutton.strFunction), "does not exist") = 0 Then
                    listButton.Add(Thisbutton)
                    ThisCommand.strFunction = Thisbutton.strFunction
                    ThisCommand.strName = Thisbutton.strName
                    lstRCCommands.Add(ThisCommand)
                End If
            Loop
        End Using
        SetNormalPosition()

        Me.TopMost = True
        If listButton.Count > 0 Then Return True
        Return False
    End Function

    Private Function CreateButton(ByVal strButton As String) As RCButton
        '.intNumber = 1
        '.strName = "unassigned"
        '.strFunction = "Unassigned"
        '.ptLocation = Here
        '.intWidth = 30
        '.intHeight = 30
        '.blEnabled = True
        '.blViz = True
        '.intType = 1
        '.intShape = ButtonStyle
        Dim PlaceButton As New RCButton
        Dim arrButtons() As String
        arrButtons = Split(strButton, "|")
        For intN As Integer = 0 To 10
            Dim ThisItem As String = arrButtons(intN)
            Select Case intN
                Case 0
                    PlaceButton.strName = ThisItem
                Case 1
                    PlaceButton.blEnabled = CBool(ThisItem)
                Case 2
                    PlaceButton.blViz = CBool(ThisItem)
                Case 3
                    PlaceButton.decRelativeHeight = CDec(ThisItem)
                Case 4
                    PlaceButton.intNumber = CInt(ThisItem)
                Case 5
                    PlaceButton.intShape = CInt(ThisItem)
                Case 6
                    PlaceButton.intType = CInt(ThisItem)
                Case 7
                    PlaceButton.decRelativeWidth = CDec(ThisItem)
                Case 8
                    PlaceButton.decRelativeLeft = CDec(ThisItem)
                Case 9
                    PlaceButton.decRelativeTop = CDec(ThisItem)
                Case 10
                    Dim buttonfile As String = CommandDirectory & PlaceButton.strName & ".btn"
                    PlaceButton.strFunction = InputCommandString(buttonfile)
            End Select
        Next
        Return PlaceButton
    End Function

    Private Sub DeleteToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        listButton.RemoveRange(CurrentButtonIndex, 1)
        SaveButtonList()
    End Sub

    Private Sub AssociateToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AssociateToolStripMenuItem.Click
        Dim fd As OpenFileDialog = New OpenFileDialog()
        Dim strFileName As String

        fd.Title = "Open File Dialog"
        fd.InitialDirectory = CommandDirectory
        fd.Filter = "Button files (*.btn)|*.btn|Button files (*.btn)|*.btn"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True
        If fd.ShowDialog() = DialogResult.OK Then
            strFileName = fd.FileName
            Dim BriefButton As RCButton = listButton(CurrentButtonIndex)
            Dim ThisButtonName As String = Path.GetFileName(fd.FileName)
            ThisButtonName = Replace(ThisButtonName, ".btn", "")
            ThisButtonName = Replace(ThisButtonName, ".Btn", "")
            ThisButtonName = Replace(ThisButtonName, ".BTN", "")
            BriefButton.strName = ThisButtonName
            BriefButton.strFunction = InputCommandString(strFileName)
            listButton.RemoveRange(CurrentButtonIndex, 1)
            listButton.Insert(CurrentButtonIndex, BriefButton)
        End If
        SaveButtonList()
    End Sub

    Private Function InputCommandString(strFile As String) As String
        If Not File.Exists(strFile) Then Return strFile & " does not exist"
        Dim line As String
        Using reader As StreamReader = New StreamReader(strFile)
            line = reader.ReadLine
        End Using
        If line = vbNullString Then Return strFile & " is empty"
        Return line
    End Function

    Private Function AmIHovering(ByVal intX As Integer, ByVal intY As Integer) As Integer
        'DETERMINE IF MOUSE IS HVOVERING ABOVE A BUTTON
        'IF SO, RECORD THE BUTTON INDEX
        Dim intN As Integer = 0
        CurrentButtonIndex = -1
        For Each item As RCButton In listButton
            Dim ThisButtonX As Integer = CInt(item.decRelativeLeft * pixRemoteControl.Width)
            Dim ThisButtonY As Integer = CInt(item.decRelativeTop * pixRemoteControl.Height)
            Dim ThisButtonWidth As Integer = CInt(item.decRelativeWidth * pixRemoteControl.Width)
            Dim ThisButtonHeight As Integer = CInt(item.decRelativeHeight * pixRemoteControl.Height)
            If intX >= ThisButtonX - 15 AndAlso intX <= ThisButtonX + CInt(ThisButtonWidth / 2) Then
                If intY >= ThisButtonY - 15 AndAlso intY <= ThisButtonY + CInt(ThisButtonHeight / 2) Then
                    CurrentButtonIndex = intN
                    Return intN
                End If
            End If
            intN += 1
        Next
        Return -1
    End Function

    Private Sub pixRemoteControl_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pixRemoteControl.MouseUp
        If Not ShowAllButtons Then pixRemoteControl.Refresh()
    End Sub

    Private Sub pixRemoteControl_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pixRemoteControl.MouseDown
        If VRCState = "Icon" Then
            VRCState = "Normal"
            Me.Top = NormalTop
            Me.Left = NormalLeft
            Me.Height = NormalHeight
            Me.Width = NormalWidth
            Me.BackColor = NormalBack
            If NormalBack = SystemColors.ActiveCaption Then
                Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
                HideControls(0)
                ResizingDone()
            Else
                Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Sizable
                HideControls(0)
                ResizingDone()
            End If
            Application.DoEvents()
            ResizingDone()
            Exit Sub
        End If

        'DETERMINE IF MOUSE IS HVOVERING ABOVE A BUTTON
        Dim AboveButton As Integer = AmIHovering(e.X, e.Y)
        If e.Button = Windows.Forms.MouseButtons.Right Then
            'If AdminMode Then
            If AboveButton < 0 Then 'not above button
                'CREATE BUTTON
                ButtonX = e.X
                ButtonY = e.Y
                ContextMenuStrip1.Show(Me, New Point(e.X, e.Y))
            Else
                'ASSOCIATE OR DELETE EXISTING BUTTON
                If CurrentButtonIndex < 0 Then Exit Sub
                If listButton Is Nothing Then Exit Sub
                TempButton = listButton(CurrentButtonIndex)
                DrawButton(TempButton)
                ButtonNameToolStripMenuItem.Text = Chr(34) & listButton(AboveButton).strName & Chr(34)
                ContextMenuStrip2.Show(Me, New Point(e.X, e.Y))
            End If
            'End If
        Else
            'SHOW BUTTON PRESS AND SEND COMMAND
            If listButton Is Nothing Then Exit Sub
            If AboveButton >= 0 Then ' above button
                TempButton = listButton(CurrentButtonIndex)
                DrawButton(TempButton)
                tmrButtonFace.Enabled = True
                If Not ActiveRun = True Then
                    SimulStartDevice = ActiveDevice.Index
                    SimulStopDevice = ActiveDevice.Index
                End If
                Scripting_Engine.BuildDeviceLists()
                SendSimulCommand(TempButton.strFunction)
            End If
        End If
    End Sub

    Public Sub RemoteButtonPress(ByVal AButton As String)
        AButton = LCase(AButton)
        If listButton Is Nothing Then Exit Sub
        For intN As Integer = 0 To listButton.Count - 1
            Dim ThatButton As String = Trim(LCase(listButton(intN).strName)).ToString
            If ThatButton = AButton Then
                'SHOW BUTTON PRESS AND SEND COMMAND
                TempButton = listButton(intN)
                DrawButton(TempButton)
                tmrButtonFace.Enabled = True
                'SendIRCommand(TempButton)
                If SimulStopDevice = 0 Then SimulStopDevice = SimulStartDevice
                SendSimulCommand(TempButton.strFunction)
                If Not ShowAllButtons Then pixRemoteControl.Refresh()
                Exit For
            End If
        Next
    End Sub

    Private Sub tmrButtonFace_Tick(sender As System.Object, e As System.EventArgs) Handles tmrButtonFace.Tick
        If Not ShowAllButtons Then pixRemoteControl.Refresh()
        tmrButtonFace.Enabled = False
    End Sub

    Private Sub HideControls(ByVal action As Integer)
        If action = 0 Then
            btnACOFF.Visible = True
            btnACON.Visible = True
            btnCC1.Visible = True
            btnCC2.Visible = True
            btnCC3.Visible = True
            cboDevice.Visible = True
            cboxOnTop.Visible = True
            txtResponse.Visible = True
        Else
            btnACOFF.Visible = False
            btnACON.Visible = False
            btnCC1.Visible = False
            btnCC2.Visible = False
            btnCC3.Visible = False
            cboDevice.Visible = False
            cboxOnTop.Visible = False
            txtResponse.Visible = False
        End If
        Application.DoEvents()
    End Sub

    Public Sub GoDark(Optional OnlyDark As Boolean = False)
        If Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None AndAlso Not OnlyDark Then
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Sizable
            Me.BackColor = SystemColors.InactiveCaption
            btnViola.Width = Me.Width + 10
            HideControls(0)
        Else
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            Me.BackColor = SystemColors.ActiveCaption
            btnViola.Width = pixRemoteControl.Width + 15
            HideControls(1)
        End If
        Application.DoEvents()
    End Sub
    Private Sub btnViola_Click(sender As System.Object, e As System.EventArgs) Handles btnViola.Click
        If VRCState = "Icon" Then
            VRCState = "Normal"
            Me.Top = NormalTop
            Me.Left = NormalLeft
            Me.Height = NormalHeight
            Me.Width = NormalWidth
            Me.BackColor = NormalBack
            If NormalBack = SystemColors.ActiveCaption Then Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            If NormalBack = SystemColors.InactiveCaption Then Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Sizable
            ResizingDone()
        End If
        ResizingDone()
        GoDark()
    End Sub

    Private Sub Form1_ResizeEnd(sender As Object, e As System.EventArgs) Handles Me.ResizeEnd
        ResizingDone()
    End Sub
    Private Sub ResizingDone()
        If Me.Height < 240 Then Me.Height = 240
        If Me.Width < 150 Then Me.Width = 150
        Application.DoEvents()
        Dim ControlLeft As Integer = pixRemoteControl.Left + +pixRemoteControl.Width + 10
        btnViola.Width = Me.Width + 10
        pixRemoteControl.Width = Me.Width - 120
        pixRemoteControl.Height = Me.Height - 60
        btnACOFF.Left = ControlLeft + btnACOFF.Width + 5
        btnACON.Left = ControlLeft
        btnCC1.Left = ControlLeft
        btnCC2.Left = ControlLeft
        btnCC3.Left = ControlLeft
        cboDevice.Left = ControlLeft
        cboxOnTop.Left = ControlLeft
        cboxShowAll.Left = ControlLeft
        txtResponse.Left = ControlLeft
        Application.DoEvents()
    End Sub

    Private Sub SetNormalPosition()
        NormalWidth = Me.Width
        NormalHeight = Me.Height
        NormalLeft = Me.Left
        NormalTop = Me.Top
        NormalBack = Me.BackColor
    End Sub

    Public Sub GoToLowerLeft()
        Dim RealEstate As Rectangle = Screen.PrimaryScreen.Bounds
        Me.Top = RealEstate.Top + RealEstate.Height - Me.Height
        Me.Left = RealEstate.Left + RealEstate.Width - CInt(Me.Width / 2)
    End Sub

    Private Sub SendCCCommand(ByVal CC As Integer, ByVal State As String)
        Dim RequestState As String = vbNullString
        If InStr(State, "ON") > 0 Then
            Select Case CC
                Case 1
                    btnCC1.Text = "CC1 OFF"
                Case 2
                    btnCC2.Text = "CC2 OFF"
                Case 3
                    btnCC3.Text = "CC3 OFF"
            End Select
            RequestState = "setstate,3:" & CC & ",1" & vbCr
        Else
            Select Case CC
                Case 1
                    btnCC1.Text = "CC1 ON"
                Case 2
                    btnCC2.Text = "CC2 ON"
                Case 3
                    btnCC3.Text = "CC3 ON"
            End Select
            RequestState = "setstate,3:" & CC & ",0" & vbCr
        End If
        IRSocketSend(RequestState)
    End Sub

    Private Sub btnCC1_Click(sender As System.Object, e As System.EventArgs) Handles btnCC1.Click
        SendCCCommand(1, btnCC1.Text)
    End Sub

    Private Sub btnCC2_Click(sender As System.Object, e As System.EventArgs) Handles btnCC2.Click
        SendCCCommand(2, btnCC2.Text)
    End Sub

    Private Sub btnCC3_Click(sender As System.Object, e As System.EventArgs) Handles btnCC3.Click
        SendCCCommand(3, btnCC3.Text)
    End Sub

    Private Sub cboxOnTop_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles cboxOnTop.CheckedChanged
        Me.TopMost = CBool(cboxOnTop.Checked)
    End Sub

    Private Sub cboDevice_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboDevice.SelectedIndexChanged
        LoadIRDevice(cboDevice.Text)
        TV.Text = ActiveDevice.Name & " - " & LabTownStreetAddress
        TV.lblURL.Text = "URL " & ActiveDevice.VideoServerIP
    End Sub

    Public Sub LoadIRDevice(ByVal DeviceName As String)
        Dim WasDevice As New IRDevice
        WasDevice = ActiveDevice
        Dim blnNewVidIP As Boolean = False

        'LOAD DEFAULT BUTTONS
        Dim DeviceFile As String
        DeviceFile = DeviceDirectory & DeviceName & ".csv"
        If Not File.Exists(DeviceFile) Then Exit Sub
        Dim line As String
        Dim LinArr() As String = Nothing
        Using reader As StreamReader = New StreamReader(DeviceFile)
            Do
                ' Read one line from file
                line = reader.ReadLine
                If line = "" Then Exit Do
                If line = "'" Then Continue Do
                LinArr = Split(line, "|")
                Dim LinKey As String = UCase(Trim(LinArr(0)))
                Dim LinValue As String = Trim(LinArr(1))
                If LinValue = "NA" Then LinValue = "0"
                With ActiveDevice
                    Select Case LinKey
                        Case "INDEX"
                            .Index = CInt(LinValue)
                        Case "NAME"
                            .Name = LinValue
                        Case "LOCATION"
                            .Location = LinValue
                        Case "DEVICETYPE"
                            .DeviceType = LinValue
                        Case "REMOTECONTROL"
                            .RemoteControl = LinValue
                        Case "IRSERVERIP"
                            .IRServerIP = LinValue
                        Case "IRSERVERPORT"
                            .IRServerPort = CInt(Val(LinValue))
                        Case "PWRSERVERTYPE"
                            .PwrServerType = LinValue
                        Case "PWRSERVERIP"
                            .PwrServerIP = LinValue
                        Case "PWRSERVERPORT"
                            .PwrServerPort = CInt(Val(LinValue))
                        Case "VIDEOSERVERIP"
                            .VideoServerIP = LinValue
                        Case "VIDEOSERVERTYPE"
                            .VideoServerType = LinValue
                        Case "VIDEOSERVERPORT"
                            .VideoServerPort = CInt(Val(LinValue))
                        Case "VIDEOSWITCHIP"
                            .VidSwIP = LinValue
                        Case "VIDEOCASCADESWITCHIP"
                            .VidCascSwIP = LinValue
                        Case "VIDEOSWITCHPORT"
                            .VidPort = CInt(Val(LinValue))
                        Case "VIDEOSWITCHTYPE"
                            .VidSwType = LinValue
                        Case "CC1"
                            .CC1 = CBool(LinValue)
                        Case "CC2"
                            .CC2 = CBool(LinValue)
                        Case "CC3"
                            .CC3 = CBool(LinValue)
                        Case Else
                            'DO NOTHING
                    End Select
                End With
            Loop
        End Using

        CreatePalette()
        HandleDevicePorts(WasDevice)
        SwitchVideo(ActiveDevice.VidSwType)
        If ActiveDevice.VideoServerIP <> WasDevice.VideoServerIP OrElse ActiveDevice.VideoServerPort <> WasDevice.VideoServerPort Then ConfigureVideoServer(ActiveDevice.VideoServerType)

        TV.Text = ActiveDevice.Name & " - " & LabTownStreetAddress
        TV.lblURL.Text = "URL " & ActiveDevice.VideoServerIP

    End Sub

    Private Sub HandleDevicePorts(ByVal LastDevice As IRDevice)
        'THERE ARE 5 DEVICE PORTS THUS FAR
        'THE 3 BELOW, THE VIDEO DEVICE WHICH CURRENTLY NEVER CHANGES IN ANY ASSET (FIX THAT)
        'AND THE SCRIPT DEVICE, WHICH IS HANDLED INDIVIDUALLY
        With ActiveDevice
            'If IRSocket.Connected Then
            '    If LastDevice.IRServerIP <> .IRServerIP OrElse LastDevice.IRServerPort <> .IRServerPort Then
            '        IRSocketClose()
            '        IRSocket = New TcpClient
            '    End If
            'End If

            If VideoSwitchSocket.Connected Then
                If LastDevice.VideoServerIP <> .VideoServerIP OrElse LastDevice.VideoServerIPPort <> .VideoServerIPPort Then
                    VideoSwitchSocket.Close()
                    VideoSwitchSocket = New TcpClient
                End If
            End If

            If ACTelnetSocket.Connected Then
                If LastDevice.PwrServerIP <> .PwrServerIP OrElse LastDevice.PwrServerPort <> .PwrServerPort Then
                    ACTelnetSocket.Close()
                    ACTelnetSocket = New TcpClient
                End If
            End If
        End With
    End Sub

    Private Sub ConfigureVideoServer(ByVal ServerType As String)
        Select Case ServerType
            Case "AXIS_M7001"
                TV.RefreshVideoServer(ActiveDevice.VideoServerIP, ActiveDevice.VideoServerType, ActiveDevice.VideoServerPort)
            Case "AXIS_243SA"
                TV.RefreshVideoServer(ActiveDevice.VideoServerIP, ActiveDevice.VideoServerType, ActiveDevice.VideoServerPort)
            Case "AXIS-MJPEG"
                TV.RefreshVideoServer(ActiveDevice.VideoServerIP, ActiveDevice.VideoServerType, ActiveDevice.VideoServerPort)
            Case "AXIS_7401"
                TV.RefreshVideoServer(ActiveDevice.VideoServerIP, ActiveDevice.VideoServerType, ActiveDevice.VideoServerPort)
            Case "AXIS_P7216"
                TV.RefreshVideoServer(ActiveDevice.VideoServerIP, ActiveDevice.VideoServerType, ActiveDevice.VideoServerPort)
            Case "BLACKMAGICPRO_H264"

        End Select
    End Sub

    Private Sub SwitchVideo(ByVal SwitchType As String)
        Dim SwitchIP As String = ActiveDevice.VidSwIP
        Dim SwitchIPPort As Integer = 4999
        Dim CascSwitchIP As String = ActiveDevice.VidCascSwIP
        If CascSwitchIP Is Nothing Then CascSwitchIP = ActiveDevice.VidSwIP
        Dim CascSwitchPort As Integer = 5000

        If InStr(ActiveDevice.VidSwIP, ":") <> 0 Then
            Dim SwitchAddrArr() As String = Split(ActiveDevice.VidSwIP, ":")
            If SwitchAddrArr.Count > 1 Then
                SwitchIP = SwitchAddrArr(0)
                SwitchIPPort = CInt(SwitchAddrArr(1))
            End If
            Dim CascSwitchAddrArr() As String = Split(ActiveDevice.VidCascSwIP, ":")
            If CascSwitchAddrArr.Count > 1 Then
                CascSwitchIP = CascSwitchAddrArr(0)
                CascSwitchPort = CInt(CascSwitchAddrArr(1))
            End If
        End If


        Select Case ActiveDevice.VidSwType
            Case "AVS816"
                If lstDevices.Count > 16 AndAlso ActiveDevice.VidPort > 15 Then
                    VideoSwitchSocket.Close()
                    VideoSwitchSocket = New TcpClient
                    SendSerialData(ActiveDevice.VidPort - 15, CascSwitchIP, CascSwitchPort)
                    VideoSwitchSocket.Close()
                    VideoSwitchSocket = New TcpClient
                    SendSerialData(16, SwitchIP, SwitchIPPort)
                Else
                    SendSerialData(ActiveDevice.VidPort, SwitchIP, SwitchIPPort)
                End If
            Case "VSW826"
                If lstDevices.Count > 16 AndAlso ActiveDevice.VidPort > 15 Then
                    VideoSwitchSocket.Close()
                    VideoSwitchSocket = New TcpClient
                    SendSerialData(ActiveDevice.VidPort - 15, CascSwitchIP, CascSwitchPort)
                    VideoSwitchSocket.Close()
                    VideoSwitchSocket = New TcpClient
                    SendSerialData(16, SwitchIP, SwitchIPPort)
                Else
                    SendSerialData(ActiveDevice.VidPort, SwitchIP, SwitchIPPort)
                End If
            Case "P7216"

            Case "GEFEN_HDMI"
                SendSerialData(ActiveDevice.VidPort, SwitchIP, SwitchIPPort)
        End Select
    End Sub
    Private Sub CreatePalette()
        With ActiveDevice
            btnCC1.Visible = .CC1
            btnCC2.Visible = .CC2
            btnCC3.Visible = .CC3
            If .PwrServerIP = "NA" Then
                btnACOFF.Visible = False
                btnACON.Visible = False
            Else
                btnACOFF.Visible = True
                btnACON.Visible = True
            End If
        End With
    End Sub
    Private Function TranslateVidSwitchNo(ByVal SwitchNum As Integer) As String
        Dim Resp As String = vbNullString
        Select Case ActiveDevice.VidSwType
            Case "AVS816"
                Dim Switch As String = vbNullString
                If SwitchNum < 10 Then
                    Switch = SwitchNum.ToString & SwitchNum.ToString
                Else
                    Select Case SwitchNum
                        Case 10
                            Switch = "AA"
                        Case 11
                            Switch = "BB"
                        Case 12
                            Switch = "CC"
                        Case 13
                            Switch = "DD"
                        Case 14
                            Switch = "EE"
                        Case 15
                            Switch = "FF"
                        Case 16
                            Switch = "GG"
                    End Select
                End If
                Resp = Chr(2) & "1" & Switch & vbCr
            Case "VSW826"
                Resp = SwitchNum.ToString & vbCr
            Case "P7216"

            Case "GEFEN_HDMI"
                Resp = SwitchNum.ToString
        End Select

        Return Resp
    End Function

    Private Sub Pause(ByVal duration As Integer)
        Dim Delay As New Stopwatch
        Delay.Start()
        Do
            Application.DoEvents()
        Loop Until Delay.ElapsedMilliseconds > duration
        Delay.Stop()

    End Sub

    Private Sub WaitForTelnetResponse()
        Dim TimeOut As New Stopwatch
        TimeOut.Start()
        Do
            Application.DoEvents()
            If ACServerResponse <> vbNullString Then Exit Do
        Loop Until TimeOut.ElapsedMilliseconds > 3000
        If InStr(LCase(ACServerResponse), "slp") <> 0 Then ACCommandSucceeded = True
    End Sub

    Private Sub btnACOFF_Click(sender As System.Object, e As System.EventArgs) Handles btnACOFF.Click
        'ACServerTransactionByBatch("Off " & ActiveDevice.PwrServerPort.ToString, ActiveDevice.PwrServerIP)
        Scripting_Engine.PWRServerCMD = "Off "
        Scripting_Engine.Timer2.Enabled = True
        Cursor = Cursors.Default
    End Sub

    Private Sub btnACON_Click(sender As System.Object, e As System.EventArgs) Handles btnACON.Click
        'ACServerTransactionByBatch("On " & ActiveDevice.PwrServerPort.ToString, ActiveDevice.PwrServerIP)
        Scripting_Engine.PWRServerCMD = "On "
        Scripting_Engine.Timer2.Enabled = True
        Cursor = Cursors.Default
    End Sub

    Private Sub pixRemoteControl_Click(sender As Object, e As EventArgs) Handles pixRemoteControl.Click

    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

    End Sub

    Private Sub ctsMenuEllipse_Click(sender As Object, e As EventArgs) Handles ctsMenuEllipse.Click

    End Sub

    Private Sub ctsMenuRectangle_Click(sender As Object, e As EventArgs) Handles ctsMenuRectangle.Click

    End Sub

    Private Sub ButtonNameToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ButtonNameToolStripMenuItem.Click

    End Sub

    Private Sub DisplayAllButtons()
        'SHOW All BUTTONS
        Dim intN As Integer = 0
        CurrentButtonIndex = 0
        For Each item As RCButton In listButton
            TempButton = listButton(CurrentButtonIndex)
            Dim DrawWidth As Integer = CInt(TempButton.decRelativeWidth * pixRemoteControl.Width)
            Dim DrawHeight As Integer = CInt(TempButton.decRelativeHeight * pixRemoteControl.Height)
            Dim DrawX As Integer = CInt((TempButton.decRelativeLeft * pixRemoteControl.Width) - DrawWidth / 2)
            Dim DrawY As Integer = CInt((TempButton.decRelativeTop * pixRemoteControl.Height) - DrawHeight / 2)

            Dim p As New System.Drawing.Pen(Color.Red, 2)
            p.DashPattern = New Single() {4, 3} '{4.0F, 2.0F, 1.0F, 3.0F}
            Dim g As System.Drawing.Graphics
            g = pixRemoteControl.CreateGraphics
            Select Case TempButton.intShape
                Case 0
                    p.DashCap = System.Drawing.Drawing2D.DashCap.Round
                    g.DrawEllipse(p, DrawX, DrawY, DrawWidth, DrawHeight)
                Case 1
                    g.DrawRectangle(p, DrawX, DrawY, DrawWidth, DrawHeight)
            End Select
            CurrentButtonIndex += 1
        Next
    End Sub

    Private Sub cboxShowAll_CheckedChanged(sender As Object, e As EventArgs) Handles cboxShowAll.CheckedChanged

        If cboxShowAll.Checked Then
            ShowAllButtons = True
            DisplayAllButtons()
        Else
            ShowAllButtons = False
            pixRemoteControl.Refresh()
        End If

    End Sub
End Class
