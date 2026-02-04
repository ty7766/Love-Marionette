using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public enum EndingType { Bad, Clear }

    [Header("UI Groups")]
    public CanvasGroup endingPanelGroup;      // 전체 검은 배경
    public CanvasGroup newsPanelGroup;        // 뉴스 패널

    [Header("Text Objects")]
    public TextMeshProUGUI dialogueText;      // 독백 대사
    public TextMeshProUGUI endingTitleText;   // 엔딩 타이틀

    [Header("AI Report UI")]
    public TextMeshProUGUI reportTitleText;   // AI 리포트 제목
    public TextMeshProUGUI aiReportText;      // AI 리포트 본문

    [Header("Button Groups")]
    public GameObject badEndingButtons;
    public GameObject clearEndingButtons;

    [Header("Scene Images")]
    public Image badNewsImage;
    public Image clearNewsImage;
    public GameObject nextCursorIcon;         // 역삼각형 커서

    [Header("Data")]
    [TextArea(1, 3)] public string[] badDialogues;
    [TextArea(1, 3)] public string[] clearDialogues;
    public string badEndingTitle = "Ending[1] : 잊혀진 기억";
    public string clearEndingTitle = "Ending[2] : 새로운 시작";

    [Header("Settings")]
    public float fadeDuration = 1.5f;
    public float typingSpeed = 0.1f;
    public float interludeDuration = 1.0f;

    private Queue<string> dialogueQueue = new Queue<string>();
    public ChatController chatController;

    // 내부 변수
    private Vector3 cursorOriginPos;
    private string fetchedReport = "";
    private bool isReportLoaded = false;

    void Start()
    {
        endingPanelGroup.alpha = 0;
        endingPanelGroup.blocksRaycasts = false;
        endingPanelGroup.gameObject.SetActive(false);
        newsPanelGroup.alpha = 0;

        dialogueText.text = "";
        endingTitleText.text = "";

        if (reportTitleText) reportTitleText.alpha = 0;
        if (aiReportText) aiReportText.text = "";

        badEndingButtons.SetActive(false);
        clearEndingButtons.SetActive(false);

        if (badNewsImage) badNewsImage.gameObject.SetActive(false);
        if (clearNewsImage) clearNewsImage.gameObject.SetActive(false);

        if (nextCursorIcon)
        {
            nextCursorIcon.SetActive(false);
            cursorOriginPos = nextCursorIcon.transform.localPosition;
        }
    }

    // 리포트 데이터 받는 함수
    public void SetReportData(string text)
    {
        fetchedReport = text;
        isReportLoaded = true;
    }

    public void PlayDefeatedEnding() => StartCoroutine(EndingSequence(EndingType.Bad));
    public void PlayClearEnding() => StartCoroutine(EndingSequence(EndingType.Clear));

    IEnumerator EndingSequence(EndingType type)
    {
        //데이터 세팅
        Image targetNewsImage = (type == EndingType.Bad) ? badNewsImage : clearNewsImage;
        GameObject targetButtons = (type == EndingType.Bad) ? badEndingButtons : clearEndingButtons;
        string[] targetDialogues = (type == EndingType.Bad) ? badDialogues : clearDialogues;
        string targetTitle = (type == EndingType.Bad) ? badEndingTitle : clearEndingTitle;

        //화면 암전
        endingPanelGroup.gameObject.SetActive(true);
        endingPanelGroup.blocksRaycasts = true;
        endingPanelGroup.interactable = true;

        yield return StartCoroutine(FadeCanvasGroup(endingPanelGroup, 0f, 1f));
        yield return new WaitForSeconds(0.5f);

        //뉴스 패널 등장
        if (targetNewsImage != null) targetNewsImage.gameObject.SetActive(true);
        yield return StartCoroutine(FadeCanvasGroup(newsPanelGroup, 0f, 1f));
        yield return new WaitForSeconds(0.5f);

        //독백 출력
        dialogueQueue.Clear();
        foreach (string line in targetDialogues) dialogueQueue.Enqueue(line);

        while (dialogueQueue.Count > 0)
        {
            dialogueText.text = dialogueQueue.Dequeue();

            if (nextCursorIcon) nextCursorIcon.SetActive(true);

            // 스페이스바 입력 대기
            while (!Input.GetKeyDown(KeyCode.Space))
            {
                if (nextCursorIcon)
                {
                    float yOffset = Mathf.Sin(Time.time * 10f) * 5f;
                    nextCursorIcon.transform.localPosition = cursorOriginPos + new Vector3(0, yOffset, 0);
                }
                yield return null;
            }
            if (nextCursorIcon) nextCursorIcon.SetActive(false);

            yield return null;
        }
        dialogueText.text = "";

        yield return StartCoroutine(FadeCanvasGroup(newsPanelGroup, 1f, 0f));
        if (targetNewsImage != null) targetNewsImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(interludeDuration);

        yield return StartCoroutine(TypewriterWithUnderline(targetTitle));
        yield return new WaitForSeconds(0.5f);

        if (aiReportText != null)
        {
            // 초기 세팅: 투명하게 시작
            aiReportText.text = "AI가 당신의 대화를 분석 중입니다...";
            Color bodyColor = aiReportText.color;
            bodyColor.a = 0;
            aiReportText.color = bodyColor;

            // 제목도 투명하게 세팅
            if (reportTitleText)
            {
                reportTitleText.alpha = 0;
            }

            // 페이드 인 (본문과 제목 동시 등장)
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime;

                bodyColor.a = t;
                aiReportText.color = bodyColor;
                if (reportTitleText) reportTitleText.alpha = t;

                yield return null;
            }

            // 데이터 대기 (5초 타임아웃)
            float waitTime = 0;
            while (!isReportLoaded && waitTime < 5.0f)
            {
                waitTime += Time.deltaTime;
                yield return null;
            }

            // 결과 출력
            if (isReportLoaded)
                aiReportText.text = fetchedReport;
            else
                aiReportText.text = "데이터 분석에 시간이 걸려 내용을 불러오지 못했습니다. 하지만 훌륭한 선택이었습니다!";
        }

        yield return new WaitForSeconds(1.0f);

        if (targetButtons != null) targetButtons.SetActive(true);
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end)
    {
        float t = 0f;
        cg.alpha = start;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            cg.alpha = Mathf.Lerp(start, end, t);
            yield return null;
        }
        cg.alpha = end;
    }

    IEnumerator TypewriterWithUnderline(string fullText)
    {
        endingTitleText.text = "";
        string currentText = "";
        foreach (char letter in fullText)
        {
            endingTitleText.text = $"{currentText}<u>{letter}</u>";
            yield return new WaitForSeconds(typingSpeed);
            currentText += letter;
            endingTitleText.text = currentText;
        }
    }

    public void OnClickRestart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    public void OnClickRollback()
    {
        StopAllCoroutines();
        endingPanelGroup.alpha = 0;
        endingPanelGroup.blocksRaycasts = false;
        endingPanelGroup.gameObject.SetActive(false);
        newsPanelGroup.alpha = 0;

        if (reportTitleText) reportTitleText.alpha = 0;
        if (aiReportText) aiReportText.text = "";

        badEndingButtons.SetActive(false);
        clearEndingButtons.SetActive(false);
        dialogueText.text = "";
        endingTitleText.text = "";

        chatController.RollbackGameState();
    }
    public void OnClickQuit() => Application.Quit();
}