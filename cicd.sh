#!/bin/bash

set -e

export CICD_COMMON_REVISION=3acb270e4000bf455519b4f5cc912805fcbcdaff
export CLASS_LIBRARY_PROJ_DIR=Source/Otc.Streaming
export TEST_PROJ_DIR=Source/Otc.Streaming.Tests

cd $TRAVIS_BUILD_DIR

wget -q https://raw.githubusercontent.com/OleConsignado/otc-cicd-common/$CICD_COMMON_REVISION/cicd-common.sh -O ./cicd-common.sh
chmod +x ./cicd-common.sh
./cicd-common.sh $@
