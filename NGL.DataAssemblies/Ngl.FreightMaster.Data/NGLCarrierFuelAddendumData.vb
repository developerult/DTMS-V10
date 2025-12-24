Imports System.Data.Linq
Imports System.ServiceModel
Imports Ngl.Core.ChangeTracker
Imports SerilogTracing

Public Class NGLCarrierFuelAddendumData : Inherits NDPBaseClass 'start working here

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierFuelAddendums
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierFuelAddendumData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierFuelAddendums
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
        Return GetCarrierFuelAddendumFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrierFuelAddendumsFiltered()
    End Function

    Public Function GetCarrierFuelAddendumFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.CarrierFuelAddendum
        Using Logger.StartActivity("GetCarrierFuelAddendumFiltered(Control: {Control})", Control)
            Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.CarrierFuelAddendum)(Function(t As LTS.CarrierFuelAddendum) t.CarrierFuelAdExes)
                oDLO.LoadWith(Of LTS.CarrierFuelAddendum)(Function(t As LTS.CarrierFuelAddendum) t.CarrierFuelAdRates)
                db.LoadOptions = oDLO
                'Get the newest record that matches the provided criteria
                Dim CarrierFuelAddendum As DataTransferObjects.CarrierFuelAddendum = (
                        From t In db.CarrierFuelAddendums
                        Where
                        (t.CarrFuelAdControl = If(Control = 0, t.CarrFuelAdControl, Control))
                        Select New DataTransferObjects.CarrierFuelAddendum With {.CarrFuelAdControl = t.CarrFuelAdControl _
                        , .CarrFuelAdCarrierControl = t.CarrFuelAdCarrierControl _
                        , .CarrFuelAdCarrTarControl = t.CarrFuelAdCarrTarControl _
                        , .CarrFuelAdCarrTarEquipControl = t.CarrFuelAdCarrTarEquipControl _
                        , .CarrFuelAdUseNatAvg = t.CarrFuelAdUseNatAvg _
                        , .CarrFuelAdUseZoneAvg = t.CarrFuelAdUseZoneAvg _
                        , .CarrFuelAdUseRatePerMile = t.CarrFuelAdUseRatePerMile _
                        , .CarrFuelAdDefFuelRate = t.CarrFuelAdDefFuelRate _
                        , .CarrFuelAdModUser = t.CarrFuelAdModUser _
                        , .CarrFuelAdModDate = t.CarrFuelAdModDate _
                        , .CarrFuelAdUpdated = t.CarrFuelAdUpdated.ToArray()}).Single
                Return CarrierFuelAddendum

            Catch ex As System.Data.SqlClient.SqlException
                Logger.Error(ex, "GetCarrierFuelAddendumsFiltered")
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Logger.Error(ex, "GetCarrierFuelAddendumsFiltered")
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Logger.Error(ex, "GetCarrierFuelAddendumsFiltered")
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing
                End Using

        End Using
    End Function

    Public Function GetCarrierFuelAddendumsFiltered(Optional ByVal CarrierControl As Integer = 0) As DataTransferObjects.CarrierFuelAddendum()
        Using Logger.StartActivity("GetCarrierFuelAddendumsFiltered(CarrierControl: {CarrierControl})", CarrierControl)
            Using db As New NGLMASCarrierDataContext(ConnectionString)
                Try
                    'Return all the contacts that match the criteria sorted by name
                    Dim CarrierFuelAddendums() As DataTransferObjects.CarrierFuelAddendum = (
                            From t In db.CarrierFuelAddendums
                            Where
                            (t.CarrFuelAdCarrierControl = If(CarrierControl = 0, t.CarrFuelAdCarrierControl, CarrierControl)) _
                            And (t.CarrFuelAdCarrTarControl = 0) And (t.CarrFuelAdCarrTarEquipControl = 0)
                            Order By t.CarrFuelAdControl
                            Select New DataTransferObjects.CarrierFuelAddendum With {.CarrFuelAdControl = t.CarrFuelAdControl _
                            , .CarrFuelAdCarrierControl = t.CarrFuelAdCarrierControl _
                            , .CarrFuelAdCarrTarControl = t.CarrFuelAdCarrTarControl _
                            , .CarrFuelAdCarrTarEquipControl = t.CarrFuelAdCarrTarEquipControl _
                            , .CarrFuelAdUseNatAvg = t.CarrFuelAdUseNatAvg _
                            , .CarrFuelAdUseZoneAvg = t.CarrFuelAdUseZoneAvg _
                            , .CarrFuelAdUseRatePerMile = t.CarrFuelAdUseRatePerMile _
                            , .CarrFuelAdDefFuelRate = t.CarrFuelAdDefFuelRate _
                            , .CarrFuelAdModUser = t.CarrFuelAdModUser _
                            , .CarrFuelAdModDate = t.CarrFuelAdModDate _
                            , .CarrFuelAdUpdated = t.CarrFuelAdUpdated.ToArray()}).ToArray()


                    Return CarrierFuelAddendums

                Catch ex As System.Data.SqlClient.SqlException
                    Logger.Error(ex, "GetCarrierFuelAddendumsFiltered")
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Logger.Error(ex, "GetCarrierFuelAddendumsFiltered")
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Logger.Error(ex, "GetCarrierFuelAddendumsFiltered")
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing
                End Using

        End Using
    End Function

    Public Function GetCarrierFuelAddendumsCarTarFiltered(ByVal CarrTarControl As Integer) As DataTransferObjects.CarrierFuelAddendum
        Using Logger.StartActivity("GetCarrierFuelAddendumsCarTarFiltered(CarrTarControl: {CarrTarControl})", CarrTarControl)
            Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CarrierFuelAddendums As DataTransferObjects.CarrierFuelAddendum = (
                        From t In db.CarrierFuelAddendums
                        Where
                        (t.CarrFuelAdCarrTarControl = CarrTarControl)
                        Order By t.CarrFuelAdControl
                        Select New DataTransferObjects.CarrierFuelAddendum With {.CarrFuelAdControl = t.CarrFuelAdControl _
                        , .CarrFuelAdCarrierControl = t.CarrFuelAdCarrierControl _
                        , .CarrFuelAdCarrTarControl = t.CarrFuelAdCarrTarControl _
                        , .CarrFuelAdCarrTarEquipControl = t.CarrFuelAdCarrTarEquipControl _
                        , .CarrFuelAdUseNatAvg = t.CarrFuelAdUseNatAvg _
                        , .CarrFuelAdUseZoneAvg = t.CarrFuelAdUseZoneAvg _
                        , .CarrFuelAdUseRatePerMile = t.CarrFuelAdUseRatePerMile _
                        , .CarrFuelAdDefFuelRate = t.CarrFuelAdDefFuelRate _
                        , .CarrFuelAdModUser = t.CarrFuelAdModUser _
                        , .CarrFuelAdModDate = t.CarrFuelAdModDate _
                        , .CarrFuelAdUpdated = t.CarrFuelAdUpdated.ToArray()}).FirstOrDefault()

                Return CarrierFuelAddendums

            Catch ex As System.Data.SqlClient.SqlException
                Logger.Error(ex, "GetCarrierFuelAddendumsCarTarFiltere")
                '         Utilities.SaveAppError(ex.Message, Me.Parameters)
                '        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Logger.Error(ex, "GetCarrierFuelAddendumsCarTarFiltere")
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                'no need to throw e_nodata error, its ok.
                'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Logger.Error(ex, "GetCarrierFuelAddendumsCarTarFiltere")
                'Utilities.SaveAppError(ex.Message, Me.Parameters)
                'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
                End Using

            Return Nothing

        End Using
    End Function



    Public Function GetCarrTarFuelAddendum(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByVal treeid As Integer) As DataTransferObjects.NGLTreeNode
        Using operation = Logger.StartActivity("GetCarrTarFuelAddendum(CarrTarControl: {CarrTarControl}, ParentTreeID: {ParentTreeID}, TreeID: {TreeID})", CarrTarControl, ParentTreeID, treeid)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim CarrierFuelAddendums As DataTransferObjects.CarrierFuelAddendum = (
                        From t In db.CarrierFuelAddendums
                        Where
                        (t.CarrFuelAdCarrTarControl = CarrTarControl)
                        Order By t.CarrFuelAdControl
                        Select New DataTransferObjects.CarrierFuelAddendum With {.CarrFuelAdControl = t.CarrFuelAdControl _
                        , .CarrFuelAdCarrierControl = t.CarrFuelAdCarrierControl _
                        , .CarrFuelAdCarrTarControl = t.CarrFuelAdCarrTarControl _
                        , .CarrFuelAdCarrTarEquipControl = t.CarrFuelAdCarrTarEquipControl _
                        , .CarrFuelAdUseNatAvg = t.CarrFuelAdUseNatAvg _
                        , .CarrFuelAdUseZoneAvg = t.CarrFuelAdUseZoneAvg _
                        , .CarrFuelAdUseRatePerMile = t.CarrFuelAdUseRatePerMile _
                        , .CarrFuelAdDefFuelRate = t.CarrFuelAdDefFuelRate _
                        , .CarrFuelAdModUser = t.CarrFuelAdModUser _
                        , .CarrFuelAdModDate = t.CarrFuelAdModDate _
                        , .CarrFuelAdUpdated = t.CarrFuelAdUpdated.ToArray()}).FirstOrDefault()

                Dim CarrierTariffFuelNode As New DataTransferObjects.NGLTreeNode
                CarrierTariffFuelNode.ParentTreeID = ParentTreeID
                CarrierTariffFuelNode.TreeID = treeid
                CarrierTariffFuelNode.Name = "Fuel"
                CarrierTariffFuelNode.Description = "Fuel Settings"
                CarrierTariffFuelNode.ClassName = "CarrierFuelAddendum"

                If CarrierFuelAddendums Is Nothing Then
                    CarrierTariffFuelNode.Control = 0
                Else
                    CarrierTariffFuelNode.Control = CarrierFuelAddendums.CarrFuelAdControl
                End If

                Return CarrierTariffFuelNode

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                Return Nothing
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrTarEquipNodes"))
            End Try
                End Using

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Used to get the updated fuel surcharge and Use Rate Per Mile values for the provided carrier,tariff or equipment
    ''' This method should not be called directly by WCF because it uses an LTS return data object.
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CarrTarControl"></param>
    ''' <param name="CarrTarEquipControl"></param>
    ''' <param name="STATE"></param>
    ''' <param name="EffectiveDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFuelSurCharge(ByVal CarrierControl As Integer,
                                     ByVal CarrTarControl As Integer,
                                     ByVal CarrTarEquipControl As Integer,
                                     ByVal STATE As String,
                                     ByVal EffectiveDate As Date) As LTS.spGetFuelSurchargeResult

        Dim result As LTS.spGetFuelSurchargeResult

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Using Logger.StartActivity("running spGetFuelSurcharge with CarrierControl: {CarrierControl}, CarTarControl:{CarTarControl},CarrTarEquipControl:{CarrTarEquipControl}, State:{State}, EffectiveDate:{EffectiveDate}", CarrierControl, CarrTarControl, CarrTarEquipControl, STATE, EffectiveDate)
                    Dim resultList = (From d In db.spGetFuelSurcharge(CarrierControl, CarrTarControl, CarrTarEquipControl, STATE, EffectiveDate) Select d).ToList()

                    result = resultList.FirstOrDefault()
                    Logger.Information("spGetFuelSurcharge returned {@resultList} records, returning First ({@result})", resultList, result)
                End Using
                Return result
            Catch ex As Exception
                Logger.Error(ex, "Error in GetFuelSurCharge")
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Used to get the updated fuel surcharge and Use Rate Per Mile values for the last stop on a load.
    ''' This method should not be called directly by WCF because it uses an LTS return data object.
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="EffectiveDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFuelSurChargeForBook(ByVal BookControl As Integer,
                                            ByVal EffectiveDate As Date) As LTS.spGetFuelSurchargeForBookResult

        Dim result As LTS.spGetFuelSurchargeForBookResult

        Using Logger.StartActivity("GetFuelSurChargeForBook(BookControl: {BookControl}, EffectiveDate: {EffectiveDate}", BookControl, EffectiveDate)
            Using db As New NGLMASCarrierDataContext(ConnectionString)
                Try
                    Logger.Information("running spGetFuelSurchargeForBook with BookControl: {BookControl}, EffectiveDate: {EffectiveDate}", BookControl, EffectiveDate)
                    result = (From d In db.spGetFuelSurchargeForBook(BookControl, EffectiveDate) Select d).FirstOrDefault()
                    Logger.Information("spGetFuelSurchargeForBook returned {@result}", result)
                    Return result
                Catch ex As Exception
                    Logger.Error(ex, "Error in GetFuelSurChargeForBook")
                    '  ManageLinqDataExceptions(ex, buildProcedureName("GetFuelSurChargeForBook"))
                End Try
            End Using
        End Using

        Return Nothing
    End Function


    ' '' This will copy an existing fuel addendum and create a new carrier/tariff or equipment {on hold for now} fuel addendum using one of the following options:

    ' ''If the UseGlobalStdFuelAddendumCarrier flag is true we always try to look up the global fuel addendum template using the GlobalStdFuelAddendumCarrier parameter.

    ' ''If the UseGlobalStdFuelAddendumCarrier is false or the GlobalStdFuelAddendumCarrier does not exist we use SourceCarrFuelAdControl to look up the template.

    ' ''This process will delete any fuel addendums associated with the Carrier, Tariff or Equipment values passed as parameters.  It will then create a new fuel addendum using the tariff.

    ' ''To create a new fuel addendum at the carrier level provide just the NewCarrierControl number.  This logic must be changed in FreightMaster to use the new WCF method above.

    ' ''To create a new fuel addendum at the tariff level provide both the carrier control as NewCarrierControl and the tariff control as NewCarrTarControl.

    ' ''We do not currently support Equipment level fuel addendums so always pass zero as the value to NewCarrTarEquipControl.
    ''' 
    ''' <summary>
    ''' Creates a new Fuel Addendum for the provided New Carrier, Tariff or Equip record using
    ''' the default Global template or the Source Addendum provided
    ''' </summary>
    ''' <param name="NewCarrierControl">Required</param>
    ''' <param name="NewCarrTarControl">Optional (set to zero) but Required if Equipment is provided</param>
    ''' <param name="NewCarrTarEquipControl">Optional (set to zero)</param>
    ''' <param name="UseGlobalStdFuelAddendumCarrier">False if SouceCarrFuelAdControl should be used</param>
    ''' <param name="SourceCarrFuelAdControl">Optional (set to zero)</param>
    ''' <param name="UserName"></param>
    ''' <returns>
    ''' returns an LTS.spCreateNewFuelAddendumFromTemplateResult table with the created CarrFuelAdControl and any errors.
    ''' </returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 2/12/2018
    ''' this method will now look up a value for SourceCarrFuelAdControl when 
    ''' 1. NewCarrierControl not equal 0
    ''' 2. UseGlobalStdFuelAddendumCarrier is false
    ''' 3. SourceCarrFuelAdControl is zero
    ''' </remarks>
    Public Function CreateNewFuelAddendumFromTemplate(ByVal NewCarrierControl As Integer,      'modified by suhas 26/08/20
                                                      ByVal NewCarrTarControl As Integer,
                                                      ByVal NewCarrTarEquipControl As Integer,
                                                      ByVal UseGlobalStdFuelAddendumCarrier As Boolean,
                                                      ByVal SourceCarrFuelAdControl As Integer,
                                                      ByVal UserName As String) As Integer
        Dim intCarrFuelAdControl As Integer = 0
        Dim oResults As LTS.spCreateNewFuelAddendumFromTemplateResult
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Modified by RHR for v-8.2 on 2/12/2018
                If UseGlobalStdFuelAddendumCarrier = False And NewCarrierControl <> 0 And SourceCarrFuelAdControl = 0 Then
                    'look up the fuel addendum using the carriercontrol
                    Dim oFAs = GetCarrierFuelAddendumsFiltered(NewCarrierControl)
                    If Not oFAs Is Nothing AndAlso oFAs.Count() > 0 Then
                        SourceCarrFuelAdControl = oFAs(0).CarrFuelAdControl
                    End If
                End If

                oResults = (From d In db.spCreateNewFuelAddendumFromTemplate(NewCarrierControl, NewCarrTarControl, 0, UseGlobalStdFuelAddendumCarrier, SourceCarrFuelAdControl, Left(UserName, 100)) Select d).FirstOrDefault() 'NewCarrTarEquipControl
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CreateNewFuelAddendumFromTemplate"))
            End Try
        End Using
        If Not oResults Is Nothing Then
            intCarrFuelAdControl = oResults.CarrFuelAdControl
            If oResults.ErrNumber <> 0 Then
                throwSQLFaultException(oResults.RetMsg)
            End If
        End If
        Return intCarrFuelAdControl
    End Function

    Public Function selectDTO(ByVal fuel As LTS.spGetFuelSurchargeResult) As DataTransferObjects.FuelSurchargeResult
        Dim result As New DataTransferObjects.FuelSurchargeResult
        If fuel IsNot Nothing Then
            result.AvgFuelPrice = fuel.AvgFuelPrice
            result.CarrierControl = fuel.CarrierControl
            result.CarrTarControl = fuel.CarrTarControl
            result.CarrTarEquipControl = fuel.CarrTarEquipControl
            result.EffectiveDate = fuel.EffectiveDate
            result.FuelSurcharge = fuel.FuelSurcharge
            result.STATE = fuel.STATE
            result.UseRatePerMile = fuel.UseRatePerMile
            Logger.Information("Fuel Found, AvgFuelPrice: {0}, CarrierControl: {1}, CarrTarControl: {2}, CarrTarEquipControl: {3}, EffectiveDate: {4}, FuelSurcharge: {5}, STATE: {6}, UseRatePerMile: {7}", result.AvgFuelPrice, result.CarrierControl, result.CarrTarControl, result.CarrTarEquipControl, result.EffectiveDate, result.FuelSurcharge, result.STATE, result.UseRatePerMile)
        End If
        Return result
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrierFuelAddendum)
        'Create New Record
        Return New LTS.CarrierFuelAddendum With {.CarrFuelAdControl = d.CarrFuelAdControl _
            , .CarrFuelAdCarrierControl = d.CarrFuelAdCarrierControl _
            , .CarrFuelAdCarrTarControl = d.CarrFuelAdCarrTarControl _
            , .CarrFuelAdCarrTarEquipControl = d.CarrFuelAdCarrTarEquipControl _
            , .CarrFuelAdUseNatAvg = d.CarrFuelAdUseNatAvg _
            , .CarrFuelAdUseZoneAvg = d.CarrFuelAdUseZoneAvg _
            , .CarrFuelAdUseRatePerMile = d.CarrFuelAdUseRatePerMile _
            , .CarrFuelAdDefFuelRate = d.CarrFuelAdDefFuelRate _
            , .CarrFuelAdModUser = d.CarrFuelAdModUser _
            , .CarrFuelAdModDate = d.CarrFuelAdModDate _
            , .CarrFuelAdUpdated = If(d.CarrFuelAdUpdated Is Nothing, New Byte() {}, d.CarrFuelAdUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrierFuelAddendumFiltered(Control:=CType(LinqTable, LTS.CarrierFuelAddendum).CarrFuelAdControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierFuelAddendum = TryCast(LinqTable, LTS.CarrierFuelAddendum)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierFuelAddendums
                       Where d.CarrFuelAdControl = source.CarrFuelAdControl
                       Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrFuelAdControl _
                           , .ModDate = d.CarrFuelAdModDate _
                           , .ModUser = d.CarrFuelAdModUser _
                           , .Updated = d.CarrFuelAdUpdated.ToArray}).First

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

    Protected Overrides Sub AddDetailsToLinq(ByRef LinqTable As Object, ByRef oData As DataTransferObjects.DTOBaseClass)

        With CType(LinqTable, LTS.CarrierFuelAddendum)
            'Add CarrierFuelAddendum Ex Records
            .CarrierFuelAdExes.AddRange(
                From d In CType(oData, DataTransferObjects.CarrierFuelAddendum).CarrierFuelAdExes
                Select New LTS.CarrierFuelAdEx With {.CarrFuelAdExControl = d.CarrFuelAdExControl _
                , .CarrFuelAdExCarrFuelAdContol = d.CarrFuelAdExCarrFuelAdContol _
                , .CarrFuelAdExState = d.CarrFuelAdExState _
                , .CarrFuelAdExRatePerMile = d.CarrFuelAdExRatePerMile _
                , .CarrFuelAdExPercent = d.CarrFuelAdExPercent _
                , .CarrFuelAdExEffDate = d.CarrFuelAdExEffDate _
                , .CarrFuelAdExModUser = d.CarrFuelAdExModUser _
                , .CarrFuelAdExModDate = d.CarrFuelAdExModDate _
                , .CarrFuelAdExUpdated = If(d.CarrFuelAdExUpdated Is Nothing, New Byte() {}, d.CarrFuelAdExUpdated)})
            'Add CarrierFuelAddendum Rates Records
            .CarrierFuelAdRates.AddRange(
                From d In CType(oData, DataTransferObjects.CarrierFuelAddendum).CarrierFuelAdRates
                Select New LTS.CarrierFuelAdRate With {.CarrFuelAdRatesControl = d.CarrFuelAdRatesControl _
                , .CarrFuelAdRatesCarrFuelAdControl = d.CarrFuelAdRatesCarrFuelAdControl _
                , .CarrFuelAdRatesPriceFrom = d.CarrFuelAdRatesPriceFrom _
                , .CarrFuelAdRatesPriceTo = d.CarrFuelAdRatesPriceTo _
                , .CarrFuelAdRatesPerMile = d.CarrFuelAdRatesPerMile _
                , .CarrFuelAdRatesPercent = d.CarrFuelAdRatesPercent _
                , .CarrFuelAdRatesEffDate = d.CarrFuelAdRatesEffDate _
                , .CarrFuelAdRatesModUser = d.CarrFuelAdRatesModUser _
                , .CarrFuelAdRatesModDate = d.CarrFuelAdRatesModDate _
                , .CarrFuelAdRatesUpdated = If(d.CarrFuelAdRatesUpdated Is Nothing, New Byte() {}, d.CarrFuelAdRatesUpdated)})

        End With
    End Sub

    Protected Overrides Sub InsertAllDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef LinqTable As Object)
        With CType(oDB, NGLMASCarrierDataContext)
            .CarrierFuelAdExes.InsertAllOnSubmit(CType(LinqTable, LTS.CarrierFuelAddendum).CarrierFuelAdExes)
            .CarrierFuelAdRates.InsertAllOnSubmit(CType(LinqTable, LTS.CarrierFuelAddendum).CarrierFuelAdRates)
        End With
    End Sub

    Protected Overrides Sub ProcessUpdatedDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        With CType(oDB, NGLMASCarrierDataContext)
            ' Process any inserted Ex records 
            .CarrierFuelAdExes.InsertAllOnSubmit(GetCarrierFuelAdExesChanges(oData, TrackingInfo.Created))
            ' Process any updated Ex records
            .CarrierFuelAdExes.AttachAll(GetCarrierFuelAdExesChanges(oData, TrackingInfo.Updated), True)
            ' Process any deleted Ex records
            Dim deletedCarrierFuelAdExes = GetCarrierFuelAdExesChanges(oData, TrackingInfo.Deleted)
            .CarrierFuelAdExes.AttachAll(deletedCarrierFuelAdExes, True)
            .CarrierFuelAdExes.DeleteAllOnSubmit(deletedCarrierFuelAdExes)
            ' Process any inserted contact records 
            .CarrierFuelAdRates.InsertAllOnSubmit(GetCarrierFuelAdRatesChanges(oData, TrackingInfo.Created))
            ' Process any updated contact records
            .CarrierFuelAdRates.AttachAll(GetCarrierFuelAdRatesChanges(oData, TrackingInfo.Updated), True)
            ' Process any deleted contact records
            Dim deletedCarrierFuelAdRates = GetCarrierFuelAdRatesChanges(oData, TrackingInfo.Deleted)
            .CarrierFuelAdRates.AttachAll(deletedCarrierFuelAdRates, True)
            .CarrierFuelAdRates.DeleteAllOnSubmit(deletedCarrierFuelAdRates)

        End With
    End Sub

    Protected Function GetCarrierFuelAdExesChanges(ByVal source As DataTransferObjects.CarrierFuelAddendum, ByVal changeType As TrackingInfo) As List(Of LTS.CarrierFuelAdEx)

        Dim details As IEnumerable(Of LTS.CarrierFuelAdEx) = (
                From d In source.CarrierFuelAdExes
                Where d.TrackingState = changeType
                Select NGLCarrierFuelAdExData.selectLTSData(d))
        Return details.ToList()
    End Function

    Protected Function GetCarrierFuelAdRatesChanges(ByVal source As DataTransferObjects.CarrierFuelAddendum, ByVal changeType As TrackingInfo) As List(Of LTS.CarrierFuelAdRate)

        Dim details As IEnumerable(Of LTS.CarrierFuelAdRate) = (
                From d In source.CarrierFuelAdRates
                Where d.TrackingState = changeType
                Select New LTS.CarrierFuelAdRate With {.CarrFuelAdRatesControl = d.CarrFuelAdRatesControl _
                , .CarrFuelAdRatesCarrFuelAdControl = d.CarrFuelAdRatesCarrFuelAdControl _
                , .CarrFuelAdRatesPriceFrom = d.CarrFuelAdRatesPriceFrom _
                , .CarrFuelAdRatesPriceTo = d.CarrFuelAdRatesPriceTo _
                , .CarrFuelAdRatesPerMile = d.CarrFuelAdRatesPerMile _
                , .CarrFuelAdRatesPercent = d.CarrFuelAdRatesPercent _
                , .CarrFuelAdRatesEffDate = d.CarrFuelAdRatesEffDate _
                , .CarrFuelAdRatesModUser = d.CarrFuelAdRatesModUser _
                , .CarrFuelAdRatesModDate = d.CarrFuelAdRatesModDate _
                , .CarrFuelAdRatesUpdated = If(d.CarrFuelAdRatesUpdated Is Nothing, New Byte() {}, d.CarrFuelAdRatesUpdated)})
        Return details.ToList()
    End Function

#End Region


#Region "LTS carrier tariff fuel addendum data"


    ''' <summary>
    ''' Returns the carrier fuel Addendum data assoicated with a Carrier Tariff.
    ''' A CarrFuelAdControl filter  or the  CarrFuelAdCarrTarControl value must be provided in the filters.ParentControl
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/25/2019
    '''  providing a CarrFuelAdControl will return one record
    '''  The CarrFuelAdCarrierControl is ignored in this method and if the CarrFuelAdControl is zero
    '''  the CarrFuelAdCarrTarControl value must be provided in the ParentControl
    ''' </remarks>
    Public Function GetCarrierTariffFuelAddendum(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.CarrierFuelAddendum()
        If filters Is Nothing Then Return Nothing

        '   [CarrFuelAdControl]        Int            IDENTITY (1, 1) Not NULL,
        '   [CarrFuelAdCarrierControl] Int            Not NULL,
        '   [CarrFuelAdCarrTarControl] Int CONSTRAINT [DF_CarrFuelAdCarrTarControl] DEFAULT (0) Not NULL,
        '   [CarrFuelAdCarrTarEquipControl] Int CONSTRAINT [DF_CarrFuelAdCarrTarEquipControl] DEFAULT (0) Not NULL,
        '   [CarrFuelAdUseNatAvg]      BIT            Not NULL,
        '   [CarrFuelAdUseZoneAvg]     BIT            Not NULL,
        '   [CarrFuelAdUseRatePerMile] BIT            Not NULL,
        '   [CarrFuelAdDefFuelRate]    Decimal(10, 5) Not NULL,
        '   [CarrFuelAdFuelAdUpdateTypeControl]  Int CONSTRAINT [DF_CarrFuelAdFuelAdUpdateTypeControl] DEFAULT (1) Not NULL,
        '   [CarrFuelAdModUser]        NVARCHAR(100) NULL,
        '   [CarrFuelAdModDate]        DateTime       NULL,
        '   [CarrFuelAdUpdated]        ROWVERSION     NULL,


        Dim iCarrFuelAdControl As Integer = 0
        Dim iCarrFuelAdCarrTarControl As Integer = 0
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        If Not filters.addParentFilterIfNoRecordControlFilter("CarrFuelAdControl", "CarrFuelAdCarrTarControl", iCarrFuelAdControl, iCarrFuelAdCarrTarControl, filterWhere, sFilterSpacer) Then
            throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
        End If
        If iCarrFuelAdCarrTarControl = 0 And iCarrFuelAdControl = 0 Then
            'we do not have a valid filter
            throwNoDataFaultMessage("E_InvalidRecordKey") ' "The reference to the record is missing. Please select another record and try again."
        End If

        Dim oRet() As LTS.CarrierFuelAddendum

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.CarrierFuelAddendum)
                iQuery = db.CarrierFuelAddendums
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                'db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierTariffFuelAddendum"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Deprecated.  Fuel is now linked to Legal Entity so call GetCarrierFuelAddendumByLECarrierControl
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Method replaced by GetCarrierFuelAddendumByLECarrierControl on 9/19/2023 by RHR for v-8.5.4.002 
    ''' </remarks>
    Public Function GetCarrierFuelAddendum(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.CarrierFuelAddendum() 'added by suhas 26/08/20
        If filters Is Nothing Then Return Nothing
        Dim iCarrFuelAdControl As Integer = 0
        Dim iLECarControl As Integer = 0
        Dim iCarrierControl As Integer = 0
        'Dim iCarrFuelAdCarrTarControl As Integer = 0 CarrFuelAdCarrierControl
        Dim iCarrFuelAdCarrierControl As Integer = 0
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        'Note: the addParentFilterIfNoRecordControlFilter does not work for this method because we are passing in the 
        '       iLECarControl as parent.  This code was left for backward compatibility
        If Not filters.addParentFilterIfNoRecordControlFilter("CarrFuelAdControl", "CarrFuelAdCarrierControl", iCarrFuelAdControl, iCarrFuelAdCarrierControl, filterWhere, sFilterSpacer) Then
            throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
        End If
        If iCarrFuelAdCarrierControl = 0 And iCarrFuelAdControl = 0 Then 'iCarrFuelAdCarrTarControl = 0 And
            'we do not have a valid filter
            throwNoDataFaultMessage("E_InvalidRecordKey") ' "The reference to the record is missing. Please select another record and try again."
        End If

        Dim oRet() As LTS.CarrierFuelAddendum

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                ' need to lookup carrier control number from tblLegalEntityCarriers
                iLECarControl = filters.ParentControl
                iCarrierControl = db.tblLegalEntityCarriers.Where(Function(x) x.LECarControl = iLECarControl).Select(Function(x) x.LECarCarrierControl).FirstOrDefault()

                If iCarrierControl = 0 Then
                    throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
                End If

                Dim iQuery As IQueryable(Of LTS.CarrierFuelAddendum)
                iQuery = db.CarrierFuelAddendums
                filterWhere = " (CarrFuelAdCarrierControl = " & iCarrierControl & ") "
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierFuelAddendum"), db)
            End Try
        End Using
        Return oRet
    End Function


    ''' <summary>
    ''' Gets Carrier Fuel Addendum for Legal Entity Carrier.  
    ''' Looks up CarrierControl using iLECarControl value
    ''' or optionally using the iCarrFuelAdControl if available
    ''' </summary>
    ''' <param name="iLECarControl"></param>
    ''' <param name="iCarrFuelAdControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Method replaces  GetCarrierFuelAddendum on 9/19/2023 by RHR for v-8.5.4.002 
    ''' </remarks>
    Public Function GetCarrierFuelAddendumByLECarrierControl(ByVal iLECarControl As Integer, Optional ByVal iCarrFuelAdControl As Integer = 0) As LTS.CarrierFuelAddendum()
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""
        Dim oRet() As LTS.CarrierFuelAddendum

        Using operation = Logger.StartActivity("GetCarrierFuelAddendumByLECarrierControl(iLECarControl: {iLECarControl}, iCarrFuelAdControl: {iCarrFuelAdControl})", iLECarControl, iCarrFuelAdControl)
            Using db As New NGLMASCarrierDataContext(ConnectionString)
                Try
                    'check for the iCarrFuelAdControl
                    If iCarrFuelAdControl <> 0 Then
                        oRet = db.CarrierFuelAddendums.Where(Function(x) x.CarrFuelAdControl = iCarrFuelAdControl).ToArray()
                    End If
                    If oRet Is Nothing OrElse oRet.Count < 1 Then
                        ' need to lookup carrier control number from tblLegalEntityCarriers
                        Dim iCarrierControl As Integer = db.tblLegalEntityCarriers.Where(Function(x) x.LECarControl = iLECarControl).Select(Function(x) x.LECarCarrierControl).FirstOrDefault()
                        If iCarrierControl = 0 Then

                            throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
                        End If
                        oRet = db.CarrierFuelAddendums.Where(Function(x) x.CarrFuelAdCarrierControl = iCarrierControl).ToArray()
                    End If

                Catch ex As Exception
                    Logger.Error(ex, "Error in GetCarrierFuelAddendumByLECarrierControl")
                    operation.Complete(Serilog.Events.LogEventLevel.Error, ex)
                    'ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierFuelAddendum"), db)
                End Try
            End Using
        End Using

        Return oRet
    End Function


    ''' <summary>
    '''  Insert or Update the carrier tariff fuel addendum data  assoicated with a Carrier tariff.
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/25/2019
    ''' this method requires a CarrFuelAdCarrTarControl in oData 
    ''' if the CarrFuelAdCarrierControl is not provided the system will look this up 
    ''' using the CarrFuelAdCarrTarControl
    ''' </remarks>
    Public Function SaveCarrierTariffFuelAddendum(ByVal oData As LTS.CarrierFuelAddendum) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Dim iCarrFuelAdControl As Integer = oData.CarrFuelAdControl
        Dim iCarrFuelAdCarrTarControl As Integer = oData.CarrFuelAdCarrTarControl
        Dim iCarrFuelAdCarrierControl As Integer = oData.CarrFuelAdCarrierControl
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'verify the Tariff exists
                If iCarrFuelAdCarrTarControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Tariff Contract Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If Not db.CarrierTariffs.Any(Function(x) x.CarrTarControl = iCarrFuelAdCarrTarControl) Then
                    Dim lDetails As New List(Of String) From {"Carrier Tariff Contract", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If

                'get the carrier control if it is missing or invalid
                If iCarrFuelAdCarrierControl = 0 OrElse Not db.Carriers.Any(Function(x) x.CarrierControl = iCarrFuelAdCarrierControl) Then
                    iCarrFuelAdCarrierControl = db.CarrierTariffs.Where(Function(x) x.CarrTarControl = iCarrFuelAdCarrTarControl).Select(Function(x) x.CarrTarCarrierControl).FirstOrDefault()
                    oData.CarrFuelAdCarrierControl = iCarrFuelAdCarrierControl
                End If

                With oData
                    .CarrFuelAdModDate = Date.Now
                    .CarrFuelAdModUser = Me.Parameters.UserName
                End With
                If iCarrFuelAdControl = 0 Then
                    oData.CarrFuelAdUpdated = New Byte() {}
                    db.CarrierFuelAddendums.InsertOnSubmit(oData)
                Else
                    db.CarrierFuelAddendums.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCarrierTariffFuelAddendum"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function SaveCarrierFuelAddendum(ByVal oData As LTS.CarrierFuelAddendum, Optional ByVal iLECarControl As Integer = 0) As Boolean 'added by suhas 26/08/20
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Dim iCarrFuelAdControl As Integer = oData.CarrFuelAdControl
        Dim iCarrFuelAdCarrTarControl As Integer = oData.CarrFuelAdCarrTarControl
        Dim iCarrFuelAdCarrierControl As Integer = oData.CarrFuelAdCarrierControl
        Dim blnDoInsert As Boolean = False
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try


                'verify the carrier exists
                If iCarrFuelAdCarrierControl = 0 Then
                    If iLECarControl > 0 Then
                        'lookup using LECarControl
                        iCarrFuelAdCarrierControl = db.tblLegalEntityCarriers.Where(Function(x) x.LECarControl = iLECarControl).Select(Function(x) x.LECarCarrierControl).FirstOrDefault()
                    End If
                    If iCarrFuelAdCarrierControl = 0 Then
                        Dim lDetails As New List(Of String) From {"Carrier Reference", " was not valid and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    'assign the carrier to the addendum
                    oData.CarrFuelAdCarrierControl = iCarrFuelAdCarrierControl
                End If

                'check if this is an insert for carrier master data
                'rules:
                ' 1. Carrier must exist
                If Not db.Carriers.Any(Function(x) x.CarrierControl = iCarrFuelAdCarrierControl) Then
                    Dim lDetails As New List(Of String) From {"Carrier information", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                '2. if CarrFuelAdCarrTarControl not 0 validate tariff must exists
                If iCarrFuelAdCarrTarControl > 0 And Not db.CarrierTariffs.Any(Function(x) x.CarrTarControl = iCarrFuelAdCarrTarControl) Then
                    Dim lDetails As New List(Of String) From {"Carrier tariff contract", " was not found and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If

                '3. if CarrFuelAdControl = 0 we must insert
                If iCarrFuelAdControl = 0 Then blnDoInsert = True


                If blnDoInsert Then
                    '4. If blnDoInsert only one allowed per carrier and tariff and iCarrFuelAdControl
                    If db.CarrierFuelAddendums.Any(Function(x) x.CarrFuelAdCarrierControl = iCarrFuelAdCarrierControl AndAlso x.CarrFuelAdCarrTarControl = iCarrFuelAdCarrTarControl) Then
                        Dim lDetails As New List(Of String) From {"Carrier Fuel Addendum", " already exists check carrier and tariff contract and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If

                Else

                    '4. If not blnDoInsert only one allowed per carrier and tariff for 
                    If db.CarrierFuelAddendums.Any(Function(x) x.CarrFuelAdCarrierControl = iCarrFuelAdCarrierControl AndAlso x.CarrFuelAdCarrTarControl = iCarrFuelAdCarrTarControl AndAlso x.CarrFuelAdControl <> iCarrFuelAdControl) Then
                        Dim lDetails As New List(Of String) From {"Carrier Fuel Addendum", " already exists check carrier and tariff contract and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If

                End If

                With oData
                    .CarrFuelAdModDate = Date.Now
                    .CarrFuelAdModUser = Me.Parameters.UserName
                End With
                If blnDoInsert Then
                    oData.CarrFuelAdUpdated = New Byte() {}
                    db.CarrierFuelAddendums.InsertOnSubmit(oData)
                Else
                    db.CarrierFuelAddendums.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveCarrierFuelAddendum"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete the provided carrier Fuel Addenum
    ''' </summary>
    ''' <param name="iControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 01/25/2019
    ''' </remarks>
    Public Function DeleteCarrierFuelAddendum(ByVal iControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iControl = 0 Then Return False 'nothing to do
        'validate before delete

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Delete the Record
                Dim oToDelete = db.CarrierFuelAddendums.Where(Function(x) x.CarrFuelAdControl = iControl).FirstOrDefault()
                If oToDelete Is Nothing OrElse oToDelete.CarrFuelAdControl = 0 Then Return True 'already deleted
                db.CarrierFuelAddendums.DeleteOnSubmit(oToDelete)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrierFuelAddendum"), db)
            End Try
        End Using
        Return blnRet
    End Function


#End Region


End Class