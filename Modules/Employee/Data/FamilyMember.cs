using System.ComponentModel.DataAnnotations;

namespace Hera.Modules.Employee.Data;

public class FamilyMember {
    [Key]
    public Guid MemberId { get; set; }
    public String FullName { get; set; }
    public Relationship Relationship { get; set; }
    public DateTime Birthdate { get; set; }
    public IDType IDType { get; set; }
    public String IDNo { get; set; }
    public MaritalStatus MaritalStatus { get; set; }
    public Gender Gender { get; set; }
    public Religion Religion { get; set; }
    public String Job { get; set; }
    public String Address { get; set; }
    public Boolean AddToEmergencyContact { get; set; }
}

public enum Relationship {
    Father,
    Mother,
    Sibling,
    Spouse,
    Child,
    Cousin,
    Nibling,
    ParentInLaw,
    BrotherInLaw,
    SisterInLaw,
    Uncle,
    Aunt,
    Friend,
    Coworker,
    Others
}

public enum IDType {
    KTP,
    SIM
}

public enum MaritalStatus {
    Single,
    Married,
    Widow,
    Widower
}

public enum Gender {
    Male,
    Female
}

public enum Religion {
    Catholic,
    Christian,
    Islam,
    Buddha,
    Hindu,
    Confucius,
    Orthodox,
    Others
}