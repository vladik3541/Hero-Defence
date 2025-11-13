using UnityEngine;
public class ThroneAndTowerSpawner : MonoBehaviour
{
    [SerializeField] private Transform throne;
    [SerializeField] private Transform[] towers;
    [SerializeField] private UnitDataBase dataBase;
    public void Initialize(RaceType raceType)
    {
        foreach (var build in dataBase.builds)
        {
            if (build.Type == raceType)
            {
                Instantiate(build.Throne, throne.position, Quaternion.identity);
                for (int i = 0; i < towers.Length; i++)
                {
                    Instantiate(build.Tower, towers[i].position, Quaternion.identity);
                }
            }
        }
    }
}
