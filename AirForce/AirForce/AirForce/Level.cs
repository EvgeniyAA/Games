using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirForce
{
    class Level
    {
        private readonly Random rnd = new Random();
        public int CountToKill=5;
        public ObjectType TypeToKill = ObjectType.EnemyPlane;
        public int Frequency=50;
        public int LevelNumber=1;
        public int PlaneTypesOnLevel=1;
        public int Killed;
        private const int Step = 5;

        public void UpGameLevel()
        {
            LevelNumber++;
            if(PlaneTypesOnLevel<4)
                PlaneTypesOnLevel++;
            if(Frequency>20)
                Frequency -=Step;
            CountToKill += Step;
            Killed = 0;
            RefreshTypeToKill();
        }

        private void RefreshTypeToKill()
        {
            TypeToKill = (ObjectType) rnd.Next(3, PlaneTypesOnLevel+1);
        }
    }
}
