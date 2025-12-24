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
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.tbERPObjectWSURL = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.tbAuthCode = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.tbLaneObjectWSURL = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbBookObjectWSURL = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.tbBookResults = New System.Windows.Forms.TextBox()
        Me.gbBookProcessDataOptions = New System.Windows.Forms.GroupBox()
        Me.btnBookRun = New System.Windows.Forms.Button()
        Me.rbBookProcessBookData80 = New System.Windows.Forms.RadioButton()
        Me.rbBookProcessBookData705 = New System.Windows.Forms.RadioButton()
        Me.rbBookProcessBookData70 = New System.Windows.Forms.RadioButton()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.rbProcessDataEx = New System.Windows.Forms.RadioButton()
        Me.rbBookProcessData70 = New System.Windows.Forms.RadioButton()
        Me.rbBookProcessData60 = New System.Windows.Forms.RadioButton()
        Me.rbBookProcessData = New System.Windows.Forms.RadioButton()
        Me.tbBookCompNo = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tbBookLaneNo = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.tbBookOrderNo = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.TabPage8 = New System.Windows.Forms.TabPage()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.gbBookProcessDataOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Controls.Add(Me.TabPage6)
        Me.TabControl1.Controls.Add(Me.TabPage7)
        Me.TabControl1.Controls.Add(Me.TabPage8)
        Me.TabControl1.Location = New System.Drawing.Point(12, 41)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(755, 349)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.tbERPObjectWSURL)
        Me.TabPage1.Controls.Add(Me.Label9)
        Me.TabPage1.Controls.Add(Me.tbAuthCode)
        Me.TabPage1.Controls.Add(Me.Label7)
        Me.TabPage1.Controls.Add(Me.tbLaneObjectWSURL)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.tbBookObjectWSURL)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(747, 323)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Web Services"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'tbERPObjectWSURL
        '
        Me.tbERPObjectWSURL.Location = New System.Drawing.Point(106, 97)
        Me.tbERPObjectWSURL.Name = "tbERPObjectWSURL"
        Me.tbERPObjectWSURL.Size = New System.Drawing.Size(608, 20)
        Me.tbERPObjectWSURL.TabIndex = 7
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(10, 97)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(54, 13)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "ERP URL"
        '
        'tbAuthCode
        '
        Me.tbAuthCode.Location = New System.Drawing.Point(106, 12)
        Me.tbAuthCode.Name = "tbAuthCode"
        Me.tbAuthCode.Size = New System.Drawing.Size(608, 20)
        Me.tbAuthCode.TabIndex = 5
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(10, 12)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(57, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Auth Code"
        '
        'tbLaneObjectWSURL
        '
        Me.tbLaneObjectWSURL.Location = New System.Drawing.Point(106, 68)
        Me.tbLaneObjectWSURL.Name = "tbLaneObjectWSURL"
        Me.tbLaneObjectWSURL.Size = New System.Drawing.Size(608, 20)
        Me.tbLaneObjectWSURL.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(90, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Lane Object URL"
        '
        'tbBookObjectWSURL
        '
        Me.tbBookObjectWSURL.Location = New System.Drawing.Point(106, 41)
        Me.tbBookObjectWSURL.Name = "tbBookObjectWSURL"
        Me.tbBookObjectWSURL.Size = New System.Drawing.Size(608, 20)
        Me.tbBookObjectWSURL.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Book Object URL"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Label8)
        Me.TabPage2.Controls.Add(Me.tbBookResults)
        Me.TabPage2.Controls.Add(Me.gbBookProcessDataOptions)
        Me.TabPage2.Controls.Add(Me.tbBookCompNo)
        Me.TabPage2.Controls.Add(Me.Label5)
        Me.TabPage2.Controls.Add(Me.tbBookLaneNo)
        Me.TabPage2.Controls.Add(Me.Label4)
        Me.TabPage2.Controls.Add(Me.tbBookOrderNo)
        Me.TabPage2.Controls.Add(Me.Label3)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(747, 323)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Book"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(10, 204)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(42, 13)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Results"
        '
        'tbBookResults
        '
        Me.tbBookResults.Location = New System.Drawing.Point(10, 222)
        Me.tbBookResults.Multiline = True
        Me.tbBookResults.Name = "tbBookResults"
        Me.tbBookResults.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbBookResults.Size = New System.Drawing.Size(687, 95)
        Me.tbBookResults.TabIndex = 7
        '
        'gbBookProcessDataOptions
        '
        Me.gbBookProcessDataOptions.Controls.Add(Me.btnBookRun)
        Me.gbBookProcessDataOptions.Controls.Add(Me.rbBookProcessBookData80)
        Me.gbBookProcessDataOptions.Controls.Add(Me.rbBookProcessBookData705)
        Me.gbBookProcessDataOptions.Controls.Add(Me.rbBookProcessBookData70)
        Me.gbBookProcessDataOptions.Controls.Add(Me.Label6)
        Me.gbBookProcessDataOptions.Controls.Add(Me.rbProcessDataEx)
        Me.gbBookProcessDataOptions.Controls.Add(Me.rbBookProcessData70)
        Me.gbBookProcessDataOptions.Controls.Add(Me.rbBookProcessData60)
        Me.gbBookProcessDataOptions.Controls.Add(Me.rbBookProcessData)
        Me.gbBookProcessDataOptions.Location = New System.Drawing.Point(10, 61)
        Me.gbBookProcessDataOptions.Name = "gbBookProcessDataOptions"
        Me.gbBookProcessDataOptions.Size = New System.Drawing.Size(687, 136)
        Me.gbBookProcessDataOptions.TabIndex = 6
        Me.gbBookProcessDataOptions.TabStop = False
        Me.gbBookProcessDataOptions.Text = "Select a Web Method"
        '
        'btnBookRun
        '
        Me.btnBookRun.Location = New System.Drawing.Point(568, 97)
        Me.btnBookRun.Name = "btnBookRun"
        Me.btnBookRun.Size = New System.Drawing.Size(75, 23)
        Me.btnBookRun.TabIndex = 8
        Me.btnBookRun.Text = "Test Now"
        Me.btnBookRun.UseVisualStyleBackColor = True
        '
        'rbBookProcessBookData80
        '
        Me.rbBookProcessBookData80.AutoSize = True
        Me.rbBookProcessBookData80.Location = New System.Drawing.Point(318, 97)
        Me.rbBookProcessBookData80.Name = "rbBookProcessBookData80"
        Me.rbBookProcessBookData80.Size = New System.Drawing.Size(132, 17)
        Me.rbBookProcessBookData80.TabIndex = 7
        Me.rbBookProcessBookData80.TabStop = True
        Me.rbBookProcessBookData80.Text = "Process Book Data 80"
        Me.rbBookProcessBookData80.UseVisualStyleBackColor = True
        '
        'rbBookProcessBookData705
        '
        Me.rbBookProcessBookData705.AutoSize = True
        Me.rbBookProcessBookData705.Location = New System.Drawing.Point(156, 97)
        Me.rbBookProcessBookData705.Name = "rbBookProcessBookData705"
        Me.rbBookProcessBookData705.Size = New System.Drawing.Size(138, 17)
        Me.rbBookProcessBookData705.TabIndex = 6
        Me.rbBookProcessBookData705.TabStop = True
        Me.rbBookProcessBookData705.Text = "Process Book Data 705"
        Me.rbBookProcessBookData705.UseVisualStyleBackColor = True
        '
        'rbBookProcessBookData70
        '
        Me.rbBookProcessBookData70.AutoSize = True
        Me.rbBookProcessBookData70.Location = New System.Drawing.Point(17, 97)
        Me.rbBookProcessBookData70.Name = "rbBookProcessBookData70"
        Me.rbBookProcessBookData70.Size = New System.Drawing.Size(132, 17)
        Me.rbBookProcessBookData70.TabIndex = 5
        Me.rbBookProcessBookData70.TabStop = True
        Me.rbBookProcessBookData70.Text = "Process Book Data 70"
        Me.rbBookProcessBookData70.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(17, 66)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(99, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "ERP Web Methods"
        '
        'rbProcessDataEx
        '
        Me.rbProcessDataEx.AutoSize = True
        Me.rbProcessDataEx.Location = New System.Drawing.Point(134, 20)
        Me.rbProcessDataEx.Name = "rbProcessDataEx"
        Me.rbProcessDataEx.Size = New System.Drawing.Size(104, 17)
        Me.rbProcessDataEx.TabIndex = 3
        Me.rbProcessDataEx.TabStop = True
        Me.rbProcessDataEx.Text = "Process Data Ex"
        Me.rbProcessDataEx.UseVisualStyleBackColor = True
        '
        'rbBookProcessData70
        '
        Me.rbBookProcessData70.AutoSize = True
        Me.rbBookProcessData70.Location = New System.Drawing.Point(380, 19)
        Me.rbBookProcessData70.Name = "rbBookProcessData70"
        Me.rbBookProcessData70.Size = New System.Drawing.Size(104, 17)
        Me.rbBookProcessData70.TabIndex = 2
        Me.rbBookProcessData70.TabStop = True
        Me.rbBookProcessData70.Text = "Process Data 70"
        Me.rbBookProcessData70.UseVisualStyleBackColor = True
        '
        'rbBookProcessData60
        '
        Me.rbBookProcessData60.AutoSize = True
        Me.rbBookProcessData60.Location = New System.Drawing.Point(261, 20)
        Me.rbBookProcessData60.Name = "rbBookProcessData60"
        Me.rbBookProcessData60.Size = New System.Drawing.Size(104, 17)
        Me.rbBookProcessData60.TabIndex = 1
        Me.rbBookProcessData60.TabStop = True
        Me.rbBookProcessData60.Text = "Process Data 60"
        Me.rbBookProcessData60.UseVisualStyleBackColor = True
        '
        'rbBookProcessData
        '
        Me.rbBookProcessData.AutoSize = True
        Me.rbBookProcessData.Location = New System.Drawing.Point(17, 20)
        Me.rbBookProcessData.Name = "rbBookProcessData"
        Me.rbBookProcessData.Size = New System.Drawing.Size(89, 17)
        Me.rbBookProcessData.TabIndex = 0
        Me.rbBookProcessData.TabStop = True
        Me.rbBookProcessData.Text = "Process Data"
        Me.rbBookProcessData.UseVisualStyleBackColor = True
        '
        'tbBookCompNo
        '
        Me.tbBookCompNo.Location = New System.Drawing.Point(414, 18)
        Me.tbBookCompNo.Name = "tbBookCompNo"
        Me.tbBookCompNo.Size = New System.Drawing.Size(100, 20)
        Me.tbBookCompNo.TabIndex = 5
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(337, 21)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(71, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Company No."
        '
        'tbBookLaneNo
        '
        Me.tbBookLaneNo.Location = New System.Drawing.Point(230, 18)
        Me.tbBookLaneNo.Name = "tbBookLaneNo"
        Me.tbBookLaneNo.Size = New System.Drawing.Size(100, 20)
        Me.tbBookLaneNo.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(173, 21)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Lane No."
        '
        'tbBookOrderNo
        '
        Me.tbBookOrderNo.Location = New System.Drawing.Point(66, 14)
        Me.tbBookOrderNo.Name = "tbBookOrderNo"
        Me.tbBookOrderNo.Size = New System.Drawing.Size(100, 20)
        Me.tbBookOrderNo.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 21)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Order No."
        '
        'TabPage3
        '
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(747, 323)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Lane"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'TabPage4
        '
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(747, 323)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Company"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'TabPage5
        '
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(747, 323)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "Carrier"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'TabPage6
        '
        Me.TabPage6.Location = New System.Drawing.Point(4, 22)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(747, 323)
        Me.TabPage6.TabIndex = 5
        Me.TabPage6.Text = "Pick"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'TabPage7
        '
        Me.TabPage7.Location = New System.Drawing.Point(4, 22)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Size = New System.Drawing.Size(747, 323)
        Me.TabPage7.TabIndex = 6
        Me.TabPage7.Text = "AP"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'TabPage8
        '
        Me.TabPage8.Location = New System.Drawing.Point(4, 22)
        Me.TabPage8.Name = "TabPage8"
        Me.TabPage8.Size = New System.Drawing.Size(747, 323)
        Me.TabPage8.TabIndex = 7
        Me.TabPage8.Text = "Payables"
        Me.TabPage8.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(809, 419)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "Form1"
        Me.Text = "DTMS Web Sevice Tester"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.gbBookProcessDataOptions.ResumeLayout(False)
        Me.gbBookProcessDataOptions.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents tbLaneObjectWSURL As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents tbBookObjectWSURL As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents tbBookLaneNo As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents tbBookOrderNo As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents TabPage4 As TabPage
    Friend WithEvents TabPage5 As TabPage
    Friend WithEvents TabPage6 As TabPage
    Friend WithEvents TabPage7 As TabPage
    Friend WithEvents TabPage8 As TabPage
    Friend WithEvents tbBookCompNo As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents gbBookProcessDataOptions As GroupBox
    Friend WithEvents rbBookProcessBookData705 As RadioButton
    Friend WithEvents rbBookProcessBookData70 As RadioButton
    Friend WithEvents Label6 As Label
    Friend WithEvents rbProcessDataEx As RadioButton
    Friend WithEvents rbBookProcessData70 As RadioButton
    Friend WithEvents rbBookProcessData60 As RadioButton
    Friend WithEvents rbBookProcessData As RadioButton
    Friend WithEvents tbAuthCode As TextBox
    Friend WithEvents rbBookProcessBookData80 As RadioButton
    Friend WithEvents tbBookResults As TextBox
    Friend WithEvents btnBookRun As Button
    Friend WithEvents Label8 As Label
    Friend WithEvents tbERPObjectWSURL As TextBox
    Friend WithEvents Label9 As Label
End Class
