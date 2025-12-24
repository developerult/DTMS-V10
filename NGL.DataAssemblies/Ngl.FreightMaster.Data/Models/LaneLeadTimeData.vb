
Namespace Models

    Public Class LaneLeadTimeData

        Private _LaneControl As Integer = 0
        Public Property LaneControl() As Integer
            Get
                Return _LaneControl
            End Get
            Set(ByVal value As Integer)
                _LaneControl = value
            End Set
        End Property

        Private _LaneOLTBenchmark As Integer? = 0
        Public Property LaneOLTBenchmark() As Integer?
            Get
                Return _LaneOLTBenchmark
            End Get
            Set(ByVal value As Integer?)
                _LaneOLTBenchmark = value
            End Set
        End Property

        Private _LaneTLTBenchmark As Integer? = 0
        Public Property LaneTLTBenchmark() As Integer?
            Get
                Return _LaneTLTBenchmark
            End Get
            Set(ByVal value As Integer?)
                _LaneTLTBenchmark = value
            End Set
        End Property

        Private _LaneTransLeadTimeCalcType As Integer? = 0
        Public Property LaneTransLeadTimeCalcType() As Integer?
            Get
                Return _LaneTransLeadTimeCalcType
            End Get
            Set(ByVal value As Integer?)
                _LaneTransLeadTimeCalcType = value
            End Set
        End Property

        Private _LaneTransLeadTimeUseMasterLane As Boolean? = False
        Public Property LaneTransLeadTimeUseMasterLane() As Boolean?
            Get
                Return _LaneTransLeadTimeUseMasterLane
            End Get
            Set(ByVal value As Boolean?)
                _LaneTransLeadTimeUseMasterLane = value
            End Set
        End Property

        Private _LaneTransLeadTimeLocationOption As Integer? = 0
        Public Property LaneTransLeadTimeLocationOption() As Integer?
            Get
                Return _LaneTransLeadTimeLocationOption
            End Get
            Set(ByVal value As Integer?)
                _LaneTransLeadTimeLocationOption = value
            End Set
        End Property



    End Class


End Namespace
