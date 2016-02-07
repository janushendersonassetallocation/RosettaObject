using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Deedle;
using Deedle.Indices;
using Newtonsoft.Json.Linq;

namespace RosettaObject
{
    public class IntIndexRObjConvertor : RObjConvertor<IIndex<int>>
    {
        public override string TypeName => ConstString.IntIndexType;

        public override JObject ToJson(IIndex<int> obj, ConvertorSet convertorSet)
        {
            return new JObject
            {
                {ConstString.Values, new JArray(obj.Keys.ToList())}
            };
        }

        public override IIndex<int> FromJson(JObject jobj, ConvertorSet convertorSet)
        {
            int[] values = jobj[ConstString.Values].ToObject<int[]>();
            return Index.Create(values);
        }
    }
}