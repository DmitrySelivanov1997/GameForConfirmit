$( function() {
        $('#getStats').click( function() {
        $('#here_table').empty();
          $.get( "http://co-yar-ws100:8080/api/tournament", function( stats ) {
        var map = stats.Map;
        var table = $('<table></table>').addClass('gameField').css("width","100%");
        for(i=0; i<map.length; i++){
            var row = $('<tr></tr>');
            for(j=0;j<map.length;j++){
                row.append($('<td></td>').css("background-color","#D3EDF6"));
            }
            table.append(row);
        }
        table = table.css("height","100%")
        $('#here_table').append(table)
    });
        } );
      } );