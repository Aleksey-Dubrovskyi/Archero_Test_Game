using UnityEngine;

public class SaveHandler : MonoBehaviour
{

    // Start is called before the first frame update
    [SerializeField]
    Shell shell;
    private void Awake()
    {
        SaveSystem.Init();
    }


    public void Save()
    {
        SaveObject save = new SaveObject
        {
            playerHP = 1500,
            playerSpeed = 5f,
            shellDamage = 150,
            meleDamage = 100,
            shellSpeed = 20f,
            slyderHealth = 200,
            flyerHealth = 400,
            bossHealth = 2500,
            slyderSpeed = 3f,
            flyerSpeed = 5f,
            bossSpeed = 2f
        };

        string json = JsonUtility.ToJson(save);
        SaveSystem.Save(json);
    }

    public void Load()
    {
        string saveString = SaveSystem.Load();
        if (saveString != null)
        {
            SaveObject save = JsonUtility.FromJson<SaveObject>(saveString);
            Player.instance.playerHP = save.playerHP;
            Player.instance.playerSpeed = save.playerSpeed;
            DamageManager.instance.shellDamage = save.shellDamage;
            DamageManager.instance.meleDamage = save.meleDamage;
            shell.speedValue = save.shellSpeed;
            GameManager.instance.SetEnemiesParameters(save.slyderHealth, save.flyerHealth, save.bossHealth, save.slyderSpeed, save.flyerSpeed, save.bossSpeed);
            GameManager.instance.SetPlayerParameters(save.playerHP, save.playerSpeed);
        }

    }

    private class SaveObject
    {
        public int playerHP;
        public float playerSpeed;
        public int shellDamage;
        public int meleDamage;
        public float shellSpeed;
        public int slyderHealth;
        public int flyerHealth;
        public int bossHealth;
        public float slyderSpeed;
        public float flyerSpeed;
        public float bossSpeed;
    }
}
