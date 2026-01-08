using UnityEngine;
using Cainos.CustomizablePixelCharacter;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint_Left;
    [SerializeField] private Transform spawnPoint_Right;
    [SerializeField] private Transform spawnPoint_Workshop;

    void Start()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("プレイヤーが見つかりません");
            return;
        }

        PixelCharacter character = player.GetComponent<PixelCharacter>();

        Transform spawnPos = null;

        PixelCharacter.FacingType targetFacing = PixelCharacter.FacingType.Right;

        // スポーン位置を決める
        switch (GameData.lastExitDirection)
        {
            case "Left":
                spawnPos = spawnPoint_Right;
                targetFacing = PixelCharacter.FacingType.Left;
                break;

            case "Right":
                spawnPos = spawnPoint_Left;
                targetFacing = PixelCharacter.FacingType.Right;
                break;
            case "Workshop":
                spawnPos = spawnPoint_Workshop;
                targetFacing = PixelCharacter.FacingType.Left;
                break;

            default:
                spawnPos = spawnPoint_Left;
                targetFacing = PixelCharacter.FacingType.Right;
                break;
        }

        if (spawnPos != null)
        {
            // プレイヤーの位置を変更
            player.transform.position = spawnPos.position;

            if (character != null)
            {
                character.Facing = targetFacing;
            }
        }
    }
}
