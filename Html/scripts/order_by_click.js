async function TdOnClick() {
    var filterName = $('#algorithmName').val();
    var filterDate = $('#date').val();
    if (orderType === "desc")
        orderType = "asc";
    else
        orderType = "desc";
    parametr = this.className;
    var stats = await webCaller.GetData(pageNumber, parametr, orderType, entriesNumber, filterName, filterDate)
    DrawTable(stats);
}