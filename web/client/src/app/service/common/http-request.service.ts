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
import { PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';


@Injectable({
  providedIn: 'root'
})
export class HttpRequestService {

  constructor(
    private httpClient: HttpClient,
    private userInfoService: UserInfoService,
  ) { }


  private result: any;
  private response: any;
  public pdfData: any;

  public currentErrorMessage:string;

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

  /**
   * Handle Http operation that failed.
   * Let the app continue.
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  private handlePdf<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      this.pdfData = error.error.text.blog();


      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      console.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
  /** Log a HeroService message with the MessageService */
  private log(message: string) {
    console.log(`HeroService: ${message}`);
  }


  /**
   * Handle Http operation that failed.
   * Let the app continue.
   * @param url - API URL
   * @param postData - Object to Json
   */
  public postSpreadSheetReqest(url: string, postData: any): Observable<any> {

    if (!StringUtil.IsNullOrEmpty(this.userInfoService.AccessToken)) {

      return this.httpClient.post(url, postData,
        {
          headers:
          {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Headers': 'Origin',
            'Access-Control-Allow-Origin': '*',
            'VOne-AuthenticationKey': 'AAA',
            'VOne-TenantCode': this.userInfoService.TenantCode,
            'VOne-AccessToken': this.userInfoService.AccessToken,
            'observe': 'response',
            'responseType': 'blob',
          },
          observe: 'response',
          responseType: 'blob',
        }
      );
    }
  }

  /**
   * Handle Http operation that failed.
   * Let the app continue.
   * @param url - API URL
   * @param postData - Object to Json
   */
  public postReportReqest(url: string, postData: any): Observable<any> {

    if (!StringUtil.IsNullOrEmpty(this.userInfoService.AccessToken)) {

      return this.httpClient.post(url, postData,
        {
          headers:
          {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Headers': 'Origin',
            'Access-Control-Allow-Origin': '*',
            'VOne-AuthenticationKey': 'AAA',
            'VOne-TenantCode': this.userInfoService.TenantCode,
            'VOne-AccessToken': this.userInfoService.AccessToken,
            'observe': 'response',
            'responseType': 'blob',
          },
          observe: 'response',
          responseType: 'blob',
        }
      );
    }
  }


  public postReqest(url: string, postData: any): Observable<any> {

    let WebApiHttpOptions;

    if (StringUtil.IsNullOrEmpty(this.userInfoService.AccessToken)) {
      WebApiHttpOptions = {
        headers: new HttpHeaders(
          {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Headers': 'Origin',
            'Access-Control-Allow-Origin': '*',
            'VOne-AuthenticationKey': 'AAA',
            'VOne-TenantCode': this.userInfoService.TenantCode,
          }
        ),
        // observe:'response'
      };
    } else {
      WebApiHttpOptions = {
        headers: new HttpHeaders(
          {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Headers': 'Origin',
            'Access-Control-Allow-Origin': '*',
            'VOne-AuthenticationKey': 'AAA',
            'VOne-TenantCode': this.userInfoService.TenantCode,
            'VOne-AccessToken': this.userInfoService.AccessToken
          }
        ),
        // observe:'response',
      };
    }

    // observe: resonseを指定することで、HttpResponseを扱える
    return this.httpClient.post<any>(url, postData, WebApiHttpOptions).pipe(
      // HTTPステータスコードを戻す
      map((res: HttpResponse<any>) => {
        //console.log(res);
        if (res == undefined) {
          return undefined;
        }
        else if (res.body != undefined && res.body == 500) {
          return undefined;
        }
        else {
          return res;
        }

      }),

      // エラー時もHTTPステータスコードを戻す
      catchError((err: HttpErrorResponse) => {
        let result: any;
        if   (err.status == 500) {
          this.currentErrorMessage=err.error.Message; 
          console.log("HttpError.Response.error.Message:"+err.error.Message);
          result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
        }
        else {
          result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
        }

        return of(result);
      }),

    );
  }

}