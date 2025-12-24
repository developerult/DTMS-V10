Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class DynamicsTMSIntegrationLog
        Inherits DTOBaseClass


#Region " Data Members"

        Private _DTMSINTLogControl As Int64 = 0
        <DataMember()> _
        Public Property DTMSINTLogControl() As Int64
            Get
                Return _DTMSINTLogControl
            End Get
            Set(ByVal value As Int64)
                _DTMSINTLogControl = value
            End Set
        End Property


        Private _DTMSINTLogLegalEntity As String = ""
        <DataMember()> _
        Public Property DTMSINTLogLegalEntity() As String
            Get
                Return Left(_DTMSINTLogLegalEntity, 100)
            End Get
            Set(ByVal value As String)
                _DTMSINTLogLegalEntity = Left(value, 100)
            End Set
        End Property

        Private _DTMSINTLogTaskName As String = ""
        <DataMember()> _
        Public Property DTMSINTLogTaskName() As String
            Get
                Return Left(_DTMSINTLogTaskName, 100)
            End Get
            Set(ByVal value As String)
                _DTMSINTLogTaskName = Left(value, 100)
            End Set
        End Property

        Private _DTMSINTLogTaskDesc As String = ""
        <DataMember()> _
        Public Property DTMSINTLogTaskDesc() As String
            Get
                Return Left(_DTMSINTLogTaskDesc, 4000)
            End Get
            Set(ByVal value As String)
                _DTMSINTLogTaskDesc = Left(value, 4000)
            End Set
        End Property

        Private _DTMSINTLogData As String = ""
        <DataMember()> _
        Public Property DTMSINTLogData() As String
            Get
                Return _DTMSINTLogData
            End Get
            Set(ByVal value As String)
                _DTMSINTLogData = value
            End Set
        End Property

        Private _DTMSINTLogTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property DTMSINTLogTime() As System.Nullable(Of Date)
            Get
                Return _DTMSINTLogTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _DTMSINTLogTime = value
            End Set
        End Property

        Private _DTMSINTLogUser As String = ""
        <DataMember()> _
        Public Property DTMSINTLogUser() As String
            Get
                Return Left(_DTMSINTLogUser, 100)
            End Get
            Set(ByVal value As String)
                _DTMSINTLogUser = Left(value, 100)
            End Set
        End Property

        Private _DTMSINTLogSource As String = ""
        <DataMember()> _
        Public Property DTMSINTLogSource() As String
            Get
                Return Left(_DTMSINTLogSource, 50)
            End Get
            Set(ByVal value As String)
                _DTMSINTLogSource = Left(value, 50)
            End Set
        End Property

        Private _DTMSINTLogMessage As String = ""
        <DataMember()> _
        Public Property DTMSINTLogMessage() As String
            Get
                Return _DTMSINTLogMessage
            End Get
            Set(ByVal value As String)
                _DTMSINTLogMessage = value
            End Set
        End Property


        Private _DTMSINTLogModUser As String = ""
        <DataMember()> _
        Public Property DTMSINTLogModUser() As String
            Get
                Return Left(_DTMSINTLogModUser, 100)
            End Get
            Set(ByVal value As String)
                _DTMSINTLogModUser = Left(value, 100)
            End Set
        End Property

        Private _DTMSINTLogModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property DTMSINTLogModDate() As System.Nullable(Of Date)
            Get
                Return _DTMSINTLogModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _DTMSINTLogModDate = value
            End Set
        End Property

        Private _DTMSINTLogUpdated As Byte()
        <DataMember()> _
        Public Property DTMSINTLogUpdated() As Byte()
            Get
                Return _DTMSINTLogUpdated
            End Get
            Set(ByVal value As Byte())
                _DTMSINTLogUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New DynamicsTMSIntegrationLog
            instance = DirectCast(MemberwiseClone(), DynamicsTMSIntegrationLog)
            Return instance
        End Function

#End Region

    End Class
End Namespace
