import { Component, OnInit, EventEmitter, AfterViewInit, ElementRef } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { FormControl, FormGroup } from '@angular/forms';
import { BankBranchMasterService } from 'src/app/service/Master/bank-branch-master.service';
import { BankBranchsResult } from 'src/app/model/bank-branchs-result.model';
import { MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { BankBranch } from 'src/app/model/bank-branch.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { StringUtil } from 'src/app/common/util/string-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { EVENT_TYPE } from 'src/app/common/const/event.const';

@Component({
  selector: 'app-modal-master-bank',
  templateUrl: './modal-master-bank.component.html',
  styleUrls: ['./modal-master-bank.component.css']
})
export class ModalMasterBankComponent extends BaseComponent implements OnInit,AfterViewInit {

  public closing = new EventEmitter<{}>();

  public bankBranchsResult:BankBranchsResult;

  public selectedBankCode: string;
  public selectedBankKana: string;
  public selectedBankName: string;
  public selectedBranchCode: string;
  public selectedBranchKana: string;
  public selectedBranchName: string;

  public searchBankKeyCtrl:FormControl;
  public searchBranchKeyCtrl:FormControl;

  public selectedObject:BankBranch;

  constructor(
    public elementRef: ElementRef,
    public bankBranchService:BankBranchMasterService,
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
    
    HtmlUtil.nextFocusByName(this.elementRef, 'searchBankKeyCtrl', EVENT_TYPE.NONE);

  }

  public setControlInit() {
    this.searchBankKeyCtrl = new FormControl("");
    this.searchBranchKeyCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      searchBankKeyCtrl: this.searchBankKeyCtrl,
      searchBranchKeyCtrl: this.searchBranchKeyCtrl,
    });
  }

  public setFormatter() {

  }  

  public get TableClumn():string[]{
    return ["","銀行コード","銀行名カナ","銀行名","支店コード","支店名カナ","支店名"];
  }


  public get Closing() {
    return this.closing;
  }
  public set Closing(value) {
    this.closing = value;
  }

  public get SelectedBankCode(): string {
    return this.selectedBankCode;
  }
  public set SelectedBankCode(value: string) {
    this.selectedBankCode = value;
  }

  public get SelectedBankKana(): string {
    return this.selectedBankKana;
  }
  public set SelectedBankKana(value: string) {
    this.selectedBankKana = value;
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

  public get SelectedBranchKana(): string {
    return this.selectedBranchKana;
  }
  public set SelectedBranchKana(value: string) {
    this.selectedBranchKana = value;
  }

  public get SelectedBranchName(): string {
    return this.selectedBranchName;
  }
  public set SelectedBranchName(value: string) {
    this.selectedBranchName = value;
  }

  public get SelectedObject(): BankBranch {
    return this.selectedObject;
  }
  public set SelectedObject(value: BankBranch) {
    this.selectedObject = value;
  }

  public clear(){
    this.MyFormGroup.reset();
    HtmlUtil.nextFocusByName(this.elementRef, 'searchBankKeyCtrl', EVENT_TYPE.NONE);
    
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

  public select(index:number){

    this.ModalStatus=MODAL_STATUS_TYPE.SELECT;

    this.selectedBankCode = this.bankBranchsResult.bankBranches[index].bankCode;
    this.selectedBankKana = this.bankBranchsResult.bankBranches[index].bankKana;
    this.selectedBankName = this.bankBranchsResult.bankBranches[index].bankName;
    this.selectedBranchCode = this.bankBranchsResult.bankBranches[index].branchCode;
    this.selectedBranchKana = this.bankBranchsResult.bankBranches[index].branchKana;
    this.selectedBranchName = this.bankBranchsResult.bankBranches[index].branchName;

    this.selectedObject = this.bankBranchsResult.bankBranches[index];

    this.closing.emit();
  }

  public search(){
    
    this.bankBranchsResult = new BankBranchsResult();
    this.bankBranchsResult.processResult = new ProcessResult();
    this.bankBranchsResult.processResult.result = false;
    this.bankBranchsResult.bankBranches = new Array<BankBranch>();

    this.bankBranchService.GetItems()
      .subscribe(response => {
        this.bankBranchsResult.processResult.result = true;
        this.bankBranchsResult.bankBranches = response;

        if (
              response != undefined 
          &&  response.length > 0 
          && (
                  !StringUtil.IsNullOrEmpty(this.searchBankKeyCtrl.value) 
              ||  !StringUtil.IsNullOrEmpty(this.searchBranchKeyCtrl.value)
             ) 
        )
        {
          let searchBankKey = this.searchBankKeyCtrl.value;
          let searchBranchKey = this.searchBranchKeyCtrl.value;
          this.bankBranchsResult.bankBranches = this.bankBranchsResult.bankBranches.filter(

            function (bankBranch: BankBranch) {

              if(StringUtil.IsNullOrEmpty(searchBankKey)){
                return
                     bankBranch.branchCode.indexOf(searchBranchKey) != -1
                  || bankBranch.branchName.indexOf(searchBranchKey) != -1
                  || bankBranch.branchKana.indexOf(searchBranchKey) != -1;

              }
              else if (StringUtil.IsNullOrEmpty(searchBranchKey)) {
                return
                    bankBranch.bankCode.indexOf(searchBankKey) != -1
                    || bankBranch.bankName.indexOf(searchBankKey) != -1
                    || bankBranch.bankKana.indexOf(searchBankKey) != -1;
              }
              else{
                return
                    bankBranch.bankCode.indexOf(searchBankKey) != -1
                  || bankBranch.bankName.indexOf(searchBankKey) != -1
                  || bankBranch.bankKana.indexOf(searchBankKey) != -1
                  || bankBranch.branchCode.indexOf(searchBranchKey) != -1
                  || bankBranch.branchName.indexOf(searchBranchKey) != -1
                  || bankBranch.branchKana.indexOf(searchBranchKey) != -1;

              }
            }
          )
        }
      });

  }

  public openSortExists: object = {}
  public sortOpenExistsInit() {
    for (let i = 0; i < this.TableClumn.length; i++) {
      this.openSortExists[i] = false;
    }
  }

  public openSort(index: number) {
    this.openSortExists[index] = !this.openSortExists[index]
  }




}
