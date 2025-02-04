using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkedCubeSpawner : MonoBehaviour
{
    // "�ش� �÷��̾�"�� ������ "id" �� "��ġ" �Ѱ���

    public static NetworkedCubeSpawner main;

    public CubeList cubeList;
    public Transform cubesRoot;
    [SerializeField] private GameObject networkedCubePrefab;

    private void Awake()
    {
        main = this; // �̱����̶�!!
    }

    public void RequestPlaceCube(int id, Vector3Int position, NetworkRunner runner)
    {
        // NetworkRunner.OnBeforeSpawned delegate �Ķ���ͷ� ���� �Ѱܼ� id, position networked property �ʱ�ȭ
        runner.Spawn(networkedCubePrefab, position, Quaternion.identity, runner.LocalPlayer, (Runner, NO) =>  //MyOnBeforeSpawnDelegate
        NO.GetComponent<NetworkedCube>().Set(id, position));

        // OnBeforeSpawned �Լ� ����
        // public delegate void OnBeforeSpawned(NetworkRunner runner, NetworkObject obj);

        // �ױرٵ� �� NO�� ����??? �� NO��� ���ž�? ���� �˰ڴµ� �� ���� �̸��� No.?
        // ������,, Network Object�� No.....��..��..... ������... ���� ���� �ð���(����)

}
}
