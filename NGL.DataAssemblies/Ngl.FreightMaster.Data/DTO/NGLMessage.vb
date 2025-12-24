Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class NGLMessage
        Inherits DTOBaseClass


#Region " Constructor"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal m As String)
            MyBase.New()
            Me.Message = m
        End Sub

        ''' <summary>
        ''' Creates a new message object with reference link data
        ''' </summary>
        ''' <param name="m">Message Maps to NMMessage in the database</param>
        ''' <param name="c">Control Maps to NMMTRefControl in database (FK to Source Table like tblSolutionTruck.SolutionTruckControl)</param>
        ''' <param name="n">ControlReferenceName Maps to NMMTRefName in database text value for ControlReference like tblSolutionTruck </param>
        ''' <param name="e">ControlReference Maps to NMNMTControl FK to database NGL Message Type table links to Utilities.NGLMessageKeyRef</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal m As String, ByVal c As Int64, ByVal n As String, ByVal e As Utilities.NGLMessageKeyRef)
            MyBase.New()
            Me.Message = m
            Me.Control = c
            Me.ControlReferenceName = n
            Me.ControlReference = e
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="m">Message Maps to NMMessage in the database</param>
        ''' <param name="c">Control Maps to NMMTRefControl in database (FK to Source Table like tblSolutionTruck.SolutionTruckControl)</param>
        ''' <param name="n">ControlReferenceName Maps to NMMTRefName in database text value for ControlReference like tblSolutionTruck</param>
        ''' <param name="e">ControlReference Maps to NMNMTControl FK to database NGL Message Type table liks to Utilities.NGLMessageKeyRef</param>
        ''' <param name="eReason">ErrorReason Maps to NMErrorReason in database</param>
        ''' <param name="eMessage">ErrorMessage Maps to NMErrorMessage in database</param>
        ''' <param name="eDetails">ErrorDetails Maps to NMErrorDetails in database</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal m As String, ByVal c As Int64, ByVal n As String, ByVal e As Utilities.NGLMessageKeyRef, ByVal eReason As String, ByVal eMessage As String, ByVal eDetails As String)
            MyBase.New()
            Me.Message = m
            Me.Control = c
            Me.ControlReferenceName = n
            Me.ControlReference = e
            Me.ErrorReason = eReason
            Me.ErrorMessage = eMessage
            Me.ErrorDetails = eDetails
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="m">Message Maps to NMMessage in the database</param>
        ''' <param name="c">Control Maps to NMMTRefControl in database (FK to Source Table like tblSolutionTruck.SolutionTruckControl)</param>
        ''' <param name="a">AlphaCode Maps to NMMTRefAlphaControl in database (FK to Source Table or view where a numeric key is not available like  tblSolutionTruck.TruckKey
        ''' may be combined with the Control key to form complex references like when a company control number is required
        ''' Limited to 100 characters (extra characters will be truncated)</param>
        ''' <param name="n">ControlReferenceName Maps to NMMTRefName in database text value for ControlReference like tblSolutionTruck</param>
        ''' <param name="e">ControlReference Maps to NMNMTControl FK to database NGL Message Type table liks to Utilities.NGLMessageKeyRef</param>
        ''' <param name="eReason">ErrorReason Maps to NMErrorReason in database</param>
        ''' <param name="eMessage">ErrorMessage Maps to NMErrorMessage in database</param>
        ''' <param name="eDetails">ErrorDetails Maps to NMErrorDetails in database</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal m As String, ByVal c As Int64, ByVal a As String, ByVal n As String, ByVal e As Utilities.NGLMessageKeyRef, ByVal eReason As String, ByVal eMessage As String, ByVal eDetails As String)
            MyBase.New()
            Me.Message = m
            Me.Control = c
            Me.AlphaCode = a
            Me.ControlReferenceName = n
            Me.ControlReference = e
            Me.ErrorReason = eReason
            Me.ErrorMessage = eMessage
            Me.ErrorDetails = eDetails
        End Sub
#End Region

#Region " Data Members"

        Private _Message As String = ""
        <DataMember()> _
        Public Property Message() As String
            Get
                Return _Message
            End Get
            Set(ByVal value As String)
                _Message = value
            End Set
        End Property

        Private _Control As Int64
        ''' <summary>
        ''' Control Maps to NMMTRefControl in database (FK to Source Table like tblSolutionTruck.SolutionTruckControl
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DataMember()> _
        Public Property Control() As Int64
            Get
                Return _Control
            End Get
            Set(ByVal value As Int64)
                _Control = value
            End Set
        End Property

        Private _AlphaCode As String
        ''' <summary>
        ''' AlphaCode Maps to NMMTRefAlphaControl in database (FK to Source Table or view where a numeric key is not available like  tblSolutionTruck.TruckKey
        ''' may be combinded with the Control key to form complex references like when a company control number is required
        ''' Limited to 100 characters (extra characters will be truncated)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DataMember()> _
        Public Property AlphaCode() As String
            Get
                Return Left(_AlphaCode, 100)
            End Get
            Set(ByVal value As String)
                _AlphaCode = Left(value, 100)
            End Set
        End Property

        Private _ControlReference As Utilities.NGLMessageKeyRef = Utilities.NGLMessageKeyRef.CarrierTariff
        ''' <summary>
        ''' ControlReference Maps to NMNMTControl FK to database NGL Message Type table liks to Utilities.NGLMessageKeyRef
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DataMember()> _
        Public Property ControlReference() As Utilities.NGLMessageKeyRef
            Get
                Return _ControlReference
            End Get
            Set(ByVal value As Utilities.NGLMessageKeyRef)
                _ControlReference = value
            End Set
        End Property

        Private _ControlReferenceName As String
        <DataMember()> _
        Public Property ControlReferenceName() As String
            Get
                Return _ControlReferenceName
            End Get
            Set(ByVal value As String)
                _ControlReferenceName = value
            End Set
        End Property

        Private _ErrorReason As String
        <DataMember()> _
        Public Property ErrorReason() As String
            Get
                Return _ErrorReason
            End Get
            Set(ByVal value As String)
                _ErrorReason = value
            End Set
        End Property

        Private _ErrorMessage As String
        <DataMember()> _
        Public Property ErrorMessage() As String
            Get
                Return _ErrorMessage
            End Get
            Set(ByVal value As String)
                _ErrorMessage = value
            End Set
        End Property

        Private _ErrorDetails As String
        <DataMember()> _
        Public Property ErrorDetails() As String
            Get
                Return _ErrorDetails
            End Get
            Set(ByVal value As String)
                _ErrorDetails = value
            End Set
        End Property


        Private _ErrorCSVDetailsParameters As String
        <DataMember()>
        Public Property ErrorCSVDetailsParameters() As String
            Get
                Return _ErrorCSVDetailsParameters
            End Get
            Set(ByVal value As String)
                _ErrorCSVDetailsParameters = value
            End Set
        End Property


        Private _MessageLocalCode As String = ""
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.5.3.001 on 05/25/2022
        ''' </remarks>
        <DataMember()>
        Public Property MessageLocalCode() As String
            Get
                Return _MessageLocalCode
            End Get
            Set(ByVal value As String)
                _MessageLocalCode = value
            End Set
        End Property

        Private _VendorMessage As String = ""
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.5.3.001 on 05/25/2022
        ''' </remarks>
        <DataMember()>
        Public Property VendorMessage() As String
            Get
                Return _VendorMessage
            End Get
            Set(ByVal value As String)
                _VendorMessage = value
            End Set
        End Property

        Private _FieldName As String = ""
        ''' <summary>
        ''' 
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.5.3.001 on 05/25/2022
        ''' </remarks>
        <DataMember()>
        Public Property FieldName() As String
            Get
                Return _FieldName
            End Get
            Set(ByVal value As String)
                _FieldName = value
            End Set
        End Property

        Private _Details As String = ""
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.5.3.001 on 05/25/2022
        ''' </remarks>
        <DataMember()>
        Public Property Details() As String
            Get
                Return _Details
            End Get
            Set(ByVal value As String)
                _Details = value
            End Set
        End Property

        Private _bLogged As Boolean = False
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.5.3.001 on 05/25/2022
        ''' </remarks>
        <DataMember()>
        Public Property bLogged() As Boolean
            Get
                Return _bLogged
            End Get
            Set(ByVal value As Boolean)
                _bLogged = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New NGLMessage
            instance = DirectCast(MemberwiseClone(), NGLMessage)
            Return instance
        End Function

        Public Sub fillCSVDetailsParametersFromList(ByVal lPars As List(Of String))
            If Not lPars Is Nothing AndAlso lPars.Count > 0 Then
                Me.ErrorCSVDetailsParameters = String.Join(",", lPars)
            End If
        End Sub

        Public Sub fillCSVDetailsParameters(ByVal ParamArray p() As String)
            If Not p Is Nothing AndAlso p.Count > 0 Then
                Me.ErrorCSVDetailsParameters = String.Join(",", p)
            End If
        End Sub

        Public Sub populateErrorInformation(ByVal r As String, ByVal m As String, ByVal d As String, ByVal ParamArray p() As String)
            Me.ErrorReason = r
            Me.ErrorMessage = m
            Me.ErrorDetails = d
            fillCSVDetailsParameters(p)
        End Sub

        Public Sub populateErrorInformationFromList(ByVal r As String, ByVal m As String, ByVal d As String, ByVal lPars As List(Of String))
            Me.ErrorReason = r
            Me.ErrorMessage = m
            Me.ErrorDetails = d
            fillCSVDetailsParametersFromList(lPars)
        End Sub


#End Region

    End Class
End Namespace
