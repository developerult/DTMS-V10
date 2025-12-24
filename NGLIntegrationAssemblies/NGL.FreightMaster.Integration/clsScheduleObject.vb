<Serializable()> _
Public Class clsScheduleObject
    Public BookCarrOrderNumber As String = ""
    Public BookCarrDockPUAssigment As String = ""
    Public BookCarrScheduleDate As String = ""
    Public BookCarrScheduleTime As String = ""
    Public BookCarrActualDate As String = ""
    Public BookCarrActualTime As String = ""
    Public BookCarrActLoadCompleteDate As String = ""
    Public BookCarrActLoadCompleteTime As String = ""
    Public CompNumber As String = ""
    Public BookPRONumber As String = ""
    Public BookOrderSequence As Integer = 0
    Public BookCarrStartLoadingDate As String = ""
    Public BookCarrStartLoadingTime As String = ""
    Public BookCarrFinishLoadingDate As String = ""
    Public BookCarrFinishLoadingTime As String = ""
    Public BookCarrDockDelAssignment As String = ""
    Public BookCarrApptDate As String = ""
    Public BookCarrApptTime As String = ""
    Public BookCarrActDate As String = ""
    Public BookCarrActTime As String = ""
    Public BookCarrStartUnloadingDate As String = ""
    Public BookCarrStartUnloadingTime As String = ""
    Public BookCarrFinishUnloadingDate As String = ""
    Public BookCarrFinishUnloadingTime As String = ""
    Public BookCarrActUnloadCompDate As String = ""
    Public BookCarrActUnloadCompTime As String = ""
    Public BookShipCarrierName As String = ""
    Public BookShipCarrierProNumber As String = ""
    Public BookShipCarrierNumber As String = ""


End Class

Public Class clsScheduleObject70 : Inherits clsImportDataBase

    Private _BookCarrOrderNumber As String = ""
    Public Property BookCarrOrderNumber As String
        Get
            Return Left(_BookCarrOrderNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookCarrOrderNumber = Left(value, 20)
        End Set
    End Property

    Private _BookCarrDockPUAssigment As String = ""
    Public Property BookCarrDockPUAssigment() As String
        Get
            Return Left(_BookCarrDockPUAssigment, 10)
        End Get
        Set(ByVal value As String)
            _BookCarrDockPUAssigment = Left(value, 10)
        End Set
    End Property

    Private _BookCarrScheduleDate As String = ""
    Public Property BookCarrScheduleDate() As String
        Get
            Return Left(_BookCarrScheduleDate, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrScheduleDate = Left(value, 25)
        End Set
    End Property

    Private _BookCarrScheduleTime As String = ""
    Public Property BookCarrScheduleTime() As String
        Get
            Return Left(_BookCarrScheduleTime, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrScheduleTime = Left(value, 25)
        End Set
    End Property

    Private _BookCarrActualDate As String = ""
    Public Property BookCarrActualDate() As String
        Get
            Return Left(_BookCarrActualDate, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrActualDate = Left(value, 25)
        End Set
    End Property

    Private _BookCarrActualTime As String = ""
    Public Property BookCarrActualTime() As String
        Get
            Return Left(_BookCarrActualTime, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrActualTime = Left(value, 25)
        End Set
    End Property

    Private _BookCarrActLoadCompleteDate As String = ""
    Public Property BookCarrActLoadCompleteDate() As String
        Get
            Return Left(_BookCarrActLoadCompleteDate, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrActLoadCompleteDate = Left(value, 25)
        End Set
    End Property

    Private _BookCarrActLoadCompleteTime As String = ""
    Public Property BookCarrActLoadCompleteTime() As String
        Get
            Return Left(_BookCarrActLoadCompleteTime, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrActLoadCompleteTime = Left(value, 25)
        End Set
    End Property

    Private _CompNumber As String = ""
    Public Property CompNumber() As String
        Get
            Return Left(_CompNumber, 50)
        End Get
        Set(ByVal value As String)
            _CompNumber = Left(value, 50)
        End Set
    End Property

    Private _BookPRONumber As String = ""
    Public Property BookPRONumber() As String
        Get
            Return Left(_BookPRONumber, 20)
        End Get
        Set(ByVal value As String)
            _BookPRONumber = Left(value, 20)
        End Set
    End Property

    Private _BookOrderSequence As Integer = 0
    Public Property BookOrderSequence As Integer
        Get
            Return _BookOrderSequence
        End Get
        Set(value As Integer)
            _BookOrderSequence = value
        End Set
    End Property

    Private _BookCarrStartLoadingDate As String = ""
    Public Property BookCarrStartLoadingDate() As String
        Get
            Return Left(_BookCarrStartLoadingDate, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrStartLoadingDate = Left(value, 25)
        End Set
    End Property

    Private _BookCarrStartLoadingTime As String = ""
    Public Property BookCarrStartLoadingTime() As String
        Get
            Return Left(_BookCarrStartLoadingTime, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrStartLoadingTime = Left(value, 25)
        End Set
    End Property

    Private _BookCarrFinishLoadingDate As String = ""
    Public Property BookCarrFinishLoadingDate() As String
        Get
            Return Left(_BookCarrFinishLoadingDate, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrFinishLoadingDate = Left(value, 25)
        End Set
    End Property

    Private _BookCarrFinishLoadingTime As String = ""
    Public Property BookCarrFinishLoadingTime() As String
        Get
            Return Left(_BookCarrFinishLoadingTime, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrFinishLoadingTime = Left(value, 25)
        End Set
    End Property

    Private _BookCarrDockDelAssignment As String = ""
    Public Property BookCarrDockDelAssignment() As String
        Get
            Return Left(_BookCarrDockDelAssignment, 10)
        End Get
        Set(ByVal value As String)
            _BookCarrDockDelAssignment = Left(value, 10)
        End Set
    End Property

    Private _BookCarrApptDate As String = ""
    Public Property BookCarrApptDate() As String
        Get
            Return Left(_BookCarrApptDate, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrApptDate = Left(value, 25)
        End Set
    End Property

    Private _BookCarrApptTime As String = ""
    Public Property BookCarrApptTime() As String
        Get
            Return Left(_BookCarrApptTime, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrApptTime = Left(value, 25)
        End Set
    End Property

    Private _BookCarrActDate As String = ""
    Public Property BookCarrActDate() As String
        Get
            Return Left(_BookCarrActDate, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrActDate = Left(value, 25)
        End Set
    End Property

    Private _BookCarrActTime As String = ""
    Public Property BookCarrActTime() As String
        Get
            Return Left(_BookCarrActTime, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrActTime = Left(value, 25)
        End Set
    End Property

    Private _BookCarrStartUnloadingDate As String = ""
    Public Property BookCarrStartUnloadingDate() As String
        Get
            Return Left(_BookCarrStartUnloadingDate, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrStartUnloadingDate = Left(value, 25)
        End Set
    End Property

    Private _BookCarrStartUnloadingTime As String = ""
    Public Property BookCarrStartUnloadingTime() As String
        Get
            Return Left(_BookCarrStartUnloadingTime, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrStartUnloadingTime = Left(value, 25)
        End Set
    End Property

    Private _BookCarrFinishUnloadingDate As String = ""
    Public Property BookCarrFinishUnloadingDate() As String
        Get
            Return Left(_BookCarrFinishUnloadingDate, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrFinishUnloadingDate = Left(value, 25)
        End Set
    End Property

    Private _BookCarrFinishUnloadingTime As String = ""
    Public Property BookCarrFinishUnloadingTime() As String
        Get
            Return Left(_BookCarrFinishUnloadingTime, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrFinishUnloadingTime = Left(value, 25)
        End Set
    End Property

    Private _BookCarrActUnloadCompDate As String = ""
    Public Property BookCarrActUnloadCompDate() As String
        Get
            Return Left(_BookCarrActUnloadCompDate, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrActUnloadCompDate = Left(value, 25)
        End Set
    End Property

    Private _BookCarrActUnloadCompTime As String = ""
    Public Property BookCarrActUnloadCompTime() As String
        Get
            Return Left(_BookCarrActUnloadCompTime, 25)
        End Get
        Set(ByVal value As String)
            _BookCarrActUnloadCompTime = Left(value, 25)
        End Set
    End Property

    Private _BookShipCarrierName As String = ""
    Public Property BookShipCarrierName() As String
        Get
            Return Left(_BookShipCarrierName, 60)
        End Get
        Set(ByVal value As String)
            _BookShipCarrierName = Left(value, 60)
        End Set
    End Property

    Private _BookShipCarrierProNumber As String = ""
    Public Property BookShipCarrierProNumber() As String
        Get
            Return Left(_BookShipCarrierProNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookShipCarrierProNumber = Left(value, 20)
        End Set
    End Property

    Private _BookShipCarrierNumber As String = ""
    Public Property BookShipCarrierNumber() As String
        Get
            Return Left(_BookShipCarrierNumber, 80)
        End Get
        Set(ByVal value As String)
            _BookShipCarrierNumber = Left(value, 80)
        End Set
    End Property

    Private _CompLegalEntity As String = ""
    Public Property CompLegalEntity As String
        Get
            Return Left(_CompLegalEntity, 50)
        End Get
        Set(ByVal value As String)
            _CompLegalEntity = Left(value, 50)
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



End Class
