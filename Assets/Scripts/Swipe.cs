using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private bool isDragging = false;  //Si estamos arrastrando el ratón o el dedo
    private Vector2 startTouch, swipeDelta;

    private void Update()
    {
        //Cada frame se resetea
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;

        //Se crea una región para agrupar los Inputs del ordenador 
        #region Standalone Inputs
        if (Input.GetMouseButton(0))
        {
            tap = true;
            isDragging = true;
            startTouch = Input.mousePosition;
        }
        //Si tenemos un swipe (deslice) ocurrirá aquí, después del buttonDown y antes del buttonUp
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            Reset();
        }
        #endregion

        //Se crea una región para agrupar los Inputs del móvil 
        #region Mobile Inputs
        //Si hay algun touch (se está tocando la pantalla con el dedo)
        if (Input.touches.Length > 0)
        {
            //Se tomará siempre el primer touch y se mira si está en fase "Began", es decir, se está pulsando la pantalla en este frame
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDragging = true;
                startTouch = Input.touches[0].position; //Se obtiene la posición del touch
            }
            //Si el touch está en fase Ended o Canceled
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDragging = false;
                Reset();
            }
        }
        #endregion

        //********************************
        //Calcular la distancia del swipe
        //********************************
        swipeDelta = Vector2.zero;
        //Esto significa si hemos empezado a hace algo, si hemos pulsado por ejemplo
        if (isDragging)
        {
            //Si hacemos el swipe
            if (Input.touches.Length > 0)
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
        //******************************************
        //Si deslizamos el dedo más de lo permitido
        //******************************************
        //Si la distancia de trazado es mayor a X píxeles de la pantalla. Hay que mover el dedo o ratón mas que 125 px
        if (swipeDelta.magnitude > 125)
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

    //Variables públicas que hacen referencia alas privadas
    public bool Tap { get { return tap; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

    private void Reset()
    {
        //Reinicia a 0
        startTouch = swipeDelta = Vector2.zero;
        isDragging = false;
    }
}
