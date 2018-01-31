namespace Shsict.Peccacy.Service
{
    /// <summary>
    ///     Interface for defining an schedule.
    /// </summary>
    public interface ISchedule
    {
        void Execute(object state);
    }
}