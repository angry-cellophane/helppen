#!/bin/bash

export NODE_PATH="$(pwd)"
node archiver.js | tee ./logs/archiver.$(date +%Y_%m_%d_%H_%M).log
