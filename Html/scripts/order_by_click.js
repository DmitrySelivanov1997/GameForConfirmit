async function TdOnClick() {
    var filterName = $('#algorithmName').val();
    var filterDate= $('#date').val();
    switch (this.className) {
        case 'Id':
            var stats = await webCaller.GetDataAfterOrdering(pageNumber, "Id", orderType, entriesNumber,filterName,filterDate)
            break;
        case 'MapSize':
            var stats = await webCaller.GetDataAfterOrdering(pageNumber, "MapSize", orderType, entriesNumber,filterName,filterDate)
            break;
        case 'GameStartTime':
            var stats = await webCaller.GetDataAfterOrdering(pageNumber, "GameStartTime", orderType, entriesNumber,filterName,filterDate)
            break;
        case 'GameDuration':
            var stats = await webCaller.GetDataAfterOrdering(pageNumber, "GameDuration", orderType, entriesNumber,filterName,filterDate)
            break;
        case 'GameResult':
            var stats = await webCaller.GetDataAfterOrdering(pageNumber, "GameResult", orderType, entriesNumber,filterName,filterDate)
            break;
        case 'TurnsNumber':
            var stats = await webCaller.GetDataAfterOrdering(pageNumber, "TurnsNumber", orderType, entriesNumber,filterName,filterDate)
            break;
        case 'WhiteAlgorithmname':
            var stats = await webCaller.GetDataAfterOrdering(pageNumber, "WhiteAlgorithmName", orderType, entriesNumber,filterName,filterDate)
            break;
        case 'BlackAlgorithmName':
            var stats = await webCaller.GetDataAfterOrdering(pageNumber, "BlackAlgorithmName", orderType, entriesNumber,filterName,filterDate)
            break;
        default:
            break;
    }
    if (orderType === "desc")
        orderType = "asc";
    else
        orderType = "desc";
    DrawTable(stats);
}