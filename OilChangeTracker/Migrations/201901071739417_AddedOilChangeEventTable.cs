namespace OilChangeTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOilChangeEventTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OilChangeEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VehicleId = c.Byte(nullable: false),
                        WorkDate = c.DateTime(nullable: false),
                        Mileage = c.Int(nullable: false),
                        OilFilterBrand = c.String(maxLength: 50),
                        OilFilterModel = c.String(maxLength: 50),
                        OilBrand = c.String(maxLength: 50),
                        OilViscosity = c.String(maxLength: 50),
                        OtherNotes = c.String(maxLength: 500),
                        Vehicle_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.Vehicle_Id)
                .Index(t => t.Vehicle_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OilChangeEvents", "Vehicle_Id", "dbo.Vehicles");
            DropIndex("dbo.OilChangeEvents", new[] { "Vehicle_Id" });
            DropTable("dbo.OilChangeEvents");
        }
    }
}