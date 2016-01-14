#!/usr/bin/env python

import imp
import os
from setuptools import setup, find_packages

SRC_DIR = 'src'
SQLNAMEMAPPER_PKG_DIR = os.path.join(SRC_DIR, 'rosettaobject')

version = imp.load_source('version', os.path.join(SQLNAMEMAPPER_PKG_DIR, 'version.py'))

with open('../readme.md') as f:
    readme = f.read()

setup(name='RosettaObject',
      version=version.VERSION_STRING,
      description='Library providing canonical cross-language mapping of certain data types to and from JSON.',
      long_description=readme,
      author='Ed Parcell',
      author_email='edparcell@gmail.com',
      url='',
      package_dir={'': SRC_DIR},
      packages=find_packages(SRC_DIR),
      requires=['numpy', 'pandas']
      )