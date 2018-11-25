namespace Funda.ProgrammingAssignment.ServiceProxy.Services.RequestStatusUpdater
{
    public interface IRequestStatusUpdater
    {
        void Setup(string text);
        void Initialize(int totalBatches, string text = null);
        void Tick();
    }
}