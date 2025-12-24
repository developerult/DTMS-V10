Imports NGL.FreightMaster.Core.UserConfiguration
Imports Ngl.FreightMaster.Core.PublicEnums
Imports NGL.FreightMaster.Data.ApplicationDataTableAdapters
Imports Ngl.Core.Utility.DataTransformation


Namespace Model

    <Serializable()> _
    <System.ComponentModel.DataObject()> _
    Public Class FormSecurityValues

#Region "Class Variables and Properties"

        Private _ovFormSecurityValuesTable As ApplicationData.vFormSecurityValuesDataTable = Nothing
        Public ReadOnly Property ovFormSecurityValuesTable() As ApplicationData.vFormSecurityValuesDataTable
            Get
                If _ovFormSecurityValuesTable Is Nothing Then
                    _ovFormSecurityValuesTable = Me.GetDataByCurrentUser
                End If
                Return _ovFormSecurityValuesTable
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

        Private _strName As String = "vFormSecurityValues"
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


        Private _Adapter As vFormSecurityValuesTableAdapter
        Protected ReadOnly Property Adapter() As vFormSecurityValuesTableAdapter
            Get
                If _Adapter Is Nothing Then
                    _Adapter = New vFormSecurityValuesTableAdapter
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

        Public Function checkFormSecurity(ByVal intIndex As Integer, Optional ByVal strName As String = "") As enmSecurityAccess
            Dim enmRet As enmSecurityAccess = enmSecurityAccess.AccessDenied
            Try
                Dim oRow As ApplicationData.vFormSecurityValuesRow = Me.ovFormSecurityValuesTable.Rows.Find(intIndex)
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

        Public Function checkFormSecurity(ByVal strName As String) As enmSecurityAccess
            Dim enmRet As enmSecurityAccess = enmSecurityAccess.AccessDenied
            Try
                Dim oRows() As System.Data.DataRow = Me.ovFormSecurityValuesTable.Select(" FormName = " & strName)
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
                Public Function GetData() As ApplicationData.vFormSecurityValuesDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetData())

        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetDataByCurrentUser() As ApplicationData.vFormSecurityValuesDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetDataByCurrentUser())
        End Function

        <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetDataByCurrentUserandFormName(ByVal strName As String) As ApplicationData.vFormSecurityValuesDataTable
            Adapter.SetCommandTimeOut(Me.CommandTimeOut)
            Return (Adapter.GetDataByCurrentUserandFormName(strName))
        End Function


#End Region

    End Class

End Namespace
