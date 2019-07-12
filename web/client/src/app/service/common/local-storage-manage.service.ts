import { Injectable } from '@angular/core';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';

@Injectable({
  providedIn: 'root'
})

export class LocalStorageManageService {

  constructor() { }

    // データの取り出し
  public get(key:string): LocalStorageItem {
      return JSON.parse(localStorage.getItem(key));
  }

  // 削除
  public delete(key:string): void {
      localStorage.removeItem(key);
  }

  // 保存
  public set(localStorageItem: LocalStorageItem): void {
      localStorage.setItem(localStorageItem.key, JSON.stringify(localStorageItem));
  }

  // 全削除
  public clear(): void {
    localStorage.clear();
  }

}

