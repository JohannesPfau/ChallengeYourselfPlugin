using System;

namespace ChallengeYourself
{
    public class CY_Init_Tanh : CY_Init
    {
        public double outputRange;
        
        public CY_Init_Tanh(double outputRange)
        {
            this.outputRange = outputRange;
        }
        
        public void init(CY_NeuronLayer CY_NeuronLayer)
        {
            int hiddenNeuronCount = 0;
            foreach (CY_NeuronToNeuron targetConnector in CY_NeuronLayer.to)
                    hiddenNeuronCount += targetConnector.TargetLayer.neurons.Length;

            double factor = calcNguyenWidrow(CY_NeuronLayer.neurons.Length, hiddenNeuronCount);

            foreach (CY_Neuron neuron in CY_NeuronLayer.Neurons)
                neuron.bias = GetRandom(-factor, factor);
        }
        
        public void init(CY_NeuronToNeuronBackprop connector)
        {            
            double nGuyenWidrowFactor = calcNguyenWidrow(connector.fromLayer.neurons.Length, connector.toLayer.neurons.Length);

            int synapsesPerNeuron = connector.synapses.Length / connector.toLayer.neurons.Length;

            foreach (CY_Neuron neuron in connector.toLayer.Neurons)
            {
                int i = 0;
                double[] normalizedVector = GetRandomVector(synapsesPerNeuron, nGuyenWidrowFactor);
                foreach (CY_Synapse_Backprop synapse in connector.GetSourceSynapses(neuron))
                    synapse.W = normalizedVector[i++];
            }
        }
        
        private double calcNguyenWidrow(int i, int h)
        {
            return 0.7d * Math.Pow(h, (1d / i)) / outputRange;
        }

        internal static double[] GetRandomVector(int count, double magnitude)
        {
            double[] result = new double[count];
            Random random = new Random();
            for (int i = 0; i < count; i++)
                result[i] = random.NextDouble();
            return Normalize(result, magnitude);
        }

        internal static double[] Normalize(double[] vector, double magnitude)
        {
            double factor = 0d;
            for (int i = 0; i < vector.Length; i++)
                factor += vector[i] * vector[i];
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

        internal static double GetRandom(double min, double max)
        {
            if (min > max)
            {
                return GetRandom(max, min);
            }
            Random random = new Random();
            return (min + (max - min) * random.NextDouble());
        }
    }
}