Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CompValidateCredit
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CompFinCreditAvail As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property CompFinCreditAvail() As Integer
            Get
                Return _CompFinCreditAvail
            End Get
            Set(ByVal value As Integer)
                _CompFinCreditAvail = value
            End Set
        End Property

        Private _Message As String
        <DataMember()> _
        Public Property Message() As String
            Get
                Return _Message
            End Get
            Set(ByVal value As String)
                _Message = value
            End Set
        End Property

        Private _Details As String
        <DataMember()> _
        Public Property Details() As String
            Get
                Return _Details
            End Get
            Set(ByVal value As String)
                _Details = value
            End Set
        End Property



#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CompValidateCredit
            instance = DirectCast(MemberwiseClone(), CompValidateCredit)
            Return instance
        End Function

#End Region


    End Class
End Namespace
