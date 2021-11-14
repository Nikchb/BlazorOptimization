using org.mariuszgromada.math.mxparser;
using System.Numerics;

namespace BlazorOptimization.Optimization.PSO
{
    public class PSOptimizer
    {
        private const double MaxValue = 10000;
        public static double[] Optimize(
            Function function,
            string objective = "min",
            int n = 30,
            int iterations = 1000,
            double f1 = 2,
            double f2 = 2,
            double k = 0.95,
            double[]? minX = null,
            double[]? maxX = null)
        {
            int d = function.getArgumentsNumber();
            if (minX == null) minX = GenerateVector(d, (value, i) => -1 * MaxValue);
            if (maxX == null) maxX = GenerateVector(d, (value, i) => MaxValue);
            var rand = new Random();
            var x = new double[n][]; 
            var v = new double[n][];
            var r = new double[n];
            var px = new double[n][];
            var pr = new double[n];
            var gbest = 0;
            pr[gbest] = objective == "min" ? double.MaxValue : double.MinValue;
            for (int j = 0; j < n; j++)
            {
                x[j] = GenerateVector(d, (value, i) => rand.NextDouble() * (maxX[i] - minX[i]) + minX[i]);
                v[j] = GenerateVector(d, (value, i) => (rand.NextDouble() * (maxX[i] - minX[i]) + minX[i]) / 10);
                r[j] = function.calculate(x[j]);
                px[j] = new double[d];
                Array.Copy(x[j], px[j], x[j].Length);
                pr[j] = r[j];
                gbest = IsBetter(objective, pr[j], pr[gbest]) ? j : gbest;
            }
            var xi = f1 + f2 >= 4 ? 2 * k / (f1 + f2 - 2 + Math.Sqrt(Math.Pow(f1 + f2, 2) - 4)) : k;
            for (int i = 0; i < iterations; i++)
            {
                for(int j = 0; j<n; j++)
                {
                    for(int l = 0; l < d; l++)
                    {
                        v[j][l] = xi * (v[j][l] + f1 * rand.NextDouble() * (px[j][l] - x[j][l]) + f2 * rand.NextDouble() * (px[gbest][l] - x[j][l]));
                        x[j][l] += v[j][l]; 
                    }
                    r[j] = function.calculate(x[j]);
                    if(IsBetter(objective, r[j], pr[j]))
                    {
                        pr[j] = r[j];
                        Array.Copy(x[j], px[j], d);
                        gbest = IsBetter(objective, pr[j], pr[gbest]) ? j : gbest;
                    }                    
                }
            }
            return px[gbest];
        }
        private static bool IsBetter(string objective, double value, double best)
        {
            if (objective == "min")
            {
                return value < best;
            }
            else
            {
                return value > best;
            }
        }
        private static double[] GenerateVector(int lenght, Func<double, int, double> func) 
        {
            var vector = new double[lenght];    
            for(int i=0; i< vector.Length; i++)
            {
                vector[i] = func(vector[i], i);
            }
            return vector;
        }
    }
}
