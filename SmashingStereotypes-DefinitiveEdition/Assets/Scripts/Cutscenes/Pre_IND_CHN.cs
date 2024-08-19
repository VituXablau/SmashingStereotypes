using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pre_IND_CHN : MonoBehaviour
{

    public GameObject IND, CHN, DIA;
    public TextMeshProUGUI dialog;

    // Start is called before the first frame update
    void Start()
    {
        DIA.GetComponent<Animator>().SetTrigger("Left");
        IND.GetComponent<Animator>().SetTrigger("Default");
        CHN.GetComponent<Animator>().SetTrigger("Smug");
        dialog.text = "Oiii internete! Será que aquele indiano aqui vende curry?! Vamos perguntar, haha!!";
        StartCoroutine(Cutscene());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Cutscene(float waitTime = 3f)
    {

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Right");
        IND.GetComponent<Animator>().SetTrigger("Angry");
        CHN.GetComponent<Animator>().SetTrigger("Smug");
        dialog.text = "Quê?! Faz um ritual samurai pra mim aí então, dragão!";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        IND.GetComponent<Animator>().SetTrigger("Angry");
        CHN.GetComponent<Animator>().SetTrigger("Angry");
        CHN.transform.localScale = new Vector3(-1, 1, 1);
        dialog.text = "#*%@$!";


    }
}
