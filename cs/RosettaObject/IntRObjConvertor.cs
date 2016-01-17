using Newtonsoft.Json.Linq;

namespace RosettaObject
{
    public class IntRObjConvertor : RObjConvertor<int>
    {
        public override string TypeName => ConstString.IntType;

        public override JObject ToJson(int obj, ConvertorSet convertorSet)
        {
            return new JObject
            {
                { ConstString.Value, obj }
            };
        }

        public override int FromJson(JObject jobj, ConvertorSet convertorSet)
        {
            return jobj.GetValue(ConstString.Value).Value<int>();
        }
    }
}