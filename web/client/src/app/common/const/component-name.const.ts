export enum ComponentId {
  NONE = 0,
  PB0101 = 1,
  PB0201,
  PB0301,
  PB0401,
  PB0501,
  PB0601,
  PB0701,
  PB0801,
  PB0901,
  PB1001,
  PB1101,
  PB1201,
  PB1301,
  PB1401,
  PB1501,
  PB1601,
  PB1701,
  PB1801,
  PB1901,
  PB2101,
  PB2201,
  PB2301,
  PC0101,
  PC0201,
  PC0301,
  PC0401,
  PC0501,
  PC0601,
  PC0701,
  PC0801,
  PC0901,
  PC1001,
  PC1301,
  PC1401,
  PC1501,
  PC1601,
  PC1701,
  PC1801,
  PD0101,
  PD0201,
  PD0301,
  PD0401,
  PD0501,
  PD0601,
  PD0701,
  PD0801,
  PD1101,
  PD1301,
  PE0101,
  PE0102,
  PE0201,
  PE0301,
  PE0401,
  PE0501,
  PE0601,
  PE0701,
  PE0801,
  PE0901,
  PE1001,
  PF0101, // 請求残高年齢表
  PF0201, // 債権総額管理表
  PF0301, // 入金予定明細表
  PF0401, // 滞留明細一覧表
  PF0501, // 得意先別消込台帳
  PF0601, // 回収予定表
  PF0701,
  PG0101,
  PG0201,
  PG0301,
  PG0401,
  PH0101,
  PH0201,
  PH0301,
  PH0401,
  PH0501,
  PH0601,
  PH0701,
  PH0801,
  PH0901,
  PH1001,
  PH1101,
  PH1201,
  PH1301,
  PH1401,
  PH1501,
  PH1801,
  PH1802,
  PH2001,
  PI0101,
  PI0201,
  PI0301,
  PI0401,

}

export const ComponentName = [
  "", // IDを１スタートにしたため
  "PB0101",
  "PB0201",
  "PB0301",
  "PB0401",
  "PB0501",
  "PB0601",
  "PB0701",
  "PB0801",
  "PB0901",
  "PB1001",
  "PB1101",
  "PB1201",
  "PB1301",
  "PB1401",
  "PB1501",
  "PB1601",
  "PB1701",
  "PB1801",
  "PB1901",
  "PB2101",
  "PB2201",
  "PB2301",
  "PC0101",
  "PC0201",
  "PC0301",
  "PC0401",
  "PC0501",
  "PC0601",
  "PC0701",
  "PC0801",
  "PC0901",
  "PC1001",
  "PC1301",
  "PC1401",
  "PC1501",
  "PC1601",
  "PC1701",
  "PC1801",
  "PD0101",
  "PD0201",
  "PD0301",
  "PD0401",
  "PD0501",
  "PD0601",
  "PD0701",
  "PD0801",
  "PD1101",
  "PD1301",
  "PE0101",
  "PE0201",
  "PE0301",
  "PE0401",
  "PE0501",
  "PE0601",
  "PE0701",
  "PE0801",
  "PE0901",
  "PE1001",
  "PF0101", // 請求残高年齢表
  "PF0201", // 債権総額管理表
  "PF0301", // 入金予定明細表
  "PF0401", // 滞留明細一覧表
  "PF0501", // 得意先別消込台帳
  "PF0601", // 回収予定表
  "PF0701",
  "PG0101",
  "PG0201",
  "PG0301",
  "PG0401",
  "PH0101",
  "PH0201",
  "PH0301",
  "PH0401",
  "PH0501",
  "PH0601",
  "PH0701",
  "PH0801",
  "PH0901",
  "PH1001",
  "PH1101",
  "PH1201",
  "PH1301",
  "PH1401",
  "PH1501",
  "PH1801",
  "PH1802",
  "PH2001",
  "PI0101",
  "PI0201",
  "PI0301",
  "PI0401",
]


export const COMPONENT_INFO = ({
  //////////////////////////////////////////////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////////
  PF0101: { id: 59, name: "PF0101"},
  PF0201: { id: 60, name: "PF0201"},
  PF0301: { id: 61, name: "PF0301"},
  PF0401: { id: 62, name: "PF0401"},
  PF0501: { id: 63, name: "PF0501"},
  PF0601: { id: 64, name: "PF0601"},
  //////////////////////////////////////////////////////////////////////////////////
});

/*
  PB0101: { id: ComponentId.PB0101, name: "PB0101", title: "会社マスター" },
  PB0201: { id: ComponentId.PB0201, name: "PB0201", title: "請求部門マスター" },
  PB0301: { id: ComponentId.PB0301, name: "PB0301", title: "ログインユーザーマスター" },
  PB0401: { id: ComponentId.PB0401, name: "PB0401", title: "営業担当者マスター" },
  PB0501: { id: ComponentId.PB0501, name: "PB0501", title: "得意先マスター" },
  PB0601: { id: ComponentId.PB0601, name: "PB0601", title: "債権代表者マスター" },
  PB0701: { id: ComponentId.PB0701, name: "PB0701", title: "科目マスター" },
  PB0801: { id: ComponentId.PB0801, name: "PB0801", title: "銀行口座マスター" },
  PB0901: { id: ComponentId.PB0901, name: "PB0901", title: "区分マスター" },
  PB1001: { id: ComponentId.PB1001, name: "PB1001", title: "学習履歴データ管理" },
  PB1401: { id: ComponentId.PB1401, name: "PB1401", title: "管理マスター" },
  PB1501: { id: ComponentId.PB1501, name: "PB1501", title: "除外カナマスター" },
  PB1601: { id: ComponentId.PB1601, name: "PB1601", title: "カレンダーマスター" },
  PB1701: { id: ComponentId.PB1701, name: "PB1701", title: "法人格マスター" },
  PB1801: { id: ComponentId.PB1801, name: "PB1801", title: "銀行・支店マスター" },
  PB1901: { id: ComponentId.PB1901, name: "PB1901", title: "決済代行会社マスター" },
  //////////////////////////////////////////////////////////////////////////////////
  PC0101: { id: ComponentId.PC0101, name: "PC0101", title: "請求フリーインポーター" },
  PC0201: { id: ComponentId.PC0201, name: "PC0201", title: "請求データ入力" },
  PC0301: { id: ComponentId.PC0301, name: "PC0301", title: "請求データ検索" },
  PC0501: { id: ComponentId.PC0501, name: "PC0501", title: "入金予定日変更" },
  PC0601: { id: ComponentId.PC0601, name: "PC0601", title: "請求仕訳出力" },
  PC0701: { id: ComponentId.PC0701, name: "PC0701", title: "口座振替依頼データ作成" },
  PC0801: { id: ComponentId.PC0801, name: "PC0801", title: "口座振替結果データ取込" },
  PC1801: { id: ComponentId.PC1801, name: "PC1801", title: "MFクラウド請求書 データ抽出" },
  //////////////////////////////////////////////////////////////////////////////////
  PD0101: { id: ComponentId.PD0101, name: "PD0101", title: "EBファイル取込" },
  //////////////////////////////////////////////////////////////////////////////////
  PE0101: { id: ComponentId.PE0101, name: "PE0101", title: "一括消込" },
  PE0102: { id: ComponentId.PE0102, name: "PE0102", title: "個別消込" },
  PE0201: { id: ComponentId.PE0201, name: "PE0201", title: "消込仕訳" },
  //////////////////////////////////////////////////////////////////////////////////
  PF0101: { id: ComponentId.PF0101, name: "PF0101", title: "請求残高年齢表" },
  PF0301: { id: ComponentId.PF0301, name: "PF0301", title: "入金予定明細表" },
  PF0401: { id: ComponentId.PF0401, name: "PF0401", title: "滞留明細一覧表" },
  PF0501: { id: ComponentId.PF0501, name: "PF0501", title: "得意先別消込台帳" },
  PF0601: { id: ComponentId.PF0601, name: "PF0601", title: "回収予定表" },
  //////////////////////////////////////////////////////////////////////////////////
  PH0101: { id: ComponentId.PH0101, name: "PH0101", title: "各種設定&セキュリティ" },
  PH1401: { id: ComponentId.PH1401, name: "PH1401", title: "MFクラウド請求書 Web API 連携設定" },
  */