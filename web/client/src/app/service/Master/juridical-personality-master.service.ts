import { Injectable } from '@angular/core';
import { JuridicalPersonality } from 'src/app/model/juridical-personality.model';
import { JuridicalPersonalitysResult } from 'src/app/model/juridical-personalitys-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { Observable } from 'rxjs';
import { HttpRequestService } from '../common/http-request.service';
import { UserInfoService } from '../common/user-info.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { MasterImportSource } from 'src/app/model/master-import-source.model';

@Injectable({
  providedIn: 'root'
})
export class JuridicalPersonalityMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }


  public GetItems(): Observable<any> {
    let juridicalPersonality = new JuridicalPersonality();
    juridicalPersonality.companyId = this.userInfoService.Company.id;

    return this.httpRequestService.postReqest(WebApiUrl + 'JuridicalPersonality' + WebApiSaffix + '/GetItems', juridicalPersonality);
  }

  public Save(personality: JuridicalPersonality): Observable<any> {
    personality.companyId = this.userInfoService.Company.id;
    personality.updateBy = this.userInfoService.LoginUser.id;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'JuridicalPersonality' + WebApiSaffix + '/Save', personality);
  }

  public Delete(personality: JuridicalPersonality): Observable<any> {
    personality.companyId = this.userInfoService.Company.id;
    personality.updateBy = this.userInfoService.LoginUser.id;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'JuridicalPersonality' + WebApiSaffix + '/Delete', personality);
  }

  public Import(source: MasterImportSource): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'JuridicalPersonality' + WebApiSaffix + '/Import', source);
  }

  public GetReport(): Observable<any> {
    let companyId = this.userInfoService.Company.id;
    return this.httpRequestService.postReportReqest(
      WebApiUrl + 'JuridicalPersonality' + WebApiSaffix + '/GetReport', companyId);
  }

}
