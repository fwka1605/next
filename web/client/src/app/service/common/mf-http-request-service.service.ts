import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { HttpClient, HttpHeaders, HttpResponse, HttpErrorResponse } from '@angular/common/http';

//import { WebApiHttpOptions } from '../../common/const/http.const';
import { Staff } from 'src/app/model/staff.model';
import { LoginService } from '../Master/login.service';
import { UserInfoService } from './user-info.service';
import { StringUtil } from 'src/app/common/util/string-util';
import { Company } from 'src/app/model/company.model';
//import { MfBaseUri, MfRedirectUri } from 'src/app/common/const/http.const';
import { WebApiSetting } from 'src/app/model/web-api-setting.model';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { HttpRequestService } from '../common/http-request.service';
import { MFWebApiOption } from 'src/app/model/mf-web-api-option.model';


@Injectable({
  providedIn: 'root'
})
export class MfHttpRequestServiceService {

  constructor(
    private httpClient: HttpClient,
    private httpRequestService: HttpRequestService,
  ) { }


  private result: any;
  private response: any;

  /**
   * Handle Http operation that failed.
   * Let the app continue.
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any, result: any): Observable<T> => {

      this.result = result;

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      console.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  public ValidateToken(mfWebApiOption: MFWebApiOption): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'MFWebApi' + WebApiSaffix + '/ValidateToken', mfWebApiOption);
  }

  public GetOffice(mfWebApiOption: MFWebApiOption): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'MFWebApi' + WebApiSaffix + '/GetOffice', mfWebApiOption);
  }

  public Save(mfWebApiOption: MFWebApiOption): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'MFWebApi' + WebApiSaffix + '/Save', mfWebApiOption);
  }

  public GetBillings(mfWebApiOption: MFWebApiOption): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'MFWebApi' + WebApiSaffix + '/GetBillings', mfWebApiOption);
  }

  public GetPartner(mfWebApiOption: MFWebApiOption): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'MFWebApi' + WebApiSaffix + '/GetPartner', mfWebApiOption);
  }

}