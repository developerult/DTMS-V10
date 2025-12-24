Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports NGL.Core.ChangeTracker

'Added By LVV on 10/5/16 for v-7.0.5.110 HDM Import Tool

Public Class NGLHDMImportToolBLL : Inherits BLLBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLHDMImportTool"
    End Sub

#End Region

    Public Sub deleteFromTmpCSVHDMZips()
        Me.Parameters.ProcedureName = "ImportCSVData"
        'Me.Parameters.ValidateAccess = True

        Dim oSecData As New DAL.NGLSecurityDataProvider(Me.Parameters)

        Using PubsConn As New SqlConnection(oSecData.ConnectionString)
            PubsConn.Open()
            Using cmd As New SqlCommand("delete from tmpCSVHDMZips", PubsConn)
                cmd.CommandTimeout = 3000
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function ExecSPImportCSVHDMZipsFromTmpTbl() As String
        Dim result As String = ""

        'if we made it this far we can continue and import from temp table.
        Me.Parameters.ProcedureName = "ImportCSVData"
        'Me.Parameters.ValidateAccess = True

        Dim oSecData As New DAL.NGLSecurityDataProvider(Me.Parameters)

        Dim PubsConn As SqlConnection = New SqlConnection(oSecData.ConnectionString)

        Dim cmd As SqlCommand = New SqlCommand("spImportCSVHDMZipsFromTmpTbl", PubsConn)
        cmd.CommandType = CommandType.StoredProcedure

        Dim UserName As SqlParameter = cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 100)
        UserName.Value = Me.Parameters.UserName
        UserName.Direction = ParameterDirection.Input

        Dim myReader As SqlDataReader = Nothing
        Try
            PubsConn.Open()
            cmd.CommandTimeout = 3000
            myReader = cmd.ExecuteReader()

            Dim success As Boolean = False
            Dim recordsprocessed As Integer = 0
            Dim retMessage As String = ""
            Do While myReader.Read
                success = myReader.GetBoolean(0)
                recordsprocessed = myReader.GetInt64(1)
                retMessage = myReader.GetString(2)
            Loop

            result = "Success: " & success.ToString() & vbCrLf &
                "Records Processed: " & recordsprocessed & vbCrLf &
                "Message: " & retMessage

            myReader.Close()
            PubsConn.Close()
        Catch ex As Exception
            Throw
        Finally
            Try
                If myReader IsNot Nothing AndAlso Not myReader.IsClosed Then
                    myReader.Close()
                End If
            Catch ex1 As Exception
                'do nothing
            End Try
            Try
                If PubsConn IsNot Nothing AndAlso PubsConn.State = ConnectionState.Open Then
                    PubsConn.Close()
                End If
            Catch ex As Exception
            End Try
        End Try
        Return result
    End Function

    Public Sub ExecSPImportCSVData(ByVal tempTblName As String, ByVal localFilePathName As String)
        'import into temp table.
        Me.Parameters.ProcedureName = "ImportCSVData"
        'Me.Parameters.ValidateAccess = True

        Dim oSecData As New DAL.NGLSecurityDataProvider(Me.Parameters)

        Dim PubsConn As SqlConnection = New SqlConnection(oSecData.ConnectionString)
        Dim cmd As SqlCommand = New SqlCommand("spImportCSVData", PubsConn)
        cmd.CommandType = CommandType.StoredProcedure

        Dim tmpCSVDump As SqlParameter = cmd.Parameters.Add("@TableName", SqlDbType.NVarChar, 4000)
        tmpCSVDump.Value = tempTblName
        tmpCSVDump.Direction = ParameterDirection.Input

        Dim FilePath As SqlParameter = cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 4000)
        FilePath.Value = localFilePathName
        FilePath.Direction = ParameterDirection.Input

        Dim TruncateData As SqlParameter = cmd.Parameters.Add("@TruncateData", SqlDbType.Bit, 1)
        TruncateData.Value = 0
        TruncateData.Direction = ParameterDirection.Input

        Dim SelectData As SqlParameter = cmd.Parameters.Add("@SelectData", SqlDbType.Bit, 1)
        SelectData.Value = 0
        SelectData.Direction = ParameterDirection.Input
        Dim myReader As SqlDataReader = Nothing
        Try
            PubsConn.Open()
            cmd.CommandTimeout = 3000
            myReader = cmd.ExecuteReader()
            Do While myReader.Read
            Loop
            myReader.Close()
            PubsConn.Close()
        Catch ex As Exception
            Throw
        Finally
            Try
                If myReader IsNot Nothing AndAlso Not myReader.IsClosed Then
                    myReader.Close()
                End If
            Catch ex1 As Exception
                'do nothing
            End Try
            Try
                If PubsConn IsNot Nothing AndAlso PubsConn.State = ConnectionState.Open Then
                    PubsConn.Close()
                End If
            Catch ex As Exception
            End Try
        End Try
    End Sub

End Class

