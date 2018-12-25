<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmButtonTailor
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmButtonTailor))
        Me.btnLeft = New System.Windows.Forms.Button()
        Me.btnRight = New System.Windows.Forms.Button()
        Me.btnDown = New System.Windows.Forms.Button()
        Me.btnUp = New System.Windows.Forms.Button()
        Me.btnFatter = New System.Windows.Forms.Button()
        Me.btnTaller = New System.Windows.Forms.Button()
        Me.btnShorter = New System.Windows.Forms.Button()
        Me.btnSkinnier = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnLeft
        '
        Me.btnLeft.BackgroundImage = CType(resources.GetObject("btnLeft.BackgroundImage"), System.Drawing.Image)
        Me.btnLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnLeft.Location = New System.Drawing.Point(12, 51)
        Me.btnLeft.Name = "btnLeft"
        Me.btnLeft.Size = New System.Drawing.Size(40, 44)
        Me.btnLeft.TabIndex = 0
        Me.btnLeft.UseVisualStyleBackColor = True
        '
        'btnRight
        '
        Me.btnRight.BackgroundImage = CType(resources.GetObject("btnRight.BackgroundImage"), System.Drawing.Image)
        Me.btnRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnRight.Location = New System.Drawing.Point(104, 51)
        Me.btnRight.Name = "btnRight"
        Me.btnRight.Size = New System.Drawing.Size(40, 44)
        Me.btnRight.TabIndex = 1
        Me.btnRight.UseVisualStyleBackColor = True
        '
        'btnDown
        '
        Me.btnDown.BackgroundImage = CType(resources.GetObject("btnDown.BackgroundImage"), System.Drawing.Image)
        Me.btnDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnDown.Location = New System.Drawing.Point(58, 92)
        Me.btnDown.Name = "btnDown"
        Me.btnDown.Size = New System.Drawing.Size(40, 44)
        Me.btnDown.TabIndex = 2
        Me.btnDown.UseVisualStyleBackColor = True
        '
        'btnUp
        '
        Me.btnUp.BackgroundImage = CType(resources.GetObject("btnUp.BackgroundImage"), System.Drawing.Image)
        Me.btnUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnUp.Location = New System.Drawing.Point(58, 12)
        Me.btnUp.Name = "btnUp"
        Me.btnUp.Size = New System.Drawing.Size(40, 44)
        Me.btnUp.TabIndex = 3
        Me.btnUp.UseVisualStyleBackColor = True
        '
        'btnFatter
        '
        Me.btnFatter.BackgroundImage = CType(resources.GetObject("btnFatter.BackgroundImage"), System.Drawing.Image)
        Me.btnFatter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnFatter.Location = New System.Drawing.Point(12, 151)
        Me.btnFatter.Name = "btnFatter"
        Me.btnFatter.Size = New System.Drawing.Size(132, 44)
        Me.btnFatter.TabIndex = 4
        Me.btnFatter.UseVisualStyleBackColor = True
        '
        'btnTaller
        '
        Me.btnTaller.BackgroundImage = CType(resources.GetObject("btnTaller.BackgroundImage"), System.Drawing.Image)
        Me.btnTaller.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnTaller.Location = New System.Drawing.Point(150, 12)
        Me.btnTaller.Name = "btnTaller"
        Me.btnTaller.Size = New System.Drawing.Size(44, 132)
        Me.btnTaller.TabIndex = 5
        Me.btnTaller.UseVisualStyleBackColor = True
        '
        'btnShorter
        '
        Me.btnShorter.BackgroundImage = CType(resources.GetObject("btnShorter.BackgroundImage"), System.Drawing.Image)
        Me.btnShorter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnShorter.Location = New System.Drawing.Point(200, 12)
        Me.btnShorter.Name = "btnShorter"
        Me.btnShorter.Size = New System.Drawing.Size(44, 132)
        Me.btnShorter.TabIndex = 6
        Me.btnShorter.UseVisualStyleBackColor = True
        '
        'btnSkinnier
        '
        Me.btnSkinnier.BackgroundImage = CType(resources.GetObject("btnSkinnier.BackgroundImage"), System.Drawing.Image)
        Me.btnSkinnier.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSkinnier.Location = New System.Drawing.Point(12, 201)
        Me.btnSkinnier.Name = "btnSkinnier"
        Me.btnSkinnier.Size = New System.Drawing.Size(132, 44)
        Me.btnSkinnier.TabIndex = 7
        Me.btnSkinnier.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(162, 161)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(65, 34)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(162, 201)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(65, 34)
        Me.btnSave.TabIndex = 9
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'frmButtonTailor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(253, 253)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSkinnier)
        Me.Controls.Add(Me.btnShorter)
        Me.Controls.Add(Me.btnTaller)
        Me.Controls.Add(Me.btnFatter)
        Me.Controls.Add(Me.btnUp)
        Me.Controls.Add(Me.btnDown)
        Me.Controls.Add(Me.btnRight)
        Me.Controls.Add(Me.btnLeft)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmButtonTailor"
        Me.Text = "Button Tailor"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnLeft As System.Windows.Forms.Button
    Friend WithEvents btnRight As System.Windows.Forms.Button
    Friend WithEvents btnDown As System.Windows.Forms.Button
    Friend WithEvents btnUp As System.Windows.Forms.Button
    Friend WithEvents btnFatter As System.Windows.Forms.Button
    Friend WithEvents btnTaller As System.Windows.Forms.Button
    Friend WithEvents btnShorter As System.Windows.Forms.Button
    Friend WithEvents btnSkinnier As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
End Class
