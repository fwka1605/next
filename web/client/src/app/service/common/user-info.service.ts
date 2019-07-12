import { Injectable } from '@angular/core';
import { Company } from 'src/app/model/company.model';
import { ApplicationControl } from 'src/app/model/application-control.model';
import { MenuAuthority } from 'src/app/model/menu-authority.model';
import { FunctionAuthority } from 'src/app/model/function-authority.model';
import { GeneralSetting } from 'src/app/model/general-setting.model';
import { Currency } from 'src/app/model/currency.model';
import { LoginUser } from 'src/app/model/login-user.model';
import { FunctionType } from 'src/app/common/const/kbn.const';

@Injectable({
  providedIn: 'root'
})
export class UserInfoService {

  constructor() {
    this.init();
  }

  public init() {
    this.accessToken = "";
    this.loginUser = null;
    this.company = null;
    this.applicationControl = null;
    this.menyuAuthority = null;
    this.functionAuthoritys = null;
    this.generalSettings = null;
    this.currency = null;
    this.tenantCode = null;
    this.userMenyuAuthoritys = null;
  }

  private loginUser: LoginUser = null;
  public get LoginUser(): LoginUser {
    return this.loginUser;
  }
  public set LoginUser(value: LoginUser) {
    this.loginUser = value;
  }


  private accessToken: string = "";
  public get AccessToken(): string {
    return this.accessToken;
  }
  public set AccessToken(value: string) {
    this.accessToken = value;
  }

  private company: Company;
  public get Company(): Company {
    return this.company;
  }
  public set Company(value: Company) {
    this.company = value;
  }

  private applicationControl: ApplicationControl;
  public get ApplicationControl(): ApplicationControl {
    return this.applicationControl;
  }
  public set ApplicationControl(value: ApplicationControl) {
    this.applicationControl = value;
  }

  private menyuAuthority: Array<MenuAuthority>;
  public get MenyuAuthority(): Array<MenuAuthority> {
    return this.menyuAuthority;
  }
  public set MenyuAuthority(value: Array<MenuAuthority>) {
    this.menyuAuthority = value;
  }

  private functionAuthoritys: Array<FunctionAuthority>;
  public get FunctionAuthoritys(): Array<FunctionAuthority> {
    return this.functionAuthoritys;
  }
  public set FunctionAuthoritys(value: Array<FunctionAuthority>) {
    this.functionAuthoritys = value;
  }

  private generalSettings: Array<GeneralSetting>;
  public get GeneralSettings(): Array<GeneralSetting> {
    return this.generalSettings;
  }
  public set GeneralSettings(value: Array<GeneralSetting>) {
    this.generalSettings = value;
  }

  private currency: Currency;
  public get Currency(): Currency {
    return this.currency;
  }
  public set Currency(value: Currency) {
    this.currency = value;
  }

  private tenantCode: string;
  public get TenantCode(): string {
    return this.tenantCode;
  }
  public set TenantCode(value: string) {
    this.tenantCode = value;
  }

  public isMenuAvailable(path: string): boolean {
    let result: boolean = false;
    if (this.userMenyuAuthoritys == null) {
      this.setUserMenyuAuthoritys();
    }
    let findIndex = this.userMenyuAuthoritys.findIndex((item) => {
      return (item.menuId.toUpperCase() === path.toUpperCase());
    });
    if (0 <= findIndex) {
      result = this.userMenyuAuthoritys[findIndex].available == 1 ? true : false;
    }
    return result;
  }

  private userMenyuAuthoritys: Array<MenuAuthority>;
  private setUserMenyuAuthoritys() {
    let userAuthoritys = new Array<MenuAuthority>();
    this.MenyuAuthority.forEach(menuAuthorityItem => {
      if (menuAuthorityItem.authorityLevel == this.loginUser.menuLevel) {
        userAuthoritys.push(menuAuthorityItem);
      }
    });
    this.userMenyuAuthoritys = userAuthoritys;
  }

  public isFunctionAvailable(functionType: FunctionType):boolean{
    let result: boolean = false;
    if (this.functionAuthoritys == null) return result; 

    if (this.functionAuthoritys.length != Object.keys(FunctionType).length) {
      this.setFunctionAuthoritys();
    }
    let findIndex = this.functionAuthoritys.findIndex((item) => {
      return (item.functionType === functionType);
    });
    if (0 <= findIndex) {
      result = this.functionAuthoritys[findIndex].available ? true: false;
    }
    return result;
  }

  private setFunctionAuthoritys() {
    let result = new Array<FunctionAuthority>();
    for (let i = 0; i < this.functionAuthoritys.length; i++) {
      if (this.functionAuthoritys[i].authorityLevel == this.loginUser.functionLevel) {
        result.push(this.functionAuthoritys[i]);
        if (result.length == Object.keys(FunctionType).length)  break;
      }
    }
    this.functionAuthoritys = result;
  }

}