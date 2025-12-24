Imports System.Data.SqlClient
Imports NGL.FreightMaster.Integration.Configuration
Imports DTran = Ngl.Core.Utility.DataTransformation

<Serializable()> _
Public Class clsUpload : Inherits clsImportExport


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

#Region " Class Variables and Properties """

    Private _intTimeout As Integer = 0
    Public Property CommandTimeOut() As Integer
        Get
            If _intTimeout < 300 Then
                _intTimeout = 300
            End If
            Return _intTimeout
        End Get
        Set(ByVal value As Integer)
            _intTimeout = value
        End Set
    End Property

    Private _intRowsAffected As Integer = 0
    Public Property RowsAffected() As Integer
        Get
            Return _intRowsAffected
        End Get
        Protected Set(ByVal value As Integer)
            _intRowsAffected = value
        End Set
    End Property

    Private _intMaxRowsReturned As Integer = 0
    Public Property MaxRowsReturned() As Integer
        Get
            Return _intMaxRowsReturned
        End Get
        Set(ByVal value As Integer)
            _intMaxRowsReturned = value
        End Set
    End Property

    Private _blnAutoConfimation As Boolean = False
    Public Property AutoConfirmation() As Boolean
        Get
            Return _blnAutoConfimation
        End Get
        Set(ByVal value As Boolean)
            _blnAutoConfimation = value
        End Set
    End Property



#End Region

#Region " Constructors "



#End Region

#Region " Functions "

    Protected Function lookupCarrierEquipmentCodesByPro(ByVal strBookProNumber As String) As String
        Dim strRet As String = ""
        Try
            If strBookProNumber <> "''" And strBookProNumber.ToUpper <> "NULL" Then
                'we have a valid pro number so check get the CarrierEquipmnetCodeString
                Dim strSQL As String = "Select dbo.udfGetBookCarrierEquipmentCodeString(NULL,'" & strBookProNumber & "') as CarrierEquipmentCodes"
                Dim intRetryCt As Integer = 0
                Do
                    intRetryCt += 1
                    Dim cmdObj As New System.Data.SqlClient.SqlCommand
                    Dim drTemp As SqlDataReader
                    Try
                        'check the active db connection
                        If Me.openConnection() Then
                            With cmdObj
                                .Connection = DBCon
                                .CommandTimeout = 300
                                .CommandText = strSQL
                                .CommandType = CommandType.Text
                                drTemp = .ExecuteReader()
                            End With
                            If drTemp.HasRows Then
                                With drTemp
                                    .Read()
                                    strRet = nz(.Item("CarrierEquipmentCodes"), "")
                                    .Close()
                                End With
                            End If
                            Exit Do
                        Else
                            If intRetryCt > Me.Retry Then
                                ITEmailMsg &= "<br />" & Source & " Failure: clsUpload.lookupCarrierEquipmentCodesByPro: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Data could not be processed correctly.<br />" & vbCrLf
                                Log("lookupCompControlByAlphaCode Failed!")
                            Else
                                Log("lookupCarrierEquipmentCodesByPro Open DB Connection Failure Retry = " & intRetryCt.ToString)
                            End If
                        End If
                    Catch ex As Exception
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: clsUpload.lookupCarrierEquipmentCodesByPro, attempted to read Data " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                            Log("lookupCarrierEquipmentCodesByPro Failed!" & readExceptionMessage(ex))
                        Else
                            Log("lookupCarrierEquipmentCodesByPro Failure Retry = " & intRetryCt.ToString)
                        End If
                    Finally
                        Try
#Disable Warning BC42104 ' Variable 'drTemp' is used before it has been assigned a value. A null reference exception could result at runtime.
                            drTemp.Close()
#Enable Warning BC42104 ' Variable 'drTemp' is used before it has been assigned a value. A null reference exception could result at runtime.
                        Catch ex As Exception

                        End Try
                        Try
                            cmdObj.Cancel()
                        Catch ex As Exception

                        End Try
                    End Try
                    'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
                Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
            Else
                strRet = "0"
            End If
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsUpload.lookupCarrierEquipmentCodesByPro: Could not read data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("lookupCarrierEquipmentCodesByPro Failed!" & readExceptionMessage(ex))
        End Try
        Return strRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="intBookControl"></param>
    ''' <param name="intCarrierControl"></param>
    ''' <param name="intCompControl"></param>
    ''' <param name="strXaction"></param>
    ''' <param name="strSenderCode"></param>
    ''' <param name="strReceiverCode"></param>
    ''' <param name="intISASequence"></param>
    ''' <param name="intGSSequence"></param>
    ''' <param name="strCarrierSCAC"></param>
    ''' <param name="strLoadNumber"></param>
    ''' <param name="strMessage"></param>
    ''' <param name="strCaller"></param>
    ''' <param name="blnEmailErrors"></param>
    ''' <param name="intRefControl"></param>
    ''' <remarks>
    ''' Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    ''' Added optional parameter intRefControl
    ''' </remarks>
    Protected Sub saveEDITransaction(ByVal intBookControl As Integer, _
                                  ByVal intCarrierControl As Integer, _
                                  ByVal intCompControl As Integer, _
                                  ByVal strXaction As String, _
                                  ByVal strSenderCode As String, _
                                  ByVal strReceiverCode As String, _
                                  ByVal intISASequence As Integer, _
                                  ByVal intGSSequence As Integer, _
                                  ByVal strCarrierSCAC As String, _
                                  ByVal strLoadNumber As String, _
                                  ByVal strMessage As String, _
                                  Optional ByVal strCaller As String = "Save EDI Transactions", _
                                  Optional ByVal blnEmailErrors As Boolean = True, _
                                  Optional ByVal intRefControl As Integer = 0)

        Dim strSQL As String = "EXEC dbo.spInserttblEDITrans " _
                    & intBookControl.ToString & ", " _
                    & intCarrierControl.ToString & ", " _
                    & intCompControl.ToString & ", " _
                    & DTran.buildSQLString(strXaction, 3) & ", " _
                    & DTran.buildSQLString(strSenderCode, 15) & ", " _
                    & DTran.buildSQLString(strReceiverCode, 15) & ", " _
                    & intISASequence.ToString & ", " _
                    & intGSSequence.ToString & ", " _
                    & DTran.buildSQLString(strCarrierSCAC, 255) & ", " _
                    & DTran.buildSQLString(strLoadNumber, 255) & ", " _
                    & DTran.buildSQLString(strMessage, 255) & ", " _
                    & intRefControl.ToString


        executeSQLQuery(strSQL, strCaller, blnEmailErrors)

    End Sub


#End Region
End Class
