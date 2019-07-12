import { Injectable } from '@angular/core';
import { ColumnNameSettingsResult } from 'src/app/model/column-name-settings-result.model';
import { ColumnNameSetting } from 'src/app/model/column-name-setting.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { Observable } from 'rxjs';
import { HttpRequestService } from '../common/http-request.service';
import { UserInfoService } from '../common/user-info.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { FreeImporterFormatType } from 'src/app/common/common-const';
import { CategoryType } from 'src/app/common/const/kbn.const';

@Injectable({
  providedIn: 'root'
})
export class ColumnNameSettingMasterService {

  constructor(
    private userInfoService:UserInfoService,
    private httpRequestService:HttpRequestService
  ) { }


  public Get(tableName:number): Observable<any> {

    let columnNameSetting = new ColumnNameSetting();
    columnNameSetting.companyId = this.userInfoService.Company.id;

    if(tableName==CategoryType.Billing){
      columnNameSetting.tableName="Billing"
    }
    else if(tableName==CategoryType.Receipt){
      columnNameSetting.tableName="Receipt"
    }
    else{
      columnNameSetting.tableName=""
    }

    return this.httpRequestService.postReqest(WebApiUrl + 'ColumnNameSetting' + WebApiSaffix + '/Get', columnNameSetting);
  }

  public Save(columnName: ColumnNameSetting): Observable<any> {
    columnName.companyId = this.userInfoService.Company.id;
    columnName.updateBy = this.userInfoService.LoginUser.id;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'ColumnNameSetting' + WebApiSaffix + '/Save', columnName);
  }      


}
