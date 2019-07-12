import { ApplicationControl } from "src/app/model/application-control.model";

export class Menu {
  private firstMenus: Array<FirstMenu>;

  public get FirstMenus(): Array<FirstMenu> {
    return this.firstMenus;
  }
  public set FirstMenus(value: Array<FirstMenu>) {
    this.firstMenus = value;
  }

  public constructor() {
    this.firstMenus = new Array<FirstMenu>();

    let index=0;

    this.firstMenus.push(new FirstMenu());
    this.firstMenus[index].Title = "請求";
    this.firstMenus[index].Icon = "icon_bill.svg";
    this.firstMenus[index].Visible = true;
    this.firstMenus[index].SecondItems = new Array<SecondItem>();
    this.firstMenus[index].SecondItems.push(new SecondItem('PC0101', '請求フリーインポーター', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PC0201', '請求データ入力', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PC0301', '請求データ検索', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PC0401', '請求書発行', false));
    this.firstMenus[index].SecondItems.push(new SecondItem('PC1401', '請求書再発行', false));
    this.firstMenus[index].SecondItems.push(new SecondItem('PC0501', '入金予定日変更', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PC0601', '請求仕訳出力', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PC0701', '口座振替依頼データ作成', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PC0801', '口座振替結果データ取込', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PC0901', '入金予定入力', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PC1001', '入金予定フリーインポーター', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PC1301', '働くDB 請求データ抽出', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PC1801', 'MFクラウド請求書 データ抽出', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PC1501', '請求書設定', true));

    this.firstMenus.push(new FirstMenu());
    ++index;
    this.firstMenus[index].Title = "入金";
    this.firstMenus[index].Icon = "icon_deposit.svg";
    this.firstMenus[index].Visible = true;
    this.firstMenus[index].SecondItems = new Array<SecondItem>();
    this.firstMenus[index].SecondItems.push(new SecondItem('PD0101', '入金EBデータ取込', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PD0201', '入金フリーインポーター', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PD0301', '入金データ入力', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PD0401', '入金データ振分', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PD0501', '入金データ検索', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PD0601', '前受一括計上処理', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PD0701', '入金仕訳出力', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PD0801', '入金部門振替処理', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PD1301', '入金データ自動連携 データ抽出', true));

    this.firstMenus.push(new FirstMenu());
    ++index;
    this.firstMenus[index].Title = "消込";
    this.firstMenus[index].Icon = "icon_removal.svg";
    this.firstMenus[index].Visible = true;
    this.firstMenus[index].SecondItems = new Array<SecondItem>();
    this.firstMenus[index].SecondItems.push(new SecondItem('PE0101', '一括消込', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PE0201', '消込仕訳出力', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PE0301', '消込履歴データ検索', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PE0401', '消込仕訳出力取消', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PE0501', '未消込請求データ削除', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PE0601', '未消込入金データ削除', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PE0701', '消込データ承認', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PE0801', '働くDB 消込結果連携', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PE0901', 'PCA会計DX 消込結果連携', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PE1001', 'MFクラウド会計 消込結果連携', true));

    this.firstMenus.push(new FirstMenu());
    ++index;
    this.firstMenus[index].Title = "管理帳票";
    this.firstMenus[index].Icon = "icon_book.svg";
    this.firstMenus[index].Visible = true;
    this.firstMenus[index].SecondItems = new Array<SecondItem>();
    this.firstMenus[index].SecondItems.push(new SecondItem('PF0101', '請求残高年齢表', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PF0201', '債権総額管理表', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PF0301', '入金予定明細表', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PF0401', '滞留明細一覧表', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PF0501', '得意先別消込台帳', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PF0601', '回収予定表', true));

    this.firstMenus.push(new FirstMenu());
    ++index;
    this.firstMenus[index].Title = "マスタ";
    this.firstMenus[index].Icon = "icon_master.svg";
    this.firstMenus[index].Visible = true;
    this.firstMenus[index].SecondItems = new Array<SecondItem>();
    this.firstMenus[index].SecondItems.push(new SecondItem('PB0501', '得意先マスター', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB0601', '債権代表者マスター', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB0201', '請求部門マスター', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB1101', '入金部門マスター', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB0401', '営業担当者マスター', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB0901', '区分マスター', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB0801', '銀行口座マスター', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB0701', '科目マスター', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB1201', '入金・請求部門対応マスター', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB1301', '入金部門・担当者対応マスター', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB0301', 'ログインユーザーマスター', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB1801', '銀行・支店マスター', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB1901', '決済代行会社マスター', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB1601', 'カレンダーマスター', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB2101', '通貨マスター', false));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB2201', '送付先マスター', false));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB2301', 'ステータスマスター', false));

    this.firstMenus.push(new FirstMenu());
    ++index;
    this.firstMenus[index].Title = "設定";
    this.firstMenus[index].Icon = "icon_setting.svg";
    this.firstMenus[index].Visible = true;
    this.firstMenus[index].SecondItems = new Array<SecondItem>();
    this.firstMenus[index].SecondItems.push(new SecondItem('PH1301', 'EBデータフォーマット一覧', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PH1001', '項目名称設定', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PH0701', 'グリッド表示設定', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PH0801', '照合ロジック設定', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB0101', '会社設定', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB1501', '除外カナ設定', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB1701', '法人格設定', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB1001', '学習履歴データ管理', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PB1401', '管理設定', true));

    this.firstMenus.push(new FirstMenu());
    ++index;
    this.firstMenus[index].Title = "メンテナンス";
    this.firstMenus[index].Icon = "icon_maintenance.svg";
    this.firstMenus[index].Visible = true;
    this.firstMenus[index].SecondItems = new Array<SecondItem>();
    this.firstMenus[index].SecondItems.push(new SecondItem('PH0101', '権限・セキュリティ', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PH0901', '操作ログ管理', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PH0201', '不要データ削除', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PH1501', '締め処理', false));
    
    this.firstMenus.push(new FirstMenu());
    ++index;
    this.firstMenus[index].Title = "連携設定";
    this.firstMenus[index].Icon = "icon_connection.svg";
    this.firstMenus[index].Visible = true;
    this.firstMenus[index].SecondItems = new Array<SecondItem>();
    this.firstMenus[index].SecondItems.push(new SecondItem('PH1401', 'MFクラウド請求書WebAPI連携設定', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PH1101', '働くDB WebAPI 連携設定', false));
    this.firstMenus[index].SecondItems.push(new SecondItem('PH1201', 'PCA会計DX WebAPI 連携設定', false));
    this.firstMenus[index].SecondItems.push(new SecondItem('PH1801', '入金明細連携 WebAPI 連携設定', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PH2001', '入金明細連携 データ検索', true));

    this.firstMenus.push(new FirstMenu());
    ++index;
    this.firstMenus[index].Title = "配信";
    this.firstMenus[index].Icon = "icon_maintenance.svg";
    this.firstMenus[index].Visible = true;
    this.firstMenus[index].SecondItems = new Array<SecondItem>();
    this.firstMenus[index].SecondItems.push(new SecondItem('PG0101', '回収通知メール配信', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PG0201', '回収遅延通知メール配信', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PG0301', 'メール設定', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PG0401', 'WebViewer公開処理', true));

    this.firstMenus.push(new FirstMenu());
    ++index;
    this.firstMenus[index].Title = "督促管理";
    this.firstMenus[index].Icon = "icon_maintenance.svg";
    this.firstMenus[index].Visible = true;
    this.firstMenus[index].SecondItems = new Array<SecondItem>();
    this.firstMenus[index].SecondItems.push(new SecondItem('PI0101', '督促データ確定', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PI0201', '督促データ管理', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PI0301', '督促管理帳票', true));
    this.firstMenus[index].SecondItems.push(new SecondItem('PI0401', '督促設定', true));
  }
}

export class FirstMenu {

  private title: string;
  private icon: string;
  private visible: boolean;
  private secondItems: Array<SecondItem>;

  public get Title(): string {
    return this.title;
  }
  public set Title(value: string) {
    this.title = value;
  }

  public get Icon(): string {
    return this.icon;
  }
  public set Icon(value: string) {
    this.icon = value;
  }

  public get Visible(): boolean {
    return this.visible;
  }
  public set Visible(value: boolean) {
    this.visible = value;
  }

  public get SecondItems(): Array<SecondItem> {
    return this.secondItems;
  }
  public set SecondItems(value: Array<SecondItem>) {
    this.secondItems = value;
  }

}


export class SecondItem {

  private path: string;

  private title: string;

  private visible: boolean;

  constructor(
    path: string,
    title: string,
    visible: boolean
  ) {
    this.path = path;
    this.title = title;
    this.visible = visible;
  }

  public get Path(): string {
    return this.path;
  }
  public set Path(value: string) {
    this.path = value;
  }

  public get Title(): string {
    return this.title;
  }
  public set Title(value: string) {
    this.title = value;
  }

  public get Visible(): boolean {
    return this.visible;
  }
  public set Visible(value: boolean) {
    this.visible = value;
  }
}

/** Menu に関する制御 */
export class MenuOption {

  /**
  * オプションによって非表示になる画面一覧を作成
  */
  static getHiddenMenuIds(appControl: ApplicationControl): Array<string> {
    let resultIds = new Array<string>();
    if (appControl.useAccountTransfer != 1) {
      resultIds.push('PB1901');
      resultIds.push('PC0701');
      resultIds.push('PC0801');
    }

    if (appControl.useAuthorization != 1) {
      resultIds.push('PE0701');
    }

    if (appControl.useClosing != 1) {
      resultIds.push('PH1501');
    }

    if (appControl.useDistribution != 1) {
      resultIds.push('PG0101');
      resultIds.push('PG0201');
      resultIds.push('PG0301');
      resultIds.push('PG0401');
    }

    if (appControl.useForeignCurrency != 1) {
      resultIds.push('PB2101');
      resultIds.push('PD1101');

    } else {
      resultIds.push('PC0401');
      resultIds.push('PC1401');
      resultIds.push('PC1501');
      resultIds.push('PF0601');
    }

    if (appControl.useHatarakuDBWebApi != 1) {
      resultIds.push('PC1301');
      resultIds.push('PE0801');
      resultIds.push('PH1101');
    }

    if (appControl.useLongTermAdvanceReceived != 1) {
      resultIds.push('PF0701');
    }

    if (appControl.useMFWebApi != 1) {
      resultIds.push('PC1801');
      resultIds.push('PE1001');
      resultIds.push('PH1401');
    }

    if (appControl.usePCADXWebApi != 1) {
      resultIds.push('PE0901');
      resultIds.push('PH1201');
    }

    if (appControl.usePublishInvoice != 1) {
      resultIds.push('PC0401');
      resultIds.push('PC1401');
      resultIds.push('PC1501');
    }

    if (appControl.useReceiptSection != 1) {
      resultIds.push('PB1101');
      resultIds.push('PB1201');
      resultIds.push('PB1301');
      resultIds.push('PD0801');
    }

    if (appControl.useReminder != 1) {
      resultIds.push('PB2301');
      resultIds.push('PI0101');
      resultIds.push('PI0201');
      resultIds.push('PI0301');
      resultIds.push('PI0401');
    }

    if (appControl.useScheduledPayment != 1) {
      resultIds.push('PC0901');
      resultIds.push('PC1001');
    }

    // 特殊
    if (appControl.usePublishInvoice != 1
      && appControl.useReminder != 1) {
      resultIds.push('PB2201');
    }

    return resultIds;
  }
}
