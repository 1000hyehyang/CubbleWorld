using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSpawner : SimulationBehaviour, IPlayerJoined //�÷��̾� ������ ȣ��
{
    public GameObject playerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        }
    }
    // ���� Instantiate~~ �� �ʿ� ���� �̷��� �ۼ��ϸ�
    // ���ǿ� �÷��̾� ���� �� ȣ���
}
