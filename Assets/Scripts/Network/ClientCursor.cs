using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClientCursor : NetworkBehaviour
{
    // ���
    // �ڽ��� CursorUpdator ��ġ�� �̵�,
    // NetworkTransform�� �� ��ġ�� ��ο��� ������ �� �ֵ���
    // �ڽ��� ��, �г��� �� ���� ����
    // 1. CursorUpdator Ŭ������ ��ġ�� �ڽ��� �̵�
    // 2. ó�� ���� ��, CursorUpdator�� ������ �� (Room�� �θ�� ����)
    // 3. ������ �ڽ� �� TextMeshPro, SpriteRenderer�� ���۷��� ����

    // ����
    // ������ PlayerCursor -> CursorUpdator�� ����, ������Ʈ Room�� �ڽ��� �Ǿ�� ��
    // NetWorkTransform - localPosition, localRotation ����ȭ �� ����
    // so PlayerCursor�� Room�� �ڽ� �Ǹ� Room �ȿ����� ����� ��ġ ��� ����ȭ �� => Room ȸ���Ǿ ����� ������

    [SerializeField] private TextMeshPro usernameLabel;// �г���
    [SerializeField] private SpriteRenderer cursorIcon;// Ŀ��

    private CursorUpdater controller;// CursorUpdator Ŭ���� ��ġ
    //[SerializeField] private GameObject cursorRoot; // �θ� �� �� �������� .. �̷��� �ϸ�.. �ȵǴµ� ����..
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
    //    // �ʿ��Ѱ� ��������
    //    if (HasStateAuthority)
    //    {
    //        controller = GameObject.FindAnyObjectByType<CursorUpdater>();
    //        //cursorRoot = GameObject.Find("Room");
    //    }

    //}

    // 2. ó�� ���� ��, CursorUpdator�� ������ �� (Room�� �θ�� ����)
    public override void Spawned()
    {
        //base.Spawned();

        // �θ��� Room ã��
        //GameObject room = GameObject.Find("Room");

        //(����)GameObject initialCursor = Instantiate(this, cursorUpdater.transform.position, cursorUpdater.transform.rotation, cursorRoot);
        //initialCursor.transform.parent = cursorRoot.transform;

        //transform.SetParent(cursorRoot.transform); // ���..? �Ʒ��� �� �����.. ������ �߰���,, start���� �̸� �ϸ� �ȵǳ�?
        // �ȵ� ^_^,, �������̵� ���ݾ�...
        transform.SetParent(GameObject.Find("Room").transform);
        TMP_InputField usernameField = GameObject.Find("UsernameField").GetComponent<TMP_InputField>();

        if (HasStateAuthority)
        {
            //�������־�� �� -> �ʱ갪�� ��
            //Username = "initialvalue";
           
            //Username = "Username";
            //UserColor = GetRandomColor();

            //usernameField.text = "Username";
            //Username = usernameField.text;
            //usernameField.onEndEdit.AddListener(SetUsername);

            //UserColor = GetRandomColor();

            SetUsername("Username");
            SetColor(GetRandomColor());

            controller = GameObject.FindAnyObjectByType<CursorUpdater>(); // �굵 �׳� ����� �����Ű��,,
            // (???) ��..!
            
            controller.client = this; // (?!! �߰��� ������ ���߳�,, ;n;..)
            initialized = true;
            setInitialPlayerValues = true;

        }
        else
        {
            //�̹� ��ȯ�� ������ �� �����Ǿ������� �ݿ��� ���ָ��
            //usernameLabel.text = Username;
            //usernameLabel.color = UserColor;

            OnUsernameChanged();
            OnUserColorChanged();
        }
        // Spawned()��.. �ܺ� Ŭ�������� ȣ��Ǵµ� 
    }

    private void Update()
    {
        if (!HasStateAuthority) // HasStateAuthority -> ���� �÷��̾�(Network Runner)���� ���� ������ true
            return;

        //TODO �� �� �� �г��Ӱ� �� ���� <- Spawned()�� �־ �˴ϴ�
        if (setInitialPlayerValues)
        {
            TMP_InputField usernameField = GameObject.Find("UsernameField").GetComponent<TMP_InputField>();
            Username = usernameField.text;
            //usernameField.onEndEdit.AddListener(SetUsername);
            usernameField.onEndEdit.AddListener(ChangeUsername); // ����ó�� �ص� �ƴµ� ��.. ���ڱ� �ȵǴ°���......(����)

            UserColor = GetRandomColor();
            setInitialPlayerValues = false;
        }

        //TODO C ���� �� �� ����
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
        //TODO CursorUpdater ��ġ�� �̵�

        // 1. CursorUpdator Ŭ������ ��ġ�� �ڽ��� �̵�
        transform.position = controller.transform.position;
    }

    // name, color ����
    public void SetColor(Color color)
    {
        usernameLabel.color = color;
        cursorIcon.color = color;
    }

    public void SetUsername(string text)
    {
        usernameLabel.text = text;
    }

    //OnChangedRender methods - �����Ӵ� ��� ȣ���ؼ� name, color set���ְ� ��!!
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

    //�� �Ľ������� ����
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
