export enum COMPONENT_STATUS_TYPE {
  CREATE = 0,
  UPDATE = 1,
  REFERE = 2,
}

export enum COMPONENT_DETAIL_STATUS_TYPE {
  CREATE = 0,
  UPDATE = 1,
  REFERE = 2,
}



export const COMPONENT_STATUS_NAME = ["登録","更新","参照"];

export enum MODAL_STATUS_TYPE {
  CLOSE = 0,
  SELECT = 1,
  OK =2,
  NO =3,
  CANCEL=4,
}

export const MODAL_STATUS_NAME = ["閉じる","選択","実行","キャンセル"];

export enum PROCESS_RESULT_STATUS_TYPE {
  RUNNING = 0,
  DONE = 1
}

export enum PROCESS_RESULT_RESULT_TYPE {
  FAILURE = -1,
  SUCCESS = 0,
  WARNING = 1
}