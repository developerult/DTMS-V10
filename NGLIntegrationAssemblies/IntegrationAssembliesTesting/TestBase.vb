Imports System
Imports Tar = NGL.FM.CarTar
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL
Imports LTS = NGL.FreightMaster.Data.LTS
Imports NGL.FreightMaster.Integration

Public Class TestBase


    Private _testParameters As DAL.WCFParameters
    Public Property testParameters() As DAL.WCFParameters
        Get
            If _testParameters Is Nothing Then
                _testParameters = New DAL.WCFParameters() With {.Database = "NGLMASPROD",
                                                               .DBServer = "NGLRDP07D",
                                                               .WCFAuthCode = "WCFDEV",
                                                                .UserName = "NGL\Lauren Van Vleet"}
            End If
            Return _testParameters
        End Get
        Set(ByVal value As DAL.WCFParameters)
            _testParameters = value
        End Set
    End Property

    Private _WSUrl As String = "http://localhost:9797/"
    Public Property WSUrl() As String
        Get
            Return _WSUrl
        End Get
        Set(ByVal value As String)
            _WSUrl = value
        End Set
    End Property

    Private _WSAuthCode As String = "WSUT"
    Public Property WSAuthCode() As String
        Get
            Return _WSAuthCode
        End Get
        Set(ByVal value As String)
            _WSAuthCode = value
        End Set
    End Property

    Enum WebServiceReturnValues
        nglDataIntegrationComplete
        nglDataConnectionFailure
        nglDataValidationFailure
        nglDataIntegrationFailure
        nglDataIntegrationHadErrors
    End Enum

    Private _NGLBookData As DAL.NGLBookData
    ''' <summary>
    ''' Local instance of the DAL.NGLBookData class
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLBookData() As DAL.NGLBookData
        Get
            If _NGLBookData Is Nothing Then _NGLBookData = New DAL.NGLBookData(testParameters)
            Return _NGLBookData
        End Get
        Set(value As DAL.NGLBookData)
            _NGLBookData = value
        End Set
    End Property

    Private _NGLBookRevenueData As DAL.NGLBookRevenueData
    ''' <summary>
    ''' Local instance of the DAL.NGLBookRevenueData class
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLBookRevenueData() As DAL.NGLBookRevenueData
        Get
            If _NGLBookRevenueData Is Nothing Then _NGLBookRevenueData = New DAL.NGLBookRevenueData(testParameters)
            Return _NGLBookRevenueData
        End Get
        Set(value As DAL.NGLBookRevenueData)
            _NGLBookRevenueData = value
        End Set
    End Property

    Private _NGLCarrierData As DAL.NGLCarrierData
    ''' <summary>
    ''' Local instance of the DAL.NGLCarrierData class
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLCarrierData() As DAL.NGLCarrierData
        Get
            If _NGLCarrierData Is Nothing Then _NGLCarrierData = New DAL.NGLCarrierData(testParameters)
            Return _NGLCarrierData
        End Get
        Set(value As DAL.NGLCarrierData)
            _NGLCarrierData = value
        End Set
    End Property

    Private _NGLPOHdrData As DAL.NGLPOHdrData
    ''' <summary>
    ''' Local instance of the DAL.NGLPOHdrData class
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLPOHdrData() As DAL.NGLPOHdrData
        Get
            If _NGLPOHdrData Is Nothing Then _NGLPOHdrData = New DAL.NGLPOHdrData(testParameters)
            Return _NGLPOHdrData
        End Get
        Set(value As DAL.NGLPOHdrData)
            _NGLPOHdrData = value
        End Set
    End Property

    Private _NGLCompData As DAL.NGLCompData
    ''' <summary>
    ''' Local instance of the DAL.NGLCompData class
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLCompData() As DAL.NGLCompData
        Get
            If _NGLCompData Is Nothing Then _NGLCompData = New DAL.NGLCompData(testParameters)
            Return _NGLCompData
        End Get
        Set(value As DAL.NGLCompData)
            _NGLCompData = value
        End Set
    End Property

    Private _NGLLaneData As DAL.NGLLaneData
    ''' <summary>
    ''' Local instance of the DAL.NGLLaneData class
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLLaneData() As DAL.NGLLaneData
        Get
            If _NGLLaneData Is Nothing Then _NGLLaneData = New DAL.NGLLaneData(testParameters)
            Return _NGLLaneData
        End Get
        Set(value As DAL.NGLLaneData)
            _NGLLaneData = value
        End Set
    End Property

    Private _NGLCompEDIData As DAL.NGLCompEDIData
    ''' <summary>
    ''' Local instance of the DAL.NGLCompEDIData class
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLCompEDIData() As DAL.NGLCompEDIData
        Get
            If _NGLCompEDIData Is Nothing Then _NGLCompEDIData = New DAL.NGLCompEDIData(testParameters)
            Return _NGLCompEDIData
        End Get
        Set(value As DAL.NGLCompEDIData)
            _NGLCompEDIData = value
        End Set
    End Property

    Private _NGLCarrierEDIData As DAL.NGLCarrierEDIData
    ''' <summary>
    ''' Local instance of the DAL.NGLCarrierEDIData class
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLCarrierEDIData() As DAL.NGLCarrierEDIData
        Get
            If _NGLCarrierEDIData Is Nothing Then _NGLCarrierEDIData = New DAL.NGLCarrierEDIData(testParameters)
            Return _NGLCarrierEDIData
        End Get
        Set(value As DAL.NGLCarrierEDIData)
            _NGLCarrierEDIData = value
        End Set
    End Property

    Private _NGLDynamicsTMSSettingData As DAL.NGLDynamicsTMSSettingData
    ''' <summary>
    ''' Local instance of the DAL.NGLDynamicsTMSSettingData class
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLDynamicsTMSSettingData() As DAL.NGLDynamicsTMSSettingData
        Get
            If _NGLDynamicsTMSSettingData Is Nothing Then _NGLDynamicsTMSSettingData = New DAL.NGLDynamicsTMSSettingData(testParameters)
            Return _NGLDynamicsTMSSettingData
        End Get
        Set(value As DAL.NGLDynamicsTMSSettingData)
            _NGLDynamicsTMSSettingData = value
        End Set
    End Property

    Protected Function getDynamicsTMSSettings(ByVal LegalEntity As String) As DTO.DynamicsTMSSetting

        Dim oDTMSData As DTO.DynamicsTMSSetting
        Try
            oDTMSData = NGLDynamicsTMSSettingData.GetDynamicsTMSSettingFiltered(LegalEntity)
        Catch ex As Exception
            'ignore errors for testing at this level (add error handlers later if needed)
        End Try
        If oDTMSData Is Nothing And Not String.IsNullOrWhiteSpace(LegalEntity) Then
            'create new settings for this Legalentity using default
            Dim NAVDTMSWebServices As String = "http://nglnav2013r2.ngl.local:7147/DynamicsTMS71/WS/CRONUS%20USA,%20Inc./Codeunit/DynamicsTMSWebServices"
            oDTMSData = CreateNAVTMSSettings(LegalEntity, NAVDTMSWebServices, "", "")
        End If
        Return oDTMSData
    End Function

    Protected Function CreateNAVTMSSettings(ByVal sDTMSLegalEntity As String,
                                            ByVal sDTMSNAVWebServiceURL As String,
                                            ByVal sDTMSNAVUserName As String,
                                            ByVal sDTMSNAVPassword As String,
                                            Optional ByVal iDTMSPicklistMaxRetry As Integer = 1,
                                            Optional ByVal iDTMSPicklistRetryMinutes As Integer = 30,
                                            Optional ByVal iDTMSPicklistMaxRowsReturned As Integer = 10,
                                            Optional ByVal bDTMSPicklistAutoConfirmation As Boolean = False,
                                            Optional ByVal iDTMSAPExportMaxRetry As Integer = 5,
                                            Optional ByVal iDTMSAPExportRetryMinutes As Integer = 30,
                                            Optional ByVal iDTMSAPExportMaxRowsReturned As Integer = 10,
                                            Optional ByVal bDTMSAPExportAutoConfirmation As Boolean = False,
                                            Optional ByVal bDTMSNAVUseDefaultCredentials As Boolean = True,
                                            Optional ByVal sDTMSWSAuthCode As String = "NGLWSDEV",
                                            Optional ByVal sDTMSWSURL As String = "http://nglwsdev704.nextgeneration.com",
                                            Optional ByVal sDTMSWCFAuthCode As String = "NGLWCFDEV",
                                            Optional ByVal sDTMSWCFURL As String = "http://nglwcfdev704.nextgeneration.com",
                                            Optional ByVal sDTMSWCFTCPURL As String = "net.tcp://nglwcfdev704.nextgeneration.com:908") As DTO.DynamicsTMSSetting


        Try
            Dim oDTMSData As New DTO.DynamicsTMSSetting With {.DTMSLegalEntity = sDTMSLegalEntity _
                                                             , .DTMSPicklistMaxRetry = iDTMSPicklistMaxRetry _
                                                             , .DTMSPicklistRetryMinutes = iDTMSPicklistRetryMinutes _
                                                             , .DTMSPicklistMaxRowsReturned = iDTMSPicklistMaxRowsReturned _
                                                             , .DTMSPicklistAutoConfirmation = bDTMSPicklistAutoConfirmation _
                                                             , .DTMSAPExportMaxRetry = iDTMSAPExportMaxRetry _
                                                             , .DTMSAPExportRetryMinutes = iDTMSAPExportRetryMinutes _
                                                             , .DTMSAPExportMaxRowsReturned = iDTMSAPExportMaxRowsReturned _
                                                             , .DTMSAPExportAutoConfirmation = bDTMSAPExportAutoConfirmation _
                                                             , .DTMSNAVWebServiceURL = sDTMSNAVWebServiceURL _
                                                             , .DTMSNAVUserName = sDTMSNAVUserName _
                                                             , .DTMSNAVPassword = sDTMSNAVPassword _
                                                             , .DTMSNAVUseDefaultCredentials = bDTMSNAVUseDefaultCredentials _
                                                             , .DTMSWSAuthCode = sDTMSWSAuthCode _
                                                             , .DTMSWSURL = sDTMSWSURL _
                                                             , .DTMSWCFAuthCode = sDTMSWCFAuthCode _
                                                             , .DTMSWCFURL = sDTMSWCFURL _
                                                             , .DTMSWCFTCPURL = sDTMSWCFTCPURL}

            Return NGLDynamicsTMSSettingData.CreateRecord(oDTMSData)
        Catch ex As Exception
            'ignore errors for testing at this level (add error handlers later if needed)
        End Try
        Return Nothing
    End Function

    Protected Function getBookByPro(ByVal sProNumber As String) As DTO.Book
        Try
            Return NGLBookData.GetBookFiltered(BookProNumber:=sProNumber)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getBookRevs(ByVal BookControl As Integer) As DTO.BookRevenue()
        Try
            Return NGLBookRevenueData.GetBookRevenues(BookControl)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getCarrierByName(ByVal sName As String) As DTO.Carrier
        Try
            Return NGLCarrierData.GetCarrierFiltered(Name:=sName)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getCompanyFiltered(Optional ByVal Control As Integer = 0, Optional ByVal Number As Integer = 0, Optional ByVal Name As String = "") As DTO.Comp
        Try
            Return NGLCompData.GetCompFiltered(Control:=Control, Number:=Number, Name:=Name)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function GetLaneFilteredByLaneNumber(ByVal LaneNumber As String) As DTO.Lane
        Try
            Return NGLLaneData.GetLaneFilteredByLaneNumber(LaneNumber:=LaneNumber)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function GetPOHdrFiltered(ByVal PONumber As String) As DTO.POHdr
        Try
            Return NGLPOHdrData.GetPOHdrFiltered(PONumber:=PONumber)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    'Protected Function GetHazmatDetails(ByVal oBase As DAL.NDPBaseClass, ByVal HazRegulation As String, ByVal HazID As String) As DTO.Hazmat()
    '    Dim strSQL As String = ""

    '    strSQL = "Select * From [dbo].[Hazmat] as h Where h.HazRegulation = " & HazRegulation
    '    strSQL &= " And h.HazID = " & HazID

    '    Return oBase.executeSQL(strSQL.ToString())

    '    'Try
    '    '    Dim oHaz As New DAL.NGLLookupDataProvider(testParameters)
    '    '    Return oHaz.GetHazmatDetails(BookItemControl:=ItemControl)
    '    'Catch ex As Exception
    '    '    Return Nothing 'no data is available
    '    'End Try
    'End Function

    Protected Sub deleteCompanyRecord(ByVal compRecord As DTO.Comp)
        Try
            NGLCompData.DeleteRecord(compRecord)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub deleteCarrierRecord(ByVal carrierRecord As DTO.Carrier)
        Try
            NGLCarrierData.DeleteRecord(carrierRecord)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub deleteLaneRecord(ByVal laneRecord As DTO.Lane)
        Try
            NGLLaneData.DeleteRecord(laneRecord)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub deletePOHdrRecord(ByVal POHdrRecord As DTO.POHdr)
        Try
            NGLPOHdrData.DeleteRecord(POHdrRecord)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Protected Function getSystemParameters() As DTO.GlobalTaskParameters
        Dim oGTPs As DTO.GlobalTaskParameters
        Try
            'get the parameter settings from the database.
            Dim oSysData As New DAL.NGLSystemDataProvider(testParameters)
            testParameters.ConnectionString = oSysData.ConnectionString
            oGTPs = oSysData.GetGlobalTaskParameters()
        Catch ex As Exception
            Return Nothing
        End Try
        Return oGTPs
    End Function


    Protected Function readCompEDI(ByVal CompControl As Integer, ByVal EDIXaction As String) As DTO.CompEDI
        Try
            Dim oEDIs = NGLCompEDIData.GetCompEDIsFiltered(CompControl, EDIXaction)
            If Not oEDIs Is Nothing AndAlso oEDIs.Count > 0 Then  Return oEDIs(0)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
        Return Nothing
    End Function

    Protected Function readCarrierEDI(ByVal CarrierControl As Integer, ByVal EDIXaction As String) As DTO.CarrierEDI
        Try
            Dim oEDIs = NGLCarrierEDIData.GetCarrierEDIsFiltered(CarrierControl, EDIXaction)
            If Not oEDIs Is Nothing AndAlso oEDIs.Count > 0 Then
                Return oEDIs(0)
            End If
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
        Return Nothing
    End Function

    Protected Function buildISAString(ByVal oCompEDI As DTO.CompEDI, _
                                      ByVal oCarrEDI As DTO.CarrierEDI, _
                                      ByVal dtSend As Date, _
                                      ByRef SegmentTerminator As String, _
                                      ByRef ISASequence As String, _
                                      Optional ByVal blnInbound As Boolean = True) As String
        Dim oISA As New clsEDIISA
        Dim strRet As String = ""
        If oCompEDI Is Nothing Then Return ""
        If oCarrEDI Is Nothing Then Return ""
        With oISA
            oISA.CarrierEDIControl = oCarrEDI.CarrierEDIControl
            oISA.CompEDIControl = oCompEDI.CompEDIControl
            .ISA01 = If(blnInbound, oCarrEDI.CarrierEDISecurityQual, oCompEDI.CompEDISecurityQual)
            .ISA02 = If(blnInbound, oCarrEDI.CarrierEDISecurityCode, oCompEDI.CompEDISecurityCode)
            .ISA03 = If(blnInbound, oCompEDI.CompEDISecurityQual, oCarrEDI.CarrierEDISecurityQual)
            .ISA04 = If(blnInbound, oCompEDI.CompEDISecurityCode, oCarrEDI.CarrierEDISecurityCode)
            .ISA05 = If(blnInbound, oCarrEDI.CarrierEDIPartnerQual, oCompEDI.CompEDIPartnerQual)
            .ISA06 = If(blnInbound, oCarrEDI.CarrierEDIPartnerCode, oCompEDI.CompEDIPartnerCode)
            .ISA07 = If(blnInbound, oCompEDI.CompEDIPartnerQual, oCarrEDI.CarrierEDIPartnerQual)
            .ISA08 = If(blnInbound, oCompEDI.CompEDIPartnerCode, oCarrEDI.CarrierEDIPartnerCode)
            .ISA09 = dtSend.ToString("yyMMdd")
            .ISA10 = dtSend.ToString("HHmm")
            .ISA11 = "U"
            .ISA12 = "00401"
            .ISA13 = If(blnInbound, (oCompEDI.CompEDIISASequence + 1), (oCarrEDI.CarrierEDIISASequence + 1)).ToString()
            .ISA14 = If(blnInbound, If(oCompEDI.CompEDIAcknowledgementRequested, "1", "0"), If(oCarrEDI.CarrierEDIAcknowledgementRequested, "1", "0"))
            .ISA15 = If(String.IsNullOrWhiteSpace(oCarrEDI.CarrierEDITestCode), "P", oCarrEDI.CarrierEDITestCode)
            SegmentTerminator = oISA.SegmentTerminator
            ISASequence = .ISA13
            strRet = .getEDIString(SegmentTerminator)
        End With
        Return strRet
    End Function

    Protected Function buildGSString(ByVal oCompEDI As DTO.CompEDI, _
                                     ByVal oCarrEDI As DTO.CarrierEDI, _
                                     ByVal dtSend As Date, _
                                     ByVal EDIXaction As String, _
                                     ByVal SegmentTerminator As String, _
                                     ByRef GSSequence As String, _
                                     Optional ByVal blnInbound As Boolean = True) As String
        Dim oGS As New clsEDIGS
        Dim strRet As String = ""
        If oCompEDI Is Nothing Then Return ""
        If oCarrEDI Is Nothing Then Return ""
        With oGS
            .CarrierEDIControl = oCarrEDI.CarrierEDIControl
            .CompEDIControl = oCompEDI.CompEDIControl
            'we have included all the supported functional groups but only the 204,210 and 997 are used by this functon
            Select Case EDIXaction
                Case "204"
                    .GS01 = "SM"
                Case "210"
                    .GS01 = "IM"
                Case "997"
                    .GS01 = "FA"
                Case "214"
                    .GS01 = "QM"
                Case "990"
                    .GS01 = "GF"
            End Select
            .GS02 = If(blnInbound, oCarrEDI.CarrierEDIPartnerCode, oCompEDI.CompEDIPartnerCode)
            .GS03 = If(blnInbound, oCompEDI.CompEDIPartnerCode, oCarrEDI.CarrierEDIPartnerCode)
            .GS04 = dtSend.ToString("yyyyMMdd")
            .GS05 = dtSend.ToString("HHmm")
            .GS06 = If(blnInbound, (oCompEDI.CompEDISequence + 1), (oCarrEDI.CarrierEDIGSSequence + 1)).ToString()
            GSSequence = .GS06
            .GS07 = "X"
            .GS08 = "004010"
            strRet = .getEDIString(SegmentTerminator)
        End With
        Return strRet

    End Function

    Protected Function buildIEAString(ByVal intISACounter As Integer, ByVal ISASequence As String, ByVal SegmentTerminator As String)

        Dim oIEA As New clsEDIIEA
        Dim strRet As String
        With oIEA
            .IEA01 = intISACounter
            .IEA02 = ISASequence
            strRet = .getEDIString(SegmentTerminator)
        End With
        Return strRet
    End Function

    Protected Function buildGEString(ByVal intSegCounter As Integer, ByVal GSSequence As String, ByVal SegmentTerminator As String) As String

        Dim oGE As New clsEDIGE
        Dim strRet As String
        With oGE
            .GE01 = intSegCounter.ToString()
            .GE02 = GSSequence
            strRet = .getEDIString(SegmentTerminator)
        End With
        Return strRet
    End Function

    ''' <summary>
    ''' Mass updates the ImportFlag field for all records in dbo.tblImportFields to either 0 = false or 1 = true
    ''' </summary>
    ''' <param name="oBase"></param>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Public Sub setAllowUpdateFlags(ByVal oBase As DAL.NDPBaseClass, ByVal value As Integer)
        Dim strSQL As String = ""

        strSQL = " Update dbo.tblImportFields "
        strSQL &= " Set ImportFlag = " & value

        oBase.executeSQL(strSQL.ToString())

    End Sub

    ''' <summary>
    ''' Restores the data in dbo.tblImportFields back to the original values
    ''' </summary>
    ''' <param name="oBase"></param>
    ''' <remarks></remarks>
    Public Sub restoretblImportFields(ByVal oBase As DAL.NDPBaseClass)
        Dim strSQL As String = ""

        strSQL = "Delete from dbo.tblImportFields "
        strSQL &= "SET IDENTITY_INSERT [dbo].[tblImportFields] ON "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (1, N'CarrierNumber', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (2, N'CarrierName', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (3, N'CarrierStreetAddress1', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (4, N'CarrierStreetAddress2', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (5, N'CarrierStreetAddress3', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (6, N'CarrierStreetCity', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (7, N'CarrierStreetState', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (8, N'CarrierStreetCountry', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (9, N'CarrierStreetZip', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (10, N'CarrierMailAddress1', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (11, N'CarrierMailAddress2', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (12, N'CarrierMailAddress3', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (13, N'CarrierMailCity', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (14, N'CarrierMailState', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (15, N'CarrierMailCountry', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (16, N'CarrierMailZip', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (17, N'CarrierTypeCode', 0, 0, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (18, N'CarrierSCAC', 0, 0, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (19, N'CarrierWebSite', 0, 0, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (20, N'CarrierEmail', 0, 0, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (21, N'CarrierQualInsuranceDate', 0, 0, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (22, N'CarrierQualQualified', 0, 0, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (23, N'CarrierQualAuthority', 0, 0, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (24, N'CarrierQualContract', 0, 0, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (25, N'CarrierQualSignedDate', 0, 0, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (26, N'CarrierQualContractExpiresDate', 0, 0, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (27, N'CarrierContCarrierControl', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (28, N'CarrierContName', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (29, N'CarrierContTitle', 0, 0, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (30, N'CarrierContactPhone', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (31, N'CarrierContactFax', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (32, N'CarrierContact800', 0, 0, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (33, N'CarrierContactEMail', 0, 0, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (34, N'CompNumber', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (35, N'CompName', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (36, N'CompNatNumber', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (37, N'CompNatName', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (38, N'CompStreetAddress1', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (39, N'CompStreetAddress2', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (40, N'CompStreetAddress3', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (41, N'CompStreetCity', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (42, N'CompStreetState', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (43, N'CompStreetCountry', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (44, N'CompStreetZip', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (45, N'CompMailAddress1', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (46, N'CompMailAddress2', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (47, N'CompMailAddress3', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (48, N'CompMailCity', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (49, N'CompMailState', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (50, N'CompMailCountry', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (51, N'CompMailZip', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (52, N'CompWeb', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (53, N'CompEmail', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (54, N'CompDirections', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (55, N'CompAbrev', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (56, N'CompActive', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (57, N'CompNEXTrack', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (58, N'CompNEXTStopAcctNo', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (59, N'CompNEXTStopPsw', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (60, N'CompNextstopSubmitRFP', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (61, N'CompFAAShipID', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (62, N'CompFAAShipDate', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (63, N'CompFinDuns', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (64, N'CompFinTaxID', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (65, N'CompFinPaymentForm', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (66, N'CompFinSIC', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (67, N'CompFinPaymentDiscount', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (68, N'CompFinPaymentDays', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (69, N'CompFinPaymentNetDays', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (70, N'CompFinCommTerms', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (71, N'CompFinCommTermsPer', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (72, N'CompFinCreditLimit', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (73, N'CompFinCreditUsed', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (74, N'CompFinInvPrnCode', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (75, N'CompFinInvEMailCode', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (76, N'CompFinCurType', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (77, N'CompFinFBToleranceHigh', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (78, N'CompFinFBToleranceLow', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (79, N'CompFinCustomerSince', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (80, N'CompFinCardType', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (81, N'CompFinCardName', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (82, N'CompFinCardExpires', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (83, N'CompFinCardAuthorizor', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (84, N'CompFinCardAuthPassword', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (85, N'CompFinUseImportFrtCost', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (86, N'CompFinBkhlFlatFee', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (87, N'CompFinBkhlCostPerc', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (88, N'CompLatitude', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (89, N'CompLongitude', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (90, N'CompMailTo', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (91, N'CompContCompControl', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (92, N'CompContName', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (93, N'CompContTitle', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (94, N'CompCont800', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (95, N'CompContPhone', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (96, N'CompContFax', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (97, N'CompContEMail', 2, 0, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (98, N'LaneNumber', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (99, N'LaneName', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (100, N'LaneNumberMaster', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (101, N'LaneNameMaster', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (102, N'LaneCompNumber', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (103, N'LaneDefaultCarrierUse', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (104, N'LaneDefaultCarrierNumber', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (105, N'LaneOrigCompNumber', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (106, N'LaneOrigName', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (107, N'LaneOrigAddress1', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (108, N'LaneOrigAddress2', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (109, N'LaneOrigAddress3', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (110, N'LaneOrigCity', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (111, N'LaneOrigState', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (112, N'LaneOrigCountry', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (113, N'LaneOrigZip', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (114, N'LaneOrigContactPhone', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (115, N'LaneOrigContactPhoneExt', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (116, N'LaneOrigContactFax', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (117, N'LaneDestCompNumber', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (118, N'LaneDestName', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (119, N'LaneDestAddress1', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (120, N'LaneDestAddress2', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (121, N'LaneDestAddress3', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (122, N'LaneDestCity', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (123, N'LaneDestState', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (124, N'LaneDestCountry', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (125, N'LaneDestZip', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (126, N'LaneDestContactPhone', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (127, N'LaneDestContactPhoneExt', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (128, N'LaneDestContactFax', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (129, N'LaneConsigneeNumber', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (130, N'LaneRecMinIn', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (131, N'LaneRecMinUnload', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (132, N'LaneRecMinOut', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (133, N'LaneAppt', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (134, N'LanePalletExchange', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (135, N'LanePalletType', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (136, N'LaneBenchMiles', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (137, N'LaneBFC', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (138, N'LaneBFCType', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (139, N'LaneRecHourStart', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (140, N'LaneRecHourStop', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (141, N'LaneDestHourStart', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (142, N'LaneDestHourStop', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (143, N'LaneComments', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (144, N'LaneCommentsConfidential', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (145, N'LaneLatitude', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (146, N'LaneLongitude', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (147, N'LaneTempType', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (148, N'LaneTransType', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (149, N'LanePrimaryBuyer', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (150, N'LaneAptDelivery', 1, 0, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (151, N'BookCarrOrderNumber', 3, 1, N'Payables') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (152, N'BookFinAPPayAmt', 3, 1, N'Payables') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (153, N'BookFinAPCheck', 3, 1, N'Payables') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (154, N'BookFinAPBillInvDate', 3, 1, N'Payables') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (155, N'BookFinAPPayDate', 3, 1, N'Payables') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (156, N'BookFinAPGLNumber', 3, 1, N'Payables') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (157, N'APGLDescription', 3, 1, N'Payables') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (158, N'BookFinAPActWgt', 3, 1, N'Payables') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (159, N'BookCarrOrderNumber', 4, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (160, N'BookCarrDockPUAssigment', 4, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (161, N'BookCarrScheduleDate', 4, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (162, N'BookCarrScheduleTime', 4, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (163, N'BookCarrActualDate', 4, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (164, N'BookCarrActualTime', 4, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (165, N'BookCarrActLoadCompleteDate', 4, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (166, N'BookCarrActLoadCompleteTime', 4, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (167, N'BookFinAPBillNumber', 3, 1, N'Payables') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (168, N'BrokerNumber', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (169, N'BrokerName', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (170, N'APCarrierNumber', 5, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (171, N'APBillNumber', 5, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (172, N'APBillDate', 5, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (173, N'APPONumber', 5, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (174, N'APCustomerID', 5, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (175, N'APCostCenterNumber', 5, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (176, N'APTotalCost', 5, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (177, N'APProNumber', 5, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (178, N'APBLNumber', 5, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (179, N'APBilledWeight', 5, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (180, N'APCNSNumber', 5, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (181, N'APReceivedDate', 5, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (182, N'APPayCode', 5, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (183, N'APTotalTax', 5, 1, NULL) "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (184, N'LaneOriginAddressUse', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (185, N'POHDRnumber', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (186, N' POHDRPOdate', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (187, N'POHDRvendor', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (188, N'POHDRShipdate', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (189, N'POHDRBuyer', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (190, N'POHDRFrt', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (191, N'POHDRTotalFrt', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (192, N'POHDRTotalCost', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (193, N'POHDRWgt', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (194, N'POHDRCube', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (195, N'POHDRQty', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (196, N'POHDRPallets', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (197, N'POHDRLines', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (198, N'POHDRConfirm', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (199, N'POHDRDefaultCarrier', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (200, N'POHDRReqDate', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (201, N'POHDRShipInstructions', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (202, N'POHDRCooler', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (203, N'POHDRFrozen', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (204, N'POHDRDry', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (205, N'POHDRTemp', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (206, N'POHDRCarType', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (207, N'POHDRShipVia', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (208, N'POHDRShipViaType', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (209, N'POHDROtherCost', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (210, N'POConsigneeNumber', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (211, N'POHDRStatusFlag', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (212, N'POHDRChepGLID', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (213, N'POHDRCarrierEquipmentCodes', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (214, N'POHDRCarrierTypeCode', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (215, N'POHDRPalletPositions', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (216, N'POHDRSchedulePUDate', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (217, N'POHDRSchedulePUTime', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (218, N'POHDRScheduleDelDate', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (219, N'POHDRScheduleDelTime', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (220, N'POHDRActPUDate', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (221, N'POHDRActPUTime', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (222, N'POHDRActDelDate', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (223, N'POHDRActDelTime', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (224, N'ItemNumber', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (225, N'FixOffInvAllow', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (226, N'FixFrtAllow', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (227, N'QtyOrdered', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (228, N'FreightCost', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (229, N'ItemCost', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (230, N'Weight', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (231, N'Cube', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (232, N'Pack', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (233, N'Size', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (234, N'Description', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (235, N'Hazmat', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (236, N'Brand', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (237, N'CostCenter', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (238, N'LotExpirationDate', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (239, N'GTIN', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (240, N'CustItemNumber', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (241, N'PalletType', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (242, N'LaneCarrierEquipmentCodes', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (243, N'LaneChepGLID', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (244, N'LaneCarrierTypeCode', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (245, N'LanePickUpMon', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (246, N'LanePickUpTue', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (247, N'LanePickUpWed', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (248, N'LanePickUpThu', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (249, N'LanePickUpFri', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (250, N'LanePickUpSat', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (251, N'LanePickUpSun', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (252, N'LaneDropOffMon', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (253, N'LaneDropOffTue', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (254, N'LaneDropOffWed', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (255, N'LaneDropOffThu', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (256, N'LaneDropOffFri', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (257, N'LaneDropOffSat', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (258, N'LaneDropOffSun', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (259, N'LaneCalOpen', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (260, N'LaneCalStartTime', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (261, N'LaneCalEndTime', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (262, N'LaneCalIsHoliday', 1, 1, N'Lane') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (263, N'CompCalOpen', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (264, N'CompCalStartTime', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (265, N'CompCalEndTime', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (266, N'CompCalIsHoliday', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (267, N'CompTimeZone', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (268, N'CompRailStationName', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (269, N'CompRailSPLC', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (270, N'CompRailFSAC', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (271, N'CompRail333', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (272, N'CompRailR260', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (273, N'CompIsTransLoad', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (274, N'CompUser1', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (275, N'CompUser2', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (276, N'CompUser3', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (277, N'CompUser4', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (278, N'CompContPhoneExt', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (279, N'Month', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (280, N'Day', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (281, N'Open', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (282, N'StartTime', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (283, N'EndTime', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (284, N'IsHoliday', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (285, N'CompContEmail', 2, 1, N'Company') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (286, N'POHDRPOdate', 7, 1, N'Book') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (287, N'CarrierUser1', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (288, N'CarrierUser2', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (289, N'CarrierUser3', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (290, N'CarrierUser4', 0, 1, N'Carrier') "
        strSQL &= " INSERT [dbo].[tblImportFields] ([ImportControl], [ImportFieldName], [ImportFileType], [ImportFlag], [ImportFileName]) VALUES (291, N'CarrierContPhoneExt', 0, 1, N'Carrier') "
        strSQL &= " SET IDENTITY_INSERT [dbo].[tblImportFields] OFF "

        oBase.executeSQL(strSQL.ToString())

    End Sub



    ''' <summary>
    ''' returns the actual boolean provided using Boolean.TryParse or false
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function getXMLBool(ByVal s As String) As Boolean
        Dim bVal As Boolean = False
        If Boolean.TryParse(s, bVal) Then
            Return bVal
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' returns the actual double provided using double.tryparse or 0
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function getXMLDouble(ByVal s As String) As Double
        Dim dVal As Double = 0
        If Double.TryParse(s, dVal) Then
            Return dVal
        Else
            Return 0
        End If
    End Function

    ''' <summary>
    ''' returns the actual Integer provided using Integer.tryparse or 0
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function getXMLInteger(ByVal s As String) As Double
        Dim iVal As Integer = 0
        If Integer.TryParse(s, iVal) Then
            Return iVal
        Else
            Return 0
        End If
    End Function

End Class
