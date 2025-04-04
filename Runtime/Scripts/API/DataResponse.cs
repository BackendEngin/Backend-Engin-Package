public class DataResponse
{
    public string Success { get; set; }
    public string Message { get; set; }
    public int ID { get; set; }
    public int Coins { get; set; }
    public int HighScore { get; set; }
    public int DefenceLevel { get; set; }
    public int AttackLevel { get; set; }
    public int PlayerLevel { get; set; }

    public string JwToken { get; set; }
}