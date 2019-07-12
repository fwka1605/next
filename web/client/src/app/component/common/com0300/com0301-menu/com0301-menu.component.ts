import { Component, ViewChildren, QueryList, AfterViewInit, Input, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';
import { Menu, MenuOption } from './menu';
import { Router } from '@angular/router';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { CollationSettingMasterService } from 'src/app/service/Master/collation-setting-master.service';
import { CollationSetting } from 'src/app/model/collation-setting.model';


@Component({
  selector: 'app-com0301-menu',
  templateUrl: './com0301-menu.component.html',
  styleUrls: ['./com0301-menu.component.css']
})

export class Com0301MenuComponent implements AfterViewInit {
  @ViewChildren('menuTrigger') openTriggers: QueryList<any>;

  public menu = new Menu();
  public menus;
  public menuNumber;

  constructor(
    public router: Router,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public collationSettingService: CollationSettingMasterService,
  ) {
  }

  /**
   * Side Menu tooltip
   * @param i : Number. i === Undefined is logout menu control
   **/
  openSummaryTip = (i = undefined) => {
    if (i === undefined) {
    // ツールチップ：ログアウト
      document.querySelector('.js-menu1st > li:last-child .js-tooltip').classList.toggle('is-show');
    }
    else if (i === -1) {
    // ツールチップ：ホーム
      document.querySelector('.js-menu1st > li:nth-child(2) .js-tooltip').classList.toggle('is-show');
    }
    else {
      const tipElm = this.menus[i].querySelector('.js-tooltip');
      tipElm.classList.toggle('is-show');
    }
  }

  /**
   * second menu control
   * @param event is For getting self element
   */

  sideMenuControl(i) {
    const selfMenuItem = this.menus[i];
    const selfTargetMenu = selfMenuItem.parentNode;
    Array.from(this.menus, (menuItem: any) => {
      this.resetOpen(menuItem);
      this.resetOpen(menuItem.parentNode);
    });
    selfMenuItem.classList.add('is-open');
    selfTargetMenu.classList.add('is-open');
  }

  resetOpen(elm) {
    elm.classList.remove('is-open');
  }

  closeMenu() {
    Array.from(this.menus, (menuItem: any) => {
      this.resetOpen(menuItem);
      this.resetOpen(menuItem.parentNode);
    });
  }

  ngAfterViewInit() {
    this.menus = this.openTriggers.map(target => target.nativeElement);
    this.menuNumber = this.openTriggers.map((target, index) => {
      return index;
    });

    setTimeout(() => {
      this.collationSettingService.Get()
        .subscribe(response => {
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
            let collationSetting = response;
            let hiddenMenuIds = MenuOption.getHiddenMenuIds(this.userInfoService.ApplicationControl);
            this.menu.FirstMenus.forEach(firstMenu => {
              firstMenu.SecondItems.forEach(secondItem => {
                // 権限レベルの確認
                secondItem.Visible = this.userInfoService.isMenuAvailable(secondItem.Path);
                // ApplicationControlのオプションの確認
                if (0 <= hiddenMenuIds.indexOf(secondItem.Path)) {
                  secondItem.Visible = false;
                }
                // 設定によるメニュー表示
                if (this.userInfoService.ApplicationControl.useReceiptSection == 0 
                  && secondItem.Path == "PD0401") {
                  if (collationSetting.useApportionMenu == 1) {
                    secondItem.Visible = true;
                  } else {
                    secondItem.Visible = false;
                  }
                }
              });
              
              let findIndex = firstMenu.SecondItems.findIndex((item) => {
                return (item.Visible === true);
              });
              if (findIndex < 0) {
                firstMenu.Visible = false;
              }
            });
          }
        });
    });
  }


  public home() {
    this.router.navigate(['main']);

  }

  public onDragOver(event: any) {
    event.preventDefault();
  }

  public onDrop(event: any) {
    event.preventDefault();
  }


  public logout() {

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

  }  


}
