using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkedCubeSpawner : MonoBehaviour
{
    // "해당 플레이어"가 선택한 "id" 및 "위치" 넘겨줌

    public static NetworkedCubeSpawner main;

    public CubeList cubeList;
    public Transform cubesRoot;
    [SerializeField] private GameObject networkedCubePrefab;

    private void Awake()
    {
        main = this; // 싱글톤이라!!
    }

    public void RequestPlaceCube(int id, Vector3Int position, NetworkRunner runner)
    {
        // NetworkRunner.OnBeforeSpawned delegate 파라미터로 람다 넘겨서 id, position networked property 초기화
        runner.Spawn(networkedCubePrefab, position, Quaternion.identity, runner.LocalPlayer, (Runner, NO) =>  //MyOnBeforeSpawnDelegate
        NO.GetComponent<NetworkedCube>().Set(id, position));

        // OnBeforeSpawned 함수 형태
        // public delegate void OnBeforeSpawned(NetworkRunner runner, NetworkObject obj);

        // 그극근데 저 NO는 뭐지??? 왜 NO라고 쓴거야? 뭔진 알겠는데 왜 하필 이름이 No.?
        // ㅇㅅㅇ,, Network Object라서 No.....엿..다..... 하하하... 괜한 곳에 시간을(절망)

}
}
