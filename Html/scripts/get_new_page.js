$(function () {
    $('#openNewPage').click(function () {
        pageNumber = $('#number').val();
        filterName = $('#algorithmName').val();
        filterDate= $('#date').val();
        GetStats();
    });
});