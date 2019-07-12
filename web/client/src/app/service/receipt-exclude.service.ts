import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserInfoService } from './common/user-info.service';
import { HttpRequestService } from './common/http-request.service';
import { WebApiUrl, WebApiSaffix } from '../common/const/http.const';

@Injectable({
  providedIn: 'root'
})
export class ReceiptExcludeService {

  constructor(
    private httpRequestService:HttpRequestService,
    private userInfoService:UserInfoService,
  ) { }

  public GetByReceiptId(receiptId: number): Observable<any> {

     return this.httpRequestService.postReqest(WebApiUrl + 'ReceiptExclude' + WebApiSaffix + '/GetByReceiptId', receiptId);
  }    
}
