using Hera.Core.Data;

namespace Hera.Modules.Employee.Data.Command;

public class AddFamilyMemberCommand : ICommand<FamilyMember> {
    private readonly EmployeeDbContext dbCtx;

    public AddFamilyMemberCommand(EmployeeDbContext dbCtx) {
        this.dbCtx = dbCtx;
    }

    public void Execute(FamilyMember member) {
        dbCtx.FamilyMembers.Add(member);
        dbCtx.SaveChanges();
    }
}
