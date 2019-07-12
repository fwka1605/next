import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { Company } from 'src/app/model/company.model';
import { CompanyMasterService } from 'src/app/service/Master/company-master.service';
import { CompanySearch } from 'src/app/model/company-search.model';
import { FileUtil } from 'src/app/common/util/file.util';
import { BUTTON_ACTION } from 'src/app/common/const/event.const';
import { PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { PartsCompanyComponent } from 'src/app/component/view-parts/parts-company/parts-company.component';

@Component({
  selector: 'app-pb0101-company-master',
  templateUrl: './pb0101-company-master.component.html',
  styleUrls: ['./pb0101-company-master.component.css']
})
export class Pb0101CompanyMasterComponent extends BaseComponent implements OnInit {

  /** 会社情報フォーム */
  @ViewChild('partsCompany') public readonly partsCompany: PartsCompanyComponent;

  public isMessage: boolean = false;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public companyMasterService: CompanyMasterService,
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
    // this.clear();
  }

  public clear() {
    this.partsCompany.clear();
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
        this.isMessage = false;
        this.registry();
        break;

      case BUTTON_ACTION.REDISPLAY:
        this.isMessage = true;
        this.redisplay();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
  }

  /**
   * データ編集
   */
  public registry() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();

    let company = new Company();
    // 編集不可
    company.id = this.userInfoService.Company.id;
    company.code = this.userInfoService.Company.code;
    company.productKey = this.userInfoService.Company.productKey;
    company.updateBy = this.userInfoService.LoginUser.id;

    // 会社情報
    company.name = this.partsCompany.companyNameCtrl.value;
    company.kana = this.partsCompany.companyNameKanaCtrl.value;
    company.postalCode = this.partsCompany.postalCodeCtrl.value;
    company.address1 = this.partsCompany.address1Ctrl.value;
    company.address2 = this.partsCompany.address2Ctrl.value;
    company.tel = this.partsCompany.telCtrl.value;
    company.fax = this.partsCompany.faxCtrl.value;
    company.bankAccountName = this.partsCompany.bankAccountNameCtrl.value;
    company.bankAccountKana = this.partsCompany.bankAccountNameKanaCtrl.value;

    // 銀行情報
    company.bankName1 = this.partsCompany.bankName1Ctrl.value;
    company.branchName1 = this.partsCompany.branchName1Ctrl.value;
    company.accountType1 = this.partsCompany.cmbAccountType1Ctrl.value;
    company.accountNumber1 = this.partsCompany.accountNumber1Ctrl.value;
    company.bankName2 = this.partsCompany.bankName2Ctrl.value;
    company.branchName2 = this.partsCompany.branchName2Ctrl.value;
    company.accountType2 = this.partsCompany.cmbAccountType2Ctrl.value;
    company.accountNumber2 = this.partsCompany.accountNumber2Ctrl.value;
    company.bankName3 = this.partsCompany.bankName3Ctrl.value;
    company.branchName3 = this.partsCompany.branchName3Ctrl.value;
    company.accountType3 = this.partsCompany.cmbAccountType3Ctrl.value;
    company.accountNumber3 = this.partsCompany.accountNumber3Ctrl.value;
    company = FileUtil.replaceNull(company);

    // オプション
    company.closingDay = this.partsCompany.closingDateCtrl.value;
    // company.presetCodeSearchDialog = this.partsCompany.cbxPresetCodeSearchDialogCtrl.value == true ? 1 : 0;
    // company.showConfirmDialog = this.partsCompany.cbxShowConfirmDialogCtrl.value == true ? 1 : 0;
    // company.showWarningDialog = this.partsCompany.cbxShowWarningDialogCtrl.value == true ? 1 : 0;
    // company.autoCloseProgressDialog = this.partsCompany.cbxAutoCloseProgressDialogCtrl.value == true ? 1 : 0;
    company.transferAggregate = this.partsCompany.cbxTransferAggregateCtrl.value == true ? 1 : 0;

    this.companyMasterService.Save(company)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, false, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.redisplay();
          componentRef.destroy();

        }
      });
  }

  /**
   * 再表示
   */
  public redisplay() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();

    let companySearch = new CompanySearch();
    companySearch.id = this.userInfoService.Company.id;
    companySearch.code = this.userInfoService.Company.code;
    companySearch.name = this.userInfoService.Company.name;

    this.companyMasterService.GetItems(companySearch.code)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtGetData(this.processCustomResult, result, this.isMessage, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.partsCompany.setUserInfo(result);
        }
        componentRef.destroy();
      });
  }

}
