import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomeComponent} from "./pages/home/home.component";
import {TemperatureComponent} from "./pages/temperature/temperature.component";
import {HistoryComponent} from "./pages/history/history.component";
import {LoginComponent} from "./authentication/login/login.component";
import {authGuard} from "./guards/auth.guard";
import {PasswordResetComponent} from "./authentication/password-reset/password-reset.component";
import {AccountComponent} from "./pages/user/account/account.component";
import {ChangePasswordComponent} from "./pages/user/change-password/change-password.component";
import {adminGuard} from "./guards/admin.guard";
import {DownloadComponent} from "./pages/download/download.component";
import {CatchDayComponent} from "./pages/catchDay/catchDay.component";
import {CatchStatisticsComponent} from "./pages/catch-statistics/catch-statistics.component";
import {
  CatchStatisticsMonthDetailComponent
} from "./pages/catch-statistics/catch-statistics-month-detail/catch-statistics-month-detail.component";
import {overseerGuard} from "./guards/overseer.guard";

// Defining routes for different components
const routes: Routes = [
  // Home route pointing to the HomeComponent
  {path: 'home', component: HomeComponent},

  // Redirecting the root URL to the home page
  {path: '', redirectTo: '/home', pathMatch: 'full'},

  // Route for the temperature component
  {path: 'temperatur', component: TemperatureComponent},

  // Route for the history component
  {path: 'geschichte', component: HistoryComponent},

  // Route for the login component
  {path: 'login', component: LoginComponent},

  // Protected route for downloads, requiring authentication using authGuard
  {path: 'downloads', component: DownloadComponent, canActivate:[authGuard]},
  {
    path: 'dashboard',
    loadChildren: () => import('./pages/dashboard/dashboard.module').then(m => m.DashboardModule),
    canActivate: [authGuard]
  },

  // Protected route for the catch day component, requiring authentication
  {path: 'angeltag', component: CatchDayComponent, canActivate:[overseerGuard]},

  // Protected route for the catch statistics component, requiring authentication
  {path: 'fangstatistik', component: CatchStatisticsComponent, canActivate:[overseerGuard]},

  // Protected route for detailed monthly catch statistics, requiring authentication
  {path: 'monatstatistik/:licenceId/:month', component: CatchStatisticsMonthDetailComponent, canActivate: [authGuard]},

  // Route for the password reset component
  {path: 'resetPassword', component: PasswordResetComponent},

  // Protected route for the user account component, requiring authentication
  {path: 'konto', component: AccountComponent, canActivate: [authGuard]},

  // Protected route for changing the password, requiring authentication
  {path: 'password-aendern', component: ChangePasswordComponent, canActivate: [authGuard]},

  // Lazy-loaded admin module with both authentication and admin guards
  {
    path: 'admin',
    loadChildren: () => import('./pages/admin/admin.module').then(m => m.AdminModule),
    canActivate: [authGuard, adminGuard]
  }
  // TODO: Uncomment the following line and create a PageNotFoundComponent
  //{ path: '**', component: PageNotFoundComponent },  // Wildcard route for a 404 page
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
