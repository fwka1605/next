
import { FormGroup, FormControl } from '@angular/forms';
import { TABLE_INDEX, TABLE_MAX_DISPLAY_INDEX } from 'src/app/common/const/table-name.const';
import { StringUtil } from 'src/app/common/util/string-util';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, PROCESS_RESULT_STATUS_TYPE, COMPONENT_DETAIL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { TableColumn } from 'src/app/common/const/design/column-size.const';
import { PlaceHolderConst } from 'src/app/common/const/design/place-holder.const';
import { ProcessResultCustom } from 'src/app/model/custom-model/process-result-custom.model';
import { BUTTON_ACTION, OPERATION_NAME } from 'src/app/common/const/event.const';
import { PanelConst } from 'src/app/common/const/design/panel.const';
import { InputText } from 'src/app/common/const/design/input-size.const';
import { ItemNameConst } from 'src/app/common/const/design/item-name.const';
import { HtmlTipsConst } from 'src/app/common/const/design/html-tips.const';
import { ButtonTipsConst } from 'src/app/common/const/design/button-tips.const';
import { Button } from 'protractor';
import { AutoCompleteType, CategoryType } from 'src/app/common/const/kbn.const';
import { startWith, map } from 'rxjs/operators';
import { Customer } from 'src/app/model/customer.model';
import { Observable } from 'rxjs';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { Department } from 'src/app/model/department.model';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { Staff } from 'src/app/model/staff.model';
import { StaffMasterService } from 'src/app/service/Master/staff-master.service';
import { Category } from 'src/app/model/category.model';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { LoginUser } from 'src/app/model/login-user.model';
import { LoginUserMasterService } from 'src/app/service/Master/login-user-master.service';
import { ImporterSettingService } from 'src/app/service/Master/importer-setting.service';
import { ImporterSetting } from 'src/app/model/importer-setting.model';
import { PaymentAgency } from 'src/app/model/payment-agency.model';
import { PaymentAgencyMasterService } from 'src/app/service/Master/payment-agency-master.service';
import { Section } from 'src/app/model/section.model';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { CustomerSearch } from 'src/app/model/customer-search.model';
import { AccountTitleMasterService } from 'src/app/service/Master/account-title-master.service';
import { AccountTitle } from 'src/app/model/account-title.model';
import { DepartmentSearch } from 'src/app/model/department-search.model';
import { TaxClass } from 'src/app/model/tax-class.model';
import { TaxClassMasterService } from 'src/app/service/Master/tax-class-master.service';
import { FreeImporterFormatType } from 'src/app/common/common-const';
import { ViewChild } from '@angular/core';
import { PartsResultMessageComponent } from '../../view-parts/parts-result-message/parts-result-message.component';
import { MSG_VAL } from 'src/app/common/const/message.const';
import { TIME_ZONE } from 'src/app/common/const/company.const';

export class BaseComponent {

  constructor(
  ) { 
  }

  public readonly TIME_ZONE: typeof TIME_ZONE = TIME_ZONE;


  // これをしないとEnum型がテンプレートで見えなかったため
  public readonly TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;
  public readonly COMPONENT_STATUS_TYPE: typeof COMPONENT_STATUS_TYPE = COMPONENT_STATUS_TYPE;
  public readonly COMPONENT_DETAIL_STATUS_TYPE: typeof COMPONENT_DETAIL_STATUS_TYPE = COMPONENT_DETAIL_STATUS_TYPE;

  public readonly MODAL_STATUS_TYPE: typeof MODAL_STATUS_TYPE = MODAL_STATUS_TYPE;
  public readonly BUTTON_ACTION: typeof BUTTON_ACTION = BUTTON_ACTION;
  public readonly PROCESS_RESULT_RESULT_TYPE: typeof PROCESS_RESULT_RESULT_TYPE = PROCESS_RESULT_RESULT_TYPE;
  public readonly StringUtil: StringUtil = new StringUtil();

  public readonly TABLE_MAX_DISPLAY_INDEX: typeof TABLE_MAX_DISPLAY_INDEX = TABLE_MAX_DISPLAY_INDEX;

  public readonly MSG_VAL: typeof MSG_VAL = MSG_VAL;

  public readonly TableColumn: TableColumn = new TableColumn();
  public readonly InputText: InputText = new InputText();
  public readonly PlaceHolderConst: PlaceHolderConst = new PlaceHolderConst();
  public readonly PanelConst: PanelConst = new PanelConst();
  public readonly ItemNameConst: ItemNameConst = new ItemNameConst();

  public readonly HtmlTipsConst: HtmlTipsConst = new HtmlTipsConst();
  public readonly ButtonTipsConst: ButtonTipsConst = new ButtonTipsConst();

  public readonly AutoCompleteType: typeof AutoCompleteType = AutoCompleteType;

  /** AutoComplete で表示される得意先件数 */
  public readonly AUTOCOMPLETE_MAX_COUNT = 20;

  /** 操作処理結果 */
  public processCustomResult: ProcessResultCustom = new ProcessResultCustom(false);
  public processModalCustomResult: ProcessResultCustom = new ProcessResultCustom(true);

  /** 子コンポーネントを読み込む */
  @ViewChild(PartsResultMessageComponent)
  public partsResultMessageComponent: PartsResultMessageComponent


  /** セキュリティレベルによる表示の制御 */
  public securityHideShow: boolean = true;

  public readonly zIndexDefSize = 5;
  public zIndex: number = this.zIndexDefSize;


  public loading: boolean = false;
  public get Loading(): boolean {
    return this.loading;
  }

  public loadStart(): void {
    this.loading = true;
  }
  public loadEnd(): void {
    this.loading = false;
  }

  public componentStatus: COMPONENT_STATUS_TYPE = COMPONENT_STATUS_TYPE.CREATE;
  public get ComponentStatus(): COMPONENT_STATUS_TYPE {
    return this.componentStatus;
  }
  public set ComponentStatus(value: COMPONENT_STATUS_TYPE) {
    this.componentStatus = value;
  }

  private modalStatus: MODAL_STATUS_TYPE;
  public get ModalStatus(): MODAL_STATUS_TYPE {
    return this.modalStatus;
  }
  public set ModalStatus(value: MODAL_STATUS_TYPE) {
    this.modalStatus = value;
  }

  public title: string;
  public get Title(): string {
    return this.title;
  }
  public set Title(title: string) {
    this.title = title;
  }

  public path: string;
  public get Path(): string {
    return this.path;
  }
  public set Path(path: string) {
    this.path = path;
  }

  public componentId: number;
  public get ComponentId(): number {
    return this.componentId;
  }
  public set ComponentId(componentId: number) {
    this.componentId = componentId;
  }

  public subTitle: string;
  public get SubTitle(): string {
    let strRtn = ""

    if (this.componentStatus == this.COMPONENT_STATUS_TYPE.CREATE) {
      strRtn = "新規";
    }
    else if (this.componentStatus == this.COMPONENT_STATUS_TYPE.REFERE) {
      strRtn = "参照";
    }
    else if (this.componentStatus == this.COMPONENT_STATUS_TYPE.UPDATE) {
      strRtn = "更新";
    }
    else {
      strRtn = "";
    }
    return strRtn;
  }


  public get DeleteButtonDisableFlag(): boolean {
    if (this.componentStatus == this.COMPONENT_STATUS_TYPE.UPDATE) {
      return false;
    }
    else {
      return true;
    }
  }

  public get UpdateButtonDisableFlag(): boolean {
    if (this.componentStatus == this.COMPONENT_STATUS_TYPE.UPDATE) {
      return false;
    }
    else {
      return true;
    }
  }

  public get RegistryButtonDisableFlag(): boolean {
    if (this.componentStatus == this.COMPONENT_STATUS_TYPE.CREATE) {
      return false;
    }
    else {
      return true;
    }
  }

    // オプションページが開いているかどうか
    public openOption: boolean = false;
    public openOptions() {
      this.openOption = !this.openOption
    }
  

  // FromGroup
  public myFormGroup: FormGroup;
  public get MyFormGroup(): FormGroup {
    return this.myFormGroup;
  }
  public set MyFormGroup(myFormGroup: FormGroup) {
    this.myFormGroup = myFormGroup;
  }



  /**
   * ボタン操作によるメソッド呼び出し
   * @param action ボタン操作
   */
  public buttonAction(action: BUTTON_ACTION) {

    let interval = setInterval(() => {
      if (this.loading) {
        this.buttonAction(action);
      }
      else {
        if (interval != null && interval != undefined) {
          clearInterval(interval);
        }

        this.buttonActionInner(action);
        // TODO:全てのページにPartを読み込めていないため一時判定を入れる
        if (this.partsResultMessageComponent != undefined) {
          this.partsResultMessageComponent.openMessage();
        }

      }
    }, 500)
  }

  protected buttonActionInner(action: BUTTON_ACTION) {

  }


  /////////////////////////////////////////////////////////////////////
  public customers: Customer[];
  public customerFilterds: Observable<Customer[]>[] = new Array<Observable<Customer[]>>();

  public initAutocompleteCustomers(
    searchCondCtrl: FormControl,
    customerMasterService: CustomerMasterService,
    filedNumber: number
  ) {
    this.customerFilterds[filedNumber] = undefined;
    searchCondCtrl.disable();


    customerMasterService.getCustomerMin()
      .subscribe(response => {
        this.customers = response;
        this.setValueChangeCustomers(searchCondCtrl, filedNumber);
        searchCondCtrl.enable();
      });

  }

  public setValueChangeCustomers(
    searchCondCtrl: FormControl,
    filedNumber: number
  ) {
    this.customerFilterds[filedNumber] = searchCondCtrl.valueChanges
      .pipe(
        startWith(''),
        map(value => (value ? this.filterCusotmers(value) : this.customers.slice()).slice(0, this.AUTOCOMPLETE_MAX_COUNT)),
      );
  }

  public filterCusotmers(value: string): Customer[] {
    if (StringUtil.IsNullOrEmpty(value)) {
      this.customers;
    }
    const searchValue = value.toLowerCase();
    return this.customers.filter(x =>
      x.code.toLowerCase().indexOf(searchValue) !== -1 ||
      x.name.toLowerCase().indexOf(searchValue) !== -1);
  }

  /////////////////////////////////////////////////////////////////////
  public parentCustomers: Customer[];
  public parentCustomerFilterds: Observable<Customer[]>[] = new Array<Observable<Customer[]>>();

  public initAutocompleteParentCustomers(
    searchCondCtrl: FormControl,
    customerMasterService: CustomerMasterService,
    filedNumber: number
  ) {
    this.customerFilterds[filedNumber] = undefined;
    searchCondCtrl.disable();

    customerMasterService.GetItems("", 1)
      .subscribe(response => {
        this.parentCustomers = response;
        this.setValueChangeParentCustomers(searchCondCtrl, filedNumber);
        searchCondCtrl.enable();
      });
  }

  public setValueChangeParentCustomers(
    searchCondCtrl: FormControl,
    filedNumber: number
  ) {
    this.parentCustomerFilterds[filedNumber] = searchCondCtrl.valueChanges
      .pipe(
        startWith(''),
        map(value => (value ? this.filterParentCusotmers(value) : this.parentCustomers.slice()).slice(0, this.AUTOCOMPLETE_MAX_COUNT)),
      );
  }

  public filterParentCusotmers(value: string): Customer[] {
    if (StringUtil.IsNullOrEmpty(value)) {
      this.parentCustomers;
    }
    const searchValue = value.toLowerCase();
    return this.parentCustomers.filter(x =>
      x.code.toLowerCase().indexOf(searchValue) !== -1 ||
      x.name.toLowerCase().indexOf(searchValue) !== -1);
  }

  /////////////////////////////////////////////////////////////////////
  public departments: Department[];
  public departmentFilterds: Observable<Department[]>[] = new Array<Observable<Department[]>>();

  public initAutocompleteDepartments(
    searchCondCtrl: FormControl,
    departmentMasterService: DepartmentMasterService,
    filedNumber: number,
    departmentSearch: DepartmentSearch = null
  ) {
    this.departmentFilterds[filedNumber] = undefined;
    searchCondCtrl.disable();
    if (departmentSearch == null) {
      departmentSearch = new DepartmentSearch();
    }
    departmentMasterService.GetItemsByDepartmentSearch(departmentSearch)
      .subscribe(response => {
        this.departments = response;
        this.setValueChangeDepartments(searchCondCtrl, filedNumber);
        searchCondCtrl.enable();
      });
  }

  public setValueChangeDepartments(
    searchCondCtrl: FormControl,
    filedNumber: number
  ) {
    this.departmentFilterds[filedNumber] = searchCondCtrl.valueChanges
      .pipe(
        startWith(''),
        map(value =>
          (value ?
            this.filterDepartments(value)
            : this.departments.slice()
          ).slice(0, this.AUTOCOMPLETE_MAX_COUNT)
        ),
      );
  }

  public filterDepartments(value: string): Department[] {
    if (StringUtil.IsNullOrEmpty(value)) {
      this.departments;
    }
    const searchValue = value.toLowerCase();
    return this.departments.filter(x =>
      x.code.toLowerCase().indexOf(searchValue) !== -1 ||
      x.name.toLowerCase().indexOf(searchValue) !== -1);
  }


  /////////////////////////////////////////////////////////////////////
  public sections: Section[];
  public sectionFilterds: Observable<Section[]>[] = new Array<Observable<Section[]>>();

  public initAutocompleteSections(
    searchCondCtrl: FormControl,
    sectionMasterService: SectionMasterService,
    filedNumber: number
  ) {
    this.departmentFilterds[filedNumber] = undefined;
    searchCondCtrl.disable();
    sectionMasterService.GetItems()
      .subscribe(response => {
        this.sections = response;
        this.setValueChangeSections(searchCondCtrl, filedNumber);
        searchCondCtrl.enable();
      });
  }

  public setValueChangeSections(
    searchCondCtrl: FormControl,
    filedNumber: number
  ) {
    this.sectionFilterds[filedNumber] = searchCondCtrl.valueChanges
      .pipe(
        startWith(''),
        map(value =>
          (value ?
            this.filterSections(value)
            : this.sections.slice()
          ).slice(0, this.AUTOCOMPLETE_MAX_COUNT)
        ),
      );
  }

  public filterSections(value: string): Section[] {
    if (StringUtil.IsNullOrEmpty(value)) {
      this.sections;
    }
    const searchValue = value.toLowerCase();
    return this.sections.filter(x =>
      x.code.toLowerCase().indexOf(searchValue) !== -1 ||
      x.name.toLowerCase().indexOf(searchValue) !== -1);
  }
  /////////////////////////////////////////////////////////////////////
  public staffs: Staff[];
  public staffFilterds: Observable<Staff[]>[] = new Array<Observable<Staff[]>>();

  public initAutocompleteStaffs(
    searchCondCtrl: FormControl,
    staffMasterService: StaffMasterService,
    filedNumber: number
  ) {
    this.staffFilterds[filedNumber] = undefined;
    searchCondCtrl.disable();
    staffMasterService.GetItems()
      .subscribe(response => {
        this.staffs = response;
        this.setValueChangeStaffs(searchCondCtrl, filedNumber);
        searchCondCtrl.enable();
      });
  }

  public setValueChangeStaffs(
    searchCondCtrl: FormControl,
    filedNumber: number
  ) {
    this.staffFilterds[filedNumber] = searchCondCtrl.valueChanges
      .pipe(
        startWith(''),
        map(value =>
          (value ?
            this.filterStaffs(value)
            : this.staffs.slice()
          ).slice(0, this.AUTOCOMPLETE_MAX_COUNT)
        ),
      );
  }

  public filterStaffs(value: string): Staff[] {
    if (StringUtil.IsNullOrEmpty(value)) {
      this.staffs;
    }
    const searchValue = value.toLowerCase();
    return this.staffs.filter(x =>
      x.code.toLowerCase().indexOf(searchValue) !== -1 ||
      x.name.toLowerCase().indexOf(searchValue) !== -1);
  }

  /////////////////////////////////////////////////////////////////////
  public categories: Category[][] = new Array<Category[]>(4);
  public categoryFilterds: Observable<Category[]>[] = new Array<Observable<Category[]>>();

  public initAutocompleteCategories(
    categoryType: CategoryType,
    searchCondCtrl: FormControl,
    categoryMasterService: CategoryMasterService,
    filedNumber: number
  ) {
    this.categoryFilterds[filedNumber] = undefined;
    searchCondCtrl.disable();
    categoryMasterService.GetItemsByCategoryType(categoryType)
      .subscribe(response => {
        this.categories[categoryType] = response;

        // 入力に使用するもののみ対象とする。
        this.categories[categoryType] = this.categories[categoryType].filter(x => x.useInput == 1);

        this.setValueChangeCategories(categoryType, searchCondCtrl, filedNumber);

        searchCondCtrl.enable();
      });
  }

  public initAutocompleteCategoriesExcludeUseLimitDate(
    categoryType: CategoryType,
    searchCondCtrl: FormControl,
    categoryMasterService: CategoryMasterService,
    filedNumber: number
  ) {
    this.categoryFilterds[filedNumber] = undefined;
    searchCondCtrl.disable();
    categoryMasterService.GetItemsByCategoryType(categoryType)
      .subscribe(response => {
        this.categories[categoryType] = response;

        // 入力に使用するもの かつ　期日入金不可　を対象とする。
        this.categories[categoryType] = this.categories[categoryType].filter(x => x.useInput == 1 && x.useLimitDate == 0);

        this.setValueChangeCategories(categoryType, searchCondCtrl, filedNumber);

        searchCondCtrl.enable();
      });
  }

  public setValueChangeCategories(
    categoryType: CategoryType,
    searchCondCtrl: FormControl,
    filedNumber: number
  ) {
    this.categoryFilterds[filedNumber] = searchCondCtrl.valueChanges
      .pipe(
        startWith(''),
        map(value =>
          (value ?
            this.filterCategories(categoryType, value, filedNumber)
            : this.categories[categoryType].slice()
          ).slice(0, this.AUTOCOMPLETE_MAX_COUNT)
        ),
      );
  }

  public filterCategories(
    categoryType: CategoryType,
    value: string,
    filedNumber: number): Category[] {
    if (StringUtil.IsNullOrEmpty(value)) {
      this.categories[categoryType];
    }
    const searchValue = value.toLowerCase();
    return this.categories[categoryType].filter(x =>
      x.code.toLowerCase().indexOf(searchValue) !== -1 ||
      x.name.toLowerCase().indexOf(searchValue) !== -1);
  }


  /////////////////////////////////////////////////////////////////////
  public loginUsers: LoginUser[];
  public loginUserFilterds: Observable<LoginUser[]>[] = new Array<Observable<LoginUser[]>>();

  public initAutocompleteLoginUsers(
    searchCondCtrl: FormControl,
    loginUserService: LoginUserMasterService,
    filedNumber: number
  ) {
    this.loginUserFilterds[filedNumber] = undefined;
    searchCondCtrl.disable();
    loginUserService.GetItems()
      .subscribe(response => {
        this.loginUsers = response;
        this.setValueChangeLoginUsers(searchCondCtrl, filedNumber);
        searchCondCtrl.enable();
      });
  }

  public setValueChangeLoginUsers(
    searchCondCtrl: FormControl,
    filedNumber: number
  ) {
    this.loginUserFilterds[filedNumber] = searchCondCtrl.valueChanges
      .pipe(
        startWith(''),
        map(value =>
          (value ?
            this.filterLoginUsers(value)
            : this.loginUsers.slice()
          ).slice(0, this.AUTOCOMPLETE_MAX_COUNT)
        ),
      );
  }

  public filterLoginUsers(value: string): LoginUser[] {
    if (StringUtil.IsNullOrEmpty(value)) {
      this.loginUsers;
    }
    const searchValue = value.toLowerCase();
    return this.loginUsers.filter(x =>
      x.code.toLowerCase().indexOf(searchValue) !== -1 ||
      x.name.toLowerCase().indexOf(searchValue) !== -1);
  }

  /////////////////////////////////////////////////////////////////////
  public importerSettings: ImporterSetting[][] = new Array<ImporterSetting[]>(4);
  public importerSettingFilterds: Observable<ImporterSetting[]>[] = new Array<Observable<ImporterSetting[]>>();

  public initAutocompleteImporterSetting(
    formatType: FreeImporterFormatType,
    searchCondCtrl: FormControl,
    importerSettingService: ImporterSettingService,
    filedNumber: number
  ) {
    this.importerSettingFilterds[filedNumber] = undefined;
    searchCondCtrl.disable();
    importerSettingService.GetHeader(formatType)
      .subscribe(response => {
        this.importerSettings[filedNumber] = response;
        this.setValueChangeImporterSettings(searchCondCtrl, filedNumber);
        searchCondCtrl.enable();
      });
  }

  public setValueChangeImporterSettings(
    searchCondCtrl: FormControl,
    filedNumber: number
  ) {
    this.importerSettingFilterds[filedNumber] = searchCondCtrl.valueChanges
      .pipe(
        startWith(''),
        map(value =>
          (value ?
            this.filterImporterSettings(value, filedNumber)
            : this.importerSettings[filedNumber].slice()
          ).slice(0, this.AUTOCOMPLETE_MAX_COUNT)
        ),
      );
  }

  public filterImporterSettings(value: string, filedNumber: number): ImporterSetting[] {
    if (StringUtil.IsNullOrEmpty(value)) {
      this.importerSettings[filedNumber];
    }
    const searchValue = value.toLowerCase();
    return this.importerSettings[filedNumber].filter(x =>
      x.code.toLowerCase().indexOf(searchValue) !== -1 ||
      x.name.toLowerCase().indexOf(searchValue) !== -1);
  }


  /////////////////////////////////////////////////////////////////////
  public paymentAgencies: PaymentAgency[];
  public paymentAgencyFilterds: Observable<PaymentAgency[]>[] = new Array<Observable<PaymentAgency[]>>();

  public initAutocompletePaymentAgencies(
    searchCondCtrl: FormControl,
    paymentAgencyService: PaymentAgencyMasterService,
    filedNumber: number
  ) {
    this.paymentAgencyFilterds[filedNumber] = undefined;
    searchCondCtrl.disable();
    paymentAgencyService.GetItems()
      .subscribe(response => {
        this.paymentAgencies = response;
        this.setValueChangePaymentAgencies(searchCondCtrl, filedNumber);
        searchCondCtrl.enable();
      });
  }

  public setValueChangePaymentAgencies(
    searchCondCtrl: FormControl,
    filedNumber: number
  ) {
    this.paymentAgencyFilterds[filedNumber] = searchCondCtrl.valueChanges
      .pipe(
        startWith(''),
        map(value =>
          (value ?
            this.filterPaymentAgencies(value)
            : this.paymentAgencies.slice()
          ).slice(0, this.AUTOCOMPLETE_MAX_COUNT)
        ),
      );
  }

  public filterPaymentAgencies(value: string): PaymentAgency[] {
    if (StringUtil.IsNullOrEmpty(value)) {
      this.paymentAgencies;
    }
    const searchValue = value.toLowerCase();
    return this.paymentAgencies.filter(x =>
      x.code.toLowerCase().indexOf(searchValue) !== -1 ||
      x.name.toLowerCase().indexOf(searchValue) !== -1);
  }


  /////////////////////////////////////////////////////////////////////
  public freeCustomers: Customer[];
  public freeCustomerFilterds: Observable<Customer[]>[] = new Array<Observable<Customer[]>>();

  public initAutocompleteFreeCustomers(
    searchCondCtrl: FormControl,
    customerService: CustomerMasterService,
    filedNumber: number
  ) {
    this.freeCustomerFilterds[filedNumber] = undefined;
    searchCondCtrl.disable();

    let CustomerOption = new CustomerSearch();
    CustomerOption.isParent = 0;
    CustomerOption.xorParentCustomerId = 0;

    customerService.GetItemsByCustomerSearch(CustomerOption)
      .subscribe(response => {
        this.freeCustomers = response;
        this.setValueChangeFreeCustomers(searchCondCtrl, filedNumber);
        searchCondCtrl.enable();
      });
  }

  public setValueChangeFreeCustomers(
    searchCondCtrl: FormControl,
    filedNumber: number
  ) {
    this.freeCustomerFilterds[filedNumber] = searchCondCtrl.valueChanges
      .pipe(
        startWith(''),
        map(value =>
          (value ?
            this.filterFreeCustomers(value)
            : this.freeCustomers.slice()
          ).slice(0, this.AUTOCOMPLETE_MAX_COUNT)
        ),
      );
  }

  public filterFreeCustomers(value: string): Customer[] {
    if (StringUtil.IsNullOrEmpty(value)) {
      this.freeCustomers;
    }
    const searchValue = value.toLowerCase();
    return this.freeCustomers.filter(x =>
      x.code.toLowerCase().indexOf(searchValue) !== -1 ||
      x.name.toLowerCase().indexOf(searchValue) !== -1);
  }


  /////////////////////////////////////////////////////////////////////
  public accountTitles: AccountTitle[];
  public accountTitleFilterds: Observable<AccountTitle[]>[] = new Array<Observable<AccountTitle[]>>();

  public initautocompleteAccountTitle(
    searchCondCtrl: FormControl,
    accountService: AccountTitleMasterService,
    filedNumber: number
  ) {
    this.accountTitleFilterds[filedNumber] = undefined;
    searchCondCtrl.disable();


    accountService.Get()
      .subscribe(response => {
        this.accountTitles = response;
        this.setValueChangeAccountTitles(searchCondCtrl, filedNumber);
        searchCondCtrl.enable();
      });
  }

  public setValueChangeAccountTitles(
    searchCondCtrl: FormControl,
    filedNumber: number
  ) {
    this.accountTitleFilterds[filedNumber] = searchCondCtrl.valueChanges
      .pipe(
        startWith(''),
        map(value =>
          (value ?
            this.filterAccountTitles(value)
            : this.accountTitles.slice()
          ).slice(0, this.AUTOCOMPLETE_MAX_COUNT)
        ),
      );
  }

  public filterAccountTitles(value: string): AccountTitle[] {
    if (StringUtil.IsNullOrEmpty(value)) {
      this.accountTitles;
    }
    const searchValue = value.toLowerCase();
    return this.accountTitles.filter(x =>
      x.code.toLowerCase().indexOf(searchValue) !== -1 ||
      x.name.toLowerCase().indexOf(searchValue) !== -1);
  }


  /////////////////////////////////////////////////////////////////////
  public taxes: TaxClass[];
  public taxFilterds: Observable<TaxClass[]>[] = new Array<Observable<TaxClass[]>>();

  public initautocompleteTaxes(
    searchCondCtrl: FormControl,
    taxClassService: TaxClassMasterService,
    filedNumber: number
  ) {
    this.taxFilterds[filedNumber] = undefined;
    searchCondCtrl.disable();


    taxClassService.GetItems(0)
      .subscribe(response => {
        this.taxes = response;
        this.setValueChangeTaxes(searchCondCtrl, filedNumber);
        searchCondCtrl.enable();
      });
  }

  public setValueChangeTaxes(
    searchCondCtrl: FormControl,
    filedNumber: number
  ) {
    this.taxFilterds[filedNumber] = searchCondCtrl.valueChanges
      .pipe(
        startWith(''),
        map(value =>
          (value ?
            this.filterTaxes(value)
            : this.taxes.slice()
          ).slice(0, this.AUTOCOMPLETE_MAX_COUNT)
        ),
      );
  }

  public filterTaxes(value: string): TaxClass[] {
    if (StringUtil.IsNullOrEmpty(value)) {
      this.taxes;
    }
    const searchValue = value.toLowerCase();
    return this.taxes.filter(x =>
      (x.id.toString()).toLowerCase().indexOf(searchValue) !== -1 ||
      x.name.toLowerCase().indexOf(searchValue) !== -1);
  }


}