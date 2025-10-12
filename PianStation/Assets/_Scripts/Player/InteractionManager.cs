using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class DialogueMapping
{
    public string objectName;
    public Dialogue dialogue;
}

public class InteractionManager : MonoBehaviour
{
    public bool IsDialogueActive => dialoguePanel.activeSelf;
    public static InteractionManager instance;

    [Header("UI 연결")]
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    [Header("대화 연출 설정")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float autoAdvanceTime = 5f;

    [Header("대사 데이터베이스")]
    [SerializeField] private List<DialogueMapping> dialogueMappings;
    private Dictionary<string, Dialogue> dialogueDictionary;

    private Queue<string> sentences;
    private Coroutine typingCoroutine;
    private IPostDialogueAction[] currentPostActions;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        sentences = new Queue<string>();

        dialogueDictionary = new Dictionary<string, Dialogue>();
        foreach (var mapping in dialogueMappings)
        {
            dialogueDictionary[mapping.objectName] = mapping.dialogue;
        }
    }

    void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
                dialogueText.text = sentences.Peek();
                
                StopAllCoroutines();
                StartCoroutine(AutoAdvance());
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }
    
    public void StartInteraction(string objectName, IPostDialogueAction[] actions)
    {
        currentPostActions = actions;

        if (dialogueDictionary.TryGetValue(objectName, out Dialogue dialogue))
        {
            dialoguePanel.SetActive(true);
            npcNameText.text = dialogue.npcName;

            sentences.Clear();
            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }
            DisplayNextSentence();
        }
        else
        {
            Debug.LogWarning($"'{objectName}' 이름에 해당하는 대사를 찾을 수 없습니다.");
            EndDialogue();
        }
    }

    public void DisplayNextSentence()
    {
        StopAllCoroutines();

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentenceToType = sentences.Dequeue();
        typingCoroutine = StartCoroutine(TypeSentence(sentenceToType));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        
        typingCoroutine = null; 
        StartCoroutine(AutoAdvance());
    }
    
    IEnumerator AutoAdvance()
    {
        yield return new WaitForSeconds(autoAdvanceTime);
        DisplayNextSentence();
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        StopAllCoroutines();

        if (currentPostActions != null && currentPostActions.Length > 0)
        {
            foreach (IPostDialogueAction action in currentPostActions)
            {
                action.OnDialogueEnd();
            }
        }
        currentPostActions = null;
    }
}