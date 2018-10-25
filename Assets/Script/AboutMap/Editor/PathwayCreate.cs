using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PathwayCreate : ScriptableWizard{


    public Vector2[] PathPoint;
    static PathwayCreate exampleWindow;

    [MenuItem("Window/Example")]
    static void Open()
    {
        if (exampleWindow == null)
        {
            exampleWindow = CreateInstance<PathwayCreate>();
        }
        DisplayWizard<PathwayCreate>("PathwayCreate");
    }

    private void OnWizardCreate()
    {
        new GameObject("New Object");
    }

}
