Public Class clsDefaultIntegrationConfiguration

#Region "Properties"

    Private _ERPTestLegalEntity As String

    Public Property ERPTestLegalEntity() As String
        Get
            Return Left(Me._ERPTestLegalEntity, 50)
        End Get
        Set(value As String)
            If (String.Equals(Me._ERPTestLegalEntity, value) = False) Then
                Me._ERPTestLegalEntity = Left(value, 50)
            End If
        End Set
    End Property

    Private _ERPPayablesLastRunDate As Date

    Public Property ERPPayablesLastRunDate() As Date
        Get
            Return Me._ERPPayablesLastRunDate
        End Get
        Set(value As Date)
            If ((Me._ERPPayablesLastRunDate = value) _
                        = False) Then
                Me._ERPPayablesLastRunDate = value
            End If
        End Set
    End Property

    Private _ERPTestExportMaxRetry As Integer = 1

    Public Property ERPTestExportMaxRetry() As Integer
        Get
            Return Me._ERPTestExportMaxRetry
        End Get
        Set(value As Integer)
            If ((Me._ERPTestExportMaxRetry = value) _
                        = False) Then
                Me._ERPTestExportMaxRetry = value
            End If
        End Set
    End Property

    Private _ERPTestExportRetryMinutes As Integer = 15

    Public Property ERPTestExportRetryMinutes() As Integer
        Get
            Return Me._ERPTestExportRetryMinutes
        End Get
        Set(value As Integer)
            If ((Me._ERPTestExportRetryMinutes = value) _
                        = False) Then
                Me._ERPTestExportRetryMinutes = value
            End If
        End Set
    End Property

    Private _ERPTestExportMaxRowsReturned As Integer = 100

    Public Property ERPTestExportMaxRowsReturned() As Integer
        Get
            Return Me._ERPTestExportMaxRowsReturned
        End Get
        Set(value As Integer)
            If ((Me._ERPTestExportMaxRowsReturned = value) _
                        = False) Then
                Me._ERPTestExportMaxRowsReturned = value
            End If
        End Set
    End Property

    Private _ERPTestExportAutoConfirmation As Boolean = False

    Public Property ERPTestExportAutoConfirmation() As Boolean
        Get
            Return Me._ERPTestExportAutoConfirmation
        End Get
        Set(value As Boolean)
            If ((Me._ERPTestExportAutoConfirmation = value) _
                        = False) Then
                Me._ERPTestExportAutoConfirmation = value
            End If
        End Set
    End Property

    Private _ERPTestFreightCost As Double = 1000.0
    Public Property ERPTestFreightCost() As Double
        Get
            Return _ERPTestFreightCost
        End Get
        Set(ByVal value As Double)
            _ERPTestFreightCost = value
        End Set
    End Property

    Private _ERPTestFreightBillNumber As String = "FB Unit Test"
    Public Property ERPTestFreightBillNumber() As String
        Get
            Return _ERPTestFreightBillNumber
        End Get
        Set(ByVal value As String)
            _ERPTestFreightBillNumber = value
        End Set
    End Property

    Private _ERPTestURI As String
    Public Property ERPTestURI() As String
        Get
            Return Left(Me._ERPTestURI, 250)
        End Get
        Set(value As String)
            If (String.Equals(Me._ERPTestURI, value) = False) Then
                Me._ERPTestURI = Left(value, 250)
            End If
        End Set
    End Property

    Private _ERPTestAuthUser As String

    Public Property ERPTestAuthUser() As String
        Get
            Return Left(Me._ERPTestAuthUser, 100)
        End Get
        Set(value As String)
            If (String.Equals(Me._ERPTestAuthUser, value) = False) Then
                Me._ERPTestAuthUser = Left(value, 100)
            End If
        End Set
    End Property

    Private _ERPTestAuthPassword As String
    ''' <summary>
    ''' Password when a user name is required
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.003 on 07/13/2021 changed size from 25 to 100
    ''' </remarks>
    Public Property ERPTestAuthPassword() As String
        Get
            Return Left(Me._ERPTestAuthPassword, 100)
        End Get
        Set(value As String)
            If (String.Equals(Me._ERPTestAuthPassword, value) = False) Then
                Me._ERPTestAuthPassword = Left(value, 100)
            End If
        End Set
    End Property

    Private _ERPTestAuthCode As String

    Public Property ERPTestAuthCode() As String
        Get
            Return Left(Me._ERPTestAuthCode, 20)
        End Get
        Set(value As String)
            If (String.Equals(Me._ERPTestAuthCode, value) = False) Then
                Me._ERPTestAuthCode = Left(value, 20)
            End If
        End Set
    End Property

    Private _RunERPTest As Boolean = True
    Public Property RunERPTest() As Boolean
        Get
            Return _RunERPTest
        End Get
        Set(ByVal value As Boolean)
            _RunERPTest = value
        End Set
    End Property

    Private _TMSTestServiceURI As String
    Public Property TMSTestServiceURI() As String
        Get
            Return Left(Me._TMSTestServiceURI, 250)
        End Get
        Set(value As String)
            If (String.Equals(Me._TMSTestServiceURI, value) = False) Then
                Me._TMSTestServiceURI = Left(value, 250)
            End If
        End Set
    End Property

    Private _TMSTestServiceAuthUser As String

    Public Property TMSTestServiceAuthUser() As String
        Get
            Return Left(Me._TMSTestServiceAuthUser, 100)
        End Get
        Set(value As String)
            If (String.Equals(Me._TMSTestServiceAuthUser, value) = False) Then
                Me._TMSTestServiceAuthUser = Left(value, 100)
            End If
        End Set
    End Property

    Private _TMSTestServiceAuthPassword As String
    ''' <summary>
    ''' Password when a user name is required
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.003 on 07/13/2021 changed size from 25 to 100
    ''' </remarks>
    Public Property TMSTestServiceAuthPassword() As String
        Get
            Return Left(Me._TMSTestServiceAuthPassword, 100)
        End Get
        Set(value As String)
            If (String.Equals(Me._TMSTestServiceAuthPassword, value) = False) Then
                Me._TMSTestServiceAuthPassword = Left(value, 100)
            End If
        End Set
    End Property

    Private _TMSTestServiceAuthCode As String

    Public Property TMSTestServiceAuthCode() As String
        Get
            Return Left(Me._TMSTestServiceAuthCode, 20)
        End Get
        Set(value As String)
            If (String.Equals(Me._TMSTestServiceAuthCode, value) = False) Then
                Me._TMSTestServiceAuthCode = Left(value, 20)
            End If
        End Set
    End Property

    Private _TMSSettingsURI As String
    Public Property TMSSettingsURI() As String
        Get
            Return Left(Me._TMSSettingsURI, 250)
        End Get
        Set(value As String)
            If (String.Equals(Me._TMSSettingsURI, value) = False) Then
                Me._TMSSettingsURI = Left(value, 250)
            End If
        End Set
    End Property

    Private _TMSSettingsAuthUser As String

    Public Property TMSSettingsAuthUser() As String
        Get
            Return Left(Me._TMSSettingsAuthUser, 100)
        End Get
        Set(value As String)
            If (String.Equals(Me._TMSSettingsAuthUser, value) = False) Then
                Me._TMSSettingsAuthUser = Left(value, 100)
            End If
        End Set
    End Property

    Private _TMSSettingsAuthPassword As String
    ''' <summary>
    ''' Password when a user name is required
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.003 on 07/13/2021 changed size from 25 to 100
    ''' </remarks>
    Public Property TMSSettingsAuthPassword() As String
        Get
            Return Left(Me._TMSSettingsAuthPassword, 100)
        End Get
        Set(value As String)
            If (String.Equals(Me._TMSSettingsAuthPassword, value) = False) Then
                Me._TMSSettingsAuthPassword = Left(value, 100)
            End If
        End Set
    End Property

    Private _TMSSettingsAuthCode As String

    Public Property TMSSettingsAuthCode() As String
        Get
            Return Left(Me._TMSSettingsAuthCode, 20)
        End Get
        Set(value As String)
            If (String.Equals(Me._TMSSettingsAuthCode, value) = False) Then
                Me._TMSSettingsAuthCode = Left(value, 20)
            End If
        End Set
    End Property

    Private _Debug As Boolean = False
    Public Property Debug() As Boolean
        Get
            Return _Debug
        End Get
        Set(ByVal value As Boolean)
            _Debug = value
        End Set
    End Property

    Private _Verbos As Boolean = False
    Public Property Verbos() As Boolean
        Get
            Return _Verbos
        End Get
        Set(ByVal value As Boolean)
            _Verbos = value
        End Set
    End Property

    Private _TMSDBName As String
    Public Property TMSDBName() As String
        Get
            Return _TMSDBName
        End Get
        Set(value As String)
            _TMSDBName = value
        End Set
    End Property

    Private _TMSDBServer As String
    Public Property TMSDBServer() As String
        Get
            Return _TMSDBServer
        End Get
        Set(value As String)
            _TMSDBServer = value
        End Set
    End Property

    Private _TMSDBUser As String
    Public Property TMSDBUser() As String
        Get
            Return _TMSDBUser
        End Get
        Set(value As String)
            _TMSDBUser = value
        End Set
    End Property

    Private _TMSDBPass As String
    Public Property TMSDBPass() As String
        Get
            Return _TMSDBPass
        End Get
        Set(value As String)
            _TMSDBPass = value
        End Set
    End Property

    Private _TMSRunLegalEntity As String
    Public Property TMSRunLegalEntity() As String
        Get
            Return Left(_TMSRunLegalEntity, 50)
        End Get
        Set(value As String)
            _TMSRunLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _ERPFrieghtAccountIndex As String
    Public Property ERPFrieghtAccountIndex() As String
        Get
            Return _ERPFrieghtAccountIndex
        End Get
        Set(ByVal value As String)
            _ERPFrieghtAccountIndex = value
        End Set
    End Property




    Private _GPFunctionsTotalSOWeight As String
    ''' <summary>
    ''' Total Sales Order Weight Configuration Query 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' </remarks>
    Public Property GPFunctionsTotalSOWeight() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsTotalSOWeight) Then
                _GPFunctionsTotalSOWeight = "select sum((sopln.QUANTITY - sopln.QTYCANCE) * iv.ITEMSHWT) as soptotalweight from SOP10200 sopln inner join IV00101 iv on iv.ITEMNMBR = sopln.ITEMNMBR where sopln.SOPNUMBE = '{0}'"
            End If
            Return _GPFunctionsTotalSOWeight
        End Get
        Set(ByVal value As String)
            _GPFunctionsTotalSOWeight = value
        End Set
    End Property

    'Added by SEM 2017-08-25
    'GPFunctionsTotalSOConWeight

    Private _GPFunctionsTotalSOConWeight As String
    Public Property GPFunctionsTotalSOConWeight() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsTotalSOConWeight) Then
                _GPFunctionsTotalSOWeight = "select sum((sopln.QUANTITY - sopln.QTYCANCE) * iv.ITEMSHWT) as soptotalweight from SOP30300 sopln inner join IV00101 iv on iv.ITEMNMBR = sopln.ITEMNMBR where sopln.SOPNUMBE = '{0}'"
            End If
            Return _GPFunctionsTotalSOConWeight
        End Get
        Set(ByVal value As String)
            _GPFunctionsTotalSOConWeight = value
        End Set
    End Property


    Private _GPFunctionsTotalTOWeight As String
    ''' <summary>
    ''' Total Sales Order Weight Configuration Query 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' </remarks>
    Public Property GPFunctionsTotalTOWeight() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsTotalTOWeight) Then
                _GPFunctionsTotalTOWeight = "select sum((invtrn.TRNSFQTY * iv.ITEMSHWT) as soptotalweight from SVC00701 invtrn inner join IV00101 iv on iv.ITEMNMBR = invtrn.ITEMNMBR where invtrn.ORDDOCID = '{0}"
            End If
            Return _GPFunctionsTotalTOWeight
        End Get
        Set(ByVal value As String)
            _GPFunctionsTotalTOWeight = value
        End Set
    End Property

    Private _GPFunctionsTotalPOWeight As String
    ''' <summary>
    ''' Total Purchase Order Weight Configuration Query 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' </remarks>
    Public Property GPFunctionsTotalPOWeight() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsTotalPOWeight) Then
                _GPFunctionsTotalPOWeight = "select sum((sopln.QTYORDER - sopln.QTYCANCE) * iv.ITEMSHWT) as soptotalweight from POP10110 sopln inner join IV00101 iv on iv.ITEMNMBR = sopln.ITEMNMBR where POLNESTA in (1, 2, 3) and  sopln.PONUMBER = '{0}'"
            End If
            Return _GPFunctionsTotalPOWeight
        End Get
        Set(ByVal value As String)
            _GPFunctionsTotalPOWeight = value
        End Set
    End Property

    Private _GPFunctionsTotalSOQuantity As String
    ''' <summary>
    ''' Total Sales Order Quantity Configuration Query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' </remarks>
    Public Property GPFunctionsTotalSOQuantity() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsTotalSOQuantity) Then
                _GPFunctionsTotalSOQuantity = "select sum(sopln.QUANTITY - sopln.QTYCANCE) as soptotalquantity from SOP10200 sopln where sopln.SOPNUMBE = '{0}'"
            End If
            Return _GPFunctionsTotalSOQuantity
        End Get
        Set(ByVal value As String)
            _GPFunctionsTotalSOQuantity = value
        End Set
    End Property

    'Added by SEM 2017-08-25
    'GPFunctionsTotalSOConQuantity
    Private _GPFunctionsTotalSOConQuantity As String
    Public Property GPFunctionsTotalSOConQuantity() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsTotalSOConQuantity) Then
                _GPFunctionsTotalSOConQuantity = "select sum(sopln.QUANTITY - sopln.QTYCANCE) as soptotalquantity from SOP30300 sopln where sopln.SOPNUMBE = '{0}'"
            End If
            Return _GPFunctionsTotalSOConQuantity
        End Get
        Set(ByVal value As String)
            _GPFunctionsTotalSOConQuantity = value
        End Set
    End Property


    Private _GPFunctionsTotalPOQuantity As String
    ''' <summary>
    ''' Total Purchase Order Quantity Configuration Query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' </remarks>
    Public Property GPFunctionsTotalPOQuantity() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsTotalPOQuantity) Then
                _GPFunctionsTotalPOQuantity = "select sum(sopln.QTYORDER - sopln.QTYCANCE) as soptotalquantity from POP10110 sopln where POLNESTA in (1, 2, 3) and sopln.ponumber = '{0}'"
            End If
            Return _GPFunctionsTotalPOQuantity
        End Get
        Set(ByVal value As String)
            _GPFunctionsTotalPOQuantity = value
        End Set
    End Property

    Private _GPFunctionsTotalTOQuantity As String
    ''' <summary>
    ''' Total Transfer Order Quantity Configuration Query 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' </remarks>
    Public Property GPFunctionsTotalTOQuantity() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsTotalTOQuantity) Then
                _GPFunctionsTotalTOQuantity = "select sum(invtrn.TRNSFQTY) as soptotalquantity from SVC00701 invtrn inner join IV00101 iv on iv.ITEMNMBR = invtrn.ITEMNMBR where invtrn.ORDDOCID = '{0}'"
            End If
            Return _GPFunctionsTotalTOQuantity
        End Get
        Set(ByVal value As String)
            _GPFunctionsTotalTOQuantity = value
        End Set
    End Property

    Private _GPFunctionsTotalSOPallets As String
    ''' <summary>
    ''' Total Sales Order Pallets Configuration Query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' </remarks>
    Public Property GPFunctionsTotalSOPallets() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsTotalSOPallets) Then
                _GPFunctionsTotalSOPallets = "select sum(tot_weight.item_weight) as tot_weight From ( select sopln.itemnmbr, case When (sopln.QUANTITY - sopln.QTYCANCE) > 0 then Case When (cast(iv.ITEMSHWT as float) * 0.01) > 0 then 	Case When (cast((sopln.QUANTITY - sopln.QTYCANCE)  as decimal) %  (cast(iv.ITEMSHWT as decimal) * 0.01)) > 0 Then cast((cast((sopln.QUANTITY - sopln.QTYCANCE)  as decimal) /  (cast(iv.ITEMSHWT as decimal) * 0.01)) as int) + 1 Else cast((sopln.QUANTITY - sopln.QTYCANCE)  as decimal) / (cast(iv.ITEMSHWT as decimal) * 0.01)	End Else 1	End	Else 0	end	as item_weight from SOP10200 sopln inner join IV00101 iv on iv.ITEMNMBR = sopln.ITEMNMBR where sopln.SOPNUMBE = '{0}'  and sopln.SOPTYPE = 2) as tot_weight"
            End If
            Return _GPFunctionsTotalSOPallets
        End Get
        Set(ByVal value As String)
            _GPFunctionsTotalSOPallets = value
        End Set
    End Property


    ' Added by SEM on 2017-10-15
    '

    Private _GPFunctionsTotalSOConPallets As String
    ''' <summary>
    ''' Total Sales Order Pallets Configuration Query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' </remarks>
    Public Property GPFunctionsTotalSOConPallets() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsTotalSOConPallets) Then
                _GPFunctionsTotalSOConPallets = "select sum(tot_weight.item_weight) as tot_weight From ( select sopln.itemnmbr, case When (sopln.QUANTITY - sopln.QTYCANCE) > 0 then Case When (cast(iv.ITEMSHWT as float) * 0.01) > 0 then 	Case When (cast((sopln.QUANTITY - sopln.QTYCANCE)  as decimal) %  (cast(iv.ITEMSHWT as decimal) * 0.01)) > 0 Then cast((cast((sopln.QUANTITY - sopln.QTYCANCE)  as decimal) /  (cast(iv.ITEMSHWT as decimal) * 0.01)) as int) + 1 Else cast((sopln.QUANTITY - sopln.QTYCANCE)  as decimal) / (cast(iv.ITEMSHWT as decimal) * 0.01)	End Else 1	End	Else 0	end	as item_weight from SOP30300 sopln inner join IV00101 iv on iv.ITEMNMBR = sopln.ITEMNMBR where sopln.SOPNUMBE = '{0}'  and sopln.SOPTYPE = 3) as tot_weight"
            End If
            Return _GPFunctionsTotalSOConPallets
        End Get
        Set(ByVal value As String)
            _GPFunctionsTotalSOConPallets = value
        End Set
    End Property


    Private _GPFunctionsTotalTOPallets As String
    ''' <summary>
    ''' Total Sales Order Pallets Configuration Query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by SEM for v-7.0.6.103 Update
    '''   added logic to use more configuration settings for GP Functions
    ''' </remarks>
    Public Property GPFunctionsTotalTOPallets() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsTotalTOPallets) Then
                _GPFunctionsTotalTOPallets = "select sum(invtrnln.TRNSFQTY) as invtrntotalquantity from SVC00701 invtrnln where invtrnln.ORDDOCID = '{0}'"
            End If
            Return _GPFunctionsTotalTOPallets
        End Get
        Set(ByVal value As String)
            _GPFunctionsTotalTOPallets = value
        End Set
    End Property

    Private _GPFunctionsTotalPOPallets As String
    ''' <summary>
    ''' Total Purchase Order Pallets Configuration Query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' </remarks>
    Public Property GPFunctionsTotalPOPallets() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsTotalPOPallets) Then
                _GPFunctionsTotalPOPallets = "select sum(QTYORDER - QTYCANCE) as total_qty from POP10110 where POLNESTA in (1, 2, 3) and  PONUMBER = '{0}'"
            End If
            Return _GPFunctionsTotalPOPallets
        End Get
        Set(ByVal value As String)
            _GPFunctionsTotalPOPallets = value
        End Set
    End Property

    'Added by SEM 2017-07-30
    Private _GPFunctionsPONotes As String
    ''' <summary>
    ''' Total Purchase Order Pallets Configuration Query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' </remarks>
    Public Property GPFunctionsPONotes() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsPONotes) Then
                _GPFunctionsPONotes = "select nt.TXTFIELD From POP10100 po inner join SY03900 nt on nt.NOTEINDX = po.PONOTIDS_4 where po.PONUMBER = '{0}'"
            End If
            Return _GPFunctionsPONotes
        End Get
        Set(ByVal value As String)
            _GPFunctionsPONotes = value
        End Set
    End Property

    Private _GPFunctionsSOPComment As String
    
#Disable Warning BC42300 ' XML comment block must immediately precede the language element to which it applies. XML comment will be ignored.
''' <summary>
    ''' Total Purchase Order Pallets Configuration Query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' </remarks>
    ''' 
    'Added by SEM 09/29/2017
    Public Property GPFunctionsSOPComment() As String
#Enable Warning BC42300 ' XML comment block must immediately precede the language element to which it applies. XML comment will be ignored.

        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsSOPComment) Then
                _GPFunctionsSOPComment = "select CMMTTEXT from SOP10106 where SOPNUMBE = '{0}'"
            End If
            Return _GPFunctionsSOPComment
        End Get
        Set(ByVal value As String)
            _GPFunctionsSOPComment = value
        End Set
    End Property


    Private _GPFunctionsPOComment As String
    ''' <summary>
    ''' Total Purchase Order Pallets Configuration Query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' </remarks>
    Public Property GPFunctionsPOComment() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsPOComment) Then
                _GPFunctionsPOComment = "select CMMTTEXT from POP10150 where po.PONUMBER = '{0}'"
            End If
            Return _GPFunctionsPOComment
        End Get
        Set(ByVal value As String)
            _GPFunctionsPOComment = value
        End Set
    End Property


    '  The next two SOP functions were added by SEM on 2017-08-02
    Private _GPFunctionsSOPNotes As String
    ''' <summary>
    ''' Total Purchase Order Pallets Configuration Query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' </remarks>
    Public Property GPFunctionsSOPNotes() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsSOPNotes) Then
                _GPFunctionsSOPNotes = "select nt.TXTFIELD From SOP10100 sop inner join SY03900 nt on nt.NOTEINDX = sop.NOTEINDX where sop.SOPNUMBE = '{0}'"
            End If
            Return _GPFunctionsSOPNotes
        End Get
        Set(ByVal value As String)
            _GPFunctionsSOPNotes = value
        End Set
    End Property



    Private _GPFunctionGetSOPOrdersOnFreightBill As String
    ''' <summary>
    ''' Total Purchase Order Pallets Configuration Query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/14/2016
    '''   added logic to use more configuration settings for GP Functions
    ''' </remarks>
    Public Property GPFunctionGetSOPOrdersOnFreightBill() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionGetSOPOrdersOnFreightBill) Then
                _GPFunctionGetSOPOrdersOnFreightBill = "select SOPNUMBE from SOP30200 where SOPTYPE = 3 and cast(DEX_ROW_TS as date) >= '{0}'"
            End If
            Return _GPFunctionGetSOPOrdersOnFreightBill
        End Get
        Set(ByVal value As String)
            _GPFunctionGetSOPOrdersOnFreightBill = value
        End Set
    End Property

    Private _GPFunctionsSOItemDetails As String
    ''' <summary>
    ''' Configuraiton Setting for Sales Order Item Details SQL Query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/17/2016
    '''   added logic to use more configuration settings for GP Functions and queries
    ''' </remarks>
    Public Property GPFunctionsSOItemDetails() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsSOItemDetails) Then
                '_GPFunctionsSOItemDetails = "select sopln.Itemnmbr as ItemNumber,isnull(UnitCost,0) as ItemCost,isnull(QUANTITY,0) - isnull(QTYCANCE,0) as QtyOrdered,isnull(((sopln.QUANTITY - sopln.QTYCANCE) * iv.ITEMSHWT),1) as [Weight],0 as FixOffInvAllow,0 as FixFrtAllow,0 as FreightCost,isnull(EXTDCOST,0) as ItemCost,0 as [Cube],0 as [Pack],'' as [Size],sopln.ITEMDESC as [Description],'N' as Hazmat,'' as Brand,'' as CostCenter,'' as LotNumber,'' as LotExpirationDate,'' as GTIN,sopln.ITEMNMBR as CustItemNumber,'N' as PalletType,'' as POItemHazmatTypeCode,'' as POItem49CFRCode,'' as POItemIATACode,'' as POItemDOTCode,'' as POItemMarineCode,'' as POItemNMFCClass,'' as POItemFAKClass,'' as POItemLimitedQtyFlag,isnull(QUANTITY,0) - isnull(QTYCANCE,0) as POItemPallets,0 as POItemTies,0 as POItemHighs,0 as POItemQtyPalletPercentage,0 as POItemQtyLength,0 as POItemQtyWidth,0 as POItemQtyHeight,0 as POItemStackable,0 as POItemLevelOfDensity,'' as POItemNMFCSubClass,'' as POItemUser1,'' as POItemUser2,'' as POItemUser3,'' as POItemUser4,UOFM as POItemWeightUnitOfMeasure,UOFM as POItemCubeUnitOfMeasure ,UOFM as POItemDimensionUnitOfMeasure FROM sop10200 sopln inner join IV00101 iv on iv.ITEMNMBR = sopln.ITEMNMBR WHERE SOPTYPE = 2 and SOPNUMBE ='{0}' "
                _GPFunctionsSOItemDetails = "select sopln.Itemnmbr as ItemNumber,isnull(UnitCost,0) as ItemCost,isnull(QUANTITY,0) - isnull(QTYCANCE,0) as QtyOrdered,isnull(((sopln.QUANTITY - sopln.QTYCANCE)),1) as [Weight],0 as FixOffInvAllow,0 as FixFrtAllow,0 as FreightCost,isnull(EXTDCOST,0) as ItemCost,0 as [Cube],0 as [Pack],'' as [Size],sopln.ITEMDESC as [Description],'N' as Hazmat,'' as Brand,'' as CostCenter,'' as LotNumber,'' as LotExpirationDate,'' as GTIN,sopln.ITEMNMBR as CustItemNumber,'N' as PalletType,'' as POItemHazmatTypeCode,'' as POItem49CFRCode,'' as POItemIATACode,'' as POItemDOTCode,'' as POItemMarineCode,'' as POItemNMFCClass,'' as POItemFAKClass,'' as POItemLimitedQtyFlag,isnull(case When (sopln.QUANTITY - sopln.QTYCANCE) > 0 then Case When (cast(iv.ITEMSHWT as float) * 0.01) > 0 then 	Case When (cast((sopln.QUANTITY - sopln.QTYCANCE)  as decimal) %  (cast(iv.ITEMSHWT as decimal) * 0.01)) > 0 Then cast((cast((sopln.QUANTITY - sopln.QTYCANCE)  as decimal) /  (cast(iv.ITEMSHWT as decimal) * 0.01)) as int) + 1 Else cast((sopln.QUANTITY - sopln.QTYCANCE)  as decimal) / (cast(iv.ITEMSHWT as decimal) * 0.01)	End Else 1	End	Else 0	end,0) as POItemPallets,0 as POItemTies,0 as POItemHighs,0 as POItemQtyPalletPercentage,0 as POItemQtyLength,0 as POItemQtyWidth,0 as POItemQtyHeight,0 as POItemStackable,0 as POItemLevelOfDensity,'' as POItemNMFCSubClass,'' as POItemUser1,'' as POItemUser2,'' as POItemUser3,'' as POItemUser4,UOFM as POItemWeightUnitOfMeasure,UOFM as POItemCubeUnitOfMeasure ,UOFM as POItemDimensionUnitOfMeasure FROM sop10200 sopln inner join IV00101 iv on iv.ITEMNMBR = sopln.ITEMNMBR WHERE SOPTYPE = 2 and SOPNUMBE ='{0}'"
            End If
            Return _GPFunctionsSOItemDetails
        End Get
        Set(ByVal value As String)
            _GPFunctionsSOItemDetails = value
        End Set
    End Property


    'Added by SEM 2017-08-25
    'GPFunctionsSOConItemDetails
    Private _GPFunctionsSOConItemDetails As String
    ''' <summary>
    ''' Configuraiton Setting for Sales Order Item Details SQL Query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/17/2016
    '''   added logic to use more configuration settings for GP Functions and queries
    ''' </remarks>
    Public Property GPFunctionsSOConItemDetails() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsSOConItemDetails) Then
                '_GPFunctionsSOConItemDetails = "select sopln.Itemnmbr as ItemNumber,isnull(UnitCost,0) as ItemCost,isnull(QUANTITY,0) - isnull(QTYCANCE,0) as QtyOrdered,isnull(((sopln.QUANTITY - sopln.QTYCANCE) * iv.ITEMSHWT),1) as [Weight],0 as FixOffInvAllow,0 as FixFrtAllow,0 as FreightCost,isnull(EXTDCOST,0) as ItemCost,0 as [Cube],0 as [Pack],'' as [Size],sopln.ITEMDESC as [Description],'N' as Hazmat,'' as Brand,'' as CostCenter,'' as LotNumber,'' as LotExpirationDate,'' as GTIN,sopln.ITEMNMBR as CustItemNumber,'N' as PalletType,'' as POItemHazmatTypeCode,'' as POItem49CFRCode,'' as POItemIATACode,'' as POItemDOTCode,'' as POItemMarineCode,'' as POItemNMFCClass,'' as POItemFAKClass,'' as POItemLimitedQtyFlag,isnull(QUANTITY,0) - isnull(QTYCANCE,0) as POItemPallets,0 as POItemTies,0 as POItemHighs,0 as POItemQtyPalletPercentage,0 as POItemQtyLength,0 as POItemQtyWidth,0 as POItemQtyHeight,0 as POItemStackable,0 as POItemLevelOfDensity,'' as POItemNMFCSubClass,'' as POItemUser1,'' as POItemUser2,'' as POItemUser3,'' as POItemUser4,UOFM as POItemWeightUnitOfMeasure,UOFM as POItemCubeUnitOfMeasure ,UOFM as POItemDimensionUnitOfMeasure FROM sop30300 sopln inner join IV00101 iv on iv.ITEMNMBR = sopln.ITEMNMBR WHERE SOPTYPE = 2  and SOPNUMBE = '{0}'"
                _GPFunctionsSOConItemDetails = "select sopln.Itemnmbr as ItemNumber,isnull(UnitCost,0) as ItemCost,isnull(QUANTITY,0) - isnull(QTYCANCE,0) as QtyOrdered,isnull(((sopln.QUANTITY - sopln.QTYCANCE)),1) as [Weight],0 as FixOffInvAllow,0 as FixFrtAllow,0 as FreightCost,isnull(EXTDCOST,0) as ItemCost,0 as [Cube],0 as [Pack],'' as [Size],sopln.ITEMDESC as [Description],'N' as Hazmat,'' as Brand,'' as CostCenter,'' as LotNumber,'' as LotExpirationDate,'' as GTIN,sopln.ITEMNMBR as CustItemNumber,'N' as PalletType,'' as POItemHazmatTypeCode,'' as POItem49CFRCode,'' as POItemIATACode,'' as POItemDOTCode,'' as POItemMarineCode,'' as POItemNMFCClass,'' as POItemFAKClass,'' as POItemLimitedQtyFlag,isnull(case When (sopln.QUANTITY - sopln.QTYCANCE) > 0 then Case When (cast(iv.ITEMSHWT as float) * 0.01) > 0 then 	Case When (cast((sopln.QUANTITY - sopln.QTYCANCE)  as decimal) %  (cast(iv.ITEMSHWT as decimal) * 0.01)) > 0 Then cast((cast((sopln.QUANTITY - sopln.QTYCANCE)  as decimal) /  (cast(iv.ITEMSHWT as decimal) * 0.01)) as int) + 1 Else cast((sopln.QUANTITY - sopln.QTYCANCE)  as decimal) / (cast(iv.ITEMSHWT as decimal) * 0.01)	End Else 1	End	Else 0	end,0) as POItemPallets,0 as POItemTies,0 as POItemHighs,0 as POItemQtyPalletPercentage,0 as POItemQtyLength,0 as POItemQtyWidth,0 as POItemQtyHeight,0 as POItemStackable,0 as POItemLevelOfDensity,'' as POItemNMFCSubClass,'' as POItemUser1,'' as POItemUser2,'' as POItemUser3,'' as POItemUser4,UOFM as POItemWeightUnitOfMeasure,UOFM as POItemCubeUnitOfMeasure ,UOFM as POItemDimensionUnitOfMeasure FROM sop30300 sopln inner join IV00101 iv on iv.ITEMNMBR = sopln.ITEMNMBR WHERE SOPTYPE = 3 and SOPNUMBE ='{0}'"
            End If
            Return _GPFunctionsSOConItemDetails
        End Get
        Set(ByVal value As String)
            _GPFunctionsSOConItemDetails = value
        End Set
    End Property


    Private _GPFunctionsTOItemDetails As String
    ''' <summary>
    ''' Configuraiton Setting for Sales Order Item Details SQL Query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by SEM for v-7.0.6.103 Update
    '''   added logic to include inventory transfer
    ''' </remarks>
    Public Property GPFunctionsTOItemDetails() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsTOItemDetails) Then
                _GPFunctionsTOItemDetails = "select sopln.Itemnmbr as ItemNumber,0 as ItemCost,isnull(TRNSFQTY,0) as QtyOrdered,isnull((sopln.QTY_To_Receive * iv.ITEMSHWT),1) as [Weight],0 as FixOffInvAllow,0 as FixFrtAllow,0 as FreightCost,0 as ItemCost,0 as [Cube],0 as [Pack],'' as [Size],DSCRIPTN as [Description],'N' as Hazmat,'' as Brand,'' as CostCenter,'' as LotNumber,'' as LotExpirationDate,'' as GTIN,sopln.ITEMNMBR as CustItemNumber,'N' as PalletType,'' as POItemHazmatTypeCode,'' as POItem49CFRCode,'' as POItemIATACode,'' as POItemDOTCode,'' as POItemMarineCode,'' as POItemNMFCClass,'' as POItemFAKClass,'' as POItemLimitedQtyFlag,isnull(TRNSFQTY,0) as POItemPallets,0 as POItemTies,0 as POItemHighs,0 as POItemQtyPalletPercentage,0 as POItemQtyLength,0 as POItemQtyWidth,0 as POItemQtyHeight,0 as POItemStackable,0 as POItemLevelOfDensity,'' as POItemNMFCSubClass,'' as POItemUser1,'' as POItemUser2,'' as POItemUser3,'' as POItemUser4,UOFM as POItemWeightUnitOfMeasure,UOFM as POItemCubeUnitOfMeasure ,UOFM as POItemDimensionUnitOfMeasure FROM SVC00701  sopln inner join IV00101 iv on iv.ITEMNMBR = sopln.ITEMNMBR WHERE ORDDOCID = '{0}'"
            End If
            Return _GPFunctionsTOItemDetails
        End Get
        Set(ByVal value As String)
            _GPFunctionsTOItemDetails = value
        End Set
    End Property

    '<GPFunctionsPOItemDetails>select Itemnmbr as ItemNumber,isnull(UnitCost,0) as ItemCost,isnull(QTYORDER,0) - isnull(QTYCANCE,0) as QtyOrdered,isnull((select top 1 iv.ITEMSHWT from IV00101 as iv where iv.ITEMNMBR = Itemnmbr),1) as [Weight],0 as FixOffInvAllow,0 as FixFrtAllow,0 as FreightCost,isnull(EXTDCOST,0) as ItemCost,0 as [Cube],0 as [Pack],'' as [Size],ITEMDESC as [Description],'N' as Hazmat,'' as Brand,'' as CostCenter,'' as LotNumber,'' as LotExpirationDate,'' as GTIN,ITEMNMBR as CustItemNumber,'N' as PalletType,'' as POItemHazmatTypeCode,'' as POItem49CFRCode,'' as POItemIATACode,'' as POItemDOTCode,'' as POItemMarineCode,'' as POItemNMFCClass,'' as POItemFAKClass,'' as POItemLimitedQtyFlag,isnull(QTYORDER,0) - isnull(QTYCANCE,0) as POItemPallets,0 as POItemTies,0 as POItemHighs,0 as POItemQtyPalletPercentage,0 as POItemQtyLength,0 as POItemQtyWidth,0 as POItemQtyHeight,0 as POItemStackable,0 as POItemLevelOfDensity,'' as POItemNMFCSubClass,'' as POItemUser1,'' as POItemUser2,'' as POItemUser3,'' as POItemUser4,UOFM as POItemWeightUnitOfMeasure,UOFM as POItemCubeUnitOfMeasure ,UOFM as POItemDimensionUnitOfMeasure from pop10110 where POLNESTA in (1, 2, 3) and PONUMBER = '{0}'</GPFunctionsPOItemDetails>
    Private _GPFunctionsPOItemDetails As String
    ''' <summary>
    ''' Configuraiton Setting for Purchase Order Item Details SQL Query
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/17/2016
    '''   added logic to use more configuration settings for GP Functions and queries
    ''' </remarks>
    Public Property GPFunctionsPOItemDetails() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsPOItemDetails) Then
                _GPFunctionsPOItemDetails = "select sopln.Itemnmbr as ItemNumber,isnull(UnitCost,0) as ItemCost,isnull(QTYORDER,0) - isnull(QTYCANCE,0) as QtyOrdered,isnull(((sopln.QTYORDER - sopln.QTYCANCE) * iv.ITEMSHWT),1) as [Weight],0 as FixOffInvAllow,0 as FixFrtAllow,0 as FreightCost,isnull(EXTDCOST,0) as ItemCost,0 as [Cube],0 as [Pack],'' as [Size],sopln.ITEMDESC as [Description],'N' as Hazmat,'' as Brand,'' as CostCenter,'' as LotNumber,'' as LotExpirationDate,'' as GTIN,sopln.ITEMNMBR as CustItemNumber,'N' as PalletType,'' as POItemHazmatTypeCode,'' as POItem49CFRCode,'' as POItemIATACode,'' as POItemDOTCode,'' as POItemMarineCode,'' as POItemNMFCClass,'' as POItemFAKClass,'' as POItemLimitedQtyFlag,isnull(QTYORDER,0) - isnull(QTYCANCE,0) as POItemPallets,0 as POItemTies,0 as POItemHighs,0 as POItemQtyPalletPercentage,0 as POItemQtyLength,0 as POItemQtyWidth,0 as POItemQtyHeight,0 as POItemStackable,0 as POItemLevelOfDensity,'' as POItemNMFCSubClass,'' as POItemUser1,'' as POItemUser2,'' as POItemUser3,'' as POItemUser4,UOFM as POItemWeightUnitOfMeasure,UOFM as POItemCubeUnitOfMeasure ,UOFM as POItemDimensionUnitOfMeasure from pop10110 sopln inner join IV00101 iv on iv.ITEMNMBR = sopln.ITEMNMBR where POLNESTA in (1, 2, 3) and PONUMBER = '{0}'"
            End If
            Return _GPFunctionsPOItemDetails
        End Get
        Set(ByVal value As String)
            _GPFunctionsPOItemDetails = value
        End Set
    End Property

    Private _GPFunctionsForceDefaultCountry As String
    ''' <summary>
    ''' Used to override country values in booking addresses ensures correct tariff selection
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 10/18/2016
    ''' </remarks>
    Public Property GPFunctionsForceDefaultCountry() As String
        Get
            Return _GPFunctionsForceDefaultCountry
        End Get
        Set(ByVal value As String)
            _GPFunctionsForceDefaultCountry = value
        End Set
    End Property



    'Added by SEM 2017-08-04
    'Added to set value of batch prefix for payables
    'GPPMBatchPrefix

    Private _GPPMBatchPrefix As String
    ''' <summary>
    ''' Flag to determine if Payables are on or off
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 11/17/2016
    ''' </remarks>
    Public Property GPPMBatchPrefix() As String
        Get
            Return _GPPMBatchPrefix
        End Get
        Set(ByVal value As String)
            _GPPMBatchPrefix = value
        End Set
    End Property


    'Added by SEM 2017-08-04
    'GPBatchDateToAdd

    Private _GPBatchDateToAdd As Integer
    Public Property GPBatchDateToAdd() As Integer
        Get
            Return _GPBatchDateToAdd
        End Get
        Set(ByVal value As Integer)
            _GPBatchDateToAdd = value
        End Set
    End Property

    'Addec by SEM 2017-08-24
    'GPFunctionOrderUpdateOn
    Private _GPFunctionSOPCostUpdateOn As Boolean
    Public Property GPFunctionSOPCostUpdateOn() As Boolean
        Get
            Return _GPFunctionSOPCostUpdateOn
        End Get
        Set(ByVal value As Boolean)
            _GPFunctionSOPCostUpdateOn = value
        End Set
    End Property

    'Added by SEM 2017-08-24
    'FrightBillCostsOn
    Private _FrightBillCostsOn As Boolean
    Public Property FrightBillCostsOn() As Boolean
        Get
            Return _FrightBillCostsOn
        End Get
        Set(ByVal value As Boolean)
            _FrightBillCostsOn = value
        End Set
    End Property

    'Added by SEM 2017-08-25
    'GPFunctionShipConfirmationOn
    Private _GPFunctionShipConfirmationOn As Boolean
    Public Property GPFunctionShipConfirmationOn() As Boolean
        Get
            Return _GPFunctionShipConfirmationOn
        End Get
        Set(ByVal value As Boolean)
            _GPFunctionShipConfirmationOn = value
        End Set
    End Property


    'Added by SEM 2017-08-24
    'GPFunctionCheckForSOPUserDefinedRecord
    Private _GPFunctionCheckForSOPUserDefinedRecord As String

    Public Property GPFunctionCheckForSOPUserDefinedRecord() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionCheckForSOPUserDefinedRecord) Then
                _GPFunctionCheckForSOPUserDefinedRecord = "select count(*) from SOP10106 where SOPTYPE = 2 and SOPNUMBE = '{0}'"
            End If
            Return _GPFunctionCheckForSOPUserDefinedRecord
        End Get
        Set(ByVal value As String)
            _GPFunctionCheckForSOPUserDefinedRecord = value
        End Set
    End Property

    'Added by SEM 2017-11-29
    'GPSOPRequiredShipDate

    Private _GPSOPOrdRequiredShipDate As String

    Public Property GPSOPOrdRequiredShipDate() As String
        Get
            If String.IsNullOrWhiteSpace(_GPSOPOrdRequiredShipDate) Then
                _GPSOPOrdRequiredShipDate = "select USRDAT01 from sop10106 where SOPTYPE = 2 and SOPNUMBE = '{0}'"
            End If
            Return _GPSOPOrdRequiredShipDate
        End Get
        Set(ByVal value As String)
            _GPSOPOrdRequiredShipDate = value
        End Set
    End Property


    'Added by SEM 2017-11-29
    'GPSOPRequiredShipDate

    Private _GPSOPShipConfirmRequiredShipDate As String

    Public Property GPSOPShipConfirmRequiredShipDate() As String
        Get
            If String.IsNullOrWhiteSpace(_GPSOPShipConfirmRequiredShipDate) Then
                _GPSOPShipConfirmRequiredShipDate = "select USRDAT01 from sop10106 where SOPTYPE = 3 and SOPNUMBE = '{0}'"
            End If
            Return _GPSOPShipConfirmRequiredShipDate
        End Get
        Set(ByVal value As String)
            _GPSOPShipConfirmRequiredShipDate = value
        End Set
    End Property


    'Added by SEM 2017-09-29
    'GPFunctionInsertUserDefinedRecord
    Private _GPFunctionInsertUserDefinedRecord As String
    Public Property GPFunctionInsertUserDefinedRecord() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionInsertUserDefinedRecord) Then
                _GPFunctionInsertUserDefinedRecord = "insert into SOP10106 (SOPTYPE, SOPNUMBE, USRDEF03, CMMTTEXT) select 2, '{0}', {1}, ' '"
            End If
            Return _GPFunctionInsertUserDefinedRecord
        End Get
        Set(ByVal value As String)
            _GPFunctionInsertUserDefinedRecord = value
        End Set
    End Property


    'Added by SEM 2017-09-04
    'GPPMDescriptionFieldValue

    Private _GPPMDescriptionFieldValue As String
    Public Property GPPMDescriptionFieldValue() As String
        Get
            If String.IsNullOrWhiteSpace(_GPPMDescriptionFieldValue) Then
                _GPPMDescriptionFieldValue = "STANDARD"
            End If
            Return _GPPMDescriptionFieldValue
        End Get
        Set(ByVal value As String)
            _GPPMDescriptionFieldValue = value
        End Set
    End Property


    'GPAPAmountField

    Private _GPAPAmountField As String
    Public Property GPAPAmountField() As String
        Get
            If String.IsNullOrWhiteSpace(_GPAPAmountField) Then
                _GPAPAmountField = "FREIGHT"
            End If
            Return _GPAPAmountField
        End Get
        Set(ByVal value As String)
            _GPAPAmountField = value
        End Set
    End Property

    'Added by SEM 2017-09-04
    'GPPMPOFieldValue
    Private _GPPMPOFieldValue As String
    Public Property GPPMPOFieldValue() As String
        Get
            If String.IsNullOrWhiteSpace(_GPPMPOFieldValue) Then
                _GPPMPOFieldValue = "PO"
            End If
            Return _GPPMPOFieldValue
        End Get
        Set(ByVal value As String)
            _GPPMPOFieldValue = value
        End Set
    End Property




    'Added by SEM 2017-08-24
    'GPFunctionUpdateUserDefinedRecord
    Private _GPFunctionUpdateUserDefinedRecord As String
    Public Property GPFunctionUpdateUserDefinedRecord() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionUpdateUserDefinedRecord) Then
                _GPFunctionUpdateUserDefinedRecord = "update SOP10106 set USRDEF03 = {1} where SOPTYPE = 2 and SOPNUMBE = '{0}'"
            End If
            Return _GPFunctionUpdateUserDefinedRecord
        End Get
        Set(ByVal value As String)
            _GPFunctionUpdateUserDefinedRecord = value
        End Set
    End Property


    'Added by SEM 2017-08-04
    'GPFunctionGetItemTemp

    Private _GPFunctionGetItemTemp As String
    Public Property GPFunctionGetItemTemp() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionGetItemTemp) Then
                _GPFunctionGetItemTemp = "select sopln.*, itm.* from SOP10200 sopln inner join IV00101 itm on itm.ITEMNMBR = sopln.ITEMNMBR where sopln.SOPNUMBE  = '{0}' and sopln.ITEMNMBR = '{1}'"
            End If
            Return _GPFunctionGetItemTemp
        End Get
        Set(ByVal value As String)
            _GPFunctionGetItemTemp = value
        End Set
    End Property


    Private _PayablesOn As Boolean
    ''' <summary>
    ''' Flag to determine if Payables are on or off
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 11/17/2016
    ''' </remarks>
    Public Property PayablesOn() As Boolean
        Get
            Return _PayablesOn
        End Get
        Set(ByVal value As Boolean)
            _PayablesOn = value
        End Set
    End Property

    Private _APExportOn As Boolean
    ''' <summary>
    ''' Flag to determine if AP Export is on or off
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 11/17/2016
    ''' </remarks>
    Public Property APExportOn() As Boolean
        Get
            Return _APExportOn
        End Get
        Set(ByVal value As Boolean)
            _APExportOn = value
        End Set
    End Property

    Private _GPFunctionsGetSOShippingMethod As String
    ''' <summary>
    ''' SQL query to get SO Shipping Method and Trans Type data
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 11/21/2016
    ''' </remarks>
    Public Property GPFunctionsGetSOShippingMethod() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsGetSOShippingMethod) Then
                _GPFunctionsGetSOShippingMethod = "select sopln.SHIPMTHD, sh.SHMTHDSC, sh.SHIPTYPE, Case sh.SHIPTYPE When 0 Then 3 Else 4 End as TransType from SOP10200 sopln inner join SY03000 sh on sh.SHIPMTHD = sopln.SHIPMTHD  where sopln.SOPTYPE = 2 and sopln.SOPNUMBE = '{0}'"
            End If
            Return _GPFunctionsGetSOShippingMethod
        End Get
        Set(ByVal value As String)
            _GPFunctionsGetSOShippingMethod = value
        End Set
    End Property

    'Added by SEM 2017-08-24
    'GPFunctionsGetSOConShippingMethod

    Private _GPFunctionsGetSOConShippingMethod As String
    Public Property GPFunctionsGetSOConShippingMethod() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsGetSOConShippingMethod) Then
                _GPFunctionsGetSOConShippingMethod = "select sopln.SHIPMTHD, sh.SHMTHDSC, sh.SHIPTYPE, Case sh.SHIPTYPE When 0 Then 3 Else 4 End as TransType from SOP30300 sopln inner join SY03000 sh on sh.SHIPMTHD = sopln.SHIPMTHD  where sopln.SOPTYPE = 2 and sopln.SOPNUMBE = '{0}'"
            End If
            Return _GPFunctionsGetSOConShippingMethod
        End Get
        Set(ByVal value As String)
            _GPFunctionsGetSOConShippingMethod = value
        End Set
    End Property


    Private _GPFunctionsGetTOShippingMethod As String
    ''' <summary>
    ''' SQL query to get SO Shipping Method and Trans Type data
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 11/21/2016
    ''' </remarks>
    Public Property GPFunctionsGetTOShippingMethod() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsGetTOShippingMethod) Then
                _GPFunctionsGetTOShippingMethod = "select 3 as TransType from SVC00701 invtrnln where invtrnln.ORDDOCID = '{0}'"
            End If
            Return _GPFunctionsGetTOShippingMethod
        End Get
        Set(ByVal value As String)
            _GPFunctionsGetTOShippingMethod = value
        End Set
    End Property


    Private _GPFunctionsGetPOShippingMethod As String
    ''' <summary>
    ''' SQL query to get PO Shipping Method and Trans Type data
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 11/21/2016
    ''' </remarks>
    Public Property GPFunctionsGetPOShippingMethod() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsGetPOShippingMethod) Then
                '_GPFunctionsGetPOShippingMethod = "select 'Inbound' as SHIPMTHD,'Purchase Order' as SHMTHDSC, 8 as TransType from POP10110 where POLNESTA in (1, 2, 3) and PONUMBER = '{0}'"
                ' Modified by SEM on 2017-07-26.  Removed line status
                _GPFunctionsGetPOShippingMethod = "select 'Inbound' as SHIPMTHD,'Purchase Order' as SHMTHDSC, 8 as TransType from POP10110 where PONUMBER = '{0}'"

            End If
            Return _GPFunctionsGetPOShippingMethod
        End Get
        Set(ByVal value As String)
            _GPFunctionsGetPOShippingMethod = value
        End Set
    End Property

    Private _GPFunctionsGetSOTemp As String
    ''' <summary>
    ''' SQL query to get SO Temperature data
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 11/21/2016
    ''' </remarks>
    Public Property GPFunctionsGetSOTemp() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsGetSOTemp) Then
                _GPFunctionsGetSOTemp = "select 'D' as SOTemp from SOP10200 sopln inner join SY03000 sh on sh.SHIPMTHD = sopln.SHIPMTHD where sopln.SOPTYPE = 2 and sopln.SOPNUMBE = '{0}'"
            End If
            Return _GPFunctionsGetSOTemp
        End Get
        Set(ByVal value As String)
            _GPFunctionsGetSOTemp = value
        End Set
    End Property


    'Added by SEM 2017-10-15

    Private _GPFunctionsGetSOConTemp As String
    ''' <summary>
    ''' SQL query to get SO Temperature data
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 11/21/2016
    ''' </remarks>
    Public Property GPFunctionsGetSOConTemp() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsGetSOConTemp) Then
                _GPFunctionsGetSOConTemp = "select 'D' as SOTemp from SOP30300 sopln inner join SY03000 sh on sh.SHIPMTHD = sopln.SHIPMTHD where sopln.SOPTYPE = 2 and sopln.SOPNUMBE = '{0}'"
            End If
            Return _GPFunctionsGetSOConTemp
        End Get
        Set(ByVal value As String)
            _GPFunctionsGetSOConTemp = value
        End Set
    End Property


    Private _GPFunctionsGetTOTemp As String
    ''' <summary>
    ''' SQL query to get SO Temperature data
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by SEM for v-7.0.6.103 Update
    ''' </remarks>
    Public Property GPFunctionsGetTOTemp() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsGetTOTemp) Then
                _GPFunctionsGetTOTemp = "select 'D' as SOTemp from SOP10200 sopln inner join SY03000 sh on sh.SHIPMTHD = sopln.SHIPMTHD where sopln.SOPTYPE = 2 and sopln.SOPNUMBE = '{0}'"
            End If
            Return _GPFunctionsGetTOTemp
        End Get
        Set(ByVal value As String)
            _GPFunctionsGetTOTemp = value
        End Set
    End Property

    Private _GPFunctionsGetPOTemp As String
    ''' <summary>
    ''' SQL query to get SO Temperature data
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 11/21/2016
    ''' </remarks>
    Public Property GPFunctionsGetPOTemp() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsGetPOTemp) Then
                _GPFunctionsGetPOTemp = "select 'D' as POTemp from POP10110 where PONUMBER = '{0}'"
            End If
            Return _GPFunctionsGetPOTemp
        End Get
        Set(ByVal value As String)
            _GPFunctionsGetPOTemp = value
        End Set
    End Property


    Private _GPFunctionsGetPayableCreditGL As String
    ''' <summary>
    ''' SQL query to get Payable Credit GL
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 11/28/2016
    ''' </remarks>
    Public Property GPFunctionsGetPayableCreditGL() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsGetPayableCreditGL) Then
                _GPFunctionsGetPayableCreditGL = "SELECT rtrim(gl.ACTNUMST) as gl_code from SY01100 post inner join gl00105  gl on gl.ACTINDX = post.ACTINDX where SERIES = 4 and SEQNUMBR = 200"
            End If
            Return _GPFunctionsGetPayableCreditGL
        End Get
        Set(ByVal value As String)
            _GPFunctionsGetPayableCreditGL = value
        End Set
    End Property

    Private _GPFunctionsGetCurrencyID As String
    ''' <summary>
    ''' configuration property for the CurrencyID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 11/28/2016
    ''' </remarks>
    Public Property GPFunctionsGetCurrencyID() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsGetCurrencyID) Then
                _GPFunctionsGetCurrencyID = "Z-US$"
            End If
            Return _GPFunctionsGetCurrencyID
        End Get
        Set(ByVal value As String)
            _GPFunctionsGetCurrencyID = value
        End Set
    End Property

    Private _GPFunctionsDefaultFreightGLAccount As String
    ''' <summary>
    ''' configuration property for the Default Freight GL Account used if users do not enter one in TMS
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 12/01/2016
    ''' </remarks>
    Public Property GPFunctionsDefaultFreightGLAccount() As String
        Get
            Return _GPFunctionsDefaultFreightGLAccount
        End Get
        Set(ByVal value As String)
            _GPFunctionsDefaultFreightGLAccount = value
        End Set
    End Property


    '************* Properties Added by RHR on 10/25/2017  **************************

    Private _PurchaseOrderOn As Boolean = True
    ''' <summary>
    ''' Flag to determine if purchase orders are on or off
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 10/25/2017
    ''' </remarks>
    Public Property PurchaseOrderOn() As Boolean
        Get
            Return _PurchaseOrderOn
        End Get
        Set(ByVal value As Boolean)
            _PurchaseOrderOn = value
        End Set
    End Property

    Private _SalesOrderOn As Boolean = True
    ''' <summary>
    ''' Flag to determine if Sales Orders are on or off
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 10/25/2017
    ''' </remarks>
    Public Property SalesOrderOn() As Boolean
        Get
            Return _SalesOrderOn
        End Get
        Set(ByVal value As Boolean)
            _SalesOrderOn = value
        End Set
    End Property

    Private _TransferOrderOn As Boolean = False
    ''' <summary>
    ''' Flag to determine if Transfer Orders are on or off (for future not currently available
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 10/25/2017
    ''' </remarks>
    Public Property TransferOrderOn() As Boolean
        Get
            Return _TransferOrderOn
        End Get
        Set(ByVal value As Boolean)
            _TransferOrderOn = value
        End Set
    End Property

    Private _IntegrationTimerMS As Integer = 5000
    ''' <summary>
    ''' Number of milliseconds between integration cycles
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 10/25/2017
    '''   used to replace the current integration timer  we now use the 
    '''   IntegrationTimerMS to store the number of milliseconds  between cycles 
    '''   in the integration service
    ''' </remarks>
    Public Property IntegrationTimerMS() As Integer
        Get
            Return _IntegrationTimerMS
        End Get
        Set(ByVal value As Integer)
            _IntegrationTimerMS = value
        End Set
    End Property


    Private _GPFunctionsSOHeaders As String
    ''' <summary>
    ''' SQL string to select Sales Order Header Records, dependent upon GPFunctionsSOsToProcess
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 10/25/2017
    ''' </remarks>
    Public Property GPFunctionsSOHeaders() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsSOHeaders) Then
                _GPFunctionsSOHeaders = "Select * from sop10100 where SOPTYPE = {0} And SOPNUMBE = '{1}'"
            End If
            Return _GPFunctionsSOHeaders
        End Get
        Set(ByVal value As String)
            _GPFunctionsSOHeaders = value
        End Set
    End Property


    Private _GPFunctionsSOConHeaders As String
    ''' <summary>
    ''' SQL string to select Ship Confirmation Header Records, dependent upon GPFunctionsSOConsToProcess
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 10/25/2017
    ''' </remarks>
    Public Property GPFunctionsSOConHeaders() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsSOConHeaders) Then
                _GPFunctionsSOConHeaders = "select * from sop30200 where SOPTYPE = {0} and SOPNUMBE = '{1}'"
            End If
            Return _GPFunctionsSOConHeaders
        End Get
        Set(ByVal value As String)
            _GPFunctionsSOConHeaders = value
        End Set
    End Property

    Private _GPFunctionsPOHeaders As String
    ''' <summary>
    ''' SQL string to select Purchase Order Header Records, dependent upon GPFunctionsPOsToProcess
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 10/25/2017
    ''' </remarks>
    Public Property GPFunctionsPOHeaders() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsPOHeaders) Then
                _GPFunctionsPOHeaders = "select * from sop30200 where SOPTYPE = {0} and SOPNUMBE = '{1}'"
            End If
            Return _GPFunctionsPOHeaders
        End Get
        Set(ByVal value As String)
            _GPFunctionsPOHeaders = value
        End Set
    End Property


    Private _GPFunctionsSOsToProcess As String
    ''' <summary>
    ''' SQL string to select The list of active Sales Orders to process
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 10/25/2017
    ''' </remarks>
    Public Property GPFunctionsSOsToProcess() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsSOsToProcess) Then
                _GPFunctionsSOsToProcess = "select SOPNUMBE From SOP10100 where SOPTYPE = {0} And ReqShipDate > dateadd(d,-14,getdate())"
            End If
            Return _GPFunctionsSOsToProcess
        End Get
        Set(ByVal value As String)
            _GPFunctionsSOsToProcess = value
        End Set
    End Property


    Private _GPFunctionsSOConsToProcess As String
    ''' <summary>
    ''' SQL string to select The list of active Ship Confirmations to process
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 10/25/2017
    ''' </remarks>
    Public Property GPFunctionsSOConsToProcess() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsSOConsToProcess) Then
                _GPFunctionsSOConsToProcess = "select SOPNUMBE from SOP30200 where SOPTYPE = {0} and dex_row_ts >= '{1}'"
            End If
            Return _GPFunctionsSOConsToProcess
        End Get
        Set(ByVal value As String)
            _GPFunctionsSOConsToProcess = value
        End Set
    End Property

    Private _GPFunctionsPOsToProcess As String
    ''' <summary>
    ''' SQL string to select The list of active Purchase Orders to process
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 10/25/2017
    ''' </remarks>
    Public Property GPFunctionsPOsToProcess() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsPOsToProcess) Then
                _GPFunctionsPOsToProcess = "select PONUMBER from pop10100 where postatus in (1, 2, 3) and REQDATE > dateadd(d,-14,getdate())"
            End If
            Return _GPFunctionsPOsToProcess
        End Get
        Set(ByVal value As String)
            _GPFunctionsPOsToProcess = value
        End Set
    End Property

    '<GPFunctionsPOLocationCode>select top 1 LOCNCODE from POP10110 where PONUMBER = '{0}'</GPFunctionsPOLocationCode>
    Private _GPFunctionsPOLocationCode As String
    ''' <summary>
    ''' SQL string to select The location code for Purchase Orders
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 10/25/2017
    ''' </remarks>
    Public Property GPFunctionsPOLocationCode() As String
        Get
            If String.IsNullOrWhiteSpace(_GPFunctionsPOLocationCode) Then
                _GPFunctionsPOLocationCode = "select top 1 LOCNCODE from POP10110 where PONUMBER = '{0}'"
            End If
            Return _GPFunctionsPOLocationCode
        End Get
        Set(ByVal value As String)
            _GPFunctionsPOLocationCode = value
        End Set
    End Property

#End Region

End Class
