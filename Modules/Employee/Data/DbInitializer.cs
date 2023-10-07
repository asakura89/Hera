namespace Hera.Modules.Employee.Data;

public static class DbInitializer {
    public static void Initialize(EmployeeDbContext context) {
        context.Database.EnsureCreated();

        // Check if the Persons table already has data
        if (context.FamilyMembers.Any()) {
            return; // No need to seed data
        }

        var familyMembers = new FamilyMember[] {
            new FamilyMember { FullName = "John Smith", Relationship = Relationship.Father, Birthdate = new DateTime(1970, 1, 1), IDType = IDType.KTP, IDNo = "1234567890", MaritalStatus = MaritalStatus.Married, Gender = Gender.Male, Religion = Religion.Christian, Job = "Engineer", Address = "123 Main Street", AddToEmergencyContact = true },
            new FamilyMember { FullName = "Jane Smith", Relationship = Relationship.Mother, Birthdate = new DateTime(1972, 2, 2), IDType = IDType.SIM, IDNo = "0987654321", MaritalStatus = MaritalStatus.Married, Gender = Gender.Female, Religion = Religion.Catholic, Job = "Teacher", Address = "123 Main Street", AddToEmergencyContact = true },
            new FamilyMember { FullName = "Jack Smith", Relationship = Relationship.Child, Birthdate = new DateTime(2000, 3, 3), IDType = IDType.KTP, IDNo = "2345678901", MaritalStatus = MaritalStatus.Single, Gender = Gender.Male, Religion = Religion.Christian, Job = "Student", Address = "123 Main Street", AddToEmergencyContact = false },
            new FamilyMember { FullName = "Jill Smith", Relationship = Relationship.Child, Birthdate = new DateTime(2002, 4, 4), IDType = IDType.SIM, IDNo = "8765432109", MaritalStatus = MaritalStatus.Single, Gender = Gender.Female, Religion = Religion.Catholic, Job = "Student", Address = "123 Main Street", AddToEmergencyContact = false }
        };

        foreach (FamilyMember member in familyMembers) {
            context.FamilyMembers.Add(member);
        }

        context.SaveChanges();
    }
}
