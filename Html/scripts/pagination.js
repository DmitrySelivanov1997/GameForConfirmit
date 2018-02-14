function pagination(c, m) {
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
function Setpagination(){
    $("ul").remove()
    var paging = $('.paginate');
    var myUl = $('<ul/>');
    var c = pagination(pageNumber,20);
    for (let i = 0, l = 20; i <= l; i++)
    {
        myUl.append($('<li/>').text(c[i]));
    }
    paging.append(myUl);
}