using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC578_Project
{
    public sealed class GameEngine
    {
        private static readonly GameEngine instance = new GameEngine();
        static GameEngine() { }
        private GameEngine() { }

        public static GameEngine Instance
        {
            get
            {
                return instance;
            }

        }
    }
}
