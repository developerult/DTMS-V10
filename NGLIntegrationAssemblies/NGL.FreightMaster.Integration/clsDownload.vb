Imports System.Data.SqlClient
Imports Ngl.FreightMaster.Integration.Configuration
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DAL = Ngl.FreightMaster.Data
Imports System.ServiceModel

<Serializable()>
Public Class clsDownload : Inherits clsImportExport


#Region "Constructors"

    Sub New()
        MyBase.new()
    End Sub

    Sub New(ByVal config As Ngl.FreightMaster.Core.UserConfiguration)
        MyBase.New(config)
    End Sub

    Sub New(ByVal admin_email As String,
            ByVal from_email As String,
            ByVal group_email As String,
            ByVal auto_retry As Integer,
            ByVal smtp_server As String,
            ByVal db_server As String,
            ByVal database_catalog As String,
            ByVal auth_code As String,
            ByVal debug_mode As Boolean,
            Optional ByVal connection_string As String = "")

        MyBase.New(admin_email, from_email, group_email, auto_retry, smtp_server, db_server, database_catalog, auth_code, debug_mode, connection_string)


    End Sub

#End Region


    Public Enum OptimizerTypes
        optTruckStops
        optPCMiler
        optFaxServer
        optany
    End Enum

    Protected oCX As New CompXrefs
    Protected oFKL As New FKLookups
    Protected oEL As New ExistingLanes
    Protected oIF As New ImportFieldFlags

    Private mintUsePCMiler As Integer = -1
    Public ReadOnly Property UsePCMiler() As Boolean
        Get
            Select Case mintUsePCMiler
                Case -1
                    If useOptimizer(OptimizerTypes.optPCMiler) Then
                        mintUsePCMiler = 1
                        Return True
                    Else
                        mintUsePCMiler = 0
                        Log("UsePCMiler is off in parameter table.")
                        Return False
                    End If
                Case 1
                    Return True
                Case Else
                    Return False
            End Select
        End Get
    End Property


    Private mintTotalItems As Integer = 0
    Public Property TotalItems() As Integer
        Get
            TotalItems = mintTotalItems
        End Get
        Protected Set(ByVal Value As Integer)
            mintTotalItems = Value
        End Set
    End Property

    Private mintTotalCalendarRecords As Integer = 0
    Public Property TotalCalendarRecords() As Integer
        Get
            TotalCalendarRecords = mintTotalCalendarRecords
        End Get
        Protected Set(ByVal Value As Integer)
            mintTotalCalendarRecords = Value
        End Set
    End Property







    Protected Function lookupCompControlByAlphaCode(ByRef oField As clsImportField,
                                                    ByVal strNewFieldName As String) As Boolean
        Dim blnRet As Boolean = False
        Dim intCompControl As Integer = 0
        Dim intCompNumber As Integer = 0
        Try
            If oField.Value <> "''" And oField.Value.ToUpper <> "NULL" Then
                'we have a valid company number so check the in memory collection
                If Integer.TryParse(stripQuotes(oField.Value), intCompNumber) Then
                    'we have a numeric company number 
                    intCompControl = oCX.getControlByNumber(intCompNumber)
                Else
                    'this is an alpha company number
                    intCompControl = oCX.getControlByAlpha(stripQuotes(oField.Value))
                End If
                If intCompControl = 0 Then
                    'next check the alpha xref table
                    Dim strSQL As String = "Select dbo.udfGetCompControlByAlpha(" & oField.Value & ") as CompControl"
                    Dim intRetryCt As Integer = 0
                    Do
                        intRetryCt += 1
                        Dim cmdObj As New System.Data.SqlClient.SqlCommand
                        Dim drTemp As SqlDataReader
                        Dim oCon As System.Data.SqlClient.SqlConnection
                        Try
                            oCon = getNewConnection(False)
                            If Not oCon Is Nothing AndAlso oCon.State = ConnectionState.Open Then

                                With cmdObj
                                    .Connection = oCon
                                    .CommandTimeout = 300
                                    .CommandText = strSQL
                                    .CommandType = CommandType.Text
                                    drTemp = .ExecuteReader()
                                End With
                                If drTemp.HasRows Then
                                    With drTemp
                                        .Read()
                                        If nz(.Item("CompControl"), 0) > 0 Then
                                            oField.DataType = clsImportField.DataTypeID.gcvdtLongInt
                                            oField.Length = 11
                                            oField.Name = strNewFieldName
                                            oField.Value = nz(.Item("CompControl"), 0)
                                            blnRet = True
                                        End If
                                        .Close()
                                    End With
                                End If
                                Exit Do
                            Else
                                If intRetryCt > Me.Retry Then
                                    ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupCompControlByAlphaCode: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Company number " & oField.Value & " could not be processed correctly.<br />" & vbCrLf
                                    Log("lookupCompControlByAlphaCode Failed!")
                                Else
                                    Log("lookupCompControlByAlphaCode Open DB Connection Failure Retry = " & intRetryCt.ToString)
                                End If
                            End If
                        Catch ex As Exception
                            If intRetryCt > Me.Retry Then
                                ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupCompControlByAlphaCode, attempted to read Company number " & oField.Value & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                                Log("lookupCompControlByAlphaCode Failed!" & readExceptionMessage(ex))
                            Else
                                Log("lookupCompControlByAlphaCode Failure Retry = " & intRetryCt.ToString)
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
                            Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                                If Not oCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                                    If oCon.State = ConnectionState.Open Then
                                        oCon.Close()
                                    End If
                                End If
                                oCon = Nothing
                            Catch ex As Exception

                            End Try
                        End Try
                        'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
                    Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
                Else
                    oField.Name = strNewFieldName
                    oField.Value = intCompControl
                    oField.DataType = clsImportField.DataTypeID.gcvdtLongInt
                    oField.Length = 11
                    blnRet = True
                End If
            Else
                oField.Name = strNewFieldName
                oField.Value = 0
                oField.DataType = clsImportField.DataTypeID.gcvdtLongInt
                oField.Length = 11
                blnRet = True
            End If
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupCompControlByAlphaCode: Could not read Company number " & oField.Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("lookupCompControlByAlphaCode Failed!" & readExceptionMessage(ex))
        End Try
        Return blnRet
    End Function

    Protected Function lookupCompNumberByAlphaCode(ByRef oField As clsImportField) As Boolean
        Dim blnRet As Boolean = False
        Try
            If oField.Value <> "''" And oField.Value.ToUpper <> "NULL" Then
                'we have a valid company number so check the alpha x-ref table
                Dim strSQL As String = "Select dbo.udfGetCompNumberByAlpha(" & oField.Value & ") as CompNumber"
                Dim intRetryCt As Integer = 0
                Do
                    intRetryCt += 1
                    Dim cmdObj As New System.Data.SqlClient.SqlCommand
                    Dim drTemp As SqlDataReader
                    Dim oCon As System.Data.SqlClient.SqlConnection
                    Try
                        oCon = getNewConnection(False)
                        If Not oCon Is Nothing AndAlso oCon.State = ConnectionState.Open Then

                            With cmdObj
                                .Connection = oCon
                                .CommandTimeout = 300
                                .CommandText = strSQL
                                .CommandType = CommandType.Text
                                drTemp = .ExecuteReader()
                            End With
                            If drTemp.HasRows Then
                                With drTemp
                                    .Read()
                                    oField.Value = nz(.Item("CompNumber"), "0")
                                    blnRet = True
                                    .Close()
                                End With
                            End If
                            Exit Do
                        Else
                            If intRetryCt > Me.Retry Then
                                ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupCompControlByAlphaCode: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Company number " & oField.Value & " could not be processed correctly.<br />" & vbCrLf
                                Log("lookupCompControlByAlphaCode Failed!")
                            Else
                                Log("lookupCompNumberByAlphaCode Open DB Connection Failure Retry = " & intRetryCt.ToString)
                            End If
                        End If
                    Catch ex As Exception
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupCompControlByAlphaCode, attempted to read Company number " & oField.Value & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                            Log("lookupCompControlByAlphaCode Failed!" & readExceptionMessage(ex))
                        Else
                            Log("lookupCompNumberByAlphaCode Failure Retry = " & intRetryCt.ToString)
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
                        Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                            If Not oCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                                If oCon.State = ConnectionState.Open Then
                                    oCon.Close()
                                End If
                            End If
                            oCon = Nothing
                        Catch ex As Exception

                        End Try
                    End Try
                    'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
                Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
            Else
                oField.Value = "0"
                blnRet = True
            End If
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupCompControlByAlphaCode: Could not read Company number " & oField.Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("lookupCompControlByAlphaCode Failed!" & readExceptionMessage(ex))
        End Try
        Return blnRet
    End Function

    Protected Function lookupCompNumberByAlphaCode(ByVal strCompNumber As String) As String
        Dim strRet As String = ""
        Try
            If strCompNumber <> "''" And strCompNumber.ToUpper <> "NULL" Then
                'we have a valid company number so check the alpha x-ref table
                Dim strSQL As String = "Select dbo.udfGetCompNumberByAlpha(" & strCompNumber & ") as CompNumber"
                Dim intRetryCt As Integer = 0
                Do
                    intRetryCt += 1
                    Dim cmdObj As New System.Data.SqlClient.SqlCommand
                    Dim drTemp As SqlDataReader
                    Dim oCon As System.Data.SqlClient.SqlConnection
                    Try
                        oCon = getNewConnection(False)
                        If Not oCon Is Nothing AndAlso oCon.State = ConnectionState.Open Then

                            With cmdObj
                                .Connection = oCon
                                .CommandTimeout = 300
                                .CommandText = strSQL
                                .CommandType = CommandType.Text
                                drTemp = .ExecuteReader()
                            End With
                            If drTemp.HasRows Then
                                With drTemp
                                    .Read()
                                    strRet = nz(.Item("CompNumber"), "0")
                                    .Close()
                                End With
                            End If
                            Exit Do
                        Else
                            If intRetryCt > Me.Retry Then
                                ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupCompControlByAlphaCode: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Company data could not be processed correctly.<br />" & vbCrLf
                                Log("lookupCompControlByAlphaCode Failed!")
                            Else
                                Log("lookupCompNumberByAlphaCode Open DB Connection Failure Retry = " & intRetryCt.ToString)
                            End If
                        End If
                    Catch ex As Exception
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupCompControlByAlphaCode, attempted to read Company data " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                            Log("lookupCompControlByAlphaCode Failed!" & readExceptionMessage(ex))
                        Else
                            Log("lookupCompNumberByAlphaCode Failure Retry = " & intRetryCt.ToString)
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
                        Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                            If Not oCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                                If oCon.State = ConnectionState.Open Then
                                    oCon.Close()
                                End If
                            End If
                            oCon = Nothing
                        Catch ex As Exception

                        End Try
                    End Try
                    'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
                Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
            Else
                strRet = "0"
            End If
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupCompControlByAlphaCode: Could not read company data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("lookupCompControlByAlphaCode Failed!" & readExceptionMessage(ex))
        End Try
        Return strRet
    End Function

    Protected Function lookupDefaultCarrier(ByRef oField As clsImportField, ByVal strNewFieldName As String) As Boolean
        Dim blnRet As Boolean = False
        Try
            If Val(oField.Value) > 0 Then
                'we have a valid carrier number so check the carrier table
                Dim strSQL As String = "Select CarrierControl From Carrier Where CarrierNumber = " & Val(oField.Value)
                Dim intRetryCt As Integer = 0
                Do
                    intRetryCt += 1
                    Dim cmdObj As New System.Data.SqlClient.SqlCommand
                    Dim drTemp As SqlDataReader
                    Dim oCon As System.Data.SqlClient.SqlConnection
                    Try
                        oCon = getNewConnection(False)
                        If Not oCon Is Nothing AndAlso oCon.State = ConnectionState.Open Then

                            With cmdObj
                                .Connection = oCon
                                .CommandTimeout = 300
                                .CommandText = strSQL
                                .CommandType = CommandType.Text
                                drTemp = .ExecuteReader()
                            End With
                            If drTemp.HasRows Then
                                With drTemp
                                    .Read()
                                    If nz(.Item("CarrierControl"), 0) > 0 Then
                                        oField.Name = strNewFieldName
                                        oField.Value = nz(.Item("CarrierControl"), 0)
                                        blnRet = True
                                    End If
                                    .Close()
                                End With
                            End If
                            Exit Do
                        Else
                            If intRetryCt > Me.Retry Then
                                ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupDefaultCarrier: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Carrier number " & oField.Value & " could not be processed correctly.<br />" & vbCrLf
                                Log("lookupDefaultCarrier Failed!")
                            Else
                                Log("lookupDefaultCarrier Open DB Connection Failure Retry = " & intRetryCt.ToString)
                            End If
                        End If
                    Catch ex As Exception
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupDefaultCarrier, attempted to read Carrier number " & oField.Value & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                            Log("lookupDefaultCarrier Failed!" & readExceptionMessage(ex))
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
                        Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                            If Not oCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                                If oCon.State = ConnectionState.Open Then
                                    oCon.Close()
                                End If
                            End If
                            oCon = Nothing
                        Catch ex As Exception

                        End Try
                    End Try
                    'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
                Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
            Else
                oField.Name = strNewFieldName
                oField.Value = "0"
                blnRet = True
            End If
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupDefaultCarrier: Could not read Carrier number " & oField.Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("lookupDefaultCarrier Failed!" & readExceptionMessage(ex))
        End Try
        Return blnRet
    End Function

    Protected Function lookupCompControlByNumber(ByVal CompNumber As String) As Integer
        Dim intRet As Integer = 0
        Try
            Dim strSQL As String = "Select Top 1 CompControl From Comp Where CompNumber = '" & CompNumber & "'"
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim cmdObj As New System.Data.SqlClient.SqlCommand
                Dim oCon As System.Data.SqlClient.SqlConnection
                Try
                    oCon = getNewConnection(False)
                    If Not oCon Is Nothing AndAlso oCon.State = ConnectionState.Open Then

                        With cmdObj
                            .Connection = oCon
                            .CommandTimeout = 300
                            .CommandText = strSQL
                            .CommandType = CommandType.Text
                            intRet = .ExecuteScalar
                        End With
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupCompControlByNumber: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Company number " & CompNumber & " could not be processed correctly.<br />" & vbCrLf
                            Log("lookupCompControlByNumber Failed!")
                        Else
                            Log("lookupCompControlByNumber Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupCompControlByNumber, attempted to read Company number " & CompNumber & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                        Log("lookupCompControlByNumber Failed!" & readExceptionMessage(ex))
                    End If
                Finally
                    Try
                        cmdObj.Cancel()
                    Catch ex As Exception

                    End Try
                    Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                        If Not oCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                            If oCon.State = ConnectionState.Open Then
                                oCon.Close()
                            End If
                        End If
                        oCon = Nothing
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.

        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupCompControlByNumber: Could not read Company number " & CompNumber & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("lookupCompControlByNumber Failed!" & readExceptionMessage(ex))
        End Try
        Return intRet
    End Function

    Protected Function lookupCompAddress(ByRef oCompField As clsImportField,
                                        ByRef oNameField As clsImportField,
                                        ByRef oAdd1Field As clsImportField,
                                        ByRef oAdd2Field As clsImportField,
                                        ByRef oAdd3Field As clsImportField,
                                        ByRef oCityField As clsImportField,
                                        ByRef oStateField As clsImportField,
                                        ByRef oCountryField As clsImportField,
                                        ByRef oZipField As clsImportField,
                                        ByRef oPhoneField As clsImportField,
                                        ByRef oFaxField As clsImportField,
                                        ByVal strNewFieldName As String) As Boolean
        Dim blnRet As Boolean = False
        Try
            Dim strCompFieldValue As String = Trim(stripQuotes(oCompField.Value))
            If strCompFieldValue <> "0" And strCompFieldValue <> "" And strCompFieldValue.ToUpper <> "NULL" Then
                'we have a valid company number so check th alpha x-ref table
                If Not lookupCompControlByAlphaCode(oCompField, strNewFieldName) Then
                    Return False
                End If
                If Val(oCompField.Value) < 1 Then Return False
                'Use the new CompControl Number to get the address information
                Dim strSQL As String = "Select CompControl, CompName,CompStreetAddress1,CompStreetAddress2,CompStreetAddress3,CompStreetCity,CompStreetState,CompStreetCountry,CompStreetZip From Comp Where CompControl = " & Val(oCompField.Value)
                Dim intRetryCt As Integer = 0
                Do
                    intRetryCt += 1
                    Dim cmdObj As New System.Data.SqlClient.SqlCommand
                    Dim drTemp As SqlDataReader
                    Dim oCon As System.Data.SqlClient.SqlConnection
                    Try
                        oCon = getNewConnection(False)
                        If Not oCon Is Nothing AndAlso oCon.State = ConnectionState.Open Then


                            With cmdObj
                                .Connection = oCon
                                .CommandTimeout = 300
                                .CommandText = strSQL
                                .CommandType = CommandType.Text
                                drTemp = .ExecuteReader()
                            End With
                            If drTemp.HasRows Then
                                With drTemp
                                    .Read()
                                    oNameField.Value = "'" & padQuotes(nz(.Item("CompName"), "")) & "'"
                                    oAdd1Field.Value = "'" & padQuotes(nz(.Item("CompStreetAddress1"), "")) & "'"
                                    oAdd2Field.Value = "'" & padQuotes(nz(.Item("CompStreetAddress2"), "")) & "'"
                                    oAdd3Field.Value = "'" & padQuotes(nz(.Item("CompStreetAddress3"), "")) & "'"
                                    oCityField.Value = "'" & padQuotes(nz(.Item("CompStreetCity"), "")) & "'"
                                    oStateField.Value = "'" & padQuotes(nz(.Item("CompStreetState"), "")) & "'"
                                    oCountryField.Value = "'" & padQuotes(nz(.Item("CompStreetCountry"), "")) & "'"
                                    oZipField.Value = "'" & padQuotes(nz(.Item("CompStreetZip"), "")) & "'"
                                    .Close()
                                End With
                            End If
                            'now get the contact information
                            strSQL = "Select top 1 CompContPhone,CompContFax from Compcont Where compcontCompcontrol = " & Val(oCompField.Value)
                            With cmdObj
                                .CommandText = strSQL
                                drTemp = .ExecuteReader()
                            End With
                            If drTemp.HasRows Then
                                With drTemp
                                    .Read()
                                    oPhoneField.Value = "'" & padQuotes(nz(.Item("CompContPhone"), "")) & "'"
                                    oFaxField.Value = "'" & padQuotes(nz(.Item("CompContFax"), "")) & "'"
                                    .Close()
                                End With
                            End If
                            blnRet = True
                            Exit Do
                        Else
                            If intRetryCt > Me.Retry Then
                                ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupCompAddress: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Company number " & oCompField.Value & " could not be processed correctly.<br />" & vbCrLf
                                Log("lookupCompAddress Failed!")
                            Else
                                Log("lookupCompAddress Open DB Connection Failure Retry = " & intRetryCt.ToString)
                            End If
                        End If
                    Catch ex As Exception
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupCompAddress, attempted to read Company number " & oCompField.Value & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                            Log("lookupCompAddress Failed!" & readExceptionMessage(ex))
                        Else
                            Log("lookupCompAddress Failure Retry = " & intRetryCt.ToString)
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
                        Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                            If Not oCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                                If oCon.State = ConnectionState.Open Then
                                    oCon.Close()
                                End If
                            End If
                            oCon = Nothing
                        Catch ex As Exception

                        End Try
                    End Try
                    'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
                Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
            Else
                oCompField.Name = strNewFieldName
                oCompField.Value = "0"
                blnRet = True
            End If
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupCompAddress: Could not read Company number " & oCompField.Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("lookupCompAddress Failed!" & readExceptionMessage(ex))
        End Try
        Return blnRet
    End Function

    Protected Function getLaneControl(ByRef oKeyField As clsImportField) As Integer
        Dim Ret As Integer = 0
        Dim cmdObj As New System.Data.SqlClient.SqlCommand
        Dim drTemp As SqlDataReader
        Try
            Dim strSQL As String = "Select LaneControl From Lane Where LaneNumber = " & oKeyField.Value
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim oCon As System.Data.SqlClient.SqlConnection
                Try
                    oCon = getNewConnection(False)
                    If Not oCon Is Nothing AndAlso oCon.State = ConnectionState.Open Then


                        With cmdObj
                            .Connection = oCon
                            .CommandTimeout = 300
                            .CommandText = strSQL
                            .CommandType = CommandType.Text
                            drTemp = .ExecuteReader()
                        End With
                        If drTemp.HasRows Then
                            drTemp.Read()
                            If drTemp.IsDBNull(0) Then
                                Return 0
                            Else
                                Return drTemp.Item(0)
                            End If
                            drTemp.Close()
                        Else
                            Return 0
                        End If
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.getLaneControl: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Lane number " & oKeyField.Value & " could not be processed correctly.<br />" & vbCrLf
                            Log("getLaneControl Failed!")
                        Else
                            Log("getLaneControl Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.getLaneControl, attempted to read Lane number " & oKeyField.Value & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                        Log("getLaneControl Failed!" & readExceptionMessage(ex))
                    Else
                        Log("getLaneControl Failure Retry = " & intRetryCt.ToString)
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
                    Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                        If Not oCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                            If oCon.State = ConnectionState.Open Then
                                oCon.Close()
                            End If
                        End If
                        oCon = Nothing
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.getLaneControl: Could not read Lane number " & oKeyField.Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("getLaneControl Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret
    End Function

    Protected Function getProNumber(ByRef oFields As clsImportFields) As Boolean
        Dim blnRet As Boolean = False
        Dim cmdObj As New System.Data.SqlClient.SqlCommand
        Dim drTemp As SqlDataReader

        Try
            'modified by RHR for v-8.2 on 3/3/2019 if single wrapper quotes exist in the pro number we were not looking up the correct pro
            Dim strProNumber As String = stripQuotes(oFields("BookProNumber").Value)
            Dim strCompNumber As String = Val(oFields("CompNumber").Value)
            Dim strOrderNumber As String = oFields("BookCarrOrderNumber").Value
            Dim intOrderSequence As Integer = oFields("BookOrderSequence").Value
            Dim intCompNumber As Integer = 0

            If strProNumber.Trim.Length < 1 Or strProNumber = "NULL" Then
                'Get the company number if needed.
                intCompNumber = lookupCompNumberByAlphaCode(strCompNumber)
                'look up the pro number using the order , order sequence and the company number
                If intCompNumber > 0 AndAlso strOrderNumber <> "NULL" Then

                    Dim strSQL As String = "Select BookProNumber From dbo.Book Inner Join dbo.Comp on dbo.Book.BookCustCompControl = dbo.Comp.CompControl " _
                        & " Where BookCarrOrderNumber = " _
                        & oFields("BookCarrOrderNumber").Value & " AND BookOrderSequence = " & Val(oFields("BookOrderSequence").Value) & " AND CompNumber = " & Val(oFields("CompNumber").Value)
                    Dim intRetryCt As Integer = 0
                    Do
                        intRetryCt += 1
                        Dim oCon As System.Data.SqlClient.SqlConnection
                        Try
                            oCon = getNewConnection(False)
                            If Not oCon Is Nothing AndAlso oCon.State = ConnectionState.Open Then


                                With cmdObj
                                    .Connection = oCon
                                    .CommandTimeout = 300
                                    .CommandText = strSQL
                                    .CommandType = CommandType.Text
                                    drTemp = .ExecuteReader()
                                End With
                                If drTemp.HasRows Then
                                    drTemp.Read()
                                    oFields("BookProNumber").Value = validateSQLValue(
                                        drTemp.Item("BookProNumber"),
                                        Val(oFields("BookProNumber").DataType),
                                        Source,
                                        "Invalid " & oFields("BookProNumber").Name,
                                        oFields("BookProNumber").Null,
                                        oFields("BookProNumber").Length)
                                    blnRet = True
                                    drTemp.Close()
                                Else
                                    ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.getProNumber: A valid Pro Number could not be found and the record could not be imported.  The selection query is: " & vbCrLf & "<hr \>" & strSQL & "<hr \>" & vbCrLf
                                    Log("getProNumber Failed!")
                                End If
                                Try
                                    drTemp.Close()
                                Catch ex As Exception

                                End Try
                                Exit Do
                            Else
                                If intRetryCt > Me.Retry Then
                                    ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.getProNumber: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Could not read from the book table.<br />" & vbCrLf
                                    Log("getProNumber Failed!")
                                Else
                                    Log("getProNumber Open DB Connection Failure Retry = " & intRetryCt.ToString)
                                End If
                            End If
                        Catch ex As Exception
                            If intRetryCt > Me.Retry Then
                                ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.getProNumber, attempted to read from  book table " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                                Log("getProNumber Failed!" & readExceptionMessage(ex))
                            Else
                                Log("getProNumber Failure Retry = " & intRetryCt.ToString)
                            End If
                        Finally
                            Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                                If Not oCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                                    If oCon.State = ConnectionState.Open Then
                                        oCon.Close()
                                    End If
                                End If
                                oCon = Nothing
                            Catch ex As Exception

                            End Try
                        End Try
                        'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
                    Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
                Else
                    ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.getProNumber, a valid Pro Number could not be found and the record could not be imported for order number " & strOrderNumber & " and company number " & intCompNumber.ToString & ".<br />" & vbCrLf
                    Log("getProNumber Failed!")
                End If
            Else
                'return true because a pro number is provided
                blnRet = True
            End If
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.getProNumber: Could not read from the book table.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("getProNumber Failed!" & readExceptionMessage(ex))
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
        Return blnRet
    End Function
    ''' <summary>
    ''' Note: this funciton has been depreciated as of v-4.8 all methods
    ''' should now call the expanded overloaded function 
    ''' </summary>
    ''' <param name="oFields"></param>
    ''' <param name="strTableName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function doesRecordExist(ByRef oFields As clsImportFields,
                                        ByVal strTableName As String) As Integer
        Dim intRet As Integer = -1
        Dim cmdObj As New System.Data.SqlClient.SqlCommand
        Dim drTemp As SqlDataReader
        Try
            Dim strSQL As String = "Select "
            Dim strValues As String = " Where "
            Dim blnKeyFound As Boolean = False
            For intct As Integer = 1 To oFields.Count
                If oFields(intct).Use Then
                    If oFields(intct).PK = gcPK Or oFields(intct).PK = gcFK Then
                        If blnKeyFound Then
                            strSQL &= " , "
                            strValues &= " AND "
                        Else
                            blnKeyFound = True
                        End If
                        strSQL &= oFields(intct).Name
                        strValues &= oFields(intct).Name & " = " & oFields(intct).Value
                    End If
                End If
            Next
            strSQL &= " From " & strTableName & strValues
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim oCon As System.Data.SqlClient.SqlConnection
                Try
                    oCon = getNewConnection(False)
                    If Not oCon Is Nothing AndAlso oCon.State = ConnectionState.Open Then


                        With cmdObj
                            .Connection = oCon
                            .CommandTimeout = 300
                            .CommandText = strSQL
                            .CommandType = CommandType.Text
                            drTemp = .ExecuteReader()
                        End With
                        Dim blnHasRows As Boolean = drTemp.HasRows
                        Try
                            drTemp.Close()
                        Catch ex As Exception

                        End Try
                        If blnHasRows Then
                            Return 1
                        Else
                            Return 0
                        End If
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.doesRecordExist: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Could not read from  " & strTableName & " table.<br />" & vbCrLf
                            Log("doesRecordExist Failed!")
                        Else
                            Log("doesRecordExist Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.doesRecordExist, attempted to read from  " & strTableName & " table " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                        Log("doesRecordExist Failed!" & readExceptionMessage(ex))
                    Else
                        Log("doesRecordExist Failure Retry = " & intRetryCt.ToString)
                    End If
                Finally
                    Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                        If Not oCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                            If oCon.State = ConnectionState.Open Then
                                oCon.Close()
                            End If
                        End If
                        oCon = Nothing
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.doesRecordExist, could not read from  " & strTableName & " table.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("doesRecordExist Failed!" & readExceptionMessage(ex))
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
        Return intRet
    End Function

    Protected Function doesRecordExist(ByRef oFields As clsImportFields,
                                       ByRef strErrorMessage As String,
                                       ByRef blnInsertRecord As Boolean,
                                       ByVal strKeyFieldMsg As String,
                                       ByVal strTableName As String) As Boolean
        Dim blnRet As Boolean = False
        Dim cmdObj As New System.Data.SqlClient.SqlCommand
        Dim drTemp As SqlDataReader
        Try
            Dim strSQL As String = "Select "
            Dim strValues As String = " Where "
            Dim blnKeyFound As Boolean = False
            For intct As Integer = 1 To oFields.Count
                If oFields(intct).Use Then
                    'Test For Key fiels and Null values we do not include fields that are null in the query
                    If (oFields(intct).PK = gcPK Or oFields(intct).PK = gcFK) And (oFields(intct).Value.ToUpper <> "NULL") Then
                        If blnKeyFound Then
                            strSQL &= " , "
                            strValues &= " AND "
                        Else
                            blnKeyFound = True
                        End If
                        strSQL &= oFields(intct).Name
                        strValues &= oFields(intct).Name & " = " & oFields(intct).Value
                    End If
                End If
            Next
            strSQL &= " From " & strTableName & strValues
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim oCon As System.Data.SqlClient.SqlConnection
                Try
                    oCon = getNewConnection(False)
                    If Not oCon Is Nothing AndAlso oCon.State = ConnectionState.Open Then
                        With cmdObj
                            .Connection = oCon
                            .CommandTimeout = 300
                            .CommandText = strSQL
                            .CommandType = CommandType.Text
                            drTemp = .ExecuteReader()
                        End With
                        Dim blnHasRows As Boolean = drTemp.HasRows
                        Try
                            drTemp.Close()
                        Catch ex As Exception

                        End Try
                        If blnHasRows Then
                            blnInsertRecord = False
                        Else
                            blnInsertRecord = True
                        End If
                        Return True
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.doesRecordExist: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Could not read from  " & strTableName & " table.<br />" & vbCrLf
                            Log("doesRecordExist Failed!")
                            strErrorMessage = "Could not check for existing record.  The " & strKeyFieldMsg & " has not been imported because of a database connection failure."
                        Else
                            Log("doesRecordExist Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.doesRecordExist, attempted to read from  " & strTableName & " table " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                        Log("doesRecordExist Failed!" & readExceptionMessage(ex))
                        strErrorMessage = "Could not check for existing record.  The " & strKeyFieldMsg & " has not been imported because the " & strTableName & " is not available."
                    Else
                        Log("doesRecordExist Failure Retry = " & intRetryCt.ToString)
                    End If
                Finally
                    Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                        If Not oCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                            If oCon.State = ConnectionState.Open Then
                                oCon.Close()
                            End If
                        End If
                        oCon = Nothing
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.doesRecordExist, could not read from  " & strTableName & " table.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("doesRecordExist Failed!" & readExceptionMessage(ex))
            strErrorMessage = "Could not check for existing record.  The " & strKeyFieldMsg & " has not been imported because the " & strTableName & " is not available."
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
        Return blnRet
    End Function

    Protected Function saveData(ByRef oFields As clsImportFields,
                                        ByVal blnInsertRecord As Boolean,
                                        ByVal strTableName As String,
                                        Optional ByVal strModUserField As String = "",
                                        Optional ByVal strModDateField As String = "",
                                        Optional ByVal blnIgnoreBlanksOnUpdate As Boolean = False) As Boolean
        Dim Ret As Boolean = False
        Dim strSQL As String = ""
        Dim strQry As String = ""
        Dim strValues As String = ""
        Dim sSpacer As String = ""
        Try

            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                If intRetryCt > 1 And blnInsertRecord Then
                    'we must confirm that the record was not partially created by a previous failed insert
                    If Not doesRecordExist(oFields,
                                            "Checking for errors on insert to avoid duplicates",
                                            blnInsertRecord,
                                            strTableName & " Record ",
                                            strTableName) Then
                        Return False
                        Exit Do
                    End If
                End If
                If blnInsertRecord Then
                    'build execute string to insert record into Header table
                    strSQL = "Insert Into " & strTableName & " ("
                    strValues = " Values ( "
                    sSpacer = ""
                    For intct As Integer = 1 To oFields.Count
                        'For insert we ignore the .Use flag and import all data except for fields marked as hidden key or ingore
                        If oFields(intct).PK <> gcHK And oFields(intct).PK <> gcIgnore Then
                            'add all fields that are not marked as hidden or ignore (hidden fields are used for lookup only)                        
                            strSQL &= sSpacer & oFields(intct).Name
                            strValues &= sSpacer & oFields(intct).Value
                            sSpacer = ","
                        End If
                    Next
                    If strModUserField.Trim.Length > 0 Then
                        strSQL &= " , " & strModUserField
                        strValues &= ",'" & mstrCreateUser & "'"
                    End If
                    If strModDateField.Trim.Length > 0 Then
                        strSQL &= " , " & strModDateField
                        strValues &= ",'" & mstrCreatedDate & "'"
                    End If
                    strSQL = strSQL & " ) " & strValues & " ) "
                    'debug.print " Insert Header SQL = " & strSQL
                Else
                    'build sql string to update current record
                    strSQL = "Update " & strTableName & " Set "
                    strValues = " Where "
                    Dim blnKeyFound As Boolean = False
                    sSpacer = ""
                    For intct As Integer = 1 To oFields.Count
                        If oFields(intct).Use And oFields(intct).PK <> gcHK Then
                            If (oFields(intct).PK = gcPK Or oFields(intct).PK = gcFK) And (oFields(intct).Value.ToUpper <> "NULL") Then
                                If blnKeyFound Then
                                    strValues &= " AND "
                                Else
                                    blnKeyFound = True
                                End If
                                strValues &= oFields(intct).Name & " = " & oFields(intct).Value
                            ElseIf oFields(intct).PK <> gcIgnore Then
                                'add the update string for the current field
                                If blnIgnoreBlanksOnUpdate Then
                                    'skip blanks or nulls
                                    Select Case oFields(intct).Value
                                        Case "NULL"
                                            'do nothing
                                        Case "''"
                                            'do nothing
                                        Case Else
                                            strSQL &= sSpacer & oFields(intct).Name & " = " & oFields(intct).Value
                                            sSpacer = ","
                                    End Select
                                Else
                                    'update all data even blanks or nulls based on use flag
                                    strSQL &= sSpacer & oFields(intct).Name & " = " & oFields(intct).Value
                                    sSpacer = ","
                                End If
                            End If
                        End If
                    Next
                    If strModUserField.Trim.Length > 0 Then
                        strSQL &= " , " & strModUserField & " = '" & mstrCreateUser & "'"
                    End If
                    If strModDateField.Trim.Length > 0 Then
                        strSQL &= " , " & strModDateField & " = '" & mstrCreatedDate & "'"
                    End If
                    strSQL &= " From " & strTableName & strValues
                    'Debug.Print " Update Header SQL = " & strSQL
                    'check how many records will be affected by the query
                    strQry = "select * from " & strTableName & strValues
                End If
                Dim cmd As New SqlCommand
                Dim drTemp As SqlDataReader
                Dim intRecCt As Integer = 0
                Dim oCon As System.Data.SqlClient.SqlConnection
                Try
                    oCon = getNewConnection(False)
                    If Not oCon Is Nothing AndAlso oCon.State = ConnectionState.Open Then


                        cmd = New SqlCommand()
                        cmd.Connection = oCon
                        cmd.CommandTimeout = 300
                        If blnInsertRecord Then
                            intRecCt = 1
                        Else
                            cmd.CommandType = CommandType.Text
                            cmd.CommandText = strQry
                            drTemp = cmd.ExecuteReader()
                            If drTemp.HasRows Then
                                Do While drTemp.Read()
                                    intRecCt += 1
                                    If intRecCt > 1 Then Exit Do
                                Loop
                            End If
                            Try
                                drTemp.Close()
                            Catch ex As Exception

                            End Try
                        End If

                        If intRecCt = 1 Then
                            ''note: example to get control nunmber back INSERT INTO Mem_Basic(Mem_Na,Mem_Occ) VALUES(@na,@occ); SELECT SCOPE_IDENTITY()"
                            'only works when inserting a single row.  check the query.
                            cmd = New SqlCommand(strSQL, oCon)
                            cmd.ExecuteScalar()
                            Return True
                        ElseIf intRecCt > 1 Then
                            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.saveData. Too Many Records Would Be Updated!  Could not write to  " & strTableName & " table using query: " & vbCrLf & "<hr \>" & strSQL & "<hr \>" & vbCrLf
                            Log("saveData Failed!")
                        Else
                            Log("saveData failure; no records exist for query:  " & vbCrLf & "******************" & vbCrLf & strQry & vbCrLf & "******************" & vbCrLf)
                        End If
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.saveData: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Could not write to  " & strTableName & " table.<br />" & vbCrLf
                            Log("saveData Failed!")
                        Else
                            Log("saveData Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As System.Data.SqlClient.SqlException
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.saveData, attempted to write to  " & strTableName & " table " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                        Log("saveData Failed!" & readExceptionMessage(ex))
                    Else
                        Log("saveData Failure Retry = " & intRetryCt.ToString)

                    End If
                Catch ex As Exception
                    Throw
                Finally
                    Try
#Disable Warning BC42104 ' Variable 'drTemp' is used before it has been assigned a value. A null reference exception could result at runtime.
                        If Not drTemp Is Nothing Then drTemp.Close()
#Enable Warning BC42104 ' Variable 'drTemp' is used before it has been assigned a value. A null reference exception could result at runtime.
                    Catch ex As Exception

                    End Try
                    Try
                        If Not cmd Is Nothing Then cmd.Cancel()
                    Catch ex As Exception

                    End Try
                    Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                        If Not oCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                            If oCon.State = ConnectionState.Open Then
                                oCon.Close()
                            End If
                        End If
                        oCon = Nothing
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.saveData: Could not write to  " & strTableName & " table.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("saveData Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret
    End Function

    Protected Function lookupFKValues(
                    ByRef oItems As clsImportFields,
                    ByRef strErrorMessage As String,
                    ByVal strHeaderTable As String,
                    ByVal strSource As String,
                    ByVal strKeyFieldMsg As String) As Boolean
        Dim Ret As Boolean = False
        Dim strSQL As String = ""
        Dim strValues As String = ""
        Dim blnFirstField As Boolean = True
        Dim intCompNumber As Integer = 0
        Try
            Dim intRetryCt As Integer = 0
            For intct As Integer = 1 To oItems.Count
                If oItems(intct).PK = gcFK Then
                    Dim oField As clsImportField = oItems(intct)
                    Dim oKeyField As clsImportField = oItems(oItems(intct).FK_Key)
                    'check for a version stored in memory
                    Dim strValue As String = oFKL.getFKValue(oKeyField.Name, oKeyField.Value, strHeaderTable, oField.Parent_Field)
                    If String.IsNullOrEmpty(strValue) Then
                        Do
                            intRetryCt += 1
                            Dim cmdObj As New SqlCommand
                            Dim oCon As System.Data.SqlClient.SqlConnection
                            Try
                                oCon = getNewConnection(False)
                                If Not oCon Is Nothing AndAlso oCon.State = ConnectionState.Open Then

                                    'Get the current highest value available
                                    strSQL = "Select Top 1 cast(" & oField.Parent_Field & " as nvarchar(50)) as RetVal " _
                                        & " From " & strHeaderTable _
                                        & " Where " & oKeyField.Name & " = " & oKeyField.Value
                                    With cmdObj
                                        .Connection = oCon
                                        .CommandTimeout = 300
                                        .CommandText = strSQL
                                        .CommandType = CommandType.Text
                                        strValue = .ExecuteScalar
                                    End With
                                    If String.IsNullOrEmpty(strValue) Then
                                        'on any FK error we return false
                                        strErrorMessage = "Could not reference " & Me.HeaderName & " using key field " & oField.Parent_Field _
                                            & ".  The " & strKeyFieldMsg & " has not been imported because no records match for " _
                                            & oKeyField.Name & " with a value of " & oKeyField.Value & "."
                                        Return False
                                    Else
                                        'Save the data to the in memory collection
                                        oFKL.AddNew(oKeyField.Name, oKeyField.Value, strHeaderTable, oField.Parent_Field, strValue)
                                    End If
                                    Exit Do
                                Else
                                    If intRetryCt > Me.Retry Then
                                        'on any FK error we return false
                                        ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupFKValue: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Could not read to from " & Me.HeaderName & " table.<br />" & vbCrLf
                                        Log("lookupFKValue Failed!")
                                        strErrorMessage = "Could not reference " & Me.HeaderName & " using key field " & oField.Parent_Field & ".  The " & strKeyFieldMsg & " has not been imported because of a database connection failure."
                                        Return False
                                    Else
                                        Log("lookupFKValue Open DB Connection Failure Retry = " & intRetryCt.ToString)
                                    End If
                                End If
                            Catch ex As Exception
                                If intRetryCt > Me.Retry Then
                                    'on any FK error we return false
                                    ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupFKValue, attempted to read to from " & strHeaderTable & " table " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                                    Log("lookupFKValue Failed!" & readExceptionMessage(ex))
                                    strErrorMessage = "Could not reference " & Me.HeaderName & " using key field " & oField.Parent_Field & ".  The " & strKeyFieldMsg & " has not been imported because the database or table is not available."
                                    Return False
                                Else
                                    Log("lookupFKValue Failure Retry = " & intRetryCt.ToString)
                                End If
                            Finally
                                Try
                                    cmdObj.Cancel()
                                Catch ex As Exception

                                End Try
                                Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                                    If Not oCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                                        If oCon.State = ConnectionState.Open Then
                                            oCon.Close()
                                        End If
                                    End If
                                    oCon = Nothing
                                Catch ex As Exception

                                End Try
                            End Try
                            'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
                        Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
                    End If
                    'Store and Validate the FK value
                    Try
                        oField.Value = validateSQLValue(
                            strValue,
                            Val(oField.DataType),
                            strSource,
                            "Invalid " & oField.Name,
                            oField.Null,
                            oField.Length)
                    Catch ex As System.ApplicationException
                        'on any FK error we return false
                        strErrorMessage = "Could not reference " & strHeaderTable & " using key field " & oField.Parent_Field & ". " & ex.Message
                        Return False
                    Catch ex As Exception
                        Throw
                    End Try
                End If
            Next
            Return True
        Catch ex As Exception
            Throw
        End Try
        Return Ret
    End Function
    ''' <summary>
    ''' Note:  This function has been depreciated as of v-4.8 
    ''' all methods should now call lookupFKValues
    ''' </summary>
    ''' <param name="oField"></param>
    ''' <param name="oKeyField"></param>
    ''' <param name="strHeaderTable"></param>
    ''' <param name="strSource"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function lookupFKValue(
                    ByRef oField As clsImportField,
                    ByRef oKeyField As clsImportField,
                    ByVal strHeaderTable As String,
                    ByVal strSource As String) As Boolean
        Dim Ret As Boolean = False
        Dim strSQL As String = ""
        Dim strValues As String = ""
        Dim blnFirstField As Boolean = True
        Dim intCompNumber As Integer = 0
        Try
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim cmdObj As New SqlCommand
                Dim drTemp As SqlDataReader
                Dim oCon As System.Data.SqlClient.SqlConnection
                Try
                    oCon = getNewConnection(False)
                    If Not oCon Is Nothing AndAlso oCon.State = ConnectionState.Open Then


                        'Get the current highest company number available
                        strSQL = "Select Top 1 " & oField.Parent_Field & " as RetVal " _
                            & " From " & strHeaderTable _
                            & " Where " & oKeyField.Name & " = " & oKeyField.Value

                        With cmdObj
                            .Connection = oCon
                            .CommandTimeout = 300
                            .CommandText = strSQL
                            .CommandType = CommandType.Text
                            drTemp = .ExecuteReader()
                        End With
                        With drTemp
                            Try
                                If .HasRows Then
                                    .Read()
                                    oField.Value = validateSQLValue(
                                        .Item("RetVal"),
                                        Val(oField.DataType),
                                        strSource,
                                        "Invalid " & oField.Name,
                                        oField.Null,
                                        oField.Length)
                                    Return True
                                End If
                            Catch ex As Exception
                                Throw
                            Finally
                                .Close()
                            End Try
                        End With
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupFKValue: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Could not read to from " & Me.HeaderName & " table.<br />" & vbCrLf
                            Log("lookupFKValue Failed!")
                        Else
                            Log("lookupFKValue Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupFKValue, attempted to read to from " & strHeaderTable & " table " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                        Log("lookupFKValue Failed!")
                    Else
                        Log("lookupFKValue Failure Retry = " & intRetryCt.ToString)
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
                    Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                        If Not oCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                            If oCon.State = ConnectionState.Open Then
                                oCon.Close()
                            End If
                        End If
                        oCon = Nothing
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.lookupFKValue: Could not read to from " & strHeaderTable & " table.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("lookupFKValue Failed!")
        End Try
        Return Ret
    End Function

    Protected Function addToErrorTable(
            ByRef oFields As clsImportFields,
            ByVal strTableName As String,
            ByVal strErrorMessage As String,
            ByVal strFile As String,
            ByVal strName As String) As Boolean
        Dim Ret As Boolean = False
        Dim strSQL As String = ""

        Try
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim cmd As New SqlCommand
                Dim oCon As System.Data.SqlClient.SqlConnection
                Try
                    oCon = getNewConnection(False)
                    If Not oCon Is Nothing AndAlso oCon.State = ConnectionState.Open Then

                        strErrorMessage = "'" & padQuotes(strErrorMessage) & "'"
                        'build data record string
                        Dim strRecord As String = ""
                        Dim blnFirstField As Boolean = True
                        For intct As Integer = 1 To oFields.Count
                            If oFields(intct).Use Then
                                If blnFirstField Then
                                    blnFirstField = False
                                Else
                                    strRecord &= ","
                                End If
                                strRecord &= oFields(intct).Name & " = " & oFields(intct).Value
                            End If
                        Next

                        strRecord = "'" & padQuotes(strRecord) & "'"
                        'build execute string into error
                        strSQL = "INSERT INTO " & strTableName _
                            & " ([ImportRecord], [CreateUser], [ErrorDate], [ErrorMsg], [ImportFileName], [ImportFileType], [ImportName]) VALUES(" _
                            & strRecord & ", '" _
                            & mstrCreateUser & "', '" _
                            & mstrCreatedDate & "', '" _
                            & padQuotes(strErrorMessage) & "', '" _
                            & padQuotes(strFile) & "', " _
                            & ImportTypeKey & ", '" _
                            & padQuotes(strName) & "')"
                        cmd = New SqlCommand(strSQL, oCon)
                        cmd.ExecuteScalar()
                        ITEmailMsg &= "<br />" & Source & " Warning: clsDownload.addToErrorTable: data validation failure. " & strErrorMessage & " Please check the " & strTableName & " table for more information." & "<br />" & vbCrLf
                        Log("Data validation failure!")
                        Return True
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.addToErrorTable: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success." & "<br />" & vbCrLf
                            Log("addToErrorTable Failed!")
                        Else
                            Log("addToErrorTable Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.addToErrorTable, attempted to write to  " & strTableName & " table " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                        Log("addToErrorTable Failed!" & readExceptionMessage(ex))
                    Else
                        Log("addToErrorTable Failure Retry = " & intRetryCt.ToString)

                    End If
                Finally
                    Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                        If Not oCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                            If oCon.State = ConnectionState.Open Then
                                oCon.Close()
                            End If
                        End If
                        oCon = Nothing
                    Catch ex As Exception

                    End Try
                End Try

                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.addToErrorTable: Could not write to " & strTableName & " table.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("addToErrorTable Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret
    End Function

    Public Function getImportFieldFlag(ByVal strFieldName As String, ByVal enmImportType As IntegrationTypes) As Boolean

        Dim oCon As System.Data.SqlClient.SqlConnection
        Dim blnRet As Boolean = True
        'check if the flag is stored in memory
        Dim intCheckFlag = oIF.getImportFlag(strFieldName, enmImportType)
        If intCheckFlag = 0 Then
            Return False
        ElseIf intCheckFlag = 1 Then
            Return True
        End If
        'if we get here the value does not exist so get it from the database
        Try
            oCon = getNewConnection()
            Dim objCom As New SqlCommand
            With objCom
                .Connection = oCon
                .Parameters.Add("@ImportFieldName", SqlDbType.NVarChar, 100)
                .Parameters("@ImportFieldName").Value = strFieldName
                .Parameters.Add("@ImportFileType", SqlDbType.Int)
                .Parameters("@ImportFileType").Value = CInt(enmImportType)
                .Parameters.Add("@ImportFileName", SqlDbType.NVarChar, 50)
                .Parameters("@ImportFileName").Value = getImportFileName(enmImportType)
                .CommandText = "spGetImportFieldsFlag"
                .CommandType = CommandType.StoredProcedure
                blnRet = .ExecuteScalar()
                'Add the data to the list
                oIF.AddNew(strFieldName, enmImportType, blnRet, getImportFileName(enmImportType))
            End With

        Catch ex As System.ApplicationException
            Throw
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsDownload.getImportFieldFlag failure for field name " & strFieldName & " with table reference number " & CStr(enmImportType) & "<br />" & vbCrLf & "<br />" & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("getImportFieldFlag Failed!" & readExceptionMessage(ex))
        Finally
            Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                oCon.Close()
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try


        Return blnRet


    End Function

    Public Overridable Function AllowUpdate(ByVal fieldName As String, ByRef par As AllowUpdateParameters) As Boolean
        If par Is Nothing Then Return False

        fieldName = fieldName.ToUpper()
        Dim oImportFieldWCFData As DAL.NDPBaseClass = Nothing
        If par.FieldDictionary Is Nothing OrElse par.FieldDictionary.Count() < 1 Then
            'populate the dictionary
            oImportFieldWCFData = New DAL.NGLtblImportFieldData(par.WCFParameters)
            Try
                par.FieldDictionary = DirectCast(oImportFieldWCFData, DAL.NGLtblImportFieldData).getImportFieldFlagDictionary(par.ImportType, par.ImportFile)
            Catch ex As Exception
                AddToITEmailMsg("Error Reading Import Field Dictionary for " & par.ImportFile & " data.  All fields will be marked as updatable until the issue is corrected. Check the App Error Log for error details.")
            End Try
        End If
        'if there is a problem reading the dictionary (it may not exist if this this a new import type) 
        'we create a new one with all the fields marked as updateable.
        If par.FieldDictionary Is Nothing Then par.FieldDictionary = New Dictionary(Of String, Boolean)(StringComparer.OrdinalIgnoreCase)
        If Not par.FieldDictionary.Keys.Contains(fieldName) Then
            Try
                'this code is to keep the data library up-to-date automatically when fields are added to the web service
                If oImportFieldWCFData Is Nothing Then oImportFieldWCFData = New DAL.NGLtblImportFieldData(par.WCFParameters)
                'CreateRecord throws InvalidKeyAlreadyExistsException if the field name already exists and we do not need to log this
                DirectCast(oImportFieldWCFData, DAL.NGLtblImportFieldData).CreateRecord(New DTO.tblImportField With {.ImportFieldName = fieldName, .ImportFileName = par.ImportFile, .ImportFileType = par.ImportType, .ImportFlag = True, .TrackingState = Ngl.Core.ChangeTracker.TrackingInfo.Created})
            Catch ex As FaultException(Of DAL.SqlFaultInfo)
                If Not ex.Detail.Details = "E_CannotSaveKeyAlreadyExists" Then
                    'we only log fault exceptions when details is not E_CannotSaveKeyAlreadyExists
                    AddToITEmailMsg("Error adding new field name, " & fieldName & ", to Import Field Data Table for " & par.ImportFile & " data.  Some fields may be marked as updatable until the issue is corrected. Check the App Error Log for error details.")
                End If
            Catch ex As Exception
                AddToITEmailMsg("Error adding new field name, " & fieldName & ", to Import Field Data Table for " & par.ImportFile & " data.  All fields will be marked as updatable until the issue is corrected. Check the App Error Log for error details.")
            Finally
                par.FieldDictionary.Add(fieldName, True)
            End Try

        End If
        If par.insertFlag = True Or (par.insertFlag = False And par.FieldDictionary(fieldName) = True) Then
            Return True
        End If
        Return False
    End Function

    Public Function useOptimizer(ByVal enumOptType As OptimizerTypes) As Boolean
        On Error Resume Next
        useOptimizer = False
        If enumOptType = OptimizerTypes.optTruckStops Then
            useOptimizer = getParValue("UseTruckStops")
        ElseIf enumOptType = OptimizerTypes.optPCMiler Then
            useOptimizer = getParValue("UsePCMiler")
        ElseIf enumOptType = OptimizerTypes.optany Then
            useOptimizer = getParValue("UseTruckStops") + getParValue("UsePCMiler")
        Else
            useOptimizer = False
        End If
    End Function

    Protected Function validateCompany(ByRef oField As clsImportField,
                                       ByRef strErrorMessage As String,
                                       ByRef oCX As CompXrefs,
                                       ByVal strSource As String) As Boolean
        Dim intCompNumber As Integer = 0
        Dim strAlphaNumber As String = stripQuotes(oField.Value)
        'Validate the company number
        If Not Integer.TryParse(strAlphaNumber, intCompNumber) Then
            'The company number is not a number so lookup the value in the xref table
            'First check if we have a copy in memory
            'store the alpha number

            intCompNumber = oCX.getNumberByAlpha(strAlphaNumber)
            If intCompNumber = 0 Then
                'Check the alpha Xref table for a Company Number
                If Not lookupCompNumberByAlphaCode(oField) Then
                    strErrorMessage = "Could not validate Company Number in Alpha Xref Table  " & oField.Value
                    Return False
                Else
                    'Revalidate the company number and save it to memory for future use
                    If Not Integer.TryParse(oField.Value, intCompNumber) Then
                        strErrorMessage = "The Alpha Company Xref Table does not contain a valid cross reference for the company " & oField.Value
                        Return False
                    Else
                        'This is a good company number so save it for future use
                        oCX.AddNew(strAlphaNumber, intCompNumber)
                    End If
                End If
            End If
        End If


        'Now change the field settings for the company to integer
        oField.DataType = clsImportField.DataTypeID.gcvdtLongInt
        oField.Length = 11
        oField.Null = False
        'now re-validate the data type
        Try
            oField.Value = validateSQLValue(
                    intCompNumber _
                    , CInt(oField.DataType) _
                    , strSource _
                    , "Invalid " & oField.Name _
                    , oField.Null _
                    , oField.Length)
            Return True
        Catch ex As System.ApplicationException
            strErrorMessage = ex.Message
            Return False
        Catch ex As Exception
            Throw
        End Try

    End Function


    ''' <summary>
    ''' this mothod will try to use the compalphacode if the oField custnumber 
    ''' It will still use the alpha xref as a last resort for backward compatibility
    ''' </summary>
    ''' <param name="oField"></param>
    ''' <param name="strErrorMessage"></param>
    ''' <param name="strSource"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="CompAlphaCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function validateCompany70(ByRef oField As clsImportField,
                                       ByRef CompControl As Integer,
                                       ByRef strErrorMessage As String,
                                       ByRef oCX As CompXrefs,
                                       ByVal strSource As String,
                                       ByVal CompLegalEntity As String,
                                       ByVal CompAlphaCode As String) As Boolean
        Dim intCompNumber As Integer = 0
        Dim strAlphaNumber As String = stripQuotes(oField.Value)
        Dim ocompxref = oCX.GetCompNumberOnImport(strAlphaNumber, CompLegalEntity, CompAlphaCode, DALParameters, strErrorMessage)
        If ocompxref Is Nothing OrElse ocompxref.CompControl = 0 Then
            'Modified by RHR for v-7.0.6.104 on 8/12/2017
            'new code to manage error messages for company validation
            strErrorMessage = String.Format("{0} Cannot find company information using Alpha: {1}, Comp Control: {2}, Legal Entity: {3}, CompAlphaCode: {4}", strErrorMessage, strAlphaNumber, CompControl, CompLegalEntity, CompAlphaCode)
            Return False
        End If
        intCompNumber = ocompxref.CompNumber
        CompControl = ocompxref.CompControl
        'Now change the field settings for the company to integer
        oField.DataType = clsImportField.DataTypeID.gcvdtLongInt
        oField.Length = 11
        oField.Null = False
        'now re-validate the data type
        Try
            oField.Value = validateSQLValue(
                    intCompNumber _
                    , CInt(oField.DataType) _
                    , strSource _
                    , "Invalid " & oField.Name _
                    , oField.Null _
                    , oField.Length)
            Return True
        Catch ex As System.ApplicationException
            strErrorMessage = ex.Message
            Return False
        Catch ex As Exception
            Throw
        End Try

    End Function

    Protected Function validateFields(ByRef oFields As clsImportFields,
                                      ByVal oRow As DataRow,
                                      ByRef strErrorMessage As String,
                                      ByVal strSource As String) As Boolean

        For intct As Integer = 1 To oFields.Count
            'Because we use all fields on insert and we only
            'apply the Use flag on updates we ignore this flag
            'when reading data 
            '(we dont know if this is an insert or an update yet)
            'If oFields(intct).Use Then
            Try
                If oFields(intct).PK <> gcIgnore And oFields(intct).PK <> gcFK Then
                    oFields(intct).Value = validateSQLValue(
                        oRow.Item(oFields(intct).Key) _
                        , CInt(oFields(intct).DataType) _
                        , strSource _
                        , "Invalid " & oFields(intct).Key _
                        , oFields(intct).Null _
                        , oFields(intct).Length)
                End If
            Catch ex As System.ArgumentException
                'there is a problem with the oFields Collection
                strErrorMessage &= "There is a problem with the data fields collection the actual error is: " & ex.Message
                Return False
            Catch ex As System.ApplicationException
                strErrorMessage &= ex.Message
                Return False
            Catch ex As Exception
                Throw
            End Try
            'End If
        Next
        'If no error we return true
        Return True
    End Function

    Protected Function validateFields(ByRef oFields As clsImportFields,
                                      ByVal oRow As clsImportDataBase,
                                      ByRef strErrorMessage As String,
                                      ByVal strSource As String) As Boolean

        For intct As Integer = 1 To oFields.Count
            'Because we use all fields on insert and we only
            'apply the Use flag on updates we ignore this flag
            'when reading data 
            '(we dont know if this is an insert or an update yet)
            'If oFields(intct).Use Then
            Try
                If oFields(intct).PK <> gcIgnore And oFields(intct).PK <> gcFK Then
                    oFields(intct).Value = validateSQLValue(
                    oRow.Item(oFields(intct).Key) _
                        , CInt(oFields(intct).DataType) _
                        , strSource _
                        , "Invalid " & oFields(intct).Key _
                        , oFields(intct).Null _
                        , oFields(intct).Length)
                End If
            Catch ex As System.ArgumentException
                'there is a problem with the oFields Collection
                strErrorMessage &= "There is a problem with the data fields collection the actual error is: " & ex.Message
                Return False
            Catch ex As System.ApplicationException
                strErrorMessage &= ex.Message
                Return False
            Catch ex As Exception
                Throw
            End Try
            'End If
        Next
        'If no error we return true
        Return True
    End Function

    ''' <summary>
    ''' Build a key value string using the first non-empty value in the dictionary
    ''' </summary>
    ''' <param name="dicFields"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function buildDataKeyString(ByVal dicFields As Dictionary(Of String, String)) As String
        Dim strRet As String = ""
        For Each d In dicFields
            If Not String.IsNullOrEmpty(d.Value) AndAlso d.Value.Trim().Length > 0 Then
                strRet = d.Key & " = " & d.Value
                Exit For
            End If
        Next
        If String.IsNullOrEmpty(strRet) OrElse strRet.Trim().Length < 1 Then strRet = "invalid record reference"
        Return strRet
    End Function


End Class

Public Class AllowUpdateParameters

    Private _insertFlag As Boolean = True
    Public Property insertFlag() As Boolean
        Get
            Return _insertFlag
        End Get
        Set(ByVal value As Boolean)
            _insertFlag = value
        End Set
    End Property

    Private _FieldDictionary As Dictionary(Of String, Boolean)
    Public Property FieldDictionary() As Dictionary(Of String, Boolean)
        Get
            Return _FieldDictionary
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            _FieldDictionary = value
        End Set
    End Property

    Private _ImportType As IntegrationTypes
    Public Property ImportType() As IntegrationTypes
        Get
            Return _ImportType
        End Get
        Set(ByVal value As IntegrationTypes)
            If _ImportType <> value Then
                _ImportType = value
                _ImportFile = getImportFileName(_ImportType)
            End If
        End Set
    End Property

    Private _WCFParameters As DAL.WCFParameters
    Public Property WCFParameters() As DAL.WCFParameters
        Get
            Return _WCFParameters
        End Get
        Set(ByVal value As DAL.WCFParameters)
            _WCFParameters = value
        End Set
    End Property

    Private _ImportFile As String
    Public ReadOnly Property ImportFile() As String
        Get
            If String.IsNullOrEmpty(_ImportFile) OrElse _ImportFile.Trim().Length < 1 Then
                _ImportFile = getImportFileName(ImportType)
            End If
            Return _ImportFile
        End Get
    End Property



End Class
