using ContentLib.API.Model.Entity;

namespace System.Runtime.CompilerServices.Model.Managers;

public interface IEntityManager
{
    void RegisterEntity(IGameEntity entityToRegister);
    void UnRegisterEntity(ulong id);
    void UnRegisterAllEntities();
    IGameEntity GetEntity(ulong id);
}