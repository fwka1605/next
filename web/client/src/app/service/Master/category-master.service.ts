import { Injectable } from '@angular/core';
import { Category } from 'src/app/model/category.model';
import { CategoriesResult } from 'src/app/model/categories-result.model'
import { CategoryResult } from 'src/app/model/category-result.model'
import { ProcessResult } from 'src/app/model/process-result.model';
import { Observable } from 'rxjs';
import { CategorySearch } from 'src/app/model/category-search.model';
import { UserInfoService } from '../common/user-info.service';
import { HttpRequestService } from '../common/http-request.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { StringUtil } from 'src/app/common/util/string-util';

@Injectable({
  providedIn: 'root'
})
export class CategoryMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }

  private categoriesResult: CategoriesResult = null;
  public get CategoriesResult(): CategoriesResult {
    return this.categoriesResult;
  }

  public GetItems(categoryType: number, categoryCode: string = null, useInput: boolean | null = true): Observable<any> {
    let categorySearch = new CategorySearch();
    categorySearch.companyId = this.userInfoService.Company.id;
    categorySearch.categoryType = categoryType;
    if (useInput != undefined) {
      categorySearch.useInput = useInput ? 1 : 0;
    }

    if (!StringUtil.IsNullOrEmpty(categoryCode)) {
      categorySearch.codes = new Array<string>();
      categorySearch.codes.push(categoryCode);
    }

    return this.httpRequestService.postReqest(
      WebApiUrl + 'Category' + WebApiSaffix + '/GetItems', categorySearch);
  }

  public GetItemsByCategoryType(categoryType: number): Observable<any> {
    let categorySearch = new CategorySearch();
    categorySearch.companyId = this.userInfoService.Company.id;
    categorySearch.categoryType = categoryType;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'Category' + WebApiSaffix + '/GetItems', categorySearch);
  }  

  public Save(category: Category) {
    category.companyId = this.userInfoService.Company.id;
    category.updateBy = this.userInfoService.LoginUser.id;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'Category' + WebApiSaffix + '/Save', category);
  }

  public Delete(category: Category) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Category' + WebApiSaffix + '/Delete', category);
  }


  ///// Exist //////////////////////////////////////////////////////////////////////
  public ExistPaymentAgency(paymentAgencyId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Category' + WebApiSaffix + '/ExistPaymentAgency', paymentAgencyId);
  }

  public ExistAccountTitle(acccountTitleId: number): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Category' + WebApiSaffix + '/ExistAccountTitle', acccountTitleId);
  }

}
