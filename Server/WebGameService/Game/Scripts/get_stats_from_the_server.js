var webCaller = new WebServiceCaller("http://co-yar-ws100:8080");
var pageNumber = 1;
var orderType = "desc";
var entriesNumber = 25;
var filterName = $('#algorithmName').val();
var filterDateBefore = $('#dateBefore').val();
var filterDateAfter = $('#dateBefore').val();
var parametr = "Id";
var pageCount;
GetStats();
SetPagination();
async function GetStats() {
    var stats = await webCaller.GetData(pageNumber, parametr, orderType, entriesNumber, filterName, filterDateBefore, filterDateAfter);
    DrawTable(stats);
}
function DrawTable(stats)
{
    var tableTitle = [
        {class:"Id",name:"Id"}, {class:"GameStartTime",name:"Start Time"}, {class:"GameDuration",name:"Duration"},{class:"TurnsNumber",name:"Number of Turns"},
        {class:"MapSize",name:"Map Size"},{class:"GameResult",name:"Game Result"},
         {class:"WhiteAlgorithmName",name:"White Algorithm"}, {class:"BlackAlgorithmName",name:"Black Algorithm"}
    ];
    $(".tableFromHB").remove();
    var source   = $("#entry-template").html();
    var template = Handlebars.compile(source);
    var context = { 
        headingName: tableTitle, 
        stats:stats
    };
    var html    = template(context);
    $("#hereTable").append(html);
}

async function SetPagination() {
    var numberOfEntries = await webCaller.GetNumberOfPages();
    pageCount = (Math.floor(numberOfEntries/entriesNumber))+1; 
    DrawPagination(pageCount);
}
