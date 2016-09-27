'use strict';

/*var gulp = require('gulp');

gulp.paths = {
  src: 'src',
  dist: 'dist',
  tmp: '.tmp',
  e2e: 'e2e'
};

require('require-dir')('./gulp');

gulp.task('build', ['clean'], function () {
    gulp.start('buildapp');
});
*/

// including plugins
var gulp = require('gulp');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');

// task
gulp.task('minify-js', function () {
    gulp.src([
        'app/triangular/layouts/layouts.module.js',
        'app/triangular/layouts/states/triangular/triangular.controller.js',
        'app/examples/email/email.module.js',
        'app/examples/email/layout/toolbar/toolbar.controller.js',
        'app/examples/calendar/calendar.module.js',
        'app/examples/calendar/layouts/toolbar/toolbar.controller.js',
        'app/triangular/layouts/default/default-layout.controller.js',
        'app/triangular/layouts/default/default-content.directive.js',
        'app/triangular/components/components.module.js',
        'app/triangular/components/wizard/wizard.directive.js',
        'app/triangular/components/wizard/wizard-form.directive.js',
        'app/triangular/components/toolbars/toolbar.controller.js',
        'app/triangular/components/widget/widget.directive.js',
        'app/triangular/components/notifications-panel/notifications-panel.controller.js',
        'app/triangular/components/menu/menu.provider.js',
        'app/triangular/components/menu/menu.directive.js',
        'app/triangular/components/menu/menu-item.directive.js',
        'app/triangular/components/table/table.directive.js',
        'app/triangular/components/table/table-start-from.filter.js',
        'app/triangular/components/table/table-cell-image.filter.js',
        'app/triangular/components/loader/loader.directive.js',
        'app/triangular/components/loader/loader-service.js',
        'app/triangular/components/footer/footer.controller.js',
        'app/triangular/components/breadcrumbs/breadcrumbs.service.js',
        'app/examples/menu/menu.module.js',
        'app/examples/menu/examples/dynamic-menu.controller.js',
        'app/examples/maps/maps.module.js',
        'app/examples/maps/examples/map-terrain-demo.controller.js',
        'app/examples/maps/examples/map-label-demo.controller.js',
        'app/examples/forms/forms.module.js',
        'app/examples/forms/examples/binding-1.controller.js',
        'app/examples/forms/examples/autocomplete-1.controller.js',
        'app/examples/elements/elements.module.js',
        'app/examples/elements/examples/upload-animate.controller.js',
        'app/examples/elements/examples/upload-1.controller.js',
        'app/examples/elements/examples/toast-1.controller.js',
        'app/examples/elements/examples/table-advanced.controller.js',
        'app/examples/elements/examples/table-advanced-service.js',
        'app/examples/elements/examples/table-1.controller.js',
        'app/examples/elements/examples/loader-1.controller.js',
        'app/examples/elements/examples/grids-1.controller.js',
        'app/examples/elements/examples/fab-speed-1.controller.js',
        'app/examples/elements/examples/dialog-1.controller.js',
        'app/examples/elements/examples/chips.controller.js',
        'app/examples/dashboards/dashboards.module.js',
        'app/examples/dashboards/widgets/widget-weather.directive.js',
        'app/examples/dashboards/widgets/widget-twitter.directive.js',
        'app/examples/dashboards/widgets/widget-todo.directive.js',
        'app/examples/dashboards/widgets/widget-server.directive.js',
        'app/examples/dashboards/widgets/widget-load-data.directive.js',
        'app/examples/dashboards/widgets/widget-google-geochart.js',
        'app/examples/dashboards/widgets/widget-contacts.directive.js',
        'app/examples/dashboards/widgets/widget-chat.directive.js',
        'app/examples/dashboards/widgets/widget-chartjs-ticker.directive.js',
        'app/examples/dashboards/widgets/widget-chartjs-pie.directive.js',
        'app/examples/dashboards/widgets/widget-chartjs-line.directive.js',
        'app/examples/dashboards/widgets/widget-calendar.directive.js',
        'app/examples/dashboards/server/dashboard-server.controller.js',
        'app/examples/dashboards/social/dashboard-social.controller.js',
        'app/examples/dashboards/sales/sales.service.js',
        'app/examples/dashboards/sales/order-dialog.controller.js',
        'app/examples/dashboards/sales/fab-button.controller.js',
        'app/examples/dashboards/sales/date-change-dialog.controller.js',
        'app/examples/dashboards/sales/dashboard-sales.controller.js',
        'app/examples/dashboards/analytics/dashboard-analytics.controller.js',
        'app/examples/charts/charts.module.js',
        'app/examples/authentication/authentication.module.js',
        'app/triangular/themes/themes.module.js',
        'app/triangular/themes/theming.provider.js',
        'app/triangular/themes/skins.provider.js',
        'app/triangular/triangular.module.js',
        'app/triangular/router/router.run.js',
        'app/triangular/router/router.provider.js',
        'app/triangular/router/router.module.js',
        'app/triangular/profiler/profiler.module.js',
        'app/triangular/profiler/profiler.config.js',
        'app/triangular/layouts/layouts.provider.js',
        'app/triangular/directives/directives.module.js',
        'app/triangular/directives/theme-background.directive.js',
        'app/triangular/directives/same-password.directive.js',
        'app/triangular/directives/palette-background.directive.js',
        'app/triangular/directives/countupto.directive.js',
        'app/permission/permission.module.js',
        'app/permission/pages/permission.controller.js',
        'app/layouts/rightsidenav/rightsidenav.controller.js',
        'app/layouts/toolbar/toolbar.controller.js',
        'app/app.module.js',
        'app/layouts/loader/loader.controller.js',
        'app/layouts/leftsidenav/leftsidenav.controller.js',
        'app/layouts/footer/footer.controller.js',
        'app/examples/ui/webfont-loader-module.js',
        'app/examples/ui/ui.module.js',
        'app/examples/ui/weather-icons.controller.js',
        'app/examples/ui/ui.run.js',
        'app/examples/ui/ui.config.js',
        'app/examples/ui/typography.controller.js',
        'app/examples/ui/typography-switcher.service.js',
        'app/examples/ui/skins.controller.js',
        'app/examples/ui/material-icons.controller.js',
        'app/examples/ui/fa-icons.controller.js',
        'app/examples/ui/colors.controller.js',
        'app/examples/ui/color-dialog.controller.js',
        'app/examples/menu/menu.config.js',
        'app/examples/menu/level.controller.js',
        'app/examples/menu/dynamicMenu.service.js',
        'app/examples/maps/maps.config.js',
        'app/examples/maps/map.controller.js',
        'app/examples/layouts/layouts.module.js',
        'app/examples/layouts/layouts.config.js',
        'app/examples/layouts/composer.controller.js',
        'app/examples/extras/extras.module.js',
        'app/examples/extras/timeline.controller.js',
        'app/examples/extras/replace-with.directive.js',
        'app/examples/extras/gallery.controller.js',
        'app/examples/extras/gallery-dialog.controller.js',
        'app/examples/extras/extras.config.js',
        'app/examples/extras/avatars.controller.js',
        'app/examples/extras/animate-element.directive.js',
        'app/examples/forms/forms.config.js',
        'app/examples/forms/form-wizard.controller.js',
        'app/examples/elements/progress.controller.js',
        'app/examples/elements/icons.controller.js',
        'app/examples/elements/elements.config.js',
        'app/examples/elements/buttons.controller.js',
        'app/examples/dashboards/dashboards.config.js',
        'app/examples/dashboards/dashboard-draggable-controller.js',
        'app/examples/markets/markets.module.js',
        'app/examples/markets/markets.config.js',
        'app/examples/charts/charts.config.js',
        'app/examples/authentication/authentication.config.js',
        'app/triangular/triangular.run.js',
        'app/triangular/settings.provider.js',
        'app/triangular/config.route.js',
        'app/translate/translate.module.js',
        'app/translate/translate.config.js',
        'app/seed-module/seed.module.js',
        'app/seed-module/seed.config.js',
        'app/seed-module/seed-page.controller.js',
        'app/permission/user.factory.js',
        'app/permission/permission.run.js',
        'app/permission/permission.config.js',
        'app/examples/examples.module.js',
        'app/value.googlechart.js',
        'app/translate.filter.js',
        'app/error-page.controller.js',
        'app/config.triangular.themes.js',
        'app/config.triangular.settings.js',
        'app/config.triangular.layout.js',
        'app/config.route.js',
        'app/config.chartjs.js',
        'app/app.run.js'
    ])
    .pipe(uglify({ mangle: false }))
        .pipe(concat('main.js'))
    .pipe(gulp.dest('./public/'));
});