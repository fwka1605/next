import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { StringUtil } from 'src/app/common/util/string-util';


@Injectable({
    providedIn: 'root'
})
export class MfAggregateWebApiClient {
  private readonly MediaTypeFormUrlEncoded = "application/x-www-form-urlencoded";
  private readonly MediaTypeJson = "application/json";
  public readonly RedirectUri = "https://www.r-ac.co.jp";
  public readonly AuthorizationEndpointUri = "https://statement.moneyforward.com";
  private readonly BaseUri = "https://statement-api.moneyforward.com";
  private readonly HeaderAuthKey = "X-MFOAuthToken";
  public readonly ApiVersion = "v1";
  public readonly Scope = "read";
  public TokenRefreshed: boolean;
  public ClientId: string;
  public ClientSecret: string;
  public AccessToken: string;
  public RefreshToken: string;

  public currentErrorMessage:string;

  public get getRedirectUri(): string {
    return this.RedirectUri;
  }

  //public Action<string> LogListener { get; set; }

  public constructor(
      public httpClient: HttpClient,
  ) {
  }

  private readonly RequestIntervalSecondForDefaultApi = 1;

  public getUriSimple(method: String, baseUri: String = this.BaseUri): string {
      return `${baseUri}/${method}`;
  }

  public getUri(method: String, baseUri: String = this.BaseUri): string {
      return this.getUriSimple(`api/${this.ApiVersion}/${method}`, baseUri);
  }

  public getAuthorizationCode() {
    // 現在は手動取得
  }

  public requestToken(code: string): Observable<any> {
    const url = this.getUriSimple('oauth/token');
    const httpOptions = {
      headers: new HttpHeaders({
          'Content-Type': 'application/x-www-form-urlencoded'
      })
      };
      const body = 'grant_type=authorization_code' +
      `&client_id=${this.ClientId}` +
      `&client_secret=${this.ClientSecret}` +
      `&redirect_uri=${this.RedirectUri}` +
      `&code=${code}`;

    return this.httpClient.post<any>(url, body, httpOptions).pipe(
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


  public getAccounts(
    account_ids: number[] = null, 
    sub_account_ids: number[] = null, 
    tags: number[] = null): Observable<any> {
    
    let queries = new Array<string>();

    if (account_ids != null && account_ids.length > 0) {
      account_ids.forEach(x => {
        queries.push(`account_ids[]=${x}`);
      });
    }

    if (sub_account_ids != null && sub_account_ids.length > 0) {
      sub_account_ids.forEach(x => {
        queries.push(`sub_account_ids[]=${x}`);
      });
    }

    if (tags != null && tags.length > 0) {
      tags.forEach(x => {
        queries.push(`tags[]=${x}`);
      });
    }

    const fixedQuery = queries.join('&');
    const url = this.getUri(`accounts?${fixedQuery}`);
    const httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json',
            //'Accept-Charset'      : 'utf-8',
            'Authorization': `Bearer ${this.AccessToken}`
        })
    };

    return this.httpClient.get(url, httpOptions).pipe(
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
  
  public getTags(): Observable<any> {
    const url = this.getUri("tags");
    const httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json',
            //'Accept-Charset'      : 'utf-8',
            'Authorization': `Bearer ${this.AccessToken}`
        })
    };

    return this.httpClient.get(url, httpOptions).pipe(
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

  public getTransactions(
    extractDateFrom: string, 
    extractDateTo: string, 
    account_ids: number[] = null, 
    sub_account_ids: number[] = null, 
    tags: number[] = null): Observable<any> {

    let queries = new Array<string>();

    if (!StringUtil.IsNullOrEmpty(extractDateFrom)) {
      queries.push(`from_date=${extractDateFrom}`);
    }

    if (!StringUtil.IsNullOrEmpty(extractDateTo)) {
      queries.push(`to_date=${extractDateTo}`);
    }

    if (account_ids != null && account_ids.length > 0) {
      account_ids.forEach(x => {
        queries.push(`account_ids[]=${x}`);
      });
    }

    if (sub_account_ids != null && sub_account_ids.length > 0) {
      sub_account_ids.forEach(x => {
        queries.push(`sub_account_ids[]=${x}`);
      });
    }

    if (tags != null && tags.length > 0) {
      tags.forEach(x => {
        queries.push(`tags[]=${x}`);
      });
    }

    const fixedQuery = queries.join('&');
    const offset  = 0;
    const limit   = 3000;

    const url = this.getUri(`transactions?${fixedQuery}&offset=${offset}&limit=${limit}`);

    const httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json',
            //'Accept-Charset'      : 'utf-8',
            'Authorization': `Bearer ${this.AccessToken}`
        })
    };

    return this.httpClient.get(url, httpOptions).pipe(
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