#!/bin/bash

export NODE_PATH="$(pwd)"
while :
do
  node server.js | tee ./logs/server.$(date +%Y_%m_%d_%H_%M).log
done
