var db = require('app/js/db')
var date = require('app/js/date');

var stashTimeLine = date.fromNow(-7); // -7 days from now

var sql = "update task set state = 'STASH' where state = 'NOT_COMPLITED' and creationDateTime < "+db.escape(stashTimeLine);
db.query(sql, function (err, rows) {
  console.log(rows);
  process.exit();
});
