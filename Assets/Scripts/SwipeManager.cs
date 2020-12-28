using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    public static bool tap, swipeLeft, swipeRight, swipeUp, swipeDown; //Se pueden acceder des de cualquier script usando el nombre del script, p.e. en el player controller
    private bool isDraging = false;     //Si estamos arrastrando el ratón o el dedo
    private Vector2 startTouch, swipeDelta; //Donde se hace el primer click y donde tenemos presionado en este momento

    private void Update()
    {
        //Cada frame se resetea
        tap = swipeDown = swipeUp = swipeLeft = swipeRight = false;
        //Se crea una región para agrupar los Inputs del ordenador
        #region Standalone Inputs
        //Si pulsamos el ratón
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            isDraging = true;
            startTouch = Input.mousePosition;
        }
        //Si dejamos de pulsar
        else if (Input.GetMouseButtonUp(0))
        {
            isDraging = false;
            Reset();
        }
        #endregion

        //Se crea una región para agrupar los Inputs del móvil
        #region Mobile Input
        //Si hay algun touch (se está tocando la pantalla con el dedo)
        if (Input.touches.Length > 0)
        {
            //Se tomará siempre el primer touch y se mira si está en fase "Began", es decir, se está pulsando la pantalla en este frame
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            //Si el touch está en fase Ended o Canceled, es decir dejamos de pulsar
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDraging = false;
                Reset();
            }
        }
        #endregion

        //********************************
        //Calcular la distancia del swipe
        //********************************
        swipeDelta = Vector2.zero;
        //Esto significa si hemos empezado a hace algo, si hemos pulsado por ejemplo y estamos arrastrando
        if (isDraging)
        {
            //Si hacemos el swipe
            if (Input.touches.Length < 0)
            {
                //el swipe delta es la diferencia (el recorrido) entre donde tenemos ahora el dedo y donde hemos empezado pulsando
                swipeDelta = Input.touches[0].position - startTouch;
            }
            //Sinó, si se está pulsando el botón izquierdo del ratón
            else if (Input.GetMouseButton(0))
            {
                //Lo mismo que antes pero con el ratón. Hace falta pasar el mousePosition (que es un Vector3) a Vector 2 para hacer la operación
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }

        //********************************************************************************
        //Si deslizamos el dedo más del límite para realizar el movimiento del personaje
        //********************************************************************************

        //Si la distancia de trazado es mayor a X píxeles de la pantalla. 
        //Hay que mover el dedo o ratón mas que 125 px para que el swipe sea true y mover el personaje des del character controller
        if (swipeDelta.magnitude > 100)
        {
            //En que dirección estamos arrastrando?
            float x = swipeDelta.x; //Posición en X donde tenemos el dedo o ratón
            float y = swipeDelta.y; //Posición en X donde tenemos el dedo o ratón
            //Miramos el valor absoluto de x e y. Así sabemos si estamos mas hacia el lado o hacia arriba o abajo para saber a que dirección hacer el swipe
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //Derecha o izquierda
                if (x < 0)
                {
                    swipeLeft = true;
                }
                else
                {
                    swipeRight = true;
                }
            }
            else
            {
                //Arriba o abajo
                if (y < 0)
                {
                    swipeDown = true;
                }
                else
                {
                    swipeUp = true;
                }
            }
            //Si entramos aquí hay que parar el swipe
            Reset();
        }

    }
    //Reinicia a 0
    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }
}