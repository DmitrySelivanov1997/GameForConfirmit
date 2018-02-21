class FilterManager {
    constructor(orderType, parametr) {
        this.OrderType = orderType;
        this.Parametr = parametr;
        this.FilterName = "";
        this.FilterDateBefore = "";
        this.FilterDateAfter = "";
    }
    SetOrderedClass() {
        var element = $('#' + this.Parametr);
        if (this.OrderType == "desc")
            element.addClass("orderedDesc");
        else
            element.addClass("orderedAsc");
        return;
    }
}
class PaginationManager {
    constructor(pageNumber, entriesNumber) {
        this.PageNumber = pageNumber;
        this.EntriesNumber = entriesNumber;
        this.PageCount;
    }
    async SetPagination() {
        var numberOfEntries = await webCaller.GetNumberOfPages();
        this.PageCount = (Math.floor(numberOfEntries / this.EntriesNumber)) + 1;
        this.DrawPagination();
    }
    Pagination() {
        var current = this.PageNumber,
            last = this.PageCount,
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
    DrawPagination() {
        $("ul").remove();
        var paging = $('.paginate');
        var myUl = $('<ul/>');
        var c = this.Pagination();
        for (let i = 0; i < c.length; i++) {
            if ((this.PageNumber) === c[i]) {
                myUl.append($('<li/>').text(c[i]).addClass("liClicked"));
            }
            else {
                myUl.append($('<li/>').text(c[i]).attr('data-value', c[i]).addClass("pageNumber").on("click", pageOnClick));
            }
        }
        paging.append(myUl);
    }
}

var webCaller = new WebServiceCaller("http://co-yar-ws100:8080");
var filterManager = new FilterManager("desc", "Id");
var paginationManager = new PaginationManager(1, 25);

GetStats();
paginationManager.SetPagination();
async function GetStats() {
    var stats = await webCaller.GetData();
    DrawTable(stats);
}
function DrawTable(stats) {
    var tableTitle = [
        { class: "Id", name: "Id" }, { class: "GameStartTime", name: "Start Time" }, { class: "GameDuration", name: "Duration" }, { class: "TurnsNumber", name: "Number of Turns" },
        { class: "MapSize", name: "Map Size" }, { class: "GameResult", name: "Game Result" },
        { class: "WhiteAlgorithmName", name: "White Algorithm" }, { class: "BlackAlgorithmName", name: "Black Algorithm" }
    ];
    $(".tableFromHB").remove();
    var source = $("#entry-template").html();
    var template = Handlebars.compile(source);
    var context = {
        headingName: tableTitle,
        stats: stats
    };
    var html = template(context);
    $("#hereTable").append(html);
    filterManager.SetOrderedClass();
}

$(function () {
    $('#openNewPage').click(function () {
        filterManager.FilterName = $('#algorithmName').val();
        filterManager.FilterDateBefore = $('#dateBefore').val();
        filterManager.FilterDateAfter = $('#dateAfter').val();
        paginationManager.PageNumber = 1;
        GetStats();
        paginationManager.SetPagination();
    });
});

$(document).on('click', '.buttonHead', async function () {
    filterManager.FilterName = $('#algorithmName').val();
    filterManager.FilterDateBefore = $('#dateBefore').val();
    filterManager.FilterDateAfter = $('#dateAfter').val();
    if (filterManager.OrderType === "desc")
        filterManager.OrderType = "asc";
    else
        filterManager.OrderType = "desc";
    filterManager.Parametr = this.id;
    var stats = await webCaller.GetData();
    DrawTable(stats);
});

function pageOnClick() {
    if (this.getAttribute('data-value') != "...") {
        paginationManager.PageNumber = parseInt(this.getAttribute('data-value'));
        paginationManager.DrawPagination();
        GetStats();
    }
}