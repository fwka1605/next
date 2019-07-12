import { ActivatedRouteSnapshot, UrlSegment } from "@angular/router";

import * as _ from 'lodash';

export class PageUtil{
  public static GetTitle(state, parent) {
    const data = [];
    if (parent && parent.snapshot.data && parent.snapshot.data.title) {
      data.push(parent.snapshot.data.title);
    }
    if (state && parent) {
      data.push(... this.GetTitle(state, state.firstChild(parent)));
    }
    return data;
  }

  public static GetComponentId(state, parent) {
    const data = [];
    if (parent && parent.snapshot.data && parent.snapshot.data.id) {
      data.push(parent.snapshot.data.id);
    }
    if (state && parent) {
      data.push(... this.GetComponentId(state, state.firstChild(parent)));
    }
    return data;
  }  

  public static GetPath(root):string[] {
    return _.map(root, (ars: ActivatedRouteSnapshot) => {
      if (_.isNil(ars.url) || _.isEmpty(ars.url)) {
       return null;
      }
      return (_.first(ars.url) as UrlSegment).path as string;
     }).filter(val => val);
  }

}