namespace AuctionService.Entities
{
    /// <summary>
    /// Represents the lifecycle status of an auction.
    /// </summary>
    public enum Status
    {
        Live,
        Finished,
        ReserveNotMet
    }
}