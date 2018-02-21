class WebServiceCaller {

    constructor(url) {
        this.url = url;
    }
    GetGameStats() {
        return $.get(this.url + "/api/tournament");
    }
    GetNumberOfPages(){
        if(filterManager.FilterDateBefore==="")
            filterManager.FilterDateBefore = "9999-12-12T23:00"
        if(filterManager.FilterDateAfter==="")
            filterManager.FilterDateAfter = "0001-12-12T23:00"
        return $.get(this.url + "/api/statistic/count"+
        "?$filter=(substringof('"+filterManager.FilterName+"', WhiteAlgorithmName) eq true or "+
        "substringof('"+filterManager.FilterName+"', BlackAlgorithmName) eq true)"+
        "and GameStartTime gt DateTime'"+filterManager.FilterDateAfter+"'and GameStartTime lt DateTime'"+filterManager.FilterDateBefore+"'");
    }
    GetAlgorithmName(algName) {
        if (algName === "white") {
            return $.get(this.url + "/api/algorithm/white");
        }
        else {
           return $.get(this.url + "/api/algorithm/black");
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
    GetData() {
        if(filterManager.FilterDateBefore==="")
            filterManager.FilterDateBefore = "9999-12-12T23:00"
        if(filterManager.FilterDateAfter==="")
        filterManager.FilterDateAfter = "0001-12-12T23:00"
        return $.get( this.url + "/api/statistic"+
        "?$orderby="+filterManager.Parametr+" "+filterManager.OrderType+"&$skip="+(paginationManager.PageNumber-1)*paginationManager.EntriesNumber+"&$top="+paginationManager.EntriesNumber+
        "&$filter=(substringof('"+filterManager.FilterName+"', WhiteAlgorithmName) eq true or "+
        "substringof('"+filterManager.FilterName+"', BlackAlgorithmName) eq true) "+
        "and GameStartTime gt DateTime'"+filterManager.FilterDateAfter+"'and GameStartTime lt DateTime'"+filterManager.FilterDateBefore+"'");
    }
}