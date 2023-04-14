using System;

namespace Model
{
    public class FightablePlayer : IFightable
    {
        public Player Player { get; private set; }

        private int _hp;
        private int _timedHp;
        public FightablePlayer(Player player)
        {
            Player = player;
            _hp = player.HP;
            _timedHp = 0;
        }

        public int GetAc() => Player.AC;

        public int GetHp() => _hp;

        public int GetInitiative() => Player.Initiative;

        public string GetName() => Player.Name;

        public string GetOptionalText() => $"{Player.Class}, {Player.Race} {Player.Lvl} уровня";

        public int GetTimedHp() => _timedHp;

        public void SetHp(int hp) => _hp = hp;

        public void SetTimedHp(int hp) => _timedHp = hp;

        public void ChangeHp(int ofset)
        {
            if(ofset < 0)
            {
                var newOfset = Math.Clamp(ofset + _timedHp, ofset, 0);
                _timedHp -= newOfset - ofset;
                ofset = newOfset;
            }
            _hp = Math.Clamp(_hp + ofset, 0, Player.HP);
        }
    }
}
