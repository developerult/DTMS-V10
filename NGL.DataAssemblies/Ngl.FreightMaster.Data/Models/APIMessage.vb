Imports Map = Ngl.API.Mapping
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects

Namespace Models
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' Added By RHR for v-8.1 on 03/30/2018
    ''' Typically used to generate BookTrack status messages
    ''' If linked to a tblLoadTenderRecord the data is stored in tblNGLMessage 
    ''' tblNGLMessage mapping rules
    ''' NMNMTControl maps to tblNGLMessageType.NMTControl for one of the following new types
    '''   NMTName = NGLAPIInfoMessages
    '''   NMTName = NGLAPIWarnings
    '''   NMTName = NGLAPIErrors  
    ''' NMMTRefControl maps to the tblLoadTender.LoadTenderControl
    ''' NMMTRefAlphaControl maps to tblLoadTender.LTBookSHID
    ''' NMMTRefName maps to Dispatch.ErrorCode if available or Info by default
    ''' NMErrorMessage maps to Dispatch.ErrorMessage 
    ''' Severity maps to NMErrorReason
    ''' Message maps to NMMessage
    ''' Diagnostic maps to NMErrorDetails
    ''' Source maps to NMKeyString
    ''' use new spAddLoadTenderMessage stored procedure
    ''' </remarks>
    Public Class APIMessage

        Private _Severity As String
        ''' <summary>
        ''' Values like  "ERROR" "WARNING" "INFO"  
        ''' Typically maps to BookTrackStatus codes based on Severity and Source
        ''' </summary>
        ''' <returns></returns>
        Public Property Severity() As String
            Get
                Return Left(_Severity, 20)
            End Get
            Set(ByVal value As String)
                _Severity = Left(value, 20)
            End Set
        End Property

        Private _Message As String
        ''' <summary>
        ''' Typically maps to BookTrackComment
        ''' </summary>
        ''' <returns></returns>
        Public Property Message() As String
            Get
                Return Left(_Message, 255)
            End Get
            Set(ByVal value As String)
                _Message = Left(value, 255)
            End Set
        End Property

        Private _Diagnostic As String
        ''' <summary>
        ''' Typically maps to BookTrackComment 
        ''' </summary>
        ''' <returns></returns>
        Public Property Diagnostic() As String
            Get
                Return Left(_Diagnostic, 255)
            End Get
            Set(ByVal value As String)
                _Diagnostic = Left(value, 255)
            End Set
        End Property

        Private _Source As String
        ''' <summary>
        ''' Values like  "SYSTEM" "CAPACITY_PROVIDER"    
        ''' Typically maps to BookTrackStatus codes based on Severity and Source
        ''' </summary>
        ''' <returns></returns>
        Public Property Source() As String
            Get
                Return Left(_Source, 20)
            End Get
            Set(ByVal value As String)
                _Source = Left(value, 20)
            End Set
        End Property


        Public Function MapNGLAPIAPIMessage() As Map.APIMessage
            Dim result As New Map.APIMessage
            result.Severity = Me.Severity
            result.Message = Me.Message
            result.Diagnostic = Me.Diagnostic
            result.Source = Me.Source
            Return result
        End Function


    End Class


End Namespace

