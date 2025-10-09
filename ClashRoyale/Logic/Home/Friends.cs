using System;

namespace ClashRoyale.Logic.Home
{
    public class FacebookData
    {
        public string Identifier { get; set; }
    }

    public class GamecenterData
    {
        public string Identifier { get; set; }
    }

    public class Friends
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public FacebookData Facebook { get; set; }
        public GamecenterData Gamecenter { get; set; }
        public int ExpLevel { get; set; }
        public int Score { get; set; }
        public int Arena { get; set; }
    }
}