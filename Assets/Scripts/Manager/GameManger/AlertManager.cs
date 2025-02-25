using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
    public GameObject AlertObject;
    public TextMeshProUGUI AlertText;
    private Coroutine _alertCoroutine;

    public void ShowAlert(string msg)
    {
        AlertText.text = msg;
        AlertObject.SetActive(true);

        if (_alertCoroutine != null) StopCoroutine(_alertCoroutine);
        _alertCoroutine = StartCoroutine(AlertCo());
    }
    private IEnumerator AlertCo()
    {
        yield return new WaitForSecondsRealtime(2f);
        AlertObject.SetActive(false);
    }
}
