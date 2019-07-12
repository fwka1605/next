using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Common.Settings
{
    [SettingFile(FileName = "UserSettings.xml", Path = SettingPath.Roaming, ContainerType = typeof(ArrayOfDialogInfo))]
    [DataContract(Namespace = "")]
    public class DialogInfo : IIndividualUserSetting, IEquatable<DialogInfo>
    {
        [DataMember(Name = "会社コード", Order = 1)]
        public string CompanyCode { get; set; }

        [DataMember(Name = "担当者コード", Order = 2)]
        public string UserCode { get; set; }

        [DataMember(Name = "キー", Order = 3)]
        public string Key { get; set; }

        [DataMember(Name = "値", Order = 4)]
        public string Value { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }

        public override int GetHashCode()
        {
            return string.Join(string.Empty,
                    Key ?? string.Empty, CompanyCode ?? string.Empty, UserCode ?? string.Empty)
                    .GetHashCode();
        }

        public bool Equals(DialogInfo other)
        {
            return other.Key == Key
                    && other.CompanyCode == CompanyCode
                    && other.UserCode == UserCode;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is DialogInfo)) return false;
            return Equals(obj as DialogInfo);
        }
    }

    [CollectionDataContract(Namespace = "")]
    public class ArrayOfDialogInfo : List<DialogInfo>
    {
    }
}
