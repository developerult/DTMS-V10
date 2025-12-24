Imports System.Data.SqlClient
Imports Ngl.FreightMaster.Integration.Configuration
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DAL = Ngl.FreightMaster.Data

Public Class CompXrefs : Inherits List(Of CompXref)


    Public Sub AddNew(ByVal strAlphanumber As String, ByVal intNumber As Integer, Optional ByVal intControl As Integer = 0)

        Dim oC As CompXref
        Dim blnAddNew As Boolean = False

#Disable Warning BC42030 ' Variable 'oC' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
        If Not fillCompByAlpha(strAlphanumber, oC) Then
#Enable Warning BC42030 ' Variable 'oC' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            If Not fillCompByNumber(intNumber, oC) Then
                If Not fillCompByControl(intControl, oC) Then
                    blnAddNew = True
                End If
            End If
        End If
        If blnAddNew OrElse oC Is Nothing Then
            oC = New CompXref
            If Not String.IsNullOrEmpty(strAlphanumber) Then oC.CompAlphaNumber = strAlphanumber
            oC.CompNumber = intNumber
            oC.CompControl = intControl
            Me.Add(oC)
        Else
            'only update values if they are not empty or zero
            If Not String.IsNullOrEmpty(strAlphanumber) Then oC.CompAlphaNumber = strAlphanumber
            If intNumber > 0 Then oC.CompNumber = intNumber
            If intControl > 0 Then oC.CompControl = intControl
        End If
    End Sub


    Private Function fillCompByAlpha(ByVal strAlphanumber As String, ByRef oComp As CompXref) As Boolean
        Dim blnRet As Boolean = False
        If Not String.IsNullOrEmpty(strAlphanumber) Then
            Dim query = From p In Me Where p.CompAlphaNumber = strAlphanumber
            If query.Count > 0 Then
                oComp = TryCast(query(0), CompXref)
                If Not oComp Is Nothing Then blnRet = True
            End If
        End If
        Return blnRet
    End Function

    Private Function fillCompByNumber(ByVal intCompNumber As Integer, ByRef oComp As CompXref) As Boolean
        Dim blnRet As Boolean = False
        If intCompNumber > 0 Then
            Dim query = From p In Me Where p.CompNumber = intCompNumber
            If query.Count > 0 Then
                oComp = TryCast(query(0), CompXref)
                If Not oComp Is Nothing Then blnRet = True
            End If
        End If
        Return blnRet
    End Function

    Private Function fillCompByControl(ByVal intCompControl As Integer, ByRef oComp As CompXref) As Boolean
        Dim blnRet As Boolean = False
        If intCompControl > 0 Then
            Dim query = From p In Me Where p.CompControl = intCompControl
            If query.Count > 0 Then
                oComp = TryCast(query(0), CompXref)
                If Not oComp Is Nothing Then blnRet = True
            End If
        End If
        Return blnRet
    End Function



    Public Function getControlByNumber(ByVal intNumber As Integer) As Integer
        Dim intRet As Integer = 0
        Dim query = From p In Me Where p.CompNumber = intNumber
        If query.Count > 0 Then
            Dim c As CompXref = query(0)
            intRet = c.CompControl
        End If
        Return intRet
    End Function

    Public Function getAlphaByNumber(ByVal intNumber As Integer) As String
        Dim strRet As String = ""
        Dim query = From p In Me Where p.CompNumber = intNumber
        If query.Count > 0 Then
            Dim c As CompXref = query(0)
            strRet = c.CompAlphaNumber
        End If
        Return strRet
    End Function

    Public Function getNumberByControl(ByVal intControl As Integer) As Integer
        Dim intRet As Integer = 0
        Dim query = From p In Me Where p.CompControl = intControl
        If query.Count > 0 Then
            Dim c As CompXref = query(0)
            intRet = c.CompNumber
        End If
        Return intRet
    End Function

    Public Function getAlphaByControl(ByVal intControl As Integer) As String
        Dim strRet As String = ""
        Dim query = From p In Me Where p.CompControl = intControl
        If query.Count > 0 Then
            Dim c As CompXref = query(0)
            strRet = c.CompAlphaNumber
        End If
        Return strRet
    End Function

    Public Function getControlByAlpha(ByVal strAlpha As String) As Integer
        Dim intRet As Integer = 0
        Dim query = From p In Me Where p.CompAlphaNumber = strAlpha
        If query.Count > 0 Then
            Dim c As CompXref = query(0)
            intRet = c.CompControl
        End If
        Return intRet
    End Function


    Public Function getNumberByAlpha(ByVal strAlpha As String) As Integer
        Dim intRet As Integer = 0
        Dim query = From p In Me Where p.CompAlphaNumber = strAlpha
        If query.Count > 0 Then
            Dim c As CompXref = query(0)
            intRet = c.CompNumber
        End If
        Return intRet
    End Function


    Public Function GetCompNumberOnImport(ByVal CustNumber As String, ByVal LegalEntity As String, ByVal AlphaCode As String, ByVal oWCFParameters As DAL.WCFParameters, ByRef strErrMsg As String) As CompXref
        Dim oRet As New CompXref
        Try
            'see if the data exists
            oRet = (From d In Me Where d.CompAlphaNumber = CustNumber And d.CompLegalEntity = LegalEntity And d.CompAlphaCode = AlphaCode).FirstOrDefault()
            If oRet Is Nothing OrElse oRet.CompControl = 0 Then
                oRet = New CompXref 'we need to create a new instance of the data object
                Dim oCompData As New DAL.NGLCompData(oWCFParameters)
                Dim oCompInfo = oCompData.GetCompNumberOnImport(CustNumber, LegalEntity, AlphaCode)
                If Not oCompInfo Is Nothing AndAlso oCompInfo.CompControl <> 0 Then
                    With oRet
                        .CompAlphaCode = AlphaCode
                        .CompAlphaNumber = CustNumber
                        .CompLegalEntity = LegalEntity
                        .CompNumber = oCompInfo.CompNumber
                        .CompControl = oCompInfo.CompControl
                    End With
                    Me.Add(oRet)
                End If
            End If
        Catch ex As Exception
            strErrMsg = "Cannot get company information: " & ex.Message
        Finally

        End Try
        Return oRet
    End Function


End Class



Public Class CompXref

    Private _CompLegalEntity As String = ""
    Public Property CompLegalEntity As String
        Get
            Return _CompLegalEntity
        End Get
        Set(value As String)
            _CompLegalEntity = value
        End Set
    End Property

    Private _CompAlphaCode As String = ""
    Public Property CompAlphaCode() As String
        Get
            Return _CompAlphaCode
        End Get
        Set(ByVal value As String)
            _CompAlphaCode = value
        End Set
    End Property

    Private _CompAlphaNumber As String
    Public Property CompAlphaNumber() As String
        Get
            Return _CompAlphaNumber
        End Get
        Set(ByVal value As String)
            _CompAlphaNumber = value
        End Set
    End Property

    Private _CompNumber As Integer
    Public Property CompNumber() As Integer
        Get
            Return _CompNumber
        End Get
        Set(ByVal value As Integer)
            _CompNumber = value
        End Set
    End Property

    Private _CompControl As Integer
    Public Property CompControl() As Integer
        Get
            Return _CompControl
        End Get
        Set(ByVal value As Integer)
            _CompControl = value
        End Set
    End Property





End Class
