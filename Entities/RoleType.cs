namespace CodeBE_LEM.Entities
{
    public class RoleType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public RoleType() { }
        public RoleType(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
