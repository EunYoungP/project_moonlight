using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    private TitleSceneDialog titleSceneDialog;

    private float fadeInTime = 2.0f;
    private float fadeOutTime = 2.0f;

    private bool waitStartState = false;

    private void Start()
    {
        titleSceneDialog = GameObject.Find("TitleSceneDialog").GetComponent<TitleSceneDialog>();
        SoundManager.Instance.Play("BGM/01_majestic_highlands", SOUND.Bgm);
        FadeIn();
    }

    public override void LoadScene(SceneState currScene)
    {
    }

    public void FadeIn()
    {
        StartCoroutine(SetWaitTouch());
    }

    IEnumerator SetWaitTouch()
    {
        yield return new WaitForSeconds(fadeInTime);
        waitStartState = true;
        yield return null;
    }

    private void StartToutch()
    {
        if (Input.GetMouseButtonUp(0))
        {
            titleSceneDialog.EnterGameState();
            FadeOut();
            waitStartState = false;
        }
    }

    public void FadeOut()
    {
        UIMng.Instance.OpenUI<Fade>(UIType.Fade).FadeOut(fadeOutTime);

        Invoke("ChangeScene", fadeOutTime);
    }

    public void ChangeScene()
    {
        SceneMng.Instance.ChangeScene(SceneState.TrainingScene, false);
    }

    private void Update()
    {
        if(waitStartState)
            StartToutch();
    }
}
