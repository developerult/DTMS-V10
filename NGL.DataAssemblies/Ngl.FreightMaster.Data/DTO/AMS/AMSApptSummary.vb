Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class AMSApptSummary
        Inherits DTOBaseClass

#Region " Data Members"

        Private _AMSApptDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property AMSApptDate As System.Nullable(Of Date)
            Get
                Return _AMSApptDate
            End Get
            Set(value As System.Nullable(Of Date))
                _AMSApptDate = value
            End Set
        End Property

        Private _AMSApptLabel As String
        <DataMember()> _
        Public Property AMSApptLabel As String
            Get
                Return Left(_AMSApptLabel, 500)
            End Get
            Set(value As String)
                _AMSApptLabel = Left(value, 500)
            End Set
        End Property

        Private _AMSApptHover As String
        <DataMember()> _
        Public Property AMSApptHover As String
            Get
                Return Left(_AMSApptHover, 4000)
            End Get
            Set(value As String)
                _AMSApptHover = Left(value, 4000)
            End Set
        End Property

        Private _AMSApptOrderCount As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property AMSApptOrderCount As System.Nullable(Of Integer)
            Get
                Return _AMSApptOrderCount
            End Get
            Set(value As System.Nullable(Of Integer))
                _AMSApptOrderCount = value
            End Set
        End Property

        Private _AMSApptCount As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property AMSApptCount As System.Nullable(Of Integer)
            Get
                Return _AMSApptCount
            End Get
            Set(value As System.Nullable(Of Integer))
                _AMSApptCount = value
            End Set
        End Property

#End Region



#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New AMSApptSummary
            instance = DirectCast(MemberwiseClone(), AMSApptSummary)
            Return instance
        End Function

#End Region
    End Class
End Namespace
