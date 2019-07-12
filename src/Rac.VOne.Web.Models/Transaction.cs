using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class Transaction
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        public Transaction(ITransactionData entity)
        {
            if (entity != null)
            {
                Id = entity.Id;
                UpdateAt = entity.UpdateAt;
            }
        }
    }

    public class Entity
    {
        public int Id { get; set; }
        public DateTime UpdateAt { get; set; }

        public Entity(IMasterData entity)
        {
            if (entity != null)
            {
                Id = entity.Id;
                UpdateAt = entity.UpdateAt;
            }
        }
    }
}
