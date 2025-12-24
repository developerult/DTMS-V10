Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookCarrier
        Inherits DTOBaseClass


#Region " Data Members"

        Private _BookControl As Integer = 0
        <DataMember()> _
        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property

        Private _BookModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookModDate() As System.Nullable(Of Date)
            Get
                Return _BookModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookModDate = value
            End Set
        End Property

        Private _BookModUser As String = ""
        <DataMember()> _
        Public Property BookModUser() As String
            Get
                Return Left(_BookModUser, 100)
            End Get
            Set(ByVal value As String)
                _BookModUser = Left(value, 100)
            End Set
        End Property

        Private _BookCarrFBNumber As String = ""
        <DataMember()> _
        Public Property BookCarrFBNumber() As String
            Get
                Return Left(_BookCarrFBNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookCarrFBNumber = Left(value, 20)
            End Set
        End Property

        Private _BookCarrOrderNumber As String = ""
        <DataMember()> _
        Public Property BookCarrOrderNumber() As String
            Get
                Return Left(_BookCarrOrderNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookCarrOrderNumber = Left(value, 20)
            End Set
        End Property

        Private _BookCarrBLNumber As String = ""
        <DataMember()> _
        Public Property BookCarrBLNumber() As String
            Get
                Return Left(_BookCarrBLNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookCarrBLNumber = Left(value, 20)
            End Set
        End Property

        Private _BookCarrBookDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrBookDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrBookDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrBookDate = value
            End Set
        End Property

        Private _BookCarrBookTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrBookTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrBookTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrBookTime = value
            End Set
        End Property

        Private _BookCarrBookContact As String = ""
        <DataMember()> _
        Public Property BookCarrBookContact() As String
            Get
                Return Left(_BookCarrBookContact, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrBookContact = Left(value, 50)
            End Set
        End Property

        Private _BookCarrScheduleDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrScheduleDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrScheduleDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrScheduleDate = value
            End Set
        End Property

        Private _BookCarrScheduleTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrScheduleTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrScheduleTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrScheduleTime = value
            End Set
        End Property

        Private _BookCarrActualDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrActualDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrActualDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActualDate = value
            End Set
        End Property

        Private _BookCarrActualTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrActualTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrActualTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActualTime = value
            End Set
        End Property

        Private _BookCarrActLoadComplete_Date As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrActLoadComplete_Date() As System.Nullable(Of Date)
            Get
                Return _BookCarrActLoadComplete_Date
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActLoadComplete_Date = value
            End Set
        End Property

        Private _BookCarrActLoadCompleteTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrActLoadCompleteTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrActLoadCompleteTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActLoadCompleteTime = value
            End Set
        End Property

        Private _BookCarrDockPUAssigment As String = ""
        <DataMember()> _
        Public Property BookCarrDockPUAssigment() As String
            Get
                Return Left(_BookCarrDockPUAssigment, 10)
            End Get
            Set(ByVal value As String)
                _BookCarrDockPUAssigment = Left(value, 10)
            End Set
        End Property

        Private _BookCarrPODate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrPODate() As System.Nullable(Of Date)
            Get
                Return _BookCarrPODate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrPODate = value
            End Set
        End Property

        Private _BookCarrPOTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrPOTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrPOTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrPOTime = value
            End Set
        End Property

        Private _BookCarrApptDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrApptDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrApptDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrApptDate = value
            End Set
        End Property

        Private _BookCarrApptTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrApptTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrApptTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrApptTime = value
            End Set
        End Property

        Private _BookCarrActDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrActDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrActDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActDate = value
            End Set
        End Property

        Private _BookCarrActTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrActTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrActTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActTime = value
            End Set
        End Property

        Private _BookCarrActUnloadCompDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrActUnloadCompDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrActUnloadCompDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActUnloadCompDate = value
            End Set
        End Property

        Private _BookCarrActUnloadCompTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrActUnloadCompTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrActUnloadCompTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActUnloadCompTime = value
            End Set
        End Property

        Private _BookCarrDockDelAssignment As String = ""
        <DataMember()> _
        Public Property BookCarrDockDelAssignment() As String
            Get
                Return Left(_BookCarrDockDelAssignment, 10)
            End Get
            Set(ByVal value As String)
                _BookCarrDockDelAssignment = Left(value, 10)
            End Set
        End Property

        Private _BookCarrVarDay As Integer = 0
        <DataMember()> _
        Public Property BookCarrVarDay() As Integer
            Get
                Return _BookCarrVarDay
            End Get
            Set(ByVal value As Integer)
                _BookCarrVarDay = value
            End Set
        End Property

        Private _BookCarrVarHrs As Integer = 0
        <DataMember()> _
        Public Property BookCarrVarHrs() As Integer
            Get
                Return _BookCarrVarHrs
            End Get
            Set(ByVal value As Integer)
                _BookCarrVarHrs = value
            End Set
        End Property

        Private _BookCarrTrailerNo As String = ""
        <DataMember()> _
        Public Property BookCarrTrailerNo() As String
            Get
                Return Left(_BookCarrTrailerNo, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrTrailerNo = Left(value, 50)
            End Set
        End Property

        Private _BookCarrSealNo As String = ""
        <DataMember()> _
        Public Property BookCarrSealNo() As String
            Get
                Return Left(_BookCarrSealNo, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrSealNo = Left(value, 50)
            End Set
        End Property

        Private _BookCarrDriverNo As String = ""
        <DataMember()> _
        Public Property BookCarrDriverNo() As String
            Get
                Return Left(_BookCarrDriverNo, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrDriverNo = Left(value, 50)
            End Set
        End Property

        Private _BookCarrDriverName As String = ""
        <DataMember()> _
        Public Property BookCarrDriverName() As String
            Get
                Return Left(_BookCarrDriverName, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrDriverName = Left(value, 50)
            End Set
        End Property

        Private _BookCarrRouteNo As String = ""
        <DataMember()> _
        Public Property BookCarrRouteNo() As String
            Get
                Return Left(_BookCarrRouteNo, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrRouteNo = Left(value, 50)
            End Set
        End Property

        Private _BookCarrTripNo As String = ""
        <DataMember()> _
        Public Property BookCarrTripNo() As String
            Get
                Return Left(_BookCarrTripNo, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrTripNo = Left(value, 50)
            End Set
        End Property

        Private _BookWhseAuthorizationNo As String = ""
        <DataMember()> _
        Public Property BookWhseAuthorizationNo() As String
            Get
                Return Left(_BookWhseAuthorizationNo, 20)
            End Get
            Set(ByVal value As String)
                _BookWhseAuthorizationNo = Left(value, 20)
            End Set
        End Property

        Private _BookCarrStartLoadingDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrStartLoadingDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrStartLoadingDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrStartLoadingDate = value
            End Set
        End Property

        Private _BookCarrStartLoadingTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrStartLoadingTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrStartLoadingTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrStartLoadingTime = value
            End Set
        End Property

        Private _BookCarrFinishLoadingDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrFinishLoadingDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrFinishLoadingDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrFinishLoadingDate = value
            End Set
        End Property

        Private _BookCarrFinishLoadingTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrFinishLoadingTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrFinishLoadingTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrFinishLoadingTime = value
            End Set
        End Property

        Private _BookCarrStartUnloadingDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrStartUnloadingDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrStartUnloadingDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrStartUnloadingDate = value
            End Set
        End Property

        Private _BookCarrStartUnloadingTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrStartUnloadingTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrStartUnloadingTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrStartUnloadingTime = value
            End Set
        End Property

        Private _BookCarrFinishUnloadingDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrFinishUnloadingDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrFinishUnloadingDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrFinishUnloadingDate = value
            End Set
        End Property

        Private _BookCarrFinishUnloadingTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrFinishUnloadingTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrFinishUnloadingTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrFinishUnloadingTime = value
            End Set
        End Property


        Private _BookAMSPickupApptControl As Integer = 0
        <DataMember()> _
        Public Property BookAMSPickupApptControl As Integer
            Get
                Return _BookAMSPickupApptControl
            End Get
            Set(value As Integer)
                _BookAMSPickupApptControl = value
            End Set
        End Property

        Private _BookAMSDeliveryApptControl As Integer = 0
        <DataMember()>
        Public Property BookAMSDeliveryApptControl As Integer
            Get
                Return _BookAMSDeliveryApptControl
            End Get
            Set(value As Integer)
                _BookAMSDeliveryApptControl = value
            End Set
        End Property

        Private _BookFinAPActWgt As Integer = 0
        <DataMember()>
        Public Property BookFinAPActWgt() As Integer
            Get
                Return _BookFinAPActWgt
            End Get
            Set(ByVal value As Integer)
                _BookFinAPActWgt = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookCarrier
            instance = DirectCast(MemberwiseClone(), BookCarrier)
            Return instance
        End Function

#End Region

    End Class
End Namespace