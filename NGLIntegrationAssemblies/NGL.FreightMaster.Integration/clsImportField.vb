<Serializable()> _
Public Class clsImportField

    Public Enum DataTypeID As Integer
        gcvdtDate = 0
        gcvdtLongInt
        gcvdtSmallInt
        gcvdtString
        gcvdtFloat
        gcvdtTinyInt
        gcvdtBit
        gcvdtMoney
        gcvdtTime
    End Enum

    Public Enum PKValue As Integer
        gcNK = 0
        gcPK
        gcFK
        gcHK
        gcIgnore 'typically used for old data fields that are no longer supported
    End Enum

    ''' <summary>
    ''' returns the current value of this field without any single quotes at the beginning and the end of the string
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ValueNoQuotes() As String
        Get
            Return Ngl.Core.Utility.DataTransformation.stripQuotes(Value)
        End Get
    End Property


    Private _strValue As String = ""
    Public Property Value() As String
        Get
            Return _strValue
        End Get
        Set(ByVal value As String)
            _strValue = value
        End Set
    End Property

    Private _strName As String = ""
    Public Property Name() As String
        Get
            Return _strName
        End Get
        Set(ByVal value As String)
            _strName = value
        End Set
    End Property

    Private _enmDataType As DataTypeID = DataTypeID.gcvdtString
    Public Property DataType() As DataTypeID
        Get
            Return _enmDataType
        End Get
        Set(ByVal value As DataTypeID)
            _enmDataType = value
        End Set
    End Property

    Private _intLength As Integer = 0
    Public Property Length() As Integer
        Get
            Return _intLength
        End Get
        Set(ByVal value As Integer)
            _intLength = value
        End Set
    End Property

    Private _blnNull As Boolean
    Public Property Null() As Boolean
        Get
            Return _blnNull
        End Get
        Set(ByVal value As Boolean)
            _blnNull = value
        End Set
    End Property

    Private _enmPK As PKValue = PKValue.gcNK
    Public Property PK() As Integer
        Get
            Return _enmPK
        End Get
        Set(ByVal value As Integer)
            _enmPK = value
        End Set
    End Property

    Private _intFK_INDEX As Integer = 0
    Public Property FK_Index() As Integer
        Get
            Return _intFK_INDEX
        End Get
        Set(ByVal value As Integer)
            _intFK_INDEX = value
        End Set
    End Property

    Private _stFK_Key As String = ""
    Public Property FK_Key() As String
        Get
            Return _stFK_Key
        End Get
        Set(ByVal value As String)
            _stFK_Key = value
        End Set
    End Property

    Private _strPARENT_FIELD As String = ""
    Public Property Parent_Field() As String
        Get
            Return _strPARENT_FIELD
        End Get
        Set(ByVal value As String)
            _strPARENT_FIELD = value
        End Set
    End Property

    Private _blnUse As Boolean = False
    Public Property Use() As Boolean
        Get
            Return _blnUse
        End Get
        Set(ByVal value As Boolean)
            _blnUse = value
        End Set
    End Property

    Private _strKey As String = ""
    Public Property Key() As String
        Get
            Return _strKey
        End Get
        Set(ByVal value As String)
            _strKey = value
        End Set
    End Property


End Class

