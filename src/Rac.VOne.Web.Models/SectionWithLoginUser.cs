using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class SectionWithLoginUser
    {

        public SectionWithLoginUser() { }

        public SectionWithLoginUser(int loginUserId, int sectionId, int userId1,  int userId2)
        {
            LoginUserId = loginUserId;
            SectionId = sectionId;
            CreateBy = userId1;
            //CreateAt = dateTime1;
            UpdateBy = userId2;
            //UpdateAt = dateTime2;
        }

        public SectionWithLoginUser(string loginUserCode)
        {
            LoginUserCode = loginUserCode;
        }

        public SectionWithLoginUser(string code, string name, int id)
        {
           SectionCode = code;
           SectionName = name;
           SectionId = id;
        }

        [DataMember] public int SectionId { get; set; }
        [DataMember] public int LoginUserId { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public string LoginUserCode { get; set; }
        [DataMember] public string LoginUserName { get; set; }
        [DataMember] public string SectionCode { get; set; }
        [DataMember] public string SectionName { get; set; }
    }

    [DataContract]
    public class SectionWithLoginUserResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public SectionWithLoginUser SectionWithLoginUser { get; set; }
    }

    [DataContract]
    public class SectionWithLoginUsersResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<SectionWithLoginUser> SectionWithLoginUsers { get; set; }
    }
}
