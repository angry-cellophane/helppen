var db = require('src/app/db')
var date = require('src/app/date');

var stashTimeLine = date.fromNow(-7);

var sql = "update task set state = 'STASH' where state = 'NOT_COMPLITED' and lastChangeDateTime < "+db.escape(stashTimeLine);
db.query(sql, function (err, rows) {
  console.log(rows);
  process.exit();
});
