<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderEntryGrid.aspx.cs" Inherits="DynamicsTMS365.OrderEntryGrid" %>

<!DOCTYPE html>

<html>
    <head >
        <title>Dynamics TMS Order Entry Grid</title>         
        <link href="Content/kendoR32023/classic-opal.css" rel="stylesheet" />               
        <link href="Content/NGL/v-8.5.4.001/common.css" rel="stylesheet" />   
        <style>

html,

body
{
    height:100%;
    margin:0;
    padding:0;
}

html
{font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}

</style>
    </head>
    <body>       
        <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>  
        <script src="Scripts/kendoR32023/kendo.all.min.js"></script>
<script>kendo.ui['Button'].fn.options['size'] = "small";</script>
        <script src="https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/core.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/NGLobjects.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/splitter2.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/SSOA.js"></script>
    <div>
    <div id="grid" tabindex="0"></div>
    <script>
      $("#grid").kendoGrid({
        // the column fields should match the excel columns
        columns: [
          { field: "Name" },
          { field: "Age" }
        ],
        dataSource: [
          { Name: "John Doe", Age: 33 }
        ]
      }).on('focusin', function(e) {
        // get the grid position
        var offset = $(this).offset();
        // crete a textarea element which will act as a clipboard
        var textarea = $("<textarea>");
        // position the textarea on top of the grid and make it transparent
        textarea.css({
          position: 'absolute',
          opacity: 0,
          top: offset.top,
          left: offset.left,
          border: 'none',
          width: $(this).width(),
          height: $(this).height()
        })
        .appendTo('body')
        .on('paste', function() {
          // handle the paste event
          setTimeout(function() {
            // the the pasted content
            var value = $.trim(textarea.val());
            // get instance to the grid
            var grid = $("#grid").data("kendoGrid");
            // get the pasted rows - split the text by new line
            var rows = value.split('\n');

            var data = [];

            for (var i = 0; i < rows.length; i++) {
              var cells = rows[i].split('\t');
              data.push({
                Name: cells[0],
                Age: cells[1]
              });
            };
            grid.dataSource.data(data);
          });
        }).on('focusout', function() {
          // remove the textarea when it loses focus
          $(this).remove();
        });
        // focus the textarea
        setTimeout(function() {
          textarea.focus();
        });
      });
    </script>
    </div>
</body>
</html>
