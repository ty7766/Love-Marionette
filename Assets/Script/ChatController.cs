using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatController : MonoBehaviour
{
    [Header("Managers")]
    public APIManager apiManager;

    [Header("UI Components")]
    public TMP_InputField chatInput;
    public Button sendButton;
    public ScrollRect chatScrollRect;
    public Transform chatContent;

    [Header("Prefabs")]
    public GameObject userBubblePrefab;
    public GameObject aiBubblePrefab;

    [Header("Game State")]
    public UserProfile myProfile;
    private int currentRiskScore = 0;
    private List<ChatMessage> chatHistory = new List<ChatMessage>();

    [Header("Ending Link")]
    public EndingManager endingManager;

    void Start()
    {
        sendButton.onClick.AddListener(OnSendClick);
        chatInput.onValidateInput += ValidateInput;

        // 게임 시작
        apiManager.StartCoroutine(apiManager.StartGame(myProfile, OnAIResponse, OnError));
    }

    private char ValidateInput(string text, int charIndex, char addedChar)
    {
        if (addedChar == '\n')
        {
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                Invoke("OnSendClick", 0.01f);

                return '\0';
            }
        }
        return addedChar;
    }

    void OnSendClick()
    {
        // 앞뒤 공백 제거
        string msg = chatInput.text.Trim();

        // 내용이 없으면 취소
        if (string.IsNullOrEmpty(msg)) return;

        // 메시지 전송
        SendMessageDirectly(msg);

        // 입력창 비우기 (엔터 납치 후 실행되므로 안전함)
        chatInput.text = "";

        // 포커스 유지 (계속 채팅 칠 수 있게)
        chatInput.ActivateInputField();
    }

    void SendMessageDirectly(string message)
    {
        CreateBubble(message, true);
        chatHistory.Add(new ChatMessage { role = "user", content = message });

        UserAction action = new UserAction
        {
            user_profile = myProfile,
            history = chatHistory,
            user_input = message,
            current_risk_score = currentRiskScore
        };

        apiManager.StartCoroutine(apiManager.SendPostRequest("/chat", JsonUtility.ToJson(action), OnAIResponse, OnError));
    }

    void OnAIResponse(GameResponse response)
    {
        int previousRisk = currentRiskScore;
        currentRiskScore += response.risk_change;
        currentRiskScore = Mathf.Clamp(currentRiskScore, 0, 100);

        CreateBubble(response.scammer_dialogue, false);
        chatHistory.Add(new ChatMessage { role = "assistant", content = response.scammer_dialogue });

        if (response.is_game_over)
        {
            chatInput.interactable = false;

            // 리포트 요청 (기존 유지)
            StartCoroutine(apiManager.RequestReport(chatHistory, response.ending_type, (reportText) =>
            {
                endingManager.SetReportData(reportText);
            }));

            if (!string.IsNullOrEmpty(response.ending_type) &&
               (response.ending_type.Contains("성공") ||
                response.ending_type.Contains("승리") ||
                response.ending_type.Contains("예방") ||
                response.ending_type.Contains("차단")))
            {
                endingManager.PlayClearEnding();
            }
            else
            {
                endingManager.PlayDefeatedEnding();
            }
        }
    }

    public void RollbackGameState()
    {
        // 1. 게임 오버 상태 해제
        chatInput.interactable = true;
        chatInput.ActivateInputField();

        // 2. 리스크 점수 되돌리기
        if (currentRiskScore >= 100)
        {
            currentRiskScore = 90;
        }

        // 3. 대화 기록에서 '게임 오버 멘트' 삭제하기
 
        if (chatHistory.Count > 0)
        {
            chatHistory.RemoveAt(chatHistory.Count - 1);

            if (chatContent.childCount > 0)
            {
                Destroy(chatContent.GetChild(chatContent.childCount - 1).gameObject);
            }
        }

        Debug.Log("게임 상태가 복구되었습니다. 다시 시도하세요!");
    }

    void OnError(string msg)
    {
        CreateBubble($"[오류] {msg}", false);
    }

    void CreateBubble(string text, bool isUser)
    {
        GameObject row = Instantiate(isUser ? userBubblePrefab : aiBubblePrefab, chatContent);
        TextMeshProUGUI tmp = row.GetComponentInChildren<TextMeshProUGUI>();

        tmp.text = text;

        StartCoroutine(AlignAndScroll(row));
    }

    IEnumerator AlignAndScroll(GameObject newRow)
    {
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(newRow.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContent.GetComponent<RectTransform>());
        yield return new WaitForEndOfFrame();
        chatScrollRect.verticalNormalizedPosition = 0f;
    }
}