
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker


Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTarEquipMatNode
        Inherits DTOBaseClass


#Region " Data Members"
        'Private _CarrTarEquipMatControl As Integer = 0
        '<DataMember()> _
        'Public Property CarrTarEquipMatControl() As Integer
        '    Get
        '        Return _CarrTarEquipMatControl
        '    End Get
        '    Set(ByVal value As Integer)
        '        If (Me._CarrTarEquipMatControl <> value) Then
        '            Me._CarrTarEquipMatControl = value
        '            Me.SendPropertyChanged("CarrTarEquipMatControl")
        '        End If
        '    End Set
        'End Property

        Private _CarrTarEquipMatCarrTarEquipControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatCarrTarEquipControl() As Integer
            Get
                Return _CarrTarEquipMatCarrTarEquipControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatCarrTarEquipControl <> value) Then
                    Me._CarrTarEquipMatCarrTarEquipControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatCarrTarEquipControl")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatCarrTarControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatCarrTarControl() As Integer
            Get
                Return _CarrTarEquipMatCarrTarControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatCarrTarControl <> value) Then
                    Me._CarrTarEquipMatCarrTarControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatCarrTarControl")
                End If
            End Set
        End Property

        'Private _CarrTarEquipMatCarrTarMatControl As System.Nullable(Of Integer)
        '<DataMember()> _
        'Public Property CarrTarEquipMatCarrTarMatControl() As System.Nullable(Of Integer)
        '    Get
        '        Return _CarrTarEquipMatCarrTarMatControl
        '    End Get
        '    Set(ByVal value As System.Nullable(Of Integer))
        '        If (Me._CarrTarEquipMatCarrTarMatControl.Equals(value) = False) Then
        '            Me._CarrTarEquipMatCarrTarMatControl = value
        '            Me.SendPropertyChanged("CarrTarEquipMatCarrTarMatControl")
        '        End If
        '    End Set
        'End Property

       

        Private _CarrTarEquipMatName As String = ""
        <DataMember()> _
        Public Property CarrTarEquipMatName() As String
            Get
                Return Left(_CarrTarEquipMatName, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarEquipMatName, value) = False) Then
                    Me._CarrTarEquipMatName = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarEquipMatName")
                End If
            End Set
        End Property

        'Private _CarrTarEquipMatClass As String = ""
        '<DataMember()> _
        'Public Property CarrTarEquipMatClass() As String
        '    Get
        '        Return Left(_CarrTarEquipMatClass, 50)
        '    End Get
        '    Set(ByVal value As String)
        '        If (String.Equals(Me._CarrTarEquipMatClass, value) = False) Then
        '            Me._CarrTarEquipMatClass = Left(value, 50)
        '            Me.SendPropertyChanged("CarrTarEquipMatClass")
        '        End If
        '    End Set
        'End Property

       
        Private _CarrTarEquipMatClassTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatClassTypeControl() As Integer
            Get
                Return _CarrTarEquipMatClassTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatClassTypeControl <> value) Then
                    Me._CarrTarEquipMatClassTypeControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatClassTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarEquipMatTarRateTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatTarRateTypeControl() As Integer
            Get
                Return _CarrTarEquipMatTarRateTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatTarRateTypeControl <> value) Then
                    Me._CarrTarEquipMatTarRateTypeControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatTarRateTypeControl")
                End If
            End Set
        End Property

         
        Private _CarrTarEquipMatTarBracketTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatTarBracketTypeControl() As Integer
            Get
                Return _CarrTarEquipMatTarBracketTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatTarBracketTypeControl <> value) Then
                    Me._CarrTarEquipMatTarBracketTypeControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatTarBracketTypeControl")
                End If
            End Set
        End Property
         
        Private _CarrTarMatBPPivot As New CarrTarMatBPPivot
        <DataMember()> _
        Public Property CarrTarMatBPPivot() As CarrTarMatBPPivot
            Get
                Return _CarrTarMatBPPivot
            End Get
            Set(ByVal value As CarrTarMatBPPivot)
                _CarrTarMatBPPivot = value
            End Set
        End Property

        Private _RatesList As New List(Of CarrTarEquipMatPivot)
        <DataMember()> _
        Public Property RatesList() As List(Of CarrTarEquipMatPivot)
            Get
                Return _RatesList
            End Get
            Set(ByVal value As List(Of CarrTarEquipMatPivot))
                _RatesList = value
            End Set
        End Property

        Private _CarrTarContract As New CarrTarContract
        Public Property Contract() As CarrTarContract
            Get
                Return _CarrTarContract
            End Get
            Set(ByVal value As CarrTarContract)
                _CarrTarContract = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrTarEquipMatNode
            instance = DirectCast(MemberwiseClone(), CarrTarEquipMatNode)
           
            Return instance
        End Function

#End Region

    End Class
End Namespace
