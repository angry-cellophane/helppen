var enc = require('app/js/auth/encrypt');
var read = require('read');
read({ prompt: 'Username: ' }, function(er, username) {
  read({ prompt: 'Password: ', silent: true }, function(er, password) {
    var token = enc.encrypt(username + ':'+password);
    console.log('Your token is: %s', token);
  });
});
