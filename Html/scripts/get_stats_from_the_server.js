var webCaller = new WebServiceCaller("http://co-yar-ws100:8080");
var pageNumber = 1;
var orderType = "desc";
var entriesNumber = 25;
var filterName = $('#algorithmName').val();
var filterDateBefore = $('#dateBefore').val();
var filterDateAfter = $('#dateBefore').val();
var parametr = "Id";
GetStats();
async function GetStats() {
    var stats = await webCaller.GetData(pageNumber, parametr, orderType, entriesNumber, filterName, filterDateBefore, filterDateAfter);
    DrawTable(stats);
}
function DrawTable(stats) {
    $("table").remove()
    var table = $('#hereTable');
    var tableTitle = [
        "Id", "Map Size", "Start Time", "Duration", "Game Result",
        "Number of Turns", "White Algorithm", "Black Algorithm"
    ];
    var tableClasses = [
        "Id", "MapSize", "GameStartTime", "GameDuration", "GameResult",
        "TurnsNumber", "WhiteAlgorithmName", "BlackAlgorithmName"
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
            $('<th/>', {text: myData}).addClass(tableClasses[myIndex]).on("click", TdOnClick)
        );
    });
    $.each(stats, function (i, myData) {
        $("tbody", myTable).append(
            $('<tr/>').append(
                $('<td/>', { text: myData.Id }).addClass(tableClasses[0]).on("click", TdOnClick),
                $('<td/>', { text: myData.MapSize }).addClass(tableClasses[1]).on("click", TdOnClick),
                $('<td/>', { text: myData.GameStartTime }).addClass(tableClasses[2]).on("click", TdOnClick),
                $('<td/>', { text: myData.GameDuration }).addClass(tableClasses[3]).on("click", TdOnClick),
                $('<td/>', { text: myData.GameResult }).addClass(tableClasses[4]).on("click", TdOnClick),
                $('<td/>', { text: myData.TurnsNumber }).addClass(tableClasses[5]).on("click", TdOnClick),
                $('<td/>', { text: myData.WhiteAlgorithmName }).addClass(tableClasses[6]).on("click", TdOnClick),
                $('<td/>', { text: myData.BlackAlgorithmName }).addClass(tableClasses[7]).on("click", TdOnClick),
            )
        );
    });
    $("thead", myTable).append(titleCell);
    table.append(myTable);
}