using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinPopup : MonoBehaviour
{
    public static void Create(Vector3 position, int value)
    {
        //float offset = 0.15f;
        //Vector3 offsetVector = new Vector3(Random.Range(-offset, offset), Random.Range(-offset, offset), 0);
        //Vector3 spawnPosition = position + offsetVector;

        Transform pfCoinPopup = GameAssets.Instance.coinPopup;
        Transform coinPopupTransform = Instantiate(pfCoinPopup, position, Quaternion.identity);

        coinPopupTransform.Find("text").GetComponent<TextMeshPro>().SetText(value.ToString());

        float destroyDelay = 1f;
        Destroy(coinPopupTransform.gameObject, destroyDelay);
    }
}
