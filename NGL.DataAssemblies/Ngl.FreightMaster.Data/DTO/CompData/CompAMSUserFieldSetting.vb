Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CompAMSUserFieldSetting
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CompAMSUserFieldSettingControl As Integer = 0
        <DataMember()> _
        Public Property CompAMSUserFieldSettingControl() As Integer
            Get
                Return _CompAMSUserFieldSettingControl
            End Get
            Set(ByVal value As Integer)
                _CompAMSUserFieldSettingControl = value
            End Set
        End Property

        Private _CompAMSUserFieldSettingCompControl As Integer = 0
        <DataMember()> _
        Public Property CompAMSUserFieldSettingCompControl() As Integer
            Get
                Return _CompAMSUserFieldSettingCompControl
            End Get
            Set(ByVal value As Integer)
                _CompAMSUserFieldSettingCompControl = value
            End Set
        End Property

        Private _CompAMSUserFieldSettingFieldName As String = ""
        <DataMember()> _
        Public Property CompAMSUserFieldSettingFieldName() As String
            Get
                Return Left(_CompAMSUserFieldSettingFieldName, 20)
            End Get
            Set(ByVal value As String)
                _CompAMSUserFieldSettingFieldName = Left(value, 20)
            End Set
        End Property

        Private _CompAMSUserFieldSettingFieldDesc As String = ""
        <DataMember()> _
        Public Property CompAMSUserFieldSettingFieldDesc() As String
            Get
                Return Left(_CompAMSUserFieldSettingFieldDesc, 255)
            End Get
            Set(ByVal value As String)
                _CompAMSUserFieldSettingFieldDesc = Left(value, 255)
            End Set
        End Property

        Private _CompAMSUserFieldSettingModUser As String = ""
        <DataMember()> _
        Public Property CompAMSUserFieldSettingModUser() As String
            Get
                Return Left(_CompAMSUserFieldSettingModUser, 100)
            End Get
            Set(ByVal value As String)
                _CompAMSUserFieldSettingModUser = Left(value, 100)
            End Set
        End Property

        Private _CompAMSUserFieldSettingModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CompAMSUserFieldSettingModDate() As System.Nullable(Of Date)
            Get
                Return _CompAMSUserFieldSettingModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CompAMSUserFieldSettingModDate = value
            End Set
        End Property

        Private _CompAMSUserFieldSettingUpdated As Byte()
        <DataMember()> _
        Public Property CompAMSUserFieldSettingUpdated() As Byte()
            Get
                Return _CompAMSUserFieldSettingUpdated
            End Get
            Set(ByVal value As Byte())
                _CompAMSUserFieldSettingUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CompAMSUserFieldSetting
            instance = DirectCast(MemberwiseClone(), CompAMSUserFieldSetting)
            Return instance
        End Function

#End Region

    End Class
End Namespace