Imports System.Threading

Public Class frmButtonTailor

    Private Resizing As Boolean

    Private Sub frmButtonTailor_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        Dim i = 0
    End Sub

    Private Sub frmButtonTailor_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Form1.pixRemoteControl.Refresh()
    End Sub

    Private Sub btnUp_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnUp.MouseDown
        Resizing = True
        Do
            Form1.ButtonY -= 1
            Form1.TempButton.decRelativeTop = CDec(Form1.ButtonY / Form1.PixHeight)
            'Form1.TempButton.ptLocation.Y -= 1
            Thread.Sleep(100)
            Form1.DrawButton(Form1.TempButton)
            Application.DoEvents()
        Loop Until Resizing = False
    End Sub

    Private Sub btnUp_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnUp.MouseUp
        Resizing = False
    End Sub

    Private Sub btnDown_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnDown.MouseDown
        Resizing = True
        Do
            Form1.ButtonY += 1
            Form1.TempButton.decRelativeTop = CDec(Form1.ButtonY / Form1.PixHeight)
            'Form1.TempButton.ptLocation.Y += 1
            Thread.Sleep(100)
            Form1.DrawButton(Form1.TempButton)
            Application.DoEvents()
        Loop Until Resizing = False
    End Sub

    Private Sub btnDown_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnDown.MouseUp
        Resizing = False
    End Sub

    Private Sub btnLeft_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnLeft.MouseDown
        Resizing = True
        Do
            Form1.ButtonX -= 1
            Form1.TempButton.decRelativeLeft = CDec(Form1.ButtonX / Form1.PixWidth)
            'Form1.TempButton.ptLocation.X -= 1
            Threading.Thread.Sleep(100)
            Form1.DrawButton(Form1.TempButton)
            Application.DoEvents()
        Loop Until Resizing = False
    End Sub

    Private Sub btnLeft_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnLeft.MouseUp
        Resizing = False
    End Sub

    Private Sub btnRight_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnRight.MouseDown
        Resizing = True
        Do
            Form1.ButtonX += 1
            Form1.TempButton.decRelativeLeft = CDec(Form1.ButtonX / Form1.PixWidth)
            'Form1.TempButton.ptLocation.X += 1
            Threading.Thread.Sleep(100)
            Form1.DrawButton(Form1.TempButton)
            Application.DoEvents()
        Loop Until Resizing = False
    End Sub

    Private Sub btnRight_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnRight.MouseUp
        Resizing = False
    End Sub

    Private Sub btnTaller_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnTaller.MouseDown
        Resizing = True
        Do
            Form1.ButtonY -= 1
            Form1.ButtonHeight += 2
            Form1.TempButton.decRelativeTop = CDec(Form1.ButtonY / Form1.PixHeight)
            Form1.TempButton.decRelativeHeight = CDec(Form1.ButtonHeight / Form1.PixHeight)
            'Form1.TempButton.intHeight -= 2
            'Form1.TempButton.ptLocation.Y += 1
            Threading.Thread.Sleep(100)
            Form1.DrawButton(Form1.TempButton)
            Application.DoEvents()
        Loop Until Resizing = False

    End Sub

    Private Sub btnTaller_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnTaller.MouseUp
        Resizing = False
    End Sub

    Private Sub btnFatter_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnFatter.MouseDown
        Resizing = True
        Do
            Form1.ButtonX -= 1
            Form1.ButtonWidth += 2
            'Form1.TempButton.decRelativeTop = CDec(Form1.ButtonY / Form1.pixRemoteControl.Height)
            Form1.TempButton.decRelativeWidth = CDec(Form1.ButtonWidth / Form1.PixWidth)
            Form1.TempButton.decRelativeLeft = CDec(Form1.ButtonX / Form1.PixWidth)
            Threading.Thread.Sleep(100)
            Form1.DrawButton(Form1.TempButton)
            Application.DoEvents()
        Loop Until Resizing = False
    End Sub

    Private Sub btnShorter_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnShorter.MouseDown
        Resizing = True
        Do
            Form1.ButtonY += 1
            Form1.ButtonHeight -= 2
            Form1.TempButton.decRelativeTop = CDec(Form1.ButtonY / Form1.pixRemoteControl.Height)
            Form1.TempButton.decRelativeHeight = CDec(Form1.ButtonHeight / Form1.PixHeight)
            'Form1.TempButton.intHeight += 2
            'Form1.TempButton.ptLocation.Y -= 1
            Threading.Thread.Sleep(100)
            Form1.DrawButton(Form1.TempButton)
            Application.DoEvents()
        Loop Until Resizing = False
    End Sub

    Private Sub btnShorter_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnShorter.MouseUp
        Resizing = False
    End Sub

    Private Sub btnSkinnier_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnSkinnier.MouseDown
        Resizing = True
        Do
            Form1.ButtonX += 1
            Form1.ButtonWidth -= 2
            Form1.TempButton.decRelativeLeft = CDec(Form1.ButtonX / Form1.PixWidth)
            Form1.TempButton.decRelativeWidth = CDec(Form1.ButtonWidth / Form1.PixWidth)
            Threading.Thread.Sleep(100)
            Form1.DrawButton(Form1.TempButton)
            Application.DoEvents()
        Loop Until Resizing = False
    End Sub

    Private Sub btnSkinnier_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnSkinnier.MouseUp
        Resizing = False
    End Sub

    Private Sub btnFatter_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnFatter.MouseUp
        Resizing = False
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Form1.pixRemoteControl.Refresh()
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        Form1.listButton.Add(Form1.TempButton)
        Form1.pixRemoteControl.Refresh()
        Form1.SaveButtonList()
        Me.Close()
    End Sub

    Private Sub btnSkinnier_Click(sender As System.Object, e As System.EventArgs) Handles btnSkinnier.Click

    End Sub

    Private Sub btnFatter_Click(sender As System.Object, e As System.EventArgs) Handles btnFatter.Click

    End Sub

    Private Sub frmButtonTailor_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.ShowInTaskbar = False
    End Sub
End Class