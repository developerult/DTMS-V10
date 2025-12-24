Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierProNumber
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrProControl As Integer = 0
        <DataMember()> _
        Public Property CarrProControl() As Integer
            Get
                Return Me._CarrProControl
            End Get
            Set(value As Integer)
                If ((Me._CarrProControl = value) _
                   = False) Then
                    Me._CarrProControl = value
                    Me.SendPropertyChanged("CarrProControl")
                End If
            End Set
        End Property

        Private _CarrProCarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrProCarrierControl() As Integer
            Get
                Return Me._CarrProCarrierControl
            End Get
            Set(value As Integer)
                If ((Me._CarrProCarrierControl = value) _
                   = False) Then
                    Me._CarrProCarrierControl = value
                    Me.SendPropertyChanged("CarrProCarrierControl")
                End If
            End Set
        End Property

        Private _CarrProCompControl As Integer = 0
        <DataMember()> _
        Public Property CarrProCompControl() As Integer
            Get
                Return Me._CarrProCompControl
            End Get
            Set(value As Integer)
                If ((Me._CarrProCompControl = value) _
                   = False) Then
                    Me._CarrProCompControl = value
                    Me.SendPropertyChanged("CarrProCompControl")
                End If
            End Set
        End Property

        Private _CarrProChkDigAlgControl As Integer = 1
        <DataMember()> _
        Public Property CarrProChkDigAlgControl() As Integer
            Get
                Return Me._CarrProChkDigAlgControl
            End Get
            Set(value As Integer)
                If ((Me._CarrProChkDigAlgControl = value) _
                   = False) Then
                    Me._CarrProChkDigAlgControl = value
                    Me.SendPropertyChanged("CarrProChkDigAlgControl")
                End If
            End Set
        End Property

        Private _CarrProName As String
        <DataMember()> _
        Public Property CarrProName() As String
            Get
                Return Left(Me._CarrProName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProName, value) = False) Then
                    Me._CarrProName = Left(value, 50)
                    Me.SendPropertyChanged("CarrProName")
                End If
            End Set
        End Property

        Private _CarrProDesc As String
        <DataMember()> _
        Public Property CarrProDesc() As String
            Get
                Return Left(Me._CarrProDesc, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProDesc, value) = False) Then
                    Me._CarrProDesc = Left(value, 255)
                    Me.SendPropertyChanged("CarrProDesc")
                End If
            End Set
        End Property

        Private _CarrProPrefix As String
        <DataMember()> _
        Public Property CarrProPrefix() As String
            Get
                Return Left(Me._CarrProPrefix, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProPrefix, value) = False) Then
                    Me._CarrProPrefix = Left(value, 10)
                    Me.SendPropertyChanged("CarrProPrefix")
                End If
            End Set
        End Property

        Private _CarrProPrefixSpacer As String
        <DataMember()> _
        Public Property CarrProPrefixSpacer() As String
            Get
                Return Left(Me._CarrProPrefixSpacer, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProPrefixSpacer, value) = False) Then
                    Me._CarrProPrefixSpacer = Left(value, 10)
                    Me.SendPropertyChanged("CarrProPrefixSpacer")
                End If
            End Set
        End Property

        Private _CarrProSuffix As String
        <DataMember()> _
        Public Property CarrProSuffix() As String
            Get
                Return Left(Me._CarrProSuffix, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProSuffix, value) = False) Then
                    Me._CarrProSuffix = Left(value, 10)
                    Me.SendPropertyChanged("CarrProSuffix")
                End If
            End Set
        End Property

        Private _CarrProSuffixSpacer As String
        <DataMember()> _
        Public Property CarrProSuffixSpacer() As String
            Get
                Return Left(Me._CarrProSuffixSpacer, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProSuffixSpacer, value) = False) Then
                    Me._CarrProSuffixSpacer = Left(value, 10)
                    Me.SendPropertyChanged("CarrProSuffixSpacer")
                End If
            End Set
        End Property

        Private _CarrProCheckDigitSpacer As String
        <DataMember()> _
        Public Property CarrProCheckDigitSpacer() As String
            Get
                Return Left(Me._CarrProCheckDigitSpacer, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProCheckDigitSpacer, value) = False) Then
                    Me._CarrProCheckDigitSpacer = Left(value, 10)
                    Me.SendPropertyChanged("CarrProCheckDigitSpacer")
                End If
            End Set
        End Property

        Private _CarrProPrintCheckDigitOnSeperateBarCode As Boolean = False
        <DataMember()> _
        Public Property CarrProPrintCheckDigitOnSeperateBarCode() As Boolean
            Get
                Return Me._CarrProPrintCheckDigitOnSeperateBarCode
            End Get
            Set(value As Boolean)
                If ((Me._CarrProPrintCheckDigitOnSeperateBarCode = value) _
                            = False) Then
                    Me._CarrProPrintCheckDigitOnSeperateBarCode = value
                    Me.SendPropertyChanged("CarrProPrintCheckDigitOnSeperateBarCode")
                End If
            End Set
        End Property

        Private _CarrProPrintSpacersOnBarCode As Boolean = False
        <DataMember()> _
        Public Property CarrProPrintSpacersOnBarCode() As Boolean
            Get
                Return Me._CarrProPrintSpacersOnBarCode
            End Get
            Set(value As Boolean)
                If ((Me._CarrProPrintSpacersOnBarCode = value) _
                            = False) Then
                    Me._CarrProPrintSpacersOnBarCode = value
                    Me.SendPropertyChanged("CarrProPrintSpacersOnBarCode")
                End If
            End Set
        End Property

        Private _CarrProActive As Boolean = True
        <DataMember()> _
        Public Property CarrProActive() As Boolean
            Get
                Return Me._CarrProActive
            End Get
            Set(value As Boolean)
                If ((Me._CarrProActive = value) _
                            = False) Then
                    Me._CarrProActive = value
                    Me.SendPropertyChanged("CarrProActive")
                End If
            End Set
        End Property

        Private _CarrProAppendPrefixForCheckDigit As Boolean = False
        <DataMember()> _
        Public Property CarrProAppendPrefixForCheckDigit() As Boolean
            Get
                Return Me._CarrProAppendPrefixForCheckDigit
            End Get
            Set(value As Boolean)
                If ((Me._CarrProAppendPrefixForCheckDigit = value) _
                            = False) Then
                    Me._CarrProAppendPrefixForCheckDigit = value
                    Me.SendPropertyChanged("CarrProAppendPrefixForCheckDigit")
                End If
            End Set
        End Property

        Private _CarrProAppendSuffixForCheckDigit As Boolean = False
        <DataMember()> _
        Public Property CarrProAppendSuffixForCheckDigit() As Boolean
            Get
                Return Me._CarrProAppendSuffixForCheckDigit
            End Get
            Set(value As Boolean)
                If ((Me._CarrProAppendSuffixForCheckDigit = value) _
                            = False) Then
                    Me._CarrProAppendSuffixForCheckDigit = value
                    Me.SendPropertyChanged("CarrProAppendSuffixForCheckDigit")
                End If
            End Set
        End Property

        Private _CarrProSeedStart As Long = 1
        <DataMember()> _
        Public Property CarrProSeedStart() As Long
            Get
                Return Me._CarrProSeedStart
            End Get
            Set(value As Long)
                If ((Me._CarrProSeedStart = value) _
                   = False) Then
                    Me._CarrProSeedStart = value
                    Me.SendPropertyChanged("CarrProSeedStart")
                End If
            End Set
        End Property

        Private _CarrProSeedEnd As Long = 9999
        <DataMember()> _
        Public Property CarrProSeedEnd() As Long
            Get
                Return Me._CarrProSeedEnd
            End Get
            Set(value As Long)
                If ((Me._CarrProSeedEnd = value) _
                   = False) Then
                    Me._CarrProSeedEnd = value
                    Me.SendPropertyChanged("CarrProSeedEnd")
                End If
            End Set
        End Property

        Private _CarrProSeedCurrent As Long = 0
        <DataMember()> _
        Public Property CarrProSeedCurrent() As Long
            Get
                Return Me._CarrProSeedCurrent
            End Get
            Set(value As Long)
                If ((Me._CarrProSeedCurrent = value) _
                   = False) Then
                    Me._CarrProSeedCurrent = value
                    Me.SendPropertyChanged("CarrProSeedCurrent")
                End If
            End Set
        End Property

        Private _CarrProSeedStepFactor As Integer = 1
        <DataMember()> _
        Public Property CarrProSeedStepFactor() As Integer
            Get
                Return Me._CarrProSeedStepFactor
            End Get
            Set(value As Integer)
                If ((Me._CarrProSeedStepFactor = value) _
                   = False) Then
                    Me._CarrProSeedStepFactor = value
                    Me.SendPropertyChanged("CarrProSeedStepFactor")
                End If
            End Set
        End Property

        Private _CarrProSeedWarningSeed As Long = 9990
        <DataMember()> _
        Public Property CarrProSeedWarningSeed() As Long
            Get
                Return Me._CarrProSeedWarningSeed
            End Get
            Set(value As Long)
                If ((Me._CarrProSeedWarningSeed = value) _
                   = False) Then
                    Me._CarrProSeedWarningSeed = value
                    Me.SendPropertyChanged("CarrProSeedWarningSeed")
                End If
            End Set
        End Property

        Private _CarrProLength As Integer = 4
        <DataMember()> _
        Public Property CarrProLength() As Integer
            Get
                Return Me._CarrProLength
            End Get
            Set(value As Integer)
                If ((Me._CarrProLength = value) _
                   = False) Then
                    Me._CarrProLength = value
                    Me.SendPropertyChanged("CarrProLength")
                End If
            End Set
        End Property

        Private _CarrProCheckDigitWeightFactor As System.Nullable(Of Long)
        <DataMember()> _
        Public Property CarrProCheckDigitWeightFactor() As System.Nullable(Of Long)
            Get
                Return Me._CarrProCheckDigitWeightFactor
            End Get
            Set(value As System.Nullable(Of Long))
                If (Me._CarrProCheckDigitWeightFactor.Equals(value) = False) Then
                    Me._CarrProCheckDigitWeightFactor = value
                    Me.SendPropertyChanged("CarrProCheckDigitWeightFactor")
                End If
            End Set
        End Property

        Private _CarrProCheckDigitSplitWeightFactorDigits As Boolean = True
        <DataMember()> _
        Public Property CarrProCheckDigitSplitWeightFactorDigits() As Boolean
            Get
                Return Me._CarrProCheckDigitSplitWeightFactorDigits
            End Get
            Set(value As Boolean)
                If ((Me._CarrProCheckDigitSplitWeightFactorDigits = value) _
                            = False) Then
                    Me._CarrProCheckDigitSplitWeightFactorDigits = value
                    Me.SendPropertyChanged("CarrProCheckDigitSplitWeightFactorDigits")
                End If
            End Set
        End Property

        Private _CarrProCheckDigitUseIndexForWeightFactor As Boolean = False
        <DataMember()> _
        Public Property CarrProCheckDigitUseIndexForWeightFactor() As Boolean
            Get
                Return Me._CarrProCheckDigitUseIndexForWeightFactor
            End Get
            Set(value As Boolean)
                If ((Me._CarrProCheckDigitUseIndexForWeightFactor = value) _
                            = False) Then
                    Me._CarrProCheckDigitUseIndexForWeightFactor = value
                    Me.SendPropertyChanged("CarrProCheckDigitUseIndexForWeightFactor")
                End If
            End Set
        End Property

        Private _CarrProCheckDigitIndexForWeightFactorMin As Integer = 2
        <DataMember()> _
        Public Property CarrProCheckDigitIndexForWeightFactorMin() As Integer
            Get
                Return Me._CarrProCheckDigitIndexForWeightFactorMin
            End Get
            Set(value As Integer)
                If ((Me._CarrProCheckDigitIndexForWeightFactorMin = value) _
                   = False) Then
                    Me._CarrProCheckDigitIndexForWeightFactorMin = value
                    Me.SendPropertyChanged("CarrProCheckDigitIndexForWeightFactorMin")
                End If
            End Set
        End Property

        Private _CarrProCheckDigitErrorCode As String
        <DataMember()> _
        Public Property CarrProCheckDigitErrorCode() As String
            Get
                Return Left(Me._CarrProCheckDigitErrorCode, 1)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProCheckDigitErrorCode, value) = False) Then
                    Me._CarrProCheckDigitErrorCode = Left(value, 1)
                    Me.SendPropertyChanged("CarrProCheckDigitErrorCode")
                End If
            End Set
        End Property

        Private _CarrProCheckDigit10Code As String
        <DataMember()> _
        Public Property CarrProCheckDigit10Code() As String
            Get
                Return Left(Me._CarrProCheckDigit10Code, 1)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProCheckDigit10Code, value) = False) Then
                    Me._CarrProCheckDigit10Code = Left(value, 1)
                    Me.SendPropertyChanged("CarrProCheckDigit10Code")
                End If
            End Set
        End Property

        Private _CarrProCheckDigitOver10Code As String
        <DataMember()> _
        Public Property CarrProCheckDigitOver10Code() As String
            Get
                Return Left(Me._CarrProCheckDigitOver10Code, 1)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProCheckDigitOver10Code, value) = False) Then
                    Me._CarrProCheckDigitOver10Code = Left(value, 1)
                    Me.SendPropertyChanged("CarrProCheckDigitOver10Code")
                End If
            End Set
        End Property

        Private _CarrProCheckDigitZeroCode As String
        <DataMember()> _
        Public Property CarrProCheckDigitZeroCode() As String
            Get
                Return Left(Me._CarrProCheckDigitZeroCode, 1)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProCheckDigitZeroCode, value) = False) Then
                    Me._CarrProCheckDigitZeroCode = Left(value, 1)
                    Me.SendPropertyChanged("CarrProCheckDigitZeroCode")
                End If
            End Set
        End Property

        Private _CarrProExp1 As String
        <DataMember()> _
        Public Property CarrProExp1() As String
            Get
                Return Left(Me._CarrProExp1, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProExp1, value) = False) Then
                    Me._CarrProExp1 = Left(value, 100)
                    Me.SendPropertyChanged("CarrProExp1")
                End If
            End Set
        End Property

        Private _CarrProExp2 As String
        <DataMember()> _
        Public Property CarrProExp2() As String
            Get
                Return Left(Me._CarrProExp2, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProExp2, value) = False) Then
                    Me._CarrProExp2 = Left(value, 100)
                    Me.SendPropertyChanged("CarrProExp2")
                End If
            End Set
        End Property

        Private _CarrProExp3 As String
        <DataMember()> _
        Public Property CarrProExp3() As String
            Get
                Return Left(Me._CarrProExp3, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProExp3, value) = False) Then
                    Me._CarrProExp3 = Left(value, 100)
                    Me.SendPropertyChanged("CarrProExp3")
                End If
            End Set
        End Property

        Private _CarrProExp4 As String
        <DataMember()> _
        Public Property CarrProExp4() As String
            Get
                Return Left(Me._CarrProExp4, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProExp4, value) = False) Then
                    Me._CarrProExp4 = Left(value, 100)
                    Me.SendPropertyChanged("CarrProExp4")
                End If
            End Set
        End Property

        Private _CarrProUser1 As String
        <DataMember()> _
        Public Property CarrProUser1() As String
            Get
                Return Left(Me._CarrProUser1, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProUser1, value) = False) Then
                    Me._CarrProUser1 = Left(value, 4000)
                    Me.SendPropertyChanged("CarrProUser1")
                End If
            End Set
        End Property

        Private _CarrProUser2 As String
        <DataMember()> _
        Public Property CarrProUser2() As String
            Get
                Return Left(Me._CarrProUser2, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProUser2, value) = False) Then
                    Me._CarrProUser2 = Left(value, 4000)
                    Me.SendPropertyChanged("CarrProUser2")
                End If
            End Set
        End Property

        Private _CarrProUser3 As String
        <DataMember()> _
        Public Property CarrProUser3() As String
            Get
                Return Left(Me._CarrProUser3, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProUser3, value) = False) Then
                    Me._CarrProUser3 = Left(value, 4000)
                    Me.SendPropertyChanged("CarrProUser3")
                End If
            End Set
        End Property

        Private _CarrProUser4 As String
        <DataMember()> _
        Public Property CarrProUser4() As String
            Get
                Return Left(Me._CarrProUser4, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProUser4, value) = False) Then
                    Me._CarrProUser4 = Left(value, 4000)
                    Me.SendPropertyChanged("CarrProUser4")
                End If
            End Set
        End Property

        Private _CarrProModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrProModDate() As System.Nullable(Of Date)
            Get
                Return Me._CarrProModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._CarrProModDate.Equals(value) = False) Then
                    Me._CarrProModDate = value
                    Me.SendPropertyChanged("CarrProModDate")
                End If
            End Set
        End Property

        Private _CarrProModUser As String
        <DataMember()> _
        Public Property CarrProModUser() As String
            Get
                Return Left(Me._CarrProModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrProModUser, value) = False) Then
                    Me._CarrProModUser = Left(value, 100)
                    Me.SendPropertyChanged("CarrProModUser")
                End If
            End Set
        End Property

        Private _CarrProUpdated As Byte()
        <DataMember()> _
        Public Property CarrProUpdated() As Byte()
            Get
                Return Me._CarrProUpdated
            End Get
            Set(value As Byte())
                Me._CarrProUpdated = value
            End Set
        End Property

        Private _CarrProCheckDigitUseSubtractionFactor As Boolean = False
        <DataMember()> _
        Public Property CarrProCheckDigitUseSubtractionFactor() As Boolean
            Get
                Return Me._CarrProCheckDigitUseSubtractionFactor
            End Get
            Set(value As Boolean)
                If ((Me._CarrProCheckDigitUseSubtractionFactor = value) _
                            = False) Then
                    Me._CarrProCheckDigitUseSubtractionFactor = value
                    Me.SendPropertyChanged("CarrProCheckDigitUseSubtractionFactor")
                End If
            End Set
        End Property

        Private _CarrProCheckDigitSubtractionFactor As Integer = 0
        <DataMember()> _
        Public Property CarrProCheckDigitSubtractionFactor() As Integer
            Get
                Return Me._CarrProCheckDigitSubtractionFactor
            End Get
            Set(value As Integer)
                If ((Me._CarrProCheckDigitSubtractionFactor = value) _
                   = False) Then
                    Me._CarrProCheckDigitSubtractionFactor = value
                    Me.SendPropertyChanged("CarrProCheckDigitSubtractionFactor")
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierProNumber
            instance = DirectCast(MemberwiseClone(), CarrierProNumber)
            Return instance
        End Function

#End Region

    End Class
End Namespace