using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Stats
{
    [Serializable]
    class PlayerStat
    {
        public string Username { get; set; }
        public int NbKills { get; set; }
        public int NbAsteroids { get; set; }
        public int NbPoints { get; set; }
        public int NbDeaths { get; set; }
        public int NbPowerUps { get; set; }
        public int NbGames { get; set; }
        public int NbWins { get; set; }
        public int MaxKills { get; set; }
        public int MaxPoints { get; set; }
        public int MaxPowerUps { get; set; }
        public int MaxDeaths { get; set; }
    }
}
