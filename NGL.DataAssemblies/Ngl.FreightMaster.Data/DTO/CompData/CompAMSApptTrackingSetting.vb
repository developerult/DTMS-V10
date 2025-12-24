Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CompAMSApptTrackingSetting
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CompAMSApptTrackingSettingControl As Integer = 0
        <DataMember()> _
        Public Property CompAMSApptTrackingSettingControl() As Integer
            Get
                Return _CompAMSApptTrackingSettingControl
            End Get
            Set(ByVal value As Integer)
                _CompAMSApptTrackingSettingControl = value
            End Set
        End Property

        Private _CompAMSApptTrackingSettingCompControl As Integer = 0
        <DataMember()> _
        Public Property CompAMSApptTrackingSettingCompControl() As Integer
            Get
                Return _CompAMSApptTrackingSettingCompControl
            End Get
            Set(ByVal value As Integer)
                _CompAMSApptTrackingSettingCompControl = value
            End Set
        End Property



        Private _CompAMSApptTrackingSettingName As String = ""
        <DataMember()> _
        Public Property CompAMSApptTrackingSettingName() As String
            Get
                Return Left(_CompAMSApptTrackingSettingName, 20)
            End Get
            Set(ByVal value As String)
                _CompAMSApptTrackingSettingName = Left(value, 20)
            End Set
        End Property

        Private _CompAMSApptTrackingSettingDesc As String = ""
        <DataMember()> _
        Public Property CompAMSApptTrackingSettingDesc() As String
            Get
                Return Left(_CompAMSApptTrackingSettingDesc, 255)
            End Get
            Set(ByVal value As String)
                _CompAMSApptTrackingSettingDesc = Left(value, 255)
            End Set
        End Property

        Private _CompAMSApptTrackingSettingModUser As String = ""
        <DataMember()> _
        Public Property CompAMSApptTrackingSettingModUser() As String
            Get
                Return Left(_CompAMSApptTrackingSettingModUser, 100)
            End Get
            Set(ByVal value As String)
                _CompAMSApptTrackingSettingModUser = Left(value, 100)
            End Set
        End Property

        Private _CompAMSApptTrackingSettingModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CompAMSApptTrackingSettingModDate() As System.Nullable(Of Date)
            Get
                Return _CompAMSApptTrackingSettingModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CompAMSApptTrackingSettingModDate = value
            End Set
        End Property

        Private _CompAMSApptTrackingSettingUpdated As Byte()
        <DataMember()> _
        Public Property CompAMSApptTrackingSettingUpdated() As Byte()
            Get
                Return _CompAMSApptTrackingSettingUpdated
            End Get
            Set(ByVal value As Byte())
                _CompAMSApptTrackingSettingUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CompAMSApptTrackingSetting
            instance = DirectCast(MemberwiseClone(), CompAMSApptTrackingSetting)
            Return instance
        End Function

#End Region

    End Class
End Namespace