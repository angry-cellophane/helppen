var dao = require('app/js/task/taskDao')
var date = require('app/js/date');

var stashTimeLine = date.fromNow(-7); // -7 days from now

dao.getByStateAndLessThenDate('NOT_COMPLITED', stashTimeLine, function (err, tasks) {
  for (var i in tasks) {
    var task = tasks[i];
    task.state = 'STASH';
    dao.update(task.ownerId, task, function (err, updTask) {
      console.log(err);
      console.log('upd task: ' + updTask);
      console.log('Moved to stash: '+ updTask);
    });
  }
  process.exit();
});
