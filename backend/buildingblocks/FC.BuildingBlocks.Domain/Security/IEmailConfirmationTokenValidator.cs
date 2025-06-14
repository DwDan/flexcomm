namespace FC.BuildingBlocks.Domain.Security
{
    public interface IEmailConfirmationTokenValidator
    {
        Guid Validate(string token);
    }
}
