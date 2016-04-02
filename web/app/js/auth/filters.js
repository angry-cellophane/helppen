module.exports = function() {
  var enc = require('app/js/auth/encrypt');
  var users = require('app/js/auth/userDao');

  var authenticate = function(req, res, next) {
    var token = req.cookies.authToken;

    if (!token) {
      next();
      return;
    }

    var dec = enc.decrypt(token);
    var id = dec.substring(0, dec.search(':'));

    users.findUserById(id, function(err, user) {
      if (err || !user) {
        next();
        return;
      }

      req.currentUser = user;
      next();
    });
  };

  var denyUnauthenticated = function(req, res, next) {
    authenticate(req, res, function() {
      if (!req.currentUser) {
        res.status(401).send('Unauthorized');
        return
      }

      next();
    });
  }

  // var authenticate = function(req, res, next) {
  //   var token = req.cookies.authToken;
  //   if (!token) {
  //     res.status(401).send('Unauthorized');
  //     console.log('Token not found');
  //     return;
  //   }
  //
  //   var dec = enc.decrypt(token);
  //   var id = dec.substring(0, dec.search(':'));
  //
  //   users.findUserById(id, function(err, user) {
  //     if (err || !user) {
  //       res.status(401).send('Unauthorized');
  //       console.log('User not found');
  //       return;
  //     }
  //
  //     req.currentUser = user;
  //     next();
  //   });
  // };

  return {
    authenticate: authenticate,
    denyUnauthenticated: denyUnauthenticated
  };
}();