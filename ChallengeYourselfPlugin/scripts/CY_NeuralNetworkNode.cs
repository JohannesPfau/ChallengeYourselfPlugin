using UnityEngine;
using System.Collections;
using ChallengeYourself;

public class CY_NeuralNetworkNode : MonoBehaviour {

    public int NeuralNetworkID;
    public bool showPreview;

    public bool initialized;
    public int nrInputs;
    public int nrHiddenLayers;
    public int nrOutputs;
    public int nrHiddenLayerNodes;
    public int nrCalls;
    public int nrTrainings;
    public double performance;

    CY_NN_Backprop network;

    // Update is called once per frame
    void Update () {

    }


    void Start()
    {
        Debug.Log("Trying to build Neural Network, ID: " + NeuralNetworkID);

        if (nrInputs < 1) { Debug.Log("ERROR: Number of inputs has to be >= 1"); return; }
        if (nrHiddenLayers < 1) { Debug.Log("ERROR: Number of hidden layers has to be >= 1"); return; }
        if (nrOutputs < 1) { Debug.Log("ERROR: Number of outputs has to be >= 1"); return; }
        if (nrHiddenLayerNodes < 1) { Debug.Log("ERROR: Number of hidden layer nodes has to be >= 1"); return; }

        //nn = new CY_NeuralNetwork(nrInputs,nrHiddenLayers,nrOutputs);
    }

    public int getNN_ID()
    {
        return NeuralNetworkID;
    }
    public int getNrOfInputs()
    {
        return nrInputs;
    }
    public int getNrOfHiddenLayers()
    {
        return nrHiddenLayers;
    }
    public int getNrOfOutputs()
    {
        return nrOutputs;
    }
    public int getNrOfHiddenLayerNodes()
    {
        return nrHiddenLayerNodes;
    }
    public int getNrOfCalls()
    {
        return nrCalls;
    }
    public int getNrOfTrainings()
    {
        return nrTrainings;
    }
    public double getPerformance()
    {
        return performance;
    }

    public float[] get(float[] inputs)
    {
        return null;
    }
    public void train(double[][] inputs)
    {
        train(inputs, 1000, 0.05, 0.01);   // standard values
    }

    public void train(double[][] inputs, int maxEpochs, double learnRate, double momentum)
    {
        //nn.Train(inputs, maxEpochs, learnRate, momentum);

        //performance = nn.Accuracy(inputs);
        //Debug.Log("Trained with accuracy: " + performance);
    }

    public void train(CY_NN_Dataset set)
    {
        CY_NeuronLayerLin inputLayer = new CY_NeuronLayerLin(nrInputs);
        CY_NeuronLayerTanh hiddenLayer = new CY_NeuronLayerTanh(nrHiddenLayerNodes);
        CY_NeuronLayerLin outputLayer = new CY_NeuronLayerLin(nrOutputs);
        new CY_NeuronToNeuronBackprop(inputLayer, hiddenLayer).initFunction = new CY_Init_RND(0d, 0.3d);
        new CY_NeuronToNeuronBackprop(hiddenLayer, outputLayer).initFunction = new CY_Init_RND(0d, 0.3d);
        network = new CY_NN_Backprop(inputLayer, outputLayer);
        network.SetLearningRate(0.1);
        network.Learn(set, 1000);
        nrTrainings++;
    }

    public void reset()
    {

    }

    public double[] run(double[] input)
    {
        var res = network.Run(input);
        performance = network.mse;
        nrCalls++;
        return res;
    }
}
