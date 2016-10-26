namespace MySite.DataContexts.DecksMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdnotrequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Decks", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Decks", "UserId", c => c.String(nullable: false));
        }
    }
}
