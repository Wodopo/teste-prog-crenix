using System;
using UnityEngine;

namespace Crenix
{
    public class InteractionManager
    {
        public static void Grab(IGrabbable grabbable, IGrabber grabber)
        {
            if (grabber.IsOccupied)
            {
                Return(grabbable);
                return;
            }
            if (grabbable.Grabber != null)
                grabbable.Grabber.Release();
            grabber.Grab(grabbable);
            grabbable.SetOwner(grabber);
        }

        public static void Drop(IGrabbable grabbable)
        {
            var overlaps = Physics2D.OverlapCircleAll(grabbable.Position, 1.0f);

            if (overlaps.Length == 0)
            {
                Return(grabbable);
                return;
            }

            GameObject GetClossest(Collider2D[] colliders, Vector2 pos)
            {
                float distance = Mathf.Infinity;
                GameObject result = null;

                for (int i = 0; i < colliders.Length; i++)
                {
                    Collider2D collider = colliders[i];
                    float tempDistance = Vector2.Distance(collider.transform.position, pos);
                    if (tempDistance < distance)
                    {
                        distance = tempDistance;
                        result = collider.gameObject;
                    }
                }

                return result;
            }

            var slotGO = GetClossest(overlaps, grabbable.Position);
            var grabber = slotGO.GetComponent<IGrabber>();

            if (grabber == null)
                throw new Exception("Should not return a null Grabber");

            Grab(grabbable, grabber);
        }

        public static void Return(IGrabbable grabbable)
        {
            if (grabbable.Grabber != null)
                grabbable.Grabber.Grab(grabbable);
        }
    }
}
