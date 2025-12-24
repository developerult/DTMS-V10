<Serializable()> _
Public Class clsPayablesObject
    Public BookCarrOrderNumber As String = ""
    Public BookFinAPPayAmt As Double = 0
    Public BookFinAPActWgt As Double = 0
    Public BookFinAPCheck As Double = 0
    Public BookFinAPPayDate As String = ""
    Public BookFinAPBillNumber As String = ""
    Public BookFinAPBillInvDate As String = ""
    Public BookFinAPGLNumber As String = ""
    Public APGLDescription As String = ""
    Public CompNumber As String = ""
    Public BookProNumber As String = ""
    Public BookOrderSequence As Integer = 0

End Class

<Serializable()> _
Public Class clsPayablesObject70 : Inherits clsImportDataBase

    Private _CompLegalEntity As String = ""
    Public Property CompLegalEntity As String
        Get
            Return Left(_CompLegalEntity, 50)
        End Get
        Set(value As String)
            _CompLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _CompNumber As Integer = 0
    Public Property CompNumber() As Integer
        Get
            Return _CompNumber
        End Get
        Set(ByVal value As Integer)
            _CompNumber = value
        End Set
    End Property

    Private _CompAlphaCode As String = ""
    Public Property CompAlphaCode() As String
        Get
            Return Left(_CompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _CompAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _BookCarrOrderNumber As String = ""
    Public Property BookCarrOrderNumber() As String
        Get
            Return Left(_BookCarrOrderNumber,20)
        End Get
        Set(ByVal value As String)
            _BookCarrOrderNumber = Left(value, 20)
        End Set
    End Property

    Private _BookOrderSequence As Integer = 0
    Public Property BookOrderSequence() As Integer
        Get
            Return _BookOrderSequence
        End Get
        Set(ByVal value As Integer)
            _BookOrderSequence = value
        End Set
    End Property

    Private _BookProNumber As String = ""
    Public Property BookProNumber() As String
        Get
            Return Left(_BookProNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookProNumber = Left(value, 20)
        End Set
    End Property

    Private _BookFinAPPayAmt As Double = 0
    Public Property BookFinAPPayAmt() As Double
        Get
            Return _BookFinAPPayAmt
        End Get
        Set(ByVal value As Double)
            _BookFinAPPayAmt = value
        End Set
    End Property

    Private _BookFinAPActWgt As Double = 0
    Public Property BookFinAPActWgt() As Double
        Get
            Return _BookFinAPActWgt
        End Get
        Set(ByVal value As Double)
            _BookFinAPActWgt = value
        End Set
    End Property

    Private _BookFinAPCheck As String = ""
    Public Property BookFinAPCheck() As String
        Get
            Return Left(_BookFinAPCheck, 15)
        End Get
        Set(ByVal value As String)
            _BookFinAPCheck = Left(value, 15)
        End Set
    End Property

    Private _BookFinAPPayDate As String = ""
    Public Property BookFinAPPayDate() As String
        Get
            Return cleanDate(_BookFinAPPayDate)
        End Get
        Set(ByVal value As String)
            _BookFinAPPayDate = value
        End Set
    End Property

    Private _BookFinAPBillNumber As String = ""
    Public Property BookFinAPBillNumber() As String
        Get
            Return Left(_BookFinAPBillNumber, 50)
        End Get
        Set(ByVal value As String)
            _BookFinAPBillNumber = Left(value, 50)
        End Set
    End Property

    Private _BookFinAPBillInvDate As String = ""
    Public Property BookFinAPBillInvDate() As String
        Get
            Return cleanDate(_BookFinAPBillInvDate)
        End Get
        Set(ByVal value As String)
            _BookFinAPBillInvDate = value
        End Set
    End Property

    Private _BookFinAPGLNumber As String = ""
    Public Property BookFinAPGLNumber() As String
        Get
            Return Left(_BookFinAPGLNumber, 50)
        End Get
        Set(ByVal value As String)
            _BookFinAPGLNumber = Left(value, 50)
        End Set
    End Property

    Private _APGLDescription As String
    Public Property APGLDescription() As String
        Get
            Return _APGLDescription
        End Get
        Set(ByVal value As String)
            _APGLDescription = value
        End Set
    End Property


    Public Shared Function GenerateSampleObject(ByVal FreightCost As Double, ByVal FreightBillNumber As String, ByVal OrderNumber As String, ByVal CompNumber As Integer, ByVal CompAlphaCode As String, ByVal CompLegalEntity As String) As clsPayablesObject70

        Return New clsPayablesObject70 With { _
                .CompLegalEntity = CompLegalEntity,
               .CompNumber = CompNumber,
               .CompAlphaCode = CompAlphaCode,
               .BookCarrOrderNumber = OrderNumber,
               .BookOrderSequence = 0,
               .BookFinAPPayAmt = FreightCost,
               .BookFinAPCheck = "unit test check",
               .BookFinAPPayDate = Date.Now(),
               .BookFinAPBillNumber = FreightBillNumber
               }

    End Function


End Class


<Serializable()> _
Public Class clsPayablesObject705 : Inherits clsPayablesObject70

    
    Private _CarrierNumber As Integer = 0
    Public Property CarrierNumber() As Integer
        Get
            Return _CarrierNumber
        End Get
        Set(ByVal value As Integer)
            _CarrierNumber = value
        End Set
    End Property

    Private _CarrierAlphaCode As String = ""
    Public Property CarrierAlphaCode() As String
        Get
            Return Left(_CarrierAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _CarrierAlphaCode = Left(value, 50)
        End Set
    End Property

End Class
