using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    public int npcID;
    public string npcName;
    public TextMeshPro nameText;

    // hierachy 상에서 프리팹의 부모
    public Transform chatBoxParent;
    public Transform questSignParent;
    public Canvas uiCanvas;

    private GameObject chatBoxPrefab;
    private QuestSign questSign;
    private Animator anim;

    private Transform prevRotation;
    private Vector3 cameraPos;

    private void Awake()
    {
        questSign = GetComponentInChildren<QuestSign>(true);
        NPCManager.Instance.AddScript(npcID, gameObject.GetComponent<NPC>());
        anim = GetComponentInChildren<Animator>();
        SetNameText();
    }

    public void Rotate(Vector3 targetPos)
    {
        prevRotation = transform;
        Vector3 targetDir = targetPos - transform.position;
        targetDir.y = 0;
        transform.rotation = Quaternion.LookRotation(targetDir);
    }

    public void CompleteQuestJump()
    {
        anim.SetTrigger("Jump");
    }

    public void OnChatBox(string talk)
    {
        if(chatBoxPrefab == null)
        {
            GameObject obj = ResourceManager.Instance.CHATBOX;
            chatBoxPrefab = Instantiate(obj);                           // Prefab 생성
            chatBoxPrefab.transform.parent = chatBoxParent;             // Hierachy 위치 지정
        }
        chatBoxPrefab.SetActive(true);
        chatBoxPrefab.GetComponent<ChatBoxSystem>().ChatBoxActive(talk, gameObject.transform, uiCanvas);
    }

    public void OffChatBox()
    {
        chatBoxPrefab.SetActive(false);
    }

    public void OnQuestSign()
    {
        questSign.ActiveQuestSign(gameObject, uiCanvas);
    }

    public void OffQuestSign()
    {
        questSign.OffQuestSign();
    }

    private void SetNameText()
    {
        nameText.text = npcName;
    }

    public void TMProLookCamera()
    {
        cameraPos = Camera.main.transform.position;
        Vector3 targetDir = (nameText.rectTransform.position - cameraPos).normalized;
        targetDir.x = 0;
        targetDir.z = 0;

        nameText.rectTransform.rotation = Quaternion.LookRotation(targetDir);
    }

    private void Update()
    {
        TMProLookCamera();
    }
}
