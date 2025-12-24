Public Class NGLCarrierProNumberData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierProNumbers
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierProNumberData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then


                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierProNumbers
                Me.LinqDB = db
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
        Return GetCarrierProNumberFiltered(Control)
    End Function

    ''' <summary>
    ''' not supported
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim CarrierProNumber As DataTransferObjects.CarrierProNumber

                If LowerControl <> 0 Then
                    CarrierProNumber = (
                        From d In db.CarrierProNumbers
                            Where d.CarrProControl >= LowerControl
                            Order By d.CarrProControl
                            Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    CarrierProNumber = (
                        From d In db.CarrierProNumbers
                            Order By d.CarrProControl
                            Select selectDTOData(d, db)).FirstOrDefault
                End If

                Return CarrierProNumber

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetFirstRecord"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the first record that matches the provided criteria
                Dim CarrierProNumber As DataTransferObjects.CarrierProNumber = (
                        From d In db.CarrierProNumbers
                        Where d.CarrProControl < CurrentControl
                        Order By d.CarrProControl Descending
                        Select selectDTOData(d, db)).FirstOrDefault

                Return CarrierProNumber
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPreviousRecord"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim CarrierProNumber As DataTransferObjects.CarrierProNumber = (
                        From d In db.CarrierProNumbers
                        Where d.CarrProControl > CurrentControl
                        Order By d.CarrProControl
                        Select selectDTOData(d, db)).FirstOrDefault

                Return CarrierProNumber
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNextRecord"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim CarrierProNumber As DataTransferObjects.CarrierProNumber

                If UpperControl <> 0 Then

                    CarrierProNumber = (
                        From d In db.CarrierProNumbers
                            Where d.CarrProControl >= UpperControl
                            Order By d.CarrProControl
                            Select selectDTOData(d, db)).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest CarrProcontrol record
                    CarrierProNumber = (
                        From d In db.CarrierProNumbers
                            Order By d.CarrProControl Descending
                            Select selectDTOData(d, db)).FirstOrDefault

                End If

                Return CarrierProNumber
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLastRecord"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrierProNumberFiltered(ByVal Control As Integer) As DataTransferObjects.CarrierProNumber
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim CarrierProNumber As DataTransferObjects.CarrierProNumber = (
                        From d In db.CarrierProNumbers
                        Where
                        d.CarrProControl = Control
                        Select selectDTOData(d, db)).FirstOrDefault()

                Return CarrierProNumber
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierProNumberFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrierProNumbersFiltered(ByVal CarrierControl As Integer,
                                                 Optional ByVal CompControl As Integer = 0,
                                                 Optional ByVal ProName As String = "",
                                                 Optional ByVal page As Integer = 1,
                                                 Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.CarrierProNumber()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 1
                Dim intPageCount As Integer = 1
                If pagesize < 1 Then pagesize = 1
                If page < 1 Then page = 1
                Dim intSkip As Integer = (page - 1) * pagesize
                Dim pros = From d In db.CarrierProNumbers
                        Where
                        d.CarrProCarrierControl = CarrierControl _
                        And
                        (CompControl = 0 OrElse d.CarrProCompControl = CompControl) _
                        And
                        (String.IsNullOrWhiteSpace(ProName) OrElse d.CarrProName = ProName)
                        Select Control = d.CarrProControl
                intRecordCount = pros.Count
                If intRecordCount < 1 Then Return Nothing
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                'Return all the records that match the criteria sorted by name
                Dim CarrierProNumbers() As DataTransferObjects.CarrierProNumber = (
                        From d In db.CarrierProNumbers
                        Where
                        d.CarrProCarrierControl = CarrierControl _
                        And
                        (CompControl = 0 OrElse d.CarrProCompControl = CompControl) _
                        And
                        (String.IsNullOrWhiteSpace(ProName) OrElse d.CarrProName = ProName)
                        Order By d.CarrProName
                        Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return CarrierProNumbers
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierProNumbersFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Lookup CarrierProNumbers by Legal Entity Carrier Control reference
    ''' </summary>
    ''' <param name="iLECarControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 01/05/23 look up carrier control from tblLegalEntityCarriers
    ''' </remarks>
    Public Function GetCarrierProNumbersByLE(ByVal iLECarControl As Integer) As DataTransferObjects.CarrierProNumber()
        Dim iCarrierControl As Integer = 0
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            iCarrierControl = db.tblLegalEntityCarriers.Where(Function(x) x.LECarControl = iLECarControl).Select(Function(x) x.LECarCarrierControl).FirstOrDefault()
        End Using
        Return GetCarrierProNumbersFiltered(iCarrierControl)
    End Function

    ''' <summary>
    ''' Used by System processes forces delete and bypasses validation 
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal oData As DataTransferObjects.CarrierProNumber)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            'Create New Record
            Dim nObject = CopyDTOToLinq(oData)
            db.CarrierProNumbers.Attach(nObject, True)
            db.CarrierProNumbers.DeleteOnSubmit(nObject)
            Try
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SystemDelete"), db)
            End Try
            DeleteCleanUp(nObject)
        End Using
    End Sub

    ''' <summary>
    ''' Used by system processes to force a delete bypassing validation 
    ''' </summary>
    ''' <param name="CarrProControl"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal CarrProControl As Integer)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            'Get the record by control number
            Dim nObject = db.CarrierProNumbers.Where(Function(x) x.CarrProControl = CarrProControl).FirstOrDefault()
            If Not nObject Is Nothing AndAlso nObject.CarrProControl <> 0 Then
                db.CarrierProNumbers.DeleteOnSubmit(nObject)
                Try
                    db.SubmitChanges()
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("SystemDelete"), db)
                End Try
                DeleteCleanUp(nObject)
            End If
        End Using

    End Sub


    ''' <summary>
    ''' Calls spGetNextCarrierProSeed which returns the next pro seed number 
    ''' if one is available or zero if a seed number cannot be generated a return 
    ''' value of -1 indicates that a system error occured and the caller should try again 
    ''' or look for an alternative option. Errors are 
    ''' generally transmitted via alerts. If zero is returned the pro number configuraton is marked as 
    ''' in-active and the caller should search for an alternate pro number configuration.
    ''' </summary>
    ''' <param name="CarrProControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getNextProSeed(ByVal CarrProControl As Integer) As Long
        Dim lngRet As Long = 0
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim nObject = db.spGetNextCarrierProSeed(CarrProControl).FirstOrDefault()
                If Not nObject Is Nothing Then
                    lngRet = If(nObject.Column1, 0)
                    If lngRet = 0 Then SetCarrProNumberInactive(db, CarrProControl)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getNextProSeed"))
            End Try
        End Using
        Return lngRet
    End Function

    Public Function getNextCarrierProBlock(ByVal CarrProControl As Integer, Optional UnattendedExecution As Boolean = True) As LTS.spGetNextCarrierProNumberBlockResult

        Dim nRet As New LTS.spGetNextCarrierProNumberBlockResult
        Dim MarkAsUsed As Boolean = True
        Dim BookProNumber As String = Nothing
        Dim ProNbrBlockNumberKey As String = Nothing
        Dim UserName As String = Me.Parameters.UserName

        Dim strRet As String = ""
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                nRet = db.spGetNextCarrierProNumberBlock(CarrProControl, UnattendedExecution, MarkAsUsed, BookProNumber, ProNbrBlockNumberKey, UserName).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getNextCarrierProBlock"))
            End Try
        End Using
        Return nRet
    End Function

    ''' <summary>
    ''' Returns the current seed or the first seed if the current seed is less than the first seed.  
    ''' Used for sample pro number generation.  
    ''' The caller may increase the seed to generate additional samples.
    ''' </summary>
    ''' <param name="CarrProControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getSampleProSeed(ByVal CarrProControl As Integer) As Long
        Dim intRet As Long = 0
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                intRet = (From d In db.CarrierProNumbers Where d.CarrProControl = CarrProControl Select If(d.CarrProSeedCurrent < d.CarrProSeedStart, d.CarrProSeedStart, d.CarrProSeedCurrent)).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getSampleProSeed"))
            End Try
        End Using
        Return intRet
    End Function

    Public Function GetCarrierProNumberByTarEquip(ByVal CarrTarEquipControl As Integer) As DataTransferObjects.CarrierProNumber
        'CarrTarEquipCarrProName
        Dim oRet As DataTransferObjects.CarrierProNumber
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim strProName = (From d In db.CarrierTariffEquipments Where d.CarrTarEquipControl = CarrTarEquipControl Select d.CarrTarEquipCarrProName).FirstOrDefault()
                If Not String.IsNullOrWhiteSpace(strProName) Then
                    'we have a match
                    oRet = (From d In db.CarrierProNumbers
                        Where d.CarrProName = strProName _
                              And d.CarrProActive = True
                        Order By d.CarrProSeedStart
                        Select selectDTOData(d, db)).FirstOrDefault()

                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getSampleProSeed"))
            End Try
        End Using
        Return oRet
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrierProNumber)
        'Create New Record
        Return New LTS.CarrierProNumber With {.CarrProControl = d.CarrProControl _
            , .CarrProCarrierControl = d.CarrProCarrierControl _
            , .CarrProCompControl = d.CarrProCompControl _
            , .CarrProChkDigAlgControl = d.CarrProChkDigAlgControl _
            , .CarrProName = d.CarrProName _
            , .CarrProDesc = d.CarrProName & " - " & d.CarrProSeedStart.ToString() _
            , .CarrProPrefix = d.CarrProPrefix _
            , .CarrProPrefixSpacer = d.CarrProPrefixSpacer _
            , .CarrProSuffix = d.CarrProSuffix _
            , .CarrProSuffixSpacer = d.CarrProSuffixSpacer _
            , .CarrProCheckDigitSpacer = d.CarrProCheckDigitSpacer _
            , .CarrProPrintCheckDigitOnSeperateBarCode = d.CarrProPrintCheckDigitOnSeperateBarCode _
            , .CarrProCheckDigitSplitWeightFactorDigits = d.CarrProCheckDigitSplitWeightFactorDigits _
            , .CarrProPrintSpacersOnBarCode = d.CarrProPrintSpacersOnBarCode _
            , .CarrProActive = d.CarrProActive _
            , .CarrProAppendPrefixForCheckDigit = d.CarrProAppendPrefixForCheckDigit _
            , .CarrProAppendSuffixForCheckDigit = d.CarrProAppendSuffixForCheckDigit _
            , .CarrProSeedStart = d.CarrProSeedStart _
            , .CarrProSeedEnd = d.CarrProSeedEnd _
            , .CarrProSeedCurrent = d.CarrProSeedCurrent _
            , .CarrProSeedStepFactor = d.CarrProSeedStepFactor _
            , .CarrProSeedWarningSeed = d.CarrProSeedWarningSeed _
            , .CarrProLength = d.CarrProLength _
            , .CarrProCheckDigitWeightFactor = d.CarrProCheckDigitWeightFactor _
            , .CarrProCheckDigitUseIndexForWeightFactor = d.CarrProCheckDigitUseIndexForWeightFactor _
            , .CarrProCheckDigitIndexForWeightFactorMin = d.CarrProCheckDigitIndexForWeightFactorMin _
            , .CarrProCheckDigitErrorCode = d.CarrProCheckDigitErrorCode _
            , .CarrProCheckDigit10Code = d.CarrProCheckDigit10Code _
            , .CarrProCheckDigitZeroCode = d.CarrProCheckDigitZeroCode _
            , .CarrProExp1 = d.CarrProExp1 _
            , .CarrProExp2 = d.CarrProExp2 _
            , .CarrProExp3 = d.CarrProExp3 _
            , .CarrProExp4 = d.CarrProExp4 _
            , .CarrProUser1 = d.CarrProUser1 _
            , .CarrProUser2 = d.CarrProUser2 _
            , .CarrProUser3 = d.CarrProUser3 _
            , .CarrProUser4 = d.CarrProUser4 _
            , .CarrProCheckDigitUseSubtractionFactor = d.CarrProCheckDigitUseSubtractionFactor _
            , .CarrProCheckDigitSubtractionFactor = d.CarrProCheckDigitSubtractionFactor _
            , .CarrProModDate = Date.Now _
            , .CarrProModUser = Parameters.UserName _
            , .CarrProUpdated = If(d.CarrProUpdated Is Nothing, New Byte() {}, d.CarrProUpdated)}

    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrierProNumberFiltered(Control:=CType(LinqTable, LTS.CarrierProNumber).CarrProControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierProNumber = TryCast(LinqTable, LTS.CarrierProNumber)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierProNumbers
                    Where d.CarrProControl = source.CarrProControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrProControl _
                        , .ModDate = d.CarrProModDate _
                        , .ModUser = d.CarrProModUser _
                        , .Updated = d.CarrProUpdated.ToArray}).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetQuickSaveResults"))
            End Try

        End Using
        Return ret
    End Function

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'check if the pro number configuraiton is being use by one of the tariffs
        With CType(oData, DataTransferObjects.CarrierProNumber)
            Dim strName As String = .CarrProName
            Dim active As String = .CarrProActive
            'Get the newest record that matches the provided criteria
            Dim pronum As DataTransferObjects.CarrierProNumber = (
                    From t In CType(oDB, NGLMASCarrierDataContext).CarrierProNumbers
                    Where
                    (t.CarrProControl = .CarrProControl)
                    Select New DataTransferObjects.CarrierProNumber With {.CarrProControl = t.CarrProControl, .CarrProName = t.CarrProName, .CarrProActive = t.CarrProActive}).FirstOrDefault()

            'do not allow them to change the name if it is in use.  the name is stored in the carrierequipment table.
            If DirectCast(oDB, NGLMASCarrierDataContext).CarrierTariffEquipments.Any(Function(x) x.CarrTarEquipCarrProName = pronum.CarrProName) Then
                If Not pronum Is Nothing AndAlso Not pronum.CarrProName = strName Then
                    throwCannotSaveInUseException("CarrProName", pronum.CarrProName)
                End If
                If Not pronum Is Nothing AndAlso active = False AndAlso pronum.CarrProActive = True Then
                    throwCannotSaveInUseException("CarrProActive", pronum.CarrProActive)
                End If
            End If
        End With
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'check if the pro number configuraiton is being use by one of the tariffs
        With CType(oData, DataTransferObjects.CarrierProNumber)
            Dim strName As String = .CarrProName
            If DirectCast(oDB, NGLMASCarrierDataContext).CarrierTariffEquipments.Any(Function(x) x.CarrTarEquipCarrProName = strName) Then
                throwCannotDeleteRecordInUseException("CarrProName", strName)
            End If
        End With
    End Sub

    Friend Function selectDTOData(ByVal d As LTS.CarrierProNumber, ByRef db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrierProNumber
        Return New DataTransferObjects.CarrierProNumber With {.CarrProControl = d.CarrProControl _
            , .CarrProCarrierControl = d.CarrProCarrierControl _
            , .CarrProCompControl = d.CarrProCompControl _
            , .CarrProChkDigAlgControl = d.CarrProChkDigAlgControl _
            , .CarrProName = d.CarrProName _
            , .CarrProDesc = d.CarrProDesc _
            , .CarrProPrefix = d.CarrProPrefix _
            , .CarrProPrefixSpacer = d.CarrProPrefixSpacer _
            , .CarrProSuffix = d.CarrProSuffix _
            , .CarrProSuffixSpacer = d.CarrProSuffixSpacer _
            , .CarrProCheckDigitSpacer = d.CarrProCheckDigitSpacer _
            , .CarrProPrintCheckDigitOnSeperateBarCode = d.CarrProPrintCheckDigitOnSeperateBarCode _
            , .CarrProCheckDigitSplitWeightFactorDigits = d.CarrProCheckDigitSplitWeightFactorDigits _
            , .CarrProPrintSpacersOnBarCode = d.CarrProPrintSpacersOnBarCode _
            , .CarrProActive = d.CarrProActive _
            , .CarrProAppendPrefixForCheckDigit = d.CarrProAppendPrefixForCheckDigit _
            , .CarrProAppendSuffixForCheckDigit = d.CarrProAppendSuffixForCheckDigit _
            , .CarrProSeedStart = d.CarrProSeedStart _
            , .CarrProSeedEnd = d.CarrProSeedEnd _
            , .CarrProSeedCurrent = d.CarrProSeedCurrent _
            , .CarrProSeedStepFactor = d.CarrProSeedStepFactor _
            , .CarrProSeedWarningSeed = d.CarrProSeedWarningSeed _
            , .CarrProLength = d.CarrProLength _
            , .CarrProCheckDigitWeightFactor = d.CarrProCheckDigitWeightFactor _
            , .CarrProCheckDigitUseIndexForWeightFactor = d.CarrProCheckDigitUseIndexForWeightFactor _
            , .CarrProCheckDigitIndexForWeightFactorMin = d.CarrProCheckDigitIndexForWeightFactorMin _
            , .CarrProCheckDigitErrorCode = d.CarrProCheckDigitErrorCode _
            , .CarrProCheckDigit10Code = d.CarrProCheckDigit10Code _
            , .CarrProCheckDigitZeroCode = d.CarrProCheckDigitZeroCode _
            , .CarrProExp1 = d.CarrProExp1 _
            , .CarrProExp2 = d.CarrProExp2 _
            , .CarrProExp3 = d.CarrProExp3 _
            , .CarrProExp4 = d.CarrProExp4 _
            , .CarrProUser1 = d.CarrProUser1 _
            , .CarrProUser2 = d.CarrProUser2 _
            , .CarrProUser3 = d.CarrProUser3 _
            , .CarrProUser4 = d.CarrProUser4 _
            , .CarrProModDate = d.CarrProModDate _
            , .CarrProModUser = d.CarrProModUser _
            , .CarrProCheckDigitUseSubtractionFactor = d.CarrProCheckDigitUseSubtractionFactor _
            , .CarrProCheckDigitSubtractionFactor = d.CarrProCheckDigitSubtractionFactor _
            , .Page = page _
            , .Pages = pagecount _
            , .RecordCount = recordcount _
            , .PageSize = pagesize _
            , .CarrProUpdated = d.CarrProUpdated.ToArray()}
    End Function

    Private Sub SetCarrProNumberInactive(ByVal db As NGLMASCarrierDataContext, ByVal CarrProControl As Integer)
        Dim nObject = db.CarrierProNumbers.Where(Function(x) x.CarrProControl = CarrProControl).FirstOrDefault()
        If Not nObject Is Nothing AndAlso nObject.CarrProControl <> 0 Then
            nObject.CarrProActive = 0
            Try
                db.SubmitChanges()
            Catch ex As Exception
                'ignore errors here just log the error
                Utilities.SaveAppError(buildProcedureName("SetCarrProNumberInactive") & " for CarrProControl " & CarrProControl.ToString() & ": " & ex.Message, Me.Parameters)
            End Try
        End If
    End Sub

#End Region

End Class