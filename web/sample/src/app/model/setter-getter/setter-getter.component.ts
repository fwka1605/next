import { Component, OnInit } from '@angular/core';
import { UserModel } from './user-data-model';

@Component({
  selector: 'app-setter-getter',
  templateUrl: './setter-getter.component.html',
  styleUrls: ['./setter-getter.component.css']
})
export class SetterGetterComponent implements OnInit {

  private userData:UserModel;

  constructor() { }

  ngOnInit() {

    this.userData = new UserModel();
    this.userData.name1="name1";
    this.userData.Name2="Name2";

    this.userData.json=JSON.stringify(this.userData);
  }

}
