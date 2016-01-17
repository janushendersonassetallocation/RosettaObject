import json
from nose2.tools.params import params
import rosettaobject
import numpy as np
import pandas as pd
from datetime import datetime
import py


def load_json(name):
    path = py.path.local().join('..', 'test_data', name)
    with path.open('r') as f:
        obj = json.load(f)
        return obj

std_robjs = load_json('std_objects.json')
std_pyobjs = [
    True, False,
    0, 1, 73,
    0.0, 3.142,
    "", "How now brown cow?",
    datetime(2015, 12, 25),

    np.array([], dtype=int),
    np.array([0, 1, 2]),
    np.array([[0, 1], [2, 3]]),
    np.array([]),
    np.array([0., 1., 2.]),
    np.array([[0., 1.], [2., 3.]]),

    [], [0, 1, 2], ['a', 'b', 'c'], [[0], [0, 1]],
    {}, {'a': 1}, {'a': [1.0], 'b': [2.0]},

    pd.Int64Index([1, 2], dtype='int64'),
    pd.Index([u'A', u'B'], dtype='object'),
    pd.DatetimeIndex(['2015-12-25', '2015-12-26'], dtype='datetime64[ns]', freq=None, tz=None),

    pd.DataFrame([[1., 2.], [3., 4.]], [0, 1], ["A", "B"])
]


def is_equal(a, b):
    if isinstance(a, np.ndarray):
        return np.array_equal(a, b)
    if isinstance(a, pd.Index):
        return np.array_equal(a, b)
    if isinstance(a, pd.DataFrame):
        return a.equals(b)
    return a == b


@params(*zip(std_pyobjs, std_robjs))
def test_to_robj(obj, expected_robj):
    actual_robj = rosettaobject.to_robj(obj)
    assert actual_robj == expected_robj


@params(*zip(std_pyobjs, std_robjs))
def test_to_pyobj(expected_pyobj, robj):
    actual_pyobj = rosettaobject.to_pyobj(robj)
    assert is_equal(actual_pyobj, expected_pyobj)
