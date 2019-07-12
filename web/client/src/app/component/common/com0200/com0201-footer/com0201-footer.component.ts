import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-com0201-footer',
  templateUrl: './com0201-footer.component.html',
  styleUrls: ['./com0201-footer.component.css']
})
export class Com0201FooterComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  public onDragOver(event:any){
    event.preventDefault();
  }

  public onDrop(event:any){
    event.preventDefault();
  }

}
