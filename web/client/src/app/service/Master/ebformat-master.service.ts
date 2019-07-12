import { Injectable } from '@angular/core';
import { EBFileSetting } from 'src/app/model/eb-file-setting.model';
import { EBFileSettingsResult } from 'src/app/model/eb-file-settings-result.model';
import { EBFormat } from 'src/app/model/eb-format.model';
import { EBFormatsResult } from 'src/app/model/eb-formats-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { HttpRequestService } from '../common/http-request.service';
import { UserInfoService } from '../common/user-info.service';

@Injectable({
  providedIn: 'root'
})
export class EBFormatMasterService {

  constructor(
    private httpRequestService:HttpRequestService,
    private userInforService:UserInfoService,    
  ) { }


  public GetItems(): Observable<any> {

    return this.httpRequestService.postReqest(WebApiUrl + 'EBFormat' + WebApiSaffix + '/GetItems', null);
  } 
  


}
