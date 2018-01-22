$( function() {
        $('#getAlgorithm').click( function() {
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
        } );
      } );