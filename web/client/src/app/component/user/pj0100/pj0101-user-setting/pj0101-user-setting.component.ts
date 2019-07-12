import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { ModalImportMethodSelectorComponent } from 'src/app/component/modal/modal-import-method-selector/modal-import-method-selector.component';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { KEY_CODE } from 'src/app/common/const/event.const';
import { LoginUserMasterService } from 'src/app/service/Master/login-user-master.service';
import { UsersResult } from 'src/app/model/users-result.model';
import { CustomValidators } from 'ng5-validation';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { LoginUser } from 'src/app/model/login-user.model';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { DateUtil } from 'src/app/common/util/date-util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { SectionWithLoginUserMasterService } from 'src/app/service/Master/section-with-login-user-master.service';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_ERR, MSG_ITEM_NUM } from 'src/app/common/const/message.const';

@Component({
  selector: 'app-pj0101-user-setting',
  templateUrl: './pj0101-user-setting.component.html',
  styleUrls: ['./pj0101-user-setting.component.css']
})
export class Pj0101UserSettingComponent extends BaseComponent implements OnInit {

  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;

  public usersResult: UsersResult;
  public user: LoginUser



  public selectIndex: number;
  public departmentId: number;
  public staffId: number;

  constructor(
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public loginUserService: LoginUserMasterService,
    public userInfoService: UserInfoService,
    public sectionWithLoginUserService: SectionWithLoginUserMasterService,
    public processResultService: ProcessResultService

  ) {
    super();
    this.Title = "ログインユーザーマスター";
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();
    this.getLoginUserData();

  }

  public setControlInit() {

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

    })
  }

  public setFormatter() {

  }

  public clear() {
    this.MyFormGroup.reset();
    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;
  }

  /**
   * データ取得
   */
  public getLoginUserData() {
    this.usersResult = new UsersResult();
    this.loginUserService.GetItems(this.userInfoService.LoginUser.code)
      .subscribe(response => {
        if (response != undefined) {
          this.usersResult.users = response;
          if (this.usersResult.users.length == 1) {
            this.user = this.usersResult.users[0];
          }
          else {
            this.processCustomResult = this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, 'ユーザ情報の取得'),
              this.partsResultMessageComponent);
          }
        }
        else {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, 'ユーザ情報の取得'),
            this.partsResultMessageComponent);
        }
      })
  }

}
