Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker
Imports Destructurama
Imports Serilog
Namespace DataTransferObjects
    <DataContract()> _
    Public MustInherit Class DTOBaseClass
        Implements System.ComponentModel.INotifyPropertyChanged
        Implements ITrackable
        Implements ICloneable

#Region " Implements "

        Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

        Private _TrackingState As TrackingInfo = TrackingInfo.Unchanged
        <DataMember()> _
        Public Property TrackingState() As Core.ChangeTracker.TrackingInfo Implements Core.ChangeTracker.ITrackable.TrackingState
            Get
                Return _TrackingState
            End Get
            Set(ByVal value As Core.ChangeTracker.TrackingInfo)
                _TrackingState = value
            End Set
        End Property

        Private Function ICloneable_Clone() As Object Implements ICloneable.Clone
            Return Clone()
        End Function

        Public MustOverride Function Clone() As DTOBaseClass

        Protected Sub NotifyPropertyChanged(ByVal info As String)
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(info))
        End Sub

        Protected Sub SendPropertyChanged(ByVal info As String)
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(info))
        End Sub


#End Region

#Region " Server Properties "
        Public Overridable Property Logger as Serilog.ILogger = Log.Logger.ForContext(of DTOBaseClass)

        Private _Parameters As WCFParameters
        Public Property Parameters As WCFParameters
            Get
                Return _Parameters
            End Get
            Set(value As WCFParameters)
                _Parameters = value
                Me.SendPropertyChanged("Parameters")
            End Set
        End Property
#End Region

#Region " Data Members "

        Private _intPage As Integer = 1
        <DataMember()> _
        Public Property Page() As Integer
            Get
                Return _intPage
            End Get
            Set(value As Integer)
                _intPage = value
            End Set
        End Property

        Private _intPages As Integer = 1
        <DataMember()> _
        Public Property Pages() As Integer
            Get
                Return _intPages
            End Get
            Set(value As Integer)
                _intPages = value
            End Set
        End Property

        Private _intRecordCount As Integer = 1
        <DataMember()> _
        Public Property RecordCount() As Integer
            Get
                Return _intRecordCount
            End Get
            Set(value As Integer)
                _intRecordCount = value
            End Set
        End Property

        Private _intPageSize As Integer = 1
        <DataMember()> _
        Public Property PageSize() As Integer
            Get
                Return _intPageSize
            End Get
            Set(value As Integer)
                _intPageSize = value
            End Set
        End Property

#End Region

    End Class

End Namespace
