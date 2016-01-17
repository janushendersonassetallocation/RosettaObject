using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace RosettaObject
{
    public class ListRObjConvertor : RObjConvertor<IEnumerable>
    {
        public override string TypeName => ConstString.ListType;

        public override JObject ToJson(IEnumerable obj, ConvertorSet convertorSet)
        {
            var items = new JArray();
            foreach (object o in obj)
            {
                items.Add(convertorSet.ToJson(o));
            }

            return new JObject
            {
                {ConstString.Values, items}
            };
        }

        public override IEnumerable FromJson(JObject jobj, ConvertorSet convertorSet)
        {
            return (from token in (JArray) jobj[ConstString.Values] select token as JObject into value select convertorSet.FromJson(value)).ToList();
        }
    }
}