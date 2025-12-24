Imports Ngl.FreightMaster.Integration.Configuration
Imports System.Data.SqlClient
Imports Ngl.FreightMaster.Integration.FMDataTableAdapters

<Serializable()> _
Public Class clsEDIISABLL : Inherits clsUpload

#Region " Class Variables and Properties "

    Private _Adapter As CarrierEDIISATableAdapter
    Protected ReadOnly Property Adapter() As CarrierEDIISATableAdapter
        Get
            If _Adapter Is Nothing Then
                _Adapter = New CarrierEDIISATableAdapter
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
    Protected Function GetDataByCarrierOutboundFlag(ByVal CarrierControl As Integer, ByVal OutboundFlag As Boolean) As FMData.CarrierEDIISADataTable
        Adapter.SetCommandTimeOut(Me.CommandTimeOut)
        Return (Adapter.GetDataByCarrierOutboundFlag(CarrierControl, OutboundFlag))
    End Function

    Public Function getEDIISAString(ByVal e As clsEDIISA) As String
        Return "ISA*" & e.ISA01 & "*" & e.ISA02 & "*" & e.ISA03 & "*" & e.ISA04 & "*" & e.ISA05 & "*" & e.ISA06 _
            & "*" & e.ISA07 & "*" & e.ISA08 & "*" & e.ISA09 & "*" & e.ISA10 & "*" & e.ISA11 & "*" & e.ISA12 & "*" & e.ISA13 _
            & "*" & e.ISA14 & "*" & e.ISA15 & "*" & e.ISA16 & e.SegmentTerminator

    End Function

    Public Function getEDIIEAString(ByVal e As clsEDIIEA, ByVal sSegTerm As String) As String
        Return "IEA*" & e.IEA01 & "*" & e.IEA02 & sSegTerm
    End Function

    ''' <summary>
    ''' This function returns an out bound ISA segment object for the carrier
    ''' </summary>
    ''' <param name="oEDIISA">out bound object parameter reference</param>
    ''' <param name="strConnection">database connection string</param>
    ''' <param name="CarrierNumber">FreightMaster Carrier Number</param>
    ''' <param name="strDate">min/max 8/8 Date yyyyMMdd (20091006) for October 06 2009</param>
    ''' <param name="strTime">min/max 4/4 Time - HHmm (1422) for 02:22 pm</param>
    ''' <param name="intGroupNumber">min/max 9/9 Control Number</param>
    ''' <returns>
    ''' 0 -- nglDataIntegrationComplete
    ''' 1 -- nglDataConnectionFailure
    ''' 2 -- nglDataValidationFailure
    ''' 3 -- nglDataIntegrationFailure
    ''' 4 -- nglDataIntegrationHadErrors
    ''' </returns>
    ''' <remarks></remarks>
    Public Function readEDIISAObject(ByRef oEDIISA As clsEDIISA, _
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
        Dim strSource As String = "clsEDIISA.readEDIISAObject"
        GroupEmailMsg = ""
        ITEmailMsg = ""
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "EDI ISA Data Integration"
        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If

        Try
            RecordErrors = 0
            TotalRecords = 0
            'check for empty strinISA being passed as parameters
            If intCarrierControl < 1 AndAlso String.IsNullOrEmpty(CarrierNumber) OrElse CarrierNumber.Trim.Length < 1 Then
                LogError("Read EDI ISA Object Data Failure", "The Carrier Number is required and may not be blank or empty.  Cannot read EDI ISA data.", AdminEmail)
                Return ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            'Check if the Carrier Number exists

            If intCarrierControl < 1 AndAlso Not Integer.TryParse(getScalarValue("Select top 1 CarrierControl from dbo.Carrier Where CarrierNumber = " & CarrierNumber, Source, True), intCarrierControl) Then
                LogError("Read EDI ISA Object Data Failure", "The Carrier Number, " & CarrierNumber & " is not valid.  The Carrier Number is required and must match an existing carrier record in the database.  Cannot read EDI ISA data.", AdminEmail)
                Return ProcessDataReturnValues.nglDataIntegrationFailure
            End If

            'get the data table records
            Dim oTable As FMData.CarrierEDIISADataTable = GetDataByCarrierOutboundFlag(intCarrierControl, True)
            If oTable.Rows.Count < 1 Then
                LastError = "No ISA records found for carrier " & CarrierNumber
                Return ProcessDataReturnValues.nglDataIntegrationFailure 'no data
            End If
            Dim oRow As FMData.CarrierEDIISARow = oTable.Rows(0)
            With oEDIISA
                .ISA01 = oRow.CarrierEDIISA01
                .ISA02 = oRow.CarrierEDIISA02
                .ISA03 = oRow.CarrierEDIISA03
                .ISA04 = oRow.CarrierEDIISA04
                .ISA05 = oRow.CarrierEDIISA05
                .ISA06 = oRow.CarrierEDIISA06
                .ISA07 = oRow.CarrierEDIISA07
                .ISA08 = oRow.CarrierEDIISA08
                .ISA09 = strDate
                .ISA10 = strTime
                .ISA11 = oRow.CarrierEDIISA11
                .ISA12 = oRow.CarrierEDIISA12
                If intGroupNumber < 1 Then
                    'lookup the control number
                    intGroupNumber = getNextSequence(.CarrierEDIISAControl)
                End If
                .ISA13 = intGroupNumber.ToString
                .ISA14 = oRow.CarrierEDIISA14
                .ISA15 = oRow.CarrierEDIISA15
                .ISA16 = oRow.CarrierEDIISA16
                .SegmentTerminator = oRow.CarrierEDIISASegTerm
            End With

            Return ProcessDataReturnValues.nglDataIntegrationComplete

        Catch ex As Exception
            LogException("Read EDI ISA Object Failure", "Could not read ISA records for carrier " & CarrierNumber & ".", AdminEmail, ex, "NGL.FreightMaster.Integration.clsEDIISABLL.readEDIISAObject Failure")
        Finally
            Try
               closeConnection
            Catch ex As Exception

            End Try
        End Try

        Return enmRet
    End Function
    

    Public Function getNextSequence(ByVal CarrierEDIISAControl As Integer) As Integer
        Dim intSequence As Integer = 1
        Dim strSequence = getScalarValue("Exec dbo.spGetEDIISASequence " & CarrierEDIISAControl)
        If Not Integer.TryParse(strSequence, intSequence) Then
            Throw New ApplicationException("Cannot get next EDI ISA Sequence Number for CarrierEDIISAControl Number " & CarrierEDIISAControl.ToString)
        End If
        Return intSequence
    End Function

#End Region

End Class
