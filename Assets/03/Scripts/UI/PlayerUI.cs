using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public TMP_Text _playerText;
    //private Player _player;


    public void SetPlayer(string name)
    {
       // _player = player;
        _playerText.text = name; 
    }
}
