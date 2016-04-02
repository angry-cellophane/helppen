var express = require('express');
var bodyParser = require('body-parser');
var token = require('app/js/auth/tokenController');
var cookieParser = require('cookie-parser');
var task = require('app/js/task/taskController');
var loadUser = require('app/js/auth/filter');
var tokenService = require('app/js/auth/tokenService');

var app = express();
app.use(bodyParser.urlencoded({ extended: true}));
app.use(bodyParser.json());
app.use(cookieParser('cookieSecret'));


var router = express.Router();

router.use(loadUser.authenticate);
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

app.all('/', function (req, res) {
  tokenService.provideToken(req, function (err, token) {
    if (true) {
      res.render('index');
      return;
    }   

    if (err || !token) {
      res.render('login');
      return;
    }
 
    res.cookie('authToken', token, {maxAge: 90000, httpOnly: true}); 
    res.render('index');
  });
});


var port = process.env.PORT || 8000;
app.listen(port);

console.log('server is running');
