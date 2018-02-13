var webCaller = new WebServiceCaller("http://co-yar-ws100:8080");
var pageNumber = 1;
var orderType = "desc";
var entriesNumber = 25;
var filterName = $('#algorithmName').val();
var filterDate= $('#date').val();
var parametr = "Id";
GetStats(pageNumber,entriesNumber,filterName,filterDate);
async function GetStats() {
    var stats = await webCaller.GetData(pageNumber,parametr,orderType,entriesNumber,filterName, filterDate);
    DrawTable(stats);
}
function DrawTable(stats) {
    $("table").remove()
    var table = $('#hereTable');
    var tableTitle = [
        "Id","Map Size", "Start Time", "Duration", "Game Result",
        "Number of Turns", "White Algorithm", "Black Algorithm"
    ];
    var myTable = $('<table/>', {
        class: 'myTable'
    }).append(
        $('<thead/>'),
        $('<tfoot/>'),
        $('<tbody/>')
        );
    var titleCell = $('<tr/>');
    $.each(tableTitle, function (myIndex, myData) {
        titleCell.append(
            $('<th/>', {
                text: myData
            })
        );
    });
    $.each(stats,function( i, myData ) {
        $("tbody",myTable).append(
            $('<tr/>').append(
                $('<td/>',{text:myData.Id}).addClass("Id").on("click",TdOnClick),
                $('<td/>',{text:myData.MapSize}).addClass("MapSize").on("click",TdOnClick),
                $('<td/>',{text:myData.GameStartTime}).addClass("GameStartTime").on("click",TdOnClick),
                $('<td/>',{text:myData.GameDuration}).addClass("GameDuration").on("click",TdOnClick), 
                $('<td/>',{text:myData.GameResult}).addClass("GameResult").on("click",TdOnClick),
                $('<td/>',{text:myData.TurnsNumber}).addClass("TurnsNumber").on("click",TdOnClick),
                $('<td/>',{text:myData.WhiteAlgorithmName}).addClass("WhiteAlgorithmname").on("click",TdOnClick),
                $('<td/>',{text:myData.BlackAlgorithmName}).addClass("BlackAlgorithmName").on("click",TdOnClick),
            )
        );
    });
    $("thead", myTable).append(titleCell);
    table.append(myTable);
}