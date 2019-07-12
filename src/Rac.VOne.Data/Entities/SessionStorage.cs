using System;

namespace Rac.VOne.Data.Entities
{
    public class SessionStorage
    {
        public string  SessionKey { get; set; }
        public string ConnectionInfo { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
