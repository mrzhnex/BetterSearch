using UnityEngine;

namespace BetterSearch
{
    class CooldownSteal : MonoBehaviour
    {
        private float timer = 0f;
        private float timeIsUp = 1.0f;
        public float cooldown = Global.cooldown_steal;

        public void Update()
        {
            timer = timer + Time.deltaTime;
            if (timer >= timeIsUp)
            {
                timer = 0f;
                cooldown = cooldown - timeIsUp;

            }

            if (cooldown <= 0f)
            {
                Destroy(gameObject.GetComponent<CooldownSteal>());
            }
        }
    }
}