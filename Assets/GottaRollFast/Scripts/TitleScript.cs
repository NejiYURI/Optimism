using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScript : MonoBehaviour
{
    public Image SetExplosion;

    public TitleCharacter titleCharacter;

    private void Start()
    {
        if (GameSettingScript.instance != null)
        {
            SetExplosion.color = GameSettingScript.instance.Explosion ? Color.white : new Color(Color.gray.r, Color.gray.g, Color.gray.b, 0.3f);
        }
    }
    public void TutorialBtn()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void LoadStage(string StageName)
    {
        SceneManager.LoadScene(StageName);
    }

    public void ChangeExplosion()
    {
        if (GameSettingScript.instance != null)
        {
            GameSettingScript.instance.Explosion = !GameSettingScript.instance.Explosion;
            SetExplosion.color = GameSettingScript.instance.Explosion ? Color.white : new Color(Color.gray.r, Color.gray.g, Color.gray.b, 0.3f);
        }
        if (titleCharacter != null) titleCharacter.ClickExplosion();
    }
}
