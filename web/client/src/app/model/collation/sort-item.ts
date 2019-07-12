export class SortItem{

  constructor(
    propertyName:string,
    sortOrder:SortOrder
  ){
    this.propertyName = propertyName;
    this.sortOrder = sortOrder;
  }
  public propertyName:string;
  public sortOrder:SortOrder;

}


export enum eSortType
{
    None = 0,
    CheckBox,
    AdvanceReceive_ASC,
    AdvanceReceive_DESC
}

//
// 概要:
//     リスト内の項目の並べ替え方法を指定します。
export enum SortOrder
{
  //
  // 概要:
  //     項目は並べ替えられません。
  None = 0,

  //
  // 概要:
  //     項目は昇順に並べ替えられます。
  Ascending = 1,
  //
  // 概要:
  //     項目は降順に並べ替えられます。
  Descending = 2
}