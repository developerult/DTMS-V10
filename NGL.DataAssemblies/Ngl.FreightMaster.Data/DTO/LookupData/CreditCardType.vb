Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CreditCardType
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CreditCardControlNumber As Integer = 0
        <DataMember()> _
        Public Property CreditCardControlNumber() As Integer
            Get
                Return _CreditCardControlNumber
            End Get
            Set(ByVal value As Integer)
                _CreditCardControlNumber = value
            End Set
        End Property

        Private _CreditCardTypeName As String = ""
        <DataMember()> _
        Public Property CreditCardTypeName() As String
            Get
                Return Left(_CreditCardTypeName, 50)
            End Get
            Set(ByVal value As String)
                _CreditCardTypeName = Left(value, 50)
            End Set
        End Property

        Private _CreditCardTypesUpdated As Byte()
        <DataMember()> _
        Public Property CreditCardTypesUpdated() As Byte()
            Get
                Return _CreditCardTypesUpdated
            End Get
            Set(ByVal value As Byte())
                _CreditCardTypesUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CreditCardType
            instance = DirectCast(MemberwiseClone(), CreditCardType)
            Return instance
        End Function

#End Region

    End Class
End Namespace