Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTarContract
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrTarControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarControl() As Integer
            Get
                Return _CarrTarControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarControl <> value) Then
                    Me._CarrTarControl = value
                    Me.SendPropertyChanged("CarrTarControl")
                End If
            End Set
        End Property


        Private _CarrTarCarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarCarrierControl() As Integer
            Get
                Return _CarrTarCarrierControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarCarrierControl <> value) Then
                    Me._CarrTarCarrierControl = value
                    Me.SendPropertyChanged("CarrTarCarrierControl")
                End If
            End Set
        End Property


        Private _CarrTarCompControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarCompControl() As Integer
            Get
                Return _CarrTarCompControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarCompControl <> value) Then
                    Me._CarrTarCompControl = value
                    Me.SendPropertyChanged("CarrTarCompControl")
                End If
            End Set
        End Property


        Private _CarrTarID As String = ""
        <DataMember()> _
        Public Property CarrTarID() As String
            Get
                Return Left(_CarrTarID, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarID, value) = False) Then
                    Me._CarrTarID = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarID")
                End If
            End Set
        End Property

        Private _CarrTarBPBracketType As Integer = 0
        <DataMember()> _
        Public Property CarrTarBPBracketType() As Integer
            Get
                Return _CarrTarBPBracketType
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarBPBracketType <> value) Then
                    Me._CarrTarBPBracketType = value
                    Me.SendPropertyChanged("CarrTarBPBracketType")
                End If
            End Set
        End Property


        Private _CarrTarTLCapacityType As Integer = 0
        <DataMember()> _
        Public Property CarrTarTLCapacityType() As Integer
            Get
                Return _CarrTarTLCapacityType
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarTLCapacityType <> value) Then
                    Me._CarrTarTLCapacityType = value
                    Me.SendPropertyChanged("CarrTarTLCapacityType")
                End If
            End Set
        End Property


        Private _CarrTarTempType As Integer = 0
        <DataMember()> _
        Public Property CarrTarTempType() As Integer
            Get
                Return _CarrTarTempType
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarTempType <> value) Then
                    Me._CarrTarTempType = value
                    Me.SendPropertyChanged("CarrTarTempType")
                End If
            End Set
        End Property


        Private _CarrTarTariffType As String = "I"
        <DataMember()> _
        Public Property CarrTarTariffType() As String
            Get
                Return If(Asc(Left(_CarrTarTariffType, 1)) < 1, "I", Left(_CarrTarTariffType, 1))
            End Get
            Set(ByVal value As String)
                If (value Is Nothing) Then value = "I"
                Dim strVal = If(Asc(Left(value, 1)) < 1, "I", Left(value, 1))
                If (String.Equals(Me._CarrTarTariffType, strVal) = False) Then
                    Me._CarrTarTariffType = strVal
                    Me.SendPropertyChanged("CarrTarTariffType")
                End If
            End Set
        End Property

        Private _CarrTarDefWgt As Boolean = False
        <DataMember()> _
        Public Property CarrTarDefWgt() As Boolean
            Get
                Return _CarrTarDefWgt
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarDefWgt <> value) Then
                    Me._CarrTarDefWgt = value
                    Me.SendPropertyChanged("CarrTarDefWgt")
                End If
            End Set
        End Property

        Private _CarrTarModUser As String = ""
        <DataMember()> _
        Public Property CarrTarModUser() As String
            Get
                Return Left(_CarrTarModUser, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarModUser, value) = False) Then
                    Me._CarrTarModUser = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarModUser")
                End If
            End Set
        End Property

        Private _CarrTarModDate As Date = DateTime.Now
        <DataMember()> _
        Public Property CarrTarModDate() As Date
            Get
                Return _CarrTarModDate
            End Get
            Set(ByVal value As Date)
                If (Me._CarrTarModDate.Equals(value) = False) Then
                    Me._CarrTarModDate = value
                    Me.SendPropertyChanged("CarrTarModDate")
                End If
            End Set
        End Property

        Private _CarrTarUpdated As Byte()
        <DataMember()> _
        Public Property CarrTarUpdated() As Byte()
            Get
                Return _CarrTarUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTarUpdated = value
            End Set
        End Property

        Private _CarrTarEffDateFrom As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarEffDateFrom() As System.Nullable(Of Date)
            Get
                Return _CarrTarEffDateFrom
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._CarrTarEffDateFrom.Equals(value) = False) Then
                    Me._CarrTarEffDateFrom = value
                    Me.SendPropertyChanged("CarrTarEffDateFrom")
                End If
            End Set
        End Property

        Private _CarrTarEffDateTo As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarEffDateTo() As System.Nullable(Of Date)
            Get
                Return _CarrTarEffDateTo
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._CarrTarEffDateTo.Equals(value) = False) Then
                    Me._CarrTarEffDateTo = value
                    Me.SendPropertyChanged("CarrTarEffDateTo")
                End If
            End Set
        End Property

        Private _CarrTarAutoAssignPro As Boolean = False
        <DataMember()> _
        Public Property CarrTarAutoAssignPro() As Boolean
            Get
                Return _CarrTarAutoAssignPro
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarAutoAssignPro <> value) Then
                    Me._CarrTarAutoAssignPro = value
                    Me.SendPropertyChanged("CarrTarAutoAssignPro")
                End If
            End Set
        End Property

        Private _CarrTarTariffTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarTariffTypeControl() As Integer
            Get
                Return _CarrTarTariffTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarTariffTypeControl <> value) Then
                    Me._CarrTarTariffTypeControl = value
                    Me.SendPropertyChanged("CarrTarTariffTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarTariffModeTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarTariffModeTypeControl() As Integer
            Get
                Return _CarrTarTariffModeTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarTariffModeTypeControl <> value) Then
                    Me._CarrTarTariffModeTypeControl = value
                    Me.SendPropertyChanged("CarrTarTariffModeTypeControl")
                End If
            End Set
        End Property

        Private _CarrTarName As String = ""
        <DataMember()> _
        Public Property CarrTarName() As String
            Get
                Return Left(_CarrTarName, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarName, value) = False) Then
                    Me._CarrTarName = Left(value, 50)
                    Me.SendPropertyChanged("CarrTarName")
                End If
            End Set
        End Property

        Private _CarrTarRevisionNumber As Integer = 0
        <DataMember()> _
        Public Property CarrTarRevisionNumber() As Integer
            Get
                Return _CarrTarRevisionNumber
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarRevisionNumber <> value) Then
                    Me._CarrTarRevisionNumber = value
                    Me.SendPropertyChanged("CarrTarRevisionNumber")
                End If
            End Set
        End Property

        Private _CarrTarApproved As Boolean = False
        <DataMember()> _
        Public Property CarrTarApproved() As Boolean
            Get
                Return _CarrTarApproved
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarApproved <> value) Then
                    Me._CarrTarApproved = value
                    Me.SendPropertyChanged("CarrTarApproved")
                End If
            End Set
        End Property

        Private _CarrTarApprovedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarApprovedDate() As System.Nullable(Of Date)
            Get
                Return _CarrTarApprovedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._CarrTarApprovedDate.Equals(value) = False) Then
                    Me._CarrTarApprovedDate = value
                    Me.SendPropertyChanged("CarrTarApprovedDate")
                End If
            End Set
        End Property

        Private _CarrTarApprovedBy As String = ""
        <DataMember()> _
        Public Property CarrTarApprovedBy() As String
            Get
                Return Left(_CarrTarApprovedBy, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarApprovedBy, value) = False) Then
                    Me._CarrTarApprovedBy = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarApprovedBy")
                End If
            End Set
        End Property

        Private _CarrTarRejected As Boolean = False
        <DataMember()> _
        Public Property CarrTarRejected() As Boolean
            Get
                Return _CarrTarRejected
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarRejected <> value) Then
                    Me._CarrTarRejected = value
                    Me.SendPropertyChanged("_CarrTarRejected")
                End If
            End Set
        End Property

        Private _CarrTarRejectedBy As String = ""
        <DataMember()> _
        Public Property CarrTarRejectedBy() As String
            Get
                Return Left(_CarrTarRejectedBy, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarRejectedBy, value) = False) Then
                    Me._CarrTarRejectedBy = Left(value, 100)
                    Me.SendPropertyChanged("CarrTarRejectedBy")
                End If
            End Set
        End Property

        Private _CarrTarRejectedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarRejectedDate() As System.Nullable(Of Date)
            Get
                Return _CarrTarRejectedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._CarrTarRejectedDate.Equals(value) = False) Then
                    Me._CarrTarRejectedDate = value
                    Me.SendPropertyChanged("CarrTarRejectedDate")
                End If
            End Set
        End Property

        Private _CarrTarOutbound As Boolean = True
        <DataMember()> _
        Public Property CarrTarOutbound() As Boolean
            Get
                Return _CarrTarOutbound
            End Get
            Set(ByVal value As Boolean)
                If (Me._CarrTarOutbound <> value) Then
                    Me._CarrTarOutbound = value
                    Me.SendPropertyChanged("CarrTarOutbound")
                End If
            End Set
        End Property

        Private _CarrTarAgentControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarAgentControl() As Integer
            Get
                Return _CarrTarAgentControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarAgentControl <> value) Then
                    Me._CarrTarAgentControl = value
                    Me.SendPropertyChanged("CarrTarAgentControl")
                End If
            End Set
        End Property

        Private _CarrTarIssuedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTarIssuedDate() As System.Nullable(Of Date)
            Get
                Return _CarrTarIssuedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._CarrTarIssuedDate.Equals(value) = False) Then
                    Me._CarrTarIssuedDate = value
                    Me.SendPropertyChanged("CarrTarIssuedDate")
                End If
            End Set
        End Property

        Private _CarrTarPreCloneControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarPreCloneControl() As Integer
            Get
                Return _CarrTarPreCloneControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarPreCloneControl <> value) Then
                    Me._CarrTarPreCloneControl = value
                    Me.SendPropertyChanged("CarrTarPreCloneControl")
                End If
            End Set
        End Property

        Private _CarrTarUser1 As String = ""
        <DataMember()> _
        Public Property CarrTarUser1() As String
            Get
                Return Left(_CarrTarUser1, 4000)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarUser1, value) = False) Then
                    Me._CarrTarUser1 = Left(value, 4000)
                    Me.SendPropertyChanged("CarrTarUser1")
                End If
            End Set
        End Property

        Private _CarrTarUser2 As String = ""
        <DataMember()> _
        Public Property CarrTarUser2() As String
            Get
                Return Left(_CarrTarUser2, 4000)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarUser2, value) = False) Then
                    Me._CarrTarUser2 = Left(value, 4000)
                    Me.SendPropertyChanged("CarrTarUser2")
                End If
            End Set
        End Property

        Private _CarrTarUser3 As String = ""
        <DataMember()> _
        Public Property CarrTarUser3() As String
            Get
                Return Left(_CarrTarUser3, 4000)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarUser3, value) = False) Then
                    Me._CarrTarUser3 = Left(value, 4000)
                    Me.SendPropertyChanged("CarrTarUser3")
                End If
            End Set
        End Property

        Private _CarrTarUser4 As String = ""
        <DataMember()> _
        Public Property CarrTarUser4() As String
            Get
                Return Left(_CarrTarUser4, 4000)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CarrTarUser4, value) = False) Then
                    Me._CarrTarUser4 = Left(value, 4000)
                    Me.SendPropertyChanged("CarrTarUser4")
                End If
            End Set
        End Property

        Private _CarrTarWillDriveSunday As Boolean = False
        <DataMember()> _
        Public Property CarrTarWillDriveSunday() As Boolean
            Get
                Return _CarrTarWillDriveSunday
            End Get
            Set(ByVal value As Boolean)
                _CarrTarWillDriveSunday = value
            End Set
        End Property

        Private _CarrTarWillDriveSaturday As Boolean = False
        <DataMember()> _
        Public Property CarrTarWillDriveSaturday() As Boolean
            Get
                Return _CarrTarWillDriveSaturday
            End Get
            Set(ByVal value As Boolean)
                _CarrTarWillDriveSaturday = value
            End Set
        End Property

        Private _DisplayName As String = ""
        <DataMember()> _
        Public Property ContractDisplayName() As String
            Get
                Return Me.CarrTarName & " " & Me.CarrTarID & " " & Me.CarrTarRevisionNumber
            End Get
            Set(ByVal value As String)
                _DisplayName = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrTarContract
            instance = DirectCast(MemberwiseClone(), CarrTarContract)
            Return instance
        End Function

#End Region

    End Class
End Namespace
