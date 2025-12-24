Public Class BookTestObject
    Private _BookHeaders As BookObjectWS.clsBookHeaderObject70()
    Public Property BookHeaders() As BookObjectWS.clsBookHeaderObject70()
        Get
            Return _BookHeaders
        End Get
        Set(ByVal value As BookObjectWS.clsBookHeaderObject70())
            _BookHeaders = value
        End Set
    End Property

    Private _BookDetails As BookObjectWS.clsBookDetailObject70()
    Public Property BookDetails() As BookObjectWS.clsBookDetailObject70()
        Get
            Return _BookDetails
        End Get
        Set(ByVal value As BookObjectWS.clsBookDetailObject70())
            _BookDetails = value
        End Set
    End Property


    Public Sub fillJuiceBookTestData()

        Dim hdrs As New List(Of BookObjectWS.clsBookHeaderObject70)
        Dim dets As New List(Of BookObjectWS.clsBookDetailObject70)
        Dim oHeader As New BookObjectWS.clsBookHeaderObject70
        With oHeader
            .PONumber = "569502"
            .POVendor = "201-1TROP-01"
            .POdate = "04/01/2019 00:00:00"
            .POShipdate = "03/30/2019 00:00:00"
            .POFrt = "4"
            .POTotalFrt = "0"
            .POTotalCost = "385.37"
            .POWgt = "0"
            .POCube = "0"
            .POQty = "433"
            .POPallets = "0"
            .POLines = "1"
            .POConfirm = "false"
            .PODefaultCustomer = "0"
            .PODefaultCarrier = "0"
            .POReqDate = "03/30/2019 00:00:00"
            '.POShipInstructions/ = "
            .POCooler = "true"
            .POFrozen = "false"
            .PODry = "false"
            .POTemp = "C"
            '.POCarType/ = "
            .POShipVia = "53"
            .POShipViaType = "ShipViaCode"
            '.POConsigneeNumber/ = "
            .POCustomerPO = "4520649624"
            .POOtherCosts = "0"
            .POStatusFlag = "0"
            .POOrderSequence = "0"
            '.POChepGLID/ = "
            .POOrigName = "NATURAL COUNTRY FARMS"
            .POOrigAddress1 = "58 WEST ROAD"
            .POOrigAddress2 = " "
            .POOrigCity = "ELLINGTON"
            .POOrigState = "CT"
            .POOrigCountry = "US"
            .POOrigZip = "06029"
            .PODestName = "TROPICANA DOLE BEVERAGE, N.A."
            .PODestAddress1 = " "
            .PODestAddress2 = " "
            .PODestAddress3 = " "
            .PODestCity = " "
            .PODestState = "CT"
            .PODestCountry = "US"
            .PODestZip = " "
            .POInbound = "false"
            .POPalletExchange = "false"
            .PODefaultRouteSequence = "0"
            .POCompAlphaCode = "201"
            .POModeTypeControl = "0"
        End With
        hdrs.Add(oHeader)
        Dim oDetail As New BookObjectWS.clsBookDetailObject70()
        With oDetail
            .ItemPONumber = "569502"
            .FixOffInvAllow = "0"
            .FixFrtAllow = "0"
            .ItemNumber = "BXPALLETPADS"
            .QtyOrdered = "433"
            .FreightCost = "0"
            .ItemCost = "0.89"
            .Weight = "0"
            .Cube = "0"
            .Pack = "0"
            .Description = "6-64  SLIP SHEETS"
            .CustomerNumber = "201"
            .POOrderSequence = "0"
            .PalletType = "N"
            .POItemLimitedQtyFlag = "false"
            .POItemPallets = "0"
            .POItemTies = "0"
            .POItemHighs = "0"
            .POItemQtyPalletPercentage = "0"
            .POItemQtyLength = "0"
            .POItemQtyWidth = "0"
            .POItemQtyHeight = "0"
            .POItemStackable = "false"
            .POItemLevelOfDensity = "0"
            .POItemCompAlphaCode = "201"
        End With
        dets.Add(oDetail)
        Me.BookHeaders = hdrs.ToArray()
        Me.BookDetails = dets.ToArray()

    End Sub

End Class
