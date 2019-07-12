import { Component, OnInit, HostListener, AfterViewInit } from '@angular/core';
import { BaseComponent } from '../../base/base-component';
import { Router } from '@angular/router';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';

@Component({
  selector: 'app-com0000-main',
  templateUrl: './com0000-main.component.html',
  styleUrls: ['./com0000-main.component.css']
})
export class Com0000MainComponent extends BaseComponent implements OnInit {

  constructor(
    public router: Router,
    public localStorageManageService:LocalStorageManageService,
    ) { super(); }
  public verticalOffset:number=0;

  public panelOpenState:boolean=false;
  public panelOpenName():string{
    
    if(this.panelOpenState){
      return "閉じる";
    }
    else{
      return "開く";
    }
  }



  ngOnInit() {
    this.Title = 'ホーム';
    //this.router.navigate(['/main/PE0101']);

    let localStorageItem = this.localStorageManageService.get(RangeSearchKey.COM0000_MENU_STATUS);

    if (localStorageItem != null&&localStorageItem.value==1) {
      this.panelOpenState=true;
    }
    else{
      this.panelOpenState=false;
    }


  }

  public isChangeCss():boolean{
    return this.router.url.indexOf("PE0102")>=0?true:false;
  }

  public menuChange(){
    this.panelOpenState=!this.panelOpenState;

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.COM0000_MENU_STATUS;
    localstorageItem.value = this.panelOpenState?1:0;
    this.localStorageManageService.set(localstorageItem);


    //this.ngAfterViewInit();
  }  



  @HostListener('window:scroll', ['$event']) // for window scroll events
  public scrollEvent(event:any){
    this.verticalOffset = window.pageYOffset 
    || document.documentElement.scrollTop 
    || document.body.scrollTop || 0;
  }
}
