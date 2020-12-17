using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class RobotIdAuthoring: MonoBehaviour, IConvertGameObjectToEntity 
{
  public int id;
  
  public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
  {
    var robotId = new RobotId { Value = id }; // A component data that has only one float called Value
    dstManager.AddComponentData(entity, robotId);
  }
}
