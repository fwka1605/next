import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { LoginComponent } from './component/login/login.component';
import { AuthPageComponent } from './component/auth-page/auth-page.component';
import { AuthGuard } from './guard/auth-guard';

const routes: Routes = [
    { path: 'auth', component: AuthPageComponent,canActivate: [AuthGuard]},
    { path: 'login', component: LoginComponent },
    { path: '**', component: LoginComponent },
  ];

  

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  declarations: [],
  exports: [ RouterModule ]
})


export class AppRoutingModule {
    

 }
