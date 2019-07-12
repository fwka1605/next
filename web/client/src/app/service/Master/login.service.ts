import { Injectable } from '@angular/core';
import { HttpRequestService } from '../common/http-request.service';
import { Observable } from 'rxjs';
import { WebApiUrl } from 'src/app/common/const/http.const';
import { LoginParameters } from 'src/app/model/login-parameters.model';
import { WebApiLoginResult } from 'src/app/model/web-api-login-result.model';
import { ProcessResultCustom } from 'src/app/model/custom-model/process-result-custom.model';
import { PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { PasswordPolicy } from 'src/app/model/password-policy.model';
import { MSG_WNG, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { PasswordValidateResult } from 'src/app/common/const/matching.const';
import { StringUtil } from 'src/app/common/util/string-util';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  public fullSymbolChars = [
    '!', '#', '%', '+', '-', '*', '/', '$', '_', '~', '\\', ';', ':', '@', '&', '?', '^'
  ];

  public escapeSymbolChars = ['+', '*', '/', '\\', '?'];
  public passwordPolicy: PasswordPolicy = null;

  constructor(
    private httpRequestService: HttpRequestService,
  ) { }

  public Login(companyCode: string, userCode: string, password: string): Observable<any> {
    let loginParams = new LoginParameters();

    loginParams.companyCode = companyCode;
    loginParams.userCode = userCode;
    loginParams.password = password;

    return this.httpRequestService.postReqest(WebApiUrl + 'Login/Login', loginParams);
  }

  public LoginSynch(loginParams: LoginParameters) {
    let token: any;

    this.httpRequestService.postReqest(WebApiUrl + 'Login/Login', loginParams)
      .subscribe(
        (result: WebApiLoginResult) => {
          token = result.sessionKey;
          token.next(token);
        },
        error => {
          token = error;
          token.next(token);
        }
      );

    return token;
  }

  public ChangePassword(loginParams: LoginParameters): Observable<any> {
    return this.httpRequestService.postReqest(WebApiUrl + 'Login/ChangePassword', loginParams);
  }

  /**
   * テナントコードが正しいか確認する処理
   */
  validateTenantCode(): Observable<any> {
    return this.httpRequestService.postReqest(WebApiUrl + 'Login/ValidateTenantCode', undefined);
  }

  /**
   * パスワード条件確認
   * @param processCustomResult 処理結果
   * @param password 新しいパスワード
   */
  public checkPassword(processCustomResult: ProcessResultCustom, password: string = null) {
    if (StringUtil.IsNullOrEmpty(password)) return false;
    let result: boolean = false;
    let errorType: PasswordValidateResult = PasswordValidateResult.Valid;

    if (this.passwordPolicy != undefined && this.passwordPolicy != null) {
      // パスワード文字数
      if (password.length < this.passwordPolicy.minLength) {
        errorType = PasswordValidateResult.ShortagePasswordLength;
      }
      if (this.passwordPolicy.maxLength < password.length) {
        errorType = PasswordValidateResult.ExceedPasswordLength;
      }

      // パスワード文字種類
      if (errorType == PasswordValidateResult.Valid) {
        let pattern: RegExp = null;
        let patternResult;

        // アルファベット
        pattern = /[A-Za-z]/g;
        patternResult = password.match(pattern);
        if (patternResult == null) patternResult = [];
        if (this.passwordPolicy.useAlphabet == 1 && 0 < this.passwordPolicy.minAlphabetUseCount) {
          if (patternResult.length < this.passwordPolicy.minAlphabetUseCount) {
            errorType = PasswordValidateResult.ShortageAlphabetCharCount;
          }
        } else {
          if (0 < patternResult.length) {
            errorType = PasswordValidateResult.ProhibitionAlphabetChar;
          }
        }

        // 数値
        pattern = /[0-9]/g;
        patternResult = password.match(pattern);
        if (patternResult == null) patternResult = [];
        if (this.passwordPolicy.useNumber == 1 && 0 < this.passwordPolicy.minNumberUseCount) {
          if (patternResult.length < this.passwordPolicy.minNumberUseCount) {
            errorType = PasswordValidateResult.ShortageNumberCharCount;
          }
        } else {
          if (0 < patternResult.length) {
            errorType = PasswordValidateResult.ProhibitionNumberChar;
          }
        }

        // 記号
        let canUseSymbol: Array<string> = new Array<string>();
        let canNotUseSymbol: Array<string> = new Array<string>();

        this.fullSymbolChars.forEach(char => {
          let escapeChar = char;
          if (0 <= this.escapeSymbolChars.indexOf(char)) {
            escapeChar = '\\' + char;
          }
          if (this.passwordPolicy.symbolType.indexOf(char) < 0) {
            canNotUseSymbol.push(escapeChar);
          } else {
            canUseSymbol.push(escapeChar);
          }
        });

        // 記号使用
        pattern = new RegExp(canUseSymbol.join("|"), 'g');
        patternResult = password.match(pattern);
        if (patternResult == null) patternResult = [];
        if (patternResult.length < this.passwordPolicy.minSymbolUseCount 
          || patternResult.join("").length < this.passwordPolicy.minSymbolUseCount ) {
          errorType = PasswordValidateResult.ShortageSymbolCharCount;
        }

        if (0 < canNotUseSymbol.length) {
          pattern = new RegExp(canNotUseSymbol.join("|"), 'g');
          patternResult = password.match(pattern);
          if (patternResult != null) {
            if (0 < patternResult.join("").length) {
              if (this.passwordPolicy.useSymbol == 1) {
                errorType = PasswordValidateResult.ProhibitionNotAllowedSymbolChar;
              } else {
                errorType = PasswordValidateResult.ProhibitionSymbolChar;
              }
            }
          }
        }
      }

      // 同じ文字を連続して使用しない
      if (errorType == PasswordValidateResult.Valid && 0 < this.passwordPolicy.minSameCharacterRepeat) {
        let useCounter = 0;
        let target: string;
        for (let i = 1; i < password.length; i++) {
          target = password.charAt(i);
          if (target == password.charAt(i - 1)) {
            useCounter += 1;
            if (useCounter == this.passwordPolicy.minSameCharacterRepeat) {
              errorType = PasswordValidateResult.ExceedSameRepeatedChar;
              break;
            }
          } else {
            useCounter = 0;
          }
        }
      }
      // パスワード履歴の保存数
      // TODO:G4では処理がないため現在では保留

      // メッセージの設定
      if (errorType != PasswordValidateResult.Valid) {
        processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
        processCustomResult.message = 'パスワードの入力に';
        switch (errorType) {
          case PasswordValidateResult.ProhibitionAlphabetChar:
            processCustomResult.message += MSG_WNG.PROHIBITION_ALPHABET_CHAR;
            break;
          case PasswordValidateResult.ProhibitionNumberChar:
            processCustomResult.message += MSG_WNG.PROHIBITION_NUMBER_CHAR;
            break;
          case PasswordValidateResult.ProhibitionSymbolChar:
            processCustomResult.message += MSG_WNG.PROHIBITION_SYMBOL_CHAR;
            break;
          case PasswordValidateResult.ProhibitionNotAllowedSymbolChar:
            processCustomResult.message += MSG_WNG.PROHIBITION_NOT_ALLOWED_SYMBOL_CHAR;
            break;
          case PasswordValidateResult.ShortageAlphabetCharCount:
            processCustomResult.message
              += MSG_WNG.SHORTAGE_ALPHABET_CHAR_COUNT.replace(MSG_ITEM_NUM.FIRST, String(this.passwordPolicy.minAlphabetUseCount));
            break;
          case PasswordValidateResult.ShortageNumberCharCount:
            processCustomResult.message
              += MSG_WNG.SHORTAGE_NUMBER_CHAR_COUNT.replace(MSG_ITEM_NUM.FIRST, String(this.passwordPolicy.minNumberUseCount));
            break;
          case PasswordValidateResult.ShortageSymbolCharCount:
            processCustomResult.message
              += MSG_WNG.SHORTAGE_SYMBOL_CHAR_COUNT.replace(MSG_ITEM_NUM.FIRST, String(this.passwordPolicy.minSymbolUseCount));
            break;
          case PasswordValidateResult.ShortagePasswordLength:
            processCustomResult.message = '';
            processCustomResult.message
              += MSG_WNG.SHORTAGE_PASSWORD_LENGTH.replace(MSG_ITEM_NUM.FIRST, String(this.passwordPolicy.minLength));
            break;
          case PasswordValidateResult.ExceedPasswordLength:
            processCustomResult.message = '';
            processCustomResult.message
              += MSG_WNG.EXCEED_PASSWORD_LENGTH.replace(MSG_ITEM_NUM.FIRST, String(this.passwordPolicy.maxLength));
            break;
          case PasswordValidateResult.ExceedSameRepeatedChar:
            processCustomResult.message
              += MSG_WNG.EXCEED_SAME_REPEATED_CHAR.replace(MSG_ITEM_NUM.FIRST, String(this.passwordPolicy.caseSensitive));
            break;
        }
        result = true;
      }
    }
    return result;
  }

}
