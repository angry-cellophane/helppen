module.exports = function () {
  var now = function() {
    return new Date().toISOString().slice(0, 19).replace('T', ' ');
  };

  var fromNow = function (days) {
    var today = new Date(now());
    return new Date(today.setTime(today.getTime() + days * 86400000));
  }

  return {
    now: now,
    fromNow: fromNow
  };
}();
