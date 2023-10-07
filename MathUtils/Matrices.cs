using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathUtils
{
    internal static class Matrices
    {
        public static void SwapTest()
        {
            Matrix<double> A = DenseMatrix.OfArray(new double[,]
            {
                { 4, 3, -7 },
                { 2, -5, 1 },
                { 11, -4, 6 }
            });

            Matrix<double> B = DenseMatrix.OfArray(new double[,]
            {
                { 0, 0, 1 },
                { 1, 0, 0 },
                { 0, 1, 0 }
            });

            Console.WriteLine(B * A);
        }
    }
}
