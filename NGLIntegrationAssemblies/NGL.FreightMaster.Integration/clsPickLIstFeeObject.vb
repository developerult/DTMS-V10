Public Class clsPickListFeeObject70 : Inherits clsImportDataBase

    Private _PLControl As Int64 = 0
    Public Property PLControl() As Int64
        Get
            Return _PLControl
        End Get
        Set(ByVal value As Int64)
            _PLControl = value
        End Set
    End Property

    Private _AccessorialCode As Integer = 0
    Public Property AccessorialCode() As Integer
        Get
            Return _AccessorialCode
        End Get
        Set(ByVal value As Integer)
            _AccessorialCode = value
        End Set
    End Property

    Private _AccessorialName As String = ""
    Public Property AccessorialName() As String
        Get
            Return Left(_AccessorialName, 50)
        End Get
        Set(ByVal value As String)
            _AccessorialName = Left(value, 50)
        End Set
    End Property

    Private _AccessorialDescription As String = ""
    Public Property AccessorialDescription() As String
        Get
            Return Left(_AccessorialDescription, 255)
        End Get
        Set(ByVal value As String)
            _AccessorialDescription = Left(value, 255)
        End Set
    End Property

    Private _AccessorialCaption As String = ""
    Public Property AccessorialCaption() As String
        Get
            Return Left(_AccessorialCaption, 50)
        End Get
        Set(ByVal value As String)
            _AccessorialCaption = Left(value, 50)
        End Set
    End Property

    Private _AccessorialAlphaCode As String
    Public Property AccessorialAlphaCode() As String
        Get
            Return Left(_AccessorialAlphaCode, 20)
        End Get
        Set(ByVal value As String)
            _AccessorialAlphaCode = Left(value, 20)
        End Set
    End Property

    Private _AccessorialEDICode As String
    Public Property AccessorialEDICode() As String
        Get
            Return Left(_AccessorialEDICode, 20)
        End Get
        Set(ByVal value As String)
            _AccessorialEDICode = Left(value, 20)
        End Set
    End Property

    Private _AccessorialTaxable As Boolean = False
    Public Property AccessorialTaxable() As Boolean
        Get
            Return _AccessorialTaxable
        End Get
        Set(ByVal value As Boolean)
            _AccessorialTaxable = value
        End Set
    End Property

    Private _AccessorialTaxSortOrder As Integer = 0
    Public Property AccessorialTaxSortOrder() As Integer
        Get
            Return _AccessorialTaxSortOrder
        End Get
        Set(ByVal value As Integer)
            _AccessorialTaxSortOrder = value
        End Set
    End Property


    Private _AccessorialIsTax As Boolean = False
    Public Property AccessorialIsTax() As Boolean
        Get
            Return _AccessorialIsTax
        End Get
        Set(ByVal value As Boolean)
            _AccessorialIsTax = value
        End Set
    End Property

    Private _AccessorialBOLText As String
    Public Property AccessorialBOLText() As String
        Get
            Return Left(_AccessorialBOLText, 4000)
        End Get
        Set(ByVal value As String)
            _AccessorialBOLText = Left(value, 4000)
        End Set
    End Property

    Private _AccessorialBOLPlacement As String
    Public Property AccessorialBOLPlacement() As String
        Get
            Return Left(_AccessorialBOLPlacement, 100)
        End Get
        Set(ByVal value As String)
            _AccessorialBOLPlacement = Left(value, 100)
        End Set
    End Property

    Private _AccessorialAmount As Double
    Public Property AccessorialAmount() As Double
        Get
            Return _AccessorialAmount
        End Get
        Set(ByVal value As Double)
            _AccessorialAmount = value
        End Set
    End Property

    Private _AccessorialGroupType As String = ""
    Public Property AccessorialGroupType() As String
        Get
            Return _AccessorialGroupType
        End Get
        Set(ByVal value As String)
            _AccessorialGroupType = value
        End Set
    End Property

End Class

''' <summary>
''' Pick Worksheet Fees
''' </summary>
''' <remarks>
''' Created by RHR v-8.2.0.117 7/17/2019
'''     there are currently no changes to the fees object only the item details
'''     for consistency we create matching object names with the 80 tag
''' </remarks>
Public Class clsPickListFeeObject80 : Inherits clsImportDataBase

    Private _PLControl As Int64 = 0
    Public Property PLControl() As Int64
        Get
            Return _PLControl
        End Get
        Set(ByVal value As Int64)
            _PLControl = value
        End Set
    End Property

    Private _AccessorialCode As Integer = 0
    Public Property AccessorialCode() As Integer
        Get
            Return _AccessorialCode
        End Get
        Set(ByVal value As Integer)
            _AccessorialCode = value
        End Set
    End Property

    Private _AccessorialName As String = ""
    Public Property AccessorialName() As String
        Get
            Return Left(_AccessorialName, 50)
        End Get
        Set(ByVal value As String)
            _AccessorialName = Left(value, 50)
        End Set
    End Property

    Private _AccessorialDescription As String = ""
    Public Property AccessorialDescription() As String
        Get
            Return Left(_AccessorialDescription, 255)
        End Get
        Set(ByVal value As String)
            _AccessorialDescription = Left(value, 255)
        End Set
    End Property

    Private _AccessorialCaption As String = ""
    Public Property AccessorialCaption() As String
        Get
            Return Left(_AccessorialCaption, 50)
        End Get
        Set(ByVal value As String)
            _AccessorialCaption = Left(value, 50)
        End Set
    End Property

    Private _AccessorialAlphaCode As String
    Public Property AccessorialAlphaCode() As String
        Get
            Return Left(_AccessorialAlphaCode, 20)
        End Get
        Set(ByVal value As String)
            _AccessorialAlphaCode = Left(value, 20)
        End Set
    End Property

    Private _AccessorialEDICode As String
    Public Property AccessorialEDICode() As String
        Get
            Return Left(_AccessorialEDICode, 20)
        End Get
        Set(ByVal value As String)
            _AccessorialEDICode = Left(value, 20)
        End Set
    End Property

    Private _AccessorialTaxable As Boolean = False
    Public Property AccessorialTaxable() As Boolean
        Get
            Return _AccessorialTaxable
        End Get
        Set(ByVal value As Boolean)
            _AccessorialTaxable = value
        End Set
    End Property

    Private _AccessorialTaxSortOrder As Integer = 0
    Public Property AccessorialTaxSortOrder() As Integer
        Get
            Return _AccessorialTaxSortOrder
        End Get
        Set(ByVal value As Integer)
            _AccessorialTaxSortOrder = value
        End Set
    End Property


    Private _AccessorialIsTax As Boolean = False
    Public Property AccessorialIsTax() As Boolean
        Get
            Return _AccessorialIsTax
        End Get
        Set(ByVal value As Boolean)
            _AccessorialIsTax = value
        End Set
    End Property

    Private _AccessorialBOLText As String
    Public Property AccessorialBOLText() As String
        Get
            Return Left(_AccessorialBOLText, 4000)
        End Get
        Set(ByVal value As String)
            _AccessorialBOLText = Left(value, 4000)
        End Set
    End Property

    Private _AccessorialBOLPlacement As String
    Public Property AccessorialBOLPlacement() As String
        Get
            Return Left(_AccessorialBOLPlacement, 100)
        End Get
        Set(ByVal value As String)
            _AccessorialBOLPlacement = Left(value, 100)
        End Set
    End Property

    Private _AccessorialAmount As Double
    Public Property AccessorialAmount() As Double
        Get
            Return _AccessorialAmount
        End Get
        Set(ByVal value As Double)
            _AccessorialAmount = value
        End Set
    End Property

    Private _AccessorialGroupType As String = ""
    Public Property AccessorialGroupType() As String
        Get
            Return _AccessorialGroupType
        End Get
        Set(ByVal value As String)
            _AccessorialGroupType = value
        End Set
    End Property

End Class

''' <summary>
''' Pick Worksheet Fees
''' </summary>
''' <remarks>
''' Created by RHR v-8.5.0.002 12/13/2021
'''   replaces the 80 version Of the data new fields pending
''' </remarks>
Public Class clsPickListFeeObject85 : Inherits clsImportDataBase
    Private _PLControl As Int64 = 0
    Public Property PLControl() As Int64
        Get
            Return _PLControl
        End Get
        Set(ByVal value As Int64)
            _PLControl = value
        End Set
    End Property

    Private _AccessorialCode As Integer = 0
    Public Property AccessorialCode() As Integer
        Get
            Return _AccessorialCode
        End Get
        Set(ByVal value As Integer)
            _AccessorialCode = value
        End Set
    End Property

    Private _AccessorialName As String = ""
    Public Property AccessorialName() As String
        Get
            Return Left(_AccessorialName, 50)
        End Get
        Set(ByVal value As String)
            _AccessorialName = Left(value, 50)
        End Set
    End Property

    Private _AccessorialDescription As String = ""
    Public Property AccessorialDescription() As String
        Get
            Return Left(_AccessorialDescription, 255)
        End Get
        Set(ByVal value As String)
            _AccessorialDescription = Left(value, 255)
        End Set
    End Property

    Private _AccessorialCaption As String = ""
    Public Property AccessorialCaption() As String
        Get
            Return Left(_AccessorialCaption, 50)
        End Get
        Set(ByVal value As String)
            _AccessorialCaption = Left(value, 50)
        End Set
    End Property

    Private _AccessorialAlphaCode As String
    Public Property AccessorialAlphaCode() As String
        Get
            Return Left(_AccessorialAlphaCode, 20)
        End Get
        Set(ByVal value As String)
            _AccessorialAlphaCode = Left(value, 20)
        End Set
    End Property

    Private _AccessorialEDICode As String
    Public Property AccessorialEDICode() As String
        Get
            Return Left(_AccessorialEDICode, 20)
        End Get
        Set(ByVal value As String)
            _AccessorialEDICode = Left(value, 20)
        End Set
    End Property

    Private _AccessorialTaxable As Boolean = False
    Public Property AccessorialTaxable() As Boolean
        Get
            Return _AccessorialTaxable
        End Get
        Set(ByVal value As Boolean)
            _AccessorialTaxable = value
        End Set
    End Property

    Private _AccessorialTaxSortOrder As Integer = 0
    Public Property AccessorialTaxSortOrder() As Integer
        Get
            Return _AccessorialTaxSortOrder
        End Get
        Set(ByVal value As Integer)
            _AccessorialTaxSortOrder = value
        End Set
    End Property


    Private _AccessorialIsTax As Boolean = False
    Public Property AccessorialIsTax() As Boolean
        Get
            Return _AccessorialIsTax
        End Get
        Set(ByVal value As Boolean)
            _AccessorialIsTax = value
        End Set
    End Property

    Private _AccessorialBOLText As String
    Public Property AccessorialBOLText() As String
        Get
            Return Left(_AccessorialBOLText, 4000)
        End Get
        Set(ByVal value As String)
            _AccessorialBOLText = Left(value, 4000)
        End Set
    End Property

    Private _AccessorialBOLPlacement As String
    Public Property AccessorialBOLPlacement() As String
        Get
            Return Left(_AccessorialBOLPlacement, 100)
        End Get
        Set(ByVal value As String)
            _AccessorialBOLPlacement = Left(value, 100)
        End Set
    End Property

    Private _AccessorialAmount As Double
    Public Property AccessorialAmount() As Double
        Get
            Return _AccessorialAmount
        End Get
        Set(ByVal value As Double)
            _AccessorialAmount = value
        End Set
    End Property

    Private _AccessorialGroupType As String = ""
    Public Property AccessorialGroupType() As String
        Get
            Return _AccessorialGroupType
        End Get
        Set(ByVal value As String)
            _AccessorialGroupType = value
        End Set
    End Property


End Class