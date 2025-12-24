Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class LaneProfileSettings
        Inherits DTOBaseClass


#Region " Data Members"
        Private _LaneProfileSettingsLaneControl As Integer = 0
        <DataMember()> _
        Public Property LaneProfileSettingsLaneControl() As Integer
            Get
                Return _LaneProfileSettingsLaneControl
            End Get
            Set(ByVal value As Integer)
                _LaneProfileSettingsLaneControl = value
            End Set
        End Property

        Private _LaneProfileSettingsLaneName As String = ""
        <DataMember()> _
        Public Property LaneProfileSettingsLaneName() As String
            Get
                Return Left(_LaneProfileSettingsLaneName, 50)
            End Get
            Set(ByVal value As String)
                _LaneProfileSettingsLaneName = Left(value, 50)
            End Set
        End Property

        Private _LaneProfileSettingsLaneNumber As String = ""
        <DataMember()> _
        Public Property LaneProfileSettingsLaneNumber() As String
            Get
                Return Left(_LaneProfileSettingsLaneNumber, 50)
            End Get
            Set(ByVal value As String)
                _LaneProfileSettingsLaneNumber = Left(value, 50)
            End Set
        End Property

        Private _LaneProfileSettingsAccessorialCode As Integer = 0
        <DataMember()> _
        Public Property LaneProfileSettingsAccessorialCode() As Integer
            Get
                Return _LaneProfileSettingsAccessorialCode
            End Get
            Set(ByVal value As Integer)
                _LaneProfileSettingsAccessorialCode = value
            End Set
        End Property

        Private _LaneProfileSettingsAccessorialName As String = ""
        <DataMember()> _
        Public Property LaneProfileSettingsAccessorialName() As String
            Get
                Return Left(_LaneProfileSettingsAccessorialName, 50)
            End Get
            Set(ByVal value As String)
                _LaneProfileSettingsAccessorialName = Left(value, 50)
            End Set
        End Property

        Private _LaneProfileSettingsSelected As Nullable(Of Boolean)
        <DataMember()> _
        Public Property LaneProfileSettingsSelected() As Nullable(Of Boolean)
            Get
                Return _LaneProfileSettingsSelected
            End Get
            Set(ByVal value As Nullable(Of Boolean))
                _LaneProfileSettingsSelected = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New LaneProfileSettings
            instance = DirectCast(MemberwiseClone(), LaneProfileSettings)
            Return instance
        End Function

#End Region

    End Class
End Namespace


