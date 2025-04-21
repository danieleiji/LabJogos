using UnityEngine;
using UnityEngine.Video; // Necessário para VideoPlayer
using UnityEngine.SceneManagement; // Necessário se for carregar outra cena
using System.Collections; // Necessário para IEnumerator se usar delay

[RequireComponent(typeof(VideoPlayer))] // Garante que o VideoPlayer esteja no mesmo objeto
public class VideoEndHandler : MonoBehaviour
{
    [Tooltip("Nome da cena a carregar quando o vídeo terminar (deixe vazio para não carregar)")]
    public string sceneToLoadAfterVideo = "MainMenu"; // Exemplo: ir para o Menu Principal

    [Tooltip("Objeto a ser ativado quando o vídeo terminar (ex: um painel com botões)")]
    public GameObject objectToEnableAfterVideo;

    private VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // Desabilita o objeto opcional no início
        if (objectToEnableAfterVideo != null)
        {
            objectToEnableAfterVideo.SetActive(false);
        }
    }

    // Subscreve ao evento quando o componente é ativado
    void OnEnable()
    {
        if (videoPlayer != null)
        {
            // loopPointReached é chamado quando o vídeo termina (se não estiver em loop) ou quando atinge o ponto de loop
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    // Dessubscreve ao evento quando o componente é desativado
    void OnDisable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }

    // Método chamado quando o evento loopPointReached é disparado
    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video Ended!");

        // Opção 1: Ativar um objeto (como um painel de UI com botões)
        if (objectToEnableAfterVideo != null)
        {
            objectToEnableAfterVideo.SetActive(true);
            // Talvez parar o vídeo aqui para liberar recursos se não for carregar cena
            // videoPlayer.Stop();
        }

        // Opção 2: Carregar outra cena (só se a Opção 1 não foi usada ou se você quiser ambos)
        // Verifica se há uma cena para carregar E se não foi configurado para ativar um objeto OU se o objeto já foi ativado
        // Adapte esta lógica se precisar de comportamento diferente
        else if (!string.IsNullOrEmpty(sceneToLoadAfterVideo))
        {
             Debug.Log($"Carregando cena: {sceneToLoadAfterVideo}");
             // Opcional: Pequeno delay antes de carregar a cena
             // StartCoroutine(LoadSceneAfterDelay(sceneToLoadAfterVideo, 0.5f));
             SceneManager.LoadScene(sceneToLoadAfterVideo);
        }
        else
        {
             // Nenhuma ação configurada para o fim do vídeo, talvez parar o player
              videoPlayer.Stop();
        }
    }

    // Coroutine opcional para adicionar um delay
    IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}