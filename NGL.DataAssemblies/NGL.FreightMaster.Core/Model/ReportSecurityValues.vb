Imports NGL.FreightMaster.Core.UserConfiguration
Imports Ngl.FreightMaster.Core.PublicEnums
Imports NGL.FreightMaster.Data.ApplicationDataTableAdapters
Imports Ngl.Core.Utility.DataTransformation

Namespace Model

    <Serializable()> _
    <System.ComponentModel.DataObject()> _
    Public Class ReportSecurityValues

#Region "Class Variables and Properties"

        Private _ovReportSecurityValuesTable As ApplicationData.vReportSecurityValuesDataTable = Nothing
        Public ReadOnly Property ovReportSecurityValuesTable() As ApplicationData.vReportSecurityValuesDataTable
            Get
                If _ovReportSecurityValuesTable Is Nothing Then
                    _ovReportSecurityValuesTable = Me.GetDataByCurrentUser
                End If
                Return _ovReportSecurityValuesTable
            End Get
        End Property


        Private _oUserConfiguration As UserConfiguration = Nothing
        Public Property oUserConfiguration() As UserConfiguration
            Get
                If _oUserConfiguration Is Nothing Then
                    _oUserConfiguration = New UserConfiguration
                End If
                Return _oUserConfiguration
            End Get
            Set(ByVal value As UserConfiguration)
                _oUserConfiguration = value
            End Set
        End Property

        Private _strUnmatched As String = ""
        Public ReadOnly Property Unmatched() As String
            Get
                Return _strUnmatched

            End Get
        End Property

        Private _intRowsAffected As Integer = 0
        Public ReadOnly Property RowsAffected() As Integer
            Get
                Return _intRowsAffected
            End Get
        End Property

        Private _strName As String = "vReportSecurityValues"
        Private _strKey As String = ""
        Private _intTimeOut As Integer = 0

        Protected Property CommandTimeOut() As Integer
            Get
                If _intTimeOut < 100 Then
                    _intTimeOut = Me.oUserConfiguration.ShortTimeOut
                End If
                Return _intTimeOut
            End Get
            Set(ByVal value As Integer)
                _intTimeOut = value
            End Set
        End Property

        Protected _Debug As Boolean = False
        Public Property Debug() As Boolean

            Get
                Return _Debug
            End Get
            Set(ByVal value As Boolean)
                _Debug = value
            End Set
        End Property

        Protected _LastError As String = ""
        Public ReadOnly Property LastError() As String
            Get
                Return _LastError
            End Get
        End Property


        Private _Adapter As vReportSecurityValuesTableAdapter
        Protected ReadOnly Property Adapter() As vReportSecurityValuesTableAdapter
            Get
                If _Adapter Is Nothing Then
                    _Adapter = New vReportSecurityValuesTableAdapter
                    If Not oUserConfiguration.ConnectionString Is Nothing Then
                        _Adapter.SetConnectionString(oUserConfiguration.ConnectionString)
                    End If
                End If

                Return _Adapter
            End Get
        End Property

#End Region

#Region "Constructors"



#End Region

#Region "Functions"

        Public Function checkReportSecurity(ByVal intIndex As Integer, Optional ByVal strName As String = "") As enmSecurityAccess
            Dim enmRet As enmSecurityAccess = enmSecurityAccess.AccessDenied
            Try
                Dim oRow As ApplicationData.vReportSecurityValuesRow = Me.ovReportSecurityValuesTable.Rows.Find(intIndex)
                If Not oRow Is Nothing Then
                    enmRet = enmSecurityAccess.AccessDenied
                Else
                    enmRet = enmSecurityAccess.FullAccess
                End If
            Catch ex As System.ApplicationException
                Throw
            Catch ex As Exception
                _LastError = ex.Message
                Throw New System.ApplicationException(_LastError, ex.InnerException)
            End Try
            Return enmRet

        End Function

        Public Function checkReportSecurity(ByVal strName As String) As enmSecurityAccess
            Dim enmRet As enmSecurityAccess = enmSecurityAccess.AccessDenied
            Try
                Dim oRows() As System.Data.DataRow = Me.ovReportSecurityValuesTable.Select(" ReportName = " & strName)
                If oRows.Length > 0 Then
                    enmRet = enmSecurityAccess.AccessDenied
                Else
                    enmRet = enmSecurityAccess.FullAccess
                End If
            Catch ex As System.ApplicationException
                Throw
            Catch ex As Exception
                _LastError = ex.Message
                Throw New System.ApplicationException(_LastError, ex.InnerException)
            End Try
            Return enmRet

        End Function


        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
                Public Function GetData() As ApplicationData.vReportSecurityValuesDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetData())

        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetDataByCurrentUser() As ApplicationData.vReportSecurityValuesDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetDataByCurrentUser())
        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetDataByCurrentUserandReportName(ByVal strName As String) As ApplicationData.vReportSecurityValuesDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetDataByCurrentUserandReportName(strName))
        End Function


#End Region

    End Class

End Namespace
