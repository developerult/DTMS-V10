Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects

    <DataContract()> _
    Public Class EmailInvoiceComps
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
                Return Left(_CompName, 40)
            End Get
            Set(ByVal value As String)
                _CompName = Left(value, 40)
            End Set
        End Property

        Private _CompFinInvPrnCode As Boolean = False
        <DataMember()> _
        Public Property CompFinInvPrnCode() As Boolean
            Get
                Return _CompFinInvPrnCode
            End Get
            Set(ByVal value As Boolean)
                _CompFinInvPrnCode = value
            End Set
        End Property

        Private _CompFinInvEMailCode As Boolean = False
        <DataMember()> _
        Public Property CompFinInvEMailCode() As Boolean
            Get
                Return _CompFinInvEMailCode
            End Get
            Set(ByVal value As Boolean)
                _CompFinInvEMailCode = value
            End Set
        End Property


        Private _CompMailTo As String = ""
        <DataMember()> _
        Public Property CompMailTo() As String
            Get
                Return Left(_CompMailTo, 500)
            End Get
            Set(ByVal value As String)
                _CompMailTo = Left(value, 500)
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New EmailInvoiceComps
            instance = DirectCast(MemberwiseClone(), EmailInvoiceComps)
            Return instance
        End Function

#End Region

    End Class

End Namespace
