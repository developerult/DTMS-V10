Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblBatchProcessRunning
        Inherits DTOBaseClass


#Region " Data Members"

        Private _BPRControl As Integer = 0
        <DataMember()> _
        Public Property BPRControl() As Integer
            Get
                Return _BPRControl
            End Get
            Set(ByVal value As Integer)
                _BPRControl = value
            End Set
        End Property

        Private _BPRUserName As String = ""
        <DataMember()> _
        Public Property BPRUserName() As String
            Get
                Return Left(_BPRUserName, 100)
            End Get
            Set(ByVal value As String)
                _BPRUserName = Left(value, 100)
            End Set
        End Property

        Private _BPRProcessName As String = ""
        <DataMember()> _
        Public Property BPRProcessName() As String
            Get
                Return Left(_BPRProcessName, 255)
            End Get
            Set(ByVal value As String)
                _BPRProcessName = Left(value, 255)
            End Set
        End Property

        Private _BPRStartDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BPRStartDate() As System.Nullable(Of Date)
            Get
                Return _BPRStartDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BPRStartDate = value
            End Set
        End Property

        Private _BPREndDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BPREndDate() As System.Nullable(Of Date)
            Get
                Return _BPREndDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BPREndDate = value
            End Set
        End Property

        Private _BPRRunning As Boolean = False
        <DataMember()> _
        Public Property BPRRunning() As Boolean
            Get
                Return _BPRRunning
            End Get
            Set(ByVal value As Boolean)
                _BPRRunning = value
            End Set
        End Property

        Private _BPHHadErrors As Boolean = False
        <DataMember()> _
        Public Property BPHHadErrors() As Boolean
            Get
                Return _BPHHadErrors
            End Get
            Set(ByVal value As Boolean)
                _BPHHadErrors = value
            End Set
        End Property

        Private _BPHErrMsg As String = ""
        <DataMember()> _
        Public Property BPHErrMsg() As String
            Get
                Return Left(_BPHErrMsg, 4000)
            End Get
            Set(ByVal value As String)
                _BPHErrMsg = Left(value, 4000)
            End Set
        End Property

        Private _BPHErrTitle As String = ""
        <DataMember()> _
        Public Property BPHErrTitle() As String
            Get
                Return Left(_BPHErrTitle, 500)
            End Get
            Set(ByVal value As String)
                _BPHErrTitle = Left(value, 500)
            End Set
        End Property

        Private _BPRUpdated As Byte()
        <DataMember()> _
        Public Property BPRUpdated() As Byte()
            Get
                Return _BPRUpdated
            End Get
            Set(ByVal value As Byte())
                _BPRUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblBatchProcessRunning
            instance = DirectCast(MemberwiseClone(), tblBatchProcessRunning)
            Return instance
        End Function

#End Region
    End Class
End Namespace

