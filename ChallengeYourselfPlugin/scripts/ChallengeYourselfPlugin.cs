
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Collections;

public class ChallengeYourselfPlugin : Editor {
    
    // Add a menu item with multiple levels of nesting

    [MenuItem("ChallengeYourself/Show All/Neural Networks")]
    private static void CreateNN()
    {

    }
    [MenuItem("ChallengeYourself/Show All/Hidden Markov Models")]
    private static void CreateHMM()
    {

    }
    [MenuItem("ChallengeYourself/Show All/Reinforcement Learner")]
    private static void CreateRL()
    {

    }

    [MenuItem("ChallengeYourself/Options")]
    private static void Options()
    {

    }

    [MenuItem("ChallengeYourself/Reset All Learners")]
    private static void ResetAll()
    {

    }

}
#endif
