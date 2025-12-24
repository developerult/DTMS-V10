Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CompCredit
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CompControl As Integer = 0
        <DataMember()> _
        Public Property CompControl() As Integer
            Get
                Return _CompControl
            End Get
            Set(ByVal value As Integer)
                _CompControl = value
            End Set
        End Property

        Private _CompNumber As Integer = 0
        <DataMember()> _
        Public Property CompNumber() As Integer
            Get
                Return _CompNumber
            End Get
            Set(ByVal value As Integer)
                _CompNumber = value
            End Set
        End Property

        Private _CompName As String = ""
        <DataMember()> _
        Public Property CompName() As String
            Get
                Return _CompName
            End Get
            Set(ByVal value As String)
                _CompName = value
            End Set
        End Property

        Private _CompCreditAssigned As Integer = 0
        <DataMember()> _
        Public Property CompCreditAssigned() As Integer
            Get
                Return _CompCreditAssigned
            End Get
            Set(ByVal value As Integer)
                _CompCreditAssigned = value
            End Set
        End Property

        Private _CompCreditUsed As Integer = 0
        <DataMember()> _
        Public Property CompCreditUsed() As Integer
            Get
                Return _CompCreditUsed
            End Get
            Set(ByVal value As Integer)
                _CompCreditUsed = value
            End Set
        End Property

        Private _CompCreditAvailable As Integer = 0
        <DataMember()> _
        Public Property CompCreditAvailable() As Integer
            Get
                Return _CompCreditAvailable
            End Get
            Set(ByVal value As Integer)
                _CompCreditAvailable = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CompCredit
            instance = DirectCast(MemberwiseClone(), CompCredit)
            Return instance
        End Function

#End Region

    End Class
End Namespace