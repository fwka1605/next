import { Injectable } from '@angular/core';
import { HttpRequestService } from '../common/http-request.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { ApplicationControl } from 'src/app/model/application-control.model';

@Injectable({
  providedIn: 'root'
})
export class ApplicationControlMasterService {


  constructor(
    private httpRequestService:HttpRequestService
  ) { }

  public Get(companyId:number): Observable<any>{
    return this.httpRequestService.postReqest(WebApiUrl + 'ApplicationControl' + WebApiSaffix + '/Get', companyId);

  }
}
