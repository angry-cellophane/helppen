module.exports = function() {

  var users = require('app/js/auth/userDao');
  var enc = require('app/js/auth/encrypt');
  var tokenService = require('app/js/auth/tokenService');

  var controller = function authController(req, res) {
    var login = req.body.login;
    var password = req.body.password;

    tokenService.provideToken(req, function(err, token) {
      if (err || !token) {
        res.status(400).send('Login or password are incorrect');
        return;
      }

      res.json({
        token: token
      });
    });
  }

  return {
    controller: controller
  }
}();
