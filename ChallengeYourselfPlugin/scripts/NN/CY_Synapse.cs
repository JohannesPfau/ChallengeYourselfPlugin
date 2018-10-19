namespace ChallengeYourself
{
    public interface CY_Synapse
    {
        double W { get; set; }
        CY_NeuronToNeuron Parent { get; }
        CY_Neuron fromNeuron { get; }
        CY_Neuron toNeuron { get; }
        void fire();
        void calcW(double learningFactor);
        void noise(double maxnoise);
    }
}