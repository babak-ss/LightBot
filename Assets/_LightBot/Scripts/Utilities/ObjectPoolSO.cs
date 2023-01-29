using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{

    /// Usage:
    /// 
    ///   There's no need to do any special setup of any kind.
    /// 
    ///   Instead of calling Instantiate(), use this:
    ///       SimplePool.Spawn(somePrefab, somePosition, someRotation);
    /// 
    ///   Instead of destroying an object, use this:
    ///       SimplePool.Remove(myGameObject);
    /// 
    ///   If desired, you can preload the pool with a number of instances:
    ///       SimplePool.Preload(somePrefab, 20);
    /// 

    [CreateAssetMenu(menuName = "One Time/ObjectPool")]
    public class ObjectPoolSO : ScriptableObject
    {
        private PoolMember _poolMemberTemp;

        // You can avoid resizing of the Stack's internal data by
        // setting this to a number equal to or greater to what you
        // expect most of your pool sizes to be.
        // Note, you can also use Preload() to set the initial size
        // of a pool -- this can be handy if only some of your pools
        // are going to be exceptionally large (for example, your bullets.)
        const int DEFAULT_POOL_SIZE = 1;

        /// <summary>
        /// The Pool class represents the pool for a particular prefab.
        /// </summary>
        class Pool
        {
            // We append an id to the name of anything we instantiate.
            // This is purely cosmetic.
            int nextId = 1;

            // The structure containing our inactive objects.
            // Why a Stack and not a List? Because we'll never need to
            // pluck an object from the start or middle of the array.
            // We'll always just grab the last one, which eliminates
            // any need to shuffle the objects around in memory.
            Stack<GameObject> inactive;

            // The prefab that we are pooling
            GameObject prefab;

            // Constructor
            public Pool(GameObject prefab, int initialQty)
            {
                this.prefab = prefab;

                // If Stack uses a linked list internally, then this
                // whole initialQty thing is a placebo that we could
                // strip out for more minimal code. But it can't *hurt*.
                inactive = new Stack<GameObject>(initialQty);
            }

            // Spawn an object from our pool
            public GameObject Get()
            {
                GameObject obj;
                if (inactive.Count == 0)
                {
                    // We don't have an object in our pool, so we
                    // instantiate a whole new object.
                    obj = Instantiate(prefab);
                    obj.SetActive(false);
                    nextId++;
                    obj.name = prefab.name + nextId;

                    // Add a PoolMember component so we know what pool
                    // we belong to.
                    obj.AddComponent<PoolMember>().myPool = this;
                }
                else
                {
                    // Grab the last object in the inactive array
                    obj = inactive.Pop();

                    if (obj == null)
                    {
                        // The inactive object we expected to find no longer exists.
                        // The most likely causes are:
                        //   - Someone calling Destroy() on our object
                        //   - A scene change (which will destroy all our objects).
                        //     NOTE: This could be prevented with a DontDestroyOnLoad
                        //	   if you really don't want this.
                        // No worries -- we'll just try the next one in our sequence.

                        return Get();
                    }
                }

                return obj;
            }

            // Return an object to the inactive pool.
            public void RetornToPool(GameObject obj)
            {
                obj.SetActive(false);

                // Since Stack doesn't have a Capacity member, we can't control
                // the growth factor if it does have to expand an internal array.
                // On the other hand, it might simply be using a linked list 
                // internally.  But then, why does it allow us to specify a size
                // in the constructor? Maybe it's a placebo? Stack is weird.
                inactive.Push(obj);
            }

        }


        /// <summary>
        /// Added to freshly instantiated objects, so we can link back
        /// to the correct pool on Remove.
        /// </summary>
        class PoolMember : MonoBehaviour
        {
            public Pool myPool;
        }

        // All of our pools
        private Dictionary<GameObject, Pool> pools;

        /// <summary>
        /// Initialize our dictionary.
        /// </summary>
        private void Init(GameObject prefab = null, int qty = DEFAULT_POOL_SIZE)
        {
            if (pools == null)
            {
                pools = new Dictionary<GameObject, Pool>();
            }

            if (prefab != null && pools.ContainsKey(prefab) == false)
            {
                pools[prefab] = new Pool(prefab, qty);
            }
        }

        /// <summary>
        /// If you want to preload a few copies of an object at the start
        /// of a scene, you can use this. Really not needed unless you're
        /// going to go from zero instances to 100+ very quickly.
        /// Could technically be optimized more, but in practice the
        /// Spawn/Remove sequence is going to be pretty darn quick and
        /// this avoids code duplication.
        /// </summary>
        public void Preload(GameObject prefab, int qty = 1)
        {
            Init(prefab, qty);

            // Make an array to grab the objects we're about to pre-spawn.
            GameObject[] obs = new GameObject[qty];
            for (int i = 0; i < qty; i++)
            {
                obs[i] = Get(prefab);
            }

            // Now Remove them all.
            for (int i = 0; i < qty; i++)
            {
                Remove(obs[i]);
            }
        }

        /// <summary>
        /// Spawns a copy of the specified prefab (instantiating one if required).
        /// NOTE: Remember that Awake() or Start() will only run on the very first
        /// spawn and that member variables won't get reset.  OnEnable will run
        /// after spawning -- but remember that toggling IsActive will also
        /// call that function.
        /// </summary>
        public GameObject Get(GameObject prefab)
        {
            Init(prefab);

            return pools[prefab].Get();
        }

        /// <summary>
        /// Remove the specified gameobject back into its pool.
        /// </summary>
        public void Remove(GameObject obj)
        {
            _poolMemberTemp = obj.GetComponent<PoolMember>();
            if (_poolMemberTemp == null)
            {
                Debug.Log("Object '" + obj.name + "' wasn't spawned from a pool. Destroying it instead.");
                GameObject.Destroy(obj);
            }
            else
            {
                _poolMemberTemp.myPool.RetornToPool(obj);
            }
        }
    }

}
