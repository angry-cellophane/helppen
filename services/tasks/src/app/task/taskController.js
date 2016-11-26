module.exports = function () {

  var tasks = require('src/app/task/taskDao');
  var enc = require('src/app/auth/encrypt');

  var getAll = function(req, res) {
    var user = req.currentUser;
    tasks.getAllByUser(user.id, function(err, rows) {
      res.json(rows);
    });
  };

  var getById = function(req, res) {
    var user = req.currentUser;
    var taskId = req.params.taskId;

    if (!taskId) {
      res.status(400).send('You have to send taskId');
      return;
    }

    tasks.getById(user.id, taskId, function(err, task) {
      if (err) {
        console.log(err);
        res.status(503).send('Internal server error');
        return;
      }

      if (!task || !task.id) {
        res.status(404).send();
        return;
      }

      res.json(task);
    });
  };

  var create = function (req, res) {
    var user = req.currentUser;
    var text = req.body.text;

    tasks.create(user.id, text, function (err, task) {
      if (err) {
        console.log(err);
        res.status(503).send('Internal servver error');
        return;
      }

      res.json(task);
    });
  };

  var update = function (req, res) {
    var user = req.currentUser;
    var taskId = req.params.taskId;
    var task = req.body;

    task.ownerId = user.id;
    task.id = taskId;

    tasks.update(user.id, task, function (err) {
      if (err) {
        res.status(503).send('Internal server error');
        console.log(err);
        return;
      }

      res.status(200).send();
    });
  };

  var remove = function (req, res) {
    var user = req.currentUser;
    var taskId = req.params.taskId;

    tasks.remove(user.id, taskId, function (err) {
      if (err) {
        res.status(503).send('Internal server error');
        console.log(err);
        return;
      }

      res.status(200).send();
    });
  };

  return {
    getAll: getAll,
    getById: getById,
    create: create,
    update: update,
    remove: remove
  };
}();
