namespace CodeBE_LEM.Entities
{
    public class Action
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Action() { }
        public Action(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
