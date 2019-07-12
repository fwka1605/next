﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Service.Master
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IColumnNameMaster" を変更できます。
    [ServiceContract]
    public interface IColumnNameSettingMaster
    {
        [OperationContract]
        Task<ColumnNameSettingResult> SaveAsync(string SessionKey, ColumnNameSetting ColumnName);

        [OperationContract]
        Task<ColumnNameSettingResult> GetAsync(string SessionKey, int CompanyId, string TableName, string ColumnName);

        [OperationContract]
        Task<ColumnNameSettingsResult> GetItemsAsync(string SessionKey, int CompanyId);

    }
}
