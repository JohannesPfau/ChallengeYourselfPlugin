using System;
using System.Collections.Generic;


namespace ChallengeYourself
{
    public abstract class CY_NeuronToNeuronConn<FromLayer, ToLayer, TSynapse> : CY_NeuronToNeuron
        where FromLayer : CY_Layer<CY_Neuron>
        where ToLayer : CY_Layer<CY_Neuron>
        where TSynapse : CY_Synapse
    {
        public FromLayer fromLayer;
        public ToLayer toLayer;
        public TSynapse[] synapses;
        public CY_NeuronToNeuronType connectionMode;
        public CY_Init initializer;

        public IEnumerable<TSynapse> Synapses
        {
            get
            {
                for (int i = 0; i < synapses.Length; i++)
                {
                    yield return synapses[i];
                }
            }
        }
        

        CY_Layer<CY_Neuron> CY_NeuronToNeuron.TargetLayer
        {
            get { return toLayer; }
        }

        IEnumerable<CY_Synapse> CY_NeuronToNeuron.Synapses
        {
            get
            {
                for (int i = 0; i < synapses.Length; i++)
                {
                    yield return synapses[i];
                }
            }
        }
        
        public CY_NeuronToNeuronType CY_NeuronToNeuronType
        {
            get { return connectionMode; }
        }
        public CY_Init initFunction
        {
            get { return initializer; }
            set { initializer = value; }
        }
        protected CY_NeuronToNeuronConn(FromLayer fromLayer, ToLayer toLayer, CY_NeuronToNeuronType connectionMode)
        {
            toLayer.from.Add(this);
            fromLayer.to.Add(this);
            this.fromLayer = fromLayer;
            this.toLayer = toLayer;
            this.connectionMode = connectionMode;
            this.initializer = new CY_Init_Tanh(1);
            switch (connectionMode)
            {
                case CY_NeuronToNeuronType.Complete:
                    synapses = new TSynapse[fromLayer.neurons.Length * toLayer.neurons.Length];
                    break;
                case CY_NeuronToNeuronType.OneOne:
                    if (fromLayer.neurons.Length == toLayer.neurons.Length)
                    {
                        synapses = new TSynapse[fromLayer.neurons.Length];
                        break;
                    }
                break;
            }
        }
        
        public void noise(double maxnoise)
        {
            for (int i = 0; i < synapses.Length; i++)
                synapses[i].noise(maxnoise);
        }

        public IEnumerable<TSynapse> GetSourceSynapses(CY_Neuron neuron)
        {
            foreach (TSynapse synapse in neuron.neurons_in)
                if (synapse.Parent == this)
                    yield return synapse;
        }
        
        public IEnumerable<TSynapse> GetTargetSynapses(CY_Neuron neuron)
        {
            foreach (TSynapse synapse in neuron.neurons_out)
                if (synapse.Parent == this)
                    yield return synapse;
        }
        
        public abstract void init();
    }
}