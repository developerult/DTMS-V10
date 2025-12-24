Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class OutdatedNStatusOrderAlert
        Inherits DTOBaseClass


#Region " Data Members"

        Private _BookControl As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property BookControl() As System.Nullable(Of Integer)
            Get
                Return Me._BookControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._BookControl.Equals(value) = False) Then
                    Me._BookControl = value
                End If
            End Set
        End Property

        Private _BookProNumber As String
        <DataMember()> _
        Public Property BookProNumber() As String
            Get
                Return Me._BookProNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookProNumber, value) = False) Then
                    Me._BookProNumber = value
                End If
            End Set
        End Property

        Private _BookDateLoad As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateLoad() As System.Nullable(Of Date)
            Get
                Return Me._BookDateLoad
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._BookDateLoad.Equals(value) = False) Then
                    Me._BookDateLoad = value
                End If
            End Set
        End Property

        Private _QueryDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property QueryDate() As System.Nullable(Of Date)
            Get
                Return Me._QueryDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._QueryDate.Equals(value) = False) Then
                    Me._QueryDate = value
                End If
            End Set
        End Property

        Private _CompControl As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property CompControl() As System.Nullable(Of Integer)
            Get
                Return Me._CompControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._CompControl.Equals(value) = False) Then
                    Me._CompControl = value
                End If
            End Set
        End Property

        Private _CompNumber As String
        <DataMember()> _
        Public Property CompNumber() As String
            Get
                Return Me._CompNumber
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CompNumber, value) = False) Then
                    Me._CompNumber = value
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New OutdatedNStatusOrderAlert
            instance = DirectCast(MemberwiseClone(), OutdatedNStatusOrderAlert)
            Return instance
        End Function

#End Region

    End Class
End Namespace

