$( function() {
        $('#getAlgorithm').click( function() {
          var firstAlg = $('#firstAlgorithm');
          var secondAlg = $('#secondAlgorithm');
          $.get( "http://co-yar-ws100:8080/api/algorithm/white", function( algorithm ) {
        firstAlg.text(algorithm);
    });
          $.get( "http://co-yar-ws100:8080/api/algorithm/black", function( algorithm ) {
        secondAlg.text(algorithm);
    });
        } );
      } );