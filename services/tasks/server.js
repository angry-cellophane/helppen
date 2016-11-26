var express = require('express');
var bodyParser = require('body-parser');
var compression = require('compression');
var token = require('app/js/auth/tokenController');
var cookieParser = require('cookie-parser');
var task = require('app/js/task/taskController');
var filters = require('app/js/auth/filters');
var tokenService = require('app/js/auth/tokenService');

var app = express();
app.use(bodyParser.urlencoded({
  extended: true
}));
app.use(bodyParser.json());
app.use(cookieParser('cookieSecret'));
app.use(compression());


var router = express.Router();

router.use(filters.denyUnauthenticated);
router.route('/tasks')
  .post(task.create)
  .get(task.getAll);

router.route('/tasks/:taskId')
  .get(task.getById)
  .put(task.update)
  .delete(task.remove)

app.use('/api', router);
app.post('/auth/token', token.controller);

app.use(express.static('public'));

app.set('view engine', 'ejs');

app.all('/', filters.authenticate, function(req, res) {
  var user = req.currentUser;

  if (!user) {
    res.render('login');
  } else {
    res.render('tasks', {
      username: user.username
    });
  }
});


var port = process.env.PORT || 8000;
app.listen(port);

console.log('server is running');
