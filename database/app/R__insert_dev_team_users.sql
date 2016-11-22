USE hpapp;
INSERT IGNORE INTO user(id, login, password_hash, username) VALUES (
  1,
  'alex',
  '53dd61b16bc79ce3eef10a5a7c',
  'Alex'
);
INSERT IGNORE INTO user(id, login, password_hash, username) VALUES (
  2,
  'nikolay',
  '5cd86fa63dd684aaf5e3095868fe7ea6175c99',
  'Nikolay Nuyakshin'
);
INSERT IGNORE INTO user(id, login, password_hash, username) VALUES (
  3,
  'pavel',
  '42d072ac3d8d8df1eef512476aff',
  'Pavel Petyanov'
);
INSERT IGNORE INTO user(id, login, password_hash, username) VALUES (
  4,
  'eugeny',
  '57c463ac3fcec7e0fcf5165f77e974',
  'Eugeny Levin'
);
INSERT IGNORE INTO user(id, login, password_hash, username) VALUES (
  5,
  'demo',
  '56d469a66bd398fdf2',
  'Demo'
);

commit;
