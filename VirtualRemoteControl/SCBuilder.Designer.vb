<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SCBuilder
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
        Me.pboxScene = New System.Windows.Forms.PictureBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.txtSCName = New System.Windows.Forms.TextBox()
        Me.lblSCName = New System.Windows.Forms.Label()
        Me.rbImage = New System.Windows.Forms.RadioButton()
        Me.rbMotion = New System.Windows.Forms.RadioButton()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.SceneCheckToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TypeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MotionDetectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageDetectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TextReconitionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.rbOCR = New System.Windows.Forms.RadioButton()
        Me.lblSCText = New System.Windows.Forms.Label()
        Me.lblSCConfidence = New System.Windows.Forms.Label()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.txtSCText = New System.Windows.Forms.RichTextBox()
        Me.ViewSceneChecks = New System.Windows.Forms.ListBox()
        Me.VBScriptInstructionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.pboxScene, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pboxScene
        '
        Me.pboxScene.Location = New System.Drawing.Point(0, 27)
        Me.pboxScene.Name = "pboxScene"
        Me.pboxScene.Size = New System.Drawing.Size(263, 208)
        Me.pboxScene.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.pboxScene.TabIndex = 0
        Me.pboxScene.TabStop = False
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(12, 241)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(68, 22)
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'txtSCName
        '
        Me.txtSCName.Location = New System.Drawing.Point(254, 246)
        Me.txtSCName.Name = "txtSCName"
        Me.txtSCName.Size = New System.Drawing.Size(113, 20)
        Me.txtSCName.TabIndex = 2
        '
        'lblSCName
        '
        Me.lblSCName.AutoSize = True
        Me.lblSCName.Location = New System.Drawing.Point(207, 249)
        Me.lblSCName.Name = "lblSCName"
        Me.lblSCName.Size = New System.Drawing.Size(41, 13)
        Me.lblSCName.TabIndex = 3
        Me.lblSCName.Text = "Name: "
        '
        'rbImage
        '
        Me.rbImage.AutoSize = True
        Me.rbImage.Checked = True
        Me.rbImage.Location = New System.Drawing.Point(90, 246)
        Me.rbImage.Name = "rbImage"
        Me.rbImage.Size = New System.Drawing.Size(89, 17)
        Me.rbImage.TabIndex = 4
        Me.rbImage.TabStop = True
        Me.rbImage.Text = "Image Detect"
        Me.rbImage.UseVisualStyleBackColor = True
        '
        'rbMotion
        '
        Me.rbMotion.AutoSize = True
        Me.rbMotion.Location = New System.Drawing.Point(90, 294)
        Me.rbMotion.Name = "rbMotion"
        Me.rbMotion.Size = New System.Drawing.Size(92, 17)
        Me.rbMotion.TabIndex = 5
        Me.rbMotion.Text = "Motion Detect"
        Me.rbMotion.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SceneCheckToolStripMenuItem, Me.VBScriptInstructionsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(567, 24)
        Me.MenuStrip1.TabIndex = 6
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'SceneCheckToolStripMenuItem
        '
        Me.SceneCheckToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.LoadToolStripMenuItem, Me.TypeToolStripMenuItem})
        Me.SceneCheckToolStripMenuItem.Name = "SceneCheckToolStripMenuItem"
        Me.SceneCheckToolStripMenuItem.Size = New System.Drawing.Size(86, 20)
        Me.SceneCheckToolStripMenuItem.Text = "Scene Check"
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(100, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'LoadToolStripMenuItem
        '
        Me.LoadToolStripMenuItem.Name = "LoadToolStripMenuItem"
        Me.LoadToolStripMenuItem.Size = New System.Drawing.Size(100, 22)
        Me.LoadToolStripMenuItem.Text = "Load"
        '
        'TypeToolStripMenuItem
        '
        Me.TypeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MotionDetectionToolStripMenuItem, Me.ImageDetectionToolStripMenuItem, Me.TextReconitionToolStripMenuItem})
        Me.TypeToolStripMenuItem.Name = "TypeToolStripMenuItem"
        Me.TypeToolStripMenuItem.Size = New System.Drawing.Size(100, 22)
        Me.TypeToolStripMenuItem.Text = "Type"
        '
        'MotionDetectionToolStripMenuItem
        '
        Me.MotionDetectionToolStripMenuItem.Name = "MotionDetectionToolStripMenuItem"
        Me.MotionDetectionToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.MotionDetectionToolStripMenuItem.Text = "Motion Detection"
        '
        'ImageDetectionToolStripMenuItem
        '
        Me.ImageDetectionToolStripMenuItem.Name = "ImageDetectionToolStripMenuItem"
        Me.ImageDetectionToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.ImageDetectionToolStripMenuItem.Text = "Image Detection"
        '
        'TextReconitionToolStripMenuItem
        '
        Me.TextReconitionToolStripMenuItem.Name = "TextReconitionToolStripMenuItem"
        Me.TextReconitionToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.TextReconitionToolStripMenuItem.Text = "Text Reconition"
        '
        'rbOCR
        '
        Me.rbOCR.AutoSize = True
        Me.rbOCR.Location = New System.Drawing.Point(90, 269)
        Me.rbOCR.Name = "rbOCR"
        Me.rbOCR.Size = New System.Drawing.Size(46, 17)
        Me.rbOCR.TabIndex = 7
        Me.rbOCR.Text = "Text"
        Me.rbOCR.UseVisualStyleBackColor = True
        '
        'lblSCText
        '
        Me.lblSCText.AutoSize = True
        Me.lblSCText.Location = New System.Drawing.Point(207, 275)
        Me.lblSCText.Name = "lblSCText"
        Me.lblSCText.Size = New System.Drawing.Size(31, 13)
        Me.lblSCText.TabIndex = 9
        Me.lblSCText.Text = "Text:"
        '
        'lblSCConfidence
        '
        Me.lblSCConfidence.AutoSize = True
        Me.lblSCConfidence.Location = New System.Drawing.Point(207, 298)
        Me.lblSCConfidence.Name = "lblSCConfidence"
        Me.lblSCConfidence.Size = New System.Drawing.Size(64, 13)
        Me.lblSCConfidence.TabIndex = 10
        Me.lblSCConfidence.Text = "Confidence:"
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(487, 289)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(68, 22)
        Me.btnBrowse.TabIndex = 13
        Me.btnBrowse.Text = "Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        Me.btnBrowse.Visible = False
        '
        'txtSCText
        '
        Me.txtSCText.Location = New System.Drawing.Point(254, 272)
        Me.txtSCText.Name = "txtSCText"
        Me.txtSCText.Size = New System.Drawing.Size(113, 39)
        Me.txtSCText.TabIndex = 16
        Me.txtSCText.Text = ""
        Me.txtSCText.WordWrap = False
        '
        'ViewSceneChecks
        '
        Me.ViewSceneChecks.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ViewSceneChecks.FormattingEnabled = True
        Me.ViewSceneChecks.ItemHeight = 19
        Me.ViewSceneChecks.Location = New System.Drawing.Point(12, 36)
        Me.ViewSceneChecks.Name = "ViewSceneChecks"
        Me.ViewSceneChecks.Size = New System.Drawing.Size(181, 194)
        Me.ViewSceneChecks.TabIndex = 19
        Me.ViewSceneChecks.Visible = False
        '
        'VBScriptInstructionsToolStripMenuItem
        '
        Me.VBScriptInstructionsToolStripMenuItem.Name = "VBScriptInstructionsToolStripMenuItem"
        Me.VBScriptInstructionsToolStripMenuItem.Size = New System.Drawing.Size(128, 20)
        Me.VBScriptInstructionsToolStripMenuItem.Text = "VBScript Instructions"
        '
        'SCBuilder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(567, 316)
        Me.Controls.Add(Me.ViewSceneChecks)
        Me.Controls.Add(Me.txtSCText)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.lblSCConfidence)
        Me.Controls.Add(Me.lblSCText)
        Me.Controls.Add(Me.rbOCR)
        Me.Controls.Add(Me.rbMotion)
        Me.Controls.Add(Me.rbImage)
        Me.Controls.Add(Me.lblSCName)
        Me.Controls.Add(Me.txtSCName)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.pboxScene)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "SCBuilder"
        Me.Text = "SCBuilder"
        CType(Me.pboxScene, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pboxScene As System.Windows.Forms.PictureBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents txtSCName As System.Windows.Forms.TextBox
    Friend WithEvents lblSCName As System.Windows.Forms.Label
    Friend WithEvents rbImage As System.Windows.Forms.RadioButton
    Friend WithEvents rbMotion As System.Windows.Forms.RadioButton
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents SceneCheckToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoadToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TypeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MotionDetectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImageDetectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rbOCR As System.Windows.Forms.RadioButton
    Friend WithEvents TextReconitionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblSCText As System.Windows.Forms.Label
    Friend WithEvents lblSCConfidence As System.Windows.Forms.Label
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents txtSCText As System.Windows.Forms.RichTextBox
    Friend WithEvents ViewSceneChecks As System.Windows.Forms.ListBox
    Friend WithEvents VBScriptInstructionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
