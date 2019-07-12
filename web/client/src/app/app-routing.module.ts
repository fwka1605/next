import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';

import { Pa0101LoginComponent } from './component/login/PA0100/pa0101-login/pa0101-login.component';
import { Com0401ErrorComponent } from './component/common/COM0400/com0401-error/com0401-error.component';
import { Pa0201LogoutComponent } from './component/logout/PA0200/pa0201-logout/pa0201-logout.component';
import { Pb0101CompanyMasterComponent } from './component/config/PB0100/pb0101-company-master/pb0101-company-master.component';
import { Com0000MainComponent } from './component/common/com0000/com0000-main/com0000-main.component';
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
import { Pd0701ReceiptJournalizingComponent } from './component/receive/PD0700/pd0701-receipt-journalizing/pd0701-receipt-journalizing.component';
import { Pd0601ReceiptAdvanceReceivedComponent } from './component/receive/PD0600/pd0601-receipt-advance-received/pd0601-receipt-advance-received.component';
import { Pd0501ReceiptSearchComponent } from './component/receive/PD0500/pd0501-receipt-search/pd0501-receipt-search.component';
import { Pd0401ReceiptApportionComponent } from './component/receive/PD0400/pd0401-receipt-apportion/pd0401-receipt-apportion.component';
import { Pd0301ReceiptInputComponent } from './component/receive/PD0300/pd0301-receipt-input/pd0301-receipt-input.component';
import { Pd0201ReceiptImporterComponent } from './component/receive/PD0200/pd0201-receipt-importer/pd0201-receipt-importer.component';
import { Pe0601ReceiptOmitComponent } from './component/clearing/PE0600/pe0601-receipt-omit/pe0601-receipt-omit.component';
import { Pe0501BillingOmitComponent } from './component/clearing/PE0500/pe0501-billing-omit/pe0501-billing-omit.component';
import { Pe0401MatchingJournalizingCancellationComponent } from './component/clearing/PE0400/pe0401-matching-journalizing-cancellation/pe0401-matching-journalizing-cancellation.component';
import { Pe0301MatchingHistorySearchComponent } from './component/clearing/PE0300/pe0301-matching-history-search/pe0301-matching-history-search.component';
import { Pe0201MatchingJournalizingComponent } from './component/clearing/PE0200/pe0201-matching-journalizing/pe0201-matching-journalizing.component';
import { Pe0101MatchingSequentialComponent } from './component/clearing/PE0100/pe0101-matching-sequential/pe0101-matching-sequential.component';
import { Pd0101EbFileImporterComponent } from './component/receive/PD0100/pd0101-eb-file-importer/pd0101-eb-file-importer.component';
import { Pf0101BillingAgingListComponent } from './component/form/PF0100/pf0101-billing-aging-list/pf0101-billing-aging-list.component';
import { Pf0201CreditAgingListComponent } from './component/form/PF0200/pf0201-credit-aging-list/pf0201-credit-aging-list.component';
import { Pf0301ScheduledPaymentListComponent } from './component/form/PF0300/pf0301-scheduled-payment-list/pf0301-scheduled-payment-list.component';
import { Pf0401ArrearagesListComponent } from './component/form/PF0400/pf0401-arrearages-list/pf0401-arrearages-list.component';
import { Pf0501CustomerLedgerComponent } from './component/form/PF0500/pf0501-customer-ledger/pf0501-customer-ledger.component';
import { Pf0601CollectionScheduleComponent } from './component/form/PF0600/pf0601-collection-schedule/pf0601-collection-schedule.component';
import { Ph0201DataMaintenanceComponent } from './component/maintenance/PH0200/ph0201-data-maintenance/ph0201-data-maintenance.component';
import { Ph0101SettingSsecurityComponent } from './component/maintenance/PH0100/ph0101-setting-ssecurity/ph0101-setting-ssecurity.component';
import { Ph0701GridSettingComponent } from './component/maintenance/PH0700/ph0701-grid-setting/ph0701-grid-setting.component';
import { Ph0801CollationSettingComponent } from './component/maintenance/PH0800/ph0801-collation-setting/ph0801-collation-setting.component';
import { Ph0901LogDataMaintenanceComponent } from './component/maintenance/PH0900/ph0901-log-data-maintenance/ph0901-log-data-maintenance.component';
import { Ph1001ColumnSettingComponent } from './component/maintenance/PH1000/ph1001-column-setting/ph1001-column-setting.component';
import { Pe0102MatchingIndividualComponent } from './component/clearing/PE0100/pe0102-matching-individual/pe0102-matching-individual.component';
import { Pb1901PaymentAgencyMasterComponent } from './component/config/PB1900/pb1901-payment-agency-master/pb1901-payment-agency-master.component';
import { Pc0701AccountTransferCreateComponent } from './component/invoice/PC0700/pc0701-account-transfer-create/pc0701-account-transfer-create.component';
import { Pc0801AccountTransferImportComponent } from './component/invoice/PC0800/pc0801-account-transfer-import/pc0801-account-transfer-import.component';
import { Pe1001MfMatchingOutputComponent } from './component/clearing/PE1000/pe1001-mf-matching-output/pe1001-mf-matching-output.component';
import { Ph1401MfWebApiSettingComponent } from './component/maintenance/PH1400/ph1401-mf-web-api-setting/ph1401-mf-web-api-setting.component';
import { Com0601TopComponent } from './component/common/COM0600/com0601-top/com0601-top.component';
import { Com0402SiteErrorComponent } from './component/common/COM0400/com0402-site-error/com0402-site-error.component';
import { Pj0101UserSettingComponent } from './component/user/pj0100/pj0101-user-setting/pj0101-user-setting.component';
import { Pj0201UserPasswordReissueComponent } from './component/user/pj0200/pj0201-user-password-reissue/pj0201-user-password-reissue.component';
import { AuthGuard } from './guard/auth.guard';
import { Pc1801MfBillingExtractComponent } from './component/invoice/PC1800/pc1801-mf-billing-extract/pc1801-mf-billing-extract.component';
import { Com0404SiteErrorComponent } from './component/common/COM0400/com0404-site-error/com0404-site-error.component';
import { Ph1301EbDataFormatListComponent } from './component/maintenance/PH1300/ph1301-eb-data-format-list/ph1301-eb-data-format-list.component';
import { Pd0602ReceiptAdvanceReceivedSplitComponent } from './component/receive/PD0600/pd0602-receipt-advance-received-split/pd0602-receipt-advance-received-split.component';
import { Pb1101SectionMasterComponent } from './component/config/PB1100/pb1101-section-master/pb1101-section-master.component';
import { Pb1201SectionWithDepartmentMasterComponent } from './component/config/PB1200/pb1201-section-with-department-master/pb1201-section-with-department-master.component';
import { Pb1301SectionWithLoginUserMasterComponent } from './component/config/PB1300/pb1301-section-with-login-user-master/pb1301-section-with-login-user-master.component';
import { Pd0801ReceiptSectionTransferComponent } from './component/receive/PD0800/pd0801-receipt-section-transfer/pd0801-receipt-section-transfer.component';
import { Pc0901PaymentScheduleInputComponent } from './component/invoice/PC0900/pc0901-payment-schedule-input/pc0901-payment-schedule-input.component';
import { Pc1001PaymentScheduleImporterComponent } from './component/invoice/PC1000/pc1001-payment-schedule-importer/pc1001-payment-schedule-importer.component';
import { Pd1301MfAggrDataExtractComponent } from './component/receive/PD1300/pd1301-mf-aggr-data-extract/pd1301-mf-aggr-data-extract.component';
import { Ph1801MfAggrWebApiSettingComponent } from './component/maintenance/PH1800/ph1801-mf-aggr-web-api-setting/ph1801-mf-aggr-web-api-setting.component';
import { Ph2001MfAggrHistorySearchComponent } from './component/maintenance/PH2000/ph2001-mf-aggr-history-search/ph2001-mf-aggr-history-search.component';
import { Ph1802MfAggrSubAccountsSettingComponent } from './component/maintenance/PH1800/ph1802-mf-aggr-sub-accounts-setting/ph1802-mf-aggr-sub-accounts-setting.component';
import { Pf0102BillingAgingListDetailComponent } from './component/form/PF0100/pf0102-billing-aging-list-detail/pf0102-billing-aging-list-detail.component';

const routes: Routes = [

  ///////////////////////////////////////////////////
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: 'login',
    component: Pa0101LoginComponent,
    data: { id:'1',title: 'ログイン', }
  },
  ///////////////////////////////////////////////////
  {
    path: 'logout',
    component: Pa0201LogoutComponent,
    data: { id:'2',title: 'ログアウト', }
  },
  ///////////////////////////////////////////////////
  {
    path: 'password-reissue',
    component: Pj0201UserPasswordReissueComponent,
    data: { id:'3',title: 'ログアウト', }
  },
  {
    path: '404-error',
    component: Com0404SiteErrorComponent,
    data: { id:'4',title: '404エラー', }
  },  
  ///////////////////////////////////////////////////
  {
    path: 'main',
    component: Com0000MainComponent,
    children: [
      {
        path: 'PB0101',
        component: Pb0101CompanyMasterComponent,
        canActivate: [AuthGuard],
        data: { id:'101',title: '会社マスター', }
      },
      {
        path: 'PB0501',
        component: Pb0501CustomerMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'102',title: '得意先マスター', }
      },
      {
        path: 'PB0301',
        component: Pb0301LoginUserMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'103',title: 'ログインユーザーマスター', }
      },
      {
        path: 'PB0201',
        component: Pb0201DepartmentMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'104',title: '請求部門マスター', }
      },
      {
        path: 'PB0401',
        component: Pb0401StaffMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'105',title: '営業担当者マスター', }
      },
      {
        path: 'PB0601',
        component: Pb0601CustomerGroupMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'106',title: '債権代表者マスター', }
      },
      {
        path: 'PB0801',
        component: Pb0801BankAccountMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'107',title: '銀行口座マスター', }
      },
      {
        path: 'PB0701',
        component: Pb0701AccountTitleMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'108',title: '科目マスター', }
      },
      {
        path: 'PB0901',
        component: Pb0901CategoryMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'109',title: '区分マスター', }
      },
      {
        path: 'PB1201',
        component: Pb1201SectionWithDepartmentMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'110',title: '入金・請求部門対応マスター', }
      },
      {
        path: 'PB1301',
        component: Pb1301SectionWithLoginUserMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'111',title: '入金部門・担当者対応マスター', }
      },
      {
        path: 'PB1401',
        component: Pb1401GeneralSettingMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'112',title: '管理マスター', }
      },
      {
        path: 'PB1001',
        component: Pb1001KanaHistoryMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'113',title: '学習履歴データ管理', }
      },
      {
        path: 'PB1501',
        component: Pb1501IgnoreKanaMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'114',title: '除外カナマスター', }
      },
      {
        path: 'PB1101',
        component: Pb1101SectionMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'115',title: '入金部門マスター', }
      },
      {
        path: 'PB1601',
        component: Pb1601CalendarMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'116',title: 'カレンダーマスター', }
      },
      {
        path: 'PB1701',
        component: Pb1701JuridicalPersonalityMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'117',title: '法人格', }
      },
      {
        path: 'PB1801',
        component: Pb1801BankBranchMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'118',title: '銀行・支店マスター', }
      },
      {
        path: 'PB1901',
        component: Pb1901PaymentAgencyMasterComponent,
        canActivate: [AuthGuard],
        data: {  id:'119',title: '決済代行会社マスター', }
      },

      ///////////////////////////////////////////////////
      {
        path: 'PC0101',
        component: Pc0101BillingImporterComponent,
        canActivate: [AuthGuard],
        data: {  id:'201',title: '請求フリーインポーター', }
      },
      {
        path: 'PC0201',
        component: Pc0201BillingInputComponent,
        canActivate: [AuthGuard],
        data: {  id:'202',title: '請求データ入力', }
      },
      {
        path: 'PC0301',
        component: Pc0301BillingSearchComponent,
        canActivate: [AuthGuard],
        data: {  id:'203',title: '請求データ検索', }
      },
      {
        path: 'PC0501',
        component: Pc0501BillingDueAtModifyComponent,
        canActivate: [AuthGuard],
        data: {  id:'204',title: '入金予定日変更', }
      },
      {
        path: 'PC0601',
        component: Pc0601BillingJournalizingComponent,
        canActivate: [AuthGuard],
        data: {  id:'205',title: '請求仕訳出力', }
      },
      {
        path: 'PC0701',
        component: Pc0701AccountTransferCreateComponent,
        canActivate: [AuthGuard],
        data: {  id:'206',title: '口座振替依頼データ作成', }
      },
      {
        path: 'PC0801',
        component: Pc0801AccountTransferImportComponent,
        canActivate: [AuthGuard],
        data: {  id:'207',title: '口座振替結果データ取込', }
      },
      {
        path: 'PC0901',
        component: Pc0901PaymentScheduleInputComponent,
        canActivate: [AuthGuard],
        data: {  id:'208',title: '入金予定入力', }
      },
      {
        path: 'PC1001',
        component: Pc1001PaymentScheduleImporterComponent,
        canActivate: [AuthGuard],
        data: {  id:'208',title: '入金予定フリーインポーター', }
      },
      {
        path: 'PC1801',
        component: Pc1801MfBillingExtractComponent,
        canActivate: [AuthGuard],
        data: {  id:'209',title: 'MFクラウド請求書 データ抽出', }
      },


      ///////////////////////////////////////////////////
      {
        path: 'PD0101',
        component: Pd0101EbFileImporterComponent,
        canActivate: [AuthGuard],
        data: { id:'301',title: '入金EBデータ取込', }
      },
      {
        path: 'PD0201',
        component: Pd0201ReceiptImporterComponent,
        canActivate: [AuthGuard],
        data: { id:'302',title: '入金フリーインポーター', }
      },
      {
        path: 'PD0301',
        component: Pd0301ReceiptInputComponent,
        canActivate: [AuthGuard],
        data: { id:'303',title: '入金データ入力', }
      },
      {
        path: 'PD0401',
        component: Pd0401ReceiptApportionComponent,
        canActivate: [AuthGuard],
        data: { id:'304',title: '入金データ振分', }
      },
      {
        path: 'PD0501',
        component: Pd0501ReceiptSearchComponent,
        canActivate: [AuthGuard],
        data: { id:'305',title: '入金データ検索', }
      },
      {
        path: 'PD0601',
        component: Pd0601ReceiptAdvanceReceivedComponent,
        canActivate: [AuthGuard],
        data: { id:'306',title: '前受一括計上処理', }
      },
      {
        path: 'PD0602',
        component: Pd0602ReceiptAdvanceReceivedSplitComponent,
        canActivate: [AuthGuard],
        data: { id:'307',title: '前受金振替・分割処理', }
      },
      {
        path: 'PD0701',
        component: Pd0701ReceiptJournalizingComponent,
        canActivate: [AuthGuard],
        data: {id:'308', title: '入金仕訳出力', }
      },
      {
        path: 'PD0801',
        component: Pd0801ReceiptSectionTransferComponent,
        canActivate: [AuthGuard],
        data: {id:'308', title: '入金部門振替処理', }
      },
      {
        path: 'PD1301',
        component: Pd1301MfAggrDataExtractComponent,
        canActivate: [AuthGuard],
        data: {id:'309', title: '入金データ自動連携 データ抽出', }
      },      
      
      ///////////////////////////////////////////////////
      {
        path: 'PE0101',
        component: Pe0101MatchingSequentialComponent,
        canActivate: [AuthGuard],
        data: { id:'401',title: '一括消込', }
      },
      {
        path: 'PE0102',
        component: Pe0102MatchingIndividualComponent,
        canActivate: [AuthGuard],
        data: { id:'402',title: '個別消込', }
      },
      {
        path: 'PE0201',
        component: Pe0201MatchingJournalizingComponent,
        canActivate: [AuthGuard],
        data: { id:'403',title: '消込仕訳出力', }
      },
      {
        path: 'PE0301',
        component: Pe0301MatchingHistorySearchComponent,
        canActivate: [AuthGuard],
        data: { id:'404',title: '消込履歴データ検索', }
      },
      {
        path: 'PE0401',
        component: Pe0401MatchingJournalizingCancellationComponent,
        canActivate: [AuthGuard],
        data: { id:'405',title: '消込仕訳出力取消', }
      },
      {
        path: 'PE0501',
        component: Pe0501BillingOmitComponent,
        canActivate: [AuthGuard],
        data: { id:'406',title: '未消込請求データ削除', }
      },
      {
        path: 'PE0601',
        component: Pe0601ReceiptOmitComponent,
        canActivate: [AuthGuard],
        data: { id:'407',title: '未消込入金データ削除', }
      },
      {
        path: 'PE1001',
        component: Pe1001MfMatchingOutputComponent,
        canActivate: [AuthGuard],
        data: { id:'408',title: 'MFクラウド会計 消込結果連携', }
      },

      ///////////////////////////////////////////////////
      {
        path: 'PF0101',
        component: Pf0101BillingAgingListComponent,
        canActivate: [AuthGuard],
        data: { id:'501',title: '請求残高年齢表', }
      },
      {
        path: 'PF0201',
        component: Pf0201CreditAgingListComponent,
        canActivate: [AuthGuard],
        data: { id:'502',title: '債権総額管理表', }
      },
      {
        path: 'PF0301',
        component: Pf0301ScheduledPaymentListComponent,
        canActivate: [AuthGuard],
        data: { id:'503',title: '入金予定明細表', }
      },
      {
        path: 'PF0401',
        component: Pf0401ArrearagesListComponent,
        canActivate: [AuthGuard],
        data: { id:'504',title: '滞留明細一覧表', }
      },
      {
        path: 'PF0501',
        component: Pf0501CustomerLedgerComponent,
        canActivate: [AuthGuard],
        data: { id:'505',title: '得意先別消込台帳', }
      },
      {
        path: 'PF0601',
        component: Pf0601CollectionScheduleComponent,
        canActivate: [AuthGuard],
        data: { id:'506',title: '回収予定表', }
      },
      {
        path: 'PF0102',
        component: Pf0102BillingAgingListDetailComponent,
        canActivate: [AuthGuard],
        data: { id:'507',title: '請求残高年齢表（明細）', }
      },      
      ///////////////////////////////////////////////////
      {
        path: 'PH0201',
        component: Ph0201DataMaintenanceComponent,
        canActivate: [AuthGuard],
        data: { id:'701',title: '不要データ削除', }
      },
      {
        path: 'PH0101',
        component: Ph0101SettingSsecurityComponent,
        canActivate: [AuthGuard],
        data: { id:'702',title: '各種設定＆セキュリティ', }
      },
      {
        path: 'PH0701',
        component: Ph0701GridSettingComponent,
        canActivate: [AuthGuard],
        data: { id:'703',title: 'グリッド表示設定', }
      },
      {
        path: 'PH0801',
        component: Ph0801CollationSettingComponent,
        canActivate: [AuthGuard],
        data: { id:'704',title: '照合ロジック設定', }
      },
      {
        path: 'PH0901',
        component: Ph0901LogDataMaintenanceComponent,
        canActivate: [AuthGuard],
        data: { id:'705',title: '操作ログ管理', }
      },
      {
        path: 'PH1001',
        component: Ph1001ColumnSettingComponent,
        canActivate: [AuthGuard],
        data: { id:'706',title: '項目名称設定', }
      },
      {
        path: 'PH1301',
        component: Ph1301EbDataFormatListComponent,
        canActivate: [AuthGuard],
        data: { id:'707',title: 'EBデータフォーマット一覧', }
      },

      {
        path: 'PH1401',
        component: Ph1401MfWebApiSettingComponent,
        canActivate: [AuthGuard],
        data: { id:'708',title: 'MFクラウド請求書 Web API 連携設定', }
      },

      {
        path: 'PH1801',
        component: Ph1801MfAggrWebApiSettingComponent,
        canActivate: [AuthGuard],
        data: { id:'708',title: '入金データ自動連携 WebAPI 連携設定', }
      },
      {
        path: 'PH1802',
        component: Ph1802MfAggrSubAccountsSettingComponent,
        canActivate: [AuthGuard],
        data: { id:'709',title: '入金データ自動連携 口座詳細', }
      },

      {
        path: 'PH2001',
        component: Ph2001MfAggrHistorySearchComponent,
        canActivate: [AuthGuard],
        data: { id:'710',title: '入金データ自動連携 データ検索', }
      },

      {
        path: '',
        component: Com0601TopComponent,
        canActivate: [AuthGuard],
        data: { title: 'ホーム', }
      },
      {
        path: '**',
        canActivate: [AuthGuard],
        component: Com0401ErrorComponent,
        data: { title: '', }
      },
    ]
  },
  ///////////////////////////////////////////////////
  {
    path: '**',
    component: Com0402SiteErrorComponent
  },
];


@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  declarations: [],
  exports: [RouterModule]
})


export class AppRoutingModule {

}
