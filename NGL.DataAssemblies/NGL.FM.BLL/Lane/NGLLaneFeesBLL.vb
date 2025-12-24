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
Imports Serilog
Imports SerilogTracing

Public Class NGLLaneFeesBLL : Inherits BLLBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLLaneFeesBLL"
        Logger = Logger.ForContext(Of NGLLaneFeesBLL)
    End Sub

#End Region

#Region " Properties "

#End Region

#Region "DAL Wrapper Methods"

    Public Function GetLaneFees() As DTO.LaneFee()
        Dim results As DTO.LaneFee()
        Using Logger.StartActivity("GetLaneFees()")
            results = NGLLaneFeeData.GetLaneFeesFiltered()
        End Using
        Return results
    End Function

    Public Function GetLaneFeesByLane(ByVal LaneControl As Integer) As DTO.LaneFee()
        Return NGLLaneFeeData.GetLaneFeesFiltered(LaneControl)
    End Function

    Public Function GetLaneFee(ByVal Control As Integer) As DTO.LaneFee
        Return NGLLaneFeeData.GetLaneFeeFiltered(Control)
    End Function

    Public Function CreateLaneFee(ByVal oData As DTO.LaneFee) As DTO.LaneFee
        Dim oRet As DTO.LaneFee = NGLLaneFeeData.CreateRecord(oData)
        Return oRet
    End Function

    Public Function CreateLaneFees(ByVal oData As DTO.LaneFee()) As Boolean
        Using Logger.StartActivity("CreateLaneFees(LaneFees(): {LaneFees})", oData)
            For Each item In oData
                NGLLaneFeeData.CreateRecord(item)
            Next
            If oData.Count > 0 Then
            End If
        End Using
        Return True
    End Function

    Public Sub DeleteLaneFee(ByVal oData As DTO.LaneFee)
        NGLLaneFeeData.DeleteRecord(oData)
    End Sub

    Public Function UpdateLaneFee(ByVal oData As DTO.LaneFee) As DTO.LaneFee
        Dim oRet As DTO.LaneFee = NGLLaneFeeData.UpdateRecord(oData)
        Return oRet
    End Function

    Public Function UpdateLaneFeeQuick(ByVal oData As DTO.LaneFee) As DTO.QuickSaveResults
        Dim oRet As DTO.QuickSaveResults = NGLLaneFeeData.UpdateRecordQuick(oData)
        Return oRet
    End Function

    Public Sub UpdateLaneFeeNoReturn(ByVal oData As DTO.LaneFee)
        NGLLaneFeeData.UpdateRecordNoReturn(oData)
    End Sub

#End Region

#Region " Public Methods"

#End Region

End Class
