<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form2))
        Me.tbStopZip = New System.Windows.Forms.TextBox()
        Me.tbStopState = New System.Windows.Forms.TextBox()
        Me.tbStopCity = New System.Windows.Forms.TextBox()
        Me.tbStopAddress = New System.Windows.Forms.TextBox()
        Me.lblZip = New System.Windows.Forms.Label()
        Me.lblState = New System.Windows.Forms.Label()
        Me.lblCity = New System.Windows.Forms.Label()
        Me.lblAddress = New System.Windows.Forms.Label()
        Me.lblOrig = New System.Windows.Forms.Label()
        Me.chkUseCom = New System.Windows.Forms.CheckBox()
        Me.cboDistanceType = New System.Windows.Forms.ComboBox()
        Me.lblDistanceType = New System.Windows.Forms.Label()
        Me.cboRouteType = New System.Windows.Forms.ComboBox()
        Me.lblRouteType = New System.Windows.Forms.Label()
        Me.dgvStops = New System.Windows.Forms.DataGridView()
        Me.btnAddStop = New System.Windows.Forms.Button()
        Me.lblErrors = New System.Windows.Forms.Label()
        Me.dgvBadAddress = New System.Windows.Forms.DataGridView()
        Me.btnRun = New System.Windows.Forms.Button()
        Me.lblBadAddress = New System.Windows.Forms.Label()
        Me.lblPCMBadAddressMsg = New System.Windows.Forms.Label()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.lblStops = New System.Windows.Forms.Label()
        Me.dgvSolution = New System.Windows.Forms.DataGridView()
        Me.lblSolution = New System.Windows.Forms.Label()
        Me.btnCATest = New System.Windows.Forms.Button()
        Me.btnCA2PickTest = New System.Windows.Forms.Button()
        Me.btnCAKeepStopTest = New System.Windows.Forms.Button()
        CType(Me.dgvStops, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvBadAddress, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvSolution, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tbStopZip
        '
        Me.tbStopZip.Location = New System.Drawing.Point(658, 67)
        Me.tbStopZip.Name = "tbStopZip"
        Me.tbStopZip.Size = New System.Drawing.Size(100, 20)
        Me.tbStopZip.TabIndex = 12
        '
        'tbStopState
        '
        Me.tbStopState.Location = New System.Drawing.Point(590, 67)
        Me.tbStopState.Name = "tbStopState"
        Me.tbStopState.Size = New System.Drawing.Size(62, 20)
        Me.tbStopState.TabIndex = 10
        '
        'tbStopCity
        '
        Me.tbStopCity.Location = New System.Drawing.Point(447, 67)
        Me.tbStopCity.Name = "tbStopCity"
        Me.tbStopCity.Size = New System.Drawing.Size(137, 20)
        Me.tbStopCity.TabIndex = 8
        '
        'tbStopAddress
        '
        Me.tbStopAddress.Location = New System.Drawing.Point(144, 67)
        Me.tbStopAddress.Name = "tbStopAddress"
        Me.tbStopAddress.Size = New System.Drawing.Size(297, 20)
        Me.tbStopAddress.TabIndex = 6
        '
        'lblZip
        '
        Me.lblZip.AutoSize = True
        Me.lblZip.Location = New System.Drawing.Point(661, 47)
        Me.lblZip.Name = "lblZip"
        Me.lblZip.Size = New System.Drawing.Size(64, 13)
        Me.lblZip.TabIndex = 13
        Me.lblZip.Text = "Postal Code"
        '
        'lblState
        '
        Me.lblState.AutoSize = True
        Me.lblState.Location = New System.Drawing.Point(590, 47)
        Me.lblState.Name = "lblState"
        Me.lblState.Size = New System.Drawing.Size(59, 13)
        Me.lblState.TabIndex = 11
        Me.lblState.Text = "State/Prov"
        '
        'lblCity
        '
        Me.lblCity.AutoSize = True
        Me.lblCity.Location = New System.Drawing.Point(451, 47)
        Me.lblCity.Name = "lblCity"
        Me.lblCity.Size = New System.Drawing.Size(24, 13)
        Me.lblCity.TabIndex = 9
        Me.lblCity.Text = "City"
        '
        'lblAddress
        '
        Me.lblAddress.AutoSize = True
        Me.lblAddress.Location = New System.Drawing.Point(141, 47)
        Me.lblAddress.Name = "lblAddress"
        Me.lblAddress.Size = New System.Drawing.Size(45, 13)
        Me.lblAddress.TabIndex = 7
        Me.lblAddress.Text = "Address"
        '
        'lblOrig
        '
        Me.lblOrig.AutoSize = True
        Me.lblOrig.Location = New System.Drawing.Point(105, 67)
        Me.lblOrig.Name = "lblOrig"
        Me.lblOrig.Size = New System.Drawing.Size(29, 13)
        Me.lblOrig.TabIndex = 5
        Me.lblOrig.Text = "Stop"
        '
        'chkUseCom
        '
        Me.chkUseCom.AutoSize = True
        Me.chkUseCom.Location = New System.Drawing.Point(587, 19)
        Me.chkUseCom.Name = "chkUseCom"
        Me.chkUseCom.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkUseCom.Size = New System.Drawing.Size(72, 17)
        Me.chkUseCom.TabIndex = 36
        Me.chkUseCom.Text = "Use COM"
        Me.chkUseCom.UseVisualStyleBackColor = True
        '
        'cboDistanceType
        '
        Me.cboDistanceType.FormattingEnabled = True
        Me.cboDistanceType.Location = New System.Drawing.Point(435, 14)
        Me.cboDistanceType.Name = "cboDistanceType"
        Me.cboDistanceType.Size = New System.Drawing.Size(121, 21)
        Me.cboDistanceType.TabIndex = 35
        '
        'lblDistanceType
        '
        Me.lblDistanceType.AutoSize = True
        Me.lblDistanceType.Location = New System.Drawing.Point(353, 19)
        Me.lblDistanceType.Name = "lblDistanceType"
        Me.lblDistanceType.Size = New System.Drawing.Size(76, 13)
        Me.lblDistanceType.TabIndex = 34
        Me.lblDistanceType.Text = "Distance Type"
        '
        'cboRouteType
        '
        Me.cboRouteType.FormattingEnabled = True
        Me.cboRouteType.Location = New System.Drawing.Point(211, 15)
        Me.cboRouteType.Name = "cboRouteType"
        Me.cboRouteType.Size = New System.Drawing.Size(121, 21)
        Me.cboRouteType.TabIndex = 33
        '
        'lblRouteType
        '
        Me.lblRouteType.AutoSize = True
        Me.lblRouteType.Location = New System.Drawing.Point(141, 20)
        Me.lblRouteType.Name = "lblRouteType"
        Me.lblRouteType.Size = New System.Drawing.Size(63, 13)
        Me.lblRouteType.TabIndex = 32
        Me.lblRouteType.Text = "Route Type"
        '
        'dgvStops
        '
        Me.dgvStops.AllowUserToAddRows = False
        Me.dgvStops.AllowUserToDeleteRows = False
        Me.dgvStops.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvStops.Location = New System.Drawing.Point(19, 121)
        Me.dgvStops.Name = "dgvStops"
        Me.dgvStops.ReadOnly = True
        Me.dgvStops.Size = New System.Drawing.Size(500, 128)
        Me.dgvStops.TabIndex = 37
        '
        'btnAddStop
        '
        Me.btnAddStop.Location = New System.Drawing.Point(18, 64)
        Me.btnAddStop.Name = "btnAddStop"
        Me.btnAddStop.Size = New System.Drawing.Size(75, 23)
        Me.btnAddStop.TabIndex = 38
        Me.btnAddStop.Text = "Add Stop"
        Me.btnAddStop.UseVisualStyleBackColor = True
        '
        'lblErrors
        '
        Me.lblErrors.AutoSize = True
        Me.lblErrors.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblErrors.ForeColor = System.Drawing.Color.Red
        Me.lblErrors.Location = New System.Drawing.Point(17, 252)
        Me.lblErrors.MaximumSize = New System.Drawing.Size(300, 100)
        Me.lblErrors.Name = "lblErrors"
        Me.lblErrors.Size = New System.Drawing.Size(44, 13)
        Me.lblErrors.TabIndex = 39
        Me.lblErrors.Text = "Errors:"
        '
        'dgvBadAddress
        '
        Me.dgvBadAddress.AllowUserToAddRows = False
        Me.dgvBadAddress.AllowUserToDeleteRows = False
        Me.dgvBadAddress.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvBadAddress.Location = New System.Drawing.Point(531, 121)
        Me.dgvBadAddress.Name = "dgvBadAddress"
        Me.dgvBadAddress.ReadOnly = True
        Me.dgvBadAddress.Size = New System.Drawing.Size(500, 128)
        Me.dgvBadAddress.TabIndex = 40
        '
        'btnRun
        '
        Me.btnRun.Location = New System.Drawing.Point(18, 37)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(75, 23)
        Me.btnRun.TabIndex = 41
        Me.btnRun.Text = "Run"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'lblBadAddress
        '
        Me.lblBadAddress.AutoSize = True
        Me.lblBadAddress.Location = New System.Drawing.Point(528, 99)
        Me.lblBadAddress.Name = "lblBadAddress"
        Me.lblBadAddress.Size = New System.Drawing.Size(154, 13)
        Me.lblBadAddress.TabIndex = 42
        Me.lblBadAddress.Text = "PCM Version of Bad Addresses"
        '
        'lblPCMBadAddressMsg
        '
        Me.lblPCMBadAddressMsg.AutoSize = True
        Me.lblPCMBadAddressMsg.Location = New System.Drawing.Point(529, 252)
        Me.lblPCMBadAddressMsg.MaximumSize = New System.Drawing.Size(300, 100)
        Me.lblPCMBadAddressMsg.Name = "lblPCMBadAddressMsg"
        Me.lblPCMBadAddressMsg.Size = New System.Drawing.Size(130, 13)
        Me.lblPCMBadAddressMsg.TabIndex = 43
        Me.lblPCMBadAddressMsg.Text = "PCMBadAddressMessage"
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(18, 8)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(75, 23)
        Me.btnClear.TabIndex = 44
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'lblStops
        '
        Me.lblStops.AutoSize = True
        Me.lblStops.Location = New System.Drawing.Point(17, 99)
        Me.lblStops.Name = "lblStops"
        Me.lblStops.Size = New System.Drawing.Size(82, 13)
        Me.lblStops.TabIndex = 45
        Me.lblStops.Text = "Stops To Route"
        '
        'dgvSolution
        '
        Me.dgvSolution.AllowUserToAddRows = False
        Me.dgvSolution.AllowUserToDeleteRows = False
        Me.dgvSolution.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSolution.Location = New System.Drawing.Point(18, 374)
        Me.dgvSolution.Name = "dgvSolution"
        Me.dgvSolution.ReadOnly = True
        Me.dgvSolution.Size = New System.Drawing.Size(1013, 128)
        Me.dgvSolution.TabIndex = 46
        '
        'lblSolution
        '
        Me.lblSolution.AutoSize = True
        Me.lblSolution.Location = New System.Drawing.Point(15, 356)
        Me.lblSolution.Name = "lblSolution"
        Me.lblSolution.Size = New System.Drawing.Size(45, 13)
        Me.lblSolution.TabIndex = 47
        Me.lblSolution.Text = "Solution"
        '
        'btnCATest
        '
        Me.btnCATest.Location = New System.Drawing.Point(774, 10)
        Me.btnCATest.Name = "btnCATest"
        Me.btnCATest.Size = New System.Drawing.Size(136, 23)
        Me.btnCATest.TabIndex = 48
        Me.btnCATest.Text = "Run CA 1 Pick Test"
        Me.btnCATest.UseVisualStyleBackColor = True
        '
        'btnCA2PickTest
        '
        Me.btnCA2PickTest.Location = New System.Drawing.Point(774, 39)
        Me.btnCA2PickTest.Name = "btnCA2PickTest"
        Me.btnCA2PickTest.Size = New System.Drawing.Size(136, 23)
        Me.btnCA2PickTest.TabIndex = 49
        Me.btnCA2PickTest.Text = "Run CA 2 Pick Test"
        Me.btnCA2PickTest.UseVisualStyleBackColor = True
        '
        'btnCAKeepStopTest
        '
        Me.btnCAKeepStopTest.Location = New System.Drawing.Point(774, 68)
        Me.btnCAKeepStopTest.Name = "btnCAKeepStopTest"
        Me.btnCAKeepStopTest.Size = New System.Drawing.Size(136, 23)
        Me.btnCAKeepStopTest.TabIndex = 50
        Me.btnCAKeepStopTest.Text = "Run CA keep stop # Test"
        Me.btnCAKeepStopTest.UseVisualStyleBackColor = True
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1049, 533)
        Me.Controls.Add(Me.btnCAKeepStopTest)
        Me.Controls.Add(Me.btnCA2PickTest)
        Me.Controls.Add(Me.btnCATest)
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
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form2"
        Me.Text = "Stop Resequence"
        CType(Me.dgvStops, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvBadAddress, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvSolution, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbStopZip As System.Windows.Forms.TextBox
    Friend WithEvents tbStopState As System.Windows.Forms.TextBox
    Friend WithEvents tbStopCity As System.Windows.Forms.TextBox
    Friend WithEvents tbStopAddress As System.Windows.Forms.TextBox
    Friend WithEvents lblZip As System.Windows.Forms.Label
    Friend WithEvents lblState As System.Windows.Forms.Label
    Friend WithEvents lblCity As System.Windows.Forms.Label
    Friend WithEvents lblAddress As System.Windows.Forms.Label
    Friend WithEvents lblOrig As System.Windows.Forms.Label
    Friend WithEvents chkUseCom As System.Windows.Forms.CheckBox
    Friend WithEvents cboDistanceType As System.Windows.Forms.ComboBox
    Friend WithEvents lblDistanceType As System.Windows.Forms.Label
    Friend WithEvents cboRouteType As System.Windows.Forms.ComboBox
    Friend WithEvents lblRouteType As System.Windows.Forms.Label
    Friend WithEvents dgvStops As System.Windows.Forms.DataGridView
    Friend WithEvents btnAddStop As System.Windows.Forms.Button
    Friend WithEvents lblErrors As System.Windows.Forms.Label
    Friend WithEvents dgvBadAddress As System.Windows.Forms.DataGridView
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents lblBadAddress As System.Windows.Forms.Label
    Friend WithEvents lblPCMBadAddressMsg As System.Windows.Forms.Label
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents lblStops As System.Windows.Forms.Label
    Friend WithEvents dgvSolution As System.Windows.Forms.DataGridView
    Friend WithEvents lblSolution As System.Windows.Forms.Label
    Friend WithEvents btnCATest As System.Windows.Forms.Button
    Friend WithEvents btnCA2PickTest As System.Windows.Forms.Button
    Friend WithEvents btnCAKeepStopTest As System.Windows.Forms.Button
End Class
