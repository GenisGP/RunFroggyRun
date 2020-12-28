using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;      //Velocidad máxima que llegará el jugador
    private float originalSpeed; //Servirá para configurar el deslizamiento y que el tiempo de deslizamiento no dependa de la velocidad actual del jugador
    private float speedMultiplier;  //Servirá también para lo mismo que el originalSpeed
    public float zDistance;     //Indica la distancia recorrida por el personaje
    private bool isMoving = true;
    public bool isDrowing, isDead, isEnding = false; //Si el jugador cae al agua o muere
    
    private float lastY;    //Para saber si el jugador está cayendo

    public int desiredLane = 1; //0:left 1:middle 2:right
    public float laneDistance = 2; //Distancia entre dos líneas

    public bool hasToJump = false;  //Ej: El jellyfish lo hace saltar automáticamente y usa esta variable
    public float jumpForce, originalJumpForce;
    public float gravity = -20f;
    public ParticleSystem jumpParticle;
    public ParticleSystem hitObstacleParticle;
    public GameObject SlideParticles;

    public Animator animator;

    public static bool isSliding = false; //Para no deslizarse cuando ya se está deslizando
    private float timeSliding;
    private float timeSlide = 1.1f;
    private float posYStartSliding;
    private bool stopSliding = false;

    public TutorialManager tutorialManager;

    public AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        lastY = transform.position.y;
        originalSpeed = forwardSpeed;
        originalJumpForce = jumpForce;
    }

    // Update is called once per frame
    void Update()
    {
        //Hasta que no toquemos la pantalla al principio no empezará el juego
        if (!PlayerManager.isGameStarted)
        {
            return;
        }
       
        //Si se está ahogando en el ahua
        if (isDrowing || isDead || isEnding)
        {
            isMoving = false;
            isSliding = false;
            //SlideParticles.SetActive(false);
            //Al ahogarse el jugador solo se mueve verticalmente hacia abajo.
            controller.Move(Vector3.up * gravity * Time.deltaTime);
        }
        
        PlayerManager.distanceScore = transform.position.z; //Puntuación de distancia

        //gamePaused ya que sinó si se hace swipe, al quitar la pausa el player se mueve
        if (isMoving && !PlayerManager.gamePaused)
        {
            FallingCheck();

            //Se usará para que el tiempo de deslizamiento en Slide() siempre sea el mismo, independientemente de la velocidad del jugador
            speedMultiplier = originalSpeed / forwardSpeed;

            animator.SetBool("isRunning", true);
            direction.z = forwardSpeed;  //Moverse hacia adelante
            animator.applyRootMotion = false;
            
            //Si está tocando el suelo saltará al pulsar la tecla adecuada
            if (controller.isGrounded || hasToJump)
            {
                animator.SetBool("isGrounded", true);
                direction.y = -1;
                //if (Input.GetKeyDown(KeyCode.UpArrow))
                if (SwipeManager.swipeUp || hasToJump)
                {
                    Jump();                    
                }
            }
            else
            {
                //Si no está en el suelo se le aplicará la gravedad
                direction.y += gravity * Time.deltaTime;
            }

            //Cuando el jugador se desliza
            if (SwipeManager.swipeDown && isSliding == false && controller.isGrounded)
            {
                StartCoroutine(Slide());
            }
            if (isSliding)
            {
                timeSliding += Time.deltaTime;
                if(timeSliding > timeSlide || posYStartSliding < transform.position.y - 0.5 || posYStartSliding > transform.position.y + 0.5)
                {
                    stopSliding = true;
                    SlideParticles.SetActive(false);
                }
            }

            controller.Move(direction * Time.deltaTime);

            //Coger los Inputs sobre que línea deberiamos ir
            //if (Input.GetKeyDown(KeyCode.RightArrow))
            if (SwipeManager.swipeRight)
            {
                desiredLane++;
                if (desiredLane > 2)
                {
                    desiredLane = 2;
                }
            }
            //if (Input.GetKeyDown(KeyCode.LeftArrow))
            if (SwipeManager.swipeLeft)
            {
                desiredLane--;
                if (desiredLane < 0)
                {
                    desiredLane = 0;
                }
            }

            //Calcular donde deberiamos ir en un futuro
            Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
            //Vector3 targetPosition = new Vector3(0f, transform.position.y, transform.position.z);

            if (desiredLane == 0)
            {
                targetPosition += Vector3.left * laneDistance;  //x = -1 + 4 = -4
            }
            else if (desiredLane == 2)
            {
                targetPosition += Vector3.right * laneDistance;  //x = 1 + 4 = 4
            }

            //transform.position = Vector3.Lerp(transform.position, targetPosition, 20 * Time.deltaTime);
            //Lo siguiente hace lo mismo que el el código comentado de arriba pero con el Character controller para ir mas fluido
            if (transform.position == targetPosition)
            {
                return;
            }
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
            if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            {
                controller.Move(moveDir);
                audioManager.PlaySound("ChangeLane");
            }
            else
            {
                controller.Move(diff);
            }
        }     
    }

    public void Jump()
    {
        direction.y = jumpForce;
        hasToJump = false;
        animator.SetBool("isGrounded", false);
        audioManager.PlaySound("Jump");
        //Partículas
        jumpParticle.transform.position = transform.position;
        jumpParticle.Play();
        jumpForce = originalJumpForce;  //El jellyfish modifica la fuerza de salto, por eso hace falta reiniciarla una vez se salte
    }

    void FallingCheck()
    {
        float distancePerSecondSinceLastFrame = (transform.position.y - lastY) * Time.deltaTime;
        lastY = transform.position.y;   //Para el siguiente frame
        if (distancePerSecondSinceLastFrame < -0.001)
        {
            animator.SetBool("isFalling", true);
            animator.SetBool("isGrounded", false);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }
    }

    //Cuando el controlador choca
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            if(transform.position.y < hit.transform.position.y + 1.5)
            {
                hitObstacleParticle.gameObject.SetActive(true);
                StartCoroutine(Dead());
            }
        }
        if (hit.transform.tag == "Fly")
        {
            audioManager.PlaySound("Eat");
        }
    }

    private IEnumerator Slide()
    {
        stopSliding = false;
        timeSliding = 0f;
        
        //Activa la animación de deslizar
        animator.SetBool("isSliding", true);

        //Audio
        audioManager.PlaySound("Slide");

        //Partículas
        SlideParticles.SetActive(true);

        //Se modifica el collider del controlador para que se adapte cuando se desliza
        controller.center = new Vector3(0, 0.45f, 0);
        controller.height = 0.8f;

        posYStartSliding = transform.position.y;

        //Se está deslizando
        isSliding = true;

        //Suspende la ejecución de la corrutina por un tiempo
        //yield return new WaitForSeconds(1.1f * speedMultiplier);
        yield return new WaitUntil(() => stopSliding);

        //Se modifica el collider del controlador para dejarlo como estaba originalmente
        controller.center = new Vector3(0, 0.74f, 0);
        controller.height = 1.5f;
        
        //Desactiva la animación de deslizar
        animator.SetBool("isSliding", false);

        //Audio
        audioManager.StopSound("Slide");

        //Partículas
        SlideParticles.SetActive(false);

        //Ya no se desliza
        isSliding = false;
    }

    public IEnumerator Dead()
    {
        //El jugador está muerto
        isDead = true;

        //Para de deslizarse si se estaba deslizando
        stopSliding = true;
        SlideParticles.SetActive(false);

        //Audio
        audioManager.PlaySound("Die");

        //Animaciones
        animator.SetBool("isDead", true);
        animator.SetBool("isRunning", false);

        //Suspende la ejecución de la corrutina por un tiempo
        yield return new WaitForSeconds(3f);

        //Game Over
        PlayerManager.gameOver = true;
    }

    public IEnumerator Drown()
    {
        //Se está ahogando
        isDrowing = true;
        isSliding = false;

        //Suspende la ejecución de la corrutina por un tiempo
        yield return new WaitForSeconds(2f);

        //Game Over
        PlayerManager.gameOver = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Tutoriales
        if (other.CompareTag("Tutorial"))  //Los GameObjects que contienen la interfaz del tutorial tienen el tag Tutorial
        {
            Debug.Log("tutorial");
            //playerManager.inTutorial = true;
            string name = other.name;   //Nombre del objeto Tutorial
            tutorialManager.TutorialActivate(name);
        }
        if (other.CompareTag("Fly"))  //Los GameObjects que contienen la interfaz del tutorial tienen el tag Tutorial
        {
            audioManager.PlaySound("Eat");
        }
    }
}
