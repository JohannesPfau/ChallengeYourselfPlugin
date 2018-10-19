using System;

namespace ChallengeYourself
{
    public class CY_NeuronLayerTanh : CY_NeuronLayer
    {
        public CY_NeuronLayerTanh(int neuronCount)
            : base(neuronCount)
        {
            this.initializer = new CY_Init_Tanh(1);
        }
        
        public override double call(double i, double o)
        {
            return Math.Tanh(i);
        }
        
        public override double Derivative(double i, double o)
        {
            return (1 - (o * o));
        }
    }
}
