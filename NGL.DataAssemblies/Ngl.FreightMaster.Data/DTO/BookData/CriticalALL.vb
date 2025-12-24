Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CriticalALL
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrierControl() As Integer
            Get
                Return _CarrierControl
            End Get
            Friend Set(ByVal value As Integer)
                _CarrierControl = value
            End Set
        End Property

        Private _CarrierNumber As Integer = 0
        <DataMember()> _
        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Friend Set(ByVal value As Integer)
                _CarrierNumber = value
            End Set
        End Property

        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return _CarrierName
            End Get
            Friend Set(ByVal value As String)
                _CarrierName = value
            End Set
        End Property

        Private _Load_Count As Integer = 0
        <DataMember()> _
        Public Property Load_Count() As Integer
            Get
                Return _Load_Count
            End Get
            Friend Set(ByVal value As Integer)
                _Load_Count = value
            End Set
        End Property

        Private _Critical As Integer = 0
        <DataMember()> _
        Public Property Critical() As Integer
            Get
                Return _Critical
            End Get
            Friend Set(ByVal value As Integer)
                _Critical = value
            End Set
        End Property

        Private _Message As String = ""
        <DataMember()> _
        Public Property Message() As String
            Get
                If Critical <> 0 Then
                    _Message = "LATE LOADS"
                Else
                    _Message = ""
                End If
                Return _Message
            End Get
            Friend Set(ByVal value As String)
                _Message = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CriticalALL
            instance = DirectCast(MemberwiseClone(), CriticalALL)
            Return instance
        End Function

#End Region
    End Class
End Namespace
