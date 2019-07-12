import { Component, OnInit,ComponentFactoryResolver, ViewContainerRef, ElementRef } from '@angular/core';
 
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { Router} from '@angular/router';

@Component({
  selector: 'app-com0101-header',
  templateUrl: './com0101-header.component.html',
  styleUrls: ['./com0101-header.component.css']
})
export class Com0101HeaderComponent extends BaseComponent implements OnInit {

  constructor(
    public router: Router,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public elementRef: ElementRef,
    public userInfoService: UserInfoService,
  ) {
    super();
  }

  ngOnInit() {
  }

  public onDragOver(event:any){
    event.preventDefault();
  }

  public onDrop(event:any){
    event.preventDefault();
  }  

  public manual(){
    window.open('https://docs.vone-cloud.jp/','_blank');
  } 

  public logout() {

    let matches =  this.elementRef.nativeElement.ownerDocument.getElementById("btnLogout");


    matches.click();
    /*

    // 削除処理
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "ログアウト"
    componentRef.instance.Closing.subscribe(() => {

      componentRef.destroy();
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {
        let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
        // processComponentRef.destroy();   

        this.userInfoService.init();
        this.router.navigate(['login']);
      }
    });
    */
  }  

}
