
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblHdm
        Inherits DTOBaseClass


#Region " Data Members"
        Private _HDMControl As Integer = 0
        <DataMember()> _
        Public Property HDMControl() As Integer
            Get
                Return _HDMControl
            End Get
            Set(ByVal value As Integer)
                _HDMControl = value
            End Set
        End Property

        Private _HDMCarrierControl As Integer = 0
        <DataMember()> _
        Public Property HDMCarrierControl() As Integer
            Get
                Return _HDMCarrierControl
            End Get
            Set(ByVal value As Integer)
                _HDMCarrierControl = value
            End Set
        End Property

        Private _HDMName As String = ""
        <DataMember()> _
        Public Property HDMName() As String
            Get
                Return Left(_HDMName, 50)
            End Get
            Set(ByVal value As String)
                _HDMName = Left(value, 50)
            End Set
        End Property

        Private _HDMDesc As String = ""
        <DataMember()> _
        Public Property HDMDesc() As String
            Get
                Return Left(_HDMDesc, 1000)
            End Get
            Set(ByVal value As String)
                _HDMDesc = Left(value, 1000)
            End Set
        End Property

        Private _HDMModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property HDMModDate() As System.Nullable(Of Date)
            Get
                Return _HDMModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _HDMModDate = value
            End Set
        End Property

        Private _HDMModUser As String = ""
        <DataMember()> _
        Public Property HDMModUser() As String
            Get
                Return Left(_HDMModUser, 100)
            End Get
            Set(ByVal value As String)
                _HDMModUser = Left(value, 100)
            End Set
        End Property

        Private _HDMUpdated As Byte()
        <DataMember()> _
        Public Property HDMUpdated() As Byte()
            Get
                Return _HDMUpdated
            End Get
            Set(ByVal value As Byte())
                _HDMUpdated = value
            End Set
        End Property

        'Added By LVV on 9/21/16 for v-7.0.5.110 HDM Enhancement
        Private _Selected As Boolean = False
        <DataMember()> _
        Public Property Selected() As Boolean
            Get
                Return _Selected
            End Get
            Set(ByVal value As Boolean)
                _Selected = value
            End Set
        End Property

        'Added By LVV on 9/27/16 for v-7.0.5.110 HDM Enhancement

        Private _HDMMinimum As Decimal = 0
        <DataMember()> _
        Public Property HDMMinimum() As Decimal
            Get
                Return _HDMMinimum
            End Get
            Set(ByVal value As Decimal)
                _HDMMinimum = value
            End Set
        End Property

        Private _HDMVariable As Double = 0
        <DataMember()> _
        Public Property HDMVariable() As Double
            Get
                Return _HDMVariable
            End Get
            Set(ByVal value As Double)
                _HDMVariable = value
            End Set
        End Property

        Private _HDMVariableCode As Integer = 0
        <DataMember()> _
        Public Property HDMVariableCode() As Integer
            Get
                Return _HDMVariableCode
            End Get
            Set(ByVal value As Integer)
                _HDMVariableCode = value
            End Set
        End Property

        Private _HDMMaximum As Decimal = 0
        <DataMember()>
        Public Property HDMMaximum() As Decimal
            Get
                Return _HDMMaximum
            End Get
            Set(ByVal value As Decimal)
                _HDMMaximum = value
            End Set
        End Property

        'Begin Modified by RHR for v-8.5.0.001 on 10/31/2021
        Private _HDMLEAdminControl As Integer? = 0
        <DataMember()>
        Public Property HDMLEAdminControl() As Integer?
            Get
                If (Not _HDMLEAdminControl.HasValue) Then _HDMLEAdminControl = 0
                Return _HDMLEAdminControl
            End Get
            Set(ByVal value As Integer?)
                If (Not value.HasValue) Then value = 0
                _HDMLEAdminControl = value
            End Set
        End Property

        Private _HDMActive As Boolean? = 0
        <DataMember()>
        Public Property HDMActive() As Boolean?
            Get
                If (Not _HDMActive.HasValue) Then _HDMActive = False
                Return _HDMActive
            End Get
            Set(ByVal value As Boolean?)
                If (Not value.HasValue) Then value = False
                _HDMActive = value
            End Set
        End Property




        Private _HDMCompControl As Integer? = 0
        <DataMember()>
        Public Property HDMCompControl() As Integer?
            Get
                If (Not _HDMCompControl.HasValue) Then _HDMCompControl = 0
                Return _HDMCompControl
            End Get
            Set(ByVal value As Integer?)
                If (Not value.HasValue) Then value = 0
                _HDMCompControl = value
            End Set
        End Property


#End Region

#Region " Public Methods"

        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblHdm
            instance = DirectCast(MemberwiseClone(), tblHdm)
            Return instance
        End Function

#End Region

    End Class
End Namespace
