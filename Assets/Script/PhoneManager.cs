using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhoneManager : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject chatWindow;      //채팅창
    public RectTransform phoneButton;  //스마트폰 버튼

    [Header("Animation Settings")]
    public float slideSpeed = 2.0f;    // 올라오는 속도
    public float setYposition = 600;
    public Vector2 targetPosition;     // 스마트폰이 도착할 최종 위치
    public Vector2 startPosition;      // 스마트폰이 숨어있을 시작 위치

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip vibrationClip;

    void Start()
    {
        chatWindow.SetActive(false);

        targetPosition = phoneButton.anchoredPosition;
        startPosition = new Vector2(targetPosition.x, targetPosition.y - setYposition);
        phoneButton.anchoredPosition = startPosition;

        StartCoroutine(SlidePhoneUp());
    }

    IEnumerator SlidePhoneUp()
    {
        yield return new WaitForSeconds(1.0f);

        if (audioSource && vibrationClip)
            audioSource.PlayOneShot(vibrationClip);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * slideSpeed;
            phoneButton.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
    }

    public void OnPhoneClick()
    {
        chatWindow.SetActive(true);
    }
}