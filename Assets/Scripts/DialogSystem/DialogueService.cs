using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueService : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI nameText;  
    [SerializeField] private TextMeshProUGUI dialogueText; 
    [SerializeField] private Image portrait;

    [SerializeField] private Dialogue testDialogue; 

    private Queue<Speach> speachQueue = new Queue<Speach>(); 
    private Queue<string> sentences = new Queue<string>();

    public event Action DialogueEnded = delegate { };

    void Start()
    {

        //StartDialogue(testDialogue);
    }

   public void StartDialogue (Dialogue dialogue)
    {
        if (speachQueue != null )
        {
            speachQueue.Clear();
        }
        
        foreach (Speach speach in dialogue.speach)
        {
            speachQueue.Enqueue(speach);
        }


        DisplayNextCharacterSpeach();

    }

    private void DisplayNextCharacterSpeach()
    {
        Debug.Log("next character speak");
        if (speachQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        sentences.Clear();
        Speach characterSpeach = speachQueue.Dequeue();

        nameText.text = characterSpeach.name;
        portrait.sprite = characterSpeach.portraite;
        
        foreach (string sentence in characterSpeach.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        Debug.Log("next line");
        if (sentences.Count == 0)
        {
            DisplayNextCharacterSpeach();
            return;
        }

        //dialogueText.text = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentences.Dequeue()));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        gameObject.SetActive(false);
        DialogueEnded();
        Debug.Log("that`s all folks");
    }
}
