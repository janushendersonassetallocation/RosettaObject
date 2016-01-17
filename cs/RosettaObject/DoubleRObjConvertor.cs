using Newtonsoft.Json.Linq;

namespace RosettaObject
{
    public class DoubleRObjConvertor : RObjConvertor<double>
    {
        public override string TypeName => ConstString.DoubleType;

        public override JObject ToJson(double obj, ConvertorSet convertorSet)
        {
            return new JObject
            {
                { ConstString.Value, obj }
            };
        }

        public override double FromJson(JObject jobj, ConvertorSet convertorSet)
        {
            return jobj.GetValue(ConstString.Value).Value<double>();
        }
    }
}