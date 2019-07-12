import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GridSetting } from 'src/app/model/grid-setting.model';
import { GridSettingSearch } from 'src/app/model/grid-setting-search.model';
import { UserInfoService } from '../common/user-info.service';
import { HttpRequestService } from '../common/http-request.service';
import { ServiceInfo } from '../util/service-info';

const controller = 'GridSetting'

@Injectable({
  providedIn: 'root'
})
export class GridSettingMasterService {
  
  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService:UserInfoService,
  ) { }

  public GetItems(gridId: number): Observable<any> {
    const gridSettingSearch: GridSettingSearch = {
      companyId:    this.userInfoService.Company.id,
      loginUserId:  this.userInfoService.LoginUser.id,
      gridId:       gridId,
      isDefault:    false,
    };
    return this.GetItemsBySearchObj(gridSettingSearch);
  }

  public GetItemsBySearchObj(gridSettingSearch:GridSettingSearch): Observable<any> {
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('GetItems', controller), gridSettingSearch);
  }

  public save(settings: GridSetting[]): Observable<any> {
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('Save', controller), settings);
  }
}
