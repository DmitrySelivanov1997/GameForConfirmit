var timerId;
$( function() {
        $('#startTournament').click( function() {
            GetAlgorithmsNames();
            PostDataToTheServer();
        } );
    } );
function GetAlgorithmsNames(){
    var firstAlg = $('#firstAlgorithm');
    var firstAlgName = $('#firstAlgorithmName');
    var secondAlg = $('#secondAlgorithm');
    var secondAlgName = $('#secondAlgorithmName');
    $.get( "http://co-yar-ws100:8080/api/algorithm/white", function( algorithm ) {
        firstAlg.text(algorithm);
        firstAlgName.text(algorithm);
    });
    $.get( "http://co-yar-ws100:8080/api/algorithm/black", function( algorithm ) {
    secondAlg.text(algorithm);
    secondAlgName.text(algorithm);
    });
}
function PostDataToTheServer(){
    var mapSize = $('#mapSize').val();
    var numberOfGames = $('#numberOfMatches').val();
    var WaitTime = $('#turnsTimeSlider').val();
    $.post("http://co-yar-ws100:8080/api/tournament/start/", { MapSize: mapSize, NumberOfGames: numberOfGames, WaitTime:WaitTime }, CreateTable );
}

function CreateTable() {
        $('#here_table').empty();
        $.get( "http://co-yar-ws100:8080/api/tournament", function( stats ) {
            var map = stats.Map;
            var table = $('<table></table>').attr('id', 'gameField').css({"width":"100%","height":"100%","margin":"auto"});
            for(i=0; i<map.length; i++){
                var row = $('<tr></tr>');
                for(j=0;j<map.length;j++){
                      row.append($('<td></td>').css("background-color",findColorForCell(i,j,map)));
                }
                 table.append(row);
                }
            $('#here_table').append(table)
            timerId = setInterval(drawingTable, 100);
    });
}

function drawingTable(){
         $.get( "http://co-yar-ws100:8080/api/tournament", function( stats ) {
            var map = stats.Map;
            var table = $('#gameField ');
            if(map === null ){
                clearInterval(timerId);
                return;
            }
            for(i=0; i<map.length; i++){
                var row = $('<tr></tr>');
                for(j=0;j<map.length;j++){
                    $(table[0].rows[i].cells[j]).css("background-color",findColorForCell(i,j,map));
                }
            }
            var whiteStatistics = stats.WhiteStatistics;
            var blackStatistics = stats.BlackStatistics;
            UpdateStats(whiteStatistics, blackStatistics);
        }
    )
}
function UpdateStats(whiteStat, blackStat){
    $('#numberOfTurns').text(whiteStat.TurnNumber);
    $('#score').text(whiteStat.NumberOfWins+':'+blackStat.NumberOfWins);

    $('#whiteArmyFoodEaten').text(whiteStat.FoodEaten);
    $('#whiteArmyCurrentUnits').text(whiteStat.CurrentArmyNumber);
    $('#whiteArmyEnemiesKilled').text(whiteStat.EnemiesKilled);
    
    $('#blackArmyFoodEaten').text(blackStat.FoodEaten);
    $('#blackArmyCurrentUnits').text(blackStat.CurrentArmyNumber);
    $('#blackArmyEnemiesKilled').text(blackStat.EnemiesKilled);
}
function findColorForCell(i,j,map){
    switch(map[i][j]) {
        case 0:
            return "#FFE4C4";
        case 1:
            return "#008000";
        case 2:
        case 4:
            return "#FFFFFF";
        case 3:
        case 5:
            return "#000000";
        case 6:
            return "#A52A2A"
    }
}

$( function() {
        $('#cancelltournament').click( function() {
        $.ajax({
            url: 'http://co-yar-ws100:8080/api/tournament',
            type: 'DELETE',
        });
    });
});