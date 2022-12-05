import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConfirmEmailPageComponent } from './pages/confirm-email-page/confirm-email-page.component';
import { ErrorPageComponent } from './pages/error-page/error-page.component';
import { ForgottenPageComponent } from './pages/forgotten-page/forgotten-page.component';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { RegisterPageComponent } from './pages/register-page/register-page.component';
import { ResendEmailPageComponent } from './pages/resend-email-page/resend-email-page.component';
import { ResetPasswordPageComponent } from './pages/reset-password-page/reset-password-page.component';
import { UserPageComponent } from './pages/user/user-page/user-page.component';
import { AuthGuard } from './services/guards/auth.guard';

const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'home', redirectTo: '/', pathMatch: 'full' },
  { path: 'login', component: LoginPageComponent },
  { path: 'register', component: RegisterPageComponent },
  { path: 'resend-email', component: ResendEmailPageComponent },
  { path: 'confirm-email', component: ConfirmEmailPageComponent },
  { path: 'forgot-password', component: ForgottenPageComponent },
  { path: 'reset-password', component: ResetPasswordPageComponent },
  {
    path: 'user/:userId',
    component: UserPageComponent,
    loadChildren: () =>
      import('./pages/user/user.module').then((m) => m.UserModule),
    canActivate: [AuthGuard],
  },
  { path: '**', component: ErrorPageComponent },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { paramsInheritanceStrategy: 'always' }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
