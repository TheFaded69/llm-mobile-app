using BaseDomain.Models;
using Main.Domain.Dialogs.Models;
using Main.Domain.Dictionary.Models;
using Main.Domain.Tests.Models;
using Main.Domain.Tutors.Models;
using Main.Domain.Users.Enums;

namespace Main.Domain.Users.Models;

public class User : GuidEntity
{
    public UserRole Role { get; set; }
    
    public UserType UserType { get; set; }
    
    public string Email { get; set; }
    
    public string UserName { get; set; }
    
    public List<Set>  CreatedSets { get; set; }
    
    public List<Set> FavoriteSets { get; set; }
    
    public List<Session> Sessions { get; set; }
}