USE hpapp;
INSERT INTO user(id, login, password_hash, username) VALUES (
  1,
  'Alex',
  '42d077ba26d88ff4',
  'Alex'
);
INSERT INTO user(id, login, password_hash, username) VALUES (
  2,
  'Nikolay',
  '42d077ba26d88ff4',
  'Nikolay Nuyakshin'
);
INSERT INTO user(id, login, password_hash, username) VALUES (
  3,
  'Pavel',
  '42d077ba26d88ff4',
  'Pavel Petyanov'
);
INSERT INTO user(id, login, password_hash, username) VALUES (
  2,
  'Eugeny',
  '42d077ba26d88ff4',
  'Eugeny Levin'
);
commit;
