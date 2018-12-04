using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Data.GemTech.Model
{
    public class RedisUrlCollection : ICollection
    {
        public IEnumerable<Uri> Urls { get; set; }

        public int Count => Urls.Count();

        public bool IsSynchronized => false;

        public object SyncRoot => null;

        public void CopyTo(Array array, int index)
        {
            foreach(var uri in Urls)
            {
                array.SetValue(uri, index);
            }
        }

        public IEnumerator GetEnumerator()
        {
            yield return Urls;
        }
    }
}
