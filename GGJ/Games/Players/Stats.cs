using Microsoft.Xna.Framework;

namespace GGJ.Games.Players
{
    internal class Stats
    {
        public float Health = 100;
        public sbyte Sanity = 100;

        public sbyte Hunger = 0;
        public sbyte MaxHunger = 100;

        public sbyte Bladder = 0;
        public sbyte MaxBladder = 100;

        public sbyte Thirst = 0;
        public sbyte MaxThirst = 100;

        public byte Maxed()
        {
            byte numMaxed = 0;

            if (Hunger >= 100)
            {
                numMaxed++;
            }

            if (Bladder >= 100)
            {
                numMaxed++;
            }

            if (Thirst >= 100)
            {
                numMaxed++;
            }

            if (Sanity <= 0)
            {
                numMaxed++;
            }

            return numMaxed;
        }

        public void AddHealth(float val)
        {
            Health = MathHelper.Clamp(Health + val, 0, 100);
        }

        public void AddSanity(sbyte val)
        {
            Sanity = (sbyte) MathHelper.Clamp(Sanity + val, 0, 100);
        }

        public void AddHunger(sbyte val)
        {
            Hunger = (sbyte) MathHelper.Clamp(Hunger + val, 0, 100);
        }

        public void AddThirst(sbyte val)
        {
            Thirst = (sbyte) MathHelper.Clamp(Thirst + val, 0, 100);
        }

        public void AddBladder(sbyte val)
        {
            Bladder = (sbyte) MathHelper.Clamp(Bladder + val, 0, 100);
        }
    }
}