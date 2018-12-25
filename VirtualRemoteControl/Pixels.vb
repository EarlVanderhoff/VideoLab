Option Strict On
Option Explicit On

Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices

Public Structure FrameData
    Public Property Pixels As Byte()
    Public Property Stride As Integer
    Public Property ImageSize As Size
    Public Property FrameNo As Integer
End Structure

Public Class Pixels
    ''' <summary>
    ''' Extract pixel colors to a one-dimensional array.
    ''' </summary>
    ''' <param name="source"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function WAS_ExtractPixels(ByVal Source As Bitmap) As FrameData
        If Source Is Nothing Then Throw New ArgumentNullException("Source bitmap is empty")
        Dim NewSize As Size
        NewSize.Width = Source.Width
        NewSize.Height = Source.Height
        Dim ImageSize As New Rectangle(New Point, NewSize)
        Dim dataBitmap As BitmapData = Nothing
        Dim pixels() As Byte

        Try
            dataBitmap = Source.LockBits(ImageSize, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb)
            ReDim pixels(dataBitmap.Stride * (dataBitmap.Height - (ImageSize.Y * 2)))
            Marshal.Copy(dataBitmap.Scan0, pixels, 0, pixels.Length)

            Dim result As New FrameData With {.Pixels = pixels,
                                              .Stride = dataBitmap.Stride,
                                              .ImageSize = New Size(ImageSize.Size.Width, ImageSize.Size.Height)}
            Source.UnlockBits(dataBitmap)

            Return result
        Catch ex As Exception
            Throw New Exception("An exception occured while extracting pixels from the source bitmap.", ex)
        End Try
    End Function
    ''' <summary>
    ''' Extract pixel colors to a one-dimensional array.
    ''' </summary>
    ''' <param name="source"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ExtractPixels(ByVal Source As Bitmap) As FrameData
        If Source Is Nothing Then Throw New ArgumentNullException("Source bitmap is empty")
        Dim NewSize As Size
        NewSize.Width = Source.Width
        NewSize.Height = Source.Height
        Dim ImageSize As New Rectangle(New Point, NewSize)
        Dim dataBitmap As BitmapData = Nothing
        Dim pixels() As Byte

        Try
            dataBitmap = Source.LockBits(ImageSize, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb)
            ReDim pixels(dataBitmap.Stride * (dataBitmap.Height - (ImageSize.Y * 2)))
            Marshal.Copy(dataBitmap.Scan0, pixels, 0, pixels.Length)

            Dim result As New FrameData With {.Pixels = pixels,
                                              .Stride = dataBitmap.Stride,
                                              .ImageSize = New Size(ImageSize.Size.Width, ImageSize.Size.Height)}
            'Dim Test As Boolean
            'If Test Then
            '    Source.Save("C:\Cropped.jpg")
            '    Dim bmp As New Bitmap(NewSize.Width, NewSize.Height)
            '    Dim dataReturn As BitmapData
            '    dataReturn = bmp.LockBits(ImageSize, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb)
            '    Dim PointerReturn As IntPtr = dataReturn.Scan0
            '    For intN As Integer = 0 To NewSize.Width * NewSize.Height
            '        Dim intx As Integer = CInt(Math.Truncate(intN / NewSize.Height))
            '        Dim inty As Integer = intN - (intx * NewSize.Height)
            '        Dim intOffset As Integer = dataReturn.Stride * inty + (3 * intx)
            '        Dim bytByte As Byte = result.Pixels(intOffset)
            '        Marshal.WriteByte(PointerReturn, intOffset, bytByte)
            '        Marshal.WriteByte(PointerReturn, intOffset + 1, bytByte)
            '        Marshal.WriteByte(PointerReturn, intOffset + 2, bytByte)
            '    Next
            '    bmp.UnlockBits(dataReturn)
            '    bmp.Save("C:\FrameData.jpg")
            'End If

            Source.UnlockBits(dataBitmap)

            Return result
        Catch ex As Exception
            Throw New Exception("An exception occured while extracting pixels from the source bitmap.", ex)
        End Try
    End Function

    ''' <summary>
    ''' Create a bitmap from a pixel array
    ''' </summary>
    ''' <param name="data"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CreateBitmap(ByVal data As FrameData) As Bitmap
        Try
            ' Create the return bitmap with the same size as the source
            Using result = New Bitmap(data.ImageSize.Width, data.ImageSize.Height, PixelFormat.Format24bppRgb)
                ' Copy the pixel array to the return bitmap
                Dim dataBitmap As BitmapData = result.LockBits(New Rectangle(0, 0, result.Width, result.Height), ImageLockMode.WriteOnly, result.PixelFormat)
                Marshal.Copy(data.Pixels, 0, dataBitmap.Scan0, data.Pixels.Length)
                result.UnlockBits(dataBitmap)
                Return DirectCast(result.Clone, Bitmap)
            End Using
        Catch ex As Exception
            Throw New Exception("An exception occured while creating the bitmap", ex)
        End Try
    End Function
End Class
