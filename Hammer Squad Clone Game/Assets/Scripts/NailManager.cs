using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class NailManager : MonoBehaviour
{
    GameManager gameManager; // -> Game Manager s�n�f�ndan veri �ekildi

    public Transform groundObjects;

    public ParticleSystem hitParticle, fragmentParticle;
    AudioSource audioSource;
    public AudioClip hitClip;

    public Animator player1Anim, player2Anim;

    public GameObject player2;
    public GameObject tapToStartPanel;

    int hitCount = 0; // -> �iviye vuru� say�s� kontrol edildi
    int groundCount = 1; // -> Zemin listesinde yok olma ayarlamas� yap�ld�

    bool isGameStart = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // -> Oyunun ba�lamas� kontrol edildi 

        if (Input.GetMouseButtonDown(0))
        {           
            isGameStart = true;
            tapToStartPanel.SetActive(false);
            player1Anim.SetBool("isGameStartBool", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGameStart)
        {
            if (other.CompareTag("Hammer"))
            {
                transform.DOMoveY(transform.position.y - 0.1f, 0.1f); // -> �ivinin Y ekseninde ne kadar hareket edece�i ve ne kadar zamanda
                                                                        // hareket edece�i kontrol edildi

                StartCoroutine(NailScaleTime()); // -> �iviye her vuru�ta Scale de�i�imi kontrol edildi

                gameManager.SpawnScoreText(); // -> Game Manager s�n�f�ndan method �a��r�ld�
                gameManager.IncraseTotalMoneyCount(); // -> // -> Game Manager s�n�f�ndan method �a��r�ld�

                hitParticle.Play();
                audioSource.PlayOneShot(hitClip);

                hitCount++;

                if (hitCount == 5) 
                {
                    fragmentParticle.Play();
                    Destroy(ListManager.instance.groundList[ListManager.instance.groundList.Count - groundCount], 0.1f); // -> Her 3 vuru�ta zemin
                                                    // yok olmas� ayarland�
                    hitCount = 0; 
                    groundCount++; 

                    StartCoroutine(UpPositionTime()); // -> Belli bir s�rede zeminin ne kadar yukar� hareket edece�i kontrol edildi

                    transform.DOMoveY(7.6f, 0.5f); // -> 3 vuru� sonras� �ivinin Y ekseninde eski halini almas� ayarland�
                }

                if (GameManager.totalScoreCount == 30) // -> Para scoru 30 oldu�unda 2 i��i �al��t�rma butonu aktif oldu
                {
                    gameManager.twoWorkerButton.SetActive(true);
                    transform.DOMoveY(transform.position.y - 0.2f, 0.1f);
                    hitCount = 3; 
                }

                if (GameManager.totalScoreCount == 100)
                {
                    isGameStart = false;
                    gameManager.youWinText.SetActive(true);
                    gameManager.restartButton.SetActive(true);
                    GameManager.totalScoreCount = 0;
                    GameManager.scoreCount = 5;
                    player1Anim.SetBool("isGameStartBool", false);
                    player2Anim.SetBool("isGameStartBool", false);
                }
            }
        }       
    }

    public void TwoWorkerRunButton() // -> �ki i��i �al��t�rmak i�in buton komutu verildi
    {
        player2.SetActive(true);
        player2Anim.SetBool("isGameStartBool", true);
        gameManager.twoWorkerButton.SetActive(false);
    }

    IEnumerator UpPositionTime()
    {
        yield return new WaitForSeconds(0.1f);
        groundObjects.transform.position += new Vector3(0, 0.25f, 0);
    }

    IEnumerator NailScaleTime()
    {
        transform.DOScale(new Vector3(0.37f, 0.37f, 0.43f), 0.1f);
        yield return new WaitForSeconds(0.1f);
        transform.DOScale(new Vector3(0.3f, 0.3f, 0.5f), 0.1f);
    }
}
