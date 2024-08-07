using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScreenManager : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
