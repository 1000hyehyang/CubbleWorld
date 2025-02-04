using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSpawner : SimulationBehaviour, IPlayerJoined //플레이어 참여시 호출
{
    public GameObject playerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        }
    }
    // 굳이 Instantiate~~ 할 필요 없이 이렇게 작성하면
    // 세션에 플레이어 참여 시 호출됨
}
