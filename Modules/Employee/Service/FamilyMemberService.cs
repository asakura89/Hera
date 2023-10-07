using Hera.Modules.Employee.Data;
using Hera.Modules.Employee.Data.Command;
using Hera.Modules.Employee.Data.Query;

namespace Hera.Modules.Employee.Service;

public class FamilyMemberService {
    private readonly EmployeeDbContext dbCtx;

    public FamilyMemberService(EmployeeDbContext dbCtx) {
        this.dbCtx = dbCtx;
    }

    public void AddFamilyMember(FamilyMember member) {
        var command = new AddFamilyMemberCommand(dbCtx);
        command.Execute(member);
    }

    public IEnumerable<FamilyMember> GetAllFamilyMembers() {
        var query = new GetAllFamilyMembersQuery(dbCtx);
        return query.Execute();
    }

    public void DeleteFamilyMember(FamilyMember member) {
        var command = new DeleteFamilyMemberCommand(dbCtx);
        command.Execute(member);
    }
}
