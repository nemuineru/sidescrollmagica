using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Resource_Banks : ScriptableObject
{
    [SerializeField]
    public List<ThingsBank> ActorBanks = new List<ThingsBank>();
    [System.SerializableAttribute]
    public class ThingsBank
    {
        public string name;
        public Sprite[] FaceBank;
    }

    [MenuItem("Assets/Create/Example/Create ResourceBank Instance")]
    static void CreateAssets()
    {
        var InstancedAsset = CreateInstance<Resource_Banks>();

            AssetDatabase.CreateAsset(InstancedAsset, "Assets/Script/Editor/ExampleAsset.asset");
            AssetDatabase.Refresh();
    }
}
