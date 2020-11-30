using UnityEngine;
using TMPro;


public class TeamMemberCell : MonoBehaviour
{
    public TextMeshProUGUI memberText;


    TeamMembersItem membersItem;


    public void SetView(TeamMembersItem membersItem)
    {
        this.membersItem = membersItem;

        memberText.text = membersItem.users.name;
    }
}
