$(function () {
    $('#openNewPage').click(function () {
        var page = $('#number').val();
        $("table").remove()
        GetStats(page);
    });
});