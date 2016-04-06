#!/bin/bash

> all.sql

for file in './appDatabase.sql' './userTable.sql' './taskTable.sql' './appUser.sql' './insertDefaultUsers.sql' ; do
  cat "$file" >> all.sql
done

mysql -uroot --password < all.sql

rm all.sql
