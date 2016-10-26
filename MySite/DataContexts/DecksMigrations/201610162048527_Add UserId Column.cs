namespace MySite.DataContexts.DecksMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserIdColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Decks", "UserId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Decks", "UserId");
        }
    }
}
