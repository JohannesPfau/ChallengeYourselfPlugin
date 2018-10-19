using System;
using System.Collections.Generic;

namespace ChallengeYourself
{
    public class CY_NeuronToNeuronBackprop
        : CY_NeuronToNeuronConn<CY_NeuronLayer, CY_NeuronLayer, CY_Synapse_Backprop>
    {
        public double momentum = 0.07d;
        
        public CY_NeuronToNeuronBackprop(CY_NeuronLayer fromLayer, CY_NeuronLayer toLayer)
            : this(fromLayer, toLayer, CY_NeuronToNeuronType.Complete)
        {
        }
        
        public CY_NeuronToNeuronBackprop(CY_NeuronLayer fromLayer, CY_NeuronLayer toLayer, CY_NeuronToNeuronType connectionMode)
            : base(fromLayer, toLayer, connectionMode)
        {
            connect();
        }
        
        public override void init()
        {
            if (initializer != null)
                initializer.init(this);
        }

        private void connect()
        {
            int i = 0;
            if (connectionMode == CY_NeuronToNeuronType.Complete)
                foreach (CY_Neuron n in toLayer.Neurons)
                    foreach (CY_Neuron f in fromLayer.Neurons)
                        synapses[i++] = new CY_Synapse_Backprop(f, n, this);
            else
            {
                IEnumerator<CY_Neuron> from = fromLayer.Neurons.GetEnumerator();
                IEnumerator<CY_Neuron> to = toLayer.Neurons.GetEnumerator();
                while (from.MoveNext() && to.MoveNext())
                    synapses[i++] = new CY_Synapse_Backprop(from.Current, to.Current, this);
            }
        }
    }
}