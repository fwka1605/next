import { Component, OnInit, EventEmitter } from '@angular/core';
import { TABLE_INDEX, TABLE_NAME, TABLE_COLUMN } from 'src/app/common/const/table-name.const';
import { BaseComponent } from '../../common/base/base-component';
import { FormControl,  FormGroup } from '@angular/forms';
import { MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { DepartmentResult } from 'src/app/model/department-result.model';
import { Department } from 'src/app/model/department.model';
import { StringUtil } from 'src/app/common/util/string-util';
import { SectionResult } from 'src/app/model/section-result.model';
import { Section } from 'src/app/model/section.model';
import { ProcessResultService } from 'src/app/service/common/process-result.service';

@Component({
  selector: 'app-modal-multi-master',
  templateUrl: './modal-multi-master.component.html',
  styleUrls: ['./modal-multi-master.component.css']
})
export class ModalMultiMasterComponent extends BaseComponent implements OnInit {

  public mastersAllResult:any;

  public tableIndex: TABLE_INDEX;
  
  public closing = new EventEmitter<{}>();
  
  public initialIds:number[];

  public selectedIds: number[];
  public selectedCodes: string[];
  public selectedObjects:any[];

  public selectedAll:boolean;

  public cbxAllSelectCtrl:FormControl;

  public searchKeyCtrl: FormControl;

  public cbxSelectCtrls:FormControl[];

  constructor(
    public departmentService:DepartmentMasterService,
    public sectionService:SectionMasterService,
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

  public setControlInit() {
    this.cbxAllSelectCtrl = new FormControl(null);
    this.searchKeyCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      cbxAllSelectCtrl: this.cbxAllSelectCtrl,
      searchKeyCtrl: this.searchKeyCtrl,
    });
  }

  public setFormatter() {
  }  

  public clear(){
    this.MyFormGroup.reset();
  }

  public get TableIndex(): TABLE_INDEX {
    return this.tableIndex;
  }
  public set TableIndex(value: TABLE_INDEX) {
    this.tableIndex = value;
    
  }
  public get TableName(): String {
    return TABLE_NAME[this.tableIndex];
  }

  public get TableColumns():string[]{
    return TABLE_COLUMN[this.tableIndex];
  }

  public get Closing() {
    return this.closing;
  }
  public set Closing(value) {
    this.closing = value;
  }

  public get SelectedIds(): number[] {
    return this.selectedIds;
  }
  public set SelectedIds(value: number[]) {
    this.selectedIds = value;
  }

  public get SelectedCodes(): string[] {
    return this.selectedCodes;
  }
  public set SelectedCodes(value: string[]) {
    this.selectedCodes = value;
  }

  public get SelectedObjects(): any[] {
    return this.selectedObjects;
  }
  public set SelectedObjects(value: any[]) {
    this.selectedObjects = value;
  }

  public get InitialIds(): number[] {
    return this.initialIds;
  }
  public set InitialIds(value: number[]) {
    this.initialIds = value;
  }

  public get isSelectedAll():boolean{
    return this.selectedAll;
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

  public search() {
    switch(this.tableIndex){
      case this.TABLE_INDEX.MASTER_DEPARTMENT:
      {
        this.mastersAllResult = new DepartmentResult();
        this.mastersAllResult.departments = new Array<Department>();

        this.selectedIds = new Array<number>();
        this.selectedCodes = new Array<string>();
        this.selectedObjects = new Array<any>();
        this.selectedAll=true;


        this.departmentService.GetItems()
            .subscribe(response => {
              this.mastersAllResult.departments = response;

              if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
                let searchKeyCtrl = this.searchKeyCtrl.value
                this.mastersAllResult.departments= this.mastersAllResult.departments.filter(
                  function (department:Department) {
                    return department.code.indexOf(searchKeyCtrl) != -1 || department.name.indexOf(searchKeyCtrl) != -1;
                  }
                )
              }

              this.cbxSelectCtrls = new Array<FormControl>(this.mastersAllResult.departments.length);
              for (let index = 0; index < this.cbxSelectCtrls.length; index++) {

                if(this.initialIds==null || this.initialIds.length==null){
                  this.cbxSelectCtrls[index] = new FormControl(true);
                  this.MyFormGroup.removeControl("cbxSelectCtrl" + index);
                  this.MyFormGroup.addControl("cbxSelectCtrl" + index, this.cbxSelectCtrls[index]);
  
                  this.selectedIds.push(this.mastersAllResult.departments[index].id);
                  this.selectedCodes.push(this.mastersAllResult.departments[index].code);
                  this.selectedObjects.push(this.mastersAllResult.departments[index]);

                  this.selectedAll=true;
                }
                else{
                  this.selectedAll=false;

                  let bExist:boolean = false;
                  this.initialIds.forEach(element=>{
                    if(element==this.mastersAllResult.departments[index].id){
                      bExist=true;
                    }
                  });

                  if(bExist){
                    this.cbxSelectCtrls[index] = new FormControl(true);
                    this.MyFormGroup.removeControl("cbxSelectCtrl" + index);
                    this.MyFormGroup.addControl("cbxSelectCtrl" + index, this.cbxSelectCtrls[index]);
    
                    this.selectedIds.push(this.mastersAllResult.departments[index].id);
                    this.selectedCodes.push(this.mastersAllResult.departments[index].code);
                    this.selectedObjects.push(this.mastersAllResult.departments[index]);

                  }
                  else{
                    this.cbxSelectCtrls[index] = new FormControl(null);
                    this.MyFormGroup.removeControl("cbxSelectCtrl" + index);
                    this.MyFormGroup.addControl("cbxSelectCtrl" + index, this.cbxSelectCtrls[index]);

                  }

                }


              }
            });
        break;
      }
      case this.TABLE_INDEX.MASTER_SECTION:
      {

        this.mastersAllResult = new SectionResult();
        this.mastersAllResult.sections = new Array<Section>();

        this.selectedIds = new Array<number>();
        this.selectedCodes = new Array<string>();
        this.selectedObjects = new Array<any>();
        this.selectedAll=true;

        this.sectionService.GetItems()
          .subscribe(response => {
            this.mastersAllResult.sections = response;

            if (response != undefined && response.length > 0 && !StringUtil.IsNullOrEmpty(this.searchKeyCtrl.value)) {
              let searchKeyCtrl = this.searchKeyCtrl.value
              this.mastersAllResult.sections = this.mastersAllResult.sections.filter(
                function (section: Section) {
                  return section.code.indexOf(searchKeyCtrl) != -1 || section.name.indexOf(searchKeyCtrl) != -1;
                }
              )
            }

            this.cbxSelectCtrls = new Array<FormControl>(this.mastersAllResult.sections.length);
            for (let index = 0; index < this.cbxSelectCtrls.length; index++) {

              if(this.initialIds==null || this.initialIds.length==null){
                this.cbxSelectCtrls[index] = new FormControl(true);
                this.MyFormGroup.removeControl("cbxSelectCtrl" + index);
                this.MyFormGroup.addControl("cbxSelectCtrl" + index, this.cbxSelectCtrls[index]);
  
                this.selectedIds.push(this.mastersAllResult.departments[index].id);
                this.selectedCodes.push(this.mastersAllResult.departments[index].code);
                this.selectedObjects.push(this.mastersAllResult.departments[index]);

                this.selectedAll=true;
              }
              else{
                this.selectedAll=false;

                let bExist:boolean = false;
                this.initialIds.forEach(element=>{
                  if(element==this.mastersAllResult.sections[index].id){
                    bExist=true;
                  }
                });

                if(bExist){
                  this.cbxSelectCtrls[index] = new FormControl(true);
                  this.MyFormGroup.removeControl("cbxSelectCtrl" + index);
                  this.MyFormGroup.addControl("cbxSelectCtrl" + index, this.cbxSelectCtrls[index]);
    
                  this.selectedIds.push(this.mastersAllResult.sections[index].id);
                  this.selectedCodes.push(this.mastersAllResult.sections[index].code);
                  this.selectedObjects.push(this.mastersAllResult.sections[index]);

                }
                else{
                  this.cbxSelectCtrls[index] = new FormControl(null);
                  this.MyFormGroup.removeControl("cbxSelectCtrl" + index);
                  this.MyFormGroup.addControl("cbxSelectCtrl" + index, this.cbxSelectCtrls[index]);
                }
              }
            }
          });
        break;
      }
    }

  }

  public select(index:number) {

    // 符号を反転する
    this.cbxSelectCtrls[index].setValue(!this.cbxSelectCtrls[index].value);

    this.selectedIds = new Array<number>();
    this.selectedCodes = new Array<string>();
    this.selectedObjects = new Array<any>();
    this.selectedAll=true;

    let loopCounter:number = 0;

    switch(this.tableIndex){
      case this.TABLE_INDEX.MASTER_DEPARTMENT:
      {
        this.cbxSelectCtrls.forEach(element=>{
          if(this.cbxSelectCtrls[loopCounter].value){
            this.selectedIds.push(this.mastersAllResult.departments[loopCounter].id);
            this.selectedCodes.push(this.mastersAllResult.departments[loopCounter].code);
            this.selectedObjects.push(this.mastersAllResult.departments[loopCounter]);
          }
          else{
            this.selectedAll = false;
          }
          loopCounter+=1;
        });
        break;
      }
      case this.TABLE_INDEX.MASTER_SECTION:
      {
        this.cbxSelectCtrls.forEach(element=>{
          if(this.cbxSelectCtrls[loopCounter].value){
            this.selectedIds.push(this.mastersAllResult.sections[loopCounter].id);
            this.selectedCodes.push(this.mastersAllResult.sections[loopCounter].code);
            this.selectedObjects.push(this.mastersAllResult.sections[loopCounter]);
          }
          else{
            this.selectedAll = false;
          }
          loopCounter+=1;
        });
        break;
      }
    }

  }


  public selectAll() {

    this.selectedIds = new Array<number>();
    this.selectedCodes = new Array<string>();
    this.selectedObjects = new Array<any>();
    this.selectedAll=true;

    this.cbxSelectCtrls.forEach(element=>element.setValue(true));

    switch(this.tableIndex){
      case this.TABLE_INDEX.MASTER_DEPARTMENT:
      {
        this.mastersAllResult.departments.forEach(element=>{
            this.selectedIds.push(element.id);
            this.selectedCodes.push(element.code);
            this.selectedObjects.push(element);
        });
        break;
      }
      case this.TABLE_INDEX.MASTER_SECTION:
      {
        this.mastersAllResult.sections.forEach(element=>{
          this.selectedIds.push(element.id);
          this.selectedCodes.push(element.code);
          this.selectedObjects.push(element);
        });
        break;      
      }
    }
  }


  public clearAll() {

    this.selectedIds = new Array<number>();
    this.selectedCodes = new Array<string>();
    this.selectedObjects = new Array<any>();
    this.selectedAll=true;

    this.cbxSelectCtrls.forEach(element=>element.setValue(null));

  }

  public checkAll() {

    if(this.cbxSelectCtrls==null || this.cbxSelectCtrls.length==0){
      return;
    }

    if(this.cbxAllSelectCtrl.value){
      this.selectAll();
    }
    else{
      this.clearAll();
    }
  }


  public registry(){
    this.ModalStatus=MODAL_STATUS_TYPE.SELECT;
    this.closing.emit();
  }

  /**
   * name
   */
  public canRegistry():boolean {
    let bRtn = false;

    if(this.cbxSelectCtrls==null){
      return false;
    }
    this.cbxSelectCtrls.forEach(element=>{
      if(element.value==true) bRtn=true;
    });

    return bRtn;
  }

}
