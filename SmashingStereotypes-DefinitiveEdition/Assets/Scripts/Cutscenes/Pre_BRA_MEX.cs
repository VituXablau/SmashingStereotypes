using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pre_BRA_MEX : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject BRA, MEX, DIA;
    public TextMeshProUGUI dialog;

    // Start is called before the first frame update
    void Start()
    {
        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Happy");
        MEX.GetComponent<Animator>().SetTrigger("Popcorn");
        dialog.text = "É meio difícil andar com esse baldão de pipoca, mas não dá pra ver ginástica sem pipoca. É muito chato. Imagina fazer ginástica.";
        StartCoroutine(Cutscene());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Cutscene(float waitTime = 3f)
    {


        yield return new WaitForSeconds(waitTime);


        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Angry");
        MEX.GetComponent<Animator>().SetTrigger("Kick");
        dialog.text = "!!!";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Right");
        BRA.GetComponent<Animator>().SetTrigger("Angry");
        MEX.GetComponent<Animator>().SetTrigger("Popcorn");
        dialog.text = "Ei, olha por onde anda, seu caminhão de taco!";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Angry");
        MEX.GetComponent<Animator>().SetTrigger("Popcorn");
        dialog.text = "Que bom que eu trouxe um colete aprova de balas pra não ser esfaqueado por brasileiro!";


           yield return new WaitForSeconds(waitTime);









    }
}
