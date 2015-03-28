using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ZitaAsteria.MenuSystem;
using ZitaAsteria.MenuSystem.World;
using ZAsteroids.World.HUD;

namespace ZAsteroids.World
{
    /// <summary>
    /// The collision detection controller.
    /// </summary>
    public static class CollisionDetectionController
    {
        /// <summary>
        /// Get the list of objects that collide with the ray.
        /// </summary>
        /// <param name="ray"></param>
        /// <returns></returns>
        public static List<RayIntersectionInfo> GetIntersectionRayObjects(Ray ray)
        {
            float? rayDistance = 0;
            List<RayIntersectionInfo> collisions = new List<RayIntersectionInfo>();
            foreach (SceneObject obj in MenuContainer.Spheres)
            {
                if ((rayDistance = obj.IsCollidingWith(ray)) != null)
                    collisions.Add(new RayIntersectionInfo(obj, ray, (float)rayDistance, MenuContainer.Spheres));
            }
            foreach (SceneObject obj in MenuContainer.Asteroids)
            {
                if ((rayDistance = obj.IsCollidingWith(ray)) != null)
                    collisions.Add(new RayIntersectionInfo(obj, ray, (float)rayDistance, MenuContainer.Asteroids));
            }
            foreach (SceneObject obj in MenuContainer.ShipsOrbiting)
            {
                if ((rayDistance = obj.IsCollidingWith(ray)) != null)
                    collisions.Add(new RayIntersectionInfo(obj, ray, (float)rayDistance, MenuContainer.ShipsOrbiting));
            }
            return collisions;
        }

        /// <summary>
        /// Check the ray against the objects
        /// </summary>
        /// <param name="ray"></param>
        public static void CheckIntersectionRayCollision(Vector3 startLocation, Ray ray, float damage, float maxRange)
        {
            List<RayIntersectionInfo> rayIntersects = GetIntersectionRayObjects(ray);
            ProcessRayIntersectionList(startLocation, rayIntersects, damage, maxRange);
        }

        /// <summary>
        /// Process the general collision detection rules.
        /// </summary>
        public static void ProcessGeneralCollisionDetection()
        {
            //General spheres
            List<SPHEREs.SPHERE> spheresRemoved = new List<SPHEREs.SPHERE>();
            List<Rock> rocksRemoved = new List<Rock>();
            foreach (SPHEREs.SPHERE sphere in MenuContainer.Spheres)
            {
                if (sphere.IsCollidable)
                {
                    //vs. Asteroids
                    foreach (Rock rock in MenuContainer.Asteroids)
                    {
                        if (sphere.IsCollidingWith(rock) && rock.IsCollidable)
                        {
                            if (!spheresRemoved.Contains(sphere))
                                spheresRemoved.Add(sphere);
                        }
                    }

                    //vs. Planet
                    if (MenuContainer.Planet.IsCollidingWith(sphere))
                        if (!spheresRemoved.Contains(sphere))
                            spheresRemoved.Add(sphere);
                }
            }

            //Rocks
            foreach (Rock rock in MenuContainer.Asteroids)
            {
                if (rock.IsCollidable)
                {
                    if (rock.IsCollidingWith(MenuContainer.Satellite))
                        if (!rocksRemoved.Contains(rock))
                        {
                            //rock.SizesTillBreak = 0; //Stop it from breaking down.
                            rock.Location -= 3.0f * rock.Velocity;
                            rock.Velocity *= -1f; //Make it head in the other direction.
                            MenuContainer.Satellite.Life -= rock.Life; //Subtract the life.
                            rocksRemoved.Add(rock);
                        }

                    //Check the shields
                    if (MenuContainer.Shield.IsDrawing)
                    {
                        if (MenuContainer.Shield.IsCollidingWith(rock))
                            if (!rocksRemoved.Contains(rock))
                            {
                                rock.SizesTillBreak = 0; //Stop it from breaking down.
                                rock.Velocity = -2.0f * rock.Velocity; //Stop it so that it flames against the shield and flies back - this is a general approximation okay, should be a cross product.
                                rocksRemoved.Add(rock);
                            }
                    }

                }
            }
            
            //Check if the player sphere has hit the satellite or the shield.
            if (
                MenuContainer.MenuSystemScene.CurrentLocation == MenuSceneLocations.ZAsteroidsSpheres && 
                MenuContainer.SpherePlayer.Life > 0 && 
                MenuContainer.SpherePlayer.IsCollidable) //We're in sphere mode, check the player sphere
            {
                //If not colliding with the shield, we're outside it - die.
                if (!MenuContainer.SpherePlayer.IsCollidingWith(MenuContainer.SpherePlayer) ||
                    MenuContainer.SpherePlayer.IsCollidingWith(MenuContainer.Satellite)) //or colliding with the satellite.
                {
                    MenuContainer.SpherePlayer.DieEffect(MenuContainer.SpherePlayer.Location);
                    MenuContainer.SpherePlayer.Life = 0;
                    //Subtract from the score!
                    HUDProperties.GameManager.ScoreZAst -= 5000;
                    if (HUDProperties.GameManager.ScoreZAst < 0) HUDProperties.GameManager.ScoreZAst = 0;
                }
            }

            //Finally, process the list.
            for (int i = 0; i < spheresRemoved.Count; i++)
            {
                spheresRemoved[i].DieEffect(spheresRemoved[i].Location);
                MenuContainer.Spheres.Remove(spheresRemoved[i]);
            }
            //Finally, process the list.
            for (int i = 0; i < rocksRemoved.Count; i++)
            {
                rocksRemoved[i].DieEffect(rocksRemoved[i].Location);
                MenuContainer.Asteroids.Remove(rocksRemoved[i]);
            }
        }

        /// <summary>   
        /// Process the list of intersections with the applied damage. This is the second step in the processing, first is creating the list, second is applying it here.
        /// Can do it in one step with CheckIntersectionRayCollision.
        /// </summary>
        /// <param name="intersects"></param>
        /// <param name="damage"></param>
        public static void ProcessRayIntersectionList(Vector3 startLocation, List<RayIntersectionInfo> rayIntersects, float damage, float maxRange)
        {
            float remainingDamage = damage;
            //Check the objects.
            for (int i = 0; i < rayIntersects.Count; i++)
            {
                if (remainingDamage > 0)
                {
                    float originalLife = rayIntersects[i].Object.Life;
                    float inflictedDamage = remainingDamage / (MathHelper.Clamp(rayIntersects[i].Distance / 600.0f, 1, 1000));
                    rayIntersects[i].Object.Life -= inflictedDamage;
                    remainingDamage -= inflictedDamage;
                    if (rayIntersects[i].Object.GetType() == typeof(Rock))
                        HUDProperties.GameManager.ScoreZAst += (int)inflictedDamage;
                    if (rayIntersects[i].Object.Life <= 0)
                    {
                        rayIntersects[i].Object.DieEffect(rayIntersects[i].Ray.Position + rayIntersects[i].Ray.Direction * rayIntersects[i].Distance);
                        rayIntersects[i].ListToRemoveFrom.Remove(rayIntersects[i].Object);
                        //Subtract from damage
                        //remainingDamage -= originalLife;
                        //Apply the points.
                    }
                    else
                    {
                        remainingDamage = 0;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Removes the asteroids inside the shields when the shield is turned on.
        /// </summary>
        public static void OnShieldChange_CleanupAsteroids()
        {
            //Check the shields
            if (MenuContainer.Shield.IsDrawing)
            {
                for(int i = MenuContainer.Asteroids.Count-1 ; i >= 0; i--)
                {
                    Rock rock = MenuContainer.Asteroids[i] as Rock;
                    if (MenuContainer.Shield.IsCollidingWith(rock))
                        MenuContainer.Asteroids.Remove(rock); //Clear out the rock.
                }
            }

        }
    }
}
