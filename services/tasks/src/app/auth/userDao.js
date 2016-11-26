module.exports = function () {
  var db = require('app/js/db');

  var findUserByQuery = function (sql, callback) {
    db.query(sql, function(err, rows) {
      var user = {};
      if (!err && rows && rows.length === 1) {
        user = {
          id: rows[0].id,
          login: rows[0].login,
          passwordHash: rows[0].password_hash,
          username: rows[0].username
        };
      }

      callback(err, user);
    });
  };

  var findUserById = function(id, callback) {
    var sql = 'select * from user where id = ' + db.escape(id);
    findUserByQuery(sql, callback);
  };

  var findUserByLogin = function (login, callback) {
    var sql = 'select * from user where login = ' + db.escape(login);
    findUserByQuery(sql, callback);
  };

  return {
    findUserById: findUserById,
    findUserByLogin: findUserByLogin
  };
}();
