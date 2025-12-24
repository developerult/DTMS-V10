' Main Imports
#Const StandAlone = True
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.ServiceModel
Imports System.IO
Imports System.Reflection
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading.Tasks
Imports System.Xml
Imports System.Xml.Serialization


'Econnect imports
Imports Microsoft.Dynamics.GP.eConnect
Imports Microsoft.Dynamics.GP.eConnect.Serialization


'NGL Imports
Imports NGL.FreightMaster
Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports DTran = NGL.Core.Utility.DataTransformation
Imports NGL.Core.Communication.Email
Imports NGL.Core.Communication.General
Imports NGL.Core
Imports NGL.FM
Imports TMS = NGL.FreightMaster.Integration
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL
Imports LTS = NGL.FreightMaster.Data.LTS
Imports GPObject = NGL.FM.GPIntegration


'Public Class clsApplication : Inherits NGLCommandLineBaseClass
Public Class clsApplication : Inherits FreightMaster.Core.NGLCommandLineBaseClass


#Region "GP Code 2015"

    'This is the main function. It gets a list of GP companies and processes each
    Public Function GPDynamics(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByVal c As clsDefaultIntegrationConfiguration, ByVal AdminEmail As String) As Boolean

        Dim ReturnValue As Boolean = True
        'Dim GPCompany As New GPDataIntegrationSTructure.GPCompanies
        'Dim GPCompaniesToProcess As New List(Of GPDataIntegrationSTructure.GPCompanies)
        Dim SOPOrdersToProcess As New List(Of GPDataIntegrationSTructure.SOPOrders)
        Dim POOrdersToProcess As New List(Of GPDataIntegrationSTructure.POOrders)
        'Dim GPSQLServer As String = "NGLGP2013R2"
        Dim SuccessFlag As Boolean = True
        Dim ProcessToRun As Int16 = 0
        Dim SOPType As Integer = 2
        Dim ErrorString As String = ""

        Try

            'Get the TMS funciton that returns list of GP companies


            ' For now, use the code below to set one GP company
            'GPCompany.GPDatabase = "TWO"
            'GPCompaniesToProcess.Add(GPCompany)

            'For Each GPDB In GPCompaniesToProcess

            For ProcessToRun = 0 To 2

                Select Case ProcessToRun

                    Case 0

                        If (c.SalesOrderOn) Then

                            'ProcessToRun = 0 is to process Sales Orders
                            If Me.Debug Then Log("Reading GetGPSOPOrders.")
                            SOPOrdersToProcess = GetGPSOPOrders(TMSSetting, SOPType, ErrorString, Me.AdminEmail, c)

                            If (SOPOrdersToProcess.Count > 0) Then

                                'For Each SOPOrder In SOPOrdersToProcess
                                ' Talk to Rob about what function to call here
                                If (ErrorString = "") Then
                                    If Me.Debug Then Log("Running ProcessGPSOPOrders.")
                                    'enable for debug testing with DLL
                                    'If System.Diagnostics.Debugger.IsAttached And Me.Debug Then
                                    '    SuccessFlag = ProcessGPSOPOrdersDLLDirect(TMSSetting, c, SOPOrdersToProcess, SOPType, 100)
                                    'Else
                                    SuccessFlag = ProcessGPSOPOrders(TMSSetting, c, SOPOrdersToProcess, SOPType, 100)
                                    'End If


                                    If (Not SuccessFlag) Then
                                        If Me.Verbose Or Me.Debug Then Log("ProcessGPSOPOrders Failed.")
                                        'Report error

                                    End If

                                Else

                                    LogError("There was an error getting SOP Orders.", ErrorString, Me.AdminEmail)

                                End If

                            Else

                                If Me.Verbose Then Log("No Sales Orders to Process")

                            End If

                        End If

                    Case 1

                        If (c.PurchaseOrderOn) Then

                            'Removed by RHR v-7.0.5.102 8/15/2016 because we do not have a list of Processed PO Orders to list
                            'Added back by SEM on 7/30/2017
                            'TODO: fix list of purchase orders
                            If Me.Verbose Then Log("Purchase Order Processing Is not Active")
                            'ProcessToRun = 1 is to process Purchase Orders

                            If Me.Debug Then Log("Reading GetGPPOOrders.")
                            POOrdersToProcess = GetGPPOOrders(c, TMSSetting, 0, ErrorString, Me.AdminEmail)

                            If (POOrdersToProcess.Count > 0) Then

                                For Each PONum In POOrdersToProcess

                                    If Me.Debug Then Log("Running POOrdersToProcess.")

                                    SuccessFlag = ProcessGPPOOrders(TMSSetting, c, POOrdersToProcess, 0, 100)

                                Next

                            Else

                                If Me.Verbose Then Log("No Purchase Orders to Process")

                            End If

                        End If

                    Case 2

                        If (c.TransferOrderOn) Then


                            'ProcessToRun = 2 is to process inventory transfers

                            If Me.Verbose Then Log("Transfer Order Processing Is not Active")

                            'Throw an error.  Values should only be 0 to 2
                            LogError("Invalid Process to Run.", "In the GP Dynamics function, a process to run value of greater than 2 was passed in.", Me.AdminEmail)


                        End If

                End Select

            Next

            'Next

        Catch ex As Exception

            LogError(Source & " Error!  Unexpected GP GPDynamics Error", Source & " Could not process any GP Company requests; the actual error is:  " & ex.Message, AdminEmail)
            'Throw

            ReturnValue = False
            'Report error

        End Try

        Return ReturnValue

    End Function


    ''' <summary>
    ''' Creates Vendor information if it is missing
    ''' </summary>
    ''' <param name="VendorFound"></param>
    ''' <param name="TMSSetting"></param>
    ''' <param name="OrigEconnectSt"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.104 on 08/28/2027 added using to database activity for auto dispose
    ''' Modified by RHR for v-7.0.6.104 on 08/28/2027 added using to file activity for auto dispose
    ''' </remarks>
    Public Function CreateVendor(ByVal VendorFound As GPObject.GPDataIntegrationSTructure.VendorFound, ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByVal OrigEconnectSt As String) As Boolean

        Dim ReturnValue As Boolean = True
        Dim EConnectStr As String = ""
        Dim VendorMstRec As New GPObject.GPDataIntegrationSTructure.VendorMaster
        Dim serializer As New XmlSerializer(GetType(eConnectType))
        Dim eConnect As New eConnectType
        Dim sCustomerDocument As String
        Dim PMVendorType As New PMVendorMasterType
        Dim PMVendorRec As New taUpdateCreateVendorRcd
        Dim XLMFileToUse As String = "C:\Users\Public\Documents\APXML.xml"


        Try
            If Not String.IsNullOrWhiteSpace(TMSSetting.ERPUser) AndAlso Not String.IsNullOrWhiteSpace(TMSSetting.ERPPassword) Then
                EConnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & VendorFound.GPCompany & ";User ID=" & TMSSetting.ERPUser & ";Password=" & TMSSetting.ERPPassword
            Else
                EConnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & VendorFound.GPCompany & ";Integrated Security=SSPI;Trusted_Connection=True"
            End If
            'EConnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & VendorFound.GPCompany & ";Integrated Security=SSPI;Trusted_Connection=True"
            'Modified by RHR for v-7.0.6.104 on 08/28/2027 added using to db activity for auto dispose
            Using SQLConn As New SqlConnection(EConnectStr)


                Using SQLCom As New SqlCommand




                    SQLConn.Open()

                    SQLCom.Connection = SQLConn

                    SQLCom.CommandText = "select * from PM00200 where VENDORID = '" + VendorFound.VendorID + "'"
                    Using TblRec As SqlDataReader = SQLCom.ExecuteReader
                        TblRec.Read()
                        If (TblRec.HasRows) Then

                            With PMVendorRec

                                .VENDORID = TblRec("VENDORID").ToString()
                                .VENDNAME = TblRec("VENDNAME").ToString()
                                .VNDCHKNM = TblRec("VNDCHKNM").ToString()
                                .VENDSHNM = TblRec("VENDSHNM").ToString()
                                .VADDCDPR = TblRec("VADDCDPR").ToString()
                                .VADCDPAD = TblRec("VADCDPAD").ToString()
                                .VADCDSFR = TblRec("VADCDSFR").ToString()
                                .VADCDTRO = TblRec("VADCDTRO").ToString()
                                .VNDCLSID = TblRec("VNDCLSID").ToString()
                                .VNDCNTCT = TblRec("VNDCNTCT").ToString()
                                .ADDRESS1 = TblRec("ADDRESS1").ToString()
                                .ADDRESS2 = TblRec("ADDRESS2").ToString()
                                .ADDRESS3 = TblRec("ADDRESS3").ToString()
                                .CITY = TblRec("CITY").ToString()
                                .STATE = TblRec("STATE").ToString()
                                .ZIPCODE = TblRec("ZIPCODE").ToString()
                                .COUNTRY = TblRec("COUNTRY").ToString()
                                .PHNUMBR1 = TblRec("PHNUMBR1").ToString()
                                .PHNUMBR2 = TblRec("PHNUMBR2").ToString()
                                .FAXNUMBR = TblRec("FAXNUMBR").ToString()
                                .UPSZONE = TblRec("UPSZONE").ToString()
                                .SHIPMTHD = TblRec("SHIPMTHD").ToString()
                                .TAXSCHID = TblRec("TAXSCHID").ToString()
                                .ACNMVNDR = TblRec("ACNMVNDR").ToString()
                                .TXIDNMBR = TblRec("TXIDNMBR").ToString()
                                .VENDSTTS = TblRec("VENDSTTS").ToString()
                                .CURNCYID = TblRec("CURNCYID").ToString()
                                .TXRGNNUM = TblRec("TXRGNNUM").ToString()
                                .TRDDISCT = TblRec("TRDDISCT").ToString()
                                .TEN99TYPE = TblRec("TEN99TYPE").ToString()
                                .TEN99BOXNUMBER = TblRec("TEN99BOXNUMBER").ToString()
                                .MINORDER = TblRec("MINORDER").ToString()
                                .PYMTRMID = TblRec("PYMTRMID").ToString()
                                .MINPYTYP = TblRec("MINPYTYP").ToString()
                                .MINPYPCT = TblRec("MINPYPCT").ToString()
                                .MINPYDLR = TblRec("MINPYDLR").ToString()
                                .MXIAFVND = TblRec("MXIAFVND").ToString()
                                .MAXINDLR = TblRec("MAXINDLR").ToString()
                                .COMMENT1 = TblRec("COMMENT1").ToString()
                                .COMMENT2 = TblRec("COMMENT2").ToString()
                                .USERDEF1 = TblRec("USERDEF1").ToString()
                                .USERDEF2 = TblRec("USERDEF2").ToString()
                                .CRLMTDLR = TblRec("CRLMTDLR").ToString()
                                .PYMNTPRI = TblRec("PYMNTPRI").ToString()
                                .KPCALHST = TblRec("KPCALHST").ToString()
                                .KGLDSTHS = TblRec("KGLDSTHS").ToString()
                                .KPERHIST = TblRec("KPERHIST").ToString()
                                .KPTRXHST = TblRec("KPTRXHST").ToString()
                                .HOLD = TblRec("HOLD").ToString()
                                .PTCSHACF = TblRec("PTCSHACF").ToString()
                                .CREDTLMT = TblRec("CREDTLMT").ToString()
                                .WRITEOFF = TblRec("WRITEOFF").ToString()
                                .MXWOFAMT = TblRec("MXWOFAMT").ToString()
                                .CHEKBKID = TblRec("CHEKBKID").ToString()
                                .RATETPID = TblRec("RATETPID").ToString()
                                .Revalue_Vendor = TblRec("Revalue_Vendor").ToString()
                                .Post_Results_To = TblRec("Post_Results_To").ToString()
                                .FREEONBOARD = TblRec("FREEONBOARD").ToString()
                                .DISGRPER = TblRec("DISGRPER").ToString()
                                .DUEGRPER = TblRec("DUEGRPER").ToString()
                                .USERLANG = TblRec("USERLANG").ToString()
                                .CCode = TblRec("CCode").ToString()

                            End With

                            PMVendorType.taUpdateCreateVendorRcd = PMVendorRec

                            'Populate the eConnect XML document object with the RMCustomerMasterType schema object
                            ReDim eConnect.PMVendorMasterType(0)

                            eConnect.PMVendorMasterType(0) = PMVendorType
                            'Modified by RHR for v-7.0.6.104 on 08/28/2027 added using to file activity for auto dispose
                            Using fs As New FileStream(XLMFileToUse, FileMode.Create)
                                Using writer As New XmlTextWriter(fs, New UTF8Encoding)
                                    ' Use the XmlTextWriter to serialize the eConnect XML document object to the file
                                    serializer.Serialize(writer, eConnect)
                                    writer.Close()
                                End Using
                            End Using

                            'Load the customer XML into an XML document object
                            Dim xmldoc As New Xml.XmlDocument
                            xmldoc.Load(XLMFileToUse)

                            'Use the XML document to create a string
                            sCustomerDocument = xmldoc.OuterXml

                            'Create a connection string to your Micrsoft Dynamics GP database
                            'Replace data source and initial catalog values to use your server and company database
                            'sConnectionString = "data source=" & DTSource & ";initial catalog=" & InitCatalog & ";Integrated Security=SSPI;Integrated Security=False;User ID=" & UserID & ";Password=" & PWord

                            'Create the new customer in Microsoft Dynamics GP.
                            'e.CreateEntity(OrigEconnectSt, sCustomerDocument)
                            'e.eConnect_EntryPoint(sConnectionString, EnumTypes.ConnectionStringType.SqlClient, sCustomerDocument, EnumTypes.SchemaValidationType.None)

                            Using e As New eConnectMethods

                                e.CreateEntity(OrigEconnectSt, sCustomerDocument)

                            End Using

                        End If

                        TblRec.Close()

                    End Using 'tblRec is disposed 
                End Using 'replaces SQLCom.Dispose()
            End Using 'replaces SQLConn.Dispose()


        Catch ex As Exception
            LogError("Error!  Unexpected GP Create Vendor Error", Source & " Could not process the create vendor requests; the actual error is:  " & ex.Message, AdminEmail)
            ReturnValue = False

        End Try

        Return ReturnValue

    End Function

    ''' <summary>
    ''' Get Freight Payment information
    ''' </summary>
    ''' <param name="EconnectStr"></param>
    ''' <param name="GPCompany"></param>
    ''' <param name="AdminEmail"></param>
    ''' <param name="c"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 'Modified by RHR for v-7.0.6.104 on 08/28/2027 
    '''   when calling return outside of try the finally statement does not close the sql connection
    '''   the ERPFrieghtAccountIndex test and other commands were moved aboce the creation of the SQL Connection
    ''' </remarks>
    Public Function GetFreightPayments(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByVal EconnectStr As String, ByVal GPCompany As String, ByVal AdminEmail As String, ByRef c As clsDefaultIntegrationConfiguration) As List(Of tmsintegrationservices.clsPayablesObject705)

        Dim ReturnValue As New List(Of tmsintegrationservices.clsPayablesObject705)
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) Then
            LogError("Missing TMS Integration settings for Freight Payment Update to TMS; nothing to do.")
            Return ReturnValue
        End If
        'Modified by RHR for v-7.0.6.104 on 08/28/2027 when calling return outside of try the finally statement does not close the sql connection
        If String.IsNullOrWhiteSpace(ERPFrieghtAccountIndex) Then
            Log("Cannot process Payables Interface due to a missing ERP Freight Account Index value.")
            Return ReturnValue
        End If
        Dim Counter As Integer = 0
        'Dim AccountIndex As Integer 'we now use ERPFrieghtAccountIndex
        Dim RunDate As Date = Date.Now.AddDays(-1)
        Dim GetFunctions As New GPFunctions(c) 'Modified by RHR for v-7.0.5.102 10/14/2016
        Dim CheckNumber As String

        Dim SQLConn = New SqlConnection(EconnectStr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader
        Dim UnpaidFreightBillas As String = ""


        Try
            Dim apExport As New tmsintegrationservices.DTMSERPIntegration()
            apExport.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                apExport.UseDefaultCredentials = True
            Else
                apExport.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
            Dim aUnpaidFreightBills() As tmsintegrationservices.APUnpaidFreightBills = apExport.GetUnpaidFreightBills(TMSSetting.TMSAuthCode, GPCompany, TMSSetting.ERPExportMaxRowsReturned, RetVal, ReturnMessage)
            apExport = Nothing
            If RetVal <> 0 Then
                Log("Error processing Freight Payment Update to TMS: " & ReturnMessage)
            End If
            If (Not aUnpaidFreightBills Is Nothing AndAlso aUnpaidFreightBills.Count > 0) Then

                Dim strSpacer As String = ""
                For Counter = 0 To aUnpaidFreightBills.Count - 1
                    If Not String.IsNullOrWhiteSpace(aUnpaidFreightBills(Counter).BookFinAPBillNumber) Then
                        UnpaidFreightBillas &= String.Format("{0}'{1}'", strSpacer, aUnpaidFreightBills(Counter).BookFinAPBillNumber)
                        strSpacer = ", "
                    End If
                Next

                'AccountIndex = GetFunctions.GetFrieghtAccountIndex ' we now use ERPFrieghtAccountIndex

                SQLConn.Open()
                SQLCom.Connection = SQLConn
                'SQLCom.CommandText = "select * from PM30200 where CURTRXAM = 0 and VCHRNMBR in (select APTVCHNM from PM30300 where DOCDATE = '" & FormatDateTime(RunDate,DateFormat.ShortDate.ToString & "') and DOCTYPE = 1 and VCHRNMBR in (select VCHRNMBR from PM30600 where DSTINDX in (select ACTINDX from GL00100 where ACTDESCR like '%freight%'))"
                'SQLCom.CommandText = "select * from PM30200 where CURTRXAM = 0 and VCHRNMBR in (select APTVCHNM from PM30300 where DOCDATE = '" & FormatDateTime(RunDate, DateFormat.ShortDate.ToString) & "') and DOCTYPE = 1 and VCHRNMBR in (select VCHRNMBR from PM30600 where DSTINDX = " & AccountIndex & ")"
                'SQLCom.CommandText = "select inv.DOCNUMBR, inv.DOCAMNT, inv.DOCDATE, inv.VENDORID, chk.APFRDCNM, chk.DOCDATE as chk_date from PM30200 inv inner join PM30300 chk on chk.APTVCHNM = inv.VCHRNMBR and inv.DOCTYPE = 1 where inv.CURTRXAM = 0 and inv.VCHRNMBR in (select APTVCHNM from PM30300 where DOCDATE = '04/12/2017') and inv.DOCTYPE = 1 and inv.VCHRNMBR in (select VCHRNMBR from PM30600 where DSTINDX = " & AccountIndex & ")'" '  and chk.VENDORID = 'ASSOCIAT0001'"
                'changed DOCDATE to >= to deal with times when the server is off line or the previous days procedure did not run
                SQLCom.CommandText = "select inv.DOCNUMBR, inv.DOCAMNT, inv.DOCDATE, inv.VENDORID, chk.APFRDCNM, chk.DOCDATE as chk_date from PM30200 inv inner join PM30300 chk on chk.APTVCHNM = inv.VCHRNMBR and inv.DOCTYPE = 1 where inv.CURTRXAM = 0 and inv.VCHRNMBR in (select APTVCHNM from PM30300 where inv.DOCTYPE = 1 and inv.DOCNUMBR in (" & UnpaidFreightBillas & "))"
                'If Me.Debug Then Log("GetFreightPayments SQL: " & SQLCom.CommandText)
                TblRec = SQLCom.ExecuteReader
                'If Me.Debug Then Log("Reading TblRec Data")
                While TblRec.Read

                    Dim SingleInv As New tmsintegrationservices.clsPayablesObject705

                    If (Len(Trim(TblRec("APFRDCNM").ToString)) > 15) Then

                        CheckNumber = Right(Trim(TblRec("APFRDCNM").ToString), 15)

                    Else

                        CheckNumber = Trim(TblRec("APFRDCNM").ToString)

                    End If
                    'SingleInv.BookCarrOrderNumber = TblRec("docnumbr").ToString
                    SingleInv.BookFinAPBillNumber = Trim(TblRec("docnumbr").ToString)
                    SingleInv.BookFinAPBillInvDate = TblRec("docdate").ToString
                    'SingleInv.BookFinAPCheck = Trim(TblRec("APFRDCNM").ToString)
                    SingleInv.BookFinAPCheck = CheckNumber
                    Double.TryParse(TblRec("DOCAMNT").ToString(), SingleInv.BookFinAPPayAmt)
                    SingleInv.BookFinAPPayDate = TblRec("chk_date").ToString
                    SingleInv.CarrierAlphaCode = Trim(TblRec("vendorid").ToString)
                    SingleInv.CompLegalEntity = GPCompany
                    SingleInv.CompAlphaCode = ""

                    ReturnValue.Add(SingleInv)

                    SingleInv = Nothing

                End While

                TblRec.Close()

            End If

            SQLCom.Dispose()

            SQLConn.Dispose()

        Catch ex As Exception

            'TblRec.Close()

            'SQLCom.Dispose()

            'SQLConn.Dispose()

            LogError(Source & " Error!  Unexpected GP GetFreightPayments Error", Source & " Could not process any get freight payment requests; the actual error is:  " & ex.Message, AdminEmail)
            'Throw
        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then TblRec.Close()
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not SQLCom Is Nothing Then SQLCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then SQLConn.Close()
                SQLConn.Dispose()
            Catch ex As Exception

            End Try
            GetFunctions = Nothing
        End Try

        Return ReturnValue

    End Function


    'Added by SEM, 2017-08-04
    '  Added for Ship Confirmation




    ''' <summary>
    ''' Returns the Freight Payment Data For DLL Direct processing
    ''' </summary>
    ''' <param name="EconnectStr"></param>
    ''' <param name="GPCompany"></param>
    ''' <param name="AdminEmail"></param>
    ''' <param name="c"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    '''   renamed to DLLDirect to allow full compatiblity and testing
    '''   all new calls should use the web services version GetFreightPayments
    ''' </remarks>
    Public Function GetFreightPaymentsDLLDirect(ByVal EconnectStr As String, ByVal GPCompany As String, ByVal AdminEmail As String, ByRef c As clsDefaultIntegrationConfiguration) As List(Of TMS.clsPayablesObject705)

        Dim ReturnValue As New List(Of TMS.clsPayablesObject705)
        Dim Counter As Integer = 0
        'Dim AccountIndex As Integer 'we now use ERPFrieghtAccountIndex
        Dim RunDate As Date = Date.Now.AddDays(-1)
        Dim GetFunctions As New GPFunctions(c) 'Modified by RHR for v-7.0.5.102 10/14/2016
        Dim CheckNumber As String
        Dim SQLConn = New SqlConnection(EconnectStr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader

        Try

            'AccountIndex = GetFunctions.GetFrieghtAccountIndex ' we now use ERPFrieghtAccountIndex

            SQLConn.Open()
            SQLCom.Connection = SQLConn
            'SQLCom.CommandText = "select * from PM30200 where CURTRXAM = 0 and VCHRNMBR in (select APTVCHNM from PM30300 where DOCDATE = '" & FormatDateTime(RunDate,DateFormat.ShortDate.ToString & "') and DOCTYPE = 1 and VCHRNMBR in (select VCHRNMBR from PM30600 where DSTINDX in (select ACTINDX from GL00100 where ACTDESCR like '%freight%'))"
            'SQLCom.CommandText = "select * from PM30200 where CURTRXAM = 0 and VCHRNMBR in (select APTVCHNM from PM30300 where DOCDATE = '" & FormatDateTime(RunDate, DateFormat.ShortDate.ToString) & "') and DOCTYPE = 1 and VCHRNMBR in (select VCHRNMBR from PM30600 where DSTINDX = " & AccountIndex & ")"
            'SQLCom.CommandText = "select inv.DOCNUMBR, inv.DOCAMNT, inv.DOCDATE, inv.VENDORID, chk.APFRDCNM, chk.DOCDATE as chk_date from PM30200 inv inner join PM30300 chk on chk.APTVCHNM = inv.VCHRNMBR and inv.DOCTYPE = 1 where inv.CURTRXAM = 0 and inv.VCHRNMBR in (select APTVCHNM from PM30300 where DOCDATE = '04/12/2017') and inv.DOCTYPE = 1 and inv.VCHRNMBR in (select VCHRNMBR from PM30600 where DSTINDX = " & AccountIndex & ")'" '  and chk.VENDORID = 'ASSOCIAT0001'"

            SQLCom.CommandText = "select inv.DOCNUMBR, inv.DOCAMNT, inv.DOCDATE, inv.VENDORID, chk.APFRDCNM, chk.DOCDATE as chk_date from PM30200 inv inner join PM30300 chk on chk.APTVCHNM = inv.VCHRNMBR and inv.DOCTYPE = 1 where inv.CURTRXAM = 0 and inv.VCHRNMBR in (select APTVCHNM from PM30300 where DOCDATE = '" & FormatDateTime(RunDate, DateFormat.ShortDate) & "') and inv.DOCTYPE = 1 and inv.VCHRNMBR in (select VCHRNMBR from PM30600 where DSTINDX in (" & ERPFrieghtAccountIndex & "))"
            'If Me.Debug Then Log("GetFreightPayments SQL: " & SQLCom.CommandText)
            TblRec = SQLCom.ExecuteReader
            'If Me.Debug Then Log("Reading TblRec Data")
            While TblRec.Read

                Dim SingleInv As New TMS.clsPayablesObject705

                If (Len(Trim(TblRec("APFRDCNM").ToString)) > 15) Then

                    CheckNumber = Right(Trim(TblRec("APFRDCNM").ToString), 15)

                Else

                    CheckNumber = Trim(TblRec("APFRDCNM").ToString)

                End If
                'SingleInv.BookCarrOrderNumber = TblRec("docnumbr").ToString
                SingleInv.BookFinAPBillNumber = Trim(TblRec("docnumbr").ToString)
                SingleInv.BookFinAPBillInvDate = TblRec("docdate").ToString
                'SingleInv.BookFinAPCheck = Trim(TblRec("APFRDCNM").ToString)
                SingleInv.BookFinAPCheck = CheckNumber
                Double.TryParse(TblRec("DOCAMNT").ToString(), SingleInv.BookFinAPPayAmt)
                SingleInv.BookFinAPPayDate = TblRec("chk_date").ToString
                SingleInv.CarrierAlphaCode = Trim(TblRec("vendorid").ToString)
                SingleInv.CompLegalEntity = GPCompany
                SingleInv.CompAlphaCode = ""

                ReturnValue.Add(SingleInv)

                SingleInv = Nothing

            End While

            TblRec.Close()

            SQLCom.Dispose()

            SQLConn.Dispose()

        Catch ex As Exception

            'TblRec.Close()

            'SQLCom.Dispose()

            'SQLConn.Dispose()

            LogError(" Error!  Unexpected GP GetFreightPayments Error", Source & " Could not process any get freight payment requests; the actual error is:  " & ex.Message, AdminEmail)
            'Throw
        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then TblRec.Close()
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not SQLCom Is Nothing Then SQLCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then SQLConn.Close()
                SQLConn.Dispose()
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function


    Private Function processPayablesDataDLLDirect(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting,
                                                  ByVal c As clsDefaultIntegrationConfiguration,
                                                  ByVal EConnectStr As String) As Boolean
        Dim blnRet As Boolean = True
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) Then
            LogError("Missing TMS Integration settings for Payables; nothing to do returning false")
            Return False
        End If
        If Not c.PayablesOn Then
            If Me.Verbose Then Log("Payables Interface is turned on in config file")
            Return True
        End If
        If (String.IsNullOrWhiteSpace(TMSSetting.LegalEntity)) Then
            If Me.Verbose Then Log("No Database defined, did not run Payables Integration")
            Return False
        End If
        Try
            If Me.Verbose Then Log("Begin Process Import Payables Data from GP to TMS for: " & c.ERPPayablesLastRunDate.ToShortDateString())

            Dim PaidInvoice As New List(Of TMS.clsPayablesObject705)
            'If Me.Debug Then Log("Paid Invoice Object Reated, passing values to GetFreightPayments: " & EConnectStr & ", " & TMSSetting.LegalEntity & ", " & Me.AdminEmail)
            'Modified by RHR for v-7.0.5.102 on 11/11/2016
            '  Warning!  the TMSSetting may be the wrong object because it is referencing the 
            '  Payables settings.
            '  Code was not changed need to test.
            PaidInvoice = GetFreightPaymentsDLLDirect(EConnectStr, TMSSetting.LegalEntity, Me.AdminEmail, c) 'Modified by RHR for v-7.0.5.102 10/14/2016
            'If Me.Debug Then Log("We have finished GetFreightPayments")
            If Not PaidInvoice Is Nothing AndAlso PaidInvoice.Count() > 0 Then
                'If Me.Debug Then Log("Processing " & PaidInvoice.Count().ToString() & " paid invoices")

                If Me.Verbose Then Log("Begin Freight Payments Integration From GP to TMS for: " & c.ERPPayablesLastRunDate)

                Dim PDInvtoTMS As New TMS.clsPayables(oConfig)
                Dim oResults = PDInvtoTMS.UpdatePayablesByFreightBill(PaidInvoice, Me.ConnectionString)

                Dim sLastError As String = PDInvtoTMS.LastError
                Dim ReturnMessage As String = ""
                'find email from Rob ERP alert

                Select Case oResults
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        If String.IsNullOrWhiteSpace(ReturnMessage) Then
                            If Me.Verbose Then Log("Data Connection Failure! could not import Payable information")
                        Else
                            LogError("Error Data Connection Failure! could not import Payable information:  " & sLastError)
                        End If

                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        If String.IsNullOrWhiteSpace(ReturnMessage) Then
                            If Me.Verbose Then Log("Data Integration Failure! could not import Payable information")
                        Else
                            'Modified by RHR for v-7.0.5.102 on 11/11/2016
                            '  Warning!  the TMSSetting may be the wrong object because it is referencing the 
                            '  Payables settings.
                            '  Code was not changed need to test.
                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Payables, ReturnMessage, False)
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        If String.IsNullOrWhiteSpace(ReturnMessage) Then
                            If Me.Verbose Then Log("Data Integration Had Errors! could not import Payable information")
                        Else
                            'Modified by RHR for v-7.0.5.102 on 11/11/2016
                            '  Warning!  the TMSSetting may be the wrong object because it is referencing the 
                            '  Payables settings.
                            '  Code was not changed need to test.
                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                        End If
                        'generateDataIntegrationFailureAlert(oResults, IntegrationModule.Payables, ReturnMessage, False)
                        'LogError("There was an error wheh processing the paid invioce number.  ", ReturnMessage, Me.AdminEmail)
                        'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                        'blnRet = True
                        'End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        If String.IsNullOrWhiteSpace(ReturnMessage) Then
                            If Me.Verbose Then Log("Data Validation Failure! could not import Payable information")
                        Else
                            'Modified by RHR for v-7.0.5.102 on 11/11/2016
                            '  Warning!  the TMSSetting may be the wrong object because it is referencing the 
                            '  Payables settings.
                            '  Code was not changed need to test.
                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Payables, ReturnMessage, False)
                        End If
                        'LogError("There was an error wheh processing the paid invioce number.  ", ReturnMessage, Me.AdminEmail)
                        'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                        'blnRet = True
                        'End If
                    Case Else
                        'success
                        'Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        'Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        If Me.Verbose Then Log("Success! The payable information was processed.")
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        'blnRet = True
                End Select


                'End of extract Freight invoices
            Else
                If Me.Debug Then Log("No paid invoices to process")
            End If
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Payables Integration Error", Source & " Unexpected Integration Error! Could not import Payables information:  " & ex.Message, AdminEmail)
            'Throw
        End Try

        Return blnRet
    End Function

    Private Function processPayablesData(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting,
                                         ByVal c As clsDefaultIntegrationConfiguration,
                                         ByVal EConnectStr As String) As Boolean
        Dim blnRet As Boolean = True
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) Then
            LogError("Missing TMS Integration settings for Payables; nothing to do returning false")
            Return False
        End If
        If Not c.PayablesOn Then
            If Me.Verbose Then Log("Payables Interface is turned off in config file")
            Return True
        End If
        If (String.IsNullOrWhiteSpace(TMSSetting.LegalEntity)) Then
            If Me.Verbose Then Log("No Database defined, did not run Payables Integration")
            Return False
        End If
        Try
            If Me.Verbose Then Log("Begin Process Import Payables Data from GP to TMS for: " & c.ERPPayablesLastRunDate.ToShortDateString())
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oPayablesIntegration As New tmsintegrationservices.DTMSERPIntegration()
            oPayablesIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oPayablesIntegration.UseDefaultCredentials = True
            Else
                oPayablesIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim PaidInvoices As List(Of tmsintegrationservices.clsPayablesObject705) = GetFreightPayments(TMSSetting, EConnectStr, TMSSetting.LegalEntity, Me.AdminEmail, c)
            If Not PaidInvoices Is Nothing AndAlso PaidInvoices.Count() > 0 Then
                Dim aPaidInvoices As tmsintegrationservices.clsPayablesObject705() = PaidInvoices.ToArray()
                Dim oResults = oPayablesIntegration.UpdatePayablesByFreightBill(TMSSetting.TMSAuthCode, aPaidInvoices, ReturnMessage)
                Select Case oResults
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        If String.IsNullOrWhiteSpace(ReturnMessage) Then
                            If Me.Verbose Then Log("Data Connection Failure! could not import Payable information")
                        Else
                            LogError("Error Data Connection Failure! could not import Payable information:  " & ReturnMessage)
                        End If

                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        If String.IsNullOrWhiteSpace(ReturnMessage) Then
                            If Me.Verbose Then Log("Data Integration Failure! could not import Payable information")
                        Else
                            'Modified by RHR for v-7.0.5.102 on 11/11/2016
                            '  Warning!  the TMSSetting may be the wrong object because it is referencing the 
                            '  Payables settings.
                            '  Code was not changed need to test.
                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Payables, ReturnMessage, False)
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        If String.IsNullOrWhiteSpace(ReturnMessage) Then
                            If Me.Verbose Then Log("Data Integration Had Errors! could not import Payable information")
                        Else
                            'Modified by RHR for v-7.0.5.102 on 11/11/2016
                            '  Warning!  the TMSSetting may be the wrong object because it is referencing the 
                            '  Payables settings.
                            '  Code was not changed need to test.
                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                        End If
                        'generateDataIntegrationFailureAlert(oResults, IntegrationModule.Payables, ReturnMessage, False)
                        'LogError("There was an error wheh processing the paid invioce number.  ", ReturnMessage, Me.AdminEmail)
                        'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                        'blnRet = True
                        'End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        If String.IsNullOrWhiteSpace(ReturnMessage) Then
                            If Me.Verbose Then Log("Data Validation Failure! could not import Payable information")
                        Else
                            'Modified by RHR for v-7.0.5.102 on 11/11/2016
                            '  Warning!  the TMSSetting may be the wrong object because it is referencing the 
                            '  Payables settings.
                            '  Code was not changed need to test.
                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Payables, ReturnMessage, False)
                        End If
                        'LogError("There was an error wheh processing the paid invioce number.  ", ReturnMessage, Me.AdminEmail)
                        'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                        'blnRet = True
                        'End If
                    Case Else
                        'success
                        'Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        'Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        If Me.Verbose Then Log("Success! The payable information was processed.")
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        'blnRet = True
                End Select
            Else
                If Me.Debug Then Log("No paid invoices to process")
            End If

        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Payables Integration Error", Source & " Unexpected Integration Error! Could not import Payables information:  " & ex.Message, AdminEmail)
            'Throw
        End Try

        Return blnRet
    End Function

    Private Function processSOPShipConfirmation(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByVal c As clsDefaultIntegrationConfiguration, ByVal EConnectStr As String) As Boolean

        Dim SOPOrdersToProcess As New List(Of GPDataIntegrationSTructure.SOPOrders)
        Dim ErrorString As String = ""
        Dim SOPType As Integer = 3
        Dim SuccessFlag As Boolean = True

        'ProcessToRun = 0 is to process Sales Orders
        If Me.Debug Then Log("Reading Ship Confirmation Records.")

        SOPOrdersToProcess = GetGPSOPOrders(TMSSetting, SOPType, ErrorString, Me.AdminEmail, c)

        If (SOPOrdersToProcess.Count > 0) Then

            'For Each SOPOrder In SOPOrdersToProcess
            ' Talk to Rob about what function to call here
            If (ErrorString = "") Then
                If Me.Debug Then Log("Running ProcessGPSOPOrders.")
                SuccessFlag = ProcessGPSOPOrders(TMSSetting, c, SOPOrdersToProcess, SOPType, 100)

                If (Not SuccessFlag) Then
                    If Me.Verbose Or Me.Debug Then Log("ProcessGPSOPOrders Failed.")
                    'Report error

                End If

            Else

                LogError("There was an error getting Ship Confirmation Orders.", ErrorString, Me.AdminEmail)

            End If

        Else

            If Me.Verbose Then Log("No Ship Confirmation Orders to Process")

        End If

        Return SuccessFlag
    End Function



    ''' <summary>
    ''' This overload is only used by the command line utility to process ship confirmations outside of the normal service layer
    ''' </summary>
    ''' <param name="sSource"></param>
    ''' <param name="c"></param>
    ''' <param name="dtLastRun"></param>
    ''' <returns></returns>
    Public Function processSOPShipConfirmationCommandLine(ByVal sSource As String, ByVal c As clsDefaultIntegrationConfiguration, ByVal dtLastRun As Date) As Date
        Dim Counter As Int16 = 0
        Dim EConnectStr As String
        Try
            If Me.Verbose Then Log("Begin Process SOP Ship Confirmation CommandLine ")
            Dim oConfig As New UserConfiguration()
            With oConfig
                .AdminEmail = Me.AdminEmail
                .AutoRetry = Me.AutoRetry
                .Database = Me.Database
                .DBServer = Me.DBServer
                .Debug = Me.Debug
                .FromEmail = Me.FromEmail
                .GroupEmail = Me.GroupEmail
                .LogFile = Me.LogFile
                .SMTPServer = Me.SMTPServer
                .UserName = "System Download"
                .WSAuthCode = "NGLSystem"
                .WCFAuthCode = "NGLSystem"
                .WCFURL = ""
                .WCFTCPURL = ""
                .ConnectionString = Me.ConnectionString

            End With
            Dim oSettings As tmsintegrationsettings.vERPIntegrationSetting()
            Dim sConfigLegals As String() = c.TMSRunLegalEntity.Split("|")
            'Modified by SEM on 7/7/2017, added split of GL codes
            Dim DefaultGLCodes As String() = c.GPFunctionsDefaultFreightGLAccount.Split("|")
            Dim DGLCode As New Dictionary(Of String, String)
            If sConfigLegals Is Nothing OrElse sConfigLegals.Count() < 1 Then
                sConfigLegals = New String() {c.TMSRunLegalEntity}
            End If
            If (sConfigLegals.Count <> DefaultGLCodes.Count) Then
                Throw New System.ApplicationException("The Default GL Configuration is not valid.  The count of Default GL Codes does not match the count of Legal Entities")
            End If
            'Added by Rob & Scott 2017-08-19
            For i As Int16 = 0 To sConfigLegals.Count - 1
                If (Not DefaultGLCodes Is Nothing AndAlso DefaultGLCodes.Count > i) Then
                    DGLCode.Add(sConfigLegals(i), DefaultGLCodes(i))
                End If
            Next
            If Not sConfigLegals Is Nothing AndAlso sConfigLegals.Count() > 0 Then
                For Each sLegal In sConfigLegals
#Disable Warning BC42030 ' Variable 'oSettings' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
                    If Not getTMSIntegrationSettings(c, oSettings, sLegal) Then Return dtLastRun
#Enable Warning BC42030 ' Variable 'oSettings' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
                    If Not oSettings Is Nothing AndAlso oSettings.Count > 0 Then
                        'group the settings by Legal Entity
                        Dim sLegals As List(Of String) = oSettings.Select(Function(x) x.LegalEntity).Distinct().ToList()
                        If Not sLegals Is Nothing AndAlso sLegals.Count() > 0 Then
                            Counter = 0
                            For Each legal In sLegals
                                Dim lLegalSettings As tmsintegrationsettings.vERPIntegrationSetting() = oSettings.Where(Function(x) x.LegalEntity = legal).ToArray()
                                If Not lLegalSettings Is Nothing AndAlso lLegalSettings.Count() > 0 Then
                                    Me.LegalEntity = legal
                                    Dim StandardSetting = getSpecificTMSSetting("Standard", lLegalSettings, Nothing)
                                    Dim GPCompany As New GPDataIntegrationSTructure.GPCompanies
                                    Dim GetFunctions As New GPFunctions(c)
                                    GPCompany.GPDatabase = Me.LegalEntity.ToString
                                    Dim TMSSetting = getSpecificTMSSetting("ShipConfirmed", lLegalSettings, StandardSetting)

                                    If Not String.IsNullOrWhiteSpace(TMSSetting.ERPUser) AndAlso Not String.IsNullOrWhiteSpace(TMSSetting.ERPPassword) Then
                                        EConnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & sLegal.ToString & ";User ID=" & TMSSetting.ERPUser & ";Password=" & TMSSetting.ERPPassword
                                    Else
                                        EConnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & sLegal.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
                                    End If

                                    Dim SOPOrdersToProcess As New List(Of GPDataIntegrationSTructure.SOPOrders)
                                    Dim ErrorString As String = ""
                                    Dim SOPType As Integer = 3
                                    Dim SuccessFlag As Boolean = True

                                    'ProcessToRun = 0 is to process Sales Orders
                                    If Me.Debug Then Log("Reading Ship Confirmation Records From: " & dtLastRun.ToString("yyyy-MM-dd HH:mm:ss.fff"))

                                    SOPOrdersToProcess = GetGPSOPOrders(TMSSetting, SOPType, ErrorString, Me.AdminEmail, c, dtLastRun)

                                    If (SOPOrdersToProcess.Count > 0) Then

                                        'For Each SOPOrder In SOPOrdersToProcess
                                        ' Talk to Rob about what function to call here
                                        If (ErrorString = "") Then
                                            If Me.Debug Then Log("Running processSOPShipConfirmationCommandLine.")
                                            Dim lGPOrderInvoces As New List(Of clsGPOrderInvoice)
                                            SuccessFlag = ProcessGPSOPOrders(TMSSetting, c, SOPOrdersToProcess, SOPType, 100, lGPOrderInvoces)

                                            If (Not SuccessFlag) Then
                                                If Me.Verbose Or Me.Debug Then Log("processSOPShipConfirmationCommandLine Failed.")
                                                'Report error
                                            Else
                                                'update the cost per pound
                                                If ProcessCostPerPoundIntoGp(TMSSetting, c, lGPOrderInvoces, AdminEmail) = "Success" AndAlso Not lGPOrderInvoces Is Nothing AndAlso lGPOrderInvoces.Count > 0 Then
                                                    'if we have updated the cost per pound return the last date run
                                                    Dim dtMax As Date = (From d In lGPOrderInvoces Select d.dex_row_ts).Max()
                                                    If dtMax > dtLastRun Then dtLastRun = dtMax Else dtLastRun = Date.Now()
                                                    If Me.Debug Then Log("Cost per Pound Update Complete for records From: " & dtLastRun.ToString("yyyy-MM-dd HH:mm:ss.fff"))
                                                Else
                                                    If Me.Debug Then Log("Process Cost Per Pound Update failed.")
                                                End If
                                            End If

                                        Else

                                            LogError("There was an error getting Ship Confirmation Orders From Command Line From: " & dtLastRun.ToString("yyyy-MM0dd HH:mm:ss.fff"), ErrorString, Me.AdminEmail)

                                        End If

                                    Else

                                        If Me.Verbose Then Log("No Ship Confirmation Orders to Process")

                                    End If

                                End If



                            Next
                        End If
                    End If
                Next
            End If
            If Me.Verbose Then Log("Process SOP Ship Confirmation CommandLine Complete")
            'TODO: add additional error handlers as needed
        Catch ex1 As System.ApplicationException
            LogError(Source & " Error!  Application Error in GP Process SOP Ship Confirmation CommandLine", Source & " : " & ex1.Message, AdminEmail)
            Throw
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected GP Process SOP Ship Confirmation CommandLine Error", Source & " Could not process ship confirmation requests; the actual error is:  " & ex.Message, AdminEmail)
            Throw
        Finally
            LogAllErrors()
            Me.closeLog(0)
        End Try

        Return dtLastRun
    End Function


    'Added by SEM 2017-08-04
    '  added for handling ship confirmation



    ''' <summary>
    ''' Read the AP Export Data from the TMS DLLs and sends it to GP
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 on 11/11/2016
    '''  moved code for calls to AP Export DLL from ProcessData method into this function
    ''' Modified by RHR for v-7.0.5.102 on 12/01/2016 
    '''  added default parameter,GPFunctionsDefaultFreightGLAccount, to populate the DebitGL (APGLNumber) if a value is not provided.
    ''' </remarks>
    Public Function processAPExportDataDLLDirect(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByVal c As clsDefaultIntegrationConfiguration) As Boolean
        Dim blnRet As Boolean = False
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) Then
            LogError("Missing TMS Integration settings for APExport; nothing to do returning false")
            Return False
        End If
        If Not c.APExportOn Then
            If Me.Verbose Then Log("AP Export Interface is turned on in config file")
            Return True
        End If
        If (String.IsNullOrWhiteSpace(TMSSetting.LegalEntity)) Then
            If Me.Verbose Then Log("No Database defined, did not run APExport")
            Return False
        End If
        Try
            If Me.Verbose Then Log("Begin Process APExport Data from TMS to GP for: " & c.ERPPayablesLastRunDate.ToShortDateString())
            Dim strConnection As String = Me.ConnectionString

            Dim oAPWCFData = New DAL.NGLAPMassEntryData(Me.WCFParameters)

            'Dim ProcessPayments As New List(Of String)
            'Dim ProcessPayments As List(Of LTS.spGetAPExportRecordsAggregatedResult)

            'Dim oAPWCFRets = oAPWCFData.GetAPExportData70(MaxRetry, RetryMinutes, CompLegalEntity, Me.MaxRowsReturned)
            'If Me.Debug Then Log("Calling GetAPExportRecordsAggregated")
            Dim ProcessPayments = oAPWCFData.GetAPExportRecordsAggregated(0, 0, Me.LegalEntity.ToString, 10000)
            If Not ProcessPayments Is Nothing AndAlso ProcessPayments.Count() > 0 Then
                For Each ProcessPymt In ProcessPayments

                    Dim InvHeader As New GPDataIntegrationSTructure.GPPMHeaderData

                    InvHeader.ChargeAmount = 0
                    InvHeader.DocumentAmount = ProcessPymt.BilledCost + ProcessPymt.APTotalTaxes
                    InvHeader.TaxAmount = ProcessPymt.APTotalTaxes
                    InvHeader.DocumentDate = GPFunctions.TryParseDate(ProcessPymt.APBillInvDate, Date.Now).ToShortDateString()
                    'used for Fabricam default GP Data must be "04/12/2017"
                    'InvHeader.DocumentDate = "04/12/2017"
                    InvHeader.DocumentType = 1
                    InvHeader.FreightAmount = ProcessPymt.BilledCost
                    InvHeader.InvoiceNumber = ProcessPymt.APBillNumber
                    If Me.Debug Then Log("Processing AP Freight Bill #: " & ProcessPymt.APBillNumber)
                    Select Case c.GPPMPOFieldValue
                        Case "PO"
                            InvHeader.PONumber = ""

                        Case "BOL"
                            '  Do not see the city, state
                            InvHeader.PONumber = ProcessPymt.BookCarrBLNumber

                        Case Else
                            InvHeader.PONumber = ""

                    End Select
                    'Modified by RHR for v-7.0.6.105 on 12/21/2017
                    '  bug do not set PONumber to an empty string
                    'InvHeader.PONumber = ""
                    InvHeader.PurchaseAmount = 0
                    InvHeader.TransactionDescription = ProcessPymt.RetMsg
                    InvHeader.VendorID = ProcessPymt.CarrierAlphaCode
                    'Modified by RHR for v-7.0.5.102 on 12/01/2016 
                    InvHeader.DebitGL = If(String.IsNullOrWhiteSpace(ProcessPymt.APGLNumber), c.GPFunctionsDefaultFreightGLAccount, ProcessPymt.APGLNumber)
                    InvHeader.CreditGL = ""
                    If Me.Debug Then Log("Sending AP Data to GP ")
                    Dim ErrorString = ProcessAPImportIntoGp(TMSSetting, c, InvHeader, Me.AdminEmail)

                    If (ErrorString = "") Then
                        If Me.Debug Then Log("Updating AP Export Status for Freight Bill # " & ProcessPymt.APBillNumber)
                        'Modified by RHR for v-7.0.6.105 on 12/14/2017 changed export date from invice date to current date
                        blnRet = oAPWCFData.UpdateStatus(ProcessPymt.BookSHID, ProcessPymt.APBillNumber, True, Date.Now(), Nothing)
                    Else

                        If (ErrorString <> "Error") Then

                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, ProcessDataReturnValues.nglDataValidationFailure, IntegrationModule.APExport, ErrorString, False)

                            LogError("There was an error processing Freight Payagbles Invoice Using Econect.", ErrorString, Me.GroupEmail)

                        End If

                    End If

                    InvHeader = Nothing

                Next
                ' End of Import AP Invoices into GP
            Else
                If Verbose Then Log("No Aggregated AP Export Updates to Process")
                blnRet = True
            End If
            If Debug Then Log("Process APExport Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Aggregated APExport Integration Error", Source & " Unexpected Integration Error! Could not process APExport information:  " & ex.Message, AdminEmail)
            blnRet = False
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Reads the AP Export Data from TMS via Web Services and sends it to GP
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 on 11/11/2016
    '''   New logice bypasses the current logic to read from the DLLs directly.
    ''' Modified by RHR for v-7.0.5.102 on 12/01/2016 
    '''  added default parameter,GPFunctionsDefaultFreightGLAccount, to populate the DebitGL (APGLNumber) if a value is not provided.
    ''' 
    ''' </remarks>
    Public Function processAPExportData(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByVal c As clsDefaultIntegrationConfiguration, CompanyGLAPCode As String) As Boolean
        Dim blnRet As Boolean = False
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) Then
            LogError("Missing TMS Integration settings for APExport; nothing to do returning false")
            Return False
        End If
        If Not c.APExportOn Then
            If Me.Verbose Then Log("AP Export Interface is turned off in config file")
            Return True
        End If
        If (String.IsNullOrWhiteSpace(TMSSetting.LegalEntity)) Then
            If Me.Verbose Then Log("No Database defined, did not run APExport")
            Return False
        End If
        Try
            If Me.Verbose Then Log("Begin Process APExport Data from TMS to GP for: " & c.ERPPayablesLastRunDate.ToShortDateString())
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
            Dim apExport As New tmsintegrationservices.DTMSERPIntegration()
            apExport.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                apExport.UseDefaultCredentials = True
            Else
                apExport.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim strCriteria As String = String.Format(" MaxRetry = {0} RetryMinutes = {1} MaxRowsReturned = {2} LegalEntity = {3}", TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, TMSSetting.ERPExportMaxRowsReturned, TMSSetting.LegalEntity)
            If Me.Debug Then Log("AP Criteria: " & strCriteria)
            'Dim oAPExportData As tmsintegrationservices.clsAPExportData70 = apExport.GetAPData70(TMSSetting.TMSAuthCode, TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, TMSSetting.LegalEntity, TMSSetting.ERPExportMaxRowsReturned, TMSSetting.ERPExportAutoConfirmation, RetVal, ReturnMessage)
            Dim oAPExportData As tmsintegrationservices.APExportRecordsAggregated() = apExport.GetAPExportRecordsAggregated(TMSSetting.TMSAuthCode, TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, TMSSetting.LegalEntity, TMSSetting.ERPExportMaxRowsReturned, RetVal, ReturnMessage)
            LastError = ReturnMessage
            If RetVal <> FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
                Select Case RetVal
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not export APExport information:  " & LastError)
                        Return False
                    Case Else
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, RetVal, IntegrationModule.APExport, LastError)
                        Return False
                End Select
            End If
            If oAPExportData Is Nothing Then
                If Me.Verbose Or Me.Debug Then Log("No AP Export Records Found")
                Return True
            Else
                If Me.Verbose Or Me.Debug Then Log(oAPExportData.Count().ToString() & " AP Export Records Found")
            End If

            If Not oAPExportData Is Nothing AndAlso oAPExportData.Count() > 0 Then
                For Each ProcessPymt In oAPExportData

                    Dim InvHeader As New GPDataIntegrationSTructure.GPPMHeaderData
                    If Me.Debug Then Log("Processing AP Export for SHID: " & ProcessPymt.BookSHID)
                    InvHeader.ChargeAmount = 0
                    InvHeader.DocumentAmount = ProcessPymt.BilledCost + ProcessPymt.APTotalTaxes
                    InvHeader.TaxAmount = ProcessPymt.APTotalTaxes
                    ' Modofied by SEM 11/29/2017
                    InvHeader.DocumentDate = GPFunctions.TryParseDate(ProcessPymt.APBillInvDate, Date.Now).ToShortDateString()
                    InvHeader.DocumentType = 1
                    InvHeader.FreightAmount = ProcessPymt.BilledCost
                    InvHeader.InvoiceNumber = ProcessPymt.APBillNumber
                    If Me.Debug Then Log("Processing AP Freight Bill #: " & ProcessPymt.APBillNumber)
                    Select Case c.GPPMPOFieldValue
                        Case "PO"
                            InvHeader.PONumber = ""

                        Case "BOL"
                            '  use the BOL number
                            InvHeader.PONumber = ProcessPymt.BookCarrBLNumber

                        Case Else
                            InvHeader.PONumber = ""

                    End Select
                    'Modified by RHR for v-7.0.6.105 on 12/21/2017
                    '  bug do not set PONumber to an empty string
                    'InvHeader.PONumber = ""
                    InvHeader.PurchaseAmount = 0
                    'InvHeader.TransactionDescription = ProcessPymt.BookSHID
                    Select Case c.GPPMDescriptionFieldValue
                        Case "STANDARD"
                            InvHeader.TransactionDescription = ProcessPymt.RetMsg
                        Case "CSW"
                            InvHeader.TransactionDescription = Right(ProcessPymt.ShippingCity.ToString + ProcessPymt.ShippingState.ToString + CStr(ProcessPymt.APActWgt), 30)
                        Case Else
                            InvHeader.TransactionDescription = ProcessPymt.RetMsg
                    End Select

                    InvHeader.VendorID = ProcessPymt.CarrierAlphaCode
                    'Modified by RHR for v-7.0.5.102 on 12/01/2016 
                    'InvHeader.DebitGL = If(String.IsNullOrWhiteSpace(ProcessPymt.APGLNumber), c.GPFunctionsDefaultFreightGLAccount, ProcessPymt.APGLNumber)
                    'Modified by SEM for v-7.0.6.103 Update on 07/17/2017, correcting parsing GL code issue
                    InvHeader.DebitGL = If(String.IsNullOrWhiteSpace(ProcessPymt.APGLNumber), CompanyGLAPCode, ProcessPymt.APGLNumber)
                    InvHeader.CreditGL = ""
                    If Me.Debug Then Log("Sending AP Data to GP ")
                    Dim ErrorString = ProcessAPImportIntoGp(TMSSetting, c, InvHeader, Me.AdminEmail)
                    'Dim ErrorString = "Debug AP Exception Loop"
                    If (ErrorString = "") Then
                        If Me.Debug Then Log("Updating AP Export Status for Freight Bill # " & ProcessPymt.APBillNumber)
                        'Modified by RHR for v-7.0.6.105 on 12/14/2017 changed export date from invice date to current date
                        blnRet = apExport.UpdateAPExportStatus(TMSSetting.TMSAuthCode, ProcessPymt.BookSHID, ProcessPymt.APBillNumber, True, Date.Now(), ReturnMessage)
                    Else
                        If (ErrorString <> "Error") Then
                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, ProcessDataReturnValues.nglDataValidationFailure, IntegrationModule.APExport, ErrorString, False)
                            LogError("There was an error processing Freight Payables Invoice Using Econect.", ErrorString, Me.GroupEmail)
                        End If
                    End If
                    InvHeader = Nothing
                Next
            Else
                If Verbose Then Log("No Aggregated AP Export Updates to Process")
                blnRet = True
            End If

            If Debug Then Log("Process APExport Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected APExport Integration Error", Source & " Unexpected Integration Error! Could not import APExport information:  " & ex.Message, AdminEmail)
            blnRet = False
        End Try

        Return blnRet
    End Function

    ''' <summary>
    ''' Do not use this method 
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <param name="c"></param>
    ''' <param name="CompanyGLAPCode"></param>
    ''' <returns></returns>
    Public Function processFreightBillCosts(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByVal c As clsDefaultIntegrationConfiguration, CompanyGLAPCode As String) As Boolean
        Dim blnRet As Boolean = False
        Dim AutoConfirmation As Boolean = False
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) Then
            LogError("Missing TMS Integration settings for APExport; nothing to do returning false")
            Return False
        End If
        If Not c.FrightBillCostsOn Then
            If Me.Verbose Then Log("Freight Bill Costs to SOP Orders Interface is turned off in config file")
            Return True
        End If
        If (String.IsNullOrWhiteSpace(TMSSetting.LegalEntity)) Then
            If Me.Verbose Then Log("No Database defined, did not run Freight Bill Costs")
            Return False
        End If
        Try
            If Me.Verbose Then Log("Begin Process Freight Bill Costs Data from TMS to GP for: " & c.ERPPayablesLastRunDate.ToShortDateString())
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
            Dim apExport As New tmsintegrationservices.DTMSERPIntegration()
            apExport.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                apExport.UseDefaultCredentials = True
            Else
                apExport.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim strCriteria As String = String.Format(" MaxRetry = {0} RetryMinutes = {1} MaxRowsReturned = {2} LegalEntity = {3}", TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, TMSSetting.ERPExportMaxRowsReturned, TMSSetting.LegalEntity)
            If Me.Debug Then Log("AP Criteria: " & strCriteria)
            'Dim oAPExportData As tmsintegrationservices.clsAPExportData70 = apExport.GetAPData70(TMSSetting.TMSAuthCode, TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, TMSSetting.LegalEntity, TMSSetting.ERPExportMaxRowsReturned, TMSSetting.ERPExportAutoConfirmation, RetVal, ReturnMessage)
            'Dim oAPExportData As tmsintegrationservices.APExportRecordsAggregated() = apExport.GetAPExportRecordsAggregated(TMSSetting.TMSAuthCode, TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, TMSSetting.LegalEntity, TMSSetting.ERPExportMaxRowsReturned, RetVal, ReturnMessage)
            'GetAPData70
            ' *********************** Uncomment when talk to Rob  *********************
            Dim oAPExportData As tmsintegrationservices.clsAPExportData70 = apExport.GetAPData70(TMSSetting.TMSAuthCode, TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, TMSSetting.LegalEntity, TMSSetting.ERPExportMaxRowsReturned, AutoConfirmation, RetVal, ReturnMessage)

            LastError = ReturnMessage
            If RetVal <> FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
                Select Case RetVal
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not get Freight Bill Costs information:  " & LastError)
                        Return False
                    Case Else
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, RetVal, IntegrationModule.APExport, LastError)
                        Return False
                End Select
            End If
            If oAPExportData.Headers Is Nothing Then
                If Me.Verbose Or Me.Debug Then Log("No Freight Bill Cost Records Found")
                Return True
            Else
                If Me.Verbose Or Me.Debug Then Log(oAPExportData.Headers.Count().ToString() & " Freight Bill Records Found")
            End If


            If Not oAPExportData Is Nothing AndAlso oAPExportData.Headers.Count() > 0 Then
                For Each FBAmt In oAPExportData.Headers

                    Dim FBAmounts As New GPDataIntegrationSTructure.FreightBillCosts
                    If Me.Debug Then Log("Processing Fright Bill Costs for SHID: " & FBAmt.ToString)
                    'FBAmounts.OrderNumber = FBAmt.BookFinAPBillNumber.ToString
                    FBAmounts.OrderNumber = FBAmt.BookCarrOrderNumber.ToString
                    FBAmounts.FbAmount = FBAmt.BookFinAPACtCost
                    If Me.Debug Then Log("Sending Freight Bill Data to GP ")
                    Dim ErrorString = ProcessFBCostsIntoGp(TMSSetting, c, FBAmounts, Me.AdminEmail)
                    If (ErrorString = "") Then
                        'If Me.Debug Then Log("Updating AP Export Status for Freight Bill # " & ProcessPymt.APBillNumber)
                        'blnRet = apExport.UpdateAPExportStatus(TMSSetting.TMSAuthCode, ProcessPymt.BookSHID, ProcessPymt.APBillNumber, True, ProcessPymt.APBillInvDate, ReturnMessage)
                    Else
                        If (ErrorString <> "Error") Then
                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, ProcessDataReturnValues.nglDataValidationFailure, IntegrationModule.APExport, ErrorString, False)
                            LogError("There was an error processing Freight Bill Costs SQL Update.", ErrorString, Me.GroupEmail)
                        End If
                    End If
                    FBAmounts = Nothing
                Next
            Else
                If Verbose Then Log("No Aggregated AP Export Updates to Process")
                blnRet = True
            End If

            If Debug Then Log("Process APExport Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected APExport Integration Error", Source & " Unexpected Integration Error! Could not import APExport information:  " & ex.Message, AdminEmail)
            blnRet = False
        End Try

        Return blnRet
    End Function


    ''' <summary>
    ''' Process the AP Integration Data
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <param name="c"></param>
    ''' <param name="GPPMHeader"></param>
    ''' <param name="AdminEmail"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' Modified by RHR for v-7.0.5.102 11/28/2016
    '''   added GPFunctionsGetCurrencyID configuration setting
    '''   added FreightAmount and set Purchase Amount to zero
    ''' Modified by RHR for v-7.0.6.104 on 08/28/2027 added using to file activity for auto dispose
    ''' </remarks>
    Public Function ProcessAPImportIntoGp(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByRef c As clsDefaultIntegrationConfiguration, ByVal GPPMHeader As GPDataIntegrationSTructure.GPPMHeaderData, ByVal AdminEmail As String) As String

        Dim EConnectStr As String = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        'Econnect must use active directory connection string
        Dim strGPEConnectionString As String = EConnectStr
        'all other connections can use sql authentication
        If Not String.IsNullOrWhiteSpace(TMSSetting.ERPUser) AndAlso Not String.IsNullOrWhiteSpace(TMSSetting.ERPPassword) Then
            EConnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";User ID=" & TMSSetting.ERPUser & ";Password=" & TMSSetting.ERPPassword
        End If


        'Dim EConnectStr As String = "Data Source=" & TMSSetting.ERPCertificate.ToString & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        Dim ReturnValue As String = ""
        Dim serializer As New XmlSerializer(GetType(eConnectType))
        Dim eConnect As New eConnectType
        Dim PMHeader As New taPMTransactionInsert
        'Dim NewCustomer As New taUpdateCreateCustomerRcd
        'Dim UserDefinedRec As New taSopUserDefined
        Dim PMTransType As New PMTransactionType
        Dim XLMFileToUse As String
        Dim sCustomerDocument As String
        Dim ErrorString As String = ""
        Dim Counter As Integer = 0
#Disable Warning BC42024 ' Unused local variable: 'ErrorFileToWrite'.
        Dim ErrorFileToWrite As System.IO.StreamWriter
#Enable Warning BC42024 ' Unused local variable: 'ErrorFileToWrite'.
        Dim SuccessFlag As Boolean = True
        Dim DocType As Integer = 1
        Dim GLLineCount As Integer = 0
        Dim VendorID As String = ""
        Dim VoucherNumber As String = ""
        Dim TransDescription As String = ""
        Dim PONumber As String = ""
        Dim BatchNumber As String = ""
        Dim GetFunctions As New GPFunctions(c) 'Modified by RHR for v-7.0.5.102 10/14/2016
        Dim GetVoucherNumber As New GetNextDocNumbers
        Dim PayablesCreditGL As String = ""
        Dim FoundVendor As New GPObject.GPDataIntegrationSTructure.VendorFound
        Dim VendorCreated As Boolean = False


        Try

            BatchNumber = GetFunctions.GetGPPMBatch()

            XLMFileToUse = "C:\Users\Public\Documents\APXML.xml"

            'Add a check for existing voucher

            If (SuccessFlag) Then

                Try

                    Using e As New eConnectMethods

                        Try

                            Try

                                FoundVendor = GetFunctions.FindVendor(EConnectStr, TMSSetting, GPPMHeader.VendorID)

                                If (FoundVendor.GPCompany <> "NA" And FoundVendor.GPCompany <> "Found") Then

                                    VendorCreated = CreateVendor(FoundVendor, TMSSetting, strGPEConnectionString)

                                End If

                                VoucherNumber = GetVoucherNumber.GetPMNextVoucherNumber(IncrementDecrement.Increment, EConnectStr)

                                With PMHeader

                                    .BACHNUMB = BatchNumber
                                    .VENDORID = GPPMHeader.VendorID
                                    VendorID = GPPMHeader.VendorID
                                    .VCHNUMWK = VoucherNumber
                                    'VoucherNumber = FieldValues(1)

                                    .DOCTYPE = GPPMHeader.DocumentType

                                    .DOCNUMBR = GPPMHeader.InvoiceNumber
                                    .DOCDATE = GPPMHeader.DocumentDate
                                    .DOCAMNT = GPPMHeader.DocumentAmount
                                    'Modified by RHR for v-7.0.5.102 on 11/28/2016
                                    'added FreightAmount and set Purchase Amount to zero

                                    Select Case c.GPAPAmountField

                                        Case "PO"

                                            .PRCHAMNT = GPPMHeader.FreightAmount
                                            .FRTAMNT = 0

                                        Case Else

                                            .PRCHAMNT = 0
                                            .FRTAMNT = GPPMHeader.FreightAmount

                                    End Select
                                    '.PRCHAMNT = 0
                                    '.FRTAMNT = GPPMHeader.FreightAmount
                                    .CHRGAMNT = GPPMHeader.DocumentAmount
                                    .PORDNMBR = GPPMHeader.PONumber
                                    .TRXDSCRN = GPPMHeader.TransactionDescription
                                    .CREATEDIST = 0
                                    .CURNCYID = c.GPFunctionsGetCurrencyID 'Modified by RHR for v-7.0.5.102 11/28/2016
                                End With

                                'Populate the RMCustomerMasterType schema with the taUpdateCreateCustomerRcd XML node object
                                PMTransType.taPMTransactionInsert = PMHeader

                                Dim PMLines(1) As taPMDistribution_ItemsTaPMDistribution

                                Dim PMLine As New taPMDistribution_ItemsTaPMDistribution

                                With PMLine

                                    .DOCTYPE = GPPMHeader.DocumentType
                                    .VCHRNMBR = VoucherNumber
                                    .VENDORID = VendorID
                                    .DSTSQNUM = 1
                                    ' Updated by SEM as the GL distirubtion was incorrect.  the original value of 6 is for purchasing distributioon.  Since the transaction amount is loaded into freight, the update to a 9 - Freight distribution is correcdt
                                    Select Case c.GPAPAmountField

                                        Case "PO"

                                            .DISTTYPE = 6

                                        Case Else

                                            .DISTTYPE = 9

                                    End Select

                                    .DistRef = ""
                                    .ACTNUMST = GPPMHeader.DebitGL
                                    .DEBITAMT = GPPMHeader.DocumentAmount
                                    .CRDTAMNT = 0

                                End With

                                PMLines(0) = PMLine

                                'Dim PMLine1 As New taPMDistribution_ItemsTaPMDistribution
                                Dim PMLine1 As New Microsoft.Dynamics.GP.eConnect.Serialization.taPMDistribution_ItemsTaPMDistribution

                                With PMLine1

                                    .DOCTYPE = GPPMHeader.DocumentType
                                    .VCHRNMBR = VoucherNumber
                                    .VENDORID = VendorID
                                    .DSTSQNUM = 2
                                    .DISTTYPE = 2
                                    .DistRef = ""
                                    .ACTNUMST = GetFunctions.GetPayableCreditGL(EConnectStr, ErrorString)
                                    .DEBITAMT = 0
                                    .CRDTAMNT = GPPMHeader.DocumentAmount

                                End With

                                PMLines(1) = PMLine1

                                PMLine = Nothing
                                PMLine1 = Nothing

                                PMTransType.taPMDistribution_Items = PMLines


                            Catch ex As Exception

                                ReturnValue = "Invoice #:  " & GPPMHeader.InvoiceNumber & Chr(13) & " Main Econnect Error 6, Inv :" & GPPMHeader.InvoiceNumber & " and error: " & ex.Message & Chr(13) & Chr(13)

                            End Try


                            'Populate the eConnect XML document object with the RMCustomerMasterType schema object
                            ReDim eConnect.PMTransactionType(0)
                            eConnect.PMTransactionType(0) = PMTransType

                            'Modified by RHR for v-7.0.6.104 on 08/28/2027 added using to file activity for auto dispose
                            Using fs As New FileStream(XLMFileToUse, FileMode.Create)
                                Using writer As New XmlTextWriter(fs, New UTF8Encoding)
                                    ' Use the XmlTextWriter to serialize the eConnect XML document object to the file
                                    serializer.Serialize(writer, eConnect)
                                    writer.Close()
                                End Using
                            End Using



                            Try

                                'Load the customer XML into an XML document object
                                Dim xmldoc As New Xml.XmlDocument
                                xmldoc.Load(XLMFileToUse)

                                'Use the XML document to create a string
                                sCustomerDocument = xmldoc.OuterXml

                                'Create the new customer in Microsoft Dynamics GP.
                                'e.CreateEntity(strGPEConnectionString, sCustomerDocument)
                                'e.eConnect_EntryPoint(sConnectionString, EnumTypes.ConnectionStringType.SqlClient, sCustomerDocument, EnumTypes.SchemaValidationType.None)
                                If (PayablesCreditGL = "Error") Then

                                    ReturnValue = PayablesCreditGL

                                Else

                                    e.CreateEntity(strGPEConnectionString, sCustomerDocument)

                                    ReturnValue = ""

                                End If

                            Catch ex As Exception
                                If ex.Message.Contains("Document Number (DOCNUMBR) already exists") Then
                                    ReturnValue = ""
                                Else
                                    ReturnValue = "Invoice #:  " & GPPMHeader.InvoiceNumber & Chr(13) & " Main Econnect Error 1:  " & ex.Message & Chr(13) & Chr(13)
                                End If

                                'ReturnValue = "Invoice #:  " & GPPMHeader.InvoiceNumber & Chr(13) & " Main Econnect Error 1:  " & ex.Message & Chr(13) & Chr(13)

                            End Try

                        Catch ex As Exception

                            ReturnValue = "Invoice #:  " & GPPMHeader.InvoiceNumber & Chr(13) & " Main Econnect Error 2, Inv :" & GPPMHeader.InvoiceNumber & " and error: " & Err.Description & Chr(13) & Chr(13)

                        End Try

                    End Using

                Catch ex As Exception

                    ReturnValue = "Invoice #:  " & GPPMHeader.InvoiceNumber & Chr(13) & "Main Econnect Error 3, Inv :" & GPPMHeader.InvoiceNumber & " and error: " & Err.Description & Chr(13) & Chr(13)

                End Try

            End If

        Catch ex As Exception

            ReturnValue = "Error"
            LogError(Source & " Error!  Unexpected GP ProcessAPImportIntoGP Error", Source & " Could not process any get freight payment requests; the actual error is:  " & ex.Message, AdminEmail)

        End Try

        Return ReturnValue

    End Function

    ''' <summary>
    ''' Do not use this method it uses the wrong order number 
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <param name="c"></param>
    ''' <param name="FBCosts"></param>
    ''' <param name="AdminEmail"></param>
    ''' <returns></returns>
    Public Function ProcessFBCostsIntoGp(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByRef c As clsDefaultIntegrationConfiguration, ByVal FBCosts As GPDataIntegrationSTructure.FreightBillCosts, ByVal AdminEmail As String) As String

        Dim EConnectStr As String = "Data Source=" & TMSSetting.ERPCertificate.ToString & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        If Not String.IsNullOrWhiteSpace(TMSSetting.ERPUser) AndAlso Not String.IsNullOrWhiteSpace(TMSSetting.ERPPassword) Then
            EConnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";User ID=" & TMSSetting.ERPUser & ";Password=" & TMSSetting.ERPPassword
        End If
        Dim ReturnValue As String = ""
        Dim SOPUserDefinedCount As Integer = 0

        Dim SQLConn = New SqlConnection(EConnectStr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader

        Try

            'For Each Ord In OrderNumbers
            SQLConn.Open()

            SQLCom.Connection = SQLConn

            If (IsNothing(FBCosts.OrderNumber)) Then

                LogError(Source & "  Order Number is Nothing", Source, AdminEmail)

                ReturnValue = "Error"

            Else

                SQLCom.CommandText = String.Format(c.GPFunctionCheckForSOPUserDefinedRecord, Trim(FBCosts.OrderNumber.ToString))
                'SQLCom.CommandText = "select count(*) from SOP10106 where SOPTYPE = 2 and SOPNUMBE = '" & FBCosts.OrderNumber.ToString & "'"

                TblRec = SQLCom.ExecuteReader

                TblRec.Read()

                If (TblRec.HasRows) Then

                    SOPUserDefinedCount = CInt(TblRec(0).ToString)

                End If

                TblRec.Close()

                If (SOPUserDefinedCount > 0) Then

                    SQLCom.CommandText = String.Format(c.GPFunctionUpdateUserDefinedRecord, Trim(FBCosts.OrderNumber.ToString), FBCosts.FbAmount.ToString)
                    'SQLCom.CommandText = "update SOP10106 set USRDEF03 = " & FBCosts.FbAmount.ToString & " where SOPTYPE = 2 and SOPNUMBE = '" & FBCosts.OrderNumber.ToString & "'"
                    SQLCom.ExecuteNonQuery()

                Else

                    'LogError(Source & " Error!  Unexpected GP Calculate SOP Cost Per Order Error", Source & " Could Not find any user defined record In SOP10106", AdminEmail)

                    'ReturnValue = "Error"
                    SQLCom.CommandText = String.Format(c.GPFunctionInsertUserDefinedRecord, Trim(FBCosts.OrderNumber.ToString), FBCosts.FbAmount.ToString)
                    'SQLCom.CommandText = "insert into SOP10106 (SOPTYPE, SOPNUMBE, USRDEF03, CMMTTEXT) select 2, '" & FBCosts.OrderNumber.ToString & "', '" & FBCosts.FbAmount.ToString & "', ' '"
                    SQLCom.ExecuteNonQuery()

                End If

            End If

            SQLCom.Dispose()

            SQLConn.Dispose()

        Catch ex As Exception

            LogError(Source & " Error!  Unexpected GP Calculate SOP Cost Per Order Error", Source & " Could Not process any calculation requests; the actual Error Is:  " & ex.Message & " - FB#:  " & FBCosts.OrderNumber.ToString & ", cost:  " & FBCosts.FbAmount.ToString, AdminEmail)
            'Throw

            ReturnValue = "Error"

        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                    TblRec.Close()
                End If

            Catch ex1 As Exception

            End Try

            Try
                If Not SQLCom Is Nothing Then
                    SQLCom.Dispose()
                End If
            Catch ex2 As Exception

            End Try

            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then
                    SQLConn.Close()
                End If

                SQLConn.Dispose()

            Catch ex3 As Exception

            End Try

        End Try

        Return ReturnValue

    End Function


    ''' <summary>
    ''' The index of strInvoiceNumbers must match the index of dblCostPerPound exactly
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <param name="c"></param>
    ''' <param name="lGPOrderInvoices"></param>
    ''' <param name="AdminEmail"></param>
    ''' <returns></returns>
    Public Function ProcessCostPerPoundIntoGp(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByRef c As clsDefaultIntegrationConfiguration, ByVal lGPOrderInvoices As List(Of clsGPOrderInvoice), ByVal AdminEmail As String) As String

        Dim EConnectStr As String = "Data Source=" & TMSSetting.ERPCertificate.ToString & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        If Not String.IsNullOrWhiteSpace(TMSSetting.ERPUser) AndAlso Not String.IsNullOrWhiteSpace(TMSSetting.ERPPassword) Then
            EConnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";User ID=" & TMSSetting.ERPUser & ";Password=" & TMSSetting.ERPPassword
        End If
        Dim ReturnValue As String = ""
        Dim SOPUserDefinedCount As Integer = 0

        Dim SQLConn = New SqlConnection(EConnectStr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader

        Try

            'For Each Ord In OrderNumbers
            SQLConn.Open()

            SQLCom.Connection = SQLConn

            If (lGPOrderInvoices Is Nothing OrElse lGPOrderInvoices.Count < 1) Then

                If Me.Debug Then Log(Source & "  No Invoice Numbers to process")

                ReturnValue = "Success"

            Else
                If Me.Debug Then Log(Source & "  Processing" & lGPOrderInvoices.Count.ToString() & " Cost Per Pound Invoices")
                Dim strMsg As String = ""
                Dim ReturnMessage As String = ""
                Dim RetVal As Integer = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                Dim oTMSService As New tmsintegrationservices.DTMSERPIntegration()
                oTMSService.Url = TMSSetting.TMSURI
                If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                    oTMSService.UseDefaultCredentials = True
                Else
                    oTMSService.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
                End If

                For Each oGPOrderInvoice In lGPOrderInvoices
                    If Not String.IsNullOrWhiteSpace(oGPOrderInvoice.OrderNumber) AndAlso Not String.IsNullOrWhiteSpace(oGPOrderInvoice.InvoiceNumber) Then
                        If Me.Debug Then Log(Source & "  Processing Cost Per Pound. Order Number: " & oGPOrderInvoice.OrderNumber & " Invoice Number: " & oGPOrderInvoice.InvoiceNumber)
                        'get the cost per pound
                        If oGPOrderInvoice.CompNumber = 0 Then
                            If Me.Debug Then Log(String.Format("{0} Getting Cost Per pound. Authcode: {1}, Order Nbr: {2}, Sequence Nbr: {3}, Comp Alpha: {4}, Legal Ent: {5}", Source, TMSSetting.TMSAuthCode, oGPOrderInvoice.OrderNumber, oGPOrderInvoice.OrderSequenceNumber, oGPOrderInvoice.CompAlphaCode, TMSSetting.LegalEntity))
                            oGPOrderInvoice.CostPerPound = oTMSService.GetCostPerPoundForOrder(TMSSetting.TMSAuthCode, oGPOrderInvoice.OrderNumber, oGPOrderInvoice.OrderSequenceNumber, oGPOrderInvoice.CompAlphaCode, TMSSetting.LegalEntity)
                        Else
                            If Me.Debug Then Log(String.Format("{0} Getting Cost Per pound. Authcode: {1}, Order Nbr: {2}, Sequence Nbr: {3}, Comp Nbr: {4}", Source, TMSSetting.TMSAuthCode, oGPOrderInvoice.OrderNumber, oGPOrderInvoice.OrderSequenceNumber, oGPOrderInvoice.CompNumber))
                            oGPOrderInvoice.CostPerPound = oTMSService.GetCostPerPoundForOrderByCompNumber(TMSSetting.TMSAuthCode, oGPOrderInvoice.OrderNumber, oGPOrderInvoice.OrderSequenceNumber, oGPOrderInvoice.CompNumber)
                        End If


                        If oGPOrderInvoice.CostPerPound <> 0 Then

                            SQLCom.CommandText = String.Format(c.GPFunctionCheckForSOPUserDefinedRecord, Trim(oGPOrderInvoice.InvoiceNumber))
                            'SQLCom.CommandText = "Select count(*) from SOP10106 where SOPTYPE = 3 And SOPNUMBE = '" & oGPOrderInvoice.InvoiceNumber.ToString & "'"

                            TblRec = SQLCom.ExecuteReader

                            TblRec.Read()

                            If (TblRec.HasRows) Then
                                SOPUserDefinedCount = CInt(TblRec(0).ToString)
                            End If

                            TblRec.Close()

                            If (SOPUserDefinedCount > 0) Then

                                SQLCom.CommandText = String.Format(c.GPFunctionUpdateUserDefinedRecord, Trim(oGPOrderInvoice.InvoiceNumber), oGPOrderInvoice.CostPerPound)
                                'SQLCom.CommandText = "update SOP10106 set USRDEF03 = " & oGPOrderInvoice.CostPerPound.ToString() & " where SOPTYPE = 3 and SOPNUMBE = '" & Trim(oGPOrderInvoice.InvoiceNumber) & "'"
                                SQLCom.ExecuteNonQuery()

                            Else

                                'LogError(Source & " Error!  Unexpected GP Calculate SOP Cost Per Order Error", Source & " Could Not find any user defined record In SOP10106", AdminEmail)

                                'ReturnValue = "Error"
                                SQLCom.CommandText = String.Format(c.GPFunctionInsertUserDefinedRecord, Trim(oGPOrderInvoice.InvoiceNumber), oGPOrderInvoice.CostPerPound)
                                'SQLCom.CommandText = "insert into SOP10106 (SOPTYPE, SOPNUMBE, USRDEF03, CMMTTEXT) select 3, '" & Trim(oGPOrderInvoice.InvoiceNumber) & "', '" &  oGPOrderInvoice.CostPerPound.ToString() & "', ' '"
                                SQLCom.ExecuteNonQuery()

                            End If
                            If Me.Debug Then Log(Source & " Running Query: " & SQLCom.CommandText)
                        Else
                            Log(Source & " Cost Per Pound is zero")
                        End If
                    Else
                        If Me.Debug Then Log(Source & "  Cannot Processing Cost Per Pound. Order Number:  " & oGPOrderInvoice.OrderNumber & " Invoice Number: " & oGPOrderInvoice.InvoiceNumber)
                    End If
                Next

            End If
            ReturnValue = "Success"
            SQLCom.Dispose()

            SQLConn.Dispose()

        Catch ex As Exception
            'toDo fix error message
            'LogError(Source & " Error!  Unexpected GP Calculate SOP Cost Per Order Error", Source & " Could Not process any calculation requests; the actual Error Is:  " & ex.Message & " - FB#:  " & FBCosts.OrderNumber.ToString & ", cost:  " & FBCosts.FbAmount.ToString, AdminEmail)
            'Throw
            If Me.Debug Or Me.Verbose Then
                LogError("There was an error updating the cost per pound  From Command Line", ex.ToString(), Me.AdminEmail)
            End If
            ReturnValue = "Error"

        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                    TblRec.Close()
                End If

            Catch ex1 As Exception

            End Try

            Try
                If Not SQLCom Is Nothing Then
                    SQLCom.Dispose()
                End If
            Catch ex2 As Exception

            End Try

            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then
                    SQLConn.Close()
                End If

                SQLConn.Dispose()

            Catch ex3 As Exception

            End Try

        End Try

        Return ReturnValue

    End Function


    Private Function fillItemDetails(ByRef SOPNumber As String, ByRef LineCols As List(Of String), ByRef LineRec As SqlDataReader, ByVal sLegalEntity As String, ByVal sLocationCode As String) As tmsintegrationservices.clsBookDetailObject705
        Dim oBookDetail As New tmsintegrationservices.clsBookDetailObject705
        oBookDetail.ItemPONumber = SOPNumber
        If (LineCols.Contains("ItemNumber")) Then oBookDetail.ItemNumber = Trim(LineRec("ItemNumber").ToString())
        If (LineCols.Contains("ItemCost")) Then Double.TryParse(LineRec("ItemCost").ToString(), oBookDetail.ItemCost)
        If (LineCols.Contains("QtyOrdered")) Then GPFunctions.TryParseInt(LineRec("QtyOrdered").ToString(), oBookDetail.QtyOrdered)
        If (LineCols.Contains("Weight")) Then Double.TryParse(LineRec("Weight").ToString(), oBookDetail.Weight)
        If (LineCols.Contains("FixOffInvAllow")) Then Double.TryParse(LineRec("FixOffInvAllow").ToString(), oBookDetail.FixOffInvAllow)
        If (LineCols.Contains("FixFrtAllow")) Then Double.TryParse(LineRec("FixFrtAllow").ToString(), oBookDetail.FixFrtAllow)
        If (LineCols.Contains("FreightCost")) Then Double.TryParse(LineRec("FreightCost").ToString(), oBookDetail.FreightCost)
        If (LineCols.Contains("ItemCost")) Then Double.TryParse(LineRec("ItemCost").ToString(), oBookDetail.ItemCost)
        If (LineCols.Contains("Cube")) Then GPFunctions.TryParseInt(LineRec("Cube").ToString(), oBookDetail.Cube)
        If (LineCols.Contains("Pack")) Then GPFunctions.TryParseInt(LineRec("Pack").ToString(), oBookDetail.Pack)
        If (LineCols.Contains("Size")) Then oBookDetail.Size = Trim(LineRec("Size").ToString())
        If (LineCols.Contains("Description")) Then oBookDetail.Description = Trim(LineRec("Description").ToString())
        If (LineCols.Contains("Hazmat")) Then oBookDetail.Hazmat = Trim(LineRec("Hazmat").ToString())
        If (LineCols.Contains("Brand")) Then oBookDetail.Brand = Trim(LineRec("Brand").ToString())
        If (LineCols.Contains("CostCenter")) Then oBookDetail.CostCenter = Trim(LineRec("CostCenter").ToString())
        If (LineCols.Contains("LotNumber")) Then oBookDetail.LotNumber = Trim(LineRec("LotNumber").ToString())
        If (LineCols.Contains("LotExpirationDate")) Then oBookDetail.LotExpirationDate = Trim(LineRec("LotExpirationDate").ToString())
        If (LineCols.Contains("GTIN")) Then oBookDetail.GTIN = Trim(LineRec("GTIN").ToString())
        If (LineCols.Contains("CustItemNumber")) Then oBookDetail.CustItemNumber = Trim(LineRec("CustItemNumber").ToString())
        oBookDetail.CustomerNumber = "0" 'Modified by RHR for v-7.0.5.102 10/17/2016 we do not use the CUSTNMBR here  HeaderRec("CUSTNMBR").ToString
        oBookDetail.POOrderSequence = 0
        If (LineCols.Contains("PalletType")) Then oBookDetail.PalletType = Trim(LineRec("PalletType").ToString())
        If (LineCols.Contains("POItemHazmatTypeCode")) Then oBookDetail.POItemHazmatTypeCode = Trim(LineRec("POItemHazmatTypeCode").ToString())
        If (LineCols.Contains("POItem49CFRCode")) Then oBookDetail.POItem49CFRCode = Trim(LineRec("POItem49CFRCode").ToString())
        If (LineCols.Contains("POItemIATACode")) Then oBookDetail.POItemIATACode = Trim(LineRec("POItemIATACode").ToString())
        If (LineCols.Contains("POItemDOTCode")) Then oBookDetail.POItemDOTCode = Trim(LineRec("POItemDOTCode").ToString())
        If (LineCols.Contains("POItemMarineCode")) Then oBookDetail.POItemMarineCode = Trim(LineRec("POItemMarineCode").ToString())
        If (LineCols.Contains("POItemNMFCClass")) Then oBookDetail.POItemNMFCClass = Trim(LineRec("POItemNMFCClass").ToString())
        If (LineCols.Contains("POItemFAKClass")) Then oBookDetail.POItemFAKClass = Trim(LineRec("POItemFAKClass").ToString())
        If (LineCols.Contains("POItemLimitedQtyFlag")) Then Boolean.TryParse(LineRec("POItemLimitedQtyFlag").ToString(), oBookDetail.POItemLimitedQtyFlag)
        If (LineCols.Contains("POItemPallets")) Then Double.TryParse(LineRec("POItemPallets").ToString(), oBookDetail.POItemPallets)
        If (LineCols.Contains("POItemTies")) Then Double.TryParse(LineRec("POItemTies").ToString(), oBookDetail.POItemTies)
        If (LineCols.Contains("POItemHighs")) Then Double.TryParse(LineRec("POItemHighs").ToString(), oBookDetail.POItemHighs)
        If (LineCols.Contains("POItemQtyPalletPercentage")) Then Double.TryParse(LineRec("POItemQtyPalletPercentage").ToString(), oBookDetail.POItemQtyPalletPercentage)
        If (LineCols.Contains("POItemQtyLength")) Then Double.TryParse(LineRec("POItemQtyLength").ToString(), oBookDetail.POItemQtyLength)
        If (LineCols.Contains("POItemQtyWidth")) Then Double.TryParse(LineRec("POItemQtyWidth").ToString(), oBookDetail.POItemQtyWidth)
        If (LineCols.Contains("POItemQtyHeight")) Then Double.TryParse(LineRec("POItemQtyHeight").ToString(), oBookDetail.POItemQtyHeight)
        If (LineCols.Contains("POItemStackable")) Then Boolean.TryParse(LineRec("POItemStackable").ToString(), oBookDetail.POItemStackable)
        If (LineCols.Contains("POItemLevelOfDensity")) Then GPFunctions.TryParseInt(LineRec("POItemLevelOfDensity").ToString(), oBookDetail.POItemLevelOfDensity)
        oBookDetail.POItemCompLegalEntity = Trim(sLegalEntity)
        oBookDetail.POItemCompAlphaCode = Trim(sLocationCode)
        'oBookDetail.POItemCompAlphaCode = "WAREHOUSE"
        If (LineCols.Contains("POItemNMFCSubClass")) Then oBookDetail.POItemNMFCSubClass = Trim(LineRec("POItemNMFCSubClass").ToString())
        If (LineCols.Contains("POItemUser1")) Then oBookDetail.POItemUser1 = Trim(LineRec("POItemUser1").ToString())
        If (LineCols.Contains("POItemUser2")) Then oBookDetail.POItemUser2 = Trim(LineRec("POItemUser2").ToString())
        If (LineCols.Contains("POItemUser3")) Then oBookDetail.POItemUser3 = Trim(LineRec("POItemUser3").ToString())
        If (LineCols.Contains("POItemUser4")) Then oBookDetail.POItemUser4 = Trim(LineRec("POItemUser4").ToString())
        If (LineCols.Contains("POItemWeightUnitOfMeasure")) Then oBookDetail.POItemWeightUnitOfMeasure = Trim(LineRec("POItemWeightUnitOfMeasure").ToString())
        If (LineCols.Contains("POItemCubeUnitOfMeasure")) Then oBookDetail.POItemCubeUnitOfMeasure = Trim(LineRec("POItemCubeUnitOfMeasure").ToString())
        If (LineCols.Contains("POItemDimensionUnitOfMeasure")) Then oBookDetail.POItemDimensionUnitOfMeasure = Trim(LineRec("POItemDimensionUnitOfMeasure").ToString())
        Return oBookDetail

    End Function

    ''' <summary>
    ''' Fill the Book Header Object with data from GP HeaderRec
    ''' </summary>
    ''' <param name="oGPFunction"></param>
    ''' <param name="HeaderRec"></param>
    ''' <param name="EconnectStr"></param>
    ''' <param name="ProAbb"></param>
    ''' <param name="sErrors"></param>
    ''' <param name="c"></param>
    ''' <param name="sLegalEntity"></param>
    ''' <param name="SOPType"></param>
    ''' <param name="LaneNumber"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 'Modified by RHR for v-7.0.6.105 on 10/13/2017 added variable for LaneNumber 
    ''' </remarks>
    Private Function fillBookHeader(ByRef oGPFunction As GPFunctions, ByRef HeaderRec As SqlDataReader, ByRef EconnectStr As String, ByRef ProAbb As String, ByRef sErrors As String, ByRef c As clsDefaultIntegrationConfiguration, ByVal sLegalEntity As String, ByVal SOPType As Int16, ByVal LaneNumber As String) As tmsintegrationservices.clsBookHeaderObject705
        Dim oBookHeader As New tmsintegrationservices.clsBookHeaderObject705
        ' Insert TMSHeader

        'Modified by RHR for v-7.0.6.105 on 10/13/2017 added variable for LaneNumber 
        oBookHeader.POVendor = LaneNumber 'oGPFunction.CalculateLane(EconnectStr, 1, HeaderRec("SOPNumbe").ToString, 16384, HeaderRec("SOPNumbe").ToString, ProAbb, SOPType, sErrors)
        oBookHeader.POFrt = oGPFunction.GetShippingMethod(EconnectStr, 1, HeaderRec("SOPNumbe").ToString, 16384, HeaderRec("SOPNumbe").ToString, SOPType)
        oBookHeader.POWgt = oGPFunction.GetSOPTotalWeight(EconnectStr, HeaderRec("SOPNUMBE").ToString, SOPType)
        oBookHeader.POQty = CInt(oGPFunction.GetSOPQuantity(EconnectStr, HeaderRec("SOPNUMBE").ToString, SOPType))
        oBookHeader.POPallets = CInt(oGPFunction.CalculatePallets(EconnectStr, HeaderRec("SOPNUMBE").ToString, 1, Me.AdminEmail, SOPType))
        oBookHeader.POTemp = oGPFunction.GetTemp(EconnectStr, 1, HeaderRec("SOPNUMBE").ToString, SOPType)
        Select Case SOPType
            Case 2
                oBookHeader.PONumber = Trim(HeaderRec("SOPNUMBE").ToString())
                oBookHeader.POStatusFlag = 5
            Case 3
                oBookHeader.PONumber = Trim(HeaderRec("ORIGNUMB").ToString())
                oBookHeader.POStatusFlag = 3
        End Select

        oBookHeader.POdate = Trim(HeaderRec("DocDate").ToString())
        oBookHeader.POShipdate = Trim(HeaderRec("DocDate").ToString())

        oBookHeader.POBuyer = ""

        Double.TryParse(HeaderRec("FRTAMNT").ToString(), oBookHeader.POTotalFrt)
        Double.TryParse(HeaderRec("EXTDCOST").ToString, oBookHeader.POTotalCost)
        'If Me.Debug Then Log("Getting Order Weight For: " & HeaderRec("SOPNUMBE").ToString())

        oBookHeader.POCube = 0

        oBookHeader.POLines = 0
        oBookHeader.POConfirm = False
        oBookHeader.PODefaultCustomer = "0"
        oBookHeader.PODefaultCarrier = 0
        oBookHeader.POReqDate = Trim(HeaderRec("ReqShipDate").ToString())
        oBookHeader.POShipInstructions = ""
        oBookHeader.POCooler = False
        oBookHeader.POFrozen = False
        oBookHeader.PODry = False

        oBookHeader.POCarType = ""
        oBookHeader.POShipVia = ""
        oBookHeader.POShipViaType = ""
        oBookHeader.POConsigneeNumber = ""
        oBookHeader.POCustomerPO = Trim(HeaderRec("CSTPONBR").ToString())
        oBookHeader.POOtherCosts = 0
        oBookHeader.POOrderSequence = 0
        oBookHeader.POChepGLID = ""
        oBookHeader.POCarrierEquipmentCodes = ""
        oBookHeader.POCarrierTypeCode = ""
        oBookHeader.POPalletPositions = ""
        ' Modified by RHR for v-7.0.5.102 on 10/05/20126
        ' We do not use the ReqShipDate for appointment informaiton 
        'oBookHeader.POSchedulePUDate = HeaderRec("ReqShipDate").ToString
        oBookHeader.POSchedulePUDate = ""
        oBookHeader.POSchedulePUTime = ""
        'oBookHeader.POScheduleDelDate = HeaderRec("ReqShipDate").ToString
        oBookHeader.POScheduleDelDate = ""
        oBookHeader.POSCheduleDelTime = ""
        oBookHeader.POActPUDate = ""
        oBookHeader.POActPUTime = ""
        oBookHeader.POActDelDate = ""
        oBookHeader.POActDelTime = ""
        oBookHeader.POOrigCompNumber = ""
        'If Me.Debug Then Log("Reading Address Info For Order Number: " & SOPNumber)
        Dim PorDAddressInfo As GPDataIntegrationSTructure.FromAddress = oGPFunction.GetPorDAddress(EconnectStr, HeaderRec("LOCNCODE"))
        If (PorDAddressInfo.AddressID = "No Address") Then
            LogError("No Address found", "There was no address found for SOP Order: " & HeaderRec("SOPNUMBE").ToString, Me.AdminEmail)
        Else
            oBookHeader.POOrigName = Trim(PorDAddressInfo.AddressName)
            oBookHeader.POOrigCompAlphaCode = Trim(PorDAddressInfo.AddressID)
            oBookHeader.POOrigAddress1 = Trim(PorDAddressInfo.Address1)
            oBookHeader.POOrigAddress2 = Trim(PorDAddressInfo.Address2)
            oBookHeader.POOrigAddress3 = Trim(PorDAddressInfo.Address3)
            oBookHeader.POOrigCity = Trim(PorDAddressInfo.City)
            oBookHeader.POOrigState = Trim(PorDAddressInfo.State)
            oBookHeader.POOrigCountry = Trim(If(String.IsNullOrWhiteSpace(c.GPFunctionsForceDefaultCountry), PorDAddressInfo.Country, c.GPFunctionsForceDefaultCountry))
            oBookHeader.POOrigZip = Trim(PorDAddressInfo.ZipCode)
            oBookHeader.POOrigContactPhone = Trim(PorDAddressInfo.Phone)
            oBookHeader.POOrigContactPhoneExt = ""
            oBookHeader.POOrigContactFax = Trim(PorDAddressInfo.Fax)
        End If

        oBookHeader.PODestCompNumber = ""
        oBookHeader.PODestCompAlphaCode = Trim(HeaderRec("PRSTADCD").ToString())
        oBookHeader.PODestName = Trim(HeaderRec("ShipToName").ToString())
        oBookHeader.PODestAddress1 = Trim(HeaderRec("ADDRESS1").ToString())
        oBookHeader.PODestAddress2 = Trim(HeaderRec("ADDRESS2").ToString())
        oBookHeader.PODestAddress3 = Trim(HeaderRec("ADDRESS1").ToString())
        oBookHeader.PODestCity = Trim(HeaderRec("CITY").ToString())
        oBookHeader.PODestState = Trim(HeaderRec("STATE").ToString())
        oBookHeader.PODestCountry = Trim(If(String.IsNullOrWhiteSpace(c.GPFunctionsForceDefaultCountry), HeaderRec("COUNTRY").ToString(), c.GPFunctionsForceDefaultCountry))
        oBookHeader.PODestZip = Trim(HeaderRec("ZIPCODE").ToString())
        oBookHeader.PODestContactPhone = Trim(HeaderRec("PHNUMBR1").ToString())
        oBookHeader.PODestContactPhoneExt = ""
        oBookHeader.PODestContactFax = Trim(HeaderRec("FAXNUMBR").ToString())
        oBookHeader.POInbound = False
        oBookHeader.POPalletExchange = False
        oBookHeader.POPalletType = ""
        oBookHeader.POComments = ""
        oBookHeader.POCommentsConfidential = ""
        oBookHeader.PODefaultRouteSequence = 0
        'Modified by RHR v-7.0.5.102 09/26/2016 removed code to populate the PORouteGuideNumber with the Lane Number
        'as this is not required.
        'oBookHeader.PORouteGuideNumber = GPFunctions.CalculateLane(EconnectStr, 1, HeaderRec("SOPNumbe").ToString, 16384, HeaderRec("SOPNumbe").ToString, ProAbb, SOPType, sErrors)
        oBookHeader.PORouteGuideNumber = ""
        oBookHeader.POCompLegalEntity = Trim(sLegalEntity)
        oBookHeader.POCompAlphaCode = Trim(HeaderRec("LOCNCODE"))
        oBookHeader.POModeTypeControl = oGPFunction.GetTransporationMode(HeaderRec("SOPNUMBE").ToString, "SOP")
        oBookHeader.POUser1 = ""
        oBookHeader.POUser2 = ""
        oBookHeader.POUser3 = ""
        oBookHeader.POUser4 = ""
        oBookHeader.POAPGLNumber = ""
        Return oBookHeader
    End Function


    ''' <summary>
    ''' Process the GP Sales Orders
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <param name="c"></param>
    ''' <param name="SOPOrderNumbers"></param>
    ''' <param name="SOPType"></param>
    ''' <param name="TMSCountToProcess"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' Modified by RHR for v-7.0.5.102 on 11/10/2016
    '''   we now use web services for booking integration to correct problem with
    '''   auto processing.  DLL direct requires a reference to WCF HTTP and Net.TCP 
    '''   services to work,  the current configuration does not support this.  However,
    '''   it is built into the web service interface.
    ''' Modified by RHR for v-7.0.6.104 on 08/28/2027 added finally statement to dispose of db connections
    ''' Modified by RHR to troubleshoot memory leak minimized functionality no save to TMS
    ''' </remarks>
    Public Function ProcessGPSOPOrdersUnsure(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByRef c As clsDefaultIntegrationConfiguration, ByVal SOPOrderNumbers As List(Of GPDataIntegrationSTructure.SOPOrders), ByVal SOPType As Int16, ByVal TMSCountToProcess As Int16) As Boolean

        Dim ReturnValue As Boolean = True
        Dim EconnectStr As String = ""
        Dim GetAppSettings = New AppSettingsReader
        Dim LaneNumber As String = ""
        Dim GPFunctions As New GPFunctions(c) 'Modified by RHR for v-7.0.5.102 10/14/2016
        Dim SOPCounter As Int16 = 0
        Dim OrderChanged As Boolean = False
        Dim OrdChangeStr As String = ""

        ' Set all the variables

        If Not String.IsNullOrWhiteSpace(TMSSetting.ERPUser) AndAlso Not String.IsNullOrWhiteSpace(TMSSetting.ERPPassword) Then
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";User ID=" & TMSSetting.ERPUser & ";Password=" & TMSSetting.ERPPassword
        Else
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        End If
        'EconnectStr = "Data Source=" & GetAppSettings.GetValue("SQLServer") & "; Initial Catalog=" & GPCompany & "Integrated Security=SSPI;Trusted_Connection=True"
        'EconnectStr = "Data Source=" & TMSSetting.ERPCertificate.ToString & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        'EconnectStr = "Data Source=NGLGP2013R2; Initial Catalog=TWO;Integrated Security=SSPI;Trusted_Connection=True"

        Dim HeaderConn = New SqlConnection(EconnectStr)
        Dim HeaderCom As New SqlCommand
        Dim HeaderRec As SqlDataReader
        Dim LineConn = New SqlConnection(EconnectStr)
        Dim LineCom As New SqlCommand
        Dim LineRec As SqlDataReader
        Dim Counter As Integer = 0
        Dim NumberofLines As Integer = 0
        Dim SOPNumber As String = ""
        Dim sIntegrationErrors As New List(Of String)
        Dim sLogMsgs As New List(Of String)
        Dim PorDAddressInfo As New GPDataIntegrationSTructure.FromAddress
        Dim ProAbb As String = ""
        Dim LaneValue As String = ""
        Dim LineNumber As Integer = 0
        Dim sErrors As String = ""

        Try


            HeaderConn.Open()

            HeaderCom.Connection = HeaderConn

            LineConn.Open()

            LineCom.Connection = LineConn


            'Integrate Sales Order
            'Create TMS Header & Line
            'Modified by RHR for v-7.0.5.102 on 11/10/2016
            'Dim oBookHeaders As New List(Of TMS.clsBookHeaderObject705)
            'Dim oBookDetails As New List(Of TMS.clsBookDetailObject705)
            Dim oBookHeaders As New List(Of tmsintegrationservices.clsBookHeaderObject705)
            Dim oBookDetails As New List(Of tmsintegrationservices.clsBookDetailObject705)

            'Modified by RHR for v-7.0.5.102 on 11/10/2016
            'Not needed with web services
            'Dim oConfig As New UserConfiguration()
            'With oConfig
            '    .AdminEmail = Me.AdminEmail
            '    .AutoRetry = Me.AutoRetry
            '    .Database = Me.Database
            '    .DBServer = Me.DBServer
            '    .Debug = Me.Debug
            '    .FromEmail = Me.FromEmail
            '    .GroupEmail = Me.GroupEmail
            '    .LogFile = Me.LogFile
            '    .SMTPServer = Me.SMTPServer
            '    .UserName = "System Download"
            '    .WSAuthCode = "NGLSystem"
            '    .WCFAuthCode = "NGLSystem"
            '    .WCFURL = ""
            '    .WCFTCPURL = ""
            '    .ConnectionString = Me.ConnectionString

            'End With

            'Modified by RHR for v-7.0.5.102 on 11/10/2016
            'Dim oBookIntegration As New TMS.clsBook(oConfig)
            Dim oBookIntegration As New tmsintegrationservices.DTMSERPIntegration()
            oBookIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oBookIntegration.UseDefaultCredentials = True
            Else
                oBookIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If

            Dim SOOrdersProcessed As New List(Of String)
            Try

                For Each SOPOrder In SOPOrderNumbers
                    'Dim oBookHeader As New tmsintegrationservices.clsBookHeaderObject705

                    Select Case SOPType

                        Case 2

                            HeaderCom.CommandText = "select * from sop10100 where SOPTYPE = " & SOPType & " and SOPNUMBE = '" & SOPOrder.SOPOrders.ToString & "'"

                        Case 3

                            HeaderCom.CommandText = "select * from sop30200 where SOPTYPE = " & SOPType & " and SOPNUMBE = '" & SOPOrder.SOPOrders.ToString & "'"

                    End Select


                    HeaderRec = HeaderCom.ExecuteReader
                    Try
                        HeaderRec.Read()
                        If (HeaderRec.HasRows) Then
                            sErrors = ""
                            ProAbb = oBookIntegration.GetCompAbrevByNumberOrAlpha(TMSSetting.TMSAuthCode, 0, HeaderRec("LOCNCODE").ToString, "", sErrors)
                            If (String.IsNullOrWhiteSpace(sErrors) AndAlso String.IsNullOrWhiteSpace(ProAbb)) Then
                                ' skip order if failed
                                If Me.Debug Then
                                    Log("Could not process order: " & HeaderRec("SOPNUMBE").ToString() & " Due to a missing Company Pro Abb for Comp Alpha Code: " & HeaderRec("LOCNCODE").ToString())
                                End If
                                HeaderRec.Close()
                            Else
                                Select Case SOPType
                                    Case 2
                                        LaneValue = GPFunctions.CalculateLane(EconnectStr, 1, HeaderRec("SOPNumbe").ToString, 16384, HeaderRec("SOPNumbe").ToString, ProAbb, SOPType, sErrors)
                                    Case 3
                                        LaneValue = GPFunctions.CalculateLane(EconnectStr, 1, HeaderRec("SOPNumbe").ToString, 16384, HeaderRec("SOPNumbe").ToString, ProAbb, SOPType, sErrors)
                                End Select

                                If (LaneValue = "Error") Then
                                    If Me.Verbose Or Me.Debug Then
                                        Log("Error when processing Lane for order: " & HeaderRec("SOPNUMBE").ToString())
                                    End If
                                    HeaderRec.Close()
                                Else
                                    Select Case SOPType
                                        Case 2
                                            SOPNumber = Trim(HeaderRec("SOPNUMBE").ToString)
                                        Case 3
                                            SOPNumber = Trim(HeaderRec("ORIGNUMB").ToString)
                                    End Select

                                    ' Insert TMSHeader
                                    Dim oBookHeader As tmsintegrationservices.clsBookHeaderObject705 = fillBookHeader(GPFunctions, HeaderRec, EconnectStr, ProAbb, sErrors, c, TMSSetting.LegalEntity, SOPType, LaneValue)

                                    PorDAddressInfo = GPFunctions.GetPorDAddress(EconnectStr, HeaderRec("LOCNCODE"))
                                    NumberofLines = 0

                                    Select Case SOPType
                                        Case 2
                                            'LineCom.CommandText = "select count(*) from sop10200 where SOPTYPE = " & SOPType & " and SOPNUMBE = '" & SOPNumber & "'"
                                            'SOPType is always 2 modified by RHR for v-7.0.6.105 on 10/16/2017
                                            LineCom.CommandText = "select count(*) from sop10200 where SOPTYPE = 2 and SOPNUMBE = '" & SOPNumber & "'"
                                        Case 3
                                            'LineCom.CommandText = "select count(*) from sop30300 where SOPTYPE = " & SOPType & " and SOPNUMBE = '" & SOPNumber & "'"
                                            'SOPType is always 2 modified by RHR for v-7.0.6.105 on 10/16/2017
                                            LineCom.CommandText = "select count(*) from sop30300 where SOPTYPE = 2 and SOPNUMBE = '" & SOPNumber & "'"
                                    End Select


                                    LineRec = LineCom.ExecuteReader
                                    LineRec.Read()
                                    If (LineRec.HasRows) Then
                                        NumberofLines = LineRec(0).ToString
                                    Else
                                        NumberofLines = 0
                                    End If
                                    LineRec.Close()
                                    Try
                                        sErrors = ""

                                        '*******************************************************************************
                                        ''Start Debug Code
                                        '*******************************************************************************
                                        'Dim oConfig As New UserConfiguration()
                                        'With oConfig
                                        '    .AdminEmail = "rramsey@nextgeneration.com"
                                        '    .AutoRetry = 0
                                        '    .Database = "NGLMASPROD"
                                        '    .DBServer = "192.168.1.8\LOGISTICS"
                                        '    .Debug = False
                                        '    .FromEmail = "system@nextgeneration.com"
                                        '    .GroupEmail = "rramsey@nextgeneration.com"
                                        '    .LogFile = "C:\Data\TMSLogs\ProcessGPSOPOrders.log"
                                        '    .SMTPServer = "nglmail.ngl.local"
                                        '    .UserName = "ngl\rramsey"
                                        '    .WSAuthCode = "WSPROD"
                                        '    .WCFAuthCode = "WCFPROD"
                                        '    .WCFURL = "http://WCFPROD.vpgc.local"
                                        '    .WCFTCPURL = "net.tcp://WCFPROD.vpgc.local:705"
                                        'End With
                                        'Dim book As New TMS.clsBook(oConfig) ' NGL.FreightMaster.Integration.clsBook(oConfig)
                                        'book.OrderNotificationEmail = "rramsey@nextgeneration.com"

                                        'Dim oHeader As New TMS.clsBookHeaderObject705
                                        'oHeader = DTran.CopyMatchingFields(oHeader, oBookHeader, Nothing, sErrors)
                                        'Dim oRet = book.HasOrderChanged(oHeader, NumberofLines, OrdChangeStr)
                                        '*******************************************************************************
                                        ''End Debug Code
                                        '********************************************************************************

                                        'Modified by RHR for v-7.0.5.102 on 11/10/2016
                                        OrderChanged = oBookIntegration.HasOrderChanged(TMSSetting.TMSAuthCode, oBookHeader, NumberofLines, OrdChangeStr, False, False, sErrors)
                                        If Not String.IsNullOrWhiteSpace(sErrors) Then Log("Process Order Changed Error: " & sErrors)
                                    Catch ex As Exception
                                        LogError("Error in Processing SOP Orders - at the Order Change.  The Error is:  ", ex.Message, Me.AdminEmail)
                                    End Try

                                    Select Case SOPType
                                        Case 2

                                        Case 3
                                            OrderChanged = True
                                            If Me.Debug Then
                                                Log("SOP type = 3 Order Changed is Alwasy True using SOPNumber: " + SOPNumber)
                                            End If
                                    End Select
                                    If (Not OrderChanged) Then
                                        If Me.Debug Then Log("Skiping order because the order has not changed: " & HeaderRec("SOPNUMBE").ToString())
                                        HeaderRec.Close()
                                    Else
                                        If Me.Debug Then Log("Processing Order because It has changed: SOPNumber: " + SOPNumber)
                                        SOOrdersProcessed.Add(SOPOrder.SOPOrders)
                                        oBookHeaders.Add(oBookHeader)
                                        Select Case SOPType
                                            Case 2
                                                LineCom.CommandText = String.Format(c.GPFunctionsSOItemDetails, SOPNumber)
                                            Case 3
                                                LineCom.CommandText = String.Format(c.GPFunctionsSOConItemDetails, SOPNumber)
                                        End Select

                                        LineRec = LineCom.ExecuteReader
                                        Dim LineCols = LineRec.GetSchemaTable().Rows.Cast(Of DataRow)().[Select](Function(row) TryCast(row("ColumnName"), String)).ToList()
                                        Counter = 0
                                        While LineRec.Read
                                            Counter = Counter + 1
                                            oBookDetails.Add(fillItemDetails(SOPNumber, LineCols, LineRec, TMSSetting.LegalEntity.ToString(), HeaderRec("LOCNCODE")))
                                        End While
                                        If (Counter = 0) Then
                                            If Me.Verbose Then Log("No Line records to process for SOP #: " & SOPNumber)
                                        End If
                                        LineRec.Close()
                                        HeaderRec.Close()
                                    End If
                                End If
                            End If
                        Else
                            If Me.Verbose Then Log("SOP In List, but query did not return header data  for SOP #: " & SOPOrder.SOPOrders.ToString)
                        End If
                        If (Not HeaderRec.IsClosed) Then
                            HeaderRec.Close()
                        End If

                    Catch ex As Exception
                        Throw
                    Finally
                        'besure the reader is closed
                        Try
                            If Not HeaderRec Is Nothing AndAlso (Not HeaderRec.IsClosed) Then

                                HeaderRec.Close()

                            End If


                        Catch ex As Exception

                        End Try

                        Try
#Disable Warning BC42104 ' Variable 'LineRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                            If Not LineRec Is Nothing AndAlso (Not LineRec.IsClosed) Then
#Enable Warning BC42104 ' Variable 'LineRec' is used before it has been assigned a value. A null reference exception could result at runtime.

                                LineRec.Close()

                            End If


                        Catch ex As Exception

                        End Try
                    End Try

                    If Not oBookHeaders Is Nothing AndAlso oBookHeaders.Count() > 0 Then
                        If Me.Debug Then Log("Processing Book Object Data; Headers: " & oBookHeaders.Count().ToString() & " Details: " & oBookDetails.Count().ToString())
                        'Process the last order before submitting orderst to TMS
                        'Modified by RHR for v-7.0.5.102 on 11/10/2016
                        'Not needed when we use the web services
                        'oBookIntegration.RunSilentTenderAsync = False

                        'Modified by RHR for v-7.0.5.102 on 11/10/2016
                        Dim aBookHeaders As tmsintegrationservices.clsBookHeaderObject705() = oBookHeaders.ToArray()
                        Dim aBookDetails As tmsintegrationservices.clsBookDetailObject705()
                        sErrors = ""
                        If Not oBookDetails Is Nothing AndAlso oBookDetails.Count() > 0 Then aBookDetails = oBookDetails.ToArray()
                        'save changes to database 
#Disable Warning BC42104 ' Variable 'aBookDetails' is used before it has been assigned a value. A null reference exception could result at runtime.
                        Dim oResults As TMS.Configuration.ProcessDataReturnValues = oBookIntegration.ProcessBookData705(TMSSetting.TMSAuthCode, aBookHeaders, aBookDetails, sErrors)
#Enable Warning BC42104 ' Variable 'aBookDetails' is used before it has been assigned a value. A null reference exception could result at runtime.
                        Dim ReturnMessage As String = sErrors
                        If Me.Debug Then Log("Results = " & oResults.ToString())
                        Select Case oResults
                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Data Connection Failure! could not import Order information")
                                Else
                                    sIntegrationErrors.Add("Data Connection Failure! could not import Order information:  " & ReturnMessage)
                                End If

                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Integration Failure! could not import Order information")
                                Else
                                    generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                                End If

                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Integration Had Errors! could not import some Order information")
                                Else
                                    generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                                End If

                                'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                'blnRet = True
                                'End If
                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Data Validation Failure! could not import some Order information")
                                Else
                                    generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                                End If

                                'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                'blnRet = True
                                'End If
                            Case Else
                                'success
                                Dim strNumbers As String = "N/A"
                                If Not SOOrdersProcessed Is Nothing AndAlso SOOrdersProcessed.Count() > 0 Then
                                    strNumbers = String.Join(", ", SOOrdersProcessed)
                                End If

                                sLogMsgs.Add("Success! the following Order Numbers were processed: " & strNumbers)
                                'Processed = oResults.
                                'TODO: add code to send confirmation back to NAV that the orders were processed
                                'mark process and success
                                'blnRet = True
                        End Select

                        oResults = Nothing
                    End If


                    oBookHeaders.Clear()
                    oBookDetails.Clear()
                Next

                If Not sIntegrationErrors Is Nothing AndAlso sIntegrationErrors.Count() > 0 Then
                    LogError("Warning!  " & Source & " Had Errors: " & vbCrLf & String.Join(vbCrLf, sIntegrationErrors.ToArray()))
                End If
                'If Not sAdminErrors Is Nothing AndAlso sAdminErrors.Count() > 0 Then
                'LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sAdminErrors.ToArray()), Me.AdminEmail)
                'End If
                'If Not sOperationErrors Is Nothing AndAlso sOperationErrors.Count() > 0 Then
                'LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sOperationErrors.ToArray()), Me.GroupEmail)
                'End If
                'If Not sLogMsgs Is Nothing AndAlso sLogMsgs.Count() > 0 Then
                If Me.Verbose Then Log(Source & ": " & vbCrLf & String.Join(vbCrLf, sLogMsgs.ToArray()))
                'End If
                If Debug Then Log("Process Order Data Complete")

            Catch ex As Exception

                'MsgBox("Inner Try " & ex.Message)

                Log("There was an erorr processing SOP Orders.  The error is: " & ex.Message)


                'Report error
            Finally
                Try
                    ' End Routine
                    HeaderCom.Dispose()
                    HeaderConn.Dispose()

                    LineCom.Dispose()
                    LineConn.Dispose()
                Catch ex As Exception

                End Try
            End Try

        Catch ex As Exception
            Log(Source & ".ProcessGPSOPOrders Error!  Unexpected GP Process SOP Orders Error could not process any integration requests; the actual error is:  " & ex.Message)
        Finally
            'Modified by RHR for v-7.0.6.104 on 08/28/2027 added finally statement to dispose of db connections
            Try
#Disable Warning BC42104 ' Variable 'HeaderRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not HeaderRec Is Nothing AndAlso (Not HeaderRec.IsClosed) Then HeaderRec.Close()
#Enable Warning BC42104 ' Variable 'HeaderRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not LineRec Is Nothing AndAlso (Not LineRec.IsClosed) Then LineRec.Close()
            Catch ex As Exception

            End Try
            Try
                If Not HeaderCom Is Nothing Then HeaderCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not LineCom Is Nothing Then LineCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not HeaderConn Is Nothing AndAlso Not HeaderConn.State = ConnectionState.Closed Then HeaderConn.Close()
                HeaderConn.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not LineConn Is Nothing AndAlso Not LineConn.State = ConnectionState.Closed Then LineConn.Close()
                LineConn.Dispose()
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function



    ''' <summary> List(Of clsGPOrderInvoice)
    ''' Process the GP Sales Orders
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <param name="c"></param>
    ''' <param name="SOPOrderNumbers"></param>
    ''' <param name="SOPType"></param>
    ''' <param name="TMSCountToProcess"></param>
    ''' <param name="lGPOrderInvoces"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' Modified by RHR for v-7.0.5.102 on 11/10/2016
    '''   we now use web services for booking integration to correct problem with
    '''   auto processing.  DLL direct requires a reference to WCF HTTP and Net.TCP 
    '''   services to work,  the current configuration does not support this.  However,
    '''   it is built into the web service interface.
    ''' Modified by RHR for v-7.0.6.104 on 08/28/2027 added finally statement to dispose of db connections
    ''' Modified by RHR for v=7.0.6.105 on 11/29/2017  populate clsGPOrderInvoice list for each ship confirmed order
    ''' </remarks>
    Public Function ProcessGPSOPOrders(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByRef c As clsDefaultIntegrationConfiguration, ByVal SOPOrderNumbers As List(Of GPDataIntegrationSTructure.SOPOrders), ByVal SOPType As Int16, ByVal TMSCountToProcess As Int16, Optional ByRef lGPOrderInvoces As List(Of clsGPOrderInvoice) = Nothing) As Boolean

        Dim ReturnValue As Boolean = True
        Dim EconnectStr As String = ""
        Dim GetAppSettings = New AppSettingsReader
        Dim LaneNumber As String = ""
        Dim GPFunctions As New GPFunctions(c) 'Modified by RHR for v-7.0.5.102 10/14/2016
        Dim SOPCounter As Int16 = 0
        Dim OrderChanged As Boolean = False
        Dim OrdChangeStr As String = ""

        ' Set all the variables

        If Not String.IsNullOrWhiteSpace(TMSSetting.ERPUser) AndAlso Not String.IsNullOrWhiteSpace(TMSSetting.ERPPassword) Then
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";User ID=" & TMSSetting.ERPUser & ";Password=" & TMSSetting.ERPPassword
        Else
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        End If

        'EconnectStr = "Data Source=" & GetAppSettings.GetValue("SQLServer") & "; Initial Catalog=" & GPCompany & "Integrated Security=SSPI;Trusted_Connection=True"
        'EconnectStr = "Data Source=" & TMSSetting.ERPCertificate.ToString & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        'EconnectStr = "Data Source=NGLGP2013R2; Initial Catalog=TWO;Integrated Security=SSPI;Trusted_Connection=True"

        Dim HeaderConn = New SqlConnection(EconnectStr)
        Dim HeaderCom As New SqlCommand
        Dim HeaderRec As SqlDataReader
        Dim LineConn = New SqlConnection(EconnectStr)
        Dim LineCom As New SqlCommand
        Dim LineRec As SqlDataReader
        Dim Counter As Integer = 0
        Dim NumberofLines As Integer = 0
        Dim SOPNumber As String = ""
        Dim sIntegrationErrors As New List(Of String)
        Dim sLogMsgs As New List(Of String)
        Dim PorDAddressInfo As New GPDataIntegrationSTructure.FromAddress
        Dim ProAbb As String = ""
        Dim LaneValue As String = ""
        Dim LineNumber As Integer = 0
        Dim sErrors As String = ""
        Dim strNumbers As String = ""
        Try

            HeaderConn.Open()

            HeaderCom.Connection = HeaderConn

            LineConn.Open()

            LineCom.Connection = LineConn


            'Integrate Sales Order
            'Create TMS Header & Line

            Dim oBookHeaders As New List(Of tmsintegrationservices.clsBookHeaderObject705)
            Dim oBookDetails As New List(Of tmsintegrationservices.clsBookDetailObject705)
            'Modified by RHR for v-7.0.6.105 on 10/31/2017  
            'new list for item details to hold the items until HasChanged is determined
            'Allows the system to undo item details if the order informaiton has not changed
            Dim lItemDetailsToProcess As New List(Of tmsintegrationservices.clsBookDetailObject705)

            Dim oBookIntegration As New tmsintegrationservices.DTMSERPIntegration()
            oBookIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oBookIntegration.UseDefaultCredentials = True
            Else
                oBookIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If

            Dim SOOrdersProcessed As New List(Of String)
            Try
                If SOPOrderNumbers Is Nothing OrElse SOPOrderNumbers.Count < 1 Then
                    If SOPType = 3 Then
                        If Me.Debug Then Log("No  Ship Confirmations to process")
                    Else
                        If Me.Debug Then Log("No  Sales Orders to process")
                    End If
                    Return False
                Else
                    If SOPType = 3 Then
                        If Me.Debug Then Log("Processing, " & SOPOrderNumbers.Count & " , Ship Confirmations")
                    Else
                        If Me.Debug Then Log("Processing, " & SOPOrderNumbers.Count & " , Sales Orders to process")
                    End If
                End If
                For Each SOPOrder In SOPOrderNumbers

                    Dim oBookHeader As New tmsintegrationservices.clsBookHeaderObject705
                    'test code
                    Dim oBookTMSHeader As New TMS.clsBookHeaderObject705()

                    Select Case SOPType

                        Case 2

                            HeaderCom.CommandText = String.Format(c.GPFunctionsSOHeaders, SOPType, SOPOrder.SOPOrders.ToString)
                            'HeaderCom.CommandText = "select * from sop10100 where SOPTYPE = " & SOPType & " And SOPNUMBE = '" & SOPOrder.SOPOrders.ToString & "'"

                        Case 3

                            HeaderCom.CommandText = String.Format(c.GPFunctionsSOConHeaders, SOPType, SOPOrder.SOPOrders.ToString)
                            'HeaderCom.CommandText = "select * from SOP30200 where SOPTYPE = " & SOPType & " and SOPNUMBE = '" & SOPOrder.SOPOrders.ToString & "'"

                        Case Else

                    End Select

                    HeaderRec = HeaderCom.ExecuteReader
                    Try

                        HeaderRec.Read()

                        If (HeaderRec.HasRows) Then
                            'If Me.Debug Then Log("Attempting to process Order: " & HeaderRec("SOPNUMBE").ToString())
                            sErrors = ""
                            'Modified by RHR for v-7.0.5.102 on 11/10/2016
                            ProAbb = oBookIntegration.GetCompAbrevByNumberOrAlpha(TMSSetting.TMSAuthCode, 0, HeaderRec("LOCNCODE").ToString, "", sErrors)
                            'ProAbb = oBookIntegration.GetCompAbrevByNumberOrAlpha(0, "WAREHOUSE", "", sErrors)

                            If (String.IsNullOrWhiteSpace(sErrors) AndAlso String.IsNullOrWhiteSpace(ProAbb)) Then
                                ' skip order if failed

                                If Me.Debug Then
                                    Log("Could not process order: " & HeaderRec("SOPNUMBE").ToString() & " Due to a missing Company Pro Abb for Comp Alpha Code: " & HeaderRec("LOCNCODE").ToString())
                                End If
                                HeaderRec.Close()
                            Else
                                'If Me.Debug Then Log("Running CalculateLane.")
                                LaneValue = GPFunctions.CalculateLane(EconnectStr, 1, HeaderRec("SOPNumbe").ToString, 16384, HeaderRec("SOPNumbe").ToString, ProAbb, SOPType, sErrors)

                                If (LaneValue = "Error") Then
                                    If Me.Verbose Or Me.Debug Then
                                        Log("Error when processing Lane for order: " & HeaderRec("SOPNUMBE").ToString())

                                    End If

                                    HeaderRec.Close()

                                    ' REturn more information 
                                    'LogError("Error when calculating Lane for order: ", sErrors, Me.AdminEmail)

                                Else
                                    'If Me.Debug Then Log("Reading SO informaiton using Lane Number: " & LaneValue)
                                    SOPNumber = Trim(HeaderRec("SOPNUMBE").ToString)
                                    ' Process Order to TMS

                                    ' Insert TMSHeader
                                    Select Case SOPType
                                        Case 2

                                            oBookHeader.PONumber = Trim(HeaderRec("SOPNUMBE").ToString())
                                            oBookTMSHeader.PONumber = oBookHeader.PONumber
                                            oBookHeader.POStatusFlag = 5
                                            oBookTMSHeader.POStatusFlag = 5
                                        Case 3
                                            'Modified by RHR for v=7.0.6.105 on 11/29/2017  populate clsGPOrderInvoice list for each order
                                            If lGPOrderInvoces Is Nothing Then lGPOrderInvoces = New List(Of clsGPOrderInvoice)
                                            Dim dtrowts As Date = Date.Now()
                                            Date.TryParse(Trim(HeaderRec("dex_row_ts")), dtrowts)
                                            lGPOrderInvoces.Add(New clsGPOrderInvoice(Trim(HeaderRec("SOPNUMBE")), Trim(HeaderRec("ORIGNUMB")), Trim(HeaderRec("LOCNCODE")), dtrowts))
                                            oBookHeader.PONumber = Trim(HeaderRec("ORIGNUMB").ToString())
                                            oBookTMSHeader.PONumber = oBookHeader.PONumber
                                            oBookHeader.POStatusFlag = 3 'ship confirmed
                                            oBookTMSHeader.POStatusFlag = 3
                                    End Select

                                    'Modified by RHR for v-7.0.6.105 on 11/29/2017 added logic to map ORDRDATE or DocDate as default to PODate using new ValidateGPDateString method
                                    oBookHeader.POdate = GPFunctions.ValidateGPDateString(Trim(HeaderRec("ORDRDATE").ToString()), Trim(HeaderRec("DocDate").ToString()))
                                    oBookTMSHeader.POdate = oBookHeader.POdate
                                    'Modified by RHR for v-7.0.6.105 on 11/29/2017 added logic to map ReqShipDate or DocDate as default to POShipdate using new ValidateGPDateString method
                                    oBookHeader.POShipdate = GPFunctions.ValidateGPDateString(Trim(HeaderRec("ReqShipDate").ToString()), Trim(HeaderRec("DocDate").ToString()))
                                    oBookTMSHeader.POShipdate = oBookHeader.POShipdate
                                    oBookHeader.POVendor = LaneValue 'GPFunctions.CalculateLane(EconnectStr, 1, HeaderRec("SOPNumbe").ToString, 16384, HeaderRec("SOPNumbe").ToString, ProAbb, SOPType, sErrors)
                                    oBookTMSHeader.POVendor = oBookHeader.POVendor
                                    'oBookHeader.POBuyer = HeaderRec("").ToString
                                    oBookHeader.POBuyer = ""
                                    oBookTMSHeader.POBuyer = ""
                                    'oBookHeader.POFrt = HeaderRec("").ToString
                                    ' Work with Rob to determine
                                    oBookHeader.POFrt = GPFunctions.GetShippingMethod(EconnectStr, 1, HeaderRec("SOPNumbe").ToString, 16384, HeaderRec("SOPNumbe").ToString, SOPType)
                                    oBookTMSHeader.POFrt = oBookHeader.POFrt

                                    Double.TryParse(HeaderRec("FRTAMNT").ToString(), oBookHeader.POTotalFrt)
                                    Double.TryParse(HeaderRec("EXTDCOST").ToString, oBookHeader.POTotalCost)
                                    'If Me.Debug Then Log("Getting Order Weight For: " & HeaderRec("SOPNUMBE").ToString())
                                    oBookHeader.POWgt = GPFunctions.GetSOPTotalWeight(EconnectStr, HeaderRec("SOPNUMBE").ToString, SOPType)
                                    oBookTMSHeader.POWgt = oBookHeader.POWgt
                                    'oBookHeader.POCube = HeaderRec("").ToString
                                    oBookHeader.POCube = 0
                                    oBookTMSHeader.POCube = oBookHeader.POCube
                                    oBookHeader.POQty = CInt(GPFunctions.GetSOPQuantity(EconnectStr, HeaderRec("SOPNUMBE").ToString, SOPType))
                                    oBookTMSHeader.POQty = oBookHeader.POQty
                                    'oBookHeader.POPallets = HeaderRec("").ToString
                                    'Need to define a Pallet formula
                                    oBookHeader.POPallets = CInt(GPFunctions.CalculatePallets(EconnectStr, HeaderRec("SOPNUMBE").ToString, 1, Me.AdminEmail, SOPType))
                                    oBookTMSHeader.POPallets = oBookHeader.POPallets
                                    'oBookHeader.POLines = HeaderRec("").ToString
                                    oBookHeader.POLines = 0
                                    oBookTMSHeader.POLines = oBookHeader.POLines
                                    oBookHeader.POConfirm = False
                                    oBookTMSHeader.POConfirm = oBookHeader.POConfirm
                                    oBookHeader.PODefaultCustomer = "0"
                                    oBookTMSHeader.PODefaultCustomer = oBookHeader.PODefaultCustomer
                                    'oBookHeader.PODefaultCarrier = HeaderRec("").ToString
                                    ' Talk to Rob on this field below
                                    oBookHeader.PODefaultCarrier = 0
                                    oBookTMSHeader.PODefaultCarrier = oBookHeader.PODefaultCarrier
                                    'oBookHeader.POReqDate = Trim(HeaderRec("ReqShipDate").ToString())
                                    'Add function
                                    oBookHeader.POReqDate = GPFunctions.ValidateGPDateString(GPFunctions.GetSOPRequiredShipDate(EconnectStr, SOPNumber, SOPType, TMSSetting, c, AdminEmail), oBookHeader.POShipdate)

                                    oBookTMSHeader.POReqDate = oBookHeader.POReqDate
                                    ' Write funtion to get shipping instructions
                                    oBookHeader.POShipInstructions = GPFunctions.GetComments(EconnectStr, HeaderRec("SOPNUMBE").ToString, 1, Me.AdminEmail)
                                    oBookHeader.POCooler = False
                                    oBookTMSHeader.POCooler = oBookHeader.POCooler
                                    oBookHeader.POFrozen = False
                                    oBookTMSHeader.POFrozen = oBookHeader.POFrozen
                                    oBookHeader.PODry = False
                                    oBookTMSHeader.PODry = oBookHeader.PODry
                                    oBookHeader.POTemp = GPFunctions.GetTemp(EconnectStr, 1, HeaderRec("SOPNUMBE").ToString, SOPType)
                                    oBookTMSHeader.POTemp = oBookHeader.POTemp
                                    oBookHeader.POCarType = ""
                                    oBookHeader.POShipVia = ""
                                    oBookHeader.POShipViaType = ""
                                    oBookHeader.POConsigneeNumber = ""
                                    oBookHeader.POCustomerPO = Trim(HeaderRec("CSTPONBR").ToString())
                                    oBookTMSHeader.POCustomerPO = oBookHeader.POCustomerPO
                                    oBookHeader.POOtherCosts = 0
                                    oBookHeader.POOrderSequence = 0
                                    oBookHeader.POChepGLID = ""
                                    oBookHeader.POCarrierEquipmentCodes = ""
                                    oBookHeader.POCarrierTypeCode = ""
                                    oBookHeader.POPalletPositions = ""
                                    ' Modified by RHR for v-7.0.5.102 on 10/05/20126
                                    ' We do not use the ReqShipDate for appointment informaiton 
                                    'oBookHeader.POSchedulePUDate = HeaderRec("ReqShipDate").ToString
                                    oBookHeader.POSchedulePUDate = ""
                                    oBookHeader.POSchedulePUTime = ""
                                    'oBookHeader.POScheduleDelDate = HeaderRec("ReqShipDate").ToString
                                    oBookHeader.POScheduleDelDate = ""
                                    oBookHeader.POSCheduleDelTime = ""
                                    oBookHeader.POActPUDate = ""
                                    oBookHeader.POActPUTime = ""
                                    oBookHeader.POActDelDate = ""
                                    oBookHeader.POActDelTime = ""
                                    oBookHeader.POOrigCompNumber = ""
                                    'If Me.Debug Then Log("Reading Address Info For Order Number: " & SOPNumber)
                                    PorDAddressInfo = GPFunctions.GetPorDAddress(EconnectStr, HeaderRec("LOCNCODE"))
                                    If (PorDAddressInfo.AddressID = "No Address") Then

                                        LogError("No Address found", "There was no address found for SOP Order: " & SOPOrder.SOPOrders.ToString, Me.AdminEmail)
                                        'Report to address found

                                    Else

                                        oBookHeader.POOrigName = Trim(PorDAddressInfo.AddressName)
                                        oBookTMSHeader.POOrigName = oBookHeader.POOrigName
                                        oBookHeader.POOrigCompAlphaCode = Trim(PorDAddressInfo.AddressID)
                                        oBookTMSHeader.POOrigCompAlphaCode = oBookHeader.POOrigCompAlphaCode
                                        oBookHeader.POOrigAddress1 = Trim(PorDAddressInfo.Address1)
                                        oBookTMSHeader.POOrigAddress1 = oBookHeader.POOrigAddress1
                                        oBookHeader.POOrigAddress2 = Trim(PorDAddressInfo.Address2)
                                        oBookTMSHeader.POOrigAddress2 = oBookHeader.POOrigAddress2
                                        oBookHeader.POOrigAddress3 = Trim(PorDAddressInfo.Address3)
                                        oBookTMSHeader.POOrigAddress3 = oBookHeader.POOrigAddress3
                                        oBookHeader.POOrigCity = Trim(PorDAddressInfo.City)
                                        oBookTMSHeader.POOrigCity = oBookHeader.POOrigCity
                                        oBookHeader.POOrigState = Trim(PorDAddressInfo.State)
                                        oBookTMSHeader.POOrigState = oBookHeader.POOrigState
                                        oBookHeader.POOrigCountry = Trim(If(String.IsNullOrWhiteSpace(c.GPFunctionsForceDefaultCountry), PorDAddressInfo.Country, c.GPFunctionsForceDefaultCountry))
                                        oBookTMSHeader.POOrigCountry = oBookHeader.POOrigCountry
                                        oBookHeader.POOrigZip = Trim(PorDAddressInfo.ZipCode)
                                        oBookTMSHeader.POOrigZip = oBookHeader.POOrigZip
                                        oBookHeader.POOrigContactPhone = Trim(PorDAddressInfo.Phone)
                                        oBookTMSHeader.POOrigContactPhone = oBookHeader.POOrigContactPhone
                                        oBookHeader.POOrigContactPhoneExt = ""
                                        oBookHeader.POOrigContactFax = Trim(PorDAddressInfo.Fax)
                                        oBookTMSHeader.POOrigContactFax = oBookHeader.POOrigContactFax

                                    End If

                                    oBookHeader.PODestCompNumber = ""
                                    oBookTMSHeader.PODestCompNumber = oBookHeader.PODestCompNumber
                                    oBookHeader.PODestCompAlphaCode = Trim(HeaderRec("PRSTADCD").ToString())
                                    oBookTMSHeader.PODestCompAlphaCode = oBookHeader.PODestCompAlphaCode
                                    oBookHeader.PODestName = Trim(HeaderRec("ShipToName").ToString())
                                    oBookTMSHeader.PODestName = oBookHeader.PODestName
                                    oBookHeader.PODestAddress1 = Trim(HeaderRec("ADDRESS1").ToString())
                                    oBookTMSHeader.PODestAddress1 = oBookHeader.PODestAddress1
                                    oBookHeader.PODestAddress2 = Trim(HeaderRec("ADDRESS2").ToString())
                                    oBookTMSHeader.PODestAddress2 = oBookHeader.PODestAddress2
                                    oBookHeader.PODestAddress3 = Trim(HeaderRec("ADDRESS1").ToString())
                                    oBookTMSHeader.PODestAddress3 = oBookHeader.PODestAddress3
                                    oBookHeader.PODestCity = Trim(HeaderRec("CITY").ToString())
                                    oBookTMSHeader.PODestCity = oBookHeader.PODestCity
                                    oBookHeader.PODestState = Trim(HeaderRec("STATE").ToString())
                                    oBookTMSHeader.PODestState = oBookHeader.PODestState
                                    oBookHeader.PODestCountry = Trim(If(String.IsNullOrWhiteSpace(c.GPFunctionsForceDefaultCountry), HeaderRec("COUNTRY").ToString(), c.GPFunctionsForceDefaultCountry))
                                    oBookTMSHeader.PODestCountry = oBookHeader.PODestCountry
                                    oBookHeader.PODestZip = Trim(HeaderRec("ZIPCODE").ToString())
                                    oBookTMSHeader.PODestZip = oBookHeader.PODestZip
                                    oBookHeader.PODestContactPhone = Trim(HeaderRec("PHNUMBR1").ToString())
                                    oBookTMSHeader.PODestContactPhone = oBookHeader.PODestContactPhone
                                    oBookHeader.PODestContactPhoneExt = ""
                                    oBookHeader.PODestContactFax = Trim(HeaderRec("FAXNUMBR").ToString())
                                    oBookTMSHeader.PODestContactFax = oBookHeader.PODestContactFax
                                    oBookHeader.POInbound = False
                                    oBookHeader.POPalletExchange = False
                                    oBookHeader.POPalletType = ""
                                    oBookHeader.POComments = ""
                                    oBookHeader.POCommentsConfidential = ""
                                    oBookHeader.PODefaultRouteSequence = 0
                                    'Modified by RHR v-7.0.5.102 09/26/2016 removed code to populate the PORouteGuideNumber with the Lane Number
                                    'as this is not required.
                                    'oBookHeader.PORouteGuideNumber = GPFunctions.CalculateLane(EconnectStr, 1, HeaderRec("SOPNumbe").ToString, 16384, HeaderRec("SOPNumbe").ToString, ProAbb, SOPType, sErrors)
                                    oBookHeader.PORouteGuideNumber = ""
                                    oBookHeader.POCompLegalEntity = Trim(TMSSetting.LegalEntity.ToString())
                                    oBookTMSHeader.POCompLegalEntity = oBookHeader.POCompLegalEntity
                                    oBookHeader.POCompAlphaCode = Trim(HeaderRec("LOCNCODE"))
                                    oBookTMSHeader.POCompAlphaCode = oBookHeader.POCompAlphaCode
                                    oBookHeader.POModeTypeControl = GPFunctions.GetTransporationMode(HeaderRec("SOPNUMBE").ToString, "SOP")
                                    oBookTMSHeader.POModeTypeControl = oBookHeader.POModeTypeControl
                                    oBookHeader.POUser1 = ""
                                    oBookHeader.POUser2 = ""
                                    oBookHeader.POUser3 = ""
                                    oBookHeader.POUser4 = ""
                                    oBookHeader.POAPGLNumber = ""
                                    oBookHeader.POAppt = True

                                    NumberofLines = 0
                                    'If Me.Debug Then Log("Calculating # Lines for SO Order Number: " & SOPNumber)

                                    '****************************  Added by SEM 2017-10-18  Removed the 1st qwery to count

                                    Select Case SOPType
                                        Case 2
                                            LineCom.CommandText = String.Format(c.GPFunctionsSOItemDetails, SOPOrder.SOPOrders.ToString)

                                        Case 3
                                            LineCom.CommandText = String.Format(c.GPFunctionsSOConItemDetails, SOPOrder.SOPOrders.ToString)

                                        Case Else

                                    End Select

                                    'LineCom.CommandText = "select * from sop10200 where SOPTYPE = " & SOPType & " and SOPNUMBE = '" & SOPOrder.SOPOrders.ToString & "'"

                                    LineRec = LineCom.ExecuteReader
                                    Dim LineCols = LineRec.GetSchemaTable().Rows.Cast(Of DataRow)().[Select](Function(row) TryCast(row("ColumnName"), String)).ToList()

                                    Counter = 0

                                    'If Me.Debug Then Log("Reading Item Details For Order Number: " & SOPNumber)
                                    While LineRec.Read

                                        NumberofLines = NumberofLines + 1

                                        Dim oBookDetail As New tmsintegrationservices.clsBookDetailObject705

                                        Counter = Counter + 1

                                        Select Case SOPType
                                            Case 2
                                                oBookDetail.ItemPONumber = SOPNumber

                                            Case 3
                                                oBookDetail.ItemPONumber = Trim(HeaderRec("ORIGNUMB").ToString())

                                            Case Else

                                        End Select

                                        If (LineCols.Contains("ItemNumber")) Then oBookDetail.ItemNumber = Trim(LineRec("ItemNumber").ToString())
                                        If (LineCols.Contains("ItemCost")) Then Double.TryParse(LineRec("ItemCost").ToString(), oBookDetail.ItemCost)
                                        If (LineCols.Contains("QtyOrdered")) Then GPFunctions.TryParseInt(LineRec("QtyOrdered").ToString(), oBookDetail.QtyOrdered)
                                        If (LineCols.Contains("Weight")) Then Double.TryParse(LineRec("Weight").ToString(), oBookDetail.Weight)
                                        If (LineCols.Contains("FixOffInvAllow")) Then Double.TryParse(LineRec("FixOffInvAllow").ToString(), oBookDetail.FixOffInvAllow)
                                        If (LineCols.Contains("FixFrtAllow")) Then Double.TryParse(LineRec("FixFrtAllow").ToString(), oBookDetail.FixFrtAllow)
                                        If (LineCols.Contains("FreightCost")) Then Double.TryParse(LineRec("FreightCost").ToString(), oBookDetail.FreightCost)
                                        If (LineCols.Contains("ItemCost")) Then Double.TryParse(LineRec("ItemCost").ToString(), oBookDetail.ItemCost)
                                        If (LineCols.Contains("Cube")) Then GPFunctions.TryParseInt(LineRec("Cube").ToString(), oBookDetail.Cube)
                                        If (LineCols.Contains("Pack")) Then GPFunctions.TryParseInt(LineRec("Pack").ToString(), oBookDetail.Pack)
                                        If (LineCols.Contains("Size")) Then oBookDetail.Size = Trim(LineRec("Size").ToString())
                                        If (LineCols.Contains("Description")) Then oBookDetail.Description = Trim(LineRec("Description").ToString())
                                        If (LineCols.Contains("Hazmat")) Then oBookDetail.Hazmat = Trim(LineRec("Hazmat").ToString())
                                        If (LineCols.Contains("Brand")) Then oBookDetail.Brand = Trim(LineRec("Brand").ToString())
                                        If (LineCols.Contains("CostCenter")) Then oBookDetail.CostCenter = Trim(LineRec("CostCenter").ToString())
                                        If (LineCols.Contains("LotNumber")) Then oBookDetail.LotNumber = Trim(LineRec("LotNumber").ToString())
                                        If (LineCols.Contains("LotExpirationDate")) Then oBookDetail.LotExpirationDate = Trim(LineRec("LotExpirationDate").ToString())
                                        If (LineCols.Contains("GTIN")) Then oBookDetail.GTIN = Trim(LineRec("GTIN").ToString())
                                        If (LineCols.Contains("CustItemNumber")) Then oBookDetail.CustItemNumber = Trim(LineRec("CustItemNumber").ToString())
                                        oBookDetail.CustomerNumber = "0" 'Modified by RHR for v-7.0.5.102 10/17/2016 we do not use the CUSTNMBR here  HeaderRec("CUSTNMBR").ToString
                                        oBookDetail.POOrderSequence = 0
                                        If (LineCols.Contains("PalletType")) Then oBookDetail.PalletType = Trim(LineRec("PalletType").ToString())
                                        If (LineCols.Contains("POItemHazmatTypeCode")) Then oBookDetail.POItemHazmatTypeCode = Trim(LineRec("POItemHazmatTypeCode").ToString())
                                        If (LineCols.Contains("POItem49CFRCode")) Then oBookDetail.POItem49CFRCode = Trim(LineRec("POItem49CFRCode").ToString())
                                        If (LineCols.Contains("POItemIATACode")) Then oBookDetail.POItemIATACode = Trim(LineRec("POItemIATACode").ToString())
                                        If (LineCols.Contains("POItemDOTCode")) Then oBookDetail.POItemDOTCode = Trim(LineRec("POItemDOTCode").ToString())
                                        If (LineCols.Contains("POItemMarineCode")) Then oBookDetail.POItemMarineCode = Trim(LineRec("POItemMarineCode").ToString())
                                        If (LineCols.Contains("POItemNMFCClass")) Then oBookDetail.POItemNMFCClass = Trim(LineRec("POItemNMFCClass").ToString())
                                        If (LineCols.Contains("POItemFAKClass")) Then oBookDetail.POItemFAKClass = Trim(LineRec("POItemFAKClass").ToString())
                                        If (LineCols.Contains("POItemLimitedQtyFlag")) Then Boolean.TryParse(LineRec("POItemLimitedQtyFlag").ToString(), oBookDetail.POItemLimitedQtyFlag)
                                        If (LineCols.Contains("POItemPallets")) Then Double.TryParse(LineRec("POItemPallets").ToString(), oBookDetail.POItemPallets)
                                        If (LineCols.Contains("POItemTies")) Then Double.TryParse(LineRec("POItemTies").ToString(), oBookDetail.POItemTies)
                                        If (LineCols.Contains("POItemHighs")) Then Double.TryParse(LineRec("POItemHighs").ToString(), oBookDetail.POItemHighs)
                                        If (LineCols.Contains("POItemQtyPalletPercentage")) Then Double.TryParse(LineRec("POItemQtyPalletPercentage").ToString(), oBookDetail.POItemQtyPalletPercentage)
                                        If (LineCols.Contains("POItemQtyLength")) Then Double.TryParse(LineRec("POItemQtyLength").ToString(), oBookDetail.POItemQtyLength)
                                        If (LineCols.Contains("POItemQtyWidth")) Then Double.TryParse(LineRec("POItemQtyWidth").ToString(), oBookDetail.POItemQtyWidth)
                                        If (LineCols.Contains("POItemQtyHeight")) Then Double.TryParse(LineRec("POItemQtyHeight").ToString(), oBookDetail.POItemQtyHeight)
                                        If (LineCols.Contains("POItemStackable")) Then Boolean.TryParse(LineRec("POItemStackable").ToString(), oBookDetail.POItemStackable)
                                        If (LineCols.Contains("POItemLevelOfDensity")) Then GPFunctions.TryParseInt(LineRec("POItemLevelOfDensity").ToString(), oBookDetail.POItemLevelOfDensity)
                                        oBookDetail.POItemCompLegalEntity = Trim(TMSSetting.LegalEntity.ToString())
                                        oBookDetail.POItemCompAlphaCode = Trim(HeaderRec("LOCNCODE"))
                                        'oBookDetail.POItemCompAlphaCode = "WAREHOUSE"
                                        If (LineCols.Contains("POItemNMFCSubClass")) Then oBookDetail.POItemNMFCSubClass = Trim(LineRec("POItemNMFCSubClass").ToString())
                                        If (LineCols.Contains("POItemUser1")) Then oBookDetail.POItemUser1 = Trim(LineRec("POItemUser1").ToString())
                                        If (LineCols.Contains("POItemUser2")) Then oBookDetail.POItemUser2 = Trim(LineRec("POItemUser2").ToString())
                                        If (LineCols.Contains("POItemUser3")) Then oBookDetail.POItemUser3 = Trim(LineRec("POItemUser3").ToString())
                                        If (LineCols.Contains("POItemUser4")) Then oBookDetail.POItemUser4 = Trim(LineRec("POItemUser4").ToString())
                                        If (LineCols.Contains("POItemWeightUnitOfMeasure")) Then oBookDetail.POItemWeightUnitOfMeasure = Trim(LineRec("POItemWeightUnitOfMeasure").ToString())
                                        If (LineCols.Contains("POItemCubeUnitOfMeasure")) Then oBookDetail.POItemCubeUnitOfMeasure = Trim(LineRec("POItemCubeUnitOfMeasure").ToString())
                                        If (LineCols.Contains("POItemDimensionUnitOfMeasure")) Then oBookDetail.POItemDimensionUnitOfMeasure = Trim(LineRec("POItemDimensionUnitOfMeasure").ToString())
                                        oBookDetail.BookItemCommCode = oBookHeader.POTemp

                                        'Modified by RHR for v-7.0.6.105 on 10/31/2017 
                                        'we do not add items to oBookDetails until HasChanged is tested
                                        lItemDetailsToProcess.Add(oBookDetail)
                                        'oBookDetails.Add(oBookDetail)

                                        oBookDetail = Nothing

                                    End While

                                    ' Report something if no line recvords
                                    If (NumberofLines = 0) Then

                                        If Me.Verbose Then Log("No Line records to process for SOP #: " & SOPOrder.SOPOrders.ToString)

                                    End If
                                    LineRec.Close()
                                    HeaderRec.Close()
                                    If SOPType <> 3 Then
                                        'we check if the order has changed but not for ship confirmed orders SOPType 3
                                        Try
                                            'Modified by RHR for v-7.0.5.102 on 11/10/2016
                                            'If Me.Debug Then Log("Checking if orer has changed For Order Number: " & SOPNumber)
                                            sErrors = ""
                                            OrderChanged = oBookIntegration.HasOrderChanged(TMSSetting.TMSAuthCode, oBookHeader, NumberofLines, OrdChangeStr, False, False, sErrors)
                                            If Not String.IsNullOrWhiteSpace(sErrors) Then Log("Process Order Changed Error: " & sErrors)
                                        Catch ex As Exception
                                            LogError("Error in Processing SOP Orders - at the Order Change.  The Error is:  ", ex.Message, Me.AdminEmail)

                                        End Try
                                    Else
                                        If Me.Debug Then Log("Skipping order change validation for SOPType 3 Ship Confirmation")
                                        OrderChanged = True
                                    End If
                                    'Added by SEM 2017-08-224
                                    'Added to enable turning off order change

                                    If (Not OrderChanged) Then
                                        If Me.Debug Then Log("Skiping order because the order has not changed: " & SOPNumber)

                                    Else

                                        SOOrdersProcessed.Add(SOPOrder.SOPOrders)
                                        'If Me.Debug Then Log("Order is new or has changed adding to Book Header collection: " & SOPNumber)
                                        oBookHeaders.Add(oBookHeader)
                                        'Modified by RHR for v-7.0.6.105 on 10/31/2017 
                                        'we do not add items to oBookDetails until HasChanged is tested
                                        If Not lItemDetailsToProcess Is Nothing AndAlso lItemDetailsToProcess.Count > 0 Then
                                            oBookDetails.AddRange(lItemDetailsToProcess)
                                        End If
                                        lItemDetailsToProcess = New List(Of tmsintegrationservices.clsBookDetailObject705)
                                    End If

                                    If (Not HeaderRec.IsClosed) Then
                                        HeaderRec.Close()
                                    End If
                                    oBookHeader = Nothing
                                End If
                            End If
                        Else
                            If Me.Verbose Then Log("SOP In List, but query did not return header data  for SOP #: " & SOPOrder.SOPOrders.ToString)
                        End If

                    Catch ex As Exception
                        Throw
                    Finally
                        'besure the reader is closed
                        Try
                            If Not HeaderRec Is Nothing AndAlso (Not HeaderRec.IsClosed) Then

                                HeaderRec.Close()

                            End If


                        Catch ex As Exception

                        End Try

                        Try
#Disable Warning BC42104 ' Variable 'LineRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                            If Not LineRec Is Nothing AndAlso (Not LineRec.IsClosed) Then
#Enable Warning BC42104 ' Variable 'LineRec' is used before it has been assigned a value. A null reference exception could result at runtime.

                                LineRec.Close()

                            End If


                        Catch ex As Exception

                        End Try
                    End Try

                    If Not oBookHeaders Is Nothing AndAlso oBookHeaders.Count() > 0 Then


                        If Me.Debug Then Log("Processing Book Object Data; Headers: " & oBookHeaders.Count().ToString() & " Details: " & oBookDetails.Count().ToString())
                        'Process the last order before submitting orderst to TMS
                        'Modified by RHR for v-7.0.5.102 on 11/10/2016
                        'Not needed when we use the web services
                        'oBookIntegration.RunSilentTenderAsync = False

                        'Modified by RHR for v-7.0.5.102 on 11/10/2016
                        Dim aBookHeaders As tmsintegrationservices.clsBookHeaderObject705() = oBookHeaders.ToArray()
                        Dim aBookDetails As tmsintegrationservices.clsBookDetailObject705()
                        sErrors = ""
                        If Not oBookDetails Is Nothing AndAlso oBookDetails.Count() > 0 Then aBookDetails = oBookDetails.ToArray()
                        'save changes to database 
#Disable Warning BC42104 ' Variable 'aBookDetails' is used before it has been assigned a value. A null reference exception could result at runtime.
                        Dim oResults As TMS.Configuration.ProcessDataReturnValues = oBookIntegration.ProcessBookData705(TMSSetting.TMSAuthCode, aBookHeaders, aBookDetails, sErrors)
#Enable Warning BC42104 ' Variable 'aBookDetails' is used before it has been assigned a value. A null reference exception could result at runtime.
                        Dim ReturnMessage As String = sErrors
                        If Me.Debug Then Log("Results = " & oResults.ToString())
                        Select Case oResults
                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Data Connection Failure! could not import Order information")
                                Else
                                    sIntegrationErrors.Add("Data Connection Failure! could not import Order information:  " & ReturnMessage)
                                End If

                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Integration Failure! could not import Order information")
                                Else
                                    generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                                End If

                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Integration Had Errors! could not import some Order information")
                                Else
                                    generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                                End If

                                'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                'blnRet = True
                                'End If
                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Data Validation Failure! could not import some Order information")
                                Else
                                    generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                                End If

                                'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                'blnRet = True
                                'End If
                            Case Else
                                'success

                                If Not SOOrdersProcessed Is Nothing AndAlso SOOrdersProcessed.Count() > 0 Then
                                    strNumbers = String.Join(", ", SOOrdersProcessed)
                                End If
                                'Processed = oResults.
                                'TODO: add code to send confirmation back to NAV that the orders were processed
                                'mark process and success
                                'blnRet = True
                        End Select

                        oResults = Nothing
                    End If
                    oBookHeaders.Clear()
                    oBookDetails.Clear()
                Next
                If Not String.IsNullOrWhiteSpace(strNumbers) Then
                    sLogMsgs.Add("Success! the following Order Numbers were processed: " & strNumbers)
                End If

                If Not sIntegrationErrors Is Nothing AndAlso sIntegrationErrors.Count() > 0 Then
                    LogError("Warning!  " & Source & " Had Errors: " & vbCrLf & String.Join(vbCrLf, sIntegrationErrors.ToArray()))
                End If
                'If Not sAdminErrors Is Nothing AndAlso sAdminErrors.Count() > 0 Then
                'LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sAdminErrors.ToArray()), Me.AdminEmail)
                'End If
                'If Not sOperationErrors Is Nothing AndAlso sOperationErrors.Count() > 0 Then
                'LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sOperationErrors.ToArray()), Me.GroupEmail)
                'End If
                If Not sLogMsgs Is Nothing AndAlso sLogMsgs.Count() > 0 AndAlso Me.Verbose Then
                    If Me.Verbose Then Log(Source & ": " & vbCrLf & String.Join(vbCrLf, sLogMsgs.ToArray()))
                End If
                If Debug Then Log("Process Order Data Complete")

            Catch ex As Exception

                'MsgBox("Inner Try " & ex.Message)

                Log("There was an error processing SOP Orders.  The error is: " & ex.Message)


                'Report error
            Finally
                Try
                    ' End Routine
                    HeaderCom.Dispose()
                    HeaderConn.Dispose()

                    LineCom.Dispose()
                    LineConn.Dispose()
                Catch ex As Exception

                End Try
            End Try

        Catch ex As Exception
            Log(Source & ".ProcessGPSOPOrders Error!  Unexpected GP Process SOP Orders Error could not process any integration requests; the actual error is:  " & ex.Message)
        Finally
            'Modified by RHR for v-7.0.6.104 on 08/28/2027 added finally statement to dispose of db connections
            Try
#Disable Warning BC42104 ' Variable 'HeaderRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not HeaderRec Is Nothing AndAlso (Not HeaderRec.IsClosed) Then HeaderRec.Close()
#Enable Warning BC42104 ' Variable 'HeaderRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not LineRec Is Nothing AndAlso (Not LineRec.IsClosed) Then LineRec.Close()
            Catch ex As Exception

            End Try
            Try
                If Not HeaderCom Is Nothing Then HeaderCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not LineCom Is Nothing Then LineCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not HeaderConn Is Nothing AndAlso Not HeaderConn.State = ConnectionState.Closed Then HeaderConn.Close()
                HeaderConn.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not LineConn Is Nothing AndAlso Not LineConn.State = ConnectionState.Closed Then LineConn.Close()
                LineConn.Dispose()
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function

    ''' <summary>
    ''' Process GP Inventory Transfers
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <param name="c"></param>
    ''' <param name="InvTransferNumbers"></param>
    ''' <param name="TMSCountToProcess"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.104 on 08/28/2027 added finally statement to dispose of db connections
    ''' </remarks>
    Public Function ProcessGPInvTransfers(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByRef c As clsDefaultIntegrationConfiguration, ByVal InvTransferNumbers As List(Of GPDataIntegrationSTructure.InvTransfers), ByVal TMSCountToProcess As Int16) As Boolean

        Dim ReturnValue As Boolean = True
        Dim EconnectStr As String = ""
        Dim GetAppSettings = New AppSettingsReader
        Dim LaneNumber As String = ""
        Dim GPFunctions As New GPFunctions(c) 'Modified by RHR for v-7.0.5.102 10/14/2016
        Dim SOPCounter As Int16 = 0
        Dim OrderChanged As Boolean = False
        Dim OrdChangeStr As String = ""

        ' Set all the variables

        If Not String.IsNullOrWhiteSpace(TMSSetting.ERPUser) AndAlso Not String.IsNullOrWhiteSpace(TMSSetting.ERPPassword) Then
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";User ID=" & TMSSetting.ERPUser & ";Password=" & TMSSetting.ERPPassword
        Else
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        End If
        'EconnectStr = "Data Source=" & GetAppSettings.GetValue("SQLServer") & "; Initial Catalog=" & GPCompany & "Integrated Security=SSPI;Trusted_Connection=True"
        'EconnectStr = "Data Source=" & TMSSetting.ERPCertificate.ToString & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        'EconnectStr = "Data Source=NGLGP2013R2; Initial Catalog=TWO;Integrated Security=SSPI;Trusted_Connection=True"


        Dim Counter As Integer = 0
        Dim NumberofLines As Integer = 0
        Dim InvTransferID As String = ""
        Dim sIntegrationErrors As New List(Of String)
        Dim sLogMsgs As New List(Of String)
        Dim PorDAddressInfo As New GPDataIntegrationSTructure.FromAddress
        Dim ProAbb As String = ""
        Dim LaneValue As String = ""
        Dim LineNumber As Integer = 0
        Dim sErrors As String = ""
        Dim HeaderConn = New SqlConnection(EconnectStr)
        Dim HeaderCom As New SqlCommand
        Dim HeaderRec As SqlDataReader
        Dim LineConn = New SqlConnection(EconnectStr)
        Dim LineCom As New SqlCommand
        Dim LineRec As SqlDataReader
        Try

            HeaderConn.Open()

            HeaderCom.Connection = HeaderConn

            LineConn.Open()

            LineCom.Connection = LineConn


            'Integrate Sales Order
            'Create TMS Header & Line
            'Modified by RHR for v-7.0.5.102 on 11/10/2016
            'Dim oBookHeaders As New List(Of TMS.clsBookHeaderObject705)
            'Dim oBookDetails As New List(Of TMS.clsBookDetailObject705)
            Dim oBookHeaders As New List(Of tmsintegrationservices.clsBookHeaderObject705)
            Dim oBookDetails As New List(Of tmsintegrationservices.clsBookDetailObject705)

            'Modified by RHR for v-7.0.5.102 on 11/10/2016
            'Not needed with web services
            'Dim oConfig As New UserConfiguration()
            'With oConfig
            '    .AdminEmail = Me.AdminEmail
            '    .AutoRetry = Me.AutoRetry
            '    .Database = Me.Database
            '    .DBServer = Me.DBServer
            '    .Debug = Me.Debug
            '    .FromEmail = Me.FromEmail
            '    .GroupEmail = Me.GroupEmail
            '    .LogFile = Me.LogFile
            '    .SMTPServer = Me.SMTPServer
            '    .UserName = "System Download"
            '    .WSAuthCode = "NGLSystem"
            '    .WCFAuthCode = "NGLSystem"
            '    .WCFURL = ""
            '    .WCFTCPURL = ""
            '    .ConnectionString = Me.ConnectionString

            'End With

            'Modified by RHR for v-7.0.5.102 on 11/10/2016
            'Dim oBookIntegration As New TMS.clsBook(oConfig)
            Dim oBookIntegration As New tmsintegrationservices.DTMSERPIntegration()
            oBookIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oBookIntegration.UseDefaultCredentials = True
            Else
                oBookIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If

            Dim InvTransferProcessed As New List(Of String)
            Try

                For Each InvTransfer In InvTransferNumbers

                    Dim oBookHeader As New tmsintegrationservices.clsBookHeaderObject705

                    HeaderCom.CommandText = "select * from SVC00700 where ORDDOCID = '" & InvTransfer.InvTransferID.ToString & "'"

                    HeaderRec = HeaderCom.ExecuteReader

                    Try


                        HeaderRec.Read()

                        If (HeaderRec.HasRows) Then
                            'If Me.Debug Then Log("Attempting to process Order: " & HeaderRec("SOPNUMBE").ToString())
                            sErrors = ""
                            'Modified by RHR for v-7.0.5.102 on 11/10/2016
                            '  Scott commented this line out as there is a different field that I think is better
                            'ProAbb = oBookIntegration.GetCompAbrevByNumberOrAlpha(TMSSetting.TMSAuthCode, 0, HeaderRec("LOCNCODE").ToString, "", sErrors)
                            ProAbb = oBookIntegration.GetCompAbrevByNumberOrAlpha(TMSSetting.TMSAuthCode, 0, HeaderRec("TRNSFLOC").ToString, "", sErrors)

                            'ProAbb = oBookIntegration.GetCompAbrevByNumberOrAlpha(0, "WAREHOUSE", "", sErrors)

                            If (String.IsNullOrWhiteSpace(sErrors) AndAlso String.IsNullOrWhiteSpace(ProAbb)) Then
                                ' skip order if failed

                                If Me.Debug Then
                                    Log("Could not process inventory transfer: " & HeaderRec("ORDDOCID").ToString() & " Due to a missing Company Pro Abb for Comp Alpha Code: " & HeaderRec("TRNSFLOC").ToString())
                                End If

                                HeaderRec.Close()


                            Else
                                'If Me.Debug Then Log("Running CalculateLane.")
                                LaneValue = GPFunctions.CalculateLane(EconnectStr, 5, HeaderRec("ORDDOCID").ToString, 1, HeaderRec("ORDDOCID").ToString, ProAbb, 0, sErrors)

                                If (LaneValue = "Error") Then
                                    If Me.Verbose Or Me.Debug Then
                                        Log("Error when processing Lane for Inventory Trnasfer: " & HeaderRec("ORDDOCID").ToString())

                                    End If

                                    HeaderRec.Close()

                                    ' REturn more information 
                                    'LogError("Error when calculating Lane for order: ", sErrors, Me.AdminEmail)

                                Else
                                    'If Me.Debug Then Log("Reading SO informaiton using Lane Number: " & LaneValue)
                                    InvTransferID = Trim(HeaderRec("ORDDOCID").ToString)
                                    ' Process Order to TMS

                                    ' Insert TMSHeader
                                    oBookHeader.PONumber = Trim(HeaderRec("ORDDOCID").ToString())
                                    oBookHeader.POdate = Trim(HeaderRec("ORDRDATE").ToString())
                                    oBookHeader.POShipdate = Trim(HeaderRec("ORDRDATE").ToString())
                                    oBookHeader.POVendor = GPFunctions.CalculateLane(EconnectStr, 5, HeaderRec("ORDDOCID").ToString, 1, HeaderRec("ORDDOCID").ToString, ProAbb, 0, sErrors)
                                    'oBookHeader.POBuyer = HeaderRec("").ToString
                                    oBookHeader.POBuyer = ""
                                    'oBookHeader.POFrt = HeaderRec("").ToString
                                    ' Work with Rob to determine
                                    oBookHeader.POFrt = GPFunctions.GetShippingMethod(EconnectStr, 5, HeaderRec("ORDDOCID").ToString, 16384, HeaderRec("ORDDOCID").ToString, 0)

                                    'Modified by SEM for v-7.0.6.103 Update on 02/22/2017
                                    'There is no frieght or extended cost on the inventory transfer

                                    'Double.TryParse(HeaderRec("FRTAMNT").ToString(), oBookHeader.POTotalFrt)
                                    'Double.TryParse(HeaderRec("EXTDCOST").ToString, oBookHeader.POTotalCost)
                                    'If Me.Debug Then Log("Getting Order Weight For: " & HeaderRec("SOPNUMBE").ToString())
                                    oBookHeader.POWgt = GPFunctions.GetInventoryTransferTotalWeight(EconnectStr, HeaderRec("ORDDOCID").ToString)
                                    'oBookHeader.POCube = HeaderRec("").ToString
                                    oBookHeader.POCube = 0
                                    oBookHeader.POQty = CInt(GPFunctions.GetInventoryTransferTotalQuantity(EconnectStr, HeaderRec("ORDDOCID").ToString))
                                    'oBookHeader.POPallets = HeaderRec("").ToString
                                    'Need to define a Pallet formula
                                    oBookHeader.POPallets = CInt(GPFunctions.CalculatePallets(EconnectStr, HeaderRec("ORDDOCID").ToString, 5, Me.AdminEmail, 1))
                                    'oBookHeader.POLines = HeaderRec("").ToString
                                    oBookHeader.POLines = 0
                                    oBookHeader.POConfirm = False
                                    oBookHeader.PODefaultCustomer = "0"
                                    'oBookHeader.PODefaultCarrier = HeaderRec("").ToString
                                    ' Talk to Rob on this field below
                                    oBookHeader.PODefaultCarrier = 0
                                    oBookHeader.POReqDate = Trim(HeaderRec("ORDRDATE").ToString())
                                    ' Write funtion to get shipping instructions
                                    oBookHeader.POShipInstructions = ""
                                    oBookHeader.POCooler = False
                                    oBookHeader.POFrozen = False
                                    oBookHeader.PODry = False
                                    oBookHeader.POTemp = GPFunctions.GetTemp(EconnectStr, 5, HeaderRec("ORDRDATE").ToString, 1)
                                    oBookHeader.POCarType = ""
                                    oBookHeader.POShipVia = ""
                                    oBookHeader.POShipViaType = ""
                                    oBookHeader.POConsigneeNumber = ""
                                    oBookHeader.POCustomerPO = "PolyQuest"
                                    oBookHeader.POOtherCosts = 0
                                    '  Need to determine how this will be set. for now, 0 =New
                                    ' Ship Confirmation, send a 3, 5 is  an order
                                    oBookHeader.POStatusFlag = 5
                                    oBookHeader.POOrderSequence = 0
                                    oBookHeader.POChepGLID = ""
                                    oBookHeader.POCarrierEquipmentCodes = ""
                                    oBookHeader.POCarrierTypeCode = ""
                                    oBookHeader.POPalletPositions = ""
                                    ' Modified by RHR for v-7.0.5.102 on 10/05/20126
                                    ' We do not use the ReqShipDate for appointment informaiton 
                                    'oBookHeader.POSchedulePUDate = HeaderRec("ReqShipDate").ToString
                                    oBookHeader.POSchedulePUDate = ""
                                    oBookHeader.POSchedulePUTime = ""
                                    'oBookHeader.POScheduleDelDate = HeaderRec("ReqShipDate").ToString
                                    oBookHeader.POScheduleDelDate = ""
                                    oBookHeader.POSCheduleDelTime = ""
                                    oBookHeader.POActPUDate = ""
                                    oBookHeader.POActPUTime = ""
                                    oBookHeader.POActDelDate = ""
                                    oBookHeader.POActDelTime = ""
                                    oBookHeader.POOrigCompNumber = ""
                                    'If Me.Debug Then Log("Reading Address Info For Order Number: " & SOPNumber)

                                    'Modified by SEM for v-7.0.6.103 Update on 02/22/2017
                                    'changing logic for GP Inventory Transfer Order origin and dest. 

                                    'PorDAddressInfo = GPFunctions.GetPorDAddress(EconnectStr, HeaderRec("TRNSFLOC"))

                                    'If (PorDAddressInfo.AddressID = "No Address") Then '

                                    'LogError("No Address found", "There was no address found for Inventnory Transfer: " & InvTransfer, Me.AdminEmail)
                                    'Report to address found

                                    'Else

                                    'oBookHeader.POOrigName = Trim(PorDAddressInfo.AddressName)
                                    'oBookHeader.POOrigCompAlphaCode = Trim(PorDAddressInfo.AddressID)
                                    'oBookHeader.POOrigAddress1 = Trim(PorDAddressInfo.Address1)
                                    'oBookHeader.POOrigAddress2 = Trim(PorDAddressInfo.Address2)
                                    'oBookHeader.POOrigAddress3 = Trim(PorDAddressInfo.Address3)
                                    'oBookHeader.POOrigCity = Trim(PorDAddressInfo.City)
                                    'oBookHeader.POOrigState = Trim(PorDAddressInfo.State)
                                    'oBookHeader.POOrigCountry = Trim(If(String.IsNullOrWhiteSpace(c.GPFunctionsForceDefaultCountry), PorDAddressInfo.Country, c.GPFunctionsForceDefaultCountry))
                                    'oBookHeader.POOrigZip = Trim(PorDAddressInfo.ZipCode)
                                    'oBookHeader.POOrigContactPhone = Trim(PorDAddressInfo.Phone)
                                    'oBookHeader.POOrigContactPhoneExt = ""
                                    'oBookHeader.POOrigContactFax = Trim(PorDAddressInfo.Fax)

                                    'End If
                                    oBookHeader.POOrigName = Trim(HeaderRec("TRNSFLOC").ToString())
                                    oBookHeader.POOrigCompAlphaCode = Trim(HeaderRec("TRNSFLOC").ToString())
                                    oBookHeader.POOrigAddress1 = Trim(HeaderRec("ADDRESS1").ToString())
                                    oBookHeader.POOrigAddress2 = Trim(HeaderRec("ADDRESS2").ToString())
                                    oBookHeader.POOrigCity = Trim(HeaderRec("CITY").ToString())
                                    oBookHeader.POOrigState = Trim(HeaderRec("STATE").ToString())
                                    'oBookHeader.POOrigCountry = Trim(If(String.IsNullOrWhiteSpace(c.GPFunctionsForceDefaultCountry), PorDAddressInfo.Country, c.GPFunctionsForceDefaultCountry))

                                    oBookHeader.POOrigCountry = Trim(If(String.IsNullOrWhiteSpace(c.GPFunctionsForceDefaultCountry), Trim(HeaderRec("COUNTRY").ToString), c.GPFunctionsForceDefaultCountry))
                                    oBookHeader.POOrigZip = Trim(HeaderRec("ZIPCODE").ToString())
                                    'oBookHeader.POOrigContactPhone = Trim(HeaderRec("TRNSFLOC").ToString())
                                    'oBookHeader.POOrigContactPhoneExt = ""
                                    'oBookHeader.POOrigContactFax = Trim(HeaderRec("TRNSFLOC").ToString())


                                    PorDAddressInfo = GPFunctions.GetPorDAddress(EconnectStr, HeaderRec("LOCNCODE"))

                                    If (PorDAddressInfo.AddressID = "No Address") Then

                                        LogError("No Address found", "There was no address found for Inventnory Transfer: " & InvTransfer.ToString, Me.AdminEmail)
                                        'Report to address found

                                    Else

                                        oBookHeader.PODestName = Trim(PorDAddressInfo.AddressName)
                                        oBookHeader.PODestCompAlphaCode = Trim(PorDAddressInfo.AddressID)
                                        oBookHeader.PODestAddress1 = Trim(PorDAddressInfo.Address1)
                                        oBookHeader.PODestAddress2 = Trim(PorDAddressInfo.Address2)
                                        oBookHeader.PODestAddress3 = Trim(PorDAddressInfo.Address3)
                                        oBookHeader.PODestCity = Trim(PorDAddressInfo.City)
                                        oBookHeader.PODestState = Trim(PorDAddressInfo.State)
                                        oBookHeader.PODestCountry = Trim(If(String.IsNullOrWhiteSpace(c.GPFunctionsForceDefaultCountry), PorDAddressInfo.Country, c.GPFunctionsForceDefaultCountry))
                                        oBookHeader.PODestZip = Trim(PorDAddressInfo.ZipCode)
                                        oBookHeader.PODestContactPhone = Trim(PorDAddressInfo.Phone)
                                        oBookHeader.PODestContactPhoneExt = ""
                                        oBookHeader.PODestContactFax = Trim(PorDAddressInfo.Fax)

                                        oBookHeader.PODestCompNumber = ""
                                        'oBookHeader.PODestCompAlphaCode = Trim(HeaderRec("PRSTADCD").ToString())
                                        'oBookHeader.PODestName = Trim(HeaderRec("ShipToName").ToString())
                                        'oBookHeader.PODestAddress1 = Trim(HeaderRec("ADDRESS1").ToString())
                                        'oBookHeader.PODestAddress2 = Trim(HeaderRec("ADDRESS2").ToString())
                                        'oBookHeader.PODestAddress3 = Trim(HeaderRec("ADDRESS1").ToString())
                                        'oBookHeader.PODestCity = Trim(HeaderRec("CITY").ToString())
                                        'oBookHeader.PODestState = Trim(HeaderRec("STATE").ToString())
                                        'oBookHeader.PODestCountry = Trim(If(String.IsNullOrWhiteSpace(c.GPFunctionsForceDefaultCountry), HeaderRec("COUNTRY").ToString(), c.GPFunctionsForceDefaultCountry))
                                        'oBookHeader.PODestZip = Trim(HeaderRec("ZIPCODE").ToString())
                                        'oBookHeader.PODestContactPhone = Trim(HeaderRec("PHNUMBR1").ToString())
                                        'oBookHeader.PODestContactPhoneExt = ""
                                        'oBookHeader.PODestContactFax = Trim(HeaderRec("FAXNUMBR").ToString())

                                    End If

                                    oBookHeader.POInbound = False
                                    oBookHeader.POPalletExchange = False
                                    oBookHeader.POPalletType = ""
                                    oBookHeader.POComments = ""
                                    oBookHeader.POCommentsConfidential = ""
                                    oBookHeader.PODefaultRouteSequence = 0
                                    'Modified by RHR v-7.0.5.102 09/26/2016 removed code to populate the PORouteGuideNumber with the Lane Number
                                    'as this is not required.
                                    'oBookHeader.PORouteGuideNumber = GPFunctions.CalculateLane(EconnectStr, 1, HeaderRec("SOPNumbe").ToString, 16384, HeaderRec("SOPNumbe").ToString, ProAbb, SOPType, sErrors)
                                    oBookHeader.PORouteGuideNumber = ""
                                    oBookHeader.POCompLegalEntity = Trim(TMSSetting.LegalEntity.ToString())
                                    oBookHeader.POCompAlphaCode = Trim(HeaderRec("LOCNCODE"))
                                    oBookHeader.POModeTypeControl = GPFunctions.GetTransporationMode(HeaderRec("ORDDOCID").ToString, "INVTRN")
                                    oBookHeader.POUser1 = ""
                                    oBookHeader.POUser2 = ""
                                    oBookHeader.POUser3 = ""
                                    oBookHeader.POUser4 = ""
                                    oBookHeader.POAPGLNumber = ""

                                    NumberofLines = 0
                                    'If Me.Debug Then Log("Calculating # Lines for SO Order Number: " & SOPNumber)
                                    LineCom.CommandText = "select count(*) from SVC00701 where ORDDOCID = '" & InvTransfer.ToString & "'"

                                    LineRec = LineCom.ExecuteReader

                                    LineRec.Read()

                                    If (LineRec.HasRows) Then

                                        NumberofLines = LineRec(0).ToString

                                    Else

                                        NumberofLines = 0

                                    End If

                                    LineRec.Close()

                                    Try
                                        'Modified by RHR for v-7.0.5.102 on 11/10/2016
                                        'If Me.Debug Then Log("Checking if orer has changed For Order Number: " & SOPNumber)
                                        sErrors = ""
                                        OrderChanged = oBookIntegration.HasOrderChanged(TMSSetting.TMSAuthCode, oBookHeader, NumberofLines, OrdChangeStr, False, False, sErrors)
                                        If Not String.IsNullOrWhiteSpace(sErrors) Then Log("Process Order Changed Error: " & sErrors)
                                    Catch ex As Exception

                                        'MsgBox("Error: " & ex.Message)

                                        LogError("Error in Processing Inventory Transfer - at the Order Change.  The Error is:  ", ex.Message, Me.AdminEmail)

                                    End Try

                                    If (Not OrderChanged) Then
                                        If Me.Debug Then Log("Skiping order because the order has not changed: " & HeaderRec("ORDDOCID").ToString())
                                        HeaderRec.Close()

                                    Else

                                        InvTransferProcessed.Add(InvTransfer.ToString)
                                        'If Me.Debug Then Log("Order is new or has changed adding to Book Header collection: " & HeaderRec("SOPNUMBE").ToString())
                                        oBookHeaders.Add(oBookHeader)
                                        'Modified by RHR for v-7.0.5.102 10/17/2016
                                        LineCom.CommandText = String.Format(c.GPFunctionsTOItemDetails, InvTransfer.ToString)
                                        'LineCom.CommandText = "select * from sop10200 where SOPTYPE = " & SOPType & " and SOPNUMBE = '" & SOPOrder.SOPOrders.ToString & "'"

                                        LineRec = LineCom.ExecuteReader
                                        Dim LineCols = LineRec.GetSchemaTable().Rows.Cast(Of DataRow)().[Select](Function(row) TryCast(row("ColumnName"), String)).ToList()

                                        Counter = 0

                                        'If Me.Debug Then Log("Reading Item Details For Order Number: " & SOPNumber)
                                        While LineRec.Read

                                            Dim oBookDetail As New tmsintegrationservices.clsBookDetailObject705

                                            Counter = Counter + 1

                                            oBookDetail.ItemPONumber = InvTransfer.ToString
                                            If (LineCols.Contains("ItemNumber")) Then oBookDetail.ItemNumber = Trim(LineRec("ItemNumber").ToString())
                                            If (LineCols.Contains("ItemCost")) Then Double.TryParse(LineRec("ItemCost").ToString(), oBookDetail.ItemCost)
                                            If (LineCols.Contains("QtyOrdered")) Then GPFunctions.TryParseInt(LineRec("QtyOrdered").ToString(), oBookDetail.QtyOrdered)
                                            If (LineCols.Contains("Weight")) Then Double.TryParse(LineRec("Weight").ToString(), oBookDetail.Weight)
                                            If (LineCols.Contains("FixOffInvAllow")) Then Double.TryParse(LineRec("FixOffInvAllow").ToString(), oBookDetail.FixOffInvAllow)
                                            If (LineCols.Contains("FixFrtAllow")) Then Double.TryParse(LineRec("FixFrtAllow").ToString(), oBookDetail.FixFrtAllow)
                                            If (LineCols.Contains("FreightCost")) Then Double.TryParse(LineRec("FreightCost").ToString(), oBookDetail.FreightCost)
                                            If (LineCols.Contains("ItemCost")) Then Double.TryParse(LineRec("ItemCost").ToString(), oBookDetail.ItemCost)
                                            If (LineCols.Contains("Cube")) Then GPFunctions.TryParseInt(LineRec("Cube").ToString(), oBookDetail.Cube)
                                            If (LineCols.Contains("Pack")) Then GPFunctions.TryParseInt(LineRec("Pack").ToString(), oBookDetail.Pack)
                                            If (LineCols.Contains("Size")) Then oBookDetail.Size = Trim(LineRec("Size").ToString())
                                            If (LineCols.Contains("Description")) Then oBookDetail.Description = Trim(LineRec("Description").ToString())
                                            If (LineCols.Contains("Hazmat")) Then oBookDetail.Hazmat = Trim(LineRec("Hazmat").ToString())
                                            If (LineCols.Contains("Brand")) Then oBookDetail.Brand = Trim(LineRec("Brand").ToString())
                                            If (LineCols.Contains("CostCenter")) Then oBookDetail.CostCenter = Trim(LineRec("CostCenter").ToString())
                                            If (LineCols.Contains("LotNumber")) Then oBookDetail.LotNumber = Trim(LineRec("LotNumber").ToString())
                                            If (LineCols.Contains("LotExpirationDate")) Then oBookDetail.LotExpirationDate = Trim(LineRec("LotExpirationDate").ToString())
                                            If (LineCols.Contains("GTIN")) Then oBookDetail.GTIN = Trim(LineRec("GTIN").ToString())
                                            If (LineCols.Contains("CustItemNumber")) Then oBookDetail.CustItemNumber = Trim(LineRec("CustItemNumber").ToString())
                                            oBookDetail.CustomerNumber = "0" 'Modified by RHR for v-7.0.5.102 10/17/2016 we do not use the CUSTNMBR here  HeaderRec("CUSTNMBR").ToString
                                            oBookDetail.POOrderSequence = 0
                                            If (LineCols.Contains("PalletType")) Then oBookDetail.PalletType = Trim(LineRec("PalletType").ToString())
                                            If (LineCols.Contains("POItemHazmatTypeCode")) Then oBookDetail.POItemHazmatTypeCode = Trim(LineRec("POItemHazmatTypeCode").ToString())
                                            If (LineCols.Contains("POItem49CFRCode")) Then oBookDetail.POItem49CFRCode = Trim(LineRec("POItem49CFRCode").ToString())
                                            If (LineCols.Contains("POItemIATACode")) Then oBookDetail.POItemIATACode = Trim(LineRec("POItemIATACode").ToString())
                                            If (LineCols.Contains("POItemDOTCode")) Then oBookDetail.POItemDOTCode = Trim(LineRec("POItemDOTCode").ToString())
                                            If (LineCols.Contains("POItemMarineCode")) Then oBookDetail.POItemMarineCode = Trim(LineRec("POItemMarineCode").ToString())
                                            If (LineCols.Contains("POItemNMFCClass")) Then oBookDetail.POItemNMFCClass = Trim(LineRec("POItemNMFCClass").ToString())
                                            If (LineCols.Contains("POItemFAKClass")) Then oBookDetail.POItemFAKClass = Trim(LineRec("POItemFAKClass").ToString())
                                            If (LineCols.Contains("POItemLimitedQtyFlag")) Then Boolean.TryParse(LineRec("POItemLimitedQtyFlag").ToString(), oBookDetail.POItemLimitedQtyFlag)
                                            If (LineCols.Contains("POItemPallets")) Then Double.TryParse(LineRec("POItemPallets").ToString(), oBookDetail.POItemPallets)
                                            If (LineCols.Contains("POItemTies")) Then Double.TryParse(LineRec("POItemTies").ToString(), oBookDetail.POItemTies)
                                            If (LineCols.Contains("POItemHighs")) Then Double.TryParse(LineRec("POItemHighs").ToString(), oBookDetail.POItemHighs)
                                            If (LineCols.Contains("POItemQtyPalletPercentage")) Then Double.TryParse(LineRec("POItemQtyPalletPercentage").ToString(), oBookDetail.POItemQtyPalletPercentage)
                                            If (LineCols.Contains("POItemQtyLength")) Then Double.TryParse(LineRec("POItemQtyLength").ToString(), oBookDetail.POItemQtyLength)
                                            If (LineCols.Contains("POItemQtyWidth")) Then Double.TryParse(LineRec("POItemQtyWidth").ToString(), oBookDetail.POItemQtyWidth)
                                            If (LineCols.Contains("POItemQtyHeight")) Then Double.TryParse(LineRec("POItemQtyHeight").ToString(), oBookDetail.POItemQtyHeight)
                                            If (LineCols.Contains("POItemStackable")) Then Boolean.TryParse(LineRec("POItemStackable").ToString(), oBookDetail.POItemStackable)
                                            If (LineCols.Contains("POItemLevelOfDensity")) Then GPFunctions.TryParseInt(LineRec("POItemLevelOfDensity").ToString(), oBookDetail.POItemLevelOfDensity)
                                            oBookDetail.POItemCompLegalEntity = Trim(TMSSetting.LegalEntity.ToString())
                                            oBookDetail.POItemCompAlphaCode = Trim(HeaderRec("LOCNCODE"))
                                            'oBookDetail.POItemCompAlphaCode = "WAREHOUSE"
                                            If (LineCols.Contains("POItemNMFCSubClass")) Then oBookDetail.POItemNMFCSubClass = Trim(LineRec("POItemNMFCSubClass").ToString())
                                            If (LineCols.Contains("POItemUser1")) Then oBookDetail.POItemUser1 = Trim(LineRec("POItemUser1").ToString())
                                            If (LineCols.Contains("POItemUser2")) Then oBookDetail.POItemUser2 = Trim(LineRec("POItemUser2").ToString())
                                            If (LineCols.Contains("POItemUser3")) Then oBookDetail.POItemUser3 = Trim(LineRec("POItemUser3").ToString())
                                            If (LineCols.Contains("POItemUser4")) Then oBookDetail.POItemUser4 = Trim(LineRec("POItemUser4").ToString())
                                            If (LineCols.Contains("POItemWeightUnitOfMeasure")) Then oBookDetail.POItemWeightUnitOfMeasure = Trim(LineRec("POItemWeightUnitOfMeasure").ToString())
                                            If (LineCols.Contains("POItemCubeUnitOfMeasure")) Then oBookDetail.POItemCubeUnitOfMeasure = Trim(LineRec("POItemCubeUnitOfMeasure").ToString())
                                            If (LineCols.Contains("POItemDimensionUnitOfMeasure")) Then oBookDetail.POItemDimensionUnitOfMeasure = Trim(LineRec("POItemDimensionUnitOfMeasure").ToString())

                                            'If Me.Debug Then Log("Adding Item Detail  Item Number: " & LineRec("Itemnmbr").ToString)
                                            oBookDetails.Add(oBookDetail)

                                            oBookDetail = Nothing

                                        End While

                                        ' Report something if no line recvords
                                        If (Counter = 0) Then

                                            If Me.Verbose Then Log("No Line records to process for Inventory Tranfer #: " & InvTransfer.ToString)

                                        End If

                                        LineRec.Close()

                                        HeaderRec.Close()

                                    End If

                                    If (Not HeaderRec.IsClosed) Then

                                        HeaderRec.Close()

                                    End If

                                    oBookHeader = Nothing

                                End If


                            End If

                        Else

                            If Me.Verbose Then Log("Inventory Transfer In List, but query did not return header data  for SOP #: " & InvTransfer.ToString)

                        End If

                    Catch ex As Exception
                        Throw
                    Finally
                        'besure the reader is closed
                        Try
                            If Not HeaderRec Is Nothing AndAlso (Not HeaderRec.IsClosed) Then

                                HeaderRec.Close()

                            End If


                        Catch ex As Exception

                        End Try

                        Try
#Disable Warning BC42104 ' Variable 'LineRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                            If Not LineRec Is Nothing AndAlso (Not LineRec.IsClosed) Then
#Enable Warning BC42104 ' Variable 'LineRec' is used before it has been assigned a value. A null reference exception could result at runtime.

                                LineRec.Close()

                            End If


                        Catch ex As Exception

                        End Try
                    End Try
                    If Not oBookHeaders Is Nothing AndAlso oBookHeaders.Count() > 0 Then


                        If Me.Debug Then Log("Processing Book Object Data; Headers: " & oBookHeaders.Count().ToString() & " Details: " & oBookDetails.Count().ToString())
                        'Process the last order before submitting orderst to TMS
                        'Modified by RHR for v-7.0.5.102 on 11/10/2016
                        'Not needed when we use the web services
                        'oBookIntegration.RunSilentTenderAsync = False

                        'Modified by RHR for v-7.0.5.102 on 11/10/2016
                        Dim aBookHeaders As tmsintegrationservices.clsBookHeaderObject705() = oBookHeaders.ToArray()
                        Dim aBookDetails As tmsintegrationservices.clsBookDetailObject705()
                        sErrors = ""
                        If Not oBookDetails Is Nothing AndAlso oBookDetails.Count() > 0 Then aBookDetails = oBookDetails.ToArray()
                        'save changes to database 
#Disable Warning BC42104 ' Variable 'aBookDetails' is used before it has been assigned a value. A null reference exception could result at runtime.
                        Dim oResults As TMS.Configuration.ProcessDataReturnValues = oBookIntegration.ProcessBookData705(TMSSetting.TMSAuthCode, aBookHeaders, aBookDetails, sErrors)
#Enable Warning BC42104 ' Variable 'aBookDetails' is used before it has been assigned a value. A null reference exception could result at runtime.
                        Dim ReturnMessage As String = sErrors
                        If Me.Debug Then Log("Results = " & oResults.ToString())
                        Select Case oResults
                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Data Connection Failure! could not process Inventory Transfer information")
                                Else
                                    sIntegrationErrors.Add("Data Connection Failure! could not process Inventory Transfer information:  " & ReturnMessage)
                                End If

                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Integration Failure! could not process Inventory Transfer information")
                                Else
                                    generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                                End If

                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Integration Had Errors! could not  process Inventory Transfer information")
                                Else
                                    generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                                End If

                                'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                'blnRet = True
                                'End If
                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Data Validation Failure! could not  process Inventory Transfer  information")
                                Else
                                    generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                                End If

                                'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                'blnRet = True
                                'End If
                            Case Else
                                'success
                                Dim strNumbers As String = "N/A"
                                If Not InvTransferProcessed Is Nothing AndAlso InvTransferProcessed.Count() > 0 Then
                                    strNumbers = String.Join(", ", InvTransferProcessed)
                                End If

                                sLogMsgs.Add("Success! the following Inventory Transfer were processed: " & strNumbers)
                                'Processed = oResults.
                                'TODO: add code to send confirmation back to NAV that the orders were processed
                                'mark process and success
                                'blnRet = True
                        End Select

                        oResults = Nothing
                    End If
                    oBookHeaders.Clear()
                    oBookDetails.Clear()
                Next

                If Not sIntegrationErrors Is Nothing AndAlso sIntegrationErrors.Count() > 0 Then
                    LogError("Warning!  " & Source & " Had Errors: " & vbCrLf & String.Join(vbCrLf, sIntegrationErrors.ToArray()))
                End If
                'If Not sAdminErrors Is Nothing AndAlso sAdminErrors.Count() > 0 Then
                'LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sAdminErrors.ToArray()), Me.AdminEmail)
                'End If
                'If Not sOperationErrors Is Nothing AndAlso sOperationErrors.Count() > 0 Then
                'LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sOperationErrors.ToArray()), Me.GroupEmail)
                'End If
                'If Not sLogMsgs Is Nothing AndAlso sLogMsgs.Count() > 0 Then
                If Me.Verbose Then Log(Source & ": " & vbCrLf & String.Join(vbCrLf, sLogMsgs.ToArray()))
                'End If
                If Debug Then Log("Process  Inventory Transfer  Data Complete")

            Catch ex As Exception

                'MsgBox("Inner Try " & ex.Message)

                Log("There was an erorr processing Inventory Transfer  Orders.  The error is: " & ex.Message)


                'Report error
            Finally
                Try
                    ' End Routine
                    HeaderCom.Dispose()
                    HeaderConn.Dispose()

                    LineCom.Dispose()
                    LineConn.Dispose()
                Catch ex As Exception

                End Try
            End Try

        Catch ex As Exception
            Log(Source & ".ProcessGPInvTrsanfers Error!  Unexpected GP Process Inventory Transfer Error could not process any integration requests; the actual error is:  " & ex.Message)
        Finally
            GPFunctions = Nothing
            'Modified by RHR for v-7.0.6.104 on 08/28/2027 added finally statement to dispose of db connections
            Try
#Disable Warning BC42104 ' Variable 'HeaderRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not HeaderRec Is Nothing AndAlso (Not HeaderRec.IsClosed) Then HeaderRec.Close()
#Enable Warning BC42104 ' Variable 'HeaderRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not LineRec Is Nothing AndAlso (Not LineRec.IsClosed) Then LineRec.Close()
            Catch ex As Exception

            End Try
            Try
                If Not HeaderCom Is Nothing Then HeaderCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not LineCom Is Nothing Then LineCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not HeaderConn Is Nothing AndAlso Not HeaderConn.State = ConnectionState.Closed Then HeaderConn.Close()
                HeaderConn.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not LineConn Is Nothing AndAlso Not LineConn.State = ConnectionState.Closed Then LineConn.Close()
                LineConn.Dispose()
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function



    ''' <summary>
    ''' Process the GP Sales Orders
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <param name="c"></param>
    ''' <param name="SOPOrderNumbers"></param>
    ''' <param name="SOPType"></param>
    ''' <param name="TMSCountToProcess"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' </remarks>
    Public Function ProcessGPSOPOrdersDLLDirect(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByRef c As clsDefaultIntegrationConfiguration, ByVal SOPOrderNumbers As List(Of GPDataIntegrationSTructure.SOPOrders), ByVal SOPType As Int16, ByVal TMSCountToProcess As Int16) As Boolean

        Dim ReturnValue As Boolean = True
        Dim EconnectStr As String = ""
        Dim GetAppSettings = New AppSettingsReader
        Dim LaneNumber As String = ""
        Dim GPFunctions As New GPFunctions(c) 'Modified by RHR for v-7.0.5.102 10/14/2016
        Dim SOPCounter As Int16 = 0
        Dim OrderChanged As Boolean = False
        Dim OrdChangeStr As String = ""

        ' Set all the variables
        If Not String.IsNullOrWhiteSpace(TMSSetting.ERPUser) AndAlso Not String.IsNullOrWhiteSpace(TMSSetting.ERPPassword) Then
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";User ID=" & TMSSetting.ERPUser & ";Password=" & TMSSetting.ERPPassword
        Else
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        End If

        'EconnectStr = "Data Source=" & GetAppSettings.GetValue("SQLServer") & "; Initial Catalog=" & GPCompany & "Integrated Security=SSPI;Trusted_Connection=True"
        'EconnectStr = "Data Source=" & TMSSetting.ERPCertificate.ToString & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        'EconnectStr = "Data Source=NGLGP2013R2; Initial Catalog=TWO;Integrated Security=SSPI;Trusted_Connection=True"

        Dim HeaderConn = New SqlConnection(EconnectStr)
        Dim HeaderCom As New SqlCommand
        Dim HeaderRec As SqlDataReader
        Dim LineConn = New SqlConnection(EconnectStr)
        Dim LineCom As New SqlCommand
        Dim LineRec As SqlDataReader
        Dim Counter As Integer = 0
        Dim NumberofLines As Integer = 0
        Dim SOPNumber As String = ""
        Dim sIntegrationErrors As New List(Of String)
        Dim sLogMsgs As New List(Of String)
        Dim PorDAddressInfo As New GPDataIntegrationSTructure.FromAddress
        Dim ProAbb As String = ""
        Dim LaneValue As String = ""
        Dim LineNumber As Integer = 0

        Try

            HeaderConn.Open()

            HeaderCom.Connection = HeaderConn

            LineConn.Open()

            LineCom.Connection = LineConn


            'Integrate Sales Order
            'Create TMS Header & Line

            Dim oBookHeaders As New List(Of TMS.clsBookHeaderObject705)
            Dim oBookDetails As New List(Of TMS.clsBookDetailObject705)

            Dim oConfig As New UserConfiguration()
            With oConfig
                .AdminEmail = Me.AdminEmail
                .AutoRetry = Me.AutoRetry
                .Database = Me.Database
                .DBServer = Me.DBServer
                .Debug = Me.Debug
                .FromEmail = Me.FromEmail
                .GroupEmail = Me.GroupEmail
                .LogFile = Me.LogFile
                .SMTPServer = Me.SMTPServer
                .UserName = "System Download"
                .WSAuthCode = "NGLSystem"
                .WCFAuthCode = "NGLSystem"
                .WCFURL = ""
                .WCFTCPURL = ""
                .ConnectionString = Me.ConnectionString

            End With


            ' Here

            Dim oBookIntegration As New TMS.clsBook(oConfig)
            Dim SOOrdersProcessed As New List(Of String)
            Try

                For Each SOPOrder In SOPOrderNumbers


                    Dim oBookHeader As New TMS.clsBookHeaderObject705

                    If (SOPCounter < 99) Then

                        HeaderCom.CommandText = "select * from sop10100 where SOPTYPE = " & SOPType & " and SOPNUMBE = '" & SOPOrder.SOPOrders.ToString & "'"

                        HeaderRec = HeaderCom.ExecuteReader
                        Try


                            HeaderRec.Read()

                            If (HeaderRec.HasRows) Then
                                'If Me.Debug Then Log("Attempting to process Order: " & HeaderRec("SOPNUMBE").ToString())
                                Dim sErrors As String = ""
                                ProAbb = oBookIntegration.GetCompAbrevByNumberOrAlpha(0, HeaderRec("LOCNCODE").ToString, sErrors)
                                'ProAbb = oBookIntegration.GetCompAbrevByNumberOrAlpha(0, "WAREHOUSE", "", sErrors)

                                If (String.IsNullOrWhiteSpace(sErrors) AndAlso String.IsNullOrWhiteSpace(ProAbb)) Then
                                    ' skip order if failed

                                    If Me.Debug Then
                                        Log("Could not process order: " & HeaderRec("SOPNUMBE").ToString() & " Due to a missing Company Pro Abb for Comp Alpha Code: " & HeaderRec("LOCNCODE").ToString())
                                    End If

                                    HeaderRec.Close()


                                Else
                                    'If Me.Debug Then Log("Running CalculateLane.")
                                    LaneValue = GPFunctions.CalculateLane(EconnectStr, 1, HeaderRec("SOPNumbe").ToString, 16384, HeaderRec("SOPNumbe").ToString, ProAbb, SOPType, sErrors)

                                    If (LaneValue = "Error") Then
                                        If Me.Verbose Or Me.Debug Then
                                            Log("Error when processing Lane for order: " & HeaderRec("SOPNUMBE").ToString())

                                        End If

                                        HeaderRec.Close()

                                        ' REturn more information 
                                        'LogError("Error when calculating Lane for order: ", sErrors, Me.AdminEmail)

                                    Else
                                        'If Me.Debug Then Log("Reading SO informaiton using Lane Number: " & LaneValue)
                                        SOPNumber = Trim(HeaderRec("SOPNUMBE").ToString)
                                        ' Process Order to TMS

                                        ' Insert TMSHeader
                                        oBookHeader.PONumber = HeaderRec("SOPNUMBE").ToString
                                        oBookHeader.POdate = HeaderRec("DocDate").ToString
                                        oBookHeader.POShipdate = HeaderRec("DocDate").ToString
                                        oBookHeader.POVendor = GPFunctions.CalculateLane(EconnectStr, 1, HeaderRec("SOPNumbe").ToString, 16384, HeaderRec("SOPNumbe").ToString, ProAbb, SOPType, sErrors)
                                        'oBookHeader.POBuyer = HeaderRec("").ToString
                                        oBookHeader.POBuyer = ""
                                        'oBookHeader.POFrt = HeaderRec("").ToString
                                        ' Work with Rob to determine
                                        oBookHeader.POFrt = GPFunctions.GetShippingMethod(EconnectStr, 1, HeaderRec("SOPNumbe").ToString, 16384, HeaderRec("SOPNumbe").ToString, SOPType)

                                        Double.TryParse(HeaderRec("FRTAMNT").ToString(), oBookHeader.POTotalFrt)
                                        Double.TryParse(HeaderRec("EXTDCOST").ToString, oBookHeader.POTotalCost)
                                        'If Me.Debug Then Log("Getting Order Weight For: " & HeaderRec("SOPNUMBE").ToString())
                                        oBookHeader.POWgt = GPFunctions.GetSOPTotalWeight(EconnectStr, HeaderRec("SOPNUMBE").ToString, SOPType)
                                        'oBookHeader.POCube = HeaderRec("").ToString
                                        oBookHeader.POCube = 0
                                        oBookHeader.POQty = CInt(GPFunctions.GetSOPQuantity(EconnectStr, HeaderRec("SOPNUMBE").ToString, SOPType))
                                        'oBookHeader.POPallets = HeaderRec("").ToString
                                        'Need to define a Pallet formula
                                        oBookHeader.POPallets = CInt(GPFunctions.CalculatePallets(EconnectStr, HeaderRec("SOPNUMBE").ToString, 1, Me.AdminEmail, SOPType))
                                        'oBookHeader.POLines = HeaderRec("").ToString
                                        oBookHeader.POLines = 0
                                        oBookHeader.POConfirm = False
                                        oBookHeader.PODefaultCustomer = "0"
                                        'oBookHeader.PODefaultCarrier = HeaderRec("").ToString
                                        ' Talk to Rob on this field below
                                        oBookHeader.PODefaultCarrier = 0
                                        oBookHeader.POReqDate = HeaderRec("ReqShipDate").ToString
                                        ' Write funtion to get shipping instructions
                                        oBookHeader.POShipInstructions = ""
                                        oBookHeader.POCooler = False
                                        oBookHeader.POFrozen = False
                                        oBookHeader.PODry = False
                                        oBookHeader.POTemp = GPFunctions.GetTemp(EconnectStr, 1, HeaderRec("SOPNUMBE").ToString, SOPType)
                                        oBookHeader.POCarType = ""
                                        oBookHeader.POShipVia = ""
                                        oBookHeader.POShipViaType = ""
                                        oBookHeader.POConsigneeNumber = ""
                                        oBookHeader.POCustomerPO = HeaderRec("CSTPONBR").ToString
                                        oBookHeader.POOtherCosts = 0
                                        '  Need to determine how this will be set. for now, 0 =New
                                        ' Ship Confirmation, send a 3, 5 is  an order
                                        oBookHeader.POStatusFlag = 5
                                        oBookHeader.POOrderSequence = 0
                                        oBookHeader.POChepGLID = ""
                                        oBookHeader.POCarrierEquipmentCodes = ""
                                        oBookHeader.POCarrierTypeCode = ""
                                        oBookHeader.POPalletPositions = ""
                                        ' Modified by RHR for v-7.0.5.102 on 10/05/20126
                                        ' We do not use the ReqShipDate for appointment informaiton 
                                        'oBookHeader.POSchedulePUDate = HeaderRec("ReqShipDate").ToString
                                        oBookHeader.POSchedulePUDate = ""
                                        oBookHeader.POSchedulePUTime = ""
                                        'oBookHeader.POScheduleDelDate = HeaderRec("ReqShipDate").ToString
                                        oBookHeader.POScheduleDelDate = ""
                                        oBookHeader.POSCheduleDelTime = ""
                                        oBookHeader.POActPUDate = ""
                                        oBookHeader.POActPUTime = ""
                                        oBookHeader.POActDelDate = ""
                                        oBookHeader.POActDelTime = ""
                                        oBookHeader.POOrigCompNumber = ""
                                        'If Me.Debug Then Log("Reading Address Info For Order Number: " & SOPNumber)
                                        PorDAddressInfo = GPFunctions.GetPorDAddress(EconnectStr, HeaderRec("LOCNCODE"))
                                        If (PorDAddressInfo.AddressID = "No Address") Then

                                            LogError("No Address found", "There was no address found for SOP Order: " & SOPOrder.SOPOrders.ToString, Me.AdminEmail)
                                            'Report to address found

                                        Else

                                            oBookHeader.POOrigName = PorDAddressInfo.AddressName
                                            oBookHeader.POOrigCompAlphaCode = PorDAddressInfo.AddressID
                                            oBookHeader.POOrigAddress1 = PorDAddressInfo.Address1
                                            oBookHeader.POOrigAddress2 = PorDAddressInfo.Address2
                                            oBookHeader.POOrigAddress3 = PorDAddressInfo.Address3
                                            oBookHeader.POOrigCity = PorDAddressInfo.City
                                            oBookHeader.POOrigState = PorDAddressInfo.State
                                            oBookHeader.POOrigCountry = If(String.IsNullOrWhiteSpace(c.GPFunctionsForceDefaultCountry), PorDAddressInfo.Country, c.GPFunctionsForceDefaultCountry)
                                            oBookHeader.POOrigZip = PorDAddressInfo.ZipCode
                                            oBookHeader.POOrigContactPhone = PorDAddressInfo.Phone
                                            oBookHeader.POOrigContactPhoneExt = ""
                                            oBookHeader.POOrigContactFax = PorDAddressInfo.Fax

                                        End If

                                        oBookHeader.PODestCompNumber = ""
                                        oBookHeader.PODestCompAlphaCode = HeaderRec("PRSTADCD").ToString
                                        oBookHeader.PODestName = HeaderRec("ShipToName").ToString
                                        oBookHeader.PODestAddress1 = HeaderRec("ADDRESS1").ToString
                                        oBookHeader.PODestAddress2 = HeaderRec("ADDRESS2").ToString
                                        oBookHeader.PODestAddress3 = HeaderRec("ADDRESS1").ToString
                                        oBookHeader.PODestCity = HeaderRec("CITY").ToString
                                        oBookHeader.PODestState = HeaderRec("STATE").ToString
                                        oBookHeader.PODestCountry = If(String.IsNullOrWhiteSpace(c.GPFunctionsForceDefaultCountry), HeaderRec("COUNTRY").ToString(), c.GPFunctionsForceDefaultCountry)
                                        oBookHeader.PODestZip = HeaderRec("ZIPCODE").ToString
                                        oBookHeader.PODestContactPhone = HeaderRec("PHNUMBR1").ToString
                                        oBookHeader.PODestContactPhoneExt = ""
                                        oBookHeader.PODestContactFax = HeaderRec("FAXNUMBR").ToString
                                        oBookHeader.POInbound = False
                                        oBookHeader.POPalletExchange = False
                                        oBookHeader.POPalletType = ""
                                        oBookHeader.POComments = ""
                                        oBookHeader.POCommentsConfidential = ""
                                        oBookHeader.PODefaultRouteSequence = 0
                                        'Modified by RHR v-7.0.5.102 09/26/2016 removed code to populate the PORouteGuideNumber with the Lane Number
                                        'as this is not required.
                                        'oBookHeader.PORouteGuideNumber = GPFunctions.CalculateLane(EconnectStr, 1, HeaderRec("SOPNumbe").ToString, 16384, HeaderRec("SOPNumbe").ToString, ProAbb, SOPType, sErrors)
                                        oBookHeader.PORouteGuideNumber = ""
                                        oBookHeader.POCompLegalEntity = TMSSetting.LegalEntity.ToString
                                        oBookHeader.POCompAlphaCode = HeaderRec("LOCNCODE")
                                        oBookHeader.POModeTypeControl = GPFunctions.GetTransporationMode(HeaderRec("SOPNUMBE").ToString, "SOP")
                                        oBookHeader.POUser1 = ""
                                        oBookHeader.POUser2 = ""
                                        oBookHeader.POUser3 = ""
                                        oBookHeader.POUser4 = ""
                                        oBookHeader.POAPGLNumber = ""

                                        NumberofLines = 0
                                        'If Me.Debug Then Log("Calculating # Lines for SO Order Number: " & SOPNumber)
                                        LineCom.CommandText = "select count(*) from sop10200 where SOPTYPE = " & SOPType & " and SOPNUMBE = '" & SOPOrder.SOPOrders.ToString & "'"

                                        LineRec = LineCom.ExecuteReader

                                        LineRec.Read()

                                        If (LineRec.HasRows) Then

                                            NumberofLines = LineRec(0).ToString

                                        Else

                                            NumberofLines = 0

                                        End If

                                        LineRec.Close()

                                        Try
                                            'If Me.Debug Then Log("Checking if orer has changed For Order Number: " & SOPNumber)
                                            OrderChanged = oBookIntegration.HasOrderChanged(oBookHeader, NumberofLines, OrdChangeStr, False, False)

                                        Catch ex As Exception

                                            'MsgBox("Error: " & ex.Message)

                                            LogError("Error in Processing SOP Orders - at the Order Change.  The Error is:  ", ex.Message, Me.AdminEmail)

                                        End Try

                                        If (Not OrderChanged) Then
                                            If Me.Debug Then Log("Skiping order because the order has not changed: " & HeaderRec("SOPNUMBE").ToString())
                                            HeaderRec.Close()

                                        Else
                                            SOOrdersProcessed.Add(SOPOrder.SOPOrders)
                                            'If Me.Debug Then Log("Order is new or has changed adding to Book Header collection: " & HeaderRec("SOPNUMBE").ToString())
                                            oBookHeaders.Add(oBookHeader)
                                            'Modified by RHR for v-7.0.5.102 10/17/2016
                                            LineCom.CommandText = String.Format(c.GPFunctionsSOItemDetails, SOPOrder.SOPOrders.ToString)
                                            'LineCom.CommandText = "select * from sop10200 where SOPTYPE = " & SOPType & " and SOPNUMBE = '" & SOPOrder.SOPOrders.ToString & "'"

                                            LineRec = LineCom.ExecuteReader
                                            Dim LineCols = LineRec.GetSchemaTable().Rows.Cast(Of DataRow)().[Select](Function(row) TryCast(row("ColumnName"), String)).ToList()

                                            Counter = 0

                                            'If Me.Debug Then Log("Reading Item Details For Order Number: " & SOPNumber)
                                            While LineRec.Read

                                                Dim oBookDetail As New TMS.clsBookDetailObject705

                                                Counter = Counter + 1

                                                oBookDetail.ItemPONumber = SOPNumber
                                                If (LineCols.Contains("ItemNumber")) Then oBookDetail.ItemNumber = LineRec("ItemNumber").ToString()
                                                If (LineCols.Contains("ItemCost")) Then Double.TryParse(LineRec("ItemCost").ToString(), oBookDetail.ItemCost)
                                                If (LineCols.Contains("QtyOrdered")) Then GPFunctions.TryParseInt(LineRec("QtyOrdered").ToString(), oBookDetail.QtyOrdered)
                                                If (LineCols.Contains("Weight")) Then Double.TryParse(LineRec("Weight").ToString(), oBookDetail.Weight)
                                                If (LineCols.Contains("FixOffInvAllow")) Then Double.TryParse(LineRec("FixOffInvAllow").ToString(), oBookDetail.FixOffInvAllow)
                                                If (LineCols.Contains("FixFrtAllow")) Then Double.TryParse(LineRec("FixFrtAllow").ToString(), oBookDetail.FixFrtAllow)
                                                If (LineCols.Contains("FreightCost")) Then Double.TryParse(LineRec("FreightCost").ToString(), oBookDetail.FreightCost)
                                                If (LineCols.Contains("ItemCost")) Then Double.TryParse(LineRec("ItemCost").ToString(), oBookDetail.ItemCost)
                                                If (LineCols.Contains("Cube")) Then GPFunctions.TryParseInt(LineRec("Cube").ToString(), oBookDetail.Cube)
                                                If (LineCols.Contains("Pack")) Then Short.TryParse(LineRec("Pack").ToString(), oBookDetail.Pack)
                                                If (LineCols.Contains("Size")) Then oBookDetail.Size = LineRec("Size").ToString()
                                                If (LineCols.Contains("Description")) Then oBookDetail.Description = LineRec("Description").ToString
                                                If (LineCols.Contains("Hazmat")) Then oBookDetail.Hazmat = LineRec("Hazmat").ToString()
                                                If (LineCols.Contains("Brand")) Then oBookDetail.Brand = LineRec("Brand").ToString()
                                                If (LineCols.Contains("CostCenter")) Then oBookDetail.CostCenter = LineRec("CostCenter").ToString()
                                                If (LineCols.Contains("LotNumber")) Then oBookDetail.LotNumber = LineRec("LotNumber").ToString()
                                                If (LineCols.Contains("LotExpirationDate")) Then oBookDetail.LotExpirationDate = LineRec("LotExpirationDate").ToString()
                                                If (LineCols.Contains("GTIN")) Then oBookDetail.GTIN = LineRec("GTIN").ToString()
                                                If (LineCols.Contains("CustItemNumber")) Then oBookDetail.CustItemNumber = LineRec("CustItemNumber").ToString()
                                                oBookDetail.CustomerNumber = "0" 'Modified by RHR for v-7.0.5.102 10/17/2016 we do not use the CUSTNMBR here  HeaderRec("CUSTNMBR").ToString
                                                oBookDetail.POOrderSequence = 0
                                                If (LineCols.Contains("PalletType")) Then oBookDetail.PalletType = LineRec("PalletType").ToString()
                                                If (LineCols.Contains("POItemHazmatTypeCode")) Then oBookDetail.POItemHazmatTypeCode = LineRec("POItemHazmatTypeCode").ToString()
                                                If (LineCols.Contains("POItem49CFRCode")) Then oBookDetail.POItem49CFRCode = LineRec("POItem49CFRCode").ToString()
                                                If (LineCols.Contains("POItemIATACode")) Then oBookDetail.POItemIATACode = LineRec("POItemIATACode").ToString()
                                                If (LineCols.Contains("POItemDOTCode")) Then oBookDetail.POItemDOTCode = LineRec("POItemDOTCode").ToString()
                                                If (LineCols.Contains("POItemMarineCode")) Then oBookDetail.POItemMarineCode = LineRec("POItemMarineCode").ToString()
                                                If (LineCols.Contains("POItemNMFCClass")) Then oBookDetail.POItemNMFCClass = LineRec("POItemNMFCClass").ToString()
                                                If (LineCols.Contains("POItemFAKClass")) Then oBookDetail.POItemFAKClass = LineRec("POItemFAKClass").ToString()
                                                If (LineCols.Contains("POItemLimitedQtyFlag")) Then Boolean.TryParse(LineRec("POItemLimitedQtyFlag").ToString(), oBookDetail.POItemLimitedQtyFlag)
                                                If (LineCols.Contains("POItemPallets")) Then Double.TryParse(LineRec("POItemPallets").ToString(), oBookDetail.POItemPallets)
                                                If (LineCols.Contains("POItemTies")) Then Double.TryParse(LineRec("POItemTies").ToString(), oBookDetail.POItemTies)
                                                If (LineCols.Contains("POItemHighs")) Then Double.TryParse(LineRec("POItemHighs").ToString(), oBookDetail.POItemHighs)
                                                If (LineCols.Contains("POItemQtyPalletPercentage")) Then Double.TryParse(LineRec("POItemQtyPalletPercentage").ToString(), oBookDetail.POItemQtyPalletPercentage)
                                                If (LineCols.Contains("POItemQtyLength")) Then Double.TryParse(LineRec("POItemQtyLength").ToString(), oBookDetail.POItemQtyLength)
                                                If (LineCols.Contains("POItemQtyWidth")) Then Double.TryParse(LineRec("POItemQtyWidth").ToString(), oBookDetail.POItemQtyWidth)
                                                If (LineCols.Contains("POItemQtyHeight")) Then Double.TryParse(LineRec("POItemQtyHeight").ToString(), oBookDetail.POItemQtyHeight)
                                                If (LineCols.Contains("POItemStackable")) Then Boolean.TryParse(LineRec("POItemStackable").ToString(), oBookDetail.POItemStackable)
                                                If (LineCols.Contains("POItemLevelOfDensity")) Then GPFunctions.TryParseInt(LineRec("POItemLevelOfDensity").ToString(), oBookDetail.POItemLevelOfDensity)
                                                oBookDetail.POItemCompLegalEntity = TMSSetting.LegalEntity.ToString
                                                oBookDetail.POItemCompAlphaCode = HeaderRec("LOCNCODE")
                                                'oBookDetail.POItemCompAlphaCode = "WAREHOUSE"
                                                If (LineCols.Contains("POItemNMFCSubClass")) Then oBookDetail.POItemNMFCSubClass = LineRec("POItemNMFCSubClass").ToString()
                                                If (LineCols.Contains("POItemUser1")) Then oBookDetail.POItemUser1 = LineRec("POItemUser1").ToString()
                                                If (LineCols.Contains("POItemUser2")) Then oBookDetail.POItemUser2 = LineRec("POItemUser2").ToString()
                                                If (LineCols.Contains("POItemUser3")) Then oBookDetail.POItemUser3 = LineRec("POItemUser3").ToString()
                                                If (LineCols.Contains("POItemUser4")) Then oBookDetail.POItemUser4 = LineRec("POItemUser4").ToString()
                                                If (LineCols.Contains("POItemWeightUnitOfMeasure")) Then oBookDetail.POItemWeightUnitOfMeasure = LineRec("POItemWeightUnitOfMeasure").ToString()
                                                If (LineCols.Contains("POItemCubeUnitOfMeasure")) Then oBookDetail.POItemCubeUnitOfMeasure = LineRec("POItemCubeUnitOfMeasure").ToString()
                                                If (LineCols.Contains("POItemDimensionUnitOfMeasure")) Then oBookDetail.POItemDimensionUnitOfMeasure = LineRec("POItemDimensionUnitOfMeasure").ToString()

                                                'If Me.Debug Then Log("Adding Item Detail  Item Number: " & LineRec("Itemnmbr").ToString)
                                                oBookDetails.Add(oBookDetail)

                                                oBookDetail = Nothing

                                            End While

                                            ' Report something if no line recvords
                                            If (Counter = 0) Then

                                                If Me.Verbose Then Log("No Line records to process for SOP #: " & SOPOrder.SOPOrders.ToString)

                                            End If

                                            LineRec.Close()

                                            HeaderRec.Close()

                                        End If

                                        If (Not HeaderRec.IsClosed) Then

                                            HeaderRec.Close()

                                        End If

                                        oBookHeader = Nothing

                                    End If


                                End If

                            Else

                                If Me.Verbose Then Log("SOP In List, but query did not return header data  for SOP #: " & SOPOrder.SOPOrders.ToString)

                            End If

                        Catch ex As Exception
                            Throw
                        Finally
                            'besure the reader is closed
                            Try
                                If Not HeaderRec Is Nothing AndAlso (Not HeaderRec.IsClosed) Then

                                    HeaderRec.Close()

                                End If


                            Catch ex As Exception

                            End Try

                            Try
#Disable Warning BC42104 ' Variable 'LineRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                                If Not LineRec Is Nothing AndAlso (Not LineRec.IsClosed) Then
#Enable Warning BC42104 ' Variable 'LineRec' is used before it has been assigned a value. A null reference exception could result at runtime.

                                    LineRec.Close()

                                End If


                            Catch ex As Exception

                            End Try
                        End Try


                    Else
                        If Me.Debug Then Log("Processing Book Object Data; Headers: " & oBookHeaders.Count().ToString() & " Details: " & oBookDetails.Count().ToString())
                        'Process the last order before submitting orderst to TMS

                        oBookIntegration.RunSilentTenderAsync = False
                        'save changes to database 
                        Dim oResults As TMS.Configuration.ProcessDataReturnValues = oBookIntegration.ProcessObjectData(oBookHeaders, oBookDetails, Me.ConnectionString)
                        Dim ReturnMessage As String = oBookIntegration.LastError
                        If Me.Debug Then Log("Results = " & oResults.ToString())
                        Select Case oResults
                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Data Connection Failure! could not import Order information")
                                Else
                                    sIntegrationErrors.Add("Data Connection Failure! could not import Order information:  " & ReturnMessage)
                                End If

                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Integration Failure! could not import Order information")
                                Else
                                    generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                                End If

                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Integration Had Errors! could not import some Order information")
                                Else
                                    generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                                End If

                                'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                'blnRet = True
                                'End If
                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Data Validation Failure! could not import some Order information")
                                Else
                                    generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                                End If

                                'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                'blnRet = True
                                'End If
                            Case Else
                                'success
                                Dim strNumbers As String = "N/A"
                                If Not SOOrdersProcessed Is Nothing AndAlso SOOrdersProcessed.Count() > 0 Then
                                    strNumbers = String.Join(", ", SOOrdersProcessed)
                                End If

                                sLogMsgs.Add("Success! the following Order Numbers were processed: " & strNumbers)
                                'Processed = oResults.
                                'TODO: add code to send confirmation back to NAV that the orders were processed
                                'mark process and success
                                'blnRet = True
                        End Select


                        SOPCounter = 0

                        oResults = Nothing
                        'sLastError = Nothing

                        oBookHeaders.Clear()
                        oBookDetails.Clear()

                    End If

                Next

                If Not sIntegrationErrors Is Nothing AndAlso sIntegrationErrors.Count() > 0 Then
                    LogError("Warning!  " & Source & " Had Errors: " & vbCrLf & String.Join(vbCrLf, sIntegrationErrors.ToArray()))
                End If
                'If Not sAdminErrors Is Nothing AndAlso sAdminErrors.Count() > 0 Then
                'LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sAdminErrors.ToArray()), Me.AdminEmail)
                'End If
                'If Not sOperationErrors Is Nothing AndAlso sOperationErrors.Count() > 0 Then
                'LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sOperationErrors.ToArray()), Me.GroupEmail)
                'End If
                'If Not sLogMsgs Is Nothing AndAlso sLogMsgs.Count() > 0 Then
                If Me.Verbose Then Log(Source & ": " & vbCrLf & String.Join(vbCrLf, sLogMsgs.ToArray()))
                'End If
                If Debug Then Log("Process Order Data Complete")

            Catch ex As Exception

                'MsgBox("Inner Try " & ex.Message)

                Log("There was an erorr processing SOP Orders.  The error is: " & ex.Message)
                Throw

                'Report error
            Finally
                Try
                    ' End Routine
                    HeaderCom.Dispose()
                    HeaderConn.Dispose()

                    LineCom.Dispose()
                    LineConn.Dispose()
                Catch ex As Exception

                End Try
            End Try
            If Not oBookHeaders Is Nothing AndAlso oBookHeaders.Count() > 0 Then
                If Me.Debug Then Log("Processing " & oBookHeaders.Count() & " SO Booking Header Records ")
                Dim oResultsLast As TMS.Configuration.ProcessDataReturnValues = oBookIntegration.ProcessObjectData(oBookHeaders, oBookDetails, Me.ConnectionString)
                Dim sLastErrorLast As String = oBookIntegration.LastError
                Select Case oResultsLast
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        If String.IsNullOrWhiteSpace(sLastErrorLast) Then
                            If Me.Verbose Then Log("Data Connection Failure! could not import Order information")
                        Else
                            LogError("Error Data Connection Failure! could not import Order information:  " & sLastErrorLast)

                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        If String.IsNullOrWhiteSpace(sLastErrorLast) Then
                            If Me.Verbose Then Log("Integration Failure! could not import Order information")
                        Else
                            LogError("Error Integration Failure! could not import Order information:  " & sLastErrorLast)
                        End If

                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        If GPTestingOn Then 'we return true so testing can continue other integration points if GPTesting Flag is on
                            Log("Integration Had Errors! Could not import some Order information:  " & sLastErrorLast)
                            'blnRet = True
                        ElseIf String.IsNullOrWhiteSpace(sLastErrorLast) Then
                            If Me.Verbose Then Log("Integration Had Errors! could not import some Order information")
                        Else
                            LogError(Source & " Warning Integration Had Errors! could not import some Order information:  " & sLastErrorLast)
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        If GPTestingOn Then 'we return true so testing can continue other integration points if GPTesting Flag is on
                            Log(Source & "Data Validation Failure! could not import Order information:  " & sLastErrorLast)
                            'blnRet = True
                        ElseIf String.IsNullOrWhiteSpace(sLastErrorLast) Then
                            If Me.Verbose Then Log("Data Validation Failure! could not import some Order information")
                        Else
                            LogError(Source & " Warning Data Validation Failure! could not import some Order information:  " & sLastErrorLast)
                        End If
                    Case Else
                        'success
                        'Dim strNumbers As String = String.Join(", ", Orders)
                        Dim strNumbers As String = "Test"
                        If Me.Verbose Then Log("Success! the following Order Numbers were processed: " & strNumbers)
                        'Processed = oResults.
                        'TODO: add code to send confirmation back to GP that the orders were processed
                        'mark process and success
                        'blnRet = True
                End Select
            Else
                If Me.Debug Then Log("No SO Booking Header records to process")

            End If
            SOPCounter = 0

            oBookHeaders.Clear()
            oBookDetails.Clear()

            'Add code here to process orders into TMS



        Catch ex As Exception

            Log(Source & ".ProcessGPSOPOrders Error!  Unexpected GP Process SOP Orders Error could not process integration requests; the actual error is:  " & ex.Message)
            'Throw
            ReturnValue = False

        Finally

            Try
#Disable Warning BC42104 ' Variable 'HeaderRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not HeaderRec.IsClosed Then HeaderRec.Close()
#Enable Warning BC42104 ' Variable 'HeaderRec' is used before it has been assigned a value. A null reference exception could result at runtime.

                If Not LineRec.IsClosed Then LineRec.Close()

            Catch ex As Exception

            End Try
            Try
                If Not HeaderCom Is Nothing Then HeaderCom.Dispose()
                If Not LineCom Is Nothing Then LineCom.Dispose()

            Catch ex As Exception

            End Try
            Try
                If Not HeaderConn Is Nothing AndAlso Not HeaderConn.State = ConnectionState.Closed Then HeaderConn.Close()
                HeaderConn.Dispose()

                If Not LineConn Is Nothing AndAlso Not LineConn.State = ConnectionState.Closed Then LineConn.Close()
                LineConn.Dispose()

            Catch ex As Exception

            End Try

        End Try

        Return ReturnValue

    End Function


    'Removed by RHR 10/31/2017 no longer used

    '''' <summary>
    '''' Process the GP Purchase Orders
    '''' </summary>
    '''' <param name="TMSSetting"></param>
    '''' <param name="c"></param>
    '''' <param name="OrderNumbers"></param>
    '''' <param name="SOPType"></param>
    '''' <param name="TMSCountToProcess"></param>
    '''' <returns></returns>
    '''' <remarks>
    '''' Modified by RHR for v-7.0.5.102 10/14/2016
    ''''   added logic to use more configuration settings for GP Functions
    '''' Modified by RHR for v-7.0.6.104 on 08/28/2027 added finally statement to dispose of db connections
    '''' </remarks>
    'Public Function ProcessGPPOOrdersOld(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByRef c As clsDefaultIntegrationConfiguration, ByVal OrderNumbers As List(Of GPDataIntegrationSTructure.POOrders), ByVal SOPType As Int16, ByVal TMSCountToProcess As Int16) As Boolean

    '    Dim ReturnValue As Boolean = True
    '    Dim EconnectStr As String = ""
    '    Dim GetAppSettings = New AppSettingsReader
    '    Dim LaneNumber As String = ""
    '    Dim GPFunctions As New GPFunctions(c) 'Modified by RHR for v-7.0.5.102 10/14/2016
    '    Dim POCounter As Int16 = 0
    '    Dim OrderChanged As Boolean = False
    '    Dim OrdChangeStr As String = ""
    '    Dim LocationCode As String = ""

    '    ' Set all the variables
    '    If Not String.IsNullOrWhiteSpace(TMSSetting.ERPUser) AndAlso Not String.IsNullOrWhiteSpace(TMSSetting.ERPPassword) Then
    '        EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";User ID=" & TMSSetting.ERPUser & ";Password=" & TMSSetting.ERPPassword
    '    Else
    '        EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
    '    End If
    '    'EconnectStr = "Data Source=" & GetAppSettings.GetValue("SQLServer") & "; Initial Catalog=" & GPCompany & "Integrated Security=SSPI;Trusted_Connection=True"
    '    'EconnectStr = "Data Source=" & TMSSetting.ERPCertificate.ToString & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
    '    'EconnectStr = "Data Source=NGLGP2013R2; Initial Catalog=TWO;Integrated Security=SSPI;Trusted_Connection=True"


    '    Dim Counter As Integer = 0
    '    Dim NumberofLines As Integer = 0
    '    Dim OrderNumber As String = ""
    '    Dim sIntegrationErrors As New List(Of String)
    '    Dim sLogMsgs As New List(Of String)
    '    Dim PorDAddressInfo As New GPDataIntegrationSTructure.FromAddress
    '    Dim ProAbb As String = ""
    '    Dim LaneValue As String = ""
    '    Dim LineNumber As Integer = 0
    '    If Me.Debug Then Log("Start Process GP PO Orders")
    '    Dim HeaderConn = New SqlConnection(EconnectStr)
    '    Dim HeaderCom As New SqlCommand
    '    Dim HeaderRec As SqlDataReader
    '    Dim LineConn = New SqlConnection(EconnectStr)
    '    Dim LineCom As New SqlCommand
    '    Dim LineRec As SqlDataReader
    '    Try

    '        HeaderConn.Open()

    '        HeaderCom.Connection = HeaderConn

    '        LineConn.Open()

    '        LineCom.Connection = LineConn


    '        'Integrate Sales Order
    '        'Create TMS Header & Line

    '        Dim oBookHeaders As New List(Of TMS.clsBookHeaderObject705)
    '        Dim oBookDetails As New List(Of TMS.clsBookDetailObject705)

    '        Dim oConfig As New UserConfiguration()
    '        With oConfig
    '            .AdminEmail = Me.AdminEmail
    '            .AutoRetry = Me.AutoRetry
    '            .Database = Me.Database
    '            .DBServer = Me.DBServer
    '            .Debug = Me.Debug
    '            .FromEmail = Me.FromEmail
    '            .GroupEmail = Me.GroupEmail
    '            .LogFile = Me.LogFile
    '            .SMTPServer = Me.SMTPServer
    '            .UserName = "System Download"
    '            .WSAuthCode = "NGLSystem"
    '            .WCFAuthCode = "NGLSystem"
    '            .WCFURL = ""
    '            .WCFTCPURL = ""
    '            .ConnectionString = Me.ConnectionString

    '        End With


    '        ' Here

    '        Dim oBookIntegration As New TMS.clsBook(oConfig)

    '        Try
    '            If OrderNumbers Is Nothing Then
    '                If Me.Debug Then Log("No Purchase Orders")
    '                Return False
    '            Else
    '                If Me.Debug Then Log("Process " & OrderNumbers.Count() & " Purchase Orders")
    '            End If
    '            For Each Order In OrderNumbers

    '                OrderNumber = Trim(Order.POOrders.ToString)
    '                'If Me.Debug Then Log("processing PO Number: " & OrderNumber)
    '                Dim oBookHeader As New TMS.clsBookHeaderObject705

    '                If (POCounter < 99) Then
    '                    'Modified by RHR for v-7.0.6.105 10/25/2017  query now stored in config file
    '                    'HeaderCom.CommandText = "select * from pop10100 where PONUMBER = '" & OrderNumber & "'"
    '                    HeaderCom.CommandText = String.Format(c.GPFunctionsPOHeaders, OrderNumber)

    '                    HeaderRec = HeaderCom.ExecuteReader

    '                    HeaderRec.Read()

    '                    If (HeaderRec.HasRows) Then
    '                        'If Me.Debug Then Log("Attempting to processing PO Number: " & OrderNumber)
    '                        Dim sErrors As String = ""
    '                        'If Me.Debug Then Log("testing For Alpha Code using Loction Code: " & HeaderRec("LOCNCODE").ToString)

    '                        'Modified by RHR for v-7.0.6.105 10/25/2017  query now stored in config file
    '                        'LineCom.CommandText = "select top 1 LOCNCODE from POP10110 where PONUMBER = '" & Trim(HeaderRec("PONUMBER").ToString) & "'"
    '                        LineCom.CommandText = String.Format(c.GPFunctionsPOLocationCode, Trim(HeaderRec("PONUMBER").ToString))
    '                        LineRec = LineCom.ExecuteReader
    '                        LineRec.Read()
    '                        If (LineRec.HasRows) Then

    '                            LocationCode = Trim(LineRec("LOCNCODE").ToString)

    '                            ProAbb = oBookIntegration.GetCompAbrevByNumberOrAlpha(0, LocationCode, sErrors)
    '                            'ProAbb = oBookIntegration.GetCompAbrevByNumberOrAlpha(0, "WAREHOUSE", "", sErrors)

    '                        Else

    '                            LogError("Error processing GP Purchase Orders - No Location assigned to PO ", "There was No lines for PO:  " & Trim(HeaderRec("PONUMBER").ToString), Me.AdminEmail)

    '                        End If

    '                        LineRec.Close()

    '                        'If (ProAbb.Count > 0) Then

    '                        If (String.IsNullOrWhiteSpace(sErrors) AndAlso String.IsNullOrWhiteSpace(ProAbb)) Then
    '                            ' skip order if failed
    '                            If Me.Debug Then
    '                                Log("Could not process order: " & OrderNumber & " Due to a missing Company Pro Abb for Comp Alpha Code: " & LocationCode)
    '                            End If
    '                            HeaderRec.Close()
    '                        Else
    '                            'If Me.Debug Then Log("Processing for Lane: " & LaneValue)
    '                            LaneValue = GPFunctions.CalculateLane(EconnectStr, 2, OrderNumber, 16384, OrderNumber, ProAbb, 0, sErrors)

    '                            If (LaneValue = "Error") Then
    '                                If Me.Verbose Or Me.Debug Then
    '                                    Log("Error when processing Lane for purchase order: " & OrderNumber)

    '                                End If
    '                                HeaderRec.Close()

    '                                ' REturn more information 
    '                                'LogError("Error when calculating Lane for order: ", sErrors, Me.AdminEmail)

    '                            Else
    '                                'If Me.Debug Then Log("Reading PO data using for Lane: " & LaneValue)
    '                                OrderNumber = Trim(HeaderRec("PONUMBER").ToString)
    '                                ' Process Order to TMS

    '                                ' Changes made by Scott McFarland for VPGC
    '                                ' 2017-07-24

    '                                ' Insert TMSHeader
    '                                oBookHeader.PONumber = HeaderRec("PONUMBER").ToString
    '                                oBookHeader.POdate = HeaderRec("DocDate").ToString
    '                                oBookHeader.POShipdate = HeaderRec("PRMSHPDTE").ToString
    '                                oBookHeader.POVendor = GPFunctions.CalculateLane(EconnectStr, 2, OrderNumber, 16384, OrderNumber, ProAbb, 0, sErrors)
    '                                'oBookHeader.POBuyer = HeaderRec("").ToString
    '                                oBookHeader.POBuyer = HeaderRec("BUYERID").ToString
    '                                'oBookHeader.POFrt = HeaderRec("").ToString
    '                                ' Work with Rob to determine
    '                                oBookHeader.POFrt = GPFunctions.GetShippingMethod(EconnectStr, 0, OrderNumber, 16384, OrderNumber, 0)
    '                                Double.TryParse(HeaderRec("FRTAMNT").ToString, oBookHeader.POTotalFrt)
    '                                Double.TryParse(HeaderRec("SUBTOTAL").ToString, oBookHeader.POTotalCost)
    '                                oBookHeader.POWgt = GPFunctions.GetPOTotalWeight(EconnectStr, OrderNumber)
    '                                'oBookHeader.POCube = HeaderRec("").ToString
    '                                oBookHeader.POCube = 0
    '                                oBookHeader.POQty = CInt(GPFunctions.GetPOQuantity(EconnectStr, OrderNumber))
    '                                'oBookHeader.POPallets = HeaderRec("").ToString
    '                                'Need to define a Pallet formula
    '                                oBookHeader.POPallets = CInt(GPFunctions.CalculatePallets(EconnectStr, OrderNumber, 2, Me.AdminEmail, 1))
    '                                'oBookHeader.POLines = HeaderRec("").ToString
    '                                oBookHeader.POLines = 0
    '                                oBookHeader.POConfirm = False
    '                                oBookHeader.PODefaultCustomer = "0"
    '                                'oBookHeader.PODefaultCarrier = HeaderRec("").ToString
    '                                ' Talk to Rob on this field below
    '                                oBookHeader.PODefaultCarrier = 0
    '                                oBookHeader.POReqDate = HeaderRec("PRMSHPDTE").ToString
    '                                ' Write funtion to get shipping instructions
    '                                'Updated by SEM 2017-07-30 added get notes
    '                                'oBookHeader.POShipInstructions = ""
    '                                oBookHeader.POShipInstructions = GPFunctions.GetComments(EconnectStr, HeaderRec("PONUMBER").ToString, 2, Me.AdminEmail)
    '                                oBookHeader.POCooler = False
    '                                oBookHeader.POFrozen = False
    '                                oBookHeader.PODry = False
    '                                oBookHeader.POTemp = GPFunctions.GetTemp(EconnectStr, 0, OrderNumber, 1)
    '                                oBookHeader.POCarType = ""
    '                                oBookHeader.POShipVia = ""
    '                                oBookHeader.POShipViaType = ""
    '                                oBookHeader.POConsigneeNumber = ""
    '                                oBookHeader.POCustomerPO = HeaderRec("VENDORID").ToString
    '                                oBookHeader.POOtherCosts = 0
    '                                '  Need to determine how this will be set. for now, 0 =New
    '                                ' Ship Confirmation, send a 3, 5 is  an order
    '                                oBookHeader.POStatusFlag = 5
    '                                oBookHeader.POOrderSequence = 0
    '                                oBookHeader.POChepGLID = ""
    '                                oBookHeader.POCarrierEquipmentCodes = ""
    '                                oBookHeader.POCarrierTypeCode = ""
    '                                oBookHeader.POPalletPositions = ""
    '                                oBookHeader.POSchedulePUDate = HeaderRec("PRMSHPDTE").ToString
    '                                oBookHeader.POSchedulePUTime = ""
    '                                oBookHeader.POScheduleDelDate = HeaderRec("PRMSHPDTE").ToString
    '                                oBookHeader.POSCheduleDelTime = ""
    '                                oBookHeader.POActPUDate = ""
    '                                oBookHeader.POActPUTime = ""
    '                                oBookHeader.POActDelDate = ""
    '                                oBookHeader.POActDelTime = ""
    '                                oBookHeader.POOrigCompNumber = ""

    '                                ' Disabled this originating address as the PO works differently
    '                                'PorDAddressInfo = GPFunctions.GetPorDAddress(EconnectStr, locationcode)
    '                                'PorDAddressInfo = GPFunctions.GetPorDAddress(EconnectStr, "WAREHOUSE")

    '                                'If (PorDAddressInfo.AddressID = "No Address") Then

    '                                'LogError("No Address found", "There was no address found for SOP Order: " & OrderNumber, Me.AdminEmail)
    '                                'Report to address found

    '                                'Else

    '                                '  This needs to be examined by Rob & SEM
    '                                'oBookHeader.POOrigName = PorDAddressInfo.AddressName
    '                                'oBookHeader.POOrigCompAlphaCode = PorDAddressInfo.AddressID
    '                                'oBookHeader.POOrigAddress1 = PorDAddressInfo.Address1
    '                                'oBookHeader.POOrigAddress2 = PorDAddressInfo.Address2
    '                                'oBookHeader.POOrigAddress3 = PorDAddressInfo.Address3
    '                                'oBookHeader.POOrigCity = PorDAddressInfo.City
    '                                'oBookHeader.POOrigState = PorDAddressInfo.State
    '                                'oBookHeader.POOrigCountry = If(String.IsNullOrWhiteSpace(c.GPFunctionsForceDefaultCountry), PorDAddressInfo.Country, c.GPFunctionsForceDefaultCountry)
    '                                'oBookHeader.POOrigZip = PorDAddressInfo.ZipCode
    '                                'oBookHeader.POOrigContactPhone = PorDAddressInfo.Phone
    '                                'oBookHeader.POOrigContactPhoneExt = ""
    '                                'oBookHeader.POOrigContactFax = PorDAddressInfo.Fax

    '                                'End If

    '                                'Originating Address for PO was updated by SEM on 2017-08-02
    '                                '  Getting originating address using the Purchase Address ID

    '                                oBookHeader.POOrigName = Trim(HeaderRec("PURCHCMPNYNAM").ToString)
    '                                oBookHeader.POOrigCompAlphaCode = Trim(HeaderRec("VADCDPAD").ToString)
    '                                oBookHeader.POOrigAddress1 = Trim(HeaderRec("PURCHADDRESS1").ToString)
    '                                oBookHeader.POOrigAddress2 = Trim(HeaderRec("PURCHADDRESS2").ToString)
    '                                oBookHeader.POOrigAddress3 = Trim(HeaderRec("PURCHADDRESS3").ToString)
    '                                oBookHeader.POOrigCity = Trim(HeaderRec("PURCHCITY").ToString)
    '                                oBookHeader.POOrigState = Trim(HeaderRec("PURCHSTATE").ToString)
    '                                oBookHeader.POOrigCountry = If(String.IsNullOrWhiteSpace(HeaderRec("PURCHCOUNTRY").ToString), "US", HeaderRec("PURCHCOUNTRY").ToString)
    '                                oBookHeader.POOrigZip = Trim(HeaderRec("PURCHZIPCODE").ToString)
    '                                oBookHeader.POOrigContactPhone = Trim(HeaderRec("PURCHPHONE1").ToString)
    '                                oBookHeader.POOrigContactPhoneExt = ""
    '                                oBookHeader.POOrigContactFax = Trim(HeaderRec("PURCHFAX").ToString)


    '                                oBookHeader.PODestCompNumber = ""
    '                                oBookHeader.PODestCompAlphaCode = HeaderRec("PRSTADCD").ToString
    '                                oBookHeader.PODestName = HeaderRec("CMPNYNAM").ToString
    '                                oBookHeader.PODestAddress1 = HeaderRec("ADDRESS1").ToString
    '                                oBookHeader.PODestAddress2 = HeaderRec("ADDRESS2").ToString
    '                                oBookHeader.PODestAddress3 = HeaderRec("ADDRESS1").ToString
    '                                oBookHeader.PODestCity = HeaderRec("CITY").ToString
    '                                oBookHeader.PODestState = HeaderRec("STATE").ToString
    '                                oBookHeader.PODestCountry = If(String.IsNullOrWhiteSpace(c.GPFunctionsForceDefaultCountry), HeaderRec("COUNTRY").ToString(), c.GPFunctionsForceDefaultCountry)
    '                                oBookHeader.PODestZip = HeaderRec("ZIPCODE").ToString
    '                                oBookHeader.PODestContactPhone = HeaderRec("PHONE1").ToString
    '                                oBookHeader.PODestContactPhoneExt = ""
    '                                oBookHeader.PODestContactFax = HeaderRec("FAX").ToString
    '                                oBookHeader.POInbound = False
    '                                oBookHeader.POPalletExchange = False
    '                                oBookHeader.POPalletType = ""
    '                                oBookHeader.POComments = ""
    '                                oBookHeader.POCommentsConfidential = ""
    '                                oBookHeader.PODefaultRouteSequence = 0
    '                                'Modified by RHR v-7.0.5.102 09/26/2016 removed code to populate the PORouteGuideNumber with the Lane Number
    '                                'as this is not required.
    '                                'oBookHeader.PORouteGuideNumber = GPFunctions.CalculateLane(EconnectStr, 2, OrderNumber, 16384, OrderNumber, ProAbb, SOPType, sErrors)
    '                                oBookHeader.PORouteGuideNumber = ""
    '                                oBookHeader.POCompLegalEntity = TMSSetting.LegalEntity.ToString
    '                                'oBookHeader.POCompAlphaCode = "WAREHOUSE"
    '                                oBookHeader.POCompAlphaCode = LocationCode
    '                                oBookHeader.POModeTypeControl = GPFunctions.GetTransporationMode(OrderNumber, "POP")
    '                                oBookHeader.POUser1 = ""
    '                                oBookHeader.POUser2 = ""
    '                                oBookHeader.POUser3 = ""
    '                                oBookHeader.POUser4 = ""
    '                                oBookHeader.POAPGLNumber = ""

    '                                NumberofLines = 0
    '                                'Modified by RHR for v-7.0.6.105 10/25/2017
    '                                'the code to count the number of items has been noved to below the get item detail c 
    '                                'counting the records with a sql command
    '                                LineCom.CommandText = String.Format(c.GPFunctionsPOItemDetails, OrderNumber)
    '                                'LineCom.CommandText = "select count(*) from pop10110 where POLNESTA in (1, 2, 3) and PONUMBER = '" & OrderNumber & "'"
    '                                'LineRec = LineCom.ExecuteReader
    '                                'LineRec.Read()

    '                                If (LineRec.HasRows) Then

    '                                    NumberofLines = LineRec(0).ToString

    '                                Else

    '                                    NumberofLines = 0

    '                                End If

    '                                LineRec.Close()

    '                                Try

    '                                    OrderChanged = oBookIntegration.HasOrderChanged(oBookHeader, NumberofLines, OrdChangeStr, False, False)

    '                                Catch ex As Exception

    '                                    'MsgBox("Error: " & ex.Message)

    '                                    LogError("Error in Processing PO Orders - at the Order Change.  The Error is:  ", ex.Message, Me.AdminEmail)

    '                                End Try

    '                                If (Not OrderChanged) Then

    '                                    HeaderRec.Close()

    '                                Else


    '                                    oBookHeaders.Add(oBookHeader)
    '                                    'Modified by RHR for v-7.0.5.102 10/17/2016
    '                                    LineCom.CommandText = String.Format(c.GPFunctionsPOItemDetails, OrderNumber)
    '                                    'LineCom.CommandText = "select * from pop10110 where POLNESTA in (1, 2, 3) and PONUMBER = '" & OrderNumber & "'"

    '                                    LineRec = LineCom.ExecuteReader
    '                                    Dim LineCols = LineRec.GetSchemaTable().Rows.Cast(Of DataRow)().[Select](Function(row) TryCast(row("ColumnName"), String)).ToList()

    '                                    Counter = 0

    '                                    While LineRec.Read

    '                                        Dim oBookDetail As New TMS.clsBookDetailObject705

    '                                        Counter = Counter + 1

    '                                        oBookDetail.ItemPONumber = OrderNumber
    '                                        If (LineCols.Contains("ItemNumber")) Then oBookDetail.ItemNumber = LineRec("ItemNumber").ToString()
    '                                        If (LineCols.Contains("ItemCost")) Then Double.TryParse(LineRec("ItemCost").ToString(), oBookDetail.ItemCost)
    '                                        If (LineCols.Contains("QtyOrdered")) Then GPFunctions.TryParseInt(LineRec("QtyOrdered").ToString(), oBookDetail.QtyOrdered)
    '                                        If (LineCols.Contains("Weight")) Then Double.TryParse(LineRec("Weight").ToString(), oBookDetail.Weight)
    '                                        If (LineCols.Contains("FixOffInvAllow")) Then Double.TryParse(LineRec("FixOffInvAllow").ToString(), oBookDetail.FixOffInvAllow)
    '                                        If (LineCols.Contains("FixFrtAllow")) Then Double.TryParse(LineRec("FixFrtAllow").ToString(), oBookDetail.FixFrtAllow)
    '                                        If (LineCols.Contains("FreightCost")) Then Double.TryParse(LineRec("FreightCost").ToString(), oBookDetail.FreightCost)
    '                                        If (LineCols.Contains("ItemCost")) Then Double.TryParse(LineRec("ItemCost").ToString(), oBookDetail.ItemCost)
    '                                        If (LineCols.Contains("Cube")) Then GPFunctions.TryParseInt(LineRec("Cube").ToString(), oBookDetail.Cube)
    '                                        If (LineCols.Contains("Pack")) Then Short.TryParse(LineRec("Pack").ToString(), oBookDetail.Pack)
    '                                        If (LineCols.Contains("Size")) Then oBookDetail.Size = LineRec("Size").ToString()
    '                                        If (LineCols.Contains("Description")) Then oBookDetail.Description = LineRec("Description").ToString
    '                                        If (LineCols.Contains("Hazmat")) Then oBookDetail.Hazmat = LineRec("Hazmat").ToString()
    '                                        If (LineCols.Contains("Brand")) Then oBookDetail.Brand = LineRec("Brand").ToString()
    '                                        If (LineCols.Contains("CostCenter")) Then oBookDetail.CostCenter = LineRec("CostCenter").ToString()
    '                                        If (LineCols.Contains("LotNumber")) Then oBookDetail.LotNumber = LineRec("LotNumber").ToString()
    '                                        If (LineCols.Contains("LotExpirationDate")) Then oBookDetail.LotExpirationDate = LineRec("LotExpirationDate").ToString()
    '                                        If (LineCols.Contains("GTIN")) Then oBookDetail.GTIN = LineRec("GTIN").ToString()
    '                                        If (LineCols.Contains("CustItemNumber")) Then oBookDetail.CustItemNumber = LineRec("CustItemNumber").ToString()
    '                                        oBookDetail.CustomerNumber = "0" 'Modified by RHR for v-7.0.5.102 10/17/2016 we do not use the CUSTNMBR here  HeaderRec("CUSTNMBR").ToString
    '                                        oBookDetail.POOrderSequence = 0
    '                                        If (LineCols.Contains("PalletType")) Then oBookDetail.PalletType = LineRec("PalletType").ToString()
    '                                        If (LineCols.Contains("POItemHazmatTypeCode")) Then oBookDetail.POItemHazmatTypeCode = LineRec("POItemHazmatTypeCode").ToString()
    '                                        If (LineCols.Contains("POItem49CFRCode")) Then oBookDetail.POItem49CFRCode = LineRec("POItem49CFRCode").ToString()
    '                                        If (LineCols.Contains("POItemIATACode")) Then oBookDetail.POItemIATACode = LineRec("POItemIATACode").ToString()
    '                                        If (LineCols.Contains("POItemDOTCode")) Then oBookDetail.POItemDOTCode = LineRec("POItemDOTCode").ToString()
    '                                        If (LineCols.Contains("POItemMarineCode")) Then oBookDetail.POItemMarineCode = LineRec("POItemMarineCode").ToString()
    '                                        If (LineCols.Contains("POItemNMFCClass")) Then oBookDetail.POItemNMFCClass = LineRec("POItemNMFCClass").ToString()
    '                                        If (LineCols.Contains("POItemFAKClass")) Then oBookDetail.POItemFAKClass = LineRec("POItemFAKClass").ToString()
    '                                        If (LineCols.Contains("POItemLimitedQtyFlag")) Then Boolean.TryParse(LineRec("POItemLimitedQtyFlag").ToString(), oBookDetail.POItemLimitedQtyFlag)
    '                                        If (LineCols.Contains("POItemPallets")) Then Double.TryParse(LineRec("POItemPallets").ToString(), oBookDetail.POItemPallets)
    '                                        If (LineCols.Contains("POItemTies")) Then Double.TryParse(LineRec("POItemTies").ToString(), oBookDetail.POItemTies)
    '                                        If (LineCols.Contains("POItemHighs")) Then Double.TryParse(LineRec("POItemHighs").ToString(), oBookDetail.POItemHighs)
    '                                        If (LineCols.Contains("POItemQtyPalletPercentage")) Then Double.TryParse(LineRec("POItemQtyPalletPercentage").ToString(), oBookDetail.POItemQtyPalletPercentage)
    '                                        If (LineCols.Contains("POItemQtyLength")) Then Double.TryParse(LineRec("POItemQtyLength").ToString(), oBookDetail.POItemQtyLength)
    '                                        If (LineCols.Contains("POItemQtyWidth")) Then Double.TryParse(LineRec("POItemQtyWidth").ToString(), oBookDetail.POItemQtyWidth)
    '                                        If (LineCols.Contains("POItemQtyHeight")) Then Double.TryParse(LineRec("POItemQtyHeight").ToString(), oBookDetail.POItemQtyHeight)
    '                                        If (LineCols.Contains("POItemStackable")) Then Boolean.TryParse(LineRec("POItemStackable").ToString(), oBookDetail.POItemStackable)
    '                                        If (LineCols.Contains("POItemLevelOfDensity")) Then GPFunctions.TryParseInt(LineRec("POItemLevelOfDensity").ToString(), oBookDetail.POItemLevelOfDensity)
    '                                        oBookDetail.POItemCompLegalEntity = TMSSetting.LegalEntity.ToString
    '                                        oBookDetail.POItemCompAlphaCode = LocationCode
    '                                        'oBookDetail.POItemCompAlphaCode = "WAREHOUSE"
    '                                        If (LineCols.Contains("POItemNMFCSubClass")) Then oBookDetail.POItemNMFCSubClass = LineRec("POItemNMFCSubClass").ToString()
    '                                        If (LineCols.Contains("POItemUser1")) Then oBookDetail.POItemUser1 = LineRec("POItemUser1").ToString()
    '                                        If (LineCols.Contains("POItemUser2")) Then oBookDetail.POItemUser2 = LineRec("POItemUser2").ToString()
    '                                        If (LineCols.Contains("POItemUser3")) Then oBookDetail.POItemUser3 = LineRec("POItemUser3").ToString()
    '                                        If (LineCols.Contains("POItemUser4")) Then oBookDetail.POItemUser4 = LineRec("POItemUser4").ToString()
    '                                        If (LineCols.Contains("POItemWeightUnitOfMeasure")) Then oBookDetail.POItemWeightUnitOfMeasure = LineRec("POItemWeightUnitOfMeasure").ToString()
    '                                        If (LineCols.Contains("POItemCubeUnitOfMeasure")) Then oBookDetail.POItemCubeUnitOfMeasure = LineRec("POItemCubeUnitOfMeasure").ToString()
    '                                        If (LineCols.Contains("POItemDimensionUnitOfMeasure")) Then oBookDetail.POItemDimensionUnitOfMeasure = LineRec("POItemDimensionUnitOfMeasure").ToString()


    '                                        oBookDetails.Add(oBookDetail)

    '                                        oBookDetail = Nothing

    '                                    End While

    '                                    ' Report something if no line recvords
    '                                    If (Counter = 0) Then

    '                                        If Me.Verbose Then Log("No Line records to process for PO #: " & OrderNumber)

    '                                    End If

    '                                    LineRec.Close()

    '                                    HeaderRec.Close()

    '                                End If

    '                                If (Not HeaderRec.IsClosed) Then

    '                                    HeaderRec.Close()

    '                                End If

    '                                oBookHeader = Nothing

    '                            End If


    '                        End If

    '                    Else

    '                        If Me.Verbose Then Log("SOP In List, but query did not return header data  for PO #: " & OrderNumber)

    '                    End If

    '                Else

    '                    'Process the last order before submitting orderst to TMS

    '                    oBookIntegration.RunSilentTenderAsync = False
    '                    'save changes to database 
    '                    Dim oResults As TMS.Configuration.ProcessDataReturnValues = oBookIntegration.ProcessObjectData(oBookHeaders, oBookDetails, Me.ConnectionString)
    '                    Dim ReturnMessage As String = oBookIntegration.LastError
    '                    Select Case oResults
    '                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
    '                            If String.IsNullOrWhiteSpace(ReturnMessage) Then
    '                                If Me.Verbose Then Log("Data Connection Failure! could not import Order information")
    '                            Else
    '                                sIntegrationErrors.Add("Data Connection Failure! could not import Order information:  " & ReturnMessage)
    '                            End If
    '                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
    '                            If String.IsNullOrWhiteSpace(ReturnMessage) Then
    '                                If Me.Verbose Then Log("Integration Failure! could not import Order information")
    '                            Else
    '                                generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
    '                            End If

    '                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
    '                            If String.IsNullOrWhiteSpace(ReturnMessage) Then
    '                                If Me.Verbose Then Log("Integration Had Errors! could not import some Order information")
    '                            Else
    '                                generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
    '                            End If

    '                            'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
    '                            'blnRet = True
    '                            'End If
    '                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
    '                            If String.IsNullOrWhiteSpace(ReturnMessage) Then
    '                                If Me.Verbose Then Log("Data Validation Failure! could not import some Order information")
    '                            Else
    '                                generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
    '                            End If
    '                            'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
    '                            'blnRet = True
    '                            'End If
    '                        Case Else
    '                            'success
    '                            'Removed by RHR v-7.0.5.102 8/15/2016 because we do not have a list of Processed PO Orders to list
    '                            'TODO: fix list of purchase orders
    '                            Dim strNumbers As String = "N/A"
    '                            'Dim strNumbers As String = String.Join(", ", ProcessGPPOOrders)
    '                            sLogMsgs.Add("Success! the following Order Numbers were processed: " & strNumbers)
    '                            'Processed = oResults.
    '                            'TODO: add code to send confirmation back to NAV that the orders were processed
    '                            'mark process and success
    '                            'blnRet = True
    '                    End Select


    '                    POCounter = 0

    '                    oResults = Nothing
    '                    'sLastError = Nothing

    '                    oBookHeaders.Clear()
    '                    oBookDetails.Clear()

    '                End If

    '            Next

    '            If Not sIntegrationErrors Is Nothing AndAlso sIntegrationErrors.Count() > 0 Then
    '                LogError("Warning!  " & Source & " Had Errors: " & vbCrLf & String.Join(vbCrLf, sIntegrationErrors.ToArray()))
    '            End If
    '            'If Not sAdminErrors Is Nothing AndAlso sAdminErrors.Count() > 0 Then
    '            'LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sAdminErrors.ToArray()), Me.AdminEmail)
    '            'End If
    '            'If Not sOperationErrors Is Nothing AndAlso sOperationErrors.Count() > 0 Then
    '            'LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sOperationErrors.ToArray()), Me.GroupEmail)
    '            'End If
    '            'If Not sLogMsgs Is Nothing AndAlso sLogMsgs.Count() > 0 Then
    '            If Me.Verbose Then Log(Source & ": " & vbCrLf & String.Join(vbCrLf, sLogMsgs.ToArray()))
    '            'End If
    '            If Debug Then Log("Process Order Data Complete")

    '        Catch ex As Exception

    '            'MsgBox("Inner Try " & ex.Message)

    '            LogError("Error processing GP Purchase Orders", "There was an erorr processing PO Orders.  The error is: " & ex.Message, Me.AdminEmail)
    '            Throw

    '            'Report error

    '        End Try

    '        Dim oResultsLast As TMS.Configuration.ProcessDataReturnValues = oBookIntegration.ProcessObjectData(oBookHeaders, oBookDetails, Me.ConnectionString)
    '        Dim sLastErrorLast As String = oBookIntegration.LastError
    '        Select Case oResultsLast
    '            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
    '                If String.IsNullOrWhiteSpace(sLastErrorLast) Then
    '                    If Me.Verbose Then Log("Data Connection Failure! could not import Order information")
    '                Else
    '                    LogError("Error Data Connection Failure! could not import Order information:  " & sLastErrorLast)

    '                End If
    '            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
    '                If String.IsNullOrWhiteSpace(sLastErrorLast) Then
    '                    If Me.Verbose Then Log("Integration Failure! could not import Order information")
    '                Else
    '                    LogError("Error Integration Failure! could not import Order information:  " & sLastErrorLast)
    '                End If
    '            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
    '                If GPTestingOn Then 'we return true so testing can continue other integration points if GPTesting Flag is on
    '                    Log("Integration Had Errors! Could not import some Order information:  " & sLastErrorLast)
    '                    'blnRet = True
    '                ElseIf String.IsNullOrWhiteSpace(sLastErrorLast) Then
    '                    If Me.Verbose Then Log("Integration Had Errors! could not import some Order information")
    '                Else
    '                    LogError(Source & " Warning Integration Had Errors! could not import some Order information:  " & sLastErrorLast)
    '                End If

    '            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
    '                If GPTestingOn Then 'we return true so testing can continue other integration points if GPTesting Flag is on
    '                    LogError(Source & " Warning!  Order Integration Had Errors", Source & " Error Data Validation Failure! could not import Order information:  " & sLastErrorLast, AdminEmail)
    '                    'blnRet = True
    '                ElseIf String.IsNullOrWhiteSpace(sLastErrorLast) Then
    '                    If Me.Verbose Then Log("Data Validation Failure! could not import some Order information")
    '                Else
    '                    LogError(Source & " Warning Integration Had Errors! could not import some Order information:  " & sLastErrorLast)
    '                End If
    '            Case Else
    '                'success
    '                'Dim strNumbers As String = String.Join(", ", Orders)
    '                Dim strNumbers As String = "Test"
    '                If Me.Verbose Then Log("Success! the following Order Numbers were processed: " & strNumbers)
    '                'Processed = oResults.
    '                'TODO: add code to send confirmation back to GP that the orders were processed
    '                'mark process and success
    '                'blnRet = True
    '        End Select

    '        POCounter = 0

    '        oBookHeaders.Clear()
    '        oBookDetails.Clear()

    '        'Add code here to process orders into TMS

    '        ' End Routine
    '        HeaderCom.Dispose()
    '        HeaderConn.Dispose()

    '        LineCom.Dispose()
    '        LineConn.Dispose()

    '    Catch ex As Exception

    '        LogError(Source & " Error!  Unexpected GP Process Purchase Orders Error", Source & " Could not process any integration requests; the actual error is:  " & ex.Message, AdminEmail)
    '        'Throw
    '        ReturnValue = False
    '    Finally
    '        'Modified by RHR for v-7.0.6.104 on 08/28/2027 added finally statement to dispose of db connections
    '        GPFunctions = Nothing
    '        Try
    '            If Not HeaderRec Is Nothing AndAlso (Not HeaderRec.IsClosed) Then HeaderRec.Close()
    '        Catch ex As Exception

    '        End Try
    '        Try
    '            If Not LineRec Is Nothing AndAlso (Not LineRec.IsClosed) Then LineRec.Close()
    '        Catch ex As Exception

    '        End Try
    '        Try
    '            If Not HeaderCom Is Nothing Then HeaderCom.Dispose()
    '        Catch ex As Exception

    '        End Try
    '        Try
    '            If Not LineCom Is Nothing Then LineCom.Dispose()
    '        Catch ex As Exception

    '        End Try
    '        Try
    '            If Not HeaderConn Is Nothing AndAlso Not HeaderConn.State = ConnectionState.Closed Then HeaderConn.Close()
    '            HeaderConn.Dispose()
    '        Catch ex As Exception

    '        End Try
    '        Try
    '            If Not LineConn Is Nothing AndAlso Not LineConn.State = ConnectionState.Closed Then LineConn.Close()
    '            LineConn.Dispose()
    '        Catch ex As Exception

    '        End Try
    '    End Try

    '    Return ReturnValue

    'End Function


    Public Function ProcessGPPOOrders(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByRef c As clsDefaultIntegrationConfiguration, ByVal OrderNumbers As List(Of GPDataIntegrationSTructure.POOrders), ByVal SOPType As Int16, ByVal TMSCountToProcess As Int16) As Boolean

        Dim ReturnValue As Boolean = True
        Dim EconnectStr As String = ""
        Dim GetAppSettings = New AppSettingsReader
        Dim LaneNumber As String = ""
        Dim GPFunctions As New GPFunctions(c) 'Modified by RHR for v-7.0.5.102 10/14/2016
        Dim POCounter As Int16 = 0
        Dim OrderChanged As Boolean = False
        Dim OrdChangeStr As String = ""
        Dim LocationCode As String = ""
        Dim sErrors As String = ""

        ' Set all the variables
        If Not String.IsNullOrWhiteSpace(TMSSetting.ERPUser) AndAlso Not String.IsNullOrWhiteSpace(TMSSetting.ERPPassword) Then
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";User ID=" & TMSSetting.ERPUser & ";Password=" & TMSSetting.ERPPassword
        Else
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        End If
        'EconnectStr = "Data Source=" & GetAppSettings.GetValue("SQLServer") & "; Initial Catalog=" & GPCompany & "Integrated Security=SSPI;Trusted_Connection=True"
        'EconnectStr = "Data Source=" & TMSSetting.ERPCertificate.ToString & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        'EconnectStr = "Data Source=NGLGP2013R2; Initial Catalog=TWO;Integrated Security=SSPI;Trusted_Connection=True"


        Dim Counter As Integer = 0
        Dim NumberofLines As Integer = 0
        Dim OrderNumber As String = ""
        Dim sIntegrationErrors As New List(Of String)
        Dim sLogMsgs As New List(Of String)
        Dim PorDAddressInfo As New GPDataIntegrationSTructure.FromAddress
        Dim ProAbb As String = ""
        Dim LaneValue As String = ""
        Dim LineNumber As Integer = 0
        If Me.Debug Then Log("Start Process GP PO Orders")
        Dim HeaderConn = New SqlConnection(EconnectStr)
        Dim HeaderCom As New SqlCommand
        Dim HeaderRec As SqlDataReader
        Dim LineConn = New SqlConnection(EconnectStr)
        Dim LineCom As New SqlCommand
        Dim LineRec As SqlDataReader
        Try

            HeaderConn.Open()

            HeaderCom.Connection = HeaderConn

            LineConn.Open()

            LineCom.Connection = LineConn


            'Integrate Sales Order
            'Create TMS Header & Line

            Dim oBookHeaders As New List(Of tmsintegrationservices.clsBookHeaderObject705)
            Dim oBookDetails As New List(Of tmsintegrationservices.clsBookDetailObject705)
            'Modified by RHR for v-7.0.6.105 on 10/31/2017  
            'new list for item details to hold the items until HasChanged is determined
            'Allows the system to undo item details if the order informaiton has not changed
            Dim lItemDetailsToProcess As New List(Of tmsintegrationservices.clsBookDetailObject705)


            Dim oBookIntegration As New tmsintegrationservices.DTMSERPIntegration()
            oBookIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oBookIntegration.UseDefaultCredentials = True
            Else
                oBookIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If

            Dim POOrdersProcessed As New List(Of String)
            Try
                If OrderNumbers Is Nothing Or OrderNumbers.Count < 1 Then
                    If Me.Debug Then Log("No Purchase Orders")
                    Return False
                Else
                    If Me.Debug Then Log("Process " & OrderNumbers.Count() & " Purchase Orders")
                End If
                For Each Order In OrderNumbers

                    OrderNumber = Trim(Order.POOrders.ToString)
                    'If Me.Debug Then Log("processing PO Number: " & OrderNumber)
                    Dim oBookHeader As New tmsintegrationservices.clsBookHeaderObject705


                    'Modified by RHR for v-7.0.6.105 10/25/2017  query now stored in config file
                    'HeaderCom.CommandText = "select * from pop10100 where PONUMBER = '" & OrderNumber & "'"
                    HeaderCom.CommandText = String.Format(c.GPFunctionsPOHeaders, OrderNumber)

                    HeaderRec = HeaderCom.ExecuteReader
                    Try
                        HeaderRec.Read()

                        If (HeaderRec.HasRows) Then
                            'If Me.Debug Then Log("Attempting to processing PO Number: " & OrderNumber)
                            sErrors = ""
                            'If Me.Debug Then Log("testing For Alpha Code using Loction Code: " & HeaderRec("LOCNCODE").ToString)

                            'Modified by RHR for v-7.0.6.105 10/25/2017  query now stored in config file
                            'LineCom.CommandText = "select top 1 LOCNCODE from POP10110 where PONUMBER = '" & Trim(HeaderRec("PONUMBER").ToString) & "'"
                            LineCom.CommandText = String.Format(c.GPFunctionsPOLocationCode, Trim(HeaderRec("PONUMBER").ToString))
                            LineRec = LineCom.ExecuteReader
                            LineRec.Read()
                            If Not (LineRec.HasRows) Then
                                LogError("Error processing GP Purchase Orders", "Could not read PO Location Code:  " & Trim(HeaderRec("PONUMBER").ToString), Me.AdminEmail)

                                LineRec.Close()
                                Continue For 'go to next po number cannot continue
                            End If
                            LocationCode = Trim(LineRec("LOCNCODE").ToString)
                            LineRec.Close()
                            ProAbb = oBookIntegration.GetCompAbrevByNumberOrAlpha(TMSSetting.TMSAuthCode, 0, LocationCode, "", sErrors)
                            If (String.IsNullOrWhiteSpace(sErrors) AndAlso String.IsNullOrWhiteSpace(ProAbb)) Then
                                ' skip order if failed
                                If Me.Debug Then
                                    Log("Could not process PO: " & OrderNumber & " Due to a missing Company Pro Abb for Comp Alpha Code: " & LocationCode)
                                End If
                                HeaderRec.Close()
                            Else
                                'If Me.Debug Then Log("Processing for Lane: " & LaneValue)
                                LaneValue = GPFunctions.CalculateLane(EconnectStr, 2, OrderNumber, 16384, OrderNumber, ProAbb, 0, sErrors)

                                If (LaneValue = "Error") Then
                                    If Me.Verbose Or Me.Debug Then
                                        Log("Error when processing Lane for purchase order: " & OrderNumber)

                                    End If
                                    HeaderRec.Close()

                                    ' REturn more information 
                                    'LogError("Error when calculating Lane for order: ", sErrors, Me.AdminEmail)

                                Else
                                    'If Me.Debug Then Log("Reading PO data using for Lane: " & LaneValue)
                                    OrderNumber = Trim(HeaderRec("PONUMBER").ToString)
                                    ' Process Order to TMS

                                    ' Changes made by Scott McFarland for VPGC
                                    ' 2017-07-24

                                    ' Insert TMSHeader
                                    oBookHeader.PONumber = HeaderRec("PONUMBER").ToString
                                    oBookHeader.POdate = HeaderRec("DocDate").ToString
                                    oBookHeader.POShipdate = HeaderRec("PRMSHPDTE").ToString
                                    oBookHeader.POVendor = LaneValue 'GPFunctions.CalculateLane(EconnectStr, 2, OrderNumber, 16384, OrderNumber, ProAbb, 0, sErrors)
                                    'oBookHeader.POBuyer = HeaderRec("").ToString
                                    oBookHeader.POBuyer = HeaderRec("BUYERID").ToString
                                    'oBookHeader.POFrt = HeaderRec("").ToString
                                    ' Work with Rob to determine
                                    oBookHeader.POFrt = GPFunctions.GetShippingMethod(EconnectStr, 0, OrderNumber, 16384, OrderNumber, 0)
                                    Double.TryParse(HeaderRec("FRTAMNT").ToString, oBookHeader.POTotalFrt)
                                    Double.TryParse(HeaderRec("SUBTOTAL").ToString, oBookHeader.POTotalCost)
                                    oBookHeader.POWgt = GPFunctions.GetPOTotalWeight(EconnectStr, OrderNumber)
                                    'oBookHeader.POCube = HeaderRec("").ToString
                                    oBookHeader.POCube = 0
                                    oBookHeader.POQty = CInt(GPFunctions.GetPOQuantity(EconnectStr, OrderNumber))
                                    'oBookHeader.POPallets = HeaderRec("").ToString
                                    'Need to define a Pallet formula
                                    oBookHeader.POPallets = CInt(GPFunctions.CalculatePallets(EconnectStr, OrderNumber, 2, Me.AdminEmail, 1))
                                    'oBookHeader.POLines = HeaderRec("").ToString
                                    oBookHeader.POLines = 0
                                    oBookHeader.POConfirm = False
                                    oBookHeader.PODefaultCustomer = "0"
                                    'oBookHeader.PODefaultCarrier = HeaderRec("").ToString
                                    ' Talk to Rob on this field below
                                    oBookHeader.PODefaultCarrier = 0
                                    oBookHeader.POReqDate = HeaderRec("PRMSHPDTE").ToString
                                    ' Write funtion to get shipping instructions
                                    'Updated by SEM 2017-07-30 added get notes
                                    'oBookHeader.POShipInstructions = ""
                                    oBookHeader.POShipInstructions = GPFunctions.GetComments(EconnectStr, HeaderRec("PONUMBER").ToString, 2, Me.AdminEmail)
                                    oBookHeader.POCooler = False
                                    oBookHeader.POFrozen = False
                                    oBookHeader.PODry = False
                                    oBookHeader.POTemp = GPFunctions.GetTemp(EconnectStr, 0, OrderNumber, 1)
                                    oBookHeader.POCarType = ""
                                    oBookHeader.POShipVia = ""
                                    oBookHeader.POShipViaType = ""
                                    oBookHeader.POConsigneeNumber = ""
                                    oBookHeader.POCustomerPO = HeaderRec("VENDORID").ToString
                                    oBookHeader.POOtherCosts = 0
                                    '  Need to determine how this will be set. for now, 0 =New
                                    ' Ship Confirmation, send a 3, 5 is  an order
                                    oBookHeader.POStatusFlag = 5
                                    oBookHeader.POOrderSequence = 0
                                    oBookHeader.POChepGLID = ""
                                    oBookHeader.POCarrierEquipmentCodes = ""
                                    oBookHeader.POCarrierTypeCode = ""
                                    oBookHeader.POPalletPositions = ""
                                    oBookHeader.POSchedulePUDate = ""
                                    oBookHeader.POSchedulePUTime = ""
                                    oBookHeader.POScheduleDelDate = ""
                                    oBookHeader.POSCheduleDelTime = ""
                                    oBookHeader.POActPUDate = ""
                                    oBookHeader.POActPUTime = ""
                                    oBookHeader.POActDelDate = ""
                                    oBookHeader.POActDelTime = ""
                                    oBookHeader.POOrigCompNumber = ""

                                    ' Disabled this originating address as the PO works differently
                                    'PorDAddressInfo = GPFunctions.GetPorDAddress(EconnectStr, locationcode)
                                    'PorDAddressInfo = GPFunctions.GetPorDAddress(EconnectStr, "WAREHOUSE")

                                    'If (PorDAddressInfo.AddressID = "No Address") Then

                                    'LogError("No Address found", "There was no address found for SOP Order: " & OrderNumber, Me.AdminEmail)
                                    'Report to address found

                                    'Else

                                    '  This needs to be examined by Rob & SEM
                                    'oBookHeader.POOrigName = PorDAddressInfo.AddressName
                                    'oBookHeader.POOrigCompAlphaCode = PorDAddressInfo.AddressID
                                    'oBookHeader.POOrigAddress1 = PorDAddressInfo.Address1
                                    'oBookHeader.POOrigAddress2 = PorDAddressInfo.Address2
                                    'oBookHeader.POOrigAddress3 = PorDAddressInfo.Address3
                                    'oBookHeader.POOrigCity = PorDAddressInfo.City
                                    'oBookHeader.POOrigState = PorDAddressInfo.State
                                    'oBookHeader.POOrigCountry = If(String.IsNullOrWhiteSpace(c.GPFunctionsForceDefaultCountry), PorDAddressInfo.Country, c.GPFunctionsForceDefaultCountry)
                                    'oBookHeader.POOrigZip = PorDAddressInfo.ZipCode
                                    'oBookHeader.POOrigContactPhone = PorDAddressInfo.Phone
                                    'oBookHeader.POOrigContactPhoneExt = ""
                                    'oBookHeader.POOrigContactFax = PorDAddressInfo.Fax

                                    'End If

                                    'Originating Address for PO was updated by SEM on 2017-08-02
                                    '  Getting originating address using the Purchase Address ID

                                    oBookHeader.POOrigName = Trim(HeaderRec("PURCHCMPNYNAM").ToString)
                                    oBookHeader.POOrigCompAlphaCode = Trim(HeaderRec("VADCDPAD").ToString)
                                    oBookHeader.POOrigAddress1 = Trim(HeaderRec("PURCHADDRESS1").ToString)
                                    oBookHeader.POOrigAddress2 = Trim(HeaderRec("PURCHADDRESS2").ToString)
                                    oBookHeader.POOrigAddress3 = Trim(HeaderRec("PURCHADDRESS3").ToString)
                                    oBookHeader.POOrigCity = Trim(HeaderRec("PURCHCITY").ToString)
                                    oBookHeader.POOrigState = Trim(HeaderRec("PURCHSTATE").ToString)
                                    oBookHeader.POOrigCountry = If(String.IsNullOrWhiteSpace(HeaderRec("PURCHCOUNTRY").ToString), c.GPFunctionsForceDefaultCountry, HeaderRec("PURCHCOUNTRY").ToString)
                                    oBookHeader.POOrigZip = Trim(HeaderRec("PURCHZIPCODE").ToString)
                                    oBookHeader.POOrigContactPhone = Trim(HeaderRec("PURCHPHONE1").ToString)
                                    oBookHeader.POOrigContactPhoneExt = ""
                                    oBookHeader.POOrigContactFax = Trim(HeaderRec("PURCHFAX").ToString)


                                    oBookHeader.PODestCompNumber = ""
                                    oBookHeader.PODestCompAlphaCode = HeaderRec("PRSTADCD").ToString
                                    oBookHeader.PODestName = HeaderRec("CMPNYNAM").ToString
                                    oBookHeader.PODestAddress1 = HeaderRec("ADDRESS1").ToString
                                    oBookHeader.PODestAddress2 = HeaderRec("ADDRESS2").ToString
                                    oBookHeader.PODestAddress3 = HeaderRec("ADDRESS1").ToString
                                    oBookHeader.PODestCity = HeaderRec("CITY").ToString
                                    oBookHeader.PODestState = HeaderRec("STATE").ToString
                                    oBookHeader.PODestCountry = If(String.IsNullOrWhiteSpace(c.GPFunctionsForceDefaultCountry), HeaderRec("COUNTRY").ToString(), c.GPFunctionsForceDefaultCountry)
                                    oBookHeader.PODestZip = HeaderRec("ZIPCODE").ToString
                                    oBookHeader.PODestContactPhone = HeaderRec("PHONE1").ToString
                                    oBookHeader.PODestContactPhoneExt = ""
                                    oBookHeader.PODestContactFax = HeaderRec("FAX").ToString
                                    oBookHeader.POInbound = True
                                    oBookHeader.POPalletExchange = False
                                    oBookHeader.POPalletType = ""
                                    oBookHeader.POComments = ""
                                    oBookHeader.POCommentsConfidential = ""
                                    oBookHeader.PODefaultRouteSequence = 0
                                    'Modified by RHR v-7.0.5.102 09/26/2016 removed code to populate the PORouteGuideNumber with the Lane Number
                                    'as this is not required.
                                    'oBookHeader.PORouteGuideNumber = GPFunctions.CalculateLane(EconnectStr, 2, OrderNumber, 16384, OrderNumber, ProAbb, SOPType, sErrors)
                                    oBookHeader.PORouteGuideNumber = ""
                                    oBookHeader.POCompLegalEntity = Trim(TMSSetting.LegalEntity.ToString())
                                    'oBookHeader.POCompAlphaCode = "WAREHOUSE"
                                    oBookHeader.POCompAlphaCode = LocationCode
                                    oBookHeader.POModeTypeControl = GPFunctions.GetTransporationMode(OrderNumber, "POP")
                                    oBookHeader.POUser1 = ""
                                    oBookHeader.POUser2 = ""
                                    oBookHeader.POUser3 = ""
                                    oBookHeader.POUser4 = ""
                                    oBookHeader.POAPGLNumber = ""
                                    oBookHeader.POAppt = True

                                    NumberofLines = 0
                                    'Modified by RHR for v-7.0.6.105 10/25/2017
                                    'the code to count the number of items has been noved to below the get item detail c 
                                    'counting the records with a sql command


                                    '**************************  Updated by SEM 10/28/2017  *****************

                                    LineCom.CommandText = String.Format(c.GPFunctionsPOItemDetails, OrderNumber)
                                    'LineCom.CommandText = "select * from pop10110 where POLNESTA in (1, 2, 3) and PONUMBER = '" & OrderNumber & "'"

                                    LineRec = LineCom.ExecuteReader
                                    Dim LineCols = LineRec.GetSchemaTable().Rows.Cast(Of DataRow)().[Select](Function(row) TryCast(row("ColumnName"), String)).ToList()

                                    Counter = 0

                                    While LineRec.Read

                                        Dim oBookDetail As New tmsintegrationservices.clsBookDetailObject705

                                        Counter = Counter + 1

                                        oBookDetail.ItemPONumber = OrderNumber
                                        If (LineCols.Contains("ItemNumber")) Then oBookDetail.ItemNumber = LineRec("ItemNumber").ToString()
                                        If (LineCols.Contains("ItemCost")) Then Double.TryParse(LineRec("ItemCost").ToString(), oBookDetail.ItemCost)
                                        If (LineCols.Contains("QtyOrdered")) Then GPFunctions.TryParseInt(LineRec("QtyOrdered").ToString(), oBookDetail.QtyOrdered)
                                        If (LineCols.Contains("Weight")) Then Double.TryParse(LineRec("Weight").ToString(), oBookDetail.Weight)
                                        If (LineCols.Contains("FixOffInvAllow")) Then Double.TryParse(LineRec("FixOffInvAllow").ToString(), oBookDetail.FixOffInvAllow)
                                        If (LineCols.Contains("FixFrtAllow")) Then Double.TryParse(LineRec("FixFrtAllow").ToString(), oBookDetail.FixFrtAllow)
                                        If (LineCols.Contains("FreightCost")) Then Double.TryParse(LineRec("FreightCost").ToString(), oBookDetail.FreightCost)
                                        If (LineCols.Contains("ItemCost")) Then Double.TryParse(LineRec("ItemCost").ToString(), oBookDetail.ItemCost)
                                        If (LineCols.Contains("Cube")) Then GPFunctions.TryParseInt(LineRec("Cube").ToString(), oBookDetail.Cube)
                                        If (LineCols.Contains("Pack")) Then Short.TryParse(LineRec("Pack").ToString(), oBookDetail.Pack)
                                        If (LineCols.Contains("Size")) Then oBookDetail.Size = LineRec("Size").ToString()
                                        If (LineCols.Contains("Description")) Then oBookDetail.Description = LineRec("Description").ToString
                                        If (LineCols.Contains("Hazmat")) Then oBookDetail.Hazmat = LineRec("Hazmat").ToString()
                                        If (LineCols.Contains("Brand")) Then oBookDetail.Brand = LineRec("Brand").ToString()
                                        If (LineCols.Contains("CostCenter")) Then oBookDetail.CostCenter = LineRec("CostCenter").ToString()
                                        If (LineCols.Contains("LotNumber")) Then oBookDetail.LotNumber = LineRec("LotNumber").ToString()
                                        If (LineCols.Contains("LotExpirationDate")) Then oBookDetail.LotExpirationDate = LineRec("LotExpirationDate").ToString()
                                        If (LineCols.Contains("GTIN")) Then oBookDetail.GTIN = LineRec("GTIN").ToString()
                                        If (LineCols.Contains("CustItemNumber")) Then oBookDetail.CustItemNumber = LineRec("CustItemNumber").ToString()
                                        oBookDetail.CustomerNumber = "0" 'Modified by RHR for v-7.0.5.102 10/17/2016 we do not use the CUSTNMBR here  HeaderRec("CUSTNMBR").ToString
                                        oBookDetail.POOrderSequence = 0
                                        If (LineCols.Contains("PalletType")) Then oBookDetail.PalletType = LineRec("PalletType").ToString()
                                        If (LineCols.Contains("POItemHazmatTypeCode")) Then oBookDetail.POItemHazmatTypeCode = LineRec("POItemHazmatTypeCode").ToString()
                                        If (LineCols.Contains("POItem49CFRCode")) Then oBookDetail.POItem49CFRCode = LineRec("POItem49CFRCode").ToString()
                                        If (LineCols.Contains("POItemIATACode")) Then oBookDetail.POItemIATACode = LineRec("POItemIATACode").ToString()
                                        If (LineCols.Contains("POItemDOTCode")) Then oBookDetail.POItemDOTCode = LineRec("POItemDOTCode").ToString()
                                        If (LineCols.Contains("POItemMarineCode")) Then oBookDetail.POItemMarineCode = LineRec("POItemMarineCode").ToString()
                                        If (LineCols.Contains("POItemNMFCClass")) Then oBookDetail.POItemNMFCClass = LineRec("POItemNMFCClass").ToString()
                                        If (LineCols.Contains("POItemFAKClass")) Then oBookDetail.POItemFAKClass = LineRec("POItemFAKClass").ToString()
                                        If (LineCols.Contains("POItemLimitedQtyFlag")) Then Boolean.TryParse(LineRec("POItemLimitedQtyFlag").ToString(), oBookDetail.POItemLimitedQtyFlag)
                                        If (LineCols.Contains("POItemPallets")) Then Double.TryParse(LineRec("POItemPallets").ToString(), oBookDetail.POItemPallets)
                                        If (LineCols.Contains("POItemTies")) Then Double.TryParse(LineRec("POItemTies").ToString(), oBookDetail.POItemTies)
                                        If (LineCols.Contains("POItemHighs")) Then Double.TryParse(LineRec("POItemHighs").ToString(), oBookDetail.POItemHighs)
                                        If (LineCols.Contains("POItemQtyPalletPercentage")) Then Double.TryParse(LineRec("POItemQtyPalletPercentage").ToString(), oBookDetail.POItemQtyPalletPercentage)
                                        If (LineCols.Contains("POItemQtyLength")) Then Double.TryParse(LineRec("POItemQtyLength").ToString(), oBookDetail.POItemQtyLength)
                                        If (LineCols.Contains("POItemQtyWidth")) Then Double.TryParse(LineRec("POItemQtyWidth").ToString(), oBookDetail.POItemQtyWidth)
                                        If (LineCols.Contains("POItemQtyHeight")) Then Double.TryParse(LineRec("POItemQtyHeight").ToString(), oBookDetail.POItemQtyHeight)
                                        If (LineCols.Contains("POItemStackable")) Then Boolean.TryParse(LineRec("POItemStackable").ToString(), oBookDetail.POItemStackable)
                                        If (LineCols.Contains("POItemLevelOfDensity")) Then GPFunctions.TryParseInt(LineRec("POItemLevelOfDensity").ToString(), oBookDetail.POItemLevelOfDensity)
                                        oBookDetail.POItemCompLegalEntity = Trim(TMSSetting.LegalEntity.ToString())
                                        oBookDetail.POItemCompAlphaCode = LocationCode
                                        'oBookDetail.POItemCompAlphaCode = "WAREHOUSE"
                                        If (LineCols.Contains("POItemNMFCSubClass")) Then oBookDetail.POItemNMFCSubClass = LineRec("POItemNMFCSubClass").ToString()
                                        If (LineCols.Contains("POItemUser1")) Then oBookDetail.POItemUser1 = LineRec("POItemUser1").ToString()
                                        If (LineCols.Contains("POItemUser2")) Then oBookDetail.POItemUser2 = LineRec("POItemUser2").ToString()
                                        If (LineCols.Contains("POItemUser3")) Then oBookDetail.POItemUser3 = LineRec("POItemUser3").ToString()
                                        If (LineCols.Contains("POItemUser4")) Then oBookDetail.POItemUser4 = LineRec("POItemUser4").ToString()
                                        If (LineCols.Contains("POItemWeightUnitOfMeasure")) Then oBookDetail.POItemWeightUnitOfMeasure = LineRec("POItemWeightUnitOfMeasure").ToString()
                                        If (LineCols.Contains("POItemCubeUnitOfMeasure")) Then oBookDetail.POItemCubeUnitOfMeasure = LineRec("POItemCubeUnitOfMeasure").ToString()
                                        If (LineCols.Contains("POItemDimensionUnitOfMeasure")) Then oBookDetail.POItemDimensionUnitOfMeasure = LineRec("POItemDimensionUnitOfMeasure").ToString()
                                        oBookDetail.BookItemCommCode = oBookHeader.POTemp

                                        'Modified by RHR for v-7.0.6.105 on 10/31/2017 
                                        'we do not add items to oBookDetails until HasChanged is tested
                                        lItemDetailsToProcess.Add(oBookDetail)
                                        'oBookDetails.Add(oBookDetail)

                                        oBookDetail = Nothing

                                    End While

                                    ' Report something if no line recvords
                                    If (Counter = 0) Then

                                        If Me.Verbose Then Log("No Line records to process for PO #: " & OrderNumber)

                                    End If

                                    LineRec.Close()
                                    HeaderRec.Close()
                                    Try
                                        sErrors = ""
                                        OrderChanged = oBookIntegration.HasOrderChanged(TMSSetting.TMSAuthCode, oBookHeader, Counter, OrdChangeStr, False, False, sErrors)
                                        If Not String.IsNullOrWhiteSpace(sErrors) Then Log("Process PO Order Changed Error: " & sErrors)
                                    Catch ex As Exception
                                        LogError("Error in Processing PO Orders - at the Order Change.  The Error is:  ", ex.Message, Me.AdminEmail)
                                    End Try

                                    If (Not OrderChanged) Then
                                        If Me.Debug Then Log("Skiping purhcase order because the order has not changed: " & OrderNumber)
                                    Else
                                        POOrdersProcessed.Add(Order.POOrders)
                                        oBookHeaders.Add(oBookHeader)
                                        'Modified by RHR for v-7.0.6.105 on 10/31/2017 
                                        'we do not add items to oBookDetails until HasChanged is tested
                                        If Not lItemDetailsToProcess Is Nothing AndAlso lItemDetailsToProcess.Count > 0 Then
                                            oBookDetails.AddRange(lItemDetailsToProcess)
                                        End If
                                        lItemDetailsToProcess = New List(Of tmsintegrationservices.clsBookDetailObject705)
                                    End If

                                    If (Not HeaderRec.IsClosed) Then
                                        HeaderRec.Close()
                                    End If
                                    oBookHeader = Nothing
                                End If
                            End If
                        Else
                            If Me.Verbose Then Log("PO In List, but query did not return header data  for PO #: " & OrderNumber)
                        End If
                    Catch ex As Exception
                        Throw
                    Finally
                        'besure the reader is closed
                        Try
                            If Not HeaderRec Is Nothing AndAlso (Not HeaderRec.IsClosed) Then
                                HeaderRec.Close()
                            End If
                        Catch ex As Exception
                        End Try
                        Try
#Disable Warning BC42104 ' Variable 'LineRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                            If Not LineRec Is Nothing AndAlso (Not LineRec.IsClosed) Then
#Enable Warning BC42104 ' Variable 'LineRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                                LineRec.Close()
                            End If
                        Catch ex As Exception
                        End Try
                    End Try
                    If Not oBookHeaders Is Nothing AndAlso oBookHeaders.Count() > 0 Then
                        If Me.Debug Then Log("Processing Book Object Data for POs; Headers: " & oBookHeaders.Count().ToString() & " Details: " & oBookDetails.Count().ToString())

                        'Modified by RHR for v-7.0.5.102 on 11/10/2016
                        Dim aBookHeaders As tmsintegrationservices.clsBookHeaderObject705() = oBookHeaders.ToArray()
                        Dim aBookDetails As tmsintegrationservices.clsBookDetailObject705()
                        sErrors = ""
                        If Not oBookDetails Is Nothing AndAlso oBookDetails.Count() > 0 Then aBookDetails = oBookDetails.ToArray()
#Disable Warning BC42104 ' Variable 'aBookDetails' is used before it has been assigned a value. A null reference exception could result at runtime.
                        Dim oResults As TMS.Configuration.ProcessDataReturnValues = oBookIntegration.ProcessBookData705(TMSSetting.TMSAuthCode, aBookHeaders, aBookDetails, sErrors)
#Enable Warning BC42104 ' Variable 'aBookDetails' is used before it has been assigned a value. A null reference exception could result at runtime.
                        Dim ReturnMessage As String = sErrors
                        If Me.Debug Then Log("Results = " & oResults.ToString())
                        Select Case oResults
                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Data Connection Failure! could not import PO information")
                                Else
                                    sIntegrationErrors.Add("Data Connection Failure! could not import PO information:  " & ReturnMessage)
                                End If
                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Integration Failure! could not import PO information")
                                Else
                                    generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                                End If

                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Integration Had Errors! could not import some PO information")
                                Else
                                    generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                                End If

                                'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                'blnRet = True
                                'End If
                            Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                                If String.IsNullOrWhiteSpace(ReturnMessage) Then
                                    If Me.Verbose Then Log("Data Validation Failure! could not import some PO information")
                                Else
                                    generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                                End If
                                'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                'blnRet = True
                                'End If
                            Case Else
                                'success
                                'Removed by RHR v-7.0.5.102 8/15/2016 because we do not have a list of Processed PO Orders to list
                                'TODO: fix list of purchase orders
                                Dim strNumbers As String = "N/A"
                                If Not POOrdersProcessed Is Nothing AndAlso POOrdersProcessed.Count() > 0 Then
                                    strNumbers = String.Join(", ", POOrdersProcessed)
                                End If
                                'Dim strNumbers As String = String.Join(", ", ProcessGPPOOrders)
                                sLogMsgs.Add("Success! the following PO Numbers were processed: " & strNumbers)
                                'Processed = oResults.
                                'TODO: add code to send confirmation back to NAV that the orders were processed
                                'mark process and success
                                'blnRet = True
                        End Select
                        oResults = Nothing
                    End If
                    oBookHeaders.Clear()
                    oBookDetails.Clear()
                Next

                If Not sIntegrationErrors Is Nothing AndAlso sIntegrationErrors.Count() > 0 Then
                    LogError("Warning!  " & Source & " Had Errors: " & vbCrLf & String.Join(vbCrLf, sIntegrationErrors.ToArray()))
                End If
                'If Not sAdminErrors Is Nothing AndAlso sAdminErrors.Count() > 0 Then
                'LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sAdminErrors.ToArray()), Me.AdminEmail)
                'End If
                'If Not sOperationErrors Is Nothing AndAlso sOperationErrors.Count() > 0 Then
                'LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sOperationErrors.ToArray()), Me.GroupEmail)
                'End If
                'If Not sLogMsgs Is Nothing AndAlso sLogMsgs.Count() > 0 Then
                If Me.Verbose Then Log(Source & ": " & vbCrLf & String.Join(vbCrLf, sLogMsgs.ToArray()))
                'End If
                If Debug Then Log("Process Order Data Complete")

            Catch ex As Exception

                'MsgBox("Inner Try " & ex.Message)

                Log("here was an error processing GP Purchase Orders. The error is: " & ex.Message)

            Finally
                Try
                    ' End Routine
                    HeaderCom.Dispose()
                    HeaderConn.Dispose()

                    LineCom.Dispose()
                    LineConn.Dispose()
                Catch ex As Exception

                End Try
            End Try
        Catch ex As Exception
            Log(Source & ".ProcessGPPOOrders Error!  Unexpected GP Process PO Orders Error could not process any integration requests; the actual error is:  " & ex.Message)

        Finally
            Try
#Disable Warning BC42104 ' Variable 'HeaderRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not HeaderRec Is Nothing AndAlso (Not HeaderRec.IsClosed) Then HeaderRec.Close()
#Enable Warning BC42104 ' Variable 'HeaderRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not LineRec Is Nothing AndAlso (Not LineRec.IsClosed) Then LineRec.Close()
            Catch ex As Exception

            End Try
            Try
                If Not HeaderCom Is Nothing Then HeaderCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not LineCom Is Nothing Then LineCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not HeaderConn Is Nothing AndAlso Not HeaderConn.State = ConnectionState.Closed Then HeaderConn.Close()
                HeaderConn.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not LineConn Is Nothing AndAlso Not LineConn.State = ConnectionState.Closed Then LineConn.Close()
                LineConn.Dispose()
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function

    Public Function GetGPSOPOrders(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByVal SOPType As Integer, ByRef ErrorString As String, ByVal AdminEmail As String, ByRef c As clsDefaultIntegrationConfiguration, Optional ByVal ShipConfirmFromDate As Date? = Nothing) As List(Of GPDataIntegrationSTructure.SOPOrders)

        Dim ReturnValue As New List(Of GPDataIntegrationSTructure.SOPOrders)
        Dim EconnectStr As String = ""
        If Not String.IsNullOrWhiteSpace(TMSSetting.ERPUser) AndAlso Not String.IsNullOrWhiteSpace(TMSSetting.ERPPassword) Then
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";User ID=" & TMSSetting.ERPUser & ";Password=" & TMSSetting.ERPPassword
        Else
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        End If
        'EconnectStr = "Data Source=" & GetAppSettings.GetValue("SQLServer") & "; Initial Catalog=" & GPCompany & "Integrated Security=SSPI;Trusted_Connection=True"
        'EconnectStr = "Data Source=" & TMSSetting.ERPCertificate.ToString & "; Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        'EconnectStr = "Data Source=NGLGP2013R2; Initial Catalog=TWO;Integrated Security=SSPI;Trusted_Connection=True"
        Dim GetAppSettings = New AppSettingsReader
        Dim SQLConn = New SqlConnection(EconnectStr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader
        ErrorString = ""

        Try


            SQLConn.Open()

            SQLCom.Connection = SQLConn

            Select Case SOPType

                Case 2
                    'select SOPNUMBE From SOP10100 where SOPTYPE = {0} and ReqShipDate > dateadd(d,-14,getdate())
                    SQLCom.CommandText = String.Format(c.GPFunctionsSOsToProcess, SOPType)
                Case 3
                    'select SOPNUMBE from SOP30200 where SOPTYPE = {0} and dex_row_ts >= '{1}'
                    Dim strFromDateFilter As String = FormatDateTime(CDate(c.ERPPayablesLastRunDate), DateFormat.ShortDate)
                    If ShipConfirmFromDate.HasValue Then
                        strFromDateFilter = ShipConfirmFromDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")
                    End If
                    SQLCom.CommandText = String.Format(c.GPFunctionsSOConsToProcess, SOPType, strFromDateFilter)
                    If Me.Debug Then Log(SQLCom.CommandText)
                Case Else

            End Select


            TblRec = SQLCom.ExecuteReader

            While (TblRec.Read)

                Dim OrderNumber As New GPDataIntegrationSTructure.SOPOrders

                OrderNumber.SOPOrders = Trim(TblRec(0).ToString())

                ReturnValue.Add(OrderNumber)

                OrderNumber = Nothing

            End While

            TblRec.Close()

            SQLCom.Dispose()

        Catch ex As Exception

            LogError(Source & " Error!  Unexpected GP Process Sales Order Error", Source & " Could not process any read sales order records, the actual error is:  " & ex.Message, AdminEmail)
            'Throw
        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then TblRec.Close()
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not SQLCom Is Nothing Then SQLCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then SQLConn.Close()
                SQLConn.Dispose()
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function

    Public Function GetGPTOPOrders(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByVal SOPType As Integer, ByRef ErrorString As String, ByVal AdminEmail As String) As List(Of GPDataIntegrationSTructure.SOPOrders)

        Dim ReturnValue As New List(Of GPDataIntegrationSTructure.SOPOrders)
        Dim EconnectStr As String = ""
        If Not String.IsNullOrWhiteSpace(TMSSetting.ERPUser) AndAlso Not String.IsNullOrWhiteSpace(TMSSetting.ERPPassword) Then
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";User ID=" & TMSSetting.ERPUser & ";Password=" & TMSSetting.ERPPassword
        Else
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        End If

        'EconnectStr = "Data Source=" & GetAppSettings.GetValue("SQLServer") & "; Initial Catalog=" & GPCompany & "Integrated Security=SSPI;Trusted_Connection=True"
        'EconnectStr = "Data Source=" & TMSSetting.ERPCertificate.ToString & "; Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        'EconnectStr = "Data Source=NGLGP2013R2; Initial Catalog=TWO;Integrated Security=SSPI;Trusted_Connection=True"
        Dim GetAppSettings = New AppSettingsReader
        Dim SQLConn = New SqlConnection(EconnectStr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader
        ErrorString = ""

        Try


            SQLConn.Open()

            SQLCom.Connection = SQLConn

            SQLCom.CommandText = "select SOPNUMBE From SOP10100 where SOPTYPE = " & SOPType & " and ReqShipDate > dateadd(d,-14,getdate())"
            'SQLCom.CommandText = "select SOPNUMBE From SOP10100 where SOPTYPE = 2 and sopnumbe = 'ORDST2225'"

            TblRec = SQLCom.ExecuteReader

            While (TblRec.Read)

                Dim OrderNumber As New GPDataIntegrationSTructure.SOPOrders

                OrderNumber.SOPOrders = Trim(TblRec("SOPNUMBE").ToString())

                ReturnValue.Add(OrderNumber)

                OrderNumber = Nothing

            End While

            TblRec.Close()

            SQLCom.Dispose()

        Catch ex As Exception

            LogError(Source & " Error!  Unexpected GP GetFreightPayments Error", Source & " Could not process any get freight payment requests; the actual error is:  " & ex.Message, AdminEmail)
            'Throw
        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then TblRec.Close()
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not SQLCom Is Nothing Then SQLCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then SQLConn.Close()
                SQLConn.Dispose()
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function

    Public Function GetGPInvTransfers(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByRef ErrorString As String, ByVal AdminEmail As String) As List(Of GPDataIntegrationSTructure.InvTransfers)

        Dim ReturnValue As New List(Of GPDataIntegrationSTructure.InvTransfers)
        Dim EconnectStr As String = ""
        If Not String.IsNullOrWhiteSpace(TMSSetting.ERPUser) AndAlso Not String.IsNullOrWhiteSpace(TMSSetting.ERPPassword) Then
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";User ID=" & TMSSetting.ERPUser & ";Password=" & TMSSetting.ERPPassword
        Else
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        End If
        'EconnectStr = "Data Source=" & GetAppSettings.GetValue("SQLServer") & "; Initial Catalog=" & GPCompany & "Integrated Security=SSPI;Trusted_Connection=True"
        'EconnectStr = "Data Source=" & TMSSetting.ERPCertificate.ToString & "; Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        'EconnectStr = "Data Source=NGLGP2013R2; Initial Catalog=TWO;Integrated Security=SSPI;Trusted_Connection=True"
        Dim GetAppSettings = New AppSettingsReader
        Dim SQLConn = New SqlConnection(EconnectStr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader
        ErrorString = ""

        Try


            SQLConn.Open()

            SQLCom.Connection = SQLConn

            SQLCom.CommandText = "select ORDDOCID From SVC00700 where ORDRDATE > dateadd(d,-14,getdate())"

            TblRec = SQLCom.ExecuteReader

            While (TblRec.Read)

                Dim OrderNumber As New GPDataIntegrationSTructure.InvTransfers

                OrderNumber.InvTransferID = Trim(TblRec("ORDDOCID").ToString())

                ReturnValue.Add(OrderNumber)

                OrderNumber = Nothing

            End While

            TblRec.Close()

            SQLCom.Dispose()
            SQLCom.Dispose()

        Catch ex As Exception

            LogError(Source & " Error!  Unexpected GP Get Inventory Transfer Error", Source & " Could not process any get inventory transfer; the actual error is:  " & ex.Message, AdminEmail)
            'Throw
        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then TblRec.Close()
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                TblRec = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not SQLCom Is Nothing Then SQLCom.Dispose()
                SQLCom = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then SQLConn.Close()
                SQLConn.Dispose()
                SQLConn = Nothing
            Catch ex As Exception

            End Try
            Try
                GetAppSettings = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function

    Public Function GetGPPOOrders(ByRef c As clsDefaultIntegrationConfiguration, ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting, ByVal SOPType As Integer, ByRef ErrorString As String, ByVal AdminEmail As String) As List(Of GPDataIntegrationSTructure.POOrders)

        Dim ReturnValue As New List(Of GPDataIntegrationSTructure.POOrders)
        Dim EconnectStr As String = ""
        Dim GetAppSettings = New AppSettingsReader
        If Not String.IsNullOrWhiteSpace(TMSSetting.ERPUser) AndAlso Not String.IsNullOrWhiteSpace(TMSSetting.ERPPassword) Then
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";User ID=" & TMSSetting.ERPUser & ";Password=" & TMSSetting.ERPPassword
        Else
            EconnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
        End If
        'EconnectStr = "Data Source=" & GetAppSettings.GetValue("SQLServer") & "; Initial Catalog=" & GPCompany & "Integrated Security=SSPI;Trusted_Connection=True"
        'EconnectStr = "Data Source=" & TMSSetting.ERPCertificate.ToString & "; Initial Catalog=" & TMSSetting.LegalEntity.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"

        'EconnectStr = "Data Source=NGLGP2013R2; Initial Catalog=TWO;Integrated Security=SSPI;Trusted_Connection=True"

        Dim SQLConn = New SqlConnection(EconnectStr)
        Dim SQLCom As New SqlCommand
        Dim TblRec As SqlDataReader
        ErrorString = ""

        Try



            SQLConn.Open()

            SQLCom.Connection = SQLConn

            SQLCom.CommandText = String.Format(c.GPFunctionsPOsToProcess)

            'SQLCom.CommandText = "select PONUMBER from pop10100 where postatus in (1, 2, 3) and REQDATE > dateadd(d,-14,getdate())"
            'SQLCom.CommandText = "select SOPNUMBE From SOP10100 where SOPTYPE = 2 and sopnumbe = 'ORDST2225'"

            TblRec = SQLCom.ExecuteReader

            While (TblRec.Read)

                Dim OrderNumber As New GPDataIntegrationSTructure.POOrders

                OrderNumber.POOrders = TblRec("PONUMBER").ToString

                ReturnValue.Add(OrderNumber)

                OrderNumber = Nothing

            End While

            TblRec.Close()

            SQLCom.Dispose()
            SQLCom.Dispose()

        Catch ex As Exception

            LogError(Source & " Error!  Unexpected GP Get Purchase Orders Error", Source & " Could not read the purchase order informaiton; the actual error is:  " & ex.Message, AdminEmail)
            'Throw
        Finally
            Try
#Disable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not TblRec Is Nothing AndAlso (Not TblRec.IsClosed) Then TblRec.Close()
#Enable Warning BC42104 ' Variable 'TblRec' is used before it has been assigned a value. A null reference exception could result at runtime.
            Catch ex As Exception

            End Try
            Try
                If Not SQLCom Is Nothing Then SQLCom.Dispose()
            Catch ex As Exception

            End Try
            Try
                If Not SQLConn Is Nothing AndAlso Not SQLConn.State = ConnectionState.Closed Then SQLConn.Close()
                SQLConn.Dispose()
            Catch ex As Exception

            End Try
        End Try

        Return ReturnValue

    End Function



#End Region




#Region "Base Class Overloads "

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns>Boolean</returns>
    ''' <remarks>
    ''' Modified By LVV 2/19/16 v-7.0.5.0
    ''' Added call to getNewTaskParameters()
    ''' </remarks>
    Public Overrides Function getTaskParameters() As Boolean
        Dim blnRet As Boolean = False
        Try
            If Not GlobalTaskParameters Is Nothing Then
                With GlobalTaskParameters
                    AutoRetry = .GlobalAutoRetry
                    AdminEmail = .GlobalAdminEmail
                    GroupEmail = .GlobalGroupEmail
                    FromEmail = .GlobalFromEmail
                    SMTPServer = .GlobalSMTPServer
                    SaveOldLog = .GlobalSaveOldLogs
                    KeepLogDays = .GlobalKeepLogDays
                    'the command line parameter overrides the global debug mode but only if it is true
                    'If Not Debug Then
                    '    Debug = .GlobalDebugMode
                    'End If
                    GlobalFuelIndexUpdateEmailNotification = .GlobalFuelIndexUpdateEmailNotification
                    GlobalFuelIndexUpdateEmailNotificationValue = .GlobalFuelIndexUpdateEmailNotificationValue
                    GlobalCarrierContractExpiredEmailNotification = .GlobalCarrierContractExpiredEmailNotification
                    GlobalCarrierContractExpiredEmailNotificationValue = .GlobalCarrierContractExpiredEmailNotificationValue
                    GlobalCarrierExposureAllEmailNotification = .GlobalCarrierExposureAllEmailNotification
                    GlobalCarrierExposureAllEmailNotificationValue = .GlobalCarrierExposureAllEmailNotificationValue
                    GlobalCarrierExposurePerShipmentEmailNotification = .GlobalCarrierExposurePerShipmentEmailNotification
                    GlobalCarrierExposurePerShipmentEmailNotificationValue = .GlobalCarrierExposurePerShipmentEmailNotificationValue
                    GlobalCarrierInsuranceExpiredEmailNotification = .GlobalCarrierInsuranceExpiredEmailNotification
                    GlobalCarrierInsuranceExpiredEmailNotificationValue = .GlobalCarrierInsuranceExpiredEmailNotificationValue
                    GlobalOutdatedNoLanePOEmailNotification = .GlobalOutdatedNoLanePOEmailNotification
                    GlobalOutdatedNoLanePOEmailNotificationValue = .GlobalOutdatedNoLanePOEmailNotificationValue
                    GlobalOutdatedNStatusEmailNotification = .GlobalOutdatedNStatusEmailNotification
                    GlobalOutdatedNStatusEmailNotificationValue = .GlobalOutdatedNStatusEmailNotificationValue
                    GlobalPOsWaitingEmailNotification = .GlobalPOsWaitingEmailNotification
                    GlobalPOsWaitingEmailNotificationValue = .GlobalPOsWaitingEmailNotificationValue
                    GlobalDefaultLoadAcceptAllowedMinutes = .GlobalDefaultLoadAcceptAllowedMinutes
                    NEXTStopAcctNo = .NEXTStopAcctNo
                    NEXTStopContact = .NEXTStopContact
                    NEXTStopHotLoadAccountName = .NEXTStopHotLoadAccountName
                    NEXTStopHotLoadContact = .NEXTStopHotLoadContact
                    NEXTStopHotLoadURL = .NEXTStopHotLoadURL
                    NEXTStopPhone = .NEXTStopPhone
                    NEXTStopURL = .NEXTStopURL
                    NEXTrackURL = .NEXTrackURL
                    NEXTRackDatabase = .NEXTRackDatabase
                    NEXTRackDatabaseServer = .NEXTRackDatabaseServer
                    GlobalSMTPUser = .GlobalSMTPUser
                    GlobalSMTPPass = .GlobalSMTPPass
                    ReportServerURL = .ReportServerURL
                    ReportServerUser = .ReportServerUser
                    ReportServerPass = .ReportServerPass
                    ReportServerDomain = .ReportServerDomain
                End With

                'Added By LVV 2/19/16 v-7.0.5.0
                processNewTaskParameters(GlobalTaskParameters)

            End If
            blnRet = True
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            LogError(Source & " Warning:  Read Global Task Parameters Failed", ex.Detail.ToString(ex.Reason.ToString()), Me.AdminEmail)
        Catch ex As Exception
            LogError(Source & " Warning:  Read Global Task Parameters Failed", "Read Global Task Parameters Failed", Me.AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 on 11/14/2016 
    '''   orverrides the default behavior of connecting to the db to validate the connection string 
    '''   and create the log file.  we now use web services for all connection to the db.  this override
    '''   creates the log file only.
    ''' </remarks>
    Public Overrides Function validateDatabase() As Boolean
        Dim blnRet As Boolean = True

        'set the default value for the log file

        If Not System.IO.Directory.Exists("C:\Data") Then
            System.IO.Directory.CreateDirectory("C:\Data")
        End If
        If Not System.IO.Directory.Exists("C:\Data\TMSLogs") Then
            System.IO.Directory.CreateDirectory("C:\Data\TMSLogs")
        End If
        LogFile = "C:\Data\TMSLogs\" & Source & "." & Database & ".log"
        openLog()

        If LogFile <> "C:\Data\TMSLogs\" & Source & "." & Database & ".log" Then
            'reset the value for the log file if the database name has changed
            LogFile = "C:\Data\TMSLogs\" & Source & "." & Database & ".log"
            closeLog(0)
            openLog()
        End If
        If Me.Debug Then Log("Ready!")

        Return blnRet

    End Function

    Public Overrides Sub LogError(strSubject As String, logMessage As String, strMailTo As String)
        Try

            If dictErrorsToLog Is Nothing Then dictErrorsToLog = New Dictionary(Of String, List(Of clsErrorMessages))
            Dim oMsg As New clsErrorMessages(strSubject, logMessage)
            Dim lMessages As New List(Of clsErrorMessages)
            If dictErrorsToLog.ContainsKey(strMailTo) Then
                lMessages = dictErrorsToLog(strMailTo)
                If lMessages Is Nothing Then lMessages = New List(Of clsErrorMessages)
                lMessages.Add(oMsg)
                dictErrorsToLog(strMailTo) = lMessages
            Else
                lMessages.Add(oMsg)
                dictErrorsToLog.Add(strMailTo, lMessages)
            End If
        Catch ex As Exception
            'do nothing when logging errors
        End Try
    End Sub

    ''' <summary>
    ''' this method call the base calss LogError method for each email address in dictErrorsToLog 
    ''' </summary>
    Public Sub LogAllErrors()
        Try
            If dictErrorsToLog Is Nothing OrElse dictErrorsToLog.Count < 1 Then Return 'nothing to do
            Dim strSubject = "GP Integration Errors"
            For Each oErr In dictErrorsToLog
                Dim lMessages As List(Of clsErrorMessages) = oErr.Value
                Dim sToEmail = oErr.Key
                Dim sbMsg As New StringBuilder()
                For Each m In lMessages
                    sbMsg.Append(String.Format("Subject: {0} {1} Body: {2} {1}", m.Subject, vbCrLf, m.Message))
                Next
                MyBase.LogError(strSubject, sbMsg.ToString(), sToEmail)
            Next
        Catch ex As Exception
            'do nothing when we log errors
        End Try

    End Sub
#End Region

#Region "Properties "

    Private _dictErrorsToLog As New Dictionary(Of String, List(Of clsErrorMessages))
    ''' <summary>
    ''' dictionary of error messages to process
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' This dictionary stores messages associated with an email address,  the key is the email address and the message is a list of clsErrorMessages
    ''' </remarks>
    Public Property dictErrorsToLog() As Dictionary(Of String, List(Of clsErrorMessages))
        Get
            Return _dictErrorsToLog
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of clsErrorMessages)))
            _dictErrorsToLog = value
        End Set
    End Property

    Private _WCFParameters As DAL.WCFParameters
#Disable Warning BC40003 ' property 'WCFParameters' shadows an overloadable member declared in the base class 'NGLCommandLineBaseClass'.  If you want to overload the base method, this method must be declared 'Overloads'.
    Public Property WCFParameters() As DAL.WCFParameters
#Enable Warning BC40003 ' property 'WCFParameters' shadows an overloadable member declared in the base class 'NGLCommandLineBaseClass'.  If you want to overload the base method, this method must be declared 'Overloads'.
        Get
            If _WCFParameters Is Nothing Then
                'Note: WCFAuthCode = "NGLSystem" does not validate user when ValidateAccess = False 
                _WCFParameters = New DAL.WCFParameters With {.UserName = "",
                                                             .Database = Me.Database,
                                                             .DBServer = Me.DBServer,
                                                             .ConnectionString = Me.ConnectionString,
                                                             .WCFAuthCode = "NGLSystem",
                                                             .ValidateAccess = False}
            End If
            Return _WCFParameters
        End Get
        Set(ByVal value As DAL.WCFParameters)
            _WCFParameters = value
        End Set
    End Property

    Private _NGLDynamicsTMSSettingData As DAL.NGLDynamicsTMSSettingData
    ''' <summary>
    ''' Local instance of the DAL.NGLDynamicsTMSSettingData class
    ''' If WCFParameters change set to nothing then get new instance
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLDynamicsTMSSettingData() As DAL.NGLDynamicsTMSSettingData
        Get
            If _NGLDynamicsTMSSettingData Is Nothing Then _NGLDynamicsTMSSettingData = New DAL.NGLDynamicsTMSSettingData(WCFParameters)
            Return _NGLDynamicsTMSSettingData
        End Get
        Set(value As DAL.NGLDynamicsTMSSettingData)
            _NGLDynamicsTMSSettingData = value
        End Set
    End Property

    Private _NGLBatchProcessDataProvider As DAL.NGLBatchProcessDataProvider
    ''' <summary>
    ''' Local instance of the DAL.NGLBatchProcessDataProvider class
    ''' If WCFParameters change set to nothing then get new instance
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLBatchProcessDataProvider() As DAL.NGLBatchProcessDataProvider
        Get
            If _NGLBatchProcessDataProvider Is Nothing Then _NGLBatchProcessDataProvider = New DAL.NGLBatchProcessDataProvider(WCFParameters)
            Return _NGLBatchProcessDataProvider
        End Get
        Set(ByVal value As DAL.NGLBatchProcessDataProvider)
            _NGLBatchProcessDataProvider = value
        End Set
    End Property

    Private _NGLSystemDataProvider As DAL.NGLSystemDataProvider
    ''' <summary>
    ''' Local instance of the DAL.NGLSystemDataProvider class
    ''' If WCFParameters change set to nothing then get new instance
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLSystemDataProvider() As DAL.NGLSystemDataProvider
        Get
            If _NGLSystemDataProvider Is Nothing Then _NGLSystemDataProvider = New DAL.NGLSystemDataProvider(WCFParameters)
            Return _NGLSystemDataProvider
        End Get
        Set(value As DAL.NGLSystemDataProvider)
            _NGLSystemDataProvider = value
        End Set
    End Property

    Private _NGLBookRevenueBLL As BLL.NGLBookRevenueBLL
    ''' <summary>
    ''' Local instance of the BLL.NGLBookRevenueBLL class
    ''' If WCFParameters change set to nothing then get new instance
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLBookRevenueBLL() As BLL.NGLBookRevenueBLL
        Get
            If _NGLBookRevenueBLL Is Nothing Then _NGLBookRevenueBLL = New BLL.NGLBookRevenueBLL(WCFParameters)
            Return _NGLBookRevenueBLL
        End Get
        Set(ByVal value As BLL.NGLBookRevenueBLL)
            _NGLBookRevenueBLL = value
        End Set
    End Property

    Private _NGLCarrierData As DAL.NGLCarrierData
    ''' <summary>
    ''' Local instance of the DAL.NGLCarrierData class
    ''' If WCFParameters change set to nothing then get new instance
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLCarrierData() As DAL.NGLCarrierData
        Get
            If _NGLCarrierData Is Nothing Then _NGLCarrierData = New DAL.NGLCarrierData(WCFParameters)
            Return _NGLCarrierData
        End Get
        Set(ByVal value As DAL.NGLCarrierData)
            _NGLCarrierData = value
        End Set
    End Property




    Private _GlobalTaskParameters As DAL.DataTransferObjects.GlobalTaskParameters
    ''' <summary>
    ''' Local instance of DAL.DataTransferObjects.GlobalTaskParameters class 
    ''' If WCFParameters change set to nothing then get a new instance
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 on 11/14/2016
    '''   removed call to getSystemParameters because we now use
    '''   web services to populate the data
    ''' </remarks>
    Public Property GlobalTaskParameters() As DAL.DataTransferObjects.GlobalTaskParameters
        Get
            'Modified by RHR for v-7.0.5.102 on 11/14/2016
            'If _GlobalTaskParameters Is Nothing Then
            '    _GlobalTaskParameters = getSystemParameters()
            'End If
            If _GlobalTaskParameters Is Nothing Then
                _GlobalTaskParameters = New DAL.DataTransferObjects.GlobalTaskParameters()
            End If
            Return _GlobalTaskParameters
        End Get
        Set(ByVal value As DAL.DataTransferObjects.GlobalTaskParameters)
            _GlobalTaskParameters = value
        End Set
    End Property

    Private _GPTestingOn As Boolean = False
    Public Property GPTestingOn() As Boolean
        Get
            Return _GPTestingOn
        End Get
        Set(ByVal value As Boolean)
            _GPTestingOn = value
        End Set
    End Property


    Protected oConfig As New UserConfiguration

#End Region

#Region "DAL Methods "

    Protected Function getDynamicsTMSSettings(ByVal LegalEntity As String) As List(Of DAL.DataTransferObjects.DynamicsTMSSetting)

        Dim oDTMSSettings As New List(Of DAL.DataTransferObjects.DynamicsTMSSetting)
        Dim oDTMSData As DAL.DataTransferObjects.DynamicsTMSSetting
        Try
            If String.IsNullOrWhiteSpace(LegalEntity) Then
                oDTMSSettings = NGLDynamicsTMSSettingData.GetDynamicsTMSSettings().ToList()
            Else
                oDTMSData = NGLDynamicsTMSSettingData.GetDynamicsTMSSettingFiltered(LegalEntity)
                If oDTMSData Is Nothing Then oDTMSSettings.Add(oDTMSData)
            End If
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            LogError(Source & " Warning:  Read Dynamics TMS Integration Settings Failed", ex.Detail.ToString(ex.Reason.ToString()), Me.AdminEmail)
        Catch ex As Exception
            LogError(Source & " Warning:  Read Dynamics TMS Integration Settings Failed", "Read Dynamics TMS Integration Settings Failed", Me.AdminEmail, ex)
        End Try
        Return oDTMSSettings
    End Function

    Protected Function CreateGPTMSSettings(ByVal sDTMSLegalEntity As String,
                                            ByVal sDTMSGPWebServiceURL As String,
                                            ByVal sDTMSGPUserName As String,
                                            ByVal sDTMSGPPassword As String,
                                            Optional ByVal iDTMSPicklistMaxRetry As Integer = 1,
                                            Optional ByVal iDTMSPicklistRetryMinutes As Integer = 30,
                                            Optional ByVal iDTMSPicklistMaxRowsReturned As Integer = 10,
                                            Optional ByVal bDTMSPicklistAutoConfirmation As Boolean = False,
                                            Optional ByVal iDTMSAPExportMaxRetry As Integer = 5,
                                            Optional ByVal iDTMSAPExportRetryMinutes As Integer = 30,
                                            Optional ByVal iDTMSAPExportMaxRowsReturned As Integer = 10,
                                            Optional ByVal bDTMSAPExportAutoConfirmation As Boolean = False,
                                            Optional ByVal bDTMSGPUseDefaultCredentials As Boolean = True,
                                            Optional ByVal sDTMSWSAuthCode As String = "NGLWSDEV",
                                            Optional ByVal sDTMSWSURL As String = "http://nglwsdev704.nextgeneration.com",
                                            Optional ByVal sDTMSWCFAuthCode As String = "NGLWCFDEV",
                                            Optional ByVal sDTMSWCFURL As String = "http://nglwcfdev704.nextgeneration.com",
                                            Optional ByVal sDTMSWCFTCPURL As String = "net.tcp://nglwcfdev704.nextgeneration.com:908") As DAL.DataTransferObjects.DynamicsTMSSetting


        Try
            Dim oDTMSData As New DAL.DataTransferObjects.DynamicsTMSSetting With {.DTMSLegalEntity = sDTMSLegalEntity _
                                                             , .DTMSPicklistMaxRetry = iDTMSPicklistMaxRetry _
                                                             , .DTMSPicklistRetryMinutes = iDTMSPicklistRetryMinutes _
                                                             , .DTMSPicklistMaxRowsReturned = iDTMSPicklistMaxRowsReturned _
                                                             , .DTMSPicklistAutoConfirmation = bDTMSPicklistAutoConfirmation _
                                                             , .DTMSAPExportMaxRetry = iDTMSAPExportMaxRetry _
                                                             , .DTMSAPExportRetryMinutes = iDTMSAPExportRetryMinutes _
                                                             , .DTMSAPExportMaxRowsReturned = iDTMSAPExportMaxRowsReturned _
                                                             , .DTMSAPExportAutoConfirmation = bDTMSAPExportAutoConfirmation _
                                                             , .DTMSGPWebServiceURL = sDTMSGPWebServiceURL _
                                                             , .DTMSGPUserName = sDTMSGPUserName _
                                                             , .DTMSGPPassword = sDTMSGPPassword _
                                                             , .DTMSGPUseDefaultCredentials = bDTMSGPUseDefaultCredentials _
                                                             , .DTMSWSAuthCode = sDTMSWSAuthCode _
                                                             , .DTMSWSURL = sDTMSWSURL _
                                                             , .DTMSWCFAuthCode = sDTMSWCFAuthCode _
                                                             , .DTMSWCFURL = sDTMSWCFURL _
                                                             , .DTMSWCFTCPURL = sDTMSWCFTCPURL}

            Return NGLDynamicsTMSSettingData.CreateRecord(oDTMSData)
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            LogError(Source & " Warning:  Read Dynamics TMS Integration Settings Failed", ex.Detail.ToString(ex.Reason.ToString()), Me.AdminEmail)
        Catch ex As Exception
            LogError(Source & " Warning:  Read Dynamics TMS Integration Settings Failed", "Read Dynamics TMS Integration Settings Failed", Me.AdminEmail, ex)
        End Try
        Return Nothing
    End Function

    ' ''' <summary>
    ' ''' Deprecated method getSystemParameters we now use web services
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks>
    ' ''' Removed by RHR for v-7.0.5.102 on 11/14/2016
    ' '''  we now use web services
    ' ''' </remarks>
    'Protected Function getSystemParameters() As DAL.DataTransferObjects.GlobalTaskParameters
    '    Dim oGTPs As DAL.DataTransferObjects.GlobalTaskParameters
    '    Try
    '        'get the parameter settings from the database.
    '        oGTPs = NGLSystemDataProvider.GetGlobalTaskParameters()
    '    Catch ex As Exception
    '        Return Nothing
    '    End Try
    '    Return oGTPs
    'End Function

#End Region

#Region "Private Methods "


#End Region

    Public Enum MstrDataType
        Comp = 0
        Cust = 1
        Carrier = 2
    End Enum

    'Removed by RHR for v-7.0.5.102 on 11/14/2016
    '   Logic moved to caller
    'Private Function ConfigureInstance(ByVal sSource As String, _
    '                        ByVal DBName As String, _
    '                        ByVal DBServer As String, _
    '                        ByVal ConnectionSting As String, _
    '                        ByVal DBUser As String, _
    '                        ByVal DBPass As String, _
    '                        ByVal LegalEntity As String, _
    '                        ByVal Debug As Boolean, _
    '                        ByVal Verbos As Boolean) As Boolean


    '    Me.Database = DBName
    '    Me.DBServer = DBServer
    '    Me.Source = sSource
    '    Me.LegalEntity = LegalEntity
    '    Me.Debug = Debug
    '    Me.Verbose = Verbos

    '    If String.IsNullOrWhiteSpace(ConnectionSting) Then
    '        'build the connection string
    '        If String.IsNullOrWhiteSpace(DBUser) Or String.IsNullOrWhiteSpace(DBPass) Then
    '            ConnectionSting = String.Format("Data Source={0};Initial Catalog={1}; Integrated Security=SSPI;", DBServer, DBName)
    '        Else
    '            ConnectionSting = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", DBServer, DBName, DBUser, DBPass)
    '        End If
    '    End If
    '    Me.ConnectionString = ConnectionSting
    '    'open log and validate database
    '    If Not validateDatabase() Then Return False
    '    If Me.Verbose Then Me.Log(Source & " Running")



    '    If Not getTaskParameters() Then Return False
    '    fillConfig()
    '    Return True
    'End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="c"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 on 11/14/2016
    '''   method used to fill the global task parameters using web services.
    ''' </remarks>
    Private Function fillGlobalTaskParameters(ByVal c As clsDefaultIntegrationConfiguration)
        Dim blnRet As Boolean = False
        Try
            Dim oSettingObject As New tmsintegrationsettings.DTMSIntegration()
            oSettingObject.Url = c.TMSSettingsURI
            Dim ReturnMessage As String = ""
            Dim globalParameters As New tmsintegrationsettings.GlobalTaskParameters()
            globalParameters = oSettingObject.GetGlobalTaskParameters(c.TMSSettingsAuthCode, ReturnMessage)
            If Not String.IsNullOrWhiteSpace(ReturnMessage) Then Log(ReturnMessage)
            Dim skipObject As New List(Of String) From {""}
            ReturnMessage = ""
            Me.GlobalTaskParameters = DTran.CopyMatchingFields(Me.GlobalTaskParameters, globalParameters, skipObject, ReturnMessage)
            If Not String.IsNullOrWhiteSpace(ReturnMessage) Then Log(ReturnMessage)
            blnRet = True
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Read Global Task Parameter Settings Error", Source & " Unexpected Read Global Task Parameter Settings Error! Could not process any integration requests; the actual error is:  " & ex.Message, AdminEmail)
            'Throw
            blnRet = False
        End Try
        Return blnRet

    End Function

    Public Sub CleanUpTestData(ByVal LegalEntity As String)
        NGLBatchProcessDataProvider.UtilityRemoveAllTestDataByLegalEntity(LegalEntity)
    End Sub

    Public Function ProcessDataUnitTest(ByVal UnitTestKeys As clsUnitTestKeys,
                                        Optional ByVal DeleteOnFinally As Boolean = True,
                                        Optional ByVal Finalize As Boolean = True) As Boolean
        'Dim blnRet As Boolean = False
        'If UnitTestKeys Is Nothing OrElse String.IsNullOrWhiteSpace(UnitTestKeys.LegalEntity) Then
        '    LogError("Invalid Unit Test Keys")
        '    Return False
        'End If

        'Dim intCompControl As Integer
        'Dim intCarrierControl As Integer
        'Dim intLaneControl As Integer
        'Dim dblSampleFreightCost As Integer = 1000.0
        'If Not ConfigureInstance(UnitTestKeys.Source, UnitTestKeys.DBName, UnitTestKeys.DBServer, UnitTestKeys.ConnectionSting, UnitTestKeys.DBUser, UnitTestKeys.DBPass, UnitTestKeys.LegalEntity, UnitTestKeys.Debug, UnitTestKeys.Verbos) Then
        '    LogError("Invalid Instance Configuration")
        '    Return False
        'End If
        'Try
        '    Log("Begin Unit Test ")
        '    Dim Processed As New List(Of Integer)
        '    Dim Orders As New List(Of String)
        '    If Not processCompanyData(Nothing, Processed, UnitTestKeys) Then Return False
        '    If Not Processed Is Nothing AndAlso Processed.Count() > 0 Then
        '        intCompControl = Processed(0)
        '    End If
        '    Processed = New List(Of Integer)
        '    If Not processCarrierData(Nothing, Processed, UnitTestKeys) Then Return False
        '    If Not Processed Is Nothing AndAlso Processed.Count() > 0 Then
        '        intCarrierControl = Processed(0)
        '    End If
        '    If intCarrierControl = 0 Then
        '        LogError("Unit Test Get Carrier Failed", "Unit Test Failed to get a carrier control number back.", Me.AdminEmail)
        '        Throw New ApplicationException(Source & " Unit Test Get Carrier Failed")
        '    End If
        '    Log("Unit Test Configure Silent Tender Settings")
        '    NGLBatchProcessDataProvider.UtilityAllowSilentTender(UnitTestKeys.LegalEntity)
        '    Processed = New List(Of Integer)
        '    If Not processPalletTypeData(Nothing, UnitTestKeys) Then Return False
        '    If Not processHazmatData(Nothing, UnitTestKeys) Then Return False
        '    If Not processLaneData(Nothing, Processed, UnitTestKeys) Then Return False
        '    If Not Processed Is Nothing AndAlso Processed.Count() > 0 Then
        '        intLaneControl = Processed(0)
        '    End If
        '    Processed = New List(Of Integer)
        '    If Not processOrderData(Nothing, Orders, UnitTestKeys) Then Return False
        '    Log("Unit Test Do Spot Rate for Order Number: " & UnitTestKeys.OrderNumber)
        '    If Not NGLBookRevenueBLL.DoAutoSpotRateWithSave(UnitTestKeys.OrderNumber, 0, intCarrierControl, UnitTestKeys.FreightCost, Finalize) Then
        '        LogError("Unit Test Spot Rate Failed", "Unit Test Failed Cannot Apply Spot Rate ", Me.AdminEmail)
        '        Throw New ApplicationException(Source & " Unit Test Failed Cannot Apply Spot Rate")
        '    End If
        '    If Not processPicklistData(Nothing, UnitTestKeys) Then Return False
        '    If Finalize Then
        '        Log("Unit Test Mass Update ALL Freight Bills")
        '        Dim oFreightBills = NGLBatchProcessDataProvider.UtilityMassUpdateAllTestFreightBills(UnitTestKeys.LegalEntity)
        '        If Not processAPExportData(Nothing, UnitTestKeys) Then Return False
        '        If Not oFreightBills Is Nothing AndAlso oFreightBills.Count() > 0 Then
        '            For Each fb In oFreightBills
        '                Log("Unit Test Processing Payable for Freight Bill: " & fb.BookFinAPBillNumber)
        '                UnitTestKeys.FreightBillNumber = fb.BookFinAPBillNumber
        '                If Not processPayablesData(Nothing, UnitTestKeys) Then Return False
        '            Next
        '        ElseIf DeleteOnFinally = False Then
        '            Log("Unit Test No Freight Bills available for payable processing because DeleteOnFinally is off so previous tests may have already processed freight bills")
        '        Else
        '            LogError("Unit Test No Freight Bills available for payable processing.")
        '            Return False
        '        End If
        '    Else
        '        Log("Finalize is off No Freight Bill tests were run.")
        '    End If
        '    Log("Unit Test Complete")
        '    blnRet = True
        '    'TBD -- is this all of the errors? add additional error handlers as needed
        'Catch ex As Exception
        '    Throw
        'Finally
        '    Try
        '        If DeleteOnFinally Then CleanUpTestData(UnitTestKeys.LegalEntity)
        '    Catch ex As Exception
        '        System.Diagnostics.Debug.Assert(False, ex.Message)
        '    End Try
        '    Me.closeLog(0)
        'End Try
        'Return blnRet
#Disable Warning BC42353 ' Function 'ProcessDataUnitTest' doesn't return a value on all code paths. Are you missing a 'Return' statement?
    End Function
#Enable Warning BC42353 ' Function 'ProcessDataUnitTest' doesn't return a value on all code paths. Are you missing a 'Return' statement?



    ''' <summary>
    ''' The clsDefaultIntegrationConfiguration must have the following:
    ''' DBName
    ''' DBServer
    ''' TMSSettingsURI
    ''' TMSSettingsAuthCode
    ''' Typically db connections are managed by the web service but some features like
    ''' email and logging connect to the database directly in this case
    ''' if DBUser and DBPass are empty the system will use Integrated Security=SSPI 
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <remarks>
    ''' FUTURE:  we may want to add logic to log exceptions to the DTMS log table and allow the process to 
    ''' skip partial failures and continue.
    ''' </remarks>

    Private Sub updateERPTestingStatus(ByVal TMSSetting As tmsintegrationsettings.vERPIntegrationSetting)
        'set the defalt to false
        Dim ERPTestingOn As Boolean = False
        If TMSSetting Is Nothing Then Return
        Try
            If Me.Verbose Then Log("Begin Read ERP Testing Flag ")
            'Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            'oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPUser)) Then
                'oNAVWebService.UseDefaultCredentials = True
            Else
                'oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPUser, TMSSetting.ERPPassword)
            End If
            'ERPTestingOn = oNAVWebService.GetTestingStatus()
            If Me.Debug Then Log("Read ERP Testing Flag Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Read ERP Testing Flag Error", Source & " Unexpected Integration Error! Could not Read ERP Testing Flag information:  " & ex.Message, AdminEmail)
            Throw
        End Try
    End Sub



    Private Function getTMSIntegrationSettings(ByVal c As clsDefaultIntegrationConfiguration, ByRef oSettings As tmsintegrationsettings.vERPIntegrationSetting(), ByVal sLegal As String) As Boolean
        Dim blnRet As Boolean = False
        Try
            Dim oSettingObject As New tmsintegrationsettings.DTMSIntegration()
            oSettingObject.Url = c.TMSSettingsURI
            Dim ReturnMessage As String
            Dim ERPTypeName As String = "GP"
            Dim RetVal As Integer
#Disable Warning BC42030 ' Variable 'ReturnMessage' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            oSettings = oSettingObject.getvERPIntegrationSettingsByName(c.TMSSettingsAuthCode, sLegal, ERPTypeName, RetVal, ReturnMessage)
#Enable Warning BC42030 ' Variable 'ReturnMessage' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            If RetVal <> TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
                Select Case RetVal
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not read Integration Settings information:  " & ReturnMessage)
                        Return False
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        LogError("Read Integration Settings Error", "Error Data Validation Failure! could not read Integration Settings information:  " & ReturnMessage, AdminEmail)
                        Return False
                    Case Else
                        LogError("Read Integration Settings Error", "Error Integration Failure! could not read Integration Settings information:  " & ReturnMessage, AdminEmail)
                        Return False
                End Select
            Else
                blnRet = True
            End If

        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Read Integration Settings Error", Source & " Unexpected Read Integration Settings Error! Could not process any integration requests; the actual error is:  " & ex.Message, AdminEmail)
            'Throw
            blnRet = False
        End Try
        Return blnRet

    End Function

    Private Function getSpecificTMSSetting(ByVal n As String, ByRef a As tmsintegrationsettings.vERPIntegrationSetting(), ByRef s As tmsintegrationsettings.vERPIntegrationSetting) As tmsintegrationsettings.vERPIntegrationSetting
        If a Is Nothing OrElse a.Count() < 1 Then Return s
        If a.Any(Function(x) x.IntegrationTypeName = n) Then
            Return a.Where(Function(x) x.IntegrationTypeName = n).FirstOrDefault()
        Else
            Return s
        End If
    End Function


    
#Disable Warning BC42301 ' Only one XML comment block is allowed per language element.
''' <summary>
    ''' DBName and DBServer are required. 
    ''' If ConnectionString is empty the system will build the connection string based on DBUser and DBPass parameters. 
    ''' If DBUser and DBPass are empty the system will use  Integrated Security=SSPI in the connection string 
    ''' If the ConnectionString is empty
    ''' </summary>
    ''' <param name="DBName">Database-Catalog</param>
    ''' <param name="DBServer">Database Server</param>
    ''' <param name="ConnectionSting">Leave Blank to Build Dynamically</param>
    ''' <param name="DBUser">SQLServer Autentication User</param>
    ''' <param name="DBPass">SQLServer Autentication Password</param>
    ''' <param name="LegalEntity">Leave Blank to use all Legal Entities</param>
    ''' <param name="Debug">For Command Line Execution when run manually</param>
    ''' <param name="Verbos">Adds additional informaiton to logs and errors where available</param>
    ''' <remarks>
    ''' TBD -- get config parameters from TMSIntegration configuration service
    ''' TODO:  we must add logic to log exceptions to the DTMS log table and allow the process to 
    ''' skip partial failures and continue.  the current logic does not return the correct value
    ''' on failure or partial failure especiallay when we have multiple records in a batch.
    ''' </remarks>
    'Public Sub ProcessData(ByVal sSource As String, _
    'ByVal DBName As String, _
    'ByVal DBServer As String, _
    'Optional ByVal ConnectionSting As String = "", _
    'Optional ByVal DBUser As String = "", _
    'Optional ByVal DBPass As String = "", _
    'Optional ByVal LegalEntity As String = "", _
    'Optional ByVal Debug As Boolean = False, _
    'Optional ByVal Verbos As Boolean = False)
    ''' <summary>
    ''' Process All Integration Data
    ''' </summary>
    ''' <param name="sSource"></param>
    ''' <param name="c"></param>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' Modified by RHR for v-7.0.6.104  08/17/2017 
    ''' -- fixed bug where we only support two legal entities 
    ''' -- and where the GL Code dictionary update could fail if the legal entities do not match the number of GL codes exactly
    ''' </remarks>
    Public Sub ProcessData(ByVal sSource As String, ByVal c As clsDefaultIntegrationConfiguration)
#Enable Warning BC42301 ' Only one XML comment block is allowed per language element.

        Dim ErrorString As String = ""
        Dim DatabaseName As String = ""
        Dim CompanyGLAPDebit(10) As String
        Dim Counter As Int16 = 0


        If Not ConfigureInstance(sSource, c) Then Return
        'clear any existing dictErrorsToLogs
        dictErrorsToLog = Nothing
        Try
            If Me.Verbose Then Log("Begin Process Data ")
            'save the ERPFrieghtAccountIndex for later use
            ERPFrieghtAccountIndex = c.ERPFrieghtAccountIndex
            'get a list of nav settings
            Dim oConfig As New UserConfiguration()
            With oConfig
                .AdminEmail = Me.AdminEmail
                .AutoRetry = Me.AutoRetry
                .Database = Me.Database
                .DBServer = Me.DBServer
                .Debug = Me.Debug
                .FromEmail = Me.FromEmail
                .GroupEmail = Me.GroupEmail
                .LogFile = Me.LogFile
                .SMTPServer = Me.SMTPServer
                .UserName = "System Download"
                .WSAuthCode = "NGLSystem"
                .WCFAuthCode = "NGLSystem"
                .WCFURL = ""
                .WCFTCPURL = ""
                .ConnectionString = Me.ConnectionString

            End With

            Dim oSettings As tmsintegrationsettings.vERPIntegrationSetting()
            'Modified by RHR for v-7.0.6.103 on 3/30/2017
            'added logic for multiple legal entities using a | (pipe) delimiter
            Dim sConfigLegals As String() = c.TMSRunLegalEntity.Split("|")
            'Modified by SEM on 7/7/2017, added split of GL codes
            Dim DefaultGLCodes As String() = c.GPFunctionsDefaultFreightGLAccount.Split("|")

            'Modified by RHR for v-7.0.6.104  08/17/2017 
            '-- fixed bug where we only support two legal entities 
            '-- and where the GL Code dictionary update could fail if the legal entities do not match the number of GL codes exactly
            'Updated by SEM & RR on 7/12/2017, fixed counter not working issue
            Dim DGLCode As New Dictionary(Of String, String)
            'DGLCode.Add(sConfigLegals(0), DefaultGLCodes(0))
            'DGLCode.Add(sConfigLegals(1), DefaultGLCodes(1))

            If sConfigLegals Is Nothing OrElse sConfigLegals.Count() < 1 Then
                sConfigLegals = New String() {c.TMSRunLegalEntity}
            End If

            ' Use this as a sample for anytime an error thrown
            If (sConfigLegals.Count <> DefaultGLCodes.Count) Then
                Throw New System.ApplicationException("The Default GL Configuration is not valid.  The count of Default GL Codes does not match the count of Legal Entities")
            End If
            'Added by Rob & Scott 2017-08-19
            For i As Int16 = 0 To sConfigLegals.Count - 1
                If (Not DefaultGLCodes Is Nothing AndAlso DefaultGLCodes.Count > i) Then
                    DGLCode.Add(sConfigLegals(i), DefaultGLCodes(i))
                End If
            Next
            If Not sConfigLegals Is Nothing AndAlso sConfigLegals.Count() > 0 Then
                For Each sLegal In sConfigLegals

#Disable Warning BC42030 ' Variable 'oSettings' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
                    If Not getTMSIntegrationSettings(c, oSettings, sLegal) Then Return
#Enable Warning BC42030 ' Variable 'oSettings' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
                    If Not oSettings Is Nothing AndAlso oSettings.Count > 0 Then
                        'group the settings by Legal Entity
                        Dim sLegals As List(Of String) = oSettings.Select(Function(x) x.LegalEntity).Distinct().ToList()
                        If Not sLegals Is Nothing AndAlso sLegals.Count() > 0 Then
                            Counter = 0
                            For Each legal In sLegals

                                Dim lLegalSettings As tmsintegrationsettings.vERPIntegrationSetting() = oSettings.Where(Function(x) x.LegalEntity = legal).ToArray()
                                If Not lLegalSettings Is Nothing AndAlso lLegalSettings.Count() > 0 Then
                                    Me.LegalEntity = legal
                                    Dim Processed As New List(Of Integer)
                                    Dim Orders As New List(Of String)
                                    Dim StandardSetting = getSpecificTMSSetting("Standard", lLegalSettings, Nothing)
                                    'update the ERP Testing Status Flag using web services if available
                                    updateERPTestingStatus(getSpecificTMSSetting("ERPTestingStatus", lLegalSettings, StandardSetting))
                                    'processCompanyData(getSpecificTMSSetting("Company", lLegalSettings, StandardSetting), Processed)

                                    'add code if needed to do something with Processed list
                                    'Processed = New List(Of Integer)
                                    'processCarrierData(getSpecificTMSSetting("Carrier", lLegalSettings, StandardSetting), Processed)

                                    'add code if needed to do something with Processed list
                                    'Processed = New List(Of Integer)
                                    'processPalletTypeData(getSpecificTMSSetting("PalletType", lLegalSettings, StandardSetting))
                                    'processHazmatData(getSpecificTMSSetting("Hazmat", lLegalSettings, StandardSetting))
                                    'processLaneData(getSpecificTMSSetting("Lane", lLegalSettings, StandardSetting), Processed)
                                    'Processed = New List(Of Integer)
                                    'processOrderData(getSpecificTMSSetting("Order", lLegalSettings, StandardSetting), getSpecificTMSSetting("Lane", lLegalSettings, StandardSetting), Orders)

                                    'Orders = New List(Of String)
                                    'processPicklistData(getSpecificTMSSetting("PickList", lLegalSettings, StandardSetting))

                                    'processAPExportData(getSpecificTMSSetting("APExport", lLegalSettings, StandardSetting))
                                    'processPayablesData(getSpecificTMSSetting("Payables", lLegalSettings, StandardSetting))

                                    ' Declare Variables
                                    Dim EConnectStr As String = ""
                                    Dim TimeToRun As Boolean = True
                                    Dim GPCompany As New GPDataIntegrationSTructure.GPCompanies
                                    Dim SuccessFlag As Boolean = True
                                    Dim GetFunctions As New GPFunctions(c) 'Modified by RHR for v-7.0.5.102 10/14/2016

                                    GPCompany.GPDatabase = Me.LegalEntity.ToString

                                    'processPayablesData(getSpecificTMSSetting("Payables", lLegalSettings, StandardSetting))
                                    Dim TMSSetting = getSpecificTMSSetting("Payables", lLegalSettings, StandardSetting)

                                    If Not String.IsNullOrWhiteSpace(TMSSetting.ERPUser) AndAlso Not String.IsNullOrWhiteSpace(TMSSetting.ERPPassword) Then
                                        EConnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & sLegal.ToString & ";User ID=" & TMSSetting.ERPUser & ";Password=" & TMSSetting.ERPPassword
                                    Else
                                        EConnectStr = "Data Source=" & TMSSetting.ERPCertificate & ";Initial Catalog=" & sLegal.ToString & ";Integrated Security=SSPI;Trusted_Connection=True"
                                    End If


                                    ' Need to work with Rob to set ERP server
                                    'GPDynamics(c.TMSDBServer.ToString, Me.LegalEntity.ToString)
                                    'SuccessFlag = GPDynamics("NGLGP2013R2", GPCompany)


                                    Dim OrderCheckSetting As New tmsintegrationsettings.vERPIntegrationSetting

                                    OrderCheckSetting = getSpecificTMSSetting("Orders", lLegalSettings, StandardSetting)

                                    If (Not String.IsNullOrWhiteSpace(OrderCheckSetting.LegalEntity)) Then
                                        SuccessFlag = GPDynamics(getSpecificTMSSetting("Orders", lLegalSettings, StandardSetting), c, Me.AdminEmail)

                                        If (SuccessFlag) Then

                                            If Me.Verbose Then Log("Success! The SOP Orders were processed.")

                                        Else

                                            If Me.Verbose Then Log("Errors! The SOP Order processing completed, but with Errors.")

                                        End If

                                    End If

                                    OrderCheckSetting = Nothing


                                    If (Not c.ERPPayablesLastRunDate = Date.MinValue AndAlso (c.ERPPayablesLastRunDate < Date.Now.ToShortDateString)) Then

                                        SuccessFlag = GetFunctions.InsertUserLink(EConnectStr, getSpecificTMSSetting("DynamicsTMSLink", lLegalSettings, StandardSetting))

                                        SuccessFlag = GetFunctions.InsertUserLink(EConnectStr, getSpecificTMSSetting("SpotRateLink", lLegalSettings, StandardSetting))
                                        'Modified by RHR for v-7.0.5.102 on 11/11/2016
                                        '  We have moved this logic into two new functions
                                        '   processAPExportData for web services and
                                        '   processAPExportDataDLLDirect to call the DLL directly (may be used for testing)

                                        'Modifed by SEM on 7/7/2017, added company GL default code
                                        ' Update GP with Cost allocation for sales order
                                        'Call GetAPData70 will have cost by order
                                        'Dim FBCostSplit = processAPExportData(getSpecificTMSSetting("APExport", lLegalSettings, StandardSetting), c, DGLCode(sLegal))
                                        If (c.GPFunctionSOPCostUpdateOn) Then
                                            'TODO:  clean this up we need to modify how this procedure works 
                                            'Dim FBCostSplit = processFreightBillCosts(getSpecificTMSSetting("APExport", lLegalSettings, StandardSetting), c, DGLCode(sLegal))

                                        End If

                                        Dim blnAPExported = processAPExportData(getSpecificTMSSetting("APExport", lLegalSettings, StandardSetting), c, DGLCode(sLegal))

                                        Dim blnPayablesImported = processPayablesData(getSpecificTMSSetting("Payables", lLegalSettings, StandardSetting), c, EConnectStr)

                                        ' Have Rob follow

                                        Dim objGPFunctions As New GPFunctions(c)

                                        'Need to get the value:  

                                        If (c.GPFunctionShipConfirmationOn) Then

                                            Dim blnShipConfirmation = processSOPShipConfirmation(getSpecificTMSSetting("ShipConfirmed", lLegalSettings, StandardSetting), c, EConnectStr)

                                        End If

                                    End If

                                End If

                            Next
                        End If
                    End If

                Next
            End If
            If Me.Verbose Then Log("Process Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex1 As System.ApplicationException
            LogError(Source & " Error!  Application Error in GP Process Data", Source & " : " & ex1.Message, AdminEmail)
            Throw
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected GP Process Data Error", Source & " Could not process any integration requests; the actual error is:  " & ex.Message, AdminEmail)
            Throw
        Finally
            LogAllErrors()
            Me.closeLog(0)
        End Try

    End Sub

    Public Sub fillConfig()
        Try

            With oConfig
                .AdminEmail = Me.AdminEmail
                .AutoRetry = Me.AutoRetry
                .ConnectionString = ConnectionString
                .Database = Me.Database
                .DBServer = Me.DBServer
                .Debug = Me.Debug
                .FromEmail = Me.FromEmail
                .GroupEmail = Me.GroupEmail
                .INIKey = Me.INIKey
                .KeepLogDays = Me.KeepLogDays
                .ResultsFile = Me.ResultsFile
                .LogFile = Me.LogFile
                .SaveOldLog = Me.SaveOldLog
                .SMTPServer = Me.SMTPServer
                .Source = Me.Source
            End With

        Catch ex As Exception
            Throw New ApplicationException(Source & " Fill Configuration Failure", ex)
        End Try
    End Sub


    ' TBD move to a common class versus re-writing it for each integration
    Private Function CopyMatchingFields(toObj As [Object], fromObj As [Object], ByVal skipObjs As List(Of String)) As Object
        If toObj Is Nothing Or fromObj Is Nothing Then
            Return Nothing
        End If

        Dim fromType As Type = fromObj.[GetType]()
        Dim toType As Type = toObj.[GetType]()

        ' Get all FieldInfo. 
        Dim fProps As PropertyInfo() = fromType.GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)
        Dim tProps As PropertyInfo() = toType.GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)
        For Each fProp As PropertyInfo In fProps
            Dim propValue As Object = fProp.GetValue(fromObj)
            'Removed by RHR 10/8/14 did not update nullable fields when null
            'If propValue IsNot Nothing Then
            If Not skipObjs.Contains(fProp.Name) Then
                For Each tProp In tProps
                    If tProp.Name = fProp.Name Then
                        If tProp.PropertyType() = fProp.PropertyType() Then
                            Try
                                tProp.SetValue(toObj, propValue)
                            Catch ex As Exception
                                Dim strMsg As String = ex.Message
                                Throw
                            End Try
                        End If
                        Exit For
                    End If
                Next
            End If
            'End If
        Next
        Return toObj

    End Function


    ' TBD -- move to a common class versus re-writing it for each integration
    Private Function CopyMatchingFieldsImplicitCast(toObj As [Object], fromObj As [Object], ByVal skipObjs As List(Of String), ByRef strMsg As String) As Object
        If toObj Is Nothing Or fromObj Is Nothing Then
            Return Nothing
        End If
        'primatives used for casting
        Dim iVal16 As Int16 = 0
        Dim iVal32 As Int32 = 0
        Dim iVal64 As Int64 = 0
        Dim dblVal As Double = 0
        Dim decVal As Decimal = 0
        Dim dtVal As Date = Date.Now()
        Dim blnVal As Boolean = False
        Dim intVal As Integer = 0

        Dim fromType As Type = fromObj.[GetType]()
        Dim toType As Type = toObj.[GetType]()

        ' Get all FieldInfo. 
        Dim fProps As PropertyInfo() = fromType.GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)
        Dim tProps As PropertyInfo() = toType.GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)
        If Me.Debug Then
            System.Diagnostics.Debug.WriteLine("")
            System.Diagnostics.Debug.WriteLine("*******************************************************")
            System.Diagnostics.Debug.WriteLine("")
        End If
        For Each fProp As PropertyInfo In fProps
            Dim propValue As Object = fProp.GetValue(fromObj)
            'Removed by RHR 10/8/14 did not update nullable fields when null
            'If propValue IsNot Nothing Then
            If Me.Debug Then System.Diagnostics.Debug.WriteLine(fProp.Name & ": " & propValue.ToString())
            If Not skipObjs.Contains(fProp.Name) Then
                For Each tProp In tProps
                    If tProp.Name = fProp.Name Then
                        If tProp.PropertyType() = fProp.PropertyType() Then
                            Try
                                tProp.SetValue(toObj, propValue)
                            Catch ex As Exception
                                strMsg &= ex.Message
                                Throw
                            End Try
                        Else
                            Try
                                Select Case tProp.PropertyType.Name
                                    Case "String"
                                        tProp.SetValue(toObj, propValue.ToString())
                                    Case "Int16"
                                        If Int16.TryParse(propValue.ToString(), iVal16) Then
                                            tProp.SetValue(toObj, iVal16)
                                        End If
                                    Case "Int32"
                                        If Int32.TryParse(propValue.ToString(), iVal32) Then
                                            tProp.SetValue(toObj, iVal32)
                                        End If
                                    Case "Int64"
                                        If Int32.TryParse(propValue.ToString(), iVal64) Then
                                            tProp.SetValue(toObj, iVal64)
                                        End If
                                    Case "Date"
                                        If Date.TryParse(propValue.ToString(), dtVal) Then
                                            tProp.SetValue(toObj, dtVal)
                                        End If
                                    Case "DateTime"
                                        If Date.TryParse(propValue.ToString(), dtVal) Then
                                            tProp.SetValue(toObj, dtVal)
                                        End If
                                    Case "Decimal"
                                        If Decimal.TryParse(propValue.ToString(), decVal) Then
                                            tProp.SetValue(toObj, decVal)
                                        End If
                                    Case "Double"
                                        If Double.TryParse(propValue.ToString(), dblVal) Then
                                            tProp.SetValue(toObj, dblVal)
                                        End If
                                    Case "Boolean"
                                        If Boolean.TryParse(propValue.ToString(), blnVal) Then
                                            tProp.SetValue(toObj, blnVal)
                                        Else
                                            'try to convert to an integer and then test for 0 any non zero is true
                                            If Integer.TryParse(propValue.ToString(), intVal) Then
                                                If intVal = 0 Then
                                                    blnVal = False
                                                Else
                                                    blnVal = True
                                                End If
                                                tProp.SetValue(toObj, blnVal)
                                            End If
                                        End If
                                    Case Else
                                        'cannot parse
                                        Dim s As String = ""
                                        If propValue IsNot Nothing Then s = propValue.ToString
                                        strMsg &= " Cannot Copy " & fProp.Name & " invalid type " & s
                                End Select
                            Catch ex As Exception
                                strMsg &= ex.Message
                                Throw
                            End Try
                        End If
                        Exit For
                    End If
                Next
            End If
            'End If
        Next
        If Me.Debug Then
            System.Diagnostics.Debug.WriteLine("")
            System.Diagnostics.Debug.WriteLine("*******************************************************")
            System.Diagnostics.Debug.WriteLine("")
        End If
        Return toObj

    End Function


    ' TBD -- move to a common class versus re-writing it for each integration
    Private Sub populateIntegrationObjectParameters(ByRef oImportExport As TMS.clsImportExport, ByVal GPSettings As DAL.DataTransferObjects.DynamicsTMSSetting, Optional ByVal UnitTestKeys As clsUnitTestKeys = Nothing)

        Dim connectionString As String = Me.ConnectionString
        With oImportExport
            .AdminEmail = Me.AdminEmail
            .FromEmail = Me.FromEmail
            .GroupEmail = Me.GroupEmail
            .Retry = Me.AutoRetry
            .SMTPServer = Me.SMTPServer
            .DBServer = Me.DBServer
            .Database = Me.Database
            .ConnectionString = connectionString
            .Debug = Me.Debug
            If UnitTestKeys Is Nothing Then
                If Not GPSettings Is Nothing Then
                    .AuthorizationCode = GPSettings.DTMSWSAuthCode
                    .WCFAuthCode = GPSettings.DTMSWCFAuthCode
                    .WCFURL = GPSettings.DTMSWCFURL
                    .WCFTCPURL = GPSettings.DTMSWCFTCPURL
                End If
            Else
                .AuthorizationCode = UnitTestKeys.WSAuthCode
                .WCFAuthCode = UnitTestKeys.WCFAuthCode
                .WCFURL = UnitTestKeys.WCFURL
                .WCFTCPURL = UnitTestKeys.WCFTCPURL
            End If
        End With

    End Sub


    ''' <summary>
    ''' TODO:  we should not be calling this code because access to the database from the GP Integration service may not be available on remote systems
    ''' </summary>
    ''' <param name="sSource"></param>
    ''' <param name="c"></param>
    ''' <returns></returns>
    Private Function ConfigureInstance(sSource As String, c As FM.GPIntegration.clsDefaultIntegrationConfiguration) As Boolean

        Me.Database = c.TMSDBName
        Me.DBServer = c.TMSDBServer
        Me.Source = sSource
        Me.LegalEntity = c.TMSRunLegalEntity
        Me.Debug = c.Debug
        Me.Verbose = c.Verbos

        'build the connection string
        If String.IsNullOrWhiteSpace(c.TMSDBUser) Or String.IsNullOrWhiteSpace(c.TMSDBPass) Then
            Me.ConnectionString = String.Format("Data Source={0};Initial Catalog={1}; Integrated Security=SSPI;", Me.DBServer, Me.Database)
        Else
            Me.ConnectionString = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", DBServer, Me.Database, c.TMSDBUser, c.TMSDBPass)
        End If
        'open log and validate database
        'Modified by RHR for v-7.0.5.102 on 11/14/2016
        '  we do not validate the data base we now use web serivces
        '  we override the validateDatabase method and only create the log file
        If Not validateDatabase() Then Return False
        If Me.Verbose Then Me.Log(Source & " Running")
        'read the global task parameters using web services
        If Not Me.fillGlobalTaskParameters(c) Then Return False
        'populate the local properties using the Global Task Parameter object
        If Not getTaskParameters() Then Return False
        'fill the local config object with local properties.
        fillConfig()
        Return True
    End Function


End Class




#If StandAlone = True Then
Namespace NGL.FM.GPIntegration.svcDynamicsERP
    Class DynamicsTMSPicks

        Property Pick As Object

    End Class
    Class DynamicsTMSLanes

        Property Lane As Object

    End Class
    Class DynamicsTMSCarriers

        Property Carrier As Object

    End Class
    Class DynamicsTMSBookings

        Property Booking As Object

        Sub GetBookings(oGPOrders As NGL.FM.GPIntegration.svcDynamicsERP.DynamicsTMSBookings, p2 As Boolean, p3 As Boolean)
            Throw New NotImplementedException
        End Sub

    End Class
    Class DynamicsTMSAP

    End Class





    Public Class DynamicsTMSWebServices
        Public Url As String

        Property UseDefaultCredentials As Boolean

        Property GetTestingStatus As Boolean

        'Sub GetLanes(oGPLanes As NGL.FM.GPIntegration.svcDynamicsERP.DynamicsTMSLanes, p2 As Boolean, p3 As Boolean)
        '    Throw New NotImplementedException
        'End Sub

        'Sub SendPicks(oGPPicklist As NGL.FM.GPIntegration.NGL.FM.GPIntegration.svcDynamicsERP.DynamicsTMSPicks)
        '    Throw New NotImplementedException
        'End Sub

    End Class
End Namespace
#End If


