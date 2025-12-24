Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class EDIStatusCodes
        Inherits DTOBaseClass


#Region " Data Members"
        Private _EDISControl As Integer = 0
        <DataMember()> _
        Public Property EDISControl() As Integer
            Get
                Return _EDISControl
            End Get
            Set(ByVal value As Integer)
                If (Me._EDISControl <> value) Then
                    Me._EDISControl = value
                    Me.SendPropertyChanged("EDISControl")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Maps to EDI Type lookup
        ''' </summary>
        ''' <remarks></remarks>
        Private _EDISEDITControl As Integer = 0
        <DataMember()> _
        Public Property EDISEDITControl() As Integer
            Get
                Return _EDISEDITControl
            End Get
            Set(ByVal value As Integer)
                If (Me._EDISEDITControl <> value) Then
                    Me._EDISEDITControl = value
                    Me.SendPropertyChanged("EDISEDITControl")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Mapps to EDI Element lookup
        ''' </summary>
        ''' <remarks></remarks>
        Private _EDISEDIEControl As Integer = 0
        <DataMember()> _
        Public Property EDISEDIEControl() As Integer
            Get
                Return _EDISEDIEControl
            End Get
            Set(ByVal value As Integer)
                If (Me._EDISEDIEControl <> value) Then
                    Me._EDISEDIEControl = value
                    Me.SendPropertyChanged("EDISEDIEControl")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Maps to EDI Action lookup
        ''' </summary>
        ''' <remarks></remarks>
        Private _EDISEDIAControl As Integer = 0
        <DataMember()> _
        Public Property EDISEDIAControl() As Integer
            Get
                Return _EDISEDIAControl
            End Get
            Set(ByVal value As Integer)
                If (Me._EDISEDIAControl <> value) Then
                    Me._EDISEDIAControl = value
                    Me.SendPropertyChanged("EDISEDIAControl")
                End If
            End Set
        End Property

        ''' <summary>
        ''' manual entry code
        ''' </summary>
        ''' <remarks></remarks>
        Private _EDISCode As String = ""
        <DataMember()> _
        Public Property EDISCode() As String
            Get
                Return Left(_EDISCode, 10)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._EDISCode, value) = False) Then
                    Me._EDISCode = Left(value, 10)
                    Me.SendPropertyChanged("EDISCode")
                End If
            End Set
        End Property

        Private _EDISDescription As String = ""
        <DataMember()> _
        Public Property EDISDescription() As String
            Get
                Return Left(_EDISDescription, 255)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._EDISDescription, value) = False) Then
                    Me._EDISDescription = Left(value, 255)
                    Me.SendPropertyChanged("EDISDescription")
                End If
            End Set
        End Property

        Private _EDISUpdated As Byte()
        <DataMember()> _
        Public Property EDISUpdated() As Byte()
            Get
                Return _EDISUpdated
            End Get
            Set(ByVal value As Byte())
                _EDISUpdated = value
            End Set
        End Property

        ''' <summary>
        ''' Maps to load status control lookup
        ''' </summary>
        ''' <remarks></remarks>
        Private _EDISLoadStatusControl As Integer = 0
        <DataMember()> _
        Public Property EDISLoadStatusControl() As Integer
            Get
                Return _EDISLoadStatusControl
            End Get
            Set(ByVal value As Integer)
                If (Me._EDISLoadStatusControl <> value) Then
                    Me._EDISLoadStatusControl = value
                    Me.SendPropertyChanged("EDISLoadStatusControl")
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New EDIStatusCodes
            instance = DirectCast(MemberwiseClone(), EDIStatusCodes)
            Return instance
        End Function

#End Region

    End Class
End Namespace
