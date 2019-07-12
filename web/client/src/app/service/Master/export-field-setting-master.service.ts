import { Injectable } from '@angular/core';
import { HttpRequestService } from 'src/app/service/common/http-request.service';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { ExportFieldSetting } from 'src/app/model/export-field-setting.model';

@Injectable({
  providedIn: 'root'
})
export class ExportFieldSettingMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }

  public GetItems(setting:ExportFieldSetting): Observable<any> {
    return  this.httpRequestService.postReqest(
              WebApiUrl + 'ExportFieldSetting' + WebApiSaffix + '/GetItems',setting);
  }

  public Save(settings:ExportFieldSetting[]): Observable<any> {
    return  this.httpRequestService.postReqest(
              WebApiUrl + 'ExportFieldSetting' + WebApiSaffix + '/Save',settings);
  }

}
