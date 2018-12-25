Imports System.IO
Imports System.Threading
Imports System.Drawing.Imaging
Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Security.Permissions

Module AppTools
    'Public II As New Stopwatch
    Public Structure IRDevice
        Dim Index As Integer 'FOR SORTING - NEEDS TO BE ADDED TO THE DEVICE FILES
        Dim Name As String
        Dim Location As String
        Dim RemoteControl As String
        Dim DeviceType As String
        Dim IRServerIP As String
        Dim IRServerPort As Integer
        Dim PwrServerType As String
        Dim PwrServerIP As String
        Dim PwrServerPort As Integer
        Dim VideoServerIP As String
        Dim VideoServerIPPort As Integer
        Dim VideoServerType As String
        Dim VideoServerPort As Integer
        Dim VidSwIP As String
        Dim VidCascSwIP As String
        Dim VidPort As Integer
        Dim VidSwType As String
        Dim CC1 As Boolean
        Dim CC2 As Boolean
        Dim CC3 As Boolean
    End Structure
    Public TCPSendTimeout As Boolean
    Public ActiveIRSockets As Integer
    Public ActiveDevice As IRDevice
    Public lstDevices As New List(Of IRDevice)
    Public lst1Devices As New List(Of IRDevice)
    Public lst2Devices As New List(Of IRDevice)
    Public lst3Devices As New List(Of IRDevice)
    Public lst4Devices As New List(Of IRDevice)
    Public lst5Devices As New List(Of IRDevice)
    Public lst6Devices As New List(Of IRDevice)

    Public Structure SimulSockets
        Dim Address As String
        Dim Port As Integer
        Dim Sock As Socket ' TcpClient
    End Structure
    Public lstIRSocketsAll As List(Of SimulSockets)

    Public Structure SceneCheck
        Dim Name As String
        Dim Rect As Rectangle
        Dim Sensitivity As Decimal
        Dim Duration As Decimal
        Dim Type As String
    End Structure

    Public Structure RCCommands
        Public strName As String
        Public strFunction As String
    End Structure
    Public lstRCCommands As New List(Of RCCommands)
    Public IRFrequency As Decimal = 6
    Public ActiveRun As Boolean
    Public MasterPath As String = "C:\VRC\"
    Public SCDirectory As String = MasterPath & "SceneChecks\"
    Public AutomationSCDirectory As String = MasterPath & "SceneChecks\"
    Public ButtonFile As String ' = MasterPath & "Commands\BigButtonFile.txt"
    Public CommandDirectory As String = MasterPath & "Commands"
    Public DeviceDirectory As String = MasterPath & "Devices\"
    Public SCFramesDirectory As String = MasterPath & "SCFrames\"
    Public CurrentSceneCheck As New SceneCheck
    Public CaptureFrames As Boolean
    Public SCFrameCount As Integer
    Public Coord As Rectangle
    Public ActiveSceneFound As Boolean
    Public FramesTaken As Integer
    Public FrameRate As Double = 29.97
    Public ActiveSCName As String
    Public ShowAllButtons As Boolean = False

    Public CurrentButtonIndex As Integer
    Public LabTownStreetAddress As String
    Public FTPAddress As String ' = "ftp://108.20.145.115/"
    Public RemoteControl As String = vbNullString
    Public RCPix As Bitmap
    Public ActiveAutomationFolder As String
    Public ActiveAutomationDirectory As String
    Public ActiveSceneCheckDirectory As String
    Public FoundFrame As Integer
    Public LTInitialized As Boolean
    Private IRCommandTimer As New Stopwatch
    Public SimulStartDevice As Integer
    Public SimulStopDevice As Integer
    Public ReceivedSimulData As String
    Public OCRSearchText As String

    Public OCRText As String
    Public BWBusy As Boolean

    Public Sub ReadSettings()
        Try
            If Not File.Exists(MasterPath & "Settings.txt") Then Exit Sub
            Using reader As StreamReader = New StreamReader(MasterPath & "Settings.txt")
                ' Read each line from file
                Dim Line As String = vbNullString
                Do
                    Line = reader.ReadLine
                    If Line = "" Then Exit Do
                    Dim LinArr() As String = Split(Line, "|")
                    Select Case UCase(LinArr(0))
                        Case Is = "STREETADDRESS"
                            LabTownStreetAddress = LinArr(1)
                        Case Is = "AUTOMATION DIRECTORY"
                            ActiveAutomationDirectory = LinArr(1)
                        Case Is = "FTP SERVER ADDRESS"
                            FTPAddress = LinArr(1)
                    End Select
                Loop Until Line = ""
            End Using
        Catch ex As Exception

        End Try

        'READ SETTINGS
    End Sub
    Private Sub CreateSettingsFile()
        File.Create(MasterPath & "Settings.txt")
        Delay(500)
    End Sub

    Public Sub WriteSettings()
        If Not Directory.Exists(MasterPath) Then Directory.CreateDirectory(MasterPath)
        'WRITE SETTINGS
        Dim SettingsFile As String = MasterPath & "Settings.txt"
        If Not File.Exists(SettingsFile) Then CreateSettingsFile()
        Dim ThisFile As FileStream = Nothing
        Dim oWrite As StreamWriter = Nothing
        ThisFile = New FileStream(SettingsFile, IO.FileMode.Create, IO.FileAccess.Write)
        oWrite = New StreamWriter(ThisFile)
        'oWrite.Write(LabTownStreetAddress)
        oWrite.WriteLine("STREETADDRESS|" & LabTownStreetAddress)
        oWrite.WriteLine("AUTOMATION DIRECTORY|" & ActiveAutomationDirectory)
        oWrite.WriteLine("FTP SERVER ADDRESS|" & FTPAddress)
        oWrite.Close()
        ThisFile.Close()
    End Sub

    Public Sub FTPTest()
        Dim buffer(1023) As Byte ' Allocate a read buffer of 1kB size
        Dim bytesIn As Integer ' Number of bytes read to buffer
        Dim totalBytesIn As Integer ' Total number of bytes received (= filesize)
        Dim output As IO.Stream ' A file to save response

        Try
            Dim FTPRequest As System.Net.FtpWebRequest = DirectCast(System.Net.WebRequest.Create("ftp://74.104.148.206/" & "RoutineText.TXT"), System.Net.FtpWebRequest)

            ' No credentials needed in this case. Usually you need to provide them. Catch the appropriate error if/when credentials are wrong!
            FTPRequest.Credentials = New System.Net.NetworkCredential("labtown", "labtown")
            ' Send a request to download a file
            FTPRequest.Method = System.Net.WebRequestMethods.Ftp.DownloadFile
            ' FTP server return a _response_ to your request
            Dim stream As System.IO.Stream = FTPRequest.GetResponse.GetResponseStream

            ' If you need the length of the file, send a request Ftp.GetFileSize and read the response
            'Dim length As Integer = CInt(FTPRequest.GetResponse.ContentLength)

            ' Write the content to the output file
            output = System.IO.File.Create(MasterPath & "file2.txt")
            bytesIn = 1 ' Set initial value to 1 to get into loop. We get out of the loop when bytesIn is zero
            Do Until bytesIn < 1
                bytesIn = stream.Read(buffer, 0, 1024) ' Read max 1024 bytes to buffer and get the actual number of bytes received
                If bytesIn > 0 Then
                    ' Dump the buffer to a file
                    output.Write(buffer, 0, bytesIn)
                    ' Calc total filesize
                    totalBytesIn += bytesIn

                    Application.DoEvents()
                End If
            Loop
            ' Close streams
            output.Close()
            stream.Close()
        Catch ex As Exception
            ' Catch exceptions. THIS IS ONLY FOR DEBUGGING ERRORS. In the production code, use a bit smarter exception catching ;)
            MessageBox.Show(ex.Message & " => AppTools.FTPTest")
        End Try
    End Sub
    Public Function IsFileAvailable(ByVal FilePath As String) As Boolean
        Dim thisFileInUse As Boolean = False
        If System.IO.File.Exists(FilePath) Then
            Try
                Using f As New IO.FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
                    ' thisFileInUse = False
                End Using
            Catch
                Return False
            End Try
        End If
        Return True
    End Function

    Public Function FTPCheckIfFileExists(ByVal fileUri As String) As Boolean
        Dim FTPRequest As FtpWebRequest = DirectCast(WebRequest.Create(fileUri), FtpWebRequest)
        FTPRequest.Credentials = New NetworkCredential("labtown", "labtown")
        FTPRequest.Method = WebRequestMethods.Ftp.GetFileSize
        Try
            Dim response As FtpWebResponse = CType(FTPRequest.GetResponse(), FtpWebResponse)
            response = response
            ' THE FILE EXISTS
        Catch ex As WebException
            Dim response As FtpWebResponse = CType(ex.Response, FtpWebResponse)
            If FtpStatusCode.ActionNotTakenFileUnavailable = response.StatusCode Then
                ' THE FILE DOES NOT EXIST
                Return False
            End If
        End Try
        Return True
    End Function

    Public Function FTPGetFileText(ByVal FilePath As String) As String
        Dim ReturnText As String
        Try
            Dim filename As String = FilePath
            Dim FTPRequest As FtpWebRequest = DirectCast(WebRequest.Create(filename), FtpWebRequest)
            FTPRequest.Credentials = New NetworkCredential("labtown", "labtown")
            FTPRequest.Method = WebRequestMethods.Ftp.DownloadFile
            Dim ftpRespStream As Stream = FTPRequest.GetResponse.GetResponseStream
            Dim reader As StreamReader
            reader = New StreamReader(ftpRespStream, System.Text.Encoding.UTF8)
            ReturnText = reader.ReadToEnd
        Catch ex As Exception
            Return vbNullString
        End Try
        Return ReturnText
    End Function

    Public Sub FTPFile(ByVal SourceFile As String, ByVal DestinationAddress As String)
        Dim buffer(1023) As Byte
        Dim bytesIn As Integer
        'Dim totalBytesIn As Integer
        Dim output As IO.Stream

        Try
            Dim FTPRequest As FtpWebRequest = DirectCast(WebRequest.Create(FTPAddress & SourceFile), FtpWebRequest)
            FTPRequest.Credentials = New NetworkCredential("labtown", "labtown")
            FTPRequest.Method = WebRequestMethods.Ftp.DownloadFile
            Dim stream As Stream = FTPRequest.GetResponse.GetResponseStream
            output = File.Create(DestinationAddress)
            bytesIn = 1
            Do Until bytesIn < 1
                bytesIn = stream.Read(buffer, 0, 1024)
                If bytesIn > 0 Then
                    output.Write(buffer, 0, bytesIn)
                    'totalBytesIn += bytesIn
                    'Label1.Text = totalBytesIn.ToString
                    Application.DoEvents()
                End If
            Loop
            output.Close()
            stream.Close()
        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Function FTPDirectoryList(ByVal SourceAddress As String) As List(Of String)
        Dim DirectoryList As New List(Of String)
        Try
            Dim FTPRequest As FtpWebRequest = DirectCast(WebRequest.Create(SourceAddress), FtpWebRequest)
            FTPRequest.Credentials = New NetworkCredential("labtown", "labtown")
            FTPRequest.Method = WebRequestMethods.Ftp.ListDirectory

            Dim FTPResponse As FtpWebResponse = DirectCast(FTPRequest.GetResponse(), FtpWebResponse)
            Dim Rsst As New StreamReader(FTPResponse.GetResponseStream)

            Dim line As String = Rsst.ReadLine
            While Not String.IsNullOrEmpty(line)
                DirectoryList.Add(line)
                line = Rsst.ReadLine
            End While

            Rsst.Close()
            FTPResponse.Close()
        Catch ex As Exception
            Return DirectoryList
            'MessageBox.Show(ex.Message)
        End Try

        Return DirectoryList
    End Function

    Public Function LocalDirectoryList(ByVal SourceAddress As String) As List(Of String)
        Dim DirectoryList As New List(Of String)
        Try
            Dim dir As New IO.DirectoryInfo(SourceAddress)
            Dim dirArr As IO.FileInfo() = dir.GetFiles()
            Dim filnam As IO.FileInfo = Nothing
            For Each filnam In dirArr
                DirectoryList.Add(filnam.ToString)
            Next
        Catch ex As Exception
            Return DirectoryList
        End Try

        Return DirectoryList
    End Function

    Public Sub SelectMultipleDevices()
        'If Not Directory.Exists(MasterPath & "Devices\") Then Exit Sub
        If Not Directory.Exists(DeviceDirectory) Then Exit Sub
        SocketsClear()
        Dim TheseAddresses As List(Of String) = GetAddressList()
        lstIRSocketsAll = CreateSockets(TheseAddresses)
        OpenSockets()
        TheseAddresses.Clear()
    End Sub

    Public Sub ReconnectSimulsocket()

    End Sub

    Public Sub SendSimulCommand(ByVal CommandLine As String)
        IRCommandTimer.Reset()
        IRCommandTimer.Start()
        ActiveIRSockets = 0
        'PERFORM PORT 1 COMMANDS
        Dim lstCommands As New List(Of IRCommand)
        For Each ThisDEvice As IRDevice In lst1Devices
            Dim FullCommand As String = BuildCommand(CommandLine, ThisDEvice.IRServerPort)
            Dim thisircommand As New IRCommand
            lstCommands.Add(thisircommand)
            thisircommand.SendCommand(FullCommand, ThisDEvice.IRServerIP)
        Next
        ClearIRCommandsList(lstCommands)

        IRCommandTimer.Reset()
        IRCommandTimer.Start()
        'PERFORM PORT 2 COMMANDS
        For Each ThisDEvice As IRDevice In lst2Devices
            Dim FullCommand As String = BuildCommand(CommandLine, ThisDEvice.IRServerPort)
            Dim thisircommand As New IRCommand
            lstCommands.Add(thisircommand)
            thisircommand.SendCommand(FullCommand, ThisDEvice.IRServerIP)
        Next
        ClearIRCommandsList(lstCommands)

        IRCommandTimer.Reset()
        IRCommandTimer.Start()
        'PERFORM PORT 3 COMMANDS
        For Each ThisDEvice As IRDevice In lst3Devices
            Dim FullCommand As String = BuildCommand(CommandLine, ThisDEvice.IRServerPort)
            Dim thisircommand As New IRCommand
            lstCommands.Add(thisircommand)
            thisircommand.SendCommand(FullCommand, ThisDEvice.IRServerIP)
        Next
        ClearIRCommandsList(lstCommands)

        IRCommandTimer.Reset()
        IRCommandTimer.Start()
        'PERFORM PORT 4 COMMANDS
        For Each ThisDEvice As IRDevice In lst4Devices
            Dim FullCommand As String = BuildCommand(CommandLine, ThisDEvice.IRServerPort)
            Dim thisircommand As New IRCommand
            lstCommands.Add(thisircommand)
            thisircommand.SendCommand(FullCommand, ThisDEvice.IRServerIP)
        Next
        ClearIRCommandsList(lstCommands)

        IRCommandTimer.Reset()
        IRCommandTimer.Start()
        'PERFORM PORT 5 COMMANDS
        For Each ThisDEvice As IRDevice In lst5Devices
            Dim FullCommand As String = BuildCommand(CommandLine, ThisDEvice.IRServerPort)
            Dim thisircommand As New IRCommand
            lstCommands.Add(thisircommand)
            thisircommand.SendCommand(FullCommand, ThisDEvice.IRServerIP)
        Next
        ClearIRCommandsList(lstCommands)

        IRCommandTimer.Reset()
        IRCommandTimer.Start()
        'PERFORM PORT 6 COMMANDS
        For Each ThisDEvice As IRDevice In lst6Devices
            Dim FullCommand As String = BuildCommand(CommandLine, ThisDEvice.IRServerPort)
            Dim thisircommand As New IRCommand
            lstCommands.Add(thisircommand)
            thisircommand.SendCommand(FullCommand, ThisDEvice.IRServerIP)
        Next
        ClearIRCommandsList(lstCommands)

        IRCommandTimer.Stop()
    End Sub
    'Private Sub SendCommand(ByVal IRCommand As String, ByVal ThisDevice As IRDevice)
    '    Try
    '        For Each indexSimulSocket As SimulSockets In lstIRSocketsAll
    '            With indexSimulSocket
    '                Dim FullAddress As String = .Address
    '                If InStr(ThisDevice.IRServerIP, ":") <> 0 Then FullAddress &= ":" & .Port
    '                If FullAddress = ThisDevice.IRServerIP Then
    '                    'If Not .Sock.Connected Then .Sock.Connect(.Address, .Port)
    '                    'Dim OutSimulSTream As Byte() = Encoding.ASCII.GetBytes(IRCommand)
    '                    'Dim InStream(10000) As Byte
    '                    'Dim SimulStream As System.Net.Sockets.NetworkStream = .Sock.GetStream
    '                    'SimulStream.Write(OutSimulSTream, 0, OutSimulSTream.Length)
    '                    'SimulStream.Flush()
    '                    'SimulStream.Read(InStream, 0, CInt(.Sock.ReceiveBufferSize))
    '                    ''receivedSimuldata = Encoding.ASCII.GetString(InStream)
    '                    ''HandleReceivedData(ReceivedSimulData)
    '                End If
    '            End With
    '        Next

    '    Catch ex As Exception

    '    End Try

    'End Sub
    Private Sub WaitForIRCommand()
        'allow a minimum of time per IR command, per Global Cache port set
        Do Until ActiveIRSockets = 0
            Application.DoEvents()
            If Not ActiveRun Then Exit Do
            If IRCommandTimer.ElapsedMilliseconds > 2000 / IRFrequency Then Exit Do
        Loop
        ActiveIRSockets = 0
        Do
            Application.DoEvents()
            If Not ActiveRun Then Exit Do
        Loop Until IRCommandTimer.ElapsedMilliseconds > 1000 / IRFrequency
        IRCommandTimer.Reset()
    End Sub

    Private Sub ClearIRCommandsList(ByRef lstCommands As List(Of IRCommand))
        If lstCommands.Count > 0 Then
            WaitForIRCommand()
            For Each thiscommand As IRCommand In lstCommands
                thiscommand.Dispose()
            Next
            lstCommands.Clear()
        End If
    End Sub

    Private Sub HandleReceivedData(ByVal msg As String)
        'Do Nothing
    End Sub

    Private Function BuildCommand(ByVal fiosCommand As String, ByVal PortNo As Integer) As String
        Dim Prefix As String = "sendir," & Form1.GetConnectorAddress(PortNo) & ",100,"
        Dim commandstring As String = Prefix & Replace(fiosCommand, Chr(34), "") & vbCr
        Return commandstring
    End Function

    Private Sub SocketsClear()
        'close and clear the simultaneous sockets
        If Not lstIRSocketsAll Is Nothing AndAlso Not lstIRSocketsAll.Count = 0 Then
            For Each indexSimulSocket As SimulSockets In lstIRSocketsAll
                indexSimulSocket.Sock.Close()
            Next
            lstIRSocketsAll.Clear()
        End If
    End Sub
    Private Function GetAddressList() As List(Of String)
        Dim lstAddresses As New List(Of String)
        For Each ThisDevice As IRDevice In lstDevices
            Dim ThisPort As Integer = 4998
            Dim ThisAddress As String = ThisDevice.IRServerIP
            If InStr(ThisDevice.IRServerIP, ":") <> 0 Then
                Dim ArrAddress() As String = Split(ThisDevice.IRServerIP, ":")
                ThisAddress = ArrAddress(0)
                ThisPort = CInt(ArrAddress(1))
            End If
            ThisAddress &= ":" & ThisPort.ToString
            Dim AddAddress As Boolean = True
            If Not lstAddresses.Count = 0 Then
                For Each strAddress As String In lstAddresses
                    If strAddress = ThisAddress Then
                        AddAddress = False
                        Exit For
                    End If
                Next
            End If
            If AddAddress Then lstAddresses.Add(ThisAddress)
        Next
        Return lstAddresses
    End Function
    Private Function WASGetAddressList(ByVal StartBox As Integer, ByVal StopBox As Integer) As List(Of String)
        Dim lstAddresses As New List(Of String)
        For STB As Integer = StartBox To StopBox
            Dim ThisDevice As New IRDevice
            For Each indexDevice As IRDevice In lstDevices
                If indexDevice.Index = STB Then
                    ThisDevice = indexDevice
                    Exit For
                End If
            Next
            Dim ThisPort As Integer = 4998
            Dim ThisAddress As String = ThisDevice.IRServerIP
            If InStr(ThisDevice.IRServerIP, ":") <> 0 Then
                Dim ArrAddress() As String = Split(ThisDevice.IRServerIP, ":")
                ThisAddress = ArrAddress(0)
                ThisPort = CInt(ArrAddress(1))
            End If
            ThisAddress &= ":" & ThisPort.ToString
            Dim AddAddress As Boolean = True
            If Not lstAddresses.Count = 0 Then
                For Each strAddress As String In lstAddresses
                    If strAddress = ThisAddress Then
                        AddAddress = False
                        Exit For
                    End If
                Next
            End If
            If AddAddress Then lstAddresses.Add(ThisAddress)
        Next
        Return lstAddresses
    End Function

    Private Function CreateSockets(ByVal IPsList As List(Of String)) As List(Of SimulSockets)
        Dim lstSimulSockets As New List(Of SimulSockets)
        For Each indexAddress As String In IPsList
            Dim ThisSimulSocket As New SimulSockets
            Dim ArrAdd() As String = Split(indexAddress, ":")
            ThisSimulSocket.Address = ArrAdd(0)
            ThisSimulSocket.Port = CInt(ArrAdd(1))
            ThisSimulSocket.Sock = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) ' New TcpClient
            lstSimulSockets.Add(ThisSimulSocket)
        Next
        Return lstSimulSockets
    End Function

    Private Sub OpenSockets()
        Dim ThisPort As Integer
        Dim ThisAddress As String = vbNullString
        Try
            For Each indexSimulsocket As SimulSockets In lstIRSocketsAll
                ThisPort = indexSimulsocket.Port
                ThisAddress = indexSimulsocket.Address
                indexSimulsocket.Sock.Connect(ThisAddress, ThisPort)
            Next
        Catch ex As Exception
            MsgBox("Cannot connect to " & ThisAddress & ":" & ThisPort.ToString)
        End Try
    End Sub


    Public Sub LoadDevices()
        Try
            'CLEAR CURRENT DEVICE LIST
            If Not lstDevices Is Nothing Then lstDevices.Clear()
            'CREATE NEW DEVICE LIST
            'If Not Directory.Exists(MasterPath & "Devices\") Then Exit Sub
            If Not Directory.Exists(DeviceDirectory) Then Exit Sub
            'Dim di As New DirectoryInfo(MasterPath & "Devices")
            Dim di As New DirectoryInfo(DeviceDirectory)
            Dim diArr As IO.FileInfo() = di.GetFiles()
            Dim dra As IO.FileInfo
            For Each dra In diArr
                Dim ThisIRDevice As IRDevice = builddevice(di.ToString & "\" & dra.ToString)
                lstDevices.Add(ThisIRDevice)
            Next
            'UPDATE THE DROP-DOWN DEVICE LIST
            Form1.cboDevice.Items.Clear()
            For IntN = 0 To lstDevices.Count - 1
                For Each indexDevice As IRDevice In lstDevices
                    If indexDevice.Index - 1 = IntN Then
                        Dim IndexedName As String = indexDevice.Index.ToString & ": " & indexDevice.Name
                        Form1.cboDevice.Items.Add(indexDevice.Name)
                    End If
                Next
            Next
            Form1.cboDevice.SelectedIndex = 0
        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try

    End Sub

    Public Function GetDevices() As Boolean
        If Not Directory.Exists(DeviceDirectory) Then Directory.CreateDirectory(DeviceDirectory)
        Dim TempLstDevices As New List(Of String)
        Dim SourcePath As String = FTPAddress & "Streets/" & LabTownStreetAddress & "/Devices/"
        TempLstDevices = AppTools.FTPDirectoryList(SourcePath)
        If TempLstDevices.Count = 0 Then Return False
        For Each item As String In TempLstDevices
            Dim FilePath = "Streets/" & LabTownStreetAddress & "/Devices/" & item
            'Dim DestinationPath As String = MasterPath & "Devices\" & item
            Dim DestinationPath As String = DeviceDirectory & item
            If Not File.Exists(DestinationPath) Then AppTools.FTPFile(FilePath, DestinationPath)
        Next
        Return True
    End Function
    Public Function builddevice(ByVal DeviceFile As String) As IRDevice
        Dim ResultDevice As New IRDevice
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
                With ResultDevice
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
        Return ResultDevice
    End Function


    Public Sub Crop(ByRef source As Bitmap, ByVal startingPoint As Point, ByVal cropSize As Size)
        Try
            Dim rectSource As New Rectangle(startingPoint, cropSize)
            Dim rectDestination As New Rectangle(New Point, cropSize)

            Using result As New Bitmap(cropSize.Width, cropSize.Height)
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


    Public Function IllegalCharacters(ByVal FileString As String) As String
        Dim Illegal As Boolean = False
        If InStr(FileString, "/") <> 0 Then Illegal = True
        If InStr(FileString, "|") <> 0 Then Illegal = True
        If InStr(FileString, "\") <> 0 Then Illegal = True
        If InStr(FileString, Chr(34)) <> 0 Then Illegal = True
        If InStr(FileString, "=") <> 0 Then Illegal = True
        If InStr(FileString, "+") <> 0 Then Illegal = True
        If InStr(FileString, "?") <> 0 Then Illegal = True
        If InStr(FileString, "#") <> 0 Then Illegal = True
        If InStr(FileString, "$") <> 0 Then Illegal = True
        If InStr(FileString, "%") <> 0 Then Illegal = True
        If InStr(FileString, "^") <> 0 Then Illegal = True
        If InStr(FileString, "&") <> 0 Then Illegal = True
        If InStr(FileString, "*") <> 0 Then Illegal = True
        If Illegal = False Then FileString = Replace(FileString, vbLf, "")
        If Illegal = False Then FileString = Replace(FileString, vbCr, "")
        If Illegal = False Then FileString = Replace(FileString, vbCrLf, "")
        If Illegal = False Then FileString = Replace(FileString, vbTab, "")
        FileString = Trim(FileString)
        If Illegal Then Return "illegal"
        Return FileString

    End Function
    Public Function CheckForSameness(ByVal bitmapMaster As Bitmap, ByVal bitmapCompare As Bitmap,
                                                      ByVal ImageThreshold As Decimal) As Boolean
        Dim bdataMaster As BitmapData = Nothing
        Dim bdataCompare As BitmapData = Nothing
        Dim Thresh As Integer = 5
        Try
            bdataMaster = bitmapMaster.LockBits(New Rectangle(0, 0, bitmapMaster.Width, bitmapMaster.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb)
            bdataCompare = bitmapCompare.LockBits(New Rectangle(0, 0, bitmapMaster.Width, bitmapMaster.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb)

            Dim decTotalWeight As Decimal
            With bdataMaster
                For intX As Integer = 0 To .Width - 1
                    For intY As Integer = 0 To .Height - 1
                        Dim intOffset As Integer = .Stride * intY + (3 * intX)
                        Dim pointerMaster As IntPtr = .Scan0
                        Dim intBlue1 As Integer = Marshal.ReadByte(pointerMaster, intOffset)
                        Dim intGreen1 As Integer = Marshal.ReadByte(pointerMaster, intOffset + 1)
                        Dim intRed1 As Integer = Marshal.ReadByte(pointerMaster, intOffset + 2)
                        DeleteObject(pointerMaster)

                        Dim pointerCompare As IntPtr = bdataCompare.Scan0
                        Dim intBlue2 As Integer = Marshal.ReadByte(pointerCompare, intOffset)
                        Dim intGreen2 As Integer = Marshal.ReadByte(pointerCompare, intOffset + 1)
                        Dim intRed2 As Integer = Marshal.ReadByte(pointerCompare, intOffset + 2)
                        DeleteObject(pointerCompare)

                        Dim decMotionWeight As Decimal
                        Dim intThisWeight As Integer = Math.Abs(intBlue2 - intBlue1)
                        If intThisWeight > Thresh Then decMotionWeight += intThisWeight
                        intThisWeight = Math.Abs(intGreen2 - intGreen1)
                        If intThisWeight > Thresh Then decMotionWeight += intThisWeight
                        intThisWeight = Math.Abs(intRed2 - intRed1)
                        If intThisWeight > Thresh Then decMotionWeight += intThisWeight
                        decMotionWeight *= CDec(0.0017)
                        'Dim decMotionWeight As Decimal = Math.Abs(intBlue2 - intBlue1)
                        'decMotionWeight += Math.Abs(intGreen2 - intGreen1)
                        'decMotionWeight += (Math.Abs(intRed2 - intRed1)) * CDec(0.0017)
                        '
                        decTotalWeight += decMotionWeight
                    Next
                Next

                Dim decMotionPercentage As Decimal = decTotalWeight / (bdataMaster.Width * bdataMaster.Height - 1)
                If decMotionPercentage < ImageThreshold Then
                    Return True
                Else : Return False
                End If
            End With
        Catch ex As Exception
            Trace.WriteLine("AppTools.CheckForSameness => " & ex.Message)
            Return False
        Finally
            If bdataMaster IsNot Nothing Then bitmapMaster.UnlockBits(bdataMaster)
            If bdataCompare IsNot Nothing Then bitmapCompare.UnlockBits(bdataCompare)
            'If bitmapMaster IsNot Nothing Then bitmapMaster.Dispose()
            'If bitmapCompare IsNot Nothing Then bitmapCompare.Dispose()
        End Try
    End Function
    Public Function CompareFrames(ByVal ThisFrame As FrameData, ByVal CompareFrame As FrameData) As Long
        Dim Wid As Integer = ThisFrame.ImageSize.Width
        Dim Hgt As Integer = ThisFrame.ImageSize.Height

        'COMPARE PIXELS
        Dim TotDelta As Long
        Dim filterthreshold As Integer = 5
        For intX As Integer = 0 To Wid - 1
            For intY As Integer = 0 To Hgt - 1
                Dim offset As Integer = ThisFrame.Stride * intY + (intX * 3)
                Dim ThisFramePixel As Integer = ThisFrame.Pixels(offset)
                Dim ThatFramePixel As Integer = CompareFrame.Pixels(offset)
                Dim PixelDelta As Integer = Math.Abs(ThisFramePixel - ThatFramePixel)
                If PixelDelta > filterthreshold Then TotDelta += PixelDelta
            Next
        Next

        'COMPUTE MEAN SQUARED ERROR
        Dim AviaMSE As Long = CLng(TotDelta / (Wid * Hgt))
        Debug.Print(AviaMSE.ToString)
        Return AviaMSE

    End Function

    Public Sub Delay(ByVal Milliseconds As Integer)
        Dim swatchTime As New Stopwatch
        swatchTime.Start()
        Do
            Try
                Application.DoEvents()
            Catch ex As Exception
            End Try
        Loop Until swatchTime.ElapsedMilliseconds >= Milliseconds
    End Sub
    Private Sub KillBW()
        TV.BackgroundWorker1.WorkerSupportsCancellation = True
        TV.BackgroundWorker1.WorkerReportsProgress = True
        Do Until BWBusy = False
            TV.BackgroundWorker1.CancelAsync()
            Application.DoEvents()
            Threading.Thread.Sleep(50)
            If Not AppTools.ActiveRun Then Exit Sub
        Loop
    End Sub
    Public Sub ExecuteOCR(ByVal SCName As String, ByVal Duration As Decimal, ByVal SCTop As Integer, ByVal SCLeft As Integer, ByVal SCWidth As Integer, ByVal SCHeight As Integer, ByVal SCType As String)
        'clear any existing files and any operating background worker
        DeleteAllSCFiles()
        KillBW()
        'set SC parameters
        AppTools.OCRText = ""
        CurrentSceneCheck.Name = SCName
        CurrentSceneCheck.Duration = Duration
        CurrentSceneCheck.Type = SCType
        Dim ThisPoint As New Point(SCLeft, SCTop)
        Dim ThisSize As New Size(SCWidth, SCHeight)
        Dim ThisRect As New Rectangle(ThisPoint, ThisSize)
        CurrentSceneCheck.Rect = ThisRect
        SCFrameCount = CInt(Duration * FrameRate)
        If Duration = 0.03 Then SCFrameCount = 1
        FramesTaken = 0
        AppTools.FoundFrame = 1000000
        CaptureFrames = True
        Dim ThatTime As Date = Now
        Dim ThisTime As Date
        'LOOP UNTIL ALL FRAMES ARE CAPTURED
        Do
            Application.DoEvents()
            ThisTime = Now
            If DateDiff("s", ThatTime, ThisTime) > Duration Then Exit Do
            If Not AppTools.ActiveRun Then Exit Do
        Loop Until CaptureFrames = False
        'II.Stop()
        'LOOP UNTIL BACKGROUNDWORKER IS DONE PROCESSING CAPTURED FRAMES
        Do
            Application.DoEvents()
            ThisTime = Now
            If DateDiff("s", ThatTime, ThisTime) > Duration Then Exit Do
            If Not AppTools.ActiveRun Then Exit Do
        Loop Until BWBusy = False
        'CLEAR VARIABLES
        CaptureFrames = False
        DeleteAllSCFiles()
        'SCDone = True

        If LCase(OCRSearchText) <> "anytext" Then
            If ActiveSceneFound = True Then
                Scripting_Engine.txtCommand.Text = "1"
            Else
                Scripting_Engine.txtCommand.Text = "0"
            End If
        Else
            Scripting_Engine.txtCommand.Text = AppTools.OCRText
        End If

    End Sub
    Public Sub ExecuteSceneCheck(ByVal SCName As String, ByVal Duration As Decimal, ByVal SCTop As Integer, ByVal SCLeft As Integer, ByVal SCWidth As Integer, ByVal SCHeight As Integer, ByVal SCType As String, ByVal SCSensitivity As Decimal)
        'II.Reset()
        'II.Start()
        KillBW()
        CurrentSceneCheck.Name = SCName
        CurrentSceneCheck.Duration = Duration
        CurrentSceneCheck.Type = SCType
        CurrentSceneCheck.Sensitivity = SCSensitivity
        Dim ThisPoint As New Point(SCLeft, SCTop)
        Dim ThisSize As New Size(SCWidth, SCHeight)
        Dim ThisRect As New Rectangle(ThisPoint, ThisSize)
        CurrentSceneCheck.Rect = ThisRect
        SCFrameCount = CInt(Duration * FrameRate)
        FramesTaken = 0
        AppTools.FoundFrame = 1000000
        CaptureFrames = True
        Dim ThatTime As Date = Now
        Dim ThisTime As Date
        'LOOP UNTIL ALL FRAMES ARE CAPTURED
        Do
            Application.DoEvents()
            ThisTime = Now
            If DateDiff("s", ThatTime, ThisTime) > Duration Then Exit Do
            If Not AppTools.ActiveRun Then Exit Do
        Loop Until CaptureFrames = False
        'II.Stop()
        'LOOP UNTIL BACKGROUNDWORKER IS DONE PROCESSING CAPTURED FRAMES
        Do
            Application.DoEvents()
            ThisTime = Now
            If DateDiff("s", ThatTime, ThisTime) > Duration Then Exit Do
            If Not AppTools.ActiveRun Then Exit Do
        Loop Until BWBusy = False
        'CLEAR VARIABLES
        CaptureFrames = False
        DeleteAllSCFiles()
        'SCDone = True

        If ActiveSceneFound Then
            Dim Timing As String
            If AppTools.FoundFrame <> 1000000 Then
                Timing = (FoundFrame / FrameRate).ToString("F2")
            Else : Timing = "NA"
            End If
            Scripting_Engine.txtCommand.Text = Timing
        Else
            Scripting_Engine.txtCommand.Text = "SC Failed"
        End If
    End Sub

    Private Function KillFile(ByVal ThisFile As String) As Boolean
        Try
            If File.Exists(ThisFile) Then File.Delete(ThisFile)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Sub DeleteAllSCFiles()
        'Dim OrigDirectory As String = MasterPath & "SCFrames\"
        Dim OrigDirectory As String = SCFramesDirectory
        For Each foundfile As String In My.Computer.FileSystem.GetFiles(OrigDirectory)
            If Not KillFile(foundfile) = True Then
                Delay(500)
                KillFile(foundfile)
            End If
            Application.DoEvents()
            If Not ActiveRun Then Exit For
        Next
        Do
            Dim counter = My.Computer.FileSystem.GetFiles(OrigDirectory)
            If counter.Count = 0 Then Exit Do
            Application.DoEvents()
            If Not ActiveRun Then Exit Do
            For Each foundfile As String In My.Computer.FileSystem.GetFiles(OrigDirectory)
                If Not KillFile(foundfile) = True Then
                    Delay(500)
                    KillFile(foundfile)
                End If
                Application.DoEvents()
                If Not ActiveRun Then Exit For
            Next
        Loop
    End Sub
    ''' <summary>
    ''' Release pointer object from memory
    ''' </summary>
    ''' <param name="hObject"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Runtime.InteropServices.DllImport("gdi32.dll")>
    Public Function DeleteObject(ByVal hObject As IntPtr) As Boolean
    End Function


End Module
