class WebServiceCaller {

    constructor(url) {
        this.url = url;
    }
    GetGameStats() {
        return $.get(this.url + "/api/tournament");
    }
    GetNumberOfPages(){
        if(filterDateBefore==="")
            filterDateBefore = "9999-12-12T23:00"
        if(filterDateAfter==="")
            filterDateAfter = "0001-12-12T23:00"
        return $.get(this.url + "/api/statistic/count"+
        "?$filter=(substringof('"+filterName+"', WhiteAlgorithmName) eq true or "+
        "substringof('"+filterName+"', BlackAlgorithmName) eq true)"+
        "and GameStartTime gt DateTime'"+filterDateAfter+"'and GameStartTime lt DateTime'"+filterDateBefore+"'");
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
    GetData(pageNumber,parametr,orderType,entriesNumber,filterName, filterDateBefore, filterDateAfter) {
        if(filterDateBefore==="")
            filterDateBefore = "9999-12-12T23:00"
        if(filterDateAfter==="")
            filterDateAfter = "0001-12-12T23:00"
        return $.get( this.url + "/api/statistic"+
        "?$orderby="+parametr+" "+orderType+"&$skip="+(pageNumber-1)*entriesNumber+"&$top="+entriesNumber+
        "&$filter=(substringof('"+filterName+"', WhiteAlgorithmName) eq true or "+
        "substringof('"+filterName+"', BlackAlgorithmName) eq true) "+
        "and GameStartTime gt DateTime'"+filterDateAfter+"'and GameStartTime lt DateTime'"+filterDateBefore+"'");
    }
}