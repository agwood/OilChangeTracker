namespace OilChangeTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOilChangeEventVehiclIdtoInt : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OilChangeEvents", "Vehicle_Id", "dbo.Vehicles");
            DropIndex("dbo.OilChangeEvents", new[] { "Vehicle_Id" });
            DropColumn("dbo.OilChangeEvents", "VehicleId");
            RenameColumn(table: "dbo.OilChangeEvents", name: "Vehicle_Id", newName: "VehicleId");
            AlterColumn("dbo.OilChangeEvents", "VehicleId", c => c.Int(nullable: false));
            AlterColumn("dbo.OilChangeEvents", "VehicleId", c => c.Int(nullable: false));
            CreateIndex("dbo.OilChangeEvents", "VehicleId");
            AddForeignKey("dbo.OilChangeEvents", "VehicleId", "dbo.Vehicles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OilChangeEvents", "VehicleId", "dbo.Vehicles");
            DropIndex("dbo.OilChangeEvents", new[] { "VehicleId" });
            AlterColumn("dbo.OilChangeEvents", "VehicleId", c => c.Int());
            AlterColumn("dbo.OilChangeEvents", "VehicleId", c => c.Byte(nullable: false));
            RenameColumn(table: "dbo.OilChangeEvents", name: "VehicleId", newName: "Vehicle_Id");
            AddColumn("dbo.OilChangeEvents", "VehicleId", c => c.Byte(nullable: false));
            CreateIndex("dbo.OilChangeEvents", "Vehicle_Id");
            AddForeignKey("dbo.OilChangeEvents", "Vehicle_Id", "dbo.Vehicles", "Id");
        }
    }
}
