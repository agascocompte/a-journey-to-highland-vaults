namespace ArionDigital
{
    using UnityEngine;
    using System.Collections.Generic;

    public class CrashCrate : MonoBehaviour
    {
        [Header("Whole Create")]
        public MeshRenderer wholeCrate;
        public BoxCollider boxCollider;
        public BoxCollider boxColliderPlayer;
        [Header("Fractured Create")]
        public GameObject fracturedCrate;
        [Header("Audio")]
        public AudioSource crashAudioClip;
        public int hitsUntilBreak = 2;
        [Header("Drops")]
        public List<GameObject> potions;
        public int potionDropRate = 25;
        public List<GameObject> buffs;
        public int buffDropRate = 25;

        float actualHits = 0;

        float timeUntilDestroy = 3f;
        //float timeUntilDisapear = 1f;
        float timer = 0f;
        bool hasToDestroy = false;
        public void Update()
        {
            if (hasToDestroy)
            {
                timer += Time.deltaTime;
                //if (timer > timeUntilDisapear)
                //{
                //    transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);
                //}

                if (timer > timeUntilDestroy)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "PlayerProjectile")
            {
                actualHits += 0.5f;
                if (actualHits >= hitsUntilBreak)
                {
                    wholeCrate.enabled = false;
                    boxCollider.enabled = false;
                    boxColliderPlayer.enabled = false;
                    fracturedCrate.SetActive(true);
                    crashAudioClip.Play();

                    GameObject objectToDrop = GeneratePossibleDrop();
                    if (objectToDrop != null)
                    {
                        Drop(objectToDrop);
                    }

                    hasToDestroy = true;
                }
                
            }
        }

        [ContextMenu("Test")]
        public void Test()
        {
            wholeCrate.enabled = false;
            boxCollider.enabled = false;
            fracturedCrate.SetActive(true);
        }


        public GameObject GeneratePossibleDrop()
        {
            // Pociones
            int haveToSpawnPotion = Random.Range(0, 100);
            if (haveToSpawnPotion < potionDropRate)
            {
                int potionTypeIndex = Random.Range(0, potions.Count);
                return potions[potionTypeIndex];
            }

            // Buffs
            int haveToSpawnBuff = Random.Range(0, 100);
            if (haveToSpawnBuff < buffDropRate)
            {
                int buffTypeIndex = Random.Range(0, buffs.Count);
                return buffs[buffTypeIndex];
            }

            return null;
        }

        public void Drop(GameObject newObject)
        {
            GameObject drop = Instantiate(newObject, transform.position, transform.rotation);
            drop.transform.Rotate(270, 0, 0);
            drop.transform.position = new Vector3(drop.transform.position.x, 0.4f, drop.transform.position.z);
        }
    }
}