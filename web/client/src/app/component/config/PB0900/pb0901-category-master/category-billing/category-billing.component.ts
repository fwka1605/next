import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import { Category } from 'src/app/model/category.model';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { UserInfoService } from 'src/app/service/common/user-info.service';

@Component({
  selector: 'app-category-billing',
  templateUrl: './category-billing.component.html',
  styleUrls: ['./category-billing.component.css']
})
export class CategoryBillingComponent extends BaseComponent {
  
  /** カテゴリデータ一覧 */
  @Input() categoriesResult: CategoriesResult;
  /** 選択した行のデータ  */
  @Output() event = new EventEmitter<Category>();

  constructor(
    public userInfoService: UserInfoService
  ){
    super();
  }

  /**
   * 選択した行のデータ取得
   * @param index 行番号
   */
  public selectLine(index: number) {
    this.event.emit(this.categoriesResult.categories[index]);
  }
}
