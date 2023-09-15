using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] players;

    public void CheckAlive()
    {
        int aliveCount = 0;

        foreach (GameObject player in players) // �÷��� ��ȸ �ݺ���
        {
            if(player.activeSelf)
            {
                aliveCount++;
            }
        }

        if (aliveCount <= 1)
        {
            Invoke(nameof(NewRound),3f);
        }
    }

    private void NewRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
