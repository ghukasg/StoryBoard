namespace StoryBoard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.GroupApplicationUsers", newName: "ApplicationUserGroups");
            DropPrimaryKey("dbo.ApplicationUserGroups");
            AddPrimaryKey("dbo.ApplicationUserGroups", new[] { "ApplicationUser_Id", "Group_GroupId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ApplicationUserGroups");
            AddPrimaryKey("dbo.ApplicationUserGroups", new[] { "Group_GroupId", "ApplicationUser_Id" });
            RenameTable(name: "dbo.ApplicationUserGroups", newName: "GroupApplicationUsers");
        }
    }
}
