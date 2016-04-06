#!/bin/bash

mysql -uroot --password < ./appDatabase.sql
mysql -uroot --password hpapp < ./userTable.sql
mysql -uroot --password hpapp < ./taskTable.sql
mysql -uroot --password hpapp < ./appUser.sql
