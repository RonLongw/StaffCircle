module.exports = {
  configureWebpack: {
    devtool: 'source-map',
    devServer: {
      disableHostCheck: true,
      proxy: 'http://localhost:8080'
    },
    watchOptions: {
      poll: true
    }
  }
}
