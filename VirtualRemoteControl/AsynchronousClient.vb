Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.Text
Public Class AsynchronousClient
    Public Event MessageReceived(ByVal Data As String)
    ' ManualResetEvent instances signal completion.
    Private Shared connectDone As New ManualResetEvent(False)
    Private Shared sendDone As New ManualResetEvent(False)
    Private Shared receiveDone As New ManualResetEvent(False)
    Private Shared EOF As String = vbNullString
    Private TmrThrd As Thread

    ' The response from the remote device.
    Private response As String = String.Empty
    Private TCPTimeout As Integer
    Private TimeDone As Boolean
    Private KillTimer As Boolean

    Public Sub Main(ByVal strAddress As String, ByVal Port As Integer, ByVal Msg As String, ByVal decTimeout As Decimal, ByVal EOFString As String)
        ' Establish the remote endpoint for the socket.
        connectDone.Reset()
        sendDone.Reset()
        receiveDone.Reset()
        EOF = LCase(EOFString)
        response = String.Empty

        'Dim ipHostInfo As IPHostEntry = Dns.Resolve(Dns.GetHostName())
        Dim ipAddress As IPAddress = ipAddress.Parse(Trim(strAddress))
        Dim remoteEP As New IPEndPoint(ipAddress, Port)

        ' Create a TCP/IP socket.
        Dim client As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)

        ' Connect to the remote endpoint.
        client.BeginConnect(remoteEP, New AsyncCallback(AddressOf ConnectCallback), client)

        ' Wait for connect.
        connectDone.WaitOne()

        ' Send test data to the remote device.
        Send(client, Msg)
        sendDone.WaitOne()

        KillTimer = False
        TCPTimeout = CInt(decTimeout * 1000)
        TimeDone = False
        If decTimeout <> 0 Then
            TmrThrd = New Thread(AddressOf ThreadTimer)
            TmrThrd.IsBackground = True
            TmrThrd.Start()
        End If

        Receive(client)
        receiveDone.WaitOne()

        ' Raise evnet
        RaiseEvent MessageReceived(response)


        ' Release the socket.
        client.Shutdown(SocketShutdown.Both)
        client.Close()

    End Sub 'Main

    Public Sub ThreadTimer()
        Dim StartTimer As New Stopwatch
        StartTimer.Start()
        Do
            Application.DoEvents()
            Thread.Sleep(20)
            If KillTimer Then
                StartTimer.Reset()
                Exit Sub
            End If
        Loop Until StartTimer.ElapsedMilliseconds > TCPTimeout
        TimeDone = True
        response = "<<Timeout>>" & vbCrLf & response
        receiveDone.Set()
    End Sub

    Private Shared Sub ConnectCallback(ByVal ar As IAsyncResult)
        ' Retrieve the socket from the state object.
        Dim client As Socket = CType(ar.AsyncState, Socket)

        ' Complete the connection.
        client.EndConnect(ar)

        Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString())

        ' Signal that the connection has been made.
        connectDone.Set()
    End Sub 'ConnectCallback


    Private Sub Receive(ByVal client As Socket)
        ' Create the state object.
        Dim state As New StateObject
        state.workSocket = client
        state.sb.Clear()

        ' Begin receiving the data from the remote device.
        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, New AsyncCallback(AddressOf ReceiveCallback), state)

    End Sub 'Receive


    Private Sub ReceiveCallback(ByVal ar As IAsyncResult)
        If TimeDone = True Then Exit Sub

        ' Retrieve the state object and the client socket 
        ' from the asynchronous state object.
        Dim state As StateObject = CType(ar.AsyncState, StateObject)
        Dim client As Socket = state.workSocket

        ' Read data from the remote device.
        Dim bytesRead As Integer = client.EndReceive(ar)

        If bytesRead > 0 Then
            ' There might be more data, so store the data received so far.
            state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead))
            If state.sb.Length > 1 Then response = state.sb.ToString()

            'If EOF is found, then Signal all bytes have been received and bail
            If Len(Trim(EOF)) > 0 Then
                If InStr(LCase(state.sb.ToString), EOF) <> 0 Then
                    KillTimer = True
                    receiveDone.Set()
                    Exit Sub
                End If
            End If

            ' Get the rest of the data.
            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, New AsyncCallback(AddressOf ReceiveCallback), state)
        Else
            ' All the data has arrived; put it in response.
            If state.sb.Length > 1 Then response = state.sb.ToString()
            ' Signal that all bytes have been received.
            KillTimer = True
            receiveDone.Set()
        End If
    End Sub 'ReceiveCallback


    Private Shared Sub Send(ByVal client As Socket, ByVal data As String)
        ' Convert the string data to byte data using ASCII encoding.
        Dim byteData As Byte() = Encoding.ASCII.GetBytes(data)

        ' Begin sending the data to the remote device.
        client.BeginSend(byteData, 0, byteData.Length, 0, New AsyncCallback(AddressOf SendCallback), client)
    End Sub 'Send


    Private Shared Sub SendCallback(ByVal ar As IAsyncResult)
        ' Retrieve the socket from the state object.
        Dim client As Socket = CType(ar.AsyncState, Socket)

        ' Complete sending the data to the remote device.
        Dim bytesSent As Integer = client.EndSend(ar)
        Console.WriteLine("Sent {0} bytes to server.", bytesSent)

        ' Signal that all bytes have been sent.
        sendDone.Set()
    End Sub 'SendCallback
End Class
