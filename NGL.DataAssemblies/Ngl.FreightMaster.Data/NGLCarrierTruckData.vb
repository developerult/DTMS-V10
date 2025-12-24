Imports System.Data.Linq
Imports System.ServiceModel

Public Class NGLCarrierTruckData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierTrucks
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierTruckData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierTrucks
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
        Return GetCarrierTruckFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrierTrucksFiltered()
    End Function

    Public Function GetCarrierTruckFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.CarrierTruck
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CarrierTruck As DataTransferObjects.CarrierTruck = (
                        From t In db.CarrierTrucks
                        Where
                        (t.CarrierTruckControl = If(Control = 0, t.CarrierTruckControl, Control))
                        Order By t.CarrierTruckControl Descending
                        Select New DataTransferObjects.CarrierTruck With {.CarrierTruckControl = t.CarrierTruckControl _
                        , .CarrierTruckCarrierControl = If(t.CarrierTruckCarrierControl.HasValue, t.CarrierTruckCarrierControl.Value, 0) _
                        , .CarrierTruckDescription = t.CarrierTruckDescription _
                        , .CarrierTruckWgtFrom = If(t.CarrierTruckWgtFrom.HasValue, t.CarrierTruckWgtFrom.Value, 0) _
                        , .CarrierTruckWgtTo = If(t.CarrierTruckWgtTo.HasValue, t.CarrierTruckWgtTo.Value, 0) _
                        , .CarrierTruckRateStarts = t.CarrierTruckRateStarts _
                        , .CarrierTruckRateExpires = t.CarrierTruckRateExpires _
                        , .CarrierTruckTL = t.CarrierTruckTL _
                        , .CarrierTruckLTL = t.CarrierTruckLTL _
                        , .CarrierTruckEquipment = t.CarrierTruckEquipment _
                        , .CarrierTruckMileRate = If(t.CarrierTruckMileRate.HasValue, t.CarrierTruckMileRate.Value, 0) _
                        , .CarrierTruckCwtRate = If(t.CarrierTruckCwtRate.HasValue, t.CarrierTruckCwtRate.Value, 0) _
                        , .CarrierTruckCaseRate = If(t.CarrierTruckCaseRate.HasValue, t.CarrierTruckCaseRate.Value, 0) _
                        , .CarrierTruckFlatRate = If(t.CarrierTruckFlatRate.HasValue, t.CarrierTruckFlatRate.Value, 0) _
                        , .CarrierTruckPltRate = If(t.CarrierTruckPltRate.HasValue, t.CarrierTruckPltRate.Value, 0) _
                        , .CarrierTruckCubeRate = If(t.CarrierTruckCubeRate.HasValue, t.CarrierTruckCubeRate.Value, 0) _
                        , .CarrierTruckTLT = If(t.CarrierTruckTLT.HasValue, t.CarrierTruckTLT.Value, 0) _
                        , .CarrierTruckTMode = t.CarrierTruckTMode _
                        , .CarrierTruckFAK = t.CarrierTruckFAK _
                        , .CarrierTruckDisc = If(t.CarrierTruckDisc.HasValue, t.CarrierTruckDisc.Value, 0) _
                        , .CarrierTruckPUMon = t.CarrierTruckPUMon _
                        , .CarrierTruckPUTue = t.CarrierTruckPUTue _
                        , .CarrierTruckPUWed = t.CarrierTruckPUWed _
                        , .CarrierTruckPUThu = t.CarrierTruckPUThu _
                        , .CarrierTruckPUFri = t.CarrierTruckPUFri _
                        , .CarrierTruckPUSat = t.CarrierTruckPUSat _
                        , .CarrierTruckPUSun = t.CarrierTruckPUSun _
                        , .CarrierTruckDLMon = t.CarrierTruckDLMon _
                        , .CarrierTruckDLTue = t.CarrierTruckDLTue _
                        , .CarrierTruckDLWed = t.CarrierTruckDLWed _
                        , .CarrierTruckDLThu = t.CarrierTruckDLThu _
                        , .CarrierTruckDLFri = t.CarrierTruckDLFri _
                        , .CarrierTruckDLSat = t.CarrierTruckDLSat _
                        , .CarrierTruckDLSun = t.CarrierTruckDLSun _
                        , .CarrierTruckPayTolPerLo = If(t.CarrierTruckPayTolPerLo.HasValue, t.CarrierTruckPayTolPerLo.Value, 0) _
                        , .CarrierTruckPayTolPerHi = If(t.CarrierTruckPayTolPerHi.HasValue, t.CarrierTruckPayTolPerHi.Value, 0) _
                        , .CarrierTruckPayTolCurLo = If(t.CarrierTruckPayTolCurLo.HasValue, t.CarrierTruckPayTolCurLo.Value, 0) _
                        , .CarrierTruckPayTolCurHi = If(t.CarrierTruckPayTolCurHi.HasValue, t.CarrierTruckPayTolCurHi.Value, 0) _
                        , .CarrierTruckCurType = If(t.CarrierTruckCurType.HasValue, t.CarrierTruckCurType.Value, 0) _
                        , .CarrierTruckModUser = t.CarrierTruckModUser _
                        , .CarrierTruckModDate = t.CarrierTruckModDate _
                        , .CarrierTruckRoute = t.CarrierTruckRoute _
                        , .CarrierTruckMiles = If(t.CarrierTruckMiles.HasValue, t.CarrierTruckMiles.Value, 0) _
                        , .CarrierTruckBkhlCostPerc = If(t.CarrierTruckBkhlCostPerc.HasValue, t.CarrierTruckBkhlCostPerc.Value, 0) _
                        , .CarrierTruckPalletCostPer = If(t.CarrierTruckPalletCostPer.HasValue, t.CarrierTruckPalletCostPer.Value, 0) _
                        , .CarrierTruckFuelSurChargePerc = If(t.CarrierTruckFuelSurChargePerc.HasValue, t.CarrierTruckFuelSurChargePerc.Value, 0) _
                        , .CarrierTruckStopCharge = If(t.CarrierTruckStopCharge.HasValue, t.CarrierTruckStopCharge.Value, 0) _
                        , .CarrierTruckDropCost = t.CarrierTruckDropCost _
                        , .CarrierTruckUnloadDiff = t.CarrierTruckUnloadDiff _
                        , .CarrierTruckCasesAvailable = t.CarrierTruckCasesAvailable _
                        , .CarrierTruckCasesOpen = t.CarrierTruckCasesOpen _
                        , .CarrierTruckCasesCommitted = t.CarrierTruckCasesCommitted _
                        , .CarrierTruckWgtAvailable = t.CarrierTruckWgtAvailable _
                        , .CarrierTruckWgtOpen = t.CarrierTruckWgtOpen _
                        , .CarrierTruckWgtCommitted = t.CarrierTruckWgtCommitted _
                        , .CarrierTruckCubesAvailable = t.CarrierTruckCubesAvailable _
                        , .CarrierTruckCubesOpen = t.CarrierTruckCubesOpen _
                        , .CarrierTruckCubesCommitted = t.CarrierTruckCubesCommitted _
                        , .CarrierTruckPltsAvailable = t.CarrierTruckPltsAvailable _
                        , .CarrierTruckPltsOpen = t.CarrierTruckPltsOpen _
                        , .CarrierTruckPltsCommitted = t.CarrierTruckPltsCommitted _
                        , .CarrierTruckTrucksAvailable = t.CarrierTruckTrucksAvailable _
                        , .CarrierTruckMaxLoadsByWeek = t.CarrierTruckMaxLoadsByWeek _
                        , .CarrierTruckMaxLoadsByMonth = t.CarrierTruckMaxLoadsByMonth _
                        , .CarrierTruckTotalLoadsForWeek = t.CarrierTruckTotalLoadsForWeek _
                        , .CarrierTruckTotalLoadsForMonth = t.CarrierTruckTotalLoadsForMonth _
                        , .CarrierTruckWeekDate = t.CarrierTruckWeekDate _
                        , .CarrierTruckMonthDate = t.CarrierTruckMonthDate _
                        , .CarrierTruckTempType = t.CarrierTruckTempType _
                        , .CarrierTruckHazmat = t.CarrierTruckHazmat _
                        , .LocalCarrierTruckCodes = db.getCarrierTruckCodeString(t.CarrierTruckControl) _
                        , .CarrierTruckUpdated = t.CarrierTruckUpdated.ToArray()}).First
                Return CarrierTruck

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

    Public Function GetCarrierTrucksFiltered(Optional ByVal CarrierControl As Integer = 0) As DataTransferObjects.CarrierTruck()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CarrierTrucks() As DataTransferObjects.CarrierTruck = (
                        From t In db.CarrierTrucks
                        Where
                        (t.CarrierTruckCarrierControl = If(CarrierControl = 0, t.CarrierTruckCarrierControl, CarrierControl))
                        Order By t.CarrierTruckControl
                        Select New DataTransferObjects.CarrierTruck With {.CarrierTruckControl = t.CarrierTruckControl _
                        , .CarrierTruckCarrierControl = If(t.CarrierTruckCarrierControl.HasValue, t.CarrierTruckCarrierControl.Value, 0) _
                        , .CarrierTruckDescription = t.CarrierTruckDescription _
                        , .CarrierTruckWgtFrom = If(t.CarrierTruckWgtFrom.HasValue, t.CarrierTruckWgtFrom.Value, 0) _
                        , .CarrierTruckWgtTo = If(t.CarrierTruckWgtTo.HasValue, t.CarrierTruckWgtTo.Value, 0) _
                        , .CarrierTruckRateStarts = t.CarrierTruckRateStarts _
                        , .CarrierTruckRateExpires = t.CarrierTruckRateExpires _
                        , .CarrierTruckTL = t.CarrierTruckTL _
                        , .CarrierTruckLTL = t.CarrierTruckLTL _
                        , .CarrierTruckEquipment = t.CarrierTruckEquipment _
                        , .CarrierTruckMileRate = If(t.CarrierTruckMileRate.HasValue, t.CarrierTruckMileRate.Value, 0) _
                        , .CarrierTruckCwtRate = If(t.CarrierTruckCwtRate.HasValue, t.CarrierTruckCwtRate.Value, 0) _
                        , .CarrierTruckCaseRate = If(t.CarrierTruckCaseRate.HasValue, t.CarrierTruckCaseRate.Value, 0) _
                        , .CarrierTruckFlatRate = If(t.CarrierTruckFlatRate.HasValue, t.CarrierTruckFlatRate.Value, 0) _
                        , .CarrierTruckPltRate = If(t.CarrierTruckPltRate.HasValue, t.CarrierTruckPltRate.Value, 0) _
                        , .CarrierTruckCubeRate = If(t.CarrierTruckCubeRate.HasValue, t.CarrierTruckCubeRate.Value, 0) _
                        , .CarrierTruckTLT = If(t.CarrierTruckTLT.HasValue, t.CarrierTruckTLT.Value, 0) _
                        , .CarrierTruckTMode = t.CarrierTruckTMode _
                        , .CarrierTruckFAK = t.CarrierTruckFAK _
                        , .CarrierTruckDisc = If(t.CarrierTruckDisc.HasValue, t.CarrierTruckDisc.Value, 0) _
                        , .CarrierTruckPUMon = t.CarrierTruckPUMon _
                        , .CarrierTruckPUTue = t.CarrierTruckPUTue _
                        , .CarrierTruckPUWed = t.CarrierTruckPUWed _
                        , .CarrierTruckPUThu = t.CarrierTruckPUThu _
                        , .CarrierTruckPUFri = t.CarrierTruckPUFri _
                        , .CarrierTruckPUSat = t.CarrierTruckPUSat _
                        , .CarrierTruckPUSun = t.CarrierTruckPUSun _
                        , .CarrierTruckDLMon = t.CarrierTruckDLMon _
                        , .CarrierTruckDLTue = t.CarrierTruckDLTue _
                        , .CarrierTruckDLWed = t.CarrierTruckDLWed _
                        , .CarrierTruckDLThu = t.CarrierTruckDLThu _
                        , .CarrierTruckDLFri = t.CarrierTruckDLFri _
                        , .CarrierTruckDLSat = t.CarrierTruckDLSat _
                        , .CarrierTruckDLSun = t.CarrierTruckDLSun _
                        , .CarrierTruckPayTolPerLo = If(t.CarrierTruckPayTolPerLo.HasValue, t.CarrierTruckPayTolPerLo.Value, 0) _
                        , .CarrierTruckPayTolPerHi = If(t.CarrierTruckPayTolPerHi.HasValue, t.CarrierTruckPayTolPerHi.Value, 0) _
                        , .CarrierTruckPayTolCurLo = If(t.CarrierTruckPayTolCurLo.HasValue, t.CarrierTruckPayTolCurLo.Value, 0) _
                        , .CarrierTruckPayTolCurHi = If(t.CarrierTruckPayTolCurHi.HasValue, t.CarrierTruckPayTolCurHi.Value, 0) _
                        , .CarrierTruckCurType = If(t.CarrierTruckCurType.HasValue, t.CarrierTruckCurType.Value, 0) _
                        , .CarrierTruckModUser = t.CarrierTruckModUser _
                        , .CarrierTruckModDate = t.CarrierTruckModDate _
                        , .CarrierTruckRoute = t.CarrierTruckRoute _
                        , .CarrierTruckMiles = If(t.CarrierTruckMiles.HasValue, t.CarrierTruckMiles.Value, 0) _
                        , .CarrierTruckBkhlCostPerc = If(t.CarrierTruckBkhlCostPerc.HasValue, t.CarrierTruckBkhlCostPerc.Value, 0) _
                        , .CarrierTruckPalletCostPer = If(t.CarrierTruckPalletCostPer.HasValue, t.CarrierTruckPalletCostPer.Value, 0) _
                        , .CarrierTruckFuelSurChargePerc = If(t.CarrierTruckFuelSurChargePerc.HasValue, t.CarrierTruckFuelSurChargePerc.Value, 0) _
                        , .CarrierTruckStopCharge = If(t.CarrierTruckStopCharge.HasValue, t.CarrierTruckStopCharge.Value, 0) _
                        , .CarrierTruckDropCost = t.CarrierTruckDropCost _
                        , .CarrierTruckUnloadDiff = t.CarrierTruckUnloadDiff _
                        , .CarrierTruckCasesAvailable = t.CarrierTruckCasesAvailable _
                        , .CarrierTruckCasesOpen = t.CarrierTruckCasesOpen _
                        , .CarrierTruckCasesCommitted = t.CarrierTruckCasesCommitted _
                        , .CarrierTruckWgtAvailable = t.CarrierTruckWgtAvailable _
                        , .CarrierTruckWgtOpen = t.CarrierTruckWgtOpen _
                        , .CarrierTruckWgtCommitted = t.CarrierTruckWgtCommitted _
                        , .CarrierTruckCubesAvailable = t.CarrierTruckCubesAvailable _
                        , .CarrierTruckCubesOpen = t.CarrierTruckCubesOpen _
                        , .CarrierTruckCubesCommitted = t.CarrierTruckCubesCommitted _
                        , .CarrierTruckPltsAvailable = t.CarrierTruckPltsAvailable _
                        , .CarrierTruckPltsOpen = t.CarrierTruckPltsOpen _
                        , .CarrierTruckPltsCommitted = t.CarrierTruckPltsCommitted _
                        , .CarrierTruckTrucksAvailable = t.CarrierTruckTrucksAvailable _
                        , .CarrierTruckMaxLoadsByWeek = t.CarrierTruckMaxLoadsByWeek _
                        , .CarrierTruckMaxLoadsByMonth = t.CarrierTruckMaxLoadsByMonth _
                        , .CarrierTruckTotalLoadsForWeek = t.CarrierTruckTotalLoadsForWeek _
                        , .CarrierTruckTotalLoadsForMonth = t.CarrierTruckTotalLoadsForMonth _
                        , .CarrierTruckWeekDate = t.CarrierTruckWeekDate _
                        , .CarrierTruckMonthDate = t.CarrierTruckMonthDate _
                        , .CarrierTruckTempType = t.CarrierTruckTempType _
                        , .CarrierTruckHazmat = t.CarrierTruckHazmat _
                        , .LocalCarrierTruckCodes = db.getCarrierTruckCodeString(t.CarrierTruckControl) _
                        , .CarrierTruckUpdated = t.CarrierTruckUpdated.ToArray()}).ToArray()
                Return CarrierTrucks

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_Nt"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function
    ''' <summary>
    ''' Method to Get the All Carrier Trucks based on Filters 
    ''' </summary>
    ''' <param name="filters">Filters</param>
    ''' <param name="RecordCount">No Of Records</param>
    ''' <remarks>
    ''' Added By ManoRama on 02-Sep-2020 for Carrier Equipment Changes
    ''' </remarks>
    ''' <returns>List of vMasterTruckList</returns>
    Public Function GetCarrierTrucks365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vMasterTruckList()
        If filters Is Nothing Then Return Nothing
        Dim iCarrierControl As Integer = 0 'Parent Control Number
        Dim iCarrierTruckControl As Integer = 0 'table primary key
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        Dim iLECarControl As Integer = 0
        Dim oRet() As LTS.vMasterTruckList 'return the table or view
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                If (filters.FilterValues Is Nothing OrElse filters.FilterValues.Count() < 1) OrElse (Not filters.FilterValues.Any(Function(x) x.filterName = "CarrierTruckControl")) Then
                    'The Record Control Filter does not exist so use the parent control fliter
                    If filters.ParentControl = 0 Then
                        throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
                    End If
                    'Modified by RHR for v-8.3.0.001 on 09/17/2020 need to lookup carrier control number from tblLegalEntityCarriers
                    iLECarControl = filters.ParentControl
                    iCarrierControl = db.tblLegalEntityCarriers.Where(Function(x) x.LECarControl = iLECarControl).Select(Function(x) x.LECarCarrierControl).FirstOrDefault()

                    If iCarrierControl = 0 Then
                        throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
                    End If
                    filterWhere = " (CarrierTruckCarrierControl = " & iCarrierControl.ToString() & ") "
                    sFilterSpacer = " And "
                Else
                    Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "CarrierTruckControl").FirstOrDefault()
                    Integer.TryParse(tFilter.filterValueFrom, iCarrierTruckControl)
                End If

                Dim iQuery As IQueryable(Of LTS.vMasterTruckList)
                iQuery = db.vMasterTruckLists
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "CarrierTruckDescription"
                    filters.sortDirection = "ASC"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierTrucks365"), db)
            End Try
            Return oRet
        End Using
    End Function
    ''' <summary>
    ''' Method to Create and Save the Carrier Trucks.
    ''' </summary>
    ''' <param name="oData">LTS CarrierTruck</param>
    ''' <remarks>
    ''' Added By ManoRama On 03-09-2020 For Carrier Equipment Changes
    ''' </remarks>
    ''' <returns>True/False</returns>
    Public Function SaveOrCreateCarrierTruckItem(ByVal oData As LTS.CarrierTruck) As Boolean
        Dim blnRet As Boolean = False
        Dim iLECarControl As Integer
        Dim iCarrierControl As Integer
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim iCarrierTruckCarrierControl = oData.CarrierTruckCarrierControl

                If oData.CarrierTruckCarrierControl = 0 Then
                    If oData.CarrierTruckControl = 0 Then
                        Dim sMsg As String = "E_MissingParent" ' "  The reference to the parent Carrier record is missing. Please select a valid Carrier record from the LeCarrierMaint  page and try again."
                        throwNoDataFaultException(sMsg)
                    End If
                End If
                ' need to lookup carrier control number from tblLegalEntityCarriers
                iLECarControl = oData.CarrierTruckCarrierControl
                iCarrierControl = db.tblLegalEntityCarriers.Where(Function(x) x.LECarControl = iLECarControl).Select(Function(x) x.LECarCarrierControl).FirstOrDefault()

                If iCarrierControl = 0 Then
                    throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
                End If
                oData.CarrierTruckCarrierControl = iCarrierControl
                oData.CarrierTruckModDate = Date.Now()
                oData.CarrierTruckModUser = Me.Parameters.UserName

                If oData.CarrierTruckControl = 0 Then
                    db.CarrierTrucks.InsertOnSubmit(oData)
                Else
                    db.CarrierTrucks.Attach(oData, True)
                End If
                db.SubmitChanges(ConflictMode.ContinueOnConflict)
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveOrCreateCarrierTruckItem"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Method to Delete the CarrieTruck
    ''' </summary>
    ''' <param name="iCarriertruckControl">Pk CarrierTruckControl</param>
    ''' <remarks>
    ''' Added By ManoRama On 03-09-2020 For Carrier Equipment Changes
    ''' </remarks>
    ''' <returns>True/False</returns>
    Public Function DeleteCarrierTruckItem(ByVal iCarriertruckControl As Integer) As Boolean
        Dim blnRet As Boolean = False

        If iCarriertruckControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'verify that the truck exists
                Dim oExisting = db.CarrierTrucks.Where(Function(x) x.CarrierTruckControl = iCarriertruckControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.CarrierTruckControl = 0 Then Return True
                db.CarrierTrucks.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrierTruckItem"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrierTruck)
        'Create New Record
        Return New LTS.CarrierTruck With {.CarrierTruckControl = d.CarrierTruckControl _
            , .CarrierTruckCarrierControl = d.CarrierTruckCarrierControl _
            , .CarrierTruckDescription = d.CarrierTruckDescription _
            , .CarrierTruckWgtFrom = d.CarrierTruckWgtFrom _
            , .CarrierTruckWgtTo = d.CarrierTruckWgtTo _
            , .CarrierTruckRateStarts = d.CarrierTruckRateStarts _
            , .CarrierTruckRateExpires = d.CarrierTruckRateExpires _
            , .CarrierTruckTL = d.CarrierTruckTL _
            , .CarrierTruckLTL = d.CarrierTruckLTL _
            , .CarrierTruckEquipment = d.CarrierTruckEquipment _
            , .CarrierTruckMileRate = d.CarrierTruckMileRate _
            , .CarrierTruckCwtRate = d.CarrierTruckCwtRate _
            , .CarrierTruckCaseRate = d.CarrierTruckCaseRate _
            , .CarrierTruckFlatRate = d.CarrierTruckFlatRate _
            , .CarrierTruckPltRate = d.CarrierTruckPltRate _
            , .CarrierTruckCubeRate = d.CarrierTruckCubeRate _
            , .CarrierTruckTLT = d.CarrierTruckTLT _
            , .CarrierTruckTMode = d.CarrierTruckTMode _
            , .CarrierTruckFAK = d.CarrierTruckFAK _
            , .CarrierTruckDisc = d.CarrierTruckDisc _
            , .CarrierTruckPUMon = d.CarrierTruckPUMon _
            , .CarrierTruckPUTue = d.CarrierTruckPUTue _
            , .CarrierTruckPUWed = d.CarrierTruckPUWed _
            , .CarrierTruckPUThu = d.CarrierTruckPUThu _
            , .CarrierTruckPUFri = d.CarrierTruckPUFri _
            , .CarrierTruckPUSat = d.CarrierTruckPUSat _
            , .CarrierTruckPUSun = d.CarrierTruckPUSun _
            , .CarrierTruckDLMon = d.CarrierTruckDLMon _
            , .CarrierTruckDLTue = d.CarrierTruckDLTue _
            , .CarrierTruckDLWed = d.CarrierTruckDLWed _
            , .CarrierTruckDLThu = d.CarrierTruckDLThu _
            , .CarrierTruckDLFri = d.CarrierTruckDLFri _
            , .CarrierTruckDLSat = d.CarrierTruckDLSat _
            , .CarrierTruckDLSun = d.CarrierTruckDLSun _
            , .CarrierTruckPayTolPerLo = d.CarrierTruckPayTolPerLo _
            , .CarrierTruckPayTolPerHi = d.CarrierTruckPayTolPerHi _
            , .CarrierTruckPayTolCurLo = d.CarrierTruckPayTolCurLo _
            , .CarrierTruckPayTolCurHi = d.CarrierTruckPayTolCurHi _
            , .CarrierTruckCurType = d.CarrierTruckCurType _
            , .CarrierTruckModUser = Parameters.UserName _
            , .CarrierTruckModDate = Date.Now _
            , .CarrierTruckRoute = d.CarrierTruckRoute _
            , .CarrierTruckMiles = d.CarrierTruckMiles _
            , .CarrierTruckBkhlCostPerc = d.CarrierTruckBkhlCostPerc _
            , .CarrierTruckPalletCostPer = d.CarrierTruckPalletCostPer _
            , .CarrierTruckFuelSurChargePerc = d.CarrierTruckFuelSurChargePerc _
            , .CarrierTruckStopCharge = d.CarrierTruckStopCharge _
            , .CarrierTruckDropCost = d.CarrierTruckDropCost _
            , .CarrierTruckUnloadDiff = d.CarrierTruckUnloadDiff _
            , .CarrierTruckCasesAvailable = d.CarrierTruckCasesAvailable _
            , .CarrierTruckCasesOpen = d.CarrierTruckCasesOpen _
            , .CarrierTruckCasesCommitted = d.CarrierTruckCasesCommitted _
            , .CarrierTruckWgtAvailable = d.CarrierTruckWgtAvailable _
            , .CarrierTruckWgtOpen = d.CarrierTruckWgtOpen _
            , .CarrierTruckWgtCommitted = d.CarrierTruckWgtCommitted _
            , .CarrierTruckCubesAvailable = d.CarrierTruckCubesAvailable _
            , .CarrierTruckCubesOpen = d.CarrierTruckCubesOpen _
            , .CarrierTruckCubesCommitted = d.CarrierTruckCubesCommitted _
            , .CarrierTruckPltsAvailable = d.CarrierTruckPltsAvailable _
            , .CarrierTruckPltsOpen = d.CarrierTruckPltsOpen _
            , .CarrierTruckPltsCommitted = d.CarrierTruckPltsCommitted _
            , .CarrierTruckTrucksAvailable = d.CarrierTruckTrucksAvailable _
            , .CarrierTruckMaxLoadsByWeek = d.CarrierTruckMaxLoadsByWeek _
            , .CarrierTruckMaxLoadsByMonth = d.CarrierTruckMaxLoadsByMonth _
            , .CarrierTruckTotalLoadsForWeek = d.CarrierTruckTotalLoadsForWeek _
            , .CarrierTruckTotalLoadsForMonth = d.CarrierTruckTotalLoadsForMonth _
            , .CarrierTruckWeekDate = d.CarrierTruckWeekDate _
            , .CarrierTruckMonthDate = d.CarrierTruckMonthDate _
            , .CarrierTruckTempType = d.CarrierTruckTempType _
            , .CarrierTruckHazmat = d.CarrierTruckHazmat _
            , .CarrierTruckUpdated = If(d.CarrierTruckUpdated Is Nothing, New Byte() {}, d.CarrierTruckUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrierTruckFiltered(Control:=CType(LinqTable, LTS.CarrierTruck).CarrierTruckControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierTruck = TryCast(LinqTable, LTS.CarrierTruck)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierTrucks
                    Where d.CarrierTruckControl = source.CarrierTruckControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrierTruckControl _
                        , .ModDate = d.CarrierTruckModDate _
                        , .ModUser = d.CarrierTruckModUser _
                        , .Updated = d.CarrierTruckUpdated.ToArray}).First

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