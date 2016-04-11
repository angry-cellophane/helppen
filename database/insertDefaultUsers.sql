USE hpapp;
INSERT INTO user(id, login, password_hash, username) VALUES (
  1,
  'Alex',
  '73dd61b16bc79ce3eef10a5a7c',
  'Alex'
);
INSERT INTO user(id, login, password_hash, username) VALUES (
  2,
  'Nikolay',
  '7cd86fa63dd684aaede7165b6ff462b2',
  'Nikolay Nuyakshin'
);
INSERT INTO user(id, login, password_hash, username) VALUES (
  3,
  'Pavel',
  '62d072ac3d8d8df1eef512476aff',
  'Pavel Petyanov'
);
INSERT INTO user(id, login, password_hash, username) VALUES (
  4,
  'Eugeny',
  '77c463ac3fcec7e0fcf5165f77e974',
  'Eugeny Levin'
);
commit;
