class WebServiceCaller {

    constructor(url) {
        this.url = url;
    }

    async GetGameStats() {
        return await $.get(this.url + "/api/tournament");
    }
    GetAlgorithmName(algName) {
        if (algName === "white") {
            $.get(this.url + "/api/algorithm/white", function (name) {
                SetAlgorithmName(name, "white");
            });
        }
        else {
            $.get(this.url + "/api/algorithm/black", function (name) {
                SetAlgorithmName(name, "black");
            });
        }
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

}