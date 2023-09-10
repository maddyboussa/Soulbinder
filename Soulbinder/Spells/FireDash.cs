using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Soulbinder
{
    /// <summary>
    /// child class of the parent Spells class,
    /// represents the fire dash spell
    /// </summary>
    public class FireDash : Spell
    {
        // fields
        private int manaCost;
        private int damage;
        private bool alreadyHit;
        private double attackDuration;
        private Player player;

        // properties
        public int ManaCost { get { return manaCost; } set { manaCost = value; } }

        public int Damage { get { return damage; } set { damage = value; } }

        // constructor
        public FireDash(Texture2D spellTexture, int x, int y, int width, int height, Player player)
            : base(spellTexture, x, y, width, height)
        {
            manaCost = 3;
            damage = 10;
            alreadyHit = false;
            this.player = player;

            unlocked = false;
            name = "Fire Dash";
        }


        // methods

        /// <summary>
        /// begins the cast of the spell
        /// </summary>
        public void BeginCast(Vector2 spellVector, Vector2 center)
        {
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

            isCasting = true;
            alreadyHit = false;

            attackDuration = 250;

            // update player invincibility
            player.StartInvincibility(attackDuration);
        }


        /// <summary>
        /// check all entities on screen for a collision with the spell hit boxes,
        /// then deal damage accordingly
        /// </summary>
        /// <param name="enemyList"></param>
        /// <param name="tileList"></param>
        public void CheckSpellCollision(List<Skeleton> enemyList, List<Rectangle> tileList, Game1 game)
        {
            // first check if a collision has already occured,
            // if so, stop here
            if (alreadyHit)
            {
                return;
            }

            Rectangle dashHitBox = new Rectangle(spellRect.X - spellRect.Width / 2, spellRect.Y - spellRect.Height / 2,
                spellRect.Width, spellRect.Height);

            // check collisions with the hitbox (just spell rectangle) and enemies
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].Collides(dashHitBox))
                {
                    enemyList[i].CurrentHealth -= damage;
                    alreadyHit = true;
                }
            }

            // check boss collisions
            if (game.CurrentLevel.Boss != null)
            {
                for (int i = game.CurrentLevel.Boss.Count - 1; i >= 0; i--)
                {
                    if (game.CurrentLevel.Boss[i].Collides(dashHitBox))
                    {
                        game.CurrentLevel.Boss[i].CurrentHealth -= damage;
                        alreadyHit = true;
                    }
                }
            }
        }

        /// <summary>
        /// once the cast has already started (BeginCast()),
        /// execute the cast for a set number of frames
        /// </summary>
        /// <param name="entity"></param>
        public void ContinueCast(List<Skeleton> enemyList, List<Rectangle> tileList, double elapsedMilliseconds, Game1 game)
        {
            //check for collisions with another entity
            CheckSpellCollision(enemyList, tileList, game);

            // check if the spell is still happening (also based on elapsed time)
            attackDuration -= elapsedMilliseconds;

            if (attackDuration <= 0)
            {
                isCasting = false;
            }
        }


        /// <summary>
        /// draws the spell
        /// </summary>
        /// <param name="sb"></param>
        public void DrawSpell(SpriteBatch sb, Vector2 center, int camX, Vector2 dashVector)
        {
            float spellAngle = (float)Math.Atan2(dashVector.Y, dashVector.X);

            sb.Draw(spellTexture, new Rectangle(spellRect.X - camX, spellRect.Y, spellRect.Width, spellRect.Height), null, Color.White, spellAngle,
             new Vector2(spellTexture.Width / 2, spellTexture.Height / 2), SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// draws the hitbox for fireDash (the spell rectangle)
        /// </summary>
        /// <param name="center"></param>
        public void DrawSpellShape(Vector2 center, int camX)
        {
            ShapeBatch.BoxOutline((spellRect.X - spellRect.Width / 2) - camX, spellRect.Y - spellRect.Height / 2, 
                spellRect.Width, spellRect.Height, Color.Fuchsia);
        }
    }
}
