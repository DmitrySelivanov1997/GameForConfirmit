$( function() {
        $('#getStats').click( function() {
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
                var timerId = setInterval(drawingTable, 100);
             });
        })
 })

function drawingTable(){
         $.get( "http://co-yar-ws100:8080/api/tournament", function( stats ) {
            var map = stats.Map;
            var table = $('#gameField ');
            if(map === undefined ){
                clearInterval(timerId);
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