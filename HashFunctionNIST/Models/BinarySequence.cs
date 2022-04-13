using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashFunctionNIST.Models
{
    public class BinarySequence
    {
        private readonly bool[] _sequence;

        public int Length => _sequence.Length;
        
        public BinarySequence(IEnumerable<byte> array)
        {
            var sb = new StringBuilder();
            
            foreach (var item in array)
                sb.Append(Convert.ToString(item, 2));

            _sequence = new bool[sb.Length];
            
            for (var i = 0; i < _sequence.Length; ++i)
                if (sb[i] is '1')
                    _sequence[i] = true;
        }

        public (int, int) GetCountOfZerosAndOnes()
        {
            var countOfZeros = _sequence.Count(symbol => symbol);

            return (countOfZeros, _sequence.Length - countOfZeros);
        }

        public string this[Range range]
        {
            get
            {
                var sb = new StringBuilder();

                for (var i = range.Start.Value; i < range.End.Value; ++i)
                    sb.Append(_sequence[i] ? '1' : '0');

                return sb.ToString();
            }
        }

        public int this[int index] => _sequence[index] ? 1 : 0;

        public override string ToString() => this[.._sequence.Length];
    }
}