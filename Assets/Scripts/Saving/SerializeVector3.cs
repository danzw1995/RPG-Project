using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RPG.Saving
{
  [Serializable]
  public class SerializeVector3
  {
    public float x;
    public float y;
    public float z;

    public SerializeVector3(Vector3 vector)
    {
      x = vector.x;
      y = vector.y;
      z = vector.z;
    }

    public Vector3 ToVector()
    {
      return new Vector3(x, y, z);
    }
  }
}
