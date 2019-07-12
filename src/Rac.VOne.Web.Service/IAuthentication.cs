using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    [ServiceContract]
    public interface IAuthentication
    {
        [OperationContract]
        Task<DataSet> AuthenticateAsync(DataSet arg);

        [OperationContract]
        Task<DataSet> AuthorizeAsync(DataSet arg);
    }


    //using System.Runtime.Serialization;
    // サービス操作に複合型を追加するには、以下のサンプルに示すようにデータ コントラクトを使用します。
    //[DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
}
