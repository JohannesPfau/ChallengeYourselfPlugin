using System.Collections.Generic;

namespace ChallengeYourself
{
    public interface CY_NeuronToNeuron
    {
        CY_Layer<CY_Neuron> TargetLayer { get; }
        IEnumerable<CY_Synapse> Synapses { get; }
        CY_NeuronToNeuronType CY_NeuronToNeuronType { get; }
        CY_Init initFunction { get; set; }
        void init();
        void noise(double maxnoise);
    }
}