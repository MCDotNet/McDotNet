using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDotNet.Data
{
    public class ExtractInstructions : IList<string>
    {
        public string this[int index] { get => ((IList<string>)Exclude)[index]; set => ((IList<string>)Exclude)[index] = value; }

        public List<string> Exclude { get; set; }

        public int Count => ((IList<string>)Exclude).Count;

        public bool IsReadOnly => ((IList<string>)Exclude).IsReadOnly;

        public void Add(string item)
        {
            ((IList<string>)Exclude).Add(item);
        }

        public void Clear()
        {
            ((IList<string>)Exclude).Clear();
        }

        public bool Contains(string item)
        {
            return ((IList<string>)Exclude).Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            ((IList<string>)Exclude).CopyTo(array, arrayIndex);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IList<string>)Exclude).GetEnumerator();
        }

        public int IndexOf(string item)
        {
            return ((IList<string>)Exclude).IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            ((IList<string>)Exclude).Insert(index, item);
        }

        public bool Remove(string item)
        {
            return ((IList<string>)Exclude).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<string>)Exclude).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<string>)Exclude).GetEnumerator();
        }
    }
}
