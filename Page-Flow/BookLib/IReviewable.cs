namespace BookLib
{
    public interface IReviewable
    {
        string Path { get; }
        int Likes { get; set; }
        int DisLikes { get; set; }
        int FakeLikes { get; set; }
        List<string> Comments { get; set; }
    }
}