import { Injectable } from '@angular/core';
import { PaymentAgenciesResult } from 'src/app/model/payment-agencies-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { PaymentAgency } from 'src/app/model/payment-agency.model';
import { PaymentFileFormatResult } from 'src/app/model/payment-file-format-result.model';
import { PaymentFileFormat } from 'src/app/model/payment-file-format.model';
import { UserInfoService } from '../common/user-info.service';
import { MasterSearchOption } from 'src/app/model/master-search-option';
import { HttpRequestService } from '../common/http-request.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';

@Injectable({
  providedIn: 'root'
})
export class PaymentAgencyMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,

  ) { }

  public GetItems() {
    let masterSearchOption = new MasterSearchOption();
    masterSearchOption.companyId = this.userInfoService.Company.id;

    return this.httpRequestService.postReqest(WebApiUrl + 'PaymentAgency' + WebApiSaffix + '/GetItems', masterSearchOption);

  }

  public GetItemsById(id:number) {
    let masterSearchOption = new MasterSearchOption();
    masterSearchOption.companyId = this.userInfoService.Company.id;
    masterSearchOption.ids = new Array<number>();
    masterSearchOption.ids.push(id);

    return this.httpRequestService.postReqest(WebApiUrl + 'PaymentAgency' + WebApiSaffix + '/GetItems', masterSearchOption);
  }


  public GetItemsByCode(code:string) {
    let masterSearchOption = new MasterSearchOption();
    masterSearchOption.companyId = this.userInfoService.Company.id;
    masterSearchOption.codes = new Array<string>();
    masterSearchOption.codes.push(code);

    return this.httpRequestService.postReqest(WebApiUrl + 'PaymentAgency' + WebApiSaffix + '/GetItems', masterSearchOption);
  }  

  public Save(agency: PaymentAgency) {
    agency.companyId = this.userInfoService.Company.id;
    agency.updateBy = this.userInfoService.LoginUser.id;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'PaymentAgency' + WebApiSaffix + '/Save', agency);
  }

  public Delete(agency: PaymentAgency) {
    agency.companyId = this.userInfoService.Company.id;
    agency.updateBy = this.userInfoService.LoginUser.id;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'PaymentAgency' + WebApiSaffix + '/Delete', agency);
  }

  public GetFileFormat(){
    return this.httpRequestService.postReqest(
      WebApiUrl + 'PaymentFileFormat' + WebApiSaffix + '/Get', "");    
  }

}
