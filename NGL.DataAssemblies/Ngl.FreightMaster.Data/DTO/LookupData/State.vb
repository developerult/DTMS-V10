Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class State
        Inherits DTOBaseClass


#Region " Data Members"
        Private _StateControl As Integer = 0
        <DataMember()> _
        Public Property StateControl() As Integer
            Get
                Return _StateControl
            End Get
            Set(ByVal value As Integer)
                _StateControl = value
            End Set
        End Property

        Private _STATEABV As String = ""
        <DataMember()> _
        Public Property STATEABV() As String
            Get
                Return Left(_STATEABV, 2)
            End Get
            Set(ByVal value As String)
                _STATEABV = Left(value, 2)
            End Set
        End Property

        Private _STATENAME As String = ""
        <DataMember()> _
        Public Property STATENAME() As String
            Get
                Return Left(_STATENAME, 2)
            End Get
            Set(ByVal value As String)
                _STATENAME = Left(value, 2)
            End Set
        End Property

        Private _StatesUpdated As Byte()
        <DataMember()> _
        Public Property StatesUpdated() As Byte()
            Get
                Return _StatesUpdated
            End Get
            Set(ByVal value As Byte())
                _StatesUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New State
            instance = DirectCast(MemberwiseClone(), State)
            Return instance
        End Function

#End Region

    End Class
End Namespace