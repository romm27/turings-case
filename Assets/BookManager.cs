using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Director director;
    [SerializeField] Image pageDisplay;
    [SerializeField] AudioClip pageMoveSound;
    [SerializeField] AudioSource pageMoveAudioSource;
 
    [Header("Data")]
    [SerializeField] Sprite[] pages;

    private int currentPage = 0;

    public void Update() {
        ManagePages();
    }

    //Methods

    private void ManagePages() {
        if (director.inBookMode) {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                MovePage(-1);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                MovePage(1);
            }
        }
    }

    private void UpdatePage() {
        pageDisplay.sprite = pages[currentPage];
    }

    public void MovePage(int _factor) {
        if(_factor < 0) {
            if(currentPage > 0) {
                currentPage--;
                pageMoveAudioSource.PlayOneShot(pageMoveSound);
                UpdatePage();
            }
        }
        else {
            if(currentPage < pages.Length - 1) {
                currentPage++;
                pageMoveAudioSource.PlayOneShot(pageMoveSound);
                UpdatePage();
            }
        }
    }
}
