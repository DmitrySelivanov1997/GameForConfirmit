
$( function() {
    $('#getAlgorithm').click( function() {
      var data = "white"
      $.get( "http://co-yar-ws100:8080/api/algorithm/white", success);
    } );
    function success( algorithm ) {
      alert(algorithm);
    }
  } );