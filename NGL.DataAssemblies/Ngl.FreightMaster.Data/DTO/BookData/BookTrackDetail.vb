Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

'Added By LVV on 9/17/19 for Bing Maps

Namespace DataTransferObjects
    <DataContract()>
    Public Class BookTrackDetail
        Inherits DTOBaseClass

#Region " Data Members"

        Private _BookTrackDetailControl As Integer = 0
        Private _BookTrackDetailBookTrackControl As Integer = 0
        Private _BookTrackDetailStopControl As Integer = 0
        Private _BookTrackDetailG6201 As String = ""
        Private _BookTrackDetailG6202 As String = ""
        Private _BookTrackDetailG6203 As String = ""
        Private _BookTrackDetailG6204 As String = ""
        Private _BookTrackDetailG6205 As String = ""
        Private _BookTrackDetailAT701 As String = ""
        Private _BookTrackDetailAT702 As String = ""
        Private _BookTrackDetailAT703 As String = ""
        Private _BookTrackDetailAT704 As String = ""
        Private _BookTrackDetailAT705 As String = ""
        Private _BookTrackDetailAT706 As String = ""
        Private _BookTrackDetailAT707 As String = ""
        Private _BookTrackDetailMS1StatusUpdate As Boolean = False
        Private _BookTrackDetailMS101 As String = ""
        Private _BookTrackDetailMS102 As String = ""
        Private _BookTrackDetailMS103 As String = ""
        Private _BookTrackDetailMS104 As String = ""
        Private _BookTrackDetailMS105 As String = ""
        Private _BookTrackDetailMS106 As String = ""
        Private _BookTrackDetailMS107 As String = ""
        Private _BookTrackDetailMS201 As String = ""
        Private _BookTrackDetailMS202 As String = ""
        Private _BookTrackDetailMS203 As String = ""
        Private _BookTrackDetailMS204 As String = ""
        Private _BookTrackDetailModUser As String = ""
        Private _BookTrackDetailModDate As System.Nullable(Of Date)

        ''' <summary>
        ''' Set to true to associate a StopControl with the BookTrackDetail record.
        ''' By default the hidden property is set to false.
        ''' The location info must come from either the public EDI fields or the hidden property Stop.
        ''' Note: This is a private property accessed only via get and set methods
        ''' </summary>
        Private _blnLinkStopRecord As Boolean = False 'private property accessed via get and set methods

        ''' <summary>
        ''' Can be used to store the comment location when it does not come from a 214.
        ''' Must set private property LinkStopRecord to true (via get and set methods) in order to associate this tblStop with the BookTrackDetail record on insert.
        ''' Note: This is a private property accessed only via get and set methods
        ''' </summary>
        Private _Stop As New tblStop '


        <DataMember()>
        Public Property BookTrackDetailControl() As Integer
            Get
                Return _BookTrackDetailControl
            End Get
            Set(ByVal value As Integer)
                _BookTrackDetailControl = value
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailBookTrackControl() As Integer
            Get
                Return _BookTrackDetailBookTrackControl
            End Get
            Set(ByVal value As Integer)
                _BookTrackDetailBookTrackControl = value
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailStopControl() As Integer
            Get
                Return _BookTrackDetailStopControl
            End Get
            Set(ByVal value As Integer)
                _BookTrackDetailStopControl = value
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailG6201() As String
            Get
                Return Left(_BookTrackDetailG6201, 2)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailG6201 = Left(value, 2)
            End Set
        End Property

        ''' <summary>DATE - Length 10</summary>
        <DataMember()>
        Public Property BookTrackDetailG6202() As String
            Get
                Return Left(_BookTrackDetailG6202, 10)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailG6202 = Left(value, 10)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailG6203() As String
            Get
                Return Left(_BookTrackDetailG6203, 2)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailG6203 = Left(value, 2)
            End Set
        End Property

        ''' <summary>TIME - Length 10</summary>
        <DataMember()>
        Public Property BookTrackDetailG6204() As String
            Get
                Return Left(_BookTrackDetailG6204, 10)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailG6204 = Left(value, 10)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailG6205() As String
            Get
                Return Left(_BookTrackDetailG6205, 2)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailG6205 = Left(value, 2)
            End Set
        End Property

        ''' <summary>
        ''' <para>SHIPMENT STATUS CODE (one of AT701 or AT703 must be provided when AT705 or AT706 are provided)</para>
        ''' X3 - Arrived at Pickup | AF - Carrier departed Pickup Location |
        ''' L1 – Carrier Loading at Pickup | CP – Complete Loading at Pickup |
        ''' X1 - Arrived at Delivery Location | CD - Carrier departed Delivery Location | 
        ''' X5 – Carrier Start Unloading at Delivery Location | D1 - Carrier Unloading Complete |
        ''' XB - Carrier Assignment Accepted | SD - Shipment Delayed | X6 – En route to Delivery Location (See MS101) |
        ''' </summary>
        ''' <returns></returns>
        <DataMember()>
        Public Property BookTrackDetailAT701() As String
            Get
                Return Left(_BookTrackDetailAT701, 2)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailAT701 = Left(value, 2)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailAT702() As String
            Get
                Return Left(_BookTrackDetailAT702, 2)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailAT702 = Left(value, 2)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailAT703() As String
            Get
                Return Left(_BookTrackDetailAT703, 2)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailAT703 = Left(value, 2)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailAT704() As String
            Get
                Return Left(_BookTrackDetailAT704, 2)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailAT704 = Left(value, 2)
            End Set
        End Property

        ''' <summary>DATE Expected when data is available (Date at AT701) - Length 10</summary>
        <DataMember()>
        Public Property BookTrackDetailAT705() As String
            Get
                Return Left(_BookTrackDetailAT705, 10)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailAT705 = Left(value, 10)
            End Set
        End Property

        ''' <summary>TIME Expected when data is available (Time at AT701) - Length 10</summary>
        <DataMember()>
        Public Property BookTrackDetailAT706() As String
            Get
                Return Left(_BookTrackDetailAT706, 10)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailAT706 = Left(value, 10)
            End Set
        End Property

        ''' <summary>
        ''' TIME CODE - Length 2 [ LT - Local Time (Must use local time) ]
        ''' </summary>
        ''' <returns></returns>
        <DataMember()>
        Public Property BookTrackDetailAT707() As String
            Get
                Return Left(_BookTrackDetailAT707, 2)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailAT707 = Left(value, 2)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailMS1StatusUpdate() As Boolean
            Get
                Return _BookTrackDetailMS1StatusUpdate
            End Get
            Set(ByVal value As Boolean)
                _BookTrackDetailMS1StatusUpdate = value
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailMS101() As String
            Get
                Return Left(_BookTrackDetailMS101, 30)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailMS101 = Left(value, 30)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailMS102() As String
            Get
                Return Left(_BookTrackDetailMS102, 2)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailMS102 = Left(value, 2)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailMS103() As String
            Get
                Return Left(_BookTrackDetailMS103, 3)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailMS103 = Left(value, 3)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailMS104() As String
            Get
                Return Left(_BookTrackDetailMS104, 7)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailMS104 = Left(value, 7)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailMS105() As String
            Get
                Return Left(_BookTrackDetailMS105, 7)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailMS105 = Left(value, 7)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailMS106() As String
            Get
                Return Left(_BookTrackDetailMS106, 1)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailMS106 = Left(value, 1)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailMS107() As String
            Get
                Return Left(_BookTrackDetailMS107, 1)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailMS107 = Left(value, 1)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailMS201() As String
            Get
                Return Left(_BookTrackDetailMS201, 4)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailMS201 = Left(value, 4)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailMS202() As String
            Get
                Return Left(_BookTrackDetailMS202, 10)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailMS202 = Left(value, 10)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailMS203() As String
            Get
                Return Left(_BookTrackDetailMS203, 2)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailMS203 = Left(value, 2)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailMS204() As String
            Get
                Return Left(_BookTrackDetailMS204, 1)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailMS204 = Left(value, 1)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailModUser() As String
            Get
                Return Left(_BookTrackDetailModUser, 100)
            End Get
            Set(ByVal value As String)
                _BookTrackDetailModUser = Left(value, 100)
            End Set
        End Property

        <DataMember()>
        Public Property BookTrackDetailModDate() As System.Nullable(Of Date)
            Get
                Return _BookTrackDetailModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookTrackDetailModDate = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookTrackDetail
            instance = DirectCast(MemberwiseClone(), BookTrackDetail)
            Return instance
        End Function

        ''' <summary>
        ''' Hidden propery LinkStopRecord is used to determine if a Stop record should be associated with the BookTrackDetails record on insert 
        ''' via the property BookTrackDetailStopControl.
        ''' Returns the value of the hidden property
        ''' </summary>
        ''' <returns>Boolean</returns>
        Public Function getLinkStopRecord() As Boolean
            Return _blnLinkStopRecord
        End Function

        ''' <summary>
        ''' Pass in true to associate a StopControl with the BookTrackDetail record.
        ''' Note: By default the hidden property is set to false
        ''' </summary>
        ''' <param name="LinkStopRecord">When true attempt to populate BookTrackDetailStopControl using other object properties. When false property BookTrackDetailStopControl will be 0</param>
        Public Sub setLinkStopRecord(ByVal LinkStopRecord As Boolean)
            _blnLinkStopRecord = LinkStopRecord
        End Sub

        ''' <summary>
        ''' Gets the hidden property Stop
        ''' This property is a DTO.tblStop object which can be used to store the 
        ''' comment location when it does not come from a 214
        ''' </summary>
        ''' <returns>DTO.tblStop</returns>
        Public Function getStop() As tblStop
            Return _Stop
        End Function

        ''' <summary>
        ''' Sets the hidden property Stop
        ''' This property is a DTO.tblStop object which can be used to store the 
        ''' comment location when it does not come from a 214
        ''' </summary>
        ''' <param name="s"></param>
        Public Sub setStop(ByVal s As tblStop)
            _Stop = s
        End Sub

#End Region

    End Class
End Namespace