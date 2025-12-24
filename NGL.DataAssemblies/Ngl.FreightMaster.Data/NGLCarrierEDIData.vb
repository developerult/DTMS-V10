Imports System.ServiceModel

Public Class NGLCarrierEDIData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters

        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierEDIs
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierEDIData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierEDIs
                _LinqDB = db
            End If

            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetCarrierEDIFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrierEDIsFiltered()
    End Function

    Public Function GetCarrierEDIFiltered(ByVal Control As Integer) As DataTransferObjects.CarrierEDI
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim CarrierEDI As DataTransferObjects.CarrierEDI = (
                        From d In db.CarrierEDIs
                        Where
                        d.CarrierEDIControl = Control
                        Select New DataTransferObjects.CarrierEDI With {.CarrierEDIControl = d.CarrierEDIControl,
                        .CarrierEDICarrierControl = d.CarrierEDICarrierControl,
                        .CarrierEDIXaction = d.CarrierEDIXaction,
                        .CarrierEDIComment = d.CarrierEDIComment,
                        .CarrierEDISecurityQual = d.CarrierEDISecurityQual,
                        .CarrierEDISecurityCode = d.CarrierEDISecurityCode,
                        .CarrierEDIPartnerQual = d.CarrierEDIPartnerQual,
                        .CarrierEDIPartnerCode = d.CarrierEDIPartnerCode,
                        .CarrierEDIISASequence = d.CarrierEDIISASequence,
                        .CarrierEDIGSSequence = d.CarrierEDIGSSequence,
                        .CarrierEDIEmailNotificationOn = d.CarrierEDIEmailNotificationOn,
                        .CarrierEDIEmailAddress = d.CarrierEDIEmailAddress,
                        .CarrierEDIAcknowledgementRequested = d.CarrierEDIAcknowledgementRequested,
                        .CarrierEDIAcceptOn997 = d.CarrierEDIAcceptOn997,
                        .CarrierEDITestCode = d.CarrierEDITestCode,
                        .CarrierEDIUpdated = d.CarrierEDIUpdated.ToArray(),
                        .CarrierEDIInboundFolder = d.CarrierEDIInboundFolder,
                        .CarrierEDIBackupFolder = d.CarrierEDIBackupFolder,
                        .CarrierEDILogFile = d.CarrierEDILogFile,
                        .CarrierEDIStartTime = d.CarrierEDIStartTime,
                        .CarrierEDIEndTime = d.CarrierEDIEndTime,
                        .CarrierEDIDaysOfWeek = d.CarrierEDIDaysOfWeek,
                        .CarrierEDISendMinutesOutbound = d.CarrierEDISendMinutesOutbound,
                        .CarrierEDIFileNameBaseOutbound = d.CarrierEDIFileNameBaseOutbound,
                        .CarrierEDIFileNameBaseInbound = d.CarrierEDIFileNameBaseInbound,
                        .CarrierEDIWebServiceAuthKey = d.CarrierEDIWebServiceAuthKey,
                        .CarrierEDINGLEDIInputWebURL = d.CarrierEDINGLEDIInputWebURL,
                        .CarrierEDINGLEDI204OutputWebURL = d.CarrierEDINGLEDI204OutputWebURL,
                        .CarrierEDIOutboundFolder = d.CarrierEDIOutboundFolder,
                        .CarrierEDILastOutboundTransmission = d.CarrierEDILastOutboundTransmission,
                        .CarrierEDIFTPOutboundFolder = d.CarrierEDIFTPOutboundFolder,
                        .CarrierEDIFTPBackupFolder = d.CarrierEDIFTPBackupFolder,
                        .CarrierEDIFTPInboundFolder = d.CarrierEDIFTPInboundFolder,
                        .CarrierEDIFTPServer = d.CarrierEDIFTPServer,
                        .CarrierEDIFTPUserName = d.CarrierEDIFTPUserName,
                        .CarrierEDIFTPPassword = d.CarrierEDIFTPPassword,
                        .CarrierEDIWebServiceURL = d.CarrierEDIWebServiceURL}).First


                Return CarrierEDI

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrierEDIsFiltered(Optional ByVal CarrierControl As Integer = 0, Optional ByVal EDIXaction As String = "") As DataTransferObjects.CarrierEDI()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Return all the contacts that match the criteria sorted by name
                Dim CarrierEDIs() As DataTransferObjects.CarrierEDI = (
                        From d In db.CarrierEDIs
                        Where
                        (d.CarrierEDICarrierControl = If(CarrierControl = 0, d.CarrierEDICarrierControl, CarrierControl)) _
                        And
                        (EDIXaction Is Nothing OrElse String.IsNullOrEmpty(EDIXaction) OrElse d.CarrierEDIXaction = EDIXaction)
                        Order By d.CarrierEDIControl
                        Select New DataTransferObjects.CarrierEDI With {.CarrierEDIControl = d.CarrierEDIControl,
                        .CarrierEDICarrierControl = d.CarrierEDICarrierControl,
                        .CarrierEDIXaction = d.CarrierEDIXaction,
                        .CarrierEDIComment = d.CarrierEDIComment,
                        .CarrierEDISecurityQual = d.CarrierEDISecurityQual,
                        .CarrierEDISecurityCode = d.CarrierEDISecurityCode,
                        .CarrierEDIPartnerQual = d.CarrierEDIPartnerQual,
                        .CarrierEDIPartnerCode = d.CarrierEDIPartnerCode,
                        .CarrierEDIISASequence = d.CarrierEDIISASequence,
                        .CarrierEDIGSSequence = d.CarrierEDIGSSequence,
                        .CarrierEDIEmailNotificationOn = d.CarrierEDIEmailNotificationOn,
                        .CarrierEDIEmailAddress = d.CarrierEDIEmailAddress,
                        .CarrierEDIAcknowledgementRequested = d.CarrierEDIAcknowledgementRequested,
                        .CarrierEDIAcceptOn997 = d.CarrierEDIAcceptOn997,
                        .CarrierEDITestCode = d.CarrierEDITestCode,
                        .CarrierEDIUpdated = d.CarrierEDIUpdated.ToArray(),
                        .CarrierEDIInboundFolder = d.CarrierEDIInboundFolder,
                        .CarrierEDIBackupFolder = d.CarrierEDIBackupFolder,
                        .CarrierEDILogFile = d.CarrierEDILogFile,
                        .CarrierEDIStartTime = d.CarrierEDIStartTime,
                        .CarrierEDIEndTime = d.CarrierEDIEndTime,
                        .CarrierEDIDaysOfWeek = d.CarrierEDIDaysOfWeek,
                        .CarrierEDISendMinutesOutbound = d.CarrierEDISendMinutesOutbound,
                        .CarrierEDIFileNameBaseOutbound = d.CarrierEDIFileNameBaseOutbound,
                        .CarrierEDIFileNameBaseInbound = d.CarrierEDIFileNameBaseInbound,
                        .CarrierEDIWebServiceAuthKey = d.CarrierEDIWebServiceAuthKey,
                        .CarrierEDINGLEDIInputWebURL = d.CarrierEDINGLEDIInputWebURL,
                        .CarrierEDINGLEDI204OutputWebURL = d.CarrierEDINGLEDI204OutputWebURL,
                        .CarrierEDIOutboundFolder = d.CarrierEDIOutboundFolder,
                        .CarrierEDILastOutboundTransmission = d.CarrierEDILastOutboundTransmission,
                        .CarrierEDIFTPOutboundFolder = d.CarrierEDIFTPOutboundFolder,
                        .CarrierEDIFTPBackupFolder = d.CarrierEDIFTPBackupFolder,
                        .CarrierEDIFTPInboundFolder = d.CarrierEDIFTPInboundFolder,
                        .CarrierEDIFTPServer = d.CarrierEDIFTPServer,
                        .CarrierEDIFTPUserName = d.CarrierEDIFTPUserName,
                        .CarrierEDIFTPPassword = d.CarrierEDIFTPPassword,
                        .CarrierEDIWebServiceURL = d.CarrierEDIWebServiceURL}).ToArray()
                Return CarrierEDIs

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrierEDIOption(ByVal CarrierControl As Integer, ByVal EDIXaction As String) As Boolean
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CarrierEDI As DataTransferObjects.CarrierEDI = (
                        From d In db.CarrierEDIs
                        Where
                        d.CarrierEDICarrierControl = CarrierControl _
                        And
                        d.CarrierEDIXaction = EDIXaction
                        Select New DataTransferObjects.CarrierEDI With {.CarrierEDIControl = d.CarrierEDIControl}).First
                'If we get here then a record exists for the desired action so return true
                Return True

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                'this is the result when not data is found so we just ignore the error
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            'we return false by default
            Return False

        End Using
    End Function



    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="PartnerCode"></param>
    ''' <param name="EDIXaction"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    ''' </remarks>
    Public Function getCarrierNameByPartnerCode(ByVal PartnerCode As String, ByVal EDIXaction As String) As String
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Dim carrName As String = ""
            Try
                'Get the newest record that matches the provided criteria
                carrName = (
                    From d In db.Carriers
                        Join c In db.CarrierEDIs
                            On d.CarrierControl Equals c.CarrierEDICarrierControl
                        Where
                            c.CarrierEDIPartnerCode = PartnerCode _
                            And
                            c.CarrierEDIXaction = EDIXaction
                        Select d.CarrierName).FirstOrDefault

                Return carrName

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                'this is the result when not data is found so we just ignore the error
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return carrName

        End Using
    End Function


    Public Function GetCarrierEDIByPartnerCode(ByVal PartnerCode As String, ByVal EDIXaction As String) As LTS.CarrierEDI
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Dim carrName As String = ""
            Try
                Return db.CarrierEDIs.Where(Function(x) x.CarrierEDIPartnerCode = PartnerCode AndAlso x.CarrierEDIXaction = EDIXaction).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierEDIByPartnerCode"), db)
            End Try
            Return Nothing
        End Using
    End Function


#End Region

#Region "Carrier EDI LTSMethods"
    ''' <summary>
    ''' Method to Get the CarrierEDI's  based on Selected Carrier
    ''' </summary>
    ''' <param name="filters">CarrierEDICarrierControl</param>
    ''' <param name="RecordCount">Result</param>
    ''' <returns>LTS.vCarrierEDI</returns>
    ''' <remarks>
    ''' Added For Carrier Data Migration Changes By ManoRama ON 19-AUG-2020
    ''' </remarks>
    Public Function GetCarrierEDIs(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCarrierEDI()
        If filters Is Nothing Then Return Nothing
        Dim iLECarControl As Integer
        Dim iCarrierControl As Integer
        Dim oRet() As LTS.vCarrierEDI

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                If filters.ParentControl = 0 Then
                    Dim sMsg As String = " The Carrier ID is missing. Please return to the Carrier  page and select a valid carrier contract."
                    throwNoDataFaultException(sMsg)
                End If

                ' need to lookup carrier control number from tblLegalEntityCarriers
                iLECarControl = filters.ParentControl
                iCarrierControl = db.tblLegalEntityCarriers.Where(Function(x) x.LECarControl = iLECarControl).Select(Function(x) x.LECarCarrierControl).FirstOrDefault()

                If iCarrierControl = 0 Then
                    throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
                End If

                Dim iQuery As IQueryable(Of LTS.vCarrierEDI)
                iQuery = db.vCarrierEDIs
                Dim filterWhere As String = " (CarrierEDICarrierControl = " & iCarrierControl & ") "
                Dim sFilterSpacer As String = " And "

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrierEDIXaction"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierEDIs"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Method to save/Insert Carrier EDI in CarrierEDI table.
    ''' </summary>
    ''' <param name="carrier">LTS.CarrierEDI</param>
    ''' <returns>True/False</returns>
    ''' <remarks>
    ''' Added For Carrier Data Migration By ManoRama On 20-AUG-2020
    ''' </remarks>
    Public Function SaveCarrierEDI(ByVal carrier As LTS.CarrierEDI) As Boolean
        Dim blnRet As Boolean = False
        If carrier Is Nothing Then Return False 'nothing to do
        Dim iLECarControl As Integer
        Dim iCarrierControl As Integer
        Dim blnNewCarrier As Boolean = False
        Dim oNew As New LTS.CarrierEDI()
        Dim oNew1 As New DataTransferObjects.CarrierEDI()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'verify the Carrier contract

                If carrier.CarrierEDICarrierControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier ID Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False

                    ' need to lookup carrier control number from tblLegalEntityCarriers
                    iLECarControl = carrier.CarrierEDICarrierControl
                    iCarrierControl = db.tblLegalEntityCarriers.Where(Function(x) x.LECarControl = iLECarControl).Select(Function(x) x.LECarCarrierControl).FirstOrDefault()

                    If iCarrierControl = 0 Then
                        throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
                    Else
                        carrier.CarrierEDICarrierControl = iCarrierControl
                    End If
                End If
                If carrier.CarrierEDIControl = 0 Then

                    ' need to lookup carrier control number from tblLegalEntityCarriers
                    iLECarControl = carrier.CarrierEDICarrierControl
                    iCarrierControl = db.tblLegalEntityCarriers.Where(Function(x) x.LECarControl = iLECarControl).Select(Function(x) x.LECarCarrierControl).FirstOrDefault()

                    If iCarrierControl = 0 Then
                        throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
                    Else
                        carrier.CarrierEDICarrierControl = iCarrierControl
                    End If

                    oNew1 = selectDTOData(carrier)
                    ValidateNewRecord(db, oNew1)
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrierEDICarrierControl", "rowguid", "CarrierEDIModDate", "CarrierEDIModUser", "CarrierEDIUpdated", "Carrier"}
                    oNew = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oNew, carrier, skipObjs, strMSG)
                    With oNew
                        .CarrierEDIModDate = Date.Now
                        .CarrierEDIModUser = Me.Parameters.UserName
                        .CarrierEDICarrierControl = iCarrierControl
                    End With
                    If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                        System.Diagnostics.Debug.WriteLine(strMSG)
                    End If
                    db.CarrierEDIs.InsertOnSubmit(oNew)
                    blnNewCarrier = True
                Else
                    oNew1 = selectDTOData(carrier)
                    ValidateUpdatedRecord(db, oNew1)
                    Dim oExisting = db.CarrierEDIs.Where(Function(x) x.CarrierEDIControl = carrier.CarrierEDIControl).FirstOrDefault()
                    If oExisting Is Nothing OrElse oExisting.CarrierEDIControl = 0 Then
                        throwRecordDeletedFaultException("Cannot save changes for CarrierEDI: " & carrier.CarrierEDIXaction)
                    End If
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrierEDIModUser", "CarrierEDIModDate", "Carrier", "CarrierEDIControl", "CarrierEDIUpdated"}
                    oExisting = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oExisting, carrier, skipObjs, strMSG)
                    With oExisting
                        .CarrierEDIModDate = Date.Now
                        .CarrierEDIModUser = Me.Parameters.UserName
                    End With
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCarrierEDI"), db)
            End Try
        End Using

        Return blnRet
    End Function

    ''' <summary>
    ''' Method to Delete the CarrierEDI based on CarrierEDI PK
    ''' </summary>
    ''' <param name="iControl">CarrierEDI PK</param>
    ''' <returns>true/False</returns>
    ''' <remarks>
    ''' Added for Carrier Data Migration By ManoRama on 20-Aug-2020
    ''' </remarks>
    Public Function DeleteCarrierEDI(ByVal iControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iControl = 0 Then Return False 'nothing to do
        'validate before delete

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Delte the Record
                Dim oToDelete = db.CarrierEDIs.Where(Function(x) x.CarrierEDIControl = iControl).FirstOrDefault()
                If oToDelete Is Nothing OrElse oToDelete.CarrierEDICarrierControl = 0 Then Return True 'already deleted
                db.CarrierEDIs.DeleteOnSubmit(oToDelete)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrierEDI"), db)
            End Try
        End Using
        Return blnRet
    End Function
#End Region


#Region "Protected Functions"
    Protected Friend Function selectDTOData(ByVal d As LTS.CarrierEDI, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrierEDI
        Return New DataTransferObjects.CarrierEDI With {.CarrierEDIControl = d.CarrierEDIControl,
            .CarrierEDICarrierControl = d.CarrierEDICarrierControl,
            .CarrierEDIXaction = d.CarrierEDIXaction,
            .CarrierEDIComment = d.CarrierEDIComment,
            .CarrierEDISecurityQual = d.CarrierEDISecurityQual,
            .CarrierEDISecurityCode = d.CarrierEDISecurityCode,
            .CarrierEDIPartnerQual = d.CarrierEDIPartnerQual,
            .CarrierEDIPartnerCode = d.CarrierEDIPartnerCode,
            .CarrierEDIISASequence = d.CarrierEDIISASequence,
            .CarrierEDIGSSequence = d.CarrierEDIGSSequence,
            .CarrierEDIEmailNotificationOn = d.CarrierEDIEmailNotificationOn,
            .CarrierEDIEmailAddress = d.CarrierEDIEmailAddress,
            .CarrierEDIAcknowledgementRequested = d.CarrierEDIAcknowledgementRequested,
            .CarrierEDIAcceptOn997 = d.CarrierEDIAcceptOn997,
            .CarrierEDITestCode = d.CarrierEDITestCode,
            .CarrierEDIUpdated = d.CarrierEDIUpdated.ToArray(),
            .CarrierEDIInboundFolder = d.CarrierEDIInboundFolder,
            .CarrierEDIBackupFolder = d.CarrierEDIBackupFolder,
            .CarrierEDILogFile = d.CarrierEDILogFile,
            .CarrierEDIStartTime = d.CarrierEDIStartTime,
            .CarrierEDIEndTime = d.CarrierEDIEndTime,
            .CarrierEDIDaysOfWeek = d.CarrierEDIDaysOfWeek,
            .CarrierEDISendMinutesOutbound = d.CarrierEDISendMinutesOutbound,
            .CarrierEDIFileNameBaseOutbound = d.CarrierEDIFileNameBaseOutbound,
            .CarrierEDIFileNameBaseInbound = d.CarrierEDIFileNameBaseInbound,
            .CarrierEDIWebServiceAuthKey = d.CarrierEDIWebServiceAuthKey,
            .CarrierEDINGLEDIInputWebURL = d.CarrierEDINGLEDIInputWebURL,
            .CarrierEDINGLEDI204OutputWebURL = d.CarrierEDINGLEDI204OutputWebURL,
            .CarrierEDIOutboundFolder = d.CarrierEDIOutboundFolder,
            .CarrierEDILastOutboundTransmission = d.CarrierEDILastOutboundTransmission,
            .CarrierEDIFTPOutboundFolder = d.CarrierEDIFTPOutboundFolder,
            .CarrierEDIFTPBackupFolder = d.CarrierEDIFTPBackupFolder,
            .CarrierEDIFTPInboundFolder = d.CarrierEDIFTPInboundFolder,
            .CarrierEDIFTPServer = d.CarrierEDIFTPServer,
            .CarrierEDIFTPUserName = d.CarrierEDIFTPUserName,
            .CarrierEDIFTPPassword = d.CarrierEDIFTPPassword,
            .CarrierEDIWebServiceURL = d.CarrierEDIWebServiceURL,
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize}
    End Function
    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrierEDI)
        'Create New Record
        Return New LTS.CarrierEDI With {.CarrierEDIControl = d.CarrierEDIControl,
            .CarrierEDICarrierControl = d.CarrierEDICarrierControl,
            .CarrierEDIXaction = d.CarrierEDIXaction,
            .CarrierEDIComment = d.CarrierEDIComment,
            .CarrierEDISecurityQual = d.CarrierEDISecurityQual,
            .CarrierEDISecurityCode = d.CarrierEDISecurityCode,
            .CarrierEDIPartnerQual = d.CarrierEDIPartnerQual,
            .CarrierEDIPartnerCode = d.CarrierEDIPartnerCode,
            .CarrierEDIISASequence = d.CarrierEDIISASequence,
            .CarrierEDIGSSequence = d.CarrierEDIGSSequence,
            .CarrierEDIEmailNotificationOn = d.CarrierEDIEmailNotificationOn,
            .CarrierEDIEmailAddress = d.CarrierEDIEmailAddress,
            .CarrierEDIAcknowledgementRequested = d.CarrierEDIAcknowledgementRequested,
            .CarrierEDIAcceptOn997 = d.CarrierEDIAcceptOn997,
            .CarrierEDITestCode = d.CarrierEDITestCode,
            .CarrierEDIUpdated = If(d.CarrierEDIUpdated Is Nothing, New Byte() {}, d.CarrierEDIUpdated),
            .CarrierEDIInboundFolder = d.CarrierEDIInboundFolder,
            .CarrierEDIBackupFolder = d.CarrierEDIBackupFolder,
            .CarrierEDILogFile = d.CarrierEDILogFile,
            .CarrierEDIStartTime = d.CarrierEDIStartTime,
            .CarrierEDIEndTime = d.CarrierEDIEndTime,
            .CarrierEDIDaysOfWeek = d.CarrierEDIDaysOfWeek,
            .CarrierEDISendMinutesOutbound = d.CarrierEDISendMinutesOutbound,
            .CarrierEDIFileNameBaseOutbound = d.CarrierEDIFileNameBaseOutbound,
            .CarrierEDIFileNameBaseInbound = d.CarrierEDIFileNameBaseInbound,
            .CarrierEDIWebServiceAuthKey = d.CarrierEDIWebServiceAuthKey,
            .CarrierEDINGLEDIInputWebURL = d.CarrierEDINGLEDIInputWebURL,
            .CarrierEDINGLEDI204OutputWebURL = d.CarrierEDINGLEDI204OutputWebURL,
            .CarrierEDIOutboundFolder = d.CarrierEDIOutboundFolder,
            .CarrierEDILastOutboundTransmission = d.CarrierEDILastOutboundTransmission,
            .CarrierEDIFTPOutboundFolder = d.CarrierEDIFTPOutboundFolder,
            .CarrierEDIFTPBackupFolder = d.CarrierEDIFTPBackupFolder,
            .CarrierEDIFTPInboundFolder = d.CarrierEDIFTPInboundFolder,
            .CarrierEDIFTPServer = d.CarrierEDIFTPServer,
            .CarrierEDIFTPUserName = d.CarrierEDIFTPUserName,
            .CarrierEDIFTPPassword = d.CarrierEDIFTPPassword,
            .CarrierEDIWebServiceURL = d.CarrierEDIWebServiceURL,
            .CarrierEDIModDate = Date.Now,
            .CarrierEDIModUser = Me.Parameters.UserName}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrierEDIFiltered(Control:=CType(LinqTable, LTS.CarrierEDI).CarrierEDIControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierEDI = TryCast(LinqTable, LTS.CarrierEDI)
                If source Is Nothing Then Return Nothing
                'Note this data source does not have a Mod Date or Mod User data field
                ret = (From d In db.CarrierEDIs
                    Where d.CarrierEDIControl = source.CarrierEDIControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrierEDIControl _
                        , .ModDate = Date.Now _
                        , .ModUser = Parameters.UserName _
                        , .Updated = d.CarrierEDIUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DataTransferObjects.CarrierEDI)
            Try
                Dim CarrierEDI As DataTransferObjects.CarrierEDI = (
                        From t In CType(oDB, NGLMASCarrierDataContext).CarrierEDIs
                        Where
                        (t.CarrierEDICarrierControl = .CarrierEDICarrierControl _
                         And
                         t.CarrierEDIXaction = .CarrierEDIXaction)
                        Select New DataTransferObjects.CarrierEDI With {.CarrierEDIControl = t.CarrierEDIControl}).FirstOrDefault()

                If Not CarrierEDI Is Nothing Then
                    Utilities.SaveAppError("Cannot save new Carrier EDI data.  The Carrier EDI XAction, " & .CarrierEDIXaction & ",  is already assigned to the selected carrier.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DataTransferObjects.CarrierEDI)
            Try
                'Get the newest record that matches the provided criteria
                Dim CarrierEDI As DataTransferObjects.CarrierEDI = (
                        From t In CType(oDB, NGLMASCarrierDataContext).CarrierEDIs
                        Where
                        (t.CarrierEDIControl <> .CarrierEDIControl) _
                        And
                        (t.CarrierEDICarrierControl = .CarrierEDICarrierControl _
                         And
                         t.CarrierEDIXaction = .CarrierEDIXaction)
                        Select New DataTransferObjects.CarrierEDI With {.CarrierEDIControl = t.CarrierEDIControl}).FirstOrDefault()

                If Not CarrierEDI Is Nothing Then
                    Utilities.SaveAppError("Cannot save Carrier EDI changes.  The Carrier EDI XAction, " & .CarrierEDIXaction & ",  is already assigned to the selected carrier.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

#End Region

End Class