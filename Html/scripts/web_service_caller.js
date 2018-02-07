class WebServiceCaller {

    constructor(url) {
        this.url = url;
    }
    GetGameStats() {
        return $.get(this.url + "/api/tournament");
    }
    GetAlgorithmName(algName) {
        if (algName === "white") {
            return $.get(this.url + "/api/algorithm/white");
        }
        else {
           return $.get(this.url + "/api/algorithm/black");
        }
    }
    GetGameSessionStatistic(id) {
        return $.get(this.url + "/api/db"+id);
    }
    PostArray(algType, array) {
        if(algType==="white"){
            var xhr = new XMLHttpRequest;
            xhr.open("POST", this.url + "/api/algorithm/white", true);
            xhr.send(array);
        }
        if(algType==="black"){
            var xhr = new XMLHttpRequest;
            xhr.open("POST", this.url + "/api/algorithm/black", true);
            xhr.send(array);
        }
    }
    PostData(data) {
        $.post(this.url + "/api/tournament/start/", data, function (result) {
            CreateTable(result)
        });
    }
    CancellTournament() {
        $.ajax({
            url: this.url + "/api/tournament",
            type: 'DELETE',
        });
    }
    PutData(data) {
        $.ajax({
            url: this.url + "/api/tournament",
            type: 'PUT',
            data: {
                "": data
            }
        });
    }
    GetDataAfterOrdering(data) {
        return $.get( this.url + "/api/db/getstatsbyorder",data);
    }
}