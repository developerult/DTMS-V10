'Public Class ResultObject

'End Class

Namespace Models

    ''' <summary>
    ''' Generic Result Object used primarily to return results to REST Service Controllers and Ajax clients
    ''' </summary>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.0.117 on 8/22/19 
    '''     added new messge function, updateResultMessage, and ResultMsgType enum 
    '''     to assist callers with message management
    '''     added new success and warning titles to assist with display of information
    '''     this class can support all three types or results, success, warning and errors
    '''     the dependent objects need to check for all three conditions and determine which 
    '''     messages to display, where and how.
    '''     See Settlement.aspx for example of how to manage results with this object 
    '''     
    '''     Titles are optional and the client may use defaults if empty
    ''' Modified by RHR for v-8.2.1.004 on 12/27/2019
    '''   added optional BookControl property to assist with processing of result object when
    '''   both a record control number and a reference to the Booking record is required.
    '''   Particularly to support changes to the new DAL SettlementSave procedure.
    ''' </remarks>
    Public Class ResultObject

        Public Enum ResultMsgType
            Success = 0
            Err
            Warning
            Msg
        End Enum

        Private _Control As Integer
        Private _Success As Boolean
        Private _ErrMsg As String
        Private _ErrTitle As String
        Private _SuccessMsg As String
        Private _SuccessTitle As String
        Private _WarningMsg As String
        Private _WarningTitle As String
        Private _LogMsg As String
        Private _LogTitle As String
        Private _validationLong As Long
        Private _ynQuestion As String
        Private _MsgTitle As String
        Private _Msg As String
        Private _BookControl As Integer = 0 'Optional Book Control to assist with resutls
        Public Property Control() As Integer
            Get
                Return _Control
            End Get
            Set(ByVal value As Integer)
                _Control = value
            End Set
        End Property

        Public Property Success() As Boolean
            Get
                Return _Success
            End Get
            Set(ByVal value As Boolean)
                _Success = value
            End Set
        End Property

        Public Property ErrMsg() As String
            Get
                Return _ErrMsg
            End Get
            Set(ByVal value As String)
                _ErrMsg = value
            End Set
        End Property

        Public Property ErrTitle() As String
            Get
                Return _ErrTitle
            End Get
            Set(ByVal value As String)
                _ErrTitle = value
            End Set
        End Property

        Private _Err As New List(Of DataTransferObjects.NGLMessage)
        Public Property Err() As List(Of DataTransferObjects.NGLMessage)
            Get
                If _Err Is Nothing Then
                    _Err = New List(Of DataTransferObjects.NGLMessage)
                End If
                Return _Err
            End Get
            Set(ByVal value As List(Of DataTransferObjects.NGLMessage))
                _Err = value
            End Set
        End Property

        Public Property SuccessMsg() As String
            Get
                Return _SuccessMsg
            End Get
            Set(ByVal value As String)
                _SuccessMsg = value
            End Set
        End Property

        Public Property SuccessTitle() As String
            Get
                Return _SuccessTitle
            End Get
            Set(ByVal value As String)
                _SuccessTitle = value
            End Set
        End Property

        Public Property WarningMsg() As String
            Get
                Return _WarningMsg
            End Get
            Set(ByVal value As String)
                _WarningMsg = value
            End Set
        End Property



        Public Property WarningTitle() As String
            Get
                Return _WarningTitle
            End Get
            Set(ByVal value As String)
                _WarningTitle = value
            End Set
        End Property

        Private _Warn As New List(Of DataTransferObjects.NGLMessage)
        Public Property Warn() As List(Of DataTransferObjects.NGLMessage)
            Get
                If _Warn Is Nothing Then
                    _Warn = New List(Of DataTransferObjects.NGLMessage)
                End If
                Return _Warn
            End Get
            Set(ByVal value As List(Of DataTransferObjects.NGLMessage))
                _Warn = value
            End Set
        End Property




        Public Property LogMsg() As String
            Get
                Return _LogMsg
            End Get
            Set(ByVal value As String)
                _LogMsg = value
            End Set
        End Property

        Public Property LogTitle() As String
            Get
                Return _LogTitle
            End Get
            Set(ByVal value As String)
                _LogTitle = value
            End Set
        End Property


        Private _Log As New List(Of DataTransferObjects.NGLMessage)
        Public Property Log() As List(Of DataTransferObjects.NGLMessage)
            Get
                If _Log Is Nothing Then
                    _Log = New List(Of DataTransferObjects.NGLMessage)
                End If
                Return _Log
            End Get
            Set(ByVal value As List(Of DataTransferObjects.NGLMessage))
                _Log = value
            End Set
        End Property

        Public Property validationLong() As Integer
            Get
                Return _validationLong
            End Get
            Set(ByVal value As Integer)
                _validationLong = value
            End Set
        End Property

        Public Property ynQuestion() As String
            Get
                Return _ynQuestion
            End Get
            Set(ByVal value As String)
                _ynQuestion = value
            End Set
        End Property

        Public Property Msg() As String
            Get
                Return _Msg
            End Get
            Set(ByVal value As String)
                _Msg = value
            End Set
        End Property

        Public Property MsgTitle() As String
            Get
                Return _MsgTitle
            End Get
            Set(ByVal value As String)
                _MsgTitle = value
            End Set
        End Property

        Private _Message As New List(Of DataTransferObjects.NGLMessage)
        Public Property Message() As List(Of DataTransferObjects.NGLMessage)
            Get
                If _Message Is Nothing Then
                    _Message = New List(Of DataTransferObjects.NGLMessage)
                End If
                Return _Message
            End Get
            Set(ByVal value As List(Of DataTransferObjects.NGLMessage))
                _Message = value
            End Set
        End Property

        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property


        ''' <summary>
        ''' Replace the message text and title for the message type with new values,  
        ''' to concatenate call appendToResultMessage
        ''' </summary>
        ''' <param name="eType"></param>
        ''' <param name="sMsg"></param>
        ''' <param name="sTitle"></param>
        ''' <remarks>
        ''' Created by RHR for v-8.2.0.117 on 8/22/19
        '''     new messge function  to assist callers with message management
        ''' </remarks>
        Public Sub updateResultMessage(ByVal eType As ResultMsgType, ByVal sMsg As String, Optional ByVal sTitle As String = "")
            Try
                Select Case eType
                    Case ResultMsgType.Warning
                        WarningMsg = sMsg
                        WarningTitle = sTitle
                    Case ResultMsgType.Err
                        ErrMsg = sMsg
                        ErrTitle = sTitle
                    Case Else
                        SuccessMsg = sMsg
                        SuccessTitle = sTitle
                End Select
            Catch ex As Exception
                'ignore all errors, this should not happen but if it does something bad happended so other processes will catch unexpected errors
            End Try

        End Sub

        ''' <summary>
        ''' Concatenate the message textfor the message type with sMsg,  
        ''' if a title is provided it will replace any existing title
        ''' </summary>
        ''' <param name="eType"></param>
        ''' <param name="sMsg"></param>
        ''' <param name="sTitle"></param>
        Public Sub appendToResultMessage(ByVal eType As ResultMsgType, ByVal sMsg As String, Optional ByVal sTitle As String = "")
            Try
                Select Case eType
                    Case ResultMsgType.Warning
                        WarningMsg &= " " & sMsg
                        If Not String.IsNullOrWhiteSpace(sTitle) Then
                            WarningTitle = sTitle
                        End If
                    Case ResultMsgType.Err
                        ErrMsg &= " " & sMsg
                        If Not String.IsNullOrWhiteSpace(sTitle) Then
                            ErrTitle = sTitle
                        End If

                    Case Else
                        SuccessMsg &= " " & sMsg
                        If Not String.IsNullOrWhiteSpace(sTitle) Then
                            SuccessTitle = sTitle
                        End If
                End Select
            Catch ex As Exception
                'ignore all errors, this should not happen but if it does something bad happended so other processes will catch unexpected errors
            End Try

        End Sub

        Public Sub addToLogList(ByVal sLogMessage As String, ByVal iControl As Int64, ByVal sAlphaCode As String, ByVal eControlReference As Utilities.NGLMessageKeyRef, ByVal sReferenceName As String)
            Dim oNGLMsg As New DataTransferObjects.NGLMessage(sLogMessage, iControl, sReferenceName, eControlReference)
            oNGLMsg.AlphaCode = sAlphaCode
            Log.Add(oNGLMsg)
        End Sub

    End Class


End Namespace

