import { serializable, deserialize } from './serializable'

@serializable
export class Staff {

  private id: number;
  private name: string;

  constructor(id: number, name: string) {
    this.id = id;
    this.name = name;
    console.log(`Created! id:${id} name:${name}`);
  }

  public get Id(): number {
    return this.id;
  }
  public set Id(value: number) {
    this.id = value;
  }

  public get Name(): string {
    return this.name;
  }
  public set Name(value: string) {
    this.name = value;
  }

  // JSONからの復元方法を指定
  static fromJSON(json: { id: number, name: string }): Staff {
    return new Staff(json.id, json.name);
  }

  static fromJSONS(jsons: [{ id: number, name: string }]): Staff[] {
    let staffs: Staff[] = new Array(jsons.length);

    let index = 0;
    jsons.forEach(json => {
      staffs[index] = new Staff(json.id, json.name);
      index++;
    });

    return staffs;
  }

}