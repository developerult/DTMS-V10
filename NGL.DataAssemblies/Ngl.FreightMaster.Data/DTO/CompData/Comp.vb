Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class Comp
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CompControl As Integer = 0
        <DataMember()> _
        Public Property CompControl() As Integer
            Get
                Return _CompControl
            End Get
            Set(ByVal value As Integer)
                _CompControl = value
            End Set
        End Property

        Private _CompNumber As Integer = 0
        <DataMember()> _
        Public Property CompNumber() As Integer
            Get
                Return _CompNumber
            End Get
            Set(ByVal value As Integer)
                _CompNumber = value
            End Set
        End Property

        Private _CompName As String = ""
        <DataMember()> _
        Public Property CompName() As String
            Get
                Return Left(_CompName, 40)
            End Get
            Set(ByVal value As String)
                _CompName = Left(value, 40)
            End Set
        End Property

        Private _CompNatNumber As Integer = 0
        <DataMember()> _
        Public Property CompNatNumber() As Integer
            Get
                Return _CompNatNumber
            End Get
            Set(ByVal value As Integer)
                _CompNatNumber = value
            End Set
        End Property

        Private _CompNatName As String = ""
        <DataMember()> _
        Public Property CompNatName() As String
            Get
                Return Left(_CompNatName, 40)
            End Get
            Set(ByVal value As String)
                _CompNatName = Left(value, 40)
            End Set
        End Property

        Private _CompStreetAddress1 As String = ""
        <DataMember()> _
        Public Property CompStreetAddress1() As String
            Get
                Return Left(_CompStreetAddress1, 40)
            End Get
            Set(ByVal value As String)
                _CompStreetAddress1 = Left(value, 40)
            End Set
        End Property

        Private _CompStreetAddress2 As String = ""
        <DataMember()> _
        Public Property CompStreetAddress2() As String
            Get
                Return Left(_CompStreetAddress2, 40)
            End Get
            Set(ByVal value As String)
                _CompStreetAddress2 = Left(value, 40)
            End Set
        End Property

        Private _CompStreetAddress3 As String = ""
        <DataMember()> _
        Public Property CompStreetAddress3() As String
            Get
                Return Left(_CompStreetAddress3, 40)
            End Get
            Set(ByVal value As String)
                _CompStreetAddress3 = Left(value, 40)
            End Set
        End Property

        Private _CompStreetCity As String = ""
        <DataMember()> _
        Public Property CompStreetCity() As String
            Get
                Return Left(_CompStreetCity, 25)
            End Get
            Set(ByVal value As String)
                _CompStreetCity = Left(value, 25)
            End Set
        End Property

        Private _CompStreetState As String = ""
        <DataMember()> _
        Public Property CompStreetState() As String
            Get
                Return Left(_CompStreetState, 8)
            End Get
            Set(ByVal value As String)
                _CompStreetState = Left(value, 8)
            End Set
        End Property

        Private _CompStreetCountry As String = ""
        <DataMember()> _
        Public Property CompStreetCountry() As String
            Get
                Return Left(_CompStreetCountry, 30)
            End Get
            Set(ByVal value As String)
                _CompStreetCountry = Left(value, 30)
            End Set
        End Property

        Private _CompStreetZip As String = ""
        <DataMember()> _
        Public Property CompStreetZip() As String
            Get
                Return Left(_CompStreetZip, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _CompStreetZip = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _CompMailAddress1 As String = ""
        <DataMember()> _
        Public Property CompMailAddress1() As String
            Get
                Return Left(_CompMailAddress1, 40)
            End Get
            Set(ByVal value As String)
                _CompMailAddress1 = Left(value, 40)
            End Set
        End Property

        Private _CompMailAddress2 As String = ""
        <DataMember()> _
        Public Property CompMailAddress2() As String
            Get
                Return Left(_CompMailAddress2, 40)
            End Get
            Set(ByVal value As String)
                _CompMailAddress2 = Left(value, 40)
            End Set
        End Property

        Private _CompMailAddress3 As String = ""
        <DataMember()> _
        Public Property CompMailAddress3() As String
            Get
                Return Left(_CompMailAddress3, 40)
            End Get
            Set(ByVal value As String)
                _CompMailAddress3 = Left(value, 40)
            End Set
        End Property

        Private _CompMailCity As String = ""
        <DataMember()> _
        Public Property CompMailCity() As String
            Get
                Return Left(_CompMailCity, 25)
            End Get
            Set(ByVal value As String)
                _CompMailCity = Left(value, 25)
            End Set
        End Property

        Private _CompMailState As String = ""
        <DataMember()> _
        Public Property CompMailState() As String
            Get
                Return Left(_CompMailState, 8)
            End Get
            Set(ByVal value As String)
                _CompMailState = Left(value, 8)
            End Set
        End Property

        Private _CompMailCountry As String = ""
        <DataMember()> _
        Public Property CompMailCountry() As String
            Get
                Return Left(_CompMailCountry, 30)
            End Get
            Set(ByVal value As String)
                _CompMailCountry = Left(value, 30)
            End Set
        End Property

        Private _CompMailZip As String = ""
        <DataMember()> _
        Public Property CompMailZip() As String
            Get
                Return Left(_CompMailZip, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _CompMailZip = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _CompWeb As String = ""
        <DataMember()> _
        Public Property CompWeb() As String
            Get
                Return Left(_CompWeb, 255)
            End Get
            Set(ByVal value As String)
                _CompWeb = Left(value, 255)
            End Set
        End Property

        Private _CompEmail As String = ""
        <DataMember()> _
        Public Property CompEmail() As String
            Get
                Return Left(_CompEmail, 50)
            End Get
            Set(ByVal value As String)
                _CompEmail = Left(value, 50)
            End Set
        End Property

        Private _CompModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CompModDate() As System.Nullable(Of Date)
            Get
                Return _CompModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CompModDate = value
            End Set
        End Property

        Private _CompModUser As String = ""
        <DataMember()> _
        Public Property CompModUser() As String
            Get
                Return Left(_CompModUser, 100)
            End Get
            Set(ByVal value As String)
                _CompModUser = Left(value, 100)
            End Set
        End Property

        Private _CompDirections As String = ""
        <DataMember()> _
        Public Property CompDirections() As String
            Get
                Return _CompDirections
            End Get
            Set(ByVal value As String)
                _CompDirections = value
            End Set
        End Property

        Private _CompAbrev As String = ""
        <DataMember()> _
        Public Property CompAbrev() As String
            Get
                Return Left(_CompAbrev, 3)
            End Get
            Set(ByVal value As String)
                _CompAbrev = Left(value, 3)
            End Set
        End Property

        Private _CompActive As Boolean = True
        <DataMember()> _
        Public Property CompActive() As Boolean
            Get
                Return _CompActive
            End Get
            Set(ByVal value As Boolean)
                _CompActive = value
            End Set
        End Property

        Private _CompNEXTrack As Boolean = True
        <DataMember()> _
        Public Property CompNEXTrack() As Boolean
            Get
                Return _CompNEXTrack
            End Get
            Set(ByVal value As Boolean)
                _CompNEXTrack = value
            End Set
        End Property

        Private _CompNEXTStopAcctNo As String = ""
        <DataMember()> _
        Public Property CompNEXTStopAcctNo() As String
            Get
                Return Left(_CompNEXTStopAcctNo, 50)
            End Get
            Set(ByVal value As String)
                _CompNEXTStopAcctNo = Left(value, 50)
            End Set
        End Property

        Private _CompNEXTStopPsw As String = ""
        <DataMember()> _
        Public Property CompNEXTStopPsw() As String
            Get
                Return Left(_CompNEXTStopPsw, 50)
            End Get
            Set(ByVal value As String)
                _CompNEXTStopPsw = Left(value, 50)
            End Set
        End Property

        Private _CompNextstopSubmitRFP As Boolean = True
        <DataMember()> _
        Public Property CompNextstopSubmitRFP() As Boolean
            Get
                Return _CompNextstopSubmitRFP
            End Get
            Set(ByVal value As Boolean)
                _CompNextstopSubmitRFP = value
            End Set
        End Property

        Private _CompTypeCategory As Short = 0
        <DataMember()> _
        Public Property CompTypeCategory() As Short
            Get
                Return _CompTypeCategory
            End Get
            Set(ByVal value As Short)
                _CompTypeCategory = value
            End Set
        End Property

        Private _CompFAAShipID As String = ""
        <DataMember()> _
        Public Property CompFAAShipID() As String
            Get
                Return Left(_CompFAAShipID, 50)
            End Get
            Set(ByVal value As String)
                _CompFAAShipID = Left(value, 50)
            End Set
        End Property

        Private _CompFAAShipDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CompFAAShipDate() As System.Nullable(Of Date)
            Get
                Return _CompFAAShipDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CompFAAShipDate = value
            End Set
        End Property

        Private _CompUpdated As Byte()
        <DataMember()> _
        Public Property CompUpdated() As Byte()
            Get
                Return _CompUpdated
            End Get
            Set(ByVal value As Byte())
                _CompUpdated = value
            End Set
        End Property

        Private _CompBudSeasDescription As String = ""
        <DataMember()> _
        Public Property CompBudSeasDescription() As String
            Get
                Return Left(_CompBudSeasDescription, 50)
            End Get
            Set(ByVal value As String)
                _CompBudSeasDescription = Left(value, 50)
            End Set
        End Property

        Private _CompBudSeasMo1 As Double = 0
        <DataMember()> _
        Public Property CompBudSeasMo1() As Double
            Get
                Return _CompBudSeasMo1
            End Get
            Set(ByVal value As Double)
                _CompBudSeasMo1 = value
            End Set
        End Property

        Private _CompBudSeasMo2 As Double = 0
        <DataMember()> _
        Public Property CompBudSeasMo2() As Double
            Get
                Return _CompBudSeasMo2
            End Get
            Set(ByVal value As Double)
                _CompBudSeasMo2 = value
            End Set
        End Property

        Private _CompBudSeasMo3 As Double = 0
        <DataMember()> _
        Public Property CompBudSeasMo3() As Double
            Get
                Return _CompBudSeasMo3
            End Get
            Set(ByVal value As Double)
                _CompBudSeasMo3 = value
            End Set
        End Property

        Private _CompBudSeasMo4 As Double = 0
        <DataMember()> _
        Public Property CompBudSeasMo4() As Double
            Get
                Return _CompBudSeasMo4
            End Get
            Set(ByVal value As Double)
                _CompBudSeasMo4 = value
            End Set
        End Property

        Private _CompBudSeasMo5 As Double = 0
        <DataMember()> _
        Public Property CompBudSeasMo5() As Double
            Get
                Return _CompBudSeasMo5
            End Get
            Set(ByVal value As Double)
                _CompBudSeasMo5 = value
            End Set
        End Property

        Private _CompBudSeasMo6 As Double = 0
        <DataMember()> _
        Public Property CompBudSeasMo6() As Double
            Get
                Return _CompBudSeasMo6
            End Get
            Set(ByVal value As Double)
                _CompBudSeasMo6 = value
            End Set
        End Property

        Private _CompBudSeasMo7 As Double = 0
        <DataMember()> _
        Public Property CompBudSeasMo7() As Double
            Get
                Return _CompBudSeasMo7
            End Get
            Set(ByVal value As Double)
                _CompBudSeasMo7 = value
            End Set
        End Property

        Private _CompBudSeasMo8 As Double = 0
        <DataMember()> _
        Public Property CompBudSeasMo8() As Double
            Get
                Return _CompBudSeasMo8
            End Get
            Set(ByVal value As Double)
                _CompBudSeasMo8 = value
            End Set
        End Property

        Private _CompBudSeasMo9 As Double = 0
        <DataMember()> _
        Public Property CompBudSeasMo9() As Double
            Get
                Return _CompBudSeasMo9
            End Get
            Set(ByVal value As Double)
                _CompBudSeasMo9 = value
            End Set
        End Property

        Private _CompBudSeasMo10 As Double = 0
        <DataMember()> _
        Public Property CompBudSeasMo10() As Double
            Get
                Return _CompBudSeasMo10
            End Get
            Set(ByVal value As Double)
                _CompBudSeasMo10 = value
            End Set
        End Property

        Private _CompBudSeasMo11 As Double = 0
        <DataMember()> _
        Public Property CompBudSeasMo11() As Double
            Get
                Return _CompBudSeasMo11
            End Get
            Set(ByVal value As Double)
                _CompBudSeasMo11 = value
            End Set
        End Property

        Private _CompBudSeasMo12 As Double = 0
        <DataMember()> _
        Public Property CompBudSeasMo12() As Double
            Get
                Return _CompBudSeasMo12
            End Get
            Set(ByVal value As Double)
                _CompBudSeasMo12 = value
            End Set
        End Property

        Private _CompBudSlsBudgetMo1 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsBudgetMo1() As Decimal
            Get
                Return _CompBudSlsBudgetMo1
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsBudgetMo1 = value
            End Set
        End Property

        Private _CompBudSlsBudgetMo2 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsBudgetMo2() As Decimal
            Get
                Return _CompBudSlsBudgetMo2
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsBudgetMo2 = value
            End Set
        End Property

        Private _CompBudSlsBudgetMo3 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsBudgetMo3() As Decimal
            Get
                Return _CompBudSlsBudgetMo3
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsBudgetMo3 = value
            End Set
        End Property

        Private _CompBudSlsBudgetMo4 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsBudgetMo4() As Decimal
            Get
                Return _CompBudSlsBudgetMo4
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsBudgetMo4 = value
            End Set
        End Property

        Private _CompBudSlsBudgetMo5 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsBudgetMo5() As Decimal
            Get
                Return _CompBudSlsBudgetMo5
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsBudgetMo5 = value
            End Set
        End Property

        Private _CompBudSlsBudgetMo6 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsBudgetMo6() As Decimal
            Get
                Return _CompBudSlsBudgetMo6
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsBudgetMo6 = value
            End Set
        End Property

        Private _CompBudSlsBudgetMo7 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsBudgetMo7() As Decimal
            Get
                Return _CompBudSlsBudgetMo7
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsBudgetMo7 = value
            End Set
        End Property

        Private _CompBudSlsBudgetMo8 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsBudgetMo8() As Decimal
            Get
                Return _CompBudSlsBudgetMo8
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsBudgetMo8 = value
            End Set
        End Property

        Private _CompBudSlsBudgetMo9 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsBudgetMo9() As Decimal
            Get
                Return _CompBudSlsBudgetMo9
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsBudgetMo9 = value
            End Set
        End Property

        Private _CompBudSlsBudgetMo10 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsBudgetMo10() As Decimal
            Get
                Return _CompBudSlsBudgetMo10
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsBudgetMo10 = value
            End Set
        End Property

        Private _CompBudSlsBudgetMo11 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsBudgetMo11() As Decimal
            Get
                Return _CompBudSlsBudgetMo11
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsBudgetMo11 = value
            End Set
        End Property

        Private _CompBudSlsBudgetMo12 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsBudgetMo12() As Decimal
            Get
                Return _CompBudSlsBudgetMo12
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsBudgetMo12 = value
            End Set
        End Property

        Private _CompBudSlsBudgetMoTotal As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsBudgetMoTotal() As Decimal
            Get
                Return _CompBudSlsBudgetMoTotal
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsBudgetMoTotal = value
            End Set
        End Property

        Private _CompBudSlsActualMo1 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsActualMo1() As Decimal
            Get
                Return _CompBudSlsActualMo1
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsActualMo1 = value
            End Set
        End Property

        Private _CompBudSlsActualMo2 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsActualMo2() As Decimal
            Get
                Return _CompBudSlsActualMo2
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsActualMo2 = value
            End Set
        End Property

        Private _CompBudSlsActualMo3 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsActualMo3() As Decimal
            Get
                Return _CompBudSlsActualMo3
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsActualMo3 = value
            End Set
        End Property

        Private _CompBudSlsActualMo4 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsActualMo4() As Decimal
            Get
                Return _CompBudSlsActualMo4
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsActualMo4 = value
            End Set
        End Property

        Private _CompBudSlsActualMo5 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsActualMo5() As Decimal
            Get
                Return _CompBudSlsActualMo5
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsActualMo5 = value
            End Set
        End Property

        Private _CompBudSlsActualMo6 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsActualMo6() As Decimal
            Get
                Return _CompBudSlsActualMo6
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsActualMo6 = value
            End Set
        End Property

        Private _CompBudSlsActualMo7 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsActualMo7() As Decimal
            Get
                Return _CompBudSlsActualMo7
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsActualMo7 = value
            End Set
        End Property

        Private _CompBudSlsActualMo8 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsActualMo8() As Decimal
            Get
                Return _CompBudSlsActualMo8
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsActualMo8 = value
            End Set
        End Property

        Private _CompBudSlsActualMo9 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsActualMo9() As Decimal
            Get
                Return _CompBudSlsActualMo9
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsActualMo9 = value
            End Set
        End Property

        Private _CompBudSlsActualMo10 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsActualMo10() As Decimal
            Get
                Return _CompBudSlsActualMo10
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsActualMo10 = value
            End Set
        End Property

        Private _CompBudSlsActualMo11 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsActualMo11() As Decimal
            Get
                Return _CompBudSlsActualMo11
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsActualMo11 = value
            End Set
        End Property

        Private _CompBudSlsActualMo12 As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsActualMo12() As Decimal
            Get
                Return _CompBudSlsActualMo12
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsActualMo12 = value
            End Set
        End Property

        Private _CompBudSlsActualMoTotal As Decimal = 0
        <DataMember()> _
        Public Property CompBudSlsActualMoTotal() As Decimal
            Get
                Return _CompBudSlsActualMoTotal
            End Get
            Set(ByVal value As Decimal)
                _CompBudSlsActualMoTotal = value
            End Set
        End Property

        Private _CompBudSlsMarginBudget As Double = 0
        <DataMember()> _
        Public Property CompBudSlsMarginBudget() As Double
            Get
                Return _CompBudSlsMarginBudget
            End Get
            Set(ByVal value As Double)
                _CompBudSlsMarginBudget = value
            End Set
        End Property

        Private _CompBudSlsMarginActual As Double = 0
        <DataMember()> _
        Public Property CompBudSlsMarginActual() As Double
            Get
                Return _CompBudSlsMarginActual
            End Get
            Set(ByVal value As Double)
                _CompBudSlsMarginActual = value
            End Set
        End Property

        Private _CompBudCogsBudgetMo1 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsBudgetMo1() As Decimal
            Get
                Return _CompBudCogsBudgetMo1
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsBudgetMo1 = value
            End Set
        End Property

        Private _CompBudCogsBudgetMo2 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsBudgetMo2() As Decimal
            Get
                Return _CompBudCogsBudgetMo2
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsBudgetMo2 = value
            End Set
        End Property

        Private _CompBudCogsBudgetMo3 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsBudgetMo3() As Decimal
            Get
                Return _CompBudCogsBudgetMo3
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsBudgetMo3 = value
            End Set
        End Property

        Private _CompBudCogsBudgetMo4 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsBudgetMo4() As Decimal
            Get
                Return _CompBudCogsBudgetMo4
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsBudgetMo4 = value
            End Set
        End Property

        Private _CompBudCogsBudgetMo5 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsBudgetMo5() As Decimal
            Get
                Return _CompBudCogsBudgetMo5
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsBudgetMo5 = value
            End Set
        End Property

        Private _CompBudCogsBudgetMo6 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsBudgetMo6() As Decimal
            Get
                Return _CompBudCogsBudgetMo6
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsBudgetMo6 = value
            End Set
        End Property

        Private _CompBudCogsBudgetMo7 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsBudgetMo7() As Decimal
            Get
                Return _CompBudCogsBudgetMo7
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsBudgetMo7 = value
            End Set
        End Property

        Private _CompBudCogsBudgetMo8 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsBudgetMo8() As Decimal
            Get
                Return _CompBudCogsBudgetMo8
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsBudgetMo8 = value
            End Set
        End Property

        Private _CompBudCogsBudgetMo9 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsBudgetMo9() As Decimal
            Get
                Return _CompBudCogsBudgetMo9
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsBudgetMo9 = value
            End Set
        End Property

        Private _CompBudCogsBudgetMo10 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsBudgetMo10() As Decimal
            Get
                Return _CompBudCogsBudgetMo10
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsBudgetMo10 = value
            End Set
        End Property

        Private _CompBudCogsBudgetMo11 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsBudgetMo11() As Decimal
            Get
                Return _CompBudCogsBudgetMo11
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsBudgetMo11 = value
            End Set
        End Property

        Private _CompBudCogsBudgetMo12 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsBudgetMo12() As Decimal
            Get
                Return _CompBudCogsBudgetMo12
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsBudgetMo12 = value
            End Set
        End Property

        Private _CompBudCogsBudgetMoTotal As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsBudgetMoTotal() As Decimal
            Get
                Return _CompBudCogsBudgetMoTotal
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsBudgetMoTotal = value
            End Set
        End Property

        Private _CompBudCogsActualMo1 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsActualMo1() As Decimal
            Get
                Return _CompBudCogsActualMo1
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsActualMo1 = value
            End Set
        End Property

        Private _CompBudCogsActualMo2 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsActualMo2() As Decimal
            Get
                Return _CompBudCogsActualMo2
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsActualMo2 = value
            End Set
        End Property

        Private _CompBudCogsActualMo3 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsActualMo3() As Decimal
            Get
                Return _CompBudCogsActualMo3
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsActualMo3 = value
            End Set
        End Property

        Private _CompBudCogsActualMo4 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsActualMo4() As Decimal
            Get
                Return _CompBudCogsActualMo4
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsActualMo4 = value
            End Set
        End Property

        Private _CompBudCogsActualMo5 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsActualMo5() As Decimal
            Get
                Return _CompBudCogsActualMo5
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsActualMo5 = value
            End Set
        End Property

        Private _CompBudCogsActualMo6 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsActualMo6() As Decimal
            Get
                Return _CompBudCogsActualMo6
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsActualMo6 = value
            End Set
        End Property

        Private _CompBudCogsActualMo7 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsActualMo7() As Decimal
            Get
                Return _CompBudCogsActualMo7
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsActualMo7 = value
            End Set
        End Property

        Private _CompBudCogsActualMo8 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsActualMo8() As Decimal
            Get
                Return _CompBudCogsActualMo8
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsActualMo8 = value
            End Set
        End Property

        Private _CompBudCogsActualMo9 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsActualMo9() As Decimal
            Get
                Return _CompBudCogsActualMo9
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsActualMo9 = value
            End Set
        End Property

        Private _CompBudCogsActualMo10 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsActualMo10() As Decimal
            Get
                Return _CompBudCogsActualMo10
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsActualMo10 = value
            End Set
        End Property

        Private _CompBudCogsActualMo11 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsActualMo11() As Decimal
            Get
                Return _CompBudCogsActualMo11
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsActualMo11 = value
            End Set
        End Property

        Private _CompBudCogsActualMo12 As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsActualMo12() As Decimal
            Get
                Return _CompBudCogsActualMo12
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsActualMo12 = value
            End Set
        End Property

        Private _CompBudCogsActualMoTotal As Decimal = 0
        <DataMember()> _
        Public Property CompBudCogsActualMoTotal() As Decimal
            Get
                Return _CompBudCogsActualMoTotal
            End Get
            Set(ByVal value As Decimal)
                _CompBudCogsActualMoTotal = value
            End Set
        End Property

        Private _CompBudCogsMarginBudget As Double = 0
        <DataMember()> _
        Public Property CompBudCogsMarginBudget() As Double
            Get
                Return _CompBudCogsMarginBudget
            End Get
            Set(ByVal value As Double)
                _CompBudCogsMarginBudget = value
            End Set
        End Property

        Private _CompBudCogsMarginActual As Double = 0
        <DataMember()> _
        Public Property CompBudCogsMarginActual() As Double
            Get
                Return _CompBudCogsMarginActual
            End Get
            Set(ByVal value As Double)
                _CompBudCogsMarginActual = value
            End Set
        End Property

        Private _CompBudProfitBudgetMo1 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitBudgetMo1() As Decimal
            Get
                Return _CompBudProfitBudgetMo1
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitBudgetMo1 = value
            End Set
        End Property

        Private _CompBudProfitBudgetMo2 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitBudgetMo2() As Decimal
            Get
                Return _CompBudProfitBudgetMo2
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitBudgetMo2 = value
            End Set
        End Property

        Private _CompBudProfitBudgetMo3 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitBudgetMo3() As Decimal
            Get
                Return _CompBudProfitBudgetMo3
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitBudgetMo3 = value
            End Set
        End Property

        Private _CompBudProfitBudgetMo4 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitBudgetMo4() As Decimal
            Get
                Return _CompBudProfitBudgetMo4
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitBudgetMo4 = value
            End Set
        End Property

        Private _CompBudProfitBudgetMo5 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitBudgetMo5() As Decimal
            Get
                Return _CompBudProfitBudgetMo5
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitBudgetMo5 = value
            End Set
        End Property

        Private _CompBudProfitBudgetMo6 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitBudgetMo6() As Decimal
            Get
                Return _CompBudProfitBudgetMo6
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitBudgetMo6 = value
            End Set
        End Property

        Private _CompBudProfitBudgetMo7 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitBudgetMo7() As Decimal
            Get
                Return _CompBudProfitBudgetMo7
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitBudgetMo7 = value
            End Set
        End Property

        Private _CompBudProfitBudgetMo8 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitBudgetMo8() As Decimal
            Get
                Return _CompBudProfitBudgetMo8
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitBudgetMo8 = value
            End Set
        End Property

        Private _CompBudProfitBudgetMo9 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitBudgetMo9() As Decimal
            Get
                Return _CompBudProfitBudgetMo9
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitBudgetMo9 = value
            End Set
        End Property

        Private _CompBudProfitBudgetMo10 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitBudgetMo10() As Decimal
            Get
                Return _CompBudProfitBudgetMo10
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitBudgetMo10 = value
            End Set
        End Property

        Private _CompBudProfitBudgetMo11 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitBudgetMo11() As Decimal
            Get
                Return _CompBudProfitBudgetMo11
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitBudgetMo11 = value
            End Set
        End Property

        Private _CompBudProfitBudgetMo12 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitBudgetMo12() As Decimal
            Get
                Return _CompBudProfitBudgetMo12
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitBudgetMo12 = value
            End Set
        End Property

        Private _CompBudProfitBudgetMoTotal As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitBudgetMoTotal() As Decimal
            Get
                Return _CompBudProfitBudgetMoTotal
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitBudgetMoTotal = value
            End Set
        End Property

        Private _CompBudProfitActualMo1 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitActualMo1() As Decimal
            Get
                Return _CompBudProfitActualMo1
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitActualMo1 = value
            End Set
        End Property

        Private _CompBudProfitActualMo2 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitActualMo2() As Decimal
            Get
                Return _CompBudProfitActualMo2
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitActualMo2 = value
            End Set
        End Property

        Private _CompBudProfitActualMo3 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitActualMo3() As Decimal
            Get
                Return _CompBudProfitActualMo3
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitActualMo3 = value
            End Set
        End Property

        Private _CompBudProfitActualMo4 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitActualMo4() As Decimal
            Get
                Return _CompBudProfitActualMo4
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitActualMo4 = value
            End Set
        End Property

        Private _CompBudProfitActualMo5 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitActualMo5() As Decimal
            Get
                Return _CompBudProfitActualMo5
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitActualMo5 = value
            End Set
        End Property

        Private _CompBudProfitActualMo6 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitActualMo6() As Decimal
            Get
                Return _CompBudProfitActualMo6
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitActualMo6 = value
            End Set
        End Property

        Private _CompBudProfitActualMo7 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitActualMo7() As Decimal
            Get
                Return _CompBudProfitActualMo7
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitActualMo7 = value
            End Set
        End Property

        Private _CompBudProfitActualMo8 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitActualMo8() As Decimal
            Get
                Return _CompBudProfitActualMo8
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitActualMo8 = value
            End Set
        End Property

        Private _CompBudProfitActualMo9 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitActualMo9() As Decimal
            Get
                Return _CompBudProfitActualMo9
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitActualMo9 = value
            End Set
        End Property

        Private _CompBudProfitActualMo10 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitActualMo10() As Decimal
            Get
                Return _CompBudProfitActualMo10
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitActualMo10 = value
            End Set
        End Property

        Private _CompBudProfitActualMo11 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitActualMo11() As Decimal
            Get
                Return _CompBudProfitActualMo11
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitActualMo11 = value
            End Set
        End Property

        Private _CompBudProfitActualMo12 As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitActualMo12() As Decimal
            Get
                Return _CompBudProfitActualMo12
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitActualMo12 = value
            End Set
        End Property

        Private _CompBudProfitActualMoTotal As Decimal = 0
        <DataMember()> _
        Public Property CompBudProfitActualMoTotal() As Decimal
            Get
                Return _CompBudProfitActualMoTotal
            End Get
            Set(ByVal value As Decimal)
                _CompBudProfitActualMoTotal = value
            End Set
        End Property

        Private _CompBudProfitMarginBudget As Double = 0
        <DataMember()> _
        Public Property CompBudProfitMarginBudget() As Double
            Get
                Return _CompBudProfitMarginBudget
            End Get
            Set(ByVal value As Double)
                _CompBudProfitMarginBudget = value
            End Set
        End Property

        Private _CompBudProfitMarginActual As Double = 0
        <DataMember()> _
        Public Property CompBudProfitMarginActual() As Double
            Get
                Return _CompBudProfitMarginActual
            End Get
            Set(ByVal value As Double)
                _CompBudProfitMarginActual = value
            End Set
        End Property

        Private _CompFinDuns As String = ""
        <DataMember()> _
        Public Property CompFinDuns() As String
            Get
                Return Left(_CompFinDuns, 11)
            End Get
            Set(ByVal value As String)
                _CompFinDuns = Left(value, 11)
            End Set
        End Property

        Private _CompFinTaxID As String = ""
        <DataMember()> _
        Public Property CompFinTaxID() As String
            Get
                Return Left(_CompFinTaxID, 20)
            End Get
            Set(ByVal value As String)
                _CompFinTaxID = Left(value, 20)
            End Set
        End Property

        Private _CompFinPaymentForm As String = ""
        <DataMember()> _
        Public Property CompFinPaymentForm() As String
            Get
                Return Left(_CompFinPaymentForm, 50)
            End Get
            Set(ByVal value As String)
                _CompFinPaymentForm = Left(value, 50)
            End Set
        End Property

        Private _CompFinSIC As String = ""
        <DataMember()> _
        Public Property CompFinSIC() As String
            Get
                Return Left(_CompFinSIC, 8)
            End Get
            Set(ByVal value As String)
                _CompFinSIC = Left(value, 8)
            End Set
        End Property

        Private _CompFinPaymentDiscount As Short = 0
        <DataMember()> _
        Public Property CompFinPaymentDiscount() As Short
            Get
                Return _CompFinPaymentDiscount
            End Get
            Set(ByVal value As Short)
                _CompFinPaymentDiscount = value
            End Set
        End Property

        Private _CompFinPaymentDays As Short = 0
        <DataMember()> _
        Public Property CompFinPaymentDays() As Short
            Get
                Return _CompFinPaymentDays
            End Get
            Set(ByVal value As Short)
                _CompFinPaymentDays = value
            End Set
        End Property

        Private _CompFinPaymentNetDays As Short = 0
        <DataMember()> _
        Public Property CompFinPaymentNetDays() As Short
            Get
                Return _CompFinPaymentNetDays
            End Get
            Set(ByVal value As Short)
                _CompFinPaymentNetDays = value
            End Set
        End Property

        Private _CompFinCommTerms As String = ""
        <DataMember()> _
        Public Property CompFinCommTerms() As String
            Get
                Return Left(_CompFinCommTerms, 15)
            End Get
            Set(ByVal value As String)
                _CompFinCommTerms = Left(value, 15)
            End Set
        End Property

        Private _CompFinCommTermsPer As Double = 0
        <DataMember()> _
        Public Property CompFinCommTermsPer() As Double
            Get
                Return _CompFinCommTermsPer
            End Get
            Set(ByVal value As Double)
                _CompFinCommTermsPer = value
            End Set
        End Property



        Private _CompFinCommCompControl As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property CompFinCommCompControl() As System.Nullable(Of Integer)
            Get
                Return _CompFinCommCompControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _CompFinCommCompControl = value
            End Set
        End Property

        Private _CompFinCreditLimit As Integer = 0
        <DataMember()> _
        Public Property CompFinCreditLimit() As Integer
            Get
                Return _CompFinCreditLimit
            End Get
            Set(ByVal value As Integer)
                _CompFinCreditLimit = value
            End Set
        End Property


        Private _CompFinCreditUsed As Integer = 0
        <DataMember()> _
        Public Property CompFinCreditUsed() As Integer
            Get
                Return _CompFinCreditUsed
            End Get
            Set(ByVal value As Integer)
                _CompFinCreditUsed = value
            End Set
        End Property

        Private _CompFinCreditAvail As Integer = 0
        <DataMember()> _
        Public Property CompFinCreditAvail() As Integer
            Get
                Return _CompFinCreditAvail
            End Get
            Set(ByVal value As Integer)
                _CompFinCreditAvail = value
            End Set
        End Property

        Private _CompFinYTDbookedCurr As Double = 0
        <DataMember()> _
        Public Property CompFinYTDbookedCurr() As Double
            Get
                Return _CompFinYTDbookedCurr
            End Get
            Set(ByVal value As Double)
                _CompFinYTDbookedCurr = value
            End Set
        End Property

        Private _CompFinYTDbookedLast As Double = 0
        <DataMember()> _
        Public Property CompFinYTDbookedLast() As Double
            Get
                Return _CompFinYTDbookedLast
            End Get
            Set(ByVal value As Double)
                _CompFinYTDbookedLast = value
            End Set
        End Property

        Private _CompFinYTDcarrierCurr As Double = 0
        <DataMember()> _
        Public Property CompFinYTDcarrierCurr() As Double
            Get
                Return _CompFinYTDcarrierCurr
            End Get
            Set(ByVal value As Double)
                _CompFinYTDcarrierCurr = value
            End Set
        End Property

        Private _CompFinYTDcarrierLast As Double = 0
        <DataMember()> _
        Public Property CompFinYTDcarrierLast() As Double
            Get
                Return _CompFinYTDcarrierLast
            End Get
            Set(ByVal value As Double)
                _CompFinYTDcarrierLast = value
            End Set
        End Property

        Private _CompFinYTDsavingsCurr As Double = 0
        <DataMember()> _
        Public Property CompFinYTDsavingsCurr() As Double
            Get
                Return _CompFinYTDsavingsCurr
            End Get
            Set(ByVal value As Double)
                _CompFinYTDsavingsCurr = value
            End Set
        End Property

        Private _CompFinYTDsavingsLast As Double = 0
        <DataMember()> _
        Public Property CompFinYTDsavingsLast() As Double
            Get
                Return _CompFinYTDsavingsLast
            End Get
            Set(ByVal value As Double)
                _CompFinYTDsavingsLast = value
            End Set
        End Property

        Private _CompFinYTDRevenuesCur As Integer = 0
        <DataMember()> _
        Public Property CompFinYTDRevenuesCur() As Integer
            Get
                Return _CompFinYTDRevenuesCur
            End Get
            Set(ByVal value As Integer)
                _CompFinYTDRevenuesCur = value
            End Set
        End Property

        Private _CompFinYTDRevenuesLast As Integer = 0
        <DataMember()> _
        Public Property CompFinYTDRevenuesLast() As Integer
            Get
                Return _CompFinYTDRevenuesLast
            End Get
            Set(ByVal value As Integer)
                _CompFinYTDRevenuesLast = value
            End Set
        End Property

        Private _CompFinYTDnoLoadsCurr As Integer = 0
        <DataMember()> _
        Public Property CompFinYTDnoLoadsCurr() As Integer
            Get
                Return _CompFinYTDnoLoadsCurr
            End Get
            Set(ByVal value As Integer)
                _CompFinYTDnoLoadsCurr = value
            End Set
        End Property

        Private _CompFinYTDnoLoadsLast As Integer = 0
        <DataMember()> _
        Public Property CompFinYTDnoLoadsLast() As Integer
            Get
                Return _CompFinYTDnoLoadsLast
            End Get
            Set(ByVal value As Integer)
                _CompFinYTDnoLoadsLast = value
            End Set
        End Property

        Private _CompFinInvPrnCode As Boolean = False
        <DataMember()> _
        Public Property CompFinInvPrnCode() As Boolean
            Get
                Return _CompFinInvPrnCode
            End Get
            Set(ByVal value As Boolean)
                _CompFinInvPrnCode = value
            End Set
        End Property

        Private _CompFinInvEMailCode As Boolean = False
        <DataMember()> _
        Public Property CompFinInvEMailCode() As Boolean
            Get
                Return _CompFinInvEMailCode
            End Get
            Set(ByVal value As Boolean)
                _CompFinInvEMailCode = value
            End Set
        End Property

        Private _CompFinCurType As Integer = 0
        <DataMember()> _
        Public Property CompFinCurType() As Integer
            Get
                Return _CompFinCurType
            End Get
            Set(ByVal value As Integer)
                _CompFinCurType = value
            End Set
        End Property

        Private _CompFinUser1 As Integer = 0
        <DataMember()> _
        Public Property CompFinUser1() As Integer
            Get
                Return _CompFinUser1
            End Get
            Set(ByVal value As Integer)
                _CompFinUser1 = value
            End Set
        End Property

        Private _CompFinUser2 As Integer = 0
        <DataMember()> _
        Public Property CompFinUser2() As Integer
            Get
                Return _CompFinUser2
            End Get
            Set(ByVal value As Integer)
                _CompFinUser2 = value
            End Set
        End Property

        Private _CompFinUser3 As Integer = 0
        <DataMember()> _
        Public Property CompFinUser3() As Integer
            Get
                Return _CompFinUser3
            End Get
            Set(ByVal value As Integer)
                _CompFinUser3 = value
            End Set
        End Property

        Private _CompFinUser4 As String = ""
        <DataMember()> _
        Public Property CompFinUser4() As String
            Get
                Return Left(_CompFinUser4, 50)
            End Get
            Set(ByVal value As String)
                _CompFinUser4 = Left(value, 50)
            End Set
        End Property

        Private _CompFinUser5 As String = ""
        <DataMember()> _
        Public Property CompFinUser5() As String
            Get
                Return Left(_CompFinUser5, 50)
            End Get
            Set(ByVal value As String)
                _CompFinUser5 = Left(value, 50)
            End Set
        End Property

        Private _CompFinCustomerSince As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CompFinCustomerSince() As System.Nullable(Of Date)
            Get
                Return _CompFinCustomerSince
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CompFinCustomerSince = value
            End Set
        End Property

        Private _CompFinCardType As String = ""
        <DataMember()> _
        Public Property CompFinCardType() As String
            Get
                Return Left(_CompFinCardType, 50)
            End Get
            Set(ByVal value As String)
                _CompFinCardType = Left(value, 50)
            End Set
        End Property

        Private _CompFinCardName As String = ""
        <DataMember()> _
        Public Property CompFinCardName() As String
            Get
                Return Left(_CompFinCardName, 50)
            End Get
            Set(ByVal value As String)
                _CompFinCardName = Left(value, 50)
            End Set
        End Property

        Private _CompFinCardExpires As String = ""
        <DataMember()> _
        Public Property CompFinCardExpires() As String
            Get
                Return Left(_CompFinCardExpires, 50)
            End Get
            Set(ByVal value As String)
                _CompFinCardExpires = Left(value, 50)
            End Set
        End Property

        Private _CompFinCardAuthorizor As String = ""
        <DataMember()> _
        Public Property CompFinCardAuthorizor() As String
            Get
                Return Left(_CompFinCardAuthorizor, 50)
            End Get
            Set(ByVal value As String)
                _CompFinCardAuthorizor = Left(value, 50)
            End Set
        End Property

        Private _CompFinCardAuthPassword As String = ""
        <DataMember()> _
        Public Property CompFinCardAuthPassword() As String
            Get
                Return Left(_CompFinCardAuthPassword, 50)
            End Get
            Set(ByVal value As String)
                _CompFinCardAuthPassword = Left(value, 50)
            End Set
        End Property

        Private _CompFinUseImportFrtCost As Boolean = False
        <DataMember()> _
        Public Property CompFinUseImportFrtCost() As Boolean
            Get
                Return _CompFinUseImportFrtCost
            End Get
            Set(ByVal value As Boolean)
                _CompFinUseImportFrtCost = value
            End Set
        End Property

        Private _CompFinBkhlFlatFee As Decimal = 0
        <DataMember()> _
        Public Property CompFinBkhlFlatFee() As Decimal
            Get
                Return _CompFinBkhlFlatFee
            End Get
            Set(ByVal value As Decimal)
                _CompFinBkhlFlatFee = value
            End Set
        End Property

        Private _CompFinBkhlCostPerc As Double = 0
        <DataMember()> _
        Public Property CompFinBkhlCostPerc() As Double
            Get
                Return _CompFinBkhlCostPerc
            End Get
            Set(ByVal value As Double)
                _CompFinBkhlCostPerc = value
            End Set
        End Property

        Private _CompLatitude As Double = 0
        <DataMember()> _
        Public Property CompLatitude() As Double
            Get
                Return _CompLatitude
            End Get
            Set(ByVal value As Double)
                _CompLatitude = value
            End Set
        End Property

        Private _CompLongitude As Double = 0
        <DataMember()> _
        Public Property CompLongitude() As Double
            Get
                Return _CompLongitude
            End Get
            Set(ByVal value As Double)
                _CompLongitude = value
            End Set
        End Property

        Private _CompMailTo As String = ""
        <DataMember()> _
        Public Property CompMailTo() As String
            Get
                Return Left(_CompMailTo, 500)
            End Get
            Set(ByVal value As String)
                _CompMailTo = Left(value, 500)
            End Set
        End Property

        Private _CompFDAShipID As String = ""
        <DataMember()> _
        Public Property CompFDAShipID() As String
            Get
                Return Left(_CompFDAShipID, 50)
            End Get
            Set(ByVal value As String)
                _CompFDAShipID = Left(value, 50)
            End Set
        End Property

        Private _CompAMS As Boolean = False
        <DataMember()> _
        Public Property CompAMS() As Boolean
            Get
                Return _CompAMS
            End Get
            Set(ByVal value As Boolean)
                _CompAMS = value
            End Set
        End Property

        Private _CompPayTolPerLo As Double = 0
        <DataMember()> _
        Public Property CompPayTolPerLo() As Double
            Get
                Return _CompPayTolPerLo
            End Get
            Set(ByVal value As Double)
                _CompPayTolPerLo = value
            End Set
        End Property

        Private _CompPayTolPerHi As Double = 0
        <DataMember()> _
        Public Property CompPayTolPerHi() As Double
            Get
                Return _CompPayTolPerHi
            End Get
            Set(ByVal value As Double)
                _CompPayTolPerHi = value
            End Set
        End Property

        Private _CompPayTolCurLo As Double = 0
        <DataMember()> _
        Public Property CompPayTolCurLo() As Double
            Get
                Return _CompPayTolCurLo
            End Get
            Set(ByVal value As Double)
                _CompPayTolCurLo = value
            End Set
        End Property

        Private _CompPayTolCurHi As Double = 0
        <DataMember()> _
        Public Property CompPayTolCurHi() As Double
            Get
                Return _CompPayTolCurHi
            End Get
            Set(ByVal value As Double)
                _CompPayTolCurHi = value
            End Set
        End Property

        Private _CompPayTolWgtFrom As Integer = 0
        <DataMember()> _
        Public Property CompPayTolWgtFrom() As Integer
            Get
                Return _CompPayTolWgtFrom
            End Get
            Set(ByVal value As Integer)
                _CompPayTolWgtFrom = value
            End Set
        End Property

        Private _CompPayTolWgtTo As Integer = 0
        <DataMember()> _
        Public Property CompPayTolWgtTo() As Integer
            Get
                Return _CompPayTolWgtTo
            End Get
            Set(ByVal value As Integer)
                _CompPayTolWgtTo = value
            End Set
        End Property

        Private _CompPayTolTaxPerLo As Double = 0
        <DataMember()> _
        Public Property CompPayTolTaxPerLo() As Double
            Get
                Return _CompPayTolTaxPerLo
            End Get
            Set(ByVal value As Double)
                _CompPayTolTaxPerLo = value
            End Set
        End Property

        Private _CompPayTolTaxPerHi As Double = 0
        <DataMember()> _
        Public Property CompPayTolTaxPerHi() As Double
            Get
                Return _CompPayTolTaxPerHi
            End Get
            Set(ByVal value As Double)
                _CompPayTolTaxPerHi = value
            End Set
        End Property

        Private _CompFinBillToCompControl As Integer = 0
        <DataMember()> _
        Public Property CompFinBillToCompControl() As Integer
            Get
                Return _CompFinBillToCompControl
            End Get
            Set(ByVal value As Integer)
                _CompFinBillToCompControl = value
            End Set
        End Property

        Private _CompSilentTender As Boolean = False
        <DataMember()> _
        Public Property CompSilentTender() As Boolean
            Get
                Return _CompSilentTender
            End Get
            Set(ByVal value As Boolean)
                _CompSilentTender = value
            End Set
        End Property

        Private _CompTimeZone As String = ""
        <DataMember()> _
        Public Property CompTimeZone() As String
            Get
                Return Left(_CompTimeZone, 100)
            End Get
            Set(ByVal value As String)
                _CompTimeZone = Left(value, 100)
            End Set
        End Property

        Private _CompRailStationName As String = ""
        <DataMember()> _
        Public Property CompRailStationName() As String
            Get
                Return Left(Me._CompRailStationName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CompRailStationName, value) = False) Then
                    Me._CompRailStationName = Left(value, 50)
                    Me.SendPropertyChanged("CompRailStationName")
                End If
            End Set
        End Property

        Private _CompRailSPLC As String = ""
        <DataMember()> _
        Public Property CompRailSPLC() As String
            Get
                Return Left(Me._CompRailSPLC, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CompRailSPLC, value) = False) Then
                    Me._CompRailSPLC = Left(value, 50)
                    Me.SendPropertyChanged("CompRailSPLC")
                End If
            End Set
        End Property

        Private _CompRailFSAC As String = ""
        <DataMember()> _
        Public Property CompRailFSAC() As String
            Get
                Return Left(Me._CompRailFSAC, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CompRailFSAC, value) = False) Then
                    Me._CompRailFSAC = Left(value, 50)
                    Me.SendPropertyChanged("CompRailFSAC")
                End If
            End Set
        End Property

        Private _CompRail333 As String = ""
        <DataMember()> _
        Public Property CompRail333() As String
            Get
                Return Left(Me._CompRail333, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CompRail333, value) = False) Then
                    Me._CompRail333 = Left(value, 50)
                    Me.SendPropertyChanged("CompRail333")
                End If
            End Set
        End Property

        Private _CompRailR260 As String = ""
        <DataMember()> _
        Public Property CompRailR260() As String
            Get
                Return Left(Me._CompRailR260, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._CompRailR260, value) = False) Then
                    Me._CompRailR260 = Left(value, 50)
                    Me.SendPropertyChanged("CompRailR260")
                End If
            End Set
        End Property

        Private _CompIsTransLoad As Boolean = False
        <DataMember()> _
        Public Property CompIsTransLoad() As Boolean
            Get
                Return Me._CompIsTransLoad
            End Get
            Set(value As Boolean)
                If ((Me._CompIsTransLoad = value) _
                            = False) Then
                    Me._CompIsTransLoad = value
                    Me.SendPropertyChanged("CompIsTransLoad")
                End If
            End Set
        End Property

        Private _CompUser1 As String = ""
        <DataMember()> _
        Public Property CompUser1() As String
            Get
                Return Left(Me._CompUser1, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._CompUser1, value) = False) Then
                    Me._CompUser1 = Left(value, 4000)
                    Me.SendPropertyChanged("CompUser1")
                End If
            End Set
        End Property

        Private _CompUser2 As String = ""
        <DataMember()> _
        Public Property CompUser2() As String
            Get
                Return Left(Me._CompUser2, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._CompUser2, value) = False) Then
                    Me._CompUser2 = Left(value, 4000)
                    Me.SendPropertyChanged("CompUser2")
                End If
            End Set
        End Property

        Private _CompUser3 As String = ""
        <DataMember()> _
        Public Property CompUser3() As String
            Get
                Return Left(Me._CompUser3, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._CompUser3, value) = False) Then
                    Me._CompUser3 = Left(value, 4000)
                    Me.SendPropertyChanged("CompUser3")
                End If
            End Set
        End Property

        Private _CompUser4 As String = ""
        <DataMember()> _
        Public Property CompUser4() As String
            Get
                Return Left(Me._CompUser4, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._CompUser4, value) = False) Then
                    Me._CompUser4 = Left(value, 4000)
                    Me.SendPropertyChanged("CompUser4")
                End If
            End Set
        End Property

        Private _CompCals As List(Of CompCal)
        <DataMember()> _
        Public Property CompCals() As List(Of CompCal)
            Get
                Return _CompCals
            End Get
            Set(ByVal value As List(Of CompCal))
                _CompCals = value
            End Set
        End Property

        Private _CompConts As List(Of CompCont)
        <DataMember()> _
        Public Property CompConts() As List(Of CompCont)
            Get
                Return _CompConts
            End Get
            Set(ByVal value As List(Of CompCont))
                _CompConts = value
            End Set
        End Property

        Private _CompEDIs As List(Of CompEDI)
        <DataMember()> _
        Public Property CompEDIs() As List(Of CompEDI)
            Get
                Return _CompEDIs
            End Get
            Set(ByVal value As List(Of CompEDI))
                _CompEDIs = value
            End Set
        End Property

        Private _CompGoals As List(Of CompGoal)
        <DataMember()> _
        Public Property CompGoals() As List(Of CompGoal)
            Get
                Return _CompGoals
            End Get
            Set(ByVal value As List(Of CompGoal))
                _CompGoals = value
            End Set
        End Property

        Private _CompTracks As List(Of CompTrack)
        <DataMember()> _
        Public Property CompTracks() As List(Of CompTrack)
            Get
                Return _CompTracks
            End Get
            Set(ByVal value As List(Of CompTrack))
                _CompTracks = value
            End Set
        End Property

        Private _CompParameters As List(Of CompParameter)
        <DataMember()> _
        Public Property CompParameters() As List(Of CompParameter)
            Get
                Return _CompParameters
            End Get
            Set(ByVal value As List(Of CompParameter))
                _CompParameters = value
            End Set
        End Property

        Private _CompAlphaCode As String = ""
        <DataMember()> _
        Public Property CompAlphaCode() As String
            Get
                Return Left(_CompAlphaCode, 50)
            End Get
            Set(ByVal value As String)
                _CompAlphaCode = Left(value, 50)
            End Set
        End Property

        Private _CompLegalEntity As String = ""
        <DataMember()> _
        Public Property CompLegalEntity() As String
            Get
                Return Left(_CompLegalEntity, 50)
            End Get
            Set(ByVal value As String)
                _CompLegalEntity = Left(value, 50)
            End Set
        End Property

        Private _CompFinFBToleranceHigh As Double = 0
        <DataMember()> _
        Public Property CompFinFBToleranceHigh() As Double
            Get
                Return _CompFinFBToleranceHigh
            End Get
            Set(ByVal value As Double)
                _CompFinFBToleranceHigh = value
            End Set
        End Property

        Private _CompFinFBToleranceLow As Double = 0
        <DataMember()> _
        Public Property CompFinFBToleranceLow() As Double
            Get
                Return _CompFinFBToleranceLow
            End Get
            Set(ByVal value As Double)
                _CompFinFBToleranceLow = value
            End Set
        End Property

        Private _CompContName As String = ""
        <DataMember()> _
        Public Property CompContName() As String
            Get
                Return Left(_CompContName, 25)
            End Get
            Set(ByVal value As String)
                _CompContName = Left(value, 25)
            End Set
        End Property

        Private _CompContTitle As String = ""
        <DataMember()> _
        Public Property CompContTitle() As String
            Get
                Return Left(_CompContTitle, 25)
            End Get
            Set(ByVal value As String)
                _CompContTitle = Left(value, 25)
            End Set
        End Property

        Private _CompCont800 As String = ""
        <DataMember()> _
        Public Property CompCont800() As String
            Get
                Return Left(_CompCont800, 50)
            End Get
            Set(ByVal value As String)
                _CompCont800 = Left(value, 50)
            End Set
        End Property

        Private _CompContPhone As String = ""
        <DataMember()> _
        Public Property CompContPhone() As String
            Get
                Return Left(_CompContPhone, 15)
            End Get
            Set(ByVal value As String)
                _CompContPhone = Left(value, 15)
            End Set
        End Property

        Private _CompContPhoneExt As String = ""
        <DataMember()> _
        Public Property CompContPhoneExt() As String
            Get
                Return Left(_CompContPhoneExt, 5)
            End Get
            Set(ByVal value As String)
                _CompContPhoneExt = Left(value, 5)
            End Set
        End Property

        Private _CompContFax As String = ""
        <DataMember()> _
        Public Property CompContFax() As String
            Get
                Return Left(_CompContFax, 15)
            End Get
            Set(ByVal value As String)
                _CompContFax = Left(value, 15)
            End Set
        End Property

        Private _CompContEmail As String = ""
        <DataMember()> _
        Public Property CompContEmail() As String
            Get
                Return Left(_CompContEmail, 50)
            End Get
            Set(ByVal value As String)
                _CompContEmail = Left(value, 50)
            End Set
        End Property

        Private _Month As Integer = 0
        <DataMember()> _
        Public Property Month() As Integer
            Get
                Return _Month
            End Get
            Set(ByVal value As Integer)
                _Month = value
            End Set
        End Property

        Private _Day As Integer = 0
        <DataMember()> _
        Public Property Day() As Integer
            Get
                Return _Day
            End Get
            Set(ByVal value As Integer)
                _Day = value
            End Set
        End Property

        Private _Open As Boolean = True
        <DataMember()> _
        Public Property Open() As Boolean
            Get
                Return _Open
            End Get
            Set(ByVal value As Boolean)
                _Open = value
            End Set
        End Property

        Private _StartTime As String = ""
        <DataMember()> _
        Public Property StartTime() As String
            Get
                Return _StartTime
            End Get
            Set(ByVal value As String)
                _StartTime = value
            End Set
        End Property

        Private _EndTime As String = ""
        <DataMember()> _
        Public Property EndTime() As String
            Get
                Return _EndTime
            End Get
            Set(ByVal value As String)
                _EndTime = value
            End Set
        End Property

        Private _IsHoliday As Boolean = False
        <DataMember()> _
        Public Property IsHoliday() As Boolean
            Get
                Return _IsHoliday
            End Get
            Set(ByVal value As Boolean)
                _IsHoliday = value
            End Set
        End Property

        'Added by LVV 10/25/16 for v-7.0.5.110 Lane Default Carrier Enhancements
        Private _CompRestrictCarrierSelection As Boolean = False
        <DataMember()> _
        Public Property CompRestrictCarrierSelection() As Boolean
            Get
                Return _CompRestrictCarrierSelection
            End Get
            Set(ByVal value As Boolean)
                _CompRestrictCarrierSelection = value
            End Set
        End Property

        'Added by LVV 10/25/16 for v-7.0.5.110 Lane Default Carrier Enhancements
        Private _CompWarnOnRestrictedCarrierSelection As Boolean = False
        <DataMember()>
        Public Property CompWarnOnRestrictedCarrierSelection() As Boolean
            Get
                Return _CompWarnOnRestrictedCarrierSelection
            End Get
            Set(ByVal value As Boolean)
                _CompWarnOnRestrictedCarrierSelection = value
            End Set
        End Property

        'Added by RHR for v-8.0 on 12/14/2017 
        Private _CompNotifyEmail As String
        <DataMember()>
        Public Property CompNotifyEmail() As String
            Get
                Return Left(_CompNotifyEmail, 255)
            End Get
            Set
                _CompNotifyEmail = Left(Value, 255)
            End Set
        End Property

        'Added by RHR for v-8.0 on 12/14/2017 
        Private _CompPhone As String
        <DataMember()>
        Public Property CompPhone() As String
            Get
                Return Left(_CompPhone, 50)
            End Get
            Set
                _CompPhone = Left(Value, 50)
            End Set
        End Property

        'Added by RHR for v-8.0 on 12/14/2017 
        Private _CompFax As String
        <DataMember()>
        Public Property CompFax() As String
            Get
                Return Left(_CompFax, 50)
            End Get
            Set
                _CompFax = Left(Value, 50)
            End Set
        End Property

        'Added by RHR for v-8.0 on 12/14/2017 
        Private _CompCarrierLoadAcceptanceAllowedMinutes As Integer = 0
        <DataMember()>
        Public Property CompCarrierLoadAcceptanceAllowedMinutes() As Integer
            Get
                Return _CompCarrierLoadAcceptanceAllowedMinutes
            End Get
            Set(ByVal value As Integer)
                _CompCarrierLoadAcceptanceAllowedMinutes = value
            End Set
        End Property

        'Added by RHR for v-8.0 on 12/14/2017 
        Private _CompRejectedLoadsTo As String
        <DataMember()>
        Public Property CompRejectedLoadsTo() As String
            Get
                Return Left(_CompRejectedLoadsTo, 255)
            End Get
            Set
                _CompRejectedLoadsTo = Left(Value, 255)
            End Set
        End Property

        'Added by RHR for v-8.0 on 12/14/2017 
        Private _CompRejectedLoadsCc As String
        <DataMember()>
        Public Property CompRejectedLoadsCc() As String
            Get
                Return Left(_CompRejectedLoadsCc, 255)
            End Get
            Set
                _CompRejectedLoadsCc = Left(Value, 255)
            End Set
        End Property

        'Added by RHR for v-8.0 on 12/14/2017 
        Private _CompExpiredLoadsTo As String
        <DataMember()>
        Public Property CompExpiredLoadsTo() As String
            Get
                Return Left(_CompExpiredLoadsTo, 255)
            End Get
            Set
                _CompExpiredLoadsTo = Left(Value, 255)
            End Set
        End Property

        'Added by RHR for v-8.0 on 12/14/2017 
        Private _CompExpiredLoadsCc As String
        <DataMember()>
        Public Property CompExpiredLoadsCc() As String
            Get
                Return Left(_CompExpiredLoadsCc, 255)
            End Get
            Set
                _CompExpiredLoadsCc = Left(Value, 255)
            End Set
        End Property

        'Added by RHR for v-8.0 on 12/14/2017 
        Private _CompAcceptedLoadsTo As String
        <DataMember()>
        Public Property CompAcceptedLoadsTo() As String
            Get
                Return Left(_CompAcceptedLoadsTo, 255)
            End Get
            Set
                _CompAcceptedLoadsTo = Left(Value, 255)
            End Set
        End Property

        'Added by RHR for v-8.0 on 12/14/2017 
        Private _CompAcceptedLoadsCc As String
        <DataMember()>
        Public Property CompAcceptedLoadsCc() As String
            Get
                Return Left(_CompAcceptedLoadsCc, 255)
            End Get
            Set
                _CompAcceptedLoadsCc = Left(Value, 255)
            End Set
        End Property

        'Added by RHR for v-8.0 on 12/14/2017 
        Private _CompHeaderLogo As String
        <DataMember()>
        Public Property CompHeaderLogo() As String
            Get
                Return Left(_CompHeaderLogo, 255)
            End Get
            Set
                _CompHeaderLogo = Left(Value, 255)
            End Set
        End Property

        'Added by RHR for v-8.0 on 12/14/2017 
        Private _CompHeaderLogoLink As String
        <DataMember()>
        Public Property CompHeaderLogoLink() As String
            Get
                Return Left(_CompHeaderLogoLink, 255)
            End Get
            Set
                _CompHeaderLogoLink = Left(Value, 255)
            End Set
        End Property


        'Added By LVV on 4/22/20
        Private _CompAMSApptDetFieldsVisible As Integer = 0
        <DataMember()>
        Public Property CompAMSApptDetFieldsVisible() As Integer
            Get
                Return _CompAMSApptDetFieldsVisible
            End Get
            Set(ByVal value As Integer)
                _CompAMSApptDetFieldsVisible = value
            End Set
        End Property

        'Added by by RHR for v-8.5.3.006 on 12/01/2022
        Private _CompWillLoadOnSunday As Boolean = False
        <DataMember()>
        Public Property CompWillLoadOnSunday() As Boolean
            Get
                Return _CompWillLoadOnSunday
            End Get
            Set(ByVal value As Boolean)
                _CompWillLoadOnSunday = value
            End Set
        End Property

        'Added by by RHR for v-8.5.3.006 on 12/01/2022
        Private _CompWillLoadOnSaturday As Boolean = False
        <DataMember()>
        Public Property CompWillLoadOnSaturday() As Boolean
            Get
                Return _CompWillLoadOnSaturday
            End Get
            Set(ByVal value As Boolean)
                _CompWillLoadOnSaturday = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New Comp
            instance = DirectCast(MemberwiseClone(), Comp)
            instance.CompCals = Nothing
            For Each item In CompCals
                instance.CompCals.Add(DirectCast(item.Clone, CompCal))
            Next
            instance.CompConts = Nothing
            For Each item In CompConts
                instance.CompConts.Add(DirectCast(item.Clone, CompCont))
            Next
            instance.CompEDIs = Nothing
            For Each item In CompEDIs
                instance.CompEDIs.Add(DirectCast(item.Clone, CompEDI))
            Next
            instance.CompGoals = Nothing
            For Each item In CompGoals
                instance.CompGoals.Add(DirectCast(item.Clone, CompGoal))
            Next
            instance.CompTracks = Nothing
            For Each item In CompTracks
                instance.CompTracks.Add(DirectCast(item.Clone, CompTrack))
            Next
            instance.CompParameters = Nothing
            For Each item In CompParameters
                instance.CompParameters.Add(DirectCast(item.Clone, CompParameter))
            Next
            Return instance
        End Function

#End Region

    End Class
End Namespace
