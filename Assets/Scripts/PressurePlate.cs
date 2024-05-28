using UnityEngine;


    public class PressurePlate : MonoBehaviour
    {
        private bool isActive = false;

        // This method could be called when a player steps on the pressure plate
        public void Activate()
        {
            if(isActive == true)
            {
                return;
            }
            isActive = true;
            transform.Translate(0, -0.06f, 0);
            // Add code here to perform an action when the pressure plate is activated
        }

        public void Deactivate()
        {
            print("Deactivate");
            if(isActive == false)
            {
                return;
            }
            isActive = false;
            transform.Translate(0, 0.06f, 0);

            // Add code here to perform an action when the pressure plate is deactivated
        }

        public bool IsActive()
        {
            return isActive;
        }
    }
