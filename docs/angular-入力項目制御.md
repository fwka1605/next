〇入力項目の制御

１．ログイン時にApplicationControl/MenuAuthority/FunctionAuthorityを取得し、
    UserInfoServiceのインスタンス変数に設定。

２．各コンポーネントのコンストラクタで、UserInfoServiceインスタンスを挿入。

３．各テンプレートで調整する。

〇修正前
              <dt class="liner-dl-wrap__container__title">入金部門</dt>
              <dd class="liner-dl-wrap__container__detail">
                <input 
                  formControlName="sectionCodeFromCtrl" 
                  name="sectionCodeFromCtrl" 
                  (blur)="setSectionCodeFrom($event.type)" 
                  (keyup.enter)="setSectionCodeFrom($event.type)" 
                  (keyup)="openMasterModal(TABLE_INDEX.MASTER_SECTION,$event.keyCode,'from')"
                  type="tel"
                  maxlength="10"
                  class="input-size--110">
                <span>&nbsp;&nbsp;</span>
                <input 
                  formControlName="sectionNameFromCtrl" 
                  class="input-size--250" 
                  readonly>
              </dd>
              <dt class="liner-dl-wrap__container__title--right">
                <span class="end-checkbox">
                  <input formControlName="cbxSectionCtrl" type="checkbox">
                </span>
              </dt>
              <dd class="liner-dl-wrap__container__detail">
                <input 
                  formControlName="sectionCodeToCtrl" 
                  name="sectionCodeToCtrl" 
                  (blur)="setSectionCodeTo($event.type)" 
                  (keyup.enter)="setSectionCodeTo($event.type)" 
                  (keyup)="openMasterModal(TABLE_INDEX.MASTER_SECTION,$event.keyCode,'to')"
                  type="tel"
                  maxlength="10"
                  class="input-size--110">
                <span>&nbsp;&nbsp;</span>
                <input 
                  formControlName="sectionNameToCtrl" 
                  class="input-size--250" 
                  readonly>
              </dd>

〇修正内容
１、制御が必要な項目をng-containerタグで囲む。
２．ng-containerタグに表示制御用の条件を記載する。
３．非表示の場合にデザインが崩れる場合は、ng-containerタグにelseを追加する。
４．ng-containerタグにelseで指定されるテンプレートを作成する。

＜例＞ pd0501-receipt-searchコンポーネントの入金部門
userInfoService.ApplicationControl?.useReceiptSection==1;else elseSectionCodeContentの記載は、
useReceiptSectionが1の場合は配下を表示し、それ以外はelseSectionCodeContentのng-templateを表示する。
デザインが崩れる箇所はelseを指定するが、それ以外は指定が不要になる。


            <ng-container *ngIf="userInfoService.ApplicationControl?.useReceiptSection==1;else elseContent">
              <dt class="liner-dl-wrap__container__title">入金部門</dt>
              <dd class="liner-dl-wrap__container__detail">
                <input 
                  formControlName="sectionCodeFromCtrl" 
                  name="sectionCodeFromCtrl" 
                  (blur)="setSectionCodeFrom($event.type)" 
                  (keyup.enter)="setSectionCodeFrom($event.type)" 
                  (keyup)="openMasterModal(TABLE_INDEX.MASTER_SECTION,$event.keyCode,'from')"
                  type="tel"
                  maxlength="10"
                  class="input-size--110">
                <span>&nbsp;&nbsp;</span>
                <input 
                  formControlName="sectionNameFromCtrl" 
                  class="input-size--250" 
                  readonly>
              </dd>
              <dt class="liner-dl-wrap__container__title--right">
                <span class="end-checkbox">
                  <input formControlName="cbxSectionCtrl" type="checkbox">
                </span>
              </dt>
              <dd class="liner-dl-wrap__container__detail">
                <input 
                  formControlName="sectionCodeToCtrl" 
                  name="sectionCodeToCtrl" 
                  (blur)="setSectionCodeTo($event.type)" 
                  (keyup.enter)="setSectionCodeTo($event.type)" 
                  (keyup)="openMasterModal(TABLE_INDEX.MASTER_SECTION,$event.keyCode,'to')"
                  type="tel"
                  maxlength="10"
                  class="input-size--110">
                <span>&nbsp;&nbsp;</span>
                <input 
                  formControlName="sectionNameToCtrl" 
                  class="input-size--250" 
                  readonly>
              </dd>
            </ng-container>
            <ng-template #elseContent>
              <dt class="liner-dl-wrap__container__title"></dt>
              <dd class="liner-dl-wrap__container__detail">
              </dd>
              <dt class="liner-dl-wrap__container__title--right">
              </dt>
              <dd class="liner-dl-wrap__container__detail">
              </dd>
            </ng-template>