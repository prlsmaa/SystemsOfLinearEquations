using System;

namespace Lab3
{
    public class Matrix
    {
        //диагональные векторы матрицы
        private double[] A { get; set; }
        private double[] B { get; set; }

        private double[] C { get; set; }

        //векторы, портящие матрицу
        private double[] P { get; set; }

        private double[] Q { get; set; }

        //вектор правой части для сгенерированного вектора Х
        public double[] F { get; set; }
        // вектор правой части для единичного вектора Х
        public double[] F1 { get; set; }

        //решение системы(полученное) 
        public double[] X { get; set; }
        public double[] X1 { get; set; }
        public double[] Xgenerate { get; set; }
        //создание матрицы 
        public Matrix(int n) //N-размерность матрицы, a-начало диапазона чисел, b- конец диапазона
        {
            A = new double[n + 1];
            B = new double[n + 1];
            C = new double[n + 1];
            P = new double[n + 1];
            Q = new double[n + 1];
            X = new double[n + 1];
            Xgenerate = new double[n + 1];
        }

        //создание тестовой матрицы из целочисленных значений
        public void Generate(int n,int a,int b)
        {
            var rnd = new Random();
            var d = rnd.Next(a*10, b*10) * 0.01;
            //зануляем первые элементы, т.к они не будут использоваться 
            A[0] = B[0] = C[0] = P[0] = Q[0] = 0;
            for (var i = 1; i < n + 1; i++)
            {
                A[i] = rnd.Next(a + 1, b - 1) + d;
                d = rnd.Next(a*10, b*10) * 0.01;
                B[i] = rnd.Next(a + 1, b - 1) + d;
                d = rnd.Next(a*10, b*10) * 0.01;
                C[i] = rnd.Next(a+1, b-1) +d;
                d = rnd.Next(a*10, b*10) * 0.01;
                P[i] = rnd.Next(a+1, b-1) +d;
                d = rnd.Next(a*10, b*10) * 0.01;
                Q[i] = rnd.Next(a+1, b-1) +d;
            }

            Q[1] = B[1];
            Q[2] = C[1];
            P[n] = B[n];
            P[n - 1] = A[n - 1];
            C[n] = 0;
            A[n] = 0;
        }

        public void Copy(Matrix m,int n)
        {
            for (var i = 0; i < n + 1; i++)
            {
                A[i] = m.A[i];
                B[i] = m.B[i];
                C[i] = m.C[i];
                P[i] = m.P[i];
                Q[i] = m.Q[i];
                X[i] = 0;
                Xgenerate[i] = 0;
            }
        }

        //Приведение матрицы к диагональному виду
        public bool MatrixSolution(int n) //N-размерность матрицы
        {
            if (StraightRun(n))
            {
                ReverseX1(n);
                ReverseXgenerate(n);
                return true;
            }
            else
            {
                return false;
            }
        }

        //прямой ход
        private bool StraightRun(int n)
        {
            double r;
            //шаг 1
            for (var i = 1; i < n; i++)
            {
                if (B[i] != 0)
                {
                    r = 1 / B[i];
                    B[i] = 1;
                    A[i] *= r;
                    P[i] *= r;
                    F[i] *= r;
                    F1[i] *= r;
                    switch (i)
                    {
                        //обнуляем элемент под B[1]
                        case 1:
                            P[i + 1] -= C[i] * P[i];
                            B[i + 1] -= C[i] * A[i];
                            F[i + 1] -= C[i] * F[i];
                            F1[i + 1] -= C[i] * F1[i];
                            Q[i] = 1;
                            Q[i + 1] = C[i] = 0;
                            break;
                        default:
                        {
                            //обнуляем элементы вектора С
                            Q[i] *= r;
                            P[i + 1] -= C[i] * P[i];
                            B[i + 1] -= C[i] * A[i];
                            Q[i + 1] -= C[i] * Q[i];
                            F[i + 1] -= C[i] * F[i];
                            F1[i + 1] -= C[i] * F1[i];
                            C[i] = 0;
                            if (i == n - 2)
                            {
                                A[i + 1] = P[i + 1]; 
                            }

                            if (i == n - 1)
                            {
                                B[i + 1] = P[i + 1]; 
                            }
                            break;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            //шаг 3. Обнуляем элемент над B[n]
            if (B[n] != 0)
            {
                r = 1 / B[n];
                B[n] = 1;
                P[n] = 1;
                F[n] *= r;
                F1[n] *= r;
                Q[n] *= r;
                Q[n - 1] -= P[n - 1] * Q[n];
                F[n - 1] -= P[n - 1] * F[n];
                F1[n - 1] -= P[n - 1] * F1[n];
                A[n - 1] = P[n - 1] = 0;
            }
            else
            {
                return false;
            }

            //шаг 4. обнуляем верхнюю кодиагональ
            for (var i = n - 2; i > 0; i--)
            {
                P[i] -= A[i] * P[i + 1];
                Q[i] -= A[i] * Q[i + 1];
                F[i] -= A[i] * F[i + 1];
                F1[i] -= A[i] * F1[i + 1];
                A[i] = 0;
            }
            B[1] = Q[1];
            C[1] = Q[2];
            //шаг 5. Обнуляем вектор Q(правый столбец)
            if (B[1] != 0)
            {
                r = 1 / B[1];
                B[1] = 1;
                Q[1] = 1;
                P[1] *= r;
                F[1] *= r;
                F1[1] *= r;
                for (var i = 2; i < n + 1; i++)
                {
                    P[i] -= Q[i] * P[1];
                    F[i] -= Q[i] * F[1];
                    F1[i] -= Q[i] * F1[1];
                    Q[i] = 0;
                    if (i == 2)
                    {
                        C[i - 1] = 0;
                    }
                }

                
                B[n] = P[n];
                A[n - 1] = P[n - 1];
            }
            else
            {
                return false;
            }

            //шаг 6. Обнуляем вектор P(левый столбец)
            if (B[n] != 0)
            {
                r = 1 / B[n];
                B[n] = P[n] = 1;
                F[n] *= r;
                F1[n] *= r;
                for (var i = n - 1; i > 0; i--)
                {
                    F[i] -= P[i] * F[n];
                    F1[i] -= P[i] * F1[n];
                    P[i] = 0;
                    if (i == n - 1)
                    {
                        A[n - 1] = P[n - 1];
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        //Обратный ход
        private void ReverseX1(int n)
        {
            X1 = new double[n + 1];
            for (int i = 1; i < n+1; i++)
            {
                X1[i] = F1[n - (i - 1)];
            }
        }
        private void ReverseXgenerate(int n)
        {
            X = new double[n + 1];
            for (var i = 1; i < n+1; i++)
            {
                X[i] = F[n - (i - 1)];
            }
        }
        //Точность решения
        public double AccuracyAssessment(int n)
        {
            var maxX = Math.Abs(X1[1] - 1);
            for (var j = 2; j < n + 1; j++)
            {
                var currentDif = Math.Abs(X1[j] - 1);
                if (currentDif > maxX)
                {
                    maxX = currentDif;
                }
            }

            return maxX;
        }

        public double RelativeError(int n)
        {
            double maxError = 0;
            for (var j = 2; j < n + 1; j++)
            {
                var currentError = Xgenerate[j] > Double.Epsilon
                    ? Math.Abs((X[j] - Xgenerate[j]) / Xgenerate[j])
                    : Math.Abs((X[j] - Xgenerate[j]));

                if (currentError > maxError)
                {
                    maxError = currentError;
                }
            }

            return maxError;
        }

        public void GenerateFx1(int n)
        {
            //для единичного вектора
            F1 = new double[n + 1];
                F1[0] = 0;
                F1[1] = P[1] + A[1] + Q[1];
                F1[2] = P[2] + A[2] + B[2] + Q[2];
                for (var i = 3; i < n - 1; i++)
                {
                    F1[i] = P[i] + A[i] + B[i] + C[i - 1] + Q[i];
                }

                F1[n - 1] = P[n - 1] + B[n - 1] + C[n - 2] + Q[n - 1];
                F1[n] = P[n] + C[n - 1] + Q[n];
            }

        public void GenerateFxGenerate(int n, int a, int b)
        {
            //для сгенерированного вектора
            var rnd = new Random();
            for (var i = 1; i < n + 1; i++)
            {
                var d = rnd.Next(a*10, b*10) * 0.01;
                Xgenerate[i] = rnd.Next(a + 1, b - 1) + d;
            }

            F = new double[n + 1];
            F[0] = 0;
            F[1] = P[1] * Xgenerate[1] + A[1] * Xgenerate[n - 1] + Q[1] * Xgenerate[n];
            F[2] = P[2] * Xgenerate[1] + A[2] * Xgenerate[n - 2] + B[2] * Xgenerate[n - 1] + Q[2] * Xgenerate[n];
            for (int i = 3; i < n - 1; i++)
            {
                F[i] = P[i] * Xgenerate[1] + A[i] * Xgenerate[n - i] + B[i] * Xgenerate[n - i+1] + C[i-1] * Xgenerate[n- i+2] + Q[i] * Xgenerate[n];
            }

            F[n - 1] = P[n - 1] * Xgenerate[1] + B[n - 1] * Xgenerate[2] + C[n - 2] * Xgenerate[3] + Q[n - 1] * Xgenerate[n];
            F[n] = P[n] * Xgenerate[1] + C[n-1] * Xgenerate[2] + Q[n] * Xgenerate[n];
        }

        public void Print(int n)
        {
            for (var i = 1; i < n + 1; i++)
            {
                switch (i)
                {
                    case 1:
                    {
                        Console.Write(P[i] + "\t");
                        for (var j = 1; j < n - (i + 1); j++)
                        {
                            Console.Write(0 + "\t");
                        }

                        Console.Write(A[i] + "\t");
                        Console.Write(B[i] + "\t");
                        Console.Write("\t   " + F1[i]);
                        Console.Write("\t   " + F[i]);
                        Console.WriteLine();
                        break;
                    }
                    case 2:
                    {
                        Console.Write(P[i] + "\t");
                        for (var j = 1; j < n - (i + 1); j++)
                        {
                            Console.Write(0 + "\t");
                        }

                        Console.Write(A[i] + "\t");
                        Console.Write(B[i] + "\t");
                        Console.Write(Q[i] + "\t");
                        Console.Write("\t  " + F1[i]);
                        Console.Write("\t  " + F[i]);
                        Console.WriteLine();
                        break;
                    }
                    default:
                    {
                        if (i == n - 1)
                        {
                            Console.Write(A[i] + "\t");
                            Console.Write(B[i] + "\t");
                            Console.Write(C[i - 1] + "\t");
                            for (var j = 1; j < n - 3; j++)
                            {
                                Console.Write(0 + "\t");
                            }

                            Console.Write(Q[i] + "\t");
                            Console.Write("\t  " + F1[i]);
                            Console.Write("\t  " + F[i]);
                            Console.WriteLine();
                        }
                        else
                        {
                            if (i == n)
                            {
                                Console.Write(B[i] + "\t");
                                Console.Write(C[i - 1] + "\t");
                                for (var j = 1; j < n - 2; j++)
                                {
                                    Console.Write(0 + "\t");
                                }

                                Console.Write(Q[i] + "\t");
                                Console.Write("\t  " + F1[i]);
                                Console.Write("\t  " + F[i]);
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.Write(P[i] + "\t");
                                for (var j = 1; j < n - (i + 1); j++)
                                {
                                    Console.Write(0 + "\t");
                                }

                                Console.Write(A[i] + "\t");
                                Console.Write(B[i] + "\t");
                                Console.Write(C[i - 1] + "\t");
                                for (var j = 1; j < i - 2; j++)
                                {
                                    Console.Write(0 + "\t");
                                }

                                Console.Write(Q[i] + "\t");
                                Console.Write("\t  " + F1[i]);
                                Console.Write("\t  " + F[i]);
                                Console.WriteLine();
                            }
                        }

                        break;
                    }
                }
            }
        }

        public void PrintVectors(int n)
        {
            Console.WriteLine("P\tA\tB\tC\tQ\t");
            for (var i = 1; i < n + 1; i++)
            {
                Console.WriteLine(P[i]+"\t"+A[i]+"\t"+B[i]+"\t"+C[i]+"\t"+Q[i]);
            }
        }
    }
}