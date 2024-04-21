$(document).ready(function () {
    $("#toggleButton").click(function () {
        var sidebarWidth = $("#sidebar").width();
        if (sidebarWidth > 0) {
            $("#sidebar").width(0);
        } else {
            $("#sidebar").width('100%');
        }
    });
});
