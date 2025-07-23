namespace ClipShare.Core.IRepo
{
    public interface IUnitOfWork : IDisposable
    {
        IChannelRepo ChannelRepo { get; }
        ICategoryRepo CategoryRepo { get; }
        IVideoRepo VideoRepo { get; }
        Task<bool> CompleteAsync();
    }
}
