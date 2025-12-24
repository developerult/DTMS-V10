var i = 0;

function timedCount() {
    i = i + 1;
    postMessage(i);
    setTimeout("timedCount()", 500);
}

self.addEventListener('message', function (e) {
    var oCount = e.data
    if ("start" in oCount) {
        timedCount();
        return;
    }
    var iCountDown = 0;
    var iCount = 0;
    var iLeft = 0
    if ("countDown" in oCount) {
        iCountDown = oCount.countDown;
    }
    if ("current" in oCount) {
        iCount = oCount.current;
    }
    iLeft = iCountDown - iCount;

    var rMsg = "You have " + iLeft.toString() + " seconds to detonation!!!"
    self.postMessage(rMsg);

}, false);
//calling timed count here will start the web worker
//timedCount();