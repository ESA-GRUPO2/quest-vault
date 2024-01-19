import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginPageComponent } from './login-page/login-page.component';
import { PasswordResetPageComponent } from './password-reset-page/password-reset-page.component';

const routes: Routes = [
  { path: '', component: PasswordResetPageComponent, pathMatch:'full'}];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
  }
