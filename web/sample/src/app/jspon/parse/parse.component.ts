import { Component, OnInit } from '@angular/core';
import { Staff } from './staff.model';
import { deserialize } from './serializable';

@Component({
  selector: 'app-parse',
  templateUrl: './parse.component.html',
  styleUrls: ['./parse.component.css']
})
export class ParseComponent implements OnInit {


  constructor() { }

  ngOnInit() {
  }

  private parse() {
    /*

    let obj: any;

    obj = JSON.parse(this.jsonData)
    console.log(this.jsonData);
    console.log(obj);

    let staff1: Staff = JSON.parse(this.jsonData);
    console.log(staff1);
    console.log(staff1.Id);
    console.log(staff1.Name);

    let staff2: Staff = JSON.parse(this.jsonData) as Staff;
    console.log(staff2);
    console.log(staff2.Id);
    console.log(staff2.Name);

    let staff3: Staff = JSON.parse(this.jsonData, object_class => Staff);
    console.log(staff3);
    console.log(staff3.Id);
    console.log(staff3.Name);
    */

    let staff1: Staff = new Staff(5, "5name");
    let jsonStaff1Data = JSON.stringify(staff1);
    console.log(jsonStaff1Data);

    let staff2: Staff = JSON.parse(jsonStaff1Data, deserialize);
    console.log(staff2);
    console.log(staff2.Id);
    console.log(staff2.Name);

    let jsonData: string = '{"id": "1", "name": "test"}';

    let staff3: Staff = Staff.fromJSON(JSON.parse(jsonData));
    console.log(staff3);
    console.log(staff3.Id);
    console.log(staff3.Name);

    let jsonData2: string = '[{"id": "1", "name": "test"},{"id": "2", "name": "testtest"}]';
    let staff4: Staff[] = Staff.fromJSONS(JSON.parse(jsonData2));

    console.log(staff4.length);
    console.log(staff4[0]);
    console.log(staff4[0].Id);
    console.log(staff4[0].Name);
    console.log(staff4[1]);
    console.log(staff4[1].Id);
    console.log(staff4[1].Name);


  }
}
