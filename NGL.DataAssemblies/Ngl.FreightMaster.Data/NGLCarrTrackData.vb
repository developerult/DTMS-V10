Imports System.ServiceModel

Public Class NGLCarrTrackData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrTracks
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrTrackData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrTracks
                _LinqDB = db
            End If

            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetCarrTrackFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrTracksFiltered()
    End Function

    Public Function GetCarrTrackFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.CarrTrack
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CarrTrack As DataTransferObjects.CarrTrack = (
                        From t In db.CarrTracks
                        Where
                        (t.CarrTrackControl = If(Control = 0, t.CarrTrackControl, Control))
                        Order By t.CarrTrackControl Descending
                        Select New DataTransferObjects.CarrTrack With {.CarrTrackControl = t.CarrTrackControl _
                        , .CarrTrackCompControl = t.CarrTrackCompControl _
                        , .CarrTrackDate = t.CarrTrackDate _
                        , .CarrTrackContact = t.CarrTrackContact _
                        , .CarrTrackComment = t.CarrTrackComment _
                        , .CarrTrackFollowUpOn = t.CarrTrackFollowUpOn _
                        , .carrTrackFollowUpOnComplete = t.carrTrackFollowUpOnComplete _
                        , .CarrTrackModDate = t.CarrTrackModDate _
                        , .CarrTrackModUser = t.CarrTrackModUser _
                        , .CarrTrackUpdated = t.CarrTrackUpdated.ToArray()}).First
                Return CarrTrack

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrTracksFiltered(Optional ByVal CompControl As Integer = 0) As DataTransferObjects.CarrTrack()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CarrTracks() As DataTransferObjects.CarrTrack = (
                        From t In db.CarrTracks
                        Where
                        (t.CarrTrackCompControl = If(CompControl = 0, t.CarrTrackCompControl, CompControl))
                        Order By t.CarrTrackControl
                        Select New DataTransferObjects.CarrTrack With {.CarrTrackControl = t.CarrTrackControl _
                        , .CarrTrackCompControl = t.CarrTrackCompControl _
                        , .CarrTrackDate = t.CarrTrackDate _
                        , .CarrTrackContact = t.CarrTrackContact _
                        , .CarrTrackComment = t.CarrTrackComment _
                        , .CarrTrackFollowUpOn = t.CarrTrackFollowUpOn _
                        , .carrTrackFollowUpOnComplete = t.carrTrackFollowUpOnComplete _
                        , .CarrTrackModDate = t.CarrTrackModDate _
                        , .CarrTrackModUser = t.CarrTrackModUser _
                        , .CarrTrackUpdated = t.CarrTrackUpdated.ToArray()}).ToArray()
                Return CarrTracks

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.CarrTrack)
        'Create New Record
        Return New LTS.CarrTrack With {.CarrTrackControl = d.CarrTrackControl _
            , .CarrTrackCompControl = d.CarrTrackCompControl _
            , .CarrTrackDate = d.CarrTrackDate _
            , .CarrTrackContact = d.CarrTrackContact _
            , .CarrTrackComment = d.CarrTrackComment _
            , .CarrTrackFollowUpOn = d.CarrTrackFollowUpOn _
            , .carrTrackFollowUpOnComplete = d.carrTrackFollowUpOnComplete _
            , .CarrTrackModDate = Date.Now _
            , .CarrTrackModUser = Parameters.UserName _
            , .CarrTrackUpdated = If(d.CarrTrackUpdated Is Nothing, New Byte() {}, d.CarrTrackUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrTrackFiltered(Control:=CType(LinqTable, LTS.CarrTrack).CarrTrackControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrTrack = TryCast(LinqTable, LTS.CarrTrack)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrTracks
                    Where d.CarrTrackControl = source.CarrTrackControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrTrackControl _
                        , .ModDate = d.CarrTrackModDate _
                        , .ModUser = d.CarrTrackModUser _
                        , .Updated = d.CarrTrackUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function


#End Region

End Class