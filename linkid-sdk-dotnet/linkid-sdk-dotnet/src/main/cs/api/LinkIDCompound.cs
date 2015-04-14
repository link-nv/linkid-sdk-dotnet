using System;
using System.Collections.Generic;
using System.Text;

namespace safe_online_sdk_dotnet
{
    public class LinkIDCompound
    {
        public Dictionary<String, LinkIDAttribute> membersMap { get; set; }
        public List<LinkIDAttribute> members { get; set; }

        public LinkIDCompound(List<LinkIDAttribute> members)
        {
            this.members = members;
            membersMap = new Dictionary<string, LinkIDAttribute>();
            foreach (LinkIDAttribute member in members)
            {
                membersMap.Add(member.getAttributeName(), member);
            }
        }

    }
}
