namespace Units.Resources
{
    public class UnitResourceInterface
    {

        private UnitResourceManager _unitResourceManager;

        public UnitResourceInterface(UnitResourceManager unitResourceManager)
        {
            _unitResourceManager = unitResourceManager;
        }

        public void AddResource(ResourceId resourceId, IUnitResource unitResource)
        {
            _unitResourceManager.AddResource(resourceId, unitResource);    
        }
        
        public IUnitResource GetUnitResource(ResourceId resourceId)
        {
            return _unitResourceManager.GetUnitResource(resourceId);
        }
        
    }
}