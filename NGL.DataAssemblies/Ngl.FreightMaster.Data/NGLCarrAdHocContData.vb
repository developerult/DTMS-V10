Imports System.ServiceModel

Public Class NGLCarrAdHocContData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()

        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrAdHocConts
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrAdHocContData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrAdHocConts
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
        Return GetCarrAdHocContFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrAdHocContsFiltered()
    End Function

    Public Function GetCarrAdHocContFiltered(Optional ByVal Control As Integer = 0, Optional ByVal Name As String = "", Optional ByVal Email As String = "") As DataTransferObjects.CarrAdHocCont
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim CarrAdHocCont As DataTransferObjects.CarrAdHocCont = (
                        From d In db.CarrAdHocConts
                        Where
                        (d.CarrAdHocContControl = If(Control = 0, d.CarrAdHocContControl, Control)) _
                        And
                        (Name Is Nothing OrElse String.IsNullOrEmpty(Name) OrElse d.CarrAdHocContName = Name) _
                        And
                        (Email Is Nothing OrElse String.IsNullOrEmpty(Email) OrElse d.CarrAdHocContactEMail = Email)
                        Select New DataTransferObjects.CarrAdHocCont With {.CarrAdHocContControl = d.CarrAdHocContControl,
                        .CarrAdHocContCarrAdHocControl = d.CarrAdHocContCarrAdHocControl,
                        .CarrAdHocContName = d.CarrAdHocContName,
                        .CarrAdHocContTitle = d.CarrAdHocContTitle,
                        .CarrAdHocContactPhone = d.CarrAdHocContactPhone,
                        .CarrAdHocContPhoneExt = d.CarrAdHocContPhoneExt,
                        .CarrAdHocContactFax = d.CarrAdHocContactFax,
                        .CarrAdHocContact800 = d.CarrAdHocContact800,
                        .CarrAdHocContactEMail = d.CarrAdHocContactEMail,
                        .CarrAdHocContUpdated = d.CarrAdHocContUpdated.ToArray()}).First


                Return CarrAdHocCont

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

    Public Function GetCarrAdHocContsFiltered(Optional ByVal CarrAdHocControl As Integer = 0, Optional ByVal Name As String = "", Optional ByVal Email As String = "") As DataTransferObjects.CarrAdHocCont()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CarrAdHocConts() As DataTransferObjects.CarrAdHocCont = (
                        From d In db.CarrAdHocConts
                        Where
                        (d.CarrAdHocContCarrAdHocControl = If(CarrAdHocControl = 0, d.CarrAdHocContCarrAdHocControl, CarrAdHocControl)) _
                        And
                        (Name Is Nothing OrElse String.IsNullOrEmpty(Name) OrElse d.CarrAdHocContName = Name) _
                        And
                        (Email Is Nothing OrElse String.IsNullOrEmpty(Email) OrElse d.CarrAdHocContactEMail = Email)
                        Order By d.CarrAdHocContName
                        Select New DataTransferObjects.CarrAdHocCont With {.CarrAdHocContControl = d.CarrAdHocContControl,
                        .CarrAdHocContCarrAdHocControl = d.CarrAdHocContCarrAdHocControl,
                        .CarrAdHocContName = d.CarrAdHocContName,
                        .CarrAdHocContTitle = d.CarrAdHocContTitle,
                        .CarrAdHocContactPhone = d.CarrAdHocContactPhone,
                        .CarrAdHocContPhoneExt = d.CarrAdHocContPhoneExt,
                        .CarrAdHocContactFax = d.CarrAdHocContactFax,
                        .CarrAdHocContact800 = d.CarrAdHocContact800,
                        .CarrAdHocContactEMail = d.CarrAdHocContactEMail,
                        .CarrAdHocContUpdated = d.CarrAdHocContUpdated.ToArray()}).ToArray()
                Return CarrAdHocConts

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
        Dim d = CType(oData, DataTransferObjects.CarrAdHocCont)
        'Create New Record
        Return New LTS.CarrAdHocCont With {.CarrAdHocContControl = d.CarrAdHocContControl,
            .CarrAdHocContCarrAdHocControl = d.CarrAdHocContCarrAdHocControl,
            .CarrAdHocContName = d.CarrAdHocContName,
            .CarrAdHocContTitle = d.CarrAdHocContTitle,
            .CarrAdHocContactPhone = d.CarrAdHocContactPhone,
            .CarrAdHocContPhoneExt = d.CarrAdHocContPhoneExt,
            .CarrAdHocContactFax = d.CarrAdHocContactFax,
            .CarrAdHocContact800 = d.CarrAdHocContact800,
            .CarrAdHocContactEMail = d.CarrAdHocContactEMail,
            .CarrAdHocContUpdated = If(d.CarrAdHocContUpdated Is Nothing, New Byte() {}, d.CarrAdHocContUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrAdHocContFiltered(Control:=CType(LinqTable, LTS.CarrAdHocCont).CarrAdHocContControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrAdHocCont = TryCast(LinqTable, LTS.CarrAdHocCont)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrAdHocConts
                    Where d.CarrAdHocContControl = source.CarrAdHocContControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrAdHocContControl _
                        , .ModDate = Date.Now _
                        , .ModUser = Parameters.UserName _
                        , .Updated = d.CarrAdHocContUpdated.ToArray}).First

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