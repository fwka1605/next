import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { ModalImportMethodSelectorComponent } from 'src/app/component/modal/modal-import-method-selector/modal-import-method-selector.component';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { KEY_CODE, BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { LoginUserMasterService } from 'src/app/service/Master/login-user-master.service';
import { UsersResult } from 'src/app/model/users-result.model';
import { CustomValidators } from 'ng5-validation';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { LoginUser } from 'src/app/model/login-user.model';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, PROCESS_RESULT_STATUS_TYPE } from 'src/app/common/const/status.const';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { DateUtil } from 'src/app/common/util/date-util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { SectionWithLoginUserMasterService } from 'src/app/service/Master/section-with-login-user-master.service';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { ExistDeleteResult } from 'src/app/model/custom-model/process-result-custom.model';
import { MSG_WNG } from 'src/app/common/const/message.const';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { StringUtil } from 'src/app/common/util/string-util';
import { StaffMasterService } from 'src/app/service/Master/staff-master.service';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { CODE_TYPE, FunctionType } from 'src/app/common/const/kbn.const';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';
import { LoginService } from 'src/app/service/Master/login.service';

@Component({
  selector: 'app-pb0301-login-user-master',
  templateUrl: './pb0301-login-user-master.component.html',
  styleUrls: ['./pb0301-login-user-master.component.css']
})

export class Pb0301LoginUserMasterComponent extends BaseComponent implements OnInit {

  public FunctionType: typeof FunctionType = FunctionType;

  public panelOpenState;

  public usersResult: UsersResult;

  public userCodeCtrl: FormControl; // ユーザコード
  public userNameCtrl: FormControl; // ユーザ名

  public departmentCodeCtrl: FormControl; // 請求部門
  public departmentNameCtrl: FormControl;

  public mailCtrl: FormControl;  // メールアドレス

  public menuLevelCtrl: FormControl;  // 権限レベル

  public functionLevelCtrl: FormControl;  // セキュリティレベル

  public cbxUseClientCtrl: FormControl; // V-One利用

  public cbxInitializePasswordCtrl: FormControl;  // パスワード初期化

  public initialPasswordCtrl: FormControl;  // 初回パスワード
  public initialPasswordConfirmationCtrl: FormControl;  // 初期化

  public staffCodeCtrl: FormControl;  // 担当者コード
  public staffNameCtrl: FormControl;

  public undefineCtrl: FormControl;

  public selectIndex: number;
  public departmentId: number;
  public staffId: number;

  public isInvalidFlag: boolean = false;

  @ViewChild('departmentCodeInput', { read: MatAutocompleteTrigger }) departmentCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('staffCodeInput', { read: MatAutocompleteTrigger }) staffCodeTrigger: MatAutocompleteTrigger;

  @ViewChild('panel') panel:MatExpansionPanel;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public loginUserService: LoginUserMasterService,
    public userInfoService: UserInfoService,
    public sectionWithLoginUserService: SectionWithLoginUserMasterService,
    public processResultService: ProcessResultService,
    public staffService: StaffMasterService,
    public departmentMasterService: DepartmentMasterService,
    public loginService: LoginService

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
    this.getLoginUserData();

    this.cbxInitializePasswordCtrl.setValue(false);
    this.initialPasswordCtrl.disable();
    this.initialPasswordConfirmationCtrl.disable();
  }

  public setControlInit() {
    this.userCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.loginUserCodeLength)]);
    this.userNameCtrl = new FormControl("", [Validators.required]);

    this.departmentCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.departmentCodeLength)]);
    this.departmentNameCtrl = new FormControl("");

    this.mailCtrl = new FormControl("", [CustomValidators.email]);

    this.menuLevelCtrl = new FormControl("", [Validators.required]);

    this.functionLevelCtrl = new FormControl("", [Validators.required]);

    this.cbxUseClientCtrl = new FormControl("");

    this.cbxInitializePasswordCtrl = new FormControl("");

    this.initialPasswordCtrl = new FormControl("");
    this.initialPasswordConfirmationCtrl = new FormControl("", [CustomValidators.equalTo(this.initialPasswordCtrl)]);

    this.staffCodeCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.staffCodeLength)]);
    this.staffNameCtrl = new FormControl("");

    this.undefineCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      userCodeCtrl: this.userCodeCtrl,
      userNameCtrl: this.userNameCtrl,

      departmentCodeCtrl: this.departmentCodeCtrl,
      departmentNameCtrl: this.departmentNameCtrl,

      mailCtrl: this.mailCtrl,

      menuLevelCtrl: this.menuLevelCtrl,

      functionLevelCtrl: this.functionLevelCtrl,

      cbxUseClientCtrl: this.cbxUseClientCtrl,

      cbxInitializePasswordCtrl: this.cbxInitializePasswordCtrl,

      initialPasswordCtrl: this.initialPasswordCtrl,
      initialPasswordConfirmationCtrl: this.initialPasswordConfirmationCtrl,

      staffCodeCtrl: this.staffCodeCtrl,
      staffNameCtrl: this.staffNameCtrl,

      undefineCtrl: this.undefineCtrl,

    })
  }

  public setFormatter() {
    if (this.userInfoService.ApplicationControl.loginUserCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.userCodeCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.userCodeCtrl);
    }

    if (this.userInfoService.ApplicationControl.departmentCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.departmentCodeCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.departmentCodeCtrl);
    }

    if (this.userInfoService.ApplicationControl.staffCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.staffCodeCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.staffCodeCtrl);
    }

    FormatterUtil.setNumberFormatter(this.menuLevelCtrl);
    FormatterUtil.setNumberFormatter(this.functionLevelCtrl);
  }

  public clear() {
    this.MyFormGroup.reset();
    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;

    this.departmentId = null;
    this.staffId = null;

    // 更新処理のためユーザーコードを非活性にする。
    this.userCodeCtrl.enable();

    this.panelOpenState = false;
    this.panel.close();
  }

  public setAutoComplete() {

    // 請求部門
    this.initAutocompleteDepartments(this.departmentCodeCtrl, this.departmentMasterService, 0);
    // 担当者
    this.initAutocompleteStaffs(this.staffCodeCtrl, this.staffService, 0);

  }


  /**
   * 初回パスワードのフォーム切り替え
   */
  public selectPassword() {
    if (this.cbxInitializePasswordCtrl.value) {
      this.initialPasswordCtrl.enable();
      this.initialPasswordConfirmationCtrl.enable();

    } else {
      this.initialPasswordCtrl.disable();
      this.initialPasswordConfirmationCtrl.disable();
    }
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

      case BUTTON_ACTION.DELETE:
        this.delete();
        break;

      case BUTTON_ACTION.PRINT:
        this.print();
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
   * データ取得
   */
  public getLoginUserData() {
    this.usersResult = new UsersResult();

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   

    this.loginUserService.GetItems()
      .subscribe(response => {
        this.processCustomResult = this.processResultService.processAtGetData(this.processCustomResult, response, false, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.usersResult.users = response;
        }
        processComponentRef.destroy();
      })
  }

  /**
   * データ登録・編集
   */
  public registry() {
    let isRegistry: boolean = false;
    let registryData = new LoginUser();

    this.isInvalidFlag = this.loginService.checkPassword(this.processCustomResult, this.initialPasswordCtrl.value);
    if (this.isInvalidFlag) {
      return;
    }

    if (this.ComponentStatus == COMPONENT_STATUS_TYPE.CREATE) {
      registryData.id = 0;
      registryData.createBy = this.userInfoService.LoginUser.id;
      isRegistry = true;

    } else if (this.ComponentStatus == COMPONENT_STATUS_TYPE.UPDATE) {
      registryData = this.usersResult.users[this.selectIndex];

    } else {
      return;
    }

    registryData.code = this.userCodeCtrl.value;
    registryData.name = this.userNameCtrl.value;

    registryData.departmentId = this.departmentId;
    registryData.assignedStaffId = this.staffId == 0 ? null : this.staffId;
    registryData.staffCode = this.staffCodeCtrl.value;
    registryData.staffName = this.staffNameCtrl.value;
    registryData.mail = this.mailCtrl.value;
    registryData.menuLevel = this.menuLevelCtrl.value;
    registryData.functionLevel = this.functionLevelCtrl.value;
    registryData.useClient = this.cbxUseClientCtrl.value == true ? 1 : 0;
    registryData = FileUtil.replaceNull(registryData);
    if (this.cbxInitializePasswordCtrl.value) {
      registryData.initialPassword = this.initialPasswordCtrl.value;
    } else {
      registryData.initialPassword = null;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   

    this.loginUserService.Save(registryData)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, isRegistry, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.clear();
          this.getLoginUserData();
        }
        processComponentRef.destroy();
      })
  }

  /**
   * データ削除
   */
  public delete() {
    let loginUser = this.usersResult.users[this.selectIndex];
    let existDeleteResultList = new Array<ExistDeleteResult>();

    if (loginUser.id == this.userInfoService.LoginUser.id) {
      this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.CANNOT_DEL_CURRENT_LOGIN_USER, this.partsResultMessageComponent);
      return;
    }

    for (let i = 0; i < 1; i++) {
      existDeleteResultList[i] = new ExistDeleteResult();
      existDeleteResultList[i].idName = 'ログインユーザーID';
    }

    this.sectionWithLoginUserService.ExistLoginUser(loginUser.id).subscribe(
      response => {

        let responseList = new Array<any>();
        responseList.push(response);
        this.processCustomResult = this.processResultService.processAtExist(
          this.processCustomResult, responseList, existDeleteResultList, this.partsResultMessageComponent);

        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.WARNING) {
          return;
        }

        // 削除処理
        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
        let componentRef = this.viewContainerRef.createComponent(componentFactory);
        componentRef.instance.ActionName = "削除"
        componentRef.instance.Closing.subscribe(() => {

          componentRef.destroy();
          if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

            let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
            let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
            // processComponentRef.destroy();   

            this.loginUserService.Delete(loginUser.id)
              .subscribe(result => {
                this.processCustomResult = this.processResultService.processAtDelete(
                  this.processCustomResult, result, this.partsResultMessageComponent);
                if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
                  this.clear();
                  this.getLoginUserData();
                }
                processComponentRef.destroy();
              });
          }
        });
      },
      error => {
        console.log(error)
      }
    );
  }

  /**
   * モーダルからデータを取得する
   * @param table 取得するテーブル名
   * @param keyCode イベント
   */
  public openMasterModal(table: TABLE_INDEX) {

    if (this.departmentCodeTrigger!=null) {this.departmentCodeTrigger.closePanel();}
    if (this.staffCodeTrigger!=null) {this.staffCodeTrigger.closePanel();}

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {
          case TABLE_INDEX.MASTER_STAFF:
            {
              this.staffId = componentRef.instance.SelectedObject.id;
              this.staffCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.staffNameCtrl.setValue(componentRef.instance.SelectedName);
              break;
            }
          case TABLE_INDEX.MASTER_DEPARTMENT:
            {
              this.departmentId = componentRef.instance.SelectedObject.id;
              this.departmentCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.departmentNameCtrl.setValue(componentRef.instance.SelectedName);
              break;
            }
        }

      }

      componentRef.destroy();
    });
  }

  /**
   * 印刷用のPDFをダウンロードする
   */
  public print() {
    let result = false;
    this.loginUserService.GetReport()
      .subscribe(response => {
        try {
          FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
          result = true;

        } catch (error) {
          console.error(error);
        }
        this.processResultService.processAtOutput(
          this.processCustomResult, result, 1, this.partsResultMessageComponent);
        this.openOptions();
      });
  }

  /**
   * インポート
   */
  public openImportMethodSelector() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalImportMethodSelectorComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = TABLE_INDEX.MASTER_LOGIN_USER;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.getLoginUserData();
    });
    this.openOptions();
  }

  /**
   * エクスポート
   */
  public export() {
    let dataList = this.usersResult.users;
    let deliveryOption: boolean = this.userInfoService.ApplicationControl.useDistribution == 0 ? false : true;
    let headerList = REPORT_HEADER.LOGIN_USER_MASTER;
    let result = false;

    // 件数チェック
    if (dataList.length <= 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.NO_EXPORT_DATA, this.partsResultMessageComponent);
      return;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   

    // ヘッダー設定
    if (deliveryOption) {
      headerList = headerList.concat(REPORT_HEADER.LOGIN_USER_MASTER_OPTION);
    }

    let headers = FileUtil.encloseItemBySymbol(headerList);
    let data: string = headers.join(",") + LINE_FEED_CODE;

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];
      dataItem.push(this.userInfoService.Company.code);
      dataItem.push(dataList[index].code);
      dataItem.push(dataList[index].name);
      dataItem.push(dataList[index].departmentCode);
      dataItem.push(dataList[index].mail);
      dataItem.push(dataList[index].menuLevel);
      dataItem.push(dataList[index].functionLevel);
      dataItem.push(dataList[index].staffCode);
      dataItem.push(dataList[index].initialPassword);

      if (deliveryOption) {
        dataItem.push(dataList[index].useClient);
        dataItem.push(dataList[index].useWebViewer);
      }

      dataItem = FileUtil.encloseItemBySymbol(dataItem);

      data = data + dataItem.join(",") + LINE_FEED_CODE;
    }
    let resultDatas: Array<any> = [];
    resultDatas.push(data);

    try {
      FileUtil.download(resultDatas, this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
      result = true;

    } catch (error) {
      console.error(error);
    }
    this.processResultService.processAtOutput(
      this.processCustomResult, result, 0, this.partsResultMessageComponent);
    this.openOptions();

    processComponentRef.destroy();
  }

  /**
   * 選択したデータをフォームに表示する
   * @param index 選択した行番号
   */
  public selectLine(index: number) {

    this.panelOpenState = true;
    this.panel.open();
    
    this.selectIndex = index;
    let userData = this.usersResult.users[index];

    this.userCodeCtrl.setValue(userData.code);
    this.userNameCtrl.setValue(userData.name);

    this.departmentCodeCtrl.setValue(userData.departmentCode);  // 請求部門
    this.departmentNameCtrl.setValue(userData.departmentName);
    this.departmentId = userData.departmentId;

    this.mailCtrl.setValue(userData.mail);

    this.menuLevelCtrl.setValue(userData.menuLevel);
    this.cbxUseClientCtrl.setValue(userData.useClient == 1 ? true : false);

    this.functionLevelCtrl.setValue(userData.functionLevel);

    this.cbxUseClientCtrl.setValue(userData.useClient);

    this.cbxInitializePasswordCtrl.setValue(userData.initialPassword);

    this.initialPasswordCtrl.setValue("");
    this.initialPasswordConfirmationCtrl.setValue("");

    this.staffCodeCtrl.setValue(userData.staffCode);
    this.staffNameCtrl.setValue(userData.staffName);
    this.setStaffCode(null);

    this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;

    this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    // 更新処理のためユーザーコードを非活性にする。
    this.userCodeCtrl.disable();



  }


  ///// Enterキー押下時の処理 //////////////////////////////////////////////////////
  public setUserCode(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.userCodeCtrl.value)) {
      let codeValue = StringUtil.setUpperCase(this.userCodeCtrl.value);
      this.userCodeCtrl.setValue(codeValue);
      let findIndex = this.usersResult.users.findIndex((item) => {
        return (item.code === codeValue);
      });
      if (0 <= findIndex) {
        this.selectLine(findIndex);
      }
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'userNameCtrl', eventType);
  }

  public setUserName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'departmentCodeCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////

  public setDepartmentCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      if (this.departmentCodeTrigger!=null) {this.departmentCodeTrigger.closePanel();}
    }

    if (!StringUtil.IsNullOrEmpty(this.departmentCodeCtrl.value)) {
      this.loadStart();
      this.departmentMasterService.GetItems(this.departmentCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response != null && 0 < response.length) {
            this.departmentCodeCtrl.setValue(response[0].code);
            this.departmentNameCtrl.setValue(response[0].name);
            this.departmentId = response[0].id;
            HtmlUtil.nextFocusByName(this.elementRef, 'mailCtrl', eventType);
          } else {
            //this.departmentCodeCtrl.setValue("");
            this.departmentNameCtrl.setValue("");
            this.departmentId = null;
          }
        });
    } else {
      this.departmentCodeCtrl.setValue("");
      this.departmentNameCtrl.setValue("");
      this.departmentId = null;
      HtmlUtil.nextFocusByName(this.elementRef, 'mailCtrl', eventType);
    }
  }

  ///////////////////////////////////////////////////////////////////////
  public setMail(eventType: string) {
    HtmlUtil.nextFocusByNames(this.elementRef, ['initialPasswordCtrl', 'staffCodeCtrl'], eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setInitialPassword(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'initialPasswordConfirmationCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setInitialPasswordConfirmation(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeCtrl', eventType);
  }

  /////////////////////////////////////////////////////////////////////////////////

  public setStaffCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      if (this.staffCodeTrigger!=null) {this.staffCodeTrigger.closePanel();}
    }

    if (!StringUtil.IsNullOrEmpty(this.staffCodeCtrl.value)) {

      this.loadStart();
      this.staffService.GetItems(this.staffCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response != null && 0 < response.length) {
            this.staffCodeCtrl.setValue(response[0].code);
            this.staffNameCtrl.setValue(response[0].name);
            this.staffId = response[0].id;
            HtmlUtil.nextFocusByName(this.elementRef, 'menuLevelCtrl', eventType);
          } else {
            //this.staffCodeCtrl.setValue("");
            this.staffNameCtrl.setValue("");
            this.staffId = 0;
          }
        });
    }
    else {
      this.staffCodeCtrl.setValue("");
      this.staffNameCtrl.setValue("");
      this.staffId = 0;
      HtmlUtil.nextFocusByName(this.elementRef, 'menuLevelCtrl', eventType);
    }

  }

  ///////////////////////////////////////////////////////////////////////
  public setMenuLevel(eventType: string) {

    let menuLevel: number = parseInt(this.menuLevelCtrl.value);

    if (menuLevel <= 0 || menuLevel > 4) {
      menuLevel = 4;
    }
    this.menuLevelCtrl.setValue("" + menuLevel);

    HtmlUtil.nextFocusByName(this.elementRef, 'functionLevelCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setFunctionLevel(eventType: string) {

    let securityLevel: number = parseInt(this.functionLevelCtrl.value);

    if (securityLevel <= 0 || securityLevel > 6) {
      securityLevel = 6;
    }
    this.functionLevelCtrl.setValue("" + securityLevel);

    HtmlUtil.nextFocusByName(this.elementRef, 'userCodeCtrl', eventType);
  }

}
