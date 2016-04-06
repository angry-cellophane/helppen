#!/bin/bash

mysql -u hpapp --password < ./appDatabase.sql
mysql -u hpapp --password < ./taskTable.sql
mysql -u hpapp --password < ./userTable.sql
mysql -u hpapp --password < ./appUser.sql
