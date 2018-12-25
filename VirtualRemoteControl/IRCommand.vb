Imports System.Text
Imports System.Net.Sockets

'Declaration

Public Class IRCommand : Implements IDisposable

    Private SimulStream As System.Net.Sockets.NetworkStream

    Public Sub SendCommand(ByVal IRCommand As String, ByVal ThisAddress As String)
        If InStr(ThisAddress, ":") = 0 Then ThisAddress = ThisAddress & ":4998"
        Dim ThisSimulSocket As New SimulSockets
        Try

            For indexSimulSocket As Integer = 0 To lstIRSocketsAll.Count - 1
                ThisSimulSocket = lstIRSocketsAll(indexSimulSocket)
                With ThisSimulSocket
                    Dim FullAddress As String = .Address & ":" & .Port
                    If FullAddress = ThisAddress Then
                        If Not .Sock.Connected Then
                            AppTools.SelectMultipleDevices()
                        End If
                        If .Sock.Connected Then
                            ActiveIRSockets += 1
                            Dim OutSimulSTream As Byte() = Encoding.ASCII.GetBytes(IRCommand)
                            .Sock.BeginSend(OutSimulSTream, 0, OutSimulSTream.Length, SocketFlags.None, New AsyncCallback(AddressOf WriteCallBack), .Sock)
                        End If
                        Exit For
                    End If
                End With
            Next
        Catch ex As Exception
            Debug.WriteLine("IRCommand.SendCommand => " & ex.Message)
        End Try

    End Sub

    Private Sub WriteCallBack(ByVal ar As IAsyncResult)
        ActiveIRSockets -= 1
        Try
            Dim remote As Socket = CType(ar.AsyncState, Socket)
            Dim result As Integer = remote.EndSend(ar)
        Catch ex As Exception
            Debug.WriteLine("IRCommand.WriteCallBack => " & ex.Message)
        End Try
    End Sub


#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
