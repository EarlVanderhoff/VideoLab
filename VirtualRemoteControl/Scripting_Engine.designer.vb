<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Scripting_Engine
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Scripting_Engine))
        Me.ScriptControl1 = New AxMSScriptControl.AxScriptControl()
        Me.txtCommand = New System.Windows.Forms.TextBox()
        Me.btnRun = New System.Windows.Forms.Button()
        Me.rtbCode = New System.Windows.Forms.RichTextBox()
        Me.cmsTxtPopup = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Copy = New System.Windows.Forms.ToolStripMenuItem()
        Me.Paste = New System.Windows.Forms.ToolStripMenuItem()
        Me.Cut = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblCurrentCommand = New System.Windows.Forms.Label()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.VBScriptingSchoolToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DelayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FTPAddressToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuScript = New System.Windows.Forms.MenuStrip()
        Me.txtResponses = New System.Windows.Forms.TextBox()
        Me.cboxInbox = New System.Windows.Forms.CheckBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.ScriptControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmsTxtPopup.SuspendLayout()
        Me.mnuScript.SuspendLayout()
        Me.SuspendLayout()
        '
        'ScriptControl1
        '
        Me.ScriptControl1.Enabled = True
        Me.ScriptControl1.Location = New System.Drawing.Point(240, 12)
        Me.ScriptControl1.Name = "ScriptControl1"
        Me.ScriptControl1.OcxState = CType(resources.GetObject("ScriptControl1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.ScriptControl1.Size = New System.Drawing.Size(38, 38)
        Me.ScriptControl1.TabIndex = 0
        '
        'txtCommand
        '
        Me.txtCommand.Location = New System.Drawing.Point(266, 3)
        Me.txtCommand.Name = "txtCommand"
        Me.txtCommand.Size = New System.Drawing.Size(270, 20)
        Me.txtCommand.TabIndex = 2
        Me.txtCommand.Visible = False
        '
        'btnRun
        '
        Me.btnRun.Location = New System.Drawing.Point(198, 3)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(50, 21)
        Me.btnRun.TabIndex = 4
        Me.btnRun.Text = "Run"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'rtbCode
        '
        Me.rtbCode.AcceptsTab = True
        Me.rtbCode.ContextMenuStrip = Me.cmsTxtPopup
        Me.rtbCode.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbCode.Location = New System.Drawing.Point(0, 27)
        Me.rtbCode.Name = "rtbCode"
        Me.rtbCode.Size = New System.Drawing.Size(234, 195)
        Me.rtbCode.TabIndex = 0
        Me.rtbCode.Text = ""
        Me.rtbCode.WordWrap = False
        '
        'cmsTxtPopup
        '
        Me.cmsTxtPopup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Copy, Me.Paste, Me.Cut})
        Me.cmsTxtPopup.Name = "cmsTxtPopup"
        Me.cmsTxtPopup.Size = New System.Drawing.Size(103, 70)
        '
        'Copy
        '
        Me.Copy.Name = "Copy"
        Me.Copy.Size = New System.Drawing.Size(102, 22)
        Me.Copy.Text = "Copy"
        '
        'Paste
        '
        Me.Paste.Name = "Paste"
        Me.Paste.Size = New System.Drawing.Size(102, 22)
        Me.Paste.Text = "Paste"
        '
        'Cut
        '
        Me.Cut.Name = "Cut"
        Me.Cut.Size = New System.Drawing.Size(102, 22)
        Me.Cut.Text = "Cut"
        '
        'lblCurrentCommand
        '
        Me.lblCurrentCommand.AutoSize = True
        Me.lblCurrentCommand.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentCommand.Location = New System.Drawing.Point(260, 8)
        Me.lblCurrentCommand.Name = "lblCurrentCommand"
        Me.lblCurrentCommand.Size = New System.Drawing.Size(0, 16)
        Me.lblCurrentCommand.TabIndex = 3
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenToolStripMenuItem, Me.SaveToolStripMenuItem1, Me.SaveToolStripMenuItem, Me.NewToolStripMenuItem, Me.ToolStripMenuItem1, Me.VBScriptingSchoolToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'SaveToolStripMenuItem1
        '
        Me.SaveToolStripMenuItem1.Name = "SaveToolStripMenuItem1"
        Me.SaveToolStripMenuItem1.Size = New System.Drawing.Size(174, 22)
        Me.SaveToolStripMenuItem1.Text = "Save"
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.SaveToolStripMenuItem.Text = "Save As"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(171, 6)
        '
        'VBScriptingSchoolToolStripMenuItem
        '
        Me.VBScriptingSchoolToolStripMenuItem.Name = "VBScriptingSchoolToolStripMenuItem"
        Me.VBScriptingSchoolToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.VBScriptingSchoolToolStripMenuItem.Text = "VBScripting School"
        '
        'ConfigToolStripMenuItem
        '
        Me.ConfigToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DelayToolStripMenuItem, Me.FTPAddressToolStripMenuItem})
        Me.ConfigToolStripMenuItem.Name = "ConfigToolStripMenuItem"
        Me.ConfigToolStripMenuItem.Size = New System.Drawing.Size(55, 20)
        Me.ConfigToolStripMenuItem.Text = "Config"
        '
        'DelayToolStripMenuItem
        '
        Me.DelayToolStripMenuItem.Name = "DelayToolStripMenuItem"
        Me.DelayToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.DelayToolStripMenuItem.Text = "IR Frequency"
        '
        'FTPAddressToolStripMenuItem
        '
        Me.FTPAddressToolStripMenuItem.Name = "FTPAddressToolStripMenuItem"
        Me.FTPAddressToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.FTPAddressToolStripMenuItem.Text = "FTP Address"
        '
        'mnuScript
        '
        Me.mnuScript.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ConfigToolStripMenuItem})
        Me.mnuScript.Location = New System.Drawing.Point(0, 0)
        Me.mnuScript.Name = "mnuScript"
        Me.mnuScript.Size = New System.Drawing.Size(554, 24)
        Me.mnuScript.TabIndex = 5
        Me.mnuScript.Text = "MenuStrip1"
        '
        'txtResponses
        '
        Me.txtResponses.Location = New System.Drawing.Point(266, 56)
        Me.txtResponses.Multiline = True
        Me.txtResponses.Name = "txtResponses"
        Me.txtResponses.Size = New System.Drawing.Size(270, 140)
        Me.txtResponses.TabIndex = 6
        Me.txtResponses.Visible = False
        '
        'cboxInbox
        '
        Me.cboxInbox.AutoSize = True
        Me.cboxInbox.Location = New System.Drawing.Point(480, 33)
        Me.cboxInbox.Name = "cboxInbox"
        Me.cboxInbox.Size = New System.Drawing.Size(56, 17)
        Me.cboxInbox.TabIndex = 7
        Me.cboxInbox.Text = "In Box"
        Me.cboxInbox.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Interval = 500
        '
        'Timer2
        '
        '
        'Scripting_Engine
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(554, 242)
        Me.Controls.Add(Me.cboxInbox)
        Me.Controls.Add(Me.txtResponses)
        Me.Controls.Add(Me.lblCurrentCommand)
        Me.Controls.Add(Me.rtbCode)
        Me.Controls.Add(Me.btnRun)
        Me.Controls.Add(Me.txtCommand)
        Me.Controls.Add(Me.ScriptControl1)
        Me.Controls.Add(Me.mnuScript)
        Me.MainMenuStrip = Me.mnuScript
        Me.Name = "Scripting_Engine"
        Me.Text = "Scripting Engine"
        CType(Me.ScriptControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmsTxtPopup.ResumeLayout(False)
        Me.mnuScript.ResumeLayout(False)
        Me.mnuScript.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ScriptControl1 As AxMSScriptControl.AxScriptControl
    Friend WithEvents txtCommand As System.Windows.Forms.TextBox
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents rtbCode As System.Windows.Forms.RichTextBox
    Friend WithEvents lblCurrentCommand As System.Windows.Forms.Label
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConfigToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DelayToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuScript As System.Windows.Forms.MenuStrip
    Friend WithEvents cmsTxtPopup As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents Copy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Paste As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Cut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FTPAddressToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents VBScriptingSchoolToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtResponses As System.Windows.Forms.TextBox
    Friend WithEvents cboxInbox As System.Windows.Forms.CheckBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Timer2 As System.Windows.Forms.Timer

End Class
