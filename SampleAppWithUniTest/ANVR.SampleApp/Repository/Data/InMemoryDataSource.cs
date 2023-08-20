namespace ANVR.SampleApp.Repository.Data;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class InMemoryDataSource<IEntity> : Dictionary<int, IEntity>
{
    private Dictionary<int, IEntity> entities = new Dictionary<int, IEntity>();

    #region Public members

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entity"></param> <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entity"></param>
    public void AddEntity(int id, IEntity entity)
    {
        entities[id] = entity;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IEntity GetEntity(int id)
    {
        if (entities.TryGetValue(id, out var entity))
        {
            return entity;
        }
        return default(IEntity);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool DeleEntiry(int id)
    {
        entities.Remove(id);
        return true;
    }
    #endregion
}

