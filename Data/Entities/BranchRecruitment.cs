namespace Data.Entities
{
    public class BranchRecruitment
    {
        public int BranchId { get; set; }
        public int RecruimentId { get; set; }
        public Branch Branch { get; set; }
        public Recruitment Recruitment { get; set; }
    }
}
