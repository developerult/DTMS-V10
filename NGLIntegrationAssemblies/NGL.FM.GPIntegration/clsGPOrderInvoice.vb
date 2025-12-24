Public Class clsGPOrderInvoice

    Sub New()
        MyBase.New()
    End Sub

    Sub New(ByVal sInvoice As String, ByVal sOrder As String)
        MyBase.New()
        InvoiceNumber = sInvoice
        OrderNumber = sOrder
    End Sub

    Sub New(ByVal sInvoice As String, ByVal sOrder As String, ByVal ts As Date, Optional ByVal iSeqNbr As Integer = 0)
        MyBase.New()
        InvoiceNumber = sInvoice
        OrderNumber = sOrder
        OrderSequenceNumber = iSeqNbr
        dex_row_ts = ts
    End Sub

    Sub New(ByVal sInvoice As String, ByVal sOrder As String, ByVal sComp As String, ByVal ts As Date, Optional ByVal iSeqNbr As Integer = 0)
        MyBase.New()
        InvoiceNumber = sInvoice
        OrderNumber = sOrder
        CompAlphaCode = sComp
        dex_row_ts = ts
        OrderSequenceNumber = iSeqNbr
    End Sub


    Private _InvoiceNumber As String
    Public Property InvoiceNumber() As String
        Get
            Return _InvoiceNumber
        End Get
        Set(ByVal value As String)
            _InvoiceNumber = value
        End Set
    End Property

    Private _OrderNumber As String
    Public Property OrderNumber() As String
        Get
            Return _OrderNumber
        End Get
        Set(ByVal value As String)
            _OrderNumber = value
        End Set
    End Property

    Private _OrderSequenceNumber As Integer
    Public Property OrderSequenceNumber() As Integer
        Get
            Return _OrderSequenceNumber
        End Get
        Set(ByVal value As Integer)
            _OrderSequenceNumber = value
        End Set
    End Property

    Private _CompAlpaCode As String
    Public Property CompAlphaCode() As String
        Get
            Return _CompAlpaCode
        End Get
        Set(ByVal value As String)
            _CompAlpaCode = value
        End Set
    End Property

    Private _CompNumber As Integer
    Public Property CompNumber() As Integer
        Get
            Return _CompNumber
        End Get
        Set(ByVal value As Integer)
            _CompNumber = value
        End Set
    End Property

    Private _CostPerPound As Double
    Public Property CostPerPound() As Double
        Get
            Return _CostPerPound
        End Get
        Set(ByVal value As Double)
            _CostPerPound = value
        End Set
    End Property

    Private _dex_row_ts As Date
    Public Property dex_row_ts() As Date
        Get
            Return _dex_row_ts
        End Get
        Set(ByVal value As Date)
            _dex_row_ts = value
        End Set
    End Property

End Class
