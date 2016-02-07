using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace RosettaObject
{
    public class ConvertorSet : IEnumerable<BaseRObjConvertor>
    {
        public static ConvertorSet Default = new ConvertorSet
        {
            new BoolRObjConvertor(),
            new IntRObjConvertor(),
            new StringRObjConvertor(),
            new DoubleRObjConvertor(),
            new DateTimeRObjConvertor(),
            new IntArrayRObjConvertor(),
            new DoubleArrayRObjConvertor(),
            new DictionaryRObjConvertor(),
            new ListRObjConvertor(),
            new IntIndexRObjConvertor(),
            new StringIndexRObjConvertor(),
            new DateTimeIndexRObjConvertor(),
            new FrameRObjConvertor()
        };

        private readonly IList<BaseRObjConvertor> _convertors;

        public ConvertorSet()
        {
            _convertors = new List<BaseRObjConvertor>();
        }

        public ConvertorSet(IList<BaseRObjConvertor> convertors)
        {
            _convertors = convertors;
        }

        public void Add(BaseRObjConvertor convertor)
        {
            _convertors.Add(convertor);
        }

        public JObject ToJson(object obj)
        {
            foreach (var convertor in _convertors)
            {
                if (convertor.CanConvert(obj))
                {
                    var result = convertor.ToJson(obj, this);
                    if (result != null)
                    {
                        result[ConstString.Type] = convertor.TypeName;
                        return result;
                    }
                }
            }
            return null;
        }

        public object FromJson(JObject jobj)
        {
            if (jobj == null)
            {
                return null;
            }
            var typeName = jobj[ConstString.Type].Value<string>();
            foreach (var convertor in _convertors)
            {
                if (convertor.TypeName == typeName)
                {
                    object result = convertor.ObjectFromJson(jobj, this);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }

        public IEnumerator<BaseRObjConvertor> GetEnumerator()
        {
            return _convertors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
