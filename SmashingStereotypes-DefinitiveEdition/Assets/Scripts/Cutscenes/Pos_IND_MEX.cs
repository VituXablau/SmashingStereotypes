using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pos_IND_MEX : MonoBehaviour
{
    // Start is called before the first frame update
   // Start is called before the first frame update
    // Start is called before the first frame update
    public GameObject IND, MEX, DIA;
    public TextMeshProUGUI dialog;

    // Start is called before the first frame update
    void Start()
    {
        DIA.GetComponent<Animator>().SetTrigger("Left");
        IND.GetComponent<Animator>().SetTrigger("Angry");
        MEX.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "Ei, sabe o que cairia bem agora? Cerveja!!!";
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
        MEX.GetComponent<Animator>().SetTrigger("Happy");
        dialog.text = "Quê? Eu tenho uma ligação importante daqui a pou-";

        yield return new WaitForSeconds(waitTime);


        DIA.GetComponent<Animator>().SetTrigger("Left");
        IND.GetComponent<Animator>().SetTrigger("Embarassed");
        MEX.GetComponent<Animator>().SetTrigger("Bump");
        MEX.transform.localScale = new Vector3(-1, 1, 1);
        dialog.text = "Cervejaaaaa!!!";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Right");
        IND.GetComponent<Animator>().SetTrigger("Angry");
        MEX.GetComponent<Animator>().SetTrigger("Bump");
        dialog.text = "...!!!";

        yield return new WaitForSeconds(waitTime);


    }
}
