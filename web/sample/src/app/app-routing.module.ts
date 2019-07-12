import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { AutoCompleteComponent } from './material/auto-complete/auto-complete.component';
import { DatePickerComponent } from './material/date-picker/date-picker.component';
import { NgFormValidatorComponent } from './validation/ng-form-validator/ng-form-validator.component';
import { Ng5ValidationComponent } from './validation/ng5-validation/ng5-validation.component';
import { SortHeaderComponent } from './material/sort-header/sort-header.component';
import { SetterGetterComponent } from './model/setter-getter/setter-getter.component';
import { AuthPageComponent } from './login/auth-page/auth-page.component';
import { LoginComponent } from './login/login/login.component';
import { AuthGuard } from './login/auth.guard';
import { AcordionComponent } from './material/acordion/acordion.component';
import { InputControllerComponent } from './input/input-controller/input-controller.component';
import { ParseComponent } from './jspon/parse/parse.component';

const routes: Routes = [
  { path: 'material/autocomplete', component: AutoCompleteComponent },
  { path: 'material/date-picker', component: DatePickerComponent },
  { path: 'material/acordion', component: AcordionComponent },
  { path: 'material/sort-header', component: SortHeaderComponent },
  { path: 'validation/ng-form-validator', component: NgFormValidatorComponent },
  { path: 'validation/ng5-validation', component: Ng5ValidationComponent },
  { path: 'input/input-controller', component: InputControllerComponent },
  { path: 'json/parse', component: ParseComponent },
  { path: 'model/setter-getter', component: SetterGetterComponent },
  { path: 'login/auth-page', component: AuthPageComponent, canActivate: [AuthGuard] },
  { path: 'login/login', component: LoginComponent },
  { path: '**', component: AutoCompleteComponent },
];


@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  declarations: [],
  exports: [RouterModule]
})


export class AppRoutingModule {


}
