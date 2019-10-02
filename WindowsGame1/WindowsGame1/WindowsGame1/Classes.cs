using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Adastra
{
    // Player.
    /// <summary>
    /// The player. User controls this.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Angle of the player in radians. Used to determine movement and other things.
        /// </summary>
        public float angle { get; set; }

        // TODO: make an angleMoving, meaning that the player doesn't
        // necessarily move the direction it's facing.

        /// <summary>
        /// Determines how much to move the player left/right during the specific game update.
        /// </summary>
        public float amountMoveX { get; set; }

        /// <summary>
        /// Determines how much to move the player forward/backward during the specific game update.
        /// </summary>
        public float amountMoveY { get; set; }

        /// <summary>
        /// Determines how much to rotate the player during the specific game update.
        /// </summary>
        public float amountSpin { get; set; }

        /// <summary>
        /// What ship the player is currently residing in. Determines the player's image, shots, and
        /// other things.
        /// </summary>
        public Ship ship { get; set; }

        /// <summary>
        /// Determines where to place the destination marker.
        /// </summary>
        public Vector2 destPos { get; set; }

        /// <summary>
        /// Where the player wants to turn to face its destination marker.
        /// </summary>
        public float destAngle { get; set; }

        /// <summary>
        /// The "position" of the player, though the player never actually moves.
        /// </summary>
        public static Vector2 pos { get; set; }

        /// <summary>
        /// The current health of the player.
        /// </summary>
        public int health { get; set; }

        /// <summary>
        /// The amount of money that the player has.
        /// </summary>
        public int money { get; set; }

        /// <summary>
        /// The amount of time before the player can fire their next shot.
        /// </summary>
        public float cooldown { get; set; }

        /// <summary>
        /// Constructor for the player.
        /// </summary>
        /// <param name="ship">The ship to start the player off in.</param>
        public Player(Ship ship)
        {
            this.angle = 0;
            this.amountMoveX = 0;
            this.amountMoveY = 0;
            this.amountSpin = 0;
            this.ship = ship;
            this.health = ship.health;
            this.money = 0;
            this.cooldown = 0f;
        }
    }


    // Entities.
    /// <summary>
    /// Any thing that moves or needs collision.
    /// (ships, asteroids, stations, etc.)
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Used for entities that have a limited lifespan (shots and explosions).
        /// </summary>
        public virtual int aliveCounter { get; set; }

        /// <summary>
        /// The image that is displayed for the entity.
        /// </summary>
        public Texture2D image { get; set; }

        /// <summary>
        /// The position of the entity.
        /// </summary>
        public Vector2 pos { get; set; }

        /// <summary>
        /// The origin that the entity rotates around.
        /// </summary>
        public Vector2 origin { get; set; }

        /// <summary>
        /// The angle of rotation of the entity.
        /// </summary>
        public float angle { get; set; }

        /// <summary>
        /// The amount to move the entity along the Y axis (forward and backward of their angle)
        /// during the specific game update.
        /// </summary>
        public float amountMoveY { get; set; }

        /// <summary>
        /// The amount to move the entity along the X axis (right and left of their angle)
        /// during the specifc game update. Negative means left, positive means right.
        /// </summary>
        public float amountMoveX { get; set; }

        /// <summary>
        /// The amount to spin the entity during the specific game update.
        /// </summary>
        public float amountSpin { get; set; }

        /// <summary>
        /// Whether or not the entity has constant movement (doesn't slow down).
        /// TODO: make this actually work.
        /// </summary>
        public bool constantMovement { get; set; }

        /// <summary>
        /// The AI that controls this entity's movement.
        /// </summary>
        public EntityAI AI { get; set; }

        /// <summary>
        /// The radius of the entity. this is used for collision detection, so
        /// make it encompass the entire picture.
        /// </summary>
        public float radius { get; private set; }

        /// <summary>
        /// Health of the entity. If this reaches 0, the entity is destroy()ed.
        /// </summary>
        public int health { get; set; }

        /// <summary>
        /// Whether or not the entity is invincible.
        /// </summary>
        public bool invincible { get; set; }

        /// <summary>
        /// Constructor of entity.
        /// </summary>
        /// <param name="image">Image that the entity uses.</param>
        /// <param name="pos">Position of the entity.</param>
        /// <param name="origin">Origin of the entity, that it rotates around.</param>
        /// <param name="angle">Angle of rotation of the entity.</param>
        /// <param name="AI">AI that determines lo que el entity does.</param>
        public Entity(Texture2D image, Vector2 pos, Vector2 origin, float angle, int health, EntityAI AI)
        {
            Init(image, pos, origin, angle, 0, 0, 0, health, AI, false);
        }
        // Needed for inheritance.
        public Entity()
        {
        }

        /// <summary>
        /// Initialization of an entity. Called in the constructor, or call whenever you
        /// need to reset an entity.
        /// </summary>
        /// <param name="image">Image that the entity uses.</param>
        /// <param name="pos">Position of the entity.</param>
        /// <param name="origin">Origin of the entity, that it rotates around.</param>
        /// <param name="angle">Angle of rotation of the entity.</param>
        public void Init(Texture2D image, Vector2 pos, Vector2 origin, float angle, float amountMoveX, float amountMoveY,
                         float amountSpin, int health, EntityAI AI, bool invincible)
        {
            this.image = image;
            this.pos = pos;
            this.origin = origin;
            this.angle = angle;
            this.amountMoveX = amountMoveX;
            this.amountMoveY = amountMoveY;
            this.amountSpin = amountSpin;
            this.AI = AI;
            this.constantMovement = false;
            if (this.image != null) { this.radius = image.Width / 2; }
            this.health = health;
            this.invincible = invincible;
        }

        /// <summary>
        /// Called in update(), if the entity's AI returned something special,
        /// (other than 0), its value (1, 2, whatever was returned other than 0)
        /// will be passed here. For example: collision detection. Only not.
        /// </summary>
        /// <param name="updateReturn">The return value from the AI's update.</param>
        public virtual void specialUpdate(int updateReturn)
        {
        }

        /// <summary>
        /// Destroys the entity, removing it from the entityList and eventually will display
        /// any death animations, images, and sounds.
        /// </summary>
        public virtual void destroy(Entity en, Player p = null)
        {
        }
    }

    /// <summary>
    /// The spawn point of the player. Used to draw the overview map.
    /// </summary>
    public class SpawnPoint : Entity
    {
        public SpawnPoint()
        {
            base.Init(null, new Vector2(600, 400), new Vector2(600, 400), 0, 0, 0, 0, 1, new NullAI(), true);
        }
    }

    /// <summary>
    /// Any enemy ship.
    /// </summary>
    public class Enemy : Entity
    {
        /// <summary>
        /// The ship that the enemy is in.
        /// </summary>
        public Ship ship { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ship">The ship that the enemy is in.</param>
        /// <param name="pos">Position of the enemy.</param>
        /// <param name="health">Health of the entity. TODO: remove this (enemy health = ship health).</param>
        /// <param name="AI">AI that controls this enemy.</param>
        public Enemy(Ship ship, Vector2 pos, int health, EntityAI AI)
        {
            base.Init(ship.image, pos, ship.origin, 0, 0, 0, 0, health, AI, false); 
            this.ship = ship;
        }

        /// <summary>
        /// When an enemy is destroyed, remove it from the entity list, make a new explosion
        /// where the enemy is, and give the player some money.
        /// </summary>
        public override void destroy(Entity en = null, Player p = null)
        {
            Adastra.theMap.entityList.Remove(this);

            Explosion explosion = new Explosion(this); explosion.AI.controller = explosion;
            Adastra.theMap.addEntity(explosion);

            Adastra.player.money += 15;
        }
    }

    /// <summary>
    /// The shots that the player fires.
    /// </summary>
    public class Shot : Entity
    {
        /// <summary>
        /// Origin of the shot. Don't know why this is here but it seems important.
        /// </summary>
        public static Vector2 origin = new Vector2(Adastra.shotImage.Width / 2, Adastra.shotImage.Height / 2);

        /// <summary>
        /// Whether or not the player fired the shot. The player can't collide with their own shots.
        /// </summary>
        public bool playerShot { get; set; }

        /// <summary>
        /// Constructor of the shots that the player fires.
        /// </summary>
        /// <param name="image">Image that the shots take on.</param>
        /// <param name="pos">Position of the shot.</param>
        /// <param name="origin">Place that the shots rotate around.</param>
        /// <param name="angle">Angle of the shots.</param>
        public Shot(Vector2 pos, float angle, bool playerShot)
        {
            base.Init(
                Adastra.shotImage, 
                pos, 
                origin, 
                angle,
                0, 0, 0, 100, 
                new NullAI(), 
                true);
            this.playerShot = playerShot;
            this.aliveCounter = 180;
        }

        /// <summary>
        /// On death, just remove the shot from the entity list.
        /// </summary>
        public override void destroy(Entity en = null, Player p = null)
        {
            Adastra.theMap.entityList.Remove(this);
        }
    }

    /// <summary>
    /// Missiles that certain ships fire.
    /// </summary>
    public class Missile : Entity
    {
        /// <summary>
        /// The entity that the missile is heading towards. Can be null.
        /// </summary>
        public Entity destEn { get; set; }

        /// <summary>
        /// The player that the missile is heading towards. Can be null.
        /// </summary>
        public Player destPlayer { get; set; }

        /// <summary>
        /// Whether the missile was fired by the player or not.
        /// </summary>
        public bool playerShot { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pos">Position of the missile.</param>
        /// <param name="angle">Angle of the missile.</param>
        /// <param name="destEn">The entity that this missile is heading towards. Can be null.</param>
        /// <param name="destPlayer">The player that this missile is heading towards. Can be null.</param>
        public Missile(Vector2 pos, float angle, bool playerShot, Entity destEn = null, Player destPlayer = null)
        {
            base.Init(
                Adastra.missileImage,
                pos,
                new Vector2(Adastra.missileImage.Width / 2, Adastra.missileImage.Height / 2),
                angle,
                0, 14, 0, 10,
                new MissileAI(),
                true);
            this.aliveCounter = 60;
            this.destEn = destEn;
            this.destPlayer = destPlayer;
            this.playerShot = playerShot;
        }

        /// <summary>
        /// On death, just remove the missile.
        /// </summary>
        public override void destroy(Entity en, Player p = null)
        {
            Adastra.theMap.entityList.Remove(this);
        }
    }

    /// <summary>
    /// Any asteroid floating around space.
    /// </summary>
    public class Asteroid : Entity
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="posX">X position of the asteroid.</param>
        /// <param name="posY">Y position of the asteroid.</param>
        public Asteroid(int posX, int posY)
        {
            base.Init(
                Adastra.astrImage,
                new Vector2(posX, posY),
                new Vector2(Adastra.astrImage.Width / 2, Adastra.astrImage.Height / 2),
                0, 0, 0, 0,
                50,
                new AsteroidAI(),
                false);
        }

        /// <summary>
        /// On death, set the asteroid's image to a destroyed asteroid and make it invincible.
        /// Also gives the player money.
        /// </summary>
        public override void destroy(Entity en = null, Player p = null)
        {
            // Change the image, and set its health to 1 so that this isn't constantly called
            this.image = Adastra.astrImageDestroyed;
            this.health = 1;

            // Give the player money if they're close enough (3 screen widths).
            if (!this.invincible && Vector2.Distance(Player.pos, this.pos) <= 3600)
                Adastra.player.money += 5;

            // Make it invincible
            this.invincible = true;
        }
    }

    /// <summary>
    /// The, or any, planet floating around space.
    /// </summary>
    public class Planet : Entity
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="posX">X position of the planet.</param>
        /// <param name="posY">Y position of the planet.</param>
        public Planet(int posX, int posY)
        {
            base.Init(
                Adastra.planetImage,
                new Vector2(posX, posY),
                new Vector2(Adastra.planetImage.Width / 2, Adastra.planetImage.Height / 2),
                0, 0, 0, 0.0003F,
                1,
                new NullAI(),
                true);
        }
        
        // No destroy, as the planet will never die!
    }

    /// <summary>
    /// Any station that the player can buy from.
    /// </summary>
    public class Station : Entity
    {
        public Station(int posX, int posY)
        {
            base.Init(
                Adastra.stationImage,
                new Vector2(posX, posY),
                new Vector2(Adastra.stationImage.Width / 2, Adastra.stationImage.Height / 2),
                0, 0, 0, 0.0006F,
                1,
                new NullAI(),
                true);
        }
    }

    /// <summary>
    /// Explosions caused from the explosions of ships.
    /// </summary>
    public class Explosion : Entity
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enFrom">The enemy/entity that died to create this explosion.</param>
        public Explosion(Entity enFrom)
        {
            Quaternion rotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, enFrom.angle);
            base.Init(
                Adastra.explosionImages[0],
                Vector2.Transform(new Vector2(-Adastra.explosionImages[0].Width / 4, -Adastra.explosionImages[0].Height / 4), rotation) + enFrom.pos,
                enFrom.origin,
                enFrom.angle,
                enFrom.amountMoveX,
                enFrom.amountMoveY,
                enFrom.amountSpin,
                1,
                new AsteroidAI(),
                true);
            this.aliveCounter = 40;
        }

        /// <summary>
        /// Simply remove on death.
        /// </summary>
        public override void destroy(Entity en, Player p = null)
        {
            Adastra.theMap.entityList.Remove(this);
        }
    }


    // Entity AI's.
    /// <summary>
    /// Any AI that controls an entity's movement or behavior.
    /// </summary>
    public abstract class EntityAI
    {
        /// <summary>
        /// The entity that is controlled by this AI. 
        /// Make sure to set this when creating an entity.
        /// </summary>
        public Entity controller { get; set; }

        /// <summary>
        /// Enemy that is controlled by this AI.
        /// Make sure to set this if the entity is an enemy.
        /// </summary>
        public Enemy controllerEnemy { get; set; }

        /// <summary>
        /// Missile that is controlled by this AI.
        /// Make sure to set this if the entity is a missile.
        /// </summary>
        public Missile controllerMissile { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public EntityAI()
        {
        }

        /// <summary>
        /// Update the entity's ai. Called once per game update(), near the end.
        /// </summary>
        public virtual int update()
        {
            return 0;
        }

        /// <summary>
        /// Update the entity's ai, though once every 5 update()s.
        /// Use this for determining where/at what to move.
        /// </summary>
        public virtual int update5()
        {
            return 0;
        }

        /// <summary>
        /// Update the entity's ai, though once every 100 update()s.
        /// Use this for very slow movements, or whatever else.
        /// </summary>
        public virtual int update100(Random r)
        {
            return 0;
        }
    }

    /// <summary>
    /// An AI that does nothing.
    /// </summary>
    public class NullAI : EntityAI
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public NullAI()
        {
        }
    }

    /// <summary>
    /// AI of the asteroids.
    /// </summary>
    public class AsteroidAI : EntityAI
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public AsteroidAI()
        {
        }

        /// <summary>
        /// Every 100 game updates this is called.
        /// Randomly assign new movements/spins to the asteroids, to make it seem like they're
        /// floating around aimlessly.
        /// </summary>
        /// <param name="r">An instance of random. Required.</param>
        /// <returns>0</returns>
        public override int update100(Random r)
        {
            int n = r.Next(1, 18);

            switch(n)
            {
                case 1: this.controller.amountMoveX = 0.25F; break;
                case 2: this.controller.amountMoveX = -0.25F; break;
                case 3: this.controller.amountMoveY = 0.25F; break;
                case 4: this.controller.amountMoveY = -0.25F; break;
                case 5: this.controller.amountSpin = 0.002F; break;
                case 6: this.controller.amountSpin = -0.002F; break;
                case 7: this.controller.amountMoveX = 0; break;
                case 8: this.controller.amountMoveY = 0; break;
                case 9: this.controller.amountSpin = 0; break;
                default: break;
            }

            return 0;
        }
    }

    /// <summary>
    /// AI that controlls most of any enemies.
    /// </summary>
    public class EnemyAI : EntityAI
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EnemyAI()
        {
        }

        /// <summary>
        /// Sets the enemy's angle to the player's angle, and speeds up/slows down if it should/can.
        /// </summary>
        /// <returns>0</returns>
        public override int update()
        {
            // Set the angle equal to the player's angle.
            this.controller.angle = (float)Math.Atan2(
                Player.pos.Y - this.controller.pos.Y, Player.pos.X - this.controller.pos.X) + (float)Math.PI / 2;

            // Move faster toward the player. Max speed is 4.
            if (Vector2.Distance(this.controller.pos, Player.pos) <= 1200 * 2 &&
                Vector2.Distance(this.controller.pos, Player.pos) >= 400)
            {
                if (this.controller.amountMoveY <= this.controllerEnemy.ship.maxSpeed / 3)
                {
                    this.controller.amountMoveY += 0.01F;
                }
            }
            // Slow down if the player's too far away.
            else
            {
                if (this.controller.amountMoveY > 0)
                {
                    this.controller.amountMoveY -= 0.03F;
                }
            }

            return 0;
        }

        /// <summary>
        /// Fires a shot, and a missile if it can.
        /// </summary>
        /// <param name="r">An instance of random.</param>
        /// <returns>0</returns>
        public override int update100(Random r)
        {
            // If they're within 2 times the width of the screen, shoot at the player.
            if (Vector2.Distance(this.controller.pos, Player.pos) <= 1200 * 2)
            {
                Quaternion rotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, this.controller.angle);

                // Shoot a missile at the player if it can.
                if (this.controllerEnemy.ship.missileDisplacements != null)
                {
                    foreach (Vector2 missileOffset in this.controllerEnemy.ship.missileDisplacements)
                    {
                        Vector2 missilePos = Vector2.Transform(missileOffset, rotation) + this.controller.pos;
                        Missile missile = new Missile(missilePos, this.controller.angle, false, null, Adastra.player);
                        missile.AI.controller = missile; missile.AI.controllerMissile = missile;
                        Adastra.theMap.entityList.Add(missile);
                    }
                }

                // Then shoot at the player.
                else
                {
                    foreach (Vector2 shotOffset in this.controllerEnemy.ship.shotDisplacements)
                    {
                        Vector2 shotPos = Vector2.Transform(shotOffset, rotation) + this.controller.pos;
                        Shot shot = new Shot(shotPos, this.controller.angle, false);
                        shot.constantMovement = true; shot.amountMoveY = 8; Adastra.theMap.entityList.Add(shot);
                    }
                }
            }

            return 0;
        }
    }

    /// <summary>
    /// The AI that controls missiles. Will home on nearest target.
    /// </summary>
    public class MissileAI : EntityAI
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MissileAI() 
        {
        }

        /// <summary>
        /// Sets the missile's angle to hone on its target player/entity.
        /// </summary>
        /// <returns>0</returns>
        public override int update()
        {
            // For if the target is an entity.
            if (this.controllerMissile.destEn != null)
            {
                this.controller.angle = (float)Math.Atan2(this.controllerMissile.destEn.pos.Y - this.controller.pos.Y,
                    this.controllerMissile.destEn.pos.X - this.controller.pos.X) + (float)(Math.PI - 0.5) / 2;
            }
            // For if the target is the player.
            else if (this.controllerMissile.destPlayer != null)
            {
                this.controller.angle = (float)Math.Atan2(Player.pos.Y - this.controller.pos.Y,
                    Player.pos.X - this.controller.pos.X) + (float)(Math.PI - 2) / 2;
            }

            return 0;
        }
    }


    // Ships
    /// <summary>
    /// Any ship that the player or enemies can control.
    /// </summary>
    public class Ship
    {
        /// <summary>
        /// The name of the ship. Displayed in the UI.
        /// </summary>
        public string name { get; protected set; }

        /// <summary>
        /// Image of the ship.
        /// </summary>
        public Texture2D image { get; set; }

        /// <summary>
        /// A list of the vectors of the displacements of the shots that the ship fires.
        /// If you want to fire a shot from 16 pixels in front of the center of the ship's image,
        /// Vector2(-16, 0).
        /// </summary>
        public List<Vector2> shotDisplacements { get; set; }

        /// <summary>
        /// A list of vectors that represent the positions that missiles fire from the ship.
        /// Same as shotDisplacements except with missiles.
        /// </summary>
        public List<Vector2> missileDisplacements { get; set; }

        /// <summary>
        /// The current amount of health that the ship has.
        /// </summary>
        public int health { get; set; }

        /// <summary>
        /// The maximum amount of health that the ship can have.
        /// </summary>
        public int maxHealth { get; set; }

        /// <summary>
        /// Maximum speed of the ship.
        /// </summary>
        public int maxSpeed { get; set; }

        /// <summary>
        /// Speed of the acceleration of the ship.
        /// </summary>
        public float accelSpeed { get; protected set; }

        /// <summary>
        /// Radius of the ship. Used for collision detection.
        /// </summary>
        public int radius { get; protected set; }

        /// <summary>
        /// The origin of the ship. It's the place that the ship spins around.
        /// </summary>
        public Vector2 origin { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of the ship.</param>
        /// <param name="image">Image of the ship.</param>
        /// <param name="shotDisplacements">The displacements of the shots that this ship fires.</param>
        /// <param name="missileDisplacements">The displacements of the missiles that this ship fires.</param>
        /// <param name="maxHealth">The maximum amount of health this ship can have.</param>
        /// <param name="origin">The origin that this ship rotates around.</param>
        /// <param name="maxSpeed">The maximum speed of this ship.</param>
        /// <param name="accelSpeed">The speed this ship accelerates at.</param>
        public Ship(string name, Texture2D image, List<Vector2> shotDisplacements, List<Vector2> missileDisplacements, 
            int maxHealth, Vector2 origin, int maxSpeed, float accelSpeed) 
        {
            this.name = name;
            this.image = image;
            this.shotDisplacements = shotDisplacements;
            this.missileDisplacements = missileDisplacements;
            this.health = health;
            this.maxHealth = maxHealth;
            this.origin = origin;
            this.maxSpeed = maxSpeed;
            this.accelSpeed = accelSpeed;
            this.radius = image.Width / 2;
        }
        // Needed for inheritance.
        public Ship() { }
    }

    /// <summary>
    /// The first tier of cruisers. Cruisers are a middle-of-the-road-type-of-ship with 
    /// decent-but-not-wonderfully-damaging weapons.
    /// </summary>
    public class LightCruiser : Ship
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public LightCruiser()
        {
            this.name = "Light Cruiser";
            this.image = Adastra.lightCruiserImg;
            this.shotDisplacements = new List<Vector2>() { new Vector2(-32F, -80F), new Vector2(32F, -80F) };
            this.missileDisplacements = null;
            this.maxHealth = 100;
            this.health = 100;
            this.origin = new Vector2(Adastra.lightCruiserImg.Width / 2, Adastra.lightCruiserImg.Height / 2 + 27);
            this.maxSpeed = 11;
            this.accelSpeed = 0.25F;
            this.radius = Adastra.lightCruiserImg.Width / 2;
        }
    }

    /// <summary>
    /// The first tier of corvettes. Corvettes are generally faster but with less firepower.
    /// Isn't corvette an awesome word? Corvette. Corvettes.
    /// </summary>
    public class LightCorvette : Ship
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public LightCorvette()
        {
            this.name = "Light Corvette";
            this.image = Adastra.lightCorvetteImg;
            this.shotDisplacements = new List<Vector2>() { new Vector2(0, -90F) };
            this.missileDisplacements = new List<Vector2>() { new Vector2(0, -90F) };
            this.maxHealth = 100;
            this.health = 100;
            this.origin = new Vector2(Adastra.lightCorvetteImg.Width / 2, Adastra.lightCorvetteImg.Height / 2 + 10);
            this.maxSpeed = 14;
            this.accelSpeed = 0.3F;
            this.radius = Adastra.lightCorvetteImg.Width / 2;
        }
    }

    /// <summary>
    /// The first tier of frigates. Frigates are bigger, slower, firepower-er, less health-er ships.
    /// </summary>
    public class LightFrigate : Ship
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public LightFrigate()
        {
            this.name = "Light Frigate";
            this.image = Adastra.lightFrigateImg;
            this.shotDisplacements = new List<Vector2>() {
                new Vector2(0, -85F),
                new Vector2(-40, -75F),
                new Vector2(40, -75F)
            };
            this.maxHealth = 80;
            this.health = 80;
            this.origin = new Vector2(Adastra.lightFrigateImg.Width / 2, Adastra.lightFrigateImg.Height / 2);
            this.maxSpeed = 9;
            this.accelSpeed = 0.2F;
            this.radius = Adastra.lightFrigateImg.Width / 2;
        }
    }

    /// <summary>
    /// The boss ship. Technically a light destroyer. Its image is awesome.
    /// </summary>
    public class BossShip : Ship
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public BossShip()
        {
            this.name = "Anarian Light Destroyer";
            this.image = Adastra.bossImage;
            this.shotDisplacements = new List<Vector2>() {
                new Vector2(0, -160),
                new Vector2(-30, -155),
                new Vector2(30, -155),
                new Vector2(-60, -140),
                new Vector2(60, -140)
            };
            this.maxHealth = 600;
            this.health = 600;
            this.origin = new Vector2(Adastra.bossImage.Width / 2, Adastra.bossImage.Height / 2 + 52);
            this.maxSpeed = 6;
            this.accelSpeed = 0.2F;
        }
    }


    /// <summary>
    /// Any tile of the background.
    /// </summary>
    public class BackgroundTile
    {
        /// <summary>
        /// Position of the background in pixels.
        /// </summary>
        public Vector2 pos { get; set; }

        /// <summary>
        /// Image that the background uses.
        /// </summary>
        public Texture2D image { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pos">Position of the BackgroundTile.</param>
        /// <param name="image">Image that the BackgroundTile uses.</param>
        public BackgroundTile(Vector2 pos, Texture2D image)
        {
            this.pos = pos;
            this.image = image;
        }
    }


    // Other
    /// <summary>
    /// Any map that is used.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// List of all the entities on the map.
        /// </summary>
        public List<Entity> entityList { get; set; }

        /// <summary>
        /// List of all the backgrounds on the map.
        /// </summary>
        public List<BackgroundTile> backgroundList { get; set; }

        /// <summary>
        /// Constructor. Makes a new empty map.
        /// </summary>
        public Map()
        {
            this.entityList = new List<Entity>();
            this.backgroundList = new List<BackgroundTile>();
        }

        /// <summary>
        /// Adds an undefined number of entities to the entityList of this map.
        /// </summary>
        /// <param name="ens">Entities that you wish to add.</param>
        public void addEntity(params Entity[] ens)
        {
                // Player Entity Enemy Shot Asteroid Planet ShipPickup Station
            foreach (Entity en in ens)
            {
                this.entityList.Add(en);
            }
        }

        /// <summary>
        /// Adds an undefined number of background tiles to the backgroundList of this map.
        /// </summary>
        /// <param name="bgs">Backgrounds that you wish to add.</param>
        public void addBackground(params BackgroundTile[] bgs)
        {
            foreach (BackgroundTile bg in bgs)
            {
                this.backgroundList.Add(bg);
            }
        }

        /// <summary>
        /// Gets the index of the station.
        /// </summary>
        public int getStoreIndex()
        {
            if (this.entityList.Contains(new Station(-6000, 3500)))
            {
                return this.entityList.IndexOf(new Station(-6000, 3500));
            }
            return -1;
        }

        /// <summary>
        /// Sets the backgrounds that I want so I don't have to type them all in loadMap().
        /// TODO: make not as many backgrounds.
        /// </summary>
        public void setGenericBackgrounds()
        {
            this.addBackground
            (
                // From top left corner of the map...
                // x: 0, y: 0-12
                new BackgroundTile(new Vector2(-1200 * 6, -800 * 6), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 6, -800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 6, -800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 6, -800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 6, -800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 6, -800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 6, 800 * 0), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 6, 800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 6, 800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 6, 800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 6, 800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 6, 800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 6, 800 * 6), Adastra.backgroundImg),

                // x: 1, y: 0-12
                new BackgroundTile(new Vector2(-1200 * 5, -800 * 6), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 5, -800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 5, -800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 5, -800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 5, -800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 5, -800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 5, 800 * 0), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 5, 800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 5, 800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 5, 800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 5, 800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 5, 800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 5, 800 * 6), Adastra.backgroundImg),

                // x: 2, y: 0-12
                new BackgroundTile(new Vector2(-1200 * 4, -800 * 6), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 4, -800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 4, -800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 4, -800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 4, -800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 4, -800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 4, 800 * 0), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 4, 800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 4, 800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 4, 800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 4, 800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 4, 800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 4, 800 * 6), Adastra.backgroundImg),

                // x: 3, y: 0-12
                new BackgroundTile(new Vector2(-1200 * 3, -800 * 6), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 3, -800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 3, -800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 3, -800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 3, -800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 3, -800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 3, 800 * 0), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 3, 800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 3, 800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 3, 800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 3, 800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 3, 800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 3, 800 * 6), Adastra.backgroundImg),

                // x: 4, y: 0-12
                new BackgroundTile(new Vector2(-1200 * 2, -800 * 6), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 2, -800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 2, -800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 2, -800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 2, -800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 2, -800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 2, 800 * 0), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 2, 800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 2, 800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 2, 800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 2, 800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 2, 800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 2, 800 * 6), Adastra.backgroundImg),

                // x: 5, y: 0-12
                new BackgroundTile(new Vector2(-1200 * 1, -800 * 6), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 1, -800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 1, -800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 1, -800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 1, -800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 1, -800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 1, 800 * 0), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 1, 800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 1, 800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 1, 800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 1, 800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 1, 800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(-1200 * 1, 800 * 6), Adastra.backgroundImg),

                // x: 6, y: 0-12
                new BackgroundTile(new Vector2(1200 * 0, -800 * 6), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 0, -800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 0, -800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 0, -800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 0, -800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 0, -800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 0, 800 * 0), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 0, 800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 0, 800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 0, 800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 0, 800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 0, 800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 0, 800 * 6), Adastra.backgroundImg),

                // x: 7, y: 0-12
                new BackgroundTile(new Vector2(1200 * 1, -800 * 6), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 1, -800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 1, -800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 1, -800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 1, -800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 1, -800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 1, 800 * 0), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 1, 800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 1, 800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 1, 800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 1, 800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 1, 800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 1, 800 * 6), Adastra.backgroundImg),

                // x: 8, y: 0-12
                new BackgroundTile(new Vector2(1200 * 2, -800 * 6), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 2, -800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 2, -800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 2, -800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 2, -800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 2, -800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 2, 800 * 0), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 2, 800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 2, 800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 2, 800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 2, 800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 2, 800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 2, 800 * 6), Adastra.backgroundImg),

                // x: 9, y: 0-12
                new BackgroundTile(new Vector2(1200 * 3, -800 * 6), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 3, -800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 3, -800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 3, -800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 3, -800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 3, -800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 3, 800 * 0), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 3, 800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 3, 800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 3, 800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 3, 800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 3, 800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 3, 800 * 6), Adastra.backgroundImg),

                // x: 10, y: 0-12
                new BackgroundTile(new Vector2(1200 * 4, -800 * 6), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 4, -800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 4, -800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 4, -800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 4, -800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 4, -800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 4, 800 * 0), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 4, 800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 4, 800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 4, 800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 4, 800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 4, 800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 4, 800 * 6), Adastra.backgroundImg),

                // x: 11, y: 0-12
                new BackgroundTile(new Vector2(1200 * 5, -800 * 6), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 5, -800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 5, -800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 5, -800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 5, -800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 5, -800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 5, 800 * 0), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 5, 800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 5, 800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 5, 800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 5, 800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 5, 800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 5, 800 * 6), Adastra.backgroundImg),

                // x: 12, y: 0-12
                new BackgroundTile(new Vector2(1200 * 6, -800 * 6), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 6, -800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 6, -800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 6, -800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 6, -800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 6, -800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 6, 800 * 0), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 6, 800 * 1), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 6, 800 * 2), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 6, 800 * 3), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 6, 800 * 4), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 6, 800 * 5), Adastra.backgroundImg),
                new BackgroundTile(new Vector2(1200 * 6, 800 * 6), Adastra.backgroundImg)
            );
        }
    }

    /// <summary>
    /// Overlays, like a store browser, ship editor; anything that is drawn over
    /// the screen with buttons to edit stuff.
    /// </summary>
    public class Overlay
    {
        /// <summary>
        /// Image of the overlay.
        /// </summary>
        public Texture2D image;

        /// <summary>
        /// Key that is used to open the overlay.
        /// </summary>
        public Keys key;

        /// <summary>
        /// List of buttons on the overlay.
        /// </summary>
        public List<Button> buttons;
        
        /// <summary>
        /// Where the store is located.
        /// </summary>
        public Entity at;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="image">Image that the overlay uses.</param>
        /// <param name="key">Key that opens the overlay.</param>
        /// <param name="buttons">What buttons are in the overlay.</param>
        public Overlay(Texture2D image, Keys key, List<Button> buttons)
        {
            this.image = image;
            this.key = key;
            this.buttons = buttons;
        }

        /// <summary>
        /// Checks if the mouse's (destination image's) position is inside of any buttons.
        /// </summary>
        /// <param name="ms">Current mouse state on clicking.</param>
        /// <returns>Null if not in button, the button it is in otherwise.</returns>
        public Button getClickedOn(MouseState ms, GamePadState gs)
        {
            if (gs.IsConnected)
            {
                foreach (Button b in buttons)
                {
                    if (new Rectangle(b.pos.X + 150, b.pos.Y + 100, b.pos.Width, b.pos.Height).Contains(new Point((int)Adastra.player.destPos.X, (int)Adastra.player.destPos.Y)))
                    {
                        return b;
                    }
                }
            }
            else
            {
                foreach (Button b in buttons)
                {
                    if (new Rectangle(b.pos.X + 150, b.pos.Y + 100, b.pos.Width, b.pos.Height).Contains(new Point(ms.X, ms.Y)))
                    {
                        return b;
                    }
                }
            }

            return null;
        }
    }

    /// <summary>
    /// Buttons that can be clicked on in an overlay.
    /// </summary>
    public class Button
    {
        /// <summary>
        /// Image that the button uses.
        /// </summary>
        public Texture2D image;

        /// <summary>
        /// Name of the button. Used to determine what it does.
        /// </summary>
        public string name;

        /// <summary>
        /// Position of the button in relation to the overlay.
        /// </summary>
        public Rectangle pos;

        /// <summary>
        /// The text displayed inside of the button.
        /// </summary>
        public string text;

        /// <summary>
        /// How much it costs to buy the button's upgrade.
        /// </summary>
        public int cost;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="image">The image that the button uses.</param>
        /// <param name="name">Name of the button.</param>
        /// <param name="pos">Position of the button in relation to the overlay.</param>
        public Button(Texture2D image, string name, Rectangle pos, string text, int cost)
        {
            this.image = image;
            this.name = name;
            this.pos = pos;
            this.text = text;
            this.cost = cost;
        }
    }
}
