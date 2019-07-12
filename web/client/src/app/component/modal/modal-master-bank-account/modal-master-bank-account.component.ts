import { Component, OnInit, EventEmitter, AfterViewInit, ElementRef } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { BankAccountsResult } from 'src/app/model/bank-accounts-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { BankAccount } from 'src/app/model/bank-account.model';
import { BankAccountMasterService } from 'src/app/service/Master/bank-account-master.service';
import { StringUtil } from 'src/app/common/util/string-util';
import { FormControl, FormGroup } from '@angular/forms';
import { MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { EVENT_TYPE } from 'src/app/common/const/event.const';
import { HtmlUtil } from 'src/app/common/util/html.util';

@Component({
  selector: 'app-modal-master-bank-account',
  templateUrl: './modal-master-bank-account.component.html',
  styleUrls: ['./modal-master-bank-account.component.css']
})
export class ModalMasterBankAccountComponent extends BaseComponent implements OnInit,AfterViewInit {

  public bankAccountsResult:BankAccountsResult;

  public closing = new EventEmitter<{}>();

  public searchKeyCtrl:FormControl;

  constructor(
    public elementRef: ElementRef,
    public bankAccountService:BankAccountMasterService,
    public processResultService:ProcessResultService

  ) {
    super();
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();

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
    this.search();
    HtmlUtil.nextFocusByName(this.elementRef, 'searchKeyCtrl', EVENT_TYPE.NONE);

  }



  public search() {

    this.bankAccountsResult = new BankAccountsResult();
    this.bankAccountsResult.processResult = new ProcessResult();
    this.bankAccountsResult.processResult.result = false;
    this.bankAccountsResult.bankAccounts = new Array<BankAccount>();

    this.bankAccountService.GetItems()
      .subscribe(response => {
        this.bankAccountsResult.processResult.result = true;
        this.bankAccountsResult.bankAccounts = response;

        if (
              response != undefined
          &&  response.length > 0
          &&  !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)
        ) {

          let searchKey = this.searchKeyCtrl.value;
          this.bankAccountsResult.bankAccounts = this.bankAccountsResult.bankAccounts.filter(

            function (bankAcount: BankAccount) {

              return
              bankAcount.bankCode.indexOf(searchKey) != -1
                || bankAcount.bankName.indexOf(searchKey) != -1

            }
          )
        }
      });
  }

  public get Closing() {
    return this.closing;
  }
  public set Closing(value) {
    this.closing = value;
  }

  public selectedBankCode: string;
  public selectedBankName: string;

  public selectedBranchCode: string;
  public selectedBranchName: string;

  public selectedAccountTypeName: string;
  public selectedAccountTypeId: number;
  public selectedAccountNumber: string;

  public get SelectedBankCode(): string {
    return this.selectedBankCode;
  }
  public set SelectedBankCode(value: string) {
    this.selectedBankCode = value;
  }

  public get SelectedBankName(): string {
    return this.selectedBankName;
  }
  public set SelectedBankName(value: string) {
    this.selectedBankName = value;
  }

  public get SelectedBranchCode(): string {
    return this.selectedBranchCode;
  }
  public set SelectedBranchCode(value: string) {
    this.selectedBranchCode = value;
  }

  public get SelectedBranchName(): string {
    return this.selectedBranchName;
  }
  public set SelectedBranchName(value: string) {
    this.selectedBranchName = value;
  }

  public get SelectedAccountTypeName(): string {
    return this.selectedAccountTypeName;
  }
  public set SelectedAccountTypeName(value: string) {
    this.selectedAccountTypeName = value;
  }

  public get SelectedAccountTypeId(): number {
    return this.selectedAccountTypeId;
  }
  public set SelectedAccountTypeId(value: number) {
    this.selectedAccountTypeId = value;
  }
  

  public get SelectedAccountNumber(): string {
    return this.selectedAccountNumber;
  }
  public set SelectedAccountNumber(value: string) {
    this.selectedAccountNumber = value;
  }

  public select(index:number){

    this.ModalStatus = MODAL_STATUS_TYPE.SELECT;
    
    this.SelectedBankCode = this.bankAccountsResult.bankAccounts[index].bankCode;
    this.SelectedBankName = this.bankAccountsResult.bankAccounts[index].bankName;
    this.SelectedBranchCode = this.bankAccountsResult.bankAccounts[index].branchCode;
    this.SelectedBranchName = this.bankAccountsResult.bankAccounts[index].branchName;
    this.SelectedAccountTypeName = this.bankAccountsResult.bankAccounts[index].accountTypeName;
    this.selectedAccountTypeId = this.bankAccountsResult.bankAccounts[index].accountTypeId;
    this.SelectedAccountNumber = this.bankAccountsResult.bankAccounts[index].accountNumber;

    this.closing.emit();

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


}
