module.exports = function() {

  var users = require('app/js/auth/userDao');
  var enc = require('app/js/auth/encrypt');

  var provideToken  = function (req, cb) {
    var login = req.body.login;
    var password = req.body.password;
    
    if (!login || !password) {
      console.log(login + '/' + password);
      cb('no login or password passed');
      return;
    }
    
    users.findUserByLogin(login, function (err, user) {
      if (err || !user) { 
        cb(err);
        return;
      }

      var passwordHash = enc.encrypt(password);

      if (passwordHash !== user.passwordHash) {
        cb(err); 
        return;
      }
  
      var now = (new Date).getTime(); 
      var expirationDate = now + 120 * 60 * 1000;
      var token = user.id + ":" + expirationDate;
      cb(err, enc.encrypt(token));
    });
  }  

  return {
    provideToken : provideToken 
  }
}();
