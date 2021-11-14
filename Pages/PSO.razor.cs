using BlazorOptimization.Optimization.PSO;
using org.mariuszgromada.math.mxparser;

namespace BlazorOptimization.Pages
{
    public partial class PSO
    {
        private string Function { get; set; } = "";

        private int Iterations { get; set; } = 1000;

        private int Particles { get; set; } = 30;

        private double[] SolutionVector { get; set; }

        private double SolutionValue { get; set; }

        private string Objective { get; set; } = "min";

        private TimeSpan ComputationTime;

        private void Optimize()
        {            
            if(string.IsNullOrWhiteSpace(Function))
            {
                Function = "F(x1,x2) = (x1 - 10)^2 + (x2 + 5)^2";
                return;
            }
            var start = DateTime.Now;
           
                var function = new Function(Function);
                SolutionVector = PSOptimizer.Optimize(function, Objective, Particles, Iterations);
                SolutionValue = function.calculate(SolutionVector);
                ComputationTime = DateTime.Now - start;
            
        }
    }
}
