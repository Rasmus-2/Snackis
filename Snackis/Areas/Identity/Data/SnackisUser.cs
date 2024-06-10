using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Snackis.Areas.Identity.Data;

// Add profile data for application users by adding properties to the SnackisUser class
public class SnackisUser : IdentityUser
{
    [PersonalData]
    public string FirstName { get; set; }
    
    [PersonalData]
    public string LastName { get; set; }
    
    [PersonalData]
    public DateTime BirthDay { get; set; }

    public string? ProfilePicture {  get; set; }
    public List<int> LikedPostsIds { get; set; }
}

