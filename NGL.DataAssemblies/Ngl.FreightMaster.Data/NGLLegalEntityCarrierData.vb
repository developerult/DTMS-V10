Imports Ngl.FreightMaster.Data.LTS

Public Class NGLLegalEntityCarrierData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.tblLegalEntityCarriers
        Me.LinqDB = db
        Me.SourceClass = "NGLLegalEntityCarrierData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.tblLegalEntityCarriers
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

    ''' <summary>
    ''' Replaces LookupDataProvider.GetCarriersForLegalEntity
    ''' Returns Carrier information for LE from view vLECarCarriers
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="LEControl"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/14/18 for v-8.2 VSTS #337
    '''  Renamed tblNGLAPICarrierXref to tblLegalEntityCarrier
    '''  Replaced LookupDataProvider.GetCarriersForLegalEntity
    ''' </remarks>
    Public Function GetCarriersForLegalEntity(ByRef RecordCount As Integer,
                                              ByVal LEControl As Integer,
                                              Optional ByVal filterWhere As String = "",
                                              Optional ByVal sortExpression As String = "CarrierName Asc",
                                              Optional ByVal page As Integer = 1,
                                              Optional ByVal pagesize As Integer = 1000,
                                              Optional ByVal skip As Integer = 0,
                                              Optional ByVal take As Integer = 0) As LTS.vLECarCarrier()
        Dim oRetData As LTS.vLECarCarrier()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim intPageCount As Integer = 1

                Dim oQuery = (From t In db.vLECarCarriers
                              Where t.LEAdminControl = LEControl
                              Select t)

                If oQuery Is Nothing Then Return Nothing
                If Not String.IsNullOrEmpty(filterWhere) Then
                    oQuery = oQuery.Where(Function(x) x.CarrierName.StartsWith(filterWhere) Or x.CarrierSCAC.StartsWith(filterWhere) Or x.CarrierNumber.ToString.StartsWith(filterWhere))
                End If

                RecordCount = oQuery.Count()
                If RecordCount < 1 Then Return Nothing

                If take <> 0 Then
                    pagesize = take
                Else
                    'calculate based on page and pagesize
                    If pagesize < 1 Then pagesize = 1
                    If RecordCount < 1 Then RecordCount = 1
                    If page < 1 Then page = 1
                    skip = (page - 1) * pagesize
                End If
                If RecordCount > pagesize Then intPageCount = ((RecordCount - 1) \ pagesize) + 1
                oRetData = oQuery.OrderBy(sortExpression).Skip(skip).Take(pagesize).ToArray()

                Return oRetData

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarriersForLegalEntity"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Replaces NGLCarrierData.GetCarrierDispatchSettingsByLE()
    ''' Returns "Carrier Dispatch Settings" aka LegalEntityCarrier records for LE
    ''' through view vLegalEntityCarrierByLEs
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/15/18 for v-8.2 VSTS #337
    '''  Renamed tblNGLAPICarrierXref to tblLegalEntityCarrier
    '''  Replaces NGLCarrierData.GetCarrierDispatchSettingsByLE()
    ''' </remarks>
    Public Function GetLegalEntityCarriersByLE(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vLegalEntityCarrierByLE()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vLegalEntityCarrierByLE
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'set default sort if none exists
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CompName"
                    filters.sortDirection = "Asc"
                End If
                'Get the data iqueryables
                Dim iQuery As IQueryable(Of LTS.vLegalEntityCarrierByLE)
                iQuery = db.vLegalEntityCarrierByLEs.Where(Function(x) x.LEAdminControl = filters.LEAdminControl)
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLegalEntityCarriersByLE"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetLegalEntityCarriersByCarrier(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vLegalEntityCarrierByLE()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vLegalEntityCarrierByLE
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'set default sort if none exists
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CompName"
                    filters.sortDirection = "Asc"
                End If
                'Get the data iqueryables
                Dim iQuery As IQueryable(Of LTS.vLegalEntityCarrierByLE)
                iQuery = db.vLegalEntityCarrierByLEs
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLegalEntityCarriersByLE"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Deletes tblLegalEntityCarriers
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 6/14/18 for v-8.2 VSTS #337
    '''  Renamed tblNGLAPICarrierXref to tblLegalEntityCarrier
    '''  Replaced CarrierDataProvider.NGLCarrierData.DeleteCarrierDispatchSetting()
    '''  Modified By LVV on 6/18/18 for v-8.2 VSTS #337
    '''  Renamed tblCarrierLegalAccessorialXref to tblLECarrierAccessorial
    ''' </remarks>
    Public Function DeleteLegalEntityCarrier(ByVal Control As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Dim oTable = db.tblLegalEntityCarriers
            Dim oChildTbl = db.tblLECarrierAccessorials
            Try
                Dim oRecord As LTS.tblLegalEntityCarrier = db.tblLegalEntityCarriers.Where(Function(x) x.LECarControl = Control).FirstOrDefault()

                Dim oChild As LTS.tblLECarrierAccessorial() = db.tblLECarrierAccessorials.Where(Function(x) x.LECALECarControl = Control).ToArray()

                If (oRecord Is Nothing OrElse oRecord.LECarControl = 0) Then Return False
                'oTable.Attach(oRecord, True)
                oTable.DeleteOnSubmit(oRecord)

                For Each r In oChild
                    If (Not r Is Nothing AndAlso r.LECAControl <> 0) Then
                        oChildTbl.DeleteOnSubmit(r)
                    End If
                Next

                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteLegalEntityCarrier"), db)
            End Try
        End Using
        Return blnRet
    End Function

    '    Public Function IsAccessorialAssignedToLE(LECarrierControl As Integer)
    '        Using db As New NGLMASCarrierDataContext(ConnectionString)
    '            Dim t = (From p In db.tblLECarrierAccessorials
    '                     Where p.LECALECarControl = LECarrierControl
    '                     Select p).FirstOrDefault())

    'End Using
    '    End Function
    ''' <summary>
    ''' Added by LVV on 6/15/18 for v-8.2 VSTS #337
    ''' Replaces LookupDataProvider.GetAccessorialsByCarrierByLegalEntity()
    ''' 
    ''' Gets the LE from the logged in user from params. Gets the list of Carriers set up for that LE, and then gets a list of 
    ''' P44 Accessorials supported by each of those carriers (elimiating duplicates). Returns a list of all supported P44 Accessorials
    ''' accross all (P44 supported) Carriers set up for the users LE.
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV on 03/14/2018 for v-8.1 VSTS Task #327 Ted Page
    '''  Removed LEControl as method param because we can now get that from the Parameters object
    '''Modified by RHR for v-8.2.0.109 on 2/20/2019
    ''' We no longer filter codes by carrier or legal entity.
    ''' All accessorials are available to all users so Criteria is not used
    ''' </remarks>
    Public Function GetAccessorialsByLegalEntityCarrier(ByRef RecordCount As Integer,
                                                        Optional ByVal filterWhere As String = "",
                                                        Optional ByVal sortExpression As String = "Code Asc",
                                                        Optional ByVal page As Integer = 1,
                                                        Optional ByVal pagesize As Integer = 1000,
                                                        Optional ByVal skip As Integer = 0,
                                                        Optional ByVal take As Integer = 0) As Models.NGLAPIAccessorial()
        Dim oRetData As Models.NGLAPIAccessorial()
        Using db As New NGLMASLookupDataContext(ConnectionString)
            Try
                Dim intPageCount As Integer = 1

                'Dim LEControl As Integer = Parameters.UserLEControl

                'Filter By LEControl
                'old query like this were removed.
                'Dim oQuery = (From t In db.vAccByLegalEntityCarriers
                '              Where t.LEAdminControl = LEControl
                '              Select t.NACCode, t.NACName, t.NACDesc, t.NACControl
                '                  ).GroupBy(Function(g) New With {g.NACCode}
                '                  ).Select(Function(x) New Models.NGLAPIAccessorial() With
                '                        {.Code = x.Key.NACCode,
                '                        .Name = x.FirstOrDefault().NACName,
                '                        .Desc = x.FirstOrDefault().NACDesc,
                '                        .Control = x.FirstOrDefault().NACControl}
                '                        )
                Dim oQuery = (From t In db.vLookupAcssCodes
                              Select New Models.NGLAPIAccessorial() With
                              {.Code = t.NACCode,
                              .Name = t.NACName,
                              .Desc = t.NACCode,
                              .Control = t.NACControl}
                        )

                If oQuery Is Nothing Then Return Nothing
                If Not String.IsNullOrEmpty(filterWhere) Then
                    oQuery = oQuery.Where(Function(x) x.Code.StartsWith(filterWhere) Or x.Name.StartsWith(filterWhere))
                End If

                RecordCount = oQuery.Count()
                If RecordCount < 1 Then Return Nothing

                If take <> 0 Then
                    pagesize = take
                Else
                    'calculate based on page and pagesize
                    If pagesize < 1 Then pagesize = 1
                    If RecordCount < 1 Then RecordCount = 1
                    If page < 1 Then page = 1
                    skip = (page - 1) * pagesize
                End If
                If RecordCount > pagesize Then intPageCount = ((RecordCount - 1) \ pagesize) + 1
                oRetData = oQuery.OrderBy(sortExpression).Skip(skip).Take(pagesize).ToArray()

                Return oRetData

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAccessorialsByLegalEntityCarrier"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Call a stored procedure used to insert or update the Legal Entity Dispatch Settings
    ''' </summary>
    ''' <param name="LEAdminControl"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="DispatchTypeControl"></param>
    ''' <param name="RateShopOnly"></param>
    ''' <param name="APIDispatching"></param>
    ''' <param name="APIStatusUpdates"></param>
    ''' <param name="ShowAuditFailReason"></param>
    ''' <param name="ShowPendingFeeFailReason"></param>
    ''' <param name="BillToCompControl"></param>
    ''' <param name="CarrierAccountRef"></param>
    ''' <param name="LECarUseDefault"></param>
    ''' <param name="LECarExpiredLoadsTo"></param>
    ''' <param name="LECarExpiredLoadsCc"></param>
    ''' <param name="LECarCarrierAcceptLoadMins"></param>
    ''' <param name="BillingAddr1"></param>
    ''' <param name="BillingAddr2"></param>
    ''' <param name="BillingAddr3"></param>
    ''' <param name="BillingCity"></param>
    ''' <param name="BillingState"></param>
    ''' <param name="BillingZip"></param>
    ''' <param name="BillingCountry"></param>
    ''' <param name="LECarAllowLTLConsolidation"></param>
    ''' <param name="LECarAllowCarrierAcceptRejectByEmail"></param>
    ''' <param name="LECarCarrierAuthCarrierAcceptRejectByEmail"></param>
    ''' <param name="LECarCarrierAuthCarrierAcceptRejectExpMin"></param>
    ''' <param name="LECarWillDriveSunday"></param>
    ''' <param name="LECarWillDriveSaturday"></param>
    ''' <param name="LECarUpliftUseCarrierSpecific"></param>
    ''' <param name="LECarCarrierSpecificUpliftPerc"></param>
    ''' <returns></returns>
    ''' ''' <remarks>
    ''' Added by LVV on 7/13/18 for v-8.2
    ''' Modified by RHR for v-8.2.0.117 on 07/17/2019
    '''   Added optional parameter for new LTL Consolidation feature
    '''   LECarAllowLTLConsolidation
    '''Modified by RHR for v-8.4 on 4/22/2021
    ''' Modified by RHR for v-8.5.3.006 on 12/01/2022
    '''     added New carrier drive days
    '''     LECarWillDriveSunday 
    '''     LECarWillDriveSaturday
    '''Modified by RHR for v-8.5.4.001 o 07/06/2023
    ''' add new carrier uplift properties
    '''     LECarUpliftUseCarrierSpecific
    '''     LECarCarrierSpecificUpliftPerc
    ''' </remarks>
    Public Function InsertOrUpdateLegalEntityCarrier(ByVal LEAdminControl As Integer,
                                                     ByVal CarrierControl As Integer,
                                                     ByVal DispatchTypeControl As Integer,
                                                     ByVal RateShopOnly As Boolean,
                                                     ByVal APIDispatching As Boolean,
                                                     ByVal APIStatusUpdates As Boolean,
                                                     ByVal ShowAuditFailReason As Boolean,
                                                     ByVal ShowPendingFeeFailReason As Boolean,
                                                     ByVal BillToCompControl As Integer,
                                                     ByVal CarrierAccountRef As String,
                                                     ByVal LECarUseDefault As Boolean,
                                                     ByVal LECarExpiredLoadsTo As String,
                                                     ByVal LECarExpiredLoadsCc As String,
                                                     ByVal LECarCarrierAcceptLoadMins As Integer,
                                                     ByVal BillingAddr1 As String,
                                                     ByVal BillingAddr2 As String,
                                                     ByVal BillingAddr3 As String,
                                                     ByVal BillingCity As String,
                                                     ByVal BillingState As String,
                                                     ByVal BillingZip As String,
                                                     ByVal BillingCountry As String,
                                                     Optional ByVal LECarAllowLTLConsolidation As Boolean = False,
                                                     Optional ByVal LECarAllowCarrierAcceptRejectByEmail As Boolean = False, 'Turn Carrier Accept/Reject With Token via Email on Or off
                                                     Optional ByVal LECarCarrierAuthCarrierAcceptRejectByEmail As Boolean = False, 'Require Carrier Username And Password for Carrier Accept/Reject in addition to Token
                                                     Optional ByVal LECarCarrierAuthCarrierAcceptRejectExpMin As Integer = 0, 'Carrier Accept/Reject With Token Expiration Minutes When Null Or zero use Global Parameter AutoExpireTenderTokenMin
                                                     Optional ByVal LECarWillDriveSunday As Boolean = False,
                                                     Optional ByVal LECarWillDriveSaturday As Boolean = False,
                                                     Optional ByVal LECarUpliftUseCarrierSpecific As Boolean = False,
                                                     Optional ByVal LECarCarrierSpecificUpliftPerc As Decimal = 0
                                                     ) As LTS.spInsertOrUpdateCarrierDispatchSettingResult
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                '** TODO LVV ** ADD LOGIC TO CHECK IF THE USER HAS ROLE CENTER PERMISSIONS TO RUN THIS PROCEDURE
                Return db.spInsertOrUpdateCarrierDispatchSetting(LEAdminControl, CarrierControl, DispatchTypeControl, RateShopOnly, APIDispatching, APIStatusUpdates,
                                                                 ShowAuditFailReason, ShowPendingFeeFailReason, BillToCompControl, CarrierAccountRef,
                                                                 LECarUseDefault, LECarExpiredLoadsTo, LECarExpiredLoadsCc, LECarCarrierAcceptLoadMins,
                                                                 BillingAddr1, BillingAddr2, BillingAddr3, BillingCity, BillingState, BillingZip, BillingCountry,
                                                                 LECarAllowLTLConsolidation, LECarAllowCarrierAcceptRejectByEmail, LECarCarrierAuthCarrierAcceptRejectByEmail,
                                                                 LECarCarrierAuthCarrierAcceptRejectExpMin, LECarWillDriveSunday, LECarWillDriveSaturday, LECarUpliftUseCarrierSpecific, LECarCarrierSpecificUpliftPerc).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateLegalEntityCarrier"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' The only time this would return false is if an exception occurs
    ''' </summary>
    ''' <param name="LECarControl"></param>
    ''' <param name="BillToCompControl"></param>
    ''' <returns></returns>
    Public Function SetLECarrierBillToComp(ByVal LECarControl As Integer, ByVal BillToCompControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Note: I think this probably breaks optimistic concurrency
                Dim oRecord As LTS.tblLegalEntityCarrier = db.tblLegalEntityCarriers.Where(Function(x) x.LECarControl = LECarControl).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.LECarControl = 0) Then Return False
                oRecord.LECarBillToCompControl = BillToCompControl
                oRecord.LECarModDate = Date.Now
                oRecord.LECarModUser = Parameters.UserName
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SetLECarrierBillToComp"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Returns the parent carrier control for the provided Legal Entity Carrier Control child table
    ''' </summary>
    ''' <param name="iLECarControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.3.0.001 on 09/21/2020
    ''' </remarks>
    Public Function GetCarrierControlForLECarControl(ByVal iLECarControl As Integer) As Integer
        Dim iCarrierControl As Integer = 0

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                ' need to lookup carrier control number from tblLegalEntityCarriers
                iCarrierControl = db.tblLegalEntityCarriers.Where(Function(x) x.LECarControl = iLECarControl).Select(Function(x) x.LECarCarrierControl).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierControlForLECarControl"), db)
            End Try
        End Using
        Return iCarrierControl
    End Function

    ''' <summary>
    ''' is Accept/Reject by email on for this carrier and this comp's legal entity
    ''' </summary>
    ''' <param name="iCarrierControl"></param>
    ''' <param name="iCompControl"></param>
    ''' <param name="iExpMin"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4 on 05/04/2021
    '''   new function to read Legal Entity Carrier Accept/Reject by Email settings
    ''' </remarks>
    Public Function AllowLECarAcceptRejectTokenByEmail(ByVal iCarrierControl As Integer, ByVal iCompControl As Integer, ByRef iExpMin As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim sCompLegalEntity As String = db.CompRefCarriers.Where(Function(x) x.CompControl = iCompControl).Select(Function(y) y.CompLegalEntity).FirstOrDefault()
                Dim iLEAdminControl = getLEAdminControlByLegalEntityName(sCompLegalEntity)
                Dim oLegalEntityCar As LTS.tblLegalEntityCarrier = db.tblLegalEntityCarriers.Where(Function(x) x.LECarCarrierControl = iCarrierControl And x.LECarLEAdminControl = iLEAdminControl).FirstOrDefault()
                If Not oLegalEntityCar Is Nothing AndAlso oLegalEntityCar.LECarControl <> 0 Then
                    If If(oLegalEntityCar.LECarAllowCarrierAcceptRejectByEmail, False) Then
                        blnRet = True
                        iExpMin = If(oLegalEntityCar.LECarCarrierAuthCarrierAcceptRejectExpMin, 0)
                        If iExpMin = 0 Then
                            iExpMin = CInt(GetParValue("AutoExpireTenderTokenMin", iCompControl))
                        End If
                    End If
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("AllowLECarAcceptRejectTokenByEmail"), db)
            End Try
        End Using
        Return blnRet

    End Function

    Public Function GetLECarrierWeekendDriveSettings(ByVal CarrierControl As Integer, ByVal CompControl As Integer) As Models.DriveDays
        Dim oRet As New Models.DriveDays()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim oData = db.spGetCarrierDispatchDriveWeekendsByCarrierComp(CarrierControl, CompControl).FirstOrDefault()
                If Not oData Is Nothing Then
                    oRet.Control = oData.Control
                    oRet.DriveSat = oData.WillDriveSaturday
                    oRet.DriveSun = oData.WillDriveSunday
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCompWeekendLoadSettings"), db)
            End Try
        End Using
        Return oRet
    End Function


    ''' <summary>
    ''' Returns the Carrier Specific Uplift value or the Company specific  CarrierCostUpcharge parameter value
    ''' </summary>
    ''' <param name="iCarrierControl"></param>
    ''' <param name="iCompControl"></param>
    ''' <param name="iLEControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created for v-8.5.4.001 on 07/06/2023 for new uplift processing
    ''' </remarks>
    Public Function GetCarrierUpliftValue(ByVal iCarrierControl As Integer, ByVal iCompControl As Integer, Optional ByVal iLEControl As Integer = 0) As Double
        Dim dblRet As Double = 0
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                If iLEControl = 0 Then iLEControl = Me.Parameters.UserLEControl
                ' Lookup carrier Legal Entity Data
                Dim oLECarrier As LTS.tblLegalEntityCarrier = db.tblLegalEntityCarriers.Where(Function(x) x.LECarCarrierControl = iCarrierControl AndAlso x.LECarLEAdminControl = iLEControl).FirstOrDefault()
                If oLECarrier Is Nothing OrElse If(oLECarrier.LECarUpliftUseCarrierSpecific, False) = False Then
                    dblRet = Me.GetParValue("CarrierCostUpcharge", iCompControl)
                Else
                    dblRet = If(oLECarrier.LECarCarrierSpecificUpliftPerc, 0)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierUpliftValue"), db)
            End Try
        End Using

        Return dblRet
    End Function


#End Region

#Region "Protected Functions"

#End Region

End Class