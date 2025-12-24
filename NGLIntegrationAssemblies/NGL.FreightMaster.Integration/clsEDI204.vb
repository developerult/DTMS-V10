Imports Ngl.FreightMaster.Integration.Configuration
Imports System.Data.SqlClient
Imports NGL.FreightMaster.Integration.FMDataTableAdapters
Imports NDT = Ngl.Core.Utility.DataTransformation
Imports DAL = NGL.FreightMaster.Data 'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects 'Added by LVV 6/15/16 for v-7.0.5.1 EDI Migration
Imports LTS = Ngl.FreightMaster.Data.LTS
''' <summary>
''' Used to generate an EDI 204 Outbound Message
''' </summary>
''' <remarks>
''' Modified by RHR on 08/08/2018 for v-6.0.4.4-m
'''   added logic to process GlobalEDI204MultipleG62On parameter
''' </remarks>
<Serializable()>
Public Class clsEDI204 : Inherits clsUpload

#Region " Class Variables and Properties "

    Private _Adapter As spGetEDI204TruckLoadDataTableAdapter
    Protected ReadOnly Property Adapter() As spGetEDI204TruckLoadDataTableAdapter
        Get
            If _Adapter Is Nothing Then
                _Adapter = New spGetEDI204TruckLoadDataTableAdapter
                _Adapter.SetConnectionString(Me.DBConnection)
            End If

            Return _Adapter
        End Get
    End Property

    Private _ItemAdapter As spgetEDI204ItemDetailsTableAdapter
    Protected ReadOnly Property ItemAdapter() As spgetEDI204ItemDetailsTableAdapter
        Get
            If _ItemAdapter Is Nothing Then
                _ItemAdapter = New spgetEDI204ItemDetailsTableAdapter
                _ItemAdapter.SetConnectionString(Me.DBConnection)
            End If

            Return _ItemAdapter
        End Get
    End Property

    Private _dictItemDetails As New Dictionary(Of Integer, FMData.spgetEDI204ItemDetailsDataTable)
    Public Property dictItemDetails() As Dictionary(Of Integer, FMData.spgetEDI204ItemDetailsDataTable)
        Get
            Return _dictItemDetails
        End Get
        Set(ByVal value As Dictionary(Of Integer, FMData.spgetEDI204ItemDetailsDataTable))
            _dictItemDetails = value
        End Set
    End Property

    'Begin Modified by RHR on 08/08/2018 for v-6.0.4.4-m
    Private _dblGlobalEDI204MultipleG62On As Double = -1
    Public ReadOnly Property dblGlobalEDI204MultipleG62On() As Double
        Get
            If _dblGlobalEDI204MultipleG62On = -1 Then
                _dblGlobalEDI204MultipleG62On = Me.getParValue("GlobalEDI204MultipleG62On", 0)
            End If
            Return _dblGlobalEDI204MultipleG62On
        End Get
    End Property
    'End Modified by RHR on 08/08/2018 for v-6.0.4.4-m
    '**************************************************************
    'Added By LVV on 2/7/18 for v-8.1 PQ EDI
    Private _EDI204OutSetting As New clsEDI204OutSetting()
    Public Property EDI204OutSetting() As clsEDI204OutSetting
        Get
            If _EDI204OutSetting Is Nothing Then _EDI204OutSetting = New clsEDI204OutSetting()
            Return _EDI204OutSetting
        End Get
        Set(ByVal value As clsEDI204OutSetting)
            If value Is Nothing Then value = New clsEDI204OutSetting()
            _EDI204OutSetting = value
        End Set
    End Property

    Public ReadOnly Property ShowItemDetailDescrtiption() As Integer
        Get
            Dim blnShowItemDetailDescrtiption As Boolean = True
            Boolean.TryParse(Me.EDI204OutSetting.MappingRules.ShowItemDetailDescrtiption, blnShowItemDetailDescrtiption)
            Return blnShowItemDetailDescrtiption
        End Get
    End Property

    Public ReadOnly Property ShowEstimatedCarrierCosts() As Integer
        Get
            Dim blnShowEstimatedCarrierCosts As Boolean = True
            Boolean.TryParse(Me.EDI204OutSetting.MappingRules.ShowEstimatedCarrierCosts, blnShowEstimatedCarrierCosts)
            Return blnShowEstimatedCarrierCosts
        End Get
    End Property

    Public ReadOnly Property IncludeNotes1() As Boolean
        Get
            Dim blnIncludeNotes1 As Boolean = False
            Boolean.TryParse(Me.EDI204OutSetting.MappingRules.IncludeNotes1, blnIncludeNotes1)
            Return blnIncludeNotes1
        End Get
    End Property

    Public ReadOnly Property IncludeNotes2() As Boolean
        Get
            Dim blnIncludeNotes2 As Boolean = False
            Boolean.TryParse(Me.EDI204OutSetting.MappingRules.IncludeNotes2, blnIncludeNotes2)
            Return blnIncludeNotes2
        End Get
    End Property

    Public ReadOnly Property IncludeNotes3() As Boolean
        Get
            Dim blnIncludeNotes3 As Boolean = False
            Boolean.TryParse(Me.EDI204OutSetting.MappingRules.IncludeNotes3, blnIncludeNotes3)
            Return blnIncludeNotes3
        End Get
    End Property

    Public ReadOnly Property IncludeItems() As Boolean
        Get
            Dim blnIncludeItems As Boolean = True
            Boolean.TryParse(Me.EDI204OutSetting.MappingRules.IncludeItems, blnIncludeItems)
            Return blnIncludeItems
        End Get
    End Property

    Public ReadOnly Property ShowContractedCost() As Boolean
        Get
            Dim blnShowContractedCost As Boolean = True
            Boolean.TryParse(Me.EDI204OutSetting.MappingRules.ShowContractedCost, blnShowContractedCost)
            Return blnShowContractedCost
        End Get
    End Property

    '**************************************************************

#End Region


#Region " Constructors "



#End Region

#Region " Functions "

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)>
    Protected Function GetTruckLoadData(ByVal CarrierControl As Integer) As FMData.spGetEDI204TruckLoadDataDataTable
        Adapter.SetCommandTimeOut(Me.CommandTimeOut)
        Return (Adapter.GetData(CarrierControl))
    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)>
    Protected Function GetItemDetailData(ByVal BookControl As Integer) As FMData.spgetEDI204ItemDetailsDataTable
        If dictItemDetails Is Nothing Then
            dictItemDetails = New Dictionary(Of Integer, FMData.spgetEDI204ItemDetailsDataTable)
        ElseIf dictItemDetails.ContainsKey(BookControl) Then
            Return dictItemDetails(BookControl)
        End If
        ItemAdapter.SetCommandTimeOut(Me.CommandTimeOut)
        Dim oDetails = ItemAdapter.GetData(BookControl)
        If Not oDetails Is Nothing Then
            dictItemDetails.Add(BookControl, oDetails)
        End If
        Return oDetails
    End Function

    ''' <summary>
    ''' Construct the EDI string from the object collections
    ''' </summary>
    ''' <param name="e"></param>
    ''' <param name="sSegTerm"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR on 05/30/2018 for v-6.0.4.4-m
    '''   added logic to process a list of NTE objects in the 300 loop 
    ''' Modified by RHR on 08/08/2018 for v-6.0.4.4-m
    '''   added logic to process multiple G62 segments if they exist
    ''' </remarks>
    Protected Function getTruckLoadEDIString(ByVal e As clsEDITruckLoad, ByVal sSegTerm As String, Optional ByVal Notes1 As String = "", Optional ByVal Notes2 As String = "", Optional ByRef Notes3 As String = "") As String
        Dim sEdi As New System.Text.StringBuilder("", 500)
        With e
            sEdi.AppendFormat("ST*{0}*{1}{2}", .ST.ST01, .ST.ST02, sSegTerm)
            sEdi.AppendFormat("B2*{0}*{1}*{2}*{3}*{4}*{5}{6}", .B2.B201, .B2.B202, .B2.B203, .B2.B204, .B2.B205, .B2.B206, sSegTerm)
            sEdi.AppendFormat("B2A*{0}*{1}{2}", .B2A.B2A01, .B2A.B2A02, sSegTerm)
            sEdi.AppendFormat("L11*{0}*{1}*{2}{3}", .L11.L1101, .L11.L1102, .L11.L1103, sSegTerm)
            sEdi.AppendFormat("PLD*{0}{1}", .PLD.PLD01, sSegTerm)
            If Not .Loop100 Is Nothing AndAlso .Loop100.Count > 0 Then
                For Each o100 In .Loop100
                    sEdi.AppendFormat("N1*{0}*{1}*{2}*{3}{4}", o100.N1.N101, o100.N1.N102, o100.N1.N103, o100.N1.N104, sSegTerm)
                    sEdi.AppendFormat("N3*{0}*{1}{2}", o100.N3.N301, o100.N3.N302, sSegTerm)
                    sEdi.AppendFormat("N4*{0}*{1}*{2}*{3}*{4}*{5}{6}", o100.N4.N401, o100.N4.N402, o100.N4.N403, o100.N4.N404, o100.N4.N405, o100.N4.N406, sSegTerm)
                Next

            End If
            If Not .Loop200 Is Nothing AndAlso .Loop200.Count > 0 Then
                For Each o200 In .Loop200
                    sEdi.AppendFormat("N7*{0}*{1}*{2}*{3}*{4}*{5}*{6}*{7}*{8}*{9}*{10}{11}", o200.N7.N701, o200.N7.N702, o200.N7.N703, o200.N7.N704, o200.N7.N705, o200.N7.N706, o200.N7.N707, o200.N7.N708, o200.N7.N709, o200.N7.N710, o200.N7.N711, sSegTerm)
                Next

            End If

            For Each o300L As clsEDI204Loop300 In .Loop300
                With o300L
                    sEdi.AppendFormat("S5*{0}*{1}*{2}*{3}*{4}*{5}*{6}*{7}{8}", .S5.S501, .S5.S502, .S5.S503, .S5.S504, .S5.S505, .S5.S506, .S5.S507, .S5.S508, sSegTerm)
                    For Each oL11 As clsEDIL11 In .L11s
                        sEdi.AppendFormat("L11*{0}*{1}*{2}{3}", oL11.L1101, oL11.L1102, oL11.L1103, sSegTerm)
                    Next
                    'Modified by RHR on 08/08/2018 for v-6.0.4.4-m
                    If Not .G62s Is Nothing AndAlso .G62s.Count > 0 Then
                        For Each oG62 In .G62s
                            sEdi.AppendFormat("G62*{0}*{1}*{2}*{3}*{4}{5}", oG62.G6201, oG62.G6202, oG62.G6203, oG62.G6204, oG62.G6205, sSegTerm)
                        Next
                    End If
                    sEdi.AppendFormat("PLD*{0}{1}", .PLD.PLD01, sSegTerm)
                    'Modified by RHR on 05/30/2018 for v-6.0.4.4-m
                    For Each oNTE In .NTEs
                        If oNTE.NTE01.Trim.Length > 0 Then
                            'add the NTE Segment
                            sEdi.AppendFormat("NTE*{0}*{1}{2}", oNTE.NTE01, oNTE.NTE02, sSegTerm)
                        End If
                    Next

                    'Added By LVV on 2/7/18 for v-8.1 PQ EDI
                    'Add logic to loop through each of the note strings that are not empty and split them into 80 char segments
                    'for each 80 char segment call sEdi.AppendFormat("NTE*{0}*{1}{2}", LOI or DEL, Notes 80 char seg, sSegTerm)
                    ' Modified by RHR for v-8.5.3.007 on 03/23/2023 added logic to 
                    '     increment intSegments counter to be added to SE01 
                    '     Note: if the first 3 characters in each Notes does not start with 
                    '     LOI (pickup) the qualifier will be DEL (delivery)
                    Dim intSegments As Integer = 0
                    AddVisibleNotes(sEdi, Notes1, sSegTerm, intSegments)
                    AddVisibleNotes(sEdi, Notes2, sSegTerm, intSegments)
                    AddVisibleNotes(sEdi, Notes3, sSegTerm, intSegments)
                    'now update the SE01 value
                    e.SE.SE01 += intSegments
                    If Not .Loop310 Is Nothing AndAlso .Loop310.Count > 0 Then
                        For Each oL310 As clsEDI204Loop310 In .Loop310
                            With oL310
                                sEdi.AppendFormat("N1*{0}*{1}*{2}*{3}{4}", .N1.N101, .N1.N102, .N1.N103, .N1.N104, sSegTerm)
                                sEdi.AppendFormat("N3*{0}*{1}{2}", .N3.N301, .N3.N302, sSegTerm)
                                sEdi.AppendFormat("N4*{0}*{1}*{2}*{3}{4}", .N4.N401, .N4.N402, .N4.N403, .N4.N404, sSegTerm)
                            End With
                        Next
                    End If

                    If Not .Loop320 Is Nothing AndAlso .Loop320.Count > 0 Then
                        For Each oL320 As clsEDI204Loop320 In .Loop320
                            With oL320
                                'Added By LVV on 2/7/18 for v-8.1 PQ EDI
                                If IncludeItems Then
                                    sEdi.AppendFormat("L5*{0}*{1}*{2}*{3}{4}", .L5.L501, .L5.L502, .L5.L503, .L5.L504, sSegTerm)
                                    sEdi.AppendFormat("AT8*{0}*{1}*{2}*{3}*{4}*{5}*{6}{7}", .AT8.AT801, .AT8.AT802, .AT8.AT803, .AT8.AT804, .AT8.AT805, .AT8.AT806, .AT8.AT807, sSegTerm)
                                End If
                            End With
                        Next
                    End If
                End With
            Next
            'sEdi.AppendFormat("L3*{0}*{1}*{2}*{3}*{4}*{5}", .L3.L301, .L3.L302, .L3.L303, .L3.L304, .L3.L305, .L3.L306)
            'sEdi.AppendFormat("*{0}", .L3.L307)
            'sEdi.AppendFormat("*{0}", .L3.L308)
            'sEdi.AppendFormat("*[0}", .L3.L309)

            'sEdi.AppendFormat("*{0}*{1}*{2}", .L3.L310, .L3.L311, .L3.L312)
            'sEdi.AppendFormat("*{0}*{1}*{2}{3}", .L3.L313, .L3.L314, .L3.L315, sSegTerm)

            'Added By LVV on 2/14/18 for v-8.1 PQ EDI
            'Modified by RHR for v-8.4.0.003 on 06/25/2021 removed the "LB" from L304 when L303 is empty
            If ShowContractedCost Then
                sEdi.AppendFormat("L3*{0}*{1}*{2}*{3}*{4}*{5}*{6}*{7}*{8}*{9}*{10}*{11}*{12}*{13}*{14}{15}", .L3.L301, .L3.L302, .L3.L303, .L3.L304, .L3.L305, .L3.L306, .L3.L307, .L3.L308, .L3.L309, .L3.L310, .L3.L311, .L3.L312, .L3.L313, .L3.L314, .L3.L315, sSegTerm)
            Else
                sEdi.AppendFormat("L3*{0}*{1}*{2}*{3}*{4}*{5}*{6}*{7}*{8}*{9}*{10}*{11}*{12}*{13}*{14}{15}", .L3.L301, .L3.L302, "", "", "", "", "", "", .L3.L309, .L3.L310, .L3.L311, .L3.L312, .L3.L313, .L3.L314, .L3.L315, sSegTerm)
            End If

            sEdi.AppendFormat("SE*{0}*{1}{2}", .SE.SE01, .SE.SE02, sSegTerm)
        End With
        Return sEdi.ToString
    End Function
    ''' <summary>
    ''' Get the clsEDITruckLoad object data using FMData.spGetEDI204TruckLoadDataDataTable table data
    ''' </summary>
    ''' <param name="intRow"></param>
    ''' <param name="oTable"></param>
    ''' <param name="oTMS"></param>
    ''' <param name="intSequence"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR by for v-8.5.3.003 on 07/13/2022 fix bug for skipping orders on 204 in for  loop
    ''' Deprecated use the LTS version
    ''' </remarks>
    Protected Function GetConsolidatedTruckLoadObject(ByRef intRow As Integer, ByRef oTable As FMData.spGetEDI204TruckLoadDataDataTable, ByVal oTMS As clsEDITMSData, ByVal intSequence As Integer) As clsEDITruckLoad
        Dim oTruckLoad As New clsEDITruckLoad
        Dim intSegments As Integer = 0
        Try
            With oTruckLoad
                .ST.ST01 = "204"
                .ST.ST02 = intSequence
                intSegments += 1 'increase the segment counter after each segment
                .B2.B201 = "DD"
                .B2.B202 = oTMS.CarrierSCAC.ToUpper
                .B2.B203 = ""
                .B2.B204 = oTMS.BookSHID
                .B2.B205 = "L"
                .B2.B206 = oTMS.CompEDIMethodOfPayment
                intSegments += 1 'increase the segment counter after each segment
                If oTMS.BookTransactionPurpose = "02" Then oTMS.BookTransactionPurpose = "00"
                .B2A.B2A01 = oTMS.BookTransactionPurpose
                .B2A.B2A02 = "LT" 'Use default of Load Tender - Truckload
                intSegments += 1 'increase the segment counter after each segment
                .L11.L1101 = oTMS.BookConsPrefix
                .L11.L1102 = "BM"
                .L11.L1103 = "FS"
                intSegments += 1 'increase the segment counter after each segment
                'We have no Special Instructions in the header
                .PLD.PLD01 = Left(oTMS.BookTotalPL.ToString, 3)
                intSegments += 1 'increase the segment counter after each segment
                'Add the Loop 100 data
                .Loop100 = New List(Of clsEDI204Loop100) From {New clsEDI204Loop100(oTMS.BillToCompName, oTMS.BillToCompNumber, oTMS.BillToCompAddress1, oTMS.BillToCompAddress2, oTMS.BillToCompCity, oTMS.BillToCompState, oTMS.BillToCompZip, oTMS.BillToCompCountry)}
                intSegments += 3 'We add 3 because the 100 loop has 3 elements
                'Add a 200 Loop for each unique equipment requirements
                Dim lComs As New List(Of String)
                Dim intComCt As Integer = 0
                Dim dblTotalWgt As Double = oTMS.BookTotalWgt
                Dim intTotalCube As Integer = oTMS.BookTotalCube
                Dim intTotalCases As Integer = oTMS.BookTotalCases
                Dim dblTotalPallets As Integer = oTMS.BookTotalPL
                Dim decTotalCost As Decimal = oTMS.BookRevTotalCost
                Dim intStopNbr As Integer = 1
                Dim orig300Dict As New Dictionary(Of String, clsEDI204Loop300)
                Dim dest300Dict As New Dictionary(Of String, clsEDI204Loop300)
                Dim loopList As New List(Of clsEDI204Loop300)
                'get all other stops on the truck by CNSNumber
                Dim oShipRows = oTable.AsQueryable.Where(Function(x) If(x.BookTransactionPurpose = "02", "00", x.BookTransactionPurpose).Trim = oTMS.BookTransactionPurpose.Trim And x.BookSHID = oTMS.BookSHID).ToList()
                If (oShipRows Is Nothing OrElse oShipRows.Count < 1) Then
                    'there is an issue with the CNS number so just use the current row stored in oTMS
                    intComCt += 1
                    If .Loop200 Is Nothing Then .Loop200 = New List(Of clsEDI204Loop200)
                    .Loop200.Add(New clsEDI204Loop200(intComCt, oTMS.CommCodeDescription, getEquipCommodityCode(oTMS.BookLoadCom)))
                    intSegments += 1 'increase the segment counter after each segment

                    'Modified by RHR 3/21/2012 used to check for duplicate salesorder numbers 
                    'and to be sure updates are being transmitted properly when order are canceled
                    'we also check the CNS number to be sure it is not empty. 
                    'NOTE: The below line was commented out because we now use the BookSHID
                    'If oTMS.BookConsPrefix.Trim.Length > 1 Then
                    'the code below is checking for errors in the data. If we have duplicate records with
                    'the same sales order we only need to send one update in the 204
                    'Note: Rob will look at this to determine which record to add to the 300 loop
                    '@TODO For now, just add the current oTMS record to the 300 loop
                    ReDim .Loop300(2)
                    .Loop300(0) = Build204300LoopByOrig(oTruckLoad, oTMS, intSegments, intStopNbr, "CL")
                    .Loop300(1) = Build204300LoopByDest(oTruckLoad, oTMS, intSegments, intStopNbr, "CU")
                    'Do
                    '    'Check if the next record has the same CNS number
                    '    If intRow < oTable.Rows.Count - 1 Then
                    '        Dim oRow As FMData.spGetEDI204TruckLoadDataRow = oTable.Rows(intRow + 1)
                    '        Dim strNextCNS As String = cleanEDI(nz(oRow, "BookConsPrefix", ""))
                    '        Dim strNextTransactonPurpose = cleanEDI(nz(oRow, "BookTransactionPurpose", "00"))
                    '        If strNextTransactonPurpose = "02" Then strNextTransactonPurpose = "00"
                    '        If oTMS.BookTransactionPurpose = strNextTransactonPurpose _
                    '            And strNextCNS = oTMS.BookConsPrefix _
                    '            And (cleanEDI(nz(oRow, "BookCarrOrderNumber", "")) & "-" & cleanEDI(nz(oRow, "BookOrderSequence", ""))) <> (oTMS.BookCarrOrderNumber & "-" & oTMS.BookOrderSequence.ToString) Then
                    '            Dim oNextOrder As New clsEDITMSData(oRow)
                    '            oStops.Add(New clsEDITMSData(oRow))
                    '            intRow += 1
                    '        Else
                    '            Exit Do
                    '        End If
                    '    End If
                    'Loop While intRow < oTable.Rows.Count - 1
                    'End If
                Else
                    'Modified by RHR by for v-8.5.3.003 on 07/13/2022
                    'If we have multiple orders on the same shid we skip the next x number of orders 
                    'by increasing intRow 1 for each additional row
                    If oShipRows.Count > 1 Then
                        Dim iRowsToSkip As Integer = oShipRows.Count - 1
                        intRow += iRowsToSkip
                    End If

                    For Each t In oShipRows
                        Dim r As New clsEDITMSData(t)
                        If Not lComs.Contains(r.BookLoadCom) Then
                            lComs.Add(r.BookLoadCom)
                            intComCt += 1
                            If .Loop200 Is Nothing Then .Loop200 = New List(Of clsEDI204Loop200)
                            .Loop200.Add(New clsEDI204Loop200(intComCt, r.CommCodeDescription, getEquipCommodityCode(r.BookLoadCom)))
                            intSegments += 1 'increase the segment counter after each segment
                        End If
                        'Check code for adding row data to an existing 300 loop, it should not increase intSegments but the code
                        'used to create a new 300 loop should increase 

                        'first do the pickups
                        If orig300Dict.ContainsKey(r.BookOrigAddress1) Then
                            If r.EDICombineOrdersForStops <> 0 Then
                                'add to existing 300 loop
                                processOtherPickups(r, orig300Dict(r.BookOrigAddress1), intSegments)
                            Else
                                'create new 300 loop
                                Dim loop300 = Build204300LoopByOrig(oTruckLoad, r, intSegments, intStopNbr, "PL")
                                orig300Dict.Add(r.BookOrigAddress1 & intStopNbr, loop300)
                                'Each time we add a 300 loop we must increase the intStopNbr
                                intStopNbr += 1
                            End If
                        Else
                            'create a new 300 loop
                            Dim loop300 = Build204300LoopByOrig(oTruckLoad, r, intSegments, intStopNbr, "PL")
                            orig300Dict.Add(r.BookOrigAddress1, loop300)
                            'Each time we add a 300 loop we must increase the intStopNbr
                            intStopNbr += 1
                        End If

                    Next
                    For Each t In oShipRows
                        Dim r As New clsEDITMSData(t)
                        'next do the dropoffs
                        'intStopNbr = 1
                        If dest300Dict.ContainsKey(r.BookDestAddress1) Then
                            If r.EDICombineOrdersForStops <> 0 Then
                                'add to existing 300 loop
                                processOtherDropOffs(r, dest300Dict(r.BookDestAddress1), intSegments)
                            Else
                                'create new 300 loop
                                Dim loop300 = Build204300LoopByDest(oTruckLoad, r, intSegments, intStopNbr, "PU")
                                dest300Dict.Add(r.BookDestAddress1 & intStopNbr, loop300)
                                'Each time we add a 300 loop we must increase the intStopNbr
                                intStopNbr += 1
                            End If
                        Else
                            'create a new 300 loop
                            Dim loop300 = Build204300LoopByDest(oTruckLoad, r, intSegments, intStopNbr, "PU")
                            dest300Dict.Add(r.BookDestAddress1, loop300)
                            'Each time we add a 300 loop we must increase the intStopNbr
                            intStopNbr += 1
                        End If
                    Next
                    'After we add all 300 loops to the dictionaries we will locate the highest intStopNbr in each
                    'dictionary and update the PL to CL or PU to CU

                    Dim origLastStop = (orig300Dict.OrderByDescending(Function(kvp) kvp.Value.S5.S501) _
                    .Select(Function(kvp) kvp.Key)).FirstOrDefault()
                    Dim lo = orig300Dict(origLastStop)
                    lo.S5.S502 = "CL"

                    Dim destLastStop = (dest300Dict.OrderByDescending(Function(kvp) kvp.Value.S5.S501) _
                    .Select(Function(kvp) kvp.Key)).FirstOrDefault()
                    Dim ld = dest300Dict(destLastStop)
                    ld.S5.S502 = "CU"

                    'once we have updated the dictionaries we can add them to the 204 300 loop array                  
                    loopList.AddRange(orig300Dict.Values)
                    loopList.AddRange(dest300Dict.Values)
                    .Loop300 = loopList.ToArray()

                    'update the totals
                    dblTotalWgt = oShipRows.Sum(Function(x) x.BookTotalWgt)
                    intTotalCube = oShipRows.Sum(Function(x) x.BookTotalCube)
                    intTotalCases = oShipRows.Sum(Function(x) x.BookTotalCases)
                    dblTotalPallets = oShipRows.Sum(Function(x) x.BookTotalPL)
                    decTotalCost = oShipRows.Sum(Function(x) x.BookRevTotalCost)
                End If
                'This finishes the 204 document
                .L3.L301 = Left(CInt(dblTotalWgt).ToString, 10)
                .L3.L302 = "G" 'Gross Weight
                .L3.L305 = NDT.formatDecimalToEDICurrency(NDT.nzDecimal(decTotalCost, 0), 12)
                .L3.L309 = Left(intTotalCube.ToString, 8)
                .L3.L310 = "E" 'Cubic Feet
                .L3.L311 = Left(intTotalCases.ToString, 7)
                intSegments += 1 'increase the segment counter after each segment
                'Update the pallet count
                .PLD.PLD01 = Left(CInt(dblTotalPallets).ToString, 3)
                .SE.SE01 = intSegments + 1
                .SE.SE02 = intSequence
            End With
        Catch ex As Exception
            'we currently have not added any special error conditions so just throw the error back to the caller
            Throw
        End Try
        Return oTruckLoad
    End Function

    ''' <summary>
    ''' Get the clsEDITruckLoad object data using  LTS.spGetEDI204TruckLoadDataResult data
    ''' </summary>
    ''' <param name="intRow"></param>
    ''' <param name="oEDI204Data"></param>
    ''' <param name="oTMS"></param>
    ''' <param name="intSequence"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR by for v-8.5.3.003 on 07/13/2022 fix bug for skipping orders on 204 in for  loop
    ''' </remarks>
    Protected Function GetConsolidatedTruckLoadObject(ByRef intRow As Integer, ByRef oEDI204Data As LTS.spGetEDI204TruckLoadDataResult(), ByVal oTMS As clsEDITMSData, ByVal intSequence As Integer) As clsEDITruckLoad
        Dim oTruckLoad As New clsEDITruckLoad
        Dim intSegments As Integer = 0
        Try
            With oTruckLoad
                .ST.ST01 = "204"
                .ST.ST02 = intSequence
                intSegments += 1 'increase the segment counter after each segment
                .B2.B201 = "DD"
                .B2.B202 = oTMS.CarrierSCAC.ToUpper
                .B2.B203 = ""
                .B2.B204 = oTMS.BookSHID
                .B2.B205 = "L"
                .B2.B206 = oTMS.CompEDIMethodOfPayment
                intSegments += 1 'increase the segment counter after each segment
                If oTMS.BookTransactionPurpose = "02" Then oTMS.BookTransactionPurpose = "00"
                .B2A.B2A01 = oTMS.BookTransactionPurpose
                .B2A.B2A02 = "LT" 'Use default of Load Tender - Truckload
                intSegments += 1 'increase the segment counter after each segment
                .L11.L1101 = oTMS.BookConsPrefix
                .L11.L1102 = "BM"
                .L11.L1103 = "FS"
                intSegments += 1 'increase the segment counter after each segment
                'We have no Special Instructions in the header
                .PLD.PLD01 = Left(oTMS.BookTotalPL.ToString, 3)
                intSegments += 1 'increase the segment counter after each segment
                'Add the Loop 100 data
                .Loop100 = New List(Of clsEDI204Loop100) From {New clsEDI204Loop100(oTMS.BillToCompName, oTMS.BillToCompNumber, oTMS.BillToCompAddress1, oTMS.BillToCompAddress2, oTMS.BillToCompCity, oTMS.BillToCompState, oTMS.BillToCompZip, oTMS.BillToCompCountry)}
                intSegments += 3 'We add 3 because the 100 loop has 3 elements
                'Add a 200 Loop for each unique equipment requirements
                Dim lComs As New List(Of String)
                Dim intComCt As Integer = 0
                Dim dblTotalWgt As Double = oTMS.BookTotalWgt
                Dim intTotalCube As Integer = oTMS.BookTotalCube
                Dim intTotalCases As Integer = oTMS.BookTotalCases
                Dim dblTotalPallets As Integer = oTMS.BookTotalPL
                Dim decTotalCost As Decimal = oTMS.BookRevTotalCost
                Dim intStopNbr As Integer = 1
                Dim orig300Dict As New Dictionary(Of String, clsEDI204Loop300)
                Dim dest300Dict As New Dictionary(Of String, clsEDI204Loop300)
                Dim loopList As New List(Of clsEDI204Loop300)
                'get all other stops on the truck by CNSNumber
                Dim oShipRows = oEDI204Data.AsQueryable.Where(Function(x) If(x.BookTransactionPurpose = "02", "00", x.BookTransactionPurpose).Trim = oTMS.BookTransactionPurpose.Trim And x.BookSHID = oTMS.BookSHID).ToList()
                If (oShipRows Is Nothing OrElse oShipRows.Count < 1) Then
                    'there is an issue with the CNS number so just use the current row stored in oTMS
                    intComCt += 1
                    If .Loop200 Is Nothing Then .Loop200 = New List(Of clsEDI204Loop200)
                    .Loop200.Add(New clsEDI204Loop200(intComCt, oTMS.CommCodeDescription, getEquipCommodityCode(oTMS.BookLoadCom)))
                    intSegments += 1 'increase the segment counter after each segment

                    'Modified by RHR 3/21/2012 used to check for duplicate salesorder numbers 
                    'and to be sure updates are being transmitted properly when order are canceled
                    'we also check the CNS number to be sure it is not empty. 
                    'NOTE: The below line was commented out because we now use the BookSHID
                    'If oTMS.BookConsPrefix.Trim.Length > 1 Then
                    'the code below is checking for errors in the data. If we have duplicate records with
                    'the same sales order we only need to send one update in the 204
                    'Note: Rob will look at this to determine which record to add to the 300 loop
                    '@TODO For now, just add the current oTMS record to the 300 loop
                    ReDim .Loop300(2)
                    .Loop300(0) = Build204300LoopByOrig(oTruckLoad, oTMS, intSegments, intStopNbr, "CL")
                    .Loop300(1) = Build204300LoopByDest(oTruckLoad, oTMS, intSegments, intStopNbr, "CU")
                    'Do
                    '    'Check if the next record has the same CNS number
                    '    If intRow < oTable.Rows.Count - 1 Then
                    '        Dim oRow As FMData.spGetEDI204TruckLoadDataRow = oTable.Rows(intRow + 1)
                    '        Dim strNextCNS As String = cleanEDI(nz(oRow, "BookConsPrefix", ""))
                    '        Dim strNextTransactonPurpose = cleanEDI(nz(oRow, "BookTransactionPurpose", "00"))
                    '        If strNextTransactonPurpose = "02" Then strNextTransactonPurpose = "00"
                    '        If oTMS.BookTransactionPurpose = strNextTransactonPurpose _
                    '            And strNextCNS = oTMS.BookConsPrefix _
                    '            And (cleanEDI(nz(oRow, "BookCarrOrderNumber", "")) & "-" & cleanEDI(nz(oRow, "BookOrderSequence", ""))) <> (oTMS.BookCarrOrderNumber & "-" & oTMS.BookOrderSequence.ToString) Then
                    '            Dim oNextOrder As New clsEDITMSData(oRow)
                    '            oStops.Add(New clsEDITMSData(oRow))
                    '            intRow += 1
                    '        Else
                    '            Exit Do
                    '        End If
                    '    End If
                    'Loop While intRow < oTable.Rows.Count - 1
                    'End If
                Else
                    'Modified by RHR by for v-8.5.3.003 on 07/13/2022
                    'If we have multiple orders on the same shid we skip the next x number of orders 
                    'by increasing intRow 1 for each additional row
                    If oShipRows.Count > 1 Then
                        Dim iRowsToSkip As Integer = oShipRows.Count - 1
                        intRow += iRowsToSkip
                    End If
                    For Each t In oShipRows
                        Dim r As New clsEDITMSData(t)
                        If Not lComs.Contains(r.BookLoadCom) Then
                            lComs.Add(r.BookLoadCom)
                            intComCt += 1
                            If .Loop200 Is Nothing Then .Loop200 = New List(Of clsEDI204Loop200)
                            .Loop200.Add(New clsEDI204Loop200(intComCt, r.CommCodeDescription, getEquipCommodityCode(r.BookLoadCom)))
                            intSegments += 1 'increase the segment counter after each segment
                        End If
                        'Check code for adding row data to an existing 300 loop, it should not increase intSegments but the code
                        'used to create a new 300 loop should increase 

                        'first do the pickups
                        If orig300Dict.ContainsKey(r.BookOrigAddress1) Then
                            If r.EDICombineOrdersForStops <> 0 Then
                                'add to existing 300 loop
                                processOtherPickups(r, orig300Dict(r.BookOrigAddress1), intSegments)
                            Else
                                'create new 300 loop
                                Dim loop300 = Build204300LoopByOrig(oTruckLoad, r, intSegments, intStopNbr, "PL")
                                orig300Dict.Add(r.BookOrigAddress1 & intStopNbr, loop300)
                                'Each time we add a 300 loop we must increase the intStopNbr
                                intStopNbr += 1
                            End If
                        Else
                            'create a new 300 loop
                            Dim loop300 = Build204300LoopByOrig(oTruckLoad, r, intSegments, intStopNbr, "PL")
                            orig300Dict.Add(r.BookOrigAddress1, loop300)
                            'Each time we add a 300 loop we must increase the intStopNbr
                            intStopNbr += 1
                        End If

                    Next
                    For Each t In oShipRows
                        Dim r As New clsEDITMSData(t)
                        'next do the dropoffs
                        'intStopNbr = 1
                        If dest300Dict.ContainsKey(r.BookDestAddress1) Then
                            If r.EDICombineOrdersForStops <> 0 Then
                                'add to existing 300 loop
                                processOtherDropOffs(r, dest300Dict(r.BookDestAddress1), intSegments)
                            Else
                                'create new 300 loop
                                Dim loop300 = Build204300LoopByDest(oTruckLoad, r, intSegments, intStopNbr, "PU")
                                dest300Dict.Add(r.BookDestAddress1 & intStopNbr, loop300)
                                'Each time we add a 300 loop we must increase the intStopNbr
                                intStopNbr += 1
                            End If
                        Else
                            'create a new 300 loop
                            Dim loop300 = Build204300LoopByDest(oTruckLoad, r, intSegments, intStopNbr, "PU")
                            dest300Dict.Add(r.BookDestAddress1, loop300)
                            'Each time we add a 300 loop we must increase the intStopNbr
                            intStopNbr += 1
                        End If
                    Next
                    'After we add all 300 loops to the dictionaries we will locate the highest intStopNbr in each
                    'dictionary and update the PL to CL or PU to CU

                    Dim origLastStop = (orig300Dict.OrderByDescending(Function(kvp) kvp.Value.S5.S501) _
                    .Select(Function(kvp) kvp.Key)).FirstOrDefault()
                    Dim lo = orig300Dict(origLastStop)
                    lo.S5.S502 = "CL"

                    Dim destLastStop = (dest300Dict.OrderByDescending(Function(kvp) kvp.Value.S5.S501) _
                    .Select(Function(kvp) kvp.Key)).FirstOrDefault()
                    Dim ld = dest300Dict(destLastStop)
                    ld.S5.S502 = "CU"

                    'once we have updated the dictionaries we can add them to the 204 300 loop array                  
                    loopList.AddRange(orig300Dict.Values)
                    loopList.AddRange(dest300Dict.Values)
                    .Loop300 = loopList.ToArray()

                    'update the totals
                    dblTotalWgt = oShipRows.Sum(Function(x) x.BookTotalWgt)
                    intTotalCube = oShipRows.Sum(Function(x) x.BookTotalCube)
                    intTotalCases = oShipRows.Sum(Function(x) x.BookTotalCases)
                    dblTotalPallets = oShipRows.Sum(Function(x) x.BookTotalPL)
                    decTotalCost = oShipRows.Sum(Function(x) x.BookRevTotalCost)
                End If
                'This finishes the 204 document
                .L3.L301 = Left(CInt(dblTotalWgt).ToString, 10)
                .L3.L302 = "G" 'Gross Weight
                .L3.L305 = NDT.formatDecimalToEDICurrency(NDT.nzDecimal(decTotalCost, 0), 12)
                .L3.L309 = Left(intTotalCube.ToString, 8)
                .L3.L310 = "E" 'Cubic Feet
                .L3.L311 = Left(intTotalCases.ToString, 7)
                intSegments += 1 'increase the segment counter after each segment
                'Update the pallet count
                .PLD.PLD01 = Left(CInt(dblTotalPallets).ToString, 3)
                .SE.SE01 = intSegments + 1
                .SE.SE02 = intSequence
            End With
        Catch ex As Exception
            'we currently have not added any special error conditions so just throw the error back to the caller
            Throw
        End Try
        Return oTruckLoad
    End Function

    ''' <summary>
    ''' Get the clsEDITruckLoad object data using   List(Of EDI204Result) data
    ''' </summary>
    ''' <param name="intRow"></param>
    ''' <param name="oTable"></param>
    ''' <param name="oTMS"></param>
    ''' <param name="intSequence"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR by for v-8.5.3.003 on 07/13/2022 fix bug for skipping orders on 204 in for  loop
    ''' Test version use the LTS version for production
    ''' </remarks>
    Protected Function GetConsolidatedTruckLoadObject(ByRef intRow As Integer, ByRef oTable As List(Of EDI204Result), ByVal oTMS As clsEDITMSData, ByVal intSequence As Integer) As clsEDITruckLoad
        Dim oTruckLoad As New clsEDITruckLoad
        Dim intSegments As Integer = 0
        Try
            With oTruckLoad
                .ST.ST01 = "204"
                .ST.ST02 = intSequence
                intSegments += 1 'increase the segment counter after each segment
                .B2.B201 = "DD"
                .B2.B202 = oTMS.CarrierSCAC.ToUpper
                .B2.B203 = ""
                .B2.B204 = oTMS.BookSHID
                .B2.B205 = "L"
                .B2.B206 = oTMS.CompEDIMethodOfPayment
                intSegments += 1 'increase the segment counter after each segment
                If oTMS.BookTransactionPurpose = "02" Then oTMS.BookTransactionPurpose = "00"
                .B2A.B2A01 = oTMS.BookTransactionPurpose
                .B2A.B2A02 = "LT" 'Use default of Load Tender - Truckload
                intSegments += 1 'increase the segment counter after each segment
                .L11.L1101 = oTMS.BookConsPrefix
                .L11.L1102 = "BM"
                .L11.L1103 = "FS"
                intSegments += 1 'increase the segment counter after each segment
                'We have no Special Instructions in the header
                .PLD.PLD01 = Left(oTMS.BookTotalPL.ToString, 3)
                intSegments += 1 'increase the segment counter after each segment
                'Add the Loop 100 data
                .Loop100 = New List(Of clsEDI204Loop100) From {New clsEDI204Loop100(oTMS.BillToCompName, oTMS.BillToCompNumber, oTMS.BillToCompAddress1, oTMS.BillToCompAddress2, oTMS.BillToCompCity, oTMS.BillToCompState, oTMS.BillToCompZip, oTMS.BillToCompCountry)}
                intSegments += 3 'We add 3 because the 100 loop has 3 elements
                'Add a 200 Loop for each unique equipment requirements
                Dim lComs As New List(Of String)
                Dim intComCt As Integer = 0
                Dim dblTotalWgt As Double = oTMS.BookTotalWgt
                Dim intTotalCube As Integer = oTMS.BookTotalCube
                Dim intTotalCases As Integer = oTMS.BookTotalCases
                Dim dblTotalPallets As Integer = oTMS.BookTotalPL
                Dim decTotalCost As Decimal = oTMS.BookRevTotalCost
                Dim intStopNbr As Integer = 1
                Dim orig300Dict As New Dictionary(Of String, clsEDI204Loop300)
                Dim dest300Dict As New Dictionary(Of String, clsEDI204Loop300)
                Dim loopList As New List(Of clsEDI204Loop300)
                'get all other stops on the truck by CNSNumber
                Dim oShipRows = oTable.Where(Function(x) If(x.BookTransactionPurpose = "02", "00", x.BookTransactionPurpose).Trim = oTMS.BookTransactionPurpose.Trim And x.BookSHID = oTMS.BookSHID).ToList()
                If (oShipRows Is Nothing OrElse oShipRows.Count < 1) Then
                    'there is an issue with the CNS number so just use the current row stored in oTMS
                    intComCt += 1
                    If .Loop200 Is Nothing Then .Loop200 = New List(Of clsEDI204Loop200)
                    .Loop200.Add(New clsEDI204Loop200(intComCt, oTMS.CommCodeDescription, getEquipCommodityCode(oTMS.BookLoadCom)))
                    intSegments += 1 'increase the segment counter after each segment

                    'Modified by RHR 3/21/2012 used to check for duplicate salesorder numbers 
                    'and to be sure updates are being transmitted properly when order are canceled
                    'we also check the CNS number to be sure it is not empty. 
                    'NOTE: The below line was commented out because we now use the BookSHID
                    'If oTMS.BookConsPrefix.Trim.Length > 1 Then
                    'the code below is checking for errors in the data. If we have duplicate records with
                    'the same sales order we only need to send one update in the 204
                    'Note: Rob will look at this to determine which record to add to the 300 loop
                    '@TODO For now, just add the current oTMS record to the 300 loop
                    ReDim .Loop300(2)
                    .Loop300(0) = Build204300LoopByOrig(oTruckLoad, oTMS, intSegments, intStopNbr, "CL")
                    .Loop300(1) = Build204300LoopByDest(oTruckLoad, oTMS, intSegments, intStopNbr, "CU")
                    'Do
                    '    'Check if the next record has the same CNS number
                    '    If intRow < oTable.Rows.Count - 1 Then
                    '        Dim oRow As FMData.spGetEDI204TruckLoadDataRow = oTable.Rows(intRow + 1)
                    '        Dim strNextCNS As String = cleanEDI(nz(oRow, "BookConsPrefix", ""))
                    '        Dim strNextTransactonPurpose = cleanEDI(nz(oRow, "BookTransactionPurpose", "00"))
                    '        If strNextTransactonPurpose = "02" Then strNextTransactonPurpose = "00"
                    '        If oTMS.BookTransactionPurpose = strNextTransactonPurpose _
                    '            And strNextCNS = oTMS.BookConsPrefix _
                    '            And (cleanEDI(nz(oRow, "BookCarrOrderNumber", "")) & "-" & cleanEDI(nz(oRow, "BookOrderSequence", ""))) <> (oTMS.BookCarrOrderNumber & "-" & oTMS.BookOrderSequence.ToString) Then
                    '            Dim oNextOrder As New clsEDITMSData(oRow)
                    '            oStops.Add(New clsEDITMSData(oRow))
                    '            intRow += 1
                    '        Else
                    '            Exit Do
                    '        End If
                    '    End If
                    'Loop While intRow < oTable.Rows.Count - 1
                    'End If
                Else
                    'Modified by RHR by for v-8.5.3.003 on 07/13/2022
                    'If we have multiple orders on the same shid we skip the next x number of orders 
                    'by increasing intRow 1 for each additional row
                    If oShipRows.Count > 1 Then
                        Dim iRowsToSkip As Integer = oShipRows.Count - 1
                        intRow += iRowsToSkip
                    End If
                    For Each t In oShipRows
                        Dim r As New clsEDITMSData(t)
                        If Not lComs.Contains(r.BookLoadCom) Then
                            lComs.Add(r.BookLoadCom)
                            intComCt += 1
                            If .Loop200 Is Nothing Then .Loop200 = New List(Of clsEDI204Loop200)
                            .Loop200.Add(New clsEDI204Loop200(intComCt, r.CommCodeDescription, getEquipCommodityCode(r.BookLoadCom)))
                            intSegments += 1 'increase the segment counter after each segment
                        End If
                        'Check code for adding row data to an existing 300 loop, it should not increase intSegments but the code
                        'used to create a new 300 loop should increase 

                        'first do the pickups
                        If orig300Dict.ContainsKey(r.BookOrigAddress1) Then
                            If r.EDICombineOrdersForStops <> 0 Then
                                'add to existing 300 loop
                                processOtherPickups(r, orig300Dict(r.BookOrigAddress1), intSegments)
                            Else
                                'create new 300 loop
                                Dim loop300 = Build204300LoopByOrig(oTruckLoad, r, intSegments, intStopNbr, "PL")
                                orig300Dict.Add(r.BookOrigAddress1 & intStopNbr, loop300)
                                'Each time we add a 300 loop we must increase the intStopNbr
                                intStopNbr += 1
                            End If
                        Else
                            'create a new 300 loop
                            Dim loop300 = Build204300LoopByOrig(oTruckLoad, r, intSegments, intStopNbr, "PL")
                            orig300Dict.Add(r.BookOrigAddress1, loop300)
                            'Each time we add a 300 loop we must increase the intStopNbr
                            intStopNbr += 1
                        End If

                    Next
                    For Each t In oShipRows
                        Dim r As New clsEDITMSData(t)
                        'next do the dropoffs
                        'intStopNbr = 1
                        If dest300Dict.ContainsKey(r.BookDestAddress1) Then
                            If r.EDICombineOrdersForStops <> 0 Then
                                'add to existing 300 loop
                                processOtherDropOffs(r, dest300Dict(r.BookDestAddress1), intSegments)
                            Else
                                'create new 300 loop
                                Dim loop300 = Build204300LoopByDest(oTruckLoad, r, intSegments, intStopNbr, "PU")
                                dest300Dict.Add(r.BookDestAddress1 & intStopNbr, loop300)
                                'Each time we add a 300 loop we must increase the intStopNbr
                                intStopNbr += 1
                            End If
                        Else
                            'create a new 300 loop
                            Dim loop300 = Build204300LoopByDest(oTruckLoad, r, intSegments, intStopNbr, "PU")
                            dest300Dict.Add(r.BookDestAddress1, loop300)
                            'Each time we add a 300 loop we must increase the intStopNbr
                            intStopNbr += 1
                        End If
                    Next
                    'After we add all 300 loops to the dictionaries we will locate the highest intStopNbr in each
                    'dictionary and update the PL to CL or PU to CU

                    Dim origLastStop = (orig300Dict.OrderByDescending(Function(kvp) kvp.Value.S5.S501) _
                    .Select(Function(kvp) kvp.Key)).FirstOrDefault()
                    Dim lo = orig300Dict(origLastStop)
                    lo.S5.S502 = "CL"

                    Dim destLastStop = (dest300Dict.OrderByDescending(Function(kvp) kvp.Value.S5.S501) _
                    .Select(Function(kvp) kvp.Key)).FirstOrDefault()
                    Dim ld = dest300Dict(destLastStop)
                    ld.S5.S502 = "CU"

                    'once we have updated the dictionaries we can add them to the 204 300 loop array                  
                    loopList.AddRange(orig300Dict.Values)
                    loopList.AddRange(dest300Dict.Values)
                    .Loop300 = loopList.ToArray()

                    'update the totals
                    dblTotalWgt = oShipRows.Sum(Function(x) x.BookTotalWgt)
                    intTotalCube = oShipRows.Sum(Function(x) x.BookTotalCube)
                    intTotalCases = oShipRows.Sum(Function(x) x.BookTotalCases)
                    dblTotalPallets = oShipRows.Sum(Function(x) x.BookTotalPL)
                    decTotalCost = oShipRows.Sum(Function(x) x.BookRevTotalCost)
                End If
                'This finishes the 204 document
                .L3.L301 = Left(CInt(dblTotalWgt).ToString, 10)
                .L3.L302 = "G" 'Gross Weight
                .L3.L305 = NDT.formatDecimalToEDICurrency(NDT.nzDecimal(decTotalCost, 0), 12)
                .L3.L309 = Left(intTotalCube.ToString, 8)
                .L3.L310 = "E" 'Cubic Feet
                .L3.L311 = Left(intTotalCases.ToString, 7)
                intSegments += 1 'increase the segment counter after each segment
                'Update the pallet count
                .PLD.PLD01 = Left(CInt(dblTotalPallets).ToString, 3)
                .SE.SE01 = intSegments + 1
                .SE.SE02 = intSequence
            End With
        Catch ex As Exception
            'we currently have not added any special error conditions so just throw the error back to the caller
            Throw
        End Try
        Return oTruckLoad
    End Function

    ''' <summary>
    ''' Build Orig 300 Loop
    ''' </summary>
    ''' <param name="oTruckLoad"></param>
    ''' <param name="oTMS"></param>
    ''' <param name="intSegments"></param>
    ''' <param name="intStopNbr"></param>
    ''' <param name="strStopReason"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.3.007 on 03/23/2023 added BookOrigPhone to G61 segment
    ''' Modified by RHR for v-8.5.3.007 on 04/03/2023 set  .S5.S501 = 1  for pickups
    ''' TODO: we need to add more mapping and options to S5.S501 
    ''' </remarks>
    Protected Function Build204300LoopByOrig(ByRef oTruckLoad As clsEDITruckLoad,
                                             ByRef oTMS As clsEDITMSData,
                                             ByRef intSegments As Integer,
                                             ByVal intStopNbr As Integer,
                                             Optional ByVal strStopReason As String = "CL") As clsEDI204Loop300
        Dim oL300ST As New clsEDI204Loop300
        With oL300ST
            .S5.S501 = 1 ' intStopNbr Modified by RHR for v-8.5.3.007 on 04/03/2023 set to 1 for pickups
            .S5.S502 = strStopReason
            .S5.S503 = Left(oTMS.BookTotalWgt.ToString, 10) 'we are limited to 10 charcter for the weight
            .S5.S504 = "L" 'we always use pounds under the current configuration.
            .S5.S505 = Left(oTMS.BookTotalCases.ToString, 10) 'we are limited to 10 characters for the cases/qty
            .S5.S506 = "CA" 'we always send cases
            .S5.S507 = Left(oTMS.BookTotalCube.ToString, 8) 'we are limited to 8 characters for the cubes
            .S5.S508 = "E" 'we always use cubic feet
            intSegments += 1 'increase the segment counter after each segment
            Dim oL11ON As New clsEDIL11
            With oL11ON
                .L1101 = Left(oTMS.BookCarrOrderNumber & "-" & oTMS.BookOrderSequence.ToString, 30)
                .L1102 = "ON" 'We use Order Number on pickup
                .L1103 = "FS"
            End With
            If oL11ON.L1101.Trim.Length > 0 Then
                .L11s.Add(oL11ON)
                intSegments += 1 'increase the segment counter after each segment
            End If
            Dim oL11PO As New clsEDIL11
            With oL11PO
                .L1101 = oTMS.BookLoadPONumber 'Customer PO Number
                .L1102 = "PO" 'We use Order Number on pickup
                .L1103 = "FS"
            End With
            If oL11PO.L1101.Trim.Length > 0 Then
                .L11s.Add(oL11PO)
                intSegments += 1 'increase the segment counter after each segment
            End If
            'check if the schedule date is provided
            If String.IsNullOrEmpty(oTMS.BookCarrScheduleDate) OrElse oTMS.BookCarrScheduleDate = "00000000" Then
                'use the requested date
                .G62.G6201 = "10" 'requested Pick up date
                .G62.G6202 = oTMS.BookDateLoad 'note this field must be formated in the tms class like YYYYMMDD
                .G62.G6203 = "Y" 'requested pick up time
                If String.IsNullOrEmpty(oTMS.BookDateLoadTime) OrElse oTMS.BookDateLoadTime = "0000" Then
                    'pick up and delivery times must be sequential if on the same day so we use 1 and 2 by default
                    .G62.G6204 = "0001"
                Else
                    .G62.G6204 = oTMS.BookDateLoadTime 'note this field must be formated in the tms class like HHmm
                End If
                .G62.G6205 = "LT"
                intSegments += 1 'increase the segment counter after each segment
            Else
                'use the schedule date
                .G62.G6201 = "69" 'scheduled Pick up date
                .G62.G6202 = oTMS.BookCarrScheduleDate 'note this field must be formated in the tms class like YYYYMMDD
                .G62.G6203 = "U" 'scheduled pick up time
                If String.IsNullOrEmpty(oTMS.BookCarrScheduleTime) OrElse oTMS.BookCarrScheduleTime = "0000" Then
                    'pick up and delivery times must be sequential if on the same day so we use 1 and 2 by default
                    .G62.G6204 = "0001"
                Else
                    .G62.G6204 = oTMS.BookCarrScheduleTime 'note this field must be formated in the tms class like HHmm
                End If
                .G62.G6205 = "LT"
                intSegments += 1 'increase the segment counter after each segment
            End If
            .PLD.PLD01 = Left(oTMS.BookTotalPL.ToString, 3)
            intSegments += 1 'increase the segment counter after each segment
            'we have no NTE comments for the pick up location
            'add the 310 items
            Dim oL310 As New clsEDI204Loop310
            'Set up the Ship From Location
            With oL310
                .N1.N101 = "SF" 'in the get truck load object the first stop is always the SF address                        
                .N1.N102 = oTMS.BookOrigName
                .N1.N103 = Left(oTMS.BookOrigIDENTIFICATIONCODEQUALIFIER, 2)
                .N1.N104 = oTMS.BookOrigCompanyNumber
                intSegments += 1 'increase the segment counter after each segment
                .N3.N301 = Left(oTMS.BookOrigAddress1, 55)
                'provide the additional address info
                If oTMS.BookOrigAddress1.Length > 55 Then
                    .N3.N302 = Left(oTMS.BookOrigAddress1.Substring(54) & " " & oTMS.BookOrigAddress2, 55)
                Else
                    .N3.N302 = Left(oTMS.BookOrigAddress2, 55)
                End If
                intSegments += 1 'increase the segment counter after each segment
                .N4.N401 = oTMS.BookOrigCity
                .N4.N402 = Left(oTMS.BookOrigState, 2)
                .N4.N403 = oTMS.BookOrigZip
                .N4.N404 = Left(oTMS.BookOrigCountry, 3)
                intSegments += 1 'increase the segment counter after each segment
                ' Modified by RHR for v-8.5.3.007 on 03/23/2023 added BookOrigPhone to G61 segment
                If (Not String.IsNullOrWhiteSpace(oTMS.BookOrigPhone)) Then
                    .G61.G6103 = "TE"
                    .G61.G6104 = oTMS.BookOrigPhone
                    intSegments += 1 'increase the segment counter after each segment
                End If
            End With
            ReDim .Loop310(0)
            .Loop310(0) = oL310
            add320Items(oTMS.BookControl, .Loop320, intSegments)
        End With
        Return oL300ST
    End Function

    ''' <summary>
    ''' Builds a 300 loop for the destination location 
    ''' </summary>
    ''' <param name="oTruckLoad"></param>
    ''' <param name="oTMS"></param>
    ''' <param name="intSegments"></param>
    ''' <param name="intStopNbr"></param>
    ''' <param name="strStopReason"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR on 05/30/2018 for v-6.0.4.4-m
    ''' Modified by RHR on 08/08/2018 for v-6.0.4.4-m
    '''   Added support for multiple G62 segments
    '''  Modified by RHR for v-8.5.3.007 on 03/23/2023 added BookDestPhone to G61 segment
    '''  Modified by RHR for v-8.5.3.007 on 04/03/2023 set .S5.S501 to BookStopNo for delivery
    '''  TODO: we need to add more mapping and options to S5.S501 
    ''' </remarks>
    Protected Function Build204300LoopByDest(ByRef oTruckLoad As clsEDITruckLoad,
                                             ByRef oTMS As clsEDITMSData,
                                             ByRef intSegments As Integer,
                                             ByVal intStopNbr As Integer,
                                             Optional ByVal strStopReason As String = "CU") As clsEDI204Loop300
        Dim oL300ST As New clsEDI204Loop300
        With oL300ST
            .S5.S501 = oTMS.BookStopNo '  Modified by RHR for v-8.5.3.007 on 04/03/2023 set to BookStopNo for delivery intStopNbr
            .S5.S502 = strStopReason
            .S5.S503 = Left(oTMS.BookTotalWgt.ToString, 10) 'we are limited to 10 charcter for the weight
            .S5.S504 = "L" 'we always use pounds under the current configuration.
            .S5.S505 = Left(oTMS.BookTotalCases.ToString, 10) 'we are limited to 10 characters for the cases/qty
            .S5.S506 = "CA" 'we always send cases
            .S5.S507 = Left(oTMS.BookTotalCube.ToString, 8) 'we are limited to 8 characters for the cubes
            .S5.S508 = "E" 'we always use cubic feet
            intSegments += 1 'increase the segment counter after each segment
            Dim oL11ON As New clsEDIL11
            With oL11ON
                .L1101 = Left(oTMS.BookCarrOrderNumber & "-" & oTMS.BookOrderSequence.ToString, 30)
                .L1102 = "ON" 'We use Order Number on pickup
                .L1103 = "FS"
            End With
            If oL11ON.L1101.Trim.Length > 0 Then
                .L11s.Add(oL11ON)
                intSegments += 1 'increase the segment counter after each segment
            End If
            Dim oL11PO As New clsEDIL11
            With oL11PO
                .L1101 = oTMS.BookLoadPONumber 'Customer PO Number
                .L1102 = "PO" 'We use Order Number on pickup
                .L1103 = "FS"
            End With
            If oL11PO.L1101.Trim.Length > 0 Then
                .L11s.Add(oL11PO)
                intSegments += 1 'increase the segment counter after each segment
            End If
            'Added by RHR on 05/30/2018 for v-6.0.4.4-m
            If Not String.IsNullOrWhiteSpace(oTMS.BookWhseAuthorizationNo) Then
                .L11s.Add(New clsEDIL11() With {.L1101 = Left(oTMS.BookWhseAuthorizationNo, 30), .L1102 = "4C", .L1103 = "Appointment Authorization No"})
                intSegments += 1 'increase the segment counter after each segment
            End If
            'check if the schedule date is provided
            If String.IsNullOrEmpty(oTMS.BookCarrApptDate) OrElse oTMS.BookCarrApptDate = "00000000" Then
                'use the requested date
                .G62.G6201 = "02" 'Requested delivery date
                .G62.G6202 = oTMS.BookDateRequired 'note this field must be formated in the tms class like YYYYMMDD
                .G62.G6203 = "Z" 'Requested delivery time
                If String.IsNullOrEmpty(oTMS.BookDateRequiredTime) OrElse oTMS.BookDateRequiredTime = "0000" Then
                    'pick up and delivery times must be sequential if on the same day so we use 1 and 2 by default
                    .G62.G6204 = "0002"
                Else
                    .G62.G6204 = oTMS.BookDateRequiredTime 'note this field must be formated in the tms class like HHmm
                End If
                .G62.G6205 = "LT"
                intSegments += 1 'increase the segment counter after each segment
            Else
                'use the schedule date
                .G62.G6201 = "70" 'scheduled delivery date
                .G62.G6202 = oTMS.BookCarrApptDate 'note this field must be formated in the tms class like YYYYMMDD
                .G62.G6203 = "X" 'scheduled delivery time
                If String.IsNullOrEmpty(oTMS.BookCarrApptTime) OrElse oTMS.BookCarrApptTime = "0000" Then
                    'pick up and delivery times must be sequential if on the same day so we use 1 and 2 by default
                    .G62.G6204 = "0002"
                Else
                    .G62.G6204 = oTMS.BookCarrApptTime 'note this field must be formated in the tms class like HHmm
                End If
                .G62.G6205 = "LT"
                intSegments += 1 'increase the segment counter after each segment
            End If
            .PLD.PLD01 = Left(oTMS.BookTotalPL.ToString, 3)
            intSegments += 1 'increase the segment counter after each segment
            'NTE comments for the delivery location
            If Not String.IsNullOrEmpty(oTMS.LaneComments) Then
                .NTE.NTE01 = "DEL" 'Delivery instructions
                .NTE.NTE02 = Left(oTMS.LaneComments, 80)
                intSegments += 1 'increase the segment counter after each segment
            End If
            'add the 310 items
            Dim oL310 As New clsEDI204Loop310
            'Set up the Ship From Location
            With oL310
                .N1.N101 = "ST" 'in the get truck load object the first stop is always the SF address                        
                .N1.N102 = oTMS.BookDestName
                .N1.N103 = Left(oTMS.BookDestIDENTIFICATIONCODEQUALIFIER, 2)
                .N1.N104 = oTMS.BookDestCompanyNumber
                intSegments += 1 'increase the segment counter after each segment
                .N3.N301 = Left(oTMS.BookDestAddress1, 55)
                'provide the additional address info
                If oTMS.BookDestAddress1.Length > 55 Then
                    .N3.N302 = Left(oTMS.BookDestAddress1.Substring(54) & " " & oTMS.BookDestAddress2, 55)
                Else
                    .N3.N302 = Left(oTMS.BookDestAddress2, 55)
                End If
                intSegments += 1 'increase the segment counter after each segment
                .N4.N401 = oTMS.BookDestCity
                .N4.N402 = Left(oTMS.BookDestState, 2)
                .N4.N403 = oTMS.BookDestZip
                .N4.N404 = Left(oTMS.BookDestCountry, 3)
                intSegments += 1 'increase the segment counter after each segment
                ' Modified by RHR for v-8.5.3.007 on 03/23/2023 added BookDestPhone to G61 segment
                If (Not String.IsNullOrWhiteSpace(oTMS.BookDestPhone)) Then
                    .G61.G6103 = "TE"
                    .G61.G6104 = oTMS.BookDestPhone
                    intSegments += 1 'increase the segment counter after each segment
                End If
            End With
            ReDim .Loop310(0)
            .Loop310(0) = oL310
            add320Items(oTMS.BookControl, .Loop320, intSegments)
        End With
        Return oL300ST
    End Function



    Protected Function UpdateStatusEx(
            ByVal APControl As Long,
            ByVal ExportDate As Date,
            Optional ByVal Exported As Boolean = True) As Boolean

        Dim blnRet As Boolean = False
        Dim strCriteria As String = ""
        Dim oCon As System.Data.SqlClient.SqlConnection
        Dim intExportRetry As Integer = 0
        Try
            oCon = getNewConnection()
            strCriteria &= "APControl Number = " & APControl.ToString
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim objCom As New SqlCommand
                Try
                    Dim lngErrNumber As Long
                    Dim strRetVal As String = ""
                    With objCom
                        .Connection = oCon
                        .CommandTimeout = Me.CommandTimeOut
                        .Parameters.Add("@APControl", SqlDbType.Int)
                        .Parameters("@APControl").Value = APControl
                        .Parameters.Add("@ModUser", SqlDbType.NVarChar, 25)
                        If Not Me.CreateUser Is Nothing AndAlso Me.CreateUser.Trim.Length > 0 Then
                            .Parameters("@ModUser").Value = Me.CreateUser
                        Else
                            .Parameters("@ModUser").Value = DBNull.Value
                        End If
                        .Parameters.Add("@RetMsg", SqlDbType.NVarChar, 4000)
                        .Parameters("@RetMsg").Direction = ParameterDirection.Output
                        .Parameters.Add("@ErrNumber", SqlDbType.Int)
                        .Parameters("@ErrNumber").Direction = ParameterDirection.Output
                        .CommandText = "spUpdateAPExportStatusEx"
                        .CommandType = CommandType.StoredProcedure
                        intExportRetry = .ExecuteScalar()
                        strRetVal = Trim(.Parameters("@RetMsg").Value.ToString)
                        If IsDBNull(.Parameters("@ErrNumber").Value) Then
                            lngErrNumber = 0
                        Else
                            lngErrNumber = .Parameters("@ErrNumber").Value
                        End If
                    End With

                    If lngErrNumber <> 0 Then
                        If intRetryCt > Me.Retry Then
                            Me.StatusUpdateErrors += 1
                            ITEmailMsg &= "<br /> The update AP status command failed " & intRetryCt & " times for the following data:" _
                                & "<br /> Criteria: " & strCriteria _
                                & "<br /> ExportDate: " & ExportDate.ToString _
                                & "<br /> Exported Flag: " & Exported.ToString _
                                & "<br /> Export Retry Counter: " & intExportRetry.ToString _
                                & "<br /> Error Number: " & lngErrNumber.ToString _
                                & "<br /> Error Message: " & strRetVal _
                                & "<br />" & vbCrLf
                            Exit Do
                        Else
                            Log("NGL.FreightMaster.Integration.clsAPExport.UpdateStatusEx Output Failure. Retry # " & intRetryCt.ToString & " of " & Me.Retry.ToString)
                        End If
                    Else
                        Log("An AP Export record status was updated using the following criteria:  " & strCriteria)
                        Return True
                        Exit Do
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        Me.StatusUpdateErrors += 1
                        'throw a system application exception and let the calling procedure determine if that application 
                        'should exit or continue processing data
                        Me.LastError = "Could not update the AP status values.  Duplicate values may be exported on the next cycle for the following criteria " & strCriteria & ". Error: " & Me.readExceptionMessage(ex)
                        Throw New System.ApplicationException(Me.LastError, ex.InnerException)
                        Exit Do
                    Else
                        Log("NGL.FreightMaster.Integration.clsAPExport.UpdateStatusEx Execution Error. Retry # " & intRetryCt.ToString & " of " & Me.Retry.ToString & ". " & ex.Message)
                    End If
                Finally
                    Try
                        objCom.Cancel()
                        objCom = Nothing
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.


        Catch ex As Exception
            Throw
        Finally
            Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                oCon.Close()
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="EDIString"></param>
    ''' <param name="strConnection"></param>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="AutoConfirm"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR 1/6/2015 v-7.0
    '''   Removed References to old RequireCNS logic
    '''   We now always use the BookSHID as the key value
    '''   mapped to segment B204 
    '''   Removed old code with comment tags no longer needed
    ''' Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    ''' Added fileName functionality
    ''' </remarks>
    Public Function getTruckLoadEDI204String(ByRef EDIString As String,
                                             ByVal strConnection As String,
                                             ByVal CarrierNumber As String,
                                             ByVal FileName As String,
                                             Optional ByVal AutoConfirm As Boolean = False,
                                             Optional sConfigFile As String = "") As ProcessDataReturnValues

        Dim oCarrierEDIBLL As New clsCarrierEDIBLL
        Dim oISA As New clsEDIISA
        Dim oIEA As New clsEDIIEA
        Dim oGS As New clsEDIGS
        Dim oGE As New clsEDIGE
        Dim strDate As String = Date.Now.ToString("yyyyMMdd")
        Dim strTime As String = Date.Now.ToString("HHmm")

        'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
        Dim oWCFParameters As New DAL.WCFParameters
        With oWCFParameters
            .UserName = ""
            .Database = Me.Database
            .DBServer = Me.DBServer
            .ConnectionString = strConnection
            .WCFAuthCode = "NGLSystem"
            .ValidateAccess = False
        End With
        Dim oEDIData As New DAL.NGLEDIData(oWCFParameters)

        'Added By LVV on 2/7/18 for v-8.1 PQ EDI
        ''Dim obj204OutSetting As New clsEDI204OutSetting()
        Dim strDefaultConfigFile As String = "C:\Data\EDIConfig.txt"
        If String.IsNullOrWhiteSpace(sConfigFile) OrElse Not System.IO.File.Exists(sConfigFile) Then
            sConfigFile = "C:\Data\EDIConfig.txt"
        End If

        Dim oSetting As New clsEDISettings()
        If System.IO.File.Exists(sConfigFile) Then
            'TODO: in v-8.0.1 add logic to pass in trading partner ID (From , To) and document type 
            'to new dynamic constructer for reading EDI document settings from the db.
            ''obj204OutSetting = oSetting.readEDI204OutSetting(sConfigFile)
            EDI204OutSetting = oSetting.readEDI204OutSetting(sConfigFile)
        End If


        Dim enmRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strTitle As String = ""
        Dim intErrors As Integer = 0
        Dim intCount As Integer = 0
        Dim strSource As String = "clsEDI.readTruckLoadObjectData"
        GroupEmailMsg = ""
        ITEmailMsg = ""
        Me.ImportTypeKey = IntegrationTypes.EDI204
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "EDI 204 Data Integration"
        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If

        Try
            RecordErrors = 0
            TotalRecords = 0
            'check for empty strings being passed as parameters
            If String.IsNullOrEmpty(CarrierNumber) OrElse CarrierNumber.Trim.Length < 1 Then
                LogError("Read 204 Truck Load Object Data Failure", "The Carrier Number Is required And may Not be blank Or empty.  Cannot read 204 data.", AdminEmail)
                Return ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            'Check if the Carrier Number exists
            Dim intCarrierControl As Integer = 0
            If Not Integer.TryParse(getScalarValue("Select top 1 CarrierControl from dbo.Carrier Where CarrierNumber = " & CarrierNumber, Source, True), intCarrierControl) Then
                LogError("Read 204 Truck Load Object Data Failure", "The Carrier Number, " & CarrierNumber & " Is Not valid.The Carrier Number Is required And must match an existing carrier record in the database.  Cannot read 204 data.", AdminEmail)
                Return ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            Dim intGroupNumber As Integer = 1
            'share the current settings including the db connection
            oCarrierEDIBLL.shareSettings(Me)

            'read the 204 data
            'Begin  by RHR for v-8.4.0.004 on 12/27/2021 added LTS data removed the dependency on xsd data Objects
            Dim oTable As FMData.spGetEDI204TruckLoadDataDataTable = Me.GetTruckLoadData(intCarrierControl)
            ' Get a reference to the NGLEDIData integration object.
            Dim oIntDAL As New DAL.NGLEDIData(oWCFParameters)
            Dim o204Header = oIntDAL.GetEDI204TruckLoadData(intCarrierControl)
            Dim intSegCounter As Integer = 0
            Dim blnFirstTime As Boolean = True
            If Not o204Header Is Nothing AndAlso o204Header.Count > 0 Then
                'If Not oTable Is Nothing AndAlso oTable.Rows.Count > 0 Then
                ' Modified by RHR for v-8.4.0.003 on 08/02/2021 added optional blnMapGS03ToISA08 parameter from EDI204OutSetting.MappingRules.getMapGS03ToISA08 setting default = false
                If Not oCarrierEDIBLL.fillEDIObjects(oISA, oGS, intCarrierControl, "204", EDI204OutSetting.MappingRules.getMapGS03ToISA08) Then
                    LastError = "Cannot read Carrier EDI Settings. " & oCarrierEDIBLL.LastError
                    Return ProcessDataReturnValues.nglDataIntegrationFailure
                End If

                'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                Dim DateSent As Date = Date.Now

                'For Each oRow As FMData.spGetEDI204TruckLoadDataRow In oTable
                For intRow As Integer = 0 To o204Header.Count - 1
                    'For intRow As Integer = 0 To oTable.Rows.Count - 1
                    Dim oRow As LTS.spGetEDI204TruckLoadDataResult = o204Header(intRow)
                    'Dim oRow As FMData.spGetEDI204TruckLoadDataRow = oTable.Rows(intRow)
                    'Check if this row has an SHID Number
                    If cleanEDI(nz(oRow.BookSHID, "")).Trim.Length < 1 Then
                        'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                        Dim s = "Warning! Cannot send EDI 204 to Carrier.  The order number " _
                            & nz(oRow.BookCarrOrderNumber, "") & "-" & nz(oRow.BookOrderSequence, 0).ToString _
                            & " has Not been assigned an SHID Number.  Orders without SHID Number Numbers cannot" _
                            & " be transmited via EDI."
                        'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                        GroupEmailMsg &= s
                        oEDIData.ArchiveEDI204(oRow.BookControl, DTO.tblLoadTender.LoadTenderStatusCodeEnum.DataValidationFail, s, DateSent)

                        GoTo PROCESS_NEXT
                    End If
                    'check the sender ID (on hold for now every 204 gets a unique ISA and GS record we use the blnFirstTime flag instead to determine if we need to create the ISE and GE record)
                    'If oISA.ISA06.Trim.Length > 1 AndAlso oISA.ISA06 <> nz(oRow.CompEDIPartnerCode, "") Then
                    If Not blnFirstTime Then
                        'Close the GS with a GE
                        With oGE
                            .GE01 = intSegCounter.ToString
                            .GE02 = oGS.GS06
                            EDIString &= .getEDIString(oISA.SegmentTerminator)
                        End With
                        'close the ISA with and ISE
                        With oIEA
                            .IEA01 = 1
                            .IEA02 = oISA.ISA13
                            EDIString &= .getEDIString(oISA.SegmentTerminator)
                        End With
                        'we only have one ISA for each record so we only get the next sequence numbers
                        oISA.ISA13 = oCarrierEDIBLL.getNextISASequence(oISA)
                        oGS.GS06 = oCarrierEDIBLL.getNextGSSequence(oGS)
                    End If
                    'Each record gets its own ISA record
                    blnFirstTime = False
                    'update the ISA and GS data for this trading partner
                    With oISA
                        .ISA01 = nz(oRow.CompEDISecurityQual, "00")
                        .ISA02 = nz(oRow.CompEDISecurityCode, "")
                        .ISA05 = nz(oRow.CompEDIPartnerQual, "")
                        .ISA06 = nz(oRow.CompEDIPartnerCode, "")
                        EDIString &= .getEDIString(oISA.SegmentTerminator)
                    End With
                    With oGS
                        .GS02 = nz(oRow.CompEDIPartnerCode, "")
                        EDIString &= .getEDIString(oISA.SegmentTerminator)
                    End With
                    'We only have one GS segment 
                    intSegCounter = 1
                    Dim strLoadNumber As String = Left(nz(oRow.BookCarrOrderNumber, "") & "-" & nz(oRow.BookOrderSequence, 0).ToString, 30)

                    'Added By LVV on 2/7/18 for v-8.1 PQ EDI
                    'NOTE: If Config file requires us to get the BookNotes, use the BookControl in oRow to get the notes from the DB
                    'Pass the notes in as 3 separate optional parameters
                    Dim notes1 = ""
                    Dim notes2 = ""
                    Dim notes3 = ""
                    If IncludeNotes1 OrElse IncludeNotes2 OrElse IncludeNotes3 Then
                        Dim oNotes As New DAL.NGLBookNoteData(oWCFParameters)
                        Dim n = oNotes.GetBookNotesFiltered(oRow.BookControl).FirstOrDefault() 'Since we are specifying a BookCotnrol this method will only ever return one record
                        If Not n Is Nothing Then
                            If IncludeNotes1 Then notes1 = n.BookNotesVisable1.Trim()
                            If IncludeNotes2 Then notes2 = n.BookNotesVisable2.Trim()
                            If IncludeNotes3 Then notes3 = n.BookNotesVisable3.Trim()
                        End If
                    End If


                    EDIString &= getTruckLoadEDIString(GetConsolidatedTruckLoadObject(intRow, o204Header, New clsEDITMSData(oRow), intSegCounter), oISA.SegmentTerminator, notes1, notes2, notes3)
                    strLoadNumber = nz(oRow.BookSHID, "")

                    saveEDITransaction(nz(oRow.BookControl, 0),
                               intCarrierControl,
                               0,
                               "204",
                               oISA.ISA06,
                               oISA.ISA08,
                               oISA.ISA13,
                               oGS.GS06,
                               nz(oRow.CarrierSCAC, ""),
                               strLoadNumber,
                               "Get Truck Load EDI 204 String",
                               "clsEDI204.getTruckLoadEDI204String")

                    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                    Dim GS06 As Integer = Integer.Parse(oGS.GS06)
                    oEDIData.InsertFileInfoTo204Table(strLoadNumber, FileName, DateSent, GS06)


PROCESS_NEXT:

                Next
                If Not blnFirstTime Then
                    'Close the GS with a GE
                    With oGE
                        .GE01 = intSegCounter.ToString
                        .GE02 = oGS.GS06
                        EDIString &= .getEDIString(oISA.SegmentTerminator)
                    End With
                    'close the ISA with and ISE
                    With oIEA
                        .IEA01 = 1
                        .IEA02 = oISA.ISA13
                        EDIString &= .getEDIString(oISA.SegmentTerminator)
                    End With
                End If
            End If
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Read EDI 204 Data Warning", "The following errors Or warnings were reported some 204 records may Not have been transmitted correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Read EDI 204 Data Failure", "The following errors Or warnings were reported some 204 records may Not have been transmitted correctly." & ITEmailMsg, AdminEmail)
            End If
            Return ProcessDataReturnValues.nglDataIntegrationComplete
        Catch ex As Exception
            LogException("Get Truck Load EDI 204 Strings Failure", "Could Not read 204 truck load records for carrier " & CarrierNumber & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsEDI.getTruckLoadEDI204Strings Failure")
        Finally
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try

        Return enmRet
    End Function

    ''' <summary>
    ''' Returns a 204 EDI document in EDIString for the provided SHID number
    ''' </summary>
    ''' <param name="EDIString"></param>
    ''' <param name="strConnection"></param>
    ''' <param name="BookSHID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.0 on 11/3/2016
    '''   primarily for testing but may be used in the future 
    '''   to resend an EDI 204 on demand.
    ''' </remarks>
    Public Function getTruckLoadEDI204String(ByRef EDIString As String,
                                            ByVal strConnection As String,
                                            ByVal BookSHID As String) As ProcessDataReturnValues
        Dim AutoConfirm As Boolean = False
        Dim oCarrierEDIBLL As New clsCarrierEDIBLL
        Dim oISA As New clsEDIISA
        Dim oIEA As New clsEDIIEA
        Dim oGS As New clsEDIGS
        Dim oGE As New clsEDIGE
        Dim strDate As String = Date.Now.ToString("yyyyMMdd")
        Dim strTime As String = Date.Now.ToString("HHmm")

        'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
        Dim oWCFParameters As New DAL.WCFParameters
        With oWCFParameters
            .UserName = ""
            .Database = Me.Database
            .DBServer = Me.DBServer
            .ConnectionString = strConnection
            .WCFAuthCode = "NGLSystem"
            .ValidateAccess = False
        End With
        Dim oEDIData As New DAL.NGLEDIData(oWCFParameters)

        'Added By LVV on 6/18/19 (PQ EDI) - kept getting bug because getTruckLoadEDIString() needs this value
        Me.EDI204OutSetting = New clsEDI204OutSetting()
        Me.EDI204OutSetting.MappingRules.ShowContractedCost = "True"


        Dim enmRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strTitle As String = ""
        Dim intErrors As Integer = 0
        Dim intCount As Integer = 0
        Dim strSource As String = "clsEDI204.getTruckLoadEDI204String"
        GroupEmailMsg = ""
        ITEmailMsg = ""
        Me.ImportTypeKey = IntegrationTypes.EDI204
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "EDI 204 Data Integration"
        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If

        Try
            RecordErrors = 0
            TotalRecords = 0
            'check for empty strings being passed as parameters
            If String.IsNullOrEmpty(BookSHID) OrElse BookSHID.Trim.Length < 1 Then
                LastError = "Read 204 Truck Load Object Data Failure The BookSHID Number Is required And may Not be blank Or empty.  Cannot read 204 data."
                Return ProcessDataReturnValues.nglDataIntegrationFailure
            End If

            Dim intGroupNumber As Integer = 1
            'share the current settings including the db connection
            oCarrierEDIBLL.shareSettings(Me)

            'read the 204 data 
            Dim o204Data As New List(Of EDI204Result)
            'call spUpdateLoadStatus only log errors, do not stop here is sp fails. 
            Using db As New LTSIntegrationDataDataContext(strConnection)
                Try
                    o204Data = (From d In db.spGetEDI204BySHID(BookSHID) Select EDI204Result.selectDTOData(d)).ToList()
                Catch ex As Exception
                    EDIString = ex.Message
                    Return ProcessDataReturnValues.nglDataIntegrationFailure
                End Try
            End Using

            Dim intSegCounter As Integer = 0
            Dim blnFirstTime As Boolean = True
            If Not o204Data Is Nothing AndAlso o204Data.Count > 0 Then
                ' Modified by RHR for v-8.4.0.003 on 08/02/2021 added optional blnMapGS03ToISA08 parameter from EDI204OutSetting.MappingRules.getMapGS03ToISA08 setting default = false
                If Not oCarrierEDIBLL.fillEDIObjects(oISA, oGS, o204Data(0).CarrierControl.Value, "204", EDI204OutSetting.MappingRules.getMapGS03ToISA08) Then
                    LastError = "Cannot read Carrier EDI Settings. " & oCarrierEDIBLL.LastError
                    Return ProcessDataReturnValues.nglDataIntegrationFailure
                End If

                'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                Dim DateSent As Date = Date.Now

                'For Each oRow As FMData.spGetEDI204TruckLoadDataRow In oTable
                'For Each oRow In o204Data
                For intRow As Integer = 0 To o204Data.Count - 1
                    Dim oRow As EDI204Result = o204Data(intRow)
                    'Check if this row has an SHID Number
                    If cleanEDI(oRow.BookSHID).Trim.Length < 1 Then
                        'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                        Dim s = "Warning! Cannot send EDI 204 to Carrier.  The order number " _
                            & oRow.BookCarrOrderNumber & "-" & If(oRow.BookOrderSequence, 0).ToString _
                            & " has Not been assigned an SHID Number.  Orders without SHID Number Numbers cannot" _
                            & " be transmited via EDI."
                        'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                        GroupEmailMsg &= s
                        oEDIData.ArchiveEDI204(oRow.BookControl, DTO.tblLoadTender.LoadTenderStatusCodeEnum.DataValidationFail, s, DateSent)

                        GoTo PROCESS_NEXT
                    End If
                    'check the sender ID (on hold for now every 204 gets a unique ISA and GS record we use the blnFirstTime flag instead to determine if we need to create the ISE and GE record)
                    'If oISA.ISA06.Trim.Length > 1 AndAlso oISA.ISA06 <> nz(oRow.CompEDIPartnerCode, "") Then
                    If Not blnFirstTime Then
                        'Close the GS with a GE
                        With oGE
                            .GE01 = intSegCounter.ToString
                            .GE02 = oGS.GS06
                            EDIString &= .getEDIString(oISA.SegmentTerminator)
                        End With
                        'close the ISA with and ISE
                        With oIEA
                            .IEA01 = 1
                            .IEA02 = oISA.ISA13
                            EDIString &= .getEDIString(oISA.SegmentTerminator)
                        End With
                        'we only have one ISA for each record so we only get the next sequence numbers
                        oISA.ISA13 = oCarrierEDIBLL.getNextISASequence(oISA)
                        oGS.GS06 = oCarrierEDIBLL.getNextGSSequence(oGS)
                    End If
                    'Each record gets its own ISA record
                    blnFirstTime = False
                    'update the ISA and GS data for this trading partner
                    With oISA
                        .ISA01 = If(String.IsNullOrWhiteSpace(oRow.CompEDISecurityQual), "00", oRow.CompEDISecurityQual)
                        .ISA02 = oRow.CompEDISecurityCode
                        .ISA05 = oRow.CompEDIPartnerQual
                        .ISA06 = oRow.CompEDIPartnerCode
                        EDIString &= .getEDIString(oISA.SegmentTerminator)
                    End With
                    With oGS
                        .GS02 = oRow.CompEDIPartnerCode
                        EDIString &= .getEDIString(oISA.SegmentTerminator)
                    End With
                    'We only have one GS segment 
                    intSegCounter = 1
                    Dim strLoadNumber As String = Left(oRow.BookCarrOrderNumber & "-" & If(oRow.BookOrderSequence, 0).ToString, 30)

                    EDIString &= getTruckLoadEDIString(GetConsolidatedTruckLoadObject(intRow, o204Data, New clsEDITMSData(oRow), intSegCounter), oISA.SegmentTerminator)
                    strLoadNumber = oRow.BookSHID

                    saveEDITransaction(If(oRow.BookControl, 0),
                               If(oRow.CarrierControl, 0),
                               0,
                               "204",
                               oISA.ISA06,
                               oISA.ISA08,
                               oISA.ISA13,
                               oGS.GS06,
                               oRow.CarrierSCAC,
                               strLoadNumber,
                               "Get Truck Load EDI 204 String For SHID",
                               "clsEDI204.getTruckLoadEDI204String")

                    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                    Dim GS06 As Integer = Integer.Parse(oGS.GS06)
                    oEDIData.InsertFileInfoTo204Table(strLoadNumber, "N/A", DateSent, GS06)


PROCESS_NEXT:

                Next
                If Not blnFirstTime Then
                    'Close the GS with a GE
                    With oGE
                        .GE01 = intSegCounter.ToString
                        .GE02 = oGS.GS06
                        EDIString &= .getEDIString(oISA.SegmentTerminator)
                    End With
                    'close the ISA with and ISE
                    With oIEA
                        .IEA01 = 1
                        .IEA02 = oISA.ISA13
                        EDIString &= .getEDIString(oISA.SegmentTerminator)
                    End With
                End If
            End If
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Read EDI 204 Data Warning", "The following errors Or warnings were reported some 204 records may Not have been transmitted correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Read EDI 204 Data Failure", "The following errors Or warnings were reported some 204 records may Not have been transmitted correctly." & ITEmailMsg, AdminEmail)
            End If
            Return ProcessDataReturnValues.nglDataIntegrationComplete
        Catch ex As Exception
            LogException("Get Truck Load EDI 204 Strings Failure", "Could Not read 204 truck load records for SHID " & BookSHID & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsEDI.getTruckLoadEDI204Strings Failure")
        Finally
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try

        Return enmRet
    End Function


    Private Function getSESequence(ByVal intSequence As Integer) As Integer
        'This function should lookup the sequence number in the database
        If intSequence < 1 Then
            Return 1
        ElseIf intSequence > 99998 Then
            Return 1
        Else
            Return intSequence + 1
        End If

    End Function

    Private Sub add320Items(ByVal BookControl As Integer, ByRef Loop320 As List(Of clsEDI204Loop320), ByRef intSegments As Integer)
        Dim oItems = GetItemDetailData(BookControl)
        If Not oItems Is Nothing AndAlso oItems.Count > 0 Then
            For i As Integer = 0 To oItems.Count - 1
                Dim oL320 As New clsEDI204Loop320
                With oItems(i)
                    oL320.L5.L501 = i + 1
                    oL320.L5.L502 = Left(nz(oItems(i), "BookItemDescription", ""), 50)
                    oL320.L5.L503 = getItemCommodityCode(nz(oItems(i), "BookLoadCom", "D"), nz(oItems(i), "BookItemHazmat", "N"))
                    oL320.L5.L504 = "N"
                    intSegments += 1
                    oL320.AT8.AT801 = "G"
                    oL320.AT8.AT802 = "L"
                    oL320.AT8.AT803 = Left(CInt(nz(oItems(i), "BookItemWeight", 1)).ToString(), 10)
                    oL320.AT8.AT804 = Left(CInt(nz(oItems(i), "BookItemPallets", 1)).ToString(), 7)
                    oL320.AT8.AT805 = Left(nz(oItems(i), "BookItemQtyOrdered", 1).ToString(), 7)
                    oL320.AT8.AT806 = "E"
                    oL320.AT8.AT807 = Left(nz(oItems(i), "BookItemCube", 1).ToString(), 8)
                    intSegments += 1
                End With
                If Loop320 Is Nothing Then Loop320 = New List(Of clsEDI204Loop320)
                Loop320.Add(oL320)
            Next
        End If
    End Sub

    Private Function getItemCommodityCode(ByVal sCom As String, ByVal sHazmat As String) As String
        sCom = sCom.Trim()
        Select Case sCom
            Case "D"
                If Not String.IsNullOrWhiteSpace(sHazmat) AndAlso sHazmat = "Y" Then
                    Return "DRHZ"
                Else
                    Return "DRY"
                End If
            Case "F"
                If Not String.IsNullOrWhiteSpace(sHazmat) AndAlso sHazmat = "Y" Then
                    Return "FRHZ"
                Else
                    Return "FZ"
                End If
            Case "M"
                Return "001"
            Case "R"
                If Not String.IsNullOrWhiteSpace(sHazmat) AndAlso sHazmat = "Y" Then
                    Return "RFHZ"
                Else
                    Return "RF"
                End If
            Case "C"
                Return "TEMP"
            Case "H"
                Return "HAZ"
            Case Else
                Return "MISC"
        End Select
    End Function

    Private Function getEquipCommodityCode(ByVal sCom As String) As String
        sCom = sCom.Trim()

        Select Case sCom

            Case "D"
                Return "TF" 'Trailer, Dry Freight"
            Case "F"
                Return "FF" 'Frozen Food Trailer"
            Case "M"
                Return "TW" 'Trailer, Refrigerated capable of keeping product cold, Different from a temperature controlled trailer which is able to keep product at a constant temperature
            Case "R"
                Return "RT" 'Controlled Temperature Trailer (Reefer)
            Case "C"
                Return "RT" 'Controlled Temperature Trailer (Reefer)
            Case Else
                Return "TL" 'Trailer (not otherwise specified)    

        End Select
    End Function

    Protected Sub processOtherPickups(ByRef strOrigStops As List(Of String),
                                      ByRef oStops As List(Of clsEDITMSData),
                                      ByRef oTruckLoad As clsEDITruckLoad,
                                      ByRef oTMS As clsEDITMSData,
                                      ByRef intSegments As Integer,
                                      ByRef intStopNbr As Integer,
                                      ByRef dblTotalWgt As Double,
                                      ByRef intTotalCube As Integer,
                                      ByRef intTotalCases As Integer,
                                      ByRef dlbTotalPallets As Double,
                                      ByRef decTotalCost As Decimal,
                                      ByVal intSeed As Integer)
        'Loop through each additional stop and add the data
        For i As Integer = intSeed To oStops.Count - 1
            Dim oStop As clsEDITMSData = oStops(i)
            If oTMS.EDICombineOrdersForStops <> 0 Then
                If oStop.BookOrigAddress1 = oTMS.BookOrigAddress1 Then
                    'this is an existing stop so just add the order number to the L11 record
                    Dim oL11ON As New clsEDIL11
                    With oL11ON
                        .L1101 = Left(oStop.BookCarrOrderNumber & "-" & oStop.BookOrderSequence.ToString, 30)
                        .L1102 = "ON" 'We use Order Number on pickup
                        .L1103 = "FS"
                    End With
                    If oL11ON.L1101.Trim.Length > 0 Then
                        oTruckLoad.Loop300(intStopNbr - 1).L11s.Add(oL11ON)
                        intSegments += 1 'increase the segment counter after each segment
                    End If
                    Dim oL11PO As New clsEDIL11
                    With oL11PO
                        .L1101 = oStop.BookLoadPONumber 'Customer PO Number
                        .L1102 = "PO" 'We use Order Number on pickup
                        .L1103 = "FS"
                    End With
                    If oL11PO.L1101.Trim.Length > 0 Then
                        oTruckLoad.Loop300(intStopNbr - 1).L11s.Add(oL11PO)
                        intSegments += 1 'increase the segment counter after each segment
                    End If
                    With oTruckLoad.Loop300(intStopNbr - 1)
                        Dim dblWgt As Double = 0
                        Double.TryParse(.S5.S503, dblWgt)
                        .S5.S503 = Left((dblWgt + oStop.BookTotalWgt).ToString, 10)
                        Dim intCases As Integer = 0
                        Integer.TryParse(.S5.S505, intCases)
                        .S5.S505 = Left((intCases + oStop.BookTotalCases).ToString, 10)
                        Dim intCubes As Integer = 0
                        Integer.TryParse(.S5.S507, intCubes)
                        .S5.S507 = Left((intCubes + oStop.BookTotalCube).ToString, 8)
                        Dim dblPallets As Double = 0
                        Double.TryParse(.PLD.PLD01, dblPallets)
                        .PLD.PLD01 = Left((dblPallets + oStop.BookTotalPL).ToString, 3)
                    End With
                ElseIf Not strOrigStops.Contains(oStop.BookOrigAddress1) Then
                    'this is a new address so we need to add a new 300 loop
                    intStopNbr += 1
                    ReDim Preserve oTruckLoad.Loop300(intStopNbr - 1)
                    oTruckLoad.Loop300(intStopNbr - 1) = Build204300LoopByOrig(oTruckLoad, oStop, intSegments, intStopNbr, "PL")
                    strOrigStops.Add(oStop.BookOrigAddress1)
                    'Call this process recursively to loop through other stops
                    processOtherPickups(strOrigStops, oStops, oTruckLoad, oStop, intSegments, intStopNbr, dblTotalWgt, intTotalCube, intTotalCases, dlbTotalPallets, decTotalCost, i + 1)
                End If
            Else
                intStopNbr += 1
                ReDim Preserve oTruckLoad.Loop300(intStopNbr - 1)
                oTruckLoad.Loop300(intStopNbr - 1) = Build204300LoopByOrig(oTruckLoad, oStop, intSegments, intStopNbr, "PL")
                strOrigStops.Add(oStop.BookOrigAddress1)
                'Call this process recursively to loop through other stops
                processOtherPickups(strOrigStops, oStops, oTruckLoad, oStop, intSegments, intStopNbr, dblTotalWgt, intTotalCube, intTotalCases, dlbTotalPallets, decTotalCost, i + 1)
            End If
            dblTotalWgt += oStop.BookTotalWgt
            intTotalCube += oStop.BookTotalCube
            intTotalCases += oStop.BookTotalCases
            dlbTotalPallets += oStop.BookTotalPL
            decTotalCost += oStop.BookRevTotalCost
        Next
    End Sub

    ''' <summary>
    ''' add additional item details to the previous loop from other orders using the same address
    ''' </summary>
    ''' <param name="oStop"></param>
    ''' <param name="oPreviousLoop"></param>
    ''' <param name="intSegments"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.0.119 on 09/19/2019
    '''   fixed bug where intSegments were not being incremented for the additional L11 segments
    ''' </remarks>
    Protected Sub processOtherPickups(ByRef oStop As clsEDITMSData,
                                      ByRef oPreviousLoop As clsEDI204Loop300,
                                      ByRef intSegments As Integer)
        'this is an existing stop so just add the order number to the L11 record
        Dim oL11ON As New clsEDIL11
        With oL11ON
            .L1101 = Left(oStop.BookCarrOrderNumber & "-" & oStop.BookOrderSequence.ToString, 30)
            .L1102 = "ON" 'We use Order Number on pickup
            .L1103 = "FS"
        End With
        If oL11ON.L1101.Trim.Length > 0 Then
            oPreviousLoop.L11s.Add(oL11ON)
            intSegments += 1 'increase the segment counter after each segment
        End If
        Dim oL11PO As New clsEDIL11
        With oL11PO
            .L1101 = oStop.BookLoadPONumber 'Customer PO Number
            .L1102 = "PO" 'We use Order Number on pickup
            .L1103 = "FS"
        End With
        If oL11PO.L1101.Trim.Length > 0 Then
            oPreviousLoop.L11s.Add(oL11PO)
            intSegments += 1 'increase the segment counter after each segment
        End If
        With oPreviousLoop
            Dim dblWgt As Double = 0
            Double.TryParse(.S5.S503, dblWgt)
            .S5.S503 = Left((dblWgt + oStop.BookTotalWgt).ToString, 10)
            Dim intCases As Integer = 0
            Integer.TryParse(.S5.S505, intCases)
            .S5.S505 = Left((intCases + oStop.BookTotalCases).ToString, 10)
            Dim intCubes As Integer = 0
            Integer.TryParse(.S5.S507, intCubes)
            .S5.S507 = Left((intCubes + oStop.BookTotalCube).ToString, 8)
            Dim dblPallets As Double = 0
            Double.TryParse(.PLD.PLD01, dblPallets)
            .PLD.PLD01 = Left((dblPallets + oStop.BookTotalPL).ToString, 3)
        End With
        add320Items(oStop.BookControl, oPreviousLoop.Loop320, intSegments)
    End Sub

    Protected Sub processOtherDropOffs(ByRef strDestStops As List(Of String),
                                          ByRef oStops As List(Of clsEDITMSData),
                                          ByRef oTruckLoad As clsEDITruckLoad,
                                          ByRef oTMS As clsEDITMSData,
                                          ByRef intSegments As Integer,
                                          ByRef intStopNbr As Integer,
                                          ByVal intSeed As Integer)
        'Loop through each additional stop and add the data
        For i As Integer = intSeed To oStops.Count - 1
            Dim oStop As clsEDITMSData = oStops(i)
            If oTMS.EDICombineOrdersForStops <> 0 Then
                If oStop.BookDestAddress1 = oTMS.BookDestAddress1 Then
                    'this is an existing stop so just add the po and order number to the L11 record  
                    Dim oL11ON As New clsEDIL11
                    With oL11ON
                        .L1101 = Left(oStop.BookCarrOrderNumber & "-" & oStop.BookOrderSequence.ToString, 30)
                        .L1102 = "ON" 'We use Order Number on pickup
                        .L1103 = "FS"
                    End With
                    If oL11ON.L1101.Trim.Length > 0 Then
                        oTruckLoad.Loop300(intStopNbr - 1).L11s.Add(oL11ON)
                        intSegments += 1 'increase the segment counter after each segment
                    End If
                    Dim oL11PO As New clsEDIL11
                    With oL11PO
                        .L1101 = oStop.BookLoadPONumber 'Customer PO Number
                        .L1102 = "PO" 'We use Order Number on pickup
                        .L1103 = "FS"
                    End With
                    If oL11PO.L1101.Trim.Length > 0 Then
                        oTruckLoad.Loop300(intStopNbr - 1).L11s.Add(oL11PO)
                        intSegments += 1 'increase the segment counter after each segment
                    End If
                    With oTruckLoad.Loop300(intStopNbr - 1)
                        Dim dblWgt As Double = 0
                        Double.TryParse(.S5.S503, dblWgt)
                        .S5.S503 = Left((dblWgt + oStop.BookTotalWgt).ToString, 10)
                        Dim intCases As Integer = 0
                        Integer.TryParse(.S5.S505, intCases)
                        .S5.S505 = Left((intCases + oStop.BookTotalCases).ToString, 10)
                        Dim intCubes As Integer = 0
                        Integer.TryParse(.S5.S507, intCubes)
                        .S5.S507 = Left((intCubes + oStop.BookTotalCube).ToString, 8)
                        Dim dblPallets As Double = 0
                        Double.TryParse(.PLD.PLD01, dblPallets)
                        .PLD.PLD01 = Left((dblPallets + oStop.BookTotalPL).ToString, 3)
                    End With
                ElseIf Not strDestStops.Contains(oStop.BookDestAddress1) Then
                    'this is a new address so we need to add a new 300 loop
                    intStopNbr += 1
                    ReDim Preserve oTruckLoad.Loop300(intStopNbr - 1)
                    oTruckLoad.Loop300(intStopNbr - 1) = Build204300LoopByDest(oTruckLoad, oStop, intSegments, intStopNbr, "PU")
                    strDestStops.Add(oStop.BookDestAddress1)
                    processOtherDropOffs(strDestStops, oStops, oTruckLoad, oStop, intSegments, intStopNbr, i + 1)
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' add additional item details to the previous loop from other orders using the same address
    ''' </summary>
    ''' <param name="oStop"></param>
    ''' <param name="oPreviousLoop"></param>
    ''' <param name="intSegments"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.0.119 on 09/19/2019
    '''   fixed bug where intSegments were not being incremented for the additional L11 segments
    ''' </remarks>
    Protected Sub processOtherDropOffs(ByRef oStop As clsEDITMSData,
                                       ByRef oPreviousLoop As clsEDI204Loop300,
                                       ByRef intSegments As Integer)
        'this is an existing stop so just add the po and order number to the L11 record  
        Dim oL11ON As New clsEDIL11
        With oL11ON
            .L1101 = Left(oStop.BookCarrOrderNumber & "-" & oStop.BookOrderSequence.ToString, 30)
            .L1102 = "ON" 'We use Order Number on pickup
            .L1103 = "FS"
        End With
        If oL11ON.L1101.Trim.Length > 0 Then
            oPreviousLoop.L11s.Add(oL11ON)
            intSegments += 1 'increase the segment counter after each segment
        End If
        Dim oL11PO As New clsEDIL11
        With oL11PO
            .L1101 = oStop.BookLoadPONumber 'Customer PO Number
            .L1102 = "PO" 'We use Order Number on pickup
            .L1103 = "FS"
        End With
        If oL11PO.L1101.Trim.Length > 0 Then
            oPreviousLoop.L11s.Add(oL11PO)
            intSegments += 1 'increase the segment counter after each segment
        End If
        With oPreviousLoop
            Dim dblWgt As Double = 0
            Double.TryParse(.S5.S503, dblWgt)
            .S5.S503 = Left((dblWgt + oStop.BookTotalWgt).ToString, 10)
            Dim intCases As Integer = 0
            Integer.TryParse(.S5.S505, intCases)
            .S5.S505 = Left((intCases + oStop.BookTotalCases).ToString, 10)
            Dim intCubes As Integer = 0
            Integer.TryParse(.S5.S507, intCubes)
            .S5.S507 = Left((intCubes + oStop.BookTotalCube).ToString, 8)
            Dim dblPallets As Double = 0
            Double.TryParse(.PLD.PLD01, dblPallets)
            .PLD.PLD01 = Left((dblPallets + oStop.BookTotalPL).ToString, 3)
        End With
        add320Items(oStop.BookControl, oPreviousLoop.Loop320, intSegments)
    End Sub

    Private Function GetStringSections(ByVal s As String, ByVal sectionLength As Integer) As String()
        'Calculate the length needed for the array
        Dim l As Integer = (s.Length / sectionLength) + 1
        Dim strArray(l) As String
        Dim i = 0

        Do While s.Length > sectionLength
            strArray(i) = s.Substring(0, sectionLength)
            s = s.Remove(0, sectionLength)
            i += 1
        Loop
        If s.Length > 0 Then strArray(i) = s

        Return strArray
    End Function


    ''' <summary>
    ''' Append new NTE segments to the EDI document for each 80 character or less section of the notes 
    ''' </summary>
    ''' <param name="sEdi"></param>
    ''' <param name="strNotes"></param>
    ''' <param name="sSegTerm"></param>
    ''' <param name="intSegments"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.3.007 on 03/23/2023 added logic to 
    '''     increment intSegments counter to be added to SE01 by the caller
    ''' </remarks>
    Private Sub AddVisibleNotes(ByRef sEdi As Text.StringBuilder, ByVal strNotes As String, ByVal sSegTerm As String, ByRef intSegments As Integer)
        If Not String.IsNullOrWhiteSpace(strNotes) Then
            Dim qual = "DEL"
            If strNotes.Substring(0, 3).ToUpper() = "LOI" Then qual = "LOI"
            If strNotes.Length > 80 Then
                'Split the string into 80 character segments and an NTE for each 
                Dim noteSections = GetStringSections(strNotes, 80)
                For i As Integer = 0 To (noteSections.Length - 1)
                    If Not noteSections(i) Is Nothing Then
                        sEdi.AppendFormat("NTE*{0}*{1}{2}", qual, noteSections(i), sSegTerm)
                        intSegments += 1
                    End If
                Next
            Else
                sEdi.AppendFormat("NTE*{0}*{1}{2}", qual, strNotes, sSegTerm)
                intSegments += 1
            End If
        End If
    End Sub


#End Region

End Class

''' <summary>
''' Shared Copy of the 204 results used by different stored procedure and queries
''' </summary>
''' <remarks>
''' Created by RHR for v-7.0.6.0 on 11/3/2016 
'''   allows extensibility for 204 results for single
'''   load vs all loads when migrating the Linq to sql procedures
''' </remarks>
Public Class EDI204Result

    Private _ControlNbr As System.Nullable(Of Integer)

    Private _CarrierSCAC As String

    Private _CarrierNumber As System.Nullable(Of Integer)

    Private _CarrierName As String

    Private _CarrierControl As System.Nullable(Of Integer)

    Private _BookControl As System.Nullable(Of Integer)

    Private _BookProNumber As String

    Private _BookConsPrefix As String

    Private _BookStopNo As System.Nullable(Of Short)

    Private _BookCarrOrderNumber As String

    Private _BookOrderSequence As System.Nullable(Of Integer)

    Private _BookRouteFinalCode As String

    Private _BookTransactionPurpose As String

    Private _BookTotalCases As System.Nullable(Of Integer)

    Private _BookTotalWgt As System.Nullable(Of Double)

    Private _BookTotalPL As System.Nullable(Of Integer)

    Private _BookTotalCube As System.Nullable(Of Integer)

    Private _BookDateLoad As System.Nullable(Of Date)

    Private _BookDateRequired As System.Nullable(Of Date)

    Private _BookCarrScheduleDate As System.Nullable(Of Date)

    Private _BookCarrScheduleTime As System.Nullable(Of Date)

    Private _BookCarrApptDate As System.Nullable(Of Date)

    Private _BookCarrApptTime As System.Nullable(Of Date)

    Private _BookOrigName As String

    Private _BookOrigAddress1 As String

    Private _BookOrigAddress2 As String

    Private _BookOrigAddress3 As String

    Private _BookOrigCity As String

    Private _BookOrigState As String

    Private _BookOrigCountry As String

    Private _BookOrigZip As String

    Private _BookOrigPhone As String

    Private _BookOrigIDENTIFICATIONCODEQUALIFIER As String

    Private _BookOrigCompanyNumber As String

    Private _BookDestName As String

    Private _BookDestAddress1 As String

    Private _BookDestAddress2 As String

    Private _BookDestAddress3 As String

    Private _BookDestCity As String

    Private _BookDestState As String

    Private _BookDestCountry As String

    Private _BookDestZip As String

    Private _BookDestPhone As String

    Private _BookDestIDENTIFICATIONCODEQUALIFIER As String

    Private _BookDestCompanyNumber As String

    Private _BookLoadPONumber As String

    Private _BookLoadCom As String

    Private _CommCodeDescription As String

    Private _LaneComments As String

    Private _LaneOriginAddressUse As System.Nullable(Of Boolean)

    Private _BookTrackDate As System.Nullable(Of Date)

    Private _CompEDISecurityQual As String

    Private _CompEDISecurityCode As String

    Private _CompEDIPartnerQual As String

    Private _CompEDIPartnerCode As String

    Private _CompEDIEmailNotificationOn As System.Nullable(Of Boolean)

    Private _CompEDIEmailAddress As String

    Private _CompEDIAcknowledgementRequested As System.Nullable(Of Boolean)

    Private _CompEDIMethodOfPayment As String

    Private _BookRouteConsFlag As System.Nullable(Of Boolean)

    Private _BookRevTotalCost As System.Nullable(Of Decimal)

    Private _BillToCompName As String

    Private _BillToCompNumber As String

    Private _BillToCompAddress1 As String

    Private _BillToCompAddress2 As String

    Private _BillToCompCity As String

    Private _BillToCompState As String

    Private _BillToCompZip As String

    Private _BillToCompCountry As String

    Private _EDICombineOrdersForStops As System.Nullable(Of Double)

    Private _BookCustCompControl As System.Nullable(Of Integer)

    Private _BookSHID As String

    Public Sub New()
        MyBase.New()
    End Sub

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_ControlNbr", DbType:="Int")>
    Public Property ControlNbr() As System.Nullable(Of Integer)
        Get
            Return Me._ControlNbr
        End Get
        Set(value As System.Nullable(Of Integer))
            If (Me._ControlNbr.Equals(value) = False) Then
                Me._ControlNbr = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_CarrierSCAC", DbType:="NVarChar(4)")>
    Public Property CarrierSCAC() As String
        Get
            Return Me._CarrierSCAC
        End Get
        Set(value As String)
            If (String.Equals(Me._CarrierSCAC, value) = False) Then
                Me._CarrierSCAC = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_CarrierNumber", DbType:="Int")>
    Public Property CarrierNumber() As System.Nullable(Of Integer)
        Get
            Return Me._CarrierNumber
        End Get
        Set(value As System.Nullable(Of Integer))
            If (Me._CarrierNumber.Equals(value) = False) Then
                Me._CarrierNumber = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_CarrierName", DbType:="NVarChar(40)")>
    Public Property CarrierName() As String
        Get
            Return Me._CarrierName
        End Get
        Set(value As String)
            If (String.Equals(Me._CarrierName, value) = False) Then
                Me._CarrierName = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_CarrierControl", DbType:="Int")>
    Public Property CarrierControl() As System.Nullable(Of Integer)
        Get
            Return Me._CarrierControl
        End Get
        Set(value As System.Nullable(Of Integer))
            If (Me._CarrierControl.Equals(value) = False) Then
                Me._CarrierControl = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookControl", DbType:="Int")>
    Public Property BookControl() As System.Nullable(Of Integer)
        Get
            Return Me._BookControl
        End Get
        Set(value As System.Nullable(Of Integer))
            If (Me._BookControl.Equals(value) = False) Then
                Me._BookControl = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookProNumber", DbType:="NVarChar(20)")>
    Public Property BookProNumber() As String
        Get
            Return Me._BookProNumber
        End Get
        Set(value As String)
            If (String.Equals(Me._BookProNumber, value) = False) Then
                Me._BookProNumber = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookConsPrefix", DbType:="NVarChar(20)")>
    Public Property BookConsPrefix() As String
        Get
            Return Me._BookConsPrefix
        End Get
        Set(value As String)
            If (String.Equals(Me._BookConsPrefix, value) = False) Then
                Me._BookConsPrefix = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookStopNo", DbType:="SmallInt")>
    Public Property BookStopNo() As System.Nullable(Of Short)
        Get
            Return Me._BookStopNo
        End Get
        Set(value As System.Nullable(Of Short))
            If (Me._BookStopNo.Equals(value) = False) Then
                Me._BookStopNo = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookCarrOrderNumber", DbType:="NVarChar(20)")>
    Public Property BookCarrOrderNumber() As String
        Get
            Return Me._BookCarrOrderNumber
        End Get
        Set(value As String)
            If (String.Equals(Me._BookCarrOrderNumber, value) = False) Then
                Me._BookCarrOrderNumber = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookOrderSequence", DbType:="Int")>
    Public Property BookOrderSequence() As System.Nullable(Of Integer)
        Get
            Return Me._BookOrderSequence
        End Get
        Set(value As System.Nullable(Of Integer))
            If (Me._BookOrderSequence.Equals(value) = False) Then
                Me._BookOrderSequence = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookRouteFinalCode", DbType:="NVarChar(2)")>
    Public Property BookRouteFinalCode() As String
        Get
            Return Me._BookRouteFinalCode
        End Get
        Set(value As String)
            If (String.Equals(Me._BookRouteFinalCode, value) = False) Then
                Me._BookRouteFinalCode = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookTransactionPurpose", DbType:="NVarChar(2)")>
    Public Property BookTransactionPurpose() As String
        Get
            Return Me._BookTransactionPurpose
        End Get
        Set(value As String)
            If (String.Equals(Me._BookTransactionPurpose, value) = False) Then
                Me._BookTransactionPurpose = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookTotalCases", DbType:="Int")>
    Public Property BookTotalCases() As System.Nullable(Of Integer)
        Get
            Return Me._BookTotalCases
        End Get
        Set(value As System.Nullable(Of Integer))
            If (Me._BookTotalCases.Equals(value) = False) Then
                Me._BookTotalCases = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookTotalWgt", DbType:="Float")>
    Public Property BookTotalWgt() As System.Nullable(Of Double)
        Get
            Return Me._BookTotalWgt
        End Get
        Set(value As System.Nullable(Of Double))
            If (Me._BookTotalWgt.Equals(value) = False) Then
                Me._BookTotalWgt = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookTotalPL", DbType:="Int")>
    Public Property BookTotalPL() As System.Nullable(Of Integer)
        Get
            Return Me._BookTotalPL
        End Get
        Set(value As System.Nullable(Of Integer))
            If (Me._BookTotalPL.Equals(value) = False) Then
                Me._BookTotalPL = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookTotalCube", DbType:="Int")>
    Public Property BookTotalCube() As System.Nullable(Of Integer)
        Get
            Return Me._BookTotalCube
        End Get
        Set(value As System.Nullable(Of Integer))
            If (Me._BookTotalCube.Equals(value) = False) Then
                Me._BookTotalCube = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookDateLoad", DbType:="DateTime")>
    Public Property BookDateLoad() As System.Nullable(Of Date)
        Get
            Return Me._BookDateLoad
        End Get
        Set(value As System.Nullable(Of Date))
            If (Me._BookDateLoad.Equals(value) = False) Then
                Me._BookDateLoad = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookDateRequired", DbType:="DateTime")>
    Public Property BookDateRequired() As System.Nullable(Of Date)
        Get
            Return Me._BookDateRequired
        End Get
        Set(value As System.Nullable(Of Date))
            If (Me._BookDateRequired.Equals(value) = False) Then
                Me._BookDateRequired = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookCarrScheduleDate", DbType:="DateTime")>
    Public Property BookCarrScheduleDate() As System.Nullable(Of Date)
        Get
            Return Me._BookCarrScheduleDate
        End Get
        Set(value As System.Nullable(Of Date))
            If (Me._BookCarrScheduleDate.Equals(value) = False) Then
                Me._BookCarrScheduleDate = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookCarrScheduleTime", DbType:="DateTime")>
    Public Property BookCarrScheduleTime() As System.Nullable(Of Date)
        Get
            Return Me._BookCarrScheduleTime
        End Get
        Set(value As System.Nullable(Of Date))
            If (Me._BookCarrScheduleTime.Equals(value) = False) Then
                Me._BookCarrScheduleTime = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookCarrApptDate", DbType:="DateTime")>
    Public Property BookCarrApptDate() As System.Nullable(Of Date)
        Get
            Return Me._BookCarrApptDate
        End Get
        Set(value As System.Nullable(Of Date))
            If (Me._BookCarrApptDate.Equals(value) = False) Then
                Me._BookCarrApptDate = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookCarrApptTime", DbType:="DateTime")>
    Public Property BookCarrApptTime() As System.Nullable(Of Date)
        Get
            Return Me._BookCarrApptTime
        End Get
        Set(value As System.Nullable(Of Date))
            If (Me._BookCarrApptTime.Equals(value) = False) Then
                Me._BookCarrApptTime = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookOrigName", DbType:="NVarChar(4000)")>
    Public Property BookOrigName() As String
        Get
            Return Me._BookOrigName
        End Get
        Set(value As String)
            If (String.Equals(Me._BookOrigName, value) = False) Then
                Me._BookOrigName = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookOrigAddress1", DbType:="NVarChar(4000)")>
    Public Property BookOrigAddress1() As String
        Get
            Return Me._BookOrigAddress1
        End Get
        Set(value As String)
            If (String.Equals(Me._BookOrigAddress1, value) = False) Then
                Me._BookOrigAddress1 = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookOrigAddress2", DbType:="NVarChar(4000)")>
    Public Property BookOrigAddress2() As String
        Get
            Return Me._BookOrigAddress2
        End Get
        Set(value As String)
            If (String.Equals(Me._BookOrigAddress2, value) = False) Then
                Me._BookOrigAddress2 = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookOrigAddress3", DbType:="NVarChar(4000)")>
    Public Property BookOrigAddress3() As String
        Get
            Return Me._BookOrigAddress3
        End Get
        Set(value As String)
            If (String.Equals(Me._BookOrigAddress3, value) = False) Then
                Me._BookOrigAddress3 = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookOrigCity", DbType:="NVarChar(25)")>
    Public Property BookOrigCity() As String
        Get
            Return Me._BookOrigCity
        End Get
        Set(value As String)
            If (String.Equals(Me._BookOrigCity, value) = False) Then
                Me._BookOrigCity = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookOrigState", DbType:="NVarChar(8)")>
    Public Property BookOrigState() As String
        Get
            Return Me._BookOrigState
        End Get
        Set(value As String)
            If (String.Equals(Me._BookOrigState, value) = False) Then
                Me._BookOrigState = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookOrigCountry", DbType:="NVarChar(30)")>
    Public Property BookOrigCountry() As String
        Get
            Return Me._BookOrigCountry
        End Get
        Set(value As String)
            If (String.Equals(Me._BookOrigCountry, value) = False) Then
                Me._BookOrigCountry = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookOrigZip", DbType:="NVarChar(20)")>
    Public Property BookOrigZip() As String
        Get
            Return Me._BookOrigZip
        End Get
        Set(value As String)
            If (String.Equals(Me._BookOrigZip, value) = False) Then
                Me._BookOrigZip = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookOrigPhone", DbType:="NVarChar(20)")>
    Public Property BookOrigPhone() As String
        Get
            Return Me._BookOrigPhone
        End Get
        Set(value As String)
            If (String.Equals(Me._BookOrigPhone, value) = False) Then
                Me._BookOrigPhone = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookOrigIDENTIFICATIONCODEQUALIFIER", DbType:="NVarChar(2)")>
    Public Property BookOrigIDENTIFICATIONCODEQUALIFIER() As String
        Get
            Return Me._BookOrigIDENTIFICATIONCODEQUALIFIER
        End Get
        Set(value As String)
            If (String.Equals(Me._BookOrigIDENTIFICATIONCODEQUALIFIER, value) = False) Then
                Me._BookOrigIDENTIFICATIONCODEQUALIFIER = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookOrigCompanyNumber", DbType:="NVarChar(50)")>
    Public Property BookOrigCompanyNumber() As String
        Get
            Return Me._BookOrigCompanyNumber
        End Get
        Set(value As String)
            If (String.Equals(Me._BookOrigCompanyNumber, value) = False) Then
                Me._BookOrigCompanyNumber = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookDestName", DbType:="NVarChar(4000)")>
    Public Property BookDestName() As String
        Get
            Return Me._BookDestName
        End Get
        Set(value As String)
            If (String.Equals(Me._BookDestName, value) = False) Then
                Me._BookDestName = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookDestAddress1", DbType:="NVarChar(4000)")>
    Public Property BookDestAddress1() As String
        Get
            Return Me._BookDestAddress1
        End Get
        Set(value As String)
            If (String.Equals(Me._BookDestAddress1, value) = False) Then
                Me._BookDestAddress1 = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookDestAddress2", DbType:="NVarChar(4000)")>
    Public Property BookDestAddress2() As String
        Get
            Return Me._BookDestAddress2
        End Get
        Set(value As String)
            If (String.Equals(Me._BookDestAddress2, value) = False) Then
                Me._BookDestAddress2 = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookDestAddress3", DbType:="NVarChar(4000)")>
    Public Property BookDestAddress3() As String
        Get
            Return Me._BookDestAddress3
        End Get
        Set(value As String)
            If (String.Equals(Me._BookDestAddress3, value) = False) Then
                Me._BookDestAddress3 = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookDestCity", DbType:="NVarChar(25)")>
    Public Property BookDestCity() As String
        Get
            Return Me._BookDestCity
        End Get
        Set(value As String)
            If (String.Equals(Me._BookDestCity, value) = False) Then
                Me._BookDestCity = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookDestState", DbType:="NVarChar(2)")>
    Public Property BookDestState() As String
        Get
            Return Me._BookDestState
        End Get
        Set(value As String)
            If (String.Equals(Me._BookDestState, value) = False) Then
                Me._BookDestState = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookDestCountry", DbType:="NVarChar(30)")>
    Public Property BookDestCountry() As String
        Get
            Return Me._BookDestCountry
        End Get
        Set(value As String)
            If (String.Equals(Me._BookDestCountry, value) = False) Then
                Me._BookDestCountry = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookDestZip", DbType:="NVarChar(10)")>
    Public Property BookDestZip() As String
        Get
            Return Me._BookDestZip
        End Get
        Set(value As String)
            If (String.Equals(Me._BookDestZip, value) = False) Then
                Me._BookDestZip = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookDestPhone", DbType:="NVarChar(15)")>
    Public Property BookDestPhone() As String
        Get
            Return Me._BookDestPhone
        End Get
        Set(value As String)
            If (String.Equals(Me._BookDestPhone, value) = False) Then
                Me._BookDestPhone = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookDestIDENTIFICATIONCODEQUALIFIER", DbType:="NVarChar(2)")>
    Public Property BookDestIDENTIFICATIONCODEQUALIFIER() As String
        Get
            Return Me._BookDestIDENTIFICATIONCODEQUALIFIER
        End Get
        Set(value As String)
            If (String.Equals(Me._BookDestIDENTIFICATIONCODEQUALIFIER, value) = False) Then
                Me._BookDestIDENTIFICATIONCODEQUALIFIER = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookDestCompanyNumber", DbType:="NVarChar(50)")>
    Public Property BookDestCompanyNumber() As String
        Get
            Return Me._BookDestCompanyNumber
        End Get
        Set(value As String)
            If (String.Equals(Me._BookDestCompanyNumber, value) = False) Then
                Me._BookDestCompanyNumber = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookLoadPONumber", DbType:="NVarChar(20)")>
    Public Property BookLoadPONumber() As String
        Get
            Return Me._BookLoadPONumber
        End Get
        Set(value As String)
            If (String.Equals(Me._BookLoadPONumber, value) = False) Then
                Me._BookLoadPONumber = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookLoadCom", DbType:="NVarChar(1)")>
    Public Property BookLoadCom() As String
        Get
            Return Me._BookLoadCom
        End Get
        Set(value As String)
            If (String.Equals(Me._BookLoadCom, value) = False) Then
                Me._BookLoadCom = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_CommCodeDescription", DbType:="NVarChar(40) NOT NULL", CanBeNull:=False)>
    Public Property CommCodeDescription() As String
        Get
            Return Me._CommCodeDescription
        End Get
        Set(value As String)
            If (String.Equals(Me._CommCodeDescription, value) = False) Then
                Me._CommCodeDescription = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_LaneComments", DbType:="NVarChar(4000)")>
    Public Property LaneComments() As String
        Get
            Return Me._LaneComments
        End Get
        Set(value As String)
            If (String.Equals(Me._LaneComments, value) = False) Then
                Me._LaneComments = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_LaneOriginAddressUse", DbType:="Bit")>
    Public Property LaneOriginAddressUse() As System.Nullable(Of Boolean)
        Get
            Return Me._LaneOriginAddressUse
        End Get
        Set(value As System.Nullable(Of Boolean))
            If (Me._LaneOriginAddressUse.Equals(value) = False) Then
                Me._LaneOriginAddressUse = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookTrackDate", DbType:="DateTime")>
    Public Property BookTrackDate() As System.Nullable(Of Date)
        Get
            Return Me._BookTrackDate
        End Get
        Set(value As System.Nullable(Of Date))
            If (Me._BookTrackDate.Equals(value) = False) Then
                Me._BookTrackDate = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_CompEDISecurityQual", DbType:="NVarChar(2)")>
    Public Property CompEDISecurityQual() As String
        Get
            Return Me._CompEDISecurityQual
        End Get
        Set(value As String)
            If (String.Equals(Me._CompEDISecurityQual, value) = False) Then
                Me._CompEDISecurityQual = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_CompEDISecurityCode", DbType:="NVarChar(10)")>
    Public Property CompEDISecurityCode() As String
        Get
            Return Me._CompEDISecurityCode
        End Get
        Set(value As String)
            If (String.Equals(Me._CompEDISecurityCode, value) = False) Then
                Me._CompEDISecurityCode = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_CompEDIPartnerQual", DbType:="NVarChar(2)")>
    Public Property CompEDIPartnerQual() As String
        Get
            Return Me._CompEDIPartnerQual
        End Get
        Set(value As String)
            If (String.Equals(Me._CompEDIPartnerQual, value) = False) Then
                Me._CompEDIPartnerQual = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_CompEDIPartnerCode", DbType:="NVarChar(15)")>
    Public Property CompEDIPartnerCode() As String
        Get
            Return Me._CompEDIPartnerCode
        End Get
        Set(value As String)
            If (String.Equals(Me._CompEDIPartnerCode, value) = False) Then
                Me._CompEDIPartnerCode = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_CompEDIEmailNotificationOn", DbType:="Bit")>
    Public Property CompEDIEmailNotificationOn() As System.Nullable(Of Boolean)
        Get
            Return Me._CompEDIEmailNotificationOn
        End Get
        Set(value As System.Nullable(Of Boolean))
            If (Me._CompEDIEmailNotificationOn.Equals(value) = False) Then
                Me._CompEDIEmailNotificationOn = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_CompEDIEmailAddress", DbType:="NVarChar(255)")>
    Public Property CompEDIEmailAddress() As String
        Get
            Return Me._CompEDIEmailAddress
        End Get
        Set(value As String)
            If (String.Equals(Me._CompEDIEmailAddress, value) = False) Then
                Me._CompEDIEmailAddress = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_CompEDIAcknowledgementRequested", DbType:="Bit")>
    Public Property CompEDIAcknowledgementRequested() As System.Nullable(Of Boolean)
        Get
            Return Me._CompEDIAcknowledgementRequested
        End Get
        Set(value As System.Nullable(Of Boolean))
            If (Me._CompEDIAcknowledgementRequested.Equals(value) = False) Then
                Me._CompEDIAcknowledgementRequested = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_CompEDIMethodOfPayment", DbType:="NVarChar(2)")>
    Public Property CompEDIMethodOfPayment() As String
        Get
            Return Me._CompEDIMethodOfPayment
        End Get
        Set(value As String)
            If (String.Equals(Me._CompEDIMethodOfPayment, value) = False) Then
                Me._CompEDIMethodOfPayment = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookRouteConsFlag", DbType:="Bit")>
    Public Property BookRouteConsFlag() As System.Nullable(Of Boolean)
        Get
            Return Me._BookRouteConsFlag
        End Get
        Set(value As System.Nullable(Of Boolean))
            If (Me._BookRouteConsFlag.Equals(value) = False) Then
                Me._BookRouteConsFlag = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookRevTotalCost", DbType:="Money")>
    Public Property BookRevTotalCost() As System.Nullable(Of Decimal)
        Get
            Return Me._BookRevTotalCost
        End Get
        Set(value As System.Nullable(Of Decimal))
            If (Me._BookRevTotalCost.Equals(value) = False) Then
                Me._BookRevTotalCost = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BillToCompName", DbType:="NVarChar(40)")>
    Public Property BillToCompName() As String
        Get
            Return Me._BillToCompName
        End Get
        Set(value As String)
            If (String.Equals(Me._BillToCompName, value) = False) Then
                Me._BillToCompName = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BillToCompNumber", DbType:="NVarChar(50)")>
    Public Property BillToCompNumber() As String
        Get
            Return Me._BillToCompNumber
        End Get
        Set(value As String)
            If (String.Equals(Me._BillToCompNumber, value) = False) Then
                Me._BillToCompNumber = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BillToCompAddress1", DbType:="NVarChar(40)")>
    Public Property BillToCompAddress1() As String
        Get
            Return Me._BillToCompAddress1
        End Get
        Set(value As String)
            If (String.Equals(Me._BillToCompAddress1, value) = False) Then
                Me._BillToCompAddress1 = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BillToCompAddress2", DbType:="NVarChar(40)")>
    Public Property BillToCompAddress2() As String
        Get
            Return Me._BillToCompAddress2
        End Get
        Set(value As String)
            If (String.Equals(Me._BillToCompAddress2, value) = False) Then
                Me._BillToCompAddress2 = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BillToCompCity", DbType:="NVarChar(25)")>
    Public Property BillToCompCity() As String
        Get
            Return Me._BillToCompCity
        End Get
        Set(value As String)
            If (String.Equals(Me._BillToCompCity, value) = False) Then
                Me._BillToCompCity = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BillToCompState", DbType:="NVarChar(8)")>
    Public Property BillToCompState() As String
        Get
            Return Me._BillToCompState
        End Get
        Set(value As String)
            If (String.Equals(Me._BillToCompState, value) = False) Then
                Me._BillToCompState = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BillToCompZip", DbType:="NVarChar(10)")>
    Public Property BillToCompZip() As String
        Get
            Return Me._BillToCompZip
        End Get
        Set(value As String)
            If (String.Equals(Me._BillToCompZip, value) = False) Then
                Me._BillToCompZip = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BillToCompCountry", DbType:="NVarChar(30)")>
    Public Property BillToCompCountry() As String
        Get
            Return Me._BillToCompCountry
        End Get
        Set(value As String)
            If (String.Equals(Me._BillToCompCountry, value) = False) Then
                Me._BillToCompCountry = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_EDICombineOrdersForStops", DbType:="Float")>
    Public Property EDICombineOrdersForStops() As System.Nullable(Of Double)
        Get
            Return Me._EDICombineOrdersForStops
        End Get
        Set(value As System.Nullable(Of Double))
            If (Me._EDICombineOrdersForStops.Equals(value) = False) Then
                Me._EDICombineOrdersForStops = value
            End If
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookCustCompControl", DbType:="Int")>
    Public Property BookCustCompControl() As System.Nullable(Of Integer)
        Get
            Return Me._BookCustCompControl
        End Get
        Set(value As System.Nullable(Of Integer))
            If (Me._BookCustCompControl.Equals(value) = False) Then
                Me._BookCustCompControl = value
            End If
        End Set
    End Property

    Private _BookNotesVisable1 As String
    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookNotesVisable1", DbType:="NVarChar(255)")>
    Public Property BookNotesVisable1() As String
        Get
            Return _BookNotesVisable1
        End Get
        Set(ByVal value As String)
            _BookNotesVisable1 = value
        End Set
    End Property

    Private _BookWhseAuthorizationNo As String
    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookWhseAuthorizationNo", DbType:="NVarChar(20)")>
    Public Property BookWhseAuthorizationNo() As String
        Get
            Return _BookWhseAuthorizationNo
        End Get
        Set(ByVal value As String)
            _BookWhseAuthorizationNo = value
        End Set
    End Property

    <Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_BookSHID", DbType:="NVarChar(50)")>
    Public Property BookSHID() As String
        Get
            Return Me._BookSHID
        End Get
        Set(value As String)
            If (String.Equals(Me._BookSHID, value) = False) Then
                Me._BookSHID = value
            End If
        End Set
    End Property

    Public Shared Function selectDTOData(ByVal source As Object) As EDI204Result
        Dim strSkipObjs As New List(Of String)
        Dim strMsg As String = ""
        Dim oRow As New EDI204Result
        oRow = NDT.CopyMatchingFields(oRow, source, strSkipObjs, strMsg)
        Return oRow
    End Function
End Class

