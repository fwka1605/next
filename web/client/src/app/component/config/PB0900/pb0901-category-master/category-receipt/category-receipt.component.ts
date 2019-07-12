import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import { Category } from 'src/app/model/category.model';
import { TAX_CLASS_TYPE_DICTIONARY, USE_LIMIT_DATE_DICTIONARY } from 'src/app/common/const/kbn.const';
import { BaseComponent } from 'src/app/component/common/base/base-component';

@Component({
  selector: 'app-category-receipt',
  templateUrl: './category-receipt.component.html',
  styleUrls: ['./category-receipt.component.css']
})
export class CategoryReceiptComponent extends BaseComponent {

  /** カテゴリデータ一覧 */
  @Input() categoriesResult: CategoriesResult;
  /** 選択した行のデータ  */
  @Output() event = new EventEmitter<Category>();

  /** 期日入力 */
  public readonly useLimitDateDictionary = USE_LIMIT_DATE_DICTIONARY;

  /**
   * 選択した行のデータ取得
   * @param index 行番号
   */
  public selectLine(index: number) {
    this.event.emit(this.categoriesResult.categories[index]);
  }

}
