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
    /// I am the bullet pool singleton. I create a bunch of general use bullets that gun classes 
    /// in the scene can pull from.
    /// </summary>

    public class BulletPool : MonoBehaviour
    {
        public static BulletPool instance;  // public static reference to self
        public int count;                   // maximum bullets in scene
        public GameObject baseBullet;       // link to bullet prefab
        Bullet[] myBullets;                 // internal storage of bullets
        int nextindex = 0;                  // index of next bullet to be spawned
                                            // Start is called before the first frame update
        void Start()
        {
            // store instance 
            instance = this;

            // create new bullets in scene and child them to this obect to pevent scene cultter.
            myBullets = new Bullet[count];
            for (int itor = 0; itor < count; itor++)
            {
                GameObject temp = Instantiate(baseBullet, transform);
                myBullets[itor] = temp.GetComponent<Bullet>();

            }
        }

        // Update is called once per frame
        void Update()
        {
            // do nothing
        }

        /// <summary>
        /// Called externaly. will create a bullet at position source with the desired properties.
        /// </summary>
        /// <param name="source"> location and rotation in game world where bullet will spawn</param>
        /// <param name="_shooter"> faction loyalty of bullet prevents shooting self</param>
        /// <param name="_speed">units per second the bullet will travel forward</param>
        /// <param name="_lifeTime">how long the bullet stays in scene if it hits nothing.</param>
        /// <param name="_dammage">dammage done to Hit Taker</param>
        /// <param name="spray">bullet inacuracy spray</param>
        public void Shoot(Transform source, int _shooter, float _speed, float _lifeTime, float _dammage, float spray)
        {
            // pass shooting instructions to selected bullet;

            myBullets[nextindex].Shoot(source, _shooter, _speed, _lifeTime, _dammage);

            // set bullet spray woggle
            Vector3 point = myBullets[nextindex].transform.position +
                                myBullets[nextindex].transform.forward * 100 +
                                myBullets[nextindex].transform.right * Random.Range(-spray, spray) +
                                myBullets[nextindex].transform.up * Random.Range(-spray, spray);
            myBullets[nextindex].transform.LookAt(point);
            // incirment nextindex to cicle through the array
            nextindex += 1;
            if (nextindex >= count) nextindex = 0;
        }
    }
}