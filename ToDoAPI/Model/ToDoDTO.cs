namespace ToDoAPI.Model
{
    public class ToDoDTO
    {
        public int Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public bool IsChecked { get; set; } 

        public DateTime CreateDate { get; set; }
    }
}
