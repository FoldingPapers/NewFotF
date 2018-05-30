using System.Reflection;
using GlobalEnums;
using Modding;

namespace NewFotF
{
    /// <summary>
    /// The main mod class
    /// </summary>
    /// <remarks>This configuration has settings that are save specific</remarks>
    public class NewFotF : Mod
    {

        private int _tempNailDamage;

        private int _NailDamageTracker;

        public int defaultNailDamage;

        /// <summary>
        /// Represents this Mod's instance.
        /// </summary>
        internal static NewFotF Instance;

        /// <summary>
        /// Fetches the Mod Version From AssemblyInfo.AssemblyVersion
        /// </summary>
        /// <returns>Mod's Version</returns>
        public override string GetVersion() => "0.0.1";



        /// <summary>
        /// Called after the class has been constructed.
        /// </summary>
        public override void Initialize()
        {
            //Assign the Instance to the instantiated mod.
            Instance = this;

            Log("Initializing");

            //Here we are hooking into the AttackHook so we can modify the damage for the attack.
            ModHooks.Instance.AttackHook += OnAttack;

            Log("Initialized");
        }


        /// <summary>
        /// Calculates Crits on attack
        /// </summary>
        /// <remarks>
        /// This checks if we have FoTF. If we do Damage is calculated based on lost masks. otherwise we revert back to normal
        /// </remarks>
        /// <param name="dir"></param>
        public void OnAttack(AttackDirection dir)
        {
            Log("Attacking");
            
            if (defaultNailDamage == 0)
            {
                defaultNailDamage = 5 + 4 * PlayerData.instance.GetInt("nailSmithUpgrades")
            }

            if (PlayerData.instance.GetInt("nailDamage") != defaultNailDamage)
            {
                PlayerData.instance.GetInt("nailDamage") = defaultNailDamage;

                if (PlayerData.instance.GetBool("equippedCharm_6"))
                {
                    Log("Stronger Attack");

                    PlayerData.instance.SetInt("nailDamage", PlayerData.instance.GetInt("nailDamage") + 5 * (PlayerData.instance.GetInt("maxHealth") - PlayerData.instance.GetInt("health");

                    PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");

                    _NailDamageTracker = PlayerData.instance.GetInt("nailDamage");

                    Log("Set Nail Damage to to " + _NailDamageTracker);

                    return;
                }
            }

        }

        /// <summary>
        /// Reverts damage
        /// </summary>
        /// <remarks>After the attack is over, we need to reset the nail damage back to what it was.</remarks>
        /// <param name="dir"></param>

    }

}
