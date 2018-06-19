using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsvReader : MonoBehaviour {

    public TextAsset CsvResource;
    public List<string[]> csvDatas = new List<string[]>();
    public int height = 0;

    // Use this for initialization
    void Start () {
    }
	// Update is called once per frame
	void Update () {
		
	}

    public void ReadData(){
        StringReader reader = new StringReader(CsvResource.text);
        while (reader.Peek() > -1) {
            string line = reader.ReadLine();
    csvDatas.Add(line.Split(','));
            height++;
        }
    }
}
