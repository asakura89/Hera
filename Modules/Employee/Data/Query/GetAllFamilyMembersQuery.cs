using Hera.Core.Data;

namespace Hera.Modules.Employee.Data.Query;

public class GetAllFamilyMembersQuery : IQuery<FamilyMember> {
    private readonly EmployeeDbContext dbCtx;

    public GetAllFamilyMembersQuery(EmployeeDbContext dbCtx) {
        this.dbCtx = dbCtx;
    }

    public IEnumerable<FamilyMember> Execute() =>
        dbCtx.FamilyMembers.ToList();
}
