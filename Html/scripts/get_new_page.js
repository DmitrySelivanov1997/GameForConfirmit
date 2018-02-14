$(function () {
    $('#openNewPage').click(function () {
       // pageNumber = $('#number').val();
        filterName = $('#algorithmName').val();
        filterDateBefore= $('#dateBefore').val();
        filterDateAfter = $('#dateAfter').val();
        GetStats();
    });
});