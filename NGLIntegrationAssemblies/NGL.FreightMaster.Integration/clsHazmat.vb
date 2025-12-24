Imports Ngl.FreightMaster.Integration.Configuration
Imports System.Data.SqlClient
Imports System.Xml.Serialization
Imports DTran = Ngl.Core.Utility.DataTransformation

<Serializable()> _
Public Class clsHazmat : Inherits clsDownload


#Region "Constructors"

    Sub New()
        MyBase.new()
    End Sub

    Sub New(ByVal config As Ngl.FreightMaster.Core.UserConfiguration)
        MyBase.New(config)
    End Sub

    Sub New(ByVal admin_email As String, _
            ByVal from_email As String, _
            ByVal group_email As String, _
            ByVal auto_retry As Integer, _
            ByVal smtp_server As String, _
            ByVal db_server As String, _
            ByVal database_catalog As String, _
            ByVal auth_code As String, _
            ByVal debug_mode As Boolean,
            Optional ByVal connection_string As String = "")

        MyBase.New(admin_email, from_email, group_email, auto_retry, smtp_server, db_server, database_catalog, auth_code, debug_mode, connection_string)


    End Sub

#End Region

    Protected mblnInsertRecord As Boolean = True

    Public Function ProcessData( _
                    ByVal oHazmats As List(Of clsHazmatObject), _
                    ByVal strConnection As String) As ProcessDataReturnValues

        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim strSource As String = "clsHazmat.ProcessData"
        Dim strHeaderTable As String = "Hazmat"
        Me.HeaderName = "Hazmat"
        Me.ImportTypeKey = IntegrationTypes.Hazmat
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Hazmat Data Integration"

        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If

        Dim dicUseField As New Dictionary(Of String, Boolean)

        Try
            'build a dictionary of the fields that can be imported or not
            dicUseField.Add("HazDesc01", getImportFieldFlag("HazDesc01", IntegrationTypes.Hazmat))
            dicUseField.Add("HazDesc02", getImportFieldFlag("HazDesc02", IntegrationTypes.Hazmat))
            dicUseField.Add("HazDesc03", getImportFieldFlag("HazDesc03", IntegrationTypes.Hazmat))
            dicUseField.Add("HazUnit", getImportFieldFlag("HazUnit", IntegrationTypes.Hazmat))
            dicUseField.Add("HazPackingGroup", getImportFieldFlag("HazPackingGroup", IntegrationTypes.Hazmat))
            dicUseField.Add("HazPackingDesc", getImportFieldFlag("HazPackingDesc", IntegrationTypes.Hazmat))
            dicUseField.Add("HazShipInst", getImportFieldFlag("HazShipInst", IntegrationTypes.Hazmat))
            dicUseField.Add("HazLtdQ", getImportFieldFlag("HazLtdQ", IntegrationTypes.Hazmat))
            dicUseField.Add("HazMarPoll", getImportFieldFlag("HazMarPoll", IntegrationTypes.Hazmat))
            dicUseField.Add("HazMarStorCat", getImportFieldFlag("HazMarStorCat", IntegrationTypes.Hazmat))
            dicUseField.Add("HazNMFCSub", getImportFieldFlag("HazNMFCSub", IntegrationTypes.Hazmat))
            dicUseField.Add("HazNMFC", getImportFieldFlag("HazNMFC", IntegrationTypes.Hazmat))
            dicUseField.Add("HazFrtClass", getImportFieldFlag("HazFrtClass", IntegrationTypes.Hazmat))
            dicUseField.Add("HazFdxGndOK", getImportFieldFlag("HazFdxGndOK", IntegrationTypes.Hazmat))
            dicUseField.Add("HazFdxAirOK", getImportFieldFlag("HazFdxAirOK", IntegrationTypes.Hazmat))
            dicUseField.Add("HazUPSgndOK", getImportFieldFlag("HazUPSgndOK", IntegrationTypes.Hazmat))
            dicUseField.Add("HazUPSAirOK", getImportFieldFlag("HazUPSAirOK", IntegrationTypes.Hazmat))

            RecordErrors = 0
            TotalRecords = 0
            Log("Importing " & oHazmats.Count & " Hazmat Records.")
            Using db As New LTSIntegrationDataDataContext(strConnection)
                For Each hz In oHazmats
                    'If Not hz Is Nothing Then
                    Dim hRegulation As String = hz.HazRegulation.Trim
                    Dim hItem As String = hz.HazItem.Trim
                    Dim hID As String = hz.HazID.Trim
                    If String.IsNullOrEmpty(hRegulation) OrElse String.IsNullOrEmpty(hItem) OrElse String.IsNullOrEmpty(hID) Then
                        Log("NGL.FreightMaster.Integration.clsHazmat.ProcessData Import Record Failure!  Invalid Key data fields,the value for Hazmat Regulation (" & hRegulation & "), Item (" & hItem & ") and ID (" & hID & ") are required and may not be left blank.")
                    Else
                        Dim oExist = (From h In db.Hazmats Where h.HazRegulation = hRegulation And h.HazItem = hItem And h.HazID = hID Select h).FirstOrDefault
                        If oExist Is Nothing Then
                            'we need to insert it into the table
                            Dim nH As New Hazmat With _
                                {.HazRegulation = hRegulation, _
                                 .HazItem = hItem, _
                                 .HazClass = hz.HazClass, _
                                 .HazID = hID, _
                                 .HazDesc01 = hz.HazDesc01, _
                                 .HazDesc02 = hz.HazDesc02, _
                                 .HazDesc03 = hz.HazDesc03, _
                                 .HazUnit = hz.HazUnit, _
                                 .HazPackingGroup = hz.HazPackingGroup, _
                                 .HazPackingDesc = hz.HazPackingDesc, _
                                 .HazShipInst = hz.HazShipInst, _
                                 .HazLtdQ = hz.HazLtdQ, _
                                 .HazMarPoll = hz.HazMarPoll, _
                                 .HazMarStorCat = hz.HazMarStorCat, _
                                 .HazNMFCSub = hz.HazNMFCSub, _
                                 .HazNMFC = hz.HazNMFC, _
                                 .HazFrtClass = hz.HazFrtClass, _
                                 .HazFdxGndOK = hz.HazFdxGndOK, _
                                 .HazFdxAirOK = hz.HazFdxAirOK, _
                                 .HazUPSgndOK = hz.HazUPSgndOK, _
                                 .HazUPSAirOK = hz.HazUPSAirOK, _
                                 .HazModDate = Me.CreatedDate, _
                                 .HazModUser = Me.CreateUser}
                            db.Hazmats.InsertOnSubmit(nH)
                        Else
                            'update the data but only if the import field flags are on
                            With oExist
                                If dicUseField("HazDesc01") Then .HazDesc01 = hz.HazDesc01
                                If dicUseField("HazDesc02") Then .HazDesc02 = hz.HazDesc02
                                If dicUseField("HazDesc03") Then .HazDesc03 = hz.HazDesc03
                                If dicUseField("HazUnit") Then .HazUnit = hz.HazUnit
                                If dicUseField("HazPackingGroup") Then .HazPackingGroup = hz.HazPackingGroup
                                If dicUseField("HazPackingDesc") Then .HazPackingDesc = hz.HazPackingDesc
                                If dicUseField("HazShipInst") Then .HazShipInst = hz.HazShipInst
                                If dicUseField("HazLtdQ") Then .HazLtdQ = hz.HazLtdQ
                                If dicUseField("HazMarPoll") Then .HazMarPoll = hz.HazMarPoll
                                If dicUseField("HazMarStorCat") Then .HazMarStorCat = hz.HazMarStorCat
                                If dicUseField("HazNMFCSub") Then .HazNMFCSub = hz.HazNMFCSub
                                If dicUseField("HazNMFC") Then .HazNMFC = hz.HazNMFC
                                If dicUseField("HazFrtClass") Then .HazFrtClass = hz.HazFrtClass
                                If dicUseField("HazFdxGndOK") Then .HazFdxGndOK = hz.HazFdxGndOK
                                If dicUseField("HazFdxAirOK") Then .HazFdxAirOK = hz.HazFdxAirOK
                                If dicUseField("HazUPSgndOK") Then .HazUPSgndOK = hz.HazUPSgndOK
                                If dicUseField("HazUPSAirOK") Then .HazUPSAirOK = hz.HazUPSAirOK
                                .HazModDate = Me.CreatedDate
                                .HazModUser = Me.CreateUser
                            End With
                        End If
                        Try
                            db.SubmitChanges()
                            TotalRecords += 1
                        Catch ex As System.Data.SqlClient.SqlException
                            RecordErrors += 1
                            ITEmailMsg &= "<br />" & Source & " Import Record Failure: NGL.FreightMaster.Integration.clsHazmat.ProcessData, attempted to import Hazmat record for ID, " & hz.HazID & ", from Data Integration DLL without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsHazmat.ProcessData Import Record Failure!" & readExceptionMessage(ex))
                        Catch ex As InvalidOperationException
                            RecordErrors += 1
                            ITEmailMsg &= "<br />" & Source & " Import Record Failure: NGL.FreightMaster.Integration.clsHazmat.ProcessData, attempted to import Hazmat record for ID, " & hz.HazID & ", from Data Integration DLL without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsHazmat.ProcessData Import Record Failure!" & readExceptionMessage(ex))
                        Catch ex As Exception
                            Throw
                        End Try
                    End If
                    'Else
                    '    AddToGroupEmailMsg("One of the hazmat records was null or empty and could not be processed.")
                    '    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                    'End If
                Next
            End Using
            strTitle = "Process Data Complete"
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Process Hazmat Data Warning", "The following errors or warnings were reported some records may not have been processed correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Process Hazmat Data Failure", "The following errors or warnings were reported some records may not have been processed correctly.." & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success!  " & Me.TotalRecords & " " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationComplete

                If Me.RecordErrors > 0 Then
                    strTitle = "Process Data Complete With Errors"
                    If Me.RecordErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & Me.HeaderName & " records could not be imported.  Please run the Hazmat Import Error Report for more information."
                    End If
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Else
                strMsg = "No " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            Log(strMsg)

        Catch ex As Exception
            intRet = ProcessDataReturnValues.nglDataIntegrationFailure
            LogException("Process Hazmat Data Failure", "Could not process the requested Hazmat data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsHazmat.ProcessData")
        Finally
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try
        Return intRet


    End Function

End Class
