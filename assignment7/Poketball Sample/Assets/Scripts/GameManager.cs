using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    UIManager MyUIManager;

    public GameObject BallPrefab;   // prefab of Ball

    // Constants for SetupBalls
    public static Vector3 StartPosition = new Vector3(0, 0.75f, -6.35f);
    public static Quaternion StartRotation = Quaternion.Euler(0, 90, 90);
    const float BallRadius = 0.286f;
    const float RowSpacing = 0.02f;

    GameObject PlayerBall;
    GameObject CamObj;

    const float CamSpeed = 3f;

    const float MinPower = 15f;
    const float PowerCoef = 1f;

    // 카메라가 공을 따라갈 때 유지할 거리(오프셋)를 저장할 변수 추가
    private Vector3 camOffset;

    void Awake()
    {
        // PlayerBall, CamObj, MyUIManager를 얻어온다.
        // ---------- TODO ---------- 
        PlayerBall = GameObject.Find("PlayerBall");
        CamObj = Camera.main.gameObject;
        
        // 캔버스 오브젝트를 찾고 그 안의 UIManager 컴포넌트를 가져옵니다.
        GameObject canvasObj = GameObject.Find("Canvas");
        if(canvasObj != null)
        {
            MyUIManager = canvasObj.GetComponent<UIManager>();
        }

        // 카메라와 공 사이의 초기 거리를 저장해둡니다 (CamMove에서 사용)
        if (PlayerBall != null && CamObj != null)
        {
            camOffset = CamObj.transform.position - PlayerBall.transform.position;
        }
        // -------------------- 
    }

    void Start()
    {
        SetupBalls();
    }

    // Update is called once per frame
    void Update()
    {
        // 좌클릭시 raycast하여 클릭 위치로 ShootBallTo 한다.
        // ---------- TODO ---------- 
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 클릭한 지점(hit.point)으로 공을 발사
                ShootBallTo(hit.point);
            }
        }
        // -------------------- 
    }

    void LateUpdate()
    {
        CamMove();
    }

    void SetupBalls()
    {
        // 15개의 공을 삼각형 형태로 배치한다.
        // ---------- TODO ---------- 
        int ballIndex = 1;
        float diameter = BallRadius * 2;
        
        // 5줄로 배치 (1개, 2개, 3개, 4개, 5개 = 총 15개)
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col <= row; col++)
            {
                // X좌표: 줄마다 가운데 정렬을 위해 계산
                // (col - row * 0.5f)를 하면 삼각형 모양으로 퍼집니다.
                float xPos = (col - (row * 0.5f)) * (diameter + RowSpacing);
                
                // Z좌표: 뒤로 갈수록 Z값이 줄어들거나 늘어남 (여기서는 뒤로 배치)
                // 삼각형 높이(sqrt(3)/2)를 고려하여 빽빽하게 배치하거나 단순하게 배치
                // 여기서는 간단히 행(row)만큼 뒤로 이동한다고 가정
                float zPos = StartPosition.z - (row * (diameter + RowSpacing)); 
                
                // 위치 벡터 생성 (Y는 StartPosition의 Y 사용)
                Vector3 spawnPos = new Vector3(xPos, StartPosition.y, zPos);

                // 공 생성
                GameObject newBall = Instantiate(BallPrefab, spawnPos, StartRotation);
                
                // 이름 변경 ({index})
                newBall.name = ballIndex.ToString();

                // Material 적용 (Resources 폴더 안의 재질 로드)
                // 재질 이름이 "ball_1", "ball_2" ... 형식이라고 가정
                MeshRenderer renderer = newBall.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material = Resources.Load<Material>("Materials/ball_" + ballIndex);
                }

                ballIndex++;
            }
        }
        // -------------------- 
    }

    void CamMove()
    {
        // CamObj는 PlayerBall을 CamSpeed의 속도로 따라간다.
        // ---------- TODO ---------- 
        if (PlayerBall != null && CamObj != null)
        {
            // 목표 위치 = 현재 공 위치 + 초기 오프셋
            Vector3 targetPos = PlayerBall.transform.position + camOffset;
            
            // Lerp를 사용하여 부드럽게 이동
            CamObj.transform.position = Vector3.Lerp(CamObj.transform.position, targetPos, CamSpeed * Time.deltaTime);
        }
        // -------------------- 
    }

    float CalcPower(Vector3 displacement)
    {
        return MinPower + displacement.magnitude * PowerCoef;
    }

    void ShootBallTo(Vector3 targetPos)
    {
        // targetPos의 위치로 공을 발사한다.
        // ---------- TODO ---------- 
        if (PlayerBall != null)
        {
            Rigidbody rb = PlayerBall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 방향 계산 (목표지점 - 내 위치)
                Vector3 direction = targetPos - PlayerBall.transform.position;
                direction.y = 0; // 공이 뜨지 않게 Y축 힘 제거

                // 거리(변위) 계산을 위해 임시 저장
                Vector3 displacement = direction;

                // 방향 정규화 (크기를 1로 만듦)
                direction.Normalize();

                // 힘 계산
                float power = CalcPower(displacement);

                // 발사 (Impulse 모드)
                rb.AddForce(direction * power, ForceMode.Impulse);
            }
        }
        // -------------------- 
    }
    
    // When ball falls
    public void Fall(string ballName)
    {
        // "{ballName} falls"을 1초간 띄운다.
        // ---------- TODO ---------- 
        if (MyUIManager != null)
        {
            MyUIManager.DisplayText(ballName + " falls", 1.0f);
        }
        // -------------------- 
    }
}