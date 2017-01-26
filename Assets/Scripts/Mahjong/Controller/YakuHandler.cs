using System.Collections;

namespace YakuHelper
{
    public class YakuHandler 
    {
        public YakuHandler Clone()
        {
            YakuHandler clone = new YakuHandler();
            clone.YakuID = this.YakuID;
            clone.hantei = this.hantei;
            clone.yakuman = this.yakuman;
            clone.doubleYakuman = this.doubleYakuman;
            clone.hanSuu = this.hanSuu;

            return clone;
        }

        protected int YakuID;

        // 役の成立判定フラグ 
        protected bool hantei = false;

        // 役満の判定フラグ 
        protected bool yakuman = false;

        // ダブル役満の判定フラグ 
        protected bool doubleYakuman = false;

        // 役の翻数 
        protected int hanSuu = 0;


        public bool isHantei() {
            return hantei;
        }
        public void setYakuHantei(bool hantei) {
            this.hantei = hantei;
        }

        public int getHanSuu() {
            return hanSuu;
        }
        public void setHanSuu(int han) {
            this.hanSuu = han;
        }


        public int getYakuID() {
            return this.YakuID;
        }

        public bool isYakuman() {
            return yakuman;
        }

        public bool isDoubleYakuman(){
            return doubleYakuman;
        }

        public string getYakuNameKey(){
            return YakuHandler.GetYakuNameKey(this.YakuID);
        }

        #region Yaku Name
        public struct YakuNameKey 
        {
            public int ID;
            public string Key;

            public YakuNameKey(int id, string key) {
                this.ID = id;
                this.Key = key;
            }
        }

        static YakuNameKey[] Yaku_IdNameKeys = new YakuNameKey[] 
        {
            new YakuNameKey( 1, "yaku_reach"),
            new YakuNameKey( 2, "yaku_doublereach"),
            new YakuNameKey( 3, "yaku_ippatsu"),
            new YakuNameKey( 4, "yaku_tsumo"),

            new YakuNameKey( 5, "yaku_haitei"),
            new YakuNameKey( 6, "yaku_houtei"),
            new YakuNameKey( 7, "yaku_rinshan"),
            new YakuNameKey( 8, "yaku_chankan"),
            new YakuNameKey( 9, "yaku_tanyao"),
            new YakuNameKey(10, "yaku_pinfu"),
            new YakuNameKey(11, "yaku_ipeikou"),

            new YakuNameKey(12, "yaku_chiitoitsu"),
            new YakuNameKey(13, "yaku_chanta"),
            new YakuNameKey(14, "yaku_ikkituukan"),
            new YakuNameKey(15, "yaku_sansyokudoujyun"),
            new YakuNameKey(16, "yaku_sansyokudoukou"),
            new YakuNameKey(17, "yaku_toitoi"),
            new YakuNameKey(18, "yaku_sanankou"),
            new YakuNameKey(19, "yaku_sankantsu"),
            new YakuNameKey(20, "yaku_honroutou"),
            new YakuNameKey(21, "yaku_shousangen"),

            new YakuNameKey(22, "yaku_ryanpeikou"),
            new YakuNameKey(23, "yaku_honitsu"),
            new YakuNameKey(24, "yaku_jyunchan"),

            new YakuNameKey(25, "yaku_chinitsu"),

            new YakuNameKey(26, "yaku_tenhou"),
            new YakuNameKey(27, "yaku_chihou"),
            new YakuNameKey(28, "yaku_renhou"),
            new YakuNameKey(29, "yaku_suuankou"),

            new YakuNameKey(30, "yaku_chinroutou"),
            new YakuNameKey(31, "yaku_ryuiisou"),
            new YakuNameKey(32, "yaku_suukantsu"),
            new YakuNameKey(33, "yaku_daisangen"),
            new YakuNameKey(34, "yaku_shousuushii"),
            new YakuNameKey(35, "yaku_tsuiisou"),

            new YakuNameKey(36, "yaku_chuurenpoutou"),
            new YakuNameKey(37, "yaku_kokushimusou"),

            new YakuNameKey(38, "yaku_daisuushii"),
            new YakuNameKey(39, "yaku_suuankou_tanki"),
            new YakuNameKey(40, "yaku_chuurenpoutou_jyunsei"),
            new YakuNameKey(41, "yaku_kokushimusou_13men"),

            new YakuNameKey(42, "yaku_lenfonhai"),
            new YakuNameKey(43, "yaku_yakuhai"),
            new YakuNameKey(44, "yaku_dora"),

            new YakuNameKey(45, "yaku_nagashimangan"), // handler is in AgariScoreManager.
        };


        public static string GetYakuNameKey(int yakuID)
        {
            return Yaku_IdNameKeys[ yakuID-1 ].Key;
        }
        #endregion
    }


    //立直.
    public class CheckReach : YakuHandler {
        public CheckReach(Yaku owner) {
            this.YakuID = 1;
            hantei = owner.checkReach() && !owner.checkDoubleReach();
            hanSuu = 1;
        }
    }
    //双立直.
    public class CheckDoubleReach : YakuHandler {
        public CheckDoubleReach(Yaku owner) {
            this.YakuID = 2;
            hantei = owner.checkDoubleReach();
            hanSuu = 2;
        }
    }
    //一发.
    public class CheckIppatsu : YakuHandler {
        public CheckIppatsu(Yaku owner) {
            this.YakuID = 3;
            hantei = owner.checkIppatu();
            hanSuu = 1;
        }
    }
    //自摸.
    public class CheckTsumo : YakuHandler {
        public CheckTsumo(Yaku owner) {
            this.YakuID = 4;
            hantei = owner.checkTsumo();
            hanSuu = 1;
        }
    }
    //海底捞月.
    public class CheckHaitei : YakuHandler {
        public CheckHaitei(Yaku owner) {
            this.YakuID = 5;
            hantei = owner.checkHaitei();
            hanSuu = 1;
        }
    }
    //河底捞鱼.
    public class CheckHoutei : YakuHandler {
        public CheckHoutei(Yaku owner) {
            this.YakuID = 6;
            hantei = owner.checkHoutei();
            hanSuu = 1;
        }
    }
    //杠上开花.
    public class CheckRinshan : YakuHandler {
        public CheckRinshan(Yaku owner) {
            this.YakuID = 7;
            hantei = owner.checkRinsyan();
            hanSuu = 1;
        }
    }
    //抢扛.
    public class CheckChankan : YakuHandler {
        public CheckChankan(Yaku owner) {
            this.YakuID = 8;
            hantei = owner.checkChankan();
            hanSuu = 1;
        }
    }
    //断幺.
    public class CheckTanyao : YakuHandler {
        public CheckTanyao(Yaku owner) {
            this.YakuID = 9;
            hantei = owner.checkTanyao();
            hanSuu = 1;
        }
    }
    //平和.
    public class CheckPinfu : YakuHandler {
        public CheckPinfu(Yaku owner) {
            this.YakuID = 10;
            hantei = owner.checkPinfu();
            hanSuu = 1;
        }
    }
    //一杯口.
    public class CheckIpeikou : YakuHandler {
        public CheckIpeikou(Yaku owner) {
            this.YakuID = 11;
            hantei = owner.checkIpeikou() && !owner.checkRyanpeikou();
            hanSuu = 1;
        }
    }

    //七对子.
    public class CheckChiitoitsu : YakuHandler {
        public CheckChiitoitsu(Yaku owner) {
            this.YakuID = 12;
            hantei = owner.checkChiitoitsu();
            hanSuu = 2;
        }
    }
    //混全带幺九.
    public class CheckChanta : YakuHandler {
        public CheckChanta(Yaku owner) {
            this.YakuID = 13;
            hantei = owner.checkCyanta() && !owner.checkJyunCyan() && !owner.checkHonroutou();

            if( owner.isNaki == true ) {
                hanSuu = 1;
            }
            else {
                hanSuu = 2;
            }
        }
    }
    //一气通贯.
    public class CheckIkkituukan : YakuHandler {
        public CheckIkkituukan(Yaku owner) {
            this.YakuID = 14;
            hantei = owner.checkIkkituukan();

            if( owner.isNaki == true ) {
                hanSuu = 1;
            }
            else {
                hanSuu = 2;
            }
        }
    }
    //三色同顺.
    public class CheckSansyokuDouJun : YakuHandler {
        public CheckSansyokuDouJun(Yaku owner) {
            this.YakuID = 15;
            hantei = owner.checkSansyokuDoujun();

            if( owner.isNaki == true ) {
                hanSuu = 1;
            }
            else {
                hanSuu = 2;
            }
        }
    }
    //三色同刻.
    public class CheckSansyokuDouKou : YakuHandler {
        public CheckSansyokuDouKou(Yaku owner) {
            this.YakuID = 16;
            hantei = owner.checkSansyokuDoukou();
            hanSuu = 2;
        }
    }
    //对对和.
    public class CheckToitoi : YakuHandler {
        public CheckToitoi(Yaku owner) {
            this.YakuID = 17;
            hantei = owner.checkToitoi();
            hanSuu = 2;
        }
    }
    //三暗刻.
    public class CheckSanankou : YakuHandler {
        public CheckSanankou(Yaku owner) {
            this.YakuID = 18;
            hantei = owner.checkSanankou();
            hanSuu = 2;
        }
    }
    //三杠子.
    public class CheckSankantsu : YakuHandler {
        public CheckSankantsu(Yaku owner) {
            this.YakuID = 19;
            hantei = owner.checkSankantsu();
            hanSuu = 2;
        }
    }
    //混老头.
    public class CheckHonroutou : YakuHandler {
        public CheckHonroutou(Yaku owner) {
            this.YakuID = 20;
            hantei = owner.checkHonroutou();
            hanSuu = 2;
        }
    }
    //混老头七对子.
    public class CheckHonroutou_Chiitoitsu : YakuHandler {
        public CheckHonroutou_Chiitoitsu(Yaku owner) {
            this.YakuID = 20; // the same name to Honroutou.
            hantei = owner.checkHonroutou_Chiitoitsu();
            hanSuu = 2;
        }
    }
    //小三元.
    public class CheckShousangen : YakuHandler {
        public CheckShousangen(Yaku owner) {
            this.YakuID = 21;
            hantei = owner.checkSyousangen();
            hanSuu = 2;
        }
    }

    //二杯口.
    public class CheckRyanpeikou : YakuHandler {
        public CheckRyanpeikou(Yaku owner) {
            this.YakuID = 22;
            hantei = owner.checkRyanpeikou();
            hanSuu = 3;
        }
    }
    //混一色.
    public class CheckHonitsu : YakuHandler {
        public CheckHonitsu(Yaku owner) {
            this.YakuID = 23;
            hantei = owner.checkHonisou() && !owner.checkTinisou();

            if( owner.isNaki == true ) {
                hanSuu = 2;
            }
            else {
                hanSuu = 3;
            }
        }
    }
    //纯全带幺九.
    public class CheckJyunChan : YakuHandler {
        public CheckJyunChan(Yaku owner) {
            this.YakuID = 24;
            hantei = owner.checkJyunCyan();

            if( owner.isNaki == true ) {
                hanSuu = 2;
            }
            else {
                hanSuu = 3;
            }
        }
    }

    //清一色.
    public class CheckChinitsu : YakuHandler {
        public CheckChinitsu(Yaku owner) {
            this.YakuID = 25;
            hantei = owner.checkTinisou();

            if( owner.isNaki == true ) {
                hanSuu = 5;
            }
            else {
                hanSuu = 6;
            }
        }
    }

    //天和.
    public class CheckTenhou : YakuHandler {
        public CheckTenhou(Yaku owner) {
            this.YakuID = 26;
            hantei = owner.checkTenhou();
            hanSuu = 13;
            yakuman = true;
        }
    }
    //地和.
    public class CheckChihou : YakuHandler {
        public CheckChihou(Yaku owner) {
            this.YakuID = 27;
            hantei = owner.checkTihou();
            hanSuu = 13;
            yakuman = true;
        }
    }
    //人和.
    public class CheckRenhou : YakuHandler{
        public CheckRenhou(Yaku owner) {
            this.YakuID = 28;
            hantei = owner.checkRenhou();
            hanSuu = 13;
            yakuman = true;
        }
    }
    //四暗刻.
    public class CheckSuuankou : YakuHandler {
        public CheckSuuankou(Yaku owner) {
            this.YakuID = 29;
            hantei = owner.checkSuuankou() && !owner.checkSuuankou_Tanki();
            hanSuu = 13;
            yakuman = true;
        }
    }
    //清老头.
    public class CheckChinroutou : YakuHandler {
        public CheckChinroutou(Yaku owner) {
            this.YakuID = 30;
            hantei = owner.checkChinroutou();
            hanSuu = 13;
            yakuman = true;
        }
    }
    //绿一色.
    public class CheckRyuiisou : YakuHandler {
        public CheckRyuiisou(Yaku owner) {
            this.YakuID = 31;
            hantei = owner.checkRyuuisou();
            hanSuu = 13;
            yakuman = true;
        }
    }
    //四杠子.
    public class CheckSuukantsu : YakuHandler {
        public CheckSuukantsu(Yaku owner) {
            this.YakuID = 32;
            hantei = owner.checkSuukantsu();
            hanSuu = 13;
            yakuman = true;
        }
    }
    //大三元.
    public class CheckDaisangen : YakuHandler {
        public CheckDaisangen(Yaku owner) {
            this.YakuID = 33;
            hantei = owner.checkDaisangen();
            hanSuu = 13;
            yakuman = true;
        }
    }
    //小四喜.
    public class CheckShousuushii : YakuHandler {
        public CheckShousuushii(Yaku owner) {
            this.YakuID = 34;
            hantei = owner.checkSyousuushi();
            hanSuu = 13;
            yakuman = true;
        }
    }
    //字一色(对对和)
    public class CheckTsuiisou : YakuHandler {
        public CheckTsuiisou(Yaku owner) {
            this.YakuID = 35;
            hantei = owner.checkTsuisou();
            hanSuu = 13;
            yakuman = true;
        }
    }
    //字一色(七对子)
    public class CheckTsuiisou_Chiitoitsu : YakuHandler {
        public CheckTsuiisou_Chiitoitsu(Yaku owner) {
            this.YakuID = 35;
            hantei = owner.checkTsuisou_Chiitoitsu();
            hanSuu = 13;
            yakuman = true;
        }
    }

    //九连宝灯.
    public class CheckChuurenpoutou : YakuHandler {
        public CheckChuurenpoutou(Yaku owner) {
            this.YakuID = 36;
            hantei = owner.checkCyuurennpoutou() && !owner.checkCyuurennpoutou_Jyunsei();
            hanSuu = 13;
            yakuman = true;
        }
    }
    //国士无双.
    public class CheckKokushiMusou : YakuHandler {
        public CheckKokushiMusou(Yaku owner) {
            this.YakuID = 37;
            hantei = owner.checkKokushi() && !owner.checkKokushi_13Men();
            hanSuu = 13;
            yakuman = true;
        }
    }

    //大四喜.
    public class CheckDaisuushii : YakuHandler {
        public CheckDaisuushii(Yaku owner) {
            this.YakuID = 38;
            hantei = owner.checkDaisuushi();
            hanSuu = 13;
            yakuman = true;
            doubleYakuman = true;
        }
    }
    //四暗刻单骑.
    public class CheckSuuankou_Tanki : YakuHandler {
        public CheckSuuankou_Tanki(Yaku owner) {
            this.YakuID = 39;
            hantei = owner.checkSuuankou_Tanki();
            hanSuu = 13;
            yakuman = true;
            doubleYakuman = true;
        }
    }
    //纯正九连宝灯.
    public class CheckChuurenpoutou_Jyunsei : YakuHandler {
        public CheckChuurenpoutou_Jyunsei(Yaku owner) {
            this.YakuID = 40;
            hantei = owner.checkCyuurennpoutou_Jyunsei();
            hanSuu = 13;
            yakuman = true;
            doubleYakuman = true;
        }
    }
    //国士无双十三面.
    public class CheckKokushiMusou_13Men : YakuHandler {
        public CheckKokushiMusou_13Men(Yaku owner) {
            this.YakuID = 41;
            hantei = owner.checkKokushi_13Men();
            hanSuu = 13;
            yakuman = true;
            doubleYakuman = true;
        }
    }


    // 连风牌.
    public class CheckLenFonHai : YakuHandler {
        public CheckLenFonHai(Yaku owner) 
        {
            this.YakuID = 42;

            EKaze jikaze = owner.AgariParam.getJikaze();
            EKaze bakaze = owner.AgariParam.getBakaze();

            if( jikaze == EKaze.Ton && bakaze == EKaze.Ton ){
                if( owner.checkTon() ){
                    hantei = true;
                    hanSuu = 2;
                }
            }
            else if( jikaze == EKaze.Nan && bakaze == EKaze.Nan ){
                if( owner.checkNan() ){
                    hantei = true;
                    hanSuu = 2;
                }
            } 
        }
    }

    // 役牌.
    public class CheckYakuHai : YakuHandler {
        public CheckYakuHai(Yaku owner) 
        {
            this.YakuID = 43;

            if( owner.checkHaku() ){
                hanSuu++;
                hantei = true;
            }
            if( owner.checkHatsu() ){
                hanSuu++;
                hantei = true;
            }
            if( owner.checkCyun() ){
                hanSuu++;
                hantei = true;
            }


            EKaze jikaze = owner.AgariParam.getJikaze();
            CheckLenFonHai lenFonHaiChecker = new CheckLenFonHai(owner);

            EKaze bakaze = owner.AgariParam.getBakaze();

            // check BaKaze hai
            if( bakaze == EKaze.Ton && jikaze != EKaze.Ton ) // lenfon is checking in CheckLenFonHai()
            {
                if( owner.checkTon() ){
                    hanSuu++;
                    hantei = true;
                }
            }
            else if( bakaze == EKaze.Nan && jikaze != EKaze.Nan ) // lenfon is checking in CheckLenFonHai()
            {
                if( owner.checkNan() ){
                    hanSuu++;
                    hantei = true;
                }
            }

            // check JiKaze hai.
            if( jikaze == EKaze.Ton )
            {
                if( lenFonHaiChecker.isHantei() ){

                }
                else if( owner.checkTon() ){
                    hanSuu++;
                    hantei = true;
                }
            }
            else if( jikaze == EKaze.Nan )
            {
                if( lenFonHaiChecker.isHantei() ){

                }
                else if( owner.checkNan() ){
                    hanSuu++;
                    hantei = true;
                }
            }
            else if( jikaze == EKaze.Sya )
            {
                if( lenFonHaiChecker.isHantei() ){

                }
                else if( owner.checkSya() ){
                    hanSuu++;
                    hantei = true;
                }
            }
            else
            {
                if( lenFonHaiChecker.isHantei() ){

                }
                else if( owner.checkPei() ){
                    hanSuu++;
                    hantei = true;
                }
            }
        }
    }

    //宝牌.
    public class CheckDora : YakuHandler {
        public CheckDora(Yaku owner) {
            this.YakuID = 44;
            hantei = owner.checkDora();
            hanSuu = 1;
        }
    }

    // 流局满贯(不能在Yaku里面判断！).
    public class CheckNagashimangan : YakuHandler {
        public CheckNagashimangan(Yaku owner) {
            this.YakuID = 45;
            hantei = true;
            hanSuu = 5;
        }
    }
}