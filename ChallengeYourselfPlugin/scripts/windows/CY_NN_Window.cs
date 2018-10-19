
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
public class CY_NN_Window : EditorWindow
{
    bool showFunctions;
    bool showStatistics;
    bool showVisualization;

    SerializedObject serializedObject;
    SerializedProperty nnidProp;
    SerializedProperty showPreProp;

    SerializedProperty initialized;
    SerializedProperty nrInputs;
    SerializedProperty nrHiddenLayers;
    SerializedProperty nrOutputs;
    SerializedProperty nrHiddenLayerNodes;
    SerializedProperty nrCalls;
    SerializedProperty nrTrainings;
    SerializedProperty performance;

    // Add menu named "My Window" to the Window menu
    //[MenuItem("Window/ChallengeYourself")]
    public static void Init(SerializedObject serializedObject)
    {
        // Get existing open window or if none, make a new one:
        CY_NN_Window window = (CY_NN_Window)EditorWindow.GetWindow(typeof(CY_NN_Window));
        window.Show();
        
        window.showFunctions = false;
        window.showStatistics = true;
        window.showVisualization = true;

        window.serializedObject = serializedObject;
        window.nnidProp = serializedObject.FindProperty("NeuralNetworkID");
        window.showPreProp = serializedObject.FindProperty("showPreview");

        window.initialized = serializedObject.FindProperty("initialized");
        window.nrInputs = serializedObject.FindProperty("nrInputs");
        window.nrHiddenLayers = serializedObject.FindProperty("nrHiddenLayers");
        window.nrOutputs = serializedObject.FindProperty("nrOutputs");
        window.nrHiddenLayerNodes = serializedObject.FindProperty("nrHiddenLayerNodes");
        window.nrCalls = serializedObject.FindProperty("nrCalls");
        window.nrTrainings = serializedObject.FindProperty("nrTrainings");
        window.performance = serializedObject.FindProperty("performance");


    }

    void OnGUI()
    {
        serializedObject.Update();

        var centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.MiddleCenter;
        var centeredStyleIntField = GUI.skin.GetStyle("textField");
        centeredStyleIntField.alignment = TextAnchor.UpperCenter;

        // HEADER
        Texture myTexture1 = (Texture)Resources.Load("NNHeader");
        GUILayout.Label("");
        GUILayout.Label(myTexture1);
        GUILayout.Label("Welcome to the Neural Network Configuration Editor of the ChallengeYourself Plugin.\r\n");
        GUILayout.Label("NN ID:\t" + nnidProp.intValue,EditorStyles.boldLabel);
        

        // VISUALIZATION
        showVisualization = EditorGUILayout.Foldout(showVisualization, "Visualization");
        if (showVisualization)
            visualizeNetwork();
        GUILayout.Label("");

        // NETWORK SETTINGS
        GUILayout.Label("Network Settings", EditorStyles.boldLabel);
        
        centeredStyle.alignment = TextAnchor.MiddleLeft;
        GUILayout.Label("Specify your network in the following lines.", centeredStyle);
        GUILayout.Label("");

        nrInputs.intValue = EditorGUILayout.IntField("Number of Input Nodes: ", nrInputs.intValue);
        nrHiddenLayerNodes.intValue = EditorGUILayout.IntField("Nodes in Hidden Layer: ", nrHiddenLayerNodes.intValue);
        nrOutputs.intValue = EditorGUILayout.IntField("Number of Output Nodes: ", nrOutputs.intValue);
        GUILayout.Label("");
        nrHiddenLayers.intValue = EditorGUILayout.IntField("Number of Hidden Layers: ", nrHiddenLayers.intValue);
        GUILayout.Label("");
        if (GUILayout.Button(new GUIContent("Save Specifications", "Changes the specifications of the network, such as the number of nodes in the various layers. Keep in mind that the original weights and connections are lost in a new specification.")))
            if (EditorUtility.DisplayDialog("Save Neural Network Specifications", "Do you really want to change the specifications of the Neural Network? This will re-initialize it with randomized weights, so it will forget everything it has learned.", "Reset and Save", "Cancel"))
                save();
        GUILayout.Label("");

        GUILayout.Label("");
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
        GUILayout.Label("");

        // STATISTICS
        //GUILayout.Label("Statistics", EditorStyles.boldLabel);
        showStatistics = EditorGUILayout.Foldout(showStatistics, "Statistics");
        if (showStatistics)
        {
            EditorGUILayout.IntField("Calls: ", nrCalls.intValue);
            EditorGUILayout.IntField("Training: ", nrTrainings.intValue);
            EditorGUILayout.DoubleField("Performance: ", performance.doubleValue);
            GUILayout.Label("");
            if (GUILayout.Button(new GUIContent("Reset Memory", "This will re-initialize the network with randomized weights, which results in a loss of every learned structure.")))
                if (EditorUtility.DisplayDialog("Reset Neural Network Memory", "Do you really want to reset the Neural Network? This will re-initialize it with randomized weights, so it will forget everything it has learned.", "Reset", "Cancel"))
                    reset();
        }

        GUILayout.Label("");
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
        GUILayout.Label("");

        // FUNCTIONS
        showFunctions = EditorGUILayout.Foldout(showFunctions, "Functions");
        var miniStyle = EditorStyles.miniLabel;
        if (showFunctions)
        {
            miniStyle.alignment = TextAnchor.MiddleLeft;
            //GUILayout.Label("Functions", EditorStyles.boldLabel);
            GUILayout.Label("You can call these functions in your scripts to interact with the Plugin in-game.", centeredStyle);
            GUILayout.Label("getNN_ID():\t\t\t  returns the Network ID as int", EditorStyles.miniLabel);
            GUILayout.Label("getNrInputs():\t\t\t  returns the nr of Input nodes as int", EditorStyles.miniLabel);
            GUILayout.Label("getNrHiddenLayers():\t\t  returns the nr of Hidden Layers as int", EditorStyles.miniLabel);
            GUILayout.Label("getNrOutputs():\t\t\t  returns the nr of Output nodes as int", EditorStyles.miniLabel);
            GUILayout.Label("getNrCalls():\t\t\t  returns the nr of Calls as int", EditorStyles.miniLabel);
            GUILayout.Label("getNrTrainings():\t\t\t  returns the nr of Training instances as int", EditorStyles.miniLabel);
            GUILayout.Label("getPerformance():\t\t\t  returns the performance as float", EditorStyles.miniLabel);
            GUILayout.Label("");
            GUILayout.Label("get(float[] inputs):\t\t\t  returns the output as a float array", EditorStyles.miniLabel);
            GUILayout.Label("train(float[] inputs,float[] outputs):\t  Trains the network to target values", EditorStyles.miniLabel);
            GUILayout.Label("");
            GUILayout.Label("reset():\t\t\t\t  Re-initializes the network with random weights", EditorStyles.miniLabel);
        }

        GUILayout.Label("");
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
        GUILayout.Label("");

        // NEVERMIND CREATIONS
        if (GUILayout.Button(new GUIContent("Thank you for using the ChallengeYourself Plugin!", "This will refer you to our homepage http://www.nevermindcreations.de")))
            Application.OpenURL("http://www.nevermindcreations.de");
        centeredStyle.alignment = TextAnchor.UpperCenter;
        Texture nvmcTex = (Texture)Resources.Load("NVMC_string");
        GUILayout.Label(nvmcTex);
        miniStyle.alignment = TextAnchor.LowerRight;
        GUILayout.Label("Johannes Pfau, 2016", miniStyle);


        serializedObject.ApplyModifiedProperties();
    }

    public void reset()
    {
        nrTrainings.intValue = 0;
        nrCalls.intValue = 0;
        performance.floatValue = 0;

        //TODO: also re-initialize weights

        Focus();
    }

    public void save()
    {
        reset();
    }

    public void visualizeNetwork()
    {
        if (nrHiddenLayers.intValue <= 0)
        {
            GUILayout.Label("Network cannot consist of 0 Hidden Layers.\r\nPlease choose a number between 1-5.", EditorStyles.boldLabel);
            return;
        }

        if (nrHiddenLayers.intValue == 1)
        {
            int vis_i = nrInputs.intValue;
            int vis_o = nrOutputs.intValue;
            int vis_hn = nrHiddenLayerNodes.intValue;

            int max = Mathf.Max(vis_i, vis_o, vis_hn);

            int diff_i = (nrInputs.intValue - max) / 2;
            int diff_o = (nrOutputs.intValue - max) / 2;
            int diff_hn = (nrHiddenLayerNodes.intValue - max) / 2;

            int i = 0;

            while (vis_i > 0 || vis_o > 0 || vis_hn > 0)
            {
                string row = "NN";
                if (vis_i > 0 && diff_i >= 0)
                {
                    row += "1";
                    vis_i--;
                }
                else
                {
                    row += "0";
                    diff_i++;
                }


                if (vis_hn > 0 && diff_hn >= 0)
                {
                    row += "1";
                    vis_hn--;
                }
                else
                {
                    row += "0";
                    diff_hn++;
                }

                if (vis_o > 0 && diff_o >= 0)
                {
                    row += "1";
                    vis_o--;
                }
                else
                {
                    row += "0";
                    diff_o++;
                }

                // display row
                if ((max % 2 == 1 && i == max / 2) || (max % 2 == 0 && i == (max-1)/2))
                    row = "NN111Center";
                Texture rowTex = (Texture)Resources.Load(row);
                GUILayout.Label(rowTex);
                i++;
            }
        }

        if (nrHiddenLayers.intValue > 5)
        {
            GUILayout.Label("Network cannot consist of more than 5 Hidden Layers.\r\nPlease choose a number between 1-5.", EditorStyles.boldLabel);
            return;
        }


    }
}
#endif