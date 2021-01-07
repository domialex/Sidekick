/// <binding ProjectOpened='_develop' />
const { series, src, dest, task, watch, parallel } = require('gulp');

const webpack = require('webpack')
const webpackConfig = require('./webpack.config.js')

function jsElectron() {
    return new Promise((resolve, reject) => {
        webpack(webpackConfig, (err, stats) => {
            if (err) {
                return reject(err)
            }
            if (stats.hasErrors()) {
                return reject(new Error(stats.compilation.errors.join('\n')))
            }
            resolve()
        })
    })
}

const jsWatch = () => watch('./Scripts/**/*.js', series(jsElectron));

task('_build', series(jsElectron));
task('_develop', parallel('_build', jsWatch));
