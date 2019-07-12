import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ColumnNameSettingMasterService } from 'src/app/service/Master/column-name-setting-master.service';
import { ColumnNameSetting } from 'src/app/model/column-name-setting.model';
import { ColumnNameSettingsResult } from 'src/app/model/column-name-settings-result.model';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { BUTTON_ACTION } from 'src/app/common/const/event.const';
import { PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';


@Component({
  selector: 'app-ph1001-column-setting',
  templateUrl: './ph1001-column-setting.component.html',
  styleUrls: ['./ph1001-column-setting.component.css']
})
export class Ph1001ColumnSettingComponent extends BaseComponent implements OnInit {
  /** 項目名称データ */
  public columnNameSettingsResult: ColumnNameSettingsResult;
  /** 種別 */
  public tableNameCtrl: FormControl;
  /** 項目名 */
  public originalNameCtrl: FormControl;
  /** 変更後名称 */
  public aliasCtrl: FormControl;
  /** 選択した行の番号 */
  public selectIndex: number;
  /** ボタンの有効無効設定 */
  public isInvalidButton: boolean = true;
  /** 登録項目の表示非表示 */
  public panelOpenState:boolean = false;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public columnNameSettingService: ColumnNameSettingMasterService,
    public processResultService:ProcessResultService
  ) {
    super();

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path:string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[1];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId,this.Title);
      }
    });    
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.getColumnNameSettingData();
  }

  public setControlInit() {
    this.tableNameCtrl = new FormControl('');   // 種別
    this.originalNameCtrl = new FormControl(''); // 項目名
    this.aliasCtrl = new FormControl('', [Validators.maxLength(10)]); // 変更後名称
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      tableNameCtrl: this.tableNameCtrl,   // 種別
      originalNameCtrl: this.originalNameCtrl, // 項目名
      aliasCtrl: this.aliasCtrl, // 変更後名称
    })
  }

  public setFormatter() {
  }

  public clear() {
    this.MyFormGroup.reset();
    this.isInvalidButton = true;
    this.panelOpenState = false;

    this.componentStatus = this.COMPONENT_STATUS_TYPE.CREATE;

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
      case BUTTON_ACTION.REGISTRY:
        this.registry();
        break;
      default:
        console.log('buttonAction Error.');
        break;
    }
  }  


  /**
   * データ取得
   */
  public getColumnNameSettingData() {
    this.columnNameSettingsResult = new ColumnNameSettingsResult();
    this.columnNameSettingsResult.columnNames = new Array<ColumnNameSetting>();

    this.columnNameSettingService.Get(0)
      .subscribe(response => {
        if (response) {
          this.columnNameSettingsResult.columnNames = response;

          for (let i = 0; i < response.length; i++) {
            this.columnNameSettingsResult.columnNames[i].tableName
              = response[i].tableName == 'Billing' ? '請求' : '入金';
          }
        }
      });
  }

  /**
   * データ登録
   */
  public registry() {
    let registryData = this.columnNameSettingsResult.columnNames[this.selectIndex];
    registryData.tableName = registryData.tableName == '請求' ? 'Billing' : 'Receipt';
    registryData.alias = this.aliasCtrl.value;

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();

    this.columnNameSettingService.Save(registryData)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, true, this.partsResultMessageComponent);
        if (result!=PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.getColumnNameSettingData();
        }
        componentRef.destroy();
      });
  }

  /**
   * 選択したデータをフォームに表示
   * @param index 選択した行番号
   */
  public selectLine(index: number) {

    this.componentStatus = this.COMPONENT_STATUS_TYPE.UPDATE;
    this.selectIndex = index;
    this.isInvalidButton = false;
    this.panelOpenState = true;
    this.tableNameCtrl.setValue(this.columnNameSettingsResult.columnNames[index].tableName);  // 種別
    this.originalNameCtrl.setValue(this.columnNameSettingsResult.columnNames[index].originalName);  // 項目名
    this.aliasCtrl.setValue(this.columnNameSettingsResult.columnNames[index].alias);  // 変更後名称
  }

}
