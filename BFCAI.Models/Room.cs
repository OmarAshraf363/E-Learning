namespace Banha_UniverCity.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public string Name { get; set; }=string.Empty;
        public int Capacity {  get; set; }
        public ICollection<ClassSchedule> Schedules { get; set; } = new List<ClassSchedule>();
    }
}
