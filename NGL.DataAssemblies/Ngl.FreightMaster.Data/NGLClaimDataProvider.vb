Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports Ngl.Core.ChangeTracker
Imports DTran = Ngl.Core.Utility.DataTransformation

Public Class NGLClaimData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASClaimDataContext(ConnectionString)
        Me.LinqTable = db.Claims
        Me.LinqDB = db
        Me.SourceClass = "NGLClaimData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASClaimDataContext(ConnectionString)
            _LinqTable = db.Claims
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetClaimFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetClaimsFiltered()
    End Function

    Public Function GetClaimFiltered(Optional ByVal Control As Integer = 0) As DTO.Claim
        Using db As New NGLMASClaimDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim Claim As DTO.Claim = ( _
                From t In db.Claims _
                Where _
                    (t.ClaimControl = 0 OrElse t.ClaimControl = Control) _
                Order By t.ClaimControl Descending _
                Select New DTO.Claim With {.ClaimControl = t.ClaimControl _
                                              , .ClaimCustCompControl = t.ClaimCustCompControl _
                                              , .ClaimProNumber = t.ClaimProNumber _
                                              , .ClaimCarrierControl = If(t.ClaimCarrierControl.HasValue, t.ClaimCarrierControl, 0) _
                                              , .ClaimCarrierContact = t.ClaimCarrierContact _
                                              , .ClaimCarrierContactPhone = t.ClaimCarrierContactPhone _
                                              , .ClaimVendCompControl = If(t.ClaimVendCompControl.HasValue, t.ClaimVendCompControl, 0) _
                                              , .ClaimVendName = t.ClaimVendName _
                                              , .ClaimVendAddress1 = t.ClaimVendAddress1 _
                                              , .ClaimVendAddress2 = t.ClaimVendAddress2 _
                                              , .ClaimVendAddress3 = t.ClaimVendAddress3 _
                                              , .ClaimVendCity = t.ClaimVendCity _
                                              , .ClaimVendState = t.ClaimVendState _
                                              , .ClaimVendCountry = t.ClaimVendCountry _
                                              , .ClaimVendZip = t.ClaimVendZip _
                                              , .ClaimVendPhone = t.ClaimVendPhone _
                                              , .ClaimVendFax = t.ClaimVendFax _
                                              , .ClaimConsCompControl = If(t.ClaimConsCompControl.HasValue, t.ClaimConsCompControl, 0) _
                                              , .ClaimConsName = t.ClaimConsName _
                                              , .ClaimConsAddress1 = t.ClaimConsAddress1 _
                                              , .ClaimConsAddress2 = t.ClaimConsAddress2 _
                                              , .ClaimConsAddress3 = t.ClaimConsAddress3 _
                                              , .ClaimConsCity = t.ClaimConsCity _
                                              , .ClaimConsState = t.ClaimConsState _
                                              , .ClaimConsCountry = t.ClaimConsCountry _
                                              , .ClaimConsZip = t.ClaimConsZip _
                                              , .ClaimConsPhone = t.ClaimConsPhone _
                                              , .ClaimConsFax = t.ClaimConsFax _
                                              , .ClaimDateSubm = t.ClaimDateSubm _
                                              , .ClaimDateAck = t.ClaimDateAck _
                                              , .ClaimDatePaid = t.ClaimDatePaid _
                                              , .ClaimCheckNo = t.ClaimCheckNo _
                                              , .ClaimCheckAmt = If(t.ClaimCheckAmt.HasValue, t.ClaimCheckAmt, 0) _
                                              , .ClaimClaimAmt = If(t.ClaimClaimAmt.HasValue, t.ClaimClaimAmt, 0) _
                                              , .ClaimDiff = If(t.ClaimDiff.HasValue, t.ClaimDiff, 0) _
                                              , .ClaimFB = t.ClaimFB _
                                              , .ClaimConnLine = t.ClaimConnLine _
                                              , .ClaimInvName = t.ClaimInvName _
                                              , .ClaimInvPhone = t.ClaimInvPhone _
                                              , .ClaimTruckNo = t.ClaimTruckNo _
                                              , .ClaimShipDesc = t.ClaimShipDesc _
                                              , .ClaimShipFrom = t.ClaimShipFrom _
                                              , .ClaimShipTo = t.ClaimShipTo _
                                              , .ClaimFinalDest = t.ClaimFinalDest _
                                              , .ClaimBOLIssueby = t.ClaimBOLIssueby _
                                              , .ClaimBOLIssueDate = t.ClaimBOLIssueDate _
                                              , .ClaimRemark = t.ClaimRemark _
                                              , .ClaimVia = t.ClaimVia _
                                              , .ClaimOrderNumber = t.ClaimOrderNumber _
                                              , .ClaimDeclined = t.ClaimDeclined _
                                              , .ClaimDeclinedAmt = If(t.ClaimDeclinedAmt.HasValue, t.ClaimDeclinedAmt, 0) _
                                              , .ClaimDeclinedDate = t.ClaimDeclinedDate _
                                              , .ClaimDeclinedByCarrRep = t.ClaimDeclinedByCarrRep _
                                              , .ClaimDeclinedReason = t.ClaimDeclinedReason _
                                              , .ClaimModDate = t.ClaimModDate _
                                              , .ClaimModUser = t.ClaimModUser _
                                              , .ClaimUpdated = t.ClaimUpdated.ToArray()}).First

                Return Claim

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

    Public Function GetClaimFilteredByPro(ByVal proNumber As Integer) As DTO.Claim
        Using db As New NGLMASClaimDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim Claim As DTO.Claim = ( _
                From t In db.Claims _
                Where _
                    (t.ClaimProNumber = 0 OrElse t.ClaimProNumber = proNumber) _
                Order By t.ClaimProNumber Descending _
                Select New DTO.Claim With {.ClaimControl = t.ClaimControl _
                                              , .ClaimCustCompControl = t.ClaimCustCompControl _
                                              , .ClaimProNumber = t.ClaimProNumber _
                                              , .ClaimCarrierControl = If(t.ClaimCarrierControl.HasValue, t.ClaimCarrierControl, 0) _
                                              , .ClaimCarrierContact = t.ClaimCarrierContact _
                                              , .ClaimCarrierContactPhone = t.ClaimCarrierContactPhone _
                                              , .ClaimVendCompControl = If(t.ClaimVendCompControl.HasValue, t.ClaimVendCompControl, 0) _
                                              , .ClaimVendName = t.ClaimVendName _
                                              , .ClaimVendAddress1 = t.ClaimVendAddress1 _
                                              , .ClaimVendAddress2 = t.ClaimVendAddress2 _
                                              , .ClaimVendAddress3 = t.ClaimVendAddress3 _
                                              , .ClaimVendCity = t.ClaimVendCity _
                                              , .ClaimVendState = t.ClaimVendState _
                                              , .ClaimVendCountry = t.ClaimVendCountry _
                                              , .ClaimVendZip = t.ClaimVendZip _
                                              , .ClaimVendPhone = t.ClaimVendPhone _
                                              , .ClaimVendFax = t.ClaimVendFax _
                                              , .ClaimConsCompControl = If(t.ClaimConsCompControl.HasValue, t.ClaimConsCompControl, 0) _
                                              , .ClaimConsName = t.ClaimConsName _
                                              , .ClaimConsAddress1 = t.ClaimConsAddress1 _
                                              , .ClaimConsAddress2 = t.ClaimConsAddress2 _
                                              , .ClaimConsAddress3 = t.ClaimConsAddress3 _
                                              , .ClaimConsCity = t.ClaimConsCity _
                                              , .ClaimConsState = t.ClaimConsState _
                                              , .ClaimConsCountry = t.ClaimConsCountry _
                                              , .ClaimConsZip = t.ClaimConsZip _
                                              , .ClaimConsPhone = t.ClaimConsPhone _
                                              , .ClaimConsFax = t.ClaimConsFax _
                                              , .ClaimDateSubm = t.ClaimDateSubm _
                                              , .ClaimDateAck = t.ClaimDateAck _
                                              , .ClaimDatePaid = t.ClaimDatePaid _
                                              , .ClaimCheckNo = t.ClaimCheckNo _
                                              , .ClaimCheckAmt = If(t.ClaimCheckAmt.HasValue, t.ClaimCheckAmt, 0) _
                                              , .ClaimClaimAmt = If(t.ClaimClaimAmt.HasValue, t.ClaimClaimAmt, 0) _
                                              , .ClaimDiff = If(t.ClaimDiff.HasValue, t.ClaimDiff, 0) _
                                              , .ClaimFB = t.ClaimFB _
                                              , .ClaimConnLine = t.ClaimConnLine _
                                              , .ClaimInvName = t.ClaimInvName _
                                              , .ClaimInvPhone = t.ClaimInvPhone _
                                              , .ClaimTruckNo = t.ClaimTruckNo _
                                              , .ClaimShipDesc = t.ClaimShipDesc _
                                              , .ClaimShipFrom = t.ClaimShipFrom _
                                              , .ClaimShipTo = t.ClaimShipTo _
                                              , .ClaimFinalDest = t.ClaimFinalDest _
                                              , .ClaimBOLIssueby = t.ClaimBOLIssueby _
                                              , .ClaimBOLIssueDate = t.ClaimBOLIssueDate _
                                              , .ClaimRemark = t.ClaimRemark _
                                              , .ClaimVia = t.ClaimVia _
                                              , .ClaimOrderNumber = t.ClaimOrderNumber _
                                              , .ClaimDeclined = t.ClaimDeclined _
                                              , .ClaimDeclinedAmt = If(t.ClaimDeclinedAmt.HasValue, t.ClaimDeclinedAmt, 0) _
                                              , .ClaimDeclinedDate = t.ClaimDeclinedDate _
                                              , .ClaimDeclinedByCarrRep = t.ClaimDeclinedByCarrRep _
                                              , .ClaimDeclinedReason = t.ClaimDeclinedReason _
                                              , .ClaimModDate = t.ClaimModDate _
                                              , .ClaimModUser = t.ClaimModUser _
                                              , .ClaimUpdated = t.ClaimUpdated.ToArray()}).First

                Return Claim

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

    Public Function GetClaimsFiltered(Optional ByVal ProNumber As String = "", _
                                      Optional ByVal CarrierControl As Integer = 0, _
                                      Optional ByVal CompControl As Integer = 0, _
                                      Optional ByVal FB As String = "") As DTO.Claim()
        Using db As New NGLMASClaimDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim Claims As DTO.Claim() = ( _
                From t In db.Claims _
                Where _
                    (ProNumber = "" OrElse t.ClaimProNumber = ProNumber) _
                    And _
                    (CompControl = 0 OrElse t.ClaimVendCompControl = CompControl) _
                    And _
                    (CarrierControl = 0 OrElse t.ClaimCarrierControl = CarrierControl) _
                    And _
                    (FB = "" OrElse t.ClaimFB = FB) _
                Order By t.ClaimControl Descending _
                Select New DTO.Claim With {.ClaimControl = t.ClaimControl _
                                              , .ClaimCustCompControl = t.ClaimCustCompControl _
                                              , .ClaimProNumber = t.ClaimProNumber _
                                              , .ClaimCarrierControl = If(t.ClaimCarrierControl.HasValue, t.ClaimCarrierControl, 0) _
                                              , .ClaimCarrierContact = t.ClaimCarrierContact _
                                              , .ClaimCarrierContactPhone = t.ClaimCarrierContactPhone _
                                              , .ClaimVendCompControl = If(t.ClaimVendCompControl.HasValue, t.ClaimVendCompControl, 0) _
                                              , .ClaimVendName = t.ClaimVendName _
                                              , .ClaimVendAddress1 = t.ClaimVendAddress1 _
                                              , .ClaimVendAddress2 = t.ClaimVendAddress2 _
                                              , .ClaimVendAddress3 = t.ClaimVendAddress3 _
                                              , .ClaimVendCity = t.ClaimVendCity _
                                              , .ClaimVendState = t.ClaimVendState _
                                              , .ClaimVendCountry = t.ClaimVendCountry _
                                              , .ClaimVendZip = t.ClaimVendZip _
                                              , .ClaimVendPhone = t.ClaimVendPhone _
                                              , .ClaimVendFax = t.ClaimVendFax _
                                              , .ClaimConsCompControl = If(t.ClaimConsCompControl.HasValue, t.ClaimConsCompControl, 0) _
                                              , .ClaimConsName = t.ClaimConsName _
                                              , .ClaimConsAddress1 = t.ClaimConsAddress1 _
                                              , .ClaimConsAddress2 = t.ClaimConsAddress2 _
                                              , .ClaimConsAddress3 = t.ClaimConsAddress3 _
                                              , .ClaimConsCity = t.ClaimConsCity _
                                              , .ClaimConsState = t.ClaimConsState _
                                              , .ClaimConsCountry = t.ClaimConsCountry _
                                              , .ClaimConsZip = t.ClaimConsZip _
                                              , .ClaimConsPhone = t.ClaimConsPhone _
                                              , .ClaimConsFax = t.ClaimConsFax _
                                              , .ClaimDateSubm = t.ClaimDateSubm _
                                              , .ClaimDateAck = t.ClaimDateAck _
                                              , .ClaimDatePaid = t.ClaimDatePaid _
                                              , .ClaimCheckNo = t.ClaimCheckNo _
                                              , .ClaimCheckAmt = If(t.ClaimCheckAmt.HasValue, t.ClaimCheckAmt, 0) _
                                              , .ClaimClaimAmt = If(t.ClaimClaimAmt.HasValue, t.ClaimClaimAmt, 0) _
                                              , .ClaimDiff = If(t.ClaimDiff.HasValue, t.ClaimDiff, 0) _
                                              , .ClaimFB = t.ClaimFB _
                                              , .ClaimConnLine = t.ClaimConnLine _
                                              , .ClaimInvName = t.ClaimInvName _
                                              , .ClaimInvPhone = t.ClaimInvPhone _
                                              , .ClaimTruckNo = t.ClaimTruckNo _
                                              , .ClaimShipDesc = t.ClaimShipDesc _
                                              , .ClaimShipFrom = t.ClaimShipFrom _
                                              , .ClaimShipTo = t.ClaimShipTo _
                                              , .ClaimFinalDest = t.ClaimFinalDest _
                                              , .ClaimBOLIssueby = t.ClaimBOLIssueby _
                                              , .ClaimBOLIssueDate = t.ClaimBOLIssueDate _
                                              , .ClaimRemark = t.ClaimRemark _
                                              , .ClaimVia = t.ClaimVia _
                                              , .ClaimOrderNumber = t.ClaimOrderNumber _
                                              , .ClaimDeclined = t.ClaimDeclined _
                                              , .ClaimDeclinedAmt = If(t.ClaimDeclinedAmt.HasValue, t.ClaimDeclinedAmt, 0) _
                                              , .ClaimDeclinedDate = t.ClaimDeclinedDate _
                                              , .ClaimDeclinedByCarrRep = t.ClaimDeclinedByCarrRep _
                                              , .ClaimDeclinedReason = t.ClaimDeclinedReason _
                                              , .ClaimModDate = t.ClaimModDate _
                                              , .ClaimModUser = t.ClaimModUser _
                                              , .ClaimUpdated = t.ClaimUpdated.ToArray()}).ToArray

                Return Claims

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

    Public Function GetClaimLookup() As DTO.ClaimLookup()
        Using db As New NGLMASClaimDataContext(ConnectionString)
            Try
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefClaims Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                'Get the newest record that matches the provided criteria
                Dim Claims As DTO.ClaimLookup() = ( _
                From t In db.vLookupClaims _
                Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.ClaimVendCompControl)) _
                Order By t.ClaimControl Descending _
                Select New DTO.ClaimLookup With {.ClaimControl = t.ClaimControl _
                                              , .ClaimProNumber = t.ClaimProNumber _
                                              , .CarrierName = t.CarrierName _
                                              , .ClaimVendName = t.ClaimVendName _
                                              , .CarrierNumber = If(t.CarrierNumber.HasValue, t.CarrierNumber, 0) _
                                              , .ClaimFB = t.ClaimFB}).ToArray

                Return Claims

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

#End Region

#Region "Claim LTS Methods"


    Public Function CreateClaim(ByVal ibookControl As Integer) As LTS.vLEClaims365
        Dim oRet As New LTS.vLEClaims365()
        Using db As New NGLMASClaimDataContext(ConnectionString)
            Try
                Dim oBook = db.BookRefClaims.Where(Function(x) x.BookControl = ibookControl).FirstOrDefault()
                If oBook Is Nothing OrElse oBook.BookControl = 0 Then
                    throwInvalidKeyFaultException(SqlFaultInfo.FaultDetailsKey.E_CannotSaveKeyFieldsRequired, New List(Of String) From {"Claim Record", "Booking Record"})
                    Return Nothing
                End If
                Dim oNewClaim As New LTS.Claim()
                With oNewClaim
                    .ClaimCustCompControl = oBook.BookCustCompControl
                    .ClaimProNumber = oBook.BookProNumber
                    .ClaimCarrierControl = oBook.BookCarrierControl
                    .ClaimCarrierContact = oBook.BookCarrierContact
                    .ClaimCarrierContactPhone = oBook.BookCarrierContactPhone
                    .ClaimOrderNumber = oBook.BookCarrOrderNumber
                    .ClaimVendCompControl = oBook.BookOrigCompControl
                    .ClaimVendName = oBook.BookOrigName
                    .ClaimVendAddress1 = oBook.BookOrigAddress1
                    .ClaimVendAddress2 = oBook.BookOrigAddress2
                    .ClaimVendAddress3 = oBook.BookOrigAddress3
                    .ClaimVendCity = oBook.BookOrigCity
                    .ClaimVendState = oBook.BookOrigState
                    .ClaimVendCountry = oBook.BookOrigCountry
                    .ClaimVendZip = oBook.BookOrigZip
                    .ClaimVendPhone = oBook.BookOrigPhone
                    .ClaimVendFax = oBook.BookOrigFax
                    .ClaimConsCompControl = oBook.BookDestCompControl
                    .ClaimConsName = oBook.BookDestName
                    .ClaimConsAddress1 = oBook.BookDestAddress1
                    .ClaimConsAddress2 = oBook.BookDestAddress2
                    .ClaimConsAddress3 = oBook.BookDestAddress3
                    .ClaimConsCity = oBook.BookDestCity
                    .ClaimConsState = oBook.BookDestState
                    .ClaimConsCountry = oBook.BookDestCountry
                    .ClaimConsZip = oBook.BookDestZip
                    .ClaimConsPhone = oBook.BookDestPhone
                    .ClaimConsFax = oBook.BookDestFax
                    .ClaimFB = oBook.BookFinAPBillNumber
                    .ClaimShipFrom = .ClaimVendCity & ", " & .ClaimVendState
                    .ClaimShipTo = .ClaimConsCity & ", " & .ClaimConsState
                    .ClaimFinalDest = .ClaimShipTo
                    .ClaimModDate = Date.Now
                    .ClaimModUser = Me.Parameters.UserName
                End With
                db.Claims.InsertOnSubmit(oNewClaim)
                db.SubmitChanges()
                oRet = db.vLEClaims365s.Where(Function(x) x.ClaimControl = oNewClaim.ClaimControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CreateClaim"), db)
            End Try
        End Using
        Return oRet

    End Function

    Public Function GetClaimsFiltered(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vLEClaims365()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vLEClaims365
        Using db As New NGLMASClaimDataContext(ConnectionString)
            Try
                If filters.LEAdminControl = 0 Then filters.LEAdminControl = Parameters.UserLEControl
                'db.Log = New DebugTextWriter
                'Get the data iqueryables
                Dim iQuery As IQueryable(Of LTS.vLEClaims365)
                iQuery = db.vLEClaims365s.Where(Function(x) x.LEAdminControl = filters.LEAdminControl)
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetClaimsFiltered"), db)
            End Try
        End Using
        Return oRet
    End Function

    Public Function SaveClaim(ByVal oClaim As LTS.vLEClaims365) As Boolean
        Dim blnRet As Boolean = False
        If oClaim Is Nothing Then Return False 'nothing to do
        Dim blnisNew As Boolean = False
        Dim oNew As New LTS.Claim()
        Using db As New NGLMASClaimDataContext(ConnectionString)
            Try
                'verify the claim data
                If oClaim.ClaimCustCompControl = 0 Then
                    Dim lDetails As New List(Of String) From {"CompanyID Reference", " was not provided "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If String.IsNullOrWhiteSpace(oClaim.ClaimProNumber) Then
                    Dim lDetails As New List(Of String) From {"Booking Reference", " booking pro was not provided "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If

                If oClaim.ClaimControl = 0 Then
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"LEAdminControl", "CompNumber", "CompName", "CompLegalEntity", "CompAlphaCode", "CarrierName", "CarrierAlphaCode", "ClaimModDate", "ClaimModUser", "ClaimUpdated", "rowguid"}
                    oNew = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oNew, oClaim, skipObjs, strMSG)
                    With oNew
                        .ClaimModDate = Date.Now
                        .ClaimModUser = Me.Parameters.UserName
                    End With

                    db.Claims.InsertOnSubmit(oNew)
                    blnisNew = True
                Else
                    Dim oExisting As LTS.Claim = db.Claims.Where(Function(x) x.ClaimControl = oClaim.ClaimControl).FirstOrDefault()
                    If oExisting Is Nothing OrElse oExisting.ClaimControl = 0 Then
                        throwRecordDeletedFaultException("Cannot save changes to Claim for Booking Pro Number : " & oClaim.ClaimProNumber & ". Please create a new claim and try again.")
                    End If
                    Dim strMsg As String = ""
                    Dim skipObjs As New List(Of String) From {"LEAdminControl", "CompNumber", "CompName", "CompLegalEntity", "CompAlphaCode", "CarrierName", "CarrierAlphaCode", "ClaimModDate", "ClaimModUser", "ClaimUpdated", "rowguid"}
                    oExisting = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oExisting, oClaim, skipObjs, strMSG)
                    With oExisting
                        .ClaimModDate = Date.Now
                        .ClaimModUser = Me.Parameters.UserName
                        .ClaimUpdated = If(oExisting.ClaimUpdated Is Nothing, New Byte() {}, oExisting.ClaimUpdated)
                    End With
                End If
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveClaim"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteClaim(ByVal iControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iControl = 0 Then Return False 'nothing to do
        'validate before delete

        Using db As New NGLMASClaimDataContext(ConnectionString)
            Try
                'Delte the Record
                Dim oToDelete = db.Claims.Where(Function(x) x.ClaimControl = iControl).FirstOrDefault()
                If oToDelete Is Nothing OrElse oToDelete.ClaimControl = 0 Then Return True 'already deleted

                db.Claims.DeleteOnSubmit(oToDelete)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteClaim"), db)
            End Try
        End Using
        Return blnRet
    End Function



#End Region

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim t = CType(oData, DTO.Claim)
        'Create New Record
        Return New LTS.Claim With {.ClaimControl = t.ClaimControl _
                                  , .ClaimCustCompControl = t.ClaimCustCompControl _
                                  , .ClaimProNumber = t.ClaimProNumber _
                                  , .ClaimCarrierControl = t.ClaimCarrierControl _
                                  , .ClaimCarrierContact = t.ClaimCarrierContact _
                                  , .ClaimCarrierContactPhone = t.ClaimCarrierContactPhone _
                                  , .ClaimVendCompControl = t.ClaimVendCompControl _
                                  , .ClaimVendName = t.ClaimVendName _
                                  , .ClaimVendAddress1 = t.ClaimVendAddress1 _
                                  , .ClaimVendAddress2 = t.ClaimVendAddress2 _
                                  , .ClaimVendAddress3 = t.ClaimVendAddress3 _
                                  , .ClaimVendCity = t.ClaimVendCity _
                                  , .ClaimVendState = t.ClaimVendState _
                                  , .ClaimVendCountry = t.ClaimVendCountry _
                                  , .ClaimVendZip = t.ClaimVendZip _
                                  , .ClaimVendPhone = t.ClaimVendPhone _
                                  , .ClaimVendFax = t.ClaimVendFax _
                                  , .ClaimConsCompControl = t.ClaimConsCompControl _
                                  , .ClaimConsName = t.ClaimConsName _
                                  , .ClaimConsAddress1 = t.ClaimConsAddress1 _
                                  , .ClaimConsAddress2 = t.ClaimConsAddress2 _
                                  , .ClaimConsAddress3 = t.ClaimConsAddress3 _
                                  , .ClaimConsCity = t.ClaimConsCity _
                                  , .ClaimConsState = t.ClaimConsState _
                                  , .ClaimConsCountry = t.ClaimConsCountry _
                                  , .ClaimConsZip = t.ClaimConsZip _
                                  , .ClaimConsPhone = t.ClaimConsPhone _
                                  , .ClaimConsFax = t.ClaimConsFax _
                                  , .ClaimDateSubm = t.ClaimDateSubm _
                                  , .ClaimDateAck = t.ClaimDateAck _
                                  , .ClaimDatePaid = t.ClaimDatePaid _
                                  , .ClaimCheckNo = t.ClaimCheckNo _
                                  , .ClaimCheckAmt = t.ClaimCheckAmt _
                                  , .ClaimClaimAmt = t.ClaimClaimAmt _
                                  , .ClaimDiff = t.ClaimDiff _
                                  , .ClaimFB = t.ClaimFB _
                                  , .ClaimConnLine = t.ClaimConnLine _
                                  , .ClaimInvName = t.ClaimInvName _
                                  , .ClaimInvPhone = t.ClaimInvPhone _
                                  , .ClaimTruckNo = t.ClaimTruckNo _
                                  , .ClaimShipDesc = t.ClaimShipDesc _
                                  , .ClaimShipFrom = t.ClaimShipFrom _
                                  , .ClaimShipTo = t.ClaimShipTo _
                                  , .ClaimFinalDest = t.ClaimFinalDest _
                                  , .ClaimBOLIssueby = t.ClaimBOLIssueby _
                                  , .ClaimBOLIssueDate = t.ClaimBOLIssueDate _
                                  , .ClaimRemark = t.ClaimRemark _
                                  , .ClaimVia = t.ClaimVia _
                                  , .ClaimOrderNumber = t.ClaimOrderNumber _
                                  , .ClaimDeclined = t.ClaimDeclined _
                                  , .ClaimDeclinedAmt = t.ClaimDeclinedAmt _
                                  , .ClaimDeclinedDate = t.ClaimDeclinedDate _
                                  , .ClaimDeclinedByCarrRep = t.ClaimDeclinedByCarrRep _
                                  , .ClaimDeclinedReason = t.ClaimDeclinedReason _
                                  , .ClaimModDate = Date.Now _
                                  , .ClaimModUser = Parameters.UserName _
                                  , .ClaimUpdated = If(t.ClaimUpdated Is Nothing, New Byte() {}, t.ClaimUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetClaimFiltered(Control:=CType(LinqTable, LTS.Claim).ClaimControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASClaimDataContext(ConnectionString)
            Try
                Dim source As LTS.Claim = TryCast(LinqTable, LTS.Claim)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.Claims _
                       Where d.ClaimControl = source.ClaimControl _
                       Select New DTO.QuickSaveResults With {.Control = d.ClaimControl _
                                                            , .ModDate = d.ClaimModDate _
                                                            , .ModUser = d.ClaimModUser _
                                                            , .Updated = d.ClaimUpdated.ToArray}).First

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

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed       
        With CType(oData, DTO.Claim)
            Try
                'Validate that required fields have been entered
                Dim blnValidationErr As Boolean = False
                Dim strValidationMsg As String = ""
                Dim strSpacer As String = ""
                If String.IsNullOrEmpty(.ClaimProNumber) OrElse .ClaimProNumber.Trim.Length < 1 Then
                    blnValidationErr = True
                    strValidationMsg = "ClaimProNumber"
                    strSpacer = ", "
                End If

                If blnValidationErr Then
                    Utilities.SaveAppError("Cannot save new Claim data.  The following key fields are required: " & strValidationMsg & ".", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

                Dim claim As DTO.Claim = ( _
                    From t In CType(oDB, NGLMASClaimDataContext).Claims _
                     Where _
                         (t.ClaimProNumber = .ClaimProNumber) _
                     Select New DTO.Claim With {.ClaimControl = t.ClaimControl}).First


                If Not claim Is Nothing Then
                    Utilities.SaveAppError("Cannot save new Claim data.  The Claim pro number, " & .ClaimProNumber & ", already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.Claim)
            Try
                'Get the newest record that matches the provided criteria
                Dim claim As DTO.Claim = ( _
                    From t In CType(oDB, NGLMASClaimDataContext).Claims _
                     Where _
                     (t.ClaimControl <> .ClaimControl) _
                     And _
                         (t.ClaimProNumber = .ClaimProNumber) _
                 Select New DTO.Claim With {.ClaimControl = t.ClaimControl}).First

                If Not claim Is Nothing Then
                    Utilities.SaveAppError("Cannot save Claim changes.  The Claim pro number, " & .ClaimProNumber & ", already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the Claim is being used by the ClaimLoad or ClaimTrack data
        With CType(oData, DTO.Claim)
            Try
                Try
                    Dim child As New NGLClaimLoadData(Me.Parameters)
                    Dim childrecords = child.GetClaimLoadsFiltered(.ClaimControl)
                    If Not childrecords Is Nothing AndAlso childrecords.Count > 0 Then
                        Utilities.SaveAppError("Cannot delete Claim data.  The Claim for Pro Number, " & .ClaimProNumber & " is being used and cannot be deleted. check the claim load detail information.", Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse"}, New FaultReason("E_DataValidationFailure"))
                    End If
                Catch ex As FaultException
                    If ex.Message <> "E_NoData" Then
                        Throw
                    End If
                End Try

                Try
                    Dim child As New NGLClaimTrackData(Me.Parameters)
                    Dim childrecords = child.GetClaimTracksFiltered(.ClaimControl)
                    If Not childrecords Is Nothing AndAlso childrecords.Count > 0 Then
                        Utilities.SaveAppError("Cannot delete Claim data.  The Claim for Pro Number, " & .ClaimProNumber & " is being used and cannot be deleted. check the claim track detail information.", Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse"}, New FaultReason("E_DataValidationFailure"))
                    End If
                Catch ex As FaultException
                    If ex.Message <> "E_NoData" Then
                        Throw
                    End If
                End Try

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub
#End Region

End Class

Public Class NGLClaimLoadData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASClaimDataContext(ConnectionString)
        Me.LinqTable = db.ClaimLoads
        Me.LinqDB = db
        Me.SourceClass = "NGLClainLoadData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASClaimDataContext(ConnectionString)
            _LinqTable = db.ClaimLoads
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetClaimLoadFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetClaimLoadsFiltered()
    End Function

    Public Function GetClaimLoadFiltered(ByVal Control As Integer) As DTO.ClaimLoad
        Using db As New NGLMASClaimDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim ClaimLoad As DTO.ClaimLoad = ( _
                From d In db.ClaimLoads _
                Where _
                    d.ClaimLoadControl = Control _
                Select New DTO.ClaimLoad With {.ClaimLoadControl = d.ClaimLoadControl _
                                              , .ClaimLoadClaimControl = If(d.ClaimLoadClaimControl.HasValue, d.ClaimLoadClaimControl, 0) _
                                              , .ClaimLoadType = d.ClaimLoadType _
                                              , .ClaimLoadItem = d.ClaimLoadItem _
                                              , .ClaimLoadDesc = d.ClaimLoadDesc _
                                              , .ClaimLoadUnitCost = If(d.ClaimLoadUnitCost.HasValue, d.ClaimLoadUnitCost, 0) _
                                              , .ClaimLoadUnitFrt = If(d.ClaimLoadUnitFrt.HasValue, d.ClaimLoadUnitFrt, 0) _
                                              , .ClaimLoadUnitQty = If(d.ClaimLoadUnitQty.HasValue, d.ClaimLoadUnitQty, 0) _
                                              , .ClaimLoadLineCost = If(d.ClaimLoadLineCost.HasValue, d.ClaimLoadLineCost, 0) _
                                              , .ClaimLoadUpdated = d.ClaimLoadUpdated.ToArray()}).First


                Return ClaimLoad

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

    Public Function GetClaimLoadsFiltered(Optional ByVal ClaimControl As Integer = 0) As DTO.ClaimLoad()
        Using db As New NGLMASClaimDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim ClaimLoads() As DTO.ClaimLoad = ( _
                From d In db.ClaimLoads _
                Where _
                    (ClaimControl = 0 OrElse d.ClaimLoadClaimControl = ClaimControl) _
                Order By d.ClaimLoadControl _
                Select New DTO.ClaimLoad With {.ClaimLoadControl = d.ClaimLoadControl _
                                              , .ClaimLoadClaimControl = If(d.ClaimLoadClaimControl.HasValue, d.ClaimLoadClaimControl, 0) _
                                              , .ClaimLoadType = d.ClaimLoadType _
                                              , .ClaimLoadItem = d.ClaimLoadItem _
                                              , .ClaimLoadDesc = d.ClaimLoadDesc _
                                              , .ClaimLoadUnitCost = If(d.ClaimLoadUnitCost.HasValue, d.ClaimLoadUnitCost, 0) _
                                              , .ClaimLoadUnitFrt = If(d.ClaimLoadUnitFrt.HasValue, d.ClaimLoadUnitFrt, 0) _
                                              , .ClaimLoadUnitQty = If(d.ClaimLoadUnitQty.HasValue, d.ClaimLoadUnitQty, 0) _
                                              , .ClaimLoadLineCost = If(d.ClaimLoadLineCost.HasValue, d.ClaimLoadLineCost, 0) _
                                              , .ClaimLoadUpdated = d.ClaimLoadUpdated.ToArray()}).ToArray()
                Return ClaimLoads

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

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.ClaimLoad)
        'Create New Record
        Return New LTS.ClaimLoad With {.ClaimLoadControl = d.ClaimLoadControl _
                                  , .ClaimLoadClaimControl = d.ClaimLoadClaimControl _
                                  , .ClaimLoadType = d.ClaimLoadType _
                                  , .ClaimLoadItem = d.ClaimLoadItem _
                                  , .ClaimLoadDesc = d.ClaimLoadDesc _
                                  , .ClaimLoadUnitCost = d.ClaimLoadUnitCost _
                                  , .ClaimLoadUnitFrt = d.ClaimLoadUnitFrt _
                                  , .ClaimLoadUnitQty = d.ClaimLoadUnitQty _
                                  , .ClaimLoadLineCost = d.ClaimLoadLineCost _
                                  , .ClaimLoadUpdated = If(d.ClaimLoadUpdated Is Nothing, New Byte() {}, d.ClaimLoadUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetClaimLoadFiltered(Control:=CType(LinqTable, LTS.ClaimLoad).ClaimLoadControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASClaimDataContext(ConnectionString)
            Try
                Dim source As LTS.ClaimLoad = TryCast(LinqTable, LTS.ClaimLoad)
                If source Is Nothing Then Return Nothing
                'Note this data source does not have a Mod Date or Mod User data field
                ret = (From d In db.ClaimLoads _
                       Where d.ClaimLoadControl = source.ClaimLoadControl _
                       Select New DTO.QuickSaveResults With {.Control = d.ClaimLoadControl _
                                                            , .ModDate = Date.Now _
                                                            , .ModUser = Me.Parameters.UserName _
                                                            , .Updated = d.ClaimLoadUpdated.ToArray}).First

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

#End Region

End Class

Public Class NGLClaimTrackData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASClaimDataContext(ConnectionString)
        Me.LinqTable = db.ClaimTracks
        Me.LinqDB = db
        Me.SourceClass = "NGLClaimTrackData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASClaimDataContext(ConnectionString)
            _LinqTable = db.ClaimTracks
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetClaimTrackFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetClaimTracksFiltered()
    End Function

    Public Function GetClaimTrackFiltered(ByVal Control As Integer) As DTO.ClaimTrack
        Using db As New NGLMASClaimDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim ClaimTrack As DTO.ClaimTrack = ( _
                From d In db.ClaimTracks _
                Where _
                    d.ClaimTrackControl = Control _
                Select New DTO.ClaimTrack With {.ClaimTrackControl = d.ClaimTrackControl _
                                              , .ClaimTrackClaimControl = If(d.ClaimTrackClaimControl.HasValue, d.ClaimTrackClaimControl, 0) _
                                              , .ClaimTrackDate = d.ClaimTrackDate _
                                              , .ClaimTrackContact = d.ClaimTrackContact _
                                              , .ClaimTrackComment = d.ClaimTrackComment _
                                              , .ClaimTrackModUser = d.ClaimTrackModUser _
                                              , .ClaimTrackModDate = d.ClaimTrackModDate _
                                              , .ClaimTrackUpdated = d.ClaimTrackUpdated.ToArray()}).First


                Return ClaimTrack

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

    Public Function GetClaimTracksFiltered(Optional ByVal ClaimControl As Integer = 0) As DTO.ClaimTrack()
        Using db As New NGLMASClaimDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim ClaimTracks() As DTO.ClaimTrack = ( _
                From d In db.ClaimTracks _
                Where _
                    (ClaimControl = 0 OrElse d.ClaimTrackClaimControl = ClaimControl) _
                Order By d.ClaimTrackControl _
                Select New DTO.ClaimTrack With {.ClaimTrackControl = d.ClaimTrackControl _
                                              , .ClaimTrackClaimControl = If(d.ClaimTrackClaimControl.HasValue, d.ClaimTrackClaimControl, 0) _
                                              , .ClaimTrackDate = d.ClaimTrackDate _
                                              , .ClaimTrackContact = d.ClaimTrackContact _
                                              , .ClaimTrackComment = d.ClaimTrackComment _
                                              , .ClaimTrackModUser = d.ClaimTrackModUser _
                                              , .ClaimTrackModDate = d.ClaimTrackModDate _
                                              , .ClaimTrackUpdated = d.ClaimTrackUpdated.ToArray()}).ToArray()
                Return ClaimTracks

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

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.ClaimTrack)
        'Create New Record
        Return New LTS.ClaimTrack With {.ClaimTrackControl = d.ClaimTrackControl _
                                  , .ClaimTrackClaimControl = d.ClaimTrackClaimControl _
                                  , .ClaimTrackDate = d.ClaimTrackDate _
                                  , .ClaimTrackContact = d.ClaimTrackContact _
                                  , .ClaimTrackComment = d.ClaimTrackComment _
                                  , .ClaimTrackModUser = Me.Parameters.UserName _
                                  , .ClaimTrackModDate = Date.Now _
                                  , .ClaimTrackUpdated = If(d.ClaimTrackUpdated Is Nothing, New Byte() {}, d.ClaimTrackUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetClaimTrackFiltered(Control:=CType(LinqTable, LTS.ClaimTrack).ClaimTrackControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASClaimDataContext(ConnectionString)
            Try
                Dim source As LTS.ClaimTrack = TryCast(LinqTable, LTS.ClaimTrack)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.ClaimTracks _
                       Where d.ClaimTrackControl = source.ClaimTrackControl _
                       Select New DTO.QuickSaveResults With {.Control = d.ClaimTrackControl _
                                                            , .ModDate = d.ClaimTrackModDate _
                                                            , .ModUser = d.ClaimTrackModUser _
                                                            , .Updated = d.ClaimTrackUpdated.ToArray}).First

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


#End Region

End Class

Public Class NGLClaimLoadTypeCodeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASClaimDataContext(ConnectionString)
        Me.LinqTable = db.ClaimLoadTypeCodes
        Me.LinqDB = db
        Me.SourceClass = "NGLClaimLoadTypeCodeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASClaimDataContext(ConnectionString)
            _LinqTable = db.ClaimLoadTypeCodes
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetClaimLoadTypeCodesFiltered()
    End Function

    Public Function GetClaimLoadTypeCodeFiltered(ByVal TypeCode As String) As DTO.ClaimLoadTypeCode
        Using db As New NGLMASClaimDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim oClaimLoadTypeCode As DTO.ClaimLoadTypeCode = ( _
                From d In db.ClaimLoadTypeCodes _
                Where _
                    d.ClaimLoadTypeCode = TypeCode _
                Select New DTO.ClaimLoadTypeCode With {.ClaimLoadTypeCode = d.ClaimLoadTypeCode _
                                              , .ClaimLoadTypeCodesUpdated = d.ClaimLoadTypeCodesUpdated.ToArray()}).First


                Return oClaimLoadTypeCode

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

    Public Function GetClaimLoadTypeCodesFiltered() As DTO.ClaimLoadTypeCode()
        Using db As New NGLMASClaimDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim ClaimLoadTypeCodes() As DTO.ClaimLoadTypeCode = ( _
                From d In db.ClaimLoadTypeCodes _
                Order By d.ClaimLoadTypeCode _
                Select New DTO.ClaimLoadTypeCode With {.ClaimLoadTypeCode = d.ClaimLoadTypeCode _
                                              , .ClaimLoadTypeCodesUpdated = d.ClaimLoadTypeCodesUpdated.ToArray()}).ToArray()
                Return ClaimLoadTypeCodes

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

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.ClaimLoadTypeCode)
        'Create New Record
        Return New LTS.ClaimLoadTypeCode With {.ClaimLoadTypeCode = d.ClaimLoadTypeCode _
                                  , .ClaimLoadTypeCodesUpdated = If(d.ClaimLoadTypeCodesUpdated Is Nothing, New Byte() {}, d.ClaimLoadTypeCodesUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetClaimLoadTypeCodeFiltered(CType(LinqTable, LTS.ClaimLoadTypeCode).ClaimLoadTypeCode)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        With CType(oData, DTO.ClaimLoadTypeCode)
            Try
               
                If String.IsNullOrEmpty(.ClaimLoadTypeCode) OrElse .ClaimLoadTypeCode.Trim.Length < 1 Then
                    Utilities.SaveAppError("Cannot save new ClaimLoadTypeCode data.  The Type Code is required and cannot be left blank.  Please try again.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                Else
                    Dim oClaim = From d In CType(oDB, NGLMASClaimDataContext).ClaimLoadTypeCodes Where d.ClaimLoadTypeCode = .ClaimLoadTypeCode
                    If Not oClaim Is Nothing AndAlso oClaim.Count > 0 Then
                        Utilities.SaveAppError("Cannot save new ClaimLoadTypeCode data.  The Type Code, " & .ClaimLoadTypeCode & " ,  already exist.", Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))

                    End If
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try

        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow records to be updated from this class
        Utilities.SaveAppError("Cannot save changes data.  Records cannot be updated using this interface.  Please use delete or add to change data!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
       
    End Sub


#End Region

End Class

