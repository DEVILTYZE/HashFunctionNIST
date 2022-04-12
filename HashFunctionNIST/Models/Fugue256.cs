using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashFunctionNIST.Models
{
    public class Fugue256
    {
        private static readonly byte[,] Iv256 =
        {
            { 0xE9, 0x66, 0xE0, 0xD2, 0xF9, 0xFB, 0x91, 0x34 },
            { 0x52, 0x71, 0xD4, 0xB0, 0x6C, 0xF9, 0x49, 0xF8 },
            { 0xBD, 0x13, 0xF6, 0xB5, 0x62, 0x29, 0xE8, 0xC2 },
            { 0xDE, 0x5F, 0x68, 0x94, 0x1D, 0xDE, 0x99, 0x48 }
        };

        private static readonly byte[,] M =
        {
            { 1, 4, 7, 1 }, 
            { 1, 1, 4, 7 }, 
            { 7, 1, 1, 4 }, 
            { 4, 7, 1, 1 }
        };

        private MatrixStates _matrix;

        // region Main ================================================================================================
        #region Main

        public byte[] Hash(string inputData)
        {
            SetStartStates();
            
            var inputArray = Encoding.UTF8.GetBytes(inputData);

            if (inputArray.Length % 4 != 0)
                Array.Resize(ref inputArray, inputArray.Length + inputArray.Length % 4);

            for (var i = 0; i < inputArray.Length / 4; ++i)
                RoundTransformation(new ArraySegment<byte>(inputArray, i * 4, 4).ToArray());

            GTransformation();

            return GetResult();
        }

        private void RoundTransformation(byte[] word)
        {
            Tix(word);

            for (var i = 0; i < 2; ++i)
            {
                _matrix.Shift(3);
                ColumnMix();
                SuperMix();
            }
        }

        private void GTransformation()
        {
            for (var i = 0; i < 10; ++i)
            {
                _matrix.Shift(3);
                ColumnMix();
                SuperMix();
            }

            for (var i = 0; i < 13; ++i)
            {
                _matrix[4] += _matrix[0];
                _matrix[15] += _matrix[0];
                _matrix.Shift(15);
                SuperMix();

                _matrix[4] += _matrix[0];
                _matrix[16] += _matrix[0];
                _matrix.Shift(14);
                SuperMix();
            }

            _matrix[4] += _matrix[0];
            _matrix[15] += _matrix[0];
        }

        #endregion
        
        // region Transformations =====================================================================================
        #region Transformations

        private void Tix(byte[] word)
        {
            _matrix[10] += _matrix[0];
            _matrix[0] = new State(word);
            _matrix[8] += _matrix[0];
            _matrix[1] += _matrix[24];
        }

        private void ColumnMix()
        {
            for (var i = 4; i < 7; ++i)
            {
                _matrix[i - 4] += _matrix[i];
                _matrix[i + 11] += _matrix[i];
            }
        }

        private void SuperMix()
        {
            var mMatrix = new Matrix(M);
            var uMatrixByteArray = _matrix.GetSBlockMatrix();
            var uZeroMatrix = Matrix.GetDiagonalMatrix(uMatrixByteArray);
            _matrix.SetSBlockMatrix((mMatrix * new Matrix(uMatrixByteArray) + uZeroMatrix * mMatrix.Transpose()).Rol());
        }

        #endregion

        //region Helpers ==============================================================================================
        #region Helpers

        private void SetStartStates() => _matrix = new MatrixStates(Iv256);

        private byte[] GetResult()
        {
            var indexes = new[] { 1, 2, 3, 4, 15, 16, 17, 18 };
            var result = new List<byte>();

            foreach (var index in indexes) 
                result.AddRange(_matrix[index].Cast<byte>());
            
            return result.ToArray();
        }

        #endregion
    }
}