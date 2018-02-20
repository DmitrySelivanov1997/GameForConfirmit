$(function () {
    $('#openNewPage').click(function () {
        filterName = $('#algorithmName').val();
        filterDateBefore = $('#dateBefore').val();
        filterDateAfter = $('#dateAfter').val();
        pageNumber = 1;
        GetStats();
        SetPagination();
    });
});
function pageOnClick() {
        if (this.getAttribute('data-value')!= "...") {
            pageNumber = parseInt(this.getAttribute('data-value'));
            DrawPagination(pageCount);
            GetStats();
        }
}