using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance { get; set; }

    [SerializeField] private Transform[] spawnPointsPlayers;
    [SerializeField] private Transform[] spawnPointsItens;

    [SerializeField] private int timeToSpawn;

    [SerializeField] private GameObject[] playersPrefabs, player1Frames, player2Frames, player1Lives, player2Lives;
    [SerializeField] private GameObject[] itensPrefabs;
    private Coroutine spawnCoroutine;

    [HideInInspector] public string p1Name, p2Name;
    [HideInInspector] public float p1Knockback, p2Knockback;
    [HideInInspector] public int p1Lives, p2Lives;

    [SerializeField] private TextMeshProUGUI p1KnockbackTxt, p2KnockbackTxt, p1KnockbackShadowTxt, p2KnockbackShadowTxt;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InstantiatePlayers();

        if (PhotonNetwork.IsMasterClient)
            spawnCoroutine = StartCoroutine(SpawnItens(timeToSpawn));

        foreach (GameObject frame in player1Frames)
        {
            if (frame.name == p1Name)
            {
                frame.SetActive(true);
            }
        }
        foreach (GameObject frame in player2Frames)
        {
            if (frame.name == p2Name)
            {
                frame.SetActive(true);
            }
        }
    }

    void Update()
    {
        GameOver();
    }

    private void GameOver()
    {
        if (p1Lives < 0 || p2Lives < 0)
        {
            if (PhotonNetwork.IsMasterClient && spawnCoroutine != null)
                StopCoroutine(spawnCoroutine);

            if (p1Lives < 0)
                p1Lives = -1;
            else if (p2Lives < 0)
                p2Lives = -1;

            StartCoroutine(LeaveRoomAndLoadScoreboard());
        }
    }

    private IEnumerator LeaveRoomAndLoadScoreboard()
    {
        yield return new WaitForSeconds(0.05f);

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        while (PhotonNetwork.InRoom || !PhotonNetwork.IsConnectedAndReady)
        {
            yield return null;
        }

        if (!isGameOver)
        {
            SceneManager.LoadScene("Scoreboard");
            isGameOver = true;
        }
    }

    private void InstantiatePlayers()
    {
        foreach (GameObject player in playersPrefabs)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (player.GetComponent<PlayerBehaviour>().characterID == PhotonNetwork.PlayerList[0].NickName)
                {
                    PhotonNetwork.Instantiate(player.name, spawnPointsPlayers[0].position, Quaternion.identity);
                }
            }
            else
            {
                if (player.GetComponent<PlayerBehaviour>().characterID == PhotonNetwork.PlayerList[1].NickName)
                {
                    PhotonNetwork.Instantiate(player.name, spawnPointsPlayers[1].position, Quaternion.identity);
                }
            }
        }

        p1Name = PhotonNetwork.PlayerList[0].NickName;
        p2Name = PhotonNetwork.PlayerList[1].NickName;
    }

    private IEnumerator SpawnItens(int timeToSpawn)
    {
        while (true)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                yield return new WaitForSeconds(timeToSpawn);

                int randomPrefab = Random.Range(0, itensPrefabs.Length);
                Transform randomPosition = spawnPointsItens[Random.Range(0, spawnPointsItens.Length)];

                PhotonNetwork.Instantiate(itensPrefabs[randomPrefab].name, randomPosition.position, Quaternion.identity);
            }
        }
    }

    public void ReceiveInfos(int playerNum, float knockbackPercentage, int lives)
    {
        switch (playerNum)
        {
            case 1:
                p1Knockback = knockbackPercentage;
                p1Lives = lives;
                break;

            case 2:
                p2Knockback = knockbackPercentage;
                p2Lives = lives;
                break;
        }
        photonView.RPC("RPC_UpdateInfos", RpcTarget.All, p1Lives, p2Lives, p1Knockback, p2Knockback);
    }

    [PunRPC]
    public void RPC_UpdateInfos(int p1Lives, int p2Lives, float p1Knockback, float p2Knockback)
    {
        this.p1Knockback = p1Knockback;
        this.p2Knockback = p2Knockback;

        this.p1Lives = p1Lives;
        this.p2Lives = p2Lives;

        UpdateUI();
    }

    void UpdateUI()
    {
        p1KnockbackTxt.text = p1Knockback + "%";
        p2KnockbackTxt.text = p2Knockback + "%";
        p1KnockbackShadowTxt.text = p1Knockback + "%";
        p2KnockbackShadowTxt.text = p2Knockback + "%";

        UpdateLivesUI(player1Lives, p1Lives);
        UpdateLivesUI(player2Lives, p2Lives);
    }

    void UpdateLivesUI(GameObject[] playerLives, int lives)
    {
        for (int i = 0; i < playerLives.Length; i++)
        {
            if (i < lives)
            {
                playerLives[i].SetActive(true);
            }
            else
            {
                playerLives[i].SetActive(false);
            }
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (p1Lives > 0 && p2Lives > 0)
            StartCoroutine(LeaveRoomAndLoadLobby());
    }

    private IEnumerator LeaveRoomAndLoadLobby()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        while (PhotonNetwork.InRoom || !PhotonNetwork.IsConnectedAndReady)
        {
            yield return null;
        }

        SceneManager.LoadScene("Lobby");
    }
}
