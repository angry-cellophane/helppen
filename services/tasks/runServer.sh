#!/bin/bash

export NODE_PATH="$(dirname "$0")"
mkdir -p "${NODE_PATH}/logs"
while :
do
  node "${NODE_PATH}"/server.js | tee "${NODE_PATH}/logs/server.$(date +%Y_%m_%d_%H_%M).log"
done
