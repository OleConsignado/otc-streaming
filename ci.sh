#!/bin/bash

set -e

export CLASS_LIBRARY_PROJ_DIR=Source/Otc.Streaming
export TEST_PROJ_DIR=Source/Otc.Streaming.Tests

cd $TRAVIS_BUILD_DIR
wget https://raw.githubusercontent.com/OleConsignado/otc-cicd-common/3acb270e4000bf455519b4f5cc912805fcbcdaff/cicd-common.sh
chmod +x ./cicd-common.sh

./cicd-common.sh $@
