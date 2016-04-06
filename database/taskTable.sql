USE hpapp;
CREATE TABLE IF NOT EXIST task(
  id varchar(200) PRIMARY KEY,
  text varchar(300) NOT NULL,
  state varchar(50)  NOT NULL,
  ownerId mediumint(9) NOT NULL,
  orderNumber int(11) NOT NULL,
  FOREIGN KEY (ownerId)
        REFERENCES user(id)
        ON DELETE CASCADE
);
