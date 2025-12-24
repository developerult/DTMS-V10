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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbUser = New System.Windows.Forms.TextBox()
        Me.tbURL = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.rtbResults = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(2, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "UserName:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(2, 31)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(32, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "URL:"
        '
        'tbUser
        '
        Me.tbUser.Location = New System.Drawing.Point(69, 4)
        Me.tbUser.Name = "tbUser"
        Me.tbUser.Size = New System.Drawing.Size(219, 20)
        Me.tbUser.TabIndex = 3
        Me.tbUser.Text = "SSRSReports@ngl.local"
        '
        'tbURL
        '
        Me.tbURL.Location = New System.Drawing.Point(69, 31)
        Me.tbURL.Name = "tbURL"
        Me.tbURL.Size = New System.Drawing.Size(219, 20)
        Me.tbURL.TabIndex = 4
        Me.tbURL.Text = "http://NGLGP2013R2:48620/Dynamics/GPService/GPService"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(106, 223)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "GO"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'rtbResults
        '
        Me.rtbResults.Location = New System.Drawing.Point(5, 60)
        Me.rtbResults.Name = "rtbResults"
        Me.rtbResults.Size = New System.Drawing.Size(275, 157)
        Me.rtbResults.TabIndex = 6
        Me.rtbResults.Text = ""
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 253)
        Me.Controls.Add(Me.rtbResults)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.tbURL)
        Me.Controls.Add(Me.tbUser)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbUser As System.Windows.Forms.TextBox
    Friend WithEvents tbURL As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents rtbResults As System.Windows.Forms.RichTextBox

End Class
