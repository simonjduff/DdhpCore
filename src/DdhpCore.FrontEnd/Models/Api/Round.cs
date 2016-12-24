namespace DdhpCore.FrontEnd.Models.Api
{
    public class Round
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int RoundNumber { get; set; }
        public bool RoundComplete { get; set; }
        public int NormalRound { get; set; }
    }
}