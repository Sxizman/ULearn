using System;
using System.Collections;
using System.Collections.Generic;

namespace hashes
{
	public class ReadonlyBytes : IEnumerable<byte>
    {
        private readonly byte[] data;
        private readonly int hash;

        public ReadonlyBytes(params byte[] data)
        {
            if (data is null)
                throw new ArgumentNullException();

            this.data = data;
            unchecked
            {
                hash = (int)0x811c9dc5;
                foreach (var b in data)
                    hash = (hash ^ b) * 0x01000193;
            }
        }

        public int Length
        {
            get { return data.Length; }
        }

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= data.Length)
                    throw new IndexOutOfRangeException();

                return data[index];
            }
        }

        public override bool Equals(object obj)
        {
            var bytes = obj as ReadonlyBytes;
            if (bytes is null)
                return false;
            if (bytes.GetType() != typeof(ReadonlyBytes))
                return false;
            
            if (bytes.data.Length != data.Length)
                return false;

            for (var i = 0; i < data.Length; i++)
                if (bytes.data[i] != data[i])
                    return false;
            
            return true;
        }

        public override int GetHashCode()
        {
            return hash;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<byte> GetEnumerator()
        {
            foreach (var b in data)
                yield return b;
        }

        public override string ToString()
        {
            return $"[{string.Join(", ", data)}]";
        }
    }
}