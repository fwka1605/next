import { Injectable } from '@angular/core';
import { HttpRequestService } from './common/http-request.service';
import { UserInfoService } from './common/user-info.service';
import { Observable } from 'rxjs';
import { MatchingHistorySearch} from '../model/matching-history-search.model';
import { WebApiUrl, WebApiSaffix } from '../common/const/http.const';

@Injectable({
  providedIn: 'root'
})
export class MatchingHistoryService {
  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }

  public Get(matchingHistorySearch: MatchingHistorySearch): Observable<any> {

    return this.httpRequestService.postReqest(WebApiUrl + 'MatchingHistory' + WebApiSaffix + '/Get', matchingHistorySearch);
  }

  /**
   * 出力履歴の出力済フラグの追加
   * @param ids 
   */
  saveOutput(ids: number[]): Observable<any> {
    return this.httpRequestService.postReqest(WebApiUrl + 'MatchingHistory' + WebApiSaffix + '/SaveOutputAt', ids);
  }
}
