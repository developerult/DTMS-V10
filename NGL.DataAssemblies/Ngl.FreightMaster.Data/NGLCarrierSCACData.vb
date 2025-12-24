Imports System.ServiceModel

Public Class NGLCarrierSCACData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrierSCACs
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierSCACData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrierSCACs
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
        Return GetCarrierSCACFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrierSCACsFiltered()
    End Function

    Public Function GetCarrierSCACFiltered(Optional ByVal Control As Integer = 0, Optional ByVal SCAC As String = "", Optional ByVal Carrier As String = "") As DataTransferObjects.CarrierSCAC
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CarrierSCAC As DataTransferObjects.CarrierSCAC = (
                        From t In db.CarrierSCACs
                        Where
                        (t.ID = If(Control = 0, t.ID, Control)) _
                        And
                        (SCAC Is Nothing OrElse String.IsNullOrEmpty(SCAC) OrElse t.SCAC = SCAC) _
                        And
                        (Carrier Is Nothing OrElse String.IsNullOrEmpty(Carrier) OrElse t.Carrier = Carrier)
                        Order By t.SCAC
                        Select New DataTransferObjects.CarrierSCAC With {.ID = t.ID _
                        , .SCAC = t.SCAC _
                        , .Carrier = t.Carrier}).First
                Return CarrierSCAC

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

    Public Function GetCarrierSCACsFiltered() As DataTransferObjects.CarrierSCAC()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CarrierSCACs() As DataTransferObjects.CarrierSCAC = (
                        From t In db.CarrierSCACs
                        Order By t.SCAC
                        Select New DataTransferObjects.CarrierSCAC With {.ID = t.ID _
                        , .SCAC = t.SCAC _
                        , .Carrier = t.Carrier}).ToArray()
                Return CarrierSCACs

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
        Dim d = CType(oData, DataTransferObjects.CarrierSCAC)
        'Create New Record
        Return New LTS.CarrierSCAC With {.ID = d.ID _
            , .SCAC = d.SCAC _
            , .Carrier = d.Carrier}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrierSCACFiltered(Control:=CType(LinqTable, LTS.CarrierSCAC).ID)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DataTransferObjects.CarrierSCAC)
            Try
                Dim CarrierSCAC As DataTransferObjects.CarrierSCAC = (
                        From t In CType(oDB, NGLMASCarrierDataContext).CarrierSCACs
                        Where
                        (t.SCAC = .SCAC _
                         Or
                         t.Carrier = .Carrier)
                        Select New DataTransferObjects.CarrierSCAC With {.ID = t.ID}).First

                If Not CarrierSCAC Is Nothing Then
                    Utilities.SaveAppError("Cannot add new CarrierSCAC.  The SCAC code, " & .SCAC & " or the Carrier name, " & .Carrier & ",  already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DataTransferObjects.CarrierSCAC)
            Try
                Dim CarrierSCAC As DataTransferObjects.CarrierSCAC = (
                        From t In CType(oDB, NGLMASCarrierDataContext).CarrierSCACs
                        Where
                        (t.ID <> .ID) _
                        And
                        (t.SCAC = .SCAC _
                         Or
                         t.Carrier = .Carrier)
                        Select New DataTransferObjects.CarrierSCAC With {.ID = t.ID}).First

                If Not CarrierSCAC Is Nothing Then
                    Utilities.SaveAppError("Cannot save the CarrierSCAC changes.  The SCAC code, " & .SCAC & " or the Carrier name, " & .Carrier & ",  already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

#End Region

End Class