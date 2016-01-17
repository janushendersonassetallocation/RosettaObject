import json
import numpy as np
import pandas as pd
from datetime import datetime
import dateutil.parser


class RosettaException(Exception): pass
class UnexpectedTypeError(RosettaException): pass


def dumps(obj):
    json_obj = to_robj(obj)
    return json.dumps(json_obj)


def loads(s):
    json_obj = json.loads(s)
    return to_pyobj(json_obj)


def to_robj(pyobj):
    if isinstance(pyobj, bool):
        return {"type": "bool", "value": pyobj}
    elif isinstance(pyobj, int):
        return {"type": "int", "value": pyobj}
    elif isinstance(pyobj, float):
        return {"type": "float", "value": pyobj}
    elif isinstance(pyobj, str):
        return {"type": "string", "value": pyobj}
    elif isinstance(pyobj, datetime):
        return {"type": "datetime", "value": pyobj.isoformat()}
    elif isinstance(pyobj, np.ndarray) and np.issubdtype(pyobj.dtype, float):
        return {"type": "array", "shape": [int(x) for x in pyobj.shape], "values": list(pyobj.flatten())}
    elif isinstance(pyobj, np.ndarray) and np.issubdtype(pyobj.dtype, int):
        return {"type": "intarray", "shape": [int(x) for x in pyobj.shape], "values": list(pyobj.flatten())}
    elif isinstance(pyobj, (tuple, list)):
        return {"type": "list", "values": [to_robj(x) for x in pyobj]}
    elif isinstance(pyobj, dict):
        if not all(isinstance(k, str) for k in pyobj.iterkeys()):
            return UnexpectedTypeError()
        return {"type": "dict", "values": {k: to_robj(v) for k, v in pyobj.iteritems()}}
    elif isinstance(pyobj, pd.DatetimeIndex):
        return {"type": "datetimeindex", "values": pyobj.map(datetime.isoformat).tolist()}
    elif isinstance(pyobj, pd.Int64Index):
        return {"type": "intindex", "values": pyobj.values.tolist()}
    elif isinstance(pyobj, pd.Index):
        return {"type": "stringindex", "values": pyobj.values.tolist()}
    elif isinstance(pyobj, pd.DataFrame):
        return {"type": "dataframe",
                "data": pyobj.values.flatten().tolist(),
                "index": to_robj(pyobj.index),
                "columns": to_robj(pyobj.columns)}
    else:
        raise UnexpectedTypeError()


def to_pyobj(robj):
    vtype = robj['type']
    if vtype == 'bool':
        return bool(robj['value'])
    elif vtype == 'int':
        return int(robj['value'])
    elif vtype == 'float':
        return float(robj['value'])
    elif vtype == 'string':
        return robj['value']
    elif vtype == 'datetime':
        return dateutil.parser.parse(robj['value'])
    elif vtype == 'array':
        arr = np.array(robj['values'])
        arr = arr.reshape(robj['shape'])
        return arr
    elif vtype == 'intarray':
        arr = np.array(robj['values'])
        arr = arr.reshape(robj['shape'])
        return arr
    elif vtype == 'list':
        return [to_pyobj(x) for x in robj['values']]
    elif vtype == 'dict':
        return {k: to_pyobj(v) for k, v in robj['values'].iteritems()}
    elif vtype == 'datetimeindex':
        return pd.DatetimeIndex([dateutil.parser.parse(x) for x in robj['values']], dtype='datetime64[ns]')
    elif vtype == 'intindex':
        return pd.Int64Index(robj['values'], dtype='int64')
    elif vtype == 'stringindex':
        return pd.Index(robj['values'], dtype='object')
    elif vtype == 'dataframe':
        index = to_pyobj(robj['index'])
        columns = to_pyobj(robj['columns'])
        data = np.array(robj['data']).reshape((len(index), len(columns)))
        return pd.DataFrame(data, index, columns)
    else:
        raise UnexpectedTypeError()
