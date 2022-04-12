using System;
using System.Collections;

namespace HashFunctionNIST.Models
{
    public class State : ICloneable, IEnumerable
    {
        public static State FullOfZero => new(new byte[]{ 0, 0, 0, 0 });
        public const int Length = 4;
        
        private readonly byte[] _array;

        public State(byte[,] array, int offset)
        {
            _array = new byte[Length];

            for (var i = 0; i < Length; ++i)
                _array[i] = array[i, offset];
        }

        public State(byte[] array) => _array = array;

        public static State operator +(State state1, State state2)
        {
            var array = new byte[4];

            for (var i = 0; i < Length; ++i)
                array[i] = (byte)(state1._array[i] + state2._array[i]);

            return new State(array);
        }

        public byte this[int index] 
        {
            get => _array[index];
            set => _array[index] = value;
        }

        public object Clone() => new State(_array.Clone() as byte[]);

        public IEnumerator GetEnumerator() => _array.GetEnumerator();
    }
}