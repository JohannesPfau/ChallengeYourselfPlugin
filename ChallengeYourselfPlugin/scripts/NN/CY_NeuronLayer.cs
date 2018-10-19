using System;

namespace ChallengeYourself
{
    public abstract class CY_NeuronLayer : CY_Layer<CY_Neuron>
    {
        protected CY_NeuronLayer(int size) : base(size)
        {
            for (int i = 0; i < size; i++)
                neurons[i] = new CY_Neuron(this);
        }
        
        public override void Initialize()
        {
            if (initializer != null)
                initializer.init(this);
        }
        
        public double SetErrors(double[] expectedOutput)
        {
            double mse = 0d;
            for (int i = 0; i < neurons.Length; i++)
            {
                neurons[i].error = expectedOutput[i] - neurons[i].output;
                mse += neurons[i].error * neurons[i].error;
            }
            return mse;
        }
        
        public void calcError()
        {
            for (int i = 0; i < neurons.Length; i++)
                neurons[i].calcError();
        }
        
        public abstract double call(double i, double o);

        public abstract double Derivative(double input, double output);
    }
}
