module.exports = function() {

  var users = require('app/js/auth/userDao');
  var enc = require('app/js/auth/encrypt');
  var tokenService = require('app/js/auth/tokenService');

  var controller = function authController(req, res) {
    var login = req.body.login;
    var password = req.body.password;

    if (!login || !password) {
      console.log(login + '/' + password);
      res.status(400).send('Login or password are not passed');
      return;
    }

    tokenService.provideTokenInfo({
      login: login.toLowerCase(),
      password: password
    }, function(err, tokenInfo) {
      if (err || !tokenInfo) {
        res.status(400).send(err);
        return;
      }

      res.json({
        token: tokenInfo.token,
	username: tokenInfo.username 
      })
    });
  }

  return {
    controller: controller
  }
}();
