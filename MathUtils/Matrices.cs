using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

            Matrix<double> C = DenseMatrix.OfArray(new double[,]
            {
                { 2, -3, 1 },
                { 3, 1, 1 },
                { -1, -2, -1 },
            });

            Matrix<double> D = DenseMatrix.OfArray(new double[,]
            {
                { 2 },
                { -1 },
                { 1 }
            });

            Console.WriteLine(C.Solve(D));

            Vector3 a = new Vector3(3, -1, 2);
            Vector3 b = new Vector3(0, -1, 1);
            Console.WriteLine(Math.Acos(Vector3.Dot(a, b) / (a.Length() * b.Length())) * (360f / (2 * Math.PI)));
        }
    }
}
