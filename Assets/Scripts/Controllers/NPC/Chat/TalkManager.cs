using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    // <npcID, npc대화목록>
    private Dictionary<int, string[]> talkDatas = new Dictionary<int, string[]>();

    private static TalkManager instance;
    public static TalkManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("TalkManager", typeof(TalkManager));
                DontDestroyOnLoad(obj);
                instance = obj.GetComponent<TalkManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        talkDatas.Clear();
        CreateChatData();
    }

    #region 대화정보
    private void CreateChatData()
    {
        // 일반 대화
        talkDatas.Add(1000, new string[] {"안녕하세요 저는 열심히 수련하는 도비입니다!",
                                          "마을에 오신것을 환영합니다!",
                                          "앗 저를 지나치지 말아주세요..!"});

        talkDatas.Add(2000, new string[] {"초원골렘을 토벌해줄 용병들은 언제 도착하는거지..."});

        talkDatas.Add(3000, new string[] { "좋은 물건들 구경하고가세요~" });


        // 퀘스트 대화
        talkDatas.Add(20 + 1000, new string[] {"처음 보는 얼굴이군!",
                                               "이곳은 마을의 수련관이라네.",
                                               "환영의 의미로 이 검을 받아주게나!"});

        talkDatas.Add(40 + 1000, new string[] {"검은 잘 받으셨나?",
                                               "훈련을 하고싶으시다면 좋은 방법이있다네!",
                                               "이웃마을의 루카를 찾아가주게."});

        talkDatas.Add(50 + 2000, new string[] {"오! 당신이 루인을 통해 찾아온 수련생입니까?",
                                               "마침 잘됐군요!",
                                               "북부평원의 초원골렘을 잡아주시겠습니까?"});

        talkDatas.Add(70 + 2000, new string[] {"초원골렘 토벌에 성공하셨군요!",
                                                "초원골렘의 눈을 판매하고 싶으시다면,",
                                               "세라보그성의 마리아에게 가보세요!"});

        talkDatas.Add(80 + 3000, new string[] {"오! 이건 초록골렘의 눈이구만?",
                                               "구하기 쉽지 않았을텐데...고맙네!"});
    }
    #endregion

    public string GetTalk(int npcID, int talkIndex)
    {
        // 퀘스트 아닌 상태에서 기본 대화 나오게 하기
        if (!talkDatas.ContainsKey(npcID))
        {
            if (!talkDatas.ContainsKey(npcID - npcID % 10))
                return GetTalk(npcID - npcID % 100, talkIndex);
            else
                return GetTalk(npcID - npcID % 10, talkIndex);
        }

        if(talkIndex == talkDatas[npcID].Length)
            return null;
        else
            return talkDatas[npcID][talkIndex];
    }
}