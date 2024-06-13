namespace CarRentalPlatform.DTOs
{
  public class RentalCreateDto
  {
    public int Car_Id { get; set; }
    public int User_Id { get; set; }
    public DateTime Start_Date { get; set; }
    public DateTime End_Date { get; set; }
    public decimal Fee { get; set; }
    public Status Status { get; set; }
  }

  public enum Status
  {
    Confirm = 0,  // 确认
    Done = 1,     // 完成
    Cancel = 2,   // 取消
    Pending = 3   // 待处理
  }

}
