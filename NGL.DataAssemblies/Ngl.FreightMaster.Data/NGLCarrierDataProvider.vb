
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.ServiceModel
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
'this is a test rob's Notes

Public Class NGLCarrierBudgetData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierBudgets
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierBudgetData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierBudgets
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
        Return GetCarrierBudgetFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrierBudgetsFiltered()
    End Function

    Public Function GetCarrierBudgetFiltered(ByVal Control As Integer) As DTO.CarrierBudget
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim CarrierBudget As DTO.CarrierBudget = (
                From d In db.CarrierBudgets
                Where
                    d.CarrierBudControl = Control
                Select New DTO.CarrierBudget With {.CarrierBudControl = d.CarrierBudControl,
                                            .CarrierBudCarrierControl = d.CarrierBudCarrierControl,
                                            .CarrierBudModDate = d.CarrierBudModDate,
                                            .CarrierBudModUser = d.CarrierBudModUser,
                                            .CarrierBudExpMo1 = If(d.CarrierBudExpMo1.HasValue, d.CarrierBudExpMo1.Value, 0),
                                            .CarrierBudExpMo2 = If(d.CarrierBudExpMo2.HasValue, d.CarrierBudExpMo2.Value, 0),
                                            .CarrierBudExpMo3 = If(d.CarrierBudExpMo3.HasValue, d.CarrierBudExpMo3.Value, 0),
                                            .CarrierBudExpMo4 = If(d.CarrierBudExpMo4.HasValue, d.CarrierBudExpMo4.Value, 0),
                                            .CarrierBudExpMo5 = If(d.CarrierBudExpMo5.HasValue, d.CarrierBudExpMo5.Value, 0),
                                            .CarrierBudExpMo6 = If(d.CarrierBudExpMo6.HasValue, d.CarrierBudExpMo6.Value, 0),
                                            .CarrierBudExpMo7 = If(d.CarrierBudExpMo7.HasValue, d.CarrierBudExpMo7.Value, 0),
                                            .CarrierBudExpMo8 = If(d.CarrierBudExpMo8.HasValue, d.CarrierBudExpMo8.Value, 0),
                                            .CarrierBudExpMo9 = If(d.CarrierBudExpMo9.HasValue, d.CarrierBudExpMo9.Value, 0),
                                            .CarrierBudExpMo10 = If(d.CarrierBudExpMo10.HasValue, d.CarrierBudExpMo10.Value, 0),
                                            .CarrierBudExpMo11 = If(d.CarrierBudExpMo11.HasValue, d.CarrierBudExpMo11.Value, 0),
                                            .CarrierBudExpMo12 = If(d.CarrierBudExpMo12.HasValue, d.CarrierBudExpMo12.Value, 0),
                                            .CarrierBudExpTotal = If(d.CarrierBudExpTotal.HasValue, d.CarrierBudExpTotal.Value, 0),
                                            .CarrierBudActMo1 = If(d.CarrierBudActMo1.HasValue, d.CarrierBudActMo1.Value, 0),
                                            .CarrierBudActMo2 = If(d.CarrierBudActMo2.HasValue, d.CarrierBudActMo2.Value, 0),
                                            .CarrierBudActMo3 = If(d.CarrierBudActMo3.HasValue, d.CarrierBudActMo3.Value, 0),
                                            .CarrierBudActMo4 = If(d.CarrierBudActMo4.HasValue, d.CarrierBudActMo4.Value, 0),
                                            .CarrierBudActMo5 = If(d.CarrierBudActMo5.HasValue, d.CarrierBudActMo5.Value, 0),
                                            .CarrierBudActMo6 = If(d.CarrierBudActMo6.HasValue, d.CarrierBudActMo6.Value, 0),
                                            .CarrierBudActMo7 = If(d.CarrierBudActMo7.HasValue, d.CarrierBudActMo7.Value, 0),
                                            .CarrierBudActMo8 = If(d.CarrierBudActMo8.HasValue, d.CarrierBudActMo8.Value, 0),
                                            .CarrierBudActMo9 = If(d.CarrierBudActMo9.HasValue, d.CarrierBudActMo9.Value, 0),
                                            .CarrierBudActMo10 = If(d.CarrierBudActMo10.HasValue, d.CarrierBudActMo10.Value, 0),
                                            .CarrierBudActMo11 = If(d.CarrierBudActMo11.HasValue, d.CarrierBudActMo11.Value, 0),
                                            .CarrierBudActMo12 = If(d.CarrierBudActMo12.HasValue, d.CarrierBudActMo12.Value, 0),
                                            .CarrierBudActTotal = If(d.CarrierBudActTotal.HasValue, d.CarrierBudActTotal.Value, 0),
                                            .CarrierBudgetUpdated = d.CarrierBudgetUpdated.ToArray()}).First


                Return CarrierBudget

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

    Public Function GetCarrierBudgetsFiltered(Optional ByVal CarrierControl As Integer = 0) As DTO.CarrierBudget()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CarrierBudgets() As DTO.CarrierBudget = (
                From d In db.CarrierBudgets
                Where
                    (d.CarrierBudCarrierControl = If(CarrierControl = 0, d.CarrierBudCarrierControl, CarrierControl))
                Order By d.CarrierBudControl
                Select New DTO.CarrierBudget With {.CarrierBudControl = d.CarrierBudControl,
                                            .CarrierBudCarrierControl = d.CarrierBudCarrierControl,
                                            .CarrierBudModDate = d.CarrierBudModDate,
                                            .CarrierBudModUser = d.CarrierBudModUser,
                                            .CarrierBudExpMo1 = If(d.CarrierBudExpMo1.HasValue, d.CarrierBudExpMo1.Value, 0),
                                            .CarrierBudExpMo2 = If(d.CarrierBudExpMo2.HasValue, d.CarrierBudExpMo2.Value, 0),
                                            .CarrierBudExpMo3 = If(d.CarrierBudExpMo3.HasValue, d.CarrierBudExpMo3.Value, 0),
                                            .CarrierBudExpMo4 = If(d.CarrierBudExpMo4.HasValue, d.CarrierBudExpMo4.Value, 0),
                                            .CarrierBudExpMo5 = If(d.CarrierBudExpMo5.HasValue, d.CarrierBudExpMo5.Value, 0),
                                            .CarrierBudExpMo6 = If(d.CarrierBudExpMo6.HasValue, d.CarrierBudExpMo6.Value, 0),
                                            .CarrierBudExpMo7 = If(d.CarrierBudExpMo7.HasValue, d.CarrierBudExpMo7.Value, 0),
                                            .CarrierBudExpMo8 = If(d.CarrierBudExpMo8.HasValue, d.CarrierBudExpMo8.Value, 0),
                                            .CarrierBudExpMo9 = If(d.CarrierBudExpMo9.HasValue, d.CarrierBudExpMo9.Value, 0),
                                            .CarrierBudExpMo10 = If(d.CarrierBudExpMo10.HasValue, d.CarrierBudExpMo10.Value, 0),
                                            .CarrierBudExpMo11 = If(d.CarrierBudExpMo11.HasValue, d.CarrierBudExpMo11.Value, 0),
                                            .CarrierBudExpMo12 = If(d.CarrierBudExpMo12.HasValue, d.CarrierBudExpMo12.Value, 0),
                                            .CarrierBudExpTotal = If(d.CarrierBudExpTotal.HasValue, d.CarrierBudExpTotal.Value, 0),
                                            .CarrierBudActMo1 = If(d.CarrierBudActMo1.HasValue, d.CarrierBudActMo1.Value, 0),
                                            .CarrierBudActMo2 = If(d.CarrierBudActMo2.HasValue, d.CarrierBudActMo2.Value, 0),
                                            .CarrierBudActMo3 = If(d.CarrierBudActMo3.HasValue, d.CarrierBudActMo3.Value, 0),
                                            .CarrierBudActMo4 = If(d.CarrierBudActMo4.HasValue, d.CarrierBudActMo4.Value, 0),
                                            .CarrierBudActMo5 = If(d.CarrierBudActMo5.HasValue, d.CarrierBudActMo5.Value, 0),
                                            .CarrierBudActMo6 = If(d.CarrierBudActMo6.HasValue, d.CarrierBudActMo6.Value, 0),
                                            .CarrierBudActMo7 = If(d.CarrierBudActMo7.HasValue, d.CarrierBudActMo7.Value, 0),
                                            .CarrierBudActMo8 = If(d.CarrierBudActMo8.HasValue, d.CarrierBudActMo8.Value, 0),
                                            .CarrierBudActMo9 = If(d.CarrierBudActMo9.HasValue, d.CarrierBudActMo9.Value, 0),
                                            .CarrierBudActMo10 = If(d.CarrierBudActMo10.HasValue, d.CarrierBudActMo10.Value, 0),
                                            .CarrierBudActMo11 = If(d.CarrierBudActMo11.HasValue, d.CarrierBudActMo11.Value, 0),
                                            .CarrierBudActMo12 = If(d.CarrierBudActMo12.HasValue, d.CarrierBudActMo12.Value, 0),
                                            .CarrierBudActTotal = If(d.CarrierBudActTotal.HasValue, d.CarrierBudActTotal.Value, 0),
                                            .CarrierBudgetUpdated = d.CarrierBudgetUpdated.ToArray()}).ToArray()
                Return CarrierBudgets

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
        Dim d = CType(oData, DTO.CarrierBudget)
        'Create New Record
        Return New LTS.CarrierBudget With {.CarrierBudControl = d.CarrierBudControl,
                                            .CarrierBudCarrierControl = d.CarrierBudCarrierControl,
                                            .CarrierBudModDate = Date.Now,
                                            .CarrierBudModUser = Parameters.UserName,
                                            .CarrierBudExpMo1 = d.CarrierBudExpMo1,
                                            .CarrierBudExpMo2 = d.CarrierBudExpMo2,
                                            .CarrierBudExpMo3 = d.CarrierBudExpMo3,
                                            .CarrierBudExpMo4 = d.CarrierBudExpMo4,
                                            .CarrierBudExpMo5 = d.CarrierBudExpMo5,
                                            .CarrierBudExpMo6 = d.CarrierBudExpMo6,
                                            .CarrierBudExpMo7 = d.CarrierBudExpMo7,
                                            .CarrierBudExpMo8 = d.CarrierBudExpMo8,
                                            .CarrierBudExpMo9 = d.CarrierBudExpMo9,
                                            .CarrierBudExpMo10 = d.CarrierBudExpMo10,
                                            .CarrierBudExpMo11 = d.CarrierBudExpMo11,
                                            .CarrierBudExpMo12 = d.CarrierBudExpMo12,
                                            .CarrierBudExpTotal = d.CarrierBudExpTotal,
                                            .CarrierBudActMo1 = d.CarrierBudActMo1,
                                            .CarrierBudActMo2 = d.CarrierBudActMo2,
                                            .CarrierBudActMo3 = d.CarrierBudActMo3,
                                            .CarrierBudActMo4 = d.CarrierBudActMo4,
                                            .CarrierBudActMo5 = d.CarrierBudActMo5,
                                            .CarrierBudActMo6 = d.CarrierBudActMo6,
                                            .CarrierBudActMo7 = d.CarrierBudActMo7,
                                            .CarrierBudActMo8 = d.CarrierBudActMo8,
                                            .CarrierBudActMo9 = d.CarrierBudActMo9,
                                            .CarrierBudActMo10 = d.CarrierBudActMo10,
                                            .CarrierBudActMo11 = d.CarrierBudActMo11,
                                            .CarrierBudActMo12 = d.CarrierBudActMo12,
                                            .CarrierBudActTotal = d.CarrierBudActTotal,
                                            .CarrierBudgetUpdated = If(d.CarrierBudgetUpdated Is Nothing, New Byte() {}, d.CarrierBudgetUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrierBudgetFiltered(Control:=CType(LinqTable, LTS.CarrierBudget).CarrierBudControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierBudget = TryCast(LinqTable, LTS.CarrierBudget)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrierBudgets
                       Where d.CarrierBudControl = source.CarrierBudControl
                       Select New DTO.QuickSaveResults With {.Control = d.CarrierBudControl _
                                                            , .ModDate = d.CarrierBudModDate _
                                                            , .ModUser = d.CarrierBudModUser _
                                                            , .Updated = d.CarrierBudgetUpdated.ToArray}).First

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

Public Class NGLCarrierDropLoadData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierDropLoads
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierDropLoadData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierDropLoads
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
        Return GetCarrierDropLoadFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrierDropLoadsFiltered()
    End Function

    Public Function GetCarrierDropLoadFiltered(ByVal Control As Integer) As DTO.CarrierDropLoad
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim CarrierDropLoad As DTO.CarrierDropLoad = (
                From d In db.CarrierDropLoads
                Where
                    d.CarrierDropControl = Control
                Select New DTO.CarrierDropLoad With {.CarrierDropControl = d.CarrierDropControl,
                                            .CarrierDropNumber = If(d.CarrierDropNumber.HasValue, d.CarrierDropNumber.Value, 0),
                                            .CarrierDropContact = d.CarrierDropContact,
                                            .CarrierDropProNumber = d.CarrierDropProNumber,
                                            .CarrierDropReason = d.CarrierDropReason,
                                            .CarrierDropDate = d.CarrierDropDate,
                                            .CarrierDropTime = d.CarrierDropTime,
                                            .CarrierDropReasonLocalized = d.CarrierDropReasonLocalized,
                                            .CarrierDropReasonKeys = d.CarrierDropReasonKeys,
                                            .CarrierDropLoadUpdated = d.CarrierDropLoadUpdated.ToArray()}).First


                Return CarrierDropLoad

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

    Public Function GetCarrierDropLoadsFiltered(Optional ByVal Number As Integer = 0, Optional ByVal Contact As String = "", Optional ByVal ProNumber As String = "") As DTO.CarrierDropLoad()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                db.Log = New DebugTextWriter
                'Return all the contacts that match the criteria sorted by name
                Dim CarrierDropLoads() As DTO.CarrierDropLoad = (
                From d In db.CarrierDropLoads
                Where
                    (d.CarrierDropNumber = If(Number = 0, d.CarrierDropNumber, Number)) _
                    And
                    (Contact Is Nothing OrElse String.IsNullOrEmpty(Contact) OrElse d.CarrierDropContact = Contact) _
                    And
                    (ProNumber Is Nothing OrElse String.IsNullOrEmpty(ProNumber) OrElse d.CarrierDropProNumber = ProNumber) _
                    And
                    (If(d.CarrierDropDate.HasValue, d.CarrierDropDate.Value, Date.Now).AddDays(366) > Date.Now)
                Order By d.CarrierDropDate Descending
                Select New DTO.CarrierDropLoad With {.CarrierDropControl = d.CarrierDropControl,
                                            .CarrierDropNumber = If(d.CarrierDropNumber.HasValue, d.CarrierDropNumber.Value, 0),
                                            .CarrierDropContact = d.CarrierDropContact,
                                            .CarrierDropProNumber = d.CarrierDropProNumber,
                                            .CarrierDropReason = d.CarrierDropReason,
                                            .CarrierDropDate = d.CarrierDropDate,
                                            .CarrierDropTime = d.CarrierDropTime,
                                            .CarrierDropReasonLocalized = d.CarrierDropReasonLocalized,
                                            .CarrierDropReasonKeys = d.CarrierDropReasonKeys,
                                            .CarrierDropLoadUpdated = d.CarrierDropLoadUpdated.ToArray()}).Take(5000).ToArray()
                Return CarrierDropLoads

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
        Dim d = CType(oData, DTO.CarrierDropLoad)
        'Create New Record
        Return New LTS.CarrierDropLoad With {.CarrierDropControl = d.CarrierDropControl,
                                            .CarrierDropNumber = d.CarrierDropNumber,
                                            .CarrierDropContact = d.CarrierDropContact,
                                            .CarrierDropProNumber = d.CarrierDropProNumber,
                                            .CarrierDropReason = d.CarrierDropReason,
                                            .CarrierDropDate = d.CarrierDropDate,
                                            .CarrierDropTime = d.CarrierDropTime,
                                            .CarrierDropReasonLocalized = d.CarrierDropReasonLocalized,
                                            .CarrierDropReasonKeys = d.CarrierDropReasonKeys,
                                            .CarrierDropLoadUpdated = If(d.CarrierDropLoadUpdated Is Nothing, New Byte() {}, d.CarrierDropLoadUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrierDropLoadFiltered(Control:=CType(LinqTable, LTS.CarrierDropLoad).CarrierDropControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierDropLoad = TryCast(LinqTable, LTS.CarrierDropLoad)
                If source Is Nothing Then Return Nothing
                'Note this data source does not have a Mod Date or Mod User data field
                ret = (From d In db.CarrierDropLoads
                       Where d.CarrierDropControl = source.CarrierDropControl
                       Select New DTO.QuickSaveResults With {.Control = d.CarrierDropControl _
                                                            , .ModDate = Date.Now _
                                                            , .ModUser = Parameters.UserName _
                                                            , .Updated = d.CarrierDropLoadUpdated.ToArray}).First

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

Public Class NGLCarrierEquipCodeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()

        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierEquipCodes
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierEquipCodeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierEquipCodes
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
        Return GetCarrierEquipCodeFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrierEquipCodesFiltered()
    End Function

    Public Function GetCarrierEquipCodeFiltered(ByVal Control As Integer) As DTO.CarrierEquipCode
        Dim retval As New DTO.CarrierEquipCode
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                retval = (From d In db.CarrierEquipCodes Where d.CarrierEquipControl = Control Select selectDTOData(d)).FirstOrDefault()

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrierEquipCodeFiltered"))
            End Try

            Return retval

        End Using
    End Function

    Public Function GetCarrierEquipCodesFiltered(Optional ByVal Code As String = "") As DTO.CarrierEquipCode()
        Dim retVal As DTO.CarrierEquipCode() = {New DTO.CarrierEquipCode()}

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                retVal = (From d In db.CarrierEquipCodes Where d.CarrierEquipCode.Contains(Code) Order By d.CarrierEquipCode Select selectDTOData(d)).ToArray()

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetCarrierEquipCodesFiltered"))
            End Try

            Return retVal

        End Using
    End Function

    Public Function GetCarrierEquipCodes(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.CarrierEquipCode()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.CarrierEquipCode

        'Dim intCompNumberFrom As Integer = 0
        'Dim intCompNumberTo As Integer = 0
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.CarrierEquipCode)
                iQuery = db.CarrierEquipCodes
                Dim filterWhere As String = ""
                Dim sFilterSpacer As String = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierEquipCodes"), db)
            End Try
        End Using
        Return oRet
    End Function

    Public Function SaveOrCreateCarrierEquipCodes(ByVal oData As LTS.CarrierEquipCode) As Boolean
        Dim blnRet As Boolean = False
        Dim blnvalue As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'oData.CarrierEquipModDate = Date.Now()
                'oData.CarrierEquipModUser = Me.Parameters.UserName
                If oData.CarrierEquipControl = 0 Then
                    db.CarrierEquipCodes.InsertOnSubmit(oData)
                Else
                    oData.rowguid = New Guid()
                    db.CarrierEquipCodes.Attach(oData, True)
                End If
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveOrCreateCarrierEquipCodes"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Deletes the CarrierEquipCode if it is not being used by a tariff (active or inactive)
    ''' Throws E_DataValidationFailure fault exception if the record is being used
    ''' </summary>
    ''' <param name="iCarrierEquipControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 09/23/2018
    ''' Modified by RHR for v-8.5.3.005 on 08/29/2022 we do not allow standard codes (control less than 100)  to be deleted
    '''     see code in ValidateDeletedRecord
    ''' </remarks>
    Public Function DeleteCarrierEquipCode(ByVal iCarrierEquipControl As Integer) As Boolean

        Dim blnRet As Boolean = False
        If iCarrierEquipControl = 0 Then Return False 'nothing to do

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim oToDelete = db.CarrierEquipCodes.Where(Function(x) x.CarrierEquipControl = iCarrierEquipControl).FirstOrDefault()
                If oToDelete Is Nothing OrElse oToDelete.CarrierEquipControl = 0 Then Return True 'already deleted

                Dim oFaultKey As SqlFaultInfo.FaultDetailsKey
                Dim oFaultDetails As New List(Of String)
                If Not ValidateDeletedRecord(db, oToDelete.CarrierEquipControl, oToDelete.CarrierEquipDescription, oFaultKey, oFaultDetails) Then
                    throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidRequest, oFaultKey, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)
                End If
                db.CarrierEquipCodes.DeleteOnSubmit(oToDelete)
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteCarrierEquipCode"), db)
            End Try
        End Using
        Return blnRet
    End Function


#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.CarrierEquipCode)
        'Create New Record
        Return New LTS.CarrierEquipCode With {.CarrierEquipControl = d.CarrierEquipControl,
                                                   .CarrierEquipCode = d.CarrierEquipCode,
                                                   .CarrierEquipDescription = d.CarrierEquipDescription,
                                                   .CarrierEquipCasesMinimum = d.CarrierEquipCasesMinimum,
                                                   .CarrierEquipCasesConsiderFull = d.CarrierEquipCasesConsiderFull,
                                                   .CarrierEquipsCasesMaximum = d.CarrierEquipCasesMaximum,
                                                   .CarrierEquipWgtMinimum = d.CarrierEquipWgtMinimum,
                                                   .CarrierEquipWgtConsiderFull = d.CarrierEquipWgtConsiderFull,
                                                   .CarrierEquipWgtMaximum = d.CarrierEquipWgtMaximum,
                                                   .CarrierEquipCubesMinimum = d.CarrierEquipCubesMinimum,
                                                   .CarrierEquipCubesConsiderFull = d.CarrierEquipCubesConsiderFull,
                                                   .CarrierEquipCubesMaximum = d.CarrierEquipCubesMaximum,
                                                   .CarrierEquipPltsMinimum = d.CarrierEquipPltsMinimum,
                                                   .CarrierEquipPltsConsiderFull = d.CarrierEquipPltsConsiderFull,
                                                   .CarrierEquipPltsMaximum = d.CarrierEquipPltsMaximum,
                                                   .CarrierEquipTempType = d.CarrierEquipTempType,
                                                   .CarrierEquipModUser = Parameters.UserName,
                                                   .CarrierEquipModDate = Date.Now(),
                                                   .CarrierEquipCodesUpdated = If(d.CarrierEquipCodesUpdated Is Nothing, New Byte() {}, d.CarrierEquipCodesUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrierEquipCodeFiltered(Control:=CType(LinqTable, LTS.CarrierEquipCode).CarrierEquipControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As New DTO.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrierEquipCode = TryCast(LinqTable, LTS.CarrierEquipCode)
                If source Is Nothing Then Return Nothing
                ret = (From d In db.CarrierEquipCodes
                       Where d.CarrierEquipControl = source.CarrierEquipControl
                       Select New DTO.QuickSaveResults With {.Control = d.CarrierEquipControl _
                                                            , .ModDate = d.CarrierEquipModDate _
                                                            , .ModUser = d.CarrierEquipModUser _
                                                            , .Updated = d.CarrierEquipCodesUpdated.ToArray}).First

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
    ''' Convert LTS Carrier Equip Code record to  DTO.CarrierEquipCode data object
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="page"></param>
    ''' <param name="pagecount"></param>
    ''' <param name="recordcount"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.3.005 on 08/29/2022 added CarrierEquipMapCode
    ''' </remarks>
    Friend Function selectDTOData(ByVal d As LTS.CarrierEquipCode, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.CarrierEquipCode
        Return New DTO.CarrierEquipCode With {.CarrierEquipControl = d.CarrierEquipControl,
                                                   .CarrierEquipCode = d.CarrierEquipCode,
                                                   .CarrierEquipDescription = d.CarrierEquipDescription,
                                                   .CarrierEquipCasesMinimum = d.CarrierEquipCasesMinimum,
                                                   .CarrierEquipCasesConsiderFull = d.CarrierEquipCasesConsiderFull,
                                                   .CarrierEquipCasesMaximum = d.CarrierEquipsCasesMaximum,
                                                   .CarrierEquipWgtMinimum = d.CarrierEquipWgtMinimum,
                                                   .CarrierEquipWgtConsiderFull = d.CarrierEquipWgtConsiderFull,
                                                   .CarrierEquipWgtMaximum = d.CarrierEquipWgtMaximum,
                                                   .CarrierEquipCubesMinimum = d.CarrierEquipCubesMinimum,
                                                   .CarrierEquipCubesConsiderFull = d.CarrierEquipCubesConsiderFull,
                                                   .CarrierEquipCubesMaximum = d.CarrierEquipCubesMaximum,
                                                   .CarrierEquipPltsMinimum = d.CarrierEquipPltsMinimum,
                                                   .CarrierEquipPltsConsiderFull = d.CarrierEquipPltsConsiderFull,
                                                   .CarrierEquipPltsMaximum = d.CarrierEquipPltsMaximum,
                                                   .CarrierEquipTempType = d.CarrierEquipTempType,
                                                   .CarrierEquipModUser = d.CarrierEquipModUser,
                                                   .CarrierEquipModDate = d.CarrierEquipModDate,
                                                   .CarrierEquipCodesUpdated = d.CarrierEquipCodesUpdated.ToArray(),
                                                   .CarrierEquipMapCode = d.CarrierEquipMapCode,
                                                   .Page = page,
                                                   .Pages = pagecount,
                                                   .RecordCount = recordcount,
                                                   .PageSize = pagesize}
    End Function

    ''' <summary>
    ''' Override for ValidateNewRecord query the CarrierEquipMapCode if it exists because the Desktop DTO does not support the CarrierEquipMapCode data
    '''     Specifically used for Legacy Desktop UI updates
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="oData"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.005 on 08/29/2022 added support for missing CarrierEquipMapCode in desktop UI
    '''     Note: do not call this method from the new Web UI.  Always use the LTS inteface SaveOrCreateCarrierEquipCodes
    ''' </remarks>
    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed      
        With CType(oData, DTO.CarrierEquipCode)
            Try
                'Check if a CarrierEquipMapCode is null or empty
                If (String.IsNullOrWhiteSpace(.CarrierEquipMapCode)) Then
                    Dim iCodeControl As Integer = .CarrierEquipControl
                    Dim oExisting = CType(oDB, NGLMASCarrierDataContext).CarrierEquipCodes.Where(Function(x) x.CarrierEquipControl = iCodeControl).FirstOrDefault()
                    If Not oExisting Is Nothing AndAlso oExisting.CarrierEquipControl = iCodeControl Then
                        If Not String.IsNullOrWhiteSpace(oExisting.CarrierEquipMapCode) AndAlso .CarrierEquipMapCode <> oExisting.CarrierEquipMapCode Then
                            .CarrierEquipMapCode = oExisting.CarrierEquipMapCode 'use the exiting map code because the desktop does not allow updates to the map code
                        End If
                    End If
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, "ValidateUpdatedRecord", oDB)
            End Try
        End With
    End Sub



    'Added By LVV on 8/11/16 for v-7.0.5.110 Ticket #2323
    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Make sure the fee type for the data is Order 
        Dim oCarrierDB As NGLMASCarrierDataContext = TryCast(oDB, NGLMASCarrierDataContext)
        Dim oCarrEquipCode As DTO.CarrierEquipCode = TryCast(oData, DTO.CarrierEquipCode)
        Dim oFaultKey As SqlFaultInfo.FaultDetailsKey
        Dim oFaultDetails As New List(Of String)
        If Not ValidateDeletedRecord(oCarrierDB, oCarrEquipCode, oFaultKey, oFaultDetails) Then
            throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidRequest, oFaultKey, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)
        End If
    End Sub

    'Added By LVV on 8/11/16 for v-7.0.5.110 Ticket #2323
    'Modified by RHR for v-8.2 on 09/23/2018 calls new ValidateDeletedRecord overload which does not require the DTO object
    Friend Overloads Shared Function ValidateDeletedRecord(ByRef oDB As NGLMASCarrierDataContext, ByRef oCarrEquipCode As DTO.CarrierEquipCode, ByRef oFaultKey As SqlFaultInfo.FaultDetailsKey, ByRef oFaultDetails As List(Of String)) As Boolean
        If oDB Is Nothing Then Return False
        If oCarrEquipCode Is Nothing Then Return True
        Dim nameList As New List(Of String)
        Dim carrierEquipControl = oCarrEquipCode.CarrierEquipControl
        Dim carrierEquipDesc = oCarrEquipCode.CarrierEquipDescription
        Return ValidateDeletedRecord(oDB, carrierEquipControl, carrierEquipDesc, oFaultKey, oFaultDetails)


    End Function


    ''' <summary>
    ''' Carrier Equipment Code standard ValidateDeletedRecord function cannot delete default codes or codes that are being used by any tariff
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="carrierEquipControl"></param>
    ''' <param name="carrierEquipDesc"></param>
    ''' <param name="oFaultKey"></param>
    ''' <param name="oFaultDetails"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 09/23/2018 this overload does not require a DTO object
    ''' Modified by RHR for v-8.5.3.005 on 08/29/2022 we do not allow standard codes (control less than 100)  to be deleted
    ''' </remarks>
    Friend Overloads Shared Function ValidateDeletedRecord(ByRef oDB As NGLMASCarrierDataContext,
                                                           ByVal carrierEquipControl As Integer,
                                                           ByVal carrierEquipDesc As String,
                                                           ByRef oFaultKey As SqlFaultInfo.FaultDetailsKey,
                                                           ByRef oFaultDetails As List(Of String)) As Boolean
        If carrierEquipControl < 100 Then
            oFaultKey = SqlFaultInfo.FaultDetailsKey.E_CannotDeleteRecordInUseDetails  'Cannot delete data the {0} value {1} is being used and cannot be modified.
            oFaultDetails = New List(Of String) From {"Default Equipment Codes", carrierEquipDesc}
            Return False
        End If

        If oDB Is Nothing Then Return False
        Dim nameList As New List(Of String)
        Using oDB
            'in vb linq to sql to query carriertarequip for all records with CarrTarEquipCarrierEquipControl = equipcontrol

            Dim retval = (From d In oDB.CarrierTariffEquipments
                          Where d.CarrTarEquipCarrierEquipControl = carrierEquipControl
                          Select d.CarrTarEquipCarrTarControl).Distinct().ToList()

            If Not retval Is Nothing AndAlso retval.Count > 0 Then
                For Each r In retval
                    Dim carName = (From d In oDB.CarrierTariffs
                                   Where d.CarrTarControl = r
                                   Select d.CarrTarName).Distinct().FirstOrDefault()
                    If Not String.IsNullOrEmpty(carName) Then
                        nameList.Add(carName)
                    End If
                Next
            End If

            'Check if a delete is allowed
            If Not nameList Is Nothing AndAlso nameList.Count > 0 Then
                'Cannot delete

                If nameList.Count > 5 Then
                    'Cannot delete data the Equipment Code {0} is being used by {1} tariffs and cannot be modified.
                    oFaultKey = SqlFaultInfo.FaultDetailsKey.E_CannotDeleteEquipCodeRecordInUseDetailsMany
                    oFaultDetails = New List(Of String) From {carrierEquipDesc, nameList.Count.ToString()}
                Else
                    'Cannot delete data the Equipment Code {0} is being used by the following tariffs and cannot be modified: {1}
                    Dim strNames As String = ""
                    For Each s In nameList
                        strNames += s + ","
                    Next
                    'remove the first , from the string
                    strNames = strNames.Trim().Substring(0, strNames.Length - 1)
                    'strNames.Remove(1, 1)
                    oFaultKey = SqlFaultInfo.FaultDetailsKey.E_CannotDeleteEquipCodeRecordInUseDetails
                    oFaultDetails = New List(Of String) From {carrierEquipDesc, strNames}
                End If

                Return False
            Else
                Return True
            End If

        End Using

    End Function

#End Region

End Class

Public Class NGLEDIStatusCodeData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.tblEDIStatusCodes
        Me.LinqDB = db
        Me.SourceClass = "NGLEDIStatusCodeData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.tblEDIStatusCodes
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
        Return GetEDIStatusCodeFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetEDIStatusCodesFiltered()
    End Function

    Public Function GetEDIStatusCodeFiltered(ByVal Control As Integer) As DTO.EDIStatusCodes
        Dim retval As New DTO.EDIStatusCodes
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                retval = (From d In db.tblEDIStatusCodes Where d.EDISControl = Control Select selectDTOData(d)).FirstOrDefault()

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetEDIStatusCodeFiltered"))
            End Try

            Return retval

        End Using
    End Function

    Public Function GetEDIStatusCodesFiltered(Optional ByVal Code As String = "") As DTO.EDIStatusCodes()
        Dim retVal As DTO.EDIStatusCodes() = {New DTO.EDIStatusCodes()}

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                retVal = (From d In db.tblEDIStatusCodes Where d.EDISCode.Contains(Code) Order By d.EDISCode Select selectDTOData(d)).ToArray()

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetEDIStatusCodesFiltered"))
            End Try

            Return retVal

        End Using
    End Function

    ''' <summary>
    ''' Get an array of records from the tblEDIStatusCodes where the 
    ''' Document Type ( EDITName), Element Name (EDIEName) and Event Code(EDISCode)
    ''' match the parameters provided
    ''' </summary>
    ''' <param name="DocTypeName"></param>
    ''' <param name="EDIElementName"></param>
    ''' <param name="EventCode"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 5/4/18 for v-8.2 Scheduler
    '''  Changed this linq query to a sp because we needed to remove the tblEDIType table
    '''  from the LTS.Carrier. This table is now only in LTS.EDIMaint
    '''  Modified by RHR for v-8.2.0.117 on 8/14/2019
    '''  fixed bug where retVal always returned an empty object in the first item of the array because it was instanciated with a New blank EDIStatusCodes
    '''   Dim retVal As New List(Of DTO.EDIStatusCodes)({New DTO.EDIStatusCodes()})
    ''' </remarks>
    Public Function GetEDIStatusCodesByDocAndElement(ByVal DocTypeName As String, ByVal EDIElementName As String, ByVal EventCode As String) As DTO.EDIStatusCodes()
        Dim retVal As New List(Of DTO.EDIStatusCodes) ' ({New DTO.EDIStatusCodes()}) 'Modified by RHR for v-8.2.0.117 on 8/14/2019
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim spRes = (From d In db.spGetEDIStatusCodesByDocAndElement(DocTypeName, EDIElementName, EventCode) Select d).ToArray()

                For Each s In spRes
                    Dim dtoSC As New DTO.EDIStatusCodes
                    With dtoSC
                        .EDISControl = s.EDISControl
                        .EDISEDITControl = s.EDISEDITControl
                        .EDISEDIEControl = s.EDISEDIEControl
                        .EDISEDIAControl = s.EDISEDIAControl
                        .EDISCode = s.EDISCode
                        .EDISDescription = s.EDISDescription
                        .EDISLoadStatusControl = s.EDISLoadStatusControl
                        .EDISUpdated = s.EDISUpdated.ToArray()
                    End With
                    retVal.Add(dtoSC)
                Next

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIStatusCodesByDocAndElement"))
            End Try
            Return retVal.ToArray()
        End Using
    End Function

    Public Function GetEDIStatusActionFactoryName(ByVal EDIAControl As Integer) As String
        Dim retVal As String = ""
        If EDIAControl = 0 Then Return retVal
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                retVal = (From d In db.tblEDIActions Where d.EDIAControl = EDIAControl Select d.EDIAFactoryName).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDIStatusActionFactoryName"))
            End Try
            Return retVal
        End Using
    End Function
#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.EDIStatusCodes)

        Dim skipObjs As New List(Of String) From {"EDISUpdated"}
        Dim oLTS As New LTS.tblEDIStatusCode
        oLTS = CopyMatchingFields(oLTS, d, skipObjs)
        'Create New Record
        With oLTS
            .EDISUpdated = If(d.EDISUpdated Is Nothing, New Byte() {}, d.EDISUpdated)
        End With

        'Create New Record
        Return oLTS
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetEDIStatusCodeFiltered(Control:=CType(LinqTable, LTS.tblEDIStatusCode).EDISControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As New DTO.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.tblEDIStatusCode = TryCast(LinqTable, LTS.tblEDIStatusCode)
                If source Is Nothing Then Return Nothing
                ret = (From d In db.tblEDIStatusCodes
                       Where d.EDISControl = source.EDISControl
                       Select New DTO.QuickSaveResults With {.Control = d.EDISControl,
                                                             .Updated = d.EDISUpdated.ToArray}).First

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

    Friend Function selectDTOData(ByVal d As LTS.tblEDIStatusCode, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.EDIStatusCodes
        Dim oDTO As New DTO.EDIStatusCodes
        Dim skipObjs As New List(Of String) From {"EDISUpdated", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .EDISUpdated = d.EDISUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

#End Region

End Class