Public Class ExistingLanes : Inherits List(Of ExistingLane)
    Public Sub AddNew(ByVal strLaneNumber As String, ByVal intLaneControl As Integer)

        Dim oLane As ExistingLane
        Dim blnAddNew As Boolean = False

#Disable Warning BC42030 ' Variable 'oLane' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
        If Not fillLaneByNumber(strLaneNumber, oLane) Then
#Enable Warning BC42030 ' Variable 'oLane' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            If Not fillLaneByControl(intLaneControl, oLane) Then
                blnAddNew = True
            End If
        End If
        If blnAddNew OrElse oLane Is Nothing Then
            oLane = New ExistingLane
            If Not String.IsNullOrEmpty(strLaneNumber) Then oLane.LaneNumber = strLaneNumber
            oLane.LaneControl = intLaneControl
            Me.Add(oLane)
        Else
            'only update values if they are not empty or zero
            If Not String.IsNullOrEmpty(strLaneNumber) Then oLane.LaneNumber = strLaneNumber
            If intLaneControl > 0 Then oLane.LaneControl = intLaneControl
        End If
    End Sub

    Private Function fillLaneByNumber(ByVal strLaneNumber As String, ByRef oLane As ExistingLane) As Boolean
        Dim blnRet As Boolean = False
        If Not String.IsNullOrEmpty(strLaneNumber) Then
            Dim query = From q In Me Where q.LaneNumber = strLaneNumber
            If query.count > 0 Then
                oLane = TryCast(query(0), ExistingLane)
                If Not oLane Is Nothing Then blnRet = True
            End If
        End If
        Return blnRet
    End Function

    Private Function fillLaneByControl(ByVal intLaneControl As Integer, ByRef oLane As ExistingLane) As Boolean
        Dim blnRet As Boolean = False
        If intLaneControl > 0 Then
            Dim query = From q In Me Where q.LaneControl = intLaneControl
            If query.count > 0 Then
                oLane = TryCast(query(0), ExistingLane)
                If Not oLane Is Nothing Then blnRet = True
            End If
        End If
        Return blnRet
    End Function


    Public Function getControlByNumber(ByVal strLaneNumber As String) As Integer
        Dim intRet As Integer = 0
        If String.IsNullOrEmpty(strLaneNumber) Then Return 0
        Dim query = From q In Me Where q.LaneNumber = strLaneNumber
        If query.Count > 0 Then
            Dim oLane As ExistingLane = TryCast(query(0), ExistingLane)
            If Not oLane Is Nothing Then intRet = oLane.LaneControl
        End If
        Return intRet
    End Function

    Public Function getNumberByControl(ByVal intControl As Integer) As String
        Dim strRet As String = ""
        Dim query = From q In Me Where q.LaneControl = intControl
        If query.Count > 0 Then
            Dim oLane As ExistingLane = TryCast(query(0), ExistingLane)
            If Not oLane Is Nothing Then strRet = oLane.LaneNumber
        End If
        Return strRet
    End Function
End Class

Public Class ExistingLane
    Public LaneControl As Integer
    Public LaneNumber As String
End Class
