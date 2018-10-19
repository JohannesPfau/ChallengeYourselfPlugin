using System;

namespace ChallengeYourself
{
    public class CY_NN_Backprop : CY_NN
    {
        public double mse;
        
        public CY_NN_Backprop(CY_NeuronLayer inputLayer, CY_NeuronLayer outputLayer)
            : base(inputLayer, outputLayer, CY_Train_Method.Supervised)
        {
            this.mse = 0d;
        }
        
        public override void Learn(CY_NN_Dataset_Entry e, int i, int max_i)
        {
            mse = 0d;
            base.Learn(e, i, max_i);
        }
        
        protected override void OnBeginEpoch(int i, CY_NN_Dataset set)
        {
            mse = 0d;
            base.OnBeginEpoch(i, set);
        }
        
        protected override void OnEndEpoch(int i, CY_NN_Dataset set)
        {
            mse /= set.trainingSamples.Count;
            base.OnEndEpoch(i, set);
        }
        
        protected override void LearnSample(CY_NN_Dataset_Entry e, int currentIteration, int trainingEpochs)
        {
            int layerCount = layers.Count;
            inputLayer.SetInput(e.inputV);
            for (int i = 0; i < layerCount; i++)
                layers[i].calcFire();
            mse += (outputLayer as CY_NeuronLayer).SetErrors(e.outputV);
            for (int i = layerCount; i > 0; )
            {
                CY_NeuronLayer layer = layers[--i] as CY_NeuronLayer;
                if (layer != null)
                    layer.calcError();
            }
            for (int i = 0; i < layerCount; i++)
                layers[i].Learn(currentIteration, trainingEpochs);
        }
    }
}