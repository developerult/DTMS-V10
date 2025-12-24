Namespace Models
    'Added By LVV On 9/16/20 For v-8.3.0.001 - Optimizer 365

    Public Class OptMsgStep

        Private _StepNumber As Integer
        Private _StepProgress As Integer
        Private _StepComplete As Boolean
        Private _StepMessage As String
        Private _StepSubMessage As String

        Public Property StepNumber() As Integer
            Get
                Return _StepNumber
            End Get
            Set(ByVal value As Integer)
                _StepNumber = value
            End Set
        End Property

        Public Property StepProgress() As Integer
            Get
                Return _StepProgress
            End Get
            Set(ByVal value As Integer)
                _StepProgress = value
            End Set
        End Property

        Public Property StepComplete() As Boolean
            Get
                Return _StepComplete
            End Get
            Set(ByVal value As Boolean)
                _StepComplete = value
            End Set
        End Property

        Public Property StepMessage() As String
            Get
                Return _StepMessage
            End Get
            Set(ByVal value As String)
                _StepMessage = value
            End Set
        End Property

        Public Property StepSubMessage() As String
            Get
                Return _StepSubMessage
            End Get
            Set(ByVal value As String)
                _StepSubMessage = value
            End Set
        End Property


    End Class

    Public Class OptMsg

        Private _Step1 As OptMsgStep
        Private _Step2 As OptMsgStep
        Private _Step3 As OptMsgStep
        Private _Step4 As OptMsgStep
        Private _Step5 As OptMsgStep
        Private _Step6 As OptMsgStep
        Private _Step7 As OptMsgStep
        Private _Step8 As OptMsgStep
        Private _Step9 As OptMsgStep
        Private _Step10 As OptMsgStep
        Private _Step11 As OptMsgStep
        Private _Step12 As OptMsgStep

        Public Property Step1() As OptMsgStep
            Get
                Return _Step1
            End Get
            Set(ByVal value As OptMsgStep)
                _Step1 = value
            End Set
        End Property

        Public Property Step2() As OptMsgStep
            Get
                Return _Step2
            End Get
            Set(ByVal value As OptMsgStep)
                _Step2 = value
            End Set
        End Property

        Public Property Step3() As OptMsgStep
            Get
                Return _Step3
            End Get
            Set(ByVal value As OptMsgStep)
                _Step3 = value
            End Set
        End Property

        Public Property Step4() As OptMsgStep
            Get
                Return _Step4
            End Get
            Set(ByVal value As OptMsgStep)
                _Step4 = value
            End Set
        End Property

        Public Property Step5() As OptMsgStep
            Get
                Return _Step5
            End Get
            Set(ByVal value As OptMsgStep)
                _Step5 = value
            End Set
        End Property

        Public Property Step6() As OptMsgStep
            Get
                Return _Step6
            End Get
            Set(ByVal value As OptMsgStep)
                _Step6 = value
            End Set
        End Property

        Public Property Step7() As OptMsgStep
            Get
                Return _Step7
            End Get
            Set(ByVal value As OptMsgStep)
                _Step7 = value
            End Set
        End Property

        Public Property Step8() As OptMsgStep
            Get
                Return _Step8
            End Get
            Set(ByVal value As OptMsgStep)
                _Step8 = value
            End Set
        End Property

        Public Property Step9() As OptMsgStep
            Get
                Return _Step9
            End Get
            Set(ByVal value As OptMsgStep)
                _Step9 = value
            End Set
        End Property

        Public Property Step10() As OptMsgStep
            Get
                Return _Step10
            End Get
            Set(ByVal value As OptMsgStep)
                _Step10 = value
            End Set
        End Property

        Public Property Step11() As OptMsgStep
            Get
                Return _Step11
            End Get
            Set(ByVal value As OptMsgStep)
                _Step11 = value
            End Set
        End Property

        Public Property Step12() As OptMsgStep
            Get
                Return _Step12
            End Get
            Set(ByVal value As OptMsgStep)
                _Step12 = value
            End Set
        End Property

    End Class

End Namespace