$(function(){
    $("#loadAlgN1").change(function() {  
      var reader = new FileReader();
      reader.onload = function() {
    
        var arrayBuffer = this.result,
        array = new Uint8Array(arrayBuffer);
        var xhr = new XMLHttpRequest;
        xhr.open("POST", 'http://co-yar-ws100:8080/api/algorithm/white', true);
        xhr.send(array);
      }
      reader.readAsArrayBuffer(this.files[0]);
    })
})
$(function(){
  $("#loadAlgN2").change(function() {  
    var reader = new FileReader();
    reader.onload = function() {
  
      var arrayBuffer = this.result,
      array = new Uint8Array(arrayBuffer);
      var xhr = new XMLHttpRequest;
      xhr.open("POST", 'http://co-yar-ws100:8080/api/algorithm/black', true);
      xhr.send(array);
    }
    reader.readAsArrayBuffer(this.files[0]);
  })
})