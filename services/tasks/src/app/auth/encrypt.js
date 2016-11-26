module.exports = function () {
  var crypto = require('crypto');
  var algorithm = 'aes-256-ctr';
  var salt = 'ok@k*29A4<23#*sDNczM34';

  var encrypt = function (text) {
    var cipher = crypto.createCipher(algorithm, salt);
    var crypted = cipher.update(text, 'utf8', 'hex')
    return crypted + cipher.final('hex');
  }

  var decrypt = function (text) {
    var decipher = crypto.createDecipher(algorithm, salt);
    var dec = decipher.update(text, 'hex', 'utf8');
    return dec + decipher.final('utf8');
  }

  return {
    encrypt : encrypt,
    decrypt: decrypt
  };
}();
