import { Injectable } from '@angular/core';
import { HttpRequestService } from '../common/http-request.service';
import { UserInfoService } from '../common/user-info.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';

@Injectable({
  providedIn: 'root'
})
export class PaymentFileFormatMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }

  public Get(): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'PaymentFileFormat' + WebApiSaffix + '/Get', '');
  }

}
