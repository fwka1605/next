import { MfAggrTag } from "../mf-aggr-tag.model";

export class Tag
{
  public id:number;
  public name:string;

  public GetModel():MfAggrTag{
    let mfAggrTag = new MfAggrTag();
    mfAggrTag.id=this.id;
    mfAggrTag.name=this.name;

    return mfAggrTag;
 }
}