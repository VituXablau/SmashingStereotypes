using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pre_IND_MEX : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    public GameObject IND, MEX, DIA;
    public TextMeshProUGUI dialog;

    // Start is called before the first frame update
    void Start()
    {
        DIA.GetComponent<Animator>().SetTrigger("Left");
        IND.GetComponent<Animator>().SetTrigger("Default");
        MEX.GetComponent<Animator>().SetTrigger("Happy");
        dialog.text = "TIRO É O ÚNICO ESPORTE QUE PRESTA!!! VAI MÉXICOOOOO!!!!!";
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
        IND.GetComponent<Animator>().SetTrigger("Embarassed");
        MEX.GetComponent<Animator>().SetTrigger("Happy");
        dialog.text = "IÉAAAAAAAA!!";

        yield return new WaitForSeconds(waitTime);


        DIA.GetComponent<Animator>().SetTrigger("Right");
        IND.GetComponent<Animator>().SetTrigger("Angry");
        MEX.GetComponent<Animator>().SetTrigger("Angry");
        dialog.text = "Dá pra torcer mais baixo? Aqui não é cinco de maio.";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        IND.GetComponent<Animator>().SetTrigger("Angry");
        MEX.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "Ou o quê? Você vai passar por cima de mim com uma vaca?.";

        yield return new WaitForSeconds(waitTime);


    }
}
