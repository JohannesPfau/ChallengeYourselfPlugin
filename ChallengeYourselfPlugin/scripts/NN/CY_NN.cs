using System;
using System.Collections.Generic;

namespace ChallengeYourself
{
    public abstract class CY_NN
    {
        public CY_Layer<CY_Neuron> inputLayer;
        public CY_Layer<CY_Neuron> outputLayer;
        public IList<CY_Layer<CY_Neuron>> layers;
        public IList<CY_NeuronToNeuron> connectors;
        public CY_Train_Method tm;
        public event CY_Dataset_Epoch_Handler BeginEpochEvent;
        public event CY_Dataset_Entry_Handler BeginSampleEvent;
        public event CY_Dataset_Entry_Handler EndSampleEvent;
        public event CY_Dataset_Epoch_Handler EndEpochEvent;
        public int jitterEpoch;
        public double jitterNoiseLimit;
        public bool isStopping = false;
                        
        public IEnumerable<CY_Layer<CY_Neuron>> Layers
        {
            get
            {
                for(int i = 0; i < layers.Count; i++)
                    yield return layers[i];
            }
        }
        public IEnumerable<CY_NeuronToNeuron> Connectors
        {
            get
            {
                for (int i = 0; i < connectors.Count; i++)
                {
                    yield return connectors[i];
                }
            }
        }

        protected CY_NN(CY_Layer<CY_Neuron> inputLayer, CY_Layer<CY_Neuron> outputLayer, CY_Train_Method tm)
        {
            this.inputLayer = inputLayer;
            this.outputLayer = inputLayer;
            this.tm = tm;
            this.jitterEpoch = 73;
            this.jitterNoiseLimit = 0.03d;
            this.layers = new List<CY_Layer<CY_Neuron>>();
            this.connectors = new List<CY_NeuronToNeuron>();
            Stack<CY_Layer<CY_Neuron>> stack = new Stack<CY_Layer<CY_Neuron>>();
            stack.Push(inputLayer);
            IDictionary<CY_Layer<CY_Neuron>, int> inDegree = new Dictionary<CY_Layer<CY_Neuron>, int>();
            while (stack.Count > 0)
            {
                this.outputLayer = stack.Pop();
                layers.Add(this.outputLayer);
                foreach (CY_NeuronToNeuron connector in this.outputLayer.to)
                {
                    connectors.Add(connector);
                    CY_Layer<CY_Neuron> targetLayer = connector.TargetLayer;
                    inDegree[targetLayer] =
                        inDegree.ContainsKey(targetLayer)
                        ? inDegree[targetLayer] - 1
                        : targetLayer.from.Count - 1;
                    if (inDegree[targetLayer] == 0)
                    {
                        stack.Push(targetLayer);
                    }
                }
            }
            Initialize();
        }
        
        protected virtual void OnBeginEpoch(int currentIteration, CY_NN_Dataset set)
        {
            if (BeginEpochEvent != null)
                BeginEpochEvent(this, new CY_Dataset_Epoch_Args(currentIteration, set));
        }
        
        protected virtual void OnEndEpoch(int currentIteration, CY_NN_Dataset set)
        {
            if (EndEpochEvent != null)
                EndEpochEvent(this, new CY_Dataset_Epoch_Args(currentIteration, set));
        }
        
        protected virtual void OnBeginSample(int currentIteration, CY_NN_Dataset_Entry e)
        {
            if (BeginSampleEvent != null)
                BeginSampleEvent(this, new CY_Dataset_Entry_Args(currentIteration, e));
        }
        protected virtual void OnEndSample(int currentIteration, CY_NN_Dataset_Entry e)
        {
            if (EndSampleEvent != null)
                EndSampleEvent(this, new CY_Dataset_Entry_Args(currentIteration, e));
        }
        public void SetLearningRate(double learningRate)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].learnFunction = new CY_NN_LinLearnFunction(learningRate, learningRate);
            }
        }
        public virtual void Initialize()
        {
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].Initialize();
                foreach (CY_NeuronToNeuron connector in layers[i].to)
                    connector.init();
            }
        }
        
        public virtual double[] Run(double[] input)
        {
            inputLayer.SetInput(input);
            for (int i = 0; i < layers.Count; i++)
                layers[i].calcFire();
            return outputLayer.GetOutput();
        }
        
        public virtual void Learn(CY_NN_Dataset set, int trainingEpochs)
        {
            isStopping = false;
            Initialize();
            for (int currentIteration = 0; currentIteration < trainingEpochs; currentIteration++)
            {
                int[] randomOrder = GetRandomOrder(set.trainingSamples.Count);
                OnBeginEpoch(currentIteration, set);
                
                if (jitterEpoch > 0 && currentIteration % jitterEpoch == 0)
                    for (int i = 0; i < connectors.Count; i++)
                        connectors[i].noise(jitterNoiseLimit);

                for (int index = 0; index < set.trainingSamples.Count; index++)
                {
                    CY_NN_Dataset_Entry e = set[randomOrder[index]];
                    OnBeginSample(currentIteration, e);
                    LearnSample(set[randomOrder[index]], currentIteration, trainingEpochs);
                    OnEndSample(currentIteration, e);
                    if (isStopping) { isStopping = false; return; }
                }
                OnEndEpoch(currentIteration, set);
                if (isStopping) { isStopping = false; return; }
            }
        }
        
        public virtual void Learn(CY_NN_Dataset_Entry e, int i, int max_i)
        {
            OnBeginSample(i, e);
            LearnSample(e, i, max_i);
            OnEndSample(i, e);
        }
        protected abstract void LearnSample(CY_NN_Dataset_Entry e, int i, int max_i);

        public int[] GetRandomOrder(int size)
        {
            int[] randomOrder = new int[size];
            for (int i = 0; i < size; i++)
                randomOrder[i] = i;
            
            for (int i = 0; i < size; i++)
            {
                Random random = new Random();
                int randomPosition = random.Next(size);
                int temp = randomOrder[i];
                randomOrder[i] = randomOrder[randomPosition];
                randomOrder[randomPosition] = temp;
            }
            return randomOrder;
        }
    }
    
}