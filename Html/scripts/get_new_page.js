$(function () {
    $('#openNewPage').click(function () {
        pageNumber = $('#number').val();
        var filterName = $('#algorithmName').val();
        var filterDate= $('#date').val();
        GetStats(pageNumber,entriesNumber,filterName,filterDate);
    });
});