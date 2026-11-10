using UnityEngine;

public class NPC : MonoBehaviour, IChangeVis
{
    public void IsVisibleToCam(bool playerLOS)
    {
        if(playerLOS == true)
        {
            this.gameObject.layer = LayerMask.NameToLayer("VisEnemies");
            print("vis");
        }
        else
        {
            this.gameObject.layer = LayerMask.NameToLayer("Enemies");
            print("invis");
        }
    }
}
