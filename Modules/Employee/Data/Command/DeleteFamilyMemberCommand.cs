using Hera.Core.Data;

namespace Hera.Modules.Employee.Data.Command;

public class DeleteFamilyMemberCommand : ICommand<FamilyMember> {
    private readonly EmployeeDbContext dbCtx;

    public DeleteFamilyMemberCommand(EmployeeDbContext dbCtx) {
        this.dbCtx = dbCtx;
    }

    public void Execute(FamilyMember member) {
        dbCtx.FamilyMembers.Remove(member);
        dbCtx.SaveChanges();
    }
}
