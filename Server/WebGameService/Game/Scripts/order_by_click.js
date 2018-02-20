$(document).on('click','.buttonHead',async function(){
    filterName = $('#algorithmName').val();
    filterDateBefore = $('#dateBefore').val();
    filterDateAfter = $('#dateAfter').val();
    if (orderType === "desc")
        orderType = "asc";
    else
        orderType = "desc";
    parametr = this.id;
    this.classList.add("orderedDesc");
    var stats = await webCaller.GetData(pageNumber, parametr, orderType, entriesNumber, filterName, filterDateBefore, filterDateAfter);
    DrawTable(stats);
    });