Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class SystemAlertNode
        Inherits DTOBaseClass


#Region " Data Members"

        Private _ATID As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property ATID() As System.Nullable(Of Integer)
            Get
                Return Me._ATID
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._ATID.Equals(value) = False) Then
                    Me._ATID = value
                End If
            End Set
        End Property

        Private _ATName As String
        <DataMember()> _
        Public Property ATName() As String
            Get
                Return Me._ATName
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ATName, value) = False) Then
                    Me._ATName = value
                End If
            End Set
        End Property

        Private _ATDescription As String
        <DataMember()> _
        Public Property ATDescription() As String
            Get
                Return Me._ATDescription
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ATDescription, value) = False) Then
                    Me._ATDescription = value
                End If
            End Set
        End Property

        Private _ATAlertCount As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property ATAlertCount() As System.Nullable(Of Integer)
            Get
                Return Me._ATAlertCount
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._ATAlertCount.Equals(value) = False) Then
                    Me._ATAlertCount = value
                End If
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New SystemAlertNode
            instance = DirectCast(MemberwiseClone(), SystemAlertNode)
            Return instance
        End Function

#End Region

    End Class
End Namespace


