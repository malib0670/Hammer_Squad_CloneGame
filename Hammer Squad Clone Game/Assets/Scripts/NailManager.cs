using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class NailManager : MonoBehaviour
{
    GameManager gameManager; // -> Game Manager sýnýfýndan veri çekildi

    public Transform groundObjects;

    public ParticleSystem hitParticle, fragmentParticle;
    AudioSource audioSource;
    public AudioClip hitClip;

    public Animator player1Anim, player2Anim;

    public GameObject player2;
    public GameObject tapToStartPanel;

    int hitCount = 0; // -> Çiviye vuruþ sayýsý kontrol edildi
    int groundCount = 1; // -> Zemin listesinde yok olma ayarlamasý yapýldý

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
        // -> Oyunun baþlamasý kontrol edildi 

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
                transform.DOMoveY(transform.position.y - 0.1f, 0.1f); // -> Çivinin Y ekseninde ne kadar hareket edeceði ve ne kadar zamanda
                                                                        // hareket edeceði kontrol edildi

                StartCoroutine(NailScaleTime()); // -> Çiviye her vuruþta Scale deðiþimi kontrol edildi

                gameManager.SpawnScoreText(); // -> Game Manager sýnýfýndan method çaðýrýldý
                gameManager.IncraseTotalMoneyCount(); // -> // -> Game Manager sýnýfýndan method çaðýrýldý

                hitParticle.Play();
                audioSource.PlayOneShot(hitClip);

                hitCount++;

                if (hitCount == 5) 
                {
                    fragmentParticle.Play();
                    Destroy(ListManager.instance.groundList[ListManager.instance.groundList.Count - groundCount], 0.1f); // -> Her 3 vuruþta zemin
                                                    // yok olmasý ayarlandý
                    hitCount = 0; 
                    groundCount++; 

                    StartCoroutine(UpPositionTime()); // -> Belli bir sürede zeminin ne kadar yukarý hareket edeceði kontrol edildi

                    transform.DOMoveY(7.6f, 0.5f); // -> 3 vuruþ sonrasý çivinin Y ekseninde eski halini almasý ayarlandý
                }

                if (GameManager.totalScoreCount == 30) // -> Para scoru 30 olduðunda 2 iþçi çalýþtýrma butonu aktif oldu
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

    public void TwoWorkerRunButton() // -> Ýki iþçi çalýþtýrmak için buton komutu verildi
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
