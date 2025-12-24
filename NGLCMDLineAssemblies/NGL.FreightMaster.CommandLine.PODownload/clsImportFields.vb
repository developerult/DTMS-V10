Public Class clsImportFields
    Implements System.Collections.IEnumerable
    'local variable to hold collection
    Private mCol As Collection

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return mCol.GetEnumerator
    End Function

    Public Function Add( _
        ByVal Key As String, _
        ByVal Name As String, _
        ByVal DataType As clsImportField.DataTypeID, _
        ByVal Length As Integer, _
        ByVal Null As Boolean) As clsImportField
        'create a new object
        Dim objNewMember As clsImportField
        objNewMember = New clsImportField
        With objNewMember
            .Name = Name
            .DataType = DataType
            .Length = Length
            .Null = Null
            .Key = Key
        End With
        If Key.Trim.Length = 0 Then
            mCol.Add(objNewMember)
        Else
            mCol.Add(objNewMember, Key)
        End If
        'return the object created
        Add = objNewMember

        objNewMember = Nothing


    End Function

    Public Function Add( _
        ByVal Key As String, _
        ByVal Name As String, _
        ByVal DataType As clsImportField.DataTypeID, _
        ByVal Length As Integer, _
        ByVal Null As Boolean, _
        ByVal PK As clsImportField.PKValue) As clsImportField
        'create a new object
        Dim objNewMember As New clsImportField
        With objNewMember
            .Name = Name
            .DataType = DataType
            .Length = Length
            .Null = Null
            .Key = Key
            .PK = PK
            If Key.Trim.Length = 0 Then
                mCol.Add(objNewMember)
            Else
                mCol.Add(objNewMember, Key)
            End If
        End With

        'return the object created
        Add = objNewMember

        objNewMember = Nothing


    End Function

    Public Function Add( _
        ByVal Key As String, _
        ByVal Name As String, _
        ByVal DataType As clsImportField.DataTypeID, _
        ByVal Length As Integer, _
        ByVal Null As Boolean, _
        ByVal PK As clsImportField.PKValue, _
        ByVal FK_Index As Integer, _
        ByVal FK_Key As String, _
        ByVal Parent_Field As String) As clsImportField
        'create a new object
        Dim objNewMember As New clsImportField
        With objNewMember
            .Name = Name
            .DataType = DataType
            .Length = Length
            .Null = Null
            .Key = Key
            .PK = PK
            .FK_Index = FK_Index
            .FK_Key = FK_Key
            .Parent_Field = Parent_Field
            If Key.Trim.Length = 0 Then
                mCol.Add(objNewMember)
            Else
                mCol.Add(objNewMember, Key)
            End If
        End With

        'return the object created
        Add = objNewMember

        objNewMember = Nothing


    End Function


    Default Public ReadOnly Property Item(ByVal vntIndexKey As Object) As clsImportField
        Get
            'used when referencing an element in the collection
            'vntIndexKey contains either the Index or Key to the collection,
            'this is why it is declared as a Variant
            'Syntax: Set foo = x.Item(xyz) or Set foo = x.Item(5)
            Item = mCol.Item(vntIndexKey)
        End Get
    End Property



    Public ReadOnly Property Count() As Integer
        Get
            'used when retrieving the number of elements in the
            'collection. Syntax: Debug.Print x.Count
            Count = mCol.Count()
        End Get
    End Property

    Public Sub Remove(ByRef vntIndexKey As Object)
        'used when removing an element from the collection
        'vntIndexKey contains either the Index or Key, which is why
        'it is declared as a Variant
        'Syntax: x.Remove(xyz)


        mCol.Remove(vntIndexKey)
    End Sub

    Public Sub New()
        MyBase.New()
        mCol = New Collection
    End Sub

    Protected Overrides Sub Finalize()
        mCol = Nothing
        MyBase.Finalize()
    End Sub
End Class
