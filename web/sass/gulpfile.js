var gulp = require('gulp');
var sass = require('gulp-sass');
var sassGlob = require('gulp-sass-glob');
var sourcemaps = require('gulp-sourcemaps');
var plumber = require('gulp-plumber');
var notify = require('gulp-notify');
var postcss = require('gulp-postcss');
var autoprefixer = require('autoprefixer');
var assets = require('postcss-assets');
var cssdeclsort = require('css-declaration-sorter');
var mqpacker = require('css-mqpacker');
var concat = require('gulp-concat');
var sourcemaps = require('gulp-sourcemaps');
var mode = require('gulp-mode')();
var uglify = require('gulp-uglify-es').default; // es6用 uglify 
var styleguide = require('sc5-styleguide');
var sassSrc = './sass/**/*.scss';
var jsSrc = './script/**/*.js';
var cssDest = '../client/src/assets/css/';
var styleguideDest = 'C:/inetpub/wwwroot';


gulp.task('sass', function () {
  return gulp.src(sassSrc)
    .pipe(plumber({ errorHandler: notify.onError("Error: <%= error.message %>") }))
    .pipe(mode.development(sourcemaps.init()))
    .pipe(sassGlob())
    .pipe(mode.development(sass({ outputStyle: 'expanded' })))
    .pipe(mode.production(sass({ outputStyle: 'compressed' })))
    .pipe(postcss([mqpacker()]))
    .pipe(postcss([cssdeclsort({ order: 'smacss' })]))
    .pipe(postcss([assets({
      loadPaths: ['./images']
    })]))
    .pipe(postcss([autoprefixer({ browsers: ['last 2 versions'] })]))
    .pipe(mode.development(sourcemaps.write()))
    .pipe(gulp.dest(cssDest))
});

gulp.task('script', function () {
  return gulp.src(jsSrc)
    .pipe(plumber({ errorHandler: notify.onError("Error: <%= error.message %>") }))
    .pipe(sourcemaps.init())
    .pipe(concat('prototype.js'))
    .pipe(uglify())
    .pipe(sourcemaps.write())
    .pipe(gulp.dest('../html/assets/js'))
})

// sassコンパイル、スタイルガイド起動
gulp.task('sass:watch', function () {
  gulp.watch([sassSrc], { usePolling: true }, ['sass','local-guide']); // usePollingでwatchとsaveの競合を回避
});

gulp.task('local-styleguide:generate', function() {
  return gulp.src(sassSrc)
  .pipe(styleguide.generate({
    port: 4000,
    title: 'next-v-one スタイルガイド',
    server: true,
    rootPath: styleguideDest,
    overviewPath: 'sc5/overview.md',
    extraHead: [
      '<link rel="stylesheet" href="/sc5Override.css">',
    ]
  }))
  .pipe(gulp.dest(styleguideDest));
});

gulp.task('release-styleguide:generate', function() {
  return gulp.src(sassSrc)
  .pipe(styleguide.generate({
    port: 4000,
    title: 'next-v-one スタイルガイド',
    server: false,
    rootPath: styleguideDest,
    overviewPath: 'sc5/overview.md',
    extraHead: [
      '<link rel="stylesheet" href="/sc5Override.css">'
    ]
  }))
  .pipe(gulp.dest(styleguideDest));
});


gulp.task('styleguide:applystyles', function() {
  return gulp.src([
    cssDest + 'all-import.css',
    './sass/_sc5.scss', // スタイルガイドのカスタマイズ用scss
  ])
  .pipe(concat('sc5Override.scss'))
  .pipe(plumber({ errorHandler: notify.onError("Error: <%= error.message %>") }))
  .pipe(sassGlob())
  .pipe(sass({
    errLogToConsole: true
  }))
  .pipe(styleguide.applyStyles())
  .pipe(gulp.dest(styleguideDest));
});

gulp.task('local-guide', ['local-styleguide:generate', 'styleguide:applystyles']);

gulp.task('release-guide', ['release-styleguide:generate', 'styleguide:applystyles']);
