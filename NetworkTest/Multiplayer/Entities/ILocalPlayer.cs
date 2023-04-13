using System.Threading.Tasks;

public interface ILocalPlayer
{
    public Task Attack(bool isHit, int damage, string hittedPlayerName);
}