using Newtonsoft.Json.Linq;

namespace RosettaObject
{
    public class StringRObjConvertor : RObjConvertor<string>
    {
        public override string TypeName => ConstString.StringType;

        public override JObject ToJson(string obj, ConvertorSet convertorSet)
        {
            return new JObject
            {
                { ConstString.Value, obj }
            };
        }

        public override string FromJson(JObject jobj, ConvertorSet convertorSet)
        {
            return jobj.GetValue(ConstString.Value).Value<string>();
        }
    }
}