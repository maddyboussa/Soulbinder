using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Soulbinder
{
    /// <summary>
    /// child class of Spell, represents a basic attack spell
    /// </summary>
    public class BasicAttack : Spell
    {
        // fields
        private int manaCost;
        private int damage;
        private bool alreadyHit;
        private Player player;
        private double attackDuration;
        private bool rotatable;

        // positioning fields
        private Vector2 circleEdge;
        private Vector2 spellHitBox1;
        private Vector2 spellHitBox2;
        private float spellAngle;

        // collision detection fields
        private float hitBoxRadius;


        // properties
        public int ManaCost { get { return manaCost; } set { manaCost = value; } }

        public int Damage { get { return damage; } set { damage = value; } }

        // constructor
        public BasicAttack(Texture2D spellTexture, int x, int y, int width, int height, Player player)
            : base(spellTexture, x, y, width, height)
        {
            manaCost = 0; // for now takes no mana
            damage = 5;
            alreadyHit = false;
            this.player = player;

            // initialize position vectors with default values
            circleEdge = new Vector2(0, 0);
            spellHitBox1 = new Vector2(0, 0);
            spellHitBox2 = new Vector2(0, 0);
            rotatable = false;
            hitBoxRadius = spellRect.Width / 4;

            // Basic Attack is always unlocked
            unlocked = true;
            name = "Basic Attack";
        }

        // methods

        /// <summary>
        /// begins the cast of the spell
        /// </summary>
        public void BeginCast(Vector2 spellVector, Vector2 center)
        {
            rotatable = true;
            isCasting = true;
            alreadyHit = false;
            attackDuration = 300;

            // If the player is not in God Mode,
            // ensures mana cannot be below zero
            // and decrements mana accordingly
            if (!player.GodMode)
            {
                if (player.CurrentMana - manaCost < 0)
                {
                    player.CurrentMana = 0;
                }
                else
                {
                    player.CurrentMana -= manaCost;
                }
            }

            // set spell angle relative to player upon casting
            spellAngle = (float)Math.Atan2(spellVector.Y, spellVector.X);

            // calculate spell positioning
            // center point of the first spell hitbox relative to player center
            spellHitBox1.X = center.X + ((32 + hitBoxRadius) * (float)Math.Cos(spellAngle));
            spellHitBox1.Y = center.Y + ((32 + hitBoxRadius) * (float)Math.Sin(spellAngle));

            // center point of the second spell hitbox relative to player center
            spellHitBox2.X = center.X + ((32 + (3 * hitBoxRadius)) * (float)Math.Cos(spellAngle));
            spellHitBox2.Y = center.Y + ((32 + (3 * hitBoxRadius)) * (float)Math.Sin(spellAngle));
        }

        /// <summary>
        /// once the cast has already started (BeginCast()),
        /// execute the cast for a set number of frames
        /// </summary>
        /// <param name="entity"></param>
        public void ContinueCast(List<Skeleton> enemyList, List<Rectangle> tileList, double elapsedMilliseconds, Vector2 center, Game1 game)
        {
            // check all collisions with the spell
            CheckSpellCollision(enemyList, tileList, game);

            // decrement the duration of the spell,
            // ensures that the spell cast lasts only for a set amount of time
            attackDuration -= elapsedMilliseconds;

            if (attackDuration <= 0)
            {
                isCasting = false;
            }

            // center point of the first spell hitbox relative to player center
            spellHitBox1.X = center.X + ((32 + hitBoxRadius) * (float)Math.Cos(spellAngle));
            spellHitBox1.Y = center.Y + ((32 + hitBoxRadius) * (float)Math.Sin(spellAngle));

            // center point of the second spell hitbox relative to player center
            spellHitBox2.X = center.X + ((32 + (3 * hitBoxRadius)) * (float)Math.Cos(spellAngle));
            spellHitBox2.Y = center.Y + ((32 + (3 * hitBoxRadius)) * (float)Math.Sin(spellAngle));
        }

        /// <summary>
        /// check all entities on screen for a collision with the spell circle hit boxes,
        /// then deal damage accordingly
        /// </summary>
        /// <param name="enemyList"></param>
        /// <param name="tileList"></param>
        public void CheckSpellCollision(List<Skeleton> enemyList, List<Rectangle> tileList, Game1 game)
        {
            if (alreadyHit)
            {
                return;
            }
            
            // check collisions with damageable enemies if the spell has not already collided
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].CircleRectCollides(spellHitBox1, hitBoxRadius) 
                    || enemyList[i].CircleRectCollides(spellHitBox2, hitBoxRadius))
                {
                    enemyList[i].CurrentHealth -= damage;

                    // gain back mana with successful attack
                    // ensures mana never goes above max
                    if (player.CurrentMana + 1 > player.MaximumMana)
                    {
                        player.CurrentMana = player.MaximumMana;
                    }
                    else
                    {
                        player.CurrentMana += 1;
                    }

                    alreadyHit = true;
                }
            }

            // Check for collision with boss
            if (game.CurrentLevel.Boss != null)
            {
                for (int i = game.CurrentLevel.Boss.Count - 1; i >= 0; i--)
                {
                    if (game.CurrentLevel.Boss[i].CircleRectCollides(spellHitBox1, hitBoxRadius)
                    || game.CurrentLevel.Boss[i].CircleRectCollides(spellHitBox2, hitBoxRadius))
                    {
                        game.CurrentLevel.Boss[i].CurrentHealth -= damage;
                    }


                    // gain back mana with successful attack
                    // ensures mana never goes above max
                    if (player.CurrentMana + 1 > player.MaximumMana)
                    {
                        player.CurrentMana = player.MaximumMana;
                    }
                    else
                    {
                        player.CurrentMana += 1;
                    }

                    alreadyHit = true;
                }
            }
        }

        /// <summary>
        /// draw the the spell if the player is casting
        /// </summary>
        /// <param name="sb"></param>
        public void DrawSpell(SpriteBatch sb, Vector2 center, int camX)
        {
            // calculate the point on the edge of the player's circle along the above angle
            // player circle radius is 64
            circleEdge.X = center.X + (64 * (float)Math.Cos(spellAngle));
            circleEdge.Y = center.Y + (64 * (float)Math.Sin(spellAngle));

            // draw spell
            // origin x is 256 because we sacled the image down by 4, 64 * 4 is 256
            sb.Draw(spellTexture, new Rectangle(spellRect.X - camX, spellRect.Y, spellRect.Width, spellRect.Height), null, 
                Color.White, spellAngle, new Vector2(-128, spellTexture.Height / 2), SpriteEffects.None, 0.0f);
        }


        /// <summary>
        /// draws the shapes of the spell hitboxes
        /// </summary>
        /// <param name="center"></param>
        /// <param name="mousePosition"></param>
        public void DrawSpellShape(Vector2 center, Vector2 mousePosition, int camX)
        {
            // draw spell hit boxes based on center vectors
            ShapeBatch.CircleOutline(new Vector2(spellHitBox1.X - camX, spellHitBox1.Y), hitBoxRadius, Color.Purple);
            ShapeBatch.CircleOutline(new Vector2(spellHitBox2.X - camX, spellHitBox2.Y), hitBoxRadius, Color.Purple);
        }
    }
}
