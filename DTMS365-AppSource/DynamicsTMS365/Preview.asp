<%
	set FP = Server.CreateObject("ADODB.Connection")
	FP.Open "DRIVER={SQL Server};server=IQSS-52\SQL2014;UID=sa;PWD=iQss2014;DATABASE=NGLEDI"

    'rs.Open "Exec Usp_PreviewEDIMasterDocumentConfig 1", FP

	'sql = "SELECT " & popsection & " FROM FP_Popup WHERE CUId= " & Request.Cookies("CuId")
	sql = "Exec Usp_PreviewEDIMasterDocumentConfig 1"
	set rs =  FP.Execute(sql)

    Response.write "<font face=""calibri"">"
    Response.write "<b><u>EDI Document - " & rs.fields("EDITName") & "&nbsp;&nbsp;" & rs.fields("MasterDocInbound") & "</b></u><br><br>"
    Response.write "<table border=""1""><tr>"
    'For each item in rs.Fields
    ' response.write "<th>" & item.Name & "</th>"
    'Next
            response.write "<th>Element</th>"
            response.write "<th>Data Type</th>"
            response.write "<th>Min. Length</th>"
            response.write "<th>Max. Length</th>"
            response.write "<th>Default Value</th>"
            response.write "<th>Validation Type</th>"
            response.write "<th>Formatting Function</th>"

    segname = ""
    Response.write "</tr>"
    While Not rs.EOF
    if rs.fields("ElementName") <> "" then
         response.write "<tr>"
         'For each item in rs.Fields
         ' Response.write "<td>" & rs(item.Name) & "</td>"
         'Next
                response.write "<td>" & rs.fields("ElementName") & "</td>"
                response.write "<td>" & rs.fields("ElementEDIDataTypeControl") & "</td>"
                response.write "<td>" & rs.fields("ElementMinLength") & "</td>"
                response.write "<td>" & rs.fields("ElementMaxLength") & "</td>"
                response.write "<td>" & rs.fields("DSEDefaultVal") & "</td>"
                response.write "<td>" & rs.fields("ElementValidationTypeControl") & "</td>"
                response.write "<td>" & rs.fields("ElementFormattingFnControl") & "</td>"
         response.write "</tr>"
    end if
     
      
      if SegmentName <> rs.fields("SegmentName") then
       response.write "</table><br><br>"
        response.write "<table border=""1"">"
        'For each item in rs.Fields
        ' response.write "<th>" & item.Name & "</th>"
        'Next
            response.write "<th>Element</th>"
            response.write "<th>Data Type</th>"
            response.write "<th>Min. Length</th>"
            response.write "<th>Max. Length</th>"
            response.write "<th>Default Value</th>"
            response.write "<th>Validation Type</th>"
            response.write "<th>Formatting Function</th>"
      end if 

      SegmentName = rs.fields("SegmentName") 
     rs.MoveNext
    Wend
    response.write "</table></font>"

%>
<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <title></title>
</head>
<body>

</body>
</html>