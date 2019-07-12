import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { EBFileSettingMasterService } from 'src/app/service/Master/ebfile-setting-master.service';
import { EBFileSetting } from 'src/app/model/eb-file-setting.model';
import { PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { FILE_FIELD_TYPE_NAMES } from 'src/app/common/const/eb-file.const';
import { FormControl, FormGroup } from '@angular/forms';
import { EBFileSettingsResult } from 'src/app/model/eb-file-settings-result.model';
import { BUTTON_ACTION } from 'src/app/common/const/event.const';
import { EBFileSettingSearch } from 'src/app/model/eb-file-setting-search.model';
import { ModalEbFileSettingComponent } from 'src/app/component/modal/modal-eb-file-setting/modal-eb-file-setting.component';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';

@Component({
  selector: 'app-ph1301-eb-data-format-list',
  templateUrl: './ph1301-eb-data-format-list.component.html',
  styleUrls: ['./ph1301-eb-data-format-list.component.css']
})
export class Ph1301EbDataFormatListComponent extends BaseComponent implements OnInit {

  public readonly fileFieldTypeNames = FILE_FIELD_TYPE_NAMES;
  public ebFileSettingResult: EBFileSettingsResult;

  public chkDoUseCtrls: Array<FormControl>;

  constructor(
    public processResultService: ProcessResultService,
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public ebFileSettingService: EBFileSettingMasterService,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef

  ) {
    super();
    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path: string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[1];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title);
        this.processModalCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title, true);
      }
    });

  }

  ngOnInit() {
    this.setValidator();
    this.getEBFileSettingResult();
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({});
  }

  public setControlInit() {
    let ebFileSettingLenght = this.ebFileSettingResult.eBFileSettings.length;
    this.chkDoUseCtrls = new Array<FormControl>(ebFileSettingLenght);
    for (let i = 0; i < ebFileSettingLenght; i++) {
      this.chkDoUseCtrls[i] = new FormControl("");
      this.MyFormGroup.addControl("chkDoUseCtrl" + i, this.chkDoUseCtrls[i]);
      this.chkDoUseCtrls[i].setValue(this.ebFileSettingResult.eBFileSettings[i].isUseable == 1 ? true : false);
    }
  }

  /**
   * データ取得
   */
  public getEBFileSettingResult() {
    this.ebFileSettingResult = new EBFileSettingsResult();
    this.ebFileSettingResult.eBFileSettings = new Array<EBFileSetting>();
    this.ebFileSettingService.GetItems()
      .subscribe(response => {
        if (response) {
          this.processCustomResult = this.processResultService.processAtGetData(
            this.processCustomResult, response, false, this.partsResultMessageComponent);
          if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
            this.ebFileSettingResult.eBFileSettings = response;
            this.setControlInit();
          }
        }
      });
  }

  /**
   * ボタン操作によるメソッド呼び出し
   * @param action ボタン操作
   */
  public buttonActionInner(action: BUTTON_ACTION) {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);
    this.processResultService.processResultStart(this.processCustomResult, action);
    if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
      return;
    }

    switch (action) {
      case BUTTON_ACTION.REGISTRY:
        this.registry();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
  }

  /**
   * データ登録
   */
  public registry() {
    let registryData = new EBFileSettingSearch();
    registryData.updateIds = new Array<number>();

    for (let i = 0; i < this.chkDoUseCtrls.length; i++) {
      if (this.chkDoUseCtrls[i].value == true) {
        registryData.updateIds.push(this.ebFileSettingResult.eBFileSettings[i].id);
      }
    }

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();


    this.ebFileSettingService.UpdateIsUseable(registryData)
      .subscribe(response => {
        if (response) {
          this.processCustomResult = this.processResultService.processAtSave(
            this.processCustomResult, response, false, this.partsResultMessageComponent);
          if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
            this.getEBFileSettingResult();
          }
        }
        componentRef.destroy();
      });
  }

  /**
   * 新規作成・修正
   */
  public openEbFileSettingModal(selectIndex:number = null) {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalEbFileSettingComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;
    if (selectIndex != null) {
      componentRef.instance.SelectData = this.ebFileSettingResult.eBFileSettings[selectIndex];
    }

    componentRef.instance.Closing.subscribe(() => {
      this.getEBFileSettingResult();

      componentRef.destroy();
    });
  }

}
