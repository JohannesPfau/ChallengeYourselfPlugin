using System;

namespace ChallengeYourself
{
    public class CY_Init_RND : CY_Init
    {
        public double min;
        public double max;        
                
        public CY_Init_RND(double min, double max)
        {
            this.min = min;
            this.max = max;
        }
        
        public void init(CY_NeuronLayer CY_NeuronLayer)
        {
            foreach (CY_Neuron neuron in CY_NeuronLayer.Neurons)
                neuron.bias = GetRandom(min, max);
        }
        
        public void init(CY_NeuronToNeuronBackprop connector)
        {
            foreach (CY_Synapse_Backprop synapse in connector.Synapses)
                synapse.W = GetRandom(min, max);
        }

        internal static double GetRandom(double min, double max)
        {
            if (min > max)
                return GetRandom(max, min);
            Random random = new Random();
            return (min + (max - min) * random.NextDouble());
        }
    }
}
