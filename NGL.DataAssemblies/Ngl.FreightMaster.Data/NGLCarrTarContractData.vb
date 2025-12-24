Imports System.Data.Linq
Imports System.Linq.Dynamic
Imports System.Linq.Expressions
Imports System.ServiceModel
Imports Ngl.Core.Utility
Imports Ngl.FreightMaster.Data.DataTransferObjects
Imports Serilog.Events
Imports SerilogTracing

Public Class NGLCarrTarContractData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierTariffs
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrTarContractData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierTariffs
                _LinqDB = db
            End If
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region


    ''' <summary>
    ''' Returns a Read Only Pivot Table of Tariff Data Records
    ''' </summary>
    ''' <param name="CompControl">Origin or Dest comp control used for tariff depending on Inbound or Outbound Tariff</param>
    ''' <param name="Country"></param>
    ''' <param name="State"></param>
    ''' <param name="City"></param>
    ''' <param name="Zip"></param>
    ''' <param name="BookModeTypeControl"></param>
    ''' <param name="TotalWgt"></param>
    ''' <param name="TotalCases"></param>
    ''' <param name="TotalPlts"></param>
    ''' <param name="TotalCubes"></param>
    ''' <param name="BookDateLoad"></param>
    ''' <param name="Inbound"></param>
    ''' <param name="BookLoadCom"></param>
    ''' <param name="LaneControl">Optional Generally 0</param>
    ''' <param name="CarrierControl">Limit the resulsts to a specific carrier 0 = All</param>
    ''' <param name="Prefered">if true limits the seliction to the Lane Prefered carrier settings</param>
    ''' <param name="NoLateDelivery">if true the system will check delivery days with Load and Required Date settings ensuring that the selected carriers can deliver on time</param>
    ''' <param name="Validated">if true only returns carriers that have been pre-qualified based on validation rules</param>
    ''' <param name="OptimizeByCapacity">if true limits the carriers returned by capacity settings in Carrier Equipment (Typically this is On)</param>
    ''' <param name="ModeTypeControl">Limit results by Mode -1 = use booking data, 0 = ALL,1 = Air,2 = Rail,3 = Road,4 = Sea</param>
    ''' <param name="TempType">Limit results by Temperature -1 = use booking data, 0 = ALL, 1 = Dry, 2 = Frzen, 3 = Refrigerated</param>
    ''' <param name="TariffTypeControl">Limit results by Tariff Type 0 = ALL, 1 = Private, 2 = Public</param>
    ''' <param name="CarrTarEquipMatClass">Limit by class A value like 100 pass Null or empty string to ignore</param>
    ''' <param name="CarrTarEquipMatClassTypeControl">Limit results by Class Type 0 = All,1 = 49CFR,2 = IATA,3 = DOT,4 = Marine,5	= NMFC,6 = FAK,7 = NA (If non-zero: use a rate type of 3 -- Class if Rate Type is not provided (0))</param>
    ''' <param name="CarrTarEquipMatTarRateTypeControl">Limit the results by Rate Type 0 = All,1 = Distance M,2 = Distance K,3	= Class,4 = Flat,5 = Unit of Measure,6 = CzarLite</param>
    ''' <param name="AgentControl">Limit the results by the specified Agent 0 = All</param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="Origip"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
    ''' </remarks>
    Public Function GetCarrierTariffsPivot(ByVal CompControl As Integer,
                                           ByVal Country As String,
                                           ByVal State As String,
                                           ByVal City As String,
                                           ByVal Zip As String,
                                           ByVal BookModeTypeControl As Integer,
                                           ByVal TotalWgt As Double,
                                           ByVal TotalCases As Integer,
                                           ByVal TotalPlts As Double,
                                           ByVal TotalCubes As Integer,
                                           ByVal BookDateLoad As Date,
                                           Optional ByVal Inbound As Boolean = True,
                                           Optional ByVal BookLoadCom As String = "D",
                                           Optional ByVal LaneControl As Integer = 0,
                                           Optional ByVal CarrierControl As Integer = 0,
                                           Optional ByVal Prefered As Boolean = True,
                                           Optional ByVal NoLateDelivery As Boolean = False,
                                           Optional ByVal Validated As Boolean = True,
                                           Optional ByVal OptimizeByCapacity As Boolean = True,
                                           Optional ByVal ModeTypeControl As Integer = 0,
                                           Optional ByVal TempType As Integer = 0,
                                           Optional ByVal TariffTypeControl As Integer = 0,
                                           Optional ByVal CarrTarEquipMatClass As String = Nothing,
                                           Optional ByVal CarrTarEquipMatClassTypeControl As Integer = 0,
                                           Optional ByVal CarrTarEquipMatTarRateTypeControl As Integer = 0,
                                           Optional ByVal AgentControl As Integer = 0,
                                           Optional ByVal page As Integer = 1,
                                           Optional ByVal pagesize As Integer = 1000,
                                           Optional ByVal Origip As String = "") As DataTransferObjects.CarrierTariffsPivot()

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                'db.Log = New DebugTextWriter

                Dim oRecords = (
                        From d In db.udfCarrierTariffsPivotQuote(CompControl, Country, State, City, Zip, LaneControl, BookModeTypeControl, TotalWgt, TotalCases, TotalPlts, TotalCubes, Not Inbound, CarrierControl, Prefered, NoLateDelivery, Validated, OptimizeByCapacity, ModeTypeControl, TempType, TariffTypeControl, CarrTarEquipMatClass, CarrTarEquipMatClassTypeControl, CarrTarEquipMatTarRateTypeControl, AgentControl, BookDateLoad, Origip)
                        Select selectDTOCarrierTarrifsPivotData(d, db)
                        ).ToList


                If oRecords Is Nothing Then Return Nothing

                intRecordCount = oRecords.Count
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize
                Dim BPPivots As New Dictionary(Of Integer, CarrTarMatBPPivot)
                Dim oBPData As New NGLCarrTarMatBPData(Me.Parameters)
                Dim CarrierTariffsPivot() As DataTransferObjects.CarrierTariffsPivot = oRecords.Skip(intSkip).Take(pagesize).ToArray()
                For Each pivot In CarrierTariffsPivot
                    If pivot.CarrTarEquipMatCarrTarMatBPControl <> 0 Then
                        If Not BPPivots.ContainsKey(pivot.CarrTarEquipMatCarrTarMatBPControl) Then
                            BPPivots.Add(pivot.CarrTarEquipMatCarrTarMatBPControl, oBPData.GetCarrTarMatBPPivot(pivot.CarrTarEquipMatCarrTarMatBPControl))
                        End If
                        pivot.BPPivot = BPPivots(pivot.CarrTarEquipMatCarrTarMatBPControl)
                    End If
                    pivot.Page = page
                    pivot.Pages = intPageCount
                    pivot.RecordCount = intRecordCount
                    pivot.PageSize = pagesize
                Next

                Return CarrierTariffsPivot

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException()
            Catch ex As Exception
                throwUnExpectedFaultException(ex, getSourceCaller("GetCarrTarEquipMatPivot"))
            End Try

            Return Nothing

        End Using
    End Function

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetCarrTarContractFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    ''' <summary>
    ''' Checks wether the key fields that make a contract unique exists or not.  
    ''' </summary> 
    ''' <param name="oData"></param>
    ''' <returns>true or false.  true is vaild, false is bad</returns>
    ''' <remarks></remarks>
    Public Function IsValidKey(ByRef oData As DataTransferObjects.DTOBaseClass, ByVal throwerrors As Boolean) As Boolean
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            With CType(oData, DataTransferObjects.CarrTarContract)
                Try
                    Dim CarrierTariff = (
                            From t In CType(db, NGLMASCarrierDataContext).CarrierTariffs
                            Where
                            (t.CarrTarCarrierControl = .CarrTarCarrierControl _
                             And
                             t.CarrTarCompControl = .CarrTarCompControl _
                             And
                             t.CarrTarTariffTypeControl = .CarrTarTariffTypeControl _
                             And
                             t.CarrTarTariffModeTypeControl = .CarrTarTariffModeTypeControl _
                             And
                             t.CarrTarID.ToUpper = .CarrTarID.ToUpper _
                             And
                             t.CarrTarRevisionNumber = .CarrTarRevisionNumber)
                            Select t).First

                    If Not CarrierTariff Is Nothing Then
                        If throwerrors Then
                            Utilities.SaveAppError("Cannot save new Contract, there already is a contract setup for this carrier, company, tariff type, mode type, carr tar id, and revision number.", Me.Parameters)
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidUniqueContract"}, New FaultReason("E_InvalidKeyField"))
                        Else
                            Return False
                        End If
                    End If

                Catch ex As FaultException
                    Throw
                Catch ex As InvalidOperationException
                    'do nothing this is the desired result.
                    Return True
                End Try
            End With
        End Using
    End Function

    ''' <summary>
    ''' The carrtarid should be unique across the board.
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <param name="throwerrors"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsIDValid(ByRef oData As DataTransferObjects.DTOBaseClass, ByVal throwerrors As Boolean) As Boolean
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            With CType(oData, DataTransferObjects.CarrTarContract)
                Try
                    Dim CarrierTariffs = (
                            From t In CType(db, NGLMASCarrierDataContext).CarrierTariffs
                            Where
                            (t.CarrTarID = .CarrTarID)
                            Select t).ToArray()

                    If Not CarrierTariffs Is Nothing AndAlso CarrierTariffs.Length >= 1 Then
                        If throwerrors Then
                            Utilities.SaveAppError("Cannot save new Contract, there already is a contract setup with this Car Tar ID.", Me.Parameters)
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_CarrTarIDUnique"}, New FaultReason("E_InvalidKeyField"))
                        Else
                            Return False
                        End If
                    End If

                Catch ex As FaultException
                    Throw
                Catch ex As InvalidOperationException
                    'do nothing this is the desired result.
                    Return True
                End Try
            End With
        End Using
    End Function

    Public Function GetCarrTarContractFiltered(ByVal Control As Integer) As DataTransferObjects.CarrTarContract
        Using Logger.StartActivity("GetCarrTarContractFiltered(Control: {Control})", Control)
            Using db As New NGLMASCarrierDataContext(ConnectionString)
                Try

                    'Dim oDLO As New DataLoadOptions
                    'oDLO.LoadWith(Of LTS.CarrierTariff)(Function(t As LTS.CarrierTariff) t.CarrierTariffBreakPoints)
                    'db.LoadOptions = oDLO

                    'Get the newest record that matches the provided criteria
                    Dim CarrierTariffContract As DataTransferObjects.CarrTarContract = (
                            From d In db.CarrierTariffs
                            Where
                            d.CarrTarControl = Control
                            Select selectDTOData(d, db)).First

                    Return CarrierTariffContract

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

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTarContractFiltered(ByVal carTarID As String) As DataTransferObjects.CarrTarContract
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CarrierTariffContract As DataTransferObjects.CarrTarContract = (
                        From d In db.CarrierTariffs
                        Where
                        d.CarrTarID.ToUpper = carTarID.ToUpper _
                        And d.CarrTarRejected = False
                        Order By d.CarrTarRevisionNumber Descending
                        Select selectDTOData(d, db)).First

                Return CarrierTariffContract

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

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="TempType"></param>
    ''' <param name="EffStartDateFrom"></param>
    ''' <param name="EffStartDateTo"></param>
    ''' <param name="EffEndDateFrom"></param>
    ''' <param name="EffEndDateTo"></param>
    ''' <param name="TariffType"></param>
    ''' <param name="ModeType"></param>
    ''' <param name="Revision"></param>
    ''' <param name="ShowActiveContracts"></param>
    ''' <param name="Approved"></param>
    ''' <param name="Rejected"></param>
    ''' <param name="Outbound"></param>
    ''' <param name="AgentControl"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 12/14/2018
    '''     we now filter by LE Carriers
    ''' </remarks>
    Public Function GetCarrTarContractsFiltered(ByVal CarrierControl As Integer,
                                                ByVal CompControl As Integer,
                                                ByVal TempType As Integer,
                                                ByVal EffStartDateFrom As System.Nullable(Of Date),
                                                ByVal EffStartDateTo As System.Nullable(Of Date),
                                                ByVal EffEndDateFrom As System.Nullable(Of Date),
                                                ByVal EffEndDateTo As System.Nullable(Of Date),
                                                ByVal TariffType As Integer,
                                                ByVal ModeType As Integer,
                                                ByVal Revision As Integer,
                                                ByVal ShowActiveContracts As Boolean,
                                                ByVal Approved As Boolean,
                                                ByVal Rejected As Boolean,
                                                ByVal Outbound As Boolean,
                                                ByVal AgentControl As Integer,
                                                Optional ByVal page As Integer = 1,
                                                Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.CarrTarContract()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try


                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefCarriers Where s.UserAdminUserName = Me.Parameters.UserName Select CompanyControl = s.CompControl
                Dim oSecureCarrier = (From s In db.vSecureCarriers Where s.UserAdminUserName = Me.Parameters.UserName Select s.CarrTarCarrierControl).Distinct.ToList()
                Dim dtNow As Date = Date.Now.ToShortDateString()
                If EffStartDateFrom.HasValue Then
                    EffStartDateFrom = DataTransformation.formatStartDateFilter(EffStartDateFrom)
                End If
                If EffStartDateTo.HasValue Then
                    EffStartDateTo = DataTransformation.formatEndDateFilter(EffStartDateTo)
                End If

                If EffEndDateFrom.HasValue Then
                    EffEndDateFrom = DataTransformation.formatStartDateFilter(EffEndDateFrom)
                End If
                If EffEndDateTo.HasValue Then
                    EffEndDateTo = DataTransformation.formatEndDateFilter(EffEndDateTo)
                End If

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                Dim oContracts = From d In db.CarrierTariffs
                                 Where
                                 (
                                     (CarrierControl = 0 OrElse d.CarrTarCarrierControl = CarrierControl) _
                                     And
                                     (oSecureCarrier Is Nothing OrElse oSecureCarrier.Count = 0 OrElse oSecureCarrier.Contains(d.CarrTarCarrierControl))
                                     ) _
                                 And
                                 (
                                     (CompControl = 0 AndAlso (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.CarrTarCompControl))) _
                                     Or
                                     (CompControl <> 0 AndAlso d.CarrTarCompControl = CompControl)
                                     ) _
                                 And
                                 (TempType = 0 OrElse d.CarrTarTempType = TempType) _
                                 And
                                 (Not EffStartDateFrom.HasValue OrElse If(d.CarrTarEffDateFrom.HasValue, d.CarrTarEffDateFrom >= EffStartDateFrom, False)) _
                                 And
                                 (Not EffStartDateTo.HasValue OrElse If(d.CarrTarEffDateFrom.HasValue, d.CarrTarEffDateFrom <= EffStartDateTo, False)) _
                                 And
                                 (Not EffEndDateFrom.HasValue OrElse If(d.CarrTarEffDateTo.HasValue, d.CarrTarEffDateTo >= EffEndDateFrom, True)) _
                                 And
                                 (Not EffEndDateTo.HasValue OrElse If(d.CarrTarEffDateTo.HasValue, d.CarrTarEffDateTo <= EffEndDateTo, True)) _
                                 And
                                 (TariffType = 0 OrElse d.CarrTarTariffTypeControl = TariffType) _
                                 And
                                 (ModeType = 0 OrElse d.CarrTarTariffModeTypeControl = ModeType) _
                                 And
                                 (Revision = 0 OrElse d.CarrTarRevisionNumber = Revision) _
                                 And
                                 (d.CarrTarApproved = Approved) _
                                 And
                                 (d.CarrTarRejected = Rejected) _
                                 And
                                 (d.CarrTarOutbound = Outbound) _
                                 And
                                 (AgentControl = 0 OrElse d.CarrTarAgentControl = AgentControl)
                                 Select d

                Dim CarrierTariffContracts() As DataTransferObjects.CarrTarContract = Nothing

                intRecordCount = oContracts.Count
                If intRecordCount < 1 Then Return Nothing
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                If ShowActiveContracts Then
                    CarrierTariffContracts = (
                        From d In oContracts Where (d.CarrTarEffDateTo.HasValue = False OrElse d.CarrTarEffDateTo.Value > dtNow) Order By d.CarrTarID, d.CarrTarRevisionNumber Descending
                        Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Else
                    CarrierTariffContracts = (
                        From d In oContracts Order By d.CarrTarControl
                        Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                End If

                Return CarrierTariffContracts

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

    ''' <summary>
    ''' Get carrier tariff contract information using the AllFilters data.
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 06/25/2018
    '''   new LTS carrier tariff query  
    ''' </remarks>
    'Public Function GetCarrTarContracts(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCarrierContract()
    '    If filters Is Nothing Then Return Nothing
    '    Dim oRet() As LTS.vCarrierContract

    '    'Dim intCompNumberFrom As Integer = 0
    '    'Dim intCompNumberTo As Integer = 0
    '    Using db As New NGLMASCarrierDataContext(ConnectionString)
    '        Try
    '            Dim oSecureComp = (From s In db.vUserAdminWithCompControlRefCarriers Where s.UserAdminUserName = Me.Parameters.UserName Select CompanyControl = s.CompControl).ToList()
    '            'TODO:  We need to add logic to get all the carriers visible to this user not just this users legal entity.
    '            '       A new stored procedure or view could combine vUserAdminWithCompControl logic with tblCarrierLegalAccessorialXrefs
    '            '       this will work for testing and for single tennant implementations

    '            'Dim oSecureCarrier = (From s In db.tblCarrierLegalAccessorialXrefs Where s.CLAXLEAdminControl = Me.Parameters.UserLEControl Select CarrierControl = s.CLAXCarrierControl).Distinct.ToList()
    '            Dim oSecureCarrier = (From s In db.vSecureCarriers Where s.UserAdminUserName = Me.Parameters.UserName Select CarrierControl = s.CarrTarCarrierControl).Distinct.ToList()

    '            Dim lCompControl = convertCompNumbersToControl(db, "CarrTarCompControl", filters, oSecureComp)
    '            Dim lCarrierControl = convertCarrierNumbersToControl(db, "CarrTarCarrierControl", filters, oSecureCarrier)
    '            Dim blnTestUserCompRestrictions As Boolean = False
    '            If Not oSecureComp Is Nothing AndAlso oSecureComp.Count > 0 Then blnTestUserCompRestrictions = True
    '            Dim iQuery As IQueryable(Of LTS.vCarrierContract)
    '            iQuery = db.vCarrierContracts
    '            Dim filterWhere As String = ""
    '            Dim sFilterSpacer As String = ""

    '            If blnTestUserCompRestrictions AndAlso (lCompControl Is Nothing OrElse lCompControl.Count < 1) Then
    '                If Not oSecureComp Is Nothing AndAlso oSecureComp.Count > 0 Then
    '                    Dim strCompControlSpacer As String = ""
    '                    filterWhere = "("
    '                    For Each i In oSecureComp
    '                        'dicCompControl.Add(i, i)
    '                        'For Each d In dicCompControl
    '                        filterWhere &= strCompControlSpacer & " ( CarrTarCompControl = " & i.ToString() & ") "
    '                        strCompControlSpacer = " Or "
    '                        'Next
    '                    Next
    '                    filterWhere &= ")"
    '                    sFilterSpacer = " And "
    '                End If
    '            Else
    '                If Not lCompControl Is Nothing AndAlso lCompControl.Count > 0 Then
    '                    Dim strCompControlSpacer As String = ""
    '                    filterWhere = "("
    '                    For Each d In lCompControl
    '                        filterWhere &= strCompControlSpacer & " ( CarrTarCompControl =  " & d.ToString() & ") "
    '                        strCompControlSpacer = " Or "
    '                    Next
    '                    filterWhere &= ")"
    '                    sFilterSpacer = " And "
    '                End If
    '            End If
    '            Dim blnTestUserCarrierRestrictions As Boolean = False
    '            If Not oSecureCarrier Is Nothing AndAlso oSecureCarrier.Count > 0 Then blnTestUserCarrierRestrictions = True
    '            If blnTestUserCarrierRestrictions AndAlso (lCarrierControl Is Nothing OrElse lCarrierControl.Count < 1) Then
    '                If Not oSecureCarrier Is Nothing AndAlso oSecureCarrier.Count > 0 Then
    '                    Dim strCarrierControlSpacer As String = ""
    '                    filterWhere &= sFilterSpacer & "("
    '                    For Each i In oSecureCarrier
    '                        filterWhere &= strCarrierControlSpacer & " ( CarrTarCarrierControl = " & i.ToString() & " ) "
    '                        strCarrierControlSpacer = " Or "
    '                    Next
    '                    filterWhere &= ")"
    '                    sFilterSpacer = " And "
    '                End If
    '            Else
    '                If Not lCarrierControl Is Nothing AndAlso lCarrierControl.Count > 0 Then
    '                    Dim strCarrierControlSpacer As String = ""
    '                    filterWhere &= sFilterSpacer & "("
    '                    For Each d In lCarrierControl
    '                        filterWhere &= strCarrierControlSpacer & " ( CarrTarCarrierControl =  " & d.ToString() & " )  "
    '                        strCarrierControlSpacer = " Or "
    '                    Next
    '                    filterWhere &= ")"
    '                    sFilterSpacer = " And "
    '                End If
    '            End If
    '            If String.IsNullOrWhiteSpace(filters.sortName) Then
    '                filters.sortName = "CarrTarEffDateFrom"
    '                filters.sortDirection = "desc"
    '            End If
    '            Dim blnActiveOnly = True
    '            If filters.FilterValues.Any(Function(x) x.filterName = "ContractActive") Then
    '                Dim sActiveFilter = filters.FilterValues.Where(Function(x) x.filterName = "ContractActive").FirstOrDefault()
    '                If (Not sActiveFilter Is Nothing) Then
    '                    If Not String.IsNullOrEmpty(sActiveFilter.filterValueFrom) AndAlso sActiveFilter.filterValueFrom.ToUpper().Contains("INACTIVE") Then
    '                        filterWhere &= sFilterSpacer & " ( CarrTarActiveFlag = false  )  "
    '                        blnActiveOnly = False
    '                    ElseIf Not String.IsNullOrEmpty(sActiveFilter.filterValueTo) AndAlso sActiveFilter.filterValueTo.ToUpper().Contains("INACTIVE") Then
    '                        filterWhere &= sFilterSpacer & " ( CarrTarActiveFlag = false  )  "
    '                        blnActiveOnly = False
    '                    End If
    '                End If
    '            Else
    '                If (filters.FilterValues.Any(Function(x) x.filterName = "CarrTarControl" AndAlso Not String.IsNullOrWhiteSpace(x.filterValueFrom))) Then
    '                    blnActiveOnly = False
    '                End If
    '            End If
    '            If blnActiveOnly = True AndAlso filters.FilterValues.Any(Function(x) x.filterName = "CarrTarActiveFlag") Then
    '                Dim sActiveFilter = filters.FilterValues.Where(Function(x) x.filterName = "CarrTarActiveFlag").FirstOrDefault()
    '                If (Not sActiveFilter Is Nothing) Then
    '                    If Not String.IsNullOrEmpty(sActiveFilter.filterValueFrom) AndAlso (sActiveFilter.filterValueFrom.ToUpper().Contains("FALSE") Or sActiveFilter.filterValueFrom.ToUpper().Contains("0")) Then
    '                        filterWhere &= sFilterSpacer & " ( CarrTarActiveFlag = false  )  "
    '                        blnActiveOnly = False
    '                    ElseIf Not String.IsNullOrEmpty(sActiveFilter.filterValueTo) AndAlso (sActiveFilter.filterValueTo.ToUpper().Contains("FALSE") Or sActiveFilter.filterValueTo.ToUpper().Contains("0")) Then
    '                        filterWhere &= sFilterSpacer & " ( CarrTarActiveFlag = false  )  "
    '                        blnActiveOnly = False
    '                    End If
    '                End If
    '            End If
    '            If blnActiveOnly = True Then
    '                filterWhere &= sFilterSpacer & " ( CarrTarActiveFlag = true  )  "
    '            End If


    '            ApplyAllFilters(iQuery, filters, filterWhere)
    '            PrepareQuery(iQuery, filters, RecordCount)
    '            db.Log = New DebugTextWriter
    '            oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
    '        Catch ex As Exception
    '            ManageLinqDataExceptions(ex, buildProcedureName("GetCarrTarContracts"), db)
    '        End Try
    '    End Using
    '    Return oRet
    'End Function



    'Public Function GetCarrTarContracts(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCarrierContract()
    '    If filters Is Nothing Then Return Nothing

    '    Using db As New NGLMASCarrierDataContext(ConnectionString)
    '        Try
    '            ' --- Secure company and carrier access ---
    '            Dim userName = Me.Parameters.UserName

    '            Dim secureComps = (From s In db.vUserAdminWithCompControlRefCarriers
    '                               Where s.UserAdminUserName = userName
    '                               Select s.CompControl).ToList()

    '            Dim secureCarriers = (From s In db.vSecureCarriers
    '                                  Where s.UserAdminUserName = userName
    '                                  Select s.CarrTarCarrierControl).Distinct().ToList()

    '            ' --- Convert filters ---
    '            Dim compFilter = convertCompNumbersToControl(db, "CarrTarCompControl", filters, secureComps)
    '            Dim carrierFilter = convertCarrierNumbersToControl(db, "CarrTarCarrierControl", filters, secureCarriers)

    '            ' --- Start query ---
    '            Dim query = db.vCarrierContracts.AsQueryable()

    '            ' --- Company restriction ---
    '            If compFilter?.Any() Then
    '                query = query.Where(Function(x) compFilter.Contains(x.CarrTarCompControl))
    '            ElseIf secureComps?.Any() Then
    '                query = query.Where(Function(x) secureComps.Contains(x.CarrTarCompControl))
    '            End If

    '            ' --- Carrier restriction ---
    '            If carrierFilter?.Any() Then
    '                query = query.Where(Function(x) carrierFilter.Contains(x.CarrTarCarrierControl))
    '            ElseIf secureCarriers?.Any() Then
    '                query = query.Where(Function(x) secureCarriers.Contains(x.CarrTarCarrierControl))
    '            End If

    '            ' --- Active/Inactive filters ---
    '            Dim activeFlag = True
    '            Dim activeFilter = filters.FilterValues.FirstOrDefault(Function(f) f.filterName = "ContractActive" OrElse f.filterName = "CarrTarActiveFlag")

    '            If activeFilter IsNot Nothing Then
    '                Dim val = (activeFilter.filterValueFrom & activeFilter.filterValueTo).ToUpperInvariant()
    '                If val.Contains("INACTIVE") OrElse val.Contains("FALSE") OrElse val.Contains("0") Then
    '                    query = query.Where(Function(x) x.CarrTarActiveFlag = False)
    '                    activeFlag = False
    '                End If
    '            End If

    '            If activeFlag Then
    '                query = query.Where(Function(x) x.CarrTarActiveFlag = True)
    '            End If

    '            ' --- Apply generic filters dynamically ---
    '            ApplyAllFilters(query, filters, "")

    '            ' --- Sorting ---
    '            If String.IsNullOrWhiteSpace(filters.sortName) Then
    '                filters.sortName = "CarrTarEffDateFrom"
    '                filters.sortDirection = "desc"
    '            End If

    '            query = OrderByProperty(query, filters.sortName, filters.sortDirection)

    '            '    query = If(filters.sortDirection.ToLower() = "desc",
    '            '    query.OrderByDescending(Function(x) GetPropertyValue(x, filters.sortName)),
    '            '    query.OrderBy(Function(x) GetPropertyValue(x, filters.sortName))
    '            ')

    '            ' --- Paging ---
    '            RecordCount = query.Count()
    '            Return query.Skip(filters.skip).Take(filters.take).ToArray()

    '        Catch ex As Exception
    '            ManageLinqDataExceptions(ex, buildProcedureName("GetCarrTarContracts"), db)
    '            Return Nothing
    '        End Try
    '    End Using
    'End Function



    ''' <summary>
    ''' Get carrier tariff contract information using the AllFilters data.
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by Ayman for v-10 on 21/10/2025
    '''   new LTS carrier tariff query  
    ''' </remarks>
    Public Function GetCarrTarContracts(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCarrierContract()
        If filters Is Nothing Then Return Nothing

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim userName = Me.Parameters.UserName

                ' --- Secure companies for this user ---
                Dim secureComps = From s In db.vUserAdminWithCompControlRefCarriers
                                  Where s.UserAdminUserName = userName
                                  Select s.CompControl

                ' --- Secure carriers for this user ---
                Dim secureCarriers = (From s In db.vSecureCarriers
                                      Where s.UserAdminUserName = userName
                                      Select s.CarrTarCarrierControl).Distinct()

                ' --- Start query: INNER JOIN to restrict by CompControl ---
                Dim query = From c In db.vCarrierContracts
                            Join sc In secureComps On c.CarrTarCompControl Equals sc
                            Select c

                ' --- Carrier restriction (optional) ---
                If secureCarriers.Any() Then
                    query = query.Where(Function(x) secureCarriers.Contains(x.CarrTarCarrierControl))
                End If

                ' --- Active/Inactive filters ---
                Dim activeFlag = True
                Dim activeFilter = filters.FilterValues.FirstOrDefault(Function(f) f.filterName = "ContractActive" OrElse f.filterName = "CarrTarActiveFlag")

                If activeFilter IsNot Nothing Then
                    Dim val = (activeFilter.filterValueFrom & activeFilter.filterValueTo).ToUpperInvariant()
                    If val.Contains("INACTIVE") OrElse val.Contains("FALSE") OrElse val.Contains("0") Then
                        query = query.Where(Function(x) x.CarrTarActiveFlag = False)
                        activeFlag = False
                    End If
                End If

                If activeFlag Then
                    query = query.Where(Function(x) x.CarrTarActiveFlag = True)
                End If

                ' --- Apply dynamic filters ---
                ApplyAllFilters(query, filters, "")

                ' --- Sorting ---
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrTarEffDateFrom"
                    filters.sortDirection = "desc"
                End If

                query = OrderByProperty(query, filters.sortName, filters.sortDirection)

                ' --- Paging ---
                RecordCount = query.Count()

                If query.Count = 1 Then
                    Return query.ToArray()
                End If

                Return query.Skip(filters.skip).Take(filters.take).ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrTarContracts"), db)
                Return Nothing
            End Try
        End Using
    End Function


    Private Function GetPropertyValue(Of T)(ByVal obj As T, ByVal propName As String) As Object
        Return obj.GetType().GetProperty(propName).GetValue(obj, Nothing)
    End Function

    Private Function OrderByProperty(Of T)(source As IQueryable(Of T), propertyName As String, direction As String) As IQueryable(Of T)
        Dim param = Expression.Parameter(GetType(T), "x")
        Dim prop = Expression.PropertyOrField(param, propertyName)
        Dim lambda = Expression.Lambda(prop, param)

        Dim methodName = If(direction.ToLower() = "desc", "OrderByDescending", "OrderBy")
        Dim resultExp = Expression.Call(
        GetType(Queryable),
        methodName,
        {GetType(T), prop.Type},
        source.Expression,
        Expression.Quote(lambda)
    )

        Return source.Provider.CreateQuery(Of T)(resultExp)
    End Function


    ''' <summary>
    ''' Insert or Update the carrier tariff contract data
    ''' </summary>
    ''' <param name="contract"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/16/2018
    ''' Modified by RHR for v-8.5.3.006 on 10/04/2022 added CarrTarAutoAssignPro and user 1 to 4 data fields
    ''' </remarks>
    Public Function SaveCarrTarContract(ByVal contract As LTS.vCarrierContract) As Boolean
        'Name, Tariff Type, Mode, Effective From, Effective To, Def Wgt, Drive Saturday, Drive Sunday
        Dim blnRet As Boolean = False
        If contract Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                If contract.CarrTarControl = 0 Then
                    If db.CarrierTariffs.Any(Function(t) t.CarrTarCarrierControl = contract.CarrTarCarrierControl And t.CarrTarCompControl = contract.CarrTarCompControl And t.CarrTarTariffTypeControl = contract.CarrTarTariffTypeControl And t.CarrTarTariffModeTypeControl = contract.CarrTarTariffModeTypeControl And t.CarrTarID.ToUpper = contract.CarrTarID.ToUpper And t.CarrTarRevisionNumber = contract.CarrTarRevisionNumber) Then
                        Utilities.SaveAppError("Cannot save new Contract, a contract already exists for this carrier, company, tariff type, mode type, carr tar id, and revision number.", Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidUniqueContract"}, New FaultReason("E_InvalidKeyField"))

                    End If
                    Dim oNew As New LTS.CarrierTariff()
                    Dim strMSG As String = ""
                    Dim skipObjs As New List(Of String) From {"CarrTarModUser", "CarrTarModDate", "CarrTarUpdated"}
                    oNew = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(oNew, contract, skipObjs, strMSG)
                    't = CopyMatchingFields(t, d, skipObjs)
                    With oNew
                        .CarrTarModDate = Date.Now
                        .CarrTarModUser = Me.Parameters.UserName
                        'check for other fields not normally part of the UI
                        If (Not .CarrTarIssuedDate.HasValue) Then .CarrTarIssuedDate = Date.Now
                        If (Not .CarrTarApprovedDate.HasValue) Then
                            .CarrTarApprovedDate = Date.Now
                            .CarrTarApprovedBy = "Auto"
                            .CarrTarApproved = True
                        End If
                        If (.CarrTarRevisionNumber = 0) Then .CarrTarRevisionNumber = 1
                        If (.CarrTarOutbound = True) Then
                            .CarrTarTariffType = "O"
                        Else
                            .CarrTarTariffType = "I"
                        End If
                    End With
                    If Debugger.IsAttached AndAlso Not String.IsNullOrWhiteSpace(strMSG) Then
                        System.Diagnostics.Debug.WriteLine(strMSG)
                    End If
                    db.CarrierTariffs.InsertOnSubmit(oNew)
                Else
                    Dim oExisting = db.CarrierTariffs.Where(Function(x) x.CarrTarControl = contract.CarrTarControl).FirstOrDefault()
                    If oExisting Is Nothing OrElse oExisting.CarrTarControl = 0 Then
                        throwRecordDeletedFaultException("Cannot save changes for tariff contract: " & contract.CarrTarName)
                    End If
                    With oExisting
                        .CarrTarName = contract.CarrTarName
                        .CarrTarTariffTypeControl = contract.CarrTarTariffTypeControl
                        .CarrTarTariffModeTypeControl = contract.CarrTarTariffModeTypeControl
                        .CarrTarEffDateFrom = contract.CarrTarEffDateFrom
                        .CarrTarEffDateTo = contract.CarrTarEffDateTo
                        .CarrTarDefWgt = contract.CarrTarDefWgt
                        .CarrTarWillDriveSaturday = contract.CarrTarWillDriveSaturday
                        .CarrTarWillDriveSunday = contract.CarrTarWillDriveSunday
                        .CarrTarModUser = Me.Parameters.UserName
                        .CarrTarModDate = Date.Now
                        .CarrTarAutoAssignPro = contract.CarrTarAutoAssignPro
                        .CarrTarUser1 = contract.CarrTarUser1
                        .CarrTarUser2 = contract.CarrTarUser2
                        .CarrTarUser3 = contract.CarrTarUser3
                        .CarrTarUser4 = contract.CarrTarUser4
                    End With
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCarrTarContract"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete the provided carrier tariff contract
    ''' </summary>
    ''' <param name="iCarrTarControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 08/16/2018
    ''' </remarks>
    Public Function DeleteCarrTarContract(ByVal iCarrTarControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iCarrTarControl = 0 Then Return False 'nothing to do
        'validate before delete

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Delete the Record
                Dim oToDelete = db.CarrierTariffs.Where(Function(x) x.CarrTarControl = iCarrTarControl).FirstOrDefault()
                If oToDelete Is Nothing OrElse oToDelete.CarrTarControl = 0 Then Return True 'already deleted
                Using Bookdb As New NGLMasBookDataContext(ConnectionString)
                    If Bookdb.Books.Any(Function(x) x.BookCarrTarControl = iCarrTarControl) Then
                        throwCannotDeleteRecordInUseException("Tariff Name", oToDelete.CarrTarName)
                    End If
                End Using
                db.CarrierTariffs.DeleteOnSubmit(oToDelete)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrTarContract"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function GetCarrTarContractSummary(ByVal iCarrTarControl As Integer) As LTS.vCarrierTariffSummary

        Dim oRet As LTS.vCarrierTariffSummary

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                oRet = db.vCarrierTariffSummaries.Where(Function(x) x.CarrTarControl = iCarrTarControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrTarContractSummary"), db)
            End Try
        End Using
        Return oRet
    End Function

    Public Function GetCarrTarContractsFilteredSimple(ByVal CarrierControl As Integer,
                                                      ByVal CompControl As Integer,
                                                      Optional ByVal page As Integer = 1,
                                                      Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.CarrTarContract()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try


                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                'Dim oAppts = (From d In db.AMSAppointments _
                '              Where d.AMSApptCompControl = CompControl _
                '              And (If(d.AMSApptStartDate.HasValue, d.AMSApptStartDate >= DTran.formatStartDateFilter(StartDate) And d.AMSApptStartDate <= DTran.formatEndDateFilter(EndDate), False)) Select d.AMSApptControl).ToArray()
                'If Not oAppts Is Nothing Then intRecordCount = oAppts.Count
                'db.Log = New DebugTextWriter

                Dim oContracts = From d In db.CarrierTariffs
                                 Where
                                 (CarrierControl = 0 OrElse d.CarrTarCarrierControl = CarrierControl) _
                                 And
                                 (CompControl = 0 OrElse d.CarrTarCompControl = CompControl)
                                 Select d

                intRecordCount = oContracts.Count
                If intRecordCount < 1 Then Return Nothing
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize


                'Dim oDLO As New DataLoadOptions
                'oDLO.LoadWith(Of LTS.CarrierTariff)(Function(t As LTS.CarrierTariff) t.CarrierTariffBreakPoints)
                'db.LoadOptions = oDLO
                Dim CarrierTariffContracts() As DataTransferObjects.CarrTarContract = (
                        From d In oContracts Order By d.CarrTarControl
                        Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()


                Return CarrierTariffContracts

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

    Public Function GetTariffShippersByCarrier(ByVal sortKey As Integer, ByVal CarrierControl As Integer) As DataTransferObjects.vComp()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim vList As DataTransferObjects.vComp()
                Dim oCarrierComps = From c In db.CarrierTariffs Where c.CarrTarCarrierControl = CarrierControl Select c.CarrTarCompControl
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefCarriers Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Select Case sortKey
                    Case 1
                        vList = (
                            From t In db.CompRefCarriers
                            Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl)) _
                                  And (oCarrierComps Is Nothing OrElse oCarrierComps.Count = 0 OrElse oCarrierComps.Contains(t.CompControl))
                            Order By t.CompName
                            Select New DataTransferObjects.vComp _
                                With {.CompControl = t.CompControl, .CompName = t.CompName, .CompNumber = t.CompNumber, .CompAbrev = t.CompAbrev, .CompStreetAddress1 = t.CompStreetAddress1, .CompStreetCity = t.CompStreetCity, .CompStreetZip = t.CompStreetZip, .CompStreetState = t.CompStreetState, .CompStreetCountry = t.CompStreetCountry}).ToArray()
                    Case 2
                        vList = (
                            From t In db.CompRefCarriers
                            Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl)) _
                                  And (oCarrierComps Is Nothing OrElse oCarrierComps.Count = 0 OrElse oCarrierComps.Contains(t.CompControl))
                            Order By t.CompStreetCity
                            Select New DataTransferObjects.vComp _
                                With {.CompControl = t.CompControl, .CompName = t.CompName, .CompNumber = t.CompNumber, .CompAbrev = t.CompAbrev, .CompStreetAddress1 = t.CompStreetAddress1, .CompStreetCity = t.CompStreetCity, .CompStreetZip = t.CompStreetZip, .CompStreetState = t.CompStreetState, .CompStreetCountry = t.CompStreetCountry}).ToArray()
                    Case Else
                        vList = (
                            From t In db.CompRefCarriers
                            Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl)) _
                                  And (oCarrierComps Is Nothing OrElse oCarrierComps.Count = 0 OrElse oCarrierComps.Contains(t.CompControl))
                            Select New DataTransferObjects.vComp _
                                With {.CompControl = t.CompControl, .CompName = t.CompName, .CompNumber = t.CompNumber, .CompAbrev = t.CompAbrev, .CompStreetAddress1 = t.CompStreetAddress1, .CompStreetCity = t.CompStreetCity, .CompStreetZip = t.CompStreetZip, .CompStreetState = t.CompStreetState, .CompStreetCountry = t.CompStreetCountry}).ToArray()
                End Select
                Return vList
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


    Public Function GetTariffShippersByCarrierAndZip(ByVal sortKey As Integer, ByVal CarrierControl As Integer, ByVal PostalCode As String, ByVal Take As Integer) As DataTransferObjects.CityStateZipTariffLookup()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim vList As DataTransferObjects.CityStateZipTariffLookup()
                Dim oCarrierComps = From c In db.CarrierTariffs Where c.CarrTarCarrierControl = CarrierControl Select c.CarrTarCompControl
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefCarriers Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Select Case sortKey
                    Case 1
                        vList = (
                            From t In db.CompRefCarriers
                            Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl)) _
                                  And (oCarrierComps Is Nothing OrElse oCarrierComps.Count = 0 OrElse oCarrierComps.Contains(t.CompControl)) _
                                  And (t.CompStreetZip.ToUpper.StartsWith(PostalCode.ToUpper))
                            Order By t.CompName
                            Select DataTransferObjects.CityStateZipTariffLookup.selectDTO(t)).Take(Take).ToArray()
                    Case 2
                        vList = (
                            From t In db.CompRefCarriers
                            Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl)) _
                                  And (oCarrierComps Is Nothing OrElse oCarrierComps.Count = 0 OrElse oCarrierComps.Contains(t.CompControl)) _
                                  And (t.CompStreetZip.ToUpper.StartsWith(PostalCode.ToUpper))
                            Order By t.CompStreetCity
                            Select DataTransferObjects.CityStateZipTariffLookup.selectDTO(t)).Take(Take).ToArray()
                    Case Else
                        vList = (
                            From t In db.CompRefCarriers
                            Where (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl)) _
                                  And (oCarrierComps Is Nothing OrElse oCarrierComps.Count = 0 OrElse oCarrierComps.Contains(t.CompControl)) _
                                  And (t.CompStreetZip.ToUpper.StartsWith(PostalCode.ToUpper))
                            Order By t.CompControl Descending
                            Select DataTransferObjects.CityStateZipTariffLookup.selectDTO(t)).Take(Take).ToArray()
                End Select
                Return vList
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

    Public Function GetTariffShippers(ByVal sortKey As Integer) As DataTransferObjects.vComp()
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim vList As DataTransferObjects.vComp()
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefComps Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Select Case sortKey
                    Case 1
                        vList = (
                            From t In db.Comps
                            Where t.CompActive = True _
                                  And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl))
                            Order By t.CompName
                            Select New DataTransferObjects.vComp _
                                With {.CompControl = t.CompControl, .CompName = t.CompName, .CompNumber = t.CompNumber, .CompAbrev = t.CompAbrev, .CompStreetAddress1 = t.CompStreetAddress1, .CompStreetCity = t.CompStreetCity, .CompStreetZip = t.CompStreetZip, .CompStreetState = t.CompStreetState, .CompStreetCountry = t.CompStreetCountry}).ToArray()
                    Case 2
                        vList = (
                            From t In db.Comps
                            Where t.CompActive = True _
                                  And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl))
                            Order By t.CompStreetCity
                            Select New DataTransferObjects.vComp _
                                With {.CompControl = t.CompControl, .CompName = t.CompName, .CompNumber = t.CompNumber, .CompAbrev = t.CompAbrev, .CompStreetAddress1 = t.CompStreetAddress1, .CompStreetCity = t.CompStreetCity, .CompStreetZip = t.CompStreetZip, .CompStreetState = t.CompStreetState, .CompStreetCountry = t.CompStreetCountry}).ToArray()
                    Case Else
                        vList = (
                            From t In db.Comps
                            Where t.CompActive = True _
                                  And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(t.CompControl))
                            Select New DataTransferObjects.vComp _
                                With {.CompControl = t.CompControl, .CompName = t.CompName, .CompNumber = t.CompNumber, .CompAbrev = t.CompAbrev, .CompStreetAddress1 = t.CompStreetAddress1, .CompStreetCity = t.CompStreetCity, .CompStreetZip = t.CompStreetZip, .CompStreetState = t.CompStreetState, .CompStreetCountry = t.CompStreetCountry}).ToArray()
                End Select
                Return vList
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

    Public Function GetTree(ByVal Control As Integer) As DataTransferObjects.CarrTarTree
        Try
            Dim intNextTreeID As Integer = 0
            Dim intParentID As Integer = 0
            Dim intNestedParentID As Integer = 0
            Dim oTree As New DataTransferObjects.CarrTarTree With {.CarrTarControl = Control, .TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Root", .Description = "Carrier Contract Tree"}
            intParentID = intNextTreeID
            'set up node level 1 (Exceptions, Equipment, Fees and Fuel)
            Dim oExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Exceptions", .Description = "Exception and Cross Reference Details"}
            Dim oEquipment As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Equipment", .Description = "Equipment Details"}
            Dim oFees As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Fees", .Description = "Accessorial Fee Details"}
            Dim oFuelCosts As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Fuel", .Description = "Fuel Details"}
            '--------------------------------------------------------------------------------
            'set up the exception nodes
            intParentID = oExceptions.TreeID
            Dim oClassExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Class", .Description = "Class Code Cross Reference"}
            'Load Class Exception Children
            intNestedParentID = oClassExceptions.TreeID
            oClassExceptions.Children = DirectCast(NDPBaseClassFactory("NGLCarrTarClassXrefData", False), NGLCarrTarClassXrefData).GetCarrTarClassXrefTree(Control, intNestedParentID, intNextTreeID)
            If oExceptions.Children Is Nothing Then oExceptions.Children = New List(Of NGLTreeNode)
            oExceptions.Children.Add(oClassExceptions)

            Dim oDiscountExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Discount", .Description = "Rating Discount Configuration Details"}
            'Load Discount Exception Children
            intNestedParentID = oDiscountExceptions.TreeID
            oDiscountExceptions.Children = DirectCast(NDPBaseClassFactory("NGLCarrTarDiscountData", False), NGLCarrTarDiscountData).GetCarrTarDiscountTree(Control, intNestedParentID, intNextTreeID)
            oExceptions.Children.Add(oDiscountExceptions)

            Dim oInterlineExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Interline", .Description = "Interline Point Details"}
            'Load Interline Exception Children
            intNestedParentID = oInterlineExceptions.TreeID
            oInterlineExceptions.Children = DirectCast(NDPBaseClassFactory("NGLCarrTarInterlineData", False), NGLCarrTarInterlineData).GetCarrTarInterlineTree(Control, intNestedParentID, intNextTreeID)
            oExceptions.Children.Add(oInterlineExceptions)

            Dim oMinChargeExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "MinCharge", .Description = "Minimum Charge Configuration Details"}
            'Load MinCharge Exception Children
            intNestedParentID = oMinChargeExceptions.TreeID
            oMinChargeExceptions.Children = DirectCast(NDPBaseClassFactory("NGLCarrTarMinChargeData", False), NGLCarrTarMinChargeData).GetCarrTarMinChargeTree(Control, intNestedParentID, intNextTreeID)
            oExceptions.Children.Add(oMinChargeExceptions)

            Dim oNonServiceExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "NonService", .Description = "Non Service Point Configuration Details"}
            'Add Code to load NonService Exception Children
            intNestedParentID = oNonServiceExceptions.TreeID
            oNonServiceExceptions.Children = DirectCast(NDPBaseClassFactory("NGLCarrTarNonServData", False), NGLCarrTarNonServData).GetCarrTarNonServTree(Control, intNestedParentID, intNextTreeID)
            oExceptions.Children.Add(oNonServiceExceptions)

            If oTree.Children Is Nothing Then oTree.Children = New List(Of NGLTreeNode)
            oTree.Children.Add(oExceptions)
            '----------------------------------------------------------------------------------
            'set up the equipment nodes
            intParentID = oEquipment.TreeID
            oEquipment.Children = DirectCast(NDPBaseClassFactory("NGLCarrTarEquipData", False), NGLCarrTarEquipData).GetCarrTarEquipTree(Control, intParentID, intNextTreeID)
            oTree.Children.Add(oEquipment)
            '----------------------------------------------------------------------------------
            'Add Code to load Fee Children for now just add sample data
            intParentID = oFees.TreeID
            oFees.Children = DirectCast(NDPBaseClassFactory("NGLCarrTarFeeData", False), NGLCarrTarFeeData).GetCarrTarFeeTree(Control, intParentID, intNextTreeID)
            oTree.Children.Add(oFees)
            '----------------------------------------------------------------------------------
            'Add Code to load Fuel Children for now just add sample data
            intParentID = oFuelCosts.TreeID
            Dim oFuel As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Sample Fuel", .Description = "Place Holder for Fuel Data"}
            If oFuelCosts.Children Is Nothing Then oFuelCosts.Children = New List(Of NGLTreeNode)
            oFuelCosts.Children.Add(oFuel)
            oTree.Children.Add(oFuelCosts)
            Return oTree
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try

    End Function

    Public Function GetTreeFlat(ByVal Control As Integer) As DataTransferObjects.CarrTarTree
        Try
            Dim intNextTreeID As Integer = 0
            Dim intParentID As Integer = 0
            Dim intNestedParentID As Integer = 0
            Dim oTree As New DataTransferObjects.CarrTarTree With {.CarrTarControl = Control, .TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Root", .Description = "Carrier Contract Tree"}
            intParentID = intNextTreeID
            'set up node level 1 (Exceptions, Equipment, Fees and Fuel)
            Dim oExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = 0, .ClassName = "Exceptions", .Name = "Exceptions", .Description = "Exception and Cross Reference Details"}
            If oTree.Children Is Nothing Then oTree.Children = New List(Of NGLTreeNode)
            oTree.Children.Add(oExceptions)
            Dim oEquipment As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = 0, .ClassName = "Equipments", .Name = "Service", .Description = "Service Details"}
            oTree.Children.Add(oEquipment)
            Dim oFees As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = 0, .ClassName = "Fees", .Name = "Fees", .Description = "Accessorial Fee Details"}
            oTree.Children.Add(oFees)
            Dim oFuelCosts As DataTransferObjects.NGLTreeNode = DirectCast(NDPBaseClassFactory("NGLCarrierFuelAddendumData", False), NGLCarrierFuelAddendumData).GetCarrTarFuelAddendum(Control, 0, incrementID(intNextTreeID))
            oTree.Children.Add(oFuelCosts)
            Dim oHolidays As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = 0, .ClassName = "NoDriveDays", .Name = "Non Drive Days", .Description = "Non Drive Days"}
            oTree.Children.Add(oHolidays)
            'Modified By LVV on 9/21/16 for v-7.0.5.110 HDM Enhancement
            Dim oHDMs As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = 0, .ClassName = "HDMs", .Name = "HDMs", .Description = "HDM Details"}
            oTree.Children.Add(oHDMs)

            'Dim oFuelCosts As New DTO.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = 0, .ClassName = "CarrierFuelAddendum", .Name = "Fuel", .Description = "Fuel Settings"}
            'oTree.Children.Add(oFuelCosts)
            '--------------------------------------------------------------------------------
            ''set up the exception nodes
            intParentID = oExceptions.TreeID
            Dim oClassExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .ClassName = "ClassExceptions", .Name = "Class", .Description = "Class Code Cross Reference"}
            oTree.Children.Add(oClassExceptions)
            Dim oDiscountExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .ClassName = "DiscountExceptions", .Name = "Discount", .Description = "Rating Discount Configuration Details"}
            oTree.Children.Add(oDiscountExceptions)
            Dim oInterlineExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .ClassName = "InterlineExceptions", .Name = "Interline", .Description = "Interline Point Details"}
            oTree.Children.Add(oInterlineExceptions)
            Dim oMinChargeExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .ClassName = "MinChargeExceptions", .Name = "MinCharge", .Description = "Minimum Charge Configuration Details"}
            oTree.Children.Add(oMinChargeExceptions)
            Dim oMinWeightExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .ClassName = "MinWeightExceptions", .Name = "MinWeight", .Description = "Minimum Weight Configuration Details"}
            oTree.Children.Add(oMinWeightExceptions)
            Dim oNonServiceExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .ClassName = "NonServiceExceptions", .Name = "NonService", .Description = "Non Service Point Configuration Details"}
            oTree.Children.Add(oNonServiceExceptions)
            ''----------------------------------------------------------------------------------
            'Add the class exception children
            'no need to add these. there maybe hunreds of records
            'intParentID = oClassExceptions.TreeID
            'oTree.Children.AddRange(DirectCast(NDPBaseClassFactory("NGLCarrTarClassXrefData", False), NGLCarrTarClassXrefData).GetCarrTarClassXrefTreeFlat(Control, intParentID, intNextTreeID))
            '----------------------------------------------------------------------------------
            'Add the discount children
            'no need to add these. there maybe hunreds of records
            'intParentID = oDiscountExceptions.TreeID
            'oTree.Children.AddRange(DirectCast(NDPBaseClassFactory("NGLCarrTarDiscountData", False), NGLCarrTarDiscountData).GetCarrTarDiscountTreeFlat(Control, intParentID, intNextTreeID))
            '----------------------------------------------------------------------------------
            'Add the interline children
            'no need to add these. there maybe hunreds of records
            ''intParentID = oInterlineExceptions.TreeID
            ''oTree.Children.AddRange(DirectCast(NDPBaseClassFactory("NGLCarrTarInterlineData", False), NGLCarrTarInterlineData).GetCarrTarInterlineTreeFlat(Control, intParentID, intNextTreeID))
            '----------------------------------------------------------------------------------
            'Add the Min Charge children
            'no need to add these. there maybe hunreds of records
            'intParentID = oMinChargeExceptions.TreeID
            'oTree.Children.AddRange(DirectCast(NDPBaseClassFactory("NGLCarrTarMinChargeData", False), NGLCarrTarMinChargeData).GetCarrTarMinChargeTreeFlat(Control, intParentID, intNextTreeID))
            '----------------------------------------------------------------------------------
            'Add the Non Service children
            'no need to add these. there maybe hunreds of records
            'intParentID = oNonServiceExceptions.TreeID
            'oTree.Children.AddRange(DirectCast(NDPBaseClassFactory("NGLCarrTarNonServData", False), NGLCarrTarNonServData).GetCarrTarNonServTreeFlat(Control, intParentID, intNextTreeID))
            '----------------------------------------------------------------------------------
            ''set up the equipment nodes
            intParentID = oEquipment.TreeID
            oTree.Children.AddRange(DirectCast(NDPBaseClassFactory("NGLCarrTarEquipData", False), NGLCarrTarEquipData).GetCarrTarEquipTreeFlat(Control, intParentID, intNextTreeID))
            '----------------------------------------------------------------------------------
            'Add Code to load Fee Children 
            intParentID = oFees.TreeID
            oTree.Children.AddRange(DirectCast(NDPBaseClassFactory("NGLCarrTarFeeData", False), NGLCarrTarFeeData).GetCarrTarFeeTreeFlat(Control, intParentID, intNextTreeID))
            '----------------------------------------------------------------------------------
            'Add code to load No Driving Days
            'GetCarrTarNDDNodes
            intParentID = oHolidays.TreeID
            oTree.Children.AddRange(DirectCast(NDPBaseClassFactory("NGLCarrierTariffNoDriveDays", False), NGLCarrierTariffNoDriveDays).GetCarrTarNDDTreeFlat(Control, intParentID, intNextTreeID))
            '----------------------------------------------------------------------------------
            'Modified By LVV on 9/21/16 for v-7.0.5.110 HDM Enhancement
            'Add Code to load HDM Children 
            intParentID = oHDMs.TreeID
            Dim hdmNodeList = DirectCast(NDPBaseClassFactory("NGLHDMData", False), NGLHDMData).GetHDMTreeFlat(Control, intParentID, intNextTreeID)
            If Not hdmNodeList Is Nothing AndAlso hdmNodeList.Count > 0 Then
                oTree.Children.AddRange(hdmNodeList)
            End If

            '----------------------------------------------------------------------------------

            'Add Code to load Fuel Children   
            intParentID = oFuelCosts.TreeID
            If oFuelCosts.Control = 0 Then 'lets add an add new node for Fuel Addendum if this contract does not have one.
                Dim singeaddnode As DataTransferObjects.NGLTreeNode = New DataTransferObjects.NGLTreeNode With {.Control = Control,
                        .TreeID = incrementID(intNextTreeID),
                        .ParentTreeID = intParentID,
                        .Name = "AddNew",
                        .Description = "AddNewCarrierFuelAddendum",
                        .AltDataKey = "",
                        .ClassName = "CarrierFuelAddendum"}
                oTree.Children.Add(singeaddnode)
            End If
            'Dim oFuel As New DTO.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .ClassName = "CarrierFuelAddendum", .Name = "Settings", .Description = "Fuel Settings"}
            'oTree.Children.Add(oFuel)
            Dim oFuelAvgFuelSurcharge As New DataTransferObjects.NGLTreeNode With {.Control = oFuelCosts.Control, .TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .ClassName = "CarrierFuelAdRates", .Name = "Avg Fuel Surcharge", .Description = "Fuel Avg Fuel Surcharge"}
            oTree.Children.Add(oFuelAvgFuelSurcharge)
            Dim oFuelExceptions As New DataTransferObjects.NGLTreeNode With {.Control = oFuelCosts.Control, .TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .ClassName = "CarrierFuelAdEx", .Name = "Fuel Exceptions", .Description = "Fuel Exceptions"}
            oTree.Children.Add(oFuelExceptions)

            Return oTree
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try

    End Function

    Public Function GetTreeFlat_Draft1(ByVal Control As Integer) As DataTransferObjects.CarrTarTree
        Try
            Dim intNextTreeID As Integer = 0
            Dim intParentID As Integer = 0
            Dim oTree As New DataTransferObjects.CarrTarTree With {.CarrTarControl = Control, .TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Root", .Description = "Carrier Contract Tree"}
            intParentID = intNextTreeID
            'set up node level 1 (Exceptions, Equipment, Fees and Fuel)
            Dim oExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Exceptions", .Description = "Exception and Cross Reference Details"}
            If oTree.Children Is Nothing Then oTree.Children = New List(Of NGLTreeNode)
            oTree.Children.Add(oExceptions)
            Dim oEquipment As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Equipment", .Description = "Equipment Details"}
            oTree.Children.Add(oEquipment)
            Dim oFees As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Fees", .Description = "Accessorial Fee Details"}
            oTree.Children.Add(oFees)
            Dim oFuelCosts As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Fuel", .Description = "Fuel Details"}
            oTree.Children.Add(oFuelCosts)
            '--------------------------------------------------------------------------------
            'set up the exception nodes
            intParentID = oExceptions.TreeID
            Dim oClassExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Class", .Description = "Class Code Cross Reference"}
            oTree.Children.Add(oClassExceptions)
            Dim oDiscountExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Discount", .Description = "Rating Discount Configuration Details"}
            oTree.Children.Add(oDiscountExceptions)
            Dim oInterlineExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Interline", .Description = "Interline Point Details"}
            oTree.Children.Add(oInterlineExceptions)
            Dim oMinChargeExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "MinCharge", .Description = "Minimum Charge Configuration Details"}
            oTree.Children.Add(oMinChargeExceptions)
            Dim oNonServiceExceptions As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "NonService", .Description = "Non Service Point Configuration Details"}
            oTree.Children.Add(oNonServiceExceptions)
            '----------------------------------------------------------------------------------
            'set up the equipment nodes
            intParentID = oEquipment.TreeID
            oTree.Children.AddRange(DirectCast(NDPBaseClassFactory("NGLCarrTarEquipData", False), NGLCarrTarEquipData).GetCarrTarEquipTreeFlat(Control, intParentID, intNextTreeID))
            '----------------------------------------------------------------------------------
            'Add Code to load Fee Children for now just add sample data
            intParentID = oFees.TreeID
            Dim oFee As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Sample Fee", .Description = "Place Holder for Fee Data"}
            oTree.Children.Add(oFee)
            '----------------------------------------------------------------------------------
            'Add Code to load Fuel Children for now just add sample data
            intParentID = oFuelCosts.TreeID
            Dim oFuel As New DataTransferObjects.NGLTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .Name = "Sample Fuel", .Description = "Place Holder for Fuel Data"}
            oTree.Children.Add(oFuel)
            Return oTree
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try

    End Function

    ''' <summary>
    ''' Returns a Read Only Pivot Table of Tariff Data Records
    ''' </summary>
    ''' <param name="BookControl"> Required</param>
    ''' <param name="CarrierControl"> Limit the resulsts to a specific carrier 0 = All</param>
    ''' <param name="Prefered">if true limits the seliction to the Lane Prefered carrier settings</param>
    ''' <param name="NoLateDelivery">if true the system will check delivery days with Load and Required Date settings ensuring that the selected carriers can deliver on time</param>
    ''' <param name="Validated">if true only returns carriers that have been pre-qualified based on validation rules</param>
    ''' <param name="OptimizeByCapacity">if true limits the carriers returned by capacity settings in Carrier Equipment (Typically this is On)</param>
    ''' <param name="ModeTypeControl">Limit results by Mode -1 = use booking data, 0 = ALL,1 = Air,2 = Rail,3 = Road,4 = Sea</param>
    ''' <param name="TempType">Limit results by Temperature -1 = use booking data, 0 = ALL, 1 = Dry, 2 = Frzen, 3 = Refrigerated</param>
    ''' <param name="TariffTypeControl">Limit results by Tariff Type 0 = ALL, 1 = Private, 2 = Public</param>
    ''' <param name="CarrTarEquipMatClass">Limit by class A value like 100 pass Null or empty string to ignore</param>
    ''' <param name="CarrTarEquipMatClassTypeControl">Limit results by Class Type 0 = All,1 = 49CFR,2 = IATA,3 = DOT,4 = Marine,5	= NMFC,6 = FAK,7 = NA (If non-zero: use a rate type of 3 -- Class if Rate Type is not provided (0))</param>
    ''' <param name="CarrTarEquipMatTarRateTypeControl">Limit the results by Rate Type 0 = All,1 = Distance M,2 = Distance K,3	= Class,4 = Flat,5 = Unit of Measure,6 = CzarLite</param>
    ''' <param name="AgentControl">Limit the results by the specified Agent 0 = All</param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarrierTariffsPivot(ByVal BookControl As Integer,
                                           Optional ByVal CarrierControl As Integer = 0,
                                           Optional ByVal Prefered As Boolean = True,
                                           Optional ByVal NoLateDelivery As Boolean = False,
                                           Optional ByVal Validated As Boolean = True,
                                           Optional ByVal OptimizeByCapacity As Boolean = True,
                                           Optional ByVal ModeTypeControl As Integer = 0,
                                           Optional ByVal TempType As Integer = 0,
                                           Optional ByVal TariffTypeControl As Integer = 0,
                                           Optional ByVal CarrTarEquipMatClass As String = Nothing,
                                           Optional ByVal CarrTarEquipMatClassTypeControl As Integer = 0,
                                           Optional ByVal CarrTarEquipMatTarRateTypeControl As Integer = 0,
                                           Optional ByVal AgentControl As Integer = 0,
                                           Optional ByVal page As Integer = 1,
                                           Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.CarrierTariffsPivot()

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                'db.Log = New DebugTextWriter

                'Dim oRecords = ( _
                'From d In db.udfCarrierTariffsPivot(BookControl, CarrierControl, Prefered, NoLateDelivery, Validated, OptimizeByCapacity, ModeTypeControl, TempType, TariffTypeControl, CarrTarEquipMatClass, CarrTarEquipMatClassTypeControl, CarrTarEquipMatTarRateTypeControl, AgentControl) _
                'Select selectDTOCarrierTarrifsPivotData(d, db)).ToList


                Dim oRecords = (
                        From d In db.spCarrierTariffsPivot(BookControl, CarrierControl, Prefered, NoLateDelivery, Validated, OptimizeByCapacity, ModeTypeControl, TempType, TariffTypeControl, CarrTarEquipMatClass, CarrTarEquipMatClassTypeControl, CarrTarEquipMatTarRateTypeControl, AgentControl)
                        Select selectDTOCarrierTarrifsPivotData(d, db)).ToList()

                Logger.Information("Getting Tariff Pivot from spCarrierTariffsPivot, found {0} records.", oRecords.Count)

                If oRecords Is Nothing Then Return Nothing

                intRecordCount = oRecords.Count
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1


                Dim intSkip As Integer = (page - 1) * pagesize
                Dim BPPivots As New Dictionary(Of Integer, CarrTarMatBPPivot)
                Dim oBPData As New NGLCarrTarMatBPData(Me.Parameters)

                Logger.Information("Getting Tariff Pivot, page {0} of {1} with {2} records.", page, intPageCount, pagesize)

                Dim CarrierTariffsPivot() As DataTransferObjects.CarrierTariffsPivot = oRecords.Skip(intSkip).Take(pagesize).ToArray()

                Logger.Information("Getting Tariff Pivot, found {0} records. ", CarrierTariffsPivot.Count)

                For Each pivot In CarrierTariffsPivot
                    If pivot.CarrTarEquipMatCarrTarMatBPControl <> 0 Then
                        If Not BPPivots.ContainsKey(pivot.CarrTarEquipMatCarrTarMatBPControl) Then
                            Logger.Information("Getting Tariff Pivot, getting BP Pivot for {0}.", pivot.CarrTarEquipMatCarrTarMatBPControl)
                            BPPivots.Add(pivot.CarrTarEquipMatCarrTarMatBPControl, oBPData.GetCarrTarMatBPPivot(pivot.CarrTarEquipMatCarrTarMatBPControl))
                        End If
                        pivot.BPPivot = BPPivots(pivot.CarrTarEquipMatCarrTarMatBPControl)

                    End If
                    pivot.Page = page
                    pivot.Pages = intPageCount
                    pivot.RecordCount = intRecordCount
                    pivot.PageSize = pagesize
                Next

                Return CarrierTariffsPivot

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException()
            Catch ex As Exception
                throwUnExpectedFaultException(ex, getSourceCaller("GetCarrTarEquipMatPivot"))
            End Try

            Logger.Error("No idea why we are here.")

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns an array of CarrierTariffsPivot filterd by Tariff Equipment; 
    ''' typically used to recalculate carrier costs for LTL loads.
    ''' Supports multiple class codes at the sku level.
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="CarrTarEquipControl"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarrierTariffsPivotByEquip(ByVal BookControl As Integer,
                                                  ByVal CarrTarEquipControl As Integer,
                                                  Optional ByVal page As Integer = 1,
                                                  Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.CarrierTariffsPivot()
        Using Logger.StartActivity("GetCarrierTariffsPivotByEquipt(BookControl: {BookControl}, CarrTarEquipControl: {CarrTarEquipControl}", BookControl, CarrTarEquipControl)
            Using db As New NGLMASCarrierDataContext(ConnectionString)
                Try

                    Dim intRecordCount As Integer = 0
                    Dim intPageCount As Integer = 1

                    'db.Log = New DebugTextWriter

                    Dim oRecords = (
                            From d In db.udfCarrierTariffsPivotByEquip(BookControl, CarrTarEquipControl)
                            Select selectDTOCarrierTarrifsPivotData(d, db)).ToList()

                    Logger.Information("Getting Tariff Pivot from udfCarrierTariffsPivotByEquip, found {0} records.", oRecords.Count)

                    If oRecords Is Nothing Then Return Nothing

                    intRecordCount = oRecords.Count
                    If pagesize < 1 Then pagesize = 1
                    If intRecordCount < 1 Then intRecordCount = 1
                    If page < 1 Then page = 1
                    If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                    Dim intSkip As Integer = (page - 1) * pagesize
                    Dim BPPivots As New Dictionary(Of Integer, CarrTarMatBPPivot)
                    Dim oBPData As New NGLCarrTarMatBPData(Me.Parameters)
                    Dim CarrierTariffsPivot() As DataTransferObjects.CarrierTariffsPivot = oRecords.Skip(intSkip).Take(pagesize).ToArray()
                    For Each pivot In CarrierTariffsPivot
                        If pivot.CarrTarEquipMatCarrTarMatBPControl <> 0 Then
                            If Not BPPivots.ContainsKey(pivot.CarrTarEquipMatCarrTarMatBPControl) Then
                                BPPivots.Add(pivot.CarrTarEquipMatCarrTarMatBPControl, oBPData.GetCarrTarMatBPPivot(pivot.CarrTarEquipMatCarrTarMatBPControl))
                            End If
                            pivot.BPPivot = BPPivots(pivot.CarrTarEquipMatCarrTarMatBPControl)
                        End If
                        pivot.Page = page
                        pivot.Pages = intPageCount
                        pivot.RecordCount = intRecordCount
                        pivot.PageSize = pagesize
                    Next

                    Return CarrierTariffsPivot

                Catch ex As System.Data.SqlClient.SqlException
                    throwSQLFaultException(ex.Message)
                Catch ex As InvalidOperationException
                    throwNoDataFaultException()
                Catch ex As Exception
                    throwUnExpectedFaultException(ex, getSourceCaller("GetCarrierTariffsPivotByEquip"))
                End Try
            End Using

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns a specific Read Only Pivot Table of Tariff Data Records
    ''' </summary>
    ''' <param name="CarrTarEquipMatControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarrierTariffPivot(ByVal CarrTarEquipMatControl As Integer) As DataTransferObjects.CarrierTariffsPivot

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Logger.Information("Getting Tariff Pivot for CarrTarEquipMatControl {0}.", CarrTarEquipMatControl)

                Dim CarrierTariffsPivot As DataTransferObjects.CarrierTariffsPivot = (From d In db.udfCarrierTariffPivot(CarrTarEquipMatControl) Select selectDTOCarrierTarrifPivotData(d)).FirstOrDefault()

                If CarrierTariffsPivot.CarrTarEquipMatCarrTarMatBPControl <> 0 Then
                    Logger.Information("Getting Tariff Pivot, getting BP Pivot for {0}.", CarrierTariffsPivot.CarrTarEquipMatCarrTarMatBPControl)
                    CarrierTariffsPivot.BPPivot = New NGLCarrTarMatBPData(Me.Parameters).GetCarrTarMatBPPivot(CarrierTariffsPivot.CarrTarEquipMatCarrTarMatBPControl)
                End If

                Return CarrierTariffsPivot

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException()
            Catch ex As Exception
                throwUnExpectedFaultException(ex, getSourceCaller("GetCarrierTariffPivot"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrierTariffPivotsAltDataKey(ByVal CarrTarControl As Integer,
                                                     ByVal CarrTarEquipControl As Integer,
                                                     ByVal TarRateTypeControl As Integer,
                                                     ByVal ClassTypeControl As Integer,
                                                     ByVal TarBracketTypeControl As Integer,
                                                     Optional ByVal page As Integer = 1,
                                                     Optional ByVal pagesize As Integer = 1000,
                                                     Optional ByVal filterWhere As String = "") As DataTransferObjects.CarrierTariffsPivot()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Logger.Information("Getting Tariff Pivot for CarrTarControl {0}, CarrTarEquipControl {1}, TarRateTypeControl {2}, ClassTypeControl {3}, TarBracketTypeControl {4}.", CarrTarControl, CarrTarEquipControl, TarRateTypeControl, ClassTypeControl, TarBracketTypeControl)

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                'db.Log = New DebugTextWriter

                Dim oRecords = (
                        From d In db.udfCarrierTariffPivot(0)
                        Where
                        (d.CarrTarControl = CarrTarControl) _
                        And
                        (d.CarrTarEquipControl = CarrTarEquipControl) _
                        And
                        d.CarrTarEquipMatTarRateTypeControl = TarRateTypeControl _
                        And
                        d.CarrTarEquipMatClassTypeControl = ClassTypeControl _
                        And
                        d.CarrTarEquipMatTarBracketTypeControl = TarBracketTypeControl
                        Select selectDTOCarrierTarrifPivotData(d)).ToList

                Logger.Information("Getting Tariff Pivot from udfCarrierTariffPivot, found {0} records.", oRecords.Count)

                If oRecords Is Nothing Then Return Nothing

                If Not String.IsNullOrEmpty(filterWhere) Then
                    oRecords = DLinqUtil.filterWhere(oRecords, filterWhere)
                End If

                intRecordCount = oRecords.Count
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize
                Dim BPPivots As New Dictionary(Of Integer, CarrTarMatBPPivot)
                Dim oBPData As New NGLCarrTarMatBPData(Me.Parameters)
                Dim CarrierTariffsPivot() As DataTransferObjects.CarrierTariffsPivot = oRecords.Skip(intSkip).Take(pagesize).ToArray()
                For Each pivot In CarrierTariffsPivot
                    If pivot.CarrTarEquipMatCarrTarMatBPControl <> 0 Then
                        If Not BPPivots.ContainsKey(pivot.CarrTarEquipMatCarrTarMatBPControl) Then
                            BPPivots.Add(pivot.CarrTarEquipMatCarrTarMatBPControl, oBPData.GetCarrTarMatBPPivot(pivot.CarrTarEquipMatCarrTarMatBPControl))
                        End If
                        pivot.BPPivot = BPPivots(pivot.CarrTarEquipMatCarrTarMatBPControl)
                    End If
                    pivot.Page = page
                    pivot.Pages = intPageCount
                    pivot.RecordCount = intRecordCount
                    pivot.PageSize = pagesize
                Next

                Return CarrierTariffsPivot

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException()
            Catch ex As Exception
                throwUnExpectedFaultException(ex, getSourceCaller("GetCarrTarEquipMatPivots"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrierTariffPivotsListAltDataKey(ByVal CarrTarEquipMatNodeList As List(Of CarrTarEquipMatNode),
                                                         Optional ByVal page As Integer = 1,
                                                         Optional ByVal pagesize As Integer = 100000,
                                                         Optional ByVal filterWhere As String = "") As DataTransferObjects.CarrierTariffsPivot()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try


                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                Dim CarrierTariffsPivotBigList As New List(Of CarrierTariffsPivot)

                'Loop through the mat node list and get the pivot rates

                For Each item As DataTransferObjects.CarrTarEquipMatNode In CarrTarEquipMatNodeList

                    Dim CarrierTariffsPivotlist As List(Of CarrierTariffsPivot) =
                            GetCarrierTariffPivotsAltDataKey(item.CarrTarEquipMatCarrTarControl,
                                                             item.CarrTarEquipMatCarrTarEquipControl,
                                                             item.CarrTarEquipMatTarRateTypeControl,
                                                             item.CarrTarEquipMatClassTypeControl,
                                                             item.CarrTarEquipMatTarBracketTypeControl,
                                                             page,
                                                             pagesize,
                                                             filterWhere).ToList

                    CarrierTariffsPivotBigList.AddRange(CarrierTariffsPivotlist)
                Next


                Return CarrierTariffsPivotBigList.ToArray

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException()
            Catch ex As Exception
                throwUnExpectedFaultException(ex, getSourceCaller("GetCarrierTariffPivotsListAltDataKey"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' This method throws an fault exception with 
    ''' Reason E_DataValidationFailure
    ''' Message E_CannotUpdateTariffApproved
    ''' Details E_ContractApporvedDetails
    ''' If the tariff contract is approved
    ''' No return value is expected
    ''' </summary>
    ''' <param name="intCarrTarControl"></param>
    ''' <param name="oDB"></param>
    ''' <remarks></remarks>
    Public Sub ValidateApproved(ByVal intCarrTarControl As Integer, ByRef oDB As NGLMASCarrierDataContext)
        'Code Removed for Beta Testing no validation required
        'If intCarrTarControl <> 0 Then
        '    Dim oCarrTarContractData As DTO.CarrTarContract = (From c In oDB.CarrierTariffs Where c.CarrTarControl = intCarrTarControl Select New DTO.CarrTarContract With {.CarrTarControl = c.CarrTarControl, .CarrTarApproved = c.CarrTarApproved, .CarrTarApprovedBy = c.CarrTarApprovedBy, .CarrTarApprovedDate = c.CarrTarApprovedDate}).FirstOrDefault()
        '    If Not oCarrTarContractData Is Nothing AndAlso oCarrTarContractData.CarrTarApproved = True Then
        '        throwCannotUpdateTariffApprovedDataException(SqlFaultInfo.FaultDetailsKey.E_ContractApporvedDetails, New List(Of String) From {If(oCarrTarContractData.CarrTarApprovedDate.HasValue, oCarrTarContractData.CarrTarApprovedDate.Value.ToShortDateString & " " & oCarrTarContractData.CarrTarApprovedDate.Value.ToShortTimeString, "No Date"), oCarrTarContractData.CarrTarApprovedBy})
        '        Return
        '    End If
        'End If
    End Sub

    ''' <summary>
    ''' Tests if any records exist in the carriertariff table where the CarrTarPreCloneControl = CarrTarControl
    ''' true indicates that this tariff has already been cloned at lease once.
    ''' </summary>
    ''' <param name="CarrTarControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function HasTariffBeenCloned(ByVal CarrTarControl As Integer) As Boolean
        Dim blnRet As Boolean = True
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                blnRet = db.CarrierTariffs.Any(Function(x) x.CarrTarPreCloneControl = CarrTarControl)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("HasTariffBeenCloned"))
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Creates a copy of the current tariff contract identified by the CarrTarControl.  
    ''' If errors are encountered by the stored procedure it will roll back any changes
    ''' and populate the ErrNumber and RetMsg values of the GenericResult object.  If 
    ''' successfull the Control will be non zero and the ErrNumber will be zero
    ''' </summary>
    ''' <param name="CarrTarControl"></param>
    ''' <param name="EffDateFrom">may be null uses current date if null</param>
    ''' <param name="EffDateTo">may be null database will be null</param>
    ''' <param name="AutoApprove"></param>
    ''' <param name="IssuedDate">may be null uses current data if null</param>
    ''' <param name="CopyClassXrefData"></param>
    ''' <param name="CopyNoDriveDays"></param>
    ''' <param name="CopyDiscountData"></param>
    ''' <param name="CopyFeeData"></param>
    ''' <param name="CopyInterlinePointData"></param>
    ''' <param name="CopyMinChargeData"></param>
    ''' <param name="CopyNonServicePointData"></param>
    ''' <param name="CopyMatrixBPData"></param>
    ''' <param name="CopyEquipmentData"></param>
    ''' <param name="CopyEquipmentRateData"></param>
    ''' <param name="CopyFuelData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CloneTariff(ByVal CarrTarControl As Integer,
                                ByVal EffDateFrom As Date?,
                                ByVal EffDateTo As Date?,
                                ByVal AutoApprove As Boolean,
                                ByVal IssuedDate As Date?,
                                ByVal CopyClassXrefData As Boolean,
                                ByVal CopyNoDriveDays As Boolean,
                                ByVal CopyDiscountData As Boolean,
                                ByVal CopyFeeData As Boolean,
                                ByVal CopyInterlinePointData As Boolean,
                                ByVal CopyMinChargeData As Boolean,
                                ByVal CopyMinWeightData As Boolean,
                                ByVal CopyNonServicePointData As Boolean,
                                ByVal CopyMatrixBPData As Boolean,
                                ByVal CopyEquipmentData As Boolean,
                                ByVal CopyEquipmentRateData As Boolean,
                                ByVal CopyFuelData As Boolean) As DataTransferObjects.GenericResults
        Dim oRet As New DataTransferObjects.GenericResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim oresults = db.spCloneTariff(CarrTarControl,
                                                EffDateFrom,
                                                EffDateTo,
                                                AutoApprove,
                                                IssuedDate,
                                                CopyClassXrefData,
                                                CopyNoDriveDays,
                                                CopyDiscountData,
                                                CopyFeeData,
                                                CopyInterlinePointData,
                                                CopyMinChargeData,
                                                CopyMinWeightData,
                                                CopyNonServicePointData,
                                                CopyMatrixBPData,
                                                CopyEquipmentData,
                                                CopyEquipmentRateData,
                                                CopyFuelData,
                                                Me.Parameters.UserName()).FirstOrDefault()
                If Not oresults Is Nothing Then
                    oRet.Control = oresults.NewCarrTarControl
                    oRet.ErrNumber = oresults.ErrNumber
                    oRet.RetMsg = oresults.RetMsg
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CloneTariff"))
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Creates a copy of the tariff and modifies it with the new company sent in parameter.
    ''' </summary>
    ''' <param name="CarrTarControl"></param>
    ''' <param name="EffDateFrom"></param>
    ''' <param name="EffDateTo"></param>
    ''' <param name="AutoApprove"></param>
    ''' <param name="IssuedDate"></param>
    ''' <param name="CopyClassXrefData"></param>
    ''' <param name="CopyNoDriveDays"></param>
    ''' <param name="CopyDiscountData"></param>
    ''' <param name="CopyFeeData"></param>
    ''' <param name="CopyInterlinePointData"></param>
    ''' <param name="CopyMinChargeData"></param>
    ''' <param name="CopyMinWeightData"></param>
    ''' <param name="CopyNonServicePointData"></param>
    ''' <param name="CopyMatrixBPData"></param>
    ''' <param name="CopyEquipmentData"></param>
    ''' <param name="CopyEquipmentRateData"></param>
    ''' <param name="CopyFuelData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyTariff(ByVal CarrTarControl As Integer,
                               ByVal EffDateFrom As Date?,
                               ByVal EffDateTo As Date?,
                               ByVal AutoApprove As Boolean,
                               ByVal IssuedDate As Date?,
                               ByVal CopyClassXrefData As Boolean,
                               ByVal CopyNoDriveDays As Boolean,
                               ByVal CopyDiscountData As Boolean,
                               ByVal CopyFeeData As Boolean,
                               ByVal CopyInterlinePointData As Boolean,
                               ByVal CopyMinChargeData As Boolean,
                               ByVal CopyMinWeightData As Boolean,
                               ByVal CopyNonServicePointData As Boolean,
                               ByVal CopyMatrixBPData As Boolean,
                               ByVal CopyEquipmentData As Boolean,
                               ByVal CopyEquipmentRateData As Boolean,
                               ByVal CopyFuelData As Boolean,
                               ByVal newCompControl As Integer,
                               ByVal newContractName As String) As DataTransferObjects.GenericResults
        Dim oRet As New DataTransferObjects.GenericResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim oresults = db.spCopyTariff(CarrTarControl,
                                               EffDateFrom,
                                               EffDateTo,
                                               AutoApprove,
                                               IssuedDate,
                                               CopyClassXrefData,
                                               CopyNoDriveDays,
                                               CopyDiscountData,
                                               CopyFeeData,
                                               CopyInterlinePointData,
                                               CopyMinChargeData,
                                               CopyMinWeightData,
                                               CopyNonServicePointData,
                                               CopyMatrixBPData,
                                               CopyEquipmentData,
                                               CopyEquipmentRateData,
                                               CopyFuelData,
                                               Me.Parameters.UserName(),
                                               newCompControl,
                                               newContractName).FirstOrDefault()
                If Not oresults Is Nothing Then
                    oRet.Control = oresults.NewCarrTarControl
                    oRet.ErrNumber = oresults.ErrNumber
                    oRet.RetMsg = oresults.RetMsg
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CopyTariff"))
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' This generates a CarrTarID in the format:
    ''' @CompNumber + '-' + @CarrierNumber + '-' + @ModeChar + '-' + @TariffTypeChar + '-' + @OutboundChar + '-' + @TempChar + '-' + convert(char(3), @EffDateFrom, 0)
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="ModeTypeControl"></param>
    ''' <param name="TariffTypeControl"></param>
    ''' <param name="TempTypeControl"></param>
    ''' <param name="EffDateFrom"></param>
    ''' <param name="Outbound"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function generateNewCarrTarID(
                                         ByVal CompControl As Integer,
                                         ByVal CarrierControl As Integer,
                                         ByVal ModeTypeControl As Integer,
                                         ByVal TariffTypeControl As Integer,
                                         ByVal TempTypeControl As Integer,
                                         ByVal EffDateFrom As Date?,
                                         ByVal Outbound As Boolean) As String
        Dim oRet As String = ""
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim oresults As String = db.udfGenerateCarrTarID(CompControl,
                                                                 CarrierControl,
                                                                 ModeTypeControl,
                                                                 TariffTypeControl,
                                                                 TempTypeControl,
                                                                 EffDateFrom,
                                                                 Outbound)
                If Not String.IsNullOrEmpty(oresults) Then
                    oRet = oresults
                Else
                    oRet = ""
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("generateNewCarrTarID"))
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Returns a list of the default class codes.  
    ''' The caller must check ParValue.  
    ''' If ParValue not equal 1 the default class code is turned off and should be ignored.
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDefaultClassCodes(ByVal CompControl As Integer) As List(Of LTS.udfGetDefaultClassCodesResult)
        Using operation = Logger.StartActivity("GetDefaultClassCodes(CompControl: {CompControl})", CompControl)
            Using db As New NGLMASCarrierDataContext(ConnectionString)
                Try
                    Return db.udfGetDefaultClassCodes(CompControl).ToList()

                Catch ex As System.Data.SqlClient.SqlException
                    operation.Complete(LogEventLevel.Error, ex)
                Catch ex As InvalidOperationException
                    operation.Complete(LogEventLevel.Error, ex)
                Catch ex As Exception
                    operation.Complete(LogEventLevel.Error, ex)
                End Try

                Return Nothing
            End Using

        End Using

    End Function

    'Public Function GetTariffCode(ByVal CarrierControl As Integer, _
    '                              ByVal CompControl As Integer, _
    '                              ByVal TariffTempType As Integer, _
    '                              ByVal TariffType As String) As String
    '    Dim strCompNumber As String = "0"
    '    Dim strCarrierNumber As String = "0"
    '    Dim strTempType As String = "0" & TariffTempType.ToString
    '    Using db As New NGLMASCarrierDataContext(ConnectionString)
    '        Try
    '            Dim varComp = From d In db.CompRefCarriers Where d.CompControl = CompControl Select d.CompNumber
    '            Dim varCarr = From c In db.Carriers Where c.CarrierControl = CarrierControl Select c.CarrierNumber

    '            If Not varComp Is Nothing AndAlso varComp.Count > 0 Then strCompNumber = varComp(0).ToString

    '            If Not varCarr Is Nothing AndAlso varCarr.Count > 0 Then strCarrierNumber = varCarr(0).ToString

    '        Catch ex As System.Data.SqlClient.SqlException
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
    '        Catch ex As InvalidOperationException
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
    '        Catch ex As Exception
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
    '        End Try

    '        Return strCarrierNumber & "-" & strCompNumber & "-" & strTempType & "-" & TariffTempType

    '    End Using
    'End Function

    ' ''' <summary>
    ' ''' This method will test if an overwrite condition exists.  It does not test if
    ' ''' the users wants to clone an existing tariff and create a new one.  The caller 
    ' ''' must test for the posibility of a clone condition.  If one of the TariffID key
    ' ''' fields changes {CarrierControl, CompControl, TariffTempType or TariffType} the
    ' ''' caller must set the CarrTarControl to zero if a clone operation is desired. They 
    ' ''' should then call AddNew, instead of Update, after calling CanSaveTariff.  It may 
    ' ''' still be necessary to set the AllowOverwrite flag of the CarrierTariffHeader object 
    ' ''' to true.
    ' ''' </summary>
    ' ''' <param name="CarrTarControl"></param>
    ' ''' <param name="CarrTarID"></param>
    ' ''' <param name="CarrierControl"></param>
    ' ''' <param name="CompControl"></param>
    ' ''' <param name="TariffTempType"></param>
    ' ''' <param name="TariffType"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function CanSaveTariff(ByVal CarrTarControl As Integer, _
    '                              ByVal CarrTarID As String, _
    '                              ByVal CarrierControl As Integer, _
    '                              ByVal CompControl As Integer, _
    '                              ByVal TariffTempType As Integer, _
    '                              ByVal TariffType As String) As Boolean
    '    Dim blnRet As Boolean = True
    '    Using db As New NGLMASCarrierDataContext(ConnectionString)
    '        Try
    '            Dim strNewTariffID = GetTariffCode(CarrierControl, CompControl, TariffTempType, TariffType)

    '            If CarrTarControl = 0 Then
    '                'this is a new tariff so we need to check if the tariff id already exists
    '                Dim varTariff = From d In db.CarrierTariffs Where d.CarrTarID = strNewTariffID Select d
    '                If Not varTariff Is Nothing AndAlso varTariff.Count > 0 Then Return False
    '            ElseIf CarrTarID <> strNewTariffID Then
    '                'if they change the TariffID we alwys return false and force the UI to ask for permission
    '                Return False
    '            Else
    '                Return True
    '            End If

    '        Catch ex As FaultException
    '            Throw
    '        Catch ex As System.Data.SqlClient.SqlException
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
    '        Catch ex As InvalidOperationException
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
    '        Catch ex As Exception
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
    '        End Try

    '        Return blnRet

    '    End Using
    'End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrTarContract)
        'Create New Record
        Return New LTS.CarrierTariff With {.CarrTarControl = d.CarrTarControl,
            .CarrTarCarrierControl = d.CarrTarCarrierControl,
            .CarrTarCompControl = d.CarrTarCompControl,
            .CarrTarID = d.CarrTarID,
            .CarrTarBPBracketType = d.CarrTarBPBracketType,
            .CarrTarTLCapacityType = d.CarrTarTLCapacityType,
            .CarrTarTempType = d.CarrTarTempType,
            .CarrTarTariffType = d.CarrTarTariffType,
            .CarrTarDefWgt = d.CarrTarDefWgt,
            .CarrTarEffDateFrom = If(d.CarrTarEffDateFrom.HasValue, d.CarrTarEffDateFrom, Date.Now()),
            .CarrTarEffDateTo = d.CarrTarEffDateTo,
            .CarrTarAutoAssignPro = d.CarrTarAutoAssignPro,
            .CarrTarTariffTypeControl = d.CarrTarTariffTypeControl,
            .CarrTarTariffModeTypeControl = d.CarrTarTariffModeTypeControl,
            .CarrTarName = d.CarrTarName,
            .CarrTarRevisionNumber = d.CarrTarRevisionNumber,
            .CarrTarApproved = d.CarrTarApproved,
            .CarrTarApprovedDate = d.CarrTarApprovedDate,
            .CarrTarApprovedBy = d.CarrTarApprovedBy,
            .CarrTarRejected = d.CarrTarRejected,
            .CarrTarRejectedDate = d.CarrTarRejectedDate,
            .CarrTarRejectedBy = d.CarrTarRejectedBy,
            .CarrTarOutbound = d.CarrTarOutbound,
            .CarrTarAgentControl = d.CarrTarAgentControl,
            .CarrTarIssuedDate = d.CarrTarIssuedDate,
            .CarrTarPreCloneControl = d.CarrTarPreCloneControl,
            .CarrTarUser1 = d.CarrTarUser1,
            .CarrTarUser2 = d.CarrTarUser2,
            .CarrTarUser3 = d.CarrTarUser3,
            .CarrTarUser4 = d.CarrTarUser4,
            .CarrTarWillDriveSunday = d.CarrTarWillDriveSunday,
            .CarrTarWillDriveSaturday = d.CarrTarWillDriveSaturday,
            .CarrTarModUser = Parameters.UserName,
            .CarrTarModDate = Date.Now,
            .CarrTarUpdated = If(d.CarrTarUpdated Is Nothing, New Byte() {}, d.CarrTarUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrTarContractFiltered(Control:=CType(LinqTable, LTS.CarrierTariff).CarrTarControl)
    End Function

    'Protected Function validateSaveTariff(ByRef CarrTarControl As Integer, _
    '                              ByRef CarrTarID As String, _
    '                              ByVal CarrierControl As Integer, _
    '                              ByVal CompControl As Integer, _
    '                              ByVal TariffTempType As Integer, _
    '                              ByVal TariffType As String, _
    '                              ByVal AllowOverwrite As Boolean) As Boolean
    '    Dim blnRet As Boolean = True
    '    Dim intOldCarTarControl As Integer = CarrTarControl
    '    Dim strOldCarrTarID As String = CarrTarID
    '    Using db As New NGLMASCarrierDataContext(ConnectionString)
    '        Try
    '            Dim strNewTariffID = GetTariffCode(CarrierControl, CompControl, TariffTempType, TariffType)

    '            If CarrTarControl = 0 Then
    '                'this is a new tariff so we need to check if the tariff id already exists
    '                Dim varTariff = (From d In db.CarrierTariffs Where d.CarrTarID = strNewTariffID Select d).First
    '                If Not varTariff Is Nothing Then
    '                    If Not AllowOverwrite Then Return False
    '                    CarrTarControl = varTariff.CarrTarControl
    '                    CarrTarID = strNewTariffID
    '                    ''Delete all of the matrix details they no longer match.
    '                    'executeSQL("DELETE FROM [dbo].[CarrierTariffMatrix] Where CarrTarMatCarrTarControl = " & CarrTarControl)
    '                End If
    '            ElseIf CarrTarID <> strNewTariffID Then
    '                'if they change the TariffID we alwys return false and force the UI to ask for permission
    '                Return False
    '            Else
    '                Return True
    '            End If

    '        Catch ex As FaultException
    '            Throw
    '        Catch ex As System.Data.SqlClient.SqlException
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
    '        Catch ex As InvalidOperationException
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
    '        Catch ex As Exception
    '            Utilities.SaveAppError(ex.Message, Me.Parameters)
    '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
    '        End Try

    '        Return blnRet

    '    End Using
    'End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierTariff = TryCast(LinqTable, LTS.CarrierTariff)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierTariffs
                       Where d.CarrTarControl = source.CarrTarControl
                       Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrTarControl _
                           , .ModDate = d.CarrTarModDate _
                           , .ModUser = d.CarrTarModUser _
                           , .Updated = d.CarrTarUpdated.ToArray}).First

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

    ''' <summary>
    ''' Checks the Book Table for a reference to the CarrTarControl.
    ''' Records cannot be deleted if a reference exists.  
    ''' Throws E_InvalidRequest FaultException if record exists in Book Table
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        Dim oCarrTarData As DataTransferObjects.CarrTarContract = TryCast(oData, DataTransferObjects.CarrTarContract)
        If oCarrTarData Is Nothing Then Return
        Dim intCarrTarControl = oCarrTarData.CarrTarControl
        If intCarrTarControl = 0 Then Return
        Dim strCarrTarName = oCarrTarData.CarrTarName
        Using db As New NGLMasBookDataContext(ConnectionString)
            If db.Books.Any(Function(x) x.BookCarrTarControl = intCarrTarControl) Then
                throwCannotDeleteRecordInUseException("Tariff Name", strCarrTarName)
            End If
        End Using
    End Sub

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the data already exists only one allowed
        IsValidKey(oData, True)
        ' IsIDValid(oData, True)
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        ''Check if the data already exists only one allowed 
        'IsValidKey(oData, True)

    End Sub

    Friend Function selectDTOData(ByVal d As LTS.CarrierTariff, ByRef db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrTarContract
        Return New DataTransferObjects.CarrTarContract With {.CarrTarControl = d.CarrTarControl,
            .CarrTarCarrierControl = d.CarrTarCarrierControl,
            .CarrTarCompControl = d.CarrTarCompControl,
            .CarrTarID = d.CarrTarID,
            .CarrTarBPBracketType = d.CarrTarBPBracketType,
            .CarrTarTLCapacityType = d.CarrTarTLCapacityType,
            .CarrTarTempType = d.CarrTarTempType,
            .CarrTarTariffType = If(Asc(d.CarrTarTariffType) < 1, "I", d.CarrTarTariffType),
            .CarrTarDefWgt = d.CarrTarDefWgt,
            .CarrTarEffDateFrom = d.CarrTarEffDateFrom,
            .CarrTarEffDateTo = d.CarrTarEffDateTo,
            .CarrTarAutoAssignPro = d.CarrTarAutoAssignPro,
            .CarrTarTariffTypeControl = d.CarrTarTariffTypeControl,
            .CarrTarTariffModeTypeControl = d.CarrTarTariffModeTypeControl,
            .CarrTarName = d.CarrTarName,
            .CarrTarRevisionNumber = d.CarrTarRevisionNumber,
            .CarrTarApproved = d.CarrTarApproved,
            .CarrTarApprovedDate = d.CarrTarApprovedDate,
            .CarrTarApprovedBy = d.CarrTarApprovedBy,
            .CarrTarRejected = d.CarrTarRejected,
            .CarrTarRejectedDate = d.CarrTarRejectedDate,
            .CarrTarRejectedBy = d.CarrTarRejectedBy,
            .CarrTarOutbound = d.CarrTarOutbound,
            .CarrTarAgentControl = d.CarrTarAgentControl,
            .CarrTarIssuedDate = d.CarrTarIssuedDate,
            .CarrTarPreCloneControl = d.CarrTarPreCloneControl,
            .CarrTarUser1 = d.CarrTarUser1,
            .CarrTarUser2 = d.CarrTarUser2,
            .CarrTarUser3 = d.CarrTarUser3,
            .CarrTarUser4 = d.CarrTarUser4,
            .CarrTarWillDriveSunday = d.CarrTarWillDriveSunday,
            .CarrTarWillDriveSaturday = d.CarrTarWillDriveSaturday,
            .CarrTarModUser = d.CarrTarModUser,
            .CarrTarModDate = d.CarrTarModDate,
            .CarrTarUpdated = d.CarrTarUpdated.ToArray(),
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize}
    End Function

    Friend Function selectDTOCarrierTarrifsPivotData(ByVal d As LTS.udfCarrierTariffsPivotResult, ByRef db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrierTariffsPivot
        Return New DataTransferObjects.CarrierTariffsPivot With {.CarrierControl = d.CarrierControl,
            .CarrierName = d.CarrierName,
            .CarrierNumber = d.CarrierNumber,
            .CarrTarControl = d.CarrTarControl,
            .CarrTarName = d.CarrTarName,
            .CarrTarID = d.CarrTarID,
            .CarrTarTempType = d.CarrTarTempType,
            .CarrTarDefWgt = d.CarrTarDefWgt,
            .CarrTarEffDateFrom = d.CarrTarEffDateFrom,
            .CarrTarEffDateTo = d.CarrTarEffDateTo,
            .CarrTarAutoAssignPro = d.CarrTarAutoAssignPro,
            .CarrTarTariffTypeControl = d.CarrTarTariffTypeControl,
            .CarrTarTariffModeTypeControl = d.CarrTarTariffModeTypeControl,
            .CarrTarRevisionNumber = d.CarrTarRevisionNumber,
            .CarrTarOutbound = d.CarrTarOutbound,
            .CarrTarWillDriveSunday = d.CarrTarWillDriveSunday,
            .CarrTarWillDriveSaturday = d.CarrTarWillDriveSaturday,
            .CarrTarAgentControl = d.CarrTarAgentControl,
            .CarrTarEquipControl = d.CarrTarEquipControl,
            .CarrTarEquipCarrierEquipControl = d.CarrTarEquipCarrierEquipControl,
            .CarrTarEquipName = d.CarrTarEquipName,
            .CarrTarEquipDescription = d.CarrTarEquipDescription,
            .CarrTarEquipMatControl = d.CarrTarEquipMatControl,
            .CarrTarEquipMatName = d.CarrTarEquipMatName,
            .CarrTarEquipMatCarrTarMatBPControl = d.CarrTarEquipMatCarrTarMatBPControl,
            .CarrTarEquipMatFromZip = d.CarrTarEquipMatFromZip,
            .CarrTarEquipMatToZip = d.CarrTarEquipMatToZip,
            .CarrTarEquipMatMin = d.CarrTarEquipMatMin,
            .CarrTarEquipMatMaxDays = d.CarrTarEquipMatMaxDays,
            .CarrTarEquipMatClass = d.CarrTarEquipMatClass,
            .CarrTarEquipMatCountry = d.CarrTarEquipMatCountry,
            .CarrTarEquipMatState = d.CarrTarEquipMatState,
            .CarrTarEquipMatCity = d.CarrTarEquipMatCity,
            .CarrTarEquipMatClassTypeControl = d.CarrTarEquipMatClassTypeControl,
            .CarrTarEquipMatTarRateTypeControl = d.CarrTarEquipMatTarRateTypeControl,
            .CarrTarEquipMatLaneControl = d.CarrTarEquipMatLaneControl,
            .CarrTarEquipMatTarBracketTypeControl = d.CarrTarEquipMatTarBracketTypeControl,
            .Val1 = d.Val1,
            .Val2 = d.Val2,
            .Val3 = d.Val3,
            .Val4 = d.Val4,
            .Val5 = d.Val5,
            .Val6 = d.Val6,
            .Val7 = d.Val7,
            .Val8 = d.Val8,
            .Val9 = d.Val9,
            .Val10 = d.Val10,
            .RatedBookControl = If(d.RatedBookControl, 0),
            .MixedCount = If(d.MixedCount, 0),
            .AllowFlatRate = If(d.AllowFlatRate, True),
            .AllowDistanceRate = If(d.AllowDistanceRate, True),
            .AllowLTLRate = If(d.AllowLTLRate, True),
            .AllowUOMRate = If(d.AllowUOMRate, True),
            .CarrTarEquipMatModUser = d.CarrTarEquipMatModUser,
            .CarrTarEquipMatModDate = d.CarrTarEquipMatModDate,
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize,
            .CarrTarEquipMatOrigZip = d.CarrTarEquipMatOrigZip,
            .CarrTarEquipMultiOrigRating = d.CarrTarEquipMultiOrigRating} ' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
    End Function

    Friend Function selectDTOCarrierTarrifsPivotData(ByVal d As LTS.udfCarrierTariffsPivotByEquipResult, ByRef db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrierTariffsPivot
        Return New DataTransferObjects.CarrierTariffsPivot With {.CarrierControl = d.CarrierControl,
            .CarrierName = d.CarrierName,
            .CarrierNumber = d.CarrierNumber,
            .CarrTarControl = d.CarrTarControl,
            .CarrTarName = d.CarrTarName,
            .CarrTarID = d.CarrTarID,
            .CarrTarTempType = d.CarrTarTempType,
            .CarrTarDefWgt = d.CarrTarDefWgt,
            .CarrTarEffDateFrom = d.CarrTarEffDateFrom,
            .CarrTarEffDateTo = d.CarrTarEffDateTo,
            .CarrTarAutoAssignPro = d.CarrTarAutoAssignPro,
            .CarrTarTariffTypeControl = d.CarrTarTariffTypeControl,
            .CarrTarTariffModeTypeControl = d.CarrTarTariffModeTypeControl,
            .CarrTarRevisionNumber = d.CarrTarRevisionNumber,
            .CarrTarOutbound = d.CarrTarOutbound,
            .CarrTarWillDriveSunday = d.CarrTarWillDriveSunday,
            .CarrTarWillDriveSaturday = d.CarrTarWillDriveSaturday,
            .CarrTarAgentControl = d.CarrTarAgentControl,
            .CarrTarEquipControl = d.CarrTarEquipControl,
            .CarrTarEquipCarrierEquipControl = d.CarrTarEquipCarrierEquipControl,
            .CarrTarEquipName = d.CarrTarEquipName,
            .CarrTarEquipDescription = d.CarrTarEquipDescription,
            .CarrTarEquipMatControl = d.CarrTarEquipMatControl,
            .CarrTarEquipMatName = d.CarrTarEquipMatName,
            .CarrTarEquipMatCarrTarMatBPControl = d.CarrTarEquipMatCarrTarMatBPControl,
            .CarrTarEquipMatFromZip = d.CarrTarEquipMatFromZip,
            .CarrTarEquipMatToZip = d.CarrTarEquipMatToZip,
            .CarrTarEquipMatMin = d.CarrTarEquipMatMin,
            .CarrTarEquipMatMaxDays = d.CarrTarEquipMatMaxDays,
            .CarrTarEquipMatClass = d.CarrTarEquipMatClass,
            .CarrTarEquipMatCountry = d.CarrTarEquipMatCountry,
            .CarrTarEquipMatState = d.CarrTarEquipMatState,
            .CarrTarEquipMatCity = d.CarrTarEquipMatCity,
            .CarrTarEquipMatClassTypeControl = d.CarrTarEquipMatClassTypeControl,
            .CarrTarEquipMatTarRateTypeControl = d.CarrTarEquipMatTarRateTypeControl,
            .CarrTarEquipMatLaneControl = d.CarrTarEquipMatLaneControl,
            .CarrTarEquipMatTarBracketTypeControl = d.CarrTarEquipMatTarBracketTypeControl,
            .Val1 = d.Val1,
            .Val2 = d.Val2,
            .Val3 = d.Val3,
            .Val4 = d.Val4,
            .Val5 = d.Val5,
            .Val6 = d.Val6,
            .Val7 = d.Val7,
            .Val8 = d.Val8,
            .Val9 = d.Val9,
            .Val10 = d.Val10,
            .RatedBookControl = If(d.RatedBookControl, 0),
            .MixedCount = If(d.MixedCount, 0),
            .AllowFlatRate = If(d.AllowFlatRate, True),
            .AllowDistanceRate = If(d.AllowDistanceRate, True),
            .AllowLTLRate = If(d.AllowLTLRate, True),
            .AllowUOMRate = If(d.AllowUOMRate, True),
            .CarrTarEquipMatModUser = d.CarrTarEquipMatModUser,
            .CarrTarEquipMatModDate = d.CarrTarEquipMatModDate,
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize,
            .CarrTarEquipMatOrigZip = d.CarrTarEquipMatOrigZip,
            .CarrTarEquipMultiOrigRating = d.CarrTarEquipMultiOrigRating} ' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
    End Function

    Friend Function selectDTOCarrierTarrifsPivotData(ByVal d As LTS.spCarrierTariffsPivotResult, ByRef db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrierTariffsPivot


        Dim skipObjs As New List(Of String) From {"Page", "Pages", "RecordCount", "PageSize"}
        Dim oRet As New DataTransferObjects.CarrierTariffsPivot
        oRet = CopyMatchingFields(oRet, d, skipObjs)
        'update settings
        With oRet
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oRet

    End Function

    Friend Function selectDTOCarrierTarrifsPivotData(ByVal d As LTS.udfCarrierTariffsPivotQuoteResult, ByRef db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrierTariffsPivot


        Dim skipObjs As New List(Of String) From {"Page", "Pages", "RecordCount", "PageSize"}
        Dim oRet As New DataTransferObjects.CarrierTariffsPivot
        oRet = CopyMatchingFields(oRet, d, skipObjs)
        'update settings
        With oRet
            .RatedBookControl = 0
            .MixedCount = 0
            .AllowFlatRate = True
            .AllowDistanceRate = True
            .AllowLTLRate = True
            .AllowUOMRate = True
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oRet

    End Function

    Friend Function selectDTOCarrierTarrifPivotData(ByVal d As LTS.udfCarrierTariffPivotResult) As DataTransferObjects.CarrierTariffsPivot
        Return New DataTransferObjects.CarrierTariffsPivot With {.CarrierControl = d.CarrierControl,
            .CarrierName = d.CarrierName,
            .CarrierNumber = d.CarrierNumber,
            .CarrTarControl = d.CarrTarControl,
            .CarrTarName = d.CarrTarName,
            .CarrTarID = d.CarrTarID,
            .CarrTarTempType = d.CarrTarTempType,
            .CarrTarDefWgt = d.CarrTarDefWgt,
            .CarrTarEffDateFrom = d.CarrTarEffDateFrom,
            .CarrTarEffDateTo = d.CarrTarEffDateTo,
            .CarrTarAutoAssignPro = d.CarrTarAutoAssignPro,
            .CarrTarTariffTypeControl = d.CarrTarTariffTypeControl,
            .CarrTarTariffModeTypeControl = d.CarrTarTariffModeTypeControl,
            .CarrTarRevisionNumber = d.CarrTarRevisionNumber,
            .CarrTarOutbound = d.CarrTarOutbound,
            .CarrTarWillDriveSunday = d.CarrTarWillDriveSunday,
            .CarrTarWillDriveSaturday = d.CarrTarWillDriveSaturday,
            .CarrTarAgentControl = d.CarrTarAgentControl,
            .CarrTarEquipControl = d.CarrTarEquipControl,
            .CarrTarEquipCarrierEquipControl = d.CarrTarEquipCarrierEquipControl,
            .CarrTarEquipName = d.CarrTarEquipName,
            .CarrTarEquipDescription = d.CarrTarEquipDescription,
            .CarrTarEquipMatControl = d.CarrTarEquipMatControl,
            .CarrTarEquipMatName = d.CarrTarEquipMatName,
            .CarrTarEquipMatCarrTarMatBPControl = d.CarrTarEquipMatCarrTarMatBPControl,
            .CarrTarEquipMatFromZip = d.CarrTarEquipMatFromZip,
            .CarrTarEquipMatToZip = d.CarrTarEquipMatToZip,
            .CarrTarEquipMatMin = d.CarrTarEquipMatMin,
            .CarrTarEquipMatMaxDays = d.CarrTarEquipMatMaxDays,
            .CarrTarEquipMatClass = d.CarrTarEquipMatClass,
            .CarrTarEquipMatCountry = d.CarrTarEquipMatCountry,
            .CarrTarEquipMatState = d.CarrTarEquipMatState,
            .CarrTarEquipMatCity = d.CarrTarEquipMatCity,
            .CarrTarEquipMatClassTypeControl = d.CarrTarEquipMatClassTypeControl,
            .CarrTarEquipMatTarRateTypeControl = d.CarrTarEquipMatTarRateTypeControl,
            .CarrTarEquipMatLaneControl = d.CarrTarEquipMatLaneControl,
            .CarrTarEquipMatTarBracketTypeControl = d.CarrTarEquipMatTarBracketTypeControl,
            .Val1 = d.Val1,
            .Val2 = d.Val2,
            .Val3 = d.Val3,
            .Val4 = d.Val4,
            .Val5 = d.Val5,
            .Val6 = d.Val6,
            .Val7 = d.Val7,
            .Val8 = d.Val8,
            .Val9 = d.Val9,
            .Val10 = d.Val10,
            .RatedBookControl = 0,
            .MixedCount = 0,
            .AllowFlatRate = True,
            .AllowDistanceRate = True,
            .AllowLTLRate = True,
            .AllowUOMRate = True,
            .CarrTarEquipMatModUser = d.CarrTarEquipMatModUser,
            .CarrTarEquipMatModDate = d.CarrTarEquipMatModDate,
            .Page = 1,
            .Pages = 1,
            .RecordCount = 1,
            .PageSize = 1,
            .CarrTarEquipMatOrigZip = d.CarrTarEquipMatOrigZip,
            .CarrTarEquipMultiOrigRating = d.CarrTarEquipMultiOrigRating} ' Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
    End Function


#End Region

End Class