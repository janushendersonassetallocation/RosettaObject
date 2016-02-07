using System;
using Newtonsoft.Json.Linq;

namespace RosettaObject
{
    public class DateTimeRObjConvertor : RObjConvertor<DateTime>
    {
        public override string TypeName => ConstString.DateTimeType;

        public override JObject ToJson(DateTime obj, ConvertorSet convertorSet)
        {
            return new JObject
            {
                { ConstString.Value, obj.ToString("o") }
            };
        }

        public override DateTime FromJson(JObject jobj, ConvertorSet convertorSet)
        {
            string dateString = jobj.GetValue(ConstString.Value).Value<string>();
            return DateTime.Parse(dateString);
        }
    }
}