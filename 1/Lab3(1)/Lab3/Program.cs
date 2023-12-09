using System;

namespace Lab3
{
  internal static class Program
  {
    public static void Main(string[] args)
    {
      //Часть для демострации работы программы
      Console.WriteLine("Введите размерность системы: ");
      int n = Convert.ToInt16(Console.ReadLine());
      Console.WriteLine("Введите левую и правую границу: ");
      int a = Convert.ToInt16(Console.ReadLine());
      int b = Convert.ToInt16(Console.ReadLine());
      //создадим матрицу, сгенерировав векторы с рандомными значениями
      var m=new Matrix(n);
      m.Generate(n,a,b);
      Console.WriteLine();
      //Создадим вектор F на основе сгенерированного вектора X
      m.GenerateFx1(n);
      m.GenerateFxGenerate(n,a,b);
      m.Print(n);
      if (m.MatrixSolution(n))
      {
        Console.WriteLine("IER 0. Ошибок нет.");
        Console.WriteLine("Решение полученной системы и сгенерированный вектор из 1 имеют вид: ");
        Console.WriteLine("X");
        for (var i = 1; i < n + 1; i++)
        {
          Console.WriteLine(m.X1[i]+"\t"+1);
        }
        Console.WriteLine();
        Console.WriteLine("Решение полученной системы и сгенерированный вектор имеют вид: ");
        Console.WriteLine("X");
        for (var i = 1; i < n + 1; i++)
        {
          Console.WriteLine(m.X[i]+"\t"+m.Xgenerate[i]);
        }
      }
      else
      {
        Console.WriteLine("IER 1. Встречено деление на 0!");
      }
      Console.WriteLine();
      Console.WriteLine("Точность полученного решения: delta="+m.AccuracyAssessment(n));
      Console.WriteLine("Относительная погрешность полученного решения: rel_delta="+m.RelativeError(n));
      //Часть для тестов 
      /*int k = 5;
      int r1 = -3;
      int r2 = 3;
      for (int j = 0; j < 3; j++)
      {
        k *= 10;
        for (int l = 0; l < 3; l++)
        {
          r1 *= 10;
          r2 *= 10;
          double mean = 0;
          double meanError = 0;
          int s = k;
          int sError = k;
          int a = 0;
          for (var i = 0; i < 10; i++)
          {
            var test = new Matrix(k);
            var test1 = test;
            test.Generate(k, r1, r2);
            
            test.GenerateFx1(k);
              if (test.MatrixSolution(k))
              {
                mean += test.AccuracyAssessment(k);
              }
              else
              {
                s--;
              }
              test1.GenerateFxGenerate(k, r1, r2);
              if (test1.MatrixSolution(k))
              {
                meanError += test1.RelativeError(k);
              }
              else
              {
                sError--;
              }

            }

          mean = mean / s;
          meanError = meanError / sError;
          Console.WriteLine("Средняя точность решения системы размерности " + k + " в диапазоне (" + r1 + ";" + r2 +
                            ") равна " + mean);
          Console.WriteLine("Средняя относительная погрешность системы размерности " + k + " в диапазоне (" + r1 + ";" +
                            r2 + ") равна " + meanError);
          Console.WriteLine();
          mean = 0;
          meanError = 0;
          if (l == 2)
          {
            r1 = -3;
            r2 = 3;
          }
        }
      }
*/
      Console.ReadKey();
    }
  }
}
