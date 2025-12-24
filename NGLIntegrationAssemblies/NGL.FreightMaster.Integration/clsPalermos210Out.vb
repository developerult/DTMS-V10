Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects

Module clsPalermos210Out

    Public Function getB301() As String
        Dim B301 As String = ""
        'If some condition, set B301 = "R" for supplimental invoice
        'Else send blank
        Return B301
    End Function

    Public Function getB304() As String
        Return "TP"
    End Function

    Public Function getR302() As String
        Return "B"
    End Function

    Public Function getR304(ByVal BookTypeCode As String) As String
        Dim R304 As String = ""
        If BookTypeCode = "Traffic-TL" Then
            R304 = "TL"
        Else
            R304 = "LT"
        End If
        Return R304
    End Function

    Public Function getB310() As String
        Return "035"
    End Function

    Public Function getOriginLocationCode(ByVal LaneNumber As String) As String
        Dim locationCode As String = ""
        Dim sElems() As String = LaneNumber.Split("-")
        If sElems.Count >= 4 Then
            locationCode = sElems(3)
        End If
        Return locationCode
    End Function

    Public Function getDestLocationCode(ByVal LaneNumber As String) As String
        Dim locationCode As String = ""
        Dim sElems() As String = LaneNumber.Split("-")
        If sElems.Count >= 4 Then
            locationCode = sElems(2)
        End If
        Return locationCode
    End Function

    Public Function getLocationQualifier(ByVal locationCode As String) As String
        'If the location code exists set the qualifier to FA, otherwise send blank
        Dim qual As String = ""
        If locationCode.Trim.Length > 0 Then
            qual = "FA"
        End If
        Return qual
    End Function

    Public Function getInboundOutbound(ByVal LaneOriginAddressUse As Boolean) As String
        If LaneOriginAddressUse = False Then
            Return "O"
        Else
            Return "I"
        End If
    End Function

    Public Function getFreightRate(ByVal BookRevBilledBFC As Decimal, ByVal feeSum As Decimal) As Decimal
        Dim FR = BookRevBilledBFC - feeSum
        Return FR
    End Function

    Public Function getCompPartnerQual(ByVal o210 As DTO.tbl210EDI) As String
        'Use the company level parameters first -- if those are null then use CompEDI settings
        Dim strRet As String = ""
        If Not o210.ParamCompEDIPartnerQual Is Nothing And o210.ParamCompEDIPartnerQual.Length > 0 Then
            strRet = o210.ParamCompEDIPartnerQual
        Else
            strRet = o210.CompEDIPartnerQual
        End If
        Return strRet
    End Function

    Public Function getCompPartnerCode(ByVal o210 As DTO.tbl210EDI) As String
        'Use the company level parameters first -- if those are null then use CompEDI settings
        Dim strRet As String = ""
        If Not o210.ParamCompEDIPartnerCode Is Nothing And o210.ParamCompEDIPartnerCode.Length > 0 Then
            strRet = (o210.ParamCompEDIPartnerCode).Trim
        Else
            strRet = (o210.CompEDIPartnerCode).Trim
        End If
        Return strRet
    End Function

    Public Function formatCarrierPartnerQual(ByVal PQ As String) As String
        Dim strRet As String = ""
        If PQ = "02" Then
            strRet = "2"
        Else
            strRet = PQ
        End If
        Return strRet
    End Function

    Public Function getPalermoTPID() As String
        Return "PALERUSD"
    End Function

    Public Function getPalermoTPIDQual() As String
        Return "ZZ"
    End Function


End Module
