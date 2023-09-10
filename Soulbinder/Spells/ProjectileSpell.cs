using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Soulbinder
{
    /// <summary>
    /// child class that inherits from spell,
    /// represents a blast-like projectile that the player can shoot at enemies
    /// </summary>
    public class ProjectileSpell : Spell
    {

        // fields
        private int manaCost;
        private int damage;
        private double attackDuration;
        private bool hasCollided;
        private Player player;

        // properties
        public int ManaCost { get { return manaCost; } set { manaCost = value; } }

        public int Damage { get { return damage; } set { damage = value; } }

        public bool HasCollided { get { return hasCollided; } set { hasCollided = value; } }

        // constructor
        public ProjectileSpell(Texture2D spellTexture, int x, int y, int width, int height, Player player)
            : base(spellTexture, x, y, width, height)
        {
            manaCost = 3;
            damage = 15;
            this.player = player;

            unlocked = false;
            name = "Projectile";
        }

        // methods

        /// <summary>
        /// begins the cast of the spell
        /// </summary>
        public void BeginCast(Vector2 spellVector, Vector2 center)
        {
            // ensures mana cannot be below zero
            // and decrements mana accordingly
            if (player.CurrentMana - manaCost < 0)
            {
                player.CurrentMana = 0;
            }
            else
            {
                player.CurrentMana -= manaCost;
            }

            isCasting = true;
            hasCollided = false;

            attackDuration = 1500;
        }

        /// <summary>
        /// check all projectiles on screen for a collision with the spell hit boxes
        /// </summary>
        public void CheckSpellCollision(List<Skeleton> enemyList, List<Rectangle> tileList, Vector2 spellVector, Game1 game)
        {
            // check collisions with the hitbox (just spell rectangle) and enemies
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].Collides(spellRect) && !hasCollided)
                {
                    enemyList[i].CurrentHealth -= damage;
                    hasCollided = true;
                }
            }

            // check boss collisions
            if (game.CurrentLevel.Boss != null)
            {
                for (int i = game.CurrentLevel.Boss.Count - 1; i >= 0; i--)
                {
                    if (game.CurrentLevel.Boss[i].Collides(spellRect))
                    {
                        game.CurrentLevel.Boss[i].CurrentHealth -= damage;
                        hasCollided = true;
                    }
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

            // stop the spell if it collides with an enemy or tile, or the duration has been exhausted
            if (attackDuration <= 0 || hasCollided)
            {
                isCasting = false;
            }
        }


        /// <summary>
        /// draw the the spell if the player is casting
        /// </summary>
        /// <param name="sb"></param>
        public void DrawSpell(SpriteBatch sb, Vector2 center, int camX, Vector2 spellVect)
        {
            sb.Draw(spellTexture, new Rectangle(spellRect.X - camX, spellRect.Y, spellRect.Width, spellRect.Height), null, Color.White,
               (float)Math.Atan2(spellVect.Y, spellVect.X), new Vector2(spellTexture.Width / 2, spellTexture.Height / 2),
               SpriteEffects.None, 0.0f);
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
