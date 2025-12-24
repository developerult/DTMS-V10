<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WhatsNew.aspx.cs" Inherits="DynamicsTMS365.WhatsNew" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS What's New</title>         
         <%=cssReference%>             
        <style>
            html,
            body {height:100%; margin:0; padding:0;}
            html {font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}
        </style>
    </head>
    <body>     
        <%=jssplitter2Scripts%>
        <%=sWaitMessage%>            
        <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">         
            <div id="vertical" style="height: 98%; width: 98%;">
                <div id="menu-pane" style="height: 100%; width: 100%; background-color: white;">
                    <div id="tab" class="menuBarTab"></div>
                </div>
                <div id="top-pane">
                    <div id="horizontal" style="height: 100%; width: 100%;">
                        <div id="left-pane">
                            <div class="pane-content">
                                <% Response.Write(MenuControl); %>
                                <div id="menuTree"></div>
                            </div>
                        </div>
                        <div id="center-pane">
                            <% Response.Write(PageErrorsOrWarnings); %>
                            <%--<div style="position:fixed; background-color: white;width: 100%;"><h1>What's New</h1></div>--%>
                            <div style="margin-left:10px;">
                                <h1>What's New</h1>
           
<div class=WordSection1>


<p class=MsoNormal><o:p>&nbsp;</o:p></p>

<p class=MsoNormal><b><span style='font-size:16.0pt;line-height:106%;
color:#4472C4;mso-themecolor:accent1'>Version 8.5.0<o:p></o:p></span></b></p>

<h1><span style='font-family:"Calibri",sans-serif;mso-ascii-theme-font:minor-latin;
mso-hansi-theme-font:minor-latin;mso-bidi-theme-font:minor-latin'>Bug Fixes<o:p></o:p></span></h1>

<p class=MsoListParagraphCxSpFirst style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;line-height:115%'><span
style='mso-bidi-font-family:Calibri;mso-bidi-theme-font:minor-latin'><o:p>&nbsp;</o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:38.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level1 lfo16'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Fixed Content Management:<span
style='mso-spacerun:yes'>  </span><o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>extend the current page content management to
match new requirements above and to allow both Legal Entities and Users to make
additional customizations.<span style='mso-spacerun:yes'>  </span><o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Some settings must be locked by (a) System and
(b) Legal Entity<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:38.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level1 lfo16'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Fixed Field chooser issues on Grid (widget)<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:38.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level1 lfo16'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Fixed Document Upload Page: link documents to
Load Board page with viewer window.<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Add Document Type<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Must work on Smart Phone<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:38.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level1 lfo16'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Fixed Responsive Design:<span
style='mso-spacerun:yes'>  </span>identify issues that may impact a responsive
design<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpLast style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>How can ensure users on smaller devices, like
smart phones, have access to the menus, actions etc...<o:p></o:p></span></p>

<h1>Enhancements Menu Tree<o:p></o:p></h1>

<p class=MsoListParagraphCxSpFirst style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:38.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level1 lfo16'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Provide an improved user experience with the
following functionality.<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>System Default Setting:<span
style='mso-spacerun:yes'>  </span>all changes must support the ability to reset
to the current system defaults<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Legal Entity will have the ability to create multiple
options for each Role that are available to users. <o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Each user can now be able to customize the
following:<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:146.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level4 lfo16'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Name/Caption<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:146.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level4 lfo16'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Color/Font<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:146.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level4 lfo16'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Bold/Style<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:110.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level3 lfo16'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Nested Links:<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:146.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level4 lfo16'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Drag and Drop from one Header to another
Header<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:146.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level4 lfo16'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Select from a list of available pages based on
security <o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:146.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level4 lfo16'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Name/Caption <o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:146.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level4 lfo16'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Color/Font <o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:146.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level4 lfo16'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Bold/Style<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>All settings stored in SQL Server via REST
Services <o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>KPI landing pages associated with Company
&amp; Carrier header pages<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpLast style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Update Menu Settings by clicking on gear <o:p></o:p></span></p>

<h1>Enhancements Load Board </h1>

<p class=MsoListParagraphCxSpFirst style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Added new configurable filters to Load Board<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Page/Grid User Filter Settings:<span
style='mso-spacerun:yes'>  </span>extended the ability to save the selected
filter, visible columns, and sorting settings to allow for multiple saved
versions.<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Added full Drag and Drop options<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Users can save view options:<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:110.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level3 lfo16'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Horizontal/Vertical<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:110.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level3 lfo16'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Number Columns/Rows<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:110.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level3 lfo16'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Optimizer Settings (new feature being
developed)<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Link to images associated with transportation
providers and location data<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:110.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level3 lfo16'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Provide the ability to upload images and convert
to thumbnails<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:110.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level3 lfo16'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Images are available to all users on the same
Legal Entity but not to other Legal Entities<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:110.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level3 lfo16'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Provide ability to use System Default images<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Map Selected Routes:<span
style='mso-spacerun:yes'>  </span>add system parameter to max number of routes
to show on map.<span style='mso-spacerun:yes'>  </span><o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:110.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level3 lfo16'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Each route should have a different color.<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:110.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level3 lfo16'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Show color code by route.<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:110.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level3 lfo16'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Added sequence number or sorting option<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:110.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level3 lfo16'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Added Load Consolidation or grouping by column<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Added Maintain System Defaults: users can
reset to system defaults<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Added Create Legal Entity Defaults: users can reset
to one or more pre-configured settings<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Added Share with other Users:<span
style='mso-spacerun:yes'>  </span>a page setting can be saved to the Legal
Entity shared list of settings.<span style='mso-spacerun:yes'>  </span>Note:
Super Users will have the ability to share settings with all Legal Entities.<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Added Share filters with similar pages:<span
style='mso-spacerun:yes'>  </span>Provide a cross reference where the same
filters can be used on multiple pages.<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Added Selected Filter:<span
style='mso-spacerun:yes'>  </span>the most recent active filter will be loaded
by default, across all cross-reference pages.<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;text-indent:-.25in;
line-height:115%;mso-list:l5 level2 lfo16'><![if !supportLists]><span
style='font-family:"Courier New";mso-fareast-font-family:"Courier New"'><span
style='mso-list:Ignore'>o<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-bidi-font-family:Calibri;
mso-bidi-theme-font:minor-latin'>Added Filter in New Window or Tab:<span
style='mso-spacerun:yes'>  </span>provide a switch where users can select to
apply on current page, open in a new Tab, or open in a new window.<o:p></o:p></span></p>

<p class=MsoListParagraphCxSpLast style='margin-top:0in;margin-right:0in;
margin-bottom:10.0pt;margin-left:74.9pt;mso-add-space:auto;line-height:115%'><span
style='mso-bidi-font-family:Calibri;mso-bidi-theme-font:minor-latin'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><b><span style='font-size:16.0pt;line-height:106%;
color:#4472C4;mso-themecolor:accent1'>Version 8.4.0<o:p></o:p></span></b></p>

<h1>Bug Fixes</h1>

<p class=MsoListParagraphCxSpFirst style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Fixed a problem with the scheduler dates being
converted from different time zones &amp; fixed miscellaneous reported
scheduler issues</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Fixed issues where the “Item Act Freight Cost” was
not updating correctly</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Fixed Tariff import rules to deal with blank or
empty Equipment Name </p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Fixed Rate Shopping – Clicking “On Clear” Button
does the following:</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l3 level2 lfo2'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>set ship date to today &amp; set delivery date
to one week from today</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l3 level2 lfo2'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>set Total Wgt = 500</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l3 level2 lfo2'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>set Total Qty = 1</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l3 level2 lfo2'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>set Total Pkgs = 1</p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:0in;margin-left:.75in;mso-add-space:auto;text-indent:-.25in;
mso-list:l3 level2 lfo2'><![if !supportLists]><span style='font-family:Wingdings;
mso-fareast-font-family:Wingdings;mso-bidi-font-family:Wingdings'><span
style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp; </span></span></span><![endif]>update
Package data with defaults</p>

<p class=MsoListParagraphCxSpLast style='margin-top:0in;margin-right:0in;
margin-bottom:0in;margin-left:.25in;mso-add-space:auto;text-indent:-.25in;
line-height:normal;mso-list:l3 level1 lfo2'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-ascii-font-family:Calibri;
mso-fareast-font-family:"Times New Roman";mso-hansi-font-family:Calibri;
mso-bidi-font-family:Calibri'>Fixed issues with the Lane Maintenance pages in
D365 web where Receiving and Delivery time windows were not working.<o:p></o:p></span></p>

<p class=MsoNormal style='margin-top:0in;margin-right:0in;margin-bottom:0in;
margin-left:.25in;text-indent:-.25in;line-height:normal;mso-list:l3 level1 lfo2'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]><span style='mso-ascii-font-family:Calibri;
mso-fareast-font-family:"Times New Roman";mso-hansi-font-family:Calibri;
mso-bidi-font-family:Calibri'>Fixed issues with the Load Board pages in D365
web where the Origin and Destination time windows were not working.</span></p>

<h1>User Maintenance </h1>

<p class=MsoListParagraphCxSpFirst style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Added Optimization and Route planning
capabilities</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Scheduler now updates key booking fields allow
mapping between appointment and booking data like Truck ID, Driver Name etc.</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Added new fields and column widths to the
Appointment and Order Search in the Scheduler</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Scheduler now updates key booking fields to
allow mapping between appointment and booking data like Truck ID, Driver Name
etc.<span style='mso-spacerun:yes'>  </span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Added new logic to Duplicate the current booking
information when adding additional freight charges to a shipment like Dock Fees
or Export Charges</p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0in;margin-right:0in;
margin-bottom:0in;margin-left:31.5pt;mso-add-space:auto;text-indent:-.25in;
mso-list:l1 level1 lfo1'><![if !supportLists]><span style='font-family:Symbol;
mso-fareast-font-family:Symbol;mso-bidi-font-family:Symbol'><span
style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Updated the following User Maintenance pages</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level2 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Carrier</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level2 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Lane</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level2 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Company</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level2 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>EDI</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level2 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Preferred Carrier</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level2 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Lane Fees </p>

<p class=MsoListParagraphCxSpLast style='margin-left:.75in;mso-add-space:auto;
text-indent:-.25in;mso-list:l1 level2 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Lane Profile Specific Fees</p>

<h1>Load Tendering</h1>

<p class=MsoListParagraphCxSpFirst style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>New Allow Carrier to Accepts &amp; Book
Appointments by Email token - Settings in Lane Screen.</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>New Booking Appointment Token logic.</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>New Carrier Accept Reject by Email Settings</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>New Carrier Accept Reject Token logic</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Add logic to place booking on hold for Optimizer
and Consolidation stamping</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Finished Consolidation Stamping logic on Load
Board</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Added Optimization and Route Planning
capabilities</p>

<p class=MsoListParagraphCxSpLast style='margin-left:31.5pt;mso-add-space:auto;
text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Improved Route Planning when using the Routing
Guide</p>

<h1>New Pages In The Web App</h1>

<p class=MsoListParagraphCxSpFirst style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Carrier Items</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l11 level2 lfo3'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Carrier Fuel Index</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l11 level2 lfo3'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Nat Fuel Zones</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l11 level2 lfo3'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Fuel</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l11 level2 lfo3'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Equipment</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Lane Items</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l8 level2 lfo4'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Lane Trans Load</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Planning &amp; Execution Menu</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l7 level2 lfo5'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Claims</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>NEXTrack Items</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l0 level2 lfo6'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>NEXTrack Only Reports </p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Other Items</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l14 level2 lfo7'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Company Details</p>

<p class=MsoListParagraphCxSpLast style='margin-left:.75in;mso-add-space:auto;
text-indent:-.25in;mso-list:l14 level2 lfo7'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Lane Details</p>

<h1>New Actions in Menus In The Web App</h1>

<p class=MsoListParagraphCxSpFirst style='margin-left:.25in;mso-add-space:auto;
text-indent:-.25in;mso-list:l1 level2 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Lane Actions</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>ReCalc Lat Long</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.25in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level2 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Carrier Actions </p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Update All Fees</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.25in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level2 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Planning </p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Duplicate Pro</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>New Sequence</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>ReCalc – Tariff</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>ReCalc – Fees</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Map It !</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Adjust Credit</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Toggle Stamp</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Trans Load</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l6 level1 lfo8'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>New Maintenance Screen for Trans Load Cross Dock
settings</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Extended Tool Tip functionality on pages to show
more hover over information.</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Added Claims Maintenance</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Extended List System List Maintenance</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Included new Trimble Map ALK PC-Miler Web
Edition</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Added several new views for data under various
Navigation Menus</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Added new sorting and grouping logic for the web
load board when grouped by consolidation number.</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Added new validation for API integration Postal
Codes</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level2 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Added new Fuel Addendum Maintenance logic to the
Web Carrier Fuel and Tariff Fuel pages</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level2 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Hide columns and data per screen by restricting
access by security group type</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:.75in;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level2 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>New Financial Workflow Company level parameter
to require a ship confirmation for the freight bill to pass audit</p>

<p class=MsoListParagraphCxSpLast style='margin-left:1.25in;mso-add-space:auto;
text-indent:-.25in;mso-list:l1 level3 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Set the APAuditRequireShipConfirmation
&nbsp;parameter &nbsp;value &nbsp;to 1 for on/active or 0 for off</p>

<h1>Reporting Workflow Enhancements</h1>

<ul style='margin-top:0in' type=disc>
 <li class=MsoNormal style='margin-bottom:0in;line-height:normal;mso-list:l13 level1 lfo9'><span
     style='mso-ascii-font-family:Calibri;mso-fareast-font-family:"Times New Roman";
     mso-hansi-font-family:Calibri;mso-bidi-font-family:Calibri'>Modified the
     Reporting Workflow: added a new company level parameter setting used to
     turn the HTML version of the BOL Report on or off. The default value for
     the AllowHTMLBOLReport parameter is off.&nbsp; Note: this setting is only
     applied to the Quick Print reports on the Load Detail page and the Load
     Status Board in the Desktop Client.<o:p></o:p></span></li>
 <li class=MsoNormal style='margin-bottom:0in;line-height:normal;mso-list:l13 level1 lfo9'><span
     style='mso-ascii-font-family:Calibri;mso-fareast-font-family:"Times New Roman";
     mso-hansi-font-family:Calibri;mso-bidi-font-family:Calibri'>Modified the
     Reporting Workflow: added a new company level parameter setting used to
     control the BOL popup window on Dispatching.&nbsp; The default value is on
     for the AutoDisplayBOLReportOnDispatch is on.<o:p></o:p></span></li>
</ul>

<h1>Integration Workflow Enhancements</h1>

<ul style='margin-top:0in' type=disc>
 <li class=MsoNormal style='margin-bottom:0in;line-height:normal;mso-list:l13 level1 lfo10'><span
     style='mso-ascii-font-family:Calibri;mso-fareast-font-family:"Times New Roman";
     mso-hansi-font-family:Calibri;mso-bidi-font-family:Calibri'>Integration
     Workflow:&nbsp; Added new logic to extend the size of phone number and
     postal code fields in bookings and Lanes to 20 characters to support
     international data integration. To implement the changes integration must
     use web methods for v-8.4 or newer.&nbsp; This update has been
     incorporated into NAV 2018 and all BC integration modules.<o:p></o:p></span></li>
</ul>

<h1>Technical Enhancements</h1>

<p class=MsoListParagraphCxSpFirst style='text-indent:-.25in;mso-list:l10 level1 lfo11'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Enhanced error reporting messages and display
window</p>

<p class=MsoListParagraphCxSpMiddle style='text-indent:-.25in;mso-list:l10 level1 lfo11'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Added Credit Hold Maintenance to Web Load Board</p>

<p class=MsoListParagraphCxSpMiddle style='text-indent:-.25in;mso-list:l10 level1 lfo11'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Added Group By Logic to web grids</p>

<p class=MsoListParagraphCxSpMiddle style='text-indent:-.25in;mso-list:l10 level1 lfo11'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Extended the save user filter procedures</p>

<p class=MsoListParagraphCxSpMiddle style='text-indent:-.25in;mso-list:l10 level1 lfo11'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Added errors and logs to the Rate It booking
option on the Web Load Board</p>

<p class=MsoListParagraphCxSpMiddle style='text-indent:-.25in;mso-list:l10 level1 lfo11'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Added new Fuel Addendum Maintenance logic to the
Web Carrier Fuel and Tariff Fuel pages</p>

<p class=MsoListParagraphCxSpMiddle style='text-indent:-.25in;mso-list:l9 level1 lfo12'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Split the FM Task Service into two separate
Windows Services to increase speed and reduce extended CPU spikes: Freight
Master Task Service &amp; Dynamics TMS Routing Service</p>

<p class=MsoListParagraphCxSpMiddle style='text-indent:-.25in;mso-list:l10 level1 lfo11'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Update user security Lane Restriction logic</p>

<p class=MsoListParagraphCxSpMiddle style='text-indent:-.25in;mso-list:l10 level1 lfo11'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>User Validation, Performance and Security
enhancements.</p>

<p class=MsoListParagraphCxSpMiddle style='text-indent:-.25in;mso-list:l12 level1 lfo13'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Speed improved on loading additional pages after
user has been authenticated</p>

<p class=MsoListParagraphCxSpMiddle style='text-indent:-.25in;mso-list:l12 level1 lfo13'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>New Security added to prevent users from opening
older book marks when security profile has changed.</p>

<p class=MsoListParagraphCxSpLast style='text-indent:-.25in;mso-list:l10 level1 lfo11'><![if !supportLists]><span
style='font-size:16.0pt;line-height:106%;font-family:Symbol;mso-fareast-font-family:
Symbol;mso-bidi-font-family:Symbol;color:#2F5496;mso-themecolor:accent1;
mso-themeshade:191'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Added errors and logs logic to the Rate It!
booking option on the Web Load Board<span style='font-size:16.0pt;line-height:
106%;font-family:"Calibri Light",sans-serif;mso-ascii-theme-font:major-latin;
mso-fareast-font-family:"Times New Roman";mso-fareast-theme-font:major-fareast;
mso-hansi-theme-font:major-latin;mso-bidi-font-family:"Times New Roman";
mso-bidi-theme-font:major-bidi;color:#2F5496;mso-themecolor:accent1;mso-themeshade:
191'><o:p></o:p></span></p>

<h1>No Longer Supported</h1>

<p class=MsoListParagraphCxSpFirst style='margin-left:.25in;mso-add-space:auto;
text-indent:-.25in;mso-list:l1 level2 lfo1'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Existing web service methods for Pick List
Status Updates and AP Export version 6 (and earlier) are no longer supported
due to support for international postal codes and phone numbers.</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Integration will be updated as part of 8.4 or in
later upgrades to TMS. Added new fields and column widths to the Appointment
and Order Search in the Scheduler</p>

<p class=MsoListParagraphCxSpMiddle style='margin-left:31.5pt;mso-add-space:
auto;text-indent:-.25in;mso-list:l1 level1 lfo1'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Added new logic to Duplicate the current booking
information when adding additional freight charges to a shipment like Dock Fees
or Export Charges</p>

<p class=MsoListParagraphCxSpMiddle style='text-indent:-.25in;mso-list:l4 level1 lfo14'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Extended the capabilities of the TMS PRO Number
and CNS Number assignment to support backward compatibility</p>

<p class=MsoListParagraphCxSpMiddle style='text-indent:-.25in;mso-list:l4 level1 lfo14'><![if !supportLists]><span
style='font-family:Symbol;mso-fareast-font-family:Symbol;mso-bidi-font-family:
Symbol'><span style='mso-list:Ignore'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span></span><![endif]>Added logic to create or update the packages when
one or more of the following fields are modified</p>

<p class=MsoListParagraphCxSpLast style='margin-left:1.0in;mso-add-space:auto;
text-indent:-.25in;mso-list:l2 level2 lfo15'><![if !supportLists]><span
style='font-family:Wingdings;mso-fareast-font-family:Wingdings;mso-bidi-font-family:
Wingdings'><span style='mso-list:Ignore'>§<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span></span><![endif]>Total Wgt; Total Qty; Total Pkgs.</p>

</div>
                                <div style="display:none;" id="divWNHTML"></div>
                            </div>
                            

                            <!-- begin Page Content -->
                            <% Response.Write(FastTabsHTML); %>
                            <!-- End Page Content -->
                        </div>
                    </div>
                </div>
                <div id="bottom-pane" class="k-block" style="height: 100%; width: 100%;">
                    <div class="pane-content">
                        <% Response.Write(PageFooterHTML); %>
                    </div>
                </div>
            </div>

    <% Response.WriteFile("~/Views/AddWhatsNewWnd.html"); %>
    <% Response.Write(PageTemplates); %>
    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>          
    <script>    
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>';          
        var oWhatsNewGrid = null;        
        var tObj = this;
        var tPage = this;                  
        

        <% Response.Write(NGLOAuth2); %>

        
        <% Response.Write(PageCustomJS); %>


        //*************  execActionClick  ****************
        function execActionClick(btn, proc){                 
            if(btn.id === "btnMngWhatsNew"){                                  
                if (typeof (tPage["wdgtMngWhatsNewDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtMngWhatsNewDialog"])){
                    tPage["wdgtMngWhatsNewDialog"].show();                
                } else{alert("Missing HTML Element (wdgtMngWhatsNewDialog is undefined)");} //Add better error handling here if cm stuff is missing    
            }
            else if (btn.id == "btnRefresh" ){ refresh(); }
            else if (btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
        }
      
        //*************  Action Menu Functions ****************
        function refresh(){ ngl.readDataSource(oWhatsNewGrid); GetWhatsNewHTML(); }


        //*************  Page Level Functions ****************  
        function execBeforeWhatsNewGridInsert(e,fk,w){
            wndAddWhatsNew.center().open();
            return false; //returning false short circuits the widget Edit function so I can circumvent it and instead call my custom function when the Edit button is clicked            
        }

        function execBeforenotesInsert(e,fk,w){      
            //We need to populate the CarrierContCarrierControl in the new CarrierCont record so we get that from the header row
            var parentRow = $(e.currentTarget).closest("tr.k-detail-row").prev("tr"); // GET PARENT ROW ELEMENT    
            var parentGrid = parentRow.closest("[data-role=nglgrid]").data("kendoNGLGrid");            
            var parentModel = parentGrid.dataItem(parentRow); // GET THE PARENT ROW MODEL      
            // ACCESS THE PARENT ROW MODEL ATTRIBUTES
            var pVersion = parentModel.WhatsNewVersion;
            var pControl = parentModel.WhatsNewControl;
            var pFeatureType = parentModel.WhatsNewFeatureTypeControl;
            if (!pVersion) { alert("No Version Field - cannot insert record"); return false;}          
            if (!pControl) { alert("No Parent Control Field - cannot insert record"); return false;}
            if (!pFeatureType) { alert("No Feature Type Field - cannot insert record"); return false;}
            w.SetFieldDefault("WhatsNewVersion",pVersion);
            w.SetFieldDefault("WhatsNewParentID",pControl.toString());
            return w.SetFieldDefault("WhatsNewFeatureTypeControl",pFeatureType.toString());
        }
        
        function GetWhatsNewHTML(){            
            var strVersion = "";
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.filteredRead("WhatsNew/GetWhatsNewReportHTML", strVersion, tObj, "GetWhatsNewHTMLSuccessCallback", "GetWhatsNewHTMLAjaxErrorCallback");  
        }
        
        
        //*************  Call Back Functions ****************
        function WhatsNewGridDataBoundCallBack(e,tGrid){ oWhatsNewGrid = tGrid; } 

        function WhatsNewGridCB(data){ refresh();  }
        function notesCB(data){ refresh();  }

        function GetWhatsNewHTMLAjaxErrorCallback(xhr, textStatus, error) {
            var oResults = new nglEventParameters();
            var tObj = this;
            oResults.source = "GetWhatsNewHTMLAjaxErrorCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = "Get Whats New Report HTML Failure";
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);        
        }
        function GetWhatsNewHTMLSuccessCallback(data) {
            var oResults = new nglEventParameters();
            oResults.source = "GetWhatsNewHTMLSuccessCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed     
            this.rData = null;
            var tObj = this;
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (ngl.stringHasValue(data.Errors)) {
                        blnErrorShown = true;
                        oResults.error = new Error();
                        oResults.error.name = "Get Whats New HTML Data Failure";
                        oResults.error.message = data.Errors;
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                    else {
                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                this.rData = data.Data;
                                oResults.data = data.Data;
                                oResults.msg = "Success";
                                blnSuccess = true;                               
                                var WNhtml = "";
                                if(ngl.stringHasValue(data.Data[0])){ WNhtml = data.Data[0]; }
                                $("#divWNHTML").html(WNhtml);
                            }
                        }
                    }
                }
                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Whats New Report HTML Failure"; }
                    oResults.error = new Error();
                    oResults.error.name = "Get Whats New Report HTML Failure";
                    oResults.error.message = strValidationMsg;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
            } catch (err) { oResults.error = err; ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); }      
        }


        $(function () {
            //wire focus of all numerictextbox widgets on the page
            $("input[type=number]").bind("focus", function () {
                var input = $(this);
                clearTimeout(input.data("selectTimeId")); //stop started time out if any
                var selectTimeId = setTimeout(function(){ input.select(); });
                input.data("selectTimeId", selectTimeId);
            }).blur(function(e) { clearTimeout($(this).data("selectTimeId")); }); //stop started timeout
        });

        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
           
            if (control != 0){              
                GetWhatsNewHTML();
            }
            var PageReadyJS = <%=PageReadyJS%>;
            menuTreeHighlightPage(); //must be called after PageReadyJS
            var divWait = $("#h1Wait");               
            if (typeof (divWait) !== 'undefined') { divWait.hide(); }      
        });
        </script>
            <style>
                .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
                .k-tooltip { max-height: 500px; max-width: 450px; overflow-y: auto; }
                .k-grid tbody .k-grid-Edit { min-width: 0; }               
                .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }                             
            </style>  
        </div>  
    </body>
</html>
