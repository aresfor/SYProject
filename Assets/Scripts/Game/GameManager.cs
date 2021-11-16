using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.EnemySystem;
using Sirenix.OdinInspector;
using Sel.SkillSystem;


public class GameManager : SerializedMonoBehaviour
{
    public static GameManager Instance;
    private EnemySystem enemySystem;
    private CharacterFactory characterFactory;
    private SkillSystem skillSystem;

    public CharacterFactory GetCharacterFactory()
    {
        return characterFactory;
    }
    public void GetAttack(int instanceID,int damage)
    {
        enemySystem.GetAttack(instanceID, damage);
    }
    public void AddEnemy(int instanceID,Enemy enemy)
    {
        enemySystem.AddEnemy(instanceID, enemy);
    }
    private void Awake()
    {
        if (Instance) { Destroy(this); }
        Instance = this;
        enemySystem = new EnemySystem();
        characterFactory = new CharacterFactory();
        skillSystem = new SkillSystem();
    }
    void Start()
    {
        CallBack call = TempCreateEnemy;
        TimerManager.Instance.SetTimer(call, 1,true,true,3);
        //GameObject.Find("irelia_fullanimation3").GetComponent<SkinnedMeshRenderer>().enabled = false;
        //StartCoroutine(loadscene());
    }
    //IEnumerator loadscene()
    //{
    //    yield return new WaitForSeconds(1);
    //    UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("tempScene",UnityEngine.SceneManagement.LoadSceneMode.Additive);
    //}
    public void TempCreateEnemy()
    {
        characterFactory.CreateEnemy(EnemyType.NormalZombie, Vector3.zero, "NormalZombie");
    }
    void Update()
    {
        //TODO XXX
        skillSystem.Update();
        
    }
}
