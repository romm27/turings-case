using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ComputerCPU computer;

    [Header("Settings")]
    public bool inBookMode = false;

    public void Start() {
        computer.EnterInputMode();
    }

    //Professor! eu não me orgulho muito desse código
    //Eu tive 1 dia para fazer tudo do zero na Unity!
    //Por isso tem umas coisas muito 'não ortodoxas' espalhadas por ai...
    // - Giovanni
    public void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            inBookMode = !inBookMode;
        }

        //Set Input
        computer.inputEnabled = !inBookMode;

        if(inBookMode) {

        }
    }
}
