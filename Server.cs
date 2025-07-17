namespace KleverensSoft_Test
{
    internal static class Server
    {
        private static ReaderWriterLockSlim _rwl = new ReaderWriterLockSlim();
        private static int _count = 0;

        public static int GetCount()
        {
            Console.WriteLine("Вход в блокировку чтения.");
            _rwl.EnterReadLock();
            try
            {
                return _count;
            }
            finally
            {
                _rwl.ExitReadLock();
                Console.WriteLine("Выход из блокировки чтения.");
            }
        }

        public static void AddToCount(int value)
        {
            Console.WriteLine("Вход в блокировку записи.");
            _rwl.EnterWriteLock();
            try
            {
                _count += value;
            }
            finally
            {
                _rwl.ExitWriteLock();
                Console.WriteLine("Выход из блокировки записи.");
            }
        }
    }
}
