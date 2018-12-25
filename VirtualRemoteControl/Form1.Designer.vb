<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.pixRemoteControl = New System.Windows.Forms.PictureBox()
        Me.btnACOFF = New System.Windows.Forms.Button()
        Me.btnCC1 = New System.Windows.Forms.Button()
        Me.btnCC2 = New System.Windows.Forms.Button()
        Me.btnCC3 = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ctsMenuEllipse = New System.Windows.Forms.ToolStripMenuItem()
        Me.ctsMenuRectangle = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtResponse = New System.Windows.Forms.TextBox()
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AssociateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ButtonNameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tmrButtonFace = New System.Windows.Forms.Timer(Me.components)
        Me.btnViola = New System.Windows.Forms.Button()
        Me.cboDevice = New System.Windows.Forms.ComboBox()
        Me.btnACON = New System.Windows.Forms.Button()
        Me.cboxOnTop = New System.Windows.Forms.CheckBox()
        Me.cboxShowAll = New System.Windows.Forms.CheckBox()
        CType(Me.pixRemoteControl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'pixRemoteControl
        '
        Me.pixRemoteControl.BackColor = System.Drawing.Color.Transparent
        Me.pixRemoteControl.Image = CType(resources.GetObject("pixRemoteControl.Image"), System.Drawing.Image)
        Me.pixRemoteControl.Location = New System.Drawing.Point(10, 20)
        Me.pixRemoteControl.Name = "pixRemoteControl"
        Me.pixRemoteControl.Size = New System.Drawing.Size(140, 420)
        Me.pixRemoteControl.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pixRemoteControl.TabIndex = 0
        Me.pixRemoteControl.TabStop = False
        '
        'btnACOFF
        '
        Me.btnACOFF.BackColor = System.Drawing.Color.Transparent
        Me.btnACOFF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnACOFF.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnACOFF.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnACOFF.Location = New System.Drawing.Point(197, 58)
        Me.btnACOFF.Name = "btnACOFF"
        Me.btnACOFF.Size = New System.Drawing.Size(35, 26)
        Me.btnACOFF.TabIndex = 1
        Me.btnACOFF.Text = "OFF"
        Me.btnACOFF.UseVisualStyleBackColor = False
        '
        'btnCC1
        '
        Me.btnCC1.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCC1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCC1.Location = New System.Drawing.Point(160, 90)
        Me.btnCC1.Name = "btnCC1"
        Me.btnCC1.Size = New System.Drawing.Size(52, 26)
        Me.btnCC1.TabIndex = 2
        Me.btnCC1.Text = "CC1 ON"
        Me.btnCC1.UseVisualStyleBackColor = True
        '
        'btnCC2
        '
        Me.btnCC2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCC2.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCC2.Location = New System.Drawing.Point(160, 122)
        Me.btnCC2.Name = "btnCC2"
        Me.btnCC2.Size = New System.Drawing.Size(52, 26)
        Me.btnCC2.TabIndex = 3
        Me.btnCC2.Text = " CC2 ON"
        Me.btnCC2.UseVisualStyleBackColor = True
        '
        'btnCC3
        '
        Me.btnCC3.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCC3.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCC3.Location = New System.Drawing.Point(160, 154)
        Me.btnCC3.Name = "btnCC3"
        Me.btnCC3.Size = New System.Drawing.Size(52, 26)
        Me.btnCC3.TabIndex = 4
        Me.btnCC3.Text = "CC3  ON"
        Me.btnCC3.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ctsMenuEllipse, Me.ctsMenuRectangle})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(127, 48)
        '
        'ctsMenuEllipse
        '
        Me.ctsMenuEllipse.Name = "ctsMenuEllipse"
        Me.ctsMenuEllipse.Size = New System.Drawing.Size(126, 22)
        Me.ctsMenuEllipse.Text = "Circle"
        '
        'ctsMenuRectangle
        '
        Me.ctsMenuRectangle.Name = "ctsMenuRectangle"
        Me.ctsMenuRectangle.Size = New System.Drawing.Size(126, 22)
        Me.ctsMenuRectangle.Text = "Rectangle"
        '
        'txtResponse
        '
        Me.txtResponse.Location = New System.Drawing.Point(157, 410)
        Me.txtResponse.Name = "txtResponse"
        Me.txtResponse.Size = New System.Drawing.Size(76, 20)
        Me.txtResponse.TabIndex = 7
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeleteToolStripMenuItem, Me.AssociateToolStripMenuItem, Me.ButtonNameToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(143, 70)
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.DeleteToolStripMenuItem.Text = "Delete"
        '
        'AssociateToolStripMenuItem
        '
        Me.AssociateToolStripMenuItem.Name = "AssociateToolStripMenuItem"
        Me.AssociateToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.AssociateToolStripMenuItem.Text = "Associate"
        '
        'ButtonNameToolStripMenuItem
        '
        Me.ButtonNameToolStripMenuItem.Name = "ButtonNameToolStripMenuItem"
        Me.ButtonNameToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.ButtonNameToolStripMenuItem.Text = "ButtonName"
        '
        'tmrButtonFace
        '
        Me.tmrButtonFace.Interval = 200
        '
        'btnViola
        '
        Me.btnViola.Location = New System.Drawing.Point(-10, -3)
        Me.btnViola.Name = "btnViola"
        Me.btnViola.Size = New System.Drawing.Size(266, 11)
        Me.btnViola.TabIndex = 9
        Me.btnViola.UseVisualStyleBackColor = True
        '
        'cboDevice
        '
        Me.cboDevice.FormattingEnabled = True
        Me.cboDevice.Location = New System.Drawing.Point(156, 20)
        Me.cboDevice.Name = "cboDevice"
        Me.cboDevice.Size = New System.Drawing.Size(81, 21)
        Me.cboDevice.TabIndex = 13
        '
        'btnACON
        '
        Me.btnACON.BackColor = System.Drawing.Color.Transparent
        Me.btnACON.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnACON.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnACON.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnACON.Location = New System.Drawing.Point(160, 58)
        Me.btnACON.Name = "btnACON"
        Me.btnACON.Size = New System.Drawing.Size(35, 26)
        Me.btnACON.TabIndex = 14
        Me.btnACON.Text = "ON"
        Me.btnACON.UseVisualStyleBackColor = False
        '
        'cboxOnTop
        '
        Me.cboxOnTop.AutoSize = True
        Me.cboxOnTop.Checked = True
        Me.cboxOnTop.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cboxOnTop.Location = New System.Drawing.Point(160, 387)
        Me.cboxOnTop.Name = "cboxOnTop"
        Me.cboxOnTop.Size = New System.Drawing.Size(62, 17)
        Me.cboxOnTop.TabIndex = 15
        Me.cboxOnTop.Text = "On Top"
        Me.cboxOnTop.UseVisualStyleBackColor = True
        '
        'cboxShowAll
        '
        Me.cboxShowAll.AutoSize = True
        Me.cboxShowAll.Location = New System.Drawing.Point(160, 364)
        Me.cboxShowAll.Name = "cboxShowAll"
        Me.cboxShowAll.Size = New System.Drawing.Size(92, 17)
        Me.cboxShowAll.TabIndex = 16
        Me.cboxShowAll.Text = "Show Buttons"
        Me.cboxShowAll.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(250, 442)
        Me.Controls.Add(Me.cboxShowAll)
        Me.Controls.Add(Me.cboxOnTop)
        Me.Controls.Add(Me.btnACON)
        Me.Controls.Add(Me.cboDevice)
        Me.Controls.Add(Me.btnViola)
        Me.Controls.Add(Me.txtResponse)
        Me.Controls.Add(Me.btnCC3)
        Me.Controls.Add(Me.btnCC2)
        Me.Controls.Add(Me.btnCC1)
        Me.Controls.Add(Me.btnACOFF)
        Me.Controls.Add(Me.pixRemoteControl)
        Me.Name = "Form1"
        Me.Text = "IR/IP"
        Me.TransparencyKey = System.Drawing.SystemColors.ActiveCaption
        CType(Me.pixRemoteControl,System.ComponentModel.ISupportInitialize).EndInit
        Me.ContextMenuStrip1.ResumeLayout(false)
        Me.ContextMenuStrip2.ResumeLayout(false)
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents pixRemoteControl As System.Windows.Forms.PictureBox
    Friend WithEvents btnACOFF As System.Windows.Forms.Button
    Friend WithEvents btnCC1 As System.Windows.Forms.Button
    Friend WithEvents btnCC2 As System.Windows.Forms.Button
    Friend WithEvents btnCC3 As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ctsMenuEllipse As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctsMenuRectangle As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtResponse As System.Windows.Forms.TextBox
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents DeleteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AssociateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmrButtonFace As System.Windows.Forms.Timer
    Friend WithEvents btnViola As System.Windows.Forms.Button
    Friend WithEvents cboDevice As System.Windows.Forms.ComboBox
    Friend WithEvents btnACON As System.Windows.Forms.Button
    Friend WithEvents cboxOnTop As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonNameToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cboxShowAll As System.Windows.Forms.CheckBox

End Class
