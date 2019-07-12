export const FILE_FIELD_TYPE_NAMES = [
  { id: 1, val: "カンマ区切り" },
  { id: 2, val: "タブ区切り" },
  { id: 3, val: "固定長 （改行あり）" },
  { id: 4, val: "固定長 （改行なし）" },
];

export const FILE_EXTENSION =
{
  CSV: ".csv",
  PDF: ".pdf",
  LOG: ".log",
  XLSX: ".xlsx"
};

export const ENCODE = [
  { id: 65001, val: "UTF-8" },
  { id: 932, val: "Shift_JIS" }
]


export enum ImportResult
{
    None = 0,
    FileNotFound,
    FileReadError,
    FileFormatError,
    BankAccountMasterError,
    ImportDataNotFound,
    DBError,
    BankAccountFormatError,
    PayerCodeFormatError,
    Success,
}

//ファイルサイズの上限 単位：Byte
export const MAX_FILE_SIZE = 10485760;  //10MB