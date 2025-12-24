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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.lblOrig = New System.Windows.Forms.Label()
        Me.lblAddress = New System.Windows.Forms.Label()
        Me.lblCity = New System.Windows.Forms.Label()
        Me.lblState = New System.Windows.Forms.Label()
        Me.lblZip = New System.Windows.Forms.Label()
        Me.tbOrigAddress = New System.Windows.Forms.TextBox()
        Me.lblDest = New System.Windows.Forms.Label()
        Me.tbDestAddress = New System.Windows.Forms.TextBox()
        Me.tbOrigCity = New System.Windows.Forms.TextBox()
        Me.tbOrigState = New System.Windows.Forms.TextBox()
        Me.tbOrigZip = New System.Windows.Forms.TextBox()
        Me.tbDestZip = New System.Windows.Forms.TextBox()
        Me.tbDestState = New System.Windows.Forms.TextBox()
        Me.tbDestCity = New System.Windows.Forms.TextBox()
        Me.btnGetMiles = New System.Windows.Forms.Button()
        Me.lblMiles = New System.Windows.Forms.Label()
        Me.tbMiles = New System.Windows.Forms.TextBox()
        Me.lblPCMAddress = New System.Windows.Forms.Label()
        Me.tbPCMOrigZip = New System.Windows.Forms.TextBox()
        Me.tbPCMOrigState = New System.Windows.Forms.TextBox()
        Me.tbPCMOrigCity = New System.Windows.Forms.TextBox()
        Me.tbPCMOrigAddress = New System.Windows.Forms.TextBox()
        Me.lblPCMReport = New System.Windows.Forms.Label()
        Me.tbPCMReport = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tbPCMDestZip = New System.Windows.Forms.TextBox()
        Me.tbPCMDestState = New System.Windows.Forms.TextBox()
        Me.tbPCMDestCity = New System.Windows.Forms.TextBox()
        Me.tbPCMDestAddress = New System.Windows.Forms.TextBox()
        Me.lblPCMDest = New System.Windows.Forms.Label()
        Me.lblErrors = New System.Windows.Forms.Label()
        Me.lblPCMBadAddressMsg = New System.Windows.Forms.Label()
        Me.lblRouteType = New System.Windows.Forms.Label()
        Me.cboRouteType = New System.Windows.Forms.ComboBox()
        Me.lblDistanceType = New System.Windows.Forms.Label()
        Me.cboDistanceType = New System.Windows.Forms.ComboBox()
        Me.chkUseCom = New System.Windows.Forms.CheckBox()
        Me.btnResequence = New System.Windows.Forms.Button()
        Me.btnShop = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblOrig
        '
        Me.lblOrig.AutoSize = True
        Me.lblOrig.Location = New System.Drawing.Point(13, 62)
        Me.lblOrig.Name = "lblOrig"
        Me.lblOrig.Size = New System.Drawing.Size(34, 13)
        Me.lblOrig.TabIndex = 0
        Me.lblOrig.Text = "Origin"
        '
        'lblAddress
        '
        Me.lblAddress.AutoSize = True
        Me.lblAddress.Location = New System.Drawing.Point(49, 42)
        Me.lblAddress.Name = "lblAddress"
        Me.lblAddress.Size = New System.Drawing.Size(45, 13)
        Me.lblAddress.TabIndex = 1
        Me.lblAddress.Text = "Address"
        '
        'lblCity
        '
        Me.lblCity.AutoSize = True
        Me.lblCity.Location = New System.Drawing.Point(359, 42)
        Me.lblCity.Name = "lblCity"
        Me.lblCity.Size = New System.Drawing.Size(24, 13)
        Me.lblCity.TabIndex = 2
        Me.lblCity.Text = "City"
        '
        'lblState
        '
        Me.lblState.AutoSize = True
        Me.lblState.Location = New System.Drawing.Point(498, 42)
        Me.lblState.Name = "lblState"
        Me.lblState.Size = New System.Drawing.Size(59, 13)
        Me.lblState.TabIndex = 3
        Me.lblState.Text = "State/Prov"
        '
        'lblZip
        '
        Me.lblZip.AutoSize = True
        Me.lblZip.Location = New System.Drawing.Point(569, 42)
        Me.lblZip.Name = "lblZip"
        Me.lblZip.Size = New System.Drawing.Size(64, 13)
        Me.lblZip.TabIndex = 4
        Me.lblZip.Text = "Postal Code"
        '
        'tbOrigAddress
        '
        Me.tbOrigAddress.Location = New System.Drawing.Point(52, 62)
        Me.tbOrigAddress.Name = "tbOrigAddress"
        Me.tbOrigAddress.Size = New System.Drawing.Size(297, 20)
        Me.tbOrigAddress.TabIndex = 1
        '
        'lblDest
        '
        Me.lblDest.AutoSize = True
        Me.lblDest.Location = New System.Drawing.Point(13, 90)
        Me.lblDest.Name = "lblDest"
        Me.lblDest.Size = New System.Drawing.Size(29, 13)
        Me.lblDest.TabIndex = 6
        Me.lblDest.Text = "Dest"
        '
        'tbDestAddress
        '
        Me.tbDestAddress.Location = New System.Drawing.Point(52, 87)
        Me.tbDestAddress.Name = "tbDestAddress"
        Me.tbDestAddress.Size = New System.Drawing.Size(297, 20)
        Me.tbDestAddress.TabIndex = 5
        '
        'tbOrigCity
        '
        Me.tbOrigCity.Location = New System.Drawing.Point(355, 62)
        Me.tbOrigCity.Name = "tbOrigCity"
        Me.tbOrigCity.Size = New System.Drawing.Size(137, 20)
        Me.tbOrigCity.TabIndex = 2
        '
        'tbOrigState
        '
        Me.tbOrigState.Location = New System.Drawing.Point(498, 62)
        Me.tbOrigState.Name = "tbOrigState"
        Me.tbOrigState.Size = New System.Drawing.Size(62, 20)
        Me.tbOrigState.TabIndex = 3
        '
        'tbOrigZip
        '
        Me.tbOrigZip.Location = New System.Drawing.Point(566, 62)
        Me.tbOrigZip.Name = "tbOrigZip"
        Me.tbOrigZip.Size = New System.Drawing.Size(100, 20)
        Me.tbOrigZip.TabIndex = 4
        '
        'tbDestZip
        '
        Me.tbDestZip.Location = New System.Drawing.Point(566, 87)
        Me.tbDestZip.Name = "tbDestZip"
        Me.tbDestZip.Size = New System.Drawing.Size(100, 20)
        Me.tbDestZip.TabIndex = 8
        '
        'tbDestState
        '
        Me.tbDestState.Location = New System.Drawing.Point(498, 87)
        Me.tbDestState.Name = "tbDestState"
        Me.tbDestState.Size = New System.Drawing.Size(62, 20)
        Me.tbDestState.TabIndex = 7
        '
        'tbDestCity
        '
        Me.tbDestCity.Location = New System.Drawing.Point(355, 87)
        Me.tbDestCity.Name = "tbDestCity"
        Me.tbDestCity.Size = New System.Drawing.Size(137, 20)
        Me.tbDestCity.TabIndex = 6
        '
        'btnGetMiles
        '
        Me.btnGetMiles.Location = New System.Drawing.Point(52, 114)
        Me.btnGetMiles.Name = "btnGetMiles"
        Me.btnGetMiles.Size = New System.Drawing.Size(75, 23)
        Me.btnGetMiles.TabIndex = 8
        Me.btnGetMiles.Text = "Get Miles"
        Me.btnGetMiles.UseVisualStyleBackColor = True
        '
        'lblMiles
        '
        Me.lblMiles.AutoSize = True
        Me.lblMiles.Location = New System.Drawing.Point(13, 146)
        Me.lblMiles.Name = "lblMiles"
        Me.lblMiles.Size = New System.Drawing.Size(31, 13)
        Me.lblMiles.TabIndex = 9
        Me.lblMiles.Text = "Miles"
        '
        'tbMiles
        '
        Me.tbMiles.Location = New System.Drawing.Point(52, 143)
        Me.tbMiles.Name = "tbMiles"
        Me.tbMiles.Size = New System.Drawing.Size(75, 20)
        Me.tbMiles.TabIndex = 10
        '
        'lblPCMAddress
        '
        Me.lblPCMAddress.AutoSize = True
        Me.lblPCMAddress.Location = New System.Drawing.Point(16, 189)
        Me.lblPCMAddress.Name = "lblPCMAddress"
        Me.lblPCMAddress.Size = New System.Drawing.Size(26, 13)
        Me.lblPCMAddress.TabIndex = 11
        Me.lblPCMAddress.Text = "Orig"
        '
        'tbPCMOrigZip
        '
        Me.tbPCMOrigZip.Location = New System.Drawing.Point(566, 186)
        Me.tbPCMOrigZip.Name = "tbPCMOrigZip"
        Me.tbPCMOrigZip.Size = New System.Drawing.Size(100, 20)
        Me.tbPCMOrigZip.TabIndex = 15
        '
        'tbPCMOrigState
        '
        Me.tbPCMOrigState.Location = New System.Drawing.Point(498, 186)
        Me.tbPCMOrigState.Name = "tbPCMOrigState"
        Me.tbPCMOrigState.Size = New System.Drawing.Size(62, 20)
        Me.tbPCMOrigState.TabIndex = 13
        '
        'tbPCMOrigCity
        '
        Me.tbPCMOrigCity.Location = New System.Drawing.Point(355, 186)
        Me.tbPCMOrigCity.Name = "tbPCMOrigCity"
        Me.tbPCMOrigCity.Size = New System.Drawing.Size(137, 20)
        Me.tbPCMOrigCity.TabIndex = 14
        '
        'tbPCMOrigAddress
        '
        Me.tbPCMOrigAddress.Location = New System.Drawing.Point(52, 186)
        Me.tbPCMOrigAddress.Name = "tbPCMOrigAddress"
        Me.tbPCMOrigAddress.Size = New System.Drawing.Size(297, 20)
        Me.tbPCMOrigAddress.TabIndex = 12
        '
        'lblPCMReport
        '
        Me.lblPCMReport.AutoSize = True
        Me.lblPCMReport.Location = New System.Drawing.Point(49, 286)
        Me.lblPCMReport.Name = "lblPCMReport"
        Me.lblPCMReport.Size = New System.Drawing.Size(65, 13)
        Me.lblPCMReport.TabIndex = 16
        Me.lblPCMReport.Text = "PCM Report"
        '
        'tbPCMReport
        '
        Me.tbPCMReport.Location = New System.Drawing.Point(52, 302)
        Me.tbPCMReport.Multiline = True
        Me.tbPCMReport.Name = "tbPCMReport"
        Me.tbPCMReport.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbPCMReport.Size = New System.Drawing.Size(614, 218)
        Me.tbPCMReport.TabIndex = 17
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(52, 170)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(145, 13)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "PCM values for Bad Address:"
        '
        'tbPCMDestZip
        '
        Me.tbPCMDestZip.Location = New System.Drawing.Point(566, 213)
        Me.tbPCMDestZip.Name = "tbPCMDestZip"
        Me.tbPCMDestZip.Size = New System.Drawing.Size(100, 20)
        Me.tbPCMDestZip.TabIndex = 23
        '
        'tbPCMDestState
        '
        Me.tbPCMDestState.Location = New System.Drawing.Point(498, 213)
        Me.tbPCMDestState.Name = "tbPCMDestState"
        Me.tbPCMDestState.Size = New System.Drawing.Size(62, 20)
        Me.tbPCMDestState.TabIndex = 21
        '
        'tbPCMDestCity
        '
        Me.tbPCMDestCity.Location = New System.Drawing.Point(355, 213)
        Me.tbPCMDestCity.Name = "tbPCMDestCity"
        Me.tbPCMDestCity.Size = New System.Drawing.Size(137, 20)
        Me.tbPCMDestCity.TabIndex = 22
        '
        'tbPCMDestAddress
        '
        Me.tbPCMDestAddress.Location = New System.Drawing.Point(52, 213)
        Me.tbPCMDestAddress.Name = "tbPCMDestAddress"
        Me.tbPCMDestAddress.Size = New System.Drawing.Size(297, 20)
        Me.tbPCMDestAddress.TabIndex = 20
        '
        'lblPCMDest
        '
        Me.lblPCMDest.AutoSize = True
        Me.lblPCMDest.Location = New System.Drawing.Point(16, 216)
        Me.lblPCMDest.Name = "lblPCMDest"
        Me.lblPCMDest.Size = New System.Drawing.Size(29, 13)
        Me.lblPCMDest.TabIndex = 19
        Me.lblPCMDest.Text = "Dest"
        '
        'lblErrors
        '
        Me.lblErrors.AutoSize = True
        Me.lblErrors.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblErrors.ForeColor = System.Drawing.Color.Red
        Me.lblErrors.Location = New System.Drawing.Point(218, 114)
        Me.lblErrors.MaximumSize = New System.Drawing.Size(300, 100)
        Me.lblErrors.Name = "lblErrors"
        Me.lblErrors.Size = New System.Drawing.Size(44, 13)
        Me.lblErrors.TabIndex = 24
        Me.lblErrors.Text = "Errors:"
        '
        'lblPCMBadAddressMsg
        '
        Me.lblPCMBadAddressMsg.AutoSize = True
        Me.lblPCMBadAddressMsg.Location = New System.Drawing.Point(52, 240)
        Me.lblPCMBadAddressMsg.MaximumSize = New System.Drawing.Size(500, 100)
        Me.lblPCMBadAddressMsg.Name = "lblPCMBadAddressMsg"
        Me.lblPCMBadAddressMsg.Size = New System.Drawing.Size(130, 13)
        Me.lblPCMBadAddressMsg.TabIndex = 25
        Me.lblPCMBadAddressMsg.Text = "PCMBadAddressMessage"
        '
        'lblRouteType
        '
        Me.lblRouteType.AutoSize = True
        Me.lblRouteType.Location = New System.Drawing.Point(19, 18)
        Me.lblRouteType.Name = "lblRouteType"
        Me.lblRouteType.Size = New System.Drawing.Size(63, 13)
        Me.lblRouteType.TabIndex = 26
        Me.lblRouteType.Text = "Route Type"
        '
        'cboRouteType
        '
        Me.cboRouteType.FormattingEnabled = True
        Me.cboRouteType.Location = New System.Drawing.Point(89, 13)
        Me.cboRouteType.Name = "cboRouteType"
        Me.cboRouteType.Size = New System.Drawing.Size(121, 21)
        Me.cboRouteType.TabIndex = 27
        '
        'lblDistanceType
        '
        Me.lblDistanceType.AutoSize = True
        Me.lblDistanceType.Location = New System.Drawing.Point(214, 17)
        Me.lblDistanceType.Name = "lblDistanceType"
        Me.lblDistanceType.Size = New System.Drawing.Size(76, 13)
        Me.lblDistanceType.TabIndex = 28
        Me.lblDistanceType.Text = "Distance Type"
        '
        'cboDistanceType
        '
        Me.cboDistanceType.FormattingEnabled = True
        Me.cboDistanceType.Location = New System.Drawing.Point(296, 12)
        Me.cboDistanceType.Name = "cboDistanceType"
        Me.cboDistanceType.Size = New System.Drawing.Size(121, 21)
        Me.cboDistanceType.TabIndex = 29
        '
        'chkUseCom
        '
        Me.chkUseCom.AutoSize = True
        Me.chkUseCom.Location = New System.Drawing.Point(422, 17)
        Me.chkUseCom.Name = "chkUseCom"
        Me.chkUseCom.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkUseCom.Size = New System.Drawing.Size(72, 17)
        Me.chkUseCom.TabIndex = 31
        Me.chkUseCom.Text = "Use COM"
        Me.chkUseCom.UseVisualStyleBackColor = True
        '
        'btnResequence
        '
        Me.btnResequence.Location = New System.Drawing.Point(500, 13)
        Me.btnResequence.Name = "btnResequence"
        Me.btnResequence.Size = New System.Drawing.Size(85, 23)
        Me.btnResequence.TabIndex = 32
        Me.btnResequence.Text = "Resequence"
        Me.btnResequence.UseVisualStyleBackColor = True
        '
        'btnShop
        '
        Me.btnShop.Location = New System.Drawing.Point(591, 13)
        Me.btnShop.Name = "btnShop"
        Me.btnShop.Size = New System.Drawing.Size(75, 23)
        Me.btnShop.TabIndex = 33
        Me.btnShop.Text = "Shop"
        Me.btnShop.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(711, 703)
        Me.Controls.Add(Me.btnShop)
        Me.Controls.Add(Me.btnResequence)
        Me.Controls.Add(Me.chkUseCom)
        Me.Controls.Add(Me.cboDistanceType)
        Me.Controls.Add(Me.lblDistanceType)
        Me.Controls.Add(Me.cboRouteType)
        Me.Controls.Add(Me.lblRouteType)
        Me.Controls.Add(Me.lblPCMBadAddressMsg)
        Me.Controls.Add(Me.lblErrors)
        Me.Controls.Add(Me.tbPCMDestZip)
        Me.Controls.Add(Me.tbPCMDestState)
        Me.Controls.Add(Me.tbPCMDestCity)
        Me.Controls.Add(Me.tbPCMDestAddress)
        Me.Controls.Add(Me.lblPCMDest)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbPCMReport)
        Me.Controls.Add(Me.lblPCMReport)
        Me.Controls.Add(Me.tbPCMOrigZip)
        Me.Controls.Add(Me.tbPCMOrigState)
        Me.Controls.Add(Me.tbPCMOrigCity)
        Me.Controls.Add(Me.tbPCMOrigAddress)
        Me.Controls.Add(Me.lblPCMAddress)
        Me.Controls.Add(Me.tbMiles)
        Me.Controls.Add(Me.lblMiles)
        Me.Controls.Add(Me.btnGetMiles)
        Me.Controls.Add(Me.tbDestZip)
        Me.Controls.Add(Me.tbDestState)
        Me.Controls.Add(Me.tbDestCity)
        Me.Controls.Add(Me.tbOrigZip)
        Me.Controls.Add(Me.tbOrigState)
        Me.Controls.Add(Me.tbOrigCity)
        Me.Controls.Add(Me.tbDestAddress)
        Me.Controls.Add(Me.lblDest)
        Me.Controls.Add(Me.tbOrigAddress)
        Me.Controls.Add(Me.lblZip)
        Me.Controls.Add(Me.lblState)
        Me.Controls.Add(Me.lblCity)
        Me.Controls.Add(Me.lblAddress)
        Me.Controls.Add(Me.lblOrig)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "NGL PC Miler Test App"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblOrig As System.Windows.Forms.Label
    Friend WithEvents lblAddress As System.Windows.Forms.Label
    Friend WithEvents lblCity As System.Windows.Forms.Label
    Friend WithEvents lblState As System.Windows.Forms.Label
    Friend WithEvents lblZip As System.Windows.Forms.Label
    Friend WithEvents tbOrigAddress As System.Windows.Forms.TextBox
    Friend WithEvents lblDest As System.Windows.Forms.Label
    Friend WithEvents tbDestAddress As System.Windows.Forms.TextBox
    Friend WithEvents tbOrigCity As System.Windows.Forms.TextBox
    Friend WithEvents tbOrigState As System.Windows.Forms.TextBox
    Friend WithEvents tbOrigZip As System.Windows.Forms.TextBox
    Friend WithEvents tbDestZip As System.Windows.Forms.TextBox
    Friend WithEvents tbDestState As System.Windows.Forms.TextBox
    Friend WithEvents tbDestCity As System.Windows.Forms.TextBox
    Friend WithEvents btnGetMiles As System.Windows.Forms.Button
    Friend WithEvents lblMiles As System.Windows.Forms.Label
    Friend WithEvents tbMiles As System.Windows.Forms.TextBox
    Friend WithEvents lblPCMAddress As System.Windows.Forms.Label
    Friend WithEvents tbPCMOrigZip As System.Windows.Forms.TextBox
    Friend WithEvents tbPCMOrigState As System.Windows.Forms.TextBox
    Friend WithEvents tbPCMOrigCity As System.Windows.Forms.TextBox
    Friend WithEvents tbPCMOrigAddress As System.Windows.Forms.TextBox
    Friend WithEvents lblPCMReport As System.Windows.Forms.Label
    Friend WithEvents tbPCMReport As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbPCMDestZip As System.Windows.Forms.TextBox
    Friend WithEvents tbPCMDestState As System.Windows.Forms.TextBox
    Friend WithEvents tbPCMDestCity As System.Windows.Forms.TextBox
    Friend WithEvents tbPCMDestAddress As System.Windows.Forms.TextBox
    Friend WithEvents lblPCMDest As System.Windows.Forms.Label
    Friend WithEvents lblErrors As System.Windows.Forms.Label
    Friend WithEvents lblPCMBadAddressMsg As System.Windows.Forms.Label
    Friend WithEvents lblRouteType As System.Windows.Forms.Label
    Friend WithEvents cboRouteType As System.Windows.Forms.ComboBox
    Friend WithEvents lblDistanceType As System.Windows.Forms.Label
    Friend WithEvents cboDistanceType As System.Windows.Forms.ComboBox
    Friend WithEvents chkUseCom As System.Windows.Forms.CheckBox
    Friend WithEvents btnResequence As System.Windows.Forms.Button
    Friend WithEvents btnShop As System.Windows.Forms.Button

End Class
