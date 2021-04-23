using UnityEngine;
using System.Collections;

public class TryAgain : MonoBehaviour {
    public void try_again()
    {
        Application.LoadLevel("Southport");
        Time.timeScale = 1;
    }
}
