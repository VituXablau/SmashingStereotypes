using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    //Criando um Singleton do GameManager
    public static GameManager Instance { get; set; }

    //Criando um vetor das posições para o Instânciamento dos jogadores e dos itens
    [SerializeField] private Transform[] spawnPointsPlayers;
    [SerializeField] private Transform[] spawnPointsItens;

    //Criando o WaitTime do spawn dos Itens
    [SerializeField] private int timeToSpawn;

    //Criando vetores dos prefabs dos Jogadores, das suas Huds e dos Itens
    [SerializeField] private GameObject[] playersPrefabs, player1Frames, player2Frames, player1Lives, player2Lives;
    [SerializeField] private GameObject[] itensPrefabs;
    //Criando a Coroutine que instância os itens
    private Coroutine spawnCoroutine;

    //String que armazena os Nomes dos Jogadores, os seus Knockbacks e as suas vidas, para a sincronização com o Multiplayer
    [HideInInspector] public string p1Name, p2Name;
    [HideInInspector] public float p1Knockback, p2Knockback;
    [HideInInspector] public int p1Lives, p2Lives;

    //Textos da Hud que mostram a porcentagem de Knockback
    [SerializeField] private TextMeshProUGUI p1KnockbackTxt, p2KnockbackTxt, p1KnockbackShadowTxt, p2KnockbackShadowTxt;

    //Bool que armazena se aconteceu o fim de jogo
    private bool isGameOver = false;

    void Awake()
    {
        //Definindo o Singleton
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
        //Chamando o método que Instância os Jogadores
        InstantiatePlayers();

        //Verificando se é o criador da sala, se for, inicia a Coroutine que Instância os itens
        if (PhotonNetwork.IsMasterClient)
            spawnCoroutine = StartCoroutine(SpawnItens(timeToSpawn));

        //Aplicando a Imagem certa de cada Jogador na Hud
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
        //Chamando o método que verifica o GameOver
        GameOver();
    }

    private void GameOver()
    {
        //Verifica se a vida de algum dos Jogadores está menor que mero, se estiver ele interrompe a coroutine que instância os itens
        if (p1Lives < 0 || p2Lives < 0)
        {
            if (PhotonNetwork.IsMasterClient && spawnCoroutine != null)
                StopCoroutine(spawnCoroutine);
            //Fixando o valor das vidas
            if (p1Lives < 0)
                p1Lives = -1;
            else if (p2Lives < 0)
                p2Lives = -1;
            //Chamando a coroutine que leva para a tela de Vitória
            StartCoroutine(LoadScoreboard());
        }
    }

    //Coroutine que muda de cena e sai da sala do Multiplayer
    private IEnumerator LoadScoreboard()
    {
        //Aguardando um tempo antes de sair da sala e mudar de cena
        yield return new WaitForSeconds(0.5f);
        //Verificando se esta na sala, se estiver ele sai
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        //Garantindo que o jogador não esteja na sala antes de mudar de cena
        while (PhotonNetwork.InRoom || !PhotonNetwork.IsConnectedAndReady)
        {
            yield return null;
        }
        //Mudando de cena
        if (!isGameOver)
        {
            SceneManager.LoadScene("Scoreboard");
            isGameOver = true;
        }
    }

    //Método que instância os Jogadores
    private void InstantiatePlayers()
    {
        //Vendo qual o Personagem de cada Jogador e instanciando o correto na cena
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
        //Definindo o nome do jogador como o Nickname do personagem escolhido
        p1Name = PhotonNetwork.PlayerList[0].NickName;
        p2Name = PhotonNetwork.PlayerList[1].NickName;
    }

    //Coroutine que instancia os itens
    private IEnumerator SpawnItens(int timeToSpawn)
    {
        while (true)
        {
            //Verificando se é o criador da sala, para os itens não serem duplicados
            if (PhotonNetwork.IsMasterClient)
            {
                yield return new WaitForSeconds(timeToSpawn);
                //Instanciando em uma posição aleatória
                int randomPrefab = Random.Range(0, itensPrefabs.Length);
                Transform randomPosition = spawnPointsItens[Random.Range(0, spawnPointsItens.Length)];

                PhotonNetwork.Instantiate(itensPrefabs[randomPrefab].name, randomPosition.position, Quaternion.identity);
            }
        }
    }

    //Método que recebe as informações dos jogadores e manda para o servidor
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

    //Método que atualiza os dados no servidor
    [PunRPC]
    public void RPC_UpdateInfos(int p1Lives, int p2Lives, float p1Knockback, float p2Knockback)
    {
        this.p1Knockback = p1Knockback;
        this.p2Knockback = p2Knockback;

        this.p1Lives = p1Lives;
        this.p2Lives = p2Lives;

        UpdateUI();
    }

    //Método que atualiza os dados na hud
    void UpdateUI()
    {
        p1KnockbackTxt.text = p1Knockback + "%";
        p2KnockbackTxt.text = p2Knockback + "%";
        p1KnockbackShadowTxt.text = p1Knockback + "%";
        p2KnockbackShadowTxt.text = p2Knockback + "%";

        UpdateLivesUI(player1Lives, p1Lives);
        UpdateLivesUI(player2Lives, p2Lives);
    }

    //Método que verifica a quantidade de vida dos jogadores para exibir o número correto de corações
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

    //Método que verifica se algum jogador abandonou a sala
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        //Se as vidas estiverem maiores que -1 então significa que abandonou a sala
        if (p1Lives >= 0 && p2Lives >= 0)
            StartCoroutine(LeaveRoomAndLoadLobby());
    }

    //Coroutine que retorna para o lobby 
    private IEnumerator LeaveRoomAndLoadLobby()
    {
        //Verificando se está na sala antes de sair dela
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        //Garantindo que saiu da sala
        while (PhotonNetwork.InRoom || !PhotonNetwork.IsConnectedAndReady)
        {
            yield return null;
        }
        //Saindo da sala
        SceneManager.LoadScene("Lobby");
    }
}
