import { StringUtil } from "./string-util";
import { JuridicalPersonality } from "src/app/model/juridical-personality.model";

export class EbDataHelper {

  /**
   * ひらがな から カタカナ 変換表
   */
  private static hira2kanaPairs: [string, string][] = [
                            [ '\u3041', '\u30A1' ], [ '\u3042', '\u30A2' ], [ '\u3043', '\u30A3' ], [ '\u3044', '\u30A4' ], [ '\u3045', '\u30A5' ], [ '\u3046', '\u30A6' ], [ '\u3047', '\u30A7' ], [ '\u3048', '\u30A8' ], [ '\u3049', '\u30A9' ], [ '\u304A', '\u30AA' ], [ '\u304B', '\u30AB' ], [ '\u304C', '\u30AC' ], [ '\u304D', '\u30AD' ], [ '\u304E', '\u30AE' ], [ '\u304F', '\u30AF' ],
    [ '\u3050', '\u30B0' ], [ '\u3051', '\u30B1' ], [ '\u3052', '\u30B2' ], [ '\u3053', '\u30B3' ], [ '\u3054', '\u30B4' ], [ '\u3055', '\u30B5' ], [ '\u3056', '\u30B6' ], [ '\u3057', '\u30B7' ], [ '\u3058', '\u30B8' ], [ '\u3059', '\u30B9' ], [ '\u305A', '\u30BA' ], [ '\u305B', '\u30BB' ], [ '\u305C', '\u30BC' ], [ '\u305D', '\u30BD' ], [ '\u305E', '\u30BE' ], [ '\u305F', '\u30BF' ],
    [ '\u3060', '\u30C0' ], [ '\u3061', '\u30C1' ], [ '\u3062', '\u30C2' ], [ '\u3063', '\u30C3' ], [ '\u3064', '\u30C4' ], [ '\u3065', '\u30C5' ], [ '\u3066', '\u30C6' ], [ '\u3067', '\u30C7' ], [ '\u3068', '\u30C8' ], [ '\u3069', '\u30C9' ], [ '\u306A', '\u30CA' ], [ '\u306B', '\u30CB' ], [ '\u306C', '\u30CC' ], [ '\u306D', '\u30CD' ], [ '\u306E', '\u30CE' ], [ '\u306F', '\u30CF' ],
    [ '\u3070', '\u30D0' ], [ '\u3071', '\u30D1' ], [ '\u3072', '\u30D2' ], [ '\u3073', '\u30D3' ], [ '\u3074', '\u30D4' ], [ '\u3075', '\u30D5' ], [ '\u3076', '\u30D6' ], [ '\u3077', '\u30D7' ], [ '\u3078', '\u30D8' ], [ '\u3079', '\u30D9' ], [ '\u307A', '\u30DA' ], [ '\u307B', '\u30DB' ], [ '\u307C', '\u30DC' ], [ '\u307D', '\u30DD' ], [ '\u307E', '\u30DE' ], [ '\u307F', '\u30DF' ],
    [ '\u3080', '\u30E0' ], [ '\u3081', '\u30E1' ], [ '\u3082', '\u30E2' ], [ '\u3083', '\u30E3' ], [ '\u3084', '\u30E4' ], [ '\u3085', '\u30E5' ], [ '\u3086', '\u30E6' ], [ '\u3087', '\u30E7' ], [ '\u3088', '\u30E8' ], [ '\u3089', '\u30E9' ], [ '\u308A', '\u30EA' ], [ '\u308B', '\u30EB' ], [ '\u308C', '\u30EC' ], [ '\u308D', '\u30ED' ], [ '\u308E', '\u30EE' ], [ '\u308F', '\u30EF' ],
    [ '\u3090', '\u30F0' ], [ '\u3091', '\u30F1' ], [ '\u3092', '\u30F2' ], [ '\u3093', '\u30F3' ], [ '\u3094', '\u30F4' ], [ '\u3095', '\u30F5' ], [ '\u3096', '\u30F6' ], [ '\u3097', '\u30FB' ], [ '\u3098', '\u30FB' ]
  ];

  /**
   * カタカナ から 半角カナ 変換表
   */
  private static kana2halfPairs: [string, string][] = [
    [ '\u30A1', '\uFF67'       ],
    [ '\u30A2', '\uFF71'       ],
    [ '\u30A3', '\uFF68'       ],
    [ '\u30A4', '\uFF72'       ],
    [ '\u30A5', '\uFF69'       ],
    [ '\u30A6', '\uFF73'       ],
    [ '\u30A7', '\uFF6A'       ],
    [ '\u30A8', '\uFF74'       ],
    [ '\u30A9', '\uFF6B'       ],
    [ '\u30AA', '\uFF75'       ],
    [ '\u30AB', '\uFF76'       ],
    [ '\u30AC', '\uFF76\uFF9E' ],
    [ '\u30AD', '\uFF77'       ],
    [ '\u30AE', '\uFF77\uFF9E' ],
    [ '\u30AF', '\uFF78'       ],
    [ '\u30B0', '\uFF78\uFF9E' ],
    [ '\u30B1', '\uFF79'       ],
    [ '\u30B2', '\uFF79\uFF9E' ],
    [ '\u30B3', '\uFF7A'       ],
    [ '\u30B4', '\uFF7A\uFF9E' ],
    [ '\u30B5', '\uFF7B'       ],
    [ '\u30B6', '\uFF7B\uFF9E' ],
    [ '\u30B7', '\uFF7C'       ],
    [ '\u30B8', '\uFF7C\uFF9E' ],
    [ '\u30B9', '\uFF7D'       ],
    [ '\u30BA', '\uFF7D\uFF9E' ],
    [ '\u30BB', '\uFF7E'       ],
    [ '\u30BC', '\uFF7E\uFF9E' ],
    [ '\u30BD', '\uFF7F'       ],
    [ '\u30BE', '\uFF7F\uFF9E' ],
    [ '\u30BF', '\uFF80'       ],
    [ '\u30C0', '\uFF80\uFF9E' ],
    [ '\u30C1', '\uFF81'       ],
    [ '\u30C2', '\uFF81\uFF9E' ],
    [ '\u30C3', '\uFF6F'       ],
    [ '\u30C4', '\uFF82'       ],
    [ '\u30C5', '\uFF82\uFF9E' ],
    [ '\u30C6', '\uFF83'       ],
    [ '\u30C7', '\uFF83\uFF9E' ],
    [ '\u30C8', '\uFF84'       ],
    [ '\u30C9', '\uFF84\uFF9E' ],
    [ '\u30CA', '\uFF85'       ],
    [ '\u30CB', '\uFF86'       ],
    [ '\u30CC', '\uFF87'       ],
    [ '\u30CD', '\uFF88'       ],
    [ '\u30CE', '\uFF89'       ],
    [ '\u30CF', '\uFF8A'       ],
    [ '\u30D0', '\uFF8A\uFF9E' ],
    [ '\u30D1', '\uFF8A\uFF9F' ],
    [ '\u30D2', '\uFF8B'       ],
    [ '\u30D3', '\uFF8B\uFF9E' ],
    [ '\u30D4', '\uFF8B\uFF9F' ],
    [ '\u30D5', '\uFF8C'       ],
    [ '\u30D6', '\uFF8C\uFF9E' ],
    [ '\u30D7', '\uFF8C\uFF9F' ],
    [ '\u30D8', '\uFF8D'       ],
    [ '\u30D9', '\uFF8D\uFF9E' ],
    [ '\u30DA', '\uFF8D\uFF9F' ],
    [ '\u30DB', '\uFF8E'       ],
    [ '\u30DC', '\uFF8E\uFF9E' ],
    [ '\u30DD', '\uFF8E\uFF9F' ],
    [ '\u30DE', '\uFF8F'       ],
    [ '\u30DF', '\uFF90'       ],
    [ '\u30E0', '\uFF91'       ],
    [ '\u30E1', '\uFF92'       ],
    [ '\u30E2', '\uFF93'       ],
    [ '\u30E3', '\uFF6C'       ],
    [ '\u30E4', '\uFF94'       ],
    [ '\u30E5', '\uFF6D'       ],
    [ '\u30E6', '\uFF95'       ],
    [ '\u30E7', '\uFF6E'       ],
    [ '\u30E8', '\uFF96'       ],
    [ '\u30E9', '\uFF97'       ],
    [ '\u30EA', '\uFF98'       ],
    [ '\u30EB', '\uFF99'       ],
    [ '\u30EC', '\uFF9A'       ],
    [ '\u30ED', '\uFF9B'       ],
    [ '\u30EE', '\uFF9C'       ],
    [ '\u30EF', '\uFF9C'       ],
    [ '\u30F0', '\uFF72'       ],
    [ '\u30F1', '\uFF74'       ],
    [ '\u30F2', '\uFF66'       ],
    [ '\u30F3', '\uFF9D'       ],
    [ '\u30F4', '\uFF73\uFF9E' ],
    [ '\u30F5', '\uFF76'       ],
    [ '\u30F6', '\uFF79'       ],
    [ '\u30F7', '\uFF9C\uFF9E' ],
    [ '\u30F8', '\uFF72\uFF9E' ],
    [ '\u30F9', '\uFF74\uFF9E' ],
    [ '\u30FA', '\uFF75\uFF9E' ],
    [ '\u30FB', '\uFF65'       ],
    [ '\u30FC', '\uFF70'       ],
    [ '\u3001', '\uFF64'       ],
    [ '\u3002', '\uFF61'       ],
    [ '\u300C', '\uFF62'       ],
    [ '\u300D', '\uFF63'       ],
    [ '\u3099', '\uFF9E'       ],
    [ '\u309A', '\uFF9F'       ],
    [ '\u309B', '\uFF9E'       ],
    [ '\u309C', '\uFF9F'       ],
  ];

  /**
   * 全角 濁音・半濁音 記号の 半角変換表
   */
  private static voicedSoundMarkPairs: [string, string][] = [
    [ '\u3099', '\uFF9E' ],
    [ '\u309A', '\uFF9F' ],
    [ '\u309B', '\uFF9E' ],
    [ '\u309C', '\uFF9F' ],
  ];

  /** EB使用不可文字の 変換表 */
  private static prohibit2validPairs: [string, string][] = [
    [ '\uFF61', '\u002E' ], /* ｡ . */
    [ '\uFF65', '\u002E' ], /* ･ . */
    [ '\uFF70', '\u002D' ], /* ｰ - */
    [ '\uFF67', '\uFF71' ], /* ｧ ｱ */
    [ '\uFF68', '\uFF72' ], /* ｨ ｲ */
    [ '\uFF69', '\uFF73' ], /* ｩ ｳ */
    [ '\uFF6A', '\uFF74' ], /* ｪ ｴ */
    [ '\uFF6B', '\uFF75' ], /* ｫ ｵ */
    [ '\uFF6C', '\uFF94' ], /* ｬ ﾔ */
    [ '\uFF6D', '\uFF95' ], /* ｭ ﾕ */
    [ '\uFF6E', '\uFF96' ], /* ｮ ﾖ */
    [ '\uFF6F', '\uFF82' ], /* ｯ ﾂ */
  ];

  /**
   * 全角 濁音・半濁音を normalize すると 半角スペースが発生してしまうため、
   * 事前に 半角の 濁音・半濁音へと変換する処理
   * @param {string} value
   */
  private static convertVoicedSoundMark(value: string): string {

    value = StringUtil.ConvertEmpty(value);

    return value.split('').map(x => {
      const mark = this.voicedSoundMarkPairs.filter(y => y[0] === x)[0];
      if (mark !== undefined) {
        x = mark[1];
      }
      return x;
    }).join('');
  }

  /**
   * normalize 処理 全角英数の半角化などが実施される
   * @param {string} value
   */
  private static normalize(value: string): string {

    value = StringUtil.ConvertEmpty(value);

    return this.convertVoicedSoundMark(value).normalize('NFKC');
  }

  /**
   * ひらがな・カタカナを 半角カナへ変換
   * @param {string} value 
   * @returns {string} 全角英数→半角 漢字などは未変換のまま返す
   */
  public static convertToKanaHalf(value: string): string {

    value = StringUtil.ConvertEmpty(value);

    return this.normalize(value).split('').map(x => {
      const hira = this.hira2kanaPairs.filter(y => y[0] === x)[0];
      if (hira !== undefined) {
        x = hira[1];
      }
      const kana = this.kana2halfPairs.filter(y => y[0] === x)[0];
      if (kana !== undefined) {
        x = kana[1];
      }
      return x;
    }).join('');
  }

  /**
   * EB使用不可文字の変換処理
   * @param {string} value 
   * @returns {string} 半角カナの長音のハイフンへの変換、促音・発音などを変換
   * (e.g.) ｰ -> -, ｧ -> ｱ, ｬ -> ﾔ, ｯ -> ﾂ
   */
  public static convertProhibitCharacter(value: string): string {

    value = StringUtil.ConvertEmpty(value);

    return value.split('').map(x => {
      const pair = this.prohibit2validPairs.filter(y => y[0] === x)[0];
      if (pair !== undefined) {
        x = pair[1];
      }
      return x;
    }).join('');
  }

  /**
   * EB使用不可文字の削除処理
   * @param {string} value 
   * @returns {string} 変換不可の記号などを削除する
   */
  public static removeProhibitCharacter(value: string): string {

    value = StringUtil.ConvertEmpty(value);

    const pattern = /[\]\[､\:;<>&^%#\?@\$\|_'\+\*=!"]/g;
    return value.replace(pattern, '');
  }

  /**
   * EB使用可能文字への変換
   * @param {string} value 
   * @returns {string} ひらがな・カタカナの 半角カナへの変換
   * 英字の 小文字から大文字への変換
   * 使用不可文字で、変換可能な文字への変換
   * 使用不可文字の削除 を実施
   */
  public static convertToValidEbkana(value: string): string {

    value = StringUtil.ConvertEmpty(value);

    value = this.convertToKanaHalf(value);
    value = value.toUpperCase();
    value = this.convertProhibitCharacter(value);
    value = this.removeProhibitCharacter(value);
    return value;
  }

  /**
   * 法人格除去
   * @param {string} value 変換対象の文字列
   * @param {JuridicalPersonality[]} personalities 除去する法人格
   * @returns {string} 法人格を 長さ降順にソートし、(pattern, (pattern), pattern) の
   * 三パターンで除去を行う
   */
  public static removePersonalities(value: string, personalities: JuridicalPersonality[]): string {
    if ( StringUtil.IsNullOrEmpty(value) ||
      personalities == undefined ||
      personalities.length === 0 ) {
      return value;
    }

    let tmppersonalities: string[] = new Array();
    for (let personality of personalities) {
      tmppersonalities.push(personality.kana);
    }

    tmppersonalities.sort((x, y) => {
      if (x.length < y.length) {
        return 1;
      }
      else if (x.length > y.length) {
        return -1;
      }

      if (x < y) {
        return 1;
      }
      else if (x > y) {
        return -1;
      }
      return 0;
    });

    for (let tmppersonality of tmppersonalities) {
      for (let i = 0; i < 3; i++) {

        const pattern
          = i === 0 ? `\\(${tmppersonality}\\)`
          : i === 1 ? `\\(${tmppersonality}`
          : i === 2 ?   `${tmppersonality}\\)`
          : undefined;
        const re = new RegExp(pattern, 'g');
        value = value.replace(re, '');
      }
    }
    return value.trim();
  }
}