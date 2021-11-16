using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseController : MonoBehaviour
{
    public AnimationComponent animationComponent;
    public HeroFsmStateBase state;
    public float radius;
    public SceneTree tree;

    public Vector3 pos;
   
    private void Awake()
    {
        //animationComponent = GetComponent<AnimationComponent>();
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //GetComponent<NavMeshAgent>().SetDestination(hit.point);
                //GameManager.Instance.GetCharacterFactory().CreateEnemy(Assets.Scripts.EnemySystem.EnemyType.NormalZombie, hit.point, "NormalZombie");
                List<GameObject> objs = tree.Sphere(hit.point, radius);
                pos = hit.point;
                //foreach (var obj in objs)
                //{
                //    Debug.LogWarning(obj.name);
                //}
                Debug.Break();
            }
        }
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pos, radius);
    }
}
