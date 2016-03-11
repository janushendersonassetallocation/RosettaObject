var rObj = (function() {
    function isInt(value) {
        var x;
        if (isNaN(value)) {
            return false;
        }
        if (typeof value !== "number"){
            return false;
        }
        x = parseFloat(value);
        return (x | 0) === x;
    }

    function isDate(value) {
        return Object.prototype.toString.call(value) === '[object Date]'
    }

    function hasTimeZone(isoStr){
        return(isoStr.includes("."))
    }

    function isMidnightStr(isoStr){
        var timeArr = isoStr.split(".")[0].split("T")[1].split(":");
        function sumReduce(a, b){
            return a + b
        }
        var out = timeArr.map(function(x){return x == "00"}).reduce(sumReduce);
        return out == 3
    }

    function isMidnightDate(dt){
        return dt.getMinutes() == 0 && dt.getHours() == 0  && dt.getSeconds() == 0
    }

    function datetimeFromString(isoStr){
        if(isMidnightStr(isoStr) && !hasTimeZone(isoStr)){
            var d = new Date(isoStr);
            d.setTime( d.getTime() + d.getTimezoneOffset()*60*1000 );
        }else{
            d = new Date(isoStr);
        }
        return d
    }

    function datetimeToString(dt){
        if(isMidnightDate(dt)){
            dt.setTime( dt.getTime() - dt.getTimezoneOffset()*60*1000 );
            var dtStr = dt.toISOString();
            return dtStr.split(".")[0]
        }else{
            return dt.toISOString()
        }

    }

    function toRobj(jsobj) {
        if (typeof jsobj == "boolean") {
            return {type: "bool", value: jsobj}
        } else if (isInt(jsobj)) {
            return {type: "int", value: jsobj}
        } else if (typeof jsobj === "number" && !isInt(jsobj)) {
            return {type: "float", value: jsobj}
        } else if (typeof jsobj === "string") {
            return {type: "string", value: jsobj}
        } else if (isDate(jsobj)) {
            return {type: "datetime", value: datetimeToString(jsobj)}
        } else if (jsobj.constructor === koala.NumericArray) {
            if (jsobj.dtype === "int") {
                return {type: "intarray", shape: jsobj.shape, values: koala.flatten2d(jsobj.values)}
            }
        } else if (jsobj.constructor === koala.NumericArray) {
            if (jsobj.dtype === "float") {
                return {type: "array", shape: jsobj.shape, values: koala.flatten2d(jsobj.values)}
            }
        } else if (Object.prototype.toString.call(jsobj) === '[object Array]'){
            var vals = []
            jsobj.forEach(function(entry){
                vals.push(toRobj(entry));
            });
            return {type: "list", values: vals}
        } else if (Object.prototype.toString.call(jsobj) === "[object Object]"
            && jsobj.constructor !== koala.NumericArray
            && jsobj.constructor !== koala.Index) {
            var retVal = {};
            for (var key in jsobj) {
                retVal[key] = toRobj(jsobj[key]);
            }
            return {type: "dict", values: retVal}
        } else if (jsobj.constructor === koala.Index && jsobj.dtype === 'datetime') {
            return {
                type: "datetimeindex", values: jsobj.map(function (x) {
                    return datetimeToString(x)
                })
            }
        } else if (jsobj.constructor === koala.Index && jsobj.dtype === 'int') {
            return {type: "intindex", values: jsobj.values}
        } else if (jsobj.constructor === koala.Index && jsobj.dtype === 'object') {
            return {type: "stringindex", values: jsobj.values}
        } else if (jsobj.constructor === koala.DataFrame) {
            return {
                type: "dataframe",
                data: koala.flatten2d(jsobj.data),
                index: toRobj(jsobj.index),
                columns: toRobj(jsobj.columns)
            }
        } else {
            throw new Error("UnexpectedTypeError");
        }
    }

    function toJsobj(robj) {
        switch (robj.type) {
            case "bool":
                return Boolean(robj.value);
                break;
            case "int":
                return parseInt(robj.value, 10);
                break;
            case "float":
                return Number(robj.value);
                break;
            case "string":
                return robj.value;
                break;
            case "datetime":
                return datetimeFromString(robj.value);
                break;
            case "array":
                if (robj.shape[1] !== undefined){
                    var vals = koala.reshape2dRows(robj.values, robj.shape[0], robj.shape[1]);
                }else{
                    var vals = robj.values;
                }
                return new koala.NumericArray(vals, "float");
                break;
            case "intarray":
                if (robj.shape[1] !== undefined){
                    var vals = koala.reshape2dRows(robj.values, robj.shape[0], robj.shape[1]);
                }else{
                    var vals = robj.values;
                }
                return new koala.NumericArray(vals, "int");
                break;
            case "list":
                var out = [];
                robj.values.forEach(function(entry){
                    out.push(toJsobj(entry));
                });
                return out;
                break;
            case "dict":
                var out = {};
                for (var key in robj.values){
                    out[key] = toJsobj(robj.values[key]);
                }
                return out;
                break;
            case "datetimeindex":
                return new koala.Index(robj.values.map(function (x) {
                    return datetimeFromString(x)
                }), "datetime");
                break;
            case "intindex":
                return new koala.Index(robj.values, "int");
                break;
            case "stringindex":
                return new koala.Index(robj.values, "object");
                break;
            case "dataframe":
                var index = toJsobj(robj.index);
                var columns = toJsobj(robj.columns);
                var data = koala.reshape2dCols(robj.data, index.length(), columns.length());
                return new koala.DataFrame(data, index, columns);
                break;
            default:
                throw new Error("UnexpectedTypeError");
        }
    }

    return {
        toJsobj: toJsobj,
        toRobj: toRobj
    }
}());