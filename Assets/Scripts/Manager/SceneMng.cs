using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Scene 넘버로 사용
public enum SceneState
{
    LogoScene,  
    TitleScene,
    SelectScene,
    TownScene,
    GameScene,
    TrainingScene,
}

public class SceneMng : MonoBehaviour
{
    Dictionary<SceneState, BaseScene> sceneDic = new Dictionary<SceneState, BaseScene>();

    private SceneState currScene = SceneState.LogoScene;
    private float delayTime = 2.0f;
    private bool loadingDone = false;

    private static SceneMng instance;
    public static  SceneMng Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("SceneMng", typeof(SceneMng));
                DontDestroyOnLoad(obj);

                instance = obj.GetComponent<SceneMng>();
            }
            return instance;
        }
    }

    // 씬 DICTIONARY에 key에맞는 script등록
    public T AddScript<T>(SceneState sceneState) where T : BaseScene
    {
        if(!sceneDic.ContainsKey(sceneState))
        {
            GameObject obj = new GameObject(typeof(T).ToString(), typeof(T));

            T t = obj.GetComponent<T>();
            t.transform.parent = transform;

            sceneDic.Add(sceneState,t);

            return t;
        }
        return sceneDic[sceneState].GetComponent<T>();
    }

    // 다음씬을 로딩하는 시점에서
    // 현재씬을 마무리하고, 다음씬을 위한 데이터를 미리 준비하는 코드
    public void ChangeScene(SceneState nextScene, bool useLoadingScene)
    {
        SoundManager.Instance.Clear();

        if(useLoadingScene)
        {
            StartCoroutine(AsyncLoadScene(nextScene));
            loadingDone = true;
        }
        else if(!useLoadingScene)
        {
            sceneDic[currScene].Exit();
            sceneDic[nextScene].LoadScene(nextScene);
            StartCoroutine(LoadSceneWithoutLoading(nextScene));
        }
    }

    // 로딩씬을 사용한 씬이동
    IEnumerator AsyncLoadScene(SceneState nextScene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene.ToString());

        UIMng.Instance.OpenUI<UILoading>(UIType.UILoading).SetLoadingImg(nextScene);
        sceneDic[currScene].Exit();

        while (!operation.isDone)
        {
            if (operation.progress < 0.85f)
            {
                UIMng.Instance.UIUpdate(UIType.UILoading, operation.progress);
                yield return null;
            }
            if (operation.progress >= 0.85f)
            {
                // 다음씬 이동준비 미완료
                if (!loadingDone)
                {
                    float waitOperationDone = 0.85f;
                    UIMng.Instance.UIUpdate(UIType.UILoading, waitOperationDone);
                }
                // 다음씬 이동준비 완료
                else if (loadingDone && operation.isDone)
                {
                    float endProgress = 0.85f;
                    while(endProgress != 1.0f)
                    {
                        endProgress = Mathf.Lerp(endProgress, 1.0f, 2.0f);
                        UIMng.Instance.UIUpdate(UIType.UILoading, endProgress);
                        yield return null;
                    }
                }
                yield return null;
            }
            UIMng.Instance.UIUpdate(UIType.UILoading, operation.progress);
            yield return null;
        }
        // operation.isDone 일 경우
        operation.allowSceneActivation = true;
        SetActive(nextScene);

        PortalManager.Instance.GetPortalList(nextScene);
        sceneDic[nextScene].LoadScene(nextScene);
        currScene = nextScene;

        // 씬변환 종료
        UIMng.Instance.CloseUI(UIType.UILoading);
        sceneDic[currScene].Enter(currScene);
    }
    
    // 로딩씬을 사용안하는 씬이동
    IEnumerator LoadSceneWithoutLoading(SceneState nextScene)
    {
        AsyncOperation operation =  SceneManager.LoadSceneAsync(nextScene.ToString());

        while(!operation.isDone)
        {
            yield return null;
        }
        SetActive(nextScene);
        currScene = nextScene;

        //씬변환 종료
        sceneDic[currScene].Enter(currScene);
        yield return null;
    }

    public void SetActive(SceneState currScene)
    {
        foreach(SceneState scene in sceneDic.Keys)
        {
            if(scene == currScene)
            {
                sceneDic[scene].gameObject.SetActive(true);
            }
            else
                sceneDic[scene].gameObject.SetActive(false);
        }
        // 새로운 씬의 메인카메라를 받아옴
        Player.Instance.GetPlayerCamera();
        // 플레이어위치에 카메라 배치
        if(Player.Instance.playerCamera != null)
            Player.Instance.playerCamera.CameraInit();
    }

    public bool LoadingState()
    {
        if (currScene == SceneState.LogoScene
            || currScene == SceneState.SelectScene
            || currScene == SceneState.TitleScene)
        {
            return true;
        }
        return false;
    }
}

