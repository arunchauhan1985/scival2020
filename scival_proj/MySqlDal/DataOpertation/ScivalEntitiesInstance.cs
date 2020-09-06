namespace MySqlDal
{
    public static class ScivalEntitiesInstance
    {
        private static ScivalEntities ScivalEntities;

        public static ScivalEntities GetInstance()
        {
            if (ScivalEntities == null)
                ScivalEntities = new ScivalEntities();

            return ScivalEntities;
        }
    }
}
