Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblBadAddress
        Inherits DTOBaseClass


#Region " Data Members"
        Private _BLAControl As Integer = 0
        <DataMember()> _
        Public Property BLAControl() As Integer
            Get
                Return _BLAControl
            End Get
            Set(ByVal value As Integer)
                _BLAControl = value
            End Set
        End Property

        Private _BLALaneControl As Integer = 0
        <DataMember()> _
        Public Property BLALaneControl() As Integer
            Get
                Return _BLALaneControl
            End Get
            Set(ByVal value As Integer)
                _BLALaneControl = value
            End Set
        End Property

        Private _BLALaneNumber As String = ""
        <DataMember()> _
        Public Property BLALaneNumber() As String
            Get
                Return Left(_BLALaneNumber, 50)
            End Get
            Set(ByVal value As String)
                _BLALaneNumber = Left(value, 50)
            End Set
        End Property

        Private _BLABookProNumber As String = ""
        <DataMember()> _
        Public Property BLABookProNumber() As String
            Get
                Return Left(_BLABookProNumber, 50)
            End Get
            Set(ByVal value As String)
                _BLABookProNumber = Left(value, 50)
            End Set
        End Property
         

        Private _BLALaneOrigAddress1 As String = ""
        <DataMember()> _
        Public Property BLALaneOrigAddress1() As String
            Get
                Return Left(_BLALaneOrigAddress1, 40)
            End Get
            Set(ByVal value As String)
                _BLALaneOrigAddress1 = Left(value, 40)
            End Set
        End Property
 
        Private _BLALaneOrigCity As String = ""
        <DataMember()> _
        Public Property BLALaneOrigCity() As String
            Get
                Return Left(_BLALaneOrigCity, 25)
            End Get
            Set(ByVal value As String)
                _BLALaneOrigCity = Left(value, 25)
            End Set
        End Property

        Private _BLALaneOrigState As String = ""
        <DataMember()> _
        Public Property BLALaneOrigState() As String
            Get
                Return Left(_BLALaneOrigState, 8)
            End Get
            Set(ByVal value As String)
                _BLALaneOrigState = Left(value, 8)
            End Set
        End Property

        Private _BLALaneOrigCountry As String = ""
        <DataMember()> _
        Public Property BLALaneOrigCountry() As String
            Get
                Return Left(_BLALaneOrigCountry, 30)
            End Get
            Set(ByVal value As String)
                _BLALaneOrigCountry = Left(value, 30)
            End Set
        End Property

        Private _BLALaneOrigZip As String = ""
        <DataMember()> _
        Public Property BLALaneOrigZip() As String
            Get
                Return Left(_BLALaneOrigZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BLALaneOrigZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

          
       

        Private _BLALaneDestAddress1 As String = ""
        <DataMember()> _
        Public Property BLALaneDestAddress1() As String
            Get
                Return Left(_BLALaneDestAddress1, 40)
            End Get
            Set(ByVal value As String)
                _BLALaneDestAddress1 = Left(value, 40)
            End Set
        End Property

         

        Private _BLALaneDestCity As String = ""
        <DataMember()> _
        Public Property BLALaneDestCity() As String
            Get
                Return Left(_BLALaneDestCity, 25)
            End Get
            Set(ByVal value As String)
                _BLALaneDestCity = Left(value, 25)
            End Set
        End Property

        Private _BLALaneDestState As String = ""
        <DataMember()> _
        Public Property BLALaneDestState() As String
            Get
                Return Left(_BLALaneDestState, 2)
            End Get
            Set(ByVal value As String)
                _BLALaneDestState = Left(value, 2)
            End Set
        End Property

        Private _BLALaneDestCountry As String = ""
        <DataMember()> _
        Public Property BLALaneDestCountry() As String
            Get
                Return Left(_BLALaneDestCountry, 30)
            End Get
            Set(ByVal value As String)
                _BLALaneDestCountry = Left(value, 30)
            End Set
        End Property

        Private _BLALaneDestZip As String = ""
        <DataMember()> _
        Public Property BLALaneDestZip() As String
            Get
                Return Left(_BLALaneDestZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BLALaneDestZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property



        Private _BLAPCMilerOrigAddress1 As String = ""
        <DataMember()> _
        Public Property BLAPCMilerOrigAddress1() As String
            Get
                Return Left(_BLAPCMilerOrigAddress1, 40)
            End Get
            Set(ByVal value As String)
                _BLAPCMilerOrigAddress1 = Left(value, 40)
            End Set
        End Property



        Private _BLAPCMilerOrigCity As String = ""
        <DataMember()> _
        Public Property BLAPCMilerOrigCity() As String
            Get
                Return Left(_BLAPCMilerOrigCity, 25)
            End Get
            Set(ByVal value As String)
                _BLAPCMilerOrigCity = Left(value, 25)
            End Set
        End Property

        Private _BLAPCMilerOrigState As String = ""
        <DataMember()> _
        Public Property BLAPCMilerOrigState() As String
            Get
                Return Left(_BLAPCMilerOrigState, 2)
            End Get
            Set(ByVal value As String)
                _BLAPCMilerOrigState = Left(value, 2)
            End Set
        End Property

        Private _BLAPCMilerOrigCountry As String = ""
        <DataMember()> _
        Public Property BLAPCMilerOrigCountry() As String
            Get
                Return Left(_BLAPCMilerOrigCountry, 30)
            End Get
            Set(ByVal value As String)
                _BLAPCMilerOrigCountry = Left(value, 30)
            End Set
        End Property

        Private _BLAPCMilerOrigZip As String = ""
        <DataMember()> _
        Public Property BLAPCMilerOrigZip() As String
            Get
                Return Left(_BLAPCMilerOrigZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BLAPCMilerOrigZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property



        Private _BLAPCMilerDestAddress1 As String = ""
        <DataMember()> _
        Public Property BLAPCMilerDestAddress1() As String
            Get
                Return Left(_BLAPCMilerDestAddress1, 40)
            End Get
            Set(ByVal value As String)
                _BLAPCMilerDestAddress1 = Left(value, 40)
            End Set
        End Property



        Private _BLAPCMilerDestCity As String = ""
        <DataMember()> _
        Public Property BLAPCMilerDestCity() As String
            Get
                Return Left(_BLAPCMilerDestCity, 25)
            End Get
            Set(ByVal value As String)
                _BLAPCMilerDestCity = Left(value, 25)
            End Set
        End Property

        Private _BLAPCMilerDestState As String = ""
        <DataMember()> _
        Public Property BLAPCMilerDestState() As String
            Get
                Return Left(_BLAPCMilerDestState, 2)
            End Get
            Set(ByVal value As String)
                _BLAPCMilerDestState = Left(value, 2)
            End Set
        End Property

        Private _BLAPCMilerDestCountry As String = ""
        <DataMember()> _
        Public Property BLAPCMilerDestCountry() As String
            Get
                Return Left(_BLAPCMilerDestCountry, 30)
            End Get
            Set(ByVal value As String)
                _BLAPCMilerDestCountry = Left(value, 30)
            End Set
        End Property

        Private _BLAPCMilerDestZip As String = ""
        <DataMember()> _
        Public Property BLAPCMilerDestZip() As String
            Get
                Return Left(_BLAPCMilerDestZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BLAPCMilerDestZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _BLAMessage As String = ""
        <DataMember()> _
        Public Property BLAMessage() As String
            Get
                Return Left(_BLAMessage, 1000)
            End Get
            Set(ByVal value As String)
                _BLAMessage = Left(value, 1000)
            End Set
        End Property

        Private _BLAModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BLAModDate() As System.Nullable(Of Date)
            Get
                Return _BLAModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BLAModDate = value
            End Set
        End Property

        Private _BLAModUser As String = ""
        <DataMember()> _
        Public Property BLAModUser() As String
            Get
                Return Left(_BLAModUser, 100)
            End Get
            Set(ByVal value As String)
                _BLAModUser = Left(value, 100)
            End Set
        End Property

        Private _BLAUpdated As Byte()
        <DataMember()> _
        Public Property BLAUpdated() As Byte()
            Get
                Return _BLAUpdated
            End Get
            Set(ByVal value As Byte())
                _BLAUpdated = value
            End Set
        End Property

        Private _BatchID As Double = 0
        <DataMember()> _
        Public Property BatchID() As Double
            Get
                Return _BatchID
            End Get
            Set(ByVal value As Double)
                _BatchID = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblBadAddress
            instance = DirectCast(MemberwiseClone(), tblBadAddress)

            Return instance
        End Function

#End Region

    End Class
End Namespace
