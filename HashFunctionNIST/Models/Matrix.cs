namespace HashFunctionNIST.Models
{
    public class Matrix
    {
        private readonly byte[,] _matrix;

        public (int, int) Length => (_matrix.GetLength(0), _matrix.GetLength(1));
        
        public Matrix(byte[,] array) => _matrix = array.Clone() as byte[,];

        public Matrix Transpose()
        {
            var transposedMatrix = new byte[4, 4];
            
            for (var j = 0; j < _matrix.GetLength(1); ++j)
            for (var i = 0; i < _matrix.GetLength(0); ++i)
                transposedMatrix[j, i] = _matrix[i, j];

            return new Matrix(transposedMatrix);
        }

        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            var result = new byte[4, 4];
            
            for (var i = 0; i < result.GetLength(0); ++i)
            for (var j = 0; j < result.GetLength(1); ++j)
                result[i, j] = (byte)(matrix1[i, j] + matrix2[i, j]);

            return new Matrix(result);
        }

        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            var result = new byte[4, 4];
            
            for (var i = 0; i < result.GetLength(0); ++i)
            for (var j = 0; j < result.GetLength(1); ++j)
            for (var k = 0; k < result.GetLength(0); ++k)
                result[i, j] += (byte)(matrix1[i, k] * matrix2[k, j]);

            return new Matrix(result);
        }

        public Matrix Rol()
        {
            var result = new byte[4, 4];
            
            for (var i = 0; i < result.GetLength(0); ++i)
            for (int j = (result.GetLength(1) - i) % result.GetLength(1), k = 0;
                 k < result.GetLength(1);
                 j = (j + 1) % result.GetLength(1), ++k)
                result[i, k] = _matrix[i, j];

            return new Matrix(result);
        }

        public byte this[int index1, int index2] => _matrix[index1, index2];

        public static Matrix GetDiagonalMatrix(byte[,] matrix)
        {
            var newMatrix = new byte[4, 4];

            for (var i = 0; i < matrix.GetLength(0); ++i)
                newMatrix[i, i] = Sum(matrix, i);

            return new Matrix(newMatrix);
        }

        private static byte Sum(byte[,] matrix, int index)
        {
            var sum = byte.MinValue;

            for (var i = 0; i < matrix.GetLength(1); ++i)
                sum += matrix[index, i];

            return sum;
        }
    }
}