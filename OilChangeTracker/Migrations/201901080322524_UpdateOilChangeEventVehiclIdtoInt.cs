namespace OilChangeTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOilChangeVehiclIdtoInt : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OilChanges", "Vehicle_Id", "dbo.Vehicles");
            DropIndex("dbo.OilChanges", new[] { "Vehicle_Id" });
            DropColumn("dbo.OilChanges", "VehicleId");
            RenameColumn(table: "dbo.OilChanges", name: "Vehicle_Id", newName: "VehicleId");
            AlterColumn("dbo.OilChanges", "VehicleId", c => c.Int(nullable: false));
            AlterColumn("dbo.OilChanges", "VehicleId", c => c.Int(nullable: false));
            CreateIndex("dbo.OilChanges", "VehicleId");
            AddForeignKey("dbo.OilChanges", "VehicleId", "dbo.Vehicles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OilChanges", "VehicleId", "dbo.Vehicles");
            DropIndex("dbo.OilChanges", new[] { "VehicleId" });
            AlterColumn("dbo.OilChanges", "VehicleId", c => c.Int());
            AlterColumn("dbo.OilChanges", "VehicleId", c => c.Byte(nullable: false));
            RenameColumn(table: "dbo.OilChanges", name: "VehicleId", newName: "Vehicle_Id");
            AddColumn("dbo.OilChanges", "VehicleId", c => c.Byte(nullable: false));
            CreateIndex("dbo.OilChanges", "Vehicle_Id");
            AddForeignKey("dbo.OilChanges", "Vehicle_Id", "dbo.Vehicles", "Id");
        }
    }
}
