module.exports = function() {

  var users = require('src/app/auth/userDao');
  var enc = require('src/app/auth/encrypt');

  var provideTokenInfo = function(userForm, cb) {
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

      console.log('user.id = ' + user.id);
      console.log('user.username = ' + user.username);
      console.log('user.login = ' + user.login);

      cb('', { 
          token: enc.encrypt(token),
          username: user.username
        }
      );
    });
  }

  return {
    provideTokenInfo: provideTokenInfo
  }
}();
