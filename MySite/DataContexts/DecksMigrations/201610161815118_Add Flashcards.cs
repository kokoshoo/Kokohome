namespace MySite.DataContexts.DecksMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFlashcards : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Flashcards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CardFront = c.String(nullable: false),
                        CardBack = c.String(),
                        ParentId = c.Int(nullable: false),
                        DeckId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Decks", t => t.DeckId, cascadeDelete: true)
                .Index(t => t.DeckId);
            
            AlterColumn("dbo.Decks", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Flashcards", "DeckId", "dbo.Decks");
            DropIndex("dbo.Flashcards", new[] { "DeckId" });
            AlterColumn("dbo.Decks", "Title", c => c.String());
            DropTable("dbo.Flashcards");
        }
    }
}
