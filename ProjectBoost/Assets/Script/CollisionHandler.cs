using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;

    Movement mv;
    bool isTransitioning = false;
    bool collisionDisable = false;

    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem failureParticle;

    private void Awake()
    {
        mv = FindObjectOfType<Movement>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L)) 
        {
            LoadNextStage();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collisionDisable)
        { return; }
        if(!isTransitioning)
        {
            switch (collision.gameObject.tag)
            {
                case "Friendly":
                    Debug.Log("this thing is friendly");
                    break;
                case "Finish":
                    mv.playSuccessSound();
                    SuccessSequence();
                    break;
                default:
                    mv.playFailSound();
                    StartCrashSequence();
                    break;
            }
        }
        
    }

    void StartCrashSequence()
    {
        failureParticle.Play();
        isTransitioning = true;
        gameObject.GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay); 
    }

    private void ReloadLevel()
    {
        int currentIdx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIdx);
    }

    private void SuccessSequence()
    {
        successParticle.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextStage", levelLoadDelay);
    }
    
    private void LoadNextStage()
    {
        isTransitioning = true;
        int currentIdx = SceneManager.GetActiveScene().buildIndex;
        int nextStageIdx = currentIdx+ 1;
        if(nextStageIdx == SceneManager.sceneCountInBuildSettings)
        {
            nextStageIdx = 0;
        }
        SceneManager.LoadScene(nextStageIdx);
    }
}
