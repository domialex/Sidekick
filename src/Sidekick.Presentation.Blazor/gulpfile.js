/// <binding ProjectOpened='_develop' />
const { series, src, dest, task, watch, parallel } = require('gulp');
const sass = require('gulp-sass');
const autoprefixer = require('gulp-autoprefixer');
const sourcemaps = require('gulp-sourcemaps');
const babel = require('gulp-babel');
const concat = require('gulp-concat');
const plumber = require('gulp-plumber');
const path = require('path');

sass.compiler = require('sass');

function cssBuild() {
    return src(`./Styles/**/*.scss`, { base: './' })
    .pipe(plumber())
    .pipe(sourcemaps.init())
    .pipe(
        sass({
            outputStyle: 'compressed',
            importer: function importer(url) {
                if (url[0] === '~') {
                return { file: path.resolve('node_modules', url.substr(1)) };
                }

                return { file: url };
            },
        })
        .on('error', sass.logError)
    )
    .pipe(
      autoprefixer({
        cascade: false,
      }),
    )
    .pipe(concat('site.css'))
    .pipe(sourcemaps.write('.'))
    .pipe(dest('./wwwroot/css/'));
}

const cssWatch = () => watch('./Styles/**/*.scss', cssBuild);

function jsDependencies() {
    return;
    return src([
    ])
    .pipe(plumber())
    .pipe(concat('dependencies.js'))
    .pipe(babel({
        presets: ["@babel/preset-env"]
    }))
    .pipe(dest('./wwwroot/js'));
}

function jsBuild() {
    return src([
        './Scripts/**/*.js',
    ])
        .pipe(plumber())
        .pipe(concat('build.js'))
        .pipe(babel({
            presets: ["@babel/preset-env"]
        }))
        .pipe(dest('./wwwroot/js'));
}

const jsWatch = () => watch('./Scripts/**/*.js', series(jsDependencies, jsBuild));

task('css.build', cssBuild);
task('js.build', series(jsDependencies, jsBuild));
task('_build', series('css.build', 'js.build'));
task('_develop', parallel('_build', cssWatch, jsWatch));
