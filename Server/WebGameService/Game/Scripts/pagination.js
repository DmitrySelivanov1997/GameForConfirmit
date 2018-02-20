function pagination(c, m)  {
    var current = c,
        last = m,
        delta = 2,
        left = current - delta,
        right = current + delta + 1,
        range = [],
        rangeWithDots = [],
        l;

    for (let i = 1; i <= last; i++) {
        if (i == 1 || i == last || i >= left && i < right) {
            range.push(i);
        }
    }

    for (let i of range) {
        if (l) {
            if (i - l === 2) {
                rangeWithDots.push(l + 1);
            } else if (i - l !== 1) {
                rangeWithDots.push('...');
            }
        }
        rangeWithDots.push(i);
        l = i;
    }

    return rangeWithDots;
}
function DrawPagination(totalPages){
    $("ul").remove();
    var paging = $('.paginate');
    var myUl = $('<ul/>');
    var c = pagination(pageNumber,totalPages);
    for (let i = 0; i < c.length; i++)
    {
        if((pageNumber)===c[i])
        {
            myUl.append($('<li/>').text(c[i]).addClass("liClicked"));
        }
        else
        { 
            myUl.append($('<li/>').text(c[i]).attr('data-value',c[i]).addClass("pageNumber").on("click", pageOnClick));
        }
    }
    paging.append(myUl);
}