using TMPro;
using UnityEngine;

public class ChatBoxSystem : MonoBehaviour
{
    private string currSentence;
    public TextMeshProUGUI textmeshpro;

    private Transform chatBoxPos;
    private RectTransform rectParent;
    private RectTransform rectFollowTarget;

    private float chatBoxOffsetY = 110f;

    // TypeEffect
    private int typeIndex;
    private float charPerSeconds = 10f;
    private float interval;
    public GameObject EndCursor;

    // 대화를 받아와서 채팅창에 표시해주는 함수
    public void ChatBoxActive(string talkData, Transform chatBoxPos, Canvas canvas)
    {
        rectParent = canvas.GetComponent<RectTransform>();
        rectFollowTarget = GetComponent<RectTransform>();
        this.chatBoxPos = chatBoxPos;

        currSentence = talkData;
        TypeEffectStart();
    }

    private void TypeEffectStart()
    {
        textmeshpro.text = "";
        typeIndex = 0;
        EndCursor.SetActive(false);

        interval = 1.0f / charPerSeconds;
        Invoke("TypeEffecting", interval);
    }

    private void TypeEffecting()
    {
        if (textmeshpro.text == currSentence)
        {
            TypeEffectEnd();
            return;
        }

        textmeshpro.text += currSentence[typeIndex];
        typeIndex++;

        Invoke("TypeEffecting", interval);
    }

    private void TypeEffectEnd()
    {
        EndCursor.SetActive(true);
    }

    // 이부분을 update, IEnumerator 로 수정해야함.
    private void SetChatBoxPos()
    {
        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(chatBoxPos.position);

        Vector2 localPosition = new Vector2(0, 0);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, targetScreenPos,  null, out localPosition);

        rectFollowTarget.localPosition = new Vector2(localPosition.x, localPosition.y + chatBoxOffsetY);
    }

    private void Update()
    {
        if(chatBoxPos!= null)
        {
            SetChatBoxPos();
        }
    }
}
