$(function () {
    $('#openNewPage').click(function () {
        filterName = $('#algorithmName').val();
        filterDateBefore= $('#dateBefore').val();
        filterDateAfter = $('#dateAfter').val();
        pageNumber = 1;
        GetStats();
        SetPagination();
    });
});
$(document).on('click','ul li', function(){
    if(this.innerText!="..."){
        pageNumber = parseInt(this.innerText);
        DrawPagination(pageCount);
        GetStats();
    }
});