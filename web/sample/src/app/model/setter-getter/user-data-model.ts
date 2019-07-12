export class UserModel{

    private _name1:string;

    private name2:string;

    public json:string;

    /**
     * Getter name1
     * @return {string}
     */
	public get name1(): string {
		return this._name1;
	}

    /**
     * Getter Name2
     * @return {string}
     */
	public get Name2(): string {
		return this.name2;
	}

    /**
     * Setter name1
     * @param {string} value
     */
	public set name1(value: string) {
		this._name1 = value;
	}

    /**
     * Setter Name2
     * @param {string} value
     */
	public set Name2(value: string) {
		this.name2 = value;
	}


}