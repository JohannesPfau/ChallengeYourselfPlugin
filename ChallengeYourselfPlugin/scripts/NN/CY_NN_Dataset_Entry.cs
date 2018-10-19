using System;

namespace ChallengeYourself
{
    public class CY_NN_Dataset_Entry
    {
        public double[] inputV;
        public double[] outputV;
        public double[] inputVnorm;
        public double[] outputVnorm;
        
        public double[] NormalizedInputVector
        {
            get { return inputVnorm; }
        }
        
        public double[] NormalizedOutputVector
        {
            get { return outputVnorm; }
        }
        
        public CY_NN_Dataset_Entry(double[] vector)
            : this(vector, new double[0])
        {
        }
        
        public CY_NN_Dataset_Entry(double[] inputVector, double[] outputVector)
        {
            this.inputV = (double[])inputVector.Clone();
            this.outputV = (double[])outputVector.Clone();
            this.inputVnorm = norm(inputVector, 1);
            this.outputVnorm = norm(outputVector, 1);
        }

        public static double[] norm(double[] vector, double magnitude)
        {
            double factor = 0d;
            for (int i = 0; i < vector.Length; i++)
            {
                factor += vector[i] * vector[i];
            }
            
            double[] normalizedVector = new double[vector.Length];
            if (factor != 0)
            {
                factor = Math.Sqrt(magnitude / factor);
                for (int i = 0; i < normalizedVector.Length; i++)
                {
                    normalizedVector[i] = vector[i] * factor;
                }
            }
            return normalizedVector;
        }
        
    }
}
