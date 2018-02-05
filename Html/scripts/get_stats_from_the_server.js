var webCaller = new WebServiceCaller("http://co-yar-ws100:8080");
GetStats();
async function GetStats (){
    var stats = await webCaller.GetGameSessionStatistic();
    for (var i = 0, len = stats.length; i < len; i++) {
        var c = stats[i];
      }
}