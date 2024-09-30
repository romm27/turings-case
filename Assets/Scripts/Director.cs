using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Director : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ComputerCPU computer;
    [SerializeField] Image fadeIn;
    [SerializeField] GameObject bookObject;

    [Header("Settings")]
    public float bookAlpha = 0.7f;
    public float fadeInTime = 1f;
    public bool inBookMode = false;
    public bool fading = true;


    float fadeInRate = 0;

    public void Start() {
        computer.EnterInputMode();
        fadeIn.gameObject.SetActive(true);
        fadeInRate = 1f/fadeInTime;
    }

    //Professor! eu n�o me orgulho muito desse c�digo
    //Eu tive 1 dia para fazer tudo do zero na Unity!
    //Por isso tem umas coisas muito 'n�o ortodoxas' espalhadas por ai...
    //Se eu tivesse mais tempo eu teria feito tudo muito diferente e escalavel.
    // - Giovanni
    public void Update() {
        if (Input.GetKeyDown(KeyCode.Tab) && !fading) {
            inBookMode = !inBookMode;

            bookObject.SetActive(inBookMode);
            SetBookFade(inBookMode);
        }

        //Set Input
        computer.inputEnabled = !inBookMode;

        if(inBookMode) {

        }

        if(fading && fadeIn.color.a > 0f) {
            ManageBlackFadeIn();
        }
        else {
            fading = false;
        }
    }

    public void SetBookFade(bool _b) {
        Color temp = fadeIn.color;
        temp.a = _b ? bookAlpha : 0f;
        fadeIn.color = temp;
        fading = false;
    }

    //Methods
    private void ManageBlackFadeIn() {
        Color temp = fadeIn.color;
        temp.a -= fadeInRate * Time.deltaTime;
        fadeIn.color = temp;
    }
}
