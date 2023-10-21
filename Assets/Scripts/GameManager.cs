using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;// avoir  une seule instance de gameManager
    public GroundPiece[] allGroundPieces;// une collection de ground piece

    private AudioSource audioSource;
    public AudioClip winSound;
    public GameObject confitti;
    public GameObject nextScreen;
    private int level = 5;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //confitti = FindObjectOfType<Conf>
        SetUpNewLevel();
        BallController1.singleton.PlayMusic();
    }

    // recupère tout les objet de type groundpiece
    private void SetUpNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>();
    }

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;

        }
        else if(singleton != this)
        {
            Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }
              
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetUpNewLevel();
    }

    public void CheckComplete()
    {
        bool isFinished = true;
       

        for (int i=0; i<allGroundPieces.Length; i++)          
        {
            if (allGroundPieces[i].isColored == false)
            {
                isFinished = false;
                break;
            }      
        }

       
        if (isFinished)
        {
            audioSource.Stop();
            confitti.SetActive(true);
            audioSource.PlayOneShot(winSound);
            nextScreen.SetActive(true);
            // StartCoroutine(PlayConfitti());
            //NextLevel();           
        }
    }
    IEnumerator PlayConfitti()
    {
        
        yield return new WaitForSeconds(20f);
        
    }
    public void NextLevel()
    {
        //StartCoroutine(PlayConfitti());

        if (SceneManager.GetActiveScene().buildIndex==4)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        
    }
  
}
