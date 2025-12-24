Public Class clsIntegrationFields : Inherits List(Of clsIntegrationField)

    Public Function AddItem( _
        ByVal Key As String, _
        ByVal Name As String, _
        ByVal DataType As clsIntegrationField.DataTypeID, _
        ByVal Length As Integer, _
        ByVal Null As Boolean, _
        ByVal Use As Boolean, _
        ByVal Value As String) As clsIntegrationField
        'create a new object
        Dim objNewMember As clsIntegrationField
        objNewMember = New clsIntegrationField
        With objNewMember
            .Name = Name
            .DataType = DataType
            .Length = Length
            .Null = Null
            .Use = Use
            .Value = Value
            .Key = Key
        End With
        Me.Add(objNewMember)
        Return objNewMember

    End Function
End Class

Public Class clsIntegrationField

#Region " Constructors "



#End Region

#Region " Enums "

    Public Enum DataTypeID As Integer
        DateStandard = 0
        Integer32
        Integer16
        StringData
        Float
        IntegerTiny
        Bit
        Money
        Time
        DateYYDDD
        DateYYYYMMDD
        DateMMDDYY
        ImpliedDecimal
    End Enum

    Public Enum PKValue As Integer
        gcNK = 0
        gcPK
        gcFK
        gcHK
    End Enum
#End Region

#Region " Properties "

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

    Private _enmDataType As DataTypeID = DataTypeID.StringData
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
    Public Property PK() As String
        Get
            Return _enmPK
        End Get
        Set(ByVal value As String)
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

    Public ReadOnly Property DataTypeName() As String
        Get
            Select Case DataType
                Case DataTypeID.Bit
                    Return "Boolean True/False"
                Case DataTypeID.DateStandard
                    Return "Date Time"
                Case DataTypeID.Float
                    Return "Floating Point Decimal Number"
                Case DataTypeID.Integer32
                    Return "Integer Number"
                Case DataTypeID.Money
                    Return "Money or Currency"
                Case DataTypeID.Integer16
                    Return "Small Integer Number"
                Case DataTypeID.StringData
                    Return "Text Data"
                Case DataTypeID.Time
                    Return "Time Value"
                Case DataTypeID.IntegerTiny
                    Return "Tiny Integer Number"
                Case Else
                    Return "Undefined Data Type"
            End Select

        End Get
    End Property

#End Region

#Region " Protected Methods "



#End Region

#Region " Public Methods "

#End Region

End Class

