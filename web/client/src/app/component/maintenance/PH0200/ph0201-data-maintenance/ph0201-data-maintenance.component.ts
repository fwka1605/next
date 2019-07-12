import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { DataMaintenanceService } from 'src/app/service/data-maintenance.service';
import { StringUtil } from 'src/app/common/util/string-util';
import { EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_INF, MSG_WNG } from 'src/app/common/const/message.const';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';

@Component({
  selector: 'app-ph0201-data-maintenance',
  templateUrl: './ph0201-data-maintenance.component.html',
  styleUrls: ['./ph0201-data-maintenance.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]

})
export class Ph0201DataMaintenanceComponent extends BaseComponent implements OnInit {

  public deleteDateCtrl: FormControl; // 削除日

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public dataMaintenanceService: DataMaintenanceService,
    public processResultService: ProcessResultService
  ) {
    super();

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path: string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[1];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title);
      }
    });
  }

  ngOnInit() {

    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();


  }


  public setControlInit() {
    this.deleteDateCtrl = new FormControl("", [Validators.required, Validators.maxLength(10)]);  // 削除日
  }
  public setValidator() {
    this.MyFormGroup = new FormGroup({
      deleteDateCtrl: this.deleteDateCtrl,   // 削除日

    });


  }

  public setFormatter() {
  }

  public clear() {
    this.MyFormGroup.reset();

    HtmlUtil.nextFocusByName(this.elementRef, 'deleteDateCtrl', EVENT_TYPE.NONE);


  }

  /**
   * ボタン操作によるメソッド呼び出し
   * @param action ボタン操作
   */
  public buttonActionInner(action: BUTTON_ACTION) {
    this.processResultService.processResultStart(this.processCustomResult, action);
    if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
      return;
    }

    switch (action) {
      case BUTTON_ACTION.DELETE:
        this.delete();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
  }


  public delete() {
    // 削除処理
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "削除"
    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let componentRef = this.viewContainerRef.createComponent(componentFactory);
        // componentRef.destroy();

        let deleteDate: string =
          this.deleteDateCtrl.value.year
          + "-" + StringUtil.setPaddingFrontZero("" + this.deleteDateCtrl.value.month, 2, true)
          + "-" + StringUtil.setPaddingFrontZero("" + this.deleteDateCtrl.value.day, 2, true)
          + "T00:00:00";

        this.dataMaintenanceService.DeleteData(deleteDate)
          .subscribe(result => {
            if (result == 0) {
              this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.NO_DELETE_DATA, this.partsResultMessageComponent);      
            } else {
              this.processCustomResult = this.processResultService.processAtDelete(
                this.processCustomResult, result, this.partsResultMessageComponent);
            }
            componentRef.destroy();
          });
      }
      else {
        this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.PROCESS_CANCELED, this.partsResultMessageComponent);
      }
      componentRef.destroy();
    });
  }
}
