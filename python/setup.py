#!/usr/bin/env python

import imp
import os
from setuptools import setup, find_packages

SRC_DIR = 'src'
SQLNAMEMAPPER_PKG_DIR = os.path.join(SRC_DIR, 'vobj')

version = imp.load_source('version', os.path.join(SQLNAMEMAPPER_PKG_DIR, 'version.py'))

with open('../readme.md') as f:
    readme = f.read()

setup(name='VObj',
      version=version.VERSION_STRING,
      description='Can',
      long_description=readme,
      author='Ed Parcell',
      author_email='edparcell@gmail.com',
      url='',
      package_dir={'': SRC_DIR},
      packages=find_packages(SRC_DIR),
      requires=['numpy', 'pandas']
      )