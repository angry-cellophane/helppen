module.exports = function() {

  var users = require('app/js/auth/userDao');
  var enc = require('app/js/auth/encrypt');

  var provideToken = function(userForm, cb) {
    users.findUserByLogin(userForm.login, function(err, user) {
      if (err || !user) {
        cb('Login or password are incorrect');
        return;
      }

      var passwordHash = enc.encrypt(userForm.login + ':' + userForm.password);

      if (passwordHash !== user.passwordHash) {
        cb('Login or password are incorrect');
        return;
      }

      var now = (new Date).getTime();
      var expirationDate = now + 120 * 60 * 1000;
      var token = user.id + ":" + expirationDate;
      cb('', enc.encrypt(token));
    });
  }

  return {
    provideToken: provideToken
  }
}();
