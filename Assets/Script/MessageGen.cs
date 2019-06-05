using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MessageGen : MonoBehaviour {

    [SerializeField]
    Text WindowText;
    public TextAsset TextResource;
    string[] ScenarioText;
    public int height = 0;
    

    void Start ()
    {
        StartCoroutine(RetrieveTexts(0.02f));
    }
	
	// Update is called once per frame
	void Update () {
	}

    IEnumerator RetrieveTexts(float TextSpeed) {
        LineContext(TextResource);
        WindowText = GameObject.Find("TalkSystems").GetComponent<Text>(); //テキストシステムのロード
        for (int Index = 0; Index < ScenarioText.Length; Index++) {

            string IndexedText = ScenarioText[Index];

            int ShownText = 0; //Tells How many Texts did show in 1 phrases
            Debug.Log(IndexedText);
            while (ShownText < IndexedText.Length) {
                ShownText++;
                WindowText.text = IndexedText.Substring(0, ShownText); //Shows the text 0 to "showntext"  
                yield return new WaitForSeconds(TextSpeed); //wait for time.
            }
            while (!Input.GetButton("Jump"))
            {
                yield return null; //Waitfor NextPage
            }
        }
    }

    void LineContext(TextAsset resources) {
        ScenarioText = resources.text.Split(new string[] { "@br" }, System.StringSplitOptions.None); //Sprit with each "@br"
        for (int i = 0; i < ScenarioText.Length; i++) { //process Commands.
             
            }
        }


    }
