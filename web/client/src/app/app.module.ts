//import { Component } from '@angular/Core';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { HttpClientModule, HttpClientXsrfModule } from '@angular/common/http';
import { DatePipe }           from '@angular/common';
import { DecimalPipe }        from '@angular/common';

// 追加！
import { MaterialModule } from './material-module';
import { MatNativeDateModule } from '@angular/material';



// 追加！
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { LOCALE_ID } from '@angular/core';

import { AppComponent } from './app.component';

// 追加
import { Ng2FlatpickrModule } from 'ng2-flatpickr';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

// 画面コンポーネント
import { Pa0101LoginComponent } from './component/login/PA0100/pa0101-login/pa0101-login.component';
import { Pa0201LogoutComponent } from './component/logout/PA0200/pa0201-logout/pa0201-logout.component';
import { Pb0101CompanyMasterComponent } from './component/config/PB0100/pb0101-company-master/pb0101-company-master.component';
import { Pb0501CustomerMasterComponent } from './component/config/PB0500/pb0501-customer-master/pb0501-customer-master.component';
import { Pb0301LoginUserMasterComponent } from './component/config/PB0300/pb0301-login-user-master/pb0301-login-user-master.component';
import { Pb0201DepartmentMasterComponent } from './component/config/PB0200/pb0201-department-master/pb0201-department-master.component';
import { Pb0401StaffMasterComponent } from './component/config/PB0400/pb0401-staff-master/pb0401-staff-master.component';
import { Pb0601CustomerGroupMasterComponent } from './component/config/PB0600/pb0601-customer-group-master/pb0601-customer-group-master.component';
import { Pb0801BankAccountMasterComponent } from './component/config/PB0800/pb0801-bank-account-master/pb0801-bank-account-master.component';
import { Pb0701AccountTitleMasterComponent } from './component/config/PB0700/pb0701-account-title-master/pb0701-account-title-master.component';
import { Pb0901CategoryMasterComponent } from './component/config/PB0900/pb0901-category-master/pb0901-category-master.component';
import { Pb1401GeneralSettingMasterComponent } from './component/config/PB1400/pb1401-general-setting-master/pb1401-general-setting-master.component';
import { Pb1001KanaHistoryMasterComponent } from './component/config/PB1000/pb1001-kana-history-master/pb1001-kana-history-master.component';
import { Pb1501IgnoreKanaMasterComponent } from './component/config/PB1500/pb1501-ignore-kana-master/pb1501-ignore-kana-master.component';
import { Pb1601CalendarMasterComponent } from './component/config/PB1600/pb1601-calendar-master/pb1601-calendar-master.component';
import { Pb1701JuridicalPersonalityMasterComponent } from './component/config/PB1700/pb1701-juridical-personality-master/pb1701-juridical-personality-master.component';
import { Pb1801BankBranchMasterComponent } from './component/config/PB1800/pb1801-bank-branch-master/pb1801-bank-branch-master.component';
import { Pc0101BillingImporterComponent } from "./component/invoice/PC0100/pc0101-billing-importer/pc0101-billing-importer.component";
import { Pc0201BillingInputComponent } from './component/invoice/PC0200/pc0201-billing-input/pc0201-billing-input.component';
import { Pc0301BillingSearchComponent } from './component/invoice/PC0300/pc0301-billing-search/pc0301-billing-search.component';
import { Pc0501BillingDueAtModifyComponent } from './component/invoice/PC0500/pc0501-billing-due-at-modify/pc0501-billing-due-at-modify.component';
import { Pc0601BillingJournalizingComponent } from './component/invoice/PC0600/pc0601-billing-journalizing/pc0601-billing-journalizing.component';
import { Pd0201ReceiptImporterComponent } from './component/receive/PD0200/pd0201-receipt-importer/pd0201-receipt-importer.component';
import { Pd0301ReceiptInputComponent } from './component/receive/PD0300/pd0301-receipt-input/pd0301-receipt-input.component';
import { Pd0401ReceiptApportionComponent } from './component/receive/PD0400/pd0401-receipt-apportion/pd0401-receipt-apportion.component';
import { Pd0501ReceiptSearchComponent } from './component/receive/PD0500/pd0501-receipt-search/pd0501-receipt-search.component';
import { Pd0601ReceiptAdvanceReceivedComponent } from './component/receive/PD0600/pd0601-receipt-advance-received/pd0601-receipt-advance-received.component';
import { Pd0701ReceiptJournalizingComponent } from './component/receive/PD0700/pd0701-receipt-journalizing/pd0701-receipt-journalizing.component';
import { Pe0101MatchingSequentialComponent } from './component/clearing/PE0100/pe0101-matching-sequential/pe0101-matching-sequential.component';
import { Pe0102MatchingIndividualComponent } from './component/clearing/PE0100/pe0102-matching-individual/pe0102-matching-individual.component';
import { Pe0201MatchingJournalizingComponent } from './component/clearing/PE0200/pe0201-matching-journalizing/pe0201-matching-journalizing.component';
import { Pe0301MatchingHistorySearchComponent } from './component/clearing/PE0300/pe0301-matching-history-search/pe0301-matching-history-search.component';
import { Pe0401MatchingJournalizingCancellationComponent } from './component/clearing/PE0400/pe0401-matching-journalizing-cancellation/pe0401-matching-journalizing-cancellation.component';
import { Pe0501BillingOmitComponent } from './component/clearing/PE0500/pe0501-billing-omit/pe0501-billing-omit.component';
import { Pe0601ReceiptOmitComponent } from './component/clearing/PE0600/pe0601-receipt-omit/pe0601-receipt-omit.component';
import { Pf0101BillingAgingListComponent } from './component/form/PF0100/pf0101-billing-aging-list/pf0101-billing-aging-list.component';
import { Pf0201CreditAgingListComponent } from './component/form/PF0200/pf0201-credit-aging-list/pf0201-credit-aging-list.component';
import { Pf0301ScheduledPaymentListComponent } from './component/form/PF0300/pf0301-scheduled-payment-list/pf0301-scheduled-payment-list.component';
import { Pf0401ArrearagesListComponent } from './component/form/PF0400/pf0401-arrearages-list/pf0401-arrearages-list.component';
import { Pf0501CustomerLedgerComponent } from './component/form/PF0500/pf0501-customer-ledger/pf0501-customer-ledger.component';
import { Ph0201DataMaintenanceComponent } from './component/maintenance/PH0200/ph0201-data-maintenance/ph0201-data-maintenance.component';
import { Ph0101SettingSsecurityComponent } from './component/maintenance/PH0100/ph0101-setting-ssecurity/ph0101-setting-ssecurity.component';
import { Ph0701GridSettingComponent } from './component/maintenance/PH0700/ph0701-grid-setting/ph0701-grid-setting.component';
import { Ph0801CollationSettingComponent } from './component/maintenance/PH0800/ph0801-collation-setting/ph0801-collation-setting.component';
import { Ph0901LogDataMaintenanceComponent } from './component/maintenance/PH0900/ph0901-log-data-maintenance/ph0901-log-data-maintenance.component';
import { Ph1001ColumnSettingComponent } from './component/maintenance/PH1000/ph1001-column-setting/ph1001-column-setting.component';
import { Sub0101ManualComponent } from './component/sub/SUB0100/sub0101-manual/sub0101-manual.component';
import { Sub0201InquiryComponent } from './component/sub/SUB0200/sub0201-inquiry/sub0201-inquiry.component';
import { Com0101HeaderComponent } from './component/common/COM0100/com0101-header/com0101-header.component';
import { Com0201FooterComponent } from './component/common/COM0200/com0201-footer/com0201-footer.component';
import { Com0301MenuComponent } from './component/common/COM0300/com0301-menu/com0301-menu.component';
import { AppRoutingModule } from './app-routing.module';
import { Com0401ErrorComponent } from './component/common/COM0400/com0401-error/com0401-error.component';
import { Com0000MainComponent } from './component/common/com0000/com0000-main/com0000-main.component';
import { Pd0101EbFileImporterComponent } from './component/receive/PD0100/pd0101-eb-file-importer/pd0101-eb-file-importer.component';
import { Pf0601CollectionScheduleComponent } from './component/form/PF0600/pf0601-collection-schedule/pf0601-collection-schedule.component';
import { ModalCustomerDetailComponent } from './component/modal/modal-customer-detail/modal-customer-detail.component';
import { ModalMasterComponent } from './component/modal/modal-master/modal-master.component';
import { RacCurrencyPipe } from './pipe/currency.pipe';
import { ModalImporterSettingComponent } from './component/modal/modal-importer-setting/modal-importer-setting.component';
import { Com0501PageTitleComponent } from './component/common/COM0500/com0501-pagetitle/com0501-pagetitle.component';
import { ModalInvoiceMemoComponent } from './component/modal/modal-invoice-memo/modal-invoice-memo.component';
import { ModalImportMethodSelectorComponent } from './component/modal/modal-import-method-selector/modal-import-method-selector.component';
import { ModalEbExcludeAccountingSettingComponent } from './component/modal/modal-eb-exclude-accounting-setting/modal-eb-exclude-accounting-setting.component';
import { ModalEbFileSettingComponent } from './component/modal/modal-eb-file-setting/modal-eb-file-setting.component';
import { ModalMemoComponent } from './component/modal/modal-memo/modal-memo.component';
import { ModalMasterBankComponent } from './component/modal/modal-master-bank/modal-master-bank.component';
import { ModalMasterBankAccountComponent } from './component/modal/modal-master-bank-account/modal-master-bank-account.component';
import { ModalFormSettingComponent } from './component/modal/modal-form-setting/modal-form-setting.component';
import { Pb1901PaymentAgencyMasterComponent } from './component/config/PB1900/pb1901-payment-agency-master/pb1901-payment-agency-master.component';
import { Pc0701AccountTransferCreateComponent } from './component/invoice/PC0700/pc0701-account-transfer-create/pc0701-account-transfer-create.component';
import { Pc0801AccountTransferImportComponent } from './component/invoice/PC0800/pc0801-account-transfer-import/pc0801-account-transfer-import.component';
import { Pc1801MfBillingExtractComponent } from './component/invoice/PC1800/pc1801-mf-billing-extract/pc1801-mf-billing-extract.component';
import { Pe1001MfMatchingOutputComponent } from './component/clearing/PE1000/pe1001-mf-matching-output/pe1001-mf-matching-output.component';
import { Ph1401MfWebApiSettingComponent } from './component/maintenance/PH1400/ph1401-mf-web-api-setting/ph1401-mf-web-api-setting.component';
import { Com0601TopComponent } from './component/common/COM0600/com0601-top/com0601-top.component';
import { Com0402SiteErrorComponent } from './component/common/COM0400/com0402-site-error/com0402-site-error.component';
import { Pj0101UserSettingComponent } from './component/user/pj0100/pj0101-user-setting/pj0101-user-setting.component';
import { Pj0201UserPasswordReissueComponent } from './component/user/pj0200/pj0201-user-password-reissue/pj0201-user-password-reissue.component';
import { ModalConfirmComponent } from './component/modal/modal-confirm/modal-confirm.component';
import { ModalMultiMasterComponent } from './component/modal/modal-multi-master/modal-multi-master.component';
import { ModalRouterProgressComponent } from './component/modal/modal-router-progress/modal-router-progress.component';
import { ModalReceiptExcludeAllComponent } from './component/modal/modal-receipt-exclude-all/modal-receipt-exclude-all.component';
import { ModalReceiptExcludeDetailComponent } from './component/modal/modal-receipt-exclude-detail/modal-receipt-exclude-detail.component';
import { ModalImporterSettingCustomerComponent } from './component/modal/modal-importer-setting-customer/modal-importer-setting-customer.component';
import { ModalImportMethodSelectorCustomerComponent } from './component/modal/modal-import-method-selector-customer/modal-import-method-selector-customer.component';
import { ModalExportMethodSelectorCustomerComponent } from './component/modal/modal-export-method-selector-customer/modal-export-method-selector-customer.component';
import { CategoryBillingComponent } from './component/config/PB0900/pb0901-category-master/category-billing/category-billing.component';
import { CategoryReceiptComponent } from './component/config/PB0900/pb0901-category-master/category-receipt/category-receipt.component';
import { CategoryCollectionComponent } from './component/config/PB0900/pb0901-category-master/category-collection/category-collection.component';
import { CategoryExcludeComponent } from './component/config/PB0900/pb0901-category-master/category-exclude/category-exclude.component';
import { ModalMatchingRecordedAtComponent } from './component/modal/modal-matching-recorded-at/modal-matching-recorded-at.component';
import { ModalCollatePrintComponent } from './component/modal/modal-collate-print/modal-collate-print.component';
import { ModalTransferFeeComponent } from './component/modal/modal-transfer-fee/modal-transfer-fee.component';
import { ModalIndividualMemoComponent } from './component/modal/modal-individual-memo/modal-individual-memo.component';
import { ModalSelectParentCustomerComponent } from './component/modal/modal-select-parent-customer/modal-select-parent-customer.component';
import { ModalConfirmMatchingComponent } from './component/modal/modal-confirm-matching/modal-confirm-matching.component';
import { ModalConfirmMatchingAdvancedCustomerComponent } from './component/modal/modal-confirm-matching-advanced-customer/modal-confirm-matching-advanced-customer.component';
import { ModalWarningFileSizeComponent } from './component/modal/modal-warning-file-size/modal-warning-file-size.component';
import { Com0404SiteErrorComponent } from './component/common/COM0400/com0404-site-error/com0404-site-error.component';
import { Ph1301EbDataFormatListComponent } from './component/maintenance/PH1300/ph1301-eb-data-format-list/ph1301-eb-data-format-list.component';
import { Pd0602ReceiptAdvanceReceivedSplitComponent } from './component/receive/PD0600/pd0602-receipt-advance-received-split/pd0602-receipt-advance-received-split.component';
import { Pb1101SectionMasterComponent } from './component/config/PB1100/pb1101-section-master/pb1101-section-master.component';
import { Pb1201SectionWithDepartmentMasterComponent } from './component/config/PB1200/pb1201-section-with-department-master/pb1201-section-with-department-master.component';
import { Pb1301SectionWithLoginUserMasterComponent } from './component/config/PB1300/pb1301-section-with-login-user-master/pb1301-section-with-login-user-master.component';
import { Pd0801ReceiptSectionTransferComponent } from './component/receive/PD0800/pd0801-receipt-section-transfer/pd0801-receipt-section-transfer.component';
import { RacSafeHtmlPipe } from './pipe/safe-html.pipe';
import { ModalOutputSettingComponent } from './component/modal/modal-output-setting/modal-output-setting.component';
import { Pc0901PaymentScheduleInputComponent } from './component/invoice/PC0900/pc0901-payment-schedule-input/pc0901-payment-schedule-input.component';
import { Pc1001PaymentScheduleImporterComponent } from './component/invoice/PC1000/pc1001-payment-schedule-importer/pc1001-payment-schedule-importer.component';
import { PartsStringSliceComponent } from './component/view-parts/parts-string-slice/parts-string-slice.component';
import { Pd1301MfAggrDataExtractComponent } from './component/receive/PD1300/pd1301-mf-aggr-data-extract/pd1301-mf-aggr-data-extract.component';
import { Ph1801MfAggrWebApiSettingComponent } from './component/maintenance/PH1800/ph1801-mf-aggr-web-api-setting/ph1801-mf-aggr-web-api-setting.component';
import { Ph2001MfAggrHistorySearchComponent } from './component/maintenance/PH2000/ph2001-mf-aggr-history-search/ph2001-mf-aggr-history-search.component';
import { ModalImporterSettingPaymentScheduleComponent } from './component/modal/modal-importer-setting-payment-schedule/modal-importer-setting-payment-schedule.component';
import { Ph1802MfAggrSubAccountsSettingComponent } from './component/maintenance/PH1800/ph1802-mf-aggr-sub-accounts-setting/ph1802-mf-aggr-sub-accounts-setting.component';
import { PartsCompanyComponent } from './component/view-parts/parts-company/parts-company.component';
import { ModalChangePasswordComponent } from './component/modal/modal-change-password/modal-change-password.component';
import { PartsResultMessageComponent } from './component/view-parts/parts-result-message/parts-result-message.component';
import { Pf0102BillingAgingListDetailComponent } from './component/form/PF0100/pf0102-billing-aging-list-detail/pf0102-billing-aging-list-detail.component';

@NgModule({
  declarations: [

    AppComponent,

    // 画面
    Pa0101LoginComponent,
    Pa0201LogoutComponent,
    Pb0101CompanyMasterComponent,
    Pb0501CustomerMasterComponent,
    Pb0301LoginUserMasterComponent,
    Pb0201DepartmentMasterComponent,
    Pb0401StaffMasterComponent,
    Pb0601CustomerGroupMasterComponent,
    Pb0801BankAccountMasterComponent,
    Pb0701AccountTitleMasterComponent,
    Pb0901CategoryMasterComponent,
    Pb1401GeneralSettingMasterComponent,
    Pb1001KanaHistoryMasterComponent,
    Pb1501IgnoreKanaMasterComponent,
    Pb1601CalendarMasterComponent,
    Pb1701JuridicalPersonalityMasterComponent,
    Pb1801BankBranchMasterComponent,
    Pc0101BillingImporterComponent,
    Pc0201BillingInputComponent,
    Pc0301BillingSearchComponent,
    Pc0501BillingDueAtModifyComponent,
    Pc0601BillingJournalizingComponent,
    Pd0201ReceiptImporterComponent,
    Pd0301ReceiptInputComponent,
    Pd0401ReceiptApportionComponent,
    Pd0501ReceiptSearchComponent,
    Pd0601ReceiptAdvanceReceivedComponent,
    Pd0701ReceiptJournalizingComponent,
    Pe0101MatchingSequentialComponent,
    Pe0102MatchingIndividualComponent,
    Pe0201MatchingJournalizingComponent,
    Pe0301MatchingHistorySearchComponent,
    Pe0401MatchingJournalizingCancellationComponent,
    Pe0501BillingOmitComponent,
    Pe0601ReceiptOmitComponent,
    Pf0101BillingAgingListComponent,
    Pf0201CreditAgingListComponent,
    Pf0301ScheduledPaymentListComponent,
    Pf0401ArrearagesListComponent,
    Pf0501CustomerLedgerComponent,
    Ph0101SettingSsecurityComponent,
    Ph0201DataMaintenanceComponent,
    Ph0701GridSettingComponent,
    Ph0801CollationSettingComponent,
    Ph0901LogDataMaintenanceComponent,
    Ph1001ColumnSettingComponent,
    Sub0101ManualComponent,
    Sub0201InquiryComponent,
    Com0101HeaderComponent,
    Com0201FooterComponent,
    Com0301MenuComponent,
    Com0501PageTitleComponent,
    Com0401ErrorComponent,
    Com0000MainComponent,
    Pd0101EbFileImporterComponent,
    Pf0601CollectionScheduleComponent,

    // モダル
    ModalCustomerDetailComponent,
    ModalMasterComponent,
    ModalMasterBankComponent,
    ModalMasterBankAccountComponent,
    ModalImporterSettingComponent,
    ModalInvoiceMemoComponent,
    ModalMemoComponent,
    ModalImportMethodSelectorComponent,
    ModalEbExcludeAccountingSettingComponent,
    ModalEbFileSettingComponent,
    ModalFormSettingComponent,
    ModalOutputSettingComponent,

    // パイプ
    RacCurrencyPipe,
    RacSafeHtmlPipe,

    Pb1901PaymentAgencyMasterComponent,

    Pc0701AccountTransferCreateComponent,

    Pc0801AccountTransferImportComponent,

    Pc1801MfBillingExtractComponent,

    Pe1001MfMatchingOutputComponent,

    Ph1401MfWebApiSettingComponent,

    Com0601TopComponent,

    Com0402SiteErrorComponent,

    Pj0101UserSettingComponent,

    Pj0201UserPasswordReissueComponent,

    ModalConfirmComponent,

    ModalMultiMasterComponent,

    ModalRouterProgressComponent,

    ModalReceiptExcludeAllComponent,

    ModalReceiptExcludeDetailComponent,

    ModalImporterSettingCustomerComponent,

    ModalImportMethodSelectorCustomerComponent,

    ModalExportMethodSelectorCustomerComponent,

    CategoryBillingComponent,

    CategoryReceiptComponent,

    CategoryCollectionComponent,

    CategoryExcludeComponent,

    ModalMatchingRecordedAtComponent,

    ModalCollatePrintComponent,

    ModalTransferFeeComponent,

    ModalIndividualMemoComponent,

    ModalSelectParentCustomerComponent,

    ModalConfirmMatchingComponent,

    ModalConfirmMatchingAdvancedCustomerComponent,

    ModalWarningFileSizeComponent,

    Com0404SiteErrorComponent,

    Ph1301EbDataFormatListComponent,

    Pd0602ReceiptAdvanceReceivedSplitComponent,

    Pb1101SectionMasterComponent,

    Pb1201SectionWithDepartmentMasterComponent,

    Pb1301SectionWithLoginUserMasterComponent,

    Pd0801ReceiptSectionTransferComponent,

    Pc0901PaymentScheduleInputComponent,

    Pc1001PaymentScheduleImporterComponent,

    PartsStringSliceComponent,

    Pd1301MfAggrDataExtractComponent,

    Ph1801MfAggrWebApiSettingComponent,

    Ph2001MfAggrHistorySearchComponent,

    ModalImporterSettingPaymentScheduleComponent,

    Ph1802MfAggrSubAccountsSettingComponent,
    
    PartsCompanyComponent,
    
    ModalChangePasswordComponent,
    
    PartsResultMessageComponent,
    
    Pf0102BillingAgingListDetailComponent,



  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    HttpClientXsrfModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    MatNativeDateModule,
    Ng2FlatpickrModule,
    NgbModule.forRoot(),

  ],
  providers: [
    DatePipe,
    DecimalPipe,
    { provide: LOCALE_ID, useValue: navigator.language }],

  bootstrap: [AppComponent],
  entryComponents: [
    ModalCustomerDetailComponent, // 得意先編集
    ModalMasterComponent,     // 汎用マスタ検索
    ModalMultiMasterComponent,  // 汎用マスタ検索（複数選択）
    ModalMasterBankComponent, // 銀行・支店検索
    ModalMasterBankAccountComponent,  // 銀行口座検索
    ModalImporterSettingComponent,   // 入金・請求インポーター設定編集
    ModalImporterSettingCustomerComponent,   // 得意先インポーター設定編集
    ModalImporterSettingPaymentScheduleComponent, // 入金予定設定編集
    ModalImportMethodSelectorComponent, // マスターインポート方法の選択
    ModalImportMethodSelectorCustomerComponent, // 得意先インポート方法の選択
    ModalExportMethodSelectorCustomerComponent, // 得意先エクスポート方法の選択
    ModalInvoiceMemoComponent,  // 請求入力メモ
    ModalMemoComponent,  // 入金入力メモ
    ModalEbExcludeAccountingSettingComponent, // EBデータ取込対象外口座設定
    ModalEbFileSettingComponent,  // EBファイル設定一覧
    ModalFormSettingComponent,  // 帳票設定
    ModalConfirmComponent,  // 確認用モーダル
    ModalRouterProgressComponent, // ロード中コンポーネント
    ModalReceiptExcludeAllComponent,
    ModalReceiptExcludeDetailComponent,
    ModalMatchingRecordedAtComponent, // 消込処理 前受有時の消込処理年月日
    ModalCollatePrintComponent,       // 消込処理 消込対象の印刷画面
    ModalTransferFeeComponent,  // 顧客マスタ　手数料登録
    //ModalIndividualCollateComponent,
    ModalIndividualMemoComponent, // 個別消込 入金・請求メモ
    ModalSelectParentCustomerComponent, // 個別消込 債権代表者の選択
    ModalConfirmMatchingComponent,  // 消込確認ダイアログ
    ModalConfirmMatchingAdvancedCustomerComponent,  // 消込 前受金振替得意先
    ModalWarningFileSizeComponent,  // 共通 ファイルサイズ警告
    ModalOutputSettingComponent,  //  出力設定　消込済入金データ・請求書発行csvレイアウト
    ModalChangePasswordComponent, // パスワード変更モーダル
  ], // 動的に生成するcomponentをentryComponentsに指定する
})
export class AppModule { }
