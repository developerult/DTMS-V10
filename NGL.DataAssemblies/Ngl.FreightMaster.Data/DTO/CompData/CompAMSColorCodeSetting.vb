Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CompAMSColorCodeSetting
        Inherits DTOBaseClass

        Public Enum ColorCodeType
            ApptStatus
            ApptType
        End Enum

        'DEPRECIATED By LVV on 6/28/18 for v-8.3 TMS365 Scheduler
        'We now reference the lookup table tblAMSApptStatus
        ''Public Enum ApptStatusColorCodeKey
        ''    Unassigned
        ''    OutOfService
        ''    Assigned
        ''    CheckedIn
        ''    StartLoad_Unload
        ''    FinishLoad_Unload
        ''    CheckedOut
        ''    Unfinalized
        ''    Other
        ''End Enum

        'DEPRECIATED By LVV on 6/28/18 for v-8.3 TMS365 Scheduler
        'We now reference the lookup table tblAMSApptType
        ''Public Enum ApptTypeColorCodeKey
        ''    SingleLoadNoCNSOutbound
        ''    ConsolidatedIntegrityOffOutbound
        ''    ConsolidatedLoadOutbound
        ''    SingleLoadNoCNSInbound
        ''    ConsolidatedIntegrityOffInbound
        ''    ConsolidatedLoadInbound
        ''    Other
        ''End Enum


#Region " Data Members"
        Private _CompAMSColorCodeSettingControl As Integer = 0
        <DataMember()> _
        Public Property CompAMSColorCodeSettingControl As Integer
            Get
                Return _CompAMSColorCodeSettingControl
            End Get
            Set(ByVal value As Integer)
                _CompAMSColorCodeSettingControl = value
            End Set
        End Property

        Private _CompAMSColorCodeSettingCompControl As Integer = 0
        <DataMember()> _
        Public Property CompAMSColorCodeSettingCompControl As Integer
            Get
                Return _CompAMSColorCodeSettingCompControl
            End Get
            Set(ByVal value As Integer)
                _CompAMSColorCodeSettingCompControl = value
            End Set
        End Property

        Private _CompAMSColorCodeSettingType As Integer = 0
        ''' <summary>
        ''' value is of type ColorCodeType
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DataMember()> _
        Public Property CompAMSColorCodeSettingType As Integer
            Get
                Return _CompAMSColorCodeSettingType
            End Get
            Set(ByVal value As Integer)
                _CompAMSColorCodeSettingType = value
            End Set
        End Property


        Private _CompAMSColorCodeSettingKey As Integer = 0
        ''' <summary>
        ''' value is of type ApptStatusColorCodeKey or ApptTypeColorCodeKey depending upon CompAMSColorCodeSettingType
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DataMember()> _
        Public Property CompAMSColorCodeSettingKey As Integer
            Get
                Return _CompAMSColorCodeSettingKey
            End Get
            Set(ByVal value As Integer)
                _CompAMSColorCodeSettingKey = value
            End Set
        End Property

        Private _CompAMSColorCodeSettingColorCode As String = ""
        <DataMember()> _
        Public Property CompAMSColorCodeSettingColorCode() As String
            Get
                Return Left(_CompAMSColorCodeSettingColorCode, 10)
            End Get
            Set(ByVal value As String)
                _CompAMSColorCodeSettingColorCode = Left(value, 10)
            End Set
        End Property

        Private _CompAMSColorCodeSettingFieldName As String = ""
        <DataMember()> _
        Public Property CompAMSColorCodeSettingFieldName() As String
            Get
                Return Left(_CompAMSColorCodeSettingFieldName, 20)
            End Get
            Set(ByVal value As String)
                _CompAMSColorCodeSettingFieldName = Left(value, 20)
            End Set
        End Property

        Private _CompAMSColorCodeSettingFieldDesc As String = ""
        <DataMember()> _
        Public Property CompAMSColorCodeSettingFieldDesc() As String
            Get
                Return Left(_CompAMSColorCodeSettingFieldDesc, 255)
            End Get
            Set(ByVal value As String)
                _CompAMSColorCodeSettingFieldDesc = Left(value, 255)
            End Set
        End Property

        Private _CompAMSColorCodeSettingModUser As String = ""
        <DataMember()> _
        Public Property CompAMSColorCodeSettingModUser() As String
            Get
                Return Left(_CompAMSColorCodeSettingModUser, 100)
            End Get
            Set(ByVal value As String)
                _CompAMSColorCodeSettingModUser = Left(value, 100)
            End Set
        End Property

        Private _CompAMSColorCodeSettingModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CompAMSColorCodeSettingModDate() As System.Nullable(Of Date)
            Get
                Return _CompAMSColorCodeSettingModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CompAMSColorCodeSettingModDate = value
            End Set
        End Property

        Private _CompAMSColorCodeSettingUpdated As Byte()
        <DataMember()> _
        Public Property CompAMSColorCodeSettingUpdated() As Byte()
            Get
                Return _CompAMSColorCodeSettingUpdated
            End Get
            Set(ByVal value As Byte())
                _CompAMSColorCodeSettingUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CompAMSColorCodeSetting
            instance = DirectCast(MemberwiseClone(), CompAMSColorCodeSetting)
            Return instance
        End Function

#End Region

    End Class
End Namespace