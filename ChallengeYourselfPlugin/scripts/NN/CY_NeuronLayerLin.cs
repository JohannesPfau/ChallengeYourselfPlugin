using System;

namespace ChallengeYourself
{
    public class CY_NeuronLayerLin : CY_NeuronLayer
    {
        public CY_NeuronLayerLin(int size)
            : base(size)
        {
        }        
        public override double call(double i, double o)
        {
            return i;
        }
        public override double Derivative(double input, double output)
        {
            return 1d;
        }
    }
}
