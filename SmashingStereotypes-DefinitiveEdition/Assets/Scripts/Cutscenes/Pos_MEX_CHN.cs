using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Pos_MEX_CHN : MonoBehaviour
{
  public GameObject MEX, CHN, DIA;
  public TextMeshProUGUI dialog;

  // Start is called before the first frame update
  void Start()
  {
    DIA.GetComponent<Animator>().SetTrigger("Left");
    MEX.GetComponent<Animator>().SetTrigger("Neutral");
    CHN.GetComponent<Animator>().SetTrigger("Sad");
    dialog.text = "Vo-você... realmente acha... que eu sou um projeto de muralha?...";
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
    MEX.GetComponent<Animator>().SetTrigger("Embarassed");
    dialog.text = "Não. Pelo seu seu tamanho tá mais pra projeto de tijolo mesmo.";

    yield return new WaitForSeconds(waitTime);

    CHN.GetComponent<Animator>().SetTrigger("Smug");
    DIA.GetComponent<Animator>().SetTrigger("Left");
    dialog.text = "Ok, eu gostei do seu humor. Vamos gravar um vídeo juntos!";

    yield return new WaitForSeconds(waitTime);

    MEX.GetComponent<Animator>().SetTrigger("Default");
    DIA.GetComponent<Animator>().SetTrigger("Right");
    dialog.text = "...humor?";

    yield return new WaitForSeconds(waitTime);

    PhotonNetwork.NickName = "";
    SceneManager.LoadScene("Lobby");

  }
}
