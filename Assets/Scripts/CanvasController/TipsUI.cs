using System;
using TMPro;
using UnityEngine;

public class TipsUI : MonoBehaviour
{

    [SerializeField] private TipsUIItem tipsPrefab;
    [SerializeField] private string tipsMessages;
    private TipsUIItem currentTipsInstance;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (AudioManager.Instanse != null && AudioManager.Instanse.interact != null)
                AudioManager.Instanse.PlaySFX(AudioManager.Instanse.interact);

            if (tipsPrefab != null)
                currentTipsInstance = Instantiate(tipsPrefab, transform.position, Quaternion.identity);

        //     if (_tipsText != null)
        //         _tipsText.text = tipsMessages;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (tipsPrefab != null)
                Destroy(currentTipsInstance.gameObject);
        }
    }
}