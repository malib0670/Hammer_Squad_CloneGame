using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // JUST UI SETTINGS

    public TextMeshProUGUI moneyScoreText;
    public TextMeshPro scoreText;
    public GameObject scoreSpawnPoint, twoWorkerButton, youWinText, restartButton;

    public static int totalScoreCount;
    public static int scoreCount;

    // Start is called before the first frame update
    void Start()
    {
        totalScoreCount = 0; 
        scoreCount = 5; 

        scoreText.text = "$ " + scoreCount.ToString(); 
        moneyScoreText.text = "TOTAL MONEY: " + totalScoreCount.ToString() + " $"; 
    }

    public void SpawnScoreText() // -> Score Text'inin artýþý ayarlandý
    {
        scoreCount += 5; 
        Instantiate(scoreText, scoreSpawnPoint.transform.position, Quaternion.identity);
        scoreText.text = "$ " + scoreCount.ToString(); 
    }

    public void IncraseTotalMoneyCount() // -> Para artýþý ayarlandý
    {
        totalScoreCount += 5; 
        moneyScoreText.text = "TOTAL MONEY: " + totalScoreCount.ToString() + " $"; 
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
