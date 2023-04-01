using System.Threading.Tasks;

public interface ILocalPlayer
{
    public Task Attack(bool isHit, float damage, string hittedPlayerName);
}