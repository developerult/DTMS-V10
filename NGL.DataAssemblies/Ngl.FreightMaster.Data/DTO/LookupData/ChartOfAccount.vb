Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class ChartOfAccount
        Inherits DTOBaseClass


#Region " Data Members"

        Private _ID As Integer = 0
        <DataMember()> _
        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property

        Private _AcctNo As String = ""
        <DataMember()> _
        Public Property AcctNo() As String
            Get
                Return Left(_AcctNo, 50)
            End Get
            Set(ByVal value As String)
                _AcctNo = Left(value, 50)
            End Set
        End Property

        Private _AcctDescription As String = ""
        <DataMember()> _
        Public Property AcctDescription() As String
            Get
                Return Left(_AcctDescription, 200)
            End Get
            Set(ByVal value As String)
                _AcctDescription = Left(value, 200)
            End Set
        End Property

        Private _AcctType As String = ""
        <DataMember()> _
        Public Property AcctType() As String
            Get
                Return Left(_AcctType, 1)
            End Get
            Set(ByVal value As String)
                _AcctType = Left(value, 1)
            End Set
        End Property

        Private _AcctLine As String = ""
        <DataMember()> _
        Public Property AcctLine() As String
            Get
                Return Left(_AcctLine, 75)
            End Get
            Set(ByVal value As String)
                _AcctLine = Left(value, 75)
            End Set
        End Property

        Private _AcctLineNumber As String = ""
        <DataMember()> _
        Public Property AcctLineNumber() As String
            Get
                Return Left(_AcctLineNumber, 5)
            End Get
            Set(ByVal value As String)
                _AcctLineNumber = Left(value, 5)
            End Set
        End Property

        Private _AcctLinNumberSub As String = ""
        <DataMember()> _
        Public Property AcctLinNumberSub() As String
            Get
                Return Left(_AcctLinNumberSub, 2)
            End Get
            Set(ByVal value As String)
                _AcctLinNumberSub = Left(value, 2)
            End Set
        End Property

        Private _AcctUpdated As Byte()
        <DataMember()> _
        Public Property AcctUpdated() As Byte()
            Get
                Return _AcctUpdated
            End Get
            Set(ByVal value As Byte())
                _AcctUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New ChartOfAccount
            instance = DirectCast(MemberwiseClone(), ChartOfAccount)
            Return instance
        End Function

#End Region

    End Class
End Namespace
