Imports System.Windows.Forms
Namespace Models

    ''' <summary>
    '''  The controller class must specifically process the data string property and perform any required conversion to objects
    ''' </summary>
    ''' <remarks> 
    ''' Modified by RHR for v-8.1 on 02/21/2018
    ''' Added Data property to store a string verison of extra data to be passed into the contoller class
    ''' The controller class must specifically process the data string and perform any required conversion to objects 
    ''' Modified by RHR for v-8.3.0.001 on 08/12/2020
    '''     added logic to store group by settings in a string array
    ''' Modified by RHR for v-8.5.1.002 on 04/09/2022 added Primary Key Property
    ''' </remarks>
    Public Class AllFilters

        Private _page As Integer
        Private _pageSize As Integer
        Private _skip As Integer
        Private _take As Integer
        Private _CarrierControlFrom As Integer
        Private _CarrierControlTo As Integer
        Private _CarrierNumberFrom As Integer
        Private _CarrierNumberTo As Integer
        Private _CompControlFrom As Integer
        Private _CompControlTo As Integer
        Private _CompNumberFrom As Integer
        Private _CompNumberTo As Integer
        Private _BookControl As Integer
        Private _LaneControl As Integer
        Private _LEAdminControl As Integer
        Private _Data As String             'Modified by RHR for v-8.1 on 02/21/2018
        Private _ParentControl As Integer
        Private _ApptControl As Integer
        Private _NatActNumber As Integer
        Private _ContactControl As Integer
        ' Modified by RHR for v-8.3.0.001 on 08/12/2020
        Private _Groups As New List(Of String) ' a list of field names used for group by values  data in results must be sorted ASC by group settings on read
        ' Modified by RHR for v-8.5.1.002 on 04/09/2022
        Private _PrimaryKey As Integer


        Private _FilterValues As New List(Of FilterDetails)
        ''' <summary>
        ''' Filter value array of filters
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.2 on 08/08/2018
        ''' </remarks>
        Public Property FilterValues() As FilterDetails()
            Get
                If _FilterValues Is Nothing Then _FilterValues = New List(Of FilterDetails)
                Return _FilterValues.ToArray()
            End Get
            Set(ByVal value As FilterDetails())
                If Not value Is Nothing Then
                    _FilterValues = value.ToList()
                Else
                    _FilterValues = New List(Of FilterDetails)
                End If
            End Set
        End Property

        Private _SortValues As New List(Of SortDetails)
        ''' <summary>
        ''' name and direction array of sorting details
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.2 on 08/08/2018
        ''' </remarks>
        Public Property SortValues() As SortDetails()
            Get
                If _SortValues Is Nothing Then _SortValues = New List(Of SortDetails)
                Return _SortValues.ToArray()
            End Get
            Set(ByVal value As SortDetails())
                If Not value Is Nothing Then
                    _SortValues = value.ToList()
                Else
                    _SortValues = New List(Of SortDetails)
                End If
            End Set
        End Property


        ''' <summary>
        ''' Legacy support for backward compatibilty only.  Use new FilterValues collection instead
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.2 on 08/08/2018
        '''   the previous version did not support multiple filters
        '''   so to support existing filter logic we use the first filter  name of the new 
        '''   FilterValues list as the default filter name when only one filter is being 
        '''   implemented
        ''' </remarks>
        Public Property filterName() As String
            Get
                If _FilterValues Is Nothing OrElse _FilterValues.Count() < 1 Then
                    Return ""
                Else
                    Return _FilterValues(0).filterName
                End If
            End Get
            Set(ByVal value As String)
                If _FilterValues Is Nothing Then _FilterValues = New List(Of FilterDetails)
                If _FilterValues.Count() < 1 Then _FilterValues.Add(New FilterDetails())
                FilterValues(0).filterName = value
            End Set
        End Property

        ''' <summary>
        ''' Legacy support for backward compatibilty only.  Use new FilterValues collection instead
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.2 on 08/08/2018
        '''   the previous version did not support multiple filters
        '''   and it did not not support a filter from and a filter to for string filters
        '''   so to support existing filter logic we use the filterValueFrom of the new 
        '''   FilterValues list as the default filter value when only one filter is being 
        '''   implemented
        ''' </remarks>
        Public Property filterValue() As String
            Get
                If _FilterValues Is Nothing OrElse _FilterValues.Count() < 1 Then
                    Return ""
                Else
                    Return _FilterValues(0).filterValueFrom
                End If
            End Get
            Set(ByVal value As String)
                If _FilterValues Is Nothing Then _FilterValues = New List(Of FilterDetails)
                If _FilterValues.Count() < 1 Then _FilterValues.Add(New FilterDetails())
                FilterValues(0).filterValueFrom = value
            End Set
        End Property

        ''' <summary>
        ''' Legacy support for backward compatibilty only.  Use new FilterValues collection instead
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.2 on 08/08/2018
        '''   the previous version did not support multiple filters
        '''   so to support existing filter logic we use the filterFrom propery of the new 
        '''   FilterValues list as the default filter from value when only one filter is being 
        '''   implemented
        ''' </remarks>
        Public Property filterFrom() As Date?
            Get
                If _FilterValues Is Nothing OrElse _FilterValues.Count() < 1 Then
                    Return Nothing
                Else
                    Return _FilterValues(0).filterFrom
                End If
            End Get
            Set(ByVal value As Date?)
                If _FilterValues Is Nothing Then _FilterValues = New List(Of FilterDetails)
                If _FilterValues.Count() < 1 Then _FilterValues.Add(New FilterDetails())
                FilterValues(0).filterFrom = value
            End Set
        End Property

        ''' <summary>
        ''' Legacy support for backward compatibilty only.  Use new FilterValues collection instead
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.2 on 08/08/2018
        '''   the previous version did not support multiple filters
        '''   so to support existing filter logic we use the filterTo propery of the new 
        '''   FilterValues list as the default filter to value when only one filter is being 
        '''   implemented
        ''' </remarks>
        Public Property filterTo() As Date?
            Get
                If _FilterValues Is Nothing OrElse _FilterValues.Count() < 1 Then
                    Return Nothing
                Else
                    Return _FilterValues(0).filterTo
                End If
            End Get
            Set(ByVal value As Date?)
                If _FilterValues Is Nothing Then _FilterValues = New List(Of FilterDetails)
                If _FilterValues.Count() < 1 Then _FilterValues.Add(New FilterDetails())
                FilterValues(0).filterTo = value
            End Set
        End Property

        ''' <summary>
        ''' Legacy support for backward compatibilty only.  Use new SortValues collection instead
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.2 on 08/08/2018
        '''   the previous version did not support multiple sorting values
        '''   so to support existing sort logic we use the first sort name of the new 
        '''   SortValues list as the default sort name when only one sorting option is being 
        '''   implemented
        ''' </remarks>
        Public Property sortName() As String
            Get
                If _SortValues Is Nothing OrElse _SortValues.Count() < 1 Then
                    Return ""
                Else
                    Return _SortValues(0).sortName
                End If
            End Get
            Set(ByVal value As String)
                If _SortValues Is Nothing Then _SortValues = New List(Of SortDetails)
                If _SortValues.Count() < 1 Then _SortValues.Add(New SortDetails())
                SortValues(0).sortName = value
            End Set
        End Property

        ''' <summary>
        ''' Legacy support for backward compatibilty only.  Use new SortValues collection instead
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.2 on 08/08/2018
        '''   the previous version did not support multiple sorting values
        '''   so to support existing sort logic we use the first sort direction of the new 
        '''   SortValues list as the default sort direction when only one sorting option is being 
        '''   implemented
        ''' </remarks>
        Public Property sortDirection() As String
            Get
                If _SortValues Is Nothing OrElse _SortValues.Count() < 1 Then
                    Return ""
                Else
                    Return _SortValues(0).sortDirection
                End If
            End Get
            Set(ByVal value As String)
                If _SortValues Is Nothing Then _SortValues = New List(Of SortDetails)
                If _SortValues.Count() < 1 Then _SortValues.Add(New SortDetails())
                SortValues(0).sortDirection = value
            End Set
        End Property

        Public Property page() As Integer
            Get
                Return _page
            End Get
            Set(ByVal value As Integer)
                _page = value
            End Set
        End Property

        Public Property pageSize() As Integer
            Get
                Return _pageSize
            End Get
            Set(ByVal value As Integer)
                _pageSize = value
            End Set
        End Property

        Public Property skip() As Integer
            Get
                Return _skip
            End Get
            Set(ByVal value As Integer)
                _skip = value
            End Set
        End Property

        Public Property take() As Integer
            Get
                Return _take
            End Get
            Set(ByVal value As Integer)
                _take = value
            End Set
        End Property

        Public Property CarrierControlFrom() As Integer
            Get
                Return _CarrierControlFrom
            End Get
            Set(ByVal value As Integer)
                _CarrierControlFrom = value
            End Set
        End Property

        Public Property CarrierControlTo() As Integer
            Get
                Return _CarrierControlTo
            End Get
            Set(ByVal value As Integer)
                _CarrierControlTo = value
            End Set
        End Property

        Public Property CarrierNumberFrom() As Integer
            Get
                Return _CarrierNumberFrom
            End Get
            Set(ByVal value As Integer)
                _CarrierNumberFrom = value
            End Set
        End Property

        Public Property CarrierNumberTo() As Integer
            Get
                Return _CarrierNumberTo
            End Get
            Set(ByVal value As Integer)
                _CarrierNumberTo = value
            End Set
        End Property

        Public Property CompControlFrom() As Integer
            Get
                Return _CompControlFrom
            End Get
            Set(ByVal value As Integer)
                _CompControlFrom = value
            End Set
        End Property

        Public Property CompControlTo() As Integer
            Get
                Return _CompControlTo
            End Get
            Set(ByVal value As Integer)
                _CompControlTo = value
            End Set
        End Property

        Public Property CompNumberFrom() As Integer
            Get
                Return _CompNumberFrom
            End Get
            Set(ByVal value As Integer)
                _CompNumberFrom = value
            End Set
        End Property

        Public Property CompNumberTo() As Integer
            Get
                Return _CompNumberTo
            End Get
            Set(ByVal value As Integer)
                _CompNumberTo = value
            End Set
        End Property

        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property

        Public Property LaneControl() As Integer
            Get
                Return _LaneControl
            End Get
            Set(ByVal value As Integer)
                _LaneControl = value
            End Set
        End Property

        Public Property LEAdminControl() As Integer
            Get
                Return _LEAdminControl
            End Get
            Set(ByVal value As Integer)
                _LEAdminControl = value
            End Set
        End Property

        ''' <summary>
        ''' Data String property can contain a string version of the data in different formats depedent upon the control class requirements
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.1 on 02/21/2018
        ''' </remarks>
        Public Property Data() As String
            Get
                Return _Data
            End Get
            Set(ByVal value As String)
                _Data = value
            End Set
        End Property

        Public Property ParentControl() As Integer
            Get
                Return _ParentControl
            End Get
            Set(ByVal value As Integer)
                _ParentControl = value
            End Set
        End Property

        Public Property ApptControl() As Integer
            Get
                Return _ApptControl
            End Get
            Set(ByVal value As Integer)
                _ApptControl = value
            End Set
        End Property

        Public Property NatActNumber() As Integer
            Get
                Return _NatActNumber
            End Get
            Set(ByVal value As Integer)
                _NatActNumber = value
            End Set
        End Property

        Public Property ContactControl() As Integer
            Get
                Return _ContactControl
            End Get
            Set(ByVal value As Integer)
                _ContactControl = value
            End Set
        End Property

        ''' <summary>
        ''' A List of field names for grouping
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.3.0.001 on 08/12/2020
        '''     A list of field name used to group grids
        '''     datasource must sort by group fields to ensure all relevant data is returned
        ''' </remarks>
        Public Property Groups() As List(Of String)
            Get
                If _Groups Is Nothing Then _Groups = New List(Of String)
                Return _Groups
            End Get
            Set(ByVal value As List(Of String))
                If value Is Nothing Then value = New List(Of String)
                _Groups = value
            End Set
        End Property

        ' Modified by RHR for v-8.5.1.002 on 04/09/2022
        Public Property PrimaryKey() As Integer
            Get
                Return _PrimaryKey
            End Get
            Set(ByVal value As Integer)
                _PrimaryKey = value
            End Set
        End Property

        ''' <summary>
        ''' Test if using recordcontrol or parent control to select data, 
        ''' updates the filterWhere and sFilterSpacer if using the ParentControl.
        ''' The caller must validate that at leaste one iRecordControl or iParentControl 
        ''' has a non zero value or the filter may not be valid.  
        ''' </summary>
        ''' <param name="sRecordControlName"></param>
        ''' <param name="sParentControlName"></param>
        ''' <param name="iRecordControl"></param>
        ''' <param name="iParentControl"></param>
        ''' <param name="filterWhere"></param>
        ''' <param name="sFilterSpacer"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.2 on 01/23/2019
        '''   check for a record specific control filter using sRecordControlName
        '''   if not found test the ParentControl 
        '''   if not valid return false 
        '''   caller must handle any errors on false
        ''' </remarks>
        Public Function addParentFilterIfNoRecordControlFilter(ByVal sRecordControlName As String, ByVal sParentControlName As String, ByRef iRecordControl As Integer, ByRef iParentControl As Integer, ByRef filterWhere As String, ByRef sFilterSpacer As String) As Boolean
            Dim blnRet As Boolean = True

            If (FilterValues Is Nothing OrElse FilterValues.Count() < 1) OrElse (Not FilterValues.Any(Function(x) x.filterName = sRecordControlName)) Then
                'The Record Control Filter does not exist so use the parent control fliter
                If ParentControl = 0 Then Return False
                filterWhere = " (" & sParentControlName & " = " & ParentControl & ") "
                sFilterSpacer = " And "
                iParentControl = ParentControl
            Else
                Dim tFilter As Models.FilterDetails = FilterValues.Where(Function(x) x.filterName = sRecordControlName).FirstOrDefault()
                Integer.TryParse(tFilter.filterValueFrom, iRecordControl)
            End If
            Return blnRet


        End Function

        ''' <summary>
        ''' Create or Add sorting to the All Filters Sort Details array
        ''' </summary>
        ''' <param name="sortField"></param>
        ''' <param name="blnSortAscending"></param>
        ''' <remarks>
        ''' Created by RHR for v-8.5.3.001 on 05/31/2022 
        ''' </remarks>
        Public Sub addToSort(ByVal sortField As String, ByVal blnSortAscending As Boolean)
            Dim sDirection As String = "Asc"

            If Not blnSortAscending Then
                sDirection = "Desc"
            End If
            If (Me.SortValues?.Length < 1 OrElse String.IsNullOrWhiteSpace(Me.sortDirection)) Then
                Me.SortValues = New SortDetails(0) {New SortDetails() With {.sortName = sortField, .sortDirection = sDirection}}
            Else
                Dim list = Me.SortValues.ToList()
                list.Add(New SortDetails With {
                    .sortName = sortField,
                    .sortDirection = sDirection
                })
                Me.SortValues = list.ToArray()
            End If
        End Sub

        ''' <summary>
        ''' Add a detail to the private _FilterValues list without recreating the array
        '''
        ''' </summary>
        ''' <param name="oDetail"></param>
        ''' <remarks>
        ''' Created by RHR for v-8.5.3.007 on 01/19/2023 new ablity to add to the private _FilterValues list data 
        ''' Note: the public FilterValues property is basiclly a read only array generated
        ''' from the private list
        ''' Modified by RHR for v-8.5.3.007 on 03/09/2023 added flag to replace instead of add
        ''' </remarks>
        Public Sub addFilterDetail(ByVal oDetail As FilterDetails, Optional ByVal blnReplace As Boolean = False)
            Dim blnAdd As Boolean = True
            If blnReplace AndAlso Not Me._FilterValues Is Nothing AndAlso Me._FilterValues.Count > 0 Then
                For x = 0 To Me._FilterValues.Count - 1
                    Dim oldDetail As FilterDetails = Me._FilterValues(x)
                    If oldDetail.filterName = oDetail.filterName Then
                        Me._FilterValues(x) = oDetail
                        blnAdd = False
                        Exit For 'just replace the first one found
                    End If
                Next
            End If
            If blnAdd Then
                Me._FilterValues.Add(oDetail)
            End If
        End Sub


    End Class


End Namespace

