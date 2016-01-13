import json
import numpy as np
import pandas as pd
from datetime import datetime
import dateutil.parser


class VObjException(Exception): pass
class UnexpectedTypeError(VObjException): pass


def dumps(obj):
    json_obj = to_canonical(obj)
    return json.dumps(json_obj)


def loads(s):
    json_obj = json.loads(s)
    return from_canonical(json_obj)


def to_canonical(obj):
    if isinstance(obj, bool):
        return {"type": "bool", "value": obj}
    elif isinstance(obj, int):
        return {"type": "int", "value": obj}
    elif isinstance(obj, float):
        return {"type": "float", "value": obj}
    elif isinstance(obj, str):
        return {"type": "string", "value": obj}
    elif isinstance(obj, datetime):
        return {"type": "datetime", "value": obj.isoformat()}
    elif isinstance(obj, np.ndarray) and np.issubdtype(obj.dtype, float):
        return {"type": "array", "shape": obj.shape, "values": list(obj.flatten())}
    elif isinstance(obj, np.ndarray) and np.issubdtype(obj.dtype, int):
        return {"type": "intarray", "shape": obj.shape, "values": list(obj.flatten())}
    elif isinstance(obj, (tuple, list)):
        return {"type": "list", "values": [to_canonical(x) for x in obj]}
    elif isinstance(obj, dict):
        if not all(isinstance(k, str) for k in obj.iterkeys()):
            return UnexpectedTypeError()
        return {"type": "dict", "values": {k: to_canonical(v) for k, v in obj.iteritems()}}
    elif isinstance(obj, pd.DatetimeIndex):
        return {"type": "datetimeindex", "values": obj.map(datetime.isoformat).tolist()}
    elif isinstance(obj, pd.Int64Index):
        return {"type": "intindex", "values": obj.values.tolist()}
    elif isinstance(obj, pd.Index):
        return {"type": "stringindex", "values": obj.values.tolist()}
    elif isinstance(obj, pd.DataFrame):
        return {"type": "dataframe",
                "data": obj.values.flatten().tolist(),
                "index": to_canonical(obj.index),
                "columns": to_canonical(obj.columns)}
    else:
        raise UnexpectedTypeError()


def from_canonical(json_obj):
    vtype = json_obj['type']
    if vtype == 'bool':
        return bool(json_obj['value'])
    elif vtype == 'int':
        return int(json_obj['value'])
    elif vtype == 'float':
        return float(json_obj['value'])
    elif vtype == 'string':
        return json_obj['value']
    elif vtype == 'datetime':
        return dateutil.parser.parse(json_obj['value'])
    elif vtype == 'array':
        arr = np.array(json_obj['values'])
        arr.shape = json_obj['shape']
        return arr
    elif vtype == 'intarray':
        arr = np.array(json_obj['values'])
        arr.shape = json_obj['shape']
        return arr
    elif vtype == 'list':
        return [from_canonical(x) for x in json_obj['values']]
    elif vtype == 'dict':
        return {k: from_canonical(v) for k, v in json_obj['values'].iteritems()}
    elif vtype == 'datetimeindex':
        return pd.DatetimeIndex([dateutil.parser.parse(x) for x in json_obj['values']], dtype='datetime64[ns]')
    elif vtype == 'intindex':
        return pd.Int64Index(json_obj['values'], dtype='int64')
    elif vtype == 'stringindex':
        return pd.Index(json_obj['values'], dtype='object')
    elif vtype == 'dataframe':
        index = from_canonical(json_obj['index'])
        columns = from_canonical(json_obj['columns'])
        data = np.array(json_obj['data']).reshape((len(index), len(columns)))
        return pd.DataFrame(data, index, columns)
    else:
        raise UnexpectedTypeError()
