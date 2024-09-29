using System.Collections;
using UnityEngine;

public class ComputerCPU : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextBoxManager textBoxManager;

    public enum ComputerMode { input, displayText}

    [Header("Settings")]
    public bool inputEnabled = true;

    [Header("Dynamic Data")]
    public ComputerMode mode = ComputerMode.input;
    public ComputerFile currentFile;

    [Header("Data Base")]
    public ComputerFile[] dataBase;

    private int currentStep = 0;
    [HideInInspector] public bool inReadMode = false;

    public void Update() {
        if (inputEnabled && inReadMode) {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0)) {
                ForwardText();
            }
        }
    }

    //Methods
    public void DisplayFile(ComputerFile _file) {
        currentFile = _file;
        inReadMode = true;
        ForwardText();
    }

    public void EnterInputMode() {
        inReadMode = false;
        textBoxManager.DisplayText("Waiting for user input...", "Input");
        textBoxManager.CurrentInput = "";
    }

    private void ForwardText() {
        if(currentStep < currentFile.content.Length) {
            textBoxManager.DisplayText(currentFile.content[currentStep], currentFile.fileName);
            currentStep++;
        }
        else {
            EnterInputMode();
        }
    }


    public void RunCommand(string _command) {
        if (true) { // on no valid command
            textBoxManager.DisplayText("Error: no valid file or command known as " + '"' + _command + '"' + " exists.\n Please refer to your user manual or file catalogue for valid entries by pressing Tab.", "Error");
        }
    }
}
