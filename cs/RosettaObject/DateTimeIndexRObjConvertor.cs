using System;
using System.Linq;
using Deedle;
using Deedle.Indices;
using Newtonsoft.Json.Linq;

namespace RosettaObject
{
    public class DateTimeIndexRObjConvertor : RObjConvertor<IIndex<DateTime>>
    {
        public override string TypeName => ConstString.DateTimeIndexType;

        public override JObject ToJson(IIndex<DateTime> obj, ConvertorSet convertorSet)
        {
            return new JObject
            {
                {ConstString.Values, new JArray(obj.Keys.ToList())}
            };
        }

        public override IIndex<DateTime> FromJson(JObject jobj, ConvertorSet convertorSet)
        {
            DateTime[] values = jobj[ConstString.Values].ToObject<DateTime[]>();
            return Index.Create(values);
        }
    }
}