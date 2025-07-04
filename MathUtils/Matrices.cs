﻿using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Random;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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

        public static void HomeworkTest()
        {
            Matrix<double> A = DenseMatrix.OfArray(new double[,]
            {
                { 2, 3 },
                { 3, 2 }
            });

            MathNet.Numerics.LinearAlgebra.Vector<double> B = DenseVector.OfArray(new double[] { 16, 14 });

            Console.WriteLine(A.Solve(B));
        }

        public static void Momentum(double m1, double m2, double v1, double v2)
        {
            Matrix<double> A = DenseMatrix.OfArray(new double[,]
            {
                { m1, m2 },
                { -m1, m2 }
            });

            Matrix<double> B = DenseMatrix.OfArray(new double[,]
            {
                { m1*v1 + m2*v2 },
                { m1*v1 - m2*v2 }
            });

            Console.WriteLine("ax + by = c");
            Console.WriteLine("Solving for x, y given a, b, c");
            Console.WriteLine(A.Solve(B));

            Matrix<double> A1 = DenseMatrix.OfArray(new double[,]
            {
                { m1, m2 },
                { m1, -m2 }
            });

            Matrix<double> B1 = DenseMatrix.OfArray(new double[,]
            {
                { m1, m2 },
                { -m1, m2 }
            });

            Console.WriteLine("ax + by = az + bw");
            Console.WriteLine("Solving for x, y, z, w given a, b ");
            Console.WriteLine(A1.Solve(B1));

            Matrix<double> C1 = DenseMatrix.OfArray(new double[,]
            {
                { v1 },
                { v2 }
            });

            Console.WriteLine("A*B = C");
            Console.WriteLine("Solving for C (v1f, v2f) via matrix multiplication of previous matrix and [v1,--v2]");
            Console.WriteLine(A1.Solve(B1) * C1);
        }

        public static void RotationTest()
        {
            Matrix<double> A = DenseMatrix.OfArray(new double[,]
            {
                { 1, 2, 3 },
                { 0, 0, 4 },
                { 0, 0, 5 }
            });

            Console.WriteLine(A);
            Console.WriteLine(A.RotateRight());
            Console.WriteLine(A.RotateAround());
            Console.WriteLine(A.RotateLeft());
        }

        public static void ConvergenceTest()
        {
            Matrix<double> A = DenseMatrix.OfArray(new double[,]
            {
                { 0, -1 },
                { 1,  0 }
            });

            Matrix<double> B = DenseMatrix.Create(4, 4, 0);
            B.SetSubMatrix(0, 0, A);
            B.SetSubMatrix(B.RowCount / 2, B.ColumnCount / 2, A);

            Matrix<double> C = DenseMatrix.Create(8, 8, 0);
            C.SetSubMatrix(0, 0, B);
            C.SetSubMatrix(C.RowCount / 2, C.ColumnCount / 2, B);

            Console.WriteLine(C);
            Console.WriteLine(C * C);
        }

        public static Matrix<T> GenerateReverseIdentity<T>(int size) where T : struct, IEquatable<T>, IFormattable
        {
            T[,] values = new T[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    values[i, j] = Matrix<T>.Zero;

            for (int i = 0; i < size; i++)
                values[i, (size - 1) - i] = Matrix<T>.One;

            Matrix<T> matrix = CreateMatrix.DenseOfArray(values);
            return matrix;
        }

        public static Matrix<double> LayerTest(Matrix<double> inputs, Matrix<double> layer)
        {
            return inputs * layer;
        }

        public static Matrix<double> LayerTest(double[] inputs, double[,] weights, double[] biases)
        {
            Matrix<double> m_inputs = DenseMatrix.OfColumnMajor(1, inputs.Length + 1, inputs);
            m_inputs.SetColumn(inputs.Length, new double[] { 1 });
            Console.WriteLine(m_inputs);

            Matrix<double> m_layer = DenseMatrix.OfArray(weights);
            m_layer = m_layer.InsertRow(inputs.Length, DenseVector.OfArray(biases));
            double[] zerosExceptForEnd = new double[inputs.Length + 1];
            zerosExceptForEnd[inputs.Length] = 1;
            m_layer = m_layer.InsertColumn(biases.Length, DenseVector.OfArray(zerosExceptForEnd));
            Console.WriteLine(m_layer);

            return LayerTest(m_inputs, m_layer);
        }

        // Extensions
        public static Matrix<T> FlipVertical<T>(this Matrix<T> A) where T : struct, IEquatable<T>, IFormattable
        {
            if (A.RowCount != A.ColumnCount)
                throw new ArgumentException("Provided matrix was not square");

            Matrix<T> B = GenerateReverseIdentity<T>(A.ColumnCount);
            return B * A;
        }

        public static Matrix<T> FlipHorizontal<T>(this Matrix<T> A) where T : struct, IEquatable<T>, IFormattable
        {
            if (A.RowCount != A.ColumnCount)
                throw new ArgumentException("Provided matrix was not square");

            Matrix<T> B = GenerateReverseIdentity<T>(A.ColumnCount);
            return A * B;
        }

        public static Matrix<T> RotateRight<T>(this Matrix<T> A) where T : struct, IEquatable<T>, IFormattable
        {
            return A.Transpose().FlipHorizontal();
        }

        public static Matrix<T> RotateLeft<T>(this Matrix<T> A) where T : struct, IEquatable<T>, IFormattable
        {
            return A.Transpose().FlipVertical();
        }

        public static Matrix<T> RotateAround<T>(this Matrix<T> A) where T : struct, IEquatable<T>, IFormattable
        {
            return A.FlipVertical().FlipHorizontal();
        }
    }
}
