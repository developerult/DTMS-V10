Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    ''' <summary>
    ''' Load Status Reason Code Data
    ''' </summary>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.0.117 on 8/10/19
    '''     added new fields [LoadStatusLSCTControl] and [LoadStatusSequenceNo]
    ''' </remarks>
    <DataContract()>
    Public Class LoadStatusCode
        Inherits DTOBaseClass


#Region " Data Members"




        Private _LoadStatusCodeControl As Integer = 0
        <DataMember()>
        Public Property LoadStatusCodeControl() As Integer
            Get
                Return _LoadStatusCodeControl
            End Get
            Set(ByVal value As Integer)
                _LoadStatusCodeControl = value
            End Set
        End Property

        Private _LoadStatusCode As Integer = 0
        <DataMember()>
        Public Property LoadStatusCode() As Integer
            Get
                Return _LoadStatusCode
            End Get
            Set(ByVal value As Integer)
                _LoadStatusCode = value
            End Set
        End Property


        Private _LoadStatusCodeDesc As String = ""
        <DataMember()>
        Public Property LoadStatusCodeDesc() As String
            Get
                Return Left(_LoadStatusCodeDesc, 50)
            End Get
            Set(ByVal value As String)
                _LoadStatusCodeDesc = Left(value, 50)
            End Set
        End Property

        Private _LoadStatusCodesUpdated As Byte()
        <DataMember()>
        Public Property LoadStatusCodesUpdated() As Byte()
            Get
                Return _LoadStatusCodesUpdated
            End Get
            Set(ByVal value As Byte())
                _LoadStatusCodesUpdated = value
            End Set
        End Property

        ''' <summary>
        ''' Source Code Type, like Freight Bill,  FK to LoadStatusCodeType
        ''' </summary>
        ''' <remarks>
        ''' Modified by RHR for v-8.2.0.117 on 8/10/19
        '''     added new field [LoadStatusLSCTControl]
        ''' </remarks>
        Private _LoadStatusLSCTControl As Integer = 0
        <DataMember()>
        Public Property LoadStatusLSCTControl() As Integer
            Get
                Return _LoadStatusLSCTControl
            End Get
            Set(ByVal value As Integer)
                _LoadStatusLSCTControl = value
            End Set
        End Property

        ''' <summary>
        ''' Code Sequence Number for lookup lists
        ''' </summary>
        ''' <remarks>
        ''' Modified by RHR for v-8.2.0.117 on 8/10/19
        '''     added new field [LoadStatusSequenceNo]
        ''' </remarks>
        Private _LoadStatusSequenceNo As Integer = 0
        <DataMember()>
        Public Property LoadStatusSequenceNo() As Integer
            Get
                Return _LoadStatusSequenceNo
            End Get
            Set(ByVal value As Integer)
                _LoadStatusSequenceNo = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New LoadStatusCode
            instance = DirectCast(MemberwiseClone(), LoadStatusCode)
            Return instance
        End Function

#End Region

    End Class
End Namespace