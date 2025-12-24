<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AutoMilesLanes.aspx.cs" Inherits="DynamicsTMS365.AutoMilesLanes" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Lane Bulk Miles Calculation</title>

    <!-- jQuery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

    <style>
        body { font-family: Arial, Helvetica, sans-serif; padding: 18px; }
        h2 { margin-bottom: 6px; }
        .controls { margin: 12px 0; display:flex; gap:10px; align-items:center; flex-wrap:wrap; }
        .controls input[type="text"] { padding:6px; width:260px; }
        .btn { padding:8px 12px; cursor:pointer; border-radius:4px; border:1px solid #999; background:#f4f4f4; }
        .btn-primary { background:#0b6; color:#fff; border-color:#087; }
        .btn-danger { background:#e44; color:#fff; border-color:#b22; }
        table { width:100%; border-collapse:collapse; margin-top:10px; }
        th, td { border:1px solid #ddd; padding:8px; text-align:left; vertical-align:middle; }
        th { background:#f6f6f6; }
        #progressWrap { margin-top:12px; display:none; }
        #progressBar { height:18px; background:#e6e6e6; border-radius:6px; overflow:hidden; }
        #progressBarInner { height:100%; width:0%; background:#4caf50; transition:width .2s; }
        .small { font-size:12px; color:#666; }
        .pager { margin-top:10px; display:flex; gap:6px; align-items:center; }
        .error { color:#b00; }
    </style>

    <script type="text/javascript">
        // ---------- CONFIG ----------
        const API_BASE = "/api/lane";                 // uses attribute routes from your controller
        const PAGE_SIZE = 500;                         // items per page
        const CONCURRENT = 5;                         // parallel requests for batch recalc
        const RETRY_COUNT = 1;                        // per item retries on failure

        // ---------- STATE ----------
        var currentPage = 1;
        var totalCount = 0;
        var totalPages = 1;
        var currentFilter = ""; // JSON filter string or search text depending on your API; we'll use simple text search in client

        $(function () {
            loadPage(1);
            $("#btnSearch").on("click", function () { currentFilter = $("#txtSearch").val().trim(); loadPage(1); });
            $("#txtSearch").on("keypress", function (e) { if (e.which === 13) { $("#btnSearch").click(); } });
            $("#btnRecalc").on("click", startBulkRecalc);
            $("#chkSelectAll").on("change", function () {
                $(".chkLane").prop("checked", $(this).prop("checked"));
            });
            $("#btnRefresh").on("click", function () { loadPage(currentPage); });
        });

        // ---------- Load paged records from GetRecords ----------
        function loadPage(page) {
            currentPage = page || 1;
            var skip = (currentPage - 1) * PAGE_SIZE;

            $("#status").text("Loading...").removeClass("error");
            // We call GetRecords with a filter string. The controller expects a JSON serialized DAL.Models.AllFilters
            // For simplicity we call it without filter and then client-side filter. If you have server-side filtering,
            // replace the URL with the proper filter JSON.
            
            var filter = {
                Groups: [],
                sortName: "",
                sortDirection: "",
                page: currentPage,
                skip: skip,
                take: PAGE_SIZE,
                LEAdminControl: 1,
                Data: $("#txtSearch").val(), // your search textbox value
                FilterValues: [
                    {
                        filterID: 2,
                        filterCaption: "Lane Bench Miles",
                        filterName: "LaneBenchMiles",
                        filterValueFrom: "0",    // <--- IMPORTANT
                        filterValueTo: "",
                        filterFrom: null,
                        filterTo: null
                    }
                ]
            };
            var filterParam = encodeURIComponent(JSON.stringify(filter));

            const url = `${API_BASE}/GetRecords?filter=` + filterParam;

            $.ajax({
                url: url,
                method: "GET",
                dataType: "json",
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
            }).done(function (res) {
                // Expected response shape: { Data: [...], Count: N } from your Models.Response
                var list = res && res.Data ? res.Data : [];
                // If user provided a text search, do a client-side filter over a few fields:
                if (currentFilter && currentFilter.length > 0) {
                    const q = currentFilter.toLowerCase();
                    list = list.filter(x =>
                        (x.ReferenceLaneNumber || "").toString().toLowerCase().includes(q)
                        || (x.LaneName || "").toString().toLowerCase().includes(q)
                        || (x.LaneControl || "").toString().toLowerCase().includes(q)
                    );
                    totalCount = res?.Count ?? list.length;
                } else {
                    // if API returns Count use it; otherwise use length
                    totalCount = (res && (typeof res.Count !== "undefined")) ? res.Count : list.length;
                }

                totalPages = Math.max(1, Math.ceil(totalCount / PAGE_SIZE));

                // compute page slice:
                var startIdx = (currentPage - 1) * PAGE_SIZE;
                var pageItems = list.slice(startIdx, startIdx + PAGE_SIZE);

                renderTable(pageItems);
                renderPager();
                $("#status").text(`Showing ${pageItems.length} of ${totalCount} lanes.`);
            }).fail(function (xhr) {
                $("#status").text("Failed to load lanes.").addClass("error");
            });
        }

        // ---------- Render table ----------
        function renderTable(items) {
            var html = "";
            if (!items || items.length === 0) {
                html = `<tr><td colspan="5">No lanes found.</td></tr>`;
            } else {
                $.each(items, function (i, lane) {
                    // adapt fields as per your model (vLELane365). Ensure LaneControl, ReferenceLaneNumber, LaneName exist.
                    html += `<tr>
                        <td style="width:40px"><input type="checkbox" class="chkLane" value="${lane.LaneControl}" data-ref="${escapeHtml(lane.ReferenceLaneNumber || '')}" /></td>
                        <td style="width:80px">${lane.LaneControl}</td>
                        <td>${escapeHtml(lane.ReferenceLaneNumber || '')}</td>
                        <td>${escapeHtml(lane.LaneName || '')}</td>
                        <td>${escapeHtml(lane.LaneNumber || '')}</td>
                        <td>${escapeHtml(lane.LaneBenchMiles || '')}</td>
                        <td class="small">Last Calc: ${lane.LastMilesCalcDate || ''}</td>
                    </tr>`;
                });
            }
            $("#tblBody").html(html);
            $("#chkSelectAll").prop("checked", false);
        }

        // ---------- Pager UI ----------
        function renderPager() {
            var html = `<div class="pager">
                <button class="btn" onclick="gotoPage(1)" ${currentPage===1?'disabled':''}>First</button>
                <button class="btn" onclick="gotoPage(${currentPage-1})" ${currentPage===1?'disabled':''}>Prev</button>
                <span>Page <b>${currentPage}</b> of <b>${totalPages}</b></span>
                <button class="btn" onclick="gotoPage(${currentPage+1})" ${currentPage===totalPages?'disabled':''}>Next</button>
                <button class="btn" onclick="gotoPage(${totalPages})" ${currentPage===totalPages?'disabled':''}>Last</button>
                <span class="small" style="margin-left:10px">Total lanes: ${totalCount}</span>
                <button class="btn" id="btnRefresh" style="margin-left:5px">Refresh</button>
            </div>`;
            $("#pager").html(html);
        }

        function gotoPage(p) {
            p = Math.max(1, Math.min(totalPages, p));
            loadPage(p);
        }

        // ---------- Bulk Recalc (concurrent queue) ----------
        function startBulkRecalc() {
            var ids = $(".chkLane:checked").map(function () { return $(this).val(); }).get();
            if (!ids || ids.length === 0) { alert("Please select one or more lanes to recalculate."); return; }

            if (!confirm(`Recalculate miles for ${ids.length} lanes?`)) return;

            // UI prepare
            $("#progressWrap").show();
            updateProgress(0, ids.length);
            $("#btnRecalc").prop("disabled", true);
            $("#status").text("Starting batch...").removeClass("error");
            $("#errorList").empty();

            // queue with concurrency
            let queue = ids.slice(); // clone
            let running = 0;
            let completed = 0;
            let failed = 0;
            let results = [];

            function next() {
                if (queue.length === 0 && running === 0) {
                    // done
                    $("#btnRecalc").prop("disabled", false);
                    $("#status").text(`Completed: ${completed} / ${ids.length}. Failed: ${failed}`);
                    return;
                }
                while (running < CONCURRENT && queue.length > 0) {
                    const id = queue.shift();
                    running++;
                    processOne(id, RETRY_COUNT).done(function (r) {
                        completed++;
                        results.push({ id: id, ok: true });
                    }).fail(function (err) {
                        failed++;
                        results.push({ id: id, ok: false, err: err });
                        $("#errorList").append(`<div class="error small">Lane ${id}: ${err}</div>`);
                    }).always(function () {
                        running--;
                        updateProgress(completed + failed, ids.length);
                        $("#status").text(`Processing... Completed: ${completed} / ${ids.length}  Failed: ${failed}`);
                        next();
                    });
                }
            }

            next();
        }

        // returns jqXHR promise
        function processOne(id, retriesLeft) {
            var d = $.Deferred();
            const url = `${API_BASE}/RecalculateLatLongMiles?id=${encodeURIComponent(id)}`;
            $.ajax({
                url: url,
                method: "GET",
                dataType: "json",
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                timeout: 30000 // 30s
            }).done(function (res) {
                // If your API returns { Data: ..., Errors: ... } you may check success/failure here:
                // treat non-null Errors or a non-success status as failure.
                if (res && res.Errors && res.Errors.length > 0) {
                    if (retriesLeft > 0) {
                        setTimeout(function () { processOne(id, retriesLeft - 1).then(d.resolve, d.reject); }, 800);
                    } else {
                        d.reject(res.Errors.join("; "));
                    }
                } else {
                    d.resolve(res);
                }
            }).fail(function (xhr, status, err) {
                if (retriesLeft > 0) {
                    setTimeout(function () { processOne(id, retriesLeft - 1).then(d.resolve, d.reject); }, 800);
                } else {
                    var msg = (xhr && xhr.responseText) ? xhr.responseText : (err || status || "Unknown error");
                    d.reject(msg);
                }
            });
            return d.promise();
        }

        function updateProgress(done, total) {
            var pct = total === 0 ? 0 : Math.round((done / total) * 100);
            $("#progressBarInner").css("width", pct + "%");
            $("#progressText").text(`${done} / ${total} (${pct}%)`);
        }

        // ---------- Helpers ----------
        function escapeHtml(s) {
            if (!s && s !== 0) return "";
            return String(s).replace(/[&<>"'`=\/]/g, function (c) { return {'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;','/':'&#47;','`':'&#96;','=':'&#61;'}[c]; });
        }
    </script>
</head>

<body>
    <h2>Lane Bulk Miles Calculation</h2>

    <div class="controls">
        <input type="text" id="txtSearch" placeholder="Search by lane id, name or reference..." />
        <button id="btnSearch" class="btn">Search</button>

        <div style="flex:1"></div>

        <label><input type="checkbox" id="chkSelectAll" /> Select All (on current page)</label>

        <button id="btnRecalc" class="btn btn-primary">Recalculate Miles for Selected</button>
        <button id="btnClearErrors" class="btn" onclick="$('#errorList').empty();">Clear Errors</button>
    </div>

    <div id="status" class="small">Loading...</div>

    <div style="margin-top:8px;">
        <table>
            <thead>
                <tr>
                    <th style="width:40px"></th>
                    <th style="width:80px">Lane ID</th>
                    <th>Reference #</th>
                    <th>Lane Name</th>
                    <th>Lane Number</th>
                    <th>Lane Bench Miles</th>
                    <th>Info</th>
                </tr>
            </thead>
            <tbody id="tblBody">
                <tr><td colspan="5">Loading...</td></tr>
            </tbody>
        </table>
    </div>

    <div id="pager"></div>

    <div id="progressWrap">
        <div id="progressBar"><div id="progressBarInner"></div></div>
        <div id="progressText" class="small" style="margin-top:6px"></div>
        <div id="errorList" style="margin-top:8px"></div>
    </div>
</body>
</html>

