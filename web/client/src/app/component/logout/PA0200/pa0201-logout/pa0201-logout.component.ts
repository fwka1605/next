import { Component, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';

@Component({
  selector: 'app-pa0201-logout',
  templateUrl: './pa0201-logout.component.html',
  styleUrls: ['./pa0201-logout.component.css']
})
export class Pa0201LogoutComponent extends BaseComponent implements OnInit {

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute ,   
    private processResultService:ProcessResultService
  ) {
    super(); 

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path:string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[0];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId,this.Title);
      }
    });    
  }

  ngOnInit() {
      this.Title="ログアウト"
  }

}
