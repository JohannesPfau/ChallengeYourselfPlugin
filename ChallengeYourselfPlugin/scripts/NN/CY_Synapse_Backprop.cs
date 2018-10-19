namespace ChallengeYourself
{
    public class CY_Synapse_Backprop : CY_Synapse
    {
        public double w;
        public double delta;
        public CY_Neuron fromNeuron;
        public CY_Neuron toNeuron;
        public CY_NeuronToNeuronBackprop parent;
        
        CY_Neuron CY_Synapse.fromNeuron
        {
            get { return fromNeuron; }
        }

        CY_Neuron CY_Synapse.toNeuron
        {
            get { return toNeuron; }
        }
        
        public double W
        {
            get { return w; }
            set { w = value; }
        }

        CY_NeuronToNeuron CY_Synapse.Parent
        {
            get { return parent; }
        }
        
        public CY_Synapse_Backprop(
            CY_Neuron sourceNeuron, CY_Neuron toNeuron, CY_NeuronToNeuronBackprop parent)
        {
            this.w = 1f;
            this.delta = 0f;
            sourceNeuron.neurons_out.Add(this);
            toNeuron.neurons_in.Add(this);
            this.fromNeuron = sourceNeuron;
            this.toNeuron = toNeuron;
            this.parent = parent;
        }
        
        public void fire()
        {
            toNeuron.input += fromNeuron.output * this.w;
        }
        public void calcW(double learningFactor)
        {
            delta = delta * parent.momentum + learningFactor * toNeuron.error * fromNeuron.output;
            w += delta;
        }
        public void backfire()
        {
            fromNeuron.error += toNeuron.error * this.w;
        }
        
        public void noise(double maxnoise)
        {
        }
    }
}