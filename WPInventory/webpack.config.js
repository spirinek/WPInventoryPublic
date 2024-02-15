const path = require("path");
const webpack = require("webpack");
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const bundleOutputDir = "./wwwroot/dist";

module.exports = env => {
  const isDevBuild = !(env && env.prod);

  const clientBundleConfig = {
    stats: { modules: false },
    context: __dirname,
    resolve: { extensions: [".js"] },
    entry: { main: "./ClientApp/main.js" },
    module: {
      rules: [
        { test: /\.js$/, include: /ClientApp/, use: "babel-loader" },
        {
          test: /\.vue$/,
          include: /ClientApp/,
          loader: "vue-loader",
          options: {
            loaders: {
              scss: "vue-style-loader!css-loader!sass-loader", // <style lang="scss">
              sass: "vue-style-loader!css-loader!sass-loader?indentedSyntax" // <style lang="sass">
            }
          }
        },
        {
          test: /\.css$/,
          use: isDevBuild
            ? ["style-loader", "css-loader"]
            : ExtractTextPlugin.extract({ use: "css-loader?minimize" })
        },
        {
          test: /\.(svg|eot|woff|ttf|svg|woff2)$/,
          use: [
            {
              loader: "file-loader",
              options: {
                name: "[name].[ext]",
                outputPath: "fonts/"
              }
            }
          ]
        },
        {
          test: /\.(png|jpg|jpeg)$/,
          use: [
            {
              loader: "file-loader",
              options: {
                limit: 25000,
                name: "img/[name].[ext]"
              }
            }
          ]
        },
        {
          test: /\.scss$/,
          use: ["vue-style-loader", "css-loader", "sass-loader"]
        }
      ]
    },
    output: {
      path: path.join(__dirname, bundleOutputDir),
      filename: "[name].js",
      publicPath: "dist/"
    },
    plugins: [
       new webpack.DefinePlugin({
           "process.env": {
               NODE_ENV: JSON.stringify(isDevBuild ? "development" : "production")
           }
       }),
       new webpack.DllReferencePlugin({
           context: __dirname,
           manifest: require("./wwwroot/dist/vendor-manifest.json")
       })
    ].concat(
       isDevBuild
           ? [
               // Plugins that apply in development builds only
               new webpack.SourceMapDevToolPlugin({
                   filename: "[file].map", // Remove this line if you prefer inline source maps
                   moduleFilenameTemplate: path.relative(
                       bundleOutputDir,
                       "[resourcePath]"
                   ) // Point sourcemap entries to the original file locations on disk
               })
           ]
           : [
               // Plugins that apply in production builds only
               new webpack.optimize.UglifyJsPlugin(),
               new ExtractTextPlugin("site.css")
           ]
    )
  };

  const serverBundleConfig = {
    resolve: { mainFields: ["main"] },
    entry: {
      'prerender': "./ClientApp/prerendering/prerender.js"
    },
    plugins: [
      new webpack.DllReferencePlugin({
        context: __dirname,
        manifest: require("./wwwroot/dist/vendor-manifest.json"),
        sourceType: "commonjs2",
        name: "./vendor"
      })
    ],
    output: {
      filename: "prerender.js",
      libraryTarget: "commonjs",
      path: path.join(__dirname, "./ClientApp/dist")
    },
    target: "node",
    devtool: "inline-source-map"
  };

  return [clientBundleConfig, serverBundleConfig];
};
