import { Injectable } from '@angular/core';
import { BUTTON_ACTION, OPERATION_NAME } from 'src/app/common/const/event.const';
import { LogDataService } from '../log-data.service';
import { ProcessResultCustom, ExistDeleteResult } from 'src/app/model/custom-model/process-result-custom.model';
import { PROCESS_RESULT_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { MSG_ERR, MSG_WNG, MSG_INF, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { LogData } from 'src/app/model/log-data.model';
import { DateUtil } from 'src/app/common/util/date-util';
import { StringUtil } from 'src/app/common/util/string-util';
import { PartsResultMessageComponent } from 'src/app/component/view-parts/parts-result-message/parts-result-message.component';


const outputGetDataLog: Array<String> = [
  BUTTON_ACTION.SEARCH,
  BUTTON_ACTION.SEARCH_BILLING,
  BUTTON_ACTION.SEARCH_RECEIPT
];

@Injectable({
  providedIn: 'root'
})
export class ProcessResultService {

  constructor(
    private logDataService: LogDataService
  ) { }

  /**
   * ProcessResultCustom の初期化
   * @param title 画面名
   */
  public processResultInit(id: number, title: string = null, isModal: boolean = false) {
    let processCustomResult = new ProcessResultCustom(isModal);
    processCustomResult.logData = new LogData();
    if (StringUtil.IsNullOrEmpty(title)) {
      processCustomResult.title = title;
    } else {
      processCustomResult.title = title;
    }

    processCustomResult.logData.menuId = id;
    processCustomResult.logData.menuName = title;
    processCustomResult.logData.firstLoggedAt = DateUtil.getYYYYMMDD(1);
    processCustomResult.logData.logCount = 0;

    processCustomResult.logData.operationName = null;
    return processCustomResult;
  }

  /**
   * 処理開始時
   * @param action 処理名（ボタン名）
   */
  public processResultStart(processResult: ProcessResultCustom, action: BUTTON_ACTION) {
    if (processResult == undefined || processResult == null) {
      processResult = new ProcessResultCustom();
      processResult.result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
      processResult.message = MSG_ERR.PROGRAM_START_ERROR;
      return processResult;
    }

    processResult.action = action;
    processResult.status = PROCESS_RESULT_STATUS_TYPE.RUNNING;
    processResult.result = null;
    processResult.message = null;

    // ログデータ
    let name = OPERATION_NAME.get(action);
    processResult.logData.operationName = StringUtil.IsNullOrEmpty(name) ? "未定" : name;
    processResult.logData.firstLoggedAt = DateUtil.getYYYYMMDD(1);

    return processResult;
  }

  /**
   * データ取得・検索時の結果設定
   * 
   * @param processResult 処理結果
   * @param result DBの処理結果
   * @param isSearch 検索処理の判定[true:検索 false:取得]
   */
  public processAtGetData(
    processResult: ProcessResultCustom,
    result: any,
    isSearch: boolean,
    partsResultMessageComponent: PartsResultMessageComponent): ProcessResultCustom {

    if (partsResultMessageComponent != null) { partsResultMessageComponent.openMessage() };

    processResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;

    if (result == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
      processResult.result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
      processResult.message = MSG_ERR.DATA_SEARCH;

    } else {
      if (result.length == 0) {
        processResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
        if (isSearch) {
          processResult.message = MSG_WNG.NOT_EXIST_SEARCH_DATA;
        } else {
          processResult.message = MSG_WNG.NO_DATA.replace(MSG_ITEM_NUM.FIRST, '登録されている');
        }

      } else {
        processResult.result = PROCESS_RESULT_RESULT_TYPE.SUCCESS;
        if (isSearch) {
          processResult.message = MSG_INF.PROCESS_FINISH;
        }
      }
    }
    if (processResult.logData != undefined && 0 <= outputGetDataLog.indexOf(processResult.action)) {
      this.createdLog(processResult.logData);
    }
    return processResult;
  }

  /**
   * データ登録・更新時の結果設定
   * @param processResult 処理結果
   * @param result DBの処理結果
   * @param isRegistry 登録処理の判定[true:登録 false:更新]
   */
  public processAtSave(
    processResult: ProcessResultCustom,
    result: any,
    isRegistry: boolean,
    partsResultMessageComponent: PartsResultMessageComponent) {

    if (partsResultMessageComponent != null) { partsResultMessageComponent.openMessage() };

    processResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
    if (result == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
      processResult.result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
      processResult.message = isRegistry ? MSG_ERR.SAVE_ERROR : MSG_ERR.UPDATE_ERROR;
    }
    else {
      processResult.result = PROCESS_RESULT_RESULT_TYPE.SUCCESS;
      processResult.message = isRegistry ? MSG_INF.SAVE_SUCCESS : MSG_INF.UPDATE_SUCCESS;
      // TODO:パスワード更新の際にログが登録できないためIf文で除外しているが解決次第if文削除
      if (processResult.logData.menuId != 1) {
        this.createdLog(processResult.logData);
      }
    }
    return processResult;
  }

  /**
   * IDの登録情報確認の結果設定
   * @param processResult 処理結果
   * @param responseList DBの処理結果
   * @param existDeleteResult 検索をしたマスター情報
   */
  public processAtExist(
    processResult: ProcessResultCustom,
    responseList: Array<any>,
    existDeleteResult: Array<ExistDeleteResult>,
    partsResultMessageComponent: PartsResultMessageComponent) {

    if (partsResultMessageComponent != null) { partsResultMessageComponent.openMessage() };

    if (responseList.length != existDeleteResult.length || responseList.indexOf(undefined) >= 0) {
      processResult.result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
      processResult.message = MSG_ERR.DATA_SEARCH;

      return processResult;
    }
    let listIndex = responseList.indexOf(true);
    let message: string;
    if (listIndex >= 0) {
      processResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
      message = MSG_WNG.DELETE_CONSTRAINT.replace(MSG_ITEM_NUM.FIRST, existDeleteResult[listIndex].masterName);
      message = message.replace(MSG_ITEM_NUM.SECOND, existDeleteResult[listIndex].idName);
      processResult.message = message;
    }

    return processResult;
  }

  /**
   * データ削除時の結果設定
   * @param processResult 処理結果
   * @param result DBの処理結果
   */
  public processAtDelete(
    processResult: ProcessResultCustom,
    result: any,
    partsResultMessageComponent: PartsResultMessageComponent) {

    if (partsResultMessageComponent != null) { partsResultMessageComponent.openMessage() };

    processResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;

    if (0 < result) {
      processResult.result = PROCESS_RESULT_RESULT_TYPE.SUCCESS;
      processResult.message = MSG_INF.DELETE_SUCCESS;
      this.createdLog(processResult.logData);

    } else {
      processResult.result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
      processResult.message = MSG_ERR.DELETE_ERROR;
    }
    return processResult;
  }

  /**
   * データエクスポート・印刷時の結果設定
   * @param processResult 処理結果
   * @param result DLの成功有無
   * @param outputType 出力するもの[0:export 1:print]
   */
  public processAtOutput(
    processResult: ProcessResultCustom,
    result: boolean,
    outputType: number,
    partsResultMessageComponent: PartsResultMessageComponent) {

    if (partsResultMessageComponent != null) { partsResultMessageComponent.openMessage() };

    processResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;

    if (!result) {
      processResult.result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
      if (outputType == 0) {
        processResult.message = MSG_ERR.EXPORT_ERROR;
      } else {
        processResult.message = MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, "印刷")
      }

    } else {
      processResult.result = PROCESS_RESULT_RESULT_TYPE.SUCCESS;
      if (outputType == 0) {
        processResult.message = MSG_INF.FINISH_EXPORT;
      } else {
        processResult.message = MSG_INF.FINISH_PRINT;
      }
      this.createdLog(processResult.logData);
    }
    return processResult;
  }


  ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

  /**
   * ワーニング
   * @param processResult 処理結果
   * @param message エラーメッセージ
   */
  public processAtWarning(
    processResult: ProcessResultCustom,
    message: string,
    partsResultMessageComponent: PartsResultMessageComponent) {

    if (partsResultMessageComponent != null) { partsResultMessageComponent.openMessage() };

    processResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
    processResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
    processResult.message = message;
    return processResult;
  }

  /**
   * 失敗
   * @param processResult 処理結果
   * @param message エラーメッセージ
   */
  public processAtFailure(
    processResult: ProcessResultCustom,
    message: string,
    partsResultMessageComponent: PartsResultMessageComponent) {

    if (partsResultMessageComponent != null) { partsResultMessageComponent.openMessage() };

    processResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
    processResult.result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
    processResult.message = message;
    return processResult;
  }

  /**
   * 成功
   * @param processResult 処理結果
   * @param message エラーメッセージ
   */
  public processAtSuccess(
    processResult: ProcessResultCustom,
    message: string,
    partsResultMessageComponent: PartsResultMessageComponent) {

    if (partsResultMessageComponent != null) { partsResultMessageComponent.openMessage() };

    processResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
    processResult.result = PROCESS_RESULT_RESULT_TYPE.SUCCESS;
    processResult.message = message;
    return processResult;
  }

  /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  /**
   * ログ作成
   */
  public createdLog(logData: LogData) {
    this.logDataService.log(logData)
      .subscribe(result => {
        if (!result || result == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          console.log(MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, "ログデータの登録"));
        }
      });
  }

  /**
   * 処理メッセージクリア
   */
  public clearProcessCustomMsg(processResult: ProcessResultCustom) {
    processResult.result = null;
    processResult.message = null;
    return processResult;
  }
}
