$( function() {
        $('#mapSize').change(function() {
        var c = this.value;
        if(c<10) c=10;
        if(c>125) c=125;
        this.value=c;
    })
        $('#numberOfMatches').change(function() {
            var c = this.value;
            if(c<1) c=1;
            if(c>250) c=250;
            this.value=c;
        })
})