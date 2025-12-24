Imports System.Data.Linq
Imports System.ServiceModel
Imports NGL.Core.ChangeTracker

Public Class NGLCarrTarMatBPData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        'Dim db As New NGLMASCarrierDataContext(ConnectionString)
        'Me.LinqTable = db.CarrierTariffMatrixBPs
        'Me.LinqDB = db
        Me.SourceClass = "NGLCarrTarMatBPData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierTariffMatrixBPs
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
        Return GetCarrTarMatBPFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetCarrTarMatBPFiltered(ByVal Control As Integer) As DataTransferObjects.CarrTarMatBP
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.CarrierTariffMatrixBP)(Function(t As LTS.CarrierTariffMatrixBP) t.CarrierTariffMatrixBPDetails)
                db.LoadOptions = oDLO

                'Get the newest record that matches the provided criteria
                Dim CarrierTariffEquipMatrixContract As DataTransferObjects.CarrTarMatBP = (
                        From d In db.CarrierTariffMatrixBPs
                        Where
                        d.CarrTarMatBPControl = Control
                        Select selectDTOData(d, db)).First

                Return CarrierTariffEquipMatrixContract

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

    Public Function GetCarrTarMatBPsFiltered(ByVal CarrTarControl As Integer,
                                             Optional ByVal page As Integer = 1,
                                             Optional ByVal pagesize As Integer = 1000) As DataTransferObjects.CarrTarMatBP()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                'db.Log = New DebugTextWriter

                Dim oRecords = (
                        From d In db.CarrierTariffMatrixBPs
                        Where
                        (d.CarrTarMatBPCarrTarControl = CarrTarControl)
                        Select d.CarrTarMatBPControl).ToArray()

                If oRecords Is Nothing Then Return Nothing

                intRecordCount = oRecords.Count
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.CarrierTariffMatrixBP)(Function(t As LTS.CarrierTariffMatrixBP) t.CarrierTariffMatrixBPDetails)
                db.LoadOptions = oDLO

                Dim CarrTarMatBPs() As DataTransferObjects.CarrTarMatBP = (
                        From d In db.CarrierTariffMatrixBPs
                        Where oRecords.Contains(d.CarrTarMatBPControl)
                        Order By d.CarrTarMatBPControl
                        Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                Return CarrTarMatBPs

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
    ''' We do not need to use page numbers because this method always returns just one record
    ''' </summary>
    ''' <param name="CarrTarMatBPControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarrTarMatBPPivot(ByVal CarrTarMatBPControl As Integer) As DataTransferObjects.CarrTarMatBPPivot
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim Pivot As DataTransferObjects.CarrTarMatBPPivot = (
                        From d In db.vCarrTarEquipMatBPPivots
                        Where d.CarrTarMatBPControl = CarrTarMatBPControl
                        Select selectDTOCarrTarMatBPPivotData(d, db)).FirstOrDefault()

                Return Pivot

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
    '''Selects the first BPPivot record with the exact parameters passed in.
    ''' </summary>
    ''' <param name="carrTarControl"></param>
    ''' <param name="rateTypeControl"></param>
    ''' <param name="classTypeControl"></param>
    ''' <param name="bracketTypeControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarrTarMatBPPivotByRef(ByVal carrTarControl As Integer,
                                              ByVal rateTypeControl As Integer,
                                              ByVal classTypeControl As Integer,
                                              ByVal bracketTypeControl As Integer) As DataTransferObjects.CarrTarMatBPPivot
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim Pivot As DataTransferObjects.CarrTarMatBPPivot = (
                        From d In db.vCarrTarEquipMatBPPivots
                        Where d.CarrTarMatBPCarrTarControl = carrTarControl _
                              And d.CarrTarMatBPClassTypeControl = classTypeControl _
                              And d.CarrTarMatBPTarRateTypeControl = rateTypeControl _
                              And d.CarrTarMatBPTarBracketTypeControl = bracketTypeControl
                        Select selectDTOCarrTarMatBPPivotData(d, db)).FirstOrDefault()

                Return Pivot

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

    'Public Function GetCarrTarMatBPPivotByRef(ByVal carrTarControl As Integer, _
    '                                          ByVal rateTypeControl As Integer, _
    '                                          ByVal classTypeControl As Integer, _
    '                                          ByVal bracketTypeControl As Integer, _
    '                                          ByVal preCloneControl As Integer) As DTO.CarrTarMatBPPivot
    '    Using db As New NGLMASCarrierDataContext(ConnectionString)
    '        Try

    '            Dim Pivot As DTO.CarrTarMatBPPivot = ( _
    '            From d In db.vCarrTarEquipMatBPPivots _
    '            Where d.CarrTarMatBPCarrTarControl = carrTarControl _
    '            And d.CarrTarMatBPClassTypeControl = classTypeControl _
    '            And d.CarrTarMatBPTarRateTypeControl = rateTypeControl _
    '            And d.CarrTarMatBPTarBracketTypeControl = bracketTypeControl _
    '            Select selectDTOCarrTarMatBPPivotData(d, db)).FirstOrDefault()

    '            Return Pivot

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

    '        Return Nothing

    '    End Using
    'End Function


    ''' <summary>
    ''' Returns the Break Point header record associated with the selected Matrix 
    ''' </summary>
    ''' <param name="BPControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 09/06/2018
    '''   new LTS carrier tariff Break Point Headers
    '''   looks up record using CarrTarMatBPDetCarrTarMatBPControl
    ''' </remarks>
    Public Function GetCarrTarEquipMatBreakPoint(ByVal BPControl As Integer) As LTS.vCarrTarEquipMatBreakPoint
        Dim oRet As LTS.vCarrTarEquipMatBreakPoint
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                oRet = db.vCarrTarEquipMatBreakPoints.Where(Function(x) x.CarrTarMatBPDetCarrTarMatBPControl = BPControl).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, getSourceCaller("GetCarrTarEquipMatBreakPoint"), db)
            End Try
        End Using
        Return oRet
    End Function


    Public Function GetCarrTarEquipMatBreakPointForContract(ByVal CarrTarControl As Integer) As LTS.vCarrTarEquipMatBreakPoint
        Dim oRet As New LTS.vCarrTarEquipMatBreakPoint()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'test if a break point exists
                If db.CarrierTariffEquipMatrixes.Any(Function(x) x.CarrTarEquipMatCarrTarControl = CarrTarControl) Then
                    'get the BPControl 
                    Dim BPControl As Integer = db.CarrierTariffEquipMatrixes.Where(Function(x) x.CarrTarEquipMatCarrTarControl = CarrTarControl).Select(Function(x) x.CarrTarEquipMatCarrTarMatBPControl).FirstOrDefault()
                    If BPControl <> 0 Then
                        oRet = db.vCarrTarEquipMatBreakPoints.Where(Function(x) x.CarrTarMatBPDetCarrTarMatBPControl = BPControl).FirstOrDefault()
                    End If
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, getSourceCaller("GetCarrTarEquipMatBreakPointForContract"), db)
            End Try
        End Using
        Return oRet
    End Function



    Public Function saveCarrTarEquipMatBreakPoint(ByVal oData As LTS.vCarrTarEquipMatBreakPoint) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                If oData.CarrTarMatBPDetCarrTarMatBPControl = 0 Then
                    throwInvalidSaveRequestException("Break Point Control", "Zero")
                End If
                Dim iBPControl = oData.CarrTarMatBPDetCarrTarMatBPControl
                Dim intBPtVals As Decimal?() = {oData.Val1, oData.Val2, oData.Val3, oData.Val4, oData.Val5, oData.Val6, oData.Val7, oData.Val8, oData.Val9, oData.Val10}
                Dim oNewBPS As New List(Of LTS.CarrierTariffMatrixBPDetail)
                Dim oExitstingBPs As New List(Of LTS.CarrierTariffMatrixBPDetail)
                Dim dtModDate = Date.Now()
                Dim sModUser = Parameters.UserName
                For i As Integer = 0 To 9
                    'i is equal to the index of the intBPtVals array values 0 to 9 are possible
                    Dim t = i + 1 ' t is equal to the id of the BP Detail Record 1 to 10 are possible
                    If db.CarrierTariffMatrixBPDetails.Any(Function(x) x.CarrTarMatBPDetCarrTarMatBPControl = iBPControl And x.CarrTarMatBPDetID = t) Then
                        'update t with values i
                        Dim oBPDet = db.CarrierTariffMatrixBPDetails.Where(Function(x) x.CarrTarMatBPDetCarrTarMatBPControl = iBPControl And x.CarrTarMatBPDetID = t).FirstOrDefault()
                        With oBPDet
                            .CarrTarMatBPDetModDate = dtModDate
                            .CarrTarMatBPDetModUser = sModUser
                            .CarrTarMatBPDetValue = If(intBPtVals(i).HasValue, intBPtVals(i), 0)
                        End With
                        oExitstingBPs.Add(oBPDet)
                    ElseIf intBPtVals(i).HasValue Then
                        'create new t with values i if i has value
                        Dim oNewBPDet = New LTS.CarrierTariffMatrixBPDetail()
                        With oNewBPDet
                            .CarrTarMatBPDetModDate = dtModDate
                            .CarrTarMatBPDetModUser = sModUser
                            .CarrTarMatBPDetValue = If(intBPtVals(i).HasValue, intBPtVals(i), 0)
                            .CarrTarMatBPDetCarrTarMatBPControl = iBPControl
                            .CarrTarMatBPDetName = "Break " + t.ToString()
                            .CarrTarMatBPDetDesc = "User Configured Break Point"
                            .CarrTarMatBPDetID = t
                        End With
                        oNewBPS.Add(oNewBPDet)
                    End If
                Next
                If Not oNewBPS Is Nothing AndAlso oNewBPS.Count() > 0 Then
                    db.CarrierTariffMatrixBPDetails.InsertAllOnSubmit(oNewBPS)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, getSourceCaller("saveCarrTarEquipMatBreakPoint"), db)
            End Try
        End Using
        Return blnRet
    End Function


    '               [CarrTarMatBPCarrTarControl] Int  Not NULL ,
    '   [CarrTarMatBPName]    NVARCHAR(50)  Not NULL,
    '   [CarrTarMatBPDesc]    NVARCHAR(100) NULL,
    '   [CarrTarMatBPClassTypeControl] Int  Not NULL CONSTRAINT [DF_CarrTarMatBPClassTypeControl]  DEFAULT (0),
    '[CarrTarMatBPTarBracketTypeControl] Int Not NULL CONSTRAINT [DF_CarrTarMatBPTarBracketTypeControl]  DEFAULT (0),
    '[CarrTarMatBPTarRateTypeControl] Int Not NULL CONSTRAINT [DF_CarrTarMatBPTarRateTypeControl] DEFAULT (0),
    '   [CarrTarMatBPModDate] DateTime       NULL,
    '   [CarrTarMatBPModUser]

    Private Function getBreakPointName(ByVal RateType As Utilities.TariffRateType, ByVal ClassType As Utilities.TariffClassType, ByVal BracketType As Utilities.BracketType) As String
        Dim strRet As String = "System Generated Break Points"
        Select Case RateType
            Case Utilities.TariffRateType.ClassRate
                Select Case ClassType
                    Case Utilities.TariffClassType.class49CFR
                        strRet = "49CFR Class Break Points"
                    Case Utilities.TariffClassType.classDOT
                        strRet = "DOT Class Break Points"
                    Case Utilities.TariffClassType.classIATA
                        strRet = "IATA Class Break Points"
                    Case Utilities.TariffClassType.classMarine
                        strRet = "Marine Class Break Points"
                    Case Utilities.TariffClassType.classNMFC
                        strRet = "NMFC Class Break Points"
                    Case Else
                        strRet = "FAK Class Break Points"
                End Select
            Case Utilities.TariffRateType.UnitOfMeasure
                Select Case BracketType
                    Case Utilities.BracketType.Cwt
                        strRet = "CWT UOM Break Points"
                    Case Utilities.BracketType.Distance
                        strRet = "Distance UOM Break Points"
                    Case Utilities.BracketType.Even
                        strRet = "Even Split UOM Break Points"
                    Case Utilities.BracketType.Lbs
                        strRet = "Weight UOM Break Points"
                    Case Utilities.BracketType.Pallets
                        strRet = "Pallet UOM Break Points"
                    Case Utilities.BracketType.Quantity
                        strRet = "Quantity/Cases UOM Break Points"
                    Case Utilities.BracketType.Volume
                        strRet = "Volumn/Cube UOM Break Points"
                    Case Else
                        strRet = "UOM Break Points"
                End Select
            Case Utilities.TariffRateType.FlatRate
                strRet = "Flat Rate Break Points"
            Case Utilities.TariffRateType.CzarLite
                strRet = "CzarLite Break Points"
            Case Utilities.TariffRateType.DistanceK
                strRet = "Kilometer Break Points"
            Case Utilities.TariffRateType.DistanceM
                strRet = "Distance Break Points"
        End Select
        Return strRet
    End Function

    Public Function createCarrTarEquipMatBreakPointForTariff(ByVal CarrTarControl As Integer, ByVal RateType As Utilities.TariffRateType, ByVal ClassType As Utilities.TariffClassType, ByVal BracketType As Utilities.BracketType, Optional CarrTarMatBPName As String = "") As Integer
        Dim iBPControl As Integer = 0
        Try
            Dim oData As New LTS.CarrierTariffMatrixBP()
            With oData
                .CarrTarMatBPCarrTarControl = CarrTarControl
                If String.IsNullOrWhiteSpace(CarrTarMatBPName) Then
                    .CarrTarMatBPName = getBreakPointName(RateType, ClassType, BracketType)
                    .CarrTarMatBPDesc = .CarrTarMatBPName
                Else
                    .CarrTarMatBPName = CarrTarMatBPName
                    .CarrTarMatBPDesc = getBreakPointName(RateType, ClassType, BracketType)
                End If
                .CarrTarMatBPClassTypeControl = ClassType
                .CarrTarMatBPTarBracketTypeControl = BracketType
                .CarrTarMatBPTarRateTypeControl = RateType
            End With

            Return insertCarrTarEquipMatBreakPoint(oData, Nothing, Nothing, Nothing)
        Catch ex As Exception
            ManageLinqDataExceptions(ex, getSourceCaller("createCarrTarEquipMatBreakPointForTariff"))
        End Try
        Return iBPControl
    End Function

    Public Function insertCarrTarEquipMatBreakPoint(ByVal oData As LTS.CarrierTariffMatrixBP, ByVal intPivotVal As Decimal?(), ByVal strBPDesc As String(), ByVal strBPName As String()) As Integer
        Dim iBPControl As Integer = 0
        If oData Is Nothing Then Return False
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim dtModDate As Date = Date.Now()
                Dim sModUser As String = Parameters.UserName
                If oData.CarrTarMatBPCarrTarControl = 0 Then
                    Dim lDetails As New List(Of String) From {"Carrier Contract Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return 0
                End If

                oData.CarrTarMatBPModDate = dtModDate
                oData.CarrTarMatBPModUser = sModUser
                db.CarrierTariffMatrixBPs.InsertOnSubmit(oData)
                db.SubmitChanges()
                iBPControl = oData.CarrTarMatBPControl
                If iBPControl = 0 Then Return False
                If intPivotVal Is Nothing OrElse intPivotVal.Count() < 1 Then
                    'create new pivot data
                    fillSystemGeneratedBreakPoints(oData.CarrTarMatBPTarRateTypeControl, intPivotVal, strBPDesc, strBPName)
                End If
                Dim oNewBPS As New List(Of LTS.CarrierTariffMatrixBPDetail)
                Dim oExitstingBPs As New List(Of LTS.CarrierTariffMatrixBPDetail)

                For i As Integer = 0 To 9
                    'i is equal to the index of the intBPtVals array values 0 to 9 are possible
                    Dim t = i + 1 ' t is equal to the id of the BP Detail Record 1 to 10 are possible
                    If i > 0 AndAlso (oData.CarrTarMatBPTarRateTypeControl <> Utilities.TariffRateType.ClassRate Or oData.CarrTarMatBPTarRateTypeControl <> Utilities.TariffRateType.UnitOfMeasure) Then
                        Exit For
                    End If
                    If db.CarrierTariffMatrixBPDetails.Any(Function(x) x.CarrTarMatBPDetCarrTarMatBPControl = iBPControl And x.CarrTarMatBPDetID = t) Then
                        'update t with values i
                        Dim oBPDet = db.CarrierTariffMatrixBPDetails.Where(Function(x) x.CarrTarMatBPDetCarrTarMatBPControl = iBPControl And x.CarrTarMatBPDetID = t).FirstOrDefault()
                        With oBPDet
                            .CarrTarMatBPDetModDate = dtModDate
                            .CarrTarMatBPDetModUser = sModUser
                            .CarrTarMatBPDetValue = If(intPivotVal(i).HasValue, intPivotVal(i), 0)
                            .CarrTarMatBPDetName = strBPName(i)
                            .CarrTarMatBPDetDesc = strBPDesc(i)
                        End With
                        oExitstingBPs.Add(oBPDet)
                    Else
                        'create new t with values i 
                        Dim oNewBPDet = New LTS.CarrierTariffMatrixBPDetail()
                        With oNewBPDet
                            .CarrTarMatBPDetModDate = dtModDate
                            .CarrTarMatBPDetModUser = sModUser
                            .CarrTarMatBPDetValue = If(intPivotVal(i).HasValue, intPivotVal(i), 0)
                            .CarrTarMatBPDetCarrTarMatBPControl = iBPControl
                            .CarrTarMatBPDetName = strBPName(i)
                            .CarrTarMatBPDetDesc = strBPDesc(i)
                            .CarrTarMatBPDetID = t
                        End With
                        oNewBPS.Add(oNewBPDet)
                    End If
                Next
                If Not oNewBPS Is Nothing AndAlso oNewBPS.Count() > 0 Then
                    db.CarrierTariffMatrixBPDetails.InsertAllOnSubmit(oNewBPS)
                End If
                db.SubmitChanges()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, getSourceCaller("insertCarrTarEquipMatBreakPoint"), db)
            End Try
        End Using
        Return iBPControl
    End Function

    Private Sub fillSystemGeneratedBreakPoints(ByVal CarrTarMatBPTarRateTypeControl As Integer, ByRef intPivotVal As Decimal?(), ByRef strBPDesc As String(), ByRef strBPName As String())
        ReDim intPivotVal(10)
        ReDim strBPDesc(10)
        ReDim strBPName(10)
        Select Case CarrTarMatBPTarRateTypeControl
            Case Utilities.TariffRateType.ClassRate
                For i As Integer = 0 To 9
                    Dim t = i + 1

                    strBPDesc(i) = "System Generated LTL Break Point"
                    strBPName(i) = "Break " & t.ToString()
                    Select Case t
                        Case 1
                            intPivotVal(i) = 1000 * t
                        Case 2
                            intPivotVal(i) = 1000 * t
                        Case 3
                            intPivotVal(i) = 5000 * t
                        Case 4
                            intPivotVal(i) = 10000 * t
                        Case 5
                            intPivotVal(i) = 15000 * t
                        Case 6
                            intPivotVal(i) = 18000 * t
                        Case 7
                            intPivotVal(i) = 20000 * t
                        Case Else
                            intPivotVal(i) = 0
                    End Select
                Next

            Case Utilities.TariffRateType.UnitOfMeasure
                For i As Integer = 0 To 9
                    Dim t = i + 1
                    strBPDesc(i) = "System Generated UOM Break Point"
                    strBPName(i) = "Break " & t.ToString()
                    intPivotVal(i) = t
                Next

            Case Else
                strBPDesc(0) = "System Generated Break Point"
                strBPName(0) = "Break 1"
                intPivotVal(0) = 1
        End Select
    End Sub


    Public Function SaveCarrTarMatBPPivot(ByVal Pivot As DataTransferObjects.CarrTarMatBPPivot) As DataTransferObjects.CarrTarMatBPPivot
        Dim intCarrTarMatBPControl As Integer = 0
        If Pivot Is Nothing Then Return Nothing
        Using Me.LinqDB
            Try
                'get a copy of the data
                Dim oData As DataTransferObjects.CarrTarMatBP = GetCarrTarMatBPFiltered(Pivot.CarrTarMatBPControl)
                With oData
                    .CarrTarMatBPCarrTarControl = Pivot.CarrTarMatBPCarrTarControl
                    .CarrTarMatBPClassTypeControl = Pivot.CarrTarMatBPClassTypeControl
                    .CarrTarMatBPDesc = Pivot.CarrTarMatBPDesc
                    .CarrTarMatBPName = Pivot.CarrTarMatBPName
                    .CarrTarMatBPTarBracketTypeControl = Pivot.CarrTarMatBPTarBracketTypeControl
                    .CarrTarMatBPTarRateTypeControl = Pivot.CarrTarMatBPTarRateTypeControl
                    .CarrTarMatBPModDate = Pivot.CarrTarMatBPModDate
                    .CarrTarMatBPModUser = Pivot.CarrTarMatBPModUser
                    .CarrTarMatBPUpdated = Pivot.CarrTarMatBPUpdated
                    .TrackingState = TrackingInfo.Updated
                End With
                '  now update the details
                If Not oData.CarrierTariffMatrixBPDetails Is Nothing AndAlso oData.CarrierTariffMatrixBPDetails.Count > 0 Then
                    Dim intPivotVal As Decimal?() = {Pivot.BPVal1, Pivot.BPVal2, Pivot.BPVal3, Pivot.BPVal4, Pivot.BPVal5, Pivot.BPVal6, Pivot.BPVal7, Pivot.BPVal8, Pivot.BPVal9, Pivot.BPVal10}
                    Dim strBPDesc As String() = {Pivot.BPDesc1, Pivot.BPDesc2, Pivot.BPDesc3, Pivot.BPDesc4, Pivot.BPDesc5, Pivot.BPDesc6, Pivot.BPDesc7, Pivot.BPDesc8, Pivot.BPDesc9, Pivot.BPDesc10}
                    Dim strBPName As String() = {Pivot.BPName1, Pivot.BPName2, Pivot.BPName3, Pivot.BPName4, Pivot.BPName5, Pivot.BPName6, Pivot.BPName7, Pivot.BPName8, Pivot.BPName9, Pivot.BPName10}

                    For Each d In oData.CarrierTariffMatrixBPDetails
                        Dim intIndex As Integer = d.CarrTarMatBPDetID - 1
                        If intIndex > 9 Then Exit For
                        If d.CarrTarMatBPDetValue <> intPivotVal(intIndex) Or d.CarrTarMatBPDetDesc <> strBPDesc(intIndex) Or d.CarrTarMatBPDetName <> strBPName(intIndex) Then
                            d.CarrTarMatBPDetValue = intPivotVal(intIndex)
                            d.CarrTarMatBPDetDesc = strBPDesc(intIndex)
                            d.CarrTarMatBPDetName = strBPName(intIndex)
                            d.CarrTarMatBPDetModDate = If(Pivot.CarrTarMatBPModDate, Date.Now())
                            d.CarrTarMatBPDetModUser = Pivot.CarrTarMatBPModUser
                            d.TrackingState = TrackingInfo.Updated
                        End If
                    Next
                    UpdateRecordWithDetails(oData)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CreateCarrTarMatBPPivot"))
            End Try
        End Using
        Return GetCarrTarMatBPPivot(Pivot.CarrTarMatBPControl)
    End Function

    Public Function CreateCarrTarMatBPPivot(ByVal pivot As DataTransferObjects.CarrTarMatBPPivot) As DataTransferObjects.CarrTarMatBPPivot
        Dim intCarrTarMatBPControl As Integer = 0
        If pivot Is Nothing Then Return Nothing
        Using Me.LinqDB
            Try
                Dim oData As New DataTransferObjects.CarrTarMatBP With
                        {
                        .CarrTarMatBPCarrTarControl = pivot.CarrTarMatBPCarrTarControl,
                        .CarrTarMatBPClassTypeControl = pivot.CarrTarMatBPClassTypeControl,
                        .CarrTarMatBPDesc = pivot.CarrTarMatBPDesc,
                        .CarrTarMatBPName = pivot.CarrTarMatBPName,
                        .CarrTarMatBPTarBracketTypeControl = pivot.CarrTarMatBPTarBracketTypeControl,
                        .CarrTarMatBPTarRateTypeControl = pivot.CarrTarMatBPTarRateTypeControl,
                        .CarrTarMatBPModDate = If(pivot.CarrTarMatBPModDate, Date.Now()),
                        .CarrTarMatBPModUser = pivot.CarrTarMatBPModUser,
                        .TrackingState = TrackingInfo.Created
                        }
                Dim oNewData As DataTransferObjects.CarrTarMatBP = Me.Add(oData, LinqTable)
                'insert the details
                If Not oNewData Is Nothing AndAlso oNewData.CarrTarMatBPControl <> 0 Then
                    intCarrTarMatBPControl = oNewData.CarrTarMatBPControl
                    Dim oDetData As NGLCarrTarMatBPDetData = Me.NDPBaseClassFactory("NGLCarrTarMatBPDetData", False)
                    Dim intPivotVal As Decimal?() = {pivot.BPVal1, pivot.BPVal2, pivot.BPVal3, pivot.BPVal4, pivot.BPVal5, pivot.BPVal6, pivot.BPVal7, pivot.BPVal8, pivot.BPVal9, pivot.BPVal10}
                    Dim strBPDesc As String() = {pivot.BPDesc1, pivot.BPDesc2, pivot.BPDesc3, pivot.BPDesc4, pivot.BPDesc5, pivot.BPDesc6, pivot.BPDesc7, pivot.BPDesc8, pivot.BPDesc9, pivot.BPDesc10}
                    Dim strBPName As String() = {pivot.BPName1, pivot.BPName2, pivot.BPName3, pivot.BPName4, pivot.BPName5, pivot.BPName6, pivot.BPName7, pivot.BPName8, pivot.BPName9, pivot.BPName10}
                    For i As Integer = 0 To 9
                        If intPivotVal(i).HasValue Then
                            Dim oDet As New DataTransferObjects.CarrTarMatBPDet
                            With oDet
                                .CarrTarMatBPDetCarrTarMatBPControl = intCarrTarMatBPControl
                                .CarrTarMatBPDetDesc = strBPDesc(i)
                                .CarrTarMatBPDetName = strBPName(i)
                                .CarrTarMatBPDetID = (i + 1)
                                .CarrTarMatBPDetValue = intPivotVal(i)
                                .TrackingState = TrackingInfo.Created
                                .CarrTarMatBPDetModUser = pivot.CarrTarMatBPModUser
                                .CarrTarMatBPDetModDate = If(pivot.CarrTarMatBPModDate, Date.Now())
                            End With
                            oDetData.CreateRecord(oDet)
                        End If
                    Next
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CreateCarrTarMatBPPivot"))
            End Try
        End Using
        Return GetCarrTarMatBPPivot(intCarrTarMatBPControl)
    End Function

    Public Function GetOrCreateCarrTarMatBPPivot(ByVal CarrTarMatBPCarrTarControl As Integer,
                                                 ByVal CarrTarEquipMatCarrTarEquipControl As Integer,
                                                 ByVal CarrTarMatBPClassTypeControl As Integer,
                                                 ByVal CarrTarMatBPTarBracketTypeControl As Integer,
                                                 ByVal CarrTarMatBPTarRateTypeControl As Integer,
                                                 Optional ByVal CarrTarMatBPDesc As String = "NA") As DataTransferObjects.CarrTarMatBPPivot
        'set up default values
        Dim oRet As DataTransferObjects.CarrTarMatBPPivot

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim eData = From d In db.vCarrTarEquipMatBPPivots
                        Where
                        d.CarrTarMatBPCarrTarControl = CarrTarMatBPCarrTarControl _
                        And
                        d.CarrTarMatBPName = CarrTarEquipMatCarrTarEquipControl.ToString _
                        And
                        d.CarrTarMatBPClassTypeControl = CarrTarMatBPClassTypeControl _
                        And
                        d.CarrTarMatBPTarBracketTypeControl = CarrTarMatBPTarBracketTypeControl _
                        And
                        d.CarrTarMatBPTarRateTypeControl = CarrTarMatBPTarRateTypeControl
                If Not eData Is Nothing Then
                    'we have a match so get the pivot data
                    oRet = (From d In eData Select selectDTOCarrTarMatBPPivotData(d, db)).FirstOrDefault()
                End If
                If oRet Is Nothing OrElse oRet.CarrTarMatBPControl = 0 Then
                    'Create a new pivot record with defaults
                    Dim pivot As New DataTransferObjects.CarrTarMatBPPivot With {
                            .CarrTarMatBPCarrTarControl = CarrTarMatBPCarrTarControl,
                            .CarrTarMatBPName = CarrTarEquipMatCarrTarEquipControl.ToString,
                            .CarrTarMatBPClassTypeControl = CarrTarMatBPClassTypeControl,
                            .CarrTarMatBPTarBracketTypeControl = CarrTarMatBPTarBracketTypeControl,
                            .CarrTarMatBPTarRateTypeControl = CarrTarMatBPTarRateTypeControl,
                            .CarrTarMatBPDesc = CarrTarMatBPDesc,
                            .TrackingState = TrackingInfo.Created}
                    Dim intPivotVal(10) As Decimal?
                    Dim strBPDesc(10) As String
                    Dim strBPName(10) As String
                    fillSystemGeneratedBreakPoints(CarrTarMatBPTarRateTypeControl, intPivotVal, strBPDesc, strBPName)
                    With pivot
                        .BPDesc1 = strBPDesc(0)
                        .BPName1 = strBPName(0)
                        .BPVal1 = intPivotVal(0)
                        If CarrTarMatBPTarRateTypeControl = Utilities.TariffRateType.ClassRate Or CarrTarMatBPTarRateTypeControl = Utilities.TariffRateType.UnitOfMeasure Then
                            .BPDesc2 = strBPDesc(1)
                            .BPName2 = strBPName(1)
                            .BPVal2 = intPivotVal(1)
                            .BPDesc3 = strBPDesc(2)
                            .BPName3 = strBPName(2)
                            .BPVal3 = intPivotVal(2)
                            .BPDesc4 = strBPDesc(3)
                            .BPName4 = strBPName(3)
                            .BPVal4 = intPivotVal(3)
                            .BPDesc5 = strBPDesc(4)
                            .BPName5 = strBPName(4)
                            .BPVal5 = intPivotVal(4)
                            .BPDesc6 = strBPDesc(5)
                            .BPName6 = strBPName(5)
                            .BPVal6 = intPivotVal(5)
                            .BPDesc7 = strBPDesc(6)
                            .BPName7 = strBPName(6)
                            .BPVal7 = intPivotVal(6)
                            .BPDesc8 = strBPDesc(7)
                            .BPName8 = strBPName(7)
                            .BPVal8 = intPivotVal(7)
                            .BPDesc9 = strBPDesc(8)
                            .BPName9 = strBPName(8)
                            .BPVal9 = intPivotVal(8)
                            .BPDesc10 = strBPDesc(9)
                            .BPName10 = strBPName(9)
                            .BPVal10 = intPivotVal(9)
                        End If
                    End With
                    oRet = CreateCarrTarMatBPPivot(pivot)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetOrCreateCarrTarMatBPPivot"))
            End Try
            Return oRet
        End Using
        Return oRet
    End Function

    Public Function GetOrCreateCarrTarMatBPPivot(ByVal pivot As DataTransferObjects.CarrTarMatBPPivot) As DataTransferObjects.CarrTarMatBPPivot
        'set up default values
        Dim oRet As DataTransferObjects.CarrTarMatBPPivot
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim eData = From d In db.vCarrTarEquipMatBPPivots
                        Where
                        d.CarrTarMatBPCarrTarControl = pivot.CarrTarMatBPCarrTarControl _
                        And
                        d.CarrTarMatBPName = pivot.CarrTarMatBPName _
                        And
                        d.CarrTarMatBPClassTypeControl = pivot.CarrTarMatBPClassTypeControl _
                        And
                        d.CarrTarMatBPTarBracketTypeControl = pivot.CarrTarMatBPTarBracketTypeControl _
                        And
                        d.CarrTarMatBPTarRateTypeControl = pivot.CarrTarMatBPTarRateTypeControl
                If Not eData Is Nothing Then
                    'we have a match so get the pivot data
                    oRet = (From d In eData Select selectDTOCarrTarMatBPPivotData(d, db)).FirstOrDefault()
                End If
                If oRet Is Nothing OrElse oRet.CarrTarMatBPControl = 0 Then
                    oRet = CreateCarrTarMatBPPivot(pivot)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetOrCreateCarrTarMatBPPivot"))
            End Try
            Return oRet
        End Using
        Return oRet
    End Function


#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrTarMatBP)
        'Create New Record
        Return New LTS.CarrierTariffMatrixBP With {.CarrTarMatBPControl = d.CarrTarMatBPControl,
            .CarrTarMatBPCarrTarControl = d.CarrTarMatBPCarrTarControl,
            .CarrTarMatBPName = d.CarrTarMatBPName,
            .CarrTarMatBPDesc = d.CarrTarMatBPDesc,
            .CarrTarMatBPClassTypeControl = d.CarrTarMatBPClassTypeControl,
            .CarrTarMatBPTarBracketTypeControl = d.CarrTarMatBPTarBracketTypeControl,
            .CarrTarMatBPTarRateTypeControl = d.CarrTarMatBPTarRateTypeControl,
            .CarrTarMatBPModUser = d.CarrTarMatBPModUser,
            .CarrTarMatBPModDate = d.CarrTarMatBPModDate,
            .CarrTarMatBPUpdated = If(d.CarrTarMatBPUpdated Is Nothing, New Byte() {}, d.CarrTarMatBPUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrTarMatBPFiltered(Control:=CType(LinqTable, LTS.CarrierTariffMatrixBP).CarrTarMatBPControl)
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
    '                Dim varTariff = (From d In db.CarrierTariffEquipMatrixes Where d.CarrTarID = strNewTariffID Select d).First
    '                If Not varTariff Is Nothing Then
    '                    If Not AllowOverwrite Then Return False
    '                    CarrTarControl = varTariff.CarrTarControl
    '                    CarrTarID = strNewTariffID
    '                    ''Delete all of the matrix details they no longer match.
    '                    'executeSQL("DELETE FROM [dbo].[CarrierTariffEquipMatrixMatrix] Where CarrTarMatCarrTarControl = " & CarrTarControl)
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
                Dim source As LTS.CarrierTariffMatrixBP = TryCast(LinqTable, LTS.CarrierTariffMatrixBP)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierTariffMatrixBPs
                    Where d.CarrTarMatBPControl = source.CarrTarMatBPControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrTarMatBPControl _
                        , .ModDate = d.CarrTarMatBPModDate _
                        , .ModUser = d.CarrTarMatBPModUser _
                        , .Updated = d.CarrTarMatBPUpdated.ToArray}).First

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
        ''Check if the data already exists only one allowed
        'With CType(oData, DTO.CarrierTariffEquipMatrix)
        '    Try
        '        Dim CarrierTariffEquipMatrix As DTO.CarrierTariffEquipMatrix = ( _
        '            From t In CType(oDB, NGLMASCarrierDataContext).CarrierTariffEquipMatrixes _
        '             Where _
        '                 (t.CarrierTariffEquipMatrixCarrierControl = .CarrierTariffEquipMatrixCarrierControl _
        '                 And _
        '                 t.CarrierTariffEquipMatrixXaction = .CarrierTariffEquipMatrixXaction) _
        '             Select New DTO.CarrierTariffEquipMatrix With {.CarrierTariffEquipMatrixControl = t.CarrierTariffEquipMatrixControl}).First

        '        If Not CarrierTariffEquipMatrix Is Nothing Then
        '            Utilities.SaveAppError("Cannot save new Carrier EDI data.  The Carrier EDI XAction, " & .CarrierTariffEquipMatrixXaction & ",  is already assigned to the selected carrier.", Me.Parameters)
        '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
        '        End If

        '    Catch ex As FaultException
        '        Throw
        '    Catch ex As InvalidOperationException
        '        'do nothing this is the desired result.
        '    End Try
        'End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        ''Check if the data already exists only one allowed
        'With CType(oData, DTO.CarrierTariffEquipMatrix)
        '    Try
        '        'Get the newest record that matches the provided criteria
        '        Dim CarrierTariffEquipMatrix As DTO.CarrierTariffEquipMatrix = ( _
        '        From t In CType(oDB, NGLMASCarrierDataContext).CarrierTariffEquipMatrixes _
        '         Where _
        '             (t.CarrierTariffEquipMatrixControl <> .CarrierTariffEquipMatrixControl) _
        '             And _
        '             (t.CarrierTariffEquipMatrixCarrierControl = .CarrierTariffEquipMatrixCarrierControl _
        '             And _
        '             t.CarrierTariffEquipMatrixXaction = .CarrierTariffEquipMatrixXaction) _
        '         Select New DTO.CarrierTariffEquipMatrix With {.CarrierTariffEquipMatrixControl = t.CarrierTariffEquipMatrixControl}).First

        '        If Not CarrierTariffEquipMatrix Is Nothing Then
        '            Utilities.SaveAppError("Cannot save Carrier EDI changes.  The Carrier EDI XAction, " & .CarrierTariffEquipMatrixXaction & ",  is already assigned to the selected carrier.", Me.Parameters)
        '            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
        '        End If

        '    Catch ex As FaultException
        '        Throw
        '    Catch ex As InvalidOperationException
        '        'do nothing this is the desired result.
        '    End Try
        'End With
    End Sub

    Friend Function selectDTOData(ByVal d As LTS.CarrierTariffMatrixBP, ByRef db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrTarMatBP
        Dim oDetail As NGLCarrTarEquipMatDetData = Me.NDPBaseClassFactory("NGLCarrTarEquipMatDetData", False)
        Return New DataTransferObjects.CarrTarMatBP With {.CarrTarMatBPControl = d.CarrTarMatBPControl,
            .CarrTarMatBPCarrTarControl = d.CarrTarMatBPCarrTarControl,
            .CarrTarMatBPName = d.CarrTarMatBPName,
            .CarrTarMatBPDesc = d.CarrTarMatBPDesc,
            .CarrTarMatBPClassTypeControl = d.CarrTarMatBPClassTypeControl,
            .CarrTarMatBPTarBracketTypeControl = d.CarrTarMatBPTarBracketTypeControl,
            .CarrTarMatBPTarRateTypeControl = d.CarrTarMatBPTarRateTypeControl,
            .CarrTarMatBPModUser = d.CarrTarMatBPModUser,
            .CarrTarMatBPModDate = d.CarrTarMatBPModDate,
            .CarrTarMatBPUpdated = d.CarrTarMatBPUpdated.ToArray(),
            .CarrierTariffMatrixBPDetails = (
                From c In d.CarrierTariffMatrixBPDetails
                    Select New DataTransferObjects.CarrTarMatBPDet _
                        With {.CarrTarMatBPDetControl = c.CarrTarMatBPDetControl _
                            , .CarrTarMatBPDetCarrTarMatBPControl = c.CarrTarMatBPDetCarrTarMatBPControl _
                            , .CarrTarMatBPDetName = c.CarrTarMatBPDetName _
                            , .CarrTarMatBPDetDesc = c.CarrTarMatBPDetDesc _
                            , .CarrTarMatBPDetID = c.CarrTarMatBPDetID _
                            , .CarrTarMatBPDetValue = c.CarrTarMatBPDetValue _
                            , .CarrTarMatBPDetModDate = c.CarrTarMatBPDetModDate _
                            , .CarrTarMatBPDetModUser = c.CarrTarMatBPDetModUser _
                            , .CarrTarMatBPDetUpdated = c.CarrTarMatBPDetUpdated.ToArray()}).ToList(),
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize}
    End Function

    Friend Function selectDTOCarrTarMatBPPivotData(ByVal d As LTS.vCarrTarEquipMatBPPivot, ByRef db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrTarMatBPPivot
        Return New DataTransferObjects.CarrTarMatBPPivot With {.CarrTarMatBPControl = d.CarrTarMatBPControl,
            .CarrTarMatBPCarrTarControl = d.CarrTarMatBPCarrTarControl,
            .CarrTarMatBPName = d.CarrTarMatBPName,
            .CarrTarMatBPDesc = d.CarrTarMatBPDesc,
            .CarrTarMatBPClassTypeControl = d.CarrTarMatBPClassTypeControl,
            .CarrTarMatBPTarBracketTypeControl = d.CarrTarMatBPTarBracketTypeControl,
            .CarrTarMatBPTarRateTypeControl = d.CarrTarMatBPTarRateTypeControl,
            .BPVal1 = d.BPVal1,
            .BPVal2 = d.BPVal2,
            .BPVal3 = d.BPVal3,
            .BPVal4 = d.BPVal4,
            .BPVal5 = d.BPVal5,
            .BPVal6 = d.BPVal6,
            .BPVal7 = d.BPVal7,
            .BPVal8 = d.BPVal8,
            .BPVal9 = d.BPVal9,
            .BPVal10 = d.BPVal10,
            .BPName1 = d.BPName1,
            .BPName2 = d.BPName2,
            .BPName3 = d.BPName3,
            .BPName4 = d.BPName4,
            .BPName5 = d.BPName5,
            .BPName6 = d.BPName6,
            .BPName7 = d.BPName7,
            .BPName8 = d.BPName8,
            .BPName9 = d.BPName9,
            .BPName10 = d.BPName10,
            .BPDesc1 = d.BPDesc1,
            .BPDesc2 = d.BPDesc2,
            .BPDesc3 = d.BPDesc3,
            .BPDesc4 = d.BPDesc4,
            .BPDesc5 = d.BPDesc5,
            .BPDesc6 = d.BPDesc6,
            .BPDesc7 = d.BPDesc7,
            .BPDesc8 = d.BPDesc8,
            .BPDesc9 = d.BPDesc9,
            .BPDesc10 = d.BPDesc10,
            .CarrTarMatBPModUser = d.CarrTarMatBPModUser,
            .CarrTarMatBPModDate = d.CarrTarMatBPModDate,
            .CarrTarMatBPUpdated = d.CarrTarMatBPUpdated.ToArray(),
            .Page = page,
            .Pages = pagecount,
            .RecordCount = recordcount,
            .PageSize = pagesize}
    End Function

    Protected Overrides Sub AddDetailsToLinq(ByRef LinqTable As Object, ByRef oData As DataTransferObjects.DTOBaseClass)

        With CType(LinqTable, LTS.CarrierTariffMatrixBP)
            'Add Detail Records
            .CarrierTariffMatrixBPDetails.AddRange(
                From d In CType(oData, DataTransferObjects.CarrTarMatBP).CarrierTariffMatrixBPDetails
                                                      Select New LTS.CarrierTariffMatrixBPDetail With {.CarrTarMatBPDetControl = d.CarrTarMatBPDetControl _
                                                      , .CarrTarMatBPDetCarrTarMatBPControl = d.CarrTarMatBPDetCarrTarMatBPControl _
                                                      , .CarrTarMatBPDetName = d.CarrTarMatBPDetName _
                                                      , .CarrTarMatBPDetDesc = d.CarrTarMatBPDetDesc _
                                                      , .CarrTarMatBPDetID = d.CarrTarMatBPDetID _
                                                      , .CarrTarMatBPDetValue = d.CarrTarMatBPDetValue _
                                                      , .CarrTarMatBPDetModDate = d.CarrTarMatBPDetModDate _
                                                      , .CarrTarMatBPDetModUser = d.CarrTarMatBPDetModUser _
                                                      , .CarrTarMatBPDetUpdated = If(d.CarrTarMatBPDetUpdated Is Nothing, New Byte() {}, d.CarrTarMatBPDetUpdated)})

        End With
    End Sub

    Protected Overrides Sub InsertAllDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef LinqTable As Object)
        With CType(oDB, NGLMASCarrierDataContext)
            .CarrierTariffMatrixBPDetails.InsertAllOnSubmit(CType(LinqTable, LTS.CarrierTariffMatrixBP).CarrierTariffMatrixBPDetails)
        End With
    End Sub

    Protected Overrides Sub ProcessUpdatedDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        With CType(oDB, NGLMASCarrierDataContext)
            ' Process any inserted detail records  
            .CarrierTariffMatrixBPDetails.InsertAllOnSubmit(GetCarrTarMatBPDetChanges(oData, TrackingInfo.Created))
            ' Process any updated bookload records
            .CarrierTariffMatrixBPDetails.AttachAll(GetCarrTarMatBPDetChanges(oData, TrackingInfo.Updated), True)
        End With
    End Sub

    Protected Function GetCarrTarMatBPDetChanges(ByVal source As DataTransferObjects.CarrTarMatBP, ByVal changeType As TrackingInfo) As List(Of LTS.CarrierTariffMatrixBPDetail)
        ' Test record details for specified change type.
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim details As IEnumerable(Of LTS.CarrierTariffMatrixBPDetail) = (
                From d In source.CarrierTariffMatrixBPDetails
                Where d.TrackingState = changeType
                Select New LTS.CarrierTariffMatrixBPDetail With {.CarrTarMatBPDetControl = d.CarrTarMatBPDetControl _
                , .CarrTarMatBPDetCarrTarMatBPControl = d.CarrTarMatBPDetCarrTarMatBPControl _
                , .CarrTarMatBPDetName = d.CarrTarMatBPDetName _
                , .CarrTarMatBPDetDesc = d.CarrTarMatBPDetDesc _
                , .CarrTarMatBPDetID = d.CarrTarMatBPDetID _
                , .CarrTarMatBPDetValue = d.CarrTarMatBPDetValue _
                , .CarrTarMatBPDetModDate = d.CarrTarMatBPDetModDate _
                , .CarrTarMatBPDetModUser = d.CarrTarMatBPDetModUser _
                , .CarrTarMatBPDetUpdated = If(d.CarrTarMatBPDetUpdated Is Nothing, New Byte() {}, d.CarrTarMatBPDetUpdated)})
        Return details.ToList()
    End Function


#End Region

End Class