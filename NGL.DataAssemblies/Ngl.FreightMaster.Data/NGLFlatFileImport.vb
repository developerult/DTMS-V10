Imports System.ServiceModel

Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports Ngl.FreightMaster.Data.Utilities
Imports System.Linq.Dynamic

Public Class NGLFlatFileImport : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.Parameters = oParameters
        Dim db As New NGLMASIntegrationDataContext(ConnectionString)
        Me.LinqTable = Nothing
        Me.LinqDB = db
        Me.SourceClass = "NGLFlatFileImport"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASIntegrationDataContext(ConnectionString)
            _LinqTable = Nothing
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property


    Protected mblnInsertRecord As Boolean = True

    Protected mstrHeaderName As String = ""
    Public Property HeaderName() As String
        Get
            Return mstrHeaderName
        End Get
        Set(ByVal Value As String)
            mstrHeaderName = Value
        End Set
    End Property

    Protected mstrItemName As String = ""
    Public Property ItemName() As String
        Get
            Return mstrItemName
        End Get
        Set(ByVal Value As String)
            mstrItemName = Value
        End Set
    End Property

    Protected mstrCalendarName As String = ""
    Public Property CalendarName() As String
        Get
            Return mstrCalendarName
        End Get
        Set(ByVal Value As String)
            mstrCalendarName = Value
        End Set
    End Property

    Protected mintImportTypeKey As Integer = 0
    Public Property ImportTypeKey() As IntegrationTypes
        Get
            Return mintImportTypeKey
        End Get
        Set(ByVal Value As IntegrationTypes)
            mintImportTypeKey = Value
        End Set
    End Property

    Protected mstrCreatedDate As String = Date.Now.ToString
    Public Property CreatedDate() As String
        Get
            Return mstrCreatedDate
        End Get
        Set(ByVal Value As String)
            mstrCreatedDate = Value
        End Set
    End Property

    Protected mstrCreateUser As String = "system download"
    Public Property CreateUser() As String
        Get
            Return mstrCreateUser
        End Get
        Set(ByVal Value As String)
            mstrCreateUser = Value
        End Set
    End Property

    Protected mstrSource As String = ""
    Public Property Source() As String
        Get
            Dim strRet As String = mstrSource
            Return strRet
        End Get
        Set(ByVal Value As String)
            mstrSource = Value
        End Set
    End Property


#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function ProcessFreightBill(ByVal record As String, Optional ByVal CarrierNumber As Integer = 0) As Models.ResultObject

        Dim oResults As New Models.ResultObject() With {.Success = False, .SuccessMsg = "Failed!"}
        Dim oBatchProcessing As New NGLBatchProcessDataProvider(Me.Parameters)
        Dim strData() As String
        Try

            strData = DTran.DecodeCSV(record)
        Catch ex As ApplicationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_DataValidationFailure"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
        If strData Is Nothing OrElse strData.Length < 1 Then Return oResults
        Dim oFreightBill As New DTO.FreightBill
        Try
            With oFreightBill
                If strData.Length > 0 Then .APPONumber = DTran.NZ(strData(0), "")
                If strData.Length > 1 Then .APPRONumber = DTran.NZ(strData(1), "")
                If strData.Length > 2 Then .APCNSNumber = DTran.NZ(strData(2), "")
                If CarrierNumber <> 0 Then
                    If strData.Length > 3 Then .APCarrierNumber = CarrierNumber
                Else
                    If strData.Length > 3 Then .APCarrierNumber = DTran.NZ(strData(3), CType(0, Integer))
                End If
                If strData.Length > 4 Then .APBillNumber = DTran.NZ(strData(4), "")
                If strData.Length > 5 Then .APBillDate = DTran.NZ(strData(5), CType(Date.Now, Date))
                If strData.Length > 6 Then .APCustomerID = DTran.NZ(strData(6), "")
                If strData.Length > 7 Then .APCostCenterNumber = DTran.NZ(strData(7), "")
                If strData.Length > 8 Then .APTotalCost = DTran.NZ(strData(8), CType(0, Decimal))
                If strData.Length > 9 Then .APBLNumber = DTran.NZ(strData(9), "")
                If strData.Length > 10 Then .APBilledWeight = DTran.NZ(strData(10), CType(0, Integer))
                If strData.Length > 11 Then .APTotalTax = DTran.NZ(strData(11), CType(0, Decimal))
                .APReceivedDate = Date.Now
                .APPayCode = "N"
                .APElectronicFlag = True
                .APOverwrite = False
            End With

        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_DataValidationFailure"))

        End Try
        Return oBatchProcessing.InsertFreightBillUnique(oFreightBill, True)
    End Function

    Public Function ProcessCSVLaneData(ByVal record As String) As Boolean
        Dim blnRet As Boolean = False
        Dim oBatchProcessing As New NGLBatchProcessDataProvider(Me.Parameters)
        Dim strData() As String
        Try
            strData = DTran.DecodeCSV(record)
        Catch ex As ApplicationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_DataValidationFailure"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
        If strData Is Nothing OrElse strData.Length < 1 Then Return False
        Dim oLane As New DTO.LaneIntegration
        Dim dblDefault As Double = 0.0
        Try
            With oLane
                If strData.Length > 0 Then .LaneNumber = DTran.NZ(strData(0), "")
                If strData.Length > 1 Then .LaneName = DTran.NZ(strData(1), "")
                If strData.Length > 2 Then .LaneNumberMaster = DTran.NZ(strData(2), "")
                If strData.Length > 3 Then .LaneNameMaster = DTran.NZ(strData(3), "")
                If strData.Length > 4 Then .LaneCompNumber = DTran.NZ(strData(4), "")
                If strData.Length > 5 Then .LaneDefaultCarrierUse = DTran.NZ(strData(5), False)
                If strData.Length > 6 Then .LaneDefaultCarrierNumber = DTran.NZ(strData(6), 0)
                If strData.Length > 7 Then .LaneOrigCompNumber = DTran.NZ(strData(7), "")
                If strData.Length > 8 Then .LaneOrigName = DTran.NZ(strData(8), "")
                If strData.Length > 9 Then .LaneOrigAddress1 = DTran.NZ(strData(9), "")
                If strData.Length > 10 Then .LaneOrigAddress2 = DTran.NZ(strData(10), "")
                If strData.Length > 11 Then .LaneOrigAddress3 = DTran.NZ(strData(11), "")
                If strData.Length > 12 Then .LaneOrigCity = DTran.NZ(strData(12), "")
                If strData.Length > 13 Then .LaneOrigState = DTran.NZ(strData(13), "")
                If strData.Length > 14 Then .LaneOrigCountry = DTran.NZ(strData(14), "")
                If strData.Length > 15 Then .LaneOrigZip = DTran.NZ(strData(15), "")
                If strData.Length > 16 Then .LaneOrigContactPhone = DTran.NZ(strData(16), "")
                If strData.Length > 17 Then .LaneOrigContactPhoneExt = DTran.NZ(strData(17), "")
                If strData.Length > 18 Then .LaneOrigContactFax = DTran.NZ(strData(18), "")
                If strData.Length > 19 Then .LaneDestCompNumber = DTran.NZ(strData(19), "")
                If strData.Length > 20 Then .LaneDestName = DTran.NZ(strData(20), "")
                If strData.Length > 21 Then .LaneDestAddress1 = DTran.NZ(strData(21), "")
                If strData.Length > 22 Then .LaneDestAddress2 = DTran.NZ(strData(22), "")
                If strData.Length > 23 Then .LaneDestAddress3 = DTran.NZ(strData(23), "")
                If strData.Length > 24 Then .LaneDestCity = DTran.NZ(strData(24), "")
                If strData.Length > 25 Then .LaneDestState = DTran.NZ(strData(25), "")
                If strData.Length > 26 Then .LaneDestCountry = DTran.NZ(strData(26), "")
                If strData.Length > 27 Then .LaneDestZip = DTran.NZ(strData(27), "")
                If strData.Length > 28 Then .LaneDestContactPhone = DTran.NZ(strData(28), "")
                If strData.Length > 29 Then .LaneDestContactPhoneExt = DTran.NZ(strData(29), "")
                If strData.Length > 30 Then .LaneDestContactFax = DTran.NZ(strData(30), "")
                If strData.Length > 31 Then .LaneConsigneeNumber = DTran.NZ(strData(31), "")
                If strData.Length > 32 Then .LaneRecMinIn = DTran.NZ(strData(32), 0)
                If strData.Length > 33 Then .LaneRecMinUnload = DTran.NZ(strData(33), 0)
                If strData.Length > 34 Then .LaneRecMinOut = DTran.NZ(strData(34), 0)
                If strData.Length > 35 Then .LaneAppt = DTran.NZ(strData(35), False)
                If strData.Length > 36 Then .LanePalletExchange = DTran.NZ(strData(36), False)
                If strData.Length > 37 Then .LanePalletType = DTran.NZ(strData(37), "")
                If strData.Length > 38 Then .LaneBenchMiles = DTran.NZ(strData(38), 0)
                If strData.Length > 39 Then .LaneBFC = DTran.NZ(strData(39), dblDefault)
                If strData.Length > 40 Then .LaneBFCType = DTran.NZ(strData(40), "")
                If strData.Length > 41 Then .LaneRecHourStart = DTran.NZ(strData(41), "")
                If strData.Length > 42 Then .LaneRecHourStop = DTran.NZ(strData(42), "")
                If strData.Length > 43 Then .LaneDestHourStart = DTran.NZ(strData(43), "")
                If strData.Length > 44 Then .LaneDestHourStop = DTran.NZ(strData(44), "")
                If strData.Length > 45 Then .LaneComments = DTran.NZ(strData(45), "")
                If strData.Length > 46 Then .LaneCommentsConfidential = DTran.NZ(strData(46), "")
                If strData.Length > 47 Then .LaneLatitude = DTran.NZ(strData(47), dblDefault)
                If strData.Length > 48 Then .LaneLongitude = DTran.NZ(strData(48), dblDefault)
                If strData.Length > 49 Then .LaneTempType = DTran.NZ(strData(49), 0)
                If strData.Length > 50 Then .LaneTransType = DTran.NZ(strData(50), 0)
                If strData.Length > 51 Then .LanePrimaryBuyer = DTran.NZ(strData(51), "")
                If strData.Length > 52 Then .LaneAptDelivery = DTran.NZ(strData(52), False)
                If strData.Length > 53 Then .BrokerNumber = DTran.NZ(strData(53), "")
                If strData.Length > 54 Then .BrokerName = DTran.NZ(strData(54), "")
                If strData.Length > 55 Then .LaneOriginAddressUse = DTran.NZ(strData(55), False)
                If strData.Length > 56 Then .LaneCarrierEquipmentCodes = DTran.NZ(strData(56), "")
                If strData.Length > 57 Then .LaneChepGLID = DTran.NZ(strData(57), "")
                If strData.Length > 58 Then .LaneCarrierTypeCode = DTran.NZ(strData(58), "")
                If strData.Length > 59 Then .LanePickUpMon = DTran.NZ(strData(59), False)
                If strData.Length > 60 Then .LanePickUpTue = DTran.NZ(strData(60), False)
                If strData.Length > 61 Then .LanePickUpWed = DTran.NZ(strData(61), False)
                If strData.Length > 62 Then .LanePickUpThu = DTran.NZ(strData(62), False)
                If strData.Length > 63 Then .LanePickUpFri = DTran.NZ(strData(63), False)
                If strData.Length > 64 Then .LanePickUpSat = DTran.NZ(strData(64), False)
                If strData.Length > 65 Then .LanePickUpSun = DTran.NZ(strData(65), False)
                If strData.Length > 67 Then .LaneDropOffMon = DTran.NZ(strData(67), False)
                If strData.Length > 68 Then .LaneDropOffTue = DTran.NZ(strData(68), False)
                If strData.Length > 69 Then .LaneDropOffWed = DTran.NZ(strData(69), False)
                If strData.Length > 70 Then .LaneDropOffThu = DTran.NZ(strData(70), False)
                If strData.Length > 71 Then .LaneDropOffFri = DTran.NZ(strData(71), False)
                If strData.Length > 72 Then .LaneDropOffSat = DTran.NZ(strData(72), False)
                If strData.Length > 73 Then .LaneDropOffSun = DTran.NZ(strData(73), False)
            End With

            blnRet = ProcessLaneData(oLane)

        Catch ex As FaultException
            Throw

        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_DataValidationFailure"))

        End Try
        Return blnRet 'oBatchProcessing.InsertFreightBillUnique(oFreightBill, True)
    End Function

    Public Function GetAllTMPCSVCarrierRates() As LTS.tmpCSVCarrierRate()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim items() = (
                From d In db.tmpCSVCarrierRates).ToArray()
                Return items

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

    Public Function GetFirstRecordTMPCSVCarrierRates() As LTS.tmpCSVCarrierRate
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim items = (
                From d In db.tmpCSVCarrierRates).FirstOrDefault
                Return items

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

    Public Function ImportAllTMPCSVInterlinePoints() As Boolean
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Return Convert.ToBoolean(db.spImportTmpCSVInterlinePoints)

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

    Public Function ImportAllTMPCSVNonServicePoints() As Boolean
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Return Convert.ToBoolean(db.spImportTmpNonServicePoints)

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

    Public Function ProcessTMPCSVCarrierRates(ByVal tableName As String,
                                              ByVal FilePath As String,
                                              ByVal TruncateData As Boolean,
                                              ByVal SelectData As Boolean) As Boolean
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Return Convert.ToBoolean(db.spImportCSVData(tableName, FilePath, TruncateData, SelectData))

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
    ''' Imports temp data from the tmpCSVCarrierRates table into the Tariff Matrix child tables
    ''' using the key parameters provided. 
    ''' On error changes are rolled back and nothing is saved.
    ''' </summary>
    ''' <param name="CarrTarEquipMatCarrTarEquipControl"></param>
    ''' <param name="CarrTarEquipMatCarrTarControl"></param>
    ''' <param name="CarrTarEquipMatCarrTarMatBPControl"></param>
    ''' <param name="CarrTarEquipMatModUser"></param>
    ''' <param name="CarrTarEquipMatName"></param>
    ''' <returns>
    ''' Return Data Table:
    ''' Success bit Not Null (true or false)
    ''' RecordsProcessed bigInt Not Null (number of records that were read.  
    '''         on error the system will roll back changes so nothing will be saved; 
    '''         this value can be used to determine which record in the temp table failed to import.)
    ''' RetMsg nvarchar(4000) NULL (Success or Error Details if ErrNumber is not zero)
    ''' ErrNumber int NULL (a value of 0 indicates no errors.  
    '''         a value of 1 indicates a parameter validation error.  
    '''         All other values are sql errors, RetMsg will have details.)
    ''' StartDate datetime NULL (Date and Time that the process started)
    ''' EndDate datetime NULL (Date and Time that the process finished)
    ''' </returns>
    ''' <remarks>
    ''' </remarks>
    Public Function ImportCSVRatesFromTmpTbl(ByVal CarrTarEquipMatCarrTarEquipControl As Integer,
                                            ByVal CarrTarEquipMatCarrTarControl As Integer,
                                            ByVal CarrTarEquipMatCarrTarMatBPControl As Integer,
                                            ByVal CarrTarEquipMatModUser As String,
                                            ByVal CarrTarEquipMatName As String) As LTS.spImportCSVRatesFromTmpTblResult

        Using db As New NGLMASIntegrationDataContext(ConnectionString, 3600)
            Try

                Return db.spImportCSVRatesFromTmpTbl(CarrTarEquipMatCarrTarEquipControl,
                                                         CarrTarEquipMatCarrTarControl,
                                                         CarrTarEquipMatCarrTarMatBPControl,
                                                         Left(CarrTarEquipMatModUser, 100),
                                                         Left(CarrTarEquipMatName, 50)).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ImportCSVRatesFromTmpTbl"))
            End Try

            Return Nothing

        End Using
    End Function
#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Return Nothing
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

    Private Function ProcessLaneData(ByVal oLane As DTO.LaneIntegration) As Boolean


        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim blnOriginUse As Boolean = False
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim strSource As String = "NGLFlatFileImport.ProcessLaneData"
        Dim strHeaderTable As String = "Lane"
        Dim strItemTable As String = ""
        Dim strCalendarTable As String = "LaneCal"
        Me.HeaderName = "Lane"
        Me.CalendarName = "Lane Calendar"
        Me.ImportTypeKey = IntegrationTypes.Lane
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Lane Data Integration"


        'set the error date time stamp and other Defaults
        'Header Information
        Dim oFields As New List(Of DTO.clsImportField)
        If Not buildHeaderCollection(oFields) Then Return ProcessDataReturnValues.nglDataIntegrationFailure
        Return importHeaderRecord(oLane, oFields)

    End Function

    Private Function buildHeaderCollection(ByRef oFields As List(Of DTO.clsImportField)) As Boolean
        Dim Ret As Boolean = False

        With oFields
            .Add(New DTO.clsImportField("LaneNumber", "LaneNumber", DTO.clsImportField.DataTypeID.gcvdtString, 50, False, DTO.clsImportField.PKValue.gcPK)) '0
            .Add(New DTO.clsImportField("LaneName", "LaneName", DTO.clsImportField.DataTypeID.gcvdtString, 50, False)) '1
            .Add(New DTO.clsImportField("LaneNumberMaster", "LaneNumberMaster", DTO.clsImportField.DataTypeID.gcvdtString, 50, True)) '2
            .Add(New DTO.clsImportField("LaneNameMaster", "LaneNameMaster", DTO.clsImportField.DataTypeID.gcvdtString, 50, True)) '3
            .Add(New DTO.clsImportField("LaneCompNumber", "LaneCompNumber", DTO.clsImportField.DataTypeID.gcvdtString, 50, True)) '4
            .Add(New DTO.clsImportField("LaneDefaultCarrierUse", "LaneDefaultCarrierUse", DTO.clsImportField.DataTypeID.gcvdtBit, 2, True)) '5
            .Add(New DTO.clsImportField("LaneDefaultCarrierNumber", "LaneDefaultCarrierNumber", DTO.clsImportField.DataTypeID.gcvdtLongInt, 11, True)) '6
            .Add(New DTO.clsImportField("LaneOrigCompNumber", "LaneOrigCompNumber", DTO.clsImportField.DataTypeID.gcvdtString, 50, True)) '7
            .Add(New DTO.clsImportField("LaneOrigName", "LaneOrigName", DTO.clsImportField.DataTypeID.gcvdtString, 40, True)) '8
            .Add(New DTO.clsImportField("LaneOrigAddress1", "LaneOrigAddress1", DTO.clsImportField.DataTypeID.gcvdtString, 40, True)) '9
            .Add(New DTO.clsImportField("LaneOrigAddress2", "LaneOrigAddress2", DTO.clsImportField.DataTypeID.gcvdtString, 40, True)) '10
            .Add(New DTO.clsImportField("LaneOrigAddress3", "LaneOrigAddress3", DTO.clsImportField.DataTypeID.gcvdtString, 40, True)) '11
            .Add(New DTO.clsImportField("LaneOrigCity", "LaneOrigCity", DTO.clsImportField.DataTypeID.gcvdtString, 25, True)) '12
            .Add(New DTO.clsImportField("LaneOrigState", "LaneOrigState", DTO.clsImportField.DataTypeID.gcvdtString, 8, True)) '13
            .Add(New DTO.clsImportField("LaneOrigCountry", "LaneOrigCountry", DTO.clsImportField.DataTypeID.gcvdtString, 30, True)) '14
            .Add(New DTO.clsImportField("LaneOrigZip", "LaneOrigZip", DTO.clsImportField.DataTypeID.gcvdtString, 50, True)) '15
            .Add(New DTO.clsImportField("LaneOrigContactPhone", "LaneOrigContactPhone", DTO.clsImportField.DataTypeID.gcvdtString, 15, True)) '16
            .Add(New DTO.clsImportField("LaneOrigContactPhoneExt", "LaneOrigContactPhoneExt", DTO.clsImportField.DataTypeID.gcvdtString, 50, True)) '17
            .Add(New DTO.clsImportField("LaneOrigContactFax", "LaneOrigContactFax", DTO.clsImportField.DataTypeID.gcvdtString, 15, True)) '18
            .Add(New DTO.clsImportField("LaneDestCompNumber", "LaneDestCompNumber", DTO.clsImportField.DataTypeID.gcvdtString, 50, True)) '19
            .Add(New DTO.clsImportField("LaneDestName", "LaneDestName", DTO.clsImportField.DataTypeID.gcvdtString, 40, True)) '20
            .Add(New DTO.clsImportField("LaneDestAddress1", "LaneDestAddress1", DTO.clsImportField.DataTypeID.gcvdtString, 40, True)) '21
            .Add(New DTO.clsImportField("LaneDestAddress2", "LaneDestAddress2", DTO.clsImportField.DataTypeID.gcvdtString, 40, True)) '22
            .Add(New DTO.clsImportField("LaneDestAddress3", "LaneDestAddress3", DTO.clsImportField.DataTypeID.gcvdtString, 40, True)) '23
            .Add(New DTO.clsImportField("LaneDestCity", "LaneDestCity", DTO.clsImportField.DataTypeID.gcvdtString, 25, True)) '24
            .Add(New DTO.clsImportField("LaneDestState", "LaneDestState", DTO.clsImportField.DataTypeID.gcvdtString, 2, True)) '25
            .Add(New DTO.clsImportField("LaneDestCountry", "LaneDestCountry", DTO.clsImportField.DataTypeID.gcvdtString, 30, True)) '26
            .Add(New DTO.clsImportField("LaneDestZip", "LaneDestZip", DTO.clsImportField.DataTypeID.gcvdtString, 10, True)) '27
            .Add(New DTO.clsImportField("LaneDestContactPhone", "LaneDestContactPhone", DTO.clsImportField.DataTypeID.gcvdtString, 15, True)) '28
            .Add(New DTO.clsImportField("LaneDestContactPhoneExt", "LaneDestContactPhoneExt", DTO.clsImportField.DataTypeID.gcvdtString, 50, True)) '29
            .Add(New DTO.clsImportField("LaneDestContactFax", "LaneDestContactFax", DTO.clsImportField.DataTypeID.gcvdtString, 15, True)) '30
            .Add(New DTO.clsImportField("LaneConsigneeNumber", "LaneConsigneeNumber", DTO.clsImportField.DataTypeID.gcvdtString, 50, True)) '31
            .Add(New DTO.clsImportField("LaneRecMinIn", "LaneRecMinIn", DTO.clsImportField.DataTypeID.gcvdtLongInt, 11, True)) '32
            .Add(New DTO.clsImportField("LaneRecMinUnload", "LaneRecMinUnload", DTO.clsImportField.DataTypeID.gcvdtLongInt, 11, True)) '33
            .Add(New DTO.clsImportField("LaneRecMinOut", "LaneRecMinOut", DTO.clsImportField.DataTypeID.gcvdtLongInt, 11, True)) '34
            .Add(New DTO.clsImportField("LaneAppt", "LaneAppt", DTO.clsImportField.DataTypeID.gcvdtBit, 2, True)) '35
            .Add(New DTO.clsImportField("LanePalletExchange", "LanePalletExchange", DTO.clsImportField.DataTypeID.gcvdtBit, 2, True)) '36
            .Add(New DTO.clsImportField("LanePalletType", "LanePalletType", DTO.clsImportField.DataTypeID.gcvdtString, 1, True)) '37
            .Add(New DTO.clsImportField("LaneBenchMiles", "LaneBenchMiles", DTO.clsImportField.DataTypeID.gcvdtLongInt, 11, True)) '38
            .Add(New DTO.clsImportField("LaneBFC", "LaneBFC", DTO.clsImportField.DataTypeID.gcvdtFloat, 11, True)) '39
            .Add(New DTO.clsImportField("LaneBFCType", "LaneBFCType", DTO.clsImportField.DataTypeID.gcvdtString, 10, True)) '40
            .Add(New DTO.clsImportField("LaneRecHourStart", "LaneRecHourStart", DTO.clsImportField.DataTypeID.gcvdtDate, 22, True)) '41
            .Add(New DTO.clsImportField("LaneRecHourStop", "LaneRecHourStop", DTO.clsImportField.DataTypeID.gcvdtDate, 22, True)) '42
            .Add(New DTO.clsImportField("LaneDestHourStart", "LaneDestHourStart", DTO.clsImportField.DataTypeID.gcvdtDate, 22, True)) '43
            .Add(New DTO.clsImportField("LaneDestHourStop", "LaneDestHourStop", DTO.clsImportField.DataTypeID.gcvdtDate, 22, True)) '44
            .Add(New DTO.clsImportField("LaneComments", "LaneComments", DTO.clsImportField.DataTypeID.gcvdtString, 255, True)) '45
            .Add(New DTO.clsImportField("LaneCommentsConfidential", "LaneCommentsConfidential", DTO.clsImportField.DataTypeID.gcvdtString, 255, True)) '46
            .Add(New DTO.clsImportField("LaneLatitude", "LaneLatitude", DTO.clsImportField.DataTypeID.gcvdtFloat, 11, True)) '47
            .Add(New DTO.clsImportField("LaneLongitude", "LaneLongitude", DTO.clsImportField.DataTypeID.gcvdtFloat, 11, True)) '48
            .Add(New DTO.clsImportField("LaneTempType", "LaneTempType", DTO.clsImportField.DataTypeID.gcvdtSmallInt, 6, True)) '49
            .Add(New DTO.clsImportField("LaneTransType", "LaneTransType", DTO.clsImportField.DataTypeID.gcvdtSmallInt, 6, True)) '50
            .Add(New DTO.clsImportField("LanePrimaryBuyer", "LanePrimaryBuyer", DTO.clsImportField.DataTypeID.gcvdtString, 50, True)) '51
            .Add(New DTO.clsImportField("LaneAptDelivery", "LaneAptDelivery", DTO.clsImportField.DataTypeID.gcvdtBit, 2, True)) '52
            .Add(New DTO.clsImportField("BrokerNumber", "BrokerNumber", DTO.clsImportField.DataTypeID.gcvdtString, 50, True)) '53
            .Add(New DTO.clsImportField("BrokerName", "BrokerName", DTO.clsImportField.DataTypeID.gcvdtString, 30, True)) '54
            .Add(New DTO.clsImportField("LaneOriginAddressUse", "LaneOriginAddressUse", DTO.clsImportField.DataTypeID.gcvdtBit, 2, True)) '55
            .Add(New DTO.clsImportField("LaneCarrierEquipmentCodes", "LaneCarrierEquipmentCodes", DTO.clsImportField.DataTypeID.gcvdtString, 50, True, DTO.clsImportField.PKValue.gcHK)) '56
            .Add(New DTO.clsImportField("LaneChepGLID", "LaneChepGLID", DTO.clsImportField.DataTypeID.gcvdtString, 50, True)) '57
            .Add(New DTO.clsImportField("LaneCarrierTypeCode", "LaneCarrierTypeCode", DTO.clsImportField.DataTypeID.gcvdtString, 20, True)) '58
            .Add(New DTO.clsImportField("LanePickUpMon", "LanePickUpMon", DTO.clsImportField.DataTypeID.gcvdtBit, 2, False)) '59
            .Add(New DTO.clsImportField("LanePickUpTue", "LanePickUpTue", DTO.clsImportField.DataTypeID.gcvdtBit, 2, False)) '60
            .Add(New DTO.clsImportField("LanePickUpWed", "LanePickUpWed", DTO.clsImportField.DataTypeID.gcvdtBit, 2, False)) '61
            .Add(New DTO.clsImportField("LanePickUpThu", "LanePickUpThu", DTO.clsImportField.DataTypeID.gcvdtBit, 2, False)) '62
            .Add(New DTO.clsImportField("LanePickUpFri", "LanePickUpFri", DTO.clsImportField.DataTypeID.gcvdtBit, 2, False)) '63
            .Add(New DTO.clsImportField("LanePickUpSat", "LanePickUpSat", DTO.clsImportField.DataTypeID.gcvdtBit, 2, False)) '64
            .Add(New DTO.clsImportField("LanePickUpSun", "LanePickUpSun", DTO.clsImportField.DataTypeID.gcvdtBit, 2, False)) '65
            .Add(New DTO.clsImportField("LaneDropOffMon", "LaneDropOffMon", DTO.clsImportField.DataTypeID.gcvdtBit, 2, False)) '66
            .Add(New DTO.clsImportField("LaneDropOffTue", "LaneDropOffTue", DTO.clsImportField.DataTypeID.gcvdtBit, 2, False)) '67
            .Add(New DTO.clsImportField("LaneDropOffWed", "LaneDropOffWed", DTO.clsImportField.DataTypeID.gcvdtBit, 2, False)) '68
            .Add(New DTO.clsImportField("LaneDropOffThu", "LaneDropOffThu", DTO.clsImportField.DataTypeID.gcvdtBit, 2, False)) '69
            .Add(New DTO.clsImportField("LaneDropOffFri", "LaneDropOffFri", DTO.clsImportField.DataTypeID.gcvdtBit, 2, False)) '70
            .Add(New DTO.clsImportField("LaneDropOffSat", "LaneDropOffSat", DTO.clsImportField.DataTypeID.gcvdtBit, 2, False)) '71
            .Add(New DTO.clsImportField("LaneDropOffSun", "LaneDropOffSun", DTO.clsImportField.DataTypeID.gcvdtBit, 2, False)) '72
        End With
        updateImportFieldFlags(oFields)

        Ret = True

        Return Ret

    End Function

    Private Sub updateImportFieldFlags(ByRef oFields As List(Of DTO.clsImportField))
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim oImportFields = (From
                                        d In db.tblImportFields
                                     Where
                                         d.ImportFileType = IntegrationTypes.Lane
                                     Select
                                         d.ImportControl, d.ImportFieldName, d.ImportFileName, d.ImportFileType, d.ImportFlag).ToList



                If Not oImportFields Is Nothing AndAlso oImportFields.Count > 0 Then
                    For Each F In oFields
                        Dim oItem = (From i In oImportFields Where i.ImportFieldName = F.Name Select i.ImportFlag).First
                        'If Not oItem Is Nothing Then
                        F.Use = oItem

                        'End If
                    Next
                End If



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
    End Sub

    Private Function importHeaderRecord(ByRef oLane As DTO.LaneIntegration,
                                         ByRef oFields As List(Of DTO.clsImportField)) As Boolean
        Dim Ret As Boolean = False
        Dim blnOriginUse As Boolean = False
        Dim LaneData As NGLLaneData = Me.NDPBaseClassFactory("NGLLaneData")
        Try

            'now get the Carrier Header Records
            Dim strSource As String = "NGLFlatFileImport.importHeaderRecord"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""


            Try
                Try
                    Dim intHeaderFieldCt As Integer = 72 'actual number of fields              
                    'set the default for the Use LaneBenchMiles flag
                    Dim blnUseBenchMilesDefault As Boolean = getFieldItemByKey(oFields, "LaneBenchMiles").Use
                    'set the default for the use LaneLatitude and LaneLongitude values
                    Dim blnUseLaneLat As Boolean = getFieldItemByKey(oFields, "LaneLatitude").Use
                    Dim blnUseLaneLong As Boolean = getFieldItemByKey(oFields, "LaneLongitude").Use

                    strErrorMessage = ""
                    blnDataValidated = validateFields(oFields, oLane, strErrorMessage, strSource)
                    'As of v-4.7.3 we expect the LaneOriginAddressUse to be provided as part of the download.  The default should always be false.
                    blnOriginUse = False
                    Boolean.TryParse(getFieldItemByKey(oFields, "LaneOriginAddressUse").Value, blnOriginUse)
                    'test if the record already exists. 
                    Select Case LaneData.GetLaneControlIfExist(oLane.LaneNumber)
                        Case -1
                            blnDataValidated = False
                            strErrorMessage = "Could not check for existing lane record.  The lane number " & getFieldItemByKey(oFields, "LaneNumber").Value & " has not been downloaded."
                        Case 0 'no records found so insert a new one
                            mblnInsertRecord = True
                        Case Else
                            mblnInsertRecord = False
                    End Select
                    'Look up the Company Control Number
                    If blnDataValidated AndAlso Not lookupCompControlByAlphaCode(getFieldItemByKey(oFields, "LaneCompNumber"), "LaneCompControl") Then
                        blnDataValidated = False
                        strErrorMessage = "Invalid reference to Company Number " & getFieldItemByKey(oFields, "LaneCompNumber").Value
                    End If
                    'Look up the Default Carrier Control Number LaneDefaultCarrierNumber
                    If blnDataValidated AndAlso Not lookupDefaultCarrier(getFieldItemByKey(oFields, "LaneDefaultCarrierNumber"), "LaneDefaultCarrierControl") Then
                        blnDataValidated = False
                        strErrorMessage = "Invalid reference to the Default Carrier Number " & getFieldItemByKey(oFields, "LaneDefaultCarrierNumber").Value
                    End If

                    'Now we need to lookup the origin address and name values if a LaneOrigCompNumber is provided
                    If blnDataValidated AndAlso Not lookupCompAddress(getFieldItemByKey(oFields, "LaneOrigCompNumber"),
                            getFieldItemByKey(oFields, "LaneOrigName"),
                            getFieldItemByKey(oFields, "LaneOrigAddress1"),
                            getFieldItemByKey(oFields, "LaneOrigAddress2"),
                            getFieldItemByKey(oFields, "LaneOrigAddress3"),
                            getFieldItemByKey(oFields, "LaneOrigCity"),
                            getFieldItemByKey(oFields, "LaneOrigState"),
                            getFieldItemByKey(oFields, "LaneOrigCountry"),
                            getFieldItemByKey(oFields, "LaneOrigZip"),
                            getFieldItemByKey(oFields, "LaneOrigContactPhone"),
                            getFieldItemByKey(oFields, "LaneOrigContactFax"),
                            "LaneOrigCompControl") Then
                        blnDataValidated = False
                        strErrorMessage = "Invalid reference to Origin Company Number " & getFieldItemByKey(oFields, "LaneOrigCompNumber").Value
                    End If

                    'Now we need to lookup the destination address and name values if a LaneDestCompNumber is provided
                    If blnDataValidated AndAlso Not lookupCompAddress(getFieldItemByKey(oFields, "LaneDestCompNumber"),
                            getFieldItemByKey(oFields, "LaneDestName"),
                            getFieldItemByKey(oFields, "LaneDestAddress1"),
                            getFieldItemByKey(oFields, "LaneDestAddress2"),
                            getFieldItemByKey(oFields, "LaneDestAddress3"),
                            getFieldItemByKey(oFields, "LaneDestCity"),
                            getFieldItemByKey(oFields, "LaneDestState"),
                            getFieldItemByKey(oFields, "LaneDestCountry"),
                            getFieldItemByKey(oFields, "LaneDestZip"),
                            getFieldItemByKey(oFields, "LaneDestContactPhone"),
                            getFieldItemByKey(oFields, "LaneDestContactFax"),
                            "LaneDestCompControl") Then
                        blnDataValidated = False
                        strErrorMessage = "Invalid reference to Destination Company Number " & getFieldItemByKey(oFields, "LaneDestCompNumber").Value
                    End If

                    If Not blnDataValidated Then
                        addToErrorTable(oFields, IntegrationTypes.Lane, strErrorMessage, "Data Integration DLL", mstrHeaderName)
                        Return False
                    Else
                        'Save the use values for Bench Miles, Lat and Long because calcLatLong and calcMiles changes them
                        blnUseBenchMilesDefault = getFieldItemByKey(oFields, "LaneBenchMiles").Use
                        blnUseLaneLat = getFieldItemByKey(oFields, "LaneLatitude").Use
                        blnUseLaneLong = getFieldItemByKey(oFields, "LaneLongitude").Use
                        'Check if the address has changed
                        ' ''If mblnInsertRecord OrElse (isAddressTheSame(oFields, getFieldItemByKey(oFields, "LaneNumber"), "Lane") = 0) Then
                        ' ''    'Get the Lat Long and miles if needed
                        ' ''    calcLatLong(oFields, False)
                        ' ''    calcMiles(oFields, blnOriginUse, False)
                        ' ''End If
                        '' ''Save the changes to the main table
                        ' ''If Not saveLaneData(oFields, mblnInsertRecord, "Lane") Then
                        ' ''    strErrorMessage = "Unable to save lane data to database download failed for lane number " & getFieldItemByKey(oFields, "LaneNumber").Value
                        ' ''    addToErrorTable(oFields, IntegrationTypes.Lane, strErrorMessage, "Data Integration DLL", mstrHeaderName)
                        ' ''Else
                        ' ''    'Get the LaneControlNumber Back
                        ' ''    Dim intLaneControl = getLaneControl(getFieldItemByKey(oFields, "LaneNumber"))
                        ' ''    If intLaneControl > 0 Then
                        ' ''        'Now add/update the Broker Number and Broker Name Fields to the LaneSec Table
                        ' ''        saveBrokerData(oFields, intLaneControl)
                        ' ''        saveSpecialCodes(oFields, intLaneControl)
                        ' ''    End If

                        ' ''End If
                        'We need to reset the Use Bench Miles and Lat and Long values
                        getFieldItemByKey(oFields, "LaneBenchMiles").Use = blnUseBenchMilesDefault
                        getFieldItemByKey(oFields, "LaneLatitude").Use = blnUseLaneLat
                        getFieldItemByKey(oFields, "LaneLongitude").Use = blnUseLaneLong
                    End If
                    'Debug.Print strSQL
                    mblnInsertRecord = False
                    strErrorMessage = ""
                    blnDataValidated = False

                    Return True
                Catch ex As Exception
                    Throw
                Finally
                    'Object dispoded of in sub routines via Using statement
                    'Try
                    '    oPCmiles.Dispose()
                    'Catch ex As Exception

                    'End Try
                End Try
            Catch ex As Exception
                ' ''If intRetryCt > Me.Retry Then
                ' ''    ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsLane.importHeaderRecords, attempted to import lane header records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                ' ''    Log("NGL.FreightMaster.Integration.clsLane.importHeaderRecords Failed!" & readExceptionMessage(ex))
                ' ''Else
                ' ''    Log("NGL.FreightMaster.Integration.clsLane.importHeaderRecords Failure Retry = " & intRetryCt.ToString)
                ' ''End If
            End Try

        Catch ex As System.InvalidCastException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_ApplicationException"}, New FaultReason("E_InvalidOperationException"))
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            Throw
        End Try
        Return Ret

    End Function

    Private Function validateFields(ByRef oFields As List(Of DTO.clsImportField),
                                      ByRef oRow As DTO.LaneIntegration,
                                      ByRef strErrorMessage As String,
                                      ByVal strSource As String) As Boolean

        For intct As Integer = 1 To oFields.Count
            'Because we use all fields on insert and we only
            'apply the Use flag on updates we ignore this flag
            'when reading data 
            '(we dont know if this is an insert or an update yet)
            'If oFields(intct).Use Then
            Try
                If oFields(intct).PK <> gcIgnore And oFields(intct).PK <> gcFK Then
                    'Dim oItem =
                    oFields(intct).Value = validateSQLValue(
                        oRow.Item(oFields(intct).Key) _
                        , CInt(oFields(intct).DataType) _
                        , strSource _
                        , "Invalid " & oFields(intct).Key _
                        , oFields(intct).Null _
                        , oFields(intct).Length)
                End If
            Catch ex As System.ArgumentException
                'there is a problem with the oFields Collection
                strErrorMessage &= "There is a problem with the data fields collection the actual error is: " & ex.Message
                Return False
            Catch ex As System.ApplicationException
                strErrorMessage &= ex.Message
                Return False
            Catch ex As Exception
                Throw
            End Try
            'End If
        Next
        'If no error we return true
        Return True
    End Function

    Private Function GetCompControlByAlpha(ByVal compNumber As String) As Integer
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                'Get compcontrol number
                Dim Control = db.udfGetCompControlByAlpha(compNumber)
                Return Control

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

    Private Function lookupCompControlByAlphaCode(ByRef oField As DTO.clsImportField,
                                                    ByVal strNewFieldName As String) As Boolean
        Dim blnRet As Boolean = False
        Dim intCompControl As Integer = 0

        If oField.Value <> "''" And oField.Value.ToUpper <> "NULL" Then

            intCompControl = GetCompControlByAlpha(DTran.stripQuotes(oField.Value))

            If intCompControl > 0 Then
                oField.Name = strNewFieldName
                oField.Value = intCompControl
                oField.DataType = DTO.clsImportField.DataTypeID.gcvdtLongInt
                oField.Length = 11
                blnRet = True
            Else
                oField.Name = strNewFieldName
                oField.Value = 0
                oField.DataType = DTO.clsImportField.DataTypeID.gcvdtLongInt
                oField.Length = 11
                blnRet = True
            End If
        Else
            oField.Name = strNewFieldName
            oField.Value = 0
            oField.DataType = DTO.clsImportField.DataTypeID.gcvdtLongInt
            oField.Length = 11
            blnRet = True
        End If
        Return blnRet
    End Function

    Private Function lookupDefaultCarrier(ByRef oField As DTO.clsImportField, ByVal strNewFieldName As String) As Boolean
        Dim blnRet As Boolean = False
        Try
            If Val(oField.Value) > 0 Then
                'we have a valid carrier number so check the carrier table
                Dim oCarrier As NGLCarrierData = Me.NDPBaseClassFactory("NGLCarrierData")
                Dim intCarrierControl As Integer = 0
                intCarrierControl = oCarrier.GetCarrierControlIfExist(Val(oField.Value))
                Select Case intCarrierControl
                    Case -1
                        blnRet = False
                    Case Else
                        oField.Name = strNewFieldName
                        oField.Value = intCarrierControl
                        blnRet = True
                End Select
            Else
                oField.Name = strNewFieldName
                oField.Value = "0"
                blnRet = True
            End If
        Catch ex As System.InvalidCastException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_ApplicationException"}, New FaultReason("E_InvalidOperationException"))
        Catch ex As FaultException
            Throw
        End Try
        Return blnRet
    End Function

    Private Function lookupCompAddress(ByRef oCompField As DTO.clsImportField,
                                        ByRef oNameField As DTO.clsImportField,
                                        ByRef oAdd1Field As DTO.clsImportField,
                                        ByRef oAdd2Field As DTO.clsImportField,
                                        ByRef oAdd3Field As DTO.clsImportField,
                                        ByRef oCityField As DTO.clsImportField,
                                        ByRef oStateField As DTO.clsImportField,
                                        ByRef oCountryField As DTO.clsImportField,
                                        ByRef oZipField As DTO.clsImportField,
                                        ByRef oPhoneField As DTO.clsImportField,
                                        ByRef oFaxField As DTO.clsImportField,
                                        ByVal strNewFieldName As String) As Boolean
        Dim blnRet As Boolean = False
        Try
            Dim strCompFieldValue As String = Trim(DTran.stripQuotes(oCompField.Value))
            If strCompFieldValue <> "0" And strCompFieldValue <> "" And strCompFieldValue.ToUpper <> "NULL" Then
                'we have a valid company number so check th alpha x-ref table
                If Not lookupCompControlByAlphaCode(oCompField, strNewFieldName) Then
                    Return False
                End If
                If Val(oCompField.Value) < 1 Then Return False
                Dim oComp As NGLCompData = Me.NDPBaseClassFactory("NGLCompData")
                Dim oCompData = oComp.GetCompFiltered(Val(oCompField.Value))
                If Not oCompData Is Nothing AndAlso oCompData.CompControl > 0 Then
                    With oCompData
                        oNameField.Value = .CompName
                        oAdd1Field.Value = .CompStreetAddress1
                        oAdd2Field.Value = .CompStreetAddress2
                        oAdd3Field.Value = .CompStreetAddress3
                        oCityField.Value = .CompStreetCity
                        oStateField.Value = .CompStreetState
                        oCountryField.Value = .CompStreetCountry
                        oZipField.Value = .CompStreetZip
                        If Not .CompConts Is Nothing AndAlso .CompConts.Count > 0 Then
                            With .CompConts(0)
                                oPhoneField.Value = .CompContPhone
                                oFaxField.Value = .CompContFax
                            End With
                        End If
                    End With
                    blnRet = True
                End If
            Else
                oCompField.Name = strNewFieldName
                oCompField.Value = "0"
                blnRet = True
            End If
        Catch ex As System.InvalidCastException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_ApplicationException"}, New FaultReason("E_InvalidOperationException"))
        Catch ex As FaultException
            Throw
        End Try
        Return blnRet
    End Function

    Private Sub addToErrorTable(
            ByRef oFields As List(Of DTO.clsImportField),
            ByVal enumImportTypeKey As IntegrationTypes,
            ByVal strErrorMessage As String,
            ByVal strFile As String,
            ByVal strName As String)

        Using db As New NGLMASIntegrationDataContext
            Try
                'build data record string
                Dim strRecord As String = ""
                Dim blnFirstField As Boolean = True
                Dim strSpacer As String = ""

                For Each F In oFields

                    If F.Use Then
                        strRecord &= strSpacer & F.Name & " = " & F.Value
                        strSpacer = ","
                    End If
                Next

                Dim nObject = New LTS.FileImportErrorLog With {.ImportRecord = strRecord _
                                                              , .ErrorMsg = Left(strErrorMessage, 500) _
                                                              , .ErrorDate = Date.Now _
                                                              , .CreateUser = Left(Me.Parameters.UserName, 25) _
                                                              , .ImportFileName = Left(strFile, 255) _
                                                              , .ImportFileType = enumImportTypeKey _
                                                              , .ImportName = Left(strName, 50)}
                db.FileImportErrorLogs.InsertOnSubmit(nObject)
                db.SubmitChanges()
            Catch ex As Exception
                'We ignore all errors while saving error log data
            End Try
        End Using
    End Sub

    Private Function getFieldValueByKey(ByRef oFields As List(Of DTO.clsImportField), ByVal strKey As String) As String
        Return (From f In oFields Where f.Key = strKey Select f.Value).First
    End Function

    Private Function getFieldItemByKey(ByRef oFields As List(Of DTO.clsImportField), ByVal strKey As String) As DTO.clsImportField
        Return (From f In oFields Where f.Key = strKey).First
    End Function


#End Region

End Class


