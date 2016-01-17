using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace RosettaObject
{
    public class DictionaryRObjConvertor : RObjConvertor<IDictionary>
    {
        public override string TypeName => ConstString.DictType;

        public override bool CanConvert(object obj)
        {
            if (obj is IDictionary)
            {
                Type[] arguments = obj.GetType().GetGenericArguments();
                if (arguments.Length > 0)
                {
                    Type keyType = arguments[0];
                    if (typeof (string).IsAssignableFrom(keyType))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override JObject ToJson(IDictionary obj, ConvertorSet convertorSet)
        {
            var dict = new Dictionary<string, object>();
            foreach (string key in obj.Keys.Cast<string>())
            {
                object value = obj[key];
                dict.Add(key, value);
            }
            return ToJson(dict, convertorSet);
        }

        public JObject ToJson(Dictionary<string, object> obj, ConvertorSet convertorSet)
        {
            var items = new JObject();
            foreach (KeyValuePair<string, object> pair in obj)
            {
                string key = pair.Key;
                object value = pair.Value;
                items[key] = convertorSet.ToJson(value);
            }
            return new JObject
            {
                {ConstString.Values, items }
            };
        }

        public override IDictionary FromJson(JObject jobj, ConvertorSet convertorSet)
        {
            var result = new Dictionary<string, object>();
            foreach (KeyValuePair<string, JToken> pair in (JObject)jobj[ConstString.Values])
            {
                string key = pair.Key;
                JObject value = pair.Value as JObject;
                result[key] = convertorSet.FromJson(value);
            }
            return result;
        }
    }
}