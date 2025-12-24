/// <summary>Kendo UI Grid Plugin.</summary>
/// <description>Demonstrate a Kendo UI Grid Plugin with a Show/Hide Columns Pick List.</description>
/// <version>1.0</version>
/// <author>John DeVight</author>
/// <site>http://jsdev.wikidot.com/blog:3</>
/// <license>
/// Licensed under the MIT License (MIT)
/// You may obtain a copy of the License at
/// http://opensource.org/licenses/mit-license.html
/// </license>
(function ($, kendo) {
    var ExtGrid = kendo.ui.Grid.extend({
        options: {
            toolbarColumnMenu: false,
            name: "ExtGrid"
        },

        init: function (element, options) {
            /// <summary>
            /// Initialize the widget.
            /// </summary>

            if (options.toolbarColumnMenu === true && typeof options.toolbar === "undefined") {
                options.toolbar = [];
            }

            // Call the base class init.
            kendo.ui.Grid.fn.init.call(this, element, options);

            this._initToolbarColumnMenu();
        },

        _initToolbarColumnMenu: function () {
            /// <summary>
            /// Determine whether the column menu should be displayed, and if so, display it.
            /// </summary>

            // The toolbar column menu should be displayed.
            if (this.options.toolbarColumnMenu === true && this.element.find(".k-ext-grid-columnmenu").length === 0) {

                // Create the column menu items.
                var $menu = $("<ul></ul>");

                // Loop over all the columns and add them to the column menu.
                for (var idx = 0; idx < this.columns.length; idx++) {
                    var column = this.columns[idx];
                    // A column must have a title to be added.
                    if ($.trim(column.title).length > 0) {
                        // Add columns to the column menu.
                        $menu.append(kendo.format("<li><input  type='checkbox' data-index='{0}' data-field='{1}' data-title='{2}' {3}>{4}</li>",
                            idx, column.field, column.title, column.hidden ? "" : "checked", column.title));
                    }
                }

                // Create a "Columns" menu for the toolbar.
                this.wrapper.find("div.k-grid-toolbar").append("<ul class='k-ext-grid-columnmenu' style='float:right;'><li data-role='menutitle'>Columns</li></ul>");
                this.wrapper.find("div.k-grid-toolbar ul.k-ext-grid-columnmenu li").append($menu);

                var that = this;

                this.wrapper.find("div.k-grid-toolbar ul.k-ext-grid-columnmenu").kendoMenu({
                    closeOnClick: false,
                    select: function (e) {
                        // Get the selected column.
                        var $item = $(e.item), $input, columns = that.columns;
                        $input = $item.find(":checkbox");
                        if ($input.attr("disabled") || $item.attr("data-role") === "menutitle") {
                            return;
                        }

                        var column = that._findColumnByTitle($input.attr("data-title"));

                        // If checked, then show the column; otherwise hide the column.
                        if ($input.is(":checked")) {
                            that.showColumn(column);
                        } else {
                            that.hideColumn(column);
                        }
                    }
                });
            }
        },

        _findColumnByTitle: function (title) {
            /// <summary>
            /// Find a column by column title.
            /// </summary>

            var result = null;

            for (var idx = 0; idx < this.columns.length && result === null; idx++) {
                column = this.columns[idx];

                if (column.title === title) {
                    result = column;
                }
            }

            return result;
        }
    });
    kendo.ui.plugin(ExtGrid);
})(window.kendo.jQuery, window.kendo);