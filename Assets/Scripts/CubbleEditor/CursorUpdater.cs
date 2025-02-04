using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorUpdater : MonoBehaviour {
    private const int MIN_POS = -2, MAX_POS = 2;

    [Header("Settings")]
    [SerializeField] private CubeList cubeList;
    [SerializeField] private LayerMask backgroundLayer, cubeLayer;
    [SerializeField] private Color validColor = Color.yellow, invalidColor = Color.red;

    [Header("Component References")]
    [SerializeField] private Camera cam;
    [SerializeField] private Transform cubesRoot;
    [SerializeField] private MeshRenderer mrenderer;

    public bool hasTarget = false, validPosition = false;

    private RaycastHit[] hits = new RaycastHit[15]; //we can expect at most 15 items in a straight line
    private Collider[] res = new Collider[1];

    public ClientCursor client;

    private void UpdateInput() {
        //place cube
        //todo: Place or remove cube at position
        // 커서를 사용해서 현재 선택된 큐브 프리팹 GameObject 설치 및 삭제
        // 좌클릭: 커서 위치에 설치
        // 우클릭: 커서 위치의 블록 삭제
        // (주의) 생성된 큐브 - 오브젝트 Room/Cubes의 자식이 되어야 함

        // hint: localPosition & localRotation을 Insatntiate parameter로 넘겨줘야 함
        // Instantiate 된 오브젝트의 부모 정해주기
        // Gameobject Instantiate(Gameobject prefab, Vector3 position, Quaternion rotation, Transform parent)

        bool overUI = EventSystem.current.IsPointerOverGameObject();
        if (validPosition && Input.GetMouseButtonDown(0) && !overUI) { // 좌클릭
                                                                       //todo: place selected block at position

            ///////////////////////////////////////////////////////
            //////////////// TODO: WRITE CODE HERE ////////////////
            ///////////////////////////////////////////////////////

            // NetworkedCube의 Spawn 함수에서 큐브 생성하도록 해서 주석처리 함
            //// 선택된 큐브 가져오기
            //int currentCubeID = CubePalette.Main.currentCubeID;
            //// 설치 - 선택된 큐브를
            //GameObject put = Instantiate(cubeList.cubes[currentCubeID], transform.position, transform.rotation, cubesRoot); // (어라) 근데 루트가 이거 자체여도 되나?
            //                                                                                                                // 업데이터가 움직일때마다 아닌가 쌓이는거 아닌가? 이게 맞나..? 근데 이거 말곤 둘데가 없는데 내가 뭐 따로 만들어줘야 되나
            //put.AddComponent<CubeData>().id = currentCubeID; // 게임 오브젝트에 동적으로 스크립트 붙이기 -> 이게 뭔 큐븐지 id 저장하려고

            if (client != null) 
                client.PlaceCube(CubePalette.Main.currentCubeID,
                    new Vector3Int(Mathf.RoundToInt(transform.localPosition.x), 
                    Mathf.RoundToInt(transform.localPosition.y), Mathf.RoundToInt(transform.localPosition.z)));
        }
        else if (Input.GetMouseButtonDown(1) && !overUI) { // 우클릭
                                                           //todo: remove cube via raycast
                                                           //use GetMinDistanceHit function

            ///////////////////////////////////////////////////////
            //////////////// TODO: WRITE CODE HERE ////////////////
            ///////////////////////////////////////////////////////

            // 생각해보니까 냅다 없애면 안되네,,
            // Cursor Updater의 자식으로 들어간 큐브 중 선택된 애를 destory 해야 됨

            // 큐브 선택 & 큐브 정보 가져오기
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            int n = Physics.RaycastNonAlloc(ray, hits, 50, cubeLayer); // Cast a ray through the Scene and store the hits into the buffer
                                                                       // 레이, buffer to store the hits into, maxDistance, layerMask
                                                                       // 리턴값: amount of hits stored into the results buffer
            //if (n == 0)
            //{
            //    n = Physics.RaycastNonAlloc(ray, hits, 50, backgroundLayer);
            //}
            //hasTarget = n > 0;
            RaycastHit target = GetMinDistanceHit(n);
            if (n > 0 && target.transform.CompareTag("CubbleObject"))
            {
                // 
                //Destroy(target.transform.gameObject);
                //// (허걱) 이렇게만 두면 본인도 없애버리네

                if (client != null && target.transform.parent.TryGetComponent<NetworkedCube>(out NetworkedCube cube))
                {
                    client.RemoveCube(cube);
                }
            }
        }
    }

    #region SKELETON
    //////////////// SKELETON CODE - do not touch ////////////////

    void Start() {
        hasTarget = validPosition = false;
        mrenderer.enabled = false;
    }

    void Update() {
        UpdatePosition();

        mrenderer.enabled = hasTarget;
        if (hasTarget) {
            //check availability
            int n = Physics.OverlapBoxNonAlloc(transform.position, Vector3.one * 0.2f, res, Quaternion.identity, cubeLayer);
            validPosition = n < 1;

            //color cursor
            mrenderer.material.color = validPosition ? validColor : invalidColor;
        }
        else {
            validPosition = false;
        }

        UpdateInput();
    }

    private void UpdatePosition() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        int n = Physics.RaycastNonAlloc(ray, hits, 50, cubeLayer);


        if (n == 0) {
            n = Physics.RaycastNonAlloc(ray, hits, 50, backgroundLayer);
        }

        hasTarget = n > 0;

        if (hasTarget) {
            RaycastHit target = GetMinDistanceHit(n);
            Vector3 pos = target.point + 0.5f * target.normal;
            transform.position = pos;

            if (!InBounds(transform.localPosition)) {
                hasTarget = false;
            }
            else {
                transform.localPosition = new Vector3(ClampPos(transform.localPosition.x), ClampPos(transform.localPosition.y), ClampPos(transform.localPosition.z));
            }
        }
    }

    private RaycastHit GetMinDistanceHit(int n) {
        RaycastHit target = hits[0];
        float dist = (hits[0].transform.position - cam.transform.position).sqrMagnitude;

        for (int i = 1; i < n; i++) {
            float d = (hits[i].transform.position - cam.transform.position).sqrMagnitude;
            if (d < dist) {
                dist = d;
                target = hits[i];
            }
        }

        return target;
    }

    private bool InBounds(Vector3 p) {
        int x = Mathf.RoundToInt(p.x);
        if (x < MIN_POS || x > MAX_POS) return false;
        x = Mathf.RoundToInt(p.y);
        if (x < MIN_POS || x > MAX_POS) return false;
        x = Mathf.RoundToInt(p.z);
        if (x < MIN_POS || x > MAX_POS) return false;
        return true;
    }

    private int ClampPos(float p) {
        return Mathf.Clamp(Mathf.RoundToInt(p), MIN_POS, MAX_POS);
    }

    //////////////// SKELETON CODE - do not touch ////////////////
    #endregion
}
