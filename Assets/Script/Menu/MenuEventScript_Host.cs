using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuEventScript_Host : MonoBehaviour {

    public MenuEventScript_Child[] Youso;
    public int selection = 0;
    public GameObject Pointer;



    MenuEventScript_Child SelectedEvent;
    // Use this for initialization
    void Start() {
        StartCoroutine(OnSelection(Youso.Length));
    }

    // Update is called once per frame
    void Update()
    {
        SelectedEvent = Youso[selection].GetComponent<MenuEventScript_Child>();

        if (Input.GetButtonDown("Jump"))
        {
            switch (SelectedEvent.type) {
                case MenuEventScript_Child.Type.loadscene:
                    UnityEngine.SceneManagement.SceneManager.LoadScene(SelectedEvent.scene.buildIndex);
                    break;
                case MenuEventScript_Child.Type.stop:
                    Application.Quit();
                    Debug.Log("Quit");
                    break;
                default:
                    break;
            }
        }
        Pointer.transform.position = SelectedEvent.transform.position
            + new Vector3(0, SelectedEvent.GetComponent<Renderer>().bounds.size.y, 0);
        GetComponent<Text>().text = SelectedEvent.menuname;
    }


    IEnumerator OnSelection(int max)
    {
        float Presstime = 0;
        while (Application.isPlaying)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1)
            {
                if (Presstime == 0 || (Presstime > 1.0 && Mathf.Round((Presstime % 0.002f) * 1000) == 0))
                {
                    selection += (int)Mathf.Sign(Input.GetAxis("Horizontal"));
                    Presstime += Time.deltaTime;
                }
                else
                    Presstime += Time.deltaTime;
            }
            else
            {
                Presstime = 0;
            }

            selection = (int)Mathf.Repeat(selection, max);


            yield return null;
        }
    }
}
