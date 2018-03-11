const path = require('path');
const webpack = require('webpack');

module.exports = {
  entry: './Client/index.tsx',
  devtool: 'inline-source-map',
  module: {
    rules: [{
      test: /\.tsx?$/,
      use: 'ts-loader',
      exclude: /node_modules/
    }]
  },
  resolve: {
    extensions: ['.tsx', '.ts', '.js']
  },
  output: {
    filename: 'app.js',
    path: path.resolve(__dirname, 'wwwroot/js'),
    libraryTarget: 'var',
    library: 'Fabric'
  },
  externals: [{
      'react': 'React',
    },
    {
      'react-dom': 'ReactDOM'
    }
  ]
};