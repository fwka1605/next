export class UserModel {

  private userId: string;
  private loginFlag: boolean;

  public get UserId(): string {
    return this.userId;
  }
  public get LoginFlag(): boolean {
    return this.loginFlag;
  }
  public set UserId(value: string) {
    this.userId = value;
  }
  public set LoginFlag(value: boolean) {
    this.loginFlag = value;
  }
}