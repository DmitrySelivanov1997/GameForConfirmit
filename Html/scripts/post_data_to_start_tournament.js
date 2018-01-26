var fieldChangeTimer;
class WebServiceCaller {

    constructor(url) {
      this.url = url;
    }
  
    Get(address,func){
        $.get( this.url+address, func);
    }
    PostArray(array,address) {
        var xhr = new XMLHttpRequest;
        xhr.open("POST", this.url+address, true);
        xhr.send(array);
    }
    PostData(address,data,func){
        $.post(this.url+address, data, func );
    }
    CancellTournament(address){
        $.ajax({
            url: this.url+address,
            type: 'DELETE',
        });
    }
    PutData(address,data){
        $.ajax({
            url: this.url+address,   
            type: 'PUT', 
            data:{
               "" : data
            }
        });
    }
  
  }
var webCaller= new WebServiceCaller("http://co-yar-ws100:8080");
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
    webCaller.Get("/api/algorithm/white", function( algorithm ) {
        firstAlg.text(algorithm);
        firstAlgName.text(algorithm);
    });
    webCaller.Get("/api/algorithm/black", function( algorithm ) {
    secondAlg.text(algorithm);
    secondAlgName.text(algorithm);
    });
}
function PostDataToTheServer(){
    var mapSize = $('#mapSize').val();
    var numberOfGames = $('#numberOfMatches').val();
    var WaitTime = $('#turnsTimeSlider').val();
    webCaller.PostData("/api/tournament/start/", { MapSize: mapSize, NumberOfGames: numberOfGames, WaitTime:WaitTime }, CreateTable );
}

function CreateTable() { //creating a new game field after starting tournament
         $("table").remove()
         webCaller.Get("/api/tournament", function( stats ) {
            var map = stats.Map;
            var table = $('<table></table>').attr('id', 'gameField');
            for(i=0; i<map.length; i++){
                var row = $('<tr></tr>');
                for(j=0;j<map.length;j++){
                    var td=$('<td></td>').css("background-color",findColorForCell(i,j,map));
                    td.css("height",td.width());
                      row.append($('<td></td>').css("background-color",findColorForCell(i,j,map)));
                }
                 table.append(row);
                }
            $('#hereTable').append(table)
            fieldChangeTimer = setInterval(drawingTable, 100);
    });
}

function drawingTable(){ // changing color of table cell within 
         webCaller.Get("/api/tournament", function( stats ) {
            var map = stats.Map;
            var whiteStatistics = stats.WhiteStatistics;
            var blackStatistics = stats.BlackStatistics;
            UpdateStats(whiteStatistics, blackStatistics);
            var table = $('#gameField');
            if(map === null ){
                clearInterval(fieldChangeTimer);
                return;
            }
            for(i=0; i<map.length; i++){
                var row = $('<tr></tr>');
                for(j=0;j<map.length;j++){
                    $(table[0].rows[i].cells[j]).css("background-color",findColorForCell(i,j,map));
                }
            }
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

$( function() { // delete request to cancell tournament
        $('#cancelltournament').click( function() {
        webCaller.CancellTournament("/api/tournament");
        ;
    });
});
$(function() { // code which happens after changing drawing slider
    var el;
    $("#drawTimeSlider").change(function() {
    el = $(this);
    el
    .next("#drawTimeOutput")
    .text(el.val());
    if(fieldChangeTimer !== undefined){
        clearInterval(fieldChangeTimer);
        fieldChangeTimer = setInterval(drawingTable, el.val());
    }
    })
    .trigger('change');
})
 $(function() { // code which happens after changing turn time slider
    var el;
    $("#turnsTimeSlider").change(function() {
    el = $(this);
    el
   .next("#turnsTimeOutput")
   .text(el.val());
   webCaller.PutData("/api/tournament",el.val());
   })
   .trigger('change');
});
$(function(){ //Posting algorithm for white
    $("#loadAlgN1").change(function() {  
      var reader = new FileReader();
      reader.onload = function() {
    
        var arrayBuffer = this.result,
        array = new Uint8Array(arrayBuffer);
        webCaller.PostArray(array,"/api/algorithm/white");
      }
      reader.readAsArrayBuffer(this.files[0]);
    })
})
$(function(){ // posting algorithm for black
  $("#loadAlgN2").change(function() {  
    var reader = new FileReader();
    reader.onload = function() {
  
      var arrayBuffer = this.result,
      array = new Uint8Array(arrayBuffer);
      webCaller.PostArray(array,"/api/algorithm/black");
    }
    reader.readAsArrayBuffer(this.files[0]);
  })
})