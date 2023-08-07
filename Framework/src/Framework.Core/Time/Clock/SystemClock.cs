namespace Framework.Core.Time.Clock;

public class SystemClock : IClock
{
    public DateTime Now() => DateTime.Now;
}