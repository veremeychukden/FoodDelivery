using System.Collections.Generic;

namespace DTO.Responses
{
    public class RoleResponse
    {
        public string Role { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}