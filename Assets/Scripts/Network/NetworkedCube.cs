using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedCube : NetworkBehaviour
{
    // int Id, Vector3Int Position�� networked property�� ���� -> cube id & cube position
    // spawned�� �� �ش� property�� �ڽſ��� �ݿ��ϸ�
    // �ڽ��� �ڽ����� ť�� ���� ������ ����
    public NetworkObject networkObject;

    [Networked]
    public int CubeID { get; set; }
    [Networked]
    public Vector3Int Position { get; set; }

    public override void Spawned()
    {
        //base.Spawned();

        // �ڽ��� �ڽ����� ť�� ���� ������ ����..
        //GameObject putObj = Instantiate(NetworkedCubeSpawner.main.cubeList.cubes[CubeID],
        //    transform.position, transform.rotation, NetworkedCubeSpawner.main.cubesRoot);
        //    GameObject putObj = Instantiate(NetworkedCubeSpawner.main.cubeList.cubes[CubeID],
        //transform, NetworkedCubeSpawner.main.cubesRoot);

        transform.SetParent(NetworkedCubeSpawner.main.cubesRoot);
        GameObject putObj = Instantiate(NetworkedCubeSpawner.main.cubeList.cubes[CubeID], transform); // ť�� ������Ʈ ������ �� �ְ�

        putObj.AddComponent<CubeData>().id = CubeID; // ���� ������Ʈ�� �������� ��ũ��Ʈ ���̱� -> �̰� �� ť���� id �����Ϸ���
        // ������ �ƴ� �� ������Ʈ�� �θ� ��ǥ�� �������� �����ǵ��� �����ǰ� �����̼ǰ� ����
        transform.localPosition = Position;
        transform.localRotation = Quaternion.identity;
    }

    // ť�� id, position Set, Get
    public void Set(int id, Vector3Int position)
    {
        if (!HasStateAuthority) // HasStateAuthority -> ���� �÷��̾�(Network Runner)���� ���� ������ true
            return;

        CubeID = id;
        Position = position;
    }

    // RPC�� ť�� �����ϱ�
    // �̰� CursorUpdator���� ��Ŭ�� ���ʿ� �����ϸ�
    // �Բ� �÷����ϴ� �÷��̾� ������ �� �� �÷��̾ ��ġ�ߴ� ��� ���� �Ұ�
    // why? 
    // +
    // ���� �÷��̾��� ��ϵ��� �������� ��������� ��
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)] // ��� Ŭ���̾�Ʈ����
    public void RemoveRpc()
    {
        Runner.Despawn(networkObject);
    }
}
