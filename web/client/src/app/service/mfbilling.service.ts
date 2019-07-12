import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { HttpRequestService } from 'src/app/service/common/http-request.service';
import { MFBillingSource } from 'src/app/model/mf-billing-source.model';
import { FormGroup } from '@angular/forms';
import { WebApiMFExtractSetting } from 'src/app/model/web-api-mf-extract-setting.model';

@Injectable({
  providedIn: 'root'
})
export class MFBillingService {

  constructor(
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

  public ExtractSetting: WebApiMFExtractSetting;   // 抽出設定項目
  public Pc1801myFormGroup: FormGroup;   // 抽出設定入力項目

  public Get(mfBillingSource: MFBillingSource): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'MFBilling' + WebApiSaffix + '/Get', mfBillingSource);
  }

  public Save(mfBillingSource: MFBillingSource): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'MFBilling' + WebApiSaffix + '/Save', mfBillingSource);
  }

}
