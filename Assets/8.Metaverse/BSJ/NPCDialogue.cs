using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCDialogue : MonoBehaviour
{
    public string dialogueData;
    public string[] dialogueLines;
    public Text dialogueText;
    public GameObject dialoguePanel;
    public float delayBetweenDialogues = 1.5f; // 대화 사이의 지연 시간 (초)

    private int currentLine = 0;
    private bool isDisplayingDialogue = false;
    private bool isPrintingDialogue = false;
    private void OnValidate()
    {
        dialogueLines = dialogueData.Split("\\n");
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.attachedRigidbody == null)
        {
            return;
        }
        NetPlayer netPlayer = other.attachedRigidbody.GetComponent<NetPlayer>();
        if (netPlayer == null)
        {
            return;
        }
        if (netPlayer.isLocalPlayer)
        {
            if (!isDisplayingDialogue && Input.GetKeyDown(KeyCode.F))
            {
                StartDialogue();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialoguePanel.SetActive(false);
            isDisplayingDialogue = false;
            currentLine = 0;
            StopAllCoroutines();
        }
    }

    private void Update()
    {
        if (isDisplayingDialogue && Input.GetKeyDown(KeyCode.F))
        {
            OnNextButtonClick();
        }
    }

    private void StartDialogue()
    {
        if(isDisplayingDialogue)
        {
            return;
        }

        dialoguePanel.SetActive(true);
        isDisplayingDialogue = true;
        currentLine = 0;
        StartCoroutine(DisplayDialogueCoroutine());
    }

    private IEnumerator DisplayDialogueCoroutine()
    {
        if(currentLine >= dialogueLines.Length)
        {
            dialoguePanel.SetActive(false);
            currentLine = 0;
            isDisplayingDialogue = false;
            yield break;
        }
        isPrintingDialogue = true;
        StartCoroutine(PrintCharacterCoroutine(dialogueLines[currentLine]));
        yield return new WaitForSeconds(delayBetweenDialogues);
        currentLine++;
    }

    private void OnNextButtonClick()
    {
        if(!isPrintingDialogue)
        {
            StartCoroutine(DisplayDialogueCoroutine());
        }
    }
    private IEnumerator PrintCharacterCoroutine(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.05f);
        }
        isPrintingDialogue = false;
    }
}
