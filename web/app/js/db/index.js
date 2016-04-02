module.exports = function () {
  var mysql = require('mysql');
  var config = require('config');

  var poolConfig = {
    connectionLimit: 100,
    host: config.get('datasource:host'),
    user: config.get('datasource:username'),
    password: config.get('datasource:password'),
    database: config.get('datasource:database'),
    debug: config.get('datasource:debug')
  };

  var pool = mysql.createPool(poolConfig);

  console.log('Connection esstablished: ' + poolConfig.user + '@' + poolConfig.host + '/' + poolConfig.database);

  var query = function (sql, callback) {
    pool.getConnection(function (err, connection) {
      if (err) {
        connection.release();
        console.log(err);
        callback(err);
        return;
      }
      
      connection.query(sql, function(err, rows){ 
        connection.release();
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
