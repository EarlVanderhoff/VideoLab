<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BugList
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
        Me.txtBugs = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtBugs
        '
        Me.txtBugs.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBugs.ForeColor = System.Drawing.Color.Maroon
        Me.txtBugs.Location = New System.Drawing.Point(8, 7)
        Me.txtBugs.Multiline = True
        Me.txtBugs.Name = "txtBugs"
        Me.txtBugs.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtBugs.Size = New System.Drawing.Size(547, 210)
        Me.txtBugs.TabIndex = 0
        Me.txtBugs.WordWrap = False
        '
        'BugList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(567, 229)
        Me.Controls.Add(Me.txtBugs)
        Me.Name = "BugList"
        Me.Text = "BugList"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtBugs As System.Windows.Forms.TextBox
End Class
