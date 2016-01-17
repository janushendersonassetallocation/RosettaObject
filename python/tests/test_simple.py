from nose2.tools.params import params
import rosettaobject
import numpy as np
import pandas as pd
from datetime import datetime

bool_tests = [
    (True, '{"type": "bool", "value": true}'),
    (False, '{"type": "bool", "value": false}'),
]
int_tests = [
    (0, '{"type": "int", "value": 0}'),
    (1, '{"type": "int", "value": 1}'),
    (73, '{"type": "int", "value": 73}')
]
float_tests = [
    (0.0, '{"type": "float", "value": 0.0}'),
    (3.142, '{"type": "float", "value": 3.142}')
]
string_tests = [
    ("", '{"type": "string", "value": ""}'),
    ("How now brown cow?", '{"type": "string", "value": "How now brown cow?"}')
]
datetime_tests = [
    (datetime(2015, 12, 25), '{"type": "datetime", "value": "2015-12-25T00:00:00"}')
]
array_tests = [
    (np.array([], dtype=int), '{"values": [], "shape": [0], "type": "intarray"}'),
    (np.array([0, 1, 2]), '{"values": [0, 1, 2], "shape": [3], "type": "intarray"}'),
    (np.array([[0, 1], [2, 3]]), '{"values": [0, 1, 2, 3], "shape": [2, 2], "type": "intarray"}'),
    (np.array([]), '{"values": [], "shape": [0], "type": "array"}'),
    (np.array([0., 1., 2.]), '{"values": [0.0, 1.0, 2.0], "shape": [3], "type": "array"}'),
    (np.array([[0., 1.], [2., 3.]]), '{"values": [0.0, 1.0, 2.0, 3.0], "shape": [2, 2], "type": "array"}'),
]
list_tests = [
    ([], '{"values": [], "type": "list"}'),
    ([0, 1, 2],
     '{"values": [{"type": "int", "value": 0}, {"type": "int", "value": 1}, {"type": "int", "value": 2}], "type": "list"}'),
    (['a', 'b', 'c'],
     '{"values": [{"type": "string", "value": "a"}, {"type": "string", "value": "b"}, {"type": "string", "value": "c"}], "type": "list"}'),
    ([[0], [0, 1]],
     '{"values": [{"values": [{"type": "int", "value": 0}], "type": "list"}, {"values": [{"type": "int", "value": 0}, {"type": "int", "value": 1}], "type": "list"}], "type": "list"}'),
]
dict_tests = [
    ({}, '{"values": {}, "type": "dict"}'),
    ({'a': 1}, '{"values": {"a": {"type": "int", "value": 1}}, "type": "dict"}'),
    ({'a': [1.0], 'b': [2.0]},
     '{"values": {"a": {"values": [{"type": "float", "value": 1.0}], "type": "list"}, "b": {"values": [{"type": "float", "value": 2.0}], "type": "list"}}, "type": "dict"}'),
]
index_tests = [
    (pd.Int64Index([1, 2], dtype='int64'), '{"values": [1, 2], "type": "intindex"}'),
    (pd.Index([u'A', u'B'], dtype='object'), '{"values": ["A", "B"], "type": "stringindex"}'),
    (pd.DatetimeIndex(['2015-12-25', '2015-12-26'], dtype='datetime64[ns]', freq=None, tz=None),
     '{"values": ["2015-12-25T00:00:00", "2015-12-26T00:00:00"], "type": "datetimeindex"}')
]
dataframe_tests = [
    (pd.DataFrame([[1., 2.], [3., 4.]], [0, 1], ["A", "B"]),
     '{"data": [1.0, 2.0, 3.0, 4.0], "type": "dataframe", "columns": {"values": ["A", "B"], "type": "stringindex"}, "index": {"values": [0, 1], "type": "intindex"}}')
]
tests = bool_tests + int_tests + float_tests + string_tests + datetime_tests + array_tests + list_tests + dict_tests + index_tests + dataframe_tests


def is_equal(a, b):
    if isinstance(a, np.ndarray):
        return np.array_equal(a, b)
    if isinstance(a, pd.Index):
        return np.array_equal(a, b)
    if isinstance(a, pd.DataFrame):
        return a.equals(b)
    return a == b


@params(*tests)
def test_serialize(value, s):
    assert is_equal(rosettaobject.dumps(value), s)


@params(*tests)
def test_deserialize(value, s):
    assert is_equal(rosettaobject.loads(s), value)
