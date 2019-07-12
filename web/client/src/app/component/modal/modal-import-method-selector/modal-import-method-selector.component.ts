import { ComponentRef, Component, OnInit, EventEmitter, ViewContainerRef, ComponentFactoryResolver } from '@angular/core';
import { TABLE_INDEX, TABLE_NAME } from 'src/app/common/const/table-name.const';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { BaseComponent } from '../../common/base/base-component';
import { LoginUserMasterService } from 'src/app/service/Master/login-user-master.service';
import { BankBranchMasterService } from 'src/app/service/Master/bank-branch-master.service';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { StaffMasterService } from 'src/app/service/Master/staff-master.service';
import { BankAccountMasterService } from 'src/app/service/Master/bank-account-master.service';
import { AccountTitleMasterService } from 'src/app/service/Master/account-title-master.service';
import { JuridicalPersonalityMasterService } from 'src/app/service/Master/juridical-personality-master.service';
import { IgnoreKanaMasterService } from 'src/app/service/Master/ignore-kana-master.service';
import { HolidayCalendarMasterService } from 'src/app/service/Master/holiday-calendar-master.service';
import { DestinationMasterService } from 'src/app/service/Master/destination-master.service';
import { MasterImportSource } from 'src/app/model/master-import-source.model';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { ImportMethod } from 'src/app/common/const/kbn.const';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { ModalRouterProgressComponent } from '../modal-router-progress/modal-router-progress.component';
import { FileUtil } from 'src/app/common/util/file.util';
import { FILE_EXTENSION, ENCODE, MAX_FILE_SIZE } from 'src/app/common/const/eb-file.const';
import { DateUtil } from 'src/app/common/util/date-util';
import { CustomerGroupMasterService } from 'src/app/service/Master/customer-group-master.service';
import { KanaHistoryCustomerMasterService } from 'src/app/service/Master/kana-history-customer-master.service';
import { KanaHistoryPaymentAgencyMasterService } from 'src/app/service/Master/kana-history-payment-agency-master.service';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { CustomerFeeMasterService } from 'src/app/service/Master/customer-fee-master.service';
import { PROCESS_RESULT_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { MSG_ERR, MSG_INF, MSG_WNG } from 'src/app/common/const/message.const';
import { ModalWarningFileSizeComponent } from 'src/app/component/modal/modal-warning-file-size/modal-warning-file-size.component';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { SectionWithDepartmentMasterService } from 'src/app/service/Master/section-with-department-master.service';
import { SectionWithLoginUserMasterService } from 'src/app/service/Master/section-with-login-user-master.service';

@Component({
  selector: 'app-modal-import-method-selector',
  templateUrl: './modal-import-method-selector.component.html',
  styleUrls: ['./modal-import-method-selector.component.css']
})
export class ModalImportMethodSelectorComponent extends BaseComponent implements OnInit {
  public readonly encodeType = ENCODE;

  public importDisplayResult: string = '';  // インポート結果（画面表示用）

  public ImportMethod: typeof ImportMethod = ImportMethod; // インポートメソッド

  public tableIndex: TABLE_INDEX;  // テーブル種別

  public importSettingCode: string;

  public modalRouterProgressComponentRef: ComponentRef<ModalRouterProgressComponent>;

  public importFileCtrl: FormControl; // インポートファイル
  public importFileNameCtrl: FormControl;
  public cmbEncodeCtrl: FormControl;
  public cbxDownloadCtrl: FormControl;

  public importFile: any; // インポートファイルのイベント


  constructor(
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService, // ログインユーザー情報
    public customerService: CustomerMasterService, // 得意先
    public loginUserService: LoginUserMasterService,  // ログインユーザー
    public departmentService: DepartmentMasterService, // 請求部門
    public staffService: StaffMasterService,  // 営業担当者
    public bankAccountService: BankAccountMasterService,  // 銀行口座
    public accountTitleService: AccountTitleMasterService, // 科目
    public juridicalPersonalityService: JuridicalPersonalityMasterService, // 法人格,学習履歴データ管理
    public ignoreKanaService: IgnoreKanaMasterService, // 除外カナ
    public holidayCalendarService: HolidayCalendarMasterService,  // カレンダー
    public bankBranchService: BankBranchMasterService,  // 銀行・支店マスタ
    public destinationService: DestinationMasterService,  // 送付先
    public categoryService: CategoryMasterService, // カテゴリー
    public customerGropuService: CustomerGroupMasterService,
    public kanaHistoryCustomerService: KanaHistoryCustomerMasterService,
    public kanaHistoryPaymentService: KanaHistoryPaymentAgencyMasterService,
    public customerFeeService: CustomerFeeMasterService,
    public processResultService:ProcessResultService,
    public sectionService: SectionMasterService,
    public sectionWithDepartmentService: SectionWithDepartmentMasterService,
    public sectionWithLoginUserService: SectionWithLoginUserMasterService,
  ) {
    super();
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();

    this.cmbEncodeCtrl.setValue(this.encodeType[0].val);
    this.cbxDownloadCtrl.setValue(true);
  }

  public setControlInit() {
    this.importFileCtrl = new FormControl("");
    this.importFileNameCtrl = new FormControl("", [Validators.required]);
    this.cmbEncodeCtrl = new FormControl("");
    this.cbxDownloadCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      importFileCtrl: this.importFileCtrl,
      importFileNameCtrl: this.importFileNameCtrl,
      cmbEncodeCtrl: this.cmbEncodeCtrl,
      cbxDownloadCtrl: this.cbxDownloadCtrl
    })
  }

  public setFormatter() {
  }

  public clear() {
    this.MyFormGroup.reset();

    this.importFileNameCtrl.setValue("");
    this.importFile = null;
  }

  public get TableIndex(): TABLE_INDEX {
    return this.tableIndex;
  }
  public set TableIndex(value: TABLE_INDEX) {
    this.tableIndex = value;
  }
  public TableName(): String {
    return TABLE_NAME[this.tableIndex];
  }

  public closing = new EventEmitter<{}>();
  public get Closing() {
    return this.closing;
  }
  public set Closing(value) {
    this.closing = value;
  }

  public close() {
    this.ModalStatus = MODAL_STATUS_TYPE.CLOSE;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public cancel() {
    this.ModalStatus = MODAL_STATUS_TYPE.CANCEL;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }  

  public fileSelect(evt: any) {
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    if (evt.target.files.length == 0) return;
    if (evt.target.files[0].size > MAX_FILE_SIZE) {
      let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalWarningFileSizeComponent);
      let componentRef = this.viewContainerRef.createComponent(componentFactory);

      componentRef.instance.MainDetail = "ファイルサイズ超過";
      componentRef.instance.SufixDetail = "上限は" + Math.round(MAX_FILE_SIZE / 1024 / 1024) + "MBです。"

      componentRef.instance.Closing.subscribe(() => {
        componentRef.destroy();
      });
      this.importFile = null;
      this.importFileCtrl.setValue("");
    }
    else {
      this.importFile = evt.target.files[0];
      this.importFileNameCtrl.setValue(evt.target.files[0].name);
    }
  }

  public onDragOver(event: any) {
    event.preventDefault();
  }

  public onDrop(event: any) {
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    event.preventDefault();

    if (event.dataTransfer.files[0].size > MAX_FILE_SIZE) {
      let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalWarningFileSizeComponent);
      let componentRef = this.viewContainerRef.createComponent(componentFactory);

      componentRef.instance.MainDetail = "ファイルサイズ超過";
      componentRef.instance.SufixDetail = "上限は" + Math.round(MAX_FILE_SIZE / 1024 / 1024) + "MBです。"

      componentRef.instance.Closing.subscribe(() => {
        componentRef.destroy();
      });
      this.importFile = null;
      this.importFileCtrl.setValue("");
    }
    else {
      this.importFile = event.dataTransfer.files[0];
      this.importFileNameCtrl.setValue(event.dataTransfer.files[0].name)
    }
  }

  public get ImporterSettingCode() {
    return this.importSettingCode;
  }
  public set ImporterSettingCode(value) {
    this.importSettingCode = value;
  }

  public get ProcessModalCustomResult() {
    return this.processModalCustomResult;
  }
  public set ProcessModalCustomResult(value) {
    this.processModalCustomResult = value;
  }

  /**
   * サービスの設定
   */
  public getService(): any {
    let service: any;
    switch (this.tableIndex) {

      case TABLE_INDEX.MASTER_CUSTOMER:
        {
          service = this.customerService;
          break;
        }
      case TABLE_INDEX.MASTER_CUSTOMER_GROUP:
        {
          service = this.customerGropuService;
          break;
        }
      case TABLE_INDEX.MASTER_LOGIN_USER:
        {
          service = this.loginUserService;
          break;
        }
      case TABLE_INDEX.MASTER_DEPARTMENT:
        {
          service = this.departmentService;
          break;
        }
      case TABLE_INDEX.MASTER_STAFF:
        {
          service = this.staffService;
          break;
        }
      case TABLE_INDEX.MASTER_BANK_ACCOUNT:
        {
          service = this.bankAccountService;
          break;
        }
      case TABLE_INDEX.MASTER_EXCLUDE_CATEGORY:
        {
        }
      case TABLE_INDEX.MASTER_ACCOUNT_TITLE:
        {
          service = this.accountTitleService;
          break;
        }
      case TABLE_INDEX.MASTER_JURDICAL_PERSONALITY:
        {
          service = this.juridicalPersonalityService;
          break;
        }
      case TABLE_INDEX.MASTER_IGNORE_KANA:
        {
          service = this.ignoreKanaService;
          break;
        }
      case TABLE_INDEX.MASTER_HOLIDAY_CALENDAR:
        {
          service = this.holidayCalendarService;
          break;
        }
      case TABLE_INDEX.MASTER_BANK_BRANCH:
        {
          service = this.bankBranchService;
          break;
        }
      case TABLE_INDEX.MASTER_DESTINATION:
        {
          service = this.destinationService;
          break;
        }
      case TABLE_INDEX.MASTER_CATEGORY:
        {
          service = this.categoryService;
          break;
        }
      case TABLE_INDEX.MASTER_KANA_HISTORY_CUSTOMER:
        {
          service = this.kanaHistoryCustomerService;
          break;
        }
      case TABLE_INDEX.MASTER_KANA_HISTORY_PAYMENT_AGENCY:
        {
          service = this.kanaHistoryPaymentService;
          break;
        }
      case TABLE_INDEX.MASTER_CUSTOMER_FEE:
        {
          service = this.customerFeeService;
          break;
        }
      case TABLE_INDEX.MASTER_SECTION:
        {
          service = this.sectionService;
          break;
        }
      case TABLE_INDEX.MASTER_SECTION_WITH_DEPARTMENT:
        {
          service = this.sectionWithDepartmentService;
          break;
        }
      case TABLE_INDEX.MASTER_SECTION_WITH_LOGINUSER:
        {
          service = this.sectionWithLoginUserService;
          break;
        }          
      default:
        {
          service = null;
          break;
        }
    }

    return service;
  }

  /**
   * インポート
   */
  public import(importType: number) {
    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    this.modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    this.modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      this.modalRouterProgressComponentRef.destroy();
    });
    this.processModalCustomResult.status = PROCESS_RESULT_STATUS_TYPE.RUNNING;

    let reader = new FileReader();
    let service: any = this.getService();
    let isPrintErrorLog: boolean = false;
    this.importDisplayResult = "";

    // CSV読込
    reader.readAsText(this.importFile, this.cmbEncodeCtrl.value);

    new Promise((resolve, reject) => {
      reader.onload = () => {
        let masterImportSource: MasterImportSource = new MasterImportSource();

        masterImportSource.companyId = this.userInfoService.Company.id;
        masterImportSource.loginUserId = this.userInfoService.LoginUser.id;
        masterImportSource.importMethod = importType;
        if (this.importSettingCode != null) {
          masterImportSource.importerSettingCode = this.importSettingCode;
        }
        masterImportSource.data = btoa(unescape(encodeURIComponent(reader.result.toString())));

        service.Import(masterImportSource)
          .subscribe(
            response => {
              this.processModalCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
              if (response == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
                this.processModalCustomResult.result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
                this.processModalCustomResult.message = MSG_ERR.IMPORT_ERROR_WITH_LOG;
                this.modalRouterProgressComponentRef.destroy();
                isPrintErrorLog = true;

              } else {
                if (response && response.logs.length <= 0) {
                  this.processModalCustomResult.result = PROCESS_RESULT_RESULT_TYPE.SUCCESS;
                  this.processModalCustomResult.message = MSG_INF.FINISH_IMPORT;
                } else {
                  this.processModalCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
                  if (response) {
                    this.processModalCustomResult.message = MSG_WNG.IMPORT_COMPLETE_PART_OF_ERROR + LINE_FEED_CODE;

                    let importDisplayItem = new Array<String>();
                    importDisplayItem.push("追加：" + response.insertCount);
                    importDisplayItem.push("更新：" + response.updateCount);
                    importDisplayItem.push("削除：" + response.deleteCount);
                    this.processModalCustomResult.message += "(" + importDisplayItem.join(", ") + ")"

                  } else {
                    this.processModalCustomResult.result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
                    this.processModalCustomResult.message = MSG_ERR.IMPORT_ERROR_WITHOUT_LOG;
                  }
                  isPrintErrorLog = response.logs != undefined ? true: false;
                }
              }

              if (isPrintErrorLog && this.cbxDownloadCtrl.value) {
                this.downloadErrorLog(response.logs);
              }

              // 確認モーダルを閉じる
              this.modalRouterProgressComponentRef.destroy();
              this.close();
            }
          )
      };

      reader.onerror = () => {
        this.processModalCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
        this.processModalCustomResult.result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
        this.processModalCustomResult.message = MSG_ERR.IMPORT_ERROR_WITH_LOG;
        this.downloadErrorLog(reader.error);

        // 確認モーダルを閉じる
        this.modalRouterProgressComponentRef.destroy();
        this.close();
      };
    })
  }

  /**
   * エラーログを出力する
   * @param error エラー
   */
  public downloadErrorLog(error: any) {
    let errorMsg = new Array<String>();
    errorMsg.push(DateUtil.getYYYYMMDD(3));
    errorMsg.push(this.TableName() + "データ：" + this.importFileNameCtrl.value);
    if (error == undefined) {
      errorMsg.push('サーバーエラーが発生しました。');
    } else {
      errorMsg.push(error.join(LINE_FEED_CODE));
    }

    let errorData = new Array<any>();
    errorData.push(errorMsg.join(LINE_FEED_CODE));

    FileUtil.download(errorData, DateUtil.getYYYYMMDD(0) + "_Import", FILE_EXTENSION.LOG);
  }

}
