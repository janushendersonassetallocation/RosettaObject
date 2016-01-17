using System;
using Newtonsoft.Json.Linq;

namespace RosettaObject
{
    public abstract class BaseRObjConvertor
    {
        public abstract string TypeName { get; }
        public abstract Type Type { get; }
        public abstract bool CanConvert(object obj);
        public abstract JObject ToJson(object obj, ConvertorSet convertorSet);
        public abstract object ObjectFromJson(JObject jobj, ConvertorSet convertorSet);
    }

    public abstract class RObjConvertor<T> : BaseRObjConvertor
    {
        public virtual bool CanConvert(T obj) => true;
        public override bool CanConvert(object obj) => obj is T && CanConvert((T)obj);
        public abstract JObject ToJson(T obj, ConvertorSet convertorSet);
        public override JObject ToJson(object obj, ConvertorSet convertorSet) => obj is T ? ToJson((T) obj, convertorSet) : null;
        public abstract T FromJson(JObject jobj, ConvertorSet convertorSet);
        public override object ObjectFromJson(JObject jobj, ConvertorSet convertorSet) => FromJson(jobj, convertorSet);
        public override Type Type => typeof (T);
    }
}