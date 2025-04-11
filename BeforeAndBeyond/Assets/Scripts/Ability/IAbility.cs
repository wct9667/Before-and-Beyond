using System;

namespace Ability
{
    public interface IAbility
    {
        //define a name for dobugging/maybe display purposes
        public string Name
        {
            get;
        }
        
        /// <summary>
        /// Activates the Ability
        /// </summary>
        public void ActivateAbility();
    }

}
