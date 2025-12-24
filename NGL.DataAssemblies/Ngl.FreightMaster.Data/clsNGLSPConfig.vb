Public Class clsNGLSPConfig

    Private _blnTwoWay As Boolean = True
    Public Property blnTwoWay As Boolean
        Get
            Return _blnTwoWay
        End Get
        Set(ByVal value As Boolean)
            _blnTwoWay = value
        End Set
    End Property

    Private _intMaxRetry As Integer = 1
    Public Property intMaxRetry As Integer
        Get
            Return _intMaxRetry
        End Get
        Set(ByVal value As Integer)
            _intMaxRetry = value
        End Set
    End Property

    Private _strBatchName As String
    Public Property strBatchName As String
        Get
            Return _strBatchName
        End Get
        Set(ByVal value As String)
            _strBatchName = value
        End Set
    End Property

    Private _strProcName As String
    Public Property strProcName As String
        Get
            Return _strProcName
        End Get
        Set(ByVal value As String)
            _strProcName = value
        End Set
    End Property
    Private _oCmd As System.Data.SqlClient.SqlCommand
    Public Property oCmd As System.Data.SqlClient.SqlCommand
        Get
            Return _oCmd
        End Get
        Set(ByVal value As System.Data.SqlClient.SqlCommand)
            _oCmd = value
        End Set
    End Property

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal BatchName As String, ByVal ProcName As String, Optional ByVal TwoWay As Boolean = True, Optional ByVal CMD As System.Data.SqlClient.SqlCommand = Nothing)
        MyBase.New()
        Me.strBatchName = BatchName
        Me.strProcName = ProcName
        Me.blnTwoWay = TwoWay
        If CMD Is Nothing Then CMD = New System.Data.SqlClient.SqlCommand
        Me.oCmd = CMD
    End Sub

    Public Sub New(ByVal CMD As System.Data.SqlClient.SqlCommand, ByVal ProcName As String, ByVal MaxRetry As Integer)
        MyBase.New()
        Me.strProcName = ProcName
        If CMD Is Nothing Then CMD = New System.Data.SqlClient.SqlCommand
        Me.oCmd = CMD
        Me.intMaxRetry = MaxRetry
    End Sub

End Class
