Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class PaymentForm
        Inherits DTOBaseClass


#Region " Data Members"
        Private _PaymentFormNumber As Integer = 0
        <DataMember()> _
        Public Property PaymentFormNumber() As Integer
            Get
                Return _PaymentFormNumber
            End Get
            Set(ByVal value As Integer)
                _PaymentFormNumber = value
            End Set
        End Property

        Private _PaymentFormType As String = ""
        <DataMember()> _
        Public Property PaymentFormType() As String
            Get
                Return Left(_PaymentFormType, 50)
            End Get
            Set(ByVal value As String)
                _PaymentFormType = Left(value, 50)
            End Set
        End Property

        Private _PaymentFormUpdated As Byte()
        <DataMember()> _
        Public Property PaymentFormUpdated() As Byte()
            Get
                Return _PaymentFormUpdated
            End Get
            Set(ByVal value As Byte())
                _PaymentFormUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New PaymentForm
            instance = DirectCast(MemberwiseClone(), PaymentForm)
            Return instance
        End Function

#End Region

    End Class
End Namespace