using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Soulbinder
{
    /// <summary>
    /// child class of spell,
    /// represents a deflection shield that will reflect enemy projectiles
    /// </summary>
    public class Deflect : Spell
    {
        // fields
        private int manaCost;
        private double attackDuration;
        private Player player;
        SpriteManager sm;

        // list of projectiles from the player
        private List<Projectile> projectiles;
        private Projectile reflectedProjectile;

        // Projectile object for deflecting
        public Projectile ReflectedProjectile 
        { 
            get { return reflectedProjectile; } 
            set { reflectedProjectile = value; } 
        }

        // properties
        public int ManaCost { get { return manaCost; } set { manaCost = value; } }

        // constructor
        public Deflect(Texture2D spellTexture, int x, int y, int width, int height, Player player, SpriteManager sm)
            : base(spellTexture, x, y, width, height)
        {
            // these are arbitrary values for the cost and damage we can change it later
            manaCost = 1;
            this.player = player;
            ReflectedProjectile = null;

            unlocked = false;
            name = "Deflect";

            this.sm = sm;
        }

        // methods

        /// <summary>
        /// begins the cast of the spell
        /// </summary>
        public void BeginCast(Vector2 spellVector, Vector2 center)
        {
            // If the player is not in God Mode
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

            isCasting = true;

            // arbitrary value for attack duration, we can change this later
            attackDuration = 250;

            // update player invincibility
            player.StartInvincibility(attackDuration);

            // update list of projectiles based on the player's property
            projectiles = player.ProjectileList;
        }

        /// <summary>
        /// check all projectiles on screen for a collision with the spell hit boxes
        /// </summary>
        public void CheckSpellCollision(List<Skeleton> enemyList, List<Rectangle> tileList, Vector2 spellVector, Game1 game)
        {
            if(projectiles == null)
            {
                return;
            }

            // create a seperate rectangle for the deflect hitbox
            // this is done in order to give the player a little more lee-way in terms of deflecting projectiles
            // Here, we specifically want to increase the width slightly of our hitbox
            Rectangle deflectHitBox = new Rectangle(spellRect.X - spellRect.Width / 2, spellRect.Y - spellRect.Height / 2,
                spellRect.Width + 25, spellRect.Height);

            // Check collision with projectiles only
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i].Collides(deflectHitBox))
                {
                    ReflectedProjectile = projectiles[i];
                    ReflectedProjectile.Damage = 10;
                    ReflectedProjectile.Sprite = sm.RockSprite;
                    projectiles.RemoveAt(i);

                    ReflectedProjectile.DeflectSpeed = new Vector2(16 * spellVector.X, 16 * spellVector.Y);
                    attackDuration = 0;

                    break;
                }
            }
        }


        /// <summary>
        /// once the cast has already started (BeginCast()),
        /// execute the cast for a set number of frames
        /// </summary>
        /// <param name="entity"></param>
        public void ContinueCast(List<Skeleton> enemyList, List<Rectangle> tileList, double elapsedMilliseconds, Vector2 spellVector, Game1 game)
        {
            //check for collisions with another entity
            CheckSpellCollision(enemyList, tileList, spellVector, game);

            // check if the spell is still happening (also based on elapsed time)
            attackDuration -= elapsedMilliseconds;

            if (attackDuration <= 0)
            {
                isCasting = false;
            }
        }

        /// <summary>
        /// draw the the spell
        /// </summary>
        /// <param name="sb"></param>
        public void DrawSpell(SpriteBatch sb, Vector2 center, int camX, Vector2 spellVect)
        {
            sb.Draw(spellTexture, new Rectangle(spellRect.X - camX, spellRect.Y, spellRect.Width, spellRect.Height), null, Color.White, 
               (float) Math.Atan2(spellVect.Y, spellVect.X), new Vector2(spellTexture.Width / 2, spellTexture.Height / 2), 
               SpriteEffects.None, 0.0f);
        }

    }
}
