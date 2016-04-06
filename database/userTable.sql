USE hpapp;
CREATE TABLE IF NOT EXIST user(
  id mediumint(9) PRIMARY KEY,
  login varchar(50) NOT NULL,
  password_hash varchar(300) NOT NULL,
  username varchar(300) NOT NULL
);
