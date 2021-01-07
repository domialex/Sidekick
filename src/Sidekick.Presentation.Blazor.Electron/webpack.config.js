//webpack.config.js
var path = require('path');
var webpack = require('webpack');

module.exports = {
    entry: {
        main: './Scripts/main.js',
    },
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: 'ts-loader',
                exclude: /node_modules/
            }
        ]
    },
    // node: {
    //     'fs': 'empty'
    // },
    resolve: {
        extensions: ['.tsx', '.ts', '.js']
    },
    output: {
        publicPath: "/js/",
        path: path.join(__dirname, '../Sidekick.Presentation.Blazor/wwwroot/js/'),
        filename: 'electron.bundle.js'
    }
};
