
public interface IObserver : IEventListener<UIEventType, object[]>
{
    
}

public interface IEventListener<TEventType, TArgument>
{
    void OnHandleEvent(TEventType eventType, TArgument args);
}
