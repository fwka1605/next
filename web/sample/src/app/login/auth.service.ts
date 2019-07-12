import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { UserModel } from './user-model';



@Injectable()
export class AuthService {


  // Store profile object in auth class
  userModel: UserModel;

  constructor(private router: Router) {
    // Set userProfile attribute of already saved profile
    this.userModel = JSON.parse(localStorage.getItem('profile'));
    if (this.userModel == null) {
      this.userModel = new UserModel();
      this.userModel.LoginFlag = false;
      this.userModel.UserId = "test";
      localStorage.setItem('profile', JSON.stringify(this.userModel));
    }
  }

  login() {
    this.userModel = JSON.parse(localStorage.getItem('profile'));
    if (this.userModel == null) {
      this.userModel = new UserModel();
      this.userModel.LoginFlag = false;
      this.userModel.UserId = "test";
    }
    else {
      this.userModel.LoginFlag = false;
      this.userModel.UserId = "test";
    }
    localStorage.setItem('profile', JSON.stringify(this.userModel));
  };


  authenticated() {
    // Set userProfile attribute of already saved profile
    this.userModel = JSON.parse(localStorage.getItem('profile'));

    if (null == this.userModel) {
      return false;
    }
    else {
      return this.userModel.LoginFlag;
    }
  };


  logout() {
    // Remove token and profile from localStorage
    localStorage.removeItem('profile');
    this.router.navigate(['/login/login']);
  };
}