var fieldChangeTimer;

var webCaller = new WebServiceCaller("http://co-yar-ws100:8080");
$(function () {
    $('#startTournament').click(function () {
        GetAlgorithmsNames();
        PostDataToTheServer();
        DisableButtons();
    });
});
async function GetAlgorithmsNames() {
    var whiteName = await webCaller.GetAlgorithmName("white");
    SetAlgorithmName(whiteName,"white");
    var blackName = await webCaller.GetAlgorithmName("black");
    SetAlgorithmName(blackName,"black");
}

function SetAlgorithmName(name, algType) {
    if (algType === "black") {
        var secondAlg = $('#secondAlgorithm');
        var secondAlgName = $('#secondAlgorithmName');
        secondAlg.text(name);
        secondAlgName.text(name);
    }
    if (algType === "white") {
        var firstAlg = $('#firstAlgorithm');
        var firstAlgName = $('#firstAlgorithmName');
        firstAlg.text(name);
        firstAlgName.text(name);
    }
}
function PostDataToTheServer() {
    var mapSize = $('#mapSize').val();
    var numberOfGames = $('#numberOfMatches').val();
    var WaitTime = $('#turnsTimeSlider').val();
    webCaller.PostData({ MapSize: mapSize, NumberOfGames: numberOfGames, WaitTime: WaitTime });
}

async function CreateTable() { //creating a new game field after starting tournament
	$("table").remove();
    var stats = await webCaller.GetGameStats();
    var map = stats.Map;
    var table = $('<table></table>').attr('id', 'gameField');
    for (i = 0; i < map.length; i++) {
        var row = $('<tr></tr>');
        for (j = 0; j < map.length; j++) {
            row.append($('<td></td>').addClass(addClassForCell(i, j, map)));
        }
        table.append(row);
    }
    $('#hereTable').append(table)
    fieldChangeTimer = setInterval(drawingTable, 100);

}

async function drawingTable() { // changing color of table cell within 
    var stats = await webCaller.GetGameStats();
    var map = stats.Map;
    var whiteStatistics = stats.WhiteStatistics;
    var blackStatistics = stats.BlackStatistics;
    UpdateStats(whiteStatistics, blackStatistics);
    var table = $('#gameField');
    if (map === null) {
        clearInterval(fieldChangeTimer);
        EnableButtons();
        return;
    }
    for (i = 0; i < map.length; i++) {
        var row = $('<tr></tr>');
        for (j = 0; j < map.length; j++) {
            $(table[0].rows[i].cells[j]).attr("class",addClassForCell(i, j, map));
        }
    }
}
function UpdateStats(whiteStat, blackStat) {
    $('#numberOfTurns').text(whiteStat.TurnNumber);
    $('#score').text(whiteStat.NumberOfWins + ':' + blackStat.NumberOfWins);

    $('#whiteArmyFoodEaten').text(whiteStat.FoodEaten);
    $('#whiteArmyCurrentUnits').text(whiteStat.CurrentArmyNumber);
    $('#whiteArmyEnemiesKilled').text(whiteStat.EnemiesKilled);

    $('#blackArmyFoodEaten').text(blackStat.FoodEaten);
    $('#blackArmyCurrentUnits').text(blackStat.CurrentArmyNumber);
    $('#blackArmyEnemiesKilled').text(blackStat.EnemiesKilled);
}
function addClassForCell(i, j, map) {
    switch (map[i][j]) {
        case 0:
            return "freeSpace";
        case 1:
            return "food";
        case 2:
            return "unitWhite";
        case 4:
            return "baseWhite";
        case 3:
            return "unitBlack";
        case 5:
            return "baseBlack";
        case 6:
            return "brick";
    }
}

$(function () { // delete request to cancell tournament
    $('#cancelltournament').click(function () {
        webCaller.CancellTournament();
        EnableButtons();
        ;
    });
});
$(function () { // code which happens after changing drawing slider
    var el;
    $("#drawTimeSlider").change(function () {
        el = $(this);
        el
            .next("#drawTimeOutput")
            .text(el.val());
        if (fieldChangeTimer !== undefined) {
            clearInterval(fieldChangeTimer);
            fieldChangeTimer = setInterval(drawingTable, el.val());
        }
    })
        .trigger('change');
})
$(function () { // code which happens after changing turn time slider
    var el;
    $("#turnsTimeSlider").change(function () {
        el = $(this);
        el
            .next("#turnsTimeOutput")
            .text(el.val());
        webCaller.PutData(el.val());
    })
        .trigger('change');
});
$(function () { //Posting algorithm for white army
    $("#loadAlgN1").change(function () {
        var reader = new FileReader();
        reader.onload = function () {

            var arrayBuffer = this.result,
                array = new Uint8Array(arrayBuffer);
            webCaller.PostArray("white",array);
        }
        reader.readAsArrayBuffer(this.files[0]);
    })
})
$(function () { // posting algorithm for black army
    $("#loadAlgN2").change(function () {
        var reader = new FileReader();
        reader.onload = function () {

            var arrayBuffer = this.result,
                array = new Uint8Array(arrayBuffer);
            webCaller.PostArray("black",array);
        }
        reader.readAsArrayBuffer(this.files[0]);
    })
})
function DisableButtons(){
    $("#loadAlgN1").prop('disabled',true);
    $("#loadAlgN2").prop('disabled',true);
}
function EnableButtons(){
    $("#loadAlgN1").prop('disabled',false);
    $("#loadAlgN2").prop('disabled',false);
}