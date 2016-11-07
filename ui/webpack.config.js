var path = require('path');
var fs = require('fs');
var webpack = require('webpack');
var minimist = require('minimist');

// Webpack Plugins
var CommonsChunkPlugin = webpack.optimize.CommonsChunkPlugin;
var autoprefixer = require('autoprefixer');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var ExtractTextPlugin = require('extract-text-webpack-plugin');
var CopyWebpackPlugin = require('copy-webpack-plugin');

// Command line arguments
var argv = minimist(process.argv.slice(2));

// Paths
const PATHS = {
  src: '/src',
  contentBase: '/bundles',
  appFile: 'bootstrap.ts',
  vendorFile: 'vendor.ts',
  polyfillsFile: 'polyfills.ts'
};

// Arguments
const PORT = argv.port || 8082
const PRODUCTION = argv.production;
const TEST = (argv._.indexOf('start') === -1) ? false : true;
var DEBUG = false;

/**
 * Config
 * Reference: http://webpack.github.io/docs/configuration.html
 * This is the object where all configuration gets set
 */
var config = {};

/**
 * Devtool
 * Reference: http://webpack.github.io/docs/configuration.html#devtool
 * Type of sourcemap to use per build type
 */
if (TEST) {
  config.devtool = 'inline-source-map';
} else if (PRODUCTION) {
  config.devtool = 'source-map';
} else {
  config.devtool = 'eval-source-map';
}

// Add debug messages
config.debug = DEBUG = !PRODUCTION || !TEST;

/**
 * Entry
 * Reference: http://webpack.github.io/docs/configuration.html#entry
 */
config.entry = TEST ? {} : {
  'polyfills': pathFor('src/helppen', 'polyfillsFile'),
  'vendor': pathFor('src/helppen', 'vendorFile'),
  'app': pathFor('src/helppen', 'appFile')
};

/**
 * Output
 * Reference: http://webpack.github.io/docs/configuration.html#output
 */
config.output = {
  path: paths('bundles'),
  filename: PRODUCTION ? '[name].[hash].js' : '[name].bundle.js',
  chunkFilename: PRODUCTION ? '[id].[hash].chunk.js' : '[id].chunk.js',
  // publicPath: 'http://localhost:' + PORT + '/'
};

if (!PRODUCTION) {
  config.output.publicPath = 'http://localhost:' + PORT + '/';
}

/**
 * Resolve
 * Reference: http://webpack.github.io/docs/configuration.html#resolve
 */
config.resolve = {
  cache: !TEST,
  root: [process.cwd(), path.resolve('src'), path.resolve('node_modules')],
  extensions: ['', '.ts', '.js', '.json', '.css', '.scss', '.html'],
  alias: {}
};

/**
 * Loaders
 * Reference: http://webpack.github.io/docs/configuration.html#module-loaders
 * List: http://webpack.github.io/docs/list-of-loaders.html
 * This handles most of the magic responsible for converting modules
 */
config.module = {
  // preLoaders: TEST ? [] : [{
    // test: /\.ts$/,
    // loader: 'tslint'
  // }],
  loaders: [
    // Support for .ts files.
    {
      test: /\.ts$/,
      loader: 'ts',
      query: {
        'ignoreDiagnostics': [
          2403, // 2403 -> Subsequent variable declarations
          2300, // 2300 -> Duplicate identifier
          2374, // 2374 -> Duplicate number index signature
          2375, // 2375 -> Duplicate string index signature
          2502  // 2502 -> Referenced directly or indirectly
        ]
      },
      exclude: [TEST ? /\.(e2e)\.ts$/ : /\.(spec|e2e)\.ts$/, /node_modules\/(?!(ng2-.+))/, /assets/, /bundles/]
    },

    // Support for *.json files.
    {
      test: /\.json$/,
      loader: 'json'
    },

    // Support for CSS as raw text
    // use 'null' loader in test mode (https://github.com/webpack/null-loader)
    // all css in src/style will be bundled in an external css file
    {
      test: /\.css$/,
      exclude: [paths('src', 'helppen'), paths('src', 'common')],
      loader: TEST ? 'null' : ExtractTextPlugin.extract('style', 'css?sourceMap!postcss')
    },

    // all css required in src/app files will be merged in js files
    {
      test: /\.css$/,
      include: [paths('src', 'helppen'), paths('src', 'common')],
      loader: 'raw!postcss'
    },

    // support for .scss files
    // use 'null' loader in test mode (https://github.com/webpack/null-loader)
    // all css in src/style will be bundled in an external css file
    {
      test: /\.scss$/,
      exclude: [paths('src', 'helppen'), paths('src', 'common')],
      loader: TEST ? 'null' : ExtractTextPlugin.extract('style', 'css?sourceMap!postcss!sass')
    },

    // all css required in src/app files will be merged in js files
    {
      test: /\.scss$/,
      exclude: paths('src', 'assets', 'styles'),
      loader: 'raw!postcss!sass'
    },

    // support for .html as raw text
    // todo: change the loader to something that adds a hash to images
    {
      test: /\.template.html$/,
      loader: 'raw'
    },

    // copy those assets to output
    /*{
     test: /\.(png|jpe?g|gif|svg|woff|woff2|ttf|eot|ico)$/,
     loader: 'file?name=fonts/[name].[hash].[ext]?'
     },*/

    // fonts support
    {
      test: /\.(woff|woff2)/,
      // test: /\.(woff|woff2)(\?\S*)?$/,
      loader: 'url?limit=10000&mimetype=application/font-woff&prefix=fonts&name=fonts/[hash].[ext]'
    },
    {
      test: /\.ttf(\?\S*)?$/,
      loader: 'url?limit=10000&mimetype=application/octet-stream&prefix=fonts&name=fonts/[hash].[ext]'
    },
    {
      test: /\.eot(\?\S*)?$/,
      loader: 'url?limit=10000&mimetype=application/vnd.ms-fontobject&prefix=fonts&name=fonts/[hash].[ext]'
    },
    {
      test: /\.svg(\?\S*)?$/,
      loader: 'url?limit=10000&mimetype=image/svg+xml&prefix=fonts&name=fonts/[hash].[ext]'
    },

    // images support
    {
      test: /.*\.(gif|png|jpe?g)$/i,
      // include: [paths('src', 'assets', 'images')],
      loader: 'file?hash=sha512&digest=hex&name=images/[hash].[ext]'
    }
  ],
  postLoaders: [],
  noParse: [/.+zone\.js\/dist\/.+/, /.+angular2\/bundles\/.+/, /angular2-polyfills\.js/]
};

/**
 * Plugins
 * Reference: http://webpack.github.io/docs/configuration.html#plugins
 * List: http://webpack.github.io/docs/list-of-plugins.html
 */
config.plugins = [
  new webpack.DefinePlugin({
    PORT: PORT,
    PRODUCTION: PRODUCTION,
    DEBUG: DEBUG
  })
];

if (!TEST) {
  config.plugins.push(
    // Generate chunks if necessary
    // Reference: https://webpack.github.io/docs/code-splitting.html
    // Reference: https://webpack.github.io/docs/list-of-plugins.html#commonschunkplugin
    new CommonsChunkPlugin({
        name: ['vendor', 'polyfills']
      }),

    // Extract css files
    // Reference: https://github.com/webpack/extract-text-webpack-plugin
    // Disabled when in test mode or not in build mode
    new ExtractTextPlugin('[name].[hash].css', {disable: !PRODUCTION}),

    // Inject script and link tags into html files
    // Reference: https://github.com/ampedandwired/html-webpack-plugin
    new HtmlWebpackPlugin({
      title: 'HELPPEN',
      template: 'src/assets/index.html',
      // chunksSortMode: packageSort(['polyfills', 'vendor', 'app'])
      chunksSortMode: 'dependency'
    //   favicon: './src/assets/images/favicon-32x32.png',
    }),

    // Copy files
    // Reference: https://github.com/kevlened/copy-webpack-plugin
    new CopyWebpackPlugin([
      // {from: paths('src/assets/fonts'), to: paths('bundles/fonts')},
      // {from: paths('src/assets/images'), to: paths('bundles/images')}
      {from: paths('src/assets/i18n')}
    ])
  );
}

if (TEST) {
  // instrument only testing sources with Istanbul, covers js compiled files for now
  config.module.postLoaders.push({
    test: /\.(js|ts)$/,
    include: path.resolve('src'),
    loader: 'istanbul-instrumenter-loader',
    exclude: [/\.spec\.ts$/, /\.e2e\.ts$/, /node_modules/]
  })
}

// Add build specific plugins
if (PRODUCTION) {
  config.plugins.push(
    // Reference: http://webpack.github.io/docs/list-of-plugins.html#noerrorsplugin
    // Only emit files when there are no errors
    new webpack.NoErrorsPlugin(),

    // Reference: http://webpack.github.io/docs/list-of-plugins.html#dedupeplugin
    // Dedupe modules in the output
    new webpack.optimize.DedupePlugin(),

    // Reference: http://webpack.github.io/docs/list-of-plugins.html#uglifyjsplugin
    // Minify all javascript, switch loaders to minimizing mode
    new webpack.optimize.UglifyJsPlugin({
      // Angular 2 is broken again, disabling mangle until beta 6 that should fix the thing
      // Todo: remove this with beta 6
      mangle: false,
      sourceMap: false
    }),

    // Copy files
    // Reference: https://github.com/kevlened/copy-webpack-plugin
    new CopyWebpackPlugin([
      {from: paths('src/assets/fonts'), to: paths('bundles/fonts')},
      {from: paths('src/assets/images'), to: paths('bundles/images')}
    ])
  );
}

/**
 * PostCSS
 * Reference: https://github.com/postcss/autoprefixer-core
 * Add vendor prefixes to your css
 */
config.postcss = [
  autoprefixer({
    browsers: ['last 2 version']
  })
];

/**
 * Sass
 * Reference: https://github.com/jtangelder/sass-loader
 * Transforms .scss files to .css
 */
config.sassLoader = {};

/**
 * Apply the tslint loader as pre/postLoader
 * Reference: https://github.com/wbuchwalter/tslint-loader
 */
config.tslint = {
  emitErrors: false,
  failOnHint: false
};

/**
 * Dev server configuration
 * Reference: http://webpack.github.io/docs/configuration.html#devserver
 * Reference: http://webpack.github.io/docs/webpack-dev-server.html
 */
config.devServer = {
  contentBase: PATHS.contentBase,
  // outputPath: paths('bundles'),
  historyApiFallback: true,
  stats: 'minimal', // none (or false), errors-only, minimal, normal (or true) and verbose
  port: PORT
};

function paths(args) {
  args = Array.prototype.slice.call(arguments, 0);
  return path.join.apply(path, [__dirname].concat(args));
}

function pathFor(path, file) {
    return paths(path) + '/' + PATHS[file];
}

function packageSort(packages) {
  // packages = ['polyfills', 'vendor', 'app']
  var len = packages.length - 1;
  var first = packages[0];
  var last = packages[len];
  return function sort(a, b) {
    // polyfills always first
    if (a.names[0] === first) {
      return -1;
    }
    // main always last
    if (a.names[0] === last) {
      return 1;
    }
    // vendor before app
    if (a.names[0] !== first && b.names[0] === last) {
      return -1;
    } else {
      return 1;
    }
  }
}

module.exports = config;
