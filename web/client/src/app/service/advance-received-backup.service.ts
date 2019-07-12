import { Injectable } from '@angular/core';
import { HttpRequestService } from './common/http-request.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from '../common/const/http.const';


@Injectable({
  providedIn: 'root'
})
export class AdvanceReceivedBackupService {

  constructor(
    private httpRequestService: HttpRequestService,
  ) { }

  public GetByOriginalReceiptId(originalReceiptId: number): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'AdvanceReceivedBackup' + WebApiSaffix + '/GetByOriginalReceiptId', originalReceiptId);
  }

}
