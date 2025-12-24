Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class AMSAppointmentResult
        Inherits DTOBaseClass

#Region " Data Members"

        Private _Appointment As AMSAppointment
        <DataMember()> _
        Public Property Appointment() As AMSAppointment
            Get
                Return _Appointment
            End Get
            Set(ByVal value As AMSAppointment)
                _Appointment = value
            End Set
        End Property

        Private _AppointmentOrders As AMSOrderList()
        <DataMember()> _
        Public Property AppointmentOrders() As AMSOrderList()
            Get
                Return _AppointmentOrders
            End Get
            Set(ByVal value As AMSOrderList())
                _AppointmentOrders = value
            End Set
        End Property

#End Region



#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New AMSAppointmentResult
            instance = DirectCast(MemberwiseClone(), AMSAppointmentResult)
            Return instance
        End Function

#End Region
    End Class
End Namespace
