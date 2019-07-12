import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-date-picker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.css']
})
export class DatePickerComponent implements OnInit {

    minDate = new Date("2018/01/01");
    maxDate = new Date("2018/12/31");

  constructor() { }

  ngOnInit() {
  }

}
