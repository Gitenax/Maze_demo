using UnityEngine;

namespace Source.Interaction.Areas
{
    public class DeadArea : Area
    {
        protected override void OnTriggerEnterHandler(Collider other)
        {
            if(LayerMask.GetMask(Constants.Layers.Player) != 1 << other.gameObject.layer)
                return;

            Player player = other.GetComponentInParent<Player>();
            if(player.ShieldActive)
                return;
            
            player.Kill();
            Debug.Log("YOU LOSE!");
        }

        protected override void OnTriggerStayHandler(Collider other)
        {
            if(LayerMask.GetMask(Constants.Layers.Player) != 1 << other.gameObject.layer)
                return;

            Player player = other.GetComponentInParent<Player>();
            if(player.ShieldActive)
                return;
            
            player.Kill();
            Debug.Log("YOU LOSE!");
        }
    }
}