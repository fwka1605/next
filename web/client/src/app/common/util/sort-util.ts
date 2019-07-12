import { SortItem, SortOrder } from "src/app/model/collation/sort-item";

export  class SortUtil{

  // ソート条件によるソートの実行
  public static Sort(sortItems:Array<SortItem>,sortData:Array<any>){

    let ascending=SortOrder.Ascending;
    let none=SortOrder.None;

    sortItems.forEach(element=>{
      element.propertyName=element.propertyName.substring(0, 1).toLowerCase() + element.propertyName.substring(1);
    });


    sortData = 
    sortData.sort(function(col1,col2){
      for(let index=0;index<sortItems.length;index++){
        if(sortItems[index].sortOrder==SortOrder.Ascending||sortItems[index].sortOrder==SortOrder.None){
          if(col1[sortItems[index].propertyName]< col2[sortItems[index].propertyName] ) return -1
          if(col1[sortItems[index].propertyName]> col2[sortItems[index].propertyName] ) return 1
        }
        else{
          if(col1[sortItems[index].propertyName]> col2[sortItems[index].propertyName] ) return -1
          if(col1[sortItems[index].propertyName]< col2[sortItems[index].propertyName] ) return 1
        }
      }
    });
    
    return sortData;
  }

}
