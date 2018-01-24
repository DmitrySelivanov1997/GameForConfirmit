$(function(){
    $("#loadAlgN1").change(function() {  
      var reader = new FileReader();
      reader.onload = function() {
    
        var arrayBuffer = this.result,
        array = new Uint8Array(arrayBuffer);
      }
      reader.readAsArrayBuffer(this.files[0]);
    })
})