using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class UnaryValue<T>
    {
        public UnaryValue() : this(default(T)) { }

        public UnaryValue(T value)
        {
            Value = value;
        }

        [DataMember] public T Value { get; set; }
    }

    public static class UnaryValueExtensions
    {
        public static UnaryValue<T> GetUnaryValue<T>(this T value) => new UnaryValue<T>(value);
    }
}
