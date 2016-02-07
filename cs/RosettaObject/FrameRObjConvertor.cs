using System;
using System.Collections.Generic;
using System.Linq;
using Deedle;
using Deedle.Indices;
using Newtonsoft.Json.Linq;

namespace RosettaObject
{
    public class FrameRObjConvertor : RObjConvertor<IFrame>
    {
        public override string TypeName => ConstString.DataFrameType;

        public override JObject ToJson(IFrame obj, ConvertorSet convertorSet)
        {
            dynamic frame = obj;

            return ToJson2(frame, convertorSet);
        }

        private static JObject ToJson2<TRowKey, TColumnKey>(Frame<TRowKey, TColumnKey> frame, ConvertorSet convertorSet)
        {
            var index = convertorSet.ToJson(frame.RowIndex);
            var columns = convertorSet.ToJson(frame.ColumnIndex);
            List<object> data = new List<object>();
            for (int i = 0; i < frame.RowCount; ++i)
            {
                for (int j = 0; j < frame.ColumnCount; ++j)
                {
                    data.Add(frame.GetColumnAt<object>(j).GetAt(i));
                }
            }
            var dataArray = new JArray(data);

            return new JObject
            {
                {ConstString.Index, index},
                {ConstString.Columns, columns},
                {ConstString.Data, dataArray}
            };
        }

        public override IFrame FromJson(JObject jobj, ConvertorSet convertorSet)
        {
            object index = convertorSet.FromJson((JObject)jobj[ConstString.Index]);
            object columns = convertorSet.FromJson((JObject)jobj[ConstString.Columns]);
            object[] data = jobj[ConstString.Data].ToObject<object[]>();

            Type indexType = index.GetType().GetGenericArguments()[0];
            Type columnType = columns.GetType().GetGenericArguments()[0];
            Frame<int, int> foo;

            return null;
            //var rows = new List<KeyValuePair<int, Series<int, object>>>();
            //foreach (var row in index)
            //{
            //    IIndex<int>
            //    rows.Add(KeyValuePair.Create());
            //}

            //var frame = Frame.FromRows(rows);
            //return frame;
        }
    }
}