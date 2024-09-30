using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerCPU : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextBoxManager textBoxManager;
    [SerializeField] AudioSource victorySource;
    [SerializeField] AudioClip victoryClip;

    public enum ComputerMode { input, displayText}

    [Header("Settings")]
    public bool inputEnabled = true;

    [Header("Dynamic Data")]
    public ComputerMode mode = ComputerMode.input;
    public ComputerFile currentFile;

    [Header("Data Base")]
    public ComputerFile[] dataBase;
    public ComputerFile youWin;

    [Header("Case Cracker")]
    [SerializeField] ComputerFile[] caseCrackerStepScreen;
    public int caseCrackerStep = 0;
    public List<bool> responses = new List<bool>();

    private int currentPage = -1;
    [HideInInspector] public bool inReadMode = false;
    [HideInInspector] public bool inCaseCracker = false;
    



    public bool IsLastPage {
        get {
            return currentFile.content.Length == currentPage;
        }
    }

    public int CurrentPage {
        get {
            return currentPage;
        }
        set {
            currentPage = value;
        }
    }

    public int CurrentFileSize {
        get {
            if(currentFile != null) {
                return currentFile.content.Length;
            }
            else {
                return 0;
            }
        }
    }

    public void Update() {
        if (inputEnabled && inReadMode) {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0)) {
                currentPage++;
                UpdateReadModeText();
            }
        }
    }

    //Methods
    public void StartCaseCracker() {
        inCaseCracker = true;
        OnCaseCrackerStep("");
    }
    public void CloseCaseCracker() {
        if (inCaseCracker) {
            textBoxManager.blockInput = false;
            inCaseCracker = false;
            caseCrackerStep = -1;
            inReadMode = false;
            responses = new List<bool>();
        }
    }

    public void DisplayFile(ComputerFile _file) {
        currentFile = _file;
        currentPage = 0;
        inReadMode = true;
        UpdateReadModeText();
    }

    public void EnterInputMode() {
        inReadMode = false;
        textBoxManager.DisplayText("Check your documents by pressing Tab! \nWaiting for user input...", "Input");
        textBoxManager.CurrentInput = "";
    }

    public void UpdateReadModeText() {
        if(currentPage <= currentFile.content.Length - 1) {
            textBoxManager.DisplayText(currentFile.content[currentPage], currentFile.fileTitle);
        }
        else {
            EnterInputMode();
        }
    }

    public ComputerFile PullFile(string _fileTitle, out bool _valid) {
        for (int i = 0; i < dataBase.Length; i++) {
            if(_fileTitle == dataBase[i].fileIdentifier) {
                _valid = true;
                return dataBase[i];
            }
        }
        _valid = false;
        return null;
    }


    public void RunCommand(string _command) {
        string tempText = _command.Trim();
        string[] textSplit = tempText.Trim().Split(' ');
        bool valid;

        if(textSplit.Length >= 2) {
            if(tempText == "run case_cracker_3000") {
                StartCaseCracker();
                return;
            }

            if (textSplit[0] == "read") { //Read Command
                ComputerFile fileTemp = PullFile(textSplit[1], out valid);
                if (valid) {
                    DisplayFile(fileTemp);
                    return;
                }
                else {
                    textBoxManager.DisplayText("No valid file named " + "'" + textSplit[1] + '"' + " exists, are you sure that you did not commit any typos? check your file catalogue by pressing Tab.", "Read Error");
                    return;
                }
            } 
        }
        else {
            if(tempText == "read") {
                textBoxManager.DisplayText("Please specify a file name! \n usage read <file_name>", "Read Error");
                return;
            }
        }
            
        if (true) { // on no valid command
            textBoxManager.DisplayText("No valid file or command known as " + '"' + _command + '"' + " exists.\n Please refer to your user manual or file catalogue for valid entries by pressing Tab.", "Error");
        }
    }

    private bool IsCaseSolved() {
        if(responses.Count != 8) {
            return false;
        }

        if (responses[0] != false)  return false;
        if (responses[1] != true) return false;
        if (responses[2] != true) return false;
        if (responses[3] != false) return false;
        if (responses[4] != true) return false;
        if (responses[5] != false) return false;
        if (responses[6] != true) return false;
        if (responses[7] != true) return false;
        return true;
    }
    public void OnCaseCrackerStep(string _input) {
        textBoxManager.blockInput = caseCrackerStep == 9;
        caseCrackerStep++;

        if(caseCrackerStep > 9) {
            return;
        }

        //Set True or False
        if (caseCrackerStep > 1 && caseCrackerStep < 10) {
            //print(caseCrackerStep);
            if (_input != "" && _input.ToLower()[0] == 't' || _input != "" && _input.ToLower()[0] == 'f') {
                bool b = _input.ToLower()[0] == 't' ? true : false;
                //print(responses.Length);
                responses.Add(b);
            }
            else {
                textBoxManager.DisplayText("Case Cracker 3000 has crashed! \n Error 301 - Invalid Input. \n Please type in only True or False.", "Input");
                CloseCaseCracker();
                return;
            }
        }
        else {
            inReadMode = false;
        }

        DisplayFile(caseCrackerStepScreen[caseCrackerStep]);

        switch (caseCrackerStep) {
            case 0:
                //DisplayFile(caseCrackerStepScreen[0]);
                inReadMode = true;
                break;
            case 9:
                textBoxManager.blockInput = true;
                inReadMode = true;
                StartCoroutine(SolveCase());
                break;
            default:
                inReadMode = false;
                break;
        }

        
    }

    IEnumerator SolveCase() {
        yield return new WaitForSeconds(3f);
        if (IsCaseSolved()) {
            OnVictoryScreen();
        }
        else {
            OnWrongResponses();
        }
    }

    private void OnVictoryScreen() {
        CloseCaseCracker();
        //textBoxManager.DisplayText("Congratulations! you have a 99.98% chance of having solved this case!(please look into our user policy for more information \n You won!", "Case Cracker 3K");
        victorySource.PlayOneShot(victoryClip);
        inReadMode = true;
        inputEnabled = true;
        DisplayFile(youWin);
        textBoxManager.blockInput = false;
    }

    private void OnWrongResponses() {
        CloseCaseCracker();
        textBoxManager.DisplayText("Unfortunately according to the data things to do not add up.\n one or multiple of your deducations are wrong.\n Feel free to Try Again!", "Case Cracker 3k");
    }


}
