namespace Battlefield
{
    public class IntensityProvider
    {
        private EnemyUnitSpawner _enemyUnitSpawner;

        public IntensityProvider(EnemyUnitSpawner enemyUnitSpawner)
        {
            _enemyUnitSpawner = enemyUnitSpawner;
        }

        public int GetIntensity()
        {
            return _enemyUnitSpawner.GetIntensity();
        }
    }
}