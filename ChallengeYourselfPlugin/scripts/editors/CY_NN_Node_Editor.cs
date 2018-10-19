
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(CY_NeuralNetworkNode))]
class CY_NN_Node_Editor : Editor
{
    SerializedProperty nnidProp;
    SerializedProperty showPreProp;

    SerializedProperty  initialized;
    SerializedProperty  nrInputs;
    SerializedProperty  nrHiddenLayers;
    SerializedProperty  nrOutputs;
    SerializedProperty  nrHiddenLayerNodes;
    SerializedProperty  nrCalls;
    SerializedProperty  nrTrainings;
    SerializedProperty  performance;

    void OnEnable()
    {
        // Setup the SerializedProperties.
        nnidProp = serializedObject.FindProperty("NeuralNetworkID");
        showPreProp = serializedObject.FindProperty("showPreview");

        initialized = serializedObject.FindProperty("initialized");
        nrInputs = serializedObject.FindProperty("nrInputs");
        nrHiddenLayers = serializedObject.FindProperty("nrHiddenLayers");
        nrOutputs = serializedObject.FindProperty("nrOutputs");
        nrHiddenLayerNodes = serializedObject.FindProperty("nrHiddenLayerNodes");
        nrCalls = serializedObject.FindProperty("nrCalls");
        nrTrainings = serializedObject.FindProperty("nrTrainings");
        performance = serializedObject.FindProperty("performance");
    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        initializeIfNecessary();


        var centeredStyleLabel = GUI.skin.GetStyle("Label");
        centeredStyleLabel.alignment = TextAnchor.UpperCenter;
        var centeredStyleIntField = GUI.skin.GetStyle("textField");
        centeredStyleIntField.alignment = TextAnchor.UpperCenter;
        
        var centeredStyleButton = GUI.skin.GetStyle("Button");
        centeredStyleButton.alignment = TextAnchor.UpperCenter;

        Texture myTexture  = (Texture)Resources.Load("CYInspector_string");
          GUILayout.Label(myTexture);

        if (showPreProp.boolValue)
        {
            visualizeNetwork();
            GUILayout.Label("", centeredStyleLabel);
        }

        showPreProp.boolValue = GUILayout.Toggle(showPreProp.boolValue, "Show Preview");
        nnidProp.intValue = EditorGUILayout.IntField("NN ID: ", nnidProp.intValue);        

        if (GUILayout.Button(new GUIContent("Open in CY-Editor","Opens a new window for editing the Neural Network Node, where you can specify the number of nodes, layers, etc."), centeredStyleButton))
            CY_NN_Window.Init(serializedObject);

        //CY_NeuralNetworkNode cynnn = (CY_NeuralNetworkNode)target;


        serializedObject.ApplyModifiedProperties();
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
                if ((max % 2 == 1 && i == max / 2) || (max % 2 == 0 && i == (max - 1) / 2))
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

    public void initializeIfNecessary()
    {
        if (initialized.boolValue)
            return;

        // standard values:
        nrInputs.intValue = 3;
        nrHiddenLayerNodes.intValue = 5;
        nrHiddenLayers.intValue = 1;
        nrOutputs.intValue = 1;
        nrCalls.intValue = 0;
        nrTrainings.intValue = 0;
        performance.floatValue = 0;
        initialized.boolValue = true;
    }
}

#endif