Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports LTS = NGL.FreightMaster.Data.LTS
Imports DTran = NGL.Core.Utility.DataTransformation
Imports TAR = NGL.FM.CarTar
Imports NGLCoreComm = NGL.Core.Communication

''' <summary>
''' All properties and methods in this Module are static and may be stored with each users session
''' data stored in this Module may still be available on future calls to WCF; Static data may not be 
''' Disposed between sessions so do not store any dynamic data 
''' </summary>
''' <remarks></remarks>
Module Utilities

    Public Const BATCHPROCESSTITLE As String = "ImportContractProcess"
    Public Const BATCHCSVIMPORTPROCESSTITLE As String = "ImportContractProcess"
    Public Const BATCHCSVIMPORTINTERLINEPROCESSTITLE As String = "ImportInterlineContractProcess"
    Public Const BATCHCSVIMPORTNONSERVCPROCESSTITLE As String = "ImportNonServcContractProcess"

    

    
    Private _dictLoadStatusControls As New Dictionary(Of Integer, Integer)
    ''' <summary>
    ''' this dictionary can be static and used across multiple sessions because the LoadStatsCodes are 
    ''' never deleted only added.  improves performance by reducing database lookups on tables that 
    ''' do not change or only allow inserts
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property dictLoadStatusControls() As Dictionary(Of Integer, Integer)
        Get
            Return _dictLoadStatusControls
        End Get
        Set(ByVal value As Dictionary(Of Integer, Integer))
            _dictLoadStatusControls = value
        End Set
    End Property

    Public Function tryGetLoadStatusControl(ByVal LoadStatusCode As Integer, ByRef LoadStatusControl As Integer) As Boolean

        Dim blnRet As Boolean = False
        Try
            If dictLoadStatusControls Is Nothing OrElse dictLoadStatusControls.Count < 1 Then Return False
            If dictLoadStatusControls.ContainsKey(LoadStatusCode) Then
                LoadStatusControl = dictLoadStatusControls(LoadStatusCode)
                blnRet = True
            End If

        Catch ex As Exception
            'ignore errors just return false
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' adds or replaces the LoadStatusControl to the dictLoadStatusControls object using the key LoadStatusCode returns the LoadStatusControl
    ''' </summary>
    ''' <param name="LoadStatusCode"></param>
    ''' <param name="LoadStatusControl"></param>
    ''' <remarks></remarks>
    Public Function storeLoadStatusControl(ByVal LoadStatusCode As Integer, ByVal LoadStatusControl As Integer) As Integer
        Try
            If dictLoadStatusControls Is Nothing Then dictLoadStatusControls = New Dictionary(Of Integer, Integer)

            If Not dictLoadStatusControls.ContainsKey(LoadStatusCode) Then
                dictLoadStatusControls.Add(LoadStatusCode, LoadStatusControl)
            ElseIf dictLoadStatusControls(LoadStatusCode) <> LoadStatusControl Then
                dictLoadStatusControls(LoadStatusCode) = LoadStatusControl
            End If

        Catch ex As Exception
            'ignore errors just return false
        End Try
        Return LoadStatusControl
    End Function







End Module
