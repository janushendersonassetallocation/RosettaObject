var koala = (function() {
    function isInt(value) {
        var x;
        if (isNaN(value)) {
            return false
        }
        if(Object.prototype.toString.call( value ) === '[object Array]'){
            return false
        }
        x = parseFloat(value);
        return (x | 0) === x;
    }

    function isDate(value) {
        return Object.prototype.toString.call(value) === '[object Date]'
    }

    function collapseType(past, x) {
        return Math.max(past, x)
    }

    function numCheck(x) {
        var curType = 4;
        var intCheck = isInt(x);
        if (intCheck) {
            curType = 1;
        }
        if (typeof x === "number" && !intCheck) {
            curType = 2;
        }
        if (x === null){
            curType = 0;
        }
        return curType
    }

    function dateCheck(x) {
        var curType = 3;
        if (!isDate(x)) {
            curType = 4;
        }
        return curType
    }

    function numToType(x) {
        var mapvals = {0: "float", 1: "int", 2: "float", 3: "datetime", 4: "object"};
        return mapvals[x]
    }

    function _inferType(x) {
        if (x.length == 0){
            return 1
        }else if(Object.prototype.toString.call( x[0] ) === '[object Array]'){
            var arrCheck = []
            x.forEach(function(entry){
            arrCheck.push(_inferType(entry));
        });
            return arrCheck.reduce(collapseType);
        }else{
            var checkNum = x.map(numCheck).reduce(collapseType)
        }
        if (checkNum == 4) {
            checkNum = x.map(dateCheck).reduce(collapseType);
        }
        return checkNum
    }

    function inferType(x){
        return numToType(_inferType((x)))
    }

    function Index(vals, dtype) {
        if (!!dtype) {
            this.dtype = dtype;
        } else {
            this.dtype = inferType(vals);
        }
        this.values = vals;
        this.length = function () {
            return this.values.length
        };
    }

    function NumericArray(vals, dtype) {
        this.values = vals;

        if (!!dtype) {
            this.dtype = dtype;
        } else {
            this.dtype = inferType(vals);
        }
        this.length = function () {
            return this.values.length
        };
    }

    function flatten2d(arr) {
        return arr.reduce(function (last, x) {
            return last.concat(x)
        })
    }

    function reshape2dRows(arr, x, y) {
        out = [];
        for (var i = 0; i < y; i++) {
            out.push(arr.slice(i * x, (i + 1) * x));
        }
        return out
    }

    function reshape2dCols(arr, x, y){
        var col = 0;
        var row = 0;
        var out = [];
        arr.forEach(function(entry){
            if(col == y){
                col = 0;
              row++;
            }
            if(row == 0){
                out.push([]);
            }
            out[col][row] = entry;
            col++;
        });
        return out
    }

    function arrList(n) {
        var out = [];
        for (var i = 0; i < n; i++) {
            out.push(i);
        }
        return out
    }

    function DataFrame(data, index, columns, dtypes, indexDtype) {
        if (data.constructor === NumericArray){
            this.data = data.values;
        }else{
            this.data = data;
        }

        if (!!index) {
            if (index.constructor == Index){
                this.index = index;
            }else{
                this.index = new Index(index, indexDtype);
            }
        } else {
            this.index = new Index(arrList(this.data[0].length), "int");
        }
        if (!!columns) {
            if (columns.constructor == Index){
                this.columns = columns;
            }else{
                this.columns = new Index(columns);
            }
        } else {
            this.columns = new Index(arrList(data.length), "int");
        }
        if (!!dtypes){
            this.dtypes = dtypes;
        }else{
            this.dtypes = [];
            for(var i = 0; i < this.data.length; i++){
               this.dtypes.push(inferType(this.data[i]));
            }
        }

        this.shape = [];
        this.shape.push(this.data[0].length);
        this.shape.push(this.data.length);
    }

    return {
        DataFrame: DataFrame,
        reshape2dRows: reshape2dRows,
        reshape2dCols: reshape2dCols,
        flatten2d: flatten2d,
        NumericArray: NumericArray,
        Index: Index
    }
}());