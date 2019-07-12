import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpRequestService } from '../common/http-request.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';

@Injectable({
  providedIn: 'root'
})
export class SettingMasterService {

  constructor(
    private httpRequestService:HttpRequestService    
  ) { }

  public GetItems(ids:Array<string>=null): Observable<any> {

    if(ids==null){
      ids = new Array<string>();
    }
    return this.httpRequestService.postReqest(WebApiUrl + 'Setting' + WebApiSaffix + '/GetItems', ids);
  } 


}
