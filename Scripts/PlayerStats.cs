using UnityEngine;

public class PlayerStats : MonoBehaviour {

    [Header("Starting Attributes")]
    public int startLives = 20;
    public int startMoney = 400;

    private int _lives;
    public int lives { get { return _lives; } }

    public static int Lives;
    public static int Money;
    public static int Rounds;

    public static int playerID = 1;


    void Start()
    {
        Lives = startLives;
        Money = startMoney;
        Rounds = 0;
    }

    public void Damage(int _dmg)
    {
        if((_lives -= _dmg) <= 0)
        {
            _lives = 0;
        }else
        {
            _lives -= _dmg;
        }
    }

}
