'Imports System
'Imports System.Collections.Generic
'Imports System.Data.Linq
'Imports System.Linq
'Imports System.Linq.Expressions

'Namespace Thuban.Data.Linq
'    Public NotInheritable Class TableView(Of TEntity As Class)
'        Implements IQueryable(Of TEntity)
'        Implements ITable
'        Private baseQuery As IQueryable(Of TEntity)
'        Private table As Table(Of TEntity)
'        Private predicate As Func(Of TEntity, Boolean)

'        Public Sub New(ByVal dataContext As DataContext, ByVal predicate As Expression(Of Func(Of TEntity, Boolean)))
'            Me.table = dataContext.GetTable(Of TEntity)()
'            Me.baseQuery = table.Where(predicate)
'            Me.predicate = predicate.Compile()
'        End Sub

'		#region IEnumerable<TEntity> Members

'        Public Function GetEnumerator() As IEnumerator(Of TEntity)
'            Return Me.baseQuery.GetEnumerator()
'        End Function
'#End Region

'		#region IEnumerable Members

'        Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
'            Return Me.GetEnumerator()
'        End Function
'#End Region

'		#region PermissionChecks

'        Friend Sub PermissionCheck(ByVal action As Action(Of TEntity), ByVal entity As TEntity)
'            If predicate(entity) Then
'                action(entity)
'            Else
'                Throw New InvalidOperationException("No permission")
'            End If
'        End Sub

'        Friend Sub PermissionCheck(ByVal action As Action(Of TEntity, TEntity), ByVal entity1 As TEntity, ByVal entity2 As TEntity)
'            If predicate(entity1) AndAlso predicate(entity2) Then
'                action(entity1, entity2)
'            Else
'                Throw New InvalidOperationException("No permission")
'            End If
'        End Sub

'        Friend Function PermissionCheck(Of TResult)(ByVal [function] As Func(Of TEntity, TResult), ByVal entity As TEntity) As TResult
'            If predicate(entity) Then
'                Return [function](entity)
'            Else
'                Throw New InvalidOperationException("No permission")
'            End If
'        End Function

'        Friend Sub PermissionCheck(ByVal action As Action(Of IEnumerable(Of TEntity)), ByVal entities As IEnumerable(Of TEntity))
'            If entities.All(predicate) Then
'                action(entities)
'            Else
'                Throw New InvalidOperationException("No permission")
'            End If
'        End Sub
'#End Region

'		#region FilteredTable<TEntity> Members

'        Sub Attach(ByVal entity As TEntity, ByVal original As TEntity)
'			PermissionCheck(Function(x As , y As ) table.Attach(x, y), entity, original)
'        End Sub

'        Sub Attach(ByVal entity As TEntity, ByVal asModified As Boolean)
'			PermissionCheck(Function(x As ) table.Attach(x, asModified), entity)
'        End Sub

'        Sub Attach(ByVal entity As TEntity)
'			PermissionCheck(Function(x As ) table.Attach(x), entity)
'        End Sub

'        Sub AttachAll(ByVal entities As IEnumerable(Of TEntity), ByVal asModified As Boolean)
'			PermissionCheck(Function(x As ) table.AttachAll(x, asModified), entities)
'        End Sub

'        Sub AttachAll(ByVal entities As IEnumerable(Of TEntity))
'			PermissionCheck(Function(x As ) table.AttachAll(x), entities)
'        End Sub

'        Sub DeleteAllOnSubmit(ByVal entities As IEnumerable(Of TEntity))
'			PermissionCheck(Function(x As ) table.DeleteAllOnSubmit(x), entities)
'        End Sub

'        Sub DeleteOnSubmit(ByVal entity As TEntity)
'			PermissionCheck(Function(x As ) table.DeleteOnSubmit(x), entity)
'        End Sub

'        Function GetModifiedMembers(ByVal entity As TEntity) As ModifiedMemberInfo()
'			Return PermissionCheck(Function(x As ) table.GetModifiedMembers(x), entity)
'        End Function

'        Function GetOriginalEntityState(ByVal entity As TEntity) As TEntity
'			Return PermissionCheck(Function(x As ) table.GetOriginalEntityState(x), entity)
'        End Function

'        Sub InsertAllOnSubmit(ByVal entities As IEnumerable(Of TEntity))
'			PermissionCheck(Function(x As ) table.InsertAllOnSubmit(x), entities)
'        End Sub

'        Sub InsertOnSubmit(ByVal entity As TEntity)
'			PermissionCheck(Function(x As ) table.InsertOnSubmit(x), entity)
'        End Sub
'#End Region

'		#region ITable Members

'        Sub Attach(ByVal entity As Object, ByVal original As Object) Implements ITable.Attach
'            Me.Attach(DirectCast(entity, TEntity), DirectCast(original, TEntity))
'        End Sub

'        Sub Attach(ByVal entity As Object, ByVal asModified As Boolean) Implements ITable.Attach
'            Me.Attach(DirectCast(entity, TEntity), asModified)
'        End Sub

'        Sub Attach(ByVal entity As Object) Implements ITable.Attach
'            Me.Attach(DirectCast(entity, TEntity))
'        End Sub

'        Sub AttachAll(ByVal entities As System.Collections.IEnumerable, ByVal asModified As Boolean) Implements ITable.AttachAll
'            Me.AttachAll(entities.Cast(Of TEntity)(), asModified)
'        End Sub

'        Sub AttachAll(ByVal entities As System.Collections.IEnumerable) Implements ITable.AttachAll
'            Me.AttachAll(entities.Cast(Of TEntity)())
'        End Sub

'        ReadOnly Property Context() As DataContext Implements ITable.Context
'            Get
'                Throw New InvalidOperationException("Access to the underlying context is not allowed.")
'            End Get
'        End Property

'        Sub DeleteAllOnSubmit(ByVal entities As System.Collections.IEnumerable) Implements ITable.DeleteAllOnSubmit
'            Me.DeleteAllOnSubmit(entities.Cast(Of TEntity)())
'        End Sub

'        Sub DeleteOnSubmit(ByVal entity As Object) Implements ITable.DeleteOnSubmit
'            Me.DeleteOnSubmit(DirectCast(entity, TEntity))
'        End Sub

'        Function GetModifiedMembers(ByVal entity As Object) As ModifiedMemberInfo() Implements ITable.GetModifiedMembers
'            Return Me.GetModifiedMembers(DirectCast(entity, TEntity))
'        End Function

'        Function GetOriginalEntityState(ByVal entity As Object) As Object Implements ITable.GetOriginalEntityState
'            Return Me.GetOriginalEntityState(DirectCast(entity, TEntity))
'        End Function

'        Sub InsertAllOnSubmit(ByVal entities As System.Collections.IEnumerable) Implements ITable.InsertAllOnSubmit
'            Me.InsertAllOnSubmit(entities.Cast(Of TEntity)())
'        End Sub

'        Sub InsertOnSubmit(ByVal entity As Object) Implements ITable.InsertOnSubmit
'            Me.InsertOnSubmit(DirectCast(entity, TEntity))
'        End Sub

'        ReadOnly Property IsReadOnly() As Boolean Implements ITable.IsReadOnly
'            Get
'                Return table.IsReadOnly
'            End Get
'        End Property
'#End Region

'		#region IQueryable Members

'        ReadOnly Property ElementType() As Type Implements IQueryable.ElementType
'            Get
'                Return baseQuery.ElementType
'            End Get
'        End Property

'        ReadOnly Property Expression() As Expression Implements IQueryable.Expression
'            Get
'                Return baseQuery.Expression
'            End Get
'        End Property

'        ReadOnly Property Provider() As IQueryProvider Implements IQueryable.Provider
'            Get
'                Return baseQuery.Provider
'            End Get
'        End Property
'#End Region
'    End Class
'End Namespace
