import { Component, OnInit, EventEmitter, ElementRef, AfterViewInit } from '@angular/core';
import { TABLE_INDEX, TABLE_NAME, TABLE_COLUMN } from 'src/app/common/const/table-name.const';
import { BaseComponent } from '../../common/base/base-component';
import { FormControl,  FormGroup } from '@angular/forms';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { ImporterSettingService } from 'src/app/service/Master/importer-setting.service';
import { PaymentAgencyMasterService } from 'src/app/service/Master/payment-agency-master.service';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { LoginUserMasterService } from 'src/app/service/Master/login-user-master.service';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { StaffMasterService } from 'src/app/service/Master/staff-master.service';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { DestinationMasterService } from 'src/app/service/Master/destination-master.service';
import { TaxClassMasterService } from 'src/app/service/Master/tax-class-master.service';
import { AccountTitleMasterService } from 'src/app/service/Master/account-title-master.service';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { Department } from 'src/app/model/department.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { DepartmentResult } from 'src/app/model/department-result.model';
import { StringUtil } from 'src/app/common/util/string-util';
import { ImporterSetting } from 'src/app/model/importer-setting.model';
import { FreeImporterFormatType } from 'src/app/common/common-const';
import { ImporterSettingsResult } from 'src/app/model/importer-settings-result.model';
import { CustomerResult } from 'src/app/model/customer-result.model';
import { Customer } from 'src/app/model/customer.model';
import { StaffsResult } from 'src/app/model/staffs-result.model';
import { Staff } from 'src/app/model/staff.model';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import {  CategoryType, SHARE_TRANSFER_FEE_DICTIONARY, CUSTOMER_PARENT_FLAG_DICTIONARY, CUSTOMER_HOLIDAY_FLAG_DICTIONARY, YES_NO_FLAG_DICTIONARY } from 'src/app/common/const/kbn.const';
import { Category } from 'src/app/model/category.model';
import { TaxClassResult } from 'src/app/model/tax-class-result.model';
import { TaxClass } from 'src/app/model/tax-class.model';
import { AccountTitlesResult } from 'src/app/model/account-titles-result.model';
import { AccountTitle } from 'src/app/model/account-title.model';
import { CurrenciesResult } from 'src/app/model/currencies-result.model';
import { Currency } from 'src/app/model/currency.model';
import { SectionResult } from 'src/app/model/section-result.model';
import { Section } from 'src/app/model/section.model';
import { UsersResult } from 'src/app/model/users-result.model';
import { LoginUser } from 'src/app/model/login-user.model';
import { CustomerGroupMasterService } from 'src/app/service/Master/customer-group-master.service';
import { PaymentAgenciesResult } from 'src/app/model/payment-agencies-result.model';
import { PaymentAgency } from 'src/app/model/payment-agency.model';
import { DestinationsResult } from 'src/app/model/destinations-result.model';
import { Destination } from 'src/app/model/destination.model';
import { BankAccountTypeMasterService } from 'src/app/service/Master/bank-account-type-master.service';
import { BankAccountTypesResult } from 'src/app/model/bank-account-types-result.model';
import { BankAccountType } from 'src/app/model/bank-account-type.model';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { EVENT_TYPE } from 'src/app/common/const/event.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { CustomerSearch } from 'src/app/model/customer-search.model';
import { FixedValue } from 'src/app/model/fixed-value.model';
import { DepartmentSearch } from 'src/app/model/department-search.model';

@Component({
  selector: 'app-modal-master',
  templateUrl: './modal-master.component.html',
  styleUrls: ['./modal-master.component.css']
})
export class ModalMasterComponent extends BaseComponent implements OnInit,AfterViewInit {

  public mastersResult:any;

  public tableIndex: TABLE_INDEX;

  public isLargeType:boolean=false;
  
  public closing = new EventEmitter<{}>();
  public selectedId: number;
  public selectedCode: string;
  public selectedName: string;
  public selectedKana: string;

  public searchKeyCtrl: FormControl;

  public selectedObject:any;


  constructor(
    public elementRef: ElementRef,
    public userInfoService:UserInfoService,
    public customerMasterService: CustomerMasterService,
    public customerGroupMasterService:CustomerGroupMasterService,
    public importerSettingService:ImporterSettingService,
    public paymentAgencyService:PaymentAgencyMasterService,
    public loginUserService:LoginUserMasterService,
    public categoryService:CategoryMasterService,
    public departmentService:DepartmentMasterService,
    public staffService:StaffMasterService,
    public currencyService:CurrencyMasterService,
    public destinationService:DestinationMasterService,
    public taxClassService:TaxClassMasterService,
    public accountTitleService:AccountTitleMasterService,
    public sectionService:SectionMasterService,
    public bankAccountTypeMasterService:BankAccountTypeMasterService,
    public processResultService:ProcessResultService
  ) {
    super();
  }


  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();

    this.isLargeType=false;

    this.search();
  }

  ngAfterViewInit(){
    HtmlUtil.nextFocusByName(this.elementRef, 'searchKeyCtrl', EVENT_TYPE.NONE);

  }

  public setControlInit() {
    this.searchKeyCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      searchKeyCtrl: this.searchKeyCtrl,
    });
  }

  public setFormatter() {
  }  

  public clear(){
    this.MyFormGroup.reset();

    HtmlUtil.nextFocusByName(this.elementRef, 'searchKeyCtrl', EVENT_TYPE.NONE);

  }

  public get TableIndex(): TABLE_INDEX {
    return this.tableIndex;
  }
  public set TableIndex(value: TABLE_INDEX) {
    this.tableIndex = value;
    this.sortOpenExistsInit();
    
  }
  public get TableName(): String {
    return TABLE_NAME[this.tableIndex];
  }

  public get TableClumn():string[]{
    return TABLE_COLUMN[this.tableIndex];
  }

  public get Closing() {
    return this.closing;
  }
  public set Closing(value) {
    this.closing = value;
  }

  public get SelectedId(): number {
    return this.selectedId;
  }
  public set SelectedId(value: number) {
    this.selectedId = value;
  }

  public get SelectedCode(): string {
    return this.selectedCode;
  }
  public set SelectedCode(value: string) {
    this.selectedCode = value;
  }

  public get SelectedName(): string {
    return this.selectedName;
  }
  public set SelectedName(value: string) {
    this.selectedName = value;
  }

  public get SelectedKana(): string {
    return this.selectedKana;
  }
  public set SelectedKana(value: string) {
    this.selectedKana = value;
  }

  public get SelectedObject(): any {
    return this.selectedObject;
  }
  public set SelectedObject(value: any) {
    this.selectedObject = value;
  }


  public close() {
    this.ModalStatus = MODAL_STATUS_TYPE.CLOSE;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public cancel() {
    this.ModalStatus = MODAL_STATUS_TYPE.CLOSE;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }  

  public search() {
    switch(this.tableIndex){
      case this.TABLE_INDEX.MASTER_CUSTOMER:
      {
        this.isLargeType=true;        

        this.mastersResult = new CustomerResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.customer = new Array<Customer>();

        this.customerMasterService.GetItems()
            .subscribe(response => {
              if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
                this.mastersResult.processResult.result = true;
                this.mastersResult.customers = response;
  
                if (response != undefined && response.length>0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                  let searchKeyCtrl = this.searchKeyCtrl.value
                  this.mastersResult.customers = this.mastersResult.customers.filter(
                    function (customer: Customer) {
                      return customer.code.indexOf(searchKeyCtrl) != -1 || customer.name.indexOf(searchKeyCtrl) != -1;
                    }
                  )
                }
              }
              else{
                this.mastersResult.processResult.result = false;
                this.mastersResult.customers = null;
              }
            });
        break;
      }
      case this.TABLE_INDEX.MASTER_PARENT_CUSTOMER:
      {
        this.isLargeType=true;        

        this.mastersResult = new CustomerResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.customer = new Array<Customer>();

        this.customerMasterService.GetItems("",1)
            .subscribe(response => {

              if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
                this.mastersResult.processResult.result = true;
                this.mastersResult.customers = response;
  
                if (response != undefined && response.length>0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                  let searchKeyCtrl = this.searchKeyCtrl.value
                  this.mastersResult.customers = this.mastersResult.customers.filter(
                    function (customer: Customer) {
                      return customer.code.indexOf(searchKeyCtrl) != -1 || customer.name.indexOf(searchKeyCtrl) != -1;
                    }
                  )
                }
              }
              else{
                this.mastersResult.processResult.result = false;
                this.mastersResult.customers = null;
              }


            });
        break;
      }
      case this.TABLE_INDEX.MASTER_CUSTOMER_PARENT:
      {

        this.isLargeType=true;        

        this.mastersResult = new CustomerResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.customer = new Array<Customer>();

        let CustomerOption = new CustomerSearch();
        CustomerOption.isParent = 0;
        CustomerOption.xorParentCustomerId = 0;

        this.customerMasterService.GetItemsByCustomerSearch(CustomerOption)
          .subscribe(response => {
            if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
              this.mastersResult.processResult.result = true;
              this.mastersResult.customers = response;

              if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.customers = this.mastersResult.customers.filter(
                  function (customer: Customer) {
                    return customer.code.indexOf(searchKeyCtrl) != -1 || customer.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }
            }
            else {
              this.mastersResult.processResult.result = false;
              this.mastersResult.customers = null;
            }
          });
        break;
      }
      case this.TABLE_INDEX.MASTER_DEPARTMENT:
      {
        this.mastersResult = new DepartmentResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.departments = new Array<Department>();

        this.departmentService.GetItems()
            .subscribe(response => {
              if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
                this.mastersResult.processResult.result = true;
                this.mastersResult.departments = response;
  
                if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                  let searchKeyCtrl = this.searchKeyCtrl.value
                  this.mastersResult.departments= this.mastersResult.departments.filter(
                    function (department:Department) {
                      return department.code.indexOf(searchKeyCtrl) != -1 || department.name.indexOf(searchKeyCtrl) != -1;
                    }
                  )
                }
                }
              else{
                this.mastersResult.processResult.result = false;
                this.mastersResult.departments = null;
              }

            });

        break;
      }
      case this.TABLE_INDEX.MASTER_SECTION_WITH_DEPARTMENT:
        {
          this.mastersResult = new DepartmentResult();
          this.mastersResult.processResult = new ProcessResult();
          this.mastersResult.processResult.result = false;
          this.mastersResult.departments = new Array<Department>();

          let departmentSearch = new DepartmentSearch();
          departmentSearch.withSectionId = this.selectedId;
          this.departmentService.GetItemsByDepartmentSearch(departmentSearch)
            .subscribe(response => {
              if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
                this.mastersResult.processResult.result = true;
                this.mastersResult.departments = response;

                if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                  let searchKeyCtrl = this.searchKeyCtrl.value
                  this.mastersResult.departments = this.mastersResult.departments.filter(
                    function (department: Department) {
                      return department.code.indexOf(searchKeyCtrl) != -1 || department.name.indexOf(searchKeyCtrl) != -1;
                    }
                  )
                }
              }
              else {
                this.mastersResult.processResult.result = false;
                this.mastersResult.departments = null;
              }
            });
          break;
        }
      case this.TABLE_INDEX.MASTER_STAFF:
      {

        this.mastersResult = new StaffsResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.staffs = new Array<Staff>();

        this.staffService.GetItems()
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
              this.mastersResult.processResult.result = true;
              this.mastersResult.staffs = response;
  
              if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.staffs = this.mastersResult.staffs.filter(
                  function (staff: Staff) {
                    return staff.code.indexOf(searchKeyCtrl) != -1 || staff.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }            
            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.staffs = null;
            }
          });
        break;
      }
      case this.TABLE_INDEX.MASTER_DESTINATION:
      {
        this.mastersResult = new DestinationsResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.destinations = new Array<Staff>();

        this.destinationService.GetItems()
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
              this.mastersResult.processResult.result = true;
              this.mastersResult.destinations = response;
  
              if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.destinations = this.mastersResult.destinations.filter(
                  function (destination: Destination) {
                    return destination.code.indexOf(searchKeyCtrl) != -1 || destination.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }              
            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.destinations = null;
            }
          });

        break; 
      }
      case this.TABLE_INDEX.MASTER_BILLING_IMPORTER_SETTING:
      {
        this.mastersResult = new ImporterSettingsResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.importerSettings = new Array<ImporterSetting>();

          this.importerSettingService.GetHeader( FreeImporterFormatType.Billing)
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
              this.mastersResult.processResult.result = true;
              this.mastersResult.importerSettings = response;
  
              if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.importerSettings = this.mastersResult.importerSettings.filter(
                  function (importerSetting: ImporterSetting) {
                    return importerSetting.code.indexOf(searchKeyCtrl) != -1 || importerSetting.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }
            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.importerSettings = null;
            }
        });

        break;
      }
      case this.TABLE_INDEX.MASTER_RECEIPT_IMPORTER_SETTING:
      {
        this.mastersResult = new ImporterSettingsResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;

        let importerSetting = new ImporterSetting();

          this.importerSettingService.GetHeader( FreeImporterFormatType.Receipt)
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
              this.mastersResult.processResult.result = true;
              this.mastersResult.importerSettings = response;
  
              if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.importerSettings = this.mastersResult.importerSettings.filter(
                  function (importerSetting: ImporterSetting) {
                    return importerSetting.code.indexOf(searchKeyCtrl) != -1 || importerSetting.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }
            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.importerSettings = null;
            }

          }
        );

        break;
      }
      case this.TABLE_INDEX.MASTER_SCHEDULE_PAYMENT_IMPORTER_SETTING:
      {
        this.isLargeType=true;

        this.mastersResult = new ImporterSettingsResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.importerSettings = new Array<ImporterSetting>();

        let importerSetting = new ImporterSetting();

          this.importerSettingService.GetHeader(FreeImporterFormatType.PaymentSchedule)
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
              this.mastersResult.processResult.result = true;
              this.mastersResult.importerSettings = response;
  
              if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.importerSettings = this.mastersResult.importerSettings.filter(
                  function (importerSetting: ImporterSetting) {
                    return importerSetting.code.indexOf(searchKeyCtrl) != -1 || importerSetting.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }
            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.importerSettings = null;
            }
          }
        );
        break;
      }
      case this.TABLE_INDEX.MASTER_CUSTOMER_IMPORTER_SETTING:
      {
        this.mastersResult = new ImporterSettingsResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.importerSettings = new Array<ImporterSetting>();


          this.importerSettingService.GetHeader(FreeImporterFormatType.Customer)
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){

              this.mastersResult.processResult = new ProcessResult();
              this.mastersResult.processResult.result = true;
              this.mastersResult.importerSettings = response;
  
              if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.importerSettings = this.mastersResult.importerSettings.filter(
                  function (importerSetting: ImporterSetting) {
                    return importerSetting.code.indexOf(searchKeyCtrl) != -1 || importerSetting.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }
            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.importerSettings = null;
            }

          }
        );
        
        break;
      }
      case this.TABLE_INDEX.MASTER_BILLING_CATEGORY:
      {
        this.mastersResult = new CategoriesResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.categories = new Array<Category>();


          this.categoryService.GetItems(CategoryType.Billing)
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
              this.mastersResult.processResult.result = true;
              this.mastersResult.categories = response;
  
              if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.categories = this.mastersResult.categories.filter(
                  function (category: Category) {
                    return category.code.indexOf(searchKeyCtrl) != -1 || category.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }
  
            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.categories = null;
            }
          }
        );

        break;
      }
      case this.TABLE_INDEX.MASTER_RECEIPT_CATEGORY:
      {
        this.mastersResult = new CategoriesResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.categories = new Array<Category>();


          this.categoryService.GetItems(CategoryType.Receipt)
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
              this.mastersResult.processResult.result = true;
              this.mastersResult.categories = response;
  
              if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.categories = this.mastersResult.categories.filter(
                  function (category: Category) {
                    return category.code.indexOf(searchKeyCtrl) != -1 || category.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }
  
            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.categories = null;
            }


          }
        );
        break;
      }
      case this.TABLE_INDEX.MASTER_COLLECT_CATEGORY:
      {
        this.mastersResult = new CategoriesResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.categories = new Array<Category>();

        this.categoryService.GetItems(CategoryType.Collection)
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){

              this.mastersResult.processResult.result = true;
              this.mastersResult.categories = response;
  
              if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.categories = this.mastersResult.categories.filter(
                  function (category: Category) {
                    return category.code.indexOf(searchKeyCtrl) != -1 || category.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }              
            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.categories = null;
            }
          });
        break;
      }
      case this.TABLE_INDEX.MASTER_EXCLUDE_CATEGORY:
      {
        this.mastersResult = new CategoriesResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.categories = new Array<Category>();

        this.categoryService.GetItems(CategoryType.Exclude)
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
              this.mastersResult.processResult.result = true;
              this.mastersResult.categories = response;
  
              if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.categories = this.mastersResult.categories.filter(
                  function (category: Category) {
                    return category.code.indexOf(searchKeyCtrl) != -1 || category.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }                
            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.categories = null;
            }
          });
        break;
      }
      case this.TABLE_INDEX.MASTER_RECEIPT_CATEGORY_EXCLUDE_USE_LIMIT_DATE:
      {
        this.mastersResult = new CategoriesResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.categories = new Array<Category>();


          this.categoryService.GetItems(CategoryType.Receipt)
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
              let categories = response as Category[];
              response = categories.filter(x => x.useLimitDate == 0);
              this.mastersResult.processResult.result = true;
              this.mastersResult.categories = response;
  
              if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.categories = this.mastersResult.categories.filter(
                  function (category: Category) {
                    return category.code.indexOf(searchKeyCtrl) != -1 || category.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }
  
            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.categories = null;
            }


          }
        );
        break;
      }
      case this.TABLE_INDEX.MASTER_LOGIN_USER:
      case this.TABLE_INDEX.MASTER_LOGIN_USER_SECTION:
      {
        this.tableIndex==this.TABLE_INDEX.MASTER_LOGIN_USER_SECTION?this.isLargeType=true:this.isLargeType=false;

        this.mastersResult = new UsersResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.users = new Array<LoginUser>();

        this.loginUserService.GetItems()
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
              this.mastersResult.processResult.result = true;
              this.mastersResult.users = response;
  
            if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
              this.mastersResult.users = this.mastersResult.users.filter(
                function (loginUser: LoginUser) {
                  return loginUser.code.indexOf(searchKeyCtrl) != -1 || loginUser.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }

            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.users = null;
            }
          });
        break;
      }
      case this.TABLE_INDEX.MASTER_SECTION:
      {

        this.mastersResult = new SectionResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.sections = new Array<Section>();

        this.sectionService.GetItems()
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){

              this.mastersResult.processResult.result = true;
              this.mastersResult.sections = response;
  
              if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.sections = this.mastersResult.sections.filter(
                  function (section: Section) {
                    return section.code.indexOf(searchKeyCtrl) != -1 || section.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }
            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.sections = null;
            }
          }
        );
        break;
      }
      case this.TABLE_INDEX.MASTER_TAX_CLASS:
      {
        this.mastersResult = new TaxClassResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.taxClasses = new Array<TaxClass>();

        this.taxClassService.GetItems(0)
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){

              this.mastersResult.processResult.result = true;
              this.mastersResult.taxClasses = response;
  
              if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.taxClasses = this.mastersResult.taxClasses.filter(
                  function (taxClass: TaxClass) {
                    return ("" + taxClass.id).indexOf(searchKeyCtrl) != -1 || taxClass.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }
            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.taxClasses = null;
            }

          }
        );

        break;
      }
      case this.TABLE_INDEX.MASTER_ACCOUNT_TITLE:
      {
        this.mastersResult = new AccountTitlesResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.categories = new Array<AccountTitle>();

        this.accountTitleService.Get()
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
              this.mastersResult.processResult.result = true;
              this.mastersResult.accountTitles = response;
  
              if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.accountTitles = this.mastersResult.accountTitles.filter(
                  function (accountTitle: AccountTitle) {
                    return accountTitle.code.indexOf(searchKeyCtrl) != -1 || accountTitle.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }              
            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.accountTitles = null;
            }
          }
        );        

        break;
      }
      case this.TABLE_INDEX.MASTER_CURRENCY:
      {
        this.mastersResult = new CurrenciesResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;
        this.mastersResult.currencies = new Array<Currency>();

        this.currencyService.GetItems()
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){

              this.mastersResult.processResult.result = true;
              this.mastersResult.currencies = response;
  
              if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.currencies = this.mastersResult.currencies.filter(
                  function (currency: Currency) {
                    return currency.code.indexOf(searchKeyCtrl) != -1 || currency.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }              
            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.currencies = null;
            }

          }
        );
        break;
      }
      case this.TABLE_INDEX.MASTER_PAYMENT_AGENCY:
      {
        this.isLargeType=true;
        
        this.mastersResult = new PaymentAgenciesResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;

        this.paymentAgencyService.GetItems()
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
              this.mastersResult.processResult.result = true;
              this.mastersResult.paymentAgencies = response;
  
              if ( response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.paymentAgencies = this.mastersResult.paymentAgencies.filter(
                  function (paymentAgency: PaymentAgency) {
                    return paymentAgency.code.indexOf(searchKeyCtrl) != -1 || paymentAgency.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }              
            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.paymentAgencies = null;
            }

          });                
        break;
      }
      case this.TABLE_INDEX.MASTER_ACCOUNT_TYPE:
      {

        this.mastersResult = new BankAccountTypesResult();
        this.mastersResult.processResult = new ProcessResult();
        this.mastersResult.processResult.result = false;

        this.bankAccountTypeMasterService.GetItems()
          .subscribe(response => {

            if(response!=PROCESS_RESULT_RESULT_TYPE.FAILURE){
              this.mastersResult.processResult.result = true;
              this.mastersResult.bankAccountTypes = response;
  
              this.mastersResult.bankAccountTypes = this.mastersResult.bankAccountTypes.filter(
                function (bankAccountType: BankAccountType) {
                  return  bankAccountType.useReceipt==1;
                }
              )
              if ( response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersResult.bankAccountTypes = this.mastersResult.bankAccountTypes.filter(
                  function (bankAccountType: BankAccountType) {
                    return  bankAccountType.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }              
            }
            else{
              this.mastersResult.processResult.result = false;
              this.mastersResult.bankAccountTypes = null;
            }
          });                
        break;
      }
      case this.TABLE_INDEX.MASTER_SHARE_TRANSFER_FEE:
      {
        this.mastersResult = new Array<FixedValue>();
        this.mastersResult = SHARE_TRANSFER_FEE_DICTIONARY; 
        break;
      }
      case this.TABLE_INDEX.MASTER_PARENT_CUSTOMER_FLAG:
      {
        this.mastersResult = new Array<FixedValue>();
        this.mastersResult = CUSTOMER_PARENT_FLAG_DICTIONARY; 
        break;
      }
      case this.TABLE_INDEX.MASTER_CUSTOMER_HOLIDAY_FLAG:
      {
        this.mastersResult = new Array<FixedValue>();
        this.mastersResult = CUSTOMER_HOLIDAY_FLAG_DICTIONARY; 
        break;
      }
      case this.TABLE_INDEX.USE_FEE_LEARNING_YES_NO_FLAG:
      case this.TABLE_INDEX.USE_KANA_LEARNING_YES_NO_FLAG:
      case this.TABLE_INDEX.USE_FEE_TOLERANCE_YES_NO_FLAG:
      case this.TABLE_INDEX.PRIORITIZE_MATCHING_INDIVIDUAL_YES_NO_FLAG:
      {
        this.mastersResult = new Array<FixedValue>();
        this.mastersResult = YES_NO_FLAG_DICTIONARY; 
        break;
      }

    }
  }

  public select(index:number) {

    this.ModalStatus=MODAL_STATUS_TYPE.SELECT;

    switch(this.tableIndex){
      case this.TABLE_INDEX.MASTER_CUSTOMER:
      {
        this.selectedCode= this.mastersResult.customers[index].code;
        this.selectedName= this.mastersResult.customers[index].name;
        this.selectedKana= this.mastersResult.customers[index].kana;
        this.selectedObject = this.mastersResult.customers[index];
        break;
      }
      case this.TABLE_INDEX.MASTER_PARENT_CUSTOMER:
      {
          this.selectedCode = this.mastersResult.customers[index].code;
          this.selectedName = this.mastersResult.customers[index].name;
          this.selectedObject = this.mastersResult.customers[index];
        break;
      }
      case this.TABLE_INDEX.MASTER_CUSTOMER_PARENT:
        {
          this.selectedCode = this.mastersResult.customers[index].code;
          this.selectedName = this.mastersResult.customers[index].name;
          this.selectedObject = this.mastersResult.customers[index];
          break;
        }      
      case this.TABLE_INDEX.MASTER_DEPARTMENT:
      case this.TABLE_INDEX.MASTER_SECTION_WITH_DEPARTMENT:
      {
        this.selectedCode= this.mastersResult.departments[index].code;
        this.selectedName= this.mastersResult.departments[index].name;
        this.selectedObject= this.mastersResult.departments[index];
        break;
      }
      case this.TABLE_INDEX.MASTER_STAFF:
      {
        this.selectedCode= this.mastersResult.staffs[index].code;
        this.selectedName= this.mastersResult.staffs[index].name;
        this.selectedObject= this.mastersResult.staffs[index];
        break;
      }
      case this.TABLE_INDEX.MASTER_DESTINATION:
      {
        this.selectedCode= this.mastersResult.destinations[index].code;
        this.selectedName= this.mastersResult.destinations[index].name;
        this.selectedObject= this.mastersResult.destinations[index];
        break;
      }
      case this.TABLE_INDEX.MASTER_BILLING_IMPORTER_SETTING:
      case this.TABLE_INDEX.MASTER_RECEIPT_IMPORTER_SETTING:
      case this.TABLE_INDEX.MASTER_SCHEDULE_PAYMENT_IMPORTER_SETTING:
      case this.TABLE_INDEX.MASTER_CUSTOMER_IMPORTER_SETTING:
      {
        this.selectedCode= this.mastersResult.importerSettings[index].code;
        this.selectedName= this.mastersResult.importerSettings[index].name;
        this.selectedObject= this.mastersResult.importerSettings[index];
        this.selectedId = this.mastersResult.importerSettings[index].id;
        break;
      }
      case this.TABLE_INDEX.MASTER_BILLING_CATEGORY:
      case this.TABLE_INDEX.MASTER_RECEIPT_CATEGORY:
      case this.TABLE_INDEX.MASTER_COLLECT_CATEGORY:
      case this.TABLE_INDEX.MASTER_EXCLUDE_CATEGORY:
      case this.TABLE_INDEX.MASTER_RECEIPT_CATEGORY_EXCLUDE_USE_LIMIT_DATE:        
      {
        this.selectedCode= this.mastersResult.categories[index].code;
        this.selectedName= this.mastersResult.categories[index].name;
        this.selectedObject= this.mastersResult.categories[index];
        break;
      }
      case this.TABLE_INDEX.MASTER_LOGIN_USER:
      case this.TABLE_INDEX.MASTER_LOGIN_USER_SECTION:
      {
        this.selectedCode= this.mastersResult.users[index].code;
        this.selectedName= this.mastersResult.users[index].name;
        this.selectedObject= this.mastersResult.users[index];
        break;
      }
      case this.TABLE_INDEX.MASTER_SECTION:
      {
        this.selectedCode= this.mastersResult.sections[index].code;
        this.selectedName= this.mastersResult.sections[index].name;
        this.selectedObject= this.mastersResult.sections[index];
        break;
      }
      case this.TABLE_INDEX.MASTER_TAX_CLASS:
      {
        this.selectedId= this.mastersResult.taxClasses[index].id;
        this.selectedName= this.mastersResult.taxClasses[index].name;
        this.selectedObject= this.mastersResult.taxClasses[index];
        break;
      }
      case this.TABLE_INDEX.MASTER_ACCOUNT_TITLE:
      {
        this.selectedCode= this.mastersResult.accountTitles[index].code;
        this.selectedName= this.mastersResult.accountTitles[index].name;
        this.selectedObject= this.mastersResult.accountTitles[index];
        break;
      }
      case this.TABLE_INDEX.MASTER_CURRENCY:
      {
        this.selectedCode= this.mastersResult.currencies[index].code;
        this.selectedName= this.mastersResult.currencies[index].name;
        this.selectedObject= this.mastersResult.currencies[index];
        break;
      }
      case this.TABLE_INDEX.MASTER_PAYMENT_AGENCY:
      {
        this.selectedCode= this.mastersResult.paymentAgencies[index].code;
        this.selectedName= this.mastersResult.paymentAgencies[index].name;
        this.selectedKana= this.mastersResult.paymentAgencies[index].kana;
        this.selectedObject= this.mastersResult.paymentAgencies[index];
        break;
      }
      case this.TABLE_INDEX.MASTER_ACCOUNT_TYPE:
      {
        this.selectedCode= this.mastersResult.bankAccountTypes[index].id;
        this.selectedName= this.mastersResult.bankAccountTypes[index].name;
        this.selectedObject= this.mastersResult.bankAccountTypes[index];
        break;
      }
      case this.TABLE_INDEX.MASTER_SHARE_TRANSFER_FEE:
      case this.TABLE_INDEX.MASTER_PARENT_CUSTOMER_FLAG:
      case this.TABLE_INDEX.MASTER_CUSTOMER_HOLIDAY_FLAG:
      case this.TABLE_INDEX.USE_FEE_LEARNING_YES_NO_FLAG:
      case this.TABLE_INDEX.USE_KANA_LEARNING_YES_NO_FLAG:
      case this.TABLE_INDEX.USE_FEE_TOLERANCE_YES_NO_FLAG:
      case this.TABLE_INDEX.PRIORITIZE_MATCHING_INDIVIDUAL_YES_NO_FLAG:
      {
        this.selectedCode= ""+this.mastersResult[index].id;
        this.selectedName= ""+this.mastersResult[index].val;
        this.selectedObject= this.mastersResult[index];
        break;
      }

    }
    this.closing.emit();
  }

  public openSortExists: object = {}
  public sortOpenExistsInit() {
    if (TABLE_COLUMN[this.TableIndex] != undefined) {
      for (let i = 0; i < TABLE_COLUMN[this.TableIndex].length; i++) {
        this.openSortExists[i] = false;
      }
    }
  }

  public openSort(index: number) {
    this.openSortExists[index] = !this.openSortExists[index]
  }


}
