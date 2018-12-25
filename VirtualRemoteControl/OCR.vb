Imports Tesseract
Imports System.IO
Public Structure OCRResult
    Dim Text As String
    Dim Confidence As String
End Structure
Public Class OCR

    Public Shared Function ReadText(ByVal Source As Bitmap) As OCRResult
        Try
            Dim ThisOCRResult As OCRResult
            Dim SizedBMP As New Bitmap(Source, New Size(CInt(Source.Width * 2.2), CInt(Source.Height * 1.5)))

            Using engine = New TesseractEngine(Application.StartupPath & "tessdata", "eng", EngineMode.Default)
                ' have to load Pix via a bitmap since Pix doesn't support loading a stream.
                Using image = New System.Drawing.Bitmap(SizedBMP)
                    Using pix = PixConverter.ToPix(image)
                        Using page = engine.Process(pix)
                            ThisOCRResult.Text = page.GetText()
                            ThisOCRResult.Confidence = [String].Format("{0:P}", page.GetMeanConfidence())
                        End Using
                    End Using
                End Using
            End Using
            SizedBMP.Dispose()

            Return ThisOCRResult
        Catch ex As Exception
            Throw New Exception("Could not Derive Text from ImgFile")
        Finally

        End Try


    End Function

End Class
