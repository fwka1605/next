import { WebApiUrl, WebApiSaffix } from "src/app/common/const/http.const";

export class ServiceInfo{

  public static getUrl(method: string,controller) : string {
    return `${WebApiUrl}${controller}${WebApiSaffix}/${method}`;
  }

}