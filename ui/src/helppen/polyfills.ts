// Polyfills
// import 'es6-shim';
require('core-js/client/shim.min');
import 'reflect-metadata';
require('zone.js/dist/zone');
// import 'ts-helpers';

if (PRODUCTION) {
  // Production

} else {
  // Development

  Error['stackTraceLimit'] = Infinity;

  require('zone.js/dist/long-stack-trace-zone');
}
