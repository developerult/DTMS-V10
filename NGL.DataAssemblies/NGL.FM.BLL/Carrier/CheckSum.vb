Public Class CheckSum

    Public Enum CheckDigitAlgorithm
        None = 1
        Mod7
        Mod10
        Mod11
        Mod9
        Batch
    End Enum


    ''' <summary>
    ''' Returns a string value representing the check digit value typically appended to the end of the carrier assigned pro number.
    ''' Caller must catch all exceptions and test the return value for the specific Error Code (default = "E").  Error Code management
    ''' may be based on individual pro number requirements.
    ''' </summary>
    ''' <param name="enmCheckType"></param>
    ''' <param name="strSeed"></param>
    ''' <param name="strWeight"></param>
    ''' <param name="blnUseIndexForWeightFactor"></param>
    ''' <param name="intIndexForWeightFactorMin"></param>
    ''' <param name="CheckDigitErrorCode"></param>
    ''' <param name="CheckDigit10Code"></param>
    ''' <param name="CheckDigitOver10Code"></param>
    ''' <param name="CheckDigitZeroCode"></param>
    ''' <param name="splitDigits"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCheckDigit(ByVal enmCheckType As CheckDigitAlgorithm, _
                                   ByVal strSeed As String, _
                                   Optional ByVal strWeight As String = "", _
                                   Optional ByVal blnUseIndexForWeightFactor As Boolean = False, _
                                   Optional ByVal intIndexForWeightFactorMin As Integer = 2, _
                                   Optional ByVal CheckDigitErrorCode As String = "E", _
                                   Optional ByVal CheckDigit10Code As String = "0", _
                                   Optional ByVal CheckDigitOver10Code As String = "0", _
                                   Optional ByVal CheckDigitZeroCode As String = "0", _
                                   Optional ByVal splitDigits As Boolean = False, _
                                   Optional ByVal blnCheckDigitUseSubtractionFactor As Boolean = False, _
                                   Optional ByVal intCheckDigitSubtractionFactor As Integer = 0) As String


        Dim strCheckDigit = CheckDigitErrorCode
        If String.IsNullOrWhiteSpace(CheckDigitZeroCode) Then CheckDigitZeroCode = "0"
        If String.IsNullOrWhiteSpace(CheckDigitErrorCode) Then CheckDigitErrorCode = "E"
        If String.IsNullOrWhiteSpace(CheckDigit10Code) Then CheckDigit10Code = "0"
        If String.IsNullOrWhiteSpace(CheckDigitOver10Code) Then CheckDigitOver10Code = "0"
        If enmCheckType = CheckDigitAlgorithm.None Then Return ""
        If strSeed.Trim.Length < 1 Then Return CheckDigitErrorCode
        Dim intSeed As Long = CLng(Val(strSeed))
        If intSeed < 1 Then Return CheckDigitErrorCode
        Dim intCheckDigit As Integer = 0
        If Not blnUseIndexForWeightFactor And strWeight.Trim.Length < 1 Then
            Select Case enmCheckType
                Case CheckDigitAlgorithm.Mod10
                    intCheckDigit = CLng(Val(strSeed) Mod 10)
                    If blnCheckDigitUseSubtractionFactor And intCheckDigitSubtractionFactor >= 10 Then
                        intCheckDigit = intCheckDigitSubtractionFactor - intCheckDigit
                    End If
                Case CheckDigitAlgorithm.Mod11
                    intCheckDigit = CLng(Val(strSeed) Mod 11)
                    If blnCheckDigitUseSubtractionFactor And intCheckDigitSubtractionFactor >= 11 Then
                        intCheckDigit = intCheckDigitSubtractionFactor - intCheckDigit
                    End If
                Case CheckDigitAlgorithm.Mod7
                    intCheckDigit = CLng(Val(strSeed) Mod 7)
                    If blnCheckDigitUseSubtractionFactor And intCheckDigitSubtractionFactor >= 7 Then
                        intCheckDigit = intCheckDigitSubtractionFactor - intCheckDigit
                    End If
                Case CheckDigitAlgorithm.Mod9
                    intCheckDigit = CLng(Val(strSeed) Mod 9)
                    If blnCheckDigitUseSubtractionFactor And intCheckDigitSubtractionFactor >= 9 Then
                        intCheckDigit = intCheckDigitSubtractionFactor - intCheckDigit
                    End If
                Case Else
                    Return CheckDigitErrorCode
            End Select
        ElseIf blnUseIndexForWeightFactor Then
            Dim sa() = strWeight.Select(Function(x) CInt(Val(x))).ToArray()
            Select Case enmCheckType
                Case CheckDigitAlgorithm.Mod10
                    intCheckDigit = CInt(SumProductUsingIndexAsFactor(sa, intIndexForWeightFactorMin, splitDigits) Mod 10)
                    If blnCheckDigitUseSubtractionFactor And intCheckDigitSubtractionFactor >= 10 Then
                        intCheckDigit = intCheckDigitSubtractionFactor - intCheckDigit
                    End If
                Case CheckDigitAlgorithm.Mod11
                    intCheckDigit = CInt(SumProductUsingIndexAsFactor(sa, intIndexForWeightFactorMin, splitDigits) Mod 11)
                    If blnCheckDigitUseSubtractionFactor And intCheckDigitSubtractionFactor >= 11 Then
                        intCheckDigit = intCheckDigitSubtractionFactor - intCheckDigit
                    End If
                Case CheckDigitAlgorithm.Mod9
                    intCheckDigit = CInt(SumProductUsingIndexAsFactor(sa, intIndexForWeightFactorMin, splitDigits) Mod 9)
                    If blnCheckDigitUseSubtractionFactor And intCheckDigitSubtractionFactor >= 9 Then
                        intCheckDigit = intCheckDigitSubtractionFactor - intCheckDigit
                    End If
                Case Else
                    Return CheckDigitErrorCode
            End Select
        ElseIf strWeight.Trim.Length > 0 Then
            If strSeed.Length <> strWeight.Length Then Return CheckDigitErrorCode

            Dim sWeight() = strWeight.Select(Function(x) CInt(Val(x))).ToArray()
            Dim sSeed() = strSeed.Select(Function(x) CInt(Val(x))).ToArray()
            Select Case enmCheckType
                Case CheckDigitAlgorithm.Mod10
                    intCheckDigit = CInt(SumProductUsingWeight(sSeed, sWeight, splitDigits) Mod 10)
                    If blnCheckDigitUseSubtractionFactor And intCheckDigitSubtractionFactor >= 10 Then
                        intCheckDigit = intCheckDigitSubtractionFactor - intCheckDigit
                    End If
                Case CheckDigitAlgorithm.Mod11
                    intCheckDigit = CInt(SumProductUsingWeight(sSeed, sWeight, splitDigits) Mod 11)
                    If blnCheckDigitUseSubtractionFactor And intCheckDigitSubtractionFactor >= 11 Then
                        intCheckDigit = intCheckDigitSubtractionFactor - intCheckDigit
                    End If
                Case CheckDigitAlgorithm.Mod9
                    intCheckDigit = CInt(SumProductUsingWeight(sSeed, sWeight, splitDigits) Mod 9)
                    If blnCheckDigitUseSubtractionFactor And intCheckDigitSubtractionFactor >= 9 Then
                        intCheckDigit = intCheckDigitSubtractionFactor - intCheckDigit
                    End If
                Case Else
                    Return CheckDigitErrorCode
            End Select
        Else
            Return CheckDigitErrorCode
        End If
        If intCheckDigit < 0 Then
            Return CheckDigitErrorCode
        ElseIf intCheckDigit = 10 Then
            strCheckDigit = CheckDigit10Code
        ElseIf intCheckDigit > 10 Then
            strCheckDigit = CheckDigitOver10Code
        ElseIf intCheckDigit = 0 Then
            strCheckDigit = CheckDigitZeroCode
        Else
            strCheckDigit = intCheckDigit.ToString()
        End If

        Return strCheckDigit
    End Function

    ''' <summary>
    ''' Calculates the sum of the products for each value in seed multiplied by the corresponding index position in seed based 
    ''' on an index minimum value (default = 2) the index positions start at the far right position and increase by one 
    ''' for each item in the index so a number,013152447, with a min of 2 would be calculated like:
    ''' 10*0+9x1+8x3+7x1+6x5+5x2+4x4+3x4+2x7.  
    ''' If splitDigits is true (default is false) the system will add each digit to the sum individually rather than
    ''' as a product so 12 becomes total + 1 + 2 instead of total + 12
    ''' </summary>
    ''' <param name="seed"></param>
    ''' <param name="intIndexForWeightFactorMin"></param>
    ''' <param name="splitDigits"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SumProductUsingIndexAsFactor(ByVal seed() As Integer, _
                                   Optional ByVal intIndexForWeightFactorMin As Integer = 2, _
                                   Optional ByVal splitDigits As Boolean = False) As Long
        If seed Is Nothing OrElse seed.Count < 1 Then Return -1
        If intIndexForWeightFactorMin < 1 Then Return -1
        Dim lngRet As Long = -1
        Try

            For i As Integer = seed.Count + (intIndexForWeightFactorMin - 1) To intIndexForWeightFactorMin Step -1
                Dim thisSum As Integer = seed((seed.Count - i) + 1) * i
                If thisSum.ToString.Length > 1 And splitDigits Then
                    'we must split the string into different sum totals
                    Dim s() = thisSum.ToString().Select(Function(x) CInt(Val(x))).ToArray()
                    For Each t In s
                        lngRet += t
                    Next
                Else
                    lngRet += thisSum
                End If
            Next
        Catch ex As System.IndexOutOfRangeException
            Return -1
        Catch ex As Exception
            Throw
        End Try
        Return lngRet

        'Examples
        '+1x7
        '' Check Digit Not used +0x10 mod 11 = 132 mod 11 = 0
        'WeightFactorMin = 1 and Count = 1 so start index = count + 0 or 1

        '9x0
        '+8x1
        '+7x3
        '+6x1
        '+5x5
        '+4x2
        '+3x4
        '+2x4
        '+1x7
        '' Check Digit Not used +0x10 mod 11 = 132 mod 11 = 0
        'WeightFactorMin = 1 and Count = 9 so start index = count + 0 or 9

        '10x0
        '+9x1
        '+8x3
        '+7x1
        '+6x5
        '+5x2
        '+4x4
        '+3x4
        '+2x7
        '' Check Digit Not used +1x10 mod 11 = 132 mod 11 = 0
        'WeightFactorMin = 2 and Count = 9 so start index = count + 1 or 10

        '11x0
        '+10x1
        '+9x3
        '+8x1
        '+7x5
        '+6x2
        '+5x4
        '+4x4
        '+3x7
        '' Not Used +1x10 mod 11 = 132 mod 11 = 0
        'WeightFactorMin = 3 and Count = 9 so start index = count + 2 or 11
    End Function

    ''' <summary>
    ''' Calculates the sum or the products for each value in seed multiplied by the corrisponding weight in factor.  
    ''' If splitDigits is true (default is false) the system will add each digit to the sum individually rather than
    ''' as a product so 12 becomes total + 1 + 2 instead of total + 12
    ''' </summary>
    ''' <param name="seed"></param>
    ''' <param name="factor"></param>
    ''' <param name="splitDigits"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SumProductUsingWeight(ByVal seed() As Integer, _
                                                ByVal factor() As Integer, _
                                                Optional ByVal splitDigits As Boolean = False) As Long
        If seed Is Nothing OrElse seed.Count < 1 Then Return -1
        If factor Is Nothing OrElse factor.Count < 1 Then Return -1
        If seed.Count <> factor.Count Then Return -1
        Dim lngRet As Long = 0
        Try
            For i As Integer = 0 To seed.Count - 1
                Dim thisSum As Integer = (seed(i) * factor(i))
                If thisSum.ToString.Length > 1 And splitDigits Then
                    'we must split the string into different sum totals
                    Dim s() = thisSum.ToString().Select(Function(x) CInt(Val(x))).ToArray()
                    For Each t In s
                        lngRet += t
                    Next
                Else
                    lngRet += thisSum
                End If
            Next
        Catch ex As System.IndexOutOfRangeException
            Return -1
        Catch ex As Exception
            lngRet = -1
            Throw
        End Try
        Return lngRet
    End Function

#Region " Old code from POC test console"


    ' ''' <summary>
    ' ''' This works
    ' ''' </summary>
    ' ''' <param name="a"></param>
    ' ''' <param name="b"></param>
    ' ''' <returns></returns>
    ' ''' <remarks>
    ' ''' Multiply each source digit in b by the weight factor in a 
    ' ''' each sum is then checked for multiple digits like 12
    ' ''' multiple digits are added together so 12 become 1 + 2 = 3
    ' ''' Add all sums to create the Mod10SumProduct return value sum
    ' ''' the caller will need to use the formula checksum =  10 - (sum Mod 10)
    ' '''   any value < 0 or > 9 must be set to zero (typically only 10 is returned for zero)
    ' ''' </remarks>
    'Private Function Mod10SumProduct(ByVal a() As Integer, ByVal b() As Integer) As Integer
    '    Dim intRet As Integer = 0
    '    For i As Integer = 0 To a.Count - 1
    '        Dim thisSum As Integer = (a(i) * b(i))
    '        Console.WriteLine("Sum a {0} with b {1} = {2}", a(i), b(i), thisSum)
    '        If thisSum.ToString.Length > 1 Then
    '            'we must split the string into different sum totals
    '            Dim s() = thisSum.ToString().Select(Function(x) CInt(Val(x))).ToArray()
    '            For Each t In s
    '                intRet += t
    '                Console.WriteLine("Split Sum {0} ", t)
    '            Next
    '        Else

    '            intRet += thisSum
    '        End If

    '    Next
    '    Return intRet
    'End Function


    'Private Function Mod11SumProduct(ByVal a() As Integer) As Integer
    '    '10x0+9x1+8x3+7x1+6x5+5x2+4x4+3x4+2x7+1x10 mod 11 = 132 mod 11 = 0
    '    Dim intRet As Integer = 0
    '    For i As Integer = a.Count + 1 To 2 Step -1
    '        intRet += a((a.Count - i) + 1) * i
    '    Next
    '    Return intRet
    'End Function

    'Private Function Mod11SumProductUsingWeight(ByVal a() As Integer, ByVal b() As Integer, Optional ByVal splitDigits As Boolean = False) As Integer

    '    Dim intRet As Integer = 0
    '    For i As Integer = 0 To a.Count - 1
    '        Dim thisSum As Integer = (a(i) * b(i))
    '        If thisSum.ToString.Length > 1 And splitDigits Then
    '            'we must split the string into different sum totals
    '            Dim s() = thisSum.ToString().Select(Function(x) CInt(Val(x))).ToArray()
    '            For Each t In s
    '                intRet += t
    '            Next
    '        Else
    '            intRet += thisSum
    '        End If

    '    Next
    '    Return intRet
    'End Function

    'Private Function GetMod7CheckDigit(ByVal strSeed As String) As Integer

    '    Return CInt(Val(strSeed) Mod 7)
    'End Function

    ' ''' <summary>
    ' ''' Calculates the Mod 10 check digit using 
    ' ''' strSeed as the initial number and strWeight as the 
    ' ''' weight factor for each number in seed the weight factor 
    ' ''' as each character position is multiplied by the seed number 
    ' ''' at the corresponding position.  
    ' ''' Returns -1 if the len of seed and weight are different or for any other error
    ' ''' </summary>
    ' ''' <param name="strSeed"></param>
    ' ''' <param name="strWeight"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function GetMod10CheckDigit(ByVal strSeed As String, ByVal strWeight As String) As Integer

    '    If strSeed.Length <> strWeight.Length Then Return -1
    '    Dim sa() = strWeight.Select(Function(x) CInt(Val(x))).ToArray()
    '    Dim sb() = strSeed.Select(Function(x) CInt(Val(x))).ToArray()
    '    Dim sumP = Mod10SumProduct(sa, sb)
    '    Dim checksum = 10 - (sumP Mod 10)
    '    If checksum > 9 Then checksum = 0
    '    If checksum < 0 Then checksum = 0
    '    Return CInt(checksum)
    'End Function

    'Private Function GetMod11CheckDigit(ByVal strSeed As String, _
    '                                    ByVal strWeight As String, _
    '                                    Optional ByVal Mod11CheckDigit10Code As String = "0", _
    '                                    Optional ByVal Mod11CheckDigitOver10Code As String = "0", _
    '                                    Optional ByVal Mod11CheckDigitErrorCode As String = "E", _
    '                                    Optional ByVal splitDigits As Boolean = False) As String
    '    Dim strMod11CheckDigit As String = Mod11CheckDigitErrorCode
    '    If strSeed.Length <> strWeight.Length Then Return Mod11CheckDigitErrorCode
    '    Dim sp() = strSeed.Select(Function(x) CInt(Val(x))).ToArray()
    '    Dim sa() = strWeight.Select(Function(x) CInt(Val(x))).ToArray()
    '    Dim Mod11Check = Mod11SumProductUsingWeight(sp, sa, splitDigits)
    '    Dim Mod11CheckSum = (11 - CInt(Mod11Check Mod 11))
    '    If Mod11CheckSum = 10 Then
    '        strMod11CheckDigit = Mod11CheckDigit10Code
    '    ElseIf Mod11CheckSum > 10 Then
    '        strMod11CheckDigit = Mod11CheckDigitOver10Code
    '    Else
    '        strMod11CheckDigit = Mod11CheckSum.ToString()
    '    End If
    '    Return Mod11CheckDigitErrorCode
    'End Function

    'Private Function GetMod11CheckDigit(ByVal strSeed As String, _
    '                                    Optional ByVal Mod11CheckDigit10Code As String = "X", _
    '                                    Optional ByVal Mod11CheckDigitOver10Code As String = "0", _
    '                                    Optional ByVal Mod11CheckDigitErrorCode As String = "E") As String
    '    Dim strMod11CheckDigit As String = Mod11CheckDigitErrorCode
    '    If strSeed.Length < 1 Then Return Mod11CheckDigitErrorCode
    '    Dim sp() = strSeed.Select(Function(x) CInt(Val(x))).ToArray()
    '    Dim Mod11Check = Mod11SumProduct(sp)
    '    Dim Mod11CheckSum = (11 - CInt(Mod11Check Mod 11))
    '    If Mod11CheckSum = 10 Then
    '        strMod11CheckDigit = Mod11CheckDigit10Code
    '    ElseIf Mod11CheckSum > 10 Then
    '        strMod11CheckDigit = Mod11CheckDigitOver10Code
    '    Else
    '        strMod11CheckDigit = Mod11CheckSum.ToString()
    '    End If
    '    Return Mod11CheckDigitErrorCode
    'End Function

    'Private Function GetMod11CheckDigitNoWeight(ByVal strSeed As String, _
    '                                    Optional ByVal CheckDigit10Code As String = "X", _
    '                                    Optional ByVal CheckDigitOver10Code As String = "0", _
    '                                    Optional ByVal CheckDigitErrorCode As String = "E") As String
    '    Dim strCheckDigit = CheckDigitErrorCode
    '    Dim intCheckDigit = 11 - CInt(Val(strSeed Mod 11))

    '    If intCheckDigit = 10 Then
    '        strCheckDigit = CheckDigit10Code
    '    ElseIf intCheckDigit > 10 Then
    '        strCheckDigit = CheckDigitOver10Code
    '    Else
    '        strCheckDigit = intCheckDigit.ToString()
    '    End If
    '    Return strCheckDigit
    'End Function


#End Region

End Class
