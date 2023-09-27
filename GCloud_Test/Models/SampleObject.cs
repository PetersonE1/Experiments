namespace GCloud_Test.Models
{
    public class SampleObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return 
                $"ID: {Id}," +
                $"\nName: {Name}," +
                $"\nDescription: {Description}";
        }
    }
}
