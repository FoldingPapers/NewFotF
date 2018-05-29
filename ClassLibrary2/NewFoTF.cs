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

            //Here want to hook into the AfterAttackHook to do something at the end of the attack animation.
            ModHooks.Instance.AfterAttackHook += OnAfterAttack;
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
            if (PlayerData.instance.equippedCharm_6)
            {
                Log("Stronger Attack");

                _tempNailDamage = PlayerData.instance.nailDamage; //Store the current nail damage.

                Log("Set _tempNailDamage to " + _tempNailDamage);


                PlayerData.instance.nailDamage += 5 * (PlayerData.instance.maxHealth - PlayerData.instance.health); //Double the nail damage

                PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");

                _NailDamageTracker = PlayerData.instance.nailDamage;

                Log("Set Nail Damage to to " + _NailDamageTracker);

                return;
            }
        }

        /// <summary>
        /// Reverts damage
        /// </summary>
        /// <remarks>After the attack is over, we need to reset the nail damage back to what it was.</remarks>
        /// <param name="dir"></param>
        private void OnAfterAttack(AttackDirection dir)
        {
            Log("Finished Attacking");

            PlayerData.instance.nailDamage = _tempNailDamage; //Attacking is done, we need to set the nail damage back to what it was before we crit.

            PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");

            _NailDamageTracker = PlayerData.instance.nailDamage;

            Log("Set Nail Damage to to " + _NailDamageTracker);
        }
    }

}