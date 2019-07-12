using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Message
{
    /// <summary>
    ///  メッセージ定数一覧
    /// </summary>
    public class Constants
    {
        #region お知らせ( I00000, MsgInf)

        /// <summary>インポートが完了しました。一部データの取込に失敗しました。詳細はログを参照してください。</summary>
        public const string MsgInfImportCompletePartOfError = "I00010";
        /// <summary>処理が完了しました。</summary>
        public const string MsgInfProcessFinish = "I00030";
        /// <summary>登録が完了しました。</summary>
        public const string MsgInfSaveSuccess = "I00060";
        /// <summary>更新が完了しました。</summary>
        public const string MsgInfUpdateSuccess = "I00070";
        /// <summary>削除が完了しました。</summary>
        public const string MsgInfDeleteSuccess = "I00080";
        /// <summary>一括消込が完了しました。</summary>
        public const string MsgInfSequentialMatchingFinish = "I00090";
        /// <summary>消込が完了しました。</summary>
        public const string MsgInfMatchingProcessFinish = "I00100";
        /// <summary>取込が完了しました。</summary>
        public const string MsgInfEBFileImportFinish = "I00110";
        /// <summary>エクスポートが完了しました。</summary>
        public const string MsgInfFinishExport = "I00120";
        /// <summary>インポートが完了しました。</summary>
        public const string MsgInfFinishImport = "I00130";
        /// <summary>新規の{0}コードです。</summary>
        public const string MsgInfSaveNewData = "I00140";
        /// <summary>新規のパターン№です。</summary>
        public const string MsgInfNewPatternNo = "I00150";
        /// <summary>{0}をキャンセルしました。</summary>
        public const string MsgInfCancelProcess = "I00160";
        /// <summary>不要データの削除が完了しました。</summary>
        public const string MsgInfDeleteUnnecessaryData = "I00170";
        /// <summary>ＤＢの最適化が完了しました。</summary>
        public const string MsgInfFinishDbOptimisation = "I00180";
        /// <summary>ＤＢのバックアップが完了しました。</summary>
        public const string MsgInfFinishDbBackup = "I00190";
        /// <summary>ＤＢのリストアが完了しました。{0}</summary>
        public const string MsgInfFinishRestoreDb = "I00200";
        /// <summary>残高の繰越が完了しました。</summary>
        public const string MsgInfFinishBalanceCarryForward = "I00230";
        /// <summary>残高の戻しが完了しました。</summary>
        public const string MsgInfReturnCarryForwardBalance = "I00240";
        /// <summary>{0}の出力が完了しました。</summary>
        public const string MsgInfOutputUnreadData = "I00260";
        /// <summary>新規の{0}です。</summary>
        public const string MsgInfNewData = "I00290";
        /// <summary>処理を中断しました。</summary>
        public const string MsgInfProcessCanceled = "I00300";
        /// <summary>データを抽出しました。</summary>
        public const string MsgInfDataExtracted = "I00310";
        /// <summary>抽出したデータの印刷が、正常に完了しました。</summary>
        public const string MsgInfPrintExtractedData = "I00320";
        /// <summary>抽出したデータの出力が、正常に完了しました。</summary>
        public const string MsgInfFinishDataExtracting = "I00330";
        /// <summary>選択したデータの再印刷が、正常に完了しました。</summary>
        public const string MsgInfReprintedSelectData = "I00340";
        /// <summary>選択したデータの再出力が、正常に完了しました。</summary>
        public const string MsgInfFinishedSuccessReOutputProcess = "I00350";
        /// <summary>選択した仕訳データの取消が、正常に完了しました。</summary>
        public const string MsgInfFinishedSuccessJournalizingCancelingProcess = "I00360";
        /// <summary>承認が完了しました。</summary>
        public const string MsgInfApprovalSuccess = "I00410";
        /// <summary>承認が解除されました。</summary>
        public const string MsgInfCancelApprovalSuccess = "I00420";
        /// <summary>解除が完了しました。</summary>
        public const string MsgInfFinishCancelation = "I00440";
        /// <summary>読込が完了しました。</summary>
        public const string MsgInfFinishFileReadingProcess = "I00480";
        /// <summary>パスワードの初期化が完了しました。</summary>
        public const string MsgInfFinishResetPassword = "I00500";
        /// <summary>金額一致する組み合わせが見つかりました。</summary>
        public const string MsgInfNotFoundMatchingAmt = "I00510";
        /// <summary>復元が完了しました。</summary>
        public const string MsgInfFinishReturnBalanceProcess = "I00520";
        /// <summary>転送が完了しました。</summary>
        public const string MsgInfFinishDataTransfer = "I00530";
        /// <summary>一括消込が完了しました。\n表示情報更新のため、再照合処理を行います。</summary>
        public const string MsgInfFinishMatchingSeqAndConfirmCollate = "I00540";
        /// <summary>表示情報更新のため、再照合処理を行います。</summary>
        public const string MsgInfConfirmCollate = "I00550";
        /// <summary>消込解除が完了しました。\n表示情報更新のため、再検索を行います。</summary>
        public const string MsgInfFinishCancelAndConfrimGetMatched = "I00560";
        /// <summary>表示情報更新のため、再検索を行います。</summary>
        public const string MsgInfConfirmGetMatched = "I00570";
        /// <summary>テスト接続が成功しました。</summary>
        public const string MsgInfSuccessTestConnection = "I00580";
        /// <summary>{0}は集計必須項目です。</summary>
        public const string MsgInfNeedForSummarized = "I00590";

        #endregion

        #region 確認( Q00000, MsgQst)

        /// <summary>終了してもよろしいですか？</summary>
        public const string MsgQstConfirmApplicationExit = "Q00010";
        /// <summary>登録してもよろしいですか？</summary>
        public const string MsgQstConfirmSave = "Q00020";
        /// <summary>{0}更新してもよろしいですか？</summary>
        public const string MsgQstConfirmUpdate = "Q00030";
        /// <summary>削除してもよろしいですか？</summary>
        public const string MsgQstConfirmDelete = "Q00050";
        /// <summary>一括消込を開始してもよろしいですか？</summary>
        public const string MsgQstConfirmStartSeqMatching = "Q00060";
        /// <summary>エクスポートを開始してもよろしいですか？</summary>
        public const string MsgQstConfirmExport = "Q00090";
        /// <summary>ＤＢをバックアップしてもよろしいですか？</summary>
        public const string MsgQstConfirmDBBackup = "Q00160";
        /// <summary>ＤＢをリストアしてもよろしいですか？</summary>
        public const string MsgQstConfirmDBRestore = "Q00170";
        /// <summary>{0}を送信してもよろしいですか？</summary>
        public const string MsgQstConfirmSendMail = "Q00210";
        /// <summary>未消込の{0}を削除してもよろしいですか？</summary>
        public const string MsgQstConfirmDelBillReceiptOmitData = "Q00220";
        /// <summary>削除済みの{0}を復元してもよろしいですか？</summary>
        public const string MsgQstConfirmRestoreDeletedData = "Q00230";
        /// <summary>繰越処理を開始してもよろしいですか？</summary>
        public const string MsgQstConfirmBalanceCarryForward = "Q00260";
        /// <summary>現在の繰越残高を削除し、前回更新前の繰越残高に戻してもよろしいですか？</summary>
        public const string MsgQstConfirmBalanceCarryBackward = "Q00270";
        /// <summary>ファイルが存在しています。上書きしてもよろしいですか？</summary>
        public const string MsgQstConfirmFileOverWrite = "Q00290";
        /// <summary>編集中のデータがありますが、画面をクリアしますか？</summary>
        public const string MsgQstConfirmClear = "Q00300";
        /// <summary>編集中のデータがありますが、終了しますか？</summary>
        public const string MsgQstConfirmClose = "Q00310";
        /// <summary>編集中のデータが破棄されますが、処理を続行しますか？</summary>
        public const string MsgQstConfirmUpdateData = "Q00320";
        /// <summary>{0}を削除してもよろしいですか？</summary>
        public const string MsgQstConfirmDeleteXXX = "Q00340";
        /// <summary>編集中のデータがありますが、
        ///ただちに登録し、処理を続行しますか？</summary>
        public const string MsgQstConfirmHasEditedRegistAndContinue = "Q00360";
        /// <summary>{0}を開始してもよろしいですか？</summary>
        public const string MsgQstConfirmStartXXX = "Q00390";
        /// <summary>承認済{0}件、未承認{1}件です。出力しますか？</summary>
        public const string MsgQstConfirmPrintForApprovalCount = "Q00410";
        /// <summary>編集中のデータがありますが、更新して{0}しますか？</summary>
        public const string MsgQstConfirmHasUpdateData = "Q00430";
        /// <summary>選択中の{0}を追加して更新しますか？</summary>
        public const string MsgQstConfirmUpdateSelectData = "Q00460";
        /// <summary>解除してもよろしいですか？</summary>
        public const string MsgQstConfirmCancel = "Q00490";
        /// <summary>承認してもよろしいですか？</summary>
        public const string MsgQstConfirmApprove = "Q00500";
        /// <summary>選択した仕訳の取消をおこないますか？</summary>
        public const string MsgQstConfirmCancelJournalizing = "Q00520";
        /// <summary>抽出したデータの出力をおこないますか？</summary>
        public const string MsgQstConfirmOutputExtractData = "Q00550";
        /// <summary>選択確定してもよろしいですか？</summary>
        public const string MsgQstConfirmSelection= "Q00590";
        /// <summary>読込してもよろしいですか？</summary>
        public const string MsgQstConfirmReading = "Q00600";
        /// <summary>パスワードを初期化してもよろしいですか？</summary>
        public const string MsgQstConfirmResetPassword = "Q00620";
        /// <summary>
        /// インポートを開始してもよろしいですか？
        /// 方法：上書
        /// マスターを削除し、インポートファイルの内容に置き換えます。
        /// </summary>
        public const string MsgQstImportOverWriteExistsData = "Q00630";
        /// <summary>
        /// インポートを開始してもよろしいですか？
        /// 方法：追加
        /// マスターに無いものだけ、インポートファイルから追加します。
        /// </summary>
        public const string MsgQstImportAddNewDataOnly = "Q00640";
        /// <summary>
        /// インポートを開始してもよろしいですか？
        /// 方法：更新
        /// マスターに無いものは追加、あるものは上書します。
        /// </summary>
        public const string MsgQstImportUpdateExistsAndAddNewData = "Q00650";
        /// <summary>得意先コード：{0}は債権代表者グループに登録されていません。
        /// {1}の債権代表者グループに登録して消込を行いますか？
        /// </summary>
        public const string MsgQstConfirmMatchingWithRegisterCustomerGroup = "Q00660";
        /// <summary>日付順・金額順にシミュレートを開始してもよろしいですか？
        ///（組み合わせが多い場合は時間がかかります）</summary>
        public const string MsgQstConfirmMatchingSimulation = "Q00670";
        /// <summary>一括削除を行いますか？</summary>
        public const string MsgQstConfirmDeleteAll = "Q00680";
        /// <summary>編集中の{0}データがある場合、すべてクリアされます。
        /// 登録してもよろしいですか？</summary>
        public const string MsgQstConfirmClearEditedXXXData = "Q00690";
        /// <summary>グリッド設定を初期値に戻してもよろしいですか？</summary>
        public const string MsgQstConfirmResetDefaultGridSetting = "Q00700";
        /// <summary>
        /// 振替済チェックリストを出力します。よろしいですか？
        /// （一度出力したデータ、消込済のデータは対象外となります）
        /// </summary>
        public const string MsgQstPrintReceiptSectionTransferData = "Q00710";
        /// <summary>取消してもよろしいですか？</summary>
        public const string MsgQstConfirmForCancel = "Q00720";
        /// <summary>選択したファイルを削除してもよろしいですか？</summary>
        public const string MsgQstConfirmDeleteSpecificFile = "Q00730";
        /// <summary>指定したファイルを転送してもよろしいですか？</summary>
        public const string MsgQstConfirmTransferForSpecificFile = "Q00740";
        /// <summary>数量×単価＝金額（抜）となっていませんが、続行しますか？</summary>
        public const string MsgQstContinueUnmatchCalculation = "Q00750";
        /// <summary>ログアウトしてもよろしいですか？</summary>
        public const string MsgQstConfirmLogout = "Q00760";
        /// <summary>編集中のデータがありますが、再読込しますか？</summary>
        public const string MsgQstConfirmReloadData = "Q00770";
        /// <summary>
        /// 請求書番号の自動採番で現採番ルールが採用されています。
        ///設定を変更すると連番がリセットされますが、よろしいですか？
        /// </summary>
        public const string MsgQstConfirmResetInvoiceNumberSetting = "Q00780";
        /// <summary>上書が行われますが、よろしいでしょうか？</summary>
        public const string MsgQstConfirmDoOverWrite = "Q00790";
        /// <summary>出力を開始してもよろしいですか？</summary>
        public const string MsgQstConfirmOutput = "Q00800";
        #endregion

        #region 警告( W00000, MsgWng)

        /// <summary>該当データは見つかりません。</summary>
        public const string MsgWngNotExistSearchData = "W00010";
        /// <summary>パスワードが正しくありません。</summary>
        public const string MsgWngInvalidPassword = "W00120";
        /// <summary>印刷対象のデータがありません。</summary>
        public const string MsgWngPrintDataNotExist = "W00130";
        /// <summary>ライセンスキーが入力されていません。</summary>
        public const string MsgWngNoLicenseKey = "W00140";
        /// <summary>ライセンスキーが正しくありません。</summary>
        public const string MsgWngUnjustLicenseKey = "W00150";
        /// <summary>ライセンスキーが重複しています。</summary>
        public const string MsgWngLicenseKeyIsRepeated = "W00160";
        /// <summary>{0}を入力して下さい。</summary>
        public const string MsgWngInputRequired = "W00170";
        /// <summary>{0}を選択して下さい。</summary>
        public const string MsgWngSelectionRequired = "W00180";
        /// <summary>最低１つは選択して下さい。</summary>
        public const string MsgWngSelectAtleastOne = "W00190";
        /// <summary>すでに他のデータが登録されているので削除できません。</summary>
        public const string MsgWngRegistedDataAndCannotDelete = "W00200";
        /// <summary>{0}が設定されていません。</summary>
        public const string MsgWngNotSettingMaster = "W00210";
        /// <summary>削除するデータがありません。</summary>
        public const string MsgWngNoDeleteData = "W00220";
        /// <summary>ログインしている担当者を削除することはできません。</summary>
        public const string MsgWngCannotDelCurrentLoginUser = "W00230";
        /// <summary>{0}に不正な文字があります。</summary>
        public const string MsgWngInputInvalidLetter = "W00240";
        /// <summary>{0}の範囲指定が誤っています。</summary>
        public const string MsgWngInputRangeChecked = "W00250";
        /// <summary>{0}が正しくありません。</summary>
        public const string MsgWngInvalidInputValue = "W00270";
        /// <summary>指定されたファイルが存在しません。</summary>
        public const string MsgWngOpenFileNotFound = "W00280";
        /// <summary>{0}を指定して下さい。</summary>
        public const string MsgWngNotExistUpdateData = "W00290";
        /// <summary>指定されたフォルダが存在しません。</summary>
        public const string MsgWngNotExistFolder = "W00300";
        /// <summary>入力された{0}コード「{1}」は{0}マスターに登録されていません。</summary>
        public const string MsgWngMasterNotExist = "W00320";
        /// <summary>入力された得意先「{0}」は債権代表者として設定されていません。</summary>
        public const string MsgWngNotParentCustomer = "W00340";
        /// <summary>入力されたパターンNo.「{0}」は登録されていません。</summary>
        public const string MsgWngNotRegistPatternNo = "W00360";
        /// <summary>入力された得意先「{0}」は債権代表者として設定されています。</summary>
        public const string MsgWngAlreadyParentCustomer = "W00390";
        /// <summary>入力された請求ID「{0}」に該当する請求データはありません。</summary>
        public const string MsgWngInvalidBillingNoForBilling = "W00410";
        /// <summary>明細を最低１行は入力してください。</summary>
        public const string MsgWngInputGridRequired = "W00440";
        /// <summary>{0}は０以外の値を入力してください。</summary>
        public const string MsgWngInputExceptZeroAmt = "W00450";
        /// <summary>入力された{1}に該当するデータが{0}マスターに存在しません。</summary>
        public const string MsgWngInputDataNotExistsAtMaster = "W00470";
        /// <summary>入力された入金ID「{0}」に該当する入金データはありません。</summary>
        public const string MsgWngInvalidReceiptIdForReceipt = "W00510";
        /// <summary>入力された入金区分「{0}」では更新できません。</summary>
        public const string MsgWngInvalidCategoryForUpdate = "W00570";
        /// <summary>ＤＢは他のアプリケーションで使用中です。{0}の実行はできません。</summary>
        public const string MsgWngOtherAppliUsingDBandCannotBkOrRestore = "W00590";
        /// <summary>プロダクトキーが不正です。大文字の英数字を七文字入力してください。</summary>
        public const string MsgWngProductKeyNeed7Character = "W00600";
        /// <summary>消込処理を行う{0}が選択されていません。</summary>
        public const string MsgWngNoSelectDataForMatching = "W00610";
        /// <summary>すでに他の端末で消込処理されている{0}データが含まれています。</summary>
        public const string MsgWngIncludeOtherUserMatchedData = "W00640";
        /// <summary>選択されたデータは{0}のデータです。{1}できません。</summary>
        public const string MsgWngSelectDataNotEditable = "W00650";
        /// <summary>{0}に登録されている{1}なので削除できません。</summary>
        public const string MsgWngDeleteConstraint = "W00660";
        /// <summary>エクスポートするデータがありません。</summary>
        public const string MsgWngNoExportData = "W00700";
        /// <summary>インポートするデータがありません。</summary>
        public const string MsgWngNoImportData = "W00710";
        /// <summary>指定された取込ファイルには入金データがありません。</summary>
        public const string MsgWngNoReceiptData = "W00740";
        /// <summary>取込ファイル内に銀行口座マスターに存在しないデータがあります。</summary>
        public const string MsgWngNotExistBankAccountMaster = "W00750";
        /// <summary>{0}は繰越年月より大きい値を入力してください。</summary>
        public const string MsgWngRequiredOverCarryOverDate = "W00760";
        /// <summary>{0}は{1}より大きい値を入力してください。</summary>
        public const string MsgWngInputValidDateForParameters = "W00770";
        /// <summary>入金残が増加する消込は行なえません。</summary>
        public const string MsgWngNotAllowedReceiptRemainIncrease = "W00820";
        /// <summary>請求残が増加する消込は行なえません。</summary>
        public const string MsgWngNotAllowedBillingRemainIncrease = "W00830";
        /// <summary>消込対象額が0円の場合は左端のチェックを外してください。</summary>
        public const string MsgWngNotAllowdTargetAmountIsZero = "W00850";
        /// <summary>{0}は{1}より後の日付を入力してください。</summary>
        public const string MsgWngInputParam1DateAfterParam2Date = "W00860";
        /// <summary>{0}は{1}より前の日付を入力してください。</summary>
        public const string MsgWngInputParam1DateBeforeParam2Date = "W00870";
        /// <summary>{0}は{1}の範囲で入力してください。</summary>
        public const string MsgWngInputRangeViolation = "W00950";
        /// <summary>請求残がありません。消込む請求データを選択してください。</summary>
        public const string MsgWngSelectBillingForMatchingAmount = "W00960";
        /// <summary>入金日は照合条件で指定した範囲で入力してください。</summary>
        public const string MsgWngInputDateBeforeIndividualMatchingRecordedAt = "W00970";
        /// <summary>{0}データがありません。</summary>
        public const string MsgWngNoData = "W00990";
        /// <summary>{0}選択中の為、照会画面の額と明細の請求残計は一致しません。</summary>
        public const string MsgWngBillingRemainNotMatched = "W01010";
        /// <summary>パスワードは、{0}文字以上です。</summary>
        public const string MsgWngShortagePasswordLength = "W01040";
        /// <summary>パスワードは、{0}文字以下です。</summary>
        public const string MsgWngExceedPasswordLength = "W01050";
        /// <summary>アルファベットを最低{0}文字入れてください。</summary>
        public const string MsgWngShortageAlphabetCharCount = "W01070";
        /// <summary>アルファベットは使用しないでください。</summary>
        public const string MsgWngProhibitionAlphabetChar = "W01080";
        /// <summary>数字を最低{0}文字入れてください。</summary>
        public const string MsgWngShortageNumberCharCount = "W01090";
        /// <summary>数字は使用しないでください。</summary>
        public const string MsgWngProhibitionNumberChar = "W01100";
        /// <summary>記号を最低{0}文字入れてください。</summary>
        public const string MsgWngShortageSymbolCharCount = "W01110";
        /// <summary>記号は使用しないでください。</summary>
        public const string MsgWngProhibitionSymbolChar = "W01120";
        /// <summary>同じ文字を{0}文字以上続けないでください。</summary>
        public const string MsgWngExceedSameRepeatedChar = "W01130";
        /// <summary>履歴に残っているパスワードは無効です。</summary>
        public const string MsgWngProhibitionSamePassword = "W01140";
        /// <summary>パスワードの有効期限が過ぎています。パスワードを変更してください。</summary>
        public const string MsgWngExpiredPaswordAndChangeNewOne = "W01150";
        /// <summary>使用できない文字が含まれています。</summary>
        public const string MsgWngProhibitionNotAllowedSymbolChar = "W01160";
        /// <summary>得意先コード:{0}は別の債権代表者グループに所属しています。</summary>
        public const string MsgWngOtherChildCustomer = "W01200";
        /// <summary>入力された請求部門コード:{0}は、すでに別の入金部門に登録されています。</summary>
        public const string MsgWngAlreadyRegSection = "W01220";
        /// <summary>入金部門を振り分けられない入金データが存在します。</summary>
        public const string MsgWngNotApportionedSection = "W01240";
        /// <summary>{0}するデータがありません。</summary>
        public const string MsgWngNoDataForProcess = "W01250";
        /// <summary>請求データ集計をする場合は、伝票集計を合計にできません。</summary>
        public const string MsgWngNotAllowedSheetSummaryWhenBillingSummary = "W01280";
        /// <summary>請求部門表示をしない時は、部門改ページありにできません。</summary>
        public const string MsgWngNotAllowedDeptPageBreakeWhenNotDisplay = "W01290";
        /// <summary>得意先「{0}」の回収条件は約定ではありません。</summary>
        public const string MsgWngNotContractCollectCategoryCustomer = "W01300";
        /// <summary>契約番号が登録されていません。</summary>
        public const string MsgWngNotRegistedContractNo = "W01310";
        /// <summary>端数処理をいずれか1つに設定してください。</summary>
        public const string MsgWngSelectAtleastOneRounding = "W01320";
        /// <summary>同じ入金部門には振替できません。</summary>
        public const string MsgWngCannotTransferSameReceiptSection = "W01330";
        /// <summary>取込対象となる口座振替結果データがありません。</summary>
        public const string MsgWngNoDataAccountTransfer = "W01360";
        /// <summary>既に登録されています。</summary>
        public const string MsgWngAlreadyExistedData = "W01370";
        /// <summary>全ての明細行が埋まっています。</summary>
        public const string MsgWngAllDetailLinesFilled = "W01390";
        /// <summary>出力する項目を最低一つ以上選択してください。</summary>
        public const string MsgWngSelectAnyOutputItem = "W01400";
        /// <summary>抽出したデータと更新する金額が一致しません。処理を中断します。</summary>
        public const string MsgWngNotEqualAbstractAmountAndUpdateAmount = "W01410";
        /// <summary>権限がないため、修正出来ません。</summary>
        public const string MsgWngPermissionDeniedForEdit = "W01430";
        /// <summary>{0}は既に登録されています。</summary>
        public const string MsgWngAlreadyRegistData = "W01500";
        /// <summary>長期前受契約マスターの初期値が設定されていません。</summary>
        public const string MsgWngNoDefaultSettingAtBillingDivisionContract = "W01530";
        /// <summary>{0}の固定値が入力されていません。</summary>
        public const string MsgWngNoInputFixedValue = "W01540";
        /// <summary>銀行コード、支店コード、仮想支店コード、仮想口座番号は全て同時に登録する必要があります。</summary>
        public const string MsgWngBankInfoIncomplete = "W01560";
        /// <summary>仮想支店コード、仮想口座番号は二つ同時に登録する必要があります。</summary>
        public const string MsgWngPayerCodeIncomplete = "W01570";
        /// <summary>仮想支店コード、仮想口座番号が重複しています。</summary>
        public const string MsgWngPayerCodeDuplicate = "W01580";
        /// <summary>管理者は最低１人は必要です。</summary>
        public const string MsgWngAdminIsNecessary = "W01610";
        /// <summary>書込ファイル名の長さが無効です。</summary>
        public const string MsgWngInvalidSaveFileNameLength = "W01710";
        /// <summary>書込ファイルに使用不可な文字が含まれています。</summary>
        public const string MsgWngInvalidCharacterAtWriteFile = "W01720";
        /// <summary>取込フォルダと同じパスは指定できません。</summary>
        public const string MsgWngCannotSetSameImportFolder = "W01730";
        /// <summary>登録済の取込フォルダパスと同じパスは指定できません。</summary>
        public const string MsgWngCannotSetAlreadyRegistedSameImportFolder = "W01740";
        /// <summary>登録済のフォルダパスと同じパスは指定できません。</summary>
        public const string MsgWngCannotSetAlreadyRegisPath = "W01750";
        /// <summary>消込済のデータは{0}できません。</summary>
        public const string MsgWngMatchedDataCannotUpdateOrDelete = "W01770";
        /// <summary>期日入金予定のデータは{0}できません。</summary>
        public const string MsgWngPaymentScheduledDataCannotUpdateOrDelete = "W01780";
        /// <summary>削除済のデータは{0}できません。</summary>
        public const string MsgWngDeletedDataCannotUpdateOrDelete = "W01790";
        /// <summary>確定フラグがあるデータは{0}できません。</summary>
        public const string MsgWngBillDivContractedDataCannotUpdateOrDelete = "W01800";
        /// <summary>前受データは{0}できません。</summary>
        public const string MsgWngAdvancedReceiptDataCannotxxx = "W01810";
        /// <summary>対象外金額の合計が入金額を超えることはできません。</summary>
        public const string MsgWngExcludeAmtOverReceipt = "W01870";
        /// <summary>未消込額が(+)の場合は、対象外額も(+)で入力してください。</summary>
        public const string MsgWngPlusSignPair = "W01900";
        /// <summary>未消込額が(-)の場合は、対象外額も(-)で入力してください。</summary>
        public const string MsgWngMinusSignPair = "W01910";
        /// <summary>{0}は01-27もしくは99で入力してください。</summary>
        public const string MsgWngNumberValueValid1To27or99 = "W01950";
        /// <summary>入金予定日は前1桁は月数、後2桁は01-27もしくは99で入力してください。</summary>
        public const string MsgWngInvalidValueOfReceiptDueDateFormat = "W01960";
        /// <summary>入力された得意先「{0}」は債権グループに登録されていません。</summary>
        public const string MsgWngInputCustomerNotExistsAtCustomerGroup = "W01970";
        /// <summary>既に他の端末で更新されているデータです。更新できません。</summary>
        public const string MsgWngAlreadyUpdated = "W01990";
        /// <summary>照合ロジック順序設定の使用を最低１個はチェックしてください。</summary>
        public const string MsgWngCheckAtleastOneCollationLogic = "W02020";
        /// <summary>ログイン担当者の数がライセンス数を超えています。
        ///          弊社営業までご連絡下さい。</summary>
        public const string MsgWngLicenseIsMax = "W02030";
        /// <summary>担当者ライセンスに不正な値があります。
        ///          弊社営業までご連絡下さい。</summary>
        public const string MsgWngLicenseIsUnjust = "W02040";
        /// <summary>V-ONE利用、WebViewer利用のどちらかにはチェックを付ける必要があります。</summary>
        public const string MsgWngSelectVOneOrWebViewer = "W02080";
        /// <summary>歩引率の合計が100%を超えるので登録できません。</summary>
        public const string MsgWngNoRegisOver100PerDiscount = "W02090";
        /// <summary>削除する相殺データがありません。</summary>
        public const string MsgWngNoDeleteNettingData = "W02100";
        /// <summary>得意先を振り分けられないデータがあります。</summary>
        public const string MsgWngNotApportionedCustomer = "W02110";
        /// <summary>組み合わせパターンが多すぎます。設定し直してください。</summary>
        public const string MsgWngMuchPatternAndNeedReset = "W02120";
        /// <summary>一致する組み合わせが見つかりません。</summary>
        public const string MsgWngCannotFoundPairPattern = "W02130";
        /// <summary>プラス・マイナスが混在する場合は、合計額が一致している必要があります。</summary>
        public const string MsgWngDifferValueSignNeedSameAmount = "W02140";
        /// <summary>債権代表者グループに所属しない得意先コードです。</summary>
        public const string MsgWngNotContainsCustomerGroup = "W02150";
        /// <summary>手数料誤差金額は小数点{0}桁までの入力としてください。</summary>
        public const string MsgWngInvalidFeeToleranceScale = "W02160";
        /// <summary>口座振替依頼済み、または引落済みのため、{0}できません。</summary>
        public const string MsgWngAccountTransferredOrWithdrawnDataCannotUpdateOrDelete = "W02170";
        /// <summary>{0}は{1}文字以内で入力してください。</summary>
        public const string MsgWngPara1InputPara2CharCount = "W02180";
        /// <summary>請求残と異なる符号の予定額は入力できません。</summary>
        public const string MsgWngInvalidValueSignForRemainAmount = "W02190";
        /// <summary>{0}は0-9の範囲で入力してください。</summary>
        public const string MsgWngInputableRangeIs0to9 = "W02200";
        /// <summary>請求額を超える歩引額の入力はできません。</summary>
        public const string MsgWngDiscountValueViolation = "W02210";
        /// <summary>仕訳出力済のデータは{0}できません。</summary>
        public const string MsgWngJournalizedCannotUpdateAndDelete = "W02220";
        /// <summary>有効桁数以下の長さのデータを入力してください。</summary>
        public const string MsgWngScaleViolation = "W02230";
        /// <summary>指定したファイル「{0}」は既に存在します。</summary>
        public const string MsgWngSpecifiedFileAlreadyExists = "W02240";
        /// <summary>指定したファイル「{0}」が存在しません。</summary>
        public const string MsgWngSpecifiedFileNotFound = "W02250";
        /// <summary>未消込ではないため、振替解除できません。</summary>
        public const string MsgWngNotClearDataOfTransferCannotCancel = "W02260";
        /// <summary>他の振替履歴が存在するため、振替解除できません。</summary>
        public const string MsgWngTransferToOtherSectionAndCannotCancel = "W02270";
        /// <summary>以下のデータが削除済みのため、消込解除できません。
        /// 消込解除前に、削除済みデータの復元を行ってください。
        /// {0}</summary>
        public const string MsgWngCancelMatchingError = "W02280";
        /// <summary>以下の請求データから生成された
        /// 期日入金予定データが消込済みのため、消込解除できません。
        /// 生成元の消込解除前に、生成されたデータの消込解除を行ってください。
        /// {0}</summary>
        public const string MsgWngCancelMatchingErrorCashOnDueDate = "W02290";
        /// <summary>パスワードが設定されていません。パスワード変更を行ってください。</summary>
        public const string MsgWngRequirePasswordChange = "W02300";
        /// <summary>作成するファイル名が長すぎます。
        /// 短いファイル名を使って再実行するか、または短いパスのフォルダーにファイルを作成してください。</summary>
        public const string MsgWngPathTooLong = "W02310";
        /// <summary>ログインしている会社を削除することはできません。</summary>
        public const string MsgWngCannotDeleteLoginCompany = "W02320";
        /// <summary>元入金の入金残が残っているため、仕訳済の前受は編集できません。</summary>
        public const string MsgWngOriginalReceiptRemainAmountNotZero = "W02330";
        /// <summary>元入金が削除されているため、編集できません。</summary>
        public const string MsgWngOriginalReceiptAlreadyDeleted = "W02340";
        /// <summary>変更後の前受金額の合計が変更前の前受金額と異なっています。</summary>
        public const string MsgWngAdvanceReceivedAmountUnmatchBeforeAndAfter = "W02350";
        /// <summary>都度請求の入金予定日は前1桁は0、後2桁は00-99で入力してください。</summary>
        public const string MsgWngNumberValueValid0ForFirstAnd00To99ForRest = "W02360";
        /// <summary>{0}は00-99の範囲で入力してください。</summary>
        public const string MsgWngNumberValueValid0To99 = "W02370";
        /// <summary>{0}は00-27もしくは99で入力してください。</summary>
        public const string MsgWngNumberValueValid0To27or99 = "W02380";
        /// <summary>最低１つは表示幅を0以上に設定してください。</summary>
        public const string MsgWngAtleastOneColumnWidthNeedGreaterThanZero = "W02390";
        /// <summary>区分識別内で重複の外部コードは登録できません。</summary>
        public const string MsgWngDuplicateExternalCode = "W02400";
        /// <summary>前受分割は最大99件までです。</summary>
        public const string MsgWngAdvancedReceiveSplitRowCountError = "W02410";
        /// <summary>ドライブ名：{0}は使用できません。</summary>
        public const string MsgWngInvalidDriveLetter = "W02420";
        /// <summary>クライアントのパスが参照できません。
        ///リモートデスクトップ接続を行う前に、ローカルリソースのドライブ設定を行ってください。</summary>
        public const string MsgWngSetLocalDriveForClientAccess = "W02430";
        /// <summary>同名のファイルまたはフォルダが既に存在します。</summary>
        public const string MsgWngAlreadyExistsFolderAndFileName = "W02440";
        /// <summary>ログインしている担当者のVONE利用を解除することはできません。</summary>
        public const string MsgWngNoLoginAuthorization = "W02450";
        /// <summary>既定のステータスコードは削除できません。</summary>
        public const string MsgWngDefaultStatusAndCannotDelete = "W02460";
        /// <summary>入力されたパターンNoは、既に他のレベルで設定済の為使用できません。</summary>
        public const string MsgWngRegistedOtherLevelPatternNo = "W02470";
        /// <summary>入力された回収区分は、既に他のパターンで設定済の為使用できません。</summary>
        public const string MsgWngCollectCategoryRegistedOtherPattern = "W02480";
        /// <summary>{0}する場合「{1}」を指定してください。</summary>
        public const string MsgWngSelectingNeedAnotherAssigment = "W02490";
        /// <summary>複数の得意先に結びついている入金データがあります。</summary>
        public const string MsgWngReceiptDataDuped = "W02500";
        /// <summary>消費税合計と請求書単位消費税合計が一致していません。</summary>
        public const string MsgWngTaxAmountDifference = "W02510";
        /// <summary>手数料負担は相手先の場合、{0}を「0:しない」に入力してください。</summary>
        public const string MsgWngShareTransferFeeMisMatch = "W02520";
        /// <summary>消込済のデータが含まれるため、発行できません。</summary>
        public const string MsgWngNotPrintReminder = "W02530";
        /// <summary>{0}は{1}で入力してください。</summary>
        public const string MsgWngInputNeedxxDigits = "W02540";
        /// <summary>APIトークンの有効期限が切れています。API連携設定でトークンの再設定を行ってください。</summary>
        public const string MsgWngExpiredToken = "W02550";
        /// <summary>請求金額が12桁以上となるデータが含まれています。該当のデータは除外されました。</summary>
        public const string MsgWngOverAmount = "W02560";
        /// <summary>設定されている{0}は、{1}マスターに存在しません。</summary>
        public const string MsgWngNotExistsMaster = "W02570";
        /// <summary>{0}以外は編集不可です。</summary>
        public const string MsgWngEditableOnlyManualInput = "W02580";
        /// <summary>督促状発行対象外のデータが含まれています。発行できません。</summary>
        public const string MsgWngExcludeReminderCannotPublish = "W02590";
        /// <summary>入力された{0}は存在しません。</summary>
        public const string MsgWngSelectedDataIsNotExists = "W02600";
        /// <summary>入力された締め処理年月「{0}」は既に締め処理済みです。</summary>
        public const string MsgWngIsClosed = "W02610";
        /// <summary>締め処理年月は「{0}」以降を入力して下さい。</summary>
        public const string MsgWngClosingMonthIsOld = "W02620";
        /// <summary>{0}が存在します。</summary>
        public const string MsgWngCommonExists = "W02630";
        #endregion

        #region エラー( E00000, MsgErr)

        /// <summary>プログラムの起動に失敗しました。</summary>
        public const string MsgErrProgramStartError = "E00010";
        /// <summary>指定された機能は現在ご利用になれません。</summary>
        public const string MsgErrOptionSettingError = "E00020";
        /// <summary>登録に失敗しました。</summary>
        public const string MsgErrSaveError = "E00030";
        /// <summary>更新に失敗しました。</summary>
        public const string MsgErrUpdateError = "E00040";
        /// <summary>削除に失敗しました。</summary>
        public const string MsgErrDeleteError = "E00050";
        /// <summary>一括消込に失敗しました。</summary>
        public const string MsgErrSequentialMatchingError = "E00060";
        /// <summary>消込に失敗しました。</summary>
        public const string MsgErrMatchingError = "E00070";
        /// <summary>取込に失敗しました。</summary>
        public const string MsgErrEBFileImportError = "E00080";
        /// <summary>インポートに失敗しました。</summary>
        public const string MsgErrImportErrorWithoutLog = "E00090";
        /// <summary>インポートに失敗しました。詳細はログを参照してください。</summary>
        public const string MsgErrImportErrorWithLog = "E00100";
        /// <summary>取込設定が行われていません。</summary>
        public const string MsgErrNoSettingError = "E00110";
        /// <summary>エクスポートに失敗しました。</summary>
        public const string MsgErrExportError = "E00120";
        /// <summary>帳票作成に失敗しました。</summary>
        public const string MsgErrCreateReportError = "E00130";
        /// <summary>データ検索中にエラーが発生しました。</summary>
        public const string MsgErrDataSearch = "E00140";
        /// <summary>不要データの削除に失敗しました。</summary>
        public const string MsgErrUnneededDataDeleteError = "E00150";
        /// <summary>ＤＢのバックアップに失敗しました。</summary>
        public const string MsgErrDBBackupError = "E00170";
        /// <summary>ＤＢのリストアに失敗しました。</summary>
        public const string MsgErrDBRestoreError = "E00180";
        /// <summary>残高の繰越に失敗しました。</summary>
        public const string MsgErrBalanceCarryForwardError = "E00200";
        /// <summary>残高の戻しに失敗しました。</summary>
        public const string MsgErrBalanceCarryBackwardError = "E00210";
        /// <summary>{0}に失敗しました。</summary>
        public const string MsgErrSomethingError = "E00230";
        /// <summary>フォルダ {0} がありません。処理を中断します。</summary>
        public const string MsgErrNotExistsFolderAndCancelProcess = "E00250";
        /// <summary>フォルダ {0} の作成に失敗しました。処理を中断します。</summary>
        public const string MsgErrFolderCreateError = "E00260";
        /// <summary>ファイル{0}の{1}に失敗しました。</summary>
        public const string MsgErrErrorFileHandling = "E00270";
        /// <summary>ファイル操作に必要なアクセス権限がありません。</summary>
        public const string MsgErrUnauthorizedAccess = "E00280";
        /// <summary>承認に失敗しました。</summary>
        public const string MsgErrApprovalError = "E00320";
        /// <summary>承認取消に失敗しました。</summary>
        public const string MsgErrCancelApprovalError = "E00330";
        /// <summary>指定フォルダへのアクセス権限がありません。</summary>
        public const string MsgErrUnauthorizedAccessTargetFolder = "E00340";
        /// <summary>サーバーにログイン失敗しました。</summary>
        public const string MsgErrServerLoginError = "E00350";
        /// <summary>読込に失敗しました。</summary>
        public const string MsgErrReadingError = "E00360";
        /// <summary>復元に失敗しました。</summary>
        public const string MsgErrRecoveryError = "E00380";
        /// <summary>指定ファイル「{0}」は、別のプロセスで使用されているため、{1}できません。</summary>
        public const string MsgErrSpecificFileUsingOtherProcess = "E00390";
        /// <summary>指定ファイル「{0}」は、アクセス権限がないため、{1}できません。</summary>
        public const string MsgErrAccessDeniedForSpecificFile = "E00400";
        /// <summary>指定フォルダー「{0}」へのアクセス権限がありません。</summary>
        public const string MsgErrAccessDeniedForSpecificFolder = "E00410";
        /// <summary>取込ファイル内の銀行口座情報のフォーマットが異なります。</summary>
        public const string MsgErrBankAccountFormatError = "E00420";
        /// <summary>取込ファイル内の専用入金口座番号のフォーマットが異なります。</summary>
        public const string MsgErrPayerCodeFormatError = "E00430";
        /// <summary>フォルダ {0} の削除に失敗しました。処理を中断します。</summary>
        public const string MsgErrFolderDeleteError = "E00440";
        /// <summary>変換処理に失敗しました。詳細はログを参照してください。</summary>
        public const string MsgErrTransformErrorWithLog = "E00450";
        /// <summary>連携処理に失敗しました。</summary>
        public const string MsgErrPostProcessFailure = "E00460";
        #endregion
    }
}
