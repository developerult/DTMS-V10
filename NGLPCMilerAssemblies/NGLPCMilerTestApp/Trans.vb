Imports System.Reflection

Public Class Trans


    ''' <summary>
    ''' Used to perform a copy of named properties from one object to another.  
    ''' The objects may be different types but the names of each property must be the same and they must have similar data types.  
    ''' Some conversion of nullable properties to non nullable data types is completed when possible.
    ''' </summary>
    ''' <param name="toObj"></param>
    ''' <param name="fromObj"></param>
    ''' <param name="skipObjs">
    ''' a list of property names to ignore during the compy process
    ''' </param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created for version 7.0.5.0 by RHR all copies of this logic in other libraries should be modified to call this core method as soon as possible.
    ''' </remarks>
    Public Shared Function CopyMatchingFields(toObj As [Object], fromObj As [Object], ByVal skipObjs As List(Of String), Optional ByRef strMsg As String = "") As Object
        If toObj Is Nothing Or fromObj Is Nothing Then
            Return Nothing
        End If

        Dim fromType As Type = fromObj.[GetType]()
        Dim toType As Type = toObj.[GetType]()

        ' Get all FieldInfo. 
        Dim fProps As PropertyInfo() = fromType.GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)
        Dim tProps As PropertyInfo() = toType.GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)
        For Each fProp As PropertyInfo In fProps
            Dim propValue As Object = fProp.GetValue(fromObj, Nothing)

            If skipObjs Is Nothing OrElse Not skipObjs.Contains(fProp.Name) Then
                For Each tProp In tProps
                    If tProp.Name = fProp.Name Then
                        transformProperty(tProp, fProp, propValue, toObj, strMsg)
                        Exit For 'we have a match
                    End If
                Next
            End If
            'End If
        Next
        Return toObj

    End Function

    Private Shared Sub transformProperty(ByRef tProp As PropertyInfo, ByRef fProp As PropertyInfo, ByRef propValue As Object, ByRef toObj As Object, ByRef strMsg As String)
        If tProp.CanWrite Then 'check for read only properties and skip them
            Dim sGetPropValMsg As String = ""
            Dim newPropVal As Object = getPropertyValue(tProp, fProp, propValue, sGetPropValMsg)
            If Not String.IsNullOrEmpty(sGetPropValMsg) Then
                strMsg &= sGetPropValMsg
                Return
            End If
            tProp.SetValue(toObj, newPropVal, Nothing)
        Else
            If Debugger.IsAttached Then
                strMsg &= " Debug Only Message: transformPoperty '" & tProp.Name & "' is read only."
            End If
        End If
    End Sub


    Private Shared Function getPropertyValue(ByRef tProp As PropertyInfo, ByRef fProp As PropertyInfo, ByRef propValue As Object, ByRef strMsg As String) As Object
        Dim newPropValue As Object = Nothing
        'primitives used for casting
        Dim iVal16 As Int16 = 0
        Dim iVal32 As Int32 = 0
        Dim iVal64 As Int64 = 0
        Dim dblVal As Double = 0
        Dim decVal As Decimal = 0
        Dim dtVal As Date = Date.Now()
        Dim blnVal As Boolean = False
        Dim intVal As Integer = 0


        If tProp.PropertyType().Equals(fProp.PropertyType()) Then
            newPropValue = propValue
        Else
            Dim sfPropName = fProp.PropertyType.Name
            Dim strPropValue As String = ""
            If Not propValue Is Nothing Then strPropValue = propValue.ToString()
            Dim stPropName = tProp.PropertyType.Name
            If stPropName.Substring(0, 4).ToUpper = "NULL" Then
                'this is a nullable data type check which type
                If tProp.PropertyType.FullName.Contains("Int16") Then
                    stPropName = "Int16"
                ElseIf tProp.PropertyType.FullName.Contains("Int32") Then
                    stPropName = "Int32"
                ElseIf tProp.PropertyType.FullName.Contains("Int64") Then
                    stPropName = "Int64"
                ElseIf tProp.PropertyType.FullName.Contains("Date") Then
                    stPropName = "Date"
                ElseIf tProp.PropertyType.FullName.Contains("Decimal") Then
                    stPropName = "Decimal"
                ElseIf tProp.PropertyType.FullName.Contains("Double") Then
                    stPropName = "Double"
                ElseIf tProp.PropertyType.FullName.Contains("Boolean") Then
                    stPropName = "Boolean"
                End If
            End If
            Try
                Select Case stPropName
                    Case "String"
                        newPropValue = strPropValue
                    Case "Int16"
                        If Not Int16.TryParse(strPropValue, iVal16) Then iVal16 = 0
                        newPropValue = iVal16
                    Case "Int32"
                        If Not Int32.TryParse(strPropValue, iVal32) Then iVal32 = 0
                        newPropValue = iVal32
                    Case "Int64"
                        If Not Int32.TryParse(strPropValue, iVal64) Then iVal64 = 0
                        newPropValue = iVal64
                    Case "Date"
                        If Not Date.TryParse(strPropValue, dtVal) Then dtVal = Date.MinValue
                        newPropValue = dtVal
                    Case "DateTime"
                        If Not Date.TryParse(strPropValue, dtVal) Then dtVal = Date.MinValue
                        newPropValue = dtVal
                    Case "Decimal"
                        If Not Decimal.TryParse(strPropValue, decVal) Then decVal = 0
                        newPropValue = decVal
                    Case "Double"
                        If Not Double.TryParse(strPropValue, dblVal) Then dblVal = 0
                        newPropValue = dblVal
                    Case "Boolean"
                        If Boolean.TryParse(strPropValue, blnVal) Then
                            newPropValue = blnVal
                        Else
                            'try to convert to an integer and then test for 0 any non zero is true
                            If Integer.TryParse(strPropValue, intVal) Then
                                If intVal = 0 Then
                                    blnVal = False
                                Else
                                    blnVal = True
                                End If
                            Else
                                blnVal = False
                            End If
                            newPropValue = blnVal
                        End If
                    Case Else
                        'cannot parse
                        Dim s As String = ""
                        If propValue IsNot Nothing Then s = propValue.ToString
                        strMsg &= " Cannot Copy " & fProp.Name & " invalid type " & s
                End Select
            Catch ex As Exception
                strMsg &= ex.Message
                Throw
            End Try
        End If
        Return newPropValue
    End Function


End Class
