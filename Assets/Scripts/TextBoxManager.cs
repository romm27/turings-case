using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TextBoxManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] TextMeshProUGUI inputText;
    [SerializeField] TextMeshProUGUI statsText;
    [SerializeField] ComputerCPU cpu;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Director director;

    [Header("Audio References")]
    [SerializeField] SoundManager soundManager;

    [Header("Settings")]
    public float charDelay = 0.05f;
    public float inputTextBlinkInterval = 0.1f;
    public float inputInterval = 0.1f;

    private bool blinkingInput = false;
    private bool writingToScreen = false;
    private bool underscoreBlink = false;
    [HideInInspector]public bool blockInput = false;
    private bool alternativeInputPrompt = false;

    public string CurrentInput {
        get {
            return inputField.text;
        }
        set {
            inputField.text = value;
        }
    }

    public void Start() {
        StartCoroutine(ManageunderscoreBlink());
    }

    public void Update() {
        statsText.gameObject.SetActive(cpu.inReadMode && !cpu.inCaseCracker);
        CheckForForwardText();
        if (!director.inBookMode) {
            if (cpu.inReadMode && !cpu.inCaseCracker) {
                //Input
                if (cpu.CurrentPage > 0) {
                    if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                        cpu.CurrentPage--;
                        cpu.UpdateReadModeText();
                        OnKeyTyped();
                    }
                }
                if (cpu.CurrentPage < cpu.CurrentFileSize - 1) {
                    if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                        cpu.CurrentPage++;
                        cpu.UpdateReadModeText();
                        OnKeyTyped();
                    }
                }

                statsText.text = "p:" + (cpu.CurrentPage + 1).ToString() + "/" + cpu.CurrentFileSize;
            }
            else {
                if (true) {
                    inputText.alignment = TextAlignmentOptions.Left;
                    ReadToInput();
                }
            }
        }

        if (cpu.inReadMode) {
            if (mainText.text.Length == mainText.maxVisibleCharacters) {
                writingToScreen = false;
                inputText.alignment = TextAlignmentOptions.Center;
                alternativeInputPrompt = cpu.inCaseCracker;
                string alt = alternativeInputPrompt ? ">Press Enter To Continue<" : "use < > to move or Esc to Leave.";
                inputText.text = blockInput? "Wait" : alt;
                if (!blinkingInput) {
                    StartCoroutine(BlinkInput());
                    blinkingInput = true;
                }
            }
            if (!blockInput) {
                
            }
        }
    }

    //Methods
    public void DisplayText(string _text, string _title) {
        writingToScreen = true;
        inputText.gameObject.SetActive(false);
        titleText.text = _title;
        mainText.text = _text;
        StartCoroutine(TextLoop());
        mainText.maxVisibleCharacters = 0;
    }

    public void ReadToInput() {
        if (cpu.inputEnabled) {
            inputText.gameObject.SetActive(true);
        }
        EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
        inputField.OnPointerClick(new PointerEventData(EventSystem.current));

        if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) {
            OnKeyTyped();
            if (CurrentInput != "") {
                if (cpu.inCaseCracker) {
                    cpu.OnCaseCrackerStep(CurrentInput);
                }
                else {
                    cpu.RunCommand(CurrentInput);
                }
                CurrentInput = "";
            }
            else{
                if (cpu.inCaseCracker) {
                    cpu.OnCaseCrackerStep("");
                }
                else {
                    cpu.EnterInputMode();
                }
            }
        }

        int max = 38;
        if(inputField.text.Length > max) {
            inputField.text = inputField.text.Substring(0, max);
        }

        string final = underscoreBlink ? "_" : "";
        inputText.text = '>' + CurrentInput + final;
    }

    private void CheckForForwardText() {
        if (!cpu.inCaseCracker) {
            if (cpu.inReadMode) {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return)) {
                    OnKeyTyped();
                    cpu.EnterInputMode();
                    cpu.CloseCaseCracker();

                }
                if (cpu.IsLastPage) {
                    if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) {
                        OnKeyTyped();
                        cpu.EnterInputMode();
                    }
                }
            }
        }
        else {
            
        }
    }

    public void OnKeyTyped() {
        soundManager.PlayClickSound();
    }

    public IEnumerator BlinkInput() {
        inputText.gameObject.SetActive(!inputText.gameObject.activeInHierarchy);
        

        yield return new WaitForSeconds(inputTextBlinkInterval);
        if(mainText.text.Length == mainText.maxVisibleCharacters) {
            StartCoroutine(BlinkInput());
        }
        else {
            blinkingInput = false;
        }
    }

    public IEnumerator TextLoop() {
        if(mainText.maxVisibleCharacters < mainText.text.Length) {
            mainText.maxVisibleCharacters++;
        }

        if (writingToScreen) {
            yield return new WaitForSeconds(charDelay);
            StartCoroutine(TextLoop());
        }
    }

    public IEnumerator ManageunderscoreBlink() {
        underscoreBlink = !underscoreBlink;
        yield return new WaitForSeconds(inputTextBlinkInterval);
        StartCoroutine(ManageunderscoreBlink());
    }
}
