<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form3
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
        Me.lblSolution = New System.Windows.Forms.Label()
        Me.dgvSolution = New System.Windows.Forms.DataGridView()
        Me.lblStops = New System.Windows.Forms.Label()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.lblPCMBadAddressMsg = New System.Windows.Forms.Label()
        Me.lblBadAddress = New System.Windows.Forms.Label()
        Me.btnRun = New System.Windows.Forms.Button()
        Me.dgvBadAddress = New System.Windows.Forms.DataGridView()
        Me.lblErrors = New System.Windows.Forms.Label()
        Me.btnAddStop = New System.Windows.Forms.Button()
        Me.dgvStops = New System.Windows.Forms.DataGridView()
        Me.chkUseCom = New System.Windows.Forms.CheckBox()
        Me.cboDistanceType = New System.Windows.Forms.ComboBox()
        Me.lblDistanceType = New System.Windows.Forms.Label()
        Me.cboRouteType = New System.Windows.Forms.ComboBox()
        Me.lblRouteType = New System.Windows.Forms.Label()
        Me.tbStopZip = New System.Windows.Forms.TextBox()
        Me.tbStopState = New System.Windows.Forms.TextBox()
        Me.tbStopCity = New System.Windows.Forms.TextBox()
        Me.tbStopAddress = New System.Windows.Forms.TextBox()
        Me.lblZip = New System.Windows.Forms.Label()
        Me.lblState = New System.Windows.Forms.Label()
        Me.lblCity = New System.Windows.Forms.Label()
        Me.lblAddress = New System.Windows.Forms.Label()
        Me.lblOrig = New System.Windows.Forms.Label()
        Me.txtOrder1 = New System.Windows.Forms.TextBox()
        Me.txtOrder2 = New System.Windows.Forms.TextBox()
        Me.txtOrder3 = New System.Windows.Forms.TextBox()
        Me.txtOrder4 = New System.Windows.Forms.TextBox()
        Me.lblCarriers = New System.Windows.Forms.Label()
        Me.dgvCarriers = New System.Windows.Forms.DataGridView()
        CType(Me.dgvSolution, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvBadAddress, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvStops, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvCarriers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblSolution
        '
        Me.lblSolution.AutoSize = True
        Me.lblSolution.Location = New System.Drawing.Point(148, 293)
        Me.lblSolution.Name = "lblSolution"
        Me.lblSolution.Size = New System.Drawing.Size(45, 13)
        Me.lblSolution.TabIndex = 75
        Me.lblSolution.Text = "Solution"
        '
        'dgvSolution
        '
        Me.dgvSolution.AllowUserToAddRows = False
        Me.dgvSolution.AllowUserToDeleteRows = False
        Me.dgvSolution.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSolution.Location = New System.Drawing.Point(151, 311)
        Me.dgvSolution.MaximumSize = New System.Drawing.Size(1013, 128)
        Me.dgvSolution.MinimumSize = New System.Drawing.Size(1013, 128)
        Me.dgvSolution.Name = "dgvSolution"
        Me.dgvSolution.ReadOnly = True
        Me.dgvSolution.Size = New System.Drawing.Size(1013, 128)
        Me.dgvSolution.TabIndex = 74
        '
        'lblStops
        '
        Me.lblStops.AutoSize = True
        Me.lblStops.Location = New System.Drawing.Point(150, 99)
        Me.lblStops.Name = "lblStops"
        Me.lblStops.Size = New System.Drawing.Size(82, 13)
        Me.lblStops.TabIndex = 73
        Me.lblStops.Text = "Stops To Route"
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(151, 8)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(75, 23)
        Me.btnClear.TabIndex = 72
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'lblPCMBadAddressMsg
        '
        Me.lblPCMBadAddressMsg.AutoSize = True
        Me.lblPCMBadAddressMsg.Location = New System.Drawing.Point(662, 252)
        Me.lblPCMBadAddressMsg.MaximumSize = New System.Drawing.Size(300, 100)
        Me.lblPCMBadAddressMsg.Name = "lblPCMBadAddressMsg"
        Me.lblPCMBadAddressMsg.Size = New System.Drawing.Size(130, 13)
        Me.lblPCMBadAddressMsg.TabIndex = 71
        Me.lblPCMBadAddressMsg.Text = "PCMBadAddressMessage"
        '
        'lblBadAddress
        '
        Me.lblBadAddress.AutoSize = True
        Me.lblBadAddress.Location = New System.Drawing.Point(661, 99)
        Me.lblBadAddress.Name = "lblBadAddress"
        Me.lblBadAddress.Size = New System.Drawing.Size(154, 13)
        Me.lblBadAddress.TabIndex = 70
        Me.lblBadAddress.Text = "PCM Version of Bad Addresses"
        '
        'btnRun
        '
        Me.btnRun.Location = New System.Drawing.Point(151, 37)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(75, 23)
        Me.btnRun.TabIndex = 69
        Me.btnRun.Text = "Run"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'dgvBadAddress
        '
        Me.dgvBadAddress.AllowUserToAddRows = False
        Me.dgvBadAddress.AllowUserToDeleteRows = False
        Me.dgvBadAddress.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvBadAddress.Location = New System.Drawing.Point(664, 121)
        Me.dgvBadAddress.MaximumSize = New System.Drawing.Size(500, 128)
        Me.dgvBadAddress.MinimumSize = New System.Drawing.Size(500, 128)
        Me.dgvBadAddress.Name = "dgvBadAddress"
        Me.dgvBadAddress.ReadOnly = True
        Me.dgvBadAddress.Size = New System.Drawing.Size(500, 128)
        Me.dgvBadAddress.TabIndex = 68
        '
        'lblErrors
        '
        Me.lblErrors.AutoSize = True
        Me.lblErrors.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblErrors.ForeColor = System.Drawing.Color.Red
        Me.lblErrors.Location = New System.Drawing.Point(150, 252)
        Me.lblErrors.MaximumSize = New System.Drawing.Size(300, 100)
        Me.lblErrors.Name = "lblErrors"
        Me.lblErrors.Size = New System.Drawing.Size(44, 13)
        Me.lblErrors.TabIndex = 67
        Me.lblErrors.Text = "Errors:"
        '
        'btnAddStop
        '
        Me.btnAddStop.Location = New System.Drawing.Point(151, 64)
        Me.btnAddStop.Name = "btnAddStop"
        Me.btnAddStop.Size = New System.Drawing.Size(75, 23)
        Me.btnAddStop.TabIndex = 66
        Me.btnAddStop.Text = "Add Stop"
        Me.btnAddStop.UseVisualStyleBackColor = True
        '
        'dgvStops
        '
        Me.dgvStops.AllowUserToAddRows = False
        Me.dgvStops.AllowUserToDeleteRows = False
        Me.dgvStops.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvStops.Location = New System.Drawing.Point(152, 121)
        Me.dgvStops.MaximumSize = New System.Drawing.Size(500, 128)
        Me.dgvStops.MinimumSize = New System.Drawing.Size(500, 128)
        Me.dgvStops.Name = "dgvStops"
        Me.dgvStops.ReadOnly = True
        Me.dgvStops.Size = New System.Drawing.Size(500, 128)
        Me.dgvStops.TabIndex = 65
        '
        'chkUseCom
        '
        Me.chkUseCom.AutoSize = True
        Me.chkUseCom.Location = New System.Drawing.Point(720, 19)
        Me.chkUseCom.Name = "chkUseCom"
        Me.chkUseCom.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkUseCom.Size = New System.Drawing.Size(72, 17)
        Me.chkUseCom.TabIndex = 64
        Me.chkUseCom.Text = "Use COM"
        Me.chkUseCom.UseVisualStyleBackColor = True
        '
        'cboDistanceType
        '
        Me.cboDistanceType.FormattingEnabled = True
        Me.cboDistanceType.Location = New System.Drawing.Point(568, 14)
        Me.cboDistanceType.Name = "cboDistanceType"
        Me.cboDistanceType.Size = New System.Drawing.Size(121, 21)
        Me.cboDistanceType.TabIndex = 63
        '
        'lblDistanceType
        '
        Me.lblDistanceType.AutoSize = True
        Me.lblDistanceType.Location = New System.Drawing.Point(486, 19)
        Me.lblDistanceType.Name = "lblDistanceType"
        Me.lblDistanceType.Size = New System.Drawing.Size(76, 13)
        Me.lblDistanceType.TabIndex = 62
        Me.lblDistanceType.Text = "Distance Type"
        '
        'cboRouteType
        '
        Me.cboRouteType.FormattingEnabled = True
        Me.cboRouteType.Location = New System.Drawing.Point(344, 15)
        Me.cboRouteType.Name = "cboRouteType"
        Me.cboRouteType.Size = New System.Drawing.Size(121, 21)
        Me.cboRouteType.TabIndex = 61
        '
        'lblRouteType
        '
        Me.lblRouteType.AutoSize = True
        Me.lblRouteType.Location = New System.Drawing.Point(274, 20)
        Me.lblRouteType.Name = "lblRouteType"
        Me.lblRouteType.Size = New System.Drawing.Size(63, 13)
        Me.lblRouteType.TabIndex = 60
        Me.lblRouteType.Text = "Route Type"
        '
        'tbStopZip
        '
        Me.tbStopZip.Location = New System.Drawing.Point(791, 67)
        Me.tbStopZip.Name = "tbStopZip"
        Me.tbStopZip.Size = New System.Drawing.Size(100, 20)
        Me.tbStopZip.TabIndex = 58
        '
        'tbStopState
        '
        Me.tbStopState.Location = New System.Drawing.Point(723, 67)
        Me.tbStopState.Name = "tbStopState"
        Me.tbStopState.Size = New System.Drawing.Size(62, 20)
        Me.tbStopState.TabIndex = 56
        '
        'tbStopCity
        '
        Me.tbStopCity.Location = New System.Drawing.Point(580, 67)
        Me.tbStopCity.Name = "tbStopCity"
        Me.tbStopCity.Size = New System.Drawing.Size(137, 20)
        Me.tbStopCity.TabIndex = 54
        '
        'tbStopAddress
        '
        Me.tbStopAddress.Location = New System.Drawing.Point(277, 67)
        Me.tbStopAddress.Name = "tbStopAddress"
        Me.tbStopAddress.Size = New System.Drawing.Size(297, 20)
        Me.tbStopAddress.TabIndex = 52
        '
        'lblZip
        '
        Me.lblZip.AutoSize = True
        Me.lblZip.Location = New System.Drawing.Point(794, 47)
        Me.lblZip.Name = "lblZip"
        Me.lblZip.Size = New System.Drawing.Size(64, 13)
        Me.lblZip.TabIndex = 59
        Me.lblZip.Text = "Postal Code"
        '
        'lblState
        '
        Me.lblState.AutoSize = True
        Me.lblState.Location = New System.Drawing.Point(723, 47)
        Me.lblState.Name = "lblState"
        Me.lblState.Size = New System.Drawing.Size(59, 13)
        Me.lblState.TabIndex = 57
        Me.lblState.Text = "State/Prov"
        '
        'lblCity
        '
        Me.lblCity.AutoSize = True
        Me.lblCity.Location = New System.Drawing.Point(584, 47)
        Me.lblCity.Name = "lblCity"
        Me.lblCity.Size = New System.Drawing.Size(24, 13)
        Me.lblCity.TabIndex = 55
        Me.lblCity.Text = "City"
        '
        'lblAddress
        '
        Me.lblAddress.AutoSize = True
        Me.lblAddress.Location = New System.Drawing.Point(274, 47)
        Me.lblAddress.Name = "lblAddress"
        Me.lblAddress.Size = New System.Drawing.Size(45, 13)
        Me.lblAddress.TabIndex = 53
        Me.lblAddress.Text = "Address"
        '
        'lblOrig
        '
        Me.lblOrig.AutoSize = True
        Me.lblOrig.Location = New System.Drawing.Point(238, 67)
        Me.lblOrig.Name = "lblOrig"
        Me.lblOrig.Size = New System.Drawing.Size(29, 13)
        Me.lblOrig.TabIndex = 51
        Me.lblOrig.Text = "Stop"
        '
        'txtOrder1
        '
        Me.txtOrder1.Location = New System.Drawing.Point(4, 20)
        Me.txtOrder1.Multiline = True
        Me.txtOrder1.Name = "txtOrder1"
        Me.txtOrder1.Size = New System.Drawing.Size(141, 100)
        Me.txtOrder1.TabIndex = 79
        Me.txtOrder1.WordWrap = False
        '
        'txtOrder2
        '
        Me.txtOrder2.Location = New System.Drawing.Point(4, 126)
        Me.txtOrder2.Multiline = True
        Me.txtOrder2.Name = "txtOrder2"
        Me.txtOrder2.Size = New System.Drawing.Size(141, 100)
        Me.txtOrder2.TabIndex = 80
        Me.txtOrder2.WordWrap = False
        '
        'txtOrder3
        '
        Me.txtOrder3.Location = New System.Drawing.Point(4, 232)
        Me.txtOrder3.Multiline = True
        Me.txtOrder3.Name = "txtOrder3"
        Me.txtOrder3.Size = New System.Drawing.Size(141, 100)
        Me.txtOrder3.TabIndex = 81
        Me.txtOrder3.WordWrap = False
        '
        'txtOrder4
        '
        Me.txtOrder4.Location = New System.Drawing.Point(1, 338)
        Me.txtOrder4.Multiline = True
        Me.txtOrder4.Name = "txtOrder4"
        Me.txtOrder4.Size = New System.Drawing.Size(141, 100)
        Me.txtOrder4.TabIndex = 82
        Me.txtOrder4.WordWrap = False
        '
        'lblCarriers
        '
        Me.lblCarriers.AutoSize = True
        Me.lblCarriers.Location = New System.Drawing.Point(148, 448)
        Me.lblCarriers.Name = "lblCarriers"
        Me.lblCarriers.Size = New System.Drawing.Size(122, 13)
        Me.lblCarriers.TabIndex = 84
        Me.lblCarriers.Text = "Transportation Providers"
        '
        'dgvCarriers
        '
        Me.dgvCarriers.AllowUserToAddRows = False
        Me.dgvCarriers.AllowUserToDeleteRows = False
        Me.dgvCarriers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCarriers.Location = New System.Drawing.Point(151, 466)
        Me.dgvCarriers.MaximumSize = New System.Drawing.Size(1013, 128)
        Me.dgvCarriers.MinimumSize = New System.Drawing.Size(1013, 128)
        Me.dgvCarriers.Name = "dgvCarriers"
        Me.dgvCarriers.ReadOnly = True
        Me.dgvCarriers.Size = New System.Drawing.Size(1013, 128)
        Me.dgvCarriers.TabIndex = 83
        '
        'Form3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1233, 620)
        Me.Controls.Add(Me.lblCarriers)
        Me.Controls.Add(Me.dgvCarriers)
        Me.Controls.Add(Me.txtOrder4)
        Me.Controls.Add(Me.txtOrder3)
        Me.Controls.Add(Me.txtOrder2)
        Me.Controls.Add(Me.txtOrder1)
        Me.Controls.Add(Me.lblSolution)
        Me.Controls.Add(Me.dgvSolution)
        Me.Controls.Add(Me.lblStops)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.lblPCMBadAddressMsg)
        Me.Controls.Add(Me.lblBadAddress)
        Me.Controls.Add(Me.btnRun)
        Me.Controls.Add(Me.dgvBadAddress)
        Me.Controls.Add(Me.lblErrors)
        Me.Controls.Add(Me.btnAddStop)
        Me.Controls.Add(Me.dgvStops)
        Me.Controls.Add(Me.chkUseCom)
        Me.Controls.Add(Me.cboDistanceType)
        Me.Controls.Add(Me.lblDistanceType)
        Me.Controls.Add(Me.cboRouteType)
        Me.Controls.Add(Me.lblRouteType)
        Me.Controls.Add(Me.tbStopZip)
        Me.Controls.Add(Me.tbStopState)
        Me.Controls.Add(Me.tbStopCity)
        Me.Controls.Add(Me.tbStopAddress)
        Me.Controls.Add(Me.lblZip)
        Me.Controls.Add(Me.lblState)
        Me.Controls.Add(Me.lblCity)
        Me.Controls.Add(Me.lblAddress)
        Me.Controls.Add(Me.lblOrig)
        Me.Name = "Form3"
        Me.Text = "Form3"
        CType(Me.dgvSolution, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvBadAddress, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvStops, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvCarriers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblSolution As System.Windows.Forms.Label
    Friend WithEvents dgvSolution As System.Windows.Forms.DataGridView
    Friend WithEvents lblStops As System.Windows.Forms.Label
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents lblPCMBadAddressMsg As System.Windows.Forms.Label
    Friend WithEvents lblBadAddress As System.Windows.Forms.Label
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents dgvBadAddress As System.Windows.Forms.DataGridView
    Friend WithEvents lblErrors As System.Windows.Forms.Label
    Friend WithEvents btnAddStop As System.Windows.Forms.Button
    Friend WithEvents dgvStops As System.Windows.Forms.DataGridView
    Friend WithEvents chkUseCom As System.Windows.Forms.CheckBox
    Friend WithEvents cboDistanceType As System.Windows.Forms.ComboBox
    Friend WithEvents lblDistanceType As System.Windows.Forms.Label
    Friend WithEvents cboRouteType As System.Windows.Forms.ComboBox
    Friend WithEvents lblRouteType As System.Windows.Forms.Label
    Friend WithEvents tbStopZip As System.Windows.Forms.TextBox
    Friend WithEvents tbStopState As System.Windows.Forms.TextBox
    Friend WithEvents tbStopCity As System.Windows.Forms.TextBox
    Friend WithEvents tbStopAddress As System.Windows.Forms.TextBox
    Friend WithEvents lblZip As System.Windows.Forms.Label
    Friend WithEvents lblState As System.Windows.Forms.Label
    Friend WithEvents lblCity As System.Windows.Forms.Label
    Friend WithEvents lblAddress As System.Windows.Forms.Label
    Friend WithEvents lblOrig As System.Windows.Forms.Label
    Friend WithEvents txtOrder1 As System.Windows.Forms.TextBox
    Friend WithEvents txtOrder2 As System.Windows.Forms.TextBox
    Friend WithEvents txtOrder3 As System.Windows.Forms.TextBox
    Friend WithEvents txtOrder4 As System.Windows.Forms.TextBox
    Friend WithEvents lblCarriers As System.Windows.Forms.Label
    Friend WithEvents dgvCarriers As System.Windows.Forms.DataGridView
End Class
