using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DroneExplorer
{
    /// <summary>
    /// Project: 
    /// This project enables the exploration of 3D space by way of a quad-copter drone. The intent 
    /// is to provide a quick drop in package to allow a user to explore a scene in the Unity Play 
    /// engine. The code is written to be modular and can incorporate other gameplay elements with 
    /// the addition of more scripts.
    /// 
    /// Author: Wilson Sauders 
    /// Email: HamsterUnity@gmail.com
    /// Date:   03 / 20 / 2022 
    /// 
    /// File:
    /// I am the gun class I contain the template for the bullets that will shoot from me. I handel internal
    /// refire rate
    /// </summary>
    public class Gun : MonoBehaviour
    {
        public bool shoot;          // flag to shoot
        public float refire;        // time between shots
        float refireCur = 0;        // current time conuter intil next shot

        public int team = 0;         // loyalty to not shoot self
        public float speed = 100;   // speed of bullet
        public float lifeTime = 5;  // how long the bullet will exist
        public float dammage = 1;   // how much dammge the bullet will do

        public float spray;         // bullet inacuracy 0 means perfect acuracy.
                                    // 1 means maximum aim woggle of one unit at 100 units

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // decriment refireCur
            if (refireCur >= 0) refireCur -= Time.deltaTime;

            // shoot if shoot flag is on and refire timeout
            if (shoot && refireCur <= 0)
            {
                refireCur = refire; // reset refire counter
                                    // tell bullet pool to fire a bullet at this position
                BulletPool pool = BulletPool.instance;
                if (pool != null)
                {
                    pool.Shoot(transform, team, speed, lifeTime, dammage, spray);
                }
            }

        }
    }
}