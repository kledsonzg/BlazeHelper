namespace BlazeHelper.BlazeRecipes
{
    public class UserExperience
    {
        public int Level {get; set;}
        public int NextLevel {get; set;}
        public int Xp {get; set;}
        public string Rank {get; set;} = "Bronze";
        public string NextLevelProgress {get; set;} = "0.00";
    }
}