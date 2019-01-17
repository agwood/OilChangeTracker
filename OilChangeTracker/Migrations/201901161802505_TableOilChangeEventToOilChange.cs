namespace OilChangeTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableOilChangeEventToOilChange : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.OilChangeEvents", newName: "OilChanges");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.OilChanges", newName: "OilChangeEvents");
        }
    }
}
