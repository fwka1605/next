import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CollationSetting } from 'src/app/model/collation-setting.model';
import { UserInfoService } from '../common/user-info.service';
import { HttpRequestService } from '../common/http-request.service';
import { ServiceInfo } from '../util/service-info';

const controller = 'CollationSetting';

@Injectable({
  providedIn: 'root'
})
export class CollationSettingMasterService {

  constructor(
    private httpRequestService:HttpRequestService,
    private userInfoService: UserInfoService,

  ) { }

  public Get(): Observable<any> {
    let collationSetting = new CollationSetting();
    collationSetting.companyId = this.userInfoService.Company.id;
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('Get', controller), collationSetting );
  }    

  
  public GetMatchingBillingOrder(): Observable<any> {
    let collationSetting = new CollationSetting();
    collationSetting.companyId = this.userInfoService.Company.id;
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('GetMatchingBillingOrder', controller), collationSetting );
  }   

  
  public GetMatchingReceiptOrder(): Observable<any> {
    let collationSetting = new CollationSetting();
    collationSetting.companyId = this.userInfoService.Company.id;
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('GetMatchingReceiptOrder', controller), collationSetting );
  }     
  
  public GetCollationOrder(): Observable<any> {
    let collationSetting = new CollationSetting();
    collationSetting.companyId = this.userInfoService.Company.id;
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('GetCollationOrder', controller), collationSetting );
  }     

  public save(setting: CollationSetting): Observable<any> {
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('Save', controller), setting);
  }
}
