Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.Text

Public Class StateObject
    ' Client socket.
    Public workSocket As Socket = Nothing
    ' Size of receive buffer.
    Public Const BufferSize As Integer = 256
    ' Receive buffer.
    Public buffer(BufferSize) As Byte
    ' Received data string.
    Public sb As New StringBuilder
End Class

