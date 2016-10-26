namespace MySite.DataContexts.DecksMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Namefix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Decks", "DateUpdated", c => c.DateTime(nullable: false));
            DropColumn("dbo.Decks", "DataUpdated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Decks", "DataUpdated", c => c.DateTime(nullable: false));
            DropColumn("dbo.Decks", "DateUpdated");
        }
    }
}
