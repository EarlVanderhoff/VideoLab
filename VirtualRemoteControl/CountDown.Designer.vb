<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CountDown
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
        Me.txtCountDown = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtCountDown
        '
        Me.txtCountDown.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCountDown.Location = New System.Drawing.Point(23, 12)
        Me.txtCountDown.Name = "txtCountDown"
        Me.txtCountDown.Size = New System.Drawing.Size(65, 29)
        Me.txtCountDown.TabIndex = 0
        Me.txtCountDown.Text = "00:00"
        '
        'CountDown
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(112, 51)
        Me.Controls.Add(Me.txtCountDown)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "CountDown"
        Me.Text = "CountDown"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtCountDown As System.Windows.Forms.TextBox
End Class
