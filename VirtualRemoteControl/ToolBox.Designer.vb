<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ToolBox
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
        Me.lbxCode = New System.Windows.Forms.ListBox()
        Me.lbxScripts = New System.Windows.Forms.ListBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.CannedAutomationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cbxCannisters = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.tmrCopyThis = New System.Windows.Forms.Timer(Me.components)
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lbxCode
        '
        Me.lbxCode.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbxCode.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbxCode.FormattingEnabled = True
        Me.lbxCode.ItemHeight = 15
        Me.lbxCode.Location = New System.Drawing.Point(7, 46)
        Me.lbxCode.Name = "lbxCode"
        Me.lbxCode.Size = New System.Drawing.Size(189, 184)
        Me.lbxCode.TabIndex = 0
        '
        'lbxScripts
        '
        Me.lbxScripts.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbxScripts.FormattingEnabled = True
        Me.lbxScripts.ItemHeight = 15
        Me.lbxScripts.Location = New System.Drawing.Point(8, 309)
        Me.lbxScripts.Name = "lbxScripts"
        Me.lbxScripts.Size = New System.Drawing.Size(188, 169)
        Me.lbxScripts.TabIndex = 1
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CannedAutomationToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(208, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'CannedAutomationToolStripMenuItem
        '
        Me.CannedAutomationToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LoadToolStripMenuItem})
        Me.CannedAutomationToolStripMenuItem.Name = "CannedAutomationToolStripMenuItem"
        Me.CannedAutomationToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.CannedAutomationToolStripMenuItem.Text = "File"
        '
        'LoadToolStripMenuItem
        '
        Me.LoadToolStripMenuItem.Name = "LoadToolStripMenuItem"
        Me.LoadToolStripMenuItem.Size = New System.Drawing.Size(212, 22)
        Me.LoadToolStripMenuItem.Text = "Load All Automation Pkgs"
        '
        'cbxCannisters
        '
        Me.cbxCannisters.FormattingEnabled = True
        Me.cbxCannisters.Location = New System.Drawing.Point(8, 268)
        Me.cbxCannisters.Name = "cbxCannisters"
        Me.cbxCannisters.Size = New System.Drawing.Size(188, 21)
        Me.cbxCannisters.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(5, 252)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(111, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Automation Packages"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(5, 292)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Canned Automation"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(4, 30)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(94, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Custom VB Scripts"
        '
        'tmrCopyThis
        '
        Me.tmrCopyThis.Interval = 150
        '
        'ToolBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(208, 503)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cbxCannisters)
        Me.Controls.Add(Me.lbxScripts)
        Me.Controls.Add(Me.lbxCode)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "ToolBox"
        Me.Text = "ToolBox"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbxCode As System.Windows.Forms.ListBox
    Friend WithEvents lbxScripts As System.Windows.Forms.ListBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents CannedAutomationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoadToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cbxCannisters As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tmrCopyThis As System.Windows.Forms.Timer
End Class
