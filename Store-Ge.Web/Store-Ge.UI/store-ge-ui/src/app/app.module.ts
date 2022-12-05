import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { RegisterPageComponent } from './pages/register-page/register-page.component';
import { ErrorPageComponent } from './pages/error-page/error-page.component';
import { SharedModule } from './shared/shared.module';
import { ResendEmailPageComponent } from './pages/resend-email-page/resend-email-page.component';
import { HttpClientModule } from '@angular/common/http';
import { ConfirmEmailPageComponent } from './pages/confirm-email-page/confirm-email-page.component';
import { ForgottenPageComponent } from './pages/forgotten-page/forgotten-page.component';
import { ResetPasswordPageComponent } from './pages/reset-password-page/reset-password-page.component';

@NgModule({
  declarations: [
    AppComponent,
    HomePageComponent,
    LoginPageComponent,
    RegisterPageComponent,
    ErrorPageComponent,
    ResendEmailPageComponent,
    ConfirmEmailPageComponent,
    ForgottenPageComponent,
    ResetPasswordPageComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    SharedModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
