#!/bin/bash

export NODE_PATH="$(dirname "$0")"
node "${NODE_PATH}/archiver.js" | tee "${NODE_PATH}/logs/archiver.$(date +%Y_%m_%d_%H_%M).log"
