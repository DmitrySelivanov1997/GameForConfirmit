var webCaller = new WebServiceCaller("http://co-yar-ws100:8080");
var pageNumber = 1;
GetStats(pageNumber);
async function GetStats(id) {
    var stats = await webCaller.GetGameSessionStatistic("/"+id);
    DrawTable(stats);
}
function DrawTable(stats) {
    var table = $('#hereTable');
    var tableTitle = [
        "Id","Map Size", "Start Time", "Duration", "Game Result",
        "Number of Turns", "White Algorithm", "Black Algorithm"
    ];
    var myTable = $('<table/>', {
        class: 'mytable'
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
                $('<td/>',{text:myData.Id}),
                $('<td/>',{text:myData.MapSize}),
                $('<td/>',{text:myData.GameStartTime}),
                $('<td/>',{text:myData.GameDuration}), 
                $('<td/>',{text:myData.GameResult}),
                $('<td/>',{text:myData.TurnsNumber}),
                $('<td/>',{text:myData.WhiteAlgorithmName}),
                $('<td/>',{text:myData.BlackAlgorithmName}),
            )
        );
    });
    $("thead", myTable).append(titleCell);
    table.append(myTable);
}