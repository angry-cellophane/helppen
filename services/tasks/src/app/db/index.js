module.exports = function () {
  var mysql = require('mysql');
  var config = require('config').datasource;
  console.log(config);

  var poolConfig = {
    connectionLimit: 100,
    host: config.host,
    user: config.username,
    password: config.password,
    database: config.database,
    debug: config.debug
  };

  var pool = mysql.createPool(poolConfig);

  console.log('Connection esstablished: ' + poolConfig.user + '@' + poolConfig.host + '/' + poolConfig.database);

  var query = function (sql, callback) {
    pool.getConnection(function (err, connection) {
      connection.release();
      if (err) {
        console.log(err);
        callback(err);
        return;
      }

      connection.query(sql, function(err, rows){
        if (err) {
          console.log(err);
        }
        callback(err, rows);
      });

      connection.on('error', function(err) {
        console.log(err);
        callback(err);
      });

    });
  };

  return {
    query : query,
    escape: mysql.escape
  }
}();
