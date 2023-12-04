using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public static class CharacterControllerTypeHolder 
{

    private static Dictionary<BodyType, System.Type> controllerDictionary;
    public static Dictionary<BodyType, System.Type> ControllerDictionary
    {
        get
        {
            if (controllerDictionary == null)
            {
                controllerDictionary = new Dictionary<BodyType, System.Type>();
                controllerDictionary[BodyType.CharacterController] = typeof(CharacterController);
                controllerDictionary[BodyType.Rigidbody] = typeof(RigidbodyCharacterController);
                controllerDictionary[BodyType.Tennis] = typeof(TennisCharacterController);
            }

            return controllerDictionary;
        }
    }


    public static System.Type GetBehevior(this BodyType controllerType)
    {
        return ControllerDictionary[controllerType];
    }
}
