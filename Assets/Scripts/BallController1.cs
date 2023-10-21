using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController1 : MonoBehaviour
{
    public static BallController1 singleton;
    public Rigidbody rb;
    public float speed = 15;

    private bool isTraveling; // pour savoir si la balle se d�place
    private Vector3 travelDirection; // la direction de d�placement de la balle
    private Vector3 nextCollisionPosition; //

    public int minSwipeRecognition = 500;//5mm

    private Vector2 swipePosLastFrame; // la position ant�rieur du curseur
    private Vector2 swipePostCurrentFrame; // la position actuelle du curseur
    private Vector2 currentSwipe;
    private Color solveColor;
   // public ParticleSystem wavePartical;

   // private AudioSource audioSource;

    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        } else if(singleton != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

        }
        
    }

    public void PlayMusic()
    {
       // audioSource.Play();
    }

    public void StopMusic()
    {
       // audioSource.Stop();
    }

    private void Start()
    {
        solveColor = Random.ColorHSV(0.5f, 1);
        GetComponent<MeshRenderer>().material.color = solveColor;
        //audioSource = GetComponent<AudioSource>();
    }



    private void FixedUpdate()
    {
        //wavePartical.gameObject.position
        // d�place la balle si elle est en mouvement
        if (isTraveling)
        {
            rb.velocity =  travelDirection * speed;          
        }

        // colorer les grid
        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up/2), 0.05f);
        //Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.05f);
        int i = 0;
        while (i < hitColliders.Length)
        {
            //Debug.Log("kikikkkkkkkkk");
            GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();
            if (ground && !ground.isColored)
            {
                ground.Colored(solveColor);
            }
            i++;
        }

        // si la position du prochain Obstacle est diffr�rent du vecteur nulle
        if (nextCollisionPosition != Vector3.zero) {
        
        //si la distance entre la balle et le prochain obstacle est inf�rieur  � 1 arreter le d�placement de la balle et permettre � la balle de changer de dirrection
            if(Vector3.Distance(transform.position,nextCollisionPosition) < 1)
            {
                isTraveling = false;
                travelDirection = Vector3.zero;
                nextCollisionPosition = Vector3.zero;
            }
        }

        
        if (isTraveling)
            return;
        
        //si l'utilisateur touche l'�cran ou effecuer un clic de la souris
        if(Input.GetMouseButton(0))
        {
            // ou se touve la position du curseur de la souris
            swipePostCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //wavePartical.Play();

            // la position pr�c�dente du curseur n'est pas un vecteur null
            if (swipePosLastFrame != Vector2.zero)
            { 
                currentSwipe = swipePostCurrentFrame - swipePosLastFrame;

                if(currentSwipe.sqrMagnitude < minSwipeRecognition)
                {
                    return;
                }

                currentSwipe.Normalize();//la normalisation permet d'obtenir la direction et non la distance du d�placement de la balle

                //Up / Down
                if (currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    //Go Up / Down
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward: Vector3.back);
                }

                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    //Go left / right
                    SetDestination(currentSwipe.x > 0 ? Vector3.right: Vector3.left);

                }
            }
            
            swipePosLastFrame = swipePostCurrentFrame;
        }

        if(Input.GetMouseButtonUp(0))
        {
            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
           // nextCollisionPosition = Vector2.zero;
        }
    }

    private void SetDestination(Vector3 direction)
    {
        travelDirection = direction;

        RaycastHit hit;// verifie quelle objet entre en collission avec la balle�

        if (Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollisionPosition = hit.point;
        }
        isTraveling = true;
    }

}
