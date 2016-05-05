using UnityEngine;
using System.Collections;

public class ContinueButton : MonoBehaviour {

    public void ContinueGame() {
        Manager.Instance.adverts.ShowAd();
    }

}
