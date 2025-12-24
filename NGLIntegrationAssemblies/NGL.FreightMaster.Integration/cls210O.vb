Imports Ngl.FreightMaster.Integration.Configuration
Imports System.Data.SqlClient
Imports Ngl.FreightMaster.Integration.FMDataTableAdapters
Imports NDT = Ngl.Core.Utility.DataTransformation
Imports DAL = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects

'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration

<Serializable()> _
Public Class cls210O : Inherits clsUpload


#Region " Class Variables and Properties "

#End Region

#Region " Constructors "



#End Region

#Region " Functions "

    Protected Function getInvoiceEDIString(ByVal e As clsEDI210Invoice, ByVal sSegTerm As String, ByVal alertInfo As AlertInfo) As String
        Dim sEdi As New System.Text.StringBuilder("", 500)
        Dim strMsg As String = ""
        Dim oWCFPar = New Ngl.FreightMaster.Data.WCFParameters() With {.Database = Me.Database, _
                                                                      .DBServer = Me.DBServer, _
                                                                      .WCFAuthCode = "NGLSystem", _
                                                                      .UserName = Me.CreateUser}
        Dim oBatchData As New NGL.FreightMaster.Data.NGLBatchProcessDataProvider(oWCFPar)
        Dim oEDIData As New NGL.FreightMaster.Data.NGLEDIData(oWCFPar)
        Dim oBookTrack As New Ngl.FreightMaster.Data.NGLBookTrackData(oWCFPar)

        If Not validateFields(e, strMsg) Then
            'Send an alert with the strMsg and return null to caller
            oBatchData.executeInsertAlertMessage("AlertCreate210OutboundFailure", alertInfo.CompControl, alertInfo.CompNumber, alertInfo.CarrierControl, alertInfo.CarrierNumber, "EDI 210Out Failed To Generate", strMsg, "", "", "", "", "")
            'oSecData.InsertAlertMessageWithEmail("AlertCreate210OutboundFailure", alertInfo.CompControl, alertInfo.CompNumber, alertInfo.CarrierControl, alertInfo.CarrierNumber, "EDI 210Out Failed To Generate", strMsg, "", "", "", "", "")
            'also set archived = true and statusCode = validation failure for this record
            oEDIData.ArchiveEDI210Out(alertInfo.RefControl, DTO.tblLoadTender.LoadTenderStatusCodeEnum.DataValidationFail, strMsg)
            oBookTrack.CreateRecord(createBookTrack(alertInfo.BookControl, "891", strMsg))
            'Return null so that this 210 does not get sent
            Return ""
        End If

        With e
            sEdi.AppendFormat("ST*{0}*{1}{2}", .ST.ST01, .ST.ST02, sSegTerm)
            sEdi.AppendFormat("B3*{0}*{1}*{2}*{3}*{4}*{5}*{6}*{7}*{8}*{9}*{10}{11}", .B3.B301, .B3.B302, .B3.B303, .B3.B304, .B3.B305, .B3.B306, .B3.B307, .B3.B308, .B3.B309, .B3.B310, .B3.B311, sSegTerm)
            sEdi.AppendFormat("C3*{0}{1}", .C3.C301, sSegTerm)
            If Not .LoopN9 Is Nothing AndAlso .LoopN9.Count > 0 Then
                For Each oN9 In .LoopN9
                    If Not oN9 Is Nothing Then
                        sEdi.AppendFormat("N9*{0}*{1}{2}", oN9.N9.N901, oN9.N9.N902, sSegTerm)
                    End If
                Next
            End If

            sEdi.AppendFormat("G62*{0}*{1}{2}", .G62.G6201, .G62.G6202, sSegTerm)

            sEdi.AppendFormat("R3*{0}*{1}*{2}*{3}{4}", .R3.R301, .R3.R302, .R3.R303, .R3.R304, sSegTerm)
            If Not .Loop100 Is Nothing AndAlso .Loop100.Count > 0 Then
                For Each o100 In .Loop100
                    If Not o100 Is Nothing Then
                        sEdi.AppendFormat("N1*{0}*{1}*{2}*{3}{4}", o100.N1.N101, o100.N1.N102, o100.N1.N103, o100.N1.N104, sSegTerm)
                        If o100.N1.N101 = "SF" OrElse o100.N1.N101 = "ST" Then
                            sEdi.AppendFormat("N3*{0}*{1}{2}", o100.N3.N301, o100.N3.N302, sSegTerm)
                            sEdi.AppendFormat("N4*{0}*{1}*{2}*{3}{4}", o100.N4.N401, o100.N4.N402, o100.N4.N403, o100.N4.N404, sSegTerm)
                        End If
                    End If
                Next
            End If
            If Not .Loop400 Is Nothing AndAlso .Loop400.Count > 0 Then
                For Each o400 In .Loop400
                    If Not o400 Is Nothing Then
                        sEdi.AppendFormat("LX*{0}{1}", o400.LX.LX01, sSegTerm)
                        sEdi.AppendFormat("L5*{0}*{1}*{2}*{3}{4}", o400.L5.L501, o400.L5.L502, o400.L5.L503, o400.L5.L504, sSegTerm)
                        sEdi.AppendFormat("L0*{0}*{1}*{2}*{3}*{4}*{5}*{6}*{7}*{8}{9}", o400.L0.L001, o400.L0.L002, o400.L0.L003, o400.L0.L004, o400.L0.L005, o400.L0.L006, o400.L0.L007, o400.L0.L008, o400.L0.L009, sSegTerm)
                        sEdi.AppendFormat("L1*{0}*{1}*{2}*{3}*{4}*{5}*{6}*{7}{8}", o400.L1.L101, o400.L1.L102, o400.L1.L103, o400.L1.L104, o400.L1.L105, o400.L1.L106, o400.L1.L107, o400.L1.L108, sSegTerm)
                    End If
                Next

            End If

            sEdi.AppendFormat("L3*{0}*{1}*{2}*{3}*{4}{5}", .L3.L301, .L3.L302, .L3.L303, .L3.L304, .L3.L305, sSegTerm)
            sEdi.AppendFormat("SE*{0}*{1}{2}", .SE.SE01, .SE.SE02, sSegTerm)
        End With
        Return sEdi.ToString
    End Function

    Protected Function GetInvoiceObject(ByVal o210 As DTO.tbl210EDI, ByVal o210Fees() As DTO.tbl210EDIFees, ByVal CarrierPartnerQual As String, ByVal CarrierPartnerCode As String, ByVal intSequence As Integer) As clsEDI210Invoice
        Dim oInvoice As New clsEDI210Invoice
        Dim intSegments As Integer = 0
        Try
            Dim SCAC As String = cleanEDI(nz(o210.CarrierSCAC.ToUpper, ""))
            CarrierPartnerQual = clsPalermos210Out.formatCarrierPartnerQual(cleanEDI(nz(CarrierPartnerQual, "02")))
            CarrierPartnerCode = cleanEDI(nz(CarrierPartnerCode.Trim, SCAC))

            With oInvoice
                'ST Segment
                .ST.ST01 = "210"
                .ST.ST02 = intSequence
                intSegments += 1 'increase the segment counter after each segment
                'B3 Segment
                .B3.B301 = clsPalermos210Out.getB301()
                .B3.B302 = cleanEDI(nz(o210.BookProNumber, ""))
                .B3.B303 = cleanEDI(nz(o210.BookCarrOrderNumber, ""))
                .B3.B304 = clsPalermos210Out.getB304()
                .B3.B305 = "L"
                .B3.B306 = nzDate(o210.BookFinARInvoiceDate, "yyyyMMdd", "00000000")
                Dim nzDec As Decimal = 0.0
                .B3.B307 = NDT.formatDecimalToEDICurrency(nz(o210.BookFinARInvoiceAmt, nzDec))
                .B3.B308 = cleanEDI(nz(o210.CorrectionIndicator, ""))
                .B3.B309 = nzDate(o210.BookCarrActDate, "yyyyMMdd", "00000000")
                .B3.B310 = clsPalermos210Out.getB310()
                .B3.B311 = SCAC
                intSegments += 1 'increase the segment counter after each segment
                'C3 Segment
                .C3.C301 = cleanEDI(nz(o210.Currency, ""))
                intSegments += 1 'increase the segment counter after each segment
                'Add the N9 Data
                ReDim .LoopN9(13)
                Dim intRet = getN9PONumbers(o210, .LoopN9)
                .LoopN9(intRet) = New clsEDI210OutLoopN9("4C", clsPalermos210Out.getInboundOutbound(o210.LaneOriginAddressUse))
                intSegments += (intRet + 1) 'increase the segment counter after each segment

                If (o210.BookConsPrefix.Trim).Length > 0 Then
                    .LoopN9(intRet + 1) = New clsEDI210OutLoopN9("QY", cleanEDI(nz(o210.BookConsPrefix, "")))
                    intSegments += 1 'increase the segment counter after each segment
                End If
                'Add the G62 Data
                .G62.G6201 = "86"
                .G62.G6202 = nzDate(o210.BookDateLoad, "yyyyMMdd", "00000000")
                intSegments += 1 'increase the segment counter after each segment
                'R3 Segment
                .R3.R301 = SCAC
                .R3.R302 = clsPalermos210Out.getR302()
                .R3.R304 = clsPalermos210Out.getR304(o210.BookTypeCode)
                intSegments += 1 'increase the segment counter after each segment
                'Add the Loop 100 data
                ReDim .Loop100(3)
                'Parse the location codes from the Lane Number
                Dim origLocationCode = clsPalermos210Out.getOriginLocationCode(o210.LaneNumber)
                Dim destLocationCode = clsPalermos210Out.getDestLocationCode(o210.LaneNumber)
                Dim origLocationQual = clsPalermos210Out.getLocationQualifier(origLocationCode)
                Dim destLocationQual = clsPalermos210Out.getLocationQualifier(destLocationCode)

                Dim CompPartnerQual = cleanEDI(nz(clsPalermos210Out.getCompPartnerQual(o210), ""))
                Dim CompPartnerCode = cleanEDI(nz(clsPalermos210Out.getCompPartnerCode(o210), ""))
                Dim PalemoTPID = clsPalermos210Out.getPalermoTPID()
                Dim PalermoTPIDQ = clsPalermos210Out.getPalermoTPIDQual()

                .Loop100(0) = New clsEDI210OutLoop100(N101:="SE", N102:=o210.CarrierName, N103:=CarrierPartnerQual, N104:=CarrierPartnerCode)
                .Loop100(1) = New clsEDI210OutLoop100(N101:="BY", N102:=o210.CompName, N103:=PalermoTPIDQ, N104:=PalemoTPID)
                .Loop100(2) = New clsEDI210OutLoop100("SF", o210.BookOrigName, origLocationQual, origLocationCode, o210.BookOrigAddress1, o210.BookOrigAddress2, o210.BookOrigCity, o210.BookOrigState, o210.BookOrigZip, o210.BookOrigCountry)
                .Loop100(3) = New clsEDI210OutLoop100("ST", o210.BookDestName, destLocationQual, destLocationCode, o210.BookDestAddress1, o210.BookDestAddress2, o210.BookDestCity, o210.BookDestState, o210.BookDestZip, o210.BookDestCountry)
                intSegments += 8 'We add 8 because the 100 loop has 8 elements


                'Add the Loop 400 data
                Dim feeCt = o210Fees.Count
                Dim feeSum As Decimal = 0
                If Not o210Fees Is Nothing Or o210Fees.Count > 0 Then
                    feeSum = o210Fees.Sum(Function(x) x.FeeCost)
                End If
                Dim lineHaul As Decimal = 0.0
                lineHaul = clsPalermos210Out.getFreightRate(nz(o210.BookRevBilledBFC, lineHaul), feeSum)
                ReDim .Loop400(20)
                .Loop400(0) = New clsEDI210OutLoop400(1, 1, "Freight Rate", "", 1, "", "", o210.BookTotalWgt.ToString, "L", o210.BookTotalPL.ToString, "PLT", 1, NDT.roundDecimalToEDICurrency(lineHaul), "FR", NDT.formatDecimalToEDICurrency(lineHaul), "400")
                intSegments += 4 'increase the segment counter after each segment
                Dim i As Integer = 1
                For Each fee As DTO.tbl210EDIFees In o210Fees
                    .Loop400(i) = New clsEDI210OutLoop400((i + 1).ToString, (i + 1).ToString, cleanEDI(fee.FeeName, ""), "", (i + 1).ToString, 1, "FR", "", "", "", "", (i + 1).ToString, NDT.roundDecimalToEDICurrency(fee.FeeCost), "FR", NDT.formatDecimalToEDICurrency(fee.FeeCost), cleanEDI(fee.EDICode, ""))
                    i += 1
                    intSegments += 4 'increase the segment counter after each segment
                Next

                'L3 Segment
                Dim total = feeSum + lineHaul
                .L3.L301 = Left(o210.BookTotalWgt.ToString, 10)
                .L3.L302 = "G" 'Gross Weight
                .L3.L305 = Left(NDT.formatDecimalToEDICurrency(total), 12)
                intSegments += 1 'increase the segment counter after each segment
                .SE.SE01 = intSegments + 1
                .SE.SE02 = intSequence
            End With
        Catch ex As Exception
            'we currently have not added any special error conditions so just throw the error back to the caller
            Throw
        End Try
        Return oInvoice
    End Function


    Public Function getEDI210OutString(ByRef EDIString As String, _
                                             ByVal strConnection As String, _
                                             ByVal FileName As String, _
                                             Optional ByVal AutoConfirm As Boolean = False) As ProcessDataReturnValues
        'Dim oISABLL As New clsEDIISABLL
        'Dim oGSBLL As New clsEDIGSBLL
        Dim oCarrierEDIBLL As New clsCarrierEDIBLL
        Dim oISA As New clsEDIISA
        Dim oIEA As New clsEDIIEA
        Dim oGS As New clsEDIGS
        Dim oGE As New clsEDIGE
        Dim strDate As String = Date.Now.ToString("yyyyMMdd")
        Dim strTime As String = Date.Now.ToString("HHmm")
        Dim edi210Str As String = ""


        Dim enmRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strTitle As String = ""
        Dim intErrors As Integer = 0
        Dim intCount As Integer = 0

        GroupEmailMsg = ""
        ITEmailMsg = ""
        Me.ImportTypeKey = IntegrationTypes.EDI210 ' WHAT IS THIS? FIND OUT
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

            Dim intGroupNumber As Integer = 1
            'share the current settings including the db connection
            oCarrierEDIBLL.shareSettings(Me)

            'read the 210 data            

            Dim oWCFParameters As New DAL.WCFParameters
            With oWCFParameters
                .UserName = ""
                .Database = Me.Database
                .DBServer = Me.DBServer
                .ConnectionString = strConnection
                .WCFAuthCode = "NGLSystem"
                .ValidateAccess = False
            End With

            'Get the 210 data
            Dim oEDIData As New DAL.NGLEDIData(oWCFParameters)
            Dim oBookTrack As New DAL.NGLBookTrackData(oWCFParameters)
            Dim o210() As DTO.tbl210EDI = oEDIData.GetEDI210OutboundData()

            Dim intCarrierControl As Integer = 0
            Dim intSegCounter As Integer = 0
            Dim blnFirstTime As Boolean = True

            If Not o210 Is Nothing AndAlso o210.Count > 0 Then
                intCarrierControl = o210(0).CarrierControl
                If Not oCarrierEDIBLL.fillEDIObjects210(oISA, oGS, intCarrierControl, "888") Then
                    LastError = "Cannot read Carrier EDI Settings. " & oCarrierEDIBLL.LastError
                    Return ProcessDataReturnValues.nglDataIntegrationFailure
                End If

                'process 210s one record at a time
                For intRow As Integer = 0 To o210.Count - 1
                    Try
                        Dim oRow = o210(intRow)
                        'get the fees for this EDI210 object
                        Dim o210Fees() As DTO.tbl210EDIFees = oEDIData.GetEDI210OutboundFeeData(oRow.EDI210Control)

                        'check the sender ID (on hold for now every 204 gets a unique ISA and GS record we use the blnFirstTime flag instead to determine if we need to create the ISE and GE record)
                        'If oISA.ISA06.Trim.Length > 1 AndAlso oISA.ISA06 <> nz(oRow.CompEDIPartnerCode, "") Then
                        If Not blnFirstTime Then
                            'Close the GS with a GE
                            With oGE
                                .GE01 = intSegCounter.ToString
                                .GE02 = oGS.GS06
                                If Not String.IsNullOrWhiteSpace(edi210Str) Then
                                    edi210Str &= .getEDIString(oISA.SegmentTerminator)
                                    oGS.GS06 = oCarrierEDIBLL.getNextGSSequence(oGS)
                                End If
                            End With
                            'close the ISA with and ISE
                            With oIEA
                                .IEA01 = 1
                                .IEA02 = oISA.ISA13
                                If Not String.IsNullOrWhiteSpace(edi210Str) Then
                                    edi210Str &= .getEDIString(oISA.SegmentTerminator)
                                    oISA.ISA13 = oCarrierEDIBLL.getNextISASequence(oISA)
                                End If
                            End With
                            If Not String.IsNullOrWhiteSpace(edi210Str) Then EDIString &= edi210Str
                        End If
                        edi210Str = ""
                        'For now each record gets its own ISA record
                        'ElseIf oISA.ISA06.Trim.Length < 1 Then
                        blnFirstTime = False
                        'update the ISA and GS data for this trading partner
                        Dim CompPartnerQual = cleanEDI(nz(clsPalermos210Out.getCompPartnerQual(oRow), ""))
                        Dim CompPartnerCode = cleanEDI(nz(clsPalermos210Out.getCompPartnerCode(oRow), ""))
                        With oISA
                            .ISA03 = nz(oRow.CompEDISecurityQual, "00")
                            .ISA04 = nz(oRow.CompEDISecurityCode, "")
                            .ISA07 = CompPartnerQual
                            .ISA08 = CompPartnerCode

                            edi210Str &= .getEDIString(oISA.SegmentTerminator)
                        End With
                        With oGS
                            .GS03 = (oISA.ISA08).Trim
                            edi210Str &= .getEDIString(oISA.SegmentTerminator)
                        End With
                        'for now we only have one GS segment 
                        intSegCounter = 1
                        Dim strLoadNumber As String = Left(nz(oRow.BookProNumber, ""), 30)

                        Dim alertInfo As New AlertInfo
                        alertInfo.CarrierControl = oRow.CarrierControl
                        alertInfo.CarrierNumber = oRow.CarrierNumber
                        alertInfo.CompControl = oRow.CompControl
                        alertInfo.CompNumber = oRow.CompNumber
                        alertInfo.RefControl = oRow.EDI210Control
                        alertInfo.BookControl = oRow.BookControl

                        Dim sNewEDI = getInvoiceEDIString(GetInvoiceObject(oRow, o210Fees, oISA.ISA05, oISA.ISA06, intSegCounter), oISA.SegmentTerminator, alertInfo)
                        'If the return value is null there is nothing to send (null because validation fails)
                        If Not String.IsNullOrWhiteSpace(sNewEDI) Then
                            edi210Str &= sNewEDI

                            Dim XAction As String = ""
                            Select Case oRow.CorrectionIndicator
                                Case "CO"
                                    XAction = "889"
                                Case "CA"
                                    XAction = "890"
                                Case Else
                                    XAction = "888"
                            End Select

                            saveEDITransaction(nz(oRow.BookControl, 0), _
                                                                      intCarrierControl, _
                                                                      0, _
                                                                      XAction, _
                                                                      oISA.ISA06, _
                                                                      oISA.ISA08, _
                                                                      oISA.ISA13, _
                                                                      oGS.GS06, _
                                                                      nz(oRow.CarrierSCAC.ToUpper, ""), _
                                                                      strLoadNumber, _
                                                                      "Get Invoice EDI 210 String", _
                                                                      "cls210O.getEDI210OutString",
                                                                      intRefControl:=alertInfo.RefControl)
                            oEDIData.InsertFileNameTo210Table(oRow.EDI210Control, FileName210:=FileName)
                        Else
                            edi210Str = ""
                        End If

                    Catch ex As Exception
                        'add unexpected exceptions to the group and it email msgs
                        GroupEmailMsg &= ex.ToString()
                        ITEmailMsg &= ex.ToString()
                    End Try


PROCESS_NEXT:

                Next
                If Not blnFirstTime Then
                    'Close the GS with a GE
                    With oGE
                        .GE01 = intSegCounter.ToString
                        .GE02 = oGS.GS06
                        If Not String.IsNullOrWhiteSpace(edi210Str) Then
                            edi210Str &= .getEDIString(oISA.SegmentTerminator)
                        End If
                    End With
                    'close the ISA with and ISE
                    With oIEA
                        .IEA01 = 1
                        .IEA02 = oISA.ISA13
                        If Not String.IsNullOrWhiteSpace(edi210Str) Then
                            edi210Str &= .getEDIString(oISA.SegmentTerminator)
                        End If
                    End With

                End If
                If Not String.IsNullOrWhiteSpace(edi210Str) Then EDIString &= edi210Str

            End If
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Read EDI 210 Data Warning", "The following errors or warnings were reported some 210 records may not have been transmitted correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Read EDI 210 Data Failure", "The following errors or warnings were reported some 210 records may not have been transmitted correctly." & ITEmailMsg, AdminEmail)
            End If
            Return ProcessDataReturnValues.nglDataIntegrationComplete
        Catch ex As Exception
            LogException("Get Invoice EDI 210 Strings Failure", "Could not read 210 invoice records.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsEDI210O.getEDI210OutStrings Failure")
        Finally
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try

        Return enmRet
    End Function

    Public Function getN9PONumbers(ByVal o210 As DTO.tbl210EDI, ByRef LoopN9() As clsEDI210OutLoopN9) As Integer
        Dim intRet As Integer = 0
        LoopN9(intRet) = New clsEDI210OutLoopN9("PO", cleanEDI(nz(o210.BookLoadPONumber, "")))
        intRet += 1
        If o210.BookLoadPONumber2.Trim.Length > 0 Then
            'ReDim LoopN9(13)
            LoopN9(intRet) = New clsEDI210OutLoopN9("PO", o210.BookLoadPONumber2)
            intRet += 1
            If o210.BookLoadPONumber3.Trim.Length > 0 Then
                LoopN9(intRet) = New clsEDI210OutLoopN9("PO", o210.BookLoadPONumber3)
                intRet += 1
                If o210.BookLoadPONumber4.Trim.Length > 0 Then
                    LoopN9(intRet) = New clsEDI210OutLoopN9("PO", o210.BookLoadPONumber4)
                    intRet += 1
                    If o210.BookLoadPONumber5.Trim.Length > 0 Then
                        LoopN9(intRet) = New clsEDI210OutLoopN9("PO", o210.BookLoadPONumber5)
                        intRet += 1
                        If o210.BookLoadPONumber6.Trim.Length > 0 Then
                            LoopN9(intRet) = New clsEDI210OutLoopN9("PO", o210.BookLoadPONumber6)
                            intRet += 1
                            If o210.BookLoadPONumber7.Trim.Length > 0 Then
                                LoopN9(intRet) = New clsEDI210OutLoopN9("PO", o210.BookLoadPONumber7)
                                intRet += 1
                                If o210.BookLoadPONumber8.Trim.Length > 0 Then
                                    LoopN9(intRet) = New clsEDI210OutLoopN9("PO", o210.BookLoadPONumber8)
                                    intRet += 1
                                    If o210.BookLoadPONumber9.Trim.Length > 0 Then
                                        LoopN9(intRet) = New clsEDI210OutLoopN9("PO", o210.BookLoadPONumber9)
                                        intRet += 1
                                        If o210.BookLoadPONumber10.Trim.Length > 0 Then
                                            LoopN9(intRet) = New clsEDI210OutLoopN9("PO", o210.BookLoadPONumber10)
                                            intRet += 1
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
        Return intRet
    End Function

    Public Function validateFields(ByVal oInvoice As clsEDI210Invoice, ByRef strMsg As String) As Boolean
        strMsg = ""
        Dim prefix = oInvoice.B3.B302.Trim + ": EDI 210Out failed because the following required fields were null: " + vbCrLf
        Dim blnRet As Boolean = True

        With oInvoice
            If String.IsNullOrEmpty(.B3.B302.Trim) Then
                strMsg += "B302 BookProNumber" + vbCrLf
            End If
            If String.IsNullOrEmpty(.B3.B303.Trim) Then
                strMsg += "B303 BookCarrOrderNumber" + vbCrLf
            End If
            If String.IsNullOrEmpty(.B3.B306.Trim) Or .B3.B306.Trim = "00000000" Then
                strMsg += "B306 BookFinARInvoiceDate" + vbCrLf
            End If
            If String.IsNullOrEmpty(.B3.B307.Trim) Or .B3.B307.Trim = "0" Then
                strMsg += "B307 BookFinARInvoiceAmt" + vbCrLf
            End If
            If String.IsNullOrEmpty(.B3.B309.Trim) Or .B3.B309.Trim = "00000000" Then
                strMsg += "B309 BookCarrActDate (Delivered Date)" + vbCrLf
            End If
            If String.IsNullOrEmpty(.B3.B311.Trim) Then
                strMsg += " B311 CarrierSCAC" + vbCrLf
            End If
            If String.IsNullOrEmpty(.C3.C301.Trim) Then
                strMsg += "C301 BookFinARCurType" + vbCrLf
            End If

            If Not .LoopN9 Is Nothing AndAlso .LoopN9.Length > 0 Then
                For Each oN9 In .LoopN9
                    If Not oN9 Is Nothing Then
                        Select Case (oN9.N9.N901.Trim)
                            Case "PO"
                                If String.IsNullOrEmpty(oN9.N9.N902.Trim) Then
                                    strMsg += "N902 BookLoadPONumber" + vbCrLf
                                End If
                            Case "4C"
                                If String.IsNullOrEmpty(oN9.N9.N902.Trim) Then
                                    strMsg += "N902 LaneOriginAddressUse" + vbCrLf
                                End If
                            Case "QY"
                                If String.IsNullOrEmpty(oN9.N9.N902.Trim) Then
                                    strMsg += "N902 BookConsPrefix" + vbCrLf
                                End If
                        End Select
                    End If
                Next
            End If
            If String.IsNullOrEmpty(.G62.G6202.Trim) Or .G62.G6202.Trim = "00000000" Then
                strMsg += "G6202 BookDateLoad" + vbCrLf
            End If
            If String.IsNullOrEmpty(.R3.R301.Trim) Then
                strMsg += "R301 CarrierSCAC" + vbCrLf
            End If
            If String.IsNullOrEmpty(.R3.R304.Trim) Then
                strMsg += "R304 BookTypeCode" + vbCrLf
            End If
            If Not .Loop100 Is Nothing AndAlso .Loop100.Length > 0 Then
                For Each o100 In .Loop100
                    If Not o100 Is Nothing Then
                        Select Case (o100.N1.N101.Trim)
                            Case "SF"
                                If String.IsNullOrEmpty(o100.N1.N102.Trim) Then strMsg += "N102 BookOrigName" + vbCrLf
                                If String.IsNullOrEmpty(o100.N4.N401.Trim) Then strMsg += "N401 BookOrigCity" + vbCrLf
                                If String.IsNullOrEmpty(o100.N4.N402.Trim) Then strMsg += "N402 BookOrigState" + vbCrLf
                                If String.IsNullOrEmpty(o100.N4.N404.Trim) Then strMsg += "N404 BookOrigCountry" + vbCrLf
                            Case "ST"
                                If String.IsNullOrEmpty(o100.N1.N102.Trim) Then strMsg += "N102 BookDestName" + vbCrLf
                                If String.IsNullOrEmpty(o100.N4.N401.Trim) Then strMsg += "N401 BookDestCity" + vbCrLf
                                If String.IsNullOrEmpty(o100.N4.N402.Trim) Then strMsg += "N402 BookDestState" + vbCrLf
                                If String.IsNullOrEmpty(o100.N4.N404.Trim) Then strMsg += "N404 BookDestCountry" + vbCrLf
                            Case "SE"
                                If String.IsNullOrEmpty(o100.N1.N102.Trim) Then strMsg += "N102 CarrierName" + vbCrLf
                                If String.IsNullOrEmpty(o100.N1.N103.Trim) Then strMsg += "N103 CarrierEDIPartnerQual" + vbCrLf
                                If String.IsNullOrEmpty(o100.N1.N104.Trim) Then strMsg += "N104 CarrierEDIPartnerCode" + vbCrLf
                            Case "BY"
                                If String.IsNullOrEmpty(o100.N1.N102.Trim) Then strMsg += "N102 CompName" + vbCrLf
                        End Select
                    End If
                Next
            End If
            If Not .Loop400 Is Nothing AndAlso .Loop400.Length > 0 Then
                For Each o400 In .Loop400
                    If Not o400 Is Nothing Then
                        Select Case (o400.L0.L002.Trim)
                            Case ""
                                'This is the line haul
                                If String.IsNullOrEmpty(o400.L5.L502.Trim) Then strMsg += "L502 Should say Freight Rate" + vbCrLf
                                If Not String.IsNullOrEmpty(o400.L0.L003.Trim) Then strMsg += "L003 Should be blank for line haul" + vbCrLf
                                If String.IsNullOrEmpty(o400.L0.L004.Trim) Then strMsg += "L004 BookTotalWgt" + vbCrLf
                                If String.IsNullOrEmpty(o400.L0.L008.Trim) Then strMsg += "L008 BookTotalPL" + vbCrLf
                                If String.IsNullOrEmpty(o400.L1.L102) Then strMsg += "L102 Line Haul. Freight Rate formula = BookRevBilledBFC - feeSum" + vbCrLf
                                If String.IsNullOrEmpty(o400.L1.L104) Then strMsg += "L104 Line Haul. Freight Rate formula = BookRevBilledBFC - feeSum" + vbCrLf
                                If String.IsNullOrEmpty(o400.L1.L108) Then strMsg += "L108 Should be 400 for Line Haul" + vbCrLf
                            Case "1"
                                'This is an accessorial
                                If String.IsNullOrEmpty(o400.L5.L502.Trim) Then strMsg += "L502 FeeName" + vbCrLf
                                If String.IsNullOrEmpty(o400.L0.L003.Trim) Then strMsg += "L003 Should be FR for Accessorial" + vbCrLf
                                If Not String.IsNullOrEmpty(o400.L0.L004.Trim) Then strMsg += "L004 Should be blank for Accessorial. Current value = " + o400.L0.L004 + vbCrLf
                                If Not String.IsNullOrEmpty(o400.L0.L008.Trim) Then strMsg += "L008 Should be blank for Accessorial. Current value = " + o400.L0.L008 + vbCrLf
                                If String.IsNullOrEmpty(o400.L1.L102) Then strMsg += "L102 Fee Cost" + vbCrLf
                                If String.IsNullOrEmpty(o400.L1.L104) Then strMsg += "L104 Fee Cost" + vbCrLf
                                If String.IsNullOrEmpty(o400.L1.L108) Then strMsg += "L108 EDI Code" + vbCrLf
                            Case Else
                                strMsg += "L002 Error: must either be empty string or 1"
                        End Select
                    End If
                Next
            End If
            If String.IsNullOrEmpty(.L3.L301.Trim) Then
                strMsg += "L301 BookTotalWgt" + vbCrLf
            End If
            If String.IsNullOrEmpty(.L3.L305.Trim) Then
                strMsg += "L305 total = feeSum + lineHaul" + vbCrLf
            End If

        End With

        If strMsg.Trim.Length > 0 Then
            blnRet = False
            strMsg = prefix + strMsg
        End If
        Return blnRet

    End Function

    Public Function createBookTrack(ByVal BookControl As Integer, Optional ByVal lXAction As String = "", Optional ByVal strMsg As String = "") As DTO.BookTrack
        Dim lsControl As Integer = 0
        Dim lsDesc As String = ""


        If lXAction.Trim.Length > 0 Then
            getLoadStatusInfo(lsControl, lsDesc, lXAction)
        End If

        If strMsg.Trim.Length > 0 Then lsDesc = strMsg

        Dim bt As New DTO.BookTrack
        With bt
            .BookTrackBookControl = BookControl
            .BookTrackComment = lsDesc
            .BookTrackContact = "EDI"
            .BookTrackDate = Date.Now
            .BookTrackStatus = lsControl
        End With
        Return bt
    End Function

    Public Function getLoadStatusInfo(ByRef LoadStatusControl As Integer, ByRef LoadStatusDesc As String, ByVal XAction As String) As Boolean
        Dim blnRetVal As Boolean = True

        Dim prefix = "NGL.FreightMaster.Integration.cls210O.getLoadStatusInfo() Failed: "
        Dim logMsg As String = ""
        Dim strSQL As String = "Select top 1 LoadStatusControl From dbo.LoadStatusCodes Where LoadStatusCode = '" & XAction & "' Order by LoadStatusControl Desc"
        Dim strSQL2 As String = "Select top 1 LoadStatusDesc From dbo.LoadStatusCodes Where LoadStatusCode = '" & XAction & "' Order by LoadStatusControl Desc"

        Try
            Dim oQuery As New Ngl.Core.Data.Query(DBServer, Database)
            If Not Integer.TryParse(oQuery.getScalarValue(DBCon, strSQL, 1), LoadStatusControl) Then
                blnRetVal = False
                logMsg = prefix & "The following query did not return a valid LoadStatusControl from the LoadStatusCodes table: " & strSQL
                Log(logMsg)
            End If
            LoadStatusDesc = oQuery.getScalarValue(DBCon, strSQL2, 1)
            If Not LoadStatusDesc.Trim.Length > 0 Then
                blnRetVal = False
                logMsg = prefix & "The following query did not return a valid LoadStatusDesc from the LoadStatusCodes table: " & strSQL
                Log(logMsg)
            End If
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Log(prefix & "Failed to update the load confirmation status: " & ex.Message)
            Return False
        Catch ex As Ngl.Core.DatabaseLogInException
            Log(prefix & "Database login failure: " & ex.Message)
            Return False
        Catch ex As Ngl.Core.DatabaseInvalidException
            Log(prefix & "Database access failure : " & ex.Message)
            Return False
        Catch ex As Ngl.Core.DatabaseDataValidationException
            Log(prefix & ex.Message)
            Return False
        Catch ex As Exception
            Throw
            Return False
        End Try

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



#End Region



End Class

Public Class AlertInfo
    Public CompControl As Integer = 0
    Public CompNumber As Integer = 0
    Public CarrierControl As Integer = 0
    Public CarrierNumber As Integer = 0
    Public RefControl As Integer = 0
    Public BookControl As Integer = 0
End Class
