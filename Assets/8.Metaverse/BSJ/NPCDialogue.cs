using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCDialogue : MonoBehaviour
{
    public string dialogueData;
    public string[] dialogueLines;

    private int currentLine = 0;

    private void OnValidate()
    {
        dialogueLines = dialogueData.Split("\\n");
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
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
                netPlayer.SetNPC(this);
                netPlayer.IsNearNPC = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
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
            netPlayer.EndDialog();
            currentLine = 0;
            StopAllCoroutines();
        }
    }
    public void StartDialogue()
    {
        currentLine = 0;
    }
    public bool TryGetCurrentLine(out string line)
    {
        line = "";
        if(currentLine >= dialogueLines.Length)
        {
            currentLine = 0;
            return false;
        }
        line = dialogueLines[currentLine];
        currentLine++;
        return true;
    }
}
