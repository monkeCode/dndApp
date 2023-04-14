
namespace Model
{
    public interface IFightable
    {
        string GetName();

        string GetOptionalText();

        int GetHp();

        int GetAc();

        int GetInitiative();

        int GetTimedHp();

        void SetHp(int hp);

        void ChangeHp(int ofset);

        void SetTimedHp(int hp);
    }
}
