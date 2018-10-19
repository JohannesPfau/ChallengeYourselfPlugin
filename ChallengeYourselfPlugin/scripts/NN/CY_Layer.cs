using System;
using System.Collections.Generic;

namespace ChallengeYourself
{
    public class CY_Layer<CY_LayerNeuron> where CY_LayerNeuron : CY_Neuron
    {
        public CY_LayerNeuron[] neurons;
        public List<CY_NeuronToNeuron> from = new List<CY_NeuronToNeuron>();
        public List<CY_NeuronToNeuron> to = new List<CY_NeuronToNeuron>();
        public CY_NN_LinLearnFunction learnFunction;
        public CY_Init initializer = null;

        public IEnumerable<CY_LayerNeuron> Neurons
        {
            get
            {
                for (int i = 0; i < neurons.Length; i++)
                    yield return neurons[i];
            }
        }
                                
        protected CY_Layer(int size)
        {            
            this.neurons = new CY_LayerNeuron[size];
            this.learnFunction = new CY_NN_LinLearnFunction(0.3d, 0.05d);
        }
        
        public void SetInput(double[] input)
        {
            for (int i = 0; i < neurons.Length; i++)
                neurons[i].input = input[i];
        }
        
        public double[] GetOutput()
        {
            double[] output = new double[neurons.Length];
            for (int i = 0; i < neurons.Length; i++)
                output[i] = neurons[i].output;
            return output;
        }
        
        public virtual void Initialize()
        {        }
        
        public void calcFire()
        {
            for (int i = 0; i < neurons.Length; i++)
            {
                neurons[i].fire();
            }
        }
        public void Learn(int i, int max_i)
        {
            for (int j = 0; j < neurons.Length; j++)
                neurons[j].Learn(learnFunction.getRate(i, max_i));
        }
    }
}