import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, ViewChild } from '@angular/core';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { TABLE_INDEX, TABLE_MAX_DISPLAY_INDEX } from 'src/app/common/const/table-name.const';
import { DateUtil } from 'src/app/common/util/date-util';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { FileUtil } from 'src/app/common/util/file.util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { StringUtil } from 'src/app/common/util/string-util';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { LogData } from 'src/app/model/log-data.model';
import { LogDataSearch } from 'src/app/model/log-data-search.model';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { FunctionAuthorityMasterService } from 'src/app/service/Master/function-authority-master.service';
import { LoginUserMasterService } from 'src/app/service/Master/login-user-master.service';
import { LogDataService } from 'src/app/service/log-data.service';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { CODE_TYPE } from 'src/app/common/const/kbn.const';
import { MSG_WNG } from 'src/app/common/const/message.const';
import { NumberUtil } from 'src/app/common/util/number-util';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-ph0901-log-data-maintenance',
  templateUrl: './ph0901-log-data-maintenance.component.html',
  styleUrls: ['./ph0901-log-data-maintenance.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]

})
export class Ph0901LogDataMaintenanceComponent extends BaseComponent implements OnInit {

  @ViewChild('loginUserCodeInput', { read: MatAutocompleteTrigger }) loginUserCodeTrigger: MatAutocompleteTrigger;

  @ViewChild('panel') panel:MatExpansionPanel;


  public logDatas: LogData[];

  public panelOpenState: boolean;
  public isDeletionDisabled: boolean;

  public loggedAtFromCtrl: FormControl;  // ログ日次
  public loggedAtToCtrl: FormControl;

  public loginUserCodeCtrl: FormControl;  // ログインユーザ
  public loginUserNameCtrl: FormControl;

  public undefineCtrl: FormControl; // 未定用

  public LogStartDataMsg: string;
  public LogCounterMsg: string;
  public logMaxCount: string;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public logDataService: LogDataService,
    public functionAuthorityService: FunctionAuthorityMasterService,
    public loginUserService: LoginUserMasterService,
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
    this.setAutoComplete();
    this.initializeValues();
    this.getStartLogInfo();

    this.clear();
  }

  public setControlInit() {

    this.loggedAtFromCtrl = new FormControl(null, [Validators.maxLength(10)]);  // ログ日時
    this.loggedAtToCtrl = new FormControl(null, [Validators.maxLength(10)]);

    this.loginUserCodeCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.loginUserCodeLength)]);  // ログインユーザ
    this.loginUserNameCtrl = new FormControl("");

    this.undefineCtrl = new FormControl(""); // 未定用


  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      loggedAtFromCtrl: this.loggedAtFromCtrl,  // ログ日時
      loggedAtToCtrl: this.loggedAtToCtrl,

      loginUserCodeCtrl: this.loginUserCodeCtrl,  // ログインユーザ
      loginUserNameCtrl: this.loginUserNameCtrl,

      undefineCtrl: this.undefineCtrl, // 未定用;


    })
  }

  public setFormatter() {
    if (this.userInfoService.ApplicationControl.loginUserCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.loginUserCodeCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.loginUserCodeCtrl);
    }
  }

  public setAutoComplete() {
    this.initAutocompleteLoginUsers(this.loginUserCodeCtrl, this.loginUserService, 0);
  }

  public initializeValues() {
    // this.functionAuthorityService.GetItems()
    this.isDeletionDisabled = false;
  }


  public openMasterModal(table: TABLE_INDEX, keyCode: string = null) {

    if (keyCode != EVENT_TYPE.BLUR) {
      this.loginUserCodeTrigger.closePanel();
    }

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {
          case TABLE_INDEX.MASTER_LOGIN_USER: {
            this.loginUserCodeCtrl.setValue(componentRef.instance.SelectedCode);
            this.loginUserNameCtrl.setValue(componentRef.instance.SelectedName);
            break;
          }
        }
      }
      componentRef.destroy();
    });
  }

  public clear() {
    this.MyFormGroup.reset();
    this.logDatas = [];

    HtmlUtil.nextFocusByName(this.elementRef, 'loggedAtFromCtrl', EVENT_TYPE.NONE);

    this.panelOpenState = true;
    this.panel.open();

    this.getStartLogInfo();

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
      case BUTTON_ACTION.SEARCH:
        this.search();
        break;
      case BUTTON_ACTION.EXPORT:
        this.export();
        break;
      default:
        console.log('buttonAction Error.');
        break;
    }
  }

  public getStartLogInfo() {
    this.logDataService.getItems(this.getSearchOption())
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
          this.LogStartDataMsg = DateUtil.getYYYYMMDD(5, response[response.length - 1].loggedAt);
          this.logMaxCount = NumberUtil.Separate(response.length);
          this.LogCounterMsg = '0 / ' + this.logMaxCount + ' 件';

        } else if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length == 0) {
          this.LogStartDataMsg = '0　件';
        }
      });
  }

  public search() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();

    this.logDataService.getItems(this.getSearchOption())
      .subscribe(response => {
        this.processCustomResult = this.processResultService.processAtGetData(
          this.processCustomResult, response, true, this.partsResultMessageComponent);
        let maxLen = 0;
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
          this.logDatas = response;
          this.panelOpenState = false;
          maxLen = TABLE_MAX_DISPLAY_INDEX < response.length ? TABLE_MAX_DISPLAY_INDEX : response.length;

        }
        else if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length === 0) {
          this.logDatas = [];
          this.panelOpenState = true;
        }
        this.LogCounterMsg = NumberUtil.Separate(maxLen) + ' / ' + this.logMaxCount + ' 件';

        componentRef.destroy();
      });
  }

  public getSearchOption(): LogDataSearch {
    const obj: LogDataSearch = {
      companyId: this.userInfoService.Company.id,
      loggedAtFrom: DateUtil.ConvertFromDatepickerToStart(this.loggedAtFromCtrl),
      loggedAtTo: DateUtil.ConvertFromDatepickerToEnd(this.loggedAtToCtrl),
      loginUserCode: this.loginUserCodeCtrl.value,
    };
    return obj;
  }

  public export() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();

    const header = [[
      '日時',
      'ユーザーコード',
      'ユーザー名',
      '名称',
      '操作',
    ].join(','),
    ];
    const lines = header.concat(this.logDatas.map(x => {
      const line = [
        DateUtil.getYYYYMMDD(5, x.loggedAt),
        x.loginUserCode,
        x.loginUserName,
        x.menuName,
        x.operationName,
      ];
      return line.map(field => {
        let requireEscape = field.includes(',') || field.includes('"');
        field = field.replace(/"/g, '""');
        if (requireEscape) {
          field = `"${field}"`;
        }
        return field;
      }).join(',');
    }));
    const data = [
      lines.join(LINE_FEED_CODE)
    ];

    let isTryResult: boolean = false;
    try {
      FileUtil.download(data, `${this.Title}${DateUtil.getYYYYMMDD(0)}`, FILE_EXTENSION.CSV);
      isTryResult = true;

    } catch (error) {
      console.error(error);
    }
    this.processResultService.processAtOutput(
      this.processCustomResult, isTryResult, 0, this.partsResultMessageComponent);

    componentRef.destroy();
  }

  public delete() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = '削除';
    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let componentRef = this.viewContainerRef.createComponent(componentFactory);
        // componentRef.destroy();

        this.logDataService.deleteAll()
          .subscribe(response => {
            this.processResultService.processAtDelete(
              this.processCustomResult, response, this.partsResultMessageComponent);
            if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
              this.clear();
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

  public setPayerName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);
  }

  public setLoggedAtFrom(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'loggedAtToCtrl', eventType);
  }

  public setLoggedAtTo(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'loginUserCodeCtrl', eventType);
  }

  public setLoginUserCode(eventType: string) {
    if (eventType != EVENT_TYPE.BLUR) {
      this.loginUserCodeTrigger.closePanel();
    }

    if (StringUtil.IsNullOrEmpty(this.loginUserCodeCtrl.value)) {
      this.loginUserNameCtrl.setValue('');
    }
    else {
      this.loadStart();
      this.loginUserService.GetItems(this.loginUserCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.loginUserCodeCtrl.setValue(response[0].code);
            this.loginUserNameCtrl.setValue(response[0].name);
          }
          else {
            this.loginUserCodeCtrl.setValue('');
            this.loginUserNameCtrl.setValue('');
          }
        });
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'loggedAtFromCtrl', eventType);
  }

}

