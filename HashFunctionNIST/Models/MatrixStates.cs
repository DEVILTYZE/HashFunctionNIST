using System;
using System.Text;

namespace HashFunctionNIST.Models
{
    public class MatrixStates
    {
        private readonly State[] _states;

        public MatrixStates(byte[,] array)
        {
            _states = new State[30];
            
            for (var i = 0; i < _states.Length - array.GetLength(1); ++i)
                _states[i] = State.FullOfZero;

            for (var i = 0; i < array.GetLength(1); ++i)
                _states[i + 22] = new State(array, i);
        }

        public void Shift(int offset, bool isLeft = true)
        {
            if (_states.Clone() is not State[] newStates)
                return;
            
            if (isLeft)
                for (var i = 0; i < _states.Length; ++i)
                    _states[i] = newStates[i - offset < 0 ? _states.Length + i - offset : i - offset];
            else
                for (var i = 0; i < _states.Length; ++i)
                    _states[i] = newStates[i + offset > _states.Length ? i + offset - _states.Length : i + offset];
        }

        public State this[int index]
        {
            get => _states[index];
            set => _states[index] = value.Clone() as State;
        }

        public byte[,] GetSBlockMatrix()
        {
            var result = new byte[4, 4];
            
            for (var i = 0; i < result.GetLength(0); ++i)
            for (var j = 0; j < result.GetLength(1); ++j)
                result[i, j] = _states[i][j];

            return result;
        }

        public void SetSBlockMatrix(Matrix matrix)
        {
            for (var i = 0; i < matrix.Length.Item1; ++i)
            for (var j = 0; j < matrix.Length.Item2; ++j)
                _states[i][j] = matrix[i, j];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            
            for (var j = 0; j < State.Length; ++j)
            {
                foreach (var state in _states)
                    sb.Append(state[j]);

                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}