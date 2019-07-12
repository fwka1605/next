import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Title } from '@angular/platform-browser';


@Component({
  selector: 'app-com0501-pagetitle',
  templateUrl: './com0501-pagetitle.component.html',
  styleUrls: ['./com0501-pagetitle.component.css']
})

export class Com0501PageTitleComponent implements OnInit {
  public pageTitle;

  constructor(titleService: Title, router: Router) {
    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.pageTitle = this.GetTitle(router.routerState, router.routerState.root).join(' > ');
        titleService.setTitle(this.pageTitle + ' - V-ONE クラウド');
      }
    });
  }

  public GetTitle(state, parent) {
    const data = [];
    if (parent && parent.snapshot.data && parent.snapshot.data.title) {
      data.push(parent.snapshot.data.title);
    }
    if (state && parent) {
      data.push(... this.GetTitle(state, state.firstChild(parent)));
    }
    return data;
  }
  ngOnInit() {
  }

  public onDragOver(event:any){
    event.preventDefault();
  }

  public onDrop(event:any){
    event.preventDefault();
  }
}
