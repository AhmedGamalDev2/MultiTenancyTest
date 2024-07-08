namespace MultiTenancyTest.Settings
{
    public class Configration
    {
        public string DBProvider { get; set; } = null!;//like => sqlie, mssqlserver
        // دي ال default connectionString اللي موجودة عندي 
        // دي تعبتر shared connectionString
        public string ConnectionString { get; set; } = null!;//علشان لو مشت محدد connection string لل tenant دي يبقى هنستخدم الاساسية اللي احنا محددينها هنا في السطر دا 
    }
}
