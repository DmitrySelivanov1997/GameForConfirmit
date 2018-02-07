function TdOnClick(){
    switch(this.className) {
        case 'Id': 
            webCaller.GetDataAfterOrdering({pageNumber:pageNumber, orderClass:"Id", orderType: orderType})
            break;
        case 'value2': 
            break;
        default:
            break;
      }
}