namespace CodeBE_LEM.Entities
{
    public class AppUserType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AppUserType() { }
        public AppUserType(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
