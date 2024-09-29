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
    [SerializeField] TextMeshProUGUI dataText;
    [SerializeField] ComputerCPU cpu;
    [SerializeField] TMP_InputField inputField;

    [Header("Audio References")]
    [SerializeField] AudioSource keyTyped;

    [Header("Settings")]
    public float charDelay = 0.05f;
    public float inputTextBlinkInterval = 0.1f;

    private bool blinkingInput = false;
    private bool writingToScreen = false;

    public string CurrentInput {
        get {
            return inputField.text;
        }
        set {
            inputField.text = value;
        }
    }

    public void Update() {
        if (cpu.inReadMode) {
            if (mainText.text.Length == mainText.maxVisibleCharacters) {
                writingToScreen = false;
                if (!blinkingInput) {
                    StartCoroutine(BlinkInput());
                    blinkingInput = true;
                }
            }
        }
        else {
            ReadToInput();
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
        inputText.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
        inputField.OnPointerClick(new PointerEventData(EventSystem.current));

        if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) {
            OnKeyTyped();
            if (CurrentInput != "") {
                cpu.RunCommand(CurrentInput);
                CurrentInput = "";
            }
            else {
                cpu.EnterInputMode();
            }
        }

        if(inputField.text.Length > 24) {
            inputField.text = inputField.text.Substring(0, 24);
        }

        
        inputText.text = '>' + CurrentInput;
    }

    public void OnKeyTyped() {
        keyTyped.PlayOneShot(keyTyped.clip);
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
}
