
import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TABLE_INDEX, TABLES } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { KEY_CODE, BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, PROCESS_RESULT_STATUS_TYPE } from 'src/app/common/const/status.const';
import { MasterImportData } from 'src/app/model/master-import-data.model';
import { DateUtil } from 'src/app/common/util/date-util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { ModalImportMethodSelectorComponent } from 'src/app/component/modal/modal-import-method-selector/modal-import-method-selector.component';
import { MSG_WNG, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { CODE_TYPE, FunctionType } from 'src/app/common/const/kbn.const';
import { StringUtil } from 'src/app/common/util/string-util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { SectionWithLoginUser } from 'src/app/model/section-with-login-user.model';
import { SectionWithLoginUsersResult } from 'src/app/model/section-with-login-users-result.model';
import { LoginUserMasterService } from 'src/app/service/Master/login-user-master.service';
import { SectionWithLoginUserMasterService } from 'src/app/service/Master/section-with-login-user-master.service';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { SectionWithLoginUserSearch } from 'src/app/model/section-with-login-user-search.model';
import { Section } from 'src/app/model/section.model';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger } from '@angular/material';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';

@Component({
  selector: 'app-pb1301-section-with-login-user-master',
  templateUrl: './pb1301-section-with-login-user-master.component.html',
  styleUrls: ['./pb1301-section-with-login-user-master.component.css']
})
export class Pb1301SectionWithLoginUserMasterComponent extends BaseComponent implements OnInit {

  public FunctionType: typeof FunctionType = FunctionType;

  public beforeSectionResult: SectionWithLoginUsersResult;
  public afterSectionResult: SectionWithLoginUsersResult;

  /** ログインユーザ */
  public loginUserCodeCtrl: FormControl;
  public loginUserNameCtrl: FormControl;
  public loginUserId: number;

  /** 入金部門 */
  public cbxSectionCtrl: FormControl;
  public sectionCodeFromCtrl: FormControl;
  public sectionNameFromCtrl: FormControl;
  public sectionCodeToCtrl: FormControl;
  public sectionNameToCtrl: FormControl;
  public sectionCodeFromId: number;
  public sectionCodeToId: number;

  public isAddButton: boolean = true;
  public sections: Array<Section>;

  @ViewChild('loginUserCodeInput', { read: MatAutocompleteTrigger }) loginUserCodeTrigger: MatAutocompleteTrigger;

  @ViewChild('sectionCodeFromInput', { read: MatAutocompleteTrigger }) sectionCodeFromTrigger: MatAutocompleteTrigger;
  @ViewChild('sectionCodeToInput', { read: MatAutocompleteTrigger }) sectionCodeToTrigger: MatAutocompleteTrigger;


  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public loginUsersService: LoginUserMasterService,
    public sectionWithLoginUserService: SectionWithLoginUserMasterService,
    public processResultService: ProcessResultService,
    public sectionService: SectionMasterService,
    public localStorageManageService: LocalStorageManageService
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
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();

    if (!this.userInfoService.isFunctionAvailable(FunctionType.MasterImport)
      && !this.userInfoService.isFunctionAvailable(FunctionType.MasterExport)) {
      this.securityHideShow = false;
    } else {
      this.securityHideShow = true;
    }

    this.setAutoComplete();
  }

  public setControlInit() {
    this.loginUserCodeCtrl = new FormControl('', [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.loginUserCodeLength)]);   // 債権代表者
    this.loginUserNameCtrl = new FormControl('');

    this.cbxSectionCtrl = new FormControl(false);
    this.sectionCodeFromCtrl = new FormControl('', [Validators.maxLength(this.userInfoService.ApplicationControl.sectionCodeLength)]);
    this.sectionNameFromCtrl = new FormControl('');
    this.sectionCodeToCtrl = new FormControl('', [Validators.maxLength(this.userInfoService.ApplicationControl.sectionCodeLength)]);
    this.sectionNameToCtrl = new FormControl('');
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      loginUserCodeCtrl: this.loginUserCodeCtrl,
      loginUserNameCtrl: this.loginUserNameCtrl,

      cbxSectionCtrl: this.cbxSectionCtrl,
      sectionCodeFromCtrl: this.sectionCodeFromCtrl,
      sectionNameFromCtrl: this.sectionNameFromCtrl,
      sectionCodeToCtrl: this.sectionCodeToCtrl,
      sectionNameToCtrl: this.sectionNameToCtrl
    })
  }

  public setFormatter() {
    if (this.userInfoService.ApplicationControl.loginUserCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.loginUserCodeCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.loginUserCodeCtrl);
    }

    if (this.userInfoService.ApplicationControl.sectionCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.sectionCodeFromCtrl);
      FormatterUtil.setNumberFormatter(this.sectionCodeToCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.sectionCodeFromCtrl);
      FormatterUtil.setCodeFormatter(this.sectionCodeToCtrl);
    }
  }

  public clear() {
    this.MyFormGroup.reset();
    this.beforeSectionResult = null;
    this.afterSectionResult = null;
    this.sectionCodeFromId = null;
    this.sectionCodeToId = null;
    this.loginUserId = null;
    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;

    this.sectionCodeFromCtrl.disable();
    this.sectionCodeToCtrl.disable();
    this.isAddButton = true;
  }

  public setAutoComplete() {

    // ログインユーザ
    this.initAutocompleteLoginUsers(this.loginUserCodeCtrl, this.loginUsersService, 0);

    // 入金部門はログインユーザを指定された時に設定にする。
  }


  public sectionCodeClear() {
    this.sectionCodeFromCtrl.setValue('');
    this.sectionNameFromCtrl.setValue('');
    this.sectionCodeToCtrl.setValue('');
    this.sectionNameFromCtrl.setValue('');

    this.sectionCodeFromCtrl.enable();
    this.sectionCodeToCtrl.enable();

    // 入金部門
    this.initAutocompleteSections(this.sectionCodeFromCtrl, this.sectionService, 0);
    this.initAutocompleteSections(this.sectionCodeToCtrl, this.sectionService, 1);

  }

  /**
   * 各テーブルからデータを取得
   * @param table 対象テーブル
   * @param keyCode イベント
   * @param type フォーム種別
   */
  public openMasterModal(table: TABLE_INDEX, type: string = null) {
    this.isAddButton = true;

    this.loginUserCodeTrigger.closePanel();
    this.sectionCodeFromTrigger.closePanel();
    this.sectionCodeToTrigger.closePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {

          case TABLE_INDEX.MASTER_LOGIN_USER_SECTION:
            {
              this.loginUserId = componentRef.instance.SelectedObject.id;
              this.loginUserCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.loginUserNameCtrl.setValue(componentRef.instance.SelectedName);

              this.getSectionWithLoginUser();
              break;
            }
          case TABLE_INDEX.MASTER_SECTION:
            {
              if (type === "from") {
                this.sectionCodeFromId = componentRef.instance.SelectedObject.id
                this.sectionCodeFromCtrl.setValue(componentRef.instance.SelectedCode);
                this.sectionNameFromCtrl.setValue(componentRef.instance.SelectedName);
              }
              else if (type === "to") {
                this.sectionCodeToId = componentRef.instance.SelectedObject.id
                this.sectionCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                this.sectionNameFromCtrl.setValue(componentRef.instance.SelectedName);
              }
              this.setAddButton();
              break;
            }
        }
      }
      componentRef.destroy();
    });
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
      case BUTTON_ACTION.IMPORT:
        this.openImportMethodSelector();
        break;

      case BUTTON_ACTION.EXPORT:
        this.export();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
  }

  /**
   * データ登録・削除
   */
  public registry() {
    if (this.StringUtil.IsNullOrEmpty(this.loginUserNameCtrl.value)) {
      this.processCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
      this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
      this.processCustomResult.message = MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, "入金部門コード");
      return;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();      

    let importData = new MasterImportData<SectionWithLoginUser>();
    let insertItems = new Array<SectionWithLoginUser>();
    let deleteItems = new Array<SectionWithLoginUser>();
    const beforeSections = this.beforeSectionResult.sectionWithLoginUsers;
    const afterSections = this.afterSectionResult.sectionWithLoginUsers;

    // 追加と削除の区別
    for (let i = 0; i < afterSections.length; i++) {
      let sectionWithLoginUser = new SectionWithLoginUser();
      sectionWithLoginUser.loginUserId = this.loginUserId;
      sectionWithLoginUser.updateBy = this.userInfoService.LoginUser.id;
      sectionWithLoginUser.createBy = this.userInfoService.LoginUser.id;

      // 新しいIDが　beforeにある：何もしない、beforeにない：追加)
      let findIndex = beforeSections.findIndex((item) => {
        return (item.sectionId === afterSections[i].sectionId);
      });
      if (findIndex < 0) {
        sectionWithLoginUser.sectionId = afterSections[i].sectionId;
        insertItems.push(sectionWithLoginUser);
      }
    }

    for (let i = 0; i < beforeSections.length; i++) {
      let sectionWithLoginUser = new SectionWithLoginUser();
      sectionWithLoginUser.loginUserId = this.loginUserId;
      sectionWithLoginUser.updateBy = this.userInfoService.LoginUser.id;
      sectionWithLoginUser.updateAt = beforeSections[i].updateAt;

      // 既存IDが　afterにある：何もしない、afterにない：削除)
      let findIndex = afterSections.findIndex((item) => {
        return (item.sectionId === beforeSections[i].sectionId);
      });
      if (findIndex < 0) {
        sectionWithLoginUser.sectionId = beforeSections[i].sectionId;
        deleteItems.push(sectionWithLoginUser);
      }
    }

    if (insertItems.length == 0 && deleteItems.length == 0) {
      this.processCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
      this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
      this.processCustomResult.message = MSG_WNG.NO_DATA.replace(MSG_ITEM_NUM.FIRST, "変更する");
      processComponentRef.destroy();
      return;
    }

    // サーバに送信
    importData.insertItems = insertItems;
    importData.deleteItems = deleteItems;
    importData.updateItems = new Array<SectionWithLoginUser>();

    this.sectionWithLoginUserService.Save(importData)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, true, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.clear();
        }
        processComponentRef.destroy();
      });
  }

  /**
   * 変更後の得意先に追加
   */
  public add() {
    let addSectionCodeIds = new Array<number>();
    let sectionCodeFrom = this.StringUtil.IsNullOrEmpty(this.sectionCodeFromCtrl.value);
    let sectionCodeTo = this.StringUtil.IsNullOrEmpty(this.sectionCodeToCtrl.value);

    if (sectionCodeFrom && sectionCodeTo) {
      // From/To ともに未入力
      this.processCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
      this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
      this.processCustomResult.message = MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, "追加する入金部門コード");
      return;
    }

    let findIndex: number;
    if (!sectionCodeFrom) {
      findIndex = this.getSectionIdIndex(this.sectionCodeFromId);
      if (0 <= findIndex) {
        addSectionCodeIds.push(findIndex);
      }
    }

    if (!sectionCodeTo) {
      findIndex = this.getSectionIdIndex(this.sectionCodeToId);
      if (0 <= findIndex) {
        addSectionCodeIds.push(findIndex);
      }
    }

    if (addSectionCodeIds.length == 1) {
      let sectionWithLoginUser = new SectionWithLoginUser();
      sectionWithLoginUser.sectionId = this.sections[findIndex].id;
      sectionWithLoginUser.sectionCode = this.sections[findIndex].code;
      sectionWithLoginUser.sectionName = this.sections[findIndex].name;
      this.afterSectionResult.sectionWithLoginUsers.push(sectionWithLoginUser);

    } else if (addSectionCodeIds.length == 2) {
      // 複数のコード選択の場合
      let sliceSections = this.sections.slice(addSectionCodeIds[0], addSectionCodeIds[1] + 1);
      let addSections = new Array<SectionWithLoginUser>();

      for (let i = 0; i < sliceSections.length; i++) {
        findIndex = this.getSectionIdIndex(sliceSections[i].id);
        if (0 <= findIndex) {
          let sectionWithLoginUser = new SectionWithLoginUser();
          sectionWithLoginUser.sectionId = this.sections[findIndex].id;
          sectionWithLoginUser.sectionCode = this.sections[findIndex].code;
          sectionWithLoginUser.sectionName = this.sections[findIndex].name;
          addSections.push(sectionWithLoginUser);
        }
      }
      this.afterSectionResult.sectionWithLoginUsers = this.afterSectionResult.sectionWithLoginUsers.concat(addSections);
    }

    this.afterSectionResult.sectionWithLoginUsers.sort(
      function (a, b) {
        if (a.sectionCode < b.sectionCode) return -1;
        if (a.sectionCode > b.sectionCode) return 1;
        return 0;
      });
  }

  /**
   * 登録可能の入金部門コードであるかチェック
   * @param targetId チェック対象のID
   */
  public getSectionIdIndex(targetId: number) {
    let isAdd: boolean = false;
    let findIndex: number = -1;

    // 登録済みと重複チェック
    findIndex = this.afterSectionResult.sectionWithLoginUsers.findIndex((item) => {
      return (item.sectionId === targetId);
    });

    isAdd = findIndex < 0 ? true : false;

    if (isAdd) {
      // 請求部門コード群から該当のIdを検索する
      findIndex = this.sections.findIndex((item) => {
        return (item.id === targetId);
      });
      return findIndex;
    }
    return -1;
  }


  /**
   * 変更対象の得意先から削除
   * @param index 行番号
   */
  public selectDeleteLine(index: number) {
    this.afterSectionResult.sectionWithLoginUsers.splice(index, 1);
  }

  /**
   * 変更対象の得意先を全削除
   */
  public allDelete() {
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);
    this.afterSectionResult.sectionWithLoginUsers.length = 0;
  }

  /**
   * エクスポート
   */
  public export() {
    let dataList = new Array<SectionWithLoginUser>();

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();  

    this.sectionWithLoginUserService.GetItems()
      .subscribe(result => {
        if (result) {
          dataList = result;

          // 件数チェック
          if (dataList.length <= 0) {
            this.processCustomResult = this.processResultService.processAtWarning(
              this.processCustomResult, MSG_WNG.NO_EXPORT_DATA, this.partsResultMessageComponent);
            processComponentRef.destroy();
            return;
          }

          let headers = FileUtil.encloseItemBySymbol(REPORT_HEADER.SECTION_WITH_LOGIN_MASTER);
          let data: string = headers.join(",") + LINE_FEED_CODE;

          for (let index = 0; index < dataList.length; index++) {
            let dataItem: Array<any> = [];
            dataItem.push(this.userInfoService.Company.code);
            dataItem.push(dataList[index].sectionCode);
            dataItem.push(dataList[index].loginUserCode);
            dataItem = FileUtil.encloseItemBySymbol(dataItem);

            data = data + dataItem.join(",") + LINE_FEED_CODE;
          }
          let resultDatas: Array<any> = [];
          resultDatas.push(data);
          let isTryResult: boolean = false;

          try {
            FileUtil.download(resultDatas, this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
            isTryResult = true;

          } catch (error) {
            console.error(error);
          }

          this.processResultService.processAtOutput(
            this.processCustomResult, isTryResult, 0, this.partsResultMessageComponent);

        }
        processComponentRef.destroy();
      });
    this.openOptions();
  }

  /**
   * インポート
   */
  public openImportMethodSelector() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalImportMethodSelectorComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = TABLES.MASTER_SECTION_WITH_LOGINUSER.id;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.clear();
    });
    this.openOptions();
  }

  /**
   * 入金部門を全件取得する
   */
  public getSections() {
    this.sectionService.GetItems()
      .subscribe(result => {
        if (result) {
          this.sections = result;
        }
      });
  }

  public setAddButton() {
    this.isAddButton = true;
    if (!this.StringUtil.IsNullOrEmpty(this.sectionCodeFromCtrl.value)
      || !this.StringUtil.IsNullOrEmpty(this.sectionCodeToCtrl.value)) {
      this.isAddButton = false;
    }
  }

  public setCbxSection(eventType: string) {
    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PB1301_SECTION;
    localstorageItem.value = this.cbxSectionCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, "sectionNameFromCtrl", eventType);
  } 

  ///// Enterキー押下時の処理 //////////////////////////////////////////////////////
  public getSectionWithLoginUser() {
    let search = new SectionWithLoginUserSearch();
    search.loginUserId = this.loginUserId;
    this.loadStart();
    this.sectionWithLoginUserService.GetItems(search)
      .subscribe(response => {
        this.loadEnd();
        this.processResultService.processAtGetData(
          this.processCustomResult, response, true, this.partsResultMessageComponent);

        this.beforeSectionResult = new SectionWithLoginUsersResult();
        this.beforeSectionResult.sectionWithLoginUsers = response;
        this.afterSectionResult = new SectionWithLoginUsersResult();
        this.afterSectionResult.sectionWithLoginUsers = new Array<SectionWithLoginUser>();
        this.beforeSectionResult.sectionWithLoginUsers.forEach(item => {
          this.afterSectionResult.sectionWithLoginUsers.push(item);
        })

        this.getSections();
        this.sectionCodeClear();
        this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;
      });
  }


  ///// Enterキー押下時の処理 //////////////////////////////////////////////////////

  public setLoginUserCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.loginUserCodeTrigger.closePanel();
    }

    this.loginUserId = null;
    if (StringUtil.IsNullOrEmpty(this.loginUserCodeCtrl.value)) {
      this.loginUserNameCtrl.setValue("");

    } else {
      this.loadStart();
      this.loginUsersService.GetItems(this.loginUserCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.loginUserId = response[0].id;
            this.loginUserCodeCtrl.setValue(response[0].code);
            this.loginUserNameCtrl.setValue(response[0].name);

            HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeFromCtrl', eventType);

            // 登録済みのログインユーザー
            this.getSectionWithLoginUser();
          }
          else {
            //this.loginUserCodeCtrl.setValue("");
            this.loginUserNameCtrl.setValue("");
          }
        });
    }

  }

  public setSectionCode(eventType: string, type: string = 'from') {
    let formControlCode: FormControl;
    let formControlName: FormControl;
    let nextFormName: string;

    if (type == 'from') {
      if (eventType != EVENT_TYPE.BLUR) {
        this.sectionCodeFromTrigger.closePanel();
      }
      this.sectionCodeFromId = null;
      formControlCode = this.sectionCodeFromCtrl;
      formControlName = this.sectionNameFromCtrl;
      nextFormName = 'sectionCodeToCtrl';
    }
    else {
      if (eventType != EVENT_TYPE.BLUR) {
        this.sectionCodeToTrigger.closePanel();
      }
      this.sectionCodeToId = null;
      formControlCode = this.sectionCodeToCtrl;
      formControlName = this.sectionNameToCtrl;
      nextFormName = 'sectionCodeFromCtrl';
    }

    if (StringUtil.IsNullOrEmpty(formControlCode.value)) {
      formControlCode.setValue("");
      formControlName.setValue("");

    } else {
      let value = StringUtil.setUpperCase(formControlCode.value);
      let findIndex = this.sections.findIndex((item) => {
        return (item.code === value);
      });

      if (0 <= findIndex) {
        if (type == 'from') {
          this.sectionCodeFromId = this.sections[findIndex].id;

          if (this.cbxSectionCtrl.value) {
            this.sectionCodeToId = this.sections[findIndex].id;
            this.sectionCodeToCtrl.setValue(this.sections[findIndex].code);
            this.sectionNameToCtrl.setValue(this.sections[findIndex].name);
          }
        } else {
          this.sectionCodeToId = this.sections[findIndex].id;
        }
        formControlCode.setValue(this.sections[findIndex].code);
        formControlName.setValue(this.sections[findIndex].name);
        this.setAddButton();

      } else {
        // formControlCode.setValue("");
        formControlName.setValue("");
      }
    }
    HtmlUtil.nextFocusByName(this.elementRef, nextFormName, eventType);
  }

}
