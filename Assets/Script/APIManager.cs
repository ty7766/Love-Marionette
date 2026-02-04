using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;
using System.Collections.Generic;

public class APIManager : MonoBehaviour
{
    // 로컬 서버 주소
    private const string BASE_URL = "http://127.0.0.1:8000";

    // 1. 게임 시작 요청
    public IEnumerator StartGame(UserProfile profile, Action<GameResponse> onSuccess, Action<string> onError)
    {
        string json = JsonUtility.ToJson(profile);
        yield return SendPostRequest("/start_game", json, onSuccess, onError);
    }

    // 2. 선택지 고르기 요청
    public IEnumerator MakeChoice(UserAction action, Action<GameResponse> onSuccess, Action<string> onError)
    {
        string json = JsonUtility.ToJson(action);
        yield return SendPostRequest("/make_choice", json, onSuccess, onError);
    }

    // 3. AI 리포트 요청 (별도 처리)
    public IEnumerator RequestReport(List<ChatMessage> history, string endingType, Action<string> onSuccess)
    {
        string url = BASE_URL + "/generate_report";

        // 리스트 직렬화를 위한 래퍼 클래스 사용
        ReportRequest req = new ReportRequest { history = history, ending_type = endingType };
        string finalJson = JsonUtility.ToJson(req);

        // POST 요청 생성
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(finalJson);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // 전송
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // 성공 시 리포트 텍스트만 추출해서 전달
            var res = JsonUtility.FromJson<ReportResponse>(request.downloadHandler.text);
            onSuccess?.Invoke(res.report_text);
        }
        else
        {
            Debug.LogError($"리포트 생성 실패: {request.error}");
            // 실패해도 게임이 멈추지 않게 기본 문구 전달
            onSuccess?.Invoke("데이터 분석 중 문제가 발생했습니다. (연결 오류)");
        }
    }

    // 공통 POST 요청 함수 (게임 진행용)
    public IEnumerator SendPostRequest(string endpoint, string jsonData, Action<GameResponse> onSuccess, Action<string> onError)
    {
        string url = BASE_URL + endpoint;

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error: {request.error} / Response: {request.downloadHandler.text}");
            onError?.Invoke(request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            try
            {
                GameResponse response = JsonUtility.FromJson<GameResponse>(responseText);
                onSuccess?.Invoke(response);
            }
            catch (Exception e)
            {
                Debug.LogError($"JSON 파싱 에러: {e.Message}");
                onError?.Invoke("JSON Parsing Error");
            }
        }
    }
}

// -----------------------------------------------------------
// DTO 클래스 정의 (JsonUtility용)
// -----------------------------------------------------------

[System.Serializable]
public class ReportRequest
{
    public List<ChatMessage> history;
    public string ending_type;
}

[System.Serializable]
public class ReportResponse
{
    public string report_text;
}