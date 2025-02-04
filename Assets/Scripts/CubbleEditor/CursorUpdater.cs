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
        // Ŀ���� ����ؼ� ���� ���õ� ť�� ������ GameObject ��ġ �� ����
        // ��Ŭ��: Ŀ�� ��ġ�� ��ġ
        // ��Ŭ��: Ŀ�� ��ġ�� ��� ����
        // (����) ������ ť�� - ������Ʈ Room/Cubes�� �ڽ��� �Ǿ�� ��

        // hint: localPosition & localRotation�� Insatntiate parameter�� �Ѱ���� ��
        // Instantiate �� ������Ʈ�� �θ� �����ֱ�
        // Gameobject Instantiate(Gameobject prefab, Vector3 position, Quaternion rotation, Transform parent)

        bool overUI = EventSystem.current.IsPointerOverGameObject();
        if (validPosition && Input.GetMouseButtonDown(0) && !overUI) { // ��Ŭ��
                                                                       //todo: place selected block at position

            ///////////////////////////////////////////////////////
            //////////////// TODO: WRITE CODE HERE ////////////////
            ///////////////////////////////////////////////////////

            // NetworkedCube�� Spawn �Լ����� ť�� �����ϵ��� �ؼ� �ּ�ó�� ��
            //// ���õ� ť�� ��������
            //int currentCubeID = CubePalette.Main.currentCubeID;
            //// ��ġ - ���õ� ť�긦
            //GameObject put = Instantiate(cubeList.cubes[currentCubeID], transform.position, transform.rotation, cubesRoot); // (���) �ٵ� ��Ʈ�� �̰� ��ü���� �ǳ�?
            //                                                                                                                // �������Ͱ� �����϶����� �ƴѰ� ���̴°� �ƴѰ�? �̰� �³�..? �ٵ� �̰� ���� �ѵ��� ���µ� ���� �� ���� �������� �ǳ�
            //put.AddComponent<CubeData>().id = currentCubeID; // ���� ������Ʈ�� �������� ��ũ��Ʈ ���̱� -> �̰� �� ť���� id �����Ϸ���

            if (client != null) 
                client.PlaceCube(CubePalette.Main.currentCubeID,
                    new Vector3Int(Mathf.RoundToInt(transform.localPosition.x), 
                    Mathf.RoundToInt(transform.localPosition.y), Mathf.RoundToInt(transform.localPosition.z)));
        }
        else if (Input.GetMouseButtonDown(1) && !overUI) { // ��Ŭ��
                                                           //todo: remove cube via raycast
                                                           //use GetMinDistanceHit function

            ///////////////////////////////////////////////////////
            //////////////// TODO: WRITE CODE HERE ////////////////
            ///////////////////////////////////////////////////////

            // �����غ��ϱ� ���� ���ָ� �ȵǳ�,,
            // Cursor Updater�� �ڽ����� �� ť�� �� ���õ� �ָ� destory �ؾ� ��

            // ť�� ���� & ť�� ���� ��������
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            int n = Physics.RaycastNonAlloc(ray, hits, 50, cubeLayer); // Cast a ray through the Scene and store the hits into the buffer
                                                                       // ����, buffer to store the hits into, maxDistance, layerMask
                                                                       // ���ϰ�: amount of hits stored into the results buffer
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
                //// (���) �̷��Ը� �θ� ���ε� ���ֹ�����

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
