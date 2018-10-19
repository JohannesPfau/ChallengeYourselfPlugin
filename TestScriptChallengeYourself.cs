using UnityEngine;
using System.Linq;
using ChallengeYourself;

/*
    Sample script for the NN-plugin of the ChallengeYourself plugin.
    You can access the configuration as in the example shown below or
    via the engine interface.

    To use the engine plugin, simply drag and drop the prefab to the
    GameObjects of the current scene.
    */

public class TestScriptChallengeYourself : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        double[] CYNN_Input = new double[] { 1,2,3,4,5,6,7 };
        double[] CYNN_Output = new double[] { -4, -2, 0, 2, 4, 6, 8 };

        // scaling:
        double min = CYNN_Output.Min();
        for (int i = 0; i < CYNN_Output.Length; i++)
            CYNN_Output[i] = CYNN_Output[i] - min;
        double max = CYNN_Output.Max();
        for(int i = 0; i < CYNN_Output.Length; i++)
            CYNN_Output[i] = CYNN_Output[i] / max;
        CY_NN_Dataset set = new CY_NN_Dataset();

        for (int i = 0; i < CYNN_Input.Count(); i++)
            set.trainingSamples.Add(new CY_NN_Dataset_Entry(new double[] { CYNN_Input[i] }, new double[] { CYNN_Output[i] }));


        int variant = 2;

        // Variant 1:
        if (variant == 1)
        {
            // Train:
            CY_NN_Backprop network;
            CY_NeuronLayerLin inputLayer = new CY_NeuronLayerLin(1);
            CY_NeuronLayerTanh hiddenLayer = new CY_NeuronLayerTanh(10);
            CY_NeuronLayerTanh hiddenLayer2 = new CY_NeuronLayerTanh(10);
            CY_NeuronLayerTanh hiddenLayer3 = new CY_NeuronLayerTanh(10);
            CY_NeuronLayerLin outputLayer = new CY_NeuronLayerLin(1);
            new CY_NeuronToNeuronBackprop(inputLayer, hiddenLayer).initFunction = new CY_Init_RND(0d, 0.3d);
            new CY_NeuronToNeuronBackprop(hiddenLayer, hiddenLayer2).initFunction = new CY_Init_RND(0d, 0.3d);
            new CY_NeuronToNeuronBackprop(hiddenLayer2, hiddenLayer3).initFunction = new CY_Init_RND(0d, 0.3d);
            new CY_NeuronToNeuronBackprop(hiddenLayer3, outputLayer).initFunction = new CY_Init_RND(0d, 0.3d);
            network = new CY_NN_Backprop(inputLayer, outputLayer);
            network.SetLearningRate(0.1);            

            network.Learn(set, 1000);

            // Test:
            for (double xVal = 1; xVal < 8; xVal++)
            {
                var res = (network.Run(new double[] { xVal })[0]);

                res = res * max;
                res = res + min;
                Debug.Log(xVal + ": " + res);
            }
            Debug.Log("error: " + network.mse);
        }

        // -------------------------------------------------------------

        // Variant 2 (Node has to be configured in the current Unity scene)
        // Click on the "CY_ Neural Network Node" prefab once it is in the scene, then "Open in CY-Editor"
        // Try e.g. a 1-10-1 network for the input data above
        if (variant == 2)
        {
            // Train:
            CY_NeuralNetworkNode NNNode = GameObject.Find("CY_ Neural Network Node").GetComponent<CY_NeuralNetworkNode>();
            NNNode.train(set);

            // Test:
            for (double xVal = 1; xVal < 8; xVal++)
            {
                var res = (NNNode.run(new double[] { xVal })[0]);

                res = res * max;
                res = res + min;
                Debug.Log(xVal + ": " + res);
            }
            Debug.Log("error: " + NNNode.getPerformance());

            // Statistics like error/performance, the number of calls or trainings etc are also displayed in the "CY-Editor".
        }
    }

}
