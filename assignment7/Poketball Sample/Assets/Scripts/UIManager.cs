using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Text MyText;

    Coroutine NowCoroutine;

    void Awake() {
        // MyText를 얻어오고, 내용을 지운다.
        // ---------- TODO ---------- 
        // Canvas(이 스크립트가 붙은 곳)의 자식 중에서 "MyText"라는 이름을 가진 오브젝트를 찾습니다.
        MyText = transform.Find("MyText").GetComponent<Text>();
        
        // 시작할 때는 아무 글자도 안 보이게 초기화
        MyText.text = "";
        // -------------------- 
    }

    public void DisplayText(string text, float duration)
    {
        // NowCoroutine이 있다면 멈추고 새로운 DisplayTextCoroutine을 시작한다.
        // ---------- TODO ---------- 
        // 만약 이미 글자가 떠서 사라지기를 기다리는 중이라면(타이머 작동 중),
        // 그 타이머를 강제로 끕니다. (새로운 메시지를 바로 보여주기 위함)
        if (NowCoroutine != null)
        {
            StopCoroutine(NowCoroutine);
        }

        // 새로운 코루틴(타이머)을 시작하고 변수에 저장해둡니다.
        NowCoroutine = StartCoroutine(DisplayTextCoroutine(text, duration));
        // -------------------- 
    }

    IEnumerator DisplayTextCoroutine(string text, float duration)
    {
        // MyText에 text를 duration초 동안 띄운다.
        // ---------- TODO ---------- 
        // 1. 화면에 글자를 보여줍니다.
        MyText.text = text;

        // 2. duration(초) 만큼 대기합니다.
        yield return new WaitForSeconds(duration);

        // 3. 시간이 지나면 글자를 다시 지웁니다.
        MyText.text = "";
        
        // 4. 코루틴이 끝났으므로 변수를 비워줍니다.
        NowCoroutine = null;
        // -------------------- 
    }
}