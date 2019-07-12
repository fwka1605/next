import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { UserInfoService } from '../service/common/user-info.service';
import { StringUtil } from '../common/util/string-util';
import { PageUtil } from '../common/util/page-util';
import { MenuOption } from '../component/common/COM0300/com0301-menu/menu';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private userService:UserInfoService,
    private router: Router,
    public activatedRoute: ActivatedRoute
  ){

  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {

    if(StringUtil.IsNullOrEmpty(this.userService.AccessToken)){
      this.router.navigate(['login']);
      return false;      
    }
    else{
      let hiddenMenuIds = MenuOption.getHiddenMenuIds(this.userService.ApplicationControl);
      const path: string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
      if (hiddenMenuIds.indexOf(path[0]) < 0) {
        return true;
      } else {
        this.router.navigate(['login']);
        return false;      
      }
    }

  }
}
