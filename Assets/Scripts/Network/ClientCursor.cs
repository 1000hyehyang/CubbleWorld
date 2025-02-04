using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClientCursor : NetworkBehaviour
{
    // 기능
    // 자신의 CursorUpdator 위치로 이동,
    // NetworkTransform이 그 위치를 모두에게 전송할 수 있도록
    // 자신의 색, 닉네임 등 수정 역할
    // 1. CursorUpdator 클래스의 위치로 자신을 이동
    // 2. 처음 스폰 시, CursorUpdator의 형제가 됨 (Room을 부모로 설정)
    // 3. 프리팹 자식 중 TextMeshPro, SpriteRenderer의 레퍼런스 가짐

    // 주의
    // 생성된 PlayerCursor -> CursorUpdator의 형제, 오브젝트 Room의 자식이 되어야 함
    // NetWorkTransform - localPosition, localRotation 직렬화 후 전송
    // so PlayerCursor가 Room의 자식 되면 Room 안에서의 상대적 위치 기반 직렬화 됨 => Room 회전되어도 제대로 대응됨

    [SerializeField] private TextMeshPro usernameLabel;// 닉네임
    [SerializeField] private SpriteRenderer cursorIcon;// 커서

    private CursorUpdater controller;// CursorUpdator 클래스 위치
    //[SerializeField] private GameObject cursorRoot; // 부모가 될 룸 가져오기 .. 이렇게 하면.. 안되는데 하하..
    private GameObject cursorRoot;

    private bool initialized = false;
    private bool setInitialPlayerValues = false;

    // calls the ColorChanged function each time a change is detected on the property during each render frame
    [Networked, OnChangedRender(nameof(OnUsernameChanged))]
    public string Username { get; set; }
    [Networked, OnChangedRender(nameof(OnUserColorChanged))]
    public Color UserColor { get; set; }


    ////private void Awake()
    //private void Start()
    //{
    //    // 필요한거 가져오기
    //    if (HasStateAuthority)
    //    {
    //        controller = GameObject.FindAnyObjectByType<CursorUpdater>();
    //        //cursorRoot = GameObject.Find("Room");
    //    }

    //}

    // 2. 처음 스폰 시, CursorUpdator의 형제가 됨 (Room을 부모로 설정)
    public override void Spawned()
    {
        //base.Spawned();

        // 부모인 Room 찾기
        //GameObject room = GameObject.Find("Room");

        //(뻘짓)GameObject initialCursor = Instantiate(this, cursorUpdater.transform.position, cursorUpdater.transform.rotation, cursorRoot);
        //initialCursor.transform.parent = cursorRoot.transform;

        //transform.SetParent(cursorRoot.transform); // 어라..? 아래에 안 생기네.. 오류도 추가됨,, start에서 미리 하면 안되나?
        // 안됨 ^_^,, 오버라이드 했잖어...
        transform.SetParent(GameObject.Find("Room").transform);
        TMP_InputField usernameField = GameObject.Find("UsernameField").GetComponent<TMP_InputField>();

        if (HasStateAuthority)
        {
            //설정해주어야 함 -> 초깃값이 됨
            //Username = "initialvalue";
           
            //Username = "Username";
            //UserColor = GetRandomColor();

            //usernameField.text = "Username";
            //Username = usernameField.text;
            //usernameField.onEndEdit.AddListener(SetUsername);

            //UserColor = GetRandomColor();

            SetUsername("Username");
            SetColor(GetRandomColor());

            controller = GameObject.FindAnyObjectByType<CursorUpdater>(); // 얘도 그냥 여기로 유배시키기,,
            // (???) 아..!
            
            controller.client = this; // (?!! 추가할 생각을 못했네,, ;n;..)
            initialized = true;
            setInitialPlayerValues = true;

        }
        else
        {
            //이미 소환될 때무터 잘 설정되어있으니 반영만 해주면됨
            //usernameLabel.text = Username;
            //usernameLabel.color = UserColor;

            OnUsernameChanged();
            OnUserColorChanged();
        }
        // Spawned()가.. 외부 클래스에서 호출되는데 
    }

    private void Update()
    {
        if (!HasStateAuthority) // HasStateAuthority -> 현재 플레이어(Network Runner)에게 권한 있으면 true
            return;

        //TODO 딱 한 번 닉네임과 색 설정 <- Spawned()에 있어도 됩니다
        if (setInitialPlayerValues)
        {
            TMP_InputField usernameField = GameObject.Find("UsernameField").GetComponent<TMP_InputField>();
            Username = usernameField.text;
            //usernameField.onEndEdit.AddListener(SetUsername);
            usernameField.onEndEdit.AddListener(ChangeUsername); // 위에처럼 해도 됐는데 왜.. 갑자기 안되는거지......(눈물)

            UserColor = GetRandomColor();
            setInitialPlayerValues = false;
        }

        //TODO C 누를 때 색 변경
        if (Input.GetKeyDown(KeyCode.C))
        {
            UserColor = GetRandomColor();
        }
    }
    public override void FixedUpdateNetwork()
    {
        //Only move own player and not every other player. Each player controls its own player object.
        if (HasStateAuthority == false || !initialized)
        {
            return;
        }
        //TODO CursorUpdater 위치로 이동

        // 1. CursorUpdator 클래스의 위치로 자신을 이동
        transform.position = controller.transform.position;
    }

    // name, color 설정
    public void SetColor(Color color)
    {
        usernameLabel.color = color;
        cursorIcon.color = color;
    }

    public void SetUsername(string text)
    {
        usernameLabel.text = text;
    }

    //OnChangedRender methods - 프레임당 얘네 호출해서 name, color set해주게 함!!
    private void OnUsernameChanged()
    {
        //TODO ?
        SetUsername(Username);
    }

    private void OnUserColorChanged()
    {
        //TODO ?
        SetColor(UserColor);
    }

    private void ChangeUsername(string username)
    {
        Username = username;
    }

    //난 파스텔톤이 좋아
    private Color GetRandomColor()
    {
        return new Color(Random.Range(0.3f, 1f), Random.Range(0.3f, 1f), Random.Range(0.3f, 1f), 1f);
    }

    /// <summary>
    /// 
    /// </summary>

    public void PlaceCube(int id, Vector3Int position)
    {
        NetworkedCubeSpawner.main.RequestPlaceCube(id, position, Runner);
    }

    public void RemoveCube(NetworkedCube cube)
    {
        if (cube.HasStateAuthority)
        {
            cube.RemoveRpc();
        }
        else
        {
            cube.networkObject.RequestStateAuthority();
            StartCoroutine(IWaitForAuthority(cube));
        }
    }

    IEnumerator IWaitForAuthority(NetworkedCube cube)
    {
        yield return new WaitUntil(() => cube.gameObject == null || cube.HasStateAuthority);
        if (cube.gameObject != null) cube.RemoveRpc();
    }
}
