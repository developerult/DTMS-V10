Imports Ngl.FreightMaster.Integration.Configuration
Imports System.Data.SqlClient
Imports Ngl.FreightMaster.Integration.FMDataTableAdapters

<Serializable()> _
Public Class clsEDIGSBLL : Inherits clsUpload

#Region " Class Variables and Properties "

    Private _Adapter As CarrierEDIGSTableAdapter
    Protected ReadOnly Property Adapter() As CarrierEDIGSTableAdapter
        Get
            If _Adapter Is Nothing Then
                _Adapter = New CarrierEDIGSTableAdapter
                _Adapter.SetConnectionString(Me.DBConnection)
            End If

            Return _Adapter
        End Get
    End Property
#End Region


#Region " Constructors "



#End Region

#Region " Functions "

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Protected Function GetDataByCarrierOutboundFlag(ByVal CarrierControl As Integer, ByVal OutboundFlag As Boolean) As FMData.CarrierEDIGSDataTable
        Adapter.SetCommandTimeOut(Me.CommandTimeOut)
        Return (Adapter.GetDataByCarrierOutboundFlag(CarrierControl, OutboundFlag))
    End Function

    Public Function getEDIGSString(ByVal e As clsEDIGS, ByVal sSegTerm As String) As String
        Return "GS*" & e.GS01 & "*" & e.GS02 & "*" & e.GS03 & "*" & e.GS04 & "*" & e.GS05 & "*" & e.GS06 & "*" & e.GS07 & "*" & e.GS08 & sSegTerm
    End Function

    Public Function getEDIGEString(ByVal e As clsEDIGE, ByVal sSegTerm As String) As String
        Return "GE*" & e.GE01 & "*" & e.GE02 & sSegTerm
    End Function

    ''' <summary>
    ''' This function returns an out bound GS segment object for the carrier
    ''' </summary>
    ''' <param name="oEDIGS">out bound object parameter reference</param>
    ''' <param name="strConnection">database connection string</param>
    ''' <param name="CarrierNumber">FreightMaster Carrier Number</param>
    ''' <param name="strDate">min/max 8/8 Date yyyyMMdd (20091006) for October 06 2009</param>
    ''' <param name="strTime">min/max 4/8 Time - HHmm (1422) for 02:22 pm</param>
    ''' <param name="intGroupNumber">min/max 1/9 Group Control Number</param>
    ''' <returns>
    ''' 0 -- nglDataIntegrationComplete
    ''' 1 -- nglDataConnectionFailure
    ''' 2 -- nglDataValidationFailure
    ''' 3 -- nglDataIntegrationFailure
    ''' 4 -- nglDataIntegrationHadErrors
    ''' </returns>
    ''' <remarks></remarks>
    Public Function readEDIGSObject(ByRef oEDIGS As clsEDIGS, _
                                            ByVal strConnection As String, _
                                            ByVal CarrierNumber As String, _
                                            ByVal strDate As String, _
                                            ByVal strTime As String, _
                                            Optional ByVal intGroupNumber As Integer = 0, _
                                            Optional ByVal intCarrierControl As Integer = 0) As ProcessDataReturnValues
        Dim enmRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strTitle As String = ""
        Dim intErrors As Integer = 0
        Dim intCount As Integer = 0
        Dim strSource As String = "clsEDIGS.readEDIGSObject"
        GroupEmailMsg = ""
        ITEmailMsg = ""
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "EDI GS Data Integration"
        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If
        Try
            RecordErrors = 0
            TotalRecords = 0
            'check for empty strings being passed as parameters
            If intCarrierControl < 1 AndAlso String.IsNullOrEmpty(CarrierNumber) OrElse CarrierNumber.Trim.Length < 1 Then
                LogError("Read EDI GS Object Data Failure", "The Carrier Number is required and may not be blank or empty.  Cannot read EDI GS data.", AdminEmail)
                Return ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            'Check if the Carrier Number exists
            If intCarrierControl < 1 AndAlso Not Integer.TryParse(getScalarValue("Select top 1 CarrierControl from dbo.Carrier Where CarrierNumber = " & CarrierNumber, Source, True), intCarrierControl) Then
                LogError("Read EDI GS Object Data Failure", "The Carrier Number, " & CarrierNumber & " is not valid.  The Carrier Number is required and must match an existing carrier record in the database.  Cannot read EDI GS data.", AdminEmail)
                Return ProcessDataReturnValues.nglDataIntegrationFailure
            End If

            'get the data table records
            Dim oTable As FMData.CarrierEDIGSDataTable = GetDataByCarrierOutboundFlag(intCarrierControl, True)
            If oTable.Rows.Count < 1 Then
                LastError = "No GS records found for carrier " & CarrierNumber
                Return ProcessDataReturnValues.nglDataIntegrationFailure 'no data
            End If
            Dim oRow As FMData.CarrierEDIGSRow = oTable.Rows(0)
            With oEDIGS
                .GS01 = oRow.CarrierEDIGS01
                .GS02 = oRow.CarrierEDIGS02
                .GS03 = oRow.CarrierEDIGS03
                .GS04 = strDate
                .GS05 = strTime
                If intGroupNumber < 1 Then
                    'lookup the control number
                    intGroupNumber = getNextSequence(.CarrierEDIGSControl)
                End If
                .GS06 = intGroupNumber.ToString
                .GS07 = oRow.CarrierEDIGS07
                .GS08 = oRow.CarrierEDIGS08
            End With

            Return ProcessDataReturnValues.nglDataIntegrationComplete

        Catch ex As Exception
            LogException("Read EDI GS Object Failure", "Could not read GS records for carrier " & CarrierNumber & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsEDIGSBLL.readEDIGSObject Failure")
        Finally
            Try
                closeConnection
            Catch ex As Exception

            End Try
        End Try

        Return enmRet
    End Function

    Private Function getNextSequence(ByVal CarrierEDIGSControl As Integer) As Integer
        Dim intSequence As Integer = 1
        Dim strSequence = getScalarValue("Exec dbo.spGetEDIGSSequence " & CarrierEDIGSControl)
        If Not Integer.TryParse(strSequence, intSequence) Then
            Throw New ApplicationException("Cannot get next EDI GS Sequence Number for CarrierEDIGSControl Number " & CarrierEDIGSControl.ToString)
        End If
        Return intSequence

    End Function

#End Region

End Class
