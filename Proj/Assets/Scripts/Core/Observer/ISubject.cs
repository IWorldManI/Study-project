
using Entity.NPC;

public interface ISubject
{
    void Attach(Customer observer);
    
    void Detach(Customer observer);

    void Notify();
}
