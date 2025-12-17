using Mirror;
using TMPro;
using UnityEngine;

public class GameUIManager : NetworkBehaviour
{
    public TMP_Text winnerText;

    public void ShowWinner(string playerName)
    {
        winnerText.text = $"{playerName} wins!";
        winnerText.gameObject.SetActive(true);
    }

    public void HideWinner()
    {
        winnerText.gameObject.SetActive(false);
    }
    public TMP_Text roundTimerText;

    [ClientRpc]
    public void RpcUpdateTimer(float time)
    {
        roundTimerText.text = $"Next round in {Mathf.CeilToInt(time)}s";
    }

}