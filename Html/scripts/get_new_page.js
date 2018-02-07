$(function () {
    $('#openNewPage').click(function () {
        pageNumber = $('#number').val();
        $("table").remove()
        GetStats(pageNumber);
    });
});