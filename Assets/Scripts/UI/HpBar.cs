using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// hpbar용 카메라와 캔버스를 따로 만들어서
// camera를 메인카메라와 같이 볼 수 있는 모드로 만들고
// 슬라이더로 hpbar를 만들어
// 플레이어, 몬스터의 위치를 월드좌표에서 스크린좌표로 바꾸고 스크린좌표에서 캔버스로컬좌표로 변경후
// hpbar를 그 위치의 위에 따라다니게 해줘 hpbar의 위치를 고정시킨다.
// 그리고 hpbar는 prefab으로 만들어 resourcesload 하여 

public class HpBar : MonoBehaviour
{
    public Slider _hpBar;
    public Canvas canvas;
    public Camera uiCamera;
    public RectTransform rectParent;
    public RectTransform rectHpBar;

    public Vector3 offset = Vector3.zero;
    public GameObject target;
    //public Transform targetTr;

    void Start()
    {
        _hpBar = GetComponent<Slider>();                         
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHpBar = _hpBar.GetComponent<RectTransform>();
    }

    public void UpdateHpBar(int maxHp, int curHp)
    {
        if (curHp <= 0)
            _hpBar.value = 0;
        if(_hpBar != null)
            _hpBar.value = (float)curHp / maxHp;
    }

    void SetHpBarPos()
    {
        if (target == null)
            return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position + offset);

        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        Vector2 localPos = Vector2.zero;
        // 1. 부모 캔버스 space, 2. hpbar 달릴 obj의 스크린좌표, 3. uiCamera 4. 좌표담을 벡터
        // hpBar가 달릴 Obj의 위치를 화면좌표에서 로컬좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);

        rectHpBar.localPosition = localPos;
    }

    private void LateUpdate()
    {
        SetHpBarPos();
    }
}
