using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace RosettaObject
{
    public class IntArrayRObjConvertor : RObjConvertor<Array>
    {
        public override string TypeName => ConstString.IntArrayType;

        public override bool CanConvert(Array obj)
        {
            return obj.GetType().GetElementType() == typeof (int);
        }

        public override JObject ToJson(Array obj, ConvertorSet convertorSet)
        {
            int nDimensions = obj.Rank;
            int[] shape = new int[nDimensions];
            for (int i = 0; i < nDimensions; ++i)
            {
                shape[i] = obj.GetLength(i);
            }
            int[] items = obj.Cast<int>().ToArray();
            return new JObject
            {
                {ConstString.Shape,  new JArray(shape)},
                {ConstString.Values, new JArray(items)}
            };
        }

        public override Array FromJson(JObject jobj, ConvertorSet convertorSet)
        {
            int[] shape = jobj[ConstString.Shape].ToObject<int[]>();
            int[] values = jobj[ConstString.Values].ToObject<int[]>();
            var arr = Array.CreateInstance(typeof (int), shape);
            int[] index = new int[shape.Length];
            for (int i = 0; i < values.Length; ++i)
            {
                arr.SetValue(values[i], index);
                if (!Multidimensional.IncIndex(index, shape))
                {
                    break;
                }
            }
            return arr;
        }
    }
}