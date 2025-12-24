Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel
Imports System.Linq.Dynamic
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports Ngl.Core.ChangeTracker
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports Ngl.FreightMaster.Data.LTS


Public Class NGLLaneData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLaneDataContext(ConnectionString)
        Me.LinqTable = db.Lanes
        Me.LinqDB = db
        Me.SourceClass = "NGLLaneData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            '    If _LinqTable Is Nothing Then
            Dim db As New NGLMASLaneDataContext(ConnectionString)
            _LinqTable = db.Lanes
            _LinqDB = db
            '   End If

            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property
    'Private _LaneFeesDependencyResult As LTS.spU
    'Public Property LaneFeesDependencyResult() As LTS.spUpdateBookDependenciesResult
    '    Get
    '        Return _LaneFeesDependencyResult
    '    End Get
    '    Set(ByVal value As LTS.spUpdateBookDependenciesResult)
    '        _LaneFeesDependencyResult = value
    '    End Set
    'End Property
#End Region

#Region "Public Methods"

    Public Function GetLanesByCarrier(ByVal CarrierControl As Integer) As DTO.Lane()
        Return GetLanesFiltered(CarrierControl:=CarrierControl)
    End Function

    Public Function GetLanesByComp(ByVal CompControl As Integer) As DTO.Lane()
        Return GetLanesFiltered(CompControl:=CompControl)
    End Function

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetLaneFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetLanesFiltered()
    End Function

    Public Function GetLaneFiltered(Optional ByVal Control As Integer = 0, Optional ByVal Number As String = "", Optional ByVal Name As String = "") As DTO.Lane
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.Lane)(Function(t As LTS.Lane) t.LaneCals)
                'oDLO.LoadWith(Of LTS.Lane)(Function(t As LTS.Lane) t.LaneCarrs)
                oDLO.LoadWith(Of LTS.Lane)(Function(t As LTS.Lane) t.LaneFees)
                oDLO.LoadWith(Of LTS.Lane)(Function(t As LTS.Lane) t.LaneSecs)
                db.LoadOptions = oDLO
                'Get the newest record that matches the provided criteria
                Dim Lane As DTO.Lane = (
                From t In db.Lanes
                Where
                    (t.LaneControl = If(Control = 0, t.LaneControl, Control)) _
                    And
                    (Number Is Nothing OrElse String.IsNullOrEmpty(Number) OrElse t.LaneNumber = Number) _
                    And
                    (Name Is Nothing OrElse String.IsNullOrEmpty(Name) OrElse t.LaneName = Name)
                Order By t.LaneNumber Descending
                Select selectDTODataWDetails(t)).First

                Return Lane

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetLanesFiltered(Optional ByVal MasterNumber As String = "", Optional ByVal MasterName As String = "", Optional ByVal CarrierControl As Integer = 0, Optional ByVal CompControl As Integer = 0) As DTO.Lane()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim Lanes As DTO.Lane() = (
                From t In db.Lanes
                Where
                    (t.LaneDefaultCarrierControl = If(CarrierControl = 0, t.LaneDefaultCarrierControl, CarrierControl)) _
                    And
                    (t.LaneCompControl = If(CompControl = 0, t.LaneCompControl, CompControl)) _
                    And
                    (MasterNumber Is Nothing OrElse String.IsNullOrEmpty(MasterNumber) OrElse t.LaneNumberMaster = MasterNumber) _
                    And
                    (MasterName Is Nothing OrElse String.IsNullOrEmpty(MasterName) OrElse t.LaneNameMaster = MasterName)
                Order By t.LaneNumber Descending
                Select selectDTOData(t)).ToArray

                Return Lanes

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function


    Public Function GetLaneFilteredByLaneNumber(ByVal LaneNumber As String) As DTO.Lane

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                Dim Lane As DTO.Lane = (
                From d In db.Lanes
                Where d.LaneNumber = LaneNumber
                Select selectDTOData(d)).FirstOrDefault()
                Return Lane
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneFilteredByLaneNumber"))
            End Try

            Return Nothing

        End Using

    End Function


    Public Function GetLaneFilteredByLegalEntity(ByVal LaneNumber As Integer, ByVal LaneLegalEntity As String) As DTO.Lane

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try


                Dim Lane As DTO.Lane = (
                From d In db.Lanes
                Where d.LaneNumber = LaneNumber _
                And (String.IsNullOrWhiteSpace(LaneLegalEntity) OrElse d.LaneLegalEntity = LaneLegalEntity)
                Select selectDTOData(d)).FirstOrDefault()


                ''This is where we would test the CompNumber parameter and throw and exception'
                'Dim largs As New List(Of String) From {"Lane", "LaneNumber", LaneNumber, LaneAlphaCode}
                'throwInvalidKeyFilterMetaDataException(SqlFaultInfo.FaultDetailsKey.E_CannotSaveRequiredValueDoesNotMatch, largs)
                'To catch this exception, you must check the SqlFaultInfo.message property for E_InvalidKeyFilterMetaData
                'To display the actual message in the log, you must interpret the SqlFaultInfo.Details emunerator 
                'and use the String.Format method with the SqlFaultInfo.DetailsList as the parameter array
                'Typically, we use the localization logic to look up the language reference for the enumerator 
                'E_CannotSaveRequiredValueDoesNotMatch, however, the web services does not have a reference to the localization 
                'project so we must manually copy the text into the String.Format method until such time as localization has been completed
                'Example: Log(String.Format("Cannot save changes to {0}. The key field {1} must be unique; the value {2} is already in use for AlphaCode {3}.", largs)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneFilteredByLegalEntity"))
            End Try

            Return Nothing

        End Using

    End Function

    Public Function GetLanesFilteredByLegalEntity(ByVal LaneLegalEntity As String) As DTO.Lane()

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim Lane() As DTO.Lane = (
                    From d In db.Lanes
                    Where d.LaneLegalEntity = LaneLegalEntity
                    Order By d.LaneNumber
                    Select selectDTOData(d)).ToArray()
                Return Lane
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLanesFilteredByLegalEntity"))
            End Try

            Return Nothing

        End Using

    End Function

    ''' <summary>
    ''' Returns the CarrierControl from CarrierRefLanes where CarrierNumber = (parameter) LaneDefaultCarrierNumber
    ''' </summary>
    ''' <param name="LaneDefaultCarrierNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarrierControl(ByVal LaneDefaultCarrierNumber As Integer) As Integer
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim nCarrierControl As Integer = (From c In db.CarrierRefLanes
                                                  Where c.CarrierNumber = LaneDefaultCarrierNumber
                                                  Select c.CarrierControl).FirstOrDefault
                Return nCarrierControl
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierControl"))
            End Try
        End Using
    End Function

    ''' <summary>
    ''' Returns the CompControl from CompRefLanes filtered by the parameters LaneLegalEntity and LaneCompAlphaCode
    ''' </summary>
    ''' <param name="LaneLegalEntity"></param>
    ''' <param name="LaneCompAlphaCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLaneCompControl(ByVal LaneLegalEntity As String, ByVal LaneCompAlphaCode As String) As Integer

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim nCompControl As Integer = (From c In db.CompRefLanes
                                               Where c.CompLegalEntity = LaneLegalEntity And c.CompAlphaCode = LaneCompAlphaCode
                                               Select c.CompControl).FirstOrDefault
                Return nCompControl
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneCompControl"))
            End Try

            Return Nothing

        End Using

    End Function

    ''' <summary>
    ''' Returns the CompControl from CompRefLanes filtered by LaneCompAlphaCode
    ''' </summary>
    ''' <param name="CompAlphaCode"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Create by RHR for v-8.2 on 04/04/2019
    '''     Should only be used only when multi-tennant is not required
    '''     typically used when the global parameter GlobalAllowBlankLegalEntityForIntegration = 1
    '''     the caller must check the GlobalAllowBlankLegalEntityForIntegration parameter value
    '''     and should not call this method when the value is not 1
    ''' </remarks>
    Public Function GetLaneCompControl(ByVal CompAlphaCode As String) As Integer

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim nCompControl As Integer = (From c In db.CompRefLanes
                                               Where c.CompAlphaCode = CompAlphaCode
                                               Select c.CompControl).FirstOrDefault
                Return nCompControl
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneCompControl"))
            End Try

            Return Nothing

        End Using

    End Function

    ''' <summary>
    ''' Returns the CompControl from CompRefLanes where CompRefLanes.CompNumber = parameter (LaneCompNumber)
    ''' </summary>
    ''' <param name="LaneCompNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLaneCompControl(ByVal LaneCompNumber As Integer) As Integer

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim nCompControl As Integer = (From c In db.CompRefLanes
                                               Where c.CompNumber = LaneCompNumber
                                               Select c.CompControl).FirstOrDefault
                Return nCompControl
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneCompControl"))
            End Try

            Return Nothing

        End Using

    End Function

    Public Function GetLaneNumber(ByVal LaneControl As Integer) As String

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim nLaneNo As String = (From c In db.Lanes
                                         Where c.LaneControl = LaneControl
                                         Select c.LaneNumber).FirstOrDefault
                Return nLaneNo
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneNumber"))
            End Try

            Return Nothing

        End Using

    End Function

    ''' <summary>
    ''' Updates the LaneSecBrokerName and LaneSecBrokerNumber of the file with the given LaneControl number.
    ''' If the provided LaneControl number does not exist, creates a new record with the given parameters.
    ''' Then saves the record to the database.
    ''' </summary>
    ''' <param name="BrokerName"></param>
    ''' <param name="BrokerNumber"></param>
    ''' <param name="intLaneControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function saveBrokerData70(ByVal BrokerName As String,
                                      ByVal BrokerNumber As String,
                                      ByVal intLaneControl As Integer) As Boolean
        Dim Ret As Boolean = False
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim blnExists = db.LaneSecs.Any(Function(x) x.LaneSecLaneControl = intLaneControl)
                Dim LaneSecData As NGLLaneSecData = NDPBaseClassFactory("NGLLaneSecData")
                Dim oData As New List(Of DTO.LaneSec)
                If blnExists Then
                    oData = LaneSecData.GetLaneSecsFiltered(intLaneControl).ToList
                    If Not oData Is Nothing AndAlso oData.Count > 0 Then
                        oData(0).LaneSecBrokerName = BrokerName
                        oData(0).LaneSecBrokerNumber = BrokerNumber
                        oData(0).TrackingState = TrackingInfo.Updated
                        LaneSecData.UpdateRecord(oData(0))
                    End If
                Else
                    Dim oLaneSec As New DTO.LaneSec With {.LaneSecLaneControl = intLaneControl, .LaneSecBrokerName = BrokerName, .LaneSecBrokerNumber = BrokerNumber, .TrackingState = TrackingInfo.Created}
                    LaneSecData.CreateRecord(oLaneSec)
                End If
                Ret = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("saveBrokerData70"))
            End Try
        End Using
        Return Ret
    End Function

    ''' <summary>
    ''' Compares Address1, City, State, Country and Zip returns true if the values for 
    ''' current address and new address do not match.  returns false if the either 
    ''' address objects are nothing.
    ''' </summary>
    ''' <param name="currentAddress"></param>
    ''' <param name="newAddress"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function HasAddressChanged(ByVal currentAddress As LaneIntegrationAddressData, ByVal newAddress As LaneIntegrationAddressData) As Boolean
        If currentAddress Is Nothing Or Not newAddress Is Nothing Then Return False
        Dim blnHaschanged As Boolean = False
        With currentAddress
            If .Address1 <> newAddress.Address1 Then Return True
            If .City <> newAddress.City Then Return True
            If .State <> newAddress.State Then Return True
            If .Country <> newAddress.Country Then Return True
        End With
        Return blnHaschanged
    End Function

    ''' <summary>
    ''' looks up the address information from the company table using the key data fields 
    ''' CompNumber, CompAlphaCode and LegalEntity where possible.  Optionally compares 
    ''' current address with updated address information and updates the HasAddressChanged 
    ''' flag in the updated address information.  If the currentAddress parameter is nothing
    ''' the HasAddressChanged flag is always false.
    ''' </summary>
    ''' <param name="inboundAddress"></param>
    ''' <param name="currentAddress"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LookupLaneAddressInformation(ByVal inboundAddress As LaneIntegrationAddressData, Optional ByVal currentAddress As LaneIntegrationAddressData = Nothing) As LaneIntegrationAddressData
        Dim updatedAddress As LaneIntegrationAddressData = inboundAddress.Clone()
        Dim intCompNumber As Integer = 0
        Dim blnExists As Boolean = False
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                If Not String.IsNullOrEmpty(inboundAddress.CompNumber) AndAlso Integer.TryParse(inboundAddress.CompNumber, intCompNumber) AndAlso intCompNumber <> 0 Then
                    'get the address information from the comp table using the company number
                    blnExists = db.CompRefLanes.Any(Function(x) x.CompNumber = intCompNumber)
                    If blnExists Then
                        updatedAddress = (From d In db.CompRefLanes
                                          Where d.CompNumber = intCompNumber
                                          Select New LaneIntegrationAddressData _
                                          With {.CompControl = d.CompControl,
                                                .CompName = d.CompName,
                                                .Address1 = d.CompStreetAddress1,
                                                .Address2 = d.CompStreetAddress2,
                                                .Address3 = d.CompStreetAddress3,
                                                .City = d.CompStreetCity,
                                                .State = d.CompStreetState,
                                                .Zip = d.CompStreetZip,
                                                .Country = d.CompStreetCountry}).FirstOrDefault()

                    End If
                Else
                    'see if the alpha code and legal entity are valid
                    If (Not String.IsNullOrEmpty(inboundAddress.CompAlphaCode) AndAlso inboundAddress.CompAlphaCode.Trim.Length > 0) AndAlso (Not String.IsNullOrEmpty(inboundAddress.LegalEntity) AndAlso inboundAddress.LegalEntity.Trim.Length > 0) Then
                        blnExists = db.CompRefLanes.Any(Function(x) x.CompLegalEntity = inboundAddress.LegalEntity And x.CompAlphaCode = inboundAddress.CompAlphaCode)
                        If blnExists Then
                            updatedAddress = (From d In db.CompRefLanes
                                              Where d.CompLegalEntity = inboundAddress.LegalEntity And d.CompAlphaCode = inboundAddress.CompAlphaCode
                                              Select New LaneIntegrationAddressData _
                                          With {.CompControl = d.CompControl,
                                                .CompName = d.CompName,
                                                .Address1 = d.CompStreetAddress1,
                                                .Address2 = d.CompStreetAddress2,
                                                .Address3 = d.CompStreetAddress3,
                                                .City = d.CompStreetCity,
                                                .State = d.CompStreetState,
                                                .Zip = d.CompStreetZip,
                                                .Country = d.CompStreetCountry}).FirstOrDefault()
                        End If
                    End If
                End If
                If updatedAddress.CompControl <> 0 Then
                    'look up the contact information
                    Dim CompContactData As NGLCompContData = NDPBaseClassFactory("NGLCompContData")
                    If Not CompContactData Is Nothing Then
                        Dim oContact As DTO.CompCont = CompContactData.GetFirstCompContFiltered(updatedAddress.CompControl)
                        If Not oContact Is Nothing AndAlso oContact.CompContControl <> 0 Then
                            updatedAddress.ContactPhone = oContact.CompContPhone
                            updatedAddress.ContactFax = oContact.CompContFax
                            updatedAddress.ContactPhoneExt = oContact.CompContPhoneExt
                            updatedAddress.ContactEmail = oContact.CompContEmail
                        End If
                    End If
                End If
                If Not currentAddress Is Nothing Then
                    updatedAddress.HasAddressChanged = HasAddressChanged(currentAddress, updatedAddress)
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("LookupLaneAddressInformation"))
            End Try
        End Using
        'provide functionality to return a boolean flag as part of the return object that indicates if the address has changed
        ' this flag will be used to recalculate the latLong if necessary
        Return updatedAddress

    End Function

    Public Function DoesRecordExist(ByVal LaneControl As Integer) As Boolean
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim blnExists = db.Lanes.Any(Function(x) x.LaneControl = LaneControl)
                Return blnExists
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DoesRecordExist"))
            End Try
        End Using
    End Function

    ''' <summary>
    ''' Deletes a Lane record by control number without validation. It can only be called by system only process.
    ''' </summary>
    ''' <param name="ControlNumber"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal ControlNumber As Integer)
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Dim nObject = db.Lanes.Where(Function(x) x.LaneControl = ControlNumber).FirstOrDefault()
            If Not nObject Is Nothing AndAlso nObject.LaneControl <> 0 Then
                db.Lanes.DeleteOnSubmit(nObject)
                Try
                    db.SubmitChanges()
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("SystemDelete"), db)
                End Try
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Validates the lane name and number then checks if a matching lane record exists based on the parameters provided.
    ''' If the validation fails the function returns false and updates the ValidationMsg with details about the failure.
    ''' </summary>
    ''' <param name="LaneCompControl"></param>
    ''' <param name="LaneCompNumber"></param>
    ''' <param name="LaneNumber"></param>
    ''' <param name="LaneName"></param>
    ''' <param name="LaneLegalEntity"></param>
    ''' <param name="LaneCompAlphaCode"></param>
    ''' <param name="ValidationMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''  Modified by RHR for v-8.2 on 04/04/2019
    '''     Added logic for backward compatibility when multi-tennant is not required
    '''     we now support the ability to check the global parameter GlobalAllowBlankLegalEntityForIntegration
    '''     When GlobalAllowBlankLegalEntityForIntegration parameter = 1 we can get the company control 
    '''     using only the LaneCompAlphaCode and LaneLegalEntity can be blank
    ''' </remarks>
    Public Function ValidateLaneBeforeInsert(ByRef LaneCompControl As Integer, ByVal LaneCompNumber As String, ByVal LaneNumber As String, ByVal LaneName As String, ByVal LaneLegalEntity As String, ByVal LaneCompAlphaCode As String, ByRef ValidationMsg As String) As Boolean
        Dim blnRet As Boolean = True
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'Validate that required fields have been entered
                'Verify LaneName is not empty and is unique
                If String.IsNullOrEmpty(ValidationMsg) Then ValidationMsg = ""
                Dim strSpacer As String = ""
                If (String.IsNullOrEmpty(LaneName) OrElse LaneName.Trim.Length < 1) AndAlso db.Lanes.Any(Function(x) x.LaneName = LaneName) Then
                    blnRet = False
                    ValidationMsg &= strSpacer & "the Lane Name is not valid"
                    strSpacer = ", and "
                End If
                'Verify LaneNumber is not empty and is unique
                If (String.IsNullOrEmpty(LaneNumber) OrElse LaneNumber = "0") AndAlso db.Lanes.Any(Function(x) x.LaneNumber = LaneNumber) Then
                    blnRet = False
                    ValidationMsg &= strSpacer & "the Lane Number is not valid"
                    strSpacer = ", and "
                End If

                Dim blnExists As Boolean = False
                '  Modified by RHR for v-8.2 on 04/04/2019
                Dim dblGlobalAllowBlankLegalEntityForIntegration As Double = GetParValue("GlobalAllowBlankLegalEntityForIntegration", 0)
                If blnRet Then
                    If (String.IsNullOrEmpty(LaneCompNumber) OrElse LaneCompNumber = 0) Then
                        If Not String.IsNullOrEmpty(LaneCompAlphaCode) AndAlso LaneCompAlphaCode.Trim.Length > 0 Then
                            '  Modified by RHR for v-8.2 on 04/04/2019
                            If dblGlobalAllowBlankLegalEntityForIntegration = 1 Then
                                LaneCompControl = GetLaneCompControl(LaneCompAlphaCode)
                            Else
                                If Not String.IsNullOrEmpty(LaneLegalEntity) AndAlso LaneLegalEntity.Trim.Length > 0 Then
                                    LaneCompControl = GetLaneCompControl(LaneLegalEntity, LaneCompAlphaCode)
                                Else
                                    'LaneLEgalEntity is required if LaneCompNumber is empty or 0
                                    blnRet = False
                                    ValidationMsg &= strSpacer & "the Lane Legal Entity cannot be blank when LaneCompNumber = 0"
                                    strSpacer = ", and "
                                End If
                            End If

                        Else
                            'LaneCompAlphaCode is required if LaneCompNumber is empty or 0
                            ValidationMsg &= strSpacer & "the Company Alpha Code cannot be blank when LaneCompNumber = 0"
                            strSpacer = ", and "
                            blnRet = False
                        End If
                    Else
                        Try
                            LaneCompControl = (From c In db.CompRefLanes
                                               Where c.CompNumber = LaneCompNumber
                                               Select c.CompControl).FirstOrDefault
                        Catch ex As Exception
                            ValidationMsg &= strSpacer & "Could not read the company data using LaneCompNumber: " & LaneCompNumber.ToString()
                            strSpacer = ", and "
                            blnRet = False
                        End Try
                    End If
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ValidateLaneBeforeInsert"))
            End Try
        End Using
        Return blnRet
    End Function

    'Removed by RHR 2/26/14 no longer used
    ' ''' <summary>
    ' ''' Validates the lane name and lane number then checks if a matching lane record exists for a lane other than the current LaneControl lane
    ' ''' based on the parameters provided; if the validation fails the function returns 
    ' ''' false and updates the ValidationMsg with details about the failure.
    ' ''' Look up the CompControl and validate that is has not changed.
    ' ''' </summary>
    ' ''' <param name="LaneCompControl"></param>
    ' ''' <param name="LaneCompNumber"></param>
    ' ''' <param name="LaneNumber"></param>
    ' ''' <param name="LaneName"></param>
    ' ''' <param name="LaneLegalEntity"></param>
    ' ''' <param name="LaneCompAlphaCode"></param>
    ' ''' <param name="ValidationMsg"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function ValidateLaneBeforeUpdate(ByRef LaneCompControl As Integer, ByVal LaneCompNumber As String, ByVal LaneNumber As String, ByVal LaneName As String, ByVal LaneLegalEntity As String, ByVal LaneCompAlphaCode As String, ByRef ValidationMsg As String) As Boolean
    '    Dim blnRet As Boolean = True
    '    Using db As New NGLMASLaneDataContext(ConnectionString)
    '        Try
    '            'Validate that required fields have been entered
    '            If String.IsNullOrEmpty(ValidationMsg) Then ValidationMsg = ""
    '            Dim strSpacer As String = ""
    '            'Validate LaneName
    '            If String.IsNullOrEmpty(LaneName) OrElse LaneName.Trim.Length < 1 Then
    '                blnRet = False
    '                ValidationMsg &= strSpacer & "the Lane Name is not valid"
    '                strSpacer = ", and "
    '            End If
    '            'Validate LaneNumber
    '            If LaneNumber = 0 Then
    '                blnRet = False
    '                ValidationMsg &= strSpacer & "the Lane Number is not valid"
    '                strSpacer = ", and "
    '            End If
    '            'Look up the LaneCompControl and validate that is hasn't changed
    '            Dim LaneCompControlQ As Integer
    '            Try
    '                LaneCompControlQ = (From c In db.CompRefLanes _
    '                                Where c.CompNumber = LaneCompNumber _
    '                                Select c.CompControl).FirstOrDefault
    '            Catch ex As Exception
    '                'I don't know what to put here
    '                'ManageLinqDataExceptions(ex, buildProcedureName("ValidateLaneBeforeUpdate"))
    '            End Try
    '            If LaneCompControlQ <> LaneCompControl Then
    '                blnRet = False
    '                ValidationMsg &= strSpacer & "the Lane Comp Control is not valid"
    '                strSpacer = ", and "
    '            End If
    '            'LaneCompAlphaCode Error
    '            Dim blnExists As Boolean = False
    '            Dim intLaneCompControl As Integer = LaneCompControl 'Can't use ByRef parameter in Lambda Expression
    '            If blnRet Then
    '                'If Not String.IsNullOrEmpty(LaneCompAlphaCode) AndAlso LaneCompAlphaCode.Trim.Length > 0 Then
    '                '    If Not String.IsNullOrEmpty(LaneLegalEntity) AndAlso LaneLegalEntity.Trim.Length > 0 Then
    '                '        'it is possible for the CarrierAlphaCode to duplciate across Legal Entities
    '                '        blnExists = db.Lanes.Any(Function(x) x.LaneCompControl <> intLaneCompControl And x.LaneName = LaneName And x.LaneNumber = LaneNumber And x.LaneCompAlphaCode = LaneCompAlphaCode And x.LaneLegalEntity = LaneLegalEntity)
    '                '    Else
    '                '        blnExists = db.Lanes.Any(Function(x) x.LaneCompControl <> intLaneCompControl And x.LaneName = LaneName And x.LaneNumber = LaneNumber And x.LaneCompAlphaCode = LaneCompAlphaCode)
    '                '    End If
    '                'Else
    '                '    blnExists = db.Lanes.Any(Function(x) x.LaneCompControl <> intLaneCompControl And x.LaneName = LaneName And x.LaneNumber = LaneNumber)
    '                'End If
    '            End If

    '            If blnExists Then
    '                blnRet = False
    '                ValidationMsg &= strSpacer & "the Lane record already exists"
    '                strSpacer = ", and "
    '            End If
    '        Catch ex As Exception
    '            ManageLinqDataExceptions(ex, buildProcedureName("ValidateCarrierBeforeUpdate"))
    '        End Try
    '    End Using
    '    Return blnRet
    'End Function


    Public Function AddBadAddress(ByVal bookControl As Integer,
                                  ByVal bLALaneControl As Integer,
                                  ByVal bLALaneOrigAddress1 As String,
                                  ByVal bLALaneOrigCity As String,
                                  ByVal bLALaneOrigState As String,
                                  ByVal bLALaneOrigZip As String,
                                  ByVal bLALaneOrigCountry As String,
                                  ByVal bLALaneDestAddress1 As String,
                                  ByVal bLALaneDestCity As String,
                                  ByVal bLALaneDestState As String,
                                  ByVal bLALaneDestZip As String,
                                  ByVal bLALaneDestCountry As String,
                                  ByVal bLAPCMilerOrigAddress1 As String,
                                  ByVal bLAPCMilerOrigCity As String,
                                  ByVal bLAPCMilerOrigState As String,
                                  ByVal bLAPCMilerOrigZip As String,
                                  ByVal bLAPCMilerOrigCountry As String,
                                  ByVal bLAPCMilerDestAddress1 As String,
                                  ByVal bLAPCMilerDestCity As String,
                                  ByVal bLAPCMilerDestState As String,
                                  ByVal bLAPCMilerDestZip As String,
                                  ByVal bLAPCMilerDestCountry As String,
                                  ByVal bLAMessage As String,
                                  ByVal batchID As Double) As Integer
        Dim intRet As Integer = 0
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oRet = db.spAddBadAddress50(bookControl,
                                   bLALaneControl,
                                   Left(bLALaneOrigAddress1, 40),
                                   Left(bLALaneOrigCity, 25),
                                   Left(bLALaneOrigState, 8),
                                   Left(bLALaneOrigZip, 20), 'Modified by RHR for v-8.4.003 on 06/25/2021
                                   Left(bLALaneOrigCountry, 30),
                                   Left(bLALaneDestAddress1, 40),
                                   Left(bLALaneDestCity, 25),
                                   Left(bLALaneDestState, 2),
                                   Left(bLALaneDestZip, 20), 'Modified by RHR for v-8.4.003 on 06/25/2021
                                   Left(bLALaneDestCountry, 30),
                                   Left(bLAPCMilerOrigAddress1, 40),
                                   Left(bLAPCMilerOrigCity, 25),
                                   Left(bLAPCMilerOrigState, 8),
                                   Left(bLAPCMilerOrigZip, 20), 'Modified by RHR for v-8.4.003 on 06/25/2021
                                   Left(bLAPCMilerOrigCountry, 40),
                                   Left(bLAPCMilerDestAddress1, 40),
                                   Left(bLAPCMilerDestCity, 25),
                                   Left(bLAPCMilerDestState, 2),
                                   Left(bLAPCMilerDestZip, 20), 'Modified by RHR for v-8.4.003 on 06/25/2021
                                   Left(bLAPCMilerDestCountry, 40),
                                   Left(bLAMessage, 1000),
                                   batchID,
                                   Me.Parameters.UserName)

                Dim oBaddAddress As New List(Of LTS.spAddBadAddress50Result)(oRet)

                If oBaddAddress.Count > 0 AndAlso oBaddAddress(0).BLAControl.HasValue Then
                    intRet = oBaddAddress(0).BLAControl.Value
                Else
                    intRet = 0
                End If


                Return intRet

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return intRet

        End Using


    End Function

    ''' <summary>
    ''' This method has been deprecieated and is no longer used
    ''' </summary>
    ''' <param name="LaneControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarriersByCostByLane(ByVal LaneControl As Integer) As DTO.CarriersByCostByLane()

        throwDepreciatedException(buildProcedureName("GetCarriersByCostByLane"))
        Return Nothing

    End Function

    'Modified By LVV on 10/27/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    'Depreciated. Now use below method AddLanePreferredCarrier()
    'Public Sub AddLaneCarrierRestriction(ByVal LaneControl As Integer, ByVal CarrierControl As Integer)
    '    Dim strProcName As String = "dbo.spAddLaneCarrierRestriction"
    '    Dim oCmd As New System.Data.SqlClient.SqlCommand
    '    oCmd.Parameters.AddWithValue("@LaneControl", LaneControl)
    '    oCmd.Parameters.AddWithValue("@CarrierControl ", CarrierControl)
    '    oCmd.Parameters.AddWithValue("@UserName", Me.Parameters.UserName)
    '    runNGLStoredProcedure(oCmd, strProcName, 2)
    'End Sub

    Public Sub RemoveLaneCarrierRestriction(ByVal LLTCControl As Integer)
        Dim strProcName As String = "dbo.spRemoveLaneCarrierRestriction"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@LLTCControl", LLTCControl)
        oCmd.Parameters.AddWithValue("@UserName", Me.Parameters.UserName)
        runNGLStoredProcedure(oCmd, strProcName, 2)
    End Sub

    Public Function DoesLaneExist(ByVal LaneNumber As String) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Get the newest record that matches the provided criteria
                Dim Lane As DTO.Lane = (
                    From t In db.Lanes
                    Where
                        (t.LaneNumber = LaneNumber)
                    Select New DTO.Lane With {.LaneControl = t.LaneControl}).First


                If Not Lane Is Nothing OrElse Lane.LaneControl <> 0 Then blnRet = True

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                'this indicates that no records exist so do nothing
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return blnRet

        End Using
    End Function


    ''' <summary>
    ''' Returns -1 on error, 0 if lane does not exist or lanecontrol if record exists
    ''' </summary>
    ''' <param name="LaneNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLaneControlIfExist(ByVal LaneNumber As String) As Integer
        Dim intRet As Integer = -1
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Get the control number
                Dim Control = (From t In db.Lanes
                               Where (t.LaneNumber = LaneNumber)
                               Select t.LaneControl).First

                Return Control

            Catch ex As System.Data.SqlClient.SqlException
                Return -1
            Catch ex As InvalidOperationException
                'this indicates that no records exist so return 0
                Return 0
            Catch ex As Exception
                Throw
            End Try

            Return intRet

        End Using
    End Function

    Public Function GetLaneProfileSettings(ByVal LaneControl As Integer,
                                           ByVal Selected As Boolean?) _
                                       As DTO.LaneProfileSettings()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oReturnData =
                    (From d In db.spGetLaneProfileSettings(LaneControl, Selected)
                     Select New DTO.LaneProfileSettings With
                            {.LaneProfileSettingsAccessorialCode =
                                If(d.AccessorialCode.HasValue, d.AccessorialCode.Value, 0),
                             .LaneProfileSettingsAccessorialName = d.AccessorialName,
                             .LaneProfileSettingsLaneControl =
                                    If(d.LaneControl.HasValue, d.LaneControl.Value, 0),
                             .LaneProfileSettingsLaneName = d.LaneName,
                             .LaneProfileSettingsLaneNumber = d.LaneNumber,
                             .LaneProfileSettingsSelected = d.Selected}).ToArray()
                Return oReturnData
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneProfileSettings "))
            End Try
        End Using
        Return Nothing
    End Function

    Public Sub UpdateLaneProfileXref(ByVal LaneControl As Integer, ByVal AccessorialCode As Integer, ByVal Selected As Nullable(Of Boolean), ByVal UserName As String)
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oRet = (From d In db.spUpdateLaneProfileXref(LaneControl, AccessorialCode, Selected, UserName)
                            Select New DTO.GenericResults With {.ErrNumber = d.ErrNumber,
                                                                .RetMsg = d.RetMsg}).FirstOrDefault
                If Not oRet Is Nothing AndAlso oRet.ErrNumber <> 0 Then
                    throwSQLFaultException(oRet.RetMsg)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateLaneProfileXref"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Is Transload: the new LaneIsTransLoad  flag must be checked (true) 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isTransLoadOn(ByVal LaneControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                blnRet = db.Lanes.Where(Function(x) x.LaneControl = LaneControl).Select(Function(x) x.LaneIsTransLoad).FirstOrDefault()
            Catch ex As Exception
                'ignore all errors just return false if it fails
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Returns the Mode Type for Lanes where TransLoad is active or zero
    ''' </summary>
    ''' <param name="LaneControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.003 on 10/16/2023
    '''     looks up the LaneModeTypeControl when LaneIsTransLoad is true
    '''     called by processSelectedTransLoadFacility
    ''' </remarks>
    Public Function getLaneModeTypeControlforActiveTransLoad(ByVal LaneControl As Integer) As Integer
        Dim iRet As Integer = 0
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                iRet = db.Lanes.Where(Function(x) x.LaneControl = LaneControl AndAlso x.LaneIsTransLoad = True).Select(Function(x) x.LaneModeTypeControl).FirstOrDefault()
            Catch ex As Exception
                'ignore all errors just return 0 if it fails
            End Try
        End Using
        Return iRet
    End Function

    Public Function isTransLoadOn(ByVal LaneNumber As String) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                blnRet = db.Lanes.Where(Function(x) x.LaneNumber = LaneNumber).Select(Function(x) x.LaneIsTransLoad).FirstOrDefault()
            Catch ex As Exception
                'ignore all errors just return false if it fails
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Returns the Commodity ID from the Commodity table using the string CommCode
    ''' If a match is not found returns 3 by default which typically maps to Dry.
    ''' </summary>
    ''' <param name="CommCode"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' follows standard design patterns and throws SQLFaultExceptions on error
    ''' Created 1/09/2016 by RHR v-7.0.5
    ''' </remarks>
    Public Function getLaneTempTypeFromCommCode(ByVal CommCode As String) As Integer
        Dim intRet As Integer = 3 'default is Dry
        Try
            Dim oCommCode As NGLCommodityCodeData = Me.NDPBaseClassFactory("NGLCommodityCodeData")
            Dim oData = oCommCode.GetCommodityCodeFiltered(CommCode)
            If Not oData Is Nothing AndAlso oData.ID <> 0 Then
                intRet = oData.ID
            End If
        Catch ex As Exception
            ManageLinqDataExceptions(ex, "getLaneTempTypeFromCommCode")
        End Try
        Return intRet
    End Function

    'Added By LVV on 9/15/16 for v-7.0.5.110 HDM Enhancement
    Public Function HasLaneAddressInfoChanged(ByVal LaneControl As Integer,
                                              ByVal LaneOrigCity As String,
                                              ByVal LaneOrigState As String,
                                              ByVal LaneOrigCountry As String,
                                              ByVal LaneOrigZip As String,
                                              ByVal LaneDestCity As String,
                                              ByVal LaneDestState As String,
                                              ByVal LaneDestCountry As String,
                                              ByVal LaneDestZip As String) As Boolean
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oRet = (From d In db.spHasLaneAddressInfoChanged(LaneControl, LaneOrigCity, LaneOrigState, LaneOrigCountry, LaneOrigZip, LaneDestCity, LaneDestState, LaneDestCountry, LaneDestZip)).FirstOrDefault()
                If Not oRet Is Nothing AndAlso Not oRet.Column1 Is Nothing Then
                    Return oRet.Column1
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("HasLaneAddressInfoChanged"))
            End Try
        End Using
    End Function

    'Added By LVV on 9/15/16 for v-7.0.5.110 HDM Enhancement
    Public Sub UpdateLaneHDMFees(ByVal LaneControl As Integer)
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim UserName As String = Me.Parameters.UserName
                Dim oRet = (From d In db.spUpdateLaneHDMFees(LaneControl, UserName)
                            Select New DTO.GenericResults With {.ErrNumber = d.ErrNumber,
                                                                .RetMsg = d.RetMsg}).FirstOrDefault
                If Not oRet Is Nothing AndAlso oRet.ErrNumber <> 0 Then
                    throwSQLFaultException(oRet.RetMsg)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateLaneHDMFees"))
            End Try
        End Using
    End Sub

    'Added By LVV on 10/27/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    Public Function LookupLaneActiveCarriers(ByVal LaneControl As Integer) As DTO.CarriersByCost()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim results As New List(Of DTO.CarriersByCost)

                Dim oRet = (From d In db.spLookupLaneActiveCarriers(LaneControl)).ToList()

                If oRet Is Nothing OrElse oRet.Count < 1 Then Return Nothing

                If Not oRet(0) Is Nothing AndAlso oRet(0).ErrNumber <> 0 Then
                    throwSQLFaultException(oRet(0).RetMsg)
                    Return Nothing
                End If

                For Each r In oRet
                    Dim c As New DTO.CarriersByCost
                    With c
                        .CarrierNumber = r.CarrierNumber
                        .CarrierName = r.CarrierName
                        .CarrierControl = r.CarrierControl
                        .BookModeTypeControl = r.ModeTypeControl
                        .ModeTypeName = r.ModeTypeName
                        .CarrTarTempType = r.TempTypeControl
                        .TempTypeName = r.TempTypeName
                        .BookCarrTarControl = r.TariffControl
                        .BookCarrTarName = r.TariffName
                        .BookCarrTarEquipName = r.TariffEquip
                        .CarrierIgnoreTariff = r.CarrierIgnoreTariff
                    End With
                    results.Add(c)
                Next

                Return results.ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("LookupLaneActiveCarriers"))
            End Try
        End Using
        Return Nothing
    End Function

    'Added for Lane Preferred Carrier Pgae todisplay the Available Carriers in the Popup,Mapping new View:vLELaneActiveCarrier,with sp result set:spLookupLaneActiveCarriers '
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="LaneControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.003 on 08/02/2021  added mapping to TariffControl
    ''' </remarks>
    Public Function LookupLaneActiveCarriers365(ByVal LaneControl As Integer) As LTS.vLELaneActiveCarrier()

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim results As New List(Of LTS.vLELaneActiveCarrier)
                Dim oRet1 = (From d In db.spLookupLaneActiveCarriers(LaneControl)).ToList()

                If oRet1 Is Nothing OrElse oRet1.Count < 1 Then Return Nothing

                If Not oRet1(0) Is Nothing AndAlso oRet1(0).ErrNumber <> 0 Then
                    throwSQLFaultException(oRet1(0).RetMsg)
                    Return Nothing
                End If

                For Each r In oRet1
                    Dim c As New LTS.vLELaneActiveCarrier()
                    With c
                        .CarrierNumber = r.CarrierNumber
                        .CarrierName = r.CarrierName
                        .CarrierControl = r.CarrierControl
                        .BookModeTypeControl = r.ModeTypeControl
                        .ModeTypeName = r.ModeTypeName
                        .CarrTarTempType = r.TempTypeControl
                        .TempTypeName = r.TempTypeName
                        .BookCarrTarControl = r.TariffControl
                        .TariffName = r.TariffName
                        .TariffEquip = r.TariffEquip
                        .CarrierIgnoreTariff = r.CarrierIgnoreTariff
                        .LaneControl = LaneControl
                        .TariffControl = r.TariffControl ' Modified by RHR for v-8.4.0.003 on 08/02/2021  added mapping to TariffControl
                    End With
                    results.Add(c)
                Next

                Return results.ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("LookupLaneActiveCarriers365"), db)
            End Try
            Return Nothing
        End Using
    End Function

    Public Function AddSelectedCarriersToRestrictedList(ByVal PreferredCarrier As LTS.LimitLaneToCarrier) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim iLLTCLaneControl = PreferredCarrier.LLTCLaneControl
                Dim iCarControl = PreferredCarrier.LLTCCarrierControl

                If PreferredCarrier.LLTCControl = 0 Then
                    If iLLTCLaneControl = 0 Then
                        Dim sMsg As String = "E_MissingParent" ' "  The reference to the parent record is missing. Please select a valid parent record and try again."
                        ''throwNPreferredCarrierFaultException(sMsg)
                    End If

                End If
                Dim car = GetFirstCarrierContForCarrier(iCarControl)
                'Modified by RHR for v-8.5.1.001 
                If (Not car Is Nothing AndAlso car.CarrierContControl <> 0) Then
                    PreferredCarrier.LLTCCarrierContControl = car.CarrierContControl
                End If
                PreferredCarrier.LLTCModDate = Date.Now()
                PreferredCarrier.LLTCModUser = Me.Parameters.UserName

                If PreferredCarrier.LLTCControl = 0 Then
                    db.LimitLaneToCarriers.InsertOnSubmit(PreferredCarrier)
                Else
                    db.LimitLaneToCarriers.Attach(PreferredCarrier, True)
                End If
                db.SubmitChanges()

                blnRet = True


            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("AddSelectedCarriersToRestrictedList"), db)
            End Try
        End Using

    End Function


    'Added By LVV on 10/28/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    Public Function AddLanePreferredCarrier(ByVal PreferredCarrier As DTO.LimitLaneToCarrier) As Boolean
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Dim blnRet As Boolean = False
            Try
                Dim lpc As New LTS.LimitLaneToCarrier
                CopyDTOToLTS(PreferredCarrier, lpc)

                db.LimitLaneToCarriers.InsertOnSubmit(lpc)
                db.SubmitChanges()
                blnRet = True

                Return blnRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("AddLanePreferredCarrier"))
            End Try

            Return blnRet
        End Using
    End Function


    ''' <summary>
    ''' Returns a DTO Array of Preferred Carriers for the provided Lane Control Number
    ''' </summary>
    ''' <param name="LaneControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 10/28/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    ''' Modified by RHR for v-7.0.6.0 on 11/04/2016
    '''   created a new view vLimitLaneToCarriers that returns  additional values using 
    '''   FK filters related tables to improve performance.
    '''     CompRestrictCarrierSelection 
    '''     CompWarnOnRestrictedCarrierSelection 
    '''     LaneRestrictCarrierSelection 
    '''     LaneWarnOnRestrictedCarrierSelection
    ''' </remarks>
    Public Function GetLanePreferredCarriers(ByVal LaneControl As Integer, Optional ByVal blnFilterActive As Boolean? = Nothing) As DTO.LimitLaneToCarrier()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim PrefCarriers As DTO.LimitLaneToCarrier()

                If blnFilterActive Is Nothing Then
                    'get all records LimitLaneToCarrier table
                    PrefCarriers = (From d In db.vLimitLaneToCarriers
                                    Where d.LLTCLaneControl = LaneControl
                                    Select selectDTOLimitLaneToCarrierData(d)).ToArray()
                Else
                    'get records in LimitLaneToCarrier filtered by Active flag
                    PrefCarriers = (From d In db.vLimitLaneToCarriers
                                    Where d.LLTCLaneControl = LaneControl _
                                    And
                                    d.LLTCSActive = blnFilterActive
                                    Select selectDTOLimitLaneToCarrierData(d)).ToArray()
                End If

                If Not PrefCarriers Is Nothing AndAlso PrefCarriers.Count > 0 Then
                    For Each c In PrefCarriers
                        Dim temp = New DTO.LimitLaneToCarrierDetails
                        With temp
                            .LLTCDControl = c.LLTCControl
                            .ModeTypeControl = c.LLTCModeTypeControl
                            .TempType = c.LLTCTempType
                            .MaxCases = c.LLTCMaxCases
                            .MaxWgt = c.LLTCMaxWgt
                            .MaxPL = c.LLTCMaxPL
                            .MaxCube = c.LLTCMaxCube
                            .MinAllowedCost = c.LLTCMinAllowedCost
                            .MaxAllowedCost = c.LLTCMaxAllowedCost
                        End With
                        c.Details = New DTO.LimitLaneToCarrierDetails() {temp}
                    Next
                End If

                Return PrefCarriers
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLanePreferredCarriers"))
            End Try
            Return Nothing
        End Using
    End Function


    ''' <summary>
    ''' Displays the LimitLaneCarriers based On lane
    ''' </summary>
    ''' <param name="LaneControl">Lane Parent Control</param>
    ''' <param name="RecordCount">No of Recors</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by Manorama for LanePreferred Carrier
    '''  Added to bind Lane preferred carrier when the page loads based on Selected Lane 
    ''' </remarks>
    Public Function GetLanePreferredCarriers365(ByVal LaneControl As Integer, ByRef RecordCount As Integer) As DTO.LimitLaneToCarrier()
        Dim PrefCarriers As DTO.LimitLaneToCarrier()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                PrefCarriers = (From d In db.vLimitLaneToCarriers
                                Where d.LLTCLaneControl = LaneControl
                                Select selectDTOLimitLaneToCarrierData(d)).ToArray()
                If Not PrefCarriers Is Nothing AndAlso PrefCarriers.Count > 0 Then
                    For Each c In PrefCarriers
                        Dim temp = New DTO.LimitLaneToCarrierDetails
                        With temp
                            .LLTCDControl = c.LLTCControl
                            .ModeTypeControl = c.LLTCModeTypeControl
                            .TempType = c.LLTCTempType
                            .MaxCases = c.LLTCMaxCases
                            .MaxWgt = c.LLTCMaxWgt
                            .MaxPL = c.LLTCMaxPL
                            .MaxCube = c.LLTCMaxCube
                            .MinAllowedCost = c.LLTCMinAllowedCost
                            .MaxAllowedCost = c.LLTCMaxAllowedCost
                        End With
                        c.Details = New DTO.LimitLaneToCarrierDetails() {temp}
                    Next
                End If

                Return PrefCarriers
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLanePreferredCarriers365"), db)
            End Try
            Return Nothing
        End Using

    End Function


    ''' <summary>
    '''  Displays the LimitLaneCarriers based on LLTCControl(pk)
    ''' </summary>
    ''' <param name="LLTCControl"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by Manorama for LanePreferred Carrier
    '''  Added for Lane preferred carrier, for edit window popup
    ''' </remarks>
    Public Function GetLanePreferredCarriersByID365(ByVal LLTCControl As Integer, ByRef RecordCount As Integer) As DTO.LimitLaneToCarrier()
        Dim iLanePrefCarControl As Integer = 0
        Dim PrefCarriers As DTO.LimitLaneToCarrier()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                PrefCarriers = (From d In db.vLimitLaneToCarriers
                                Where d.LLTCControl = LLTCControl
                                Select selectDTOLimitLaneToCarrierData(d)).ToArray()
                If Not PrefCarriers Is Nothing AndAlso PrefCarriers.Count > 0 Then
                    For Each c In PrefCarriers
                        Dim temp = New DTO.LimitLaneToCarrierDetails
                        With temp
                            .LLTCDControl = c.LLTCControl
                            .ModeTypeControl = c.LLTCModeTypeControl
                            .TempType = c.LLTCTempType
                            .MaxCases = c.LLTCMaxCases
                            .MaxWgt = c.LLTCMaxWgt
                            .MaxPL = c.LLTCMaxPL
                            .MaxCube = c.LLTCMaxCube
                            .MinAllowedCost = c.LLTCMinAllowedCost
                            .MaxAllowedCost = c.LLTCMaxAllowedCost
                        End With
                        c.Details = New DTO.LimitLaneToCarrierDetails() {temp}
                    Next
                End If

                Return PrefCarriers
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLanePreferredCarriersByID365"), db)
            End Try
            Return PrefCarriers
        End Using

    End Function



    ''' <summary>
    ''' Selects non expired tariffs using the lane and Lane preferred carrier settings.
    ''' </summary>
    ''' <param name="LaneControl"></param>
    ''' <param name="blnFilterActive"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.103 on 02/13/2017
    '''  used to select all non expired tariffs using lane preferred carrier settings
    ''' </remarks>
    Public Function GetLanePreferredCarrTars(ByVal LaneControl As Integer, Optional ByVal blnFilterActive As Boolean? = Nothing) As DTO.LimitLaneToCarrier()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim PrefCarriers As DTO.LimitLaneToCarrier()

                If blnFilterActive Is Nothing Then
                    'get all records LimitLaneToCarrier table
                    PrefCarriers = (From d In db.vLimitLaneToCarrTars
                                    Where d.LLTCLaneControl = LaneControl
                                    Select selectDTOLimitLaneToCarrierData(d)).ToArray()
                Else
                    'get records in LimitLaneToCarrier filtered by Active flag
                    PrefCarriers = (From d In db.vLimitLaneToCarrTars
                                    Where d.LLTCLaneControl = LaneControl _
                                And
                                d.LLTCSActive = blnFilterActive
                                    Select selectDTOLimitLaneToCarrierData(d)).ToArray()
                End If

                If Not PrefCarriers Is Nothing AndAlso PrefCarriers.Count > 0 Then
                    For Each c In PrefCarriers
                        Dim temp = New DTO.LimitLaneToCarrierDetails
                        With temp
                            .LLTCDControl = c.LLTCControl
                            .ModeTypeControl = c.LLTCModeTypeControl
                            .TempType = c.LLTCTempType
                            .MaxCases = c.LLTCMaxCases
                            .MaxWgt = c.LLTCMaxWgt
                            .MaxPL = c.LLTCMaxPL
                            .MaxCube = c.LLTCMaxCube
                            .MinAllowedCost = c.LLTCMinAllowedCost
                            .MaxAllowedCost = c.LLTCMaxAllowedCost
                        End With
                        c.Details = New DTO.LimitLaneToCarrierDetails() {temp}
                    Next
                End If

                Return PrefCarriers
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLanePreferredCarrTars"))
            End Try
            Return Nothing
        End Using
    End Function


    ''' <summary>
    ''' Returns a DTO Array of Active Preferred Carriers where LLTCAllowAutoAssignment is true
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.0 on 11/22/2016
    '''   selects active carriers configured as default
    '''   it does not polulate the LimitLaneToCarrierDetails property
    ''' </remarks>
    Public Function GetFirstCarrierContForCarrier(ByVal CarrierControl As Integer) As DTO.CarrierCont
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CarrierCont As DTO.CarrierCont = (
                From d In db.CarrierConts
                Where
                    (d.CarrierContCarrierControl = CarrierControl)
                Order By d.CarrierContactDefault Descending, d.CarrierContControl Ascending
                Select New DTO.CarrierCont With {.CarrierContControl = d.CarrierContControl,
                                            .CarrierContCarrierControl = d.CarrierContCarrierControl,
                                            .CarrierContName = d.CarrierContName,
                                            .CarrierContTitle = d.CarrierContTitle,
                                            .CarrierContactPhone = d.CarrierContactPhone,
                                            .CarrierContPhoneExt = d.CarrierContPhoneExt,
                                            .CarrierContactFax = d.CarrierContactFax,
                                            .CarrierContact800 = d.CarrierContact800,
                                            .CarrierContactEMail = d.CarrierContactEMail,
                                            .CarrierContactDefault = d.CarrierContactDefault,
                                            .CarrierContLECarControl = d.CarrierContLECarControl,
                                            .CarrierContSchedContact = d.CarrierContSchedContact,
                                            .CarrierContUpdated = d.CarrierContUpdated.ToArray()}).FirstOrDefault()
                Return CarrierCont

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
    ''' Returns a DTO Array of Active Preferred Carriers where LLTCAllowAutoAssignment is true
    ''' </summary>
    ''' <param name="LaneControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.0 on 11/22/2016
    '''   selects active carriers configured as default
    '''   it does not polulate the LimitLaneToCarrierDetails property
    ''' </remarks>
    Public Function GetLaneDefaultCarriers(ByVal LaneControl As Integer) As DTO.LimitLaneToCarrier()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                Dim Carriers As DTO.LimitLaneToCarrier() = (From d In db.vLimitLaneToCarriers
                                                            Where d.LLTCLaneControl = LaneControl _
                                                            And d.LLTCAllowAutoAssignment = True _
                                                            And d.LLTCSActive = True
                                                            Select selectDTOLimitLaneToCarrierData(d)).ToArray()
                Return Carriers
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneDefaultCarriers"))
            End Try

            Return Nothing

        End Using
    End Function

    'Added By LVV on 12/9/16 for v-7.0.5.110 Lane Default Carrier Enhancements 
    Public Function UpdateLanePreferredCarrier(ByVal PreferredCarrier As DTO.LimitLaneToCarrier) As Boolean
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Dim blnRet As Boolean = False
            Dim LaneControl As Integer = PreferredCarrier.LLTCLaneControl
            Try
                ValidateUpdateLLTCRecord(db, PreferredCarrier)

                Dim lpc As New LTS.LimitLaneToCarrier
                CopyDTOToLTS(PreferredCarrier, lpc)
                db.LimitLaneToCarriers.Attach(lpc, True)
                db.SubmitChanges()

                If LaneControl > 0 Then
                    Dim oLaneUpdateResults = db.spUpdateLaneDefaultUsingPreferredCarrier(LaneControl).FirstOrDefault()
                    If Not oLaneUpdateResults Is Nothing AndAlso oLaneUpdateResults.ErrNumber <> 0 Then
                        throwInvalidRequiredKeysException("Lane", oLaneUpdateResults.RetMsg)
                    End If
                End If

                blnRet = True
                Return blnRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateLanePreferredCarrier"), db)
            End Try

            Try
                If LaneControl > 0 Then
                    Dim oLaneUpdateResults = db.spUpdateLaneDefaultUsingPreferredCarrier(LaneControl).FirstOrDefault()
                    If Not oLaneUpdateResults Is Nothing AndAlso oLaneUpdateResults.ErrNumber <> 0 Then
                        throwInvalidRequiredKeysException("Lane", oLaneUpdateResults.RetMsg)
                    End If
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateLanePreferredCarrier_Default"), db)
            End Try

            Return blnRet
        End Using
    End Function
    ''' <summary>
    ''' Method to Update the LimitLaneToCarrier
    ''' </summary>
    ''' <param name="PreferredCarrier">LTS LimitLaneToCarrier</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added for LanePreferred Carrier page ,for update
    ''' Added By ManoRama 
    ''' Modified by RHR for v-8.5.1.002 on 04/09/2022 
    '''     fixed bug where spUpdateLaneDefaultUsingPreferredCarrier was not being called
    ''' </remarks>
    Public Function UpdateLanePreferredCarrier365(ByVal PreferredCarrier As LTS.LimitLaneToCarrier) As Boolean
        Dim blnRet As Boolean = False
        Dim LaneControl As Integer = 0
        If PreferredCarrier Is Nothing Then Return False
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim iLLTCControl = PreferredCarrier.LLTCControl
                LaneControl = PreferredCarrier.LLTCLaneControl

                If PreferredCarrier.LLTCControl = 0 Then
                    If iLLTCControl = 0 Then
                        Dim sMsg As String = "E_MissingParent" ' The reference to the parent record is missing. Please select a valid parent record and try again.
                        ''throwNPreferredCarrierFaultException(sMsg)
                    End If

                End If

                PreferredCarrier.LLTCModDate = Date.Now()
                PreferredCarrier.LLTCModUser = Me.Parameters.UserName

                If PreferredCarrier.LLTCControl = 0 Then
                    db.LimitLaneToCarriers.InsertOnSubmit(PreferredCarrier)
                Else
                    db.LimitLaneToCarriers.Attach(PreferredCarrier, True)
                End If

                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateLanePreferredCarrier365"), db)
            Finally
                If LaneControl > 0 Then
                    Dim oLaneUpdateResults = db.spUpdateLaneDefaultUsingPreferredCarrier(LaneControl).FirstOrDefault()
                    If Not oLaneUpdateResults Is Nothing AndAlso oLaneUpdateResults.ErrNumber <> 0 Then
                        throwInvalidRequiredKeysException("Lane", oLaneUpdateResults.RetMsg)
                    End If
                End If
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Method to Update the LimitLaneToCarrier
    ''' </summary>
    ''' <param name="PreferredCarrier">LTS LimitLaneToCarrier</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added for LanePreferred Carrier page ,for update
    ''' Added By ManoRama 
    ''' Modified by RHR for v-8.5.1.002 on 04/09/2022 
    '''     call LTS method to maintain common code base line 
    ''' </remarks>
    Public Function UpdateLanePreferredCarrier365DTO(ByVal PreferredCarrier As DTO.LimitLaneToCarrier) As Boolean
        Dim blnRet As Boolean = False
        Dim LaneControl As Integer = 0
        If PreferredCarrier Is Nothing Then Return False
        Dim lpc As New LTS.LimitLaneToCarrier
        CopyDTOToLTS(PreferredCarrier, lpc)
        Return UpdateLanePreferredCarrier365(lpc)
    End Function
    Public Function UpdateLanePreferredCarrier_RHRTESTS(ByVal PreferredCarrier As DTO.LimitLaneToCarrier) As Boolean
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Dim blnRet As Boolean = False
            If PreferredCarrier Is Nothing Then Return False

            Dim LaneControl As Integer = PreferredCarrier.LLTCLaneControl
            Try
                ValidateUpdateLLTCRecord(db, PreferredCarrier)
                Dim blnAttachementRequired = False
                Dim lpc As New LTS.LimitLaneToCarrier
                'If PreferredCarrier.LLTCControl <> 0 AndAlso db.LimitLaneToCarriers.Any(Function(x) x.LLTCControl = PreferredCarrier.LLTCControl) Then
                '    lpc = db.LimitLaneToCarriers.Where(Function(x) x.LLTCControl = PreferredCarrier.LLTCControl).FirstOrDefault()
                'End If
                If lpc.LLTCControl = 0 Then blnAttachementRequired = True
                CopyDTOToLTS(PreferredCarrier, lpc)
                If blnAttachementRequired Then db.LimitLaneToCarriers.Attach(lpc, True)
                db.SubmitChanges()

                If LaneControl > 0 Then
                    Dim oLaneUpdateResults = db.spUpdateLaneDefaultUsingPreferredCarrier(LaneControl).FirstOrDefault()
                    If Not oLaneUpdateResults Is Nothing AndAlso oLaneUpdateResults.ErrNumber <> 0 Then
                        throwInvalidRequiredKeysException("Lane", oLaneUpdateResults.RetMsg)
                    End If
                End If

                blnRet = True
                Return blnRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateLanePreferredCarrier"), db)
            End Try

            Return blnRet
        End Using
    End Function

    'Added By LVV on 12/9/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    Public Function UpdateAllLPCActiveFlags(ByVal LaneControl As Integer, blnActive As Boolean) As Boolean
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Dim blnRet As Boolean = False
            Try
                Dim LPCs = (
                    From d In db.LimitLaneToCarriers
                    Where
                    d.LLTCLaneControl = LaneControl).ToArray()

                For Each pc In LPCs
                    If Not pc Is Nothing Then
                        pc.LLTCSActive = blnActive
                        pc.LLTCModDate = Date.Now
                        pc.LLTCModUser = Me.Parameters.UserName
                    End If
                Next

                db.SubmitChanges()
                blnRet = True

                Return blnRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateAllLPCActiveFlags"))
            End Try

            Return blnRet
        End Using
    End Function

    'Added By LVV on 12/9/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    Protected Sub ValidateUpdateLLTCRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.LimitLaneToCarrier)
        'Check if the data already exists only one allowed
        With oData
            Try
                'oDB.Log = New DebugTextWriter
                'Get the newest record that matches the provided criteria
                Dim Carr As DTO.Carrier = (
                From t In CType(oDB, NGLMASLaneDataContext).CarrierRefLanes
                Where
                    (t.CarrierControl = .LLTCCarrierControl) _
                    And
                    (
                        t.CarrierActive.HasValue = False _
                        Or
                        (t.CarrierActive.HasValue AndAlso t.CarrierActive.Value = False)
                   )
                Select New DTO.Carrier With {.CarrierControl = t.CarrierControl}).FirstOrDefault()

                If Not Carr Is Nothing AndAlso Carr.CarrierControl <> 0 Then
                    Dim msg = "Cannot save Preferred Carrier changes. Changes cannot be made to the Preferred Carrier " & .LLTCCarrierName & " if the Master Carrier is Inactive."
                    Utilities.SaveAppError(msg, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = msg}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    'Added By LVV on 11/1/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    Public Function SetPreferredCarriersInactiveByControl(ByVal CarrierControl As Integer, ByVal CarrierName As String, ByVal CarrierNumber As String) As DTO.GenericResults
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'Create a subscription alert
                Dim otblAlertMessageDAL As New NGLtblAlertMessageData(Me.Parameters)
                Dim Subject As String = "Carrier Set Inactive  - " + CarrierNumber + " " + CarrierName
                Dim Body As String = "The Carrier Active Setting has been changed to False for Master Carrier Number " + CarrierNumber + " " + CarrierName + "."
                otblAlertMessageDAL.InsertAlertMessage("AlertMasterCarrierActiveChanged", "Master Carrier Active Setting Has Been Changed", Subject, Body, 0, 0, CarrierControl, 0, "", "", "", "")


                Dim oRet = (From d In db.spSetPreferredCarriersInactive(CarrierControl)
                            Select New DTO.GenericResults With {.ErrNumber = d.ErrNumber,
                                                                .RetMsg = d.RetMsg}).FirstOrDefault

                If Not oRet Is Nothing AndAlso oRet.ErrNumber > 10 Then
                    throwSQLFaultException(oRet.RetMsg)
                    Return Nothing
                End If

                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SetPreferredCarriersInactiveByControl"))
            End Try
            Return Nothing
        End Using
    End Function

    'Added By LVV on 11/2/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    Public Function GetPreferredCarriersForUser(ByVal CarrierControl As Integer) As DTO.PreferredCarrier()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim UserName As String = Me.Parameters.UserName
                Dim oRet = (From d In db.spGetPreferredCarriersForUser(CarrierControl, UserName)).ToList()

                If oRet Is Nothing OrElse oRet.Count < 1 Then Return Nothing

                If oRet(0).ErrNumber > 10 Then
                    throwSQLFaultException(oRet(0).RetMsg)
                    Return Nothing
                End If

                Dim preferredCarriers As New List(Of DTO.PreferredCarrier)

                For Each r In oRet
                    Dim pc As New DTO.PreferredCarrier
                    With pc
                        .blnHasCompRestrictions = r.blnHasCompRestrictions
                        .LLTCControl = r.LLTCControl
                        .LLTCCarrierControl = r.LLTCCarrierControl
                        .LLTCCarrierNumber = r.LLTCCarrierNumber
                        .LLTCCarrierName = r.LLTCCarrierName
                        .LLTCLaneControl = r.LLTCLaneControl
                        .LaneNumber = r.LaneNumber
                        .LaneName = r.LaneName
                        .CompNumber = r.CompNumber
                        .CompName = r.CompName
                        .LLTCModeTypeControl = r.LLTCModeTypeControl
                        .LLTCSequenceNumber = r.LLTCSequenceNumber
                        .LLTCSActive = r.LLTCSActive
                        .LLTCTempType = r.LLTCTempType
                        .LLTCMaxCases = r.LLTCMaxCases
                        .LLTCMaxWgt = r.LLTCMaxWgt
                        .LLTCMaxCube = r.LLTCMaxCube
                        .LLTCMaxPL = r.LLTCMaxPL
                        .LLTCTariffControl = r.LLTCTariffControl
                        .LLTCTariffName = r.LLTCTariffName
                        .LLTCTariffEquip = r.LLTCTariffEquip
                        .LLTCMinAllowedCost = r.LLTCMinAllowedCost
                        .LLTCMaxAllowedCost = r.LLTCMaxAllowedCost
                        .LLTCAllowAutoAssignment = r.LLTCAllowAutoAssignment
                        .LLTCIgnoreTariff = r.LLTCIgnoreTariff
                        .RetMsg = r.RetMsg
                        .ErrNumber = r.ErrNumber
                    End With
                    preferredCarriers.Add(pc)
                Next

                Return preferredCarriers.ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPreferredCarriersForUser"))
            End Try
            Return Nothing
        End Using
    End Function

    'Added By LVV on 11/3/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    Public Function ActivatePreferredCarriers(ByVal strControls As String, ByVal CarrierControl As Integer, ByVal CarrierName As String, ByVal CarrierNumber As String) As DTO.GenericResults
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'Create a subscription alert
                Dim otblAlertMessageDAL As New NGLtblAlertMessageData(Me.Parameters)
                Dim Subject As String = "Carrier Activated - " + CarrierNumber + " " + CarrierName
                Dim Body As String = "The Carrier Active Setting has been changed to True for Master Carrier Number " + CarrierNumber + " " + CarrierName + "."
                otblAlertMessageDAL.InsertAlertMessage("AlertMasterCarrierActiveChanged", "Master Carrier Active Setting Has Been Changed", Subject, Body, 0, 0, CarrierControl, 0, "", "", "", "")


                Dim oRet = (From d In db.spActivatePreferredCarriers(strControls, CarrierControl)
                            Select New DTO.GenericResults With {.ErrNumber = d.ErrNumber,
                                                                .RetMsg = d.RetMsg}).FirstOrDefault

                If Not oRet Is Nothing AndAlso oRet.ErrNumber > 10 Then
                    throwSQLFaultException(oRet.RetMsg)
                    Return Nothing
                End If

                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ActivatePreferredCarriers"))
            End Try
            Return Nothing
        End Using
    End Function

    'Added By LVV on 11/7/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    Public Function GetLLTCLanesByComp(ByVal CompControl As String) As DTO.Lane()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim limit = db.LimitLaneToCarriers.Select(Function(x) x.LLTCLaneControl)

                Dim Lanes As DTO.Lane() = (
                    From d In db.Lanes
                    Where
                        (d.LaneCompControl = CompControl) _
                        And
                        (limit.Contains(d.LaneControl))
                    Select selectDTOData(d)).ToArray()

                Return Lanes

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLLTCLanesByComp"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Sets either the LaneRestrictCarrierSelection Flag to true
    ''' or the LaneWarnOnRestrictedCarrierSelection Flag to true for Lanes filtered by LaneControl.
    ''' If Action = 0 then Restrict
    ''' If Action = 1 then Warn
    ''' The value of Value is what so set the Action Flag to True or False
    ''' </summary>
    ''' <param name="strControls"></param>
    ''' <param name="Action"></param>
    ''' <param name="Value"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 11/7/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    ''' </remarks>
    Public Function RestrictOrWarnLaneNPC(ByVal strControls As String, ByVal Action As Boolean, ByVal Value As Boolean) As DTO.GenericResults
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oRet = (From d In db.spRestrictOrWarnLaneNPC(strControls, Action, Value)
                            Select New DTO.GenericResults With {.ErrNumber = d.ErrNumber,
                                                                .RetMsg = d.RetMsg}).FirstOrDefault

                If Not oRet Is Nothing AndAlso oRet.ErrNumber > 10 Then
                    throwSQLFaultException(oRet.RetMsg)
                    Return Nothing
                End If

                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("RestrictOrWarnLaneNPC"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Sets the LaneRestrictedAtCompLevel Flag to Value for Lanes filtered by CompControl.
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="Value"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 11/9/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    ''' </remarks>
    Public Function UpdateLaneRestrictedAtCompLevel(ByVal CompControl As Integer, ByVal Value As Boolean) As DTO.GenericResults
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oRet = (From d In db.spUpdateLaneRestrictedAtCompLevel(CompControl, Value)
                            Select New DTO.GenericResults With {.ErrNumber = d.ErrNumber,
                                                                .RetMsg = d.RetMsg}).FirstOrDefault

                If Not oRet Is Nothing AndAlso oRet.ErrNumber > 10 Then
                    throwSQLFaultException(oRet.RetMsg)
                    Return Nothing
                End If

                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateLaneRestrictedAtCompLevel"))
            End Try
            Return Nothing
        End Using
    End Function


    ''' <summary>
    ''' uses Reflection via CopyMatchingFields to copy the properties from the LTS object in d to the DTO.LimitLaneToCarrier return data 
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="page"></param>
    ''' <param name="pagecount"></param>
    ''' <param name="recordcount"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 10/28/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    ''' Modified by RHR for v-7.0.6.0 on 11/4/2016
    '''   changed source object d from specific LTS class to object bass class so the 
    '''   code can be executed using the table or the view.  Will work with any object 
    '''   in d that can map to the LimitLaneToCarrier DTO object.
    '''   Also renamed to selectDTOLimitLaneToCarrierData to avoid future conflicts
    '''   with Lane Data objects when/if they are converted to use the selectDTOData
    '''   methods
    ''' </remarks>
    Friend Function selectDTOLimitLaneToCarrierData(ByVal d As Object, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.LimitLaneToCarrier
        Dim oDTO As New DTO.LimitLaneToCarrier
        Dim skipObjs As New List(Of String) From {"Details", "LimitLaneToCarrierUpdated", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .LimitLaneToCarrierUpdated = d.LimitLaneToCarrierUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

#Region "TMS365 Methods"

    ''' <summary>
    ''' Gets a list of all addresses for the lanes associated with the Legal Entity
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="LEAdminControl"></param>
    ''' <param name="filter"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/11/17 for v-8.0 TMS365
    ''' </remarks>
    Public Function GetAddressBookForLE(ByRef RecordCount As Integer,
                                            ByVal LEAdminControl As Integer,
                                            ByVal filter As String,
                                            Optional ByVal page As Integer = 1,
                                            Optional ByVal pagesize As Integer = 1000,
                                            Optional ByVal skip As Integer = 0,
                                            Optional ByVal take As Integer = 0) As Models.AddressBook()
        Dim oRetData As Models.AddressBook()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                Dim intPageCount As Integer = 1

                Dim oQuery = (From d In db.spGetAddressBookForLE(LEAdminControl, filter)
                              Select New Models.AddressBook With {
                                  .Name = d.Name,
                                  .Address1 = d.Address1,
                                  .Address2 = d.Address2,
                                  .Address3 = d.Address3,
                                  .City = d.City,
                                  .State = d.State,
                                  .Country = d.Country,
                                  .Zip = d.Zip}).ToList()

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
                oRetData = oQuery.Skip(skip).Take(pagesize).ToArray()

                Return oRetData

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAddressBookForLE"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Returns secure lane data by user legal entity and selected filters
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4 on 06/02/2021 added new filter logic
    '''     set default filter to active = true
    '''     added user level  by lane via the view
    '''     
    ''' </remarks>
    Public Function GetLELanesFiltered(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vLELane365()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vLELane365
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'Get the data iqueryables   
                Dim iQuery As IQueryable(Of LTS.vLELane365)
                iQuery = db.vLELane365s.Where(Function(x) x.LEAdminControl = filters.LEAdminControl And x.UserSecurityControl = Me.Parameters.UserControl)
                Dim filterWhere = ""
                'set the default filter for active to true if the user has not provided one
                'first see if a filter exists
                If Not filters.FilterValues.Any(Function(x) x.filterName = "LaneActive") Then
                    filterWhere = " (LaneActive = true) "
                End If
                db.Log = New DebugTextWriter
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLELanesFiltered"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetLELane365(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vLELane365()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vLELane365
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vLELane365)
                iQuery = db.vLELane365s
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLELane365"), db)
            End Try
        End Using
        Return Nothing
    End Function
    Public Function GetLELaneSummary(ByVal iLaneControl As Integer) As LTS.vLaneLESummary

        Dim oRet As LTS.vLaneLESummary

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                oRet = db.vLaneLESummaries.Where(Function(x) x.LaneControl = iLaneControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLELaneSummary"), db)
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Does the same functionality as Add() and Update() but without all the DTO overhead
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <param name="LTTransType"></param>
    ''' <returns>LaneControl</returns>
    ''' <remarks>
    ''' Added by LVV on 04/30/2018 for v-8.1 VSTS Task #331 Lane Maint
    ''' Modified By LVV on 5/11/20
    '''  Allow users to set TL capcity settings on create new lane from 365 (LaneTLCases, LaneTLWgt, LaneTLCube, and LaneTLPL)
    ''' Modified by LVV on 5/18/20
    '''  Deprecated udfCreateLaneNumber365 and replaced it with a call to spCreateLaneNumber365
    '''  The sp uses the same logic and business rules as does the Create Booking logic on the LoadBoard page
    ''' Modified By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
    '''  Commented out default values on Insert because we now allow the users to set that data on the screen
    '''  Also fixed error handler so it would return details
    ''' </remarks>
    Public Function InsertOrUpdateLane365(ByVal oData As LTS.vLELane365, ByVal LTTransType As Integer) As Models.ResultObject
        Dim result As New Models.ResultObject
        result.Success = True
        result.SuccessMsg = "Success!"
        result.Control = 0
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim blnHasChanged As Boolean = True
                'Modifie by RHR for v-8.5.4.004 on 01/12/2024 we now let users enter a lane number
                If oData.LaneControl = 0 Then
                    Dim strLaneNumber = oData.LaneNumber
                    Dim refLaneNumber = oData.ReferenceLaneNumber
                    'Modified by RHR for v-8.5.4.004 on 01/26/2024 we now allow 0 for orig and dest comp on inbound or outbound lanes
                    'Modified by Ayman for v-10 on 01/11/2025 LaneNumber now save in the referenceLaneNumber and generate new format LaneNumber
                    'If String.IsNullOrWhiteSpace(oData.LaneNumber) Then
                    Dim iLaneOrigCompControl = oData.LaneOrigCompControl
                    Dim sLaneOrigName = oData.LaneOrigName
                    Dim sLaneOrigAddress1 = oData.LaneOrigAddress1
                    Dim sLaneOrigCity = oData.LaneOrigCity
                    Dim sLaneOrigState = oData.LaneOrigState
                    Dim sLaneOrigZip = oData.LaneOrigZip
                    Dim iLaneDestCompControl = oData.LaneDestCompControl
                    Dim sLaneDestName = oData.LaneDestName
                    Dim sLaneDestAddress1 = oData.LaneDestAddress1
                    Dim sLaneDestCity = oData.LaneDestCity
                    Dim sLaneDestState = oData.LaneDestState
                    Dim sLaneDestZip = oData.LaneDestZip
                    If LTTransType < 1 Then LTTransType = 1
                    If LTTransType = 1 Then
                        If oData.LaneOrigCompControl = 0 Then
                            iLaneOrigCompControl = oData.LaneCompControl
                            sLaneOrigName = ""
                            sLaneOrigAddress1 = ""
                            sLaneOrigCity = ""
                            sLaneOrigState = ""
                            sLaneOrigZip = ""
                        End If
                    ElseIf LTTransType = 2 Then
                        If oData.LaneDestCompControl = 0 Then
                            iLaneDestCompControl = oData.LaneCompControl
                            sLaneDestName = ""
                            sLaneDestAddress1 = ""
                            sLaneDestCity = ""
                            sLaneDestState = ""
                            sLaneDestZip = ""
                        End If
                    End If
                    Dim spRes = db.spCreateLaneNumber365(LTTransType, iLaneOrigCompControl, sLaneOrigName, sLaneOrigAddress1, sLaneOrigCity, sLaneOrigState, sLaneOrigZip, iLaneDestCompControl, sLaneDestName, sLaneDestAddress1, sLaneDestCity, sLaneDestState, sLaneDestZip, Parameters.UserLEControl).FirstOrDefault()
                    strLaneNumber = spRes.LaneNumber
                    Try
                        If spRes.ErrNumber <> 0 Then
                            result.Success = False
                            result.ErrMsg = spRes.RetMsg
                            Utilities.SaveAppError("Cannot save new Lane data. " & spRes.RetMsg, Me.Parameters)
                            Return result
                        End If

                    Catch ex As FaultException
                        Throw
                    Catch ex As InvalidOperationException
                        'do nothing this is the desired result.
                    End Try
                    'End If
                    If db.Lanes.Any(Function(x) x.LaneNumber = strLaneNumber) Then
                        Dim strDetails As String = "Cannot save new Lane data. The Lane Number: " & strLaneNumber & " already exists."
                        Utilities.SaveAppError(strDetails, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                    End If
                    Dim oRecord = SelectLTSData(oData)
                    'set some default values (from spCreateLaneFromLoadTender)
                    oRecord.LaneNumber = strLaneNumber
                    'oRecord.LaneDefaultCarrierUse = False
                    'oRecord.LaneBFC = 100
                    'oRecord.LaneBFCType = "PERC"
                    'oRecord.LaneAutoTenderFlag = False
                    'oRecord.LaneDoNotInvoice = True
                    'oRecord.LaneIsCrossDockFacility = False
                    'oRecord.LaneModeTypeControl = 3
                    'oRecord.LaneIsTransLoad = False
                    'oRecord.LaneAllowInterline = True
                    'oRecord.LaneRouteTypeCode = 6
                    oRecord.ReferenceLaneNumber = refLaneNumber
                    oRecord.LaneModDate = Date.Now
                    oRecord.LaneModUser = Parameters.UserName
                    db.Lanes.InsertOnSubmit(oRecord)
                    db.SubmitChanges()
                    result.Control = oRecord.LaneControl
                Else

                    Dim laneNumber = oData.LaneNumber
                    Dim referenceLaneNumber = oData.ReferenceLaneNumber



                    Try
                        If db.Lanes.Any(Function(x) x.LaneControl <> oData.LaneControl And x.LaneNumber = oData.LaneNumber) Then
                            Dim strDetails As String = "Cannot save Lane changes. The Lane Number " & oData.LaneNumber & " already exists."
                            Utilities.SaveAppError(strDetails, Me.Parameters)
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                        End If
                    Catch ex As FaultException
                        Throw
                    Catch ex As InvalidOperationException
                        'do nothing this is the desired result.
                    End Try
                    Dim oLane = db.Lanes.Where(Function(x) x.LaneControl = oData.LaneControl).FirstOrDefault()
                    If oLane Is Nothing Then
                        Dim strDetails As String = "Cannot save Lane changes. The Lane Control: " & oData.LaneControl & " does not exist."
                        Utilities.SaveAppError(strDetails, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                    End If
                    oLane.LaneNumber = oData.LaneNumber
                    oLane.ReferenceLaneNumber = oData.ReferenceLaneNumber
                    CopyToLTSData(oLane, oData)
                    blnHasChanged = HasLaneAddressInfoChanged(oLane.LaneControl, oLane.LaneOrigCity, oLane.LaneOrigState, oLane.LaneOrigCountry, oLane.LaneOrigZip, oLane.LaneDestCity, oLane.LaneDestState, oLane.LaneDestCountry, oLane.LaneDestZip)
                    oLane.LaneModDate = Date.Now
                    oLane.LaneModUser = Parameters.UserName
                    db.SubmitChanges()
                    result.Control = oLane.LaneControl
                End If
                Try
                    'call sp updatelanehmdfees here
                    If blnHasChanged Then UpdateLaneHDMFees(result.Control)
                Catch ex As Exception
                    'do nothing
                End Try
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateLane365"), db)
            End Try
        End Using
        Return result
    End Function

    Public Sub DeleteLane365(ByVal LaneControl As Integer)
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oRecord As LTS.Lane = db.Lanes.Where(Function(x) x.LaneControl = LaneControl).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.LaneControl = 0) Then Return

                Try
                    Dim oBook As New NGLBookData(Me.Parameters)
                    Dim blnLaneInBook As Boolean = True
                    Try
                        blnLaneInBook = oBook.DoesLaneExistInBook(LaneControl)
                    Catch ex As FaultException
                        If ex.Message <> "E_NoData" Then Throw
                    End Try
                    If (blnLaneInBook) Then
                        Dim strDetails As String = "Cannot delete Lane data.  The Lane Number: " & oRecord.LaneNumber & " is being used and cannot be deleted. Check the booking information."
                        Utilities.SaveAppError(strDetails, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                    End If
                Catch ex As FaultException
                    Throw
                Catch ex As InvalidOperationException
                    'do nothing this is the desired result.
                End Try

                db.Lanes.DeleteOnSubmit(oRecord)
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteLane365"), db)
            End Try
        End Using
    End Sub

    Public Sub SaveLaneMilesLatLong(ByVal LaneControl As Integer, ByVal LaneBenchMiles As Double, ByVal LaneLat As Double, ByVal LaneLong As Double, ByVal blnSaveLatLong As Boolean, Optional ByVal LaneOrigZip As String = Nothing, Optional ByVal LaneDestZip As String = Nothing)
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oLane = db.Lanes.Where(Function(x) x.LaneControl = LaneControl).FirstOrDefault()
                If oLane Is Nothing Then Return
                oLane.LaneBenchMiles = LaneBenchMiles
                If Not String.IsNullOrWhiteSpace(LaneOrigZip) Then oLane.LaneOrigZip = LaneOrigZip
                If Not String.IsNullOrWhiteSpace(LaneDestZip) Then oLane.LaneDestZip = LaneDestZip
                If blnSaveLatLong Then
                    oLane.LaneLatitude = LaneLat
                    oLane.LaneLongitude = LaneLong
                End If
                oLane.LaneModDate = Date.Now
                oLane.LaneModUser = Parameters.UserName
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveLaneMilesLatLong"), db)
            End Try
        End Using
    End Sub


    ''' <summary>
    ''' is Accept/Reject by email on for this carrier and this comp's legal entity
    ''' </summary>
    ''' <param name="iLaneControl"></param>
    ''' <param name="iCompControl"></param>
    ''' <param name="iExpMin"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4 on 05/04/2021
    '''   new function to read Legal Entity Carrier Accept/Reject by Email settings
    ''' </remarks>
    Public Function AllowLaneBookApptTokenByEmail(ByVal iLaneControl As Integer, ByVal iCompControl As Integer, ByRef iExpMin As Integer, ByRef bUseCarrieContEmail As Boolean, ByRef sBookApptEmail As String) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oLane As LTS.Lane = db.Lanes.Where(Function(x) x.LaneControl = iLaneControl).FirstOrDefault()
                If Not oLane Is Nothing AndAlso oLane.LaneControl <> 0 Then
                    blnRet = If(oLane.LaneAllowCarrierBookApptByEmail, False)
                    If blnRet Then
                        iExpMin = CInt(GetParValue("AutoExpireApptTenderTokenMin", iCompControl))
                        bUseCarrieContEmail = If(oLane.LaneUseCarrieContEmailForBookApptByEmail, False)
                        sBookApptEmail = oLane.LaneCarrierBookApptviaTokenEmail
                    End If
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("AllowLaneBookApptTokenByEmail"), db)
            End Try
        End Using
        Return blnRet

    End Function

    Public Function GetLaneTransLeadTimeCalcType(ByVal iLaneControl As Integer) As Integer
        Dim iRet As Integer = 0
        If iLaneControl = 0 Then Return 0
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                iRet = db.Lanes.Where(Function(x) x.LaneControl = iLaneControl).Select(Function(x) x.LaneTransLeadTimeCalcType).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneTransLeadTimeCalcType"), db)
            End Try
        End Using
        Return iRet
    End Function

    Public Function GetLaneTransLeadTimeData(ByVal iLaneControl As Integer) As Models.LaneLeadTimeData
        Dim oData As New Models.LaneLeadTimeData()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                oData = (From x In db.Lanes
                         Where x.LaneControl = iLaneControl
                         Select New Models.LaneLeadTimeData With {.LaneControl = x.LaneControl _
                             , .LaneOLTBenchmark = x.LaneOLTBenchmark _
                             , .LaneTLTBenchmark = x.LaneTLTBenchmark _
                             , .LaneTransLeadTimeCalcType = x.LaneTransLeadTimeCalcType _
                             , .LaneTransLeadTimeUseMasterLane = x.LaneTransLeadTimeUseMasterLane _
                             , .LaneTransLeadTimeLocationOption = x.LaneTransLeadTimeLocationOption}).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneTransLeadTimeData"), db)
            End Try
        End Using
        Return oData
    End Function







#End Region

#End Region

#Region "Protected Methods"

    ''' <summary>
    ''' CopyDTOToLinq
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 11/1/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    ''' Added fields LaneRestrictCarrierSelection, LaneWarnOnRestrictedCarrierSelection, LaneRestrictedAtCompLevel
    ''' Modified by RHR for v-8.4 on 04/22/2021 added new fields for email Token appt bookings
    ''' Modified by RHR for v-8.5.4.005 on 04/04/2024 New Time Zone settings
    ''' </remarks>
    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim t = CType(oData, DTO.Lane)
        'Create New Record
        Return New LTS.Lane With {.LaneControl = t.LaneControl _
                                , .LaneNumber = t.LaneNumber _
                                , .LaneName = t.LaneName _
                                , .LaneNumberMaster = t.LaneNumberMaster _
                                , .LaneNameMaster = t.LaneNameMaster _
                                , .LaneCompControl = t.LaneCompControl _
                                , .LaneDefaultCarrierUse = t.LaneDefaultCarrierUse _
                                , .LaneDefaultCarrierControl = t.LaneDefaultCarrierControl _
                                , .LaneDefaultCarrierContact = t.LaneDefaultCarrierContact _
                                , .LaneDefaultCarrierPhone = t.LaneDefaultCarrierPhone _
                                , .LaneOrigCompControl = t.LaneOrigCompControl _
                                , .LaneOrigName = t.LaneOrigName _
                                , .LaneOrigAddress1 = t.LaneOrigAddress1 _
                                , .LaneOrigAddress2 = t.LaneOrigAddress2 _
                                , .LaneOrigAddress3 = t.LaneOrigAddress3 _
                                , .LaneOrigCity = t.LaneOrigCity _
                                , .LaneOrigState = t.LaneOrigState _
                                , .LaneOrigCountry = t.LaneOrigCountry _
                                , .LaneOrigZip = t.LaneOrigZip _
                                , .LaneOrigContactPhone = t.LaneOrigContactPhone _
                                , .LaneOrigContactPhoneExt = t.LaneOrigContactPhoneExt _
                                , .LaneOrigContactFax = t.LaneOrigContactFax _
                                , .LaneDestCompControl = t.LaneDestCompControl _
                                , .LaneDestName = t.LaneDestName _
                                , .LaneDestAddress1 = t.LaneDestAddress1 _
                                , .LaneDestAddress2 = t.LaneDestAddress2 _
                                , .LaneDestAddress3 = t.LaneDestAddress3 _
                                , .LaneDestCity = t.LaneDestCity _
                                , .LaneDestState = t.LaneDestState _
                                , .LaneDestCountry = t.LaneDestCountry _
                                , .LaneDestZip = t.LaneDestZip _
                                , .LaneDestContactPhone = t.LaneDestContactPhone _
                                , .LaneDestContactPhoneExt = t.LaneDestContactPhoneExt _
                                , .LaneDestContactFax = t.LaneDestContactFax _
                                , .LaneModDate = Date.Now _
                                , .LaneModUser = Parameters.UserName _
                                , .LaneConsigneeNumber = t.LaneConsigneeNumber _
                                , .LaneRecMinIn = t.LaneRecMinIn _
                                , .LaneRecMinUnload = t.LaneRecMinUnload _
                                , .LaneRecMinOut = t.LaneRecMinOut _
                                , .LaneAppt = t.LaneAppt _
                                , .LanePalletExchange = t.LanePalletExchange _
                                , .LanePalletType = t.LanePalletType _
                                , .LaneBenchMiles = t.LaneBenchMiles _
                                , .LaneBFC = t.LaneBFC _
                                , .LaneBFCType = t.LaneBFCType _
                                , .LaneRecHourStart = t.LaneRecHourStart _
                                , .LaneRecHourStop = t.LaneRecHourStop _
                                , .LaneDestHourStart = t.LaneDestHourStart _
                                , .LaneDestHourStop = t.LaneDestHourStop _
                                , .LaneComments = t.LaneComments _
                                , .LaneCommentsConfidential = t.LaneCommentsConfidential _
                                , .LaneCurType = t.LaneCurType _
                                , .LaneActive = t.LaneActive _
                                , .LaneLatitude = t.LaneLatitude _
                                , .LaneLongitude = t.LaneLongitude _
                                , .LaneTempType = t.LaneTempType _
                                , .LaneTransType = t.LaneTransType _
                                , .LanePrimaryBuyer = t.LanePrimaryBuyer _
                                , .LaneOLTBenchmark = t.LaneOLTBenchmark _
                                , .LaneTLTBenchmark = t.LaneTLTBenchmark _
                                , .LaneAptDelivery = t.LaneAptDelivery _
                                , .LaneUpdated = If(t.LaneUpdated Is Nothing, New Byte() {}, t.LaneUpdated) _
                                , .LaneOrderControl = t.LaneOrderControl _
                                , .LaneOrderSTDWgt = t.LaneOrderSTDWgt _
                                , .LaneOrderSTDCases = t.LaneOrderSTDCases _
                                , .LaneOrderSTDCubes = t.LaneOrderSTDCubes _
                                , .LaneOrderSTDPUAllow = t.LaneOrderSTDPUAllow _
                                , .LaneOrderSTDAllowType = t.LaneOrderSTDAllowType _
                                , .LaneOrderSTDAllowValue = t.LaneOrderSTDAllowValue _
                                , .LaneOrderSTDMonthlyOrder = t.LaneOrderSTDMonthlyOrder _
                                , .LaneOrderSTDYearlyFRT = t.LaneOrderSTDYearlyFRT _
                                , .LaneOrderSTDCarrierControl = t.LaneOrderSTDCarrierControl _
                                , .LaneOrderSTDCostMile = t.LaneOrderSTDCostMile _
                                , .LaneOrderSTDCostCWT = t.LaneOrderSTDCostCWT _
                                , .LaneOrderSTDCostFlat = t.LaneOrderSTDCostFlat _
                                , .LaneOrderACTWgt = t.LaneOrderACTWgt _
                                , .LaneOrderACTCases = t.LaneOrderACTCases _
                                , .LaneOrderACTCubes = t.LaneOrderACTCubes _
                                , .LaneOrderACTPUAllow = t.LaneOrderACTPUAllow _
                                , .LaneOrderACTAllowType = t.LaneOrderACTAllowType _
                                , .LaneOrderACTAllowValue = t.LaneOrderACTAllowValue _
                                , .LaneOrderACTMonthlyOrder = t.LaneOrderACTMonthlyOrder _
                                , .LaneOrderACTYearlyFRT = t.LaneOrderACTYearlyFRT _
                                , .LaneOrderACTCarrierControl = t.LaneOrderACTCarrierControl _
                                , .LaneOrderACTCostMile = t.LaneOrderACTCostMile _
                                , .LaneOrderACTCostFlat = t.LaneOrderACTCostFlat _
                                , .LaneOrderACTCostCWT = t.LaneOrderACTCostCWT _
                                , .LaneStops = t.LaneStops _
                                , .LaneFixedTime = t.LaneFixedTime _
                                , .LaneOriginAddressUse = t.LaneOriginAddressUse _
                                , .LaneSDFUse = t.LaneOriginAddressUse _
                                , .LaneSDFSRZone = t.LaneSDFSRZone _
                                , .LaneSDFMRZone = t.LaneSDFMRZone _
                                , .LaneSDFFixedTime = t.LaneSDFFixedTime _
                                , .LaneSDFEarlyTM1 = t.LaneSDFEarlyTM1 _
                                , .LaneSDFLateTM1 = t.LaneSDFLateTM1 _
                                , .LaneSDFDay1 = t.LaneSDFDay1 _
                                , .LaneSDFEarlyTM2 = t.LaneSDFEarlyTM2 _
                                , .LaneSDFLateTM2 = t.LaneSDFLateTM2 _
                                , .LaneSDFDay2 = t.LaneSDFDay2 _
                                , .LaneSDFEarlyTM3 = t.LaneSDFEarlyTM3 _
                                , .LaneSDFLateTM3 = t.LaneSDFLateTM3 _
                                , .LaneSDFDay3 = t.LaneSDFDay3 _
                                , .LaneSDFEarlyTM4 = t.LaneSDFEarlyTM4 _
                                , .LaneSDFLateTM4 = t.LaneSDFLateTM4 _
                                , .LaneSDFDay4 = t.LaneSDFDay4 _
                                , .LaneSDFEarlyTM5 = t.LaneSDFEarlyTM5 _
                                , .LaneSDFLateTM5 = t.LaneSDFLateTM5 _
                                , .LaneSDFDay5 = t.LaneSDFDay5 _
                                , .LaneSDFEarlyTM6 = t.LaneSDFEarlyTM6 _
                                , .LaneSDFLateTM6 = t.LaneSDFLateTM6 _
                                , .LaneSDFDay6 = t.LaneSDFDay6 _
                                , .LaneSDFEarlyTM7 = t.LaneSDFEarlyTM7 _
                                , .LaneSDFLateTM7 = t.LaneSDFEarlyTM7 _
                                , .LaneSDFDay7 = t.LaneSDFDay7 _
                                , .LaneSDFUnldRate1 = t.LaneSDFUnldRate1 _
                                , .LaneSDFUnldRate2 = t.LaneSDFUnldRate2 _
                                , .LaneSDFUnldRate3 = t.LaneSDFUnldRate3 _
                                , .LaneSDFUnldRate4 = t.LaneSDFUnldRate4 _
                                , .LaneSDFUnldRate5 = t.LaneSDFUnldRate5 _
                                , .LaneAutoTenderFlag = t.LaneAutoTenderFlag _
                                , .LaneCascadingDispatchingFlag = t.LaneCascadingDispatchingFlag _
                                , .LanePortofEntry = t.LanePortofEntry _
                                , .LaneDoNotInvoice = t.LaneDoNotInvoice _
                                , .LaneTLCases = t.LaneTLCases _
                                , .LaneTLWgt = t.LaneTLWgt _
                                , .LaneTLCube = t.LaneTLCube _
                                , .LaneTLPL = t.LaneTLPL _
                                , .LaneChepGLID = t.LaneChepGLID _
                                , .LaneCarrierTypeCode = t.LaneCarrierTypeCode _
                                , .LaneCarrierEquipmentCodes = t.LaneCarrierEquipmentCodes _
                                , .LanePickUpMon = t.LanePickUpMon _
                                , .LanePickUpTue = t.LanePickUpTue _
                                , .LanePickUpWed = t.LanePickUpWed _
                                , .LanePickUpThu = t.LanePickUpThu _
                                , .LanePickUpFri = t.LanePickUpFri _
                                , .LanePickUpSat = t.LanePickUpSat _
                                , .LanePickUpSun = t.LanePickUpSun _
                                , .LaneDropOffMon = t.LaneDropOffMon _
                                , .LaneDropOffTue = t.LaneDropOffTue _
                                , .LaneDropOffWed = t.LaneDropOffWed _
                                , .LaneDropOffThu = t.LaneDropOffThu _
                                , .LaneDropOffFri = t.LaneDropOffFri _
                                , .LaneDropOffSat = t.LaneDropOffSat _
                                , .LaneDropOffSun = t.LaneDropOffSun _
                                , .LaneOrigStopControl = t.LaneOrigStopControl _
                                , .LaneDestStopControl = t.LaneDestStopControl _
                                , .LaneRouteTypeCode = t.LaneRouteTypeCode _
                                , .LaneDefaultRouteSequence = t.LaneDefaultRouteSequence _
                                , .LaneRouteGuideControl = t.LaneRouteGuideControl _
                                , .LaneRouteGuideNumber = t.LaneRouteGuideNumber _
                                , .LaneIsCrossDockFacility = t.LaneIsCrossDockFacility _
                                , .LaneRequiredOnTimeServiceLevel = t.LaneRequiredOnTimeServiceLevel _
                                , .LaneCalcOnTimeServiceLevel = t.LaneCalcOnTimeServiceLevel _
                                , .LaneCalcOnTimeNoMonthsUsed = t.LaneCalcOnTimeNoMonthsUsed _
                                , .LaneModeTypeControl = t.LaneModeTypeControl _
                                , .LaneUser1 = t.LaneUser1 _
                                , .LaneUser2 = t.LaneUser2 _
                                , .LaneUser3 = t.LaneUser3 _
                                , .LaneUser4 = t.LaneUser4 _
                                , .LaneIsTransLoad = t.LaneIsTransLoad _
                                , .LaneLegalEntity = t.LaneLegalEntity _
                                , .LaneAllowInterline = t.LaneAllowInterline _
                                , .LaneRestrictCarrierSelection = t.LaneRestrictCarrierSelection _
                                , .LaneWarnOnRestrictedCarrierSelection = t.LaneWarnOnRestrictedCarrierSelection _
                                , .LaneRestrictedAtCompLevel = t.LaneRestrictedAtCompLevel _
                                , .LaneOrigContactName = t.LaneOrigContactName _
                                , .LaneOrigContactEmail = t.LaneOrigContactEmail _
                                , .LaneOrigEmergencyContactPhone = t.LaneOrigEmergencyContactPhone _
                                , .LaneOrigEmergencyContactName = t.LaneOrigEmergencyContactName _
                                , .LaneDestContactName = t.LaneDestContactName _
                                , .LaneDestContactEmail = t.LaneDestContactEmail _
                                , .LaneDestEmergencyContactPhone = t.LaneDestEmergencyContactPhone _
                                , .LaneDestEmergencyContactName = t.LaneDestEmergencyContactName _
                                , .LaneWeightUnit = t.LaneWeightUnit _
                                , .LaneLengthUnit = t.LaneLengthUnit _
                                , .LaneAllowCarrierBookApptByEmail = t.LaneAllowCarrierBookApptByEmail _
                                , .LaneRequireCarrierAuthBookApptByEmail = t.LaneRequireCarrierAuthBookApptByEmail _
                                , .LaneUseCarrieContEmailForBookApptByEmail = t.LaneUseCarrieContEmailForBookApptByEmail _
                                , .LaneCarrierBookApptviaTokenEmail = t.LaneCarrierBookApptviaTokenEmail _
                                , .LaneCarrierBookApptviaTokenFailEmail = t.LaneCarrierBookApptviaTokenFailEmail _
                                , .LaneCarrierBookApptviaTokenFailPhone = t.LaneCarrierBookApptviaTokenFailPhone _
                                , .LaneTransLeadTimeCalcType = t.LaneTransLeadTimeCalcType _
                                , .LaneTransLeadTimeUseMasterLane = t.LaneTransLeadTimeUseMasterLane _
                                , .LaneTransLeadTimeLocationOption = t.LaneTransLeadTimeLocationOption _
                                , .LaneOrigTimeZone = t.LaneOrigTimeZone _
                                , .LaneDestTimeZone = t.LaneDestTimeZone}

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetLaneFiltered(Control:=CType(LinqTable, LTS.Lane).LaneControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim source As LTS.Lane = TryCast(LinqTable, LTS.Lane)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.Lanes
                       Where d.LaneControl = source.LaneControl
                       Select New DTO.QuickSaveResults With {.Control = d.LaneControl _
                                                            , .ModDate = d.LaneModDate _
                                                            , .ModUser = d.LaneModUser _
                                                            , .Updated = d.LaneUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

    ''' <summary>
    ''' Validate that the lane numbere is unique and apply default values from parameters
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="oData"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.005 on 03/26/2024
    '''     check for default parameters and update the DTO data as needed
    '''     parameter values must be active parval = 1 and partext = default
    '''     new parameters are:
    '''     LaneDefaultCarrierUseDefault
    '''     LaneAutoTenderFlagDefault
    '''     LaneCascadingDispatchingFlagDefault
    ''' </remarks>
    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.Lane)
            Try
                Dim Lane As DTO.Lane = (
                    From t In CType(oDB, NGLMASLaneDataContext).Lanes
                    Where
                        (t.LaneNumber = .LaneNumber)
                    Select New DTO.Lane With {.LaneControl = t.LaneControl}).First

                If Not Lane Is Nothing Then
                    Utilities.SaveAppError("Cannot save new Lane data.  The Lane number, " & .LaneNumber & " or the Lane name, " & .LaneName & ",  already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If
                'update the defaults for new lanes
                Dim dblLaneDefaultCarrierUseDefault = GetParValue("LaneDefaultCarrierUseDefault", .LaneCompControl)
                Dim dblLaneAutoTenderFlagDefault = GetParValue("LaneAutoTenderFlagDefault", .LaneCompControl)
                Dim dblLaneCascadingDispatchingFlagDefault = GetParValue("LaneCascadingDispatchingFlagDefault", .LaneCompControl)
                Dim sParText As String = ""
                Dim blnParBit As String = False
                If dblLaneDefaultCarrierUseDefault = 1 Then
                    sParText = GetParText("LaneDefaultCarrierUseDefault", .LaneCompControl)
                    If Boolean.TryParse(sParText, blnParBit) Then
                        .LaneDefaultCarrierUse = blnParBit
                    End If
                End If
                If dblLaneDefaultCarrierUseDefault = 1 Then
                    sParText = GetParText("LaneAutoTenderFlagDefault", .LaneCompControl)
                    If Boolean.TryParse(sParText, blnParBit) Then
                        .LaneAutoTenderFlag = blnParBit
                    End If
                End If
                If dblLaneDefaultCarrierUseDefault = 1 Then
                    sParText = GetParText("LaneCascadingDispatchingFlagDefault", .LaneCompControl)
                    If Boolean.TryParse(sParText, blnParBit) Then
                        .LaneCascadingDispatchingFlag = blnParBit
                    End If
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
        With CType(oData, DTO.Lane)
            Try

                'oDB.Log = New DebugTextWriter
                Dim Lane As DTO.Lane = (
                    From t In CType(oDB, NGLMASLaneDataContext).Lanes
                    Where
                        (t.LaneControl <> .LaneControl) _
                        And
                        (t.LaneNumber = .LaneNumber)
                    Select New DTO.Lane With {.LaneControl = t.LaneControl}).First

                If Not Lane Is Nothing Then
                    Dim strDetails As String = "Cannot save Lane changes.  The Lane number, " & .LaneNumber & " or the Lane name, " & .LaneName & ",  already exist."
                    Utilities.SaveAppError(strDetails, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    'Modified By LVV on 4/30/18 for v-8.1
    'Changed it to call DoesLaneExistInBook because calling GetBooksByLane was unneccessary and slow
    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the Lane is being used by the book data or the lane data
        With CType(oData, DTO.Lane)
            Try
                'Add code here to call the Book data provider when they are created
                Dim dpBook As New NGLBookData(Me.Parameters)
                Dim blnLaneInBook As Boolean = True
                Try
                    blnLaneInBook = dpBook.DoesLaneExistInBook(.LaneControl)
                Catch ex As FaultException
                    If ex.Message <> "E_NoData" Then Throw
                End Try
                If (blnLaneInBook) Then
                    Dim strDetails As String = "Cannot delete Lane data.  The Lane number, " & .LaneNumber & " is being used and cannot be deleted. Check the booking information."
                    Utilities.SaveAppError(strDetails, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                End If
            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub AddDetailsToLinq(ByRef LinqTable As Object, ByRef oData As DTO.DTOBaseClass)
        With CType(LinqTable, LTS.Lane)
            'Add Lane Calendar Records
            .LaneCals.AddRange(
                     From d In CType(oData, DTO.Lane).LaneCals
                     Select New LTS.LaneCal With {.LaneCalControl = d.LaneCalControl _
                                                  , .LaneCalLaneControl = d.LaneCalLaneControl _
                                                  , .LaneCalMonth = d.LaneCalMonth _
                                                  , .LaneCalDay = d.LaneCalDay _
                                                  , .LaneCalOpen = d.LaneCalOpen _
                                                  , .LaneCalStartTime = d.LaneCalStartTime _
                                                  , .LaneCalEndTime = d.LaneCalEndTime _
                                                  , .LaneCalIsHoliday = d.LaneCalIsHoliday _
                                                  , .LaneCalApplyToOrigin = d.LaneCalApplyToOrigin _
                                                  , .LaneCalUpdated = If(d.LaneCalUpdated Is Nothing, New Byte() {}, d.LaneCalUpdated)})

            ''Add Lane Carr Records
            '.LaneCarrs.AddRange( _
            '         From d In CType(oData, DTO.Lane).LaneCarrs _
            '         Select New LTS.LaneCarr With {.LaneCarrControl = d.LaneCarrControl _
            '                                      , .LaneCarrLaneControl = d.LaneCarrLaneControl _
            '                                      , .LaneCarrCarrierControl = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrWgtFrom = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrWgtTo = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrRateStarts = d.LaneCarrRateStarts _
            '                                      , .LaneCarrRateExpires = d.LaneCarrRateExpires _
            '                                      , .LaneCarrTL = d.LaneCarrTL _
            '                                      , .LaneCarrLTL = d.LaneCarrLTL _
            '                                      , .LaneCarrEquipment = d.LaneCarrEquipment _
            '                                      , .LaneCarrMileRate = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrCwtRate = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrCaseRate = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrFlatRate = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrPltRate = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrCubeRate = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrTLT = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrTMode = d.LaneCarrTMode _
            '                                      , .LaneCarrFAK = d.LaneCarrFAK _
            '                                      , .LaneCarrDisc = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrPUMon = d.LaneCarrPUMon _
            '                                      , .LaneCarrPUTue = d.LaneCarrPUTue _
            '                                      , .LaneCarrPUWed = d.LaneCarrPUWed _
            '                                      , .LaneCarrPUThu = d.LaneCarrPUThu _
            '                                      , .LaneCarrPUFri = d.LaneCarrPUFri _
            '                                      , .LaneCarrPUSat = d.LaneCarrPUSat _
            '                                      , .LaneCarrPUSun = d.LaneCarrPUSun _
            '                                      , .LaneCarrDLMon = d.LaneCarrDLMon _
            '                                      , .LaneCarrDLTue = d.LaneCarrDLTue _
            '                                      , .LaneCarrDLWed = d.LaneCarrDLWed _
            '                                      , .LaneCarrDLThu = d.LaneCarrDLThu _
            '                                      , .LaneCarrDLFri = d.LaneCarrDLFri _
            '                                      , .LaneCarrDLSat = d.LaneCarrDLSat _
            '                                      , .LaneCarrDLSun = d.LaneCarrDLSun _
            '                                      , .LaneCarrPayTolPerLo = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrPayTolPerHi = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrPayTolCurLo = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrPayTolCurHi = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrCurType = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrModUser = Parameters.UserName _
            '                                      , .LaneCarrModDate = Date.Now _
            '                                      , .LaneCarrRoute = d.LaneCarrRoute _
            '                                      , .LaneCarrPltsOpen = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrPltsCommitted = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrPltsAvailable = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrMiles = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrBkhlCostPerc = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrPalletCostPer = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrFuelSurChargePerc = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrStopCharge = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrDropCost = d.LaneCarrDropCost _
            '                                      , .LaneCarrUnloadDiff = d.LaneCarrUnloadDiff _
            '                                      , .LaneCarrCasesAvailable = d.LaneCarrCasesAvailable _
            '                                      , .LaneCarrCasesOpen = d.LaneCarrCasesOpen _
            '                                      , .LaneCarrCasesCommitted = d.LaneCarrCasesCommitted _
            '                                      , .LaneCarrWgtAvailable = d.LaneCarrWgtAvailable _
            '                                      , .LaneCarrWgtOpen = d.LaneCarrWgtOpen _
            '                                      , .LaneCarrWgtCommitted = d.LaneCarrWgtCommitted _
            '                                      , .LaneCarrCubesAvailable = d.LaneCarrCubesAvailable _
            '                                      , .LaneCarrCubesOpen = d.LaneCarrCubesOpen _
            '                                      , .LaneCarrCubesCommitted = d.LaneCarrCubesCommitted _
            '                                      , .LaneCarrCarrierTruckControl = d.LaneCarrCarrierControl _
            '                                      , .LaneCarrLockSettings = d.LaneCarrLockSettings _
            '                                      , .LaneCarrUpdated = If(d.LaneCarrUpdated Is Nothing, New Byte() {}, d.LaneCarrUpdated)})
            'Add Lane Fee Records
            .LaneFees.AddRange(
                     From d In CType(oData, DTO.Lane).LaneFees
                     Select New LTS.LaneFee With {.LaneFeesControl = d.LaneFeesControl _
                                      , .LaneFeesLaneControl = d.LaneFeesLaneControl _
                                      , .LaneFeesMinimum = d.LaneFeesMinimum _
                                      , .LaneFeesVariable = d.LaneFeesVariable _
                                      , .LaneFeesAccessorialCode = d.LaneFeesAccessorialCode _
                                      , .LaneFeesModDate = Date.Now _
                                      , .LaneFeesModUser = Parameters.UserName _
                                      , .LaneFeesUpdated = If(d.LaneFeesUpdated Is Nothing, New Byte() {}, d.LaneFeesUpdated)})

            'Add Lane Sec Records
            .LaneSecs.AddRange(
                     From d In CType(oData, DTO.Lane).LaneSecs
                     Select New LTS.LaneSec With {.LaneSecControl = d.LaneSecControl _
                                      , .LaneSecLaneControl = d.LaneSecLaneControl _
                                      , .LaneSecPUName = d.LaneSecPUName _
                                      , .LaneSecPUAddress1 = d.LaneSecPUAddress1 _
                                      , .LaneSecPUAddress2 = d.LaneSecPUAddress2 _
                                      , .LaneSecPUAddress3 = d.LaneSecPUAddress3 _
                                      , .LaneSecPUCity = d.LaneSecPUCity _
                                      , .LaneSecPUState = d.LaneSecPUState _
                                      , .LaneSecPUCountry = d.LaneSecPUCountry _
                                      , .LaneSecPUZip = d.LaneSecPUZip _
                                      , .LaneSecPUContactPhone = d.LaneSecPUContactPhone _
                                      , .LaneSecPUContactFax = d.LaneSecPUContactFax _
                                      , .LaneSecBrokerNumber = d.LaneSecBrokerNumber _
                                      , .LaneSecBrokerName = d.LaneSecBrokerName _
                                      , .LaneSecBrokerAddress1 = d.LaneSecBrokerAddress1 _
                                      , .LaneSecBrokerAddress2 = d.LaneSecBrokerAddress2 _
                                      , .LaneSecBrokerAddress3 = d.LaneSecBrokerAddress3 _
                                      , .LaneSecBrokerCity = d.LaneSecBrokerCity _
                                      , .LaneSecBrokerState = d.LaneSecBrokerState _
                                      , .LaneSecBrokerCountry = d.LaneSecBrokerCountry _
                                      , .LaneSecBrokerZip = d.LaneSecBrokerZip _
                                      , .LaneSecBrokerContactPhone = d.LaneSecBrokerContactPhone _
                                      , .LaneSecBrokerContactFax = d.LaneSecBrokerContactFax _
                                      , .LaneSecBrokerContactName = d.LaneSecBrokerContactName _
                                      , .LaneSecBrokerOpHourStart = d.LaneSecBrokerOpHourStart _
                                      , .LaneSecBrokerOpHourStop = d.LaneSecBrokerOpHourStop _
                                      , .LaneSecComment = d.LaneSecComment _
                                      , .LaneSecModDate = Date.Now _
                                      , .LaneSecModUser = Parameters.UserName _
                                      , .LaneSecUpdated = If(d.LaneSecUpdated Is Nothing, New Byte() {}, d.LaneSecUpdated)})
        End With

    End Sub

    Protected Overrides Sub InsertAllDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef LinqTable As Object)
        With CType(oDB, NGLMASLaneDataContext)
            .LaneCals.InsertAllOnSubmit(CType(LinqTable, LTS.Lane).LaneCals)
            '.LaneCarrs.InsertAllOnSubmit(CType(LinqTable, LTS.Lane).LaneCarrs)
            .LaneFees.InsertAllOnSubmit(CType(LinqTable, LTS.Lane).LaneFees)
            .LaneSecs.InsertAllOnSubmit(CType(LinqTable, LTS.Lane).LaneSecs)
        End With

    End Sub

    Protected Overrides Sub ProcessUpdatedDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        With CType(oDB, NGLMASLaneDataContext)
            ' Process any inserted contact records 
            .LaneCals.InsertAllOnSubmit(GetLaneCalChanges(oData, TrackingInfo.Created))
            ' Process any updated contact records
            .LaneCals.AttachAll(GetLaneCalChanges(oData, TrackingInfo.Updated), True)
            ' Process any deleted contact records
            Dim deletedContDetails = GetLaneCalChanges(oData, TrackingInfo.Deleted)
            .LaneCals.AttachAll(deletedContDetails, True)
            .LaneCals.DeleteAllOnSubmit(deletedContDetails)
            '' Process any inserted LaneCarr records 
            '.LaneCarrs.InsertAllOnSubmit(GetLaneCarrChanges(oData, TrackingInfo.Created))
            '' Process any updated LaneCarr records
            '.LaneCarrs.AttachAll(GetLaneCarrChanges(oData, TrackingInfo.Updated), True)
            '' Process any deleted LaneCarr records
            'Dim deletedLaneCarrDetails = GetLaneCarrChanges(oData, TrackingInfo.Deleted)
            '.LaneCarrs.AttachAll(deletedLaneCarrDetails, True)
            '.LaneCarrs.DeleteAllOnSubmit(deletedLaneCarrDetails)
            ' Process any inserted LaneFee records 
            .LaneFees.InsertAllOnSubmit(GetLaneFeeChanges(oData, TrackingInfo.Created))
            ' Process any updated LaneFee records
            .LaneFees.AttachAll(GetLaneFeeChanges(oData, TrackingInfo.Updated), True)
            ' Process any deleted LaneFee records
            Dim deletedLaneFeeDetails = GetLaneFeeChanges(oData, TrackingInfo.Deleted)
            .LaneFees.AttachAll(deletedLaneFeeDetails, True)
            .LaneFees.DeleteAllOnSubmit(deletedLaneFeeDetails)
            ' Process any inserted LaneSec records 
            .LaneSecs.InsertAllOnSubmit(GetLaneSecChanges(oData, TrackingInfo.Created))
            ' Process any updated LaneSec records
            .LaneSecs.AttachAll(GetLaneSecChanges(oData, TrackingInfo.Updated), True)
            ' Process any deleted LaneSec records
            Dim deletedLaneSecDetails = GetLaneSecChanges(oData, TrackingInfo.Deleted)
            .LaneSecs.AttachAll(deletedLaneSecDetails, True)
            .LaneSecs.DeleteAllOnSubmit(deletedLaneSecDetails)

        End With
    End Sub

    Protected Function GetLaneCalChanges(ByVal source As DTO.Lane, ByVal changeType As TrackingInfo) As List(Of LTS.LaneCal)
        ' Test record details for specified change type.
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim details As IEnumerable(Of LTS.LaneCal) = (
          From d In source.LaneCals
          Where d.TrackingState = changeType
          Select New LTS.LaneCal With {.LaneCalControl = d.LaneCalControl _
                                                  , .LaneCalLaneControl = d.LaneCalLaneControl _
                                                  , .LaneCalMonth = d.LaneCalMonth _
                                                  , .LaneCalDay = d.LaneCalDay _
                                                  , .LaneCalOpen = d.LaneCalOpen _
                                                  , .LaneCalStartTime = d.LaneCalStartTime _
                                                  , .LaneCalEndTime = d.LaneCalEndTime _
                                                  , .LaneCalIsHoliday = d.LaneCalIsHoliday _
                                                  , .LaneCalApplyToOrigin = d.LaneCalApplyToOrigin _
                                                  , .LaneCalUpdated = If(d.LaneCalUpdated Is Nothing, New Byte() {}, d.LaneCalUpdated)})
        Return details.ToList()
    End Function

    'Protected Function GetLaneCarrChanges(ByVal source As DTO.Lane, ByVal changeType As TrackingInfo) As List(Of LTS.LaneCarr)
    '    ' Test record details for specified change type.
    '    ' If Updated is null, set to byte[0] (for inserted items).
    '    Dim details As IEnumerable(Of LTS.LaneCarr) = ( _
    '      From d In source.LaneCarrs _
    '      Where d.TrackingState = changeType _
    '      Select New LTS.LaneCarr With {.LaneCarrControl = d.LaneCarrControl _
    '                                              , .LaneCarrLaneControl = d.LaneCarrLaneControl _
    '                                              , .LaneCarrCarrierControl = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrWgtFrom = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrWgtTo = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrRateStarts = d.LaneCarrRateStarts _
    '                                              , .LaneCarrRateExpires = d.LaneCarrRateExpires _
    '                                              , .LaneCarrTL = d.LaneCarrTL _
    '                                              , .LaneCarrLTL = d.LaneCarrLTL _
    '                                              , .LaneCarrEquipment = d.LaneCarrEquipment _
    '                                              , .LaneCarrMileRate = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrCwtRate = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrCaseRate = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrFlatRate = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrPltRate = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrCubeRate = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrTLT = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrTMode = d.LaneCarrTMode _
    '                                              , .LaneCarrFAK = d.LaneCarrFAK _
    '                                              , .LaneCarrDisc = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrPUMon = d.LaneCarrPUMon _
    '                                              , .LaneCarrPUTue = d.LaneCarrPUTue _
    '                                              , .LaneCarrPUWed = d.LaneCarrPUWed _
    '                                              , .LaneCarrPUThu = d.LaneCarrPUThu _
    '                                              , .LaneCarrPUFri = d.LaneCarrPUFri _
    '                                              , .LaneCarrPUSat = d.LaneCarrPUSat _
    '                                              , .LaneCarrPUSun = d.LaneCarrPUSun _
    '                                              , .LaneCarrDLMon = d.LaneCarrDLMon _
    '                                              , .LaneCarrDLTue = d.LaneCarrDLTue _
    '                                              , .LaneCarrDLWed = d.LaneCarrDLWed _
    '                                              , .LaneCarrDLThu = d.LaneCarrDLThu _
    '                                              , .LaneCarrDLFri = d.LaneCarrDLFri _
    '                                              , .LaneCarrDLSat = d.LaneCarrDLSat _
    '                                              , .LaneCarrDLSun = d.LaneCarrDLSun _
    '                                              , .LaneCarrPayTolPerLo = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrPayTolPerHi = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrPayTolCurLo = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrPayTolCurHi = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrCurType = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrModUser = Parameters.UserName _
    '                                              , .LaneCarrModDate = Date.Now _
    '                                              , .LaneCarrRoute = d.LaneCarrRoute _
    '                                              , .LaneCarrPltsOpen = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrPltsCommitted = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrPltsAvailable = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrMiles = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrBkhlCostPerc = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrPalletCostPer = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrFuelSurChargePerc = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrStopCharge = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrDropCost = d.LaneCarrDropCost _
    '                                              , .LaneCarrUnloadDiff = d.LaneCarrUnloadDiff _
    '                                              , .LaneCarrCasesAvailable = d.LaneCarrCasesAvailable _
    '                                              , .LaneCarrCasesOpen = d.LaneCarrCasesOpen _
    '                                              , .LaneCarrCasesCommitted = d.LaneCarrCasesCommitted _
    '                                              , .LaneCarrWgtAvailable = d.LaneCarrWgtAvailable _
    '                                              , .LaneCarrWgtOpen = d.LaneCarrWgtOpen _
    '                                              , .LaneCarrWgtCommitted = d.LaneCarrWgtCommitted _
    '                                              , .LaneCarrCubesAvailable = d.LaneCarrCubesAvailable _
    '                                              , .LaneCarrCubesOpen = d.LaneCarrCubesOpen _
    '                                              , .LaneCarrCubesCommitted = d.LaneCarrCubesCommitted _
    '                                              , .LaneCarrCarrierTruckControl = d.LaneCarrCarrierControl _
    '                                              , .LaneCarrLockSettings = d.LaneCarrLockSettings _
    '                                              , .LaneCarrUpdated = If(d.LaneCarrUpdated Is Nothing, New Byte() {}, d.LaneCarrUpdated)})
    '    Return details.ToList()
    'End Function

    Protected Function GetLaneFeeChanges(ByVal source As DTO.Lane, ByVal changeType As TrackingInfo) As List(Of LTS.LaneFee)
        ' Test record details for specified change type.
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim details As IEnumerable(Of LTS.LaneFee) = (
          From d In source.LaneFees
          Where d.TrackingState = changeType
          Select New LTS.LaneFee With {.LaneFeesControl = d.LaneFeesControl _
                                      , .LaneFeesLaneControl = d.LaneFeesLaneControl _
                                      , .LaneFeesMinimum = d.LaneFeesMinimum _
                                      , .LaneFeesVariable = d.LaneFeesVariable _
                                      , .LaneFeesAccessorialCode = d.LaneFeesAccessorialCode _
                                      , .LaneFeesModDate = Date.Now _
                                      , .LaneFeesModUser = Parameters.UserName _
                                      , .LaneFeesUpdated = If(d.LaneFeesUpdated Is Nothing, New Byte() {}, d.LaneFeesUpdated)})
        Return details.ToList()
    End Function

    Protected Function GetLaneSecChanges(ByVal source As DTO.Lane, ByVal changeType As TrackingInfo) As List(Of LTS.LaneSec)
        ' Test record details for specified change type.
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim details As IEnumerable(Of LTS.LaneSec) = (
          From d In source.LaneSecs
          Where d.TrackingState = changeType
          Select New LTS.LaneSec With {.LaneSecControl = d.LaneSecControl _
                                      , .LaneSecLaneControl = d.LaneSecLaneControl _
                                      , .LaneSecPUName = d.LaneSecPUName _
                                      , .LaneSecPUAddress1 = d.LaneSecPUAddress1 _
                                      , .LaneSecPUAddress2 = d.LaneSecPUAddress2 _
                                      , .LaneSecPUAddress3 = d.LaneSecPUAddress3 _
                                      , .LaneSecPUCity = d.LaneSecPUCity _
                                      , .LaneSecPUState = d.LaneSecPUState _
                                      , .LaneSecPUCountry = d.LaneSecPUCountry _
                                      , .LaneSecPUZip = d.LaneSecPUZip _
                                      , .LaneSecPUContactPhone = d.LaneSecPUContactPhone _
                                      , .LaneSecPUContactFax = d.LaneSecPUContactFax _
                                      , .LaneSecBrokerNumber = d.LaneSecBrokerNumber _
                                      , .LaneSecBrokerName = d.LaneSecBrokerName _
                                      , .LaneSecBrokerAddress1 = d.LaneSecBrokerAddress1 _
                                      , .LaneSecBrokerAddress2 = d.LaneSecBrokerAddress2 _
                                      , .LaneSecBrokerAddress3 = d.LaneSecBrokerAddress3 _
                                      , .LaneSecBrokerCity = d.LaneSecBrokerCity _
                                      , .LaneSecBrokerState = d.LaneSecBrokerState _
                                      , .LaneSecBrokerCountry = d.LaneSecBrokerCountry _
                                      , .LaneSecBrokerZip = d.LaneSecBrokerZip _
                                      , .LaneSecBrokerContactPhone = d.LaneSecBrokerContactPhone _
                                      , .LaneSecBrokerContactFax = d.LaneSecBrokerContactFax _
                                      , .LaneSecBrokerContactName = d.LaneSecBrokerContactName _
                                      , .LaneSecBrokerOpHourStart = d.LaneSecBrokerOpHourStart _
                                      , .LaneSecBrokerOpHourStop = d.LaneSecBrokerOpHourStop _
                                      , .LaneSecComment = d.LaneSecComment _
                                      , .LaneSecModDate = Date.Now _
                                      , .LaneSecModUser = Parameters.UserName _
                                      , .LaneSecUpdated = If(d.LaneSecUpdated Is Nothing, New Byte() {}, d.LaneSecUpdated)})
        Return details.ToList()
    End Function

    ''' <summary>
    ''' selectDTOData
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="page"></param>
    ''' <param name="pagecount"></param>
    ''' <param name="recordcount"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 11/1/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    ''' Added fields LaneRestrictCarrierSelection, LaneWarnOnRestrictedCarrierSelection, LaneRestrictedAtCompLevel
    ''' Modified by RHR for v-8.5.4.005 on 04/04/2024 New Time Zone settings
    ''' </remarks>
    Friend Function selectDTOData(ByVal d As LTS.Lane, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.Lane

        Return New DTO.Lane With {.LaneControl = d.LaneControl _
                                              , .LaneNumber = d.LaneNumber _
                                              , .LaneName = d.LaneName _
                                              , .LaneNumberMaster = d.LaneNumberMaster _
                                              , .LaneNameMaster = d.LaneNameMaster _
                                              , .LaneCompControl = d.LaneCompControl _
                                              , .LaneDefaultCarrierUse = If(d.LaneDefaultCarrierUse.HasValue, d.LaneDefaultCarrierUse.Value, False) _
                                              , .LaneDefaultCarrierControl = If(d.LaneDefaultCarrierControl.HasValue, d.LaneDefaultCarrierControl.Value, 0) _
                                              , .LaneDefaultCarrierContact = d.LaneDefaultCarrierContact _
                                              , .LaneDefaultCarrierPhone = d.LaneDefaultCarrierPhone _
                                              , .LaneOrigCompControl = If(d.LaneOrigCompControl.HasValue, d.LaneOrigCompControl.Value, 0) _
                                              , .LaneOrigName = d.LaneOrigName _
                                              , .LaneOrigAddress1 = d.LaneOrigAddress1 _
                                              , .LaneOrigAddress2 = d.LaneOrigAddress2 _
                                              , .LaneOrigAddress3 = d.LaneOrigAddress3 _
                                              , .LaneOrigCity = d.LaneOrigCity _
                                              , .LaneOrigState = d.LaneOrigState _
                                              , .LaneOrigCountry = d.LaneOrigCountry _
                                              , .LaneOrigZip = d.LaneOrigZip _
                                              , .LaneOrigContactPhone = d.LaneOrigContactPhone _
                                              , .LaneOrigContactPhoneExt = d.LaneOrigContactPhoneExt _
                                              , .LaneOrigContactFax = d.LaneOrigContactFax _
                                              , .LaneDestCompControl = If(d.LaneDestCompControl.HasValue, d.LaneDestCompControl.Value, 0) _
                                              , .LaneDestName = d.LaneDestName _
                                              , .LaneDestAddress1 = d.LaneDestAddress1 _
                                              , .LaneDestAddress2 = d.LaneDestAddress2 _
                                              , .LaneDestAddress3 = d.LaneDestAddress3 _
                                              , .LaneDestCity = d.LaneDestCity _
                                              , .LaneDestState = d.LaneDestState _
                                              , .LaneDestCountry = d.LaneDestCountry _
                                              , .LaneDestZip = d.LaneDestZip _
                                              , .LaneDestContactPhone = d.LaneDestContactPhone _
                                              , .LaneDestContactPhoneExt = d.LaneDestContactPhoneExt _
                                              , .LaneDestContactFax = d.LaneDestContactFax _
                                              , .LaneConsigneeNumber = d.LaneConsigneeNumber _
                                              , .LaneRecMinIn = If(d.LaneRecMinIn.HasValue, d.LaneRecMinIn.Value, 0) _
                                              , .LaneRecMinUnload = If(d.LaneRecMinUnload.HasValue, d.LaneRecMinUnload.Value, 0) _
                                              , .LaneRecMinOut = If(d.LaneRecMinOut.HasValue, d.LaneRecMinOut.Value, 0) _
                                              , .LaneAppt = If(d.LaneAppt.HasValue, d.LaneAppt.Value, False) _
                                              , .LanePalletExchange = If(d.LanePalletExchange.HasValue, d.LanePalletExchange.Value, False) _
                                              , .LanePalletType = d.LanePalletType _
                                              , .LaneBenchMiles = If(d.LaneBenchMiles.HasValue, d.LaneBenchMiles.Value, 0) _
                                              , .LaneBFC = If(d.LaneBFC.HasValue, d.LaneBFC.Value, 0) _
                                              , .LaneBFCType = d.LaneBFCType _
                                              , .LaneRecHourStart = d.LaneRecHourStart _
                                              , .LaneRecHourStop = d.LaneRecHourStop _
                                              , .LaneDestHourStart = d.LaneDestHourStart _
                                              , .LaneDestHourStop = d.LaneDestHourStop _
                                              , .LaneComments = d.LaneComments _
                                              , .LaneCommentsConfidential = d.LaneCommentsConfidential _
                                              , .LaneCurType = If(d.LaneCurType.HasValue, d.LaneCurType.Value, 0) _
                                              , .LaneActive = If(d.LaneActive.HasValue, d.LaneActive.Value, True) _
                                              , .LaneLatitude = If(d.LaneLatitude.HasValue, d.LaneLatitude.Value, 0) _
                                              , .LaneLongitude = If(d.LaneLongitude.HasValue, d.LaneLongitude.Value, 0) _
                                              , .LaneTempType = d.LaneTempType _
                                              , .LaneTransType = d.LaneTransType _
                                              , .LanePrimaryBuyer = d.LanePrimaryBuyer _
                                              , .LaneOLTBenchmark = If(d.LaneOLTBenchmark.HasValue, d.LaneOLTBenchmark.Value, 0) _
                                              , .LaneTLTBenchmark = If(d.LaneTLTBenchmark.HasValue, d.LaneTLTBenchmark.Value, 0) _
                                              , .LaneAptDelivery = If(d.LaneAptDelivery.HasValue, d.LaneAptDelivery.Value, False) _
                                              , .LaneOrderControl = If(d.LaneOrderControl.HasValue, d.LaneOrderControl.Value, 0) _
                                              , .LaneOrderSTDWgt = If(d.LaneOrderSTDWgt.HasValue, d.LaneOrderSTDWgt.Value, 0) _
                                              , .LaneOrderSTDCases = If(d.LaneOrderSTDCases.HasValue, d.LaneOrderSTDCases.Value, 0) _
                                              , .LaneOrderSTDCubes = If(d.LaneOrderSTDCubes.HasValue, d.LaneOrderSTDCubes.Value, 0) _
                                              , .LaneOrderSTDPUAllow = If(d.LaneOrderSTDPUAllow.HasValue, d.LaneOrderSTDPUAllow.Value, 0) _
                                              , .LaneOrderSTDAllowType = d.LaneOrderSTDAllowType _
                                              , .LaneOrderSTDAllowValue = If(d.LaneOrderSTDAllowValue.HasValue, d.LaneOrderSTDAllowValue.Value, 0) _
                                              , .LaneOrderSTDMonthlyOrder = If(d.LaneOrderSTDMonthlyOrder.HasValue, d.LaneOrderSTDMonthlyOrder.Value, 0) _
                                              , .LaneOrderSTDYearlyFRT = If(d.LaneOrderSTDYearlyFRT.HasValue, d.LaneOrderSTDYearlyFRT.Value, 0) _
                                              , .LaneOrderSTDCarrierControl = If(d.LaneOrderSTDCarrierControl.HasValue, d.LaneOrderSTDCarrierControl.Value, 0) _
                                              , .LaneOrderSTDCostMile = If(d.LaneOrderSTDCostMile.HasValue, d.LaneOrderSTDCostMile.Value, 0) _
                                              , .LaneOrderSTDCostCWT = If(d.LaneOrderSTDCostCWT.HasValue, d.LaneOrderSTDCostCWT.Value, 0) _
                                              , .LaneOrderSTDCostFlat = If(d.LaneOrderSTDCostFlat.HasValue, d.LaneOrderSTDCostFlat.Value, 0) _
                                              , .LaneOrderACTWgt = If(d.LaneOrderACTWgt.HasValue, d.LaneOrderACTWgt.Value, 0) _
                                              , .LaneOrderACTCases = If(d.LaneOrderACTCases.HasValue, d.LaneOrderACTCases.Value, 0) _
                                              , .LaneOrderACTCubes = If(d.LaneOrderACTCubes.HasValue, d.LaneOrderACTCubes.Value, 0) _
                                              , .LaneOrderACTPUAllow = If(d.LaneOrderACTPUAllow.HasValue, d.LaneOrderACTPUAllow.Value, 0) _
                                              , .LaneOrderACTAllowType = d.LaneOrderACTAllowType _
                                              , .LaneOrderACTAllowValue = If(d.LaneOrderACTAllowValue.HasValue, d.LaneOrderACTAllowValue.Value, 0) _
                                              , .LaneOrderACTMonthlyOrder = If(d.LaneOrderACTMonthlyOrder.HasValue, d.LaneOrderACTMonthlyOrder.Value, 0) _
                                              , .LaneOrderACTYearlyFRT = If(d.LaneOrderACTYearlyFRT.HasValue, d.LaneOrderACTYearlyFRT.Value, 0) _
                                              , .LaneOrderACTCarrierControl = If(d.LaneOrderACTCarrierControl.HasValue, d.LaneOrderACTCarrierControl.Value, 0) _
                                              , .LaneOrderACTCostMile = If(d.LaneOrderACTCostMile.HasValue, d.LaneOrderACTCostMile.Value, 0) _
                                              , .LaneOrderACTCostFlat = If(d.LaneOrderACTCostFlat.HasValue, d.LaneOrderACTCostFlat.Value, 0) _
                                              , .LaneOrderACTCostCWT = If(d.LaneOrderACTCostCWT.HasValue, d.LaneOrderACTCostCWT.Value, 0) _
                                              , .LaneStops = If(d.LaneStops.HasValue, d.LaneStops.Value, 0) _
                                              , .LaneFixedTime = d.LaneFixedTime _
                                              , .LaneOriginAddressUse = If(d.LaneOriginAddressUse.HasValue, d.LaneOriginAddressUse.Value, False) _
                                              , .LaneSDFUse = If(d.LaneOriginAddressUse.HasValue, d.LaneOriginAddressUse.Value, False) _
                                              , .LaneSDFSRZone = If(d.LaneSDFSRZone.HasValue, d.LaneSDFSRZone.Value, 0) _
                                              , .LaneSDFMRZone = If(d.LaneSDFMRZone.HasValue, d.LaneSDFMRZone.Value, 0) _
                                              , .LaneSDFFixedTime = If(d.LaneSDFFixedTime.HasValue, d.LaneSDFFixedTime.Value, 0) _
                                              , .LaneSDFEarlyTM1 = If(d.LaneSDFEarlyTM1.HasValue, d.LaneSDFEarlyTM1.Value, 0) _
                                              , .LaneSDFLateTM1 = If(d.LaneSDFLateTM1.HasValue, d.LaneSDFLateTM1.Value, 0) _
                                              , .LaneSDFDay1 = d.LaneSDFDay1 _
                                              , .LaneSDFEarlyTM2 = If(d.LaneSDFEarlyTM2.HasValue, d.LaneSDFEarlyTM2.Value, 0) _
                                              , .LaneSDFLateTM2 = If(d.LaneSDFLateTM2.HasValue, d.LaneSDFLateTM2.Value, 0) _
                                              , .LaneSDFDay2 = d.LaneSDFDay2 _
                                              , .LaneSDFEarlyTM3 = If(d.LaneSDFEarlyTM3.HasValue, d.LaneSDFEarlyTM3.Value, 0) _
                                              , .LaneSDFLateTM3 = If(d.LaneSDFLateTM3.HasValue, d.LaneSDFLateTM3.Value, 0) _
                                              , .LaneSDFDay3 = d.LaneSDFDay3 _
                                              , .LaneSDFEarlyTM4 = If(d.LaneSDFEarlyTM4.HasValue, d.LaneSDFEarlyTM4.Value, 0) _
                                              , .LaneSDFLateTM4 = If(d.LaneSDFLateTM4.HasValue, d.LaneSDFLateTM4.Value, 0) _
                                              , .LaneSDFDay4 = d.LaneSDFDay4 _
                                              , .LaneSDFEarlyTM5 = If(d.LaneSDFEarlyTM5.HasValue, d.LaneSDFEarlyTM5.Value, 0) _
                                              , .LaneSDFLateTM5 = If(d.LaneSDFLateTM5.HasValue, d.LaneSDFLateTM5.Value, 0) _
                                              , .LaneSDFDay5 = d.LaneSDFDay5 _
                                              , .LaneSDFEarlyTM6 = If(d.LaneSDFEarlyTM6.HasValue, d.LaneSDFEarlyTM6.Value, 0) _
                                              , .LaneSDFLateTM6 = If(d.LaneSDFLateTM6.HasValue, d.LaneSDFLateTM6.Value, 0) _
                                              , .LaneSDFDay6 = d.LaneSDFDay6 _
                                              , .LaneSDFEarlyTM7 = If(d.LaneSDFEarlyTM7.HasValue, d.LaneSDFEarlyTM7.Value, 0) _
                                              , .LaneSDFLateTM7 = If(d.LaneSDFLateTM7.HasValue, d.LaneSDFLateTM7.Value, 0) _
                                              , .LaneSDFDay7 = d.LaneSDFDay7 _
                                              , .LaneSDFUnldRate1 = If(d.LaneSDFUnldRate1.HasValue, d.LaneSDFUnldRate1.Value, 0) _
                                              , .LaneSDFUnldRate2 = If(d.LaneSDFUnldRate2.HasValue, d.LaneSDFUnldRate2.Value, 0) _
                                              , .LaneSDFUnldRate3 = If(d.LaneSDFUnldRate3.HasValue, d.LaneSDFUnldRate3.Value, 0) _
                                              , .LaneSDFUnldRate4 = If(d.LaneSDFUnldRate4.HasValue, d.LaneSDFUnldRate4.Value, 0) _
                                              , .LaneSDFUnldRate5 = If(d.LaneSDFUnldRate5.HasValue, d.LaneSDFUnldRate5.Value, 0) _
                                              , .LaneAutoTenderFlag = d.LaneAutoTenderFlag _
                                              , .LaneCascadingDispatchingFlag = d.LaneCascadingDispatchingFlag _
                                              , .LanePortofEntry = d.LanePortofEntry _
                                              , .LaneDoNotInvoice = d.LaneDoNotInvoice _
                                              , .LaneTLCases = If(d.LaneTLCases.HasValue, d.LaneTLCases.Value, 0) _
                                              , .LaneTLWgt = If(d.LaneTLWgt.HasValue, d.LaneTLWgt.Value, 0) _
                                              , .LaneTLCube = If(d.LaneTLCube.HasValue, d.LaneTLCube.Value, 0) _
                                              , .LaneTLPL = If(d.LaneTLPL.HasValue, d.LaneTLPL.Value, 0) _
                                              , .LaneChepGLID = d.LaneChepGLID _
                                              , .LaneCarrierTypeCode = d.LaneCarrierTypeCode _
                                              , .LaneCarrierEquipmentCodes = d.LaneCarrierEquipmentCodes _
                                              , .LanePickUpMon = d.LanePickUpMon _
                                              , .LanePickUpTue = d.LanePickUpTue _
                                              , .LanePickUpWed = d.LanePickUpWed _
                                              , .LanePickUpThu = d.LanePickUpThu _
                                              , .LanePickUpFri = d.LanePickUpFri _
                                              , .LanePickUpSat = d.LanePickUpSat _
                                              , .LanePickUpSun = d.LanePickUpSun _
                                              , .LaneDropOffMon = d.LaneDropOffMon _
                                              , .LaneDropOffTue = d.LaneDropOffTue _
                                              , .LaneDropOffWed = d.LaneDropOffWed _
                                              , .LaneDropOffThu = d.LaneDropOffThu _
                                              , .LaneDropOffFri = d.LaneDropOffFri _
                                              , .LaneDropOffSat = d.LaneDropOffSat _
                                              , .LaneDropOffSun = d.LaneDropOffSun _
                                              , .LaneOrigStopControl = d.LaneOrigStopControl _
                                              , .LaneDestStopControl = d.LaneDestStopControl _
                                              , .LaneRouteTypeCode = d.LaneRouteTypeCode _
                                              , .LaneDefaultRouteSequence = d.LaneDefaultRouteSequence _
                                              , .LaneRouteGuideControl = d.LaneRouteGuideControl _
                                              , .LaneRouteGuideNumber = d.LaneRouteGuideNumber _
                                              , .LaneIsCrossDockFacility = d.LaneIsCrossDockFacility _
                                              , .LaneRequiredOnTimeServiceLevel = d.LaneRequiredOnTimeServiceLevel _
                                              , .LaneCalcOnTimeServiceLevel = d.LaneCalcOnTimeServiceLevel _
                                              , .LaneCalcOnTimeNoMonthsUsed = d.LaneCalcOnTimeNoMonthsUsed _
                                              , .LaneModeTypeControl = d.LaneModeTypeControl _
                                              , .LaneUser1 = d.LaneUser1 _
                                              , .LaneUser2 = d.LaneUser2 _
                                              , .LaneUser3 = d.LaneUser3 _
                                              , .LaneUser4 = d.LaneUser4 _
                                              , .LaneIsTransLoad = d.LaneIsTransLoad _
                                              , .LaneLegalEntity = d.LaneLegalEntity _
                                              , .LaneAllowInterline = d.LaneAllowInterline _
                                              , .LaneRestrictCarrierSelection = d.LaneRestrictCarrierSelection _
                                              , .LaneWarnOnRestrictedCarrierSelection = d.LaneWarnOnRestrictedCarrierSelection _
                                              , .LaneRestrictedAtCompLevel = d.LaneRestrictedAtCompLevel _
                                              , .LaneOrigContactName = d.LaneOrigContactName _
                                              , .LaneOrigContactEmail = d.LaneOrigContactEmail _
                                              , .LaneOrigEmergencyContactPhone = d.LaneOrigEmergencyContactPhone _
                                              , .LaneOrigEmergencyContactName = d.LaneOrigEmergencyContactName _
                                              , .LaneDestContactName = d.LaneDestContactName _
                                              , .LaneDestContactEmail = d.LaneDestContactEmail _
                                              , .LaneDestEmergencyContactPhone = d.LaneDestEmergencyContactPhone _
                                              , .LaneDestEmergencyContactName = d.LaneDestEmergencyContactName _
                                              , .LaneWeightUnit = d.LaneWeightUnit _
                                              , .LaneLengthUnit = d.LaneLengthUnit _
                                              , .LaneAllowCarrierBookApptByEmail = If(d.LaneAllowCarrierBookApptByEmail, False) _
                                              , .LaneRequireCarrierAuthBookApptByEmail = If(d.LaneRequireCarrierAuthBookApptByEmail, False) _
                                              , .LaneUseCarrieContEmailForBookApptByEmail = If(d.LaneUseCarrieContEmailForBookApptByEmail, False) _
                                              , .LaneCarrierBookApptviaTokenEmail = d.LaneCarrierBookApptviaTokenEmail _
                                              , .LaneCarrierBookApptviaTokenFailEmail = d.LaneCarrierBookApptviaTokenFailEmail _
                                              , .LaneCarrierBookApptviaTokenFailPhone = d.LaneCarrierBookApptviaTokenFailPhone _
                                              , .LaneTransLeadTimeCalcType = d.LaneTransLeadTimeCalcType _
                                              , .LaneTransLeadTimeUseMasterLane = d.LaneTransLeadTimeUseMasterLane _
                                              , .LaneTransLeadTimeLocationOption = d.LaneTransLeadTimeLocationOption _
                                              , .LaneModDate = d.LaneModDate _
                                              , .LaneModUser = d.LaneModUser _
                                              , .LaneUpdated = d.LaneUpdated.ToArray() _
                                              , .Page = page _
                                              , .Pages = pagecount _
                                              , .RecordCount = recordcount _
                                              , .PageSize = pagesize _
                                              , .LaneOrigTimeZone = d.LaneOrigTimeZone _
                                              , .LaneDestTimeZone = d.LaneDestTimeZone}

    End Function

    ''' <summary>
    ''' selectDTODataWDetails
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="page"></param>
    ''' <param name="pagecount"></param>
    ''' <param name="recordcount"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 11/1/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    ''' Added fields LaneRestrictCarrierSelection, LaneWarnOnRestrictedCarrierSelection, LaneRestrictedAtCompLevel
    ''' Modified by RHR for v-8.5.4.005 on 04/04/2024 New Time Zone settings
    ''' </remarks>
    Friend Function selectDTODataWDetails(ByVal d As LTS.Lane, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.Lane

        Dim oLaneCalData As NGLLaneCalData = New NGLLaneCalData(Parameters)
        Dim oLaneFeeData As NGLLaneFeeData = New NGLLaneFeeData(Parameters)
        Dim oLaneSecData As NGLLaneSecData = New NGLLaneSecData(Parameters)

        Return New DTO.Lane With {.LaneControl = d.LaneControl _
                                              , .LaneNumber = d.LaneNumber _
                                              , .LaneName = d.LaneName _
                                              , .LaneNumberMaster = d.LaneNumberMaster _
                                              , .LaneNameMaster = d.LaneNameMaster _
                                              , .LaneCompControl = d.LaneCompControl _
                                              , .LaneDefaultCarrierUse = If(d.LaneDefaultCarrierUse.HasValue, d.LaneDefaultCarrierUse.Value, False) _
                                              , .LaneDefaultCarrierControl = If(d.LaneDefaultCarrierControl.HasValue, d.LaneDefaultCarrierControl.Value, 0) _
                                              , .LaneDefaultCarrierContact = d.LaneDefaultCarrierContact _
                                              , .LaneDefaultCarrierPhone = d.LaneDefaultCarrierPhone _
                                              , .LaneOrigCompControl = If(d.LaneOrigCompControl.HasValue, d.LaneOrigCompControl.Value, 0) _
                                              , .LaneOrigName = d.LaneOrigName _
                                              , .LaneOrigAddress1 = d.LaneOrigAddress1 _
                                              , .LaneOrigAddress2 = d.LaneOrigAddress2 _
                                              , .LaneOrigAddress3 = d.LaneOrigAddress3 _
                                              , .LaneOrigCity = d.LaneOrigCity _
                                              , .LaneOrigState = d.LaneOrigState _
                                              , .LaneOrigCountry = d.LaneOrigCountry _
                                              , .LaneOrigZip = d.LaneOrigZip _
                                              , .LaneOrigContactPhone = d.LaneOrigContactPhone _
                                              , .LaneOrigContactPhoneExt = d.LaneOrigContactPhoneExt _
                                              , .LaneOrigContactFax = d.LaneOrigContactFax _
                                              , .LaneDestCompControl = If(d.LaneDestCompControl.HasValue, d.LaneDestCompControl.Value, 0) _
                                              , .LaneDestName = d.LaneDestName _
                                              , .LaneDestAddress1 = d.LaneDestAddress1 _
                                              , .LaneDestAddress2 = d.LaneDestAddress2 _
                                              , .LaneDestAddress3 = d.LaneDestAddress3 _
                                              , .LaneDestCity = d.LaneDestCity _
                                              , .LaneDestState = d.LaneDestState _
                                              , .LaneDestCountry = d.LaneDestCountry _
                                              , .LaneDestZip = d.LaneDestZip _
                                              , .LaneDestContactPhone = d.LaneDestContactPhone _
                                              , .LaneDestContactPhoneExt = d.LaneDestContactPhoneExt _
                                              , .LaneDestContactFax = d.LaneDestContactFax _
                                              , .LaneConsigneeNumber = d.LaneConsigneeNumber _
                                              , .LaneRecMinIn = If(d.LaneRecMinIn.HasValue, d.LaneRecMinIn.Value, 0) _
                                              , .LaneRecMinUnload = If(d.LaneRecMinUnload.HasValue, d.LaneRecMinUnload.Value, 0) _
                                              , .LaneRecMinOut = If(d.LaneRecMinOut.HasValue, d.LaneRecMinOut.Value, 0) _
                                              , .LaneAppt = If(d.LaneAppt.HasValue, d.LaneAppt.Value, False) _
                                              , .LanePalletExchange = If(d.LanePalletExchange.HasValue, d.LanePalletExchange.Value, False) _
                                              , .LanePalletType = d.LanePalletType _
                                              , .LaneBenchMiles = If(d.LaneBenchMiles.HasValue, d.LaneBenchMiles.Value, 0) _
                                              , .LaneBFC = If(d.LaneBFC.HasValue, d.LaneBFC.Value, 0) _
                                              , .LaneBFCType = d.LaneBFCType _
                                              , .LaneRecHourStart = d.LaneRecHourStart _
                                              , .LaneRecHourStop = d.LaneRecHourStop _
                                              , .LaneDestHourStart = d.LaneDestHourStart _
                                              , .LaneDestHourStop = d.LaneDestHourStop _
                                              , .LaneComments = d.LaneComments _
                                              , .LaneCommentsConfidential = d.LaneCommentsConfidential _
                                              , .LaneCurType = If(d.LaneCurType.HasValue, d.LaneCurType.Value, 0) _
                                              , .LaneActive = If(d.LaneActive.HasValue, d.LaneActive.Value, True) _
                                              , .LaneLatitude = If(d.LaneLatitude.HasValue, d.LaneLatitude.Value, 0) _
                                              , .LaneLongitude = If(d.LaneLongitude.HasValue, d.LaneLongitude.Value, 0) _
                                              , .LaneTempType = d.LaneTempType _
                                              , .LaneTransType = d.LaneTransType _
                                              , .LanePrimaryBuyer = d.LanePrimaryBuyer _
                                              , .LaneOLTBenchmark = If(d.LaneOLTBenchmark.HasValue, d.LaneOLTBenchmark.Value, 0) _
                                              , .LaneTLTBenchmark = If(d.LaneTLTBenchmark.HasValue, d.LaneTLTBenchmark.Value, 0) _
                                              , .LaneAptDelivery = If(d.LaneAptDelivery.HasValue, d.LaneAptDelivery.Value, False) _
                                              , .LaneOrderControl = If(d.LaneOrderControl.HasValue, d.LaneOrderControl.Value, 0) _
                                              , .LaneOrderSTDWgt = If(d.LaneOrderSTDWgt.HasValue, d.LaneOrderSTDWgt.Value, 0) _
                                              , .LaneOrderSTDCases = If(d.LaneOrderSTDCases.HasValue, d.LaneOrderSTDCases.Value, 0) _
                                              , .LaneOrderSTDCubes = If(d.LaneOrderSTDCubes.HasValue, d.LaneOrderSTDCubes.Value, 0) _
                                              , .LaneOrderSTDPUAllow = If(d.LaneOrderSTDPUAllow.HasValue, d.LaneOrderSTDPUAllow.Value, 0) _
                                              , .LaneOrderSTDAllowType = d.LaneOrderSTDAllowType _
                                              , .LaneOrderSTDAllowValue = If(d.LaneOrderSTDAllowValue.HasValue, d.LaneOrderSTDAllowValue.Value, 0) _
                                              , .LaneOrderSTDMonthlyOrder = If(d.LaneOrderSTDMonthlyOrder.HasValue, d.LaneOrderSTDMonthlyOrder.Value, 0) _
                                              , .LaneOrderSTDYearlyFRT = If(d.LaneOrderSTDYearlyFRT.HasValue, d.LaneOrderSTDYearlyFRT.Value, 0) _
                                              , .LaneOrderSTDCarrierControl = If(d.LaneOrderSTDCarrierControl.HasValue, d.LaneOrderSTDCarrierControl.Value, 0) _
                                              , .LaneOrderSTDCostMile = If(d.LaneOrderSTDCostMile.HasValue, d.LaneOrderSTDCostMile.Value, 0) _
                                              , .LaneOrderSTDCostCWT = If(d.LaneOrderSTDCostCWT.HasValue, d.LaneOrderSTDCostCWT.Value, 0) _
                                              , .LaneOrderSTDCostFlat = If(d.LaneOrderSTDCostFlat.HasValue, d.LaneOrderSTDCostFlat.Value, 0) _
                                              , .LaneOrderACTWgt = If(d.LaneOrderACTWgt.HasValue, d.LaneOrderACTWgt.Value, 0) _
                                              , .LaneOrderACTCases = If(d.LaneOrderACTCases.HasValue, d.LaneOrderACTCases.Value, 0) _
                                              , .LaneOrderACTCubes = If(d.LaneOrderACTCubes.HasValue, d.LaneOrderACTCubes.Value, 0) _
                                              , .LaneOrderACTPUAllow = If(d.LaneOrderACTPUAllow.HasValue, d.LaneOrderACTPUAllow.Value, 0) _
                                              , .LaneOrderACTAllowType = d.LaneOrderACTAllowType _
                                              , .LaneOrderACTAllowValue = If(d.LaneOrderACTAllowValue.HasValue, d.LaneOrderACTAllowValue.Value, 0) _
                                              , .LaneOrderACTMonthlyOrder = If(d.LaneOrderACTMonthlyOrder.HasValue, d.LaneOrderACTMonthlyOrder.Value, 0) _
                                              , .LaneOrderACTYearlyFRT = If(d.LaneOrderACTYearlyFRT.HasValue, d.LaneOrderACTYearlyFRT.Value, 0) _
                                              , .LaneOrderACTCarrierControl = If(d.LaneOrderACTCarrierControl.HasValue, d.LaneOrderACTCarrierControl.Value, 0) _
                                              , .LaneOrderACTCostMile = If(d.LaneOrderACTCostMile.HasValue, d.LaneOrderACTCostMile.Value, 0) _
                                              , .LaneOrderACTCostFlat = If(d.LaneOrderACTCostFlat.HasValue, d.LaneOrderACTCostFlat.Value, 0) _
                                              , .LaneOrderACTCostCWT = If(d.LaneOrderACTCostCWT.HasValue, d.LaneOrderACTCostCWT.Value, 0) _
                                              , .LaneStops = If(d.LaneStops.HasValue, d.LaneStops.Value, 0) _
                                              , .LaneFixedTime = d.LaneFixedTime _
                                              , .LaneOriginAddressUse = If(d.LaneOriginAddressUse.HasValue, d.LaneOriginAddressUse.Value, False) _
                                              , .LaneSDFUse = If(d.LaneOriginAddressUse.HasValue, d.LaneOriginAddressUse.Value, False) _
                                              , .LaneSDFSRZone = If(d.LaneSDFSRZone.HasValue, d.LaneSDFSRZone.Value, 0) _
                                              , .LaneSDFMRZone = If(d.LaneSDFMRZone.HasValue, d.LaneSDFMRZone.Value, 0) _
                                              , .LaneSDFFixedTime = If(d.LaneSDFFixedTime.HasValue, d.LaneSDFFixedTime.Value, 0) _
                                              , .LaneSDFEarlyTM1 = If(d.LaneSDFEarlyTM1.HasValue, d.LaneSDFEarlyTM1.Value, 0) _
                                              , .LaneSDFLateTM1 = If(d.LaneSDFLateTM1.HasValue, d.LaneSDFLateTM1.Value, 0) _
                                              , .LaneSDFDay1 = d.LaneSDFDay1 _
                                              , .LaneSDFEarlyTM2 = If(d.LaneSDFEarlyTM2.HasValue, d.LaneSDFEarlyTM2.Value, 0) _
                                              , .LaneSDFLateTM2 = If(d.LaneSDFLateTM2.HasValue, d.LaneSDFLateTM2.Value, 0) _
                                              , .LaneSDFDay2 = d.LaneSDFDay2 _
                                              , .LaneSDFEarlyTM3 = If(d.LaneSDFEarlyTM3.HasValue, d.LaneSDFEarlyTM3.Value, 0) _
                                              , .LaneSDFLateTM3 = If(d.LaneSDFLateTM3.HasValue, d.LaneSDFLateTM3.Value, 0) _
                                              , .LaneSDFDay3 = d.LaneSDFDay3 _
                                              , .LaneSDFEarlyTM4 = If(d.LaneSDFEarlyTM4.HasValue, d.LaneSDFEarlyTM4.Value, 0) _
                                              , .LaneSDFLateTM4 = If(d.LaneSDFLateTM4.HasValue, d.LaneSDFLateTM4.Value, 0) _
                                              , .LaneSDFDay4 = d.LaneSDFDay4 _
                                              , .LaneSDFEarlyTM5 = If(d.LaneSDFEarlyTM5.HasValue, d.LaneSDFEarlyTM5.Value, 0) _
                                              , .LaneSDFLateTM5 = If(d.LaneSDFLateTM5.HasValue, d.LaneSDFLateTM5.Value, 0) _
                                              , .LaneSDFDay5 = d.LaneSDFDay5 _
                                              , .LaneSDFEarlyTM6 = If(d.LaneSDFEarlyTM6.HasValue, d.LaneSDFEarlyTM6.Value, 0) _
                                              , .LaneSDFLateTM6 = If(d.LaneSDFLateTM6.HasValue, d.LaneSDFLateTM6.Value, 0) _
                                              , .LaneSDFDay6 = d.LaneSDFDay6 _
                                              , .LaneSDFEarlyTM7 = If(d.LaneSDFEarlyTM7.HasValue, d.LaneSDFEarlyTM7.Value, 0) _
                                              , .LaneSDFLateTM7 = If(d.LaneSDFLateTM7.HasValue, d.LaneSDFLateTM7.Value, 0) _
                                              , .LaneSDFDay7 = d.LaneSDFDay7 _
                                              , .LaneSDFUnldRate1 = If(d.LaneSDFUnldRate1.HasValue, d.LaneSDFUnldRate1.Value, 0) _
                                              , .LaneSDFUnldRate2 = If(d.LaneSDFUnldRate2.HasValue, d.LaneSDFUnldRate2.Value, 0) _
                                              , .LaneSDFUnldRate3 = If(d.LaneSDFUnldRate3.HasValue, d.LaneSDFUnldRate3.Value, 0) _
                                              , .LaneSDFUnldRate4 = If(d.LaneSDFUnldRate4.HasValue, d.LaneSDFUnldRate4.Value, 0) _
                                              , .LaneSDFUnldRate5 = If(d.LaneSDFUnldRate5.HasValue, d.LaneSDFUnldRate5.Value, 0) _
                                              , .LaneAutoTenderFlag = d.LaneAutoTenderFlag _
                                              , .LaneCascadingDispatchingFlag = d.LaneCascadingDispatchingFlag _
                                              , .LanePortofEntry = d.LanePortofEntry _
                                              , .LaneDoNotInvoice = d.LaneDoNotInvoice _
                                              , .LaneTLCases = If(d.LaneTLCases.HasValue, d.LaneTLCases.Value, 0) _
                                              , .LaneTLWgt = If(d.LaneTLWgt.HasValue, d.LaneTLWgt.Value, 0) _
                                              , .LaneTLCube = If(d.LaneTLCube.HasValue, d.LaneTLCube.Value, 0) _
                                              , .LaneTLPL = If(d.LaneTLPL.HasValue, d.LaneTLPL.Value, 0) _
                                              , .LaneChepGLID = d.LaneChepGLID _
                                              , .LaneCarrierTypeCode = d.LaneCarrierTypeCode _
                                              , .LaneCarrierEquipmentCodes = d.LaneCarrierEquipmentCodes _
                                              , .LanePickUpMon = d.LanePickUpMon _
                                              , .LanePickUpTue = d.LanePickUpTue _
                                              , .LanePickUpWed = d.LanePickUpWed _
                                              , .LanePickUpThu = d.LanePickUpThu _
                                              , .LanePickUpFri = d.LanePickUpFri _
                                              , .LanePickUpSat = d.LanePickUpSat _
                                              , .LanePickUpSun = d.LanePickUpSun _
                                              , .LaneDropOffMon = d.LaneDropOffMon _
                                              , .LaneDropOffTue = d.LaneDropOffTue _
                                              , .LaneDropOffWed = d.LaneDropOffWed _
                                              , .LaneDropOffThu = d.LaneDropOffThu _
                                              , .LaneDropOffFri = d.LaneDropOffFri _
                                              , .LaneDropOffSat = d.LaneDropOffSat _
                                              , .LaneDropOffSun = d.LaneDropOffSun _
                                              , .LaneOrigStopControl = d.LaneOrigStopControl _
                                              , .LaneDestStopControl = d.LaneDestStopControl _
                                              , .LaneRouteTypeCode = d.LaneRouteTypeCode _
                                              , .LaneDefaultRouteSequence = d.LaneDefaultRouteSequence _
                                              , .LaneRouteGuideControl = d.LaneRouteGuideControl _
                                              , .LaneRouteGuideNumber = d.LaneRouteGuideNumber _
                                              , .LaneIsCrossDockFacility = d.LaneIsCrossDockFacility _
                                              , .LaneRequiredOnTimeServiceLevel = d.LaneRequiredOnTimeServiceLevel _
                                              , .LaneCalcOnTimeServiceLevel = d.LaneCalcOnTimeServiceLevel _
                                              , .LaneCalcOnTimeNoMonthsUsed = d.LaneCalcOnTimeNoMonthsUsed _
                                              , .LaneModeTypeControl = d.LaneModeTypeControl _
                                              , .LaneUser1 = d.LaneUser1 _
                                              , .LaneUser2 = d.LaneUser2 _
                                              , .LaneUser3 = d.LaneUser3 _
                                              , .LaneUser4 = d.LaneUser4 _
                                              , .LaneLegalEntity = d.LaneLegalEntity _
                                              , .LaneIsTransLoad = d.LaneIsTransLoad _
                                              , .LaneAllowInterline = d.LaneAllowInterline _
                                              , .LaneRestrictCarrierSelection = d.LaneRestrictCarrierSelection _
                                              , .LaneWarnOnRestrictedCarrierSelection = d.LaneWarnOnRestrictedCarrierSelection _
                                              , .LaneRestrictedAtCompLevel = d.LaneRestrictedAtCompLevel _
                                              , .LaneOrigContactName = d.LaneOrigContactName _
                                              , .LaneOrigContactEmail = d.LaneOrigContactEmail _
                                              , .LaneOrigEmergencyContactPhone = d.LaneOrigEmergencyContactPhone _
                                              , .LaneOrigEmergencyContactName = d.LaneOrigEmergencyContactName _
                                              , .LaneDestContactName = d.LaneDestContactName _
                                              , .LaneDestContactEmail = d.LaneDestContactEmail _
                                              , .LaneDestEmergencyContactPhone = d.LaneDestEmergencyContactPhone _
                                              , .LaneDestEmergencyContactName = d.LaneDestEmergencyContactName _
                                              , .LaneWeightUnit = d.LaneWeightUnit _
                                              , .LaneLengthUnit = d.LaneLengthUnit _
                                              , .LaneAllowCarrierBookApptByEmail = If(d.LaneAllowCarrierBookApptByEmail, False) _
                                              , .LaneRequireCarrierAuthBookApptByEmail = If(d.LaneRequireCarrierAuthBookApptByEmail, False) _
                                              , .LaneUseCarrieContEmailForBookApptByEmail = If(d.LaneUseCarrieContEmailForBookApptByEmail, False) _
                                              , .LaneCarrierBookApptviaTokenEmail = d.LaneCarrierBookApptviaTokenEmail _
                                              , .LaneCarrierBookApptviaTokenFailEmail = d.LaneCarrierBookApptviaTokenFailEmail _
                                              , .LaneCarrierBookApptviaTokenFailPhone = d.LaneCarrierBookApptviaTokenFailPhone _
                                              , .LaneTransLeadTimeCalcType = d.LaneTransLeadTimeCalcType _
                                              , .LaneTransLeadTimeUseMasterLane = d.LaneTransLeadTimeUseMasterLane _
                                              , .LaneTransLeadTimeLocationOption = d.LaneTransLeadTimeLocationOption _
                                              , .LaneModDate = d.LaneModDate _
                                              , .LaneModUser = d.LaneModUser _
                                              , .LaneUpdated = d.LaneUpdated.ToArray() _
                                              , .LaneCals = (From lc In d.LaneCals Select oLaneCalData.selectDTOData(lc)).ToList() _
                                              , .LaneFees = (From lf In d.LaneFees Select oLaneFeeData.selectDTOData(lf)).ToList() _
                                              , .LaneSecs = (From ls In d.LaneSecs Select oLaneSecData.selectDTOData(ls)).ToList() _
                                              , .Page = page _
                                              , .Pages = pagecount _
                                              , .RecordCount = recordcount _
                                              , .PageSize = pagesize _
                                              , .LaneOrigTimeZone = d.LaneOrigTimeZone _
                                              , .LaneDestTimeZone = d.LaneDestTimeZone}

    End Function

    'Added By LVV on 9/12/16 for v-7.0.5.110 HDM Enhancement
    Public Overrides Function Update(Of TEntity As Class)(ByVal oData As Object,
                               ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Using LinqDB
            Dim nObject As Object
            Try
                'Note: the ValidateData Function must throw a FaultException error on failure
                ValidateUpdatedRecord(LinqDB, oData)
                'Create New Record 
                nObject = CopyDTOToLinq(oData)

                'if true then do nothing. if false save changes under submit changes then call the updatelanehdmfees sp
                Dim blnHasChanged = HasLaneAddressInfoChanged(nObject.LaneControl, nObject.LaneOrigCity, nObject.LaneOrigState, nObject.LaneOrigCountry, nObject.LaneOrigZip, nObject.LaneDestCity, nObject.LaneDestState, nObject.LaneDestCountry, nObject.LaneDestZip)
                ' Attach the record 
                oLinqTable.Attach(nObject, True)
                LinqDB.SubmitChanges()

                If blnHasChanged Then UpdateLaneHDMFees(nObject.LaneControl)

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("Update"))
            End Try
            ' Return the updated order
            Return GetDTOUsingLinqTable(nObject)
        End Using
    End Function

    'Added By LVV on 9/15/16 for v-7.0.5.110 HDM Enhancement
    Public Overrides Function UpdateWithDetails(Of TEntity As Class)(ByVal oData As Object,
                                ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Using LinqDB
            Dim nObject As Object
            Try
                'Note: the ValidateData Function must throw a FaultException error on failure
                ValidateUpdatedRecord(LinqDB, oData)
                'Create New Record 
                nObject = CopyDTOToLinq(oData)

                'if true then do nothing. if false save changes under submit changes then call the updatelanehdmfees sp
                Dim blnHasChanged = HasLaneAddressInfoChanged(nObject.LaneControl, nObject.LaneOrigCity, nObject.LaneOrigState, nObject.LaneOrigCountry, nObject.LaneOrigZip, nObject.LaneDestCity, nObject.LaneDestState, nObject.LaneDestCountry, nObject.LaneDestZip)
                ' Attach the record 
                oLinqTable.Attach(nObject, True)
                ProcessUpdatedDetails(LinqDB, oData)
                LinqDB.SubmitChanges()

                If blnHasChanged Then UpdateLaneHDMFees(nObject.LaneControl)

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateWithDetails"))
            End Try
            ' Return the updated order
            Return GetDTOUsingLinqTable(nObject)
        End Using
    End Function

    'Added By LVV on 9/12/16 for v-7.0.5.110 HDM Enhancement
    Public Overrides Function UpdateQuick(Of TEntity As Class)(ByVal oData As Object,
                                ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As DTO.QuickSaveResults
        Using LinqDB
            Dim nObject As Object
            Try
                'Note: the ValidateData Function must throw a FaultException error on failure
                ValidateUpdatedRecord(LinqDB, oData)
                'Create New Record 
                nObject = CopyDTOToLinq(oData)

                'if true then do nothing. if false save changes under submit changes then call the updatelanehdmfees sp
                Dim blnHasChanged = HasLaneAddressInfoChanged(nObject.LaneControl, nObject.LaneOrigCity, nObject.LaneOrigState, nObject.LaneOrigCountry, nObject.LaneOrigZip, nObject.LaneDestCity, nObject.LaneDestState, nObject.LaneDestCountry, nObject.LaneDestZip)
                ' Attach the record 
                oLinqTable.Attach(nObject, True)
                LinqDB.SubmitChanges()

                If blnHasChanged Then UpdateLaneHDMFees(nObject.LaneControl)

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateQuick"))
            End Try
            ' Return the quick results object
            Return GetQuickSaveResults(nObject)
        End Using
    End Function

    'Added By LVV on 9/12/16 for v-7.0.5.110 HDM Enhancement
    Public Overrides Sub UpdateNoReturn(Of TEntity As Class)(ByVal oData As Object,
                                ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        Using LinqDB
            Dim nObject As Object
            Try
                'Note: the ValidateData Function must throw a FaultException error on failure
                ValidateUpdatedRecord(LinqDB, oData)
                'Create New Record 
                nObject = CopyDTOToLinq(oData)

                'if true then do nothing. if false save changes under submit changes then call the updatelanehdmfees sp
                Dim blnHasChanged = HasLaneAddressInfoChanged(nObject.LaneControl, nObject.LaneOrigCity, nObject.LaneOrigState, nObject.LaneOrigCountry, nObject.LaneOrigZip, nObject.LaneDestCity, nObject.LaneDestState, nObject.LaneDestCountry, nObject.LaneDestZip)
                ' Attach the record 
                oLinqTable.Attach(nObject, True)
                LinqDB.SubmitChanges()

                If blnHasChanged Then UpdateLaneHDMFees(nObject.LaneControl)

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateNoReturn"))
            End Try
            NoReturnCleanUp(nObject)
        End Using
    End Sub

    'Added By LVV on 9/15/16 for v-7.0.5.110 HDM Enhancement
    Public Overrides Sub UpdateWithDetailsNoReturn(Of TEntity As Class)(ByVal oData As Object,
                                    ByVal oLinqTable As System.Data.Linq.Table(Of TEntity))
        Using LinqDB
            Dim nObject As Object
            Try
                'Note: the ValidateData Function must throw a FaultException error on failure
                ValidateUpdatedRecord(LinqDB, oData)
                'Create New Record 
                nObject = CopyDTOToLinq(oData)

                'if true then do nothing. if false save changes under submit changes then call the updatelanehdmfees sp
                Dim blnHasChanged = HasLaneAddressInfoChanged(nObject.LaneControl, nObject.LaneOrigCity, nObject.LaneOrigState, nObject.LaneOrigCountry, nObject.LaneOrigZip, nObject.LaneDestCity, nObject.LaneDestState, nObject.LaneDestCountry, nObject.LaneDestZip)
                ' Attach the record 
                oLinqTable.Attach(nObject, True)
                ProcessUpdatedDetails(LinqDB, oData)

                If blnHasChanged Then UpdateLaneHDMFees(nObject.LaneControl)

            Catch ex As FaultException
                Throw
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Try
                LinqDB.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateWithDetailsNoReturn"))
            End Try
            NoReturnCleanUp(nObject)
        End Using
    End Sub

    'Added By LVV on 9/12/16 for v-7.0.5.110 HDM Enhancement
    ' Modified by RHR for V-8.5.3.001 on 06/13/2022 added new default parameters 
    ' Modified by RHR for v-8.5.4.005 on 04/04/2024 added new default parameters
    Public Overrides Function Add(Of TEntity As Class)(ByVal oData As DTO.DTOBaseClass,
                               ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As DTO.DTOBaseClass
        Using LinqDB
            Dim nObject As Object
            Try
                'Note: the ValidateData Function must throw a FaultException error on failure
                ValidateNewRecord(LinqDB, oData)
                'Get default parameter settings 



                'Create New Record 
                nObject = CopyDTOToLinq(oData)
                Dim iCompControl = DirectCast(nObject, LTS.Lane).LaneCompControl
                Dim blnInbound = DirectCast(nObject, LTS.Lane).LaneOriginAddressUse
                Dim LaneTransLeadTimeCalcTypeDefault As Integer = 0
                Dim LaneTransLeadTimeLocationOptionDefault As Integer = 0
                Dim LaneTransLeadTimeUseMasterLaneDefault As Double = 0
                Dim LaneTLTBenchmarkDefault As Integer = 0
                Dim LaneOLTBenchmarkDefault As Integer = 0

                'Modified by RHR for v-8.5.4.005 on 04/04/2024 added new default parameters
                Dim iUseLaneDefaultCarrierUseDefault As Integer = CInt(GetParValue("LaneDefaultCarrierUseDefault", iCompControl))
                Dim iUseLaneAutoTenderFlagDefault As Integer = CInt(GetParValue("LaneAutoTenderFlagDefault", iCompControl))
                Dim iUseLaneCascadingDispatchingFlagDefault As Integer = CInt(GetParValue("LaneCascadingDispatchingFlagDefault", iCompControl))
                Dim sLaneDefaultCarrierUseDefault As String = "0"
                Dim sLaneAutoTenderFlagDefault As String = "0"
                Dim sLaneCascadingDispatchingFlagDefault As String = "0"

                If (iUseLaneDefaultCarrierUseDefault = 1) Then
                    sLaneDefaultCarrierUseDefault = GetParText("LaneDefaultCarrierUseDefault", iCompControl)
                    If sLaneDefaultCarrierUseDefault = "1" Or sLaneDefaultCarrierUseDefault.ToLower() = "true" Then
                        DirectCast(nObject, LTS.Lane).LaneDefaultCarrierUse = True
                    Else
                        DirectCast(nObject, LTS.Lane).LaneDefaultCarrierUse = False
                    End If
                End If

                If (iUseLaneAutoTenderFlagDefault = 1) Then
                    sLaneAutoTenderFlagDefault = GetParText("LaneAutoTenderFlagDefault", iCompControl)
                    If sLaneAutoTenderFlagDefault = "1" Or sLaneAutoTenderFlagDefault.ToLower() = "true" Then
                        DirectCast(nObject, LTS.Lane).LaneAutoTenderFlag = True
                    Else
                        DirectCast(nObject, LTS.Lane).LaneAutoTenderFlag = False
                    End If
                End If

                If (iUseLaneCascadingDispatchingFlagDefault = 1) Then
                    sLaneCascadingDispatchingFlagDefault = GetParText("LaneCascadingDispatchingFlagDefault", iCompControl)
                    If sLaneCascadingDispatchingFlagDefault = "1" Or sLaneCascadingDispatchingFlagDefault.ToLower() = "true" Then
                        DirectCast(nObject, LTS.Lane).LaneCascadingDispatchingFlag = True
                    Else
                        DirectCast(nObject, LTS.Lane).LaneCascadingDispatchingFlag = False
                    End If
                End If


                If blnInbound.HasValue AndAlso blnInbound = True Then
                    LaneTransLeadTimeCalcTypeDefault = CInt(GetParValue("LaneTransLeadTimeInboundCalcTypeDefault", iCompControl)) 'Company level parameter for Default Lane Lead Time Calc Type on New Inbound Lanes, values 0 = None, 1 = Calculate  Ship Date, 2 = Calculate Required Date',7,0
                    LaneTransLeadTimeLocationOptionDefault = CInt(GetParValue("LaneTransLeadTimeInboundLocationOptionDefault", iCompControl)) 'Company level parameter for Default Lane Lead Time Location Option on New Inbound Lanes, values 0 = None, 1 = Compare By State, 2 = Compare By City , 3 = Compare By 3 digit postal code range',7,0 
                    LaneTransLeadTimeUseMasterLaneDefault = GetParValue("LaneTransLeadTimeInbundUseMasterLaneDefault", iCompControl) 'Company level parameter for Default Lane Lead Time Location Option on New Inbound Lanes, values 0 = Off, 1 = On',7,0  
                    LaneTLTBenchmarkDefault = CInt(GetParValue("LaneInboundTLTBenchmarkDefault", iCompControl)) 'Company level parameter for Default Lane Transit Lead Time Days on New Inbound Lanes, values = minimum transit days',7,0  
                    LaneOLTBenchmarkDefault = CInt(GetParValue("LaneInboundOLTBenchmarkDefault", iCompControl)) 'Company level parameter for Default Lane Order Lead Time Days on New Inbound Lanes, value = minimum Order or Production Lead Time days',7,0 
                Else
                    LaneTransLeadTimeCalcTypeDefault = GetParValue("LaneTransLeadTimeOutboundCalcTypeDefault", iCompControl) 'Company level parameter for Default Lane Lead Time Calc Type on New Outbound Lanes, values 0 = None, 1 = Calculate  Ship Date, 2 = Calculate Required Date
                    LaneTransLeadTimeLocationOptionDefault = GetParValue("LaneTransLeadTimeOutboundLocationOptionDefault", iCompControl) 'Company level parameter for Default Lane Lead Time Location Option on New Outbound Lanes, values 0 = None, 1 = Compare By State, 2 = Compare By City , 3 = Compare By 3 digit postal code range',7,0 
                    LaneTransLeadTimeUseMasterLaneDefault = GetParValue("LaneTransLeadTimeOutboundUseMasterLaneDefault", iCompControl) 'Company level parameter for Default Lane Lead Time Location Option on New Outbound Lanes, values 0 = Off, 1 = On',7,0  
                    LaneTLTBenchmarkDefault = CInt(GetParValue("LaneOutboundTLTBenchmarkDefault", iCompControl)) 'Company level parameter for Default Lane Transit Lead Time Days on New Outbound Lanes, value = minimum transit days',7,0  
                    LaneOLTBenchmarkDefault = CInt(GetParValue("LaneOutboundOLTBenchmarkDefault", iCompControl)) 'Company level parameter for Default Lane Order Lead Time Days on New Outbound Lanes, value = minimum Order or Production Lead Time days',7,0 
                End If

                If LaneTransLeadTimeCalcTypeDefault < 0 Or LaneTransLeadTimeCalcTypeDefault > 2 Then LaneTransLeadTimeCalcTypeDefault = 0
                DirectCast(nObject, LTS.Lane).LaneTransLeadTimeCalcType = LaneTransLeadTimeCalcTypeDefault
                If LaneTransLeadTimeLocationOptionDefault < 0 Or LaneTransLeadTimeLocationOptionDefault > 2 Then LaneTransLeadTimeLocationOptionDefault = 0
                DirectCast(nObject, LTS.Lane).LaneTransLeadTimeLocationOption = LaneTransLeadTimeLocationOptionDefault
                If LaneTransLeadTimeUseMasterLaneDefault = 1 Then
                    DirectCast(nObject, LTS.Lane).LaneTransLeadTimeUseMasterLane = True
                Else
                    DirectCast(nObject, LTS.Lane).LaneTransLeadTimeUseMasterLane = False
                End If
                DirectCast(nObject, LTS.Lane).LaneTLTBenchmark = LaneTLTBenchmarkDefault
                DirectCast(nObject, LTS.Lane).LaneOLTBenchmark = LaneOLTBenchmarkDefault

                oLinqTable.InsertOnSubmit(nObject)

                LinqDB.SubmitChanges()
                'call sp updatelanehmdfees here
                UpdateLaneHDMFees(nObject.LaneControl)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("Add"))
            End Try
            CreateCleanUp(nObject)
            Return GetDTOUsingLinqTable(nObject)
        End Using

    End Function

    'Added By LVV on 10/28/16 for v-7.0.5.110 Lane Default Carrier Enhancements
    Protected Sub CopyDTOToLTS(ByRef d As DTO.LimitLaneToCarrier, ByRef l As LTS.LimitLaneToCarrier)
        'Create New Record
        With l
            .LLTCControl = d.LLTCControl
            .LLTCLaneControl = d.LLTCLaneControl
            .LLTCCarrierControl = d.LLTCCarrierControl
            .LLTCCarrierNumber = d.LLTCCarrierNumber
            .LLTCCarrierName = d.LLTCCarrierName
            .LLTCModeTypeControl = d.LLTCModeTypeControl
            .LLTCSequenceNumber = d.LLTCSequenceNumber
            .LLTCSActive = d.LLTCSActive
            .LLTCTempType = d.LLTCTempType
            .LLTCMaxCases = d.LLTCMaxCases
            .LLTCMaxWgt = d.LLTCMaxWgt
            .LLTCMaxCube = d.LLTCMaxCube
            .LLTCMaxPL = d.LLTCMaxPL
            .LLTCTariffControl = d.LLTCTariffControl
            .LLTCTariffName = d.LLTCTariffName
            .LLTCTariffEquip = d.LLTCTariffEquip
            .LLTCMinAllowedCost = d.LLTCMinAllowedCost
            .LLTCMaxAllowedCost = d.LLTCMaxAllowedCost
            .LLTCAllowAutoAssignment = d.LLTCAllowAutoAssignment
            .LLTCIgnoreTariff = d.LLTCIgnoreTariff
            .LLTCCarrierContControl = d.LLTCCarrierContControl
            .LLTCModDate = Date.Now
            .LLTCModUser = Me.Parameters.UserName
            .LimitLaneToCarrierUpdated = If(d.LimitLaneToCarrierUpdated Is Nothing, New Byte() {}, d.LimitLaneToCarrierUpdated)
        End With

    End Sub

#Region "TMS 365"

    Friend Shared Function SelectLTSData(ByVal d As LTS.vLELane365) As LTS.Lane
        Dim oLTS As New LTS.Lane
        Dim skipObjs As New List(Of String) From {"LaneUpdated", "rowguid", "_Lane", "Lane", "_tblRouteType", "tblRouteType", "Page", "Pages", "RecordCount", "PageSize"}
        oLTS = CopyMatchingFields(oLTS, d, skipObjs)
        'add custom formatting
        With oLTS
            .LaneUpdated = d.LaneUpdated.ToArray()
            '.LaneUpdated = d.LaneUpdated
        End With
        Return oLTS
    End Function

    Friend Shared Sub CopyToLTSData(ByRef l As LTS.Lane, ByVal d As LTS.vLELane365)
        Dim skipObjs As New List(Of String) From {"LaneUpdated", "rowguid", "_Lane", "Lane", "_tblRouteType", "tblRouteType", "Page", "Pages", "RecordCount", "PageSize"}
        l = CopyMatchingFields(l, d, skipObjs)
        'add custom formatting
        l.LaneUpdated = d.LaneUpdated.ToArray()
        'l.LaneUpdated = d.LaneUpdated
    End Sub

#End Region

#End Region


#Region "Scrub Methods"


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="iCompControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 02/19/2021 
    '''     fixed bug where X.LaneActive = 1 converts to negative 1 instead of true
    '''     we now use X.LaneActive = True
    ''' </remarks>
    Public Function Scrub(Optional ByVal iCompControl As Integer = 0) As Integer
        Dim iProcessed As Integer = 0
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim dblCaseType As Double = 0
                Dim oLanes = db.Lanes.Where(Function(X) X.LaneActive = True And (iCompControl = 0 OrElse X.LaneCompControl = iCompControl)).ToArray()
                If Not oLanes Is Nothing AndAlso oLanes.Count() > 0 Then
                    Dim oAKAList = db.AKARefLanes.ToArray()
                    If Not oAKAList Is Nothing AndAlso oAKAList.Count() > 0 Then
                        iProcessed = oLanes.Count()
                        For Each oAKA In oAKAList
                            Dim strValue As String = oAKA.AKAValue.ToString
                            Dim arrAKAs As String() = {Trim(oAKA.AKA1),
                                Trim(oAKA.AKA2),
                                Trim(oAKA.AKA3),
                                Trim(oAKA.AKA4),
                                Trim(oAKA.AKA5),
                                Trim(oAKA.AKA6),
                                Trim(oAKA.AKA7),
                                Trim(oAKA.AKA8),
                                Trim(oAKA.AKA9),
                                Trim(oAKA.AKA10)}
                            For i As Integer = 0 To 9
                                Dim strAKA As String = arrAKAs(i)
                                If strAKA.Length > 0 Then
                                    For Each oLane In oLanes
                                        dblCaseType = GetParValue("GlobalAddressScrubbingCapitalCaseType", oLane.LaneCompControl)
                                        ScrubLane(strAKA, strValue, oLane, dblCaseType)
                                    Next
                                End If
                            Next
                        Next
                    End If
                End If
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("Scrub"), db)
            End Try
        End Using
        Return iProcessed
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="iCompControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 02/19/2021 
    '''     fixed bug where X.LaneActive = 1 converts to negative 1 instead of true
    '''     we now use X.LaneActive = True
    ''' </remarks>
    Public Function CaseType(Optional ByVal iCompControl As Integer = 0) As Integer
        Dim iProcessed As Integer = 0
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim dblCaseType As Double = 0
                Dim oLanes = db.Lanes.Where(Function(X) X.LaneActive = True And (iCompControl = 0 OrElse X.LaneCompControl = iCompControl)).ToArray()
                If Not oLanes Is Nothing AndAlso oLanes.Count() > 0 Then
                    iProcessed = oLanes.Count()
                    For Each oLane In oLanes
                        dblCaseType = GetParValue("GlobalAddressScrubbingCapitalCaseType", oLane.LaneCompControl)
                        Select Case dblCaseType
                            Case 0
                                oLane.LaneOrigAddress1 = Utilities.UpperFirst(oLane.LaneOrigAddress1)
                                oLane.LaneOrigAddress2 = Utilities.UpperFirst(oLane.LaneOrigAddress2)
                                oLane.LaneOrigAddress3 = Utilities.UpperFirst(oLane.LaneOrigAddress3)
                                oLane.LaneOrigCity = Utilities.UpperFirst(oLane.LaneOrigCity)
                                oLane.LaneOrigState = oLane.LaneOrigState.ToUpper
                                oLane.LaneOrigCountry = oLane.LaneOrigCountry.ToUpper
                                oLane.LaneOrigZip = oLane.LaneOrigZip.ToUpper
                                oLane.LaneDestAddress1 = Utilities.UpperFirst(oLane.LaneDestAddress1)
                                oLane.LaneDestAddress2 = Utilities.UpperFirst(oLane.LaneDestAddress2)
                                oLane.LaneDestAddress3 = Utilities.UpperFirst(oLane.LaneDestAddress3)
                                oLane.LaneDestCity = Utilities.UpperFirst(oLane.LaneDestCity)
                                oLane.LaneDestState = oLane.LaneDestState.ToUpper
                                oLane.LaneDestCountry = oLane.LaneDestCountry.ToUpper
                                oLane.LaneDestZip = oLane.LaneDestZip.ToUpper
                            Case 1
                                oLane.LaneOrigAddress1 = oLane.LaneOrigAddress1.ToUpper
                                oLane.LaneOrigAddress2 = oLane.LaneOrigAddress2.ToUpper
                                oLane.LaneOrigAddress3 = oLane.LaneOrigAddress3.ToUpper
                                oLane.LaneOrigCity = oLane.LaneOrigCity.ToUpper
                                oLane.LaneOrigState = oLane.LaneOrigState.ToUpper
                                oLane.LaneOrigCountry = oLane.LaneOrigCountry.ToUpper
                                oLane.LaneOrigZip = oLane.LaneOrigZip.ToUpper
                                oLane.LaneDestAddress1 = oLane.LaneDestAddress1.ToUpper
                                oLane.LaneDestAddress2 = oLane.LaneDestAddress2.ToUpper
                                oLane.LaneDestAddress3 = oLane.LaneDestAddress3.ToUpper
                                oLane.LaneDestCity = oLane.LaneDestCity.ToUpper
                                oLane.LaneDestState = oLane.LaneDestState.ToUpper
                                oLane.LaneDestCountry = oLane.LaneDestCountry.ToUpper
                                oLane.LaneDestZip = oLane.LaneDestZip.ToUpper
                            Case Else
                                oLane.LaneOrigAddress1 = oLane.LaneOrigAddress1.ToLower
                                oLane.LaneOrigAddress2 = oLane.LaneOrigAddress2.ToLower
                                oLane.LaneOrigAddress3 = oLane.LaneOrigAddress3.ToLower
                                oLane.LaneOrigCity = oLane.LaneOrigCity.ToLower
                                oLane.LaneOrigState = oLane.LaneOrigState.ToLower
                                oLane.LaneOrigCountry = oLane.LaneOrigCountry.ToLower
                                oLane.LaneOrigZip = oLane.LaneOrigZip.ToLower
                                oLane.LaneDestAddress1 = oLane.LaneDestAddress1.ToLower
                                oLane.LaneDestAddress2 = oLane.LaneDestAddress2.ToLower
                                oLane.LaneDestAddress3 = oLane.LaneDestAddress3.ToLower
                                oLane.LaneDestCity = oLane.LaneDestCity.ToLower
                                oLane.LaneDestState = oLane.LaneDestState.ToLower
                                oLane.LaneDestCountry = oLane.LaneDestCountry.ToLower
                                oLane.LaneDestZip = oLane.LaneDestZip.ToLower
                        End Select
                    Next
                End If
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CaseType"), db)
            End Try
        End Using
        Return iProcessed
    End Function


    Public Sub ScrubLane(ByVal strAKA As String, ByVal strValue As String, ByRef oLane As LTS.Lane, ByVal dblCaseType As Double)
        Dim strAddress As String = " " & oLane.LaneOrigAddress1.ToLower & " "
        strAKA = strAKA.ToLower
        strValue = strValue.ToLower
        If InStr(strAddress, " " & Trim(strAKA) & " ", CompareMethod.Text) > 0 Then
            oLane.LaneOrigAddress1 = strAddress.Replace(" " & Trim(strAKA) & " ", " " & strValue & " ")
            If dblCaseType = 1 Then oLane.LaneOrigAddress1 = oLane.LaneOrigAddress1.ToUpper
            If dblCaseType = 0 Then oLane.LaneOrigAddress1 = Utilities.UpperFirst(oLane.LaneOrigAddress1)
        End If
    End Sub

#End Region


End Class

Public Class NGLLaneCalData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLaneDataContext(ConnectionString)
        Me.LinqTable = db.LaneCals
        Me.LinqDB = db
        Me.SourceClass = "NGLLaneCalData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLaneDataContext(ConnectionString)
            Me.LinqTable = db.LaneCals
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
        Return GetLaneCalFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetLaneCalsFiltered()
    End Function

    Public Function GetLaneCalFiltered(ByVal Control As Integer) As DTO.LaneCal
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim LaneCal As DTO.LaneCal = (
                From d In db.LaneCals
                Where
                    d.LaneCalControl = Control
                Select selectDTOData(d)).First


                Return LaneCal

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetLaneCalsFiltered(Optional ByVal LaneControl As Integer = 0) As DTO.LaneCal()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim LaneCals() As DTO.LaneCal = (
                From d In db.LaneCals
                Where
                    (d.LaneCalLaneControl = If(LaneControl = 0, d.LaneCalLaneControl, LaneControl))
                Order By d.LaneCalControl
                Select selectDTOData(d)).ToArray()
                Return LaneCals

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Sub InsertOrUpdateLaneCal70(ByVal LaneLegalEntity As String,
                                            ByVal LaneNumber As String,
                                            ByVal LaneCompAlphaCode As String,
                                            ByVal Month As Integer,
                                            ByVal MonthAllowUpdate As Boolean,
                                            ByVal Day As Integer,
                                            ByVal DayAllowUpdate As Boolean,
                                            ByVal Open As Boolean,
                                            ByVal OpenAllowUpdate As Boolean,
                                            ByVal StartTime As String,
                                            ByVal StartTimeAllowUpdate As Boolean,
                                            ByVal EndTime As String,
                                            ByVal EndTimeAllowUpdate As Boolean,
                                            ByVal IsHoliday As Boolean,
                                            ByVal IsHolidayAllowUpdate As Boolean,
                                            ByVal ApplyToOrigin As Boolean,
                                            ByVal ApplyToOriginAllowUpdate As Boolean)

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oRet = db.spInsertOrUpdateLaneCal70(LaneLegalEntity, LaneNumber, LaneCompAlphaCode, Month, MonthAllowUpdate, Day, DayAllowUpdate, Open, OpenAllowUpdate, StartTime, StartTimeAllowUpdate, EndTime, EndTimeAllowUpdate, IsHoliday, IsHolidayAllowUpdate, ApplyToOrigin, ApplyToOriginAllowUpdate).ToList()
                If Not oRet Is Nothing AndAlso oRet.Count() > 0 AndAlso oRet(0).ErrNumber <> 0 Then
                    throwSQLFaultException(oRet(0).RetMsg)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateLaneCal70"))
            End Try
        End Using
    End Sub

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.LaneCal)
        'Create New Record
        Return New LTS.LaneCal With {.LaneCalControl = d.LaneCalControl _
                                  , .LaneCalLaneControl = d.LaneCalLaneControl _
                                  , .LaneCalMonth = d.LaneCalMonth _
                                  , .LaneCalDay = d.LaneCalDay _
                                  , .LaneCalOpen = d.LaneCalOpen _
                                  , .LaneCalStartTime = d.LaneCalStartTime _
                                  , .LaneCalEndTime = d.LaneCalEndTime _
                                  , .LaneCalIsHoliday = d.LaneCalIsHoliday _
                                  , .LaneCalApplyToOrigin = d.LaneCalApplyToOrigin _
                                  , .LaneCalUpdated = If(d.LaneCalUpdated Is Nothing, New Byte() {}, d.LaneCalUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetLaneCalFiltered(Control:=CType(LinqTable, LTS.LaneCal).LaneCalControl)
    End Function

    Friend Function selectDTOData(ByVal d As LTS.LaneCal, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.LaneCal

        Return New DTO.LaneCal With {.LaneCalControl = d.LaneCalControl _
                                              , .LaneCalLaneControl = d.LaneCalLaneControl _
                                              , .LaneCalMonth = d.LaneCalMonth _
                                              , .LaneCalDay = d.LaneCalDay _
                                              , .LaneCalOpen = d.LaneCalOpen _
                                              , .LaneCalStartTime = d.LaneCalStartTime _
                                              , .LaneCalEndTime = d.LaneCalEndTime _
                                              , .LaneCalIsHoliday = d.LaneCalIsHoliday _
                                              , .LaneCalApplyToOrigin = d.LaneCalApplyToOrigin _
                                              , .LaneCalUpdated = d.LaneCalUpdated.ToArray() _
                                              , .Page = page _
                                              , .Pages = pagecount _
                                              , .RecordCount = recordcount _
                                              , .PageSize = pagesize}
    End Function

#End Region

End Class

Public Class NGLLaneCarrData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLaneDataContext(ConnectionString)
        Me.LinqTable = db.LaneCarrs
        Me.LinqDB = db
        Me.SourceClass = "NGLLaneCarrData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLaneDataContext(ConnectionString)
            Me.LinqTable = db.LaneCarrs
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
        Return GetLaneCarrFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetLaneCarrsFiltered()
    End Function

    Public Function GetLaneCarrFiltered(ByVal Control As Integer) As DTO.LaneCarr
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim LaneCarr As DTO.LaneCarr = (
                From d In db.LaneCarrs
                Where
                    d.LaneCarrControl = Control
                Select New DTO.LaneCarr With {.LaneCarrControl = d.LaneCarrControl _
                                              , .LaneCarrLaneControl = d.LaneCarrLaneControl _
                                              , .LaneCarrCarrierControl = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrWgtFrom = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrWgtTo = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrRateStarts = d.LaneCarrRateStarts _
                                              , .LaneCarrRateExpires = d.LaneCarrRateExpires _
                                              , .LaneCarrTL = d.LaneCarrTL _
                                              , .LaneCarrLTL = d.LaneCarrLTL _
                                              , .LaneCarrEquipment = d.LaneCarrEquipment _
                                              , .LaneCarrMileRate = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrCwtRate = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrCaseRate = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrFlatRate = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrPltRate = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrCubeRate = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrTLT = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrTMode = d.LaneCarrTMode _
                                              , .LaneCarrFAK = d.LaneCarrFAK _
                                              , .LaneCarrDisc = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrPUMon = d.LaneCarrPUMon _
                                              , .LaneCarrPUTue = d.LaneCarrPUTue _
                                              , .LaneCarrPUWed = d.LaneCarrPUWed _
                                              , .LaneCarrPUThu = d.LaneCarrPUThu _
                                              , .LaneCarrPUFri = d.LaneCarrPUFri _
                                              , .LaneCarrPUSat = d.LaneCarrPUSat _
                                              , .LaneCarrPUSun = d.LaneCarrPUSun _
                                              , .LaneCarrDLMon = d.LaneCarrDLMon _
                                              , .LaneCarrDLTue = d.LaneCarrDLTue _
                                              , .LaneCarrDLWed = d.LaneCarrDLWed _
                                              , .LaneCarrDLThu = d.LaneCarrDLThu _
                                              , .LaneCarrDLFri = d.LaneCarrDLFri _
                                              , .LaneCarrDLSat = d.LaneCarrDLSat _
                                              , .LaneCarrDLSun = d.LaneCarrDLSun _
                                              , .LaneCarrPayTolPerLo = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrPayTolPerHi = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrPayTolCurLo = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrPayTolCurHi = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrCurType = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrModUser = d.LaneCarrModUser _
                                              , .LaneCarrModDate = d.LaneCarrModDate _
                                              , .LaneCarrRoute = d.LaneCarrRoute _
                                              , .LaneCarrPltsOpen = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrPltsCommitted = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrPltsAvailable = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrMiles = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrBkhlCostPerc = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrPalletCostPer = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrFuelSurChargePerc = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrStopCharge = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrDropCost = d.LaneCarrDropCost _
                                              , .LaneCarrUnloadDiff = d.LaneCarrUnloadDiff _
                                              , .LaneCarrCasesAvailable = d.LaneCarrCasesAvailable _
                                              , .LaneCarrCasesOpen = d.LaneCarrCasesOpen _
                                              , .LaneCarrCasesCommitted = d.LaneCarrCasesCommitted _
                                              , .LaneCarrWgtAvailable = d.LaneCarrWgtAvailable _
                                              , .LaneCarrWgtOpen = d.LaneCarrWgtOpen _
                                              , .LaneCarrWgtCommitted = d.LaneCarrWgtCommitted _
                                              , .LaneCarrCubesAvailable = d.LaneCarrCubesAvailable _
                                              , .LaneCarrCubesOpen = d.LaneCarrCubesOpen _
                                              , .LaneCarrCubesCommitted = d.LaneCarrCubesCommitted _
                                              , .LaneCarrCarrierTruckControl = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrLockSettings = d.LaneCarrLockSettings _
                                              , .LaneCarrUpdated = d.LaneCarrUpdated.ToArray()}).First


                Return LaneCarr

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetLaneCarrsFiltered(Optional ByVal LaneControl As Integer = 0) As DTO.LaneCarr()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim LaneCarrs() As DTO.LaneCarr = (
                From d In db.LaneCarrs
                Where
                    (d.LaneCarrLaneControl = If(LaneControl = 0, d.LaneCarrLaneControl, LaneControl))
                Order By d.LaneCarrControl
                Select New DTO.LaneCarr With {.LaneCarrControl = d.LaneCarrControl _
                                              , .LaneCarrLaneControl = d.LaneCarrLaneControl _
                                              , .LaneCarrCarrierControl = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrWgtFrom = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrWgtTo = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrRateStarts = d.LaneCarrRateStarts _
                                              , .LaneCarrRateExpires = d.LaneCarrRateExpires _
                                              , .LaneCarrTL = d.LaneCarrTL _
                                              , .LaneCarrLTL = d.LaneCarrLTL _
                                              , .LaneCarrEquipment = d.LaneCarrEquipment _
                                              , .LaneCarrMileRate = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrCwtRate = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrCaseRate = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrFlatRate = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrPltRate = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrCubeRate = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrTLT = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrTMode = d.LaneCarrTMode _
                                              , .LaneCarrFAK = d.LaneCarrFAK _
                                              , .LaneCarrDisc = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrPUMon = d.LaneCarrPUMon _
                                              , .LaneCarrPUTue = d.LaneCarrPUTue _
                                              , .LaneCarrPUWed = d.LaneCarrPUWed _
                                              , .LaneCarrPUThu = d.LaneCarrPUThu _
                                              , .LaneCarrPUFri = d.LaneCarrPUFri _
                                              , .LaneCarrPUSat = d.LaneCarrPUSat _
                                              , .LaneCarrPUSun = d.LaneCarrPUSun _
                                              , .LaneCarrDLMon = d.LaneCarrDLMon _
                                              , .LaneCarrDLTue = d.LaneCarrDLTue _
                                              , .LaneCarrDLWed = d.LaneCarrDLWed _
                                              , .LaneCarrDLThu = d.LaneCarrDLThu _
                                              , .LaneCarrDLFri = d.LaneCarrDLFri _
                                              , .LaneCarrDLSat = d.LaneCarrDLSat _
                                              , .LaneCarrDLSun = d.LaneCarrDLSun _
                                              , .LaneCarrPayTolPerLo = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrPayTolPerHi = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrPayTolCurLo = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrPayTolCurHi = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrCurType = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrModUser = d.LaneCarrModUser _
                                              , .LaneCarrModDate = d.LaneCarrModDate _
                                              , .LaneCarrRoute = d.LaneCarrRoute _
                                              , .LaneCarrPltsOpen = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrPltsCommitted = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrPltsAvailable = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrMiles = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrBkhlCostPerc = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrPalletCostPer = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrFuelSurChargePerc = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrStopCharge = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrDropCost = d.LaneCarrDropCost _
                                              , .LaneCarrUnloadDiff = d.LaneCarrUnloadDiff _
                                              , .LaneCarrCasesAvailable = d.LaneCarrCasesAvailable _
                                              , .LaneCarrCasesOpen = d.LaneCarrCasesOpen _
                                              , .LaneCarrCasesCommitted = d.LaneCarrCasesCommitted _
                                              , .LaneCarrWgtAvailable = d.LaneCarrWgtAvailable _
                                              , .LaneCarrWgtOpen = d.LaneCarrWgtOpen _
                                              , .LaneCarrWgtCommitted = d.LaneCarrWgtCommitted _
                                              , .LaneCarrCubesAvailable = d.LaneCarrCubesAvailable _
                                              , .LaneCarrCubesOpen = d.LaneCarrCubesOpen _
                                              , .LaneCarrCubesCommitted = d.LaneCarrCubesCommitted _
                                              , .LaneCarrCarrierTruckControl = If(d.LaneCarrCarrierControl.HasValue, d.LaneCarrCarrierControl.Value, 0) _
                                              , .LaneCarrLockSettings = d.LaneCarrLockSettings _
                                              , .LaneCarrUpdated = d.LaneCarrUpdated.ToArray()}).ToArray()
                Return LaneCarrs

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.LaneCarr)
        'Create New Record
        Return New LTS.LaneCarr With {.LaneCarrControl = d.LaneCarrControl _
                                      , .LaneCarrLaneControl = d.LaneCarrLaneControl _
                                      , .LaneCarrCarrierControl = d.LaneCarrCarrierControl _
                                      , .LaneCarrWgtFrom = d.LaneCarrCarrierControl _
                                      , .LaneCarrWgtTo = d.LaneCarrCarrierControl _
                                      , .LaneCarrRateStarts = d.LaneCarrRateStarts _
                                      , .LaneCarrRateExpires = d.LaneCarrRateExpires _
                                      , .LaneCarrTL = d.LaneCarrTL _
                                      , .LaneCarrLTL = d.LaneCarrLTL _
                                      , .LaneCarrEquipment = d.LaneCarrEquipment _
                                      , .LaneCarrMileRate = d.LaneCarrCarrierControl _
                                      , .LaneCarrCwtRate = d.LaneCarrCarrierControl _
                                      , .LaneCarrCaseRate = d.LaneCarrCarrierControl _
                                      , .LaneCarrFlatRate = d.LaneCarrCarrierControl _
                                      , .LaneCarrPltRate = d.LaneCarrCarrierControl _
                                      , .LaneCarrCubeRate = d.LaneCarrCarrierControl _
                                      , .LaneCarrTLT = d.LaneCarrCarrierControl _
                                      , .LaneCarrTMode = d.LaneCarrTMode _
                                      , .LaneCarrFAK = d.LaneCarrFAK _
                                      , .LaneCarrDisc = d.LaneCarrCarrierControl _
                                      , .LaneCarrPUMon = d.LaneCarrPUMon _
                                      , .LaneCarrPUTue = d.LaneCarrPUTue _
                                      , .LaneCarrPUWed = d.LaneCarrPUWed _
                                      , .LaneCarrPUThu = d.LaneCarrPUThu _
                                      , .LaneCarrPUFri = d.LaneCarrPUFri _
                                      , .LaneCarrPUSat = d.LaneCarrPUSat _
                                      , .LaneCarrPUSun = d.LaneCarrPUSun _
                                      , .LaneCarrDLMon = d.LaneCarrDLMon _
                                      , .LaneCarrDLTue = d.LaneCarrDLTue _
                                      , .LaneCarrDLWed = d.LaneCarrDLWed _
                                      , .LaneCarrDLThu = d.LaneCarrDLThu _
                                      , .LaneCarrDLFri = d.LaneCarrDLFri _
                                      , .LaneCarrDLSat = d.LaneCarrDLSat _
                                      , .LaneCarrDLSun = d.LaneCarrDLSun _
                                      , .LaneCarrPayTolPerLo = d.LaneCarrCarrierControl _
                                      , .LaneCarrPayTolPerHi = d.LaneCarrCarrierControl _
                                      , .LaneCarrPayTolCurLo = d.LaneCarrCarrierControl _
                                      , .LaneCarrPayTolCurHi = d.LaneCarrCarrierControl _
                                      , .LaneCarrCurType = d.LaneCarrCarrierControl _
                                      , .LaneCarrModUser = Parameters.UserName _
                                      , .LaneCarrModDate = Date.Now _
                                      , .LaneCarrRoute = d.LaneCarrRoute _
                                      , .LaneCarrPltsOpen = d.LaneCarrCarrierControl _
                                      , .LaneCarrPltsCommitted = d.LaneCarrCarrierControl _
                                      , .LaneCarrPltsAvailable = d.LaneCarrCarrierControl _
                                      , .LaneCarrMiles = d.LaneCarrCarrierControl _
                                      , .LaneCarrBkhlCostPerc = d.LaneCarrCarrierControl _
                                      , .LaneCarrPalletCostPer = d.LaneCarrCarrierControl _
                                      , .LaneCarrFuelSurChargePerc = d.LaneCarrCarrierControl _
                                      , .LaneCarrStopCharge = d.LaneCarrCarrierControl _
                                      , .LaneCarrDropCost = d.LaneCarrDropCost _
                                      , .LaneCarrUnloadDiff = d.LaneCarrUnloadDiff _
                                      , .LaneCarrCasesAvailable = d.LaneCarrCasesAvailable _
                                      , .LaneCarrCasesOpen = d.LaneCarrCasesOpen _
                                      , .LaneCarrCasesCommitted = d.LaneCarrCasesCommitted _
                                      , .LaneCarrWgtAvailable = d.LaneCarrWgtAvailable _
                                      , .LaneCarrWgtOpen = d.LaneCarrWgtOpen _
                                      , .LaneCarrWgtCommitted = d.LaneCarrWgtCommitted _
                                      , .LaneCarrCubesAvailable = d.LaneCarrCubesAvailable _
                                      , .LaneCarrCubesOpen = d.LaneCarrCubesOpen _
                                      , .LaneCarrCubesCommitted = d.LaneCarrCubesCommitted _
                                      , .LaneCarrCarrierTruckControl = d.LaneCarrCarrierControl _
                                      , .LaneCarrLockSettings = d.LaneCarrLockSettings _
                                      , .LaneCarrUpdated = If(d.LaneCarrUpdated Is Nothing, New Byte() {}, d.LaneCarrUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetLaneCarrFiltered(Control:=CType(LinqTable, LTS.LaneCarr).LaneCarrControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim source As LTS.LaneCarr = TryCast(LinqTable, LTS.LaneCarr)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.LaneCarrs
                       Where d.LaneCarrControl = source.LaneCarrControl
                       Select New DTO.QuickSaveResults With {.Control = d.LaneCarrControl _
                                                            , .ModDate = d.LaneCarrModDate _
                                                            , .ModUser = d.LaneCarrModUser _
                                                            , .Updated = d.LaneCarrUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

#End Region

End Class

Public Class NGLLaneFeeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLaneDataContext(ConnectionString)
        Me.LinqTable = db.LaneFees
        Me.LinqDB = db
        Me.SourceClass = "NGLLaneFeeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLaneDataContext(ConnectionString)
            Me.LinqTable = db.LaneFees
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
        Return GetLaneFeeFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetLaneFeesFiltered()
    End Function

    Public Function GetLaneFeeFiltered(ByVal Control As Integer) As DTO.LaneFee
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim LaneFee As DTO.LaneFee = (
                From d In db.LaneFees
                Where
                    d.LaneFeesControl = Control
                Select selectDTOData(d)).First


                Return LaneFee

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetLaneFeesFiltered(Optional ByVal LaneControl As Integer = 0) As DTO.LaneFee()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim LaneFees() As DTO.LaneFee = (
                From d In db.LaneFees
                Where
                    (d.LaneFeesLaneControl = LaneControl)
                Order By d.LaneFeesControl
                Select selectDTOData(d)).ToArray()
                Return LaneFees

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#Region "LTS Methods"

    ''' <summary>
    ''' sample Lane Fees LTS query reading data from a table
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.3.0.001 on 07/08/2020
    ''' </remarks>
    Public Function GetLaneFees365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vLELaneFee()
        If filters Is Nothing Then Return Nothing
        Dim iLaneControl As Integer = 0 'Parent Control Number
        Dim iLaneFeesControl As Integer = 0 'table primary key
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""

        Dim oRet() As LTS.vLELaneFee 'return the table or view
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                If (filters.FilterValues Is Nothing OrElse filters.FilterValues.Count() < 1) OrElse (Not filters.FilterValues.Any(Function(x) x.filterName = "LaneFeesControl")) Then
                    'The Record Control Filter does not exist so use the parent control fliter
                    If filters.ParentControl = 0 Then
                        throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
                    End If
                    iLaneControl = filters.ParentControl
                    filterWhere = " (LaneFeesLaneControl = " & iLaneControl.ToString() & ") "
                    sFilterSpacer = " And "
                Else
                    Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "LaneFeesControl").FirstOrDefault()
                    Integer.TryParse(tFilter.filterValueFrom, iLaneFeesControl)
                End If

                Dim iQuery As IQueryable(Of LTS.vLELaneFee)
                iQuery = db.vLELaneFees
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "LaneFeesCaption"
                    filters.sortDirection = "ASC"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneFees365"), db)
            End Try
            Return oRet
        End Using
    End Function


    Public Function GetLaneFee365(ByVal LaneFeesControl As Integer) As LTS.vLELaneFee

        Dim oRet As LTS.vLELaneFee 'return the table or view
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                oRet = db.vLELaneFees.Where(Function(x) x.LaneFeesControl = LaneFeesControl).FirstOrDefault() 'added firstordefault to fix edit error Manorama' 16-Jun-2020
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneFee365"), db)
            End Try
            Return oRet
        End Using
    End Function

    Public Function SaveOrCreateLaneFeeItem(ByVal oData As LTS.LaneFee, Optional ByVal iLaneFeeControl As Integer = 0) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do


        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim iLaneFeeLaneControl = oData.LaneFeesLaneControl
                If oData.LaneFeesLaneControl = 0 Then
                    If iLaneFeeControl = 0 Then
                        Dim sMsg As String = "E_MissingParent" ' The reference to the parent record is missing. Please select a valid parent record and try again.
                        throwNoDataFaultException(sMsg)
                    End If

                End If

                '    Dim blnProcessed As Boolean = False
                oData.LaneFeesModDate = Date.Now()
                oData.LaneFeesModUser = Me.Parameters.UserName

                If oData.LaneFeesControl = 0 Then
                    db.LaneFees.InsertOnSubmit(oData)
                Else
                    db.LaneFees.Attach(oData, True)
                End If
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveOrCreateLaneFeeItem"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteLaneFeeItem(ByVal iLaneFeeControl As Integer) As Boolean
        Dim blnRet As Boolean = False

        If iLaneFeeControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.LaneFees.Where(Function(x) x.LaneFeesControl = iLaneFeeControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.LaneFeesControl = 0 Then Return True
                db.LaneFees.DeleteOnSubmit(oExisting)
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteLaneFeeItem"), db)
            End Try
        End Using
        Return blnRet
    End Function
#End Region

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.LaneFee)
        'Create New Record
        Return New LTS.LaneFee With {.LaneFeesControl = d.LaneFeesControl _
                                    , .LaneFeesLaneControl = d.LaneFeesLaneControl _
                                    , .LaneFeesMinimum = d.LaneFeesMinimum _
                                    , .LaneFeesVariable = d.LaneFeesVariable _
                                    , .LaneFeesAccessorialCode = d.LaneFeesAccessorialCode _
                                    , .LaneFeesVariableCode = d.LaneFeesVariableCode _
                                    , .LaneFeesVisible = d.LaneFeesVisible _
                                    , .LaneFeesAutoApprove = d.LaneFeesAutoApprove _
                                    , .LaneFeesAllowCarrierUpdates = d.LaneFeesAllowCarrierUpdates _
                                    , .LaneFeesCaption = d.LaneFeesCaption _
                                    , .LaneFeesEDICode = d.LaneFeesEDICode _
                                    , .LaneFeesTaxable = d.LaneFeesTaxable _
                                    , .LaneFeesIsTax = d.LaneFeesIsTax _
                                    , .LaneFeesTaxSortOrder = d.LaneFeesTaxSortOrder _
                                    , .LaneFeesBOLText = d.LaneFeesBOLText _
                                    , .LaneFeesBOLPlacement = d.LaneFeesBOLPlacement _
                                    , .LaneFeesAccessorialFeeAllocationTypeControl = d.LaneFeesAccessorialFeeAllocationTypeControl _
                                    , .LaneFeesTarBracketTypeControl = d.LaneFeesTarBracketTypeControl _
                                    , .LaneFeesAccessorialFeeCalcTypeControl = d.LaneFeesAccessorialFeeCalcTypeControl _
                                    , .LaneFeesModDate = Date.Now _
                                    , .LaneFeesModUser = Parameters.UserName _
                                    , .LaneFeesUpdated = If(d.LaneFeesUpdated Is Nothing, New Byte() {}, d.LaneFeesUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetLaneFeeFiltered(Control:=CType(LinqTable, LTS.LaneFee).LaneFeesControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim source As LTS.LaneFee = TryCast(LinqTable, LTS.LaneFee)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.LaneFees
                       Where d.LaneFeesControl = source.LaneFeesControl
                       Select New DTO.QuickSaveResults With {.Control = d.LaneFeesControl _
                                                            , .ModDate = d.LaneFeesModDate _
                                                            , .ModUser = d.LaneFeesModUser _
                                                            , .Updated = d.LaneFeesUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

    Friend Function selectDTOData(ByVal d As LTS.LaneFee, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.LaneFee

        Return New DTO.LaneFee With {.LaneFeesControl = d.LaneFeesControl _
                                    , .LaneFeesLaneControl = If(d.LaneFeesLaneControl.HasValue, d.LaneFeesLaneControl.Value, 0) _
                                    , .LaneFeesMinimum = If(d.LaneFeesMinimum.HasValue, d.LaneFeesMinimum.Value, 0) _
                                    , .LaneFeesVariable = If(d.LaneFeesVariable.HasValue, d.LaneFeesVariable.Value, 0) _
                                    , .LaneFeesAccessorialCode = If(d.LaneFeesAccessorialCode.HasValue, d.LaneFeesAccessorialCode.Value, 0) _
                                    , .LaneFeesVariableCode = If(d.LaneFeesVariableCode, 0) _
                                    , .LaneFeesVisible = d.LaneFeesVisible _
                                    , .LaneFeesAutoApprove = d.LaneFeesAutoApprove _
                                    , .LaneFeesAllowCarrierUpdates = d.LaneFeesAllowCarrierUpdates _
                                    , .LaneFeesCaption = d.LaneFeesCaption _
                                    , .LaneFeesEDICode = d.LaneFeesEDICode _
                                    , .LaneFeesTaxable = d.LaneFeesTaxable _
                                    , .LaneFeesIsTax = d.LaneFeesIsTax _
                                    , .LaneFeesTaxSortOrder = d.LaneFeesTaxSortOrder _
                                    , .LaneFeesBOLText = d.LaneFeesBOLText _
                                    , .LaneFeesBOLPlacement = d.LaneFeesBOLPlacement _
                                    , .LaneFeesAccessorialFeeAllocationTypeControl = d.LaneFeesAccessorialFeeAllocationTypeControl _
                                    , .LaneFeesTarBracketTypeControl = d.LaneFeesTarBracketTypeControl _
                                    , .LaneFeesAccessorialFeeCalcTypeControl = d.LaneFeesAccessorialFeeCalcTypeControl _
                                    , .LaneFeesModDate = d.LaneFeesModDate _
                                    , .LaneFeesModUser = d.LaneFeesModUser _
                                    , .LaneFeesUpdated = d.LaneFeesUpdated.ToArray() _
                                    , .Page = page _
                                    , .Pages = pagecount _
                                    , .RecordCount = recordcount _
                                    , .PageSize = pagesize}
    End Function

#End Region

End Class

Public Class NGLLaneSecData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLaneDataContext(ConnectionString)
        Me.LinqTable = db.LaneSecs
        Me.LinqDB = db
        Me.SourceClass = "NGLLaneSecData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLaneDataContext(ConnectionString)
            Me.LinqTable = db.LaneSecs
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
        Return GetLaneSecFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetLaneSecsFiltered()
    End Function

    Public Function GetLaneSecFiltered(ByVal Control As Integer) As DTO.LaneSec
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim LaneSec As DTO.LaneSec = (
                From d In db.LaneSecs
                Where
                    d.LaneSecControl = Control
                Select selectDTOData(d)).First


                Return LaneSec

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetLaneSecsFiltered(Optional ByVal LaneControl As Integer = 0) As DTO.LaneSec()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim LaneSecs() As DTO.LaneSec = (
                From d In db.LaneSecs
                Where
                    (d.LaneSecLaneControl = If(LaneControl = 0, d.LaneSecLaneControl, LaneControl))
                Order By d.LaneSecControl
                Select selectDTOData(d)).ToArray()
                If LaneSecs Is Nothing OrElse LaneSecs.Count < 1 Then
                    'we need to add a new record because there is an expected 1 to 1 relationship with the lane table
                    LaneSecs = New DTO.LaneSec() {CreateDefautLaneSecRecord(LaneControl)}
                End If
                Return LaneSecs

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                'if we get here we need to add a new record because one does not exist.  1 to 1 relationship.               
                Return New DTO.LaneSec() {CreateDefautLaneSecRecord(LaneControl)}
                'Utilities.SaveAppError(ex.Message, Me.Parameters)
                'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.LaneSec)
        'Create New Record
        Return New LTS.LaneSec With {.LaneSecControl = d.LaneSecControl _
                                      , .LaneSecLaneControl = d.LaneSecLaneControl _
                                      , .LaneSecPUName = d.LaneSecPUName _
                                      , .LaneSecPUAddress1 = d.LaneSecPUAddress1 _
                                      , .LaneSecPUAddress2 = d.LaneSecPUAddress2 _
                                      , .LaneSecPUAddress3 = d.LaneSecPUAddress3 _
                                      , .LaneSecPUCity = d.LaneSecPUCity _
                                      , .LaneSecPUState = d.LaneSecPUState _
                                      , .LaneSecPUCountry = d.LaneSecPUCountry _
                                      , .LaneSecPUZip = d.LaneSecPUZip _
                                      , .LaneSecPUContactPhone = d.LaneSecPUContactPhone _
                                      , .LaneSecPUContactFax = d.LaneSecPUContactFax _
                                      , .LaneSecBrokerNumber = d.LaneSecBrokerNumber _
                                      , .LaneSecBrokerName = d.LaneSecBrokerName _
                                      , .LaneSecBrokerAddress1 = d.LaneSecBrokerAddress1 _
                                      , .LaneSecBrokerAddress2 = d.LaneSecBrokerAddress2 _
                                      , .LaneSecBrokerAddress3 = d.LaneSecBrokerAddress3 _
                                      , .LaneSecBrokerCity = d.LaneSecBrokerCity _
                                      , .LaneSecBrokerState = d.LaneSecBrokerState _
                                      , .LaneSecBrokerCountry = d.LaneSecBrokerCountry _
                                      , .LaneSecBrokerZip = d.LaneSecBrokerZip _
                                      , .LaneSecBrokerContactPhone = d.LaneSecBrokerContactPhone _
                                      , .LaneSecBrokerContactFax = d.LaneSecBrokerContactFax _
                                      , .LaneSecBrokerContactName = d.LaneSecBrokerContactName _
                                      , .LaneSecBrokerOpHourStart = d.LaneSecBrokerOpHourStart _
                                      , .LaneSecBrokerOpHourStop = d.LaneSecBrokerOpHourStop _
                                      , .LaneSecComment = d.LaneSecComment _
                                      , .LaneSecModDate = Date.Now _
                                      , .LaneSecModUser = Parameters.UserName _
                                      , .LaneSecUpdated = If(d.LaneSecUpdated Is Nothing, New Byte() {}, d.LaneSecUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetLaneSecFiltered(Control:=CType(LinqTable, LTS.LaneSec).LaneSecControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim source As LTS.LaneSec = TryCast(LinqTable, LTS.LaneSec)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.LaneSecs
                       Where d.LaneSecControl = source.LaneSecControl
                       Select New DTO.QuickSaveResults With {.Control = d.LaneSecControl _
                                                            , .ModDate = d.LaneSecModDate _
                                                            , .ModUser = d.LaneSecModUser _
                                                            , .Updated = d.LaneSecUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

    Private Function CreateDefautLaneSecRecord(ByVal LaneControl As Integer) As DTO.LaneSec
        Dim oNewData As New DTO.LaneSec
        oNewData.LaneSecLaneControl = LaneControl
        Return Me.CreateRecord(oNewData)
    End Function

    Friend Function selectDTOData(ByVal d As LTS.LaneSec, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.LaneSec

        Return New DTO.LaneSec With {.LaneSecControl = d.LaneSecControl _
                                    , .LaneSecLaneControl = If(d.LaneSecLaneControl.HasValue, d.LaneSecLaneControl.Value, 0) _
                                    , .LaneSecPUName = d.LaneSecPUName _
                                    , .LaneSecPUAddress1 = d.LaneSecPUAddress1 _
                                    , .LaneSecPUAddress2 = d.LaneSecPUAddress2 _
                                    , .LaneSecPUAddress3 = d.LaneSecPUAddress3 _
                                    , .LaneSecPUCity = d.LaneSecPUCity _
                                    , .LaneSecPUState = d.LaneSecPUState _
                                    , .LaneSecPUCountry = d.LaneSecPUCountry _
                                    , .LaneSecPUZip = d.LaneSecPUZip _
                                    , .LaneSecPUContactPhone = d.LaneSecPUContactPhone _
                                    , .LaneSecPUContactFax = d.LaneSecPUContactFax _
                                    , .LaneSecBrokerNumber = d.LaneSecBrokerNumber _
                                    , .LaneSecBrokerName = d.LaneSecBrokerName _
                                    , .LaneSecBrokerAddress1 = d.LaneSecBrokerAddress1 _
                                    , .LaneSecBrokerAddress2 = d.LaneSecBrokerAddress2 _
                                    , .LaneSecBrokerAddress3 = d.LaneSecBrokerAddress3 _
                                    , .LaneSecBrokerCity = d.LaneSecBrokerCity _
                                    , .LaneSecBrokerState = d.LaneSecBrokerState _
                                    , .LaneSecBrokerCountry = d.LaneSecBrokerCountry _
                                    , .LaneSecBrokerZip = d.LaneSecBrokerZip _
                                    , .LaneSecBrokerContactPhone = d.LaneSecBrokerContactPhone _
                                    , .LaneSecBrokerContactFax = d.LaneSecBrokerContactFax _
                                    , .LaneSecBrokerContactName = d.LaneSecBrokerContactName _
                                    , .LaneSecBrokerOpHourStart = d.LaneSecBrokerOpHourStart _
                                    , .LaneSecBrokerOpHourStop = d.LaneSecBrokerOpHourStop _
                                    , .LaneSecComment = d.LaneSecComment _
                                    , .LaneSecModDate = d.LaneSecModDate _
                                    , .LaneSecModUser = d.LaneSecModUser _
                                    , .LaneSecUpdated = d.LaneSecUpdated.ToArray() _
                                    , .Page = page _
                                    , .Pages = pagecount _
                                    , .RecordCount = recordcount _
                                    , .PageSize = pagesize}
    End Function

#End Region

End Class

Public Class NGLLaneTransLoadXrefData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLaneDataContext(ConnectionString)
        Me.LinqTable = db.LaneTransLoadXrefs
        Me.LinqDB = db
        Me.SourceClass = "NGLLaneTransLoadXrefData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLaneDataContext(ConnectionString)
            Me.LinqTable = db.LaneTransLoadXrefs
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

    Private _NGLXDetData As NGLLaneTransLoadXrefDetData
    Public ReadOnly Property NGLXDetData() As NGLLaneTransLoadXrefDetData
        Get
            If _NGLXDetData Is Nothing Then _NGLXDetData = Me.NDPBaseClassFactory("NGLLaneTransLoadXrefDetData", False)
            Return _NGLXDetData
        End Get
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetLaneTransLoadXrefFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetLaneTransLoadXrefNextSequenceForMode(ByVal LaneControl As Integer, ByVal modetypecontrol As Integer) As DTO.GenericResults
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oRet = (From d In db.vLaneTransLoadXrefs
                            Where d.LaneTranXLaneControl = LaneControl And
                               d.LaneTranXModeTypeControl = modetypecontrol
                            Order By d.LaneTranXModeTypeControl, d.LaneTranXSequence
                            Select d.LaneTranXSequence).Max
                Dim gr As New DTO.GenericResults
                gr.Data = oRet + 1
                Return gr
            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                'nothing was found.
                Dim gr As New DTO.GenericResults
                gr.Data = 1
                Return gr
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetLaneTransLoadXrefsLaneModeFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetLaneTransLoadXrefFiltered(ByVal Control As Integer) As DTO.LaneTransLoadXref
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Return (From d In db.LaneTransLoadXrefs
                        Where d.LaneTranXControl = Control
                        Select selectDTOData(d)).First
            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetLaneTransLoadXrefFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetLaneTransLoadXrefsFiltered(ByVal LaneControl As Integer) As DTO.LaneTransLoadXref()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Return (From d In db.LaneTransLoadXrefs
                        Where d.LaneTranXLaneControl = LaneControl
                        Order By d.LaneTranXModeTypeControl, d.LaneTranXSequence
                        Select selectDTOData(d)).ToArray()
            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetLaneTransLoadXrefsFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetLaneTransLoadXrefsLaneModeFiltered(ByVal LaneControl As Integer, ByVal modetypecontrol As Integer) As DTO.LaneTransLoadXref()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'Return (From d In db.LaneTransLoadXrefs _
                '        Where d.LaneTranXLaneControl = LaneControl And _
                '          d.LaneTranXModeTypeControl = modetypecontrol _
                '        Order By d.LaneTranXModeTypeControl, d.LaneTranXSequence _
                '        Select selectDTOData(d)).ToArray()

                Dim oRet As DTO.LaneTransLoadXref() = (From d In db.vLaneTransLoadXrefs
                                                       Where d.LaneTranXLaneControl = LaneControl And
                                                          d.LaneTranXModeTypeControl = modetypecontrol
                                                       Order By d.LaneTranXModeTypeControl, d.LaneTranXSequence
                                                       Select New DTO.LaneTransLoadXref With {.LaneTranXControl = d.LaneTranXControl _
                                                                                             , .LaneTranXName = d.LaneTranXName _
                                                                                             , .LaneTranXLaneName = d.LaneTranXLaneName _
                                                                                             , .LaneTranXLaneNumber = d.LaneTranXLaneNumber _
                                                                                             , .LaneTranXLaneControl = d.LaneTranXLaneControl _
                                                                                             , .LaneTranXModeTypeName = d.LaneTranXModeTypeName _
                                                                                             , .LaneTranXModeTypeControl = d.LaneTranXModeTypeControl _
                                                                                             , .LaneTranXSequence = d.LaneTranXSequence _
                                                                                             , .LaneTranXTempTypeName = d.LaneTranXTempTypeName _
                                                                                             , .LaneTranXTempType = d.LaneTranXTempType _
                                                                                             , .LaneTranXMinCases = d.LaneTranXMinCases _
                                                                                             , .LaneTranXMinWgt = d.LaneTranXMinWgt _
                                                                                             , .LaneTranXMinCube = d.LaneTranXMinCube _
                                                                                             , .LaneTranXMinPL = d.LaneTranXMinPL _
                                                                                             , .LaneTranXMaxCases = d.LaneTranXMaxCases _
                                                                                             , .LaneTranXMaxWgt = d.LaneTranXMaxWgt _
                                                                                             , .LaneTranXMaxCube = d.LaneTranXMaxCube _
                                                                                             , .LaneTranXMaxPL = d.LaneTranXMaxPL _
                                                                                             , .LaneTranXBenchMiles = d.LaneTranXBenchMiles _
                                                                                             , .LaneTranXUser1 = d.LaneTranXUser1 _
                                                                                             , .LaneTranXUser2 = d.LaneTranXUser2 _
                                                                                             , .LaneTranXUser3 = d.LaneTranXUser3 _
                                                                                             , .LaneTranXUser4 = d.LaneTranXUser4 _
                                                                                             , .LaneTranXModDate = d.LaneTranXModDate _
                                                                                             , .LaneTranXModUser = d.LaneTranXModUser _
                                                                                             , .LaneTranXUpdated = d.LaneTranXUpdated.ToArray() _
                                                                                            , .Page = 1 _
                                                                                            , .Pages = 1 _
                                                                                            , .RecordCount = 1 _
                                                                                            , .PageSize = 1000}).Distinct().ToArray()
                Return oRet
            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetLaneTransLoadXrefsLaneModeFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetLaneTransLoadXrefFilteredWDetails(ByVal Control As Integer) As DTO.LaneTransLoadXref
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.LaneTransLoadXref)(Function(t As LTS.LaneTransLoadXref) t.LaneTransLoadXrefDets)
                db.LoadOptions = oDLO
                Return (From d In db.LaneTransLoadXrefs
                        Where d.LaneTranXControl = Control
                        Select selectDTODataWDetails(d)).First
            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetLaneTransLoadXrefFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetLaneTransLoadXrefsFilteredWDetails(ByVal LaneControl As Integer) As DTO.LaneTransLoadXref()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.LaneTransLoadXref)(Function(t As LTS.LaneTransLoadXref) t.LaneTransLoadXrefDets)
                db.LoadOptions = oDLO
                Return (From d In db.LaneTransLoadXrefs
                        Where d.LaneTranXLaneControl = LaneControl
                        Order By d.LaneTranXModeTypeControl, d.LaneTranXSequence
                        Select selectDTODataWDetails(d)).ToArray()
            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetLaneTransLoadXrefsFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function UpdateListLaneTransLoadXrefs(ByVal olist As DTO.LaneTransLoadXref()) As DTO.LaneTransLoadXref()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                For Each item As DTO.LaneTransLoadXref In olist
                    db.LaneTransLoadXrefs.Attach(CopyDTOToLinq(item), True)
                Next
                db.SubmitChanges()

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetLaneTransLoadXrefsFiltered"))
            End Try
            Return Nothing
        End Using
    End Function


    ''' <summary>
    ''' Read the LaneTransLoad Configuration
    ''' </summary>
    ''' <param name="LaneControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.004 on 01/28/2024
    '''     Convert nullable booleans to false
    '''     .LaneTranXDetBilledSeperately = If(dtx.LaneTranXDetBilledSeperately,False)
    '''     .LaneTranXDetConsolidateSplits = If(dtx.LaneTranXDetConsolidateSplits,False)
    ''' </remarks>
    Public Function GetLaneTransLoadXrefsFilteredWDetailsAndText(ByVal LaneControl As Integer) As DTO.LaneTransLoadXref()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter


                Dim oRet As DTO.LaneTransLoadXref() = (From d In db.vLaneTransLoadXrefs Where d.LaneTranXLaneControl = LaneControl
                                                       Select New DTO.LaneTransLoadXref With {.LaneTranXControl = d.LaneTranXControl _
                                                                                             , .LaneTranXName = d.LaneTranXName _
                                                                                             , .LaneTranXLaneName = d.LaneTranXLaneName _
                                                                                             , .LaneTranXLaneNumber = d.LaneTranXLaneNumber _
                                                                                             , .LaneTranXLaneControl = d.LaneTranXLaneControl _
                                                                                             , .LaneTranXModeTypeName = d.LaneTranXModeTypeName _
                                                                                             , .LaneTranXModeTypeControl = d.LaneTranXModeTypeControl _
                                                                                             , .LaneTranXSequence = d.LaneTranXSequence _
                                                                                             , .LaneTranXTempTypeName = d.LaneTranXTempTypeName _
                                                                                             , .LaneTranXTempType = d.LaneTranXTempType _
                                                                                             , .LaneTranXMinCases = d.LaneTranXMinCases _
                                                                                             , .LaneTranXMinWgt = d.LaneTranXMinWgt _
                                                                                             , .LaneTranXMinCube = d.LaneTranXMinCube _
                                                                                             , .LaneTranXMinPL = d.LaneTranXMinPL _
                                                                                             , .LaneTranXMaxCases = d.LaneTranXMaxCases _
                                                                                             , .LaneTranXMaxWgt = d.LaneTranXMaxWgt _
                                                                                             , .LaneTranXMaxCube = d.LaneTranXMaxCube _
                                                                                             , .LaneTranXMaxPL = d.LaneTranXMaxPL _
                                                                                             , .LaneTranXBenchMiles = d.LaneTranXBenchMiles _
                                                                                             , .LaneTranXUser1 = d.LaneTranXUser1 _
                                                                                             , .LaneTranXUser2 = d.LaneTranXUser2 _
                                                                                             , .LaneTranXUser3 = d.LaneTranXUser3 _
                                                                                             , .LaneTranXUser4 = d.LaneTranXUser4 _
                                                                                             , .LaneTranXModDate = d.LaneTranXModDate _
                                                                                             , .LaneTranXModUser = d.LaneTranXModUser _
                                                                                             , .LaneTranXUpdated = d.LaneTranXUpdated.ToArray() _
                                                                                             , .LaneTransLoadXrefDets = (From dtx In db.vLaneTransLoadXrefDets
                                                                                                                         Where dtx.LaneTranXDetLaneTranXControl = d.LaneTranXControl
                                                                                                                         Order By dtx.LaneTranXDetSequence Ascending
                                                                                                                         Select New DTO.LaneTransLoadXrefDet With {.LaneTranXDetControl = dtx.LaneTranXDetControl _
                                                                                                                         , .LaneTranXDetName = dtx.LaneTranXDetName _
                                                                                                                        , .LaneTranXDetLaneTranXControl = dtx.LaneTranXDetLaneTranXControl _
                                                                                                                        , .LaneTranXDetLaneName = dtx.LaneTranXDetLaneName _
                                                                                                                        , .LaneTranXDetLaneNumber = dtx.LaneTranXDetLaneNumber _
                                                                                                                        , .LaneTranXDetLaneControl = dtx.LaneTranXDetLaneControl _
                                                                                                                        , .LaneTranXDetSequence = dtx.LaneTranXDetSequence _
                                                                                                                        , .LaneTranXDetCarrierName = dtx.LaneTranXDetCarrierName _
                                                                                                                        , .LaneTranXDetCarrierNumber = dtx.LaneTranXDetCarrierNumber _
                                                                                                                        , .LaneTranXDetCarrierControl = dtx.LaneTranXDetCarrierControl _
                                                                                                                        , .LaneTranXDetCarrierContControl = dtx.LaneTranXDetCarrierContControl _
                                                                                                                        , .LaneTranXDetContName = dtx.LaneTranXDetContName _
                                                                                                                        , .LaneTranXDetContPhone = dtx.LaneTranXDetContPhone _
                                                                                                                        , .LaneTranXDetContExt = dtx.LaneTranXDetContExt _
                                                                                                                        , .LaneTranXDetCont800 = dtx.LaneTranXDetCont800 _
                                                                                                                        , .LaneTranXDetModeTypeControl = dtx.LaneTranXDetModeTypeControl _
                                                                                                                        , .LaneTranXDetModeTypeName = dtx.LaneTranXDetModeTypeName _
                                                                                                                        , .LaneTranXDetCarrTarControl = dtx.LaneTranXDetCarrTarControl _
                                                                                                                        , .LaneTranXDetCarrTarName = dtx.LaneTranXDetCarrTarName _
                                                                                                                        , .LaneTranXDetCarrTarEquipControl = dtx.LaneTranXDetCarrTarEquipControl _
                                                                                                                        , .LaneTranXDetCarrTarEquipName = dtx.LaneTranXDetCarrTarEquipName _
                                                                                                                        , .LaneTranXDetRule11Required = dtx.LaneTranXDetRule11Required _
                                                                                                                        , .LaneTranXDetBilledSeperately = If(dtx.LaneTranXDetBilledSeperately, False) _
                                                                                                                        , .LaneTranXDetConsolidateSplits = If(dtx.LaneTranXDetConsolidateSplits, False) _
                                                                                                                        , .LaneTranXDetTransType = dtx.LaneTranXDetTransType _
                                                                                                                        , .LaneTranXDetTransTypeName = dtx.LaneTranXDetTransTypeName _
                                                                                                                        , .LaneTranXDetBFC = dtx.LaneTranXDetBFC _
                                                                                                                        , .LaneTranXDetBFCType = dtx.LaneTranXDetBFCType _
                                                                                                                        , .LaneTranXDetBenchMiles = dtx.LaneTranXDetBenchMiles _
                                                                                                                        , .LaneTranXDetUser1 = dtx.LaneTranXDetUser1 _
                                                                                                                        , .LaneTranXDetUser2 = dtx.LaneTranXDetUser2 _
                                                                                                                        , .LaneTranXDetUser3 = dtx.LaneTranXDetUser3 _
                                                                                                                        , .LaneTranXDetUser4 = dtx.LaneTranXDetUser4 _
                                                                                                                        , .LaneTranXDetTransitHours = dtx.LaneTranXDetTransitHours _
                                                                                                                        , .LaneTranXDetTransferHours = dtx.LaneTranXDetTransferHours _
                                                                                                                        , .LaneTranXDetModDate = dtx.LaneTranXDetModDate _
                                                                                                                        , .LaneTranXDetModUser = dtx.LaneTranXDetModUser _
                                                                                                                        , .LaneTranXDetUpdated = dtx.LaneTranXDetUpdated.ToArray() _
                                                                                                                         , .Page = 1 _
                                                                                                                         , .Pages = 1 _
                                                                                                                         , .RecordCount = 1 _
                                                                                                                         , .PageSize = 100}).ToList() _
                                                                                            , .Page = 1 _
                                                                                            , .Pages = 1 _
                                                                                            , .RecordCount = 1 _
                                                                                            , .PageSize = 1000}).Distinct().ToArray()








                'Dim oRet As DTO.LaneTransLoadXref() = (From d In db.LaneTransLoadXrefs _
                '        Join det In db.LaneTransLoadXrefDets On d.LaneTranXControl Equals det.LaneTranXDetLaneTranXControl _
                '        Join l In db.Lanes On d.LaneTranXLaneControl Equals l.LaneControl _
                '        Join ld In db.Lanes On det.LaneTranXDetLaneControl Equals ld.LaneControl _
                '        Join m In db.tblModeTypeRefLanes On d.LaneTranXModeTypeControl Equals m.ModeTypeControl _
                '        Join md In db.tblModeTypeRefLanes On det.LaneTranXDetModeTypeControl Equals md.ModeTypeControl _
                '        Join t In db.LaneTranRefLanes On det.LaneTranXDetTransType Equals t.LaneTransNumber _
                '        From tmp In db.TempTypeRefLanes.Where(Function(x) x.ID = d.LaneTranXTempType).DefaultIfEmpty() _
                '        From tar In db.CarrierTariffRefLanes.Where(Function(x) x.CarrTarControl = det.LaneTranXDetCarrTarControl).DefaultIfEmpty() _
                '        From eq In db.CarrierTariffEquipmentRefLanes.Where(Function(x) x.CarrTarEquipControl = det.LaneTranXDetCarrTarEquipControl).DefaultIfEmpty() _
                '        From c In db.CarrierRefLanes.Where(Function(x) x.CarrierControl = det.LaneTranXDetCarrierControl).DefaultIfEmpty() _
                '        From cnt In db.CarrierContRefLanes.Where(Function(x) x.CarrierContControl = det.LaneTranXDetCarrierContControl).DefaultIfEmpty() _
                '        Where d.LaneTranXLaneControl = LaneControl _
                '        Order By d.LaneTranXModeTypeControl, d.LaneTranXSequence _
                '        Select New DTO.LaneTransLoadXref With {.LaneTranXControl = d.LaneTranXControl _
                '                         , .LaneTranXName = d.LaneTranXName _
                '                         , .LaneTranXLaneControl = d.LaneTranXLaneControl _
                '                         , .LaneTranXModeTypeControl = d.LaneTranXModeTypeControl _
                '                         , .LaneTranXSequence = d.LaneTranXSequence _
                '                         , .LaneTranXTempType = d.LaneTranXTempType _
                '                         , .LaneTranXMinCases = d.LaneTranXMinCases _
                '                         , .LaneTranXMinWgt = d.LaneTranXMinWgt _
                '                         , .LaneTranXMinCube = d.LaneTranXMinCube _
                '                         , .LaneTranXMinPL = d.LaneTranXMinPL _
                '                         , .LaneTranXMaxCases = d.LaneTranXMaxCases _
                '                         , .LaneTranXMaxWgt = d.LaneTranXMaxWgt _
                '                         , .LaneTranXMaxCube = d.LaneTranXMaxCube _
                '                         , .LaneTranXMaxPL = d.LaneTranXMaxPL _
                '                         , .LaneTranXBenchMiles = d.LaneTranXBenchMiles _
                '                         , .LaneTranXUser1 = d.LaneTranXUser1 _
                '                         , .LaneTranXUser2 = d.LaneTranXUser2 _
                '                         , .LaneTranXUser3 = d.LaneTranXUser3 _
                '                         , .LaneTranXUser4 = d.LaneTranXUser4 _
                '                         , .LaneTranXModDate = d.LaneTranXModDate _
                '                         , .LaneTranXModUser = d.LaneTranXModUser _
                '                         , .LaneTranXUpdated = d.LaneTranXUpdated.ToArray() _
                '                         , .LaneTransLoadXrefDets = (From dtx In db.LaneTransLoadXrefDets
                '                                                      Select _
                '                                                     New DTO.LaneTransLoadXrefDet With {.LaneTranXDetControl = dtx.LaneTranXDetControl _
                '                                                 , .LaneTranXDetName = dtx.LaneTranXDetName _
                '                                                 , .LaneTranXDetLaneTranXControl = dtx.LaneTranXDetLaneTranXControl _
                '                                                 , .LaneTranXDetLaneControl = dtx.LaneTranXDetLaneControl _
                '                                                 , .LaneTranXDetSequence = dtx.LaneTranXDetSequence _
                '                                                 , .LaneTranXDetCarrierControl = dtx.LaneTranXDetCarrierControl _
                '                                                 , .LaneTranXDetCarrierContControl = dtx.LaneTranXDetCarrierContControl _
                '                                                 , .LaneTranXDetModeTypeControl = dtx.LaneTranXDetModeTypeControl _
                '                                                 , .LaneTranXDetCarrTarControl = dtx.LaneTranXDetCarrTarControl _
                '                                                 , .LaneTranXDetCarrTarEquipControl = dtx.LaneTranXDetCarrTarEquipControl _
                '                                                 , .LaneTranXDetRule11Required = dtx.LaneTranXDetRule11Required _
                '                                                 , .LaneTranXDetBilledSeperately = dtx.LaneTranXDetBilledSeperately _
                '                                                 , .LaneTranXDetConsolidateSplits = dtx.LaneTranXDetConsolidateSplits _
                '                                                 , .LaneTranXDetTransType = dtx.LaneTranXDetTransType _
                '                                                 , .LaneTranXDetBFC = dtx.LaneTranXDetBFC _
                '                                                 , .LaneTranXDetBFCType = dtx.LaneTranXDetBFCType _
                '                                                 , .LaneTranXDetBenchMiles = dtx.LaneTranXDetBenchMiles _
                '                                                 , .LaneTranXDetUser1 = dtx.LaneTranXDetUser1 _
                '                                                 , .LaneTranXDetUser2 = dtx.LaneTranXDetUser2 _
                '                                                 , .LaneTranXDetUser3 = dtx.LaneTranXDetUser3 _
                '                                                 , .LaneTranXDetUser4 = dtx.LaneTranXDetUser4 _
                '                                                 , .LaneTranXDetModDate = dtx.LaneTranXDetModDate _
                '                                                 , .LaneTranXDetModUser = dtx.LaneTranXDetModUser _
                '                                                 , .LaneTranXDetUpdated = dtx.LaneTranXDetUpdated.ToArray() _
                '                                                 , .Page = 1 _
                '                                                 , .Pages = 1 _
                '                                                 , .RecordCount = 1 _
                '                                                 , .PageSize = 100}).ToList() _
                '                         , .Page = 1 _
                '                         , .Pages = 1 _
                '                         , .RecordCount = 1 _
                '                         , .PageSize = 1000}).Distinct().ToArray()

                Return oRet

                'Select selectDTODataWDetails(d, det, l, ld, m, md, t, tmp, tar, eq, c, cnt)).Distinct().ToArray()


            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetLaneTransLoadXrefsFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Overrides UpdateWithDetails allows unique keys to be processed in order of 
    ''' Delete, Update then Insert to avoid confilcts
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="oData"></param>
    ''' <param name="oLinqTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function UpdateWithDetails(Of TEntity As Class)(ByVal oData As Object,
                                ByVal oLinqTable As System.Data.Linq.Table(Of TEntity)) As Object
        Using LinqDB
            Dim nObject As Object
            Try
                'Note: the ValidateData Function must throw a FaultException error on failure
                ValidateUpdatedRecord(LinqDB, oData)
                'Create New Record 
                nObject = CopyDTOToLinq(oData)
                ' Attach the record 
                oLinqTable.Attach(nObject, True)
                'When updates to details can violate unique keys we must process deletes, then updates then inserts seperately
                'ProcessUpdatedDetails(LinqDB, oData)
                'processf any deleted records
                Dim deletedLoadDetails = GetLaneTransLoadXrefDetChanges(oData, TrackingInfo.Deleted)
                If deletedLoadDetails.Count > 0 Then
                    With CType(LinqDB, NGLMASLaneDataContext)
                        .LaneTransLoadXrefDets.AttachAll(deletedLoadDetails, True)
                        .LaneTransLoadXrefDets.DeleteAllOnSubmit(deletedLoadDetails)
                    End With
                    LinqDB.SubmitChanges()
                End If
                'process any updates
                With CType(LinqDB, NGLMASLaneDataContext)
                    .LaneTransLoadXrefDets.AttachAll(GetLaneTransLoadXrefDetChanges(oData, TrackingInfo.Updated), True)
                End With

                LinqDB.SubmitChanges()
                'process any inserts
                With CType(LinqDB, NGLMASLaneDataContext)
                    .LaneTransLoadXrefDets.InsertAllOnSubmit(GetLaneTransLoadXrefDetChanges(oData, TrackingInfo.Created))
                End With
                LinqDB.SubmitChanges()
            Catch ex As SqlException
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateWithDetails"))
            End Try
            ' Return the updated order
            Return GetDTOUsingLinqTable(nObject)
        End Using
    End Function

#Region "LTS Methods"

    ''' <summary>
    ''' Query Lane TransLoadXrefs LTS data using AllFilters Object for Rest Services
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.001 on 02/16/2021
    ''' </remarks>
    Public Function GetLaneTransLoadXrefs365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vLaneTransLoadXref()
        If filters Is Nothing Then Return Nothing
        Dim iLaneControl As Integer = 0 'Parent Control Number
        Dim iLaneTranXControl As Integer = 0 'table primary key
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""

        Dim oRet() As LTS.vLaneTransLoadXref 'return the table or view
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                If (filters.FilterValues Is Nothing OrElse filters.FilterValues.Count() < 1) OrElse (Not filters.FilterValues.Any(Function(x) x.filterName = "LaneTranXControl")) Then
                    'The Record Control Filter does not exist so use the parent control fliter
                    If filters.ParentControl = 0 Then
                        throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
                    End If
                    iLaneControl = filters.ParentControl
                    filterWhere = " (LaneTranXLaneControl = " & iLaneControl.ToString() & ") "
                    sFilterSpacer = " And "
                Else
                    Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "LaneTranXControl").FirstOrDefault()
                    Integer.TryParse(tFilter.filterValueFrom, iLaneTranXControl)
                End If

                Dim iQuery As IQueryable(Of LTS.vLaneTransLoadXref)
                iQuery = db.vLaneTransLoadXrefs
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "LaneTranXName"
                    filters.sortDirection = "ASC"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneTransLoadXrefs365"), db)
            End Try
            Return oRet
        End Using
    End Function

    ''' <summary>
    ''' read a specific  Lane TransLoadXrefs LTS record using the primary key
    ''' </summary>
    ''' <param name="LaneTranXControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.001 on 02/16/2021
    ''' </remarks>
    Public Function GetLaneTransLoadXref365(ByVal LaneTranXControl As Integer) As LTS.vLaneTransLoadXref

        Dim oRet As New LTS.vLaneTransLoadXref() 'return the table or view
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                oRet = db.vLaneTransLoadXrefs.Where(Function(x) x.LaneTranXControl = LaneTranXControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneTransLoadXref365"), db)
            End Try
            Return oRet
        End Using
    End Function

    Public Function SaveOrCreateLaneTransLoadXrefItem(ByVal oData As LTS.LaneTransLoadXref, Optional ByVal iLaneTranXLaneControl As Integer = 0) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do


        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'LaneTranXLaneControl
                Dim iLaneTranXControl = oData.LaneTranXControl
                Dim tblData = New LTS.LaneTransLoadXref()
                If iLaneTranXControl = 0 Then
                    'oData.LaneTranXLaneControl = 0
                    If oData.LaneTranXLaneControl = 0 Then
                        If iLaneTranXLaneControl = 0 Then
                            Dim sMsg As String = "E_MissingParent" ' "  The reference to the parent record is missing. Please select a valid lane record from the Lane page and try again."
                            throwNoDataFaultException(sMsg)
                        End If
                        oData.LaneTranXLaneControl = iLaneTranXLaneControl
                    End If

                End If
                '    Dim blnProcessed As Boolean = False
                oData.LaneTranXModDate = Date.Now()
                oData.LaneTranXModUser = Me.Parameters.UserName


                If oData.LaneTranXControl = 0 Then
                    db.LaneTransLoadXrefs.InsertOnSubmit(oData)
                Else
                    db.LaneTransLoadXrefs.Attach(oData, True)
                End If
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveOrCreateLaneTransLoadXrefItem"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteLaneTransLoadXrefItem(ByVal iLaneTranXControl As Integer) As Boolean
        Dim blnRet As Boolean = False

        If iLaneTranXControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                Dim oExisting = db.LaneTransLoadXrefs.Where(Function(x) x.LaneTranXControl = iLaneTranXControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.LaneTranXControl = 0 Then Return True
                db.LaneTransLoadXrefs.DeleteOnSubmit(oExisting)
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteLaneTransLoadXrefItem"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.LaneTransLoadXref)
        'Create New Record
        Return New LTS.LaneTransLoadXref With {.LaneTranXControl = d.LaneTranXControl _
                                                 , .LaneTranXName = d.LaneTranXName _
                                                 , .LaneTranXLaneControl = d.LaneTranXLaneControl _
                                                 , .LaneTranXModeTypeControl = d.LaneTranXModeTypeControl _
                                                 , .LaneTranXSequence = d.LaneTranXSequence _
                                                 , .LaneTranXTempType = d.LaneTranXTempType _
                                                 , .LaneTranXMinCases = d.LaneTranXMinCases _
                                                 , .LaneTranXMinWgt = d.LaneTranXMinWgt _
                                                 , .LaneTranXMinCube = d.LaneTranXMinCube _
                                                 , .LaneTranXMinPL = d.LaneTranXMinPL _
                                                 , .LaneTranXMaxCases = d.LaneTranXMaxCases _
                                                 , .LaneTranXMaxWgt = d.LaneTranXMaxWgt _
                                                 , .LaneTranXMaxCube = d.LaneTranXMaxCube _
                                                 , .LaneTranXMaxPL = d.LaneTranXMaxPL _
                                                 , .LaneTranXBenchMiles = d.LaneTranXBenchMiles _
                                                 , .LaneTranXUser1 = d.LaneTranXUser1 _
                                                 , .LaneTranXUser2 = d.LaneTranXUser2 _
                                                 , .LaneTranXUser3 = d.LaneTranXUser3 _
                                                 , .LaneTranXUser4 = d.LaneTranXUser4 _
                                                 , .LaneTranXModDate = Date.Now _
                                                 , .LaneTranXModUser = Parameters.UserName _
                                                 , .LaneTranXUpdated = If(d.LaneTranXUpdated Is Nothing, New Byte() {}, d.LaneTranXUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetLaneTransLoadXrefFiltered(Control:=CType(LinqTable, LTS.LaneTransLoadXref).LaneTranXControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim source As LTS.LaneTransLoadXref = TryCast(LinqTable, LTS.LaneTransLoadXref)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.LaneTransLoadXrefs
                       Where d.LaneTranXControl = source.LaneTranXControl
                       Select New DTO.QuickSaveResults With {.Control = d.LaneTranXControl _
                                                            , .ModDate = d.LaneTranXModDate _
                                                            , .ModUser = d.LaneTranXModUser _
                                                            , .Updated = d.LaneTranXUpdated.ToArray}).FirstOrDefault()
                If ret Is Nothing OrElse ret.Control = 0 Then throwNoDataFaultException()

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetQuickSaveResults"))
            End Try

        End Using
        Return ret
    End Function

    ''' <summary>
    ''' Test record details for specified change type.  
    ''' If Updated is null, set to byte[0] (for inserted items)
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="changeType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function GetLaneTransLoadXrefDetChanges(ByVal source As DTO.LaneTransLoadXref, ByVal changeType As TrackingInfo) As List(Of LTS.LaneTransLoadXrefDet)
        Dim oList As List(Of LTS.LaneTransLoadXrefDet) = (From d In source.LaneTransLoadXrefDets Where d.TrackingState = changeType Select NGLXDetData.GetLTSFromDTO(d)).ToList()
        Return oList
    End Function

    ''' <summary>
    ''' Called by AddWithDetails from the base class
    ''' Executes AddRange for the child table data
    ''' </summary>
    ''' <param name="LinqTable"></param>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub AddDetailsToLinq(ByRef LinqTable As Object, ByRef oData As DTO.DTOBaseClass)
        CType(LinqTable, LTS.LaneTransLoadXref).LaneTransLoadXrefDets.AddRange(From d In CType(oData, DTO.LaneTransLoadXref).LaneTransLoadXrefDets Select NGLXDetData.GetLTSFromDTO(d))
    End Sub

    ''' <summary>
    ''' Called by AddWithDetails from the base class.
    ''' Executes InsertAllOnSubmit for the child table.
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="LinqTable"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub InsertAllDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef LinqTable As Object)
        CType(oDB, NGLMASLaneDataContext).LaneTransLoadXrefDets.InsertAllOnSubmit(CType(LinqTable, LTS.LaneTransLoadXref).LaneTransLoadXrefDets)
    End Sub

    Friend Function selectDTOData(ByVal d As LTS.LaneTransLoadXref, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.LaneTransLoadXref

        Return New DTO.LaneTransLoadXref With {.LaneTranXControl = d.LaneTranXControl _
                                         , .LaneTranXName = d.LaneTranXName _
                                         , .LaneTranXLaneControl = d.LaneTranXLaneControl _
                                         , .LaneTranXModeTypeControl = d.LaneTranXModeTypeControl _
                                         , .LaneTranXSequence = d.LaneTranXSequence _
                                         , .LaneTranXTempType = d.LaneTranXTempType _
                                         , .LaneTranXMinCases = d.LaneTranXMinCases _
                                         , .LaneTranXMinWgt = d.LaneTranXMinWgt _
                                         , .LaneTranXMinCube = d.LaneTranXMinCube _
                                         , .LaneTranXMinPL = d.LaneTranXMinPL _
                                         , .LaneTranXMaxCases = d.LaneTranXMaxCases _
                                         , .LaneTranXMaxWgt = d.LaneTranXMaxWgt _
                                         , .LaneTranXMaxCube = d.LaneTranXMaxCube _
                                         , .LaneTranXMaxPL = d.LaneTranXMaxPL _
                                         , .LaneTranXBenchMiles = d.LaneTranXBenchMiles _
                                         , .LaneTranXUser1 = d.LaneTranXUser1 _
                                         , .LaneTranXUser2 = d.LaneTranXUser2 _
                                         , .LaneTranXUser3 = d.LaneTranXUser3 _
                                         , .LaneTranXUser4 = d.LaneTranXUser4 _
                                         , .LaneTranXModDate = d.LaneTranXModDate _
                                         , .LaneTranXModUser = d.LaneTranXModUser _
                                         , .LaneTranXUpdated = d.LaneTranXUpdated.ToArray() _
                                         , .Page = page _
                                         , .Pages = pagecount _
                                         , .RecordCount = recordcount _
                                         , .PageSize = pagesize}
    End Function

    Friend Function selectDTODataWDetails(ByVal d As LTS.LaneTransLoadXref, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.LaneTransLoadXref
        Return New DTO.LaneTransLoadXref With {.LaneTranXControl = d.LaneTranXControl _
                                         , .LaneTranXName = d.LaneTranXName _
                                         , .LaneTranXLaneControl = d.LaneTranXLaneControl _
                                         , .LaneTranXModeTypeControl = d.LaneTranXModeTypeControl _
                                         , .LaneTranXSequence = d.LaneTranXSequence _
                                         , .LaneTranXTempType = d.LaneTranXTempType _
                                         , .LaneTranXMinCases = d.LaneTranXMinCases _
                                         , .LaneTranXMinWgt = d.LaneTranXMinWgt _
                                         , .LaneTranXMinCube = d.LaneTranXMinCube _
                                         , .LaneTranXMinPL = d.LaneTranXMinPL _
                                         , .LaneTranXMaxCases = d.LaneTranXMaxCases _
                                         , .LaneTranXMaxWgt = d.LaneTranXMaxWgt _
                                         , .LaneTranXMaxCube = d.LaneTranXMaxCube _
                                         , .LaneTranXMaxPL = d.LaneTranXMaxPL _
                                         , .LaneTranXBenchMiles = d.LaneTranXBenchMiles _
                                         , .LaneTranXUser1 = d.LaneTranXUser1 _
                                         , .LaneTranXUser2 = d.LaneTranXUser2 _
                                         , .LaneTranXUser3 = d.LaneTranXUser3 _
                                         , .LaneTranXUser4 = d.LaneTranXUser4 _
                                         , .LaneTranXModDate = d.LaneTranXModDate _
                                         , .LaneTranXModUser = d.LaneTranXModUser _
                                         , .LaneTranXUpdated = d.LaneTranXUpdated.ToArray() _
                                         , .LaneTransLoadXrefDets = (From det In d.LaneTransLoadXrefDets Order By det.LaneTranXDetSequence Ascending Select NGLXDetData.selectDTOData(det)).ToList() _
                                         , .Page = page _
                                         , .Pages = pagecount _
                                         , .RecordCount = recordcount _
                                         , .PageSize = pagesize}


    End Function

#End Region

End Class

Public Class NGLLaneTransLoadXrefDetData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLaneDataContext(ConnectionString)
        Me.LinqTable = db.LaneTransLoadXrefDets
        Me.LinqDB = db
        Me.SourceClass = "NGLLaneTransLoadXrefDetData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLaneDataContext(ConnectionString)
            Me.LinqTable = db.LaneTransLoadXrefDets
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
        Return GetLaneTransLoadXrefDetFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetLaneTransLoadXrefDetFiltered(ByVal Control As Integer) As DTO.LaneTransLoadXrefDet
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Return (From d In db.LaneTransLoadXrefDets
                        Where d.LaneTranXDetControl = Control
                        Select selectDTOData(d)).First
            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetLaneTransLoadXrefDetFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetLaneTransLoadXrefDetsFiltered(ByVal LaneTranXControl As Integer) As DTO.LaneTransLoadXrefDet()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Return (From d In db.LaneTransLoadXrefDets
                        Where d.LaneTranXDetLaneTranXControl = LaneTranXControl
                        Order By d.LaneTranXDetSequence
                        Select selectDTOData(d)).ToArray()
            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetLaneTransLoadXrefDetsFiltered"))
            End Try
            Return Nothing
        End Using
    End Function


#Region "LTS Methods"

    ''' <summary>
    ''' Query Lane TransLoadXrefDet LTS data using AllFilters Object for Rest Services
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.001 on 02/16/2021
    ''' </remarks>
    Public Function GetLaneTransLoadXrefDets365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vLaneTransLoadXrefDet()
        If filters Is Nothing Then Return Nothing
        Dim iLaneTransLoadXrefControl As Integer = 0 'Parent Control Number LaneTranXDetLaneTranXControl
        Dim iLaneTranXDetControl As Integer = 0 'table primary key 
        Dim filterWhere As String = ""
        Dim sFilterSpacer As String = ""

        Dim oRet() As LTS.vLaneTransLoadXrefDet 'return the table or view
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                If (filters.FilterValues Is Nothing OrElse filters.FilterValues.Count() < 1) OrElse (Not filters.FilterValues.Any(Function(x) x.filterName = "LaneTranXDetControl")) Then
                    'The Record Control Filter does not exist so use the parent control fliter
                    If filters.ParentControl = 0 Then
                        throwNoDataFaultMessage("E_MissingParent") ' "The reference to the parent record is missing. Please select a valid parent record and try again."
                    End If
                    iLaneTransLoadXrefControl = filters.ParentControl
                    filterWhere = " (LaneTranXDetLaneTranXControl = " & iLaneTransLoadXrefControl.ToString() & ") "
                    sFilterSpacer = " And "
                Else
                    Dim tFilter As Models.FilterDetails = filters.FilterValues.Where(Function(x) x.filterName = "LaneTranXDetControl").FirstOrDefault()
                    Integer.TryParse(tFilter.filterValueFrom, iLaneTranXDetControl)
                End If

                Dim iQuery As IQueryable(Of LTS.vLaneTransLoadXrefDet)
                iQuery = db.vLaneTransLoadXrefDets
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "LaneTranXDetName"
                    filters.sortDirection = "ASC"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneTransLoadXrefDets365"), db)
            End Try
            Return oRet
        End Using
    End Function

    ''' <summary>
    ''' read a specific  Lane TransLoadXrefDet LTS record using the primary key
    ''' </summary>
    ''' <param name="LaneTranXDetControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.001 on 02/16/2021
    ''' </remarks>
    Public Function GetLaneTransLoadXrefDet365(ByVal LaneTranXDetControl As Integer) As LTS.vLaneTransLoadXrefDet

        Dim oRet As New LTS.vLaneTransLoadXrefDet() 'return the table or view
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                oRet = db.vLaneTransLoadXrefDets.Where(Function(x) x.LaneTranXDetControl = LaneTranXDetControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLaneTransLoadXrefDet365"), db)
            End Try
            Return oRet
        End Using
    End Function

    Public Function SaveOrCreateLaneTransLoadXrefDetItem(ByVal oData As LTS.LaneTransLoadXrefDet, Optional ByVal iLaneTranXDetLaneTranXControl As Integer = 0) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do


        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'LaneTranXDetLaneTranXControl
                Dim iLaneTranXDetControl = oData.LaneTranXDetControl
                Dim tblData = New LTS.LaneTransLoadXrefDet()
                If iLaneTranXDetControl = 0 Then
                    'oData.LaneTranXLaneControl = 0
                    If oData.LaneTranXDetLaneTranXControl = 0 Then
                        If iLaneTranXDetLaneTranXControl = 0 Then
                            Dim sMsg As String = "E_MissingParent" ' "  The reference to the parent record is missing. Please select a valid lane record from the Lane page and try again."
                            throwNoDataFaultException(sMsg)
                        End If
                        oData.LaneTranXDetLaneTranXControl = iLaneTranXDetLaneTranXControl
                    End If

                End If
                '    Dim blnProcessed As Boolean = False
                oData.LaneTranXDetModDate = Date.Now()
                oData.LaneTranXDetModUser = Me.Parameters.UserName


                If oData.LaneTranXDetControl = 0 Then
                    db.LaneTransLoadXrefDets.InsertOnSubmit(oData)
                Else
                    db.LaneTransLoadXrefDets.Attach(oData, True)
                End If
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveOrCreateLaneTransLoadXrefDetItem"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteLaneTransLoadXrefDetItem(ByVal iLaneTranXDetControl As Integer) As Boolean
        Dim blnRet As Boolean = False

        If iLaneTranXDetControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                Dim oExisting = db.LaneTransLoadXrefDets.Where(Function(x) x.LaneTranXDetControl = iLaneTranXDetControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.LaneTranXDetControl = 0 Then Return True
                db.LaneTransLoadXrefDets.DeleteOnSubmit(oExisting)
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteLaneTransLoadXrefDetItem"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region



#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.LaneTransLoadXrefDet)
        'Create New Record
        Return New LTS.LaneTransLoadXrefDet With {.LaneTranXDetControl = d.LaneTranXDetControl _
                                                , .LaneTranXDetName = d.LaneTranXDetName _
                                                , .LaneTranXDetLaneTranXControl = d.LaneTranXDetLaneTranXControl _
                                                , .LaneTranXDetLaneControl = d.LaneTranXDetLaneControl _
                                                , .LaneTranXDetSequence = d.LaneTranXDetSequence _
                                                , .LaneTranXDetCarrierControl = d.LaneTranXDetCarrierControl _
                                                , .LaneTranXDetCarrierContControl = d.LaneTranXDetCarrierContControl _
                                                , .LaneTranXDetModeTypeControl = d.LaneTranXDetModeTypeControl _
                                                , .LaneTranXDetCarrTarControl = d.LaneTranXDetCarrTarControl _
                                                , .LaneTranXDetCarrTarEquipControl = d.LaneTranXDetCarrTarEquipControl _
                                                , .LaneTranXDetRule11Required = d.LaneTranXDetRule11Required _
                                                , .LaneTranXDetBilledSeperately = d.LaneTranXDetBilledSeperately _
                                                , .LaneTranXDetConsolidateSplits = d.LaneTranXDetConsolidateSplits _
                                                , .LaneTranXDetTransType = d.LaneTranXDetTransType _
                                                , .LaneTranXDetBFC = d.LaneTranXDetBFC _
                                                , .LaneTranXDetBFCType = d.LaneTranXDetBFCType _
                                                , .LaneTranXDetBenchMiles = d.LaneTranXDetBenchMiles _
                                                , .LaneTranXDetUser1 = d.LaneTranXDetUser1 _
                                                , .LaneTranXDetUser2 = d.LaneTranXDetUser2 _
                                                , .LaneTranXDetUser3 = d.LaneTranXDetUser3 _
                                                , .LaneTranXDetUser4 = d.LaneTranXDetUser4 _
                                                , .LaneTranXDetTransitHours = d.LaneTranXDetTransitHours _
                                                , .LaneTranXDetTransferHours = d.LaneTranXDetTransferHours _
                                                , .LaneTranXDetModDate = Date.Now _
                                                , .LaneTranXDetModUser = Parameters.UserName _
                                                , .LaneTranXDetUpdated = If(d.LaneTranXDetUpdated Is Nothing, New Byte() {}, d.LaneTranXDetUpdated)}
    End Function

    Friend Overloads Function GetLTSFromDTO(ByVal oData As DTO.LaneTransLoadXrefDet) As LTS.LaneTransLoadXrefDet
        Return CopyDTOToLinq(oData)

        'Dim d = CType(oData, DTO.LaneTransLoadXrefDet)
        'Create New Record
        Return New LTS.LaneTransLoadXrefDet With {.LaneTranXDetControl = oData.LaneTranXDetControl _
                                                , .LaneTranXDetName = oData.LaneTranXDetName _
                                                , .LaneTranXDetLaneTranXControl = oData.LaneTranXDetLaneTranXControl _
                                                , .LaneTranXDetLaneControl = oData.LaneTranXDetLaneControl _
                                                , .LaneTranXDetSequence = oData.LaneTranXDetSequence _
                                                , .LaneTranXDetCarrierControl = oData.LaneTranXDetCarrierControl _
                                                , .LaneTranXDetCarrierContControl = oData.LaneTranXDetCarrierContControl _
                                                , .LaneTranXDetModeTypeControl = oData.LaneTranXDetModeTypeControl _
                                                , .LaneTranXDetCarrTarControl = oData.LaneTranXDetCarrTarControl _
                                                , .LaneTranXDetCarrTarEquipControl = oData.LaneTranXDetCarrTarEquipControl _
                                                , .LaneTranXDetRule11Required = oData.LaneTranXDetRule11Required _
                                                , .LaneTranXDetBilledSeperately = oData.LaneTranXDetBilledSeperately _
                                                , .LaneTranXDetConsolidateSplits = oData.LaneTranXDetConsolidateSplits _
                                                , .LaneTranXDetTransType = oData.LaneTranXDetTransType _
                                                , .LaneTranXDetBFC = oData.LaneTranXDetBFC _
                                                , .LaneTranXDetBFCType = oData.LaneTranXDetBFCType _
                                                , .LaneTranXDetBenchMiles = oData.LaneTranXDetBenchMiles _
                                                , .LaneTranXDetUser1 = oData.LaneTranXDetUser1 _
                                                , .LaneTranXDetUser2 = oData.LaneTranXDetUser2 _
                                                , .LaneTranXDetUser3 = oData.LaneTranXDetUser3 _
                                                , .LaneTranXDetUser4 = oData.LaneTranXDetUser4 _
                                                , .LaneTranXDetTransitHours = oData.LaneTranXDetTransitHours _
                                                , .LaneTranXDetTransferHours = oData.LaneTranXDetTransferHours _
                                                , .LaneTranXDetModDate = Date.Now() _
                                                , .LaneTranXDetModUser = Parameters.UserName _
                                                , .LaneTranXDetUpdated = If(oData.LaneTranXDetUpdated Is Nothing, New Byte() {}, oData.LaneTranXDetUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetLaneTransLoadXrefDetFiltered(Control:=CType(LinqTable, LTS.LaneTransLoadXrefDet).LaneTranXDetControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim source As LTS.LaneTransLoadXrefDet = TryCast(LinqTable, LTS.LaneTransLoadXrefDet)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.LaneTransLoadXrefDets
                       Where d.LaneTranXDetControl = source.LaneTranXDetControl
                       Select New DTO.QuickSaveResults With {.Control = d.LaneTranXDetControl _
                                                            , .ModDate = d.LaneTranXDetModDate _
                                                            , .ModUser = d.LaneTranXDetModUser _
                                                            , .Updated = d.LaneTranXDetUpdated.ToArray}).FirstOrDefault()
                If ret Is Nothing OrElse ret.Control = 0 Then throwNoDataFaultException()

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetQuickSaveResults"))
            End Try

        End Using
        Return ret
    End Function

    ''' <summary>
    ''' Map LTS data to DTO data
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="page"></param>
    ''' <param name="pagecount"></param>
    ''' <param name="recordcount"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.004 on 01/28/2024
    '''     Convert nullable booleans to false
    '''     .LaneTranXDetBilledSeperately = If(d.LaneTranXDetBilledSeperately,False)
    '''     .LaneTranXDetConsolidateSplits = If(d.LaneTranXDetConsolidateSplits,False)
    ''' </remarks>
    Friend Function selectDTOData(ByVal d As LTS.LaneTransLoadXrefDet, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.LaneTransLoadXrefDet

        Return New DTO.LaneTransLoadXrefDet With {.LaneTranXDetControl = d.LaneTranXDetControl _
                                         , .LaneTranXDetName = d.LaneTranXDetName _
                                         , .LaneTranXDetLaneTranXControl = d.LaneTranXDetLaneTranXControl _
                                         , .LaneTranXDetLaneControl = d.LaneTranXDetLaneControl _
                                         , .LaneTranXDetSequence = d.LaneTranXDetSequence _
                                         , .LaneTranXDetCarrierControl = d.LaneTranXDetCarrierControl _
                                         , .LaneTranXDetCarrierContControl = d.LaneTranXDetCarrierContControl _
                                         , .LaneTranXDetModeTypeControl = d.LaneTranXDetModeTypeControl _
                                         , .LaneTranXDetCarrTarControl = d.LaneTranXDetCarrTarControl _
                                         , .LaneTranXDetCarrTarEquipControl = d.LaneTranXDetCarrTarEquipControl _
                                         , .LaneTranXDetRule11Required = d.LaneTranXDetRule11Required _
                                         , .LaneTranXDetBilledSeperately = If(d.LaneTranXDetBilledSeperately, False) _
                                         , .LaneTranXDetConsolidateSplits = If(d.LaneTranXDetConsolidateSplits, False) _
                                         , .LaneTranXDetTransType = d.LaneTranXDetTransType _
                                         , .LaneTranXDetBFC = d.LaneTranXDetBFC _
                                         , .LaneTranXDetBFCType = d.LaneTranXDetBFCType _
                                         , .LaneTranXDetBenchMiles = d.LaneTranXDetBenchMiles _
                                         , .LaneTranXDetUser1 = d.LaneTranXDetUser1 _
                                         , .LaneTranXDetUser2 = d.LaneTranXDetUser2 _
                                         , .LaneTranXDetUser3 = d.LaneTranXDetUser3 _
                                         , .LaneTranXDetUser4 = d.LaneTranXDetUser4 _
                                         , .LaneTranXDetTransitHours = d.LaneTranXDetTransitHours _
                                         , .LaneTranXDetTransferHours = d.LaneTranXDetTransferHours _
                                         , .LaneTranXDetModDate = d.LaneTranXDetModDate _
                                         , .LaneTranXDetModUser = d.LaneTranXDetModUser _
                                         , .LaneTranXDetUpdated = d.LaneTranXDetUpdated.ToArray() _
                                         , .Page = page _
                                         , .Pages = pagecount _
                                         , .RecordCount = recordcount _
                                         , .PageSize = pagesize}
    End Function

    ''' <summary>
    ''' Loads the LaneTransLoadXrefDet with text for the lookup data fields
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="ld"></param>
    ''' <param name="md"></param>
    ''' <param name="t"></param>
    ''' <param name="tar"></param>
    ''' <param name="eq"></param>
    ''' <param name="c"></param>
    ''' <param name="cnt"></param>
    ''' <param name="page"></param>
    ''' <param name="pagecount"></param>
    ''' <param name="recordcount"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.004 on 01/28/2024
    '''     Convert nullable booleans to false
    '''     .LaneTranXDetBilledSeperately = If(d.LaneTranXDetBilledSeperately,False)
    '''     .LaneTranXDetConsolidateSplits = If(d.LaneTranXDetConsolidateSplits,False)
    ''' </remarks>
    Friend Function selectDTOData(ByVal d As LTS.LaneTransLoadXrefDet,
                                    ByVal ld As LTS.Lane,
                                    ByVal md As LTS.tblModeTypeRefLane,
                                    ByVal t As LTS.LaneTranRefLane,
                                    ByVal tar As LTS.CarrierTariffRefLane,
                                    ByVal eq As LTS.CarrierTariffEquipmentRefLane,
                                    ByVal c As LTS.CarrierRefLane,
                                    ByVal cnt As LTS.CarrierContRefLane,
                                    Optional page As Integer = 1,
                                    Optional pagecount As Integer = 1,
                                    Optional ByVal recordcount As Integer = 1,
                                    Optional pagesize As Integer = 1000) As DTO.LaneTransLoadXrefDet

        Return New DTO.LaneTransLoadXrefDet With {.LaneTranXDetControl = d.LaneTranXDetControl _
                                         , .LaneTranXDetName = d.LaneTranXDetName _
                                         , .LaneTranXDetLaneTranXControl = d.LaneTranXDetLaneTranXControl _
                                         , .LaneTranXDetLaneControl = d.LaneTranXDetLaneControl _
                                                 , .LaneTranXDetLaneName = ld.LaneName _
                                                 , .LaneTranXDetLaneNumber = ld.LaneNumber _
                                         , .LaneTranXDetSequence = d.LaneTranXDetSequence _
                                         , .LaneTranXDetCarrierControl = d.LaneTranXDetCarrierControl _
                                                 , .LaneTranXDetCarrierName = c.CarrierName _
                                                 , .LaneTranXDetCarrierNumber = c.CarrierNumber _
                                         , .LaneTranXDetCarrierContControl = d.LaneTranXDetCarrierContControl _
                                                 , .LaneTranXDetContName = cnt.CarrierContName _
                                                 , .LaneTranXDetContPhone = cnt.CarrierContactPhone _
                                                 , .LaneTranXDetContExt = cnt.CarrierContPhoneExt _
                                                 , .LaneTranXDetCont800 = cnt.CarrierContact800 _
                                         , .LaneTranXDetModeTypeControl = d.LaneTranXDetModeTypeControl _
                                                 , .LaneTranXDetModeTypeName = md.ModeTypeName _
                                         , .LaneTranXDetCarrTarControl = d.LaneTranXDetCarrTarControl _
                                                 , .LaneTranXDetCarrTarName = tar.CarrTarName _
                                         , .LaneTranXDetCarrTarEquipControl = d.LaneTranXDetCarrTarEquipControl _
                                                 , .LaneTranXDetCarrTarEquipName = eq.CarrTarEquipName _
                                         , .LaneTranXDetRule11Required = d.LaneTranXDetRule11Required _
                                         , .LaneTranXDetBilledSeperately = If(d.LaneTranXDetBilledSeperately, False) _
                                         , .LaneTranXDetConsolidateSplits = If(d.LaneTranXDetConsolidateSplits, False) _
                                         , .LaneTranXDetTransType = d.LaneTranXDetTransType _
                                                 , .LaneTranXDetTransTypeName = t.LaneTransTypeDesc _
                                         , .LaneTranXDetBFC = d.LaneTranXDetBFC _
                                         , .LaneTranXDetBFCType = d.LaneTranXDetBFCType _
                                         , .LaneTranXDetBenchMiles = d.LaneTranXDetBenchMiles _
                                         , .LaneTranXDetUser1 = d.LaneTranXDetUser1 _
                                         , .LaneTranXDetUser2 = d.LaneTranXDetUser2 _
                                         , .LaneTranXDetUser3 = d.LaneTranXDetUser3 _
                                         , .LaneTranXDetUser4 = d.LaneTranXDetUser4 _
                                         , .LaneTranXDetTransitHours = d.LaneTranXDetTransitHours _
                                         , .LaneTranXDetTransferHours = d.LaneTranXDetTransferHours _
                                         , .LaneTranXDetModDate = d.LaneTranXDetModDate _
                                         , .LaneTranXDetModUser = d.LaneTranXDetModUser _
                                         , .LaneTranXDetUpdated = d.LaneTranXDetUpdated.ToArray() _
                                         , .Page = page _
                                         , .Pages = pagecount _
                                         , .RecordCount = recordcount _
                                         , .PageSize = pagesize}
    End Function

#End Region

End Class


Public Class NGLLaneOptimizationSDFSettingsData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLaneDataContext(ConnectionString)
        Me.LinqTable = db.Lanes
        Me.LinqDB = db
        Me.SourceClass = "NGLLaneOptimizationSDFSettingsData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLaneDataContext(ConnectionString)
            Me.LinqTable = db.Lanes
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
        Return GetLaneOptimizationSDFSettingsFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetLaneOptimizationSDFSettingsFiltered(Optional ByVal Control As Integer = 0, Optional ByVal Number As String = "", Optional ByVal Name As String = "") As DTO.LaneOptimizationSDFSettings
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'LaneOptimizationSDFSettings
                'Get the newest record that matches the provided criteria
                Dim LaneOptimizationSDFSettings As DTO.LaneOptimizationSDFSettings = (
                From t In db.Lanes
                Where
                    (Control = 0 OrElse t.LaneControl = Control) _
                    And
                    (Number Is Nothing OrElse String.IsNullOrEmpty(Number) OrElse t.LaneNumber = Number) _
                    And
                    (Name Is Nothing OrElse String.IsNullOrEmpty(Name) OrElse t.LaneName = Name)
                Order By t.LaneNumber Descending
                Select New DTO.LaneOptimizationSDFSettings With {.LaneControl = t.LaneControl _
                                              , .LaneNumber = t.LaneNumber _
                                              , .LaneName = t.LaneName _
                                              , .LaneNumberMaster = t.LaneNumberMaster _
                                              , .LaneNameMaster = t.LaneNameMaster _
                                              , .LaneCompControl = t.LaneCompControl _
                                              , .LaneSDFUse = If(t.LaneOriginAddressUse.HasValue, t.LaneOriginAddressUse.Value, False) _
                                              , .LaneSDFSRZone = If(t.LaneSDFSRZone.HasValue, t.LaneSDFSRZone.Value, 0) _
                                              , .LaneSDFMRZone = If(t.LaneSDFMRZone.HasValue, t.LaneSDFMRZone.Value, 0) _
                                              , .LaneSDFFixedTime = If(t.LaneSDFFixedTime.HasValue, t.LaneSDFFixedTime.Value, 0) _
                                              , .LaneSDFEarlyTM1 = If(t.LaneSDFEarlyTM1.HasValue, t.LaneSDFEarlyTM1.Value, 0) _
                                              , .LaneSDFLateTM1 = If(t.LaneSDFLateTM1.HasValue, t.LaneSDFLateTM1.Value, 0) _
                                              , .LaneSDFDay1 = t.LaneSDFDay1 _
                                              , .LaneSDFEarlyTM2 = If(t.LaneSDFEarlyTM2.HasValue, t.LaneSDFEarlyTM2.Value, 0) _
                                              , .LaneSDFLateTM2 = If(t.LaneSDFLateTM2.HasValue, t.LaneSDFLateTM2.Value, 0) _
                                              , .LaneSDFDay2 = t.LaneSDFDay2 _
                                              , .LaneSDFEarlyTM3 = If(t.LaneSDFEarlyTM3.HasValue, t.LaneSDFEarlyTM3.Value, 0) _
                                              , .LaneSDFLateTM3 = If(t.LaneSDFLateTM3.HasValue, t.LaneSDFLateTM3.Value, 0) _
                                              , .LaneSDFDay3 = t.LaneSDFDay3 _
                                              , .LaneSDFEarlyTM4 = If(t.LaneSDFEarlyTM4.HasValue, t.LaneSDFEarlyTM4.Value, 0) _
                                              , .LaneSDFLateTM4 = If(t.LaneSDFLateTM4.HasValue, t.LaneSDFLateTM4.Value, 0) _
                                              , .LaneSDFDay4 = t.LaneSDFDay4 _
                                              , .LaneSDFEarlyTM5 = If(t.LaneSDFEarlyTM5.HasValue, t.LaneSDFEarlyTM5.Value, 0) _
                                              , .LaneSDFLateTM5 = If(t.LaneSDFLateTM5.HasValue, t.LaneSDFLateTM5.Value, 0) _
                                              , .LaneSDFDay5 = t.LaneSDFDay5 _
                                              , .LaneSDFEarlyTM6 = If(t.LaneSDFEarlyTM6.HasValue, t.LaneSDFEarlyTM6.Value, 0) _
                                              , .LaneSDFLateTM6 = If(t.LaneSDFLateTM6.HasValue, t.LaneSDFLateTM6.Value, 0) _
                                              , .LaneSDFDay6 = t.LaneSDFDay6 _
                                              , .LaneSDFEarlyTM7 = If(t.LaneSDFEarlyTM7.HasValue, t.LaneSDFEarlyTM7.Value, 0) _
                                              , .LaneSDFLateTM7 = If(t.LaneSDFEarlyTM7.HasValue, t.LaneSDFEarlyTM7.Value, 0) _
                                              , .LaneSDFDay7 = t.LaneSDFDay7 _
                                              , .LaneSDFUnldRate1 = If(t.LaneSDFUnldRate1.HasValue, t.LaneSDFUnldRate1.Value, 0) _
                                              , .LaneSDFUnldRate2 = If(t.LaneSDFUnldRate2.HasValue, t.LaneSDFUnldRate2.Value, 0) _
                                              , .LaneSDFUnldRate3 = If(t.LaneSDFUnldRate3.HasValue, t.LaneSDFUnldRate3.Value, 0) _
                                              , .LaneSDFUnldRate4 = If(t.LaneSDFUnldRate4.HasValue, t.LaneSDFUnldRate4.Value, 0) _
                                              , .LaneSDFUnldRate5 = If(t.LaneSDFUnldRate5.HasValue, t.LaneSDFUnldRate5.Value, 0)}).First

                Return LaneOptimizationSDFSettings

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim t = CType(oData, DTO.Lane)
        'Create New Record
        Return New LTS.Lane With {.LaneControl = t.LaneControl _
                                  , .LaneNumber = t.LaneNumber _
                                  , .LaneName = t.LaneName _
                                  , .LaneNumberMaster = t.LaneNumberMaster _
                                  , .LaneNameMaster = t.LaneNameMaster _
                                  , .LaneCompControl = t.LaneCompControl _
                                  , .LaneSDFUse = t.LaneOriginAddressUse _
                                  , .LaneSDFSRZone = t.LaneSDFSRZone _
                                  , .LaneSDFMRZone = t.LaneSDFMRZone _
                                  , .LaneSDFFixedTime = t.LaneSDFFixedTime _
                                  , .LaneSDFEarlyTM1 = t.LaneSDFEarlyTM1 _
                                  , .LaneSDFLateTM1 = t.LaneSDFLateTM1 _
                                  , .LaneSDFDay1 = t.LaneSDFDay1 _
                                  , .LaneSDFEarlyTM2 = t.LaneSDFEarlyTM2 _
                                  , .LaneSDFLateTM2 = t.LaneSDFLateTM2 _
                                  , .LaneSDFDay2 = t.LaneSDFDay2 _
                                  , .LaneSDFEarlyTM3 = t.LaneSDFEarlyTM3 _
                                  , .LaneSDFLateTM3 = t.LaneSDFLateTM3 _
                                  , .LaneSDFDay3 = t.LaneSDFDay3 _
                                  , .LaneSDFEarlyTM4 = t.LaneSDFEarlyTM4 _
                                  , .LaneSDFLateTM4 = t.LaneSDFLateTM4 _
                                  , .LaneSDFDay4 = t.LaneSDFDay4 _
                                  , .LaneSDFEarlyTM5 = t.LaneSDFEarlyTM5 _
                                  , .LaneSDFLateTM5 = t.LaneSDFLateTM5 _
                                  , .LaneSDFDay5 = t.LaneSDFDay5 _
                                  , .LaneSDFEarlyTM6 = t.LaneSDFEarlyTM6 _
                                  , .LaneSDFLateTM6 = t.LaneSDFLateTM6 _
                                  , .LaneSDFDay6 = t.LaneSDFDay6 _
                                  , .LaneSDFEarlyTM7 = t.LaneSDFEarlyTM7 _
                                  , .LaneSDFLateTM7 = t.LaneSDFEarlyTM7 _
                                  , .LaneSDFDay7 = t.LaneSDFDay7 _
                                  , .LaneSDFUnldRate1 = t.LaneSDFUnldRate1 _
                                  , .LaneSDFUnldRate2 = t.LaneSDFUnldRate2 _
                                  , .LaneSDFUnldRate3 = t.LaneSDFUnldRate3 _
                                  , .LaneSDFUnldRate4 = t.LaneSDFUnldRate4 _
                                  , .LaneSDFUnldRate5 = t.LaneSDFUnldRate5}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetLaneOptimizationSDFSettingsFiltered(Control:=CType(LinqTable, LTS.Lane).LaneControl)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow company records to be added from this class
        Utilities.SaveAppError("Cannot add data.  Records cannot be inserted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow company records to be updated from this class
        Dim strDetails As String = "Cannot save data.  Records cannot be updated using this interface!"
        Utilities.SaveAppError(strDetails, Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow booking records to be deleted from this class
        Dim strDetails As String = "Cannot delete data.  Records cannot be deleted using this interface!"
        Utilities.SaveAppError(strDetails, Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
    End Sub

#End Region

End Class

Public Class NGLBadLaneAddressData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLaneDataContext(ConnectionString)
        Me.LinqTable = db.tblBadLaneAddresses
        Me.LinqDB = db
        Me.SourceClass = "NGLBadLaneAddressData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLaneDataContext(ConnectionString)
            Me.LinqTable = db.tblBadLaneAddresses
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
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                'db.Log = New DebugTextWriter

                'Return all the records that match the criteria sorted by parkey
                Dim badAddresses As DTO.tblBadAddress = (
                From t In db.tblBadLaneAddresses
                Where
                    (Control = 0 OrElse t.BLAControl = Control)
                Order By t.BLAControl
                Select New DTO.tblBadAddress With {.BLAControl = t.BLAControl _
                                        , .BLALaneControl = t.BLALaneControl _
                                        , .BLALaneNumber = t.BLALaneNumber _
                                        , .BatchID = t.BatchID _
                                        , .BLABookProNumber = t.BLABookProNumber _
                                        , .BLALaneDestAddress1 = t.BLALaneDestAddress1 _
                                        , .BLALaneDestCity = t.BLALaneDestCity _
                                        , .BLALaneDestCountry = t.BLALaneDestCountry _
                                        , .BLALaneDestState = t.BLALaneDestState _
                                        , .BLALaneDestZip = t.BLALaneDestZip _
                                        , .BLALaneOrigAddress1 = t.BLALaneOrigAddress1 _
                                        , .BLALaneOrigCity = t.BLALaneOrigCity _
                                        , .BLALaneOrigCountry = t.BLALaneOrigCountry _
                                        , .BLALaneOrigState = t.BLALaneOrigState _
                                        , .BLALaneOrigZip = t.BLALaneOrigZip _
                                        , .BLAMessage = t.BLAMessage _
                                        , .BLAModDate = t.BLAModDate _
                                        , .BLAModUser = t.BLAModUser _
                                        , .BLAPCMilerDestAddress1 = t.BLAPCMilerDestAddress1 _
                                        , .BLAPCMilerDestCity = t.BLAPCMilerDestCity _
                                        , .BLAPCMilerDestCountry = t.BLAPCMilerDestCountry _
                                        , .BLAPCMilerDestState = t.BLAPCMilerDestState _
                                        , .BLAPCMilerDestZip = t.BLAPCMilerDestZip _
                                        , .BLAPCMilerOrigAddress1 = t.BLAPCMilerOrigAddress1 _
                                        , .BLAPCMilerOrigCity = t.BLAPCMilerOrigCity _
                                        , .BLAPCMilerOrigState = t.BLAPCMilerOrigState _
                                        , .BLAPCMilerOrigCountry = t.BLAPCMilerOrigCountry _
                                        , .BLAPCMilerOrigZip = t.BLAPCMilerOrigZip _
                                        , .BLAUpdated = t.BLAUpdated.ToArray()}).First()

                Return badAddresses

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetTblBadAddressesFiltered(ByVal BatchID As Double, Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblBadAddress()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    If BatchID > 0 Then
                        intRecordCount = getScalarInteger("select COUNT(dbo.tblBadLaneAddress.BLAControl) from dbo.tblBadLaneAddress Where BatchID =  " & BatchID)
                    Else
                        intRecordCount = getScalarInteger("select COUNT(dbo.tblBadLaneAddress.BLAControl) from dbo.tblBadLaneAddress ")
                    End If


                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'db.Log = New DebugTextWriter

                'Return all the records that match the criteria sorted by parkey
                Dim badAddresses() As DTO.tblBadAddress = (
                From t In db.tblBadLaneAddresses
                Where
                    (BatchID = 0 OrElse t.BatchID = BatchID)
                Order By t.BLAControl
                Select New DTO.tblBadAddress With {.BLAControl = t.BLAControl _
                                        , .BLALaneControl = t.BLALaneControl _
                                        , .BLALaneNumber = t.BLALaneNumber _
                                        , .BatchID = t.BatchID _
                                        , .BLABookProNumber = t.BLABookProNumber _
                                        , .BLALaneDestAddress1 = t.BLALaneDestAddress1 _
                                        , .BLALaneDestCity = t.BLALaneDestCity _
                                        , .BLALaneDestCountry = t.BLALaneDestCountry _
                                        , .BLALaneDestState = t.BLALaneDestState _
                                        , .BLALaneDestZip = t.BLALaneDestZip _
                                        , .BLALaneOrigAddress1 = t.BLALaneOrigAddress1 _
                                        , .BLALaneOrigCity = t.BLALaneOrigCity _
                                        , .BLALaneOrigCountry = t.BLALaneOrigCountry _
                                        , .BLALaneOrigState = t.BLALaneOrigState _
                                        , .BLALaneOrigZip = t.BLALaneOrigZip _
                                        , .BLAMessage = t.BLAMessage _
                                        , .BLAModDate = t.BLAModDate _
                                        , .BLAModUser = t.BLAModUser _
                                        , .BLAPCMilerDestAddress1 = t.BLAPCMilerDestAddress1 _
                                        , .BLAPCMilerDestCity = t.BLAPCMilerDestCity _
                                        , .BLAPCMilerDestCountry = t.BLAPCMilerDestCountry _
                                        , .BLAPCMilerDestState = t.BLAPCMilerDestState _
                                        , .BLAPCMilerDestZip = t.BLAPCMilerDestZip _
                                        , .BLAPCMilerOrigAddress1 = t.BLAPCMilerOrigAddress1 _
                                        , .BLAPCMilerOrigCity = t.BLAPCMilerOrigCity _
                                        , .BLAPCMilerOrigState = t.BLAPCMilerOrigState _
                                        , .BLAPCMilerOrigCountry = t.BLAPCMilerOrigCountry _
                                        , .BLAPCMilerOrigZip = t.BLAPCMilerOrigZip _
                                        , .Page = page _
                                        , .Pages = intPageCount _
                                        , .RecordCount = intRecordCount _
                                        , .PageSize = pagesize _
                                        , .BLAUpdated = t.BLAUpdated.ToArray()}).Skip(intSkip).Take(pagesize).ToArray()


                Return badAddresses

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblBadAddress)
        'Create(New Record)
        Return New LTS.tblBadLaneAddress With {.BLAControl = d.BLAControl _
                                        , .BLALaneControl = d.BLALaneControl _
                                        , .BLALaneNumber = d.BLALaneNumber _
                                        , .BatchID = d.BatchID _
                                        , .BLABookProNumber = d.BLABookProNumber _
                                        , .BLALaneDestAddress1 = d.BLALaneDestAddress1 _
                                        , .BLALaneDestCity = d.BLALaneDestCity _
                                        , .BLALaneDestCountry = d.BLALaneDestCountry _
                                        , .BLALaneDestState = d.BLALaneDestState _
                                        , .BLALaneDestZip = d.BLALaneDestZip _
                                        , .BLALaneOrigAddress1 = d.BLALaneOrigAddress1 _
                                        , .BLALaneOrigCity = d.BLALaneOrigCity _
                                        , .BLALaneOrigCountry = d.BLALaneOrigCountry _
                                        , .BLALaneOrigState = d.BLALaneOrigState _
                                        , .BLALaneOrigZip = d.BLALaneOrigZip _
                                        , .BLAMessage = d.BLAMessage _
                                        , .BLAModDate = d.BLAModDate _
                                        , .BLAModUser = d.BLAModUser _
                                        , .BLAPCMilerDestAddress1 = d.BLAPCMilerDestAddress1 _
                                        , .BLAPCMilerDestCity = d.BLAPCMilerDestCity _
                                        , .BLAPCMilerDestCountry = d.BLAPCMilerDestCountry _
                                        , .BLAPCMilerDestState = d.BLAPCMilerDestState _
                                        , .BLAPCMilerDestZip = d.BLAPCMilerDestZip _
                                        , .BLAPCMilerOrigAddress1 = d.BLAPCMilerOrigAddress1 _
                                        , .BLAPCMilerOrigCity = d.BLAPCMilerOrigCity _
                                        , .BLAPCMilerOrigState = d.BLAPCMilerOrigState _
                                        , .BLAPCMilerOrigCountry = d.BLAPCMilerOrigCountry _
                                        , .BLAPCMilerOrigZip = d.BLAPCMilerOrigZip _
                                        , .BLAUpdated = If(d.BLAUpdated Is Nothing, New Byte() {}, d.BLAUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetRecordFiltered(CType(LinqTable, LTS.tblBadLaneAddress).BatchID)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim source As LTS.tblBadLaneAddress = TryCast(LinqTable, LTS.tblBadLaneAddress)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblBadLaneAddresses
                       Where d.BLAControl = source.BLAControl
                       Select New DTO.QuickSaveResults With {.Control = d.BLAControl _
                                                            , .ModDate = d.BLAModDate _
                                                            , .ModUser = d.BLAModUser _
                                                            , .Updated = d.BLAUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

    'Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
    '    'We do not allow parameter data to be deleted via WCF
    '    Utilities.SaveAppError("Cannot delete bad address Data data.  Bad Lane Address information are required by the system and cannot be deleted!", Me.Parameters)
    '    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    'End Sub

#End Region

End Class

Public Class NGLtblStaticRouteData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLaneDataContext(ConnectionString)
        Me.LinqTable = db.tblStaticRoutes
        Me.LinqDB = db
        Me.SourceClass = "NGLtblStaticRouteData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLaneDataContext(ConnectionString)
            Me.LinqTable = db.tblStaticRoutes
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
        Return GettblStaticRouteFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblStaticRoutesFiltered()
    End Function

    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' Enter a compcontrol for the FKControl parameter or zero to ignore it
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim tblStaticRoute As DTO.tblStaticRoute

                If LowerControl <> 0 Then
                    tblStaticRoute = (
                   From d In db.tblStaticRoutes
                   Where d.StaticRouteControl >= LowerControl _
                   And
                   (FKControl = 0 OrElse d.StaticRouteCompControl = FKControl)
                   Order By d.StaticRouteControl
                   Select selectDTOData(d, db)).FirstOrDefault

                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblStaticRoute = (
                   From d In db.tblStaticRoutes
                   Where (FKControl = 0 OrElse d.StaticRouteCompControl = FKControl)
                   Order By d.StaticRouteControl
                   Select selectDTOData(d, db)).FirstOrDefault
                End If



                Return tblStaticRoute

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' Enter a compcontrol for the FKControl parameter or zero to ignore it
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblStaticRoute As DTO.tblStaticRoute = (
                From d In db.tblStaticRoutes
                Where d.StaticRouteControl < CurrentControl _
                And
                (FKControl = 0 OrElse d.StaticRouteCompControl = FKControl)
                Order By d.StaticRouteControl Descending
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblStaticRoute

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' Enter a compcontrol for the FKControl parameter or zero to ignore it
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblStaticRoute As DTO.tblStaticRoute = (
                From d In db.tblStaticRoutes
                Where d.StaticRouteControl > CurrentControl _
                And
                (FKControl = 0 OrElse d.StaticRouteCompControl = FKControl)
                Order By d.StaticRouteControl
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblStaticRoute

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' Enter a compcontrol for the FKControl parameter or zero to ignore it
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim tblStaticRoute As DTO.tblStaticRoute

                If UpperControl <> 0 Then

                    tblStaticRoute = (
                    From d In db.tblStaticRoutes
                    Where d.StaticRouteControl >= UpperControl _
                    And
                    (FKControl = 0 OrElse d.StaticRouteCompControl = FKControl)
                    Order By d.StaticRouteControl
                    Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest StaticRoutecontrol record
                    tblStaticRoute = (
                    From d In db.tblStaticRoutes
                    Order By d.StaticRouteControl Descending
                    Select selectDTOData(d, db)).FirstOrDefault

                End If


                Return tblStaticRoute

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblStaticRouteFiltered(ByVal Control As Long) As DTO.tblStaticRoute
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim StaticRoute As DTO.tblStaticRoute = (
                From d In db.tblStaticRoutes
                Where
                    d.StaticRouteControl = Control
                Select selectDTOData(d, db)).First


                Return StaticRoute

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblStaticRoutesFiltered(Optional ByVal CompControl As Integer = 0, Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblStaticRoute()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("select COUNT(*) from dbo.tblStaticRoute")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblStaticRoutes() As DTO.tblStaticRoute = (
                From d In db.tblStaticRoutes
                Where (CompControl = 0 OrElse d.StaticRouteCompControl = CompControl)
                Order By d.StaticRouteControl Descending
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                Return tblStaticRoutes

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function


    Friend Function selectDTOData(ByVal d As LTS.tblStaticRoute, ByRef db As NGLMASLaneDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblStaticRoute
        Return New DTO.tblStaticRoute With {.StaticRouteControl = d.StaticRouteControl _
                                                , .StaticRouteName = d.StaticRouteName _
                                                , .StaticRouteDescription = d.StaticRouteDescription _
                                                , .StaticRouteCompControl = d.StaticRouteCompControl _
                                                , .StaticRouteNumber = d.StaticRouteNumber _
                                                , .StaticRouteNatNumber = d.StaticRouteNatNumber _
                                                , .StaticRouteNatName = d.StaticRouteNatName _
                                                , .StaticRouteCompName = d.StaticRouteCompName _
                                                , .StaticRouteCompNumber = d.StaticRouteCompNumber _
                                                , .StaticRouteAutoTenderFlag = d.StaticRouteAutoTenderFlag _
                                                , .StaticRouteUseShipDateFlag = d.StaticRouteUseShipDateFlag _
                                                , .StaticRouteGuideDateSelectionDaysBefore = d.StaticRouteGuideDateSelectionDaysBefore _
                                                , .StaticRouteGuideDateSelectionDaysAfter = d.StaticRouteGuideDateSelectionDaysAfter _
                                                , .StaticRouteSplitOversizedLoads = d.StaticRouteSplitOversizedLoads _
                                                , .StaticRouteCapacityPreference = d.StaticRouteCapacityPreference _
                                                , .StaticRouteRequireAutoTenderApproval = d.StaticRouteRequireAutoTenderApproval _
                                                , .StaticRouteFillLargestFirst = d.StaticRouteFillLargestFirst _
                                                , .StaticRoutePlaceOnHold = d.StaticRoutePlaceOnHold _
                                               , .StaticRouteModDate = d.StaticRouteModDate _
                                               , .StaticRouteModUser = d.StaticRouteModUser _
                                               , .StaticRouteURI = d.StaticRouteURI _
                                               , .Page = page _
                                               , .Pages = pagecount _
                                               , .PageSize = pagesize _
                                               , .StaticRouteUpdated = d.StaticRouteUpdated.ToArray()}
    End Function

#End Region

#Region "LTS Methods"

    ''' <summary>
    ''' Get a filtered array of Static Route Data
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 01/07/2023 for New LTS Static Route Logic
    ''' </remarks>
    Public Function GettblStaticRoutes(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblStaticRoute()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tblStaticRoute
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim leComps As Integer() = db.vLEComp365RefLanes.Where(Function(t) t.LEAdminControl = Parameters.UserLEControl And t.UserSecurityControl = Parameters.UserControl).Select(Function(x) x.CompControl).ToArray()
                Dim iQuery As IQueryable(Of LTS.tblStaticRoute)
                iQuery = (From t In db.tblStaticRoutes
                          Where (leComps.Contains(t.StaticRouteCompControl))
                          Select t)
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblStaticRoutes"), db)
            End Try
        End Using
        Return Nothing
    End Function


    ''' <summary>
    ''' Insert or Update Static Route Data
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 01/07/2023 for New LTS Static Route Logic
    ''' </remarks>
    Public Function SavetblStaticRoute(ByVal oData As LTS.tblStaticRoute) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim strDetails As String = ""
                If oData.StaticRouteCompControl = 0 Then
                    strDetails = "Cannot save Route changes.  The Route has not been assigned to a warehouse, please select a valid warehouse and try again."
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                End If
                Dim oComp As LTS.vLEComp365RefLane = db.vLEComp365RefLanes.Where(Function(t) t.LEAdminControl = Parameters.UserLEControl And t.UserSecurityControl = Parameters.UserControl And t.CompControl = oData.StaticRouteCompControl).FirstOrDefault()


                If (oComp Is Nothing OrElse oComp.CompControl = 0) Then
                    strDetails = "Cannot save Route changes.  The Route has not been assigned to a warehouse, please select a valid warehouse and try again."
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                End If

                ValidateSaveRecord(db, oData.StaticRouteControl, oData.StaticRouteNumber)
                Dim iStaticRouteControl As Integer = oData.StaticRouteControl
                oData.StaticRouteModDate = Date.Now()
                oData.StaticRouteModUser = Parameters.UserName
                oData.StaticRouteCompName = oComp.CompName
                oData.StaticRouteCompNumber = If(oComp.CompNumber, 0)
                oData.StaticRouteNatName = oComp.CompNatName
                oData.StaticRouteNatNumber = If(oComp.CompNatNumber, 0)

                'check for insert
                If (iStaticRouteControl = 0) Then
                    'this is an insert 
                    db.tblStaticRoutes.InsertOnSubmit(oData)
                Else
                    'This is an update 
                    db.tblStaticRoutes.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SavetblStaticRoute"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete a specific  Static Route Record
    ''' </summary>
    ''' <param name="iStaticRouteControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 01/07/2023 for New LTS Static Route Logic
    ''' </remarks>
    Public Function DeletetblStaticRoute(ByVal iStaticRouteControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iStaticRouteControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                'verify the record
                Dim oExisting = db.tblStaticRoutes.Where(Function(x) x.StaticRouteControl = iStaticRouteControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.StaticRouteControl = 0 Then Return True
                ValidateDeletedRecord(db, iStaticRouteControl, oExisting.StaticRouteNumber)
                db.tblStaticRoutes.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeletetblStaticRoute"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblStaticRoute)
        'Create New Record
        Return New LTS.tblStaticRoute With {.StaticRouteControl = d.StaticRouteControl _
                                                , .StaticRouteName = d.StaticRouteName _
                                                , .StaticRouteDescription = d.StaticRouteDescription _
                                                , .StaticRouteCompControl = d.StaticRouteCompControl _
                                                , .StaticRouteNumber = d.StaticRouteNumber _
                                                , .StaticRouteNatNumber = d.StaticRouteNatNumber _
                                                , .StaticRouteNatName = d.StaticRouteNatName _
                                                , .StaticRouteCompName = d.StaticRouteCompName _
                                                , .StaticRouteCompNumber = d.StaticRouteCompNumber _
                                                , .StaticRouteAutoTenderFlag = d.StaticRouteAutoTenderFlag _
                                                , .StaticRouteUseShipDateFlag = d.StaticRouteUseShipDateFlag _
                                                , .StaticRouteGuideDateSelectionDaysBefore = d.StaticRouteGuideDateSelectionDaysBefore _
                                                , .StaticRouteGuideDateSelectionDaysAfter = d.StaticRouteGuideDateSelectionDaysAfter _
                                                , .StaticRouteSplitOversizedLoads = d.StaticRouteSplitOversizedLoads _
                                                , .StaticRouteCapacityPreference = d.StaticRouteCapacityPreference _
                                                , .StaticRouteRequireAutoTenderApproval = d.StaticRouteRequireAutoTenderApproval _
                                                , .StaticRouteFillLargestFirst = d.StaticRouteFillLargestFirst _
                                                , .StaticRoutePlaceOnHold = d.StaticRoutePlaceOnHold _
                                                , .StaticRouteModDate = Date.Now _
                                                , .StaticRouteModUser = Parameters.UserName _
                                                , .StaticRouteURI = d.StaticRouteURI _
                                                , .StaticRouteUpdated = If(d.StaticRouteUpdated Is Nothing, New Byte() {}, d.StaticRouteUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblStaticRouteFiltered(Control:=CType(LinqTable, LTS.tblStaticRoute).StaticRouteControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim source As LTS.tblStaticRoute = TryCast(LinqTable, LTS.tblStaticRoute)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblStaticRoutes
                       Where d.StaticRouteControl = source.StaticRouteControl
                       Select New DTO.QuickSaveResults With {.Control = d.StaticRouteControl _
                                                            , .ModDate = d.StaticRouteModDate _
                                                            , .ModUser = d.StaticRouteModUser _
                                                            , .Updated = d.StaticRouteUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

    ''' <summary>
    ''' DTO base class override to validate delete of tblStaticRoute record
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="oData"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.3.006 on 01/07/2023 for New LTS Static Route Logic
    ''' </remarks>
    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the StaticRoute is being used by the lane data 
        With CType(oData, DTO.tblStaticRoute)
            Using db As New NGLMASLaneDataContext(ConnectionString)
                ValidateDeletedRecord(db, .StaticRouteControl, .StaticRouteNumber)
                Dim intRouteControl As Integer = .StaticRouteControl
            End Using
        End With
    End Sub

    ''' <summary>
    ''' Overload of DTO base class used to validate delete of tblStaticRoute record
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="iStaticRouteControl"></param>
    ''' <param name="sStaticRouteNumber"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 01/07/2023 for New LTS Static Route Logic
    ''' </remarks>
    Protected Overloads Sub ValidateDeletedRecord(ByRef db As NGLMASLaneDataContext, ByVal iStaticRouteControl As Integer, ByVal sStaticRouteNumber As String)
        'Check if the StaticRoute is being used by the lane data 
        Try
            Dim ret = (From d In db.Lanes
                       Where d.LaneRouteGuideControl = iStaticRouteControl
                       Select d.LaneControl, d.LaneNumber, d.LaneName).FirstOrDefault()
            If (Not ret Is Nothing AndAlso ret.LaneControl <> 0) Then
                Dim strDetails As String = "Cannot delete Route data.  The Route , " & sStaticRouteNumber & " is being used and cannot be deleted. check the lane information including lane number " & ret.LaneNumber & "."
                Utilities.SaveAppError(strDetails, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
            End If
        Catch ex As FaultException
            If ex.Message <> "E_NoData" Then
                Throw
            End If
        Catch ex As InvalidOperationException
            'Do nothing this indicates no data 
        Catch ex As Exception
            Throw
        End Try

    End Sub

    ''' <summary>
    ''' DTO base class override to validate update of tblStaticRoute record
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="oData"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.3.006 on 01/07/2023 for New LTS Static Route Logic
    ''' </remarks>
    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.tblStaticRoute)
            Try
                ValidateSaveRecord(CType(oDB, NGLMASLaneDataContext), .StaticRouteControl, .StaticRouteNumber)

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    ''' <summary>
    ''' New method used to validate new or updated tblStaticRoute record
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="iStaticRouteControl"></param>
    ''' <param name="sStaticRouteNumber"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 01/07/2023 for New LTS Static Route Logic
    ''' </remarks>
    Protected Sub ValidateSaveRecord(ByRef db As NGLMASLaneDataContext, ByVal iStaticRouteControl As Integer, ByVal sStaticRouteNumber As String)
        'Check if the data already exists only one allowed
        Try
            Dim strDetails As String = ""
            If String.IsNullOrWhiteSpace(sStaticRouteNumber) Then
                strDetails = "Cannot save Route changes.  The Route Number cannot be empty, please enter a Route Number."
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
            End If
            Dim Existing As LTS.tblStaticRoute = Nothing
            If iStaticRouteControl = 0 Then
                Existing = db.tblStaticRoutes.Where(Function(t) t.StaticRouteNumber = sStaticRouteNumber).FirstOrDefault()
            Else
                Existing = db.tblStaticRoutes.Where(Function(t) t.StaticRouteControl <> iStaticRouteControl And t.StaticRouteNumber = sStaticRouteNumber).FirstOrDefault()
            End If
            'oDB.Log = New DebugTextWriter
            If Not Existing Is Nothing AndAlso Existing.StaticRouteControl <> 0 Then
                strDetails = "Cannot save Route changes.  The Route Number, " & sStaticRouteNumber & " is not available, please enter a different Route Number."
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
            End If

        Catch ex As FaultException
            Throw
        Catch ex As InvalidOperationException
            'do nothing this is the desired result.
        End Try
    End Sub

#End Region

End Class

Public Class NGLtblStaticRouteCarrData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLaneDataContext(ConnectionString)
        Me.LinqTable = db.tblStaticRouteCarrs
        Me.LinqDB = db
        Me.SourceClass = "NGLtblStaticRouteCarrData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLaneDataContext(ConnectionString)
            Me.LinqTable = db.tblStaticRouteCarrs
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
        Return GettblStaticRouteCarrFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblStaticRouteCarrsFiltered()
    End Function

    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' Enter a StaticRouteControl for the FKControl parameter or zero to ignore it
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim tblStaticRouteCarr As DTO.tblStaticRouteCarr

                If LowerControl <> 0 Then
                    tblStaticRouteCarr = (
                   From d In db.tblStaticRouteCarrs
                   Where d.StaticRouteCarrControl >= LowerControl _
                   And
                   (FKControl = 0 OrElse d.StaticRouteCarrStaticRouteControl = FKControl)
                   Order By d.StaticRouteCarrControl
                   Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblStaticRouteCarr = (
                   From d In db.tblStaticRouteCarrs
                   Where (FKControl = 0 OrElse d.StaticRouteCarrStaticRouteControl = FKControl)
                   Order By d.StaticRouteCarrControl
                   Select selectDTOData(d, db)).FirstOrDefault
                End If

                Return tblStaticRouteCarr

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' Enter a StaticRouteControl for the FKControl parameter or zero to ignore it
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblStaticRouteCarr As DTO.tblStaticRouteCarr = (
                From d In db.tblStaticRouteCarrs
                Where d.StaticRouteCarrControl < CurrentControl _
                And
                (FKControl = 0 OrElse d.StaticRouteCarrStaticRouteControl = FKControl)
                Order By d.StaticRouteCarrControl Descending
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblStaticRouteCarr

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' Enter a StaticRouteControl for the FKControl parameter or zero to ignore it
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblStaticRouteCarr As DTO.tblStaticRouteCarr = (
                From d In db.tblStaticRouteCarrs
                Where d.StaticRouteCarrControl > CurrentControl _
                And
                (FKControl = 0 OrElse d.StaticRouteCarrStaticRouteControl = FKControl)
                Order By d.StaticRouteCarrControl
                Select selectDTOData(d, db)).FirstOrDefault

                Return tblStaticRouteCarr

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' Enter a StaticRouteControl for the FKControl parameter or zero to ignore it
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim tblStaticRouteCarr As DTO.tblStaticRouteCarr

                If UpperControl <> 0 Then

                    tblStaticRouteCarr = (
                    From d In db.tblStaticRouteCarrs
                    Where d.StaticRouteCarrControl >= UpperControl _
                    And
                    (FKControl = 0 OrElse d.StaticRouteCarrStaticRouteControl = FKControl)
                    Order By d.StaticRouteCarrControl
                    Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest StaticRouteCarrcontrol record
                    tblStaticRouteCarr = (
                    From d In db.tblStaticRouteCarrs
                    Order By d.StaticRouteCarrControl Descending
                    Select selectDTOData(d, db)).FirstOrDefault

                End If


                Return tblStaticRouteCarr

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblStaticRouteCarrFiltered(ByVal Control As Long) As DTO.tblStaticRouteCarr
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim StaticRouteCarr As DTO.tblStaticRouteCarr = (
                From d In db.tblStaticRouteCarrs
                Where
                    d.StaticRouteCarrControl = Control
                Select selectDTOData(d, db)).First


                Return StaticRouteCarr

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblStaticRouteCarrsFiltered(Optional ByVal StaticRouteControl As Integer = 0, Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblStaticRouteCarr()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("select COUNT(*) from dbo.tblStaticRouteCarr")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblStaticRouteCarrs() As DTO.tblStaticRouteCarr = (
                From d In db.tblStaticRouteCarrs
                Where (StaticRouteControl = 0 OrElse d.StaticRouteCarrStaticRouteControl = StaticRouteControl)
                Order By d.StaticRouteCarrName Ascending
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                Return tblStaticRouteCarrs

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblStaticRouteCarrsTreeDataFiltered(ByVal StaticRouteControl As Integer) As DTO.tblUserGroup()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try


                'Return all the contacts that match the criteria sorted by name
                Dim tblStaticRouteCarrs() As DTO.tblUserGroup = (
                From d In db.tblStaticRouteCarrs
                Where (StaticRouteControl = 0 OrElse d.StaticRouteCarrStaticRouteControl = StaticRouteControl)
                Order By d.StaticRouteCarrName Ascending
                Select New DTO.tblUserGroup With {.UserGroupsControl = d.StaticRouteCarrControl _
                                                 , .UserGroupsAltControl = d.StaticRouteCarrCarrierControl _
                                                 , .UserGroupsName = d.StaticRouteCarrName _
                                               , .UserGroupsUpdated = d.StaticRouteCarrUpdated.ToArray()}).ToArray()

                Return tblStaticRouteCarrs

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                'Utilities.SaveAppError(ex.Message, Me.Parameters)
                'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
                'its ok if there are no records
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Friend Function selectDTOData(ByVal d As LTS.tblStaticRouteCarr, ByRef db As NGLMASLaneDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblStaticRouteCarr
        Return New DTO.tblStaticRouteCarr With {.StaticRouteCarrControl = d.StaticRouteCarrControl _
                                                , .StaticRouteCarrName = d.StaticRouteCarrName _
                                                , .StaticRouteCarrDescription = d.StaticRouteCarrDescription _
                                                , .StaticRouteCarrCarrierControl = d.StaticRouteCarrCarrierControl _
                                                , .StaticRouteCarrStaticRouteControl = d.StaticRouteCarrStaticRouteControl _
                                                , .StaticRouteCarrCarrierNumber = d.StaticRouteCarrCarrierNumber _
                                                , .StaticRouteCarrCarrierName = d.StaticRouteCarrCarrierName _
                                                , .StaticRouteCarrRouteTypeCode = d.StaticRouteCarrRouteTypeCode _
                                                , .StaticRouteCarrTendLeadTime = d.StaticRouteCarrTendLeadTime _
                                                , .StaticRouteCarrAutoTenderFlag = d.StaticRouteCarrAutoTenderFlag _
                                                , .StaticRouteCarrMaxStops = d.StaticRouteCarrMaxStops _
                                                , .StaticRouteCarrHazmatFlag = d.StaticRouteCarrHazmatFlag _
                                                , .StaticRouteCarrTransType = d.StaticRouteCarrTransType _
                                                , .StaticRouteCarrRouteSequence = d.StaticRouteCarrRouteSequence _
                                                , .StaticRouteCarrAutoAcceptLoads = d.StaticRouteCarrAutoAcceptLoads _
                                                , .StaticRouteCarrRequireAutoTenderApproval = d.StaticRouteCarrRequireAutoTenderApproval _
                                                , .StaticRouteStateFilter = d.StaticRouteStateFilter _
                                               , .StaticRouteCarrModDate = d.StaticRouteCarrModDate _
                                               , .StaticRouteCarrModUser = d.StaticRouteCarrModUser _
                                               , .StaticRouteCarrURI = d.StaticRouteCarrURI,
                                                .Page = page,
                                                .Pages = pagecount,
                                                .RecordCount = recordcount,
                                                .PageSize = pagesize _
                                               , .StaticRouteCarrUpdated = d.StaticRouteCarrUpdated.ToArray()}
    End Function

#End Region


#Region "LTS Methods"

    ''' <summary>
    ''' Get a filtered array of Static Route Carrier Data
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 01/11/2023 for New LTS Static Route Logic
    ''' </remarks>
    Public Function GettblStaticRouteCarrs(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblStaticRouteCarr()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tblStaticRouteCarr
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.tblStaticRouteCarr)
                iQuery = db.tblStaticRouteCarrs
                Dim filterWhere = ""
                'verify:  one must exist
                '(a) StaticRouteCarrControl  
                '(b) StaticRouteCarrStaticRouteControl 
                '(c) ParentControl
                If Not filters.FilterValues.Any(Function(x) x.filterName = "StaticRouteCarrControl") Then
                    'we need a StaticRouteCarrStaticRouteControl fliter or a ParentControl number
                    If Not filters.FilterValues.Any(Function(x) x.filterName = "StaticRouteCarrStaticRouteControl") Then
                        If filters.ParentControl = 0 Then
                            Dim sMsg As String = "E_MissingParent" ' "  The reference to the parent  record is missing. Please select a valid booking record from the load planning page and try again."
                            throwNoDataFaultException(sMsg)
                        Else
                            filterWhere = " (StaticRouteCarrStaticRouteControl = " & filters.ParentControl & ") "
                        End If
                    End If
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblStaticRouteCarrs"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Insert or Update Static Route Carrier Data.  Note: the caller must limit access to valid carrier data for legal entity
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 01/11/2023 for New LTS Static Route Logic
    ''' </remarks>
    Public Function SavetblStaticRouteCarr(ByVal oData As LTS.tblStaticRouteCarr) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Dim strDetails = ""
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                If oData.StaticRouteCarrStaticRouteControl = 0 Then
                    strDetails = "Cannot save Routing Carrier changes.  The Routing Carrier has not been assigned to a valid Routing Guide, please select a valid Routing Guide and try again."
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                End If

                If (Not db.tblStaticRoutes.Any(Function(x) x.StaticRouteControl = oData.StaticRouteCarrStaticRouteControl)) Then
                    strDetails = "Cannot save Routing Carrier changes.  The Routing Carrier has not been assigned to a valid Routing Guide, please select a valid Routing Guide and try again."
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                End If

                If oData.StaticRouteCarrCarrierControl = 0 Then
                    strDetails = "Cannot save Routing Carrier changes.  The Routing Carrier has not been assigned to a valid Carrier, please select a valid Carrier and try again."
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                End If

                If (Not db.CarrierRefLanes.Any(Function(x) x.CarrierControl = oData.StaticRouteCarrCarrierControl)) Then
                    strDetails = "Cannot save Routing Carrier changes.  The Routing Carrier has not been assigned to a valid Carrier, please select a valid Carrier and try again."
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                End If

                If oData.StaticRouteCarrRouteTypeCode = 0 Then
                    strDetails = "Cannot save Routing Carrier changes.  The Routing Carrier has not been assigned a valid Route Type, please select a valid Route Type and try again."
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                End If

                If (Not db.tblRouteTypeRefLanes.Any(Function(x) x.RouteTypeControl = oData.StaticRouteCarrRouteTypeCode)) Then
                    strDetails = "Cannot save Routing Carrier changes.  The Routing Carrier has not been assigned a valid Route Type, please select a valid Route Type and try again."
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                End If

                Dim iStaticRouteCarrControl As Integer = oData.StaticRouteCarrControl
                oData.StaticRouteCarrModDate = Date.Now()
                oData.StaticRouteCarrModUser = Parameters.UserName
                'check for insert
                If (iStaticRouteCarrControl = 0) Then
                    'this is an insert 
                    db.tblStaticRouteCarrs.InsertOnSubmit(oData)
                Else
                    'This is an update 
                    db.tblStaticRouteCarrs.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SavetblStaticRouteCarr"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete a specific  Static Route Carrier Record
    ''' </summary>
    ''' <param name="iStaticRouteCarrControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 01/07/2023 for New LTS Static Route Logic
    ''' </remarks>
    Public Function DeletetblStaticRouteCarr(ByVal iStaticRouteCarrControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iStaticRouteCarrControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                'verify the record
                Dim oExisting = db.tblStaticRouteCarrs.Where(Function(x) x.StaticRouteCarrControl = iStaticRouteCarrControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.StaticRouteCarrControl = 0 Then Return True
                db.tblStaticRouteCarrs.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeletetblStaticRouteCarr"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblStaticRouteCarr)
        'Create New Record
        Return New LTS.tblStaticRouteCarr With {.StaticRouteCarrControl = d.StaticRouteCarrControl _
                                                , .StaticRouteCarrName = d.StaticRouteCarrName _
                                                , .StaticRouteCarrDescription = d.StaticRouteCarrDescription _
                                                , .StaticRouteCarrCarrierControl = d.StaticRouteCarrCarrierControl _
                                                , .StaticRouteCarrStaticRouteControl = d.StaticRouteCarrStaticRouteControl _
                                                , .StaticRouteCarrCarrierNumber = d.StaticRouteCarrCarrierNumber _
                                                , .StaticRouteCarrCarrierName = d.StaticRouteCarrCarrierName _
                                                , .StaticRouteCarrRouteTypeCode = d.StaticRouteCarrRouteTypeCode _
                                                , .StaticRouteCarrTendLeadTime = d.StaticRouteCarrTendLeadTime _
                                                , .StaticRouteCarrAutoTenderFlag = d.StaticRouteCarrAutoTenderFlag _
                                                , .StaticRouteCarrMaxStops = d.StaticRouteCarrMaxStops _
                                                , .StaticRouteCarrHazmatFlag = d.StaticRouteCarrHazmatFlag _
                                                , .StaticRouteCarrTransType = d.StaticRouteCarrTransType _
                                                , .StaticRouteCarrRouteSequence = d.StaticRouteCarrRouteSequence _
                                                , .StaticRouteCarrAutoAcceptLoads = d.StaticRouteCarrAutoAcceptLoads _
                                                , .StaticRouteCarrRequireAutoTenderApproval = d.StaticRouteCarrRequireAutoTenderApproval _
                                                , .StaticRouteStateFilter = d.StaticRouteStateFilter _
                                                , .StaticRouteCarrURI = d.StaticRouteCarrURI _
                                                , .StaticRouteCarrModDate = Date.Now _
                                                , .StaticRouteCarrModUser = Parameters.UserName _
                                                , .StaticRouteCarrUpdated = If(d.StaticRouteCarrUpdated Is Nothing, New Byte() {}, d.StaticRouteCarrUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblStaticRouteCarrFiltered(Control:=CType(LinqTable, LTS.tblStaticRouteCarr).StaticRouteCarrControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim source As LTS.tblStaticRouteCarr = TryCast(LinqTable, LTS.tblStaticRouteCarr)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblStaticRouteCarrs
                       Where d.StaticRouteCarrControl = source.StaticRouteCarrControl
                       Select New DTO.QuickSaveResults With {.Control = d.StaticRouteCarrControl _
                                                            , .ModDate = d.StaticRouteCarrModDate _
                                                            , .ModUser = d.StaticRouteCarrModUser _
                                                            , .Updated = d.StaticRouteCarrUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

#End Region

End Class

Public Class NGLtblStaticRouteEquipData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLaneDataContext(ConnectionString)
        Me.LinqTable = db.tblStaticRouteEquips
        Me.LinqDB = db
        Me.SourceClass = "NGLtblStaticRouteEquipData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLaneDataContext(ConnectionString)
            Me.LinqTable = db.tblStaticRouteEquips
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
        Return GettblStaticRouteEquipFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblStaticRouteEquipsFiltered()
    End Function

    Private Function selectDTOData(ByVal d As LTS.tblStaticRouteEquip, ByRef db As NGLMASLaneDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblStaticRouteEquip
        Return New DTO.tblStaticRouteEquip With {.StaticRouteEquipControl = d.StaticRouteEquipControl _
                                                , .StaticRouteEquipName = d.StaticRouteEquipName _
                                                , .StaticRouteEquipDescription = d.StaticRouteEquipDescription _
                                                , .StaticRouteEquipStaticRouteCarrControl = d.StaticRouteEquipStaticRouteCarrControl _
                                                , .StaticRouteEquipCarrierTruckControl = d.StaticRouteEquipCarrierTruckControl _
                                                , .StaticRouteEquipCarrierTruckDescription = d.StaticRouteEquipCarrierTruckDescription _
                                                , .StaticRouteEquipTruckDetails = IIf(d.CarrierTruckRefLane Is Nothing,
                                                                                      New DTO.CarrierTruck,
                                                                                      New DTO.CarrierTruck With
                                                                                      {.CarrierTruckControl = d.CarrierTruckRefLane.CarrierTruckControl _
                                                                                      , .CarrierTruckCarrierControl = If(d.CarrierTruckRefLane.CarrierTruckCarrierControl.HasValue, d.CarrierTruckRefLane.CarrierTruckCarrierControl.Value, 0) _
                                                                                      , .CarrierTruckDescription = d.CarrierTruckRefLane.CarrierTruckDescription _
                                                                                      , .CarrierTruckWgtFrom = If(d.CarrierTruckRefLane.CarrierTruckWgtFrom.HasValue, d.CarrierTruckRefLane.CarrierTruckWgtFrom.Value, 0) _
                                                                                      , .CarrierTruckWgtTo = If(d.CarrierTruckRefLane.CarrierTruckWgtTo.HasValue, d.CarrierTruckRefLane.CarrierTruckWgtTo.Value, 0) _
                                                                                      , .CarrierTruckRateStarts = d.CarrierTruckRefLane.CarrierTruckRateStarts _
                                                                                      , .CarrierTruckRateExpires = d.CarrierTruckRefLane.CarrierTruckRateExpires _
                                                                                      , .CarrierTruckTL = d.CarrierTruckRefLane.CarrierTruckTL _
                                                                                      , .CarrierTruckLTL = d.CarrierTruckRefLane.CarrierTruckLTL _
                                                                                      , .CarrierTruckEquipment = d.CarrierTruckRefLane.CarrierTruckEquipment _
                                                                                      , .CarrierTruckMileRate = If(d.CarrierTruckRefLane.CarrierTruckMileRate.HasValue, d.CarrierTruckRefLane.CarrierTruckMileRate.Value, 0) _
                                                                                      , .CarrierTruckCwtRate = If(d.CarrierTruckRefLane.CarrierTruckCwtRate.HasValue, d.CarrierTruckRefLane.CarrierTruckCwtRate.Value, 0) _
                                                                                      , .CarrierTruckCaseRate = If(d.CarrierTruckRefLane.CarrierTruckCaseRate.HasValue, d.CarrierTruckRefLane.CarrierTruckCaseRate.Value, 0) _
                                                                                      , .CarrierTruckFlatRate = If(d.CarrierTruckRefLane.CarrierTruckFlatRate.HasValue, d.CarrierTruckRefLane.CarrierTruckFlatRate.Value, 0) _
                                                                                      , .CarrierTruckPltRate = If(d.CarrierTruckRefLane.CarrierTruckPltRate.HasValue, d.CarrierTruckRefLane.CarrierTruckPltRate.Value, 0) _
                                                                                      , .CarrierTruckCubeRate = If(d.CarrierTruckRefLane.CarrierTruckCubeRate.HasValue, d.CarrierTruckRefLane.CarrierTruckCubeRate.Value, 0) _
                                                                                      , .CarrierTruckTLT = If(d.CarrierTruckRefLane.CarrierTruckTLT.HasValue, d.CarrierTruckRefLane.CarrierTruckTLT.Value, 0) _
                                                                                      , .CarrierTruckTMode = d.CarrierTruckRefLane.CarrierTruckTMode _
                                                                                      , .CarrierTruckFAK = d.CarrierTruckRefLane.CarrierTruckFAK _
                                                                                      , .CarrierTruckDisc = If(d.CarrierTruckRefLane.CarrierTruckDisc.HasValue, d.CarrierTruckRefLane.CarrierTruckDisc, 0) _
                                                                                      , .CarrierTruckPUMon = d.CarrierTruckRefLane.CarrierTruckPUMon _
                                                                                      , .CarrierTruckPUTue = d.CarrierTruckRefLane.CarrierTruckPUTue _
                                                                                      , .CarrierTruckPUWed = d.CarrierTruckRefLane.CarrierTruckPUWed _
                                                                                      , .CarrierTruckPUThu = d.CarrierTruckRefLane.CarrierTruckPUThu _
                                                                                      , .CarrierTruckPUFri = d.CarrierTruckRefLane.CarrierTruckPUFri _
                                                                                      , .CarrierTruckPUSat = d.CarrierTruckRefLane.CarrierTruckPUSat _
                                                                                      , .CarrierTruckPUSun = d.CarrierTruckRefLane.CarrierTruckPUSun _
                                                                                      , .CarrierTruckDLMon = d.CarrierTruckRefLane.CarrierTruckDLMon _
                                                                                      , .CarrierTruckDLTue = d.CarrierTruckRefLane.CarrierTruckDLTue _
                                                                                      , .CarrierTruckDLWed = d.CarrierTruckRefLane.CarrierTruckDLWed _
                                                                                      , .CarrierTruckDLThu = d.CarrierTruckRefLane.CarrierTruckDLThu _
                                                                                      , .CarrierTruckDLFri = d.CarrierTruckRefLane.CarrierTruckDLFri _
                                                                                      , .CarrierTruckDLSat = d.CarrierTruckRefLane.CarrierTruckDLSat _
                                                                                      , .CarrierTruckDLSun = d.CarrierTruckRefLane.CarrierTruckDLSun _
                                                                                      , .CarrierTruckPayTolPerLo = If(d.CarrierTruckRefLane.CarrierTruckPayTolPerLo.HasValue, d.CarrierTruckRefLane.CarrierTruckPayTolPerLo.Value, 0) _
                                                                                      , .CarrierTruckPayTolPerHi = If(d.CarrierTruckRefLane.CarrierTruckPayTolPerHi.HasValue, d.CarrierTruckRefLane.CarrierTruckPayTolPerHi.Value, 0) _
                                                                                      , .CarrierTruckPayTolCurLo = If(d.CarrierTruckRefLane.CarrierTruckPayTolCurLo.HasValue, d.CarrierTruckRefLane.CarrierTruckPayTolCurLo.Value, 0) _
                                                                                      , .CarrierTruckPayTolCurHi = If(d.CarrierTruckRefLane.CarrierTruckPayTolCurHi.HasValue, d.CarrierTruckRefLane.CarrierTruckPayTolCurHi.Value, 0) _
                                                                                      , .CarrierTruckCurType = If(d.CarrierTruckRefLane.CarrierTruckCurType.HasValue, d.CarrierTruckRefLane.CarrierTruckCurType.Value, 0) _
                                                                                      , .CarrierTruckModUser = d.CarrierTruckRefLane.CarrierTruckModUser _
                                                                                      , .CarrierTruckModDate = d.CarrierTruckRefLane.CarrierTruckModDate _
                                                                                      , .CarrierTruckRoute = d.CarrierTruckRefLane.CarrierTruckRoute _
                                                                                      , .CarrierTruckMiles = If(d.CarrierTruckRefLane.CarrierTruckMiles.HasValue, d.CarrierTruckRefLane.CarrierTruckMiles.Value, 0) _
                                                                                      , .CarrierTruckBkhlCostPerc = If(d.CarrierTruckRefLane.CarrierTruckBkhlCostPerc.HasValue, d.CarrierTruckRefLane.CarrierTruckBkhlCostPerc.Value, 0) _
                                                                                      , .CarrierTruckPalletCostPer = If(d.CarrierTruckRefLane.CarrierTruckPalletCostPer.HasValue, d.CarrierTruckRefLane.CarrierTruckPalletCostPer.Value, 0) _
                                                                                      , .CarrierTruckFuelSurChargePerc = If(d.CarrierTruckRefLane.CarrierTruckFuelSurChargePerc.HasValue, d.CarrierTruckRefLane.CarrierTruckFuelSurChargePerc.Value, 0) _
                                                                                      , .CarrierTruckStopCharge = If(d.CarrierTruckRefLane.CarrierTruckStopCharge.HasValue, d.CarrierTruckRefLane.CarrierTruckStopCharge.Value, 0) _
                                                                                      , .CarrierTruckDropCost = d.CarrierTruckRefLane.CarrierTruckDropCost _
                                                                                      , .CarrierTruckUnloadDiff = d.CarrierTruckRefLane.CarrierTruckUnloadDiff _
                                                                                      , .CarrierTruckCasesAvailable = d.CarrierTruckRefLane.CarrierTruckCasesAvailable _
                                                                                      , .CarrierTruckCasesOpen = d.CarrierTruckRefLane.CarrierTruckCasesOpen _
                                                                                      , .CarrierTruckCasesCommitted = d.CarrierTruckRefLane.CarrierTruckCasesCommitted _
                                                                                      , .CarrierTruckWgtAvailable = d.CarrierTruckRefLane.CarrierTruckWgtAvailable _
                                                                                      , .CarrierTruckWgtOpen = d.CarrierTruckRefLane.CarrierTruckWgtOpen _
                                                                                      , .CarrierTruckWgtCommitted = d.CarrierTruckRefLane.CarrierTruckWgtCommitted _
                                                                                      , .CarrierTruckCubesAvailable = d.CarrierTruckRefLane.CarrierTruckCubesAvailable _
                                                                                      , .CarrierTruckCubesOpen = d.CarrierTruckRefLane.CarrierTruckCubesOpen _
                                                                                      , .CarrierTruckCubesCommitted = d.CarrierTruckRefLane.CarrierTruckCubesCommitted _
                                                                                      , .CarrierTruckPltsAvailable = d.CarrierTruckRefLane.CarrierTruckPltsAvailable _
                                                                                      , .CarrierTruckPltsOpen = d.CarrierTruckRefLane.CarrierTruckPltsOpen _
                                                                                      , .CarrierTruckPltsCommitted = d.CarrierTruckRefLane.CarrierTruckPltsCommitted _
                                                                                      , .CarrierTruckTrucksAvailable = d.CarrierTruckRefLane.CarrierTruckTrucksAvailable _
                                                                                      , .CarrierTruckMaxLoadsByWeek = d.CarrierTruckRefLane.CarrierTruckMaxLoadsByWeek _
                                                                                      , .CarrierTruckMaxLoadsByMonth = d.CarrierTruckRefLane.CarrierTruckMaxLoadsByMonth _
                                                                                      , .CarrierTruckTotalLoadsForWeek = d.CarrierTruckRefLane.CarrierTruckTotalLoadsForWeek _
                                                                                      , .CarrierTruckTotalLoadsForMonth = d.CarrierTruckRefLane.CarrierTruckTotalLoadsForMonth _
                                                                                      , .CarrierTruckWeekDate = d.CarrierTruckRefLane.CarrierTruckWeekDate _
                                                                                      , .CarrierTruckMonthDate = d.CarrierTruckRefLane.CarrierTruckMonthDate _
                                                                                      , .CarrierTruckTempType = d.CarrierTruckRefLane.CarrierTruckTempType _
                                                                                      , .CarrierTruckHazmat = d.CarrierTruckRefLane.CarrierTruckHazmat _
                                                                                      , .LocalCarrierTruckCodes = db.getCarrierTruckCodeStringRefLane(d.CarrierTruckRefLane.CarrierTruckControl) _
                                                                                      , .CarrierTruckUpdated = d.CarrierTruckRefLane.CarrierTruckUpdated.ToArray()
                                                                                      }) _
                                                                    , .StaticRouteEquipModDate = d.StaticRouteEquipModDate _
                                                                    , .StaticRouteEquipModUser = d.StaticRouteEquipModUser _
                                                                    , .StaticRouteEquipUpdated = d.StaticRouteEquipUpdated.ToArray(),
                                                                   .Page = page,
                                                                   .Pages = pagecount,
                                                                   .RecordCount = recordcount,
                                                                   .PageSize = pagesize}
    End Function

    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' Enter a StaticRouteCarrControl for the FKControl parameter or zero to ignore it
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.tblStaticRouteEquip)(Function(t As LTS.tblStaticRouteEquip) t.CarrierTruckRefLane)
                db.LoadOptions = oDLO

                Dim tblStaticRouteEquip As DTO.tblStaticRouteEquip

                If LowerControl <> 0 Then
                    tblStaticRouteEquip = (
                   From d In db.tblStaticRouteEquips
                   Where d.StaticRouteEquipControl >= LowerControl _
                   And
                   (FKControl = 0 OrElse d.StaticRouteEquipStaticRouteCarrControl = FKControl)
                   Order By d.StaticRouteEquipControl
                   Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblStaticRouteEquip = (
                   From d In db.tblStaticRouteEquips
                   Where (FKControl = 0 OrElse d.StaticRouteEquipStaticRouteCarrControl = FKControl)
                   Order By d.StaticRouteEquipControl
                   Select selectDTOData(d, db)).FirstOrDefault
                End If

                Return tblStaticRouteEquip

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' Enter a StaticRouteCarrControl for the FKControl parameter or zero to ignore it
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.tblStaticRouteEquip)(Function(t As LTS.tblStaticRouteEquip) t.CarrierTruckRefLane)
                db.LoadOptions = oDLO
                'Get the first record that matches the provided criteria
                Dim tblStaticRouteEquip As DTO.tblStaticRouteEquip = (
                From d In db.tblStaticRouteEquips
                Where d.StaticRouteEquipControl < CurrentControl _
                And
                (FKControl = 0 OrElse d.StaticRouteEquipStaticRouteCarrControl = FKControl)
                Order By d.StaticRouteEquipControl Descending
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblStaticRouteEquip

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' Enter a StaticRouteCarrControl for the FKControl parameter or zero to ignore it
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.tblStaticRouteEquip)(Function(t As LTS.tblStaticRouteEquip) t.CarrierTruckRefLane)
                db.LoadOptions = oDLO
                'Get the first record that matches the provided criteria
                Dim tblStaticRouteEquip As DTO.tblStaticRouteEquip = (
                From d In db.tblStaticRouteEquips
                Where d.StaticRouteEquipControl > CurrentControl _
                And
                (FKControl = 0 OrElse d.StaticRouteEquipStaticRouteCarrControl = FKControl)
                Order By d.StaticRouteEquipControl
                Select selectDTOData(d, db)).FirstOrDefault


                Return tblStaticRouteEquip

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' Enter a StaticRouteCarrControl for the FKControl parameter or zero to ignore it
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.tblStaticRouteEquip)(Function(t As LTS.tblStaticRouteEquip) t.CarrierTruckRefLane)
                db.LoadOptions = oDLO
                Dim tblStaticRouteEquip As DTO.tblStaticRouteEquip

                If UpperControl <> 0 Then

                    tblStaticRouteEquip = (
                    From d In db.tblStaticRouteEquips
                    Where d.StaticRouteEquipControl >= UpperControl _
                    And
                    (FKControl = 0 OrElse d.StaticRouteEquipStaticRouteCarrControl = FKControl)
                    Order By d.StaticRouteEquipControl
                    Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest StaticRouteEquipcontrol record
                    tblStaticRouteEquip = (
                    From d In db.tblStaticRouteEquips
                    Order By d.StaticRouteEquipControl Descending
                    Select selectDTOData(d, db)).FirstOrDefault

                End If


                Return tblStaticRouteEquip

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblStaticRouteEquipFiltered(ByVal Control As Long) As DTO.tblStaticRouteEquip
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.tblStaticRouteEquip)(Function(t As LTS.tblStaticRouteEquip) t.CarrierTruckRefLane)
                db.LoadOptions = oDLO
                'Get the newest record that matches the provided criteria
                Dim StaticRouteEquip As DTO.tblStaticRouteEquip = (
                From d In db.tblStaticRouteEquips
                Where
                    d.StaticRouteEquipControl = Control
                Select selectDTOData(d, db)).First


                Return StaticRouteEquip

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblStaticRouteEquipsFiltered(Optional ByVal StaticRouteCarrControl As Integer = 0, Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblStaticRouteEquip()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.tblStaticRouteEquip)(Function(t As LTS.tblStaticRouteEquip) t.CarrierTruckRefLane)
                db.LoadOptions = oDLO
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("select COUNT(*) from dbo.tblStaticRouteEquip")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblStaticRouteEquips() As DTO.tblStaticRouteEquip = (
                From d In db.tblStaticRouteEquips
                Where (StaticRouteCarrControl = 0 OrElse d.StaticRouteEquipStaticRouteCarrControl = StaticRouteCarrControl)
                Order By d.StaticRouteEquipControl Descending
                Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                Return tblStaticRouteEquips

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblStaticRouteEquipsTreeFiltered(ByVal StaticRouteCarrControl As Integer) As DTO.tblUserGroup()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                'Return all the contacts that match the criteria sorted by name
                Dim tblStaticRouteEquips() As DTO.tblUserGroup = (
                From d In db.tblStaticRouteEquips
                Where (StaticRouteCarrControl = 0 OrElse d.StaticRouteEquipStaticRouteCarrControl = StaticRouteCarrControl)
                Order By d.StaticRouteEquipName Ascending
                Select New DTO.tblUserGroup With {.UserGroupsControl = d.StaticRouteEquipControl _
                                                , .UserGroupsName = d.StaticRouteEquipName _
                                                , .UserGroupsDescription = d.StaticRouteEquipDescription _
                                                , .UserGroupsIcon = "ApplicationCheck16"
                                                }).ToArray()

                Return tblStaticRouteEquips

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                'Utilities.SaveAppError(ex.Message, Me.Parameters)
                'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
                'Its ok if there are not any results.
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function
#End Region


#Region "LTS Methods"

    ''' <summary>
    ''' Get a filtered array of Static Route Equipment Data
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 01/11/2023 for New LTS Static Route Logic
    ''' </remarks>
    Public Function GettblStaticRouteEquips(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblStaticRouteEquip()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tblStaticRouteEquip
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.tblStaticRouteEquip)
                iQuery = db.tblStaticRouteEquips
                Dim filterWhere = ""
                'verify:  one must exist
                '(a) StaticRouteEquipControl  
                '(b) StaticRouteEquipStaticRouteCarrControl  
                '(c) ParentControl
                If Not filters.FilterValues.Any(Function(x) x.filterName = "StaticRouteEquipControl") Then
                    'we need a StaticRouteEquipStaticRouteCarrControl fliter or a ParentControl number
                    If Not filters.FilterValues.Any(Function(x) x.filterName = "StaticRouteEquipStaticRouteCarrControl") Then
                        If filters.ParentControl = 0 Then
                            Dim sMsg As String = "E_MissingParent" ' "  The reference to the parent  record is missing. Please select a valid booking record from the load planning page and try again."
                            throwNoDataFaultException(sMsg)
                        Else
                            filterWhere = " (StaticRouteEquipStaticRouteCarrControl = " & filters.ParentControl & ") "
                        End If
                    End If
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblStaticRouteEquips"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Insert or Update Static Route Equipment Data.  Note: the caller must limit access to valid Equipment data for legal entity
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 01/11/2023 for New LTS Static Route Logic
    ''' </remarks>
    Public Function SavetblStaticRouteEquip(ByVal oData As LTS.tblStaticRouteEquip) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Dim strDetails = ""
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                If oData.StaticRouteEquipStaticRouteCarrControl = 0 Then
                    strDetails = "Cannot save Routing Equipment changes.  The Routing Equipment has not been assigned to a valid Routing Guide Carrier, please select a valid Routing Guide Carrier and try again."
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                End If

                If (Not db.tblStaticRouteCarrs.Any(Function(x) x.StaticRouteCarrControl = oData.StaticRouteEquipStaticRouteCarrControl)) Then
                    strDetails = "Cannot save Routing Equipment changes.  The Routing Equipment has not been assigned to a valid Routing Guide Carrier, please select a valid Routing Guide Carrier and try again."
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                End If

                If oData.StaticRouteEquipCarrierTruckControl = 0 Then
                    strDetails = "Cannot save Routing Equipment changes.  The Routing Equipment has not been assigned a valid Carrier Truck Equipmnet Type, please select a valid Carrier Truck Equipmnet Type and try again."
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                End If

                If (Not db.tblRouteTypeRefLanes.Any(Function(x) x.RouteTypeControl = oData.StaticRouteEquipCarrierTruckControl)) Then
                    strDetails = "Cannot save Routing Equipment changes.  The Routing Equipment has not been assigned a valid Carrier Truck Equipmnet Type, please select a valid Carrier Truck Equipmnet Type and try again."
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                End If

                Dim iStaticRouteEquipControl As Integer = oData.StaticRouteEquipControl
                oData.StaticRouteEquipModDate = Date.Now()
                oData.StaticRouteEquipModUser = Parameters.UserName
                'check for insert
                If (iStaticRouteEquipControl = 0) Then
                    'this is an insert 
                    db.tblStaticRouteEquips.InsertOnSubmit(oData)
                Else
                    'This is an update 
                    db.tblStaticRouteEquips.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SavetblStaticRouteEquip"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete a specific  Static Route Equipment Record
    ''' </summary>
    ''' <param name="iStaticRouteEquipControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 01/07/2023 for New LTS Static Route Logic
    ''' </remarks>
    Public Function DeletetblStaticRouteEquip(ByVal iStaticRouteEquipControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iStaticRouteEquipControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                'verify the record
                Dim oExisting = db.tblStaticRouteEquips.Where(Function(x) x.StaticRouteEquipControl = iStaticRouteEquipControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.StaticRouteEquipControl = 0 Then Return True
                db.tblStaticRouteEquips.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeletetblStaticRouteEquip"), db)
            End Try
        End Using
        Return blnRet
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblStaticRouteEquip)
        'Create New Record
        Return New LTS.tblStaticRouteEquip With {.StaticRouteEquipControl = d.StaticRouteEquipControl _
                                                , .StaticRouteEquipName = d.StaticRouteEquipName _
                                                , .StaticRouteEquipDescription = d.StaticRouteEquipDescription _
                                                , .StaticRouteEquipStaticRouteCarrControl = d.StaticRouteEquipStaticRouteCarrControl _
                                                , .StaticRouteEquipCarrierTruckControl = d.StaticRouteEquipCarrierTruckControl _
                                                , .StaticRouteEquipCarrierTruckDescription = d.StaticRouteEquipCarrierTruckDescription _
                                                , .StaticRouteEquipModDate = Date.Now _
                                                , .StaticRouteEquipModUser = Parameters.UserName _
                                                , .StaticRouteEquipUpdated = If(d.StaticRouteEquipUpdated Is Nothing, New Byte() {}, d.StaticRouteEquipUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblStaticRouteEquipFiltered(Control:=CType(LinqTable, LTS.tblStaticRouteEquip).StaticRouteEquipControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim source As LTS.tblStaticRouteEquip = TryCast(LinqTable, LTS.tblStaticRouteEquip)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblStaticRouteEquips
                       Where d.StaticRouteEquipControl = source.StaticRouteEquipControl
                       Select New DTO.QuickSaveResults With {.Control = d.StaticRouteEquipControl _
                                                            , .ModDate = d.StaticRouteEquipModDate _
                                                            , .ModUser = d.StaticRouteEquipModUser _
                                                            , .Updated = d.StaticRouteEquipUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

#End Region

End Class



''' <summary>
''' Used by Lane integration to lookup address information from the comp table where possible
''' </summary>
''' <remarks></remarks>
Public Class LaneIntegrationAddressData
    Implements ICloneable

    Private _CompControl As Integer
    Public Property CompControl() As Integer
        Get
            Return _CompControl
        End Get
        Set(ByVal value As Integer)
            _CompControl = value
        End Set
    End Property

    Private _CompNumber As String
    Public Property CompNumber() As String
        Get
            Return _CompNumber
        End Get
        Set(ByVal value As String)
            _CompNumber = value
        End Set
    End Property

    Private _LegalEntity As String = ""
    Public Property LegalEntity As String
        Get
            Return _LegalEntity
        End Get
        Set(value As String)
            _LegalEntity = value
        End Set
    End Property

    Private _CompAlphaCode As String = ""
    Public Property CompAlphaCode() As String
        Get
            Return _CompAlphaCode
        End Get
        Set(ByVal value As String)
            _CompAlphaCode = value
        End Set
    End Property

    Private _CompName As String
    Public Property CompName() As String
        Get
            Return _CompName
        End Get
        Set(ByVal value As String)
            _CompName = value
        End Set
    End Property

    Private _Address1 As String
    Public Property Address1() As String
        Get
            Return _Address1
        End Get
        Set(ByVal value As String)
            _Address1 = value
        End Set
    End Property

    Private _Address2 As String
    Public Property Address2() As String
        Get
            Return _Address2
        End Get
        Set(ByVal value As String)
            _Address2 = value
        End Set
    End Property

    Private _Address3 As String
    Public Property Address3() As String
        Get
            Return _Address3
        End Get
        Set(ByVal value As String)
            _Address3 = value
        End Set
    End Property

    Private _City As String
    Public Property City() As String
        Get
            Return _City
        End Get
        Set(ByVal value As String)
            _City = value
        End Set
    End Property

    Private _Country As String
    Public Property Country() As String
        Get
            Return _Country
        End Get
        Set(ByVal value As String)
            _Country = value
        End Set
    End Property

    Private _State As String
    Public Property State() As String
        Get
            Return _State
        End Get
        Set(ByVal value As String)
            _State = value
        End Set
    End Property

    Private _Zip As String
    Public Property Zip() As String
        Get
            Return _Zip
        End Get
        Set(ByVal value As String)
            _Zip = value
        End Set
    End Property

    Private _ContactPhone As String
    Public Property ContactPhone() As String
        Get
            Return _ContactPhone
        End Get
        Set(ByVal value As String)
            _ContactPhone = value
        End Set
    End Property

    Private _ContactPhoneExt As String
    Public Property ContactPhoneExt() As String
        Get
            Return _ContactPhoneExt
        End Get
        Set(ByVal value As String)
            _ContactPhoneExt = value
        End Set
    End Property

    Private _ContactFax As String
    Public Property ContactFax() As String
        Get
            Return _ContactFax
        End Get
        Set(ByVal value As String)
            _ContactFax = value
        End Set
    End Property

    Private _ContactEmail As String
    Public Property ContactEmail() As String
        Get
            Return _ContactEmail
        End Get
        Set(ByVal value As String)
            _ContactEmail = value
        End Set
    End Property


    Private _HasAddressChanged As Boolean = False
    ''' <summary>
    ''' If the new address does not match the existing address then we set this flag to true
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HasAddressChanged() As Boolean
        Get
            Return _HasAddressChanged
        End Get
        Set(ByVal value As Boolean)
            _HasAddressChanged = value
        End Set
    End Property


    Public Function Clone() As Object Implements ICloneable.Clone
        Dim instance As New LaneIntegrationAddressData
        instance = DirectCast(MemberwiseClone(), LaneIntegrationAddressData)
        Return instance
    End Function

End Class

Public Class NGLHDMData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASLaneDataContext(ConnectionString)
        Me.LinqTable = db.tblHDMs
        Me.LinqDB = db
        Me.SourceClass = "NGLHDMData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASLaneDataContext(ConnectionString)
            Me.LinqTable = db.tblHDMs
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
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim objectlist As DTO.tblHdm = (
                From t In db.tblHDMs
                Where
                    (Control = 0 OrElse t.HDMControl = Control)
                Order By t.HDMControl
                Select SelectDTOData(t)).FirstOrDefault()

                Return objectlist

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Friend Shared Function SelectDTOData(ByVal d As LTS.tblHDM, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblHdm
        Dim oDTO As New DTO.tblHdm
        Dim skipObjs As New List(Of String) From {"HDMUpdated", "Page", "Pages", "RecordCount", "PageSize", "Selected"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .HDMUpdated = d.HDMUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO

    End Function

    'Added By LVV on 9/26/16 for v-7.0.5.110 HDM Enhancement
    Friend Shared Function SelectDTOData(ByVal d As LTS.tblHDMZip, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblHDMZip
        Dim oDTO As New DTO.tblHDMZip
        Dim skipObjs As New List(Of String) From {"HDMZipUpdated", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .HDMZipUpdated = d.HDMZipUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO

    End Function

    'Added By LVV on 9/21/16 for v-7.0.5.110 HDM Enhancement
    Public Function GetHDMsByCarrTarControl(ByVal CarrTarControl As Integer,
                                            Optional ByVal page As Integer = 1,
                                            Optional ByVal pagesize As Integer = 1000) As DTO.tblHdm()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'Select all the HDMControls associated with CarrTarControl

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                Dim oQuery = From d In db.tblHDMTariffXrefs Join t In db.tblHDMs On d.HDMTariffXrefHDMControl Equals t.HDMControl
                             Where d.HDMTariffXrefCarrTarControl = CarrTarControl
                             Select t

                intRecordCount = oQuery.Count
                If intRecordCount < 1 Then Return Nothing
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                Dim oRecords() As DTO.tblHdm = (
                    From d In oQuery
                    Order By d.HDMControl
                    Select SelectDTOData(d, page, intPageCount, intRecordCount, pagesize)
                    ).Skip(intSkip).Take(pagesize).ToArray()

                Return oRecords

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetHDMsByCarrTarControl"))
            End Try

            Return Nothing

        End Using
    End Function

    'Added By LVV on 9/21/16 for v-7.0.5.110 HDM Enhancement
    Public Function GetTariffHDMs(ByVal CarrTarControl As Integer,
                                  Optional ByVal page As Integer = 1,
                                  Optional ByVal pagesize As Integer = 1000,
                                  Optional ByVal filterWhere As String = "",
                                  Optional ByVal sSortKey As String = "") As DTO.tblHdm()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                'Select all the HDMControls associated with CarrTarControl

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                Dim CarrierControl As Integer = 0

                CarrierControl = (
                From d In db.CarrierTariffRefLanes
                Where (d.CarrTarControl = CarrTarControl)
                Select d.CarrTarCarrierControl).FirstOrDefault()

                Dim oXref = (From d In db.tblHDMTariffXrefs
                             Where d.HDMTariffXrefCarrTarControl = CarrTarControl
                             Select d).ToList()

                Dim oQuery = From d In db.tblHDMs
                             Where d.HDMCarrierControl = CarrierControl
                             Select d

                If oQuery Is Nothing Then Return Nothing
                If Not String.IsNullOrEmpty(filterWhere) Then
                    oQuery = DLinqUtil.filterWhere(oQuery, filterWhere)
                End If

                intRecordCount = oQuery.Count
                If intRecordCount < 1 Then Return Nothing
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                Dim oRecords() As DTO.tblHdm

                If Not String.IsNullOrWhiteSpace(sSortKey) Then
                    'example sort key "CarrTarNonServCountry ASC,CarrTarNonServState DESC,CarrTarNonServCity ASC"
                    Dim nQuery = oQuery.OrderBy(sSortKey)
                    oRecords = (
                        From d In oQuery
                        Select SelectDTOData(d, page, intPageCount, intRecordCount, pagesize)
                        ).Skip(intSkip).Take(pagesize).ToArray()
                Else
                    'sort using default values
                    oRecords = (
                        From d In oQuery
                        Order By d.HDMControl
                        Select SelectDTOData(d, page, intPageCount, intRecordCount, pagesize)
                        ).Skip(intSkip).Take(pagesize).ToArray()
                End If

                If (Not oRecords Is Nothing AndAlso oRecords.Count > 0) AndAlso (Not oXref Is Nothing AndAlso oXref.Count) Then
                    For Each r In oRecords
                        If oXref.Any(Function(x) x.HDMTariffXrefHDMControl = r.HDMControl) Then
                            r.Selected = True
                        End If
                    Next
                End If

                Return oRecords

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetTariffHDMs"))
            End Try

            Return Nothing

        End Using
    End Function

    'Added By LVV on 9/21/16 for v-7.0.5.110 HDM Enhancement
    Public Function GetHDMTreeFlat(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer, ByRef intNextTreeID As Integer) As List(Of DTO.NGLTreeNode)
        Try
            Dim oTreeNodes As List(Of DTO.NGLTreeNode) = GetHDMNodes(CarrTarControl, ParentTreeID)
            If Not oTreeNodes Is Nothing AndAlso oTreeNodes.Count > 0 Then
                Dim intNextChildTreeID As Integer = intNextTreeID + oTreeNodes.Count
                For Each node In oTreeNodes
                    Dim intNodeTreeID = incrementID(intNextTreeID)
                    node.TreeID = intNodeTreeID
                Next
                intNextTreeID = intNextChildTreeID
            End If
            Return oTreeNodes
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("GetHDMTreeFlat"))
        End Try
        Return Nothing
    End Function

    'Added By LVV on 9/21/16 for v-7.0.5.110 HDM Enhancement
    Public Function GetHDMNodes(ByVal CarrTarControl As Integer, ByVal ParentTreeID As Integer) As List(Of DTO.NGLTreeNode)
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                Dim hdms = GetTariffHDMs(CarrTarControl)
                If (Not hdms Is Nothing) AndAlso (hdms.Count > 0) Then

                    Dim oNodes As List(Of DTO.NGLTreeNode) = (
                    From t In hdms
                    Order By t.HDMControl
                    Select New DTO.NGLTreeNode With {.Control = t.HDMControl,
                                                     .ParentTreeID = ParentTreeID,
                                                     .Name = t.HDMName,
                                                     .Description = t.HDMDesc,
                                                     .ClassName = "tblHdm"}).ToList

                    Return oNodes
                End If
                Return Nothing
            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                Return Nothing
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetHDMNodes"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Inserts or Updates an HDM Header Table Record.
    ''' If Selected is true then adds a record to tblHDMTariffXref
    ''' Returns the updated/inserted HDM record
    ''' </summary>
    ''' <param name="hdm"></param>
    ''' <param name="CarrTarControl"></param>
    ''' <remarks>
    ''' Added By LVV on 9/26/16 for v-7.0.5.110 HDM Enhancement
    ''' </remarks>
    Public Function InsertOrUpdateTariffHDM(ByVal hdm As DTO.tblHdm, ByVal CarrTarControl As Integer) As Integer
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oSPData = (From d In db.spInsertOrUpdateTariffHDM(hdm.HDMControl, hdm.HDMCarrierControl, hdm.HDMName, hdm.HDMDesc, Me.Parameters.UserName, CarrTarControl, hdm.Selected, hdm.HDMMinimum, hdm.HDMVariable, hdm.HDMVariableCode, hdm.HDMMaximum, hdm.HDMLEAdminControl, hdm.HDMActive, hdm.HDMCompControl) Select d).FirstOrDefault()

                If oSPData Is Nothing Then Return 0

                If Not oSPData Is Nothing AndAlso oSPData.ErrNumber <> 0 Then
                    throwSQLFaultException(oSPData.RetMsg)
                End If

                Return oSPData.HDMControl

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateTariffHDM"))
            End Try
        End Using
    End Function

    ''' <summary>
    ''' Delete an HDM by Control
    ''' Also deletes any existing CarrierTariffFees and LaneProfileXref using Accessorial Codes.
    ''' If refrences exist in BookFees, BookFeesPending, LaneFees, and BookRevHistoricalFees
    ''' then set AccessorialHDMControl = 0 and rename the fee in Accessorial Del-[SCAC]-[Name].
    ''' Otherwise, delete the Accessorial Fee.
    ''' Finally, Deletes the record from tblHDM.
    ''' Records in tblHDMTariffXref and tblHDMZips will also automatically be deleted because of referential integrity
    ''' </summary>
    ''' <param name="HDMControl"></param>
    ''' <param name="Username"></param>
    ''' <remarks>
    ''' Added By LVV on 9/27/16 for v-7.0.5.110 HDM Enhancement
    ''' </remarks>
    Public Sub DeleteHDM(ByVal HDMControl As Integer, Optional ByVal Username As String = Nothing)
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                db.spDeleteHDM(HDMControl, Username)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteHDM"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Gets the HDMZips for the provided HDMControl
    ''' </summary>
    ''' <param name="HDMControl"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sSortKey"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 9/26/16 for v-7.0.5.110 HDM Enhancement
    ''' </remarks>
    Public Function GetTariffHDMZips(ByVal HDMControl As Integer,
                                     Optional ByVal page As Integer = 1,
                                     Optional ByVal pagesize As Integer = 1000,
                                     Optional ByVal filterWhere As String = "",
                                     Optional ByVal sSortKey As String = "") As DTO.tblHDMZip()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                Dim oQuery = From d In db.tblHDMZips
                             Where d.HDMZipHDMControl = HDMControl
                             Select d

                If oQuery Is Nothing Then Return Nothing
                If Not String.IsNullOrEmpty(filterWhere) Then
                    oQuery = DLinqUtil.filterWhere(oQuery, filterWhere)
                End If

                intRecordCount = oQuery.Count
                If intRecordCount < 1 Then Return Nothing
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize
                Dim oRecords() As DTO.tblHDMZip

                If Not String.IsNullOrWhiteSpace(sSortKey) Then
                    'example sort key "CarrTarNonServCountry ASC,CarrTarNonServState DESC,CarrTarNonServCity ASC"
                    Dim nQuery = oQuery.OrderBy(sSortKey)
                    oRecords = (
                           From d In nQuery
                           Select SelectDTOData(d, page, intPageCount, intRecordCount, pagesize)
                           ).Skip(intSkip).Take(pagesize).ToArray()
                Else
                    'sort using default values
                    oRecords = (
                    From d In oQuery
                    Order By d.HDMZipCountry, d.HDMZipState, d.HDMZipCity, d.HDMZipTo, d.HDMZipFrom
                    Select SelectDTOData(d, page, intPageCount, intRecordCount, pagesize)
                    ).Skip(intSkip).Take(pagesize).ToArray()
                End If

                Return oRecords

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetTariffHDMZips"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Calls stored procedure which Inserts or Updates an HDM Regions
    ''' Zip Code Range Record. Automatically refreshes
    ''' Lane HDM Profile Specific Fees based on the new
    ''' and old Zip Codes.
    ''' </summary>
    ''' <param name="hdmz"></param>
    ''' <param name="Username"></param>
    ''' <remarks>
    ''' Added By LVV on 9/26/16 for v-7.0.5.110 HDM Enhancement
    ''' </remarks>
    Public Sub InsertOrUpdateTariffHDMZip(ByVal hdmz As DTO.tblHDMZip, Optional ByVal Username As String = Nothing)
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                db.spUpdateHDMZipRange(hdmz.HDMZipHDMControl, hdmz.HDMZipControl, hdmz.HDMZipFrom, hdmz.HDMZipTo, hdmz.HDMZipCity, hdmz.HDMZipState, hdmz.HDMZipCountry, Username)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateTariffHDMZip"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Deletes an HDM Regions Zip Code Range Record.  
    ''' Automatically refreshes Lane HDM Profile Specific Fees 
    ''' based on the removed Zip Codes.
    ''' </summary>
    ''' <param name="HDMZipControl"></param>
    ''' <param name="Username"></param>
    ''' <remarks>
    ''' Added By LVV on 9/28/16 for v-7.0.5.110 HDM Enhancement
    ''' </remarks>
    Public Sub DeleteHDMZipRange(ByVal HDMZipControl As Integer, Optional ByVal Username As String = Nothing)
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                db.spDeleteHDMZipRange(HDMZipControl, Username)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteHDMZipRange"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Delete all the HDM locations for this HDM Control
    ''' </summary>
    ''' <param name="HDMControl"></param>
    ''' <remarks>
    ''' Creaed by RHR for v-8.5.0.001 on 11/09/2021
    ''' </remarks>
    Public Sub DeleteAllHDMZip(ByVal HDMControl As Integer)
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                db.spDeleteHDMZipRangesForCarrierTariffs(HDMControl, Parameters.UserName)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteAllHDMZip"))
            End Try
        End Using
    End Sub

    'Added By LVV on 10/5/16 for v-7.0.5.110 HDM Import Tool
    Public Function GetHDMFilteredByCarrier(ByVal CarrierControl As Integer) As DTO.tblHdm()
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim objectlist() As DTO.tblHdm = (
                From t In db.tblHDMs
                Where
                    (t.HDMCarrierControl = CarrierControl)
                Order By t.HDMControl
                Select SelectDTOData(t)).ToArray()

                Return objectlist

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetHDMFilteredByCarrier"))
            End Try

            Return Nothing

        End Using
    End Function

    'Added By LVV on 10/6/16 for v-7.0.5.110 HDM Import Tool
    Public Function GetHDMByName(ByVal HDMName As String) As DTO.tblHdm
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Dim hdm As New DTO.tblHdm
            Try
                hdm = (
                    From t In db.tblHDMs
                    Where
                    (t.HDMName = HDMName)
                    Select SelectDTOData(t)).FirstOrDefault()
                Return hdm
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetHDMByName"))
            End Try
            Return Nothing
        End Using
    End Function


#End Region

#Region "LTS Methods"

    Public Function GetLELaneHDM(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vtblHDM()
        Dim oRet() As LTS.vtblHDM
        If filters Is Nothing Then Return Nothing

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                Dim iLEAdminControl = filters.LEAdminControl
                If (iLEAdminControl = 0) Then
                    iLEAdminControl = Parameters.UserLEControl
                Else
                    If iLEAdminControl <> Parameters.UserLEControl Then
                        'access denied
                        throwDataValidationException("The requested data does not exist or you are not authorized to access it.", SqlFaultInfo.FaultDetailsKey.E_ServerMsgDetails, True)
                        Return Nothing ' access denied
                    End If
                End If
                If (iLEAdminControl = 0) Then
                    throwDataValidationException("The requested data does not exist.", SqlFaultInfo.FaultDetailsKey.E_ServerMsgDetails, True)
                    Return Nothing
                End If
                Dim iQuery As IQueryable(Of LTS.vtblHDM) = db.vtblHDMs.Where(Function(x) x.HDMLEAdminControl = iLEAdminControl)
                Dim filterWhere = ""
                If Not filters.FilterValues.Any(Function(x) x.filterName = "HDMActive") Then
                    filterWhere = " (HDMActive = true) "
                End If

                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLELaneHDM"), db)
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetLELaneHDMTemplate(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vtblHDM()
        Dim oRet() As LTS.vtblHDM
        If filters Is Nothing Then Return Nothing

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                Dim iQuery As IQueryable(Of LTS.vtblHDM) = db.vtblHDMs.Where(Function(x) x.HDMLEAdminControl = 0)
                Dim filterWhere = ""

                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLELaneHDMTemplate"), db)
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetLEHDMZip(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vtblHDMZip()
        Dim oRet() As LTS.vtblHDMZip
        If filters Is Nothing Then Return Nothing

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try

                Dim filterWhere = ""
                Dim iZipControl As Integer = 0
                Dim iHDMControl As Integer
                If filters.FilterValues.Any(Function(x) x.filterName = "HDMZipControl") Then
                    Integer.TryParse(filters.FilterValues.Where(Function(x) x.filterName = "HDMZipControl").Select(Function(y) y.filterValueFrom).FirstOrDefault(), iZipControl)
                End If
                If iZipControl = 0 Then

                    iHDMControl = filters.ParentControl

                    If iHDMControl = 0 Then
                        If filters.FilterValues.Any(Function(x) x.filterName = "HDMZipHDMControl") Then
                            Integer.TryParse(filters.FilterValues.Where(Function(x) x.filterName = "HDMZipHDMControl").Select(Function(y) y.filterValueFrom).FirstOrDefault(), iHDMControl)
                        End If
                    Else
                        filterWhere = " (HDMZipHDMControl = " & iHDMControl.ToString() & ") "
                    End If
                    If iHDMControl = 0 Then
                        Dim lDetails As New List(Of String) From {"HDM Record Reference", " was not provided and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return Nothing
                    Else
                        If Not db.tblHDMs.Any(Function(x) x.HDMControl = iHDMControl) Then
                            Dim lDetails As New List(Of String) From {"HDM Record Reference", " was not valid and "}
                            throwInvalidKeyParentRequiredException(lDetails)
                            Return Nothing
                        End If
                    End If
                Else
                    ' get the iHDMControl
                    iHDMControl = db.tblHDMZips.Where(Function(x) x.HDMZipControl = iZipControl).Select(Function(y) y.HDMZipHDMControl).FirstOrDefault()
                End If
                'verify that the user is authorized to access this data
                Dim iLEAdminControl As Integer? = db.tblHDMs.Where(Function(x) x.HDMControl = iHDMControl).Select(Function(y) y.HDMLEAdminControl).FirstOrDefault()
                If iLEAdminControl.HasValue AndAlso iLEAdminControl > 0 Then
                    'verify that the user is authorized to access this data
                    If iLEAdminControl <> Parameters.UserLEControl Then
                        throwDataValidationException("The requested data does not exist or you are not authorized to access it.", SqlFaultInfo.FaultDetailsKey.E_ServerMsgDetails, True)
                        Return Nothing ' access denied
                    End If
                End If
                Dim iQuery As IQueryable(Of LTS.vtblHDMZip) = db.vtblHDMZips
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLEHDMZip"), db)
            End Try

            Return Nothing

        End Using
    End Function


    Public Function GetTariffHDM(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vtblHDMTariffXref()
        Dim oRet() As LTS.vtblHDMTariffXref
        If filters Is Nothing Then Return Nothing

        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim filterWhere As String = ""
                Dim iXrefControl As Integer = 0
                Dim iCarrTarControl As Integer = 0
                If filters.FilterValues.Any(Function(x) x.filterName = "HDMTariffXrefControl") Then
                    Integer.TryParse(filters.FilterValues.Where(Function(x) x.filterName = "HDMTariffXrefControl").Select(Function(y) y.filterValueFrom).FirstOrDefault(), iXrefControl)
                End If
                If iXrefControl = 0 Then

                    iCarrTarControl = filters.ParentControl
                    'If iXrefControl = 0 Then
                    '    Dim sMsg As String = " The Tariff ID is missing. Please return to the tariff contract page and select a valid carrier contract."
                    '    throwNoDataFaultException(sMsg)
                    'End If
                    If iCarrTarControl = 0 Then
                        If filters.FilterValues.Any(Function(x) x.filterName = "HDMTariffXrefCarrTarControl") Then
                            Integer.TryParse(filters.FilterValues.Where(Function(x) x.filterName = "HDMTariffXrefCarrTarControl").Select(Function(y) y.filterValueFrom).FirstOrDefault(), iCarrTarControl)
                        End If
                    Else
                        filterWhere = " (HDMTariffXrefCarrTarControl = " & iCarrTarControl.ToString() & ") "
                    End If
                    If iCarrTarControl = 0 Then
                        Dim lDetails As New List(Of String) From {"Tariff Contract Record Reference", " was not provided and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return Nothing
                    End If
                Else
                    filterWhere = " (HDMTariffXrefControl = " & iXrefControl & ") "
                End If
                Dim iQuery As IQueryable(Of LTS.vtblHDMTariffXref)
                iQuery = db.vtblHDMTariffXrefs

                Dim sFilterSpacer As String = " And "

                If Not filters.FilterValues.Any(Function(x) x.filterName = "HDMActive") Then
                    filterWhere = filterWhere & sFilterSpacer & " (HDMActive = true) "
                End If

                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetTariffHDM"), db)
            End Try

            Return Nothing

        End Using
    End Function


    Public Function GetAvailableTariffHDM(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vtblHDMTariffAvailable()
        Dim oRet() As LTS.vtblHDMTariffAvailable
        If filters Is Nothing Then Return Nothing
        If filters.ParentControl = 0 Then
            Dim sMsg As String = " The Tariff ID is missing. Please return to the tariff contract page and select a valid carrier contract."
            throwNoDataFaultException(sMsg)
        End If
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try


                Dim iQuery As IQueryable(Of LTS.vtblHDMTariffAvailable)
                iQuery = db.vtblHDMTariffAvailables
                Dim filterWhere As String = " (CarrTarControl = " & filters.ParentControl & ") "
                Dim sFilterSpacer As String = " And "

                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAvailableTariffHDM"), db)
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetLELaneHDMLocation(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vtblHDMLocation()
        Dim oRet() As LTS.vtblHDMLocation
        If filters Is Nothing Then Return Nothing
        If filters.ParentControl = 0 Then
            Dim sMsg As String = " The HDM data is missing. Please return to the HDM page and select a valid record."
            throwNoDataFaultException(sMsg)
        End If
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try


                Dim iQuery As IQueryable(Of LTS.vtblHDMLocation)
                iQuery = db.vtblHDMLocations
                Dim filterWhere As String = " (HDMControl = " & filters.ParentControl & ") "
                Dim sFilterSpacer As String = " And "

                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLELaneHDMLocation"), db)
            End Try

            Return Nothing

        End Using
    End Function

    Public Sub InsertOrUpdateTariffHDMZip(ByVal hdmz As LTS.vtblHDMZip)
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim iHDMControl As Integer = hdmz.HDMZipHDMControl
                Dim iHDMZipControl As Integer = hdmz.HDMZipControl
                Dim sHDMZipFrom As String = If(String.IsNullOrWhiteSpace(hdmz.HDMZipFrom), "", hdmz.HDMZipFrom)
                Dim sHDMZipTo As String = If(String.IsNullOrWhiteSpace(hdmz.HDMZipTo), "", hdmz.HDMZipTo)
                Dim sHDMZipCity = If(String.IsNullOrWhiteSpace(hdmz.HDMZipCity), "", hdmz.HDMZipCity)
                Dim sHDMZipState = If(String.IsNullOrWhiteSpace(hdmz.HDMZipState), "", hdmz.HDMZipState)
                Dim sHDMZipCountry = If(String.IsNullOrWhiteSpace(hdmz.HDMZipCountry), "", hdmz.HDMZipCountry)
                Dim sLocation = String.Concat(sHDMZipFrom, sHDMZipTo, sHDMZipCity, sHDMZipState, sHDMZipCountry)
                If sLocation.Trim.Length > 0 Then
                    'only process the udpate if the location is not empty
                    If sHDMZipCountry.Trim.Length < 1 Then sHDMZipCountry = "US"  'set US as the default if it is missing but only if some location data is provied
                    db.spUpdateHDMZipRange(iHDMControl, iHDMZipControl, sHDMZipFrom, sHDMZipTo, sHDMZipCity, sHDMZipState, sHDMZipCountry, Me.Parameters.UserName)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateTariffHDMZip"))
            End Try
        End Using
    End Sub

    Public Function InsertOrUpdateTariffHDM(ByVal hdm As LTS.vtblHDM, ByVal CarrTarControl As Integer, Optional bSelected As Boolean = True) As Integer
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim oSPData = (From d In db.spInsertOrUpdateTariffHDM(hdm.HDMControl, hdm.HDMCarrierControl, If(String.IsNullOrWhiteSpace(hdm.HDMName), "", hdm.HDMName), If(String.IsNullOrWhiteSpace(hdm.HDMDesc), "", hdm.HDMDesc), Me.Parameters.UserName, CarrTarControl, bSelected, hdm.HDMMinimum, hdm.HDMVariable, hdm.HDMVariableCode, hdm.HDMMaximum, hdm.HDMLEAdminControl, hdm.HDMActive, hdm.HDMCompControl) Select d).FirstOrDefault()

                If oSPData Is Nothing Then Return 0

                If Not oSPData Is Nothing AndAlso oSPData.ErrNumber <> 0 Then
                    throwSQLFaultException(oSPData.RetMsg)
                End If

                Return oSPData.HDMControl

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateTariffHDM"))
            End Try
        End Using
    End Function


    Public Function InsertTariffHDM(ByVal iHDMControl As Integer, ByVal CarrTarControl As Integer) As Integer
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim hdm As LTS.tblHDM = db.tblHDMs.Where(Function(x) x.HDMControl = iHDMControl).FirstOrDefault()
                If (hdm IsNot Nothing AndAlso hdm.HDMControl <> 0) Then
                    Dim oSPData = (From d In db.spInsertOrUpdateTariffHDM(hdm.HDMControl, hdm.HDMCarrierControl, hdm.HDMName, hdm.HDMDesc, Me.Parameters.UserName, CarrTarControl, True, hdm.HDMMinimum, hdm.HDMVariable, hdm.HDMVariableCode, hdm.HDMMaximum, hdm.HDMLEAdminControl, hdm.HDMActive, hdm.HDMCompControl) Select d).FirstOrDefault()
                    If oSPData Is Nothing Then Return 0

                    If Not oSPData Is Nothing AndAlso oSPData.ErrNumber <> 0 Then
                        throwSQLFaultException(oSPData.RetMsg)
                    End If

                    Return oSPData.HDMControl
                Else
                    Return 0
                End If


            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertTariffHDM"))
            End Try
        End Using
    End Function

    Public Function DeleteTariffHDM(ByVal iHDMControl As Integer, ByVal CarrTarControl As Integer) As Integer
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim hdm As LTS.tblHDM = db.tblHDMs.Where(Function(x) x.HDMControl = iHDMControl).FirstOrDefault()
                If (hdm IsNot Nothing AndAlso hdm.HDMControl <> 0) Then
                    Dim oSPData = (From d In db.spInsertOrUpdateTariffHDM(hdm.HDMControl, hdm.HDMCarrierControl, hdm.HDMName, hdm.HDMDesc, Me.Parameters.UserName, CarrTarControl, False, hdm.HDMMinimum, hdm.HDMVariable, hdm.HDMVariableCode, hdm.HDMMaximum, hdm.HDMLEAdminControl, hdm.HDMActive, hdm.HDMCompControl) Select d).FirstOrDefault()
                    If oSPData Is Nothing Then Return 0

                    If Not oSPData Is Nothing AndAlso oSPData.ErrNumber <> 0 Then
                        throwSQLFaultException(oSPData.RetMsg)
                    End If

                    Return oSPData.HDMControl
                Else
                    Return 0
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateTariffHDM"))
            End Try
        End Using
    End Function


#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DTO.tblHdm)

        'Modified By LVV on 9/21/16 for v-7.0.5.110 HDM Enhancement
        Dim skipObjs As New List(Of String) From {"HDMUpdated", "HDMModDate", "HDMModUser", "Page", "Pages", "RecordCount", "PageSize", "Selected"}
        Dim oLTS As New LTS.tblHDM
        oLTS = CopyMatchingFields(oLTS, d, skipObjs)
        'Create New Record
        With oLTS
            .HDMModDate = Date.Now
            .HDMModUser = Me.Parameters.UserName
            .HDMUpdated = If(d.HDMUpdated Is Nothing, New Byte() {}, d.HDMUpdated)
        End With
        Return oLTS
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetRecordFiltered(CType(LinqTable, LTS.tblHDM).HDMControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASLaneDataContext(ConnectionString)
            Try
                Dim source As LTS.tblHDM = TryCast(LinqTable, LTS.tblHDM)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblHDMs
                       Where d.HDMControl = source.HDMControl
                       Select New DTO.QuickSaveResults With {.Control = d.HDMControl _
                                                            , .ModDate = d.HDMModDate _
                                                            , .ModUser = d.HDMModUser _
                                                            , .Updated = d.HDMUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

#End Region

End Class