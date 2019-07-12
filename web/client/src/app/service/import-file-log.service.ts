import { Injectable } from '@angular/core';
import { ImportFileLog} from '../model/import-file-log.model';
import { ImportFileLogsResult } from '../model/import-file-logs-result.model';
import { ProcessResult } from '../model/process-result.model';
import { Observable } from 'rxjs';
import { EBFileSettingSearch } from '../model/eb-file-setting-search.model';
import { UserInfoService } from './common/user-info.service';
import { HttpRequestService } from './common/http-request.service';
import { WebApiUrl, WebApiSaffix } from '../common/const/http.const';
import { EbFileInformation } from '../model/eb-file-information.model';

@Injectable({
  providedIn: 'root'
})
export class ImportFileLogService {

  constructor(
    private httpRequestService:HttpRequestService,
    private userInfoService:UserInfoService,
  ) { }

  public GetHistory(): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'ImportFileLog' + WebApiSaffix + '/GetHistory', 
      this.userInfoService.Company.id);
  } 

  public DeleteItems(deleteLogs:Array<number>): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'ImportFileLog' + WebApiSaffix + '/DeleteItems', 
      deleteLogs);
  } 
  
  public Import(ebFileInformations:Array<EbFileInformation>): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'ImportFileLog' + WebApiSaffix + '/Import', 
      ebFileInformations);
  } 
  
}
