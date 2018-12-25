<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TV
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TV))
        Me.btnStretch = New System.Windows.Forms.Button()
        Me.btnPlayback = New System.Windows.Forms.Button()
        Me.btnSceneCheck = New System.Windows.Forms.Button()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.UserOptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowAutomationPanelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowIRRemoteControlToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowOutputScreenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowAutomationToolboxToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BugListToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewAddressToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblURL = New System.Windows.Forms.Label()
        Me.AMC = New AxAXISMEDIACONTROLLib.AxAxisMediaControl()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.AMC, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnStretch
        '
        Me.btnStretch.Location = New System.Drawing.Point(12, 334)
        Me.btnStretch.Name = "btnStretch"
        Me.btnStretch.Size = New System.Drawing.Size(59, 23)
        Me.btnStretch.TabIndex = 3
        Me.btnStretch.Text = "AutoSize"
        Me.btnStretch.UseVisualStyleBackColor = True
        '
        'btnPlayback
        '
        Me.btnPlayback.Location = New System.Drawing.Point(77, 334)
        Me.btnPlayback.Name = "btnPlayback"
        Me.btnPlayback.Size = New System.Drawing.Size(59, 23)
        Me.btnPlayback.TabIndex = 4
        Me.btnPlayback.Text = "Play File"
        Me.btnPlayback.UseVisualStyleBackColor = True
        '
        'btnSceneCheck
        '
        Me.btnSceneCheck.Location = New System.Drawing.Point(142, 334)
        Me.btnSceneCheck.Name = "btnSceneCheck"
        Me.btnSceneCheck.Size = New System.Drawing.Size(69, 23)
        Me.btnSceneCheck.TabIndex = 6
        Me.btnSceneCheck.Text = "SceneChk"
        Me.btnSceneCheck.UseVisualStyleBackColor = True
        '
        'BackgroundWorker1
        '
        Me.BackgroundWorker1.WorkerSupportsCancellation = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UserOptionsToolStripMenuItem, Me.ConfigToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(404, 24)
        Me.MenuStrip1.TabIndex = 7
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'UserOptionsToolStripMenuItem
        '
        Me.UserOptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowAutomationPanelToolStripMenuItem, Me.ShowIRRemoteControlToolStripMenuItem, Me.ShowOutputScreenToolStripMenuItem, Me.ShowAutomationToolboxToolStripMenuItem, Me.BugListToolStripMenuItem})
        Me.UserOptionsToolStripMenuItem.Name = "UserOptionsToolStripMenuItem"
        Me.UserOptionsToolStripMenuItem.Size = New System.Drawing.Size(68, 20)
        Me.UserOptionsToolStripMenuItem.Text = "Windows"
        '
        'ShowAutomationPanelToolStripMenuItem
        '
        Me.ShowAutomationPanelToolStripMenuItem.Name = "ShowAutomationPanelToolStripMenuItem"
        Me.ShowAutomationPanelToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ShowAutomationPanelToolStripMenuItem.Text = "Automation Panel"
        '
        'ShowIRRemoteControlToolStripMenuItem
        '
        Me.ShowIRRemoteControlToolStripMenuItem.Name = "ShowIRRemoteControlToolStripMenuItem"
        Me.ShowIRRemoteControlToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ShowIRRemoteControlToolStripMenuItem.Text = "IR Remote Control"
        '
        'ShowOutputScreenToolStripMenuItem
        '
        Me.ShowOutputScreenToolStripMenuItem.Name = "ShowOutputScreenToolStripMenuItem"
        Me.ShowOutputScreenToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ShowOutputScreenToolStripMenuItem.Text = "Output Screen"
        '
        'ShowAutomationToolboxToolStripMenuItem
        '
        Me.ShowAutomationToolboxToolStripMenuItem.Name = "ShowAutomationToolboxToolStripMenuItem"
        Me.ShowAutomationToolboxToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ShowAutomationToolboxToolStripMenuItem.Text = "AutomationToolbox"
        '
        'BugListToolStripMenuItem
        '
        Me.BugListToolStripMenuItem.Name = "BugListToolStripMenuItem"
        Me.BugListToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.BugListToolStripMenuItem.Text = "Bug List"
        '
        'ConfigToolStripMenuItem
        '
        Me.ConfigToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewAddressToolStripMenuItem})
        Me.ConfigToolStripMenuItem.Name = "ConfigToolStripMenuItem"
        Me.ConfigToolStripMenuItem.Size = New System.Drawing.Size(55, 20)
        Me.ConfigToolStripMenuItem.Text = "Config"
        '
        'NewAddressToolStripMenuItem
        '
        Me.NewAddressToolStripMenuItem.Name = "NewAddressToolStripMenuItem"
        Me.NewAddressToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.NewAddressToolStripMenuItem.Text = "Address"
        '
        'lblURL
        '
        Me.lblURL.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblURL.AutoSize = True
        Me.lblURL.Location = New System.Drawing.Point(400, 9)
        Me.lblURL.Name = "lblURL"
        Me.lblURL.Size = New System.Drawing.Size(0, 13)
        Me.lblURL.TabIndex = 8
        '
        'AMC
        '
        Me.AMC.Enabled = True
        Me.AMC.Location = New System.Drawing.Point(0, 28)
        Me.AMC.Name = "AMC"
        Me.AMC.OcxState = CType(resources.GetObject("AMC.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AMC.Size = New System.Drawing.Size(400, 300)
        Me.AMC.TabIndex = 1
        '
        'Button1
        '
        Me.Button1.Enabled = False
        Me.Button1.Location = New System.Drawing.Point(331, 334)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(61, 26)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'TV
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(404, 371)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lblURL)
        Me.Controls.Add(Me.btnSceneCheck)
        Me.Controls.Add(Me.btnPlayback)
        Me.Controls.Add(Me.btnStretch)
        Me.Controls.Add(Me.AMC)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "TV"
        Me.Text = "TV"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.AMC, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AMC As AxAXISMEDIACONTROLLib.AxAxisMediaControl
    Friend WithEvents btnStretch As System.Windows.Forms.Button
    Friend WithEvents btnPlayback As System.Windows.Forms.Button
    Friend WithEvents btnSceneCheck As System.Windows.Forms.Button
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents UserOptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowAutomationPanelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowIRRemoteControlToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowOutputScreenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowAutomationToolboxToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConfigToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewAddressToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BugListToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblURL As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
End Class
