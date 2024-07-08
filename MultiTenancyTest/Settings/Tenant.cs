namespace MultiTenancyTest.Settings
{
    public class Tenant
    {
        public string Name { get; set; } = null!;
        public string TId { get; set; } = null!;
        //لو احنا محددين ال ConnectionString دي فالبرنامج هيستخدمها ولو مش محددها هيستخدم ال default ConnectionString in configration class
        public string? ConnectionString { get; set; } // وهنا ال ConnectionString مخلييينها nullable => علشان ممكن متحددش لي ال tenant دي   مستخدمة مين بالظبط من ال databases  وبالتي انا هاستخدم ال default اللي موجودة عندي في ال Configration class

    }
}
