import {AuthGuard} from './login/auth.guard';
import { Component } from '@angular/Core';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';

import { HttpClientModule,HttpClientXsrfModule    } from '@angular/common/http';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { LOCALE_ID } from '@angular/core';

import { AppComponent } from './app.component';

import { AppRoutingModule } from './app-routing.module';
import { AutoCompleteComponent } from './material/auto-complete/auto-complete.component';

import { NgFormValidatorComponent } from './validation/ng-form-validator/ng-form-validator.component';
import { Ng5ValidationComponent } from './validation/ng5-validation/ng5-validation.component'


// 追加！
import { MaterialModule } from './material-module';
import {MatNativeDateModule} from '@angular/material';
import { ValidatorsModule  } from 'ng-form-validator';
import { CustomFormsModule  } from 'ng5-validation';
import { DatePickerComponent } from './material/date-picker/date-picker.component';
import { SortHeaderComponent } from './material/sort-header/sort-header.component';
import { SetterGetterComponent } from './model/setter-getter/setter-getter.component';
import { LoginComponent } from './login/login/login.component';
import { AuthPageComponent } from './login/auth-page/auth-page.component';
import { AuthService } from './login/auth.service';
import { AcordionComponent } from './material/acordion/acordion.component';
import { InputControllerComponent } from './input/input-controller/input-controller.component';
import { ParseComponent } from './jspon/parse/parse.component';

// 追加！



@NgModule({
  declarations: [
    AppComponent,
    AutoCompleteComponent,
    NgFormValidatorComponent,
    Ng5ValidationComponent,
    DatePickerComponent,
    SortHeaderComponent,
    SetterGetterComponent,
    LoginComponent,
    AuthPageComponent,
    AcordionComponent,
    InputControllerComponent,
    ParseComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    HttpClientXsrfModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    MatNativeDateModule,
    ValidatorsModule .forRoot(),
    CustomFormsModule,
  ], 
  providers: [{provide: LOCALE_ID, useValue: 'ja-JP' },AuthGuard,AuthService],

  bootstrap: [AppComponent]
})
export class AppModule { }
