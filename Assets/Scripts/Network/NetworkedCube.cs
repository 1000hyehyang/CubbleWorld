using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedCube : NetworkBehaviour
{
    // int Id, Vector3Int Position을 networked property로 가짐 -> cube id & cube position
    // spawned될 때 해당 property를 자신에게 반영하며
    // 자신의 자식으로 큐브 외형 프리팹 생성
    public NetworkObject networkObject;

    [Networked]
    public int CubeID { get; set; }
    [Networked]
    public Vector3Int Position { get; set; }

    public override void Spawned()
    {
        //base.Spawned();

        // 자신의 자식으로 큐브 외형 프리팹 생성..
        //GameObject putObj = Instantiate(NetworkedCubeSpawner.main.cubeList.cubes[CubeID],
        //    transform.position, transform.rotation, NetworkedCubeSpawner.main.cubesRoot);
        //    GameObject putObj = Instantiate(NetworkedCubeSpawner.main.cubeList.cubes[CubeID],
        //transform, NetworkedCubeSpawner.main.cubesRoot);

        transform.SetParent(NetworkedCubeSpawner.main.cubesRoot);
        GameObject putObj = Instantiate(NetworkedCubeSpawner.main.cubeList.cubes[CubeID], transform); // 큐브 오브젝트 생성할 수 있게

        putObj.AddComponent<CubeData>().id = CubeID; // 게임 오브젝트에 동적으로 스크립트 붙이기 -> 이게 뭔 큐븐지 id 저장하려고
        // 전역이 아닌 요 오브젝트의 부모 좌표계 기준으로 설정되도록 포지션과 로테이션값 설정
        transform.localPosition = Position;
        transform.localRotation = Quaternion.identity;
    }

    // 큐브 id, position Set, Get
    public void Set(int id, Vector3Int position)
    {
        if (!HasStateAuthority) // HasStateAuthority -> 현재 플레이어(Network Runner)에게 권한 있으면 true
            return;

        CubeID = id;
        Position = position;
    }

    // RPC로 큐브 삭제하기
    // 이거 CursorUpdator에서 우클릭 그쪽에 구현하면
    // 함께 플레이하던 플레이어 나갔을 때 그 플레이어가 설치했던 블록 삭제 불가
    // why? 
    // +
    // 나간 플레이어의 블록들을 내쪽으로 가져와줘야 됨
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)] // 모든 클라이언트에게
    public void RemoveRpc()
    {
        Runner.Despawn(networkObject);
    }
}
