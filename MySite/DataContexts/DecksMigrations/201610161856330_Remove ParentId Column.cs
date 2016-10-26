namespace MySite.DataContexts.DecksMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveParentIdColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Flashcards", "ParentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Flashcards", "ParentId", c => c.Int(nullable: false));
        }
    }
}
