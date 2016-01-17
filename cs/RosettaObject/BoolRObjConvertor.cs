using System;
using Newtonsoft.Json.Linq;

namespace RosettaObject
{
    public class BoolRObjConvertor : RObjConvertor<bool>
    {
        public override string TypeName => ConstString.BoolType;

        public override JObject ToJson(bool obj, ConvertorSet convertorSet)
        {
            return new JObject
            {
                { ConstString.Value, obj }
            };
        }

        public override bool FromJson(JObject jobj, ConvertorSet convertorSet)
        {
            return jobj.GetValue(ConstString.Value).Value<bool>();
        }
    }
}