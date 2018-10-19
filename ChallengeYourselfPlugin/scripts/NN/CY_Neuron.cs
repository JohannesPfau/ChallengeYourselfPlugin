using System.Collections.Generic;

namespace ChallengeYourself
{
    public class CY_Neuron
    {
        public double input;
        public double output;
        public double error;
        public double bias;

        public IList<CY_Synapse> neurons_in = new List<CY_Synapse>();
        public IList<CY_Synapse> neurons_out = new List<CY_Synapse>();

        public CY_NeuronLayer parent;

        public CY_Neuron(CY_NeuronLayer parent)
        {
            this.input = 0d;
            this.output = 0d;
            this.error = 0d;
            this.bias = 0d;
            this.parent = parent;
        }
        
        public void fire()
        {
            if (neurons_in.Count > 0)
            {
                input = 0d;
                for (int i = 0; i < neurons_in.Count; i++)
                    neurons_in[i].fire();
            }
            output = parent.call(bias + input, output);
        }
        
        public void calcError()
        {
            if (neurons_out.Count > 0)
            {
                error = 0d;
                foreach (CY_Synapse_Backprop synapse in neurons_out)
                    synapse.backfire();
            }
            error *= parent.Derivative(input, output);
        }
        
        public void Learn(double learningRate)
        {
            bias += learningRate * error;
            for (int i = 0; i < neurons_in.Count; i++)
                neurons_in[i].calcW(learningRate);
        }
    }
}
