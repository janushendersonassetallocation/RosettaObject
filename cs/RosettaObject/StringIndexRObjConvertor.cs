using System.Linq;
using Deedle;
using Deedle.Indices;
using Newtonsoft.Json.Linq;

namespace RosettaObject
{
    public class StringIndexRObjConvertor : RObjConvertor<IIndex<string>>
    {
        public override string TypeName => ConstString.StringIndexType;

        public override JObject ToJson(IIndex<string> obj, ConvertorSet convertorSet)
        {
            return new JObject
            {
                {ConstString.Values, new JArray(obj.Keys.ToList())}
            };
        }

        public override IIndex<string> FromJson(JObject jobj, ConvertorSet convertorSet)
        {
            string[] values = jobj[ConstString.Values].ToObject<string[]>();
            return Index.Create(values);
        }
    }
}