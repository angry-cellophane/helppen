module.exports = function () {

  var uuid = require('node-uuid');
  var db = require('app/js/db');

  var getAll = function (ownerId, callback) {
    var sql = 'select * from task where ownerId = ' + db.escape(ownerId) + ' order by orderNumber desc';
    db.query(sql, callback);
  };

  var getNewOrderNumber = function (ownerId, callback) {
    var sql = 'select max(orderNumber) orderNumber from task where ownerId = ' + db.escape(ownerId);
    db.query(sql, function(err, rows) {
      if (err) {
        console.log(err);
        return;
      }

      var maxOrderNumber = 0;
      if (rows[0].orderNumber !== null) {
        maxOrderNumber = rows[0].orderNumber + 1.0;
      }
      callback(maxOrderNumber);
    });
  };

  var create = function(ownerId, text, callback) {
    var newId = uuid.v4();
    getNewOrderNumber(ownerId, function (maxOrderNumber) {
      var newTask = {id: newId, text: text, state: 'NOT_COMPLITED', orderNumber: maxOrderNumber, ownerId: ownerId };
      var creationDateTime = new Date().toISOString().slice(0, 19).replace('T', ' ');
      var sql = 'insert into task (id, text, state, ownerId, orderNumber, creationDateTime) values ('
        + db.escape(newTask.id)
        + ',' + db.escape(newTask.text)
        +',' + db.escape(newTask.state)
        + ', ' + ownerId
        + ',' + db.escape(newTask.orderNumber)
        + ',' + db.escape(creationDateTime)
        +')';
      db.query(sql, function (err, rows) {
        callback(err, newTask);
      });
    });
  };

  var getById = function (ownerId, taskId, callback) {
    var sql = 'select * from task where ownerId = ' + db.escape(ownerId) + ' and id = ' + db.escape(taskId);
    db.query(sql, function (err, rows) {
      var task = {};
      if (!err && rows && rows.length === 1) {
        task = rows[0];
      }
      callback(err, task);
    });
  };

  var update = function (ownerId, task, callback) {
    var sql = 'update task set text = ' + db.escape(task.text) + ', state = ' + db.escape(task.state) + ', orderNumber = ' + db.escape(task.orderNumber) + ' where id = ' + db.escape(task.id) + ' and ownerId = ' + ownerId;
    db.query(sql, function (err, rows) {
      callback(err, task);
    });
  };

  var remove = function (ownerId, taskId, callback) {
    var sql = 'delete from task where id = ' + db.escape(taskId) +' and ownerId = ' + db.escape(ownerId);
    db.query(sql, callback);
  };

  return {
    getAll: getAll,
    getById: getById,
    create: create,
    update: update,
    remove: remove
  };
}();
