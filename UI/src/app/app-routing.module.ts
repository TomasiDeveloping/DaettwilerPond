import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {HomeComponent} from "./pages/home/home.component";
import {TemperatureComponent} from "./pages/temperature/temperature.component";
import {HistoryComponent} from "./pages/history/history.component";
import {LoginComponent} from "./authentication/login/login.component";
import {DashboardComponent} from "./pages/dashboard/dashboard.component";
import {authGuard} from "./guards/auth.guard";
import {PasswordResetComponent} from "./authentication/password-reset/password-reset.component";
import {AccountComponent} from "./pages/user/account/account.component";
import {ChangePasswordComponent} from "./pages/user/change-password/change-password.component";
import {AdminComponent} from "./pages/admin/admin.component";

const routes: Routes = [
  {path: 'home', component: HomeComponent},
  {path: '', redirectTo: '/home', pathMatch: 'full'},
  {path: 'temperatur', component: TemperatureComponent},
  {path: 'geschichte', component: HistoryComponent},
  {path: 'login', component: LoginComponent},
  {path: 'dashboard', component: DashboardComponent, canActivate: [authGuard]},
  {path: 'resetPassword', component: PasswordResetComponent},
  {path: 'konto', component: AccountComponent, canActivate: [authGuard]},
  {path: 'password-aendern', component: ChangePasswordComponent, canActivate:[authGuard]},
  {path: 'admin', component: AdminComponent}
  // TODO
  //{ path: '**', component: PageNotFoundComponent },  // Wildcard route for a 404 page
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
