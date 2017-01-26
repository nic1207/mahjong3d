
/// <summary>
/// Score, has the same function to class ScoreInfo, but is more clear.
/// </summary>
public struct Score 
{
    //parent.
    public int oyaAgari;
    public int oyaTsumoKoPay;

    //child
    public int koAgari;
    public int koTsumoKoPay;
    public int koTsumoOyaPay;


    public Score( int oyaAgari, int oyaTsumoKoPay, int koAgari, int koTsumoKoPay, int koTsumoOyaPay )
    {
        this.oyaAgari = oyaAgari;
        this.oyaTsumoKoPay = oyaTsumoKoPay;

        this.koAgari = koAgari;
        this.koTsumoKoPay = koTsumoKoPay;
        this.koTsumoOyaPay = koTsumoOyaPay;
    }

    public Score( Score other )
    {
        this.oyaAgari = other.oyaAgari;
        this.oyaTsumoKoPay = other.oyaTsumoKoPay;

        this.koAgari = other.koAgari;
        this.koTsumoKoPay = other.koTsumoKoPay;
        this.koTsumoOyaPay = other.koTsumoOyaPay;
    }

    public override string ToString()
    {
        return string.Format("[Score]: oyaAgari = {0}, oyaTsumoKoPay = {1},\n koAgari = {2}, koTsumoKoPay = {3}, koTsumoOyaPay = {4}",
                             oyaAgari, oyaTsumoKoPay, koAgari, koTsumoKoPay, koTsumoOyaPay );
    }
}
